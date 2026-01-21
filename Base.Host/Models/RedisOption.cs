namespace Base.Host.Models
{
    /// <summary>
    /// 缓存配置
    /// </summary>
    public class RedisOption
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 实例名称
        /// </summary>
        public string InstanceName { get; set; } = "Base:";

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379";

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
