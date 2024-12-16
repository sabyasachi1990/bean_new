using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.RepositoryPattern;
using AppsWorld.CommonModule.Models;
using AppsWorld.Framework;
using System.Net;
using Newtonsoft.Json;

namespace AppsWorld.CommonModule.Service
{
    public class FinancialSettingService : Service<FinancialSetting>, IFinancialSettingService
    {
        private readonly ICommonModuleRepositoryAsync<FinancialSetting> _financialSettingRepository;
        private string fsErrorMsg = string.Empty;
        public FinancialSettingService(ICommonModuleRepositoryAsync<FinancialSetting> financialSettingRepository, ICommonModuleRepositoryAsync<AppsWorld.CommonModule.Entities.GSTSetting> gSTSettingRepository)
            : base(financialSettingRepository)
        {
            _financialSettingRepository = financialSettingRepository;
        }
        public FinancialSetting GetFinancialSetting(long companyId)
        {
            return _financialSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public string GetFinancialSettingByCurrency(long companyId)
        {
            return _financialSettingRepository.Query(c => c.CompanyId == companyId).Select(x => x.BaseCurrency).FirstOrDefault();
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
            //FinancialSetting setting = GetFinancialSetting(companyId);
            FinancialSetting setting = _financialSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
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
                throw new Exception("Finance setting must be save");
            }

            if (setting.PeriodLockDate != null && setting.PeriodEndDate != null)
                return DocDate.Date >= setting.PeriodLockDate && DocDate.Date <= setting.PeriodEndDate;
            else if (setting.PeriodLockDate != null && setting.PeriodEndDate == null)
                return DocDate.Date >= setting.PeriodLockDate;
            else if (setting.PeriodLockDate == null && setting.PeriodEndDate != null)
                return DocDate.Date <= setting.PeriodEndDate;
            else
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
        public bool RevaluationPeriodLuck(DateTime DocDate, long companyId)
        {
            var financial = _financialSettingRepository.Query(c => DocDate.Date >= c.PeriodLockDate && DocDate.Date <= c.PeriodEndDate).Select().FirstOrDefault();
            return financial != null ? true : false;
        }
        public decimal? GetExRateInformation(string DocumentCurrency, DateTime? Documentdate, long CompanyId)
        {
            try
            {
                decimal? exchangeRate = null;
                string BaseCurrency = string.Empty;
                FinancialSetting financialSetting = _financialSettingRepository.Query(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
                BaseCurrency = financialSetting.BaseCurrency;

                if (BaseCurrency == DocumentCurrency)
                {
                    exchangeRate = 1.0000000000m;
                    //return exchangeRate;
                }
                else
                {
                    BeanForex forex = new BeanForex();
                    string GstBaseCurrency = string.Empty;
                    string date = Documentdate.Value.ToString("yyyy-MM-dd");
                    forex.DocumentDate = date;
                    forex.Provider = "Fixer";
                    var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + DocumentCurrency + "&symbols=" + BaseCurrency;
                    CurrencyModel currencyRates = _download_serialized_json_data<CurrencyModel>(url);
                    //if (currencyRates.Base == null)
                    //{
                    //    BeanForex forex1 = new BeanForex();
                    //    return forex1;
                    //}

                    //decimal sgdorusdValue;
                    //var value = currencyRates.Rates.TryGetValue(BaseCurrency, out sgdorusdValue);
                    /*forex.BaseUnitPerUSD*/
                    exchangeRate = currencyRates.Rates.Where(c => c.Key == BaseCurrency).Select(c => c.Value).FirstOrDefault();
                }

                return exchangeRate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static T _download_serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }

        public async Task<FinancialSetting> GetFinancialSettingAsync(long companyId)
        {
            return await Task.Run(() => _financialSettingRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault());
        }
      
    }
}
