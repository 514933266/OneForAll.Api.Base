using Base.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Aggregates
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class SysUserRoleAggr : SysRole
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid SysUserId { get; set; }
    }
}
