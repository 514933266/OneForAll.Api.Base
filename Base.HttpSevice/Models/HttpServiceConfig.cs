using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.HttpService.Models
{
    /// <summary>
    /// 数据资源服务配置
    /// </summary>
    public class HttpServiceConfig
    {
        /// <summary>
        /// Api日志
        /// </summary>
        public string SysApiLog { get; set; } = "SysApiLog";

        /// <summary>
        /// 异常日志
        /// </summary>
        public string SysExceptionLog { get; set; } = "SysExceptionLog";

        /// <summary>
        /// 全局异常日志
        /// </summary>
        public string SysGlobalExceptionLog { get; set; } = "SysGlobalExceptionLog";

        /// <summary>
        /// 操作日志
        /// </summary>
        public string SysOperationLog { get; set; } = "SysOperationLog";

        /// <summary>
        /// 登录日志
        /// </summary>
        public string SysLoginLog { get; set; } = "SysLoginLog";
    }
}
