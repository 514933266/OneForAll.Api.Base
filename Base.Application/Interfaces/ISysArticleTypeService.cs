using Base.Application.Dtos;
using Base.Domain.Models;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Base.Application.Interfaces
{
    /// <summary>
    /// 机构文章分类应用服务
    /// </summary>
    public interface ISysArticleTypeService
    {

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns>列表</returns>
        Task<IEnumerable<SysArticleTypeDto>> GetTreeAsync();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="tenantId">租户id</param>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> AddAsync(Guid tenantId, SysArticleTypeForm form);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">实体</param>
        /// <returns>结果</returns>
        Task<BaseErrType> UpdateAsync(SysArticleTypeForm form);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">实体id</param>
        /// <returns>结果</returns>
        Task<BaseErrType> DeleteAsync(Guid id);

        /// <summary>
        /// 获取文章分页列表
        /// </summary>
        /// <param name="id">分类id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>分页列表</returns>
        Task<PageList<SysArticleDto>> GetPageArticleAsync(Guid id, int pageIndex, int pageSize, string key);
    }
}
