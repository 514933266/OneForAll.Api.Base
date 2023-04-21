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
    /// 领域服务：地区组
    /// </summary>
    public interface ISysAreaGroupManager
    {
        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysAreaGroup>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid tenantId, SysAreaGroupForm entity);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysAreaGroupForm entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);
    }
}
