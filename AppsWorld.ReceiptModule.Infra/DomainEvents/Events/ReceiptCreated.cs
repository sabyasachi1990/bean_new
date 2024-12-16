using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorld.ReceiptModule.Models;

namespace AppsWorld.ReceiptModule.Infra
{
    public class ReceiptCreated:IDomainEvent
    {
        public ReceiptModel ReceiptModel { get; private set; }
        public ReceiptCreated(ReceiptModel receiptModel)
        {
            ReceiptModel = receiptModel;
        }
    }
}
