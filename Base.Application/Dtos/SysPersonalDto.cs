using Base.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.Dtos
{
    /// <summary>
    /// 实体：个人信息
    /// </summary>
    public class SysPersonalDto
    {
        public SysPersonalDto()
        {
            Menus = new HashSet<SysPersonalMenuDto>();
        }

        /// <summary>
        /// 所属机构id
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// 所属机构名称
        /// </summary>
        public string TenantName { get; set; }

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
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        /// 是否默认用户（管理员）
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 菜单权限
        /// </summary>
        public virtual IEnumerable<SysPersonalMenuDto> Menus { get; set; }
    }
}
