using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface ICreditNoteApplicationDetailService:IService<CreditNoteApplicationDetail>
    {
        List<CreditNoteApplicationDetail> GetById(Guid Id);
        List<CreditNoteApplicationDetail> GetCreditNoteDetail(Guid CreditNoteApplicationId);
        List<CreditNoteApplicationDetail> GetCNADetailByInvoiceId(Guid Id, string docType);


    }
}
