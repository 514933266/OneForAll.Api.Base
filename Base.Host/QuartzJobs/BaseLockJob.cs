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
    /// 定时任务分布式锁基类 - 在 BaseJob 基础上向调度中心（ScheduleJob）请求并发检查，返回成功后再执行
    /// </summary>
    public abstract class BaseLockJob : BaseJob
    {
        private readonly JobLockConfig _lockConfig;

        protected BaseLockJob(
            QuartzScheduleJobConfig jobConfig,
            AuthConfig authConfig,
            JobLockConfig lockConfig,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService logHttpService)
            : base(jobConfig, authConfig, jobHttpService, logHttpService)
        {
            _lockConfig = lockConfig;
        }

        /// <summary>
        /// Quartz 执行入口（叠加调度中心并发控制）
        /// </summary>
        public override async Task Execute(IJobExecutionContext context)
        {
            var taskName = GetTaskName();

            // 请求调度中心检查执行资格
            JobLockCheckResponse check = null;
            Exception checkEx = null;
            try
            {
                check = await _jobHttpService.TryAcquireLockAsync(_jobConfig?.AppId, taskName, _authConfig?.ClientCode);
            }
            catch (Exception ex)
            {
                checkEx = ex;
            }

            // 并发检查异常或返回为空：按配置决定放行或跳过
            if (check == null)
            {
                checkEx = checkEx ?? new Exception("调度中心并发检查返回为空");

                if (_lockConfig?.FailOpenOnException == true)
                {
                    // 放行执行（本次不参与并发控制）
                    await AddLogAsync($"任务 [{taskName}] 调度中心并发检查异常，已放行执行（本次未参与并发控制）: {checkEx.Message}", true);
                    await base.Execute(context);
                }
                else
                {
                    await AddLogAsync($"任务 [{taskName}] 调度中心并发检查异常，本次执行被跳过: {checkEx.Message}", true);
                }
                return;
            }

            // 已达到最大并发数，跳过执行
            if (!check.CanExecute)
            {
                await LogSkipExecutionAsync(taskName);
                return;
            }

            try
            {
                // 执行实际业务逻辑（异常捕获由基类 BaseJob 处理）
                await base.Execute(context);
            }
            finally
            {
                // 仅当本次真正持有锁时才需要释放（未启用并发控制或检查异常放行时未参与并发控制）
                if (check.LockAcquired)
                {
                    // 延迟3秒后再释放锁，防止任务刚执行完立即再次触发
                    await Task.Delay(3000);
                    await ReleaseLockAsync(taskName, check.Version);
                }
            }
        }

        /// <summary>
        /// 记录跳过执行的日志
        /// </summary>
        protected virtual async Task LogSkipExecutionAsync(string taskName)
        {
            await AddLogAsync($"任务 [{taskName}] 已达到最大并发数，本次执行被跳过");
        }

        /// <summary>
        /// 释放执行资格（失败重试3次后上报全局异常，不阻断任务流程）
        /// </summary>
        private async Task ReleaseLockAsync(string taskName, int version)
        {
            var retry = 0;
            Exception lastEx = null;
            while (retry < 3)
            {
                retry++;
                try
                {
                    await _jobHttpService.ReleaseLockAsync(_jobConfig?.AppId, taskName, version);
                    return;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                    await Task.Delay(1000);
                }
            }

            // 释放失败：锁计数无法回退，需人工检查调度中心锁记录
            await SendGlobalExceptionAsync(lastEx);
        }
    }
}
