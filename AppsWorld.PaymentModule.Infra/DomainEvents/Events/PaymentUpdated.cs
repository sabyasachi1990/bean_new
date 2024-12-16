using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.PaymentModule.Models;
using Domain.Events;

namespace AppsWorld.PaymentModule.Infra 
{
    public class PaymentUpdated:IDomainEvent
    {
        public PaymentModel PaymentModel { get; private set; }
        public PaymentUpdated(PaymentModel paymentModel)
        {
            PaymentModel = paymentModel;
        }
    }
}
