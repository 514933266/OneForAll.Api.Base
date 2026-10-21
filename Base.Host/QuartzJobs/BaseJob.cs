using System;
using System.Threading.Tasks;
using Quartz;
using Base.Host.Models;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Base.Public.Models;

namespace Base.Host.QuartzJobs
{
    /// <summary>
    /// 定时任务业务基类 - 提供日志、异常上报等通用能力
    /// </summary>
    public abstract class BaseJob : IJob
    {
        protected readonly QuartzScheduleJobConfig _jobConfig;
        protected readonly AuthConfig _authConfig;
        protected readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _logHttpService;

        protected BaseJob(
            QuartzScheduleJobConfig jobConfig,
            AuthConfig authConfig,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
        {
            _jobConfig = jobConfig;
            _authConfig = authConfig;
            _jobHttpService = jobHttpService;
            _logHttpService = logHttpService;
        }

        /// <summary>
        /// Quartz 执行入口
        /// </summary>
        public virtual async Task Execute(IJobExecutionContext context)
        {
            try
            {
                // 执行实际业务逻辑
                await ExecuteInternalAsync(context);
            }
            catch (Exception ex)
            {
                // 记录异常（调度中心异常日志 + 全局异常上报）
                await AddLogAsync($"任务 [{GetTaskName()}] 执行异常: {ex.Message}\r\n{ex.StackTrace}", true);
                await SendGlobalExceptionAsync(ex);
            }
        }

        /// <summary>
        /// 子类必须实现的实际执行逻辑
        /// </summary>
        protected abstract Task ExecuteInternalAsync(IJobExecutionContext context);

        /// <summary>
        /// 获取任务名称（默认为类名）
        /// </summary>
        protected virtual string GetTaskName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// 记录运行日志（上报调度中心，失败时输出控制台，不阻断任务流程）
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="isException">是否为异常日志</param>
        protected async Task AddLogAsync(string log, bool isException = false)
        {
            try
            {
                await _jobHttpService.LogAsync(_jobConfig?.AppId, GetTaskName(), log, isException);
            }
            catch
            {
                Console.WriteLine($"[{GetTaskName()}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} {log}");
            }
        }

        /// <summary>
        /// 发送全局异常（上报失败不再抛出）
        /// </summary>
        protected async Task SendGlobalExceptionAsync(Exception ex)
        {
            try
            {
                await _logHttpService.AddAsync(new SysGlobalExceptionLogRequest
                {
                    ModuleName = _authConfig?.ClientName,
                    ModuleCode = _authConfig?.ClientCode,
                    Name = ex.Message,
                    Content = ex.InnerException == null ? ex.StackTrace : ex.InnerException.StackTrace
                });
            }
            catch
            {
                // 异常上报失败不再抛出
            }
        }
    }
}
