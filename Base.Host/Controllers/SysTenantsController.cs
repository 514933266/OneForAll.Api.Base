using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OneForAll.Core;
using Base.Application.Dtos;
using System.Threading.Tasks;
using Base.Host.Models;
using Base.Domain.Models;
using Base.Application.Interfaces;
using Base.Public.Models;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 子租户（机构）
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.ADMIN)]
    public class SysTenantsController : BaseController
    {
        private readonly ISysTenantService _service;

        public SysTenantsController(ISysTenantService service)
        {
            _service = service;
        }

        //#region 子机构

        ///// <summary>
        ///// 获取机构
        ///// </summary>
        ///// <param name="id">机构id</param>
        ///// <returns>机构</returns>
        //[HttpGet]
        //[Route("{id}")]
        //public async Task<SysTenantDto> GetAsync(Guid id)
        //{
        //    return await _service.GetAsync(id);
        //}

        ///// <summary>
        ///// 获取分页
        ///// </summary>
        ///// <param name="pageIndex">页码</param>
        ///// <param name="pageSize">页数</param>
        ///// <param name="key">关键字</param>
        ///// <param name="isEnabled">合作状态（false未合作，true合作中）</param>
        ///// <param name="startDate">注册开始时间</param>
        ///// <param name="endDate">注册结束时间</param>
        ///// <returns>机构列表</returns>
        //[HttpGet]
        //[Route("{pageIndex}/{pageSize}")]
        //public async Task<PageList<SysTenantDto>> GetPageAsync(
        //    int pageIndex,
        //    int pageSize,
        //    [FromQuery] string key = default,
        //    [FromQuery] bool isEnabled = true,
        //    [FromQuery] DateTime? startDate = default,
        //    [FromQuery] DateTime? endDate = default)
        //{
        //    return await _service.GetPageAsync(pageIndex, pageSize, key, isEnabled, startDate, endDate);
        //}

        ///// <summary>
        ///// 添加
        ///// </summary>
        //[HttpPost]
        //[CheckPermission]
        //public async Task<BaseMessage> AddAsync([FromBody]SysTenantForm form)
        //{
        //    var msg = new BaseMessage();
        //    msg.ErrType = await _service.AddAsync(TenantId, form);

        //    switch (msg.ErrType)
        //    {
        //        case BaseErrType.Success:   return msg.Success("添加成功");
        //        case BaseErrType.DataExist: return msg.Fail("机构名称或代码已被使用");
        //        default: return msg.Fail("添加失败");
        //    }
        //}

        ///// <summary>
        ///// 修改
        ///// </summary>
        //[HttpPut]
        //[CheckPermission]
        //public async Task<BaseMessage> UpdateAsync([FromBody]SysTenantForm form)
        //{
        //    var msg = new BaseMessage();
        //    msg.ErrType = await _service.UpdateAsync(form);

        //    switch (msg.ErrType)
        //    {
        //        case BaseErrType.Success: return msg.Success("修改成功");
        //        case BaseErrType.DataExist: return msg.Fail("机构名称或代码已被使用");
        //        default: return msg.Fail("修改失败");
        //    }
        //}

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="ids">机构id</param>
        ///// <returns>消息</returns>
        //[HttpPatch]
        //[CheckPermission]
        //[Route("Batch/IsDeleted")]
        //public async Task<BaseMessage> DeleteAsync([FromBody]IEnumerable<Guid> ids)
        //{
        //    var msg = new BaseMessage();
        //    msg.ErrType = await _service.DeleteAsync(ids);

        //    switch (msg.ErrType)
        //    {
        //        case BaseErrType.Success: return msg.Success("删除成功");
        //        default: return msg.Fail("删除失败");
        //    }
        //}
        //#endregion

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns>菜单树</returns>
        [HttpGet]
        [Route("Current/MenuTrees")]
        public async Task<IEnumerable<SysMenuTreeDto>> GetListMenuTreeAsync()
        {
            return await _service.GetListMenuTreeAsync();
        }
    }
}