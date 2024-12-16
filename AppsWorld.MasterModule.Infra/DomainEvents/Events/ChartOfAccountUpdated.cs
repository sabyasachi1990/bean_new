using AppsWorld.MasterModule.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
    public class ChartOfAccountUpdated : IDomainEvent
    {
        public ChartOfAccount ChartOfAccount { get; private set; }

        public ChartOfAccountUpdated(ChartOfAccount chartOfAccount)
        {
            ChartOfAccount = chartOfAccount;
        }
    }
}
