using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.Models;
using Domain.Events;

namespace AppsWorld.BankWithdrawalModule.Infra
{
    public class DepositUpdated : IDomainEvent
    {
        public WithdrawalModel DepositModel { get; private set; }
        public DepositUpdated(WithdrawalModel depositModel)
        {
            DepositModel = depositModel;
        }
    }
}
