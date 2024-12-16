using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.RepositoryPattern;
using Service.Pattern;

namespace AppsWorld.PaymentModule.Service
{
    public class PaymentDetailService : Service<PaymentDetail>, IPaymentDetailService
    {
        private readonly IPaymentModuleRepositoryAsync<PaymentDetail> _paymentDetailRepository;

        public PaymentDetailService(IPaymentModuleRepositoryAsync<PaymentDetail> paymentDetailRepository)
            : base(paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
        }
        public PaymentDetail GetPaymentDetail(Guid id, Guid paymenttId)
        {
            return _paymentDetailRepository.Query(c => c.Id == id && c.PaymentId == paymenttId).Select().FirstOrDefault();
        }
        public List<PaymentDetail> GetByPaymentId(Guid paymentId)
        {
            return _paymentDetailRepository.Query(c => c.PaymentId == paymentId).Select().ToList();
        }
        public List<PaymentDetail> GetByPaymentIdServiceId(Guid paymentId, long serviceComanyId, DateTime? docDate, string currency)
        {
            return _paymentDetailRepository.Query(c => c.PaymentId == paymentId && c.ServiceCompanyId == serviceComanyId && c.DocumentDate <= docDate && c.Currency == currency).Select().ToList();
        }
        public PaymentDetail GetPaymentById(Guid id)
        {
            return _paymentDetailRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
        public List<PaymentDetail> GetByPaymentDetailById(Guid paymentId, DateTime? docDate, string currency)
        {
            return _paymentDetailRepository.Query(c => c.PaymentId == paymentId && c.DocumentDate <= docDate && c.Currency == currency).Select().ToList();
        }
        public List<PaymentDetail> GetListOfPaymentDetails(List<Guid> paymentIds)
        {
            return _paymentDetailRepository.Query(x => paymentIds.Contains(x.Id)).Select().ToList();
        }
    }
}
