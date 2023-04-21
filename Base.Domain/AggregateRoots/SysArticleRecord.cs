using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 子表：文章阅读记录
    /// </summary>
    public class SysArticleRecord : AggregateRoot<Guid>
    {
        /// <summary>
        /// 文章Id
        /// </summary>
        public Guid SysArticleId { get; set; }

        public virtual SysArticle SysArticle { get; set; }

        /// <summary>
        /// 浏览用户Id
        /// </summary>
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
