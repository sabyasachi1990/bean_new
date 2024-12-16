using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;

namespace AppsWorld.DebitNoteModule.Service
{
    public class DebitNoteNoteService:Service<DebitNoteNote>,IDebitNoteNoteService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<DebitNoteNote> _debitNoteNoteRepository;
        public DebitNoteNoteService(IDebitNoteMoluleRepositoryAsync<DebitNoteNote> debitNoteNoteRepository):base(debitNoteNoteRepository)
        {
            _debitNoteNoteRepository = debitNoteNoteRepository;
        }
        public List<DebitNoteNote> AllDebitNoteNote(Guid debitNoteId)
        {
            return _debitNoteNoteRepository.Query(a => a.DebitNoteId == debitNoteId).Select().ToList();
        }
        public DebitNoteNote GetDebitNoteNote(Guid id,Guid debitNoteId)
        {
            return _debitNoteNoteRepository.Query(x => x.Id == id && x.DebitNoteId == debitNoteId).Select().FirstOrDefault();
        }
    }
}
