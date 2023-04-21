using OneForAll.EFCore;
using Sys.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public interface ISysRolePermContactRepository : IEFCoreRepository<SysRolePermContact>
    {
        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysRolePermContact>> GetListAsync(Guid roleId);

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
