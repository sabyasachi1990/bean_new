using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public class PaymentDetailService : Service<PaymentDetail>, IPaymentDetailService
    {
        private readonly IBillModuleRepositoryAsync<PaymentDetail> _paymentDetailRepository;

        public PaymentDetailService(IBillModuleRepositoryAsync<PaymentDetail> paymentDetailRepository)
            : base(paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
        }
        public List<PaymentDetail> GetById(Guid bilid)
        {
            return _paymentDetailRepository.Query(a => a.DocumentId == bilid && a.PaymentAmount > 0 && a.DocumentState != "Void").Include(x => x.Payment).Select().ToList();
        }
        public PaymentDetail GetBillPaymentById(Guid paymentid)
        {
            return _paymentDetailRepository.Query(a => a.PaymentId == paymentid).Select().FirstOrDefault();
        }
    }
}
