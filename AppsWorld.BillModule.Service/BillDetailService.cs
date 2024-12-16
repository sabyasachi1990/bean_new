using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
    public class BillDetailService : Service<BillDetail>, IBillDetailService
    {
        private readonly IBillModuleRepositoryAsync<BillDetail> _billDetailRepository;
        private readonly IBillModuleRepositoryAsync<ReceiptDetailCompact> _receiptDetailRepository;
        public BillDetailService(IBillModuleRepositoryAsync<BillDetail> billDetailRepository, IBillModuleRepositoryAsync<ReceiptDetailCompact> receiptDetailRepository) : base(billDetailRepository)
        {
            _billDetailRepository = billDetailRepository;
            _receiptDetailRepository = receiptDetailRepository;
        }
        public List<BillDetail> GetAllBillDetailModel(Guid BillId)
        {
            return _billDetailRepository.Query(x => x.BillId == BillId).Select().ToList();
        }
        public BillDetail CreateBillDetail(Guid billId, Guid billDetailId)
        {
            return _billDetailRepository.Query(x => x.Id == billDetailId && x.BillId == billId).Select().FirstOrDefault();
        }
        public List<ReceiptDetailCompact> GetById(Guid receiptId)
        {
            return _receiptDetailRepository.Query(a => a.DocumentId == receiptId && a.ReceiptAmount > 0 && a.DocumentState != "Void").Include(x => x.Receipts).Select().ToList();
        }
    }
}
