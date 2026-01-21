using System;

namespace Base.Host.Models
{
    /// <summary>
    /// 定时任务
    /// </summary>
    public class QuartzScheduleJob
    {
        /// <summary>
        /// 定时任务类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public Type JobType { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 传递参数
        /// </summary>
        public string Data { get; set; }
    }
}

