using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Models;
using AppsWorld.InvoiceModule.Application;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.Infra.Resources;
using AppsWorld.InvoiceModule.Models;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/invoice")]
    public class InvoiceController : BaseController
    {
        InvoiceApplicationService _invoiceApplicationService;
        CommonApplicationService _commonAppService;


        public InvoiceController(InvoiceApplicationService invoiceApplicationService, CommonApplicationService commonAppService)
        {
            this._invoiceApplicationService = invoiceApplicationService;
        }






        [HttpGet]
        //[AllowAnonymous]
        [Route("getallinvoiceskold")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllInvoicesKOld(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var querData = await _invoiceApplicationService.GetAllInvoicesKOld(AuthInformation.userName, AuthInformation.companyId.Value);
                var model = querData.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region Invoice

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallinvoicesk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllInvoicesK(HttpRequestMessage requestMessage)
        {
            try
            {
                var queryParams = requestMessage.RequestUri.ParseQueryString();
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(queryParams.GetKey(0));

                var result = await _invoiceApplicationService.GetAllInvoicesK(AuthInformation.userName, AuthInformation.companyId.Value);
                var querData = result.Item1;
                var recordsCount = result.Item2;
                var model = await querData.ToDataSourceResultAsync(request.Take, request.Skip, request.Sort, request.Filter, recordsCount);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createinvoice")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> CreateInvoice(Guid id, bool isCopy)
        {
            try
            {
                InvoiceModel model = await _invoiceApplicationService.CreateInvoice(AuthInformation.companyId.Value, id, isCopy, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallinvoicelus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllInvoiceLUs(Guid invoiceId)
        {
            try
            {
                return Ok( await _invoiceApplicationService.NewGetAllInvoiceLUs(AuthInformation.userName, invoiceId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveinvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "getallinvoicelus,getinvoicedetail,createinvoice,getallinvoicesk,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createreceiptbyinvoice,createcreditnoteapplication,getallpostedrecurringsK")]
        ////[CacheInvalidate(Position = 12, Keys = "CreateReceiptModelEdit", Controller = "ReceiptController")]
        ////[CacheInvalidate(Position = 13, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        //////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        //////[CacheInvalidate(Position = 11, Keys = "getallpostedrecurringsK")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInVoice(InvoiceModel Invoice)
        {
            try
            {
                Invoice.CompanyId = AuthInformation.companyId.Value;
                Invoice.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Invoice Invoicemodel = _invoiceApplicationService.SaveInvoice(Invoice, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(Invoicemodel);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>() 
                { 
                    { "Data", JsonConvert.SerializeObject(Invoice) } 
                };
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getallinvoicelus")]
        // [CommonHeaders(Position = 1)]    public IHttpActionResult GetAllInvoiceLUs(Guid invoiceId, long companyid)
        //{
        //    try
        //    {
        //        InvoiceModelLU model = _invoiceApplicationService.GetAllInvoiceLUs(invoiceId, companyid);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        //[AllowAnonymous]
        [Route("getinvoicedetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetInvoiceDetail(Guid invoiceId, Guid invoiceDetalId)
        {
            try
            {
                InvoiceDetail model = _invoiceApplicationService.GetInvoiceDetail(invoiceId, invoiceDetalId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getinvoicegstdetail")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetInvoiceGSTDetail(Guid invoiceId, Guid invoiceGSTDetalId)
        //{
        //    try
        //    {
        //        InvoiceGSTDetail model = _invoiceApplicationService.GetInvoiceGSTDetail(invoiceId, invoiceGSTDetalId);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        //[AllowAnonymous]
        [Route("getinvoicenote")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetInvoiceNote(Guid invoiceId, Guid invoiceNoteId)
        {
            try
            {
                InvoiceNote model = _invoiceApplicationService.GetInvoiceNote(invoiceId, invoiceNoteId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createinvoicedocumentvoid")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateInvoiceDocumentVoid(Guid id)
        {
            try
            {

                DocumentVoidModel model = _invoiceApplicationService.CreateInvoiceDocumentVoid(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveinvoicedocumentvoid")]
        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,createcreditnote,getallcreditnotelus,getallinvoicesk,createinvoice,getallinvoicelus,getinvoicedetail,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes,CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,GetAllParkedInvoiceK,getallparkedinvoicek,getallvoidinvoicek,getallpostedrecurringsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        //////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        //[CacheInvalidate(Position = 12, Keys = "CreateReceipt", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 13, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedInvoiceK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringInvoiceK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoidInvoiceK")]
        ////[CacheInvalidate(Position = 11, Keys = "getallpostedrecurringsK")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInvoiceDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Invoice model = _invoiceApplicationService.SaveInvoiceDocumentVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getprovision")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetProvision(Guid id, Guid invoiceId)
        {
            try
            {
                ProvisionModel model = _invoiceApplicationService.GetProvision(id, AuthInformation.companyId.Value, invoiceId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditnotebyinvoice")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNoteByInvoice(Guid invoiceId)
        {
            try
            {
                CreditNoteModel model = _invoiceApplicationService.CreateCreditNoteByInvoice(invoiceId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebtbyinvoice")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDoubtFulDebtByInvoice(Guid invoiceId)
        {
            try
            {
                DoubtfulDebtModel model = _invoiceApplicationService.CreateDoubtFulDebtByInvoice(invoiceId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createreceiptbyinvoice")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateReceiptByInvoice(Guid invoiceId)
        {
            try
            {
                ReceiptModel model = _invoiceApplicationService.CreateReceiptByInvoice(invoiceId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("moduleactivations")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ModuleActivations()
        {
            try
            {
                ModuleActivationModel model = _invoiceApplicationService.ModuleActivations(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("saveinvoicenote")]
        ////[CacheInvalidate(Position = 11, Keys = "createinvoicenote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInvoiceNote(InvoiceNote note)
        {
            try
            {
                InvoiceNote invoiceNote = _invoiceApplicationService.SaveInvoiceNote(note);
                return Ok(invoiceNote);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("taxLU")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult TaxLU(DateTime date, string docSubType = null)
        {
            try
            {
                var model = _invoiceApplicationService.TaxCodeLU(date, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection,docSubType);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createinvoicenote")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateInvoiceNote(Guid id, Guid invoiceId)
        {
            try
            {
                var model = _invoiceApplicationService.CreateInvoiceNote(id, invoiceId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]xz
        [Route("saveinvoicenotes")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInVoiceNote(List<InvoiceNoteModel> InvoiceNote)
        {
            try
            {
                List<InvoiceNote> Invoicemodel = _invoiceApplicationService.SaveInvoiceNotes(InvoiceNote, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(Invoicemodel);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(InvoiceNote));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region CreditNote


        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcreditnoteK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllCreditNoteK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
                requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = await _invoiceApplicationService.GetAllCreditNoteK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditnote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNote(Guid id)
        {
            try
            {
                CreditNoteModel model = _invoiceApplicationService.CreateCreditNote(AuthInformation.companyId.Value, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcreditnotelus")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllCreditNoteLUs(Guid creditid, DateTime? docdate = null)
        {
            try
            {
                return Ok(await _invoiceApplicationService.NewGetAllCreditNoteLU(AuthInformation.userName, AuthInformation.companyId.Value, creditid, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, docdate));
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditnote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetProvision")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        // ////[CacheInvalidate(Position =11,Keys = "CreateCreditNoteByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,getallinvoicesk,createcreditnote,getallcreditnotelus,createinvoice,getallinvoicelus,getinvoicedetail,getinvoicegstdetail,getinvoicenote,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes, CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptByDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNote(CreditNoteModel dto)
        {
            try
            {
                dto.CompanyId = AuthInformation.companyId.Value;
                dto.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Invoice model = _invoiceApplicationService.SaveCreditNote(dto, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(dto));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditnoteapplication")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetProvision")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,getallinvoicesk,createcreditnote,getallcreditnotelus,createinvoice,getallinvoicelus,getinvoicedetail,getinvoicegstdetail,getinvoicenote,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes, CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,CreateCreditNoteByDebitNote")]
        //for this controller
        //invalidate DebitNote
        //[CacheInvalidate(Position = 11, Keys = "GetDebitNoteAllLUs", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetDebitNoteDetail", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetDebitNoteNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNoteDocumentVoid", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptByDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetDebitProvision", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNoteApplication(CreditNoteApplicationModel dto)
        {
            try
            {
                dto.CompanyId = AuthInformation.companyId.Value;
                CreditNoteApplication model = _invoiceApplicationService.SaveCreditNoteApplication(dto, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(dto));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getallcreditnoteapplications")]
        // [CommonHeaders(Position = 1)]    public List<CreditNoteApplication> GetAllCreditNoteApplications(long companyId, Guid invoiceId)
        //{
        //    return _creditNoteApplicationService.GetAllCreditNoteApplications(companyId, invoiceId);
        //}


        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditnoteapplication")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public CreditNoteApplicationModel CreateCreditNoteApplication(Guid creditNoteId, Guid cnApplicationId, bool isView, DateTime applicationDate)
        {
            return _invoiceApplicationService.CreateCreditNoteApplication(creditNoteId, cnApplicationId, AuthInformation.companyId.Value, isView, applicationDate);
        }

        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createcreditnotedocumentvoid")]
        // [CommonHeaders(Position = 1)]    public DocumentVoidModel CreateCreditNoteDocumentVoid(Guid id, long companyId)
        //{
        //    return _invoiceApplicationService.CreateCreditNoteDocumentVoid(id, companyId);
        //}

        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditnotedocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetProvision")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,createcreditnote,getallcreditnotelus,getallinvoicesk,createinvoice,getallinvoicelus,getinvoicedetail,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes,CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,GetAllParkedInvoiceK,getallparkedinvoicek,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNoteDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Invoice model = _invoiceApplicationService.SaveCreditNoteDocumentVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditnoteapplicationreset")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public DocumentResetModel CreateCreditNoteApplicationReset(Guid id, Guid creditNoteId)
        {
            return _invoiceApplicationService.CreateCreditNoteApplicationReset(id, creditNoteId, AuthInformation.companyId.Value);
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditnoteapplicationvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetProvision")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceiptByDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,createcreditnote,getallcreditnotelus,getallinvoicesk,createinvoice,getallinvoicelus,getinvoicedetail,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes,CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,GetAllParkedInvoiceK,getallparkedinvoicek,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNoteApplicationVoid(DocumentResetModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                CreditNoteApplication model = _invoiceApplicationService.SaveCreditNoteApplicationReset(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("GetAllCreditNotes")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public PageResult<Invoice> GetAllCreditNotes(ODataQueryOptions<Invoice> options, int pageSize, long companyId)
        {
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = pageSize
            };

            IQueryable results = options.ApplyTo(_invoiceApplicationService.GetAll(DocTypeConstants.CreditNote, companyId).AsQueryable(), settings);

            Uri uri = Request.GetNextPageLink();
            long? inlineCount = Request.GetInlineCount();

            PageResult<Invoice> response = new PageResult<Invoice>(
                results as IEnumerable<Invoice>,
                uri,
                inlineCount);

            return response;
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("CreateCreditNoteByDebitNote")]
        // [CommonHeaders(Position = 1)]    public CreditNoteModel CreateCreditNoteByDebitNote(Guid debitNoteId, long companyId)
        //{

        //    return _invoiceApplicationService.CreateCreditNoteByDebitNote(debitNoteId, companyId);
        //}

        [HttpGet]
        //[AllowAnonymous]
        [Route("CreateCreditNoteByDebitNote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNoteByDebitNote(Guid debitNoteId)
        {
            try
            {
                CreditNoteModel model = _invoiceApplicationService.CreateCreditNoteByDebitNote(debitNoteId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region DoubtfulDebt
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebt")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDoubtfulDebt(Guid id)
        {
            try
            {
                DoubtfulDebtModel model = _invoiceApplicationService.CreateDoubtfulDebt(AuthInformation.companyId.Value, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldoubtfuldebtlus")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public DoubtfulDebtLU GetAllDoubtfulDebtLUs(Guid doubtfullId)
        {
            return _invoiceApplicationService.GetAllDoubtfulDebtLUs(AuthInformation.userName, doubtfullId, AuthInformation.companyId.Value);
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savedoubtfuldebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]

        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,createcreditnote,getallcreditnotelus,getallinvoicesk,createinvoice,getallinvoicelus,getinvoicedetail,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes,CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,GetAllParkedInvoiceK,getallparkedinvoicek,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDoubtfulDebt(DoubtfulDebtModel dto)
        {
            //if (ModelState.IsValid)
            //{
            try
            {
                dto.CompanyId = AuthInformation.companyId.Value;
                dto.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Invoice model = _invoiceApplicationService.SaveDoubtfulDebt(dto, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            //}
            //return BadRequest("Model is invalid");
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savedoubtfuldebtallocation")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,createcreditnote,getallcreditnotelus,getallinvoicesk,createinvoice,getallinvoicelus,getinvoicedetail,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes,CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDoubtFulDebtAllocation(DoubtfulDebtAllocationModel dto)
        {
            try
            {
                dto.CompanyId = AuthInformation.companyId.Value;
                DoubtfulDebtAllocation model = _invoiceApplicationService.SaveDoubtFulDebtAllocation(dto, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return BadRequest(ex.InnerException.Message);
                else
                    return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebtallocation")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public DoubtfulDebtAllocationModel CreateDoubtFulDebtAllocation(Guid doubtfulDebtId, Guid ddAclocationId, bool isView, DateTime allocationDate)
        {
            return _invoiceApplicationService.CreateDoubtFulDebtAllocation(doubtfulDebtId, ddAclocationId, AuthInformation.companyId.Value, isView, allocationDate);
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebtreverse")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public DoubtfulDebtReverseModel CreateDoubtfulDebtReverse(Guid doubtfulDebtId)
        {
            return _invoiceApplicationService.CreateDoubtfulDebtReverse(doubtfulDebtId, AuthInformation.companyId.Value);
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savedoubtfuldebtreverse")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        //[CacheInvalidate(Position = 11, Keys = "getallinvoicesk,createinvoice,getallinvoicelus,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDoubtfulDebtReverse(DoubtfulDebtReverseModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Invoice model = _invoiceApplicationService.SaveDoubtfulDebtReverse(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebtdocumentvoid")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public DocumentVoidModel CreateDoubtfulDebtDocumentVoid(Guid id)
        {
            return _invoiceApplicationService.CreateDoubtfulDebtDocumentVoid(id, AuthInformation.companyId.Value);
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savedoubtfuldebtdocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]

        //[CacheInvalidate(Position = 11, Keys = "getallinvoicesk,createinvoice,getallinvoicelus,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDoubtfulDebtDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Invoice model = _invoiceApplicationService.SaveDoubtfulDebtDocumentVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createdoubtfuldebtallocationreset")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDoubtfulDebtAllocationReset(Guid id, Guid doubtfulDebtId)
        {
            try
            {
                DocumentResetModel model = _invoiceApplicationService.CreateDoubtfulDebtAllocationReset(id, doubtfulDebtId, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        //[AllowAnonymous]
        [Route("savedobtfuldebtallocationreset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]

        //[CacheInvalidate(Position = 11, Keys = "getallinvoicesk,createinvoice,getallinvoicelus,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,getallvoidinvoicek,getallpostedrecurringsK,CreateDoubtFulDebtByDebitNote")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveDobtfulDebtAllocationReset(DocumentResetModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                DoubtfulDebtAllocation model = _invoiceApplicationService.SaveDobtfulDebtAllocationReset(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldebitfulldebitK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllDebitfulldebitK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = await _invoiceApplicationService.GetAllDebitfulldebitK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("GetAllDoubtfulDebts")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public PageResult<Invoice> GetAllDoubtfulDebts(ODataQueryOptions<Invoice> options, int pageSize)
        {
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = pageSize
            };

            IQueryable results = options.ApplyTo(_invoiceApplicationService.GetAll(DocTypeConstants.DoubtFulDebitNote, AuthInformation.companyId.Value).AsQueryable(), settings);

            Uri uri = Request.GetNextPageLink();
            long? inlineCount = Request.GetInlineCount();

            PageResult<Invoice> response = new PageResult<Invoice>(
                results as IEnumerable<Invoice>,
                uri,
                inlineCount);

            return response;
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("CreateDoubtFulDebtByDebitNote")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateDoubtFulDebtByDebitNote(Guid debitNoteId)
        {
            try
            {
                DoubtfulDebtModel model = _invoiceApplicationService.CreateDoubtFulDebtByDebitNote(debitNoteId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Workflowinvoicevoid
        [HttpPost]
        [AllowAnonymous]
        [Route("saveworkflowinvoicevoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        //[CacheInvalidate(Position = 11, Keys = "getallcreditnoteK,createcreditnote,getallcreditnotelus,getallinvoicesk,createinvoice,getallinvoicelus,getinvoicedetail,createreceiptbyinvoice,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createcreditnoteapplication,GetAllCreditNotes,CreateCreditNoteByDebitNote,createdoubtfuldebt,getalldoubtfuldebtlus,createdoubtfuldebtallocation,createdoubtfuldebtreverse,getalldebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote,GetAllParkedInvoiceK,getallparkedinvoicek,getallvoidinvoicek,getallpostedrecurringsK")]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveWorkflowInvoiceVoid(InvoiceVM tObject)
        {
            try
            {
                //tObject.CompanyId = AuthInformation.companyId.Value;
                return Ok(_invoiceApplicationService.SaveWorkflowInvoiceVoid(tObject));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Recurring_Invoice
        [HttpGet]
        [Route("getrecurringinvoicelist")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetRecurringInvoicelList(Guid id, DateTime docDate, DateTime? endDate, string docNo, int frequencyValue)
        {
            try
            {
                return Ok(_invoiceApplicationService.GetRecurrInvoiceDocNo(id, AuthInformation.companyId.Value, docDate, endDate, docNo, frequencyValue));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallparkedinvoicek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllParkedInvoiceK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = _invoiceApplicationService.GetAllParkedInvoicesK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallrecurringinvoicek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllRecurringInvoiceK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = await _invoiceApplicationService.GetAllRecurringInvoicesK(AuthInformation.userName, AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallvoidinvoicek")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllVoidInvoiceK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = _invoiceApplicationService.GetAllVoidInvoicesK(AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createrecurringinvoice")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateRecurringInvoice(Guid id)
        {
            try
            {
                RecurringModel model = _invoiceApplicationService.CreateRecurringInvoice(AuthInformation.companyId.Value, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("saverecurringinvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRecurringInvoice")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllVoidInvoiceK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllRecurringInvoiceK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllParkedInvoiceK")]
        ////[CacheInvalidate(Position = 11, Keys = "getallpostedrecurringsK")]
        //////[CacheInvalidate(Position =11,Keys = "CreateCreditNoteApplication")]
        //////[CacheInvalidate(Position =11,Keys = "CreateReceiptModelEdit", "ReceiptController")]
        //////[CacheInvalidate(Position =11,Keys = "GetCustomerLU", "MasterModuleController")]

        //[CacheInvalidate(Position = 11, Keys = "getallinvoicelus,getinvoicedetail,CreateInvoice,GetAllInvoicesK,CreateRecurringInvoice,GetAllVoidInvoiceK,GetAllRecurringInvoiceK,GetAllParkedInvoiceK,getallpostedrecurringsK,CreateCreditNoteApplication")]

        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveRecurringInVoice(InvoiceModel Invoice)
        {
            try
            {
                // _eventStoreOperations.SaveEventToStream(inVoice, "BeanMaster-SaveInVoice");
                //string _name = HttpContext.Current.User.Identity.Name;
                Invoice.CompanyId = AuthInformation.companyId.Value;
                Invoice.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                Invoice Invoicemodel = _invoiceApplicationService.SaveRecurringInvoice(Invoice, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(Invoicemodel);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(Invoice));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallpostedrecurringsK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllPostedRecurring(HttpRequestMessage requestMessage, Guid id)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = _invoiceApplicationService.GetAllRecurringPostedInvoicesK(AuthInformation.companyId.Value, id).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getdeletedaudittrail")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetDeletedAuditTrail(Guid invoiceId)
        {
            try
            {
                List<InvoiceStateModel> model = _invoiceApplicationService.GetDeletedAuditTrail(invoiceId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion Recurring_Invoice

        #region GST_Regstration_De_Registration
        [HttpGet]
        [Route("getgstderegistration")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetGstDeregistration()
        {
            try
            {
                var serviceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_invoiceApplicationService.GetAllRecordBasedOnServiceCompanyId(serviceCompanyId, AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        #endregion GST_Regstration_De_Registration

        #region Common_Call_For_All_InvoiceDocument
        [HttpGet]
        [Route("getalldocument")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllDocument(Guid documentId, string docType)
        {
            try
            {
                return Ok(_invoiceApplicationService.GetAllDocumentByInvoiceId(AuthInformation.companyId.Value, documentId, docType, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region   Bean - WF Sync
        [HttpPost]
        [AllowAnonymous]
        [Route("saveinvoiceSync")]
        //[CacheInvalidate(Position = 11, Keys = "getallinvoicelus,getinvoicedetail,createinvoice,getallinvoicesk,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createreceiptbyinvoice,createcreditnoteapplication,getallpostedrecurringsK")]
        //[CacheInvalidate(Position = 12, Keys = "CreateReceiptModelEdit", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 13, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        //////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        //////[CacheInvalidate(Position = 11, Keys = "getallpostedrecurringsK")]
        // [RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInVoiceSync(InvoiceModel Invoice)
        {
            try
            {
                return Ok(_invoiceApplicationService.SaveInvoice(Invoice, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                _invoiceApplicationService.SmartCursorSyncing(Invoice.CompanyId, CursorConstants.Workflow, DocTypeConstants.Invoice, Invoice.DocumentId, null, InvoiceConstants.Failed, ex.Message, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return BadRequest(ex.Message);

            }
        }



        #region Workflowinvoicevoid


        #region wf_bean_sync_call
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("saveinvoicesync")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs,GetInvoiceDetail,CreateInvoice,GetAllInvoicesK,CreateCreditNoteByInvoice,CreateDoubtFulDebtByInvoice,CreateReceiptByInvoice,CreateCreditNoteApplication,getallpostedrecurringsK")]
        ////[CacheInvalidate(Position = 12, Keys = "CreateReceiptModelEdit", Controller = "ReceiptController")]
        ////[CacheInvalidate(Position = 13, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        ////////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        ////////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        ////////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        ////////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        ////////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        ////////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        ////////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        ////////[CacheInvalidate(Position = 11, Keys = "getallpostedrecurringsK")]
        ////[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SaveInVoiceSync(InvoiceModel Invoice)
        //{
        //    try
        //    {
        //        // _eventStoreOperations.SaveEventToStream(inVoice, "BeanMaster-SaveInVoice");
        //        //string _name = HttpContext.Current.User.Identity.Name;
        //        Invoice.CompanyId = AuthInformation.companyId.Value;
        //        Invoice.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
        //        Invoice Invoicemodel = _invoiceApplicationService.SaveInvoice(Invoice);
        //        return Ok(Invoicemodel);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        #endregion
        //[HttpPost]
        //[AllowAnonymous]
        //[Route("saveworkflowinvoicevoidSync")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNote")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs")]
        //////[CacheInvalidate(Position = 11, Keys = "GetInvoiceDetail")]
        //////[CacheInvalidate(Position = 11, Keys = "GetInvoiceGSTDetail")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateReceiptByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByInvoice")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateInvoiceDocumentVoid")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplication")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteApplicationReset")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNotes")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteByDebitNote")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtAllocation")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtReverse")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtDocumentVoid")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebtAllocationReset")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebts")]
        //////[CacheInvalidate(Position = 11, Keys = "CreateDoubtFulDebtByDebitNote")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK,CreateCreditNote,GetAllCreditNoteLUs,GetAllInvoicesK,CreateInvoice,GetAllInvoiceLUs,GetInvoiceDetail,GetInvoiceGSTDetail,CreateReceiptByInvoice,CreateCreditNoteByInvoice,CreateDoubtFulDebtByInvoice,CreateInvoiceDocumentVoid,CreateCreditNoteApplication,CreateCreditNoteApplicationReset,GetAllCreditNotes,CreateCreditNoteByDebitNote,CreateDoubtfulDebt,GetAllDoubtfulDebtLUs,CreateDoubtFulDebtAllocation,CreateDoubtfulDebtReverse,CreateDoubtfulDebtDocumentVoid,CreateDoubtfulDebtAllocationReset,GetAllDebitfulldebitK,GetAllDoubtfulDebts,CreateDoubtFulDebtByDebitNote")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SaveWorkflowInvoiceVoidSync(InvoiceVM tObject)
        //{
        //    try
        //    {
        //        tObject.CompanyId = AuthInformation.companyId.Value;
        //        return Ok(_invoiceApplicationService.SaveWorkflowInvoiceVoid(tObject));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        #endregion


        [HttpPost]
        //[AllowAnonymous]
        [Route("getmongofiles")]
        public IHttpActionResult GetMongoFiles(WorkFlowMongoFile mongoFile)
        {
            try
            {
                return Ok(_invoiceApplicationService.SaveAttachments(mongoFile));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("wfinvoicemigration")]
        public IHttpActionResult WFInvoiceMigration(WorkFlowMongoFile mongoFile)
        {
            try
            {
                return Ok(_invoiceApplicationService.SaveAttachments(mongoFile));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion  Bean - WF Sync



    }

}
