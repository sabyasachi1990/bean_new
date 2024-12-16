
using System;
using AppsWorldEventStore;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class SegmentMasterCreatedHandler : IDomainEventHandler<SegmentMasterCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;

        public SegmentMasterCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

        public void When(SegmentMasterCreated @event)
        {
            object metaDataObj = new
            {
                Type = "SegmentMaster",
                Id = @event.SegmentMaster.Id.ToString(),
                CompanyId = @event.SegmentMaster.CompanyId.ToString(),
                Description = String.Format("A SegmentMaster with name {0} is created by {1} on {2}", @event.SegmentMaster.Name,@event.SegmentMaster.UserCreated,@event.SegmentMaster.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.SegmentMaster.CompanyId + "-SegmentMaster", typeof(SegmentMasterCreated).Name);
        }
    }
}
