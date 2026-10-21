using Base.Domain.Entities;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Base.Domain.Aggregates;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 用户
    /// </summary>
    public interface ISysUserRepository : IEFCoreRepository<SysUser>
    {
        #region 全局

        /// <summary>
        /// 查询指定用户（全表，忽略租户过滤）
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>实体</returns>
        Task<SysUser> GetByIdIQFAsync(Guid id);

        /// <summary>
        /// 按账号查询用户（全表，忽略租户过滤）
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns>实体</returns>
        Task<SysUser> GetByNameIQFAsync(string username);

        /// <summary>
        /// 按手机号查询用户（全表，忽略租户过滤）
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>实体</returns>
        Task<SysUser> GetByMobileIQFAsync(string mobile);

        #endregion

        #region 当前机构

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
