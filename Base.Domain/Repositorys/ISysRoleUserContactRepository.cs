using OneForAll.EFCore;
using Base.Domain.AggregateRoots;
using Base.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 角色用户
    /// </summary>
    public interface ISysRoleUserContactRepository : IEFCoreRepository<SysRoleUserContact>
    {
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysRoleUserContact>> GetListByUserAsync(IEnumerable<Guid> userIds);

        /// <summary>
        /// 查询角色用户
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysUser>> GetListUserByRoleAsync(Guid roleId);

        /// <summary>
        /// 查询角色用户id
        /// </summary>
        /// <param name="roleId">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListUserIdByRoleAsync(Guid roleId);

        /// <summary>
        /// 查询用户权限id
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<Guid>> GetListPermIdByUserAsync(Guid userId);

        /// <summary>
        /// 查询角色用户数量
        /// </summary>
        /// <param name="roleIds">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysRoleMemberCountVo>> GetListRoleUserCountAsync(IEnumerable<Guid> roleIds);
    }
}
