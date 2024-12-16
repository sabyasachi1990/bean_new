using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Infra;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.PaymentModule.Service
{
    public class DebitNoteCompactService : Service<DebitNoteCompact>, IDebitNoteCompactService
    {
        private readonly IPaymentModuleRepositoryAsync<DebitNoteCompact> _debitNoteCompactRepository;
        public DebitNoteCompactService(IPaymentModuleRepositoryAsync<DebitNoteCompact> debitNoteCompactRepository) : base(debitNoteCompactRepository)
        {
            this._debitNoteCompactRepository = debitNoteCompactRepository;
        }
        public List<DebitNoteCompact> GetListOfDebitNotes(long companyId, string doctype, Guid entityId, List<Guid> documentIds)
        {
            return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.DocSubType == doctype && a.EntityId == entityId && documentIds.Contains(a.Id)).Select().ToList();
        }
        public List<DebitNoteCompact> GetListOfDebitNoteWithOutInter(long companyId, long serviceCompanyId, Guid entityId, string currency, DateTime? docDate)
        {
            if (currency != null)
                return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.EntityId == entityId && a.DocCurrency == currency && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid")).Select().ToList();
            else
                return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.ServiceCompanyId == serviceCompanyId && a.EntityId == entityId && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid")).Select().ToList();
        }
        public List<DebitNoteCompact> GetListOfDebitNoteWithInter(long companyId, Guid entityId, string currency, DateTime? docDate)
        {
            if (currency != null)
                return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.EntityId == entityId && a.DocCurrency == currency && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid")).Select().ToList();
            else
                return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.EntityId == entityId && a.DocDate <= docDate && (a.DocumentState != "Void" && a.DocumentState != "Fully Paid")).Select().ToList();
        }
        public List<DebitNoteCompact> GetListOfDebitNotesByDocIds(long companyId, List<Guid> ids)
        {
            return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && ids.Contains(a.Id)).Select().ToList();
        }
        public List<DebitNoteCompact> GetlistDNByDocIds(List<Guid> docIds, string docCurrency, Guid entityId, long companyId)
        {
            return _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.DocCurrency == docCurrency && a.EntityId == entityId && docIds.Contains(a.Id)).Select().ToList();
        }
        public List<DebitNoteCompact> GetListOfDebitNote(long companyid, List<Guid> lstDocumentId) => _debitNoteCompactRepository.Query(a => a.CompanyId == companyid && lstDocumentId.Contains(a.Id)).Select().ToList();
        public decimal? GetDebitNoteCompact(long companyId, Guid documentId) => _debitNoteCompactRepository.Query(a => a.CompanyId == companyId && a.Id == documentId).Select(a => a.ExchangeRate).FirstOrDefault();
        public List<string> GetByEntityId(Guid entityId, long companyId)
        {
            return _debitNoteCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();

        }
        public List<string> GetByIdState(Guid entityId, long companyId)
        {
            return _debitNoteCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && c.DocumentState != "Void").Select(c => c.DocCurrency).ToList();

        }
        public List<string> GetAllDNByEntityId(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _debitNoteCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && (c.DocumentState == InvoiceState.NotPaid || c.DocumentState == InvoiceState.PartialPaid)).Select(c => c.DocCurrency).ToList();
        }
        public List<string> GetAllDNByEntityIdState(Guid entityId, long companyId, string baseCurrency, string bankCurrency)
        {
            return _debitNoteCompactRepository.Query(c => c.EntityId == entityId && c.CompanyId == companyId && (c.DocCurrency == baseCurrency || c.DocCurrency == bankCurrency) && c.DocumentState != InvoiceState.Void).Select(c => c.DocCurrency).ToList();
        }
    }
}
