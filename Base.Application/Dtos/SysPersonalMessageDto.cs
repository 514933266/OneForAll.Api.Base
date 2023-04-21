using Sys.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 系统通知
    /// </summary>
    public class SysPersonalMessageDto
    {
        /// <summary>
        /// 消息id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public UmsMessageTypeEnum Type { get; set; }

        /// <summary>
        /// 接收账号id
        /// </summary>
        public Guid ToAccountId { get; set; }

        /// <summary>
        /// 已读
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
