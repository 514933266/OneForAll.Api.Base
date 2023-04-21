using Sys.Domain;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 实体：地区组
    /// </summary>
    public class SysAreaGroupDto : Entity<Guid>
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
        /// 成员人数
        /// </summary>
        public int MemberCount { get; set; }
    }
}
