using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;

namespace AppsWorld.ReceiptModule.Service
{
	public class FinancialSettingService : Service<FinancialSetting>, IFinancialSettingService
    {
		private readonly IReceiptModuleRepositoryAsync<FinancialSetting> _financialSettingRepository;
		private string fsErrorMsg = "";
		public FinancialSettingService(IReceiptModuleRepositoryAsync<FinancialSetting> financialSettingRepository)
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
		private bool ValidateFainancialSetting(FinancialSetting financialSetting)
		{
			if (financialSetting.PeriodLockDate != null || financialSetting.PeriodEndDate != null)
			{
				if (financialSetting.PeriodLockDatePassword == null || financialSetting.PeriodLockDatePassword == "")
				{
					fsErrorMsg = "Period Lock Date Password is mandatory.";
					return false;
				}
			}
			if (financialSetting.PeriodEndDate < DateTime.UtcNow && (financialSetting.PeriodLockDatePassword == null || financialSetting.PeriodLockDatePassword == ""))
			{
				fsErrorMsg = "Period Lock Date Achieved, cannot be saved.";
				return false;
			}
			return true;
		}
		public bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId)
		{
			FinancialSetting setting = GetFinancialSetting(companyId);
			if (setting == null)
			{
				throw new Exception("No Active Finance Setting found");
			}
			if (setting.PeriodLockDate != null && setting.PeriodEndDate != null)
				return DocDate >= setting.PeriodLockDate && DocDate <= setting.PeriodEndDate;
			else // There is no lock period setting
				return true;
		}
		public bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId)
		{
			if (!ValidateFinancialOpenPeriod(DocDate, companyId))
			{
				FinancialSetting setting = GetFinancialSetting(companyId);
				return setting.PeriodLockDatePassword == Password;
			}
			return false;
		}

		public bool ValidateYearEndLockDate(DateTime DocDate, long companyId)
		{
			DateTime? EndDate = GetFinancialYearEndLockDate(companyId);
			return EndDate == null ? true : DocDate >= EndDate.Value;
		}

	}
}
