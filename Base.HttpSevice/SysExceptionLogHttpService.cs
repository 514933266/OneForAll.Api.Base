using Microsoft.AspNetCore.Http;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OneForAll.Core;

namespace Base.HttpService
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public class SysExceptionLogHttpService : BaseHttpService, ISysExceptionLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysExceptionLogHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysExceptionLogRequest entity)
        {
            var client = GetHttpClient(_config.SysLog);
            if (client != null && client.BaseAddress != null)
            {
                await client.PostAsync("api/SysExceptionLogs", entity, new JsonMediaTypeFormatter());
            }
        }
    }
}
