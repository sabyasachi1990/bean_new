using System;
using System.Collections.Generic;
using System.Linq;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities.V2;
using AppsWorld.DebitNoteModule.RepositoryPattern.V2;

namespace AppsWorld.DebitNoteModule.Service.V2
{
    public class DebitNoteService : Service<DebitNote>, IDebitNoteService
    {
        readonly IDebitNoteRepositoryAsync<DebitNote> _debitNoteRepository;
        readonly IDebitNoteRepositoryAsync<DebitNoteDetail> _debitNoteDetailRepository;
        readonly IDebitNoteRepositoryAsync<InvoiceCompact> _invoiceRepository;
        public DebitNoteService(IDebitNoteRepositoryAsync<DebitNote> debitNoteRepository, IDebitNoteRepositoryAsync<DebitNoteDetail> debitNoteDetailRepository, IDebitNoteRepositoryAsync<InvoiceCompact> invoiceRepository)
            : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
            _debitNoteDetailRepository = debitNoteDetailRepository;
            _invoiceRepository = invoiceRepository;
        }
        public DebitNote GetDebitNoteById(Guid id, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(x => x.DebitNoteDetails).Select().FirstOrDefault();
        }
        public DebitNote GetDebitNote(Guid id, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id == id && x.CompanyId == companyId).Include(x => x.DebitNoteDetails).Include(x => x.DebitNoteNotes).Select().FirstOrDefault();
        }
        public DateTime? GetDebitNoteCreatedDate(Guid id, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id == id && x.CompanyId == companyId).Select(x => x.CreatedDate).FirstOrDefault();
        }
        public Dictionary<DateTime, DateTime?> GetDocDate(long companyId)
        {
            return _debitNoteRepository.Query(e => e.CompanyId == companyId).Select(e => new { DocDate = e.DocDate, CreatedDate = e.CreatedDate }).OrderByDescending(a => a.CreatedDate).ToDictionary(docdate => docdate.DocDate, cdate => cdate.CreatedDate);
        }
        public DebitNote CreateDebitNoteForDocNo(long companyId)
        {
            return _debitNoteRepository.Query(x => x.CompanyId == companyId && x.DocumentState != "Void").Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public DebitNote GetDocNo(string docNo, long companyId)
        {
            return _debitNoteRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public DebitNote GetDebitNoteDocNo(Guid id, string docNo, long companyId)
        {
            return _debitNoteRepository.Query(x => x.Id != id && x.DocNo == docNo && x.CompanyId == companyId && x.DocumentState != "Void").Select().FirstOrDefault();
        }

        public List<DebitNote> GetAllDebitModel(long companyId)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId).Select().OrderByDescending(c => c.CreatedDate).ToList();
        }
        public DebitNote GetDebittNote(Guid id)
        {
            return _debitNoteRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
        public DateTime? GetDNLastPostedDate(long companyId)
        {
            return _debitNoteRepository.Query(e => e.CompanyId == companyId).Select().OrderByDescending(a => a.CreatedDate).Select(a => a.CreatedDate).FirstOrDefault();
        }
        public DebitNoteDetail GetDebitNoteDetail(Guid id, Guid debitNoteId)
        {
            return _debitNoteDetailRepository.Query(x => x.Id == id && x.DebitNoteId == debitNoteId).Select().FirstOrDefault();
        }
        public DateTime? GetInvoiceByCompany(long companyId, string docType)
        {
            return _invoiceRepository.Queryable().Where(x => x.CompanyId == companyId && x.DocType == docType).OrderByDescending(x => x.CreatedDate).Select(x => x.CreatedDate).FirstOrDefault();
        }
        public string GetCNDocDate(long companyId, string docType)
        {
            return _invoiceRepository.Queryable().Where(x => x.CompanyId == companyId && x.DocType == docType).OrderByDescending(x => x.CreatedDate).Select(x => x.DocNo).FirstOrDefault();
        }
        public string GetDuplicateInvoice(long companyId, string docType, string docNo)
        {
            return _invoiceRepository.Query(x => x.CompanyId == companyId && x.DocType == docType && x.DocNo == docNo).Select(x => x.DocNo).FirstOrDefault();
        }
    }
}
