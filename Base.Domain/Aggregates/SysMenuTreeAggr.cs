using OneForAll.Core;
using Base.Domain.AggregateRoots;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Aggregates
{
    /// <summary>
    /// 实体：菜单树
    /// </summary>
    public class SysMenuTreeAggr : SysMenu, IChildren<SysMenuTreeAggr>
    {
        /// <summary>
        /// 子级
        /// </summary>
        public IEnumerable<SysMenuTreeAggr> Children { get; set; }
    }
}
