using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.InvoiceModule.Models;

namespace AppsWorld.InvoiceModule.Infra
{
    public class CreditNoteStatusChanged : IDomainEvent
    {
        public CreditNoteModel CreditNoteModel { get; private set; }

        public CreditNoteStatusChanged(CreditNoteModel creditNoteModel)
        {
            CreditNoteModel = creditNoteModel;
        }
    }
}

