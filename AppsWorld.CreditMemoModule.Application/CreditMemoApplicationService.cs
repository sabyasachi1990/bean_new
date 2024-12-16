using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CreditMemoModule.Service;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Models;
using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.CreditMemoModule.Models;
using Logger;
using Serilog;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CreditMemoModule.Infra;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Repository.Pattern.Infrastructure;
using System.Data.Entity.Validation;
using AppsWorld.CreditMemoModule.RepositoryPattern;
using Ziraff.Section;
using System.Configuration;
using Domain.Events;
using System.Data.SqlClient;
using System.Data;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using AppaWorld.Bean;
using System.Net;
using Newtonsoft.Json;
using AppsWorld.CommonModule.Application;

namespace AppsWorld.CreditMemoModule.Application
{
    public class CreditMemoApplicationService
    {
        private readonly ICreditMemoService _creditMemoService;
        private readonly ICreditMemoDetailService _creditMemoDetailService;
        private readonly ICreditMemoApplicationService _creditMemoApplicationService;
        private readonly ICreditMemoApplicationDetailService _creditMemoApplicationDetailService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly AppsWorld.CreditMemoModule.Service.IJournalService _journalService;
        //private readonly IForexService _forexService;
        //private readonly IGSTSettingService _gstSettingService;
        private readonly IBeanEntityService _beanEntityService;
        //private readonly ISegmentMasterService _segmentMasterService;
        private readonly ChartOfAccountService _chartOfAccountService;
        private readonly TaxCodeService _taxCodeService;
        //private readonly IControlCodeCategoryService _controlCodeCategoryService;
        //private readonly IControlCodeService _controlCodeService;
        private readonly ICurrencyService _currencyService;
        private readonly ICompanyService _companyService;
        private readonly ITermsOfPaymentService _termsOfPaymentService;
        private readonly ICreditMemoModuleUnitOfWorkAsync _unitOfWork;
        private readonly AppsWorld.CreditMemoModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly AppsWorld.CreditMemoModule.Service.IAutoNumberService _autoNumberService;
        private readonly IBillService _billService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly AppsWorld.CreditMemoModule.Service.IJournalDetailService _journalDetailService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        private readonly CommonApplicationService _commonApplicationService;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string query = string.Empty;
        //private string ConnectionString { get; set; }
        public CreditMemoApplicationService(ICreditMemoService creditMemoServices, ICreditMemoDetailService creditMemoDetailServices, ICreditMemoApplicationService creditMemoApplicationServices, ICreditMemoApplicationDetailService creditMemoApplicationDetailServices, IFinancialSettingService financialSettingService, AppsWorld.CreditMemoModule.Service.IJournalService journalService, /*IForexService forexService, IGSTSettingService gstSettingService,*/ IBeanEntityService beanEntityService, /*ISegmentMasterService segmentMasterService,*/ ChartOfAccountService chartOfAccountService, TaxCodeService taxCodeService, /*IControlCodeCategoryService controlCodeCategoryService, IControlCodeService controlCodeService,*/ ICurrencyService currencyService, ICompanyService companyService, ITermsOfPaymentService termsOfPaymentService, ICreditMemoModuleUnitOfWorkAsync unitOfWork, IBillService billService, IAccountTypeService accountTypeService, AppsWorld.CreditMemoModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.CreditMemoModule.Service.IAutoNumberService autoNumberService, AppsWorld.CreditMemoModule.Service.IJournalDetailService journalDetailService, AppsWorld.CommonModule.Service.IAutoNumberService autoService, CommonApplicationService commonApplicationService)
        {
            this._creditMemoService = creditMemoServices;
            this._creditMemoDetailService = creditMemoDetailServices;
            this._creditMemoApplicationService = creditMemoApplicationServices;
            this._creditMemoApplicationDetailService = creditMemoApplicationDetailServices;
            this._financialSettingService = financialSettingService;
            this._journalService = journalService;
            //this._forexService = forexService;
            //this._gstSettingService = gstSettingService;
            this._beanEntityService = beanEntityService;
            //this._segmentMasterService = segmentMasterService;
            this._chartOfAccountService = chartOfAccountService;
            this._taxCodeService = taxCodeService;
            //this._controlCodeCategoryService = controlCodeCategoryService;
            //this._controlCodeService = controlCodeService;
            this._currencyService = currencyService;
            this._termsOfPaymentService = termsOfPaymentService;
            this._companyService = companyService;
            this._unitOfWork = unitOfWork;
            this._billService = billService;
            this._accountTypeService = accountTypeService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._autoNumberService = autoNumberService;
            this._journalDetailService = journalDetailService;
            this._autoService = autoService;
            this._commonApplicationService = commonApplicationService;

        }

