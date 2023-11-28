using Base.Domain.AggregateRoots;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 微信登录用户
    /// </summary>
    public interface ISysWechatUserRepository : IEFCoreRepository<SysWechatUser>
    {
    }
}
