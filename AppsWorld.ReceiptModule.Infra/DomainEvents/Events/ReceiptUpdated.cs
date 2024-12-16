using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Models;
using Domain.Events;

namespace AppsWorld.ReceiptModule.Infra
{
    public class ReceiptUpdated:IDomainEvent
    {
        public ReceiptModel ReceiptModel { get; private set; }
        public ReceiptUpdated(ReceiptModel receiptModel)
        {
            ReceiptModel = receiptModel;
        }
    }
}
