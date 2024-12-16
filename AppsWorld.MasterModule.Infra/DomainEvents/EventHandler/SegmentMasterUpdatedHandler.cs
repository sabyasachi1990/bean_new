
using System;
using AppsWorldEventStore;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class SegmentMasterUpdatedHandler : IDomainEventHandler<SegmentMasterUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;

        public SegmentMasterUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

        public void When(SegmentMasterUpdated @event)
        {
            object metaDataObj = new
            {
                Type = "SegmentMaster",
                Id = @event.SegmentMaster.Id.ToString(),
                CompanyId = @event.SegmentMaster.CompanyId.ToString(),
                Description = String.Format("A SegmentMaster with name {0} is created by {1} on {2}", @event.SegmentMaster.Name,@event.SegmentMaster.UserCreated,@event.SegmentMaster.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.SegmentMaster.CompanyId + "-SegmentMaster", typeof(SegmentMasterUpdated).Name);
        }
    }
}
