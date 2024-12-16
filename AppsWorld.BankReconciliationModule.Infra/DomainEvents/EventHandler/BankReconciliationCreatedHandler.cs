using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Infra
{
   public class BankReconciliationCreatedHandler:IDomainEventHandler<BankReconciliationCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public BankReconciliationCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Bank Reconciliation";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(BankReconciliationCreated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.BankReconciliationModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "BankReconciliation",
                Heading = "Bank Reconciliation",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.BankReconciliationModel.Id.ToString(),
                CompanyId = @event.BankReconciliationModel.CompanyId.ToString(),
                Description = String.Format("A BankReconciliation with Type {0} is created by {1}", @event.BankReconciliationModel.UserCreated, @event.BankReconciliationModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.BankReconciliationModel.CompanyId + "-BankReconciliation", typeof(BankReconciliationCreated).Name);
        }
    }
}
