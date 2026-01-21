using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Base.Domain.Models
{
    /// <summary>
    /// 机构文章表单
    /// </summary>
    public class SysArticleForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 所属分类Id
        /// </summary>
        [Required]
        public Guid TypeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 封面图路径
        /// </summary>
        [StringLength(300)]
        public string IconUrl { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Required]
        public bool IsPublish { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [StringLength(100)]
        public string Source { get; set; }
    }
}
