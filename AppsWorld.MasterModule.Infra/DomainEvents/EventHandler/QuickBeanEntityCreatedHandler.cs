using AppsWorldEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class QuickBeanEntityCreatedHandler : IDomainEventHandler<QuickBeanEntityCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;

        public QuickBeanEntityCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

        public void When(QuickBeanEntityCreated @event)
        {
            object metaDataObj = new
            {
                Type = "QuickBeanEntity",
                Id = @event.QuickBeanEntityModel.Id.ToString(),
                CompanyId = @event.QuickBeanEntityModel.CompanyId.ToString(),
                Description = String.Format("An BeanEntity with name {0} is created by {1} ", @event.QuickBeanEntityModel.UserCreated, @event.QuickBeanEntityModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.QuickBeanEntityModel.CompanyId + "-QuickBeanEntity", typeof(QuickBeanEntityCreated).Name);
        }
    }
}
