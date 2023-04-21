using Sys.Domain.AggregateRoots;
using Sys.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Repository
{
    public class SysArticleTypeRepository : Repository<SysArticleType>, ISysArticleTypeRepository
    {
        public SysArticleTypeRepository(DbContext context)
            : base(context)
        {

        }
    }
}
