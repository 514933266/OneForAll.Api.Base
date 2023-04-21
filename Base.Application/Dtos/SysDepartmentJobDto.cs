using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 部门岗位
    /// </summary>
    public class SysDepartmentJobDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 成员
        /// </summary>
        public virtual ICollection<SysJobUserDto> Users { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public virtual ICollection<SysJobRoleDto> Roles { get; set; }
    }
}
