using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Entities.V2;
using AppsWorld.CashSalesModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Service.V2
{
    public interface ICashSalesKService:IService<CashSaleK>
    {
        IQueryable<CashSaleModelK> GetAllCashSalesK(string username, long companyId);
    }
}
