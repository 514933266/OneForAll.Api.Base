using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Repository
{
    public class SysArticleTypeRepository : Repository<SysArticleType>, ISysArticleTypeRepository
    {
        public SysArticleTypeRepository(DbContext context)
            : base(context)
        {

        }
    }
}
