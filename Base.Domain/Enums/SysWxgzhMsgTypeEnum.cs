using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Enums
{
    /// <summary>
    /// 微信公众号关注类型
    /// </summary>
    public enum SysWxgzhMsgTypeEnum
    {
        /// <summary>
        /// 公众号关注
        /// </summary>
        Subscribe = 1000,

        /// <summary>
        /// 公众号取消关注
        /// </summary>
        UnSubscribe = 1001,

        /// <summary>
        /// 二维码关注
        /// </summary>
        QrCodeSubscribe = 1002,

        /// <summary>
        /// 二维码关注
        /// </summary>
        QrCodeUnSubscribe = 1003
    }
}
