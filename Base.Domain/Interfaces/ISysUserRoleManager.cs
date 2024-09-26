using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 角色用户
    /// </summary>
    public interface ISysUserRoleManager
    {
        /// <summary>
        /// 获取用户角色列表
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<SysUserRoleAggr>> GetListAsync(IEnumerable<Guid> userIds);
    }
}
