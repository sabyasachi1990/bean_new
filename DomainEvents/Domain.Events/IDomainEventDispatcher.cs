
namespace Domain.Events
{
    /// <summary>
    /// Provides access to a domain event dispatcher, responsible for 
    /// dispatching domain events to their subscribed handlers
    /// </summary>
    public interface IDomainEventDispatcher
    {
        /// <summary>
        /// Dispatches the specified event all subscribed handlers
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event to dispatch.</typeparam>
        /// <param name="event">The domain event to dispatch to all event handlers</param>
        void Dispatch<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IDomainEvent;
    }
}
