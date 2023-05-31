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
    public class SysArticleRecordRepository : Repository<SysArticleRecord>, ISysArticleRecordRepository
    {
        public SysArticleRecordRepository(DbContext context)
            : base(context)
        {
        }

        public async Task<SysArticleRecord> GetAsync(Guid userId, Guid articleId, DateTime datetime)
        {
            return await DbSet
                .Where(w => w.SysUserId.Equals(userId) &&
                            w.SysArticleId.Equals(articleId) &&
                            w.CreateTime.Year.Equals(datetime.Year)&&
                            w.CreateTime.Month.Equals(datetime.Month)&&
                            w.CreateTime.Day.Equals(datetime.Day))
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SysArticleRecord>> GetListAsync(Guid userId, IEnumerable<Guid> articleIds)
        {
            return await DbSet
                .Where(w => w.SysUserId.Equals(userId) && articleIds.Contains(w.SysArticleId))
                .ToListAsync();
        }
    }
}
