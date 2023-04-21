using Sys.Domain.AggregateRoots;
using Sys.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sys.Domain.Enums;

namespace Sys.Domain.Aggregates
{
    /// <summary>
    /// 实体：登录用户
    /// </summary>
    public class SysLoginUserAggr : SysTenantUserAggr
    {
        /// <summary>
        /// 菜单权限
        /// </summary>
        public virtual List<SysLoginUserMenu> SysLoginUserMenus { get; set; } = new List<SysLoginUserMenu>();

        /// <summary>
        /// 初始化用户权限
        /// </summary>
        /// <param name="permissions">用户</param>
        public void SetPermissons(IEnumerable<SysPermissionAggr> permissions)
        {
            permissions.ForEach(permission =>
            {
                var menu = SysLoginUserMenus.FirstOrDefault(w => w.Id == permission.SysMenuId);
                if (menu == null)
                {
                    menu = new SysLoginUserMenu()
                    {
                        Id = permission.SysMenu.Id,
                        ParentId = permission.SysMenu.ParentId,
                        Url = permission.SysMenu.Url,
                        Code = permission.SysMenu.Code,
                        Type = permission.SysMenu.Type,
                        Name = permission.SysMenu.Name
                    };
                }
                if (!menu.SysLoginUserPermissions.Any(w => w.Code == permission.Code))
                {
                    menu.SysLoginUserPermissions.Add(new SysLoginUserPermission()
                    {
                        Code = permission.Code,
                        Name = permission.Name,
                        SortCode = permission.SortCode
                    });
                }
                if (!SysLoginUserMenus.Any(w => w.Id == menu.Id) && menu.Type == SysMenuTypeEnum.Page)
                {
                    SysLoginUserMenus.Add(menu);
                }
            });
        }

        /// <summary>
        /// 校验权限
        /// </summary>
        /// <param name="menuCode">菜单代码</param>
        /// <param name="permCode">权限代码</param>
        /// <returns>结果</returns>
        public BaseErrType ValidatePermission(string menuCode, string permCode)
        {
            var menus = SysLoginUserMenus.Where(w => w.Code.Contains(menuCode)).ToList();
            for (var i = 0; i < menus.Count; i++)
            {
                if (menus[i].SysLoginUserPermissions.Any(w => w.Code.Equals(permCode)))
                {
                    return BaseErrType.Success;
                }
            }
            return BaseErrType.PermissionNotEnough;
        }
    }
}
