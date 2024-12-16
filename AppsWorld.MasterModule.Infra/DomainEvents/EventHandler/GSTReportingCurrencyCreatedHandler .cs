
using System;
using AppsWorldEventStore;
using System.Configuration;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class GSTReportingCurrencyCreatedHandler : IDomainEventHandler<GSTReportingCurrencyCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public GSTReportingCurrencyCreatedHandler()
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

        public void When(GSTReportingCurrencyCreated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.GSTSetting.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "GSTReportingCurrencyChanged",
				Heading = "General Settings",
				ModuleName = MenuConstants.MENU_ADMIN,
				Url = url,
                Id = @event.GSTSetting.Id.ToString(),
                CompanyId = @event.GSTSetting.CompanyId.ToString(),
                Description = String.Format("A GSTReportingCurrencyCreated with number {0} is created by {1} on {2}", @event.GSTSetting.Number,@event.GSTSetting.UserCreated,@event.GSTSetting.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment+ @event.GSTSetting.CompanyId + "-GSTReportingCurrencyChanged", typeof(GSTReportingCurrencyCreated).Name);
        }
    }
}
