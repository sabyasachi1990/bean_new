using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System.Net;

namespace OpeningBalancesUnitTest
{
    public class RestHelper
    {
        public static IRestResponse PostBasicAuthenication(string baseUrl, string resourceUrl, string userName, string password, string json)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }

        public static string ConvertObjectToJason<T>(T arg)
        {
            return JsonConvert.SerializeObject(arg);
        }
        public static IRestResponse ZPost(string baseUrl, string resourceUrl, string json)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["IsProtocol"].ToString().ToLower() == "true")
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            var client = new RestClient(baseUrl);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }


        public static object GetResponseObject(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<object>(response.Content);
        }

        public static IRestResponse ZGet(string baseUrl, string resourceUrl, List<List<string, string>> paraMeters)
        {
            string parameters = null;
            if (paraMeters.Any())
            {
                parameters = paraMeters.Aggregate(parameters, (current, param) => current + (param.Name + "=" + param.Value + "&"));
                parameters = parameters.Trim('&');
            }
            var client = new RestClient(baseUrl);
            RestRequest request;
            if (parameters == null)
            {
                request = new RestRequest(resourceUrl, Method.GET);
            }
            else
            {
                request = new RestRequest(resourceUrl + "?" + parameters, Method.GET);
            }
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }
        public static IRestResponse ZGet(string baseUrl, string resourceUrl)
        {
            var client = new RestClient(baseUrl);
            RestRequest request;
            request = new RestRequest(resourceUrl, Method.GET);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }
    }

    public class List<T, D>
    {
        public T Name { get; set; }
        public D Value { get; set; }
    }
}
