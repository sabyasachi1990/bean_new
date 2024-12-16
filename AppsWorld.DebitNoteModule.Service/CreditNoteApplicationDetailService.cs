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
    public class CreditNoteApplicationDetailService : Service<CreditNoteApplicationDetail>, ICreditNoteApplicationDetailService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<CreditNoteApplicationDetail> _creditNoteApplicationDetailService;
        public CreditNoteApplicationDetailService(IDebitNoteMoluleRepositoryAsync<CreditNoteApplicationDetail> creditNoteApplicationDetailService)
            : base(creditNoteApplicationDetailService)
        {
            _creditNoteApplicationDetailService = creditNoteApplicationDetailService;
        }
        public List<CreditNoteApplicationDetail> GetById(Guid Id)
        {
            return _creditNoteApplicationDetailService.Query(c => c.DocumentId == Id && c.DocumentType == DocTypeConstants.DebitNote && c.CreditAmount > 0).Select().ToList();
        }
    }
}
