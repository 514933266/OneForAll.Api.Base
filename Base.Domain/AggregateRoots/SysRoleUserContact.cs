using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 角色用户
    /// </summary>
    public class SysRoleUserContact : AggregateRoot<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid SysRoleId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }
    }
}
