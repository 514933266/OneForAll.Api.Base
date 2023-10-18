using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.Models;
using Base.Host.Models;
using Base.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using OneForAll.Core.Upload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Base.Host.Filters.AuthorizationFilter;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 文章公告
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class SysArticlesController : BaseController
    {
        private readonly ISysArticleService _articleService;

        public SysArticlesController(ISysArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<SysReadArticleDto>> GetPagePublishedAsync(int pageIndex, int pageSize)
        {
            return await _articleService.GetPagePublishedAsync(LoginUser, pageIndex, pageSize);
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        [CheckPermission]
        public async Task<BaseMessage> AddAsync([FromBody]SysArticleForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _articleService.AddAsync(LoginUser, TenantId, form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("添加成功");
                case BaseErrType.DataNotFound:  return msg.Fail("分类不存在");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPut]
        [CheckPermission]
        public async Task<BaseMessage> UpdateAsync([FromBody]SysArticleForm form)
        {
            var msg = new BaseMessage() { Status = false };
            msg.ErrType = await _articleService.UpdateAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("修改成功");
                case BaseErrType.DataNotFound:  return msg.Fail("分类不存在");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        [HttpDelete("{id}")]
        [CheckPermission]
        public async Task<BaseMessage> DeleteAsync(Guid id)
        {
            var msg = new BaseMessage();
            var errType = await _articleService.DeleteAsync(id);

            switch (errType)
            {
                case BaseErrType.Success:       return msg.Success("删除成功");
                case BaseErrType.DataExist:     return msg.Fail("当前菜单存在子级");
                default:                        return msg.Fail("删除失败");
            }
        }

        /// <summary>
        /// 上传封面
        /// </summary>
        [HttpPost]
        [CheckPermission]
        [Route("{id}/Covers")]
        public async Task<BaseMessage> UploadCoverAsync(
            Guid id,
            [FromForm]IFormCollection form)
        {
            var msg = new BaseMessage();
            if (form.Files.Count > 0)
            {
                var file = form.Files[0];
                if (id.Equals(Guid.Empty)) id = Guid.NewGuid(); // 实现先传图再创建对象
                var callbacks = await _articleService.UploadCoverAsync(TenantId, id, file.FileName, file.OpenReadStream());

                msg.Data = new { Id = id, Result = callbacks };

                switch (callbacks.State)
                {
                    case UploadEnum.Success: return msg.Success("上传成功");
                    case UploadEnum.Overflow: return msg.Fail("文件超出限制大小500KB");
                    case UploadEnum.Error: return msg.Fail("上传过程中发生未知错误");
                }
            }
            return msg.Fail("上传失败，请选择文件");
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        [HttpPost]
        [CheckPermission]
        [Route("{id}/Images")]
        public async Task<BaseMessage> UploadImageAsync(
            Guid id,
            [FromForm]IFormCollection form)
        {
            var msg = new BaseMessage();
            if (form.Files.Count > 0)
            {
                var file = form.Files[0];
                if (id.Equals(Guid.Empty)) id = Guid.NewGuid(); // 实现先传图再创建对象
                var callbacks = await _articleService.UploadImageAsync(TenantId, id, file.FileName, file.OpenReadStream());

                msg.Data = new { Id = id, Result = callbacks };

                switch (callbacks.State)
                {
                    case UploadEnum.Success:    return msg.Success("上传成功");
                    case UploadEnum.Overflow:   return msg.Fail("文件超出限制大小2MB");
                    case UploadEnum.Error:      return msg.Fail("上传过程中发生未知错误");
                }
            }
            return msg.Fail("上传失败，请选择文件");
        }

        /// <summary>
        /// 发布
        /// </summary>
        [HttpPatch]
        [CheckPermission]
        [Route("{id}/Publish")]
        public async Task<BaseMessage> PublishAsync(Guid id)
        {
            var msg = new BaseMessage();
            var errType = await _articleService.PublishAsync(id);

            switch (errType)
            {
                case BaseErrType.Success:       return msg.Success("发布成功");
                case BaseErrType.DataNotFound:  return msg.Fail("该文章已被删除");
                default:                        return msg.Fail("发布失败");
            }
        }

        /// <summary>
        /// 添加阅读记录（当前用户）
        /// </summary>
        [HttpPost]
        [Route("{id}/Records")]
        public async Task ReadAsync(Guid id)
        {
            await _articleService.ReadAsync(LoginUser, id);
        }
    }
}
