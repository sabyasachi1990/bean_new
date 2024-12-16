using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Ziraff.FrameWork;

namespace AppsWorld.CommonModule.Application
{
    public class RestHelper
    {

        private static string _skipAuthPassword = null;
        private static string skipAuthPassword
        {
            get
            {
                if (_skipAuthPassword == null)
                {
                    //    _skipAuthPassword = KeyVaultService.GetSecret(
                    // ConfigurationManager.AppSettings["SkipAuthPasswordClientId"],
                    //ConfigurationManager.AppSettings["SkipAuthPasswordClientSecret"],
                    //ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"]);
                    _skipAuthPassword = Ziraff.FrameWork.SingleTon.CommonConnection.SkipAuthPassword;
                }
                return _skipAuthPassword;
            }
        }

        public static IRestResponse PostBasicAuthenication(string baseUrl, string resourceUrl, string userName, string password, string json)
        {
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            AddRestSharpHeader(request);
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
            AddRestSharpHeader(request);
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
            AddRestSharpHeader(request);
            return client.Execute(request);
        }
        public static IRestResponse ZGet(string baseUrl, string resourceUrl)
        {
            var client = new RestClient(baseUrl);
            RestRequest request;
            request = new RestRequest(resourceUrl, Method.GET);
            request.RequestFormat = DataFormat.Json;
            AddRestSharpHeader(request);
            return client.Execute(request);
        }



        public static IRestResponse RestGet(string baseUrl, string resourceUrl, List<List<string, string>> paraMeters)
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
            //AddRestSharpHeader(request);
            return client.Execute(request);
        }

        private static void AddRestSharpHeader(RestRequest request)
        {


            request.AddHeader("SkipAuthPassword", skipAuthPassword);
            if (HttpContext.Current.Request.Headers != null && HttpContext.Current.Request.Headers.AllKeys.Contains("BasicParams"))
            {
                request.AddHeader("AuthInformation", HttpContext.Current.Request.Headers.GetValues("BasicParams").FirstOrDefault());
            }
            if (HttpContext.Current.Request.Headers != null && HttpContext.Current.Request.Headers.AllKeys.Contains("Authorization"))
            {
                request.AddHeader("Authorization", HttpContext.Current.Request.Headers.GetValues("Authorization").FirstOrDefault());
            }
        }

    }

    public class List<T, D>
    {
        public T Name { get; set; }
        public D Value { get; set; }
    }
}

