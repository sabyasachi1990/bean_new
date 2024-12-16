using AppsWorld.CommonModule.Infra;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Service
{
    public class CreditNoteApplicationService : Service<CreditNoteApplication>, ICreditNoteApplicationService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<CreditNoteApplication> _creditNoteApplicationService;
        public CreditNoteApplicationService(IDebitNoteMoluleRepositoryAsync<CreditNoteApplication> creditNoteApplicationService)
            : base(creditNoteApplicationService)
        {
            _creditNoteApplicationService = creditNoteApplicationService;
        }

        public CreditNoteApplication GetCreditNoteById(Guid Id)
        {
            return _creditNoteApplicationService.Query(c => c.Id == Id && c.Status != CreditNoteApplicationStatus.Void).Select().FirstOrDefault();
        }
    }
}
