using AppsWorld.CreditMemoModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Infra
{
    public class CreditMemoDocStatusChanged : IDomainEvent
    {
        public CreditMemoModel CreditMemoModel { get; private set; }

        public CreditMemoDocStatusChanged(CreditMemoModel creditMemoModel)
        {
            CreditMemoModel = creditMemoModel;
        }
    }
}
