using OneForAll.Core;
using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：权限
    /// </summary>
    public partial class SysPermission : AggregateRoot<Guid>
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required]
        public Guid SysMenuId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 代码（由后端开发人员填写，规则为Action的名称）
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Code { get; set; }

        /// <summary>
        /// 排序代码
        /// </summary>
        [Required]
        public string SortCode { get; set; } = "";

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Remark { get; set; } = "";

    }
}
