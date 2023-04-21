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
    /// 领域服务：岗位
    /// </summary>
    public interface ISysJobManager
    {
        /// <summary>
        /// 获取部门岗位列表
        /// </summary>
        /// <param name="departmentId">部门id</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<SysJob>> GetListAsync(Guid departmentId);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="departmentId">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid departmentId, SysJobForm entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="departmentId">部门组织Id</param>
        /// <param name="entity">岗位表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(Guid departmentId, SysJobForm entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="departmentId">部门组织Id</param>
        /// <param name="ids">岗位id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(Guid departmentId, IEnumerable<Guid> ids);

        #region 角色、成员

        /// <summary>
        /// 获取未加入岗位的角色列表
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="key">用户关键字</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<SysRole>> GetListUnJoinedRoleAsync(Guid id, string key);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="ids">角色id集合</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddRoleAsync(Guid id, IEnumerable<Guid> ids);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveRoleAsync(Guid id, Guid roleId);

        /// <summary>
        /// 获取未加入岗位的用户列表
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="key">用户关键字</param>
        /// <returns>角色列表</returns>
        Task<IEnumerable<SysUser>> GetListUnJoinedUserAsync(Guid id, string key);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="ids">用户id集合</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddUserAsync(Guid id, IEnumerable<Guid> ids);

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="id">岗位id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveUserAsync(Guid id, Guid userId);

        #endregion

    }
}
