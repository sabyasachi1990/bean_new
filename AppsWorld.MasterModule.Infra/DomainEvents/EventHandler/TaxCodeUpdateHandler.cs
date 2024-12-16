
using System;
using System.Configuration;
using AppsWorld.Framework;
using AppsWorldEventStore;

using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    
    public class TaxCodeUpdateHandler : IDomainEventHandler<TaxCodeUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public TaxCodeUpdateHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Tax Codes";
        private string moduleName = MenuConstants.MENU_BEAN;
        public void When(TaxCodeUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.TaxCode.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "TaxCode",
                Heading = "Tax Codes",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.TaxCode.Id.ToString(),
                CompanyId = @event.TaxCode.CompanyId.ToString(),
                Description = String.Format("A TaxCode with name {0} is created by {1} on {2}", @event.TaxCode.Name, @event.TaxCode.UserCreated, @event.TaxCode.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj,environment+ @event.TaxCode.CompanyId + "-TaxCode", typeof(TaxCodeUpdated).Name);
        }
    }
}
