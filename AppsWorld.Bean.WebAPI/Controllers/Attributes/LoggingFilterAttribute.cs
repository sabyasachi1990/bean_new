using System;
using System.Web;
using Serilog;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;
using Logger;

namespace AppsWorld.Bean.WebAPI.Controllers.Attributes
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            var data = new 
            {
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Action = filterContext.ActionDescriptor.ActionName,
                IP = filterContext.Request.GetClientIpAddress(),
            };
            Log.Logger.ZInfo(data);
            base.OnActionExecuting(filterContext);
        }

        //public override void OnActionExecuted(HttpActionExecutedContext filterContext)
        //{
        //    Log.Debug("{DateTime}{@context}{Method}", DateTime.Now, filterContext.ActionContext.RequestContext, filterContext.ActionContext.ActionDescriptor.ActionName);
        //    base.OnActionExecuted(filterContext);
        //}

        
    }

    public class NotImplExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
        }
    }

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }

    public static class HttpRequestMessageExtensions
    {
        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            string contextname = string.Empty;
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                contextname = "MS_HttpContext";
            }
            if (request.Properties.ContainsKey("MS_OwinContext"))
            {
                contextname = "MS_OwinContext";
            }

            var myRequest = ((HttpContextWrapper)request.Properties[contextname]).Request;
            var ip = myRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ip))
            {
                string[] ipRange = ip.Split(',');
                int le = ipRange.Length - 1;
                return ipRange[0];
            }
            else
            {
                return myRequest.ServerVariables["REMOTE_ADDR"];
            }
        }
    }  

    
}