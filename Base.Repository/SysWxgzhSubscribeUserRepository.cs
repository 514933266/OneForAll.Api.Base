using Base.Domain.AggregateRoots;
using Base.Domain.Repositorys;
using Microsoft.EntityFrameworkCore;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Repository
{
    /// <summary>
    /// 微信公众号关注用户
    /// </summary>
    public class SysWxgzhSubscribeUserRepository : Repository<SysWxgzhSubscribeUser>, ISysWxgzhSubscribeUserRepository
    {
        public SysWxgzhSubscribeUserRepository(DbContext context)
            : base(context)
        {

        }
    }
}
