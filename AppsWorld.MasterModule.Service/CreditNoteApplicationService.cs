using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Service
{
    public class CreditNoteApplicationService : Service<CreditNoteApplication>, ICreditNoteApplicationService
    {
        private readonly IMasterModuleRepositoryAsync<CreditNoteApplication> _creditNoteApplicationRepository;
        public CreditNoteApplicationService(IMasterModuleRepositoryAsync<CreditNoteApplication> creditNoteApplicationRepository)
           : base(creditNoteApplicationRepository)
        {
            _creditNoteApplicationRepository = creditNoteApplicationRepository;
        }
        public CreditNoteApplication GetCNAppById(Guid? CnId)
        {
            return _creditNoteApplicationRepository.Query(x => x.InvoiceId == CnId).Select().FirstOrDefault();
        }
    }
}
