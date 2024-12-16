using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.PaymentModule.Application;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Models;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/payment")]
    public class PaymentController : BaseController
    {

        #region API
        private readonly PaymentApplicationSevice _PaymentApplicationService;
       
        public PaymentController(PaymentApplicationSevice PaymentApplicationSevice)
        {
            _PaymentApplicationService = PaymentApplicationSevice;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("paymentslu")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetPaymentsLookups(Guid id)
        {
            try
            {
                PaymentModelLU model = _PaymentApplicationService.GetAllPaymentLUs(id, AuthInformation.companyId.Value, AuthInformation.userName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createpayment")]
        // //[Cache(Position =10,ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreatePayment(Guid paymentId, string docType)
        {
            try
            {
                PaymentModel model = _PaymentApplicationService.CreatePayment(paymentId, AuthInformation.companyId.Value, AuthInformation.userName , docType, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savepayment")]
        //[CacheInvalidate(Position = 11, Keys = "getallpayrollpaymentsk,paymentslu,createpayment,getpaymentdetails,getallpaymentsk ")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPayrollPaymentsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetPaymentsLookups")]
        ////[CacheInvalidate(Position = 11, Keys = "CreatePayment")]
        ////[CacheInvalidate(Position = 11, Keys = "GetPaymentDetails")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllpaymentsK")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBills", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceipts", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "PaymentByBill", Controller = "BillController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Save(PaymentModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_PaymentApplicationService.SavePayment(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(PaymentModule.Infra.Resources.PaymentsConstants.PaymentController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createpaymentdetails")]
        //[CommonHeaders(Position = 1)]   public IHttpActionResult CreatePaymentDetails(Guid paymentId, long companyId)
        //{
        //    try
        //    {
        //        var model = _PaymentApplicationService.CreatePaymentModel(paymentId, companyId);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        //[AllowAnonymous]
        [Route("getpaymentdetails")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetPaymentDetails(Guid paymentId, Guid entityId, string currency, DateTime? docDate, string docType, bool isInterCompany, bool? isCustomer)
        {
            try
            {
                var serviceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                var model = _PaymentApplicationService.GetPaymentDetails(paymentId, entityId, currency, AuthInformation.companyId.Value, serviceCompanyId,AuthInformation.userName ,docDate, docType, isInterCompany, isCustomer, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getcurrencylu")]
        //[RolePermissionFilter(ScreenName = Constant.Payments, PermissionName = Constant.View, ModuleName = Constant.BeanCursor)]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 6000)]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCurrencyLookup(Guid paymentId, Guid entityId, string baseCurrency, string bankCurrency, bool isMultyCurrency)
        {
            try
            {
                CurrencyLU model = _PaymentApplicationService.GetCurrencyLookup(entityId, AuthInformation.companyId.Value, baseCurrency, bankCurrency, isMultyCurrency, paymentId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savepaymentdocumentvoid")]
        //[CacheInvalidate(Position = 11, Keys = "createpayment,getpaymentdetails,getallpaymentsk ")]
        ////[CacheInvalidate(Position = 11, Keys = "CreatePayment")]
        ////[CacheInvalidate(Position = 11, Keys = "GetPaymentDetails")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllpaymentsK")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBills", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceipts", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPayrollPaymentsK")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNoteDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                //Payment model = _PaymentApplicationService.SavePaymentDocumentVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(_PaymentApplicationService.SavePayementVoidNew(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
                //return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallpaymentsk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllpaymentsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
               requestMessage.RequestUri.ParseQueryString().GetKey(0)
               );
                var model = _PaymentApplicationService.GetAllPaymentsK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Payroll Payment
        #region GridCall 

        [HttpGet]
        [Route("getallpayrollpaymentsk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public DataSourceResult GetAllPayrollPaymentsK(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
            return _PaymentApplicationService.GetAllPayrollPaymentsK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        #endregion

        #region Lookup

        [HttpGet]
        [Route("getentitylu")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetEntityLookUp()
        {
            try
            {
                return Ok(_PaymentApplicationService.GetEntityLU(AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #endregion

        #region WFGetCompanyFeature call
        [HttpGet]
        //[AllowAnonymous]
        [Route("getcompanyfeature")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCompanyFeature()
        {
            try
            {
                return Ok(_PaymentApplicationService.GetCompanyFeature(AuthInformation.companyId.Value, AuthInformation.moduleDetailId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


    }
}

