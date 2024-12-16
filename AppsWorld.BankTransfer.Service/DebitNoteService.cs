using AppsWorld.BankTransferModule.Entities.Models;
using AppsWorld.BankTransferModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public class DebitNoteService : Service<DebitNote>, IDebitNoteService
    {
        private readonly IBankTransferModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        public DebitNoteService(IBankTransferModuleRepositoryAsync<DebitNote> debitNoteRepository) : base(debitNoteRepository)
        {
            this._debitNoteRepository = debitNoteRepository;
        }
        public List<DebitNote> GetListOfICDNBySEIdandEntId(long companyId, List<long?> lstServiceEntityIds, List<Guid> lstEntityIds, DateTime transferDate, string currency)
        {
            return _debitNoteRepository.Query(a => a.CompanyId == companyId && lstServiceEntityIds.Contains(a.ServiceCompanyId.Value) && lstEntityIds.Contains(a.EntityId) && a.DocDate <= transferDate && a.DocumentState != "Void" && a.Nature == "Interco" && a.DocCurrency == currency).Select().ToList();
        }
        public List<DebitNote> GetListOfDNsByCompanyIdAndDocId(long companyId, List<Guid> docIds)
        {
            return _debitNoteRepository.Query(a => a.CompanyId == companyId && docIds.Contains(a.Id) && a.DocumentState != "Void" && a.Nature=="Interco").Select().ToList();
        }
    }
}
