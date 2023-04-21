using Sys.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Aggregates
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SysRoleAggr : SysRole
    {
        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCount { get; set; }
    }
}
