using AppsWorld.CashSalesModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Infra
{
    public class CashSaleCreated : IDomainEvent
    {
       public CashSaleModel CashSaleModel { get; private set; }
       public CashSaleCreated(CashSaleModel cashsaleModel)
       {
           CashSaleModel = cashsaleModel;
       }
    }
}
