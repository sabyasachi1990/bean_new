﻿
using System;
using AppsWorldEventStore;

namespace AppsWorld.InvoiceModule.Infra
{
    //using Domain.Events;
    //public class JournalLedgerUpdatedHandler : IDomainEventHandler<JournalLedgerUpdated>
    //{
    //    private readonly IEventStoreOperations _eventStoreOperations;

    //    public JournalLedgerUpdatedHandler()
    //    {
    //        _eventStoreOperations = new PublishAppsWorldEvent();
    //    }

    //    public IEventStoreOperations EventStoreOperations
    //    {
    //        get { return _eventStoreOperations; }
    //    }

    //    //public void When(JournalLedgerUpdated @event)
    //    //{
    //    //    object metaDataObj = new
    //    //    {
    //    //        Type = "JournalEntry",
    //    //        Id = @event.JournalEntry.Id.ToString(),
    //    //        CompanyId = @event.JournalEntry.CompanyId.ToString(),
    //    //        Description = String.Format("A JournalEntry with DocType {0} is created by {1} on {2}", @event.JournalEntry.DocType,@event.JournalEntry.UserCreated,@event.JournalEntry.CreatedDate)
    //    //    };
    //    //    _eventStoreOperations.SaveEventToStream(@event, metaDataObj, @event.JournalEntry.CompanyId + "-JournalEntry", typeof(JournalLedgerUpdated).Name);
    //    //}
    //}
}