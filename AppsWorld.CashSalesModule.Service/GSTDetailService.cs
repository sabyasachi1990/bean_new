using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.CashSalesModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Service
{
    //public class GSTDetailService : Service<GSTDetail>, IGSTDetailService
    //{
    //    private readonly ICashSalesModuleRepositoryAsync<GSTDetail> _cashSaleRepository;

    //    public GSTDetailService(ICashSalesModuleRepositoryAsync<GSTDetail> cashSaleRepository) :
    //        base(cashSaleRepository)
    //    {
    //        this._cashSaleRepository = cashSaleRepository;
    //    }
    //    public List<GSTDetail> GetAllGstDetail(Guid id)
    //    {
    //        return _cashSaleRepository.Query(x => x.DocId == id).Select().ToList();
    //    }

    //    public GSTDetail GetGSTById(Guid id)
    //    {
    //        return _cashSaleRepository.Queryable().Where(x => x.Id == id).FirstOrDefault();
    //    }
    //}
}
