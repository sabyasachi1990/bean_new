using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface ICreditNoteApplicationService : IService<CreditNoteApplication>
    {
        CreditNoteApplication GetCreditNoteById(Guid Id);
        CreditNoteApplication GetCreditNoteByIds(Guid Id);
        CreditNoteApplication GetAllCreditNote(Guid creditNoteId, Guid cnApplicationId, long companyId);
        CreditNoteApplication GetCreditNoteByCompanyId(long companyId);
        CreditNoteApplication GetAllCreditNoteByCompanyIdAndId(Guid id, Guid creditNoteId, long CompanyId);
        CreditNoteApplication GetCNAppById(Guid? CnId);
        List<CreditNoteApplication> GetListofCreditNoteById(List<Guid> Id);
    }
}
