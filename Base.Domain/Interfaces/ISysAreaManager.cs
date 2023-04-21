using Sys.Domain.AggregateRoots;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：地区
    /// </summary>
    public interface ISysAreaManager
    {
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns>列表</returns>
        Task<IEnumerable<SysArea>> GetListProvinceAsync();

        /// <summary>
        /// 获取子级
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>列表</returns>

        Task<IEnumerable<SysArea>> GetChildrenAsync(int parentId);
    }
}
