using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Application;
using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Domain.AggregateRoots;
using Base.Domain.Enums;
using Base.Domain.Models;
using Base.Domain.ValueObjects;
using Base.Host.Models;
using Base.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using OneForAll.Core.Upload;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 个人中心
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class SysPersonalController : BaseController
    {
        private readonly ISysPersonalService _service;

        public SysPersonalController(ISysPersonalService personalService)
        {
            _service = personalService;
        }

        /// <summary>
        /// 获取个人信息（含机构、菜单、权限）
        /// </summary>
        [HttpGet]
        public async Task<SysPersonalDto> GetAsync()
        {
            return await _service.GetAsync();
        }

        /// <summary>
        /// 查询个人权限菜单
        /// </summary>
        /// <returns>菜单树</returns>
        [HttpGet]
        [Route("Menus")]
        public async Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync()
        {
            return await _service.GetListMenuTreeAsync();
        }

        /// <summary>
        /// 查询个人权限菜单
        /// </summary>
        /// <returns>子菜单列表</returns>
        [HttpGet]
        [Route("SubMenus")]
        public async Task<IEnumerable<SysMenuDto>> GetListSubMenuAsync()
        {
            return await _service.GetListMenuAsync();
        }

        /// <summary>
        /// 修改资料
        /// </summary>
        [HttpPut]
        public async Task<BaseMessage> UpdateAsync([FromBody] SysPersonalForm form)
        {
            var msg = new BaseMessage();
            var errType = await _service.UpdateAsync(form);

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

                var callbacks = await _service.UploadHeaderAsync(file.FileName, file.OpenReadStream());

                msg.Data = new { Username = LoginUser.UserName, Result = callbacks };

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
            msg.ErrType = await _service.ChangePasswordAsync(password);

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
        /// 修改绑定所属机构
        /// </summary>
        /// <param name="tenantId">要新绑定的租户id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [Route("TenantId")]
        public async Task<BaseMessage> UpdateTenantAsync([FromQuery] Guid tenantId)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.UpdateTenantAsync(tenantId);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                case BaseErrType.DataNotFound: return msg.Fail("用户不存在");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 登录记录
        /// </summary>
        [HttpPost]
        public async Task<BaseErrType> LoginAsync([FromBody] SysPersonalLoginLogForm form)
        {
            return await _service.LoginAsync(form);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        [HttpDelete]
        public async Task<BaseErrType> LoginoutAsync()
        {
            return await _service.LoginoutAsync();
        }

        #region 微信相关

        /// <summary>
        /// 获取微信access_token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Wechat/AccessToken")]
        public async Task<BaseMessage> GetWxAccessTokenAsync()
        {
            var msg = new BaseMessage();
            msg.Data = await _service.GetWxAccessTokenAsync(LoginUser.Id);
            return msg.Success();
        }

        /// <summary>
        /// 获取微信access_token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Wechat/SubscribeInfo")]
        public async Task<BaseMessage> GetWxgzhSubscribeUserAsync()
        {
            var msg = new BaseMessage();
            msg.Data = await _service.GetWxgzhSubscribeUserAsync(LoginUser.Id);
            return msg.Success();
        }

        #endregion
    }
}