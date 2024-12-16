using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AppsWorld.DebitNoteModule.Models;
using AppsWorld.DebitNoteModule.Application;
using AppsWorld.DebitNoteModule.Entities;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/debitnote")]
    public class DebitNoteController : BaseController
    {
        DebitNoteApplicationService _DebitNoteApplicationService;
        //AuthInformation authInfo = GetAuthInfo(System.Web.HttpContext.Current.Request.Headers);

        //private static AuthInformation GetAuthInfo(NameValueCollection headers)
        //{
        //    return headers["AuthInformation"] != null ? JsonConvert.DeserializeObject<AuthInformation>(headers.GetValues("AuthInformation").FirstOrDefault().ToString()) : new Controllers.AuthInformation();
        //}
        public DebitNoteController(DebitNoteApplicationService DebitNoteApplicationService)
        {
            _DebitNoteApplicationService = DebitNoteApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getdebitnotealllus")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDebitNoteAllLUs(Guid debitNoteId)
        {
            try
            {
                //DebitNoteModelLU model = _DebitNoteApplicationService.GetDebitNoteAllLUs(AuthInformation.userName, debitNoteId, AuthInformation.companyId.Value);
                DebitNoteModelLU model= _DebitNoteApplicationService.NewGetDebitNoteAllLUs(AuthInformation.userName, debitNoteId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdebitnote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDebitNote(Guid id,bool isCopy)
        {
            try
            {
                DebitNoteModel model = _DebitNoteApplicationService.CreateDebitNote(AuthInformation.companyId.Value, id,isCopy,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldebitnotek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> getalldebitnotek(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = await _DebitNoteApplicationService.GetAllDebitNotesK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {

               return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getdebitnotedetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDebitNoteDetail(Guid debitNoteDetailId, Guid debitNoteId)
        {
            try
            {
                DebitNoteDetail model = _DebitNoteApplicationService.GetDebitNoteDetail(debitNoteDetailId, debitNoteId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savedebitnote")]
        //////[CacheInvalidate(Position =11,Keys = "GetAllDebitNotesK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteAllLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptModelEdit", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication", Controller = "InvoiceController")]

        //[CacheInvalidate(Position = 11, Keys = "getalldebitnotek,GetDebitNoteAllLUs,createdebitnote,getdebitnotedetail,getdebitnotenote,createreceiptbydebitnote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDebitNote(DebitNoteModel debitNote)
        {
            try
            {
                debitNote.CompanyId = AuthInformation.companyId.Value;
                debitNote.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                DebitNote dn = _DebitNoteApplicationService.SaveDebitNote(debitNote, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(dn);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(debitNote));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(DebitNoteModule.Infra.Resources.DebitNoteConstants.DebitNoteController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getdebitnotenote")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDebitNoteNote(Guid debitNoteNoteId, Guid debitNoteId)
        {
            try
            {
                DebitNoteNote model = _DebitNoteApplicationService.GetDebitNoteNote(debitNoteNoteId, debitNoteId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("debitnotenotesave")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "createdebitnote,getalldebitnotek,getdebitnotedetail,getdebitnotenote,createdoubtfuldebtbydebitnote,createcreditnotebydebitnote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult DebitNoteNoteSave(DebitNoteNote debitNoteNote)
        {
            try
            {
                DebitNoteNote note = _DebitNoteApplicationService.Save(debitNoteNote, AuthInformation.companyId.Value);
                return Ok(note);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdebitnotedocumentvoid")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDebitNoteDocumentVoid(Guid id)
        {
            try
            {
                DocumentVoidModel model = _DebitNoteApplicationService.CreateDebitNoteDocumentVoid(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savedebitnotedocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDebitNoteDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptModelEdit", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "createdebitnote,getalldebitnotek,getdebitnotedetail,getdebitnotenote,createdoubtfuldebtbydebitnote,createcreditnotebydebitnote,createreceiptbydebitnote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDebitNoteDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                //TObject.ModifiedBy = AuthInformation.userName;
                DebitNote model = _DebitNoteApplicationService.SaveDebitNoteDocumentVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditnotebydebitnote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNoteByDebitNote(Guid debitNoteId)
        {
            try
            {
                CreditNoteModel model = _DebitNoteApplicationService.CreateCreditNoteByDebitNote(debitNoteId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebtbydebitnote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDoubtFulDebtByDebitNote(Guid debitNoteId)
        {
            try
            {
                DoubtfulDebtModel model = _DebitNoteApplicationService.CreateDoubtFulDebtByDebitNote(debitNoteId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createreceiptbydebitnote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateReceiptByDebitNote(Guid debitNoteId)
        {
            try
            {
                ReceiptModel model = _DebitNoteApplicationService.CreateReceiptByDebitNote(debitNoteId, AuthInformation.companyId.Value,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getdebitprovision")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetDebitProvision(Guid id, long companyId)
        //{
        //    try
        //    {
        //        ProvisionModel model = _DebitNoteApplicationService.GetProvision(id, AuthInformation.companyId.Value);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost]
        //[AllowAnonymous]
        [Route("savedebitnotenotes")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDebitNoteNote(List<DebitNoteNoteModel> debitNoteNote)
        {
            try
            {
                List<DebitNoteNote> note = _DebitNoteApplicationService.SaveDebitNoteNote(debitNoteNote, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(note);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
