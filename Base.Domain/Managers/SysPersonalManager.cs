using AutoMapper;
using Base.Domain.Entities;
using Base.Domain.Events;
using Base.Domain.Aggregates;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core;
using OneForAll.Core.Events;
using OneForAll.Core.Extension;
using OneForAll.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OneForAll.Core.Utility;
using OneForAll.EFCore;
using OneForAll.Core.UploadFile;

namespace Base.Domain.Managers
{
    /// <summary>
    /// 领域服务：个人中心
    /// </summary>
    public class SysPersonalManager : BaseDomainManager, ISysPersonalManager
    {
        private readonly string CACHE_KEY_CODE = "UpdMobileCode:{0}";
        private readonly string UPLOAD_PATH = "/upload/tenants/{0}/headers/";// 文件存储路径
        private readonly string VIRTUAL_PATH = "/resources/tenants/{0}/headers/";// 虚拟路径：根据Startup配置设置,返回给前端访问资源

        private readonly IMapper _mapper;
        private readonly IUploader _uploader;
        private readonly ISysUserRepository _userRepository;
        private readonly IDistributedCache _cacheRepository;
        private readonly ISysTenantRepository _tenantRepository;
        private readonly ISysMidTenantUserRepository _tenantUserRepository;

        private readonly IEventDispatcher _eventDispatcher;
        public SysPersonalManager(
            IMapper mapper,
            IUploader uploader,
            ICurrentUser currentUser,
            ISysUserRepository userRepository,
            IDistributedCache cacheRepository,
            ISysTenantRepository tenantRepository,
            ISysMidTenantUserRepository tenantUserRepository,
            IEventDispatcher eventDispatcher) : base(currentUser)
        {
            _mapper = mapper;
            _uploader = uploader;
            _userRepository = userRepository;
            _cacheRepository = cacheRepository;
            _tenantRepository = tenantRepository;
            _tenantUserRepository = tenantUserRepository;

            _eventDispatcher = eventDispatcher;
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysPersonalForm form)
        {
            var data = await _userRepository.FindAsync(LoginUser.Id.TryGuid());
            if (data == null) 
                return BaseErrType.DataNotFound;

            _mapper.Map(form, data);
            return await ResultAsync(() => _userRepository.UpdateAsync(data));
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>上传结果</returns>
        public async Task<IUploadResult> UploadHeaderAsync(string filename, Stream file)
        {
            var maxSize = 500 * 1024;
            var extension = Path.GetExtension(filename);

            var result = new UploadResult();
            var data = await _userRepository.FindAsync(LoginUser.Id.TryGuid());
            if (data == null)
                return result;

            var newfileName = data.UserName + extension;
            var uploadPath = AppDomain.CurrentDomain.BaseDirectory + UPLOAD_PATH.Fmt(LoginUser.TenantId);
            var virtualPath = VIRTUAL_PATH.Fmt(LoginUser.TenantId);

            if (new ValidateImageType().Validate(NormalizeImageFileName(filename), file))
            {
                result = await _uploader.WriteAsync(file, uploadPath, newfileName, maxSize) as UploadResult;
                // 设置返回虚拟路径
                if (result.State == UploadEnum.Success)
                {
                    result.Url = Path.Combine(virtualPath, newfileName);
                    data.UploadHeader(result.Url);
                    await _userRepository.UpdateAsync(data);
                    return result;
                }
            }
            else
            {
                result.State = UploadEnum.TypeError;
            }
            return result;
        }

        /// <summary>
        /// 规范化图片文件扩展名（OneForAll.File 图片文件名校验仅识别 ".Jpg"/".Png" 等首字母大写形式，
        /// 统一转换后再校验，兼容 ".png"/".PNG" 等常见写法）
        /// </summary>
        /// <param name="filename">原始文件名</param>
        /// <returns>规范化后的文件名</returns>
        private string NormalizeImageFileName(string filename)
        {
            var extension = Path.GetExtension(filename);
            if (extension.IsNullOrEmpty())
                return filename;

            var ext = extension.TrimStart('.').ToLower();
            var name = Path.GetFileNameWithoutExtension(filename);
            return "{0}.{1}{2}".Fmt(name, ext.Substring(0, 1).ToUpper(), ext.Substring(1));
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ChangePasswordAsync(ResetPasswordVo password)
        {
            var data = await _userRepository.FindAsync(LoginUser.Id.TryGuid());
            if (data == null) 
                return BaseErrType.DataNotFound;

            var errType = data.ChangePassword(password);
            if (errType == BaseErrType.Success)
            {
                return await ResultAsync(() => _userRepository.UpdateAsync(data));
            }
            return errType;
        }

        /// <summary>
        /// 更换所属租户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<BaseErrType> UpdateTenantAsync(Guid tenantId)
        {
            var data = await _userRepository.GetByIdIQFAsync(LoginUser.Id.TryGuid());
            if (data == null)
                return BaseErrType.DataError;

            var tenant = await _tenantRepository.GetIQFAsync(tenantId);
            if (tenant == null)
                return BaseErrType.DataNotFound;

            if (data.SysTenantId == Guid.Empty || (data.SysTenantId != tenantId && tenantId != Guid.Empty))
            {
                data.AssignTenant(tenantId);
                var result = await ResultAsync(_userRepository.SaveChangesAsync);
                if (result == BaseErrType.Success)
                    await _eventDispatcher.PublishAsync(new UserTenantChangedEvent(LoginUser.Id.TryGuid()));
                return result;
            }
            return BaseErrType.Fail;
        }

        /// <summary>
        /// 发起修改手机号：校验是否可发送验证码并生成验证码。
        /// 短信发送与模板/签名配置属基础设施，交由 Application 编排，此处不发送。
        /// </summary>
        /// <param name="mobile">目标手机号</param>
        /// <returns>Err=NotAllow 表示仍在有效期内须稍后再试；Err=Success 时 Code 为待发送验证码</returns>
        public async Task<(BaseErrType Err, string Code)> BeginSendMobileUpdateCodeAsync(string mobile)
        {
            string cacheKey = CACHE_KEY_CODE.Fmt(LoginUser.Id);
            var cache = await _cacheRepository.GetStringAsync(cacheKey);
            if (cache != null)
                return (BaseErrType.NotAllow, null);

            var code = StringHelper.GetRandomNumber(4);
            return (BaseErrType.Success, code);
        }

        /// <summary>
        /// 确认验证码短信已发出，记录验证码（60 秒有效期）。仅发送成功后调用，失败时不记录，用户可立即重试。
        /// </summary>
        /// <param name="mobile">目标手机号</param>
        /// <param name="code">验证码</param>
        public async Task ConfirmMobileUpdateCodeSentAsync(string mobile, string code)
        {
            string cacheKey = CACHE_KEY_CODE.Fmt(LoginUser.Id);
            var cacheVal = "{0}|{1}".Fmt(code, mobile);
            await _cacheRepository.SetStringAsync(cacheKey, cacheVal, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60)
            });
        }

