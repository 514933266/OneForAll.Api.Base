using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 系统岗位-用户关联表
    /// </summary>
    public class SysJobUserContact : AggregateRoot<Guid>
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }
        public virtual SysUser SysUser { get; set; }

        /// <summary>
        /// 岗位Id
        /// </summary>
        [Required]
        public Guid SysJobId { get; set; }

        public virtual SysJob SysJob { get; set; }
    }
}
