using AppsWorld.ReceiptModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.Events
{
    public class ReceiptCreated1:IDomainEvent
    {
        public ReceiptModel ReceiptModel { get; private set; }

        public ReceiptCreated1(ReceiptModel receiptModel)
        {
            ReceiptModel = receiptModel;
        }
    }
}
