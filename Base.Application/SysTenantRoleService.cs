using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;

namespace Base.Application
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SysTenantRoleService : ISysTenantRoleService
    {
        private readonly IMapper _mapper;
        private readonly ISysTenantRoleManager _roleManager;
        private readonly ISysRolePermissionManager _rolePermManager;
        private readonly ISysRoleMemberManager _roleMemberManager;
        public SysTenantRoleService(
            IMapper mapper,
            ISysTenantRoleManager roleManager,
            ISysRolePermissionManager rolePermManager,
            ISysRoleMemberManager roleMemberManager)
        {
            _mapper = mapper;
            _roleManager = roleManager;
            _rolePermManager = rolePermManager;
            _roleMemberManager = roleMemberManager;
        }

        #region 角色

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysRoleDto>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var data = await _roleManager.GetPageAsync(pageIndex, pageSize, key);
            var items = _mapper.Map<IEnumerable<SysRoleAggr>, IEnumerable<SysRoleDto>>(data.Items);
            return new PageList<SysRoleDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">角色</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysRoleForm form)
        {
            return await _roleManager.AddAsync(tenantId, form);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">角色</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysRoleForm form)
        {
            return await _roleManager.UpdateAsync(form);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">角色id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            return await _roleManager.DeleteAsync(ids);
        }

        #endregion

        #region 权限

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns>权限列表</returns>
        public async Task<IEnumerable<SysMenuPermissionDto>> GetListPermissionAsync(Guid id)
        {
            var data = await _rolePermManager.GetListAsync(id);
            return _mapper.Map<IEnumerable<SysPermission>, IEnumerable<SysMenuPermissionDto>>(data);
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="forms">权限id</param>
        /// <returns>权限列表</returns>
        public async Task<BaseErrType> AddPermissionAsync(Guid id, IEnumerable<SysMenuPermissionForm> forms)
        {
            return await _rolePermManager.AddAsync(id, forms);
        }
        #endregion

        #region 成员

        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns>成员列表</returns>
        public async Task<IEnumerable<SysRoleMemberDto>> GetListMemberAsync(Guid id)
        {
            var data = await _roleMemberManager.GetListAsync(id);
            return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysRoleMemberDto>>(data);
        }

        /// <summary>
        /// 获取未加入角色的成员列表
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        public async Task<IEnumerable<SysRoleSelectionMemberDto>> GetListUnJoinedUserAsync(Guid id, string key)
        {
            var data = await _roleMemberManager.GetListUnJoinedAsync(id, key);
            return _mapper.Map<IEnumerable<SysUser>, IEnumerable<SysRoleSelectionMemberDto>>(data);
        }

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userIds">用户id集合</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddMemberAsync(Guid id, IEnumerable<Guid> userIds)
        {
            return await _roleMemberManager.AddAsync(id, userIds);
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userIds">用户关联Id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> RemoveMemberAsync(Guid id, IEnumerable<Guid> userIds)
        {
            return await _roleMemberManager.RemoveAsync(id, userIds);
        }
        #endregion
    }
}
