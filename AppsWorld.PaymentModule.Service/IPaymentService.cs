using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
namespace AppsWorld.PaymentModule.Service
{
    public interface IPaymentService : IService<Payment>
    {
        Payment GetPayment(Guid id, long companyId);
        List<Payment> GetAllPaymentModel(long companyId);
        Payment CreatePayment(long companyId, string docType);
        Payment GetPayments(Guid id, long companyId, string docType);
        Payment GetDocNo(string docNo, long companyId);
        Payment CheckDocNo(Guid id, string docNo, long companyId, string docType);
        Payment CheckPaymentById(Guid id, string docType);
        Payment CreatePaymentDocNo(long companyId, string docType);
        void UpdatePayment(Payment payment);
        void InsertPayment(Payment payment);
        IQueryable<PaymentModelK> GetAllPaymentK(string username, long companyId, string docType);
        Payment GetPaymentsById(Guid id, long companyId, string docType, Guid entityId);

        bool? CheckDocumentState(Guid id, string docType);
    }
}
