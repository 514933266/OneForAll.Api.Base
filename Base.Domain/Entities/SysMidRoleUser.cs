using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Entities
{
    /// <summary>
    /// 角色用户
    /// </summary>
    public class SysMidRoleUser
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
        /// 用户Id
        /// </summary>
        [Required]
        public Guid SysUserId { get; set; }
    }
}
