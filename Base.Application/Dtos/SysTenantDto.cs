using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Application.Dtos
{
    /// <summary>
    /// 实体：机构（租户）
    /// </summary>
    public class SysTenantDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 机构代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 负责人
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 状态（0待审核 1已开通）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否默认（默认机构禁止删除）
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 是否启用（未启用机构用户禁止登录）
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
