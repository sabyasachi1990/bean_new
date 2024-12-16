using AppsWorld.CashSalesModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CashSalesModule.RepositoryPattern;
using AppsWorld.CashSalesModule.Entities.Models;

namespace AppsWorld.CashSalesModule.Service
{
    public class CashSalesDetailService:Service<CashSaleDetail>,ICashSalesDetailService
    {
        private readonly ICashSalesModuleRepositoryAsync<CashSaleDetail> _cashSaleRepository;
        public CashSalesDetailService(ICashSalesModuleRepositoryAsync<CashSaleDetail> cashSaleRepository):
            base(cashSaleRepository)
        {
            this._cashSaleRepository = cashSaleRepository;
        }

        public CashSaleDetail GetCashSaleDetailSale(Guid id)
        {
            return _cashSaleRepository.Query(a => a.Id == id).Select().FirstOrDefault();
        }

        public List<CashSaleDetail> GetCashSaleById(Guid CashSaleid)
        {
            return _cashSaleRepository.Query(a => a.CashSaleId == CashSaleid).Select().ToList();
        }
    }

}
