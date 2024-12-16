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
    public class ReceiptDetailService : Service<ReceiptDetail>, IReceiptDetailService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<ReceiptDetail> _receiptDetailRepository;
        public ReceiptDetailService(IDebitNoteMoluleRepositoryAsync<ReceiptDetail> receiptDetailRepository)
            : base(receiptDetailRepository)
        {
            this._receiptDetailRepository = receiptDetailRepository;
        }
        public List<ReceiptDetail> lstDetails(Guid DocumentId)
        {
            return _receiptDetailRepository.Query(c => c.DocumentId == DocumentId && c.ReceiptAmount > 0).Select().ToList();
        }
    }
}
