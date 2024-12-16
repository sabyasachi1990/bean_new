using System;
using AppsWorldEventStore;

namespace AppsWorld.InvoiceModule.Infra
{
	using System.Configuration;	  
    using AppsWorld.CommonModule.Infra;
    using AppsWorld.InvoiceModule.Infra;
    using Domain.Events;
	public class InvoiceDocStatusChangedHandler : IDomainEventHandler<InvoiceDocStatusChanged>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
		public InvoiceDocStatusChangedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
		private string heading = "Invoices";
		private string moduleName = MenuConstants.MENU_BEAN;
        private AppsWorld.InvoiceModule.Models.InvoiceModel TObject;
		public void When(InvoiceDocStatusChanged @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.Invoice.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Invoice",
				Heading = "Invoices",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.Invoice.Id.ToString(),
                CompanyId = @event.Invoice.CompanyId.ToString(),
                Description = String.Format("An Invoice with name {0} is created by {1} on {2}", @event.Invoice.DocSubType, @event.Invoice.UserCreated, @event.Invoice.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.Invoice.CompanyId + "-Invoice", typeof(InvoiceDocStatusChanged).Name);
        }


    }
}
