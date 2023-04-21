using Sys.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sys.Domain.Aggregates;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 系统权限领域
    /// </summary>
    public interface ISysPermissionRepository : IEFCoreRepository<SysPermission>
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysPermissionAggr>> GetListAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="menuIds">菜单id</param>
        /// <returns>结果</returns>
        Task<IEnumerable<SysPermission>> GetListByMenuAsync(IEnumerable<Guid> menuIds);
    }
}
