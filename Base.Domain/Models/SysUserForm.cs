using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    /// <summary>
    /// 系统用户表单
    /// </summary>
    public class SysUserForm
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(32)]
        public string Password { get; set; }

        /// <summary>
        /// 重复密码
        /// </summary>
        [StringLength(32)]
        public string RePassword { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 用户状态（关联BaseErrType，1正常 0异常 -20006禁止登录)
        /// </summary>
        public int Status { get; set; }
    }
}
