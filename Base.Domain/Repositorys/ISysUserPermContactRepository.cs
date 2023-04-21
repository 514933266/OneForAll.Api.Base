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
    /// 用户权限
    /// </summary>
    public interface ISysUserPermContactRepository : IEFCoreRepository<SysUserPermContact>
    {
        /// <summary>
        /// 查询用户权限id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListPermissionIdByUserAsync(Guid userId);
    }
}
