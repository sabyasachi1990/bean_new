
using System;
using AppsWorldEventStore;
using System.Configuration;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class FinancialYearEndChangedCreatedHandller : IDomainEventHandler<FinancialYearEndChangedCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public FinancialYearEndChangedCreatedHandller()
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
        public void When(FinancialYearEndChangedCreated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.FinancialSetting.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "FinancialYearEndChanged",
				Heading = "General Settings",
				ModuleName = MenuConstants.MENU_ADMIN,
				Url = url,
                Id = @event.FinancialSetting.Id.ToString(),
                CompanyId = @event.FinancialSetting.CompanyId.ToString(),
                Description = String.Format("An FinancialYearEndChanged is created by {0} on {1}", @event.FinancialSetting.UserCreated,@event.FinancialSetting.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.FinancialSetting.CompanyId + "-FinancialYearEndChanged", typeof(FinancialYearEndChangedCreated).Name);
        }
    }
}
