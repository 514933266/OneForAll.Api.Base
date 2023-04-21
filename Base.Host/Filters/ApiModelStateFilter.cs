using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OneForAll.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sys.Host.Filters
{
    /// <summary>
    /// 过滤器：接口参数校验
    /// </summary>
    public class ApiModelStateFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = context.ModelState.Where(m => m.Value.Errors.Any())
                       .Select(x => new { x.Key, x.Value.Errors }).FirstOrDefault().Errors.First().ErrorMessage;
                var msg = new BaseMessage()
                {
                    Status = false,
                    ErrType = BaseErrType.DataError,
                    Message = error
                };
                context.Result = new JsonResult(msg);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
