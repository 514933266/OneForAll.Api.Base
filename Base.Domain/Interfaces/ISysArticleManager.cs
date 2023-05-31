using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 机构文章领域
    /// </summary>
    public interface ISysArticleManager
    {
        /// <summary>
        /// 获取已发布分页列表（含用户阅读记录）
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysArticle>> GetPagePublishedAsync(SysLoginUserAggr user, int pageIndex, int pageSize);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="typeId">分类Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysArticle>> GetPageAsync(Guid typeId, int pageIndex, int pageSize, string key);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">文章表单</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(SysLoginUserAggr user, Guid tenantId, SysArticleForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">文章表单</param>
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
        Task<BaseErrType> ReadAsync(SysLoginUserAggr user, Guid id);
    }
}
