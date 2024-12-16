using AppsWorld.BankTransferModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Infra
{
    public class BankTransferCreated: IDomainEvent
    {
        public BankTransferModel BankTransferModel { get; private set; }
        public BankTransferCreated(BankTransferModel bankTransferModel)
        {
            BankTransferModel = bankTransferModel;
        }
    }
}
