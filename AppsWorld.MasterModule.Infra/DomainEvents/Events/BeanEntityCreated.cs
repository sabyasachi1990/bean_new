
using AppsWorld.MasterModule.Models;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class BeanEntityCreated : IDomainEvent
    {
        public BeanEntityModel BeanEntity { get; private set; }

        public BeanEntityCreated(BeanEntityModel beanEntity)
        {
            BeanEntity = beanEntity;
        }
    }
}
