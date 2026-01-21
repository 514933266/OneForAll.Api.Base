using Base.Application.Dtos;
using Base.Application.Interfaces;
using Base.Public.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core.OAuth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.Admin)]
    public class SysUserRolesController : BaseController
    {
        private readonly ISysUserRoleService _service;
        public SysUserRolesController(ISysUserRoleService service)
        {
            _service = service;
        }

        /// <summary>
        /// 获取用户的角色信息
        /// </summary>
        /// <param name="userIds">用户id</param>
        /// <returns>角色列表</returns>
        [HttpGet]
        public async Task<IEnumerable<SysUserRoleDto>> GetListAsync([FromQuery] IEnumerable<Guid> userIds)
        {
            return await _service.GetListAsync(userIds);
        }
    }
}
