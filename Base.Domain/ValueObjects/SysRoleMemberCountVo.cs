using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.ValueObjects
{
    /// <summary>
    /// 角色成员数量
    /// </summary>
    public class SysRoleMemberCountVo
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public Guid SysRoleId { get; set; }

        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCount { get; set; }
    }
}
