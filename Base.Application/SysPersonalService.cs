using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Enums;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application
{
    /// <summary>
    /// 个人中心
    /// </summary>
    public class SysPersonalService : ISysPersonalService
    {
        private readonly IMapper _mapper;
        private readonly ISysLoginLogHttpService _logHttpService;
        private readonly ISysPersonalManager _manager;
        private readonly ISysWechatUserRepository _wxUserRepository;
        private readonly ISysWxgzhSubscribeUserRepository _wxgzhUserRepository;
        public SysPersonalService(
            IMapper mapper,
            ISysLoginLogHttpService logHttpService,
            ISysPersonalManager manager,
            ISysWechatUserRepository wxUserRepository,
            ISysWxgzhSubscribeUserRepository wxgzhUserRepository)
        {
            _mapper = mapper;
            _logHttpService = logHttpService;
            _manager = manager;
            _wxUserRepository = wxUserRepository;
            _wxgzhUserRepository = wxgzhUserRepository;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <returns>实体</returns>
        public async Task<SysPersonalDto> GetAsync()
        {
            var data = await _manager.GetAsync();
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
        public async Task<BaseErrType> ChangePasswordAsync(Password password)
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
        /// 获取菜单
        /// </summary>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync()
        {
            var data = await _manager.GetListMenuAsync();
            var menus = _mapper.Map<IEnumerable<SysMenu>, IEnumerable<SysMenuTreeDto>>(data);
            return menus.ToTree<SysMenuTreeDto, Guid>();
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysMenuDto>> GetListMenuAsync()
        {
            var data = await _manager.GetListMenuAsync();
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
            return await _manager.LoginoutAsync();
        }

        #region 微信相关

        /// <summary>
        /// 获取微信access_token
        /// </summary>
        /// <param name="userId">登录用户id</param>
        /// <returns></returns>
        public async Task<SysWechatUserAccessTokenDto> GetWxAccessTokenAsync(Guid userId)
        {
            var data = await _wxUserRepository.GetAsync(w => w.SysUserId == userId);
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
            var data = await _wxgzhUserRepository.GetAsync(w => w.SysUserId == userId);
            var item = _mapper.Map<SysWxgzhSubscribeUserDto>(data);
            if (data == null)
                item.IsUnSubscribed = true;
            return item;
        }
        #endregion
    }
}
