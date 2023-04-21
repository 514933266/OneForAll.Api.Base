using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 系统岗位
    /// </summary>
    public class SysJobDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 所属部门Id
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual ICollection<SysJobRoleDto> Roles { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public virtual ICollection<SysJobUserDto> Users { get; set; }
    }
}
