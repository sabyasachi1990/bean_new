using AppsWorld.MasterModule.Models;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
   public class BeanEntityStatusChanged:IDomainEvent
    {
        public BeanEntityModel BeanEntity { get; private set; }

        public BeanEntityStatusChanged(BeanEntityModel beanEntity)
        {
            BeanEntity = beanEntity;
        }
    }
}
