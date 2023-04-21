using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 岗位角色
    /// </summary>
    public class SysJobRoleDto
    {
        /// <summary>
        /// 岗位角色关联表Id
        /// </summary>
        public Guid Id { get; set; }

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
