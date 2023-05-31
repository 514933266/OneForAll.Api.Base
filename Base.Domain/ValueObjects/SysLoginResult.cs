using Base.Domain.AggregateRoots;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.ValueObjects
{
    public class SysLoginResult
    {
        public BaseErrType ErrType { get; set; }
        public SysUser User { get; set; }

        /// <summary>
        /// 剩余密码可错误次数
        /// </summary>
        public int LessPwdErrCount { get; set; }

        /// <summary>
        /// 剩余冻结时间
        /// </summary>
        public double LessBanTime { get; set; }
    }
}
