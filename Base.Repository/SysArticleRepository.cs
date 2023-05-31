using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    public class SysArticleRepository : Repository<SysArticle>, ISysArticleRepository
    {
        public SysArticleRepository(DbContext context)
            : base(context)
        {
        }

        public async Task<PageList<SysArticle>> GetPagePublishedAsync(int pageIndex, int pageSize)
        {
            var predicate = PredicateBuilder.Create<SysArticle>(w => w.IsPublish);

            var total = await DbSet.CountAsync(predicate);
            var data = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(e => e.CreateTime)
                .Include(p => p.SysArticleType)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<SysArticle>(total, pageSize, pageIndex, data);
        }

        public async Task<PageList<SysArticle>> GetPageAsync(IEnumerable<Guid> typeIds, int pageIndex, int pageSize, string key)
        {
            var predicate = PredicateBuilder.Create<SysArticle>(w => true);
            if (!key.IsNullOrEmpty()) predicate = predicate.And(w => w.Title.Contains(key));
            if (typeIds.Any()) predicate = predicate.And(w => typeIds.Contains(w.SysArticleTypeId));

            var total = await DbSet.CountAsync(predicate);
            var data = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .OrderByDescending(e => e.CreateTime)
                .Include(p => p.SysArticleType)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<SysArticle>(total, pageSize, pageIndex, data);
        }
    }
}
