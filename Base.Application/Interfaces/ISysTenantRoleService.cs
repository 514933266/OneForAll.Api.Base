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
    /// 角色
    /// </summary>
    public interface ISysTenantRoleService
    {
        #region 角色

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysRoleDto>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">角色</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid tenantId, SysRoleForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">角色</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysRoleForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">角色id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);

        #endregion

        #region 权限

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<SysMenuPermissionDto>> GetListPermissionAsync(Guid id);

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="forms">权限id</param>
        /// <returns>权限列表</returns>
        Task<BaseErrType> AddPermissionAsync(Guid id, IEnumerable<SysMenuPermissionForm> forms);

        #endregion

        #region 成员

        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns>成员列表</returns>
        Task<IEnumerable<SysRoleMemberDto>> GetListMemberAsync(Guid id);

        /// <summary>
        /// 获取未加入角色的成员列表
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysRoleSelectionMemberDto>> GetListUnJoinedUserAsync(Guid id, string key);

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userIds">用户id集合</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddMemberAsync(Guid id, IEnumerable<Guid> userIds);

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userIds">用户关联Id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveMemberAsync(Guid id, IEnumerable<Guid> userIds);

        #endregion
    }
}
