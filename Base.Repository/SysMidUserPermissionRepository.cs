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
    /// 用户权限
    /// </summary>
    public class SysMidUserPermissionRepository : Repository<SysMidUserPermission>, ISysMidUserPermissionRepository
    {
        public SysMidUserPermissionRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询用户权限id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<Guid>> GetListPermIdByUserAsync(Guid userId)
        {
            return await DbSet.Where(w => w.SysUserId == userId).Select(s => s.SysPermissionId).ToListAsync();
        }

        /// <summary>
        /// 批量查询多个用户的直授权限关联
        /// </summary>
        /// <param name="userIds">用户id集合</param>
        /// <returns>用户权限关联列表</returns>
        public async Task<IEnumerable<SysMidUserPermission>> GetListByUsersAsync(IEnumerable<Guid> userIds)
        {
            if (userIds == null || !userIds.Any())
                return new List<SysMidUserPermission>();
            return await DbSet.Where(w => userIds.Contains(w.SysUserId)).ToListAsync();
        }
    }
}
