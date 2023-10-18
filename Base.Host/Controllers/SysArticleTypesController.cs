using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.Models;
using Base.Host.Models;
using Base.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 文章类型
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class SysArticleTypesController: BaseController
    {
        private readonly ISysArticleTypeService _typeService;

        public SysArticleTypesController(ISysArticleTypeService typeService)
        {
            _typeService = typeService;
        }

        #region 分类
        /// <summary>
        /// 获取文章分类列表
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<SysArticleTypeDto>> GetTreeAsync()
        {
            return await _typeService.GetTreeAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        public async Task<BaseMessage> AddAsync([FromBody]SysArticleTypeForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _typeService.AddAsync(TenantId, form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("添加成功");
                case BaseErrType.DataNotFound:  return msg.Fail("找不到上级分类");
                case BaseErrType.DataExist:     return msg.Fail("已存在相同名称的分类");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPut]
        public async Task<BaseMessage> UpdateAsync([FromBody]SysArticleTypeForm form)
        {
            var msg = new BaseMessage() { Status = false };
            msg.ErrType = await _typeService.UpdateAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("修改成功");
                case BaseErrType.DataNotFound:  return msg.Fail("找不到上级分类");
                case BaseErrType.DataExist:     return msg.Fail("已存在相同名称的分类");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<BaseMessage> DeleteAsync(Guid id)
        {
            var msg = new BaseMessage();
            var errType = await _typeService.DeleteAsync(id);

            switch (errType)
            {
                case BaseErrType.Success:       return msg.Success("删除成功");
                case BaseErrType.DataExist:     return msg.Fail("当前菜单存在子级");
                default:                        return msg.Fail("删除失败");
            }
        }
        #endregion

        #region 分类文章

        /// <summary>
        /// 获取文章分页列表
        /// </summary>
        [HttpGet]
        [Route("{id}/Articles/{pageIndex}/{pageSize}")]
        public async Task<PageList<SysArticleDto>> GetPageArticleAsync(Guid id, int pageIndex, int pageSize, [FromQuery]string key)
        {
            return await _typeService.GetPageArticleAsync(id, pageIndex, pageSize, key);
        }
        #endregion
    }
}
