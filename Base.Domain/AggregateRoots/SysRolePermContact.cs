using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public partial class SysRolePermContact : AggregateRoot<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid SysRoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [Required]
        public Guid SysPermissionId { get; set; }
    }
}
