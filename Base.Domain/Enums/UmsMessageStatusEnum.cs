using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums
{
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum UmsMessageStatusEnum
    {
        /// <summary>
        /// 无
        /// </summary>
        None = -1,

        /// <summary>
        /// 未读
        /// </summary>
        UnRead = 0,

        /// <summary>
        /// 已读
        /// </summary>
        Readed = 1
    }
}
