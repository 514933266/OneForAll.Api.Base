using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entities
{
    /// <summary>
    /// 用户权限
    /// </summary>
    public partial class SysMidUserPermission
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [Required]
        public Guid SysPermissionId { get; set; }
    }
}
