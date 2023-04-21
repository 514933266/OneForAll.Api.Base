using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Base.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：部门岗位
    /// </summary>
    public class SysJob : AggregateRoot<Guid>
    {
        public SysJob()
        {
            SysJobRoleContacts = new HashSet<SysJobRoleContact>();
            SysJobUserContacts = new HashSet<SysJobUserContact>();
        }
        /// <summary>
        /// 所属部门Id
        /// </summary>
        [Required]
        public Guid SysDepartmentId { get; set; }

        public virtual SysDepartment SysDepartment { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Remark { get; set; } = "";

        public virtual ICollection<SysJobRoleContact> SysJobRoleContacts { get; set; }

        public virtual ICollection<SysJobUserContact> SysJobUserContacts { get; set; }
    }
}
