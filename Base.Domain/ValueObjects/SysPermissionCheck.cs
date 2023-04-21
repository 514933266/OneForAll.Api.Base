using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sys.Domain.ValueObjects
{
    /// <summary>
    /// 值对象：权限验证
    /// </summary>
    public class SysPermissionCheck
    {
        /// <summary>
        /// Controller名称
        /// </summary>
        [Required]
        public string Controller { get; set; }

        /// <summary>
        /// Action的名称
        /// </summary>
        [Required]
        public string Action { get; set; }
    }
}
