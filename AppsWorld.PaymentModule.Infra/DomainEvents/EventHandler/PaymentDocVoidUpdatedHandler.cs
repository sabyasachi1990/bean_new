﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorldEventStore;
using System.Configuration;
using Domain.Events;
using FrameWork;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.PaymentModule.Infra
{
    public class PaymentDocVoidUpdatedHandler : IDomainEventHandler<PaymentDocVoidUpdated>
    {
        private readonly IEventStoreOperations _eventStoreOperations;
        string environment = ConfigurationManager.AppSettings["Environment"] ?? String.Empty;
        public PaymentDocVoidUpdatedHandler()
        {
            _eventStoreOperations = new PublishAppsWorldEvent();
        }
        public IEventStoreOperations EventStoreOperations
        {
            get { return _eventStoreOperations; }
        }
        ModuleDetailUrl.ModuleDetailUrl moduleDetailUrl = new ModuleDetailUrl.ModuleDetailUrl();
        private string heading = "Payment";
        private string moduleName = MenuConstants.MENU_BEAN;

        public void When(PaymentDocVoidUpdated @event)
        {
            long id = moduleDetailUrl.GetByCompany(@event.PaymentModel.CompanyId);
            string url = moduleDetailUrl.GetUrl(heading, moduleName, id);
            if (!string.IsNullOrEmpty(environment)) environment = environment + "-";
            object metaDataObj = new
            {
                Type = "Payment",
                Heading = "PaymentDocVoid",
                ModuleName = MenuConstants.MENU_BEAN,
                Url = url,
                Id = @event.PaymentModel.Id.ToString(),
                CompanyId = @event.PaymentModel.CompanyId.ToString(),
                Description = String.Format("An PaymentDocVoid with name {0} is created by {1}", @event.PaymentModel.UserCreated, @event.PaymentModel.CreatedDate)
            };
            _eventStoreOperations.SaveEventToStream(@event, metaDataObj, environment + @event.PaymentModel.CompanyId + "-PaymentDocVoid", typeof(PaymentDocVoidUpdated).Name);
        }
    }
}