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
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SysRoleRepository : Repository<SysRole>, ISysRoleRepository
    {
        public SysRoleRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>结果</returns>
        public async Task<PageList<SysRole>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var predicate = PredicateBuilder.Create<SysRole>(w => true);
            if (!key.IsNullOrEmpty()) predicate = predicate.And(w => w.Name.Contains(key));

            var total = await DbSet.CountAsync(predicate);

            var data = await DbSet
                .Where(predicate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PageList<SysRole>(total, pageSize, pageIndex, data);
        }

        /// <summary>
        /// 查询实体（含成员）
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        public async Task<SysRole> GetWithMembersAsync(Guid id)
        {
            return null;
            //return await DbSet
            //    .Where(w => w.Id.Equals(id))
            //    .Include(e => e.SysRoleUserContacts)
            //        .ThenInclude(e => e.SysUser)
            //    .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        public async Task<SysRole> GetByNameAsync(string name)
        {
            return await DbSet
                .Where(w => w.Name.Equals(name))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询实体（含权限）
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        public async Task<SysRole> GetWithPermsAsync(Guid id)
        {
            return null;
            //return await DbSet
            //    .Where(w => w.Id.Equals(id))
            //    .Include(e => e.SysRolePermContacts)
            //        .ThenInclude(e => e.SysPermission)
            //    .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysRole>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet
                .Where(w => ids.Contains(w.Id))
                .ToListAsync();
        }
    }
}
