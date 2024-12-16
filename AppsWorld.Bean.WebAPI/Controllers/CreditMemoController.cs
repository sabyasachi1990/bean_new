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
using AppsWorld.CreditMemoModule.Application;
using AppsWorld.CreditMemoModule.Models;
using AppsWorld.CreditMemoModule.Entities;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/creditmemo")]
    public class CreditMemoController : BaseController
    {
        private readonly CreditMemoApplicationService _CreditMemoApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public CreditMemoController(CreditMemoApplicationService CreditMemoApplicationService)
        {
            this._CreditMemoApplicationService = CreditMemoApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcreditmemok")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCreditMemoK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
            );
                var model = _CreditMemoApplicationService.GetAllCreditMemoK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditmemo")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditMemo(Guid id)
        {
            try
            {
                CreditMemoModel model = _CreditMemoApplicationService.CreateCreditMemo(AuthInformation.companyId.Value, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcreditmemolus")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCreditNoteLUs(Guid creditid, DateTime? docdate = null)
        {
            try
            {
                CreditMemoLU model = _CreditMemoApplicationService.GetAllCreditNoteLU(AuthInformation.userName, AuthInformation.companyId.Value, creditid, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, docdate);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditmemo")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        ////[CacheInvalidate(Position = 11, Keys = "CreditMemoByBill", Controller = "BillController")]

        //[CacheInvalidate(Position = 11, Keys = "getallcreditmemok,createcreditmemo,getallcreditmemolus,createcreditmemoapplication")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Save(CreditMemoModel memoModel)
        {
            try
            {
                memoModel.CompanyId = AuthInformation.companyId.Value;
                memoModel.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                CreditMemo model = _CreditMemoApplicationService.SaveCreditMemo(memoModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(memoModel));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(CreditMemoModule.Infra.CreditMemoConstant.CreditMemoController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditmemoapplication")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditMemoApplication(Guid creditMemoId, Guid cmApplicationId, bool isView, DateTime applicationDate)
        {
            try
            {
                CreditMemoApplicationModel memo = _CreditMemoApplicationService.CreateCreditMemoApplication(creditMemoId, cmApplicationId, AuthInformation.companyId.Value, isView, applicationDate, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName);
                return Ok(memo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("CreateCreditMemoApplicationLU")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditMemoApplicationLU(Guid cmApplicationId)
        {
            try
            {
                return Ok(_CreditMemoApplicationService.creditmemoApplicationLu(AuthInformation.companyId.Value, cmApplicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditmemoapplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateMemoApplicationReset")]

        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBills", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceipts", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]

        //[CacheInvalidate(Position = 11, Keys = "createcreditmemoapplication,createcreditmemo")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditMemoApplication(CreditMemoApplicationModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                CreditMemoApplication memo = _CreditMemoApplicationService.SaveCreditMemoApplication(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection

);
                return Ok(memo);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(CreditMemoModule.Infra.CreditMemoConstant.CreditMemoController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savedocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication")]

        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBills", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceipts", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]

        //[CacheInvalidate(Position = 11, Keys = "getallcreditmemok,createcreditmemo,createcreditmemoapplication")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDocumentVoid(DocumentVoidModel VModel)
        {
            try
            {
                VModel.CompanyId = AuthInformation.companyId.Value;
                CreditMemo model = _CreditMemoApplicationService.SaveCreditMemoDocumentVoid(VModel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createcreditmemodocumentvoid")]
        // [CommonHeaders(Position = 1)]    public IHttpActionResult CreateCreditMemoDocumentVoid(Guid id, long companyId)
        //{
        //    try
        //    {
        //        DocumentVoidModel model = _CreditMemoApplicationService.CreateCreditMemoDocumentVoid(id, companyId);
        //        return Ok(model);
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);

        //    }
        //}
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditmemodocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK,CreateCreditMemo,CreateCreditMemoApplication")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditMemoDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                CreditMemo memo = _CreditMemoApplicationService.SaveCreditMemoDocumentVoid(TObject);
                return Ok(memo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getcreditmemodetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCreditMemoDetail()
        {
            try
            {
                CreditMemoDetailModel model = _CreditMemoApplicationService.GetCreditMemoDetail();
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("creditmemoapplicationreset")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateMemoApplicationReset(Guid id, Guid creditMemoId)
        {
            try
            {
                DocumentResetModel memo = _CreditMemoApplicationService.CreateCreditMemoApplicationReset(id, creditMemoId, AuthInformation.companyId.Value);
                return Ok(memo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditmemovoid")]
        ////[CacheInvalidate(Position = 11, Keys = "creditmemoapplicationreset")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBills", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceipts", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel", Controller = "BillController")]
        ////[CacheInvalidate(Position =11,Keys = "GetAllBillsK", "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "createcreditmemo,createcreditmemoapplication")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveMemoApplication(DocumentResetModel resetModel)
        {
            try
            {
                resetModel.CompanyId = AuthInformation.companyId.Value;
                CreditMemoApplication memo = _CreditMemoApplicationService.SaveCreditMemoApplicationReset(resetModel, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(memo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
