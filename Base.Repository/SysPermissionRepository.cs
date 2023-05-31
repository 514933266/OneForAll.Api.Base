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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;

namespace Base.Repository
{
    /// <summary>
    /// 系统权限仓储
    /// </summary>
    public class SysPermissionRepository : Repository<SysPermission>, ISysPermissionRepository
    {
        public SysPermissionRepository(DbContext context)
            : base(context)
        {
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysPermissionAggr>> GetListAsync(IEnumerable<Guid> ids)
        {
            var menuDbSet = Context.Set<SysMenu>();
            var data = (from perm in DbSet
                        join menu in menuDbSet on perm.SysMenuId equals menu.Id
                        where ids.Contains(perm.Id)
                        select new SysPermissionAggr()
                        {
                            Id = perm.Id,
                            SysMenuId = perm.SysMenuId,
                            Code = perm.Code,
                            Name = perm.Name,
                            SortCode = perm.SortCode,
                            Remark = perm.Remark,
                            SysMenu = menu
                        });

            return await data.ToListAsync();
        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="menuIds">菜单id</param>
        /// <returns>结果</returns>
        public async Task<IEnumerable<SysPermission>> GetListByMenuAsync(IEnumerable<Guid> menuIds)
        {
            return await DbSet.Where(w => menuIds.Contains(w.SysMenuId)).ToListAsync();
        }
    }
}
