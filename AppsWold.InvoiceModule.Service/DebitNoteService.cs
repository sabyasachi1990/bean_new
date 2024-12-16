using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.Models;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class DebitNoteService : Service<DebitNote>, IDebitNoteService
    {
        private readonly IInvoiceModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        public DebitNoteService(IInvoiceModuleRepositoryAsync<DebitNote> debitNoteRepository)
            : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
        }
        public List<DebitNote> GetTaggedDebitNotesByCustomerAndCurrency(Guid customerId, string currency, long companyId)
        {
            return _debitNoteRepository.Query(a => a.EntityId == customerId && a.DocCurrency == currency && a.CompanyId == companyId && a.AllocatedAmount > 0).Select().ToList();
        }
        public DebitNote GetDebitNoteById(Guid Id)
        {
            return _debitNoteRepository.Query(c => c.Id == Id).Include(x => x.DebitNoteDetails).Select().FirstOrDefault(); ;
        }
        public List<DebitNote> GetAllDebitNoteByIdAndNature(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime docdate,string Nature)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == EntityId && c.DocCurrency == DocCurrency && c.Nature==Nature&&c.DocDate <= docdate && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == DebitNoteStates.NotPaid || c.DocumentState == DebitNoteStates.PartialPaid)).Select().ToList();
        }
        public List<DebitNote> GetAllDebitNoteById(long companyId, Guid EntityId, string DocCurrency, long ServiceCompanyId, DateTime docdate)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId && c.EntityId == EntityId && c.DocCurrency == DocCurrency &&c.DocDate <= docdate && c.ServiceCompanyId == ServiceCompanyId && (c.DocumentState == DebitNoteStates.NotPaid || c.DocumentState == DebitNoteStates.PartialPaid)).Select().ToList();
        }
        public DebitNote GetAllDebitNoteById(Guid Id)
        {
            return _debitNoteRepository.Query(e => e.Id == Id).Include(a => a.DebitNoteDetails).Include(b => b.DebitNoteGSTDetails).Include(c => c.DebitNoteNotes).Select().FirstOrDefault();
        }
        public List<DebitNote> GetDebitNoteByEntity(Guid? entityId)
        {
            return _debitNoteRepository.Queryable().Where(x => x.EntityId == entityId && x.DocumentState != InvoiceStates.Void).ToList();
        }
        public List<DebitNote> GetDDByDebitNoteId(List<Guid> Ids)
        {
            return _debitNoteRepository.Queryable().Where(c => Ids.Contains(c.Id)).ToList();
        }
        public decimal? GetBalanceAmount(long companyId, Guid Id)
        {
            return _debitNoteRepository.Query(a => a.CompanyId == companyId && a.Id == Id).Select(a => a.BalanceAmount).FirstOrDefault();
        }
        public bool IsVoid(long companyId, Guid id)
        {
            return _debitNoteRepository.Query(a => a.CompanyId == companyId && a.Id == id).Select(a => a.DocumentState == InvoiceStates.Void).FirstOrDefault() == true;
        }
        public List<string> GetDNStatusByIds(List<Guid> Ids)
        {
            return _debitNoteRepository.Queryable().Where(c => Ids.Contains(c.Id)).Select(c => c.DocumentState).ToList();
        }
    }
}
