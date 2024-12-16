using AppsWorld.ReceiptModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.Events
{
  public  class ReceiptUpdated:IDomainEvent
    {
        public ReceiptModel ReceiptModel { get; private set; }

        public ReceiptUpdated(ReceiptModel receiptModel)
        {
            ReceiptModel = receiptModel;
        }
    }
}
