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
        /// <returns结果</returns>
        public async Task<BaseErrType> LogAsync(string appId, string taskName, string log)
        {
            var client = GetHttpClient(_config.SysJob);
            if (client != null && client.BaseAddress != null)
            {
                var response = await client.PostAsync($"api/ScheduleJobs/{appId}/{taskName}/Logs", log, new JsonMediaTypeFormatter());
                var msg = await response.Content.ReadAsAsync<BaseMessage>();
                return msg.ErrType;
            }
            return BaseErrType.Fail;
        }
    }
}