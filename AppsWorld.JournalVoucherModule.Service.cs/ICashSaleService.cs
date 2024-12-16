using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ICashSaleService : IService<CashSale>
    {
        CashSale GetCashSaleDetail(Guid cashSaleId);
        CashSale GetCashSaleId(Guid? Id);
        BankReconciliation GetBRByCOAID(long? coaId, long? serviceCompanyId, long? companyId);
    }
}
