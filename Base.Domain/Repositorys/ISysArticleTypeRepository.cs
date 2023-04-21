using Sys.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 机构文章分类仓储
    /// </summary>
    public interface ISysArticleTypeRepository : IEFCoreRepository<SysArticleType>
    {
    }
}
