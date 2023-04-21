using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 个人菜单
    /// </summary>
    public class SysPersonalMenuDto
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 权限Code
        /// </summary>
        public virtual IEnumerable<string> Permissions { get; set; }
    }
}
