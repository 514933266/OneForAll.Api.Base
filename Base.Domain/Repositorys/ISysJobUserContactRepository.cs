using Sys.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 岗位用户关联仓储
    /// </summary>
    public interface ISysJobUserContactRepository : IEFCoreRepository<SysJobUserContact>
    {
    }
}
