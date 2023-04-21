using AutoMapper;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Aggregates;
using Sys.Domain.Enums;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using Sys.Domain.ValueObjects;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Application
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public class SysPersonalMessageService : ISysPersonalMessageService
    {
        private readonly IMapper _mapper;
        private readonly ISysPersonalMessageManager _manager;

        public SysPersonalMessageService(
            IMapper mapper,
            ISysPersonalMessageManager manager
            )
        {
            _mapper = mapper;
            _manager = manager;
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="top">数量</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysPersonalMessageDto>> GetListMessageAsync(int top)
        {
            var data = await _manager.GetListAsync(top);
            return _mapper.Map<IEnumerable<UmsMessage>, IEnumerable<SysPersonalMessageDto>>(data);
        }

        /// <summary>
        /// 获取消息分页列表
        /// </summary>rr
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysPersonalMessageDto>> GetPageMessageAsync(int pageIndex, int pageSize, string key, UmsMessageStatusEnum status)
        {
            var data = await _manager.GetPageAsync(pageIndex, pageSize, key, status);
            var items = _mapper.Map<IEnumerable<UmsMessage>, IEnumerable<SysPersonalMessageDto>>(data.Items);
            return new PageList<SysPersonalMessageDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> GetUnReadCountAsync()
        {
            return await _manager.GetUnReadCountAsync();
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids)
        {
            if (ids.Any())
            {
                return await _manager.ReadAsync(ids);
            }
            else
            {
                return await _manager.ReadAllAsync();
            }
            
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            if (ids.Any())
            {
                return await _manager.DeleteAsync(ids);
            }
            else
            {
                return await _manager.DeleteAsync();
            }
        }
    }
}
