using AppsWorld.BankWithdrawalModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Infra
{
    public class WithdrawalUpdated:IDomainEvent
    {
        public WithdrawalModel WithdrawalModel { get; private set; }
        public WithdrawalUpdated(WithdrawalModel withdrawalModel)
        {
            WithdrawalModel = withdrawalModel;
        }
    }
}
