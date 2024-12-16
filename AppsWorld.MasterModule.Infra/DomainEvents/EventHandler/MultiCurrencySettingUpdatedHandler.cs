using AppsWorldEventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.Framework;
using System.Configuration;
using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class MultiCurrencySettingUpdatedHandler : IDomainEventHandler<MultiCurrencySettingUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public MultiCurrencySettingUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
		private string heading = "General Settings";
		private string moduleName = MenuConstants.MENU_ADMIN;
        public void When(MultiCurrencySettingUpdated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.MultiCurrencySetting.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "MultiCurrencySetting",
				Heading = "General Settings",
				ModuleName = MenuConstants.MENU_ADMIN,
				Url = url,
                Id = @event.MultiCurrencySetting.Id.ToString(),
                CompanyId = @event.MultiCurrencySetting.CompanyId.ToString(),
                Description = String.Format("An MultiCurrencySetting with name {0} is created by {1} ", @event.MultiCurrencySetting.UserCreated, @event.MultiCurrencySetting.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.MultiCurrencySetting.CompanyId + "-MultiCurrencySetting", typeof(MultiCurrencySettingUpdated).Name);
        }
    }
}

