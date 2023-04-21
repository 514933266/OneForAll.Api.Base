using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 角色成员
    /// </summary>
    public class SysRoleMemberDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        public string IconUrl { get; set; }
    }
}
