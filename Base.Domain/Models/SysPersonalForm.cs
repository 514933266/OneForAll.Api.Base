using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    /// <summary>
    /// 个人信息表单
    /// </summary>
    public class SysPersonalForm
    {
        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [StringLength(100)]
        public string Signature { get; set; }
    }
}
