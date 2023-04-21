using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：租户（租户）
    /// </summary>
    public partial class SysTenant : AggregateRoot<Guid>, ICreateTime, IParent<Guid>
    {
        /// <summary>
        /// 上级id
        /// </summary>
        [Required]

        public Guid ParentId { get; set; }

        /// <summary>
        /// 租户代码
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Code { get; set; }


        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Manager { get; set; } = "";

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Phone { get; set; } = "";

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = "";

        /// <summary>
        /// 是否默认（默认租户禁止删除）
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否启用（未启用租户用户禁止登录）
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Description { get; set; } = "";

        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
