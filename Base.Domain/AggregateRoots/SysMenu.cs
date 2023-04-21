using Newtonsoft.Json;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Sys.Domain.Models;
using System.Linq;
using Sys.Domain.Enums;

namespace Sys.Domain.AggregateRoots
{
    /// <summary>
    /// 基础表：菜单
    /// </summary>
    public partial class SysMenu : AggregateRoot<Guid>, IParent<Guid>
    {
        /// <summary>
        /// 上级Id
        /// </summary>
        [Required]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 类型 0节点 1远程组件 2页面
        /// </summary>
        [Required]
        public SysMenuTypeEnum Type { get; set; }

        /// <summary>
        /// 打开方式 0标签内打开 1新标签打开 2新窗口打开
        /// </summary>
        [Required]
        public SysMenuOpenTypeEnum OpenType { get; set; }

        /// <summary>
        /// 菜单代码（由开发人员填写，值为Controller名称）
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Code { get; set; } = "";

        /// <summary>
        /// 页面路由
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Url { get; set; } = "";

        /// <summary>
        /// Api地址
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string ApiUrl { get; set; } = "";

        /// <summary>
        /// 图标（参考font-awesome）
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Icon { get; set; } = "";

        /// <summary>
        /// 是否启用（启用的菜单才能被加载出来）
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 是否默认（默认菜单不可删除）
        /// </summary>
        [Required]
        public bool IsDefault { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int SortNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Remark { get; set; } = "";


        /// <summary>
        /// 启用菜单
        /// </summary>
        public void SetEnable()
        {
            IsEnabled = !IsEnabled;
        }

        /// <summary>
        /// 启用菜单
        /// </summary>
        /// <param name="isEnable">是否启用</param>
        public void SetEnable(bool isEnable)
        {
            IsEnabled = isEnable;
        }
    }
}
