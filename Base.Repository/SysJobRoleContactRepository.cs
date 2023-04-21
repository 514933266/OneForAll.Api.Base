using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Repository
{
    /// <summary>
    /// 系统岗位角色关联仓储
    /// </summary>
    public class SysJobRoleContactRepository : Repository<SysJobRoleContact>, ISysJobRoleContactRepository
    {
        public SysJobRoleContactRepository(DbContext context)
            : base(context)
        {

        }
    }
}
