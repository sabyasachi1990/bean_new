
using AppsWorld.MasterModule.Entities;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class FinancialSettingCreated : IDomainEvent
    {
        public FinancialSetting FinancialSetting { get; private set; }

        public FinancialSettingCreated(FinancialSetting financialSetting)
        {
            FinancialSetting = financialSetting;
        }
    }
}
