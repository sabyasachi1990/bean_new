using AppsWorld.MasterModule.Models;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class ForexCreated : IDomainEvent
    {
        public ForexModel ForexModel { get; private set; }

        public ForexCreated(ForexModel forexModel)
        {
            ForexModel = forexModel;
        }
    }
}
