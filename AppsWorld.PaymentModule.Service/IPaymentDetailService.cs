using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.PaymentModule.Entities;

namespace AppsWorld.PaymentModule.Service
{
    public interface IPaymentDetailService : IService<PaymentDetail>
    {
        PaymentDetail GetPaymentDetail(Guid id, Guid paymentId);
        List<PaymentDetail> GetByPaymentId(Guid paymentId);
        PaymentDetail GetPaymentById(Guid id);
        List<PaymentDetail> GetByPaymentIdServiceId(Guid paymentId, long serviceComanyId, DateTime? docDate, string currency);
        List<PaymentDetail> GetByPaymentDetailById(Guid paymentId, DateTime? docDate, string currency);
        List<PaymentDetail> GetListOfPaymentDetails(List<Guid> paymentIds);
    }
}
