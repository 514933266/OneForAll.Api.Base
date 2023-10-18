using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService.Models
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SysExceptionLogForm
    {
        /// <summary>
        /// 租户id
        /// </summary>
        [Required]
        public Guid TenantId { get; set; }

        /// <summary>
        /// 所属模块名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleName { get; set; }

        /// <summary>
        /// 模块代码
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MoudleCode { get; set; }

        /// <summary>
        /// 控制器
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Controller { get; set; }

        /// <summary>
        /// 控制器方法
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Action { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 详细内容
        /// </summary>
        [Required]
        public string Content { get; set; }

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
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
