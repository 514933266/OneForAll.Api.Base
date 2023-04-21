using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Enums
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public enum SysEnabledEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = -1,

        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 0,

        /// <summary>
        /// 启用
        /// </summary>
        Enabled = 1
    }
}
