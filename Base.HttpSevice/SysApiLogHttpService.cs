using Microsoft.AspNetCore.Http;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Base.Public.Models;
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
    /// Api日志
    /// </summary>
    public class SysApiLogHttpService : BaseHttpService, ISysApiLogHttpService
    {
        private readonly AuthConfig _authConfig;
        private readonly HttpServiceConfig _config;
        private readonly HttpServiceLogConfig _logConfig;

        public SysApiLogHttpService(
            AuthConfig authConfig,
            HttpServiceConfig config,
            HttpServiceLogConfig logConfig,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _authConfig = authConfig;
            _config = config;
            _logConfig = logConfig;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns></returns>
        public async Task AddAsync(SysApiLogRequest form)
        {
            // 检查是否启用日志
            if (!_logConfig.ApiLog)
                return;

            try
            {
                form.CreateTime = DateTime.UtcNow;
                form.ModuleCode = _authConfig.ClientCode;
                form.ModuleName = _authConfig.ClientName;

                var client = GetHttpClient(_config.SysLog);
                if (client != null && client.BaseAddress != null)
                {
                    await client.PostAsync("api/SysApiLogs", form, new JsonMediaTypeFormatter());
                }
            }
            catch
            {
                // 忽略异常
            }
        }
    }
}