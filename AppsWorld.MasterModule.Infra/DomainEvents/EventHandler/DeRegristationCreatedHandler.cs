
using System;
using AppsWorldEventStore;
using System.Configuration;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class DeRegristationCreatedHandler : IDomainEventHandler<DeRegristationCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;

        public DeRegristationCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

        public void When(DeRegristationCreated @event)
        {
            object metaDataObj = new
            {
                Type = "DeRegristation",
                Id = @event.gstSetting.Id.ToString(),
                CompanyId = @event.gstSetting.CompanyId.ToString(),
                Description = String.Format("A DeRegristation with number {0} is created by {1} on {2}", @event.gstSetting.Number,@event.gstSetting.UserCreated,@event.gstSetting.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, ConfigurationManager.AppSettings["Environment"] + @event.gstSetting.CompanyId + "-DeRegristation", typeof(DeRegristationCreated).Name);
        }
    }
}
