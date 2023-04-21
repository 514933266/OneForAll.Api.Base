using Sys.Domain.AggregateRoots;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：部门组织
    /// </summary>
    public interface ISysDepartmentManager
    {
        /// <summary>
        /// 获取机构组织列表
        /// </summary>
        /// <returns>部门列表</returns>
        Task<IEnumerable<SysDepartment>> GetListAsync();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">部门表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid tenantId, SysDepartmentForm entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">部门表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysDepartmentForm entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(Guid id);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="entity">排序表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> SortAsync(Guid id, SysDepartmentForm entity);

        /// <summary>
        /// 获取部门角色列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysJobRoleContact>> GetListRoleAsync(Guid id);

        /// <summary>
        /// 获取部门成员列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysJobUserContact>> GetListUserAsync(Guid id);
    }
}
