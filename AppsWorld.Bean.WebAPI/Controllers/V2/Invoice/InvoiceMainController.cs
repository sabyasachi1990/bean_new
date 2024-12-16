using AppsWorld.Bean.WebAPI.Utils;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Models;
using AppsWorld.InvoiceModule.Application.V2;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.Models;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;

namespace AppsWorld.Bean.WebAPI.Controllers.V2
{
    [RoutePrefix("api/v2/invoice")]
    public class InvoiceMainController : BaseController
    {
        private readonly InvoiceApplicationService _invoiceMasterApplicationService;
        public InvoiceMainController(InvoiceApplicationService invoiceMasterApplicationService)
        {
            this._invoiceMasterApplicationService = invoiceMasterApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createinvoice")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateInvoice(Guid id)
        {
            try
            {
                return Ok( _invoiceMasterApplicationService.CreateInvoice(AuthInformation.companyId.Value, id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallinvoicenewlus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllInvoiceNewLUs(Guid invoiceId)
        {
            try
            {
                return Ok(_invoiceMasterApplicationService.GetAllInvoiceNewLUs(AuthInformation.userName, invoiceId, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("saveinvoice")]
        //[CacheInvalidate(Position = 11, Keys = "getallinvoicelus,getinvoicedetail,createinvoice,getallinvoicesk,createcreditnotebyinvoice,createdoubtfuldebtbyinvoice,createreceiptbyinvoice,createcreditnoteapplication,getallpostedrecurringsK")]
        //[CacheInvalidate(Position = 12, Keys = "CreateReceiptModelEdit", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 13, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInVoice(InvoiceModel Invoice)
        {
            try
            {
                Invoice.CompanyId = AuthInformation.companyId.Value;
                Invoice.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_invoiceMasterApplicationService.SaveInvoice(Invoice, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(Invoice));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceModule.Infra.Resources.InvoiceLoggingValidation.InvoiceController, ex, ex.Message, AdditionalInfo);

                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getinvoicedetail")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetInvoiceDetail(Guid invoiceId, Guid invoiceDetalId)
        {
            try
            {
                return Ok(_invoiceMasterApplicationService.GetInvoiceDetail(invoiceId, invoiceDetalId));
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

        //[CacheInvalidate(Position = 12, Keys = "CreateReceipt", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 13, Keys = "GetCustomerLU", Controller = "MasterModuleController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInvoiceDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                return Ok(_invoiceMasterApplicationService.SaveInvoiceDocumentVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
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
                return Ok(_invoiceMasterApplicationService.CreateCreditNoteByInvoice(invoiceId, AuthInformation.companyId.Value));
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
                return Ok(_invoiceMasterApplicationService.CreateDoubtFulDebtByInvoice(invoiceId, AuthInformation.companyId.Value));
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
                return Ok( _invoiceMasterApplicationService.CreateReceiptByInvoice(invoiceId, AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
