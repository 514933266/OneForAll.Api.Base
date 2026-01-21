using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Base.Domain.Entities
{
    /// <summary>
    /// 子表：文章阅读记录
    /// </summary>
    public class SysArticleRecord
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 文章Id
        /// </summary>
        [Required]
        public Guid SysArticleId { get; set; }

        /// <summary>
        /// 浏览用户Id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.UtcNow;
    }
}
