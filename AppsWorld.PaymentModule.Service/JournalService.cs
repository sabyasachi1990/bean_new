using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Infra.Resources;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppsWorld.PaymentModule.Service
{
    public class JournalService:Service<Journal>,IJournalServices
    {
        private readonly IPaymentModuleRepositoryAsync<Journal> _journalRepository;
        public JournalService(IPaymentModuleRepositoryAsync<Journal> journalRepository):base(journalRepository)
        {
            this._journalRepository = journalRepository;
        }

        public Journal GetJournals(Guid documentId, long companyId)
        {
            return _journalRepository.Query(a => a.DocumentId == documentId && a.CompanyId == companyId).Include(e => e.JournalDetails).Select().FirstOrDefault();
        }
        public List<Journal> GetAllJournalsByDocId(List<Guid> journalIds,long companyId)
        {
            return _journalRepository.Query(c => c.CompanyId == companyId && c.DocType== PaymentsConstants.DocType && journalIds.Contains(c.DocumentId.Value)).Select().ToList();
        }
    }
}
