using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Base.Domain.Interfaces;
using Base.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneForAll.Core;
using OneForAll.Core.OAuth;

namespace Base.Host.Controllers
{
    /// <summary>
    /// 权限校验
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoleType.Admin)]
    public class SysPermissionCheckController : BaseController
    {
        private readonly ISysPermissionCheckManager _permManager;
        public SysPermissionCheckController(ISysPermissionCheckManager permManager)
        {
            _permManager = permManager;
        }

        /// <summary>
        /// 校验用户权限
        /// </summary>
        /// <param name="form">表单</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<BaseMessage> ValidateAsync([FromBody] SysPermissionCheck form)
        {
            var msg = new BaseMessage();
            msg.ErrType = await _permManager.ValidateAsync(form);
            switch (msg.ErrType)
            {
                case BaseErrType.Success: return msg.Success("验证通过");
                case BaseErrType.DataNotFound: return msg.Success("用户未登录");
                case BaseErrType.TokenInvalid: return msg.Fail("未授权的客户端");
                case BaseErrType.PermissionNotEnough: return msg.Fail("对不起，您没有操作此功能的权限");
                default: return msg.Fail("发生未知错误");
            }
        }
    }
}
