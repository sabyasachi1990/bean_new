using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.ReceiptModule.Application;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using System;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Infra;
using Ziraff.FrameWork.Logging;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/receipt")]
    public class ReceiptController : BaseController
    {
        ReceiptApplicationService _ReceiptApplicationService;

        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public ReceiptController(ReceiptApplicationService ReceiptApplicationService)
        {
            this._ReceiptApplicationService = ReceiptApplicationService;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("receipts")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult receipts()
        {
            try
            {
                List<ReceiptModel> model = _ReceiptApplicationService.GetAllReceipts(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("receipts")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllReceipts(ODataQueryOptions<ReceiptModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_ReceiptApplicationService.GetAllReceipts(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<ReceiptModel> response = new PageResult<ReceiptModel>(
                    results as IEnumerable<ReceiptModel>,
                    uri,
                    inlineCount);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("receiptslu")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetReceiptsLookups(Guid id,DateTime? docdate=null)
        {
            try
            {
                return Ok( _ReceiptApplicationService.GetAllReceiptLUs(AuthInformation.userName, id, AuthInformation.companyId.Value,docdate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createreceipt")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateReceipt(Guid receiptId)
        {
            try
            {
                ReceiptModel model = _ReceiptApplicationService.CreateReceiptNew(receiptId, AuthInformation.companyId.Value, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("generateautonotype")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GenerateAutoNumberTypr(string Type, string companyCode)
        {
            try
            {
                string model = _ReceiptApplicationService.GenerateAutoNumberForType(AuthInformation.companyId.Value, Type, companyCode);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savereceipt")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceipt,GetReceiptsLookups,GetAllReceipts,GetAllReceiptsK,CreateReceiptBalancingItem,CreateReceiptDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceipt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetReceiptsLookups")]
        ////[CacheInvalidate(Position = 11, Keys = "receipts")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceipts")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceiptsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptBalancingItem")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptDetail")]
        //invalidate all invoice controller get/create/kendo calls
        //[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateInvoice", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice", Controller = "InvoiceController")]
        //invalidate all DebitNote Controller get/create/kendo calls
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetDebitNoteDetail", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptByDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Save(ReceiptModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                TObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Receipt receipt = _ReceiptApplicationService.Save(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(receipt);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(ReceiptModule.Infra.ReceiptLoggingValidation.ReceiptController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createreceiptdetails")]
        //[CommonHeaders(Position = 1)]   public IHttpActionResult CreatereceiptDetails(Guid receiptId, long companyId)
        //{
        //          try
        //          {
        //              List<ReceiptDetailModel> model = _ReceiptApplicationService.CreateReceiptModel(receiptId, companyId);
        //              return Ok(model);
        //          }
        //          catch (Exception ex)
        //          {
        //              return BadRequest(ex.Message);
        //          }
        //}
        [HttpGet]
        //[AllowAnonymous]
        [Route("getreceiptdetail")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateReceiptModelEdit(Guid receiptId, Guid entityId, string currency, DateTime? docDate, bool isInterCompanyActive, bool isVendor)
        {
            try
            {
                var serviceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                ReceiptDetailModel model = _ReceiptApplicationService.GetReceiptDetails(receiptId, entityId, currency, AuthInformation.companyId.Value, serviceCompanyId,AuthInformation.userName ,docDate, isInterCompanyActive, isVendor, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
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
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCurrencyLookup(Guid receiptId, Guid entityId, string baseCurrency, string bankCurrency, bool isMultyCurrency)
        {
            try
            {
                CurrencyLU model = _ReceiptApplicationService.GetCurrencyLookup(receiptId, entityId, AuthInformation.companyId.Value, baseCurrency, bankCurrency, isMultyCurrency);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createreceiptdocumentvoid")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNoteDocumentVoid(Guid id)
        {
            try
            {
                DocumentVoidModel model = _ReceiptApplicationService.CreateCreditNoteDocumentVoid(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savereceiptdocumentvoid")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceipt,receipts,GetAllReceiptsK,getreceiptdetail,CreateReceiptBalancingItem,CreateReceiptDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceipt")]
        ////[CacheInvalidate(Position = 11, Keys = "receipts")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceipts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceiptsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptModelEdit")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptBalancingItem")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptDetail")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNoteDocumentVoid(DocumentVoidModel TObject)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Receipt model = _ReceiptApplicationService.SaveCreditNoteDocumentVoidNew(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ReceiptLoggingValidation.ReceiptController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createreceiptbalancingitem")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateReceiptBalancingItem(Guid id, Guid receiptId)
        {
            try
            {
                ReceiptBalancingItem model = _ReceiptApplicationService.CreateBalancingItem(id, receiptId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createreceiptdetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateReceiptDetail(Guid id, Guid receiptId)
        {
            try
            {
                ReceiptDetail model = _ReceiptApplicationService.CreateReceiptDetail(id, receiptId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("isgstallowed")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public bool IsGSTAllowed(DateTime docDate)
        {
            return _ReceiptApplicationService.IsGSTAllowed(AuthInformation.companyId.Value, docDate);
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("GetAllReceiptsK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllReceiptsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
               requestMessage.RequestUri.ParseQueryString().GetKey(0));

                var model =  _ReceiptApplicationService.GetAllReceiptsK(AuthInformation.userName, AuthInformation.companyId.Value);;
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

