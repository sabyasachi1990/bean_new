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
    public class CNApplicationController : BaseController
    {
        private readonly CNAApplicationService _CNAApplicationService;
        public CNApplicationController(CNAApplicationService CNAApplicationService)
        {
            this._CNAApplicationService = CNAApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditnoteapplication")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNoteApplication(Guid creditNoteId, Guid cnApplicationId, bool isView, DateTime applicationDate, bool isICActive)
        {
            try
            {
                return Ok(_CNAApplicationService.CreateCreditNoteApplication(creditNoteId, cnApplicationId, AuthInformation.companyId.Value, AuthInformation.userName,isView, applicationDate, isICActive, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("creditnoteapplicationlu")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreditNoteApplicationLU(Guid CreditnoteapplicationId)
        {
            try
            {
                return Ok(_CNAApplicationService.CreditNoteApplicationLU(AuthInformation.companyId.Value, CreditnoteapplicationId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditnoteapplicationvoid")]
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
                return Ok(_CNAApplicationService.SaveCreditNoteApplicationVoid(TObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecreditnoteapplication")]
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
                CreditNoteApplication model = _CNAApplicationService.SaveCreditNoteApplication(dto, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(dto));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(InvoiceModule.Infra.Resources.InvoiceLoggingValidation.CNApplicationController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
    }
}
