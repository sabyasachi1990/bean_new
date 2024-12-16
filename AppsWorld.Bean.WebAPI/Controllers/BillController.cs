using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.BillModule.Application;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.Models;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Linq;
using System;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Infra;
using WebApi.RedisCache.V2;
using System.Collections.Specialized;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using Ziraff.FrameWork.Logging;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/bill")]
    public class BillController : BaseController
    {
        private readonly BillApplicationService _billApplicationService;

        #region Bill
        public BillController(BillApplicationService BillApplicationService)
        {
            this._billApplicationService = BillApplicationService;
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("bill")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllBills()
        {
            try
            {
                List<Bill> model = _billApplicationService.GetAllBills(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("bills")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllReceipts(ODataQueryOptions<BillModel> options, int pageSize)
        {
            try
            {
                ODataQuerySettings settings = new ODataQuerySettings()
                {
                    PageSize = pageSize
                };

                IQueryable results = options.ApplyTo(_billApplicationService.GetAllBillModel(AuthInformation.companyId.Value).AsQueryable(), settings);

                Uri uri = Request.GetNextPageLink();
                long? inlineCount = Request.GetInlineCount();

                PageResult<BillModel> response = new PageResult<BillModel>(
                    results as IEnumerable<BillModel>,
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
        //[AllowAnonymous]
        [Route("createbill")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateBill(Guid id, string cursorType, string docType, bool isCopy)
        {
            try
            {
                BillModel model = _billApplicationService.CreateBill(AuthInformation.userName, id, AuthInformation.companyId.Value, cursorType, docType, isCopy);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("billalllu")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllBillLU(Guid billId, string docType)
        {
            try
            {
                BillLU model = _billApplicationService.GetAllBillLUs(AuthInformation.userName, billId, AuthInformation.companyId.Value, docType);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createbilldetail")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllBillDetailModel(Guid billId, Guid billDetailId)
        {
            try
            {
                BillDetailModel model = _billApplicationService.GetAllBillDetailModel(billId, billDetailId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createbillgstdetail")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetGstBillDetailById(Guid Id, Guid billId)
        //{
        //    try
        //    {
        //        BillGstDetailModel model = _billApplicationService.GetGstBillDetailById(Id, billId);
        //        return Ok(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebill")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBill")]
        ////[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBills")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceipts")]
        ////[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillssK")]
        ////[CacheInvalidate(Position = 11, Keys = "PaymentByBill")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreditMemoByBill")]

        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGSTStatus", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GSTSettings", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetPaymentDetails", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "createbill,createbillgstdetail,billalllu,bill,bills,isgstallowed,createbilldetail,getallbillsk,GetAllBillssK,createpaymentbybill,getallpayrollbillsk,createcreditmemobybill,getalldocbybill")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBill(BillModel billObject)
        {
            try
            {
                billObject.CompanyId = AuthInformation.companyId.Value;
                billObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_billApplicationService.SaveBill(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(billObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(BillModule.Infra.Resources.BillConstants.BillController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createbilldocumentvoid")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCreditNoteDocumentVoid(Guid id)
        {
            try
            {
                DocumentVoidModel model = _billApplicationService.CreateBillDocumentVoid(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebilldocumentvoid")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBill")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateCreditNoteDocumentVoid")]
        ////[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBills")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceipts")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillssK")]

        //[CacheInvalidate(Position = 11, Keys = "createbill,createbilldocumentvoid,createbillgstdetail,bill,bills,createbilldetail,getallbillsk,GetAllBillssK,getalldocbybill")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCreditNoteDocumentVoid(DocumentVoidModel TObject)
        {
            try
            {
                TObject.CompanyId = AuthInformation.companyId.Value;
                Bill model = _billApplicationService.SaveBillNoteDocumentVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("vendorlu")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult VendorLu(Guid entityId)
        {
            try
            {
                var model = _billApplicationService.VendorLu(entityId, AuthInformation.companyId.Value);
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
        public IHttpActionResult IsGSTAllowed(DateTime docDate)
        {
            try
            {
                bool model = _billApplicationService.IsGSTAllowed(AuthInformation.companyId.Value, docDate);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createcreditmemobybill")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreditMemoByBill(Guid Id)
        {
            try
            {
                var model = _billApplicationService.CreateCreditMemoByBill(Id, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createpaymentbybill")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult PaymentByBill(Guid Id, string docType)
        {
            try
            {
                var model = _billApplicationService.CreatePaymentByBill(Id, AuthInformation.companyId.Value, docType, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getalldocbybill")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllDocByBill(Guid Id, string docType)
        {
            try
            {
                return Ok(_billApplicationService.GetAllDocumentByBillId(AuthInformation.companyId.Value, Id, docType, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, AuthInformation.userName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region KendoGrid
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallbillsk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        // [CommonHeaders(Position = 1)]    public DataSourceResult GetAllBillsK(HttpRequestMessage requestMessage, long companyId)
        //{
        //	DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
        //		// The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
        //	   requestMessage.RequestUri.ParseQueryString().GetKey(0)
        //	);
        //	return _billApplicationService.GetAllBillsK(companyId).OrderBy(c => c.CompanyId).Select(c => new
        //	{
        //		c.DocNo,
        //		c.PostingDate,
        //		c.DueDate,
        //		c.BaseCurrency,
        //		c.Entity.Name,
        //		c.GrandTotal,
        //		c.DocumentState,
        //		c.SystemReferenceNumber,
        //		c.DocCurrency,
        //		c.VendorType

        //	}).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        //}

        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllBillsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
              // The request is in the format GET api/products?{take:10,skip:0} and ParseQueryString treats it as a key without value
              requestMessage.RequestUri.ParseQueryString().GetKey(0)
           );
                var model = _billApplicationService.GetAllBillModelk(AuthInformation.companyId.Value).Select(c => new
                {
                    c.DocNo,
                    c.PostingDate,
                    c.DueDate,
                    c.BaseCurrency,
                    c.EntityName,
                    c.GrandTotal,
                    c.DocumentState,
                    c.SystemReferenceNumber,
                    c.DocCurrency
                    // c.VendorType

                }).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("GetAllBillssK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllBillssK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
             requestMessage.RequestUri.ParseQueryString().GetKey(0)
          );

                var model = _billApplicationService.GetAllBillssK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion KendoGrid

        #endregion Bill

        #region Payroll Bill
        [HttpGet]
        [Route("getallpayrollbillsk")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllPayrollBillsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
             requestMessage.RequestUri.ParseQueryString().GetKey(0)
          );

                var model = _billApplicationService.GetAllPayrollBillsK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savepayrollbilldocvoidsync")]
        //[CacheInvalidate(Position = 11, Keys = "getallpayrollbillsk")]
        //[CacheInvalidate(Position = 11, Keys = "createbill")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById")]
        //[CacheInvalidate(Position = 11, Keys = "bill")]
        //[CacheInvalidate(Position = 11, Keys = "createbilldetail")]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult savepayrollbilldocvoidsync(DocumentVoidModel TObject)
        {
            try
            {
                //TObject.CompanyId = AuthInformation.companyId.Value;
                bool? model = _billApplicationService.SavePayrollBillDocVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("savepayrollbilldocvoidsyncmanual")]
        //[CacheInvalidate(Position = 11, Keys = "getallpayrollbillsk")]
        //[CacheInvalidate(Position = 11, Keys = "createbill")]
        //[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById")]
        //[CacheInvalidate(Position = 11, Keys = "bill")]
        //[CacheInvalidate(Position = 11, Keys = "createbilldetail")]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult savepayrollbilldocvoidsyncmanual(DocumentVoidModel TObject)
        {
            try
            {
                //TObject.CompanyId = AuthInformation.companyId.Value;
                bool? model = _billApplicationService.SavePayrollBillDocVoid(TObject);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion Payrolll Bill

        #region HR_Bean_Sync
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebillsync")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBill")]
        ////[CacheInvalidate(Position = 11, Keys = "GetGstBillDetailById")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBills")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllReceipts")]
        ////[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillDetailModel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillssK")]
        ////[CacheInvalidate(Position = 11, Keys = "PaymentByBill")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreditMemoByBill")]

        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGSTStatus", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GSTSettings", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetPaymentDetails", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "createbill,GetGstBillDetailById,billalllu,bill,bills,isgstallowed,createbilldetail,getallbillsk,GetAllBillssK,createpaymentbybill,getallpayrollbillsk,createcreditmemobybill,getentity")]
        //[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBillSync(BillModel billObject)
        {
            try
            {
                //billObject.CompanyId = AuthInformation.companyId.Value;
                //billObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_billApplicationService.SaveBill(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getclaimsbill")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult GetClaimsBill(Guid Id, long companyId)
        {
            try
            {
                return Ok(_billApplicationService.GetBillForClaims(Id, companyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Bill_migration_to_Mongo
        [HttpPost]
        //[AllowAnonymous]
        [Route("billmigration")]
        // [CommonHeaders(Position = 1)]
        public IHttpActionResult BillMigration(long companyId)
        {
            try
            {
                return Ok(_billApplicationService.BillMigrationCall(companyId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion Bill_migration_to_Mongo

        #region HR_Payroll_and_Claims_Syncing(Batch Job)
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebatchbillsync")]


        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGSTStatus", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GSTSettings", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetPaymentDetails", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "createbill,GetGstBillDetailById,billalllu,bill,bills,isgstallowed,createbilldetail,getallbillsk,GetAllBillssK,createpaymentbybill,getallpayrollbillsk,createcreditmemobybill,getentity")]
        //[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBatchBillSync(List<BillModel> billObject)
        {
            try
            {
                //billObject.CompanyId = AuthInformation.companyId.Value;
                //billObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                //Bill model = _billApplicationService.SaveBill(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(_billApplicationService.SavePayrollandClaims(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("savebatchbillsyncanonymous")]


        //[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GetGSTStatus", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "GSTSettings", Controller = "MasterModuleController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemoApplication", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetPaymentDetails", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "createbill,GetGstBillDetailById,billalllu,bill,bills,isgstallowed,createbilldetail,getallbillsk,GetAllBillssK,createpaymentbybill,getallpayrollbillsk,createcreditmemobybill,getentity")]
        //[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBatchBillSyncAnonymous(List<BillModel> billObject)
        {
            try
            {
                //billObject.CompanyId = AuthInformation.companyId.Value;
                //billObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                //Bill model = _billApplicationService.SaveBill(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(_billApplicationService.SavePayrollandClaims(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Peppol

        [HttpPost]
        [AllowAnonymous]
        [Route("InBoundInvoicetosavebill")]
        public IHttpActionResult AllInBoundInvoices(InBoundFilesModel model)
        {
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Object", JsonConvert.SerializeObject(model));
                LoggingHelper.LogMessage("BillController", "SaveInvoice ModelObject", AdditionalInfo);
                return Ok(_billApplicationService.InBoundAllInvoice(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("savebillduplicate")]
        public async Task<IHttpActionResult> SaveBillDuplicate(BillModel billObject)
        {
            try
            {
                // billObject.CompanyId = AuthInformation.companyId.Value;
                // billObject.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(await Task.Run(() => _billApplicationService.SaveBill(billObject, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)));

            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(billObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(BillModule.Infra.Resources.BillConstants.BillController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }


        #endregion




    }
}

