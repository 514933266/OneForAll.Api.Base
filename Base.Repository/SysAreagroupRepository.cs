using Base.Domain;
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
    /// <summary>
    /// 仓储：地区组
    /// </summary>
    public class SysAreaGroupRepository : Repository<SysAreaGroup>, ISysAreaGroupRepository
    {

        public SysAreaGroupRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询分页列表（含关联表信息）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>结果</returns>
        public async Task<PageList<SysAreaGroup>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var predicate = PredicateBuilder.Create<SysAreaGroup>(w => true);
            if (!key.IsNullOrEmpty()) predicate = predicate.And(w => w.Name.Contains(key));

            var total = await DbSet
                .CountAsync(predicate);

            var data = await DbSet
                .AsNoTracking()
                .Where(predicate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<SysAreaGroup>(total, pageSize, pageIndex, data);
        }

        /// <summary>
        /// 查询地区组
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>地区组</returns>
        public async Task<IEnumerable<SysAreaGroup>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet
                .Where(w => ids.Contains(w.Id))
                .ToListAsync();
        }

        /// <summary>
        /// 查询地区组（含成员）
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <returns>地区组</returns>
        public async Task<SysAreaGroup> GetWithMembersAsync(Guid id)
        {
            return await DbSet
                .Where(w => w.Id.Equals(id))
                .Include(e => e.SysAreaGroupUserContacts)
                    .ThenInclude(e => e.SysUser)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询地区组
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>地区组</returns>
        public async Task<SysAreaGroup> GetByNameAsync(string name)
        {
            return await DbSet
                .Where(w => w.Name.Equals(name))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询地区组（含地区）
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <returns>地区组</returns>
        public async Task<SysAreaGroup> GetWithAreaContactsAsync(Guid id)
        {
            return await DbSet
                .Where(w => w.Id.Equals(id))
                .Include(e => e.SysAreaGroupContacts)
                .FirstOrDefaultAsync();
        }
    }
}
