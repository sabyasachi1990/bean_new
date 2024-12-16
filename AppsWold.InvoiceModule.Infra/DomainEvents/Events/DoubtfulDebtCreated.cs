using AppsWorld.InvoiceModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppsWorld.InvoiceModule.Infra
{
    public class DoubtfulDebtCreated : IDomainEvent
    {
        public DoubtfulDebtModel DoubtfulDebtModel { get; private set; }

        public DoubtfulDebtCreated(DoubtfulDebtModel doubtfulDebtModel)
        {
            DoubtfulDebtModel = doubtfulDebtModel;
        }
    }
}
