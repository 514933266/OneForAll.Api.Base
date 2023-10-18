using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Enums;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using OneForAll.File;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OneForAll.Core.Utility;
using System.Security.Cryptography;
using OneForAll.Core.Security;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;

namespace Base.Domain
{
    /// <summary>
    /// 领域服务：个人中心
    /// </summary>
    public class SysPersonalManager : SysBaseManager, ISysPersonalManager
    {
        private readonly string CACHE_KEY = "LoginInfo:{0}";
        private readonly string UPLOAD_PATH = "/upload/tenants/{0}/headers/";// 文件存储路径
        private readonly string VIRTUAL_PATH = "/resources/tenants/{0}/headers/";// 虚拟路径：根据Startup配置设置,返回给前端访问资源

        private readonly IUploader _uploader;
        private readonly ISysUserRepository _userRepository;
        private readonly ISysMenuRepository _menuRepository;
        private readonly IDistributedCache _cacheRepository;
        private readonly ISysPermissionRepository _permRepository;
        private readonly ISysRoleUserContactRepository _roleUserRepository;
        private readonly ISysUserPermContactRepository _userPermRepository;

        private readonly ISysGlobalExceptionLogHttpService _exceptionHttpService;
        public SysPersonalManager(
            IMapper mapper,
            IUploader uploader,
            IHttpContextAccessor httpContextAccessor,
            ISysUserRepository userRepository,
            ISysMenuRepository menuRepository,
            IDistributedCache cacheRepository,
            ISysPermissionRepository permRepository,
            ISysRoleUserContactRepository roleUserRepository,
            ISysUserPermContactRepository userPermRepository,
            ISysGlobalExceptionLogHttpService exceptionHttpService) : base(mapper, httpContextAccessor)
        {
            _uploader = uploader;
            _userRepository = userRepository;
            _menuRepository = menuRepository;
            _cacheRepository = cacheRepository;
            _permRepository = permRepository;
            _roleUserRepository = roleUserRepository;
            _userPermRepository = userPermRepository;

            _exceptionHttpService = exceptionHttpService;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns>实体</returns>
        public async Task<SysLoginUserAggr> GetAsync()
        {
            var user = await _userRepository.GetAsync(LoginUser.Id);
            var pids = await _roleUserRepository.GetListPermIdByUserAsync(LoginUser.Id);
            var pids2 = await _userPermRepository.GetListPermIdByUserAsync(LoginUser.Id);
            pids = pids.Concat(pids2);

            var permissions = await _permRepository.GetListAsync(pids);
            var loginUser = _mapper.Map<SysTenantUserAggr, SysLoginUserAggr>(user);
            loginUser.SetPermissons(permissions);
            try
            {
                var cacheKey = CACHE_KEY.Fmt(LoginUser.Id);
                await _cacheRepository.SetStringAsync(cacheKey, loginUser.ToJson());
                return loginUser;
            }
            catch (Exception ex)
            {
                // redis服务没有启动，发送日志
                await _exceptionHttpService.AddAsync(new SysGlobalExceptionLogRequest()
                {
                    MoudleName = "系统管理-用户信息",
                    MoudleCode = "OneForAll.Base",
                    Name = "redis服务异常：",
                    Content = ex.StackTrace,
                    CreateTime = DateTime.Now,
                });
            }
            return loginUser;
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysPersonalForm form)
        {
            var data = await _userRepository.FindAsync(LoginUser.Id);
            if (data == null) return BaseErrType.DataNotFound;

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
            var data = await _userRepository.FindAsync(LoginUser.Id);
            if (data == null)
                return result;

            var newfileName = data.UserName + extension;
            var uploadPath = AppDomain.CurrentDomain.BaseDirectory + UPLOAD_PATH.Fmt(LoginUser.SysTenantId);
            var virtualPath = VIRTUAL_PATH.Fmt(LoginUser.SysTenantId);

            if (new ValidateImageType().Validate(filename, file))
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
        /// 修改密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ChangePasswordAsync(Password password)
        {
            var data = await _userRepository.FindAsync(LoginUser.Id);
            if (data == null) return BaseErrType.DataNotFound;

            var errType = data.ChangePassword(password);
            if (errType == BaseErrType.Success)
            {
                return await ResultAsync(() => _userRepository.UpdateAsync(data));
            }
            return errType;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysMenu>> GetListMenuAsync()
        {
            var menus = new List<SysMenu>();
            var user = await GetAsync();
            if (user != null)
            {
                var ids = user.SysLoginUserMenus.Select(s => s.Id);
                var data = await _menuRepository.GetListAsync();
                // 拥有下级菜单权限，代表拥有上级菜单
                ids.ForEach(id =>
                {
                    var userMenus = GetUserMenus(id, data);
                    menus.AddRange(userMenus);
                });
            }
            return menus.OrderBy(o => o.SortNumber).Distinct();
        }

        // 获取菜单以及上级
        private IEnumerable<SysMenu> GetUserMenus(Guid id, IEnumerable<SysMenu> sources)
        {
            var result = new List<SysMenu>();
            var data = sources.FirstOrDefault(w => w.Id.Equals(id));
            if (data != null)
            {
                result.Add(data);
                if (data.ParentId != Guid.Empty)
                {
                    var parents = GetUserMenus(data.ParentId, sources);
                    result.AddRange(parents);
                }
            }
            return result;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> LoginoutAsync()
        {
            try
            {
                var cacheKey = CACHE_KEY.Fmt(LoginUser.Id);
                await _cacheRepository.RemoveAsync(cacheKey);
            }
            catch
            {

            }
            return BaseErrType.Success;
        }
    }
}
