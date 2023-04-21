using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.Models;
using Sys.Host.Models;
using Sys.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using static Sys.Host.Filters.AuthorizationFilter;

namespace Sys.Host.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysTenantUsersController : BaseController
    {
        private readonly ISysTenantUserService _userService;
        public SysTenantUsersController(ISysTenantUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>用户列表</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<SysTenantUserDto>> GetListAsync(int pageIndex, int pageSize, [FromQuery] string key)
        {
            return await _userService.GetPageAsync(pageIndex, pageSize, key);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="ids">关键字</param>
        /// <returns>用户列表</returns>
        [HttpGet]
        public async Task<IEnumerable<SysTenantUserDto>> GetListAsync(
            [FromQuery] IEnumerable<Guid> ids = default)
        {
            return await _userService.GetListAsync(ids);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPost]
        [CheckPermission]
        public async Task<BaseMessage> AddAsync([FromBody] SysUserForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _userService.AddAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataExist: return msg.Fail("用户名已被使用");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPut]
        [CheckPermission]
        public async Task<BaseMessage> UpdateAsync([FromBody] SysUserUpdateForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _userService.UpdateAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                case BaseErrType.DataExist: return msg.Fail("用户名已被使用");
                case BaseErrType.DataNotMatch: return msg.Fail("两次输入的密码不一致");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">用户id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [CheckPermission]
        [Route("Batch/IsDeleted")]
        public async Task<BaseMessage> DeleteAsync([FromBody] IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _userService.DeleteAsync(ids);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                default: return msg.Fail("删除失败");
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        [HttpPatch]
        [CheckPermission]
        [Route("Batch/Password")]
        public async Task<BaseMessage> UpdateAsync([FromBody] IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _userService.ResetPasswordAsync(ids);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("重置成功");
                case BaseErrType.DataNotFound: return msg.Fail("用户不存在");
                case BaseErrType.DataNotMatch: return msg.Fail("机构不匹配");
                default: return msg.Fail("重置失败");
            }
        }
    }
}
