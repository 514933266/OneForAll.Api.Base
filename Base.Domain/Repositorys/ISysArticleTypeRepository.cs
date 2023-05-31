using Base.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 机构文章分类仓储
    /// </summary>
    public interface ISysArticleTypeRepository : IEFCoreRepository<SysArticleType>
    {
    }
}
