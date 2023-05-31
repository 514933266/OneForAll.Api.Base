﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Public.Models
{
    /// <summary>
    /// 授权角色类型
    /// </summary>
    public static class UserRoleType
    {
        /// <summary>
        /// 系统开发者
        /// </summary>
        public const string RULER = "ruler";

        /// <summary>
        /// 系统使用者
        /// </summary>
        public const string ADMIN = "admin";

        /// <summary>
        /// 公共
        /// </summary>
        public const string PUBLIC = "ruler,admin";
    }
}
