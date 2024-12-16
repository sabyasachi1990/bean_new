
using AppsWorld.BankReconciliationModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Infra
{
   public class BankReconciliationUpdated:IDomainEvent
    {
        public BankReconciliationModel BankReconciliationModel { get; private set; }

        public BankReconciliationUpdated(BankReconciliationModel bankReconciliationModel)
        {
            BankReconciliationModel = bankReconciliationModel;
        }
    }
}
