using AppsWorld.OpeningBalancesModule.Application;
using System.Collections.Generic;
using System.Web.Http;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.OpeningBalancesModule.Models;
using AppsWorld.OpeningBalancesModule.Entities;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/openingbalance")]
    public class OpeningBalanceController : BaseController
    {
        OpeningBalancesApplicationService _OpeningBalancesApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}

        public OpeningBalanceController(OpeningBalancesApplicationService OpeningBalancesApplicationService)
        {
            this._OpeningBalancesApplicationService = OpeningBalancesApplicationService;
        }

        #region KendoGrid_getallbalancessk
        [HttpGet]
        //[AllowAnonymous]
        [RolePermission(Position = 3)]
        [Route("getallbalancessk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult getallbalancessk(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
         );
                var model = _OpeningBalancesApplicationService.GetAllOpeningBalancessK(AuthInformation.companyId.Value, AuthInformation.userName).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Lookup_getopeningbalancelu
        [HttpGet]
        //[AllowAnonymous]
        [Route("getopeningbalancelu")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult getopeningbalancelu()
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                OpeningBalanceLU openingBalanceLU = _OpeningBalancesApplicationService.GetOpeningBalance(AuthInformation.companyId.Value, ServiceCompanyId, AuthInformation.userName);
                return Ok(openingBalanceLU);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetCall

        #region GetCall_getservicecompanyopeningbalance
        [HttpGet]
        //[AllowAnonymous]
        [Route("getservicecompanyopeningbalance")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 11, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetServiceCompanyOpeningBalance(bool? isInterCompanyActive)
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                OpeningBalanceModel openingBalanceModel = _OpeningBalancesApplicationService.GetServiceCompanyOpeningBalance(AuthInformation.companyId.Value, ServiceCompanyId, isInterCompanyActive, AuthInformation.userName);
                return Ok(openingBalanceModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region GetCall_getlineitemsforcoa
        [HttpGet]
        //[AllowAnonymous]
        [Route("getlineitemsforcoa")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetLineItemsForCOA(long COAId, string currency)
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                List<OpeningBalanceLineItemModel> model = _OpeningBalancesApplicationService.GetLineItemsForCOA(COAId, ServiceCompanyId, currency);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getlineitemsforcoas")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public PageResult<OpeningBalanceLineItemModel> GetLineItemsForCOA(ODataQueryOptions<OpeningBalanceLineItemModel> options, int pageSize, long COAId, string Currency,Guid detailId)
        {
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = pageSize
            };
            var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
            IQueryable Results = options.ApplyTo(_OpeningBalancesApplicationService.GetLineItemsForCOA(COAId, ServiceCompanyId, Currency, AuthInformation.companyId.Value, detailId,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection), settings);

            Uri uri = Request.GetNextPageLink();

            long? inlineCount = Request.GetInlineCount();
            PageResult<OpeningBalanceLineItemModel> Responce = new PageResult<OpeningBalanceLineItemModel>(
          Results as IEnumerable<OpeningBalanceLineItemModel>,
            uri,
            inlineCount);

            return Responce;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getlineitemsforcoass")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetLineItemsForCOAss(long COAId, string currency, Guid detailId)
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                IQueryable<OpeningBalanceLineItemModel> model = _OpeningBalancesApplicationService.GetLineItemsForCOA(COAId, ServiceCompanyId, currency,AuthInformation.companyId.Value, detailId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region GetCall_Getlineitems
        [HttpGet]
        //[AllowAnonymous]
        [Route("getlineitems")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Getlineitems()

        {
            try
            {
                OpeningBalanceLineItemModel model = _OpeningBalancesApplicationService.Getlineitems();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region _GetLineItemsByCOAID
        [HttpGet]
        //[AllowAnonymous]
        [Route("getdetailsbycoaid")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetLineItemsByCOAID(long COAId)
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                List<OpeningBalanceDetailModel> model = _OpeningBalancesApplicationService.GetDetailsByCOAID(COAId, ServiceCompanyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region DeleteAllUnnecessary_record_from_OB_OBD_OBLD
        [HttpGet]
        //[AllowAnonymous]
        [Route("deletetempob")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult DeleteTempOB(Guid obId)
        {
            try
            {
                return Ok(_OpeningBalancesApplicationService.DeleteTempRecordFromOB_OBD_OBLD(obId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion DeleteAllUnnecessary_record_from_OB_OBD_OBLD
        #endregion GetCall
        #region save saveopeningbalance
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveopeningbalance")]
        //[CacheInvalidate(Position = 11, Keys = "getservicecompanyopeningbalance,getallbalancessk,getdetailsbycoaid,getlineitemsforcoa")]
        //[CacheInvalidate(Position = 11, Keys = "getopeningbalancelu")]
        //[CacheInvalidate(Position = 11, Keys = "GetSaleItems", Controller = "MasterModuleController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        ////[CacheInvalidate(Position = 11, Keys = "GetServiceCompanyOpeningBalance")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllOpeningBalancessK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetLineItemsByCOAID")]
        ////[CacheInvalidate(Position = 11, Keys = "GetLineItemsForCOA")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Save(OpeningBalanceModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                OpeningBalance openingBalance = _OpeningBalancesApplicationService.SaveOpeningBalanceNew(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);

                return Ok(openingBalance);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(OpeningBalancesModule.Infra.OpeningBalanceLoggingValidation.OpeningBalancesController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("SaveOpeningBalanceMaster")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 11, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveOpeningBalanceMaster(OpeningBalanceModel openingBalanceModel)
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                OpeningBalanceModel openingBalance = _OpeningBalancesApplicationService.SaveOpeningBalanceMasterNew(openingBalanceModel, AuthInformation.companyId.Value, ServiceCompanyId);
                return Ok(openingBalance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("saveopeningbalancenew")]
        //[CacheInvalidate(Position = 11, Keys = "getservicecompanyopeningbalance,getallbalancessk,getdetailsbycoaid,getlineitemsforcoa")]
        //[CacheInvalidate(Position = 11, Keys = "getopeningbalancelu")]
        //[CacheInvalidate(Position = 11, Keys = "GetSaleItems", Controller = "MasterModuleController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        ////[CacheInvalidate(Position = 11, Keys = "GetServiceCompanyOpeningBalance")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllOpeningBalancessK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetLineItemsByCOAID")]
        ////[CacheInvalidate(Position = 11, Keys = "GetLineItemsForCOA")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveOBNew(OpeningBalanceModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                OpeningBalance openingBalance = _OpeningBalancesApplicationService.SaveOpeningBalanceNewRecent(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(openingBalance);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(OpeningBalancesModule.Infra.OpeningBalanceLoggingValidation.OpeningBalancesController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("saveobdetailitem")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDetailItem(OpeningBalanceDetailModel TObject)
        {
            try
            {
                long CompanyId = AuthInformation.companyId.Value;
                long ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_OpeningBalancesApplicationService.SaveOBDetailLineItems(TObject, ServiceCompanyId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, CompanyId, TObject.OpeningBalanceId));

            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(OpeningBalancesModule.Infra.OpeningBalanceLoggingValidation.OpeningBalancesController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}