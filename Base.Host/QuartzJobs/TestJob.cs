using Quartz;
using System.Threading.Tasks;

namespace Base.Host.QuartzJobs
{
    /// <summary>
    /// 测试用任务
    /// </summary>
    public class TestJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {

        }
    }
}
