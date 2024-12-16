using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.Framework;
using AppsWorld.RevaluationModule.RepositoryPattern.V2;
using AppsWorld.RevaluationModule.Entities.V2;
using AppsWorld.CommonModule.Infra;
using AppsWorld.RevaluationModule.Infra;

namespace AppsWorld.RevaluationModule.Service.V2
{
    public class MasterCompactService : Service<BeanEntityCompact>, IMasterCompactService
    {
        readonly IRevaluationRepositoryAsync<BeanEntityCompact> _entityRepository;
        readonly IRevaluationRepositoryAsync<FinancialSettingCompact> _financialSettingsRepository;
        readonly IRevaluationRepositoryAsync<CompanyCompact> _companyRepository;
        readonly IRevaluationRepositoryAsync<CompanyUserCompact> _companyUserRepository;
        readonly IRevaluationRepositoryAsync<ChartOfAccountCompact> _chartOfAccountRepository;
        readonly IRevaluationRepositoryAsync<MultiCurrencySettingCompact> _multiCurrencyRepository;

        public MasterCompactService(IRevaluationRepositoryAsync<BeanEntityCompact> entityRepository, IRevaluationRepositoryAsync<FinancialSettingCompact> financialSettingsRepository, IRevaluationRepositoryAsync<CompanyCompact> companyRepository, IRevaluationRepositoryAsync<ChartOfAccountCompact> chartOfAccountRepository, IRevaluationRepositoryAsync<CompanyUserCompact> companyUserRepository, IRevaluationRepositoryAsync<MultiCurrencySettingCompact> multiCurrencyRepository)
            : base(entityRepository)
        {
            this._entityRepository = entityRepository;
            this._financialSettingsRepository = financialSettingsRepository;
            _companyRepository = companyRepository;
            this._chartOfAccountRepository = chartOfAccountRepository;
            this._companyUserRepository = companyUserRepository;
            _multiCurrencyRepository = multiCurrencyRepository;
        }
        public FinancialSettingCompact GetFinancialSetting(long companyId)
        {
            return _financialSettingsRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public bool ValidateYearEndLockDate(DateTime DocDate, long companyId)
        {
            DateTime? EndDate = GetFinancialYearEndLockDate(companyId);
            return EndDate == null ? true : DocDate >= EndDate.Value;
        }
        public DateTime? GetFinancialYearEndLockDate(long companyId)
        {
            FinancialSettingCompact setting = _financialSettingsRepository.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
            return setting == null ? null : setting.EndOfYearLockDate;
        }
        public bool ValidateFinancialOpenPeriod(DateTime DocDate, long companyId)
        {
            FinancialSettingCompact setting = GetFinancialSetting(companyId);
            if (setting == null)
            {
                throw new Exception(RevaluationConstant.The_Financial_setting_should_be_activated);
            }
            if (setting.PeriodLockDate != null && setting.PeriodEndDate != null)
                return DocDate.Date >= setting.PeriodLockDate && DocDate.Date <= setting.PeriodEndDate;
            else // There is no lock period setting
                return true;
        }
        public bool ValidateFinancialLockPeriodPassword(DateTime DocDate, string Password, long companyId)
        {
            if (!ValidateFinancialOpenPeriod(DocDate, companyId))
            {
                FinancialSettingCompact setting = GetFinancialSetting(companyId);
                return setting.PeriodLockDatePassword == Password;
            }
            return false;
        }
        public bool RevaluationPeriodLuck(DateTime DocDate, long companyId)
        {
            return _financialSettingsRepository.Query(c => c.PeriodLockDate != null && DocDate.Date >= c.PeriodLockDate && DocDate.Date <= c.PeriodEndDate && c.CompanyId == companyId).Select().FirstOrDefault() != null;
        }

        //public long GetChartOfAccountByNature(string nature, long CompanyId)
        //{
        //    return _chartOfAccountRepository.Query(a => a.Name == (nature == "Trade" ? COAConstants.AccountsReceivables : COAConstants.OtherReceivables) && a.CompanyId == CompanyId).Select(d => d.Id).FirstOrDefault();
        //}
        public Dictionary<long, string> GetCOAByName(long? companyId, string name)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == companyId && c.Name == name).Select(c => new { COAID = c.Id, Nature = c.Nature }).ToDictionary(COAID => COAID.COAID, Nature => Nature.Nature);
        }
        public string GetNameById(long Id)
        {
            return _companyRepository.Query(a => a.Id == Id).Select(a => a.ShortName).FirstOrDefault();
        }
        public List<ChartOfAccountCompact> GetAllRevalAccount(long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.CompanyId == companyId && (a.IsRevaluation == 1 || a.Revaluation == true)).Select().ToList();
        }
        public bool IsMultiCurrecySettings(long companyId)
        {
            return _multiCurrencyRepository.Query(c => c.CompanyId == companyId).Select(c => c.Id) != null;
        }
        public string GetBaseCurrency(long companyId)
        {
            return _financialSettingsRepository.Query(c => c.CompanyId == companyId).Select(c => c.BaseCurrency).FirstOrDefault();
        }
    }
}
