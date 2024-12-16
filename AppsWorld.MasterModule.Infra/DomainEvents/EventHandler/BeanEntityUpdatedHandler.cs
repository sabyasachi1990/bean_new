using System;
using AppsWorldEventStore;
using System.Configuration;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
    public class BeanEntityUpdatedHandler : IDomainEventHandler<BeanEntityUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public BeanEntityUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
		private string heading = "Entities";
		private string moduleName = MenuConstants.MENU_BEAN;
        public void When(BeanEntityUpdated @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.BeanEntity.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "BeanEntity",
				Heading = "Entities",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.BeanEntity.Id.ToString(),
                CompanyId = @event.BeanEntity.CompanyId.ToString(),
                Description = String.Format("An BeanEntity with name {0} is modified by {1} on {2}", @event.BeanEntity.Name, @event.BeanEntity.ModifiedBy, @event.BeanEntity.ModifiedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj,environment + @event.BeanEntity.CompanyId + "-BeanEntity", typeof(BeanEntityUpdated).Name);
        }
    }
}
