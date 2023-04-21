using OneForAll.Core.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 实体：用户
    /// </summary>
    public class SysTenantUserDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 头像路径
        /// </summary>
        public string IconUrl { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 用户状态（关联BaseErrType，1正常 0异常 -20006禁止登录)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否默认（默认用户禁止删除）
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 最后登录Ip
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 菜单权限
        /// </summary>
        public virtual IEnumerable<SysTenantUserMenuDto> Menus { get; set; }
    }
}
