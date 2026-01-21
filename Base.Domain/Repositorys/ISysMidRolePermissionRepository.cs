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
    }
}
