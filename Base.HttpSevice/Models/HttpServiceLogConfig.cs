using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.HttpService.Models
{
    /// <summary>
    /// HTTP服务日志配置
    /// </summary>
    public class HttpServiceLogConfig
    {
        /// <summary>
        /// API日志是否启用
        /// </summary>
        public bool ApiLog { get; set; } = false;

        /// <summary>
        /// 操作日志是否启用
        /// </summary>
        public bool OperationLog { get; set; } = false;

        /// <summary>
        /// 异常日志是否启用
        /// </summary>
        public bool ExceptionLog { get; set; } = false;

        /// <summary>
        /// 全局异常日志是否启用
        /// </summary>
        public bool GlobalExceptionLog { get; set; } = false;

        /// <summary>
        /// 登录日志是否启用
        /// </summary>
        public bool LoginLog { get; set; } = false;
    }
}
