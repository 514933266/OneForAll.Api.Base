using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sys.Host.Models;
using Sys.Application.Dtos;
using Sys.Application.Interfaces;
using Sys.Public.Models;

namespace Sys.Host.Controllers.Core
{
    /// <summary>
    /// 地区
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysAreasController : BaseController
    {
        private readonly ISysAreaService _areaService;

        public SysAreasController(ISysAreaService areaService)
        {
            _areaService = areaService;
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        [HttpGet]
        [Route("Provinces")]
        public async Task<IEnumerable<SysAreaSelectionDto>> GetProvinceAsync()
        {
            return await _areaService.GetListProvinceAsync();
        }

        /// <summary>
        /// 获取子级地区
        /// </summary>
        [HttpGet]
        [Route("{id}/Children")]
        public async Task<IEnumerable<SysAreaSelectionDto>> GetChildrenAsync(int id)
        {
            return await _areaService.GetChildrenAsync(id);
        }
    }
}
