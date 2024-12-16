using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using System.Configuration;
using AppsWorldEventStore;
using FrameWork;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.ReceiptModule.Infra
{
    public class ReceiptCreatedHandler:IDomainEventHandler<ReceiptCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public ReceiptCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Receipt";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(ReceiptCreated @event)
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
                Description = String.Format("An Receipt with name {0} is created by {1}", @event.ReceiptModel.UserCreated, @event.ReceiptModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.ReceiptModel.CompanyId + "-Receipt", typeof(ReceiptCreated).Name);
        }
    }
}
