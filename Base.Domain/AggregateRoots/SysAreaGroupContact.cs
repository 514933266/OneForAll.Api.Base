using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 关联表：地区组权限
    /// </summary>
    public class SysAreaGroupContact : AggregateRoot<Guid>
    {
        /// <summary>
        /// 地区组Id
        /// </summary>
        [Required]
        public Guid SysAreaGroupId { get; set; }

        /// <summary>
        /// 地区代码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string AreaCode { get; set; }

        public virtual SysAreaGroup SysAreaGroup { get; set; }
    }
}
