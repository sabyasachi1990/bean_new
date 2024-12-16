using AppaWorld.Bean;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Infra;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.Service;
using AppsWorld.Framework;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.Entities.Models;
using AppsWorld.InvoiceModule.Infra;
using AppsWorld.InvoiceModule.Infra.Resources;
using AppsWorld.InvoiceModule.Models;
using AppsWorld.InvoiceModule.RepositoryPattern;
using AppsWorld.InvoiceModule.Service;
using Logger;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ziraff.FrameWork.Logging;

namespace AppsWorld.InvoiceModule.Application
{
    public class InvoiceApplicationService
    {
        private readonly IInvoiceEntityService _invoiceEntityService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly ICompanySettingService _companySettingService;
        private readonly IGSTSettingService _gstSettingService;
        private readonly IMultiCurrencySettingService _mltiCurrencySettingService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly ITaxCodeService _taxCodeService;
        private readonly IInvoiceNoteService _invoiceNoteService;
        private readonly ICreditNoteApplicationDetailService _creditNoteApplicationDetailService;
        private readonly ICreditNoteApplicationService _creditNoteApplicationService;
        private readonly IDoubtfulDebtallocationDetailService _doubtfulDebtallocationDetailService;
        private readonly IDoubtfulDebtAllocationService _doubtfulDebtAllocationService;
        private readonly ICurrencyService _currencyService;
        private readonly ICompanyService _companyService;
        private readonly ITermsOfPaymentService _termsOfPaymentService;
        private readonly IInvoiceModuleUnitOfWorkAsync unitOfWork;
        private readonly AppsWorld.InvoiceModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly AppsWorld.InvoiceModule.Service.IAutoNumberService _autoNumberService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        private readonly IDebitNoteService _debitNoteService;
        private readonly AppsWorld.InvoiceModule.Service.IJournalService _journalService;
        private readonly IReceiptDetailService _receiptDetailService;
        private readonly IReceiptService _receiptService;
        private readonly IJournalDetailService _journalDetailService;
        private readonly IItemService _itemService;
        private readonly ICommonForexService _commonForexService;


        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string query = string.Empty;


        public InvoiceApplicationService(IInvoiceModuleUnitOfWorkAsync unitOfWork, IInvoiceEntityService invoiceEntityService, IFinancialSettingService financialSettingService, ICompanySettingService companySettingService, IGSTSettingService gstSettingService, IMultiCurrencySettingService mltiCurrencySettingService, IBeanEntityService beanEntityService, IChartOfAccountService chartOfAccountService, IInvoiceDetailService invoiceDetailService, ITaxCodeService taxCodeService, IInvoiceNoteService invoiceNoteService, ICreditNoteApplicationDetailService creditNoteApplicationDetailService, ICreditNoteApplicationService creditNoteApplicationService, IDoubtfulDebtallocationDetailService doubtfulDebtallocationDetailService, IDoubtfulDebtAllocationService doubtfulDebtAllocationService, ICurrencyService currencyService, ICompanyService companyService, ITermsOfPaymentService termsOfPaymentService, AppsWorld.InvoiceModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.InvoiceModule.Service.IAutoNumberService autoNumberService, IDebitNoteService debitNoteService, AppsWorld.InvoiceModule.Service.IJournalService journalService, IReceiptDetailService receiptDetailService, ReceiptService receiptService, IJournalDetailService journalDetailService, IItemService itemService, IAccountTypeService accountTypeService, AppsWorld.CommonModule.Service.IAutoNumberService autoService, ICommonForexService commonForexService)
        {
            this._invoiceEntityService = invoiceEntityService;
            this._financialSettingService = financialSettingService;
            this._companySettingService = companySettingService;
            this._gstSettingService = gstSettingService;
            this._mltiCurrencySettingService = mltiCurrencySettingService;
            this._beanEntityService = beanEntityService;
            this._chartOfAccountService = chartOfAccountService;
            this._invoiceDetailService = invoiceDetailService;
            this._taxCodeService = taxCodeService;
            this._invoiceNoteService = invoiceNoteService;
            this._creditNoteApplicationDetailService = creditNoteApplicationDetailService;
            this._creditNoteApplicationService = creditNoteApplicationService;
            this._doubtfulDebtallocationDetailService = doubtfulDebtallocationDetailService;
            this._doubtfulDebtAllocationService = doubtfulDebtAllocationService;
            this._currencyService = currencyService;
            this._companyService = companyService;
            this._termsOfPaymentService = termsOfPaymentService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._autoNumberService = autoNumberService;
            this._debitNoteService = debitNoteService;
            this.unitOfWork = unitOfWork;
            this._journalService = journalService;
            this._receiptDetailService = receiptDetailService;
            this._receiptService = receiptService;
            this._journalDetailService = journalDetailService;
            this._itemService = itemService;
            this._accountTypeService = accountTypeService;
            this._autoService = autoService;
            this._commonForexService = commonForexService;

        }

        #region Invoice
        #region Invoice Kendo


        public async Task<IQueryable<InvoiceModelK>> GetAllInvoicesKOld(string userName, long companyId)
        {
            return await _invoiceEntityService.GetAllInvoicesKOld(userName, companyId, InvoiceState.Posted);
        }

        public async Task<Tuple<IQueryable<InvoiceModelK>, int>> GetAllInvoicesK(string userName, long companyId)
        {
            return await _invoiceEntityService.GetAllInvoicesK(userName, companyId, InvoiceState.Posted);
        }
        public IQueryable<InvoiceModelK> GetAllParkedInvoicesK(string username, long companyId)
        {
            //return _invoiceEntityService.GetAllInvoicesK(username, companyId, InvoiceState.Parked);
            return _invoiceEntityService.GetAllParkedInvoice(username, companyId, InvoiceState.Parked);
        }
        public async Task<IQueryable<RecurringInvoiceK>> GetAllRecurringInvoicesK(string userName, long companyId)
        {
            return await _invoiceEntityService.GetAllRecurringInvoicesK(userName, companyId);
        }
        public IQueryable<InvoiceVoidK> GetAllVoidInvoicesK(long companyId)
        {
            return _invoiceEntityService.GetAllVoidInvoicesK(companyId);
        }

        public IQueryable<InvoiceModelK> GetAllRecurringPostedInvoicesK(long companyId, Guid id)
        {
            return _invoiceEntityService.GetAllRecuurringPostedInvoicesK(companyId, InvoiceState.Posted, id);
        }

        #endregion

        #region Create Invoice

        public async Task<InvoiceModel> CreateInvoice(long companyid, Guid id, bool isCopy, string connectionString)
        {
            InvoiceModel invDTO = new InvoiceModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_Request_Message);
                FinancialSetting financSettings = await _financialSettingService.GetFinancialSettingAsync(companyid);
                if (financSettings == null)
                {
                    throw new Exception(InvoiceValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice invoice = await _invoiceEntityService.GetCompanyAndIdAsync(companyid, id);
                if (invoice == null)
                {
                    Invoice lastInvoiceDate = await _invoiceEntityService.GetByCompanyIdForInvoiceAsync(companyid, "Invoice");
                    invDTO.Id = Guid.NewGuid();
                    invDTO.CompanyId = companyid;
                    invDTO.DocDate = lastInvoiceDate == null ? DateTime.Now : lastInvoiceDate.DocDate;
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.Invoice);
                    if (invDTO.IsDocNoEditable == true)
                    {
                        invDTO.DocNo = _autoService.GetAutonumber(companyid, DocTypeConstant.Invoice, connectionString);
                    }
                    else
                    {
                        invDTO.DocNo = null;
                    }
                    invDTO.DocumentState = "Not Paid";
                    invDTO.DueDate = DateTime.UtcNow;
                    invDTO.NoSupportingDocument = false;
                    invDTO.IsRepeatingInvoice = false;
                    invDTO.CreatedDate = DateTime.UtcNow;

                    invDTO.BaseCurrency = financSettings.BaseCurrency;
                    invDTO.DocCurrency = invDTO.BaseCurrency;
                    invDTO.IsLocked = false;

                }
                else
                {

                    FillInvoice(invDTO, invoice, isCopy);

                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.Invoice);
                    if (isCopy)
                        invDTO.DocNo = isCopy && invDTO.IsDocNoEditable == true ? _autoService.GetAutonumber(companyid, DocTypeConstant.Invoice, connectionString) : null;
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return invDTO;
        }

        private string GetAutoNumberByEntityType(long companyId, Invoice lastInvoice, string entityType, AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo, bool isInvoice, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;

            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    if (entityType == InvoiceState.Recurring)
                        outPutNumber = GetRecInvoiceDocumentNumber(DocTypeConstants.Invoice, InvoiceState.Recurring, companyId);
                    else
                        outPutNumber = GetNewInvoiceDocumentNumber(entityType, companyId, isInvoice);
                    //invDTO.IsEditable = true;
                    if (entityType == DocTypeConstants.CreditNote && outPutNumber == null)
                    {
                        outPutNumber = _autoNo.Preview;
                    }
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
                                //isExist = lstInvoice.Where(a => a.DocNo.Equals(outPutNumber)).Any();
                                //while (isExist)
                                //{
                                //    counter++;
                                //    outPutNumber = autonoFormat + counter.ToString().PadLeft(_autoNo.CounterLength.Value, '0');
                                //    if (lstInvoice.Where(a => a.DocNo.Equals(outPutNumber)).Any() == false)
                                //        isExist = false;
                                //}
                            }
                            else
                            {
                                string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                        }
                        else if (_autoNo.Format.Contains("{YYYY}"))
                        {
                            if (DateTime.UtcNow.Year == lastInvoice.CreatedDate.Value.Year)
                            {
                                string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                            else
                            {
                                string output = "1";
                                //string output = Convert.ToString(_autoNo.StartNumber);
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
                        //string output = Convert.ToString(_autoNo.StartNumber);
                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //counter = Convert.ToInt32(value);
                    }
                }
            }
            return outPutNumber;
        }



        #endregion

        #region Invoice LookUp
        public InvoiceModelLU GetAllInvoiceLUs(string username, Guid invoiceId, long companyid, string connectionString)
        {
            InvoiceModelLU invoiceLU = new InvoiceModelLU();
            try
            {
                List<long?> taxIds = null;
                bool isObInvoice = false;
                string query = null;
                SqlConnection con = null;
                //Invoice lastInvoice = _invoiceEntityService.GetByCompanyId(companyid);//commented for optimizing purpose
                DateTime? lastPostedDocDate = _invoiceEntityService.GetLastPostedDate(companyid);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Request_Message);
                Invoice invoice = _invoiceEntityService.GetAllInvoiceLu(companyid, invoiceId);
                //DateTime date = invoice == null ? lastInvoice == null ? DateTime.Now : lastInvoice.DocDate : invoice.DocDate;//commented for optimizing purpose
                DateTime date = invoice == null ? lastPostedDocDate == null ? DateTime.Now : lastPostedDocDate.Value : invoice.DocDate;
                Guid invoiceGuid = invoiceId;
                //invoiceLU.NatureLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyid, ControlCodeConstants.Control_Codes_Nature);
                invoiceLU.NatureLU = new List<string> { "Trade", "Others", "Interco" };
                //invoiceLU.AllowableNonallowableLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyid, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                //invoiceLU.DocumentTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyid, ControlCodeConstants.Control_codes_DocumentType);
                if (invoice != null && invoice.InvoiceDetails.Count > 0)
                    taxIds = invoice.InvoiceDetails.Select(x => x.TaxId).ToList();
                invoiceLU.CompanyId = companyid;
                if (invoice != null)
                {
                    invoiceLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyid, invoice.DocCurrency, ControlCodeConstants.Currency_DefaultCode);
                    //var lookUpNature = _controlCodeCategoryService.GetInactiveControlcode(companyid,
                    //       ControlCodeConstants.Control_Codes_Nature, invoice.Nature);
                    //if (lookUpNature != null)
                    //{
                    //    invoiceLU.NatureLU.Lookups.Add(lookUpNature);
                    //}
                }
                else
                {
                    invoiceLU.CurrencyLU = _currencyService.GetByCurrencies(companyid, ControlCodeConstants.Currency_DefaultCode);
                }
                //List<LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyid);
                //if (invoice == null)
                //{
                //    if (segments.Count > 0)
                //        invoiceLU.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        invoiceLU.SegmentCategory2LU = segments[1];
                //    // invoiceLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyid);
                //}
                //else
                //{
                //    if (invoice.SegmentMasterid1 != null)
                //        invoiceLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyid, invoice.SegmentMasterid1);
                //    else
                //        if (segments.Count > 0)
                //        invoiceLU.SegmentCategory1LU = segments[0];
                //    if (invoice.SegmentMasterid2 != null)
                //        invoiceLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyid, invoice.SegmentMasterid2);
                //    else
                //        if (segments.Count > 1)
                //        invoiceLU.SegmentCategory2LU = segments[1];

                //}

                long? credit = invoice == null ? 0 : invoice.CreditTermsId == null ? 0 : invoice.CreditTermsId;
                invoiceLU.TermsOfPaymentLU = _termsOfPaymentService.Queryable().Where(a => (a.Status == RecordStatusEnum.Active || a.Id == credit) && a.CompanyId == companyid && a.IsCustomer == true).Select(x => new LookUp<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    TOPValue = x.TOPValue,
                    RecOrder = x.RecOrder
                }).OrderBy(c => c.TOPValue).ToList();
                long? comp = invoice == null ? 0 : invoice.ServiceCompanyId == null ? 0 : invoice.ServiceCompanyId;
                invoiceLU.SubsideryCompanyLU = _companyService.GetSubCompany(username, companyid, comp);
                //invoiceLU.SubsideryCompanyLU = invoiceLU.SubsideryCompanyLU.OrderBy(x => x.ShortName).ToList();
                List<COALookup<string>> lstEditCoa = new List<COALookup<string>>();
                List<TaxCodeLookUp<string>> lstEditTax = null;



                //Newlly Modified Code for COA new changes
                //List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
                if (invoice != null)
                {
                    if (invoice.IsOBInvoice == true)
                        isObInvoice = true;
                }
                /*{string.Join(",", coaName)}*/
                if (isObInvoice)
                    query = $"Select COA.Id,COA.Code,COA.Name,COA.Class,COA.DisAllowable,COA.Status, Case When Coa.Category='Income Statement' Then 1 Else 0 End as IsPLAccount, Case When COA.Class in('Assets','Liabilities','Equity') Then 1 Else 0 End as IsTaxCodeNotEditable from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId where ACT.CompanyId = {companyid} and COA.Name = 'Opening Balance' and ACT.Name in ('System') order by coa.Name";
                else
                    query = $"Select COA.Id,COA.Code,COA.Name,COA.Class,COA.DisAllowable,COA.Status, Case When Coa.Category = 'Income Statement' Then 1 Else 0 End as IsPLAccount, Case When COA.Class in('Assets','Liabilities','Equity') Then 1 Else 0 End as IsTaxCodeNotEditable,Case When Invd.COAId=COA.Id Then 1 Else 0 END as InterCoa from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.InvoiceDetail Invd on Invd.COAId = COA.Id and Invd.InvoiceId ='{invoiceId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and ACT.Name in ('Revenue','Other Income')  and(COA.Status = 1 or Invd.COAId = COA.Id)order by coa.Name";
                List<COALookup<string>> lstCoaLookUp = new List<COALookup<string>>();
                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        COALookup<string> coaLookup = new COALookup<string>();
                        coaLookup.Id = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : 0;
                        coaLookup.Name = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : null;
                        coaLookup.Code = dr["Code"] != DBNull.Value ? Convert.ToString(dr["Code"]) : null;
                        coaLookup.Class = dr["Class"] != DBNull.Value ? Convert.ToString(dr["Class"]) : null;
                        coaLookup.IsAllowDisAllow = dr["DisAllowable"] != DBNull.Value ? Convert.ToBoolean(dr["DisAllowable"]) : (bool?)null;
                        coaLookup.IsPLAccount = dr["IsPLAccount"] != DBNull.Value ? Convert.ToBoolean(dr["IsPLAccount"]) : (bool?)null;
                        coaLookup.Status = Convert.ToBoolean(dr["Status"]) == true ? RecordStatusEnum.Active : RecordStatusEnum.Inactive;
                        coaLookup.IsTaxCodeNotEditable = dr["IsTaxCodeNotEditable"] != DBNull.Value ? Convert.ToBoolean(dr["IsTaxCodeNotEditable"]) : (bool?)null;
                        coaLookup.IsInterCoBillingCOA = dr["InterCoa"] != DBNull.Value ? Convert.ToBoolean(dr["InterCoa"]) : (bool?)null;
                        lstCoaLookUp.Add(coaLookup);
                    }
                    con.Close();
                    invoiceLU.ChartOfAccountLU = lstCoaLookUp;
                }











                //commnted by lokanath 0n 19-09-2019 for interco changes
                ////string coaName = COANameConstants.Revenue;
                //List<AccountType> accType = _accountTypeService.GetAllAccounyTypeByName(companyid, coaName);
                //invoiceLU.ChartOfAccountLU = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                //{
                //    Name = x.Name,
                //    Code = x.Code,
                //    Id = x.Id,
                //    RecOrder = x.RecOrder,
                //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                //    Class = x.Class,
                //    Status = x.Status,
                //    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                //}).OrderBy(d => d.Name).ToList()).ToList();

                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodesNew(companyid, date, taxIds);
                if (allTaxCodes.Any())
                    invoiceLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (invoice != null && invoice.InvoiceDetails.Count > 0)
                {

                    //List<long> CoaIds = invoice.InvoiceDetails.Select(c => c.COAId).ToList();
                    //if (invoiceLU.ChartOfAccountLU.Any())
                    //    CoaIds = CoaIds.Except(invoiceLU.ChartOfAccountLU.Select(x => x.Id)).ToList();

                    if (invoiceLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(invoiceLU.TaxCodeLU.Select(d => d.Id)).ToList();
                    //if (invoice.IsWorkFlowInvoice == true)
                    //{
                    //    List<COALookup<string>> lstEditCoaAc = new List<COALookup<string>>();
                    //    ChartOfAccount roundingId = _chartOfAccountService.GetWorkFolwItemByCoaId(companyid);
                    //    COALookup<string> LstCoA = new COALookup<string>()
                    //    {
                    //        Name = roundingId.Name,
                    //        Code = roundingId.Code,
                    //        Id = roundingId.Id,
                    //        RecOrder = roundingId.RecOrder,
                    //        IsAllowDisAllow = roundingId.DisAllowable == true ? true : false,
                    //        IsPLAccount = roundingId.Category == "Income Statement" ? true : false,
                    //        Class = roundingId.Class,
                    //        Status = roundingId.Status,
                    //        IsTaxCodeNotEditable = (roundingId.Class == "Assets" || roundingId.Class == "Liabilities" || roundingId.Class == "Equity") ? true : false,
                    //    };
                    //    invoiceLU.ChartOfAccountLU.Add(LstCoA);
                    //}

                    //commnted by lokanath 0n 19-09-2019 for interco changes
                    //if (CoaIds.Any())
                    //{
                    //    lstEditCoa = accType.SelectMany(y => y.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                    //    {
                    //        Name = x.Name,
                    //        Code = x.Code,
                    //        Id = x.Id,
                    //        RecOrder = x.RecOrder,
                    //        IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    //        IsPLAccount = x.Category == "Income Statement" ? true : false,
                    //        Class = x.Class,
                    //        Status = x.Status,
                    //        IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                    //    }).OrderBy(d => d.Name).ToList()).ToList();
                    //    invoiceLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    //}

                    //commnted by lokanath 0n 19-09-2019 for interco changes
                    //if (invoice.IsOBInvoice == true)
                    //{
                    //    var chartOfAccount = _chartOfAccountService.GetChartOfAccountByName("Opening balance", companyid);
                    //    if (chartOfAccount != null)
                    //    {
                    //        List<COALookup<string>> lstOBCoa = new List<COALookup<string>>() { new COALookup<string>() { Name=chartOfAccount.Name,Code=chartOfAccount.Code,Id=chartOfAccount.Id,RecOrder=chartOfAccount.RecOrder,IsAllowDisAllow=chartOfAccount.DisAllowable==true?true:false,IsPLAccount=chartOfAccount.Category=="Income Statement"?true:false,Class=chartOfAccount.Class,Status=chartOfAccount.Status
                    //            } }.ToList();
                    //        invoiceLU.ChartOfAccountLU.AddRange(lstOBCoa);
                    //    }
                    //}

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
                        }).ToList();
                        invoiceLU.TaxCodeLU.AddRange(lstEditTax);
                        invoiceLU.TaxCodeLU = invoiceLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message + " for " + companyid);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return invoiceLU;
        }
        public async Task<InvoiceModelLU> NewGetAllInvoiceLUs(string userName, Guid invoiceId, long companyId, string connectionString)
        {
            InvoiceModelLU invoiceLU = new InvoiceModelLU();
            try
            {
                Invoice lastInvoice = await _invoiceEntityService.GetByCompanyIdForInvoiceAsync(companyId, "Invoice");
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Request_Message);
                Invoice invoice = await _invoiceEntityService.GetAllInvoiceLuAsync(companyId, invoiceId);
                DateTime date = lastInvoice?.DocDate ?? invoice?.DocDate ?? DateTime.UtcNow;
                invoiceLU.NatureLU = new List<string> { "Trade", "Others", "Interco" };
                long? credit = invoice?.CreditTermsId ?? 0;
                long? comp = invoice?.ServiceCompanyId ?? 0;
                List<CommonLookUps<string>> lstLookUps = new List<CommonLookUps<string>>();
                LookUpCategory<string> currency = new LookUpCategory<string>();
                string currencyCode = invoice != null ? invoice.DocCurrency : string.Empty;
                query = InvoiceCommonQuery(userName, companyId, date, credit, comp, currencyCode, invoiceId);
                int? resultSetCount = query.Split(';').Count();
                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    for (int i = 1; i <= resultSetCount; i++)
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
                    invoiceLU.CurrencyLU = currency;
                    invoiceLU.TermsOfPaymentLU = lstLookUps.Where(c => c.TableName == "TERMSOFPAY").Select(x => new LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();
                    invoiceLU.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.isGstActivated,
                        IsIBServiceEntity = x.IsInterCo
                    }).ToList();
                }


                if (invoice != null)
                {
                    if (invoice.IsOBInvoice == true)
                        invoiceLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "OBCHARTOFACCOUNT").Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement",
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity",
                        }).OrderBy(d => d.Name).ToList();
                    else
                        invoiceLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT").Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement",
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity",
                            IsInterCoBillingCOA = x.IsInterCo,
                        }).OrderBy(d => d.Name).ToList();
                }
                else
                    invoiceLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT").Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Code = x.Code,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsPLAccount = x.COACategory == "Income Statement",
                        Class = x.Class,
                        Status = x.Status,
                        IsTaxCodeNotEditable = x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity",
                        IsInterCoBillingCOA = x.IsInterCo,
                    }).OrderBy(d => d.Name).ToList();

                invoiceLU.TaxCodeLU = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
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

                if (invoice != null)
                {
                    var lsttax = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Inactive).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.TaxCode,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.TaxCode,


                        IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                    }).OrderBy(c => c.Code).ToList();
                    List<long?> taxIdss = invoice.InvoiceDetails.Select(x => x.TaxId).ToList();
                    if (invoiceLU.TaxCodeLU.Any())
                        taxIdss = taxIdss.Except(invoiceLU.TaxCodeLU.Select(d => d.Id)).ToList();
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
                        invoiceLU.TaxCodeLU.AddRange(lstTax);
                        invoiceLU.TaxCodeLU = invoiceLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_SuccessFully_Message);
            }

            catch (Exception ex)
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message + " for " + companyId);
                throw ex;
            }

            return invoiceLU;
        }
        private static string InvoiceCommonQuery(string username, long companyId, DateTime date, long? credit, long? comp, string currencyCode, Guid invoiceId)
        {

            return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY,0 as IsInterCo FROM Bean.Currency WHERE CompanyId={companyId} AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY,0 as IsInterCo FROM Common.TermsOfPayment where CompanyId={companyId} AND (Status=1 OR Id= {credit}) AND IsCustomer=1;Select distinct COA.Id,'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.InvoiceDetail Invd on Invd.COAId = COA.Id and Invd.InvoiceId = '{invoiceId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyId} and ACT.Name in ('Revenue','Other income')  and(COA.Status = 1 or Invd.COAId = COA.Id)order by coa.Name;SELECT 'OBCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,0 as IsInterCo FROM Bean.ChartOfAccount COA where COA.CompanyId ={companyId} and COA.Name='Opening balance';Select 'SERVICECOMPANY' as TABLENAME,comp.Id as ID,comp.Name as NAME,comp.ShortName as SHOTNAME,comp.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY,Case When comp.id=interDetail.ServiceEntityId Then 1 Else 0 End as IsInterCo from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Billing'and interDetail.Status=1 right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId JOIN Common.CompanyUser CU on comp.ParentId=CU.CompanyId Join common.CompanyUserDetail CUD On (Comp.Id = CUD.ServiceEntityId and CU.Id = CUD.CompanyuserId) where comp.ParentId = {companyId} and (comp.Status=1 or comp.Id={comp}) and CU.Username='{username}' order by comp.ShortName;SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxCode=Tax.Code Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyId} Right Join Bean.TaxCode Tax on Tax.Code=TaxMapDetail.CustTaxCode where Tax.CompanyId={companyId}  and Tax.Status<3 and EffectiveFrom<='{String.Format("{0:MM/dd/yyyy}", date)}' and (EffectiveTo>='{String.Format("{0:MM/dd/yyyy}", date)}' OR EffectiveTo is null)";
        }
        private static string CreditNoteCommonQuery(string username, long companyid, DateTime date, long? credit, long? comp, string currencyCode, Guid creditNoteId)
        {
            //return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY,0 as IsInterCo FROM Bean.Currency WHERE CompanyId={companyid} AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY,0 as IsInterCo FROM Common.TermsOfPayment where CompanyId={companyid} AND (Status=1 OR Id= {credit}) AND IsCustomer=1;Select distinct COA.Id,'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.InvoiceDetail Invd on Invd.COAId = COA.Id and Invd.InvoiceId = '{creditNoteId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and ACT.Name Not in ('Trade receivables','Other receivables','Trade payables','Other payables','Retained earnings','Intercompany billing','Intercompany clearing','System','Cash and bank balances')  and(COA.Status = 1 or Invd.COAId = COA.Id)order by coa.Name;SELECT 'OBCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,0 as IsInterCo FROM Bean.ChartOfAccount COA where COA.CompanyId ={companyid} and COA.Name='Opening balance';Select distinct COA.Id,'ClearingCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.InvoiceDetail Invd on Invd.COAId = COA.Id and Invd.InvoiceId = '{creditNoteId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and COA.Name  in ('Clearing - Receipts')  and(COA.Status = 1 or Invd.COAId = COA.Id)order by coa.Name;Select 'SERVICECOMPANY' as TABLENAME,comp.Id as ID,comp.Name as NAME,comp.ShortName as SHOTNAME,comp.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY,Case When comp.id=interDetail.ServiceEntityId Then 1 Else 0 End as IsInterCo from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Billing'and interDetail.Status=1 Right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId JOIN Common.CompanyUser CU on comp.ParentId=CU.CompanyId where comp.ParentId = {companyid} and (comp.Status=1 or comp.Id={comp}) and CU.Username='{username}' order by comp.ShortName;SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxId=Tax.Id Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyid} Right Join Bean.TaxCode Tax on Tax.Code=TaxMapDetail.CustTaxCode where Tax.CompanyId=0  and Tax.Status<3 and EffectiveFrom<='{date}' and (EffectiveTo>='{date}' OR EffectiveTo is null)";
            return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY,0 as IsInterCo FROM Bean.Currency WHERE CompanyId={companyid} AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY,0 as IsInterCo FROM Common.TermsOfPayment where CompanyId={companyid} AND (Status=1 OR Id= {credit}) AND IsCustomer=1;Select distinct COA.Id,'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.InvoiceDetail Invd on Invd.COAId = COA.Id and Invd.InvoiceId = '{creditNoteId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and ACT.Name Not in ('Trade receivables','Other receivables','Trade payables','Other payables','Retained earnings','Intercompany billing','Intercompany clearing','System','Cash and bank balances')  and(COA.Status = 1 or Invd.COAId = COA.Id)order by coa.Name;SELECT 'OBCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,0 as IsInterCo FROM Bean.ChartOfAccount COA where COA.CompanyId ={companyid} and COA.Name='Opening balance';Select distinct COA.Id,'ClearingCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY,Case When COAMap.CustCOAId=COA.Id Then 1 Else 0 END as IsInterCo from Bean.AccountType ACT Join Bean.ChartOfAccount COA on ACT.Id = COA.AccountTypeId Left Outer Join Bean.InvoiceDetail Invd on Invd.COAId = COA.Id and Invd.InvoiceId = '{creditNoteId}' Left Join Bean.COAMappingDetail COAMap on COAMap.CustCOAId = COA.Id and COAMap.Status = 1 where ACT.CompanyId = {companyid} and COA.Name  in ('Clearing - Receipts')  and(COA.Status = 1 or Invd.COAId = COA.Id)order by coa.Name;Select 'SERVICECOMPANY' as TABLENAME,comp.Id as ID,comp.Name as NAME,comp.ShortName as SHOTNAME,comp.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY,Case When comp.id=interDetail.ServiceEntityId Then 1 Else 0 End as IsInterCo from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Billing'and interDetail.Status=1 right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId JOIN Common.CompanyUser CU on comp.ParentId=CU.CompanyId Join common.CompanyUserDetail CUD On (Comp.Id = CUD.ServiceEntityId and CU.Id = CUD.CompanyuserId) where comp.ParentId = {companyid} and (comp.Status=1 or comp.Id={comp}) and CU.Username='{username}' order by comp.ShortName;SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxCode=Tax.Code Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyid} Right Join Bean.TaxCode Tax on Tax.Code=TaxMapDetail.CustTaxCode where Tax.CompanyId={companyid}  and Tax.Status<3 and EffectiveFrom<='{date.ToString("yyyy-MM-dd")}' and (EffectiveTo>='{date.ToString("yyyy-MM-dd")}' OR EffectiveTo is null)";
        }
        private static string DBQuery(Guid creditNoteId, long companyId, string extensionType, string docType)
        {
            if (extensionType == "Invoice")
            {
                return $"SELECT INV.DocDate as DocDate from (SELECT CNAD.DocumentId from Bean.Invoice I with (nolock) Inner join Bean.CreditNoteApplication CNA with (nolock) on CNA.InvoiceId = I.Id Inner join Bean.CreditNoteApplicationDetail CNAD with (nolock) on CNAD.CreditNoteApplicationId = CNA.Id WHERE I.DocType = '{docType}' and I.Id = '{creditNoteId}' and I.ExtensionType = '{extensionType}') AS A JOIN Bean.Invoice INV with (nolock) ON INV.Id = A.DocumentId Where INV.CompanyId = {companyId}";
            }
            else
            {
                return $"SELECT INV.DocDate as DocDate from (SELECT CNAD.DocumentId from Bean.Invoice I with (nolock) Inner join Bean.CreditNoteApplication CNA with (nolock) on CNA.InvoiceId = I.Id Inner join Bean.CreditNoteApplicationDetail CNAD with (nolock) on CNAD.CreditNoteApplicationId = CNA.Id WHERE I.DocType = '{docType}' and I.Id = '{creditNoteId}' and I.ExtensionType = '{extensionType}') AS A JOIN Bean.DebitNote INV with (nolock) ON INV.Id = A.DocumentId Where INV.CompanyId = {companyId}";
            }
        }
        #endregion

        #region Save Invoice

        public Invoice SaveInvoice(InvoiceModel TObject, string ConnectionString)
        {
            bool isNew = false;
            bool isDocNoAdd = false;
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "ObjectSave", AdditionalInfo);

            //ChkTaxCodeForALEIsNull(TObject.)

            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_Request_Message);
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new InvalidOperationException(_errors);
            }

            //to check if it is void or not
            if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

            if (TObject.EntityId == null)
            {
                throw new InvalidOperationException(InvoiceValidation.Entity_is_mandatory);
            }
            if (TObject.InvoiceDetailModels != null && TObject.IsWorkFlowInvoice == true && TObject.InvoiceDetailModels.Where(c => c.TaxId != null && c.TaxIdCode != "NA").Any(c => c.TaxRate == null))
                throw new InvalidOperationException(InvoiceValidation.Tax_rate_missing_for_required_line_items);
            if (TObject.DocDate == null)
            {
                throw new InvalidOperationException(InvoiceValidation.Invalid_Document_Date);
            }

            if (TObject.ServiceCompanyId == null)
                throw new InvalidOperationException(InvoiceValidation.Service_Company_Is_Mandatory);

            if (TObject.DueDate == null || TObject.DueDate < TObject.DocDate)
            {
                throw new InvalidOperationException(InvoiceValidation.Invalid_Due_Date);
            }
            if (TObject.IsOBInvoice != true && TObject.CreditTermsId == null)
            {
                throw new InvalidOperationException(InvoiceValidation.Terms_Payment_is_mandatory);
            }
            if (TObject.IsWorkFlowInvoice != true && TObject.IsDocNoEditable == true && _invoiceEntityService.GetRecurringDocNo_modified(TObject.CompanyId, TObject.Id, TObject.DocNo))
            {
                TObject.IsDocNoExists = true;
                throw new InvalidOperationException(InvoiceValidation.Document_number_already_exist);
            }
            if (TObject.GrandTotal < 0)
            {
                throw new InvalidOperationException(InvoiceValidation.Grand_Total_should_be_greater_than_zero);
            }
            if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null && TObject.InvoiceDetails.Any())
            {
                foreach (var invoice in TObject.InvoiceDetails)
                {
                    if (invoice.ItemCode != null && invoice.Qty == null)
                        throw new InvalidOperationException(InvoiceValidation.Please_Enter_Quantity);
                    if (invoice.ItemCode == null && invoice.Qty != null)
                        throw new InvalidOperationException(InvoiceValidation.Please_Select_Item);
                }
            }
            if (TObject.IsOBInvoice != true && (TObject.CreditTermsId == null || TObject.CreditTermsId == 0))
            {
                throw new InvalidOperationException(InvoiceValidation.Terms_of_Payment_is_mandatory);
            }
            if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null)
            {
                if (TObject.InvoiceDetails == null || TObject.InvoiceDetails.Count == 0)
                {
                    throw new InvalidOperationException(InvoiceValidation.Atleast_one_Sale_Item_is_required_in_the_Invoice);
                }
                else
                {
                    int itemCount = TObject.InvoiceDetails.Count(a => a.RecordStatus != "Deleted");
                    if (itemCount == 0)
                    {
                        throw new InvalidOperationException(InvoiceValidation.Atleast_one_Sale_Item_is_required_in_the_Invoice);
                    }
                }
            }

            if ((TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null) && TObject.IsRepeatingInvoice && (TObject.RepEveryPeriod == null || TObject.RepEveryPeriodNo == null))
            {
                throw new InvalidOperationException(InvoiceValidation.Repeating_Invoice_fields_should_be_entered);
            }

            if (TObject.ExchangeRate == 0)
                throw new InvalidOperationException(InvoiceValidation.ExchangeRate_Should_Be_Grater_Than_Zero);

            if (TObject.GSTExchangeRate == 0)
                throw new InvalidOperationException(InvoiceValidation.GSTExchangeRate_Should_Be_Grater_Than_Zero);

            //Need to verify the invoice is within Financial year
            FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(TObject.CompanyId);
            if (financialSetting != null)
            {
                if (!(financialSetting.EndOfYearLockDate == null || TObject.DocDate >= financialSetting.EndOfYearLockDate))
                {
                    throw new InvalidOperationException(InvoiceValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (TObject.IsOBInvoice != true && !ValidateFinancialOpenPeriod(TObject.DocDate, financialSetting))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(InvoiceValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (financialSetting.PeriodLockDatePassword != TObject.PeriodLockPassword)
                    {
                        throw new InvalidOperationException(InvoiceValidation.Invalid_Financial_Period_Lock_Password);
                    }
                }
            }
            Invoice _invoice = null;
            if (TObject.Nature == "Interco")
            {
                if (TObject.GrandTotal == 0)
                    throw new InvalidOperationException("You can't create an interco invoice with '0' Amount.");
                TObject.DocType = DocTypeConstants.Invoice;
                FillDocumentAndDetailType(TObject, ConnectionString);
                _invoice = _invoiceEntityService.GetAllInvoiceByIdDocType(TObject.Id);
            }
            else
            {
                try
                {
                    if (TObject.IsWorkFlowInvoice == true)
                        _invoice = _invoiceEntityService.GetInvoiceByIdAndDocumentId(TObject.DocumentId ?? Guid.Empty, TObject.CompanyId);
                    else
                        _invoice = _invoiceEntityService.GetAllInvoiceByIdDocType(TObject.Id);
                    if (_invoice != null)
                    {
                        if (_invoice.ExchangeRate != TObject.ExchangeRate)
                            _invoice.RoundingAmount = 0;
                        string timeStamp = "0x" + string.Concat(Array.ConvertAll(_invoice.Version, x => x.ToString("X2")));
                        if (!timeStamp.Equals(TObject.Version))
                            throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);
                        decimal? docTotal = _invoice.GrandTotal - TObject.GrandTotal;
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Checking_Invoice_is_null_or_not);
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.InsertInvoice_method_came);
                        InsertInvoice(TObject, _invoice);
                        _invoice.DocNo = TObject.DocNo;
                        _invoice.InvoiceNumber = _invoice.DocNo;
                        TObject.CustCreditlimit -= docTotal;
                        _invoice.DocSubType = TObject.DocSubType;
                        _invoice.InternalState = TObject.InternalState;
                        _invoice.BalanceAmount = _invoice.DocumentState == InvoiceState.NotPaid ? TObject.GrandTotal : /*TObject.BalanceAmount*/_invoice.BalanceAmount;
                        _invoice.DocumentState = TObject.GrandTotal == 0 ? InvoiceState.FullyPaid : TObject.DocumentState;
                        _invoice.ModifiedBy = TObject.ModifiedBy;
                        _invoice.ModifiedDate = DateTime.UtcNow;
                        _invoice.ObjectState = ObjectState.Modified;
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceDetails_method_came);
                        UpdateInvoiceDetails(TObject, _invoice);
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceNotes_method_came);

                        #region Commented Note Changes While InvoiceUpdate Time
                        //UpdateInvoiceNotes(TObject, _invoice);
                        #endregion

                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceGSTDetails_method_came);
                        _invoiceEntityService.Update(_invoice);
                    }
                    else
                    {
                        _invoice = new Invoice();
                        isNew = true;
                        InsertInvoice(TObject, _invoice);
                        _invoice.DocSubType = (TObject.IsWorkFlowInvoice == true || TObject.IsOBInvoice == true) ? TObject.DocSubType : "General";
                        _invoice.DocumentState = TObject.GrandTotal == 0 ? InvoiceState.FullyPaid : InvoiceState.NotPaid;
                        _invoice.Id = TObject.IsOBInvoice == true ? TObject.Id : TObject.IsWorkFlowInvoice == true ? TObject.WFDocumentId.Value : Guid.NewGuid();
                        _invoice.InternalState = InvoiceState.Posted;
                        _invoice.BalanceAmount = TObject.GrandTotal;
                        if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null)
                        {
                            if (TObject.InvoiceDetails.Count > 0 || TObject.InvoiceDetails != null)
                            {
                                UpdateInvoiceDetails(TObject, _invoice);
                            }
                        }
                        else
                        {
                            if (TObject.IsWorkFlowInvoice == true && TObject.InvoiceDetailModels != null)
                                FillWorkflowInvoiceDeatils(TObject, _invoice);
                            else if (TObject.IsWorkFlowInvoice == true)
                                UpdateInvoiceDetails(TObject, _invoice);

                        }
                        _invoice.Status = RecordStatusEnum.Active;
                        _invoice.UserCreated = TObject.UserCreated;
                        _invoice.CreatedDate = DateTime.UtcNow;
                        _invoice.ObjectState = ObjectState.Added;
                        if (TObject.IsWorkFlowInvoice == true)
                        {
                            var lstInvoiceNumber = _invoiceEntityService.GetInvoiceNumber(TObject.CompanyId, TObject.InvoiceNumber);
                            if (lstInvoiceNumber.Any())
                                throw new InvalidOperationException("Invoice Number already exists");
                            _invoice.SyncWFInvoiceId = TObject.DocumentId;
                            _invoice.SyncWFInvoiceStatus = InvoiceConstants.Completed;
                            _invoice.SyncWFInvoiceDate = DateTime.UtcNow;
                            _invoice.SyncWFInvoiceRemarks = "";
                        }
                        else
                            _invoice.InvoiceNumber = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstant.Invoice, ConnectionString) : TObject.DocNo;
                        isDocNoAdd = true;
                        _invoice.DocNo = _invoice.InvoiceNumber;
                        _invoiceEntityService.Insert(_invoice);
                    }
                }
                catch (Exception)
                {
                    if (isNew && isDocNoAdd && TObject.IsDocNoExists == false)
                    {
                        AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocType, ConnectionString);
                    }
                    throw;
                }
                try
                {
                    unitOfWork.SaveChanges();

                    #region WorkFlow_Status_Update
                    if (TObject.IsWorkFlowInvoice == true && isNew)
                    {
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.WF_invoice_status_call_executing);
                        SmartCursorSyncing(_invoice.CompanyId, CursorConstants.Workflow, DocTypeConstants.Invoice, _invoice.DocumentId, _invoice.Id, InvoiceConstants.Completed, string.Empty, ConnectionString);
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Out_from_WF_invoice_status_call_executing);
                    }
                    #endregion WorkFlow_Status_Update

                    if (TObject.IsOBInvoice != true)
                    {
                        AppaWorld.Bean.Common.SavePosting(_invoice.CompanyId, _invoice.Id, _invoice.DocType, ConnectionString);
                    }

                    #region Cust_CreditLimit_Updation
                    //decimal? custCreditLimit = _beanEntityService.GetCteditLimitsValue(_invoice.EntityId);

                    //Commented Code
                    //if (TObject.CustCreditlimit != null)
                    //{
                    //    SqlConnection con = new SqlConnection(ConnectionString);
                    //    if (con.State != ConnectionState.Open)
                    //        con.Open();
                    //    SqlCommand cmd = new SqlCommand("BC_UPDATE_ENTITY_CREDITTERMS", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@EntityId", _invoice.EntityId.ToString());
                    //    cmd.Parameters.AddWithValue("@BaseAmount", isExeedLimitEdit == true ? TObject.CustCreditlimit : _invoice.GrandTotal);
                    //    cmd.Parameters.AddWithValue("@DocType", _invoice.DocType);
                    //    cmd.Parameters.AddWithValue("@CompanyId", _invoice.CompanyId);
                    //    cmd.Parameters.AddWithValue("@isEdit", isExeedLimitEdit);
                    //    cmd.Parameters.AddWithValue("@isVoid", false);
                    //    cmd.ExecuteNonQuery();
                    //    con.Close();
                    //}
                    #endregion

                    #region DomainEvent commented Code
                    //if (eventStatus == "Updated")
                    //{
                    //    DomainEventChannel.Raise(new InvoiceUpdated(TObject));
                    //    if (eventStatuschanged != TObject.Status)
                    //    {
                    //        DomainEventChannel.Raise(new InvoiceStatusChanged(TObject));
                    //    }
                    //    else if (eventDocStatusChanged != TObject.DocumentState)
                    //    {
                    //        DomainEventChannel.Raise(new InvoiceDocStatusChanged(TObject));
                    //    }
                    //}
                    //else
                    //{
                    //    DomainEventChannel.Raise(new InvoiceCreated(TObject));

                    //}
                    #endregion DomainEvent commented Code

                    #region
                    //if (TObject.IsWorkFlowInvoice == true && isNew)
                    //{
                    //    saveScreenRecords(TObject.Id.ToString(), TObject.EntityId.ToString(), TObject.EntityId.ToString(), TObject.DocNo, TObject.UserCreated, DateTime.UtcNow, isNew, TObject.CompanyId, InvoiceConstants.Invoices);
                    //    if (TObject.DocTilesFilesVMs.Any())
                    //    {
                    //        foreach (var attachments in TObject.DocTilesFilesVMs)
                    //        {
                    //            SaveInvoiceScreenFiles(TObject.MongoId, attachments.name, mongoFile.FileSize, TObject.EntityId.ToString(), TObject.Id.ToString(), TObject.Id.ToString(), TObject.CompanyId, "Added", null, TObject.UserCreated, null);
                    //        }

                    //    }
                    //}
                    #endregion

                    #region  Peppol
                    try
                    {

                        if (TObject.IsWorkFlowInvoice != true && isNew)
                        {
                            string isPeppolEnable = null;
                            string str = "Select case when isnull(cf.IsChecked,0) = 1 then 'true' else 'false' end as PeppolEnable from Common.Feature as f join Common.CompanyFeatures  as cf on f.Id = cf.FeatureId where f.Name = 'Peppol' and f.Status = 1 and cf.CompanyId = " + TObject.CompanyId;
                            int? resultsetCount = query.Split(';').Count();
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand(str, con);
                                dr = cmd.ExecuteReader();
                                cmd.CommandTimeout = 0;
                                for (int i = 1; i <= resultsetCount; i++)
                                {
                                    if (dr.HasRows)
                                    {
                                        while (dr.Read())
                                        {
                                            isPeppolEnable = dr["PeppolEnable"].ToString();
                                        }
                                    }
                                    dr.NextResult();
                                }
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            if (isPeppolEnable == "true")
                            {
                                var entity = _beanEntityService.GetEntityById(TObject.EntityId);
                                var serviceCompany = _companyService.GetCompanyByCompanyid(TObject.ServiceCompanyId.Value);

                                if (isPeppolEnable == "true" && !string.IsNullOrEmpty(entity.PeppolDocumentId) && !string.IsNullOrEmpty(serviceCompany.ParticipantPeppolId))
                                {
                                    PeppolInvoiceModelBinding(TObject, _invoice, entity, serviceCompany);
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                    }

                    #endregion

                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_SuccessFully_Message);
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_Exception_Message);
                    //LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                    //Log.Logger.ZCritical(e.StackTrace);
                    //throw e;

                    if (TObject.IsWorkFlowInvoice == true && isNew)
                        SmartCursorSyncing(TObject.CompanyId, CursorConstants.Workflow, DocTypeConstants.Invoice, TObject.DocumentId, null, InvoiceConstants.Failed, ex.Message, ConnectionString);
                    LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                    throw ex;
                }
            }
            return _invoice;
        }
        private bool ValidateFinancialOpenPeriod(DateTime DocDate, FinancialSetting setting)
        {
            if (setting.PeriodLockDate != null && setting.PeriodEndDate != null)
                return DocDate.Date >= setting.PeriodLockDate && DocDate.Date <= setting.PeriodEndDate;
            else if (setting.PeriodLockDate != null && setting.PeriodEndDate == null)
                return DocDate.Date >= setting.PeriodLockDate;
            else if (setting.PeriodLockDate == null && setting.PeriodEndDate != null)
                return DocDate.Date <= setting.PeriodEndDate;
            else
                return true;
        }
        public Invoice SaveInvoiceDocumentVoid(DocumentVoidModel TObject, string ConnectionString)
        {
            SqlConnection con = null;
            SqlCommand cmd = null;
            string query = string.Empty;
            string DocNo = "-V";
            bool? isVoid = true;
            string DocDescription = "Void-";
            //Invoice _document = _invoiceEntityService.Query(e => e.Id == TObject.Id && e.CompanyId == TObject.CompanyId && e.DocType == DocTypeConstants.Invoice).Select().FirstOrDefault();

            //to check if it is void or not
            if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.This_transaction_has_already_void);

            Invoice _document = _invoiceEntityService.GetAllInvoiceLu(TObject.CompanyId, TObject.Id);
            if (_document != null)
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                if (_document.Nature == DocTypeConstants.Interco)
                {
                    query = $"select DocumentState from Bean.Bill where CompanyId={_document.CompanyId} and PayrollId='{_document.Id}' and Nature='Interco'";
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        string billState = Convert.ToString(cmd.ExecuteScalar());
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                        if (billState != InvoiceState.NotPaid)
                            throw new Exception("The corresponding Bill state has been changed.");
                    }
                }
            }
            try
            {
                if (TObject.IsDelete != true)
                {
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoiceDocumentVoid_SaveCall_Request_Message);
                    if (_document == null)
                        throw new Exception("Invalid Invoice");
                    if (_document.InternalState != InvoiceState.Recurring)
                        if (_document.DocumentState != InvoiceStates.NotPaid)
                            throw new Exception("State should be " + InvoiceStates.NotPaid);
                    if (_document.InvoiceDetails.Any())
                        if (_document.InvoiceDetails.Any(a => a.ClearingState == InvoiceState.Cleared))
                            throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

                    if (_document.ClearCount != null && _document.ClearCount > 0)
                        throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);


                    //Need to verify the invoice is within Financial year
                    if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
                    {
                        throw new Exception("Transaction date is in closed financial period and cannot be posted.");
                    }

                    //Verify if the invoice is out of open financial period and lock password is entered and valid
                    if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
                    {
                        if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                        {
                            throw new Exception("Transaction date is in locked accounting period and cannot be posted.");
                        }
                        else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                        {
                            throw new Exception("Invalid Financial Period Lock Password.");
                        }
                    }
                    _document.DocNo = _document.DocNo + DocNo;
                    _document.DocDescription = DocDescription + _document.DocDescription;
                    _document.DocumentState = InvoiceStates.Void;
                    if (_document.InternalState == InvoiceState.Recurring)
                        _document.NextDue = null;
                    //_document.InternalState = InvoiceState.Void;
                    _document.ModifiedDate = DateTime.UtcNow;
                    _document.ModifiedBy = TObject.ModifiedBy;
                    _document.ObjectState = ObjectState.Modified;
                    _invoiceEntityService.Update(_document);
                    try
                    {
                        unitOfWork.SaveChanges();

                        #region Interco_Billing_Void
                        if (_document.Nature.Equals("Interco"))
                        {
                            //query = $"Update Bean.Bill set DocumentState='Void',BalanceAmount={_document.BalanceAmount},ModifiedBy='System',ModifiedDate=GETUTCDATE() where PayrollId='{_document.Id}' and CompanyId={_document.CompanyId} and DocumentState='Not Paid'";
                            //using (con = new SqlConnection(ConnectionString))
                            //{
                            //    if (con.State != ConnectionState.Open)
                            //        con.Open();
                            //    cmd = new SqlCommand(query, con);
                            //    cmd.ExecuteNonQuery();
                            //    query = $"Update Bean.Journal set DocumentState='Not Paid',BalanceAmount={_document.BalanceAmount},ModifiedBy='System',ModifiedDate=GETUTCDATE() where DocumentId in (Select Id from Bean.Bill where PayrollId='{_document.Id}' and CompanyId={_document.CompanyId})";
                            //    cmd = new SqlCommand(query, con);
                            //    cmd.ExecuteNonQuery();
                            //    if (con.State == ConnectionState.Open)
                            //        con.Close();
                            //}
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand("Bean_Insert_Document_History", con);
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

                        if (_document.InternalState == InvoiceState.Recurring)
                        {
                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_document.Id, _document.CompanyId, _document.Id, _document.DocType, _document.DocSubType, _document.DocumentState, _document.DocCurrency, _document.GrandTotal, _document.BalanceAmount, _document.ExchangeRate.Value, _document.ModifiedBy != null ? _document.ModifiedBy : _document.UserCreated, _document.Remarks, _document.DocDate, 0, 0);

                                if (lstdocumet.Any())
                                    AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion Documentary History
                        }
                        if (_document.InternalState != InvoiceState.Recurring)
                        {
                            JournalSaveModel tObject = new JournalSaveModel();
                            tObject.Id = TObject.Id;
                            tObject.CompanyId = TObject.CompanyId;
                            tObject.DocNo = _document.DocNo;
                            tObject.ModifiedBy = TObject.ModifiedBy;
                            deleteJVPostInvoce(tObject);
                        }
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
                        //    cmd.Parameters.AddWithValue("@DocType", _document.DocType);
                        //    cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                        //    cmd.Parameters.AddWithValue("@isEdit", false);
                        //    cmd.Parameters.AddWithValue("@isVoid", isVoid);
                        //    cmd.ExecuteNonQuery();
                        //    con.Close();
                        //}
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                        throw ex;
                    }
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoiceDocumentVoid_SuccessFully_Message);
                }
                else
                {
                    //_document.Status = RecordStatusEnum.Delete;
                    _document.DocumentState = InvoiceState.Deleted;
                    _document.ObjectState = ObjectState.Modified;
                    _invoiceEntityService.Update(_document);
                    #region Documentary History
                    try
                    {
                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_document.Id, _document.CompanyId, _document.Id, _document.DocType, null, _document.DocumentState, _document.ExCurrency, _document.GrandTotal, _document.BalanceAmount, _document.ExchangeRate.Value, _document.ModifiedBy != null ? _document.ModifiedBy : _document.UserCreated, _document.Remarks, _document.DocDate, 0, 0);
                        if (lstdocumet.Any())
                            AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    #endregion Documentary History
                    try
                    {
                        unitOfWork.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_SaveInvoiceDocumentVoid_SaveCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message + " for " + TObject.CompanyId);
                // Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return _document;
        }


        #endregion

        #region GetInvoiceDetail
        public InvoiceDetail GetInvoiceDetail(Guid invoiceId, Guid invoiceDetalId)
        {


            InvoiceDetail detail = _invoiceDetailService.GetAllInvoiceIdAndId(invoiceId, invoiceDetalId);
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetInvoiceDetail_GetCall_Request_Message);
                if (detail == null)
                {
                    detail = new InvoiceDetail();
                    detail.Qty = 1;
                    detail.Discount = 0;
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetInvoiceDetail_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_GetInvoiceDetail_GetCall_SuccessFully_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return detail;
        }
        #endregion

        #region GetInvoiceNote
        public InvoiceNote GetInvoiceNote(Guid invoiceId, Guid invoiceNoteId)
        {

            InvoiceNote note = _invoiceNoteService.GetInvoiceNote(invoiceId, invoiceNoteId);
            if (note == null)
                note = new InvoiceNote();
            return note;
        }
        #endregion

        #region GetInvoiceGSTDetail
        //public InvoiceGSTDetail GetInvoiceGSTDetail(Guid invoiceId, Guid invoiceGSTDetailId)
        //{
        //    InvoiceGSTDetail detail = _invoiceGSTDetailService.GetInvoiceGSTDetail(invoiceId, invoiceGSTDetailId);
        //    if (detail == null)
        //        detail = new InvoiceGSTDetail();
        //    return detail;
        //}
        #endregion

        #region CommonTaxcodeLu
        public List<TaxCodeLookUp<string>> TaxCodeLU(DateTime date, long companyId, string connectionString, string docSubType)
        {
            return GetTaxcodesLu(date, companyId, connectionString, docSubType);
        }
        #endregion

        #region GetAll
        public List<Invoice> GetAll(string DocType, long CompanyId)
        {
            List<Invoice> lstinvoices = _invoiceEntityService.GetCompanyIdAndId(CompanyId, DocType);
            List<Invoice> invoices = new List<Invoice>();
            foreach (Invoice invoice in lstinvoices)
            {
                var bean = _beanEntityService.GetEntityById(invoice.EntityId);
                invoice.EntityName = bean.Name;
                invoice.BeanEntity = null;
                invoice.BaseAmount = Math.Round((decimal)(invoice.GrandTotal * invoice.ExchangeRate), 2);
                invoices.Add(invoice);
            }
            return invoices;
        }
        #endregion

        #region GetTaggedInvoicesByCustomerAndCurrency
        public List<Invoice> GetTaggedInvoicesByCustomerAndCurrency(Guid customerId, string currency, long companyId)
        {
            return _invoiceEntityService.Query(a => a.EntityId == customerId && a.DocCurrency == currency && a.CompanyId == companyId && a.AllocatedAmount > 0).Select().ToList();
        }
        #endregion

        #region CreateInvoiceDocumentVoid

        public DocumentVoidModel CreateInvoiceDocumentVoid(Guid id, long companyId)
        {
            DocumentVoidModel DVModel = new DocumentVoidModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoiceDocumentVoid_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                    throw new Exception(InvoiceValidation.The_Financial_setting_should_be_activated);

                Invoice invoice = _invoiceEntityService.GetAllInvoiceLu(companyId, id);
                if (invoice == null)
                    throw new Exception(InvoiceValidation.Invalid_Invoice);
                if (invoice != null)
                {
                    DVModel.CompanyId = companyId;
                    DVModel.Id = invoice.Id;
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoiceDocumentVoid_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateInvoiceDocumentVoid_CreateCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return DVModel;
        }
        #endregion

        #region GetProvision
        public ProvisionModel GetProvision(Guid id, long companyId, Guid invoiceId)
        {
            ProvisionModel provisionModel = new ProvisionModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetProvision_CreateCall_Request_Message);
                Invoice invoice = _invoiceEntityService.GetInvoiceByIdAndComapnyId(companyId, invoiceId);
                provisionModel.DocumentDate = DateTime.UtcNow;
                if (invoice != null)
                {
                    provisionModel.Currency = invoice.DocCurrency;
                }
                //provisionModel.DocNo = GetNewProvisionDocumentNumber(companyId);
                provisionModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetProvision_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                // LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_GetProvision_GetCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return provisionModel;
        }
        #endregion

        #region CreateCreditNoteByInvoice
        public CreditNoteModel CreateCreditNoteByInvoice(Guid invoiceId, long companyId, string connectionString)
        {
            CreditNoteModel invDTO = new CreditNoteModel();
            try
            {
                //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CreditNote);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_Request_Message);
                //to check if it is void or not
                if (_invoiceEntityService.IsVoid(companyId, invoiceId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
                AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(InvoiceValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                //Invoice lastInvoice = _invoiceEntityService.GetCompanyById(companyId);
                Invoice invoice = _invoiceEntityService.GetAllInvoiceById(invoiceId);
                invDTO.Id = Guid.NewGuid();

                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = DocTypeConstants.CreditNote;

                //bool? isEdit = false;
                //invDTO.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice, DocTypeConstants.CreditNote, _autoNo, false, ref isEdit);
                //invDTO.IsDocNoEditable = isEdit;

                invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.CreditNote);
                if (invDTO.IsDocNoEditable == true)
                {
                    invDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstant.CreditNote, connectionString);
                }


                //invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, invDTO.CompanyId, false);
                invDTO.CreditTermsId = invoice.CreditTermsId;
                invDTO.DocDate = invoice.DocDate;
                var top = _termsOfPaymentService.GetTermsOfPaymentById(invDTO.CreditTermsId);
                if (top != null)
                {
                    invDTO.CreditTermsName = top.Name;
                    invDTO.DueDate = invDTO.DocDate.AddDays(top.TOPValue.Value);
                }
                invDTO.DueDate = invoice.DocSubType == DocTypeConstants.OpeningBalance ? invoice.DueDate : invDTO.DueDate;
                invDTO.EntityId = invoice.EntityId;
                BeanEntity beanEntity = _beanEntityService.GetEntityById(invoice.EntityId);
                invDTO.EntityName = beanEntity.Name;

                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;

                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;

                invDTO.IsGstSettings = invoice.IsGstSettings;
                invDTO.GSTExCurrency = invoice.GSTExCurrency;
                invDTO.GSTExchangeRate = invoice.GSTExchangeRate;
                invDTO.GSTExDurationFrom = invoice.GSTExDurationFrom;
                invDTO.GSTExDurationTo = invoice.GSTExDurationTo;

                invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                //invDTO.SegmentCategory1 = invoice.SegmentCategory1;
                //invDTO.SegmentCategory2 = invoice.SegmentCategory2;

                invDTO.GSTTotalAmount = invoice.GSTTotalAmount;
                invDTO.GrandTotal = invoice.DocumentState == InvoiceState.NotPaid ? invoice.GrandTotal : invoice.BalanceAmount;
                invDTO.BalanceAmount = invoice.BalanceAmount;
                invDTO.ExtensionType = invoice.IsOBInvoice == true ? DocTypeConstants.OBInvoice : ExtensionType.Invoice;
                //invDTO.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
                invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;

                //invDTO.SegmentMasterid1 = invoice.SegmentMasterid1;


                //if (invoice.SegmentMasterid1 != null)
                //{
                //    var segment1 = _segmentMasterService.GetSegmentMastersById(invoice.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    invDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
                //}
                //if (invoice.SegmentMasterid2 != null)
                //{
                //    var segment2 = _segmentMasterService.GetSegmentMastersById(invoice.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    invDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
                //}

                //invDTO.SegmentMasterid2 = invoice.SegmentMasterid2;
                //invDTO.SegmentDetailid1 = invoice.SegmentDetailid1;
                //invDTO.SegmentDetailid2 = invoice.SegmentDetailid2;



                invDTO.Remarks = invoice.DocDescription;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = CreditNoteState.NotApplied;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;

                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                //invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
                //invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
                //invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
                //invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);

                List<InvoiceDetail> lstInvDetail = new List<InvoiceDetail>();
                if (invoice.InvoiceDetails.Any())
                {
                    foreach (var detail in invoice.InvoiceDetails)
                    {
                        InvoiceDetail cnDetail = new InvoiceDetail();
                        cnDetail.Id = Guid.NewGuid();
                        cnDetail.InvoiceId = invDTO.Id;
                        //cnDetail.AccountName = detail.AccountName;
                        cnDetail.AmtCurrency = detail.AmtCurrency;
                        cnDetail.BaseAmount = detail.BaseAmount;
                        //cnDetail.AllowDisAllow = detail.AllowDisAllow;
                        cnDetail.BaseTaxAmount = detail.BaseTaxAmount;
                        cnDetail.BaseTotalAmount = detail.BaseTotalAmount;
                        //cnDetail.Qty = detail.Qty;
                        //cnDetail.IsPLAccount = detail.IsPLAccount;
                        cnDetail.COAId = invoice.IsOBInvoice == true ? 0 : detail.COAId;
                        //cnDetail.Discount = 0;
                        //cnDetail.DiscountType = detail.DiscountType;
                        //if (detail.DiscountType == "$")
                        //{
                        //    cnDetail.DocAmount = (Convert.ToDecimal(detail.Qty) * detail.UnitPrice.Value) - Convert.ToDecimal(detail.Discount);
                        //    //cnDetail.UnitPrice = Convert.ToDecimal(cnDetail.DocAmount) / Convert.ToDecimal(cnDetail.Qty);
                        //}
                        //else
                        //{
                        //    //cnDetail.DocAmount = Convert.ToDecimal(detail.UnitPrice - Convert.ToDecimal(detail.UnitPrice * Convert.ToDecimal(detail.Discount / 100)));
                        //    //  cnDetail.UnitPrice = Convert.ToDecimal(detail.UnitPrice) * Convert.ToDecimal(cnDetail.Qty);
                        //    //cnDetail.UnitPrice = Convert.ToDecimal(detail.DocAmount) / Convert.ToDecimal(detail.Qty);
                        //}
                        cnDetail.DocAmount = detail.DocAmount;
                        cnDetail.DocTaxAmount = detail.DocTaxAmount;
                        cnDetail.DocTotalAmount = detail.DocTotalAmount;
                        //cnDetail.ItemCode = detail.ItemCode;
                        cnDetail.ItemDescription = detail.ItemDescription;
                        //cnDetail.ItemId = detail.ItemId;
                        cnDetail.Remarks = detail.Remarks;
                        //cnDetail.TaxCurrency = detail.TaxCurrency;
                        cnDetail.TaxId = detail.TaxId;
                        cnDetail.TaxIdCode = detail.TaxIdCode;
                        cnDetail.TaxRate = detail.TaxRate;
                        cnDetail.RecOrder = detail.RecOrder;
                        //if (cnDetail.TaxId != null)
                        //{
                        //    TaxCode tax = _taxCodeService.GetTaxCode(cnDetail.TaxId.Value);
                        //    cnDetail.TaxCode = tax.Code;
                        //    cnDetail.TaxType = tax.TaxType;
                        //    cnDetail.TaxRate = tax.TaxRate;
                        //    cnDetail.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
                        //}
                        //cnDetail.Unit = detail.Unit;

                        lstInvDetail.Add(cnDetail);
                    }
                }
                invDTO.InvoiceDetails = lstInvDetail.OrderBy(c => c.RecOrder).ToList();
                //List<InvoiceGSTDetail> lstInvGstDetail = new List<InvoiceGSTDetail>();
                //if (invoice.InvoiceGSTDetails.Any())
                //{
                //    foreach (var gstDetail in invoice.InvoiceGSTDetails)
                //    {
                //        InvoiceGSTDetail invoiceGSTDetail = new InvoiceGSTDetail();
                //        invoiceGSTDetail.Id = Guid.NewGuid();
                //        invoiceGSTDetail.InvoiceId = invDTO.Id;
                //        invoiceGSTDetail.TaxId = gstDetail.TaxId;
                //        if (invoiceGSTDetail.TaxId != null)
                //        {
                //            TaxCode tax = _taxCodeService.GetTaxCode(invoiceGSTDetail.TaxId.Value);
                //        }
                //        invoiceGSTDetail.Amount = gstDetail.Amount;
                //        invoiceGSTDetail.TaxAmount = gstDetail.TaxAmount;
                //        invoiceGSTDetail.TotalAmount = gstDetail.TotalAmount;

                //        lstInvGstDetail.Add(invoiceGSTDetail);
                //    }
                //}
                //invDTO.InvoiceGSTDetails = lstInvGstDetail;

                CreditNoteApplicationModel CNAModel = new CreditNoteApplicationModel();

                CNAModel.Id = Guid.NewGuid();
                CNAModel.InvoiceId = invDTO.Id;
                CNAModel.CompanyId = invoice.CompanyId;
                CNAModel.DocNo = invoice.DocNo;
                CNAModel.DocCurrency = invoice.DocCurrency;
                CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
                CNAModel.DocDate = invDTO.DocDate;
                CNAModel.Remarks = invoice.DocDescription;
                decimal sumLineTotal = 0;
                if (invDTO.InvoiceDetails.Any())
                {
                    sumLineTotal = invDTO.InvoiceDetails.Sum(od => od.DocTotalAmount);
                }
                CNAModel.CreditAmount = invoice.GrandTotal;
                CNAModel.CreditNoteAmount = invoice.GrandTotal;
                CNAModel.CreditNoteBalanceAmount = invoice.GrandTotal;
                CNAModel.CreditNoteApplicationDate = invoice.DocDate;
                CNAModel.EntityName = invDTO.EntityName;
                CNAModel.ExchangeRate = invDTO.ExchangeRate;
                CNAModel.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                CNAModel.NoSupportingDocument = invoice.NoSupportingDocs;
                //CNAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
                //CNAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
                CNAModel.FinancialPeriodLockStartDate = invDTO.FinancialPeriodLockStartDate;
                CNAModel.FinancialPeriodLockEndDate = invDTO.FinancialPeriodLockEndDate;
                CNAModel.CreatedDate = DateTime.UtcNow;
                CNAModel.UserCreated = invoice.UserCreated;
                CNAModel.Status = CreditNoteApplicationStatus.Posted;

                invDTO.CreditNoteApplicationModel = CNAModel;

                List<CreditNoteApplicationDetailModel> lstCNADModel = new List<CreditNoteApplicationDetailModel>();
                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();

                detailModel.Id = Guid.NewGuid();
                detailModel.CreditNoteApplicationId = CNAModel.Id;
                detailModel.BalanceAmount = invoice.BalanceAmount;
                detailModel.Nature = invoice.Nature;
                detailModel.DocCurrency = CNAModel.DocCurrency;
                detailModel.DocType = DocTypeConstants.Invoice;
                detailModel.DocAmount = invoice.GrandTotal;
                detailModel.DocDate = invoice.DocDate;
                detailModel.DocumentId = invoice.Id;
                detailModel.DocNo = invoice.DocNo;
                detailModel.DocState = invoice.DocumentState;
                detailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                detailModel.IsHyperLinkEnable = true;
                if (invDTO.ServiceCompanyId != null)
                {
                    Company company = _companyService.GetById(invDTO.ServiceCompanyId.Value);
                    if (company != null)
                    {
                        detailModel.ServiceEntityId = company.Id;
                        detailModel.ServEntityName = company.ShortName;
                    }
                }
                //detailModel.SegmentCategory1 = invoice.SegmentCategory1;
                //detailModel.SegmentCategory2 = invoice.SegmentCategory2;
                detailModel.BaseCurrencyExchangeRate = invoice.ExchangeRate.Value;
                decimal sumLineTotal1 = 0;
                //decimal diff = 0;
                if (invDTO.InvoiceDetails.Any())
                {
                    sumLineTotal1 = invDTO.InvoiceDetails.Sum(od => od.DocTotalAmount);
                }
                detailModel.CreditAmount = invoice.BalanceAmount;
                lstCNADModel.Add(detailModel);

                invDTO.CreditNoteApplicationModel.CreditNoteApplicationDetailModels = lstCNADModel;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_Exception_Message);

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return invDTO;
        }



        #endregion

        #region CreateDoubtFulDebtByInvoice
        public DoubtfulDebtModel CreateDoubtFulDebtByInvoice(Guid invoiceId, long companyId, string connectionString)
        {
            DoubtfulDebtModel invDTO = new DoubtfulDebtModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateDoubtFulDebtByInvoice_CreateCall_Request_Message);
                //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, /*DocTypeConstants.DoubtFulDebitNote*/"Debt Provision");
                //to check if it is void or not
                if (_invoiceEntityService.IsVoid(companyId, invoiceId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                //Invoice lastInvoice = _invoiceEntityService.GetAllDebtNoteByCompanyId(companyId);
                Invoice invoice = _invoiceEntityService.GetAllInvoiceById(invoiceId);
                invDTO.Id = Guid.NewGuid();
                //invDTO.IsSegmentActive1 = true;
                //invDTO.IsSegmentActive2 = true;
                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = DocTypeConstants.DoubtFulDebitNote;

                //bool? isEdit = false;
                //invDTO.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice, DocTypeConstants.DoubtFulDebitNote, _autoNo, false, ref isEdit);
                //invDTO.IsDocNoEditable = isEdit;

                invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.DoubtFulDebitNote);
                if (invDTO.IsDocNoEditable == true)
                {
                    invDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.DoubtFulDebitNote, connectionString);
                }

                //invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.DoubtFulDebitNote, invDTO.CompanyId, false);
                invDTO.DocDate = invoice.DocDate;
                invDTO.EntityId = invoice.EntityId;
                BeanEntity beanEntity = _beanEntityService.GetEntityById(invoice.EntityId);
                if (beanEntity != null)
                    invDTO.EntityName = beanEntity.Name;
                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;
                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;
                invDTO.ExtensionType = ExtensionType.Invoice;
                //invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                //invDTO.SegmentCategory1 = invoice.SegmentCategory1;
                //invDTO.SegmentCategory2 = invoice.SegmentCategory2;
                invDTO.GrandTotal = invoice.BalanceAmount;
                invDTO.BalanceAmount = invoice.BalanceAmount;
                //invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
                //invDTO.IsAllowableDisallowableActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;
                //invDTO.SegmentMasterid1 = invoice.SegmentMasterid1;

                //if (invoice.SegmentMasterid1 != null)
                //{
                //    var segment1 = _segmentMasterService.GetSegmentMastersById(invoice.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    invDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
                //}
                //if (invoice.SegmentMasterid2 != null)
                //{
                //    var segment2 = _segmentMasterService.GetSegmentMastersById(invoice.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    invDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
                //}

                //invDTO.SegmentMasterid2 = invoice.SegmentMasterid2;
                //invDTO.SegmentDetailid1 = invoice.SegmentDetailid1;
                //invDTO.SegmentDetailid2 = invoice.SegmentDetailid2;
                invDTO.Remarks = invoice.DocDescription;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = DoubtfulDebtState.NotAllocated;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;

                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                //invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
                //invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
                //invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
                //invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);

                DoubtfulDebtAllocationModel DDAModel = new DoubtfulDebtAllocationModel();
                DDAModel.Id = Guid.NewGuid();
                DDAModel.CompanyId = companyId;
                DDAModel.ExchangeRate = invDTO.ExchangeRate;
                DDAModel.EntityName = invDTO.EntityName;
                DDAModel.InvoiceId = invDTO.Id;
                DDAModel.DocNo = invoice.DocNo;
                DDAModel.DoubtfulDebitAllocationDate = invDTO.DocDate;
                DDAModel.DocCurrency = invoice.DocCurrency;
                DDAModel.DoubtfulDebtAmount = invoice.BalanceAmount;
                DDAModel.DoubtfulDebtBalanceAmount = invoice.BalanceAmount;
                DDAModel.AllocateAmount = invoice.BalanceAmount;
                DDAModel.DoubtfulDebtAllocationNumber = invoice.InvoiceNumber;
                //DDAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
                //DDAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
                DDAModel.FinancialPeriodLockStartDate = invDTO.FinancialPeriodLockStartDate;
                DDAModel.FinancialPeriodLockEndDate = invDTO.FinancialPeriodLockEndDate;
                DDAModel.EntityName = invDTO.EntityName;
                DDAModel.ExchangeRate = invDTO.ExchangeRate;
                DDAModel.DocDate = invoice.DocDate;
                DDAModel.Status = DoubtfulDebtAllocationStatus.Tagged;
                DDAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, invDTO.CompanyId);
                DDAModel.NoSupportingDocument = false;
                DDAModel.Remarks = invoice.DocDescription;
                invDTO.DoubtfulDebtAllocation = DDAModel;

                List<DoubtfulDebtAllocationDetailModel> lstDDAD = new List<DoubtfulDebtAllocationDetailModel>();

                DoubtfulDebtAllocationDetailModel dDAD = new DoubtfulDebtAllocationDetailModel();

                dDAD.Id = Guid.NewGuid();
                dDAD.DoubtfulDebitAllocationId = DDAModel.Id;
                dDAD.DocType = DocTypeConstants.Invoice;
                dDAD.DocumentId = invoiceId;
                dDAD.DocCurrency = DDAModel.DocCurrency;
                dDAD.DocAmount = invoice.GrandTotal;
                dDAD.DocDate = invoice.DocDate;
                dDAD.DocumentId = invoice.Id;
                dDAD.DocNo = invoice.DocNo;
                dDAD.DocState = invoice.DocumentState;
                if (invDTO.ServiceCompanyId != null)
                {
                    Company company = _companyService.GetById(invDTO.ServiceCompanyId.Value);
                    if (company != null)
                    {
                        dDAD.ServiceEntityId = company.Id;
                        dDAD.ServEntityName = company.ShortName;
                    }
                }
                dDAD.SystemReferenceNumber = invoice.InvoiceNumber;
                dDAD.AllocateAmount = invoice.BalanceAmount;
                dDAD.BalanceAmount = invoice.BalanceAmount;
                dDAD.Nature = invoice.Nature;
                lstDDAD.Add(dDAD);

                invDTO.DoubtfulDebtAllocation.DoubtfulDebtAllocationDetailModels = lstDDAD;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateDoubtFulDebtByInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateDoubtFulDebtByInvoice_CreateCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return invDTO;

        }
        #endregion

        #region CreateReceiptByInvoice
        public ReceiptModel CreateReceiptByInvoice(Guid invoiceId, long companyId, string connectionString)
        {
            ReceiptModel _receiptModel = new ReceiptModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_Enter_into_CreateReceiptByInvoice_action);
                //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.Receipt);
                //Receipt lastReceipt = _receiptService.GetAllReceipts(companyId);

                //to check if it is void or not
                if (_invoiceEntityService.IsVoid(companyId, invoiceId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                //Invoice lastInvoice = _invoiceEntityService.GetCompanyById(companyId);
                Invoice invoice = _invoiceEntityService.GetAllInvoiceById(invoiceId);
                _receiptModel.Id = Guid.NewGuid();

                _receiptModel.CompanyId = invoice.CompanyId;
                _receiptModel.DocSubType = DocTypeConstants.Receipt;

                _receiptModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.Receipt);
                if (_receiptModel.IsDocNoEditable == true)
                    _receiptModel.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.Receipt, connectionString);

                //bool? isEdit = false;
                //_receiptModel.DocNo = GetAutoNumberForReceipt(companyId, lastReceipt, DocTypeConstants.Receipt, _autoNo, ref isEdit);
                //_receiptModel.IsDocNoEditable = isEdit;

                //_receiptModel.DocNo = GetNewReceiptDocumentNumber(companyId);

                _receiptModel.DocDate = invoice.DocDate;
                _receiptModel.CreditTermId = invoice.CreditTermsId;
                //var top = _termsOfPaymentService.GetTermsOfPaymentById(_receiptModel.CreditTermsModels.CreditTermId);
                //if (top != null)
                //{
                //    _receiptModel.CreditTermsModels.CreditTermName = top.Name;
                //    _receiptModel.DueDate = _receiptModel.DocDate.AddDays(top.TOPValue.Value);
                //}
                _receiptModel.EntityId = invoice.EntityId;
                //BeanEntity beanEntity = _beanEntityService.GetEntityById(invoice.EntityId);
                _receiptModel.EntityName = _beanEntityService.GetEntityName(invoice.EntityId);
                _receiptModel.ISMultiCurrency = invoice.IsMultiCurrency;
                _receiptModel.DocCurrency = invoice.DocCurrency;
                _receiptModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
                //var company = _companyService.GetById(invoice.ServiceCompanyId.Value);
                //if (company != null)
                //    _receiptModel.ServiceCompanyMOdels.ServiceCompanyName = company.ShortName;
                _receiptModel.GstExchangeRate = invoice.GSTExchangeRate;
                _receiptModel.BaseCurrency = invoice.ExCurrency;
                _receiptModel.ExchangeRate = invoice.ExchangeRate;
                _receiptModel.ExDurationFrom = invoice.ExDurationFrom;
                //_receiptModel.ExDurationTo = invoice.ExDurationTo;
                //_receiptModel.IsGSTApplied = invoice.IsGSTApplied;
                //_receiptModel.ReceiptRefNo = invoice.InvoiceNumber;
                _receiptModel.IsGstSettings = invoice.IsGstSettings;
                _receiptModel.GSTTotalAmount = invoice.GSTTotalAmount;
                //_receiptModel.DueDate = null;
                //_receiptModel.GrandTotal = invoice.DocumentState == InvoiceState.NotPaid ? invoice.GrandTotal : invoice.BalanceAmount;
                //_receiptModel.BankReceiptAmmount = invoice.BalanceAmount;
                _receiptModel.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                _receiptModel.NoSupportingDocument = invoice.NoSupportingDocs;
                _receiptModel.Remarks = invoice.DocDescription;
                _receiptModel.Status = invoice.Status;
                _receiptModel.DocumentState = ReceiptState.Created;
                _receiptModel.CreatedDate = invoice.CreatedDate;
                _receiptModel.UserCreated = invoice.UserCreated;
                _receiptModel.GstReportingCurrency = invoice.GSTExCurrency;
                _receiptModel.SaveType = "InDirect";
                _receiptModel.ExtensionType = ExtensionType.Invoice;
                _receiptModel.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                _receiptModel.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                if (financSettings != null)
                {
                    _receiptModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                    _receiptModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                }
                //_receiptModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(_receiptModel.CompanyId);
                //_receiptModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(_receiptModel.CompanyId);

                //var companyName = _companyService.GetByName(invoice.ServiceCompanyId.Value);

                List<ReceiptDetailModel> lstDetail = new List<ReceiptDetailModel>();
                ReceiptDetailModel detail = new ReceiptDetailModel();
                detail.Id = Guid.NewGuid();
                detail.ReceiptId = _receiptModel.Id;
                detail.DocumentId = invoice.Id;
                detail.DocumentDate = invoice.DocDate;
                detail.DocumentType = invoice.DocType;
                detail.DocumentNo = invoice.DocNo;
                detail.DocumentState = invoice.DocumentState;
                detail.Nature = invoice.Nature;
                detail.DocumentAmmount = invoice.GrandTotal;
                detail.Currency = invoice.DocCurrency;
                detail.SystemReferenceNumber = invoice.InvoiceNumber;
                detail.AmmountDue = invoice.BalanceAmount;
                detail.ReceiptAmount = invoice.BalanceAmount;
                //if (company != null)
                //{
                //detail.ServiceCompanyName = company.ShortName;
                detail.ServiceCompanyId = invoice.ServiceCompanyId;
                //}
                detail.ServiceCompanyName = _companyService.GetByName(invoice.ServiceCompanyId.Value);
                lstDetail.Add(detail);
                _receiptModel.ReceiptDetailModels = lstDetail;//.OrderBy(x=>x.DocumentDate).ThenBy(d=>d.SystemReferenceNumber).ToList();
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return _receiptModel;
        }

        private string GetAutoNumberForReceipt(long companyId, Receipt lastInvoice, string entityType, AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewReceiptDocumentNumber(companyId);
                    isEdit = true;
                }
                else
                {
                    isEdit = false;
                    string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
                    string number = "1";
                    if (lastInvoice != null)
                    {
                        if (_autoNo.Format.Contains("{MM/YYYY}"))
                        {
                            if (lastInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
                            {
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
                    }
                }
            }
            return outPutNumber;
        }
        #endregion

        #region ModuleActivations
        public ModuleActivationModel ModuleActivations(long companyId)
        {
            ModuleActivationModel module = new ModuleActivationModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_ModuleActivations_ActivationCall_Request_Message);
                module.IsNoSupportingDocumentActive = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
                module.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
                module.IsGSTActive = _gstSettingService.IsGSTAllowed(companyId, DateTime.UtcNow.Date);
                bool? multi = _mltiCurrencySettingService.GetByCompanyId(companyId);
                module.IsMultiCurrencyActive = multi;
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                module.IsFinancialActive = financSettings != null;
                if (financSettings != null)
                    module.BaseCurrency = financSettings.BaseCurrency;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_ModuleActivations_ActivationCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_ModuleActivations_ActivationCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);

                //Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return module;
        }
        #endregion

        #region SaveInvoiceNote
        public InvoiceNote Save(InvoiceNote note, long companyId)
        {
            string _errors = CommonValidation.ValidateObject(note);

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            InvoiceNote invoiceNote = _invoiceNoteService.GetInvoiceNote(note.Id, note.InvoiceId);
            if (invoiceNote == null)
            {
                throw new Exception(InvoiceValidation.Invoice_Note_is_already_exist);
            }
            else
            {
                note.CreatedDate = DateTime.UtcNow;
                note.ObjectState = ObjectState.Added;
                _invoiceNoteService.Insert(note);
            }
            try
            {
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return note;
        }
        #endregion

        public InvoiceNote CreateInvoiceNote(Guid id, Guid invoiceId)
        {
            Invoice inv = _invoiceEntityService.GetAllInvoiceById(invoiceId);
            if (inv == null)
                throw new Exception("Invalid Invoice");
            InvoiceNote note = new InvoiceNote();
            var invoice = _invoiceNoteService.GetInvoiceNote(invoiceId, id);
            if (invoice != null)
            {
                note = invoice;
            }
            else
            {
                note.Id = Guid.NewGuid();
                note.InvoiceId = invoiceId;
                note.ExpectedPaymentDate = null;
            }

            return note;
        }
        public InvoiceNote SaveInvoiceNote(InvoiceNote note)
        {
            Invoice inv = _invoiceEntityService.GetAllInvoiceById(note.InvoiceId);
            if (inv == null)
                throw new Exception("Invalid Invoice");
            InvoiceNote invoiceNote = new InvoiceNote();
            invoiceNote = _invoiceNoteService.GetInvoiceNote(note.InvoiceId, note.Id);
            if (invoiceNote != null)
            {
                invoiceNote.InvoiceId = note.InvoiceId;
                invoiceNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
                invoiceNote.Notes = note.Notes;
                invoiceNote.ModifiedDate = DateTime.UtcNow;
                invoiceNote.ModifiedBy = note.ModifiedBy;
                invoiceNote.ObjectState = ObjectState.Modified;
                _invoiceNoteService.Update(invoiceNote);
            }
            else
            {
                invoiceNote = new InvoiceNote();
                invoiceNote.Id = note.Id;
                invoiceNote.InvoiceId = note.InvoiceId;
                invoiceNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
                invoiceNote.Notes = note.Notes;
                invoiceNote.CreatedDate = DateTime.UtcNow;
                invoiceNote.UserCreated = note.UserCreated;
                invoiceNote.ObjectState = ObjectState.Modified;
                _invoiceNoteService.Insert(invoiceNote);
            }
            try
            {
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return invoiceNote;
        }
        #endregion

        #region CreditNote

        #region CreateCreditNote
        public CreditNoteModel CreateCreditNote(long companyid, Guid id, string connectionString, string username)
        {
            CreditNoteModel invDTO = new CreditNoteModel();
            try
            {

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNote_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyid);
                if (financSettings == null)
                {
                    throw new Exception(CreditNoteValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice document = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(companyid, id);
                if (document == null)
                {
                    var creditnote = _creditNoteApplicationService.Query(c => c.Id == id).Select().FirstOrDefault();
                    if (creditnote != null)
                        document = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(companyid, creditnote.InvoiceId);
                }

                if (document == null)
                {
                    Invoice lastInvoice = _invoiceEntityService.GetCreditNoteByCompanyId(companyid);
                    invDTO.Id = Guid.NewGuid();
                    invDTO.CompanyId = companyid;
                    invDTO.DocDate = lastInvoice == null ? DateTime.Now : lastInvoice.DocDate;
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.CreditNote);
                    if (invDTO.IsDocNoEditable == true)
                    {
                        invDTO.DocNo = _autoService.GetAutonumber(companyid, DocTypeConstant.CreditNote, connectionString);
                    }

                    invDTO.DocumentState = CreditNoteState.NotApplied;
                    invDTO.DueDate = DateTime.UtcNow;
                    invDTO.BaseCurrency = financSettings.BaseCurrency;
                    invDTO.DocCurrency = invDTO.BaseCurrency;
                    invDTO.ExtensionType = ExtensionType.General;
                    invDTO.CreatedDate = DateTime.UtcNow;
                    invDTO.IsLocked = false;
                }
                else
                {
                    if (!_companyService.GetPermissionBasedOnUser(document.ServiceCompanyId, document.CompanyId, username))
                        throw new Exception(CommonConstant.Access_denied);
                    FillCreditNote(invDTO, document, username);
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.CreditNote);
                    if (document.DocumentState != InvoiceState.Void)
                    {
                        AppsWorld.InvoiceModule.Entities.Journal journal = _journalService.GetJournal(companyid, document.Id);
                        if (journal != null)
                            invDTO.JournalId = journal.Id;
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNote_CreateCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNote_CreateCall_Exception_Message);

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw;
                throw ex;
            }

            return invDTO;
        }
        #endregion

        #region GetAllCreditNoteLU
        public async Task<CreditNoteLU> NewGetAllCreditNoteLU(string userName, long companyId, Guid creditNoteId, string connectionString, DateTime? docdate = null)
        {
            CreditNoteLU creditNoteLu = new CreditNoteLU();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetAllCreditNoteLUs_LookupCall_Request_Message);
                Invoice lastCreditNote = await _invoiceEntityService.GetByCompanyIdForInvoiceAsync(companyId, "Credit Note");
                Invoice invoice = await _invoiceEntityService.GetCompanyAndIdAsync(companyId, creditNoteId);
                DateTime date = (invoice ?? lastCreditNote)?.DocDate ?? DateTime.Now;
                creditNoteLu.CompanyId = companyId;
                creditNoteLu.NatureLU = new List<string> { "Trade", "Others", "Interco" };
                date = docdate != null ? (DateTime)docdate : date;
                long? credit = invoice?.CreditTermsId ?? 0;
                long? comp = invoice?.ServiceCompanyId ?? 0;
                List<CommonLookUps<string>> lstLookUps = new List<CommonLookUps<string>>();
                LookUpCategory<string> currency = new LookUpCategory<string>();
                #region Invoice to CreditNote Direct call modification for Date
                string dbQuery = null;
                if (invoice != null && invoice.ExtensionType != null)
                {
                    dbQuery = DBQuery(invoice.Id, invoice.CompanyId, invoice.ExtensionType, invoice.DocType);
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
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
                #endregion Invoice to CreditNote Direct call modification for Date
                string currencyCode = invoice != null ? invoice.DocCurrency : string.Empty;
                query = CreditNoteCommonQuery(userName, companyId, date, credit, comp, currencyCode, creditNoteId);
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
                    creditNoteLu.CurrencyLU = currency;

                    creditNoteLu.TermsOfPaymentLU = lstLookUps.Where(c => c.TableName == "TERMSOFPAY").Select(x => new LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();
                    creditNoteLu.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.isGstActivated,
                        IsIBServiceEntity = x.IsInterCo
                    }).ToList();
                }
                if (invoice != null)
                {
                    if (invoice.IsOBInvoice == true)
                        creditNoteLu.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "OBCHARTOFACCOUNT").Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement",
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                        }).OrderBy(d => d.Name).ToList();
                    else
                        creditNoteLu.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT").Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement",
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                            IsInterCoBillingCOA = x.IsInterCo,
                        }).OrderBy(d => d.Name).ToList();
                    if (invoice.ExtensionType == DocTypeConstants.Receipt)
                    {
                        creditNoteLu.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "ClearingCHARTOFACCOUNT").Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement",
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                        }).OrderBy(d => d.Name).ToList();
                    }
                }
                else
                    creditNoteLu.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT").Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Code = x.Code,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsPLAccount = x.COACategory == "Income Statement",
                        Class = x.Class,
                        Status = x.Status,
                        IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                        IsInterCoBillingCOA = x.IsInterCo,
                    }).OrderBy(d => d.Name).ToList();

                creditNoteLu.TaxCodeLU = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.Id,
                    Code = x.TaxCode,
                    Name = x.Name,
                    TaxRate = x.TaxRate,
                    TaxType = x.TaxType,
                    Status = x.Status,
                    TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.TaxCode,
                    IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                }).OrderBy(c => c.Code).ToList();

                if (invoice != null)
                {
                    var lsttax = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Inactive).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.TaxCode,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.TaxCode,
                        IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                    }).OrderBy(c => c.Code).ToList();
                    List<long?> taxIdss = invoice.InvoiceDetails.Select(x => x.TaxId).ToList();
                    if (creditNoteLu.TaxCodeLU.Any())
                        taxIdss = taxIdss.Except(creditNoteLu.TaxCodeLU.Select(d => d.Id)).ToList();
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
                        creditNoteLu.TaxCodeLU.AddRange(lstTax);
                        var data = creditNoteLu.TaxCodeLU;
                        creditNoteLu.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                    }
                }

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetAllCreditNoteLUs_LookupCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return creditNoteLu;
        }
        public CreditNoteLU GetAllCreditNoteLU(string username, long companyId, Guid creditId)
        {
            CreditNoteLU creditLU = new CreditNoteLU();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetAllCreditNoteLUs_LookupCall_Request_Message);
                Invoice lastCreditNote = _invoiceEntityService.GetByCompanyId(companyId);
                Invoice invoice = _invoiceEntityService.GetCompanyAndId(companyId, creditId);
                DateTime date = invoice == null ? lastCreditNote == null ? DateTime.Now : lastCreditNote.DocDate : invoice.DocDate;
                //List<InvoiceDetail> lstinvoicedetails = _invoiceDetailService.GetInvoiceDetailById(creditId);
                //Guid invoiceGuid = creditId;
                creditLU.CompanyId = companyId;
                //creditLU.NatureLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId,
                //ControlCodeConstants.Control_Codes_Nature);
                creditLU.NatureLU = new List<string> { "Trade", "Others" };
                //creditLU.AllowableNonallowableLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId,
                //    ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                //creditLU.DocumentTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId,
                //    ControlCodeConstants.Control_codes_DocumentType);
                if (invoice != null)
                {
                    //string currencyCode = invoice.DocCurrency;
                    creditLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, invoice.DocCurrency,
                        ControlCodeConstants.Currency_DefaultCode);
                    //var lookUpNature = _controlCodeCategoryService.GetInactiveControlcode(companyId,
                    //      ControlCodeConstants.Control_Codes_Nature, invoice.Nature);
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

                //List<LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
                //if (invoice == null)
                //{
                //    if (segments.Count > 0)
                //        creditLU.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        creditLU.SegmentCategory2LU = segments[1];
                //}
                //else
                //{
                //    if (invoice.SegmentMasterid1 != null)
                //        creditLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyId, invoice.SegmentMasterid1);
                //    else
                //        if (segments.Count > 0)
                //        creditLU.SegmentCategory1LU = segments[0];
                //    if (invoice.SegmentMasterid2 != null)
                //        creditLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyId, invoice.SegmentMasterid2);
                //    else
                //        if (segments.Count > 1)
                //        creditLU.SegmentCategory2LU = segments[1];

                //}

                long? credit = invoice == null ? 0 : invoice.CreditTermsId == null ? 0 : invoice.CreditTermsId;
                creditLU.TermsOfPaymentLU =
                    _termsOfPaymentService.Queryable().AsEnumerable().Where(a => (a.Status == RecordStatusEnum.Active || a.Id == credit) && a.CompanyId == companyId && a.IsCustomer == true)
                        .Select(x => new LookUp<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            TOPValue = x.TOPValue,
                            RecOrder = x.RecOrder
                        }).OrderBy(a => a.TOPValue).ToList();
                //long comp = invoice == null ? 0 : invoice.CompanyId;
                long? comp = invoice == null ? 0 : invoice.ServiceCompanyId == null ? 0 : invoice.ServiceCompanyId;
                creditLU.SubsideryCompanyLU = _companyService.GetSubCompany(username, companyId, comp);
                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                //string coaName = COANameConstants.Revenue;
                List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                List<AccountType> accType = _accountTypeService.GetAllAccountTypeNameByCompanyId(companyId, coaName);
                creditLU.ChartOfAccountLU = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                })).OrderBy(r => r.Name).ToList();
                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    creditLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (invoice != null && invoice.InvoiceDetails.Count > 0)
                {
                    List<long> CoaIds = invoice.InvoiceDetails.Select(c => c.COAId).ToList();
                    if (creditLU.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(creditLU.ChartOfAccountLU.Select(x => x.Id)).ToList();

                    List<long?> taxIds = invoice.InvoiceDetails.Select(x => x.TaxId).ToList();
                    if (creditLU.TaxCodeLU.Any())
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
                        //    Class = x.Class;
                        //}).ToList();
                        //Invoice creditnote = _invoiceEntityService.GetInvoicebyExtenctionType(creditId);
                        //if (creditnote == null)
                        //{
                        lstEditCoa = accType.SelectMany(d => d.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        })).OrderBy(c => c.Name).ToList();
                        creditLU.ChartOfAccountLU.AddRange(lstEditCoa);
                        //}
                        //else
                        //{
                        //    foreach (var credits in creditnote.InvoiceDetails)
                        //    {
                        if (invoice.ExtensionType == "Receipt")
                        {
                            ChartOfAccount chart = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.Clearing_Receipts, companyId);
                            List<COALookup<string>> lstCOALookup = new List<COALookup<string>>();
                            creditLU.ChartOfAccountLU = new List<COALookup<string>>();
                            COALookup<string> ss = new COALookup<string>()
                            {
                                Name = chart.Name,
                                Id = chart.Id,
                                RecOrder = chart.RecOrder,
                                IsAllowDisAllow = chart.DisAllowable == true ? true : false,
                                IsPLAccount = chart.Category == "Income Statement" ? true : false,
                                Class = chart.Class,
                                Status = chart.Status,
                                IsTaxCodeNotEditable = (chart.Class == "Assets" || chart.Class == "Liabilities" || chart.Class == "Equity") ? true : false,
                            };
                            lstCOALookup.Add(ss);
                            creditLU.ChartOfAccountLU = lstCOALookup;
                        }

                        //  }
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
                        creditLU.TaxCodeLU.AddRange(lstEditTax);
                        creditLU.TaxCodeLU = creditLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }

                    if (invoice.IsOBInvoice == true && invoice.DocType == DocTypeConstants.CreditNote && invoice.DocSubType == DocTypeConstants.OpeningBalance)
                    {
                        ChartOfAccount chartOfAccount = _chartOfAccountService.GetChartOfAccountByName("Opening balance", companyId);
                        if (chartOfAccount != null)
                        {
                            List<COALookup<string>> lstOBCoa = new List<COALookup<string>>() { new COALookup<string>() { Name=chartOfAccount.Name,Code=chartOfAccount.Code,Id=chartOfAccount.Id,RecOrder=chartOfAccount.RecOrder,IsAllowDisAllow=chartOfAccount.DisAllowable==true?true:false,IsPLAccount=chartOfAccount.Category=="Income Statement"?true:false,Class=chartOfAccount.Class,Status=chartOfAccount.Status
                                } }.ToList();
                            creditLU.ChartOfAccountLU.AddRange(lstOBCoa);
                        }
                    }

                }
                //if (lstinvoicedetails.Count > 0)
                //{
                //    creditLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyId);
                //    creditLU.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, true);
                //}
                //else
                //{
                //    creditLU.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, true);
                //    creditLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, false, companyId);
                //}
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetAllCreditNoteLUs_LookupCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_GetAllCreditNoteLUs_LookupCall_Exception_Message);

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                // Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return creditLU;
        }
        #endregion

        public async Task<IQueryable<CreditNoteModelK>> GetAllCreditNoteK(string userName, long companyId)
        {
            return await _invoiceEntityService.GetAllCreditNoteK(userName, companyId);
        }

        private RecordStatusEnum eventStatusChangedCredit;

        #region SaveCreditNote

        public Invoice SaveCreditNote(CreditNoteModel TObject, string ConnectionString)
        {
            bool isNewAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "ObjectSave", AdditionalInfo);
                string eventDocStatusChanged = "";
                string _errors = CommonValidation.ValidateObject(TObject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new Exception(_errors);
                }
                //to check if it is void or not
                if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

                if (TObject.EntityId == null)
                {
                    throw new Exception(CreditNoteValidation.Entity_is_mandatory);
                }

                if (TObject.DocDate == null)
                {
                    throw new Exception(CreditNoteValidation.Invalid_Document_Date);
                }

                if (TObject.DocDate == null || TObject.DocDate < TObject.DocDate)
                {
                    throw new Exception(CreditNoteValidation.Invalid_Due_Date);
                }
                if (TObject.SaveType == "InDirect" && TObject.BalanceAmount > 0 && TObject.CreditNoteApplicationModel.CreditAmount > 0 && ((TObject.BalanceAmount -= TObject.CreditNoteApplicationModel.CreditAmount) < 0))
                {
                    if (TObject.ExtensionType == DocTypeConstants.Invoice || TObject.ExtensionType == DocTypeConstants.OBInvoice)
                        throw new Exception("Credit note amount shouldn't be greater than outstanding balance of Invoice ");
                    if (TObject.ExtensionType == ExtensionType.DebitNote)
                        throw new Exception("Credit note amount shouldn't be greater than outstanding balance of Debit Note ");

                }
                #region 2_Tab_Validation
                if (TObject.CreditNoteApplicationModel != null)
                {
                    List<string> lstInvoiceData = _invoiceEntityService.GetInvoiceStatusByIds(TObject.CreditNoteApplicationModel.CreditNoteApplicationDetailModels.Where(s => s.DocType == DocTypeConstant.Invoice).Select(d => d.DocumentId).ToList());
                    if (lstInvoiceData.Any())
                        if (TObject.CreditNoteApplicationModel.CreditNoteApplicationDetailModels.Where(d => !lstInvoiceData.Contains(d.DocState)).Any())
                            throw new Exception(CreditNoteValidation.CN_application_status_Change);
                    List<string> lstDNData = _debitNoteService.GetDNStatusByIds(TObject.CreditNoteApplicationModel.CreditNoteApplicationDetailModels.Where(s => s.DocType == DocTypeConstants.DebitNote).Select(d => d.DocumentId).ToList());
                    if (lstDNData.Any())
                        if (TObject.CreditNoteApplicationModel.CreditNoteApplicationDetailModels.Where(d => !lstDNData.Contains(d.DocState)).Any())
                            throw new Exception(CreditNoteValidation.CN_application_status_Change);
                }
                #endregion 2_Tab_Validation
                if (TObject.SaveType == "InDirect")
                {
                    TObject.BalanceAmount = TObject.GrandTotal;
                    if (TObject.GrandTotal <= 0)
                        throw new Exception(CreditNoteValidation.Grand_Total_Should_Be_Grater_Than_Zero);
                    if (TObject.GrandTotal < TObject.BalanceAmount)
                        throw new Exception(CreditNoteValidation.Grand_Total_Should_Be_Greater_Than_Or_Equal_To_Balance_Amount);
                    if (TObject.CreditNoteApplicationModel.CreditAmount > TObject.CreditNoteApplicationModel.CreditNoteBalanceAmount)
                        throw new Exception(CreditNoteValidation.Credit_Note_Amount_should_be_less_than_or_equal_to_Remaining_Amount);
                }

                if (TObject.CreditTermsId == null && TObject.CreditTermsId == 0)
                {
                    throw new Exception(CreditNoteValidation.Terms_of_Payment_is_mandatory);
                }
                if (TObject.IsDocNoEditable == true)
                    if (IsDocumentNumberExists(DocTypeConstants.CreditNote, TObject.DocNo, TObject.Id, TObject.CompanyId))
                    {
                        throw new Exception(CreditNoteValidation.Document_number_already_exist);
                    }
                if (TObject.ServiceCompanyId == null)
                    throw new Exception(CreditNoteValidation.Service_Company_Is_Mandatory);

                if (TObject.GrandTotal <= 0)
                {
                    throw new Exception(CreditNoteValidation.Grand_Total_should_be_greater_than_zero);
                }
                if (TObject.InvoiceDetails == null || TObject.InvoiceDetails.Count == 0)
                {
                    throw new Exception(CreditNoteValidation.Atleast_one_Sale_Item_is_required);
                }
                else
                {
                    int itemCount = TObject.InvoiceDetails.Where(a => a.RecordStatus != "Deleted").Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(CreditNoteValidation.Atleast_one_Sale_Item_is_required);
                    }
                }

                if (TObject.ExchangeRate == 0)
                    throw new Exception(CreditNoteValidation.ExchangeRate_Should_Be_Grater_Than_Zero);

                if (TObject.GSTExchangeRate == 0)
                    throw new Exception(CreditNoteValidation.GSTExchangeRate_Should_Be_Grater_Than_Zero);

                //Verify if the Items are duplicated
                //var qryDuplicates = TObject.InvoiceDetails.Where(a => a.RecordStatus != "Deleted").GroupBy(x => x.ItemId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                //if (qryDuplicates.Count > 0)
                //    throw new Exception(CreditNoteValidation.Items_are_duplicated_in_the_Details);

                //Verify if the TaxId is not null
                //if (TObject.IsGstSettings && !TObject.IsGSTDeregistered)
                //{
                //    var qryNullTaxes = TObject.InvoiceDetails.Where(a => a.TaxId == null).ToList();
                //    if (qryNullTaxes.Count > 0)
                //        throw new Exception(CreditNoteValidation.Tax_Codes_are_not_selected_in_detail);
                //}
                //Need to verify the invoice is within Financial year
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
                {
                    throw new Exception(CreditNoteValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new Exception(CreditNoteValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new Exception(CreditNoteValidation.Invalid_Financial_Period_Lock_Password);
                    }
                }
                Invoice _document = null;
                if (TObject.Nature == DocTypeConstants.Interco)
                {
                    if (TObject.GrandTotal == 0)
                        throw new Exception("You can't create an interco Credit Note with '0' Amount.");
                    if (TObject.SaveType != "InDirect")
                    {
                        TObject.DocType = DocTypeConstants.CreditNote;
                        FillDocumentAndDetailTypeForCN(TObject, ConnectionString);
                    }
                    else
                    {
                        TObject.DocType = DocTypeConstants.CreditNote;
                        FillDocumentAndDetailTypeForCNIndirect(TObject, TObject.CreditNoteApplicationModel, ConnectionString);
                    }

                    _document = _invoiceEntityService.GetIntercoCreditNote(TObject.CompanyId, TObject.Id, DocTypeConstants.Interco);
                }
                else
                {
                    _document = _invoiceEntityService.GetCreditNoteById(TObject.Id);
                    if (_document != null)
                    {
                        if (_document.ExchangeRate != TObject.ExchangeRate)
                            _document.RoundingAmount = 0;
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNote_SaveCall_UpdateRequest_Message);

                        //2 Tabs manupulation with delete/add operation
                        string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                        if (!timeStamp.Equals(TObject.Version))
                            throw new Exception(CommonConstant.Document_has_been_modified_outside);

                        decimal? docTotal = _document.GrandTotal - TObject.GrandTotal;
                        eventStatusChangedCredit = _document.Status;
                        eventDocStatusChanged = _document.DocumentState;
                        InsertCreditNote(TObject, _document, false);
                        TObject.CustCreditlimit += docTotal;
                        _document.DocNo = TObject.DocNo;
                        _document.InvoiceNumber = TObject.DocNo;
                        _document.ModifiedBy = TObject.ModifiedBy;
                        _document.ModifiedDate = DateTime.UtcNow;
                        _document.ObjectState = ObjectState.Modified;
                        UpdateCreditNoteDetails(TObject, _document);
                        _document.BaseGrandTotal = Math.Round(_document.InvoiceDetails.Where(c => c.ObjectState != ObjectState.Deleted).Sum(a => (decimal)a.BaseTotalAmount), 2, MidpointRounding.AwayFromZero);
                        _document.BaseBalanceAmount = _document.BaseGrandTotal;
                        _invoiceEntityService.Update(_document);
                    }
                    else
                    {
                        isNewAdd = true;
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNote_SaveCall_NewRequest_Message);
                        _document = new Invoice();
                        InsertCreditNote(TObject, _document, true);
                        _document.Id = TObject.Id;
                        _document.DocumentState = String.IsNullOrEmpty(TObject.DocumentState) ? CreditNoteState.NotApplied : TObject.DocumentState;
                        int? recorder = 0;
                        if (TObject.InvoiceDetails.Count > 0 || TObject.InvoiceDetails != null)
                        {
                            _document.InvoiceDetails = TObject.InvoiceDetails;
                            foreach (InvoiceDetail detail in _document.InvoiceDetails)
                            {
                                detail.RecOrder = recorder + 1;
                                recorder = detail.RecOrder;
                                detail.Id = Guid.NewGuid();
                                detail.InvoiceId = _document.Id;
                                detail.ItemDescription = detail.ItemDescription;
                                detail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
                                detail.BaseTaxAmount = TObject.ExchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocTaxAmount : detail.DocTaxAmount;
                                detail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);

                                detail.ObjectState = ObjectState.Added;
                            }
                        }
                        _document.BaseGrandTotal = Math.Round(_document.InvoiceDetails.Sum(a => (decimal)a.BaseTotalAmount), 2, MidpointRounding.AwayFromZero);
                        _document.BaseBalanceAmount = _document.BaseGrandTotal;
                        _document.Status = RecordStatusEnum.Active;
                        _document.UserCreated = TObject.UserCreated;
                        _document.CreatedDate = DateTime.UtcNow;
                        _document.ObjectState = ObjectState.Added;
                        _document.InvoiceNumber = TObject.IsDocNoEditable != true ? /*GenerateAutoNumberForType(company.Id, DocTypeConstants.CreditNote, company.ShortName)*/_autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.CreditNote, ConnectionString) : TObject.DocNo;
                        _document.DocNo = _document.InvoiceNumber;
                        isDocAdd = true;
                        _invoiceEntityService.Insert(_document);
                    }
                    try
                    {
                        unitOfWork.SaveChanges();
                        AppaWorld.Bean.Common.SavePosting(_document.CompanyId, _document.Id, _document.DocType, ConnectionString);
                        //unitOfWork.SaveChanges(); //commented for CN

                        if (TObject.SaveType == "InDirect")
                        {

                            TObject.CreditNoteApplicationModel.PeriodLockPassword = TObject.PeriodLockPassword;
                            SaveCreditNoteApplication(TObject.CreditNoteApplicationModel, ConnectionString);
                        }
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
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, e, e.Message);
                        throw e;
                    }
                }
                return _document;
            }
            catch (Exception ex)
            {
                if (isNewAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, DocTypeConstants.CreditNote, ConnectionString);
                }
                throw ex;
            }
        }
        #endregion

        #region SaveCreditNoteApplication
        public CreditNoteApplication SaveCreditNoteApplication(CreditNoteApplicationModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "ObjectSave", AdditionalInfo);
            Invoice creditNote = _invoiceEntityService.Query(e => e.Id == TObject.InvoiceId).Select().FirstOrDefault();
            ValidateCreditNoteApplication(creditNote, TObject);
            if (TObject.CreditAmount > TObject.CreditNoteBalanceAmount)
                throw new Exception(CreditNoteValidation.Credit_Amount_should_be_less_than_or_equal_to_Remaining_Amount);
            CreditNoteApplication application = _creditNoteApplicationService.GetCreditNoteByIds(TObject.Id);
            bool isNew = false;
            if (application == null)
            {
                application = new CreditNoteApplication();
                isNew = true;
            }
            else
                creditNote.BalanceAmount += application.CreditAmount;
            application.CreditNoteApplicationDate = TObject.CreditNoteApplicationDate;
            application.ExchangeRate = creditNote.ExchangeRate;

            if (TObject.IsNoSupportingDocument != null)
                application.IsNoSupportingDocument = TObject.IsNoSupportingDocument.Value;
            if (application.IsNoSupportingDocument == true)
                application.IsNoSupportingDocumentActivated = TObject.NoSupportingDocument == true ? true : false;
            application.CreditAmount = TObject.CreditAmount;
            application.Remarks = TObject.Remarks;
            application.Status = CreditNoteApplicationStatus.Posted;
            application.IsRevExcess = TObject.IsRevExcess;
            List<DocumentHistoryModel> lstDocuments = new List<DocumentHistoryModel>();
            Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
            decimal roundingAmount = 0;
            if (isNew)
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteApplication_SaveCall_NewRequest_Message);
                application.Id = TObject.Id;
                application.InvoiceId = TObject.InvoiceId;
                application.CompanyId = TObject.CompanyId;
                application.ExchangeRate = creditNote.ExchangeRate;
                if (TObject.IsRevExcess != true)
                    UpdateCreditNoteApplicationDetails(TObject, application, ConnectionString, TObject.Id, lstDocuments, lstOfRoundingAmount);
                else
                    UpdateCreditNoteApplicationRevExcessDetails(TObject, application);
                application.CreditNoteApplicationNumber = GetNextApplicationNumber(TObject.InvoiceId);
                application.UserCreated = TObject.UserCreated;
                application.CreatedDate = DateTime.UtcNow;
                application.CreditNoteApplicationDetails = application.CreditNoteApplicationDetails.Where(c => c.CreditAmount != 0).ToList();
                application.ObjectState = ObjectState.Added;
                _creditNoteApplicationService.Insert(application);
            }
            else
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteApplication_SaveCall_UpdateRequest_Message);

                //Data concurancy verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(application.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                if (TObject.IsRevExcess != true)
                    UpdateCreditNoteApplicationDetails(TObject, application, ConnectionString, TObject.Id, lstDocuments, lstOfRoundingAmount);
                else
                    UpdateCreditNoteApplicationRevExcessDetails(TObject, application);
                application.ModifiedBy = TObject.ModifiedBy;
                application.ModifiedDate = DateTime.UtcNow;
                application.ObjectState = ObjectState.Modified;
                _creditNoteApplicationService.Update(application);
                isNew = false;
            }
            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(creditNote.Id, creditNote.CompanyId, TObject.Id, creditNote.DocType, DocTypeConstants.Application, "Posted", creditNote.DocCurrency, application.CreditAmount, application.CreditAmount, creditNote.ExchangeRate.Value, application.ModifiedBy != null ? application.ModifiedBy : application.UserCreated, creditNote.Remarks, TObject.DocDate, application.CreditAmount, 0);
                if (lstdocumet.Any())
                    lstDocuments.AddRange(lstdocumet);

            }
            catch (Exception ex)
            {

            }
            #endregion Documentary History

            creditNote.BalanceAmount -= TObject.CreditAmount;
            if (creditNote.BalanceAmount == 0)
            {
                creditNote.DocumentState = CreditNoteState.FullyApplied;
                if (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)
                    roundingAmount = ((creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0) ? (decimal)creditNote.RoundingAmount : 0);
                else
                    roundingAmount = Math.Round(application.CreditAmount * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)creditNote.BaseBalanceAmount;

                creditNote.BaseBalanceAmount = 0;
                if (roundingAmount != 0)
                    lstOfRoundingAmount.Add(creditNote.Id, roundingAmount);
                creditNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)) ? creditNote.RoundingAmount - roundingAmount : 0;
                application.RoundingAmount = roundingAmount;

            }
            else
            {
                creditNote.DocumentState = CreditNoteState.PartialApplied;
                creditNote.BaseBalanceAmount -= Math.Round(application.CreditAmount * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
            }
            creditNote.ModifiedBy = InvoiceConstants.System;
            creditNote.ModifiedDate = DateTime.UtcNow;
            creditNote.ObjectState = ObjectState.Modified;
            _invoiceEntityService.Update(creditNote);


            try
            {
                List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, creditNote.CompanyId, creditNote.Id, creditNote.DocType, creditNote.DocSubType, creditNote.DocumentState, creditNote.DocCurrency, creditNote.GrandTotal, creditNote.BalanceAmount, creditNote.ExchangeRate.Value, InvoiceConstants.System, application.Remarks, application.CreditNoteApplicationDate, application.CreditAmount < 0 ? application.CreditAmount : -application.CreditAmount, roundingAmount);
                if (lstdocumet1.Any())
                    lstDocuments.AddRange(lstdocumet1);
                if (lstDocuments.Any())
                    AppaWorld.Bean.Common.SaveDocumentHistory(lstDocuments, ConnectionString);
            }
            catch (Exception ex)
            {
            }



            var updateState = _journalService.GetJournal(TObject.CompanyId, TObject.InvoiceId);
            if (updateState != null)
            {
                updateState.BalanceAmount = creditNote.BalanceAmount;
                updateState.DocumentState = creditNote.DocumentState;
                updateState.ModifiedBy = InvoiceConstants.System;
                updateState.ModifiedDate = DateTime.UtcNow;
                updateState.ObjectState = ObjectState.Modified;
                _journalService.Update(updateState);
            }
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteApplication_SaveCall_SuccessFully_Message);

            if (creditNote != null)
            {
                if (creditNote.DocSubType == DocTypeConstants.OpeningBalance && creditNote.DocType == DocTypeConstants.CreditNote && creditNote.DocSubType == DocTypeConstants.OpeningBalance)
                {
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Enter_Into_Update_OB_LineItem);
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", creditNote.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", creditNote.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", creditNote.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", creditNote.BalanceAmount == creditNote.GrandTotal ? true : false);
                    int res = oBcmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            try
            {
                unitOfWork.SaveChanges();
                JVModel jvm = new JVModel();
                FillCreditNoteJournal(jvm, application, isNew, TObject.IsGstSettings, lstOfRoundingAmount);
                SaveInvoice1(jvm);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return application;
        }
        #endregion

        #region CreateCreditNoteApplication
        public CreditNoteApplicationModel CreateCreditNoteApplication(Guid creditNoteId, Guid cnApplicationId, long companyId, bool isView, DateTime applicationDate)
        {
            CreditNoteApplicationModel CNAModel = new CreditNoteApplicationModel();

            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplication_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CreditNoteValidation.The_Financial_setting_should_be_activated);
                }
                CNAModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                CNAModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice creditNote = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(companyId, creditNoteId);
                {
                    if (creditNote == null)
                        throw new Exception(CreditNoteValidation.Invalid_CreditNote);
                }
                List<Company> lstComapny = new List<Company>();
                List<long> lstServiceEntityIds = new List<long>();
                CreditNoteApplication CNApplication = _creditNoteApplicationService.GetAllCreditNote(creditNoteId, cnApplicationId, companyId);
                if (CNApplication != null)
                {
                    FillCreditNoteApplicationModel(CNAModel, CNApplication);
                    CNAModel.EntityName = _beanEntityService.GetEntityName(creditNote.EntityId);
                    AppsWorld.InvoiceModule.Entities.Journal journal = _journalService.GetJournals(cnApplicationId, companyId);
                    if (journal != null)
                    {
                        CNAModel.JournalId = journal.Id;
                        CNAModel.DocSubType = journal.DocSubType;
                    }
                    CNAModel.DocDate = creditNote.DocDate;
                    CNAModel.Remarks = creditNote.DocDescription;
                    List<CreditNoteApplicationDetailModel> lstDetailModel = new List<CreditNoteApplicationDetailModel>();
                    List<CreditNoteApplicationDetail> lstCNAD = _creditNoteApplicationDetailService.GetCreditNoteDetail(cnApplicationId);
                    List<Guid> invoiceIds = lstCNAD.Where(c => c.DocumentType == DocTypeConstants.Invoice).Select(d => d.DocumentId).ToList();
                    List<Guid> debitNoteIds = lstCNAD.Where(c => c.DocumentType == DocTypeConstants.DebitNote).Select(d => d.DocumentId).ToList();
                    List<Invoice> lstInvoices = _invoiceEntityService.GetAllDDByInvoiceId(invoiceIds);
                    List<DebitNote> lstDNs = _debitNoteService.GetDDByDebitNoteId(debitNoteIds);
                    if (lstInvoices.Any())
                        lstServiceEntityIds.AddRange(lstInvoices.Select(c => c.ServiceCompanyId.Value).ToList());
                    if (lstDNs.Any())
                        lstServiceEntityIds.AddRange(lstDNs.Select(c => c.ServiceCompanyId.Value).ToList());
                    if (lstServiceEntityIds.Any())
                        lstComapny = _companyService.GetAllCompanies(lstServiceEntityIds).ToList();
                    if (CNApplication.IsRevExcess != true)
                    {
                        foreach (CreditNoteApplicationDetail detail in lstCNAD)
                        {
                            CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                            detailModel.Id = detail.Id;
                            detailModel.CreditNoteApplicationId = detail.CreditNoteApplicationId;
                            detailModel.DocCurrency = CNAModel.DocCurrency;
                            detailModel.CreditAmount = detail.CreditAmount;
                            if (detail.DocumentType == DocTypeConstants.Invoice)
                            {
                                //var invoice = _invoiceEntityService.GetCreditNoteByDocumentId(detail.DocumentId);
                                Invoice invoice = lstInvoices.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                                detailModel.DocAmount = invoice.GrandTotal;
                                detailModel.DocDate = invoice.DocDate;
                                detailModel.DocumentId = invoice.Id;
                                detailModel.DocNo = invoice.DocNo;
                                detailModel.ServiceEntityId = lstComapny.Where(c => c.Id == invoice.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                                detailModel.ServEntityName = lstComapny.Where(c => c.Id == invoice.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault();
                                detailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                                if (CNApplication.Status == CreditNoteApplicationStatus.Void)
                                    detailModel.BalanceAmount = invoice.BalanceAmount;
                                else
                                    detailModel.BalanceAmount = invoice.BalanceAmount + detail.CreditAmount;
                                detailModel.DocType = invoice.DocType;
                                detailModel.Nature = invoice.Nature;
                                detailModel.DocState = invoice.DocumentState;
                                //detailModel.SegmentCategory1 = invoice.SegmentCategory1;
                                //detailModel.SegmentCategory2 = invoice.SegmentCategory2;
                                detailModel.BaseCurrencyExchangeRate = invoice.ExchangeRate.Value;
                            }
                            else if (detail.DocumentType == DocTypeConstants.DebitNote)
                            {
                                //var debitNote = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                                DebitNote debitNote = lstDNs.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                                detailModel.DocAmount = debitNote.GrandTotal;
                                detailModel.DocDate = debitNote.DocDate;
                                detailModel.DocumentId = debitNote.Id;
                                detailModel.DocNo = debitNote.DocNo;
                                detailModel.ServiceEntityId = lstComapny.Where(c => c.Id == debitNote.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                                detailModel.ServEntityName = lstComapny.Where(c => c.Id == debitNote.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault();
                                detailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                                if (CNApplication.Status == CreditNoteApplicationStatus.Void)
                                    detailModel.BalanceAmount = debitNote.BalanceAmount;
                                else
                                    detailModel.BalanceAmount = debitNote.BalanceAmount + detail.CreditAmount;
                                detailModel.DocType = debitNote.DocSubType;
                                detailModel.Nature = debitNote.Nature;
                                detailModel.DocState = debitNote.DocumentState;
                                //detailModel.SegmentCategory1 = debitNote.SegmentCategory1;
                                //detailModel.SegmentCategory2 = debitNote.SegmentCategory2;
                                detailModel.BaseCurrencyExchangeRate = debitNote.ExchangeRate.Value;
                            }
                            lstDetailModel.Add(detailModel);
                        }
                    }
                    if (isView != true)
                    {
                        List<Invoice> lstInvoice = _invoiceEntityService.GetAllCreditNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                        List<DebitNote> lstDebitNote = _debitNoteService.GetAllDebitNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                        lstComapny = new List<Company>();
                        lstServiceEntityIds = new List<long>();
                        if (lstInvoice.Any())
                            lstServiceEntityIds.AddRange(lstInvoice.Select(c => c.ServiceCompanyId.Value).ToList());
                        if (lstDebitNote.Any())
                            lstServiceEntityIds.AddRange(lstDebitNote.Select(c => c.ServiceCompanyId.Value).ToList());
                        if (lstServiceEntityIds.Any())
                            lstComapny = _companyService.GetAllCompanies(lstServiceEntityIds).ToList();
                        foreach (Invoice detail in lstInvoice)
                        {
                            var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                            if (d == null)
                            {
                                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                detailModel.DocNo = detail.DocNo;
                                detailModel.DocType = detail.DocType;
                                detailModel.DocumentId = detail.Id;
                                detailModel.DocDate = detail.DocDate;
                                detailModel.DocAmount = detail.GrandTotal;
                                detailModel.DocCurrency = detail.DocCurrency;
                                detailModel.BalanceAmount = detail.BalanceAmount;
                                detailModel.Nature = detail.Nature;
                                detailModel.ServiceEntityId = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                                detailModel.ServEntityName = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault();
                                detailModel.DocState = detail.DocumentState;
                                detailModel.SystemReferenceNumber = detail.InvoiceNumber;
                                //detailModel.SegmentCategory1 = detail.SegmentCategory1;
                                //detailModel.SegmentCategory2 = detail.SegmentCategory2;
                                detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                                lstDetailModel.Add(detailModel);
                            }
                        }


                        if (lstDNs.Any())
                            lstServiceEntityIds.AddRange(lstDNs.Select(c => c.ServiceCompanyId.Value).ToList());
                        foreach (DebitNote detail in lstDebitNote)
                        {
                            var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                            if (d == null)
                            {
                                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                detailModel.DocNo = detail.DocNo;
                                detailModel.DocType = DocTypeConstants.DebitNote;
                                detailModel.DocumentId = detail.Id;
                                detailModel.DocDate = detail.DocDate;
                                detailModel.DocAmount = detail.GrandTotal;
                                detailModel.DocCurrency = detail.DocCurrency;
                                detailModel.BalanceAmount = detail.BalanceAmount;
                                detailModel.Nature = detail.Nature;
                                detailModel.ServiceEntityId = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                                detailModel.ServEntityName = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault();
                                detailModel.SystemReferenceNumber = detail.DebitNoteNumber;
                                detailModel.DocState = detail.DocumentState;
                                //detailModel.SegmentCategory1 = detail.SegmentCategory1;
                                //detailModel.SegmentCategory2 = detail.SegmentCategory2;
                                detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;

                                lstDetailModel.Add(detailModel);
                            }
                        }
                    }
                    if (CNApplication.IsRevExcess != true)
                        CNAModel.CreditNoteApplicationDetailModels = lstDetailModel.OrderBy(c => c.DocDate).ThenBy(d => d.SystemReferenceNumber).ToList();
                    else
                        CNAModel.ReverseExcessModels = CNApplication.CreditNoteApplicationDetails.Select(d => new ReverseExcessModel()
                        {
                            Id = d.Id,
                            CompanyId = companyId,
                            DocAmount = d.CreditAmount,
                            DocTaxAmount = d.TaxAmount,
                            DocTotalAmount = d.TotalAmount,
                            TaxId = d.TaxId,
                            TaxRate = d.TaxRate,
                            TaxIdCode = d.TaxIdCode,
                            RecOrder = d.RecOrder,
                            COAId = d.COAId,
                            ItemDescription = d.DocDescription
                        }).OrderBy(d => d.RecOrder).ToList();
                }
                else
                {
                    CreditNoteApplication CNA = _creditNoteApplicationService.GetCreditNoteByCompanyId(companyId);
                    CNAModel.Id = Guid.NewGuid();
                    CNAModel.EntityName = _beanEntityService.GetEntityName(creditNote.EntityId);
                    CNAModel.CompanyId = companyId;
                    CNAModel.InvoiceId = creditNoteId;
                    Invoice invoice = _invoiceEntityService.GetinvoiceById(creditNoteId);
                    CNAModel.DocCurrency = invoice.DocCurrency;
                    CNAModel.DocNo = invoice.DocNo;
                    CNAModel.DocDate = creditNote.DocDate;
                    CNAModel.CreditNoteAmount = invoice.GrandTotal;
                    CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
                    CNAModel.CreditNoteBalanceAmount = invoice.BalanceAmount;
                    CNAModel.CreditAmount = invoice.GrandTotal;
                    CNAModel.CreditNoteApplicationNumber = invoice.InvoiceNumber;
                    CNAModel.ExchangeRate = invoice.ExchangeRate;
                    CNAModel.GSTExchangeRate = invoice.GSTExchangeRate;
                    CNAModel.Remarks = invoice.DocDescription;
                    CNAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
                    CNAModel.NoSupportingDocument = false;
                    List<Invoice> lstInvoice = _invoiceEntityService.GetAllCreditNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                    List<DebitNote> lstDebitNote = _debitNoteService.GetAllDebitNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                    if (lstInvoice.Any())
                        lstServiceEntityIds.AddRange(lstInvoice.Select(c => c.ServiceCompanyId.Value).ToList());
                    if (lstDebitNote.Any())
                        lstServiceEntityIds.AddRange(lstDebitNote.Select(c => c.ServiceCompanyId.Value).ToList());
                    if (lstServiceEntityIds.Any())
                        lstComapny = _companyService.GetAllCompanies(lstServiceEntityIds).ToList();
                    List<CreditNoteApplicationDetailModel> lstCredtNoteDetail = new List<CreditNoteApplicationDetailModel>();
                    foreach (Invoice detail in lstInvoice)
                    {
                        CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                        detailModel.DocNo = detail.DocNo;
                        detailModel.DocType = detail.DocType;
                        detailModel.DocumentId = detail.Id;
                        detailModel.DocDate = detail.DocDate;
                        detailModel.DocAmount = detail.GrandTotal;
                        detailModel.DocCurrency = detail.DocCurrency;
                        detailModel.BalanceAmount = detail.BalanceAmount;
                        detailModel.Nature = detail.Nature;
                        detailModel.ServiceEntityId = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                        detailModel.ServEntityName = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault();
                        detailModel.DocState = detail.DocumentState;
                        detailModel.SystemReferenceNumber = detail.InvoiceNumber;
                        //detailModel.SegmentCategory1 = detail.SegmentCategory1;
                        //detailModel.SegmentCategory2 = detail.SegmentCategory2;
                        detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;

                        //CNAModel.CreditNoteApplicationDetailModels.Add(detailModel);
                        lstCredtNoteDetail.Add(detailModel);
                    }

                    foreach (DebitNote detail in lstDebitNote)
                    {
                        CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                        detailModel.DocNo = detail.DocNo;
                        detailModel.DocType = DocTypeConstants.DebitNote;
                        detailModel.DocumentId = detail.Id;
                        detailModel.DocDate = detail.DocDate;
                        detailModel.DocAmount = detail.GrandTotal;
                        detailModel.DocCurrency = detail.DocCurrency;
                        detailModel.ServiceEntityId = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                        detailModel.ServEntityName = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault();
                        detailModel.DocState = detail.DocumentState;
                        detailModel.BalanceAmount = detail.BalanceAmount;
                        detailModel.Nature = detail.Nature;
                        detailModel.SystemReferenceNumber = detail.DebitNoteNumber;
                        //detailModel.SegmentCategory1 = detail.SegmentCategory1;
                        //detailModel.SegmentCategory2 = detail.SegmentCategory2;
                        detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;

                        // CNAModel.CreditNoteApplicationDetailModels.Add(detailModel);
                        lstCredtNoteDetail.Add(detailModel);
                    }
                    CNAModel.CreditNoteApplicationDetailModels = lstCredtNoteDetail.OrderBy(x => x.DocDate).ThenBy(x => x.SystemReferenceNumber).ToList();
                }

                //CNAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                //CNAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                //CNAModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                //CNAModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplication_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteApplication_CreateCall_Exception_Message);
                ////LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return CNAModel;
        }
        #endregion

        #region CreateCreditNoteDocumentVoid
        //public DocumentVoidModel CreateCreditNoteDocumentVoid(Guid id, long companyId)
        //{
        //    DocumentVoidModel DVModel = new DocumentVoidModel();
        //    try
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteDocumentVoid_CreateCall_Request_Message);
        //        FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
        //        if (financSettings == null)
        //            throw new Exception(CreditNoteValidation.Credit_amount_should_be_less_than_or_equal_to_Balance_Amount);

        //        Invoice doubtfulDebt = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(companyId, id);
        //        if (doubtfulDebt == null)
        //            throw new Exception(CreditNoteValidation.Invalid_CreditNote);
        //        DVModel.CompanyId = companyId;
        //        DVModel.Id = (doubtfulDebt == null) ? Guid.NewGuid() : id;
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteDocumentVoid_CreateCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteDocumentVoid_CreateCall_SuccessFully_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }

        //    return DVModel;
        //}

        #endregion

        #region SaveCreditNoteDocumentVoid
        public Invoice SaveCreditNoteDocumentVoid(DocumentVoidModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteDocumentVoid_SaveCall_Request_Message);
            //to check if it is void or not
            if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new InvalidOperationException(CommonConstant.This_transaction_has_already_void);
            string DocNo = "-V";
            bool? isVoid = true;
            //string DocDescription = "Void-";
            Invoice _document = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(TObject.CompanyId, TObject.Id);
            if (_document == null)
                throw new InvalidOperationException(CreditNoteValidation.Invalid_CreditNote);
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);
            }

            if (_document.DocumentState != CreditNoteState.NotApplied)
                throw new InvalidOperationException("State should be " + CreditNoteState.NotApplied);

            if (_document.InvoiceDetails.Any(a => a.ClearingState == InvoiceState.Cleared))
                throw new InvalidOperationException(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            if (_document.ClearCount != null && _document.ClearCount > 0)
                throw new InvalidOperationException(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
            {
                throw new InvalidOperationException(CreditNoteValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new InvalidOperationException(CreditNoteValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CreditNoteValidation.Invalid_Financial_Period_Lock_Password);
                }
            }

            _document.DocumentState = CreditNoteState.Void;
            _document.DocNo = _document.DocNo + DocNo;
            _document.ModifiedDate = DateTime.UtcNow;
            _document.ModifiedBy = _document.UserCreated;
            _document.ObjectState = ObjectState.Modified;
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteDocumentVoid_SaveCall_SuccessFully_Message);
            try
            {
                unitOfWork.SaveChanges();
                //JournalSaveModel tObject = new JournalSaveModel();
                //tObject.Id = TObject.Id;
                //tObject.CompanyId = TObject.CompanyId;
                //tObject.DocNo = _document.DocNo;
                //tObject.ModifiedBy = _document.ModifiedBy;
                //deleteJVPostInvoce(tObject);

                if (_document != null)
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString))
                   { 
                        using (SqlCommand command = new SqlCommand("[Bean].[UpdateJournalVoid_proc]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Id", _document.Id);
                            command.Parameters.AddWithValue("@DocumentState", _document.DocumentState);
                            command.Parameters.AddWithValue("@DocNo", _document.DocNo);
                            command.Parameters.AddWithValue("@ModifiedBy", _document.ModifiedBy);
                            command.Parameters.AddWithValue("@ModifiedDate", DateTime.UtcNow);
                            command.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    try
                    {
                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_document.Id, _document.CompanyId, _document.Id, _document.DocType, _document.DocSubType, _document.DocumentState, _document.DocCurrency, _document.GrandTotal , _document.BalanceAmount, _document.ExchangeRate ?? 1, _document.ModifiedBy, _document.Remarks, null, 0, 0);
                            if (lstdocumet.Any())
                            {
                                AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                            }
                        
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                    }
                }

                if (_document.DocSubType == DocTypeConstants.Interco && _document.Nature == DocTypeConstants.Interco)
                {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_Insert_Document_History", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                        cmd.Parameters.AddWithValue("@DocumentId", _document.Id);
                        cmd.Parameters.AddWithValue("@DocumentType", DocTypeConstants.BillCreditMemo);
                        cmd.Parameters.AddWithValue("@IsVoid", true);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

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
                //    cmd.Parameters.AddWithValue("@DocType", _document.DocType);
                //    cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                //    cmd.Parameters.AddWithValue("@isEdit", false);
                //    cmd.Parameters.AddWithValue("@isVoid", isVoid);
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_SaveCreditNoteDocumentVoid_SaveCall_SuccessFully_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return _document;
        }
        #endregion

        #region CreateCreditNoteApplicationReset
        public DocumentResetModel CreateCreditNoteApplicationReset(Guid id, Guid creditNoteId, long companyId)
        {
            DocumentResetModel DDAModel = new DocumentResetModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CreditNoteValidation.The_Financial_setting_should_be_activated);
                }

                Invoice doubtfulDebt = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(companyId, creditNoteId);
                if (doubtfulDebt == null)
                {
                    throw new Exception(CreditNoteValidation.Invalid_CreditNote);
                }
                CreditNoteApplication DDAllocation = _creditNoteApplicationService.GetAllCreditNoteByCompanyIdAndId(id, creditNoteId, companyId);
                if (DDAllocation != null)
                {
                    DDAModel.CompanyId = companyId;
                    DDAModel.Id = id;
                    DDAModel.InvoiceId = creditNoteId;
                    DDAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                    DDAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                    DDAModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                    DDAModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                }
                else
                {
                    throw new Exception(CreditNoteValidation.Invalid_Credit_Note_Application);
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_SuccessFully_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return DDAModel;
        }
        #endregion

        #region SaveCreditNoteApplicationReset
        public CreditNoteApplication SaveCreditNoteApplicationReset(DocumentResetModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_Request_Message);
            //Need to verify the invoice is within Financial year
            //if (!_financialSettingService.ValidateYearEndLockDate(TObject.ResetDate.Value, TObject.CompanyId))
            //{
            //    throw new Exception(CreditNoteValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            //}

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.ResetDate.Value, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CreditNoteValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.ResetDate.Value, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CreditNoteValidation.Invalid_Financial_Period_Lock_Password);
                }
            }

            Invoice creditNote = _invoiceEntityService.GetCreditNoteByCompanyIdAndId(TObject.CompanyId, TObject.InvoiceId);

            if (creditNote == null)
            {
                throw new Exception(CreditNoteValidation.Invalid_CreditNote);
            }
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(creditNote.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            CreditNoteApplication allocation = _creditNoteApplicationService.GetAllCreditNote(TObject.InvoiceId, TObject.Id, TObject.CompanyId);
            List<DocumentHistoryModel> lstDocuments = new List<DocumentHistoryModel>();
            Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
            if (allocation != null)
            {
                //if (allocation.CreditNoteApplicationDate.Date > TObject.ResetDate.Value.Date)
                //    throw new Exception(CreditNoteValidation.Reset_Date_should_be_greater_than_Allocation_date);


                allocation.Status = CreditNoteApplicationStatus.Void;
                allocation.CreditNoteApplicationResetDate = TObject.ResetDate;
                allocation.CreditNoteApplicationNumber = allocation.CreditNoteApplicationNumber + "-V";
                allocation.ObjectState = ObjectState.Modified;
                allocation.ModifiedDate = DateTime.UtcNow;
                _creditNoteApplicationService.Update(allocation);
                //var updateJournal = _journalService.GetJournal(TObject.CompanyId, TObject.Id);
                //if (updateJournal != null)
                //{
                //    updateJournal.Status = CreditNoteApplicationStatus.Reset;
                //    updateJournal.DocumentState = "Reset";
                //    _journalService.Update(updateJournal);
                //}
                decimal? roundAmount = 0;
                foreach (CreditNoteApplicationDetail detail in allocation.CreditNoteApplicationDetails)
                    UpdateDocumentState(detail.DocumentId, detail.DocumentType, -detail.CreditAmount, ConnectionString, TObject.Id, allocation.CreditNoteApplicationResetDate, lstDocuments, lstOfRoundingAmount, out roundAmount);

                creditNote.BalanceAmount += allocation.CreditAmount;

                if (creditNote.BalanceAmount == 0)
                    creditNote.DocumentState = CreditNoteState.FullyApplied;
                else
                    creditNote.DocumentState = CreditNoteState.PartialApplied;


                if (creditNote.BalanceAmount == 0)
                    creditNote.DocumentState = CreditNoteState.FullyApplied;
                else if (creditNote.BalanceAmount > 0)
                    creditNote.DocumentState = CreditNoteState.PartialApplied;
                else
                {
                    throw new Exception(String.Format("CreditNote ({0}) Balance Amount is becoming negative", creditNote.InvoiceNumber));
                }
                if (creditNote.GrandTotal == creditNote.BalanceAmount)
                    creditNote.DocumentState = CreditNoteState.NotApplied;
                creditNote.ObjectState = ObjectState.Modified;
                //string docNo = "-R";
                //var lstcreditApplication = _journalDetailService.ListJDetail(allocation.Id);
                //if (lstcreditApplication.Any())
                //{
                //    foreach (var jDetail in lstcreditApplication)
                //    {
                //        JournalDetail newDetail = new JournalDetail();
                //        newDetail.Id = Guid.NewGuid();
                //        newDetail.DocCredit = jDetail.DocDebit;
                //        newDetail.DocDebit = jDetail.DocCredit;
                //        newDetail.BaseCredit = jDetail.BaseDebit;
                //        newDetail.COAId = jDetail.COAId;
                //        newDetail.BaseDebit = jDetail.BaseCredit;
                //        newDetail.DocNo = jDetail.DocNo + docNo;
                //        newDetail.SystemRefNo = jDetail.SystemRefNo + docNo;
                //        newDetail.DocDate = jDetail.DocDate;
                //        newDetail.DocType = jDetail.DocType;
                //        newDetail.DocumentDetailId = jDetail.DocumentDetailId;
                //        newDetail.DocumentId = jDetail.DocumentId;
                //        newDetail.ServiceCompanyId = jDetail.ServiceCompanyId;
                //        newDetail.JournalId = jDetail.JournalId;
                //        newDetail.ObjectState = ObjectState.Added;
                //        _journalDetailService.Insert(newDetail);
                //    }
                //}

                //Added By Pradhan
                //if (creditNote != null)
                //{
                //    var journalByMemoId = _journalService.GetJournal(TObject.CompanyId, TObject.InvoiceId);
                //    if (journalByMemoId != null)
                //    {
                //        journalByMemoId.BalanceAmount = creditNote.BalanceAmount;
                //        journalByMemoId.DocumentState = creditNote.DocumentState;
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
                //#region Update_Journal_Detail_Clearing_Status
                if (creditNote.IsOBInvoice != true && creditNote.DocSubType != DocTypeConstants.OpeningBalance)
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyId", TObject.CompanyId);
                    cmd.Parameters.AddWithValue("@documentId", creditNote.Id);
                    cmd.Parameters.AddWithValue("@docState", creditNote.DocumentState);
                    cmd.Parameters.AddWithValue("@balanceAmount", creditNote.BalanceAmount);
                    int count = cmd.ExecuteNonQuery();
                    con.Close();
                }
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(creditNote.Id, creditNote.CompanyId, creditNote.Id, creditNote.DocType, creditNote.DocSubType, creditNote.DocumentState, creditNote.DocCurrency, creditNote.GrandTotal, creditNote.BalanceAmount, creditNote.ExchangeRate.Value, creditNote.ModifiedBy != null ? creditNote.ModifiedBy : creditNote.UserCreated, creditNote.Remarks, creditNote.DocDate, 0, 0);
                    if (lstdocumet.Any())
                        //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                        lstDocuments.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {

                }

                if (lstDocuments.Any())
                    AppaWorld.Bean.Common.SaveDocumentHistory(lstDocuments, ConnectionString);

                #endregion Documentary History
                //#endregion Update_Journal_Detail_Clearing_Status
            }
            else
            {
                throw new Exception(CreditNoteValidation.Invalid_Credit_Note_Application);
            }
            try
            {
                unitOfWork.SaveChanges();
                if (creditNote != null)
                {
                    if (creditNote.DocSubType == DocTypeConstants.OpeningBalance && creditNote.DocType == DocTypeConstants.CreditNote && creditNote.DocSubType == DocTypeConstants.OpeningBalance)
                    {
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Enter_Into_Update_OB_LineItem);
                        SqlConnection con = new SqlConnection(ConnectionString);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                        oBcmd.CommandType = CommandType.StoredProcedure;
                        oBcmd.Parameters.AddWithValue("@OBId", creditNote.OpeningBalanceId);
                        oBcmd.Parameters.AddWithValue("@DocumentId", creditNote.Id);
                        oBcmd.Parameters.AddWithValue("@CompanyId", creditNote.CompanyId);
                        oBcmd.Parameters.AddWithValue("@IsEqual", creditNote.BalanceAmount == creditNote.GrandTotal ? true : false);
                        int res = oBcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = creditNote.DocNo;
                tObject.ModifiedBy = creditNote.ModifiedBy;
                deleteJVPostInvoce(tObject);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex; 
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;

            }

            return allocation;
        }
        #endregion


        public CreditNoteModel CreateCreditNoteByDebitNote(Guid debitNoteId, long companyId, string connectionString)
        {
            CreditNoteModel invDTO = new CreditNoteModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message);
                //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CreditNote);

                //to check if it is void or not
                if (_debitNoteService.IsVoid(companyId, debitNoteId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                //Invoice lastCreditNote = _invoiceEntityService.GetCreditNoteByCompanyId(companyId);
                DebitNote debitNote = _debitNoteService.GetAllDebitNoteById(debitNoteId);
                invDTO.Id = Guid.NewGuid();

                invDTO.CompanyId = debitNote.CompanyId;
                invDTO.EntityType = debitNote.EntityType;
                invDTO.DocSubType = DocTypeConstants.CreditNote;


                //bool? isEdit = false;
                //invDTO.DocNo = GetAutoNumberByEntityType(companyId, debitNote, DocTypeConstants.CreditNote, _autoNo, lastCreditNote, ref isEdit);
                //invDTO.IsDocNoEditable = isEdit;

                invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.CreditNote);
                if (invDTO.IsDocNoEditable == true)
                {
                    invDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstant.CreditNote, connectionString);
                }


                invDTO.CreditTermsId = debitNote.CreditTermsId;
                invDTO.DocDate = debitNote.DocDate;
                var top = _termsOfPaymentService.GetTermsOfPaymentById(invDTO.CreditTermsId);
                if (top != null)
                {
                    invDTO.DueDate = invDTO.DocDate.AddDays(top.TOPValue.Value);
                    invDTO.CreditTermsName = top.Name;

                }
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

                invDTO.IsGstSettings = debitNote.IsGstSettings;
                invDTO.GSTExCurrency = debitNote.GSTExCurrency;
                invDTO.GSTExchangeRate = debitNote.GSTExchangeRate;
                invDTO.GSTExDurationFrom = debitNote.GSTExDurationFrom;
                invDTO.GSTExDurationTo = debitNote.GSTExDurationTo;
                invDTO.ExtensionType = ExtensionType.DebitNote;

                invDTO.IsSegmentReporting = debitNote.IsSegmentReporting;
                //invDTO.SegmentCategory1 = debitNote.SegmentCategory1;
                //invDTO.SegmentCategory2 = debitNote.SegmentCategory2;

                invDTO.GSTTotalAmount = debitNote.GSTTotalAmount;
                invDTO.GrandTotal = debitNote.BalanceAmount;
                invDTO.BalanceAmount = debitNote.BalanceAmount;

                //invDTO.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
                invDTO.IsAllowableNonAllowable = debitNote.IsAllowableNonAllowable;

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
                invDTO.DocumentState = CreditNoteState.NotApplied;
                invDTO.CreatedDate = debitNote.CreatedDate;
                invDTO.UserCreated = debitNote.UserCreated;

                invDTO.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
                //invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
                //invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
                //invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);
                //invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(invDTO.CompanyId);


                List<InvoiceDetail> lstInvDetail = new List<InvoiceDetail>();
                if (debitNote.DebitNoteDetails.Any())
                {
                    foreach (var detail in debitNote.DebitNoteDetails)
                    {
                        InvoiceDetail cnDetail = new InvoiceDetail();
                        cnDetail.Id = Guid.NewGuid();
                        cnDetail.InvoiceId = invDTO.Id;
                        //cnDetail.AccountName = detail.AccountName;
                        //cnDetail.AllowDisAllow = detail.AllowDisAllow;
                        cnDetail.BaseAmount = detail.BaseAmount;
                        cnDetail.BaseTaxAmount = detail.BaseTaxAmount;
                        cnDetail.BaseTotalAmount = detail.BaseTotalAmount;
                        cnDetail.COAId = detail.COAId;
                        cnDetail.DocAmount = detail.DocAmount;
                        cnDetail.DocTaxAmount = detail.DocTaxAmount;
                        cnDetail.DocTotalAmount = detail.DocTotalAmount;
                        cnDetail.TaxId = detail.TaxId;
                        cnDetail.TaxRate = detail.TaxRate;
                        //cnDetail.IsPLAccount = detail.IsPLAccount;
                        cnDetail.RecOrder = detail.RecOrder;
                        cnDetail.ItemDescription = detail.AccountDescription;
                        cnDetail.TaxIdCode = detail.TaxIdCode;
                        //if (cnDetail.TaxId != null)
                        //{
                        //    //TaxCode tax = _taxCodeService.Query(c => c.Id == cnDetail.TaxId).Select().FirstOrDefault();
                        //    TaxCode tax = _taxCodeService.GetTaxCode(cnDetail.TaxId.Value);
                        //    cnDetail.TaxCode = tax.Code;
                        //    cnDetail.TaxType = tax.TaxType;
                        //    cnDetail.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
                        //}
                        lstInvDetail.Add(cnDetail);
                    }
                }
                invDTO.InvoiceDetails = lstInvDetail.OrderBy(x => x.RecOrder).ToList();
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

                CNAModel.Id = Guid.NewGuid();
                CNAModel.InvoiceId = invDTO.Id;
                CNAModel.CompanyId = debitNote.CompanyId;
                CNAModel.DocNo = debitNote.DocNo;
                CNAModel.DocCurrency = debitNote.DocCurrency;
                CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
                CNAModel.DocDate = invDTO.DocDate;
                CNAModel.Remarks = debitNote.Remarks;
                decimal sumLineTotal = 0;
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
                //CNAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(invDTO.CompanyId);
                //CNAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(invDTO.CompanyId);
                CNAModel.FinancialPeriodLockStartDate = invDTO.FinancialPeriodLockStartDate;
                CNAModel.FinancialPeriodLockEndDate = invDTO.FinancialPeriodLockEndDate;
                CNAModel.CreatedDate = DateTime.UtcNow;
                CNAModel.UserCreated = debitNote.UserCreated;
                CNAModel.Status = CreditNoteApplicationStatus.Posted;
                CNAModel.EntityName = beanEntity.Name;
                CNAModel.ExchangeRate = debitNote.ExchangeRate;

                invDTO.CreditNoteApplicationModel = CNAModel;

                List<CreditNoteApplicationDetailModel> lstCNADModel = new List<CreditNoteApplicationDetailModel>();
                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();

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
                detailModel.DocState = debitNote.DocumentState;
                detailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                detailModel.IsHyperLinkEnable = true;
                //detailModel.SegmentCategory1 = debitNote.SegmentCategory1;
                //detailModel.SegmentCategory2 = debitNote.SegmentCategory2;
                detailModel.BaseCurrencyExchangeRate = debitNote.ExchangeRate.Value;
                if (invDTO.ServiceCompanyId != null)
                {
                    Company company = _companyService.GetById(invDTO.ServiceCompanyId.Value);
                    if (company != null)
                    {
                        detailModel.ServiceEntityId = company.Id;
                        detailModel.ServEntityName = company.ShortName;
                    }
                }
                decimal sumLineTotal1 = 0;
                decimal diff = 0;
                if (invDTO.InvoiceDetails.Any())
                {
                    sumLineTotal1 = invDTO.InvoiceDetails.Sum(od => od.DocTotalAmount);
                }
                detailModel.CreditAmount = debitNote.BalanceAmount;
                lstCNADModel.Add(detailModel);

                invDTO.CreditNoteApplicationModel.CreditNoteApplicationDetailModels = lstCNADModel;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteByDebitNote_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteByDebitNote_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return invDTO;
        }

        private string GetAutoNumberByEntityType(long companyId, DebitNote lastInvoice, string entityType, AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo, Invoice oldInvoice, ref bool? isEdit)
        {
            string outPutNumber = null;
            string output = null;
            if (_autoNo != null)
            {
                string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewInvoiceDocumentNumber(entityType, companyId, false);
                    isEdit = true;
                    if (oldInvoice == null && outPutNumber == null)
                    {
                        output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                    }
                }

                else
                {
                    //invDTO.IsEditable = false;
                    isEdit = false;
                    //List<Invoice> lstInvoice = _invoiceEntityService.GetAllInvoiceByCIDandType(companyid, DocTypeConstants.Invoice);

                    string number = "1";
                    //if (oldInvoice != null)
                    //{
                    //    outPutNumber = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, companyId, false);
                    //}
                    //else
                    //{
                    if (oldInvoice != null)
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
                //}
            }
            return outPutNumber;
        }


        #endregion

        #region DoubtfulDebit

        #region CreateDoubtfulDebt
        public DoubtfulDebtModel CreateDoubtfulDebt(long companyid, Guid id, string connectionString, string username)
        {
            DoubtfulDebtModel invDTO = new DoubtfulDebtModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebt_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyid);
                if (financSettings == null)
                {
                    throw new Exception(DoubtfulDebitValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice document = _invoiceEntityService.GetDoubtfulDebtByIdAndCompanyId(companyid, id);
                //invDTO.IsSegmentActive1 = true;
                //invDTO.IsSegmentActive2 = true;
                if (document == null)
                {
                    //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyid, "Debt Provision");
                    Invoice lastInvoice = _invoiceEntityService.GetDoubtFuldbtByCompanyId(companyid);

                    invDTO.Id = Guid.NewGuid();
                    invDTO.CompanyId = companyid;
                    invDTO.ExtensionType = ExtensionType.General;
                    invDTO.DocDate = lastInvoice == null ? DateTime.Now : lastInvoice.DocDate;
                    //invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.DoubtFulDebitNote, companyid, false);
                    invDTO.DocumentState = DoubtfulDebtState.NotAllocated;

                    //bool? isEdit = false;
                    //invDTO.DocNo = GetAutoNumberByEntityType(companyid, lastInvoice, DocTypeConstants.DoubtFulDebitNote, _autoNo, false, ref isEdit);
                    //invDTO.IsDocNoEditable = isEdit;

                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.DoubtFulDebitNote);
                    if (invDTO.IsDocNoEditable == true)
                        invDTO.DocNo = _autoService.GetAutonumber(companyid, DocTypeConstants.DoubtFulDebitNote, connectionString);

                    //invDTO.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyid);
                    //invDTO.IsAllowableDisallowableActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyid);
                    //invDTO.IsAllowableNonAllowable = false;
                    //invDTO.IsMultiCurrency = false;
                    //invDTO.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, companyid);
                    invDTO.BaseCurrency = financSettings.BaseCurrency;
                    invDTO.DocCurrency = invDTO.BaseCurrency;
                    invDTO.CreatedDate = DateTime.UtcNow;
                    invDTO.IsLocked = false;
                    //Forex forexBean;
                    //MultiCurrencySetting multi = _mltiCurrencySettingService.GetByCompanyId(companyid);
                    //invDTO.IsMultiCurrency = multi != null;
                    //forexBean = _forexService.GetMultiCurrencyInformation(invDTO.BaseCurrency, invDTO.DocDate, true, invDTO.CompanyId);
                    //if (forexBean != null)
                    //{
                    //    invDTO.ExchangeRate = forexBean.UnitPerUSD;
                    //    invDTO.ExDurationFrom = forexBean.DateFrom;
                    //    invDTO.ExDurationTo = forexBean.Dateto;
                    //}
                }
                else
                {
                    if (!_companyService.GetPermissionBasedOnUser(document.ServiceCompanyId, document.CompanyId, username))
                        throw new Exception(CommonConstant.Access_denied);
                    FillDoubtfulDebt(invDTO, document);
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.DoubtFulDebitNote);
                    invDTO.IsLocked = document.IsLocked;
                    if (document.DocumentState != InvoiceState.Void)
                    {
                        AppsWorld.InvoiceModule.Entities.Journal journal = null;
                        if (invDTO.DocumentState == "Reversed")
                        {
                            journal = _journalService.GetJournalByDocId(companyid, id);
                        }
                        else
                            journal = _journalService.GetJournal(companyid, id);
                        if (journal != null)
                            invDTO.JournalId = journal.Id;
                    }
                }
                //invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyid);
                //invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyid);
                //invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyid);
                //invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyid);
                //var gst = _gstSettingService.GetByCompanyId(companyid);
                //if (gst != null)
                //    invDTO.DeRegistrationDate = gst.DeRegistration;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebt_CreateCall_Request_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebt_CreateCall_Request_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return invDTO;
        }
        #endregion

        #region GetAllDoubtfulDebtLUs
        public DoubtfulDebtLU GetAllDoubtfulDebtLUs(string username, Guid doubtfullId, long companyId)
        {
            DoubtfulDebtLU DDMLU = new DoubtfulDebtLU();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_GetAllDoubtfulDebtLUs_GetCall_Request_Message);
                Invoice invoice = _invoiceEntityService.GetDoubtfulDebtByIdAndCompanyId(companyId, doubtfullId);
                //DDMLU.NatureLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_Nature);
                DDMLU.NatureLU = new List<string> { "Trade", "Others" };
                DDMLU.CompanyId = companyId;
                if (invoice != null)
                {
                    string currencyCode = invoice.DocCurrency;
                    DDMLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, currencyCode, ControlCodeConstants.Currency_DefaultCode);
                    //var lookUpNature = _controlCodeCategoryService.GetInactiveControlcode(companyId,
                    //     ControlCodeConstants.Control_Codes_Nature, invoice.Nature);
                    //if (lookUpNature != null)
                    //{
                    //    DDMLU.NatureLU.Lookups.Add(lookUpNature);
                    //}
                }
                else
                {
                    DDMLU.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
                }

                //DDMLU.AllowableNonallowableLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                //List<LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
                //if (segments.Count > 0)
                //    DDMLU.SegmentCategory1LU = segments[0];
                //if (segments.Count > 1)
                //    DDMLU.SegmentCategory2LU = segments[1];

                //long comp = invoice == null ? 0 : invoice.CompanyId;
                long? comp = invoice == null ? 0 : invoice.ServiceCompanyId == null ? 0 : invoice.ServiceCompanyId;
                DDMLU.SubsideryCompanyLU = _companyService.GetSubCompany(username, companyId, comp);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_GetNextAllocationNumber_GetCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return DDMLU;
        }
        #endregion

        #region SaveDoubtfulDebt

        public Invoice SaveDoubtfulDebt(DoubtfulDebtModel TObject, string ConnectionString)
        {
            bool isNewAdd = false;
            bool isDocAdd = false;
            try
            {
                string _errors = CommonValidation.ValidateObject(TObject);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebt_SaveCall_Request_Message);

                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }

                //to check if it is void or not
                if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

                if (TObject.EntityId == null)
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Entity_is_mandatory);
                }

                if (TObject.SaveType == "InDirect")
                {
                    if (TObject.GrandTotal <= 0)
                        throw new InvalidOperationException(DoubtfulDebitValidation.Grand_Total_Should_Be_Grater_Than_Zero);
                    if (TObject.GrandTotal > TObject.BalanceAmount)
                        throw new InvalidOperationException(DoubtfulDebitValidation.Grand_Total_Should_Be_Less_Than_Or_Equal_To_Balance_Amount);
                    if (TObject.DoubtfulDebtAllocation.AllocateAmount > TObject.DoubtfulDebtAllocation.DoubtfulDebtBalanceAmount)
                        throw new InvalidOperationException(DoubtfulDebitValidation.DoubtfulDebt_Amount_should_be_less_than_or_equal_to_Remaining_Amount);
                    foreach (var detail in TObject.DoubtfulDebtAllocation.DoubtfulDebtAllocationDetailModels)
                    {
                        if (detail.AllocateAmount > detail.BalanceAmount)
                            throw new InvalidOperationException(DoubtfulDebitValidation.DoubtfulDebt_amount_should_be_less_than_or_equal_to_Invoice_Balance_Amount);
                    }
                }

                if (TObject.DocDate == null)
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Invalid_Document_Date);
                }
                if (TObject.ServiceCompanyId == null)
                    throw new InvalidOperationException(DoubtfulDebitValidation.Service_Company_is_mandatory);

                if (TObject.GrandTotal <= 0)
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Amount_should_be_greater_than_zero);
                }

                if (TObject.ExchangeRate == 0)
                    throw new InvalidOperationException(DoubtfulDebitValidation.ExchangeRate_Should_Be_Grater_Than_Zero);

                if (TObject.IsDocNoEditable == true)
                    if (IsDocumentNumberExists(DocTypeConstants.DoubtFulDebitNote, TObject.DocNo, TObject.Id, TObject.CompanyId))
                    {
                        throw new InvalidOperationException(DoubtfulDebitValidation.Document_number_already_exist);
                    }

                //Need to verify the invoice is within Financial year
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new InvalidOperationException(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                    }
                }
                Invoice _document = _invoiceEntityService.DoubtFulDebtbyId(TObject.Id);
                if (_document != null)
                {
                    //For data concurancy
                    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                    if (!timeStamp.Equals(TObject.Version))
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                    InsertDoubtfulDebt(TObject, _document, false);
                    _document.DocNo = TObject.DocNo;
                    _document.InvoiceNumber = _document.DocNo;
                    _document.ModifiedBy = TObject.ModifiedBy;
                    _document.ModifiedDate = DateTime.UtcNow;
                    _document.ObjectState = ObjectState.Modified;
                    _invoiceEntityService.Update(_document);
                }
                else
                {
                    isNewAdd = true;
                    _document = new Invoice();
                    InsertDoubtfulDebt(TObject, _document, true);
                    _document.Id = TObject.Id;
                    _document.Status = RecordStatusEnum.Active;
                    _document.UserCreated = TObject.UserCreated;
                    _document.CreatedDate = DateTime.UtcNow;
                    _document.ObjectState = ObjectState.Added;

                    _document.InvoiceNumber = TObject.IsDocNoEditable != true ?/* GenerateAutoNumberForType(company.Id, "Debt Provision", company.ShortName)*/ _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.DoubtFulDebitNote, ConnectionString) : TObject.DocNo;
                    _document.DocNo = _document.InvoiceNumber;
                    _invoiceEntityService.Insert(_document);
                }
                try
                {
                    unitOfWork.SaveChanges();

                    #region Posting_New_Approach_SP

                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_Posting", con);
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SourceId", _document.Id);
                        cmd.Parameters.AddWithValue("@Type", DocTypeConstants.DoubtFulDebitNote);
                        cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }

                    #endregion Posting_New_Approach_SP
                    if (TObject.SaveType == "InDirect")
                    {
                        TObject.DoubtfulDebtAllocation.PeriodLockPassword = TObject.PeriodLockPassword;
                        SaveDoubtFulDebtAllocation(TObject.DoubtfulDebtAllocation, ConnectionString);
                    }
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebt_saveCall_SuccessFully_Message);
                }
                catch (Exception ex)
                {
                    LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                    throw ex;
                }
                return _document;
            }
            catch (Exception ex)
            {
                if (isNewAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, DocTypeConstants.DoubtFulDebitNote, ConnectionString);
                }
                throw ex;
            }
        }

        public DoubtfulDebtAllocation SaveDoubtFulDebtAllocation(DoubtfulDebtAllocationModel TObject, string ConnectionString)
        {
            ValidateDoubtfulDebtAllocation(TObject);
            DateTime? oldAllocationDate = null;
            bool? isOldReverseExcessChecked = false;
            Invoice doubtfulDebt = _invoiceEntityService.GetinvoiceById(TObject.InvoiceId);
            DoubtfulDebtAllocation allocation = _doubtfulDebtAllocationService.GetAllDebtful(TObject.Id);
            //if the state is reset and trying to save.
            if (allocation != null && allocation.Status == DoubtfulDebtAllocationStatus.Reset)
                throw new Exception(CommonConstant.Document_has_been_modified_outside);
            List<Guid> lstInvoiceId = TObject.DoubtfulDebtAllocationDetailModels.Where(c => c.DocType == DocTypeConstants.Invoice).Select(c => c.DocumentId).ToList();
            List<Guid> lstDNId = TObject.DoubtfulDebtAllocationDetailModels.Where(c => c.DocType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList();
            List<Invoice> lstInv = _invoiceEntityService.GetAllDDByInvoiceId(lstInvoiceId);
            List<DebitNote> lstDNs = _debitNoteService.GetDDByDebitNoteId(lstDNId);
            List<DocumentHistoryModel> lstDocHistoryModels = new List<DocumentHistoryModel>();
            if (TObject.IsRevExcess != true)
                if (TObject.DoubtfulDebtAllocationDetailModels.Any())
                {
                    foreach (var item in TObject.DoubtfulDebtAllocationDetailModels)
                    {
                        if (item.DocType == DocTypeConstants.Invoice)
                        {
                            Invoice ddAmount = lstInv.Where(c => c.Id == item.DocumentId).FirstOrDefault();
                            if (item.AllocateAmount > ddAmount.BalanceAmount)
                                throw new Exception(DoubtfulDebitValidation.Amount_to_Allocate_cannot_be_greater_than_Amount_Due);
                        }
                        else
                        {
                            DebitNote debitAmount = lstDNs.Where(c => c.Id == item.DocumentId).FirstOrDefault();
                            if (item.AllocateAmount > debitAmount.BalanceAmount)
                                throw new Exception(DoubtfulDebitValidation.Amount_to_Allocate_cannot_be_greater_than_Amount_Due);
                        }
                    }
                }
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtFulDebtAllocation_SaveCall_Request_Message);
            bool isNew = false;
            if (allocation == null)
            {
                allocation = new DoubtfulDebtAllocation();
                isNew = true;
                oldAllocationDate = TObject.DoubtfulDebitAllocationDate;
                isOldReverseExcessChecked = TObject.IsRevExcess;
            }
            else
            {
                doubtfulDebt.BalanceAmount += allocation.AllocateAmount;
                oldAllocationDate = allocation.DoubtfulDebtAllocationDate;
                isOldReverseExcessChecked = allocation.IsRevExcess;
            }

            allocation.DoubtfulDebtAllocationDate = TObject.DoubtfulDebitAllocationDate;
            allocation.IsNoSupportingDocumentActivated = TObject.IsNoSupportingDocument.Value;
            if (allocation.IsNoSupportingDocumentActivated)
                allocation.IsNoSupportingDocument = TObject.NoSupportingDocument;
            allocation.AllocateAmount = TObject.AllocateAmount;
            allocation.Remarks = TObject.Remarks;
            allocation.Status = DoubtfulDebtAllocationStatus.Tagged;

            if (isNew)
            {
                allocation.Id = TObject.Id;
                allocation.InvoiceId = TObject.InvoiceId;
                allocation.CompanyId = TObject.CompanyId;
                allocation.DoubtfulDebtAllocationNumber = "";
                allocation.AllocateAmount = TObject.AllocateAmount;
                if (TObject.IsRevExcess != true)
                    UpdateDoubtfulDebtAllocationDetails(TObject, allocation, ConnectionString, allocation.Id, lstDocHistoryModels);
                allocation.IsRevExcess = TObject.IsRevExcess;
                allocation.DoubtfulDebtAllocationNumber = GetNextAllocationNumber(TObject.InvoiceId);
                allocation.UserCreated = TObject.UserCreated;
                allocation.CreatedDate = DateTime.UtcNow;
                allocation.ObjectState = ObjectState.Added;
                _doubtfulDebtAllocationService.Insert(allocation);
            }
            else
            {
                //Data Concurancy verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(allocation.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                if (TObject.IsRevExcess != true)
                {
                    UpdateDoubtfulDebtAllocationDetails(TObject, allocation, ConnectionString, allocation.Id, lstDocHistoryModels);

                    #region Delete_Reverse_Excess_Journal
                    AppsWorld.InvoiceModule.Entities.Journal jrnl = _journalService.GetJournal(TObject.CompanyId, TObject.Id);
                    if (jrnl != null)
                    {
                        List<JournalDetail> lstJDetail = _journalDetailService.AllJDetail(jrnl.Id);
                        if (lstJDetail.Any())
                            foreach (var detail in lstJDetail)
                                detail.ObjectState = ObjectState.Deleted;
                        jrnl.ObjectState = ObjectState.Deleted;
                    }

                    #endregion Delete_Reverse_Excess_Journal

                }
                else
                    UpdateDoubtfulProvisionDetails(TObject, allocation, lstDocHistoryModels);
                allocation.Status = DoubtfulDebtAllocationStatus.Tagged;
                allocation.ModifiedBy = TObject.ModifiedBy;
                allocation.ModifiedDate = DateTime.UtcNow;
                allocation.IsRevExcess = TObject.IsRevExcess;
                allocation.ObjectState = ObjectState.Modified;
                _doubtfulDebtAllocationService.Update(allocation);
            }
            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(doubtfulDebt.Id, doubtfulDebt.CompanyId, allocation.Id, doubtfulDebt.DocType, "Allocation", "Posted", doubtfulDebt.DocCurrency, allocation.AllocateAmount, allocation.AllocateAmount, doubtfulDebt.ExchangeRate.Value, allocation.ModifiedBy != null ? allocation.ModifiedBy : allocation.UserCreated, allocation.Remarks, allocation.DoubtfulDebtAllocationDate, allocation.AllocateAmount, 0);
                if (lstdocumet.Any())
                    lstDocHistoryModels.AddRange(lstdocumet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion Documentary History

            doubtfulDebt.BalanceAmount -= allocation.AllocateAmount;
            doubtfulDebt.ObjectState = ObjectState.Modified;
            doubtfulDebt.ModifiedBy = InvoiceConstants.System;
            doubtfulDebt.ModifiedDate = DateTime.UtcNow;
            if (doubtfulDebt.BalanceAmount == 0)
                doubtfulDebt.DocumentState = DoubtfulDebtState.FullyAllocated;
            else
                doubtfulDebt.DocumentState = DoubtfulDebtState.PartialAllocated;
            _invoiceEntityService.Update(doubtfulDebt);
            #region Documentary History
            try
            {

                List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, doubtfulDebt.CompanyId, doubtfulDebt.Id, doubtfulDebt.DocType, doubtfulDebt.DocSubType, doubtfulDebt.DocumentState, doubtfulDebt.DocCurrency, doubtfulDebt.GrandTotal, doubtfulDebt.BalanceAmount, doubtfulDebt.ExchangeRate.Value, /*doubtfulDebt.ModifiedBy != null ? doubtfulDebt.ModifiedBy : doubtfulDebt.UserCreated*/InvoiceConstants.System, doubtfulDebt.Remarks, allocation.DoubtfulDebtAllocationDate, allocation.AllocateAmount, 0);
                if (lstdocumet1.Any())
                    lstDocHistoryModels.AddRange(lstdocumet1);
                if (lstDocHistoryModels.Any())
                    AppaWorld.Bean.Common.SaveDocumentHistory(lstDocHistoryModels, ConnectionString);


                if (oldAllocationDate != TObject.DoubtfulDebitAllocationDate)
                {
                    string query = $"Update Bean.DocumentHistory Set PostingDate='{String.Format("{0:MM/dd/yyyy}", allocation.DoubtfulDebtAllocationDate)}' where TransactionId='{allocation.Id}' and CompanyId={allocation.CompanyId} and TransactionId<>DocumentId and doctype in ('Invoice','Debit Note') and AgingState is null;";
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
                    LoggingHelper.LogMessage(InvoiceConstants.InvoiceApplicationService, "Sucessfully Updated the documents in DocumentHistory if DocDate is changed in Edit Mode");
                }
                if (isOldReverseExcessChecked == true && allocation.IsRevExcess != true)
                {
                    string query = $"Update Bean.DocumentHistory Set AgingState='Deleted' where TransactionId='{allocation.Id}' and CompanyId={allocation.CompanyId} and TransactionId<>DocumentId and doctype in ('Invoice','Debit Note') and AgingState is null;";
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
                    LoggingHelper.LogMessage(InvoiceConstants.InvoiceApplicationService, "Sucessfully Updated the documents in DocumentHistory if DocDate is changed in Edit Mode");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion Documentary History



            #region journal_DocumentStateUpdate 
            AppsWorld.InvoiceModule.Entities.Journal journal = _journalService.GetJournal(TObject.CompanyId, TObject.InvoiceId);
            if (journal != null)
            {
                journal.DocumentState = doubtfulDebt.DocumentState;
                _journalService.Update(journal);
            }

            #endregion journal_DocumentStateUpdate  


            try
            {
                unitOfWork.SaveChanges();
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtFulDebtAllocation_saveCall_SuccessFully_Message);

                //new posting approch
                if (allocation.IsRevExcess == true)
                    AppaWorld.Bean.Common.SaveMultiplePosting(allocation.CompanyId, doubtfulDebt.Id, allocation.Id, DocTypeConstants.DoubtFulDebitNote, DocTypeConstants.Allocation, false, false, ConnectionString, null);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return allocation;
        }

        #endregion

        #region CreateDoubtFulDebtAllocation
        public DoubtfulDebtAllocationModel CreateDoubtFulDebtAllocation(Guid doubtfulDebtId, Guid ddAclocationId, long companyId, bool isView, DateTime allocationDate)
        {
            DoubtfulDebtAllocationModel DDAModel = new DoubtfulDebtAllocationModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtFulDebtAllocation_CreateCall_Request_Message);
                //to check if it is void or not
                //if (_invoiceEntityService.IsVoid(companyId, doubtfulDebtId))
                //    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(DoubtfulDebitValidation.The_Financial_setting_should_be_activated);
                }
                DDAModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                DDAModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice doubtfulDebt = _invoiceEntityService.GetDoubtfuldebtByCompanyIdAndId(doubtfulDebtId, companyId);
                if (doubtfulDebt == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Doubtful_Debit);
                }
                DDAModel.EntityName = doubtfulDebt != null ? _beanEntityService.GetEntityName(doubtfulDebt.EntityId) : null;
                DoubtfulDebtAllocation DDAllocation = _doubtfulDebtAllocationService.GetDoubtfuldebtIdAndCompanyId(doubtfulDebtId, ddAclocationId, companyId);
                if (DDAllocation != null)
                {
                    decimal? allocatedAmount = 0;
                    FillDoubtfulDebtAllocationModel(DDAModel, DDAllocation, _invoiceEntityService.GetinvoiceById(doubtfulDebtId));
                    List<DoubtfulDebtAllocationDetailModel> lstDetailModel = new List<DoubtfulDebtAllocationDetailModel>();
                    List<DoubtfulDebtAllocationDetail> lstDDAD = _doubtfulDebtallocationDetailService.GetDoubtfuDebtById(ddAclocationId);
                    List<DoubtfulDebtAllocationDetail> lstDDDetailByDocId = _doubtfulDebtallocationDetailService.GetDoubtfuDebtByDocId(lstDDAD.Select(c => c.DocumentId).ToList());
                    List<Invoice> lstInvoices = _invoiceEntityService.GetAllDDByInvoiceId(lstDDAD.Select(c => c.DocumentId).ToList());
                    List<DebitNote> lstDN = _debitNoteService.GetDDByDebitNoteId(lstDDAD.Select(c => c.DocumentId).ToList());
                    if (DDAllocation.Status == DoubtfulDebtAllocationStatus.Reset || _invoiceEntityService.IsVoid(companyId, doubtfulDebtId))
                    {
                        foreach (DoubtfulDebtAllocationDetail dad in lstDDAD.Where(x => x.AllocateAmount > 0).ToList())
                        {
                            DoubtfulDebtAllocationDetailModel DDAdetailModel = new DoubtfulDebtAllocationDetailModel();
                            DDAdetailModel.AllocateAmount = dad.AllocateAmount;
                            DDAdetailModel.DocCurrency = dad.DocCurrency;
                            DDAdetailModel.DocDate = dad.DocDate;
                            DDAdetailModel.DocNo = dad.DocNo;
                            DDAdetailModel.DocType = dad.DocumentType;
                            DDAdetailModel.DocumentId = dad.DocumentId;
                            DDAdetailModel.DoubtfulDebitAllocationId = dad.DoubtfulDebtAllocationId;
                            DDAdetailModel.Id = dad.Id;
                            DDAdetailModel.PreviousAllocateAmmount = allocatedAmount;
                            if (dad.DocumentType == DocTypeConstants.Invoice)
                            {
                                Invoice invoice = lstInvoices.Where(x => x.Id == dad.DocumentId).FirstOrDefault();
                                if (invoice != null)
                                {
                                    DDAdetailModel.DocAmount = invoice.GrandTotal;
                                    DDAdetailModel.DocDate = invoice.DocDate;
                                    DDAdetailModel.DocumentId = invoice.Id;
                                    DDAdetailModel.DocNo = invoice.DocNo;
                                    DDAdetailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                                    if (invoice.AllocatedAmount == null || invoice.AllocatedAmount == 0)
                                        DDAdetailModel.BalanceAmount = invoice.BalanceAmount;
                                    else if (DDAModel.Status == DoubtfulDebtAllocationStatus.Reset)
                                        DDAdetailModel.BalanceAmount = invoice.BalanceAmount - invoice.AllocatedAmount;
                                    else
                                        DDAdetailModel.BalanceAmount = invoice.DocumentState == InvoiceState.NotPaid ? invoice.GrandTotal : invoice.BalanceAmount - (invoice.AllocatedAmount == null ? 0 : invoice.AllocatedAmount.Value);
                                    DDAdetailModel.DocType = invoice.DocType;
                                    DDAdetailModel.Nature = invoice.Nature;
                                }
                                DDAdetailModel.PreviousAllocateAmmount = allocatedAmount;
                            }
                            else if (dad.DocumentType == DocTypeConstants.DebitNote)
                            {
                                DebitNote debitNote = lstDN.Where(c => c.Id == dad.DocumentId).FirstOrDefault();
                                if (debitNote != null)
                                {
                                    DDAdetailModel.DocAmount = debitNote.GrandTotal;
                                    DDAdetailModel.DocDate = debitNote.DocDate;
                                    DDAdetailModel.DocumentId = debitNote.Id;
                                    DDAdetailModel.DocNo = debitNote.DocNo;
                                    DDAdetailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                                    if (debitNote.AllocatedAmount == null || debitNote.AllocatedAmount == 0)
                                        DDAdetailModel.BalanceAmount = debitNote.BalanceAmount;
                                    else if (DDAModel.Status == DoubtfulDebtAllocationStatus.Reset)
                                        DDAdetailModel.BalanceAmount = debitNote.BalanceAmount - debitNote.AllocatedAmount;
                                    else
                                        DDAdetailModel.BalanceAmount = debitNote.DocumentState == InvoiceState.NotPaid ? debitNote.GrandTotal : debitNote.BalanceAmount - (debitNote.AllocatedAmount == null ? 0 : debitNote.AllocatedAmount.Value);
                                    DDAdetailModel.DocType = debitNote.DocSubType;
                                    DDAdetailModel.Nature = debitNote.Nature;
                                }
                                DDAdetailModel.PreviousAllocateAmmount = allocatedAmount;
                            }
                            lstDetailModel.Add(DDAdetailModel);
                        }
                        DDAModel.DoubtfulDebtAllocationDetailModels = lstDetailModel;
                    }
                    else
                    {

                        if (DDAllocation.IsRevExcess != true)
                        {
                            foreach (DoubtfulDebtAllocationDetail detail in lstDDAD)
                            {
                                DoubtfulDebtAllocationDetailModel detailModel = new DoubtfulDebtAllocationDetailModel();
                                detailModel.Id = detail.Id;
                                detailModel.DoubtfulDebitAllocationId = detail.DoubtfulDebtAllocationId;
                                detailModel.DocCurrency = DDAModel.DocCurrency;
                                if (lstDDDetailByDocId.Any())
                                {
                                    allocatedAmount = lstDDDetailByDocId.Where(x => x.DocumentId == detail.DocumentId && x.Id != detail.Id).Select(d => d.AllocateAmount).FirstOrDefault();
                                }
                                detailModel.AllocateAmount = detail.AllocateAmount;
                                if (detail.DocumentType == DocTypeConstants.Invoice)
                                {
                                    Invoice invoice = lstInvoices.Where(x => x.Id == detail.DocumentId).FirstOrDefault();
                                    if (invoice != null)
                                    {
                                        detailModel.DocAmount = invoice.GrandTotal;
                                        detailModel.DocDate = invoice.DocDate;
                                        detailModel.DocumentId = invoice.Id;
                                        detailModel.DocNo = invoice.DocNo;
                                        detailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                                        if (invoice.AllocatedAmount == null || invoice.AllocatedAmount == 0)
                                            detailModel.BalanceAmount = invoice.BalanceAmount;
                                        else if (DDAModel.Status == DoubtfulDebtAllocationStatus.Reset)
                                            detailModel.BalanceAmount = invoice.BalanceAmount - invoice.AllocatedAmount;
                                        else
                                            //detailModel.BalanceAmount = invoice.BalanceAmount - (invoice.AllocatedAmount == null ? 0 : invoice.AllocatedAmount.Value) + detail.AllocateAmount;
                                            detailModel.BalanceAmount = invoice.DocumentState == InvoiceState.NotPaid ? invoice.GrandTotal : invoice.BalanceAmount - (invoice.AllocatedAmount == null ? 0 : invoice.AllocatedAmount.Value);
                                        detailModel.DocType = invoice.DocType;
                                        detailModel.Nature = invoice.Nature;
                                    }
                                    detailModel.PreviousAllocateAmmount = allocatedAmount;
                                }
                                else if (detail.DocumentType == DocTypeConstants.DebitNote)
                                {
                                    DebitNote debitNote = lstDN.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                                    if (debitNote != null)
                                    {
                                        detailModel.DocAmount = debitNote.GrandTotal;
                                        detailModel.DocDate = debitNote.DocDate;
                                        detailModel.DocumentId = debitNote.Id;
                                        detailModel.DocNo = debitNote.DocNo;
                                        detailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                                        if (debitNote.AllocatedAmount == null || debitNote.AllocatedAmount == 0)
                                            detailModel.BalanceAmount = debitNote.BalanceAmount;
                                        else if (DDAModel.Status == DoubtfulDebtAllocationStatus.Reset)
                                            detailModel.BalanceAmount = debitNote.BalanceAmount - debitNote.AllocatedAmount;
                                        else
                                            //detailModel.BalanceAmount = debitNote.BalanceAmount - (debitNote.AllocatedAmount == null ? 0 : debitNote.AllocatedAmount.Value) + detail.AllocateAmount;
                                            detailModel.BalanceAmount = debitNote.DocumentState == InvoiceState.NotPaid ? debitNote.GrandTotal : debitNote.BalanceAmount - (debitNote.AllocatedAmount == null ? 0 : debitNote.AllocatedAmount.Value);
                                        detailModel.DocType = debitNote.DocSubType;
                                        detailModel.Nature = debitNote.Nature;
                                    }
                                    //detailModel.PreviousAllocateAmmount = debitNote.AllocatedAmount;
                                    detailModel.PreviousAllocateAmmount = allocatedAmount;
                                }
                                lstDetailModel.Add(detailModel);
                            }
                        }
                        if (isView != true)
                        {
                            List<Invoice> lstInvoice = _invoiceEntityService.GetAllInvoice(companyId, doubtfulDebt.EntityId, doubtfulDebt.DocCurrency, doubtfulDebt.ServiceCompanyId.Value, allocationDate, doubtfulDebt.Nature);
                            foreach (Invoice detail in lstInvoice)
                            {
                                var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                if (d == null)
                                {
                                    DoubtfulDebtAllocationDetailModel detailModel = new DoubtfulDebtAllocationDetailModel();
                                    detailModel.DocNo = detail.DocNo;
                                    detailModel.DocType = detail.DocType;
                                    detailModel.DocumentId = detail.Id;
                                    detailModel.DocDate = detail.DocDate;
                                    detailModel.DocAmount = detail.GrandTotal;
                                    detailModel.DocCurrency = detail.DocCurrency;
                                    detailModel.BalanceAmount = detail.DocumentState == InvoiceState.NotPaid ? detail.GrandTotal : detail.BalanceAmount - (detail.AllocatedAmount == null ? 0 : detail.AllocatedAmount.Value);
                                    detailModel.Nature = detail.Nature;
                                    detailModel.SystemReferenceNumber = detail.InvoiceNumber;
                                    detailModel.PreviousAllocateAmmount = detail.AllocatedAmount;
                                    if (detailModel.BalanceAmount != 0)
                                        lstDetailModel.Add(detailModel);
                                }
                            }
                            List<DebitNote> lstDebitNote = _debitNoteService.GetAllDebitNoteByIdAndNature(companyId, doubtfulDebt.EntityId, doubtfulDebt.DocCurrency, doubtfulDebt.ServiceCompanyId.Value, allocationDate, doubtfulDebt.Nature);
                            foreach (DebitNote detail in lstDebitNote)
                            {
                                var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                if (d == null)
                                {
                                    DoubtfulDebtAllocationDetailModel detailModel = new DoubtfulDebtAllocationDetailModel();
                                    detailModel.DocNo = detail.DocNo;
                                    detailModel.DocType = DocTypeConstants.DebitNote;
                                    detailModel.DocumentId = detail.Id;
                                    detailModel.DocDate = detail.DocDate;
                                    detailModel.DocAmount = detail.GrandTotal;
                                    detailModel.DocCurrency = detail.DocCurrency;
                                    detailModel.BalanceAmount = detail.DocumentState == InvoiceState.NotPaid ? detail.GrandTotal : detail.BalanceAmount - (detail.AllocatedAmount == null ? 0 : detail.AllocatedAmount.Value);
                                    detailModel.Nature = detail.Nature;
                                    detailModel.SystemReferenceNumber = detail.DebitNoteNumber;
                                    detailModel.PreviousAllocateAmmount = detail.AllocatedAmount;
                                    if (detailModel.BalanceAmount != 0)
                                        lstDetailModel.Add(detailModel);
                                    //lstDetailModel.Add(detailModel);
                                }
                            }
                        }
                        //if (DDAllocation.IsRevExcess != true)
                        DDAModel.DoubtfulDebtAllocationDetailModels = lstDetailModel.OrderBy(d => d.DocDate).ToList();
                        //else
                        if (DDAllocation.IsRevExcess == true)
                            DDAModel.DDReverseExcessModels = DDAllocation.DoubtfulDebtAllocationDetails.Select(d => new DDReverseExcessModel()
                            {
                                Id = d.Id,
                                DoubtfulDebtAllocationId = d.DoubtfulDebtAllocationId,
                                DocCurrency = d.DocCurrency,
                                AllocateAmount = d.AllocateAmount,
                                DocNo = d.DocNo,
                                DocDate = d.DocDate,
                                EntityId = d.EntityId,
                                ExchangeRate = d.ExchangeRate
                            }).ToList();
                    }

                }
                else
                {
                    DoubtfulDebtAllocation DDA = _doubtfulDebtAllocationService.GetDoubtfilDebtCompanyId(companyId);

                    DDAModel.Id = Guid.NewGuid();
                    DDAModel.CompanyId = companyId;
                    DDAModel.InvoiceId = doubtfulDebtId;
                    var invoice = _invoiceEntityService.GetinvoiceById(doubtfulDebtId);
                    DDAModel.DocNo = invoice.DocNo;
                    DDAModel.DocCurrency = invoice.DocCurrency;
                    DDAModel.DoubtfulDebtAmount = invoice.GrandTotal;
                    DDAModel.DoubtfulDebtBalanceAmount = invoice.BalanceAmount;
                    DDAModel.DoubtfulDebtAllocationNumber = invoice.InvoiceNumber;
                    DDAModel.DocDate = invoice.DocDate;
                    DDAModel.NoSupportingDocument = false;
                    DDAModel.ExchangeRate = invoice.ExchangeRate;
                    DDAModel.IsNoSupportingDocument = false;
                    List<Invoice> lstInvoice = _invoiceEntityService.GetAllInvoice(companyId, doubtfulDebt.EntityId, doubtfulDebt.DocCurrency, doubtfulDebt.ServiceCompanyId.Value, allocationDate, doubtfulDebt.Nature);
                    List<DoubtfulDebtAllocationDetailModel> lstDoubtfulAllocation = new List<DoubtfulDebtAllocationDetailModel>();
                    foreach (Invoice detail in lstInvoice)
                    {
                        if (detail.AllocatedAmount != detail.BalanceAmount)
                        {
                            DoubtfulDebtAllocationDetailModel detailModel = new DoubtfulDebtAllocationDetailModel();
                            detailModel.DocNo = detail.DocNo;
                            detailModel.DocType = detail.DocType;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocDate = detail.DocDate;
                            detailModel.DocAmount = detail.GrandTotal;
                            detailModel.DocCurrency = detail.DocCurrency;
                            //detailModel.BalanceAmount = detail.DocumentState == InvoiceState.NotPaid ? detail.GrandTotal : detail.BalanceAmount - (detail.AllocatedAmount == null ? 0 : detail.AllocatedAmount.Value);
                            detailModel.BalanceAmount = detail.DocumentState == InvoiceState.NotPaid ? detail.GrandTotal : detail.BalanceAmount;
                            //detailModel.BalanceAmount = detail.BalanceAmount;
                            detailModel.Nature = detail.Nature;
                            detailModel.SystemReferenceNumber = detail.InvoiceNumber;
                            detailModel.PreviousAllocateAmmount = detail.AllocatedAmount;
                            if (detailModel.BalanceAmount != 0)
                                lstDoubtfulAllocation.Add(detailModel);
                        }
                    }
                    List<DebitNote> lstDebitNote = _debitNoteService.GetAllDebitNoteByIdAndNature(companyId, doubtfulDebt.EntityId, doubtfulDebt.DocCurrency, doubtfulDebt.ServiceCompanyId.Value, allocationDate, doubtfulDebt.Nature);

                    foreach (DebitNote detail in lstDebitNote)
                    {
                        if (detail.AllocatedAmount != detail.BalanceAmount)
                        {
                            DoubtfulDebtAllocationDetailModel detailModel = new DoubtfulDebtAllocationDetailModel();
                            detailModel.DocNo = detail.DocNo;
                            detailModel.DocType = DocTypeConstants.DebitNote;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocDate = detail.DocDate;
                            detailModel.DocAmount = detail.GrandTotal;
                            detailModel.DocCurrency = detail.DocCurrency;
                            //detailModel.BalanceAmount = detail.DocumentState == InvoiceState.NotPaid ? detail.GrandTotal : detail.BalanceAmount - (detail.AllocatedAmount == null ? 0 : detail.AllocatedAmount.Value);
                            detailModel.BalanceAmount = detail.DocumentState == InvoiceState.NotPaid ? detail.GrandTotal : detail.BalanceAmount;
                            //detailModel.BalanceAmount = detail.BalanceAmount;
                            detailModel.Nature = detail.Nature;
                            detailModel.SystemReferenceNumber = detail.DebitNoteNumber;
                            detailModel.PreviousAllocateAmmount = detail.AllocatedAmount;
                            if (detailModel.BalanceAmount != 0)
                                lstDoubtfulAllocation.Add(detailModel);
                        }
                    }
                    DDAModel.DoubtfulDebtAllocationDetailModels = lstDoubtfulAllocation.OrderBy(c => c.DocDate).ToList();
                }
                //DDAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                //DDAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                //DDAModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                //DDAModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtFulDebtAllocation_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtFulDebtAllocation_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return DDAModel;
        }
        #endregion

        #region CreateDoubtfulDebtReverse
        public DoubtfulDebtReverseModel CreateDoubtfulDebtReverse(Guid doubtfulDebtId, long companyId)
        {
            DoubtfulDebtReverseModel DDRModel = new DoubtfulDebtReverseModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtReverse_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.The_Financial_setting_should_be_activated);
                }

                Invoice doubtfulDebt = _invoiceEntityService.GetDoubtfuldebtByCompanyIdAndId(doubtfulDebtId, companyId);

                if (doubtfulDebt != null)
                {
                    DDRModel.CompanyId = companyId;
                    DDRModel.DoubtfulDebtId = doubtfulDebtId;
                    var invoice = _invoiceEntityService.GetinvoiceById(doubtfulDebtId);
                    DDRModel.DocDate = invoice.DocDate;
                    DDRModel.ReverseDate = invoice.ReverseDate;
                    DDRModel.Version = "0x" + string.Concat(Array.ConvertAll(doubtfulDebt.Version, x => x.ToString("X2")));
                    DDRModel.ReverseIsSupportingDocument = invoice.ReverseIsSupportingDocument;
                    DDRModel.ReverseRemarks = invoice.ReverseRemarks;
                }
                else
                {
                    DDRModel.CompanyId = companyId;
                    DDRModel.DoubtfulDebtId = Guid.NewGuid();
                }
                DDRModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                DDRModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                DDRModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                DDRModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtReverse_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtReverse_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return DDRModel;

        }
        #endregion

        #region SaveDoubtfulDebtReverse
        public Invoice SaveDoubtfulDebtReverse(DoubtfulDebtReverseModel TObject)
        {
            //Need to verify the invoice is within Financial year
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebtReverse_SaveCall_Request_Message);
            if (!_financialSettingService.ValidateYearEndLockDate(TObject.ReverseDate.Value, TObject.CompanyId))
            {
                throw new InvalidOperationException(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.ReverseDate.Value, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
            }
            Invoice _document = _invoiceEntityService.GetInvoiceByIdAndComapnyId(TObject.CompanyId, TObject.DoubtfulDebtId);
            if (_document != null)
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);
                _document.ReverseDate = TObject.ReverseDate;
                _document.ReverseIsSupportingDocument = TObject.ReverseIsSupportingDocument;
                _document.ReverseRemarks = TObject.ReverseRemarks;
                _document.DocumentState = DoubtfulDebtState.Reversed;
                //_document.ModifiedDate = DateTime.UtcNow;
                //_document.ModifiedBy = _document.UserCreated;
                _document.ObjectState = ObjectState.Modified;
                _invoiceEntityService.Update(_document);

                #region commented_code
                //new doubtful debit
                //Invoice docNew = new Invoice();
                //docNew.Id = Guid.NewGuid();
                //docNew.CompanyId = TObject.CompanyId;
                //docNew.DocType = DocTypeConstants.DoubtFulDebitNote;
                //docNew.DocSubType = "DoubtfulDebt";
                //docNew.EntityType = "Customer";
                //docNew.DocDate = TObject.ReverseDate.Value.Date;
                //docNew.DocNo = _document.DocNo + "-R";
                //docNew.EntityId = _document.EntityId;
                //docNew.Nature = _document.Nature;
                //docNew.ServiceCompanyId = _document.ServiceCompanyId;
                //docNew.DocCurrency = _document.DocCurrency;
                //docNew.IsMultiCurrency = _document.IsMultiCurrency;
                //docNew.ExCurrency = _document.ExCurrency;
                //docNew.ExchangeRate = _document.ExchangeRate;

                //docNew.ExtensionType = _document.ExtensionType;
                //docNew.ExDurationFrom = _document.ExDurationFrom;
                //docNew.ExDurationTo = _document.ExDurationTo;
                ////decimal diff = 0;
                ////docNew.BalanceAmount = docNew.BalanceAmount + diff;

                //docNew.GrandTotal = _document.GrandTotal;
                //docNew.BalanceAmount = _document.GrandTotal;
                //docNew.IsAllowableNonAllowable = _document.IsAllowableNonAllowable;
                //docNew.IsNoSupportingDocument = _document.IsNoSupportingDocument;
                //docNew.NoSupportingDocs = _document.NoSupportingDocs;

                //docNew.IsSegmentReporting = _document.IsSegmentReporting;

                //docNew.SegmentMasterid1 = _document.SegmentMasterid1;
                //docNew.SegmentDetailid1 = _document.SegmentDetailid1;
                //docNew.SegmentCategory1 = _document.SegmentCategory1;

                //docNew.SegmentMasterid2 = _document.SegmentMasterid2;
                //docNew.SegmentDetailid2 = _document.SegmentDetailid2;
                //docNew.SegmentCategory2 = _document.SegmentCategory2;

                //docNew.Remarks = _document.Remarks;
                //docNew.IsSegmentReporting = _document.IsSegmentReporting;
                //docNew.Status = RecordStatusEnum.Active;
                //docNew.IsBaseCurrencyRateChanged = _document.IsBaseCurrencyRateChanged;
                //docNew.DocumentState = DoubtfulDebtState.NotAllocated;
                //docNew.UserCreated = _document.UserCreated;
                //docNew.CreatedDate = DateTime.UtcNow;
                //docNew.InvoiceNumber = _document.InvoiceNumber + "-R";
                //docNew.ObjectState = ObjectState.Added;
                //_invoiceEntityService.Insert(docNew);

                #endregion


                AppsWorld.InvoiceModule.Entities.Journal journal = _journalService.GetDoubtfulDebtRecord(TObject.CompanyId, TObject.DoubtfulDebtId);
                if (journal != null)
                {
                    #region commented_Code
                    //journal.DocumentState = "Reversed";
                    //journal.ObjectState = ObjectState.Modified;
                    //_journalService.Update(journal);
                    //AppsWorld.InvoiceModule.Entities.Journal newjournal = new Entities.Journal();
                    //journal.Id = Guid.NewGuid();
                    //newjournal = journal;
                    //newjournal.Id = Guid.NewGuid();
                    //newjournal.SystemReferenceNo = journal.SystemReferenceNo + "-R";
                    //newjournal.DocumentId = TObject.DoubtfulDebtId;
                    //newjournal.Status = RecordStatusEnum.Active;
                    //newjournal.DocumentState = "Not Allocated";
                    //newjournal.ObjectState = ObjectState.Added;
                    //_journalService.Insert(newjournal);
                    //if (journal.JournalDetails.Any())
                    //{
                    //    foreach (var journalDetail in journal.JournalDetails)
                    //    {
                    //        JournalDetail jDetail = new JournalDetail();
                    //        jDetail = journalDetail;
                    //        jDetail.Id = Guid.NewGuid();
                    //        jDetail.SystemRefNo = newjournal.SystemReferenceNo;
                    //        jDetail.JournalId = newjournal.Id;
                    //        jDetail.DocCredit = journalDetail.DocDebit;
                    //        jDetail.DocDebit = journalDetail.DocCredit;
                    //        jDetail.BaseCredit = journalDetail.BaseDebit;
                    //        jDetail.BaseDebit = journalDetail.BaseCredit;
                    //        jDetail.DocTaxCredit = journalDetail.DocTaxDebit;
                    //        jDetail.DocTaxDebit = journalDetail.DocTaxCredit;
                    //        jDetail.ObjectState = ObjectState.Added;
                    //        _journalDetailService.Insert(jDetail);
                    //    }
                    //}
                    #endregion
                    JournalSaveModel saveModel = new JournalSaveModel();
                    saveModel.Id = journal.Id;
                    saveModel.CompanyId = journal.CompanyId;
                    saveModel.ReversalDate = TObject.ReverseDate;
                    //saveModel.DocumentId = docNew.Id;
                    SaveInvoice1(saveModel);
                }

            }
            try
            {
                unitOfWork.SaveChanges();
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebtReverse_saveCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebtReverse_SaveCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return _document;
        }

        public void SaveInvoice1(JournalSaveModel clientModel)
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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;

                var response = RestSharpHelper.Post(url, "api/journal/savereversal", json);

                if (response.ErrorMessage != null)
                {

                }
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        #endregion

        #region CreateDoubtfulDebtDocumentVoid
        public DocumentVoidModel CreateDoubtfulDebtDocumentVoid(Guid id, long companyId)
        {
            DocumentVoidModel DVModel = new DocumentVoidModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtDocumentVoid_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                    throw new Exception(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);

                Invoice doubtfulDebt = _invoiceEntityService.GetDoubtfuldebtByCompanyIdAndId(id, companyId);
                if (doubtfulDebt == null)
                    throw new Exception(DoubtfulDebitValidation.Invalid_Doubtful_Debit);
                if (doubtfulDebt != null)
                {
                    DVModel.CompanyId = companyId;
                    DVModel.Id = doubtfulDebt.Id;
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtDocumentVoid_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtDocumentVoid_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return DVModel;
        }
        #endregion

        #region SaveDoubtfulDebtDocumentVoid
        public Invoice SaveDoubtfulDebtDocumentVoid(DocumentVoidModel TObject)
        {
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebtDocumentVoid_SaveCall_Request_Message);

            //to check if it is void or not
            if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.This_transaction_has_already_void);

            string DocNo = "-V";
            //string DocDescription = "Void-";
            Invoice _document = _invoiceEntityService.GetDoubtfuldebtByCompanyIdAndId(TObject.Id, TObject.CompanyId);
            if (_document == null)
                throw new Exception(DoubtfulDebitValidation.Invalid_Doubtful_Debit);
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            if (_document.DocumentState != DoubtfulDebtState.NotAllocated)
                throw new Exception("State should be " + DoubtfulDebtState.NotAllocated);

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
            {
                throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
            }

            _document.DocumentState = DoubtfulDebtState.Void;
            _document.DocNo = _document.DocNo + DocNo;
            _document.ModifiedDate = DateTime.UtcNow;
            _document.ModifiedBy = _document.UserCreated;
            _document.ObjectState = ObjectState.Modified;
            try
            {
                unitOfWork.SaveChanges();
                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = _document.DocNo;
                tObject.ModifiedBy = _document.ModifiedBy;
                deleteJVPostInvoce(tObject);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebtDocumentVoid_saveCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtfulDebtDocumentVoid_SaveCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return _document;
        }
        #endregion

        #region CreateDoubtfulDebtAllocationReset
        public DocumentResetModel CreateDoubtfulDebtAllocationReset(Guid id, Guid doubtfulDebtId, long companyId)
        {
            DocumentResetModel DDAModel = new DocumentResetModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtAllocationReset_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(DoubtfulDebitValidation.The_Financial_setting_should_be_activated);
                }
                Invoice doubtfulDebt = _invoiceEntityService.GetDoubtfuldebtByCompanyIdAndId(doubtfulDebtId, companyId);
                if (doubtfulDebt == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Doubtful_Debit);
                }

                DoubtfulDebtAllocation DDAllocation = _doubtfulDebtAllocationService.GetAllDoubtFulNote(doubtfulDebtId, id, companyId);
                if (DDAllocation != null)
                {
                    DDAModel.CompanyId = companyId;
                    DDAModel.Id = id;
                    DDAModel.InvoiceId = doubtfulDebtId;
                }
                else
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtAllocationReset_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtfulDebtAllocationReset_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return DDAModel;
        }
        #endregion

        #region SaveDobtfulDebtAllocationReset
        public DoubtfulDebtAllocation SaveDobtfulDebtAllocationReset(DocumentResetModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDobtfulDebtAllocationReset_SaveCall_Request_Message);
            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(TObject.ResetDate.Value, TObject.CompanyId))
            {
                throw new InvalidOperationException(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            List<DocumentHistoryModel> lstDocHistoryModels = new List<DocumentHistoryModel>();
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.ResetDate.Value, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.ResetDate.Value, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new InvalidOperationException(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
            }

            Invoice doubtfulDebt = _invoiceEntityService.GetDoubtfuldebtByCompanyIdAndId(TObject.InvoiceId, TObject.CompanyId);
            if (doubtfulDebt == null)
            {
                throw new InvalidOperationException(DoubtfulDebitValidation.Invalid_Doubtful_Debit);
            }
            DoubtfulDebtAllocation allocation = _doubtfulDebtAllocationService.GetAllDoubtFulNote(TObject.InvoiceId, TObject.Id, TObject.CompanyId);
            if (allocation != null)
            {
                //Data concurancy verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(allocation.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                if (allocation.Status == DoubtfulDebtAllocationStatus.Reset)
                    throw new InvalidOperationException(DoubtfulDebitValidation.It_Is_Already_In_Reset_State);
                if (allocation.DoubtfulDebtAllocationDate.Date > TObject.ResetDate.Value.Date)
                    throw new InvalidOperationException(DoubtfulDebitValidation.Reset_Date_should_be_greater_than_Allocation_date);
                allocation.Status = DoubtfulDebtAllocationStatus.Reset;
                allocation.DoubtfulDebtAllocationResetDate = TObject.ResetDate;
                allocation.ObjectState = ObjectState.Modified;
                allocation.ModifiedDate = DateTime.UtcNow;
                allocation.ModifiedBy = TObject.ModifiedBy;
                _doubtfulDebtAllocationService.Update(allocation);

                #region Documentary History
                try
                {
                    //List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(doubtfulDebt.Id, allocation.CompanyId, allocation.Id, doubtfulDebt.DocType, DocTypeConstants.Allocation, InvoiceState.Void, doubtfulDebt.DocCurrency, doubtfulDebt.GrandTotal, doubtfulDebt.BalanceAmount, doubtfulDebt.ExchangeRate.Value, doubtfulDebt.ModifiedBy != null ? doubtfulDebt.ModifiedBy : doubtfulDebt.UserCreated, doubtfulDebt.Remarks, null, 0, 0);

                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(doubtfulDebt.Id, allocation.CompanyId, allocation.Id, doubtfulDebt.DocType, DocTypeConstants.Allocation, allocation.IsRevExcess == true ? InvoiceState.Void : InvoiceState.Reset, doubtfulDebt.DocCurrency, doubtfulDebt.GrandTotal, doubtfulDebt.BalanceAmount, doubtfulDebt.ExchangeRate.Value, doubtfulDebt.ModifiedBy != null ? doubtfulDebt.ModifiedBy : doubtfulDebt.UserCreated, allocation.IsRevExcess == true ? doubtfulDebt.Remarks : InvoiceState.Void, allocation.DoubtfulDebtAllocationResetDate, allocation.IsRevExcess == true ? 0 : -allocation.AllocateAmount, 0);

                    if (lstdocumet.Any())
                        //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                        lstDocHistoryModels.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {

                }
                #endregion Documentary History


                List<Invoice> lstInvoice = allocation.IsRevExcess != true ? _invoiceEntityService.GetAllDDByInvoiceId(allocation.DoubtfulDebtAllocationDetails.Where(c => c.DocumentType == DocTypeConstants.Invoice).Select(a => a.DocumentId).ToList()) : null;
                List<DebitNote> lstDN = allocation.IsRevExcess != true ? _debitNoteService.GetDDByDebitNoteId(allocation.DoubtfulDebtAllocationDetails.Where(c => c.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId).ToList()) : null;

                foreach (DoubtfulDebtAllocationDetail detail in allocation.DoubtfulDebtAllocationDetails)
                    UpdateDocumentAllocationState(detail.DocumentId, detail.DocumentType, -detail.AllocateAmount, true, ConnectionString, allocation.Id, allocation.DoubtfulDebtAllocationResetDate, true, lstDocHistoryModels, lstInvoice, lstDN);


                doubtfulDebt.BalanceAmount += allocation.AllocateAmount;

                if (doubtfulDebt.BalanceAmount == 0)
                    doubtfulDebt.DocumentState = DoubtfulDebtState.FullyAllocated;
                else
                    doubtfulDebt.DocumentState = DoubtfulDebtState.PartialAllocated;


                if (doubtfulDebt.BalanceAmount == 0)
                    doubtfulDebt.DocumentState = DoubtfulDebtState.FullyAllocated;
                else if (doubtfulDebt.BalanceAmount > 0)
                    doubtfulDebt.DocumentState = DoubtfulDebtState.PartialAllocated;
                else
                {
                    throw new InvalidOperationException(String.Format("CreditNote ({0}) Balance Amount is becoming negative", doubtfulDebt.InvoiceNumber));
                }
                if (doubtfulDebt.GrandTotal == doubtfulDebt.BalanceAmount)
                    doubtfulDebt.DocumentState = DoubtfulDebtState.NotAllocated;
                doubtfulDebt.ModifiedBy = "System";
                doubtfulDebt.ObjectState = ObjectState.Modified;
            }
            else
            {
                throw new InvalidOperationException(DoubtfulDebitValidation.Invalid_Doubtful_Debit);
            }
            try
            {
                unitOfWork.SaveChanges();

                #region Documentary History
                try
                {
                    //List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(allocation.Id, doubtfulDebt.CompanyId, doubtfulDebt.Id, doubtfulDebt.DocType, doubtfulDebt.DocSubType, doubtfulDebt.DocumentState, doubtfulDebt.DocCurrency, doubtfulDebt.GrandTotal, doubtfulDebt.BalanceAmount, doubtfulDebt.ExchangeRate.Value, doubtfulDebt.ModifiedBy != null ? doubtfulDebt.ModifiedBy : doubtfulDebt.UserCreated, doubtfulDebt.Remarks, null, 0, 0);

                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(allocation.Id, doubtfulDebt.CompanyId, doubtfulDebt.Id, doubtfulDebt.DocType, doubtfulDebt.DocSubType, allocation.IsRevExcess == true ? doubtfulDebt.DocumentState : InvoiceState.Reset, doubtfulDebt.DocCurrency, doubtfulDebt.GrandTotal, doubtfulDebt.BalanceAmount, doubtfulDebt.ExchangeRate.Value, doubtfulDebt.ModifiedBy != null ? doubtfulDebt.ModifiedBy : doubtfulDebt.UserCreated, allocation.IsRevExcess == true ? doubtfulDebt.Remarks : doubtfulDebt.DocumentState, allocation.DoubtfulDebtAllocationResetDate, allocation.IsRevExcess == true ? 0 : allocation.AllocateAmount, 0);

                    if (lstdocumet.Any())
                    {
                        lstDocHistoryModels.AddRange(lstdocumet);

                        AppaWorld.Bean.Common.SaveDocumentHistory(lstDocHistoryModels, ConnectionString);
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion Documentary History

                if (allocation.IsRevExcess == true)
                {
                    #region update_Reverse_Allocation_Reset_JV
                    JournalSaveModel tObject = new JournalSaveModel();
                    tObject.Id = TObject.Id;
                    tObject.CompanyId = TObject.CompanyId;
                    tObject.DocNo = allocation.DoubtfulDebtAllocationNumber;
                    tObject.ModifiedBy = TObject.ModifiedBy;
                    deleteJVPostInvoce(tObject);
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDoubtFulDebtAllocationReset_Jornal_Exception_Message);
                    #endregion
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_SaveDobtfulDebtAllocationReset_saveCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {

                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_SaveDobtfulDebtAllocationReset_SaveCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return allocation;
        }
        #endregion

        #region GetAllDebitfulldebitK
        public async Task<IQueryable<DoubtfullDebitModelK>> GetAllDebitfulldebitK(string userName, long companyId)
        {
            return await _invoiceEntityService.GetAllDebitfulldebitK(userName, companyId);
        }
        #endregion


        public DoubtfulDebtModel CreateDoubtFulDebtByDebitNote(Guid debitNoteId, long companyId, string connectionString)
        {
            DoubtfulDebtModel invDTO = new DoubtfulDebtModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_Request_Message);
                //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, "Debt Provision");
                //Invoice lastInvoice = _invoiceEntityService.GetDoubtfulDebtByCompanyId(companyId);

                //to check if it is void or not
                if (_debitNoteService.IsVoid(companyId, debitNoteId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

                DebitNote debitNote = _debitNoteService.GetAllDebitNoteById(debitNoteId);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                invDTO.Id = Guid.NewGuid();

                invDTO.CompanyId = debitNote.CompanyId;
                invDTO.EntityType = debitNote.EntityType;
                invDTO.DocSubType = DocTypeConstants.DoubtFulDebitNote;
                //invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.DoubtFulDebitNote, invDTO.CompanyId, false);

                invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.DoubtFulDebitNote);
                if (invDTO.IsDocNoEditable == true)
                    invDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.DoubtFulDebitNote, connectionString);


                //bool? isEdit = false;
                //invDTO.DocNo = GetAutoNumberByEntityType(companyId, debitNote, DocTypeConstants.DoubtFulDebitNote, _autoNo, lastInvoice, ref isEdit);
                //invDTO.IsDocNoEditable = isEdit;

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
                //invDTO.IsSegmentReporting = debitNote.IsSegmentReporting;
                //invDTO.SegmentCategory1 = debitNote.SegmentCategory1;
                //invDTO.SegmentCategory2 = debitNote.SegmentCategory2;

                invDTO.GrandTotal = debitNote.BalanceAmount;
                invDTO.BalanceAmount = debitNote.BalanceAmount;

                //invDTO.IsAllowableNonAllowable = debitNote.IsAllowableNonAllowable;
                //invDTO.IsAllowableDisallowableActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
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

                DoubtfulDebtAllocationModel DDAModel = new DoubtfulDebtAllocationModel();
                DDAModel.Id = Guid.NewGuid();
                DDAModel.CompanyId = companyId;
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
                DDAModel.Status = DoubtfulDebtAllocationStatus.Tagged;
                DDAModel.DocDate = debitNote.DocDate;
                DDAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, invDTO.CompanyId);
                DDAModel.NoSupportingDocument = false;
                DDAModel.ExchangeRate = debitNote.ExchangeRate;
                DDAModel.EntityName = beanEntity.Name;
                DDAModel.Remarks = debitNote.Remarks;

                invDTO.DoubtfulDebtAllocation = DDAModel;

                List<DoubtfulDebtAllocationDetailModel> lstDDAD = new List<DoubtfulDebtAllocationDetailModel>();

                DoubtfulDebtAllocationDetailModel dDAD = new DoubtfulDebtAllocationDetailModel();

                dDAD.Id = Guid.NewGuid();
                dDAD.DoubtfulDebitAllocationId = DDAModel.Id;
                dDAD.DocType = DocTypeConstants.DebitNote;
                dDAD.DocumentId = debitNoteId;
                dDAD.DocCurrency = DDAModel.DocCurrency;
                dDAD.DocAmount = debitNote.GrandTotal;
                dDAD.DocDate = debitNote.DocDate;
                dDAD.DocumentId = debitNote.Id;
                dDAD.DocNo = debitNote.DocNo;
                dDAD.SystemReferenceNumber = debitNote.DebitNoteNumber;
                dDAD.AllocateAmount = debitNote.BalanceAmount;
                dDAD.BalanceAmount = debitNote.BalanceAmount;
                dDAD.Nature = debitNote.Nature;
                lstDDAD.Add(dDAD);

                invDTO.DoubtfulDebtAllocation.DoubtfulDebtAllocationDetailModels = lstDDAD;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return invDTO;

        }

        #endregion

        #region PrivateMethods

        public bool saveScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName)
        {
            ScreenRecordsSave screenRecords = new ScreenRecordsSave();
            screenRecords.ReferenceId = refrenceId;
            screenRecords.FeatureId = featureId;
            screenRecords.RecordId = recordId;
            screenRecords.recordName = recordName;
            screenRecords.UserName = userName;
            screenRecords.Date = date.Value;
            screenRecords.isAdd = isAdd;
            screenRecords.CursorName = "Bean Cursor";
            screenRecords.ScreenName = screenName;
            screenRecords.CompanyId = comapnyid;
            var json = RestSharpHelper.ConvertObjectToJason(screenRecords);
            try
            {
                object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.Post(url, "api/document/savescreenfolders", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<ScreenRecordsSave>(response.Content);
                    screenRecords = data;
                }
                else
                {
                    throw new Exception(response.Content);
                }
                return true;
            }
            catch (Exception ex)
            {
                //var message = ex.Message;
                LoggingHelper.LogError(InvoiceConstants.InvoiceApplicationService, ex, InvoiceLoggingValidation.Invoice_Folder_Creation_Failed);
                return false;
            }
        }


        private void FillInvoice(InvoiceModel invDTO, Invoice invoice, bool isCopy)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillInvoice_FillCall_Request_Message);
                invDTO.Id = isCopy ? Guid.NewGuid() : invoice.Id;
                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = invoice.DocSubType;
                invDTO.IsModify = isCopy ? false : invoice.ClearCount > 0;
                invDTO.DocType = invoice.DocType;
                invDTO.InvoiceNumber = invoice.InvoiceNumber;
                invDTO.DocNo = invoice.DocNo;
                invDTO.DocDescription = invoice.DocDescription;
                invDTO.DocDate = invoice.DocDate;
                invDTO.DueDate = invoice.DueDate;
                invDTO.PONo = invoice.PONo;
                invDTO.EntityId = invoice.EntityId;
                invDTO.EntityName = _beanEntityService.GetEntityName(invoice.EntityId);
                invDTO.CreditTermsId = invoice.CreditTermsId;
                invDTO.Version = "0x" + string.Concat(Array.ConvertAll(invoice.Version, x => x.ToString("X2")));

                //new customer Credit Limit Calculation
                if (invoice.DocumentState == InvoiceState.NotPaid)
                {
                    decimal? creditLimit = _beanEntityService.GetCteditLimitsValue(invoice.EntityId);
                    if (creditLimit != null)
                    {
                        invDTO.CustCreditlimit = invoice.GrandTotal + creditLimit;
                    }
                    else
                        invDTO.CustCreditlimit = null;
                }
                else
                    invDTO.CustCreditlimit = null;


                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;

                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;

                invDTO.IsGstSettings = invoice.IsGstSettings;
                invDTO.GSTExCurrency = invoice.GSTExCurrency;
                invDTO.GSTExchangeRate = invoice.GSTExchangeRate;
                invDTO.GSTExDurationFrom = invoice.GSTExDurationFrom;
                invDTO.GSTExDurationTo = invoice.GSTExDurationTo;

                invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                invDTO.IsRepeatingInvoice = invoice.IsRepeatingInvoice;
                invDTO.RepEveryPeriodNo = invoice.RepEveryPeriodNo;
                invDTO.RepEveryPeriod = invoice.RepEveryPeriod;
                invDTO.EndDate = invoice.RepEndDate;
                invDTO.ParentInvoiceID = invoice.ParentInvoiceID;

                invDTO.GSTTotalAmount = invoice.GSTTotalAmount;
                invDTO.GrandTotal = invoice.GrandTotal;
                invDTO.Remarks = invoice.Remarks;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;

                invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;

                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;

                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;

                invDTO.Status = invoice.Status;
                invDTO.DocumentState = isCopy ? null : invoice.DocumentState;
                invDTO.ModifiedDate = isCopy ? null : invoice.ModifiedDate;
                invDTO.ModifiedBy = isCopy ? null : invoice.ModifiedBy;
                invDTO.CreatedDate = isCopy ? null : invoice.CreatedDate;
                invDTO.UserCreated = isCopy ? null : invoice.UserCreated;

                invDTO.IsWorkFlowInvoice = invoice.IsWorkFlowInvoice;
                invDTO.CursorType = invoice.CursorType;
                invDTO.DocumentId = invoice.DocumentId;
                invDTO.InternalState = invoice.InternalState;
                invDTO.RecurInvId = invoice.RecurInvId;
                invDTO.IsOBInvoice = invoice.IsOBInvoice;
                invDTO.OpeningBalanceId = invoice.OpeningBalanceId;
                invDTO.AllocatedAmount = invoice.AllocatedAmount;
                invDTO.IsLocked = invoice.IsLocked;
                List<Item> lstItem = _itemService.GetAllItemById(invoice.InvoiceDetails.Select(c => c.ItemId).ToList(), invoice.CompanyId);
                List<InvoiceDetail> lstDetail = new List<InvoiceDetail>();
                InvoiceDetail invDetail;
                foreach (var invD in invoice.InvoiceDetails)
                {
                    invDetail = new InvoiceDetail();
                    invDetail.TaxId = invD.TaxId;
                    invDetail.TaxRate = invD.TaxRate;
                    invDetail.Id = invD.Id;
                    invDetail.AmtCurrency = invD.AmtCurrency;
                    invDetail.BaseAmount = invD.BaseAmount;
                    invDetail.BaseTaxAmount = invD.BaseTaxAmount;
                    invDetail.BaseTotalAmount = invD.BaseTotalAmount;
                    invDetail.UnitPrice = invD.UnitPrice;
                    invDetail.Unit = invD.Unit;
                    invDetail.COAId = invD.COAId;
                    invDetail.Discount = invD.Discount;
                    invDetail.DiscountType = invD.DiscountType;
                    invDetail.DocAmount = invD.DocAmount;
                    invDetail.DocTaxAmount = invD.DocTaxAmount;
                    invDetail.DocTotalAmount = invD.DocTotalAmount;
                    invDetail.InvoiceId = invD.InvoiceId;
                    invDetail.ItemId = invD.ItemId;
                    invDetail.ItemDescription = (invD.ItemDescription != null || invD.ItemDescription != string.Empty) ? invD.ItemDescription : lstItem.Where(c => c.Id == invD.ItemId).Select(d => d.Description).FirstOrDefault();
                    invDetail.Qty = invD.Qty;
                    invDetail.Remarks = invD.Remarks;
                    invDetail.RecOrder = invD.RecOrder;
                    invDetail.ClearingState = invD.ClearingState;
                    invDetail.TaxIdCode = invD.TaxIdCode;
                    lstDetail.Add(invDetail);
                }
                invDTO.InvoiceDetails = lstDetail.OrderBy(c => c.RecOrder).ToList();

                invDTO.InvoiceNotes = isCopy ? null : _invoiceNoteService.GetInvoiceByid(invoice.Id).OrderByDescending(a => a.ModifiedDate).ThenByDescending(a => a.CreatedDate).ToList();



            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }
        public decimal? GetCreditlimit(Guid id, decimal? creditLimitAmount, Guid invoiceId, bool isMulticurrency)
        {
            //creditLimitAmount = creditLimitAmount == null ? 0 : creditLimitAmount;
            if (creditLimitAmount == null)
                return creditLimitAmount;
            decimal? creditamount = 0;
            //List<InvoiceDetail> details = new List<InvoiceDetail>();
            //var invoice = _invoiceEntityService.GetAllByEntityId(id, invoiceId);
            //if (invoice.Count > 0)
            //{
            //    foreach (var inv in invoice)
            //    {
            //        details.AddRange(inv.InvoiceDetails);
            //    }
            //    creditamount = isMulticurrency == true ? details.Sum(c => c.DocTotalAmount) : details.Sum(x => x.DocAmount);
            //}
            //creditLimitAmount = creditLimitAmount - creditamount;
            //return creditLimitAmount;

            List<CreditNoteApplication> lstCNApplication = new List<CreditNoteApplication>();
            var lstInvoice = _invoiceEntityService.GetAllByEntityId(id, invoiceId);
            var lstDebitNote = _debitNoteService.GetDebitNoteByEntity(id);
            var lstCN = _invoiceEntityService.GetAllCnByEntityId(id);
            var lstreceipt = _receiptService.GetAllReceiptByEntity(id);
            decimal? invoiceAmount = 0, debitAmount = 0, receiptAmount = 0, creditAmount = 0;
            if (lstInvoice.Any())
                invoiceAmount = lstInvoice.Sum(x => x.GrandTotal);
            if (lstCN.Any())
            {
                foreach (var creditNote in lstCN)
                {
                    var creditNoteApplication = _creditNoteApplicationService.GetCNAppById(creditNote.Id);
                    if (creditNoteApplication != null)
                        lstCNApplication.Add(creditNoteApplication);
                }
                creditAmount = lstCNApplication.Sum(c => c.CreditAmount);
            }
            if (lstDebitNote.Any())
                debitAmount = lstDebitNote.Sum(x => x.GrandTotal);
            if (lstreceipt.Any())
                receiptAmount = lstreceipt.Sum(x => x.GrandTotal);
            creditamount = (creditLimitAmount) - ((invoiceAmount + debitAmount) - (creditAmount + receiptAmount));
            return creditamount;
        }

        private string GetNewInvoiceDocumentNumber(string docType, long CompanyId, bool isInvoice)
        {
            Invoice invoice = null;
            if (isInvoice == true)
            {
                invoice = _invoiceEntityService.GetAllIvoiceByCidAndDocSubtype(docType, CompanyId, "General");
            }
            else
                invoice = _invoiceEntityService.GetAllInvoiceByDoctypeAndCompanyId(docType, CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
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

                    duplicatInvoice = _invoiceEntityService.GetAllInvovoice(strNewDocNo, docType, CompanyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }
        private string GetNewReceiptDocumentNumber(long CompanyId)
        {
            Receipt invoice = _receiptService.GetAllReceipts(CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (invoice != null)
            {
                string strOldNo = String.Empty;
                Receipt duplicatInvoice;
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

                    duplicatInvoice = _receiptService.GetDocNo(strNewDocNo, CompanyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }


        //private void FillProvisionModel(ProvisionModel provisionModel, Provision Provision)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillProvisionModel_FillCall_Request_Message);
        //        provisionModel.Id = Provision.Id;
        //        provisionModel.InvoiceId = Provision.RefDocumentId;
        //        provisionModel.CompanyId = Provision.CompanyId;
        //        provisionModel.IsAllowableDisallowable = Provision.IsAllowableDisallowable;
        //        provisionModel.IsDisAllow = Provision.IsDisAllow;
        //        provisionModel.IsNoSupportingDocument = Provision.IsNoSupportingDocument;
        //        provisionModel.ModifiedBy = Provision.ModifiedBy;
        //        provisionModel.ModifiedDate = Provision.ModifiedDate;
        //        provisionModel.NoSupportingDocument = Provision.NoSupportingDocument;
        //        provisionModel.Provisionamount = Provision.Provisionamount;
        //        provisionModel.Remarks = Provision.Remarks;
        //        provisionModel.Status = Provision.Status;
        //        provisionModel.SystemRefNo = Provision.SystemRefNo;
        //        provisionModel.UserCreated = Provision.UserCreated;
        //        provisionModel.CreatedDate = Provision.CreatedDate;
        //        provisionModel.Currency = Provision.Currency;

        //        provisionModel.DocNo = Provision.DocNo;
        //        provisionModel.DocumentDate = Provision.DocumentDate;
        //        provisionModel.DocumentType = Provision.DocumentType;
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillProvisionModel_FillCall_SuccessFully_Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_FillProvisionModel_FillCall_Exception_Message);
        //        //Log.Logger.ZCritical(ex.StackTrace);
        //        //throw ex;

        //        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
        //        throw ex;
        //    }

        //}

        //private string GetNewProvisionDocumentNumber(long CompanyId)
        //{
        //    Provision provision = _provisionService.GetProvision(CompanyId, DocTypeConstants.Invoice);
        //    string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
        //    try
        //    {

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

        //                duplicatInvoice = _provisionService.GetProvisionByDocNoAndCompanyId(strNewDocNo, CompanyId);
        //            } while (duplicatInvoice != null);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
        //        throw ex;
        //    }
        //    return strNewDocNo;
        //}

        private void FillCreditNoteDetailModel(CreditNoteDetailModel creditNoteDetailModel, InvoiceDetail invoiceDetail)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillCreditNoteDetailModel_FillCall_Request_Message);
                //creditNoteDetailModel.AccountName = invoiceDetail.AccountName;
                creditNoteDetailModel.AmtCurrency = invoiceDetail.AmtCurrency;
                creditNoteDetailModel.BaseAmount = invoiceDetail.BaseAmount;
                creditNoteDetailModel.BaseTaxAmount = invoiceDetail.BaseTaxAmount;
                creditNoteDetailModel.BaseTotalAmount = invoiceDetail.BaseTotalAmount;

                creditNoteDetailModel.COAId = invoiceDetail.COAId;
                creditNoteDetailModel.Discount = invoiceDetail.Discount;
                creditNoteDetailModel.DiscountType = invoiceDetail.DiscountType;
                creditNoteDetailModel.DocAmount = invoiceDetail.DocAmount;
                creditNoteDetailModel.DocTaxAmount = invoiceDetail.DocTaxAmount;
                creditNoteDetailModel.DocTotalAmount = invoiceDetail.DocTotalAmount;

                creditNoteDetailModel.Item = null;
                creditNoteDetailModel.ItemCode = invoiceDetail.ItemCode;
                creditNoteDetailModel.ItemDescription = invoiceDetail.ItemDescription;
                if (invoiceDetail.ItemId != null)
                    creditNoteDetailModel.ItemId = invoiceDetail.ItemId.Value;
                creditNoteDetailModel.Qty = invoiceDetail.Qty;
                creditNoteDetailModel.Remarks = invoiceDetail.Remarks;
                creditNoteDetailModel.TaxId = invoiceDetail.TaxId;

                //creditNoteDetailModel.TaxCurrency = invoiceDetail.TaxCurrency;
                if (invoiceDetail.TaxId != null)
                {
                    TaxCode tax = _taxCodeService.GetTaxCode(creditNoteDetailModel.TaxId.Value);
                    creditNoteDetailModel.TaxIdCode = tax.Code;

                    creditNoteDetailModel.TaxRate = tax.TaxRate;
                    creditNoteDetailModel.TaxType = tax.TaxType;
                }

                creditNoteDetailModel.Unit = invoiceDetail.Unit;
                creditNoteDetailModel.UnitPrice = invoiceDetail.UnitPrice;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillCreditNoteDetailModel_FillCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_FillCreditNoteDetailModel_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private bool IsDocumentNumberExists(string DocType, string DocNo, Guid id, long companyId)
        {
            string DocumentVoidState = "";
            switch (DocType)
            {
                case DocTypeConstants.CreditNote:
                    DocumentVoidState = CreditNoteState.Void;
                    break;
                case DocTypeConstants.DoubtFulDebitNote:
                    DocumentVoidState = DoubtfulDebtState.Void;
                    break;

            }
            Invoice doc = _invoiceEntityService.GetAllInvoice(id, DocType, DocNo, companyId, DocumentVoidState);
            return doc != null;
        }
        string value = "";
        public string GenerateAutoNumberForType(long CompanyId, string Type, string companyCode)
        {
            AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(CompanyId, Type);
            string generatedAutoNumber = "";

            if (Type == "Invoice" || /*Type == DocTypeConstants.DoubtFulDebitNote*/Type == "Debt Provision" || Type == DocTypeConstants.CreditNote || Type == DocTypeConstants.Provision)
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
                    AppsWorld.InvoiceModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AppsWorld.InvoiceModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.InvoiceModule.Entities.AutoNumberCompany();
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
            List<Invoice> lstInvoices = null;
            bool ifMonthcontains = false;
            int currentMonth = 0;
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
                ifMonthcontains = true;
            }
            else if (companyFormatHere.Contains("{COMPANYCODE}"))
            {
                companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
            }
            double val = 0;
            if (Type == DocTypeConstants.Invoice)
            {
                lstInvoices = _invoiceEntityService.GetCompanyIdAndDocType(companyId);

                if (lstInvoices.Any() && ifMonthcontains)
                {
                    AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (currentMonth == lastInvCreatedMonth)
                        {
                            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                        }
                        else
                        { val = 1; }
                    }
                    else
                        val = 1;
                }
                else
                {
                    if (lstInvoices.Any())
                    {
                        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        #region Commented Code
                        //foreach (var invoice in lstInvoices)
                        //{
                        //    if (invoice.InvoiceNumber != autonumber.Preview)
                        //        val = Convert.ToInt32(IncreamentVal);
                        //    else
                        //    {
                        //        val = Convert.ToInt32(IncreamentVal) + 1;
                        //        break;
                        //    }
                        //}
                        #endregion

                        val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                    }
                    else
                    {
                        val = Convert.ToInt32(IncreamentVal);
                    }
                }
            }
            else if (Type == /*DocTypeConstants.DoubtFulDebitNote*/"Debt Provision")
            {
                lstInvoices = _invoiceEntityService.GetCompanyIdByDoubtFulDbt(companyId);

                if (lstInvoices.Any() && ifMonthcontains)
                {
                    AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (currentMonth == lastInvCreatedMonth)
                        {
                            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                        }
                        else
                        {
                            val = 1;
                        }
                    }
                    else
                    { val = 1; }

                }
                else
                {
                    if (lstInvoices.Any())
                    {
                        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        #region Commented Code
                        //foreach (var invoice in lstInvoices)
                        //{
                        //    if (invoice.InvoiceNumber != autonumber.Preview)
                        //        val = Convert.ToInt32(IncreamentVal);
                        //    else
                        //    {
                        //        val = Convert.ToInt32(IncreamentVal) + 1;
                        //        break;
                        //    }
                        //}
                        #endregion

                        val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                    }
                    else
                    {
                        val = Convert.ToInt32(IncreamentVal);
                    }
                }

            }
            else if (Type == DocTypeConstants.CreditNote)
            {
                lstInvoices = _invoiceEntityService.GetCompanyIdByCreditNote(companyId);

                if (lstInvoices.Any() && ifMonthcontains)
                {
                    AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);

                    int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (currentMonth == lastInvCreatedMonth)
                        {
                            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                        }
                        else
                        {
                            val = 1;
                        }
                    }
                    else
                        val = 1;
                }
                else
                {
                    if (lstInvoices.Any())
                    {
                        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        #region commented Code
                        //foreach (var invoice in lstInvoices)
                        //{
                        //    if (invoice.InvoiceNumber != autonumber.Preview)
                        //        val = Convert.ToInt32(IncreamentVal);
                        //    else
                        //    {
                        //        val = Convert.ToInt32(IncreamentVal) + 1;
                        //        break;
                        //    }
                        //}
                        #endregion

                        val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                    }
                    else
                    {
                        val = Convert.ToInt32(IncreamentVal);
                    }
                }
            }
            //else if (Type == DocTypeConstants.Provision)
            //{
            //    List<Provision> lstProvisions = _provisionService.lstInvoiceProvision(companyId);
            //    if (lstProvisions.Any())
            //    {
            //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
            //        foreach (var provision in lstProvisions)
            //        {
            //            if (provision.SystemRefNo != autonumber.Preview)
            //                val = Convert.ToInt32(IncreamentVal);
            //            else
            //            {
            //                val = Convert.ToInt32(IncreamentVal) + 1;
            //                break;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        val = Convert.ToInt32(IncreamentVal);
            //    }
            //}
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

            if (lstInvoices.Any())
            {
                OutputNumber = GetNewNumber(lstInvoices, Type, OutputNumber, counter, companyFormatHere, counterLength);
            }

            return OutputNumber;
        }

        private static double GetValue(string IncreamentVal, List<Invoice> lstInvoices, double val, Entities.AutoNumber autonumber)
        {
            foreach (var invoice in lstInvoices)
            {
                if (invoice.InvoiceNumber != autonumber.Preview)
                    //val = Convert.ToInt32(IncreamentVal);
                    val = 1;
                else
                {
                    val = Convert.ToInt32(IncreamentVal) + 1;

                    break;
                }
            }

            return val;
        }

        private string GetNewNumber(List<Invoice> lstInvoice, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstInvoice.Where(a => a.InvoiceNumber == outputNumber).FirstOrDefault();
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
                    var inv = lstInvoice.Where(c => c.InvoiceNumber == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }

        private void InsertInvoice(InvoiceModel TObject, Invoice invoiceNew)
        {
            try
            {
                invoiceNew.CompanyId = TObject.CompanyId;
                invoiceNew.DocType = DocTypeConstants.Invoice;
                invoiceNew.EntityType = "Customer";
                invoiceNew.DocDate = TObject.DocDate.Date;
                invoiceNew.DueDate = TObject.DueDate.Value.Date;
                invoiceNew.PONo = TObject.PONo;
                invoiceNew.EntityId = TObject.EntityId;
                invoiceNew.CreditTermsId = TObject.CreditTermsId;
                invoiceNew.Nature = TObject.Nature;
                invoiceNew.ServiceCompanyId = TObject.ServiceCompanyId;
                //FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(invoiceNew.CompanyId);
                if (TObject.IsMultiCurrency)
                    invoiceNew.DocCurrency = TObject.DocCurrency;
                else
                    invoiceNew.DocCurrency = TObject.BaseCurrency;
                invoiceNew.IsMultiCurrency = TObject.IsMultiCurrency;
                invoiceNew.ExCurrency = TObject.BaseCurrency;
                invoiceNew.ExchangeRate = TObject.ExchangeRate;
                invoiceNew.ExDurationFrom = TObject.ExDurationFrom;
                invoiceNew.ExDurationTo = TObject.ExDurationTo;
                invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                invoiceNew.DocDescription = TObject.DocDescription;
                invoiceNew.IsGstSettings = TObject.IsGstSettings;
                invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                if (TObject.IsGstSettings)
                {
                    invoiceNew.GSTExchangeRate = TObject.GSTExchangeRate;
                    invoiceNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    invoiceNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }

                invoiceNew.IsRepeatingInvoice = TObject.IsRepeatingInvoice;
                if (TObject.IsRepeatingInvoice)
                {
                    invoiceNew.RepEveryPeriodNo = TObject.RepEveryPeriodNo;
                    invoiceNew.RepEveryPeriod = TObject.RepEveryPeriod;
                    if (TObject.EndDate == null)
                        invoiceNew.RepEndDate = null;
                    else
                        invoiceNew.RepEndDate = TObject.EndDate.Value.Date;
                    invoiceNew.ParentInvoiceID = TObject.ParentInvoiceID;
                }
                else
                {
                    invoiceNew.RepEveryPeriodNo = null;
                    invoiceNew.RepEveryPeriod = null;
                    invoiceNew.RepEndDate = null;
                    invoiceNew.ParentInvoiceID = null;
                }

                //invoiceNew.BalanceAmount = TObject.GrandTotal;
                invoiceNew.GrandTotal = TObject.GrandTotal;
                invoiceNew.GSTTotalAmount = TObject.GSTTotalAmount;

                invoiceNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;

                invoiceNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                invoiceNew.NoSupportingDocs = TObject.NoSupportingDocument;

                #region commented_code
                //invoiceNew.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, TObject.CompanyId);
                //if (invoiceNew.IsSegmentReporting)
                //{

                //invoiceNew.SegmentMasterid1 = TObject.SegmentMasterid1;
                //invoiceNew.SegmentDetailid1 = TObject.SegmentDetailid1;
                //invoiceNew.SegmentCategory1 = TObject.SegmentCategory1;

                //invoiceNew.SegmentMasterid2 = TObject.SegmentMasterid2;
                //invoiceNew.SegmentDetailid2 = TObject.SegmentDetailid2;
                //invoiceNew.SegmentCategory2 = TObject.SegmentCategory2;

                //}
                //else
                //{
                //    invoiceNew.SegmentCategory1 = null;
                //    invoiceNew.SegmentCategory2 = null;
                //    invoiceNew.SegmentMasterid1 = null;
                //    invoiceNew.SegmentMasterid2 = null;
                //    invoiceNew.SegmentDetailid1 = null;
                //    invoiceNew.SegmentDetailid2 = null;
                //}
                #endregion

                invoiceNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                invoiceNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                //invoiceNew.Remarks = TObject.Remarks;
                invoiceNew.IsSegmentReporting = TObject.IsSegmentReporting;
                invoiceNew.Status = TObject.Status;
                invoiceNew.InvoiceNumber = TObject.InvoiceNumber;
                //invoiceNew.DocumentState = invoiceNew.BalanceAmount == 0 ? InvoiceState.FullyPaid : String.IsNullOrEmpty(TObject.DocumentState) ? InvoiceState.NotPaid : TObject.DocumentState;
                invoiceNew.IsWorkFlowInvoice = TObject.IsWorkFlowInvoice;
                invoiceNew.CursorType = TObject.CursorType;
                invoiceNew.DocumentId = TObject.DocumentId;
                invoiceNew.IsOBInvoice = TObject.IsOBInvoice;
                invoiceNew.OpeningBalanceId = TObject.OpeningBalanceId;
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }
        public void RecurringModel(RecurringModel invoiceNew, RecurringModel TObject)
        {
            try
            {
                invoiceNew.CompanyId = TObject.CompanyId;
                invoiceNew.DocType = DocTypeConstants.Invoice;
                //invoiceNew.DocSubType = "Invoice";
                invoiceNew.EntityType = "Customer";
                invoiceNew.DocDate = TObject.DocDate.Date;
                invoiceNew.DueDate = TObject.DueDate.Value.Date;
                invoiceNew.PONo = TObject.PONo;
                invoiceNew.EntityId = TObject.EntityId;
                invoiceNew.CreditTermsId = TObject.CreditTermsId;
                invoiceNew.Nature = TObject.Nature;
                invoiceNew.ServiceCompanyId = TObject.ServiceCompanyId;
                if (TObject.IsMultiCurrency == true)
                    invoiceNew.DocCurrency = TObject.DocCurrency;
                else
                    invoiceNew.DocCurrency = TObject.BaseCurrency;
                invoiceNew.IsMultiCurrency = TObject.IsMultiCurrency;
                invoiceNew.BaseCurrency = TObject.BaseCurrency;
                invoiceNew.ExchangeRate = TObject.ExchangeRate;
                invoiceNew.ExDurationFrom = TObject.ExDurationFrom;
                invoiceNew.ExDurationTo = TObject.ExDurationTo;
                invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                invoiceNew.DocDescription = TObject.DocDescription;
                invoiceNew.IsGstSettings = TObject.IsGstSettings;
                invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                if (TObject.IsGstSettings)
                {
                    invoiceNew.GSTExchangeRate = TObject.GSTExchangeRate;
                    invoiceNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    invoiceNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                else
                {
                    invoiceNew.RepEveryPeriodNo = null;
                    invoiceNew.RepEveryPeriod = null;
                    invoiceNew.ParentInvoiceID = null;
                }

                invoiceNew.BalanceAmount = TObject.GrandTotal;
                invoiceNew.GrandTotal = TObject.GrandTotal;
                invoiceNew.GSTTotalAmount = TObject.GSTTotalAmount;
                invoiceNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;
                invoiceNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                invoiceNew.NoSupportingDocument = TObject.NoSupportingDocument;
                invoiceNew.SegmentMasterid1 = TObject.SegmentMasterid1;
                invoiceNew.SegmentDetailid1 = TObject.SegmentDetailid1;
                invoiceNew.SegmentCategory1 = TObject.SegmentCategory1;
                invoiceNew.SegmentMasterid2 = TObject.SegmentMasterid2;
                invoiceNew.SegmentDetailid2 = TObject.SegmentDetailid2;
                invoiceNew.SegmentCategory2 = TObject.SegmentCategory2;
                invoiceNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                invoiceNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                invoiceNew.IsSegmentReporting = TObject.IsSegmentReporting;
                invoiceNew.Status = TObject.Status;
                invoiceNew.InvoiceNumber = TObject.InvoiceNumber;
                invoiceNew.DocumentState = InvoiceState.NotPaid;
                invoiceNew.CursorType = TObject.CursorType;
                invoiceNew.DocumentId = TObject.DocumentId;
                invoiceNew.InternalState = InvoiceState.Posted;
                invoiceNew.InvoiceDetailModels = TObject.InvoiceDetailModels.Select(c => new InvoiceDetailModel()
                {
                    Id = Guid.NewGuid(),
                    InvoiceId = invoiceNew.Id,
                    AllowDisAllow = c.AllowDisAllow,
                    COAId = c.COAId,
                    BaseAmount = c.BaseAmount,
                    BaseTaxAmount = c.BaseTaxAmount,
                    BaseTotalAmount = c.BaseTotalAmount,
                    DocAmount = c.DocAmount,
                    DocTaxAmount = c.DocTaxAmount,
                    DocTotalAmount = c.DocTotalAmount,
                    Discount = c.Discount,
                    DiscountType = c.DiscountType,
                    ItemCode = c.ItemCode,
                    ItemDescription = c.ItemDescription,
                    ItemId = c.ItemId,
                    AmtCurrency = c.AmtCurrency,
                    TaxIdCode = c.TaxIdCode,
                    TaxId = c.TaxId,
                    TaxCurrency = c.TaxCurrency,
                    TaxRate = c.TaxRate,
                    Qty = c.Qty,
                    RecOrder = c.RecOrder,
                    Unit = c.Unit,
                    UnitPrice = c.UnitPrice,
                    IsPLAccount = c.IsPLAccount
                }).ToList();
                if (TObject.IsGstSettings)
                {
                    invoiceNew.InvoiceGSTDetailModels = TObject.InvoiceGSTDetailModels.Select(c => new InvoiceGSTDetailModel()
                    {

                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw;
            }
        }
        public void UpdateInvoiceDetails(InvoiceModel TObject, Invoice _invoiceNew)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Request_Message);
                if (TObject.InvoiceDetails != null)
                {
                    foreach (InvoiceDetail detail in TObject.InvoiceDetails)
                    {
                        if (detail.RecordStatus == "Added")
                        {
                            detail.RecOrder = recorder + 1;
                            recorder = detail.RecOrder;
                            detail.InvoiceId = _invoiceNew.Id;
                            detail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
                            decimal? amount = detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocTaxAmount;
                            detail.BaseTaxAmount = TObject.ExchangeRate != null ? amount : detail.DocTaxAmount;
                            detail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                            detail.ObjectState = ObjectState.Added;
                            _invoiceNew.InvoiceDetails.Add(detail);
                        }
                        else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                        {
                            InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.FirstOrDefault(a => a.Id == detail.Id);
                            if (invoiceDetail != null)
                            {
                                invoiceDetail.InvoiceId = _invoiceNew.Id;
                                invoiceDetail.ItemId = detail.ItemId;
                                invoiceDetail.ItemCode = detail.ItemCode;
                                invoiceDetail.ItemDescription = detail.ItemDescription;
                                invoiceDetail.Qty = detail.Qty;
                                invoiceDetail.Unit = detail.Unit;
                                invoiceDetail.UnitPrice = detail.UnitPrice;
                                invoiceDetail.DiscountType = detail.DiscountType;
                                invoiceDetail.Discount = detail.Discount;
                                invoiceDetail.COAId = detail.COAId;
                                invoiceDetail.TaxId = detail.TaxId;
                                invoiceDetail.TaxRate = detail.TaxRate;
                                invoiceDetail.DocTaxAmount = detail.DocTaxAmount;
                                invoiceDetail.DocAmount = detail.DocAmount;
                                invoiceDetail.AmtCurrency = detail.AmtCurrency;
                                invoiceDetail.DocTotalAmount = detail.DocTotalAmount;
                                invoiceDetail.Remarks = detail.Remarks;
                                invoiceDetail.TaxIdCode = detail.TaxIdCode;
                                invoiceDetail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(invoiceDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocAmount;
                                decimal? amount = invoiceDetail.DocTaxAmount != null ? Math.Round((decimal)invoiceDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocTaxAmount;
                                invoiceDetail.BaseTaxAmount = TObject.ExchangeRate != null ? amount : detail.DocTaxAmount;
                                invoiceDetail.BaseTotalAmount = Math.Round((decimal)invoiceDetail.BaseAmount + (invoiceDetail.BaseTaxAmount != null ? (decimal)invoiceDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);

                                invoiceDetail.RecOrder = recorder + 1;
                                recorder = invoiceDetail.RecOrder;
                                invoiceDetail.ObjectState = ObjectState.Modified;
                            }
                        }
                        else if (detail.RecordStatus == "Deleted")
                        {
                            InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.FirstOrDefault(a => a.Id == detail.Id);
                            if (invoiceDetail != null)
                            {
                                invoiceDetail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_SuccessFully_Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw;
            }
        }
        //public void UpdateInvoiceNotes(InvoiceModel TObject, Invoice _invoiceNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Request_Message);
        //        if (TObject.InvoiceNotes != null)
        //        {
        //            foreach (InvoiceNote note in TObject.InvoiceNotes)
        //            {
        //                if (note.RecordStatus == "Added")
        //                {
        //                    note.CreatedDate = DateTime.UtcNow;
        //                    note.UserCreated = note.UserCreated;
        //                    note.ObjectState = ObjectState.Added;
        //                    _invoiceNew.InvoiceNotes.Add(note);
        //                }
        //                else if (note.RecordStatus != "Added" && note.RecordStatus != "Deleted")
        //                {
        //                    InvoiceNote invoiceNote = _invoiceNew.InvoiceNotes.Where(a => a.Id == note.Id).FirstOrDefault();
        //                    if (invoiceNote != null)
        //                    {
        //                        invoiceNote.InvoiceId = TObject.Id;
        //                        invoiceNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
        //                        invoiceNote.Notes = note.Notes;
        //                        invoiceNote.ModifiedDate = DateTime.UtcNow;
        //                        invoiceNote.ModifiedBy = note.ModifiedBy;
        //                        invoiceNote.ObjectState = ObjectState.Modified;
        //                    }
        //                }
        //                else if (note.RecordStatus == "Deleted")
        //                {
        //                    InvoiceNote invoiceNote = _invoiceNew.InvoiceNotes.Where(a => a.Id == note.Id).FirstOrDefault();
        //                    if (invoiceNote != null)
        //                    {
        //                        invoiceNote.ObjectState = ObjectState.Deleted;
        //                    }
        //                }
        //                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Exception_Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Exception_Message);
        //        //Log.Logger.ZCritical(ex.StackTrace);
        //        //throw ex;

        //        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
        //        throw ex;
        //    }
        //}

        //public void UpdateInvoiceGSTDetails(InvoiceModel TObject, Invoice _invoiceNew)
        //{

        //    try
        //    {
        //        if (TObject.InvoiceGSTDetails != null)
        //        {
        //            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceGSTDetails_Update_Request_Message);
        //            foreach (InvoiceGSTDetail detail in TObject.InvoiceGSTDetails)
        //            {
        //                if (detail.RecordStatus == "Added")
        //                {
        //                    detail.ObjectState = ObjectState.Added;
        //                    _invoiceNew.InvoiceGSTDetails.Add(detail);
        //                }
        //                else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
        //                {
        //                    InvoiceGSTDetail invoiceGSTDetail = _invoiceNew.InvoiceGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                    if (invoiceGSTDetail != null)
        //                    {
        //                        invoiceGSTDetail.InvoiceId = TObject.Id;
        //                        invoiceGSTDetail.TaxId = detail.TaxId;
        //                        invoiceGSTDetail.Amount = detail.Amount;
        //                        invoiceGSTDetail.TaxAmount = detail.TaxAmount;
        //                        invoiceGSTDetail.TotalAmount = detail.TotalAmount;
        //                        invoiceGSTDetail.ObjectState = ObjectState.Modified;
        //                    }
        //                }
        //                else if (detail.RecordStatus == "Deleted")
        //                {
        //                    InvoiceGSTDetail invoiceGSTDetail = _invoiceNew.InvoiceGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                    if (invoiceGSTDetail != null)
        //                    {
        //                        invoiceGSTDetail.ObjectState = ObjectState.Deleted;
        //                    }
        //                }
        //                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceGSTDetails_Update_SuccessFully_Message);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceGSTDetails_Update_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}


        private void FillCreditNote(CreditNoteModel invDTO, Invoice invoice, string username)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_FillCreditNote_FillCall_Request_Message);
                invDTO.Id = invoice.Id;
                invDTO.IsModify = invoice.ClearCount > 0;
                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = invoice.DocType;
                invDTO.InvoiceNumber = invoice.InvoiceNumber;
                invDTO.DocNo = invoice.DocNo;
                invDTO.DocDate = invoice.DocDate;
                invDTO.DueDate = invoice.DueDate;
                invDTO.EntityId = invoice.EntityId;
                invDTO.EntityName = _beanEntityService.GetEntityName(invoice.EntityId);
                invDTO.CreditTermsId = invoice.CreditTermsId;
                invDTO.CustCreditlimit = _beanEntityService.GetCteditLimitsValue(invoice.EntityId);
                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;
                invDTO.Version = "0x" + string.Concat(Array.ConvertAll(invoice.Version, x => x.ToString("X2")));
                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;

                invDTO.IsGstSettings = invoice.IsGstSettings;
                invDTO.GSTExCurrency = invoice.GSTExCurrency;
                invDTO.GSTExchangeRate = invoice.GSTExchangeRate;
                invDTO.GSTExDurationFrom = invoice.GSTExDurationFrom;
                invDTO.GSTExDurationTo = invoice.GSTExDurationTo;
                invDTO.ExternalType = invoice.ExtensionType ?? ExtensionType.Receipt;
                invDTO.DocumentId = invoice.DocumentId;
                invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                invDTO.GSTTotalAmount = invoice.GSTTotalAmount;
                invDTO.GrandTotal = invoice.GrandTotal;
                invDTO.BalanceAmount = invoice.BalanceAmount;
                invDTO.ExtensionType = ExtensionType.General;
                invDTO.SavedFrom = invoice.ExtensionType;
                invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;

                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;
                invDTO.Remarks = invoice.DocDescription;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = invoice.DocumentState;
                invDTO.ModifiedDate = invoice.ModifiedDate;
                invDTO.ModifiedBy = invoice.ModifiedBy;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;

                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;

                invDTO.InvoiceDetails = invoice.InvoiceDetails.OrderBy(c => c.RecOrder).ToList();
                invDTO.OpeningBalanceId = invoice.OpeningBalanceId;
                invDTO.IsLocked = invoice.IsLocked;
                List<CreditNoteApplication> lstApplications = _creditNoteApplicationService.Query(c => c.InvoiceId == invoice.Id).Include(c => c.CreditNoteApplicationDetails).Select().OrderByDescending(a => a.CreatedDate).ToList();
                if (lstApplications.Count > 0)
                {
                    List<long> lstCompanies = _companyService.GetAllSubCompaniesId(username, invoice.CompanyId);
                    foreach (var application in lstApplications)
                    {
                        CreditNoteApplicationModel model = new CreditNoteApplicationModel();
                        FillCreditNoteApplicationModel(model, application);
                        if (application.IsRevExcess != true)
                        {
                            foreach (var detail in application.CreditNoteApplicationDetails)
                            {
                                if (detail.ServiceEntityId != null)
                                    if (!lstCompanies.Contains((Int64)(detail.ServiceEntityId)))
                                    {
                                        model.IsVoidEnable = false;
                                        invDTO.IsVoidEnable = false;
                                    }
                            }
                        }
                        model.IsVoidEnable = model.IsVoidEnable == false ? false : true;
                        invDTO.CreditNoteApplicationModels.Add(model);
                    }
                }

                invDTO.IsVoidEnable = invDTO.IsVoidEnable == false ? false : true;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_FillCreditNote_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        private void FillCreditNoteApplicationModel(CreditNoteApplicationModel CNAModel, CreditNoteApplication CCA)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_FillCreditNoteApplicationModel_FillCall_Request_Message);
                CNAModel.Id = CCA.Id;
                CNAModel.InvoiceId = CCA.InvoiceId;
                CNAModel.CompanyId = CCA.CompanyId;
                CNAModel.IsModify = CCA.ClearCount > 0;
                CNAModel.IsLocked = CCA.IsLocked;
                var invoice = _invoiceEntityService.Query(c => c.Id == CCA.InvoiceId).Select().FirstOrDefault();
                if (invoice != null)
                {
                    CNAModel.DocNo = invoice.DocNo;
                    CNAModel.DocCurrency = invoice.DocCurrency;
                    CNAModel.CreditNoteAmount = invoice.GrandTotal;
                    CNAModel.ExchangeRate = invoice.ExchangeRate;
                    CNAModel.GSTExchangeRate = invoice.GSTExchangeRate;
                    if (CCA.Status == CreditNoteApplicationStatus.Void)
                        CNAModel.CreditNoteBalanceAmount = CNAModel.CreditNoteAmount;
                    else
                        CNAModel.CreditNoteBalanceAmount = invoice.BalanceAmount + CCA.CreditAmount;
                }
                CNAModel.CreditNoteApplicationNumber = CCA.CreditNoteApplicationNumber;
                CNAModel.Version = "0x" + string.Concat(Array.ConvertAll(CCA.Version, x => x.ToString("X2")));
                CNAModel.CreditAmount = CCA.CreditAmount;
                CNAModel.IsNoSupportingDocument = CCA.IsNoSupportingDocument;
                CNAModel.NoSupportingDocument = CCA.IsNoSupportingDocumentActivated;
                CNAModel.CreditNoteApplicationDate = CCA.CreditNoteApplicationDate;
                CNAModel.CreditNoteApplicationResetDate = CCA.CreditNoteApplicationResetDate;
                CNAModel.Remarks = CCA.Remarks;
                CNAModel.DocumentId = CCA.DocumentId;
                CNAModel.CreatedDate = CCA.CreatedDate;
                CNAModel.UserCreated = CCA.UserCreated;
                CNAModel.Status = CCA.Status;
                CNAModel.ModifiedBy = CCA.ModifiedBy;
                CNAModel.ModifiedDate = CCA.ModifiedDate;
                CNAModel.IsRevExcess = CCA.IsRevExcess;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_FillCreditNoteApplicationModel_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_FillCreditNoteApplicationModel_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        private void InsertCreditNote(CreditNoteModel TObject, Invoice invoiceNew, bool isNew)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_InsertCreditNote_FillCall_Request_Message);
                invoiceNew.CompanyId = TObject.CompanyId;
                invoiceNew.Id = TObject.Id;
                invoiceNew.DocType = DocTypeConstants.CreditNote;
                invoiceNew.DocSubType = TObject.ExternalType == "Receipt" ? "Excess" : "General";
                invoiceNew.EntityType = "Customer";
                invoiceNew.DocDate = TObject.DocDate.Date;
                invoiceNew.DueDate = TObject.DocDate.Date;
                invoiceNew.DocNo = TObject.DocNo;
                invoiceNew.EntityId = TObject.EntityId;
                invoiceNew.DocumentId = TObject.DocumentId;
                invoiceNew.CreditTermsId = TObject.CreditTermsId;
                invoiceNew.Nature = TObject.Nature;
                invoiceNew.ServiceCompanyId = TObject.ServiceCompanyId;
                invoiceNew.DocCurrency = TObject.DocCurrency;
                invoiceNew.IsMultiCurrency = TObject.IsMultiCurrency;
                invoiceNew.ExCurrency = TObject.BaseCurrency;
                invoiceNew.ExchangeRate = TObject.ExchangeRate;
                invoiceNew.ExDurationFrom = TObject.ExDurationFrom;
                invoiceNew.ExDurationTo = TObject.ExDurationTo;
                invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                invoiceNew.InvoiceNumber = invoiceNew.DocNo;

                if (isNew)
                    invoiceNew.ExtensionType = TObject.ExtensionType;
                invoiceNew.IsGstSettings = TObject.IsGstSettings;
                invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                if (TObject.IsGstSettings)
                {

                    invoiceNew.GSTExchangeRate = TObject.GSTExchangeRate;
                    invoiceNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    invoiceNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                decimal Remaining = 0;
                if (!isNew)
                {
                    if (invoiceNew.GrandTotal != TObject.GrandTotal)
                    {
                        if (invoiceNew.GrandTotal > TObject.GrandTotal)
                        {

                            Remaining = invoiceNew.GrandTotal - TObject.GrandTotal;
                            invoiceNew.BalanceAmount = invoiceNew.BalanceAmount - Remaining;
                        }
                        else if (invoiceNew.GrandTotal < TObject.GrandTotal)
                        {
                            Remaining = TObject.GrandTotal - invoiceNew.GrandTotal;
                            invoiceNew.BalanceAmount = invoiceNew.BalanceAmount + Remaining;
                        }
                    }
                }
                invoiceNew.GrandTotal = TObject.GrandTotal;

                if (isNew)
                    invoiceNew.BalanceAmount = TObject.GrandTotal;
                invoiceNew.GSTTotalAmount = TObject.GSTTotalAmount;
                invoiceNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;
                invoiceNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                if (invoiceNew.IsNoSupportingDocument != null)
                    invoiceNew.NoSupportingDocs = invoiceNew.IsNoSupportingDocument.Value ? TObject.NoSupportingDocument : null;
                invoiceNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                invoiceNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                invoiceNew.DocDescription = TObject.Remarks;
                invoiceNew.IsGstSettings = TObject.IsGstSettings;
                invoiceNew.IsSegmentReporting = TObject.IsSegmentReporting;
                invoiceNew.Status = TObject.Status;

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_InsertCreditNote_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void UpdateCreditNoteDetails(CreditNoteModel TObject, Invoice _invoiceNew)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteDetails_Update_Request_Message);
                foreach (InvoiceDetail detail in TObject.InvoiceDetails)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        detail.RecOrder = recorder + 1;
                        recorder = detail.RecOrder;
                        detail.Id = Guid.NewGuid();
                        detail.TaxIdCode = detail.TaxIdCode;
                        detail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
                        detail.BaseTaxAmount = TObject.ExchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocTaxAmount : detail.DocTaxAmount;
                        detail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                        detail.ObjectState = ObjectState.Added;
                        _invoiceNew.InvoiceDetails.Add(detail);
                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (invoiceDetail != null)
                        {
                            invoiceDetail.InvoiceId = TObject.Id;
                            invoiceDetail.ItemId = detail.ItemId;
                            invoiceDetail.ItemCode = detail.ItemCode;
                            invoiceDetail.ItemDescription = detail.ItemDescription;
                            invoiceDetail.Qty = detail.Qty;
                            invoiceDetail.Unit = detail.Unit;
                            invoiceDetail.UnitPrice = detail.UnitPrice;
                            invoiceDetail.DiscountType = detail.DiscountType;
                            invoiceDetail.Discount = detail.Discount;
                            invoiceDetail.COAId = detail.COAId;
                            invoiceDetail.TaxId = detail.TaxId;
                            invoiceDetail.TaxRate = detail.TaxRate;
                            invoiceDetail.DocTaxAmount = detail.DocTaxAmount;
                            invoiceDetail.DocAmount = detail.DocAmount;
                            invoiceDetail.AmtCurrency = detail.AmtCurrency;
                            invoiceDetail.DocTotalAmount = detail.DocTotalAmount;
                            invoiceDetail.Remarks = detail.Remarks;
                            invoiceDetail.TaxIdCode = detail.TaxIdCode;
                            invoiceDetail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocAmount;
                            invoiceDetail.BaseTaxAmount = TObject.ExchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocTaxAmount : detail.DocTaxAmount;
                            invoiceDetail.BaseTotalAmount = Math.Round((decimal)invoiceDetail.BaseAmount + (invoiceDetail.BaseTaxAmount != null ? (decimal)invoiceDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                            invoiceDetail.RecOrder = recorder + 1;
                            recorder = invoiceDetail.RecOrder;
                            _invoiceDetailService.Update(invoiceDetail);
                            invoiceDetail.ObjectState = ObjectState.Modified;
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (invoiceDetail != null)
                        {
                            invoiceDetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteDetails_Update_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        //private void UpdateCreditNoteGSTDetails(CreditNoteModel TObject, Invoice _invoiceNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_UpdateCreditNoteGSTDetails_Update_Request_Message);
        //        if (TObject.InvoiceGSTDetails != null)
        //        {
        //            foreach (InvoiceGSTDetail detail in TObject.InvoiceGSTDetails)
        //            {
        //                if (detail.RecordStatus == "Added")
        //                {
        //                    detail.ObjectState = ObjectState.Added;
        //                    _invoiceNew.InvoiceGSTDetails.Add(detail);
        //                }
        //                else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
        //                {
        //                    InvoiceGSTDetail invoiceGSTDetail =
        //                        _invoiceNew.InvoiceGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                    if (invoiceGSTDetail != null)
        //                    {
        //                        invoiceGSTDetail.InvoiceId = TObject.Id;
        //                        invoiceGSTDetail.TaxId = detail.TaxId;
        //                        invoiceGSTDetail.Amount = detail.Amount;
        //                        invoiceGSTDetail.TaxAmount = detail.TaxAmount;
        //                        invoiceGSTDetail.TotalAmount = detail.TotalAmount;

        //                        invoiceGSTDetail.ObjectState = ObjectState.Modified;
        //                    }
        //                }
        //                else if (detail.RecordStatus == "Deleted")
        //                {
        //                    InvoiceGSTDetail invoiceGSTDetail =
        //                        _invoiceNew.InvoiceGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                    if (invoiceGSTDetail != null)
        //                    {
        //                        invoiceGSTDetail.ObjectState = ObjectState.Deleted;
        //                    }
        //                }
        //            }
        //        }
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_UpdateCreditNoteGSTDetails_Update_SuccessFully_Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_UpdateCreditNoteGSTDetails_Update_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }

        //}

        private void ValidateCreditNoteApplication(Invoice creditNote, CreditNoteApplicationModel TObject)
        {
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            if (TObject.CreditNoteApplicationDate == null)
            {
                throw new Exception(DoubtfulDebitValidation.Invalid_Application_Date);
            }

            if ((TObject.CreditNoteApplicationDetailModels == null || TObject.CreditNoteApplicationDetailModels.Count == 0) && (TObject.ReverseExcessModels == null || TObject.ReverseExcessModels.Count == 0))
            {
                throw new Exception(DoubtfulDebitValidation.Atleast_one_Application_is_required);
            }
            else
            {
                if (TObject.IsRevExcess != true)
                {
                    int itemCount = TObject.CreditNoteApplicationDetailModels.Where(a => a.CreditAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(DoubtfulDebitValidation.Total_Amount_To_Credit_should_be_greater_than_Zero);
                    }
                }
                else
                {
                    int itemCount = TObject.ReverseExcessModels.Where(a => a.DocAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(DoubtfulDebitValidation.Total_Amount_To_Credit_should_be_greater_than_Zero);
                    }
                }
            }

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(TObject.CreditNoteApplicationDate, TObject.CompanyId))
            {
                throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.CreditNoteApplicationDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.CreditNoteApplicationDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
            }
            string DocNumbers = "";
            List<Guid> lstDocIds = TObject.CreditNoteApplicationDetailModels.Where(a => a.DocType == DocTypeConstants.Invoice).Select(a => a.DocumentId).ToList();
            List<Invoice> lstTaggedInvoices = new List<Invoice>();
            if (lstDocIds.Count > 0)
            {
                lstTaggedInvoices = GetTaggedInvoicesByCustomerAndCurrency(creditNote.EntityId, creditNote.DocCurrency, creditNote.CompanyId).Where(a => lstDocIds.Contains(a.Id)).ToList();
                if (lstTaggedInvoices.Count > 0)
                {
                    foreach (Invoice v in lstTaggedInvoices)
                        DocNumbers += v.InvoiceNumber + ",";
                }
            }
            lstDocIds = TObject.CreditNoteApplicationDetailModels.Where(a => a.DocType == DocTypeConstants.DebitNote).Select(a => a.DocumentId).ToList();
            List<DebitNote> lstTaggedDebitNotes = new List<DebitNote>();
            if (lstDocIds.Count > 0)
            {
                lstTaggedDebitNotes = _debitNoteService.GetTaggedDebitNotesByCustomerAndCurrency(creditNote.EntityId, creditNote.DocCurrency, creditNote.CompanyId).Where(a => lstDocIds.Contains(a.Id)).ToList();
                if (lstTaggedDebitNotes.Count > 0)
                {
                    foreach (DebitNote v in lstTaggedDebitNotes)
                        DocNumbers += v.DebitNoteNumber + ",";
                }
            }
            //Verify if any of the application have amount
            if (TObject.IsRevExcess != true)
            {
                var amountDocuments = TObject.CreditNoteApplicationDetailModels.Where(a => a.CreditAmount > 0).ToList();
                if (amountDocuments.Count == 0)
                    throw new Exception(DoubtfulDebitValidation.Atleast_one_Application_should_be_given);
                //Verify Duplication Documents in details
                var duplicateDocuments = TObject.CreditNoteApplicationDetailModels.GroupBy(x => x.DocumentId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (duplicateDocuments.Count > 0)
                    throw new Exception(DoubtfulDebitValidation.Duplicate_documents_in_details);
            }
        }

        public void UpdateCreditNoteApplicationDetails(CreditNoteApplicationModel model, CreditNoteApplication cnApplication, string ConnectionString, Guid transationId, List<DocumentHistoryModel> lstDocuments, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteApplicationDetails_Update_Request_Message);
                List<CreditNoteApplicationDetail> lstDetails = cnApplication.CreditNoteApplicationDetails.Where(a => !model.CreditNoteApplicationDetailModels.Any(b => b.Id == a.Id)).ToList();

                foreach (CreditNoteApplicationDetail detailDelete in lstDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;

                decimal? roundingAmount = 0;

                foreach (CreditNoteApplicationDetailModel detailModel in model.CreditNoteApplicationDetailModels)
                {
                    CreditNoteApplicationDetail detail = cnApplication.CreditNoteApplicationDetails.Where(a => a.Id == detailModel.Id).FirstOrDefault();
                    roundingAmount = 0;
                    if (detail == null)
                    {
                        if (detailModel.CreditAmount != null || detailModel.CreditAmount != 0)
                        {
                            detail = new CreditNoteApplicationDetail();
                            detail.Id = Guid.NewGuid();
                            detail.CreditNoteApplicationId = model.Id;
                            detail.DocumentId = detailModel.DocumentId;
                            detail.DocumentType = detailModel.DocType;
                            detail.DocCurrency = detailModel.DocCurrency;
                            detail.CreditAmount = detailModel.CreditAmount;
                            detail.BaseCurrencyExchangeRate = Convert.ToDecimal(detailModel.BaseCurrencyExchangeRate);
                            UpdateDocumentState(detail.DocumentId, detail.DocumentType, detail.CreditAmount, ConnectionString, transationId, cnApplication.CreditNoteApplicationDate, lstDocuments, lstOfRoundingAmount, out roundingAmount);
                            detail.RoundingAmount = roundingAmount;
                            detail.ObjectState = ObjectState.Added;
                            cnApplication.CreditNoteApplicationDetails.Add(detail);
                        }
                    }
                    else
                    {
                        UpdateDocumentState(detail.DocumentId, detail.DocumentType, detailModel.CreditAmount - detail.CreditAmount, ConnectionString, transationId, cnApplication.CreditNoteApplicationDate, lstDocuments, lstOfRoundingAmount, out roundingAmount);
                        if (detailModel.CreditAmount == null || detailModel.CreditAmount == 0)
                        {
                            detail.ObjectState = ObjectState.Deleted;
                        }
                        else
                        {
                            detail.RoundingAmount = roundingAmount;
                            detail.CreditAmount = detailModel.CreditAmount;
                            detail.ObjectState = ObjectState.Modified;
                        }
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteApplicationDetails_Update_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        private void UpdateCreditNoteApplicationRevExcessDetails(CreditNoteApplicationModel TObject, CreditNoteApplication cnApplication)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteDetails_Update_Request_Message);

                #region CNApplication_ReverseExcess_Check_UnCheck_DataUpdation_Invoice_DN

                if (cnApplication.CreditNoteApplicationDetails.Any())
                {
                    List<Guid> invoiceIds = cnApplication.CreditNoteApplicationDetails.Where(d => d.DocumentType == DocTypeConstants.Invoice).Select(c => c.DocumentId).ToList();
                    List<Guid> debitNoteIds = cnApplication.CreditNoteApplicationDetails.Where(d => d.DocumentType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList();
                    List<Guid> documentIds = new List<Guid>();
                    documentIds.AddRange(invoiceIds);
                    documentIds.AddRange(debitNoteIds);
                    List<AppsWorld.InvoiceModule.Entities.Journal> lstJournal = _journalService.GetListOfJournalByDocId(documentIds, TObject.CompanyId);
                    if (invoiceIds.Any())
                    {
                        List<Invoice> lstInvoice = _invoiceEntityService.GetAllDDByInvoiceId(invoiceIds);
                        foreach (Invoice inv in lstInvoice)
                        {
                            CreditNoteApplicationDetail detail = cnApplication.CreditNoteApplicationDetails.Where(c => c.DocumentId == inv.Id).FirstOrDefault();
                            inv.BalanceAmount += detail.CreditAmount;
                            inv.DocumentState = inv.BalanceAmount == inv.GrandTotal ? InvoiceState.NotPaid : inv.DocumentState;
                            inv.ObjectState = ObjectState.Modified;
                            _invoiceEntityService.Update(inv);
                            if (lstJournal.Any())
                            {
                                AppsWorld.InvoiceModule.Entities.Journal journal = lstJournal.Where(a => a.CompanyId == TObject.CompanyId && a.DocumentId == inv.Id).FirstOrDefault();
                                if (journal != null)
                                {
                                    journal.BalanceAmount = inv.BalanceAmount;
                                    journal.DocumentState = inv.DocumentState;
                                    journal.ObjectState = ObjectState.Modified;
                                    _journalService.Update(journal);
                                }

                            }

                        }
                    }
                    if (debitNoteIds.Any())
                    {
                        List<DebitNote> lstDebitNote = _debitNoteService.GetDDByDebitNoteId(debitNoteIds);
                        foreach (DebitNote debitNote in lstDebitNote)
                        {
                            CreditNoteApplicationDetail detail = cnApplication.CreditNoteApplicationDetails.Where(c => c.DocumentId == debitNote.Id).FirstOrDefault();
                            debitNote.BalanceAmount += detail.CreditAmount;
                            debitNote.DocumentState = debitNote.BalanceAmount == debitNote.GrandTotal ? InvoiceState.NotPaid : debitNote.DocumentState;
                            debitNote.ObjectState = ObjectState.Modified;
                            _debitNoteService.Update(debitNote);

                            if (lstJournal.Any())
                            {
                                AppsWorld.InvoiceModule.Entities.Journal journal = lstJournal.Where(a => a.CompanyId == TObject.CompanyId && a.DocumentId == debitNote.Id).FirstOrDefault();
                                if (journal != null)
                                {
                                    journal.BalanceAmount = debitNote.BalanceAmount;
                                    journal.DocumentState = debitNote.DocumentState;
                                    journal.ObjectState = ObjectState.Modified;
                                    _journalService.Update(journal);
                                }

                            }

                        }
                    }
                }

                #region deleting_existing_record

                foreach (CreditNoteApplicationDetail detailDelete in cnApplication.CreditNoteApplicationDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;

                #endregion deleting_existing_record
                #endregion CNApplication_ReverseExcess_Check_UnCheck_DataUpdation_Invoice_DN

                #region CNApplication_ReverseExcess_Account_Added_Modified
                List<CreditNoteApplicationDetail> lstCNApplicationDetail = _creditNoteApplicationDetailService.GetCreditNoteDetail(cnApplication.Id);
                foreach (ReverseExcessModel detail in TObject.ReverseExcessModels)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        CreditNoteApplicationDetail cnAppDetail = new CreditNoteApplicationDetail();
                        cnAppDetail.Id = Guid.NewGuid();
                        FillCNApplicationReverseModel(cnApplication, detail, cnAppDetail);
                        cnAppDetail.RecOrder = ++recorder;
                        cnAppDetail.DocCurrency = TObject.DocCurrency;
                        cnAppDetail.ObjectState = ObjectState.Added;
                        _creditNoteApplicationDetailService.Insert(cnAppDetail);
                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        CreditNoteApplicationDetail cnAppDetail = lstCNApplicationDetail.Where(c => c.Id == detail.Id).FirstOrDefault();
                        if (cnAppDetail != null)
                        {
                            FillCNApplicationReverseModel(cnApplication, detail, cnAppDetail);
                            cnAppDetail.RecOrder = detail.RecOrder;
                            cnAppDetail.DocCurrency = TObject.DocCurrency;
                            cnAppDetail.ObjectState = ObjectState.Modified;
                            _creditNoteApplicationDetailService.Update(cnAppDetail);
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        CreditNoteApplicationDetail cnAppDetail = lstCNApplicationDetail.Where(c => c.Id == detail.Id).FirstOrDefault();
                        if (cnAppDetail != null)
                            cnAppDetail.ObjectState = ObjectState.Deleted;
                    }
                }
                #endregion CNApplication_ReverseExcess_Account_Added_Modified

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteDetails_Update_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void FillCNApplicationReverseModel(CreditNoteApplication cnApplication, ReverseExcessModel detail, CreditNoteApplicationDetail cnAppDetail)
        {
            cnAppDetail.CreditNoteApplicationId = cnApplication.Id;
            cnAppDetail.DocDescription = detail.ItemDescription;
            cnAppDetail.CreditAmount = detail.DocAmount;
            cnAppDetail.TaxAmount = detail.DocTaxAmount;
            cnAppDetail.TotalAmount = detail.DocTotalAmount;
            cnAppDetail.TaxId = detail.TaxId;
            cnAppDetail.TaxRate = detail.TaxRate;
            cnAppDetail.TaxIdCode = detail.TaxIdCode;
            cnAppDetail.COAId = detail.COAId;
        }

        private string GetNextApplicationNumber(Guid id)
        {
            string DocNumber = "";
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetNextApplicationNumber_GetCall_Request_Message);
                Invoice invoice = _invoiceEntityService.Query(a => a.DocType == DocTypeConstants.CreditNote && a.Id == id).Select().FirstOrDefault();
                var CNAM = _creditNoteApplicationService.Query(a => a.InvoiceId == id).Select(x => new { CNAppNo = x.CreditNoteApplicationNumber, Status = x.Status, Date = x.CreatedDate }).OrderByDescending(a => a.Date).FirstOrDefault();
                int DocNo = 0;
                if (CNAM != null)
                {
                    DocNo = CNAM.Status != CreditNoteApplicationStatus.Void ? Convert.ToInt32(CNAM.CNAppNo.Substring(CNAM.CNAppNo.LastIndexOf("-A") + 2)) : Convert.ToInt32(CNAM.CNAppNo.Substring(CNAM.CNAppNo.IndexOf("-A") + 2).Remove(CNAM.CNAppNo.Substring(CNAM.CNAppNo.IndexOf("-A") + 2).LastIndexOf("-V"), 2));
                }
                DocNo++;
                DocNumber = invoice.InvoiceNumber + ("-A" + DocNo);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetNextApplicationNumber_GetCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return DocNumber;
        }

        private void UpdateDocumentState(Guid documentId, string DocType, decimal amount, string ConnectionString, Guid transationId, DateTime? postingDate, List<DocumentHistoryModel> lstDocuments, Dictionary<Guid, decimal> lstOfRoundingAmount, out decimal? detailRoundingAmount)
        {
            decimal roundingAmount = 0;
            detailRoundingAmount = 0;
            if (amount == 0)
                return;
            UpdatePosting up = new UpdatePosting();
            if (DocType == DocTypeConstants.Invoice)
            {
                Invoice document = _invoiceEntityService.Query(a => a.Id == documentId).Select().FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Invoice_to_Update_Balance_Amount);
                }
                //if the document update outside
                if (document.DocumentState == InvoiceState.Void)
                    throw new Exception(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);
                document.BalanceAmount -= amount;
                if (document.BalanceAmount == 0)
                {
                    document.DocumentState = InvoiceStates.FullyPaid;
                    if (document.RoundingAmount != null && document.RoundingAmount != 0)
                        roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                    else
                        roundingAmount = Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)document.BaseBalanceAmount;

                    document.BaseBalanceAmount = 0;
                    if (roundingAmount != 0)
                        lstOfRoundingAmount.Add(document.Id, roundingAmount);
                    detailRoundingAmount = roundingAmount;

                    document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                    if (document.IsOBInvoice != true)
                    {
                        #region Proc_Update_ClearingStatus_For_Invoice
                        //SqlConnection con = new SqlConnection(ConnectionString);
                        //if (con.State != ConnectionState.Open)
                        //    con.Open();
                        //SqlCommand cmd = new SqlCommand("PROC_UpdateClearedItem_Bean_Update", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@Id", document.Id);
                        //cmd.Parameters.AddWithValue("@DocDate", document.DocDate);
                        //cmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                        //int updateCount = cmd.ExecuteNonQuery();
                        //con.Close();
                        #endregion Update_ClearingStatus_For_Invoice
                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(ConnectionString))
                        {
                            if (conn.State != ConnectionState.Open)
                                conn.Open();
                            SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                            oBcmd.CommandType = CommandType.StoredProcedure;
                            oBcmd.Parameters.AddWithValue("@OBId", document.OpeningBalanceId);
                            oBcmd.Parameters.AddWithValue("@DocumentId", document.Id);
                            oBcmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                            oBcmd.Parameters.AddWithValue("@IsEqual", false);
                            oBcmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                }
                else if (document.BalanceAmount > 0)
                {
                    document.DocumentState = InvoiceStates.PartialPaid;
                    document.BaseBalanceAmount -= Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                }

                else if (document.BalanceAmount < 0)
                {
                    throw new Exception("Credit note amount shouldn't be greater than outstanding balance of invoice");
                }
                if (document.GrandTotal == document.BalanceAmount)
                    document.DocumentState = InvoiceStates.NotPaid;

                document.ObjectState = ObjectState.Modified;
                document.ModifiedBy = InvoiceConstants.System;
                document.ModifiedDate = DateTime.UtcNow;
                _invoiceEntityService.Update(document);
                if (document.IsWorkFlowInvoice == true)
                    FillWokflowInvoice(document, ConnectionString);
                #region OB_Invoice_State_change
                if (/*document.DocumentState != InvoiceStates.NotPaid &&*/ document.IsOBInvoice == true)
                {
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                        oBcmd.CommandType = CommandType.StoredProcedure;
                        oBcmd.Parameters.AddWithValue("@OBId", document.OpeningBalanceId);
                        oBcmd.Parameters.AddWithValue("@DocumentId", document.Id);
                        oBcmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                        oBcmd.Parameters.AddWithValue("@IsEqual", document.BalanceAmount == document.GrandTotal && (document.AllocatedAmount == 0 || document.AllocatedAmount == null) ? true : false);
                        oBcmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                #endregion
                else
                {

                    #region Update_Journal_Detail_Clearing_Status
                    using (SqlConnection conn = new SqlConnection(ConnectionString))
                    {
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand cmd1 = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", conn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@companyId", document.CompanyId);
                        cmd1.Parameters.AddWithValue("@documentId", document.Id);
                        cmd1.Parameters.AddWithValue("@docState", document.DocumentState);
                        cmd1.Parameters.AddWithValue("@balanceAmount", document.BalanceAmount);
                        cmd1.ExecuteNonQuery();
                        conn.Close();
                    }
                    #endregion Update_Journal_Detail_Clearing_Status

                }
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transationId, document.CompanyId, document.Id, document.DocType, document.DocSubType, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.Remarks, postingDate, amount < 0 ? amount : -amount, roundingAmount);
                    if (lstdocumet.Any())
                        lstDocuments.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {
                }
                #endregion Documentary History

            }
            else if (DocType == DocTypeConstants.DebitNote)
            {
                DebitNote document = _debitNoteService.Query(a => a.Id == documentId).Select().FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Debit_Note_to_Update_Balance_Amount);
                }
                //if the document update outside
                if (document.DocumentState == InvoiceState.Void)
                    throw new Exception(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);
                document.BalanceAmount -= amount;
                if (document.BalanceAmount == 0)
                {
                    document.DocumentState = DebitNoteStates.FullyPaid;
                    if (document.RoundingAmount != null && document.RoundingAmount != 0)
                        roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                    else
                        roundingAmount = Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)document.BaseBalanceAmount;

                    document.BaseBalanceAmount = 0;
                    if (roundingAmount != 0)
                        lstOfRoundingAmount.Add(document.Id, roundingAmount);

                    detailRoundingAmount = roundingAmount;
                    document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;

                    #region Proc_Update_ClearingStatus_For_DebitNote
                    //SqlConnection con = new SqlConnection(ConnectionString);
                    //if (con.State != ConnectionState.Open)
                    //    con.Open();
                    //SqlCommand cmd = new SqlCommand("PROC_UpdateClearedItem_Bean_Update", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@Id", document.Id);
                    //cmd.Parameters.AddWithValue("@DocDate", document.DocDate);
                    //cmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                    //int updateCount = cmd.ExecuteNonQuery();
                    //con.Close();
                    #endregion Update_ClearingStatus_For_DebitNote

                }
                else if (document.BalanceAmount > 0)
                {
                    document.DocumentState = DebitNoteStates.PartialPaid;
                    document.BaseBalanceAmount -= Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                }
                else if (document.BalanceAmount < 0)
                {
                    //document.DocumentState = DebitNoteStates.ExcesPaid;
                    throw new Exception("Credit note amount shouldn't be greater than outstanding balance of debit note");
                }
                if (document.GrandTotal == document.BalanceAmount)
                    document.DocumentState = DebitNoteStates.NotPaid;
                document.ObjectState = ObjectState.Modified;
                document.ModifiedBy = InvoiceConstants.System;
                document.ModifiedDate = DateTime.UtcNow;
                _debitNoteService.Update(document);
                #region Update_Journal_Detail_Clearing_Status
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd1 = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", conn);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@companyId", document.CompanyId);
                    cmd1.Parameters.AddWithValue("@documentId", document.Id);
                    cmd1.Parameters.AddWithValue("@docState", document.DocumentState);
                    cmd1.Parameters.AddWithValue("@balanceAmount", document.BalanceAmount);
                    cmd1.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion Update_Journal_Detail_Clearing_Status
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transationId, document.CompanyId, document.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.Remarks, postingDate, amount < 0 ? amount : -amount, roundingAmount);
                    if (lstdocumet.Any())
                        lstDocuments.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {
                }
            }
        }



        private void FillDoubtfulDebt(DoubtfulDebtModel invDTO, Invoice doubtfulDebt)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_FillDoubtfulDebt_FillCall_Request_Message);
                invDTO.Id = doubtfulDebt.Id;
                invDTO.CompanyId = doubtfulDebt.CompanyId;
                invDTO.EntityType = doubtfulDebt.EntityType;
                invDTO.DocSubType = doubtfulDebt.DocType;
                invDTO.InvoiceNumber = doubtfulDebt.InvoiceNumber;
                invDTO.DocNo = doubtfulDebt.DocNo;
                invDTO.IsLocked = doubtfulDebt.IsLocked;
                invDTO.DocDate = doubtfulDebt.DocDate;
                invDTO.EntityId = doubtfulDebt.EntityId;
                //BeanEntity beanEntity = _beanEntityService.GetEntityById(doubtfulDebt.EntityId);
                invDTO.EntityName = _beanEntityService.GetEntityName(doubtfulDebt.EntityId);
                invDTO.Nature = doubtfulDebt.Nature;
                invDTO.DocCurrency = doubtfulDebt.DocCurrency;
                invDTO.ServiceCompanyId = doubtfulDebt.ServiceCompanyId;
                invDTO.ExtensionType = ExtensionType.General;
                invDTO.IsMultiCurrency = doubtfulDebt.IsMultiCurrency;
                invDTO.BaseCurrency = doubtfulDebt.ExCurrency;
                invDTO.ExchangeRate = doubtfulDebt.ExchangeRate;
                invDTO.ExDurationFrom = doubtfulDebt.ExDurationFrom;
                invDTO.ExDurationTo = doubtfulDebt.ExDurationTo;
                //invDTO.IsSegmentReporting = doubtfulDebt.IsSegmentReporting;
                //invDTO.SegmentCategory1 = doubtfulDebt.SegmentCategory1;
                //invDTO.SegmentCategory2 = doubtfulDebt.SegmentCategory2;
                invDTO.Version = "0x" + string.Concat(Array.ConvertAll(doubtfulDebt.Version, x => x.ToString("X2")));
                invDTO.IsGSTApplied = doubtfulDebt.IsGSTApplied;
                invDTO.BalanceAmount = doubtfulDebt.BalanceAmount;
                invDTO.GrandTotal = doubtfulDebt.GrandTotal;
                invDTO.IsBaseCurrencyRateChanged = doubtfulDebt.IsBaseCurrencyRateChanged;
                //invDTO.IsAllowableDisallowableActivated = doubtfulDebt.IsAllowableDisallowableActivated;
                //invDTO.IsAllowableNonAllowable = doubtfulDebt.IsAllowableNonAllowable;

                invDTO.IsNoSupportingDocument = doubtfulDebt.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = doubtfulDebt.NoSupportingDocs;
                //invDTO.SegmentMasterid1 = doubtfulDebt.SegmentMasterid1;

                //if (doubtfulDebt.SegmentMasterid1 != null)
                //{
                //    var segment1 = _segmentMasterService.GetSegmentMastersById(doubtfulDebt.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    invDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
                //}
                //if (doubtfulDebt.SegmentMasterid2 != null)
                //{
                //    var segment2 = _segmentMasterService.GetSegmentMastersById(doubtfulDebt.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    invDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
                //}

                //invDTO.SegmentMasterid2 = doubtfulDebt.SegmentMasterid2;
                //invDTO.SegmentDetailid1 = doubtfulDebt.SegmentDetailid1;
                //invDTO.SegmentDetailid2 = doubtfulDebt.SegmentDetailid2;

                invDTO.Remarks = doubtfulDebt.DocDescription;

                invDTO.Status = doubtfulDebt.Status;
                invDTO.DocumentState = doubtfulDebt.DocumentState;
                invDTO.ModifiedDate = doubtfulDebt.ModifiedDate;
                invDTO.ModifiedBy = doubtfulDebt.ModifiedBy;
                invDTO.CreatedDate = doubtfulDebt.CreatedDate;
                invDTO.UserCreated = doubtfulDebt.UserCreated;

                List<DoubtfulDebtAllocation> lstAllocations = _doubtfulDebtAllocationService.GetDoubtfuldbyId(doubtfulDebt.Id);
                List<Invoice> lstInvoice = _invoiceEntityService.GetAllDDByInvoiceId(lstAllocations.Select(c => c.InvoiceId).ToList());
                foreach (DoubtfulDebtAllocation allocation in lstAllocations)
                {
                    DoubtfulDebtAllocationModel model = new DoubtfulDebtAllocationModel();
                    FillDoubtfulDebtAllocationModel(model, allocation, lstInvoice.Where(c => c.Id == allocation.InvoiceId).FirstOrDefault());
                    invDTO.DoubtfulDebtAllocations.Add(model);
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_FillDoubtfulDebt_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_FillDoubtfulDebt_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void FillDoubtfulDebtAllocationModel(DoubtfulDebtAllocationModel DDAModel, DoubtfulDebtAllocation DDA, Invoice invoice)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_FillDoubtfulDebtAllocationModel_FillCall_Request_Message);

                DDAModel.Id = DDA.Id;
                DDAModel.InvoiceId = DDA.InvoiceId;
                DDAModel.CompanyId = DDA.CompanyId;
                DDAModel.IsLocked = DDA.IsLocked;
                //var invoice = _invoiceEntityService.GetinvoiceById(DDA.InvoiceId);

                if (invoice != null)
                {
                    DDAModel.DocNo = invoice.DocNo;

                    DDAModel.DocCurrency = invoice.DocCurrency;
                    DDAModel.DoubtfulDebtAmount = invoice.GrandTotal;
                    DDAModel.DoubtfulDebtBalanceAmount = invoice.BalanceAmount + DDA.AllocateAmount;
                    if (DDAModel.DoubtfulDebtBalanceAmount > DDAModel.DoubtfulDebtAmount)
                        DDAModel.DoubtfulDebtBalanceAmount = invoice.BalanceAmount;
                    DDAModel.DoubtfulDebtAllocationNumber = DDA.DoubtfulDebtAllocationNumber;
                    DDAModel.DocDate = invoice.DocDate;
                }
                DDAModel.AllocateAmount = DDA.AllocateAmount;
                DDAModel.Version = "0x" + string.Concat(Array.ConvertAll(DDA.Version, x => x.ToString("X2")));
                DDAModel.IsNoSupportingDocument = DDA.IsNoSupportingDocumentActivated;
                DDAModel.NoSupportingDocument = DDA.IsNoSupportingDocument;
                DDAModel.DoubtfulDebitAllocationDate = DDA.DoubtfulDebtAllocationDate;
                DDAModel.DoubtfulDebitAllocationResetDate = DDA.DoubtfulDebtAllocationResetDate;
                DDAModel.Remarks = DDA.Remarks;
                DDAModel.CreatedDate = DateTime.UtcNow;
                DDAModel.IsRevExcess = DDA.IsRevExcess;
                DDAModel.ExchangeRate = invoice.ExchangeRate;
                DDAModel.UserCreated = DDA.UserCreated;
                DDAModel.ModifiedBy = DDA.ModifiedBy;
                DDAModel.ModifiedDate = DDA.ModifiedDate;
                DDAModel.Status = DDA.Status;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_FillDoubtfulDebtAllocationModel_FillCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_FillDoubtfulDebtAllocationModel_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void InsertDoubtfulDebt(DoubtfulDebtModel TObject, Invoice docNew, bool isNew)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_InsertDoubtfulDebt_FillCall_Request_Message);
                docNew.CompanyId = TObject.CompanyId;
                docNew.DocType = DocTypeConstants.DoubtFulDebitNote;
                docNew.DocSubType = "General";
                docNew.EntityType = "Customer";
                docNew.DocDate = TObject.DocDate.Date;
                docNew.EntityId = TObject.EntityId;
                docNew.Nature = TObject.Nature;
                docNew.ServiceCompanyId = TObject.ServiceCompanyId;
                docNew.DocCurrency = TObject.DocCurrency;
                docNew.IsMultiCurrency = TObject.IsMultiCurrency;
                docNew.ExCurrency = TObject.BaseCurrency;
                docNew.ExchangeRate = TObject.ExchangeRate;
                if (isNew == true)
                    docNew.ExtensionType = TObject.ExtensionType;
                docNew.ExDurationFrom = TObject.ExDurationFrom;
                docNew.ExDurationTo = TObject.ExDurationTo;
                decimal diff = 0;
                if (isNew == false)
                {
                    if (docNew.GrandTotal != TObject.GrandTotal)
                    {
                        if (docNew.GrandTotal < TObject.GrandTotal)
                        {
                            diff = TObject.GrandTotal - docNew.GrandTotal;
                            docNew.BalanceAmount = docNew.BalanceAmount + diff;
                        }
                        else if (docNew.GrandTotal > TObject.GrandTotal)
                        {
                            diff = docNew.GrandTotal - TObject.GrandTotal;
                            docNew.BalanceAmount = docNew.BalanceAmount - diff;
                        }
                    }
                    else
                    {
                        var count = _doubtfulDebtAllocationService.GetAllDoubtfuldbtById(TObject.Id);
                        if (count.Any())
                        {
                            docNew.BalanceAmount = count.Sum(c => c.AllocateAmount);
                        }
                        else
                            docNew.BalanceAmount = docNew.GrandTotal;
                    }
                }

                docNew.GrandTotal = TObject.GrandTotal;
                if (isNew)
                    docNew.BalanceAmount = TObject.GrandTotal;

                docNew.IsAllowableDisallowableActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, TObject.CompanyId);
                docNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                if (TObject.NoSupportingDocument != null)
                    docNew.NoSupportingDocs = docNew.IsNoSupportingDocument.Value ? TObject.NoSupportingDocument : null;
                docNew.DocDescription = TObject.Remarks;
                docNew.Status = TObject.Status;
                docNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                docNew.DocumentState = String.IsNullOrEmpty(TObject.DocumentState) ? DoubtfulDebtState.NotAllocated : TObject.DocumentState;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_InsertDoubtfulDebt_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void ValidateDoubtfulDebtAllocation(DoubtfulDebtAllocationModel TObject)
        {
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            if (TObject.DoubtfulDebitAllocationDate == null)
            {
                throw new Exception(DoubtfulDebitValidation.Invalid_Allocation_Date);
            }
            if (TObject.IsRevExcess != true)
            {
                if (TObject.DoubtfulDebtAllocationDetailModels == null || TObject.DoubtfulDebtAllocationDetailModels.Count == 0)
                {
                    throw new Exception(DoubtfulDebitValidation.Atleast_one_Allocation_is_required);
                }
                else
                {
                    int itemCount = TObject.DoubtfulDebtAllocationDetailModels.Where(a => a.AllocateAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(DoubtfulDebitValidation.Atleast_one_Allocation_is_required);
                    }
                }
                var amountDpcuments = TObject.DoubtfulDebtAllocationDetailModels.Where(a => a.AllocateAmount > 0).ToList();
                if (amountDpcuments.Count == 0)
                    throw new Exception(DoubtfulDebitValidation.Atleast_one_allocation_should_be_given);
            }

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(TObject.DoubtfulDebitAllocationDate, TObject.CompanyId))
            {
                throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DoubtfulDebitAllocationDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DoubtfulDebitAllocationDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
            }
            //Verify if any of the allocations have amount


            //Verify Duplication Documents in details
            var duplicateDocuments = TObject.DoubtfulDebtAllocationDetailModels.GroupBy(x => x.DocumentId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            if (duplicateDocuments.Count > 0)
                throw new Exception(DoubtfulDebitValidation.Duplicate_documents_in_details);
        }

        public void UpdateDoubtfulDebtAllocationDetails(DoubtfulDebtAllocationModel model, DoubtfulDebtAllocation ddAllocation, string ConnectionString, Guid transationId, List<DocumentHistoryModel> lstDocHistoryModels)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Request_Message);
                List<DoubtfulDebtAllocationDetail> lstDetails = ddAllocation.DoubtfulDebtAllocationDetails.Where(a => !model.DoubtfulDebtAllocationDetailModels.Any(b => b.Id == a.Id)).ToList();

                foreach (DoubtfulDebtAllocationDetail detailDelete in lstDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;

                List<Invoice> lstInvoice = _invoiceEntityService.GetAllDDByInvoiceId(model.DoubtfulDebtAllocationDetailModels.Where(c => c.DocType == DocTypeConstants.Invoice).Select(a => a.DocumentId).ToList());
                List<DebitNote> lstDN = _debitNoteService.GetDDByDebitNoteId(model.DoubtfulDebtAllocationDetailModels.Where(c => c.DocType == DocTypeConstants.DebitNote).Select(a => a.DocumentId).ToList());

                //Checking the documentstate before proceeding to save the Credit note app details
                if ((lstInvoice.Any() && lstInvoice.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstDN.Any() && lstDN.Any(a => a.DocumentState == InvoiceStates.Void)))
                    throw new Exception(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);

                foreach (DoubtfulDebtAllocationDetailModel detailModel in model.DoubtfulDebtAllocationDetailModels)
                {
                    DoubtfulDebtAllocationDetail detail = ddAllocation.DoubtfulDebtAllocationDetails.Where(a => a.Id == detailModel.Id).FirstOrDefault();

                    if (detail == null)
                    {
                        detail = new DoubtfulDebtAllocationDetail();
                        detail.Id = Guid.NewGuid();
                        detail.DoubtfulDebtAllocationId = model.Id;
                        detail.DocumentId = detailModel.DocumentId;
                        detail.DocumentType = detailModel.DocType;
                        detail.DocCurrency = detailModel.DocCurrency;
                        detail.AllocateAmount = detailModel.AllocateAmount;

                        UpdateDocumentAllocationState(detail.DocumentId, detail.DocumentType, detail.AllocateAmount, false, ConnectionString, transationId, ddAllocation.DoubtfulDebtAllocationDate, false, lstDocHistoryModels, lstInvoice, lstDN);

                        detail.ObjectState = ObjectState.Added;
                        ddAllocation.DoubtfulDebtAllocationDetails.Add(detail);
                    }
                    else
                    {
                        UpdateDocumentAllocationState(detail.DocumentId, detail.DocumentType, detailModel.AllocateAmount - detail.AllocateAmount, false, ConnectionString, transationId, ddAllocation.DoubtfulDebtAllocationDate, false, lstDocHistoryModels, lstInvoice, lstDN);

                        detail.AllocateAmount = detailModel.AllocateAmount;

                        detail.ObjectState = ObjectState.Modified;
                    }

                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,DoubtFulDebitNoteLoggingValidation.Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }
        public void UpdateDoubtfulProvisionDetails(DoubtfulDebtAllocationModel TObject, DoubtfulDebtAllocation ddAllocation, List<DocumentHistoryModel> lstDocHistoryModels)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_Request_Message);
                if (TObject.DoubtfulDebtAllocationDetailModels.Any())
                {
                    bool isInvoice = false;
                    Invoice invoice = null;
                    DebitNote debitnote = null;
                    List<Invoice> lstInvoices = _invoiceEntityService.GetAllDDByInvoiceId(TObject.DoubtfulDebtAllocationDetailModels.Where(x => x.DocType == DocTypeConstant.Invoice && x.RecordStatus == "Deleted").Select(x => x.DocumentId).ToList());

                    List<DebitNote> lstDebitNotes = _debitNoteService.GetDDByDebitNoteId(TObject.DoubtfulDebtAllocationDetailModels.Where(x => x.DocType == DocTypeConstant.DebitNote && x.RecordStatus == "Deleted").Select(x => x.DocumentId).ToList());
                    foreach (var item in TObject.DoubtfulDebtAllocationDetailModels.Where(x => x.RecordStatus == "Deleted"))
                    {
                        if (item.DocType == DocTypeConstant.Invoice)
                        {
                            invoice = lstInvoices.Where(x => x.Id == item.DocumentId).FirstOrDefault();
                            if (invoice != null)
                            {
                                isInvoice = true;
                                invoice.AllocatedAmount -= ddAllocation.AllocateAmount;
                                invoice.ObjectState = ObjectState.Modified;
                                _invoiceEntityService.Update(invoice);
                            }
                        }
                        else if (item.DocType == DocTypeConstant.DebitNote)
                        {
                            debitnote = lstDebitNotes.Where(x => x.Id == item.DocumentId).FirstOrDefault();
                            debitnote.AllocatedAmount -= ddAllocation.AllocateAmount;
                            debitnote.ObjectState = ObjectState.Modified;
                            _debitNoteService.Update(debitnote);
                        }

                    }

                    var ddAllocationDetails = _doubtfulDebtallocationDetailService.GetDoubtfuAllocationByIds(TObject.DoubtfulDebtAllocationDetailModels.Where(x => x.RecordStatus == "Deleted").Select(x => x.Id).ToList());
                    foreach (var ddallocation in ddAllocationDetails)
                    {
                        ddallocation.ObjectState = ObjectState.Deleted;
                        _doubtfulDebtallocationDetailService.Delete(ddallocation);
                    }
                    #region Documentary History
                    try
                    {
                        List<DocumentHistoryModel> lstdocumet = isInvoice ? AppaWorld.Bean.Common.FillDocumentHistory(ddAllocation.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, invoice.DocDate, 0, 0)
                            :
                            AppaWorld.Bean.Common.FillDocumentHistory(ddAllocation.Id, debitnote.CompanyId, debitnote.Id, debitnote.DocSubType, debitnote.DocSubType, debitnote.DocumentState, debitnote.DocCurrency, debitnote.GrandTotal, debitnote.BalanceAmount, debitnote.ExchangeRate.Value, debitnote.ModifiedBy != null ? debitnote.ModifiedBy : debitnote.UserCreated, debitnote.Remarks, debitnote.DocDate, 0, 0);

                        if (lstdocumet.Any())
                            lstDocHistoryModels.AddRange(lstdocumet);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    #endregion Documentary History

                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, DoubtFulDebitNoteLoggingValidation.Log_UpdateDoubtfulDebtAllocationDetails_UpdateCall_SuccessFully_Message);

                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void FillDDReverseAllocation(DoubtfulDebtAllocationModel TObject, DDReverseExcessModel detailModel, DoubtfulDebtAllocationDetail DDADetail)
        {
            DDADetail.DoubtfulDebtAllocationId = TObject.Id;
            DDADetail.DocNo = TObject.DocNo;
            DDADetail.DocDate = TObject.DoubtfulDebitAllocationDate;
            DDADetail.AllocateAmount = detailModel.AllocateAmount;
            DDADetail.ExchangeRate = detailModel.ExchangeRate;
            DDADetail.EntityId = detailModel.EntityId;
            DDADetail.DocCurrency = detailModel.DocCurrency;
        }

        private void FillDoubtFulDebtJournal(JVModel headJournal, DoubtfulDebtAllocation _document, bool isFirst, string DocType, Invoice doubtfulDebt)
        {
            if (isFirst)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = doubtfulDebt.Id;

            headJournal.DocumentId = _document.Id;
            headJournal.CompanyId = _document.CompanyId;
            headJournal.PostingDate = _document.DoubtfulDebtAllocationDate;
            headJournal.DocNo = _document.DoubtfulDebtAllocationNumber;
            headJournal.DocType = DocTypeConstants.DoubtFulDebitNote;
            headJournal.DocSubType = "Allocation";
            headJournal.DocDate = _document.DoubtfulDebtAllocationDate;
            headJournal.DueDate = null;
            headJournal.DocumentState = "Posted";
            headJournal.SystemReferenceNo = _document.DoubtfulDebtAllocationNumber;
            headJournal.ServiceCompanyId = doubtfulDebt.ServiceCompanyId;
            headJournal.Nature = doubtfulDebt.Nature;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = doubtfulDebt.EntityId;
            headJournal.COAId = _chartOfAccountService.GetChartOfAccountByNature(doubtfulDebt.Nature, doubtfulDebt.CompanyId);
            //headJournal.COAId = account.Id;
            //headJournal.AccountCode = account.Code;
            //headJournal.AccountName = account.Name;
            headJournal.DocCurrency = doubtfulDebt.DocCurrency;
            headJournal.GrandDocDebitTotal = _document.AllocateAmount;
            headJournal.BaseCurrency = doubtfulDebt.ExCurrency;
            headJournal.ExchangeRate = doubtfulDebt.ExchangeRate;
            headJournal.IsGstSettings = doubtfulDebt.IsGstSettings;
            headJournal.IsMultiCurrency = doubtfulDebt.IsMultiCurrency;
            headJournal.DocumentDescription = _document.Remarks;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(_document.AllocateAmount * (doubtfulDebt.ExchangeRate != null ? doubtfulDebt.ExchangeRate : 1)), 2);
            headJournal.GSTExCurrency = doubtfulDebt.GSTExCurrency;
            headJournal.GSTExchangeRate = doubtfulDebt.GSTExchangeRate;
            headJournal.UserCreated = _document.UserCreated;
            headJournal.CreatedDate = _document.CreatedDate == DateTime.UtcNow ? DateTime.UtcNow : _document.CreatedDate;
            headJournal.ModifiedBy = _document.ModifiedBy;
            headJournal.ModifiedDate = _document.ModifiedDate;


            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            FillDebitReverseExcessFilling(jModel, _document, doubtfulDebt, true);
            lstJD.Add(jModel);
            jModel = new JVVDetailModel();
            FillDebitReverseExcessFilling(jModel, _document, doubtfulDebt, false);
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD;


        }

        private void FillDebitReverseExcessFilling(JVVDetailModel jDetail, DoubtfulDebtAllocation allocation, Invoice doubtfulDebt, bool isFirst)
        {
            jDetail.DocumentId = allocation.Id;
            jDetail.SystemRefNo = allocation.DoubtfulDebtAllocationNumber;
            jDetail.DocNo = allocation.DoubtfulDebtAllocationNumber;
            jDetail.ServiceCompanyId = doubtfulDebt.ServiceCompanyId.Value;
            jDetail.Nature = doubtfulDebt.Nature;
            jDetail.DocType = DocTypeConstants.DoubtFulDebitNote;
            jDetail.DocSubType = "Allocation";
            jDetail.AccountDescription = doubtfulDebt.Remarks;
            ChartOfAccount account = new ChartOfAccount();
            if (isFirst)
                account = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.Doubtful_Debt_expense, allocation.CompanyId);
            else
                account = _chartOfAccountService.GetChartOfAccountByName(doubtfulDebt.Nature == "Trade" ? COANameConstants.Doubtful_Debt_Provision_AR : COANameConstants.Doubtful_Debt_Provision_OR, doubtfulDebt.CompanyId);
            if (account != null)
            {
                jDetail.COAId = account.Id;
                jDetail.AccountCode = account.Code;
                jDetail.AccountName = account.Name;
            }
            jDetail.CreditTermsId = doubtfulDebt.CreditTermsId;
            jDetail.DueDate = null;
            jDetail.EntityId = doubtfulDebt.EntityId;
            jDetail.DocCurrency = doubtfulDebt.DocCurrency;
            jDetail.BaseCurrency = doubtfulDebt.ExCurrency;
            jDetail.ExchangeRate = doubtfulDebt.ExchangeRate;
            jDetail.GSTExCurrency = doubtfulDebt.GSTExCurrency;
            jDetail.GSTExchangeRate = doubtfulDebt.GSTExchangeRate;
            jDetail.AccountDescription = doubtfulDebt.DocDescription;
            if (isFirst)
            {
                jDetail.DocCredit = allocation.AllocateAmount;
                jDetail.BaseCredit = Math.Round((decimal)doubtfulDebt.ExchangeRate == null ? (decimal)jDetail.DocCredit : (decimal)(jDetail.DocCredit * doubtfulDebt.ExchangeRate), 2);
            }
            else
            {
                jDetail.DocDebit = allocation.AllocateAmount;
                jDetail.BaseDebit = Math.Round((decimal)doubtfulDebt.ExchangeRate == null ? (decimal)jDetail.DocDebit : (decimal)(jDetail.DocDebit * doubtfulDebt.ExchangeRate), 2);

            }
            jDetail.DocDate = allocation.DoubtfulDebtAllocationDate;
            jDetail.PostingDate = allocation.DoubtfulDebtAllocationDate;
        }

        private void UpdateDocumentAllocationState(Guid documentId, string DocType, decimal amount, bool? isReset, string ConnectionString, Guid transationId, DateTime? postingDate, bool isVoid, List<DocumentHistoryModel> lstDocHistoryModels, List<Invoice> lstOfInvoices, List<DebitNote> lstOfDebitNotes)
        {
            if (amount == 0)
                return;
            if (DocType == DocTypeConstants.Invoice)
            {
                Invoice document = lstOfInvoices.Where(a => a.Id == documentId).FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Invoice_to_Update_Balance_Amount);
                }
                document.AllocatedAmount = (document.AllocatedAmount == null ? 0 : document.AllocatedAmount.Value) + amount;
                document.ModifiedBy = "System";
                document.ModifiedDate = DateTime.UtcNow;
                document.ObjectState = ObjectState.Modified;

                _invoiceEntityService.Update(document);
                if (document.IsWorkFlowInvoice == true)
                {
                    FillWokflowInvoice(document, ConnectionString);
                }
                if (document.IsOBInvoice == true)
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", document.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", document.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", isReset == true ? document.DocumentState == InvoiceState.NotPaid ? true : false : false);
                    int res = oBcmd.ExecuteNonQuery();
                    con.Close();
                }


                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transationId, document.CompanyId, document.Id, document.DocType, document.DocSubType, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.DocDescription, isVoid ? null : postingDate, isVoid ? 0 : amount, 0);

                    if (lstdocumet.Any())
                        lstDocHistoryModels.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion Documentary History




            }
            else if (DocType == DocTypeConstants.DebitNote)
            {
                DebitNote document = lstOfDebitNotes.Where(a => a.Id == documentId).FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Debit_Note_to_Update_Balance_Amount_);
                }
                document.AllocatedAmount = (document.AllocatedAmount == null ? 0 : document.AllocatedAmount.Value) + amount;
                document.ModifiedBy = "System";
                document.ModifiedDate = DateTime.UtcNow;
                document.ObjectState = ObjectState.Modified;
                _debitNoteService.Update(document);

                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transationId, document.CompanyId, document.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.Remarks, isVoid ? null : postingDate, isVoid ? 0 : amount, 0);

                    if (lstdocumet.Any())
                        lstDocHistoryModels.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                #endregion Documentary History


            }
        }

        private string GetNextAllocationNumber(Guid id)
        {
            string DocNumber = "";
            Invoice invoice = _invoiceEntityService.GetDoubtfulDebtNoteById(id);
            var DDAM = _doubtfulDebtAllocationService.GetAllDebtNote(id);
            int DocNo = 0;
            if (DDAM != null)
            {
                DocNo = Convert.ToInt32(DDAM.DoubtfulDebtAllocationNumber.Substring(DDAM.DoubtfulDebtAllocationNumber.LastIndexOf("-A") + 2));
            }
            DocNo++;
            DocNumber = invoice.InvoiceNumber + ("-A" + DocNo);
            return DocNumber;
        }

        private void FillJournalState(UpdatePosting _posting, Invoice invoice)
        {
            _posting.Id = invoice.Id;
            _posting.CompanyId = invoice.CompanyId;
            _posting.DocumentState = invoice.DocumentState;
            _posting.BalanceAmount = invoice.BalanceAmount;
        }
        public void FillWokflowInvoice(Invoice invoice, string ConnectionString)
        {
            InvoiceVM invoicevm = new InvoiceVM();
            invoicevm.Id = invoice.DocumentId.Value;
            invoicevm.CompanyId = invoice.CompanyId;
            invoicevm.TotalFee = invoice.GrandTotal;
            invoicevm.BalanceFee = invoice.BalanceAmount;
            string state = invoice.DocumentState;
            invoicevm.InvoiceState = state == "Partial Paid" ? "Partially paid" : invoice.DocumentState;
            invoicevm.ModifiedBy = invoice.ModifiedBy;
            invoicevm.Status = RecordStatusEnum.Active;
            try
            {
                SqlConnection con = null;
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("Bean_BCInvoice_State_To_WFInvoice", con);
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DocumentId", invoice.DocumentId.ToString());
                    cmd.Parameters.AddWithValue("@State", invoice.DocumentState.ToString());
                    cmd.Parameters.AddWithValue("@Amount", invoice.BalanceAmount);
                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                    cmd.Parameters.AddWithValue("@BaseBalanceAmount", invoice.BaseBalanceAmount);
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception)
            {

            }

        }
        private void FillWorkflowInvoiceDeatils(InvoiceModel TObject, Invoice _invoiceNew)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Request_Message);
                List<long?> lstTaxId = TObject.IsGstSettings ? TObject.InvoiceDetailModels.Where(a => a.TaxIdCode == null).Select(a => a.TaxId).ToList() : null;
                Dictionary<long, string> lstTaxs = lstTaxId != null ? _taxCodeService.GetTaxCodes(lstTaxId, 0) : null;

                foreach (InvoiceDetailModel detail in TObject.InvoiceDetailModels)
                {
                    InvoiceDetail invoiceDetails = _invoiceNew.InvoiceDetails.FirstOrDefault(a => a.InvoiceId == detail.Id);
                    if (invoiceDetails == null)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();
                        invoiceDetail.Id = Guid.NewGuid();
                        invoiceDetail.InvoiceId = _invoiceNew.Id;
                        invoiceDetail.ItemId = detail.ItemId;
                        invoiceDetail.ItemCode = detail.ItemCode;
                        invoiceDetail.ItemDescription = detail.ItemDescription;
                        invoiceDetail.Qty = detail.Qty;
                        invoiceDetail.Unit = detail.Unit;
                        invoiceDetail.UnitPrice = detail.UnitPrice;
                        invoiceDetail.COAId = detail.COAId;
                        invoiceDetail.TaxId = detail.TaxId;
                        invoiceDetail.TaxRate = detail.TaxRate;
                        if (TObject.IsGstSettings && detail.TaxIdCode == null)
                        {
                            invoiceDetail.TaxIdCode = lstTaxs != null ? lstTaxs.Where(a => a.Key == detail.TaxId).Select(a => a.Value != "NA" ? (a.Value + "-" + invoiceDetail.TaxRate + "%") : "NA").FirstOrDefault() : detail.TaxIdCode;
                        }
                        else
                            invoiceDetail.TaxIdCode = detail.TaxIdCode;
                        invoiceDetail.DocTaxAmount = detail.DocTaxAmount == null ? 0 : detail.DocTaxAmount;
                        //invoiceDetail.TaxCurrency = detail.TaxCurrency;
                        invoiceDetail.DocAmount = detail.DocAmount;
                        invoiceDetail.AmtCurrency = detail.AmtCurrency;
                        invoiceDetail.DocTotalAmount = detail.DocTotalAmount;
                        //invoiceDetail.AllowDisAllow = lstCoa.Where(c => c.Id == detail.COAId).Select(c => c.DisAllowable).FirstOrDefault();
                        //invoiceDetail.IsPLAccount = detail.IsPLAccount;
                        //invoiceDetail.BaseAmount = detail.BaseAmount;
                        //invoiceDetail.BaseTaxAmount = detail.BaseTaxAmount;
                        //invoiceDetail.BaseTotalAmount = detail.BaseTotalAmount;
                        invoiceDetail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(invoiceDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocAmount;
                        decimal? amount = invoiceDetail.DocTaxAmount != null ? Math.Round((decimal)invoiceDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocTaxAmount;
                        invoiceDetail.BaseTaxAmount = TObject.ExchangeRate != null ? amount : invoiceDetail.DocTaxAmount;
                        invoiceDetail.BaseTotalAmount = Math.Round((decimal)invoiceDetail.BaseAmount + (invoiceDetail.BaseTaxAmount != null ? (decimal)invoiceDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                        invoiceDetail.RecOrder = recorder + 1;
                        recorder = invoiceDetail.RecOrder;
                        invoiceDetail.ObjectState = ObjectState.Added;
                        _invoiceDetailService.Insert(invoiceDetail);
                    }
                    //if (TObject.InvoiceGSTDetailModels != null)
                    //{
                    //    foreach (InvoiceGSTDetailModel gstdetail in TObject.InvoiceGSTDetailModels)
                    //    {
                    //        InvoiceGSTDetail gstdetails = new InvoiceGSTDetail();
                    //        gstdetails.Id = Guid.NewGuid();
                    //        gstdetails.InvoiceId = _invoiceNew.Id;
                    //        gstdetails.TaxAmount = gstdetail.TaxAmount;
                    //        gstdetails.Amount = gstdetail.Amount;
                    //        gstdetails.TotalAmount = gstdetail.TotalAmount;
                    //        gstdetails.TaxId = gstdetail.TaxId;
                    //        gstdetails.ObjectState = ObjectState.Added;
                    //        _invoiceGSTDetailService.Insert(gstdetails);
                    //    }

                    //}
                    //else
                    //{                                                
                    //    invoiceDetails.AllowDisAllow = detail.AllowDisAllow;
                    //    //invoiceDetails.RecOrder = recorder + 1;                        
                    //    invoiceDetails.ObjectState = ObjectState.Modified;
                    //    _invoiceDetailService.Update(invoiceDetails);
                    //}
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw;
            }
        }
        private void FillJournalState(UpdatePosting _posting, DebitNote debit)
        {
            _posting.Id = debit.Id;
            _posting.CompanyId = debit.CompanyId;
            _posting.DocumentState = debit.DocumentState;
            _posting.BalanceAmount = debit.BalanceAmount;
        }

        private void FillRecurringInvoice(RecurringModel invDTO, Invoice invoice)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillInvoice_FillCall_Request_Message);
                invDTO.Id = invoice.Id;
                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocNo = invoice.DocNo;
                invDTO.DocDescription = invoice.DocDescription;
                invDTO.DocDate = invoice.DocDate;
                invDTO.DueDate = invoice.DueDate;
                invDTO.PONo = invoice.PONo;
                invDTO.EntityId = invoice.EntityId;
                invDTO.Version = "0x" + string.Concat(Array.ConvertAll(invoice.Version, x => x.ToString("X2")));
                //BeanEntity beanEntity = _beanEntityService.GetEntityById(invoice.EntityId);
                //invDTO.EntityName = beanEntity.Name;
                invDTO.CreditTermsId = invoice.CreditTermsId;
                invDTO.CustCreditlimit = _beanEntityService.GetCteditLimitsValue(invoice.EntityId);
                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;
                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;
                invDTO.IsGstSettings = invoice.IsGstSettings;
                invDTO.GSTExCurrency = invoice.GSTExCurrency;
                invDTO.GSTExchangeRate = invoice.GSTExchangeRate;
                invDTO.GSTExDurationFrom = invoice.GSTExDurationFrom;
                invDTO.GSTExDurationTo = invoice.GSTExDurationTo;
                invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                invDTO.SegmentCategory1 = invoice.SegmentCategory1;
                invDTO.SegmentCategory2 = invoice.SegmentCategory2;
                invDTO.IsRepeatingInvoice = invoice.IsRepeatingInvoice;
                invDTO.RepEveryPeriodNo = invoice.RepEveryPeriodNo;
                invDTO.RepEveryPeriod = invoice.RepEveryPeriod;
                invDTO.EndDate = invoice.RepEndDate;
                invDTO.ParentInvoiceID = invoice.ParentInvoiceID;
                invDTO.BalanceAmount = invoice.BalanceAmount;
                invDTO.GSTTotalAmount = invoice.GSTTotalAmount;
                invDTO.GrandTotal = invoice.GrandTotal;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;
                invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;
                invDTO.InternalState = invoice.InternalState;
                invDTO.EntityName = _beanEntityService.GetEntityName(invDTO.EntityId);
                invDTO.DocDescription = invoice.DocDescription;
                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;

                //if (invoice.SegmentMasterid1 != null)
                //{
                //    var segment1 = _segmentMasterService.GetSegmentMastersById(invoice.SegmentMasterid1.Value).FirstOrDefault();
                //    invDTO.IsSegmentActive1 = true;
                //}
                //if (invoice.SegmentMasterid2 != null)
                //{
                //    var segment2 = _segmentMasterService.GetSegmentMastersById(invoice.SegmentMasterid2.Value).FirstOrDefault();
                //    invDTO.IsSegmentActive2 = true;
                //}

                invDTO.SegmentMasterid2 = invoice.SegmentMasterid2;
                invDTO.SegmentDetailid1 = invoice.SegmentDetailid1;
                invDTO.SegmentDetailid2 = invoice.SegmentDetailid2;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = invoice.DocumentState;
                invDTO.ModifiedDate = invoice.ModifiedDate;
                invDTO.ModifiedBy = invoice.ModifiedBy;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;
                invDTO.CursorType = invoice.CursorType;
                invDTO.DocumentId = invoice.DocumentId;
                invDTO.IsPost = invoice.IsPost;
                invDTO.InvoiceNumber = invoice.InvoiceNumber;
                invDTO.Remarks = invoice.Remarks;
                invDTO.SegmentMasterid1 = invoice.SegmentMasterid1;
                //invDTO.Remarks = invoice.DocDescription;
                //invDTO.DocDescription = invoice.Remarks;
                invDTO.RecurInvId = invoice.RecurInvId;

                #region commented_code
                //List<InvoiceDetail> lstDetail = new List<InvoiceDetail>();
                ////  invDTO.InvoiceDetails = _invoiceDetailService.GetById(invoice.Id);
                //foreach (var invD in invoice.InvoiceDetails)
                //{
                //    InvoiceDetail invDetail = new InvoiceDetail();
                //    var item1 = _itemService.GetItemById(invD.ItemId.Value, invoice.CompanyId);
                //    ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountById(invD.COAId);
                //    invDetail.AccountName = coa.Name;
                //    if (invD.TaxId != null)
                //    {

                //        TaxCode tax = _taxCodeService.Query(c => c.Id == invD.TaxId).Select().FirstOrDefault();
                //        invDetail.TaxId = tax.Id;
                //        invDetail.TaxCode = tax.Code;
                //        invDetail.TaxRate = tax.TaxRate;
                //        invDetail.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
                //        invDetail.TaxType = tax.TaxType;
                //    }
                //    invDetail.Id = invD.Id;
                //    invDetail.AllowDisAllow = invD.AllowDisAllow;
                //    invDetail.AmtCurrency = invD.AmtCurrency;
                //    invDetail.BaseAmount = invD.BaseAmount;
                //    invDetail.BaseTaxAmount = invD.BaseTaxAmount;
                //    invDetail.BaseTotalAmount = invD.BaseTotalAmount;
                //    invDetail.UnitPrice = invD.UnitPrice;
                //    invDetail.Unit = invD.Unit;
                //    invDetail.COAId = invD.COAId;
                //    invDetail.Discount = invD.Discount;
                //    invDetail.DiscountType = invD.DiscountType;
                //    invDetail.DocAmount = invD.DocAmount;
                //    invDetail.DocTaxAmount = invD.DocTaxAmount;
                //    invDetail.DocTotalAmount = invD.DocTotalAmount;
                //    invDetail.InvoiceId = invD.InvoiceId;
                //    invDetail.ItemId = invD.ItemId;
                //    invDetail.ItemDescription = item1.Description == null ? invD.ItemDescription != null ? invD.ItemDescription : item1.Description : item1.Description;
                //    invDetail.Qty = invD.Qty;
                //    invDetail.Remarks = invD.Remarks;
                //    invDetail.TaxCurrency = invD.TaxCurrency;
                //    invDetail.ItemCode = item1.Code;
                //    //invDetail.ItemDescription = item1.Description;
                //    invDetail.RecOrder = invD.RecOrder;
                //    invDetail.IsPLAccount = invD.IsPLAccount;
                //    lstDetail.Add(invDetail);
                //}
                //invDTO.InvoiceDetails = lstDetail.OrderBy(c => c.RecOrder).ToList();
                #endregion commented_code



                // join item in _itemService.Queryable()  on invD.ItemId equals item.Id
                invDTO.InvoiceDetails = (from invD in invoice.InvoiceDetails
                                             //join tax in _taxCodeService.Queryable()
                                             //on invD.TaxId equals tax.Id into j
                                             //from s in j.DefaultIfEmpty()
                                         select new InvoiceDetail
                                         {
                                             Id = invD.Id,
                                             //AllowDisAllow = invD.AllowDisAllow,
                                             AmtCurrency = invD.AmtCurrency,
                                             BaseAmount = invD.BaseAmount,
                                             BaseTaxAmount = invD.BaseTaxAmount,
                                             BaseTotalAmount = invD.BaseTotalAmount,
                                             COAId = invD.COAId,
                                             //IsPLAccount = invD.IsPLAccount,
                                             Discount = invD.Discount,
                                             DiscountType = invD.DiscountType,
                                             DocAmount = invD.DocAmount,
                                             DocTaxAmount = invD.DocTaxAmount,
                                             DocTotalAmount = invD.DocTotalAmount,
                                             InvoiceId = invD.InvoiceId,
                                             ItemId = invD.ItemId,
                                             ItemCode = invD.ItemCode,
                                             ItemDescription = invD.ItemDescription,
                                             Qty = invD.Qty,
                                             Unit = invD.Unit,
                                             UnitPrice = invD.UnitPrice,
                                             Remarks = invD.Remarks,
                                             TaxId = invD.TaxId,
                                             TaxIdCode = invD.TaxIdCode,
                                             TaxRate = invD.TaxRate
                                         }).ToList();

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_FillInvoice_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        public string GetDocNumbers(long companyId, string docNumber)
        {
            string docNo = docNumber;
            int i = 0;
            bool isBreak = false;
            List<Invoice> lstInvoices = _invoiceEntityService.GetDocNumber(companyId, docNumber);
            if (lstInvoices.Any())
            {
                while (isBreak == false)
                {
                    i++;
                    docNo = docNumber + "-" + i;
                    var inc = lstInvoices.Where(a => a.DocNo == docNo).Select(a => a.DocNo).FirstOrDefault();
                    if (inc == null)
                    {
                        isBreak = true;
                    }
                }
            }
            return docNo;
        }

        public void SmartCursorSyncing(long? companyId, string cursor, string doctype, Guid? sourceId, Guid? targetId, string status, string remarks, string ConnectionString)
        {
            if (doctype == DocTypeConstants.Invoice && cursor == CursorConstants.Workflow)
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.WF_invoice_status_call_executing);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmdd = new SqlCommand("SmartCursors_Syncing_Process", conn);
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.CommandTimeout = 0;
                    cmdd.Parameters.AddWithValue("@TargetId", targetId == null ? Guid.NewGuid().ToString() : targetId.ToString());
                    cmdd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmdd.Parameters.AddWithValue("@Status", status);
                    cmdd.Parameters.AddWithValue("@SourceId", sourceId.ToString());
                    cmdd.Parameters.AddWithValue("@Cursor", cursor);
                    cmdd.Parameters.AddWithValue("@DocType", doctype);
                    cmdd.Parameters.AddWithValue("@Remarks", remarks);
                    cmdd.ExecuteNonQuery();
                    conn.Close();
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.WF_invoice_status_call_execution_completed);
                }
            }
        }

        #endregion

        #region jv
        private void FillJournal(JVModel headJournal, Invoice invoice, bool isNew, string type)
        {
            decimal? baseAmount = 0;
            fillJV(headJournal, invoice, type);
            headJournal.COAId = _chartOfAccountService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            int? recOreder = 0;

            foreach (InvoiceDetail detail in invoice.InvoiceDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                FillJVDetail(journal, invoice, detail, type);
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                //var account1 = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
                journal.COAId = detail.COAId;
                journal.RecOrder = detail.RecOrder;

                //journal.RecOrder = recOreder + 1;
                //recOreder = journal.RecOrder;
                lstJD.Add(journal);
            }
            //if (invoice.IsWorkFlowInvoice == false || invoice.IsWorkFlowInvoice == null)
            //{
            if (invoice.IsGstSettings)
            {
                ChartOfAccount account2 = _chartOfAccountService.Query(a => a.CompanyId == invoice.CompanyId && a.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();
                //ChartOfAccount gstAccount = _chartOfAccountService.GetChartOfAccountByCompanyId(invoice.CompanyId);
                List<TaxCode> lstTaxCodes = _taxCodeService.GetTaxCodes(invoice.CompanyId);
                foreach (InvoiceDetail detail in invoice.InvoiceDetails.Where(c => c.TaxRate != null && c.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    FillJVGstDetail(journal, detail, invoice, type, account2, lstTaxCodes);
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    //journal.RecOrder = recOreder + 1;
                    //recOreder = journal.RecOrder;
                    journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(journal);
                }
            }
            //}
            if (type == DocTypeConstants.Invoice)
            {
                baseAmount = lstJD.Sum(c => c.BaseCredit);
                headJournal.GrandBaseDebitTotal = baseAmount;
            }
            else if (type == DocTypeConstants.CreditNote)
            {
                baseAmount = lstJD.Sum(c => c.BaseDebit);
                headJournal.GrandBaseCreditTotal = baseAmount;
            }
            FillJVHeadDetail(jModel, invoice, type);
            if (type != DocTypeConstants.CreditNote)
                jModel.AmountDue = (invoice.DocumentState != InvoiceState.NotPaid /*|| invoice.DocumentState != InvoiceState.NotApplied*/) ? headJournal.BalanceAmount : null;
            //if (type == DocTypeConstants.Invoice)
            //{
            //    jModel.BaseDebit = baseAmount;
            //}
            //if (type == DocTypeConstants.CreditNote)
            //{
            //    jModel.BaseCredit = baseAmount;
            //}
            jModel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            jModel.COAId = headJournal.COAId;
            recOreder = jModel.RecOrder;
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0).OrderBy(x => x.RecOrder).ToList();
        }
        private void FillJVGstDetail(JVVDetailModel journal, InvoiceDetail detail, Invoice invoice, string type, ChartOfAccount account2, List<TaxCode> lstTaxCodes)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.DocNo = invoice.DocNo;
            journal.DocType = invoice.DocType;
            journal.DocSubType = invoice.DocSubType;
            journal.PostingDate = invoice.DocDate;
            //ChartOfAccount account2 = _chartOfAccountService.Query(a => a.CompanyId == invoice.CompanyId && a.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();
            journal.COAId = account2.Id;
            journal.AccountCode = account2.Code;
            journal.AccountName = account2.Name;
            journal.DocCurrency = invoice.DocCurrency;
            journal.BaseCurrency = invoice.ExCurrency;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.EntityId = invoice.EntityId;
            journal.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            //journal.AccountDescription = detail.ItemDescription;
            journal.AccountDescription = invoice.DocDescription;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //TaxCode tax = lstTaxCodes.Where(a => a.Id == detail.TaxId).FirstOrDefault();
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxRate = detail.TaxRate;
            }
            if (type == DocTypeConstants.Invoice)
            {

                journal.DocCredit = detail.DocTaxAmount.Value;
                journal.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocCredit : (decimal)(journal.DocCredit * invoice.ExchangeRate), 2);
            }
            else if (type == DocTypeConstants.CreditNote)
            {
                journal.DocDebit = detail.DocTaxAmount.Value;
                journal.BaseDebit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * invoice.ExchangeRate), 2);
            }
            journal.IsTax = true;
        }
        private void FillJVDetail(JVVDetailModel journal, Invoice invoice, InvoiceDetail detail, string type)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.DocNo = invoice.DocNo;
            journal.DocDate = invoice.DocDate;
            journal.DocType = invoice.DocType;
            journal.PostingDate = invoice.DocDate;
            journal.EntityId = invoice.EntityId;
            journal.SystemRefNo = invoice.InvoiceNumber;
            journal.DocSubType = invoice.DocSubType;
            if (detail.ItemId != null)
            {
                journal.ItemId = detail.ItemId.Value;
                journal.ItemCode = detail.ItemCode;
                journal.AccountDescription = detail.ItemDescription;
            }
            journal.AccountDescription = detail.ItemDescription;
            journal.Qty = detail.Qty.Value;
            journal.Unit = detail.Unit;
            journal.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;

            journal.UnitPrice = detail.UnitPrice == null ? detail.DocTotalAmount : detail.UnitPrice.Value;


            journal.Discount = detail.Discount == null ? 0.0 : detail.Discount;
            journal.DiscountType = detail.DiscountType;
            //journal.AllowDisAllow = detail.AllowDisAllow;
            journal.DocCurrency = invoice.DocCurrency;
            journal.BaseCurrency = invoice.ExCurrency;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            //journal.AccountDescription = invoice.Remarks;
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
            if (type == DocTypeConstants.Invoice)
            {
                journal.DocCredit = detail.DocAmount;
                journal.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocCredit : (decimal)(journal.DocCredit * invoice.ExchangeRate), 2);
                journal.DocTaxCredit = detail.DocTaxAmount != null ? detail.DocTaxAmount.Value : (decimal?)null;
                journal.BaseTaxCredit = detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (invoice.GSTExchangeRate != null ? (decimal)invoice.GSTExchangeRate : 1), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
                journal.GSTCredit = detail.DocAmount != null ? Math.Round(detail.DocAmount * (invoice.GSTExchangeRate != null ? (decimal)invoice.GSTExchangeRate : 1), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
            }
            else if (type == DocTypeConstants.CreditNote)
            {

                journal.DocDebit = detail.DocAmount;
                journal.BaseDebit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * invoice.ExchangeRate), 2);
                journal.DocTaxDebit = detail.DocTaxAmount != null ? detail.DocTaxAmount.Value : (decimal?)null;
                journal.BaseTaxDebit = detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (invoice.GSTExchangeRate != null ? (decimal)invoice.GSTExchangeRate : 1), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
                journal.GSTDebit = detail.DocAmount != null ? Math.Round(detail.DocAmount * (invoice.GSTExchangeRate != null ? (decimal)invoice.GSTExchangeRate : 1), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
                //journal.BaseDebit = detail.BaseAmount;
            }
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.BaseTaxableAmount = detail.BaseAmount;
            journal.BaseTaxAmount = detail.BaseTaxAmount;
            //journal.SegmentCategory1 = invoice.SegmentCategory1;
            //journal.SegmentCategory2 = invoice.SegmentCategory2;
            //journal.SegmentMasterid1 = invoice.SegmentMasterid1;
            //journal.SegmentMasterid2 = invoice.SegmentMasterid2;
            //journal.SegmentDetailid1 = invoice.SegmentDetailid1;
            //journal.SegmentDetailid2 = invoice.SegmentDetailid2;
            journal.IsTax = false;
            journal.DocCreditTotal = detail.DocTotalAmount;
        }
        private void FillJVHeadDetail(JVVDetailModel jModel, Invoice invoice, string type)
        {
            jModel.DocumentId = invoice.Id;
            jModel.SystemRefNo = invoice.InvoiceNumber;
            jModel.DocNo = invoice.DocNo;
            jModel.DocDate = invoice.DocDate;
            jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocSubType = invoice.DocSubType;
            jModel.DocType = invoice.DocType;
            //jModel.Remarks = invoice.Remarks;
            jModel.AccountDescription = invoice.DocDescription;
            jModel.PostingDate = invoice.DocDate;
            jModel.PONo = invoice.PONo;
            jModel.CreditTermsId = invoice.CreditTermsId;
            jModel.DueDate = invoice.DueDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            jModel.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            if (type == DocTypeConstants.Invoice)
            {
                jModel.DocDebit = invoice.GrandTotal;
                //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocDebit : (decimal)(jModel.DocDebit * jModel.ExchangeRate), 2);
                decimal amount = 0;
                foreach (var detail in invoice.InvoiceDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseDebit = amount;
            }
            if (type == DocTypeConstants.CreditNote)
            {
                jModel.DocCredit = invoice.GrandTotal;
                //jModel.BaseCredit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocCredit : (decimal)(jModel.DocCredit * jModel.ExchangeRate), 2);
                decimal amount = 0;
                foreach (var detail in invoice.InvoiceDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseCredit = amount;
            }
            //jModel.SegmentCategory1 = invoice.SegmentCategory1;
            //jModel.SegmentCategory2 = invoice.SegmentCategory2;
            //jModel.SegmentMasterid1 = invoice.SegmentMasterid1;
            //jModel.SegmentMasterid2 = invoice.SegmentMasterid2;
            //jModel.SegmentDetailid1 = invoice.SegmentDetailid1;
            //jModel.SegmentDetailid2 = invoice.SegmentDetailid2;
            jModel.BaseAmount = invoice.BalanceAmount;
        }
        private void fillJV(JVModel headJournal, Invoice invoice, string type)
        {
            string strServiceCompany = _companyService.GetIdBy(invoice.ServiceCompanyId.Value);
            TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice.CreditTermsId);
            headJournal.DocumentId = invoice.Id;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = invoice.DocDate;
            headJournal.DocNo = invoice.DocNo;
            if (type == DocTypeConstants.Invoice)
                headJournal.DocType = DocTypeConstants.Invoice;
            if (type == DocTypeConstants.CreditNote)
                headJournal.DocType = DocTypeConstants.CreditNote;
            if (type == DocTypeConstants.DoubtFulDebitNote)
                headJournal.DocType = DocTypeConstants.DoubtFulDebitNote;
            headJournal.DocSubType = invoice.DocSubType;
            headJournal.DocDate = invoice.DocDate;
            headJournal.PostingDate = invoice.DocDate;
            //headJournal.DocumentDescription = invoice.IsWorkFlowInvoice == true ? invoice.DocDescription : invoice.Remarks;
            headJournal.DocumentDescription = invoice.DocDescription;
            headJournal.DueDate = invoice.DueDate.Value;
            headJournal.DocumentState = invoice.DocumentState;
            headJournal.SystemReferenceNo = invoice.InvoiceNumber;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = invoice.Nature;
            headJournal.PoNo = invoice.PONo;
            headJournal.ModifiedBy = invoice.ModifiedBy;
            headJournal.ModifiedDate = invoice.ModifiedDate;
            headJournal.ExDurationFrom = invoice.ExDurationFrom;
            headJournal.ExDurationTo = invoice.ExDurationTo;
            headJournal.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
            headJournal.GSTExDurationFrom = invoice.GSTExDurationFrom;
            headJournal.GSTExDurationTo = invoice.GSTExDurationTo;
            headJournal.CreditTermsId = invoice.CreditTermsId != null ? invoice.CreditTermsId.Value : (long?)null;
            headJournal.BalanceAmount = invoice.BalanceAmount;

            if (top != null)
                headJournal.CreditTerms = (int)top.TOPValue;
            headJournal.IsSegmentReporting = invoice.IsSegmentReporting;
            headJournal.SegmentCategory1 = invoice.SegmentCategory1;
            headJournal.SegmentCategory2 = invoice.SegmentCategory2;
            headJournal.SegmentMasterid1 = invoice.SegmentMasterid1;
            headJournal.SegmentMasterid2 = invoice.SegmentMasterid2;
            headJournal.SegmentDetailid1 = invoice.SegmentDetailid1;
            headJournal.SegmentDetailid2 = invoice.SegmentDetailid2;
            headJournal.NoSupportingDocument = invoice.NoSupportingDocs;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            headJournal.EntityType = invoice.EntityType;
            headJournal.IsRepeatingInvoice = invoice.IsRepeatingInvoice;
            headJournal.RepEveryPeriod = invoice.RepEveryPeriod;
            headJournal.RepEveryPeriodNo = invoice.RepEveryPeriodNo;
            headJournal.EndDate = invoice.RepEndDate;
            headJournal.DocCurrency = invoice.DocCurrency;
            if (type == DocTypeConstants.Invoice || type == DocTypeConstants.DoubtFulDebitNote)
            {
                headJournal.GrandDocDebitTotal = invoice.GrandTotal;
                headJournal.GrandBaseDebitTotal = Math.Round((decimal)(invoice.GrandTotal * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            }
            else if (type == DocTypeConstants.CreditNote)
            {
                headJournal.GrandDocCreditTotal = invoice.GrandTotal;
                headJournal.GrandBaseCreditTotal = Math.Round((decimal)(invoice.GrandTotal * invoice.ExchangeRate), 2);
                headJournal.GrandDocDebitTotal = invoice.GrandTotal;
                headJournal.GrandBaseDebitTotal = Math.Round((decimal)(invoice.GrandTotal * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            }
            headJournal.BaseCurrency = invoice.ExCurrency;
            headJournal.ExchangeRate = invoice.ExchangeRate;
            headJournal.IsGstSettings = invoice.IsGstSettings;
            headJournal.IsGSTApplied = invoice.IsGSTApplied;
            headJournal.IsMultiCurrency = invoice.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = invoice.IsNoSupportingDocument;

            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.Remarks = invoice.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = invoice.CreatedDate;
        }
        private void FillCreditNoteJournal(JVModel headJournal, CreditNoteApplication CreditNoteApplication, bool isNew, bool? isGstSettings, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            decimal? amountDue = 0;
            Invoice invoice = _invoiceEntityService.GetinvoiceById(CreditNoteApplication.InvoiceId);
            //For Analytics Modified by Pradhan
            if (invoice != null)
            {
                var journalByInvoiceId = _journalService.GetJournal(invoice.CompanyId, invoice.Id);
                if (journalByInvoiceId != null)
                {
                    journalByInvoiceId.BalanceAmount = invoice.BalanceAmount - CreditNoteApplication.CreditAmount;
                    amountDue = journalByInvoiceId.BalanceAmount;
                    journalByInvoiceId.ObjectState = ObjectState.Modified;
                    journalByInvoiceId.ModifiedDate = DateTime.UtcNow;
                    journalByInvoiceId.ModifiedBy = "System";
                    _journalService.Update(journalByInvoiceId);
                    JournalDetail jDetail = _journalDetailService.GetDetailByJournalId(journalByInvoiceId.Id);
                    if (jDetail != null)
                    {
                        jDetail.AmountDue = amountDue;
                        jDetail.ObjectState = ObjectState.Modified;
                        _journalDetailService.Update(jDetail);
                    }
                }
            }
            headJournal.COAId = _chartOfAccountService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            //headJournal.COAId = account.Id;
            //headJournal.AccountCode = account.Code;
            int? recOreder = 0;
            //headJournal.AccountName = account.Name;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillCNJV(headJournal, CreditNoteApplication, invoice);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            FillJVHeadCNDetails(jModel, CreditNoteApplication, invoice, lstOfRoundingAmount);
            jModel.AmountDue = amountDue;
            jModel.COAId = headJournal.COAId;
            lstJD.Add(jModel);

            //for Tax LineItem
            long? taxPaybleGstId = null;
            if (isGstSettings == true)
            {
                taxPaybleGstId = _chartOfAccountService.GetChartOfAccountIDByName("Tax payable (GST)", CreditNoteApplication.CompanyId);
            }
            foreach (CreditNoteApplicationDetail detail in CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted))
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillCNDetails(journal, CreditNoteApplication, invoice, detail, lstOfRoundingAmount);
                //DebitNote debitNote = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                //if (detail.DocumentType == DocTypeConstants.Invoice)
                //{
                //    foreach (var invDetail in invoice.InvoiceDetails)
                //    {
                //        journal.AllowDisAllow = invDetail.AllowDisAllow;
                //    }
                //}
                //if (detail.DocumentType == DocTypeConstants.DebitNote)
                //{
                //    foreach (var debitDetail in debitNote.DebitNoteDetails)
                //    {
                //        journal.AllowDisAllow = debitDetail.AllowDisAllow;
                //    }
                //}
                journal.BaseCurrency = invoice.ExCurrency;
                journal.RecOrder = recOreder + 1;
                recOreder = journal.RecOrder;
                lstJD.Add(journal);
                if (CreditNoteApplication.IsRevExcess == true)
                {
                    if (isGstSettings == true && (detail.TaxRate != null && detail.TaxIdCode != "NA"))
                    {
                        JVVDetailModel gstJournal = new JVVDetailModel();
                        if (isNew)
                            gstJournal.Id = Guid.NewGuid();
                        else
                            gstJournal.Id = detail.Id;
                        FillGSTSettings(gstJournal, CreditNoteApplication, invoice, detail, taxPaybleGstId);
                        lstJD.Add(gstJournal);
                    }
                }
                if (CreditNoteApplication.IsRevExcess != true)
                {
                    if (invoice.DocCurrency != invoice.ExCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        journal1.DocType = invoice.DocType;
                        journal1.DocSubType = "Application";
                        journal1.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
                        journal1.AccountDescription = CreditNoteApplication.Remarks;
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.DocumentId = CreditNoteApplication.Id;
                        if (detail.DocumentType == DocTypeConstants.Invoice)
                        {
                            Invoice inv = _invoiceEntityService.GetinvoiceById(detail.DocumentId);
                            if (inv != null)
                            {
                                journal1.Nature = inv.Nature;
                                journal1.EntityId = inv.EntityId;
                                journal1.OffsetDocument = inv.InvoiceNumber;
                                journal1.ExchangeRate = inv.ExchangeRate;
                                //journal1.AmountDue = inv.BalanceAmount;
                            }
                        }
                        if (detail.DocumentType == DocTypeConstants.DebitNote)
                        {
                            DebitNote inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                            if (inv != null)
                            {
                                journal1.Nature = inv.Nature;
                                journal1.EntityId = inv.EntityId;
                                journal1.OffsetDocument = inv.DebitNoteNumber;
                                journal1.ExchangeRate = inv.ExchangeRate;
                                //journal1.AmountDue = inv.BalanceAmount;
                            }
                        }
                        ChartOfAccount account2 = _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == invoice.CompanyId).Select().FirstOrDefault();
                        if (account2 != null)
                        {
                            journal1.COAId = account2.Id;
                        }
                        journal1.DocCurrency = detail.DocCurrency;
                        journal1.BaseCurrency = invoice.ExCurrency;
                        journal1.ServiceCompanyId = invoice != null ? invoice.ServiceCompanyId.Value : 0;
                        journal1.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
                        journal.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
                        journal1.RecOrder = recOreder + 1;
                        recOreder = journal1.RecOrder;
                        if (invoice.ExchangeRate > journal1.ExchangeRate)
                        {
                            journal1.BaseCredit = Math.Round((decimal)(invoice.ExchangeRate - journal1.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (invoice.ExchangeRate < journal1.ExchangeRate)
                        {
                            journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - invoice.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (invoice.ExchangeRate != journal1.ExchangeRate)
                        {
                            lstJD.Add(journal1);
                        }
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.OrderBy(x => x.RecOrder).ToList();
        }

        private void FillGSTSettings(JVVDetailModel jVDetail, CreditNoteApplication creditNoteApplication, Invoice invoice, CreditNoteApplicationDetail CNAppDetail, long? taxPaybleGstId)
        {
            jVDetail.DocumentDetailId = CNAppDetail.Id;
            jVDetail.DocumentId = creditNoteApplication.Id;
            jVDetail.Nature = invoice.Nature;
            jVDetail.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jVDetail.DocNo = creditNoteApplication.CreditNoteApplicationNumber;
            jVDetail.DocType = DocTypeConstants.CreditNote;
            jVDetail.DocSubType = "Application";
            jVDetail.PostingDate = creditNoteApplication.CreditNoteApplicationDate;
            jVDetail.EntityId = creditNoteApplication.Invoice.EntityId;
            //ChartOfAccount account2 = _chartOfAccountService.Query(a => a.CompanyId == invoice.CompanyId && a.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();
            if (taxPaybleGstId == null)
            {
                taxPaybleGstId = _chartOfAccountService.GetChartOfAccountIDByName("Tax payable (GST)", creditNoteApplication.CompanyId);
            }
            jVDetail.COAId = taxPaybleGstId.Value;
            jVDetail.DocCurrency = CNAppDetail.DocCurrency;
            jVDetail.BaseCurrency = invoice.ExCurrency;
            jVDetail.ExchangeRate = invoice.ExchangeRate;
            jVDetail.GSTExCurrency = invoice.GSTExCurrency;
            jVDetail.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            jVDetail.GSTExchangeRate = invoice.GSTExchangeRate;
            //jVDetail.AccountDescription = CNAppDetail.DocDescription;
            jVDetail.AccountDescription = creditNoteApplication.Remarks;
            if (CNAppDetail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                //TaxCode tax = lstTaxCodes.Where(a => a.Id == detail.TaxId).FirstOrDefault();
                jVDetail.TaxId = CNAppDetail.TaxId;
                jVDetail.TaxCode = CNAppDetail.TaxIdCode;
                jVDetail.TaxRate = CNAppDetail.TaxRate;
            }
            jVDetail.DocCredit = CNAppDetail.TaxAmount;
            jVDetail.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)jVDetail.DocCredit : (decimal)(jVDetail.DocCredit * invoice.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            jVDetail.IsTax = true;
        }

        private void FillCNDetails(JVVDetailModel journal, CreditNoteApplication CreditNoteApplication, Invoice invoice, CreditNoteApplicationDetail detail, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            Guid? docId = Guid.Empty;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = CreditNoteApplication.Id;
            journal.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            if (detail.DocumentType == DocTypeConstants.Invoice)
            {
                var inv = _invoiceEntityService.GetinvoiceById(detail.DocumentId);
                if (inv != null)
                {
                    journal.Nature = inv.Nature;
                    journal.EntityId = inv.EntityId;
                    journal.OffsetDocument = inv.InvoiceNumber;
                    journal.ExchangeRate = inv.ExchangeRate;
                    docId = inv.Id;
                    //journal.DocDate = inv.DocDate;
                    //journal.DocNo = inv.DocNo;
                    //journal.SegmentCategory1 = inv.SegmentCategory1;
                    //journal.SegmentCategory2 = inv.SegmentCategory2;
                    //journal.SegmentMasterid1 = inv.SegmentMasterid1;
                    //journal.SegmentMasterid2 = inv.SegmentMasterid2;
                    //journal.SegmentDetailid1 = inv.SegmentDetailid1;
                    ////journal.AmountDue = inv.BalanceAmount;
                    //journal.SegmentDetailid2 = inv.SegmentDetailid2;
                }
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                var inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                if (inv != null)
                {
                    journal.Nature = inv.Nature;
                    journal.EntityId = inv.EntityId;
                    journal.OffsetDocument = inv.DebitNoteNumber;
                    journal.ExchangeRate = inv.ExchangeRate;
                    docId = inv.Id;
                    //journal.DocDate = inv.DocDate;
                    //journal.DocNo = inv.DocNo;
                    //journal.SegmentCategory1 = inv.SegmentCategory1;
                    //journal.SegmentCategory2 = inv.SegmentCategory2;
                    //journal.SegmentMasterid1 = inv.SegmentMasterid1;
                    //journal.SegmentMasterid2 = inv.SegmentMasterid2;
                    //journal.SegmentDetailid1 = inv.SegmentDetailid1;
                    //journal.SegmentDetailid2 = inv.SegmentDetailid2;
                    //journal.AmountDue = inv.BalanceAmount;
                }
            }
            if (CreditNoteApplication.IsRevExcess != true)
            {
                ChartOfAccount account1 = _chartOfAccountService.Query(a => a.Name == (journal.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables) && a.CompanyId == invoice.CompanyId).Select().FirstOrDefault();
                if (account1 != null)
                {
                    journal.COAId = account1.Id;
                }
                //journal.ExchangeRate = CreditNoteApplication.ExchangeRate;
            }
            else
            {
                journal.COAId = detail.COAId.Value;
                journal.ExchangeRate = CreditNoteApplication.ExchangeRate;
                journal.EntityId = invoice.EntityId;
            }
            journal.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            journal.SettlementMode = "CN Application";
            journal.SettlementRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            journal.SettlementDate = CreditNoteApplication.CreatedDate;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            //journal.ExchangeRate = CreditNoteApplication.IsRevExcess == true ? CreditNoteApplication.ExchangeRate : journal.ExchangeRate;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            //journal.DocType = detail.DocumentType;
            //journal.DocSubType = invoice.DocSubType;
            journal.DocType = invoice.DocType;
            journal.DocSubType = "Application";
            //journal.DocDescription = CreditNoteApplication.Remarks;
            journal.AccountDescription = CreditNoteApplication.Remarks;
            //journal.ExchangeRate = invoice.ExchangeRate;
            journal.DocCredit = detail.CreditAmount;
            //journal.BaseCredit = Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);//commented on 05-02-2020

            journal.BaseCredit = (invoice.DocCurrency != invoice.ExCurrency && lstOfRoundingAmount.Where(a => a.Key == docId).Any()) ? Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero) - lstOfRoundingAmount.Where(a => a.Key == docId).Select(a => a.Value).FirstOrDefault() : Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);

            journal.DocCreditTotal = detail.CreditAmount;

        }

        private void FillJVHeadCNDetails(JVVDetailModel jModel, CreditNoteApplication CreditNoteApplication, Invoice invoice, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            jModel.DocumentId = CreditNoteApplication.Id;
            jModel.SystemRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocType = DocTypeConstants.CreditNote;
            jModel.DocSubType = "Application";
            //jModel.DocSubType = invoice.DocSubType;
            //jModel.DocDescription = CreditNoteApplication.Remarks;
            jModel.AccountDescription = CreditNoteApplication.Remarks;
            jModel.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            jModel.DocDebit = CreditNoteApplication.CreditAmount;
            //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocDebit) : (decimal)jModel.DocDebit, 2, MidpointRounding.AwayFromZero);//commented on 05-02-2020

            jModel.BaseDebit = (invoice.DocumentState == CreditNoteState.FullyApplied && invoice.DocCurrency != invoice.ExCurrency && lstOfRoundingAmount.Where(a => a.Key == invoice.Id).Any()) ? Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocDebit) : (decimal)jModel.DocDebit, 2, MidpointRounding.AwayFromZero) - lstOfRoundingAmount.Where(a => a.Key == invoice.Id).Select(a => a.Value).FirstOrDefault() : Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocDebit) : (decimal)jModel.DocDebit, 2, MidpointRounding.AwayFromZero);

            //jModel.SegmentCategory1 = invoice.SegmentCategory1;
            //jModel.SegmentCategory2 = invoice.SegmentCategory2;
            //jModel.SegmentMasterid1 = invoice.SegmentMasterid1;
            //jModel.SegmentMasterid2 = invoice.SegmentMasterid2;
            //jModel.SegmentDetailid1 = invoice.SegmentDetailid1;
            //jModel.SegmentDetailid2 = invoice.SegmentDetailid2;
            jModel.SettlementMode = "CN Application";
            jModel.SettlementRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.OffsetDocument = invoice.InvoiceNumber;
        }

        private void FillCNJV(JVModel headJournal, CreditNoteApplication CreditNoteApplication, Invoice invoice)
        {
            string strServiceCompany = _companyService.GetIdBy(invoice.ServiceCompanyId.Value);
            TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice.CreditTermsId);
            headJournal.DocumentId = CreditNoteApplication.Id;
            //headJournal.DocumentId = invoice.Id;
            headJournal.ParentId = CreditNoteApplication.InvoiceId;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            headJournal.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            headJournal.DocumentDescription = CreditNoteApplication.Remarks;
            headJournal.DocType = DocTypeConstants.CreditNote;
            headJournal.DocSubType = "Application";
            headJournal.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            headJournal.DueDate = invoice.DueDate.Value;
            headJournal.DocumentState = CreditNoteApplication.Status.ToString();
            headJournal.SystemReferenceNo = CreditNoteApplication.CreditNoteApplicationNumber;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = invoice.Nature;
            headJournal.IsGstSettings = invoice.IsGstSettings;
            headJournal.IsGSTApplied = invoice.IsGSTApplied;
            headJournal.IsMultiCurrency = invoice.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = CreditNoteApplication.IsNoSupportingDocument;
            headJournal.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
            headJournal.PONo = invoice.PONo;
            headJournal.ExDurationFrom = invoice.ExDurationFrom;
            headJournal.ExDurationTo = invoice.ExDurationTo;
            headJournal.GSTExDurationFrom = invoice.GSTExDurationFrom;
            headJournal.GSTExDurationTo = invoice.GSTExDurationTo;
            headJournal.CreditTerms = top != null ? (int)top.TOPValue : (int?)null;
            headJournal.IsSegmentReporting = invoice.IsSegmentReporting;
            headJournal.SegmentCategory1 = invoice.SegmentCategory1;
            headJournal.SegmentMasterid1 = invoice.SegmentMasterid1;
            headJournal.SegmentCategory2 = invoice.SegmentCategory2;
            headJournal.SegmentMasterid2 = invoice.SegmentMasterid2;
            headJournal.SegmentDetailid1 = invoice.SegmentDetailid1;
            headJournal.SegmentDetailid2 = invoice.SegmentDetailid2;
            headJournal.NoSupportingDocument = CreditNoteApplication.IsNoSupportingDocumentActivated;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            BeanEntity entity = _beanEntityService.GetEntityById(invoice.EntityId);
            headJournal.EntityName = entity.Name;
            headJournal.EntityType = invoice.EntityType;
            headJournal.CreditTermsId = invoice.CreditTermsId;
            headJournal.DocCurrency = invoice.DocCurrency;
            headJournal.BaseCurrency = invoice.ExCurrency;
            headJournal.ExchangeRate = invoice.ExchangeRate;
            headJournal.GrandDocDebitTotal = CreditNoteApplication.CreditAmount;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(CreditNoteApplication.CreditAmount * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.Remarks = CreditNoteApplication.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = invoice.CreatedDate;
            headJournal.ModifiedBy = invoice.ModifiedBy;
            headJournal.ModifiedDate = invoice.ModifiedDate;
        }
        private void FillDoubtFulDebtJournal(JVModel headJournal, Invoice invoice, bool isNew, string type)
        {
            FillDDJV(headJournal, invoice, type);
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            FillDoubtfulDebtPosting(jModel, invoice, true);
            lstJD.Add(jModel);
            jModel = new JVVDetailModel();
            FillDoubtfulDebtPosting(jModel, invoice, false);
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD;
        }
        private void FillDDJV(JVModel headJournal, Invoice invoice, string type)
        {
            string strServiceCompany = _companyService.GetIdBy(invoice.ServiceCompanyId.Value);
            headJournal.DocumentId = invoice.Id;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = invoice.DocDate;
            headJournal.DocNo = invoice.DocNo;
            if (type == DocTypeConstants.DoubtFulDebitNote)
                headJournal.DocType = DocTypeConstants.DoubtFulDebitNote;
            headJournal.DocSubType = invoice.DocSubType;
            headJournal.DocDate = invoice.DocDate;
            headJournal.DueDate = invoice.DueDate;
            headJournal.DocumentState = invoice.DocumentState;
            headJournal.SystemReferenceNo = invoice.InvoiceNumber;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = invoice.Nature;
            headJournal.PONo = invoice.PONo;
            headJournal.ExDurationFrom = invoice.ExDurationFrom;
            headJournal.ExDurationTo = invoice.ExDurationTo;
            //headJournal.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
            headJournal.GSTExDurationFrom = invoice.GSTExDurationFrom;
            headJournal.GSTExDurationTo = invoice.GSTExDurationTo;
            //headJournal.IsSegmentReporting = invoice.IsSegmentReporting;
            //headJournal.SegmentCategory1 = invoice.SegmentCategory1;
            //headJournal.SegmentCategory2 = invoice.SegmentCategory2;
            //headJournal.SegmentMasterid1 = invoice.SegmentMasterid1;
            //headJournal.SegmentMasterid2 = invoice.SegmentMasterid2;
            //headJournal.SegmentDetailid1 = invoice.SegmentDetailid1;
            //headJournal.SegmentDetailid2 = invoice.SegmentDetailid2;
            headJournal.NoSupportingDocument = invoice.NoSupportingDocs;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            //BeanEntity entity = _beanEntityService.GetEntityById(invoice.EntityId);
            //headJournal.EntityName = entity.Name;
            //headJournal.EntityType = invoice.EntityType;
            headJournal.IsRepeatingInvoice = invoice.IsRepeatingInvoice;
            headJournal.RepEveryPeriod = invoice.RepEveryPeriod;
            headJournal.RepEveryPeriodNo = invoice.RepEveryPeriodNo;
            headJournal.EndDate = invoice.RepEndDate;
            headJournal.COAId = _chartOfAccountService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            //headJournal.COAId = account.Id;
            //headJournal.AccountCode = account.Code;
            //headJournal.AccountName = account.Name;
            headJournal.DocCurrency = invoice.DocCurrency;
            headJournal.GrandDocDebitTotal = invoice.GrandTotal;
            headJournal.BalanceAmount = invoice.BalanceAmount;
            headJournal.BaseCurrency = invoice.ExCurrency;
            headJournal.ExchangeRate = invoice.ExchangeRate;
            headJournal.IsGstSettings = invoice.IsGstSettings;
            headJournal.IsGSTApplied = invoice.IsGSTApplied;
            headJournal.IsMultiCurrency = invoice.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = invoice.IsNoSupportingDocument;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(invoice.GrandTotal * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.DocumentDescription = invoice.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = invoice.CreatedDate == DateTime.UtcNow ? DateTime.UtcNow : invoice.CreatedDate;
            headJournal.ModifiedBy = invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated;
            headJournal.ModifiedDate = invoice.ModifiedDate;
        }
        private void FillDoubtfulDebtPosting(JVVDetailModel jModel, Invoice invoice, bool isOriginal)
        {
            jModel.DocumentId = invoice.Id;
            jModel.SystemRefNo = invoice.InvoiceNumber;
            jModel.DocNo = invoice.DocNo;
            jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocSubType = invoice.DocSubType;
            //jModel.Remarks = invoice.Remarks;
            ChartOfAccount account = new ChartOfAccount();
            if (isOriginal)
                account = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.Doubtful_Debt_expense, invoice.CompanyId);
            else
                account = _chartOfAccountService.GetChartOfAccountByName(invoice.Nature == "Trade" ? COANameConstants.Doubtful_Debt_Provision_AR : COANameConstants.Doubtful_Debt_Provision_OR, invoice.CompanyId);
            if (account != null)
            {
                jModel.COAId = account.Id;
                jModel.AccountCode = account.Code;
                jModel.AccountName = account.Name;
            }
            jModel.PONo = invoice.PONo;
            jModel.CreditTermsId = invoice.CreditTermsId;
            jModel.DueDate = invoice.DueDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            jModel.AccountDescription = invoice.DocDescription;
            if (isOriginal)
            {
                jModel.DocDebit = invoice.GrandTotal;
                jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocDebit : (decimal)(jModel.DocDebit * jModel.ExchangeRate), 2);
            }
            else
            {
                jModel.DocCredit = invoice.GrandTotal;
                jModel.BaseCredit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocCredit : (decimal)(jModel.DocCredit * jModel.ExchangeRate), 2);
            }
            //jModel.SegmentCategory1 = invoice.SegmentCategory1;
            //jModel.SegmentCategory2 = invoice.SegmentCategory2;
            //jModel.SegmentMasterid1 = invoice.SegmentMasterid1;
            //jModel.SegmentMasterid2 = invoice.SegmentMasterid2;
            //jModel.SegmentDetailid1 = invoice.SegmentDetailid1;
            //jModel.SegmentDetailid2 = invoice.SegmentDetailid2;
            jModel.BaseAmount = invoice.BalanceAmount;
            jModel.DocDate = invoice.DocDate;
            jModel.PostingDate = invoice.DocDate;
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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;
                //url = "http://localhost:57584/";
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);

                if (response.ErrorMessage != null)
                {

                }
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }
        public void deleteJVPostInvoce(JournalSaveModel tObject)
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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                if (response.ErrorMessage != null)
                {
                    // Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {


                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityBean)
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
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        #endregion

        #region WfBeanSyncing
        public void WorkflowInvoicePosting(InvoiceVM invoiceModel)
        {
            var json = RestSharpHelper.ConvertObjectToJason(invoiceModel);
            try
            {
                string url = ConfigurationManager.AppSettings["WorkflowUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityWorkflow)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                //object obj = upmodel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/Invoice/savebeaninvoice", json);
                if (response.ErrorMessage != null)
                {
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region WfInvoiceVoid
        public string SaveWorkflowInvoiceVoid(InvoiceVM TObject)
        {
            string DocNo = "-V";
            string DocState = "Void";
            Invoice invoice = _invoiceEntityService.GetInvoiceByIdAndDocumentId(TObject.Id, TObject.CompanyId);
            if (invoice == null)
            {
                throw new Exception("Invalid invoiceId");
            }
            if (invoice != null)
            {
                if (invoice.DocumentState != InvoiceConstants.Not_Paid)
                {
                    throw new Exception("State Should be " + InvoiceConstants.Not_Paid);
                }
                invoice.DocumentState = DocState;
                invoice.ModifiedBy = TObject.ModifiedBy;
                invoice.ModifiedDate = TObject.ModifiedDate;
                invoice.DocNo = invoice.DocNo + DocNo;
                invoice.InternalState = InvoiceState.Void;
                invoice.ObjectState = ObjectState.Modified;

                try
                {
                    JournalSaveModel tObject = new JournalSaveModel();
                    tObject.Id = invoice.Id;
                    tObject.CompanyId = TObject.CompanyId;
                    tObject.DocNo = invoice.DocNo;
                    tObject.ModifiedBy = invoice.ModifiedBy;
                    deleteJVPostInvoce(tObject);

                }
                catch (Exception ex)
                {
                    //Log.Logger.ZCritical(ex.StackTrace);
                    //throw ex;
                    throw ex;
                }
            }
            unitOfWork.SaveChanges();
            return "Invoice Void Successfully.";
        }
        #endregion

        #region Recurring_Invoice
        public InvoiceDocModel GetRecurrInvoiceDocNo(Guid id, long companyId, DateTime docDate, DateTime? endDate, string docNo, int frequencyValue)
        {
            bool isSystemDate = false;
            DateTime systemDate = DateTime.UtcNow;
            DateTime lstDocDate = new DateTime();
            bool isDocNoExists = false;

            string documentNo = null;
            int? counter = 1;
            int count = 0;
            int i = 0;

            if (_invoiceEntityService.GetRecurringDocNo(companyId, id, InvoiceState.Recurring, docNo))
            {
                throw new Exception(InvoiceValidation.Document_number_already_exist);
            }

            InvoiceDocModel invoiceDocModel = new InvoiceDocModel();
            List<InvoiceDocNoModel> lstInvoiceDocno = new List<InvoiceDocNoModel>();
            List<InvoiceDocNoModel> lstInvoiceDocno1 = new List<InvoiceDocNoModel>();

            List<string> lstPotedDocNo = _invoiceEntityService.GetListofPotedDocNo(companyId, InvoiceState.Posted);
            Invoice invoice = _invoiceEntityService.GetInvoiceByRecurId(companyId, id);
            if (invoice != null)
            {
                if (invoice.Counter != null)
                {
                    //return invoiceDocModel;

                    //docDate = invoice.DocumentState == InvoiceState.Parked ? invoice.ReverseDate.Value : invoice.DocDate;
                    docDate = invoice.ReverseDate.Value;
                    counter = invoice.Counter;
                    int num = 0;
                    while (isSystemDate == false)
                    {
                        counter++;
                        InvoiceDocNoModel journalDocModel = new InvoiceDocNoModel();
                        num++;
                        lstDocDate = docDate.AddMonths(num * frequencyValue);
                        if (endDate >= lstDocDate || endDate == null)
                        {
                            if (systemDate >= lstDocDate)
                            {
                                documentNo = null;
                                journalDocModel.DocDate = lstDocDate;
                                documentNo = docNo + "-" + counter;
                                isDocNoExists = lstPotedDocNo.Where(a => a.Equals(documentNo)).Any();
                                while (isDocNoExists)
                                {
                                    i++;
                                    documentNo = null;
                                    documentNo = docNo + "-" + counter + "-" + i;
                                    if (isDocNoExists == lstPotedDocNo.Where(a => a.Equals(documentNo)).Any() == false)
                                        isDocNoExists = false;
                                }
                                journalDocModel.DocNo = documentNo;
                                invoiceDocModel.NextDue = null;
                                //docDate = lstDocDate;
                                lstInvoiceDocno.Add(journalDocModel);
                            }
                            else
                            {
                                isSystemDate = true;
                                invoiceDocModel.NextDue = lstDocDate;
                                //lstJournalDocno.Add(journalDocModel);
                            }
                        }
                        else
                        {
                            isSystemDate = true;
                            invoiceDocModel.NextDue = null;
                            //lstJournalDocno.Add(journalDocModel);
                        }
                        //jDModel.JournalDocModels.AddRange(lstJournalDocno);
                    }
                    //return lstJournalDocno.Where(a => a.DocDate != null).OrderBy(a => a.DocDate).ToList();
                    invoiceDocModel.JournalDocModels.AddRange(lstInvoiceDocno);
                    invoiceDocModel.JournalDocModels.Where(a => a.DocDate != null).OrderBy(a => a.DocDate).ToList();
                }
                else
                {
                    invoiceDocModel.NextDue = invoice.NextDue;
                }
                return invoiceDocModel;
            }
            else
            {
                if (docDate.Date > DateTime.UtcNow.Date)
                {
                    invoiceDocModel.NextDue = docDate;
                }
                else
                {
                    InvoiceDocNoModel invoiceDocModel1 = new InvoiceDocNoModel();
                    invoiceDocModel1.DocDate = docDate;
                    documentNo = docNo + "-" + 1;
                    isDocNoExists = lstPotedDocNo.Where(a => a.Equals(documentNo)).Any();
                    while (isDocNoExists)
                    {
                        i++;
                        documentNo = null;
                        documentNo = docNo + "-" + 1 + "-" + i;
                        if (isDocNoExists == lstPotedDocNo.Where(a => a.Equals(documentNo)).Any() == false)
                            isDocNoExists = false;
                    }
                    invoiceDocModel1.DocNo = documentNo;

                    while (isSystemDate == false)
                    {
                        InvoiceDocNoModel invoiceDocNoModel = new InvoiceDocNoModel();
                        i = 0;
                        counter++;
                        count++;
                        lstDocDate = docDate.AddMonths(count * frequencyValue);
                        if (endDate >= lstDocDate || endDate == null)
                        {
                            if (systemDate >= lstDocDate)
                            {
                                documentNo = null;
                                invoiceDocNoModel.DocDate = lstDocDate;
                                documentNo = docNo + "-" + counter;
                                isDocNoExists = lstPotedDocNo.Where(a => a.Equals(documentNo)).Any();
                                while (isDocNoExists)
                                {
                                    i++;
                                    documentNo = null;
                                    documentNo = docNo + "-" + counter + "-" + i;
                                    if (isDocNoExists == lstPotedDocNo.Where(a => a.Equals(documentNo)).Any() == false)
                                        isDocNoExists = false;
                                }
                                invoiceDocNoModel.DocNo = documentNo;
                                invoiceDocModel.NextDue = null;
                                //docDate = lstDocDate;
                                lstInvoiceDocno.Add(invoiceDocNoModel);
                            }
                            else
                            {
                                isSystemDate = true;
                                invoiceDocModel.NextDue = lstDocDate;
                            }
                        }
                        else
                        {
                            isSystemDate = true;
                            invoiceDocModel.NextDue = null;
                        }
                    }
                    lstInvoiceDocno1.Add(invoiceDocModel1);
                    invoiceDocModel.JournalDocModels.AddRange(lstInvoiceDocno1);
                    invoiceDocModel.JournalDocModels.AddRange(lstInvoiceDocno);
                }

            }
            invoiceDocModel.JournalDocModels.OrderBy(a => a.DocDate).ToList();
            return invoiceDocModel;
        }

        public RecurringModel CreateRecurringInvoice(long companyid, Guid id, string connectionString)
        {
            RecurringModel invDTO = new RecurringModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyid);
                if (financSettings == null)
                {
                    throw new Exception(InvoiceValidation.The_Financial_setting_should_be_activated);
                }
                //AppsWorld.InvoiceModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyid, DocTypeConstants.Invoice);
                Invoice invoice = _invoiceEntityService.GetCompanyAndId(companyid, id);
                if (invoice != null)
                {
                    FillRecurringInvoice(invDTO, invoice);
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstant.Invoice);
                    invDTO.IsLocked = invoice.IsLocked;
                }
                else
                {
                    Invoice lastInvoice = _invoiceEntityService.GetByCompanyIdForInvoiceWithDocSubType(companyid, "Invoice", "Recurring");
                    invDTO.Id = Guid.NewGuid();
                    invDTO.CompanyId = companyid;
                    invDTO.DocDate = lastInvoice == null ? DateTime.Now : lastInvoice.DocDate;
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstant.Invoice);
                    //invDTO.DocNo = _autoService.GetAutonumber(companyid, DocTypeConstant.Invoice, connectionString);

                    //Docno Block
                    if (invDTO.IsDocNoEditable == true)
                        invDTO.DocNo = GetRecInvoiceDocumentNumber(DocTypeConstants.Invoice, InvoiceState.Recurring, companyid);
                    else
                        invDTO.DocNo = _autoService.GetAutonumberInAddMode(companyid, DocTypeConstant.Invoice, connectionString);

                    //bool? isEdit = false;
                    //invDTO.DocNo = GetAutoNumberByEntityType(companyid, lastInvoice, InvoiceState.Recurring, _autoNo, false, ref isEdit);
                    //invDTO.IsDocNoEditable = isEdit;


                    //invDTO.DocNo = GetRecInvoiceDocumentNumber(DocTypeConstants.Invoice, InvoiceState.Recurring, companyid);
                    invDTO.DocumentState = "Not Paid";
                    invDTO.DueDate = DateTime.UtcNow;
                    invDTO.NoSupportingDocument = false;
                    invDTO.IsRepeatingInvoice = false;
                    invDTO.CreatedDate = DateTime.UtcNow;
                    invDTO.BaseCurrency = financSettings.BaseCurrency;
                    invDTO.DocCurrency = invDTO.BaseCurrency;
                    invDTO.InternalState = InvoiceState.Recurring;
                    //Forex forexBean;
                    decimal? rate = 0;
                    //forexBean = _forexService.GetMultiCurrencyInformation(invDTO.BaseCurrency, invDTO.DocDate, true, invDTO.CompanyId);
                    //if (forexBean != null)
                    //{
                    //    invDTO.ExchangeRate = forexBean.UnitPerUSD;
                    //    invDTO.ExDurationFrom = forexBean.DateFrom;
                    //    invDTO.ExDurationTo = forexBean.Dateto;
                    //}
                    invDTO.IsGstSettings = false;
                    invDTO.IsBaseCurrencyRateChanged = false;
                    invDTO.IsGSTCurrencyRateChanged = false;
                    invDTO.IsLocked = false;
                }
                invDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyid);
                invDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyid);
                invDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyid);
                invDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyid);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return invDTO;
        }


        private string GetRecInvoiceDocumentNumber(string docType, string internalStatel, long companyId)
        {
            Invoice invoice = _invoiceEntityService.GetRecInvoiceByIStateAndCId(DocTypeConstants.Invoice, InvoiceState.Recurring, companyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
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

                    duplicatInvoice = _invoiceEntityService.GetAllInvovoice(strNewDocNo, docType, companyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }


        public Invoice SaveRecurringInvoice(InvoiceModel TObject, string connectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "ObjectSave", AdditionalInfo);
            bool isEdit = false;
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_Request_Message);
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            //to check if it is void or not
            if (_invoiceEntityService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

            if (TObject.EntityId == null)
            {
                throw new Exception(InvoiceValidation.Entity_is_mandatory);
            }

            if (TObject.DocDate == null)
            {
                throw new Exception(InvoiceValidation.Invalid_Document_Date);
            }

            if (TObject.ServiceCompanyId == null)
                throw new Exception(InvoiceValidation.Service_Company_Is_Mandatory);

            if (TObject.DueDate == null || TObject.DueDate < TObject.DocDate)
            {
                throw new Exception(InvoiceValidation.Invalid_Due_Date);
            }

            if (TObject.CreditTermsId == null)
            {
                throw new Exception(InvoiceValidation.Terms_Payment_is_mandatory);
            }

            //if (IsDocumentNumberExists(DocTypeConstants.Invoice, TObject.DocNo, TObject.Id, TObject.CompanyId))
            //{
            //    throw new Exception(InvoiceValidation.Document_number_already_exist);
            //}
            if (TObject.IsDocNoEditable == true)
                if (_invoiceEntityService.GetRecurringDocNo(TObject.CompanyId, TObject.Id, InvoiceState.Recurring, TObject.DocNo))
                {
                    throw new Exception(InvoiceValidation.Document_number_already_exist);
                }

            if (TObject.GrandTotal < 0)
            {
                throw new Exception(InvoiceValidation.Grand_Total_should_be_greater_than_zero);
            }
            if (TObject.IsPosted != true)
                if (TObject.InvoiceDetails.Any())
                {
                    foreach (var invoice in TObject.InvoiceDetails)
                    {
                        if (invoice.ItemCode != null && invoice.Qty == null)
                            throw new Exception(InvoiceValidation.Please_Enter_Quantity);
                        if (invoice.ItemCode == null && invoice.Qty != null)
                            throw new Exception(InvoiceValidation.Please_Select_Item);
                    }
                }
            if (TObject.CreditTermsId == null || TObject.CreditTermsId == 0)
            {
                throw new Exception(InvoiceValidation.Terms_of_Payment_is_mandatory);
            }
            //if (TObject.SegmentMasterid1 == null && TObject.SegmentMasterid2 == null)
            //{

            //}
            //else if (TObject.SegmentMasterid1 == TObject.SegmentMasterid2)
            //{
            //    throw new Exception(InvoiceValidation.Segments_duplicates_cannot_be_allowed);
            //}

            if (TObject.InvoiceDetails == null || TObject.InvoiceDetails.Count == 0)
            {
                throw new Exception(InvoiceValidation.Atleast_one_Sale_Item_is_required_in_the_Invoice);
            }
            else
            {
                int itemCount = TObject.InvoiceDetails.Where(a => a.RecordStatus != "Deleted").Count();
                if (itemCount == 0)
                {
                    throw new Exception(InvoiceValidation.Atleast_one_Sale_Item_is_required_in_the_Invoice);
                }
            }

            if (TObject.ExchangeRate == 0)
                throw new Exception(InvoiceValidation.ExchangeRate_Should_Be_Grater_Than_Zero);

            if (TObject.GSTExchangeRate == 0)
                throw new Exception(InvoiceValidation.GSTExchangeRate_Should_Be_Grater_Than_Zero);
            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
            {
                throw new Exception(InvoiceValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(InvoiceValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(InvoiceValidation.Invalid_Financial_Period_Lock_Password);
                }
            }
            Invoice _invoice = null;
            _invoice = _invoiceEntityService.GetAllInvoiceByIdDocType(TObject.Id);
            if (_invoice != null)
            {
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_invoice.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            if (TObject.IsDeleted != true)
            {
                if (_invoice != null)
                {
                    DateTime? RecEndDate = _invoice.RepEndDate;
                    if (TObject.EndDate != null)
                    {
                        if (TObject.EndDate != _invoice.ReverseDate)
                            if (TObject.EndDate < _invoice.ReverseDate)
                            {
                                throw new Exception(InvoiceValidation.Cannot_give_EndDate_less_than_Last_Posted_Date);
                            }
                    }

                    isEdit = true;
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Checking_Invoice_is_null_or_not);
                    _invoice.DocNo = TObject.DocNo;
                    _invoice.InvoiceNumber = _invoice.DocNo;
                    _invoice.BalanceAmount = _invoice.BalanceAmount;
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.InsertInvoice_method_came);
                    InsertInvoice(TObject, _invoice);
                    if (_invoice.InternalState == InvoiceState.Parked)
                    {
                        _invoice.InternalState = InvoiceState.Parked;
                        _invoice.ModifiedDate = DateTime.UtcNow;
                        if (TObject.IsPosted == true)
                        {
                            _invoice.InternalState = InvoiceState.Posted;
                            _invoice.UserCreated = "System";
                            _invoice.CreatedDate = DateTime.UtcNow;
                            _invoice.ModifiedBy = null;
                            _invoice.ModifiedDate = null;
                            _invoice.BalanceAmount = TObject.GrandTotal;
                            Invoice recInvoice = TObject.RecurInvId != null ? _invoiceEntityService.GetRecurringMaster(TObject.CompanyId, TObject.RecurInvId.Value) : null;
                            if (recInvoice != null)
                            {
                                recInvoice.LastPostedDate = TObject.DocDate;
                                _invoiceEntityService.Update(recInvoice);
                            }
                        }
                    }
                    else
                    {
                        _invoice.ModifiedBy = TObject.ModifiedBy;
                        _invoice.ModifiedDate = DateTime.UtcNow;
                        _invoice.InternalState = InvoiceState.Recurring;
                        _invoice.IsPost = TObject.IsPost;
                    }
                    _invoice.ModifiedBy = TObject.IsPosted == true ? null : TObject.ModifiedBy;
                    _invoice.DocumentState = TObject.InternalState == InvoiceState.Recurring ? TObject.DocumentState : InvoiceState.NotPaid;
                    _invoice.BalanceAmount = TObject.GrandTotal;
                    _invoice.DocType = DocTypeConstants.Invoice;
                    _invoice.DocSubType = InvoiceState.Recurring;

                    //for Recurring invoice Enddate and nexposted date calculation
                    if (TObject.InternalState != InvoiceState.Parked)
                    {
                        if (TObject.EndDate != RecEndDate)
                        {
                            _invoice.NextDue = TObject.EndDate >= _invoice.NextDue ? _invoice.NextDue : null;
                            _invoice.DocumentState = _invoice.NextDue == null ? InvoiceConstants.Completed : _invoice.DocumentState;
                        }
                    }


                    _invoice.ObjectState = ObjectState.Modified;
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceDetails_method_came);
                    UpdateInvoiceDetails(TObject, _invoice);
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceNotes_method_came);
                    //UpdateInvoiceNotes(TObject, _invoice);
                    //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.UpdateInvoiceGSTDetails_method_came);
                    //if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null)

                    //UpdateInvoiceGSTDetails(TObject, _invoice);

                    _invoiceEntityService.Update(_invoice);
                    //unitOfWork.SaveChanges();

                    //posting not required for Invoice Record
                    if (TObject.IsPosted == true)
                    {
                        JVModel jvm = new JVModel();
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.FillJournal_method_came);
                        FillJournal(jvm, _invoice, false, DocTypeConstants.Invoice);
                        jvm.Id = _invoice.Id;

                        //AppaWorld.Bean.Common.SavePosting(_invoice.CompanyId, _invoice.Id, _invoice.DocType, connectionString);//new parked journal posting

                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.SaveInvoice1_method_came);
                        SaveInvoice1(jvm);
                    }
                }
                else
                {
                    isEdit = false;
                    _invoice = new Invoice();
                    _invoice.Id = Guid.NewGuid();
                    InsertInvoice(TObject, _invoice);
                    _invoice.DocumentState = InvoiceState.NotPaid;
                    _invoice.BalanceAmount = TObject.BalanceAmount;

                    //if (_invoice.BalanceAmount == 0)
                    //    _invoice.DocumentState = InvoiceState.FullyPaid;

                    //_invoice.Id = TObject.IsWorkFlowInvoice == true ? Guid.NewGuid() : TObject.Id;


                    if (TObject.InvoiceDetails.Count > 0 || TObject.InvoiceDetails != null)
                    {
                        UpdateInvoiceDetails(TObject, _invoice);
                    }

                    //if (TObject.InvoiceGSTDetails != null)
                    //{
                    //    _invoice.InvoiceGSTDetails = TObject.InvoiceGSTDetails;
                    //    foreach (InvoiceGSTDetail gstdetail in _invoice.InvoiceGSTDetails)
                    //    {
                    //        gstdetail.InvoiceId = _invoice.Id;
                    //        gstdetail.ObjectState = ObjectState.Added;
                    //    }
                    //}

                    _invoice.Status = TObject.IsDeleted == true ? RecordStatusEnum.Delete : RecordStatusEnum.Active;
                    _invoice.UserCreated = TObject.UserCreated;
                    _invoice.DocNo = TObject.DocNo;
                    _invoice.RecurInvId = _invoice.Id;
                    _invoice.CreatedDate = DateTime.UtcNow;
                    _invoice.InternalState = TObject.IsDeleted == true ? InvoiceState.Deleted : InvoiceState.Recurring;
                    _invoice.IsPost = TObject.IsPost;
                    _invoice.DocType = DocTypeConstants.Invoice;
                    _invoice.DocSubType = InvoiceState.Recurring;
                    //Company company = _companyService.GetCompanyByCompanyid(TObject.CompanyId);
                    _invoice.InvoiceNumber = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstant.Invoice, connectionString) /*GenerateAutoNumberForType(TObject.CompanyId, DocTypeConstants.Invoice, null)*/ : TObject.DocNo;
                    _invoice.DocNo = _invoice.InvoiceNumber;
                    _invoice.ObjectState = ObjectState.Added;
                    _invoiceEntityService.Insert(_invoice);
                    //posting not required for Recurring Invoice
                    //JVModel jvm = new JVModel();
                    //FillJournal(jvm, _invoice, true, DocTypeConstants.Invoice);
                    //jvm.DocumentState = InvoiceStates.NotPaid;
                    //SaveInvoice1(jvm);
                }
            }
            else
            {
                _invoice.Status = RecordStatusEnum.Disable;
                _invoice.InternalState = InvoiceConstants.Deleted;
                _invoice.ObjectState = ObjectState.Modified;
                _invoiceEntityService.Update(_invoice);
            }
            try
            {

                unitOfWork.SaveChanges();
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_SuccessFully_Message);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_Exception_Message);
                //Log.Logger.ZCritical(e.StackTrace);
                //throw e;
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            if (TObject.IsPosted != true && TObject.InternalState == InvoiceState.Recurring && isEdit != true)
            {

                if (TObject.DocDate.Date <= DateTime.UtcNow.Date)
                {
                    GSTSetting gstSetting = _gstSettingService.GetByCompanyId(TObject.CompanyId);
                    List<TaxCode> lstTaxCodes = _taxCodeService.GetTaxCodes(TObject.CompanyId);
                    //ChartOfAccount gstAccount = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.TaxPayableGST, TObject.CompanyId);
                    List<TermsOfPayment> lstTOP = _termsOfPaymentService.GetAllTOPByCid(TObject.CompanyId);
                    int? counter = 0;
                    DateTime? nextPostedDate = new DateTime();
                    int? count = 0;
                    DateTime? docDate = TObject.DocDate;
                    if (!isEdit)
                    {
                        PostInvFromRecurring(TObject, _invoice, lstTOP, gstSetting, connectionString);
                        counter = 1;
                        //docDate = TObject.DocDate;
                    }
                    else
                    {
                        counter = _invoice.Counter;
                        docDate = _invoice.ReverseDate;
                        //count = 0;
                        //nextPostedDate = _invoice.ReverseDate;
                        //docDate = _invoice.LastPostedDate;
                    }
                    if (TObject.InternalState == InvoiceState.Recurring)
                    {
                        bool isFalse = false;
                        while (isFalse == false)
                        {
                            count++;
                            nextPostedDate = docDate.Value.AddMonths(count.Value * TObject.RepEveryPeriodNo.Value);
                            if (TObject.EndDate >= nextPostedDate || TObject.EndDate == null)
                            {
                                if (DateTime.UtcNow >= nextPostedDate)
                                {
                                    counter++;
                                    SavePostedFromRecurring(TObject, gstSetting, lstTaxCodes, nextPostedDate, _invoice, lstTOP, counter, connectionString);
                                    //docDate = nextPostedDate;
                                }
                                else
                                {
                                    _invoice.NextDue = nextPostedDate;
                                    _invoice.DocumentState = InvoiceConstants.InProgress;
                                    _invoice.LastPostedDate = nextPostedDate.Value.AddMonths(-TObject.RepEveryPeriodNo.Value);
                                    _invoice.ReverseDate = _invoice.LastPostedDate;
                                    _invoice.Counter = counter;
                                    isFalse = true;
                                }
                            }
                            else
                            {
                                _invoice.NextDue = null;
                                _invoice.DocumentState = InvoiceConstants.Completed;
                                _invoice.LastPostedDate = nextPostedDate.Value.AddMonths(-TObject.RepEveryPeriodNo.Value);
                                _invoice.ReverseDate = _invoice.LastPostedDate;
                                _invoice.Counter = counter;
                                isFalse = true;
                            }
                        }
                        try
                        {
                            _invoiceEntityService.Update(_invoice);
                            unitOfWork.SaveChanges();
                            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_SuccessFully_Message);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                else
                {
                    _invoice.NextDue = TObject.DocDate;
                    _invoice.DocumentState = InvoiceConstants.InProgress;
                    _invoice.ReverseDate = _invoice.NextDue;
                    _invoiceEntityService.Update(_invoice);
                    unitOfWork.SaveChanges();
                }
            }
            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_invoice.Id, _invoice.CompanyId, _invoice.Id, _invoice.DocType, _invoice.DocSubType, _invoice.DocumentState, _invoice.DocCurrency, _invoice.GrandTotal, _invoice.BalanceAmount, _invoice.ExchangeRate.Value, _invoice.ModifiedBy != null ? _invoice.ModifiedBy : _invoice.UserCreated, _invoice.Remarks, _invoice.DocDate, 0, 0);

                if (lstdocumet.Any())
                    AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, connectionString);
            }
            catch (Exception ex)
            {

            }
            #endregion Documentary History
            return _invoice;
        }
        private void SavePostedFromRecurring(InvoiceModel TObject, GSTSetting gstSetting, List<TaxCode> lstTaxCodes, DateTime? nextPostedDate, Invoice _invoice, List<TermsOfPayment> lstTOP, int? counter, string connectionString)
        {
            Invoice newInvoice = new Invoice();
            try
            {
                newInvoice.Id = Guid.NewGuid();
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.InsertInvoice_method_came);
                InsertRecurringInvoice(TObject, newInvoice, _invoice, nextPostedDate, lstTOP, true, gstSetting);
                //newInvoice.InvoiceNumber = TObject.IsDocNoEditable != true ? GenerateAutoNumberForType(TObject.CompanyId, DocTypeConstants.Invoice, null) : GetPostedInvDocNo(TObject.CompanyId, TObject.DocNo + "-" + counter);
                newInvoice.InvoiceNumber = GetPostedInvDocNo(TObject.CompanyId, _invoice.DocNo + "-" + counter);
                //newInvoice.DocNo = GetPostedInvDocNo(TObject.CompanyId, TObject.DocNo + "-" + counter);
                newInvoice.DocNo = newInvoice.InvoiceNumber;
                newInvoice.RecurInvId = _invoice.Id;
                newInvoice.Counter = counter;
                newInvoice.ObjectState = ObjectState.Added;
                _invoiceEntityService.Insert(newInvoice);

                if (TObject.InvoiceDetails != null)
                {
                    int? recOrder = 0;
                    foreach (var detail in TObject.InvoiceDetails)
                    {
                        try
                        {
                            recOrder++;
                            InvoiceDetail invoiceDetail = new InvoiceDetail();
                            invoiceDetail.Id = Guid.NewGuid();
                            FillInvoiceDetail(newInvoice, TObject, gstSetting, lstTaxCodes, nextPostedDate, invoiceDetail, recOrder, detail);
                            invoiceDetail.ObjectState = ObjectState.Added;
                            //_invoiceDetailService.Insert(invoiceDetail);
                            newInvoice.InvoiceDetails.Add(invoiceDetail);
                        }
                        catch (Exception ex)
                        {
                            LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                        }
                    }
                    //jDetail.Sum(a => a.DocCredit) + jDetail.Sum(a => a.DocTaxCredit);
                    newInvoice.GrandTotal = newInvoice.InvoiceDetails.Any() ? Math.Round(newInvoice.InvoiceDetails.Sum(a => a.DocAmount) + (decimal)newInvoice.InvoiceDetails.Sum(a => a.DocTaxAmount != null ? a.DocTaxAmount : 0), 2, MidpointRounding.AwayFromZero) : TObject.GrandTotal;
                    newInvoice.BalanceAmount = newInvoice.GrandTotal;
                }
                #region Commented_code
                //if (TObject.InvoiceGSTDetails != null)
                //{
                //    foreach (var detail in TObject.InvoiceGSTDetails)
                //    {
                //        try
                //        {
                //            InvoiceGSTDetail gstDetail = new InvoiceGSTDetail();
                //            gstDetail.Id = Guid.NewGuid();
                //            gstDetail.InvoiceId = newInvoice.Id;
                //            fillPostedGstDetail(gstDetail, detail, gstSetting, lstTaxCodes, nextPostedDate);
                //            gstDetail.ObjectState = ObjectState.Added;
                //            //_invoiceGSTDetailService.Insert(gstDetail);
                //            newInvoice.InvoiceGSTDetails.Add(gstDetail);
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //}
                #endregion
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Exited_from_SavePostedFromRecurring_method);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            try
            {
                unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            //posting Region
            //JVModel jvm = new JVModel();
            //FillJournal(jvm, newInvoice, true, DocTypeConstants.Invoice);
            //jvm.DocumentState = InvoiceStates.NotPaid;
            //SaveInvoice1(jvm);

            AppaWorld.Bean.Common.SavePosting(newInvoice.CompanyId, newInvoice.Id, newInvoice.DocType, connectionString);
        }
        private void InsertRecurringInvoice(InvoiceModel TObject, Invoice invoiceNew, Invoice invoice, DateTime? nextPostedDate, List<TermsOfPayment> lstTOP, bool isFirst, GSTSetting gstSetting)
        {
            try
            {
                invoiceNew.CompanyId = TObject.CompanyId;
                invoiceNew.DocType = DocTypeConstants.Invoice;
                invoiceNew.DocSubType = InvoiceState.Recurring;
                invoiceNew.EntityType = "Customer";
                invoiceNew.DocDate = nextPostedDate.Value;
                invoiceNew.CreditTermsId = TObject.CreditTermsId;
                invoiceNew.DueDate = nextPostedDate.Value.AddDays(Convert.ToDouble(lstTOP.Where(a => a.Id == TObject.CreditTermsId).Select(a => a.TOPValue).FirstOrDefault()));
                invoiceNew.PONo = TObject.PONo;
                invoiceNew.EntityId = TObject.EntityId;
                invoiceNew.Nature = TObject.Nature;
                invoiceNew.ServiceCompanyId = TObject.ServiceCompanyId;
                //FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(invoiceNew.CompanyId);
                if (TObject.IsMultiCurrency == true)
                    invoiceNew.DocCurrency = TObject.DocCurrency;
                else
                    invoiceNew.DocCurrency = TObject.BaseCurrency;
                invoiceNew.IsMultiCurrency = TObject.IsMultiCurrency;
                invoiceNew.ExCurrency = TObject.BaseCurrency;
                invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                //Forex forex = _forexService.GetMultiCurrencyInformation(TObject.DocCurrency, nextPostedDate.Value, TObject.BaseCurrency == "SGD" ? true : false, TObject.CompanyId);
                if (isFirst == true)
                {
                    //invoiceNew.ExchangeRate = TObject.DocCurrency == TObject.BaseCurrency ? TObject.ExchangeRate : _financialService.GetExRateInformation(invoiceNew.DocCurrency, invoiceNew.DocDate, TObject.CompanyId);//commented on 01/06/2020

                    invoiceNew.ExchangeRate = TObject.DocCurrency == TObject.BaseCurrency ? TObject.ExchangeRate : GetExRateInformation(TObject.DocCurrency, TObject.BaseCurrency, invoiceNew.DocDate, invoiceNew.CompanyId);


                    if (TObject.IsGstSettings == true)
                    {
                        if (gstSetting.IsDeregistered != true)
                        {
                            invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                            invoiceNew.IsGstSettings = TObject.IsGstSettings;

                            //invoiceNew.GSTExchangeRate = TObject.DocCurrency == TObject.GSTExCurrency ? TObject.GSTExchangeRate : _financialService.GetExRateInformation(invoiceNew.GSTExCurrency, invoiceNew.DocDate, TObject.CompanyId); //commented on 01/06/2020


                            invoiceNew.GSTExchangeRate = TObject.DocCurrency == TObject.GSTExCurrency ? TObject.GSTExchangeRate : GetExRateInformation(TObject.DocCurrency, TObject.GSTExCurrency, invoiceNew.DocDate, invoiceNew.CompanyId);


                        }
                        else
                            invoiceNew.IsGstSettings = false;
                    }
                    else
                        invoiceNew.IsGstSettings = false;
                }
                else
                {
                    invoiceNew.ExchangeRate = TObject.ExchangeRate;
                    //invoiceNew.ExDurationFrom = TObject.ExDurationFrom;
                    //invoiceNew.ExDurationTo = TObject.ExDurationTo;
                    if (TObject.IsGstSettings)
                    {
                        invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                        invoiceNew.IsGstSettings = TObject.IsGstSettings;
                        //invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                        invoiceNew.GSTExchangeRate = TObject.GSTExchangeRate;
                        //invoiceNew.GSTExDurationFrom = /*TObject.BaseCurrency == "SGD" ? forex != null ? forex.DateFrom : TObject.GSTExDurationFrom : forex != null ? forex.DateFrom :*/ TObject.GSTExDurationFrom;
                        //invoiceNew.GSTExDurationTo = /*TObject.BaseCurrency == "SGD" ? forex != null ? forex.Dateto : TObject.GSTExDurationTo : forex != null ? forex.Dateto :*/ TObject.GSTExDurationTo;
                    }
                }

                #region Commented_Code
                //invoiceNew.IsRepeatingInvoice = TObject.IsRepeatingInvoice;
                //if (TObject.IsRepeatingInvoice)
                //{
                //    invoiceNew.RepEveryPeriodNo = TObject.RepEveryPeriodNo;
                //    invoiceNew.RepEveryPeriod = TObject.RepEveryPeriod;
                //    if (TObject.EndDate == null)
                //        invoiceNew.RepEndDate = null;
                //    else
                //        invoiceNew.RepEndDate = TObject.EndDate.Value.Date;
                //    invoiceNew.ParentInvoiceID = TObject.ParentInvoiceID;
                //}
                //else
                //{
                //    invoiceNew.RepEveryPeriodNo = null;
                //    invoiceNew.RepEveryPeriod = null;
                //    invoiceNew.RepEndDate = null;
                //    invoiceNew.ParentInvoiceID = null;
                //}
                #endregion Commented_Code
                invoiceNew.DocDescription = TObject.DocDescription;
                invoiceNew.RepEveryPeriodNo = null;
                invoiceNew.RepEveryPeriod = null;
                invoiceNew.RepEndDate = null;
                invoiceNew.ParentInvoiceID = null;
                invoiceNew.BalanceAmount = TObject.GrandTotal;
                invoiceNew.GrandTotal = TObject.GrandTotal;
                //invoiceNew.GSTTotalAmount = TObject.GrandTotal*;
                invoiceNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;
                invoiceNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                invoiceNew.NoSupportingDocs = TObject.NoSupportingDocument;
                //invoiceNew.SegmentMasterid1 = invoice.SegmentMasterid1;
                //invoiceNew.SegmentDetailid1 = invoice.SegmentDetailid1;
                //invoiceNew.SegmentCategory1 = invoice.SegmentCategory1;
                //invoiceNew.SegmentMasterid2 = invoice.SegmentMasterid2;
                //invoiceNew.SegmentDetailid2 = invoice.SegmentDetailid2;
                //invoiceNew.SegmentCategory2 = invoice.SegmentCategory2;
                invoiceNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                invoiceNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                invoiceNew.Remarks = TObject.Remarks;
                invoiceNew.IsSegmentReporting = TObject.IsSegmentReporting;
                invoiceNew.Status = TObject.Status;
                invoiceNew.DocumentState = InvoiceState.NotPaid;
                invoiceNew.InternalState = InvoiceState.Posted;
                invoiceNew.Status = RecordStatusEnum.Active;
                invoiceNew.CreatedDate = DateTime.UtcNow;
                //invoiceNew.UserCreated = TObject.UserCreated;
                invoiceNew.UserCreated = "System";
                invoiceNew.ModifiedBy = null;
                invoiceNew.ModifiedDate = null;
                invoiceNew.CursorType = TObject.CursorType;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }


        private void FillInvoiceDetail(Invoice _invoiceNew, InvoiceModel TObject, GSTSetting gstSetting, List<TaxCode> lstTaxCodes, DateTime? nextPostedDate, InvoiceDetail invoiceDetail, int? recOrder, InvoiceDetail detail)
        {
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Came_to_FillInvoiceDetail_method);
            invoiceDetail.InvoiceId = _invoiceNew.Id;
            invoiceDetail.ItemId = detail.ItemId;
            invoiceDetail.ItemCode = detail.ItemCode;
            invoiceDetail.ItemDescription = detail.ItemDescription;
            invoiceDetail.Qty = detail.Qty;
            invoiceDetail.Unit = detail.Unit;
            invoiceDetail.UnitPrice = detail.UnitPrice;
            invoiceDetail.DiscountType = detail.DiscountType;
            invoiceDetail.Discount = detail.Discount;
            invoiceDetail.COAId = detail.COAId;
            //invoiceDetail.AllowDisAllow = detail.AllowDisAllow;
            invoiceDetail.DocAmount = detail.DocAmount;
            if (TObject.IsGstSettings && gstSetting.IsDeregistered != true)
            {
                FillGStReporting(gstSetting, lstTaxCodes, invoiceDetail, detail, nextPostedDate, TObject, _invoiceNew);
            }
            invoiceDetail.AmtCurrency = detail.AmtCurrency;
            invoiceDetail.Remarks = detail.Remarks;
            //invoiceDetail.AccountName = detail.AccountName;
            invoiceDetail.RecOrder = recOrder;
            recOrder = invoiceDetail.RecOrder;
            //invoiceDetail.IsPLAccount = detail.IsPLAccount;
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Existed_from_FillInvoiceDetail_method);
        }

        private void FillGStReporting(GSTSetting gstSetting, List<TaxCode> lstTaxCode, InvoiceDetail invoiceDetail, InvoiceDetail oldDetail, DateTime? nextPostedDate, InvoiceModel TObject, Invoice newInvoice)
        {
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Came_to_FillGStReporting_method_for_GST_Calculation);
            if (gstSetting.IsDeregistered == false || gstSetting.IsDeregistered == null)
            {
                TaxCode taxCode = lstTaxCode.Where(a => a.Id == oldDetail.TaxId).FirstOrDefault();
                var taxCodes = lstTaxCode.Where(a => a.Code == taxCode.Code && a.EffectiveFrom <= nextPostedDate && (a.EffectiveTo >= nextPostedDate || a.EffectiveTo == null)).FirstOrDefault();
                if (taxCodes != null)
                {
                    invoiceDetail.TaxId = taxCodes.Id;
                    invoiceDetail.TaxRate = taxCodes.TaxRate;
                    invoiceDetail.TaxIdCode = taxCodes.Code != "NA" ? taxCodes.Code + "-" + taxCodes.TaxRate + (taxCodes.TaxRate != null ? "%" : "NA") /*+ "(" + taxCodes.TaxType[0] + ")"*/ : taxCodes.Code;
                    invoiceDetail.DocAmount = oldDetail.DocAmount;
                    invoiceDetail.DocTaxAmount = (taxCodes.TaxRate != null && taxCodes.TaxRate != 0) ? Math.Round(invoiceDetail.DocAmount * (decimal)taxCodes.TaxRate / 100, 2, MidpointRounding.AwayFromZero) : 0;
                    invoiceDetail.DocTotalAmount = Math.Round(invoiceDetail.DocAmount + (decimal)invoiceDetail.DocTaxAmount, 2, MidpointRounding.AwayFromZero);
                    //invoiceDetail.DocTotalAmount = Math.Round(invoiceDetail.DocAmount + (decimal)invoiceDetail.DocTaxAmount, 2, MidpointRounding.AwayFromZero);
                    invoiceDetail.BaseAmount = newInvoice.ExchangeRate != null ? Math.Round(invoiceDetail.DocAmount * (decimal)newInvoice.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocAmount;
                    //invoiceDetail.BaseTaxAmount = (taxCodes.TaxRate != null && taxCodes.TaxRate != 0) ? Math.Round((decimal)invoiceDetail.BaseAmount * (decimal)taxCodes.TaxRate / 100, 2, MidpointRounding.AwayFromZero) : 0;
                    invoiceDetail.BaseTaxAmount = newInvoice.ExchangeRate != null ? Math.Round((decimal)invoiceDetail.DocTaxAmount * (decimal)newInvoice.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocTaxAmount;
                    invoiceDetail.BaseTotalAmount = Math.Round((decimal)invoiceDetail.BaseAmount + (decimal)invoiceDetail.BaseTaxAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Exist_from_FillGStReporting_method_for_GST_Calculation);
        }
        //private void fillPostedGstDetail(InvoiceGSTDetail invoiceGstDetail, InvoiceGSTDetail oldDetail, GSTSetting gstSetting, List<TaxCode> lstTaxCode, DateTime? nextPostedDate)
        //{
        //    if (gstSetting.IsDeregistered == false || gstSetting.IsDeregistered == null)
        //    {
        //        TaxCode taxCode = lstTaxCode.Where(a => a.Id == oldDetail.TaxId).FirstOrDefault();
        //        var taxCodes = lstTaxCode.Where(a => a.Code == taxCode.Code && a.EffectiveFrom <= nextPostedDate && (a.EffectiveTo >= nextPostedDate || a.EffectiveTo == null)).FirstOrDefault();
        //        if (taxCodes != null)
        //        {
        //            invoiceGstDetail.TaxId = taxCodes.Id;
        //            invoiceGstDetail.Amount = oldDetail.Amount;
        //            invoiceGstDetail.TaxAmount = Math.Round(invoiceGstDetail.Amount * (decimal)taxCodes.TaxRate / 100, 2, MidpointRounding.AwayFromZero);
        //            invoiceGstDetail.TotalAmount = Math.Round(invoiceGstDetail.Amount + invoiceGstDetail.TaxAmount, 2, MidpointRounding.AwayFromZero);
        //        }
        //    }

        //}
        private string GetPostedInvDocNo(long companyId, string recDocNo)
        {
            List<string> lstDocNo = _invoiceEntityService.GetListOfPostedDocNo(companyId, InvoiceState.Posted);
            string documentNo = null;
            string newDocNo = null;
            if (lstDocNo.Any())
            {
                int count = 0;
                string docNo = recDocNo;
                bool isExist = lstDocNo.Where(a => a.Equals(docNo)).Any();
                while (isExist)
                {
                    count++;
                    documentNo = recDocNo + "-" + count;
                    if ((lstDocNo.Where(a => a.Equals(documentNo)).Any()) == false)
                        isExist = false;
                }
                newDocNo = documentNo ?? docNo;
            }
            else
                newDocNo = recDocNo;
            return newDocNo;
        }

        private void PostInvFromRecurring(InvoiceModel invoiceModel, Invoice invoice, List<TermsOfPayment> lstTOP, GSTSetting gstSetting, string connectionString)
        {
            Invoice _invoice = new Invoice();
            _invoice.Id = Guid.NewGuid();
            _invoice.IsRepeatingInvoice = false;
            InsertRecurringInvoice(invoiceModel, _invoice, invoice, invoiceModel.DocDate, lstTOP, false, gstSetting);
            // _invoice.InvoiceNumber = invoiceModel.IsDocNoEditable != null ? GenerateAutoNumberForType(invoiceModel.CompanyId, DocTypeConstants.Invoice, null) : GetPostedInvDocNo(invoiceModel.CompanyId, invoiceModel.DocNo + "-" + 1);
            _invoice.InvoiceNumber = GetPostedInvDocNo(invoiceModel.CompanyId, invoice.DocNo + "-" + 1);
            //_invoice.DocNo = GetPostedInvDocNo(invoiceModel.CompanyId, invoiceModel.DocNo + "-" + 1);
            _invoice.DocNo = _invoice.InvoiceNumber;
            _invoice.RecurInvId = invoice.Id;
            _invoice.Counter = 1;
            _invoice.ObjectState = ObjectState.Added;
            _invoiceEntityService.Insert(_invoice);
            if (invoiceModel.InvoiceDetails.Count > 0 || invoiceModel.InvoiceDetails != null)
            {
                FillInvoiceDetail(invoiceModel, _invoice);
            }
            //if (invoiceModel.InvoiceGSTDetails != null)
            //{
            //    _invoice.InvoiceGSTDetails = invoiceModel.InvoiceGSTDetails;
            //    foreach (InvoiceGSTDetail gstdetail in _invoice.InvoiceGSTDetails)
            //    {
            //        gstdetail.InvoiceId = _invoice.Id;
            //        gstdetail.ObjectState = ObjectState.Added;
            //    }
            //}
            try
            {
                unitOfWork.SaveChanges();
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_SuccessFully_Message);
                //JVModel jvm = new JVModel();
                //FillJournal(jvm, _invoice, true, DocTypeConstants.Invoice);
                //jvm.DocumentState = InvoiceStates.NotPaid;
                //SaveInvoice1(jvm);
                AppaWorld.Bean.Common.SavePosting(_invoice.CompanyId, _invoice.Id, _invoice.DocType, connectionString);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }
        public void FillInvoiceDetail(InvoiceModel TObject, Invoice _invoiceNew)
        {
            try
            {
                //List<InvoiceDetail> lstinvoicedetail = new List<InvoiceDetail>();
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Request_Message);
                if (TObject.InvoiceDetails != null)
                {
                    foreach (InvoiceDetail detail in TObject.InvoiceDetails)
                    {
                        if (detail.RecordStatus == "Added")
                        {
                            InvoiceDetail invoiceDetail = new InvoiceDetail();
                            invoiceDetail.Id = Guid.NewGuid();

                            FillInvDetail(_invoiceNew, detail, invoiceDetail);

                            //invoiceDetail.RecOrder = recorder + 1;
                            //recorder = detail.RecOrder;
                            //invoiceDetail.InvoiceId = _invoiceNew.Id;
                            ////invoiceDetail.TaxCodes = null;
                            ////detail.IsPLAccount = detail.ChartOfAccount.Category == "Income Statement" ? true : false;
                            ////detail.ChartOfAccount = null;
                            //invoiceDetail.TaxId = detail.TaxId;
                            //invoiceDetail.TaxRate = detail.TaxRate;
                            //invoiceDetail.DocTaxAmount = detail.DocTaxAmount;
                            invoiceDetail.RecOrder = detail.RecOrder;
                            invoiceDetail.ObjectState = ObjectState.Added;
                            _invoiceNew.InvoiceDetails.Add(invoiceDetail);
                        }
                        else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                        {
                            //InvoiceDetail invoiceDetail = null;
                            //if (TObject.IsWorkFlowInvoice == true)
                            //    invoiceDetail = TObject.InvoiceDetails.Where(a => a.InvoiceId == TObject.Id).FirstOrDefault();
                            //else
                            InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                            if (invoiceDetail != null)
                            {
                                FillInvDetail(_invoiceNew, detail, invoiceDetail);
                                //invoiceDetail.AccountName = detail.AccountName;
                                invoiceDetail.RecOrder = recorder + 1;
                                recorder = invoiceDetail.RecOrder;
                                invoiceDetail.ObjectState = ObjectState.Modified;
                                //lstinvoicedetail.Add(invoiceDetail);
                            }
                        }
                        else if (detail.RecordStatus == "Deleted")
                        {
                            InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                            if (invoiceDetail != null)
                            {
                                invoiceDetail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_SuccessFully_Message);
                    }
                }

                //_invoiceNew.InvoiceDetails = lstinvoicedetail;
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void FillInvDetail(Invoice _invoiceNew, InvoiceDetail detail, InvoiceDetail invoiceDetail)
        {
            invoiceDetail.InvoiceId = _invoiceNew.Id;
            invoiceDetail.ItemId = detail.ItemId;
            invoiceDetail.ItemCode = detail.ItemCode;
            invoiceDetail.ItemDescription = detail.ItemDescription;
            invoiceDetail.Qty = detail.Qty;
            invoiceDetail.Unit = detail.Unit;
            invoiceDetail.UnitPrice = detail.UnitPrice;
            invoiceDetail.DiscountType = detail.DiscountType;
            invoiceDetail.Discount = detail.Discount;
            invoiceDetail.COAId = detail.COAId;
            //invoiceDetail.AllowDisAllow = detail.AllowDisAllow;
            invoiceDetail.TaxId = detail.TaxId;
            //TaxCode taxcode = _taxCodeService.GetTaxCode(detail.TaxId.Value);
            invoiceDetail.TaxRate = detail.TaxRate;
            invoiceDetail.DocTaxAmount = detail.DocTaxAmount;
            //invoiceDetail.TaxCurrency = detail.TaxCurrency;
            invoiceDetail.DocAmount = detail.DocAmount;
            invoiceDetail.AmtCurrency = detail.AmtCurrency;
            invoiceDetail.DocTotalAmount = detail.DocTotalAmount;
            invoiceDetail.Remarks = detail.Remarks;
            invoiceDetail.TaxIdCode = detail.TaxIdCode;
            //invoiceDetail.BaseAmount = detail.BaseAmount;
            //invoiceDetail.BaseTaxAmount = detail.BaseTaxAmount;
            //invoiceDetail.BaseTotalAmount = detail.BaseTotalAmount;
            invoiceDetail.BaseAmount = _invoiceNew.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)_invoiceNew.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
            invoiceDetail.BaseTaxAmount = _invoiceNew.ExchangeRate != null ? invoiceDetail.DocTaxAmount != null ? Math.Round((decimal)invoiceDetail.DocTaxAmount * (decimal)_invoiceNew.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocTaxAmount : invoiceDetail.DocTaxAmount;
            invoiceDetail.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (invoiceDetail.BaseTaxAmount != null ? (decimal)invoiceDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
            //invoiceDetail.IsPLAccount = detail.IsPLAccount;
        }
        private void InsertRecurringInvoice(RecurringModel TObject, Invoice invoiceNew)
        {
            try
            {
                invoiceNew.CompanyId = TObject.CompanyId;
                invoiceNew.DocType = DocTypeConstants.Invoice;
                invoiceNew.DocSubType = "Invoice";
                invoiceNew.EntityType = "Customer";
                invoiceNew.DocDate = TObject.DocDate.Date;
                invoiceNew.DueDate = TObject.DueDate.Value.Date;
                invoiceNew.PONo = TObject.PONo;
                invoiceNew.EntityId = TObject.EntityId;
                invoiceNew.CreditTermsId = TObject.CreditTermsId;
                invoiceNew.Nature = TObject.Nature;
                invoiceNew.ServiceCompanyId = TObject.ServiceCompanyId;
                if (TObject.IsMultiCurrency == true)
                    invoiceNew.DocCurrency = TObject.DocCurrency;
                else
                    invoiceNew.DocCurrency = TObject.BaseCurrency;
                invoiceNew.IsMultiCurrency = TObject.IsMultiCurrency;
                invoiceNew.ExCurrency = TObject.BaseCurrency;
                invoiceNew.ExchangeRate = TObject.ExchangeRate;
                invoiceNew.ExDurationFrom = TObject.ExDurationFrom;
                invoiceNew.ExDurationTo = TObject.ExDurationTo;
                invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                invoiceNew.DocDescription = TObject.DocDescription;
                invoiceNew.IsGstSettings = TObject.IsGstSettings;
                invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                if (TObject.IsGstSettings)
                {
                    invoiceNew.GSTExchangeRate = TObject.GSTExchangeRate;
                    invoiceNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    invoiceNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }

                invoiceNew.IsRepeatingInvoice = TObject.IsRepeatingInvoice;
                if (TObject.IsRepeatingInvoice)
                {
                    invoiceNew.RepEveryPeriodNo = TObject.RepEveryPeriodNo;
                    invoiceNew.RepEveryPeriod = TObject.RepEveryPeriod;
                    if (TObject.EndDate == null)
                        invoiceNew.RepEndDate = null;
                    else
                        invoiceNew.RepEndDate = TObject.EndDate.Value.Date;
                    invoiceNew.ParentInvoiceID = TObject.ParentInvoiceID;
                }
                //else
                //{
                //    invoiceNew.RepEveryPeriodNo = null;
                //    invoiceNew.RepEveryPeriod = null;
                //    invoiceNew.RepEndDate = null;
                //    invoiceNew.ParentInvoiceID = null;
                //}

                invoiceNew.BalanceAmount = TObject.GrandTotal;
                invoiceNew.GrandTotal = TObject.GrandTotal;
                invoiceNew.GSTTotalAmount = TObject.GSTTotalAmount;

                invoiceNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;

                invoiceNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                invoiceNew.NoSupportingDocs = TObject.NoSupportingDocument;

                //invoiceNew.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, TObject.CompanyId);
                //if (invoiceNew.IsSegmentReporting)
                //{
                if (TObject.IsSegmentActive1 != null || TObject.IsSegmentActive1 == true)
                {
                    invoiceNew.SegmentMasterid1 = TObject.SegmentMasterid1;
                    invoiceNew.SegmentDetailid1 = TObject.SegmentDetailid1;
                    invoiceNew.SegmentCategory1 = TObject.SegmentCategory1;
                }
                if (TObject.IsSegmentActive2 != null || TObject.IsSegmentActive2 == true)
                {
                    invoiceNew.SegmentMasterid2 = TObject.SegmentMasterid2;
                    invoiceNew.SegmentDetailid2 = TObject.SegmentDetailid2;
                    invoiceNew.SegmentCategory2 = TObject.SegmentCategory2;
                }

                invoiceNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                invoiceNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                invoiceNew.Remarks = TObject.Remarks;
                invoiceNew.IsSegmentReporting = TObject.IsSegmentReporting;
                invoiceNew.Status = TObject.Status;
                invoiceNew.DocumentState = invoiceNew.BalanceAmount == 0 ? InvoiceState.FullyPaid : String.IsNullOrEmpty(TObject.DocumentState) ? InvoiceState.NotPaid : TObject.DocumentState;
                invoiceNew.CursorType = TObject.CursorType;
                invoiceNew.DocumentId = TObject.DocumentId;
                invoiceNew.InternalState = InvoiceState.Recurring;
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }


        public decimal? GetExRateInformation(string documentCurrency, string currency, DateTime Documentdate, long CompanyId)
        {
            decimal? exchangeRate = null;
            try
            {
                string date = Documentdate.ToString("yyyy-MM-dd");

                if (documentCurrency == currency)
                    exchangeRate = 1;
                else
                {
                    //new changes
                    CommonForex commonForex = _commonForexService.GetForexyByDateAndCurrency(CompanyId, currency, documentCurrency, Convert.ToDateTime(Documentdate));
                    if (commonForex != null)
                    {
                        exchangeRate = commonForex.FromForexRate;
                    }
                    else
                    {
                        var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + documentCurrency + "&symbols=" + currency;
                        AppsWorld.CommonModule.Models.CurrencyModel currencyRates = _download_serialized_json_data<AppsWorld.CommonModule.Models.CurrencyModel>(url);

                        exchangeRate = currencyRates.Rates.Where(c => c.Key == currency).Select(c => c.Value).FirstOrDefault();
                        FillCommonForexFrom(documentCurrency, Documentdate, exchangeRate, currency);

                    }
                }
                return exchangeRate ?? 1;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceConstants.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static T _download_serialized_json_data<T>(string url) where T : new()
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                try
                {
                    json_data = w.DownloadString(url);
                }
                catch (Exception) { }
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }

        private void FillCommonForexFrom(string DocumentCurrency, DateTime Documentdate, decimal? exchageRate, string BaseCurrency)
        {
            CommonForex commonForex = new CommonForex();
            commonForex.Id = Guid.NewGuid();
            commonForex.CompanyId = 0;
            commonForex.DateFrom = Convert.ToDateTime(Documentdate);
            commonForex.Dateto = commonForex.DateFrom;
            commonForex.FromForexRate = exchageRate;
            commonForex.ToForexRate = commonForex.FromForexRate;
            commonForex.FromCurrency = BaseCurrency;
            commonForex.ToCurrency = DocumentCurrency;
            commonForex.Status = RecordStatusEnum.Active;
            commonForex.Source = "Fixer";
            commonForex.UserCreated = "System";
            commonForex.CreatedDate = DateTime.UtcNow;
            commonForex.ObjectState = ObjectState.Added;
            _commonForexService.Insert(commonForex);

        }

        #endregion Recurring_Invoice

        #region GST_Regstration_De_Registration
        public bool? GetAllRecordBasedOnServiceCompanyId(long? serviceCompanyId, long? companyId)
        {
            TaxCode taxCode = _taxCodeService.Query(c => c.Code == "NA" && c.CompanyId == companyId).Select().FirstOrDefault();
            List<Invoice> lstAllInvoice = _invoiceEntityService.GetAllInvoiceByServiceCompany(serviceCompanyId.Value, companyId);
            List<AppsWorld.InvoiceModule.Entities.Journal> lstAllJournal = _journalService.GetAllJournalbyServiceCompanyId(serviceCompanyId.Value, companyId);
            List<InvoiceDetail> lstInvoiceDetail = lstAllInvoice.SelectMany(c => c.InvoiceDetails).Where(c => c.TaxId != taxCode.Id).ToList();
            List<JournalDetail> lstJournalDetail = lstAllJournal.SelectMany(c => c.JournalDetails).Where(c => c.TaxId != taxCode.Id).ToList();
            return lstInvoiceDetail.Count > 0 || lstJournalDetail.Count > 0 ? true : false;
        }
        #endregion GST_Regstration_De_Registration

        #region common_Call
        public DoctypeModel GetAllDocumentByInvoiceId(long companyId, Guid invoiceId, string docType, string connectionString, string username)
        {
            try
            {

                List<InvoiceDocumentDetailModel> invoiceDocumentDetails = new List<InvoiceDocumentDetailModel>();
                DoctypeModel doctypeModel = new DoctypeModel();
                List<long> lstCompanyIds = _companyService.GetAllSubCompaniesId(username, companyId);
                //For DoubtfulDebit
                var debtAllocationDetail = _doubtfulDebtallocationDetailService.GetDoubtfulDebtallocationdetailByDocumentId(invoiceId, docType);
                if (debtAllocationDetail.Any())
                {
                    Invoice debitProvision = new Invoice();
                    List<Guid> ddappDetailIds = debtAllocationDetail.Select(a => a.DoubtfulDebtAllocationId).ToList();
                    List<InvoiceDocumentDetailModel> debtDocumentDetails = new List<InvoiceDocumentDetailModel>();
                    //var debtAllocation = debtAllocationDetail.Select(a => a.DoubtfulDebtAllocation.Invoice);
                    var debtAllocation = _doubtfulDebtAllocationService.GetDDAllocationByCDetailid(ddappDetailIds);
                    if (debtAllocation.Any())
                        debitProvision = _invoiceEntityService.GetInvoiceByIdAndComapnyId(companyId, debtAllocation.Select(c => c.InvoiceId).FirstOrDefault());
                    // var invoice = debtAllocation.Any() ? debtAllocation.Select(a => a.Invoice) : null;

                    debtDocumentDetails = /*invoice != null ? invoice*/debtAllocationDetail.Select(a => new InvoiceDocumentDetailModel()
                    {
                        Id = debitProvision != null ? debitProvision.Id : (Guid?)null,
                        DocNo = debitProvision != null ? debtAllocation.Where(d => d.Id == a.DoubtfulDebtAllocationId).Select(d => d.DoubtfulDebtAllocationNumber).FirstOrDefault() : null,
                        //Amount = debtAllocation.Where(c => c.InvoiceId == a.Id).Select(x => x.DoubtfulDebtAllocationDetails.Select(p => p.AllocateAmount).FirstOrDefault()).FirstOrDefault(),
                        Amount = debtAllocation.Where(d => d.Id == a.DoubtfulDebtAllocationId).Select(d => d.Status).FirstOrDefault() == DoubtfulDebtAllocationStatus.Reset ? (decimal?)null : a.AllocateAmount,
                        DocType = DocTypeConstants.DoubtFulDebitNote,
                        //DocDate = /*debtAllocation.Where(d => d.Id == a.DoubtfulDebtAllocationId).Select(d => d.Status).FirstOrDefault() == DoubtfulDebtAllocationStatus.Reset ? null :*/ debitProvision != null ? debitProvision.DocDate : (DateTime?)null
                        DocDate = debtAllocation.Where(d => d.Id == a.DoubtfulDebtAllocationId).Select(d => d.DoubtfulDebtAllocationDate).FirstOrDefault(),
                        IsHyperLinkEnable = lstCompanyIds.Contains((Int64)(debitProvision.ServiceCompanyId))
                    }).ToList();

                    doctypeModel.DoubtfulDebitTotalAmount = debtDocumentDetails != null ? debtDocumentDetails.Sum(a => a.Amount) : null;
                    //doctypeModel.InvoiceDocumentDetails = debtDocumentDetails;
                    invoiceDocumentDetails.AddRange(debtDocumentDetails);
                }
                //for Receipt
                List<ReceiptDetail> lstReceiptDetail = _receiptDetailService.GetAllReceiptsByDocumentId(invoiceId);
                if (lstReceiptDetail.Any())
                {
                    List<InvoiceDocumentDetailModel> receiptDocumentDetails = new List<InvoiceDocumentDetailModel>();
                    var receipts = lstReceiptDetail.Select(a => a.Receipts);
                    receiptDocumentDetails = receipts.Select(a => new InvoiceDocumentDetailModel()
                    {
                        Id = a.Id,
                        DocNo = a.DocNo,
                        DocDate = a.DocDate,
                        DocType = DocTypeConstants.Receipt,
                        Amount = a.DocumentState != InvoiceState.Void ? lstReceiptDetail.Where(c => c.ReceiptId == a.Id).Select(x => x.ReceiptAmount).FirstOrDefault() : (decimal?)null,
                        IsHyperLinkEnable = lstCompanyIds.Contains(a.ServiceCompanyId)
                    }).ToList();
                    doctypeModel.ReceiptTotalAmount = receiptDocumentDetails.Sum(a => a.Amount);
                    invoiceDocumentDetails.AddRange(receiptDocumentDetails);
                }
                //for Credit Note
                List<CreditNoteApplicationDetail> lstCNADetail = _creditNoteApplicationDetailService.GetCNADetailByInvoiceId(invoiceId, docType);
                if (lstCNADetail.Any())
                {
                    Dictionary<Guid, long> lstOfCreditNote = new Dictionary<Guid, long>();
                    //Invoice creditNote = new Invoice();
                    List<InvoiceDocumentDetailModel> creditNotes = new List<InvoiceDocumentDetailModel>();
                    List<Guid> cnaDetailIds = lstCNADetail.Select(a => a.CreditNoteApplicationId).ToList();
                    var lstCreditNotes = _creditNoteApplicationService.GetListofCreditNoteById(cnaDetailIds);
                    if (lstCreditNotes.Any())
                    {
                        //creditNote = _invoiceEntityService.GetInvoiceByIdAndComapnyId(companyId, lstCreditNotes.Select(c => c.InvoiceId).FirstOrDefault());
                        lstOfCreditNote = _invoiceEntityService.GetListOsServiceEntityByInvoiceId(companyId, lstCreditNotes.Select(a => a.InvoiceId).Distinct().ToList(), DocTypeConstants.CreditNote);
                    }

                    if (lstCreditNotes.Any())
                    {
                        //var creditNote = lstCreditNotes.Any() ? lstCreditNotes.Select(a => a.Invoice) : null;
                        creditNotes =/* creditNote != null ? creditNote*/lstCNADetail.Select(a => new InvoiceDocumentDetailModel()
                        {
                            //Id = creditNote != null ? creditNote.Id : (Guid?)null,
                            Id = lstCreditNotes.Where(d => d.Id == a.CreditNoteApplicationId).Select(d => d.InvoiceId).FirstOrDefault(),
                            DocNo = lstCreditNotes.Where(d => d.Id == a.CreditNoteApplicationId).Select(d => d.CreditNoteApplicationNumber).FirstOrDefault(),
                            DocDate = lstCreditNotes.Where(d => d.Id == a.CreditNoteApplicationId).Select(d => d.CreditNoteApplicationDate).FirstOrDefault(),
                            DocType = DocTypeConstants.CreditNote,
                            //Amount = lstCreditNotes.Where(c => c.InvoiceId == a.Id).Select(x => x.CreditNoteApplicationDetails.Select(p => p.CreditAmount).FirstOrDefault()).FirstOrDefault(),
                            Amount = lstCreditNotes.Where(d => d.Id == a.CreditNoteApplicationId).Select(d => d.Status).FirstOrDefault() != CreditNoteApplicationStatus.Void ? a.CreditAmount : (decimal?)null,
                            IsHyperLinkEnable = lstCompanyIds.Contains((Int64)lstOfCreditNote.Where(d => d.Key == (lstCreditNotes.Where(b => b.Id == a.CreditNoteApplicationId).Select(c => c.InvoiceId).FirstOrDefault())).Select(x => x.Value).FirstOrDefault())
                        }).ToList(); //null;
                        doctypeModel.CreditNoteTotalAmount = creditNotes.Sum(a => a.Amount);
                    }

                    invoiceDocumentDetails.AddRange(creditNotes);
                }

                //for Payment and Payroll Payment
                List<PaymentDetailCompact> lstPaymentDetail = _receiptDetailService.GetById(invoiceId);
                if (lstPaymentDetail.Any())
                {
                    List<InvoiceDocumentDetailModel> paymentDocumentDetails = new List<InvoiceDocumentDetailModel>();
                    var payments = lstPaymentDetail.Select(a => a.Payment);
                    paymentDocumentDetails = payments.Select(a => new InvoiceDocumentDetailModel()
                    {
                        Id = a.Id,
                        DocNo = a.DocNo,
                        DocDate = a.DocDate,
                        DocType = DocTypeConstants.Payment,
                        Amount = a.DocumentState != InvoiceState.Void ? lstPaymentDetail.Where(c => c.PaymentId == a.Id).Select(x => x.PaymentAmount).FirstOrDefault() : (decimal?)null,
                        IsHyperLinkEnable = lstCompanyIds.Contains(a.ServiceCompanyId)
                    }).ToList();
                    doctypeModel.PaymentTotalAmount = paymentDocumentDetails.Sum(a => a.Amount);
                    invoiceDocumentDetails.AddRange(paymentDocumentDetails);
                }
                // For Bank Transfer
                if (docType == DocTypeConstants.Invoice)
                {
                    List<InvoiceDocumentDetailModel> transferDocumentDetails = new List<InvoiceDocumentDetailModel>();
                    string query = $"select BT.Id,BT.DocNo,BT.TransferDate,SD.SettledAmount,BT.DocType from Bean.Invoice inv join Bean.SettlementDetail SD on inv.Id = SD.DocumentId join Bean.BankTransfer BT on BT.Id = SD.BankTransferId and BT.CompanyId = {companyId} where inv.Id = '{invoiceId}' and inv.CompanyId = {companyId} and BT.DocumentState <> 'Void'";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            InvoiceDocumentDetailModel docModel = new InvoiceDocumentDetailModel();
                            docModel.Id = dr["Id"] != DBNull.Value ? Guid.Parse(dr["Id"].ToString()) : (Guid?)null;
                            docModel.DocNo = dr["DocNo"] != DBNull.Value ? Convert.ToString(dr["DocNo"]) : null;
                            docModel.DocDate = dr["TransferDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferDate"]) : (DateTime?)null;
                            docModel.DocType = dr["DocType"] != DBNull.Value ? Convert.ToString(dr["DocType"]) : null;
                            docModel.Amount = dr["SettledAmount"] != DBNull.Value ? Convert.ToDecimal(dr["SettledAmount"]) : (decimal?)null;
                            docModel.IsHyperLinkEnable = true;
                            transferDocumentDetails.Add(docModel);
                        }
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                    if (transferDocumentDetails.Any() && transferDocumentDetails.Count > 0)
                    {
                        doctypeModel.TransferTotalAmount = transferDocumentDetails.Sum(a => a.Amount);
                        invoiceDocumentDetails.AddRange(transferDocumentDetails);
                    }
                }
                if (docType == DocTypeConstants.DebitNote)
                {
                    List<InvoiceDocumentDetailModel> transferDocumentDetails = new List<InvoiceDocumentDetailModel>();
                    string query = $"select BT.Id,BT.DocNo,BT.TransferDate,SD.SettledAmount,BT.DocType from Bean.DebitNote inv join Bean.SettlementDetail SD on inv.Id = SD.DocumentId join Bean.BankTransfer BT on BT.Id = SD.BankTransferId and BT.CompanyId = {companyId} where inv.Id = '{invoiceId}' and inv.CompanyId = {companyId} and BT.DocumentState <> 'Void'";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            InvoiceDocumentDetailModel docModel = new InvoiceDocumentDetailModel();
                            docModel.Id = dr["Id"] != DBNull.Value ? Guid.Parse(dr["Id"].ToString()) : (Guid?)null;
                            docModel.DocNo = dr["DocNo"] != DBNull.Value ? Convert.ToString(dr["DocNo"]) : null;
                            docModel.DocDate = dr["TransferDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransferDate"]) : (DateTime?)null;
                            docModel.DocType = dr["DocType"] != DBNull.Value ? Convert.ToString(dr["DocType"]) : null;
                            docModel.Amount = dr["SettledAmount"] != DBNull.Value ? Convert.ToDecimal(dr["SettledAmount"]) : (decimal?)null;
                            docModel.IsHyperLinkEnable = true;
                            transferDocumentDetails.Add(docModel);
                        }
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                    if (transferDocumentDetails.Any() && transferDocumentDetails.Count > 0)
                    {
                        doctypeModel.TransferTotalAmount = transferDocumentDetails.Sum(a => a.Amount);
                        invoiceDocumentDetails.AddRange(transferDocumentDetails);
                    }
                }

                //doctypeModel.AmountDue = GrandTotal - (doctypeModel.CreditNoteTotalAmount + doctypeModel.ReceiptTotalAmount);
                doctypeModel.InvoiceDocumentDetailModels = invoiceDocumentDetails.OrderBy(c => c.DocNo).ThenBy(d => d.DocDate).ToList();

                if (doctypeModel.InvoiceDocumentDetailModels.Any())
                {
                    if (docType == DocTypeConstants.Invoice)
                        doctypeModel.AmountDue = _invoiceEntityService.GetBalanceAmount(companyId, invoiceId) ?? null;//for Balane Amount
                    else
                        doctypeModel.AmountDue = _debitNoteService.GetBalanceAmount(companyId, invoiceId) ?? null;//for Balane Amount
                }
                return doctypeModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        public List<InvoiceStateModel> GetDeletedAuditTrail(Guid invoiceId)
        {
            return _invoiceEntityService.GetDeletedAuditTrail(invoiceId).ToList();
        }






        public bool ChkTaxCodeForALEIsNull(List<ChkTaxCodeForALEIsNullVm> chkTaxCodeForALEIsNullVms)
        {
            List<long> lstCoaIds = _chartOfAccountService.GetAllCOAByIdsAndALE(chkTaxCodeForALEIsNullVms.Select(z => z.CoaId).ToList(), "Assets,Liabilities,Equity");
            if (chkTaxCodeForALEIsNullVms.Any(z => lstCoaIds.Contains(z.CoaId) && z.TaxId == null))
            {
                throw new Exception("Tax Code Should be NA for Classes Assets,Liabilities,Equity");
            }
            return true;
        }
        public class ChkTaxCodeForALEIsNullVm
        {
            public long CoaId { get; set; }
            public long? TaxId { get; set; }
        }

        #region WFInvoiceMongoFile
        public string SaveAttachments(WorkFlowMongoFile mongoFile)
        {
            string message = null;
            Invoice wfInvoice = _invoiceEntityService.GetInvoiceByCIdandId(mongoFile.InvoiceId, mongoFile.CompanyId);
            if (wfInvoice != null)
            {
                bool isSucessed = SaveInvoiceScreenFiles(mongoFile.MongoId, mongoFile.FileName, mongoFile.FileSize, wfInvoice.EntityId.ToString(), wfInvoice.Id.ToString(), wfInvoice.Id.ToString(), mongoFile.CompanyId, "Added", null, wfInvoice.UserCreated, null);
                if (isSucessed)
                    message = "File Added sucessfully";
                else
                    message = "Failed";
            }
            return message;
        }
        private bool SaveScreenFiles(string FileId, string fileName, string fileSize, string featureId, string recordId, string referenceId, long companyId)
        {
            List<DocTilesFilesVM> lstTilesVm = new List<DocTilesFilesVM>();
            DocTilesFilesVM tilesFileVM = new DocTilesFilesVM();
            tilesFileVM.FileId = FileId;
            tilesFileVM.Name = fileName;
            tilesFileVM.FileSize = fileSize;
            tilesFileVM.FeatureId = featureId;
            tilesFileVM.RecordId = recordId;
            tilesFileVM.ReferenceId = referenceId;
            tilesFileVM.CompanyId = companyId;
            tilesFileVM.IsSystem = true;
            tilesFileVM.ModuleName = "Bean Cursor";
            tilesFileVM.RecordStatus = "Added";
            lstTilesVm.Add(tilesFileVM);
            var json = RestSharpHelper.ConvertObjectToJason(lstTilesVm);
            try
            {
                object obj = lstTilesVm;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.Post(url, "api/document/savesscreenfiles", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<DocTilesFilesVM>(response.Content);
                    //lstTilesVm = data;
                }
                else
                {
                    throw new Exception(response.Content);
                }
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        private bool SaveInvoiceScreenFiles(string FileId, string fileName, string fileSize, string featureId, string recordId, string referenceId, long companyId, string recordStatus, string desc, string createdBy, string modifiedBy)
        {
            List<DocTilesFilesVM> lstTilesVm = new List<DocTilesFilesVM>();
            DocTilesFilesVM tilesFileVM = new DocTilesFilesVM();
            tilesFileVM.FileId = FileId;
            tilesFileVM.Name = fileName;
            tilesFileVM.FileSize = fileSize;
            tilesFileVM.FeatureId = featureId;
            tilesFileVM.RecordId = recordId;
            tilesFileVM.ReferenceId = referenceId;
            tilesFileVM.CompanyId = companyId;
            tilesFileVM.IsSystem = true;
            tilesFileVM.ModuleName = "Bean Cursor";
            tilesFileVM.TabName = "Invoices";
            tilesFileVM.Status = RecordStatusEnum.Active;
            tilesFileVM.RecordStatus = recordStatus;
            tilesFileVM.CreatedBy = createdBy;
            tilesFileVM.ModifiedBy = modifiedBy;
            if (recordStatus == "Added")
                tilesFileVM.CreatedDate = DateTime.UtcNow;
            else
                tilesFileVM.ModifiedDate = DateTime.UtcNow;
            lstTilesVm.Add(tilesFileVM);
            var json = RestSharpHelper.ConvertObjectToJason(lstTilesVm);
            try
            {
                object obj = lstTilesVm;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.Post(url, "api/document/savesscreenfiles", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<DocTilesFilesVM>(response.Content);
                    //lstTilesVm = data;
                }
                else
                {
                    throw new Exception(response.Content);
                }
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return false;
            }
        }
        public List<string> SaveListOfWFInvoice(long companyId)
        {
            try
            {
                List<Invoice> lstOfInvoice = _invoiceEntityService.GetListOfWFInvoice(companyId);
                List<string> lstInvoiceIds = new List<string>();
                if (lstOfInvoice.Any())
                {
                    foreach (Invoice invoice in lstOfInvoice)
                    {
                        bool isSuceeds = saveScreenRecords(invoice.Id.ToString(), invoice.EntityId.ToString(), invoice.EntityId.ToString(), invoice.DocNo, invoice.UserCreated, invoice.CreatedDate, true, invoice.CompanyId, "Invoices");
                        if (isSuceeds)
                        {
                            lstInvoiceIds.Add(invoice.Id.ToString());
                        }
                    }
                }
                return lstInvoiceIds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region GetAuto_number_call
        public string GetAutonumber(long companyId, string entityType, string connectionString, ref bool? isEdit)
        {
            string docNo = null;
            bool? isEditable;
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("Common_GenerateDocNo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@CursorName", "Bean Cursor");
                    cmd.Parameters.AddWithValue("@EntityType", entityType);
                    cmd.Parameters.AddWithValue("@IsAdd", false);
                    cmd.Parameters.Add("@DocNo", SqlDbType.NVarChar, 100);
                    cmd.Parameters["@DocNo"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@IsDocNoEditable", SqlDbType.Bit);
                    cmd.Parameters["@IsDocNoEditable"].Direction = ParameterDirection.Output;
                    cmd.ExecuteScalar();
                    docNo = cmd.Parameters["@DocNo"].Value != DBNull.Value ? Convert.ToString(cmd.Parameters["@DocNo"].Value) : null;

                    isEditable = cmd.Parameters["@IsDocNoEditable"].Value != DBNull.Value ? Convert.ToBoolean(cmd.Parameters["@IsDocNoEditable"].Value) : (bool?)null;
                    isEdit = isEditable;
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return docNo;
        }

        public void SaveAutoNumber(long companyId, string entityType, string docNo)
        {
            try
            {
                string oldDocNo = docNo;
                string oldNo = string.Empty;
                Entities.AutoNumber _auto = _autoNumberService.GetAutoNumber(companyId, entityType);
                if (_auto != null)
                {
                    if (_auto.IsEditable == true)
                    {
                        for (int i = oldDocNo.Length - 1; i >= 0; i--)
                        {
                            if (Char.IsDigit(oldDocNo[i]))
                                oldNo = oldDocNo[i] + oldNo;
                            else
                                break;
                        }
                        long subIndex = 0;
                        try
                        {
                            subIndex = long.Parse(oldNo);
                        }
                        catch { }
                        int index = oldDocNo.LastIndexOf(oldNo);
                        //_auto.EditableFormat = oldDocNo.Substring(0, index);
                        //_auto.EditableNumber = oldDocNo.Substring(index);
                        //_auto.GeneratedNumber = value;
                        _auto.IsDisable = true;
                        _auto.ObjectState = ObjectState.Modified;
                        //string s3 = s1 + s2;
                    }
                    else
                    {
                        long value = Convert.ToInt64(_auto.GeneratedNumber) + 1;
                        _auto.GeneratedNumber = Convert.ToString(value);
                        _auto.IsDisable = true;
                        _auto.ObjectState = ObjectState.Modified;
                        _autoNumberService.Update(_auto);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion



        #region InvoiceDocumentSave
        private void FillDocumentAndDetailType(InvoiceModel invoiceModel, string connectionString)
        {
            int count = 0;
            List<DocumentTypeModel> lstDocumentType = new List<DocumentTypeModel>();
            DocumentTypeModel documentType = new DocumentTypeModel();
            documentType.Id = invoiceModel.Id;
            documentType.CompanyId = invoiceModel.CompanyId;
            documentType.EntityId = invoiceModel.EntityId;
            documentType.ServiceCompanyId = invoiceModel.ServiceCompanyId;
            documentType.DocType = invoiceModel.DocType;
            documentType.DocSubType = invoiceModel.DocSubType;
            documentType.DocNo = invoiceModel.DocNo;
            documentType.DocumentState = invoiceModel.GrandTotal == 0 ? InvoiceState.FullyPaid : invoiceModel.DocumentState;
            documentType.DocCurrency = invoiceModel.DocCurrency;
            documentType.BaseCurrency = invoiceModel.BaseCurrency;
            documentType.GSTCurrency = invoiceModel.GSTExCurrency;
            documentType.PostingDate = invoiceModel.DocDate;
            documentType.DocDate = invoiceModel.DocDate;
            documentType.DueDate = invoiceModel.DueDate;
            documentType.BalanaceAmount = invoiceModel.GrandTotal == 0 ? 0 : invoiceModel.BalanceAmount;
            documentType.ExchangeRate = invoiceModel.ExchangeRate;
            documentType.GSTExchangeRate = invoiceModel.GSTExchangeRate;
            documentType.CreditTermsId = invoiceModel.CreditTermsId;
            documentType.DocDescription = invoiceModel.DocDescription;
            documentType.PONo = invoiceModel.PONo;
            documentType.NoSupportingDocs = invoiceModel.NoSupportingDocument;
            documentType.Nature = invoiceModel.Nature;
            documentType.GSTTotalAmount = invoiceModel.GSTTotalAmount;
            documentType.GrandTotal = invoiceModel.GrandTotal;
            documentType.IsGstSettings = invoiceModel.IsGstSettings;
            documentType.IsGSTApplied = invoiceModel.IsGSTApplied;
            documentType.IsMultiCurrency = invoiceModel.IsMultiCurrency;
            documentType.IsAllowableNonAllowable = invoiceModel.IsAllowableNonAllowable;
            documentType.IsNoSupportingDocument = invoiceModel.IsNoSupportingDocument;
            documentType.IsAllowableDisallowableActivated = invoiceModel.IsAllowableNonAllowable;
            documentType.Status = true;
            documentType.UserCreated = invoiceModel.UserCreated;
            documentType.CreatedDate = DateTime.UtcNow;
            documentType.ModifiedBy = invoiceModel.ModifiedBy;
            documentType.ModifiedDate = invoiceModel.ModifiedDate;
            documentType.IsBaseCurrencyRateChanged = invoiceModel.IsBaseCurrencyRateChanged;
            documentType.IsGSTCurrencyRateChanged = invoiceModel.IsGSTCurrencyRateChanged;
            lstDocumentType.Add(documentType);
            List<DocumentDetailTypeModel> lstDocumentDetailType = invoiceModel.InvoiceDetails.Select(a => new DocumentDetailTypeModel
            {
                Id = a.Id,
                ItemId = a.ItemId,
                ItemCode = a.ItemCode,
                ItemDescription = a.ItemDescription,
                Qty = a.Qty,
                Unit = a.Unit,
                UnitPrice = a.UnitPrice,
                DiscountType = a.DiscountType,
                Discount = a.Discount,
                COAId = a.COAId,
                TaxId = a.TaxId,
                TaxRate = a.TaxRate,
                DocTaxAmount = a.DocTaxAmount,
                DocAmount = a.DocAmount,
                DocTotalAmount = a.DocTotalAmount,
                BaseTaxAmount = a.BaseTaxAmount,
                BaseAmount = a.BaseAmount,
                BaseTotalAmount = a.BaseTotalAmount,
                AmtCurrency = a.AmtCurrency,
                RecOrder = (a.RecOrder == 0 || a.RecOrder == null) ? count++ : a.RecOrder,
                TaxIdCode = a.TaxIdCode,
                RecordStatus = a.RecordStatus
            }).ToList();
            SaveInterCoInvoice(lstDocumentType, lstDocumentDetailType, connectionString);
        }


        private void SaveInterCoInvoice(List<DocumentTypeModel> lstDocumentType, List<DocumentDetailTypeModel> lstDocumentTypeDetail, string connectionString)
        {
            con = null;
            dr = null;
            cmd = null;
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

        private void FillDocumentAndDetailTypeForCN(CreditNoteModel creditNoteModel, string connectionString)
        {
            int count = 0;
            List<DocumentTypeModel> lstDocumentType = new List<DocumentTypeModel>();
            DocumentTypeModel documentType = new DocumentTypeModel();
            documentType.Id = creditNoteModel.Id;
            documentType.CompanyId = creditNoteModel.CompanyId;
            documentType.EntityId = creditNoteModel.EntityId;
            documentType.ServiceCompanyId = creditNoteModel.ServiceCompanyId;
            documentType.DocType = DocTypeConstants.CreditNote;
            documentType.DocSubType = DocTypeConstants.Interco;
            documentType.DocNo = creditNoteModel.DocNo;
            documentType.DocumentState = creditNoteModel.GrandTotal == 0 ? CreditNoteState.FullyApplied : creditNoteModel.DocumentState;
            documentType.DocCurrency = creditNoteModel.DocCurrency;
            documentType.BaseCurrency = creditNoteModel.BaseCurrency;
            documentType.GSTCurrency = creditNoteModel.GSTExCurrency;
            documentType.PostingDate = creditNoteModel.DocDate;
            documentType.DocDate = creditNoteModel.DocDate;
            documentType.DueDate = creditNoteModel.DueDate;
            documentType.BalanaceAmount = creditNoteModel.GrandTotal == 0 ? 0 : creditNoteModel.BalanceAmount;
            documentType.ExchangeRate = creditNoteModel.ExchangeRate;
            documentType.GSTExchangeRate = creditNoteModel.GSTExchangeRate;
            documentType.CreditTermsId = creditNoteModel.CreditTermsId;
            documentType.DocDescription = creditNoteModel.Remarks;
            documentType.PONo = null;
            documentType.NoSupportingDocs = creditNoteModel.NoSupportingDocument;
            documentType.Nature = creditNoteModel.Nature;
            documentType.GSTTotalAmount = creditNoteModel.GSTTotalAmount;
            documentType.GrandTotal = creditNoteModel.GrandTotal;
            documentType.IsGstSettings = creditNoteModel.IsGstSettings;
            documentType.IsGSTApplied = creditNoteModel.IsGSTApplied;
            documentType.IsMultiCurrency = creditNoteModel.IsMultiCurrency;
            documentType.IsAllowableNonAllowable = creditNoteModel.IsAllowableNonAllowable;
            documentType.IsNoSupportingDocument = creditNoteModel.IsNoSupportingDocument;
            documentType.IsAllowableDisallowableActivated = creditNoteModel.IsAllowableNonAllowable;
            documentType.Status = true;
            documentType.UserCreated = creditNoteModel.UserCreated;
            documentType.CreatedDate = DateTime.UtcNow;
            documentType.ModifiedBy = creditNoteModel.ModifiedBy;
            documentType.ModifiedDate = creditNoteModel.ModifiedDate;
            documentType.IsBaseCurrencyRateChanged = creditNoteModel.IsBaseCurrencyRateChanged;
            documentType.IsGSTCurrencyRateChanged = creditNoteModel.IsGSTCurrencyRateChanged;
            lstDocumentType.Add(documentType);
            List<DocumentDetailTypeModel> lstDocumentDetailType = creditNoteModel.InvoiceDetails.Select(a => new DocumentDetailTypeModel
            {
                Id = a.Id,
                ItemId = a.ItemId,
                ItemCode = a.ItemCode,
                ItemDescription = a.ItemDescription,
                Qty = a.Qty,
                Unit = a.Unit,
                UnitPrice = a.UnitPrice,
                DiscountType = a.DiscountType,
                Discount = a.Discount,
                COAId = a.COAId,
                TaxId = a.TaxId,
                TaxRate = a.TaxRate,
                DocTaxAmount = a.DocTaxAmount,
                DocAmount = a.DocAmount,
                DocTotalAmount = a.DocTotalAmount,
                BaseTaxAmount = a.BaseTaxAmount,
                BaseAmount = a.BaseAmount,
                BaseTotalAmount = a.BaseTotalAmount,
                AmtCurrency = a.AmtCurrency,
                RecOrder = (a.RecOrder == 0 || a.RecOrder == null) ? count++ : a.RecOrder,
                TaxIdCode = a.TaxIdCode,
                RecordStatus = a.RecordStatus
            }).ToList();
            SaveInterCoInvoice(lstDocumentType, lstDocumentDetailType, connectionString);
        }


        private void FillDocumentAndDetailTypeForCNIndirect(CreditNoteModel creditNoteModel, CreditNoteApplicationModel CNAppModel, string connectionString)
        {
            int count = 0;
            List<DocumentTypeModel> lstDocumentType = new List<DocumentTypeModel>();
            List<CreditNoteAppTypeModel> lstCNAppTypeModel = new List<CreditNoteAppTypeModel>();
            List<CreditNoteAppDetailTypeModel> lstCreditNoteAppTypeDetail = null;

            DocumentTypeModel documentType = new DocumentTypeModel();
            documentType.Id = creditNoteModel.Id;
            documentType.CompanyId = creditNoteModel.CompanyId;
            documentType.EntityId = creditNoteModel.EntityId;
            documentType.ServiceCompanyId = creditNoteModel.ServiceCompanyId;
            documentType.DocType = DocTypeConstants.CreditNote;
            documentType.DocSubType = DocTypeConstants.Interco;
            documentType.DocNo = creditNoteModel.DocNo;
            documentType.DocumentState = creditNoteModel.GrandTotal == 0 ? CreditNoteState.FullyApplied : creditNoteModel.DocumentState;
            documentType.DocCurrency = creditNoteModel.DocCurrency;
            documentType.BaseCurrency = creditNoteModel.BaseCurrency;
            documentType.GSTCurrency = creditNoteModel.GSTExCurrency;
            documentType.PostingDate = creditNoteModel.DocDate;
            documentType.DocDate = creditNoteModel.DocDate;
            documentType.DueDate = creditNoteModel.DueDate;
            documentType.BalanaceAmount = creditNoteModel.GrandTotal == 0 ? 0 : creditNoteModel.BalanceAmount;
            documentType.ExchangeRate = creditNoteModel.ExchangeRate;
            documentType.GSTExchangeRate = creditNoteModel.GSTExchangeRate;
            documentType.CreditTermsId = creditNoteModel.CreditTermsId;
            documentType.DocDescription = creditNoteModel.Remarks;
            documentType.PONo = null;
            documentType.NoSupportingDocs = creditNoteModel.NoSupportingDocument;
            documentType.Nature = creditNoteModel.Nature;
            documentType.GSTTotalAmount = creditNoteModel.GSTTotalAmount;
            documentType.GrandTotal = creditNoteModel.GrandTotal;
            documentType.IsGstSettings = creditNoteModel.IsGstSettings;
            documentType.IsGSTApplied = creditNoteModel.IsGSTApplied;
            documentType.IsMultiCurrency = creditNoteModel.IsMultiCurrency;
            documentType.IsAllowableNonAllowable = creditNoteModel.IsAllowableNonAllowable;
            documentType.IsNoSupportingDocument = creditNoteModel.IsNoSupportingDocument;
            documentType.IsAllowableDisallowableActivated = creditNoteModel.IsAllowableNonAllowable;
            documentType.Status = true;
            documentType.UserCreated = creditNoteModel.UserCreated;
            documentType.CreatedDate = DateTime.UtcNow;
            documentType.ModifiedBy = creditNoteModel.ModifiedBy;
            documentType.ModifiedDate = creditNoteModel.ModifiedDate;
            documentType.IsBaseCurrencyRateChanged = creditNoteModel.IsBaseCurrencyRateChanged;
            documentType.IsGSTCurrencyRateChanged = creditNoteModel.IsGSTCurrencyRateChanged;
            lstDocumentType.Add(documentType);
            List<DocumentDetailTypeModel> lstDocumentDetailType = creditNoteModel.InvoiceDetails.Select(a => new DocumentDetailTypeModel
            {
                Id = a.Id,
                ItemId = a.ItemId,
                ItemCode = a.ItemCode,
                ItemDescription = a.ItemDescription,
                Qty = a.Qty,
                Unit = a.Unit,
                UnitPrice = a.UnitPrice,
                DiscountType = a.DiscountType,
                Discount = a.Discount,
                COAId = a.COAId,
                TaxId = a.TaxId,
                TaxRate = a.TaxRate,
                DocTaxAmount = a.DocTaxAmount,
                DocAmount = a.DocAmount,
                DocTotalAmount = a.DocTotalAmount,
                BaseTaxAmount = a.BaseTaxAmount,
                BaseAmount = a.BaseAmount,
                BaseTotalAmount = a.BaseTotalAmount,
                AmtCurrency = a.AmtCurrency,
                RecOrder = (a.RecOrder == 0 || a.RecOrder == null) ? count++ : a.RecOrder,
                TaxIdCode = a.TaxIdCode,
                RecordStatus = a.RecordStatus
            }).ToList();

            if (creditNoteModel.CreditNoteApplicationModel != null)
            {
                CreditNoteAppTypeModel CNAppTypeModel = new CreditNoteAppTypeModel();
                CNAppTypeModel.Id = CNAppModel.Id;
                CNAppTypeModel.CompanyId = creditNoteModel.CompanyId;
                CNAppTypeModel.InvoiceId = CNAppModel.InvoiceId;
                CNAppTypeModel.IsRevExcess = CNAppModel.IsRevExcess;
                CNAppTypeModel.UserCreated = CNAppModel.UserCreated;
                CNAppTypeModel.CreatedDate = CNAppModel.CreatedDate;
                CNAppTypeModel.ModifiedBy = CNAppModel.ModifiedBy;
                CNAppTypeModel.ModifiedDate = CNAppModel.ModifiedDate;
                CNAppTypeModel.Remarks = CNAppModel.Remarks;
                CNAppTypeModel.Status = 1;
                CNAppTypeModel.ExchangeRate = creditNoteModel.ExchangeRate;
                CNAppTypeModel.CreditNoteApplicationDate = CNAppModel.CreditNoteApplicationDate;
                CNAppTypeModel.CreditNoteApplicationResetDate = CNAppModel.CreditNoteApplicationResetDate;
                CNAppTypeModel.CreditNoteApplicationNumber = CNAppModel.CreditNoteApplicationNumber;
                CNAppTypeModel.CreditAmount = CNAppModel.CreditAmount;
                lstCNAppTypeModel.Add(CNAppTypeModel);
                lstCreditNoteAppTypeDetail = CNAppModel.CreditNoteApplicationDetailModels.Select(a => new CreditNoteAppDetailTypeModel
                {
                    Id = a.Id,
                    COAId = a.COAId,
                    TaxId = null,
                    TaxRate = null,
                    TaxAmount = null,
                    TotalAmount = a.CreditAmount,
                    DocumentId = a.DocumentId,
                    DocumentType = a.DocType,
                    DocNo = a.DocNo,
                    DocDescription = null,
                    CreditAmount = a.CreditAmount,
                    BaseCurrencyExchangeRate = Convert.ToDecimal(a.BaseCurrencyExchangeRate),
                    ServiceEntityId = a.ServiceEntityId,
                    CreditNoteApplicationId = a.CreditNoteApplicationId,
                    DocCurrency = a.DocCurrency,
                    RecOrder = 1,
                    TaxIdCode = null,

                }).ToList();

            }
            SaveInterCoCreditNote(lstDocumentType, lstDocumentDetailType, lstCNAppTypeModel, lstCreditNoteAppTypeDetail, connectionString);
        }


        private static void SaveInterCoCreditNote(List<DocumentTypeModel> lstDocumentType, List<DocumentDetailTypeModel> lstDocumentTypeDetail, List<CreditNoteAppTypeModel> lstCNAppType, List<CreditNoteAppDetailTypeModel> lstCNAppDetailType, string connectionString)
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

                    SqlParameter CNAppmaster = new SqlParameter();
                    CNAppmaster.ParameterName = "@CreditNoteApp";
                    CNAppmaster.TypeName = "dbo.CreditNoteApplication";
                    CNAppmaster.Value = ToDataTable(lstCNAppType);
                    cmd.Parameters.Add(CNAppmaster);

                    SqlParameter CNAppdetail = new SqlParameter();
                    CNAppdetail.ParameterName = "@CreditNoteAppDetial";
                    CNAppdetail.TypeName = "dbo.CreditNoteApplicationDetail";
                    CNAppdetail.Value = ToDataTable(lstCNAppDetailType);
                    cmd.Parameters.Add(CNAppdetail);

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


        #endregion


        #region CommonTaxCodeLu
        private List<TaxCodeLookUp<string>> GetTaxcodesLu(DateTime date, long companyId, string connectionString, string docSubType)
        {
            try
            {
                query = string.Empty;
                List<TaxCodeLookUp<string>> lstTaxCodeLookUp = new List<TaxCodeLookUp<string>>();
                if (docSubType == "Posted")
                    query = $"SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,IsApplicable as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxCode=Tax.Code Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyId} Right Join Bean.TaxCode Tax on Tax.Code=TaxMapDetail.CustTaxCode where Tax.CompanyId={companyId}  and Tax.Status < 2 order by tax.Code";
                else
                    query = $"SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,IsApplicable as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxCode=Tax.Code Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyId} Right Join Bean.TaxCode Tax on Tax.Code=TaxMapDetail.CustTaxCode where Tax.CompanyId={companyId}  and Tax.Status<2 and EffectiveFrom<='{date.ToString("yyyy-MM-dd")}' and (EffectiveTo>='{date.ToString("yyyy-MM-dd")}' OR EffectiveTo is null) order by tax.Code";

                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TaxCodeLookUp<string> taxCodeLookUp = new TaxCodeLookUp<string>();
                        taxCodeLookUp.Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0;
                        taxCodeLookUp.Code = dr["TXCODE"].ToString();
                        taxCodeLookUp.Name = dr["NAME"].ToString();
                        taxCodeLookUp.TaxRate = dr["TAXRATE"] != DBNull.Value ? Convert.ToDouble(dr["TAXRATE"]) : (double?)null;
                        taxCodeLookUp.IsTaxAmountEditable = !(taxCodeLookUp.TaxRate == 0 || taxCodeLookUp.TaxRate == null);
                        taxCodeLookUp.TaxType = dr["TAXTYPE"].ToString();
                        taxCodeLookUp.Status = (RecordStatusEnum)dr["STATUS"];
                        taxCodeLookUp.TaxIdCode = taxCodeLookUp.Code != "NA" ? taxCodeLookUp.Code + "-" + taxCodeLookUp.TaxRate + (taxCodeLookUp.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")" */: taxCodeLookUp.Code;
                        taxCodeLookUp.IsInterCoBillingTaxCode = taxCodeLookUp.Code == "NA" ? true : dr["IsInterCo"] != DBNull.Value ? Convert.ToBoolean(dr["IsInterCo"]) : (bool?)null;
                        taxCodeLookUp.IsApplicable = dr["IsGstActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstActive"]) : (bool?)null;
                        lstTaxCodeLookUp.Add(taxCodeLookUp);
                    }
                    if (con.State != ConnectionState.Closed)
                        con.Close();
                }
                return lstTaxCodeLookUp;
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Save Note
        public List<InvoiceNote> SaveInvoiceNotes(List<InvoiceNoteModel> TObject, string ConnectionString)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Request_Message);
                InvoiceNote invoiceNote = new InvoiceNote();
                List<InvoiceNote> invoiceNotes = new List<InvoiceNote>();
                Invoice invoice = _invoiceEntityService.GetinvoiceById(TObject.Select(x => x.InvoiceId).FirstOrDefault());
                if (invoice != null)
                {
                    invoiceNotes = _invoiceNoteService.GetInvoiceByid(invoice.Id).ToList();
                    if (TObject != null)
                    {
                        foreach (InvoiceNoteModel note in TObject)
                        {
                            if (invoiceNotes.Any())
                            {
                                if (note.RecordStatus == "Added")
                                {
                                    InsertNote(invoiceNote, note);
                                    invoiceNotes.Add(invoiceNote);
                                    _invoiceNoteService.Insert(invoiceNote);
                                }
                                else if (note.RecordStatus != "Added" && note.RecordStatus != "Deleted" && note.RecordStatus != null)
                                {
                                    InvoiceNote invoiceNoteForUpdate = invoiceNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                                    if (invoiceNoteForUpdate != null)
                                    {
                                        invoiceNoteForUpdate.InvoiceId = note.InvoiceId;
                                        invoiceNoteForUpdate.ExpectedPaymentDate = note.ExpectedPaymentDate;
                                        invoiceNoteForUpdate.Notes = note.Notes;
                                        invoiceNoteForUpdate.ModifiedDate = DateTime.UtcNow;
                                        invoiceNoteForUpdate.ModifiedBy = note.ModifiedBy;
                                        invoiceNoteForUpdate.ObjectState = ObjectState.Modified;
                                    }
                                }
                                else if (note.RecordStatus == "Deleted")
                                {
                                    InvoiceNote invoiceNoteForDelete = invoiceNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                                    if (invoiceNote != null)
                                    {
                                        invoiceNoteForDelete.ObjectState = ObjectState.Deleted;
                                    }
                                }
                            }
                            else
                            {
                                if (note.RecordStatus == "Added")
                                {
                                    InsertNote(invoiceNote, note);
                                    _invoiceNoteService.Insert(invoiceNote);
                                }
                            }
                        }

                        var journal = _journalService.GetJournals(invoice.Id, invoice.CompanyId);
                        if (invoice.DocSubType != "Opening Bal")
                        {
                            if (journal.IsAddNote == null || journal.IsAddNote != true)
                            {
                                journal.IsAddNote = true;
                                _journalService.Update(journal);
                            }
                        }
                        unitOfWork.SaveChanges();
                        #region Update_Note_In_Journal

                        invoiceNotes = _invoiceNoteService.GetInvoiceByid(invoice.Id).ToList();
                        if (!invoiceNotes.Any())
                            if (journal.IsAddNote == true)
                            {
                                //journal.IsAddNote = false;
                                //_journalService.Update(journal);
                                using (con = new SqlConnection(ConnectionString))
                                {
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    //query = $"Select IsAddNote from Bean.Journal where DocumentId='{invoice.Id}' and CompanyId={invoice.CompanyId}";
                                    //cmd = new SqlCommand(query, con);
                                    //dr = cmd.ExecuteReader();
                                    //while (dr.Read())
                                    //{
                                    //    if (Convert.ToBoolean(dr["IsAddNote"]))
                                    //    {
                                    query = $"Update Bean.Journal Set IsAddNote=0 where DocumentId='{invoice.Id}' and CompanyId={invoice.CompanyId}";
                                    //dr.Close();
                                    cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    //    }
                                    //}
                                    //if (con.State == ConnectionState.Open)
                                    con.Close();
                                }
                            }

                        #endregion Update_Note_In_Journal
                    }
                }
                else
                {
                    throw new Exception("Invalid invoiceId");
                }
                return invoiceNotes;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void InsertNote(InvoiceNote invoiceNote, InvoiceNoteModel note)
        {
            invoiceNote.CreatedDate = DateTime.UtcNow;
            invoiceNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
            invoiceNote.Id = /*Guid.NewGuid()*/note.Id;
            invoiceNote.InvoiceId = note.InvoiceId;
            invoiceNote.Notes = note.Notes;
            invoiceNote.UserCreated = note.UserCreated;
            invoiceNote.ObjectState = ObjectState.Added;
        }
        #endregion

















        #region Peppol

        public void PeppolInvoiceModelBinding(InvoiceModel TObject, Invoice _invoice, BeanEntity entity, Company serviceCompany)
        {

            var bankdetails = _invoiceEntityService.GetBankDetailsByCompanyId(serviceCompany.Id);
            PeppolInvModel peppolInvModel = new PeppolInvModel();

            var month = DateTime.UtcNow.Month;
            var day = DateTime.UtcNow.Day;
            var hours = DateTime.UtcNow.Hour;
            var mintues = DateTime.UtcNow.Minute;
            var sec = DateTime.UtcNow.Second;
            var creationDateAndTime = DateTime.UtcNow.Year + "-" + (month > 0 && month < 10 ? "0" + month.ToString() : month.ToString()) + "-" + (day > 0 && day < 10 ? "0" + day.ToString() : day.ToString()) + "T" + (hours > 0 && hours < 10 ? "0" + hours.ToString() : hours.ToString()) +
            ":" + (mintues > 0 && mintues < 10 ? "0" + mintues.ToString() : mintues.ToString()) + ":" + (sec > 0 && sec < 10 ? "0" + sec.ToString() : sec.ToString()) + "+08:00";

            var docDate = _invoice.DocDate;
            var invDocDate = docDate.Year + "-" + (docDate.Month > 0 && docDate.Month < 10 ? "0" + docDate.Month.ToString() : docDate.Month.ToString()) + "-" + (docDate.Day > 0 && docDate.Day < 10 ? "0" + docDate.Day.ToString() : docDate.Day.ToString());
            var dueDate = _invoice.DueDate != null ? _invoice.DueDate.Value : DateTime.UtcNow;
            var invDueDate = dueDate.Year + "-" + (dueDate.Month > 0 && dueDate.Month < 10 ? "0" + dueDate.Month.ToString() : dueDate.Month.ToString()) + "-" + (dueDate.Day > 0 && dueDate.Day < 10 ? "0" + dueDate.Day.ToString() : dueDate.Day.ToString());

            _invoice.DocDescription = !string.IsNullOrEmpty(_invoice.DocDescription) ? _invoice.DocDescription : "Generated from Peppol (" + serviceCompany.ParticipantPeppolId + ")";

            peppolInvModel.IssueDate = invDocDate;
            peppolInvModel.DueDate = invDueDate;
            peppolInvModel.InvoiceTypeCode = "380";
            peppolInvModel.Note = _invoice.DocDescription;
            peppolInvModel.DocumentCurrencyCode = _invoice.DocCurrency;
            // peppolInvModel.AccountingCost=_invoice.DocA
            peppolInvModel.BuyerReference = _invoice.InvoiceNumber;
            peppolInvModel.InvStartDate = invDocDate;
            peppolInvModel.InvEndDate = invDocDate;
            peppolInvModel.BuyerReference = _invoice.InvoiceNumber;
            peppolInvModel.OrderReferenceId = _invoice.InvoiceNumber;
            peppolInvModel.OrderRefSalesOrderId = _invoice.InvoiceNumber;
            peppolInvModel.BillingRefId = _invoice.InvoiceNumber;
            peppolInvModel.BillingRefIssueDate = invDocDate;
            peppolInvModel.AccSupplierEndpointID = serviceCompany.RegistrationNo;
            peppolInvModel.AccSupplierPartyIdentifiID = serviceCompany.RegistrationNo;
            peppolInvModel.AccSupplierPartyIdentiPartyName = serviceCompany.Name;
            //Address
            // peppolInvModel.AccSupplierStreetName= serviceCompany.M
            if (_invoice.ExCurrency == "SGD")
                peppolInvModel.AccSupplierIdentificationCode = "SG";
            else
                peppolInvModel.AccSupplierIdentificationCode = _invoice.ExCurrency;

            peppolInvModel.AccSupPartyTaxSchemeCompanyId = serviceCompany.RegistrationNo;
            peppolInvModel.AccSupPartyTaxSchemeID = "GST";
            peppolInvModel.TaxSchemeID = "GST";
            peppolInvModel.AccSupLegalEntityRegName = serviceCompany.Name;

            //Customer
            peppolInvModel.AccCustEndpointID = entity.GSTRegNo;
            peppolInvModel.AccCustPartyIdentifiID = entity.GSTRegNo;
            peppolInvModel.AccCustPartyIdentiPartyName = entity.Name;
            //Address
            peppolInvModel.AccCustIdentificationCode = peppolInvModel.AccSupplierIdentificationCode;
            peppolInvModel.AccCustLegalEntityRegName = entity.Name;
            //Contact

            peppolInvModel.PaymentMeansCode = "30";//Bank Transfer
            peppolInvModel.PayeeFinancialAccountID = bankdetails.AccountNumber;
            peppolInvModel.PayeeFinancialAccountName = bankdetails.AccountName;
            peppolInvModel.FinancialInstituBranchId = bankdetails.BranchCode;

            //TaxAmount
            int j = 1;
            decimal sumOfExtAmount = 0;
            decimal sumOfAllowAmount = 0;
            decimal sumOfChargeAmt = 0;
            List<PeppolInvLineItemModel> lstInvLineItems = new List<PeppolInvLineItemModel>();
            foreach (var invDetails in _invoice.InvoiceDetails)
            {
                PeppolInvLineItemModel invLineItemModel = new PeppolInvLineItemModel();
                invLineItemModel.ID = j++;
                invLineItemModel.Note = _invoice.DocDescription;
                invLineItemModel.InvoicedQuantity = invDetails.Qty;
                invLineItemModel.BaseQuantity = 1;
                invLineItemModel.PriceAmount = invDetails.UnitPrice != null ? invDetails.UnitPrice.Value : invDetails.DocAmount;

                if (invDetails.Discount != null && invDetails.Discount > 0)
                {
                    invLineItemModel.AllowanceChargeReason = "Discount";
                    invLineItemModel.AllowanceChargeReasonCode = "100";
                    invLineItemModel.BaseAmount = invDetails.UnitPrice != null ? invDetails.UnitPrice.Value * Convert.ToDecimal(invLineItemModel.InvoicedQuantity) : invDetails.DocAmount;

                    if (invDetails.DiscountType == "%")
                    {
                        invLineItemModel.MultiplierFactorNumeric = invDetails.Discount;
                        invLineItemModel.AllowanceAmount = invLineItemModel.BaseAmount *
                           (Convert.ToDecimal(invLineItemModel.MultiplierFactorNumeric / 100));
                    }

                    else //  DiscountType =="$"
                    {
                        if (invDetails.DiscountType == "$")
                        {
                            var disCountPer = (invDetails.Discount.Value / Convert.ToDouble(invLineItemModel.BaseAmount)) * 100;
                            invLineItemModel.MultiplierFactorNumeric = disCountPer;
                            invLineItemModel.AllowanceAmount = invLineItemModel.BaseAmount *
                           (Convert.ToDecimal(invLineItemModel.MultiplierFactorNumeric / 100));
                        }
                    }
                    invLineItemModel.ChargeAmount = 0;
                    var lineExtAmount = (invLineItemModel.PriceAmount * Convert.ToDecimal(invLineItemModel.InvoicedQuantity)) + Convert.ToDecimal(invLineItemModel.ChargeAmount) - invLineItemModel.AllowanceAmount;
                    invLineItemModel.LineExtensionAmount = lineExtAmount;
                    sumOfAllowAmount = sumOfAllowAmount + invLineItemModel.AllowanceAmount;
                    sumOfChargeAmt = sumOfChargeAmt + invLineItemModel.ChargeAmount;


                }
                else
                {
                    var lineExtAmount = (invLineItemModel.PriceAmount / Convert.ToDecimal(invLineItemModel.BaseQuantity)) * Convert.ToDecimal(invLineItemModel.InvoicedQuantity);
                    invLineItemModel.LineExtensionAmount = lineExtAmount;
                }

                sumOfExtAmount = sumOfExtAmount + Convert.ToDecimal(invLineItemModel.LineExtensionAmount);
                invLineItemModel.ItemName = _invoice.InvoiceNumber;
                if (_invoice.ExCurrency == "SGD")
                    invLineItemModel.IdentificationCode = "SG";
                else
                    invLineItemModel.IdentificationCode = _invoice.ExCurrency;

                if (serviceCompany != null && serviceCompany.IsGstSetting == true)
                {
                    var taxcode = _taxCodeService.GetTaxCodesById(invDetails.TaxId.Value).FirstOrDefault();
                    if (taxcode != null)
                    {
                        invLineItemModel.ClassifiedTaxCategID = taxcode.Code;
                        var taxRate = taxcode.TaxRate != null ? Convert.ToInt32(taxcode.TaxRate) : 0;
                        invLineItemModel.ClassifiedTaxCategPercent = taxRate;
                        invLineItemModel.singleTaxAmount = (invLineItemModel.LineExtensionAmount *
                            invLineItemModel.ClassifiedTaxCategPercent) / 100;
                    }
                }
                else
                {
                    invLineItemModel.ClassifiedTaxCategID = "NG";
                    invLineItemModel.ClassifiedTaxCategPercent = 0;
                    invLineItemModel.singleTaxAmount = invLineItemModel.LineExtensionAmount *
                            (invLineItemModel.ClassifiedTaxCategPercent / 100);
                }
                invLineItemModel.TaxSchemeID = "GST";
                invLineItemModel.SellersItemIdentifiID = invDetails.ItemDescription;

                lstInvLineItems.Add(invLineItemModel);
            }
            peppolInvModel.InvLineItems = lstInvLineItems;
            peppolInvModel.LegalLineExtensionAmount = sumOfExtAmount;
            sumOfAllowAmount = 0; //need to Conformation
            sumOfChargeAmt = 0;
            peppolInvModel.LegalAllowanceTotalAmount = sumOfAllowAmount.ToString();
            peppolInvModel.LegalChargeTotalAmount = sumOfChargeAmt.ToString();
            var excluseAmount = sumOfExtAmount - sumOfAllowAmount + sumOfChargeAmt;
            peppolInvModel.LegalTaxExclusiveAmount = excluseAmount;
            //Tax
            peppolInvModel.TaxableAmount = sumOfExtAmount + sumOfChargeAmt - sumOfAllowAmount;
            var difftaxIds = _invoice.InvoiceDetails.GroupBy(x => x.TaxId).Where(a => a.Count() > 1).Select(y => y.Key).ToList();
            if (difftaxIds.Count() > 0)
            {
                peppolInvModel.TaxCategoryID = lstInvLineItems.Select(x => x.ClassifiedTaxCategID).FirstOrDefault();
                peppolInvModel.TaxCategoryPercent = lstInvLineItems.Select(x => x.ClassifiedTaxCategPercent).FirstOrDefault();
                peppolInvModel.TaxAmount = lstInvLineItems.Sum(x => x.singleTaxAmount);
            }
            else
            {
                peppolInvModel.TaxCategoryID = lstInvLineItems.Select(x => x.ClassifiedTaxCategID).FirstOrDefault();
                peppolInvModel.TaxCategoryPercent = lstInvLineItems.Select(x => x.ClassifiedTaxCategPercent).FirstOrDefault();
                peppolInvModel.TaxAmount = (peppolInvModel.TaxableAmount * peppolInvModel.TaxCategoryPercent) / 100;
            }
            peppolInvModel.LegalTaxInclusiveAmount = peppolInvModel.LegalTaxExclusiveAmount + peppolInvModel.TaxAmount;
            peppolInvModel.LegalPayableAmount = peppolInvModel.LegalTaxInclusiveAmount;



            var lstAddress = _invoiceEntityService.GetAddress(_invoice.EntityId);
            if (lstAddress.Count > 0)
            {
                foreach (var address in lstAddress)
                {
                    if (address.AddressBook != null && address.AddSectionType == "Mailing Address")
                    {
                        peppolInvModel.CustMaillingUnit = address.AddressBook.UnitNo;
                        peppolInvModel.CustMaillingBlock = address.AddressBook.BlockHouseNo;
                        peppolInvModel.CustMaillingBuilding = address.AddressBook.BuildingEstate;
                        peppolInvModel.CustMaillingStreet = address.AddressBook.Street;
                        peppolInvModel.CustMaillingCity = address.AddressBook.City;
                        peppolInvModel.CustMaillingState = address.AddressBook.State;
                        peppolInvModel.CustMaillingCountry = address.AddressBook.Country;
                        peppolInvModel.CustMaillingPostalCode = address.AddressBook.PostalCode;
                        //peppolInvModel.CustMaillingAddLine1 = address.AddressBook.Street;
                        //peppolInvModel.CustMaillingAddLine2 = address.AddressBook.UnitNo;
                        //peppolInvModel.CustIsMaillingLocalAddress = address.AddressBook.IsLocal == true ? true : false;
                        //peppolInvModel.CustIsMaillingForeignAddress = address.AddressBook.IsLocal == false ? true : false;
                    }
                    if (address.AddressBook != null && address.AddSectionType == "Registered Address")
                    {
                        peppolInvModel.CustRegisteredUnit = address.AddressBook.UnitNo;
                        peppolInvModel.CustRegisteredBlock = address.AddressBook.BlockHouseNo;
                        peppolInvModel.CustRegisteredBuilding = address.AddressBook.BuildingEstate;
                        peppolInvModel.CustRegisteredStreet = address.AddressBook.Street;
                        peppolInvModel.CustRegisteredCity = address.AddressBook.City;
                        peppolInvModel.CustRegisteredState = address.AddressBook.State;
                        peppolInvModel.CustRegisteredCountry = address.AddressBook.Country;
                        peppolInvModel.CustRegisteredPostalCode = address.AddressBook.PostalCode;
                    }
                }
            }

            string firstName = null;
            string ContactCom = null;
            string ConDetailCom = null;

            string str = "Select c.FirstName,c.Communication as ConCommuni,cd.Communication as ConDetaCom from Common.Contact as c join Common.ContactDetails as cd on c.Id = cd.ContactId where cd.EntityId= '" + _invoice.EntityId + "'";
            int? resultsetCount = query.Split(';').Count();
            SqlConnection con = new SqlConnection(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection);
            con.Open();
            SqlCommand cmd = new SqlCommand(str, con);
            SqlDataReader dr = cmd.ExecuteReader();
            for (int i = 1; i <= resultsetCount; i++)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        firstName = dr["FirstName"].ToString();
                        ContactCom = dr["ConCommuni"].ToString();
                        ConDetailCom = dr["ConDetaCom"].ToString();
                    }
                }
                dr.NextResult();
            }
            con.Close();

            peppolInvModel.AccCustContactName = firstName;
            if (!string.IsNullOrEmpty(ContactCom))
            {
                var lstclientcommu = JsonConvert.DeserializeObject<List<CommunicationBindingModel>>(ContactCom);
                if (lstclientcommu.Count > 0)
                {
                    if (lstclientcommu.Exists(x => x.Key == "Email"))
                    {
                        peppolInvModel.AccCustContactEmail = lstclientcommu.Where(x => x.Key == "Email").Select(c => c.Value).FirstOrDefault();
                    }
                    if (lstclientcommu.Any(x => x.Key == "Phone"))
                    {
                        peppolInvModel.AccCustContactTelephone = lstclientcommu.Where(x => x.Key == "Phone").Select(c => c.Value).FirstOrDefault();
                    }
                    if (lstclientcommu.Any(x => x.Key == "Mobile"))
                    {
                        peppolInvModel.AccCustContactTelephone = lstclientcommu.Where(x => x.Key == "Mobile").Select(c => c.Value).FirstOrDefault();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(ConDetailCom))
                {
                    var lstclientcommu = JsonConvert.DeserializeObject<List<CommunicationBindingModel>>(ConDetailCom);
                    if (lstclientcommu.Count > 0)
                    {
                        if (lstclientcommu.Any(x => x.Key == "Email"))
                        {
                            peppolInvModel.AccCustContactEmail = lstclientcommu.Where(x => x.Key == "Email").Select(c => c.Value).FirstOrDefault();
                        }
                        if (lstclientcommu.Any(x => x.Key == "Phone"))
                        {
                            peppolInvModel.AccCustContactTelephone = lstclientcommu.Where(x => x.Key == "Phone").Select(c => c.Value).FirstOrDefault();
                        }
                        if (lstclientcommu.Any(x => x.Key == "Mobile"))
                        {
                            peppolInvModel.AccCustContactTelephone = lstclientcommu.Where(x => x.Key == "Mobile").Select(c => c.Value).FirstOrDefault();
                        }
                    }
                }
            }

            string fileName = NameReplaceMethod(_invoice.InvoiceNumber);
            fileName = _invoice.CompanyId + "_" + fileName;
            // string tempPath = ConfigurationManager.AppSettings["TempPath"];
            string filePath = @ConfigurationManager.AppSettings["TempPath"] + fileName + ".xml";
            using (FileStream fs = File.Create(filePath))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                fs.Write(info, 0, info.Length);
            }


            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Start Namespaces");

            XNamespace spc = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace ccts = "urn:un:unece:uncefact:documentation:2";
            XNamespace ext = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
            XNamespace qdt = "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2";
            XNamespace udt = "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2";
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace xs = "http://www.w3.org/2001/XMLSchema";
            XNamespace ns = "http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader";


            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Start Peppol invoice Xml file generation");

            XDocument doc = new XDocument(
                  new XDeclaration("1.0", "UTF-8", null),
                     new XElement(ns + "StandardBusinessDocument",
                        new XAttribute(XNamespace.Xmlns + "xs", xs.NamespaceName),
                        new XAttribute("xmlns", ns.NamespaceName),
                     new XElement("StandardBusinessDocumentHeader",
                      new XElement("HeaderVersion", "1.0"),
                       new XElement("Sender",
                       new XElement("Identifier",
                       new XAttribute("Authority", "iso6523-actorid-upis"), "0195:" + serviceCompany.ParticipantPeppolId)),
                        new XElement("Receiver",
                       new XElement("Identifier",
                       new XAttribute("Authority", "iso6523-actorid-upis"), "0195:" + entity.PeppolDocumentId)),


                         new XElement("DocumentIdentification",
                         new XElement("Standard", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2"),
                         new XElement("TypeVersion", "2.1"),
                         new XElement("InstanceIdentifier", "b0f952ba-df85-450a-b543-8fddc362c277"),
                         new XElement("Type", "Invoice"),
                         new XElement("CreationDateAndTime", creationDateAndTime)),

                          new XElement("BusinessScope",
                          new XElement("Scope",
                          new XElement("Type", "DOCUMENTID"),
                          new XElement("InstanceIdentifier", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2::Invoice##urn:cen.eu:en16931:2017#conformant#urn:fdc:peppol.eu:2017:poacc:billing:international:sg:3.0::2.1"),
                          new XElement("Identifier", "busdox-docid-qns")),

                          new XElement("Scope",
                          new XElement("Type", "PROCESSID"),
                          new XElement("InstanceIdentifier", "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0"),
                          new XElement("Identifier", "cenbii-procid-ubl")))),

                   new XElement(spc + "Invoice",
                   new XAttribute("xmlns", spc.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "cac", cac.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "cbc", cbc.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "ccts", ccts.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "ext", ext.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "qdt", qdt.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "udt", udt.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "xsd", xsd.NamespaceName),
                   new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),

                    new XElement(cbc + "UBLVersionID", "2.1"),
                    new XElement(cbc + "CustomizationID", "urn:cen.eu:en16931:2017#conformant#urn:fdc:peppol.eu:2017:poacc:billing:international:sg:3.0"),
                    new XElement(cbc + "ProfileID", "urn:fdc:peppol.eu:2017:poacc:billing:01:1.0"),
                     new XElement(cbc + "ID", _invoice.InvoiceNumber),

                     new XElement(cbc + "IssueDate", peppolInvModel.IssueDate),
                     new XElement(cbc + "DueDate", peppolInvModel.DueDate),
                     new XElement(cbc + "InvoiceTypeCode", peppolInvModel.InvoiceTypeCode),
                     new XElement(cbc + "Note", peppolInvModel.Note),
                     new XElement(cbc + "DocumentCurrencyCode", peppolInvModel.DocumentCurrencyCode),
                     // new XElement(cbc + "AccountingCost", peppolInvModel.AccountingCost),
                     new XElement(cbc + "BuyerReference", peppolInvModel.BuyerReference),

                      new XElement(cac + "InvoicePeriod",
                       new XElement(cbc + "StartDate", peppolInvModel.IssueDate),
                        new XElement(cbc + "EndDate", peppolInvModel.IssueDate)),

                      new XElement(cac + "OrderReference",
                       new XElement(cbc + "ID", peppolInvModel.OrderReferenceId),
                        new XElement(cbc + "SalesOrderID", peppolInvModel.OrderRefSalesOrderId)),

                      new XElement(cac + "BillingReference",
                       new XElement(cac + "InvoiceDocumentReference",
                        new XElement(cbc + "ID", peppolInvModel.BillingRefId),
                         new XElement(cbc + "IssueDate", peppolInvModel.BillingRefIssueDate))),


                    new XElement(cac + "AccountingSupplierParty",
                    new XElement(cac + "Party",
                    new XElement(cbc + "EndpointID",
                        new XAttribute("schemeID", "0195"), "SGUEN" + peppolInvModel.AccSupplierEndpointID),
                    new XElement(cac + "PartyIdentification",
                    new XElement(cbc + "ID",
                       new XAttribute("schemeID", "0195"), "SGUEN" + peppolInvModel.AccSupplierPartyIdentifiID)),
                    new XElement(cac + "PartyName",
                    new XElement(cbc + "Name", peppolInvModel.AccSupplierPartyIdentiPartyName)),

                     new XElement(cac + "PostalAddress",
                     new XElement(cbc + "StreetName", peppolInvModel.SupMaillingStreet != null && peppolInvModel.SupMaillingStreet != "" ? peppolInvModel.SupMaillingStreet : peppolInvModel.SupRegisteredStreet != null && peppolInvModel.SupRegisteredStreet != "" ? peppolInvModel.SupRegisteredStreet : "NA"),
                     new XElement(cbc + "CityName", peppolInvModel.SupMaillingCity != null && peppolInvModel.SupMaillingCity != "" ? peppolInvModel.SupMaillingCity : peppolInvModel.SupRegisteredCity != null && peppolInvModel.SupRegisteredCity != "" ? peppolInvModel.SupRegisteredCity : "NA"),
                     new XElement(cbc + "PostalZone", peppolInvModel.SupMaillingPostalCode != null && peppolInvModel.SupMaillingPostalCode != "" ? peppolInvModel.SupMaillingPostalCode : peppolInvModel.SupRegisteredPostalCode != null && peppolInvModel.SupRegisteredPostalCode != "" ? peppolInvModel.SupRegisteredPostalCode : "NA"),
                     new XElement(cbc + "CountrySubentity", peppolInvModel.SupMaillingCountry != null && peppolInvModel.SupMaillingCountry != "" ? peppolInvModel.SupMaillingCountry : peppolInvModel.SupRegisteredCountry != null && peppolInvModel.SupRegisteredCountry != "" ? peppolInvModel.SupRegisteredCountry : "NA"),

                     new XElement(cac + "Country",
                     new XElement(cbc + "IdentificationCode", peppolInvModel.AccSupplierIdentificationCode))),

                 new XElement(cac + "PartyTaxScheme",
                 new XElement(cbc + "CompanyID", peppolInvModel.AccSupPartyTaxSchemeCompanyId),//Gstno "M2-1234567-K"
                 new XElement(cac + "TaxScheme",
                 new XElement(cbc + "ID", peppolInvModel.AccSupPartyTaxSchemeID))),

                 new XElement(cac + "PartyLegalEntity",
                 new XElement(cbc + "RegistrationName", peppolInvModel.AccSupLegalEntityRegName))//seller name
                 )),


                 //AccountingCustomerParty  
                 new XElement(cac + "AccountingCustomerParty",
                    new XElement(cac + "Party",
                    new XElement(cbc + "EndpointID",
                        new XAttribute("schemeID", "0195"), "SGUEN" + peppolInvModel.AccCustEndpointID),
                    new XElement(cac + "PartyIdentification",
                    new XElement(cbc + "ID",
                       new XAttribute("schemeID", "0195"), "SGUEN" + peppolInvModel.AccCustPartyIdentifiID)),
                    new XElement(cac + "PartyName",
                    new XElement(cbc + "Name", peppolInvModel.AccCustPartyIdentiPartyName)),

                     new XElement(cac + "PostalAddress",
                     new XElement(cbc + "StreetName", peppolInvModel.CustRegisteredStreet != null && peppolInvModel.CustRegisteredStreet != "" ? peppolInvModel.CustRegisteredStreet : peppolInvModel.CustMaillingStreet != null && peppolInvModel.CustMaillingStreet != "" ? peppolInvModel.CustMaillingStreet : "NA"),
                     new XElement(cbc + "CityName", peppolInvModel.CustRegisteredCity != null && peppolInvModel.CustRegisteredCity != "" ? peppolInvModel.CustRegisteredCity : peppolInvModel.CustMaillingCity != null && peppolInvModel.CustMaillingCity != "" ? peppolInvModel.CustMaillingCity : "NA"),
                     new XElement(cbc + "PostalZone", peppolInvModel.CustRegisteredPostalCode != null && peppolInvModel.CustRegisteredPostalCode != "" ? peppolInvModel.CustRegisteredPostalCode : peppolInvModel.CustMaillingPostalCode != null && peppolInvModel.CustMaillingPostalCode != "" ? peppolInvModel.CustMaillingPostalCode : "NA"),
                     new XElement(cbc + "CountrySubentity", peppolInvModel.CustRegisteredCountry != null && peppolInvModel.CustRegisteredCountry != "" ? peppolInvModel.CustRegisteredCountry : peppolInvModel.CustMaillingCountry != null && peppolInvModel.CustMaillingCountry != "" ? peppolInvModel.CustMaillingCountry : "NA"),

                     new XElement(cac + "Country",
                     new XElement(cbc + "IdentificationCode", peppolInvModel.AccCustIdentificationCode))),



                 new XElement(cac + "PartyLegalEntity",
                 new XElement(cbc + "RegistrationName", peppolInvModel.AccCustLegalEntityRegName)),

                 new XElement(cac + "Contact",
                 new XElement(cbc + "Name", !string.IsNullOrEmpty(peppolInvModel.AccCustContactName) ? peppolInvModel.AccCustContactName : "NA"),
                 new XElement(cbc + "Telephone", !string.IsNullOrEmpty(peppolInvModel.AccCustContactTelephone) ? peppolInvModel.AccCustContactTelephone : "NA"),
                 new XElement(cbc + "ElectronicMail", !string.IsNullOrEmpty(peppolInvModel.AccCustContactEmail) ? peppolInvModel.AccCustContactEmail : "NA"))
                 )),

                   new XElement(cac + "PaymentMeans",
                     new XElement(cbc + "PaymentMeansCode",
                     new XAttribute("name", "Bank Transfer"), peppolInvModel.PaymentMeansCode),
                     new XElement(cbc + "PaymentID", Guid.NewGuid()),
                   new XElement(cac + "PayeeFinancialAccount",
                     new XElement(cbc + "ID", peppolInvModel.PayeeFinancialAccountID),
                     new XElement(cbc + "Name", peppolInvModel.PayeeFinancialAccountName),
                   new XElement(cac + "FinancialInstitutionBranch",
                     new XElement(cbc + "ID", peppolInvModel.FinancialInstituBranchId)))),

                 new XElement(cac + "TaxTotal",
                 new XElement(cbc + "TaxAmount",
                     new XAttribute("currencyID", "SGD"), peppolInvModel.TaxAmount),
                 new XElement(cac + "TaxSubtotal",
                 new XElement(cbc + "TaxableAmount",
                      new XAttribute("currencyID", "SGD"), peppolInvModel.TaxableAmount),
                  new XElement(cbc + "TaxAmount",
                      new XAttribute("currencyID", "SGD"), peppolInvModel.TaxAmount),
                     new XElement(cac + "TaxCategory",
                     new XElement(cbc + "ID", peppolInvModel.TaxCategoryID),
                     new XElement(cbc + "Percent", peppolInvModel.TaxCategoryPercent),
                     new XElement(cac + "TaxScheme",
                     new XElement(cbc + "ID", peppolInvModel.TaxSchemeID)))
                     )),


                  new XElement(cac + "LegalMonetaryTotal",
                 new XElement(cbc + "LineExtensionAmount",
                 new XAttribute("currencyID", "SGD"), peppolInvModel.LegalLineExtensionAmount),
                 new XElement(cbc + "TaxExclusiveAmount",
                 new XAttribute("currencyID", "SGD"), peppolInvModel.LegalTaxExclusiveAmount),
                 new XElement(cbc + "TaxInclusiveAmount",
                 new XAttribute("currencyID", "SGD"), peppolInvModel.LegalTaxInclusiveAmount),
                 new XElement(cbc + "AllowanceTotalAmount",
                 new XAttribute("currencyID", "SGD"), peppolInvModel.LegalAllowanceTotalAmount),
                 new XElement(cbc + "ChargeTotalAmount",
                 new XAttribute("currencyID", "SGD"), peppolInvModel.LegalChargeTotalAmount),
                 new XElement(cbc + "PrepaidAmount",
                 new XAttribute("currencyID", "SGD"), "0.00"),
                 new XElement(cbc + "PayableRoundingAmount",
                 new XAttribute("currencyID", "SGD"), "0.00"),
                 new XElement(cbc + "PayableAmount",
                 new XAttribute("currencyID", "SGD"), peppolInvModel.LegalPayableAmount)),

                  incidentalsLine(peppolInvModel)

                 )));

            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file inside spaces removing.");
            if (serviceCompany.IsGstSetting == true)
            {
                foreach (var node in doc.Root.Descendants())
                {
                    if (node.Name.NamespaceName == "")
                    {
                        node.Attributes("xmlns").Remove();
                        node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                    }
                }
            }
            else
            {
                foreach (var node in doc.Root.Descendants())
                {
                    if (node.Name.NamespaceName == "")
                    {
                        node.Attributes("xmlns").Remove();
                        node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                    }
                    if (node.Name.LocalName == "PartyLegalEntity" && (node.PreviousNode.Parent.Parent.Name.LocalName == "AccountingSupplierParty"))
                    {
                        node.PreviousNode.Remove();
                    }
                }
            }

            if (_invoice.InvoiceDetails.Any(x => x.Discount == 0 || x.Discount == null))
            {
                foreach (var node in doc.Root.Descendants())
                {
                    if (node.Name.LocalName == "LineExtensionAmount" && node.Parent.Name.LocalName == "InvoiceLine")
                    {
                        node.NextNode.Remove();
                    }
                }
            }

            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file inside spaces removed Successful.");

            doc.Save(filePath);

            XmlDocument newDoc = new XmlDocument();
            if (!string.IsNullOrEmpty(filePath))
                newDoc.Load(filePath);
            string jsonText = JsonConvert.SerializeXmlNode(newDoc);
            _invoice.XMLFileData = jsonText;

            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file generated successful.");

            using (var httpClient = new HttpClient())
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file upload restsharp method started.");
                string apiKey = ConfigurationManager.AppSettings["PeppolApiKey"].ToString();
                string peppolURL = ConfigurationManager.AppSettings["PeppolURL"].ToString();

                // string peppolURL = "https://api.ap-connect.dev.einvoice.sg/";
                //var client = new RestClient("https://api.ap-connect.dev.einvoice.sg/v1/invoice/outbound/upload");

                var client = new RestClient(peppolURL + "v1/invoice/outbound/upload");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "text/xml");
                request.AddHeader("api_key", apiKey);
                // byte[] bytes = File.ReadAllBytes(@"C:/temp/SampleWithSBDHFile1.xml"); // File path
                byte[] bytes = File.ReadAllBytes(filePath);
                request.AddParameter("text/xml", bytes, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file upload restsharp method executed.");

                PeppolResponseModel respModel = null;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    respModel = JsonConvert.DeserializeObject<PeppolResponseModel>(response.Content);
                    if (string.IsNullOrEmpty(respModel.message))
                        respModel.message = "NULL";

                    query = "Update Bean.Invoice set PeppolDocumentId='" + respModel.docId + "',PeppolStatus='" + respModel.status + "',PeppolRemarks='" + respModel.message + "',XMLFileData='" + jsonText + "',PeppolStatusCode='" + response.StatusCode + "' where Id='" + _invoice.Id + "'";
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file uploaded  successful.and status code is ok.");
                }
                else
                {
                    respModel = JsonConvert.DeserializeObject<PeppolResponseModel>(response.Content);
                    query = "Update Bean.Invoice set PeppolDocumentId='" + respModel.docId + "',PeppolStatus='" + respModel.status + "',PeppolRemarks='" + respModel.message + "',XMLFileData='" + jsonText + "',PeppolStatusCode='" + response.StatusCode + "' where Id='" + _invoice.Id + "'";

                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Staus" + respModel.status + respModel.message);
                }

                using (SqlConnection conn = new SqlConnection(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd1 = new SqlCommand(query, conn);
                    cmd1.ExecuteNonQuery();
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "Peppol invoice Xml file generated successful.and saved in invoice table");
            }

        }

        public List<XElement> incidentalsLine(PeppolInvModel peppolInvoiceModel)
        {
            List<XElement> lstele = new List<XElement>();
            XNamespace spc = "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2";
            XNamespace cac = "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2";
            XNamespace cbc = "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2";
            XNamespace ccts = "urn:un:unece:uncefact:documentation:2";
            XNamespace ext = "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2";
            XNamespace qdt = "urn:oasis:names:specification:ubl:schema:xsd:QualifiedDatatypes-2";
            XNamespace udt = "urn:un:unece:uncefact:data:specification:UnqualifiedDataTypesSchemaModule:2";
            XNamespace xsd = "http://www.w3.org/2001/XMLSchema";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

            foreach (var invLineItem in peppolInvoiceModel.InvLineItems)
            {
                var data = new XElement(cac + "InvoiceLine",
                new XElement(cbc + "ID", invLineItem.ID),
                // new XElement(cbc + "Note", invoice.Remarks),
                new XElement(cbc + "InvoicedQuantity",
                new XAttribute("unitCode", "H87"), invLineItem.InvoicedQuantity),
                new XElement(cbc + "LineExtensionAmount",
                new XAttribute("currencyID", "SGD"), invLineItem.LineExtensionAmount),


               new XElement(cac + "AllowanceCharge",
               new XElement(cbc + "ChargeIndicator", false),
               new XElement(cbc + "AllowanceChargeReasonCode", invLineItem.AllowanceChargeReasonCode),
               new XElement(cbc + "AllowanceChargeReason", invLineItem.AllowanceChargeReason),
               new XElement(cbc + "MultiplierFactorNumeric", invLineItem.MultiplierFactorNumeric),
               new XElement(cbc + "Amount",
               new XAttribute("currencyID", "SGD"), invLineItem.AllowanceAmount),
               new XElement(cbc + "BaseAmount",
               new XAttribute("currencyID", "SGD"), invLineItem.BaseAmount)),

               new XElement(cac + "Item",
               new XElement(cbc + "Name", invLineItem.ItemName),
               new XElement(cac + "SellersItemIdentification",
               new XElement(cbc + "ID", invLineItem.SellersItemIdentifiID)),
               //new XElement(cac + "StandardItemIdentification",
               //new XElement(cbc + "ID", new XAttribute("schemeID", "0160"), 1234567890121)),//need to chnge
               new XElement(cac + "OriginCountry",
               new XElement(cbc + "IdentificationCode", invLineItem.IdentificationCode)),
               new XElement(cac + "CommodityClassification",
               new XElement(cbc + "ItemClassificationCode",
               new XAttribute("listID", "MP"), 43211503)),
               new XElement(cac + "ClassifiedTaxCategory",
               new XElement(cbc + "ID", invLineItem.ClassifiedTaxCategID),
               new XElement(cbc + "Percent", invLineItem.ClassifiedTaxCategPercent),
               new XElement(cac + "TaxScheme",
               new XElement(cbc + "ID", invLineItem.TaxSchemeID)))
               ),

               new XElement(cac + "Price",
               new XElement(cbc + "PriceAmount", new XAttribute("currencyID", "SGD"), invLineItem.PriceAmount),
               new XElement(cbc + "BaseQuantity",/* new XAttribute("unitCode", "H87"),*/ invLineItem.BaseQuantity)));
                lstele.Add(data);
            }
            return lstele;
        }

        public string NameReplaceMethod(string name)
        {
            name = name.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
            .Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
            Regex re = new Regex("[.]*(?=[.]$)");
            name = re.Replace(name, "");
            name = name.EndsWith(".") ? name.Remove(name.Length - 1) : name;
            name = name.Trim();
            return name;
        }


        #endregion

    }
}
