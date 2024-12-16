using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.CreditMemoModule.Models;

namespace AppsWorld.CreditMemoModule.Infra
{
    public class CreditMemoCreated : IDomainEvent
    {
        public CreditMemoModel CreditMemoModel { get; private set; }
        public CreditMemoCreated(CreditMemoModel creditMemoModel)
        {
            CreditMemoModel = creditMemoModel;
        }
    }
}
