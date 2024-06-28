using Base.Domain.AggregateRoots;
using Base.Domain.Enums;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 站内信
    /// </summary>
    public interface ISysPersonalMessageManager
    {
        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="top">数量</param>
        /// <returns>列表</returns>
        Task<IEnumerable<UmsMessage>> GetListAsync(int top);

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="day">近几天</param>
        /// <returns>列表</returns>
        Task<IEnumerable<UmsMessage>> GetListByDayAsync(int day);

        /// <summary>
        /// 获取消息分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        Task<PageList<UmsMessage>> GetPageAsync(int pageIndex, int pageSize, string key, UmsMessageStatusEnum status);

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>分页列表</returns>
        Task<int> GetUnReadCountAsync();

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAllAsync();

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync();
    }
}
