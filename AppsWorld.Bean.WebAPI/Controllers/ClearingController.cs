using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Models;
//using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;
using AppsWorld.GLClearingModule.Application;
//using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.GLClearingModule.Models;
using AppsWorld.GLClearingModule.Entities;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;

namespace AppsWorld.Bean.WebAPI.Controllers
{

    [RoutePrefix("api/clearing")]
    public class ClearingController : BaseController
    {
        private readonly GLClearingApplicationService _ClearingApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public ClearingController(GLClearingApplicationService ClearingApplicationService)
        {
            this._ClearingApplicationService = ClearingApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallclearinglus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllClearingLUs(Guid id)
        {
            try
            {
                ClearingModelLUs model = _ClearingApplicationService.GetClearingLUs(AuthInformation.userName, id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createclearing")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateClearing(Guid id)
        {
            try
            {
                //var CompanyId =19;
                ClearingModel model = _ClearingApplicationService.CreateClearing(id, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createclearingdetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateClearingDetail(DateTime? fromDate, DateTime? toDate, long coaId, bool? IsClearingChecked)
        {
            try
            {
                var serviceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                //var serviceCompanyId = 263;
                List<ClearingDetailModel> model = _ClearingApplicationService.CreateClearingDetailNew(fromDate, toDate, coaId, serviceCompanyId, IsClearingChecked, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region GetAllClearingK
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getallclearingk")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = true)]
        //[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetAllClearingK(HttpRequestMessage requestMessage)
        //{
        //    try
        //    {
        //        DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
        //      requestMessage.RequestUri.ParseQueryString().GetKey(0)
        //    );
        //        var model = _ClearingApplicationService.GetAllClearingK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        #endregion

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallclrearedk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllClrearedK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );
                //TObject.CompanyId = AuthInformation.companyId.Value;
                //var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                var model = _ClearingApplicationService.GetAllClrearedK(AuthInformation.companyId.Value,AuthInformation.userName).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveclearing")]
        // //[Cache(Position =10,ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        //[CacheInvalidate(Position = 11, Keys = "GetAllClearingK")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearing")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllClearingLUs")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingReset")]
        //[CacheInvalidate(Position = 11, Keys = "getallclrearedk")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveClearing(ClearingModel TObject)
        {
            try
            {
                //TObject.CompanyId = 19;
                //TObject.ServiceCompanyId = 20;
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                GLClearing model = _ClearingApplicationService.SaveClearing(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("ClearingController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createclearingreset")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult CreateClearingReset(Guid id)
        //{
        //    try
        //    {
        //        DocumentResetModel model = _ClearingApplicationService.CreateClearingReset(id, AuthInformation.companyId.Value);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //Form Level
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveclearingunclear")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllClearingK")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearing")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingReset")]
        //[CacheInvalidate(Position = 11, Keys = "getallclrearedk")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveClearingReset(DocumentResetModel TObject)
        {
            try
            {
                long CompanyId = Convert.ToUInt32(AuthInformation.companyId.Value);
                _ClearingApplicationService.SaveUnclearForm(TObject, CompanyId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("saveunclear")]
        // //[Cache(Position =10,ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        //[CacheInvalidate(Position = 11, Keys = "GetAllClearingK")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingDetail")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearing")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllClearingLUs")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearingReset")]
        //[CacheInvalidate(Position = 11, Keys = "getallclrearedk")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveUnclear(UnclearModel TObject)
        {
            try
            {
                //TObject.CompanyId = 19;
                //TObject.ServiceCompanyId = 20;
                long CompanyId = Convert.ToUInt32(AuthInformation.companyId.Value);
                _ClearingApplicationService.SaveClearingGrid(TObject, CompanyId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
