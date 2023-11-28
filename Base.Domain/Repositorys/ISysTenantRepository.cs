using Base.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 机构（租户）
    /// </summary>
    public interface ISysTenantRepository : IEFCoreRepository<SysTenant>
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="isEnabled">合作状态（false未合作，true合作中）</param>
        /// <param name="startDate">注册开始时间</param>
        /// <param name="endDate">注册结束时间</param>
        /// <returns>机构列表</returns>
        Task<PageList<SysTenant>> GetPageAsync(
            int pageIndex,
            int pageSize,
            string key,
            bool isEnabled,
            DateTime? startDate,
            DateTime? endDate);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">机构id</param>
        /// <returns>机构列表</returns>
        Task<IEnumerable<SysTenant>> GetListAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 查询指定租户
        /// </summary>
        /// <param name="id">租户id</param>
        /// <returns>实体</returns>
        Task<SysTenant> GetIQFAsync(Guid id);

        /// <summary>
        /// 查询机构（含权限、菜单关联）
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构</returns>
        Task<SysTenant> GetWithMenusAsync(Guid id);

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        Task<SysTenant> GetByNameAsync(string name);

        /// <summary>
        /// 查询机构（全表查询）
        /// </summary>
        /// <param name="code">机构信用代码</param>
        /// <returns>结果</returns>
        Task<SysTenant> GetByCodeAsync(string code);


    }
}
