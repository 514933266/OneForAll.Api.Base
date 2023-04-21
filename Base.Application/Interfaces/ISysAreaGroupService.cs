using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application.Interfaces
{
    /// <summary>
    /// 应用服务：地区组
    /// </summary>
    public interface ISysAreaGroupService
    {
        #region 地区组

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysAreaGroupDto>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">地区组</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid tenantId, SysAreaGroupForm entity);


        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">地区组</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysAreaGroupForm entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);

        #endregion

        #region 权限

        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <returns>地区列表</returns>
        Task<IEnumerable<SysAreaDto>> GetListAreaAsync(Guid id);

        /// <summary>
        /// 绑定地区
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="areaCodes">地区代码</param>
        /// <returns>地区列表</returns>
        Task<BaseErrType> AddAreaAsync(Guid id, IEnumerable<string> areaCodes);

        #endregion

        #region 成员

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <returns>成员列表</returns>
        Task<IEnumerable<SysAreaGroupMemberDto>> GetListMemberAsync(Guid id);

        /// <summary>
        /// 获取没有加入组的人员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="key">用户查询关键字</param>
        /// <returns>人员列表</returns>
        Task<IEnumerable<SysAreaGroupSelectionMemberDto>> GetListUnJoinedUserAsync(Guid id, string key);


        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="userIds">用户id集合</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddMemberAsync(Guid id, IEnumerable<Guid> userIds);

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveMemberAsync(Guid id, IEnumerable<Guid> userIds);

        #endregion
    }
}
