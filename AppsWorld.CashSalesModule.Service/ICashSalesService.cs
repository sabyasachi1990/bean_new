using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.CashSalesModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Service
{
    public interface ICashSalesService : IService<CashSale>
    {
        List<string> GetAutoNumber(long companyId);

        CashSale GetCashSaleDocNo(Guid id, string docNo, long companyId);

        CashSale CreateCashSales(long companyId, Guid id);

        CashSale GetAllCashSalesLUs(Guid cashsaleId, long companyId);

        IQueryable<CashSaleModelK> GetAllCashSalesK(string username, long companyId);

        List<CashSale> GetAllCashSale(long companyId);

        CashSale GetCashSaleByIdAndCompanyId(Guid id, long companyid);

        CashSale GetCashSaleLU(long companyId, Guid Id);

        CashSale GetDocTypeAndCompanyid(string DocType, long companyId);

        CashSale DuplicateCashsale(string DocNo, string docType, long companyId);

        CashSale GetCashsaleByCompanyId(long companyId);

        //to check wheather it is void or not
        bool IsVoid(long companyId, Guid id);
    }
}
