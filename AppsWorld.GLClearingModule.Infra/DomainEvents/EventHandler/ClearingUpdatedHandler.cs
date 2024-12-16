using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Infra
{
    public class ClearingUpdatedHandler:IDomainEventHandler<ClearingUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public ClearingUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Clearing";
        private string moduleName = MenuConstants.MENU_BEAN;
        public void When(ClearingUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.ClearingModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Clearing",
                Heading = "Clearing",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.ClearingModel.Id.ToString(),
                CompanyId = @event.ClearingModel.CompanyId.ToString(),
                Description = String.Format("A Clearing is created by {0} on {1}", @event.ClearingModel.UserCreated, @event.ClearingModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.ClearingModel.CompanyId + "-Clearing", typeof(ClearingUpdated).Name);
        }
    }
}
