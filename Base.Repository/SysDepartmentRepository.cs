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
    /// <summary>
    /// 仓储：部门组织
    /// </summary>
    public class SysDepartmentRepository : Repository<SysDepartment>, ISysDepartmentRepository
    {
        public SysDepartmentRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询部门列表
        /// </summary>
        /// <returns>部门</returns>
        public new async Task<IEnumerable<SysDepartment>> GetListAsync()
        {
            return await DbSet
                .AsNoTracking()
                .OrderByDescending(e => e.SortNumber)
                .ToListAsync();
        }

        /// <summary>
        /// 统计子集节点数量
        /// </summary>
        /// <param name="parentId">上级Id</param>
        /// <returns>数量</returns>
        public async Task<int> CountChildrenAsync(Guid parentId)
        {
            return await DbSet
                .CountAsync(w => w.ParentId.Equals(parentId));
        }

        /// <summary>
        /// 查询部门（含岗位）
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>部门</returns>
        public async Task<SysDepartment> GetWithJobsAsync(Guid id)
        {
            return await DbSet
                .Where(w => w.Id == id)
                .Include(e => e.SysJobs)
                .FirstOrDefaultAsync();
        }
    }
}
