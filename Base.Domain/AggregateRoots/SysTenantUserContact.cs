using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 多租户用户关联表
    /// </summary>
    public class SysTenantUserContact
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid SysTenantId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 加入时间
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
