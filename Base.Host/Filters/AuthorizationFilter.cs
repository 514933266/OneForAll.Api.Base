using OneForAll.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using Base.Domain;
using Base.Host.Models;
using Base.Domain.Interfaces;
using Base.Domain.ValueObjects;
using OneForAll.Core.Security;
using OneForAll.Core.Extension;
using OneForAll.Core.Utility;
using Base.Public.Models;
using Microsoft.AspNetCore.Authorization;

namespace Base.Host.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        private readonly AuthConfig _config;
        private readonly ISysPermissionCheckManager _permCheckManager;
        public AuthorizationFilter(
            AuthConfig config,
            ISysPermissionCheckManager permCheckManager)
        {
            _config = config;
            _permCheckManager = permCheckManager;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter) ||
                !(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return;
            }
            var unChecked = context.HttpContext.Request.Headers["Unchecked"];
            if (!unChecked.IsNull())
            {
                // 不检查权限
                var sign = "clientId={0}&clientSecret={1}&apiName={2}&tt={3}".Fmt(_config.ClientId, _config.ClientSecret, _config.ApiName, DateTime.Now.ToString("yyyyMMddhhmm")).ToMd5();
                if (unChecked.ToString() == sign) return;
            };

            var classAttrs = new List<CheckPermissionAttribute>();
            var methodAttrs = new List<CheckPermissionAttribute>();
            methodAttrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true).OfType<CheckPermissionAttribute>().ToList());
            classAttrs.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<CheckPermissionAttribute>().ToList());

            if (methodAttrs.Count > 0 || classAttrs.Count > 0)
            {
                var msg = new BaseMessage();
                if (methodAttrs.Count > 0)
                {
                    msg = ValidateAuthorization(context, methodAttrs);
                }
                else
                {
                    msg = ValidateAuthorization(context, classAttrs);
                }

                if (msg.ErrType != BaseErrType.Success)
                {
                    context.Result = new JsonResult(new BaseMessage()
                    {
                        Status = false,
                        ErrType = BaseErrType.PermissionNotEnough,
                        Message = "对不起，您没有操作此功能的权限"
                    });
                }
            }
        }

        // 校验功能权限
        private BaseMessage ValidateAuthorization(AuthorizationFilterContext context, List<CheckPermissionAttribute> attrs)
        {
            var msg = new BaseMessage();
            var controller = context.ActionDescriptor.RouteValues["controller"];
            var action = context.ActionDescriptor.RouteValues["action"];

            foreach (var attr in attrs)
            {
                controller = attr.Controller.IsNullOrEmpty() ? controller : attr.Controller;
                action = attr.Action.IsNullOrEmpty() ? action : attr.Action;
                msg.ErrType = _permCheckManager.ValidateAsync(new SysPermissionCheck()
                {
                    Controller = controller,
                    Action = action,

                }).Result;
                if (msg.ErrType == BaseErrType.Success)
                    break;
            }
            return msg;
        }

        /// <summary>
        /// 权限检测
        /// </summary>
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
        public class CheckPermissionAttribute : AuthorizeAttribute
        {
            public string Controller { get; set; }
            public string Action { get; set; }

        }
    }
}
