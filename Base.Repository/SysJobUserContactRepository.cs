using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Repository
{
    public class SysJobUserContactRepository : Repository<SysJobUserContact>, ISysJobUserContactRepository
    {
        public SysJobUserContactRepository(DbContext context)
            : base(context)
        {

        }
    }
}
