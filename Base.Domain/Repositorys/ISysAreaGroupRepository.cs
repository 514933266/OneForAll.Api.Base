using Sys.Domain.AggregateRoots;
using OneForAll.Core;
using OneForAll.Core.ORM;
using OneForAll.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Domain.Repositorys
{
    /// <summary>
    /// 仓储：地区组
    /// </summary>
    public interface ISysAreaGroupRepository : IEFCoreRepository<SysAreaGroup>
    {
        /// <summary>
        /// 查询分页列表（含关联表）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>结果</returns>
        Task<PageList<SysAreaGroup>> GetPageAsync(int pageIndex, int pageSize, string key);

        /// <summary>
        /// 查询地区组
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>地区组</returns>
        Task<IEnumerable<SysAreaGroup>> GetListAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 查询实体（含成员）
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        Task<SysAreaGroup> GetWithMembersAsync(Guid id);

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>实体</returns>
        Task<SysAreaGroup> GetByNameAsync(string name);

        /// <summary>
        /// 查询实体（含地区）
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>实体</returns>
        Task<SysAreaGroup> GetWithAreaContactsAsync(Guid id);
    }
}
