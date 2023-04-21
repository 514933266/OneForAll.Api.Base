using Sys.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 仓储：部门组织
    /// </summary>
    public interface ISysDepartmentRepository : IEFCoreRepository<SysDepartment>
    {

        /// <summary>
        /// 统计子集节点数量
        /// </summary>
        /// <param name="parentId">上级Id</param>
        /// <returns>数量</returns>
        Task<int> CountChildrenAsync(Guid parentId);

        /// <summary>
        /// 查询部门（含岗位）
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>部门</returns>
        Task<SysDepartment> GetWithJobsAsync(Guid id);
    }
}
