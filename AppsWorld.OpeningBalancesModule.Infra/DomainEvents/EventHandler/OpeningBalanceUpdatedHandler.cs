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

namespace AppsWorld.OpeningBalancesModule.Infra
{
    public class OpeningBalanceUpdatedHandler : IDomainEventHandler<OpeningBalanceUpdated>
    {
                private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public OpeningBalanceUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "OpeningBalance";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(OpeningBalanceUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.OpeningBalanceModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "OpeningBalance",
                Heading = "OpeningBalance",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.OpeningBalanceModel.Id.ToString(),
                CompanyId = @event.OpeningBalanceModel.CompanyId.ToString(),
                Description = String.Format("An Payment with name {0} is created by {1}", @event.OpeningBalanceModel.UserModified, @event.OpeningBalanceModel.ModifiedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.OpeningBalanceModel.CompanyId + "-OpeningBalance", typeof(OpeningBalanceUpdated).Name);
        }

    }
}
