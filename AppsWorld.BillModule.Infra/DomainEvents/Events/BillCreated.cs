using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Models;
using Domain.Events;

namespace AppsWorld.BillModule.Infra
{
    public class BillCreated:IDomainEvent
    {
        public BillModel BillModel { get; private set; }
        public BillCreated(BillModel billModule)
        {
            BillModel = billModule;
        }
    }
}
