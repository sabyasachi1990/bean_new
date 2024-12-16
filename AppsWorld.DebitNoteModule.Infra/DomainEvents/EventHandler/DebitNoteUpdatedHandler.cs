using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Domain.Events;
using AppsWorldEventStore;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.DebitNoteModule.Infra
{
   public class DebitNoteUpdatedHandler:IDomainEventHandler<DebitNoteUpdated>
   {
       private readonly IEventStoreOperations _eventStoreOperations;
	   string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

       public DebitNoteUpdatedHandler()
       {
           _eventStoreOperations=new PublishAppsWorldEvent();
       }

       public IEventStoreOperations EventStoreOperations
       {
           get { return _eventStoreOperations; }
       }

       ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
	   private string heading = "Debit Notes";
	   private string moduleName = MenuConstants.MENU_BEAN;
       public void When(DebitNoteUpdated @event)
       {
		   long id = moduleDetailUrl.GetByCompany(@event.DebitNoteModel.CompanyId);
		   string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
		   if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
           object metaDataObj = new
           {
               Type = "DebitNote",
			   Heading = "Debit Notes",
			   ModuleName = MenuConstants.MENU_BEAN,
			   Url = url,
               Id = @event.DebitNoteModel.Id.ToString(),
               CompanyId = @event.DebitNoteModel.CompanyId.ToString(),
               Description=string.Format("A DebitNote is modified by {0} on {1}",@event.DebitNoteModel.ModifiedBy,@event.DebitNoteModel.ModifiedDate)
           };
		   _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.DebitNoteModel.CompanyId + "-DebitNote", typeof(DebitNoteUpdated).Name);
       }
   }
}
