using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using FrameWork;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class CreditNoteApplicationService:Service<CreditNoteApplication>,ICreditNoteApplicationService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<CreditNoteApplication> _creditNoteApplicationRepository;

        public CreditNoteApplicationService(IJournalVoucherModuleRepositoryAsync<CreditNoteApplication> creditNoteApplicationRepository)
			: base(creditNoteApplicationRepository)
        {
            _creditNoteApplicationRepository = creditNoteApplicationRepository;
        }
        public CreditNoteApplication GetAppication(Guid id)
        {
            return _creditNoteApplicationRepository.Query(x => x.Id == id && x.Status != CreditNoteApplicationStatus.Void).Select().FirstOrDefault();
        }
    }
}
