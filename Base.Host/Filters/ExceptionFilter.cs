using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;
using OneForAll.Core;
using OneForAll.Core.Extension;

namespace Sys.Host.Filters
{
    /// <summary>
    /// 过滤器：全局异常
    /// </summary>
    public class ExceptionFilter : IAsyncExceptionFilter
    {
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
            }
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
