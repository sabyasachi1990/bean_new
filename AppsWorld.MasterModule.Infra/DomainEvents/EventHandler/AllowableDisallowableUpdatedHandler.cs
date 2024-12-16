
using System;
using AppsWorldEventStore;
using Domain.Events;

namespace AppsWorld.MasterModule.Infra
{
	public class AllowableDisallowableUpdatedHandler : IDomainEventHandler<AllowableDisallowableUpdated>
	{
		private readonly IEventStoreOperations _eventStoreOperations;

		public AllowableDisallowableUpdatedHandler()
		{
			_eventStoreOperations = new PublishAppsWorldEvent();
		}

		public IEventStoreOperations EventStoreOperations
		{
			get { return _eventStoreOperations; }
		}

		public void When(AllowableDisallowableUpdated @event)
		{
			object metaDataObj = new
			{
				Type = "AllowableDisallowable",
				Id = @event.CompanySetting.Id.ToString(),
				CompanyId = @event.CompanySetting.CompanyId.ToString(),
				Description = String.Format("An AllowableDisallowable with name {0} is created by {1} ", @event.CompanySetting.UserCreated, @event.CompanySetting.CreatedDate)
			};
			_eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.CompanySetting.CompanyId + "-AllowableDisallowable", typeof(AllowableDisallowableUpdated).Name);
		}
	}
}
