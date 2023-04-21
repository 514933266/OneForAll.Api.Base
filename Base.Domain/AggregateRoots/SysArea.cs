using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：中国地区
    /// </summary>
    public partial class SysArea : AggregateRoot<int>
    {

        /// <summary>
        /// 父级id
        /// </summary>
        [Required]
        public int ParentId { get; set; }

        /// <summary>
        /// 地区代码（下级地区继承上级，如00,0021,002133）
        /// </summary>
        [Required]
        [Column(TypeName ="varchar(20)")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string ShortName { get; set; } = "";

        /// <summary>
        /// 1省 2市 3县区 4镇街
        /// </summary>
        [Required]
        public byte Level { get; set; }
    }
}
