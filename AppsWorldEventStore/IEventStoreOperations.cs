namespace AppsWorldEventStore
{
    public interface IEventStoreOperations
    {
        void SaveEventToStream(object eventObject, object metaData, string eventStreamName, string eventName);
    }
}
