using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using ConnectionSettings = EventStore.ClientAPI.ConnectionSettings;

namespace DB.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            const string stream = "1-AppsWorldMasterData";

            const int defaultport = 1113;

            var settings = ConnectionSettings.Create();

            var addresses = Dns.GetHostAddresses("appsworldelk.cloudapp.net");
            //var addresses = Dns.GetHostAddresses("192.168.0.118");


            using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(addresses[0], defaultport)))
            {
                conn.ConnectAsync().Wait();
                conn.SubscribeToStreamFrom(stream, StreamPosition.Start, true,
                    (_, x) =>
                    {
                        var metaData = Encoding.ASCII.GetString(x.Event.Metadata);
                        var jsonMetaResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(metaData);

                        var companyId = string.Empty;
                        var id = string.Empty;
                        var description = string.Empty;
                        var indexName = string.Empty;
                        var status = string.Empty;
                        if (jsonMetaResult.ContainsKey("CompanyId"))
                        {
                            companyId = jsonMetaResult["CompanyId"];
                        }
                        if (jsonMetaResult.ContainsKey("Id"))
                        {
                            id = jsonMetaResult["Id"];
                        }
                        if (jsonMetaResult.ContainsKey("Description"))
                        {
                            description = jsonMetaResult["Description"];
                        }
                        if (jsonMetaResult.ContainsKey("Type"))
                        {
                            indexName = jsonMetaResult["Type"];
                        }
                        var eventData = Encoding.ASCII.GetString(x.Event.Data);
                        var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(eventData).ToList();
                        foreach (var at in jsonResult.Select(r => r.Value))
                        {
                            id = at["Id"].Value.ToString();
                            status = at["Status"].Value.ToString();
                        }

                        var url = "/" + indexName + "?Id=" + id;

                        Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
                        //SaveInEs(jsonResult, id, indexName.ToLowerInvariant(), "AppsWorld", url, description, status, companyId);
                        //UpdateQuotationDetail 
                        Console.WriteLine(eventData);

                        Console.WriteLine("=========================");

                        Console.WriteLine(metaData);
                    });
                Console.WriteLine("waiting for events. press enter to exit");
                Console.ReadLine();
            }
        }


    }
}
