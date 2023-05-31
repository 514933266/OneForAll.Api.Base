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
    /// 系统角色 仓储
    /// </summary>
    public interface ISysRoleRepository : IEFCoreRepository<SysRole>
    {
        /// <summary>
        /// 查询分页（包含关联权限、用户）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>结果</returns>
        Task<PageList<SysRole>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 查询实体（含成员）
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        Task<SysRole> GetWithMembersAsync(Guid id);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>结果</returns>
        Task<SysRole> GetByNameAsync(string name);

        /// <summary>
        /// 查询实体（含权限）
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        Task<SysRole> GetWithPermsAsync(Guid id);

        /// <summary>
        /// 查询列表
        /// </summary>
        /// <param name="ids">角色id</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysRole>> GetListAsync(IEnumerable<Guid> ids);
    }
}
