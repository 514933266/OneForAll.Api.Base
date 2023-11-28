using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：文章类型
    /// </summary>
    public class SysArticleType : AggregateRoot<Guid>
    {
        /// <summary>
        /// 所属租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Remark { get; set; } = "";
    }
}
