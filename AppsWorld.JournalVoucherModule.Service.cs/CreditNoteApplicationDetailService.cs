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

    public class CreditNoteApplicationDetailService:Service<CreditNoteApplicationDetail> ,ICreditNoteApplicationDetailService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<CreditNoteApplicationDetail> _creditNoteApplicationDetailRepository;

        public CreditNoteApplicationDetailService(IJournalVoucherModuleRepositoryAsync<CreditNoteApplicationDetail> creditNoteApplicationDetailRepository)
			: base(creditNoteApplicationDetailRepository)
        {
            this._creditNoteApplicationDetailRepository = creditNoteApplicationDetailRepository;
        }
        public List<CreditNoteApplicationDetail> GetApplicationDetails(Guid? id)
        {
            return _creditNoteApplicationDetailRepository.Query(x => x.DocumentId == id.Value && x.DocumentType == DocTypeConstants.Invoice).Select().ToList();
        }
    }
}
