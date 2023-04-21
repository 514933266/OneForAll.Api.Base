using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Domain.Models;
using Sys.Host.Models;
using Sys.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;

namespace Sys.Host.Controllers
{
    /// <summary>
    /// 部门角色
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysDepartmentsController : BaseController
    {
        private readonly ISysDepartmentService _departmentService;
        public SysDepartmentsController(ISysDepartmentService departService)
        {
            _departmentService = departService;
        }

        #region 部门

        /// <summary>
        /// 获取所有部门
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<SysDepartmentDto>> GetListAsync()
        {
            return await _departmentService.GetListAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        [HttpPost]
        [CheckPermission]
        public async Task<BaseMessage> AddAsync([FromBody]SysDepartmentForm entity)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.AddAsync(TenantId, entity);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("添加成功");
                case BaseErrType.DataNotFound:  return msg.Fail("找不到上级菜单");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        [HttpPut]
        [CheckPermission]
        public async Task<BaseMessage> UpdateAsync([FromBody]SysDepartmentForm entity)
        {
            var msg = new BaseMessage() { Status = false };
            msg.ErrType = await _departmentService.UpdateAsync(entity);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("修改成功");
                case BaseErrType.DataNotFound:  return msg.Fail("找不到上级");
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
            var errType = await _departmentService.DeleteAsync(id);

            switch (errType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                case BaseErrType.DataExist: return msg.Fail("当前部门组织存在子级");
                default: return msg.Fail("删除失败");
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="entity">表单</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [CheckPermission]
        [Route("{id}/SortNumber")]
        public async Task UpdateAsync(Guid id, [FromBody]SysDepartmentForm entity)
        {
            await _departmentService.SortAsync(id, entity);
        }
        #endregion

        #region 岗位

        /// <summary>
        /// 获取岗位（包含下级部门的岗位）
        /// </summary>
        [HttpGet]
        [Route("{id}/Jobs")]
        public async Task<IEnumerable<SysJobDto>> GetListJobAsync(Guid id)
        {
            return await _departmentService.GetListJobAsync(id);
        }

        /// <summary>
        /// 添加岗位
        /// </summary>
        [HttpPost]
        [CheckPermission]
        [Route("{id}/Jobs")]
        public async Task<BaseMessage> AddJobAsync(Guid id, [FromBody]SysJobForm job)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.AddJobAsync(id, job);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("添加成功");
                case BaseErrType.DataExist:     return msg.Fail("该部门已经存在相同岗位");
                case BaseErrType.DataNotFound:  return msg.Fail("找不到对应部门组织");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 修改岗位
        /// </summary>
        [HttpPut]
        [CheckPermission]
        [Route("{id}/Jobs")]
        public async Task<BaseMessage> UpdateJobAsync(Guid id, [FromBody]SysJobForm job)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.UpdateJobAsync(id, job);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("修改成功");
                case BaseErrType.DataExist:     return msg.Fail("该部门已经存在相同岗位");
                case BaseErrType.DataNotFound:  return msg.Fail("找不到对应部门组织");
                default: return msg.Fail("修改失败");
            }
        }

        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="id">部门id</param>
        /// <param name="jobIds">岗位id</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [CheckPermission]
        [Route("{id}/Jobs/Batch/IsDeleted")]
        public async Task<BaseMessage> DeleteJobAsync(Guid id, [FromBody]IEnumerable<Guid> jobIds)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.DeleteJobAsync(id, jobIds);

            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("操作成功");
                case BaseErrType.DataNotFound:  return msg.Fail("岗位可能不属于当前部门组织");
                default: return msg.Fail("操作失败");
            }
        }
        #endregion

        #region 角色

        /// <summary>
        /// 获取角色（包含下级）
        /// </summary>
        [HttpGet]
        [Route("{id}/Roles")]
        public async Task<IEnumerable<SysDepartmentRoleDto>> GetListRoleAsync(Guid id)
        {
            return await _departmentService.GetListRoleAsync(id);
        }

        /// <summary>
        /// 获取未加入岗位角色
        /// </summary>
        [HttpGet]
        [Route("{id}/Jobs/{jobId}/UnJoined/Roles")]
        public async Task<IEnumerable<SysJobRoleSelectionDto>> GetListUnJoinedRoleAsync(Guid id,Guid jobId, [FromQuery]string key)
        {
            return await _departmentService.GetListUnJoinedRoleAsync(id, jobId, key);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        [HttpPost]
        [CheckPermission]
        [Route("{id}/Jobs/{jobId}/Roles")]
        public async Task<BaseMessage> AddJobRoleAsync(Guid id, Guid jobId, [FromBody]IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.AddJobRoleAsync(id, jobId, ids);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataNotFound: return msg.Fail("岗位不存在");
                case BaseErrType.DataEmpty: return msg.Fail("角色不存在");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        [HttpDelete]
        [CheckPermission]
        [Route("{id}/Jobs/{jobId}/Roles/{roleId}")]
        public async Task<BaseMessage> RemoveJobRoleAsync(Guid id, Guid jobId, Guid roleId)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.RemoveJobRoleAsync(id, jobId, roleId);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("删除成功");
                case BaseErrType.DataNotFound: return msg.Fail("岗位不存在");
                case BaseErrType.DataEmpty: return msg.Fail("角色不存在");
                default: return msg.Fail("删除失败");
            }
        }
        #endregion

        #region 用户

        /// <summary>
        /// 获取用户（包含下级）
        /// </summary>
        [HttpGet]
        [Route("{id}/Users")]
        public async Task<IEnumerable<SysDepartmentUserDto>> GetListUserAsync(Guid id)
        {
            return await _departmentService.GetListUserAsync(id);
        }

        /// <summary>
        /// 获取未加入岗位用户
        /// </summary>
        [HttpGet]
        [Route("{id}/Jobs/{jobId}/UnJoined/Users")]
        public async Task<IEnumerable<SysJobUserSelectionDto>> GetListUnJoinedUserAsync(Guid id, Guid jobId, [FromQuery]string key)
        {
            return await _departmentService.GetListUnJoinedUserAsync(id, jobId, key);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpPost]
        [CheckPermission]
        [Route("{id}/Jobs/{jobId}/Users")]
        public async Task<BaseMessage> AddJobUserAsync(Guid id, Guid jobId, [FromBody]IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.AddJobUserAsync(id, jobId, ids);

            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("添加成功");
                case BaseErrType.DataNotFound: return msg.Fail("岗位不存在");
                case BaseErrType.DataEmpty: return msg.Fail("用户不存在");
                default: return msg.Fail("添加失败");
            }
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        [HttpDelete]
        [CheckPermission]
        [Route("{id}/Jobs/{jobId}/Users/{userId}")]
        public async Task<BaseMessage> RemoveJobUserAsync(Guid id, Guid jobId, Guid userId)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _departmentService.RemoveJobUserAsync(id, jobId, userId);
            switch (msg.ErrType)
            {
                case BaseErrType.Success:       return msg.Success("删除成功");
                case BaseErrType.DataEmpty:     return msg.Fail("用户不存在");
                default: return msg.Fail("删除失败");
            }
        }
        #endregion
    }
}
