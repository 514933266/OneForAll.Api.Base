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

namespace Base.Host.Controllers
{
    /// <summary>
    /// 消息中心
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.PUBLIC)]
    public class SysPersonalMessagesController : BaseController
    {
        private readonly ISysPersonalMessageService _service;

        public SysPersonalMessagesController(
            ISysPersonalMessageService service)
        {
            _service = service;
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("{top}")]
        public async Task<IEnumerable<SysPersonalMessageDto>> GetListMessageAsync(int top)
        {
            return await _service.GetListMessageAsync(top);
        }

        /// <summary>
        /// 获取消息列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="key">关键字</param>
        /// <param name="status"></param>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("{pageIndex}/{pageSize}")]
        public async Task<PageList<SysPersonalMessageDto>> GetPageMessageAsync(int pageIndex, int pageSize, [FromQuery] string key, [FromQuery] UmsMessageStatusEnum status)
        {
            return await _service.GetPageMessageAsync(pageIndex, pageSize, key, status);
        }

        /// <summary>
        /// 查询未读消息
        /// </summary>
        /// <returns>列表</returns>
        [HttpGet]
        [Route("UnReadCount")]
        public async Task<int> GetUnReadCountAsync()
        {
            return await _service.GetUnReadCountAsync();
        }

        /// <summary>
        /// 已读
        /// </summary>
        /// <param name="ids">消息id，为空时将全部已读</param>
        /// <param name="isQuiet">是否不带返回提示</param>
        /// <returns>结果</returns>
        [HttpPatch]
        [Route("IsRead")]
        public async Task<object> ReadAsync([FromBody] IEnumerable<Guid> ids, [FromQuery] bool isQuiet = false)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.ReadAsync(ids);
            if (isQuiet)
            {
                return msg.ErrType;
            }
            else
            {
                switch (msg.ErrType)
                {
                    case BaseErrType.Success: return msg.Success("操作成功");
                    default: return msg.Fail("操作失败");
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">消息id，为空时将全部删除</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<BaseMessage> DeleteAsync([FromBody] IEnumerable<Guid> ids)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _service.DeleteAsync(ids);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("操作成功");
                default: return msg.Fail("操作失败");
            }
        }
    }
}
