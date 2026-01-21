using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Base.Domain.ValueObjects
{
    /// <summary>
    /// 重置密码数据传输对象 (VO)
    /// 用于封装用户在修改密码时需要提交的原始密码、新密码以及重复新密码的验证信息。
    /// </summary>
    public class ResetPasswordVo
    {
        /// <summary>
        /// 获取或设置用户的原始密码（旧密码）。
        /// 在修改密码时，用于验证用户身份。
        /// </summary>
        public string Old { get; set; }

        /// <summary>
        /// 获取或设置用户想要设置的新密码。
        /// 新密码应符合系统的密码复杂度要求。
        /// </summary>
        public string New { get; set; }

        /// <summary>
        /// 获取或设置用于确认的新密码（重复新密码）。
        /// 通常用于与 'New' 属性进行比对，确保用户两次输入的新密码一致，防止输入错误。
        /// </summary>
        public string Repeat { get; set; }
    }
}
