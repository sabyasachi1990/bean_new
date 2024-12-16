
namespace Domain.Events
{
    /// <summary>
    /// Responsible for synchronously raising domain events.
    /// </summary>
    public static class DomainEventChannel
    {
        /// <summary>
        /// Gets or sets the domain event dispatcher implementation to use.
        /// </summary>
        /// <value>The domain event dispatcher implementation.</value>
        public static IDomainEventDispatcher Dispatcher { get; set; }

        /// <summary>
        /// Raises the specified domain event via the configured Dispatcher.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of domain event being raised</typeparam>
        /// <param name="event">The domain event to raise via the dispatcher.</param>
        public static void Raise<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IDomainEvent
        {
            if (Dispatcher == null) return;

            Dispatcher.Dispatch(@event);
        }
    }
}
