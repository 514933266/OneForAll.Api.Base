using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 实体：部门组织
    /// </summary>
    public class SysDepartmentDto : Entity<Guid>, IParent<Guid>, IEntity<Guid>, IChildren<SysDepartmentDto>
    {
        /// <summary>
        /// 上级Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int SortNumber { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 子级
        /// </summary>

        public IEnumerable<SysDepartmentDto> Children { get; set; }
    }
}
