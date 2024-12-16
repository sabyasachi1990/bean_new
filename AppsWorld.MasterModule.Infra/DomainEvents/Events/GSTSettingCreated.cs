using AppsWorld.MasterModule.Entities;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class GSTSettingCreated : IDomainEvent
    {
        public  GSTSetting GSTSetting { get; private set; }

        public GSTSettingCreated(GSTSetting gSTSetting)
        {
            GSTSetting = gSTSetting;
        }
    }
}
