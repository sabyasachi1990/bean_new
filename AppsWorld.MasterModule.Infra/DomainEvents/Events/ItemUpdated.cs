using AppsWorld.MasterModule.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
   public class ItemUpdated:IDomainEvent
    {
       public Item Item { get; private set; }

       public ItemUpdated(Item item)
       {
           Item = item;
       }
    }
}
