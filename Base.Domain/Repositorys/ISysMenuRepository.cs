using Sys.Domain.AggregateRoots;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public interface ISysMenuRepository : IEFCoreRepository<SysMenu>
    {

        /// <summary>
        /// 查询机构菜单权限列表
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysMenu>> GetListByTenantAsync(Guid tenantId);
    }
}
