using Base.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 微信相关读路径服务（从 Entity 读取，不含 DTO）
    /// </summary>
    public interface ISysWechatQueryService
    {
        /// <summary>
        /// 按系统用户id获取微信用户
        /// </summary>
        /// <param name="userId">系统用户id</param>
        /// <returns>微信用户，不存在返回 null</returns>
        Task<SysWechatUser> GetUserAsync(Guid userId);

        /// <summary>
        /// 按系统用户id获取微信公众号关注者
        /// </summary>
        /// <param name="userId">系统用户id</param>
        /// <returns>关注者，不存在返回 null</returns>
        Task<SysWechatGzhSubscriber> GetGzhSubscriberAsync(Guid userId);
    }
}
