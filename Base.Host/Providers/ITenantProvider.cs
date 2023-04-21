using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Host
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}
