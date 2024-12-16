using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class CurrencyService : Service<Currency>, ICurrencyService
    {

        private readonly IMasterModuleRepositoryAsync<Currency> _currencyServiceRepository;
        public CurrencyService(IMasterModuleRepositoryAsync<Currency> currencyServiceRepository)
            : base(currencyServiceRepository)
        {
            this._currencyServiceRepository = currencyServiceRepository;
        }

        public LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode)
        {
            var lookUpCurrency = new LookUpCategory<string>();
            try
            {
                // Log.Logger.ZInfo(CurrencyLoggingValidation.Log_Currency_LU_GetByCurrencies_Request_Message);
                //var currencyOne = _currencyServiceRepository.Query(a => a.CompanyId == CompanyId && a.DefaultValue == CategoryCode).Select().FirstOrDefault();


                var currencyAll = _currencyServiceRepository.Query(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.DefaultValue == CategoryCode)).Select().ToList();

                if (currencyAll.Any(c => c.DefaultValue == CategoryCode))
                {
                    lookUpCurrency.CategoryName = CategoryCode;
                    lookUpCurrency.DefaultValue = CategoryCode;
                    lookUpCurrency.Lookups = currencyAll.Select(x => new LookUp<string>()
                    {
                        Code = x.Code,
                        Name = x.Name,
                        RecOrder = x.RecOrder
                    }).ToList();
                }
                //  Log.Logger.ZInfo(CurrencyLoggingValidation.Log_Currency_LU_GetByCurrencies_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //Log.Logger.ZInfo(CurrencyLoggingValidation.Log_Currency_LU_GetByCurrencies_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return lookUpCurrency;
        }


        public async Task<LookUpCategory<string>> GetByCurrenciesAsync(long CompanyId, string CategoryCode)
        {
            var lookUpCurrency = new LookUpCategory<string>();
            try
            {
                var currencyAll = await Task.Run(()=> _currencyServiceRepository.Query(a => a.CompanyId == CompanyId && (a.Status == AppsWorld.Framework.RecordStatusEnum.Active || a.DefaultValue == CategoryCode)).Select().ToList());

                if (currencyAll.Any(c => c.DefaultValue == CategoryCode))
                {
                    lookUpCurrency.CategoryName = CategoryCode;
                    lookUpCurrency.DefaultValue = CategoryCode;
                    lookUpCurrency.Lookups = currencyAll.Select(x => new LookUp<string>()
                    {
                        Code = x.Code,
                        Name = x.Name,
                        RecOrder = x.RecOrder
                    }).ToList();
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lookUpCurrency;
        }
        public async Task<LookUpCategory<string>> GetByCurrenciesByEntity(string currency, long companyId)
        {
            var lookUpCurrency = new LookUpCategory<string>();
            try
            {
               
                List<Currency> currencyAll = await Task.Run(()=> _currencyServiceRepository.Queryable().Where(a => a.CompanyId == companyId).ToList());
                Currency currencyFirst = currencyAll.Where(c => c.Code == currency && c.CompanyId == companyId).FirstOrDefault();
                currencyAll = currencyAll.Where(c => c.Status == RecordStatusEnum.Active).ToList();
                if (currencyFirst != null && !currencyAll.Any(c => c.DefaultCurrency == currencyFirst.DefaultCurrency))
                {
                    currencyAll.Add(currencyFirst);
                }

                if (currencyAll.Any())
                {
                    lookUpCurrency.CategoryName = currencyAll.Select(s => s.Code).FirstOrDefault();
                    lookUpCurrency.DefaultValue = currencyAll.Select(s => s.DefaultValue).FirstOrDefault();
                    lookUpCurrency.Lookups = currencyAll.Select(x => new LookUp<string>()
                    {
                        Code = x.Code,
                        Name = x.Name,
                        RecOrder = x.RecOrder
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            return lookUpCurrency;
        }

        public List<Currency> GetByCurrencyById(long companyId)
        {
            return _currencyServiceRepository.Queryable().Where(c => c.CompanyId == companyId && c.Status == RecordStatusEnum.Active).ToList();
        }

        public List<Currency> GetByCurrency(long companyId, string currencyCode)
        {
            return _currencyServiceRepository.Queryable().Where(c => c.CompanyId == companyId && (c.Status == RecordStatusEnum.Active || c.Code == currencyCode)).ToList();
        }

        public IQueryable<Currency> GetAllCurrency(long companyId)
        {
            //return
            //    _currencyServiceRepository.Queryable()
            //        .Where(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable).Select(d => new Currency
            //        {
            //            Id = d.Id,
            //            Code = d.Code,
            //            Name = d.Name,
            //            RecOrder = d.RecOrder,
            //            Status = d.Status,
            //            UserCreated = d.UserCreated,
            //            CreatedDate = d.CreatedDate,
            //            ModifiedBy = d.ModifiedBy,
            //            ModifiedDate = d.ModifiedDate,
            //            DefaultValue = d.DefaultValue,
            //            DefaultCurrency = "SGD",
            //            ObjectState = d.ObjectState
            //        })
            //        .OrderByDescending(a => a.Status).ThenBy(a => a.Name).AsQueryable();
            var curr = _currencyServiceRepository.Queryable().Where(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable).ToList();
            var currency = _currencyServiceRepository.Queryable().Where(c => c.CompanyId == 0 && c.Status < RecordStatusEnum.Disable).ToList();
            List<Currency> currencies = (from cur in currency
                                         join c in curr
                                         on cur.Code equals c.Code into eGrp
                                         from d in eGrp.DefaultIfEmpty()
                                         select new Currency
                                         {
                                             Id = d != null ? d.Id : cur.Id,
                                             Code = d != null ? d.Code : cur.Code,
                                             Name = d != null ? d.Name : cur.Name,
                                             RecOrder = d != null ? d.RecOrder : cur.RecOrder,
                                             Status = d != null ? d.Status : cur.Status,
                                             UserCreated = d != null ? d.UserCreated : cur.UserCreated,
                                             CreatedDate = d != null ? d.CreatedDate : cur.CreatedDate,
                                             ModifiedBy = d != null ? d.ModifiedBy : cur.ModifiedBy,
                                             ModifiedDate = d != null ? d.ModifiedDate : cur.ModifiedDate,
                                             DefaultValue = d != null ? d.DefaultValue : cur.DefaultValue
                                         }).ToList();
            return (from cur in currencies
                    select new Currency
                    {
                        Id = cur.Id,
                        Code = cur.Code,
                        Name = cur.Name,
                        RecOrder = cur.RecOrder,
                        Status = cur.Status,
                        UserCreated = cur.UserCreated,
                        CreatedDate = cur.CreatedDate,
                        ModifiedBy = cur.ModifiedBy,
                        ModifiedDate = cur.ModifiedDate,
                        DefaultValue = cur.DefaultValue,
                        DefaultCurrency = "SGD",
                        ObjectState = cur.ObjectState
                    }).OrderByDescending(a => a.Status).ThenBy(a => a.Name).AsQueryable();
            //List<Currency> currencys = new List<Currency>();
            //foreach (Currency currency in lstcurrencies)
            //{

            //    currency.DefaultCurrency = "SGD";

            //    currencys.Add(currency);
            //}
            //return currencys.OrderByDescending(a => a.Status).ThenBy(a => a.Name).AsQueryable();

        }
        public Currency GetById(long id)
        {
            return _currencyServiceRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }
        public Currency GetByIdandCompanyId(long id,long companyId)
        {
            return _currencyServiceRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
        }
    }
}
