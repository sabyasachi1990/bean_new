using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;

namespace AppsWorld.RevaluationModule.Service
{
    public class JournalService : Service<Journal>, IJournalService
    {
        private readonly IRevaluationModuleRepositoryAsync<Journal> _journalRepository;
        public JournalService(IRevaluationModuleRepositoryAsync<Journal> journalRepository)
            : base(journalRepository)
        {
            _journalRepository = journalRepository;
        }
        public List<Journal> GetPostingJournal(DateTime dateTime, long companyId, long serviceCompanyId)
        {
            return _journalRepository.Query(x => x.DocDate <= dateTime && x.CompanyId == companyId && (x.DocumentState != "Fully Paid" && x.DocumentState != "Fully Applied" && x.DocumentState != "Void" && x.DocumentState != "Parked" && x.DocumentState != "Recurring") && x.ServiceCompanyId == serviceCompanyId && x.ClearingStatus != "Cleared" && (/*x.DocType != "Withdrawal" && x.DocType != "Cash Payment" && x.DocType != "Deposit" && x.DocType != "Transfer" && x.DocType != "Receipt" && x.DocType != "Payment" &&*/ x.DocSubType != "Revaluation" && x.DocSubType != "CM Application" && x.DocSubType != "Application")).Include(x => x.JournalDetails).Select().ToList();//&& (x.DocType == "Invoice" || x.DocType == "Debit Note" || x.DocType == "VendorBill" || x.DocType == "Bill" || x.DocType == "Journal")
        }
        public Journal GetJournal(long companyId, Guid documentId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId).Select().FirstOrDefault();
        }
        public List<Journal> GetAllJournal(long companyId, Guid documentId)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.DocumentId == documentId && x.DocumentState != "Parked").Include(c => c.JournalDetails).Select().ToList();
        }
        public Journal GetJournalById(long companyId, Guid id)
        {
            return _journalRepository.Query(x => x.CompanyId == companyId && x.Id == id).Select().FirstOrDefault();
        }
    }
}
