using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class CreditNoteApplicationDetailService : Service<CreditNoteApplicationDetail>, ICreditNoteApplicationDetailService
    {
        private readonly IInvoiceModuleRepositoryAsync<CreditNoteApplicationDetail> _creditNoteApplicationDetailService;
        public CreditNoteApplicationDetailService(IInvoiceModuleRepositoryAsync<CreditNoteApplicationDetail> creditNoteApplicationDetailService)
            : base(creditNoteApplicationDetailService)
        {
            _creditNoteApplicationDetailService = creditNoteApplicationDetailService;
        }
        public List<CreditNoteApplicationDetail> GetById(Guid Id)
        {
            return _creditNoteApplicationDetailService.Query(c => c.DocumentId == Id && c.DocumentType == DocTypeConstants.Invoice && c.CreditAmount > 0).Select().ToList();
        }

        public List<CreditNoteApplicationDetail> GetCreditNoteDetail(Guid CreditNoteApplicationId)
        {
            return _creditNoteApplicationDetailService.Query(c => c.CreditNoteApplicationId == CreditNoteApplicationId && c.CreditAmount > 0).Select().ToList();
        }
        public List<CreditNoteApplicationDetail> GetCNADetailByInvoiceId(Guid Id, string docType)
        {
            return _creditNoteApplicationDetailService.Query(c => c.DocumentId == Id && c.DocumentType == docType && c.CreditAmount > 0)/*.Include(a => a.CreditNoteApplication.Invoice)*/.Select().ToList();
        }
    }
}
