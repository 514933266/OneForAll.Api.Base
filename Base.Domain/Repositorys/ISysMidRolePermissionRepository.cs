using OneForAll.EFCore;
using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public interface ISysMidRolePermissionRepository : IEFCoreRepository<SysMidRolePermission>
    {
        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysMidRolePermission>> GetListAsync(Guid roleId);

        /// <summary>
        /// 查询角色权限id
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListPermissionIdAsync(Guid roleId);

        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysPermission>> GetListPermissionAsync(Guid roleId);

        /// <summary>
        /// 批量查询多个角色的权限关联（用于分页装配消除 N+1）
        /// </summary>
        /// <param name="roleIds">角色id集合</param>
        /// <returns>角色权限关联列表</returns>
        Task<IEnumerable<SysMidRolePermission>> GetListByRolesAsync(IEnumerable<Guid> roleIds);
    }
}