        /// <summary>
        /// 提交修改手机号：校验验证码、手机号一致性与是否被占用（校验通过后清除验证码缓存）。
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> CommitMobileUpdateCodeAsync(SysPersonalUpdateMobileForm form)
        {
            string cacheKey = CACHE_KEY_CODE.Fmt(LoginUser.Id);
            var cache = await _cacheRepository.GetStringAsync(cacheKey);
            if (cache == null)
                return BaseErrType.AuthCodeInvalid;

            // 1.校验验证码
            var code = cache.Split('|')[0];
            if (code != form.Code)
                return BaseErrType.AuthCodeInvalid;

            // 2.删除验证码缓存
            await _cacheRepository.RemoveAsync(cacheKey);

            // 3.校验手机号
            var mobile = cache.Split('|')[1];
            if (mobile != form.Mobile)
                return BaseErrType.DataNotMatch;

            // 4.校验手机号是否已存在
            var exists = await _userRepository.GetByMobileIQFAsync(form.Mobile);
            if (exists != null)
                return BaseErrType.DataExist;

            return BaseErrType.Success;
        }

        /// <summary>
        /// 变更手机号（领域写操作：聚合行为 + 持久化 + 发布领域事件）。
        /// </summary>
        /// <param name="mobile">新手机号</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ChangeMobileAsync(string mobile)
        {
            var user = await _userRepository.GetByIdIQFAsync(LoginUser.Id.TryGuid());
            if (user == null)
                return BaseErrType.DataNotFound;

            user.ChangeMobile(mobile);
            var result = await ResultAsync(() => _userRepository.SaveChangesAsync());
            if (result == BaseErrType.Success)
                await _eventDispatcher.PublishAsync(new UserMobileChangedEvent(LoginUser.Id.TryGuid()));
            return result;
        }

    }
}
