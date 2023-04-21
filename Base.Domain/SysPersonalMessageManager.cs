﻿using AutoMapper;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Aggregates;
using Sys.Domain.Enums;
using Sys.Domain.Interfaces;
using Sys.Domain.Models;
using Sys.Domain.Repositorys;
using Sys.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core;
using OneForAll.Core.DDD;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using OneForAll.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sys.Domain
{
    /// <summary>
    /// 站内信
    /// </summary>
    public class SysPersonalMessageManager : SysBaseManager, ISysPersonalMessageManager
    {
        private readonly IUmsMessageRepository _messageRepository;

        public SysPersonalMessageManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageRepository messageRepository) : base(mapper, httpContextAccessor)
        {
            _messageRepository = messageRepository;
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>分页列表</returns>
        public async Task<IEnumerable<UmsMessage>> GetListAsync(int top)
        {
            if (top > 10) top = 10;
            return await _messageRepository.GetListAsync(LoginUser.Id, top);
        }

        /// <summary>
        /// 获取消息分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <param name="status">状态</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<UmsMessage>> GetPageAsync(int pageIndex, int pageSize, string key, UmsMessageStatusEnum status)
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;
            return await _messageRepository.GetPageAsync(LoginUser.Id, pageIndex, pageSize, key, status);
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> GetUnReadCountAsync()
        {
            return await _messageRepository.GetUnReadCountAsync(LoginUser.Id);
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids)
        {
            var effect = await _messageRepository.UpdateIsReadAsync(LoginUser.Id, ids);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAllAsync()
        {
            var effect = await _messageRepository.UpdateIsReadAsync(LoginUser.Id);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var effect = await _messageRepository.DeleteAsync(LoginUser.Id, ids);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync()
        {
            var effect = await _messageRepository.DeleteAsync(LoginUser.Id);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }
    }
}