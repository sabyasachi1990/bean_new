using AppsWorld.MasterModule.Entities;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
public class DeRegristationCreated:IDomainEvent
    {
        public GSTSetting gstSetting { get; private set; }

        public DeRegristationCreated(GSTSetting gstSetting)
        {
            this.gstSetting = gstSetting;
        }
    }
}