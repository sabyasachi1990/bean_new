using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Infra
{
    public class CartOfAccountStatusChangedHandler : IDomainEventHandler<ChartOfAccountStatusChanged>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public CartOfAccountStatusChangedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Chart Of Accounts";
        private string moduleName = MenuConstants.MENU_BEAN;
        public void When(ChartOfAccountStatusChanged @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.ChartOfAccount.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "ChartOfAccount",
                Heading = "Chart Of Accounts",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.ChartOfAccount.Id.ToString(),
                CompanyId = @event.ChartOfAccount.CompanyId.ToString(),
                Description = String.Format("An ChartOfAccount with name {0} is modified by {1} on {2}", @event.ChartOfAccount.Name, @event.ChartOfAccount.ModifiedBy, @event.ChartOfAccount.ModifiedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.ChartOfAccount.CompanyId + "-ChartOfAccount", typeof(ChartOfAccountStatusChanged).Name);
        }
    }
}
