using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：文章
    /// </summary>
    public class SysArticle : AggregateRoot<Guid>, ICreator<Guid>, ICreateTime
    {
        /// <summary>
        /// 所属租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        public virtual SysTenant SysTenant { get; set; }

        /// <summary>
        /// 所属分类Id
        /// </summary>
        [Required]
        public Guid SysArticleTypeId { get; set; }

        public virtual SysArticleType SysArticleType { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 封面图路径
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string IconUrl { get; set; } = "";

        /// <summary>
        /// 是否发布
        /// </summary>
        [Required]
        public bool IsPublish { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Source { get; set; } = "";

        /// <summary>
        /// 创建人Id
        /// </summary>
        [Required]
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Required]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public ICollection<SysArticleRecord> SysArticleRecords { get; set; } = new HashSet<SysArticleRecord>();
    }
}
