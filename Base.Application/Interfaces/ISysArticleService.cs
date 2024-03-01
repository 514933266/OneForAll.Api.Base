﻿using Base.Application.Dtos;
using Base.Domain.Aggregates;
using Base.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.OAuth;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    /// <summary>
    /// 机构文章应用服务
    /// </summary>
    public interface ISysArticleService
    {
        /// <summary>
        /// 获取已发布分页列表（含用户阅读记录）
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysReadArticleDto>> GetPagePublishedAsync(LoginUser user, int pageIndex, int pageSize);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(LoginUser user, Guid tenantId, SysArticleForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysArticleForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(Guid id);

        /// <summary>
        /// 上传封面
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="id">文章id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>结果</returns>
        Task<IUploadResult> UploadCoverAsync(Guid tenantId, Guid id, string filename, Stream file);

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="id">文章id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>结果</returns>
        Task<IUploadResult> UploadImageAsync(Guid tenantId, Guid id, string filename, Stream file);

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> PublishAsync(Guid id);

        /// <summary>
        /// 阅读文章
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> ReadAsync(LoginUser user, Guid id);
    }
}
