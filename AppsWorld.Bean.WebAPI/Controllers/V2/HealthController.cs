using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;

namespace AppsWorld.Bean.WebAPI.Controllers.V2
{
    [RoutePrefix("api/Health")]
    public class HealthController : ApiController
    {

        [HttpGet]
        [Route("GetConfig")]
        public IHttpActionResult GetAppSettings()
        {
            try
            {
                return Ok(FillAppSettings());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        private AppSettings FillAppSettings()
        {
            AppSettings appSettings = new AppSettings
            {
                ScreenPermissionEnable = ConfigurationManager.AppSettings["ScreenPermissionEnable"],
                seqUrl = ConfigurationManager.AppSettings["seqUrl"],
                SecretKeyKeySecretUri = ConfigurationManager.AppSettings["SecretKeyKeySecretUri"],
                SkipAuthPasswordKeySecretUri = ConfigurationManager.AppSettings["SkipAuthPasswordKeySecretUri"],
                AppsWorldDBContextKeySecretUri = ConfigurationManager.AppSettings["AppsWorldDBContextKeySecretUri"],
                Authority = ConfigurationManager.AppSettings["Authority"],
                IsAuthorize = ConfigurationManager.AppSettings["IsAuthorize"],
                instrumentationKey = ConfigurationManager.AppSettings["instrumentationKey"],
                IsLogEnabled = ConfigurationManager.AppSettings["IsLogEnabled"],
                AdminUrl = ConfigurationManager.AppSettings["AdminUrl"],
                AzureUrl = ConfigurationManager.AppSettings["AzureUrl"],
                BeanUrl = ConfigurationManager.AppSettings["BeanUrl"],
                WorkflowUrl = ConfigurationManager.AppSettings["WorkflowUrl"],
                AzureFuncUrl = ConfigurationManager.AppSettings["AzureFuncUrl"],
                IsProtocol = ConfigurationManager.AppSettings["IsProtocol"],
                managedIdentityId = ConfigurationManager.AppSettings["managedIdentityId"],
                SecurityCredentialsMode = ConfigurationManager.AppSettings["SecurityCredentialsMode"]
            };
            return appSettings;
        }


    }
    public class AppSettings
    {
        public string ScreenPermissionEnable { get; set; }
        public string seqUrl { get; set; }
        public string SecretKeyKeySecretUri { get; set; }
        public string SkipAuthPasswordKeySecretUri { get; set; }
        public string AppsWorldDBContextKeySecretUri { get; set; }
        public string Authority { get; set; }
        public string IsAuthorize { get; set; }
        public string instrumentationKey { get; set; }
        public string IsLogEnabled { get; set; }
        public string AdminUrl { get; set; }
        public string AzureUrl { get; set; }
        public string BeanUrl { get; set; }
        public string WorkflowUrl { get; set; }
        public string AzureFuncUrl { get; set; }
        public string IsProtocol { get; set; }
        public string managedIdentityId { get; set; }
        public string SecurityCredentialsMode { get; set; }
    }
}
