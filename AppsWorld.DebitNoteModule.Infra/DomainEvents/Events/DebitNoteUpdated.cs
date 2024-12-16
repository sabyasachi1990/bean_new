using AppsWorld.DebitNoteModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Infra
{
    public class DebitNoteUpdated:IDomainEvent
    {
        public DebitNoteModel DebitNoteModel { get; private set; }

        public DebitNoteUpdated(DebitNoteModel debitNoteModel)
        {
            DebitNoteModel = debitNoteModel;
        }
        
    }
}
