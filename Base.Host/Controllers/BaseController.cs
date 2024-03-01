using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core.Extension;
using OneForAll.Core.OAuth;

namespace Base.Host.Controllers
{
    public class BaseController : Controller
    {
        protected LoginUser LoginUser
        {
            get
            {
                var claims = HttpContext?.User.Claims;
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
    }
}