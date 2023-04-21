using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 部门角色
    /// </summary>
    public class SysDepartmentRoleDto
    {
        /// <summary>
        /// 岗位角色关联表Id
        /// </summary>
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
