using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Public.Models
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 所属机构Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 所属机构id
        /// </summary>
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
