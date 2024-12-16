using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Service.Pattern;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.RevaluationModule.Service
{
    public class DebitNoteService:Service<DebitNote>,IDebitNoteService
    {
        private readonly IRevaluationModuleRepositoryAsync<DebitNote> _debitNoteRepository;
        public DebitNoteService(IRevaluationModuleRepositoryAsync<DebitNote> debitNoteRepository)
            : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
        }
        public List<DebitNote> lstDebits(long companyId)
        {
            return _debitNoteRepository.Query(c => c.CompanyId == companyId && (c.DocumentState == DebitNoteStates.NotPaid || c.DocumentState == DebitNoteStates.PartialPaid)).Select().ToList();
        }

    }
}
