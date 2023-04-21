using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sys.Application;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.AggregateRoots;
using Sys.Domain.Enums;
using Sys.Domain.Models;
using Sys.Domain.ValueObjects;
using Sys.Host.Models;
using Sys.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using OneForAll.Core.Upload;

namespace Sys.Host.Controllers
{
    /// <summary>
    /// 个人中心
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysPersonalController : BaseController
    {
        private readonly ISysPersonalService _personalService;

        public SysPersonalController(
            ISysPersonalService personalService)
        {
            _personalService = personalService;
        }

        /// <summary>
        /// 获取个人信息（含机构、菜单、权限）
        /// </summary>
        [HttpGet]
        public async Task<SysPersonalDto> GetAsync()
        {
            return await _personalService.GetAsync();
        }

        /// <summary>
        /// 查询个人权限菜单
        /// </summary>
        /// <returns>菜单树</returns>
        [HttpGet]
        [Route("Menus")]
        public async Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync()
        {
            return await _personalService.GetListMenuTreeAsync();
        }

        /// <summary>
        /// 查询个人权限菜单
        /// </summary>
        /// <returns>子菜单列表</returns>
        [HttpGet]
        [Route("SubMenus")]
        public async Task<IEnumerable<SysMenuDto>> GetListSubMenuAsync()
        {
            return await _personalService.GetListMenuAsync();
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        [HttpPut]
        public async Task<BaseMessage> UpdateAsync([FromBody] SysPersonalForm form)
        {
            var msg = new BaseMessage();
            var errType = await _personalService.UpdateAsync(form);

            switch (errType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                default: return msg.Fail();
            }
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        [HttpPost]
        [Route("Header")]
        public async Task<BaseMessage> UploadHeaderAsync([FromForm] IFormCollection form)
        {
            var msg = new BaseMessage();
            if (form.Files.Count > 0)
            {
                var file = form.Files[0];

                var callbacks = await _personalService.UploadHeaderAsync(file.FileName, file.OpenReadStream());

                msg.Data = new { Username = UserName, Result = callbacks };

                switch (callbacks.State)
                {
                    case UploadEnum.Success: return msg.Success("上传成功");
                    case UploadEnum.Overflow: return msg.Fail("文件超出限制大小");
                    case UploadEnum.TypeError: return msg.Fail("请选择图片文件上传");
                    case UploadEnum.Error: return msg.Fail("上传过程中发生未知错误");
                }
            }
            return msg.Fail("上传失败，请选择文件");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        [HttpPatch, HttpPut]
        [Route("Password")]
        public async Task<BaseMessage> UpdatePasswordAsync([FromBody] Password password)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _personalService.ChangePasswordAsync(password);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                case BaseErrType.DataNotFound: return msg.Fail("用户不存在");
                case BaseErrType.DataNotMatch: return msg.Fail("两次密码不一致");
                case BaseErrType.PasswordInvalid: return msg.Fail("密码错误");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        [HttpDelete]
        public async Task<BaseErrType> LoginoutAsync()
        {
            return await _personalService.LoginoutAsync();
        }
    }
}