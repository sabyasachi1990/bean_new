using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorldEventStore;
using System.Configuration;
using Domain.Events;

using AppsWorld.CommonModule.Infra;

namespace AppsWorld.InvoiceModule.Infra
{
	public class CreditNoteDocStatusChangedHandler : IDomainEventHandler<CreditNoteDocStatusChanged>
    {
       private readonly IEventStoreOperations _eventStoreOperations;
	   string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
	   public CreditNoteDocStatusChangedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
		private string heading = "Credit Notes";
		private string moduleName = MenuConstants.MENU_BEAN;
		public void When(CreditNoteDocStatusChanged @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.CreditNoteModel.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "CreditNote",
				Heading = "Credit Notes",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.CreditNoteModel.Id.ToString(),
                CompanyId = @event.CreditNoteModel.CompanyId.ToString(),
                Description = String.Format("A CreditNote is modified by {0} on {1}", @event.CreditNoteModel.ModifiedBy,@event.CreditNoteModel.ModifiedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.CreditNoteModel.CompanyId + "-CreditNote", typeof(CreditNoteDocStatusChanged).Name);
        }
    }

    }

