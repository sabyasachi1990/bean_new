using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorldEventStore;
using System.Configuration;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
  public  class ItemCreatedHandler:IDomainEventHandler<ItemCreated>
  {
      private readonly IEventStoreOperations _eventStoreOperations;
	  string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

      public ItemCreatedHandler()
      {
          _eventStoreOperations=new PublishAppsWorldEvent();
      }

      public IEventStoreOperations EventStoreOperations
      {
          get { return _eventStoreOperations; }
      }
      ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
	  private string heading = "Items";
	  private string moduleName = MenuConstants.MENU_BEAN;
      public void When(ItemCreated @event)
      {
		  long id = moduleDetailUrl.GetByCompany(@event.Item.CompanyId);
		  string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
		  if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
         object metaDataObj = new
            {
                Type = "Item",
				Heading = "Items",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.Item.Id.ToString(),
                CompanyId = @event.Item.CompanyId.ToString(),
                Description = String.Format("An Item  is created by {0} on {1}", @event.Item.UserCreated, @event.Item.CreatedDate)
            };
		 _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.Item.CompanyId + "-Item", typeof(ItemCreated).Name);
        }
      }
  
}
