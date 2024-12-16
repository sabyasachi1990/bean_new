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
   public class BillCreatedHandler:IDomainEventHandler<BillCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public BillCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
		ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl();
		private string heading = "Entities";
		private string moduleName = MenuConstants.MENU_BEAN;

        public void When(BillCreated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.BillModel.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
				Type = "VendorBill",
				Heading = "Vendor Bill",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.BillModel.Id.ToString(),
				CompanyId = @event.BillModel.CompanyId.ToString(),
				Description = String.Format("An VendorBill with name {0} is created by {1}", @event.BillModel.UserCreated, @event.BillModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.BillModel.CompanyId + "-Bill", typeof(BillCreated).Name);
        }
    }
}

