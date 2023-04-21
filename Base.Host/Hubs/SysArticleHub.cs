using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sys.Host.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class SysArticleHub : Hub
    {
        public SysArticleHub ()
        {

        }

        /// <summary>
        /// 客户端调用方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task SendMessage(string data)
        {
            //服务端返回是调用方法
            return Clients.All.SendAsync("ReceiveMessage", data);
        }

        public Task SendMessageToCaller(string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", message);
        }

        public Task SendMessageToGroup(string message)
        {
            return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", message);
        }
    }
}
