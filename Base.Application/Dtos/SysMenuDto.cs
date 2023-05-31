using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Dtos
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class SysMenuDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型 0节点 1远程组件 2页面
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 打开方式 0标签内打开 1新标签打开 2新窗口打开
        /// </summary>
        public int OpenType { get; set; }

        /// <summary>
        /// 菜单代码（由开发人员填写，值为Controller名称）
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 页面路由
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否新窗口菜单（如果为True，则打开新标签访问）
        /// </summary>
        public bool IsBlank { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
