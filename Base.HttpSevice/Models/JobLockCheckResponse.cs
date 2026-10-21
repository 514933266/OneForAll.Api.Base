namespace Base.HttpService.Models
{
    /// <summary>
    /// 定时任务运行锁检查结果
    /// </summary>
    public class JobLockCheckResponse
    {
        /// <summary>
        /// 是否允许执行（false：已达最大并发数，本次跳过）
        /// </summary>
        public bool CanExecute { get; set; }

        /// <summary>
        /// 是否已真正持有锁（true：执行完需调用释放接口）
        /// </summary>
        public bool LockAcquired { get; set; }

        /// <summary>
        /// 当前锁版本号（释放时回传）
        /// </summary>
        public int Version { get; set; }
    }
}
