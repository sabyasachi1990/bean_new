using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CashSalesModule.Application.V2;
using AppsWorld.CashSalesModule.Models;
using AppsWorld.CommonModule.Models;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V2
{
    [RoutePrefix("api/v2/cashsales")]
    public class CashSalesMainController : BaseController
    {
        private readonly CashSaleApplicationService _cashSalesApplicationService;
        public CashSalesMainController(CashSaleApplicationService cashSalesApplicationService)
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
                return Ok(_cashSalesApplicationService.SaveCashSale(TObject));
            }
            catch (Exception ex)
            {
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
                return Ok(_cashSalesApplicationService.SaveCashSaleDocumentVoid(TObject));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getcashsale")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCashSaleDetail(Guid id)
        {
            try
            {
                return Ok(_cashSalesApplicationService.GetCashSale(id));
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
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCashSales(Guid id)
        {
            try
            {
                return Ok(_cashSalesApplicationService.CreateCashSales(AuthInformation.companyId.Value, id));
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
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCashSalesLUs(Guid cashsaleId)
        {
            try
            {
                return Ok(_cashSalesApplicationService.GetAllCashSalesLUs(AuthInformation.userName, cashsaleId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
