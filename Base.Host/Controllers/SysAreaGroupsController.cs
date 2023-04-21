using System;
using System.Collections.Generic;
using OneForAll.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sys.Application.Dtos;
using Sys.Domain.AggregateRoots;
using System.Threading.Tasks;
using Sys.Host.Models;
using Sys.Domain.Models;
using Sys.Application.Interfaces;
using Sys.Public.Models;

namespace Sys.Host.Controllers
{
    /// <summary>
    /// 地区组
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysAreaGroupsController : BaseController
    {
        private readonly ISysAreaGroupService _groupService;
        public SysAreaGroupsController(
            ISysAreaGroupService roleService)
        {
            _groupService = roleService;
        }

        #region 地区组

        /// <summary>
        /// 查询分页
        /// </summary>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<SysAreaGroupDto>> GetPageAsync(
            int pageIndex,
            int pageSize,
            [FromQuery] string key = default)
        {
            return await _groupService.GetPageAsync(pageIndex, pageSize, key);
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        [CheckPermission]
        public async Task<BaseMessage> AddAsync([FromBody]SysAreaGroupForm entity)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _groupService.AddAsync(TenantId, entity);
            switch (msg.ErrType)
            {
                case BaseErrType.Success:   return msg.Success("添加成功");
                case BaseErrType.DataExist: return msg.Fail("地区组名已被使用");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPut]
        [CheckPermission]
        public async Task<BaseMessage> UpdateAsync([FromBody]SysAreaGroupForm entity)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _groupService.UpdateAsync(entity);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:   return msg.Success("修改成功");
                case BaseErrType.DataExist: return msg.Fail("地区组名已被使用");
                default: return msg.Fail("修改失败");
            }

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">地区组id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [CheckPermission]
        [Route("Batch/IsDeleted")]
        public async Task<BaseMessage> DeleteAsync([FromBody]IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _groupService.DeleteAsync(ids);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                default: return msg.Fail("删除失败");
            }
        }

        #endregion

        #region 地区权限

        /// <summary>
        /// 获取地区
        /// </summary>
        [HttpGet]
        [Route("{id}/Areas")]
        public async Task<IEnumerable<SysAreaDto>> GetAreasAsync(Guid id)
        {
            return await _groupService.GetListAreaAsync(id);
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        [HttpPost]
        [Route("{id}/Areas")]
        public async Task<BaseMessage> AddAreaAsync(Guid id, [FromBody]IEnumerable<string> areaCodes)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _groupService.AddAreaAsync(id, areaCodes);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataNotFound: return msg.Fail("地区组不存在");
                default: return msg.Fail("添加失败");
            }
        }
        #endregion

        #region 地区用户

        /// <summary>
        /// 获取绑定用户
        /// </summary>
        [HttpGet]
        [Route("{id}/Members")]
        public async Task<IEnumerable<SysAreaGroupMemberDto>> GetListMemberAsync(Guid id)
        {
            return await _groupService.GetListMemberAsync(id);
        }

        /// <summary>
        /// 未加入地区组成员
        /// </summary>
        [HttpGet]
        [Route("{id}/Users")]
        public async Task<IEnumerable<SysAreaGroupSelectionMemberDto>> GetListUnJoinedUserAsync(Guid id, [FromQuery]string key)
        {
            return await _groupService.GetListUnJoinedUserAsync(id, key);
        }

        /// <summary>
        /// 绑定用户
        /// </summary>
        [HttpPost]
        [Route("{id}/Members")]
        public async Task<BaseMessage> AddMemberAsync(Guid id, [FromBody]IEnumerable<Guid> userIds)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _groupService.AddMemberAsync(id, userIds);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:           return msg.Success("添加成功");
                case BaseErrType.DataNotFound:      return msg.Fail("地区组不存在");
                default:                            return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="id">地区组id</param>
        /// <param name="userIds">用户id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [Route("{id}/Members")]
        public async Task<BaseMessage> RemoveMemberAsync(Guid id, [FromBody]IEnumerable<Guid> userIds)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _groupService.RemoveMemberAsync(id, userIds);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("移除成功");
                default: return msg.Fail("移除失败");
            }
        }
        #endregion
    }
}
