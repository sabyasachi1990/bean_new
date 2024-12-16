using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Events.Autofac
{
    using global::Autofac;

    /// <summary>
    /// Responsible for dispatching domain events using an autofac container
    /// </summary>
    public class AutofacDomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly ILifetimeScope _container;

        public AutofacDomainEventDispatcher(ILifetimeScope container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;
        }

        /// <summary>
        /// Dispatches the specified domain event to all subscribers.
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event to raise.</typeparam>
        /// <param name="event">The domain event to raise.</param>
        public void Dispatch<TDomainEvent>(TDomainEvent @event) where TDomainEvent : IDomainEvent
        {
            var handlers = _container.Resolve<IEnumerable<IDomainEventHandler<TDomainEvent>>>();

            handlers.ToList().ForEach(x => x.When(@event));
        }
    }
}
