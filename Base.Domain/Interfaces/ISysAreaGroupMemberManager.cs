using Sys.Domain.AggregateRoots;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Interfaces
{
    /// <summary>
    /// 领域服务：地区组成员
    /// </summary>
    public interface ISysAreaGroupMemberManager
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <returns>权限列表</returns>
        Task<IEnumerable<SysUser>> GetListAsync(Guid groupId);

        /// <summary>
        /// 获取未加入实体的成员列表
        /// </summary>
        /// <param name="groupId">实体id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        Task<IEnumerable<SysUser>> GetListUnJoinedAsync(Guid groupId, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>权限列表</returns>
        Task<BaseErrType> AddAsync(Guid groupId, IEnumerable<Guid> userIds);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="groupId">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> RemoveAsync(Guid groupId, IEnumerable<Guid> userIds);
    }
}

