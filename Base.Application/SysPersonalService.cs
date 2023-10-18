using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Enums;
using Base.Domain.Interfaces;
using Base.Domain.Models;
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

        public SysPersonalService(IMapper mapper, ISysLoginLogHttpService logHttpService, ISysPersonalManager manager)
        {
            _mapper = mapper;
            _logHttpService = logHttpService;
            _manager = manager;
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
            var data = _mapper.Map<SysLoginLogForm>(form);
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
    }
}
