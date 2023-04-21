using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    /// <summary>
    /// 表单：岗位
    /// </summary>
    public class SysJobForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(300)]
        public string Remark { get; set; }
    }
}
