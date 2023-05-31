using AutoMapper;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Interfaces;
using Base.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application
{
    public class SysArticleTypeService : ISysArticleTypeService
    {
        private readonly IMapper _mapper;
        private readonly ISysArticleTypeManager _typeManager;
        private readonly ISysArticleManager _articleManager;
        public SysArticleTypeService(
            IMapper mapper,
            ISysArticleTypeManager typeManager,
            ISysArticleManager articleManager)
        {
            _mapper = mapper;
            _typeManager = typeManager;
            _articleManager = articleManager;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        public async Task<IEnumerable<SysArticleTypeDto>> GetTreeAsync()
        {
            var data = await _typeManager.GetTreeAsync();
            return _mapper.Map<IEnumerable<SysArticleType>, IEnumerable<SysArticleTypeDto>>(data);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> AddAsync(Guid tenantId, SysArticleTypeForm form)
        {
            return await _typeManager.AddAsync(tenantId, form);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> UpdateAsync(SysArticleTypeForm form)
        {
            return await _typeManager.UpdateAsync(form);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        public async Task<BaseErrType> DeleteAsync(Guid id)
        {
            return await _typeManager.DeleteAsync(id);
        }

        /// <summary>
        /// 获取文章分页列表
        /// </summary>
        /// <param name="id">分类id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        public async Task<PageList<SysArticleDto>> GetPageArticleAsync(Guid id, int pageIndex, int pageSize, string key)
        {
            var data = await _articleManager.GetPageAsync(id, pageIndex, pageSize, key);
            var items = _mapper.Map<IEnumerable<SysArticle>, IEnumerable<SysArticleDto>>(data.Items);
            return new PageList<SysArticleDto>(data.Total, data.PageSize, data.PageIndex, items);
        }
    }
}
