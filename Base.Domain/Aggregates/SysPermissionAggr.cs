using Base.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Aggregates
{
    /// <summary>
    /// 权限
    /// </summary>
    public class SysPermissionAggr : SysPermission
    {
        /// <summary>
        /// 菜单
        /// </summary>
        public SysMenu SysMenu { get; set; }
    }
}
