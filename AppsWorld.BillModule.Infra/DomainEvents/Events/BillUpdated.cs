using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.BillModule.Models;

namespace AppsWorld.BillModule.Infra
{
    public class BillUpdated:IDomainEvent
    {
        public BillModel BillModel { get; private set; }
        public BillUpdated(BillModel billModel)
        {
            BillModel = billModel;
        }
    }
}
