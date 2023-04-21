using Sys.Domain;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.Core;
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
    /// 系统菜单
    /// </summary>
    public class SysMenuRepository : Repository<SysMenu>, ISysMenuRepository
    {
        public SysMenuRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询机构菜单权限列表
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysMenu>> GetListByTenantAsync(Guid tenantId)
        {
            var tenantDbSet = Context.Set<SysTenant>().Where(w => w.Id == tenantId);
            var tenantPermDbSet = Context.Set<SysTenantPermContact>();
            var permDbSet = Context.Set<SysPermission>();

            var data = (from tenant in tenantDbSet
                        join tenantPerm in tenantPermDbSet on tenant.Id equals tenantPerm.SysTenantId
                        join perm in permDbSet on tenantPerm.SysPermissionId equals perm.Id
                        join menu in DbSet on perm.SysMenuId equals menu.Id
                        select menu);

            return await data.ToListAsync();
        }
    }
}
