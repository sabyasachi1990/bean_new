using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Entities.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Service
{
    public interface ICashSalesDetailService :IService<CashSaleDetail>
    {
        CashSaleDetail GetCashSaleDetailSale(Guid id);

        List<CashSaleDetail> GetCashSaleById(Guid CashSaleid);
    }
}
