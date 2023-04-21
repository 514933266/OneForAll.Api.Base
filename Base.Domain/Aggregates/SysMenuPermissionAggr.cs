using Sys.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Aggregates
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenuPermissionAggr : SysMenu
    {
        /// <summary>
        /// 权限
        /// </summary>
        public List<SysPermission> SysPermissions { get; set; } = new List<SysPermission>();
    }
}
