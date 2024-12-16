
using AppsWorld.MasterModule.Entities;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class FinancialYearEndChangedCreated : IDomainEvent
    {
        public FinancialSetting FinancialSetting { get; private set; }

        public FinancialYearEndChangedCreated(FinancialSetting financialSetting)
        {
            FinancialSetting = financialSetting;
        }
    }
}
