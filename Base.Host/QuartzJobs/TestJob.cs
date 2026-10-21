using Quartz;
using System.Threading.Tasks;
using Base.Host.Models;
using Base.HttpService.Interfaces;
using Base.Public.Models;

namespace Base.Host.QuartzJobs
{
    /// <summary>
    /// 测试用任务
    /// </summary>
    public class TestJob : BaseLockJob
    {
        public TestJob(
            QuartzScheduleJobConfig jobConfig,
            AuthConfig authConfig,
            JobLockConfig lockConfig,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
            : base(jobConfig, authConfig, lockConfig, jobHttpService, logHttpService)
        {
        }

        /// <summary>
        /// 执行任务（测试任务暂无业务逻辑）
        /// </summary>
        protected override Task ExecuteInternalAsync(IJobExecutionContext context)
        {
            return Task.CompletedTask;
        }
    }
}
