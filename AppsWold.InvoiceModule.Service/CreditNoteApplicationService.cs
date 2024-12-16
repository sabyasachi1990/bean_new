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
    public class CreditNoteApplicationService : Service<CreditNoteApplication>, ICreditNoteApplicationService
    {
        private readonly IInvoiceModuleRepositoryAsync<CreditNoteApplication> _creditNoteApplicationService;
        public CreditNoteApplicationService(IInvoiceModuleRepositoryAsync<CreditNoteApplication> creditNoteApplicationService)
            : base(creditNoteApplicationService)
        {
            _creditNoteApplicationService = creditNoteApplicationService;
        }

        public CreditNoteApplication GetCreditNoteById(Guid Id)
        {
            return _creditNoteApplicationService.Query(c => c.Id == Id && c.Status != CreditNoteApplicationStatus.Void).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetCreditNoteByIds(Guid Id)
        {
            return _creditNoteApplicationService.Query(e => e.Id == Id).Include(a => a.CreditNoteApplicationDetails).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetAllCreditNote(Guid creditNoteId, Guid cnApplicationId, long companyId)
        {
            return _creditNoteApplicationService.Query(c => c.InvoiceId == creditNoteId && c.Id == cnApplicationId && c.CompanyId == companyId).Include(c => c.CreditNoteApplicationDetails).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetCreditNoteByCompanyId(long companyId)
        {
            return _creditNoteApplicationService.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetAllCreditNoteByCompanyIdAndId(Guid id, Guid creditNoteId, long CompanyId)
        {
            return _creditNoteApplicationService.Query(c => c.InvoiceId == creditNoteId && c.Id == id && c.CompanyId == CompanyId).Select().FirstOrDefault();
        }
        public CreditNoteApplication GetCNAppById(Guid? CnId)
        {
            return _creditNoteApplicationService.Query(x => x.InvoiceId == CnId).Select().FirstOrDefault();
        }
        public List<CreditNoteApplication> GetListofCreditNoteById(List<Guid> Id)
        {
            return _creditNoteApplicationService.Query(c => Id.Contains(c.Id) /*&& c.Status != CreditNoteApplicationStatus.Void*/).Include(a => a.Invoice).Select().ToList();
        }
    }
}
