using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events;
using FrameWork;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CashSalesModule.Infra
{
    public class CashSaleCreatedHandler : IDomainEventHandler<CashSaleCreated>
    {
         private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public CashSaleCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "CashSale";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(CashSaleCreated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.CashSaleModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "CashSale",
                Heading = "CashSale",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.CashSaleModel.Id.ToString(),
                CompanyId = @event.CashSaleModel.CompanyId.ToString(),
                Description = String.Format("An CashSale with name {0} is created by {1}", @event.CashSaleModel.UserCreated, @event.CashSaleModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.CashSaleModel.CompanyId + "-CashSale", typeof(CashSaleCreated).Name);
        }
    }
}
