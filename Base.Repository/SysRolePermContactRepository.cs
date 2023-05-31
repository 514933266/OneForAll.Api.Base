using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 角色权限
    /// </summary>
    public class SysRolePermContactRepository : Repository<SysRolePermContact>, ISysRolePermContactRepository
    {
        public SysRolePermContactRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysRolePermContact>> GetListAsync(Guid roleId)
        {
            return await DbSet.Where(w => w.SysRoleId == roleId).ToListAsync();
        }

        /// <summary>
        /// 查询角色权限id
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<Guid>> GetListPermissionIdAsync(Guid roleId)
        {
            return await DbSet.Where(w => w.SysRoleId == roleId).Select(s => s.SysPermissionId).ToListAsync();
        }

        /// <summary>
        /// 查询角色权限
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysPermission>> GetListPermissionAsync(Guid roleId)
        {
            var permDbSet = Context.Set<SysPermission>();
            var query = (from rolePerm in DbSet
                         join perm in permDbSet on rolePerm.SysPermissionId equals perm.Id
                         where rolePerm.SysRoleId == roleId
                         select perm);
            return await query.ToListAsync();
        }
    }
}
