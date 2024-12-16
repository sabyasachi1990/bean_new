using System;
using System.Collections.Generic;
using Service.Pattern;
using System.Linq;
using AppsWorld.DebitNoteModule.Entities;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IInvoiceDetailService : IService<InvoiceDetail>
    {
        InvoiceDetail GetInvoiceDetail(Guid id);
    }
}
