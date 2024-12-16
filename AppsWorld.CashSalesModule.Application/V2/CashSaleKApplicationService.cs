using AppsWorld.CashSalesModule.Models;
using AppsWorld.CashSalesModule.Service.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Application.V2
{
    public class CashSaleKApplicationService
    {
        ICashSalesKService _cashSaleService;
        public CashSaleKApplicationService(ICashSalesKService cashSaleService)
        {
            this._cashSaleService = cashSaleService;
        }
        #region Kendo Call
        public IQueryable<CashSaleModelK> GetAllCashSalesK(string username, long companyId)
        {
            return _cashSaleService.GetAllCashSalesK(username, companyId);
        }
        #endregion
    }
}
