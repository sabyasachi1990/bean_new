using Service.Pattern;
using System;
using System.Linq;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.RevaluationModule.Entities;
using AppsWorld.RevaluationModule.Entities.Models;

namespace AppsWorld.RevaluationModule.Service
{
    public class FinancialSettingService : Service<FinancialSetting>, IFinancialSettingService
    {
        private readonly IRevaluationModuleRepositoryAsync<FinancialSetting> _financialSettingRepository;

        public FinancialSettingService(IRevaluationModuleRepositoryAsync<FinancialSetting> financialSettingRepository)
			: base(financialSettingRepository)
        {
			_financialSettingRepository = financialSettingRepository;
        }
		public FinancialSetting GetFinancialSetting(long companyId)
		{
			return _financialSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
		}

		public DateTime? GetFinancialOpenPeriodStarDate(long companyId)
		{
			FinancialSetting setting = GetFinancialSetting(companyId);
			return setting.PeriodLockDate;
		}
		public DateTime? GetFinancialOpenPeriodEndDate(long companyId)
		{
			FinancialSetting setting = GetFinancialSetting(companyId);
			return setting.PeriodEndDate;
		}

		public DateTime? GetFinancialYearEndLockDate(long companyId)
		{
			FinancialSetting setting = GetFinancialSetting(companyId);
			return setting == null ? null : setting.EndOfYearLockDate;
		}

        public bool ValidateFinancialOpenPeriod(DateTime PostDate, long companyId)
        {
            FinancialSetting setting = GetFinancialSetting(companyId);
            if (setting == null)
            {
                throw new Exception("No Active Finance Setting found");
            }
            if (setting.PeriodLockDate != null && setting.PeriodEndDate != null)
                return PostDate >= setting.PeriodLockDate && PostDate <= setting.PeriodEndDate;
            else // There is no lock period setting
                return true;
        }

        public bool ValidateFinancialLockPeriodPassword(DateTime PostDate, string Password, long companyId)
        {
            if (!ValidateFinancialOpenPeriod(PostDate, companyId))
            {
                FinancialSetting setting = GetFinancialSetting(companyId);
                return setting.PeriodLockDatePassword == Password;
            }
            return false;
        }

        public bool ValidateYearEndLockDate(DateTime PostDate, long companyId)
        {
            DateTime? EndDate = GetFinancialYearEndLockDate(companyId);
            return EndDate == null ? true : PostDate >= EndDate.Value;
        }
    }
}
