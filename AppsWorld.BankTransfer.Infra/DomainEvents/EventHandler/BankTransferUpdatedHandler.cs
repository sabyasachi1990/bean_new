using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Infra
{
    public class BankTransferUpdatedHandler : IDomainEventHandler<BankTransferUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public BankTransferUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Bank Transfer";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(BankTransferUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.BankTransferModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Transfer",
                Heading = "Bank Transfer",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.BankTransferModel.Id.ToString(),
                CompanyId = @event.BankTransferModel.CompanyId.ToString(),
                Description = String.Format("An Bank Transfer with name {0} is created by {1}", @event.BankTransferModel.UserCreated, @event.BankTransferModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.BankTransferModel.CompanyId + "-Transfer", typeof(BankTransferUpdated).Name);
        }
    }
}
