using Sys.Application.Dtos;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application.Interfaces
{
    /// <summary>
    /// 应用服务：部门组织
    /// </summary>
    public interface ISysDepartmentService
    {
        #region 部门

        /// <summary>
        /// 获取机构组织列表
        /// </summary>
        /// <returns>部门列表</returns>
        Task<IEnumerable<SysDepartmentDto>> GetListAsync();

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

        #endregion

        #region 岗位

        /// <summary>
        /// 获取部门岗位列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>岗位列表</returns>
        Task<IEnumerable<SysJobDto>> GetListJobAsync(Guid id);

        /// <summary>
        /// 添加岗位
        /// </summary>
        /// <param name="id">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddJobAsync(Guid id, SysJobForm entity);

        /// <summary>
        /// 修改岗位
        /// </summary>
        /// <param name="id">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateJobAsync(Guid id, SysJobForm entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">部门组织Id</param>
        /// <param name="jobIds">岗位id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteJobAsync(Guid id, IEnumerable<Guid> jobIds);

        #endregion

        /// <summary>
        /// 获取部门角色列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysDepartmentRoleDto>> GetListRoleAsync(Guid id);

        /// <summary>
        /// 获取没有加入岗位的角色列表
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="key">关键字</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<SysJobRoleSelectionDto>> GetListUnJoinedRoleAsync(Guid id, Guid jobId, string key);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="ids">角色id集合</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddJobRoleAsync(Guid id, Guid jobId, IEnumerable<Guid> ids);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveJobRoleAsync(Guid id, Guid jobId, Guid roleId);

        /// <summary>
        /// 获取部门成员列表
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysDepartmentUserDto>> GetListUserAsync(Guid id);

        /// <summary>
        /// 获取没有加入岗位的用户列表
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysJobUserSelectionDto>> GetListUnJoinedUserAsync(Guid id, Guid jobId, string key);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="ids">用户id集合</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddJobUserAsync(Guid id, Guid jobId, IEnumerable<Guid> ids);

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobId">岗位id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveJobUserAsync(Guid id, Guid jobId, Guid userId);
    }
}
