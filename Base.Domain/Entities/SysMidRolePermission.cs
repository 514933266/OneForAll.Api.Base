using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entities
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public partial class SysMidRolePermission
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public Guid SysRoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        [Required]
        public Guid SysPermissionId { get; set; }
    }
}
