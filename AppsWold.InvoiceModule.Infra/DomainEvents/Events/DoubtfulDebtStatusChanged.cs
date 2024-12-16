using AppsWorld.InvoiceModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AppsWorld.InvoiceModule.Infra
{
    public class DoubtfulDebtStatusChanged : IDomainEvent
    {
        public DoubtfulDebtModel DoubtfulDebtModel { get; private set; }

        public DoubtfulDebtStatusChanged(DoubtfulDebtModel doubtfulDebtModel)
        {
            DoubtfulDebtModel = doubtfulDebtModel;
        }
    }
}
