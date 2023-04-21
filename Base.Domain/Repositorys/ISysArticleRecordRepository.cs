using Sys.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 文章阅读记录仓储
    /// </summary>
    public interface ISysArticleRecordRepository : IEFCoreRepository<SysArticleRecord>
    {
        /// <summary>
        /// 查询用户指定时间文章阅读记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="articleId">文章Id</param>
        /// <param name="datetime">日期</param>
        /// <returns>结果</returns>
        Task<SysArticleRecord> GetAsync(Guid userId, Guid articleId, DateTime datetime);

        /// <summary>
        /// 查询用户文章阅读记录
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="articleIds">文章Id集合</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysArticleRecord>> GetListAsync(Guid userId, IEnumerable<Guid> articleIds);
    }
}
