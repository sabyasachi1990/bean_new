using AppsWorld.Bean.WebAPI.Controllers.Attributes;
using AppsWorld.Bean.WebAPI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ziraff.FrameWork;
using System.Configuration;

namespace AppsWorld.Bean.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AutoMapperConfig.RegisterMappings();
            DomainEventsConfig.RegisterDomainEvents();
            SeriLogConfig.ConfigureLogs();


            GlobalConfiguration.Configuration.Services.Add(typeof(System.Web.Http.Filters.IFilterProvider), new WebApi.RedisCache.V2.BaseActionFilterAttribute.CustomFilterProvider());
            var providers = GlobalConfiguration.Configuration.Services.GetFilterProviders();
            var defaultprovider = providers.First(i => i is ActionDescriptorFilterProvider);
            GlobalConfiguration.Configuration.Services.Remove(typeof(System.Web.Http.Filters.IFilterProvider), defaultprovider);
            //AzureVaultKickStart();


            var scMode = ConfigurationManager.AppSettings["SecurityCredentialsMode"];
            //MI : ManagedIdentity, WC : WebConfig,  AD : ActiveDirectory(i.e. currently using)
            switch (scMode)
            {
                case "MI":
                    { AzureVaultMI(); }
                    break;
                case "WC":
                    { AzureVaultWC(); }
                    break;
                case "AD":
                    { AzureVaultAD(); }
                    break;
                default:
                    throw new System.Exception("SecurityCredentialsMode required");
            }

        }
        //private static void AzureVaultKickStart()
        //{
        //    KeyVaultProperty.SecretKey = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SecretKeyClientId"], ConfigurationManager.AppSettings["SecretKeyClientSecret"], ConfigurationManager.AppSettings["SecretKeyKeySecretUri"]);

        //    KeyVaultProperty.SkipAuthPassword = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SkipAuthPasswordClientId"], ConfigurationManager.AppSettings["SkipAuthPasswordClientSecret"], ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"]);
        //    Ziraff.FrameWork.SingleTon.CommonConnection.SecretKey = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AppsWorldDBContextClientId"], ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"], ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]);
        //    KeyVaultProperty.HangFireContext = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["HangFireContextClientId"], ConfigurationManager.AppSettings["HangFireContextClientSecret"], ConfigurationManager.AppSettings["HangFireContextKeySecretUri"]);
        //    //KeyVaultProperty.AzureStorage = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AzureStorageClientId"], ConfigurationManager.AppSettings["AzureStorageClientSecret"], ConfigurationManager.AppSettings["AzureStorageKeySecretUri"]);

        //}


        //Managed Identity
        private static void AzureVaultMI()
        {
            Ziraff.FrameWork.SingleTon.CommonConnection.SecretKey = Ziraff.FrameWork.ManagedIdentity.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SecretKeyKeySecretUri"]).Result;
            Ziraff.FrameWork.SingleTon.CommonConnection.SkipAuthPassword = Ziraff.FrameWork.ManagedIdentity.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"]).Result;
            Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection = Ziraff.FrameWork.ManagedIdentity.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]).Result;
            //Ziraff.FrameWork.SingleTon.CommonConnection.HangFireConnection = Ziraff.FrameWork.ManagedIdentity.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["HangFireContextKeySecretUri"]).Result;
            //Ziraff.FrameWork.SingleTon.CommonConnection.AzureStorage = Ziraff.FrameWork.ManagedIdentity.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AzureStorageKeySecretUri"]).Result;
        }

        //Web Config
        private static void AzureVaultWC()
        {
            Ziraff.FrameWork.SingleTon.CommonConnection.SecretKey = ConfigurationManager.AppSettings["SecretKey"];
            Ziraff.FrameWork.SingleTon.CommonConnection.SkipAuthPassword = ConfigurationManager.AppSettings["SkipAuthPassword"];
            Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection = ConfigurationManager.AppSettings["AppsWorldDBContext"];
        }

        //Azure Active Directory
        private static void AzureVaultAD()
        {


            Ziraff.FrameWork.SingleTon.CommonConnection.SecretKey = Ziraff.FrameWork.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SecretKeyClientId"], ConfigurationManager.AppSettings["SecretKeyClientSecret"], ConfigurationManager.AppSettings["SecretKeyKeySecretUri"]);
            Ziraff.FrameWork.SingleTon.CommonConnection.SkipAuthPassword = Ziraff.FrameWork.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SkipAuthPasswordClientId"], ConfigurationManager.AppSettings["SkipAuthPasswordClientSecret"], ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"]);
            Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection = Ziraff.FrameWork.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AppsWorldDBContextClientId"], ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"], ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]);
            CommonConnectionNew.SecondaryDbConnection = Ziraff.FrameWork.KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AppsWorldDBContextClientId"], ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"], ConfigurationManager.AppSettings["AppsWorldDBContextSecondaryDb"]);
            Dictionary<object, object> keys = new Dictionary<object, object>();
            keys.Add("SecondaryDbConnection", CommonConnectionNew.SecondaryDbConnection);
            Ziraff.FrameWork.SingleTon.CommonConnection.CommonKeys = keys;


        }






        //private static void AzureVaultKickStart()
        //{
        //    KeyVaultProperty.SecretKey = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SecretKeyClientId"], ConfigurationManager.AppSettings["SecretKeyClientSecret"], ConfigurationManager.AppSettings["SecretKeyKeySecretUri"]);

        //    KeyVaultProperty.SkipAuthPassword = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["SkipAuthPasswordClientId"], ConfigurationManager.AppSettings["SkipAuthPasswordClientSecret"], ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"]);
        //    Ziraff.FrameWork.SingleTon.CommonConnection.SecretKey = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AppsWorldDBContextClientId"], ConfigurationManager.AppSettings["AppsWorldDBContextClientSecret"], ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"]);
        //    //KeyVaultProperty.HangFireContext = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["HangFireContextClientId"], ConfigurationManager.AppSettings["HangFireContextClientSecret"], ConfigurationManager.AppSettings["HangFireContextKeySecretUri"]);
        //    //KeyVaultProperty.AzureStorage = KeyVaultService.GetSecret(ConfigurationManager.AppSettings["AzureStorageClientId"], ConfigurationManager.AppSettings["AzureStorageClientSecret"], ConfigurationManager.AppSettings["AzureStorageKeySecretUri"]);

        //}
    }
}
