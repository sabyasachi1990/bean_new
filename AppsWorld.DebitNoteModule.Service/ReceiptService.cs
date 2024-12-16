using AppsWorld.CommonModule.Infra;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Service
{
    public class ReceiptService : Service<Receipt>, IReceiptService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<Receipt> _receiptRepository;
        public ReceiptService(IDebitNoteMoluleRepositoryAsync<Receipt> receiptRepository)
            : base(receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }
        public Receipt GetReceipt(Guid id, long companyId)
        {
            return _receiptRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public Receipt GetReceiptByComapnyId(long companyId, string docType)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId && c.DocSubType == docType && c.DocumentState != "Void").Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public Receipt GetDuplicateReceipt(long companyId, string docType, string docNo)
        {
            return _receiptRepository.Query(x => x.CompanyId == companyId && x.DocSubType == docType && x.DocNo == docNo).Select().FirstOrDefault();
        }
        public Receipt GetLastCreatedReceipt(long companyId)
        {
            return _receiptRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
    }
}
