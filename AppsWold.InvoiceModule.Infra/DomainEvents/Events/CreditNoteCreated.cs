using AppsWorld.InvoiceModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Infra
{
    public class CreditNoteCreated : IDomainEvent
    {
        public CreditNoteModel CreditNoteModel { get; private set; }

        public CreditNoteCreated(CreditNoteModel creditNoteModel)
        {
            CreditNoteModel = creditNoteModel;
        }
    }
}
