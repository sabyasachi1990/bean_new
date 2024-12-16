using System;
using AppsWorld.CashSalesModule.Entities.V2;
using AppsWorld.CashSalesModule.RepositoryPattern.V2;
using Service.Pattern;
using System.Linq;
using FrameWork;
using System.Collections.Generic;
using AppsWorld.Framework;
using AppsWorld.CashSalesModule.Infra;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public class MasterCompactService : Service<BeanEntityCompact>, IMasterCompactService
    {
        private readonly ICashSalesMasterRepositoryAsync<BeanEntityCompact> _entityRepository;
        private readonly ICashSalesMasterRepositoryAsync<FinancialSettingCompact> _financialSettingsRepository;
        private readonly ICashSalesMasterRepositoryAsync<ChartOfAccountCompact> _chartOfAccountRepository;
        private readonly ICashSalesMasterRepositoryAsync<CompanyK> _companyRepository;
        private readonly ICashSalesMasterRepositoryAsync<TaxCodeCompact> _taxCodeRepository;

        public MasterCompactService(ICashSalesMasterRepositoryAsync<BeanEntityCompact> entityRepository, ICashSalesMasterRepositoryAsync<FinancialSettingCompact> financialSettingsRepository, ICashSalesMasterRepositoryAsync<ChartOfAccountCompact> chartOfAccountRepository, ICashSalesMasterRepositoryAsync<CompanyK> companyRepository, ICashSalesMasterRepositoryAsync<TaxCodeCompact> taxCodeRepository)
            : base(entityRepository)
        {
            _entityRepository = entityRepository;
            _financialSettingsRepository = financialSettingsRepository;
            _chartOfAccountRepository = chartOfAccountRepository;
            _companyRepository = companyRepository;
            _taxCodeRepository = taxCodeRepository;
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
                throw new Exception(CashSaleValidation.No_Active_Finance_Setting_found);
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
        public string GetEntityName(Guid? id)
        {
            return _entityRepository.Query(c => c.Id == id).Select(d => d.Name).FirstOrDefault();
        }
        public Dictionary<long, string> GetCompanyByCompanyid(long companyId)
        {
            return _companyRepository.Query(a => a.Id == companyId).Select(a => new { Id = a.Id, ShotName = a.ShortName }).ToDictionary(d => d.Id, d => d.ShotName);
        }
        public long? GetTaxPaybleGstCOA(long? companyId, string name)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == companyId && c.Name == name).Select(c => c.Id).FirstOrDefault();
        }
        public long? GetCOAId(long? companyId, long? coaId)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == companyId && c.Id == coaId).Select(c => c.Id).FirstOrDefault();
        }
        public TaxCodeCompact GetTaxById(long taxId)
        {
            return _taxCodeRepository.Query(c => c.Id == taxId).Select().FirstOrDefault();
        }
    }
}
