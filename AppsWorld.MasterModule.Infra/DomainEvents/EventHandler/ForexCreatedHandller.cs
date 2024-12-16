using System;
using AppsWorldEventStore;

namespace AppsWorld.MasterModule.Infra 
{
    using Domain.Events;
	using System.Configuration;
	using AppsWorld.Framework;
    using AppsWorld.CommonModule.Infra;
    public class ForexCreatedHandller : IDomainEventHandler<ForexCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public ForexCreatedHandller()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
		private string heading = "Forex";
		private string moduleName = MenuConstants.MENU_BEAN;
        public void When(ForexCreated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.ForexModel.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Forex",
				Heading = "Forex",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.ForexModel.Id.ToString(),
                CompanyId = @event.ForexModel.CompanyId.ToString(),
                Description = String.Format("A Forex with Type {0} is created by {1} on {2}",@event.ForexModel.Type, @event.ForexModel.UserCreated,@event.ForexModel.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.ForexModel.CompanyId + "-Forex", typeof(ForexCreated).Name);
        }
    }
}
