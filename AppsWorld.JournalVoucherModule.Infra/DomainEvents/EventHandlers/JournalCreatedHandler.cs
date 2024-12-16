using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorldEventStore;
using Domain.Events;
using System.Configuration;
using FrameWork;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Infra
{
    public class JournalCreatedHandler :IDomainEventHandler<JournalCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public JournalCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Journal";
        private string moduleName = MenuConstants.MENU_BEAN;
        public void When(JournalCreated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.JournalModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Journal",
                Heading = "Journal",
                Url = url,
                Id = @event.JournalModel.Id.ToString(),
                ModuleName = MenuConstants.MENU_BEAN,
                CompanyId = @event.JournalModel.CompanyId.ToString(),
                Description = String.Format("A Forex with Type {0} is created by {1}", @event.JournalModel.UserCreated, @event.JournalModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj,environment+@event.JournalModel.CompanyId + "-Journal", typeof(JournalCreated).Name);
        }
    }
}
