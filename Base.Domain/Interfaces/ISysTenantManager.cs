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
    /// 领域服务：机构（租户）
    /// </summary>
    public interface ISysTenantManager
    {
        /// <summary>
        /// 获取机构
        /// </summary>
        /// <param name="id">机构id</param>
        /// <returns>机构</returns>
        Task<SysTenant> GetAsync(Guid id);

        /// <summary>
        /// 获取分页
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
        /// 添加
        /// </summary>
        /// <param name="tenantId">登录机构id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid tenantId, SysTenantForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysTenantForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">机构id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);
    }
}
