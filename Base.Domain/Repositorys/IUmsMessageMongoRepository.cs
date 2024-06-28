using Base.Domain.AggregateRoots;
using Base.Domain.Enums;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Repositorys
{
    /// <summary>
    /// 系统消息
    /// </summary>
    public interface IUmsMessageMongoRepository
    {
        /// <summary>
        /// 查询用户前x条未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="top">数量</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetListAsync(Guid userId, int top);

        /// <summary>
        /// 查询用户近x天未读消息
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="day">近几天</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetListByDayAsync(Guid userId, int day);

        /// <summary>
        /// 查询用户消息分页列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsMessage>> GetPageAsync(Guid userId, int pageIndex, int pageSize, string key, UmsMessageStatusEnum status);

        /// <summary>
        /// 查询用户未读数量
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<int> GetUnReadCountAsync(Guid userId);

        /// <summary>
        /// 查询用户未读列表
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        Task<IEnumerable<UmsMessage>> GetUnReadListAsync(Guid userId);

        /// <summary>
        /// 已读
        /// </summary>
        /// <returns>结果</returns>
        Task<int> UpdateIsReadAsync(Guid userId, IEnumerable<Guid> ids);

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        Task<int> UpdateIsReadAsync(Guid userId);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>结果</returns>
        Task<int> DeleteAsync(Guid userId, IEnumerable<Guid> ids);

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        Task<int> DeleteAsync(Guid userId);
    }
}
