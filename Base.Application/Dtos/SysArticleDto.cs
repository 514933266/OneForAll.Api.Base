using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.Dtos
{
    /// <summary>
    /// 机构文章
    /// </summary>
    public class SysArticleDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 所属分类Id
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// 所属分类名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 封面图路径
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublish { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        public int ReadNumber { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 作者名称
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
