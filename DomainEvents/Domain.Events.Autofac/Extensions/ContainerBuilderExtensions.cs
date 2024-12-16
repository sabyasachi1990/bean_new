
namespace Domain.Events.Autofac.Extensions
{
    using global::Autofac;

    public static class ContainerBuilderExtensions
    {
        /// <summary>
        /// Registers the specified TDomainEventHandler for the TDomainEvent
        /// </summary>
        /// <typeparam name="TDomainEvent">The type of the domain event for which the handler is registered.</typeparam>
        /// <typeparam name="TDomainEventHandler">The type of the domain event handler responsible for handling the domain event.</typeparam>
        /// <param name="containerBuilder">The autofac container builder.</param>
        public static void RegisterDomainEventHandler<TDomainEvent, TDomainEventHandler>(this ContainerBuilder containerBuilder)
            where TDomainEvent : IDomainEvent
            where TDomainEventHandler : IDomainEventHandler<TDomainEvent>
        {
            containerBuilder.RegisterType<TDomainEventHandler>().As<IDomainEventHandler<TDomainEvent>>();
        }
    }
}
