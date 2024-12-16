using System.Collections.Generic;
using System.Web.Http;
using AppsWorld.BankReconciliationModule.Application;
using System;
using AppsWorld.BankReconciliationModule.Models;
using System.Net.Http;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System.Linq;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix(BankReconciliationConst.api_bank)]
    [CommonHeaders(Position = 1)]
    public class BankReconciliationController : BaseController
    {
        private readonly BankReconciliationApplicationService _bankReconciliationApplicationService;
        public BankReconciliationController(BankReconciliationApplicationService bankReconciliationApplicationService)
        {
            this._bankReconciliationApplicationService = bankReconciliationApplicationService;
        }

        #region Create

        [HttpGet]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.getclearingtransaction)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public ClearingModel GetClearingTransaction(long chartid, DateTime? fromDate, DateTime toDate)
        {
            var subcompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
            return _bankReconciliationApplicationService.GetClearingTransaction(AuthInformation.companyId.Value, subcompanyId, chartid, fromDate, toDate);
        }

        [HttpGet]
        [Route(BankReconciliationConst.create_bankreconciliation_detail_data)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateBankReconciliationDetails(int page, int pageSize, Guid id, long chartid, DateTime reconcileDate, bool IsClearedTab)
        {
            try
            {
                var subcompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_bankReconciliationApplicationService.CreateBankReconciliationDetails(id, AuthInformation.companyId.Value, subcompanyId, chartid, reconcileDate, IsClearedTab, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, page, pageSize));
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.Create_Bank_Recon_master_data)]
        [RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> CreateBankreconciliationMasterData(Guid id, long chartid, DateTime reconcileDate)
        {
            try
            {
                var subcompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(await _bankReconciliationApplicationService.CreateBankReconciliationMaster(id, AuthInformation.companyId.Value, subcompanyId, chartid, reconcileDate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.getclearingrec)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetClearingRec(Guid bankRecId, DateTime? bankRecDate, DateTime? lastRecDate, long coaId)
        {
            //var subcompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
            //return _bankReconciliationApplicationService.GetClearingRec(AuthInformation.companyId.Value, subcompanyId, chartid, fromDate, toDate);
            try
            {
                var subcompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                return Ok(_bankReconciliationApplicationService.GetListOfClearingRec(AuthInformation.companyId.Value, subcompanyId, bankRecId, bankRecDate, lastRecDate, coaId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        #endregion Create

        #region KendoGrid

        [HttpGet]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.getallbankreconciliationsK)]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public DataSourceResult GetAllBankReconciliationsK(HttpRequestMessage requestMessage)
        {
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
            return _bankReconciliationApplicationService.GetAllBankReconciliationsK(AuthInformation.userName, AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("getclearingtransactionk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public DataSourceResult GetClearingTransactionK(HttpRequestMessage requestMessage, long chartid, DateTime? fromDate, DateTime? toDate)
        {
            var subcompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
            DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));

            return _bankReconciliationApplicationService.GetClearingTransactionK(AuthInformation.userName, AuthInformation.companyId.Value, subcompanyId, chartid, fromDate, toDate).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }

        #endregion KendoGrid

        #region LookUp

        [HttpGet]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.bankreconciliationllu)]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult BankReconciliationLu(Guid id)
        {
            try
            {
                return Ok(_bankReconciliationApplicationService.BankReconciliationLu(id, AuthInformation.companyId.Value, AuthInformation.userName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion LookUp

        #region Save

        [HttpPost]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.savebankreconciliation)]
        ////[CacheInvalidate(Position = 11, Keys = "BankReconciliationLu")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBankReconciliationsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingRec")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingTransaction")]

        //[CacheInvalidate(Position = 11, Keys = "CreateInvoice", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCashSales", Controller = "CashSalesController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNote", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceipt", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "CreatePayment", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateWithdrawal", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankTransfer", Controller = "BankTransferController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateJournalL", Controller = "JournalVoucherController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearing", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesK", Controller = "CashSalesController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceiptsK", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllpaymentsK", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollPaymentsK", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBankWithdrawal", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDeposit", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashPayments", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "banktransferk", Controller = "BankTransferController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK", Controller = "JournalVoucherController")]

        //[CacheInvalidate(Position = 11, Keys = "bankreconciliationllu,getallbankreconciliationsK,createbankreconciliation,getclearingrec,getclearingtransaction")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Savebankreconciliation(BankReconciliationModel BankReconciliation)
        {

            try
            {
                BankReconciliation.CompanyId = AuthInformation.companyId.Value;
                BankReconciliation.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                BankReconciliationModel brm = _bankReconciliationApplicationService.SaveClearingDate(BankReconciliation, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(brm);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(BankReconciliation));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError(BankReconciliationModule.Infra.BRConstants.BankReconciliationController, ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.Voidbankreconciliation)]
        ////[CacheInvalidate(Position = 11, Keys = "BankReconciliationLu")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBankReconciliationsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation")]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingTransaction")]

        ////[CacheInvalidate(Position = 11, Keys = BankReconciliationLu,GetAllBankReconciliationsK,CreateBankreconciliation,GetClearingTransaction")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Voidbankreconciliation(BankReconciliationModel VBankReconciliation)
        {
            try
            {
                VBankReconciliation.CompanyId = AuthInformation.companyId.Value;
                VBankReconciliation.ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                //VBankReconciliationConst.ModifiedBy = AuthInformation.userName;
                BankReconciliationModel vbrm = _bankReconciliationApplicationService.Voidbankreconciliation(VBankReconciliation, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(vbrm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.SaveClearings)]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingTransactionK")]
        ////[CacheInvalidate(Position = 11, Keys = "BankReconciliationLu")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBankReconciliationsK")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation")]
        //[CacheInvalidate(Position = 11, Keys = "CreateInvoice", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCashSales", Controller = "CashSalesController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNote", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceipt", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "CreatePayment", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateWithdrawal", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankTransfer", Controller = "BankTransferController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateJournalL", Controller = "JournalVoucherController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearing", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitNotesK", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesK", Controller = "CashSalesController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceiptsK", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllpaymentsK", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollPaymentsK", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBankWithdrawal", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDeposit", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashPayments", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "banktransferk", Controller = "BankTransferController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK", Controller = "JournalVoucherController")]
        //[CacheInvalidate(Position = 11, Keys = "bankreconciliationllu,getallbankreconciliationsK,createbankreconciliation,getclearingtransaction")]
        [RolePermission(Position = 3)]

        [CommonHeaders(Position = 1)]
        public void SaveClearings(List<BankReconciliationDetailModel> model)
        {
            //model = new List<BankReconciliationDetailModel>() { new BankReconciliationDetailModel() { CompanyId = AuthInformation.companyId.Value } };
            _bankReconciliationApplicationService.SaveClearings(model, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route(BankReconciliationConst.Savebankreconcile)]
        ////[CacheInvalidate(Position = 11, Keys = "GetClearingTransactionK")]
        ////[CacheInvalidate(Position = 11, Keys = "BankReconciliationLu")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBankReconciliationsK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateBankreconciliation")]
        //[CacheInvalidate(Position = 11, Keys = "bankreconciliationllu,getallbankreconciliationsK,createbankreconciliation,getclearingtransaction")]
        [CommonHeaders(Position = 1)]
        public void Savebankreconcile(List<BankReconciliationDetailModel> BankReconciliation)
        {
            //BankReconciliation = new List<BankReconciliationDetailModel>() { new BankReconciliationDetailModel() { CompanyId = AuthInformation.companyId.Value } };
            _bankReconciliationApplicationService.SaveReconcile(BankReconciliation);
        }

        #endregion Save[[

        #region static parameters
        [CommonHeaders(Position = 1)]
        public static class BankReconciliationConst
        {
            public const string api_bank = "api/bank";
            public const string getclearingtransaction = "getclearingtransaction";
            public const string create_bankreconciliation_detail_data = "createbankreconciliation";
            public const string getclearingrec = "getclearingrec";
            public const string getallbankreconciliationsK = "getallbankreconciliationsK";
            public const string bankreconciliationllu = "bankreconciliationllu";
            public const string savebankreconciliation = "savebankreconciliation";
            public const string Voidbankreconciliation = "Voidbankreconciliation";
            public const string SaveClearings = "SaveClearings";
            public const string Savebankreconcile = "Savebankreconcile";
            public const string Create_Bank_Recon_master_data = "createbankreconmasterdata";

        }
        #endregion static parameters

        #region pagenation
        // [HttpGet]
        // //[AllowAnonymous]
        // [Route("getclearingtransaction")]
        //// [RolePermissionFilter(ScreenName = Constant., PermissionName = Constant.ViewandAddandEdit, ModuleName = Constant.BeanCursor)]
        // // //[Cache(Position =10,ClientTimeSpan = 0, ServerTimeSpan = 6000,IsCompanyIdRequired =true)]
        // [CommonHeaders(Position = 1)]    [CommonHeaders(Position = 1)]    public PageResult<BankReconciliationDetailModel> GetClearingTransactionK(ODataQueryOptions<BankReconciliationDetailModel> options, int pageSize, long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime? toDate)
        // {
        //     ODataQuerySettings settings = new ODataQuerySettings()
        //     {
        //         PageSize = pageSize
        //     };

        //     IQueryable results = options.ApplyTo(_bankReconciliationApplicationService.GetClearingTransactionK( companyId,  subcompanyId,  chartid,  fromDate,  toDate).AsQueryable(), settings);

        //     Uri uri = Request.GetNextPageLink();
        //     long? inlineCount = Request.GetInlineCount();

        //     PageResult<BankReconciliationDetailModel> response = new PageResult<BankReconciliationDetailModel>(
        //         results as IEnumerable<BankReconciliationDetailModel>,
        //         uri,
        //         inlineCount);

        //     return response;

        // }






        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("createbankreconciliationvoid")]
        //[CommonHeaders(Position = 1)]    [CommonHeaders(Position = 1)]    public DocumentVoidModel CreateBankReconciliationVoid(Guid id, long companyId)
        //{
        //    return _bankReconciliationApplicationService.CreateBankReconciliationVoid(id, companyId);
        //}

        //[HttpPost]
        ////[AllowAnonymous]
        //[Route("savebankreconciliationvoid")]
        //[CommonHeaders(Position = 1)]    [CommonHeaders(Position = 1)]    public BankReconciliation SaveBankReconciliationVoid(DocumentVoidModel TObject)
        //{
        //    return _bankReconciliationApplicationService.SaveBankReconciliationVoid(TObject);
        //}


        //#endregion
        #endregion pagenation

    }
}