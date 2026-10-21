using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService
{
    /// <summary>
    /// 定时任务调度中心
    /// </summary>
    public class ScheduleJobHttpService : BaseHttpService, IScheduleJobHttpService
    {
        private readonly HttpServiceConfig _config;

        public ScheduleJobHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        public async Task<BaseErrType> RegisterAsync(JobRegisterRequest request)
        {
            var client = GetHttpClient(_config.SysJob);
            if (client != null && client.BaseAddress != null)
            {
                var response = await client.PostAsync($"api/ScheduleJobs", request, new JsonMediaTypeFormatter());
                var msg = await response.Content.ReadAsAsync<BaseMessage>();
                return msg.ErrType;
            }
            return BaseErrType.Fail;
        }

        /// <summary>
        /// 定时服务下线
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <returns结果</returns>
        public async Task DownLineAsync(string appId, string taskName)
        {
            var client = GetHttpClient(_config.SysJob);
            if (client != null && client.BaseAddress != null)
            {
                await client.DeleteAsync($"api/ScheduleJobs/{appId}/{taskName}");
            }
        }

        /// <summary>
        /// 定时服务运行日志
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="log">日志内容</param>
        /// <param name="isException">是否异常日志</param>
        /// <returns结果</returns>
        public async Task<BaseErrType> LogAsync(string appId, string taskName, string log, bool isException = false)
        {
            var client = GetHttpClient(_config.SysJob);
            if (client != null && client.BaseAddress != null)
            {
                var url = $"api/ScheduleJobs/{appId}/{taskName}/Logs?isException={isException}";
                var response = await client.PostAsync(url, log, new JsonMediaTypeFormatter());
                var msg = await response.Content.ReadAsAsync<BaseMessage>();
                return msg.ErrType;
            }
            return BaseErrType.Fail;
        }

        /// <summary>
        /// 尝试获取任务执行资格（调度中心统一并发控制，返回成功后执行）
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="clientCode">客户端编号</param>
        /// <returns>检查结果（调度中心不可达或接口报错时抛出异常）</returns>
        public async Task<JobLockCheckResponse> TryAcquireLockAsync(string appId, string taskName, string clientCode)
        {
            var client = GetHttpClient(_config.SysJob);
            if (client == null || client.BaseAddress == null)
                throw new Exception("调度中心（ScheduleJob）服务地址未配置");

            var url = $"api/ScheduleJobs/{appId}/{taskName}/LockChecks?clientCode={clientCode}";
            var response = await client.PostAsync(url, null);
            var msg = await response.Content.ReadAsAsync<BaseMessage<JobLockCheckResponse>>();
            return msg?.Data;
        }

        /// <summary>
        /// 释放任务执行资格
        /// </summary>
        /// <param name="appId">应用程序id</param>
        /// <param name="taskName">定时任务名称</param>
        /// <param name="version">锁版本号</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReleaseLockAsync(string appId, string taskName, int version)
        {
            var client = GetHttpClient(_config.SysJob);
            if (client != null && client.BaseAddress != null)
            {
                var url = $"api/ScheduleJobs/{appId}/{taskName}/LockChecks/{version}";
                var response = await client.DeleteAsync(url);
                var msg = await response.Content.ReadAsAsync<BaseMessage>();
                return msg.ErrType;
            }
            return BaseErrType.Fail;
        }
    }
}