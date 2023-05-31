using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 角色用户
    /// </summary>
    public class SysRoleUserContactRepository : Repository<SysRoleUserContact>, ISysRoleUserContactRepository
    {
        public SysRoleUserContactRepository(DbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysRoleUserContact>> GetListByUserAsync(IEnumerable<Guid> userIds)
        {
            return await DbSet.Where(w => userIds.Contains(w.SysUserId)).ToListAsync();
        }

        /// <summary>
        /// 查询角色用户
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysUser>> GetListUserByRoleAsync(Guid roleId)
        {
            var userDbSet = Context.Set<SysUser>();

            var data = (from roleUser in DbSet
                        join user in userDbSet on roleUser.SysUserId equals user.Id
                        where roleUser.SysRoleId == roleId
                        select user);

            return await data.ToListAsync();
        }

        /// <summary>
        /// 查询角色用户id
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<Guid>> GetListUserIdByRoleAsync(Guid roleId)
        {
            var userDbSet = Context.Set<SysUser>();

            var data = (from roleUser in DbSet
                        join user in userDbSet on roleUser.SysUserId equals user.Id
                        where roleUser.SysRoleId == roleId
                        select user.Id);

            return await data.ToListAsync();
        }

        /// <summary>
        /// 查询用户权限id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<Guid>> GetListPermissionIdByUserAsync(Guid userId)
        {
            var roleDbSet = Context.Set<SysRole>();
            var rolePermDbSet = Context.Set<SysRolePermContact>();

            var data = (from roleUser in DbSet
                        join rolePerm in rolePermDbSet on roleUser.SysRoleId equals rolePerm.SysRoleId
                        where roleUser.SysUserId == userId
                        select rolePerm.SysPermissionId);

            return await data.ToListAsync();
        }

        /// <summary>
        /// 查询角色用户数量
        /// </summary>
        /// <param name="roleIds">角色id</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysRoleMemberCountVo>> GetListRoleUserCountAsync(IEnumerable<Guid> roleIds)
        {
            var data = (from roleUser in DbSet
                        where roleIds.Contains(roleUser.SysRoleId)
                        group roleUser by roleUser.SysRoleId into gRoleUser
                        select new SysRoleMemberCountVo
                        {
                            SysRoleId = gRoleUser.Key,
                            MemberCount = gRoleUser.Count()
                        });

            return await data.ToListAsync();
        }
    }
}
