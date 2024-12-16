using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    //public class ForexService : Service<Forex>, IForexService
    //{
    //    private readonly IMasterModuleRepositoryAsync<Forex> _forexRepository;
    //    public ForexService(IMasterModuleRepositoryAsync<Forex> forexRepository)
    //        : base(forexRepository)
    //    {
    //        this._forexRepository = forexRepository;
    //    }
    //    public Forex GetForex(string DocumentCurrency, DateTime Documentdate, long CompanyId)
    //    {
    //        return _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && Documentdate >= a.DateFrom && Documentdate <= a.Dateto && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
    //    }
    //    public Forex GetForexbycId(string DocumentCurrency, long CompanyId, DateTime docDate)
    //    {
    //        //return _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active)
    //        //                .Select().OrderBy(b => b.DateFrom).FirstOrDefault();
    //        return _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "Base Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active && a.Dateto < docDate)
    //                       .Select().OrderByDescending(a => a.Dateto).FirstOrDefault();
    //    }
    //    public Forex GetByGSTCurrency(string DocumentCurrency, DateTime Documentdate, long CompanyId)
    //    {
    //        return _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "GST Currency" && a.CompanyId == CompanyId && (Documentdate >= a.DateFrom && Documentdate <= a.Dateto) && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
    //    }
    //    public Forex GetByGSTCurrency(string DocumentCurrency, long CompanyId, DateTime docDate)
    //    {
    //        //return _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "GST Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).Select().OrderByDescending(b => b.Dateto).FirstOrDefault();
    //        return _forexRepository.Query(a => a.Currency == DocumentCurrency && a.Type == "GST Currency" && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active && a.DateFrom < docDate).Select().OrderByDescending(a => a.Dateto).FirstOrDefault();
    //    }
    //    public Forex GetForex(long id, long companyId)
    //    {
    //        return _forexRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().FirstOrDefault();
    //    }
    //    public Forex GetForx(long companyId)
    //    {
    //        return _forexRepository.Query(x => x.CompanyId == companyId).Select().FirstOrDefault();
    //    }
    //    public List<Forex> GetForexs(long id, long companyId)
    //    {
    //        return _forexRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select().ToList();
    //    }
    //    public Forex GetByGSTCurr(string type, string currency, DateTime fromDate, DateTime toDate, long CompanyId)
    //    {
    //        return _forexRepository.Query(a => a.Type == type && a.Currency == currency && a.DateFrom == fromDate && a.Dateto == toDate && a.CompanyId == CompanyId && a.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
    //    }

    //    public List<Forex> GetAllForex(string type, long companyId, string currency)
    //    {
    //        return _forexRepository.Query(x => x.Type == type && x.CompanyId == companyId && x.Currency == currency && x.Status == RecordStatusEnum.Active).Select().ToList();
    //    }
    //    public List<Forex> GetForexAll(long companyId, string type)
    //    {
    //        return _forexRepository.Queryable().Where(c => c.CompanyId == companyId && c.Type == type).OrderByDescending(c => c.CreatedDate).AsEnumerable().ToList();
    //    }
    //    public Forex Forx(long id)
    //    {
    //        return _forexRepository.Query(x => x.Id == id).Select().FirstOrDefault();
    //    }
    //}
}
