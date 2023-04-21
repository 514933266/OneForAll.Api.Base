using Sys.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 地区
    /// </summary>
    public interface ISysAreaRepository : IEFCoreRepository<SysArea>
    {
        /// <summary>
        /// 查询子级
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysArea>> GetChildrenAsync(int parentId);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="codes">代码集合</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysArea>> GetListAsync(IEnumerable<string> codes);
    }
}
