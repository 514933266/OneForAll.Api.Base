namespace Base.Host.Models
{
    /// <summary>
    /// 定时任务运行锁配置
    /// </summary>
    public class JobLockConfig
    {
        /// <summary>
        /// 调度中心并发检查异常时是否放行执行（false：跳过本次执行）
        /// </summary>
        public bool FailOpenOnException { get; set; } = false;
    }
}
