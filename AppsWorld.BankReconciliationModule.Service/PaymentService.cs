using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
   public class PaymentService:Service<Payment>,IPaymentService
    {
       private readonly IBankReconciliationModuleRepositoryAsync<Payment> _paymentrepository;

       public PaymentService(IBankReconciliationModuleRepositoryAsync<Payment> paymentrepository)
           : base(paymentrepository)
        {
            _paymentrepository = paymentrepository;
        }
       public Payment GetPayment(Guid id, long companyid)
       {
           return _paymentrepository.Query(a => a.Id == id && a.CompanyId == companyid).Select().FirstOrDefault();
       }

    }
}
