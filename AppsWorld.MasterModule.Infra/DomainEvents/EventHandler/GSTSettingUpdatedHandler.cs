﻿
using System;
using AppsWorldEventStore;
using System.Configuration;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class GSTSettingUpdatedHandler : IDomainEventHandler<GSTSettingUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public GSTSettingUpdatedHandler()
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
        public void When(GSTSettingUpdated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.GSTSetting.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "GSTSetting",
				Heading = "General Settings",
				ModuleName = MenuConstants.MENU_ADMIN,
				Url = url,
                Id = @event.GSTSetting.Id.ToString(),
                CompanyId = @event.GSTSetting.CompanyId.ToString(),
                Description = String.Format("A GSTSetting with number {0} is created by {1} on {2}", @event.GSTSetting.Number,@event.GSTSetting.UserCreated,@event.GSTSetting.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.GSTSetting.CompanyId + "-GSTSetting", typeof(GSTSettingUpdated).Name);
        }
    }
}