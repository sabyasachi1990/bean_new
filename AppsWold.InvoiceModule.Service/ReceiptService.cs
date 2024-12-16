using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class ReceiptService : Service<Receipt>, IReceiptService
    {
        private readonly IInvoiceModuleRepositoryAsync<Receipt> _receiptRepository;
        public ReceiptService(IInvoiceModuleRepositoryAsync<Receipt> receiptRepository)
            : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }
        public Receipt GetReceipt(Guid id, long companyId)
        {
            return _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Receipt GetAllReceipts(long companyId)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public Receipt GetDocNo(string docNo, long companyId)
        {
            return _receiptRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public List<Receipt> GetAllReceiptByEntity(Guid? entityId)
        {
            return _receiptRepository.Queryable().Where(x => x.EntityId == entityId && x.DocumentState != ReceiptState.Void).ToList();
        }
    }
}
