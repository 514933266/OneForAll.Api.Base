using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Dtos
{
    /// <summary>
    /// 机构用户权限
    /// </summary>
    public class SysTenantUserMenuDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 权限Code
        /// </summary>
        public virtual IEnumerable<string> Permissions { get; set; }
    }
}
