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
    public class DepositUpdatedHandler:IDomainEventHandler<DepositUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public DepositUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Deposit";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(DepositUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.DepositModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Deposit",
                Heading = "Deposit",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.DepositModel.Id.ToString(),
                CompanyId = @event.DepositModel.CompanyId.ToString(),
                Description = String.Format("An Deposit with name {0} is created by {1}", @event.DepositModel.UserCreated, @event.DepositModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.DepositModel.CompanyId + "-Deposit", typeof(DepositUpdated).Name);
        }
    }
}
