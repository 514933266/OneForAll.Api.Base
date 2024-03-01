using Base.Host.Models;
using Base.Public.Models;
using Microsoft.AspNetCore.Http;
using OneForAll.Core.OAuth;
using System;
using System.Linq;

namespace Base.Host
{
    public class TenantProvider : ITenantProvider
    {
        private IHttpContextAccessor _context;

        public TenantProvider(IHttpContextAccessor context)
        {
            _context = context;
        }

        public Guid GetTenantId()
        {
            var tenantId = _context.HttpContext?.User.Claims.FirstOrDefault(e => e.Type == UserClaimType.TENANT_ID);
            if (tenantId != null)
            {
                return new Guid(tenantId.Value);
            }
            else
            {
                return Guid.Empty; ;
            }
        }
    }
}
