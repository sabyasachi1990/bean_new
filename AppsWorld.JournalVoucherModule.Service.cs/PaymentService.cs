using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.Framework;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class PaymentService:Service<Payment>,IPaymentService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Payment> _paymentRepository;

        public PaymentService(IJournalVoucherModuleRepositoryAsync<Payment> paymentRepository)
            : base(paymentRepository)
		{
			_paymentRepository = paymentRepository;
		}
        public Payment GetPayment(Guid? id)
        {
            return _paymentRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
        
    }
}
