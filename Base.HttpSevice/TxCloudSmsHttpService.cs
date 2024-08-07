﻿using Base.HttpService.Models;
using Base.HttpService;
using Microsoft.AspNetCore.Http;
using Base.HttpService.Interfaces;
using OneForAll.Core;
using OneForAll.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace Base.HttpService
{
    /// <summary>
    /// 腾讯云短信发送
    /// </summary>
    public class TxCloudSmsHttpService : BaseHttpService, ITxCloudSmsHttpService
    {
        private readonly HttpServiceConfig _config;

        public TxCloudSmsHttpService(
            HttpServiceConfig config,
            IHttpContextAccessor httpContext,
            IHttpClientFactory httpClientFactory) : base(httpContext, httpClientFactory)
        {
            _config = config;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="signName">短信签名</param>
        /// <param name="content">短信内容</param>
        /// <param name="templateId">模板id</param>
        /// <param name="phoneNumber">手机号</param>
        /// <returns>登录结果</returns>
        public async Task<BaseMessage> SendAsync(string signName, string content, string templateId, string phoneNumber)
        {
            var data = new BaseMessage();
            var client = GetHttpClient(_config.TxCloudSms);
            if (client != null && client.BaseAddress != null)
            {
                var code = "OneForAll.Base";
                var sign = (code + DateTime.Now.ToString("yyyyMMddhhmm")).ToMd5();
                var result = await client.PostAsync(client.BaseAddress, new TxCloudSmsRequest()
                {
                    MoudleCode = code,
                    MoudleName = "系统管理",
                    SignName = signName,
                    Content = content,
                    TemplateId = templateId,
                    PhoneNumber = phoneNumber,
                    Sign = sign
                }, new JsonMediaTypeFormatter());
                data = await result.Content.ReadAsAsync<BaseMessage>();

                if (!data.Status)
                {
                    data.Message = $"发送短信失败：{data.Message}";
                }
            }
            return data;
        }
    }
}
