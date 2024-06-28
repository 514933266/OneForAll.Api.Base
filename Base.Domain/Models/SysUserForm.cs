using Base.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Base.Domain.Models
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
        /// 手机号码
        /// </summary>
        [StringLength(20)]
        [RegularExpression("^1[0-9]{10}$", ErrorMessage = "手机号码格式错误")]
        public string Mobile { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public SysUserStatusEnum Status { get; set; }
    }
}
