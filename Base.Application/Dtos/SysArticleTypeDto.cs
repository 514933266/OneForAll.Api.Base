using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 机构文章分类
    /// </summary>
    public class SysArticleTypeDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
