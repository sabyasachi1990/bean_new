using AppsWorld.MasterModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
   public class QuickBeanEntityCreated:IDomainEvent
    {
        public QuickBeanEntityModel QuickBeanEntityModel { get; private set; }

        public QuickBeanEntityCreated(QuickBeanEntityModel quickBeanEntityModel)
        {
            QuickBeanEntityModel = quickBeanEntityModel;
        }
    }
}
