
using AppsWorld.ReceiptModule.Entities.Models.V2.Receipt;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;
using System;
using System.Linq;

namespace AppsWorld.ReceiptModule.Service.V2.Receipt.K_Service
{
    public interface IReceiptKService : IService<ReceiptK>
    {
        IQueryable<ReceiptModelK> GetAllReceiptsK(string username, long companyId);
    }
}
