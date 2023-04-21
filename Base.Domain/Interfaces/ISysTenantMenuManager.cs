﻿using Sys.Domain.AggregateRoots;
using Sys.Domain.Aggregates;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：机构菜单
    /// </summary>
    public interface ISysTenantMenuManager
    {
        /// <summary>
        /// 获取菜单权限树
        /// </summary>
        /// <returns>列表</returns>
        Task<IEnumerable<SysMenuPermissionAggr>> GetListAsync();
    }
}
