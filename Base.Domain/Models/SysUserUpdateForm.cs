using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Models
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
        /// 用户状态（关联BaseErrType，1正常 0异常 -20006禁止登录)
        /// </summary>
        public int Status { get; set; }
    }
}
