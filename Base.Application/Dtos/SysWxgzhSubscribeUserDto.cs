using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Enums;

namespace Base.Application.Dtos
{
    /// <summary>
    /// 微信公众号关注用户
    /// </summary>
    public class SysWxgzhSubscribeUserDto
    {
        /// <summary>
        /// 订阅时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 关注方式
        /// </summary>
        public SysWxgzhMsgTypeEnum SubscribeType { get; set; }

        /// <summary>
        /// 是否已取消订阅
        /// </summary>
        public bool IsUnSubscribed { get; set; }
    }
}
