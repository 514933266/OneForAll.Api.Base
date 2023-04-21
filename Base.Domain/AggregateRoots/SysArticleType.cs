using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.AggregateRoots
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

        public virtual SysTenant SysTenant { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Remark { get; set; } = "";

        public ICollection<SysArticle> SysArticles { get; set; } = new HashSet<SysArticle>();
    }
}
