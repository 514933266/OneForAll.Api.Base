using Base.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 多租户用户关联表
    /// </summary>
    public interface ISysTenantUserContactRepository : IEFCoreRepository<SysTenantUserContact>
    {
    }
}
