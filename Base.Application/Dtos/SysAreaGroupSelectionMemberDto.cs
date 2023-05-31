﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.Dtos
{
    /// <summary>
    /// 地区组成员选择
    /// </summary>
    public class SysAreaGroupSelectionMemberDto
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
        /// 头像地址
        /// </summary>
        public string IconUrl { get; set; }
    }
}
