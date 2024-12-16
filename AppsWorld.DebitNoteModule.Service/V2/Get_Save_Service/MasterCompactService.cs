using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.Framework;
using AppsWorld.DebitNoteModule.RepositoryPattern.V2;
using AppsWorld.DebitNoteModule.Entities.V2;
using AppsWorld.CommonModule.Infra;
using AppsWorld.DebitNoteModule.Infra;

namespace AppsWorld.DebitNoteModule.Service.V2
{
    public class MasterCompactService : Service<BeanEntityCompact>, IMasterCompactService
    {
        private readonly IDebitNoteRepositoryAsync<BeanEntityCompact> _entityRepository;
        private readonly IDebitNoteRepositoryAsync<FinancialSettingCompact> _financialSettingsRepository;
        private readonly IDebitNoteRepositoryAsync<CompanyCompact> _companyRepository;
        private readonly IDebitNoteRepositoryAsync<CompanyUserCompact> _companyUserRepository;
        private readonly IDebitNoteRepositoryAsync<ChartOfAccountCompact> _chartOfAccountRepository;
        private readonly IDebitNoteRepositoryAsync<TaxCodeCompact> _taxCodeRepository;
        private readonly IDebitNoteRepositoryAsync<TermsOfPaymentCompact> _termsOfPaymentRepository;

