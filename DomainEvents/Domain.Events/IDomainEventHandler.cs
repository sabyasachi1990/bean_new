
namespace Domain.Events
{
    /// <summary>
    /// - Subscribes to and processes domain events synchronously
    /// </summary>
    /// <typeparam name="T">The type of domain event to handle</typeparam>
    public interface IDomainEventHandler<in T> where T : IDomainEvent
    {
        void When(T @event);
    }
}

