using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using OneForAll.Core;
using OneForAll.Core.Extension;
using Base.Host.Models;
using Base.HttpService.Interfaces;
using Base.HttpService.Models;

namespace Base.Host.Filters
{
    /// <summary>
    /// 过滤器：全局异常
    /// </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
        private readonly AuthConfig _authConfig;
        private readonly ISysExceptionLogHttpService _httpService;
        public ExceptionFilter(AuthConfig authConfig, ISysExceptionLogHttpService httpService)
        {
            _authConfig = authConfig;
            _httpService = httpService;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                var result = new BaseMessage
                {
                    Status = false,
                    ErrType = BaseErrType.ServerError,
                    Message = context.Exception.Message
                };
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status200OK,
                    ContentType = "application/json;charset=utf-8",
                    Content = result.ToJson()
                };
                #region 记录日志
                var controller = context.ActionDescriptor.RouteValues["controller"];
                var action = context.ActionDescriptor.RouteValues["action"];
                _httpService.AddAsync(new SysExceptionLogRequest()
                {
                    MoudleName = _authConfig.ClientName,
                    MoudleCode = _authConfig.ClientCode,
                    Controller = controller,
                    Action = action,
                    Name = context.Exception.Message,
                    Content = context.Exception.StackTrace
                });
                #endregion
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
