namespace Base.Host.Models
{
    /// <summary>
    /// MongoDB 配置选项
    /// </summary>
    public class MongoDbOption
    {
        /// <summary>
        /// 是否启用 MongoDB
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// MongoDB 连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = "mongodb://127.0.0.1";

        /// <summary>
        /// 默认数据库名称
        /// </summary>
        public string DatabaseName { get; set; } = "OneForAll_UserMessage";
    }
}
