using AppsWorld.BankWithdrawalModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Infra
{
   public class CashPaymentUpdated:IDomainEvent
    {
       public WithdrawalModel CashPaymentsModel { get; private set; }
       public CashPaymentUpdated(WithdrawalModel cashPaymentsModel)
       {
           CashPaymentsModel = cashPaymentsModel;
       }
    }
}
