using AppsWorldEventStore;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Infra
{
 public  class BankReconciliationUpdatedHandler:IDomainEventHandler<BankReconciliationUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;

        public BankReconciliationUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }

        public void When(BankReconciliationUpdated @event)
        {
            object metaDataObj = new
            {
                Type = "BankReconciliation",
                Id = @event.BankReconciliationModel.Id,
                CompanyId = @event.BankReconciliationModel.CompanyId,
                Description = String.Format("A BankReconciliation with Type {0} is created by {1}", @event.BankReconciliationModel.UserCreated, @event.BankReconciliationModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.BankReconciliationModel.CompanyId + "-BankReconciliation", typeof(BankReconciliationUpdated).Name);
        }
    }
}
