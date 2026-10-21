using OneForAll.Core.OAuth;

namespace Base.Domain.Interfaces
{
    /// <summary>
    /// 当前登录用户端口（领域层不感知 HTTP 上下文，由 Host 适配 Claims 实现）
    /// </summary>
    public interface ICurrentUser
    {
        /// <summary>
        /// 当前登录用户信息（未登录时各字段为空串/默认值）
        /// </summary>
        LoginUser LoginUser { get; }
    }
}
