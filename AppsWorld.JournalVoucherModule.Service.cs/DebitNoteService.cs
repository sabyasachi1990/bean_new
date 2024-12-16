using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using FrameWork;
using AppsWorld.JournalVoucherModule.RepositoryPattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class DebitNoteService : Service<DebitNote>,IDebitNoteService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<DebitNote> _debitNoteRepository;

        public DebitNoteService(IJournalVoucherModuleRepositoryAsync<DebitNote> debitNoteRepository)
            : base(debitNoteRepository)
        {
            _debitNoteRepository = debitNoteRepository;
        }
    
    }
}
