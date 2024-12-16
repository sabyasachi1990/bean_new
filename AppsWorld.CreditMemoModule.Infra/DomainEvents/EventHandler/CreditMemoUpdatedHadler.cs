using AppsWorld.CommonModule.Infra;
using AppsWorldEventStore;
using Domain.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Infra
{
    public class CreditMemoUpdatedHadler : IDomainEventHandler<CreditMemoUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public CreditMemoUpdatedHadler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Credit Memos";
        private string moduleName = MenuConstants.MENU_BEAN;
        public void When(CreditMemoUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.CreditMemoModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "CreditMemo",
                Heading = "Credit Memos",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.CreditMemoModel.Id.ToString(),
                CompanyId = @event.CreditMemoModel.CompanyId.ToString(),
                Description = String.Format("A CreditMemo is modified by {0} on {1}", @event.CreditMemoModel.ModifiedBy, @event.CreditMemoModel.ModifiedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.CreditMemoModel.CompanyId + "-CreditMemo", typeof(CreditMemoUpdated).Name);
        }
    }
}
