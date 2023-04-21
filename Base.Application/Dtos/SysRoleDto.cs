using Sys.Domain;
using Sys.Domain.AggregateRoots;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 实体：角色
    /// </summary>
    public class SysRoleDto : Entity<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 成员数量
        /// </summary>
        public int MemberCount { get; set; }
    }
}
