using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.DebitNoteModule.Entities;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IDebitNoteNoteService:IService<DebitNoteNote>
    {
        List<DebitNoteNote> AllDebitNoteNote(Guid debitNoteId);
        DebitNoteNote GetDebitNoteNote(Guid id, Guid debitNoteId);
    }
}
