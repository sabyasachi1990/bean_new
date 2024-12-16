
using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CashSalesModule.Application;
using AppsWorld.CashSalesModule.Entities;
using AppsWorld.CashSalesModule.Entities.Models;
using AppsWorld.CashSalesModule.Models;
using AppsWorld.CommonModule.Models;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/cashsales")]
    public class CashSalesController : BaseController
    {
        CashSaleApplicationService _cashSalesApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public CashSalesController(CashSaleApplicationService cashSalesApplicationService)
        {
            this._cashSalesApplicationService = cashSalesApplicationService;
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savecashsale")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesLUs")]
        //[CacheInvalidate(Position = 11, Keys = "GetCashSaleDetail")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCashSales")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesK")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCashSale(CashSaleModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                CashSale cashsale = _cashSalesApplicationService.SaveCashSale(TObject,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(cashsale);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(CashSalesModule.Infra.CommonConstants.CashSaleController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savecashsaledocumentvoid")]
        //[CacheInvalidate(Position = 11, Keys = "GetCashSaleDetail")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCashSales")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesK")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveWithdrawalDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;                
                CashSale model = _cashSalesApplicationService.SaveCashSaleDocumentVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getcashsale")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCashSaleDetail(Guid id)
        {
            try
            {
                CashSaleDetailModel model = _cashSalesApplicationService.GetCashSale(id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcashsales")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCashSales(Guid id, bool isCopy)
        {
            try
            {
                CashSaleModel model = _cashSalesApplicationService.CreateCashSales(AuthInformation.companyId.Value, id, isCopy,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcashsaleslu")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true,IsUsernameRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCashSalesLUs(Guid cashsaleId)
        {
            try
            {
                CashSaleModelLU model = _cashSalesApplicationService.GetAllCashSalesLUs(AuthInformation.userName, cashsaleId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        //[Route("getcashandbankcurrencylu")]
        // [CommonHeaders(Position = 1)]    public IHttpActionResult GetCashAndBankCurrencyLU(Guid cashsaleId,long companyId, string currency)
        //{
        //    try
        //    {
        //        CashAndBankCurrencyLU model = _cashSalesApplicationService.GetCashAndBankCurrencyLU(cashsaleId, companyId, currency);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcashsalesk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true,IsUsernameRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCashSalesK(HttpRequestMessage requestMessage)
        {
            try
            {
                //string authInformation = System.Web.HttpContext.Current.Request.Headers.GetValues("AuthInformation").FirstOrDefault();
                //AuthInformation authInfo = JsonConvert.DeserializeObject<AuthInformation>(authInformation.ToString());

                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
            requestMessage.RequestUri.ParseQueryString().GetKey(0)
         );
                var model = _cashSalesApplicationService.GetAllCashSalesK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
