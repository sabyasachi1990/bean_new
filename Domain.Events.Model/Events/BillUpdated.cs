using AppsWorld.BillModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.Events
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
