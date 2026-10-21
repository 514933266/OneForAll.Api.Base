using MediatR;
using OneForAll.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Base.Domain.Events
{
    /// <summary>
    /// 用户删除事件（批量）
    /// </summary>
    public class UserDeletedEvent : IDomainEvent, INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.Now;

        /// <summary>
        /// 被删除的用户id集合
        /// </summary>
        public IReadOnlyList<Guid> UserIds { get; }

        public UserDeletedEvent(IEnumerable<Guid> userIds)
        {
            UserIds = userIds == null ? new List<Guid>() : userIds.ToList();
        }
    }

    /// <summary>
    /// 密码重置事件（批量）
    /// </summary>
    public class UserPasswordResetEvent : IDomainEvent, INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.Now;

        /// <summary>
        /// 被重置密码的用户id集合
        /// </summary>
        public IReadOnlyList<Guid> UserIds { get; }

        public UserPasswordResetEvent(IEnumerable<Guid> userIds)
        {
            UserIds = userIds == null ? new List<Guid>() : userIds.ToList();
        }
    }

    /// <summary>
    /// 手机号变更事件（单个）
    /// </summary>
    public class UserMobileChangedEvent : IDomainEvent, INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.Now;

        /// <summary>
        /// 变更手机号的用户id
        /// </summary>
        public Guid UserId { get; }

        public UserMobileChangedEvent(Guid userId)
        {
            UserId = userId;
        }
    }

    /// <summary>
    /// 所属租户变更事件（单个；影响权限求交结果，须失效登录态缓存）
    /// </summary>
    public class UserTenantChangedEvent : IDomainEvent, INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();
        public DateTime OccurredOn { get; } = DateTime.Now;

        /// <summary>
        /// 变更所属租户的用户id
        /// </summary>
        public Guid UserId { get; }

        public UserTenantChangedEvent(Guid userId)
        {
            UserId = userId;
        }
    }
}
