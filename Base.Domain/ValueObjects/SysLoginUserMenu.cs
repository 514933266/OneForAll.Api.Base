using Base.Domain.ValueObjects;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Domain.Enums;

namespace Base.Domain.ValueObjects
{
    /// <summary>
    /// 值对象：登录用户菜单
    /// </summary>
    public class SysLoginUserMenu : IEntity<Guid>, IParent<Guid>, IChildren<SysLoginUserMenu>
    {
        public SysLoginUserMenu()
        {
            SysLoginUserPermissions = new HashSet<SysLoginUserPermission>();
        }

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public SysMenuTypeEnum Type { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单权限
        /// </summary>
        public ICollection<SysLoginUserPermission> SysLoginUserPermissions { get; set; }

        /// <summary>
        /// 子级
        /// </summary>
        public IEnumerable<SysLoginUserMenu> Children { get; set; }
    }
}
