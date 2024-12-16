using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.Models;
using Domain.Events;

namespace AppsWorld.BankWithdrawalModule.Infra
{
    public class DepositCreated:IDomainEvent
    {
       public WithdrawalModel DepositModel { get; private set; }
       public DepositCreated(WithdrawalModel depositModel)
       {
           DepositModel = depositModel;
       }
    }
}
