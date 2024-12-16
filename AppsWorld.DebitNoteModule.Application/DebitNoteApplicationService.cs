using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Service;
using AppsWorld.DebitNoteModule.Service;
using AppsWorld.DebitNoteModule.Models;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using AppsWorld.CommonModule.Infra;
using Serilog;
using Logger;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Entities;
using AppsWorld.DebitNoteModule.Infra;
using System.Data.Entity.Validation;
using Domain.Events;
using Repository.Pattern.Infrastructure;
using System.Configuration;
using AppsWorld.CommonModule.Models;
using Ziraff.Section;
using AppsWorld.DebitNoteModule.Infra.Resources;
using System.Data.SqlClient;
using System.Data;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AppsWorld.DebitNoteModule.Application
{
    public class DebitNoteApplicationService
    {
        private readonly IDebitNoteService _debitNoteService;
        private readonly IDebitNoteNoteService _debitNoteNoteService;
        private readonly IDebitNoteDetailService _debitNoteDetailService;
        private readonly ICurrencyService _currencyService;
        //private readonly IControlCodeCategoryService _controlCodeCatService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ITaxCodeService _taxCodeService;
        private readonly ICompanyService _companyService;
        //private readonly ISegmentMasterService _segmentMasterService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly ICompanySettingService _companySettingService;
        private readonly IBeanEntityService _beanEntityService;
        //private readonly IDebitNoteGstDetailService _gstDetailService;
        //private readonly IGSTSettingService _gstSettingService;
        //private readonly IForexService _forexService;
        //private readonly IMultiCurrencySettingService _multiCurSettingService;
        private readonly ITermsOfPaymentService _termsOfPaymentService;
        private readonly IDebitNoteModuleUnitOfWorkAsync _unitOfWork;
        private readonly AppsWorld.DebitNoteModule.Service.IAutoNumberService _autoNumberService;
        private readonly AppsWorld.DebitNoteModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        //private readonly IProvisionService _provisionService;
        private readonly IInvoiceService _invoiceService;
        //private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IJournalService _journalService;
        //private readonly ICreditNoteApplicationDetailService _creditNoteApplicationDetailService;
        //private readonly ICreditNoteApplicationService _creditNoteApplicationService;
        //private readonly IDoubtfulDebtallocationDetailService _doubtfulDebtallocationDetailService;
        //private readonly IDoubtfulDebtAllocationService _doubtfulDebtAllocationService;
        //private readonly IReceiptDetailService _receiptDetailService;
        private readonly IReceiptService _receiptService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        SqlConnection con = null;
        SqlDataReader dr = null;
        SqlCommand cmd = null;
        string query = string.Empty;


        #region Constructor Region
        public DebitNoteApplicationService(IDebitNoteService debitNoteService, IDebitNoteNoteService debitNoteNoteService, IDebitNoteDetailService debitNoteDetailService, ICurrencyService currencyService/*, IControlCodeCategoryService controlCodeCatService*/, IChartOfAccountService chartOfAccountService, ITaxCodeService taxCodeService, ICompanyService companyService/*, ISegmentMasterService segmentMasterService*/, IFinancialSettingService financialSettingService, ICompanySettingService companySettingService, IBeanEntityService beanEntityService/*, IDebitNoteGstDetailService gstDetailService, IGSTSettingService gstSettingService*//*, IForexService forexService, IMultiCurrencySettingService multiCurSettingService*/, ITermsOfPaymentService termsOfPaymentService, IDebitNoteModuleUnitOfWorkAsync unitOfWork, AppsWorld.DebitNoteModule.Service.AutoNumberService autoNumberService, AppsWorld.DebitNoteModule.Service.IAutoNumberCompanyService autoNumberCompanyService,/* IProvisionService provisionService,*/ IJournalService journalService, IInvoiceService invoiceService/*, IInvoiceDetailService invoiceDetailService*//*, IJournalService journalService, ICreditNoteApplicationDetailService creditNoteApplicationDetailService, ICreditNoteApplicationService creditNoteApplicationService, IDoubtfulDebtallocationDetailService doubtfulDebtallocationDetailService, IDoubtfulDebtAllocationService doubtfulDebtAllocationService, IReceiptDetailService receiptDetailService*/, IReceiptService receiptService, IAccountTypeService accountTypeService, AppsWorld.CommonModule.Service.IAutoNumberService autoService)
        {
            this._debitNoteService = debitNoteService;
            this._debitNoteNoteService = debitNoteNoteService;
            this._debitNoteDetailService = debitNoteDetailService;
            this._currencyService = currencyService;
            //this._controlCodeCatService = controlCodeCatService;
            this._chartOfAccountService = chartOfAccountService;
            this._taxCodeService = taxCodeService;
            this._companyService = companyService;
            //this._segmentMasterService = segmentMasterService;
            this._financialSettingService = financialSettingService;
            this._companySettingService = companySettingService;
            this._beanEntityService = beanEntityService;
            //this._gstDetailService = gstDetailService;
            //this._gstSettingService = gstSettingService;
            //this._forexService = forexService;
            //this._multiCurSettingService = multiCurSettingService;
            this._termsOfPaymentService = termsOfPaymentService;
            this._unitOfWork = unitOfWork;
            this._autoNumberService = autoNumberService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            //this._provisionService = provisionService;
            this._invoiceService = invoiceService;
            //this._invoiceDetailService = invoiceDetailService;
            this._journalService = journalService;
            //this._creditNoteApplicationDetailService = creditNoteApplicationDetailService;
            //this._creditNoteApplicationService = creditNoteApplicationService;
            //this._doubtfulDebtallocationDetailService = doubtfulDebtallocationDetailService;
            //this._doubtfulDebtAllocationService = doubtfulDebtAllocationService;
            //this._receiptDetailService = receiptDetailService;
            this._receiptService = receiptService;
            this._accountTypeService = accountTypeService;
            this._autoService = autoService;
        }
        #endregion

        #region Create and Lookup Call
        public DebitNoteModelLU GetDebitNoteAllLUs(string username, Guid debitNoteId, long companyId)
        {
            DebitNoteModelLU debitNoteLU = new DebitNoteModelLU();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteAllLUs_GetCall_Request_Message);
                DebitNote lastDebitNote = _debitNoteService.CreateDebitNote(companyId);
                DebitNote debitNote = _debitNoteService.GetDebitNoteById(debitNoteId, companyId);
                DateTime date = debitNote == null ? lastDebitNote == null ? DateTime.Now : lastDebitNote.DocDate : debitNote.DocDate;
                //List<DebitNoteDetail> lstDebitNotedetails = _debitNoteDetailService.GetAllDebitNoteDetail(debitNoteId);
                //debitNoteLU.NatureLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_Nature);
                debitNoteLU.NatureLU = new List<string> { "Trade", "Others" };
                //debitNoteLU.AllowableNonAllowableLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                debitNoteLU.CompanyId = companyId;
                if (debitNote != null)
                {
                    //string currencyCode = debitNote.DocCurrency;
                    debitNoteLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, debitNote.DocCurrency, ControlCodeConstants.Currency_DefaultCode);
                    //var lookUpNature = _controlCodeCatService.GetInactiveControlcode(companyId,
                    //      ControlCodeConstants.Control_codes_VendorType, debitNote.Nature);
                    //if (lookUpNature != null)
                    //{
                    //    debitNoteLU.NatureLU.Lookups.Add(lookUpNature);
                    //}

                }
                else
                {
                    debitNoteLU.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
                }
                #region Segment_reporting_Commented_code
                //List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
                //if (debitNote == null)
                //{
                //    if (segments.Count > 0)
                //        debitNoteLU.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        debitNoteLU.SegmentCategory2LU = segments[1];
                //}
                //else
                //{
                //    if (debitNote.SegmentMasterid1 != null)
                //        debitNoteLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyId, debitNote.SegmentMasterid1);
                //    else
                //        if (segments.Count > 0)
                //        debitNoteLU.SegmentCategory1LU = segments[0];
                //    if (debitNote.SegmentMasterid2 != null)
                //        debitNoteLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyId, debitNote.SegmentMasterid2);
                //    else
                //        if (segments.Count > 1)
                //        debitNoteLU.SegmentCategory2LU = segments[1];
                //}
                #endregion Segment_reporting_Commented_code

                long credit = debitNote == null ? 0 : debitNote.CreditTermsId;
                debitNoteLU.TermsOfPaymentLU = _termsOfPaymentService.Queryable()/*.AsEnumerable()*/.Where(a => (a.Status == RecordStatusEnum.Active || a.Id == credit) && a.CompanyId == companyId && a.IsCustomer == true).Select(x => new LookUp<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    TOPValue = x.TOPValue,
                    RecOrder = x.RecOrder
                }).OrderBy(a => a.TOPValue).ToList();

                //if (lstDebitNotedetails.Count > 0)
                //{
                //    foreach (var debitNoteDetail in lstDebitNotedetails)
                //    {
                //        long? coa = debitNoteDetail == null ? 0 : debitNoteDetail.COAId;
                //        debitNoteLU.ChartOfAccountLU = _chartOfAccountService.ListCOADetail(companyId, true);
                //    }
                //    debitNoteLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyId);
                //}
                //else
                //{
                //debitNoteLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, false, companyId);
                //debitNoteLU.ChartOfAccountLU = _chartOfAccountService.ListCOADetail(companyId, false);
                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                List<AccountType> accType = _accountTypeService.GetAllAccountTypeNameByCompanyId(companyId, coaName);
                debitNoteLU.ChartOfAccountLU = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                })).OrderBy(x => x.Name).ToList();

                //for observation
                //List<COALookup<string>> emptyCoa = new List<COALookup<string>> { new COALookup<string>() { Name = "Select Option", Id = 0 } };
                //debitNoteLU.ChartOfAccountLU.AddRange(emptyCoa);



                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    debitNoteLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (debitNote != null && debitNote.DebitNoteDetails.Count > 0)
                {
                    List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
                    if (debitNoteLU.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(debitNoteLU.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId).ToList();
                    if (debitNoteLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(debitNoteLU.TaxCodeLU.Select(d => d.Id)).ToList();
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
                        lstEditCoa = accType.SelectMany(r => r.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        }).OrderBy(d => d.Name).ToList()).ToList();
                        debitNoteLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                    if (taxIds.Any())
                    {
                        lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                        }).OrderBy(c => c.Code).ToList();
                        debitNoteLU.TaxCodeLU.AddRange(lstEditTax);
                        debitNoteLU.TaxCodeLU = debitNoteLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                    #region commnted_Code
                    //List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
                    //List<long> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId.Value).ToList();

                    ////creditLU.ChartOfAccountLU.Where(c => c.Id == CoaIds.Contains());
                    //if (CoaIds.Any())
                    //{
                    //    List<long> lstcoaId = debitNoteLU.ChartOfAccountLU.Select(c => c.Id).ToList().Intersect(CoaIds.ToList()).ToList();
                    //    var coalst = lstcoaId.Except(debitNoteLU.ChartOfAccountLU.Select(x => x.Id));
                    //    if (coalst.Any())
                    //    {
                    //        lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => coalst.Contains(x.Id)).Select(x => new COALookup<string>()
                    //        {
                    //            Name = x.Name,
                    //            Id = x.Id,
                    //            RecOrder = x.RecOrder,
                    //            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    //            IsPLAccount = x.Category == "Income Statement" ? true : false,
                    //            Class = c.Class
                    //        }).ToList()).ToList();
                    //        debitNoteLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    //    }
                    //}


                    ////var common = creditLU.ChartOfAccountLU.Intersect(lstEditCoa.Select(x=>x.Id));


                    //if (taxIds.Any())
                    //{
                    //    List<long> lsttaxId = debitNoteLU.TaxCodeLU.Select(d => d.Id).ToList().Intersect(taxIds.ToList()).ToList();
                    //    var taxlst = lsttaxId.Except(debitNoteLU.TaxCodeLU.Select(x => x.Id));
                    //    if (taxlst.Any())
                    //    {
                    //        lstEditTax = allTaxCodes.Where(c => taxlst.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                    //        {
                    //            Id = x.Id,
                    //            Code = x.Code,
                    //            Name = x.Name,
                    //            TaxRate = x.TaxRate,
                    //            TaxType = x.TaxType,
                    //            Status = x.Status,
                    //            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                    //        }).OrderBy(c => c.Code).ToList();
                    //        debitNoteLU.TaxCodeLU.AddRange(lstEditTax);
                    //    }
                    //}
                    //List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
                    //List<long?> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId).ToList();
                    //if (CoaIds.Any())
                    //    //lstEditCoa = _chartOfAccountService.GetAllCOAById(companyid, CoaIds).Select(x => new COALookup<string>()
                    //    //{
                    //    //    Name = x.Name,
                    //    //    Id = x.Id,
                    //    //    RecOrder = x.RecOrder,
                    //    //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    //    //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    //    //    Class = x.Class
                    //    //}).ToList();
                    //    lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                    //    {
                    //        Name = x.Name,
                    //        Id = x.Id,
                    //        RecOrder = x.RecOrder,
                    //        IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    //        IsPLAccount = x.Category == "Income Statement" ? true : false,
                    //        Class = c.Class
                    //    }).ToList().OrderBy(d => d.Name)).ToList();
                    //debitNoteLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    //if (taxIds.Any())
                    //{
                    //    lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                    //    {
                    //        Id = x.Id,
                    //        Code = x.Code,
                    //        Name = x.Name,
                    //        TaxRate = x.TaxRate,
                    //        TaxType = x.TaxType,
                    //        Status = x.Status,
                    //        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                    //    }).OrderBy(c => c.Code).ToList();
                    //    debitNoteLU.TaxCodeLU.AddRange(lstEditTax);
                    //}
                    #endregion

                }
                //}
                //long comp = debitNote == null ? 0 : debitNote.CompanyId;
                long? comp = debitNote == null ? 0 : debitNote.ServiceCompanyId == null ? 0 : debitNote.ServiceCompanyId;
                debitNoteLU.SubsideryCompanyLU = _companyService.GetSubCompany(username, companyId, comp);
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteAllLUs_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return debitNoteLU;
        }

        public DebitNoteModelLU NewGetDebitNoteAllLUs(string username, Guid debitNoteId, long companyId, string connectionString)
        {
            DebitNoteModelLU debitNoteLU = new DebitNoteModelLU();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteAllLUs_GetCall_Request_Message);
                DebitNote lastDebitNote = _debitNoteService.CreateDebitNote(companyId);
                DebitNote debitNote = _debitNoteService.GetDebitNoteById(debitNoteId, companyId);
                DateTime date = debitNote == null ? lastDebitNote == null ? DateTime.Now : lastDebitNote.DocDate : debitNote.DocDate;
                debitNoteLU.NatureLU = new List<string> { "Trade", "Others","Interco" };
                debitNoteLU.CompanyId = companyId;
                long credit = debitNote == null ? 0 : debitNote.CreditTermsId;
                long? comp = debitNote == null ? 0 : debitNote.ServiceCompanyId == null ? 0 : debitNote.ServiceCompanyId;
                LookUpCategory<string> currency = new LookUpCategory<string>();
                string currencyCode = debitNote != null ? debitNote.DocCurrency : string.Empty;
                List<CommonLookUps<string>> lstLookUps = new List<CommonLookUps<string>>();
                List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                string accountTypes = string.Join(",", coaName);
                query = DebitNoteCommonQuery(username, companyId, date, credit, comp, currencyCode, debitNoteId, accountTypes);
                int? resultsetCount = query.Split(';').Count();
                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);


                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    for (int i = 1; i <= resultsetCount; i++)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                lstLookUps.Add(new CommonLookUps<string>
                                {
                                    TableName = dr["TABLENAME"].ToString(),
                                    Code = dr["CODE"].ToString(),
                                    Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0,
                                    Name = dr["NAME"].ToString(),
                                    RecOrder = dr["RECORDER"] != DBNull.Value ? Convert.ToInt32(dr["RECORDER"]) : (int?)null,
                                    TaxRate = dr["TAXRATE"] != DBNull.Value ? Convert.ToDouble(dr["TAXRATE"]) : (double?)null,
                                    TaxType = dr["TAXTYPE"].ToString(),
                                    TaxCode = dr["TXCODE"].ToString(),
                                    TOPValue = dr["TOPVALUE"] != DBNull.Value ? Convert.ToDouble(dr["TOPVALUE"]) : (double?)null,
                                    Currency = dr["CURRENCY"].ToString(),
                                    Class = dr["Class"].ToString(),
                                    isGstActivated = dr["IsGstActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstActive"]) : (bool?)null,
                                    ShortName = dr["SHOTNAME"].ToString(),
                                    //ServiceCompanyId = row["Id"] != DBNull.Value ? Convert.ToInt64(row["Id"]) : (long?)null,
                                    DefaultValue = "SGD",
                                    CategoryName = "SGD",
                                    Status = (RecordStatusEnum)dr["STATUS"],
                                    IsInterCo = dr["IsInterCo"] != DBNull.Value ? Convert.ToBoolean(dr["IsInterCo"]) : (bool?)null
                                });
                            }
                        }
                        dr.NextResult();
                    }
                    con.Close();
                }
                if (lstLookUps.Any())
                {
                    currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                    currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                    currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYEDIT").Select(c => new LookUp<string>()
                    {
                        Code = c.Code,
                        Name = c.Name,
                        RecOrder = c.RecOrder
                    }).ToList();
                    debitNoteLU.CurrencyLU = currency;
                    
                    debitNoteLU.TermsOfPaymentLU = lstLookUps.Where(c => c.TableName == "TERMSOFPAY").Select(x => new LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();
                    debitNoteLU.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.isGstActivated,
                        IsIBServiceEntity = x.IsInterCo
                    }).ToList();
                }
                List<COALookup<string>> lstEditCoa = new List<COALookup<string>>();
                //List<TaxCodeLookUp<string>> lstEditTax = null;
                if (debitNoteLU != null)
                {
                   
                        debitNoteLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT").Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                            IsInterCoBillingCOA = x.IsInterCo,
                        }).OrderBy(d => d.Name).ToList();
                }
                else
                    debitNoteLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT").Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Code = x.Code,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsPLAccount = x.COACategory == "Income Statement" ? true : false,
                        Class = x.Class,
                        Status = x.Status,
                        IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        IsInterCoBillingCOA = x.IsInterCo,
                    }).OrderBy(d => d.Name).ToList();

                debitNoteLU.TaxCodeLU = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.Id,
                    Code = x.TaxCode,
                    Name = x.Name,
                    TaxRate = x.TaxRate,
                    TaxType = x.TaxType,
                    Status = x.Status,
                    TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.TaxCode,
                    IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                }).OrderBy(c => c.Code).ToList();

                if (debitNote != null)
                {
                    var lsttax = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Inactive).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.TaxCode,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.TaxCode,
                        IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                    }).OrderBy(c => c.Code).ToList();
                    List<long?> taxIdss = debitNote.DebitNoteDetails.Select(x => x.TaxId).ToList();
                    if (debitNoteLU.TaxCodeLU.Any())
                        taxIdss = taxIdss.Except(debitNoteLU.TaxCodeLU.Select(d => d.Id)).ToList();
                    if (taxIdss.Any())
                    {
                        var lstTax = lsttax.Where(c => taxIdss.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.Code,
                            IsInterCoBillingTaxCode = x.Code == "NA" ? true : x.IsInterCoBillingTaxCode
                        }).OrderBy(c => c.Code).ToList();
                        debitNoteLU.TaxCodeLU.AddRange(lstTax);
                        var data = debitNoteLU.TaxCodeLU;
                        debitNoteLU.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                    }
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteAllLUs_GetCall_SuccessFully_Message);
            }


            
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return debitNoteLU;
        }

        public DebitNoteModel CreateDebitNote(long companyid, Guid id, bool isCopy, string connectionString)
        {
            DebitNoteModel debitNoteModel = new DebitNoteModel();
            try
            {

                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDebitNote_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyid);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                debitNoteModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                debitNoteModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                DebitNote debitNote = _debitNoteService.GetDebitNote(id, companyid);
                //lastcreated Debit_note
                DateTime? date = _debitNoteService.GetDNLastPostedDate(companyid);
                //debitNoteModel.NatureLU = new List<string> { "Trade", "Others" };
                //debitNoteModel.IsSegmentActive1 = true;
                //debitNoteModel.IsSegmentActive2 = true;
                if (debitNote == null)
                {
                    //AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyid, DocTypeConstants.DebitNote);
                    FillNewDebitNoteModel(debitNoteModel, financSettings, companyid/*, _autoNo*/);
                    //debitNoteModel.CurrencyLU = _currencyService.GetByCurrencies(companyid, ControlCodeConstants.Currency_DefaultCode);

                    debitNoteModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.DebitNote);
                    if (debitNoteModel.IsDocNoEditable == true)
                        debitNoteModel.DocNo = _autoService.GetAutonumber(companyid, DocTypeConstants.DebitNote, connectionString);
                    debitNoteModel.IsLocked = false;
                    //bool? isEdit = false;
                    //dtoModel.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice, DocTypeConstants.DebitNote, _autoNo, null, ref isEdit);
                    //dtoModel.IsDocNoEditable = isEdit;


                }
                else
                {
                    FillViewModel(debitNoteModel, debitNote,isCopy);
                    debitNoteModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.DebitNote);
                    if (isCopy)
                        debitNoteModel.DocNo = isCopy && debitNoteModel.IsDocNoEditable == true ? _autoService.GetAutonumber(companyid, DocTypeConstants.DebitNote, connectionString):null;
                    //debitNoteModel.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyid, debitNote.DocCurrency, ControlCodeConstants.Currency_DefaultCode);

                    //if (invoice.DocumentState != DebitNoteStates.Void)
                    //{
                    //    Journal journal = _journalService.GetJournal(companyid, id);
                    //    if (journal != null)
                    //        debitNoteModel.JournalId = journal.Id;
                    //}
                }
                #region LooUP_call
                //for LookUP call
                //long credit = debitNote == null ? 0 : debitNote.CreditTermsId;
                //debitNoteModel.TermsOfPaymentLU = _termsOfPaymentService.Queryable()/*.AsEnumerable()*/.Where(a => (a.Status == RecordStatusEnum.Active || a.Id == credit) && a.CompanyId == companyid && a.IsCustomer == true).Select(x => new LookUp<string>()
                //{
                //    Name = x.Name,
                //    Id = x.Id,
                //    TOPValue = x.TOPValue,
                //    RecOrder = x.RecOrder
                //}).OrderBy(a => a.TOPValue).ToList();
                //List<COALookup<string>> lstEditCoa = null;
                //List<TaxCodeLookUp<string>> lstEditTax = null;
                //List<string> coaName = new List<string> { COANameConstants.Revenue };
                //List<AccountType> accType = _accountTypeService.GetAllAccounyTypeByName(companyid, coaName);
                //debitNoteModel.ChartOfAccountLU = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                //{
                //    Name = x.Name,
                //    Id = x.Id,
                //    RecOrder = x.RecOrder,
                //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                //    Class = c.Class,
                //    Status = c.Status
                //}).ToList().OrderBy(d => d.Name)).ToList();
                //List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(0, date);
                //if (allTaxCodes.Any())
                //    debitNoteModel.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                //    {
                //        Id = x.Id,
                //        Code = x.Code,
                //        Name = x.Name,
                //        TaxRate = x.TaxRate,
                //        TaxType = x.TaxType,
                //        Status = x.Status,
                //        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                //    }).OrderBy(c => c.Code).ToList();
                //if (debitNote != null && debitNote.DebitNoteDetails.Count > 0)
                //{
                //    List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
                //    if (debitNoteModel.ChartOfAccountLU.Any())
                //        CoaIds = CoaIds.Except(debitNoteModel.ChartOfAccountLU.Select(x => x.Id)).ToList();
                //    List<long?> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId).ToList();
                //    if (debitNoteModel.TaxCodeLU.Any())
                //        taxIds = taxIds.Except(debitNoteModel.TaxCodeLU.Select(d => d.Id)).ToList();
                //    if (CoaIds.Any())
                //    {
                //        //lstEditCoa = _chartOfAccountService.GetAllCOAById(companyid, CoaIds).Select(x => new COALookup<string>()
                //        //{
                //        //    Name = x.Name,
                //        //    Id = x.Id,
                //        //    RecOrder = x.RecOrder,
                //        //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //        //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                //        //    Class = x.Class
                //        //}).ToList();
                //        lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                //        {
                //            Name = x.Name,
                //            Id = x.Id,
                //            RecOrder = x.RecOrder,
                //            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //            IsPLAccount = x.Category == "Income Statement" ? true : false,
                //            Class = c.Class,
                //            Status = c.Status
                //        }).ToList().OrderBy(d => d.Name)).ToList();
                //        debitNoteModel.ChartOfAccountLU.AddRange(lstEditCoa);
                //    }
                //    if (taxIds.Any())
                //    {
                //        lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                //        {
                //            Id = x.Id,
                //            Code = x.Code,
                //            Name = x.Name,
                //            TaxRate = x.TaxRate,
                //            TaxType = x.TaxType,
                //            Status = x.Status,
                //            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                //        }).OrderBy(c => c.Code).ToList();
                //        debitNoteModel.TaxCodeLU.AddRange(lstEditTax);
                //        debitNoteModel.TaxCodeLU = debitNoteModel.TaxCodeLU.OrderBy(c => c.Code).ToList();
                //    }
                #endregion


                #region commented_Code
                //debitNoteModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyid);
                //debitNoteModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyid);
                //debitNoteModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyid);
                //debitNoteModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyid);

                //var gst = _gstSettingService.GetByCompanyId(companyid);
                //if (gst != null)
                //    debitNoteModel.DeRegistrationDate = gst.DeRegistration;
                #endregion

                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDebitNote_CreateCall_SuccessFully_Message);
            }

            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return debitNoteModel;
        }
        public DebitNoteDetail GetDebitNoteDetail(Guid id, Guid detailId)
        {
            DebitNoteDetail detail = _debitNoteDetailService.GetDebitNoteDetail(id, detailId);
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteDetail_GetCall_Request_Message);
                if (detail == null)
                {
                    detail = new DebitNoteDetail();
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteDetail_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return detail;
        }
        public DebitNoteNote GetDebitNoteNote(Guid id, Guid noteId)
        {
            DebitNoteNote note = _debitNoteNoteService.GetDebitNoteNote(id, noteId);
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteNote_GetCall_Request_Message);
                if (note == null)
                    note = new DebitNoteNote();
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteNote_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return note;
        }
        public AppsWorld.DebitNoteModule.Models.DocumentVoidModel CreateDebitNoteDocumentVoid(Guid id, long companyId)
        {
            AppsWorld.DebitNoteModule.Models.DocumentVoidModel DVModel = new AppsWorld.DebitNoteModule.Models.DocumentVoidModel();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDebitNoteDocumentVoid_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);

                DebitNote debitNote = _debitNoteService.GetDebitNoteById(id, companyId);
                if (debitNote == null)
                    throw new Exception(DebitNoteValidation.Invalid_DebitNote);

                if (debitNote != null)
                {
                    DVModel.CompanyId = companyId;
                    DVModel.Id = debitNote.Id;
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDebitNoteDocumentVoid_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return DVModel;
        }
        public CreditNoteModel CreateCreditNoteByDebitNote(Guid debitNoteId, long companyId)
        {
            CreditNoteModel invDTO = new CreditNoteModel();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message);
                AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CreditNote);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(DebitNoteValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice lastCreditNote = _invoiceService.GetInvoiceByCompany(companyId, DocTypeConstants.CreditNote);
                DebitNote debitNote = _debitNoteService.GetDebitNote(debitNoteId, companyId);

                FillCreditNoteByDebitNote(invDTO, debitNote);
                invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, invDTO.CompanyId);

                bool? isEdit = false;
                invDTO.DocNo = GetAutoNumberByEntityType(companyId, debitNote, DocTypeConstants.CreditNote, _autoNo, lastCreditNote, ref isEdit);
                invDTO.IsDocNoEditable = isEdit;

                List<InvoiceDetail> lstInvDetail = new List<InvoiceDetail>();
                if (debitNote.DebitNoteDetails.Any())
                {
                    foreach (var detail in debitNote.DebitNoteDetails)
                    {
                        InvoiceDetail cnDetail = new InvoiceDetail();
                        FillInvoiceDetailV(cnDetail, detail, invDTO);
                        lstInvDetail.Add(cnDetail);
                    }
                }
                invDTO.InvoiceDetails = lstInvDetail;
                //List<InvoiceGSTDetail> lstInvGstDetail = new List<InvoiceGSTDetail>();
                //if (debitNote.DebitNoteGSTDetails.Any())
                //{
                //    foreach (var gstDetail in debitNote.DebitNoteGSTDetails)
                //    {
                //        InvoiceGSTDetail invoiceGSTDetail = new InvoiceGSTDetail();
                //        invoiceGSTDetail.Id = Guid.NewGuid();
                //        invoiceGSTDetail.InvoiceId = invDTO.Id;
                //        invoiceGSTDetail.TaxId = gstDetail.TaxId;
                //        invoiceGSTDetail.Amount = gstDetail.Amount;
                //        invoiceGSTDetail.TaxAmount = gstDetail.TaxAmount;
                //        invoiceGSTDetail.TotalAmount = gstDetail.TotalAmount;

                //        lstInvGstDetail.Add(invoiceGSTDetail);
                //    }
                //}
                //invDTO.InvoiceGSTDetails = lstInvGstDetail;

                CreditNoteApplicationModel CNAModel = new CreditNoteApplicationModel();
                FillCreditNoteApplication(CNAModel, debitNote, invDTO);
                invDTO.CreditNoteApplicationModel = CNAModel;

                List<CreditNoteApplicationDetailModel> lstCNADModel = new List<CreditNoteApplicationDetailModel>();
                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();

                FillCreditNoteApplicationDetail(detailModel, debitNote, CNAModel, invDTO);
                lstCNADModel.Add(detailModel);

                invDTO.CreditNoteApplicationModel.CreditNoteApplicationDetailModels = lstCNADModel;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateCreditNoteByDebitNote_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return invDTO;
        }
        public DoubtfulDebtModel CreateDoubtFulDebtByDebitNote(Guid debitNoteId, long companyId)
        {
            DoubtfulDebtModel invDTO = new DoubtfulDebtModel();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_Request_Message);
                AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.DoubtFulDebitNote);
                Invoice lastInvoice = _invoiceService.GetInvoiceByCompany(companyId, DocTypeConstants.CreditNote);
                DebitNote debitNote = _debitNoteService.GetDebitNote(debitNoteId, companyId);
                FillDoubtfulDebt(invDTO, debitNote);

                //invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.DoubtFulDebitNote, invDTO.CompanyId);

                bool? isEdit = false;
                invDTO.DocNo = GetAutoNumberByEntityType(companyId, debitNote, DocTypeConstants.DoubtFulDebitNote, _autoNo, lastInvoice, ref isEdit);
                invDTO.IsDocNoEditable = isEdit;

                DoubtfulDebtAllocationModel DDAModel = new DoubtfulDebtAllocationModel();
                FillDoubtfulDebtAllocation(DDAModel, debitNote, invDTO);
                invDTO.DoubtfulDebtAllocation = DDAModel;

                List<DoubtfulDebtAllocationDetailModel> lstDDAD = new List<DoubtfulDebtAllocationDetailModel>();
                DoubtfulDebtAllocationDetailModel dDAD = new DoubtfulDebtAllocationDetailModel();
                FillDoubtfulDebtAllocationDetail(dDAD, debitNote, DDAModel);
                lstDDAD.Add(dDAD);

                invDTO.DoubtfulDebtAllocation.DoubtfulDebtAllocationDetailModels = lstDDAD;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return invDTO;
        }
        //public ProvisionModel GetProvision(Guid id, long companyId)
        //{
        //    ProvisionModel provisionModel = new ProvisionModel();
        //    provisionModel.DocumentDate = DateTime.UtcNow;
        //    provisionModel.DocNo = GetNewProvisionDocumentNumber(companyId);
        //    provisionModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
        //    return provisionModel;
        //}
        public ReceiptModel CreateReceiptByDebitNote(Guid debitNoteId, long companyId, string connectionString)
        {
            ReceiptModel _model = new ReceiptModel();
            try
            {
                //to check if it is void or not
                if (_debitNoteService.IsVoid(companyId, debitNoteId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                Receipt lastReceipt = _receiptService.GetLastCreatedReceipt(companyId);
                //AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.Receipt);
                AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(DebitNoteValidation.The_Financial_setting_should_be_activated);
                }
                _model.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                _model.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                DebitNote debitNote = _debitNoteService.GetDebitNote(debitNoteId, companyId);
                _model.Id = Guid.NewGuid();
                _model.CompanyId = debitNote.CompanyId;
                //_model.EntityModels.EntityType = debitNote.EntityType;
                _model.DocSubType = DocTypeConstants.Receipt;
                // _model.DocNo = GetNewReceiptDocNo(DocTypeConstants.ReceiptDoc, companyId);

                //bool? isEdit = false;
                //_model.DocNo = GetAutoNumberForReceipt(companyId, lastReceipt, DocTypeConstants.Receipt, _autoNo, ref isEdit);
                //_model.IsDocNoEditable = isEdit;
                _model.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.Receipt);
                if (_model.IsDocNoEditable == true)
                    _model.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.Receipt, connectionString);

                _model.DocDate = debitNote.DocDate;
                //_model.DueDate = debitNote.DueDate;
                _model.EntityId = debitNote.EntityId;
                _model.CreditTermId = debitNote.CreditTermsId;
                _model.DocCurrency = debitNote.DocCurrency;
                //var company = _companyService.GetById(debitNote.ServiceCompanyId.Value);
                if (debitNote.ServiceCompanyId != null)
                {
                    _model.ServiceCompanyId = debitNote.ServiceCompanyId.Value;
                    //if (company != null)
                    //    _model.ServiceCompanyName = company.ShortName;
                }
                BeanEntity beanEntity = _beanEntityService.GetEntityById(debitNote.EntityId);
                _model.EntityName = beanEntity.Name;
                // _model.CustCreditlimit = beanEntity.CustCreditLimit;
                //_model.IsGSTApplied = debitNote.IsGSTApplied;
                _model.ISMultiCurrency = debitNote.IsMultiCurrency;
                _model.BaseCurrency = debitNote.ExCurrency;
                _model.ExchangeRate = debitNote.ExchangeRate;
                _model.ExDurationFrom = debitNote.ExDurationFrom;
                //_model.ExDurationTo = debitNote.ExDurationTo;
                _model.IsGstSettings = debitNote.IsGstSettings;
                _model.Remarks = debitNote.Remarks;
                //_model.BankReceiptAmmount = debitNote.BalanceAmount;
                _model.GSTTotalAmount = debitNote.GSTTotalAmount;
                _model.GstExchangeRate = debitNote.GSTExchangeRate;
                _model.DocumentState = ReceiptState.Posted;
                //_model.GrandTotal = debitNote.GrandTotal;
                _model.GstReportingCurrency = debitNote.GSTExCurrency;
                //_model.IsAllowDisAllow = debitNote.IsAllowableNonAllowable;
                _model.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
                _model.NoSupportingDocument = debitNote.NoSupportingDocs;
                _model.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
                _model.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
                _model.Status = debitNote.Status;
                _model.ExtensionType = ExtensionType.DebitNote;
                _model.DocumentState = debitNote.DocumentState;
                _model.ModifiedDate = debitNote.ModifiedDate;
                _model.ModifiedBy = debitNote.ModifiedBy;
                _model.CreatedDate = debitNote.CreatedDate;
                _model.UserCreated = debitNote.UserCreated;
                _model.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
                _model.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
                //_model.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(_model.CompanyId);
                //_model.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(_model.CompanyId);
                //_model.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(_model.CompanyId);
                //_model.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(_model.CompanyId);

                List<ReceiptDetailModel> lstDetail = new List<ReceiptDetailModel>();
                ReceiptDetailModel detail = new ReceiptDetailModel();
                detail.Id = Guid.NewGuid();
                detail.ReceiptId = _model.Id;
                detail.DocumentId = debitNote.Id;
                detail.DocumentDate = debitNote.DocDate;
                detail.DocumentType = debitNote.DocSubType;
                detail.DocumentNo = debitNote.DocNo;
                detail.SystemReferenceNumber = debitNote.DebitNoteNumber;
                detail.DocumentState = debitNote.DocumentState;
                detail.Nature = debitNote.Nature;
                detail.DocumentAmmount = debitNote.GrandTotal;
                detail.Currency = debitNote.DocCurrency;
                detail.AmmountDue = debitNote.BalanceAmount;
                detail.ReceiptAmount = debitNote.BalanceAmount;
                //if (company != null)
                //{
                //    detail.ServiceCompanyName = company.ShortName;
                //    detail.ServiceCompanyId = company.Id;
                //}
                detail.ServiceCompanyName = _companyService.GetByName(_model.ServiceCompanyId);
                detail.ServiceCompanyId = _model.ServiceCompanyId;
                lstDetail.Add(detail);
                _model.ReceiptDetailModels = lstDetail;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw ex;
            }
            return _model;
        }


        #endregion

        #region Kendo Call
        public async Task<IQueryable<DebitNoteKModel>> GetAllDebitNotesK(string userName, long companyId)
        {
            return await _debitNoteService.GetAllDebitNotesK(userName, companyId);
        }
        #endregion

        #region Save Call
        private RecordStatusEnum eventStatusChanged;
        public DebitNote SaveDebitNote(DebitNoteModel TObject, string ConnectionString)
        {
			bool isNewAdd = false;
			bool isDocAdd = false;
			try
            {
				var AdditionalInfo = new Dictionary<string, object>();
				AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
				LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, "ObjectSave", AdditionalInfo);
				LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_Save_SaveCall_Request_Message);
				string _errors = CommonValidation.ValidateObject(TObject);
				bool? isExeedLimitEdit = false;
				//bool isNewAdd = false;
				if (!string.IsNullOrEmpty(_errors))
				{
					throw new Exception(_errors);
				}
				if (_debitNoteService.IsVoid(TObject.CompanyId, TObject.Id))
					throw new Exception(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);
				if (TObject.DocDate == null)
				{
					throw new Exception(CommonConstant.Invalid_Document_Date);
				}

				if (TObject.DueDate == null || TObject.DueDate < TObject.DocDate)
				{
					throw new Exception(DebitNoteValidation.Invalid_Due_Date);
				}
				if (TObject.GrandTotal <= 0)
					throw new Exception(DebitNoteValidation.Debit_Amount_Should_Be_Grater_Than_0);
				if (TObject.CreditTermsId == null || TObject.CreditTermsId == 0)
				{
					throw new Exception(DebitNoteValidation.Terms_of_Payment_Is_Mandatory);
				}
				if (TObject.IsDocNoEditable == true)
				{
					DebitNote _debitNotedoc = _debitNoteService.GetDebitNoteDocNo(TObject.Id, TObject.DocNo, TObject.CompanyId);
					if (_debitNotedoc != null)
					{
						throw new Exception(CommonConstant.Document_number_already_exist);
					}
				}

				if (TObject.DebitNoteDetails == null || TObject.DebitNoteDetails.Count == 0)
				{
					throw new Exception(DebitNoteValidation.Atleast_one_Chart_of_Account_is_required_in_the_Debit_Note);
				}
				else
				{
					int itemCount = TObject.DebitNoteDetails.Where(a => a.RecordStatus != "Deleted").Count();
					if (itemCount == 0)
					{
						throw new Exception(DebitNoteValidation.Atleast_one_Chart_of_Account_is_required_in_the_Debit_Note);
					}
				}

				if (TObject.ExchangeRate == 0)
					throw new Exception(CommonConstant.ExchangeRate_Should_Be_Grater_Than_0);
				if (TObject.GSTExchangeRate == 0)
					throw new Exception(CommonConstant.GSTExchangeRate_Should_Be_Grater_Than_0);

				//Verify if the COAs are duplicated
				//var qryDuplicates = TObject.DebitNoteDetails.Where(a => a.RecordStatus != "Deleted").GroupBy(x => x.COAId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
				//if (qryDuplicates.Count > 0)
				//    throw new Exception(DebitNoteValidation.Chart_of_Accounts_are_duplicated_in_the_Details);

				//Verify if the TaxId is not null
				//if (TObject.IsGstSettings && !TObject.IsGSTDeregistered)
				//{
				//    var qryNullTaxes = TObject.DebitNoteDetails.Where(a => a.TaxId == null).ToList();
				//    if (qryNullTaxes.Count > 0)
				//        throw new Exception(DebitNoteValidation.Tax_Codes_are_not_selected_in_detail);
				//}
				//Need to verify the invoice is within Financial year
				if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate.Date, TObject.CompanyId))
				{
					throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
				}

				//Verify if the invoice is out of open financial period and lock password is entered and valid
				if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
				{
					if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
					{
						throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
					}
					else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
					{
						throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
					}
				}
				DebitNote debitNote = null;
				if (TObject.Nature == "Interco")
				{
					if (TObject.GrandTotal == 0)
						throw new Exception("You can't create an interco Debit Note with '0' Amount.");
					FillDocumentAndDetailType(TObject, ConnectionString);
					debitNote = _debitNoteService.GetDebitNote(TObject.Id, TObject.CompanyId);
				}
				else
				{
					debitNote = _debitNoteService.GetDebitNote(TObject.Id, TObject.CompanyId);
					if (debitNote != null)
					{
						if (debitNote.ExchangeRate != TObject.ExchangeRate)
							debitNote.RoundingAmount = 0;
						decimal? docTotal = debitNote.GrandTotal - TObject.GrandTotal;

						//Data concurancy check
						string timeStamp = "0x" + string.Concat(Array.ConvertAll(debitNote.Version, x => x.ToString("X2")));
						if (!timeStamp.Equals(TObject.Version))
							throw new Exception(CommonConstant.Document_has_been_modified_outside);

						isExeedLimitEdit = true;
						eventStatusChanged = debitNote.Status;
						InsertDebitNote(TObject, debitNote);
						TObject.CustCreditlimit += docTotal;
						debitNote.DocNo = TObject.DocNo;
						debitNote.DebitNoteNumber = TObject.DocNo;
						debitNote.BalanceAmount = debitNote.DocumentState == InvoiceState.NotPaid ? TObject.GrandTotal : debitNote.BalanceAmount;
						debitNote.ModifiedBy = TObject.ModifiedBy;
						debitNote.ModifiedDate = DateTime.UtcNow;
						debitNote.ObjectState = ObjectState.Modified;
						UpdateDebitNoteDetails(TObject, debitNote);
						_debitNoteService.Update(debitNote);
						isNewAdd = false;
					}
					else
					{
						debitNote = new DebitNote();
						isExeedLimitEdit = false;
						InsertDebitNote(TObject, debitNote);
						debitNote.Id = Guid.NewGuid();
						debitNote.BalanceAmount = TObject.GrandTotal;
						int? recorder = 0;
						if (TObject.DebitNoteDetails.Count > 0 || TObject.DebitNoteDetails != null)
						{
							debitNote.DebitNoteDetails = TObject.DebitNoteDetails;
							foreach (DebitNoteDetail detail in debitNote.DebitNoteDetails)
							{
								detail.RecOrder = recorder + 1;
								recorder = detail.RecOrder;
								detail.DebitNoteId = debitNote.Id;
								detail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
								detail.BaseTaxAmount = TObject.ExchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0 : detail.DocTaxAmount;
								detail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
								detail.ObjectState = ObjectState.Added;
							}
						}
						debitNote.Status = RecordStatusEnum.Active;
						debitNote.DocumentState = InvoiceState.NotPaid;
						debitNote.UserCreated = TObject.UserCreated;
						debitNote.CreatedDate = DateTime.UtcNow;
						debitNote.ObjectState = ObjectState.Added;

						debitNote.DebitNoteNumber = TObject.IsDocNoEditable != true ? /*GenerateAutoNumberForType(company.Id, DocTypeConstants.DebitNote, company.ShortName)*/_autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.DebitNote, ConnectionString) : TObject.DocNo;
						debitNote.DocNo = debitNote.DebitNoteNumber;
                        isDocAdd = true;
						_debitNoteService.Insert(debitNote);
						isNewAdd = true;
					}

					try
					{
						_unitOfWork.SaveChanges();
						#region DN_Posting_SP
						AppaWorld.Bean.Common.SavePosting(debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, ConnectionString);
						#endregion DN_Posting_SP

					}
					catch (DbEntityValidationException e)
					{
						LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, e, e.Message);
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
						throw;
					}
				}
				return debitNote;
			}
            catch (Exception ex)
            {
				if (isNewAdd && isDocAdd && TObject.IsDocNoEditable == false)
				{
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocSubType, ConnectionString);
				}
				throw ex;
            }
        }
        public DebitNote SaveDebitNoteDocumentVoid(AppsWorld.DebitNoteModule.Models.DocumentVoidModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_SaveDebitNoteDocumentVoid_SaveCall_Request_Message);
            if (_debitNoteService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.This_transaction_has_already_void);
            bool? isVoid = true;
            DebitNote _document = _debitNoteService.GetDebitNoteById(TObject.Id, TObject.CompanyId);
            if (_document == null)
                throw new Exception(DebitNoteValidation.Invalid_DebitNote);
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }

            if (_document.DocumentState != DebitNoteState.NotPaid)
                throw new Exception("State should be " + DebitNoteState.NotPaid);

            if (_document.DebitNoteDetails.Any(a => a.ClearingState == ReceiptState.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
            {
                throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
            }
            _document.DocumentState = DebitNoteState.Void;
            _document.DocNo = _document.DocNo + "-V";
            _document.ModifiedBy = TObject.ModifiedBy;
            _document.ModifiedDate = DateTime.UtcNow;
            _document.ObjectState = ObjectState.Modified;
            try
            {
                _unitOfWork.SaveChanges();

                #region Interco_Billing_Void
                if (_document.Nature.Equals("Interco"))
                {
                    
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_Insert_Document_History", con);
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                        cmd.Parameters.AddWithValue("@DocumentId", _document.Id);
                        cmd.Parameters.AddWithValue("@DocumentType", DocTypeConstants.Bills);
                        cmd.Parameters.AddWithValue("@IsVoid", true);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                }
                #endregion Interco_Billing_Void

                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = _document.DocNo;
                tObject.ModifiedBy = TObject.ModifiedBy;
                deleteJVPostDebitNote(tObject);

                #region Cust_CreditLimit_Updation
                //decimal? custCreditLimit = _beanEntityService.GetCteditLimitsValue(_document.EntityId);
                //if (custCreditLimit != null)
                //{
                //    SqlConnection con = new SqlConnection(ConnectionString);
                //    if (con.State != ConnectionState.Open)
                //        con.Open();
                //    SqlCommand cmd = new SqlCommand("BC_UPDATE_ENTITY_CREDITTERMS", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@EntityId", _document.EntityId.ToString());
                //    cmd.Parameters.AddWithValue("@BaseAmount", _document.GrandTotal);
                //    cmd.Parameters.AddWithValue("@DocType", _document.DocSubType);
                //    cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                //    cmd.Parameters.AddWithValue("@isEdit", false);
                //    cmd.Parameters.AddWithValue("@isVoid", isVoid);
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                #endregion

                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_SaveDebitNoteDocumentVoid_SaveCalll_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return _document;
        }
        public DebitNoteNote Save(DebitNoteNote note, long companyId)
        {
            string _errors = CommonValidation.ValidateObject(note);

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            DebitNoteNote dnNote = _debitNoteNoteService.GetDebitNoteNote(note.Id, note.DebitNoteId);
            if (dnNote == null)
            {
                throw new Exception(DebitNoteValidation.DebiteNote_Note_is_already_exist);
            }
            else
            {
                note.CreatedDate = DateTime.UtcNow;
                note.ObjectState = ObjectState.Added;
                _debitNoteNoteService.Insert(note);
            }
            try
            {
                _unitOfWork.SaveChanges();

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw new Exception(ex.Message);
            }
            return note;
        }
        #endregion

        #region PriateMethods

        private void FillDocumentAndDetailType(DebitNoteModel debitNoteModel, string connectionString)
        {
            int count = 0;
            List<DocumentTypeModel> lstDocumentType = new List<DocumentTypeModel>();
            DocumentTypeModel documentType = new DocumentTypeModel();
            documentType.Id = debitNoteModel.Id;
            documentType.CompanyId = debitNoteModel.CompanyId;
            documentType.EntityId = debitNoteModel.EntityId;
            documentType.ServiceCompanyId = debitNoteModel.ServiceCompanyId;
            documentType.DocType = DocTypeConstants.DebitNote;
            documentType.DocSubType = DocTypeConstants.Interco;
            documentType.DocNo = debitNoteModel.DocNo;
            documentType.DocumentState = debitNoteModel.GrandTotal == 0 ? InvoiceState.FullyPaid : debitNoteModel.DocumentState;
            documentType.DocCurrency = debitNoteModel.DocCurrency;
            documentType.BaseCurrency = debitNoteModel.BaseCurrency;
            documentType.GSTCurrency = debitNoteModel.GSTExCurrency;
            documentType.PostingDate = debitNoteModel.DocDate;
            documentType.DocDate = debitNoteModel.DocDate;
            documentType.DueDate = debitNoteModel.DueDate;
            documentType.BalanaceAmount = debitNoteModel.GrandTotal == 0 ? 0 : debitNoteModel.BalanceAmount;
            documentType.ExchangeRate = debitNoteModel.ExchangeRate;
            documentType.GSTExchangeRate = debitNoteModel.GSTExchangeRate;
            documentType.CreditTermsId = debitNoteModel.CreditTermsId;
            documentType.DocDescription = debitNoteModel.Remarks;
            documentType.PONo = debitNoteModel.PONo;
            documentType.NoSupportingDocs = debitNoteModel.NoSupportingDocument;
            documentType.Nature = debitNoteModel.Nature;
            documentType.GSTTotalAmount = debitNoteModel.GSTTotalAmount;
            documentType.GrandTotal = debitNoteModel.GrandTotal;
            documentType.IsGstSettings = debitNoteModel.IsGstSettings;
            documentType.IsGSTApplied = debitNoteModel.IsGSTApplied;
            documentType.IsMultiCurrency = debitNoteModel.IsMultiCurrency;
            documentType.IsAllowableNonAllowable = debitNoteModel.IsAllowableNonAllowable;
            documentType.IsNoSupportingDocument = debitNoteModel.IsNoSupportingDocument;
            documentType.IsAllowableDisallowableActivated = debitNoteModel.IsAllowableNonAllowable;
            documentType.Status = true;
            documentType.UserCreated = debitNoteModel.UserCreated;
            documentType.CreatedDate = DateTime.UtcNow;
            documentType.ModifiedBy = debitNoteModel.ModifiedBy;
            documentType.ModifiedDate = debitNoteModel.ModifiedDate;
            documentType.IsBaseCurrencyRateChanged = debitNoteModel.IsBaseCurrencyRateChanged;
            documentType.IsGSTCurrencyRateChanged = debitNoteModel.IsGSTCurrencyRateChanged;
            lstDocumentType.Add(documentType);
            List<DocumentDetailTypeModel> lstDocumentDetailType = debitNoteModel.DebitNoteDetails.Select(a => new DocumentDetailTypeModel
            {
                Id = a.Id,
                ItemId = Guid.Empty,
                ItemCode = string.Empty,
                ItemDescription = a.AccountDescription,
                Qty = 0,
                Unit = string.Empty,
                UnitPrice = 0,
                DiscountType = string.Empty,
                Discount = 0,
                COAId = a.COAId,
                TaxId = a.TaxId,
                TaxRate = a.TaxRate,
                DocTaxAmount = a.DocTaxAmount,
                DocAmount = a.DocAmount,
                DocTotalAmount = a.DocTotalAmount,
                BaseTaxAmount = a.BaseTaxAmount,
                BaseAmount = a.BaseAmount,
                BaseTotalAmount = a.BaseTotalAmount,
                AmtCurrency = string.Empty,
                RecOrder = (a.RecOrder == 0 || a.RecOrder == null) ? count++ : a.RecOrder,
                TaxIdCode = a.TaxIdCode,
                RecordStatus = a.RecordStatus
            }).ToList();
            SaveInterCoDebitNote(lstDocumentType, lstDocumentDetailType, connectionString);
        }

        private static void SaveInterCoDebitNote(List<DocumentTypeModel> lstDocumentType, List<DocumentDetailTypeModel> lstDocumentTypeDetail, string connectionString)
        {
            SqlConnection con = null;
            SqlDataReader dr = null;
            SqlCommand cmd = null;
            string query = string.Empty;

            using (con = new SqlConnection(connectionString))
            {
                try
                {
                    DataSet ds = new DataSet();
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_Save_Interco_invoice", con);
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter master = new SqlParameter();
                    master.ParameterName = "@document";
                    master.TypeName = "dbo.DocumentType";
                    master.Value = ToDataTable(lstDocumentType);
                    cmd.Parameters.Add(master);

                    SqlParameter detail = new SqlParameter();
                    detail.ParameterName = "@documentDetail";
                    detail.TypeName = "dbo.DocumentDetailType";
                    detail.Value = ToDataTable(lstDocumentTypeDetail);
                    cmd.Parameters.Add(detail);

                    SqlDataAdapter sqlDA = new SqlDataAdapter();
                    sqlDA.SelectCommand = cmd;
                    sqlDA.Fill(ds);

                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);

            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }


        private static string DebitNoteCommonQuery(string username, long companyid, DateTime date, long? credit, long? comp, string currencyCode, Guid debitNoteId, string accountTypes)
        {

            //return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY,0 as IsInterCo FROM Bean.Currency WHERE CompanyId={companyid} AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY,0 as IsInterCo FROM Common.TermsOfPayment where CompanyId={companyid} AND (Status=1 OR Id= {credit}) AND IsCustomer=1;Select distinct COA.Id,'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.DebitNoteDetail Invd on Invd.COAId = COA.Id and Invd.DebitNoteId = '{debitNoteId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and ACT.Name Not in ('Cash and bank balances','Trade receivables','Other receivables','Trade payables','Other payables','Retained earnings','Intercompany clearing','Intercompany billing','System')  and(COA.Status = 1 or Invd.COAId = COA.Id) and isrealcoa=1 order by coa.Name;Select 'SERVICECOMPANY' as TABLENAME,comp.Id as ID,comp.Name as NAME,comp.ShortName as SHOTNAME,comp.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY,Case When comp.id=interDetail.ServiceEntityId Then 1 Else 0 End as IsInterCo from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Billing'and interDetail.Status=1 Right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId JOIN Common.CompanyUser CU on comp.ParentId=CU.CompanyId where comp.ParentId = {companyid} and (comp.Status=1 or comp.Id={comp}) and CU.Username='{username}' order by comp.ShortName;SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxId=Tax.Id Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyid} Right Join Bean.TaxCode Tax on Tax.Id=TaxMapDetail.CustTaxId where Tax.CompanyId=0  and Tax.Status<3 and EffectiveFrom<='{date}' and (EffectiveTo>='{date}' OR EffectiveTo is null)";
            return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY,0 as IsInterCo FROM Bean.Currency WHERE CompanyId={companyid} AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY,0 as IsInterCo FROM Common.TermsOfPayment where CompanyId={companyid} AND (Status=1 OR Id= {credit}) AND IsCustomer=1;Select distinct COA.Id,'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.DebitNoteDetail Invd on Invd.COAId = COA.Id and Invd.DebitNoteId = '{debitNoteId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and ACT.Name Not in ('Cash and bank balances','Trade receivables','Other receivables','Trade payables','Other payables','Retained earnings','Intercompany clearing','Intercompany billing','System')  and(COA.Status = 1 or Invd.COAId = COA.Id) and isrealcoa=1 order by coa.Name;Select 'SERVICECOMPANY' as TABLENAME,comp.Id as ID,comp.Name as NAME,comp.ShortName as SHOTNAME,comp.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY,Case When comp.id=interDetail.ServiceEntityId Then 1 Else 0 End as IsInterCo from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Billing'and interDetail.Status=1 right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId JOIN Common.CompanyUser CU on comp.ParentId=CU.CompanyId Join common.CompanyUserDetail CUD On (Comp.Id = CUD.ServiceEntityId and CU.Id = CUD.CompanyuserId) where comp.ParentId = {companyid} and (comp.Status=1 or comp.Id={comp}) and CU.Username='{username}' order by comp.ShortName;SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxCode=Tax.Code Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyid} Right Join Bean.TaxCode Tax on Tax.Code=TaxMapDetail.CustTaxCode where Tax.CompanyId={companyid}  and Tax.Status<3 and EffectiveFrom<='{date.ToString("yyyy-MM-dd")}' and (EffectiveTo>='{date.ToString("yyyy-MM-dd")}' OR EffectiveTo is null)";
        }


        private void FillViewModel(DebitNoteModel dtoModel, DebitNote debitNote,bool isCopy)
        {
            try
            {
                BeanEntity beanEntity = _beanEntityService.GetEntityById(debitNote.EntityId);
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillViewModel_FillCall_Request_Message);
                dtoModel.Id = isCopy ? Guid.NewGuid() :debitNote.Id;
                dtoModel.CompanyId = debitNote.CompanyId;
                dtoModel.EntityType = debitNote.EntityType;
                dtoModel.DocSubType = debitNote.DocSubType;
                dtoModel.DebitNoteNumber = debitNote.DebitNoteNumber;
                dtoModel.DocNo = debitNote.DocNo;
                dtoModel.DocDate = debitNote.DocDate;
                dtoModel.DueDate = debitNote.DueDate;
                dtoModel.PONo = debitNote.PONo;
                dtoModel.EntityId = debitNote.EntityId;
                dtoModel.CreditTermsId = debitNote.CreditTermsId;
                dtoModel.Nature = debitNote.Nature;
                dtoModel.DocCurrency = debitNote.DocCurrency;
                dtoModel.ServiceCompanyId = debitNote.ServiceCompanyId;
                dtoModel.EntityName = beanEntity.Name;
                dtoModel.Version = "0x" + string.Concat(Array.ConvertAll(debitNote.Version, x => x.ToString("X2")));
                //dtoModel.CustCreditlimit = _beanEntityService.GetCteditLimitsValue(debitNote.EntityId);

                //dtoModel.CustCreditlimit = beanEntity.CreditLimitValue;
                //modified code for credit Limit
                if (debitNote.DocumentState == DebitNoteState.NotPaid)
                {
                    decimal? creditLimit = beanEntity.CreditLimitValue;
                    if (creditLimit != null)
                    {
                        dtoModel.CustCreditlimit = debitNote.GrandTotal + creditLimit;
                    }
                    else
                        dtoModel.CustCreditlimit = null;
                }
                else
                    dtoModel.CustCreditlimit = null;




                dtoModel.IsGSTApplied = debitNote.IsGSTApplied;
                dtoModel.IsMultiCurrency = debitNote.IsMultiCurrency;
                dtoModel.BaseCurrency = debitNote.ExCurrency;
                dtoModel.ExchangeRate = debitNote.ExchangeRate;
                //dtoModel.ExDurationFrom = debitNote.ExDurationFrom;
                //dtoModel.ExDurationTo = debitNote.ExDurationTo;

                dtoModel.IsGstSettings = debitNote.IsGstSettings;
                dtoModel.GSTExCurrency = debitNote.GSTExCurrency;
                dtoModel.GSTExchangeRate = debitNote.GSTExchangeRate;
                //dtoModel.GSTExDurationFrom = debitNote.GSTExDurationFrom;
                //dtoModel.GSTExDurationTo = debitNote.GSTExDurationTo;
                dtoModel.Remarks = debitNote.Remarks;

                dtoModel.IsSegmentReporting = debitNote.IsSegmentReporting;
                //dtoModel.SegmentCategory1 = debitNote.SegmentCategory1;
                //dtoModel.SegmentCategory2 = debitNote.SegmentCategory2;


                dtoModel.BalanceAmount =isCopy?debitNote.GrandTotal: debitNote.BalanceAmount;
                dtoModel.GSTTotalAmount = debitNote.GSTTotalAmount;
                dtoModel.GrandTotal = debitNote.GrandTotal;

                dtoModel.IsAllowableNonAllowable = debitNote.IsAllowableNonAllowable;

                dtoModel.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
                dtoModel.NoSupportingDocument = debitNote.NoSupportingDocs;

                dtoModel.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
                dtoModel.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;

                dtoModel.Status = debitNote.Status;
                dtoModel.DocumentState =isCopy?null: debitNote.DocumentState;
                dtoModel.ModifiedDate = isCopy ? null : debitNote.ModifiedDate;
                dtoModel.ModifiedBy = isCopy ? null : debitNote.ModifiedBy;
                dtoModel.CreatedDate = isCopy ? null : debitNote.CreatedDate;
                dtoModel.UserCreated = isCopy ? null : debitNote.UserCreated;
                dtoModel.AllocatedAmount = debitNote.AllocatedAmount;
                dtoModel.IsLocked = debitNote.IsLocked;
                //dtoModel.SegmentDetailid1 = debitNote.SegmentDetailid1;
                //dtoModel.SegmentDetailid2 = debitNote.SegmentDetailid2;
                //dtoModel.SegmentMasterid1 = debitNote.SegmentMasterid1;
                //dtoModel.SegmentMasterid2 = debitNote.SegmentMasterid2;
                //if (debitNote.SegmentMasterid1 != null)
                //{
                //    var segment1 = _segmentMasterService.GetSegmentMastersById(debitNote.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    dtoModel.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
                //}
                //if (debitNote.SegmentMasterid2 != null)
                //{
                //    var segment2 = _segmentMasterService.GetSegmentMastersById(debitNote.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    dtoModel.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
                //}

                //dtoModel.DebitNoteDetails = _debitNoteDetailRepository.Query(a => a.DebitNoteId == debitNote.Id).Include(a => a.ChartOfAccount).Select().OrderBy(c => c.RecOrder).ToList();
                dtoModel.DebitNoteDetails = debitNote.DebitNoteDetails.OrderBy(c => c.RecOrder).ToList();
                //foreach (var details in dtoModel.DebitNoteDetails)
                //{
                //    if (details.TaxId != null)
                //    {
                //        ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountById(details.COAId);
                //        details.AccountName = coa.Name;
                //        TaxCode tax = _taxCodeService.GetTaxCode(details.TaxId.Value);
                //        details.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
                //        details.TaxType = tax.TaxType;
                //        details.TaxCode = tax.Code;
                //    }
                //}
                dtoModel.DebitNoteNotes =isCopy?null: _debitNoteNoteService.AllDebitNoteNote(debitNote.Id);
                #region Commented_code
                //dtoModel.DebitNoteGSTDetails = _gstDetailService.AllGstDetail(debitNote.Id);
                //var lstProvisions = _provisionService.GetProvisionByInvoiceId(debitNote.Id);
                //List<ProvisionModel> lstProvisionModel = new List<ProvisionModel>();
                //dtoModel.ProvisionCount = 0;
                //if (lstProvisions.Any())
                //{
                //	foreach (var provision in lstProvisions)
                //	{
                //		if (provision != null)
                //		{
                //			ProvisionModel provisionModel = new ProvisionModel();
                //			FillProvisionModel(provisionModel, provision);
                //			dtoModel.ProvisionCount = dtoModel.ProvisionCount + provisionModel.Provisionamount;
                //			lstProvisionModel.Add(provisionModel);
                //			//invDTO.ProvisionModel = provisionModel;
                //		}
                //	}
                //}
                //else
                //{
                //	ProvisionModel provisionModel = new ProvisionModel();
                //	provisionModel.DocumentDate = DateTime.UtcNow;
                //	provisionModel.DocNo = GetNewProvisionDocumentNumber(dtoModel.CompanyId);
                //	provisionModel.Currency = debitNote.DocCurrency;
                //	provisionModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, dtoModel.CompanyId);
                //	lstProvisionModel.Add(provisionModel);
                //}
                //dtoModel.ProvisionModels = lstProvisionModel;

                //CreditNoteModel creditNoteModel = new CreditNoteModel();
                //creditNoteModel.DocDate = DateTime.UtcNow;
                //creditNoteModel.DocCurrency = debitNote.DocCurrency;
                //creditNoteModel.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, dtoModel.CompanyId);
                //creditNoteModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, dtoModel.CompanyId);
                //creditNoteModel.IsMultiCurrency = false;
                //MultiCurrencySetting multi = _multiCurSettingService.Query(e => e.CompanyId == dtoModel.CompanyId && e.Status == RecordStatusEnum.Active).Select().FirstOrDefault();
                //creditNoteModel.IsMultiCurrency = multi != null;
                //dtoModel.CreditNoteModel = creditNoteModel;
                //    creditNoteModel.DocDate = DateTime.UtcNow;
                //    creditNoteModel.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, invDTO.CompanyId);
                //    creditNoteModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, invDTO.CompanyId);

                //    var lstInvoiceDetils =  _debitNoteDetailService.Query(x => x.DebitNoteId == dtoModel.Id).Select().ToList();
                //    List<CreditNoteDetailModel> lstCreditDetilsModel = new List<CreditNoteDetailModel>();
                //    CreditNoteDetailModel credit = new CreditNoteDetailModel();
                //    if (lstInvoiceDetils.Any())
                //    {
                //        foreach (var creditNoteDetail in lstInvoiceDetils)
                //        {
                //            CreditNoteDetailModel creditNoteDetailModel = new CreditNoteDetailModel();
                //            //FillCreditNoteDetailModel(creditNoteDetailModel, creditNoteDetail);
                //            lstCreditDetilsModel.Add(creditNoteDetailModel);
                //        }

                //        if (lstCreditDetilsModel.Any())
                //            dtoModel.CreditNoteDetailModels = lstCreditDetilsModel;
                //    }
                //    else
                //    {
                //        lstCreditDetilsModel.Add(credit);
                //        dtoModel.CreditNoteDetailModels = lstCreditDetilsModel;
                //    }
                //List<DebitNoteCreditNoteModel> lstDebitCNModel = new List<DebitNoteCreditNoteModel>();
                //var cNADetails = _creditNoteApplicationDetailService.GetById(dtoModel.Id);
                //if (cNADetails.Any())
                //{
                //    foreach (var credit in cNADetails)
                //    {
                //        DebitNoteCreditNoteModel debitCNModel = new DebitNoteCreditNoteModel();
                //        var cNA = _creditNoteApplicationService.GetCreditNoteById(credit.CreditNoteApplicationId);

                //        if (cNA != null)
                //        {
                //            var creditNote = _invoiceService.GetInvoice(cNA.InvoiceId);
                //            if (creditNote != null)
                //            {
                //                debitCNModel.CreditNoteId = creditNote.Id;
                //                debitCNModel.DocDate = creditNote.DocDate;
                //                debitCNModel.DocNo = creditNote.DocNo;
                //                debitCNModel.SystemRefNo = creditNote.InvoiceNumber;
                //                debitCNModel.Ammount = credit.CreditAmount;
                //                debitCNModel.DocType = "Credit Note";
                //                lstDebitCNModel.Add(debitCNModel);
                //            }
                //        }
                //    }
                //    dtoModel.DebitNoteCreditNoteModels = lstDebitCNModel;
                //    if (dtoModel.DebitNoteCreditNoteModels.Any())
                //    {
                //        dtoModel.CreditNoteTotalAmount = dtoModel.DebitNoteCreditNoteModels.Sum(c => c.Ammount);
                //    }
                //}

                //List<DebitNoteDoubtFulDebtModel> lstDebitDDModel = new List<DebitNoteDoubtFulDebtModel>();
                //var dDADetails = _doubtfulDebtallocationDetailService.GetDoubtfulDebtallocationdetailById(dtoModel.Id);
                //if (dDADetails.Any())
                //{
                //    foreach (var dDebit in dDADetails)
                //    {
                //        DebitNoteDoubtFulDebtModel debitDDModel = new DebitNoteDoubtFulDebtModel();
                //        var dDA = _doubtfulDebtAllocationService.GetDoubtfulDebtAllocation(dDebit.DoubtfulDebtAllocationId);
                //        if (dDA != null)
                //        {
                //            var creditNote = _invoiceService.GetInvoice(dDA.InvoiceId);
                //            if (creditNote != null)
                //            {
                //                debitDDModel.DoubtFulDebtId = creditNote.Id;
                //                debitDDModel.DocDate = creditNote.DocDate;
                //                debitDDModel.DocNo = creditNote.DocNo;
                //                debitDDModel.DocSubType = "Debt Provision";
                //                debitDDModel.SystemRefNo = creditNote.InvoiceNumber;
                //                debitDDModel.Ammount = dDebit.AllocateAmount;
                //                lstDebitDDModel.Add(debitDDModel);
                //            }
                //        }
                //    }
                //    dtoModel.DebitNoteDoubtFulDebtModels = lstDebitDDModel;
                //    if (dtoModel.DebitNoteDoubtFulDebtModels.Any())
                //    {
                //        dtoModel.DoubtfulDebitTotalAmount = dtoModel.DebitNoteDoubtFulDebtModels.Sum(c => c.Ammount);
                //    }
                //}
                //dtoModel.IsDoubtfulDebtShow = (debitNote.GrandTotal - debitNote.BalanceAmount) + dtoModel.DoubtfulDebitTotalAmount == debitNote.GrandTotal ? false : true;
                //var lstdetails = _receiptDetailService.lstDetails(debitNote.Id);
                //if (lstdetails.Any())
                //{
                //    foreach (var detail in lstdetails)
                //    {
                //        var receipt = _receiptService.GetReceipt(detail.ReceiptId, debitNote.CompanyId);
                //        if (receipt != null)
                //        {
                //            DebitNoteCreditNoteModel model = new DebitNoteCreditNoteModel();
                //            model.CreditNoteId = receipt.Id;
                //            model.DocDate = receipt.DocDate;
                //            model.DocNo = receipt.DocNo;
                //            model.SystemRefNo = receipt.SystemRefNo;
                //            if (receipt.DocumentState != ReceiptState.Void)
                //                model.Ammount = detail.ReceiptAmount;
                //            else
                //                model.Ammount = null;
                //            model.DocType = "Receipt";
                //            lstDebitCNModel.Add(model);
                //        }
                //    }
                //}
                //dtoModel.DebitNoteCreditNoteModels = lstDebitCNModel;
                //if (dtoModel.DebitNoteCreditNoteModels.Any())
                //    dtoModel.ReceiptTotalAmount = dtoModel.DebitNoteCreditNoteModels.Sum(c => c.Ammount);
                #endregion


                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillViewModel_FillCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillViewModel_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        private void FillNewDebitNoteModel(DebitNoteModel dtoModel, FinancialSetting financSettings, long companyId/*, AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo*/)
        {
            DebitNote lastInvoice = _debitNoteService.CreateDebitNote(companyId);

            dtoModel.Id = Guid.NewGuid();
            dtoModel.CompanyId = companyId;
            dtoModel.DocDate = lastInvoice == null ? DateTime.Now : lastInvoice.DocDate;
            //dtoModel.DocNo = GetNewDebitNoteDocumentNumber(companyId);
            dtoModel.DocumentState = "Not Paid";
            dtoModel.DueDate = DateTime.UtcNow;


            //bool? isEdit = false;
            //dtoModel.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice, DocTypeConstants.DebitNote, _autoNo, null, ref isEdit);
            //dtoModel.IsDocNoEditable = isEdit;


            //dtoModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
            //dtoModel.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
            //dtoModel.IsMultiCurrency = false;
            dtoModel.CreatedDate = DateTime.UtcNow;
            //dtoModel.IsGSTDeregistered = _gstSettingService.IsGSTAllowed(companyId, dtoModel.DocDate);
            //dtoModel.IsGstSettings = _gstSettingService.IsGSTSettingActivated(companyId);
            //dtoModel.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, companyId);
            dtoModel.BaseCurrency = financSettings.BaseCurrency;
            dtoModel.DocCurrency = dtoModel.BaseCurrency;
            //MultiCurrencySetting multi = _multiCurSettingService.GetMultiCurrency(companyId);
            //dtoModel.IsMultiCurrency = multi != null;
            //Forex forexBean = _forexService.GetMultiCurrencyInformation(dtoModel.BaseCurrency, dtoModel.DocDate, true, dtoModel.CompanyId);
            //if (forexBean != null)
            //{
            //    dtoModel.ExchangeRate = forexBean.UnitPerUSD;
            //    dtoModel.ExDurationFrom = forexBean.DateFrom;
            //    dtoModel.ExDurationTo = forexBean.Dateto;
            //}
            //GSTSetting GSTSetting = _gstSettingService.GetGSTSettings(companyId);
            //if (GSTSetting != null)
            //{
            //    dtoModel.GSTExCurrency = GSTSetting.GSTRepoCurrency;
            //    forexBean = _forexService.GetMultiCurrencyInformation(dtoModel.BaseCurrency, dtoModel.DocDate, false, dtoModel.CompanyId);
            //    if (forexBean != null)
            //    {
            //        dtoModel.GSTExchangeRate = forexBean.UnitPerUSD;
            //        dtoModel.GSTExDurationFrom = forexBean.DateFrom;
            //        dtoModel.GSTExDurationTo = forexBean.Dateto;
            //    }
            //}
            //else
            //dtoModel.IsGstSettings = false;
            //dtoModel.IsBaseCurrencyRateChanged = false;
            //dtoModel.IsGSTCurrencyRateChanged = false;
        }
        private string GetAutoNumberByEntityType(long companyId, DebitNote lastInvoice, string entityType, AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo, Invoice oldInvoice, ref bool? isEdit)
        {
            string outPutNumber = null;
            string output = null;
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    if (entityType != DocTypeConstants.DebitNote)
                        outPutNumber = GetNewInvoiceDocumentNumber(entityType, companyId);
                    else
                        outPutNumber = GetNewDebitNoteDocumentNumber(companyId);
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
                    if (entityType != DocTypeConstants.DebitNote)
                    {
                        if (oldInvoice != null)
                        {
                            if (_autoNo.Format.Contains("{MM/YYYY}"))
                            {
                                //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                                if (oldInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
                                {
                                    //number = "1";
                                    outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');
                                }
                                else
                                {
                                    output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                    outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                                }
                            }
                            else
                            {
                                output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                        }
                        else
                        {
                            output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
                            outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            //counter = Convert.ToInt32(value);
                        }
                    }

                    else
                    {
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
                                    output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                    outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                                }
                            }
                            else
                            {
                                output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                        }
                        else
                        {
                            output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
                            outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            //counter = Convert.ToInt32(value);
                        }
                    }
                }
            }
            return outPutNumber;
        }

        private string GetAutoNumberForReceipt(long companyId, Receipt lastInvoice, string entityType, AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.ReceiptModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewReceiptDocNo(DocTypeConstants.ReceiptDoc, companyId);
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

        private string GetNewDebitNoteDocumentNumber(long CompanyId)
        {
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewDebitNoteDocumentNumber_GetCall_Request_Message);
                DebitNote debitnote = _debitNoteService.CreateDebitNoteForDocNo(CompanyId);
                if (debitnote != null)
                {
                    string strOldNo = String.Empty;
                    DebitNote duplicatDebiteNote;
                    int index;
                    strOldDocNo = debitnote.DocNo;

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

                        duplicatDebiteNote = _debitNoteService.GetDocNo(strNewDocNo, CompanyId);
                    } while (duplicatDebiteNote != null);
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewDebitNoteDocumentNumber_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return strNewDocNo;
        }
        private void InsertDebitNote(DebitNoteModel TObject, DebitNote debitnoteNew)
        {
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_InsertDebitNote_FillCall_Request_Message);
                debitnoteNew.CompanyId = TObject.CompanyId;
                debitnoteNew.DocSubType = DocTypeConstants.DebitNote;
                debitnoteNew.DocDate = TObject.DocDate.Date;
                debitnoteNew.DueDate = TObject.DueDate.Date;
                debitnoteNew.PONo = TObject.PONo;
                debitnoteNew.EntityType = "Customer";
                debitnoteNew.EntityId = TObject.EntityId;
                debitnoteNew.CreditTermsId = TObject.CreditTermsId;
                debitnoteNew.Nature = TObject.Nature;
                debitnoteNew.ServiceCompanyId = TObject.ServiceCompanyId;
                debitnoteNew.DocCurrency = TObject.DocCurrency;
                debitnoteNew.IsMultiCurrency = TObject.IsMultiCurrency;
                debitnoteNew.ExCurrency = TObject.BaseCurrency;
                debitnoteNew.ExchangeRate = TObject.ExchangeRate;
                debitnoteNew.ExDurationFrom = TObject.ExDurationFrom;
                debitnoteNew.ExDurationTo = TObject.ExDurationTo;
                debitnoteNew.Remarks = TObject.Remarks;
                debitnoteNew.IsGSTApplied = TObject.IsGSTApplied;
                debitnoteNew.IsGstSettings = TObject.IsGstSettings;
                debitnoteNew.GSTExCurrency = TObject.GSTExCurrency;
                if (TObject.IsGstSettings)
                {

                    debitnoteNew.GSTExchangeRate = TObject.GSTExchangeRate;
                    debitnoteNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    debitnoteNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                debitnoteNew.GrandTotal = TObject.GrandTotal;
                debitnoteNew.GSTTotalAmount = TObject.GSTTotalAmount;
                debitnoteNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;
                debitnoteNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                debitnoteNew.NoSupportingDocs = debitnoteNew.IsNoSupportingDocument ? TObject.NoSupportingDocument : null;
                debitnoteNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                debitnoteNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                debitnoteNew.Status = TObject.Status;
                debitnoteNew.DebitNoteNumber = TObject.DebitNoteNumber;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_InsertDebitNote_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        public void UpdateDebitNoteDetails(DebitNoteModel TObject, DebitNote _debitnoteNew)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteDetails_UpdateCall_Request_Message);
                foreach (DebitNoteDetail detail in TObject.DebitNoteDetails)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        detail.RecOrder = recorder + 1;
                        recorder = detail.RecOrder;
                        detail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
                        detail.BaseTaxAmount = TObject.ExchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0 : detail.DocTaxAmount;
                        detail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                        detail.ObjectState = ObjectState.Added;
                        _debitnoteNew.DebitNoteDetails.Add(detail);
                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        DebitNoteDetail debitnoteDetail = _debitnoteNew.DebitNoteDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (debitnoteDetail != null)
                        {
                            debitnoteDetail.DebitNoteId = TObject.Id;
                            debitnoteDetail.COAId = detail.COAId;
                            debitnoteDetail.AllowDisAllow = detail.AllowDisAllow;
                            debitnoteDetail.TaxId = detail.TaxId;
                            debitnoteDetail.TaxRate = detail.TaxRate;
                            debitnoteDetail.TaxType = detail.TaxType;
                            debitnoteDetail.DocAmount = detail.DocAmount;
                            debitnoteDetail.DocTaxAmount = detail.DocTaxAmount;
                            debitnoteDetail.DocTotalAmount = detail.DocTotalAmount;
                            debitnoteDetail.TaxIdCode = detail.TaxIdCode;

                            debitnoteDetail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(debitnoteDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : debitnoteDetail.DocAmount;
                            debitnoteDetail.BaseTaxAmount = TObject.ExchangeRate != null ? debitnoteDetail.DocTaxAmount != null ? Math.Round((decimal)debitnoteDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0 : debitnoteDetail.DocTaxAmount;
                            debitnoteDetail.BaseTotalAmount = Math.Round((decimal)debitnoteDetail.BaseAmount + (debitnoteDetail.BaseTaxAmount != null ? (decimal)debitnoteDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);

                            debitnoteDetail.AccountDescription = detail.AccountDescription;
                            debitnoteDetail.IsPLAccount = detail.IsPLAccount;
                            debitnoteDetail.RecOrder = recorder + 1;
                            recorder = debitnoteDetail.RecOrder;
                            debitnoteDetail.ObjectState = ObjectState.Modified;
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        DebitNoteDetail debitnoteDetail = _debitnoteNew.DebitNoteDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (debitnoteDetail != null)
                        {
                            debitnoteDetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                    LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteDetails_UpdateCall_SuccessFully_Message);
                }
            }

            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteDetails_UpdateCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        public void UpdateDebitNoteNotes(DebitNoteModel TObject, DebitNote _debitnoteNew)
        {

            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteNotes_UpdateCall_Request_Message);
                if (TObject.DebitNoteNotes != null)
                {
                    foreach (DebitNoteNote note in TObject.DebitNoteNotes)
                    {
                        if (note.RecordStatus == "Added")
                        {
                            note.Id = Guid.NewGuid();
                            note.CreatedDate = DateTime.UtcNow;
                            note.UserCreated = note.UserCreated;
                            note.ObjectState = ObjectState.Added;
                            _debitnoteNew.DebitNoteNotes.Add(note);
                        }
                        else if (note.RecordStatus != "Added" && note.RecordStatus != "Deleted")
                        {
                            DebitNoteNote debitnoteNote =
                                _debitnoteNew.DebitNoteNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                            if (debitnoteNote != null)
                            {
                                debitnoteNote.DebitNoteId = TObject.Id;
                                debitnoteNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
                                debitnoteNote.Notes = note.Notes;
                                debitnoteNote.ModifiedDate = DateTime.UtcNow;
                                debitnoteNote.ModifiedBy = note.ModifiedBy;
                                debitnoteNote.ObjectState = ObjectState.Modified;
                            }
                        }
                        else if (note.RecordStatus == "Deleted")
                        {
                            DebitNoteNote debitnoteNote =
                                _debitnoteNew.DebitNoteNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                            if (debitnoteNote != null)
                            {
                                debitnoteNote.ObjectState = ObjectState.Deleted;
                            }
                        }
                        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteNotes_UpdateCall_SuccessFully_Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteNotes_UpdateCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        //public void UpdateDebitNoteGSTDetails(DebitNoteModel TObject, DebitNote _debitnoteNew)
        //{

        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteGSTDetails_UpdateCall_Request_Message);
        //        foreach (DebitNoteGSTDetail detail in TObject.DebitNoteGSTDetails)
        //        {
        //            if (detail.RecordStatus == "Added")
        //            {
        //                detail.ObjectState = ObjectState.Added;
        //                _debitnoteNew.DebitNoteGSTDetails.Add(detail);
        //            }
        //            else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
        //            {
        //                DebitNoteGSTDetail debitnoteGSTDetail = _debitnoteNew.DebitNoteGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                if (debitnoteGSTDetail != null)
        //                {
        //                    debitnoteGSTDetail.DebitNoteId = TObject.Id;
        //                    debitnoteGSTDetail.TaxId = detail.TaxId;
        //                    debitnoteGSTDetail.Amount = detail.Amount;
        //                    debitnoteGSTDetail.TaxAmount = detail.TaxAmount;
        //                    debitnoteGSTDetail.TotalAmount = detail.TotalAmount;

        //                    debitnoteGSTDetail.ObjectState = ObjectState.Modified;
        //                }
        //            }
        //            else if (detail.RecordStatus == "Deleted")
        //            {
        //                DebitNoteGSTDetail debitnoteGSTDetail = _debitnoteNew.DebitNoteGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                if (debitnoteGSTDetail != null)
        //                {
        //                    debitnoteGSTDetail.ObjectState = ObjectState.Deleted;
        //                }
        //            }
        //            LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteGSTDetails_UpdateCall_SuccessFully_Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteGSTDetails_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}
        //private void UpdateProvision(DebitNoteModel TObject, DebitNote _debitnoteNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateProvision_UpdateCall_Request_Message);
        //        foreach (var provisionModel in TObject.ProvisionModels)
        //        {
        //            Provision provision = new Provision();
        //            if (provisionModel.RecordStatus == "Added")
        //            {
        //                if (provisionModel.Id == new Guid("00000000-0000-0000-0000-000000000000"))
        //                {

        //                }
        //                else
        //                {
        //                    FillProvision(provision, provisionModel);
        //                    provision.Id = provisionModel.Id;

        //                    Company company = _companyService.GetById(TObject.CompanyId);
        //                    provision.SystemRefNo = GenerateAutoNumberForType(company.Id, DocTypeConstants.Provision, company.ShortName);

        //                    provision.ObjectState = ObjectState.Added;
        //                    _provisionService.Insert(provision);

        //                }
        //            }
        //            else if (provisionModel.RecordStatus != "Added" && provisionModel.RecordStatus != "Deleted")
        //            {
        //                Provision provisioneDetail = _provisionService.GetProvisionById(provisionModel.Id);
        //                if (provisioneDetail != null)
        //                {
        //                    FillProvision(provisioneDetail, provisionModel);

        //                    provisioneDetail.ObjectState = ObjectState.Modified;
        //                    _provisionService.Update(provisioneDetail);
        //                }
        //            }
        //            else if (provisionModel.RecordStatus == "Deleted")
        //            {
        //                Provision provisioneDetail = _provisionService.GetProvisionById(provisionModel.Id);
        //                if (provisioneDetail != null)
        //                {
        //                    provisioneDetail.ObjectState = ObjectState.Deleted;
        //                }
        //            }
        //            LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateProvision_UpdateCall_SuccessFully_Message);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateProvision_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}
        //private void FillProvision(Provision provision, ProvisionModel provisionModel)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillProvision_FillCall_Request_Message);
        //        provision.RefDocumentId = provisionModel.InvoiceId;
        //        provision.CompanyId = provisionModel.CompanyId;
        //        provision.IsAllowableDisallowable = provisionModel.IsAllowableDisallowable;
        //        provision.IsDisAllow = provisionModel.IsDisAllow;
        //        provision.IsNoSupportingDocument = provisionModel.IsNoSupportingDocument;
        //        provision.ModifiedBy = provisionModel.ModifiedBy;
        //        provision.ModifiedDate = provisionModel.ModifiedDate;
        //        provision.NoSupportingDocument = provisionModel.NoSupportingDocument;
        //        provision.Provisionamount = provisionModel.Provisionamount;
        //        provision.Remarks = provisionModel.Remarks;

        //        provision.Status = provisionModel.Status;
        //        provision.SystemRefNo = provisionModel.SystemRefNo;
        //        provision.UserCreated = provisionModel.UserCreated;
        //        provision.CreatedDate = provisionModel.CreatedDate;
        //        provision.Currency = provisionModel.Currency;

        //        provision.DocNo = provisionModel.DocNo;
        //        provision.DocumentDate = provisionModel.DocumentDate;
        //        provision.DocumentType = "Provision";
        //        provision.RefDocType = DocTypeConstants.DebitNote;
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillProvision_FillCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillProvision_FillCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}
        //private void UpdateCreditNote(DebitNoteModel TObject, DebitNote _invoiceNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateCreditNote_UpdateCall_Request_Message);
        //        Invoice invoice = new Invoice();
        //        if (TObject.CreditNoteModel.RecordStatus == "Added")
        //        {
        //            if (TObject.CreditNoteModel.Id == new Guid("00000000-0000-0000-0000-000000000000"))
        //            {

        //            }
        //            else
        //            {
        //                //FillCreditNote(TObject.CreditNoteModel, invoice);

        //                invoice.Id = TObject.CreditNoteModel.Id;
        //                invoice.CompanyId = TObject.CompanyId;
        //                invoice.DocSubType = DocTypeConstants.CreditNote;
        //                invoice.DocType = DocTypeConstants.CreditNote;
        //                invoice.DocDate = TObject.CreditNoteModel.DocDate;
        //                invoice.DueDate = TObject.DueDate;
        //                invoice.DocNo = TObject.CreditNoteModel.DocNo;
        //                invoice.IsNoSupportingDocument = TObject.CreditNoteModel.IsNoSupportingDocument;
        //                invoice.NoSupportingDocs = TObject.CreditNoteModel.NoSupportingDocument;
        //                invoice.Remarks = TObject.CreditNoteModel.Remarks;
        //                invoice.DocCurrency = TObject.DocCurrency;
        //                invoice.EntityType = "Customer";

        //                invoice.EntityId = TObject.EntityId;
        //                invoice.CreditTermsId = TObject.CreditTermsId;
        //                invoice.Nature = TObject.Nature;
        //                invoice.ServiceCompanyId = TObject.ServiceCompanyId;

        //                invoice.IsMultiCurrency = TObject.IsMultiCurrency;
        //                invoice.ExCurrency = TObject.BaseCurrency;
        //                invoice.ExchangeRate = TObject.ExchangeRate;
        //                invoice.ExDurationFrom = TObject.ExDurationFrom;
        //                invoice.ExDurationTo = TObject.ExDurationTo;
        //                invoice.IsGstSettings = _gstSettingService.IsGSTSettingActivated(TObject.CompanyId) && !_gstSettingService.IsGSTDeregistered(TObject.CompanyId);
        //                if (TObject.IsGstSettings)
        //                {
        //                    invoice.GSTExCurrency = TObject.GSTExCurrency;
        //                    invoice.GSTExchangeRate = TObject.GSTExchangeRate;
        //                    invoice.GSTExDurationFrom = TObject.GSTExDurationFrom;
        //                    invoice.GSTExDurationTo = TObject.GSTExDurationTo;
        //                }
        //                invoice.BalanceAmount = TObject.GrandTotal;
        //                invoice.GrandTotal = TObject.GrandTotal;
        //                invoice.GSTTotalAmount = TObject.GSTTotalAmount;

        //                invoice.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;



        //                invoice.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, TObject.CompanyId);
        //                if (invoice.IsSegmentReporting)
        //                {
        //                    if (TObject.IsSegmentActive1.Value)
        //                    {
        //                        invoice.SegmentMasterid1 = TObject.SegmentMasterid1;
        //                        invoice.SegmentDetailid1 = TObject.SegmentDetailid1;
        //                        invoice.SegmentCategory1 = TObject.SegmentCategory1;
        //                    }
        //                    if (TObject.IsSegmentActive2.Value)
        //                    {
        //                        invoice.SegmentMasterid2 = TObject.SegmentMasterid2;
        //                        invoice.SegmentDetailid2 = TObject.SegmentDetailid2;
        //                        invoice.SegmentCategory2 = TObject.SegmentCategory2;
        //                    }
        //                }
        //                else
        //                {
        //                    invoice.SegmentCategory1 = null;
        //                    invoice.SegmentCategory2 = null;
        //                    invoice.SegmentMasterid1 = null;
        //                    invoice.SegmentMasterid2 = null;
        //                    invoice.SegmentDetailid1 = null;
        //                    invoice.SegmentDetailid2 = null;
        //                }
        //                invoice.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
        //                invoice.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;

        //                invoice.IsGstSettings = TObject.IsGstSettings;
        //                invoice.IsSegmentReporting = TObject.IsSegmentReporting;
        //                invoice.Status = TObject.Status;
        //                invoice.DocumentState = CreditNoteState.NotApplied;
        //                invoice.UserCreated = TObject.UserCreated;
        //                invoice.CreatedDate = DateTime.UtcNow;
        //                Company company = _companyService.GetById(TObject.CompanyId);
        //                invoice.InvoiceNumber = GenerateAutoNumberForType(company.Id, DocTypeConstants.CreditNote, company.ShortName);

        //                invoice.ObjectState = ObjectState.Added;
        //                _invoiceService.Insert(invoice);

        //            }
        //        }
        //        else if (TObject.CreditNoteModel.RecordStatus != "Added" && TObject.CreditNoteModel.RecordStatus != "Deleted")
        //        {
        //            Invoice invoiceDetail = _invoiceService.GetInvoice(TObject.CreditNoteModel.Id);
        //            if (invoiceDetail != null)
        //            {

        //                invoiceDetail.ObjectState = ObjectState.Modified;
        //                _invoiceService.Update(invoiceDetail);
        //            }
        //        }
        //        else if (TObject.CreditNoteModel.RecordStatus == "Deleted")
        //        {
        //            Invoice invoiceDetail = _invoiceService.GetInvoice(TObject.CreditNoteModel.Id);
        //            if (invoiceDetail != null)
        //            {
        //                invoiceDetail.ObjectState = ObjectState.Deleted;
        //            }
        //        }
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateCreditNote_UpdateCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateCreditNote_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }

        //}
        //private void UpdateCreditNoteDetail(DebitNoteModel TObject, DebitNote _invoiceNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateCreditNoteDetail_UpdateCall_Request_Message);
        //        foreach (CreditNoteDetailModel creditNoteDetailModel in TObject.CreditNoteDetailModels)
        //        {
        //            InvoiceDetail invoiceDetail = new InvoiceDetail();
        //            if (creditNoteDetailModel.RecordStatus == "Added")
        //            {
        //                if (creditNoteDetailModel.Id == new Guid("00000000-0000-0000-0000-000000000000"))
        //                {

        //                }
        //                else
        //                {
        //                    FillCreditNoteDetail(invoiceDetail, creditNoteDetailModel);
        //                    invoiceDetail.Id = creditNoteDetailModel.Id;
        //                    invoiceDetail.InvoiceId = TObject.CreditNoteModel.Id;
        //                    invoiceDetail.ObjectState = ObjectState.Added;
        //                    _invoiceDetailService.Insert(invoiceDetail);
        //                }
        //            }
        //            else if (creditNoteDetailModel.RecordStatus != "Added" && creditNoteDetailModel.RecordStatus != "Deleted")
        //            {
        //                InvoiceDetail invoiceDetails = _invoiceDetailService.GetInvoiceDetail(creditNoteDetailModel.Id);
        //                if (invoiceDetail != null)
        //                {
        //                    FillCreditNoteDetail(invoiceDetails, creditNoteDetailModel);

        //                    invoiceDetail.ObjectState = ObjectState.Modified;
        //                    _invoiceDetailService.Update(invoiceDetails);
        //                }
        //            }
        //            else if (creditNoteDetailModel.RecordStatus == "Deleted")
        //            {
        //                InvoiceDetail invoiceDetails = _invoiceDetailService.GetInvoiceDetail(creditNoteDetailModel.Id);
        //                if (invoiceDetail != null)
        //                {
        //                    invoiceDetail.ObjectState = ObjectState.Deleted;
        //                }
        //            }
        //            LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateCreditNoteDetail_UpdateCall_SuccessFully_Message);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateCreditNoteDetail_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}
        private void FillCreditNoteDetail(InvoiceDetail invoiceDetail, CreditNoteDetailModel creditNoteDetailModel)
        {
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillCreditNoteDetail_FillCall_Request_Message);
                invoiceDetail.AccountName = creditNoteDetailModel.AccountName;
                invoiceDetail.AmtCurrency = creditNoteDetailModel.AmtCurrency;
                invoiceDetail.BaseAmount = creditNoteDetailModel.BaseAmount;
                invoiceDetail.BaseTaxAmount = creditNoteDetailModel.BaseTaxAmount;
                invoiceDetail.BaseTotalAmount = creditNoteDetailModel.BaseTotalAmount;

                invoiceDetail.COAId = creditNoteDetailModel.COAId;
                invoiceDetail.Discount = creditNoteDetailModel.Discount;
                invoiceDetail.DiscountType = creditNoteDetailModel.DiscountType;
                invoiceDetail.DocAmount = creditNoteDetailModel.DocAmount;
                invoiceDetail.DocTaxAmount = creditNoteDetailModel.DocTaxAmount;
                invoiceDetail.DocTotalAmount = creditNoteDetailModel.DocTotalAmount;


                invoiceDetail.ItemCode = creditNoteDetailModel.ItemCode;
                invoiceDetail.ItemDescription = creditNoteDetailModel.ItemDescription;
                invoiceDetail.ItemId = creditNoteDetailModel.ItemId;
                invoiceDetail.Qty = creditNoteDetailModel.Qty;
                invoiceDetail.Remarks = creditNoteDetailModel.Remarks;

                invoiceDetail.TaxCurrency = creditNoteDetailModel.TaxCurrency;
                invoiceDetail.TaxId = creditNoteDetailModel.TaxId;
                invoiceDetail.TaxRate = creditNoteDetailModel.TaxRate;

                invoiceDetail.Unit = creditNoteDetailModel.Unit;
                invoiceDetail.UnitPrice = creditNoteDetailModel.UnitPrice;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillCreditNoteDetail_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillCreditNoteDetail_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        private void FillCreditNoteByDebitNote(CreditNoteModel invDTO, DebitNote debitNote)
        {
            invDTO.Id = Guid.NewGuid();

            invDTO.CompanyId = debitNote.CompanyId;
            invDTO.EntityType = debitNote.EntityType;
            invDTO.DocSubType = DocTypeConstants.CreditNote;

            invDTO.CreditTermsId = debitNote.CreditTermsId;
            invDTO.DocDate = debitNote.DocDate;
            var top = _termsOfPaymentService.GetTermsOfPaymentById(invDTO.CreditTermsId);
            if (top != null)
            {
                invDTO.DueDate = invDTO.DocDate.AddDays(top.TOPValue.Value);
                invDTO.CreditTermsName = top.Name;

            }
            invDTO.EntityId = debitNote.EntityId;
            BeanEntity beanEntity = _beanEntityService.Query(a => a.Id == debitNote.EntityId).Select().FirstOrDefault();
            invDTO.EntityName = beanEntity.Name;

            invDTO.Nature = debitNote.Nature;
            invDTO.DocCurrency = debitNote.DocCurrency;
            invDTO.ServiceCompanyId = debitNote.ServiceCompanyId;

            invDTO.IsMultiCurrency = debitNote.IsMultiCurrency;
            invDTO.BaseCurrency = debitNote.ExCurrency;
            invDTO.ExchangeRate = debitNote.ExchangeRate;
            invDTO.ExDurationFrom = debitNote.ExDurationFrom;
            invDTO.ExDurationTo = debitNote.ExDurationTo;
            //invDTO.IsGSTApplied = debitNote.IsGSTApplied;

            invDTO.IsGstSettings = debitNote.IsGstSettings;
            invDTO.GSTExCurrency = debitNote.GSTExCurrency;
            invDTO.GSTExchangeRate = debitNote.GSTExchangeRate;
            invDTO.GSTExDurationFrom = debitNote.GSTExDurationFrom;
            invDTO.GSTExDurationTo = debitNote.GSTExDurationTo;
            invDTO.ExtensionType = ExtensionType.DebitNote;

            //invDTO.IsSegmentReporting = debitNote.IsSegmentReporting;
            invDTO.SegmentCategory1 = debitNote.SegmentCategory1;
            invDTO.SegmentCategory2 = debitNote.SegmentCategory2;

            invDTO.GSTTotalAmount = debitNote.GSTTotalAmount;
            invDTO.GrandTotal = debitNote.BalanceAmount;
            invDTO.BalanceAmount = debitNote.BalanceAmount;

            invDTO.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, debitNote.CompanyId);

            invDTO.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
            invDTO.NoSupportingDocument = debitNote.NoSupportingDocs;

            //invDTO.SegmentMasterid1 = debitNote.SegmentMasterid1;

            //if (debitNote.SegmentMasterid1 != null)
            //{
            //    var segment1 = _segmentMasterService.GetSegmentMastersById(debitNote.SegmentMasterid1.Value).FirstOrDefault(); ;
            //    invDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
            //}
            //if (debitNote.SegmentMasterid2 != null)
            //{
            //    var segment2 = _segmentMasterService.GetSegmentMastersById(debitNote.SegmentMasterid2.Value).FirstOrDefault(); ;
            //    invDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
            //}

            //invDTO.SegmentMasterid2 = debitNote.SegmentMasterid2;
            //invDTO.SegmentDetailid1 = debitNote.SegmentDetailid1;
            //invDTO.SegmentDetailid2 = debitNote.SegmentDetailid2;



            invDTO.Remarks = debitNote.Remarks;
            invDTO.Status = debitNote.Status;
            invDTO.DocumentState = debitNote.DocumentState;
            invDTO.CreatedDate = debitNote.CreatedDate;
            invDTO.UserCreated = debitNote.UserCreated;

            invDTO.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
            invDTO.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
            //invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
            //invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
            //invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
            //invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
        }

        private string GetNewInvoiceDocumentNumber(string docType, long CompanyId)
        {
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_Request_Message);
                Invoice invoice = _invoiceService.GetInvoiceByCompany(CompanyId, docType);

                if (invoice != null)
                {
                    string strOldNo = String.Empty;
                    Invoice duplicatInvoice;
                    int index;
                    strOldDocNo = invoice.DocNo;

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

                        duplicatInvoice = _invoiceService.GetDuplicateInvoice(CompanyId, docType, strNewDocNo);
                    } while (duplicatInvoice != null);
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return strNewDocNo;
        }
        private void FillInvoiceDetailV(InvoiceDetail cnDetail, DebitNoteDetail detail, CreditNoteModel invDTO)
        {
            cnDetail.Id = Guid.NewGuid();
            cnDetail.InvoiceId = invDTO.Id;
            //cnDetail.AccountName = detail.AccountName;
            cnDetail.BaseAmount = detail.BaseAmount;
            cnDetail.BaseTaxAmount = detail.BaseTaxAmount;
            cnDetail.BaseTotalAmount = detail.BaseTotalAmount;
            cnDetail.COAId = detail.COAId;
            cnDetail.DocAmount = detail.DocAmount;
            cnDetail.DocTaxAmount = detail.DocTaxAmount;
            cnDetail.DocTotalAmount = detail.DocTotalAmount;
            cnDetail.TaxId = detail.TaxId;
            cnDetail.TaxRate = detail.TaxRate;
            if (cnDetail.TaxId != null)
            {
                TaxCode tax = _taxCodeService.Query(c => c.Id == cnDetail.TaxId).Select().FirstOrDefault();
                cnDetail.TaxIdCode = tax.Code;
                cnDetail.TaxType = tax.TaxType;
            }
        }
        private void FillCreditNoteApplication(CreditNoteApplicationModel CNAModel, DebitNote debitNote, CreditNoteModel invDTO)
        {
            CNAModel.Id = Guid.NewGuid();
            CNAModel.InvoiceId = invDTO.Id;
            CNAModel.CompanyId = debitNote.CompanyId;
            CNAModel.DocNo = debitNote.DocNo;
            CNAModel.DocCurrency = debitNote.DocCurrency;
            CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
            CNAModel.DocDate = invDTO.DocDate;
            decimal? sumLineTotal = 0;
            if (invDTO.InvoiceDetails.Any())
            {
                sumLineTotal = invDTO.InvoiceDetails.Sum(od => od.DocTotalAmount);
            }
            CNAModel.CreditAmount = debitNote.BalanceAmount;
            CNAModel.CreditNoteAmount = debitNote.BalanceAmount;
            CNAModel.CreditNoteBalanceAmount = debitNote.BalanceAmount;
            CNAModel.CreditNoteApplicationDate = debitNote.DocDate;
            CNAModel.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
            CNAModel.NoSupportingDocument = debitNote.NoSupportingDocs;
            CNAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
            CNAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
            CNAModel.CreatedDate = DateTime.UtcNow;
            CNAModel.UserCreated = debitNote.UserCreated;
            CNAModel.Status = CreditNoteApplicationStatus.Posted;
        }
        private void FillCreditNoteApplicationDetail(CreditNoteApplicationDetailModel detailModel, DebitNote debitNote, CreditNoteApplicationModel CNAModel, CreditNoteModel invDTO)
        {
            detailModel.Id = Guid.NewGuid();
            detailModel.CreditNoteApplicationId = CNAModel.Id;
            detailModel.BalanceAmount = debitNote.BalanceAmount;
            detailModel.DocCurrency = CNAModel.DocCurrency;
            detailModel.DocType = DocTypeConstants.DebitNote;
            detailModel.Nature = debitNote.Nature;
            detailModel.DocAmount = debitNote.GrandTotal;
            detailModel.DocDate = debitNote.DocDate;
            detailModel.DocumentId = debitNote.Id;
            detailModel.DocNo = debitNote.DocNo;
            detailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
            detailModel.SegmentCategory1 = debitNote.SegmentCategory1;
            detailModel.SegmentCategory2 = debitNote.SegmentCategory2;
            detailModel.BaseCurrencyExchangeRate = debitNote.ExchangeRate.Value;
            decimal sumLineTotal1 = 0;
            decimal diff = 0;
            if (invDTO.InvoiceDetails.Any())
            {
                detailModel.CreditAmount = debitNote.BalanceAmount;
            }
        }
        private void FillDoubtfulDebt(DoubtfulDebtModel invDTO, DebitNote debitNote)
        {
            invDTO.Id = Guid.NewGuid();

            invDTO.CompanyId = debitNote.CompanyId;
            invDTO.EntityType = debitNote.EntityType;
            invDTO.DocSubType = DocTypeConstants.DoubtFulDebitNote;

            invDTO.DocDate = debitNote.DocDate;

            invDTO.EntityId = debitNote.EntityId;
            BeanEntity beanEntity = _beanEntityService.GetEntityById(debitNote.EntityId);
            invDTO.EntityName = beanEntity.Name;

            invDTO.Nature = debitNote.Nature;
            invDTO.DocCurrency = debitNote.DocCurrency;
            invDTO.ServiceCompanyId = debitNote.ServiceCompanyId;

            invDTO.IsMultiCurrency = debitNote.IsMultiCurrency;
            invDTO.BaseCurrency = debitNote.ExCurrency;
            invDTO.ExchangeRate = debitNote.ExchangeRate;
            invDTO.ExDurationFrom = debitNote.ExDurationFrom;
            invDTO.ExDurationTo = debitNote.ExDurationTo;
            invDTO.IsGSTApplied = debitNote.IsGSTApplied;
            invDTO.ExtensionType = ExtensionType.DebitNote;
            invDTO.IsSegmentReporting = debitNote.IsSegmentReporting;
            invDTO.SegmentCategory1 = debitNote.SegmentCategory1;
            invDTO.SegmentCategory2 = debitNote.SegmentCategory2;

            invDTO.GrandTotal = debitNote.BalanceAmount;
            invDTO.BalanceAmount = debitNote.BalanceAmount;

            invDTO.IsAllowableNonAllowable = debitNote.IsAllowableNonAllowable;
            invDTO.IsAllowableDisallowableActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, debitNote.CompanyId);
            invDTO.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
            invDTO.NoSupportingDocument = debitNote.NoSupportingDocs;

            //invDTO.SegmentMasterid1 = debitNote.SegmentMasterid1;

            //if (debitNote.SegmentMasterid1 != null)
            //{
            //    var segment1 = _segmentMasterService.GetSegmentMastersById(debitNote.SegmentMasterid1.Value).FirstOrDefault(); ;
            //    invDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
            //}
            //if (debitNote.SegmentMasterid2 != null)
            //{
            //    var segment2 = _segmentMasterService.GetSegmentMastersById(debitNote.SegmentMasterid2.Value).FirstOrDefault(); ;
            //    invDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
            //}

            //invDTO.SegmentMasterid2 = debitNote.SegmentMasterid2;
            //invDTO.SegmentDetailid1 = debitNote.SegmentDetailid1;
            //invDTO.SegmentDetailid2 = debitNote.SegmentDetailid2;
            invDTO.Remarks = debitNote.Remarks;
            invDTO.Status = debitNote.Status;
            invDTO.DocumentState = debitNote.DocumentState;
            invDTO.CreatedDate = debitNote.CreatedDate;
            invDTO.UserCreated = debitNote.UserCreated;

            invDTO.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
            invDTO.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
            invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
            invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
            invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
            invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
        }
        private void FillDoubtfulDebtAllocation(DoubtfulDebtAllocationModel DDAModel, DebitNote debitNote, DoubtfulDebtModel invDTO)
        {
            DDAModel.Id = Guid.NewGuid();
            DDAModel.CompanyId = debitNote.CompanyId;
            DDAModel.InvoiceId = invDTO.Id;
            DDAModel.DocNo = debitNote.DocNo;
            DDAModel.DoubtfulDebitAllocationDate = invDTO.DocDate;
            DDAModel.DocCurrency = debitNote.DocCurrency;
            DDAModel.DoubtfulDebtAmount = debitNote.BalanceAmount;
            DDAModel.DoubtfulDebtBalanceAmount = debitNote.BalanceAmount;
            DDAModel.AllocateAmount = debitNote.BalanceAmount;
            DDAModel.DoubtfulDebtAllocationNumber = debitNote.DebitNoteNumber;
            DDAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
            DDAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
            DDAModel.Status = DoubtfulDebtAllocationStatus.Posted;
            DDAModel.DocDate = debitNote.DocDate;
            DDAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, invDTO.CompanyId);
            DDAModel.NoSupportingDocument = false;
        }
        private void FillDoubtfulDebtAllocationDetail(DoubtfulDebtAllocationDetailModel dDAD, DebitNote debitNote, DoubtfulDebtAllocationModel DDAModel)
        {
            dDAD.Id = Guid.NewGuid();
            dDAD.DoubtfulDebitAllocationId = DDAModel.Id;
            dDAD.DocType = DocTypeConstants.DebitNote;
            //dDAD.DocumentId = debitNoteId;
            dDAD.DocCurrency = DDAModel.DocCurrency;
            dDAD.DocAmount = debitNote.GrandTotal;
            dDAD.DocDate = debitNote.DocDate;
            dDAD.DocumentId = debitNote.Id;
            dDAD.DocNo = debitNote.DocNo;
            dDAD.SystemReferenceNumber = debitNote.DebitNoteNumber;
            dDAD.AllocateAmount = debitNote.BalanceAmount;
            dDAD.BalanceAmount = debitNote.BalanceAmount;
            dDAD.Nature = debitNote.Nature;
        }

        //private string GetNewProvisionDocumentNumber(long CompanyId)
        //{
        //    string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewProvisionDocumentNumber_GetCall_Request_Message);
        //        //Provision provision = _provisionService.Query(a => a.CompanyId == CompanyId && a.RefDocType == DocTypeConstants.DebitNote).Select().OrderByDescending(b => b.CreatedDate).FirstOrDefault();

        //        if (provision != null)
        //        {
        //            string strOldNo = String.Empty;
        //            Provision duplicatInvoice;
        //            int index;
        //            strOldDocNo = provision.DocNo;

        //            for (int i = strOldDocNo.Length - 1; i >= 0; i--)
        //            {
        //                if (Char.IsDigit(strOldDocNo[i]))
        //                    strOldNo = strOldDocNo[i] + strOldNo;
        //                else
        //                    break;
        //            }
        //            long docNo = 0;
        //            try
        //            { docNo = long.Parse(strOldNo); }
        //            catch { }

        //            index = strOldDocNo.LastIndexOf(strOldNo);
        //            do
        //            {
        //                docNo++;
        //                strNewNo = docNo.ToString().PadLeft(strOldNo.Length, '0');
        //                strNewDocNo = (docNo == 1) ? strOldDocNo + strNewNo : strOldDocNo.Substring(0, index) + strNewNo;

        //                duplicatInvoice = _provisionService.Query(a => a.DocNo == strNewDocNo && a.CompanyId == CompanyId).Select().FirstOrDefault();
        //            } while (duplicatInvoice != null);
        //        }
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewProvisionDocumentNumber_GetCall_Request_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewProvisionDocumentNumber_GetCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return strNewDocNo;
        //}

        private string GetNewReceiptDocNo(string docType, long CompanyId)
        {
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_Request_Message);
                Receipt receipt = _receiptService.GetReceiptByComapnyId(CompanyId, docType);

                if (receipt != null)
                {
                    string strOldNo = String.Empty;
                    Receipt duplicatInvoice;
                    int index;
                    strOldDocNo = receipt.DocNo;

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

                        duplicatInvoice = _receiptService.GetDuplicateReceipt(CompanyId, docType, strNewDocNo);
                    } while (duplicatInvoice != null);
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return strNewDocNo;
        }
        #endregion

        #region AutoNumber Block
        string value = "";
        public string GenerateAutoNumberForType(long CompanyId, string Type, string companyCode)
        {
            string generatedAutoNumber = "";
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GenerateAutoNumberForType_GenerateCall_Request_Message);
                AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(CompanyId, Type);

                if (Type == DocTypeConstants.DebitNote || Type == DocTypeConstants.Provision || Type == DocTypeConstants.CreditNote)
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
                        AppsWorld.DebitNoteModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                        _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                    }
                    else
                    {
                        AppsWorld.DebitNoteModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.DebitNoteModule.Entities.AutoNumberCompany();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.Id = Guid.NewGuid();
                        _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                        _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                    }

                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GenerateAutoNumberForType_GenerateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GenerateAutoNumberForType_GenerateCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return generatedAutoNumber;
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, string Companycode = null)
        {
            List<DebitNote> lstDebitNote = null;
            int? currentMonth = 0;
            bool ifMonthContains = false;
            string OutputNumber = "";
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GenerateFromFormat_GenerateCall_Request_Message);
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
                    ifMonthContains = true;
                }
                else if (companyFormatHere.Contains("{COMPANYCODE}"))
                {
                    companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
                }
                double val = 0;
                if (Type == DocTypeConstants.DebitNote)
                {
                    lstDebitNote = _debitNoteService.GetAllDebitModel(companyId);

                    if (lstDebitNote.Any() && ifMonthContains)
                    {
                        AppsWorld.DebitNoteModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        int? lastCratedMonth = lstDebitNote.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                        if (currentMonth == lastCratedMonth)
                        {
                            foreach (var debitNote in lstDebitNote)
                            {
                                if (debitNote.DebitNoteNumber != autonumber.Preview)
                                    val = Convert.ToInt32(IncreamentVal);
                                else
                                {
                                    val = Convert.ToInt32(IncreamentVal) + 1;
                                    break;
                                }
                            }
                        }
                    }
                    else if (lstDebitNote.Any() && ifMonthContains == false)
                    {
                        AppsWorld.DebitNoteModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        foreach (var debitNote in lstDebitNote)
                        {
                            if (debitNote.DebitNoteNumber != autonumber.Preview)
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

                if (lstDebitNote.Any())
                {
                    OutputNumber = GetNewNumber(lstDebitNote, Type, OutputNumber, counter, companyFormatHere, counterLength);
                }

                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GenerateFromFormat_GenerateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GenerateFromFormat_GenerateCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return OutputNumber;

        }

        private string GetNewNumber(List<DebitNote> lstDebitnote, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstDebitnote.Where(a => a.DebitNoteNumber == outputNumber).FirstOrDefault();
            bool isNotexist = false;
            int i = Convert.ToInt32(counter);
            if (invoice != null)
            {
                while (isNotexist == false)
                {
                    i++;
                    string length = i.ToString();
                    value = length.PadLeft(counterLength, '0');
                    val2 = format + value;
                    var inv = lstDebitnote.Where(c => c.DebitNoteNumber == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }

        #endregion

        #region JV Methods
        public void SaveInvoice1(JVModel clientModel)
        {
            //LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, clientModel);
            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == DebitNoteConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
                if (response.ErrorMessage != null)
                {
                    Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);

                var message = ex.Message;
            }
        }
        public void deleteJVPostDebitNote(JournalSaveModel tObject)
        {
            //LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, tObject);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == DebitNoteConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = tObject;
                // string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                if (response.ErrorMessage != null)
                {
                    Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);

                var message = ex.Message;
            }
        }
        private void FillJournal(JVModel headJournal, DebitNote invoice, bool isNew)
        {
            decimal? baseAmount = 0;
            int count = 1;
            string strServiceCompany = _companyService.GetIdBy(invoice.ServiceCompanyId.Value);

            TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice.CreditTermsId);
            //JournalModel headJournal = new JournalModel();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillJV(headJournal, invoice, strServiceCompany, top);
            ChartOfAccount account =
               _chartOfAccountService.GetChartOfAccountByName(
                   invoice.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables,
                   invoice.CompanyId);
            headJournal.COAId = account.Id;
            headJournal.AccountCode = account.Code;
            headJournal.AccountName = account.Name;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();

            foreach (var detail in invoice.DebitNoteDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillJvDetails(invoice, journal, detail);
                //var acc = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
                //journal.COAId = acc.Id;
                journal.COAId = detail.COAId;
                //journal.RecOrder = recOreder + 1;
                //recOreder = journal.RecOrder;

                journal.RecOrder = detail.RecOrder;
                lstJD.Add(journal);
            }
            if (invoice.IsGstSettings)
            {
                ChartOfAccount gstAccount = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.TaxPayableGST, invoice.CompanyId);

                foreach (var detail in invoice.DebitNoteDetails.Where(c => c.TaxRate != null && c.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    FillJvGstDetail(invoice, journal, detail, gstAccount);
                    if (journal.TaxCode != "NA" || journal.TaxRate != 0 || journal.TaxRate != null)
                    {
                        //journal.RecOrder = recOreder + 1;
                        //recOreder = journal.RecOrder;
                        journal.RecOrder = detail.RecOrder;
                        lstJD.Add(journal);
                    }
                    //journal.CreditDC = detail.DocTaxAmount == null ? 0 : detail.DocTaxAmount.Value;
                    //journal.CreditBC = detail.BaseTaxAmount == null ? 0 : detail.BaseTaxAmount.Value;
                    //baseTotal += journal.CreditBC.Value;
                    //journal.CreditGSTR = Math.Round((decimal)(journal.CreditDC * invoice.GSTExchangeRate), 2);
                }
            }
            baseAmount = lstJD.Sum(c => c.BaseCredit);
            JVVDetailModel jModel = new JVVDetailModel();
            FillJVinDetail(invoice, jModel);
            if (headJournal.ModifiedDate != null && headJournal.ModifiedBy != null)
                jModel.AmountDue = invoice.DocumentState != InvoiceState.NotPaid ? headJournal.BalanceAmount : null;
            // jModel.BaseDebit = baseAmount;
            //jModel.RecOrder = recOreder + 1;
            //recOreder = jModel.RecOrder;
            jModel.COAId = account.Id;
            lstJD.Add(jModel);
            headJournal.GrandBaseDebitTotal = baseAmount;
            headJournal.JVVDetailModels = lstJD.OrderBy(x => x.RecOrder).ToList();
        }

        private void FillJvGstDetail(DebitNote invoice, JVVDetailModel journal, DebitNoteDetail detail, ChartOfAccount gstAccount)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.DocNo = invoice.DocNo;
            journal.DocType = DocTypeConstants.DebitNote;
            journal.DocSubType = "General";
            //journal.AccountDescription = detail.AccountDescription;
            journal.AccountDescription = invoice.Remarks;
            //account = _chartOfAccountService.GetChartOfAccountById(gstAccount.Id);
            journal.COAId = gstAccount.Id;
            journal.AccountCode = gstAccount.Code;
            journal.PostingDate = invoice.DocDate;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.AccountName = gstAccount.Name;
            journal.BaseCurrency = invoice.ExCurrency;
            journal.DocCurrency = invoice.DocCurrency;
            journal.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxRate = detail.TaxRate;
            }
            journal.DocCredit = detail.DocTaxAmount.Value;
            journal.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? journal.DocCredit.Value : (journal.DocCredit * invoice.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            journal.IsTax = true;
        }

        private void FillJvDetails(DebitNote invoice, JVVDetailModel journal, DebitNoteDetail detail)
        {
            //ChartOfAccount account;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.SystemRefNo = invoice.DebitNoteNumber;
            journal.DocType = DocTypeConstants.DebitNote;
            journal.DocSubType = DocTypeConstants.General;
            journal.DocCurrency = invoice.DocCurrency;
            journal.BaseCurrency = invoice.ExCurrency;
            journal.PostingDate = invoice.DocDate;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.DocNo = invoice.DocNo;
            journal.EntityId = invoice.EntityId;
            journal.DocDate = invoice.DocDate;
            //journal.DocSubType = invoice.DocSubType;
            journal.AccountDescription = detail.AccountDescription;
            journal.COAId = detail.COAId;
            //account = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
            //journal.COAId = account.Id;
            //journal.AccountCode = account.Code;
            //journal.AccountName = account.Name;
            journal.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            journal.AllowDisAllow = detail.AllowDisAllow;
            //if (invoice.IsGstSettings)
            //{
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxRate = detail.TaxRate;
            }
            //}
            journal.DocCredit = detail.DocAmount;
            journal.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? journal.DocCredit.Value : (journal.DocCredit * invoice.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            journal.DocTaxCredit = detail.DocTaxAmount != null ? detail.DocTaxAmount : null;
            journal.BaseTaxCredit = detail.BaseTaxAmount != null ? detail.BaseTaxAmount : null;
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.BaseTaxableAmount = detail.BaseAmount;

            journal.BaseTaxAmount = detail.BaseTaxAmount;
            //journal.SegmentCategory1 = invoice.SegmentCategory1;
            //journal.SegmentCategory2 = invoice.SegmentCategory2;
            //journal.SegmentMasterid1 = invoice.SegmentMasterid1;
            //journal.SegmentMasterid2 = invoice.SegmentMasterid2;
            //journal.SegmentDetailid1 = invoice.SegmentDetailid1;
            //journal.SegmentDetailid2 = invoice.SegmentDetailid2;
            journal.IsTax = false;
        }

        private static void FillJVinDetail(DebitNote invoice, JVVDetailModel jModel)
        {
            jModel.DocumentId = invoice.Id;
            jModel.SystemRefNo = invoice.DebitNoteNumber;
            jModel.DocNo = invoice.DocNo;
            jModel.DocDate = invoice.DocDate;
            jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocSubType = "General";
            jModel.DocType = DocTypeConstants.DebitNote;
            jModel.Remarks = invoice.Remarks;
            jModel.PONo = invoice.PONo;
            jModel.AccountDescription = invoice.Remarks;
            jModel.PostingDate = invoice.DocDate;
            jModel.CreditTermsId = invoice.CreditTermsId;
            jModel.DueDate = invoice.DueDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            jModel.DocDebit = invoice.GrandTotal;
            //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? jModel.DocDebit.Value : (jModel.DocDebit * jModel.ExchangeRate).Value, 2);
            decimal amount = 0;
            //newlly modified code
            amount = (invoice.GrandTotal != null && invoice.GrandTotal != 0) ? invoice.ExchangeRate != null ? Math.Round(invoice.GrandTotal * (decimal)invoice.ExchangeRate, 2, MidpointRounding.AwayFromZero) : Math.Round(invoice.GrandTotal, 2, MidpointRounding.AwayFromZero) : invoice.GrandTotal;

            //foreach (var detail in invoice.DebitNoteDetails)
            //{
            //    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
            //    amount = Math.Round((decimal)(amount + (detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            //}
            jModel.BaseDebit = amount;
            jModel.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            //jModel.SegmentCategory1 = invoice.SegmentCategory1;
            //jModel.SegmentCategory2 = invoice.SegmentCategory2;
            //jModel.SegmentMasterid1 = invoice.SegmentMasterid1;
            //jModel.SegmentMasterid2 = invoice.SegmentMasterid2;
            //jModel.SegmentDetailid1 = invoice.SegmentDetailid1;
            //jModel.SegmentDetailid2 = invoice.SegmentDetailid2;
        }

        private void FillJV(JVModel headJournal, DebitNote invoice, string strServiceCompany, TermsOfPayment top)
        {
            headJournal.DocumentId = invoice.Id;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = invoice.DocDate;
            headJournal.CreditTermsId = invoice.CreditTermsId;
            headJournal.DocNo = invoice.DocNo;
            headJournal.DocType = DocTypeConstants.DebitNote;
            headJournal.DocSubType = "General";
            headJournal.DocDate = invoice.DocDate;
            headJournal.DueDate = invoice.DueDate;
            headJournal.DocumentState = invoice.DocumentState;
            headJournal.SystemReferenceNo = invoice.DebitNoteNumber;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = invoice.Nature;
            headJournal.PONo = invoice.PONo;
            headJournal.ExDurationFrom = invoice.ExDurationFrom;
            headJournal.ExDurationTo = invoice.ExDurationTo;
            headJournal.GSTExDurationFrom = invoice.GSTExDurationFrom;
            headJournal.IsGstSettings = invoice.IsGstSettings;
            headJournal.IsGSTApplied = invoice.IsGSTApplied;
            headJournal.IsMultiCurrency = invoice.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = invoice.IsNoSupportingDocument;
            headJournal.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
            headJournal.GSTExDurationTo = invoice.GSTExDurationTo;
            headJournal.CreditTerms = (int)top.TOPValue;
            //headJournal.IsSegmentReporting = invoice.IsSegmentReporting;
            //headJournal.SegmentCategory1 = invoice.SegmentCategory1;
            //headJournal.SegmentCategory2 = invoice.SegmentCategory2;
            //headJournal.SegmentMasterid1 = invoice.SegmentMasterid1;
            //headJournal.SegmentMasterid2 = invoice.SegmentMasterid2;
            //headJournal.SegmentDetailid1 = invoice.SegmentDetailid1;
            //headJournal.SegmentDetailid2 = invoice.SegmentDetailid2;
            headJournal.NoSupportingDocument = invoice.NoSupportingDocs;
            headJournal.BalanceAmount = invoice.BalanceAmount;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            headJournal.EntityType = invoice.EntityType;
            //ChartOfAccount account =
            //    _chartOfAccountService.GetChartOfAccountByName(
            //        invoice.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables,
            //        invoice.CompanyId);
            //headJournal.COAId = account.Id;
            //headJournal.AccountCode = account.Code;
            //headJournal.AccountName = account.Name;
            headJournal.DocCurrency = invoice.DocCurrency;
            headJournal.GrandDocDebitTotal = invoice.GrandTotal;
            headJournal.BaseCurrency = invoice.ExCurrency;
            headJournal.ExchangeRate = invoice.ExchangeRate;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(invoice.GrandTotal * invoice.ExchangeRate), 2);
            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.Remarks = invoice.Remarks;
            headJournal.DocumentDescription = invoice.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = invoice.CreatedDate;
            headJournal.ModifiedBy = invoice.ModifiedBy;
            headJournal.ModifiedDate = invoice.ModifiedDate;
        }

        #endregion

        #region Save DebitNote Notes
        public List<DebitNoteNote> SaveDebitNoteNote(List<DebitNoteNoteModel> TObject, string ConnectionString)
        {
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteNotes_UpdateCall_Request_Message);
                DebitNoteNote debitnotenote = new DebitNoteNote();
                List<DebitNoteNote> DebitNoteNotes = new List<DebitNoteNote>();
                DebitNoteNotes = _debitNoteNoteService.AllDebitNoteNote(TObject.Select(x => x.DebitNoteId).FirstOrDefault());
                var debitnote = _debitNoteService.GetDebittNote(TObject.Select(x => x.DebitNoteId).FirstOrDefault());
                if (debitnote != null)
                {
                    if (TObject != null)
                    {
                        foreach (DebitNoteNoteModel note in TObject)
                        {
                            if (DebitNoteNotes.Any())
                            {
                                if (note.RecordStatus == "Added")
                                {
                                    InsertDebitNoteNote(debitnotenote, note);
                                    DebitNoteNotes.Add(debitnotenote);
                                    _debitNoteNoteService.Insert(debitnotenote);
                                }
                                else if (note.RecordStatus != "Added" && note.RecordStatus != "Deleted" && note.RecordStatus != null)
                                {
                                    DebitNoteNote debitnoteNote = DebitNoteNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                                    if (debitnoteNote != null)
                                    {
                                        debitnoteNote.DebitNoteId = note.DebitNoteId;
                                        debitnoteNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
                                        debitnoteNote.Notes = note.Notes;
                                        debitnoteNote.ModifiedDate = DateTime.UtcNow;
                                        debitnoteNote.ModifiedBy = note.ModifiedBy;
                                        debitnoteNote.ObjectState = ObjectState.Modified;
                                    }
                                }
                                else if (note.RecordStatus == "Deleted")
                                {
                                    DebitNoteNote debitnoteNote = DebitNoteNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                                    if (debitnoteNote != null)
                                    {
                                        debitnoteNote.ObjectState = ObjectState.Deleted;
                                    }
                                }
                            }
                            else
                            {
                                if (note.RecordStatus == "Added")
                                {
                                    InsertDebitNoteNote(debitnotenote, note);
                                    _debitNoteNoteService.Insert(debitnotenote);
                                }
                            }
                            LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteNotes_UpdateCall_SuccessFully_Message);
                        }
                        _unitOfWork.SaveChanges();
                        #region Update AddNote For Journal 
                        var journal = _journalService.GetJournals(debitnote.Id, debitnote.CompanyId);
                        if (journal != null)
                        {
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                query = $"Update Bean.Journal Set IsAddNote=1 where DocumentId='{debitnote.Id}' and CompanyId={debitnote.CompanyId}";
                                cmd = new SqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        #endregion
                        #region Update_Note_In_Journal if delete all Records of Notes
                        DebitNoteNotes = _debitNoteNoteService.AllDebitNoteNote(debitnote.Id);
                        if (!DebitNoteNotes.Any())
                            if (journal.IsAddNote == true)
                            {
                                using (con = new SqlConnection(ConnectionString))
                                {
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    query = $"Update Bean.Journal Set IsAddNote=0 where DocumentId='{debitnote.Id}' and CompanyId={debitnote.CompanyId}";
                                    cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                        #endregion Update_Note_In_Journal

                    }

                }
                else
                {
                    throw new Exception("DebitNote Id is Invalid");
                }
                return DebitNoteNotes;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_UpdateDebitNoteNotes_UpdateCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        public void InsertDebitNoteNote(DebitNoteNote debitnotenote, DebitNoteNoteModel note)
        {
            debitnotenote.Id = note.Id/*Guid.NewGuid()*/;
            debitnotenote.CreatedDate = DateTime.UtcNow;
            debitnotenote.ExpectedPaymentDate = note.ExpectedPaymentDate;
            debitnotenote.Notes = note.Notes;
            debitnotenote.DebitNoteId = note.DebitNoteId;
            debitnotenote.UserCreated = note.UserCreated;
            debitnotenote.ObjectState = ObjectState.Added;
        }

    }
}
