using AppsWorld.MasterModule.Entities;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class FinancialSettingUpdated : IDomainEvent
    {
        public FinancialSetting FinancialSetting { get; private set; }

        public FinancialSettingUpdated(FinancialSetting financialSetting)
        {
            FinancialSetting = financialSetting;
        }
    }
}
