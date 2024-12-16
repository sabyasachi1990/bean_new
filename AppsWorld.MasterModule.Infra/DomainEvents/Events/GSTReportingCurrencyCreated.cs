
using AppsWorld.MasterModule.Entities;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class GSTReportingCurrencyCreated : IDomainEvent
    {
        public  GSTSetting GSTSetting { get; private set; }

        public GSTReportingCurrencyCreated(GSTSetting gSTSetting)
        {
            GSTSetting = gSTSetting;
        }
    }
}
