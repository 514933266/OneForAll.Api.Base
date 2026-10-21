using OneForAll.Core.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Base.Host.Events
{
    /// <summary>
    /// IEventDispatcher 端口的 MediatR 实现：IDomainEvent 为 OneForAll.Core 的纯抽象（不绑事件总线），
    /// 具体领域事件在 Base.Domain 中同时实现 INotification，故直接交由 IPublisher 按运行时类型
    /// 派发到对应 INotificationHandler&lt;TEvent&gt;，无需包装/桥接。
    /// </summary>
    public class DomainEventDispatcher : IEventDispatcher
    {
        private readonly IPublisher _publisher;

        public DomainEventDispatcher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        /// <summary>
        /// 发布领域事件（同步顺序执行所有匹配的订阅者）
        /// </summary>
        public async Task PublishAsync(IDomainEvent domainEvent)
        {
            if (domainEvent == null)
                return;

            // 强制绑定 Publish(object) 非泛型重载，令 MediatR 依运行时类型解析对应 INotificationHandler<>；
            // IDomainEvent 静态类型并非 INotification，必须走非泛型重载按具体事件类型派发。
            await _publisher.Publish((object)domainEvent, CancellationToken.None);
        }
    }
}
