using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService.Models
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SysLoginLogRequest
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Source { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        [Required]
        [StringLength(20)]
        public string LoginType { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        [Required]
        [StringLength(20)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 创建人id
        /// </summary>
        [Required]
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        [Required]
        [StringLength(20)]
        public string CreatorName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; }
    }
}
