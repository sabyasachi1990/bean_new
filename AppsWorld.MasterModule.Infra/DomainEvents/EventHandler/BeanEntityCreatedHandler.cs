using System;
using AppsWorldEventStore;
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

namespace AppsWorld.MasterModule.Infra
{
    public class BeanEntityCreatedHandler : IDomainEventHandler<BeanEntityCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public BeanEntityCreatedHandler()
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

        public void When(BeanEntityCreated @event)
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
                Description = String.Format("An BeanEntity with name {0} is created by {1} on {2}", @event.BeanEntity.Name, @event.BeanEntity.UserCreated, @event.BeanEntity.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj,environment + @event.BeanEntity.CompanyId + "-BeanEntity", typeof(BeanEntityCreated).Name);
        }
    }
}
