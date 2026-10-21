using Base.Domain.Events;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OneForAll.Core.Extension;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Base.Host.Events
{
    /// <summary>
    /// 登录态缓存失效订阅者：用户被删除/改密码/换手机号/切租户后，主动移除 LoginInfo:{userId} 缓存，
    /// 使下一次权限校验读库重建（消除"删用户后缓存仍放行"的既有隐患）。
    /// 横切职责从 Manager 上移至此，Manager 不再直接依赖 IDistributedCache。
    /// 领域事件（Base.Domain）在实现 Core.IDomainEvent 的同时实现 INotification，故本订阅者直接实现 INotificationHandler&lt;TEvent&gt;。
    /// </summary>
    public class UserLoginInfoCacheInvalidationHandler :
        INotificationHandler<UserDeletedEvent>,
        INotificationHandler<UserPasswordResetEvent>,
        INotificationHandler<UserMobileChangedEvent>,
        INotificationHandler<UserTenantChangedEvent>
    {
        // 登录态缓存键（与 QueryService/OAuth 保持一致）
        private readonly string LOGIN_CACHE_KEY = "LoginInfo:{0}";

        private readonly IDistributedCache _cacheRepository;

        public UserLoginInfoCacheInvalidationHandler(IDistributedCache cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
        {
            foreach (var userId in notification.UserIds)
                await InvalidateAsync(userId);
        }

        public async Task Handle(UserPasswordResetEvent notification, CancellationToken cancellationToken)
        {
            foreach (var userId in notification.UserIds)
                await InvalidateAsync(userId);
        }

        public Task Handle(UserMobileChangedEvent notification, CancellationToken cancellationToken)
        {
            return InvalidateAsync(notification.UserId);
        }

        public Task Handle(UserTenantChangedEvent notification, CancellationToken cancellationToken)
        {
            return InvalidateAsync(notification.UserId);
        }

        // 移除单个用户登录态缓存（Redis 不可用时静默降级，不影响主流程）
        private async Task InvalidateAsync(Guid userId)
        {
            try
            {
                await _cacheRepository.RemoveAsync(LOGIN_CACHE_KEY.Fmt(userId));
            }
            catch
            {
                // 缓存失效失败不阻断业务写操作
            }
        }
    }
}
