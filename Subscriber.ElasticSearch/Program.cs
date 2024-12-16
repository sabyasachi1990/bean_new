using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EventStore.ClientAPI;
using Nest;
using Newtonsoft.Json;
using ConnectionSettings = EventStore.ClientAPI.ConnectionSettings;
using AppsWorld.ReceiptModule.Entities;

namespace ElasticSearch.Subscriber
{
	class Program
	{
		static void Main(string[] args)
		{

			ReceiptContext context = new ReceiptContext();
			List<Company> lstCompany = context.Companies.ToList();

			const int defaultport = 1113;
			var settings = ConnectionSettings.Create();

			var addresses = Dns.GetHostAddresses("appsworldelk.cloudapp.net");
			//const string serviceStream = "613-ServiceGroup";
			using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(addresses[0], defaultport)))
			{
				// reUsable re = new reUsable();
				conn.ConnectAsync().Wait();
				//foreach (var company in lstCompany)
				//{
				List<string> lstMainStreams = new List<string>
                {
                    "Receipt"
                };
				string stream = String.Empty;
				//for (int i = 0; i < lstCompany.Count - 1; i++)
				//{
				//    for (int j = 0; j < lstMainStreams.Count; j++)
				//    {
				//        stream = lstCompany[i].Id.ToString() + "-" + lstMainStreams[j];
				stream = "735-Receipt";
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
						var jsonResult =
							JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(eventData).ToList();
						foreach (var at in jsonResult.Select(r => r.Value))
						{
							id = at["Id"].Value.ToString();
							status = at["Status"].Value.ToString();
						}

						var url = "/" + indexName + "?Id=" + id;

						Console.WriteLine("Received: " + x.Event.EventStreamId + ":" + x.Event.EventNumber);
						SaveInEs(jsonResult, id, indexName.ToLowerInvariant(), "AppsWorld", url, description, status, companyId);
						Console.WriteLine(eventData);

						Console.WriteLine("=========================");

						Console.WriteLine("=========================");

						Console.WriteLine(metaData);
					});
				//    }
				//}
				Console.WriteLine("waiting for events. press enter to exit");
				Console.ReadLine();
			}
		}

		public static bool SaveInEs<T>(T entity, string indexId, string indexName, string indexHeadline, string indexURL, string indexDescription, string status, string companyId)
		{


			var connectionSettings = new Nest.ConnectionSettings(new Uri("http://appsworldelk.cloudapp.net:9200/"));
			connectionSettings.SetDefaultIndex(indexName);
			var client = new ElasticClient(connectionSettings);

			var gsIndex = new GsIndex<T>
			{
				Id = indexId,
				CompanyId = companyId,
				Index = entity,
				Headline = indexHeadline,
				URL = indexURL,
				Description = indexDescription,
				status = status
			};
			if (entity != null)
			{
				client.Update<GsIndex<T>, object>(u => u.Id(gsIndex.Id).Doc(gsIndex).Upsert(gsIndex));
				return true;
			}

			return false;

		}

		public class GsIndex<T>
		{
			public T Index { get; set; }
			public string GsId { get; set; }
			public string Id { get; set; }
			public string CompanyId { get; set; }
			public string Headline { get; set; }
			public string URL { get; set; }
			public string Description { get; set; }

			public string status { get; set; }
		}
	}
}
