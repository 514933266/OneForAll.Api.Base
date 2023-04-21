using OneForAll.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Sys.Application;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using System.Threading.Tasks;
using Sys.Host.Models;
using Sys.Domain.Models;
using Sys.Application.Interfaces;
using Sys.Public.Models;
using static Sys.Host.Filters.AuthorizationFilter;

namespace Sys.Host.Controllers.Core
{
    /// <summary>
    /// 角色
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysTenantRolesController : BaseController
    {
        private readonly ISysTenantRoleService _service;
        public SysTenantRolesController(
            ISysTenantRoleService service)
        {
            _service = service;
        }

        #region 角色

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页数</param>
        /// <param name="key">关键字</param>
        /// <returns>角色列表</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<SysRoleDto>> GetPageAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] string key = default)
        {
            return await _service.GetPageAsync(pageIndex, pageSize, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        [CheckPermission]
        public async Task<BaseMessage> AddAsync([FromBody] SysRoleForm form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddAsync(TenantId, form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataExist: return msg.Fail("角色名已被使用");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPut]
        [CheckPermission]
        public async Task<BaseMessage> UpdateAsync([FromBody] SysRoleForm form)
        {

            var msg = new BaseMessage();
            msg.ErrType = await _service.UpdateAsync(form);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("修改成功");
                case BaseErrType.DataExist: return msg.Fail("角色名已被使用");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">角色id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [CheckPermission]
        [Route("Batch/IsDeleted")]
        public async Task<BaseMessage> DeleteAsync([FromBody] IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.DeleteAsync(ids);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                default: return msg.Fail("删除失败");
            }
        }
        #endregion

        #region 权限

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}/Permissions")]
        public async Task<IEnumerable<SysMenuPermissionDto>> GetListPermissionAsync(Guid id)
        {
            return await _service.GetListPermissionAsync(id);
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="forms">权限id</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("{id}/Permissions")]
        [CheckPermission(Action = "Update")]
        public async Task<BaseMessage> AddPermissionAsync(Guid id, [FromBody] IEnumerable<SysMenuPermissionForm> forms)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddPermissionAsync(id, forms);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataNotFound: return msg.Fail("角色不存在");
                default: return msg.Fail("添加失败");
            }
        }

        #endregion

        #region 成员

        /// <summary>
        /// 获取成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns>成员列表</returns>
        [HttpGet]
        [Route("{id}/Members")]
        public async Task<IEnumerable<SysRoleMemberDto>> GetListMemberAsync(Guid id)
        {
            return await _service.GetListMemberAsync(id);
        }

        /// <summary>
        /// 未加入角色成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="key">关键字</param>
        /// <returns>用户</returns>
        [HttpGet]
        [Route("{id}/Users")]
        [CheckPermission(Action = "Update")]
        public async Task<IEnumerable<SysRoleSelectionMemberDto>> GetListUnJoinedUserAsync(
            Guid id,
            [FromQuery] string key = default)
        {
            return await _service.GetListUnJoinedUserAsync(id, key);
        }

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        [HttpPost]
        [Route("{id}/Members")]
        [CheckPermission(Action = "Update")]
        public async Task<BaseMessage> AddMemberAsync(Guid id, [FromBody] IEnumerable<Guid> userIds)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.AddMemberAsync(id, userIds);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataNotFound: return msg.Fail("角色不存在");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="id">角色id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [Route("{id}/Members/Batch/IsDeleted")]
        [CheckPermission(Action = "Delete")]
        public async Task<BaseMessage> RemoveMemberAsync(Guid id, [FromBody] IEnumerable<Guid> userIds)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.RemoveMemberAsync(id, userIds);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("移除成功");
                default: return msg.Fail("移除失败");
            }
        }
        #endregion
    }
}
