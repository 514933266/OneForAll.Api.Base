using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Host
{
    public interface ITenantProvider
    {
        Guid GetTenantId();
    }
}
