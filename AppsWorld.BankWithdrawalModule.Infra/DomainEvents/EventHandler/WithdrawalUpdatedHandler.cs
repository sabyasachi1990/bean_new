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
    public class WithdrawalUpdatedHandler:IDomainEventHandler<WithdrawalUpdated>
    {
         private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public WithdrawalUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Withdrawal";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(WithdrawalUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.WithdrawalModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Withdrawal",
                Heading = "Withdrawal",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.WithdrawalModel.Id.ToString(),
                CompanyId = @event.WithdrawalModel.CompanyId.ToString(),
                Description = String.Format("An Withdrawal with name {0} is created by {1}", @event.WithdrawalModel.UserCreated, @event.WithdrawalModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.WithdrawalModel.CompanyId + "-Withdrawal", typeof(WithdrawalUpdated).Name);
        }
    }
}
