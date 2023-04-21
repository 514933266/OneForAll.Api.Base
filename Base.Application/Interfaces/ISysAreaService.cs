using Sys.Application.Dtos;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application.Interfaces
{
    /// <summary>
    /// 地区
    /// </summary>
    public interface ISysAreaService
    {
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns>列表</returns>
        Task<IEnumerable<SysAreaSelectionDto>> GetListProvinceAsync();

        /// <summary>
        /// 获取子级
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysAreaSelectionDto>> GetChildrenAsync(int parentId);
    }
}
