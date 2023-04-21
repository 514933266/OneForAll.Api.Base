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
    /// 机构（租户）
    /// </summary>
    public class SysTenantRepository : Repository<SysTenant>, ISysTenantRepository
    {
        public SysTenantRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="isEnabled">合作状态（false未合作，true合作中）</param>
        /// <param name="startDate">注册开始时间</param>
        /// <param name="endDate">注册结束时间</param>
        /// <returns>机构列表</returns>
        public async Task<PageList<SysTenant>> GetPageAsync(
            int pageIndex,
            int pageSize,
            string key,
            bool isEnabled,
            DateTime? startDate,
            DateTime? endDate)
        {
            var predicate = PredicateBuilder.Create<SysTenant>(w => true);
            if (isEnabled) predicate = predicate.And(w => w.IsEnabled);
            if (!key.IsNullOrEmpty()) predicate = predicate.And(w => w.Name.Contains(key));
            if (startDate != null) predicate = predicate.And(w => w.CreateTime >= startDate);
            if (endDate != null)
            {
                var date = endDate.Value.AddDays(1);
                predicate = predicate.And(w => w.CreateTime <= date);
            }

            var total = await DbSet
                .CountAsync(predicate);

            var data = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(e => e.CreateTime)
                .ToListAsync();

            return new PageList<SysTenant>(total, pageSize, pageIndex, data);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">机构id</param>
        /// <returns>机构列表</returns>
        public async Task<IEnumerable<SysTenant>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => ids.Contains(w.Id))
                .ToListAsync();
        }

        /// <summary>
        /// 查询机构（含权限、菜单关联）
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构</returns>
        public async Task<SysTenant> GetWithMenusAsync(Guid id)
        {
            return null;
            //return await DbSet
            //    .IgnoreQueryFilters()
            //    .Where(w => w.Id.Equals(id))
            //    .Include(e => e.SysTenantPermContacts)
            //        .ThenInclude(e => e.SysPermission)
            //            .ThenInclude(e => e.SysMenu)
            //    .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public async Task<SysTenant> GetByNameAsync(string name)
        {
            return await DbSet
                .IgnoreQueryFilters()
                .Where(w => w.Name.Equals(name))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="code">机构信用代码</param>
        /// <returns>结果</returns>
        public async Task<SysTenant> GetByCodeAsync(string code)
        {
            return await DbSet
                .IgnoreQueryFilters()
                .Where(w => w.Code.Equals(code))
                .FirstOrDefaultAsync();
        }
    }
}
