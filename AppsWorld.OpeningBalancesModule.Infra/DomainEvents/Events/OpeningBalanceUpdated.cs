using AppsWorld.OpeningBalancesModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Infra
{
    public class OpeningBalanceUpdated : IDomainEvent
    {
        public OpeningBalanceModel OpeningBalanceModel { get; private set; }
        public OpeningBalanceUpdated(OpeningBalanceModel openingBalanceModel)
        {
            OpeningBalanceModel = openingBalanceModel;
        }

    }
}
