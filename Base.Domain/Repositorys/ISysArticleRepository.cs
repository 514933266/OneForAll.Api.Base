using Base.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 机构文章仓储
    /// </summary>
    public interface ISysArticleRepository : IEFCoreRepository<SysArticle>
    {
        /// <summary>
        /// 查询已发布的文章分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysArticle>> GetPagePublishedAsync(int pageIndex, int pageSize);

        /// <summary>
        /// 查询分类文章分页（含分类）
        /// </summary>
        /// <param name="typeIds">分类Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysArticle>> GetPageAsync(IEnumerable<Guid> typeIds, int pageIndex, int pageSize, string key);
    }
}
