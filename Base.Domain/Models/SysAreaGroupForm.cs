using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    /// <summary>
    /// 表单：地区组
    /// </summary>
    public class SysAreaGroupForm : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(100)]
        public string Remark { get; set; }
    }
}
