using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
    public interface IReceiptService : IService<Receipt>
    {
        List<Receipt> GetAllReceiptModel(long companyId);
        Receipt GetReceipt(Guid id, long companyId);

        Receipt CreateReceipt(long companyId);
        DateTime CreateReceiptNew(long companyId);
        Receipt GetReceipts(Guid id, long companyId);
        Receipt GetDocNo(string docNo, long companyId);
        Receipt CheckDocNo(Guid id, string docNo, long companyId);
        Receipt CheckReceiptById(Guid id);

        void UpdateReceipt(Receipt receipt);
        void InsertReceipt(Receipt receipt);
        IQueryable<ReceiptModelK> GetAllReceiptsK(string username, long companyId);
        Receipt GetReceiptById(Guid receiptId, Guid entityId, long companyId);
        List<string> GetAllBillByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetAllBillEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetAllCreditMemoByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetAllCreditMemoEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency);
        List<string> GetByBillId(Guid entityId, long companyId);
        List<string> GetByStateandBillEntity(Guid entityId, long companyId);
        List<string> GetByCreditMemoId(Guid entityId, long companyId);
        List<string> GetByStateandCreditMemoEntity(Guid entityId, long companyId);
        bool IsVoid(long companyId, Guid id);
        bool IsVoidNew(long companyId, Guid id);
        Task<Receipt> CreateReceiptAsync(long companyId);
        Task<Receipt> GetReceiptAsync(Guid id, long companyId);
    }
}
