using Base.Domain.AggregateRoots;
using Base.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：角色权限
    /// </summary>
    public interface ISysRolePermissionManager
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<SysPermission>> GetListAsync(Guid roleId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <param name="forms">权限id</param>
        /// <returns>权限列表</returns>
        Task<BaseErrType> AddAsync(Guid roleId, IEnumerable<SysMenuPermissionForm> forms);
    }
}
