using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models
{
    /// <summary>
    /// 修改个人手机号
    /// </summary>
    public class SysPersonalUpdateMobileForm
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required]
        [StringLength(20)]
        [RegularExpression(@"^1[0-9]{10}$", ErrorMessage = "手机号格式不正确")]
        public string Mobile { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [StringLength(20)]
        public string Code { get; set; }
    }
}
