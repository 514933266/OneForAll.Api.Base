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
using Sys.Domain.Aggregates;

namespace Sys.Repository
{
    /// <summary>
    /// 用户
    /// </summary>
    public class SysUserRepository : Repository<SysUser>, ISysUserRepository
    {
        public SysUserRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询机构用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>实体</returns>
        public async Task<SysTenantUserAggr> GetAsync(Guid id)
        {
            var tenantDbSet = Context.Set<SysTenant>();
            var query = (from user in DbSet
                         join tenant in tenantDbSet on user.SysTenantId equals tenant.Id
                         where user.Id == id
                         select new SysTenantUserAggr()
                         {
                             Id = user.Id,
                             Name = user.Name,
                             Status = user.Status,
                             UserName = user.UserName,
                             Password = user.Password,
                             Signature = user.Signature,
                             PwdErrCount = user.PwdErrCount,
                             IconUrl = user.IconUrl,
                             IsDefault = user.IsDefault,
                             UpdateTime = user.UpdateTime,
                             SysTenantId = user.SysTenantId,
                             LastLoginIp = user.LastLoginIp,
                             LastLoginTime = user.LastLoginTime,
                             SysTenant = tenant
                         });

            return await query.IgnoreQueryFilters().FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>实体</returns>
        public async Task<SysUser> GetAsync(string username)
        {
            return await DbSet
                .Where(w => w.UserName == username)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysUser>> GetPageAsync(int pageIndex, int pageSize, string key)
        {
            var predicate = PredicateBuilder.Create<SysUser>(w => true);
            if (!key.IsNullOrEmpty()) predicate = predicate.And(w => w.UserName.Contains(key) || w.Name.Contains(key));

            var total = await DbSet
                .CountAsync(predicate);

            var items = await DbSet
                .Where(predicate)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();

            return new PageList<SysUser>(total, pageSize, pageIndex, items);
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(string key)
        {
            var predicate = PredicateBuilder.Create<SysUser>(w => true);
            if (!key.IsNullOrEmpty())
                predicate = predicate.And(w => w.UserName.Contains(key) || w.Name.Contains(key));

            return await DbSet
                .Where(predicate)
                .ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysUser>> GetListAsync(IEnumerable<Guid> ids)
        {
            return await DbSet.Where(w => ids.Contains(w.Id)).ToListAsync();
        }
    }
}
