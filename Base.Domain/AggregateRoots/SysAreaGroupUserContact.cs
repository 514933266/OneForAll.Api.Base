using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 关联表：地区组用户
    /// </summary>
    public partial class SysAreaGroupUserContact: AggregateRoot<Guid>
    {

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 地区组Id
        /// </summary>
        public Guid SysAreaGroupId { get; set; }

        public virtual SysAreaGroup SysAreaGroup { get; set; }
        public virtual SysUser SysUser { get; set; }
    }
}
