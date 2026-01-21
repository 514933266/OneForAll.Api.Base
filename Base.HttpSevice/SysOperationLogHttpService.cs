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
    /// 操作日志
    /// </summary>
    public class SysOperationLogHttpService : BaseHttpService, ISysOperationLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysOperationLogHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysOperationLogRequest form)
        {
            form.CreateTime = DateTime.UtcNow;

            var client = GetHttpClient(_config.SysLog);
            if (client != null && client.BaseAddress != null)
            {
                await client.PostAsync("api/SysOperationLogs", form, new JsonMediaTypeFormatter());
            }
        }
    }
}
