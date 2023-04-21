using AutoMapper;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core.Extension;
using Sys.Domain.Aggregates;

namespace Sys.Application
{
    /// <summary>
    /// 机构（子机构）
    /// </summary>
    public class SysTenantService : ISysTenantService
    {
        private readonly IMapper _mapper;
        private readonly ISysTenantMenuManager _menuManager;
        private readonly ISysTenantUserManager _tenantUserManager;
        public SysTenantService(
            IMapper mapper,
            ISysTenantMenuManager menuManager,
            ISysTenantUserManager tenantUserManager)
        {
            _mapper = mapper;
            _menuManager = menuManager;
            _tenantUserManager = tenantUserManager;
        }

        //#region 子机构

        ///// <summary>
        ///// 获取机构
        ///// </summary>
        ///// <param name="id">机构id</param>
        ///// <returns>机构</returns>
        //public async Task<SysTenantDto> GetAsync(Guid id)
        //{
        //    var data = await _manager.GetAsync(id);
        //    return _mapper.Map<SysTenant, SysTenantDto>(data);
        //}

        ///// <summary>
        ///// 获取分页
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">页数</param>
        ///// <param name="key">关键字</param>
        ///// <param name="isEnabled">合作状态（false未合作，true合作中）</param>
        ///// <param name="startDate">注册开始时间</param>
        ///// <param name="endDate">注册结束时间</param>
        ///// <returns>机构列表</returns>
        //public async Task<PageList<SysTenantDto>> GetPageAsync(
        //    int pageIndex,
        //    int pageSize,
        //    string key,
        //    bool isEnabled,
        //    DateTime? startDate,
        //    DateTime? endDate)
        //{
        //    var data = await _manager.GetPageAsync(pageIndex, pageSize, key, isEnabled, startDate, endDate);
        //    var items = _mapper.Map<IEnumerable<SysTenant>, IEnumerable<SysTenantDto>>(data.Items);
        //    return new PageList<SysTenantDto>(data.Total, data.PageIndex, data.PageSize, items);
        //}

        ///// <summary>
        ///// 获取用户列表
        ///// </summary>
        ///// <param name="id">机构id</param>
        ///// <param name="key">关键字</param>
        ///// <returns>用户列表</returns>
        //public async Task<IEnumerable<SysUserDto>> GetListUserAsync(Guid id, string key)
        //{
        //    var data = await _tenantUserManager.GetListAsync(id, key);
        //    return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysUserDto>>(data);
        //}

        ///// <summary>
        ///// 添加
        ///// </summary>
        ///// <param name="tenantId">登录机构id</param>
        ///// <param name="form">实体</param>
        ///// <returns>结果</returns>
        //public async Task<BaseErrType> AddAsync(Guid tenantId, SysTenantForm form)
        //{
        //    return await _manager.AddAsync(tenantId, form);
        //}

        ///// <summary>
        ///// 修改
        ///// </summary>
        ///// <param name="form">实体</param>
        ///// <returns>结果</returns>
        //public async Task<BaseErrType> UpdateAsync(SysTenantForm form)
        //{
        //    return await _manager.UpdateAsync(form);
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="ids">菜单id</param>
        ///// <returns>结果</returns>
        //public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        //{
        //    return await _manager.DeleteAsync(ids);
        //}
        //#endregion

        /// <summary>
        /// 查询机构菜单
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync()
        {
            var data = await _menuManager.GetListAsync();
            var menus = _mapper.Map<IEnumerable<SysMenuPermissionAggr>, IEnumerable<SysMenuTreeDto>>(data);
            return menus.ToTree<SysMenuTreeDto, Guid>();
        }
    }
}
