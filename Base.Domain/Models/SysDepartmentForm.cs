using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    public class SysDepartmentForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 上级Id
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 排序号（由大到小排序）
        /// </summary>
        [Required]
        public int SortNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(300)]
        public string Remark { get; set; }
    }
}
