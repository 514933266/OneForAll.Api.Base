using Base.HttpService.Interfaces;
using Base.HttpService.Models;
using Microsoft.AspNetCore.Http;
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
    public class WxgzhHttpService : BaseHttpService, IWxgzhHttpService
    {
        private readonly HttpServiceConfig _config;

        public WxgzhHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }
    }
}
