using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using Ziraff.FrameWork;
using System.Configuration;
using System;
using System.Web;

namespace AppsWorld.CommonModule.Infra
{
    public class RestSharpHelper
    {

        private static string _skipAuthPassword = null;
        private static string skipAuthPassword
        {
            get
            {
                if (_skipAuthPassword == null)
                {
                    //    _skipAuthPassword = KeyVaultService.GetSecret(
                    //ConfigurationManager.AppSettings["SkipAuthPasswordClientId"],
                    //ConfigurationManager.AppSettings["SkipAuthPasswordClientSecret"],
                    //ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"]);
                    _skipAuthPassword = Ziraff.FrameWork.SingleTon.CommonConnection.SkipAuthPassword;
                }
                return _skipAuthPassword;
            }
        }

        public static IRestResponse PostBasicAuthenication(string baseUrl, string resourceUrl, string userName,
            string password, string json)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["IsProtocol"].ToString().ToLower() == "true")
            {

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            }
            var client = new RestClient(baseUrl);
            client.Authenticator = new HttpBasicAuthenticator(userName, password);
            var request = new RestRequest(resourceUrl, Method.POST);
            request.AddParameter("text/json", json, ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;
            return client.Execute(request);
        }

        public static IRestResponse Post(string baseUrl, string resourceUrl, string json)
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
            //AddRestSharpHeader(request);
            return client.Execute(request);
        }

        public static string ConvertObjectToJason<T>(T arg)
        {
            return JsonConvert.SerializeObject(arg);
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


        //public static ResponseObject GetResponseObject(IRestResponse response)
        //{
        //    return JsonConvert.DeserializeObject<ResponseObject>(response.Content);
        //}
    }
}






