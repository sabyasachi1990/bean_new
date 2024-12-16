using AppsWorld.InvoiceModule.Entities;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AppsWorldEventStore;
using AppsWorld.CommonModule.Infra;


namespace AppsWorld.InvoiceModule.Infra
{
    public class CreditNoteCreatedHandler : IDomainEventHandler<CreditNoteCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public CreditNoteCreatedHandler()
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
        public void When(CreditNoteCreated @event)
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
                Description = String.Format("A CreditNote is created by {0} on {1}", @event.CreditNoteModel.UserCreated, @event.CreditNoteModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.CreditNoteModel.CompanyId + "-CreditNote", typeof(CreditNoteCreated).Name);
        }
    }
}
