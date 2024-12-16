using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.Application;
using AppsWorld.RevaluationModule.Models;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/revaluation")]
    public class RevaluationController : BaseController
    {
        RevaluationApplicationService _revaluationApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public RevaluationController(RevaluationApplicationService revaluationApplicationService)
        {
            this._revaluationApplicationService = revaluationApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallrevaluationk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllRevaluationK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
               requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );

                var model = _revaluationApplicationService.GetAllRevaluationK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createrevaluation")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateRevaluation(DateTime dateTime, Guid id)
        {
            try
            {
                return Ok(_revaluationApplicationService.CreateRevaluation(dateTime, AuthInformation.companyId.Value, id, Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value)));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("revaluationlus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetRevaluationLookup(Guid id)
        {
            try
            {
                RevaluationLUs model = _revaluationApplicationService.RevaluationLUs(AuthInformation.companyId.Value, id, AuthInformation.userName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createrevaluationmodel")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateRevaluationModel()
        {
            try
            {
                RevaluationSaveModel model = _revaluationApplicationService.CreateRevaluationModel(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saverevaluation")]
        //[CacheInvalidate(Position = 11, Keys = "getallrevaluationk,createrevaluation,revaluationlus,createrevaluationmodel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRevaluationK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRevaluation")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRevaluationLookup")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRevaluationModel")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveRevalution(RevaluationSaveModel revaluationModel)
        {
            try
            {
                revaluationModel.CompanyId = AuthInformation.companyId.Value;
                revaluationModel.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok( _revaluationApplicationService.SaveRevaluation(revaluationModel,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(revaluationModel));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("RevaluationController", ex, ex.Message, AdditionalInfo);

                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecancelrevaluation")]
        //[CacheInvalidate(Position = 11, Keys = "getallrevaluationk,createrevaluation,revaluationlus,createrevaluationmodel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRevaluationK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRevaluation")]
        ////[CacheInvalidate(Position = 11, Keys = "GetRevaluationLookup")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRevaluationModel")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCancelRevalution(RevaluationCancelModel revCancelModel)
        {
            try
            {
                revCancelModel.CompanyId = AuthInformation.companyId.Value;
                Revaluation model = _revaluationApplicationService.SaveCancelRevaluation(revCancelModel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("revaluationfinancialmodel")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult RevaluationFinancialModel()
        {
            try
            {
                RevaluationFinancialModel model = _revaluationApplicationService.FinancialModel(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

