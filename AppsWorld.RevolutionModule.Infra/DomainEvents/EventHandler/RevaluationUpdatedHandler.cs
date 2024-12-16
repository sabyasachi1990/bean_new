using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Models;
using Domain.Events;
using AppsWorldEventStore;
using System.Configuration;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.RevaluationModule.Infra 
{
    public class RevaluationUpdatedHandler:IDomainEventHandler<RevaluationUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public RevaluationUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }

        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Revaluation";
        private string moduleName = MenuConstants.MENU_BEAN;
        public void When(RevaluationUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.RevaluationModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Revaluation",
                Heading = "Revaluation",
                Url = url,
                Id = @event.RevaluationModel.Id.ToString(),
                ModuleName = MenuConstants.MENU_BEAN,
                CompanyId = @event.RevaluationModel.CompanyId.ToString(),
                Description = String.Format("A Forex with Type {0} is created by {1}", @event.RevaluationModel.UserCreated, @event.RevaluationModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.RevaluationModel.CompanyId + "-Revaluation", typeof(RevaluationUpdated).Name);
        }
    }
}
