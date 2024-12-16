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
    public class DoubtfulDebtCreatedHandler : IDomainEventHandler<DoubtfulDebtCreated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public DoubtfulDebtCreatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Doubtful Debts";
        private string moduleName = MenuConstants.MENU_BEAN;


        public void When(DoubtfulDebtCreated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.DoubtfulDebtModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "DoubtfulDebt",
                Heading = "Doubtful Debts",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.DoubtfulDebtModel.Id.ToString(),
                CompanyId = @event.DoubtfulDebtModel.CompanyId.ToString(),
                Description = String.Format("A DoubtfulDebt is created by {0} on {1}", @event.DoubtfulDebtModel.UserCreated, @event.DoubtfulDebtModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.DoubtfulDebtModel.CompanyId + "-DoubtfulDebt", typeof(DoubtfulDebtCreated).Name);
        }
    }
}
