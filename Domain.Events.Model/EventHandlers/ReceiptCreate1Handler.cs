using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events.Model.Events;
using FrameWork;
using ModuleUrl.CommonConstant;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Model.EventHandlers
{
    public class ReceiptCreate1Handler : IDomainEventHandler<ReceiptCreated1>
    {
          private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public ReceiptCreate1Handler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
		ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl();
        private string heading = "Receipt";
		private string moduleName = MenuConstants.MENU_BEAN;

        public void When(ReceiptCreated1 @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.ReceiptModel.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Receipt",
                Heading = "Receipt",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.ReceiptModel.Id.ToString(),
                CompanyId = @event.ReceiptModel.CompanyId.ToString(),
                Description = String.Format("An VendorBill with name {0} is created by {1}", @event.ReceiptModel.UserCreated, @event.ReceiptModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.ReceiptModel.CompanyId + "-Receipt", typeof(ReceiptCreated1).Name);
        }
    }
}
