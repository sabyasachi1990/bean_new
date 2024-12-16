using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Configuration;

namespace AppsWorldEventStore
{
    public class PublishAppsWorldEvent : IEventStoreOperations
    {
        readonly string _eventStoreServer;
        readonly bool _isEventStoreEnabled;
        private readonly string _eventStreamName;
        public PublishAppsWorldEvent()
        {
            var section = (NameValueCollection)ConfigurationManager.GetSection("EventStoreSettings");
            _eventStoreServer = section["AppsWorldEventStoreServer"];
            _isEventStoreEnabled = Convert.ToBoolean(section["EventStoreEnabled"]);
            _eventStreamName = section["EventStreamName"];
        }
        public void SaveEventToStream(object eventObject, object metaData, string eventStreamName, string eventName)
        {
            try
            {
                if (_isEventStoreEnabled)
                {
                    var eventObjectjsonString = JsonConvert.SerializeObject(eventObject);
                    var metaDataObjectjsonString = JsonConvert.SerializeObject(metaData);
                    //var stream = _eventStreamName;
                    const int defaultport = 1113;
                    var settings = ConnectionSettings.Create();

                    var addresses = Dns.GetHostAddresses(_eventStoreServer);
                    using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(addresses[0], defaultport)))
                    {
                        conn.ConnectAsync().Wait();
                        conn.AppendToStreamAsync(eventStreamName,
                            ExpectedVersion.Any,
                            GetEventDataFor(metaDataObjectjsonString, eventObjectjsonString, eventName)).Wait();
                    }
                }
            }
            catch (Exception eventStoreException)
            {
                eventStoreException = null;

            }
        }
        public EventData GetEventDataFor(string metaDataObjectjsonString, string eventObjjsonString, string eventType)
        {
            return new EventData(
                Guid.NewGuid(),
                eventType,
                true,
                Encoding.ASCII.GetBytes(eventObjjsonString),
                Encoding.ASCII.GetBytes(metaDataObjectjsonString)
                );
        }
    }
}
