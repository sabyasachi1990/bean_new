using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;
using Ziraff.FrameWork.Logging;

namespace AppsWorld.Bean.WebAPI.Utils
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommonHeadersAttribute : BaseActionFilterAttribute
    {
        private DateTime starttime { get; set; }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            starttime = DateTime.UtcNow;
            try
            {
                var controller = actionContext.ControllerContext.Controller as BaseController;
                AuthInformation authInformationModel = new AuthInformation();
                if (actionContext.Request.Headers != null && actionContext.Request.Headers.Contains(ConstantVariables.AuthInformation))
                {

                    var encryptedAuth = actionContext.Request.Headers.GetValues(ConstantVariables.AuthInformation).FirstOrDefault();
                    if (encryptedAuth != null)
                    {
                        if (!(actionContext.Request.Headers.Contains("SkipAuthPassword") && actionContext.Request.Headers.GetValues("SkipAuthPassword").FirstOrDefault() == Ziraff.FrameWork.SingleTon.CommonConnection.SkipAuthPassword))
                        {
                            encryptedAuth = AESEncryptDecrypt.DecryptStringAESNew(encryptedAuth);
                        }
                        authInformationModel = JsonConvert.DeserializeObject<AuthInformation>(encryptedAuth.ToString());
                        HttpContext.Current.Request.Headers.Add("BasicParams", encryptedAuth);
                        controller.AuthInformation = authInformationModel;

                        if (!actionContext.Request.Headers.Contains("LogCorrelationID"))
                        {
                            AddCorRelationID(actionContext, controller);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("CommonHeaders", ex, $"{ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
            finally
            {
                //var milliSecInCommon = (DateTime.UtcNow - starttime).TotalMilliseconds;
                LoggingHelper.LogMessage("CommonHeaders", $"ElapsedMilliseconds: {(DateTime.UtcNow - starttime).TotalMilliseconds}, CommonHeader execution time.");
            }
        }
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                //var elaspedinms = (DateTime.UtcNow - starttime).TotalMilliseconds;
                var currenturi = actionExecutedContext != null ? (actionExecutedContext.Request != null ? (actionExecutedContext.Request.RequestUri) : null) : null;
                LoggingHelper.LogMessage("CallExecution", $"ElapsedMilliseconds:{(DateTime.UtcNow - starttime).TotalMilliseconds}, Execution time for {currenturi} call");
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("CommonHeaders", ex, $"Exception occured in OnActionExecuted- {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }

        }


        void AddCorRelationID(HttpActionContext actionContext, BaseController controller)
        {
            try
            {
                var companyId = controller.AuthInformation.companyId ?? -1;
                var methodName = (actionContext.Request == null ? (null) : (actionContext.Request.RequestUri == null ? (null) : (actionContext.Request.RequestUri.LocalPath)));
                if (methodName != null)
                {
                    methodName = methodName.Replace("/api/", "");
                    methodName = methodName.Replace("/", "-");
                }
                var logCorrelationID = $"{ controller.AuthInformation.userName}_{DateTime.UtcNow.ToString()}_{(companyId)}_{methodName ?? null}";
                HttpContext.Current.Request.Headers.Add("LogCorrelationID", logCorrelationID);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError("CommonHeaders", ex, $"Exception occured in AddCorRelationID- {ex.Message}");
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }

}