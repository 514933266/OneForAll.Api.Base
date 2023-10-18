using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SysPersonalLoginLogForm
    {
        /// <summary>
        /// 来源
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Source { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        [Required]
        [StringLength(20)]
        public string LoginType { get; set; }
    }
}
