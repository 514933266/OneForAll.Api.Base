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
    /// 机构权限
    /// </summary>
    public interface ISysTenantPermContactRepository : IEFCoreRepository<SysTenantPermContact>
    {
        /// <summary>
        /// 查询机构权限id
        /// </summary>
        /// <param name="tenantId">机构id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListPermissionIdAsync(Guid tenantId);
    }
}
