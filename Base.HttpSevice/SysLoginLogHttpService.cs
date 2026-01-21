using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Microsoft.AspNetCore.Http;
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
    /// 登录日志
    /// </summary>
    public class SysLoginLogHttpService : BaseHttpService, ISysLoginLogHttpService
    {
        private readonly HttpServiceConfig _config;

        public SysLoginLogHttpService(
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
        public async Task AddAsync(SysLoginLogRequest form)
        {
            form.UserName = LoginUser.UserName;
            form.CreatorId = LoginUser.Id;
            form.CreatorName = LoginUser.DisplayName;
            form.TenantId = LoginUser.TenantId;
            form.CreateTime = DateTime.UtcNow;
            form.IPAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            var client = GetHttpClient(_config.SysLog);
            if (client != null && client.BaseAddress != null)
            {
                await client.PostAsync("api/SysLoginLogs", form, new JsonMediaTypeFormatter());
            }
        }
    }
}