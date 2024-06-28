﻿using AutoMapper;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Enums;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using Base.Domain.Repositorys;
using Base.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using NPOI.XWPF.UserModel;
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

namespace Base.Domain
{
    /// <summary>
    /// 站内信
    /// </summary>
    public class SysPersonalMessageManager : SysBaseManager, ISysPersonalMessageManager
    {
        private readonly IUmsMessageMongoRepository _repository;

        public SysPersonalMessageManager(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IUmsMessageMongoRepository repository) : base(mapper, httpContextAccessor)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <param name="top">前几条</param>
        /// <returns>分页列表</returns>
        public async Task<IEnumerable<UmsMessage>> GetListAsync(int top)
        {
            if (top > 100) top = 100;
            try
            {
                return await _repository.GetListAsync(LoginUser.Id, top);
            }
            catch
            {
                // 没有安装Mongodb
                return new List<UmsMessage>();
            }
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <param name="day">近几天</param>
        /// <returns>列表</returns>
        public async Task<IEnumerable<UmsMessage>> GetListByDayAsync(int day)
        {
            if (day > 30) day = 30;
            try
            {
                return await _repository.GetListByDayAsync(LoginUser.Id, day);
            }
            catch
            {
                // 没有安装Mongodb
                return new List<UmsMessage>();
            }
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
            return await _repository.GetPageAsync(LoginUser.Id, pageIndex, pageSize, key, status);
        }

        /// <summary>
        /// 获取未读消息
        /// </summary>
        /// <returns>结果</returns>
        public async Task<int> GetUnReadCountAsync()
        {
            return await _repository.GetUnReadCountAsync(LoginUser.Id);
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAsync(IEnumerable<Guid> ids)
        {
            var effect = await _repository.UpdateIsReadAsync(LoginUser.Id, ids);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }

        /// <summary>
        /// 全部已读
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAllAsync()
        {
            var effect = await _repository.UpdateIsReadAsync(LoginUser.Id);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(IEnumerable<Guid> ids)
        {
            var effect = await _repository.DeleteAsync(LoginUser.Id, ids);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync()
        {
            var effect = await _repository.DeleteAsync(LoginUser.Id);
            return effect > 0 ? BaseErrType.Success : BaseErrType.Fail;
        }
    }
}
