using Autofac.Core;
using Base.Host.Models;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Microsoft.Extensions.Hosting;
using OneForAll.Core.Extension;
using Quartz;
using Quartz.Spi;
using System;
using System.Reflection.Metadata;
using System.Threading;
using System.Threading.Tasks;

namespace Base.Host.Providers
{
    /// <summary>
    /// 定时任务启动服务
    /// </summary>
    public class QuartzJobHostedService : IHostedService
    {
        private readonly AuthConfig _authConfig;
        private readonly QuartzScheduleJobConfig _config;

        private readonly IJobFactory _jobFactory;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IScheduleJobHttpService _jobHttpService;
        private readonly ISysGlobalExceptionLogHttpService _globalExceptionHttpService;

        public IScheduler Scheduler { get; private set; }
        public QuartzJobHostedService(
            AuthConfig authConfig,
            QuartzScheduleJobConfig config,
            IJobFactory jobFactory,
            ISchedulerFactory schedulerFactory,
            IScheduleJobHttpService jobHttpService,
            ISysGlobalExceptionLogHttpService globalExceptionHttpService)
        {
            _authConfig = authConfig;
            _config = config;
            _jobFactory = jobFactory;
            _schedulerFactory = schedulerFactory;
            _jobHttpService = jobHttpService;
            _globalExceptionHttpService = globalExceptionHttpService;
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            Scheduler.JobFactory = _jobFactory;

            foreach (var jobSchedule in _config.ScheduleJobs)
            {
                if (jobSchedule.JobType == null)
                    continue;

                try
                {
                    // 尝试向调度中心注册
                    await _jobHttpService.RegisterAsync(new HttpService.Models.JobRegisterRequest()
                    {
                        AppId = _config.AppId,
                        AppSecret = _config.AppSecret,
                        GroupName = _config.GroupName,
                        NodeName = _config.NodeName,
                        Cron = jobSchedule.Cron,
                        Name = jobSchedule.TypeName,
                        Remark = jobSchedule.Remark
                    });
                }
                catch (Exception ex)
                {
                    // 防御性处理：注册过程异常也不应阻断本地任务
                    await _globalExceptionHttpService.AddAsync(new SysGlobalExceptionLogRequest()
                    {
                        ModuleName = _authConfig.ClientName,
                        ModuleCode = _authConfig.ClientCode,
                        Name = "定时任务注册异常",
                        Content = ex.Message ?? "无堆栈信息"
                    });
                }

                // 无论注册是否成功，都创建并调度本地 Quartz 任务
                var job = CreateJob(jobSchedule);
                var trigger = CreateTrigger(jobSchedule);
                await Scheduler.ScheduleJob(job, trigger, cancellationToken);
            }

            await Scheduler.Start(cancellationToken);
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
            foreach (var jobSchedule in _config.ScheduleJobs)
            {
                await _jobHttpService.DownLineAsync(_config.AppId, jobSchedule.TypeName);
            }
        }

        // 创建任务
        private IJobDetail CreateJob(QuartzScheduleJob schedule)
        {
            return JobBuilder
                .Create(schedule.JobType)
                .WithIdentity(schedule.JobType.FullName)
                .WithDescription(schedule.JobType.Name)
                .UsingJobData("Data", schedule.Data)
                .Build();
        }

        // 创建触发器
        private ITrigger CreateTrigger(QuartzScheduleJob schedule)
        {
            return TriggerBuilder
                .Create()
                .WithIdentity($"{schedule.JobType.FullName}.Trigger")
                .WithCronSchedule(schedule.Cron)
                .WithDescription($"{schedule.JobType.Name}.Trigger")
                .Build();
        }
    }
}