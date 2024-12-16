using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
	public interface IFinancialSettingService : IService<FinancialSetting>
    {
		FinancialSetting GetFinancialSetting(long companyId);
		DateTime? GetFinancialOpenPeriodStarDate(long companyId);

		DateTime? GetFinancialOpenPeriodEndDate(long companyId);

		DateTime? GetFinancialYearEndLockDate(long companyId);

		bool ValidateYearEndLockDate(DateTime DocDate, long companyId);

		bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId);

		bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId);

    }
}
