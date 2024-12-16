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
    public class PaymentDetailService : Service<PaymentDetail>, IPaymentDetailService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<PaymentDetail> _paymentDetailRepository;

        public PaymentDetailService(IJournalVoucherModuleRepositoryAsync<PaymentDetail> paymentDetailRepository)
            : base(paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
        }
        public PaymentDetail GetPaymentDetail(Guid? id)
        {
            return _paymentDetailRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
    }
}
