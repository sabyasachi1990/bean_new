using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Models;

namespace Domain.Events.Model.Events
{
    public class BillCreated:IDomainEvent
    {
        public BillModel BillModel { get; private set; }

        public BillCreated(BillModel billModel)
        {
            BillModel = billModel;
        }
    }
}
