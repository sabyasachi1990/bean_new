using AppsWorld.DebitNoteModule.Models;
using Domain.Events;

namespace AppsWorld.DebitNoteModule.Infra
{
    public class DebitNoteCreated : IDomainEvent
    {
        public DebitNoteModel DebitNoteModel { get; private set; }

        public DebitNoteCreated(DebitNoteModel debitNoteModel)
        {
           DebitNoteModel=debitNoteModel;
        }
    }
}
