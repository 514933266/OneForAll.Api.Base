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
    /// 用户权限
    /// </summary>
    public interface ISysMidUserPermissionRepository : IEFCoreRepository<SysMidUserPermission>
    {
        /// <summary>
        /// 查询用户权限id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListPermIdByUserAsync(Guid userId);

        /// <summary>
        /// 批量查询多个用户的直授权限关联（用于分页装配消除 N+1）
        /// </summary>
        /// <param name="userIds">用户id集合</param>
        /// <returns>用户权限关联列表</returns>
        Task<IEnumerable<SysMidUserPermission>> GetListByUsersAsync(IEnumerable<Guid> userIds);
    }
}
