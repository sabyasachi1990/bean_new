using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class ReciptService : Service<Receipt>, IReceiptService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Receipt> _receipterepository;

        public ReciptService(IJournalVoucherModuleRepositoryAsync<Receipt> receipterepository)
            : base(receipterepository)
        {
            this._receipterepository = receipterepository;
        }
        public Receipt GetReciprDetail(Guid? reciptId)
        {
            return _receipterepository.Query(x => x.Id == reciptId).Select().FirstOrDefault();
        }
    }
}
