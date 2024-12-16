
using System;
using AppsWorldEventStore;
using System.Configuration;
using Domain.Events;

using AppsWorld.CommonModule.Infra;

namespace AppsWorld.DebitNoteModule.Infra
{
	public class DebitNoteStatusChangedHandler : IDomainEventHandler<DebitNoteStatusChanged>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
		string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;

        public DebitNoteStatusChangedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
		private string heading = "DebitNotes";
		private string moduleName = MenuConstants.MENU_BEAN;
		public void When(DebitNoteStatusChanged @event)
        {
			long id = moduleDetailUrl.GetByCompany(@event.DebitNoteModel.CompanyId);
			string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
			if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "DebitNoteDocStateChanged",
				Heading = "DebitNotes",
				ModuleName = MenuConstants.MENU_BEAN,
				Url = url,
                Id = @event.DebitNoteModel.Id.ToString(),
                CompanyId = @event.DebitNoteModel.CompanyId.ToString(),
                Description = String.Format("A DebitNoteDocStateChanged is created by {0} on {1}", @event.DebitNoteModel.UserCreated,@event.DebitNoteModel.CreatedDate)
            };
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.DebitNoteModel.CompanyId + "-DebitNoteDocStateChanged", typeof(DebitNoteStatusChanged).Name);
        }
    }
}
