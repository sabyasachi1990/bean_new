using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.MasterModule.Entities;
using Service.Pattern;
using AppsWorld.MasterModule.RepositoryPattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.MasterModule.Service
{
    public class DebitNoteService:Service<DebitNote>,IDebitNoteService
    {
        private readonly IMasterModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        public DebitNoteService(IMasterModuleRepositoryAsync<DebitNote> debitNoteRepository)
           : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
        }
        public List<DebitNote> GetDebitNoteByEntity(Guid? entityId )
        {
            return _debitNoteRepository.Queryable().Where(x => x.EntityId == entityId && x.DocumentState != InvoiceStates.Void).ToList();
        }
    }
}
