using System;
using AppsWorldEventStore;
using Domain.Events;
namespace AppsWorld.MasterModule.Infra
{
	using System.Configuration;
	using AppsWorld.Framework;
 
    using AppsWorld.CommonModule.Infra;
    public class ForexUpdatedHandller : IDomainEventHandler<ForexUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public ForexUpdatedHandller()
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
        public void When(ForexUpdated @event)
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
                Description = String.Format("A Forex with Type {0} is modified by {1} on {2}",@event.ForexModel.Type, @event.ForexModel.ModifiedBy,@event.ForexModel.ModifiedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.ForexModel.CompanyId + "-Forex", typeof(ForexUpdated).Name);
        }
    }
}
