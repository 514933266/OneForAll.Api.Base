using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Base.Host.Models
{
    /// <summary>
    /// Quartz配置
    /// </summary>
    public class QuartzScheduleJobConfig
    {
        public QuartzScheduleJobConfig()
        {
            ScheduleJobs = new List<QuartzScheduleJob>();
        }

        /// <summary>
        /// 模式（Center:任务调度中心 Job:定时任务）
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// 应用程序id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用程序密钥
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 所属组
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 运行节点名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string NodeName { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Required]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 定时任务列表
        /// </summary>
        public List<QuartzScheduleJob> ScheduleJobs { get; set; }
    }
}