        public MasterCompactService(IDebitNoteRepositoryAsync<BeanEntityCompact> entityRepository, IDebitNoteRepositoryAsync<FinancialSettingCompact> financialSettingsRepository, IDebitNoteRepositoryAsync<CompanyCompact> companyRepository, IDebitNoteRepositoryAsync<ChartOfAccountCompact> chartOfAccountRepository, IDebitNoteRepositoryAsync<TaxCodeCompact> taxCodeRepository, IDebitNoteRepositoryAsync<CompanyUserCompact> companyUserRepository, IDebitNoteRepositoryAsync<TermsOfPaymentCompact>  termsOfPaymentRepository)
            : base(entityRepository)
        {
            this._entityRepository = entityRepository;
            this._financialSettingsRepository = financialSettingsRepository;
            _companyRepository = companyRepository;
            this._chartOfAccountRepository = chartOfAccountRepository;
            this._taxCodeRepository = taxCodeRepository;
            this._companyUserRepository = companyUserRepository;
            _termsOfPaymentRepository = termsOfPaymentRepository;
        }
        public decimal? GetCteditLimitsValue(Guid id)
        {
            return _entityRepository.Query(c => c.Id == id).Select(d => d.CreditLimitValue).FirstOrDefault();
        }
        public string GetEntityName(Guid? id)
        {
            return _entityRepository.Query(c => c.Id == id).Select(d => d.Name).FirstOrDefault();
        }
        //public Dictionary<Guid, string> GetListOfEntity(long companyId, List<Guid?> entityId)
        //{
        //    return _entityRepository.Query(a => a.CompanyId == companyId && entityId.Contains(a.Id)).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        //}
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
                throw new Exception(DebitNoteValidation.No_active_financial_settings_found);
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
        public CompanyCompact GetCompanyByCompanyid(long companyId)
        {
            return _companyRepository.Query(a => a.Id == companyId).Select().FirstOrDefault();
        }
        public long GetChartOfAccountByNature(string nature, long CompanyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == (nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables) && a.CompanyId == CompanyId).Select(d => d.Id).FirstOrDefault();
        }
        public long? GetTaxPaybleGstCOA(long? companyId, string name)
        {
            return _chartOfAccountRepository.Query(c => c.CompanyId == companyId && c.Name == name).Select(c => c.Id).FirstOrDefault();
        }
        public string GetIdBy(long Id)
        {
            return _companyRepository.Query(a => a.Id == Id).Select(a => a.ShortName).FirstOrDefault();
        }
        //public double? GetTermsOfPaymentById(long? id)
        //{
        //    return _termsOfPaymentRepository.Query(a => a.Id == id).Select(c => c.TOPValue).FirstOrDefault();
        //}
        public List<TaxCodeCompact> GetTaxCodes(long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId).Select().ToList();
        }
        public TaxCodeCompact GetTaxById(long taxId)
        {
            return _taxCodeRepository.Query(c => c.Id == taxId).Select().FirstOrDefault();
        }
        public List<TaxCodeCompact> GetAllTaxById(List<long?> taxId)
        {
            return _taxCodeRepository.Queryable().Where(c => taxId.Contains(c.Id)).ToList();
        }
        public Dictionary<long, string> GetTaxCodes(List<long?> taxIds, long companyId)
        {
            return _taxCodeRepository.Query(a => a.CompanyId == companyId && taxIds.Contains(a.Id)).Select(c => new { c.Id, c.Code }).ToDictionary(c => c.Id, c => c.Code);
        }
        //public IQueryable<LookUp<string>> GetTermsOfPayment(long? id, long? companyId)
        //{
        //    return _termsOfPaymentRepository.Queryable().Where(a => (a.Status == RecordStatusEnum.Active || a.Id == id) && a.CompanyId == companyId && a.IsCustomer == true).Select(x => new LookUp<string>()
        //    {
        //        Name = x.Name,
        //        Id = x.Id,
        //        TOPValue = x.TOPValue,
        //        RecOrder = x.RecOrder
        //    }).OrderBy(c => c.TOPValue);
        //}
        public List<LookUpCompany<string>> GetSubCompany(string username, long companyId, long? subcompanyid)
        {
            return GetCompany(companyId, subcompanyid, username).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                isGstActivated = x.IsGstSetting
            }).ToList();
            //return lstCompany;
        }
        public List<CompanyCompact> GetCompany(long companyId, long? companyIdCheck, string username = null)
        {
            //string username = "lokanath@yopmail.com";
            return (from c in _companyRepository.Queryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    where (c.Status == RecordStatusEnum.Active || c.Id == companyIdCheck) && c.ParentId == companyId
                    && (cu.ServiceEntities != null ? cu.ServiceEntities.Contains(c.Id.ToString()) : true) && cu.Username == username
                    select c).ToList();

            //return _companyRepository.Queryable().Where(a => (a.Status == RecordStatusEnum.Active || a.Id == companyIdCheck) && a.ParentId == companyId).ToList();
        }
        //public List<AccountTypeCompact> GetAllAccounyTypeByName(long companyId, List<string> name)
        //{
        //    return _accountTypeRepository.Query(c => c.CompanyId == companyId && name.Contains(c.Name) /*&& c.Status == RecordStatusEnum.Active*/).Include(c => c.ChartOfAccounts).Select().ToList();
        //}
        public List<TaxCodeCompact> GetTaxAllCodes(long companyId, DateTime? date)
        {
            return _taxCodeRepository.Query(a => a.EffectiveFrom <= date && (a.EffectiveTo >= date || a.EffectiveTo == null) && (a.Status == RecordStatusEnum.Inactive || a.Status == RecordStatusEnum.Active) && a.CompanyId == 0).Select().ToList();
        }
        public ChartOfAccountCompact GetChartOfAccountByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == name && a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public IDictionary<long, string> GetById(long id)
        {
            return _companyRepository.Query(c => c.Id == id).Select(c => new { ServiceCompanyId = c.Id, ShotName = c.ShortName }).ToDictionary(d => d.ServiceCompanyId, d => d.ShotName);
        }
        public IDictionary<double?, string> GetTermsOfPayment(long? id)
        {
            return _termsOfPaymentRepository.Query(a => a.Id == id).Select(c => new { TopValue = c.TOPValue, Name = c.Name }).ToDictionary(d => d.TopValue, d => d.Name);
        }
        public Dictionary<string,decimal?> GetEntityDataById(Guid entityId)
        {
            return _entityRepository.Query(c => c.Id == entityId).Select(c => new { Name = c.Name, CLimits = c.CreditLimitValue }).ToDictionary(Name => Name.Name, Limit => Limit.CLimits);
        }
    }
}
