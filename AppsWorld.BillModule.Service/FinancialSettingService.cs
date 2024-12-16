using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.BillModule.RepositoryPattern;

namespace AppsWorld.BillModule.Service
{
	public class FinancialSettingService : Service<FinancialSetting>, IFinancialSettingService
    {
        private readonly IBillModuleRepositoryAsync<FinancialSetting> _financialSettingRepository;

        public FinancialSettingService(IBillModuleRepositoryAsync<FinancialSetting> financialSettingRepository)
			: base(financialSettingRepository)
        {
			_financialSettingRepository = financialSettingRepository;
        }
		public FinancialSetting GetFinancialSetting(long companyId)
		{
			return _financialSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
		}
        public Tuple<DateTime? , DateTime?,string> GetFinancialSettingNew(long companyId)
        {
            var financialSetting = _financialSettingRepository
                .Query(c => c.CompanyId == companyId)
                .Select(c => new { c.PeriodLockDate, c.PeriodEndDate,c.PeriodLockDatePassword })
                .FirstOrDefault();
            if (financialSetting == null)
            {
                return new Tuple<DateTime?, DateTime?, string>(null, null, null);
            }
            return new Tuple<DateTime?, DateTime?,string>(financialSetting.PeriodLockDate, financialSetting.PeriodEndDate,financialSetting.PeriodLockDatePassword);
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
