using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Domain.ValueObjects
{
    /// <summary>
    /// 值对象：登录用户权限
    /// </summary>
    public class SysLoginUserPermission
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string SortCode { get; set; }
    }
}
