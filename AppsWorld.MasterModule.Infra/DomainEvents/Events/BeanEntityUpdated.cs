
using AppsWorld.MasterModule.Models;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
    public class BeanEntityUpdated : IDomainEvent
    {
        public BeanEntityModel BeanEntity { get; private set; }

        public BeanEntityUpdated(BeanEntityModel beanEntity)
        {
            BeanEntity = beanEntity;
        }
    }
}
