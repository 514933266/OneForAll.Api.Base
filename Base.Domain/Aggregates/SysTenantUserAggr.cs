using Base.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Aggregates
{
    /// <summary>
    /// 租户用户
    /// </summary>
    public class SysTenantUserAggr : SysUser
    {
        /// <summary>
        /// 租户
        /// </summary>
        public SysTenant SysTenant { get; set; }
    }
}
