using OneForAll.EFCore;
using Base.Domain.AggregateRoots;
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
    public interface ISysUserPermContactRepository : IEFCoreRepository<SysUserPermContact>
    {
        /// <summary>
        /// 查询用户权限id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListPermIdByUserAsync(Guid userId);
    }
}
