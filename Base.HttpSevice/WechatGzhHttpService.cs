using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Microsoft.AspNetCore.Http;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService
{
    /// <summary>
    /// 微信公众号
    /// </summary>
    public class WechatGzhHttpService : BaseHttpService, IWechatGzhHttpService
    {
        private readonly HttpServiceConfig _config;

        public WechatGzhHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }
    }
}
