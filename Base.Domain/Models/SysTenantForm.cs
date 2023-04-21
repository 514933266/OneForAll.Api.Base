using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sys.Domain.Models
{
    /// <summary>
    /// 表单：系统机构
    /// </summary>
    public class SysTenantForm : Entity<Guid>
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 机构代码
        /// </summary>
        [StringLength(50, MinimumLength = 8)]
        public string Code { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        [StringLength(50)]
        public string Manager { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [StringLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// 企业地址
        /// </summary>
        [StringLength(300)]
        public string Address { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(300)]
        public string Description { get; set; }
    }
}
