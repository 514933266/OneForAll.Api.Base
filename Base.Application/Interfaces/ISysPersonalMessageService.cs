using Sys.Application.Dtos;
using Sys.Domain.Enums;
using Sys.Domain.Models;
using Sys.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application.Interfaces
{
    /// <summary>
    /// 个人消息中心
    /// </summary>
    public interface ISysPersonalMessageService
    {
        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="top">数量</param>
        /// <returns>列表</returns>
        Task<IEnumerable<SysPersonalMessageDto>> GetListMessageAsync(int top);

        /// <summary>
        /// 获取消息分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysPersonalMessageDto>> GetPageMessageAsync(int pageIndex, int pageSize, string key, UmsMessageStatusEnum status);

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>结果</returns>
        Task<int> GetUnReadCountAsync();

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids);
    }
}
