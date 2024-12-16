using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.PaymentModule.Models;

namespace AppsWorld.PaymentModule.Infra
{
    public class PaymentDocVoidUpdated:IDomainEvent
    {
        public PaymentModel PaymentModel { get; private set; }
        public PaymentDocVoidUpdated(PaymentModel paymentModel)
        {
            PaymentModel = paymentModel;
        }
    }
}
