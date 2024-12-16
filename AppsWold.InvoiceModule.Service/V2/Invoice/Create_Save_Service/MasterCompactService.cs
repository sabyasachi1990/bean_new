using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.Infra.Resources;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public class MasterCompactService : Service<BeanEntityCompact>, IMasterCompactService
    {
        private readonly IInvoiceComptModuleRepositoryAsync<BeanEntityCompact> _entityRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<FinancialSettingCompact> _financialSettingsRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<CompanyCompact> _companyRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<CompanyUserCompact> _companyUserRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<ChartOfAccountCompact> _chartOfAccountRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<TaxCodeCompact> _taxCodeRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<TermsOfPaymentCompact> _termsOfPaymentRepository;
        private readonly IInvoiceComptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> _companyUserDetailRepository;
        //private readonly IInvoiceComptModuleRepositoryAsync<AccountTypeCompact> _accountTypeRepository;

        public MasterCompactService(IInvoiceComptModuleRepositoryAsync<BeanEntityCompact> entityRepository, IInvoiceComptModuleRepositoryAsync<FinancialSettingCompact> financialSettingsRepository, IInvoiceComptModuleRepositoryAsync<CompanyCompact> companyRepository, IInvoiceComptModuleRepositoryAsync<ChartOfAccountCompact> chartOfAccountRepository, IInvoiceComptModuleRepositoryAsync<TaxCodeCompact> taxCodeRepository, IInvoiceComptModuleRepositoryAsync<CompanyUserCompact> companyUserRepository, IInvoiceComptModuleRepositoryAsync<TermsOfPaymentCompact> termsOfPaymentRepository, IInvoiceComptModuleRepositoryAsync<AppsWorld.CommonModule.Entities.Models.CompanyUserDetail> companyUserDetailRepository)
            : base(entityRepository)
        {
            this._entityRepository = entityRepository;
            this._financialSettingsRepository = financialSettingsRepository;
            _companyRepository = companyRepository;
            this._chartOfAccountRepository = chartOfAccountRepository;
            this._taxCodeRepository = taxCodeRepository;
            this._companyUserRepository = companyUserRepository;
            this._termsOfPaymentRepository = termsOfPaymentRepository;
            _companyUserDetailRepository = companyUserDetailRepository;
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
                throw new Exception(InvoiceConstants.No_Active_Finance_Setting_found);
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
        public Dictionary<long, string> GetReceivableAccounts(long CompanyId)
        {
            return _chartOfAccountRepository.Query(c => (c.Name == COANameConstants.AccountsReceivables || c.Name == COANameConstants.OtherReceivables) && c.CompanyId == CompanyId).Select(c => new { COAID = c.Id, Name = c.Name }).ToDictionary(COAID => COAID.COAID, Name => Name.Name);
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
        public long GetChartOfAccountByName(string name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => a.Name == name && a.CompanyId == companyId).Select(a => a.Id).FirstOrDefault();
        }
        public IDictionary<long, string> GetById(long id)
        {
            return _companyRepository.Query(c => c.Id == id).Select(c => new { ServiceCompanyId = c.Id, ShotName = c.ShortName }).ToDictionary(d => d.ServiceCompanyId, d => d.ShotName);
        }
        public IDictionary<long, string> GetAllCompaniesCode(List<long?> ids)
        {
            return _companyRepository.Query(c => ids.Contains(c.Id)).Select(c => new { ServiceCompanyId = c.Id, ShotName = c.ShortName }).ToDictionary(d => d.ServiceCompanyId, d => d.ShotName);
        }
        public IDictionary<long, string> GetAllSubCompanies(List<long?> Ids, string username, long companyId)
        {
            return (from c in _companyRepository.Query(c => Ids.Contains(c.Id)).Select().AsQueryable()
                    join cu in _companyUserRepository.Queryable() on c.ParentId equals cu.CompanyId
                    join cud in _companyUserDetailRepository.Queryable() on cu.Id equals cud.CompanyUserId
                    where c.Id == cud.ServiceEntityId
                    where c.ParentId == companyId && cu.Username == username
                    select c).ToList().Select(c => new { Ids = c.Id, Code = c.ShortName }).ToDictionary(Id => Id.Ids, nameof => nameof.Code);
        }
        public IDictionary<long, long?> GetAllICAccount(List<long?> ids)
        {
            return _chartOfAccountRepository.Query(c => ids.Contains(c.SubsidaryCompanyId) && c.Name.Contains("I/C")).Select(c => new { COAID = c.Id, ServiceEntityId = c.SubsidaryCompanyId }).ToDictionary(d => d.COAID, d => d.ServiceEntityId);
        }
        public IDictionary<double?, string> GetTermsOfPayment(long? id)
        {
            return _termsOfPaymentRepository.Query(a => a.Id == id).Select(c => new { TopValue = c.TOPValue, Name = c.Name }).ToDictionary(d => d.TopValue, d => d.Name);
        }

        public Dictionary<long, string> GetChartofAccounts(List<string> Name, long companyId)
        {
            return _chartOfAccountRepository.Query(a => Name.Contains(a.Name) && a.CompanyId == companyId).Select(a => new { a.Id, a.Name }).ToDictionary(t => t.Id, t => t.Name);
        }
    }
}
