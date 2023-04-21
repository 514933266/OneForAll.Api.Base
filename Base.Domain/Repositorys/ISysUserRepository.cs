using Sys.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sys.Domain.Aggregates;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface ISysUserRepository : IEFCoreRepository<SysUser>
    {
        #region 当前机构

        /// <summary>
        /// 查询机构用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>实体</returns>
        Task<SysTenantUserAggr> GetAsync(Guid id);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>实体</returns>
        Task<SysUser> GetAsync(string username);

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysUser>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysUser>> GetListAsync(string key);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysUser>> GetListAsync(IEnumerable<Guid> ids);
        #endregion
    }
}
