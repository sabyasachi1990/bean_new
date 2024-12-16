using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.MasterModule.Entities;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
   public class ItemCreated:IDomainEvent
    {
       public Item Item { get; private set; }

       public ItemCreated(Item item)
       {
           Item = item;
       }
    }
}
