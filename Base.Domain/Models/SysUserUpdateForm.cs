using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Models
{
    /// <summary>
    /// 修改用户
    /// </summary>
    public class SysUserUpdateForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [StringLength(20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public int Status { get; set; }
    }
}
