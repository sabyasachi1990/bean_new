using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
//using AppsWorld.ReceiptModule.Models;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.ReceiptModule.Service
{
    public class ReceiptDetailService : Service<ReceiptDetail>, IReceiptDetailService
    {
        private readonly IReceiptModuleRepositoryAsync<ReceiptDetail> _receiptDetailRepository;
        private readonly IReceiptModuleRepositoryAsync<CreditMemoApplicationCompact> _creditMemoApplicationRepository;
        private readonly IReceiptModuleRepositoryAsync<CreditNoteApplicationCompact> _creditNoteApplicationRepository;

        public ReceiptDetailService(IReceiptModuleRepositoryAsync<ReceiptDetail> receiptDetailRepository, IReceiptModuleRepositoryAsync<CreditMemoApplicationCompact> creditMemoApplicationRepository, IReceiptModuleRepositoryAsync<CreditNoteApplicationCompact> creditNoteApplicationRepository)
            : base(receiptDetailRepository)
        {
            _receiptDetailRepository = receiptDetailRepository;
            _creditNoteApplicationRepository = creditNoteApplicationRepository;
            _creditMemoApplicationRepository = creditMemoApplicationRepository;
        }

        public ReceiptDetail GetReceiptDetail(Guid id, Guid receiptId)
        {
            return _receiptDetailRepository.Query(c => c.Id == id && c.ReceiptId == receiptId).Select().FirstOrDefault();
        }
        public List<ReceiptDetail> GetByReceiptId(Guid receiptId)
        {
            return _receiptDetailRepository.Query(c => c.ReceiptId == receiptId).Select().ToList();
        }
        public List<ReceiptDetail> GetByReceiptIdSerId(Guid receiptId, long? serviceId, DateTime? docDate, string currency)
        {
            return _receiptDetailRepository.Query(c => c.ReceiptId == receiptId && c.ServiceCompanyId == serviceId && c.DocumentDate <= docDate && c.Currency == currency).Select().ToList();
        }
        public List<ReceiptDetail> GetReceiptDetailById(Guid receiptId, DateTime? docDate, string currency)
        {
            return _receiptDetailRepository.Query(c => c.ReceiptId == receiptId && c.DocumentDate <= docDate && c.Currency == currency).Select().ToList();
        }
        public CreditNoteApplicationCompact GetCNApplicationByDocId(Guid detailId)
        {
            return _creditNoteApplicationRepository.Query(c => c.DocumentId == detailId && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
        public CreditMemoApplicationCompact GetCMApplicationByDocId(Guid detailId)
        {
            return _creditMemoApplicationRepository.Query(c => c.DocumentId == detailId && c.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
        }
    }
}
