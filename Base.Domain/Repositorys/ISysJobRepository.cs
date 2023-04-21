using Sys.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 系统岗位仓储
    /// </summary>
    public interface ISysJobRepository : IEFCoreRepository<SysJob>
    {
        /// <summary>
        /// 查询岗位（含用户、角色关联）
        /// </summary>
        /// <param name="departmentIds">部门组织Id集合（一般是上级和下级节点）</param>
        /// <returns>岗位列表</returns>
        Task<IEnumerable<SysJob>> GetListWithContactsAsync(IEnumerable<Guid> departmentIds);

        /// <summary>
        /// 查询岗位（含角色关联）
        /// </summary>
        /// <param name="departmentIds">部门组织Id集合（一般是上级和下级节点）</param>
        /// <returns>岗位列表</returns>
        Task<IEnumerable<SysJob>> GetListWithRolesAsync(IEnumerable<Guid> departmentIds);

        /// <summary>
        /// 查询岗位（含用户关联）
        /// </summary>
        /// <param name="departmentIds">部门组织Id集合（一般是上级和下级节点）</param>
        /// <returns>岗位列表</returns>
        Task<IEnumerable<SysJob>> GetListWithUsersAsync(IEnumerable<Guid> departmentIds);

        /// <summary>
        /// 查询岗位（含角色关联）
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <returns>岗位</returns>
        Task<SysJob> GetWithRoleContactsAsync(Guid id);

        /// <summary>
        /// 查询岗位（含用户关联）
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <returns>岗位</returns>
        Task<SysJob> GetWithUserContactsAsync(Guid id);
    }
}
