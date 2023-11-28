using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 多租户用户关联
    /// </summary>
    public class SysTenantUserContactRepository : Repository<SysTenantUserContact>, ISysTenantUserContactRepository
    {
        public SysTenantUserContactRepository(DbContext context)
            : base(context)
        {

        }
    }
}
