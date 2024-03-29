﻿using Microsoft.EntityFrameworkCore;
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
    /// 用户权限
    /// </summary>
    public class SysUserPermContactRepository : Repository<SysUserPermContact>, ISysUserPermContactRepository
    {
        public SysUserPermContactRepository(DbContext context)
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
    }
}
