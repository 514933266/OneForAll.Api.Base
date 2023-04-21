using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    /// <summary>
    /// 角色表单
    /// </summary>
    public class SysRoleForm
    {
        public Guid Id { get; set; }

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
