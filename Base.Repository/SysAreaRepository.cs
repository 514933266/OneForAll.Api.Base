using Sys.Domain;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Repositorys;
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

namespace Sys.Repository
{
    /// <summary>
    /// 地区
    /// </summary>
    public class SysAreaRepository : Repository<SysArea>, ISysAreaRepository
    {
        public SysAreaRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询子级
        /// </summary>
        /// <param name="parentId">父级id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysArea>> GetChildrenAsync(int parentId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => w.ParentId == parentId)
                .ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="codes">代码集合</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysArea>> GetListAsync(IEnumerable<string> codes)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => codes.Contains(w.Code))
                .ToListAsync();
        }
    }
}
