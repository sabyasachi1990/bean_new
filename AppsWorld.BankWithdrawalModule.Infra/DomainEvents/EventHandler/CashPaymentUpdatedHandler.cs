using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Events;
using AppsWorldEventStore;
using System.Configuration;
using FrameWork;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BankWithdrawalModule.Infra
{
    public class CashPaymentUpdatedHandler:IDomainEventHandler<CashPaymentUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public CashPaymentUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Cash Payments";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(CashPaymentUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.CashPaymentsModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Cash Payments",
                Heading = "Cash Payments",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.CashPaymentsModel.Id.ToString(),
                CompanyId = @event.CashPaymentsModel.CompanyId.ToString(),
                Description = String.Format("An Cash_Payment with name {0} is created by {1}", @event.CashPaymentsModel.UserCreated, @event.CashPaymentsModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.CashPaymentsModel.CompanyId + "-Cash Payments ", typeof(CashPaymentUpdated).Name);
        }
    }
}
