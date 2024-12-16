using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.InvoiceModule.Models;

namespace AppsWorld.InvoiceModule.Infra
{
    public class DoubtfulDebtUpdated : IDomainEvent
    {
        public DoubtfulDebtModel DoubtfulDebtModel { get; private set; }

        public DoubtfulDebtUpdated(DoubtfulDebtModel doubtfulDebtModel)
        {
            DoubtfulDebtModel = doubtfulDebtModel;
        }
    }
}
