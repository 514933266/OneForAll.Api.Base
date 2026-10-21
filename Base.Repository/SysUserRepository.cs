using Base.Domain.Entities;
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
using Base.Domain.Aggregates;

namespace Base.Repository
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

        #region 全局

        /// <summary>
        /// 查询指定用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>实体</returns>
        public async Task<SysUser> GetByIdIQFAsync(Guid id)
        {
            return await DbSet.IgnoreQueryFilters().FirstOrDefaultAsync(w => w.Id == id);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>实体</returns>
        public async Task<SysUser> GetByNameIQFAsync(string username)
        {
            return await DbSet
                .Where(w => w.UserName == username)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>实体</returns>
        public async Task<SysUser> GetByMobileIQFAsync(string mobile)
        {
            return await DbSet
                .Where(w => w.Mobile == mobile)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync();
        }

        #endregion

        #region 当前机构

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

            return new PageList<SysUser>(total, pageIndex, pageSize, items);
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

        #endregion
    }
}
