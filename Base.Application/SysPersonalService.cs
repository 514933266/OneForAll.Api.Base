using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.Entities;
using Base.Domain.Aggregates;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.ValueObjects;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Microsoft.Extensions.Configuration;
using OneForAll.Core;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.UploadFile;

namespace Base.Application
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public class SysPersonalService : ISysPersonalService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly ISysLoginLogHttpService _logHttpService;
        private readonly ISysPersonalManager _manager;
        private readonly ISysUserQueryService _queryService;
        private readonly ITxCloudSmsHttpService _smsHttpService;
        private readonly ISysWechatQueryService _wechatQueryService;
        public SysPersonalService(
            IMapper mapper,
            IConfiguration config,
            ISysLoginLogHttpService logHttpService,
            ISysPersonalManager manager,
            ISysUserQueryService queryService,
            ITxCloudSmsHttpService smsHttpService,
            ISysWechatQueryService wechatQueryService)
        {
            _mapper = mapper;
            _config = config;
            _logHttpService = logHttpService;
            _manager = manager;
            _queryService = queryService;
            _smsHttpService = smsHttpService;
            _wechatQueryService = wechatQueryService;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns>实体</returns>
        public async Task<SysPersonalDto> GetAsync()
        {
            // 读编排归 Application：登录态组装走 Domain 读服务（含 LoginInfo 缓存回填）
            var data = await _queryService.GetPersonalAsync();
            return _mapper.Map<SysLoginUserAggr, SysPersonalDto>(data);
        }

        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysPersonalForm form)
        {
            return await _manager.UpdateAsync(form);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>上传结果</returns>
        public async Task<IUploadResult> UploadHeaderAsync(string filename, Stream file)
        {
            return await _manager.UploadHeaderAsync(filename, file);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ChangePasswordAsync(ResetPasswordVo password)
        {
            return await _manager.ChangePasswordAsync(password);
        }

        /// <summary>
        /// 修改绑定所属机构
        /// </summary>
        /// <param name="tenantId">要新绑定的租户id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateTenantAsync(Guid tenantId)
        {
            return await _manager.UpdateTenantAsync(tenantId);
        }

        /// <summary>
        /// 修改手机号（两段式协议编排：短信发送/模板配置属基础设施，不下沉领域 Manager）
        /// </summary>
        public async Task<BaseErrType> UpdateMobileAsync(SysPersonalUpdateMobileForm form)
        {
            if (form.Code.IsNullOrEmpty())
            {
                // 第一段：申请并发送验证码
                var (err, code) = await _manager.BeginSendMobileUpdateCodeAsync(form.Mobile);
                if (err != BaseErrType.Success)
                    return err; // NotAllow：验证码仍在有效期内

                var templateId = _config["TxCloudSms:TemplateId"];
                var signName = _config["TxCloudSms:SignName"];
                var msg = await _smsHttpService.SendAsync(signName, code, templateId, form.Mobile);
                if (!msg.Status)
                    return BaseErrType.Fail; // 发送失败：不记录验证码，用户可立即重试

                await _manager.ConfirmMobileUpdateCodeSentAsync(form.Mobile, code);
                return BaseErrType.Success;
            }
            else
            {
                // 第二段：校验验证码并变更手机号
                var err = await _manager.CommitMobileUpdateCodeAsync(form);
                if (err != BaseErrType.Success)
                    return err;

                return await _manager.ChangeMobileAsync(form.Mobile);
            }
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync()
        {
            var data = await _queryService.GetPersonalMenusAsync();
            var menus = _mapper.Map<IEnumerable<SysMenu>, IEnumerable<SysMenuTreeDto>>(data);
            return menus.ToTree<SysMenuTreeDto, Guid>();
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysMenuDto>> GetListMenuAsync()
        {
            var data = await _queryService.GetPersonalMenusAsync();
            return _mapper.Map<IEnumerable<SysMenu>, IEnumerable<SysMenuDto>>(data);
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> LoginAsync(SysPersonalLoginLogForm form)
        {
            var data = _mapper.Map<SysLoginLogRequest>(form);
            await _logHttpService.AddAsync(data);
            return BaseErrType.Success;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> LoginoutAsync()
        {
            return await _queryService.LoginoutAsync();
        }

        #region 微信相关

        /// <summary>
        /// 获取微信access_token
        /// </summary>
        /// <param name="userId">登录用户id</param>
        /// <returns></returns>
        public async Task<SysWechatUserAccessTokenDto> GetWxAccessTokenAsync(Guid userId)
        {
            var data = await _wechatQueryService.GetUserAsync(userId);
            if (data == null)
                return new SysWechatUserAccessTokenDto();

            return new SysWechatUserAccessTokenDto()
            {
                AccessToken = data.AccessToken,
                ExpiresIn = data.AccessTokenExpiresIn,
                CreateTime = data.AccessTokenCreateTime
            };
        }

        /// <summary>
        /// 获取关注微信公众号信息
        /// </summary>
        /// <returns></returns>
        public async Task<SysWxgzhSubscribeUserDto> GetWxgzhSubscribeUserAsync(Guid userId)
        {
            var data = await _wechatQueryService.GetGzhSubscriberAsync(userId);
            var item = _mapper.Map<SysWxgzhSubscribeUserDto>(data);
            if (data == null)
                item.IsUnSubscribed = true;
            return item;
        }
        #endregion
    }
}
