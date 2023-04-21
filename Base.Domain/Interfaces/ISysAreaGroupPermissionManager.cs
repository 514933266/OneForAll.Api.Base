using Sys.Domain.AggregateRoots;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：地区组权限
    /// </summary>
    public interface ISysAreaGroupPermissionManager
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<SysArea>> GetListAsync(Guid groupId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <param name="areaCodes">地区代码</param>
        /// <returns>权限列表</returns>
        Task<BaseErrType> AddAsync(Guid groupId, IEnumerable<string> areaCodes);
    }
}
