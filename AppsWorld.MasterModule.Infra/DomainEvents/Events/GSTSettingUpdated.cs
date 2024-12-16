using AppsWorld.MasterModule.Entities;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class GSTSettingUpdated : IDomainEvent
    {
        public  GSTSetting GSTSetting { get; private set; }

        public GSTSettingUpdated(GSTSetting gSTSetting)
        {
            GSTSetting = gSTSetting;
        }
    }
}
