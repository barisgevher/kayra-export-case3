using MediatR;

namespace Shared.Domain.Events
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
        string EventType { get; }
    }
}