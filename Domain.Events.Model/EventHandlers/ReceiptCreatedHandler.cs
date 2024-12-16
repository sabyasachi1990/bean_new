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
	public class ReceiptCreatedHandler : IDomainEventHandler<ReceiptCreated>
	{ 
		 private readonly IEventStoreOperations _eventStoreOperations;

		 public ReceiptCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

		public void When(ReceiptCreated @event)
        {
            object metaDataObj = new
            {
                Type = "Receipt",
                Id = @event.ReceiptModel.Id.ToString(),
				CompanyId = @event.ReceiptModel.CompanyId.ToString(),
				Description = String.Format("An AccountType with name {0} is created by {1} ", @event.ReceiptModel.UserCreated, @event.ReceiptModel.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.ReceiptModel.CompanyId + "-Receipt", typeof(ReceiptCreated).Name);
        }
	}
}
