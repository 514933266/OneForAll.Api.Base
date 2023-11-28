using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Enums;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 微信公众号订阅用户
    /// </summary>
    public class SysWxgzhSubscribeUser
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 基础用户id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        [Required]
        [StringLength(200)]
        public string AppId { get; set; }

        /// <summary>
        /// 微信用户OpenId
        /// </summary>
        [Required]
        [StringLength(200)]
        public string OpenId { get; set; }

        /// <summary>
        /// 订阅时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 关注方式
        /// </summary>
        [Required]
        public SysWxgzhMsgTypeEnum SubscribeType { get; set; } = SysWxgzhMsgTypeEnum.Subscribe;

        /// <summary>
        /// 是否已取消订阅
        /// </summary>
        [Required]
        public bool IsUnSubscribed { get; set; }
    }
}
