using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 部门用户
    /// </summary>
    public class SysDepartmentUserDto
    {
        /// <summary>
        /// 岗位用户关联表Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 所属部门Id
        /// </summary>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string IconUrl { get; set; }
    }
}
