using System;
using System.Collections.Generic;
using Service.Pattern;
using System.Linq;
using AppsWorld.DebitNoteModule.Entities;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IInvoiceService : IService<Invoice>
    {
        Invoice GetInvoice(Guid id);
        Invoice GetInvoiceByCompany(long companyId, string docType);
        Invoice GetDuplicateInvoice(long companyId, string docType, string docNo);
    }
}
