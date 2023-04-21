using System;
using System.Linq;
using Sys.Domain.Aggregates;
using Sys.Domain.Models;
using Sys.Host.Models;
using Sys.Public.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OneForAll.Core.Extension;

namespace Sys.Host.Controllers
{
    public class BaseController : Controller
    {
        protected Guid UserId
        {
            get
            {
                var userId = HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_ID);

                if (userId != null)
                {
                    return new Guid(userId.Value);
                }
                return Guid.Empty;
            }
        }

        protected string UserName
        {
            get
            {
                var username = HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USERNAME);

                if (username != null)
                {
                    return username.Value;
                }
                return null;
            }
        }

        protected Guid TenantId
        {
            get
            {
                var tenantId = HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);

                if (tenantId != null)
                {
                    return new Guid(tenantId.Value);
                }
                return Guid.Empty;
            }
        }

        protected SysLoginUserAggr LoginUser
        {
            get
            {
                var name = HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.USER_NICKNAME);

                var role = HttpContext
                .User
                .Claims
                .FirstOrDefault(e => e.Type == UserClaimType.ROLE);

                return new SysLoginUserAggr()
                {
                    Id = UserId,
                    Name = name?.Value,
                    IsDefault = role.Value.Equals(UserRoleType.RULER)
                };
            }
        }
    }
}