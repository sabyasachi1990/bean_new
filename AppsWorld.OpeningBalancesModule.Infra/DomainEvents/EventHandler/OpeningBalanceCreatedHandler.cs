using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorldEventStore;
using System.Configuration;
using FrameWork;
using AppsWorld.CommonModule.Infra;
namespace AppsWorld.OpeningBalancesModule.Infra
{
    public class OpeningBalanceCreatedHandler : IDomainEventHandler<OpeningBalanceCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public OpeningBalanceCreatedHandler()
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

        public void When(OpeningBalanceCreated @event)
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
                Description = String.Format("An Payment with name {0} is created by {1}", @event.OpeningBalanceModel.UserCreated, @event.OpeningBalanceModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.OpeningBalanceModel.CompanyId + "-OpeningBalance", typeof(OpeningBalanceCreated).Name);
        }

    }
}
