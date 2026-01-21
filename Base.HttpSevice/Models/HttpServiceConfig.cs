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
        /// 系统日志服务
        /// </summary>
        public string SysLog { get; set; } = "SysLog";

        /// <summary>
        /// 消息服务
        /// </summary>
        public string SysUms { get; set; } = "SysUms";

        /// <summary>
        /// 定时任务调度服务
        /// </summary>
        public string SysJob { get; set; } = "SysJob";

        /// <summary>
        /// OA服务
        /// </summary>
        public string OA { get; set; } = "OA";

    }
}
