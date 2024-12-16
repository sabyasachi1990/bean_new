using AppsWorld.MasterModule.Application;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.Entities;
using Kendo.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using WebApi.RedisCache.V2;
using Ziraff.FrameWork.Controller;
using AppsWorld.Bean.WebAPI.Utils;
using System.Threading.Tasks;

namespace AppsWorld.Bean.WebAPI.Controllers
{
    [RoutePrefix("api/mastermodule")]
    public class MasterModuleController : BaseController
    {
        MasterModuleApplicationService _masterModuleApplicationService;

        public MasterModuleController(MasterModuleApplicationService masterModuleApplicationService)
        {
            this._masterModuleApplicationService = masterModuleApplicationService;
        }

        #region BeanEntity

        #region KendoGrid_GetAllBeanEntitysK
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallbeanentitysK")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllBeanEntitysK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var dr = _masterModuleApplicationService.GetAllBeanEntitysK(AuthInformation.companyId.Value).OrderByDescending(a => a.CreatedDate);
                var result= dr.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Create
        [HttpGet]
        //[AllowAnonymous]
        [Route("createentity")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> CreateEntity(Guid id)
        {
            try
            {
                return Ok(await _masterModuleApplicationService.CreateEntity(AuthInformation.companyId.Value, id, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("quickcreateentity")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult QuickCreateEntity(bool isCustomer, DateTime? docDate)
        {
            try
            {
                QuickBeanEntityModel quickBeanEntityModel = _masterModuleApplicationService.QuickCreateEntity(AuthInformation.companyId.Value, isCustomer, docDate);
                return Ok(quickBeanEntityModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getcustomerlu")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetCustomerLU(string invoiceId, string docType)
        {
            try
            {
                //List<LookUpGuid<string>> lookupguid = await _masterModuleApplicationService.GetCustomers(invoiceId, AuthInformation.companyId.Value, docType);
                IQueryable<LookUpGuid<string>> lookupguid = _masterModuleApplicationService.GetCustomersNew(invoiceId, AuthInformation.companyId.Value, docType);
                return Ok(lookupguid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("multicurrencyinformation")]
        //[RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult MultiCurrencyInformation(string DocumentCurrency, DateTime Documentdate, bool IsBase)
        //{
        //    try
        //    {
        //        Forex forex = _masterModuleApplicationService.GetMultiCurrencyInformation(DocumentCurrency, Documentdate, IsBase, AuthInformation.companyId.Value);
        //        return Ok(forex);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet]
        //[AllowAnonymous]
        [Route("getexrateinformation")]
        [RolePermission(Position = 3)]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> ExchangeRateInformations(string DocumentCurrency, DateTime Documentdate, bool IsBase, bool IsGst)
        {
            try
            {
                BeanForex forex =await _masterModuleApplicationService.GetExRateInformation(DocumentCurrency, Documentdate, IsBase, AuthInformation.companyId.Value, IsGst);
                return Ok(forex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getvendorlu")]
        [RolePermission(Position = 3)]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetVendorLU(string invoiceId, DateTime? docDate, string docType)
        {
            try
            {
                List<LookUpVendor<string>> getVendorLU = await _masterModuleApplicationService.GetVendors(invoiceId, AuthInformation.companyId.Value, docDate, docType);
                return Ok(getVendorLU);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("getSSIcode")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetSSICode(string type)
        {
            try
            {
                return Ok(_masterModuleApplicationService.GetSSICodeByType(type, AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Save

        [HttpPost]
        //[AllowAnonymous]
        [Route("savebeanentitymodel")]

        ////[CacheInvalidate(Position = 11, Keys = "GetAllBeanEntitysK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "GetVendorLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCustomerLU")]

        //[CacheInvalidate(Position = 11, Keys = "getallbeanentitysK,createentity,getvendorlu,getcustomerlu,getentitycontacts,createcontact")]
        //[CacheInvalidate(Position = 11, Keys = "GetEntityLU", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "getclaimsbill", Controller = "BillController")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllInvoicesK", Controller = "InvoiceController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllRecurringInvoiceK", Controller = "InvoiceController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllParkedInvoiceK", Controller = "InvoiceController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesK", Controller = "CashSalesController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "getalldebitnotek", Controller = "DebitNoteController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteK", Controller = "InvoiceController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDebitfulldebitK", Controller = "InvoiceController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllReceiptsK", Controller = "ReceiptController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillssK", Controller = "BillController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditMemoK", Controller = "CreditMemoController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllpaymentsK", Controller = "PaymentController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "getallcashpaymentsk", Controller = "BankWithdrawalController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollBillsK", Controller = "ReceiptController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPayrollPaymentsK", Controller = "PaymentController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "getalldepositk", Controller = "BankWithdrawalController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "getallbankwithdrawalk", Controller = "BankWithdrawalController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "banktransferk", Controller = "BankTransferController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "getallpostedsK", Controller = "BankTransferController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllPostedsK", Controller = "JournalVoucherController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllRecurringsK", Controller = "JournalVoucherController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllParkedsK", Controller = "JournalVoucherController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllRevaluationK", Controller = "RevaluationController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]
        //[CacheInvalidate(Position = 11, Keys = "getallbalancessk", Controller = "OpeningBalanceController", Namespace = "AppsWorld.Bean.WebAPI.Controllers")]

        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBeanEntityModel(BeanEntityModel beanEntity)
        {
            try
            {
                beanEntity.CompanyId = AuthInformation.companyId.Value;
                BeanEntity BeanEntity = _masterModuleApplicationService.SaveEntityModel(beanEntity, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(BeanEntity);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(beanEntity));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("MasterModuleController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("quickentitysave")]

        ////[CacheInvalidate(Position = 11, Keys = "GetAllBeanEntitysK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "GetVendorLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCustomerLU")]
        //[CacheInvalidate(Position = 11, Keys = "GetEntityLU", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "getentitycontacts,createcontact")]
        //[CacheInvalidate(Position = 11, Keys = "getallbeanentitysK,createentity,getvendorlu,getcustomerlu")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult QuickEntitySave(QuickBeanEntityModel beanEntity)
        {
            try
            {
                beanEntity.CompanyId = AuthInformation.companyId.Value;
                BeanEntity BeanEntity = _masterModuleApplicationService.QuickEntitySave(beanEntity);
                return Ok(BeanEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

		#endregion

		[HttpGet]
		//[AllowAnonymous]
		[Route("FinancialSummary")]
		[RolePermission(Position = 3)]
		//[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
		[CommonHeaders(Position = 1)]
		public async Task<IHttpActionResult> GetFinancialSummary(Guid EntityId)
		{
			try
			{
				FinancialSummaryModel beanEntity =await _masterModuleApplicationService.GetFinancialSummary(EntityId,AuthInformation.companyId.Value, AuthInformation.userName, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
				return Ok(beanEntity);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		#endregion


		#region FinancialSetting

		#region Get

		[HttpGet]
	   //[AllowAnonymous]
	   [Route("createfinancialsetting")]
	   [RolePermission(Position = 3)]
	   //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
	   [CommonHeaders(Position = 1)]
	   public IHttpActionResult CreateFinancialSetting()
	   {
		  try
		  {
			 FinancialSetting settings = _masterModuleApplicationService.GetFinancialSetting(AuthInformation.companyId.Value);
			 if (settings == null)
			 {
				settings = new FinancialSetting();
				settings.TimeZone = "[GMT+08:00] Asia/Singapore";
			 }
			 else
			 {
			 }
			 return Ok(settings);
		  }
		  catch (Exception ex)
		  {
			 return BadRequest(ex.Message);
		  }
	   }
	   [HttpGet]
	   //[AllowAnonymous]
	   [Route("verifyfinanciallockperiodpassword")]
	   [RolePermission(Position = 3)]
	   //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
	   [CommonHeaders(Position = 1)]
	   public IHttpActionResult VerifyFinancialLockPeriodPassword(string password)
	   {
		  try
		  {
			 bool settings = _masterModuleApplicationService.VerifyFinancialLockPeriodPassword(password, AuthInformation.companyId.Value);
			 return Ok(settings);
		  }
		  catch (Exception ex)
		  {
			 return BadRequest(ex.Message);
		  }
	   }


        #endregion

        #region Save

        [HttpPost]
        //[AllowAnonymous]
        [Route("savefinancialsetting")]
        [RolePermission(Position = 3)]

        ////[CacheInvalidate(Position = 11, Keys = "CreateFinancialSetting")]
        ////[CacheInvalidate(Position = 11, Keys = "GetVendorLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetFinancialDetails")]
        ////[CacheInvalidate(Position = 11, Keys = "ModuleActivations")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexGridModel")]

        //[CacheInvalidate(Position = 11, Keys = "CreateInvoice", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCashSales", Controller = "CashSalesController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDebitNote", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditNote", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateDoubtfulDebt", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateReceipt", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBill", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateCreditMemo", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "CreatePayment", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateWithdrawal", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateBankTransfer", Controller = "BankTransferController")]
        //[CacheInvalidate(Position = 11, Keys = "BankReconciliationLu", Controller = "BankReconciliationController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateJournalL", Controller = "JournalVoucherController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateClearing", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "GetRevaluationLookup", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "createfinancialsetting,getvendorlu,getfinancialdetails,moduleactivations")]
        //[CacheInvalidate(Position = 11, Keys = "verifyfinanciallockperiodpassword", Controller = "PayrollController", Namespace = "AppsWorld.HR.WebAPI.Controllers")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveFinancialSetting(FinancialSetting financialSetting)
        {
            try
            {
                financialSetting.CompanyId = AuthInformation.companyId.Value;
                financialSetting.ModuleDetailId = AuthInformation.moduleDetailId;
                string name = HttpContext.Current.User.Identity.Name;
                FinancialSetting financialS = _masterModuleApplicationService.Save(financialSetting, name);
                return Ok(financialS);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Currency

        [HttpGet]
        //[AllowAnonymous]
        [Route("currencyes")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult Currencyes()
        {
            try
            {
                List<Currency> Currencyes = _masterModuleApplicationService.GetAllCurrencyes(AuthInformation.companyId.Value);
                return Ok(Currencyes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcurrencycode")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public PageResult<Currency> GetAllCurrencyCode(ODataQueryOptions<Currency> options, int pageSize)
        {
            ODataQuerySettings settings = new ODataQuerySettings()
            {
                PageSize = pageSize
            };
            int companyId = Convert.ToInt32(AuthInformation.companyId.Value);
            IQueryable results = options.ApplyTo(_masterModuleApplicationService.GetAllCurrency(companyId).AsQueryable(), settings);

            Uri uri = Request.GetNextPageLink();
            long? inlineCount = Request.GetInlineCount();

            PageResult<Currency> response = new PageResult<Currency>(
               results as IEnumerable<Currency>,
               uri,
               inlineCount);

            return response;
        }

        //required to change when that will come in live
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getallcurrencycode")]
        //[RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetAllCurrencyCode(HttpRequestMessage requestMessage)
        //{
        //    try
        //    {
        //        DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
        //        DataSourceResult dr = _masterModuleApplicationService.GetAllCurrency(AuthInformation.companyId.Value).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        //        return Ok(dr);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        //[AllowAnonymous]
        [Route("currencycode")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IEnumerable<Currency> CurrencyCode(long id)
        {
            return _masterModuleApplicationService.GetCurrencyById(id);
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("currencycode")]
        ////[CacheInvalidate(Position = 11, Keys = "CurrencyCode")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCurrencyCode")]
        ////[CacheInvalidate(Position = 11, Keys = "Currencyes")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CurrencyCode(Currency currency)
        {
            try
            {
                string _name = HttpContext.Current.User.Identity.Name;
                currency.CompanyId = Convert.ToInt32(AuthInformation.companyId.Value);
                Currency Currency = _masterModuleApplicationService.Save(currency, _name);

                //string Headline = string.Format("{0} {1}", currency.Id, currency.Name);
                //string URL = string.Format("/{0}/{1}", "currency", currency.CompanyId.ToString());
                //string Description = "To be Defined";
                //int status = Convert.ToInt32(currency.Status);
                //_esOperations.SaveInES<Currency>(currency, currency.CompanyId.ToString(), "currency", Headline, URL, Description, status);

                return Ok(Currency);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }


        #endregion

        #region MultiCurrency

        [HttpPost]
        //[AllowAnonymous]
        [Route("savemulticurrencysetting")]
        [RolePermission(Position = 3)]

        ////[CacheInvalidate(Position = 11, Keys = "GetMultiCurrencyStatus")]
        ////[CacheInvalidate(Position = 11, Keys = "MultiCurrencySettings")]
        ////[CacheInvalidate(Position = 11, Keys = "QuickCreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "ModuleActivations")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexGridModel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllChartOfAccountsK")]
        ////[CacheInvalidate(Position = 11, Keys = "ChartOfAccount")]

        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetRevaluationLookup", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "getmulticurrencystatus,multicurrencysettings,quickcreateentity,moduleactivations,getallchartofaccountsK,chartofaccount")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveMultiCurrencySetting(MultiCurrencySetting multiCurrencySetting)
        {
            try
            {
                multiCurrencySetting.CompanyId = AuthInformation.companyId.Value;
                multiCurrencySetting.ModuleDetailId = AuthInformation.moduleDetailId;
                string _name = HttpContext.Current.User.Identity.Name;
                MultiCurrencySetting MultiCurrencySetting = _masterModuleApplicationService.SaveMulti(multiCurrencySetting, _name);
                return Ok(MultiCurrencySetting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }

        }


        [HttpPost]
        [AllowAnonymous]
        [Route("savemulticurrencysettingsync")]
        //[RolePermission(Position = 3)]

        ////[CacheInvalidate(Position = 11, Keys = "GetMultiCurrencyStatus")]
        ////[CacheInvalidate(Position = 11, Keys = "MultiCurrencySettings")]
        ////[CacheInvalidate(Position = 11, Keys = "QuickCreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "ModuleActivations")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexGridModel")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllChartOfAccountsK")]
        ////[CacheInvalidate(Position = 11, Keys = "ChartOfAccount")]

        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "GetRevaluationLookup", Controller = "RevaluationController")]
        //[CacheInvalidate(Position = 11, Keys = "getmulticurrencystatus,multicurrencysettings,quickcreateentity,moduleactivations,getallchartofaccountsK,chartofaccount")]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveMultiCurrencySettingSync(MultiCurrencySetting multiCurrencySetting)
        {
            try
            {
                //multiCurrencySetting.CompanyId = AuthInformation.companyId.Value;
                //multiCurrencySetting.ModuleDetailId = AuthInformation.moduleDetailId;
                string _name = HttpContext.Current.User.Identity.Name;
                MultiCurrencySetting MultiCurrencySetting = _masterModuleApplicationService.SaveMulti(multiCurrencySetting, _name);
                return Ok(MultiCurrencySetting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }

        }



        [HttpGet]
        //[AllowAnonymous]
        [Route("activatemulticurrency")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ActivateMultiCurrency()
        {
            try
            {

                bool ActivateMultiCurrency = _masterModuleApplicationService.ActivateMultiCurrencyModule(AuthInformation.companyId.Value);
                return Ok(ActivateMultiCurrency);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("multicurrencysettings")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult MultiCurrencySettings()
        {
            try
            {
                MultiCurrencySetting MultiCurrencySettings = _masterModuleApplicationService.GetAllMultiCurrencySettings(AuthInformation.companyId.Value);
                return Ok(MultiCurrencySettings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getmulticurrencystatus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetMultiCurrencyStatus()
        {
            try
            {
                bool GetMultiCurrencyStatus = _masterModuleApplicationService.GetMultiCurrencyModuleStatus(AuthInformation.companyId.Value);
                return Ok(GetMultiCurrencyStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region GstSetting

        #region Get

        [HttpGet]
        //[AllowAnonymous]
        [Route("gstsettings")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GSTSettings()
        {
            try
            {
                GSTSetting GSTSettings = _masterModuleApplicationService.GetGstSettingcompany(AuthInformation.companyId.Value);
                return Ok(GSTSettings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("isgstallowed")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult IsGSTAllowed(DateTime docDate)
        {
            try
            {
                var ServiceCompanyId = Convert.ToInt64(AuthInformation.Values.FirstOrDefault(s => s.Key == "ServiceCompanyId").Value);
                bool IsGSTAllowed = _masterModuleApplicationService.GetGSTSettingByCompanyIdAndDate(AuthInformation.companyId.Value, docDate, ServiceCompanyId);
                return Ok(IsGSTAllowed);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getgststatus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetGSTStatus()
        {
            try
            {
                bool model = _masterModuleApplicationService.GetModuleStatus(ModuleNameConstants.GST, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("activategst")]
        ////[CacheInvalidate(Position = 11, Keys = "GetGSTStatus")]
        ////[CacheInvalidate(Position = 11, Keys = "ModuleActivations")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ActivateGST()
        {
            try
            {
                bool model = _masterModuleApplicationService.ActivateModule(ModuleNameConstants.GST, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region Save

        [HttpPost]
        //[AllowAnonymous]
        [Route("savegstsetting")]
        //[CacheInvalidate(Position = 11, Keys = "ActivateGST,GetGSTStatus,GSTSettings,ddIsGSTAllowed,ActivateMultiCurrency,ModuleActivations,GetForexGridModel")]
        ////[CacheInvalidate(Position = 11, Keys = "ActivateGST")]
        ////[CacheInvalidate(Position = 11, Keys = "GetGSTStatus")]
        ////[CacheInvalidate(Position = 11, Keys = "GSTSettings")]
        ////[CacheInvalidate(Position = 11, Keys = "IsGSTAllowed")]
        ////[CacheInvalidate(Position = 11, Keys = "ActivateMultiCurrency")]
        ////[CacheInvalidate(Position = 11, Keys = "ModuleActivations")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexGridModel")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveGSTSetting(GSTSetting gstSetting)
        {
            try
            {
                gstSetting.CompanyId = AuthInformation.companyId.Value;
                //  string _name = HttpContext.Current.User.Identity.Name;
                GSTSetting GSTSetting = _masterModuleApplicationService.SaveGst(gstSetting, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(GSTSetting);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion

        #endregion

        #region segMentMaster

        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("segmentmasters")]
        //[RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SegmentMasters()
        //{
        //try
        //{
        //IEnumerable<SegmentMasterDTO> SegmentMasters = _masterModuleApplicationService.GetAllMasters(AuthInformation.companyId.Value);
        //return Ok(SegmentMasters);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        [HttpGet]
        //[AllowAnonymous]
        [Route("getsegmentreportstatus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetSegmentReportStatus()
        {
            try
            {
                bool GetSegmentReportStatus = _masterModuleApplicationService.GetModuleStatu(AuthInformation.companyId.Value);
                return Ok(GetSegmentReportStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpPost]
        ////[AllowAnonymous]
        //[Route("savesegmentmaster")]
        //[RolePermission(Position = 3)]

        //////[CacheInvalidate(Position = 11, Keys = "SegmentMasters")]
        //////[CacheInvalidate(Position = 11, Keys = "EnableOrDisableSegmentReport")]
        //////[CacheInvalidate(Position = 11, Keys = "GetSegmentReportStatus")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesLUs", Controller = "CashSalesController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs", Controller = "InvoiceController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteAllLUs", Controller = "DebitNoteController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "InvoiceController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs", Controller = "InvoiceController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetReceiptsLookups", Controller = "ReceiptController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "CreditMemoController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalsLookups", Controller = "BankWithdrawalController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU", Controller = "JournalVoucherController")]
        ////[CacheInvalidate(Position = 11, Keys = "OpeningBalanceController", Controller = "GetOpeningBalanceLookups")]
        //////[CacheInvalidate(Position = 11, Keys = "SegmentMasters,EnableOrDisableSegmentReport,GetSegmentReportStatus")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SaveSegmentMaster(SegmentMasterDTO segmentMasterDTO)
        //{
        //try
        //{
        //segmentMasterDTO.CompanyId = AuthInformation.companyId.Value;
        ////_eventStoreOperations.SaveEventToStream(beanEntity, "BeanMaster-SaveBeanEntity");
        //string _name = HttpContext.Current.User.Identity.Name;
        //SegmentMasterDTO segmentMasterDTOResponse = _masterModuleApplicationService.SaveSegmentMaster(segmentMasterDTO);
        //return Ok(segmentMasterDTOResponse);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //throw;
        //}
        //}
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("enableOrdisablesegmentreport")]
        //[RolePermission(Position = 3)]
        //////[CacheInvalidate(Position = 11, Keys = "SegmentMasters,GetSegmentReportStatus")]
        //////[CacheInvalidate(Position = 11, Keys = "GetSegmentReportStatus")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesLUs", Controller = "CashSalesController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs", Controller = "InvoiceController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetDebitNoteAllLUs", Controller = "DebitNoteController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "InvoiceController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs", Controller = "InvoiceController")]
        //////[CacheInvalidate(Position =11,Keys = "GetReceiptsLookups", "ReceiptController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "CreditMemoController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetWithdrawalsLookups", Controller = "BankWithdrawalController")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU", Controller = "JournalVoucherController")]
        ////[CacheInvalidate(Position = 11, Keys = "OpeningBalanceController", Controller = "GetOpeningBalanceLookups")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult EnableOrDisableSegmentReport(long Id, string status)
        //{
        //try
        //{
        //RecordStatusEnum sgStatus = (RecordStatusEnum)Enum.Parse(typeof(RecordStatusEnum), status);

        //            RecordStatusEnum result = _masterModuleApplicationService.EnableOrDisableSegmentReport(AuthInformation.companyId.Value, Id, sgStatus, AuthInformation.userName);
        //            return Ok(result.ToString());
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //            throw;
        //        }
        //    }

        [HttpPost]
        //[AllowAnonymous]
        [Route("activatesegmentreport")]
        //[CacheInvalidate(Position = 11, Keys = "moduleactivations")]
        ////[CacheInvalidate(Position = 11, Keys = "ModuleActivations")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ActivateSegmentReport()
        {
            try
            {
                bool model = _masterModuleApplicationService.ActivateModule(ModuleNameConstants.SegmentReporting, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion

        #region AllowableNonAllowable

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallowablenonallowablestatus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllowableNonAllowableStatus()
        {
            try
            {
                bool getAllowableNonAllowableStatus = _masterModuleApplicationService.GetModuleStatus(AuthInformation.companyId.Value);
                return Ok(getAllowableNonAllowableStatus);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("activateallowablenonallowable")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ActivateAllowableNonAllowable()
        {
            try
            {
                bool ActivateAllowableNonAllowable = _masterModuleApplicationService.ActivateAllowableNonAllowable(ModuleNameConstants.AllowableNonAllowable, AuthInformation.companyId.Value);
                return Ok(ActivateAllowableNonAllowable);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region NoSupportingdocument
        [HttpGet]
        //[AllowAnonymous]
        [Route("getnosupportingdocuments")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetNoSupportingDocuments()
        {
            try
            {
                bool getNoSupportingDocuments = _masterModuleApplicationService.GetNoSupportingDocuments(AuthInformation.companyId.Value);
                return Ok(getNoSupportingDocuments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("activatenosupportingdocuments")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ActivateNoSupportingDocuments()
        {
            try
            {
                bool noSupportingDocuments = _masterModuleApplicationService.ActivateAllowableNonAllowable(ModuleNameConstants.NoSupportingDocuments, AuthInformation.companyId.Value);
                return Ok(noSupportingDocuments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region General_Setting_Features
        [HttpPost]
        //[AllowAnonymous]
        [Route("savebeangeneralsetting")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult savebeangeneralsetting(GeneralSettingFeatureModel generalSettingFeatureModel)
        {
            try
            {
                return Ok(_masterModuleApplicationService.SaveGeneralSettingFeature(generalSettingFeatureModel,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection,AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region BankReconciliation

        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("bankreconciliations")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult BankReconciliations()
        //{
        //try
        //{
        //BankReconciliationSetting bankReconciliation = _masterModuleApplicationService.GetBankReconciliationBYCompany(AuthInformation.companyId.Value);
        //return Ok(bankReconciliation);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}

        [HttpGet]
        //[AllowAnonymous]
        [Route("getbankreconciliationstatus")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetBankReconciliationStatus()
        {
            try
            {
                bool getBankReconciliation = _masterModuleApplicationService.GetModuleStatuss(AuthInformation.companyId.Value);
                return Ok(getBankReconciliation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        ////[AllowAnonymous]
        //[Route("savebankreconciliation")]
        //////[CacheInvalidate(Position = 11, Keys = "BankReconciliations")]
        //////[CacheInvalidate(Position = 11, Keys = "GetBankReconciliationStatus")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SaveBankReconciliation(BankReconciliationSetting bankReconciliation)
        //{
        //try
        //{
        //bankReconciliation.CompanyId = AuthInformation.companyId.Value;
        //BankReconciliationSetting dn = _masterModuleApplicationService.Save(bankReconciliation);
        //ActivateBankReconciliation(AuthInformation.companyId.Value);
        //return Ok(dn);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}


        private IHttpActionResult ActivateBankReconciliation(long companyId)
        {
            try
            {
                bool ActivateBankReconciliation = _masterModuleApplicationService.ActivateModule(AuthInformation.companyId.Value);
                return Ok(ActivateBankReconciliation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region ForexBlock
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getallforexsk")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetAllForexsK(HttpRequestMessage requestMessage, string type)
        //{
        //try
        //{
        //int companyId = Convert.ToInt32(AuthInformation.companyId.Value);
        //DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
        //var model = _masterModuleApplicationService.GetAllForexById(companyId, type).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        //return Ok(model);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        //[HttpGet]
        ////[AllowAnonymous]

        //[Route("getforexbyid")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetForexById(long id, string type)
        //{
        //try
        //{
        //ForexModel model = _masterModuleApplicationService.GetForexByIdNew(id, AuthInformation.companyId.Value, type);
        //return Ok(model);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        //[HttpGet]
        ////[AllowAnonymous]
        ////[EnableQuery]
        //[Route("getforexgridmodel")]
        //[RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetForexGridModel(DateTime date)
        //{
        //try
        //{
        //ForexGridModel model = _masterModuleApplicationService.GetForexGridModel(AuthInformation.companyId.Value, date);
        //return Ok(model);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        //[HttpPost]
        ////[AllowAnonymous]
        //[Route("saveforexmodel")]
        ////////[CacheInvalidate(Position =11,Keys = "GetAllForexs")]       
        ////[CacheInvalidate(Position = 11, Keys = "GetAllForexs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllForexsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexById")]
        ////[CacheInvalidate(Position = 11, Keys = "EnableOrDisableForex")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexGridModel")]
        ////[CacheInvalidate(Position = 11, Keys = "MultiCurrencyInformation")]
        ////[CacheInvalidate(Position = 11, Keys = "ExchangeRateInformation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SaveForexModel(ForexModel forex)
        //{
        //try
        //{
        //forex.CompanyId = AuthInformation.companyId.Value;
        //string _name = HttpContext.Current.User.Identity.Name;
        //string result = _masterModuleApplicationService.SaveForex(forex);

        //return Ok(result);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("getallforexs")]
        //[RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetAllForexs(ODataQueryOptions<Forex> options, int pageSize, string type)
        //{
        //try
        //{
        //ODataQuerySettings settings = new ODataQuerySettings()
        //{
        //PageSize = pageSize
        //};
        //int companyId = Convert.ToInt32(AuthInformation.companyId.Value);
        //IQueryable results = options.ApplyTo(_masterModuleApplicationService.GetAllForex(companyId, type).AsQueryable().OrderByDescending(c => c.CreatedDate), settings);

        //            Uri uri = Request.GetNextPageLink();
        //            long? inlineCount = Request.GetInlineCount();

        //            PageResult<Forex> response = new PageResult<Forex>(
        //               results as IEnumerable<Forex>,
        //               uri,
        //               inlineCount);

        //            return Ok(response);
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }

        [HttpGet]
        [Route("getfinancialdetails")]
        //[AllowAnonymous]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetFinancialDetails()
        {
            try
            {
                FinancialDetailModel model = _masterModuleApplicationService.GetFinancialDetailModel(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet]
        //[Route("enableordisableforex")]
        ////[AllowAnonymous]
        //[RolePermission(Position = 3)]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllForexs")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllForexsK")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexById")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexGridModel")]
        ////[CacheInvalidate(Position = 11, Keys = "MultiCurrencyInformation")]
        ////[CacheInvalidate(Position = 11, Keys = "ExchangeRateInformation")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult EnableOrDisableForex(string Id, string tableName, string status, string userName)
        //{
        //try
        //{
        //bool model = _masterModuleApplicationService.EnableOrDisableForex(Id, tableName, status, userName);
        //return Ok(model);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        #endregion

        #region ChartOfAccount

	   [HttpGet]
	   //[AllowAnonymous]
	   [Route("getallchartofaccountsK")]
	   [RolePermission(Position = 3)]
	   //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
	   [CommonHeaders(Position = 1)]
	   public async Task<IHttpActionResult> GetAllChartOfAccountsK(HttpRequestMessage requestMessage)
	   {
		  try
		  {
			 int companyId = Convert.ToInt32(AuthInformation.companyId.Value);
			 DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var GetAllChartOfAccountsK = await _masterModuleApplicationService.GetAllChartOfAccountsK(companyId, AuthInformation.userName);
			    return Ok(GetAllChartOfAccountsK.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
			
		  }
		  catch (Exception ex)
		  {
			 return BadRequest(ex.Message);
		  }
	   }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getaccounttypes")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAccountTypes(bool IsSystem)
        {
            try
            {
                IEnumerable<AccountType> GetAccountTypes = _masterModuleApplicationService.GetAccountTypes(AuthInformation.companyId.Value, IsSystem);
                return Ok(GetAccountTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("savechartofaccount")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllChartOfAccountsK")]
        //[CacheInvalidate(Position = 11, Keys = "GetChartOfAccounts")]
        //[CacheInvalidate(Position = 11, Keys = "ChartOfAccount")]
        //[CacheInvalidate(Position = 11, Keys = "GetAccountTypes")]
        //[CacheInvalidate(Position = 11, Keys = "CreateItemsLU")]
        //[CacheInvalidate(Position = 11, Keys = "GetDebitNoteAllLUs", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU", Controller = "JournalVoucherController")]
        //[CacheInvalidate(Position = 11, Keys = "GetWithdrawalsLookups", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllClearingLUs", Controller = "ClearingController")]
        //[CacheInvalidate(Position = 11, Keys = "GetServiceCompanyOpeningBalance", Controller = "OpeningBalanceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetReceiptsLookups", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "CreateRevaluation", Controller = "RevaluationController")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveChartOfAccount(ChartOfAccount chartOfAccount)
        {

            try
            {
                chartOfAccount.CompanyId = AuthInformation.companyId.Value;
                //string _name = HttpContext.Current.User.Identity.Name;
                ChartOfAccount ChartOfAccount = _masterModuleApplicationService.SaveChartOfAccount(chartOfAccount);
                return Ok(ChartOfAccount);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(chartOfAccount));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("MasterModuleController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
                throw;
            }

        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("getchartofaccounts")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetChartOfAccounts()
        {
            try
            {
                ChartOfAccountModelLU GetChartOfAccounts = _masterModuleApplicationService.GetChartOfAccountLU(AuthInformation.companyId.Value);
                return Ok(GetChartOfAccounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("deletechartofaccount")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult DeleteChartOfAccount(AppsWorld.MasterModule.Models.DeleteChartofaccount deleteChartofaccount)
        {
            try
            {
                deleteChartofaccount.CompanyId = AuthInformation.companyId.Value;
                string GetChartOfAccounts = _masterModuleApplicationService.DeleteChartofaccount(deleteChartofaccount, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(GetChartOfAccounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("chartofaccount")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ChartOfAccount(long Id)
        {
            try
            {
                ChartOfAccountDTO ChartOfAccount = _masterModuleApplicationService.GetChartOfAccountById(Id);
                return Ok(ChartOfAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region TaxCodes

        [HttpGet]
        //[AllowAnonymous]
        [Route("getalltaxcodemodelsk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllTaxCodeModelsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(
               requestMessage.RequestUri.ParseQueryString().GetKey(0));

                var model = _masterModuleApplicationService.GetAllTaxCodeModelsK(AuthInformation.companyId.Value,Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("gettaxcodes")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetTaxCodeLU()
        {
            try
            {
                TaxCodeModel model = _masterModuleApplicationService.GetTaxCodeLU(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("taxcode")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult TaxCode(long Id)
        {
            try
            {
                //IEnumerable<TaxCode> model = _masterModuleApplicationService.TaxCode(Id);
                TaxCode model = _masterModuleApplicationService.TaxCode(Id);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savetaxcode")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllTaxCodeModelsK,TaxCode,GetTaxCodeLU")]
        ////[CacheInvalidate(Position = 11, Keys = "TaxCode")]
        ////[CacheInvalidate(Position = 11, Keys = "GetTaxCodeLU")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveTaxCode(TaxCode taxCode)
        {
            try
            {
                taxCode.CompanyId = AuthInformation.companyId.Value;
                string name = HttpContext.Current.User.Identity.Name;
                TaxCode TaxCode = _masterModuleApplicationService.Save(taxCode, name, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);

                //string Headline = string.Format("{0} {1}", taxCode.Code, taxCode.Description);
                //string URL = string.Format("/{0}/{1}", "taxcode", taxCode.CompanyId.ToString());
                //string Description = "To be Defined";
                //int status = Convert.ToInt32(taxCode.Status);
                //_esOperations.SaveInES<TaxCode>(taxCode, taxCode.CompanyId.ToString(), "taxcode", Headline, URL, Description, status);

                return Ok(TaxCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Item

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallitemsk")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<IHttpActionResult> GetAllItemsK(HttpRequestMessage requestMessage)
        {
            try
            {
                DataSourceRequest request = JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                var model = await _masterModuleApplicationService.GetAllItemK(AuthInformation.companyId.Value);
                return Ok(model.ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createitemsLU")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateItemsLU(Guid id, bool isGst)
        {
            try
            {
                ItemModelLU CreateItemsLU = _masterModuleApplicationService.CreateItemLU(AuthInformation.companyId.Value, id, isGst);
                return Ok(CreateItemsLU);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createitem")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateItem(Guid id)
        {
            try
            {
                ItemModel model = _masterModuleApplicationService.CreateItem(id, AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("createworkflowitembyrounding")]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult CreateWorkFlowItem(long companyId)
        {
            try
            {
                ItemWFModel model = _masterModuleApplicationService.CreateWFItem(companyId);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        //[HttpPost]
        //[Route("saveitem")]
        ////[CacheInvalidate(Position = 11, Keys = "getallitemsk,createitemsLU,createitem,getsaleitems,getincidentalitem,GetItemBasedIncidentalSetup")]
        //////[CacheInvalidate(Position = 11, Keys = "GetAllItemsK")]
        //////[CacheInvalidate(Position = 12, Keys = "CreateItemsLU")]
        //////[CacheInvalidate(Position = 13, Keys = "CreateItem")]
        //////[CacheInvalidate(Position = 14, Keys = "GetSaleItems")]
        //////[CacheInvalidate(Position = 15, Keys = "GetItemBasedIncidentalSetup")]
        //[RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult SaveItem(Item item)
        //{

        //try
        //{
        //item.CompanyId = AuthInformation.companyId.Value;
        ////_eventStoreOperations.SaveEventToStream(item, "BeanMaster-SaveItem");
        //string _name = HttpContext.Current.User.Identity.Name;
        //Item Item = _masterModuleApplicationService.Save(item, _name, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
        //return Ok(Item);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //throw;
        //}


        //}

        [HttpGet]
        //[AllowAnonymous]
        [Route("getsaleitems")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IEnumerable<Item> GetSaleItems(bool IsActiveItems)
        {
            return _masterModuleApplicationService.GetSaleItems(IsActiveItems, AuthInformation.companyId.Value);
        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("getsaleitems")]
        [RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public async Task<List<ItemModelVM>> GetSaleItems(Guid? DocumentId, string type)
            {
                return await _masterModuleApplicationService.GetSaleItems(AuthInformation.companyId.Value, DocumentId, type);
            }

        #endregion

        #region ModuleActivations

        [HttpGet]
        //[AllowAnonymous]
        [Route("moduleactivations")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult ModuleActivations()
        {
            try
            {
                ModuleActivationModel moduleActivation = _masterModuleApplicationService.ModuleActivations(AuthInformation.companyId.Value);
                return Ok(moduleActivation);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        [HttpGet]
        [Route("gethistory")]
        //[AllowAnonymous]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetHistory(Guid id, string type)
        {
            try
            {
                List<ActivityHistory> activityHistory = _masterModuleApplicationService.GetHistory(AuthInformation.companyId.Value, id, type);
                return Ok(activityHistory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("currencyenableordisable")]
        ////[CacheInvalidate(Position = 11, Keys = "CurrencyCode")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllCurrencyCode")]
        ////[CacheInvalidate(Position = 11, Keys = "Currencyes")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "QuickCreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "GetForexById")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCashSalesLUs", Controller = "CashSalesController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllInvoiceLUs", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetDebitNoteAllLUs", Controller = "DebitNoteController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllDoubtfulDebtLUs", Controller = "InvoiceController")]
        //[CacheInvalidate(Position = 11, Keys = "GetReceiptsLookups", Controller = "ReceiptController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBillLU", Controller = "BillController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllCreditNoteLUs", Controller = "CreditMemoController")]
        //[CacheInvalidate(Position = 11, Keys = "GetPaymentsLookups", Controller = "PaymentController")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllJournalLU", Controller = "JournalVoucherController")]
        //[CacheInvalidate(Position = 11, Keys = "GetOpeningBalanceLookups", Controller = "OpeningBalanceController")]
        //[CacheInvalidate(Position = 11, Keys = "currencycode,getallcurrencycode,currencyes,createentity,quickcreateentity")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CurrencyEnableOrDisable(EnableOrDisable enableOrDisable)
        {

            try
            {
                enableOrDisable.CompanyId = AuthInformation.companyId.Value;
                EnableOrDisable enableOrdisable = _masterModuleApplicationService.CurrencyEnableOrDisable(enableOrDisable);
                return Ok(enableOrdisable);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region IncidentalSetUp
        //[HttpGet]
        //[AllowAnonymous]
        // [Route("getincidentalitem")]
        ////[AllowAnonymous]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult GetItemBasedIncidentalSetup()
        //{
        //try
        //{
        //IncidentalModel model = _masterModuleApplicationService.GetIncidetalModel(AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
        //return Ok(model);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}

        [HttpGet]
        //[AllowAnonymous]
        [Route("getallincidentalitem")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllIncidentalItem(long documentId)
        {
            try
            {
                return Ok(_masterModuleApplicationService.GetListOfItemWF(AuthInformation.companyId.Value, documentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        [HttpGet]
        //[AllowAnonymous]
        [Route("getentity")]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult GetEntity(Guid documentId)
        {
            try
            {
                return Ok(_masterModuleApplicationService.GetEntity(documentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }















        #region Contacts


        [HttpGet]
        [Route("createcontact")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult createcontact()
        {
            try
            {
                var createContactModel = _masterModuleApplicationService.CreateContactModel(AuthInformation.companyId.Value);
                return Ok(createContactModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("getentitycontacts")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult getentitycontacts(Guid entityId)
        {
            try
            {
                var contacts = _masterModuleApplicationService.GetEntityContacts(AuthInformation.companyId.Value, entityId);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("savebeanentitycontact")]
        [CacheInvalidate(Keys = "getentitycontacts,createcontact")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult savebeanentitycontact(ContactModel contactModel, Guid entityId)
        {
            try
            {
                contactModel.CompanyId = AuthInformation.companyId.Value;
                _masterModuleApplicationService.SaveBeanEntityContact(contactModel, entityId, false, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion    Contacts
        #region   Nagendra
        [HttpPost]
        [Route("savebeanentitycontact")]
        public IHttpActionResult savebeanentitycontact(ContactModel contactModel)
        {
            try
            {
                _masterModuleApplicationService.SaveBeanEntityContact(contactModel, contactModel.ClientId, false, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion  Nagendra



        #region Bean - WF sync 
        #region Save

        [HttpPost]
        //[AllowAnonymous]
        [Route("savebeanentitymodelSync")]

        ////[CacheInvalidate(Position = 11, Keys = "GetAllBeanEntitysK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "GetVendorLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCustomerLU")]

        //[CacheInvalidate(Position = 11, Keys = "GetAllBeanEntitysK,CreateEntity,GetVendorLU,getcustomerlu,getentitycontacts,createcontact")]
        //[CacheInvalidate(Position = 11, Keys = "GetEntityLU", Controller = "BankWithdrawalController")]
        // [RolePermission(Position = 3)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult SaveBeanEntityModelSync(BeanEntityModel beanEntity)
        {
            try
            {

                //beanEntity.CompanyId = AuthInformation.companyId.Value;
                //_eventStoreOperations.SaveEventToStream(beanEntity, "BeanMaster-SaveBeanEntity");
                string _name = HttpContext.Current.User.Identity.Name;
                //BeanEntity BeanEntity = _beanEntityService.SaveEntityModel(beanEntity, _name);
                BeanEntity BeanEntity = _masterModuleApplicationService.SaveEntityModel(beanEntity, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(BeanEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("quickentitysaveSync")]

        ////[CacheInvalidate(Position = 11, Keys = "GetAllBeanEntitysK")]
        ////[CacheInvalidate(Position = 11, Keys = "CreateEntity")]
        ////[CacheInvalidate(Position = 11, Keys = "GetVendorLU")]
        ////[CacheInvalidate(Position = 11, Keys = "GetCustomerLU")]
        //[CacheInvalidate(Position = 11, Keys = "GetEntityLU", Controller = "BankWithdrawalController")]
        //[CacheInvalidate(Position = 11, Keys = "getentitycontacts,createcontact")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllBeanEntitysK,CreateEntity,GetVendorLU,getcustomerlu")]
        //  [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult QuickEntitySaveSync(QuickBeanEntityModel beanEntity)
        {
            try
            {
                beanEntity.CompanyId = AuthInformation.companyId.Value;
                BeanEntity BeanEntity = _masterModuleApplicationService.QuickEntitySave(beanEntity);
                return Ok(BeanEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion


        [HttpPost]
        //[AllowAnonymous]
        [Route("savebeanentitycontactSync")]
        [CacheInvalidate(Keys = "getentitycontacts,createcontact")]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //  [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult savebeanentitycontactSync(ContactModel contactModel)
        {
            try
            {
                contactModel.CompanyId = AuthInformation.companyId.Value;
                bool? isWorkFlow = true;
                _masterModuleApplicationService.SaveBeanEntityContact(contactModel, contactModel.ClientId, false, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, null, isWorkFlow);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #endregion


        #region Bean - WF Sync

        [HttpGet]
        //[AllowAnonymous]
        [Route("verifyfinanciallockperiodpasswordsync")]
        // [RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        public IHttpActionResult VerifyFinancialLockPeriodPasswordSync(string password, long companyId)
        {
            try
            {
                bool settings = _masterModuleApplicationService.VerifyFinancialLockPeriodPassword(password, companyId);
                return Ok(settings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        //[AllowAnonymous]
        [Route("createfinancialsettingSync")]
        //[RolePermission(Position = 3)]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateFinancialSettingSync(long companyId)
        {
            try
            {
                FinancialSetting settings = _masterModuleApplicationService.GetFinancialSetting(companyId);
                if (settings == null)
                {
                    settings = new FinancialSetting();
                    settings.TimeZone = "[GMT+08:00] Asia/Singapore";
                }
                else
                {
                }
                return Ok(settings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Route("saveitemsync")]
        //[CacheInvalidate(Position = 11, Keys = "GetAllItemsK,CreateItemsLU,CreateItem,GetSaleItems,GetItemBasedIncidentalSetup")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllItemsK")]
        ////[CacheInvalidate(Position = 12, Keys = "CreateItemsLU")]
        ////[CacheInvalidate(Position = 13, Keys = "CreateItem")]
        ////[CacheInvalidate(Position = 14, Keys = "GetSaleItems")]
        ////[CacheInvalidate(Position = 15, Keys = "GetItemBasedIncidentalSetup")]
        //[RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveItemSync(Item item)
        {

            try
            {
                item.CompanyId = AuthInformation.companyId.Value;
                //_eventStoreOperations.SaveEventToStream(item, "BeanMaster-SaveItem");
                string _name = HttpContext.Current.User.Identity.Name;
                Item Item = _masterModuleApplicationService.Save(item, _name, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(Item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }


        }
        #endregion Bean 


        #region HR_Bean_sync
        //[HttpGet]
        ////[AllowAnonymous]
        //[Route("multicurrencyinformationsync")]
        ////[RolePermission(Position = 3)]
        ////[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        //[CommonHeaders(Position = 1)]
        //public IHttpActionResult multicurrencyinformationsync(string DocumentCurrency, DateTime Documentdate, bool IsBase)
        //{
        //try
        //{
        //Forex forex = _masterModuleApplicationService.GetMultiCurrencyInformation(DocumentCurrency, Documentdate, IsBase, AuthInformation.companyId.Value);
        //return Ok(forex);
        //}
        //catch (Exception ex)
        //{
        //return BadRequest(ex.Message);
        //}
        //}
        #endregion



        #region EntityMigrationForOldCompany

        [HttpGet]
        //[AllowAnonymous]
        [Route("entitymigration")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult EntityMigration()
        {
            try
            {
                return Ok(_masterModuleApplicationService.EntityMigration(AuthInformation.companyId.Value));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion EntityMigrationForOldCompany





        #region 



        [HttpGet]
        //[AllowAnonymous]
        [Route("getincidentalitem")]
        //[AllowAnonymous]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetItemBasedIncidentalSetupNew(Guid id)
        {
            try
            {
                IncidentalNewModel IncidentalModel = _masterModuleApplicationService.GetIncidetalModelNew(id, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(IncidentalModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }






        [HttpPost]
        [Route("saveitem")]
        //[CacheInvalidate(Position = 11, Keys = "getallitemsk,createitemsLU,createitem,getsaleitems,getincidentalitem,GetItemBasedIncidentalSetup")]
        ////[CacheInvalidate(Position = 11, Keys = "GetAllItemsK")]
        ////[CacheInvalidate(Position = 12, Keys = "CreateItemsLU")]
        ////[CacheInvalidate(Position = 13, Keys = "CreateItem")]
        ////[CacheInvalidate(Position = 14, Keys = "GetSaleItems")]
        ////[CacheInvalidate(Position = 15, Keys = "GetItemBasedIncidentalSetup")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveItem(IncidentalNewModel itemsmodel)
        {

            try
            {
                long companyId = AuthInformation.companyId.Value;
                //_eventStoreOperations.SaveEventToStream(item, "BeanMaster-SaveItem");
                string _name = HttpContext.Current.User.Identity.Name;
                var Item = _masterModuleApplicationService.SaveItems(itemsmodel, _name, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection, companyId);
                return Ok(Item);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(itemsmodel));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("MasterModuleController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
                throw;
            }


        }





        [HttpGet]
        //[AllowAnonymous]
        [Route("deleteincidentalitem")]
        //[AllowAnonymous]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult DeleteIncidentalItem(Guid id)
        {
            try
            {
                string incidental = _masterModuleApplicationService.DeleteIncidentalItem(id, AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(incidental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpGet]
        //[AllowAnonymous]
        [Route("getallincidentalitems")]
        //[AllowAnonymous]
        //[Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllIncidentalItems()
        {
            try
            {
                List<IncidentalNewModel> IncidentalModel = _masterModuleApplicationService.GetAllIncidentalItems(AuthInformation.companyId.Value);
                return Ok(IncidentalModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region  communication
        [HttpGet]
        // [AllowAnonymous]//newly added
        [Route("getcommunicationK")]
        //[RolePermissionFilter (ScreenName =Constrant.Communication,PermissionName =Constrant.View,ModuleName = Constrant.WorkflowCursor,ParentScreenName = Constrant.Clients)]
        //[CacheOutput(ClientTimeSpan = 0, ServerTimeSpan = 6000)]
        //[RolePermissionFilter(Position = 0, ScreenName = Constrant.Communication, PermissionName = Constrant.View, ModuleName = Constrant.WorkflowCursor, ParentScreenName = Constrant.Clients, ClientTimeSpan = 0, ServerTimeSpan = 6000)]
        public DataSourceResult GetCommunicationK(HttpRequestMessage requestMessage, Guid entityId)
        {
            DataSourceRequest request =
               JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
            return _masterModuleApplicationService.GetCommunication(entityId, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection)
               .OrderByDescending(a => a.CreatedDate)
               .ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
        }


        #endregion communication

        #region InterCo

        [HttpGet]
        //[AllowAnonymous]
        [Route("InterCoClearingLu")]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult InterCoClearingLu(bool? isClearing)
        {
            try
            {
                return Ok(_masterModuleApplicationService.InterCompnayClearingLu(AuthInformation.companyId.Value, isClearing, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("SaveInterCoClearing")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveInterCoClearing(InterCoClearingVM interCoClearingVM)
        {
            try
            {
                interCoClearingVM.CompanyId = AuthInformation.companyId.Value;
                InterCoClearingVM interCompanyClearingVM = _masterModuleApplicationService.SaveInterCoClearing(interCoClearingVM, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(interCompanyClearingVM);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(interCoClearingVM));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("MasterModuleController", ex, ex.Message, AdditionalInfo);

                return BadRequest(ex.Message);
                throw;
            }
        }
        [HttpGet]
        //[AllowAnonymous]
        [Route("getallcoamappinglus")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllCOAMappingLUS()
        {
            try
            {
                COAMappingLU model = _masterModuleApplicationService.GetCoaMappingLu(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createcoamapping")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateCOAMapping()
        {
            try
            {
                COAMappingModel model = _masterModuleApplicationService.CreateCOAMapping(AuthInformation.companyId.Value, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        //[AllowAnonymous]
        [Route("savecoamapping")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveCOAMapping(COAMappingModel interCoClearingVM)
        {
            try
            {
                interCoClearingVM.CompanyId = AuthInformation.companyId.Value;
                COAMappingModel cOAMappingModel = _masterModuleApplicationService.SaveCOAMapping(interCoClearingVM);
                return Ok(cOAMappingModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("GetAllTaxCodeMappingLu")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult GetAllTaxCodeMappingLu()
        {
            try
            {
                TaxCodeMappingLU taxCodeMappingLU = _masterModuleApplicationService.GetAllTaxCodeMappingLu(AuthInformation.companyId.Value);
                return Ok(taxCodeMappingLU);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        //[AllowAnonymous]
        [Route("createtaxcodemapping")]
        [Cache(Position = 10, ClientTimeSpan = 0, ServerTimeSpan = 6000, IsCompanyIdRequired = true, IsUsernameRequired = true)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult CreateTaxCodeMapping()
        {
            try
            {
                TaxCodeMappingModel model = _masterModuleApplicationService.CreateTaxCodeMapping(AuthInformation.companyId.Value);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        //[AllowAnonymous]
        [Route("SaveTaxCodeMapping")]
        [RolePermission(Position = 3)]
        [CommonHeaders(Position = 1)]
        public IHttpActionResult SaveTaxCodeMapping(TaxCodeMappingModel taxCodeMappingModel)
        {
            try
            {
                taxCodeMappingModel.CompanyId = AuthInformation.companyId.Value;
                TaxCodeMappingModel taxCodeMappingModels = _masterModuleApplicationService.SaveTaxCodeMapping(taxCodeMappingModel);
                return Ok(taxCodeMappingModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }

        #endregion InterCo


        [HttpGet]
        //[AllowAnonymous]
        [Route("getlinkedAccountsK")]
        [CommonHeaders(Position = 1)]
        public DataSourceResult GetLinkedAccounts(HttpRequestMessage requestMessage)
        {
            try
            {
                long? companyid = AuthInformation.companyId.Value;
                DataSourceRequest request =
              JsonConvert.DeserializeObject<DataSourceRequest>(requestMessage.RequestUri.ParseQueryString().GetKey(0));
                return _masterModuleApplicationService.GetLinkedAccountsK(companyid, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection,AuthInformation.userName).ToDataSourceResult(request.Take, request.Skip, request.Sort, request.Filter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



























        [HttpPost]
        [AllowAnonymous]
        [Route("savebeanentitymodelduplicate")]
        public IHttpActionResult SaveBeanEntityModelDuplicate(BeanEntityModel beanEntity)
        {
            try
            {
                //beanEntity.CompanyId = AuthInformation.companyId.Value;
                //string _name = HttpContext.Current.User.Identity.Name;
                BeanEntity BeanEntity = _masterModuleApplicationService.SaveEntityModel(beanEntity, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(BeanEntity);
            }
            catch (Exception ex)
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(beanEntity));
                Ziraff.FrameWork.Logging.LoggingHelper.LogError("MasterModuleController", ex, ex.Message, AdditionalInfo);
                return BadRequest(ex.Message);
                throw;
            }
        }
        #region Bean_Exchangerate_Insertion_by_from_and_to_currency
        [HttpGet]
        [Route("SubmitExchangeRate")]
        public IHttpActionResult ExchangeRateInformation(string FromCurrency, string ToCurrency)
        {
            try
            {
                string forex = _masterModuleApplicationService.SubmitExchangeRate(FromCurrency, ToCurrency, Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
                return Ok(forex);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

    }
}

