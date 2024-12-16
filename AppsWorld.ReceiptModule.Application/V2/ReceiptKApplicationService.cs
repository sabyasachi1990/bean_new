using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.Service.V2.Receipt.K_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Application.V2
{
    public class ReceiptKApplicationService
    {
        IReceiptKService _receiptService;
        public ReceiptKApplicationService(IReceiptKService receiptService)
        {
            this._receiptService = receiptService;
        }


        #region Kendo Grid Call
        public IQueryable<ReceiptModelK> GetAllReceiptsK(string username, long companyId)
        {
            return _receiptService.GetAllReceiptsK(username, companyId);
        }
        #endregion
    }
}
