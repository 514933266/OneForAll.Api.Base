using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sys.Host.Model
{
    /// <summary>
    /// 跨域配置模型
    /// </summary>
    public class CorsConfig
    {
        /// <summary>
        /// 域名集合
        /// </summary>
        public string[] Origins { get; set; }
    }
}
