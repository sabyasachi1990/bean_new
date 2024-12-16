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
    public class ReceiptDetailService : Service<ReceiptDetail>, IReceiptDetailService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<ReceiptDetail> _receiptDetailrepository;

        public ReceiptDetailService(IJournalVoucherModuleRepositoryAsync<ReceiptDetail> receiptDetailrepository)
            : base(receiptDetailrepository)
        {
            _receiptDetailrepository = receiptDetailrepository;
        }
        public ReceiptDetail GetReceiptDetail(Guid? id)
        {
            return _receiptDetailrepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
    }
}
