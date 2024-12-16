using AppsWorld.CashSalesModule.Entities.V2;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public interface ICashSalesService : IService<CashSale>
    {
        List<string> GetAutoNumber(long companyId);

        CashSale GetCashSaleDocNo(Guid id, string docNo, long companyId);

        CashSale CreateCashSales(long companyId, Guid id);

        CashSale GetAllCashSalesLUs(Guid cashsaleId, long companyId);

        List<CashSale> GetAllCashSale(long companyId);

        CashSale GetCashSaleByIdAndCompanyId(Guid id, long companyid);

        CashSale GetCashSaleLU(long companyId, Guid Id);

        CashSale GetDocTypeAndCompanyid(string DocType, long companyId);

        CashSale DuplicateCashsale(string DocNo, string docType, long companyId);

        DateTime GetCashsaleByCompanyId(long companyId);
        void CashSaleDetailInsert(CashSaleDetail detail);
        void CashSaleDetailUpdate(CashSaleDetail detail);
        Dictionary<DateTime?, DateTime> GetCashsaleCreatedDate(long companyId);
    }
}
