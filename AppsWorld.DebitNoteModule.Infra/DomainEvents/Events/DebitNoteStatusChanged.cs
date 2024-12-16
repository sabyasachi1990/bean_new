using AppsWorld.DebitNoteModule.Models;
using Domain.Events;

namespace AppsWorld.DebitNoteModule.Infra
{
    public class DebitNoteStatusChanged : IDomainEvent
    {
        public DebitNoteModel DebitNoteModel { get; private set; }

		public DebitNoteStatusChanged(DebitNoteModel debitNoteModel)
        {
           DebitNoteModel=debitNoteModel;
        }
    }
}
