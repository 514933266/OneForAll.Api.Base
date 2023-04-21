using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Enums
{
    /// <summary>
    /// 系统通知类型
    /// </summary>
    public enum SysNotificationTypeEnum
    {
        /// <summary>
        /// 所有账号
        /// </summary>
        AllAccount = 0,

        /// <summary>
        /// 主账号
        /// </summary>
        MainAccount = 1,

        /// <summary>
        /// 指定用户
        /// </summary>
        AnyAccount = 2
    }
}
