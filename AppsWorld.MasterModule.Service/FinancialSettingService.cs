using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Infrastructure;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppsWorld.MasterModule.Service
{
    public class FinancialSettingService : Service<FinancialSetting>, IFinancialSettingService
    {
        private readonly IMasterModuleRepositoryAsync<FinancialSetting> _financialSettingRepository;
        private readonly IMasterModuleRepositoryAsync<Journal> _journalRepository;
        private readonly IMasterModuleUnitOfWorkAsync _unitOfWorkAsync;
        public FinancialSettingService(IMasterModuleRepositoryAsync<FinancialSetting> financialSettingRepository, IMasterModuleRepositoryAsync<Journal> journalRepository, IMasterModuleUnitOfWorkAsync unitOfWorkAsync)
            : base(financialSettingRepository)
        {
            this._financialSettingRepository = financialSettingRepository;
            this._journalRepository = journalRepository;
            this._unitOfWorkAsync = unitOfWorkAsync;
        }
        public FinancialSetting GetFinancialNyCompanyId(long CompanyId)
        {
            return _financialSettingRepository.Query(a => a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public string GetFinancial(long CompanyId)
        {
            return _financialSettingRepository.Query(a => a.CompanyId == CompanyId).Select(x => x.BaseCurrency).FirstOrDefault();
        }

        public FinancialSetting GetFinancialByIdCId(long Id, long CompanyId)
        {
            return _financialSettingRepository.Query(e => e.Id == Id && e.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public DateTime? GetFinancialYearEndLockDate(long companyId)
        {
            FinancialSetting setting = GetFinancialNyCompanyId(companyId);
            return setting == null ? null : setting.EndOfYearLockDate;
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
        public bool ValidateYearEndLockDate(DateTime DocDate, long companyId)
        {
            DateTime? EndDate = GetFinancialYearEndLockDate(companyId);
            return EndDate == null ? true : DocDate >= EndDate.Value;
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
        public FinancialSetting GetFinancialSetting(long companyId)
        {
            FinancialSetting setting = GetFinancialNyCompanyId(companyId);
            try
            {
                if (setting != null)
                {
                    if (setting.IsPosted == null || setting.IsPosted == false)
                    {
                        var journal = _journalRepository.Queryable().Where(x => x.CompanyId == companyId).FirstOrDefault();
                        setting.IsPosted = journal != null ? true : false;
                        //	setting.IsPosted =  _journalEntryService.IsTransactionPosted(companyId);
                        if (setting.IsPosted.Value)
                        {
                            setting.ObjectState = ObjectState.Modified;
                            _financialSettingRepository.Update(setting);
                            try
                            {
                                _unitOfWorkAsync.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                    string[] obj = setting.FinancialYearEnd.Split("-".ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries);
                    setting.Date = obj[0];
                    setting.Month = obj[1];
                }

            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            return setting;
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
        public FinancialSetting VerifyFinancialLockPeriodPassword(string password, long companyId)
        {
            return _financialSettingRepository.Queryable().Where(t => t.PeriodLockDatePassword == password && t.CompanyId == companyId).FirstOrDefault();
        }

        public bool GetFinancialByCid(long companyid)
        {
            return _financialSettingRepository.Query(x => x.CompanyId == companyid && x.Status == RecordStatusEnum.Active).Select().Any();
        }
        public async Task<string> GetBaseCurrencyByCompanyId(long companyId)
        {
            return await Task.Run(()=> _financialSettingRepository.Query(a => a.CompanyId == companyId && a.Status == RecordStatusEnum.Active).Select(a => a.BaseCurrency).FirstOrDefault());
        }
    }
}
