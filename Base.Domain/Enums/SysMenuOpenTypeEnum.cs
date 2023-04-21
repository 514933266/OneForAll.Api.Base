using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Enums
{
    /// <summary>
    /// 菜单打开类型
    /// </summary>
    public enum SysMenuOpenTypeEnum
    {
        None = -1,

        /// <summary>
        /// 标签内打开
        /// </summary>
        Tab = 0,

        /// <summary>
        /// 新标签打开
        /// </summary>
        NewTab = 1,

        /// <summary>
        /// 新窗口
        /// </summary>
        NewWindow = 2,
    }
}
