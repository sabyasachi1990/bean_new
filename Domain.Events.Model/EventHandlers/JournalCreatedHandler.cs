using AppsWorldEventStore;
using Domain.Events.Model.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.EventHandlers
{
    using Events;
   public  class JournalCreatedHandler:IDomainEventHandler<JournalCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;

        public JournalCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

        public void When(JournalCreated @event)
        {
            object metaDataObj = new
            {
                Type = "Journal",
                Id = @event.JournalModel.Id.ToString(),
                CompanyId = @event.JournalModel.CompanyId.ToString(),
                Description = String.Format("A Forex with Type {0} is created by {1}  }", @event.JournalModel.UserCreated, @event.JournalModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.JournalModel.CompanyId + "-Journal", typeof(JournalCreated).Name);
        }
    }
}
