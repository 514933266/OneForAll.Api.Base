using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using Base.Domain.Entities;
using Base.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 机构权限
    /// </summary>
    public class SysMidTenantPermissionRepository : Repository<SysMidTenantPermission>, ISysMidTenantPermissionRepository
    {
        public SysMidTenantPermissionRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询机构权限id
        /// </summary>
        /// <param name="tenantId">机构id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<Guid>> GetListPermissionIdAsync(Guid tenantId)
        {
            return await DbSet.Where(w => w.SysTenantId == tenantId).Select(s => s.SysPermissionId).ToListAsync();
        }
    }
}
