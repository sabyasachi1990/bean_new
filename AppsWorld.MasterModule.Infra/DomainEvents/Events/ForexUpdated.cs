using AppsWorld.MasterModule.Models;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class ForexUpdated : IDomainEvent
    {
        public ForexModel ForexModel { get; private set; }

        public ForexUpdated(ForexModel forexModel)
        {
            ForexModel=forexModel;
        }
    }
}
