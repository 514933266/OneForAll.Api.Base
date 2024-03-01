using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.Extension;
using Base.Public.Models;
using OneForAll.Core.OAuth;

namespace Base.HttpService
{
    /// <summary>
    /// Http基类
    /// </summary>
    public class BaseHttpService
    {
        private readonly string AUTH_KEY = "Authorization";
        protected readonly IHttpContextAccessor _httpContext;
        protected readonly IHttpClientFactory _httpClientFactory;
        public BaseHttpService(
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory)
        {
            _httpContext = httpContext;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 登录token
        /// </summary>
        protected string Token
        {
            get
            {
                var context = _httpContext.HttpContext;
                if (context != null)
                {
                    return context.Request.Headers
                      .FirstOrDefault(w => w.Key.Equals(AUTH_KEY))
                      .Value.TryString();
                }
                return "";
            }
        }

        protected LoginUser LoginUser
        {
            get
            {
                var claims = _httpContext.HttpContext?.User.Claims;
                if (claims.Any())
                {
                    return new LoginUser()
                    {
                        Name = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME)?.Value ?? "",
                        UserName = claims.FirstOrDefault(e => e.Type == UserClaimType.USERNAME)?.Value ?? "",
                        WxAppId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_APPID)?.Value ?? "",
                        WxOpenId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_OPENID)?.Value ?? "",
                        WxUnionId = claims.FirstOrDefault(e => e.Type == UserClaimType.WX_UNIONID)?.Value ?? "",
                        Id = claims.FirstOrDefault(e => e.Type == UserClaimType.USER_ID).Value.TryGuid(),
                        SysTenantId = claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID).Value.TryGuid(),
                        IsDefault = claims.FirstOrDefault(e => e.Type == UserClaimType.IS_DEFAULT).Value.TryBoolean()
                    };
                }
                return new LoginUser();
            }
        }

        /// <summary>
        /// 获取HttpClient
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected HttpClient GetHttpClient(string name)
        {
            if (!Token.IsNullOrEmpty())
            {
                var client = _httpClientFactory.CreateClient(name);
                client.DefaultRequestHeaders.Add(AUTH_KEY, Token);
                return client;
            }
            return null;
        }
    }
}
