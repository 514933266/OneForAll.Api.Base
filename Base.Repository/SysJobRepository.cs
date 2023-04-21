using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    public class SysJobRepository : Repository<SysJob>, ISysJobRepository
    {
        public SysJobRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询岗位（含用户、角色关联）
        /// </summary>
        /// <param name="departmentIds">部门组织Id集合（一般是上级和下级节点）</param>
        /// <returns>岗位列表</returns>
        public async Task<IEnumerable<SysJob>> GetListWithContactsAsync(IEnumerable<Guid> departmentIds)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => departmentIds.Contains(w.SysDepartmentId))
                .Include(e => e.SysJobRoleContacts)
                .Include(e => e.SysJobUserContacts)
                .ToListAsync();
        }


        /// <summary>
        /// 查询岗位（含角色关联）
        /// </summary>
        /// <param name="departmentIds">部门组织Id集合（一般是上级和下级节点）</param>
        /// <returns>岗位列表</returns>
        public async Task<IEnumerable<SysJob>> GetListWithRolesAsync(IEnumerable<Guid> departmentIds)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => departmentIds.Contains(w.SysDepartmentId))
                .Include(e => e.SysJobRoleContacts)
                .ToListAsync();
        }

        /// <summary>
        /// 查询岗位（含用户关联）
        /// </summary>
        /// <param name="departmentIds">部门组织Id集合（一般是上级和下级节点）</param>
        /// <returns>岗位列表</returns>
        public async Task<IEnumerable<SysJob>> GetListWithUsersAsync(IEnumerable<Guid> departmentIds)
        {
            return await DbSet
                .AsNoTracking()
                .Where(w => departmentIds.Contains(w.SysDepartmentId))
                .Include(e => e.SysJobUserContacts)
                .ToListAsync();
        }

        /// <summary>
        /// 查询岗位（含角色关联）
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <returns>岗位</returns>
        public async Task<SysJob> GetWithRoleContactsAsync(Guid id)
        {
            return await DbSet
               .Where(w => w.Id.Equals(id))
               .Include(e => e.SysJobRoleContacts)
               .FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询岗位（含用户关联）
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <returns>岗位</returns>
        public async Task<SysJob> GetWithUserContactsAsync(Guid id)
        {
            return await DbSet
               .Where(w => w.Id.Equals(id))
               .Include(e => e.SysJobUserContacts)
               .FirstOrDefaultAsync();
        }
    }
}
