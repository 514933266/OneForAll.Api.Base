using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Aggregates;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using OneForAll.Core;
using OneForAll.Core.Extension;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application
{
    /// <summary>
    /// 系统文章应用服务
    /// </summary>
    public class SysArticleService : ISysArticleService
    {
        private readonly IMapper _mapper;
        private readonly ISysArticleManager _articleManager;
        public SysArticleService(
            IMapper mapper,
            ISysArticleManager articleManager
            )
        {
            _mapper = mapper;
            _articleManager = articleManager;
        }

        /// <summary>
        /// 获取已发布分页列表（含用户阅读记录）
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysReadArticleDto>> GetPagePublishedAsync(SysLoginUserAggr user, int pageIndex, int pageSize)
        {
            var data = await _articleManager.GetPagePublishedAsync(user, pageIndex, pageSize);
            var items = _mapper.Map<IEnumerable<SysArticle>, IEnumerable<SysReadArticleDto>>(data.Items);
            data.Items.ForEach(e =>
            {
                var item = items.First(w => w.Id.Equals(e.Id));
                item.HasRead = e.SysArticleRecords.Any(w => w.SysUserId.Equals(user.Id));
                item.IsNew = e.CreateTime <= DateTime.Now && !item.HasRead;
            });
            return new PageList<SysReadArticleDto>(data.Total, data.PageSize, data.PageIndex, items);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(SysLoginUserAggr user, Guid tenantId, SysArticleForm form)
        {
            return await _articleManager.AddAsync(user, tenantId, form);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysArticleForm form)
        {
            return await _articleManager.UpdateAsync(form);
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            return await _articleManager.DeleteAsync(id);
        }


        /// <summary>
        /// 上传封面
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="id">文章id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>结果</returns>
        public async Task<IUploadResult> UploadCoverAsync(Guid tenantId, Guid id, string filename, Stream file)
        {
            return await _articleManager.UploadCoverAsync(tenantId, id, filename, file);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="id">文章id</param>
        /// <param name="filename">文件名</param>
        /// <param name="file">文件流</param>
        /// <returns>结果</returns>
        public async Task<IUploadResult> UploadImageAsync(Guid tenantId, Guid id, string filename, Stream file)
        {
            return await _articleManager.UploadImageAsync(tenantId, id, filename, file);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> PublishAsync(Guid id)
        {
            return await _articleManager.PublishAsync(id);
        }

        /// <summary>
        /// 阅读文章
        /// </summary>
        /// <param name="user">当前登录用户</param>
        /// <param name="id">文章id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> ReadAsync(SysLoginUserAggr user, Guid id)
        {
            return await _articleManager.ReadAsync(user, id);
        }
    }
}
