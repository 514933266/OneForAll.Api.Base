using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Host.Models
{
    /// <summary>
    /// OAuth授权配置
    /// </summary>
    public class AuthConfig
    {
        /// <summary>
        /// 客户端id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密码
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 客户端代码
        /// </summary>
        public string ClientCode { get; set; }

        /// <summary>
        /// 授权验证的域名，如果不配置则默认使用请求的Host，用于局域网验证
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Jwt加密密钥（部署生产时务必重新随机生成32位随机字符串作为新密钥）
        /// </summary>
        public string JwtKey { get; set; }
    }
}