        #region CreditMemo Create and Lookup Call
        public CreditMemoModel CreateCreditMemo(long companyId, Guid id, string connectionString, string username)
        {
            CreditMemoModel memoDTO = new CreditMemoModel();
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemo_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CreditMemoValidations.The_Financial_setting_should_be_activated);
                }
                memoDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                memoDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                //AppsWorld.CreditMemoModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.BillCreditMemo);
                CreditMemo document = _creditMemoService.GetCreditMemoByCompanyId(companyId, id);
                if (document == null)
                {
                    var creditMemoApp = _creditMemoApplicationService.GetCreditMemo(id);
                    if (creditMemoApp != null)
                        document = _creditMemoService.GetCreditMemoByCompanyId(companyId, creditMemoApp.CreditMemoId);
                }
                List<CreditMemoDetailModel> lstCreditMemo = new List<CreditMemoDetailModel>();
                //memoDTO.IsSegmentActive1 = true;
                //memoDTO.IsSegmentActive2 = true;
                if (document != null)
                {
                    if (!_companyService.GetPermissionBasedOnUser(document.ServiceCompanyId, document.CompanyId, username))
                        throw new Exception(CommonConstant.Access_denied);
                    FillCreditMemo(memoDTO, document);
                    memoDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.BillCreditMemo);
                    //if (document.DocumentState != CreditMemoState.Void)
                    //{
                    //    AppsWorld.CreditMemoModule.Entities.Journal journal = _journalService.GetJournal(companyId, document.Id);
                    //    if (journal != null)
                    //        memoDTO.JournalId = journal.Id;
                    //}
                    #region Commented_code
                    //var lstTaxCodes = _taxCodeService.GetTaxCodes(companyId);
                    //var account = _accountTypeService.GetById(companyId, AccountNameConstants.Cash_and_bank_balances);
                    //List<ChartOfAccount> lstCoa = _chartOfAccountService.GetCashAndBankCOAId(companyId, account.Id);
                    //memoDTO.CreditMemoDetailModels = (from memoDetail in document.CreditMemoDetails
                    //                                  join coa in lstCoa on memoDetail.COAId equals coa.Id
                    //                                  join taxCode in lstTaxCodes on memoDetail.TaxId equals taxCode.Id
                    //                                  select new CreditMemoDetailModel
                    //                                  {
                    //                                      Id = memoDetail.Id,
                    //                                      CreditMemoId = memoDetail.CreditMemoId,
                    //                                      TaxId = memoDetail.TaxId,
                    //                                      TaxRate = taxCode.TaxRate,
                    //                                      TaxType = taxCode.TaxType,
                    //                                      TaxIdCode = taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") + "(" + taxCode.TaxType[0] + ")" : taxCode.Code,
                    //                                      TaxCode = taxCode.Code,
                    //                                      COAId = memoDetail.COAId,
                    //                                      AccountName = coa.Name,
                    //                                      AmtCurrency = memoDetail.AmtCurrency,
                    //                                      Remarks = memoDetail.Remarks,
                    //                                      Discount = memoDetail.Discount,
                    //                                      AllowDisAllow = memoDetail.AllowDisAllow,
                    //                                      BaseAmount = memoDetail.BaseAmount,
                    //                                      BaseTaxAmount = memoDetail.BaseTaxAmount,
                    //                                      BaseTotalAmount = memoDetail.BaseTotalAmount,
                    //                                      DiscountType = memoDetail.DiscountType,
                    //                                      Qty = memoDetail.Qty,
                    //                                      Unit = memoDetail.Unit,
                    //                                      UnitPrice = memoDetail.UnitPrice
                    //                                  }).ToList();
                    //foreach (var memoDetail in document.CreditMemoDetails)
                    //{
                    //    CreditMemoDetailModel model = new CreditMemoDetailModel();
                    // FillCreditMemoDetailModel(model, memoDetail);
                    //    lstCreditMemo.Add(model);
                    //}
                    #endregion
                    string name = _beanEntityService.GetEntityName(memoDTO.CompanyId, memoDTO.EntityId);
                    string DocNo = _commonApplicationService.StringCharactersReplaceFunction(document.DocNo);
                    //document.DocNo.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
                    //.Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
                    string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                    //   name.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
                    //.Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
                    memoDTO.Path = DocumentConstants.Entities + "/" + EntityName + "/" + document.DocType + "s" + "/" + DocNo;
                    lstCreditMemo = (from detail in document.CreditMemoDetails
                                     select new CreditMemoDetailModel()

                                     {
                                         Id = detail.Id,
                                         CreditMemoId = detail.CreditMemoId,
                                         TaxId = detail.TaxId,
                                         COAId = detail.COAId,
                                         TaxIdCode = detail.TaxIdCode,
                                         DocAmount = detail.DocAmount,
                                         DocTotalAmount = detail.DocTotalAmount,
                                         DocTaxAmount = detail.DocTaxAmount,
                                         Description = detail.Description,
                                         AllowDisAllow = detail.AllowDisAllow,
                                         BaseAmount = detail.BaseAmount,
                                         BaseTaxAmount = detail.BaseTaxAmount,
                                         BaseTotalAmount = detail.BaseTotalAmount,
                                         RecOrder = detail.RecOrder,
                                         IsPLAccount = detail.IsPLAccount,
                                         TaxRate = detail.TaxRate,
                                         ClearingState = detail.ClearingState,
                                     }).ToList();
                    memoDTO.CreditMemoDetailModels = lstCreditMemo.OrderBy(c => c.RecOrder).ToList();
                }
                else
                {
                    CreditMemo lastMemo = _creditMemoService.GetCreditMemoByCompanyId(companyId);

                    memoDTO.Id = Guid.NewGuid();
                    memoDTO.CompanyId = companyId;
                    memoDTO.DocDate = lastMemo == null ? DateTime.Now : lastMemo.DocDate;
                    //memoDTO.DocNo = GetNewCreditMemoDocNo(DocTypeConstants.BillCreditMemo, companyId);

                    bool? isEdit = false;
                    //memoDTO.DocNo = GetAutoNumberByEntityType(companyId, lastMemo, DocTypeConstants.BillCreditMemo, _autoNo, ref isEdit);
                    memoDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.BillCreditMemo);
                    if (memoDTO.IsDocNoEditable == true)
                        memoDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.BillCreditMemo, connectionString);


                    memoDTO.DocumentState = CreditNoteState.NotApplied;
                    memoDTO.PostingDate = lastMemo == null ? DateTime.Now : lastMemo.PostingDate;
                    //memoDTO.DueDate = DateTime.UtcNow;
                    memoDTO.BaseCurrency = financSettings.BaseCurrency;
                    memoDTO.DocCurrency = memoDTO.BaseCurrency;
                    memoDTO.ExtensionType = ExtensionType.General;
                    memoDTO.CreatedDate = DateTime.UtcNow;
                    //Forex forexBean;
                    //forexBean = _forexService.GetMultiCurrencyInformation(memoDTO.BaseCurrency, memoDTO.DocDate, true, memoDTO.CompanyId);
                    //if (forexBean != null)
                    //{
                    //    memoDTO.ExchangeRate = forexBean.UnitPerUSD;
                    //    memoDTO.ExDurationFrom = forexBean.DateFrom;
                    //    memoDTO.ExDurationTo = forexBean.Dateto;
                    //}
                    memoDTO.IsGstSettings = false;
                    memoDTO.IsBaseCurrencyRateChanged = false;
                    memoDTO.IsGSTCurrencyRateChanged = false;
                    memoDTO.ModifiedBy = memoDTO.ModifiedBy;
                }
                //memoDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                //memoDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                //memoDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                //memoDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemo_CreateCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw;
            }

            return memoDTO;
        }

        public CreditMemoLU GetAllCreditNoteLU(string userName, long companyId, Guid creditId, string connectionString, DateTime? docdate = null)
        {
            CreditMemoLU creditLU = new CreditMemoLU();
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_GetAllCreditMemoLUs_LookupCall_Request_Message);
                CreditMemo lastMemo = _creditMemoService.GetByCompanyId(companyId);
                CreditMemo creditMemo = _creditMemoService.GetMemoLU(companyId, creditId);
                //DateTime date = creditMemo == null ? lastMemo == null ? DateTime.Now : lastMemo.DocDate : creditMemo.DocDate;
                DateTime date = creditMemo == null ? lastMemo == null ? DateTime.UtcNow : lastMemo.DocDate : creditMemo.DocDate;
                date = docdate != null ? (DateTime)docdate : date;
                //List<CreditMemoDetail> lstCreditDetails = _creditMemoDetailService.GetCreditMemoDetailById(creditId);
                Guid invoiceGuid = creditId;
                creditLU.CompanyId = companyId;
                creditLU.NatureLU = new List<string> { "Trade", "Others" };
                #region Bill to CreditMemo Direct call modification for Date
                string dbQuery = null;
                if (creditMemo != null && creditMemo.ExtensionType != null)
                {
                    dbQuery = DBQuery(creditMemo.Id, creditMemo.CompanyId, creditMemo.ExtensionType, creditMemo.DocType);
                    using (con = new SqlConnection(connectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand(dbQuery, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                            while (dr.Read())
                                date = dr["DocDate"] != DBNull.Value ? Convert.ToDateTime(dr["DocDate"]) : date;
                        //date = Convert.ToDateTime(dr["DocDate"]);
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
                #endregion Invoice to CreditNote Direct call modification for Date
                //creditLU.AllowableNonallowableLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId,
                //    ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                //creditLU.DocumentTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId,
                //    ControlCodeConstants.Control_codes_DocumentType);
                if (creditMemo != null)
                {
                    string currencyCode = creditMemo.DocCurrency;
                    creditLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, currencyCode,
                        ControlCodeConstants.Currency_DefaultCode);
                    //var lookUpNature = _controlCodeCategoryService.GetInactiveControlcode(companyId,
                    //      ControlCodeConstants.Control_Codes_Nature, creditMemo.Nature);
                    //if (lookUpNature != null)
                    //{
                    //    creditLU.NatureLU.Lookups.Add(lookUpNature);
                    //}
                }
                else
                {
                    creditLU.CurrencyLU = _currencyService.GetByCurrencies(companyId,
                        ControlCodeConstants.Currency_DefaultCode);
                }
                //List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
                //if (creditMemo == null)
                //{
                //    if (segments.Count > 0)
                //        creditLU.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        creditLU.SegmentCategory2LU = segments[1];
                //    // invoiceLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyid);
                //}
                //else
                //{
                //    if (creditMemo.SegmentMasterid1 != null)
                //        creditLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyId, creditMemo.SegmentMasterid1);
                //    else
                //        if (segments.Count > 0)
                //        creditLU.SegmentCategory1LU = segments[0];
                //    if (creditMemo.SegmentMasterid2 != null)
                //        creditLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyId, creditMemo.SegmentMasterid2);
                //    else
                //        if (segments.Count > 1)
                //        creditLU.SegmentCategory2LU = segments[1];
                //}
                long? credit = creditMemo == null ? 0 : creditMemo.CreditTermsId == null ? 0 : creditMemo.CreditTermsId;
                creditLU.TermsOfPaymentLU =
                    _termsOfPaymentService.Queryable().Where(a => (a.Status == RecordStatusEnum.Active || a.Id == credit) && a.CompanyId == companyId && a.IsVendor == true)
                        .Select(x => new LookUp<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            TOPValue = x.TOPValue,
                            RecOrder = x.RecOrder
                        }).OrderBy(a => a.TOPValue).ToList();
                long comp = creditMemo == null ? 0 : creditMemo.CompanyId;
                creditLU.SubsideryCompanyLU = _companyService.GetSubCompany(userName, companyId, comp);

                //if (lstCreditDetails.Count > 0)
                //{
                //    creditLU.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, true);
                //    creditLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyId);
                //}
                //else
                //{
                //    creditLU.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, false);
                //    creditLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, false, companyId);
                //}
                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<AccountType> accType = null;
                List<string> coaName = new List<string> { /*COANameConstants.Revenue,*/ COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                //List<long> accType = _accountTypeService.GetAllAccounyTypeByNameByID(companyId, coaName);
                //List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountType(accType);
                //creditLU.ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
                //{
                //    Name = x.Name,
                //    Id = x.Id,
                //    RecOrder = x.RecOrder,
                //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                //    Class = x.Class
                //}).ToList();

                //if it is a Opening Balance Credit Memo
                //if (creditMemo != null && creditMemo.DocSubType == DocTypeConstants.OpeningBalance && creditMemo.ExtensionType == "OBCM")
                //{
                //    //if (creditMemo.DocSubType == DocTypeConstants.OpeningBalance && creditMemo.ExtensionType == "OBCM")
                //    //{
                //    coaName = new List<string> { COANameConstants.System };
                //    accType = _accountTypeService.GetAllAccounyTypeByName(companyId, coaName);
                //    //}
                //}
                //else
                accType = _accountTypeService.GetAllAccountTypeNameByCompanyId(companyId, coaName);
                List<COALookup<string>> lstCoas = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                })).OrderBy(d => d.Name).ToList();
                creditLU.ChartOfAccountLU = lstCoas.OrderBy(s => s.Name).ToList();
                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    creditLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        IsTaxAmountEditable = (x.TaxRate == 0 || x.TaxRate == null) ? false : true,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        IsApplicable = x.IsApplicable,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (creditMemo != null && creditMemo.CreditMemoDetails.Count > 0)
                {
                    List<long> CoaIds = creditMemo.CreditMemoDetails.Select(c => c.COAId).ToList();
                    if (creditLU.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(creditLU.ChartOfAccountLU.Select(x => x.Id)).ToList();

                    List<long?> taxIds = creditMemo.CreditMemoDetails.Select(x => x.TaxId).ToList();
                    if (creditLU.TaxCodeLU != null && creditLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(creditLU.TaxCodeLU.Select(d => d.Id)).ToList();
                    if (CoaIds.Any())
                    {
                        //lstEditCoa = _chartOfAccountService.GetAllCOAById(companyid, CoaIds).Select(x => new COALookup<string>()
                        //{
                        //    Name = x.Name,
                        //    Id = x.Id,
                        //    RecOrder = x.RecOrder,
                        //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                        //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                        //    Class = x.Class
                        //}).ToList();
                        lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        }).ToList().OrderBy(d => d.Name)).ToList();
                        creditLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    }

                    //for OB COA
                    if (creditMemo != null && creditMemo.DocSubType == DocTypeConstants.OpeningBalance && creditMemo.ExtensionType == "OBCM")
                    {
                        var chartOfAccount = _chartOfAccountService.GetChartOfAccountByName("Opening balance", companyId);
                        if (chartOfAccount != null)
                        {
                            List<COALookup<string>> lstOBCoa = new List<COALookup<string>>() { new COALookup<string>() { Name=chartOfAccount.Name,Code=chartOfAccount.Code,Id=chartOfAccount.Id,RecOrder=chartOfAccount.RecOrder,IsAllowDisAllow=chartOfAccount.DisAllowable==true?true:false,IsPLAccount=chartOfAccount.Category=="Income Statement"?true:false,Class=chartOfAccount.Class,Status=chartOfAccount.Status
                                } }.ToList();
                            creditLU.ChartOfAccountLU.AddRange(lstOBCoa);
                        }
                    }

                    if (creditMemo.IsGstSettings && taxIds.Any())
                    {
                        lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            IsTaxAmountEditable = (x.TaxRate == 0 || x.TaxRate == null) ? false : true,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            IsApplicable = x.IsApplicable,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                        }).OrderBy(c => c.Code).ToList();
                        creditLU.TaxCodeLU.AddRange(lstEditTax);
                        var data = creditLU.TaxCodeLU;
                        creditLU.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                    }
                    // List<long> CoaIds = creditMemo.CreditMemoDetails.Select(c => c.COAId).ToList();
                    // List<long> taxIds = creditMemo.CreditMemoDetails.Select(x => x.TaxId.Value).ToList();



                    ////creditLU.ChartOfAccountLU.Where(c => c.Id == CoaIds.Contains());
                    // if (CoaIds.Any())
                    // {
                    //     List<long> lstcoaId = creditLU.ChartOfAccountLU.Select(c => c.Id).ToList().Intersect(CoaIds.ToList()).ToList();
                    //     var coalst=  lstcoaId.Except(creditLU.ChartOfAccountLU.Select(x=>x.Id));
                    //     if (coalst.Any())
                    //     {
                    //         lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => coalst.Contains(x.Id)).Select(x => new COALookup<string>()
                    //         {
                    //             Name = x.Name,
                    //             Id = x.Id,
                    //             RecOrder = x.RecOrder,
                    //             IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    //             IsPLAccount = x.Category == "Income Statement" ? true : false,
                    //             Class = c.Class
                    //         }).ToList()).ToList();
                    //         creditLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    //     }
                    // }


                    // //var common = creditLU.ChartOfAccountLU.Intersect(lstEditCoa.Select(x=>x.Id));


                    // if (taxIds.Any())
                    // {
                    //     List<long> lsttaxId = creditLU.TaxCodeLU.Select(d => d.Id).ToList().Intersect(taxIds.ToList()).ToList();
                    //     var taxlst = lsttaxId.Except(creditLU.TaxCodeLU.Select(x => x.Id));
                    //     if (taxlst.Any())
                    //     {
                    //         lstEditTax = allTaxCodes.Where(c => taxlst.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                    //         {
                    //             Id = x.Id,
                    //             Code = x.Code,
                    //             Name = x.Name,
                    //             TaxRate = x.TaxRate,
                    //             TaxType = x.TaxType,
                    //             Status = x.Status,
                    //             TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                    //         }).OrderBy(c => c.Code).ToList();
                    //         creditLU.TaxCodeLU.AddRange(lstEditTax);
                    //     }
                    // }
                }
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_GetAllCreditMemoLUs_LookupCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
            }
            return creditLU;
        }
        #endregion

        #region CreditMemo Kendo Call
        public IQueryable<CreditMemoModelK> GetAllCreditMemoK(string username, long companyId)
        {
            return _creditMemoService.GetAllCreditMemoK(username, companyId);
        }
        #endregion

        #region Create CreditMemoApplication
        public CreditMemoApplicationModel CreateCreditMemoApplication(Guid creditMemoId, Guid cmApplicationId, long companyId, bool isView, DateTime applicationDate, string connectionString, string username)
        {
            CreditMemoApplicationModel CMAModel = new CreditMemoApplicationModel();

            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemoApplication_CreateCall_Request_Message);

                //to check if it is void or not
                //if (_creditMemoService.IsVoid(companyId, creditMemoId))
                //    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CreditMemoValidations.The_Financial_setting_should_be_activated);
                }
                CMAModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                CMAModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                CreditMemo creditMemo = _creditMemoService.GetCreditMemoByCompanyId(companyId, creditMemoId);

                if (creditMemo == null)
                    throw new Exception(CreditMemoValidations.Invalid_CreditMemo);

                CreditMemoApplication CMApplication = _creditMemoApplicationService.GetAllCreditMemo(creditMemoId, cmApplicationId, companyId);
                if (CMApplication != null)
                {
                    FillCreditMemoApplicationModel(CMAModel, CMApplication);
                    //AppsWorld.CreditMemoModule.Entities.Journal journal = _journalService.GetJournals(cmApplicationId, companyId);
                    //if (journal != null)
                    //{
                    //    CMAModel.JournalId = journal.Id;
                    //    CMAModel.DocSubType = journal.DocSubType;
                    //}
                    CMAModel.DocSubType = DocTypeConstants.Application;
                    CMAModel.ExchangeRate = creditMemo.ExchangeRate;
                    CMAModel.CreatedDate = CMApplication.CreatedDate;
                    CMAModel.DocDate = creditMemo.PostingDate;
                    CMAModel.Remarks = CMApplication.Remarks != null || CMApplication.Remarks != string.Empty ? CMApplication.Remarks : creditMemo.DocDescription;
                    CMAModel.EntityName = _beanEntityService.GetEntityName(creditMemo.EntityId);
                    //CMAModel.Remarks = creditMemo.DocDescription;

                    List<CreditMemoApplicationDetailModel> lstDetailModel = new List<CreditMemoApplicationDetailModel>();
                    List<CreditMemoApplicationDetail> lstCNAD = _creditMemoApplicationDetailService.GetAllCreditMemoDetail(cmApplicationId);
                    List<Bill> lstBills = _billService.GetAllBills(lstCNAD.Where(c => c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim).Select(x => x.DocumentId).ToList(), companyId);
                    CreditMemo memo = _creditMemoService.GetMemoByDocId(companyId, lstCNAD.Where(c => c.DocumentType == DocTypeConstants.Receipt || c.DocumentType == DocTypeConstants.BillPayment).Select(x => x.DocumentId).FirstOrDefault());

                    #region Payment/Receipt Hiper_Link
                    Guid? receiptId = Guid.Empty;
                    Guid? paymentId = Guid.Empty;
                    long? paymentComp = null;
                    long? receiptComp = null;
                    string paymentDocState = null;
                    string receiptDocState = null;
                    string receiptServCompName = null;
                    string paymentServCompName = null;
                    if (CMApplication.IsRevExcess != true && CMApplication.DocumentId != null && (CMApplication.CreditMemoApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.BillPayment).Any() || CMApplication.CreditMemoApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.Receipt).Any()))
                    {
                        using (con = new SqlConnection(connectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            if (CMApplication.CreditMemoApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.BillPayment).Any())
                                query = "Select PD.PaymentId as 'PaymentId',P.ServiceCompanyId as 'ServiceCompanyId', P.DocumentState as DocState, C.ShortName as ShortName from Bean.Payment P JOIN Bean.PaymentDetail PD on P.Id = PD.PaymentId JOIN Common.Company C on C.ParentId = P.CompanyId AND C.ID = P.ServiceCompanyID where PD.Id ='" + CMApplication.DocumentId + "'";
                            else if (CMApplication.CreditMemoApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.Receipt).Any())
                                query = "Select RD.ReceiptId as 'ReceiptId',R.ServiceCompanyId as 'ServiceCompanyId', R.DocumentState as DocState, C.ShortName as ShortName from Bean.Receipt R JOIN Bean.ReceiptDetail RD on R.Id = RD.ReceiptId JOIN Common.Company C on C.ParentId = R.CompanyId AND C.ID = R.ServiceCompanyID where RD.Id ='" + CMApplication.DocumentId + "'";
                            cmd = new SqlCommand(query, con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    if (CMApplication.CreditMemoApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.BillPayment).Any())
                                    {
                                        paymentId = dr["PaymentId"] != DBNull.Value ? Guid.Parse(dr["PaymentId"].ToString()) : Guid.Empty;
                                        paymentComp = dr["ServiceCompanyId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceCompanyId"]) : Convert.ToInt64(dr["ServiceCompanyId"]);
                                        paymentDocState = dr["DocState"] != DBNull.Value ? dr["DocState"].ToString() : null;
                                        paymentServCompName = dr["ShortName"] != DBNull.Value ? dr["ShortName"].ToString() : null;
                                    }
                                    else if (CMApplication.CreditMemoApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.Receipt).Any())
                                    {
                                        receiptId = dr["ReceiptId"] != DBNull.Value ? Guid.Parse(dr["ReceiptId"].ToString()) : Guid.Empty;
                                        receiptComp = dr["ServiceCompanyId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceCompanyId"]) : Convert.ToInt64(dr["ServiceCompanyId"]);
                                        receiptDocState = dr["DocState"] != DBNull.Value ? dr["DocState"].ToString() : null;
                                        receiptServCompName = dr["ShortName"] != DBNull.Value ? dr["ShortName"].ToString() : null;
                                    }
                                }
                            }
                        }
                    }

                    #endregion Payment/Receipt Hiper_Link
                    if (CMApplication.Status == CreditMemoApplicationStatus.Void || _creditMemoService.IsVoid(companyId, creditMemoId))
                    {
                        Dictionary<long, string> servCompanies = null;
                        if (lstBills != null)
                            servCompanies = _companyService.GetAllCompaniesName(lstBills.Select(x => x.ServiceCompanyId).ToList());
                        foreach (CreditMemoApplicationDetail CMADetail in lstCNAD)
                        {
                            CreditMemoApplicationDetailModel model = new CreditMemoApplicationDetailModel();
                            model.BaseCurrencyExchangeRate = Convert.ToDecimal(CMADetail.BaseCurrencyExchangeRate);
                            model.COAId = CMADetail.COAId;
                            model.CreditAmount = CMADetail.CreditAmount;
                            model.CreditMemoApplicationId = CMADetail.CreditMemoApplicationId;
                            model.DocCurrency = CMADetail.DocCurrency;
                            model.DocNo = CMADetail.DocNo;
                            model.DocType = CMADetail.DocumentType;
                            if (CMADetail.DocumentId != null)
                                model.DocumentId = (Guid)(CMADetail.DocumentId);
                            model.Id = CMADetail.Id;
                            model.SystemReferenceNumber = CMADetail.DocNo;

                            if (CMADetail.DocumentType == DocTypeConstants.General || CMADetail.DocumentType == DocTypeConstants.Bills || CMADetail.DocumentType == DocTypeConstants.OpeningBalance || CMADetail.DocumentType == DocTypeConstants.Claim)
                            {
                                Bill bill = lstBills.Any() ? lstBills.Where(x => x.Id == CMADetail.DocumentId).FirstOrDefault() : null;
                                if (bill != null)
                                {
                                    model.DocAmount = bill.GrandTotal;
                                    model.DocDate = bill.DocumentDate;
                                    model.DocumentId = bill.Id;
                                    model.DocNo = bill.DocNo;
                                    model.SystemReferenceNumber = bill.SystemReferenceNumber;
                                    if (CMApplication.Status == CreditMemoApplicationStatus.Void)
                                        model.BalanceAmount = bill.BalanceAmount;
                                    else
                                        model.BalanceAmount = bill.BalanceAmount + CMADetail.CreditAmount;
                                    model.DocType = bill.DocType;
                                    model.Nature = bill.Nature;
                                    model.BaseCurrencyExchangeRate = bill.ExchangeRate.Value;
                                    model.ServiceEntityId = bill.ServiceCompanyId;
                                    model.ServCompanyName = servCompanies.Any() ? servCompanies.Where(a => a.Key == bill.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                                    model.DocState = bill.DocumentState;
                                    model.IsHyperLinkEnable = true;
                                }
                            }
                            else
                            {
                                if (memo != null)
                                {
                                    model.DocAmount = memo.GrandTotal;
                                    model.DocDate = (CMADetail.DocumentType == DocTypeConstants.Receipt || CMADetail.DocumentType == DocTypeConstants.BillPayment) ? CMApplication.CreditMemoApplicationDate : memo.DocDate;
                                    model.DocumentId = CMADetail.DocumentType == DocTypeConstants.Receipt ? (receiptId != Guid.Empty ? receiptId.Value : Guid.Empty) : (paymentId != Guid.Empty ? paymentId.Value : Guid.Empty);
                                    model.DocNo = CMADetail.DocNo;
                                    if (CMApplication.Status == CreditMemoApplicationStatus.Void)
                                        model.BalanceAmount = memo.BalanceAmount;
                                    else
                                        model.BalanceAmount = memo.BalanceAmount + CMADetail.CreditAmount;
                                    model.DocType = CMADetail.DocumentType;
                                    model.Nature = memo.Nature;

                                    //detailModel.SegmentCategory1 = bill.SegmentCategory1;
                                    //detailModel.SegmentCategory2 = bill.SegmentCategory2;
                                    model.BaseCurrencyExchangeRate = memo.ExchangeRate.Value;
                                    model.IsHyperLinkEnable = CMADetail.DocumentType == DocTypeConstants.Receipt ? _companyService.GetServiceCompanyStatusByUsername((Int64)(receiptComp), companyId, username) : CMADetail.DocumentType == DocTypeConstants.BillPayment ? _companyService.GetServiceCompanyStatusByUsername((Int64)(paymentComp), companyId, username) : true;
                                    model.ServiceEntityId = CMADetail.DocumentType == DocTypeConstants.Receipt ? (Int64)(receiptComp).Value : CMADetail.DocumentType == DocTypeConstants.BillPayment ? (Int64)(paymentComp).Value : memo.ServiceCompanyId;
                                    model.DocState = CMADetail.DocumentType == DocTypeConstants.Receipt ? receiptDocState : CMADetail.DocumentType == DocTypeConstants.BillPayment ? paymentDocState : memo.DocumentState;
                                    model.ServCompanyName = CMADetail.DocumentType == DocTypeConstants.Receipt ? receiptServCompName : CMADetail.DocumentType == DocTypeConstants.BillPayment ? paymentServCompName : null;
                                }
                            }
                            if (CMApplication.IsRevExcess == true)
                                CMAModel.ReverseExcessModels = CMApplication.CreditMemoApplicationDetails.Select(a => new ReverseExcessModel()
                                {
                                    Id = a.Id,
                                    CompanyId = companyId,
                                    DocAmount = a.CreditAmount,
                                    DocTaxAmount = a.TaxAmount,
                                    DocTotalAmount = a.TotalAmount,
                                    TaxId = a.TaxId,
                                    TaxRate = a.TaxRate,
                                    TaxIdCode = a.TaxIdCode,
                                    RecOrder = a.RecOrder,
                                    COAId = a.COAId,
                                    Description = a.DocDescription
                                }).OrderBy(c => c.RecOrder).ToList();
                            lstDetailModel.Add(model);
                        }
                        CMAModel.CreditMemoApplicationDetailModels = lstDetailModel;
                    }
                    else
                    {
                        if (CMApplication.IsRevExcess != true)
                        {
                            foreach (CreditMemoApplicationDetail detail in lstCNAD)
                            {
                                CreditMemoApplicationDetailModel detailModel = new CreditMemoApplicationDetailModel();
                                FillCreditMemoAppDetail(detailModel, detail, CMAModel, CMApplication, lstBills, memo, receiptId, paymentId, receiptComp, paymentComp, username, companyId, receiptDocState, paymentDocState, receiptServCompName, paymentServCompName);
                                lstDetailModel.Add(detailModel);
                            }
                        }
                        if (isView != true)
                        {
                            List<Bill> lstBill = _billService.GetAllCreditMemoById(companyId, creditMemo.EntityId, creditMemo.DocCurrency, creditMemo.ServiceCompanyId.Value, applicationDate);
                            //var result = (from detail in lstBill
                            //              from d in lstDetailModel
                            //              where (d == null) && (d.DocumentId == detail.Id)
                            //              select new CreditMemoApplicationDetailModel()
                            //              {
                            //                  DocNo = detail.DocNo,
                            //                  DocType = detail.DocSubType,
                            //                  DocumentId = detail.Id,
                            //                  DocDate = detail.DocumentDate,
                            //                  DocAmount = detail.GrandTotal,
                            //                  DocCurrency = detail.DocCurrency,
                            //                  BalanceAmount = detail.BalanceAmount,
                            //                  Nature = detail.Nature,
                            //                  SystemReferenceNumber = detail.SystemReferenceNumber,
                            //                  BaseCurrencyExchangeRate = detail.ExchangeRate.Value
                            //              }).ToList();
                            //CMAModel.CreditMemoApplicationDetailModels = result;
                            foreach (Bill detail in lstBill)
                            {
                                var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                if (d == null)
                                {
                                    CreditMemoApplicationDetailModel detailModel = new CreditMemoApplicationDetailModel();
                                    detailModel.DocNo = detail.DocNo;
                                    detailModel.DocType = detail.DocType;
                                    detailModel.DocumentId = detail.Id;
                                    detailModel.DocDate = detail.DocumentDate;
                                    detailModel.DocAmount = detail.GrandTotal;
                                    detailModel.DocCurrency = detail.DocCurrency;
                                    detailModel.BalanceAmount = detail.BalanceAmount;
                                    detailModel.Nature = detail.Nature;
                                    detailModel.PostingDate = detail.PostingDate;
                                    detailModel.SystemReferenceNumber = detail.SystemReferenceNumber;
                                    detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                                    detailModel.ServiceEntityId = detail.ServiceCompanyId;
                                    detailModel.DocState = detail.DocumentState;
                                    detailModel.IsHyperLinkEnable = true;
                                    lstDetailModel.Add(detailModel);
                                }
                            }
                            //CMAModel.CreditMemoApplicationDetailModels = lstDetailModel.OrderBy(c => c.DocDate).ThenBy(d => d.SystemReferenceNumber).ToList();
                        }
                        //if (CMAModel.IsRevExcess != true)
                        CMAModel.CreditMemoApplicationDetailModels = lstDetailModel.OrderBy(c => c.DocDate).ThenBy(d => d.SystemReferenceNumber).ToList();
                        //else
                        //{
                        if (CMApplication.IsRevExcess == true)
                            CMAModel.ReverseExcessModels = CMApplication.CreditMemoApplicationDetails.Select(a => new ReverseExcessModel()
                            {
                                Id = a.Id,
                                CompanyId = companyId,
                                DocAmount = a.CreditAmount,
                                DocTaxAmount = a.TaxAmount,
                                DocTotalAmount = a.TotalAmount,
                                TaxId = a.TaxId,
                                TaxRate = a.TaxRate,
                                TaxIdCode = a.TaxIdCode,
                                RecOrder = a.RecOrder,
                                COAId = a.COAId,
                                Description = a.DocDescription
                            }).OrderBy(c => c.RecOrder).ToList();
                        //}
                    }
                }
                else
                {
                    // CreditMemoApplication CNA = _creditNoteApplicationService.GetCreditNoteByCompanyId(companyId);
                    FillCreditMemoAppNew(CMAModel, companyId, creditMemo);
                    CMAModel.EntityName = _beanEntityService.GetEntityName(creditMemo.EntityId);
                    CMAModel.ExchangeRate = creditMemo.ExchangeRate;
                    List<Bill> lstBill = _billService.GetAllCreditMemoById(companyId, creditMemo.EntityId, creditMemo.DocCurrency, creditMemo.ServiceCompanyId.Value, applicationDate);
                    if (lstBill.Count > 0 || lstBill.Any())
                    {
                        List<CreditMemoApplicationDetailModel> lstDetailModel = new List<CreditMemoApplicationDetailModel>();
                        lstDetailModel = (from detail in lstBill
                                          select new CreditMemoApplicationDetailModel()
                                          {
                                              DocNo = detail.DocNo,
                                              DocType = detail.DocType,
                                              DocumentId = detail.Id,
                                              DocDate = detail.DocumentDate,
                                              DocAmount = detail.GrandTotal,
                                              DocCurrency = detail.DocCurrency,
                                              BalanceAmount = detail.BalanceAmount,
                                              Nature = detail.Nature,
                                              PostingDate = detail.PostingDate,
                                              SystemReferenceNumber = detail.SystemReferenceNumber,
                                              BaseCurrencyExchangeRate = detail.ExchangeRate.Value,
                                              ServiceEntityId = detail.ServiceCompanyId,
                                              DocState = detail.DocumentState,
                                              IsHyperLinkEnable = true
                                          }).ToList();
                        CMAModel.CreditMemoApplicationDetailModels = lstDetailModel.OrderBy(x => x.DocDate).ThenBy(d => d.SystemReferenceNumber).ToList();
                    }

                    //foreach (Bill detail in lstBill)
                    //{
                    //    CreditMemoApplicationDetailModel detailModel = new CreditMemoApplicationDetailModel();
                    //    detailModel.DocNo = detail.DocNo;
                    //    detailModel.DocType = detail.DocSubType;
                    //    detailModel.DocumentId = detail.Id;
                    //    detailModel.DocDate = detail.DocumentDate;
                    //    detailModel.DocAmount = detail.GrandTotal;
                    //    detailModel.DocCurrency = detail.DocCurrency;
                    //    detailModel.BalanceAmount = detail.BalanceAmount;
                    //    detailModel.Nature = detail.Nature;
                    //    detailModel.SystemReferenceNumber = detail.SystemReferenceNumber;
                    //    detailModel.SegmentCategory1 = detail.SegmentCategory1;
                    //    detailModel.SegmentCategory2 = detail.SegmentCategory2;
                    //    detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;

                    //    CMAModel.CreditMemoApplicationDetailModels.Add(detailModel);
                    //}
                }
                //CMAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                //CMAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                //CMAModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                //CMAModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemoApplication_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, ex.StackTrace);
                throw;
            }

            return CMAModel;
        }
        #endregion
        #region CreditmemoapplicationLu
        public CreditmemoApplicationLu creditmemoApplicationLu(long companyId, Guid CMAId)
        {
            CreditmemoApplicationLu CMALU = new CreditmemoApplicationLu();
            try
            {
                CreditMemoApplication CMApplication = _creditMemoApplicationService.GetAllCreditMemoApplication(CMAId, companyId);
                List<TaxCodeLookUp<string>> lstTaxes = new List<TaxCodeLookUp<string>>();
                if (CMApplication.IsRevExcess == true)
                {
                    List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, CMApplication.CreditMemoApplicationDate);
                    if (allTaxCodes.Any())
                        CMALU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            IsTaxAmountEditable = (x.TaxRate == 0 || x.TaxRate == null) ? false : true,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            IsApplicable = x.IsApplicable,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.Code
                        }).OrderBy(c => c.Code).ToList();
                    if (CMApplication != null && CMApplication.CreditMemoApplicationDetails.Count > 0)
                    {
                        List<long?> taxIds = CMApplication.CreditMemoApplicationDetails.Select(x => x.TaxId).ToList();
                        if (CMALU.TaxCodeLU != null && CMALU.TaxCodeLU.Any())
                            taxIds = taxIds.Except(CMALU.TaxCodeLU.Select(d => d.Id)).ToList();
                        if (taxIds.Any())
                        {
                            List<TaxCode> taxCodes = _taxCodeService.GetTaxAllCodesByIds(taxIds);
                            if (taxCodes.Any())
                            {
                                lstTaxes = taxCodes.Select(x => new TaxCodeLookUp<string>()
                                {
                                    Id = x.Id,
                                    Code = x.Code,
                                    Name = x.Name,
                                    TaxRate = x.TaxRate,
                                    IsTaxAmountEditable = (x.TaxRate == 0 || x.TaxRate == null) ? false : true,
                                    TaxType = x.TaxType,
                                    Status = x.Status,
                                    IsApplicable = x.IsApplicable,
                                    TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.Code
                                }).OrderBy(c => c.Code).ToList();
                                CMALU.TaxCodeLU.AddRange(lstTaxes);
                            }
                        }
                    }
                }
                return CMALU;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create CreditMemoDocVoid Call
        //public DocumentVoidModel CreateCreditMemoDocumentVoid(Guid id, long companyId)
        //{
        //    DocumentVoidModel DVModel = new DocumentVoidModel();
        //    try
        //    {
        //        LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService,CreditMemoLoggingValidation.Log_CreateCreditNoteDocumentVoid_CreateCall_Request_Message);
        //        FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
        //        if (financSettings == null)
        //            throw new Exception(CreditMemoValidations.Credit_amount_should_be_less_than_or_equal_to_Balance_Amount);

        //        CreditMemo creditMemo = _creditMemoService.GetCreditMemoByCompanyId(companyId, id);
        //        if (creditMemo == null)
        //            throw new Exception(CreditMemoValidations.Invalid_CreditMemo);
        //        DVModel.CompanyId = companyId;
        //        DVModel.Id = (creditMemo == null) ? Guid.NewGuid() : id;
        //        LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService,CreditMemoLoggingValidation.Log_CreateCreditMemoDocumentVoid_CreateCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService,CreditMemoLoggingValidation.Log_CreateCreditMemoDocumentVoid_CreateCall_SuccessFully_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }

        //    return DVModel;
        //}
        #endregion

        #region SaveCreditMemo
        public CreditMemo SaveCreditMemo(CreditMemoModel TObject, string ConnectionString)
        {
            bool isAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, "ObjectSave", AdditionalInfo);
                string _errors = CommonValidation.ValidateObject(TObject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }

                if (TObject.EntityId == null)
                {
                    throw new InvalidOperationException(CreditMemoValidations.Entity_is_mandatory);
                }
                //to check if it is void or not
                if (_creditMemoService.IsVoid(TObject.CompanyId, TObject.Id))
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                if (TObject.DocDate == null)
                {
                    throw new InvalidOperationException(CreditMemoValidations.Invalid_Document_Date);
                }

                if (TObject.DueDate == null || TObject.DueDate < TObject.DocDate)
                {
                    throw new InvalidOperationException(CreditMemoValidations.Invalid_Due_Date);
                }

                if (TObject.SaveType == "InDirect" && TObject.BalanceAmount > 0 && TObject.CreditMemoApplicationModel.CreditAmount > 0 && ((TObject.BalanceAmount -= TObject.CreditMemoApplicationModel.CreditAmount) < 0))
                {
                    if (TObject.ExtensionType == DocTypeConstants.Bills || TObject.ExtensionType == DocTypeConstants.OBBill) throw new InvalidOperationException("Credit Memo amount shouldn't be greater than outstanding balance of Bill ");
                }

                #region 2_Tab_Validation
                if (TObject.CreditMemoApplicationModel != null)
                {
                    List<decimal?> lstInvoiceData = _billService.GetBillStatusByIds(TObject.CreditMemoApplicationModel.CreditMemoApplicationDetailModels.Where(s => s.DocType == DocTypeConstants.Bill).Select(d => d.DocumentId).ToList());
                    if (lstInvoiceData.Any())
                        if (TObject.CreditMemoApplicationModel.CreditMemoApplicationDetailModels.Where(d => !lstInvoiceData.Contains(d.BalanceAmount)).Any())
                            throw new Exception(CreditMemoValidations.CM_application_status_Change);
                }

                #endregion 2_Tab_Validation

                if (TObject.SaveType == "InDirect")
                {
                    TObject.BalanceAmount = TObject.GrandTotal;
                    if (TObject.GrandTotal <= 0)
                        throw new InvalidOperationException(CreditMemoValidations.Grand_Total_Should_Be_Grater_Than_Zero);
                    if (TObject.GrandTotal < TObject.BalanceAmount)
                        throw new InvalidOperationException(CreditMemoValidations.Grand_Total_Should_Be_Greater_Than_Or_Equal_To_Balance_Amount);
                    if (TObject.CreditMemoApplicationModel.CreditAmount > TObject.CreditMemoApplicationModel.CreditMemoBalanceAmount)
                        throw new InvalidOperationException(CreditMemoValidations.Credit_Note_Amount_should_be_less_than_or_equal_to_Remaining_Amount);
                    Guid? billId = TObject.CreditMemoApplicationModel.CreditMemoApplicationDetailModels.Select(c => c.DocumentId).FirstOrDefault();
                    if (billId != null)
                    {
                        Bill bill = _billService.GetCrediMemoByDocId(billId.Value);
                        if (bill != null && Math.Round(TObject.CreditMemoDetailModels.Sum(c => c.DocAmount), 2) > bill.BillDetails.Sum(c => c.DocAmount))
                        {
                            throw new InvalidOperationException(CreditMemoValidations.credit_memo_amount);
                        }
                    }
                }

                if (TObject.CreditTermsId == null && TObject.CreditTermsId == 0)
                {
                    throw new InvalidOperationException(CreditMemoValidations.Terms_of_Payment_is_mandatory);
                }
                if (TObject.IsDocNoEditable == true && _creditMemoService.IsDocumentNumberExists(TObject.Id, DocTypeConstants.BillCreditMemo, TObject.DocNo, TObject.CompanyId, TObject.EntityId))
                {
                    throw new InvalidOperationException(CreditMemoValidations.Document_number_already_exist);
                }

                if (TObject.ServiceCompanyId == null)
                    throw new InvalidOperationException(CreditMemoValidations.Service_Company_Is_Mandatory);

                if (TObject.GrandTotal <= 0)
                {
                    throw new InvalidOperationException(CreditMemoValidations.Grand_Total_should_be_greater_than_zero);
                }

                if (TObject.ExchangeRate == 0)
                    throw new InvalidOperationException(CreditMemoValidations.ExchangeRate_Should_Be_Grater_Than_Zero);

                if (TObject.GSTExchangeRate == 0)
                    throw new InvalidOperationException(CreditMemoValidations.GSTExchangeRate_Should_Be_Grater_Than_Zero);
                //Need to verify the invoice is within Financial year
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.PostingDate, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CreditMemoValidations.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.PostingDate, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(CreditMemoValidations.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.PostingDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new InvalidOperationException(CreditMemoValidations.Invalid_Financial_Period_Lock_Password);
                    }
                }
                CreditMemo _document = null;
                if (TObject.Nature == DocTypeConstants.Interco)
                {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        query = $"Update Bean.CreditMemo set DocDescription='{TObject.DocDescription}',ModifiedBy='{TObject.ModifiedBy}',ModifiedDate=GETUTCDATE() where Id='{TObject.Id}' and CompanyId={TObject.CompanyId}";
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("Bean_Insert_Document_History", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId.ToString());
                        cmd.Parameters.AddWithValue("@DocumentId", TObject.Id.ToString());
                        cmd.Parameters.AddWithValue("@DocumentType", DocTypeConstants.BillCreditMemo);
                        cmd.Parameters.AddWithValue("@IsVoid", false);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    _document = _creditMemoService.GetCreditMemoById(TObject.Id);
                }
                else
                {
                    _document = _creditMemoService.GetCreditMemoById(TObject.Id);
                    string oldDocumentNo = string.Empty;
                    if (_document != null)
                    {
                        oldDocumentNo = _document.DocNo;
                        if (_document.ExchangeRate != TObject.ExchangeRate)
                            _document.RoundingAmount = 0;
                        //Data Concurrency verify
                        string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                        if (!timeStamp.Equals(TObject.Version))
                            throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                        LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemo_SaveCall_UpdateRequest_Message);
                        InsertCreditMemo(TObject, _document, false);
                        _document.DocNo = TObject.DocNo;
                        _document.CreditMemoNumber = _document.DocNo;
                        _document.ModifiedBy = TObject.ModifiedBy;
                        _document.ModifiedDate = DateTime.UtcNow;
                        _document.ObjectState = ObjectState.Modified;

                        UpdateCreditMemoDetails(TObject, _document);
                        _document.BaseGrandTotal = Math.Round(_document.CreditMemoDetails.Sum(a => (decimal)a.BaseTotalAmount), 2, MidpointRounding.AwayFromZero);
                        _document.BaseBalanceAmount = _document.BaseGrandTotal;

                        _creditMemoService.Update(_document);
                        TObject.EventStatus = "update";
                    }
                    else
                    {
                        isAdd = true;
                        LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemo_SaveCall_NewRequest_Message);
                        _document = new CreditMemo();
                        InsertCreditMemo(TObject, _document, true);
                        _document.Id = TObject.Id;
                        _document.DocumentState = String.IsNullOrEmpty(TObject.DocumentState) ? CreditNoteState.NotApplied : TObject.DocumentState;
                        int? recorder = 0;
                        if (TObject.CreditMemoDetailModels.Count > 0 || TObject.CreditMemoDetailModels != null)
                        {
                            foreach (CreditMemoDetailModel detail in TObject.CreditMemoDetailModels)
                            {
                                if (detail.RecordStatus != "Deleted")
                                {
                                    CreditMemoDetail memoDetail = new CreditMemoDetail();
                                    FillMemoDetail(memoDetail, detail, TObject.ExchangeRate);
                                    memoDetail.RecOrder = ++recorder;
                                    memoDetail.Id = Guid.NewGuid();
                                    memoDetail.CreditMemoId = _document.Id;
                                    memoDetail.ObjectState = ObjectState.Added;
                                    _creditMemoDetailService.Insert(memoDetail);
                                    _document.CreditMemoDetails.Add(memoDetail);
                                }
                            }
                        }
                        _document.BaseGrandTotal = Math.Round(_document.CreditMemoDetails.Sum(a => (decimal)a.BaseTotalAmount), 2, MidpointRounding.AwayFromZero);
                        _document.BaseBalanceAmount = _document.BaseGrandTotal;
                        _document.Status = RecordStatusEnum.Active;
                        _document.UserCreated = TObject.UserCreated;
                        _document.CreatedDate = DateTime.UtcNow;
                        _document.ObjectState = ObjectState.Added;
                        _document.CreditMemoNumber = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.BillCreditMemo, ConnectionString) : TObject.DocNo;
                        isDocAdd = true;
                        _document.DocNo = _document.CreditMemoNumber;
                        _creditMemoService.Insert(_document);
                        TObject.EventStatus = "insert";

                    }
                    try
                    {
                        _unitOfWork.SaveChanges();

                        #region New_Posting_Through_SP

                        using (con = new SqlConnection(ConnectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd = new SqlCommand("Bean_Posting", con);
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@SourceId", _document.Id);
                            cmd.Parameters.AddWithValue("@Type", DocTypeConstants.BillCreditMemo);
                            cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }

                        #endregion New_Posting_Through_SP

                        if (TObject.SaveType == "InDirect")
                        {
                            TObject.CreditMemoApplicationModel.PeriodLockPassword = TObject.PeriodLockPassword;
                            SaveCreditMemoApplication(TObject.CreditMemoApplicationModel, ConnectionString);
                        }
                        #region DocumentAttachment_Save_Call
                        if (isAdd == true)
                        {
                            if (TObject.TileAttachments != null && TObject.TileAttachments.Count() > 0)
                            {
                                string name = _beanEntityService.GetEntityName(TObject.CompanyId, TObject.EntityId);
                                string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                                string DocuNo = _commonApplicationService.StringCharactersReplaceFunction(_document.DocNo);
                                string path = DocumentConstants.Entities + "/" + EntityName + "/" + _document.DocType + "s" + "/" + DocuNo;
                                _document.Path = DocumentConstants.Entities + "/" + EntityName + "/" + _document.DocType + "s" + "/" + DocuNo;
                                SaveTailsAttachments(TObject.CompanyId, path, TObject.UserCreated, TObject.TileAttachments);
                            }
                        }
                        #endregion

                        #region Document Folder Rename

                        if (isAdd == false && oldDocumentNo != TObject.DocNo)
                        {
                            string name = _beanEntityService.GetEntityName(TObject.CompanyId, TObject.EntityId);
                            string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                            _commonApplicationService.ChangeFolderName(TObject.CompanyId, TObject.DocNo, oldDocumentNo, EntityName, "Credit Memos");
                        }

                        #endregion
                    }
                    catch (DbEntityValidationException e)
                    {

                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine(
                                "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, e, e.Message);
                        Log.Logger.ZCritical(e.StackTrace);
                        throw e;
                    }
                }
                return _document;
            }
            catch (Exception ex)
            {
                if (isAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocType, ConnectionString);
                }
                throw ex;
            }
        }
        #endregion

        #region CreditMemoDetail Empty Call
        public CreditMemoDetailModel GetCreditMemoDetail()
        {
            CreditMemoDetailModel memoDetail = new CreditMemoDetailModel();
            return memoDetail;
        }
        #endregion

        #region CreateCreditMemoApplication Reset
        public DocumentResetModel CreateCreditMemoApplicationReset(Guid id, Guid creditMemoId, long companyId)
        {
            DocumentResetModel DDAModel = new DocumentResetModel();
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemoApplicationReset_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CreditMemoValidations.The_Financial_setting_should_be_activated);
                }

                CreditMemo creditMemo = _creditMemoService.GetCreditMemoByCompanyId(companyId, creditMemoId);
                if (creditMemo == null)
                {
                    throw new Exception(CreditMemoValidations.Invalid_CreditMemo);
                }
                CreditMemoApplication CMApplication = _creditMemoApplicationService.GetAllCreditMemo(creditMemoId, id, companyId);
                if (CMApplication != null)
                {
                    DDAModel.CompanyId = companyId;
                    DDAModel.Id = id;
                    DDAModel.CreditMemoId = creditMemoId;
                    DDAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                    DDAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                    DDAModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                    DDAModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                }
                else
                {
                    throw new Exception(CreditMemoValidations.Invalid_Credit_Memo_Application);
                }
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemoApplicationReset_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return DDAModel;
        }
        #endregion

        #region SaveCreditMemoApplication Reset
        public CreditMemoApplication SaveCreditMemoApplicationReset(DocumentResetModel TObject, string ConnectionString)
        {
            SqlConnection con;
            LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemoApplicationReset_CreateCall_Request_Message);
            //Need to verify the invoice is within Financial year
            //if (!_financialSettingService.ValidateYearEndLockDate(TObject.ResetDate.Value, TObject.CompanyId))
            //{
            //    throw new Exception(CreditMemoValidations.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            //}

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            //if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.ResetDate.Value, TObject.CompanyId))
            //{
            //    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
            //    {
            //        throw new Exception(CreditMemoValidations.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
            //    }
            //    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.ResetDate.Value, TObject.PeriodLockPassword, TObject.CompanyId))
            //    {
            //        throw new Exception(CreditMemoValidations.Invalid_Financial_Period_Lock_Password);
            //    }
            //}
            List<DocumentHistoryModel> lstOfDocumentHistoryModel = new List<DocumentHistoryModel>();
            Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
            CreditMemo creditMemo = _creditMemoService.GetCreditMemoByCompanyId(TObject.CompanyId, TObject.CreditMemoId);

            if (creditMemo == null)
            {
                throw new Exception(CreditMemoValidations.Invalid_CreditMemo);
            }
            CreditMemoApplication allocation = _creditMemoApplicationService.GetAllCreditMemo(TObject.CreditMemoId, TObject.Id, TObject.CompanyId);

            if (allocation != null)
            {
                //if (allocation.CreditMemoApplicationDate.Date > TObject.ResetDate.Value.Date)
                //    throw new Exception(CreditMemoValidations.Reset_Date_should_be_greater_than_Allocation_date);

                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(allocation.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                if (allocation.ClearCount != null && allocation.ClearCount > 0)
                    throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

                allocation.Status = CreditMemoApplicationStatus.Void;
                allocation.CreditMemoApplicationResetDate = TObject.ResetDate;
                allocation.CreditMemoApplicationNumber = allocation.CreditMemoApplicationNumber + "-V";
                allocation.ObjectState = ObjectState.Modified;
                allocation.ModifiedDate = DateTime.UtcNow;
                allocation.ModifiedBy = TObject.ModifiedBy;
                _creditMemoApplicationService.Update(allocation);
                //var updateJournal = _journalService.GetJournal(TObject.CompanyId, TObject.Id);
                //if (updateJournal != null)
                //{
                //    updateJournal.Status = CreditMemoApplicationStatus.Reset;
                //    updateJournal.DocumentState = "Reset";
                //    _journalService.Update(updateJournal);
                //}


                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(creditMemo.Id, creditMemo.CompanyId, allocation.Id, creditMemo.DocType, DocTypeConstants.Application, "Void", creditMemo.DocCurrency, allocation.CreditAmount, allocation.CreditAmount, creditMemo.ExchangeRate.Value, TObject.ModifiedBy != null ? TObject.ModifiedBy : creditMemo.UserCreated, creditMemo.Remarks, allocation.CreditMemoApplicationResetDate, 0, 0);

                    if (lstdocumet.Any())
                        lstOfDocumentHistoryModel.AddRange(lstdocumet);
                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                }
                catch (Exception ex)
                {
                }

                //Getting the bill details by giving the 

                List<Bill> lstOfBills = _billService.GetAllBills(allocation.CreditMemoApplicationDetails.Select(a => a.DocumentId).ToList(), allocation.CompanyId);
                decimal? roundAmount = 0;
                foreach (CreditMemoApplicationDetail detail in allocation.CreditMemoApplicationDetails)
                    if (detail.DocumentId != null)
                        UpdateDocumentState(detail.DocumentId.Value, detail.DocumentType, -detail.CreditAmount, ConnectionString, allocation.Id, allocation.CreditMemoApplicationResetDate, lstOfDocumentHistoryModel, true, 0, lstOfRoundingAmount, false, 0, 0, detail.RoundingAmount, lstOfBills, out roundAmount);

                creditMemo.BalanceAmount += allocation.CreditAmount;

                //if (creditMemo.BalanceAmount == 0)
                //    creditMemo.DocumentState = CreditNoteState.FullyApplied;
                //else
                //    creditMemo.DocumentState = CreditNoteState.PartialApplied;


                if (creditMemo.BalanceAmount == 0)
                    creditMemo.DocumentState = CreditNoteState.FullyApplied;
                else if (creditMemo.BalanceAmount > 0)
                {
                    creditMemo.DocumentState = CreditNoteState.PartialApplied;
                    //roundingAmount = Math.Round((creditMemo.GrandTotal - allocation.CreditAmount) * (creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((allocation.CreditAmount * (creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)creditMemo.BaseGrandTotal;
                    creditMemo.BaseBalanceAmount += (Math.Round((allocation.CreditAmount * (creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                }

                else
                {
                    throw new Exception(String.Format("CreditMemo ({0}) Balance Amount is becoming negative", creditMemo.CreditMemoNumber));
                }
                if (creditMemo.GrandTotal == creditMemo.BalanceAmount)
                {
                    creditMemo.DocumentState = CreditNoteState.NotApplied;
                    creditMemo.BaseBalanceAmount = creditMemo.BaseGrandTotal;
                }
                creditMemo.RoundingAmount = (allocation.RoundingAmount != null && allocation.RoundingAmount != 0) ? allocation.RoundingAmount : 0;
                creditMemo.ModifiedBy = "System";
                creditMemo.ObjectState = ObjectState.Modified;
                //if (creditMemo != null)
                //{
                //    var journalByMemoId = _journalService.GetJournal(TObject.CompanyId, TObject.CreditMemoId);
                //    if (journalByMemoId != null)
                //    {
                //        journalByMemoId.BalanceAmount = creditMemo.BalanceAmount;
                //        journalByMemoId.DocumentState = creditMemo.DocumentState;
                //        journalByMemoId.ObjectState = ObjectState.Modified;
                //        _journalService.Update(journalByMemoId);
                //        JournalDetail jDetail = _journalDetailService.GetDetailByJournalId(journalByMemoId.Id);
                //        if (jDetail != null)
                //        {
                //            jDetail.AmountDue = journalByMemoId.BalanceAmount;
                //            jDetail.ObjectState = ObjectState.Modified;
                //            _journalDetailService.Update(jDetail);
                //        }
                //    }
                //}
                //Modified by Pradhan
                if (creditMemo.DocSubType == DocTypeConstants.OpeningBalance)
                {
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                    oBcmd.CommandTimeout = 30;
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", creditMemo.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", creditMemo.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", creditMemo.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", creditMemo.BalanceAmount == creditMemo.GrandTotal ? true : false);
                    int res = oBcmd.ExecuteNonQuery();
                    con.Close();
                }
                #region Update_Journal_Detail_Clearing_Status
                if (creditMemo.DocSubType != DocTypeConstants.OpeningBalance)
                {
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyId", TObject.CompanyId);
                    cmd.Parameters.AddWithValue("@documentId", TObject.CreditMemoId);
                    cmd.Parameters.AddWithValue("@docState", creditMemo.DocumentState);
                    cmd.Parameters.AddWithValue("@balanceAmount", creditMemo.BalanceAmount);
                    int count = cmd.ExecuteNonQuery();
                    con.Close();
                }

                #endregion Update_Journal_Detail_Clearing_Status
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(allocation.Id, creditMemo.CompanyId, creditMemo.Id, creditMemo.DocType, creditMemo.DocSubType, creditMemo.DocumentState, creditMemo.DocCurrency, creditMemo.GrandTotal, creditMemo.BalanceAmount, creditMemo.ExchangeRate.Value, creditMemo.ModifiedBy != null ? creditMemo.ModifiedBy : creditMemo.UserCreated, creditMemo.Remarks, allocation.CreditMemoApplicationResetDate, 0, 0);

                    if (lstdocumet.Any())
                    {
                        lstOfDocumentHistoryModel.AddRange(lstdocumet);
                        AppaWorld.Bean.Common.SaveDocumentHistory(lstOfDocumentHistoryModel, ConnectionString);
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion Documentary History
            }
            else
            {
                throw new Exception(CreditMemoValidations.Invalid_Credit_Memo_Application);
            }
            try
            {
                _unitOfWork.SaveChanges();
                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = allocation.CreditMemoApplicationNumber;
                tObject.ModifiedBy = TObject.ModifiedBy;
                DeleteJVPostMemo(tObject);
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_CreateCreditMemoApplicationReset_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return allocation;
        }
        #endregion

        #region Save CreditMemoApplication
        public CreditMemoApplication SaveCreditMemoApplication(CreditMemoApplicationModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, "ObjectSave", AdditionalInfo);
            string oldDocState = null;
            DateTime? oldAppDate = null;
            decimal oldCreditAmount = 0;
            CreditMemo creditMemo = _creditMemoService.GetMemo(TObject.CreditMemoId);
            ValidateCreditNoteApplication(creditMemo, TObject);
            if (TObject.CreditAmount > TObject.CreditMemoBalanceAmount)
                throw new Exception(CreditMemoValidations.Credit_Amount_should_be_less_than_or_equal_to_Remaining_Amount);
            List<DocumentHistoryModel> lstOfDocumentHistoryModel = new List<DocumentHistoryModel>();
            Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
            decimal roundingAmount = 0;
            CreditMemoApplication application = _creditMemoApplicationService.GetCreditMemoById(TObject.Id);
            bool isNew = false;
            if (application == null)
            {
                application = new CreditMemoApplication();
                isNew = true;
                oldAppDate = TObject.CreditMemoApplicationDate;
            }
            else
            {
                if (application.Status == CreditMemoApplicationStatus.Void)
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
                creditMemo.BalanceAmount += application.CreditAmount;
                oldAppDate = application.CreditMemoApplicationDate;
                oldCreditAmount = application.CreditAmount;
            }
            oldDocState = creditMemo.DocumentState;
            application.CreditMemoApplicationDate = TObject.CreditMemoApplicationDate;
            application.ExchangeRate = creditMemo.ExchangeRate;
            application.IsNoSupportingDocument = TObject.IsNoSupportingDocument.Value;
            if (application.IsNoSupportingDocument == true)
                application.IsNoSupportingDocumentActivated = TObject.NoSupportingDocument != null;
            application.CreditAmount = TObject.CreditAmount;
            application.Remarks = TObject.Remarks;
            application.Status = TObject.Status;
            application.IsRevExcess = TObject.IsRevExcess;
            application.DocumentId = TObject.DocumentId;

            if (isNew)
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemoApplication_SaveCall_NewRequest_Message);
                application.Id = TObject.Id;
                application.CreditMemoId = TObject.CreditMemoId;
                application.CompanyId = TObject.CompanyId;
                application.ExchangeRate = creditMemo.ExchangeRate;

                if (TObject.IsRevExcess == true)
                    UpdateCMApplicationReverseExcessDetails(TObject, application, true, lstOfDocumentHistoryModel);
                else
                    UpdateCreditMemoApplicationDetails(TObject, application, ConnectionString, application.CreditMemoApplicationDate, lstOfDocumentHistoryModel, lstOfRoundingAmount);
                application.CreditMemoApplicationNumber = GetNextApplicationNumber(TObject.CreditMemoId);
                application.UserCreated = TObject.UserCreated;
                application.CreatedDate = DateTime.UtcNow;
                application.CreditMemoApplicationDetails = application.CreditMemoApplicationDetails.Where(c => c.CreditAmount != 0).ToList();
                application.ObjectState = ObjectState.Added;
                _creditMemoApplicationService.Insert(application);
                //JVModel jvm = new JVModel();
                ////application.CreditMemoApplicationDetails = application.CreditMemoApplicationDetails.Where(c => c.CreditAmount != 0).ToList();
                //FillCreditMemoJournal(jvm, application, true);
                //SaveInvoice1(jvm);
                //isNew = false;
            }
            else
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemoApplication_SaveCall_UpdateRequest_Message);

                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(application.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
                if (TObject.IsRevExcess == true)
                    UpdateCMApplicationReverseExcessDetails(TObject, application, false, lstOfDocumentHistoryModel);
                else
                    UpdateCreditMemoApplicationDetails(TObject, application, ConnectionString, application.CreditMemoApplicationDate, lstOfDocumentHistoryModel, lstOfRoundingAmount);
                //UpdateCreditMemoApplicationDetails(TObject, application, ConnectionString);

                application.ModifiedBy = TObject.ModifiedBy;
                application.ModifiedDate = DateTime.UtcNow;

                application.ObjectState = ObjectState.Modified;
                _creditMemoApplicationService.Update(application);
                //_unitOfWork.SaveChanges();
                //VModel jvm = new JVModel();
                //FillCreditMemoJournal(jvm, application, false);
                //SaveInvoice1(jvm);
            }
            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(creditMemo.Id, creditMemo.CompanyId, application.Id, creditMemo.DocType, DocTypeConstants.Application, "Posted", creditMemo.DocCurrency, application.CreditAmount, application.CreditAmount, creditMemo.ExchangeRate.Value, TObject.ModifiedBy != null ? TObject.ModifiedBy : application.UserCreated, creditMemo.Remarks, application.CreditMemoApplicationDate, application.CreditAmount, 0);

                if (lstdocumet.Any())
                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                    lstOfDocumentHistoryModel.AddRange(lstdocumet);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            #endregion Documentary History
            creditMemo.BalanceAmount -= TObject.CreditAmount;
            if (creditMemo.BalanceAmount == 0)
            {
                creditMemo.DocumentState = CreditNoteState.FullyApplied;
                if (isNew)
                {
                    //roundingAmount = Math.Round(application.CreditAmount * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)creditMemo.BaseBalanceAmount;
                    //creditMemo.BaseBalanceAmount = 0;
                    //if (roundingAmount != 0)
                    //    lstOfRoundingAmount.Add(creditMemo.Id, roundingAmount);
                    if (creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0)
                        roundingAmount = ((creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0) ? (decimal)creditMemo.RoundingAmount : 0);
                    else
                        roundingAmount = Math.Round(application.CreditAmount * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)creditMemo.BaseBalanceAmount;

                    creditMemo.BaseBalanceAmount = 0;
                    if (roundingAmount != 0)
                        lstOfRoundingAmount.Add(creditMemo.Id, roundingAmount);
                    creditMemo.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0)) ? creditMemo.RoundingAmount - roundingAmount : 0;
                    application.RoundingAmount = roundingAmount;
                }
                else
                {
                    if (oldCreditAmount != application.CreditAmount)
                    {
                        //roundingAmount = (((decimal)creditMemo.BaseGrandTotal - (Math.Round(Math.Abs(oldCreditAmount - application.CreditAmount) * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) + (decimal)creditMemo.BaseBalanceAmount) - (decimal)creditMemo.BaseGrandTotal;
                        //creditMemo.BaseBalanceAmount = 0;
                        //if (roundingAmount != 0)
                        //    lstOfRoundingAmount.Add(creditMemo.Id, roundingAmount);
                        if (application.CreditAmount == creditMemo.GrandTotal)
                        {
                            roundingAmount = Math.Round(Math.Abs(application.CreditAmount) * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)creditMemo.BaseGrandTotal);
                            if (roundingAmount != 0)
                                lstOfRoundingAmount.Add(creditMemo.Id, roundingAmount);
                            application.RoundingAmount = roundingAmount;
                            creditMemo.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0)) ? creditMemo.RoundingAmount - roundingAmount : 0;
                            creditMemo.BaseBalanceAmount = 0;
                        }

                        else if (application.RoundingAmount != null && application.RoundingAmount != 0)
                        {
                            creditMemo.BaseBalanceAmount = 0;
                            lstOfRoundingAmount.Add(creditMemo.Id, application.RoundingAmount.Value);
                            //newRoundingAmount = detailRoundingAmount.Value;
                            roundingAmount = application.RoundingAmount.Value;
                        }
                        else
                        {
                            if (creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0)
                                roundingAmount = ((creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0) ? (decimal)creditMemo.RoundingAmount : 0);
                            else
                                roundingAmount = Math.Round(Math.Abs(application.CreditAmount - oldCreditAmount) * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)creditMemo.BaseBalanceAmount);

                            creditMemo.BaseBalanceAmount = 0;
                            if (roundingAmount != 0)
                                lstOfRoundingAmount.Add(creditMemo.Id, roundingAmount);
                            application.RoundingAmount = roundingAmount;
                            creditMemo.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditMemo.RoundingAmount != null && creditMemo.RoundingAmount != 0)) ? creditMemo.RoundingAmount - roundingAmount : 0;
                        }
                    }
                    else
                    {
                        if (application.RoundingAmount != null && application.RoundingAmount != 0)
                        {
                            creditMemo.BaseBalanceAmount = 0;
                            lstOfRoundingAmount.Add(creditMemo.Id, application.RoundingAmount.Value);
                            //newRoundingAmount = detailRoundingAmount.Value;
                            roundingAmount = application.RoundingAmount.Value;
                        }
                    }
                }
            }
            else
            {
                creditMemo.DocumentState = CreditNoteState.PartialApplied;
                if (isNew)
                    creditMemo.BaseBalanceAmount -= Math.Round(application.CreditAmount * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                else
                {
                    creditMemo.BaseBalanceAmount = oldCreditAmount > application.CreditAmount ? creditMemo.BaseBalanceAmount + (Math.Round(Math.Abs(oldCreditAmount - application.CreditAmount) * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : creditMemo.BaseBalanceAmount - (Math.Round(Math.Abs(oldCreditAmount - application.CreditAmount) * ((creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                    if (oldDocState == CreditNoteState.FullyApplied)
                    {
                        creditMemo.RoundingAmount = ((application.RoundingAmount != null && application.RoundingAmount != 0) && (oldDocState != creditMemo.DocumentState)) ? application.RoundingAmount : creditMemo.RoundingAmount;
                        application.RoundingAmount = ((application.RoundingAmount != null && application.RoundingAmount != 0) && (oldDocState != creditMemo.DocumentState)) ? 0 : application.RoundingAmount;
                    }
                }
            }



            if (creditMemo != null)
            {
                if (creditMemo.DocSubType == DocTypeConstants.OpeningBalance)
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                    oBcmd.CommandTimeout = 30;
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", creditMemo.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", creditMemo.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", creditMemo.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", creditMemo.BalanceAmount == creditMemo.GrandTotal ? true : false);
                    int res = oBcmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            creditMemo.ObjectState = ObjectState.Modified;
            creditMemo.ModifiedBy = CreditMemoConstant.System;
            creditMemo.ModifiedDate = DateTime.UtcNow;
            _creditMemoService.Update(creditMemo);
            var updateState = _journalService.GetJournal(TObject.CompanyId, TObject.CreditMemoId);
            if (updateState != null)
            {
                updateState.DocumentState = creditMemo.DocumentState;
                updateState.BalanceAmount = creditMemo.BalanceAmount;
                updateState.ObjectState = ObjectState.Modified;
                updateState.ModifiedBy = CreditMemoConstant.System;
                updateState.ModifiedDate = DateTime.UtcNow;
                _journalService.Update(updateState);
            }
            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, creditMemo.CompanyId, creditMemo.Id, creditMemo.DocType, creditMemo.DocSubType, creditMemo.DocumentState, creditMemo.DocCurrency, creditMemo.GrandTotal, creditMemo.BalanceAmount, creditMemo.ExchangeRate.Value, /*creditMemo.ModifiedBy != null ? creditMemo.ModifiedBy : creditMemo.UserCreated*/CreditMemoConstant.System, creditMemo.Remarks, application.CreditMemoApplicationDate, application.CreditAmount < 0 ? application.CreditAmount : -application.CreditAmount, roundingAmount);

                if (lstdocumet.Any())
                    lstOfDocumentHistoryModel.AddRange(lstdocumet);

                if (lstOfDocumentHistoryModel.Any())
                    AppaWorld.Bean.Common.SaveDocumentHistory(lstOfDocumentHistoryModel, ConnectionString);

                if (oldAppDate != TObject.CreditMemoApplicationDate)
                {
                    string query = $"Update Bean.DocumentHistory Set PostingDate='{String.Format("{0:MM/dd/yyyy}", application.CreditMemoApplicationDate)}' where TransactionId='{application.Id}' and CompanyId={application.CompanyId} and TransactionId<>DocumentId and doctype in ('Bill') and AgingState is null;";
                    SqlConnection con = null;
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                //throw ex;
            }
            #endregion Documentary History
            LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemoApplication_SaveCall_SuccessFully_Message);
            try
            {
                _unitOfWork.SaveChanges();
                //JVModel jvm = new JVModel();
                //FillCreditMemoJournal(jvm, application, isNew, TObject.IsOffset);
                //SaveInvoice1(jvm);
                AppaWorld.Bean.Common.SaveMultiplePosting(application.CompanyId, creditMemo.Id, application.Id, DocTypeConstants.BillCreditMemo, DocTypeConstants.Application, application.IsRevExcess == null ? false : application.IsRevExcess.Value, TObject.IsOffset == null ? false : TObject.IsOffset.Value, ConnectionString, string.Join(":", lstOfRoundingAmount));

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return application;
        }
        #endregion

        #region SaveCreditMemoDocVoid Call
        public CreditMemo SaveCreditMemoDocumentVoid(DocumentVoidModel TObject)
        {
            LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemoDocumentVoid_SaveCall_Request_Message);
            string DocNo = "-V";

            //string DocDescription = "Void-";
            CreditMemo _document = _creditMemoService.GetCreditMemoByCompanyId(TObject.CompanyId, TObject.Id);
            if (_document == null)
                throw new Exception(CreditMemoValidations.Invalid_CreditMemo);
            else
            {
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            if (_creditMemoService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
            if (_document.CreditMemoDetails.Any(s => s.ClearingState == CreditMemoState.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);
            if (_document.DocumentState != CreditNoteState.NotApplied)
                throw new Exception("State should be " + CreditNoteState.NotApplied);

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
            {
                throw new Exception(CreditMemoValidations.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CreditMemoValidations.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CreditMemoValidations.Invalid_Financial_Period_Lock_Password);
                }
            }

            if (_document.CreditMemoDetails.Any(s => s.ClearingState == CreditMemoState.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            if (_document.ClearCount != null && _document.ClearCount > 0)
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            _document.DocumentState = CreditNoteState.Void;
            _document.DocNo = _document.DocNo + DocNo;
            _document.ModifiedDate = DateTime.UtcNow;
            _document.ModifiedBy = _document.UserCreated;
            _document.ObjectState = ObjectState.Modified;
            LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_SaveCreditMemoDocumentVoid_SaveCall_SuccessFully_Message);
            try
            {
                _unitOfWork.SaveChanges();
                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = _document.DocNo;
                DeleteJVPostMemo(tObject);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return _document;
        }
        #endregion

        #region Private Method Block

        private void UpdateCMApplicationReverseExcessDetails(CreditMemoApplicationModel cmAppModel, CreditMemoApplication cmApplication, bool isAdd, List<DocumentHistoryModel> lstOfDocumentHistoryModel)
        {
            decimal roundingAmount = 0;
            //if Reverse Model Checked unchecked scenario
            if (isAdd == false)
            {
                List<Guid?> lstDocumentId = cmApplication.CreditMemoApplicationDetails.Where(a => a.DocumentId != null).Select(a => a.DocumentId).ToList();
                if (lstDocumentId.Any())
                {
                    List<AppsWorld.CreditMemoModule.Entities.Journal> lstJournal = _journalService.GetListOfJournalByDocId(lstDocumentId, cmAppModel.CompanyId);
                    List<Bill> lstBill = _billService.GetAllBills(lstDocumentId, cmAppModel.CompanyId);
                    if (lstBill.Any() && lstJournal.Any())
                    {
                        foreach (var bill in lstBill)
                        {
                            roundingAmount = 0;
                            CreditMemoApplicationDetail detail = cmApplication.CreditMemoApplicationDetails.Where(a => a.DocumentId == bill.Id).FirstOrDefault();
                            if (detail != null)
                            {
                                bill.BalanceAmount += detail.CreditAmount;
                                bill.DocumentState = bill.BalanceAmount == bill.GrandTotal ? CreditMemoConstant.NotPaid : bill.DocumentState;
                                bill.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                if (bill.DocumentState == CreditMemoConstant.NotPaid)
                                    bill.BaseBalanceAmount = bill.BaseGrandTotal;
                                else
                                {
                                    //roundingAmount = (Math.Round((bill.GrandTotal - detail.CreditAmount) * (bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.CreditAmount * (bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)bill.BaseGrandTotal;
                                    bill.BaseBalanceAmount += (Math.Round((detail.CreditAmount * (bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                }


                                bill.ObjectState = ObjectState.Modified;
                                _billService.Update(bill);

                                AppsWorld.CreditMemoModule.Entities.Journal journal = lstJournal.Where(a => a.CompanyId == cmAppModel.CompanyId && a.DocumentId == bill.Id).FirstOrDefault();
                                if (journal != null)
                                {
                                    journal.BalanceAmount = bill.BalanceAmount;
                                    journal.DocumentState = bill.DocumentState;
                                    journal.ObjectState = ObjectState.Modified;
                                    _journalService.Update(journal);
                                }
                            }
                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(cmApplication.Id, cmApplication.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, bill.Remarks, null, 0, 0);

                                if (lstdocumet.Any())
                                    lstOfDocumentHistoryModel.AddRange(lstdocumet);
                                //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                            #endregion Documentary History
                        }
                    }
                }
            }

            int? recOrder = 0;
            if (isAdd == false)
                foreach (CreditMemoApplicationDetail detailDelete in cmApplication.CreditMemoApplicationDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;

            foreach (ReverseExcessModel revDeatail in cmAppModel.ReverseExcessModels)
            {
                if (revDeatail.RecordStatus == "Added")
                {
                    CreditMemoApplicationDetail cmAppDetail = new CreditMemoApplicationDetail();
                    cmAppDetail.Id = Guid.NewGuid();
                    cmAppDetail.CreditMemoApplicationId = cmApplication.Id;
                    FillCMReverseApplication(cmAppDetail, revDeatail);
                    //FillCNApplicationReverseModel(cnApplication, detail, cnAppDetail);
                    cmAppDetail.RecOrder = ++recOrder;
                    cmAppDetail.DocCurrency = cmAppModel.DocCurrency;
                    cmAppDetail.ObjectState = ObjectState.Added;
                    _creditMemoApplicationDetailService.Insert(cmAppDetail);
                }
                else if (revDeatail.RecordStatus != "Added" && revDeatail.RecordStatus != "Deleted")
                {
                    CreditMemoApplicationDetail cmAppDetail = cmApplication.CreditMemoApplicationDetails.Where(c => c.Id == revDeatail.Id).FirstOrDefault();
                    if (cmAppDetail != null)
                    {
                        FillCMReverseApplication(cmAppDetail, revDeatail);
                        cmAppDetail.RecOrder = revDeatail.RecOrder;
                        cmAppDetail.DocCurrency = cmAppModel.DocCurrency;
                        cmAppDetail.ObjectState = ObjectState.Modified;
                        _creditMemoApplicationDetailService.Update(cmAppDetail);
                    }
                }
                else if (revDeatail.RecordStatus == "Deleted")
                {
                    CreditMemoApplicationDetail cmAppDetail = cmApplication.CreditMemoApplicationDetails.Where(c => c.Id == revDeatail.Id).FirstOrDefault();
                    if (cmAppDetail != null)
                        cmAppDetail.ObjectState = ObjectState.Deleted;
                }
            }
        }

        private void FillCMReverseApplication(CreditMemoApplicationDetail cmAppDetail, ReverseExcessModel reverseExcess)
        {
            cmAppDetail.DocDescription = reverseExcess.Description;
            cmAppDetail.CreditAmount = reverseExcess.DocAmount;
            cmAppDetail.TaxAmount = reverseExcess.DocTaxAmount;
            cmAppDetail.TotalAmount = reverseExcess.DocTotalAmount;
            cmAppDetail.TaxId = reverseExcess.TaxId;
            cmAppDetail.TaxRate = reverseExcess.TaxRate;
            cmAppDetail.TaxIdCode = reverseExcess.TaxIdCode;
            cmAppDetail.COAId = reverseExcess.COAId;
        }

        private string GetNewCreditMemoDocNo(string docType, long CompanyId)
        {
            CreditMemo creditMemo = _creditMemoService.GetAllMemoByDoctypeAndCompanyId(docType, CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (creditMemo != null)
            {
                string strOldNo = String.Empty;
                CreditMemo duplicatInvoice;
                int index;
                strOldDocNo = creditMemo.DocNo;

                for (int i = strOldDocNo.Length - 1; i >= 0; i--)
                {
                    if (Char.IsDigit(strOldDocNo[i]))
                        strOldNo = strOldDocNo[i] + strOldNo;
                    else
                        break;
                }
                long docNo = 0;
                try
                { docNo = long.Parse(strOldNo); }
                catch { }

                index = strOldDocNo.LastIndexOf(strOldNo);

                do
                {
                    docNo++;
                    strNewNo = docNo.ToString().PadLeft(strOldNo.Length, '0');
                    strNewDocNo = (docNo == 1) ? strOldDocNo + strNewNo : strOldDocNo.Substring(0, index) + strNewNo;

                    duplicatInvoice = _creditMemoService.GetMemos(strNewDocNo, docType, CompanyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }
        private void InsertCreditMemo(CreditMemoModel TObject, CreditMemo crediNoteNew, bool isNew)
        {
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_InsertCreditMemo_FillCall_Request_Message);
                crediNoteNew.CompanyId = TObject.CompanyId;
                crediNoteNew.DocType = DocTypeConstants.BillCreditMemo;
                crediNoteNew.DocSubType = DocTypeConstants.General;
                crediNoteNew.EntityType = "Vendor";
                crediNoteNew.DocDate = TObject.DocDate.Date;
                crediNoteNew.DueDate = TObject.DueDate.Value.Date;
                //crediNoteNew.DocNo = TObject.DocNo;
                crediNoteNew.EntityId = TObject.EntityId;
                crediNoteNew.CreditTermsId = TObject.CreditTermsId;
                crediNoteNew.Nature = TObject.Nature;
                crediNoteNew.ServiceCompanyId = TObject.ServiceCompanyId;
                crediNoteNew.DocCurrency = TObject.DocCurrency;
                crediNoteNew.IsMultiCurrency = TObject.IsMultiCurrency;
                crediNoteNew.ExCurrency = TObject.BaseCurrency;
                crediNoteNew.PostingDate = TObject.PostingDate.Date;
                crediNoteNew.ExchangeRate = TObject.ExchangeRate;
                crediNoteNew.ExDurationFrom = TObject.ExDurationFrom;
                crediNoteNew.ExDurationTo = TObject.ExDurationTo;
                crediNoteNew.IsGSTApplied = TObject.IsGSTApplied;
                if (isNew == true)
                    crediNoteNew.ExtensionType = TObject.ExtensionType;
                crediNoteNew.IsGstSettings = TObject.IsGstSettings;
                crediNoteNew.GSTExCurrency = TObject.GSTExCurrency;
                crediNoteNew.GSTExchangeRate = TObject.GSTExchangeRate;
                if (TObject.IsGstSettings)
                {
                    crediNoteNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    crediNoteNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                decimal Remaining = 0;
                if (isNew == false)
                {
                    if (crediNoteNew.GrandTotal != TObject.GrandTotal)
                    {
                        if (crediNoteNew.GrandTotal > TObject.GrandTotal)
                        {
                            Remaining = crediNoteNew.GrandTotal - TObject.GrandTotal;
                            crediNoteNew.BalanceAmount = crediNoteNew.BalanceAmount - Remaining;
                        }
                        else if (crediNoteNew.GrandTotal < TObject.GrandTotal)
                        {
                            Remaining = TObject.GrandTotal - crediNoteNew.GrandTotal;
                            crediNoteNew.BalanceAmount = crediNoteNew.BalanceAmount + Remaining;
                        }
                    }
                }

                crediNoteNew.GrandTotal = TObject.GrandTotal;

                if (isNew)
                    crediNoteNew.BalanceAmount = TObject.GrandTotal;


                crediNoteNew.GSTTotalAmount = TObject.GSTTotalAmount;

                //crediNoteNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;

                crediNoteNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                crediNoteNew.NoSupportingDocs = crediNoteNew.IsNoSupportingDocument == true ? TObject.NoSupportingDocument : false;
                //  crediNoteNew.IsSegmentReporting = TObject.IsSegmentReporting;
                //if (TObject.IsSegmentActive1 != null || TObject.IsSegmentActive1 == true)
                //{
                //    crediNoteNew.SegmentMasterid1 = TObject.SegmentMasterid1;
                //    crediNoteNew.SegmentDetailid1 = TObject.SegmentDetailid1;
                //    crediNoteNew.SegmentCategory1 = TObject.SegmentCategory1;
                //}
                //if (TObject.IsSegmentActive2 != null || TObject.IsSegmentActive2 == true)
                //{
                //    crediNoteNew.SegmentMasterid2 = TObject.SegmentMasterid2;
                //    crediNoteNew.SegmentDetailid2 = TObject.SegmentDetailid2;
                //    crediNoteNew.SegmentCategory2 = TObject.SegmentCategory2;
                //}
                crediNoteNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                crediNoteNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                crediNoteNew.Remarks = TObject.Remarks;
                crediNoteNew.DocDescription = TObject.DocDescription;
                //crediNoteNew.IsGstSettings = TObject.IsGstSettings;
                //crediNoteNew.IsSegmentReporting = TObject.IsSegmentReporting;
                crediNoteNew.Status = TObject.Status;
                crediNoteNew.ModifiedBy = TObject.ModifiedBy;
                crediNoteNew.ModifiedDate = TObject.ModifiedDate;

                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_InsertCreditMemo_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

        }
        private void FillCreditMemo(CreditMemoModel memoDTO, CreditMemo creditMemo)
        {
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_FillCreditMemo_FillCall_Request_Message);
                memoDTO.IsModify = creditMemo.ClearCount > 0;
                memoDTO.Id = creditMemo.Id;
                memoDTO.CompanyId = creditMemo.CompanyId;
                memoDTO.EntityType = creditMemo.EntityType;
                memoDTO.DocType = creditMemo.DocType;
                memoDTO.IsLocked = creditMemo.IsLocked;
                memoDTO.DocSubType = creditMemo.DocSubType;
                memoDTO.CreditMemoNumber = creditMemo.CreditMemoNumber;
                memoDTO.DocNo = creditMemo.DocNo;
                memoDTO.DocDate = creditMemo.DocDate;
                memoDTO.DueDate = creditMemo.DueDate;
                memoDTO.EntityId = creditMemo.EntityId;
                memoDTO.Version = "0x" + string.Concat(Array.ConvertAll(creditMemo.Version, x => x.ToString("X2")));
                //BeanEntity beanEntity = _beanEntityService.Query(a => a.Id == creditMemo.EntityId).Select().FirstOrDefault();
                memoDTO.EntityName = _beanEntityService.GetEntityName(creditMemo.EntityId);
                memoDTO.CreditTermsId = creditMemo.CreditTermsId;
                memoDTO.Nature = creditMemo.Nature;
                memoDTO.DocCurrency = creditMemo.DocCurrency;
                memoDTO.ServiceCompanyId = creditMemo.ServiceCompanyId;
                memoDTO.PostingDate = creditMemo.PostingDate;
                memoDTO.IsMultiCurrency = creditMemo.IsMultiCurrency;
                memoDTO.BaseCurrency = creditMemo.ExCurrency;
                memoDTO.ExchangeRate = creditMemo.ExchangeRate;
                memoDTO.ExDurationFrom = creditMemo.ExDurationFrom;
                memoDTO.ExDurationTo = creditMemo.ExDurationTo;
                memoDTO.IsGSTApplied = creditMemo.IsGSTApplied;

                memoDTO.IsGstSettings = creditMemo.IsGstSettings;
                memoDTO.GSTExCurrency = creditMemo.GSTExCurrency;
                memoDTO.GSTExchangeRate = creditMemo.GSTExchangeRate;
                memoDTO.GSTExDurationFrom = creditMemo.GSTExDurationFrom;
                memoDTO.GSTExDurationTo = creditMemo.GSTExDurationTo;

                //memoDTO.IsSegmentReporting = creditMemo.IsSegmentReporting;
                //memoDTO.SegmentCategory1 = creditMemo.SegmentCategory1;
                //memoDTO.SegmentCategory2 = creditMemo.SegmentCategory2;

                memoDTO.GSTTotalAmount = creditMemo.GSTTotalAmount;
                memoDTO.GrandTotal = creditMemo.GrandTotal;
                memoDTO.BalanceAmount = creditMemo.BalanceAmount;
                memoDTO.ExtensionType = ExtensionType.General;
                memoDTO.ExternalType = creditMemo.DocSubType == DocTypeConstants.OpeningBalance ? creditMemo.ExtensionType : null;
                memoDTO.SavedFrom = creditMemo.ExtensionType;
                memoDTO.ModifiedBy = creditMemo.ModifiedBy;
                //memoDTO.IsAllowableNonAllowable = creditMemo.IsAllowableNonAllowable;

                //memoDTO.IsNoSupportingDocument = creditMemo.IsNoSupportingDocument;
                memoDTO.NoSupportingDocument = creditMemo.NoSupportingDocs;

                //memoDTO.SegmentMasterid1 = creditMemo.SegmentMasterid1;

                //if (creditMemo.SegmentMasterid1 != null)
                //{
                //    var segment1 = _segmentMasterService.GetSegmentMastersById(creditMemo.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    memoDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
                //}
                //if (creditMemo.SegmentMasterid2 != null)
                //{
                //    var segment2 = _segmentMasterService.GetSegmentMastersById(creditMemo.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    memoDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
                //}

                //memoDTO.SegmentMasterid2 = creditMemo.SegmentMasterid2;
                //memoDTO.SegmentDetailid1 = creditMemo.SegmentDetailid1;
                //memoDTO.SegmentDetailid2 = creditMemo.SegmentDetailid2;

                memoDTO.DocDescription = creditMemo.DocDescription;
                memoDTO.Status = creditMemo.Status;
                memoDTO.DocumentState = creditMemo.DocumentState;
                memoDTO.ModifiedDate = creditMemo.ModifiedDate;
                memoDTO.ModifiedBy = creditMemo.ModifiedBy;
                memoDTO.CreatedDate = creditMemo.CreatedDate;
                memoDTO.UserCreated = creditMemo.UserCreated;

                memoDTO.IsBaseCurrencyRateChanged = creditMemo.IsBaseCurrencyRateChanged;
                memoDTO.IsGSTCurrencyRateChanged = creditMemo.IsGSTCurrencyRateChanged;
                memoDTO.OpeningBalanceId = creditMemo.OpeningBalanceId;
                List<CreditMemoApplication> lstApplications = _creditMemoApplicationService.GetAllMemoApplication(creditMemo.Id);
                foreach (CreditMemoApplication application in lstApplications)
                {
                    CreditMemoApplicationModel model = new CreditMemoApplicationModel();
                    FillCreditMemoApplicationModel(model, application);
                    memoDTO.CreditMemoApplicationModels.Add(model);
                }
                //    memoDTO.CreditMemoApplicationModels = (from CCA in _creditMemoApplicationService.Queryable()
                //                                           where CCA.CreditMemoId== creditMemo.Id
                //                                           select new CreditMemoApplicationModel()

                //                     {
                //                                               Id = CCA.Id,
                //    CreditMemoId = CCA.CreditMemoId,
                //    CompanyId = CCA.CompanyId,
                //    //var creditMemo = _creditMemoService.GetMemo(CCA.CreditMemoId),
                //    //CNAModel.DocNo = creditMemo.DocNo,
                //    //CNAModel.DocCurrency = creditMemo.DocCurrency,
                //    //CNAModel.CreditMemoAmount = creditMemo.GrandTotal,
                //    //if (CCA.Status == CNAModel.CreditMemoBalanceAmount)
                //    //    CNAModel.CreditMemoBalanceAmount = CNAModel.CreditMemoAmount,
                //    //else
                //    //    CNAModel.CreditMemoBalanceAmount = creditMemo.BalanceAmount + CCA.CreditAmount,
                //    CreditMemoBalanceAmount= CCA.Status== CNAModel.CreditMemoBalanceAmount ? CNAModel.CreditMemoAmount
                //    CreditMemoApplicationNumber = CCA.CreditMemoApplicationNumber,
                //    CreditAmount = CCA.CreditAmount,
                //    IsNoSupportingDocument = CCA.IsNoSupportingDocument,
                //    NoSupportingDocument = CCA.IsNoSupportingDocumentActivated,
                //    CreditMemoApplicationDate = CCA.CreditMemoApplicationDate,
                //    CreditMemoApplicationResetDate = CCA.CreditMemoApplicationResetDate,
                //    Remarks = CCA.Remarks,
                //    CreatedDate = DateTime.UtcNow,
                //    UserCreated = CCA.UserCreated,
                //    Status = CCA.Status,
                //}).ToList();
                //List<MemoApplicationModel> lstMemoAppModel = new List<MemoApplicationModel>();
                //var lstCreditMemoApp = _creditMemoApplicationService.GetCreditMemoApp(creditMemo.Id);
                ////var lstMemoDetails = _creditMemoApplicationDetailService.GetCreditMemoDetailById(creditMemo.Id);
                //if (lstCreditMemoApp.Any())
                //{
                //    foreach (var memoAppD in lstCreditMemoApp)
                //    {
                //        MemoApplicationModel memoAppModel = new MemoApplicationModel();
                //        memoAppModel.MemoDetailId = memoAppD.Id;
                //        memoAppModel.DocDate = memoAppD.CreditMemoApplicationDate;
                //        memoAppModel.DocNo = memoAppD.CreditMemoApplicationNumber;
                //        memoAppModel.SystemRefNo = memoAppD.CreditMemoApplicationNumber;
                //        memoAppModel.Ammount = memoAppD.CreditAmount;
                //        memoAppModel.DocType = "CM Application";
                //        lstMemoAppModel.Add(memoAppModel);
                //    }
                //    memoDTO.MemoApplicationModels = lstMemoAppModel;
                //    if (memoDTO.MemoApplicationModels.Any())
                //    {
                //        memoDTO.MemoTotalAmount = memoDTO.MemoApplicationModels.Sum(c => c.Ammount);
                //    }
                //}
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_FillCreditMemo_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        private void FillCreditMemoDetailModel(CreditMemoDetailModel detailModel, CreditMemoDetail detail)
        {
            detailModel.Id = detail.Id;
            detailModel.CreditMemoId = detail.CreditMemoId;
            detailModel.TaxId = detail.TaxId;
            //if (detail.TaxId != null)
            //{
            //    var taxCode = _taxCodeService.GetTaxCode(detail.TaxId.Value);
            //    if (taxCode != null)
            //    {
            //        detailModel.TaxRate = taxCode.TaxRate;
            //        detailModel.TaxType = taxCode.TaxType;
            //        detailModel.TaxIdCode = taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") + "(" + taxCode.TaxType[0] + ")" : taxCode.Code;
            //        detailModel.TaxCode = taxCode.Code;
            //    }
            //}
            //detailModel.COAId = detail.COAId;
            //var coa = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
            //if (coa != null)
            //    detailModel.AccountName = coa.Name;
            detailModel.DocAmount = detail.DocAmount;
            detailModel.DocTotalAmount = detail.DocTotalAmount;
            detailModel.DocTaxAmount = detail.DocTaxAmount;
            detailModel.Description = detail.Description;
            detailModel.AllowDisAllow = detail.AllowDisAllow;
            detailModel.BaseAmount = detail.BaseAmount;
            detailModel.BaseTaxAmount = detail.BaseTaxAmount;
            detailModel.BaseTotalAmount = detail.BaseTotalAmount;
            detailModel.RecOrder = detail.RecOrder;
            detailModel.IsPLAccount = detail.IsPLAccount;
        }
        private void FillCreditMemoApplicationModel(CreditMemoApplicationModel CNAModel, CreditMemoApplication CCA)
        {
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_FillCreditMemoApplicationModel_FillCall_Request_Message);
                CNAModel.Id = CCA.Id;
                CNAModel.CreditMemoId = CCA.CreditMemoId;
                CNAModel.IsModify = CCA.ClearCount > 0;
                CNAModel.IsLocked = CCA.IsLocked;
                CNAModel.CompanyId = CCA.CompanyId;
                var creditMemo = _creditMemoService.GetMemo(CCA.CreditMemoId);
                CNAModel.DocNo = creditMemo.DocNo;
                CNAModel.DocCurrency = creditMemo.DocCurrency;
                CNAModel.Version = "0x" + string.Concat(Array.ConvertAll(CCA.Version, x => x.ToString("X2")));
                CNAModel.CreditMemoAmount = creditMemo.GrandTotal;
                if (CCA.Status == CreditMemoApplicationStatus.Void)
                    CNAModel.CreditMemoBalanceAmount = CNAModel.CreditMemoAmount;
                else
                    CNAModel.CreditMemoBalanceAmount = creditMemo.BalanceAmount + CCA.CreditAmount;
                CNAModel.CreditMemoApplicationNumber = CCA.CreditMemoApplicationNumber;
                CNAModel.DocState = CCA.Status == CreditMemoApplicationStatus.Void ? CreditMemoState.Void : null;
                CNAModel.CreditAmount = CCA.CreditAmount;
                CNAModel.IsNoSupportingDocument = CCA.IsNoSupportingDocument;
                CNAModel.NoSupportingDocument = CCA.IsNoSupportingDocumentActivated;
                CNAModel.CreditMemoApplicationDate = CCA.CreditMemoApplicationDate;
                CNAModel.CreditMemoApplicationResetDate = CCA.CreditMemoApplicationResetDate;
                CNAModel.Remarks = CCA.Remarks;
                CNAModel.CreatedDate = DateTime.UtcNow;
                CNAModel.UserCreated = CCA.UserCreated;
                CNAModel.Status = CCA.Status;
                CNAModel.IsRevExcess = CCA.IsRevExcess;
                CNAModel.DocumentId = CCA.DocumentId;
                CNAModel.ClearingState = CCA.ClearingState;
                CNAModel.ModifiedBy = CCA.ModifiedBy;
                CNAModel.ModifiedDate = CCA.ModifiedDate;
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_FillCreditMemoApplicationModel_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw;
            }

        }
        private void UpdateCreditMemoDetails(CreditMemoModel TObject, CreditMemo creditMemoNew)
        {
            try
            {
                int? recorder = TObject.CreditMemoDetailModels.Any() ? TObject.CreditMemoDetailModels.Max(d => d.RecOrder) : 0;
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_UpdateCreditMemoDetails_Update_Request_Message);
                foreach (CreditMemoDetailModel detail in TObject.CreditMemoDetailModels)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        CreditMemoDetail memoDetail = new CreditMemoDetail();
                        FillMemoDetail(memoDetail, detail, TObject.ExchangeRate);
                        memoDetail.Id = Guid.NewGuid();
                        memoDetail.CreditMemoId = TObject.Id;
                        memoDetail.RecOrder = recorder + 1;
                        recorder = memoDetail.RecOrder;
                        memoDetail.ObjectState = ObjectState.Added;
                        _creditMemoDetailService.Insert(memoDetail);

                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        CreditMemoDetail memoDetail = _creditMemoDetailService.GetCreditMemoDetail(detail.Id);
                        if (memoDetail != null)
                        {
                            FillMemoDetail(memoDetail, detail, TObject.ExchangeRate);
                            memoDetail.RecOrder = detail.RecOrder;
                            memoDetail.ObjectState = ObjectState.Modified;
                            _creditMemoDetailService.Update(memoDetail);
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        CreditMemoDetail memoDetail = _creditMemoDetailService.GetCreditMemoDetail(detail.Id);
                        if (memoDetail != null)
                        {
                            memoDetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_UpdateCreditMemoDetails_Update_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

        }
        private void FillMemoDetail(CreditMemoDetail detail, CreditMemoDetailModel detailModel, decimal? exchangeRate)
        {
            detail.COAId = detailModel.COAId;
            detail.AllowDisAllow = detailModel.AllowDisAllow;
            detail.TaxId = detailModel.TaxId;
            detail.TaxRate = detailModel.TaxRate;
            detail.TaxIdCode = detailModel.TaxIdCode;
            detail.DocTaxAmount = detailModel.DocTaxAmount;
            detail.TaxCurrency = detailModel.TaxCurrency;
            detail.DocAmount = detailModel.DocAmount;
            detail.DocTotalAmount = detailModel.DocTotalAmount;
            detail.Description = detailModel.Description;
            detail.BaseAmount = exchangeRate != null ? Math.Round((decimal)detail.DocAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
            detail.BaseTaxAmount = exchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocTaxAmount : detail.DocTaxAmount;
            detail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
            detail.IsPLAccount = detailModel.IsPLAccount;

        }
        private static string DBQuery(Guid creditMemoId, long companyId, string extensionType, string docType)
        {
            return $"SELECT BIL.DocumentDate as DocDate from (SELECT CMAD.DocumentId from Bean.CreditMemo CM with (nolock) Inner join Bean.CreditMemoApplication CMA with (nolock) on CMA.CreditMemoId = CM.Id Inner join Bean.CreditMemoApplicationDetail CMAD with (nolock) on CMAD.CreditMemoApplicationId = CMA.Id WHERE CM.DocType = '{docType}' and CM.Id = '{creditMemoId}' and CM.ExtensionType = '{extensionType}') AS A JOIN Bean.Bill BIL with (nolock) ON BIL.Id = A.DocumentId Where BIL.CompanyId = {companyId}";
        }
        private void FillCreditMemoAppDetail(CreditMemoApplicationDetailModel detailModel, CreditMemoApplicationDetail detail, CreditMemoApplicationModel CMAModel, CreditMemoApplication CMApplication, List<Bill> lstBills, CreditMemo memo, Guid? receiptId, Guid? paymentId, long? receiptServiceComp, long? paymentServiceComp, string username, long companyId, string receiptDocState, string paymentDocState, string receiptServComp, string paymentServComp)
        {
            detailModel.Id = detail.Id;
            detailModel.CreditMemoApplicationId = detail.CreditMemoApplicationId;
            detailModel.DocCurrency = CMAModel.DocCurrency;
            detailModel.CreditAmount = detail.CreditAmount;
            //var bill = _billService.GetCrediMemoByDocId(detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
            {
                Bill bill = lstBills.Any() ? lstBills.Where(x => x.Id == detail.DocumentId).FirstOrDefault() : null;
                if (bill != null)
                {
                    detailModel.DocAmount = bill.GrandTotal;
                    detailModel.DocDate = bill.DocumentDate;
                    detailModel.DocumentId = bill.Id;
                    detailModel.DocNo = bill.DocNo;
                    detailModel.SystemReferenceNumber = bill.SystemReferenceNumber;
                    if (CMApplication.Status == CreditMemoApplicationStatus.Void)
                        detailModel.BalanceAmount = bill.BalanceAmount;
                    else
                        detailModel.BalanceAmount = bill.BalanceAmount + detail.CreditAmount;
                    detailModel.DocType = bill.DocType;
                    detailModel.Nature = bill.Nature;
                    detailModel.PostingDate = bill.PostingDate;
                    //detailModel.SegmentCategory1 = bill.SegmentCategory1;
                    //detailModel.SegmentCategory2 = bill.SegmentCategory2;
                    detailModel.BaseCurrencyExchangeRate = bill.ExchangeRate.Value;
                    detailModel.ServiceEntityId = bill.ServiceCompanyId;
                    detailModel.DocState = bill.DocumentState;
                    detailModel.IsHyperLinkEnable = true;
                }
            }
            else
            {
                if (memo != null)
                {
                    detailModel.DocAmount = memo.GrandTotal;
                    detailModel.DocDate = (detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment) ? CMApplication.CreditMemoApplicationDate : memo.DocDate;
                    //detailModel.DocumentId = memo.Id;
                    detailModel.DocumentId = detail.DocumentType == DocTypeConstants.Receipt ? (receiptId != Guid.Empty ? receiptId.Value : Guid.Empty) : (paymentId != Guid.Empty ? paymentId.Value : Guid.Empty);
                    detailModel.DocNo = detail.DocNo;
                    if (CMApplication.Status == CreditMemoApplicationStatus.Void)
                        detailModel.BalanceAmount = memo.BalanceAmount;
                    else
                        detailModel.BalanceAmount = memo.BalanceAmount + detail.CreditAmount;
                    detailModel.DocType = detail.DocumentType;
                    detailModel.Nature = memo.Nature;
                    //detailModel.SegmentCategory1 = bill.SegmentCategory1;
                    //detailModel.SegmentCategory2 = bill.SegmentCategory2;
                    detailModel.BaseCurrencyExchangeRate = memo.ExchangeRate.Value;
                    if (detail.DocumentType == DocTypeConstants.Receipt)
                        detailModel.IsHyperLinkEnable = _companyService.GetServiceCompanyStatusByUsername((Int64)(receiptServiceComp), companyId, username);
                    else if (detail.DocumentType == DocTypeConstants.BillPayment)
                        detailModel.IsHyperLinkEnable = _companyService.GetServiceCompanyStatusByUsername((Int64)(paymentServiceComp), companyId, username);
                    detailModel.ServiceEntityId = detail.DocumentType == DocTypeConstants.Receipt ? (Int64)(receiptServiceComp).Value : detail.DocumentType == DocTypeConstants.BillPayment ? (Int64)(paymentServiceComp).Value : memo.ServiceCompanyId;
                    detailModel.DocState = detail.DocumentType == DocTypeConstants.Receipt ? receiptDocState : detail.DocumentType == DocTypeConstants.BillPayment ? paymentDocState : memo.DocumentState;
                    detailModel.ServCompanyName = detail.DocumentType == DocTypeConstants.Receipt ? receiptServComp : detail.DocumentType == DocTypeConstants.BillPayment ? paymentServComp : null;
                }
            }
        }
        private void FillCreditMemoAppNew(CreditMemoApplicationModel CMAModel, long companyId, CreditMemo crediMemo)
        {
            CMAModel.Id = Guid.NewGuid();
            CMAModel.CompanyId = companyId;
            CMAModel.CreditMemoId = crediMemo.Id;
            //var crediMemo = _creditMemoService.GetMemo(creditMemoId);
            CMAModel.DocCurrency = crediMemo.DocCurrency;
            CMAModel.DocNo = crediMemo.DocNo;
            CMAModel.DocDate = crediMemo.PostingDate;
            CMAModel.CreditMemoAmount = crediMemo.GrandTotal;
            CMAModel.CreditMemoApplicationDate = DateTime.UtcNow;
            CMAModel.CreditMemoBalanceAmount = crediMemo.BalanceAmount;
            CMAModel.CreditAmount = crediMemo.GrandTotal;
            CMAModel.CreditMemoApplicationNumber = crediMemo.CreditMemoNumber;
            CMAModel.Remarks = crediMemo.DocDescription;
            //CMAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
            CMAModel.NoSupportingDocument = false;
        }
        private void ValidateCreditNoteApplication(CreditMemo creditMemo, CreditMemoApplicationModel TObject)
        {
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            if (TObject.CreditMemoApplicationDate == null)
            {
                throw new Exception(CreditMemoValidations.Invalid_Application_Date);
            }

            if ((TObject.CreditMemoApplicationDetailModels == null || TObject.CreditMemoApplicationDetailModels.Count == 0) && TObject.IsRevExcess != true)
            {
                throw new Exception(CreditMemoValidations.Atleast_one_Application_is_required);
            }
            else
            {
                if (TObject.IsRevExcess != true)
                {
                    int itemCount = TObject.CreditMemoApplicationDetailModels.Where(a => a.CreditAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(CreditMemoValidations.Total_Amount_To_Credit_should_be_greater_than_Zero);
                    }
                }
                else
                {
                    int itemCount = TObject.ReverseExcessModels.Where(a => a.DocAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(CreditMemoValidations.Total_Amount_To_Credit_should_be_greater_than_Zero);
                    }
                }

            }

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(TObject.CreditMemoApplicationDate, TObject.CompanyId))
            {
                throw new Exception(CreditMemoValidations.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.CreditMemoApplicationDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CreditMemoValidations.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.CreditMemoApplicationDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CreditMemoValidations.Invalid_Financial_Period_Lock_Password);
                }
            }

            //Verify if any Invoices or Debit Notes have Tagged amount, so that it is not allowed credit the amount. 
            // ( That tagged amount should be reset in Debt Provision allocations )
            string DocNumbers = "";
            List<Guid> lstDocIds = TObject.CreditMemoApplicationDetailModels.Where(a => a.DocType == DocTypeConstants.BillCreditMemo).Select(a => a.DocumentId).ToList();
            List<CreditMemo> lstTaggedInvoices = new List<CreditMemo>();
            if (lstDocIds.Count > 0)
            {
                lstTaggedInvoices = _creditMemoService.GetTaggedMemoByCustomerAndCurrency(creditMemo.EntityId, creditMemo.DocCurrency, creditMemo.CompanyId).Where(a => lstDocIds.Contains(a.Id)).ToList();
                if (lstTaggedInvoices.Count > 0)
                {
                    foreach (CreditMemo v in lstTaggedInvoices)
                        DocNumbers += v.CreditMemoNumber + ",";
                }
            }
            if (TObject.IsRevExcess != true)
            {
                //Verify if any of the application have amount
                var amountDocuments = TObject.CreditMemoApplicationDetailModels.Where(a => a.CreditAmount > 0).ToList();
                if (amountDocuments.Count == 0)
                    throw new Exception(CreditMemoValidations.Atleast_one_Application_should_be_given);

                //Verify Duplication Documents in details
                var duplicateDocuments = TObject.CreditMemoApplicationDetailModels.GroupBy(x => x.DocumentId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (duplicateDocuments.Count > 0)
                    throw new Exception(CreditMemoValidations.Duplicate_documents_in_details);
            }
        }
        public void UpdateCreditMemoApplicationDetails(CreditMemoApplicationModel model, CreditMemoApplication cnApplication, string ConnectionString, DateTime? postingDate, List<DocumentHistoryModel> lstOfDocumentHistoryModel, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_UpdateCreditMemoApplicationDetails_Update_Request_Message);
                List<CreditMemoApplicationDetail> lstDetails = cnApplication.CreditMemoApplicationDetails.Where(a => !model.CreditMemoApplicationDetailModels.Any(b => b.Id == a.Id)).ToList();

                foreach (CreditMemoApplicationDetail detailDelete in lstDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;

                bool isAdded = false;
                decimal oldCreditAmount = 0;
                decimal? roundAmount = 0;

                //Getting the bills by passing the documentids
                List<Bill> lstOfBills = _billService.GetAllBillsByDocIds(model.CreditMemoApplicationDetailModels.Select(a => a.DocumentId).ToList(), cnApplication.CompanyId);

                //Checking the documentstate before proceeding to save the Credit note app details
                if ((lstOfBills.Any() && lstOfBills.Any(a => a.DocumentState == InvoiceStates.Void)))
                    throw new Exception(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);

                foreach (CreditMemoApplicationDetailModel detailModel in model.CreditMemoApplicationDetailModels)
                {
                    oldCreditAmount = cnApplication.CreditMemoApplicationDetails.Where(a => a.Id == detailModel.Id).Select(a => a.CreditAmount).FirstOrDefault();
                    CreditMemoApplicationDetail detail = cnApplication.CreditMemoApplicationDetails.Where(a => a.Id == detailModel.Id).FirstOrDefault();
                    roundAmount = 0;
                    if (detail == null)
                    {
                        if (detailModel.CreditAmount != null || detailModel.CreditAmount != 0)
                        {
                            detail = new CreditMemoApplicationDetail();
                            detail.Id = Guid.NewGuid();
                            detail.CreditMemoApplicationId = model.Id;
                            detail.DocumentId = detailModel.DocumentId;
                            detail.DocumentType = detailModel.DocType;
                            detail.DocCurrency = detailModel.DocCurrency;
                            detail.CreditAmount = detailModel.CreditAmount;
                            detail.BaseCurrencyExchangeRate = detailModel.BaseCurrencyExchangeRate;
                            detail.DocNo = detailModel.DocNo;
                            detail.COAId = detailModel.COAId;
                            //detail.SegmentCategory1 = detailModel.SegmentCategory1;
                            //detail.SegmentCategory2 = detailModel.SegmentCategory2;
                            isAdded = true;
                            UpdateDocumentState(detail.DocumentId.Value, detail.DocumentType, detail.CreditAmount, ConnectionString, model.Id, postingDate, lstOfDocumentHistoryModel, false, detail.CreditAmount, lstOfRoundingAmount, true, detail.CreditAmount, detailModel.CreditAmount, detail.RoundingAmount, lstOfBills, out roundAmount);
                            detail.RoundingAmount = roundAmount;
                            detail.ObjectState = ObjectState.Added;
                            cnApplication.CreditMemoApplicationDetails.Add(detail);
                        }
                    }
                    else
                    {
                        isAdded = false;
                        UpdateDocumentState(detail.DocumentId.Value, detail.DocumentType, detailModel.CreditAmount - detail.CreditAmount, ConnectionString, model.Id, postingDate, lstOfDocumentHistoryModel, false, detailModel.CreditAmount, lstOfRoundingAmount, isAdded, oldCreditAmount, detailModel.CreditAmount, detail.RoundingAmount, lstOfBills, out roundAmount);
                        if (detailModel.CreditAmount == null || detailModel.CreditAmount == 0)
                        {
                            detail.ObjectState = ObjectState.Deleted;
                        }
                        else
                        {
                            detail.RoundingAmount = roundAmount;
                            detail.COAId = detailModel.COAId;
                            detail.CreditAmount = detailModel.CreditAmount;
                            detail.ObjectState = ObjectState.Modified;
                        }
                    }
                }
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_UpdateCreditMemoApplicationDetails_Update_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw;
            }
        }

        private void UpdateDocumentState(Guid documentId, string DocType, decimal amount, string ConnectionString, Guid applicationId, DateTime? postingDate, List<DocumentHistoryModel> lstOfDocumentHistoryModel, bool isVoid, decimal creditAmount, Dictionary<Guid, decimal> lstOfRoundingAmount, bool isAdded, decimal oldCreditAmount, decimal newCreditAmount, decimal? detailRoundingAmount, List<Bill> lstOfBills, out decimal? newRoundingAmount)
        {
            string docState = null;
            decimal roundingAmount = 0;
            bool? isUpdate = true;
            newRoundingAmount = detailRoundingAmount;

            //if (amount == 0)
            //    return;
            UpdatePosting up = new UpdatePosting();
            if (DocType == DocTypeConstants.Bills || DocType == DocTypeConstants.General || DocType == DocTypeConstants.PayrollBill || DocType == DocTypeConstants.OpeningBalance || DocType == DocTypeConstants.Claim)
            {
                //Bill document = _billService.GetCrediMemoByDocId(documentId);
                Bill document = lstOfBills.Where(a => a.Id == documentId).FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(CreditMemoValidations.Invalid_Bill_to_Update_Balance_Amount);
                }
                if (isVoid != true && amount == 0 && isAdded == false && document.DocumentState == InvoiceStates.FullyPaid)
                {

                    if (detailRoundingAmount != 0 && detailRoundingAmount != null)
                        lstOfRoundingAmount.Add(document.Id, detailRoundingAmount.Value);

                    newRoundingAmount = detailRoundingAmount != null ? detailRoundingAmount.Value : 0;
                    return;
                }
                docState = document.DocumentState;
                document.BalanceAmount -= amount;
                if (document.BalanceAmount == 0)
                {
                    document.DocumentState = InvoiceStates.FullyPaid;
                    if (isAdded)
                    {
                        if (document.RoundingAmount != null && document.RoundingAmount != 0)
                            roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                        else
                            roundingAmount = Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)document.BaseBalanceAmount;

                        document.BaseBalanceAmount = 0;
                        if (roundingAmount != 0)
                            lstOfRoundingAmount.Add(document.Id, roundingAmount);
                        document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                        newRoundingAmount = roundingAmount;
                    }
                    else
                    {
                        if (oldCreditAmount != newCreditAmount)
                        {
                            if (newCreditAmount == document.GrandTotal)
                            {
                                roundingAmount = Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)document.BaseGrandTotal);
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(document.Id, roundingAmount);
                                newRoundingAmount = roundingAmount;
                                document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                                document.BaseBalanceAmount = 0;
                            }
                            else if (detailRoundingAmount != 0 && detailRoundingAmount != null)
                            {
                                document.BaseBalanceAmount = 0;
                                lstOfRoundingAmount.Add(document.Id, detailRoundingAmount.Value);
                                newRoundingAmount = detailRoundingAmount.Value;
                                roundingAmount = detailRoundingAmount.Value;
                            }
                            else
                            {
                                if (document.RoundingAmount != null && document.RoundingAmount != 0)
                                    roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                                else
                                    //roundingAmount = (((Math.Round(Math.Abs(document.GrandTotal - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) + (Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) - (decimal)document.BaseGrandTotal) - (document.RoundingAmount != null ? (decimal)document.RoundingAmount : 0);
                                    roundingAmount = Math.Round(Math.Abs(newCreditAmount - oldCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)document.BaseBalanceAmount);

                                document.BaseBalanceAmount = 0;
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(document.Id, roundingAmount);
                                newRoundingAmount = roundingAmount;
                                document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                            }
                        }
                    }
                    //JournalDetail detail = _journalDetailService.GetDetail(document.Id);
                    //if (detail != null)
                    //{
                    //    detail.ClearingStatus = "Cleared";
                    //    detail.ClearingDate = document.DocumentDate;
                    //    detail.ObjectState = ObjectState.Modified;
                    //    _journalDetailService.Update(detail);
                    //}
                    if (document.DocSubType != DocTypeConstants.OpeningBalance)
                    {
                        #region Proc_Update_ClearingStatus_For_Bill_And_PayrollBill

                        //SqlConnection con = new SqlConnection(ConnectionString);
                        //if (con.State != ConnectionState.Open)
                        //    con.Open();
                        //SqlCommand cmd = new SqlCommand("PROC_UpdateClearedItem_Bean_Update", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@Id", document.Id);
                        //cmd.Parameters.AddWithValue("@DocDate", document.DocumentDate);
                        //cmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                        ////cmd.Parameters.AddWithValue("@IsEqual", false);
                        //int updateCount = cmd.ExecuteNonQuery();
                        //con.Close();

                        #endregion Update_ClearingStatus_For_Bill_And_PayrollBill
                    }
                }

                else if (document.BalanceAmount > 0)
                {
                    document.DocumentState = InvoiceStates.PartialPaid;
                    if (isVoid != true)
                    {
                        if (isAdded)
                            document.BaseBalanceAmount -= Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                        else
                        {
                            if (oldCreditAmount != newCreditAmount)
                            {
                                document.BaseBalanceAmount = oldCreditAmount > newCreditAmount ? document.BaseBalanceAmount + (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : document.BaseBalanceAmount - (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                if (docState == InvoiceStates.FullyPaid)
                                {
                                    document.RoundingAmount = ((detailRoundingAmount != 0 && detailRoundingAmount != null) && (docState != document.DocumentState)) ? detailRoundingAmount : document.RoundingAmount;
                                    newRoundingAmount = ((detailRoundingAmount != 0 && detailRoundingAmount != null) && (docState != document.DocumentState)) ? 0 : detailRoundingAmount;
                                }
                            }
                        }
                    }
                    else
                    {
                        document.BaseBalanceAmount += (Math.Round((Math.Abs(amount) * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                        document.RoundingAmount += (detailRoundingAmount != null && detailRoundingAmount != 0) ? detailRoundingAmount : 0;
                        isUpdate = true;
                        roundingAmount = 0;
                    }
                }

                else if (document.BalanceAmount < 0)
                {
                    //document.DocumentState = InvoiceStates.ExcesPaid;
                    //throw new Exception(String.Format("Invoice ({0}) Balance Amount is becoming negative", document.InvoiceNumber));
                    throw new Exception("Credit memo amount shouldn't be greater than outstanding balance of bill");
                }
                if (document.GrandTotal == document.BalanceAmount)
                {
                    document.DocumentState = InvoiceStates.NotPaid;
                    document.BaseBalanceAmount = document.BaseGrandTotal;
                    if (isUpdate == false)
                        document.RoundingAmount += (detailRoundingAmount != null && detailRoundingAmount != 0) ? detailRoundingAmount : 0;
                    newRoundingAmount = 0;
                }

                document.ObjectState = ObjectState.Modified;
                document.ModifiedBy = CreditMemoConstant.System;
                document.ModifiedDate = DateTime.UtcNow;
                _billService.Update(document);

                if (document.DocSubType == DocTypeConstants.OpeningBalance)
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                    oBcmd.CommandTimeout = 30;
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", document.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", document.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", document.BalanceAmount == document.GrandTotal ? true : false);
                    int res = oBcmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    #region Update_Journal_Detail_Clearing_Status
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd1 = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", conn);
                    cmd1.CommandTimeout = 30;
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@companyId", document.CompanyId);
                    cmd1.Parameters.AddWithValue("@documentId", document.Id);
                    cmd1.Parameters.AddWithValue("@docState", document.DocumentState);
                    cmd1.Parameters.AddWithValue("@balanceAmount", document.BalanceAmount);
                    int count = cmd1.ExecuteNonQuery();

                    conn.Close();
                    #endregion Update_Journal_Detail_Clearing_Status
                }
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(applicationId, document.CompanyId, document.Id, document.DocType, document.DocSubType, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount.Value, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.Remarks, isVoid ? null : postingDate, isVoid ? 0 : creditAmount < 0 ? creditAmount : -creditAmount, roundingAmount);

                    if (lstdocumet.Any())
                        lstOfDocumentHistoryModel.AddRange(lstdocumet);
                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                #endregion Documentary History
                //if (document.DocSubType != DocTypeConstants.OpeningBalance)
                //{
                //    FillJournalState(up, document);
                //    UpdatePosting(up);
                //}
            }
        }
        private void FillJournalState(UpdatePosting _posting, Bill debit)
        {
            _posting.Id = debit.Id;
            _posting.CompanyId = debit.CompanyId;
            _posting.DocumentState = debit.DocumentState;
            _posting.BalanceAmount = debit.BalanceAmount;
            _posting.ModifiedBy = CreditMemoConstant.System;
            _posting.ModifiedDate = DateTime.UtcNow;
        }
        public void UpdatePosting(UpdatePosting upmodel)
        {

            var json = RestSharpHelper.ConvertObjectToJason(upmodel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == CreditMemoConstant.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = upmodel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/updateposting", json);
                if (response.ErrorMessage != null)
                {
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
            }
        }
        private string GetNextApplicationNumber(Guid id)
        {
            string DocNumber = "";
            try
            {
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_GetNextApplicationNumber_GetCall_Request_Message);
                CreditMemo memo = _creditMemoService.GetMemo(id);
                var CNAM = _creditMemoApplicationService.Query(x => x.CreditMemoId == id /*&& x.Status != CreditMemoApplicationStatus.Void*/).Select(x => new { MemoNo = x.CreditMemoApplicationNumber, Status = x.Status, Date = x.CreatedDate }).OrderByDescending(a => a.Date).FirstOrDefault();
                int DocNo = 0;
                if (CNAM != null)
                {
                    DocNo = CNAM.Status != CreditMemoApplicationStatus.Void ? Convert.ToInt32(CNAM.MemoNo.Substring(CNAM.MemoNo.LastIndexOf("-A") + 2)) : Convert.ToInt32(CNAM.MemoNo.Substring(CNAM.MemoNo.IndexOf("-A") + 2).Remove(CNAM.MemoNo.Substring(CNAM.MemoNo.IndexOf("-A") + 2).LastIndexOf("-V"), 2));
                }
                DocNo++;
                DocNumber = memo.CreditMemoNumber + ("-A" + DocNo);
                LoggingHelper.LogMessage(CreditMemoConstant.CreditMemoApplicationService, CreditMemoLoggingValidation.Log_GetNextApplicationNumber_GetCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return DocNumber;
        }
        private void FillCreditMemoJournal(JVModel headJournal, CreditMemoApplication creditMemoApplication, bool isNew, bool? isOffset)
        {
            decimal? amountDue = 0;
            int? recOrder = 0;
            CreditMemo memo = _creditMemoService.GetCreditMemoById(creditMemoApplication.CreditMemoId);

            //modified for COA
            Dictionary<long, string> coaNames = _chartOfAccountService.GetChartofAccounts(new List<string>() { COANameConstants.AccountsPayable, COANameConstants.OtherPayables }, memo.CompanyId);
            //For Analytics Modified by Pradhan
            //if (memo != null)
            //{
            //    var journalByMemoId = _journalService.GetJournal(memo.CompanyId, memo.Id);
            //    if (journalByMemoId != null)
            //    {
            //        journalByMemoId.BalanceAmount = memo.BalanceAmount - creditMemoApplication.CreditAmount;
            //        amountDue = journalByMemoId.BalanceAmount;
            //        journalByMemoId.ObjectState = ObjectState.Modified;
            //        _journalService.Update(journalByMemoId);
            //        JournalDetail jDetail = _journalDetailService.GetDetailByJournalId(journalByMemoId.Id);
            //        if (jDetail != null)
            //        {
            //            jDetail.AmountDue = amountDue;
            //            jDetail.ObjectState = ObjectState.Modified;
            //            _journalDetailService.Update(jDetail);
            //        }
            //    }
            //}
            if (coaNames != null)
            {
                headJournal.COAId = memo.Nature == "Trade" ? coaNames.Where(a => a.Value == COANameConstants.AccountsPayable).Select(a => a.Key).FirstOrDefault() : coaNames.Where(a => a.Value == COANameConstants.OtherPayables).Select(a => a.Key).FirstOrDefault();
            }
            else
            {
                ChartOfAccount account = _chartOfAccountService.Query(a => a.Name == (memo.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables) && a.CompanyId == memo.CompanyId).Select().FirstOrDefault();
                if (account != null)
                {
                    headJournal.COAId = account.Id;
                    headJournal.AccountCode = account.Code;
                    headJournal.AccountName = account.Name;
                }
            }
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = memo.Id;
            FillCMJV(headJournal, creditMemoApplication, memo);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            FillJVHeadCMDetails(jModel, creditMemoApplication, memo);
            //if (account != null)
            //    jModel.COAId = account.Id;
            if (headJournal.COAId != null)
                jModel.COAId = headJournal.COAId;
            jModel.AmountDue = amountDue;
            lstJD.Add(jModel);

            long? taxPaybleGstId = _chartOfAccountService.GetCoaIdByNameAndCompanyId(COANameConstants.TaxPayableGST, creditMemoApplication.CompanyId);

            foreach (CreditMemoApplicationDetail detail in creditMemoApplication.CreditMemoApplicationDetails.Where(a => a.CreditAmount > 0))
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillCMDetails(journal, creditMemoApplication, memo, detail, coaNames, isOffset);
                headJournal.BalanceAmount = journal.AmountDue;
                journal.BaseCurrency = memo.ExCurrency;
                journal.RecOrder = ++recOrder;
                lstJD.Add(journal);
                //for Taxpayble GST line Item
                if (creditMemoApplication.IsRevExcess == true)
                {
                    if (memo.IsGstSettings && (detail.TaxRate != null && detail.TaxIdCode != "NA"))
                    {
                        JVVDetailModel gstJDetailModel = new JVVDetailModel();
                        if (isNew)
                            gstJDetailModel.Id = Guid.NewGuid();
                        else
                            gstJDetailModel.Id = detail.Id;

                        FillGSTDetail(gstJDetailModel, creditMemoApplication, detail, taxPaybleGstId, memo);
                        gstJDetailModel.RecOrder = lstJD.Max(a => a.RecOrder) + 1;
                        lstJD.Add(gstJDetailModel);
                    }
                }
                else
                {
                    if (memo.DocCurrency != memo.ExCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.DocumentId = creditMemoApplication.Id;
                        journal1.DocType = memo.DocType;
                        journal1.DocSubType = "Application";
                        journal1.AccountDescription = creditMemoApplication.Remarks;
                        journal1.DocDate = creditMemoApplication.CreditMemoApplicationDate;
                        journal1.PostingDate = creditMemoApplication.CreditMemoApplicationDate;
                        journal1.DocNo = creditMemoApplication.CreditMemoApplicationNumber;
                        journal1.RecOrder = lstJD.Max(a => a.RecOrder) + 1;
                        var inv = _billService.GetCrediMemoByDocId(detail.DocumentId.Value);
                        if (inv != null)
                        {
                            journal1.Nature = inv.Nature;
                            journal1.EntityId = inv.EntityId;
                            //journal1.AmountDue = inv.BalanceAmount;
                            // journal1.OffsetDocument = inv.SystemReferenceNumber;
                            journal1.ExchangeRate = inv.ExchangeRate;
                        }
                        journal1.ExchangeRate = isOffset == true ? detail.BaseCurrencyExchangeRate : journal1.ExchangeRate;
                        ChartOfAccount account2 = _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == memo.CompanyId).Select().FirstOrDefault();
                        if (account2 != null)
                        {
                            journal1.COAId = account2.Id;
                        }
                        journal1.DocCurrency = detail.DocCurrency;
                        journal1.BaseCurrency = memo.ExCurrency;
                        journal1.ServiceCompanyId = memo != null ? memo.ServiceCompanyId.Value : 0;
                        if (memo.ExchangeRate > journal1.ExchangeRate)
                        {
                            journal1.BaseDebit = Math.Round((decimal)(memo.ExchangeRate - journal1.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (memo.ExchangeRate < journal1.ExchangeRate)
                        {
                            journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - memo.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (memo.ExchangeRate != journal1.ExchangeRate)
                        {
                            lstJD.Add(journal1);
                        }
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseDebit > 0 || a.BaseCredit > 0).OrderBy(x => x.RecOrder).ToList();
        }
        private void FillGSTDetail(JVVDetailModel gstJDetailModel, CreditMemoApplication cmApplication, CreditMemoApplicationDetail cmAppDetail, long? gstTaxId, CreditMemo creditMemo)
        {
            gstJDetailModel.DocumentDetailId = cmAppDetail.Id;
            gstJDetailModel.DocumentId = cmApplication.Id;
            gstJDetailModel.Nature = creditMemo.Nature;
            gstJDetailModel.ServiceCompanyId = creditMemo.ServiceCompanyId.Value;
            gstJDetailModel.DocNo = cmApplication.CreditMemoApplicationNumber;
            gstJDetailModel.DocType = DocTypeConstants.BillCreditMemo;
            gstJDetailModel.DocSubType = DocTypeConstants.Application;
            gstJDetailModel.PostingDate = cmApplication.CreditMemoApplicationDate;
            gstJDetailModel.EntityId = creditMemo.EntityId;
            //ChartOfAccount account2 = _chartOfAccountService.Query(a => a.CompanyId == invoice.CompanyId && a.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();

            gstJDetailModel.COAId = gstTaxId.Value;
            gstJDetailModel.DocCurrency = cmAppDetail.DocCurrency;
            gstJDetailModel.BaseCurrency = creditMemo.ExCurrency;
            gstJDetailModel.ExchangeRate = creditMemo.ExchangeRate;
            gstJDetailModel.GSTExCurrency = creditMemo.GSTExCurrency;
            gstJDetailModel.NoSupportingDocs = creditMemo.IsNoSupportingDocument != true && creditMemo.NoSupportingDocs != true ? false : true;
            gstJDetailModel.GSTExchangeRate = creditMemo.GSTExchangeRate;
            //jVDetail.AccountDescription = CNAppDetail.DocDescription;
            gstJDetailModel.AccountDescription = cmApplication.Remarks;
            if (cmAppDetail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //TaxCode tax = lstTaxCodes.Where(a => a.Id == detail.TaxId).FirstOrDefault();
                gstJDetailModel.TaxId = cmAppDetail.TaxId;
                gstJDetailModel.TaxCode = cmAppDetail.TaxIdCode;
                gstJDetailModel.TaxRate = cmAppDetail.TaxRate;
            }
            gstJDetailModel.DocDebit = cmAppDetail.TaxAmount;
            gstJDetailModel.BaseDebit = Math.Round((decimal)creditMemo.ExchangeRate == null ? (decimal)gstJDetailModel.DocDebit : (decimal)(gstJDetailModel.DocDebit * creditMemo.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            gstJDetailModel.IsTax = true;
        }
        private void FillCMJV(JVModel headJournal, CreditMemoApplication creditMemoApplication, CreditMemo memo)
        {
            string strServiceCompany = _companyService.GetIdBy(memo.ServiceCompanyId.Value);
            //TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(memo.CreditTermsId);
            headJournal.DocumentId = creditMemoApplication.Id;
            headJournal.ParentId = creditMemoApplication.CreditMemoId;
            headJournal.CompanyId = memo.CompanyId;
            headJournal.PostingDate = creditMemoApplication.CreditMemoApplicationDate;
            headJournal.DocNo = creditMemoApplication.CreditMemoApplicationNumber;
            headJournal.DocType = DocTypeConstants.BillCreditMemo;
            headJournal.DocSubType = "Application";
            headJournal.DocDate = creditMemoApplication.CreditMemoApplicationDate;
            headJournal.DueDate = memo.DueDate;
            headJournal.DocumentState = creditMemoApplication.Status.ToString();
            headJournal.SystemReferenceNo = creditMemoApplication.CreditMemoApplicationNumber;
            headJournal.ServiceCompanyId = memo.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = memo.Nature;
            headJournal.IsGstSettings = memo.IsGstSettings;
            headJournal.IsGSTApplied = memo.IsGSTApplied;
            headJournal.IsMultiCurrency = memo.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = creditMemoApplication.IsNoSupportingDocument;
            if (memo.IsAllowableDisallowableActivated != null)
                headJournal.IsAllowableNonAllowable = creditMemoApplication.IsNoSupportingDocumentActivated;
            //headJournal.PONo = memo.PONo;
            headJournal.ExDurationFrom = memo.ExDurationFrom;
            headJournal.ExDurationTo = memo.ExDurationTo;
            headJournal.GSTExDurationFrom = memo.GSTExDurationFrom;
            headJournal.GSTExDurationTo = memo.GSTExDurationTo;
            //headJournal.CreditTerms = (int)top.TOPValue;
            headJournal.IsSegmentReporting = memo.IsSegmentReporting;
            //headJournal.SegmentCategory1 = memo.SegmentCategory1;
            //headJournal.SegmentMasterid1 = memo.SegmentMasterid1;
            //headJournal.SegmentCategory2 = memo.SegmentCategory2;
            //headJournal.SegmentMasterid2 = memo.SegmentMasterid2;
            //headJournal.SegmentDetailid1 = memo.SegmentDetailid1;
            //headJournal.SegmentDetailid2 = memo.SegmentDetailid2;
            headJournal.CreditTermsId = memo.CreditTermsId;
            headJournal.NoSupportingDocument = creditMemoApplication.IsNoSupportingDocumentActivated;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = memo.EntityId;
            //BeanEntity entity = _beanEntityService.GetEntityById(memo.EntityId);
            //headJournal.EntityName = entity.Name;
            headJournal.EntityType = memo.EntityType;
            headJournal.DocCurrency = memo.DocCurrency;
            headJournal.BaseCurrency = memo.ExCurrency;
            headJournal.ExchangeRate = memo.ExchangeRate;
            headJournal.GrandDocDebitTotal = creditMemoApplication.CreditAmount;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(creditMemoApplication.CreditAmount * (memo.ExchangeRate != null ? memo.ExchangeRate : 1)), 2);
            if (memo.IsGstSettings)
            {
                headJournal.GSTExCurrency = memo.GSTExCurrency;
                headJournal.GSTExchangeRate = memo.GSTExchangeRate;
            }
            headJournal.DocumentDescription = creditMemoApplication.Remarks;
            //headJournal.Remarks = creditMemoApplication.Remarks;
            headJournal.UserCreated = memo.UserCreated;
            headJournal.CreatedDate = memo.CreatedDate;
            headJournal.ModifiedDate = memo.ModifiedDate;
            headJournal.ModifiedBy = memo.ModifiedBy;
            headJournal.IsFirst = true;
        }
        private void FillCMDetails(JVVDetailModel journal, CreditMemoApplication creditMemoApplication, CreditMemo memo, CreditMemoApplicationDetail detail, Dictionary<long, string> lstCoaNames, bool? isOffset)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = creditMemoApplication.Id;
            journal.PostingDate = creditMemoApplication.CreditMemoApplicationDate;
            journal.DocType = memo.DocType;
            journal.DocSubType = "Application";
            journal.DocNo = creditMemoApplication.CreditMemoApplicationNumber;
            journal.AccountDescription = creditMemoApplication.IsRevExcess == true ? detail.DocDescription : creditMemoApplication.Remarks;
            if (detail.DocumentId != null)
            {
                if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                {
                    Bill bill = _billService.GetCrediMemoByDocId(detail.DocumentId.Value);
                    if (bill != null)
                    {
                        journal.Nature = bill.Nature;
                        journal.EntityId = bill.EntityId;
                        journal.OffsetDocument = bill.SystemReferenceNumber;
                        journal.ExchangeRate = bill.ExchangeRate;
                        //journal.DocDate = bill.DocumentDate;
                    }
                }
                else
                {
                    journal.Nature = memo.Nature;
                    journal.EntityId = memo.EntityId;
                    journal.OffsetDocument = memo.DocNo;
                    //journal.DocDate = memo.DocDate;
                }
            }
            if (creditMemoApplication.IsRevExcess == true)
            {
                journal.EntityId = memo.EntityId;
                journal.ExchangeRate = memo.ExchangeRate;
                journal.COAId = detail.COAId;
            }
            else
            {
                if (lstCoaNames != null)
                {
                    journal.COAId = journal.Nature == "Trade" ? lstCoaNames.Where(a => a.Value == COANameConstants.AccountsPayable).Select(a => a.Key).FirstOrDefault() : lstCoaNames.Where(a => a.Value == COANameConstants.OtherPayables).Select(a => a.Key).FirstOrDefault();
                }
                else
                {
                    ChartOfAccount account1 = _chartOfAccountService.Query(a => a.Name == (journal.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables) && a.CompanyId == memo.CompanyId).Select().FirstOrDefault();
                    if (account1 != null)
                    {
                        journal.COAId = account1.Id;
                    }
                }

            }
            journal.DocDate = creditMemoApplication.CreditMemoApplicationDate;
            journal.COAId = isOffset == true ? detail.COAId : journal.COAId;
            journal.ExchangeRate = isOffset == true && memo.ExCurrency != memo.DocCurrency ? detail.BaseCurrencyExchangeRate : journal.ExchangeRate;
            journal.SettlementMode = "CM Application";
            journal.SettlementRefNo = creditMemoApplication.CreditMemoApplicationNumber;
            journal.SettlementDate = creditMemoApplication.CreatedDate;
            journal.ServiceCompanyId = memo.ServiceCompanyId.Value;
            //  journal.DocSubType = memo.DocSubType;
            //journal.ExchangeRate = billoice.ExchangeRate;
            journal.DocDebit = detail.CreditAmount;
            journal.BaseDebit = Math.Round((decimal)journal.DocDebit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            journal.DocCreditTotal = detail.CreditAmount;
        }
        private void FillJVHeadCMDetails(JVVDetailModel jModel, CreditMemoApplication creditMemoApplication, CreditMemo memo)
        {
            jModel.DocumentId = creditMemoApplication.Id;
            jModel.SystemRefNo = creditMemoApplication.CreditMemoApplicationNumber;
            jModel.DocNo = creditMemoApplication.CreditMemoApplicationNumber;
            jModel.DocDate = creditMemoApplication.CreditMemoApplicationDate;
            jModel.ServiceCompanyId = memo.ServiceCompanyId.Value;
            jModel.Nature = memo.Nature;
            //jModel.DocSubType = memo.DocSubType;
            jModel.DocType = memo.DocSubType;
            jModel.DocSubType = "Application";
            //jModel.Remarks = memo.Remarks;
            jModel.EntityId = memo.EntityId;
            jModel.DocCurrency = memo.DocCurrency;
            jModel.BaseCurrency = memo.ExCurrency;
            jModel.PostingDate = creditMemoApplication.CreditMemoApplicationDate;
            jModel.ExchangeRate = memo.ExchangeRate;
            jModel.GSTExCurrency = memo.GSTExCurrency;
            jModel.GSTExchangeRate = memo.GSTExchangeRate;
            jModel.DocCredit = creditMemoApplication.CreditAmount;
            jModel.BaseCredit = Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocCredit) : (decimal)jModel.DocCredit, 2, MidpointRounding.AwayFromZero);
            //jModel.SegmentCategory1 = memo.SegmentCategory1;
            //jModel.SegmentCategory2 = memo.SegmentCategory2;
            //jModel.SegmentMasterid1 = memo.SegmentMasterid1;
            //jModel.SegmentMasterid2 = memo.SegmentMasterid2;
            //jModel.SegmentDetailid1 = memo.SegmentDetailid1;
            //jModel.SegmentDetailid2 = memo.SegmentDetailid2;
            jModel.SettlementMode = "CM Application";
            jModel.SettlementRefNo = creditMemoApplication.CreditMemoApplicationNumber;
            jModel.OffsetDocument = memo.CreditMemoNumber;
            jModel.AccountDescription = creditMemoApplication.Remarks;
        }

        private string GetAutoNumberByEntityType(long companyId, CreditMemo lastInvoice, string entityType, AppsWorld.CreditMemoModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.CreditMemoModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewCreditMemoDocNo(DocTypeConstants.BillCreditMemo, companyId);
                    //invDTO.IsEditable = true;
                    isEdit = true;
                }
                else
                {
                    //invDTO.IsEditable = false;
                    isEdit = false;
                    //List<Invoice> lstInvoice = _invoiceEntityService.GetAllInvoiceByCIDandType(companyid, DocTypeConstants.Invoice);
                    string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
                    string number = "1";
                    if (lastInvoice != null)
                    {
                        if (_autoNo.Format.Contains("{MM/YYYY}"))
                        {
                            //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                            if (lastInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
                            {
                                //number = "1";
                                outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                            else
                            {
                                string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                        }
                        else
                        {
                            string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                            outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        }
                    }
                    else
                    {
                        string output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //counter = Convert.ToInt32(value);
                    }
                }
            }
            return outPutNumber;
        }
        #endregion

        #region Posting Block
        private void FillJournal(JVModel headJournal, CreditMemo creditMemo, bool isNew, string type)
        {
            fillJV(headJournal, creditMemo, type);
            headJournal.COAId = _chartOfAccountService.GetChartOfAccountByNature(creditMemo.Nature, creditMemo.CompanyId);
            //headJournal.COAId = account.Id;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = creditMemo.Id;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            int? recOreder = 0;

            foreach (CreditMemoDetail detail in creditMemo.CreditMemoDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                FillJVDetail(journal, creditMemo, detail, type);
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                //account = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
                //journal.COAId = account.Id;
                //journal.RecOrder = recOreder + 1;
                //recOreder = journal.RecOrder;
                journal.RecOrder = detail.RecOrder;
                lstJD.Add(journal);
            }
            FillJVHeadDetail(jModel, creditMemo, type);
            //jModel.AmountDue = creditMemo.DocumentState != CreditNoteState.NotApplied ? headJournal.BalanceAmount : null;
            //jModel.RecOrder = recOreder + 1;
            //recOreder = jModel.RecOrder;
            lstJD.Add(jModel);
            if (creditMemo.IsGstSettings)
            {
                ChartOfAccount account = _chartOfAccountService.Query(a => a.CompanyId == creditMemo.CompanyId && a.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();
                foreach (CreditMemoDetail detail in creditMemo.CreditMemoDetails.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    FillJVGstDetail(journal, detail, creditMemo, type, account);
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    journal.RecOrder = detail.RecOrder;
                    lstJD.Add(journal);
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0).OrderBy(d => d.RecOrder).ToList();
        }
        private void FillJVGstDetail(JVVDetailModel journal, CreditMemoDetail detail, CreditMemo creditMemo, string type, ChartOfAccount account)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = creditMemo.Id;
            journal.Nature = creditMemo.Nature;
            journal.DocDate = creditMemo.DocDate;
            journal.PostingDate = creditMemo.PostingDate;
            journal.ServiceCompanyId = creditMemo.ServiceCompanyId.Value;
            journal.DocNo = creditMemo.DocNo;
            journal.DocType = creditMemo.DocType;
            //journal.DocSubType = creditMemo.DocSubType;
            journal.DocSubType = DocTypeConstants.General;
            journal.COAId = account.Id;
            journal.AccountCode = account.Code;
            journal.AccountName = account.Name;
            journal.DocCurrency = creditMemo.DocCurrency;
            journal.BaseCurrency = creditMemo.ExCurrency;
            journal.ExchangeRate = creditMemo.ExchangeRate;
            journal.GSTExCurrency = creditMemo.GSTExCurrency;
            journal.GSTExchangeRate = creditMemo.GSTExchangeRate;
            journal.EntityId = creditMemo.EntityId;
            //journal.AccountDescription = detail.Description;
            journal.AccountDescription = creditMemo.DocDescription;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxCode = detail.TaxIdCode;
                journal.TaxRate = detail.TaxRate;
            }
            journal.DocCredit = detail.DocTaxAmount;
            journal.BaseCredit = Math.Round(creditMemo.ExchangeRate == null ? (decimal)journal.DocCredit : (decimal)((journal.DocCredit == null || journal.DocCredit == 0 ? 0 : journal.DocCredit) * creditMemo.ExchangeRate), 2);
            journal.IsTax = true;
        }
        private void FillJVDetail(JVVDetailModel journal, CreditMemo creditMemo, CreditMemoDetail detail, string type)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = creditMemo.Id;
            journal.Nature = creditMemo.Nature;
            journal.ServiceCompanyId = creditMemo.ServiceCompanyId.Value;
            journal.DocNo = creditMemo.DocNo;
            journal.DocType = creditMemo.DocType;
            journal.PostingDate = creditMemo.PostingDate;
            //journal.DocSubType = creditMemo.DocSubType;
            journal.DocSubType = DocTypeConstants.General;
            journal.EntityId = creditMemo.EntityId;
            //ChartOfAccount account1 =
            // _chartOfAccountService.GetChartOfAccountByName(creditMemo.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables,
            //     creditMemo.CompanyId);
            //if (account1 != null)
            //{
            //    journal.COAId = account1.Id;
            //    journal.AccountName = account1.Name;
            //    journal.AccountCode = account1.Code;
            //}
            journal.COAId = detail.COAId;
            journal.SystemRefNo = creditMemo.CreditMemoNumber;
            journal.DocDate = creditMemo.DocDate;
            //if (detail.Qty != null)
            //    journal.Qty = detail.Qty.Value;
            //journal.Unit = detail.Unit;
            //journal.UnitPrice = detail.UnitPrice.Value;
            //journal.Discount = detail.Discount.Value;
            //journal.DiscountType = detail.DiscountType;
            journal.AllowDisAllow = detail.AllowDisAllow;
            journal.DocCurrency = creditMemo.DocCurrency;
            journal.BaseCurrency = creditMemo.ExCurrency;
            journal.ExchangeRate = creditMemo.ExchangeRate;
            journal.GSTExCurrency = creditMemo.GSTExCurrency;
            journal.GSTExchangeRate = creditMemo.GSTExchangeRate;
            journal.AccountDescription = detail.Description;
            //journal.SegmentCategory1 = creditMemo.SegmentCategory1;
            //journal.SegmentCategory2 = creditMemo.SegmentCategory2;
            //journal.SegmentMasterid1 = creditMemo.SegmentMasterid1;
            //journal.SegmentMasterid2 = creditMemo.SegmentMasterid2;
            //journal.SegmentDetailid1 = creditMemo.SegmentDetailid1;
            //journal.SegmentDetailid2 = creditMemo.SegmentDetailid2;
            //if (creditMemo.IsGstSettings)
            //{
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxCode = detail.TaxIdCode;
                journal.TaxRate = detail.TaxRate;
            }
            //  }
            if (type == DocTypeConstants.BillCreditMemo)
            {
                journal.DocCredit = detail.DocAmount;
                journal.BaseCredit = Math.Round((decimal)creditMemo.ExchangeRate == null ? (decimal)journal.DocCredit : (decimal)(journal.DocCredit * creditMemo.ExchangeRate), 2);
                journal.DocTaxCredit = detail.DocTaxAmount;
                journal.BaseTaxCredit = detail.BaseTaxAmount;
            }
            //if (type == DocTypeConstants.BillCreditMemo)
            //{
            //    journal.DocDebit = detail.DocAmount;
            //    journal.BaseDebit = Math.Round((decimal)creditMemo.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * creditMemo.ExchangeRate), 2);
            //    //journal.BaseDebit = detail.BaseAmount;
            //}
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.BaseTaxableAmount = detail.BaseAmount;
            journal.BaseTaxAmount = detail.BaseTaxAmount;
            journal.IsTax = false;
            journal.DocCreditTotal = detail.DocTotalAmount;
        }
        private void FillJVHeadDetail(JVVDetailModel jModel, CreditMemo creditMemo, string type)
        {
            jModel.DocumentId = creditMemo.Id;
            jModel.SystemRefNo = creditMemo.CreditMemoNumber;
            jModel.DocNo = creditMemo.DocNo;
            jModel.ServiceCompanyId = creditMemo.ServiceCompanyId.Value;
            jModel.Nature = creditMemo.Nature;
            jModel.DocDate = creditMemo.DocDate;
            jModel.DocType = creditMemo.DocType;
            jModel.DocSubType = creditMemo.DocSubType;
            //jModel.Remarks = creditMemo.Remarks;
            //jModel.DocDescription = creditMemo.DocDescription;
            jModel.AccountDescription = creditMemo.DocDescription;
            jModel.PostingDate = creditMemo.PostingDate;
            jModel.PONo = creditMemo.PONo;
            jModel.CreditTermsId = creditMemo.CreditTermsId;
            jModel.DueDate = creditMemo.DueDate;
            jModel.EntityId = creditMemo.EntityId;
            jModel.DocCurrency = creditMemo.DocCurrency;
            jModel.BaseCurrency = creditMemo.ExCurrency;
            jModel.ExchangeRate = creditMemo.ExchangeRate;
            jModel.GSTExCurrency = creditMemo.GSTExCurrency;
            jModel.GSTExchangeRate = creditMemo.GSTExchangeRate;
            if (type == DocTypeConstants.BillCreditMemo)
            {
                jModel.DocDebit = creditMemo.GrandTotal;
                //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocDebit : (decimal)(jModel.DocDebit * jModel.ExchangeRate), 2);
                decimal amount = 0;
                foreach (var detail in creditMemo.CreditMemoDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseDebit = amount;
            }
            ChartOfAccount account1 =
               _chartOfAccountService.GetChartOfAccountByName(creditMemo.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables,
                   creditMemo.CompanyId);
            jModel.COAId = account1.Id;
            jModel.AccountName = account1.Name;
            //if (type == DocTypeConstants.BillCreditMemo)
            //{
            //    jModel.DocCredit = creditMemo.GrandTotal;
            //    jModel.BaseCredit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocCredit : (decimal)(jModel.DocCredit * jModel.ExchangeRate), 2);
            //}
            //jModel.SegmentCategory1 = creditMemo.SegmentCategory1;
            //jModel.SegmentCategory2 = creditMemo.SegmentCategory2;
            //jModel.SegmentMasterid1 = creditMemo.SegmentMasterid1;
            //jModel.SegmentMasterid2 = creditMemo.SegmentMasterid2;
            //jModel.SegmentDetailid1 = creditMemo.SegmentDetailid1;
            //jModel.SegmentDetailid2 = creditMemo.SegmentDetailid2;
            jModel.BaseAmount = creditMemo.BalanceAmount;
        }
        private void fillJV(JVModel headJournal, CreditMemo creditMemo, string type)
        {
            string strServiceCompany = _companyService.GetIdBy(creditMemo.ServiceCompanyId.Value);
            TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(creditMemo.CreditTermsId);
            headJournal.DocumentId = creditMemo.Id;
            headJournal.CompanyId = creditMemo.CompanyId;
            headJournal.PostingDate = creditMemo.DocDate;
            headJournal.DocNo = creditMemo.DocNo;
            headJournal.DocType = DocTypeConstants.BillCreditMemo;
            //headJournal.DocSubType = creditMemo.DocSubType;
            headJournal.DocSubType = DocTypeConstants.General;
            headJournal.DocDate = creditMemo.DocDate;
            headJournal.DueDate = creditMemo.DueDate.Value;
            headJournal.DocumentState = creditMemo.DocumentState;
            headJournal.SystemReferenceNo = creditMemo.CreditMemoNumber;
            headJournal.ServiceCompanyId = creditMemo.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = creditMemo.Nature;
            headJournal.PONo = creditMemo.PONo;
            headJournal.ExDurationFrom = creditMemo.ExDurationFrom;
            headJournal.ExDurationTo = creditMemo.ExDurationTo;
            headJournal.IsAllowableNonAllowable = creditMemo.IsAllowableNonAllowable;
            headJournal.GSTExDurationFrom = creditMemo.GSTExDurationFrom;
            headJournal.GSTExDurationTo = creditMemo.GSTExDurationTo;
            //headJournal.CreditTermsId = creditMemo.CreditTermsId.Value;
            headJournal.BalanceAmount = creditMemo.BalanceAmount;
            if (top != null)
                headJournal.CreditTerms = (int)top.TOPValue;
            headJournal.IsSegmentReporting = creditMemo.IsSegmentReporting;
            //headJournal.SegmentCategory1 = creditMemo.SegmentCategory1;
            //headJournal.SegmentCategory2 = creditMemo.SegmentCategory2;
            //headJournal.SegmentMasterid1 = creditMemo.SegmentMasterid1;
            //headJournal.SegmentMasterid2 = creditMemo.SegmentMasterid2;
            //headJournal.SegmentDetailid1 = creditMemo.SegmentDetailid1;
            //headJournal.SegmentDetailid2 = creditMemo.SegmentDetailid2;
            headJournal.NoSupportingDocument = creditMemo.NoSupportingDocs;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = creditMemo.EntityId;
            headJournal.EntityType = creditMemo.EntityType;
            //headJournal.IsRepeatingInvoice = creditMemo.IsRepeatingInvoice;
            headJournal.DocCurrency = creditMemo.DocCurrency;
            if (type == DocTypeConstants.BillCreditMemo)
            {
                headJournal.GrandDocCreditTotal = creditMemo.GrandTotal;
                headJournal.GrandBaseCreditTotal = Math.Round((decimal)(creditMemo.GrandTotal * (creditMemo.ExchangeRate != null ? creditMemo.ExchangeRate : 1)), 2);
                headJournal.GrandDocDebitTotal = creditMemo.GrandTotal;
                headJournal.GrandBaseDebitTotal = Math.Round((decimal)(creditMemo.GrandTotal * (creditMemo.ExchangeRate != null ? creditMemo.ExchangeRate : 1)), 2);
            }
            headJournal.BaseCurrency = creditMemo.ExCurrency;
            headJournal.ExchangeRate = creditMemo.ExchangeRate;
            headJournal.IsGstSettings = creditMemo.IsGstSettings;
            headJournal.IsGSTApplied = creditMemo.IsGSTApplied;
            headJournal.IsMultiCurrency = creditMemo.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = creditMemo.IsNoSupportingDocument;

            if (creditMemo.IsGstSettings)
            {
                headJournal.GSTExCurrency = creditMemo.GSTExCurrency;
                headJournal.GSTExchangeRate = creditMemo.GSTExchangeRate;
            }
            //headJournal.Remarks = creditMemo.DocDescription;
            headJournal.DocumentDescription = creditMemo.DocDescription;
            headJournal.UserCreated = creditMemo.UserCreated;
            headJournal.CreatedDate = creditMemo.CreatedDate;
            headJournal.ModifiedBy = creditMemo.ModifiedBy;
            headJournal.ModifiedDate = creditMemo.ModifiedDate;
        }
        public void SaveInvoice1(JVModel clientModel)
        {

            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == CreditMemoConstant.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;

                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);

                if (response.ErrorMessage != null)
                {

                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                var message = ex.Message;
            }

        }
        public void DeleteJVPostMemo(JournalSaveModel tObject)
        {
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == CreditMemoConstant.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = tObject;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                if (response.ErrorMessage != null)
                {
                    Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CreditMemoConstant.CreditMemoApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }
        #endregion

        #region AutoNumber 
        string value = "";
        public string GenerateAutoNumberForType(long CompanyId, string Type, string companyCode)
        {
            AppsWorld.CreditMemoModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(CompanyId, Type);
            string generatedAutoNumber = "";

            if (Type == DocTypeConstants.BillCreditMemo)
            {
                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength), _autoNo.GeneratedNumber, CompanyId, companyCode);

                if (_autoNo != null)
                {
                    _autoNo.GeneratedNumber = value;
                    _autoNo.IsDisable = true;
                    _autoNo.ObjectState = ObjectState.Modified;
                    _autoNumberService.Update(_autoNo);
                }
                var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
                if (_autonumberCompany.Any())
                {
                    AppsWorld.CreditMemoModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AppsWorld.CreditMemoModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.CreditMemoModule.Entities.AutoNumberCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;

                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }
            }
            return generatedAutoNumber;
        }

        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, string Companycode = null)
        {
            int? currentMonth = 0;
            bool ifContainsMonth = false;
            List<CreditMemo> lstCreditMemo = null;

            string OutputNumber = "";
            string counter = "";
            string companyFormatHere = companyFormatFrom.ToUpper();

            if (companyFormatHere.Contains("{YYYY}"))
            {
                companyFormatHere = companyFormatHere.Replace("{YYYY}", DateTime.Now.Year.ToString());
            }
            else if (companyFormatHere.Contains("{MM/YYYY}"))
            {
                companyFormatHere = companyFormatHere.Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
                currentMonth = DateTime.Now.Month;
                ifContainsMonth = true;
            }
            else if (companyFormatHere.Contains("{COMPANYCODE}"))
            {
                companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
            }
            double val = 0;
            if (Type == DocTypeConstants.BillCreditMemo)
            {

                lstCreditMemo = _creditMemoService.GetCompanyIdAndDocType(companyId);


                if (lstCreditMemo.Any() && ifContainsMonth)
                {
                    if (DateTime.Now.Year == lstCreditMemo.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        AppsWorld.CreditMemoModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        int? lastCreatedDate = lstCreditMemo.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                        if (currentMonth == lastCreatedDate)
                        {
                            foreach (var creditMemo in lstCreditMemo)
                            {
                                if (creditMemo.CreditMemoNumber != autonumber.Preview)
                                    val = Convert.ToInt32(IncreamentVal);
                                else
                                {
                                    val = Convert.ToInt32(IncreamentVal) + 1;
                                    break;
                                }
                            }
                        }
                        else
                            val = 1;
                    }
                    else
                        val = 1;
                }
                else if (lstCreditMemo.Any() && ifContainsMonth == false)
                {
                    AppsWorld.CreditMemoModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var creditMemo in lstCreditMemo)
                    {
                        if (creditMemo.CreditMemoNumber != autonumber.Preview)
                            val = Convert.ToInt32(IncreamentVal);
                        else
                        {
                            val = Convert.ToInt32(IncreamentVal) + 1;
                            break;
                        }
                    }
                }
                else
                {
                    val = Convert.ToInt32(IncreamentVal);
                }
            }
            if (counterLength == 1)
                counter = string.Format("{0:0}", val);
            else if (counterLength == 2)
                counter = string.Format("{0:00}", val);
            else if (counterLength == 3)
                counter = string.Format("{0:000}", val);
            else if (counterLength == 4)
                counter = string.Format("{0:0000}", val);
            else if (counterLength == 5)
                counter = string.Format("{0:00000}", val);
            else if (counterLength == 6)
                counter = string.Format("{0:000000}", val);
            else if (counterLength == 7)
                counter = string.Format("{0:0000000}", val);
            else if (counterLength == 8)
                counter = string.Format("{0:00000000}", val);
            else if (counterLength == 9)
                counter = string.Format("{0:000000000}", val);
            else if (counterLength == 10)
                counter = string.Format("{0:0000000000}", val);

            value = counter;
            OutputNumber = companyFormatHere + counter;

            if (lstCreditMemo.Any())
            {
                OutputNumber = GetNewNumber(lstCreditMemo, Type, OutputNumber, counter, companyFormatHere, counterLength);
            }

            return OutputNumber;
        }
        private string GetNewNumber(List<CreditMemo> lstCreditMemo, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstCreditMemo.Where(a => a.CreditMemoNumber == outputNumber).FirstOrDefault();
            bool isNotexist = false;
            int i = Convert.ToInt32(counter);
            if (invoice != null)
            {
                var lstCMo = lstCreditMemo.Where(a => a.CreditMemoNumber.Contains(format)).ToList();
                if (lstCMo.Any())
                {
                    while (isNotexist == false)
                    {
                        i++;
                        string length = i.ToString();
                        value = length.PadLeft(counterLength, '0');
                        val2 = format + value;
                        var inv = lstCMo.Where(c => c.CreditMemoNumber == val2).FirstOrDefault();
                        if (inv == null)
                            isNotexist = true;
                    }
                    val1 = val2;
                }
            }
            return val1;
        }
        #endregion
        #region Attachment_Rest_client_call
        public void SaveTailsAttachments(long CompanyId, string path, string usercreated, List<TailsModel> lsttailsattachments)
        {
            Tails tails = new Tails();
            tails.FileShareName = CompanyId;
            tails.CompanyId = CompanyId;
            tails.Path = path;
            tails.LstTailsModel = lsttailsattachments;
            tails.CursorName = DocumentConstants.CursorName;
            if (tails.LstTailsModel.Count() > 0)
            {
                var json = RestHelper.ConvertObjectToJason(tails);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureUrl"];
                    var response = RestSharpHelper.Post(url, "api/storage/tailsaddmodesave", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<Tails>(response.Content);
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                }
            }
        }
        #endregion
    }
}

