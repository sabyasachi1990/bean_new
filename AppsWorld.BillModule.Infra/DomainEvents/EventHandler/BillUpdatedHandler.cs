using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorldEventStore;
using Domain.Events;
using FrameWork;
using System.Configuration;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Infra
{
    public class BillUpdatedHandler:IDomainEventHandler<BillUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public BillUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Entities";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(BillUpdated @event)
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
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.BillModel.CompanyId + "-Bill", typeof(BillUpdated).Name);
        }
    }
}
