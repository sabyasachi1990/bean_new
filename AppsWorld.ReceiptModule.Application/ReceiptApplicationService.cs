using System.Collections.Generic;
using AppsWorld.ReceiptModule.RepositoryPattern;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Service;
using AppsWorld.ReceiptModule.Models;
using System;
using System.Linq;
using AppsWorld.Framework;
using Repository.Pattern.Infrastructure;
using System.Data.Entity.Validation;
using AppsWorld.ReceiptModule.Infra;
using System.Configuration;
using Logger;
using Serilog;
using AppsWorld.CommonModule.Infra;
using AppsWorld.ReceiptModule.Infra.Resources;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using AppaWorld.Bean;
using Ziraff.FrameWork.Logging;
using System.Net;

namespace AppsWorld.ReceiptModule.Application
{
    public class ReceiptApplicationService
    {
        private readonly IReceiptService _receiptService;
        private readonly ICompanyService _companyService;
        private readonly ICurrencyService _currencyService;
        private readonly ITermsOfPaymentService _termsOfPaymentService;
        private readonly AppsWorld.CommonModule.Service.ITaxCodeService _taxCodeService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IControlCodeCategoryService _controlCodeCategoryService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly IGSTSettingService _gstSettingService;
        private readonly IControlCodeService _controlCodeService;
        private readonly IReceiptBalancingItemService _receiptBalancingItemService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly ICompanySettingService _companySettingService;
        private readonly IMultiCurrencySettingService _multiCurrencySettingService;
        private readonly AppsWorld.CommonModule.Service.IAccountTypeService _accountTypeService;
        private readonly IReceiptDetailService _receiptDetailService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly IInvoiceService _invoiceService;
        private readonly IDebitNoteService _debitNoteService;
        private readonly IJournalService _journalService;
        private readonly IJournalDetailService _journalDetailService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        string doc = "";
        IReceiptModuleUnitOfWorkAsync _unitOfWorkAsync;
        SqlCommand cmd = null;
        SqlConnection con = null;
        SqlDataReader dr = null;
        string query = string.Empty;


        #region Constuctor
        public ReceiptApplicationService(IReceiptService receiptService, ICompanyService companyService,
            ICurrencyService currencyService, ITermsOfPaymentService termsOfPaymentService, AppsWorld.CommonModule.Service.ITaxCodeService taxCodeService,
            IChartOfAccountService chartOfAccountService, IControlCodeCategoryService controlCodeCategoryService,
            IBeanEntityService beanEntityService, IGSTSettingService gstSettingService, IControlCodeService controlCodeService,
            IAutoNumberService autoNumberService, IAutoNumberCompanyService autoNumberCompanyService,
            IReceiptBalancingItemService receiptBalancingItemService, IFinancialSettingService financialSettingService,
            ICompanySettingService companySettingService, IMultiCurrencySettingService multiCurrencySettingService,
             AppsWorld.CommonModule.Service.IAccountTypeService accountTypeService, IReceiptDetailService receiptDetailService, IInvoiceService invoiceService, IDebitNoteService debitNoteService,
            IReceiptModuleUnitOfWorkAsync unitOfWorkAsync, IJournalService journalService, IJournalDetailService journalDetailService, AppsWorld.CommonModule.Service.IAutoNumberService autoService)
        {
            this._receiptService = receiptService;
            this._companyService = companyService;
            this._currencyService = currencyService;
            this._termsOfPaymentService = termsOfPaymentService;
            this._taxCodeService = taxCodeService;
            this._chartOfAccountService = chartOfAccountService;
            this._controlCodeCategoryService = controlCodeCategoryService;
            this._gstSettingService = gstSettingService;
            this._controlCodeService = controlCodeService;
            this._receiptBalancingItemService = receiptBalancingItemService;
            this._financialSettingService = financialSettingService;
            this._companySettingService = companySettingService;
            this._multiCurrencySettingService = multiCurrencySettingService;
            this._beanEntityService = beanEntityService;
            this._accountTypeService = accountTypeService;
            this._receiptDetailService = receiptDetailService;
            this._autoNumberService = autoNumberService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._invoiceService = invoiceService;
            this._debitNoteService = debitNoteService;
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._journalService = journalService;
            this._journalDetailService = journalDetailService;
            this._autoService = autoService;

        }
        #endregion

        #region Grid Calls
        public List<ReceiptModel> GetAllReceipts(long companyId)
        {
            List<ReceiptModel> lstreceipt = new List<ReceiptModel>();
            var receipts = _receiptService.GetAllReceiptModel(companyId);
            foreach (var receipt in receipts)
            {
                ReceiptModel receiptModel = new ReceiptModel();
                FillReceiptModel(receiptModel, receipt);
                lstreceipt.Add(receiptModel);
            }
            return lstreceipt;
        }
        #endregion

        #region Create And LookUp Call
        public ReceiptModelLU GetAllReceiptLUs(string userName, Guid receiptId, long companyid, DateTime? docdate = null)
        {
            Receipt lastReceipt = _receiptService.CreateReceipt(companyid);
            Receipt receipt = _receiptService.GetReceipt(receiptId, companyid);
            DateTime date = lastReceipt?.DocDate ?? receipt?.DocDate ?? DateTime.Now;
            ReceiptModelLU receiptLU = new ReceiptModelLU();
            receiptLU.CompanyId = companyid;
            date = docdate != null ? (DateTime)docdate : date;

            if (receipt != null)
            {
                string currencyCode = receipt.DocCurrency;
                receiptLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyid, currencyCode,
                    ControlCodeConstants.Currency_DefaultCode);
                receiptLU.ModeOfReceiptLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyid,
                ControlCodeConstants.Control_codes_ModeOfTransfer, receipt.ModeOfReceipt);
            }
            else
            {
                receiptLU.ModeOfReceiptLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyid,
                ControlCodeConstants.Control_codes_ModeOfTransfer, string.Empty);
                receiptLU.CurrencyLU = _currencyService.GetByCurrencies(companyid, ControlCodeConstants.Currency_DefaultCode);
            }
            long comp = receipt == null ? 0 : receipt.ServiceCompanyId;
            AppsWorld.ReceiptModule.Entities.FinancialSetting financial = _financialSettingService.GetFinancialSetting(companyid);
            AppsWorld.CommonModule.Entities.AccountType account = _accountTypeService.GetCashBankAccountbyName(companyid, AccountNameConstants.Cash_and_bank_balances);
            receiptLU.SubsideryCompanyLU = _companyService.GetCompany(userName, companyid, comp).Select(x => new LookUpCompany<string>()
            {
                Id = x.Id,
                Name = x.Name,
                ShortName = x.ShortName,
                isGstActivated = x.IsGstSetting,
                LookUps = receiptId == Guid.Empty ? account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && c.IsLinkedAccount != null && c.IsRevaluation == null && c.Status == RecordStatusEnum.Active).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    Name = (a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,//x.ShortName + '-' +
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = a.SubsidaryCompanyId,
                    Status = a.Status

                }).ToList() : account.ChartOfAccounts.Where(c => (c.SubsidaryCompanyId == x.Id || c.SubsidaryCompanyId == null) && (/*c.IsSystem == false ||*/ c.IsLinkedAccount != null && c.IsRevaluation == null) && (c.Status == RecordStatusEnum.Active || c.Id == receipt.COAId)).Select(a => new CommonModule.Infra.LookUp<string>()
                {
                    Id = a.Id,
                    Name = (a.Name == COANameConstants.Petty_Cash || a.Name == COANameConstants.Fixed_Deposit) ? a.Name + "(" + (a.Currency != null ? a.Currency : financial.BaseCurrency) + ")" : a.Name,//x.ShortName + '-' +
                    Currency = a.Currency != null ? a.Currency : financial.BaseCurrency,
                    Code = a.Code,
                    ServiceCompanyId = a.SubsidaryCompanyId,
                    Status = a.Status
                }).ToList()
            }).OrderBy(x => x.ShortName).ToList();

            #region Commented
            //List<COALookup<string>> lstEditCoa = null;
            //List<TaxCodeLookUp<string>> lstEditTax = null;
            //List<string> coaName = new List<string> { COANameConstants.General_and_admin_expenses, COANameConstants.Interest_expense, COANameConstants.Rounding, COANameConstants.Interest_income, COANameConstants.Operating_expenses, COANameConstants.Other_expenses, COANameConstants.Other_income, COANameConstants.Sales_and_marketing_expenses };
            //List<long> accType = _accountTypeService.GetAllAccounyTypeByNameByID(companyid, coaName);
            //List<AppsWorld.ReceiptModule.Entities.ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountType(accType);
            //receiptLU.AllChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
            //{
            //    Name = x.Name,
            //    Id = x.Id,
            //    Code = x.Code,
            //    RecOrder = x.RecOrder,
            //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
            //    IsPLAccount = x.Category == "Income Statement" ? true : false,
            //    Class = x.Class
            //}).ToList();
            //List<AppsWorld.ReceiptModule.Entities.TaxCode> allTaxCodes = _taxCodeService.GetTaxCodes(0);
            //if (allTaxCodes.Any())
            //    receiptLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
            //    {
            //        Id = x.Id,
            //        Code = x.Code,
            //        Name = x.Name,
            //        TaxRate = x.TaxRate,
            //        TaxType = x.TaxType,
            //        Status = x.Status,
            //        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
            //    }).OrderBy(c => c.Code).ToList();
            //if (receipt != null && receipt.ReceiptBalancingItems.Count > 0)
            //{
            //    List<long> CoaIds = receipt.ReceiptBalancingItems.Select(c => c.COAId).ToList();
            //    List<long?> taxIds = receipt.ReceiptBalancingItems.Select(x => x.TaxId).ToList();

            //    if (CoaIds.Any())
            //        lstEditCoa = chartofaAccount.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
            //        {
            //            Name = x.Name,
            //            Id = x.Id,
            //            Code = x.Code,
            //            RecOrder = x.RecOrder,
            //            IsAllowDisAllow = x.DisAllowable == true ? true : false,
            //            IsPLAccount = x.Category == "Income Statement" ? true : false,
            //            Class = x.Class
            //        })/*.ToList())*/.ToList();
            //    receiptLU.AllChartOfAccountLU.AddRange(lstEditCoa);
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
            //        receiptLU.TaxCodeLU.AddRange(lstEditTax);
            //    }
            //}
            #endregion Commented 

            List<COALookup<string>> lstEditCoa = null;
            List<TaxCodeLookUp<string>> lstEditTax = null;
            List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
            List<CommonModule.Entities.AccountType> accType = _accountTypeService.GetAllAccountTypeNameByCompanyId(companyid, coaName);
            List<COALookup<string>> lstCoas = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
            {
                Name = x.Name,
                Id = x.Id,
                RecOrder = x.RecOrder,
                IsAllowDisAllow = x.DisAllowable,
                IsPLAccount = x.Category == "Income Statement",
                Class = x.Class,
                Status = x.Status,
                IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
            }).OrderBy(d => d.Name)).ToList();
            receiptLU.AllChartOfAccountLU = lstCoas.OrderBy(s => s.Name).ToList();
            List<CommonModule.Entities.TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyid, date);
            if (allTaxCodes.Any())
                receiptLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    TaxRate = x.TaxRate,
                    TaxType = x.TaxType,
                    Status = x.Status,
                    TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                }).OrderBy(c => c.Code).ToList();
            if (receipt != null && receipt.ReceiptBalancingItems.Count > 0)
            {
                List<long> CoaIds = receipt.ReceiptBalancingItems.Select(c => c.COAId).ToList();
                if (receiptLU.AllChartOfAccountLU.Any())
                    CoaIds = CoaIds.Except(receiptLU.AllChartOfAccountLU.Select(x => x.Id)).ToList();
                List<long?> taxIds = receipt.ReceiptBalancingItems.Select(x => x.TaxId).ToList();
                if (receiptLU.TaxCodeLU != null && receiptLU.TaxCodeLU.Any())
                    taxIds = taxIds.Except(receiptLU.TaxCodeLU.Select(d => d.Id)).ToList();
                if (CoaIds.Any())
                {

                    lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsAllowDisAllow = x.DisAllowable,
                        IsPLAccount = x.Category == "Income Statement",
                        Status = x.Status,
                        IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                    }).OrderBy(d => d.Name)).ToList();
                    receiptLU.AllChartOfAccountLU.AddRange(lstEditCoa);
                }
                if (receipt.IsGstSettings && taxIds.Any())
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
                    receiptLU.TaxCodeLU.AddRange(lstEditTax);
                    var data = receiptLU.TaxCodeLU;
                    receiptLU.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                }
            }
            return receiptLU;


        }
        public CurrencyLU GetCurrencyLookup(Guid receiptId, Guid entityId, long companyId, string baseCurrency, string bankCurrency, bool isMultyCurrency)
        {
            try
            {
                CurrencyLU currencyLU = new CurrencyLU();
                if (isMultyCurrency)
                    GetCurrecnyValue(entityId, companyId, baseCurrency, bankCurrency, currencyLU, receiptId);
                else
                {
                    List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> lstLookup1 = new List<AppsWorld.CommonModule.Infra.LookUpCategory<string>>();
                    AppsWorld.CommonModule.Infra.LookUpCategory<string> lookup = new AppsWorld.CommonModule.Infra.LookUpCategory<string>();
                    lookup.Code = baseCurrency;
                    lstLookup1.Add(lookup);
                    currencyLU.CurrencyLu = lstLookup1;
                }
                if (!currencyLU.CurrencyLu.Any() && currencyLU.CurrencyLu.Count == 0)
                    currencyLU.CurrencyLu.Add(new CommonModule.Infra.LookUpCategory<string>() { Code = bankCurrency });
                return currencyLU;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void GetCurrecnyValue(Guid entityId, long companyId, string baseCurrency, string bankCurrency, CurrencyLU currencyLU, Guid receiptId)
        {
            List<string> allCurrencies = new List<string>();
            allCurrencies.Add(baseCurrency);
            List<string> lstinvoice = null;
            if (baseCurrency == bankCurrency)
            {
                if (receiptId == new Guid())
                    lstinvoice = _invoiceService.GetByEntityId(entityId, companyId);
                else
                    lstinvoice = _invoiceService.GetByStateandEntity(entityId, companyId);
                //lstinvoice = lstinvoice.GroupBy(c => c.DocCurrency).Select(c => c.First()).ToList();
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);

                //Bill
                if (receiptId == new Guid())
                    lstinvoice = _receiptService.GetByBillId(entityId, companyId);
                else
                    lstinvoice = _receiptService.GetByStateandBillEntity(entityId, companyId);
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);

                //Credit Memo
                if (receiptId == new Guid())
                    lstinvoice = _receiptService.GetByCreditMemoId(entityId, companyId);
                else
                    lstinvoice = _receiptService.GetByStateandCreditMemoEntity(entityId, companyId);
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);


                //{
                //    currencyLU.CurrencyLu = lstinvoice.Select(x => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                //    {
                //        Code = x.DocCurrency
                //    }).ToList();
                //}
                if (receiptId == new Guid())
                    lstinvoice = _debitNoteService.GetByEntityId(entityId, companyId);
                else
                    lstinvoice = _debitNoteService.GetByIdState(entityId, companyId);
                //lstDebitNote = lstDebitNote.GroupBy(c => c.DocCurrency).Select(c => c.First()).ToList();
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);
                if (receiptId != new Guid())
                    lstinvoice = _receiptService.GetReceipts(receiptId, companyId).ReceiptDetails.Select(c => c.Currency).ToList();
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);
                //{
                //    List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> lstCurrencyLu = new List<AppsWorld.CommonModule.Infra.LookUpCategory<string>>();
                //    if (currencyLU.CurrencyLu != null && currencyLU.CurrencyLu.Any())
                //    {
                //        lstCurrencyLu = (lstDebitNote.Select(x => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                //        {
                //            Code = x.DocCurrency
                //        }).ToList());
                //    }
                //    else
                //    {
                //        lstCurrencyLu = (lstDebitNote.Select(x => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                //        {
                //            Code = x.DocCurrency
                //        }).ToList());
                //    }
                //    if (currencyLU.CurrencyLu != null)
                //        currencyLU.CurrencyLu.AddRange(lstCurrencyLu);
                //    else
                //        currencyLU.CurrencyLu = lstCurrencyLu;
                //}
                //if (currencyLU.CurrencyLu != null)
                //{
                //    currencyLU.CurrencyLu = currencyLU.CurrencyLu.GroupBy(c => c.Code).Select(c => c.First()).ToList();
                //}
                if (allCurrencies.Any())
                {
                    currencyLU.CurrencyLu = allCurrencies.GroupBy(d => d).Select(c => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                    {
                        Code = c.Key
                    }).ToList();
                }
            }
            else
            {
                //List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> lstLookup1 = new List<AppsWorld.CommonModule.Infra.LookUpCategory<string>>();
                //AppsWorld.CommonModule.Infra.LookUpCategory<string> lookup1 = new AppsWorld.CommonModule.Infra.LookUpCategory<string>();
                //lookup1.Code = bankCurrency;
                //lstLookup1.Add(lookup1);
                //lookup1 = new AppsWorld.CommonModule.Infra.LookUpCategory<string>();
                //lookup1.Code = baseCurrency;
                //lstLookup1.Add(lookup1);
                //currencyLU.CurrencyLu = lstLookup1;
                //List<string> allCurrencies = new List<string>();

                //Inv
                if (receiptId == new Guid())
                    lstinvoice = _invoiceService.GetAllInvoiceByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                else
                    lstinvoice = _invoiceService.GetAllInvoiceByEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);

                //Bill
                if (receiptId == new Guid())
                    lstinvoice = _receiptService.GetAllBillByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                else
                    lstinvoice = _receiptService.GetAllBillEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);

                //Credit Memo
                if (receiptId == new Guid())
                    lstinvoice = _receiptService.GetAllCreditMemoByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                else
                    lstinvoice = _receiptService.GetAllCreditMemoEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);

                //DN
                if (receiptId == new Guid())
                    lstinvoice = _debitNoteService.GetAllDNByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                else
                    lstinvoice = _debitNoteService.GetAllDNByEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                allCurrencies.AddRange(lstinvoice);
                if (receiptId != new Guid())
                    lstinvoice = _receiptService.GetReceipts(receiptId, companyId).ReceiptDetails.Select(c => c.Currency).ToList();
                if (lstinvoice.Any())
                    allCurrencies.AddRange(lstinvoice);
                if (allCurrencies.Any())
                {
                    currencyLU.CurrencyLu = allCurrencies.GroupBy(d => d).Select(c => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                    {
                        Code = c.Key
                    }).ToList();
                }
            }
        }

        public ReceiptModel CreateReceipt(Guid id, long companyId, string username, string connectionString)
        {
            ReceiptModel receiptModel = new ReceiptModel();
            try
            {
                AppsWorld.ReceiptModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new InvalidOperationException(CommonConstant.The_Financial_setting_should_be_activated);
                }
                receiptModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                receiptModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Receipt receipt = _receiptService.GetReceipt(id, companyId);
                if (receipt == null)
                {
                    receiptModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.Receipt);
                    if (receiptModel.IsDocNoEditable == true)
                        receiptModel.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.Receipt, connectionString);
                    FillNewReceiptModel(receiptModel, financSettings/*, _autoNo*/);
                    receiptModel.IsLocked = false;
                }
                else
                {
                    if (!_companyService.GetPermissionBasedOnUser(receipt.ServiceCompanyId, receipt.CompanyId, username))
                        throw new InvalidOperationException(CommonConstant.Access_denied);
                    FillReceiptModel(receiptModel, receipt);
                    receiptModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.Receipt);
                    receiptModel.IsLocked = receipt.IsLocked;

                    receiptModel.ReceiptDetailModels = CreateReceiptModel(id, companyId, username).OrderBy(x => x.DocumentDate).ThenBy(c => c.SystemReferenceNumber).ToList();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return receiptModel;
        }

        public ReceiptModel CreateReceiptNew(Guid id, long companyId, string username, string connectionString)
        {
            ReceiptModel receiptModel = new ReceiptModel();
            try
            {
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptApplicationService, ReceiptLoggingValidation.Log_Receipts_CreateReceiptsNewApplication_Request_Message);
                #region DBHits and Validations

                AppsWorld.ReceiptModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                bool? autonumEditOrNot = _autoNumberService.GetAutoNumberFlag(companyId, DocTypeConstants.Receipt);
                if (financSettings == null)
                {
                    throw new InvalidOperationException(CommonConstant.The_Financial_setting_should_be_activated);
                }
                receiptModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                receiptModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Receipt receipt = _receiptService.GetReceipt(id, companyId);
                var receiptDetails = receipt?.ReceiptDetails.ToList();

                Invoice invoice = null;
                string entityName = null;
                decimal? creditTermDecimal = null;
                bool seviceEntityid = false;
                List<Invoice> lstAllInvoice = null;
                List<DebitNote> lstAllDN = null;
                List<BillCompact> lstAllBill = null;
                List<CreditMemoCompact> lstAllCM = null;
                Dictionary<long, RecordStatusEnum> entityStatus = null;
                Dictionary<long, string> lstCompanies = null;
                Dictionary<long, string> lstSubCompanies = null;
                if (receipt != null)
                {
                    seviceEntityid = _companyService.GetPermissionBasedOnUser(receipt.ServiceCompanyId, receipt.CompanyId, username);
                    invoice = _invoiceService.GetInvoiceByDocumentByState(receipt.Id);
                    entityName = _beanEntityService.GetEntityNameById(receipt.EntityId);
                    creditTermDecimal = _beanEntityService.GetEntityCreditTermsValue(receipt.EntityId);

                    //CreateRecepitModel

                    lstAllInvoice = _invoiceService.GetAllInvoiceByDocId(receiptDetails.Where(d => d.DocumentType == DocTypeConstants.Invoice || d.DocumentType == DocTypeConstants.CreditNote && d.ReceiptAmount != 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                    lstAllDN = _debitNoteService.GetAllDebitNoteById(receiptDetails.Where(d => d.DocumentType == DocTypeConstants.DebitNote && d.ReceiptAmount > 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                    entityStatus = _companyService.GetAllCompaniesStatus(receiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                    if (receipt.IsVendor == true)
                    {
                        lstAllBill = _debitNoteService.GetAllBillsById(receiptDetails.Where(d => d.DocumentType == DocTypeConstants.Bills && d.ReceiptAmount != 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                        lstAllCM = _debitNoteService.GetAllCreditMemoById(receiptDetails.Where(d => d.DocumentType == DocTypeConstants.BillCreditMemo && d.ReceiptAmount > 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                    }
                    lstCompanies = _companyService.GetAllCompanies(receiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                    lstSubCompanies = _companyService.GetAllSubCompanies(receiptDetails.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId);
                }
                #endregion
                if (receipt == null)
                {
                    receiptModel.IsDocNoEditable = autonumEditOrNot;
                    if (receiptModel.IsDocNoEditable == true)
                        receiptModel.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.Receipt, connectionString);
                    FillNewReceiptModel(receiptModel, financSettings);
                    receiptModel.IsLocked = false;
                }
                else
                {
                    if (!seviceEntityid)
                        throw new InvalidOperationException(CommonConstant.Access_denied);
                    FillReceiptModelNew(receiptModel, receipt, invoice, entityName, creditTermDecimal);
                    receiptModel.IsDocNoEditable = autonumEditOrNot;
                    receiptModel.IsLocked = receipt.IsLocked; receiptModel.ReceiptDetailModels = CreateReceiptModelNew(id, companyId, username, financSettings, receipt, receiptDetails, lstAllInvoice, lstAllDN, lstAllBill, lstAllCM, entityStatus, lstCompanies, lstSubCompanies).OrderBy(x => x.DocumentDate).ThenBy(c => c.SystemReferenceNumber).ToList();
                }
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptApplicationService, ReceiptLoggingValidation.Log_Receipts_CreateReceiptsNewApplication_Request_Message_Completed);
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                LoggingHelper.LogError(ReceiptLoggingValidation.ReceiptApplicationService, ex, ex.Message);
                throw ex;
            }
            return receiptModel;
        }
        public ReceiptDetailModel GetReceiptDetail(Guid id, Guid receiptId)
        {
            ReceiptDetailModel rdModel = new ReceiptDetailModel();
            var receiptDetail = _receiptDetailService.GetReceiptDetail(id, receiptId);
            if (receiptDetail != null)
            {
                FillReceiptDetail(rdModel, receiptDetail);
            }
            else
            {
                rdModel.Id = Guid.NewGuid();
                rdModel.ReceiptId = receiptId;
            }
            return rdModel;
        }
        public ReceiptBalancingItemModel GetRBIModel(Guid id, Guid receiptId)
        {
            ReceiptBalancingItemModel rBIModel = new ReceiptBalancingItemModel();
            return rBIModel;
        }

        private List<ReceiptDetailModel> CreateReceiptModel(Guid receiptId, long companyId, string username)
        {

            AppsWorld.ReceiptModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
            if (financSettings == null)
            {
                throw new InvalidOperationException(CommonConstant.The_Financial_setting_should_be_activated);
            }
            Receipt receipt = _receiptService.GetReceipt(receiptId, companyId);
            Dictionary<long, string> lstCompanies = null;
            List<ReceiptDetailModel> lstRDetailModel = new List<ReceiptDetailModel>();
            List<ReceiptDetail> lstReceiptDetails = _receiptDetailService.GetByReceiptId(receiptId);
            if (lstReceiptDetails.Count > 0)
            {
                lstCompanies = _companyService.GetAllCompanies(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                //If the state of the receipt is void, then no need the fetch dynamically. We can fetch directly from here. 
                if (receipt.DocumentState == CommonModule.Infra.ReceiptState.Void)
                {
                    foreach (ReceiptDetail detail in lstReceiptDetails.Where(a => a.ReceiptAmount > 0).ToList())
                    {
                        ReceiptDetailModel receiptDetailModel = new ReceiptDetailModel();
                        receiptDetailModel.Id = detail.Id;
                        receiptDetailModel.ReceiptId = detail.ReceiptId;
                        receiptDetailModel.DocumentNo = detail.DocumentNo;
                        receiptDetailModel.DocumentType = detail.DocumentType;
                        receiptDetailModel.DocumentState = detail.DocumentState;
                        receiptDetailModel.DocumentDate = detail.DocumentDate;
                        receiptDetailModel.DocumentId = detail.DocumentId;
                        receiptDetailModel.AmmountDue = detail.AmmountDue;
                        receiptDetailModel.DocumentAmmount = detail.DocumentAmmount;
                        receiptDetailModel.Currency = detail.Currency;
                        receiptDetailModel.RoundingAmount = detail.RoundingAmount;
                        receiptDetailModel.ServiceCompanyId = detail.ServiceCompanyId;
                        receiptDetailModel.ReceiptAmount = (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills) ? -detail.ReceiptAmount : detail.ReceiptAmount;

                        if (detail.ServiceCompanyId != null)
                        {
                            receiptDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == receiptDetailModel.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                        }
                        lstRDetailModel.Add(receiptDetailModel);
                    }
                }
                else
                {
                    List<Invoice> lstAllInvoice = _invoiceService.GetAllInvoiceByDocId(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.Invoice || d.DocumentType == DocTypeConstants.CreditNote && d.ReceiptAmount != 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                    List<DebitNote> lstAllDN = _debitNoteService.GetAllDebitNoteById(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.DebitNote && d.ReceiptAmount > 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                    Dictionary<long, RecordStatusEnum> entityStatus = _companyService.GetAllCompaniesStatus(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                    List<BillCompact> lstAllBill = null;
                    List<CreditMemoCompact> lstAllCM = null;
                    if (receipt.IsVendor == true)
                    {
                        lstAllBill = _debitNoteService.GetAllBillsById(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.Bills && d.ReceiptAmount != 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                        lstAllCM = _debitNoteService.GetAllCreditMemoById(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.BillCreditMemo && d.ReceiptAmount > 0).Select(c => c.DocumentId).ToList(), receipt.CompanyId, receipt.DocCurrency, receipt.EntityId);
                    }
                    lstCompanies = _companyService.GetAllCompanies(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                    Dictionary<long, string> lstSubCompanies = _companyService.GetAllSubCompanies(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId);
                    Dictionary<long, string> lstComp = lstCompanies.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                    foreach (var rDetail in lstReceiptDetails.Where(c => c.ReceiptAmount != 0))
                    {
                        ReceiptDetailModel RDetailModel = new ReceiptDetailModel();
                        RDetailModel.Id = rDetail.Id;
                        RDetailModel.ReceiptId = rDetail.ReceiptId;
                        RDetailModel.RoundingAmount = rDetail.RoundingAmount;
                        RDetailModel.RecOrder = rDetail.RecOrder;
                        if (rDetail.DocumentType == DocTypeConstants.Invoice || rDetail.DocumentType == DocTypeConstants.CreditNote)
                        {
                            Invoice invoice = lstAllInvoice?.FirstOrDefault(c => c.Id == rDetail.DocumentId);

                            if (invoice != null)
                            {
                                RDetailModel.Id = rDetail.Id;
                                RDetailModel.DocumentDate = invoice.DocDate;
                                RDetailModel.DocumentNo = invoice.DocNo;
                                RDetailModel.DocumentState = invoice.DocumentState;
                                RDetailModel.DocumentAmmount = invoice.DocType == DocTypeConstants.CreditNote ? -invoice.GrandTotal : invoice.GrandTotal;
                                RDetailModel.Nature = invoice.Nature;
                                if (entityStatus.Any())
                                    RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void ? (invoice.DocType == DocTypeConstants.CreditNote ? -invoice.BalanceAmount : invoice.BalanceAmount) : (invoice.DocType == DocTypeConstants.CreditNote ? -(invoice.BalanceAmount + rDetail.ReceiptAmount) : invoice.BalanceAmount + rDetail.ReceiptAmount);
                                RDetailModel.ReceiptAmount = rDetail.DocumentType == DocTypeConstants.CreditNote ? -rDetail.ReceiptAmount : rDetail.ReceiptAmount;
                                RDetailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                                RDetailModel.Currency = invoice.DocCurrency;
                                RDetailModel.BaseExchangeRate = invoice.ExchangeRate;
                                if (rDetail.ServiceCompanyId != null)
                                {
                                    RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                    RDetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == rDetail.ServiceCompanyId).Any() ? false : true : true;
                                }
                                RDetailModel.DocumentId = rDetail.DocumentId;
                                RDetailModel.DocumentType = rDetail.DocumentType == DocTypeConstants.CreditNote ? DocTypeConstants.CreditNote : DocTypeConstants.Invoice;
                                lstRDetailModel.Add(RDetailModel);
                            }
                        }
                        else if (rDetail.DocumentType == DocTypeConstants.DebitNote)
                        {
                            DebitNote debitNote = lstAllDN?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                            if (debitNote != null)
                            {
                                RDetailModel.Id = rDetail.Id;
                                RDetailModel.DocumentDate = debitNote.DocDate;
                                RDetailModel.DocumentNo = debitNote.DocNo;
                                RDetailModel.DocumentState = debitNote.DocumentState;
                                RDetailModel.DocumentAmmount = debitNote.GrandTotal;
                                RDetailModel.Nature = debitNote.Nature;
                                RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void ? debitNote.BalanceAmount : debitNote.BalanceAmount + rDetail.ReceiptAmount;
                                RDetailModel.ReceiptAmount = rDetail.ReceiptAmount;
                                RDetailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                                RDetailModel.Currency = debitNote.DocCurrency;
                                RDetailModel.BaseExchangeRate = debitNote.ExchangeRate;
                                RDetailModel.DocumentId = rDetail.DocumentId;
                                if (rDetail.ServiceCompanyId != null)
                                {
                                    RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                    RDetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == rDetail.ServiceCompanyId).Any() ? false : true : true;
                                }
                                if (entityStatus.Any())
                                    RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                RDetailModel.DocumentType = DocTypeConstants.DebitNote;
                                lstRDetailModel.Add(RDetailModel);
                            }
                        }
                        if (receipt.IsVendor == true)
                        {
                            if (rDetail.DocumentType == DocTypeConstants.Bills)
                            {
                                BillCompact bill = lstAllBill?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                                if (bill != null)
                                {
                                    RDetailModel.Id = rDetail.Id;
                                    RDetailModel.DocumentDate = bill.PostingDate;
                                    RDetailModel.DocumentNo = bill.DocNo;
                                    RDetailModel.DocumentState = bill.DocumentState;
                                    RDetailModel.DocumentAmmount = -bill.GrandTotal;
                                    RDetailModel.Nature = bill.Nature;
                                    RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void ? -bill.BalanceAmount.Value : -(bill.BalanceAmount.Value + rDetail.ReceiptAmount);
                                    RDetailModel.ReceiptAmount = -rDetail.ReceiptAmount;
                                    RDetailModel.SystemReferenceNumber = bill.DocNo;
                                    RDetailModel.Currency = bill.DocCurrency;
                                    RDetailModel.BaseExchangeRate = bill.ExchangeRate;
                                    RDetailModel.DocumentId = rDetail.DocumentId;
                                    if (entityStatus.Any())
                                        RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    if (rDetail.ServiceCompanyId != null)
                                    {
                                        RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                        RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                        RDetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == rDetail.ServiceCompanyId).Any() ? false : true : true;
                                    }
                                    RDetailModel.DocumentType = DocTypeConstants.Bills;
                                    lstRDetailModel.Add(RDetailModel);
                                }
                            }
                            else if (rDetail.DocumentType == DocTypeConstants.BillCreditMemo)
                            {
                                CreditMemoCompact creditMemo = lstAllCM?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                                if (creditMemo != null)
                                {
                                    RDetailModel.Id = rDetail.Id;
                                    RDetailModel.DocumentDate = creditMemo.DocDate;
                                    RDetailModel.DocumentNo = creditMemo.DocNo;
                                    RDetailModel.DocumentState = creditMemo.DocumentState;
                                    RDetailModel.DocumentAmmount = creditMemo.GrandTotal;
                                    RDetailModel.Nature = creditMemo.Nature;
                                    RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void ? creditMemo.BalanceAmount : creditMemo.BalanceAmount + rDetail.ReceiptAmount;
                                    RDetailModel.ReceiptAmount = rDetail.ReceiptAmount;
                                    RDetailModel.SystemReferenceNumber = creditMemo.DocNo;
                                    RDetailModel.Currency = creditMemo.DocCurrency;
                                    RDetailModel.BaseExchangeRate = creditMemo.ExchangeRate;
                                    RDetailModel.DocumentId = rDetail.DocumentId;
                                    if (entityStatus.Any())
                                        RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    if (rDetail.ServiceCompanyId != null)
                                    {
                                        RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                        RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                        RDetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == rDetail.ServiceCompanyId).Any() ? false : true : true;
                                    }
                                    RDetailModel.DocumentType = DocTypeConstants.BillCreditMemo;
                                    lstRDetailModel.Add(RDetailModel);
                                }
                            }
                        }
                    }
                }
            }
            return lstRDetailModel;
        }

        private List<ReceiptDetailModel> CreateReceiptModelNew(Guid receiptId, long companyId, string username, AppsWorld.ReceiptModule.Entities.FinancialSetting financSettings, Receipt receipt, List<ReceiptDetail> lstReceiptDetails, List<Invoice> lstAllInvoice, List<DebitNote> lstAllDN, List<BillCompact> lstAllBill, List<CreditMemoCompact> lstAllCM, Dictionary<long, RecordStatusEnum> entityStatus, Dictionary<long, string> lstCompanies, Dictionary<long, string> lstSubCompanies)
        {
            if (financSettings == null)
            {
                throw new InvalidOperationException(CommonConstant.The_Financial_setting_should_be_activated);
            }
            List<ReceiptDetailModel> lstRDetailModel = new List<ReceiptDetailModel>();
            if (lstReceiptDetails.Count > 0)
            {
                if (receipt.DocumentState == CommonModule.Infra.ReceiptState.Void)
                {
                    lstRDetailModel = lstReceiptDetails.Where(a => a.ReceiptAmount > 0)
                                      .Select(detail => new ReceiptDetailModel
                                      {
                                          Id = detail.Id,
                                          ReceiptId = detail.ReceiptId,
                                          DocumentNo = detail.DocumentNo,
                                          DocumentType = detail.DocumentType,
                                          DocumentState = detail.DocumentState,
                                          DocumentDate = detail.DocumentDate,
                                          DocumentId = detail.DocumentId,
                                          AmmountDue = detail.AmmountDue,
                                          DocumentAmmount = detail.DocumentAmmount,
                                          Currency = detail.Currency,
                                          RoundingAmount = detail.RoundingAmount,
                                          ServiceCompanyId = detail.ServiceCompanyId,
                                          ReceiptAmount = (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills) ? -detail.ReceiptAmount : detail.ReceiptAmount,
                                          ServiceCompanyName = detail.ServiceCompanyId != null
                                          ? lstCompanies.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value)
                                          .FirstOrDefault() : null
                                      }).ToList();

                }
                else
                {
                    Dictionary<long, string> lstComp = lstCompanies.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                    foreach (var rDetail in lstReceiptDetails.Where(c => c.ReceiptAmount != 0))
                    {
                        var RDetailModel = new ReceiptDetailModel
                        {
                            Id = rDetail.Id,
                            ReceiptId = rDetail.ReceiptId,
                            RoundingAmount = rDetail.RoundingAmount,
                            RecOrder = rDetail.RecOrder,
                            DocumentId = rDetail.DocumentId,
                            ServiceCompanyId = rDetail.ServiceCompanyId,
                            DocumentType = rDetail.DocumentType
                        };

                        var serviceCompanyName = rDetail.ServiceCompanyId != null
                            ? lstCompanies.FirstOrDefault(c => c.Key == rDetail.ServiceCompanyId).Value
                            : null;

                        var isHyperLinkEnable = rDetail.ServiceCompanyId != null
                            ? !lstComp.Any(c => c.Key == rDetail.ServiceCompanyId)
                            : true;

                        if (entityStatus.Any())
                        {
                            RDetailModel.ServiceEntityStatus = entityStatus.FirstOrDefault(c => c.Key == rDetail.ServiceCompanyId).Value;
                        }

                        switch (rDetail.DocumentType)
                        {
                            case DocTypeConstants.Invoice:
                            case DocTypeConstants.CreditNote:
                                var invoice = lstAllInvoice?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                                if (invoice != null)
                                {
                                    RDetailModel.DocumentDate = invoice.DocDate;
                                    RDetailModel.DocumentNo = invoice.DocNo;
                                    RDetailModel.DocumentState = invoice.DocumentState;
                                    RDetailModel.DocumentAmmount = invoice.DocType == DocTypeConstants.CreditNote ? -invoice.GrandTotal : invoice.GrandTotal;
                                    RDetailModel.Nature = invoice.Nature;
                                    RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void
                                        ? (invoice.DocType == DocTypeConstants.CreditNote ? -invoice.BalanceAmount : invoice.BalanceAmount)
                                        : (invoice.DocType == DocTypeConstants.CreditNote ? -(invoice.BalanceAmount + rDetail.ReceiptAmount) : invoice.BalanceAmount + rDetail.ReceiptAmount);
                                    RDetailModel.ReceiptAmount = rDetail.DocumentType == DocTypeConstants.CreditNote ? -rDetail.ReceiptAmount : rDetail.ReceiptAmount;
                                    RDetailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                                    RDetailModel.Currency = invoice.DocCurrency;
                                    RDetailModel.BaseExchangeRate = invoice.ExchangeRate;
                                    RDetailModel.ServiceCompanyName = serviceCompanyName;
                                    RDetailModel.IsHyperLinkEnable = isHyperLinkEnable;
                                    RDetailModel.DocumentType = rDetail.DocumentType;
                                    lstRDetailModel.Add(RDetailModel);
                                }
                                break;

                            case DocTypeConstants.DebitNote:
                                var debitNote = lstAllDN?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                                if (debitNote != null)
                                {
                                    RDetailModel.DocumentDate = debitNote.DocDate;
                                    RDetailModel.DocumentNo = debitNote.DocNo;
                                    RDetailModel.DocumentState = debitNote.DocumentState;
                                    RDetailModel.DocumentAmmount = debitNote.GrandTotal;
                                    RDetailModel.Nature = debitNote.Nature;
                                    RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void
                                        ? debitNote.BalanceAmount
                                        : debitNote.BalanceAmount + rDetail.ReceiptAmount;
                                    RDetailModel.ReceiptAmount = rDetail.ReceiptAmount;
                                    RDetailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                                    RDetailModel.Currency = debitNote.DocCurrency;
                                    RDetailModel.BaseExchangeRate = debitNote.ExchangeRate;
                                    RDetailModel.ServiceCompanyName = serviceCompanyName;
                                    RDetailModel.IsHyperLinkEnable = isHyperLinkEnable;
                                    RDetailModel.DocumentType = DocTypeConstants.DebitNote;
                                    lstRDetailModel.Add(RDetailModel);
                                }
                                break;

                            case DocTypeConstants.Bills:
                                if (receipt.IsVendor == true)
                                {
                                    var bill = lstAllBill?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                                    if (bill != null)
                                    {
                                        RDetailModel.DocumentDate = bill.PostingDate;
                                        RDetailModel.DocumentNo = bill.DocNo;
                                        RDetailModel.DocumentState = bill.DocumentState;
                                        RDetailModel.DocumentAmmount = -bill.GrandTotal;
                                        RDetailModel.Nature = bill.Nature;
                                        RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void
                                            ? -bill.BalanceAmount.Value
                                            : -(bill.BalanceAmount.Value + rDetail.ReceiptAmount);
                                        RDetailModel.ReceiptAmount = -rDetail.ReceiptAmount;
                                        RDetailModel.SystemReferenceNumber = bill.DocNo;
                                        RDetailModel.Currency = bill.DocCurrency;
                                        RDetailModel.BaseExchangeRate = bill.ExchangeRate;
                                        RDetailModel.ServiceCompanyName = serviceCompanyName;
                                        RDetailModel.IsHyperLinkEnable = isHyperLinkEnable;
                                        RDetailModel.DocumentType = DocTypeConstants.Bills;
                                        lstRDetailModel.Add(RDetailModel);
                                    }
                                }
                                break;

                            case DocTypeConstants.BillCreditMemo:
                                if (receipt.IsVendor == true)
                                {
                                    var creditMemo = lstAllCM?.FirstOrDefault(c => c.Id == rDetail.DocumentId);
                                    if (creditMemo != null)
                                    {
                                        RDetailModel.DocumentDate = creditMemo.DocDate;
                                        RDetailModel.DocumentNo = creditMemo.DocNo;
                                        RDetailModel.DocumentState = creditMemo.DocumentState;
                                        RDetailModel.DocumentAmmount = creditMemo.GrandTotal;
                                        RDetailModel.Nature = creditMemo.Nature;
                                        RDetailModel.AmmountDue = receipt != null && receipt.DocumentState == AppsWorld.ReceiptModule.Infra.ReceiptState.Void
                                            ? creditMemo.BalanceAmount
                                            : creditMemo.BalanceAmount + rDetail.ReceiptAmount;
                                        RDetailModel.ReceiptAmount = rDetail.ReceiptAmount;
                                        RDetailModel.SystemReferenceNumber = creditMemo.DocNo;
                                        RDetailModel.Currency = creditMemo.DocCurrency;
                                        RDetailModel.BaseExchangeRate = creditMemo.ExchangeRate;
                                        RDetailModel.ServiceCompanyName = serviceCompanyName;
                                        RDetailModel.IsHyperLinkEnable = isHyperLinkEnable;
                                        RDetailModel.DocumentType = DocTypeConstants.BillCreditMemo;
                                        lstRDetailModel.Add(RDetailModel);
                                    }
                                }
                                break;
                        }
                    }

                }
            }
            return lstRDetailModel;
        }
        public ReceiptDetailModel GetReceiptDetails(Guid receiptId, Guid EntityId, string currency, long? companyId, long serviceCompanyId, string username, DateTime? docDate, bool isInterCompanyActive, bool isVenor, string ConnectionString)
        {
            ReceiptDetailModel RDetailModel1 = new ReceiptDetailModel();
            List<ReceiptDetailModel> lstRDetailModel = new List<ReceiptDetailModel>();
            List<ReceiptDetail> lstReceiptDetails = null;
            Dictionary<long, string> lstCompanies1 = new Dictionary<long, string>();
            long? serviceEntityId = null;
            string companyName = null;
            bool isIC = false;
            #region Interco_ServiceEntity_Settings_Changes
            using (con = new SqlConnection(ConnectionString))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                query = $"Select ICG.ServiceEntityId as ServiceEntityId,COM.ShortName as Name from Bean.InterCompanySetting IC JOIN Bean.InterCompanySettingDetail ICG on IC.Id=ICG.InterCompanySettingId Left JOIN Common.Company COM On ICG.ServiceEntityId = COM.Id JOIN Common.CompanyUser CU on CU.CompanyId = COM.ParentId JOIN Common.CompanyUserDetail CUD on CUD.CompanyUserId = CU.Id and CUD.ServiceEntityId = COM.Id  where COM.ParentId = {companyId} and IC.InterCompanyType = 'Clearing' and ICG.Status=1 and CU.username ='{username}'";
                cmd = new SqlCommand(query, con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        serviceEntityId = dr["ServiceEntityId"] != null ? Convert.ToInt64(dr["ServiceEntityId"]) : (long?)null;
                        companyName = dr["Name"].ToString();
                        if (serviceEntityId != null)
                            lstCompanies1.Add(serviceEntityId.Value, companyName);
                    }
                }
            }
            if (lstCompanies1.Any())
                isIC = lstCompanies1.Any(c => c.Key == serviceCompanyId);

            #endregion Interco_ServiceEntity_Settings_Changes

            if (isInterCompanyActive)
                lstReceiptDetails = _receiptDetailService.GetReceiptDetailById(receiptId, docDate, currency);
            else
                lstReceiptDetails = _receiptDetailService.GetByReceiptIdSerId(receiptId, serviceCompanyId, docDate, currency);
            Receipt receipt = _receiptService.GetReceiptById(receiptId, EntityId, companyId.Value);



            if (lstReceiptDetails.Count > 0 && receipt != null)
            {
                List<Invoice> lstAllInvoice = _invoiceService.GetAllInvoiceByDocId(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.Invoice || d.DocumentType == DocTypeConstants.CreditNote).Select(c => c.DocumentId).ToList(), companyId.Value, currency, EntityId);
                List<DebitNote> lstAllDN = _debitNoteService.GetAllDebitNoteById(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList(), companyId.Value, currency, EntityId);

                List<BillCompact> lstAllBill = null;
                List<CreditMemoCompact> lstAllCM = null;
                if (isVenor)
                {
                    lstAllBill = _debitNoteService.GetAllBillsById(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.Bills).Select(c => c.DocumentId).ToList(), companyId.Value, currency, EntityId);
                    lstAllCM = _debitNoteService.GetAllCreditMemoById(lstReceiptDetails.Where(d => d.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList(), companyId.Value, currency, EntityId);
                }
                Dictionary<long, string> lstCompanies = _companyService.GetAllCompanies(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                Dictionary<long, string> lstSubCompanies = _companyService.GetAllSubCompanies(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId.Value);
                Dictionary<long, RecordStatusEnum> entityStatus = _companyService.GetAllCompaniesStatus(lstReceiptDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                Dictionary<long, string> lstComp = lstCompanies.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                foreach (var rDetail in lstReceiptDetails)
                {
                    ReceiptDetailModel RDetailModel = new ReceiptDetailModel();
                    RDetailModel.Id = rDetail.Id;
                    RDetailModel.ReceiptId = rDetail.ReceiptId;
                    RDetailModel.RoundingAmount = rDetail.RoundingAmount;
                    if (rDetail.DocumentType == DocTypeConstants.Invoice || rDetail.DocumentType == DocTypeConstants.CreditNote)
                    {
                        Invoice invoice = lstAllInvoice?.Find(c => c.Id == rDetail.DocumentId);
                        if (invoice != null)
                        {
                            RDetailModel.Id = rDetail.Id;
                            RDetailModel.DocumentDate = invoice.DocDate;
                            RDetailModel.DocumentNo = invoice.DocNo;
                            RDetailModel.DocumentState = invoice.DocumentState;
                            if (entityStatus.Any())
                                RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                            RDetailModel.DocumentAmmount = invoice.DocType == DocTypeConstants.CreditNote ? -invoice.GrandTotal : invoice.GrandTotal;
                            RDetailModel.Nature = invoice.Nature;
                            RDetailModel.AmmountDue = invoice.DocType == DocTypeConstants.CreditNote ? -(invoice.BalanceAmount + rDetail.ReceiptAmount) : invoice.BalanceAmount + rDetail.ReceiptAmount;
                            RDetailModel.ReceiptAmount = invoice.DocType == DocTypeConstants.CreditNote ? -rDetail.ReceiptAmount : rDetail.ReceiptAmount;
                            RDetailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                            RDetailModel.Currency = invoice.DocCurrency;
                            RDetailModel.BaseExchangeRate = invoice.ExchangeRate;
                            if (rDetail.ServiceCompanyId != null)
                            {
                                RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                RDetailModel.IsHyperLinkEnable = !lstComp.Any() || !lstComp.Any(c => c.Key == rDetail.ServiceCompanyId);
                            }
                            RDetailModel.DocumentId = rDetail.DocumentId;
                            RDetailModel.DocumentType = invoice.DocType;
                            lstRDetailModel.Add(RDetailModel);
                        }
                    }
                    else if (rDetail.DocumentType == DocTypeConstants.DebitNote)
                    {
                        DebitNote debitNote = lstAllDN?.Find(c => c.Id == rDetail.DocumentId);
                        if (debitNote != null)
                        {
                            RDetailModel.Id = rDetail.Id;
                            RDetailModel.DocumentDate = debitNote.DocDate;
                            RDetailModel.DocumentNo = debitNote.DocNo;
                            RDetailModel.DocumentState = debitNote.DocumentState;
                            RDetailModel.DocumentAmmount = debitNote.GrandTotal;
                            RDetailModel.Nature = debitNote.Nature;
                            if (entityStatus.Any())
                                RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                            RDetailModel.AmmountDue = debitNote.BalanceAmount + rDetail.ReceiptAmount;
                            RDetailModel.ReceiptAmount = rDetail.ReceiptAmount;
                            RDetailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
                            RDetailModel.Currency = debitNote.DocCurrency;
                            RDetailModel.BaseExchangeRate = debitNote.ExchangeRate;
                            RDetailModel.DocumentId = rDetail.DocumentId;
                            if (rDetail.ServiceCompanyId != null)
                            {
                                RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                RDetailModel.IsHyperLinkEnable = !lstComp.Any() || !lstComp.Any(c => c.Key == rDetail.ServiceCompanyId);
                            }
                            RDetailModel.DocumentType = DocTypeConstants.DebitNote;
                            lstRDetailModel.Add(RDetailModel);
                        }
                    }
                    if (isVenor)
                    {
                        if (rDetail.DocumentType == DocTypeConstants.Bills)
                        {
                            BillCompact bill = lstAllBill?.Find(c => c.Id == rDetail.DocumentId);
                            if (bill != null)
                            {
                                RDetailModel.Id = rDetail.Id;
                                RDetailModel.DocumentDate = bill.PostingDate;
                                RDetailModel.DocumentNo = bill.DocNo;
                                RDetailModel.DocumentState = bill.DocumentState;
                                RDetailModel.DocumentAmmount = -bill.GrandTotal;
                                RDetailModel.Nature = bill.Nature;
                                RDetailModel.AmmountDue = -(bill.BalanceAmount.Value + rDetail.ReceiptAmount);
                                RDetailModel.ReceiptAmount = -rDetail.ReceiptAmount;
                                RDetailModel.SystemReferenceNumber = bill.DocNo;
                                RDetailModel.Currency = bill.DocCurrency;
                                if (entityStatus.Any())
                                    RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                RDetailModel.BaseExchangeRate = bill.ExchangeRate;
                                RDetailModel.DocumentId = rDetail.DocumentId;
                                if (rDetail.ServiceCompanyId != null)
                                {
                                    RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                    RDetailModel.IsHyperLinkEnable = !lstComp.Any() || !lstComp.Any(c => c.Key == rDetail.ServiceCompanyId);
                                }
                                RDetailModel.DocumentType = DocTypeConstants.Bills;
                                lstRDetailModel.Add(RDetailModel);
                            }
                        }
                        else if (rDetail.DocumentType == DocTypeConstants.BillCreditMemo)
                        {
                            CreditMemoCompact creditMemo = lstAllCM?.Find(c => c.Id == rDetail.DocumentId);
                            if (creditMemo != null)
                            {
                                RDetailModel.Id = rDetail.Id;
                                RDetailModel.DocumentDate = creditMemo.DocDate;
                                RDetailModel.DocumentNo = creditMemo.DocNo;
                                RDetailModel.DocumentState = creditMemo.DocumentState;
                                RDetailModel.DocumentAmmount = creditMemo.GrandTotal;
                                RDetailModel.Nature = creditMemo.Nature;
                                if (entityStatus.Any())
                                    RDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                RDetailModel.AmmountDue = creditMemo.BalanceAmount + rDetail.ReceiptAmount;
                                RDetailModel.ReceiptAmount = rDetail.ReceiptAmount;
                                RDetailModel.SystemReferenceNumber = creditMemo.DocNo;
                                RDetailModel.Currency = creditMemo.DocCurrency;
                                RDetailModel.BaseExchangeRate = creditMemo.ExchangeRate;
                                RDetailModel.DocumentId = rDetail.DocumentId;
                                if (rDetail.ServiceCompanyId != null)
                                {
                                    RDetailModel.ServiceCompanyName = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    RDetailModel.ServiceCompanyId = lstCompanies.Where(c => c.Key == rDetail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                                    RDetailModel.IsHyperLinkEnable = !lstComp.Any() || !lstComp.Any(c => c.Key == rDetail.ServiceCompanyId);
                                }
                                RDetailModel.DocumentType = DocTypeConstants.BillCreditMemo;
                                lstRDetailModel.Add(RDetailModel);
                            }
                        }
                    }
                }
            }
            List<Invoice> lstInvoice = null;

            if (isIC)
            {
                lstInvoice = _invoiceService.GetInvoiceCNByIdAndDocType(companyId.Value, EntityId, currency, docDate);
            }
            else
                lstInvoice = _invoiceService.GetInvoicesCNByEntity(companyId.Value,
                    EntityId, currency, serviceCompanyId, docDate);
            Dictionary<long, string> lstSubCompanies1 = null;
            Dictionary<long, string> lstComp1 = null;
            if (lstInvoice.Any())
            {
                lstSubCompanies1 = _companyService.GetAllSubCompanies(lstInvoice.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId.Value);
                lstComp1 = lstCompanies1.Except(lstSubCompanies1).ToDictionary(Id => Id.Key, Name => Name.Value);
            }
            foreach (Invoice detail in isIC ? lstInvoice.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstInvoice)
            {
                var d = lstRDetailModel?.Find(a => a.DocumentId == detail.Id);
                if (d == null)
                {
                    ReceiptDetailModel detailModel = new ReceiptDetailModel();
                    detailModel.DocumentNo = detail.DocNo;
                    detailModel.DocumentType = detail.DocType;
                    detailModel.DocumentState = detail.DocumentState;
                    detailModel.DocumentId = detail.Id;
                    detailModel.DocumentDate = detail.DocDate;
                    detailModel.ServiceEntityStatus = RecordStatusEnum.Active;
                    detailModel.DocumentAmmount = detail.DocType == DocTypeConstants.CreditNote ? -detail.GrandTotal : detail.GrandTotal;
                    detailModel.Currency = detail.DocCurrency;
                    detailModel.BaseExchangeRate = detail.ExchangeRate;
                    detailModel.AmmountDue = detail.DocType == DocTypeConstants.CreditNote ? -detail.BalanceAmount : detail.BalanceAmount;
                    detailModel.Nature = detail.Nature;
                    detailModel.SystemReferenceNumber = detail.InvoiceNumber;
                    if (detail.ServiceCompanyId != null)
                    {
                        detailModel.ServiceCompanyName = lstCompanies1 != null ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                        detailModel.ServiceCompanyId = isIC && lstCompanies1 != null ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : serviceCompanyId;
                        detailModel.IsHyperLinkEnable = !lstComp1.Any() || !lstComp1.Any(c => c.Key == detail.ServiceCompanyId);
                    }
                    lstRDetailModel.Add(detailModel);
                }
            }
            List<DebitNote> lstDebitNote = null;
            if (isIC)
            {
                lstDebitNote = _debitNoteService.GetDebitNoteByEntityAndDocdate(companyId.Value, EntityId, currency, docDate);
            }
            else
                lstDebitNote = _debitNoteService.GetDebitNoteByEntity(companyId.Value, EntityId, currency, serviceCompanyId, docDate);
            if (lstDebitNote.Any())
            {
                lstSubCompanies1 = _companyService.GetAllSubCompanies(lstDebitNote.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId.Value);
                lstComp1 = lstCompanies1.Except(lstSubCompanies1).ToDictionary(Id => Id.Key, Name => Name.Value);
            }
            foreach (DebitNote detail in isIC ? lstDebitNote.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstDebitNote)
            {
                var d = lstRDetailModel?.Find(a => a.DocumentId == detail.Id);
                if (d == null)
                {
                    ReceiptDetailModel detailModel = new ReceiptDetailModel();
                    detailModel.DocumentNo = detail.DocNo;
                    detailModel.DocumentType = DocTypeConstants.DebitNote;
                    detailModel.DocumentId = detail.Id;
                    detailModel.DocumentDate = detail.DocDate;
                    detailModel.DocumentState = detail.DocumentState;
                    detailModel.DocumentAmmount = detail.GrandTotal;
                    detailModel.Currency = detail.DocCurrency;
                    detailModel.ServiceEntityStatus = RecordStatusEnum.Active;
                    detailModel.BaseExchangeRate = detail.ExchangeRate;
                    detailModel.AmmountDue = detail.BalanceAmount;
                    detailModel.Nature = detail.Nature;
                    detailModel.SystemReferenceNumber = detail.DebitNoteNumber;
                    if (detail.ServiceCompanyId != null)
                    {
                        detailModel.ServiceCompanyName = lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                        detailModel.ServiceCompanyId = isIC && lstCompanies1 != null ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : serviceCompanyId;
                        detailModel.IsHyperLinkEnable = !lstComp1.Any() || !lstComp1.Any(c => c.Key == detail.ServiceCompanyId);
                    }

                    lstRDetailModel.Add(detailModel);
                }
            }
            if (isVenor)
            {
                List<BillCompact> lstBill = null;
                if (isIC)
                {
                    lstBill = _debitNoteService.GetBillByEntityAndDocdate(companyId.Value, EntityId, currency, docDate);
                }
                else
                    lstBill = _debitNoteService.GetBillByEntity(companyId.Value, EntityId, currency, serviceCompanyId, docDate);
                if (lstBill.Any())
                {
                    lstSubCompanies1 = _companyService.GetAllSubCompanies(lstBill.Select(c => c.ServiceCompanyId).ToList(), username, companyId.Value);
                    lstComp1 = lstCompanies1.Except(lstSubCompanies1).ToDictionary(Id => Id.Key, Name => Name.Value);
                }

                foreach (BillCompact detail in isIC ? lstBill.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId) && a.DocSubType != DocTypeConstants.Payroll) : lstBill.Where(b => b.DocSubType != DocTypeConstants.Payroll))
                {
                    var d = lstRDetailModel?.Find(a => a.DocumentId == detail.Id);
                    if (d == null)
                    {
                        ReceiptDetailModel detailModel = new ReceiptDetailModel();
                        detailModel.DocumentNo = detail.DocNo;
                        detailModel.DocumentType = DocTypeConstants.Bills;
                        detailModel.DocumentId = detail.Id;
                        detailModel.DocumentDate = detail.PostingDate;
                        detailModel.DocumentState = detail.DocumentState;
                        detailModel.DocumentAmmount = -detail.GrandTotal;
                        detailModel.Currency = detail.DocCurrency;
                        detailModel.BaseExchangeRate = detail.ExchangeRate;
                        detailModel.AmmountDue = -detail.BalanceAmount.Value;
                        detailModel.Nature = detail.Nature;
                        detailModel.ServiceEntityStatus = RecordStatusEnum.Active;
                        detailModel.SystemReferenceNumber = detail.DocNo;
                        if (detail.ServiceCompanyId != 0)
                        {
                            detailModel.ServiceCompanyName = lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                            detailModel.ServiceCompanyId = isIC && lstCompanies1 != null ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : serviceCompanyId;
                            detailModel.IsHyperLinkEnable = !lstComp1.Any() || !lstComp1.Any(c => c.Key == detail.ServiceCompanyId);
                        }

                        lstRDetailModel.Add(detailModel);
                    }
                }
                List<CreditMemoCompact> lstCreditMemo = null;
                if (isIC)
                {
                    lstCreditMemo = _debitNoteService.GetCMEntityAndDocdate(companyId.Value, EntityId, currency, docDate);
                }
                else
                    lstCreditMemo = _debitNoteService.GetCMByEntity(companyId.Value, EntityId, currency, serviceCompanyId, docDate);
                if (lstCreditMemo.Any())
                {
                    lstSubCompanies1 = _companyService.GetAllSubCompanies(lstCreditMemo.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId.Value);
                    lstComp1 = lstCompanies1.Except(lstSubCompanies1).ToDictionary(Id => Id.Key, Name => Name.Value);
                }
                foreach (CreditMemoCompact detail in isIC ? lstCreditMemo.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstCreditMemo)
                {
                    var d = lstRDetailModel?.Find(a => a.DocumentId == detail.Id);
                    if (d == null)
                    {
                        ReceiptDetailModel detailModel = new ReceiptDetailModel();
                        detailModel.DocumentNo = detail.DocNo;
                        detailModel.DocumentType = DocTypeConstants.BillCreditMemo;
                        detailModel.DocumentId = detail.Id;
                        detailModel.DocumentDate = detail.DocDate;
                        detailModel.DocumentState = detail.DocumentState;
                        detailModel.DocumentAmmount = detail.GrandTotal;
                        detailModel.Currency = detail.DocCurrency;
                        detailModel.BaseExchangeRate = detail.ExchangeRate;
                        detailModel.AmmountDue = detail.BalanceAmount;
                        detailModel.Nature = detail.Nature;
                        detailModel.ServiceEntityStatus = RecordStatusEnum.Active;
                        detailModel.SystemReferenceNumber = detail.DocNo;
                        if (detail.ServiceCompanyId != null)
                        {
                            detailModel.ServiceCompanyName = lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                            detailModel.ServiceCompanyId = isIC && lstCompanies1 != null ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : serviceCompanyId;
                            detailModel.IsHyperLinkEnable = !lstComp1.Any() || !lstComp1.Any(c => c.Key == detail.ServiceCompanyId);
                        }

                        lstRDetailModel.Add(detailModel);
                    }
                }
            }
            RDetailModel1.ReceiptDetailModels = lstRDetailModel.OrderBy(c => c.DocumentDate).ThenBy(s => s.SystemReferenceNumber).ToList();
            return RDetailModel1;
        }
        public DocumentVoidModel CreateCreditNoteDocumentVoid(Guid id, long companyId)
        {
            AppsWorld.ReceiptModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
            if (financSettings == null)
                throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);

            Receipt doubtfulDebt = _receiptService.GetReceipt(id, companyId);
            if (doubtfulDebt == null)
                throw new Exception(ReceiptConstant.Invalid_Receipt);

            DocumentVoidModel DVModel = new DocumentVoidModel();
            DVModel.CompanyId = companyId;
            if (doubtfulDebt != null)
                DVModel.Id = doubtfulDebt.Id;
            return DVModel;
        }
        public ReceiptBalancingItem CreateBalancingItem(Guid id, Guid receiptId, long companyId)
        {
            ReceiptBalancingItem rbReceiptBalancingItem = new ReceiptBalancingItem();
            Receipt receipt = _receiptService.GetReceipt(receiptId, companyId);
            if (receipt != null)
            {

                ReceiptBalancingItem RBItem = _receiptBalancingItemService.GetRBI(id, receiptId);
                if (RBItem != null)
                {
                    rbReceiptBalancingItem = RBItem;
                }

            }
            return rbReceiptBalancingItem;
        }
        public ReceiptDetail CreateReceiptDetail(Guid id, Guid receiptId, long companyId)
        {
            ReceiptDetail receiptDetail = new ReceiptDetail();
            Receipt receipt = _receiptService.GetReceipt(receiptId, companyId);
            if (receipt != null)
            {

                ReceiptDetail RDetail = _receiptDetailService.GetReceiptDetail(id, receiptId);
                if (RDetail != null)
                {
                    receiptDetail = RDetail;
                }
            }
            return receiptDetail;
        }
        public bool IsGSTAllowed(long companyId, DateTime docDate)
        {
            AppsWorld.ReceiptModule.Entities.GSTSetting setting = _gstSettingService.GetGSTSettings(companyId);
            bool isGST = setting != null;
            if (isGST)
            {
                if (setting.IsDeregistered != null && setting.IsDeregistered.Value)
                {
                    isGST = docDate < setting.DeRegistration.Value;
                }
            }
            return isGST;
        }

        #endregion

        #region AutuNumber Calls
        string value = "";
        public string GenerateAutoNumberForType(long companyId, string Type, string companyCode)
        {

            AppsWorld.ReceiptModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
            string generatedAutoNumber = "";

            if (Type == "Receipt")
            {
                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                    _autoNo.GeneratedNumber, companyId, companyCode);

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
                    AppsWorld.ReceiptModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AppsWorld.ReceiptModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.ReceiptModule.Entities.AutoNumberCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }
            }
            return generatedAutoNumber;
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
            long companyId, string Companycode = null)
        {
            List<Receipt> lstReceipt = null;
            int? currentMonth = 0;
            bool ifMonthContains = false;

            string OutputNumber = "";
            string counter = "";
            string companyFormatHere = companyFormatFrom.ToUpper();

            if (companyFormatHere.Contains("{YYYY}"))
            {
                companyFormatHere = companyFormatHere.Replace("{YYYY}", DateTime.Now.Year.ToString());
            }
            else if (companyFormatHere.Contains("{MM/YYYY}"))
            {
                companyFormatHere = companyFormatHere.Replace("{MM/YYYY}",
                    string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
                currentMonth = DateTime.Now.Month;
                ifMonthContains = true;
            }
            else if (companyFormatHere.Contains("{COMPANYCODE}"))
            {
                companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
            }
            double val = 0;
            if (Type == DocTypeConstants.Receipt)
            {
                lstReceipt = _receiptService.GetAllReceiptModel(companyId);

                if (lstReceipt.Any() && ifMonthContains)
                {
                    AppsWorld.ReceiptModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastCreateDate = lstReceipt.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (lastCreateDate == currentMonth)
                    {
                        foreach (var bill in lstReceipt)
                        {
                            if (bill.SystemRefNo != autonumber.Preview)
                                val = Convert.ToInt32(IncreamentVal);
                            else
                            {
                                val = Convert.ToInt32(IncreamentVal) + 1;
                                break;
                            }
                        }
                    }
                }

                else if (lstReceipt.Any() && ifMonthContains == false)
                {
                    AppsWorld.ReceiptModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var bill in lstReceipt)
                    {
                        if (bill.SystemRefNo != autonumber.Preview)
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

            if (lstReceipt.Any())
            {
                OutputNumber = GetNewNumber(lstReceipt, Type, OutputNumber, counter, companyFormatHere, counterLength);
            }

            return OutputNumber;
        }
        private string GetNewNumber(List<Receipt> lstReceipt, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstReceipt.Where(a => a.SystemRefNo == outputNumber).FirstOrDefault();
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
                    var inv = lstReceipt.Where(c => c.SystemRefNo == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }
        #endregion

        #region Save Calls
        public Receipt Save(ReceiptModel TObject, string ConnectionString)
        {
            bool isNewAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptApplicationService, "ObjectSave", AdditionalInfo);
                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_SaveReceipt_Method);
                long? serviceCompanyId = null;
                bool? isEdit = false;
                DateTime? oldDocdate = null;
                string _errors = CommonValidation.ValidateObject(TObject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }

                #region Interco_Validation_on_ServiceEntitties
                int entityCount = 0;
                bool isICActive = TObject.ReceiptDetailModels != null && TObject.ReceiptDetailModels.Any(c => c.ServiceCompanyId != TObject.ServiceCompanyId);
                List<long?> lstEntityIds = null;
                if (isICActive)
                {
                    entityCount = TObject.ReceiptApplicationAmmount > 0 ? TObject.ReceiptDetailModels.Where(d => d.ReceiptAmount != 0).Select(c => c.ServiceCompanyId).Distinct().Count() : TObject.ReceiptDetailModels.Select(c => c.ServiceCompanyId).Distinct().Count();
                    lstEntityIds = TObject.ReceiptApplicationAmmount > 0 ? TObject.ReceiptDetailModels.Where(d => d.ReceiptAmount != 0).Select(c => c.ServiceCompanyId).Distinct().ToList() : TObject.ReceiptDetailModels.Select(c => c.ServiceCompanyId).Distinct().ToList();
                    lstEntityIds.Add(TObject.ServiceCompanyId);
                    entityCount = lstEntityIds.Distinct().Count();
                    string lstServEntities = string.Join(",", lstEntityIds.Distinct());
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        query = $"Select COUNT(*) as Count from Bean.InterCompanySetting ICT Inner join Bean.InterCompanySettingDetail ICTD on ICTD.InterCompanySettingId=ICT.Id where ICT.CompanyId={TObject.CompanyId} and ICT.InterCompanyType='Clearing' and ICTD.Status=1 and ICTD.ServiceEntityId in (Select items from dbo.SplitToTable('{lstServEntities}',','))";
                        cmd = new SqlCommand(query, con);
                        int count = (Int32)cmd.ExecuteScalar();
                        con.Close();
                        if (count > 0 && entityCount != count)
                        {
                            throw new InvalidOperationException(CommonConstant.The_State_of_the_service_entity_had_been_changed);
                        }
                    }
                }
                #endregion Interco_Validation_on_ServiceEntitties
                #region Validation checking
                //if the document state is void
                if (_receiptService.IsVoid(TObject.CompanyId, TObject.Id))
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                if (TObject.EntityId == null)
                {
                    throw new InvalidOperationException(CommonConstant.Entity_is_mandatory);
                }
                if (TObject.ServiceCompanyId == 0)
                    throw new InvalidOperationException(CommonConstant.ServiceCompany_is_mandatory);
                if (TObject.DocDate == null)
                {
                    throw new InvalidOperationException(CommonConstant.Invalid_Document_Date);
                }

                //Modified by Pradhan due to Cindi recent(01/04/19) changes
                //if (TObject.BankReceiptAmmount <= 0)
                //    throw new InvalidOperationException(ReceiptConstant.BankReceiptAmount_should_be_greater_than_zero);
                if (TObject.ReceiptBalancingItems.Any())
                {
                    if (TObject.ReceiptBalancingItems.Where(a => a.RecordStatus != "Deleted").Any(balance => balance.DocAmount == null))
                        throw new InvalidOperationException(ReceiptConstant.Please_enter_the_Balancing_Items_amount);
                    if (TObject.ReceiptBalancingItems.Where(a => a.RecordStatus != "Deleted").Any(balance => balance.DocAmount <= 0))
                        throw new InvalidOperationException(ReceiptConstant.Balancing_Item_amount_should_be_greater_than_zero);
                }
                if (TObject.IsDocNoEditable == true && IsDocumentNumberExists(TObject.DocNo, TObject.Id, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CommonConstant.Document_number_already_exist);
                }

                //Need to verify the invoice is within Financial year
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
                {
                    if (string.IsNullOrWhiteSpace(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (
                        !_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword,
                            TObject.CompanyId))
                    {
                        throw new InvalidOperationException(CommonConstant.Invalid_Financial_Period_Lock_Password);
                    }
                }
                #endregion

                bool? isBaseExchangerate = TObject.IsBaseCurrencyRateChanged == true || TObject.SystemCalculatedExchangeRate != null;
                long clearingReceiptCOA = _chartOfAccountService.GetByNameId(ReceiptConstant.Clearing_Receipts, TObject.CompanyId);
                Receipt _receipt = _receiptService.CheckReceiptById(TObject.Id);
                List<DocumentHistoryModel> lstdocuments = new List<DocumentHistoryModel>();
                Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
                if (_receipt != null)
                {
                    //Data Concorancy check
                    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_receipt.Version, x => x.ToString("X2")));
                    if (!timeStamp.Equals(TObject.Version))
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                    decimal? oldExchangeRate = _receipt.ExchangeRate;
                    decimal? oldSysCalExRate = _receipt.SystemCalculatedExchangeRate;
                    decimal? excessPaidAmount = _receipt.ExcessPaidByClientAmmount;
                    isEdit = true;
                    serviceCompanyId = _receipt.ServiceCompanyId;
                    oldDocdate = _receipt.DocDate;
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_If_Condition_Of_The_Receipt_And_Check_Receipt_Is_Not_Null);
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Insert_Receipt_Method);
                    decimal? docTotal = _receipt.GrandTotal - TObject.GrandTotal;
                    InsertReceipt(TObject, _receipt);
                    TObject.CustCreditlimit -= docTotal;
                    _receipt.DocNo = TObject.DocNo;
                    _receipt.SystemRefNo = _receipt.DocNo;
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Come_Out_From_Insert_Receipt_Bill);
                    _receipt.ModifiedBy = TObject.ModifiedBy;
                    _receipt.ModifiedDate = DateTime.UtcNow;
                    _receipt.ObjectState = ObjectState.Modified;
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Update_Receipt_Details_Method);
                    UpdateReceiptDetails(TObject, _receipt, isEdit, ConnectionString, clearingReceiptCOA, isICActive, oldExchangeRate, oldSysCalExRate, lstdocuments, oldDocdate, lstOfRoundingAmount, serviceCompanyId);
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Come_Out_From_Update_Receipt_Details_Method);
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Update_Receipt_Balancing_Items_Method);
                    UpdateReceiptBalancingItems(TObject, _receipt);
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Come_Out_From_Update_Update_Receipt_Balancing_Items_Method);
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Inter_Into_Update_Receipt_GST_Details_Method);
                    _receiptService.UpdateReceipt(_receipt);
                    Invoice inv = _invoiceService.GetInvoiceByDocument(_receipt.Id);
                    if (inv != null)
                    {
                        if (inv.DocumentState == "Not Applied" && (TObject.ExcessPaidByClientAmmount == null || TObject.ExcessPaidByClientAmmount == 0))
                        {
                            DocumentVoidModel documentVoidModel = new DocumentVoidModel();
                            documentVoidModel.Id = inv.Id;
                            documentVoidModel.CompanyId = TObject.CompanyId;
                            documentVoidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                            documentVoidModel.Version = "0x" + string.Concat(Array.ConvertAll(inv.Version, x => x.ToString("X2")));
                            var json = RestSharpHelper.ConvertObjectToJason(documentVoidModel);
                            try
                            {
                                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();

                                RestSharpHelper.Post(url, "api/invoice/savecreditnotedocumentvoid", json);

                            }
                            catch (InvalidOperationException ex)
                            {

                            }

                        }
                        else
                        {
                            CreditNoteModel creditNoteMpdel = new CreditNoteModel();
                            creditNoteMpdel.Version = "0x" + string.Concat(Array.ConvertAll(inv.Version, x => x.ToString("X2")));
                            UpdateCreditnote(TObject, creditNoteMpdel, inv, isBaseExchangerate);
                            var json = RestSharpHelper.ConvertObjectToJason(creditNoteMpdel);
                            try
                            {
                                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString(); ;
                                RestSharpHelper.Post(url, "api/invoice/savecreditnote", json);
                            }
                            catch (InvalidOperationException ex)
                            {

                            }

                        }
                    }
                    if ((TObject.ExcessPaidByClientAmmount != null || TObject.ExcessPaidByClientAmmount > 0) && (excessPaidAmount == null || excessPaidAmount == 0))
                    {
                        CreditNoteModel creditNoteMpdel = new CreditNoteModel();
                        FillCreditnote(TObject, creditNoteMpdel, isBaseExchangerate, _receipt.DocNo, clearingReceiptCOA);
                        var json = RestSharpHelper.ConvertObjectToJason(creditNoteMpdel);
                        try
                        {
                            string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                            RestSharpHelper.Post(url, "api/invoice/savecreditnote", json);
                        }
                        catch (InvalidOperationException ex)
                        {

                        }
                    }
                }
                else
                {
                    isNewAdd = true;
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Else_Of_the_Receipt_Save_Method);
                    _receipt = new Receipt();
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Else_And_Insert_Receipt_Method);
                    InsertReceipt(TObject, _receipt);
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Come_Out_From_Insert_Receipt_In_Else_Of_SaveReceptMethod);
                    _receipt.DocumentState = AppsWorld.ReceiptModule.Infra.ReceiptState.Posted;
                    _receipt.Id = Guid.NewGuid();
                    _receipt.SystemRefNo = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.Receipt, ConnectionString)/* GenerateAutoNumberForType(company.Id, DocTypeConstants.Receipt, company.ShortName)*/ : TObject.DocNo;
                    _receipt.DocNo = _receipt.SystemRefNo;
                    isDocAdd = true;
                    oldDocdate = _receipt.DocDate;
                    int? servEntCount = 0;
                    long? icId = 0;

                    if (TObject.ReceiptDetailModels != null && TObject.ReceiptDetailModels.Any())
                    {
                        List<long?> lstServeIds = new List<long?>();
                        lstServeIds.AddRange(TObject.ReceiptDetailModels.Select(d => d.ServiceCompanyId));
                        lstServeIds.Add(TObject.ServiceCompanyId);
                        servEntCount = lstServeIds.GroupBy(a => a.Value).Count();
                        if (servEntCount > 1)
                            icId = _chartOfAccountService.GetICAccountId(_receipt.CompanyId, _receipt.ServiceCompanyId);
                        int? recOrder = 0;
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_If_And_Check_ReceiptDetails_Is_Not_Null);
                        //_receipt.ReceiptDetails = TObject.ReceiptDetailModels;

                        List<Invoice> lstInvoice = null;
                        List<DebitNote> lstDebitNote = null;
                        List<CreditMemoCompact> lstCM = null;
                        List<BillCompact> lstBill = null;
                        decimal? sumAllValue = 0;
                        decimal? allGrandTotal = 0;

                        lstInvoice = _invoiceService.GetListOfInvoices(_receipt.CompanyId, TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.Invoice || d.DocumentType == DocTypeConstants.CreditNote).Select(c => c.DocumentId).ToList());
                        lstDebitNote = _debitNoteService.GetListOfDebitNote(_receipt.CompanyId, TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList());

                        if (TObject.IsVendor)
                        {
                            lstBill = _debitNoteService.GetAllBillsByDocId(TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.Bills).Select(c => c.DocumentId).ToList(), _receipt.CompanyId);
                            lstCM = _debitNoteService.GetAllCMByDocId(TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList(), _receipt.CompanyId);
                        }

                        //Checking the documentstate before proceeding to save the receipt details
                        if ((lstInvoice != null && lstInvoice.Exists(a => a.DocumentState == InvoiceStates.Void)) || (lstDebitNote != null && lstDebitNote.Exists(a => a.DocumentState == InvoiceStates.Void)) || (lstBill != null && lstBill.Exists(a => a.DocumentState == InvoiceStates.Void)) || (lstCM != null && lstCM.Exists(a => a.DocumentState == InvoiceStates.Void)))
                            throw new InvalidOperationException(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);

                        #region 2_Tab_Transaction
                        if (lstBill != null && lstBill.Count > 0)
                        {
                            sumAllValue += lstBill.Sum(c => c.BalanceAmount);
                            allGrandTotal += lstBill.Sum(d => d.GrandTotal);
                        }
                        if (lstCM != null && lstCM.Count > 0)
                        {
                            sumAllValue += lstCM.Sum(c => c.BalanceAmount);
                            allGrandTotal += lstCM.Sum(d => d.GrandTotal);
                        }
                        if (lstInvoice != null && lstInvoice.Count > 0)
                        {
                            sumAllValue += lstInvoice.Sum(d => d.BalanceAmount);
                            allGrandTotal += lstInvoice.Sum(d => d.GrandTotal);
                        }
                        if (lstDebitNote != null && lstDebitNote.Count > 0)
                        {
                            sumAllValue += lstDebitNote.Sum(d => d.BalanceAmount);
                            allGrandTotal += lstDebitNote.Sum(d => d.GrandTotal);
                        }
                        if ((sumAllValue != 0 || allGrandTotal != 0) && TObject.ReceiptDetailModels.Any())
                        {
                            if ((sumAllValue != TObject.ReceiptDetailModels.Sum(d => Math.Abs(d.AmmountDue))) || allGrandTotal != TObject.ReceiptDetailModels.Sum(c => Math.Abs(c.DocumentAmmount)))
                                throw new InvalidOperationException(ReceiptConstant.Receipt_Status_Change);
                        }

                        #endregion 2_Tab_Transaction


                        decimal roundingAmount = 0;

                        foreach (ReceiptDetailModel detailModel in TObject.ReceiptDetailModels.Where(d => d.ReceiptAmount != 0))
                        {
                            ReceiptDetail detail = new ReceiptDetail();
                            roundingAmount = 0;
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_Of_ReceiptDetails);
                            detail.Id = Guid.NewGuid();
                            detail.ReceiptId = _receipt.Id;
                            detail.ReceiptAmount = detailModel.DocumentType == DocTypeConstants.Bills || detailModel.DocumentType == DocTypeConstants.CreditNote ? -detailModel.ReceiptAmount : detailModel.ReceiptAmount;
                            detail.AmmountDue = detailModel.DocumentType == DocTypeConstants.Bills || detailModel.DocumentType == DocTypeConstants.CreditNote ? -detailModel.AmmountDue : detailModel.AmmountDue;
                            detail.DocumentAmmount = detailModel.DocumentType == DocTypeConstants.Bills || detailModel.DocumentType == DocTypeConstants.CreditNote ? -detailModel.DocumentAmmount : detailModel.DocumentAmmount;
                            detail.RecOrder = ++recOrder;
                            detail.DocumentNo = detailModel.DocumentNo;
                            detail.SystemReferenceNumber = detailModel.SystemReferenceNumber;
                            detail.DocumentState = detailModel.DocumentState;
                            detail.DocumentType = detailModel.DocumentType;
                            detail.Currency = detailModel.Currency;
                            detail.Nature = detailModel.Nature;
                            detail.DocumentDate = detailModel.DocumentDate;
                            detail.DocumentId = detailModel.DocumentId;
                            detail.ServiceCompanyId = detailModel.ServiceCompanyId;
                            #region Invoice
                            if (detailModel.DocumentType == DocTypeConstants.Invoice)
                            {
                                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_ReceiptDetails_And_Check_DocumentType_Is_Equal_To_Invoice_Or_Not);
                                Invoice invoice = lstInvoice?.FirstOrDefault(c => c.Id == detailModel.DocumentId);

                                if (detailModel.AmmountDue == detailModel.ReceiptAmount)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_ReceiptDetails_And_Check_AmmountDue_Is_Equal_To_Invoice_Or_Not);
                                    if (invoice != null)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_ReceiptDetails_And_Check_invoice_Is_Not_Equal_To_Null_Or_Not);
                                        invoice.BalanceAmount = 0;
                                        invoice.DocumentState = InvoiceState.FullyPaid;
                                        if (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)
                                            roundingAmount = (invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? (decimal)invoice.RoundingAmount : 0;
                                        else
                                            roundingAmount = Math.Round(detailModel.ReceiptAmount * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)invoice.BaseBalanceAmount;
                                        invoice.RoundingAmount = (roundingAmount != 0 && invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? invoice.RoundingAmount - roundingAmount : 0;
                                        detail.RoundingAmount = roundingAmount;
                                        invoice.BaseBalanceAmount = 0;
                                        if (roundingAmount != 0)
                                            lstOfRoundingAmount.Add(invoice.Id, roundingAmount);

                                        invoice.ObjectState = ObjectState.Modified;
                                        invoice.ModifiedDate = DateTime.UtcNow;
                                        invoice.ModifiedBy = ReceiptConstants.System;
                                        _invoiceService.Update(invoice);
                                        if (invoice.IsWorkFlowInvoice == true)
                                            FillWFInvoice(invoice, ConnectionString);

                                        if (invoice.IsOBInvoice == true)
                                        {
                                            con = new SqlConnection(ConnectionString);
                                            if (con.State != ConnectionState.Open)
                                                con.Open();
                                            cmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                                            cmd.CommandTimeout = 30;
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                            cmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                            cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                            cmd.Parameters.AddWithValue("@IsEqual", false);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        else
                                        {
                                            UpdatePosting up = new UpdatePosting();
                                            FillJournalState(up, invoice);
                                            UpdatePosting(up);
                                        }
                                    }
                                }
                                else if (detailModel.AmmountDue != detailModel.ReceiptAmount)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Else_Of_The_ReceiptDetails_And_Check_AmmountDue_Is_Not_Equal_To_ReceiptAmountOr_Not);
                                    if (invoice != null)
                                    {
                                        invoice.BalanceAmount = detailModel.AmmountDue - detailModel.ReceiptAmount;
                                        invoice.BaseBalanceAmount -= Math.Round(detailModel.ReceiptAmount * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                        invoice.DocumentState = InvoiceState.PartialPaid;
                                        invoice.ObjectState = ObjectState.Modified;
                                        invoice.ModifiedDate = DateTime.UtcNow;
                                        invoice.ModifiedBy = ReceiptConstants.System;
                                        _invoiceService.Update(invoice);

                                        if (invoice.IsOBInvoice == true)
                                        {
                                            con = new SqlConnection(ConnectionString);
                                            if (con.State != ConnectionState.Open)
                                                con.Open();
                                            cmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                                            cmd.CommandTimeout = 30;
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                            cmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                            cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                            cmd.Parameters.AddWithValue("@IsEqual", false);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        else
                                        {
                                            UpdatePosting up = new UpdatePosting();
                                            FillJournalState(up, invoice);
                                            UpdatePosting(up);
                                        }

                                        if (invoice.IsWorkFlowInvoice == true)
                                            FillWFInvoice(invoice, ConnectionString);
                                    }
                                }
                                try
                                {
                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_receipt.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate != null ? invoice.ExchangeRate.Value : 1, /*invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated*/ReceiptConstants.System, _receipt.Remarks, _receipt.DocDate, -detailModel.ReceiptAmount, roundingAmount);
                                    if (lstdocumet.Any())
                                        lstdocuments.AddRange(lstdocumet);
                                }
                                catch (InvalidOperationException ex)
                                {
                                }

                            }
                            #endregion Invoice

                            #region DebitNote
                            else if (detailModel.DocumentType == DocTypeConstants.DebitNote)
                            {
                                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Else_Of_The_ReceiptDetails_And_Check_DocumentType_Is_Equal_To_DebitNote_Are_Not);
                                DebitNote debitNote = lstDebitNote?.FirstOrDefault(c => c.Id == detailModel.DocumentId);

                                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_ReceiptDetails_And_Check_ReceiptAmount_Is_Not_Equal_To_Or_Not);
                                if (detailModel.AmmountDue == detailModel.ReceiptAmount)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_else_ReceiptDetails_And_Check_AmmountDue_Is_Equal_To_ReceiptAmount_Or_Not);
                                    if (debitNote != null)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_else_ReceiptDetails_And_Check_invoice_Is_Not_Equal_To_Null_Or_Not);
                                        debitNote.BalanceAmount = 0;
                                        debitNote.DocumentState = InvoiceState.FullyPaid;
                                        if (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)
                                            roundingAmount = ((debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0) ? (decimal)debitNote.RoundingAmount : 0);
                                        else
                                            roundingAmount = Math.Round(detailModel.ReceiptAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)debitNote.BaseBalanceAmount;
                                        debitNote.RoundingAmount = (roundingAmount != 0 && debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0) ? debitNote.RoundingAmount - roundingAmount : 0;
                                        detail.RoundingAmount = roundingAmount;
                                        debitNote.BaseBalanceAmount = 0;
                                        if (roundingAmount != 0)
                                            lstOfRoundingAmount.Add(debitNote.Id, roundingAmount);

                                        debitNote.ModifiedDate = DateTime.UtcNow;
                                        debitNote.ModifiedBy = ReceiptConstants.System;
                                        debitNote.ObjectState = ObjectState.Modified;
                                        _debitNoteService.Update(debitNote);
                                        //Commented by Pradhan
                                        #region Proc_Update_ClearingStatus_For_DebitNote
                                        //SqlConnection con = new SqlConnection(ConnectionString);
                                        //if (con.State != ConnectionState.Open)
                                        //    con.Open();
                                        //SqlCommand cmd = new SqlCommand("PROC_UpdateClearedItem_Bean_Update", con);
                                        //cmd.CommandType = CommandType.StoredProcedure;
                                        //cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                        //cmd.Parameters.AddWithValue("@DocDate", invoice.DocDate);
                                        //cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                        //int updateCount = cmd.ExecuteNonQuery();
                                        //con.Close();
                                        #endregion Update_ClearingStatus_For_DebitNote

                                        UpdatePosting up = new UpdatePosting();
                                        FillJournalState(up, debitNote);
                                        UpdatePosting(up);
                                    }
                                }
                                else if (detailModel.AmmountDue != detailModel.ReceiptAmount)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_else_ReceiptDetails_And_Check_AmmountDue_Is_Not_Equal_To_AmmountDue_Or_Not);
                                    if (debitNote != null)
                                    {
                                        debitNote.BalanceAmount = detailModel.AmmountDue - detailModel.ReceiptAmount;
                                        debitNote.DocumentState = InvoiceState.PartialPaid;
                                        debitNote.BaseBalanceAmount -= Math.Round(detailModel.ReceiptAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                        debitNote.ModifiedDate = DateTime.UtcNow;
                                        debitNote.ModifiedBy = ReceiptConstants.System;
                                        debitNote.ObjectState = ObjectState.Modified;
                                        _debitNoteService.Update(debitNote);
                                        UpdatePosting up = new UpdatePosting();
                                        FillJournalState(up, debitNote);
                                        UpdatePosting(up);
                                    }
                                }

                                try
                                {
                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_receipt.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate != null ? debitNote.ExchangeRate.Value : 1, /*debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated*/ReceiptConstants.System, _receipt.Remarks, _receipt.DocDate, -detailModel.ReceiptAmount, roundingAmount);
                                    if (lstdocumet.Any())
                                        lstdocuments.AddRange(lstdocumet);
                                }
                                catch (InvalidOperationException ex)
                                {
                                }

                            }
                            #endregion DebitNote

                            #region CreditNote
                            //New Changes(Introduced Credit Note)
                            else if (detailModel.DocumentType == DocTypeConstants.CreditNote)
                            {
                                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Else_Of_The_ReceiptDetails_And_Check_DocumentType_Is_Equal_To_DebitNote_Are_Not);
                                Invoice invoice = lstInvoice?.Find(c => c.Id == detailModel.DocumentId);

                                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_ReceiptDetails_And_Check_ReceiptAmount_Is_Not_Equal_To_Or_Not);

                                #region CreditNote_Application_Creation
                                if (detailModel.ReceiptAmount != 0)
                                {
                                    detailModel.Id = detail.Id;
                                    detailModel.DocumentAmmount = detail.DocumentAmmount;
                                    detailModel.AmmountDue = detail.AmmountDue;
                                    detailModel.ReceiptAmount = detail.ReceiptAmount;
                                    CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                    FillCreditNoteAplication(creditNoteModel, invoice, detailModel, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                    creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                    creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                    creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                    CNAplicationREST(creditNoteModel);
                                }
                                #endregion CreditNote_Application_Creation
                            }
                            #endregion CreditNote

                            if (TObject.IsVendor)
                            {
                                #region Bills
                                if (detailModel.DocumentType == DocTypeConstants.Bills)
                                {
                                    detailModel.DocumentAmmount = detail.DocumentAmmount;
                                    detailModel.AmmountDue = detail.AmmountDue;
                                    detailModel.ReceiptAmount = detail.ReceiptAmount;
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Else_Of_The_ReceiptDetails_And_Check_DocumentType_Is_Equal_To_DebitNote_Are_Not);
                                    BillCompact bill = lstBill?.Find(c => c.Id == detailModel.DocumentId);

                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_ReceiptDetails_And_Check_ReceiptAmount_Is_Not_Equal_To_Or_Not);
                                    if (detailModel.AmmountDue == detailModel.ReceiptAmount)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_else_ReceiptDetails_And_Check_AmmountDue_Is_Equal_To_ReceiptAmount_Or_Not);
                                        if (bill != null)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_else_ReceiptDetails_And_Check_invoice_Is_Not_Equal_To_Null_Or_Not);
                                            bill.BalanceAmount = 0;
                                            bill.DocumentState = InvoiceState.FullyPaid;
                                            if (bill.RoundingAmount != null && bill.RoundingAmount != 0)
                                                roundingAmount = ((bill.RoundingAmount != null && bill.RoundingAmount != 0) ? (decimal)bill.RoundingAmount : 0);
                                            else
                                                roundingAmount = Math.Round(detailModel.ReceiptAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)bill.BaseBalanceAmount;
                                            bill.RoundingAmount = (roundingAmount != 0 && bill.RoundingAmount != null && bill.RoundingAmount != 0) ? bill.RoundingAmount - roundingAmount : 0;
                                            detail.RoundingAmount = roundingAmount;
                                            bill.BaseBalanceAmount = 0;
                                            if (roundingAmount != 0)
                                                lstOfRoundingAmount.Add(bill.Id, roundingAmount);

                                            bill.ObjectState = ObjectState.Modified;
                                            bill.ModifiedDate = DateTime.UtcNow;
                                            bill.ModifiedBy = ReceiptConstants.System;
                                            UpdatePosting up = new UpdatePosting();
                                            FillJournalState(up, bill);
                                            UpdatePosting(up);
                                        }
                                    }
                                    else if (detailModel.AmmountDue != detailModel.ReceiptAmount)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_else_ReceiptDetails_And_Check_AmmountDue_Is_Not_Equal_To_AmmountDue_Or_Not);
                                        if (bill != null)
                                        {
                                            bill.BalanceAmount = detailModel.AmmountDue - detailModel.ReceiptAmount;
                                            bill.DocumentState = InvoiceState.PartialPaid;
                                            bill.BaseBalanceAmount -= Math.Round(detailModel.ReceiptAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                            bill.ObjectState = ObjectState.Modified;
                                            bill.ModifiedDate = DateTime.UtcNow;
                                            bill.ModifiedBy = ReceiptConstants.System;
                                            UpdatePosting up = new UpdatePosting();
                                            FillJournalState(up, bill);
                                            UpdatePosting(up);
                                        }
                                    }
                                    try
                                    {
                                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_receipt.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount != null ? bill.BalanceAmount.Value : bill.GrandTotal, bill.ExchangeRate != null ? bill.ExchangeRate.Value : 1, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, _receipt.Remarks, _receipt.DocDate, -detailModel.ReceiptAmount, roundingAmount);

                                        if (lstdocumet.Any())
                                            lstdocuments.AddRange(lstdocumet);
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                    }

                                }
                                #endregion Bills

                                #region CreditMemo
                                if (detailModel.DocumentType == DocTypeConstants.BillCreditMemo)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Else_Of_The_ReceiptDetails_And_Check_DocumentType_Is_Equal_To_DebitNote_Are_Not);
                                    CreditMemoCompact creditMemo = lstCM != null ? lstCM.Where(c => c.Id == detailModel.DocumentId).FirstOrDefault() : null;

                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Foreach_of_ReceiptDetails_And_Check_ReceiptAmount_Is_Not_Equal_To_Or_Not);

                                    #region CreditMemo_Application_Creation

                                    if (detailModel.ReceiptAmount > 0)
                                    {
                                        detailModel.Id = detail.Id;
                                        CreditMemoApplicationModel creditMemoModel = new CreditMemoApplicationModel();
                                        FillCreditMemoAplication(creditMemoModel, creditMemo, detailModel, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                        creditMemoModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                        creditMemoModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                        creditMemoModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                        CMAplicationREST(creditMemoModel);
                                    }

                                    #endregion Application_Creation
                                }
                                #endregion CreditMemo
                            }
                            detail.ObjectState = ObjectState.Added;
                            _receiptDetailService.Insert(detail);
                        }
                    }
                    if (TObject.ReceiptBalancingItems.Count > 0 || TObject.ReceiptBalancingItems != null)
                    {
                        int? recOrder = 0;
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entre_If_Of_ReceiptModel_And_Check_ReceiptBalancingItems_Count_Graterthan_zero_OR_ReceiptBalancingItems_Count_Not_Equal_To_Null);
                        _receipt.ReceiptBalancingItems = TObject.ReceiptBalancingItems;
                        foreach (ReceiptBalancingItem detail in _receipt.ReceiptBalancingItems)
                        {
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entre_Foreach_Of_ReceiptBalancingItem);
                            detail.Id = Guid.NewGuid();
                            detail.ReceiptId = _receipt.Id;
                            detail.RecOrder = recOrder + 1;
                            recOrder = detail.RecOrder;
                            detail.ObjectState = ObjectState.Added;
                        }
                    }

                    if ((TObject.ExcessPaidByClientAmmount != null || TObject.ExcessPaidByClientAmmount > 0))
                    {
                        CreditNoteModel creditNoteMpdel = new CreditNoteModel();
                        TObject.Id = _receipt.Id;
                        FillCreditnote(TObject, creditNoteMpdel, isBaseExchangerate, _receipt.DocNo, clearingReceiptCOA);
                        var json = RestSharpHelper.ConvertObjectToJason(creditNoteMpdel);
                        try
                        {
                            string url = ConfigurationManager.AppSettings["BeanUrl"].ToString(); ;
                            RestSharpHelper.Post(url, "api/invoice/savecreditnote", json);
                        }
                        catch (InvalidOperationException ex)
                        {

                        }

                    }
                    _receipt.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                    _receipt.UserCreated = TObject.UserCreated;
                    _receipt.CreatedDate = DateTime.UtcNow;
                    _receipt.ObjectState = ObjectState.Added;
                    _receiptService.Insert(_receipt);
                }
                try
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Try_Block_OF_The_Receip_Call);
                    _unitOfWorkAsync.SaveChanges();

                    #region Documentary History
                    try
                    {
                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_receipt.Id, _receipt.CompanyId, _receipt.Id, "Receipt", DocTypeConstants.General, _receipt.DocumentState, _receipt.DocCurrency, _receipt.GrandTotal, 0, _receipt.ExchangeRate != null ? _receipt.ExchangeRate.Value : _receipt.SystemCalculatedExchangeRate != null ? _receipt.SystemCalculatedExchangeRate.Value : 1, _receipt.ModifiedBy != null ? _receipt.ModifiedBy : _receipt.UserCreated, _receipt.Remarks, _receipt.DocDate, _receipt.GrandTotal, 0);
                        if (lstdocumet.Any())
                            lstdocuments.AddRange(lstdocumet);
                        if (lstdocuments.Any())
                        {
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entering_Into_Saving_DocumentHistory_Block);
                            AppaWorld.Bean.Common.SaveDocumentHistory(lstdocuments, ConnectionString);
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Sucessfully_inserted_the_documents_in_DocumentHistory);
                        }

                        if (oldDocdate != TObject.DocDate)
                        {

                            query = $"Update Bean.DocumentHistory Set PostingDate='{String.Format("{0:MM/dd/yyyy}", TObject.DocDate)}' where TransactionId='{_receipt.Id}' and CompanyId={_receipt.CompanyId} and TransactionId<>DocumentId and AgingState is null;";
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Sucessfully_Updated_the_documents_in_DocumentHistory_if_DocDate_is_changed_in_EditMode);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Issues_While_inserting_the_record_in_document_history);
                    }
                    #endregion Documentary History
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_RecieptPosting_Sp", con);
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SourceId", _receipt.Id);
                        cmd.Parameters.AddWithValue("@Type", DocTypeConstants.Receipt);
                        cmd.Parameters.AddWithValue("@CompanyId", _receipt.CompanyId);
                        cmd.Parameters.AddWithValue("@RoundingAmount", string.Join(":", lstOfRoundingAmount));
                        cmd.ExecuteNonQuery();
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Catch_Block_Of_SaveReceipt_Of_Save_Call);
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw new InvalidOperationException("An error has occurred!Please try after sometimes.");
                }
                return _receipt;
            }
            catch (InvalidOperationException ex)
            {
                if (isNewAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, DocTypeConstants.Receipt, ConnectionString);
                }
                throw ex;
            }
        }



        public void FillCreditnote(ReceiptModel TObject, CreditNoteModel creditNoteMpdel, bool? isBaseExchagerate, string docNo, long clearingReceiptCOA)
        {
            creditNoteMpdel.CompanyId = TObject.CompanyId;
            creditNoteMpdel.Id = Guid.NewGuid();
            creditNoteMpdel.BalanceAmount = TObject.ExcessPaidByClientAmmount;
            creditNoteMpdel.BaseCurrency = TObject.BaseCurrency;
            creditNoteMpdel.CreatedDate = TObject.CreatedDate;
            creditNoteMpdel.CreditTermsId = _termsOfPaymentService.GetCreditTermById(TObject.CompanyId);
            creditNoteMpdel.DocumentId = TObject.Id;
            creditNoteMpdel.CustCreditlimit = TObject.CustCreditlimit;
            creditNoteMpdel.DocCurrency = TObject.ExcessPaidByClientCurrency;
            creditNoteMpdel.DocDate = TObject.DocDate;
            creditNoteMpdel.DocNo = docNo;
            creditNoteMpdel.DocType = DocTypeConstants.CreditNote;
            creditNoteMpdel.DocSubType = "Excess";
            creditNoteMpdel.DocumentState = "Not Applied";
            creditNoteMpdel.EntityId = TObject.EntityId;
            creditNoteMpdel.ExchangeRate = TObject.BaseCurrency == TObject.BankReceiptAmmountCurrency ? 1 : TObject.SystemCalculatedExchangeRate != null ? TObject.SystemCalculatedExchangeRate : TObject.ExchangeRate;
            creditNoteMpdel.ExDurationFrom = TObject.ExDurationFrom;
            creditNoteMpdel.ExtensionType = "Receipt";
            creditNoteMpdel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
            creditNoteMpdel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
            creditNoteMpdel.GrandTotal = TObject.ExcessPaidByClientAmmount;
            creditNoteMpdel.GSTExchangeRate = TObject.GstExchangeRate;
            creditNoteMpdel.GSTExCurrency = TObject.GstReportingCurrency;
            creditNoteMpdel.IsBaseCurrencyRateChanged = isBaseExchagerate;
            creditNoteMpdel.IsDocNoEditable = false;
            creditNoteMpdel.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
            creditNoteMpdel.IsGstSettings = TObject.IsGstSettings;
            creditNoteMpdel.IsMultiCurrency = TObject.ISMultiCurrency;
            creditNoteMpdel.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
            creditNoteMpdel.ModifiedBy = TObject.ModifiedBy;
            creditNoteMpdel.ModifiedDate = TObject.ModifiedDate;
            creditNoteMpdel.Nature = TObject.ExcessPaidByClient.StartsWith("(TR)") ? "Trade" : "Others"; ;
            creditNoteMpdel.NoSupportingDocument = TObject.NoSupportingDocument;
            creditNoteMpdel.PeriodLockPassword = TObject.PeriodLockPassword;
            creditNoteMpdel.SaveType = null;
            creditNoteMpdel.ServiceCompanyId = TObject.ServiceCompanyId;
            creditNoteMpdel.Status = TObject.Status;
            creditNoteMpdel.UserCreated = "System";
            creditNoteMpdel.CreatedDate = DateTime.UtcNow;
            creditNoteMpdel.Remarks = "Excess Paid by Client - " + docNo;
            InvoiceDetailModel lstinvoicedetail = new InvoiceDetailModel();
            List<InvoiceDetailModel> InvoiceDetails = new List<InvoiceDetailModel>();
            lstinvoicedetail.Id = Guid.NewGuid();
            lstinvoicedetail.InvoiceId = creditNoteMpdel.Id;
            lstinvoicedetail.ItemDescription = "Excess Paid by Client - " + docNo;
            lstinvoicedetail.AmtCurrency = creditNoteMpdel.BaseCurrency;
            lstinvoicedetail.COAId = clearingReceiptCOA;
            lstinvoicedetail.TaxIdCode = ReceiptConstant.NA;
            lstinvoicedetail.TaxId = _taxCodeService.GetTaxCodeByName(ReceiptConstant.NA, TObject.CompanyId);
            lstinvoicedetail.DocTotalAmount = TObject.ExcessPaidByClientAmmount;
            lstinvoicedetail.DocAmount = TObject.ExcessPaidByClientAmmount;
            lstinvoicedetail.BaseAmount = TObject.ExcessPaidByClientAmmount * creditNoteMpdel.ExchangeRate;
            lstinvoicedetail.BaseTotalAmount = lstinvoicedetail.BaseAmount;
            lstinvoicedetail.Qty = 1;
            InvoiceDetails.Add(lstinvoicedetail);
            creditNoteMpdel.InvoiceDetails = InvoiceDetails;
        }
        public void UpdateCreditnote(ReceiptModel TObject, CreditNoteModel creditNoteMpdel, Invoice inv, bool? isBaseExchangeRate)
        {
            creditNoteMpdel.CompanyId = TObject.CompanyId;
            creditNoteMpdel.Id = inv.Id;
            creditNoteMpdel.BalanceAmount = TObject.ExcessPaidByClientAmmount;
            creditNoteMpdel.BaseCurrency = TObject.BaseCurrency;
            creditNoteMpdel.CreatedDate = TObject.CreatedDate;
            creditNoteMpdel.CreditTermsId = _termsOfPaymentService.GetCreditTermById(TObject.CompanyId);
            creditNoteMpdel.DocumentId = TObject.Id;
            creditNoteMpdel.CustCreditlimit = TObject.CustCreditlimit;
            creditNoteMpdel.DocCurrency = TObject.ExcessPaidByClientCurrency;
            creditNoteMpdel.DocDate = TObject.DocDate;
            creditNoteMpdel.DocNo = inv.DocNo;
            creditNoteMpdel.DocType = DocTypeConstants.CreditNote;
            creditNoteMpdel.DocSubType = "Excess";
            creditNoteMpdel.DocumentState = "Not Applied";
            creditNoteMpdel.EntityId = TObject.EntityId;
            creditNoteMpdel.ExchangeRate = TObject.ExchangeRate;
            creditNoteMpdel.ExDurationFrom = TObject.ExDurationFrom;
            creditNoteMpdel.ExtensionType = "Receipt";
            creditNoteMpdel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
            creditNoteMpdel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
            creditNoteMpdel.GrandTotal = TObject.ExcessPaidByClientAmmount;
            creditNoteMpdel.GSTExchangeRate = TObject.GstExchangeRate;
            creditNoteMpdel.IsBaseCurrencyRateChanged = isBaseExchangeRate;
            creditNoteMpdel.IsDocNoEditable = TObject.IsDocNoEditable;
            creditNoteMpdel.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
            creditNoteMpdel.IsGstSettings = TObject.IsGstSettings;
            creditNoteMpdel.IsMultiCurrency = TObject.ISMultiCurrency;
            creditNoteMpdel.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
            creditNoteMpdel.ModifiedBy = "System";
            creditNoteMpdel.ModifiedDate = DateTime.UtcNow;
            creditNoteMpdel.Nature = TObject.ExcessPaidByClient.StartsWith("(TR)") ? "Trade" : "Others"; ;
            creditNoteMpdel.NoSupportingDocument = TObject.NoSupportingDocument;
            creditNoteMpdel.PeriodLockPassword = TObject.PeriodLockPassword;
            creditNoteMpdel.Remarks = TObject.Remarks;
            creditNoteMpdel.SaveType = TObject.SaveType;
            creditNoteMpdel.ServiceCompanyId = TObject.ServiceCompanyId;
            creditNoteMpdel.Status = TObject.Status;
            creditNoteMpdel.UserCreated = TObject.UserCreated;
            creditNoteMpdel.Remarks = "Excess Paid by Client - " + TObject.DocNo;

            InvoiceDetailModel lstinvoicedetail = new InvoiceDetailModel();
            List<InvoiceDetailModel> InvoiceDetails = new List<InvoiceDetailModel>();
            AppsWorld.ReceiptModule.Entities.FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(TObject.CompanyId);

            creditNoteMpdel.ExchangeRate = TObject.BaseCurrency == TObject.BankReceiptAmmountCurrency ? 1 : TObject.SystemCalculatedExchangeRate != null ? TObject.SystemCalculatedExchangeRate : TObject.ExchangeRate;
            lstinvoicedetail.Id = _invoiceService.GetInvoiceByDetailid(inv.Id) == null ? Guid.Empty : _invoiceService.GetInvoiceByDetailid(inv.Id);
            lstinvoicedetail.InvoiceId = creditNoteMpdel.Id;
            lstinvoicedetail.ItemDescription = "Excess Paid by Client - " + TObject.DocNo;
            lstinvoicedetail.AmtCurrency = creditNoteMpdel.BaseCurrency;
            lstinvoicedetail.COAId = _chartOfAccountService.GetByNameId(ReceiptConstant.Clearing_Receipts, TObject.CompanyId);
            lstinvoicedetail.TaxIdCode = ReceiptConstant.NA;
            lstinvoicedetail.TaxId = _taxCodeService.GetTaxCodeByName(ReceiptConstant.NA, TObject.CompanyId);
            lstinvoicedetail.DocTotalAmount = TObject.ExcessPaidByClientAmmount;
            lstinvoicedetail.DocAmount = TObject.ExcessPaidByClientAmmount;
            lstinvoicedetail.BaseAmount = TObject.ExcessPaidByClientAmmount * creditNoteMpdel.ExchangeRate;
            lstinvoicedetail.BaseTotalAmount = lstinvoicedetail.BaseAmount;
            InvoiceDetails.Add(lstinvoicedetail);
            creditNoteMpdel.InvoiceDetails = InvoiceDetails;
        }

        public Receipt SaveCreditNoteDocumentVoid(DocumentVoidModel TObject, string ConnectionString)
        {
            Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_SaveCreditNoteDocumentVoid_Method);
            string DocNo = "-V";
            decimal roundingAmount = 0;
            Receipt _document = _receiptService.GetReceipt(TObject.Id, TObject.CompanyId);
            if (_document == null)
                throw new InvalidOperationException(ReceiptConstant.Invalid_Receipt);
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);
            }
            //if the document state is void
            if (_receiptService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

            //if (_document.DocumentState != CreditNoteState.NotApplied)
            //	throw new Exception("State should be " + CreditNoteState.NotApplied);

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
            {
                throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            Invoice creditNote = null;
            if (_document.ExcessPaidByClientAmmount != null)
            {
                creditNote = _invoiceService.GetInvoiceByDocumentByState(TObject.Id);

                if (creditNote != null)
                {
                    if (creditNote.DocumentState != "Not Applied")
                    {
                        throw new InvalidOperationException("You can't void the record");
                    }
                    else
                    {
                        DocumentVoidModel documentVoidModel = new DocumentVoidModel();
                        documentVoidModel.Id = creditNote.Id;
                        documentVoidModel.CompanyId = TObject.CompanyId;
                        documentVoidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                        documentVoidModel.Version = "0x" + string.Concat(Array.ConvertAll(creditNote.Version, x => x.ToString("X2")));
                        var json = RestSharpHelper.ConvertObjectToJason(documentVoidModel);
                        try
                        {
                            string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                            //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                            //if (section.Ziraff.Count > 0)
                            //{
                            //    for (int i = 0; i < section.Ziraff.Count; i++)
                            //    {
                            //        if (section.Ziraff[i].Name == ReceiptConstants.IdentityBean)
                            //        {
                            //            url = section.Ziraff[i].ServerUrl;
                            //            break;
                            //        }
                            //    }
                            //}
                            object obj = documentVoidModel;
                            // string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                            var response = RestSharpHelper.Post(url, "api/invoice/savecreditnotedocumentvoid", json);
                            if (response.ErrorMessage != null)
                            {
                                //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                            }
                        }
                        catch (Exception ex)
                        {
                            //Log.Logger.ZCritical(ex.StackTrace);

                            var message = ex.Message;
                        }
                    }

                }
            }
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
            {
                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Not_Null_Of_DocDate_And_CompanyId);
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
            }
            _document.DocumentState = AppsWorld.ReceiptModule.Infra.ReceiptState.Void;
            _document.DocNo += DocNo;
            _document.ModifiedBy = TObject.ModifiedBy;
            _document.ModifiedDate = DateTime.UtcNow;
            _document.ObjectState = ObjectState.Modified;
            List<Invoice> lstInv = null;
            List<DebitNote> lstDN = null;
            List<BillCompact> lstBills = null;
            List<CreditMemoCompact> lstCM = null;
            List<DocumentHistoryModel> lstdocuments = new List<DocumentHistoryModel>();
            if (_document.ReceiptDetails.Any())
            {
                lstInv = _invoiceService.GetListOfInvoices(_document.CompanyId, _document.ReceiptDetails.Where(c => (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.CreditNote) && c.ReceiptAmount != 0).Select(c => c.DocumentId).ToList());
                lstDN = _debitNoteService.GetListOfDebitNote(_document.CompanyId, _document.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.DebitNote && c.ReceiptAmount > 0).Select(c => c.DocumentId).ToList());
            }
            if (_document.IsVendor == true)
            {
                lstBills = _debitNoteService.GetAllBillsByDocId(_document.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.Bills && c.ReceiptAmount != 0).Select(c => c.DocumentId).ToList(), _document.CompanyId);
                lstCM = _debitNoteService.GetAllCMByDocId(_document.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.BillCreditMemo && c.ReceiptAmount > 0).Select(c => c.DocumentId).ToList(), _document.CompanyId);
            }
            if (_document.ReceiptDetails != null && _document.ReceiptDetails.Count > 0)
            {
                foreach (ReceiptDetail detail in _document.ReceiptDetails.Where(c => c.ReceiptAmount != 0).ToList())
                {
                    roundingAmount = 0;
                    if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
                    {
                        Invoice invoice = lstInv?.FirstOrDefault(c => c.Id == detail.DocumentId);
                        if (invoice != null && invoice.DocType == DocTypeConstants.Invoice)
                        {
                            invoice.BalanceAmount += detail.ReceiptAmount;
                            if (invoice.BalanceAmount == invoice.GrandTotal)
                            {
                                invoice.DocumentState = InvoiceState.NotPaid;
                                invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                                invoice.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                invoice.ModifiedDate = DateTime.UtcNow;
                                invoice.ModifiedBy = "System";
                                invoice.ObjectState = ObjectState.Modified;

                                if (invoice.IsWorkFlowInvoice == true)
                                    //FillWokflowInvoice(invoice);
                                    FillWFInvoice(invoice, ConnectionString);

                            }
                            else if (invoice.BalanceAmount != invoice.GrandTotal)
                            {
                                invoice.DocumentState = InvoiceState.PartialPaid;
                                invoice.BaseBalanceAmount += (Math.Round((detail.ReceiptAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                invoice.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;

                                invoice.ModifiedDate = DateTime.UtcNow;
                                invoice.ModifiedBy = "System";
                                invoice.ObjectState = ObjectState.Modified;
                                if (invoice.IsWorkFlowInvoice == true)
                                    //FillWokflowInvoice(invoice);
                                    FillWFInvoice(invoice, ConnectionString);
                            }

                            if (invoice.IsOBInvoice == true)
                            {
                                SqlConnection conn = new SqlConnection(ConnectionString);
                                if (conn.State != ConnectionState.Open)
                                    conn.Open();
                                SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                                oBcmd.CommandTimeout = 30;
                                oBcmd.CommandType = CommandType.StoredProcedure;
                                oBcmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                oBcmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                oBcmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                oBcmd.Parameters.AddWithValue("@IsEqual", invoice.BalanceAmount == invoice.GrandTotal && (invoice.AllocatedAmount == 0 || invoice.AllocatedAmount == null));
                                oBcmd.ExecuteNonQuery();
                                conn.Close();

                            }
                            if (invoice.IsOBInvoice != true)
                            {
                                #region Update_Journal_Detail_Clearing_Status
                                SqlConnection con = new SqlConnection(ConnectionString);
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                                cmd.CommandTimeout = 30;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@companyId", invoice.CompanyId);
                                cmd.Parameters.AddWithValue("@documentId", invoice.Id);
                                cmd.Parameters.AddWithValue("@docState", invoice.DocumentState);
                                cmd.Parameters.AddWithValue("@balanceAmount", invoice.BalanceAmount);
                                cmd.ExecuteNonQuery();
                                con.Close();
                                #endregion Update_Journal_Detail_Clearing_Status
                            }
                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, null, 0, 0);
                                if (lstdocumet.Any())
                                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                                    lstdocuments.AddRange(lstdocumet);
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion Documentary History
                        }
                        else
                        {
                            CreditNoteApplicationCompact cnApplication = _receiptDetailService.GetCNApplicationByDocId(detail.Id);
                            if (cnApplication != null)
                            {
                                DocumentResetModel voidModel = new DocumentResetModel();
                                voidModel.Id = cnApplication.Id;
                                voidModel.InvoiceId = cnApplication.InvoiceId;
                                voidModel.ResetDate = DateTime.UtcNow;
                                voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2")));
                                CNAplicationVoidREST(voidModel);
                            }
                        }
                    }
                    else if (detail.DocumentType == DocTypeConstants.DebitNote)
                    {
                        DebitNote debitNote = lstDN?.FirstOrDefault(d => d.Id == detail.DocumentId);
                        if (debitNote != null)
                        {
                            debitNote.BalanceAmount += detail.ReceiptAmount;
                            if (debitNote.BalanceAmount == debitNote.GrandTotal)
                            {
                                debitNote.DocumentState = InvoiceState.NotPaid;
                                debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                                debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                debitNote.ModifiedDate = DateTime.UtcNow;
                                debitNote.ModifiedBy = "System";
                                debitNote.ObjectState = ObjectState.Modified;
                            }
                            else if (debitNote.BalanceAmount != debitNote.GrandTotal)
                            {
                                debitNote.DocumentState = InvoiceState.PartialPaid;
                                debitNote.BaseBalanceAmount += (Math.Round((detail.ReceiptAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;

                                debitNote.ModifiedDate = DateTime.UtcNow;
                                debitNote.ModifiedBy = "System";
                                debitNote.ObjectState = ObjectState.Modified;
                            }
                        }

                        #region Update_Journal_Detail_Clearing_Status
                        SqlConnection con = new SqlConnection(ConnectionString);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@companyId", debitNote.CompanyId);
                        cmd.Parameters.AddWithValue("@documentId", debitNote.Id);
                        cmd.Parameters.AddWithValue("@docState", debitNote.DocumentState);
                        cmd.Parameters.AddWithValue("@balanceAmount", debitNote.BalanceAmount);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        #endregion Update_Journal_Detail_Clearing_Status

                        #region Documentary History
                        try
                        {
                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, null, 0, 0);
                            if (lstdocumet.Any())
                                lstdocuments.AddRange(lstdocumet);
                        }
                        catch (Exception ex)
                        {
                            //throw ex;
                        }
                        #endregion Documentary History

                    }
                    if (_document.IsVendor == true)
                    {
                        if (detail.DocumentType == DocTypeConstants.Bills)
                        {
                            BillCompact bills = lstBills?.FirstOrDefault(d => d.Id == detail.DocumentId);
                            if (bills != null)
                            {
                                bills.BalanceAmount += detail.ReceiptAmount;
                                if (bills.BalanceAmount == bills.GrandTotal)
                                {
                                    bills.DocumentState = InvoiceState.NotPaid;
                                    bills.BaseBalanceAmount = bills.BaseGrandTotal;
                                    bills.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                    bills.ModifiedDate = DateTime.UtcNow;
                                    bills.ModifiedBy = "System";
                                    bills.ObjectState = ObjectState.Modified;
                                }
                                else if (bills.BalanceAmount != bills.GrandTotal)
                                {
                                    bills.DocumentState = InvoiceState.PartialPaid;
                                    roundingAmount = (Math.Round((bills.GrandTotal - detail.ReceiptAmount) * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.ReceiptAmount * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)bills.BaseGrandTotal;
                                    bills.BaseBalanceAmount += (Math.Round((detail.ReceiptAmount * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - roundingAmount);
                                    bills.ModifiedDate = DateTime.UtcNow;
                                    bills.ModifiedBy = "System";
                                    bills.ObjectState = ObjectState.Modified;
                                }
                            }
                            if (bills.IsExternal == true && bills.DocSubType == DocTypeConstants.OpeningBalance)
                            {
                                SqlConnection conn = new SqlConnection(ConnectionString);
                                if (conn.State != ConnectionState.Open)
                                    conn.Open();
                                SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                                oBcmd.CommandTimeout = 30;
                                oBcmd.CommandType = CommandType.StoredProcedure;
                                oBcmd.Parameters.AddWithValue("@OBId", bills.OpeningBalanceId);
                                oBcmd.Parameters.AddWithValue("@DocumentId", bills.Id);
                                oBcmd.Parameters.AddWithValue("@CompanyId", bills.CompanyId);
                                oBcmd.Parameters.AddWithValue("@IsEqual", bills.BalanceAmount == bills.GrandTotal ? true : false);
                                oBcmd.ExecuteNonQuery();
                                conn.Close();

                            }
                            #region Update_Journal_Detail_Clearing_Status
                            SqlConnection con = new SqlConnection(ConnectionString);
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                            cmd.CommandTimeout = 30;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@companyId", bills.CompanyId);
                            cmd.Parameters.AddWithValue("@documentId", bills.Id);
                            cmd.Parameters.AddWithValue("@docState", bills.DocumentState);
                            cmd.Parameters.AddWithValue("@balanceAmount", bills.BalanceAmount);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            #endregion Update_Journal_Detail_Clearing_Status
                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, bills.CompanyId, bills.Id, bills.DocType, bills.DocSubType, bills.DocumentState, bills.DocCurrency, bills.GrandTotal, bills.BalanceAmount.Value != null ? bills.BalanceAmount.Value : 0, bills.ExchangeRate.Value, bills.ModifiedBy != null ? bills.ModifiedBy : bills.UserCreated, bills.DocDescription, null, 0, 0);

                                if (lstdocumet.Any())
                                    lstdocuments.AddRange(lstdocumet);
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion Documentary History
                        }
                        if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                        {
                            CreditMemoApplicationCompact cmApplication = _receiptDetailService.GetCMApplicationByDocId(detail.Id);
                            if (cmApplication != null)
                            {
                                DocumentResetModel voidModel = new DocumentResetModel();
                                voidModel.Id = cmApplication.Id;
                                voidModel.CreditMemoId = cmApplication.CreditMemoId;
                                voidModel.ResetDate = DateTime.UtcNow;
                                voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));
                                CMAplicationVoidREST(voidModel);
                            }

                            #region Update_Journal_Detail_Clearing_Status
                            //SqlConnection con = new SqlConnection(ConnectionString);
                            //if (con.State != ConnectionState.Open)
                            //    con.Open();
                            //SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@companyId", creditMemo.CompanyId);
                            //cmd.Parameters.AddWithValue("@documentId", creditMemo.Id);
                            //cmd.Parameters.AddWithValue("@docState", creditMemo.DocumentState);
                            //cmd.Parameters.AddWithValue("@balanceAmount", creditMemo.BalanceAmount);
                            //int count = cmd.ExecuteNonQuery();
                            //con.Close();
                            #endregion Update_Journal_Detail_Clearing_Status

                        }

                    }
                }
            }
            try
            {
                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Try_Of_The_SaveCreditNoteDocumentVoid_Block);
                _unitOfWorkAsync.SaveChanges();

                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, _document.CompanyId, _document.Id, "Receipt", DocTypeConstants.General, _document.DocumentState, _document.DocCurrency, _document.GrandTotal, _document.GrandTotal, _document.ExchangeRate != null ? _document.ExchangeRate.Value : _document.SystemCalculatedExchangeRate != null ? _document.SystemCalculatedExchangeRate.Value : 1, _document.ModifiedBy != null ? _document.ModifiedBy : _document.UserCreated, _document.Remarks, null, 0, 0);
                    if (lstdocumet.Any())
                        lstdocuments.AddRange(lstdocumet);
                    if (lstdocuments.Any())
                        AppaWorld.Bean.Common.SaveDocumentHistory(lstdocuments, ConnectionString);
                }
                catch (Exception ex)
                {

                }
                #endregion Documentary History

                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = _document.DocNo;
                tObject.ModifiedBy = TObject.ModifiedBy;
                deleteJVPostInvoce(tObject);

                #region Cust_CreditLimit_Updation
                //decimal? custCreditLimit = _beanEntityService.GetEntityCreditTermsValue(_document.EntityId);
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
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _document;
        }

        public Receipt SaveCreditNoteDocumentVoidNew(DocumentVoidModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            try
            {
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptApplicationService, ReceiptLoggingValidation.Log_Receipts_SaveCreditNoteDocumentVoidNew_Request_Message);
                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_SaveCreditNoteDocumentVoid_Method);
                IQueryable<Invoice> lstInv = null;
                IQueryable<DebitNote> lstDN = null;
                IQueryable<BillCompact> lstBills = null;
                IQueryable<CreditMemoCompact> lstCM = null;
                List<DocumentHistoryModel> lstdocuments = new List<DocumentHistoryModel>();
                string DocNo = "-V";
                decimal roundingAmount = 0;
                #region DBHits
                Receipt _document = _receiptService.GetReceipt(TObject.Id, TObject.CompanyId);
                bool isVoidAreNot = _receiptService.IsVoidNew(TObject.CompanyId, TObject.Id);
                bool validateYearEndLockDate = _financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId);
                bool validatefinOpenPeriod = _financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId);
                if (_document.ReceiptDetails.Any())
                {
                    lstInv = _invoiceService.GetListOfInvoicesNew(_document.CompanyId, _document.ReceiptDetails.Where(c => (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.CreditNote) && c.ReceiptAmount != 0).Select(c => c.DocumentId).ToList());
                    lstDN = _debitNoteService.GetListOfDebitNoteNew(_document.CompanyId, _document.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.DebitNote && c.ReceiptAmount > 0).Select(c => c.DocumentId).ToList());
                }
                if (_document.IsVendor == true)
                {
                    lstBills = _debitNoteService.GetAllBillsByDocIdNew(_document.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.Bills && c.ReceiptAmount != 0).Select(c => c.DocumentId).ToList(), _document.CompanyId);
                    lstCM = _debitNoteService.GetAllCMByDocIdNew(_document.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.BillCreditMemo && c.ReceiptAmount > 0).Select(c => c.DocumentId).ToList(), _document.CompanyId);
                }
                #endregion
                #region Validation Checking
                if (_document == null)
                    throw new InvalidOperationException(ReceiptConstant.Invalid_Receipt);
                else
                {
                    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                    if (!timeStamp.Equals(TObject.Version))
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);
                }
                //if the document state is void
                if (isVoidAreNot)
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);

                //Need to verify the invoice is within Financial year
                if (!validateYearEndLockDate)
                {
                    throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }
                if (!validatefinOpenPeriod)
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Not_Null_Of_DocDate_And_CompanyId);
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new InvalidOperationException(CommonConstant.Invalid_Financial_Period_Lock_Password);
                    }
                }
                #endregion
                #region ExcessPaidByClientAmmountisNotNull
                if (_document.ExcessPaidByClientAmmount != null)
                {
                    Invoice creditNote = _invoiceService.GetInvoiceByDocumentByState(TObject.Id);

                    if (creditNote != null)
                    {
                        if (creditNote.DocumentState != "Not Applied")
                        {
                            throw new InvalidOperationException("You can't void the record");
                        }
                        else
                        {
                            DocumentVoidModel documentVoidModel = new DocumentVoidModel();
                            documentVoidModel.Id = creditNote.Id;
                            documentVoidModel.CompanyId = TObject.CompanyId;
                            documentVoidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                            documentVoidModel.Version = "0x" + string.Concat(Array.ConvertAll(creditNote.Version, x => x.ToString("X2")));
                            var json = RestSharpHelper.ConvertObjectToJason(documentVoidModel);
                            try
                            {
                                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                                object obj = documentVoidModel;
                                var response = RestSharpHelper.Post(url, "api/invoice/savecreditnotedocumentvoid", json);
                                if (response.ErrorMessage != null)
                                {
                                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                                }
                            }
                            catch (Exception ex)
                            {
                                //Log.Logger.ZCritical(ex.StackTrace);

                                var message = ex.Message;
                            }
                        }
                    }
                }
                #endregion
                _document.DocumentState = AppsWorld.ReceiptModule.Infra.ReceiptState.Void;
                _document.DocNo += DocNo;
                _document.ModifiedBy = TObject.ModifiedBy;
                _document.ModifiedDate = DateTime.UtcNow;
                _document.ObjectState = ObjectState.Modified;
                if (_document.ReceiptDetails != null && _document.ReceiptDetails.Count > 0)
                {
                    _document.ReceiptDetails
                        .Where(c => c.ReceiptAmount != 0)
                        .ToList()
                        .ForEach(detail =>
                        {
                            roundingAmount = 0;
                            switch (detail.DocumentType)
                            {
                                case DocTypeConstants.Invoice:
                                case DocTypeConstants.CreditNote:
                                    ProcessInvoiceOrCreditNote(lstInv, detail, TObject, lstdocuments, ConnectionString);
                                    break;

                                case DocTypeConstants.DebitNote:
                                    ProcessDebitNote(lstDN, detail, lstdocuments, TObject, ConnectionString);
                                    break;

                                case DocTypeConstants.Bill:
                                    if (_document.IsVendor == true)
                                    {
                                        ProcessBillNote(lstBills, detail, roundingAmount, TObject, lstdocuments, ConnectionString);
                                    }
                                    break;

                                case DocTypeConstants.BillCreditMemo:
                                    if (_document.IsVendor == true)
                                    {
                                        ProcessBillCreditMemoNote(detail, TObject);
                                    }
                                    break;
                            }
                        });
                }

                try
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Try_Of_The_SaveCreditNoteDocumentVoid_Block);
                    _unitOfWorkAsync.SaveChanges();

                    #region Documentary History
                    try
                    {
                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, _document.CompanyId, _document.Id, "Receipt", DocTypeConstants.General, _document.DocumentState, _document.DocCurrency, _document.GrandTotal, _document.GrandTotal, _document.ExchangeRate != null ? _document.ExchangeRate.Value : _document.SystemCalculatedExchangeRate != null ? _document.SystemCalculatedExchangeRate.Value : 1, _document.ModifiedBy != null ? _document.ModifiedBy : _document.UserCreated, _document.Remarks, null, 0, 0);
                        if (lstdocumet.Any())
                            lstdocuments.AddRange(lstdocumet);
                        if (lstdocuments.Any())
                            AppaWorld.Bean.Common.SaveDocumentHistory(lstdocuments, ConnectionString);
                    }
                    catch (Exception ex)
                    {

                    }
                    #endregion Documentary History

                    JournalSaveModel tObject = new JournalSaveModel();
                    tObject.Id = TObject.Id;
                    tObject.CompanyId = TObject.CompanyId;
                    tObject.DocNo = _document.DocNo;
                    tObject.ModifiedBy = TObject.ModifiedBy;
                    deleteJVPostInvoce(tObject);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptApplicationService, ReceiptLoggingValidation.Log_Receipts_SaveCreditNoteDocumentVoidNew_Request_Message_Completed);
                return _document;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ReceiptLoggingValidation.ReceiptApplicationService, ex, ex.Message, AdditionalInfo);
                throw ex;
            }

        }
        #endregion
        #region Switchcase fill methods in recepitsdetails
        private void ProcessBillCreditMemoNote(ReceiptDetail detail, DocumentVoidModel TObject)
        {
            CreditMemoApplicationCompact cmApplication = _receiptDetailService.GetCMApplicationByDocId(detail.Id);
            if (cmApplication != null)
            {
                DocumentResetModel voidModel = new DocumentResetModel();
                voidModel.Id = cmApplication.Id;
                voidModel.CreditMemoId = cmApplication.CreditMemoId;
                voidModel.ResetDate = DateTime.UtcNow;
                voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));
                CMAplicationVoidREST(voidModel);
            }


        }
        private void ProcessBillNote(IQueryable<BillCompact> lstBills, ReceiptDetail detail, decimal roundingAmount, DocumentVoidModel TObject, List<DocumentHistoryModel> lstdocuments, string ConnectionString)
        {
            BillCompact bills = lstBills?.FirstOrDefault(d => d.Id == detail.DocumentId);
            if (bills != null)
            {
                bills.BalanceAmount += detail.ReceiptAmount;
                if (bills.BalanceAmount == bills.GrandTotal)
                {
                    bills.DocumentState = InvoiceState.NotPaid;
                    bills.BaseBalanceAmount = bills.BaseGrandTotal;
                    bills.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                    bills.ModifiedDate = DateTime.UtcNow;
                    bills.ModifiedBy = "System";
                    bills.ObjectState = ObjectState.Modified;
                }
                else if (bills.BalanceAmount != bills.GrandTotal)
                {
                    bills.DocumentState = InvoiceState.PartialPaid;
                    roundingAmount = (Math.Round((bills.GrandTotal - detail.ReceiptAmount) * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.ReceiptAmount * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)bills.BaseGrandTotal;
                    bills.BaseBalanceAmount += (Math.Round((detail.ReceiptAmount * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - roundingAmount);
                    bills.ModifiedDate = DateTime.UtcNow;
                    bills.ModifiedBy = "System";
                    bills.ObjectState = ObjectState.Modified;
                }
            }
            if (bills.IsExternal == true && bills.DocSubType == DocTypeConstants.OpeningBalance)
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                oBcmd.CommandTimeout = 30;
                oBcmd.CommandType = CommandType.StoredProcedure;
                oBcmd.Parameters.AddWithValue("@OBId", bills.OpeningBalanceId);
                oBcmd.Parameters.AddWithValue("@DocumentId", bills.Id);
                oBcmd.Parameters.AddWithValue("@CompanyId", bills.CompanyId);
                oBcmd.Parameters.AddWithValue("@IsEqual", bills.BalanceAmount == bills.GrandTotal ? true : false);
                oBcmd.ExecuteNonQuery();
                conn.Close();

            }
            #region Update_Journal_Detail_Clearing_Status
            SqlConnection con = new SqlConnection(ConnectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
            cmd.CommandTimeout = 30;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@companyId", bills.CompanyId);
            cmd.Parameters.AddWithValue("@documentId", bills.Id);
            cmd.Parameters.AddWithValue("@docState", bills.DocumentState);
            cmd.Parameters.AddWithValue("@balanceAmount", bills.BalanceAmount);
            cmd.ExecuteNonQuery();
            con.Close();
            #endregion Update_Journal_Detail_Clearing_Status
            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, bills.CompanyId, bills.Id, bills.DocType, bills.DocSubType, bills.DocumentState, bills.DocCurrency, bills.GrandTotal, bills.BalanceAmount.Value != null ? bills.BalanceAmount.Value : 0, bills.ExchangeRate.Value, bills.ModifiedBy != null ? bills.ModifiedBy : bills.UserCreated, bills.DocDescription, null, 0, 0);

                if (lstdocumet.Any())
                    lstdocuments.AddRange(lstdocumet);
            }
            catch (Exception ex)
            {

            }
            #endregion Documentary History

        }
        private void ProcessDebitNote(IQueryable<DebitNote> lstDN, ReceiptDetail detail, List<DocumentHistoryModel> lstdocuments, DocumentVoidModel TObject, string ConnectionString)
        {
            DebitNote debitNote = lstDN?.FirstOrDefault(d => d.Id == detail.DocumentId);
            if (debitNote != null)
            {
                debitNote.BalanceAmount += detail.ReceiptAmount;
                if (debitNote.BalanceAmount == debitNote.GrandTotal)
                {
                    debitNote.DocumentState = InvoiceState.NotPaid;
                    debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                    debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                    debitNote.ModifiedDate = DateTime.UtcNow;
                    debitNote.ModifiedBy = "System";
                    debitNote.ObjectState = ObjectState.Modified;
                }
                else if (debitNote.BalanceAmount != debitNote.GrandTotal)
                {
                    debitNote.DocumentState = InvoiceState.PartialPaid;
                    debitNote.BaseBalanceAmount += (Math.Round((detail.ReceiptAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                    debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;

                    debitNote.ModifiedDate = DateTime.UtcNow;
                    debitNote.ModifiedBy = "System";
                    debitNote.ObjectState = ObjectState.Modified;
                }
            }

            #region Update_Journal_Detail_Clearing_Status
            SqlConnection con = new SqlConnection(ConnectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
            cmd.CommandTimeout = 30;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@companyId", debitNote.CompanyId);
            cmd.Parameters.AddWithValue("@documentId", debitNote.Id);
            cmd.Parameters.AddWithValue("@docState", debitNote.DocumentState);
            cmd.Parameters.AddWithValue("@balanceAmount", debitNote.BalanceAmount);
            cmd.ExecuteNonQuery();
            con.Close();
            #endregion Update_Journal_Detail_Clearing_Status

            #region Documentary History
            try
            {
                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, null, 0, 0);
                if (lstdocumet.Any())
                    lstdocuments.AddRange(lstdocumet);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            #endregion Documentary History
        }
        private void ProcessInvoiceOrCreditNote(IQueryable<Invoice> lstInv, ReceiptDetail detail, DocumentVoidModel TObject, List<DocumentHistoryModel> lstdocuments, string ConnectionString)
        {
            // IQueryable<Invoice> lstInvs = lstInv;
            Invoice invoice = lstInv?.FirstOrDefault(c => c.Id == detail.DocumentId);
            if (invoice != null && invoice.DocType == DocTypeConstants.Invoice)
            {
                invoice.BalanceAmount += detail.ReceiptAmount;
                if (invoice.BalanceAmount == invoice.GrandTotal)
                {
                    invoice.DocumentState = InvoiceState.NotPaid;
                    invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                    invoice.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                    invoice.ModifiedDate = DateTime.UtcNow;
                    invoice.ModifiedBy = "System";
                    invoice.ObjectState = ObjectState.Modified;

                    if (invoice.IsWorkFlowInvoice == true)
                        //FillWokflowInvoice(invoice);
                        FillWFInvoice(invoice, ConnectionString);

                }
                else if (invoice.BalanceAmount != invoice.GrandTotal)
                {
                    invoice.DocumentState = InvoiceState.PartialPaid;
                    invoice.BaseBalanceAmount += (Math.Round((detail.ReceiptAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                    invoice.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;

                    invoice.ModifiedDate = DateTime.UtcNow;
                    invoice.ModifiedBy = "System";
                    invoice.ObjectState = ObjectState.Modified;
                    if (invoice.IsWorkFlowInvoice == true)
                        //FillWokflowInvoice(invoice);
                        FillWFInvoice(invoice, ConnectionString);
                }

                if (invoice.IsOBInvoice == true)
                {
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                    oBcmd.CommandTimeout = 30;
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", invoice.BalanceAmount == invoice.GrandTotal && (invoice.AllocatedAmount == 0 || invoice.AllocatedAmount == null));
                    oBcmd.ExecuteNonQuery();
                    conn.Close();

                }
                if (invoice.IsOBInvoice != true)
                {
                    #region Update_Journal_Detail_Clearing_Status
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                    cmd.CommandTimeout = 30;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyId", invoice.CompanyId);
                    cmd.Parameters.AddWithValue("@documentId", invoice.Id);
                    cmd.Parameters.AddWithValue("@docState", invoice.DocumentState);
                    cmd.Parameters.AddWithValue("@balanceAmount", invoice.BalanceAmount);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    #endregion Update_Journal_Detail_Clearing_Status
                }
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, null, 0, 0);
                    if (lstdocumet.Any())
                        lstdocuments.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {

                }
                #endregion Documentary History
            }
            else
            {
                CreditNoteApplicationCompact cnApplication = _receiptDetailService.GetCNApplicationByDocId(detail.Id);
                if (cnApplication != null)
                {
                    DocumentResetModel voidModel = new DocumentResetModel();
                    voidModel.Id = cnApplication.Id;
                    voidModel.InvoiceId = cnApplication.InvoiceId;
                    voidModel.ResetDate = DateTime.UtcNow;
                    voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                    voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2")));
                    CNAplicationVoidREST(voidModel);
                }
            }
        }
        #endregion
        #region Private Methods
        private void InsertReceipt(ReceiptModel TObject, Receipt _receipt)
        {
            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Recept_Method_Of_Receipt);
            _receipt.CompanyId = TObject.CompanyId;
            _receipt.BalancingItemPayDRAmount = TObject.BalancingItemPayDRAmount;
            _receipt.BalancingItemPayDRCurrency = TObject.BalancingItemPayDRCurrency;
            _receipt.BalancingItemReciveCRAmount = TObject.BalancingItemReciveCRAmount;
            _receipt.BalancingItemReciveCRCurrency = TObject.BalancingItemReciveCRCurrency;
            _receipt.BankCharges = TObject.BankCharges;
            _receipt.BankChargesCurrency = TObject.BankChargesCurrency;
            _receipt.BankClearingDate = TObject.BankClearingDate;
            _receipt.BankReceiptAmmount = TObject.BankReceiptAmmount;
            _receipt.BankReceiptAmmountCurrency = TObject.BankReceiptAmmountCurrency;
            _receipt.BaseCurrency = TObject.BaseCurrency;
            _receipt.COAId = TObject.COAId;
            _receipt.DocCurrency = TObject.DocCurrency;
            _receipt.DocDate = TObject.DocDate;
            _receipt.IsVendor = TObject.IsVendor;
            _receipt.DocSubType = "Customer";
            _receipt.EntityType = "Customer";
            _receipt.ReceiptApplicationAmmount = TObject.ReceiptApplicationAmmount;
            _receipt.ReceiptApplicationCurrency = TObject.ReceiptApplicationCurrency;
            _receipt.SystemCalculatedExchangeRate = TObject.SystemCalculatedExchangeRate;
            if (TObject.VarianceExchangeRate != null)
            {

                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_Of_Insert_Receipt_And_Check_VarianceExchangeRate_Is_Not_Null);
                var varience = Convert.ToDecimal(TObject.VarianceExchangeRate);
                _receipt.VarianceExchangeRate = varience;
            }
            _receipt.EntityId = TObject.EntityId.Value;
            _receipt.ExDurationFrom = TObject.ExDurationFrom;
            _receipt.ExCurrency = TObject.ExCurrency;
            _receipt.ExcessPaidByClient = TObject.ExcessPaidByClient;
            _receipt.ExcessPaidByClientAmmount = TObject.ExcessPaidByClientAmmount;
            _receipt.ExcessPaidByClientCurrency = TObject.ExcessPaidByClientCurrency;
            _receipt.ExchangeRate = TObject.ExchangeRate;
            _receipt.IsGstSettings = TObject.IsGstSettings;
            _receipt.GSTExCurrency = TObject.GstReportingCurrency;
            if (TObject.IsGstSettings)
            {
                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_Of_Insert_Receipt_And_Check_IsGstSettings);
                _receipt.GSTExchangeRate = TObject.GstExchangeRate;
                _receipt.GSTExDurationFrom = TObject.GstdurationFrom;
            }
            _receipt.GSTTotalAmount = TObject.GSTTotalAmount;
            _receipt.IsMultiCurrency = TObject.ISMultiCurrency;
            _receipt.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
            _receipt.NoSupportingDocs = TObject.NoSupportingDocument;
            _receipt.GrandTotal = TObject.GrandTotal;
            _receipt.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
            _receipt.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
            _receipt.Remarks = TObject.Remarks;
            _receipt.IsExchangeRateLabel = TObject.IsExchangeRateLabel;
            _receipt.Status = TObject.Status;
            _receipt.ReceiptRefNo = TObject.ReceiptRefNo;
            _receipt.ServiceCompanyId = TObject.ServiceCompanyId;
            _receipt.ModeOfReceipt = TObject.ModeOfReceipt;
            _receipt.IsInterCompanyActive = TObject.IsInterCompanyActive;
            Log.Logger.ZInfo(ReceiptLoggingValidation.End_Of_The_Insert_Receipt);
        }
        private void UpdateReceiptDetails(ReceiptModel TObject, Receipt _receipt, bool? isEdit, string ConnectionString, long clearingReceiptCOA, bool isICActive, decimal? OldExRate, decimal? oldSysCalExRate, List<DocumentHistoryModel> lstDocuments, DateTime? oldDocDate, Dictionary<Guid, decimal> lstOfRoundingAmount, long? ServiceCompanyId)
        {
            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptDetails_Method);
            decimal roundingAmount = 0;
            if (_receipt != null)
            {
                //This block is meant to delete all transaction from ReceiptDetail and rollback all outstanding state in edit if user changed the entity or service entity
                if (isEdit == true)
                {
                    List<ReceiptDetail> lstReceptDetail = _receipt.ReceiptDetails.Where(a => a.Id != Guid.Empty && a.ReceiptAmount != 0).ToList();
                    if (lstReceptDetail.Any())
                    {
                        List<Guid> lstInvoiceIds = lstReceptDetail.Where(a => a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.CreditNote).Select(a => a.DocumentId).ToList();
                        List<Invoice> lstInvoices = lstInvoiceIds.Any() ? _invoiceService.GetListOfInvoices(TObject.CompanyId, lstInvoiceIds) : null;
                        List<Guid> lstDebitNoteIds = lstReceptDetail.Where(a => a.DocumentType == DocTypeConstants.DebitNote).Select(a => a.DocumentId).ToList();
                        List<DebitNote> lstDebitNotes = lstDebitNoteIds.Any() ? _debitNoteService.GetListOfDebitNote(TObject.CompanyId, lstDebitNoteIds) : null;
                        List<BillCompact> lstBills = lstReceptDetail.Where(a => a.DocumentType == DocTypeConstants.Bills).Select(a => a.DocumentId).Any() ? _debitNoteService.GetAllBillsByDocId(lstReceptDetail.Where(a => a.DocumentType == DocTypeConstants.Bills).Select(a => a.DocumentId).ToList(), TObject.CompanyId) : null;
                        List<CreditMemoCompact> lstMemos = lstReceptDetail.Where(a => a.DocumentType == DocTypeConstants.BillCreditMemo).Select(a => a.DocumentId).Any() ? _debitNoteService.GetAllCMByDocId(lstReceptDetail.Where(a => a.DocumentType == DocTypeConstants.BillCreditMemo).Select(a => a.DocumentId).ToList(), TObject.CompanyId) : null;
                        foreach (ReceiptDetail rDetail in lstReceptDetail)
                        {
                            roundingAmount = 0;
                            if (!TObject.ReceiptDetailModels.Any(a => a.DocumentId == rDetail.DocumentId))
                            {
                                if (rDetail.DocumentType == DocTypeConstants.Invoice || rDetail.DocumentType == DocTypeConstants.CreditNote)
                                {
                                    if (lstInvoices != null)
                                    {
                                        Invoice invoice = lstInvoices.FirstOrDefault(a => a.Id == rDetail.DocumentId);
                                        if (invoice != null)
                                        {
                                            invoice.BalanceAmount += rDetail.ReceiptAmount;
                                            if (invoice.GrandTotal == invoice.BalanceAmount)
                                                invoice.DocumentState = rDetail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.Not_Applied : InvoiceStates.NotPaid;
                                            else if (invoice.BalanceAmount == 0)
                                                invoice.DocumentState = rDetail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.FullyApplied : InvoiceStates.FullyPaid;
                                            else
                                                invoice.DocumentState = rDetail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.PartialApplied : InvoiceStates.PartialPaid;

                                            if (invoice.DocumentState == InvoiceState.NotPaid || invoice.DocumentState == InvoiceState.Not_Applied)
                                            {
                                                invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                                                invoice.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                            }

                                            else
                                            {
                                                invoice.BaseBalanceAmount += (Math.Round((rDetail.ReceiptAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                                invoice.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                            }


                                            if (invoice.IsWorkFlowInvoice == true)
                                                FillWFInvoice(invoice, ConnectionString);

                                            invoice.ModifiedBy = ReceiptConstants.System;
                                            invoice.ModifiedDate = DateTime.UtcNow;
                                            invoice.ObjectState = ObjectState.Modified;
                                            if (invoice.IsOBInvoice != true)
                                            {
                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, invoice);
                                                //UpdatePosting(up);
                                            }
                                            else
                                            {
                                                con = new SqlConnection(ConnectionString);
                                                if (con.State != ConnectionState.Open)
                                                    con.Open();
                                                cmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                                                cmd.CommandTimeout = 30;
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                                cmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                                cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                cmd.Parameters.AddWithValue("@IsEqual", invoice.DocumentState == InvoiceStates.NotPaid);
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            Guid cnAppId = Guid.NewGuid();
                                            if (rDetail.DocumentType == DocTypeConstants.CreditNote)
                                            {
                                                CreditNoteApplicationCompact cnApplication = _receiptDetailService.GetCNApplicationByDocId(rDetail.Id);
                                                if (cnApplication != null)
                                                {
                                                    cnAppId = cnApplication.Id;
                                                    con = new SqlConnection(ConnectionString);
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("DELETE_POSTING_JOURNAL", con);
                                                    cmd.CommandTimeout = 30;
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@DocumentId", cnApplication.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", cnApplication.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocType", DocTypeConstants.Application);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                            }
                                            try
                                            {
                                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(rDetail.DocumentType == DocTypeConstants.CreditNote ? cnAppId : _receipt.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate != null ? invoice.ExchangeRate.Value : 1, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, _receipt.Remarks, null, 0, 0);
                                                if (lstdocumet.Any())
                                                    lstDocuments.AddRange(lstdocumet);
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }

                                    rDetail.ObjectState = ObjectState.Deleted;
                                }
                                else if (rDetail.DocumentType == DocTypeConstants.DebitNote)
                                {
                                    DebitNote debitNote = lstDebitNoteIds != null ? lstDebitNotes.FirstOrDefault(a => a.Id == rDetail.DocumentId) : null;
                                    if (debitNote != null)
                                    {
                                        debitNote.BalanceAmount += rDetail.ReceiptAmount;

                                        if (debitNote.GrandTotal == debitNote.BalanceAmount)
                                            debitNote.DocumentState = InvoiceState.NotPaid;
                                        else if (debitNote.BalanceAmount == 0)
                                            debitNote.DocumentState = InvoiceState.FullyPaid;
                                        else
                                            debitNote.DocumentState = InvoiceState.PartialPaid;

                                        if (debitNote.DocumentState == InvoiceState.NotPaid)
                                        {
                                            debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                                            debitNote.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                        }
                                        else
                                        {
                                            debitNote.BaseBalanceAmount += (Math.Round((rDetail.ReceiptAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                            debitNote.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                        }

                                        debitNote.ModifiedBy = ReceiptConstants.System;
                                        debitNote.ModifiedDate = DateTime.UtcNow;
                                        debitNote.ObjectState = ObjectState.Modified;
                                        _debitNoteService.Update(debitNote);

                                        #region Update_journal
                                        using (con = new SqlConnection(ConnectionString))
                                        {
                                            if (con.State != ConnectionState.Open)
                                                con.Open();
                                            cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                            cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                            cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                            cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                            cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                            cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        #endregion Update_journal

                                        //UpdatePosting up = new UpdatePosting();
                                        //FillJournalState(up, debitNote);
                                        //UpdatePosting(up);
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_receipt.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate != null ? debitNote.ExchangeRate.Value : 1, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, _receipt.Remarks, null, 0, 0);
                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {
                                        }


                                        rDetail.ObjectState = ObjectState.Deleted;
                                    }
                                }
                                else if (rDetail.DocumentType == DocTypeConstants.Bills)
                                {
                                    BillCompact bill = lstBills != null ? lstBills.FirstOrDefault(a => a.Id == rDetail.DocumentId) : null;
                                    if (bill != null)
                                    {
                                        bill.BalanceAmount += rDetail.ReceiptAmount;

                                        if (bill.GrandTotal == bill.BalanceAmount)
                                            bill.DocumentState = InvoiceState.NotPaid;
                                        else if (bill.BalanceAmount == 0)
                                            bill.DocumentState = InvoiceState.FullyPaid;
                                        else
                                            bill.DocumentState = InvoiceState.PartialPaid;

                                        if (bill.DocumentState == InvoiceState.NotPaid)
                                        {
                                            bill.BaseBalanceAmount = bill.BaseGrandTotal;
                                            bill.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                        }
                                        else
                                        {
                                            bill.BaseBalanceAmount += (Math.Round((rDetail.ReceiptAmount * (bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                            bill.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                        }

                                        bill.ModifiedBy = ReceiptConstants.System;
                                        bill.ModifiedDate = DateTime.UtcNow;
                                        bill.ObjectState = ObjectState.Modified;
                                        _debitNoteService.Update(bill);

                                        #region Update_journal
                                        using (con = new SqlConnection(ConnectionString))
                                        {
                                            if (con.State != ConnectionState.Open)
                                                con.Open();
                                            cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Id", bill.Id);
                                            cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                            cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                            cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                            cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                            cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        #endregion Update_journal

                                        //UpdatePosting up = new UpdatePosting();
                                        //FillJournalState(up, bill);
                                        //UpdatePosting(up);
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_receipt.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount != null ? bill.BalanceAmount.Value : bill.GrandTotal, bill.ExchangeRate != null ? bill.ExchangeRate.Value : 1, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, _receipt.Remarks, null, 0, 0);
                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        rDetail.ObjectState = ObjectState.Deleted;
                                    }
                                }
                                else if (rDetail.DocumentType == DocTypeConstants.BillCreditMemo)
                                {
                                    CreditMemoCompact creditMemo = lstMemos != null ? lstMemos.FirstOrDefault(a => a.Id == rDetail.DocumentId) : null;
                                    if (creditMemo != null)
                                    {
                                        creditMemo.BalanceAmount += rDetail.ReceiptAmount;
                                        if (creditMemo.GrandTotal == creditMemo.BalanceAmount)
                                            creditMemo.DocumentState = InvoiceState.Not_Applied;
                                        else if (creditMemo.BalanceAmount == 0)
                                            creditMemo.DocumentState = InvoiceState.FullyApplied;
                                        else
                                            creditMemo.DocumentState = InvoiceState.PartialApplied;

                                        if (creditMemo.DocumentState == InvoiceState.NotPaid)
                                        {
                                            creditMemo.BaseBalanceAmount = creditMemo.BaseGrandTotal;
                                            creditMemo.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                        }
                                        else
                                        {
                                            creditMemo.BaseBalanceAmount += (Math.Round((rDetail.ReceiptAmount * (creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                            creditMemo.RoundingAmount += (rDetail.RoundingAmount != null && rDetail.RoundingAmount != 0) ? rDetail.RoundingAmount : 0;
                                        }

                                        creditMemo.ModifiedBy = ReceiptConstants.System;
                                        creditMemo.ModifiedDate = DateTime.UtcNow;
                                        creditMemo.ObjectState = ObjectState.Modified;
                                        _debitNoteService.Update(creditMemo);

                                        #region Update_journal
                                        using (con = new SqlConnection(ConnectionString))
                                        {
                                            if (con.State != ConnectionState.Open)
                                                con.Open();
                                            cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Id", creditMemo.Id);
                                            cmd.Parameters.AddWithValue("@CompanyId", creditMemo.CompanyId);
                                            cmd.Parameters.AddWithValue("@DocumentState", creditMemo.DocumentState);
                                            cmd.Parameters.AddWithValue("@BalanceAmount", creditMemo.BalanceAmount);
                                            cmd.Parameters.AddWithValue("@ModifiedBy", creditMemo.ModifiedBy);
                                            cmd.Parameters.AddWithValue("@ModifiedDate", creditMemo.ModifiedDate);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                        #endregion Update_journal

                                        //UpdatePosting up = new UpdatePosting();
                                        //FillJournalState(up, creditMemo);
                                        //UpdatePosting(up);

                                    }
                                    CreditMemoApplicationCompact cmApplication = _receiptDetailService.GetCMApplicationByDocId(rDetail.Id);
                                    if (cmApplication != null)
                                    {
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(cmApplication.Id, creditMemo.CompanyId, creditMemo.Id, creditMemo.DocType, creditMemo.DocSubType, creditMemo.DocumentState, creditMemo.DocCurrency, creditMemo.GrandTotal, creditMemo.BalanceAmount, creditMemo.ExchangeRate != null ? creditMemo.ExchangeRate.Value : 1, creditMemo.ModifiedBy != null ? creditMemo.ModifiedBy : creditMemo.UserCreated, _receipt.Remarks, null, 0, 0);
                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        con = new SqlConnection(ConnectionString);
                                        if (con.State != ConnectionState.Open)
                                            con.Open();
                                        cmd = new SqlCommand("DELETE_POSTING_JOURNAL", con);
                                        cmd.CommandTimeout = 30;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@DocumentId", cmApplication.Id);
                                        cmd.Parameters.AddWithValue("@CompanyId", cmApplication.CompanyId);
                                        cmd.Parameters.AddWithValue("@DocType", DocTypeConstants.CMApplication);
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                    rDetail.ObjectState = ObjectState.Deleted;
                                }
                            }
                        }
                    }
                }

                //Pradhan Modified based on new receipt offeset changes

                List<Invoice> lstInvoice = null;
                List<DebitNote> lstDebitNote = null;
                List<CreditMemoCompact> lstCM = null;
                List<BillCompact> lstBill = null;
                decimal? sumAllValue = 0;
                decimal? allGrandTotal = 0;
                if (TObject.ReceiptDetailModels.Any())
                {
                    lstInvoice = _invoiceService.GetListOfInvoices(_receipt.CompanyId, TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.Invoice || d.DocumentType == DocTypeConstants.CreditNote).Select(c => c.DocumentId).ToList());
                    lstDebitNote = _debitNoteService.GetListOfDebitNote(_receipt.CompanyId, TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList());

                    if (TObject.IsVendor)
                    {
                        lstBill = _debitNoteService.GetAllBillsByDocId(TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.Bills).Select(c => c.DocumentId).ToList(), _receipt.CompanyId);
                        lstCM = _debitNoteService.GetAllCMByDocId(TObject.ReceiptDetailModels.Where(d => d.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList(), _receipt.CompanyId);
                    }
                }
                //Checking the documentstate before proceeding to save the receipt details
                if ((lstInvoice != null && lstInvoice.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstDebitNote != null && lstDebitNote.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstBill != null && lstBill.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstCM != null && lstCM.Any(a => a.DocumentState == InvoiceStates.Void)))
                    throw new InvalidOperationException(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);

                #region 2_Tab_Transaction               
                if (lstBill != null)
                {
                    sumAllValue += lstBill.Sum(c => c.BalanceAmount);
                    allGrandTotal += lstBill.Sum(d => d.GrandTotal);
                }
                if (lstCM != null)
                {
                    sumAllValue += lstCM.Sum(c => c.BalanceAmount);
                    allGrandTotal += lstCM.Sum(d => d.GrandTotal);
                }
                if (lstInvoice != null)
                {
                    sumAllValue += lstInvoice.Sum(d => d.BalanceAmount);
                    allGrandTotal += lstInvoice.Sum(d => d.GrandTotal);
                }
                if (lstDebitNote != null)
                {
                    sumAllValue += lstDebitNote.Sum(d => d.BalanceAmount);
                    allGrandTotal += lstDebitNote.Sum(d => d.GrandTotal);
                }
                if ((sumAllValue != 0 || allGrandTotal != 0) && TObject.ReceiptDetailModels.Any())
                {
                    if (allGrandTotal < (TObject.ReceiptDetailModels.Sum(d => Math.Abs(d.ReceiptAmount))) || allGrandTotal != TObject.ReceiptDetailModels.Sum(c => Math.Abs(c.DocumentAmmount)))
                        throw new Exception(ReceiptConstant.Receipt_Status_Change);
                }

                #endregion 2_Tab_Transaction

                int? servEntCount = 0;
                long? icId = 0;
                if (TObject.ReceiptDetailModels.Any())
                {
                    List<long?> lstServeIds = new List<long?>();
                    lstServeIds.AddRange(TObject.ReceiptDetailModels.Select(d => d.ServiceCompanyId));
                    lstServeIds.Add(TObject.ServiceCompanyId);
                    servEntCount = lstServeIds.GroupBy(a => a.Value).Count();
                    if (servEntCount > 1)
                        icId = _chartOfAccountService.GetICAccountId(_receipt.CompanyId, _receipt.ServiceCompanyId);
                }
                decimal oldReceiptAmount = 0;
                string docState = null;
                foreach (ReceiptDetailModel detail in TObject.ReceiptDetailModels)
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Foreach_Of_ReceiptDetails_Method);
                    roundingAmount = 0;
                    detail.ReceiptAmount = detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.CreditNote ? -detail.ReceiptAmount : detail.ReceiptAmount;
                    detail.AmmountDue = detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.CreditNote ? -detail.AmmountDue : detail.AmmountDue;
                    detail.DocumentAmmount = detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.CreditNote ? -detail.DocumentAmmount : detail.DocumentAmmount;
                    if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_ElseIf_Of_UpdateReceiptDetails_And_Check_RecordStatus_Not_Equal_To_Added_And_Deleted);
                        ReceiptDetail receiptDetail = _receipt.ReceiptDetails.FirstOrDefault(a => a.Id == detail.Id);
                        if (receiptDetail != null)
                        {
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_UpdateReceiptDetails_And_Check_receiptDetail_Not_Equal_To_Null);
                            oldReceiptAmount = Math.Abs(receiptDetail.ReceiptAmount);
                            receiptDetail.ReceiptId = TObject.Id;
                            receiptDetail.AmmountDue = detail.AmmountDue;
                            receiptDetail.Currency = detail.Currency;
                            receiptDetail.DocumentAmmount = detail.DocumentAmmount;
                            receiptDetail.DocumentDate = detail.DocumentDate;
                            receiptDetail.DocumentNo = detail.DocumentNo;
                            receiptDetail.DocumentState = detail.DocumentState;
                            receiptDetail.DocumentType = detail.DocumentType;
                            receiptDetail.Nature = detail.Nature;
                            receiptDetail.ServiceCompanyId = detail.ServiceCompanyId;
                            receiptDetail.SystemReferenceNumber = detail.SystemReferenceNumber;
                            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
                            {
                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_UpdateReceiptDetails_And_Check_DocumentType);
                                Invoice invoice = lstInvoice?.FirstOrDefault(c => c.Id == detail.DocumentId);
                                if (invoice != null && invoice.DocumentState == InvoiceState.FullyPaid && oldReceiptAmount == detail.ReceiptAmount && oldReceiptAmount != 0 && detail.ReceiptAmount != 0 && receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null)
                                {

                                    lstOfRoundingAmount.Add(invoice.Id, receiptDetail.RoundingAmount.Value);
                                    roundingAmount = receiptDetail.RoundingAmount.Value;

                                }
                                if (receiptDetail.ReceiptAmount > detail.ReceiptAmount || receiptDetail.ReceiptAmount < detail.ReceiptAmount || TObject.ExchangeRate != OldExRate || TObject.SystemCalculatedExchangeRate != oldSysCalExRate || ServiceCompanyId != TObject.ServiceCompanyId)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_If_Condition_And_Check_ReceiptAmount);
                                    if (receiptDetail.ReceiptAmount > detail.ReceiptAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_If_Condition_Of_UpdateDateRecipt_And_Check_receiptDetail_Less_Than_detail);
                                        decimal Ammount = 0;
                                        Ammount = receiptDetail.ReceiptAmount - detail.ReceiptAmount;
                                        receiptDetail.ReceiptAmount -= Ammount;
                                        if (invoice != null)
                                        {
                                            docState = invoice.DocumentState;
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptDetails_And_Check_Invoice_Is_Not_Equal_To_null_Or_Not);
                                            invoice.BalanceAmount += Ammount;
                                            if (invoice.GrandTotal == invoice.BalanceAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptDetail_And_Check_GrandTotal_IsEqual_To_BalanceAmount);
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.Not_Applied : InvoiceState.NotPaid;
                                                invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                                                invoice.RoundingAmount += (receiptDetail.RoundingAmount != null && receiptDetail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                                receiptDetail.RoundingAmount = 0;
                                            }
                                            else if (invoice.GrandTotal > invoice.BalanceAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptDetail_And_Check_GrandTotal_GraterThan_To_BalanceAmount);
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.PartialApplied : InvoiceState.PartialPaid;


                                                if (oldReceiptAmount != detail.ReceiptAmount)
                                                    invoice.BaseBalanceAmount = oldReceiptAmount > detail.ReceiptAmount ? (invoice.BaseBalanceAmount + (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) : invoice.BaseBalanceAmount - (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));


                                                if (docState == InvoiceStates.FullyPaid)
                                                {
                                                    invoice.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? receiptDetail.RoundingAmount : invoice.RoundingAmount;
                                                    receiptDetail.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? 0 : receiptDetail.RoundingAmount;
                                                }
                                            }
                                            invoice.ModifiedBy = "System";
                                            invoice.ModifiedDate = DateTime.UtcNow;
                                            invoice.ObjectState = ObjectState.Modified;
                                            _invoiceService.Update(invoice);
                                            if (invoice.IsWorkFlowInvoice == true)
                                                FillWFInvoice(invoice, ConnectionString);
                                            if (invoice.IsOBInvoice == true)
                                            {
                                                con = new SqlConnection(ConnectionString);
                                                if (con.State != ConnectionState.Open)
                                                    con.Open();
                                                cmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                                                cmd.CommandTimeout = 30;
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                                cmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                                cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                cmd.Parameters.AddWithValue("@IsEqual", invoice.DocumentState == InvoiceStates.NotPaid ? true : false);
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            if (invoice.IsOBInvoice != true)
                                            {
                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal
                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, invoice);
                                                //UpdatePosting(up);
                                            }
                                        }
                                    }
                                    else if (receiptDetail.ReceiptAmount < detail.ReceiptAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Else_If_of_The_UpdateReceiptDetails_And_Check_receiptDetail_LessThan_detail);
                                        if (detail.AmmountDue == detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_If_Condition_UpdateRedeiptDetails_And_Check_AmmountDue_IsEqualTo_ReceiptAmount);
                                            if (invoice != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_Invoice_IsNot_Null_Or_Not);
                                                invoice.BalanceAmount = 0;
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.FullyApplied : InvoiceState.FullyPaid;
                                                invoice.ModifiedBy = "System";
                                                invoice.ModifiedDate = DateTime.UtcNow;
                                                invoice.ObjectState = ObjectState.Modified;
                                                _invoiceService.Update(invoice);
                                                if (invoice.IsWorkFlowInvoice == true)
                                                    FillWFInvoice(invoice, ConnectionString);
                                                if (invoice.DocType == DocTypeConstants.Invoice)
                                                {
                                                    if (detail.ReceiptAmount == invoice.GrandTotal)
                                                    {
                                                        roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)invoice.BaseGrandTotal);
                                                        if (roundingAmount != 0)
                                                            lstOfRoundingAmount.Add(invoice.Id, roundingAmount);
                                                        receiptDetail.RoundingAmount = roundingAmount;
                                                        invoice.RoundingAmount = (roundingAmount != 0 && invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? invoice.RoundingAmount - roundingAmount : 0;
                                                        invoice.BaseBalanceAmount = 0;
                                                    }

                                                    else if (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null)
                                                    {
                                                        invoice.BaseBalanceAmount = 0;
                                                        lstOfRoundingAmount.Add(invoice.Id, receiptDetail.RoundingAmount.Value);
                                                        roundingAmount = (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) ? receiptDetail.RoundingAmount.Value : 0;
                                                    }
                                                    else
                                                    {
                                                        if (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)
                                                            roundingAmount = ((invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? (decimal)invoice.RoundingAmount : 0);
                                                        else
                                                            roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount - oldReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)invoice.BaseBalanceAmount);

                                                        invoice.BaseBalanceAmount = 0;
                                                        if (roundingAmount != 0)
                                                            lstOfRoundingAmount.Add(invoice.Id, roundingAmount);
                                                        receiptDetail.RoundingAmount = roundingAmount;
                                                        invoice.RoundingAmount = (roundingAmount != 0 && (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)) ? invoice.RoundingAmount - roundingAmount : 0;
                                                    }
                                                }
                                                if (invoice.IsOBInvoice == true)
                                                {
                                                    con = new SqlConnection(ConnectionString);
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                                                    cmd.CommandTimeout = 30;
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                                    cmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                    cmd.Parameters.AddWithValue("@IsEqual", invoice.DocumentState == InvoiceStates.NotPaid ? true : false);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                if (invoice.IsOBInvoice != true)
                                                {
                                                    #region Update_journal
                                                    using (con = new SqlConnection(ConnectionString))
                                                    {
                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();
                                                        cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                        cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                        cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                        cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                        cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                        cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    #endregion Update_journal
                                                    //UpdatePosting up = new UpdatePosting();
                                                    //FillJournalState(up, invoice);
                                                    //UpdatePosting(up);
                                                }
                                            }
                                        }
                                        else if (detail.AmmountDue != detail.ReceiptAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_ElseIf_Condition_And_Check_AmmountDue_Is_NotEqualTo_ReceiptAmount);
                                            if (invoice != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_IfCondition_Of_UpdateReceiptDetail_And_Check_InVoice_is_not_EqualTo_Null_Or_Not);
                                                invoice.BalanceAmount = detail.AmmountDue - detail.ReceiptAmount;
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.PartialApplied : InvoiceState.PartialPaid;
                                                if (oldReceiptAmount != detail.ReceiptAmount)
                                                    invoice.BaseBalanceAmount = oldReceiptAmount > detail.ReceiptAmount ? invoice.BaseBalanceAmount + (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : invoice.BaseBalanceAmount - (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                if (docState == InvoiceStates.FullyPaid)
                                                {
                                                    invoice.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? receiptDetail.RoundingAmount : invoice.RoundingAmount;
                                                    receiptDetail.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? 0 : receiptDetail.RoundingAmount;
                                                }
                                                invoice.ModifiedBy = "System";
                                                invoice.ModifiedDate = DateTime.UtcNow;
                                                invoice.ObjectState = ObjectState.Modified;
                                                _invoiceService.Update(invoice);
                                                if (invoice.IsWorkFlowInvoice == true)
                                                    FillWFInvoice(invoice, ConnectionString);

                                                if (invoice.IsOBInvoice == true)
                                                {
                                                    con = new SqlConnection(ConnectionString);
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                                                    cmd.CommandTimeout = 30;
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                                    cmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                    cmd.Parameters.AddWithValue("@IsEqual", invoice.DocumentState == InvoiceStates.NotPaid ? true : false);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                if (invoice.IsOBInvoice != true)
                                                {
                                                    #region Update_journal
                                                    using (con = new SqlConnection(ConnectionString))
                                                    {
                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();
                                                        cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                        cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                        cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                        cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                        cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                        cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    #endregion Update_journal
                                                    //UpdatePosting up = new UpdatePosting();
                                                    //FillJournalState(up, invoice);
                                                    //UpdatePosting(up);
                                                }

                                            }
                                        }
                                    }

                                    if (detail.DocumentType != DocTypeConstants.CreditNote)
                                    {
                                        #region Documentary History
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, TObject.DocDate, -detail.ReceiptAmount, roundingAmount);

                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion Documentary History
                                    }

                                    #region CreditNote_Apllication_Updation
                                    if (detail.DocumentType == DocTypeConstants.CreditNote)
                                    {
                                        CreditNoteApplicationCompact cnApplication = _receiptDetailService.GetCNApplicationByDocId(detail.Id);
                                        if (cnApplication != null)
                                        {
                                            if (detail.ReceiptAmount > 0)
                                            {
                                                CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                                FillCreditNoteAplication(creditNoteModel, invoice, detail, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                                creditNoteModel.Id = cnApplication.Id;
                                                creditNoteModel.CreditNoteBalanceAmount = invoice.DocumentState != InvoiceState.FullyApplied ? invoice.BalanceAmount + cnApplication.CreditAmount : detail.ReceiptAmount;
                                                creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                creditNoteModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2")));
                                                CNAplicationREST(creditNoteModel);
                                            }
                                            else
                                            {
                                                DocumentResetModel voidModel = new DocumentResetModel();
                                                voidModel.Id = cnApplication.Id;
                                                voidModel.InvoiceId = cnApplication.InvoiceId;
                                                voidModel.ResetDate = DateTime.UtcNow;
                                                voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2"))); ;
                                                CNAplicationVoidREST(voidModel);
                                            }
                                        }
                                        else
                                        {
                                            if (detail.ReceiptAmount > 0)
                                            {
                                                CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                                FillCreditNoteAplication(creditNoteModel, invoice, detail, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                                creditNoteModel.Id = Guid.NewGuid();
                                                creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                CNAplicationREST(creditNoteModel);
                                            }
                                        }
                                    }
                                    #endregion CreditNote_Apllication_Updation

                                }

                                receiptDetail.ReceiptAmount = detail.ReceiptAmount;
                                receiptDetail.ObjectState = detail.ReceiptAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                            }
                            else if (detail.DocumentType == DocTypeConstants.DebitNote)
                            {
                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_ElseIf_Of_DocumentType_And_Check_is_Equlto_DebitNote_Or_Not);
                                DebitNote debitNote = lstDebitNote?.FirstOrDefault(c => c.Id == detail.DocumentId);

                                if (debitNote != null && debitNote.DocumentState == InvoiceState.FullyPaid && oldReceiptAmount == detail.ReceiptAmount && oldReceiptAmount != 0 && detail.ReceiptAmount != 0)
                                {
                                    if (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null)
                                    {
                                        lstOfRoundingAmount.Add(debitNote.Id, receiptDetail.RoundingAmount.Value);
                                        roundingAmount = receiptDetail.RoundingAmount.Value;
                                    }
                                }


                                if (receiptDetail.ReceiptAmount > detail.ReceiptAmount || receiptDetail.ReceiptAmount < detail.ReceiptAmount)
                                {
                                    if (receiptDetail.ReceiptAmount > detail.ReceiptAmount)
                                    {
                                        decimal Ammount = 0;
                                        Ammount = receiptDetail.ReceiptAmount - detail.ReceiptAmount;
                                        receiptDetail.ReceiptAmount = receiptDetail.ReceiptAmount - Ammount;
                                        if (debitNote != null)
                                        {
                                            docState = debitNote.DocumentState;
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_Invoice_is_null_or_not);
                                            debitNote.BalanceAmount = debitNote.BalanceAmount + Ammount;
                                            if (debitNote.GrandTotal == debitNote.BalanceAmount)
                                            {
                                                debitNote.DocumentState = InvoiceState.NotPaid;
                                                debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                                                debitNote.RoundingAmount += (receiptDetail.RoundingAmount != null && receiptDetail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                                receiptDetail.RoundingAmount = 0;
                                            }
                                            else if (debitNote.GrandTotal > debitNote.BalanceAmount)
                                            {
                                                debitNote.DocumentState = InvoiceState.PartialPaid;
                                                debitNote.BaseBalanceAmount = oldReceiptAmount > detail.ReceiptAmount ? debitNote.BaseBalanceAmount + (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : debitNote.BaseBalanceAmount - (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                if (docState == InvoiceStates.FullyPaid)
                                                {
                                                    debitNote.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? receiptDetail.RoundingAmount : debitNote.RoundingAmount;
                                                    receiptDetail.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? 0 : receiptDetail.RoundingAmount;
                                                }


                                            }
                                            debitNote.ModifiedBy = "System";
                                            debitNote.ModifiedDate = DateTime.UtcNow;
                                            debitNote.ObjectState = ObjectState.Modified;
                                            _debitNoteService.Update(debitNote);

                                            #region Update_journal
                                            using (con = new SqlConnection(ConnectionString))
                                            {
                                                if (con.State != ConnectionState.Open)
                                                    con.Open();
                                                cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                                cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                                cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                                cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                                cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                                cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            #endregion Update_journal

                                            //UpdatePosting up = new UpdatePosting();
                                            //FillJournalState(up, debitNote);
                                            //UpdatePosting(up);
                                        }
                                    }
                                    else if (receiptDetail.ReceiptAmount < detail.ReceiptAmount)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_ElseIf_Condition_And_Check_receiptDetail_Lessthan_detail);
                                        if (detail.AmmountDue == detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_IfCondition_And_Check_AmmountDue_Is_EqualTo_ReceiptAmount);
                                            if (debitNote != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptDetails_And_Check_Invoice_is_Null_or_Not);
                                                debitNote.DocumentState = InvoiceState.FullyPaid;
                                                debitNote.BalanceAmount = 0;
                                                if (detail.ReceiptAmount == debitNote.GrandTotal)
                                                {
                                                    roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)debitNote.BaseGrandTotal);
                                                    if (roundingAmount != 0)
                                                        lstOfRoundingAmount.Add(debitNote.Id, roundingAmount);
                                                    receiptDetail.RoundingAmount = roundingAmount;
                                                    debitNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)) ? debitNote.RoundingAmount - roundingAmount : 0;
                                                    debitNote.BaseBalanceAmount = 0;
                                                }

                                                else if (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null)
                                                {
                                                    debitNote.BaseBalanceAmount = 0;
                                                    lstOfRoundingAmount.Add(debitNote.Id, receiptDetail.RoundingAmount.Value);
                                                    roundingAmount = (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) ? receiptDetail.RoundingAmount.Value : 0;
                                                }
                                                else
                                                {
                                                    if (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)
                                                        roundingAmount = ((debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0) ? (decimal)debitNote.RoundingAmount : 0);
                                                    else
                                                        roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount - oldReceiptAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)debitNote.BaseBalanceAmount);


                                                    debitNote.BaseBalanceAmount = 0;
                                                    if (roundingAmount != 0)
                                                        lstOfRoundingAmount.Add(debitNote.Id, roundingAmount);
                                                    receiptDetail.RoundingAmount = roundingAmount;
                                                    debitNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)) ? debitNote.RoundingAmount - roundingAmount : 0;
                                                }


                                                debitNote.ObjectState = ObjectState.Modified;
                                                debitNote.ModifiedBy = "System";
                                                debitNote.ModifiedDate = DateTime.UtcNow;
                                                _debitNoteService.Update(debitNote);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, debitNote);
                                                //UpdatePosting(up);
                                            }

                                        }
                                        else if (detail.AmmountDue != detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Elseif_And_check_AmmountDue_Is_NotEqualTo_ReceiptAmount);
                                            if (debitNote != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_Invoice_Is_Null_Or_Not);
                                                debitNote.BalanceAmount = detail.AmmountDue - detail.ReceiptAmount;
                                                debitNote.DocumentState = InvoiceState.PartialPaid;
                                                debitNote.BaseBalanceAmount = oldReceiptAmount > detail.ReceiptAmount ? debitNote.BaseBalanceAmount + (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : debitNote.BaseBalanceAmount - (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                if (docState == InvoiceStates.FullyPaid)
                                                {
                                                    debitNote.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? receiptDetail.RoundingAmount : debitNote.RoundingAmount;
                                                    receiptDetail.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? 0 : receiptDetail.RoundingAmount;
                                                }


                                                debitNote.ModifiedBy = "System";
                                                debitNote.ModifiedDate = DateTime.UtcNow;
                                                debitNote.ObjectState = ObjectState.Modified;
                                                _debitNoteService.Update(debitNote);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, debitNote);
                                                //UpdatePosting(up);
                                            }
                                        }
                                    }
                                    #region Documentary History
                                    try
                                    {
                                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, debitNote.CompanyId, debitNote.Id, "Debit Note", "General", debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, TObject.DocDate, -detail.ReceiptAmount, roundingAmount);

                                        if (lstdocumet.Any())
                                            lstDocuments.AddRange(lstdocumet);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    #endregion Documentary History
                                }
                                receiptDetail.ReceiptAmount = detail.ReceiptAmount;
                                receiptDetail.ObjectState = detail.ReceiptAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;

                            }
                            if (TObject.IsVendor)
                            {
                                if (detail.DocumentType == DocTypeConstants.Bills)
                                {

                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_ElseIf_Of_DocumentType_And_Check_is_Equlto_DebitNote_Or_Not);
                                    BillCompact bill = lstBill?.FirstOrDefault(c => c.Id == detail.DocumentId);
                                    if (bill != null && bill.DocumentState == InvoiceState.FullyPaid && oldReceiptAmount == detail.ReceiptAmount && oldReceiptAmount != 0 && detail.ReceiptAmount != 0)
                                    {
                                        if (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null)
                                        {
                                            lstOfRoundingAmount.Add(bill.Id, receiptDetail.RoundingAmount.Value);
                                            roundingAmount = receiptDetail.RoundingAmount.Value;
                                        }
                                    }

                                    if (receiptDetail.ReceiptAmount > detail.ReceiptAmount || receiptDetail.ReceiptAmount < detail.ReceiptAmount)
                                    {
                                        if (receiptDetail.ReceiptAmount > detail.ReceiptAmount)
                                        {
                                            decimal Ammount = 0;
                                            Ammount = receiptDetail.ReceiptAmount - detail.ReceiptAmount;
                                            receiptDetail.ReceiptAmount -= Ammount;
                                            if (bill != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_Invoice_is_null_or_not);
                                                docState = bill.DocumentState;
                                                bill.BalanceAmount += Ammount;
                                                if (bill.GrandTotal == bill.BalanceAmount)
                                                {
                                                    bill.DocumentState = InvoiceState.NotPaid;
                                                    bill.BaseBalanceAmount = bill.BaseGrandTotal;
                                                    bill.RoundingAmount += (receiptDetail.RoundingAmount != null && receiptDetail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                                }
                                                else if (bill.GrandTotal > bill.BalanceAmount)
                                                {
                                                    bill.DocumentState = InvoiceState.PartialPaid;
                                                    bill.BaseBalanceAmount = oldReceiptAmount > detail.ReceiptAmount ? bill.BaseBalanceAmount + (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : bill.BaseBalanceAmount - (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                    if (docState == InvoiceStates.FullyPaid)
                                                    {
                                                        bill.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? receiptDetail.RoundingAmount : bill.RoundingAmount;
                                                        receiptDetail.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? 0 : receiptDetail.RoundingAmount;
                                                    }
                                                }
                                                bill.ModifiedBy = "System";
                                                bill.ModifiedDate = DateTime.UtcNow;
                                                bill.ObjectState = ObjectState.Modified;
                                                _debitNoteService.Update(bill);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", bill.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, bill);
                                                //UpdatePosting(up);
                                            }
                                        }
                                        else if (receiptDetail.ReceiptAmount < detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_ElseIf_Condition_And_Check_receiptDetail_Lessthan_detail);
                                            if (detail.AmmountDue == detail.ReceiptAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_IfCondition_And_Check_AmmountDue_Is_EqualTo_ReceiptAmount);
                                                if (bill != null)
                                                {
                                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptDetails_And_Check_Invoice_is_Null_or_Not);
                                                    bill.DocumentState = InvoiceState.FullyPaid;
                                                    bill.BalanceAmount = 0;

                                                    if (detail.ReceiptAmount == bill.GrandTotal)
                                                    {
                                                        roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)bill.BaseGrandTotal);
                                                        if (roundingAmount != 0)
                                                            lstOfRoundingAmount.Add(bill.Id, roundingAmount);
                                                        receiptDetail.RoundingAmount = roundingAmount;
                                                        bill.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (bill.RoundingAmount != null && bill.RoundingAmount != 0)) ? bill.RoundingAmount - roundingAmount : 0;
                                                        bill.BaseBalanceAmount = 0;
                                                    }
                                                    else if (oldReceiptAmount != detail.ReceiptAmount)
                                                    {
                                                        if (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null)
                                                        {
                                                            bill.BaseBalanceAmount = 0;
                                                            lstOfRoundingAmount.Add(bill.Id, receiptDetail.RoundingAmount.Value);
                                                            receiptDetail.RoundingAmount = receiptDetail.RoundingAmount;
                                                            roundingAmount = (receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) ? receiptDetail.RoundingAmount.Value : 0;
                                                        }
                                                        else
                                                        {
                                                            if (bill.RoundingAmount != null && bill.RoundingAmount != 0)
                                                                roundingAmount = ((bill.RoundingAmount != null && bill.RoundingAmount != 0) ? (decimal)bill.RoundingAmount : 0);
                                                            else
                                                                roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount - oldReceiptAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)bill.BaseBalanceAmount);

                                                            bill.BaseBalanceAmount = 0;
                                                            if (roundingAmount != 0)
                                                                lstOfRoundingAmount.Add(bill.Id, roundingAmount);
                                                            receiptDetail.RoundingAmount = roundingAmount;
                                                            bill.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (bill.RoundingAmount != null && bill.RoundingAmount != 0)) ? bill.RoundingAmount - roundingAmount : 0;
                                                        }

                                                    }
                                                    bill.ObjectState = ObjectState.Modified;
                                                    bill.ModifiedBy = "System";
                                                    bill.ModifiedDate = DateTime.UtcNow;
                                                    _debitNoteService.Update(bill);

                                                    #region Update_journal
                                                    using (con = new SqlConnection(ConnectionString))
                                                    {
                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();
                                                        cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.AddWithValue("@Id", bill.Id);
                                                        cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                                        cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                                        cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                                        cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                                        cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    #endregion Update_journal

                                                    //UpdatePosting up = new UpdatePosting();
                                                    //FillJournalState(up, bill);
                                                    //UpdatePosting(up);
                                                }
                                            }
                                            else if (detail.AmmountDue != detail.ReceiptAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Elseif_And_check_AmmountDue_Is_NotEqualTo_ReceiptAmount);
                                                if (bill != null)
                                                {
                                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_Invoice_Is_Null_Or_Not);
                                                    bill.BalanceAmount = detail.AmmountDue - detail.ReceiptAmount;
                                                    bill.DocumentState = InvoiceState.PartialPaid;

                                                    bill.BaseBalanceAmount = oldReceiptAmount > detail.ReceiptAmount ? bill.BaseBalanceAmount + (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : bill.BaseBalanceAmount - (Math.Round(Math.Abs(oldReceiptAmount - detail.ReceiptAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                    if (docState == InvoiceStates.FullyPaid)
                                                    {
                                                        bill.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? receiptDetail.RoundingAmount : bill.RoundingAmount;
                                                        receiptDetail.RoundingAmount = ((receiptDetail.RoundingAmount != 0 && receiptDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? 0 : receiptDetail.RoundingAmount;
                                                    }

                                                    bill.ModifiedBy = "System";
                                                    bill.ModifiedDate = DateTime.UtcNow;
                                                    bill.ObjectState = ObjectState.Modified;
                                                    _debitNoteService.Update(bill);

                                                    #region Update_journal
                                                    using (con = new SqlConnection(ConnectionString))
                                                    {
                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();
                                                        cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.AddWithValue("@Id", bill.Id);
                                                        cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                                        cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                                        cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                                        cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                                        cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    #endregion Update_journal

                                                    //UpdatePosting up = new UpdatePosting();
                                                    //FillJournalState(up, bill);
                                                    //UpdatePosting(up);
                                                }
                                            }
                                        }
                                        #region Documentary History
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, bill.DocDescription, TObject.DocDate, -detail.ReceiptAmount, roundingAmount);

                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion Documentary History
                                    }

                                    receiptDetail.ReceiptAmount = detail.ReceiptAmount;
                                    receiptDetail.ObjectState = detail.ReceiptAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                                }

                                else if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_ElseIf_Of_DocumentType_And_Check_is_Equlto_DebitNote_Or_Not);
                                    if (receiptDetail.ReceiptAmount > detail.ReceiptAmount || receiptDetail.ReceiptAmount < detail.ReceiptAmount || TObject.ExchangeRate != OldExRate || TObject.SystemCalculatedExchangeRate != oldSysCalExRate || oldDocDate != TObject.DocDate || ServiceCompanyId != TObject.ServiceCompanyId)
                                    {
                                        CreditMemoCompact memo = lstCM?.FirstOrDefault(c => c.Id == detail.DocumentId);

                                        #region CreditMemo_Apllication_Updation

                                        if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                                        {
                                            CreditMemoApplicationCompact cmApplication = _receiptDetailService.GetCMApplicationByDocId(detail.Id);
                                            if (cmApplication != null)
                                            {
                                                if (detail.ReceiptAmount > 0)
                                                {
                                                    CreditMemoApplicationModel creditMemoModel = new CreditMemoApplicationModel();
                                                    detail.Id = receiptDetail.Id;
                                                    FillCreditMemoAplication(creditMemoModel, memo, detail, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                                    creditMemoModel.Id = cmApplication.Id;
                                                    creditMemoModel.CreditMemoBalanceAmount = memo.DocumentState != InvoiceState.FullyApplied ? memo.BalanceAmount + cmApplication.CreditAmount : detail.ReceiptAmount;
                                                    creditMemoModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                    creditMemoModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                    creditMemoModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    creditMemoModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));
                                                    CMAplicationREST(creditMemoModel);
                                                }
                                                else
                                                {
                                                    DocumentResetModel voidModel = new DocumentResetModel();
                                                    voidModel.Id = cmApplication.Id;
                                                    voidModel.CreditMemoId = cmApplication.CreditMemoId;
                                                    voidModel.ResetDate = DateTime.UtcNow;
                                                    voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));
                                                    CMAplicationVoidREST(voidModel);
                                                }
                                            }
                                            else
                                            {
                                                if (detail.ReceiptAmount > 0)
                                                {
                                                    CreditMemoApplicationModel creditMemoModel = new CreditMemoApplicationModel();
                                                    detail.Id = receiptDetail.Id;
                                                    FillCreditMemoAplication(creditMemoModel, memo, detail, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                                    creditMemoModel.Id = Guid.NewGuid();
                                                    creditMemoModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                    creditMemoModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                    creditMemoModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    CMAplicationREST(creditMemoModel);
                                                }
                                            }
                                        }

                                        #endregion CreditMemo_Apllication_Updation
                                    }
                                    receiptDetail.ReceiptAmount = detail.ReceiptAmount;
                                    receiptDetail.ObjectState = detail.ReceiptAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                                }
                            }
                        }
                        else
                        {
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Else_And_check_ReceptDetail);
                            ReceiptDetail receiptDetailNew = new ReceiptDetail();
                            receiptDetailNew.Id = Guid.NewGuid();
                            receiptDetailNew.ReceiptId = TObject.Id;
                            receiptDetailNew.AmmountDue = detail.AmmountDue;
                            receiptDetailNew.Currency = detail.Currency;
                            receiptDetailNew.DocumentAmmount = detail.DocumentAmmount;
                            receiptDetailNew.DocumentDate = detail.DocumentDate;
                            receiptDetailNew.DocumentNo = detail.DocumentNo;
                            receiptDetailNew.DocumentState = detail.DocumentState;
                            receiptDetailNew.DocumentId = detail.DocumentId;
                            receiptDetailNew.DocumentType = detail.DocumentType;
                            receiptDetailNew.Nature = detail.Nature;
                            receiptDetailNew.RecOrder = TObject.ReceiptDetailModels.Max(c => c.RecOrder) + 1;
                            receiptDetailNew.ServiceCompanyId = detail.ServiceCompanyId;
                            receiptDetailNew.SystemReferenceNumber = detail.SystemReferenceNumber;
                            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
                            {
                                if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount || receiptDetailNew.ReceiptAmount < detail.ReceiptAmount)
                                {
                                    Invoice invoice = lstInvoice != null ? lstInvoice.Where(c => c.Id == detail.DocumentId).FirstOrDefault() : null;
                                    if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_receiptDetailNew_LessThan_detail);
                                        decimal Ammount = 0;
                                        Ammount = receiptDetailNew.ReceiptAmount - detail.ReceiptAmount;
                                        receiptDetailNew.ReceiptAmount = receiptDetailNew.ReceiptAmount - Ammount;
                                        if (invoice != null)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_Invoice_Is_Null_Or_Not);
                                            invoice.BalanceAmount = invoice.BalanceAmount + Ammount;
                                            if (invoice.GrandTotal == invoice.BalanceAmount)
                                            {
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.Not_Applied : InvoiceState.NotPaid;
                                            }
                                            else if (invoice.GrandTotal > invoice.BalanceAmount)
                                            {
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.PartialApplied : InvoiceState.PartialPaid;
                                                invoice.BaseBalanceAmount -= Math.Round(detail.ReceiptAmount * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                            }
                                            invoice.ModifiedBy = ReceiptConstants.System;
                                            invoice.ModifiedDate = DateTime.UtcNow;
                                            invoice.ObjectState = ObjectState.Modified;
                                            _invoiceService.Update(invoice);
                                            if (invoice.IsWorkFlowInvoice == true)
                                                FillWFInvoice(invoice, ConnectionString);

                                            #region Update_journal
                                            using (con = new SqlConnection(ConnectionString))
                                            {
                                                if (con.State != ConnectionState.Open)
                                                    con.Open();
                                                cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            #endregion Update_journal

                                            //UpdatePosting up = new UpdatePosting();
                                            //FillJournalState(up, invoice);
                                            //UpdatePosting(up);
                                        }
                                    }
                                    else if (receiptDetailNew.ReceiptAmount < detail.ReceiptAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Elseif_Condition_And_Check_receiptDetailNew_GraterThan_Or_Not);
                                        if (detail.AmmountDue == detail.ReceiptAmount)
                                        {
                                            if (invoice != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_invoice_Is_Null_Or_Not);
                                                invoice.BalanceAmount = 0;
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.FullyApplied : InvoiceState.FullyPaid;
                                                if (invoice.DocType == DocTypeConstants.Invoice)
                                                {
                                                    if (invoice.GrandTotal == detail.ReceiptAmount)
                                                    {
                                                        roundingAmount = Math.Round(Math.Abs(detail.ReceiptAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)invoice.BaseGrandTotal);
                                                    }
                                                    else
                                                    {
                                                        if (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)
                                                            roundingAmount = ((invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? (decimal)invoice.RoundingAmount : 0);
                                                        else
                                                            roundingAmount = Math.Round(detail.ReceiptAmount * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)invoice.BaseBalanceAmount;
                                                    }
                                                    invoice.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)) ? invoice.RoundingAmount - roundingAmount : 0;
                                                    receiptDetailNew.RoundingAmount = roundingAmount;
                                                    invoice.BaseBalanceAmount = 0;
                                                    if (roundingAmount != 0)
                                                        lstOfRoundingAmount.Add(invoice.Id, roundingAmount);

                                                }

                                                invoice.ModifiedBy = ReceiptConstants.System;
                                                invoice.ModifiedDate = DateTime.UtcNow;
                                                invoice.ObjectState = ObjectState.Modified;
                                                _invoiceService.Update(invoice);
                                                if (invoice.IsWorkFlowInvoice == true)
                                                    FillWFInvoice(invoice, ConnectionString);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, invoice);
                                                //UpdatePosting(up);
                                            }
                                        }
                                        else if (detail.AmmountDue != detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_ElseIf_And_Check_AmmountDue_Is_Not_Null_ReceiptAmount);
                                            if (invoice != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_check_Invoice_Is_Null_Or_Not);
                                                invoice.BalanceAmount = detail.AmmountDue - detail.ReceiptAmount;
                                                invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.PartialApplied : InvoiceState.PartialPaid;
                                                invoice.BaseBalanceAmount -= Math.Round(detail.ReceiptAmount * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                                invoice.ModifiedBy = ReceiptConstants.System;
                                                invoice.ModifiedDate = DateTime.UtcNow;
                                                invoice.ObjectState = ObjectState.Modified;
                                                _invoiceService.Update(invoice);
                                                if (invoice.IsWorkFlowInvoice == true)
                                                    FillWFInvoice(invoice, ConnectionString);


                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", invoice.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", invoice.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", invoice.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", invoice.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", invoice.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal
                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, invoice);
                                                //UpdatePosting(up);
                                            }
                                        }
                                        #region Documentary History
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, TObject.DocDate, -detail.ReceiptAmount, roundingAmount);

                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion Documentary History
                                    }



                                    #region CreditNote_Apllication_Creation
                                    if (detail.ReceiptAmount > 0 && detail.DocumentType == DocTypeConstants.CreditNote)
                                    {
                                        CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                        FillCreditNoteAplication(creditNoteModel, invoice, detail, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                        creditNoteModel.DocumentId = receiptDetailNew.Id;
                                        creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                        creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                        creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                        CNAplicationREST(creditNoteModel);
                                    }
                                    #endregion CreditNote_Apllication_Creation
                                }
                                receiptDetailNew.ReceiptAmount = detail.ReceiptAmount;
                                receiptDetailNew.ObjectState = ObjectState.Added;
                                _receiptDetailService.Insert(receiptDetailNew);
                            }
                            else if (detail.DocumentType == DocTypeConstants.DebitNote)
                            {
                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_ElseIf_and_Check_DocumentType);
                                if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount || receiptDetailNew.ReceiptAmount < detail.ReceiptAmount)
                                {
                                    DebitNote debitNote = lstDebitNote != null ? lstDebitNote.Where(c => c.Id == detail.DocumentId).FirstOrDefault() : null;
                                    if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount)
                                    {
                                        decimal Ammount = 0;
                                        Ammount = receiptDetailNew.ReceiptAmount - detail.ReceiptAmount;
                                        receiptDetailNew.ReceiptAmount = receiptDetailNew.ReceiptAmount - Ammount;
                                        if (debitNote != null)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_If_Block_And_Check_Invoice_Null_Or_Not);
                                            debitNote.BalanceAmount = debitNote.BalanceAmount + Ammount;
                                            if (debitNote.GrandTotal == debitNote.BalanceAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_GrandTotal_IsEqual_To_BalanceAmount);
                                                debitNote.DocumentState = InvoiceState.NotPaid;

                                            }
                                            else if (debitNote.GrandTotal > debitNote.BalanceAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Else_If_Condition_And_Check_GrandTotal_GraterThan_BalanceAmount);
                                                debitNote.DocumentState = InvoiceState.PartialPaid;
                                                debitNote.BaseBalanceAmount -= Math.Round(detail.ReceiptAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                            }
                                            debitNote.ObjectState = ObjectState.Modified;
                                            debitNote.ModifiedBy = ReceiptConstants.System;
                                            debitNote.ModifiedDate = DateTime.UtcNow;
                                            _debitNoteService.Update(debitNote);

                                            #region Update_journal
                                            using (con = new SqlConnection(ConnectionString))
                                            {
                                                if (con.State != ConnectionState.Open)
                                                    con.Open();
                                                cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                                cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                                cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                                cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                                cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                                cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                                cmd.ExecuteNonQuery();
                                                con.Close();
                                            }
                                            #endregion Update_journal

                                            //UpdatePosting up = new UpdatePosting();
                                            //FillJournalState(up, debitNote);
                                            //UpdatePosting(up);
                                        }

                                    }
                                    else if (receiptDetailNew.ReceiptAmount < detail.ReceiptAmount)
                                    {
                                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_ElseIf_And_And_Check_receiptDetailNew_Is_GraterThan_detail_Or_Not);
                                        if (detail.AmmountDue == detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_Into_If_And_Check_AmmountDue_Is_Equal_To_ReceiptAmount);
                                            if (debitNote != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_condition_And_Check_Invoice_Is_Null_Or_Not);
                                                debitNote.DocumentState = InvoiceState.FullyPaid;
                                                debitNote.BalanceAmount = 0;
                                                if (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)
                                                    roundingAmount = ((debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0) ? (decimal)debitNote.RoundingAmount : 0);
                                                else
                                                    roundingAmount = Math.Round(detail.ReceiptAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)debitNote.BaseBalanceAmount;
                                                debitNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)) ? debitNote.RoundingAmount - roundingAmount : 0;
                                                receiptDetailNew.RoundingAmount = roundingAmount;
                                                debitNote.BaseBalanceAmount = 0;
                                                if (roundingAmount != 0)
                                                    lstOfRoundingAmount.Add(debitNote.Id, roundingAmount);

                                                debitNote.ModifiedBy = ReceiptConstants.System;
                                                debitNote.ModifiedDate = DateTime.UtcNow;
                                                debitNote.ObjectState = ObjectState.Modified;
                                                _debitNoteService.Update(debitNote);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, debitNote);
                                                //UpdatePosting(up);
                                            }
                                        }
                                        else if (detail.AmmountDue != detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_ElseIf_Condition_And_Check_AmmountDue_Is_Not_Equal_To_ReceiptAmount);
                                            if (debitNote != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_IfCondition_And_Check_Invoice_Invoice_Is_Null_OR_Not);
                                                debitNote.BalanceAmount = detail.AmmountDue - detail.ReceiptAmount;
                                                debitNote.DocumentState = InvoiceState.PartialPaid;
                                                debitNote.BaseBalanceAmount -= Math.Round(detail.ReceiptAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                                debitNote.ModifiedBy = ReceiptConstants.System;
                                                debitNote.ModifiedDate = DateTime.UtcNow;
                                                debitNote.ObjectState = ObjectState.Modified;
                                                _debitNoteService.Update(debitNote);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", debitNote.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", debitNote.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", debitNote.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", debitNote.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", debitNote.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", debitNote.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, debitNote);
                                                //UpdatePosting(up);
                                            }
                                        }
                                    }
                                    #region Documentary History
                                    try
                                    {
                                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, debitNote.CompanyId, debitNote.Id, "Debit Note", "General", debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, TObject.DocDate, -detail.ReceiptAmount, roundingAmount);

                                        if (lstdocumet.Any())
                                            lstDocuments.AddRange(lstdocumet);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    #endregion Documentary History
                                }

                                receiptDetailNew.ReceiptAmount = detail.ReceiptAmount;
                                receiptDetailNew.ObjectState = ObjectState.Added;
                                _receiptDetailService.Insert(receiptDetailNew);
                            }
                            if (TObject.IsVendor)
                            {
                                if (detail.DocumentType == DocTypeConstants.Bills)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_ElseIf_and_Check_DocumentType);

                                    if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount || receiptDetailNew.ReceiptAmount < detail.ReceiptAmount)
                                    {
                                        BillCompact bill = lstBill != null ? lstBill.Where(c => c.Id == detail.DocumentId).FirstOrDefault() : null;
                                        if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount)
                                        {
                                            decimal Ammount = 0;
                                            Ammount = receiptDetailNew.ReceiptAmount - detail.ReceiptAmount;
                                            receiptDetailNew.ReceiptAmount = receiptDetailNew.ReceiptAmount - Ammount;
                                            if (bill != null)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_If_Block_And_Check_Invoice_Null_Or_Not);
                                                bill.BalanceAmount = bill.BalanceAmount + Ammount;
                                                if (bill.GrandTotal == bill.BalanceAmount)
                                                {
                                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_GrandTotal_IsEqual_To_BalanceAmount);
                                                    bill.DocumentState = InvoiceState.NotPaid;
                                                }
                                                else if (bill.GrandTotal > bill.BalanceAmount)
                                                {
                                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Else_If_Condition_And_Check_GrandTotal_GraterThan_BalanceAmount);
                                                    bill.DocumentState = InvoiceState.PartialPaid;
                                                    bill.BaseBalanceAmount -= Math.Round(detail.ReceiptAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                                }
                                                bill.ModifiedBy = ReceiptConstants.System;
                                                bill.ModifiedDate = DateTime.UtcNow;
                                                bill.ObjectState = ObjectState.Modified;
                                                _debitNoteService.Update(bill);

                                                #region Update_journal
                                                using (con = new SqlConnection(ConnectionString))
                                                {
                                                    if (con.State != ConnectionState.Open)
                                                        con.Open();
                                                    cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.AddWithValue("@Id", bill.Id);
                                                    cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                                    cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                                    cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                                    cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                                    cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                                    cmd.ExecuteNonQuery();
                                                    con.Close();
                                                }
                                                #endregion Update_journal

                                                //UpdatePosting up = new UpdatePosting();
                                                //FillJournalState(up, bill);
                                                //UpdatePosting(up);
                                            }

                                        }
                                        else if (receiptDetailNew.ReceiptAmount < detail.ReceiptAmount)
                                        {
                                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_ElseIf_And_And_Check_receiptDetailNew_Is_GraterThan_detail_Or_Not);
                                            if (detail.AmmountDue == detail.ReceiptAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_Into_If_And_Check_AmmountDue_Is_Equal_To_ReceiptAmount);
                                                if (bill != null)
                                                {
                                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_condition_And_Check_Invoice_Is_Null_Or_Not);
                                                    bill.DocumentState = InvoiceState.FullyPaid;
                                                    bill.BalanceAmount = 0;
                                                    if (bill.RoundingAmount != null && bill.RoundingAmount != 0)
                                                        roundingAmount = ((bill.RoundingAmount != null && bill.RoundingAmount != 0) ? (decimal)bill.RoundingAmount : 0);
                                                    else
                                                        roundingAmount = Math.Round(detail.ReceiptAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)bill.BaseBalanceAmount;
                                                    bill.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (bill.RoundingAmount != null && bill.RoundingAmount != 0)) ? bill.RoundingAmount - roundingAmount : 0;
                                                    receiptDetailNew.RoundingAmount = roundingAmount;
                                                    bill.BaseBalanceAmount = 0;
                                                    if (roundingAmount != 0)
                                                        lstOfRoundingAmount.Add(bill.Id, roundingAmount);

                                                    bill.ModifiedBy = ReceiptConstants.System;
                                                    bill.ModifiedDate = DateTime.UtcNow;
                                                    bill.ObjectState = ObjectState.Modified;
                                                    _debitNoteService.Update(bill);

                                                    #region Update_journal
                                                    using (con = new SqlConnection(ConnectionString))
                                                    {
                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();
                                                        cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.AddWithValue("@Id", bill.Id);
                                                        cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                                        cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                                        cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                                        cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                                        cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    #endregion Update_journal

                                                    //UpdatePosting up = new UpdatePosting();
                                                    //FillJournalState(up, bill);
                                                    //UpdatePosting(up);
                                                }

                                            }
                                            else if (detail.AmmountDue != detail.ReceiptAmount)
                                            {
                                                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_ElseIf_Condition_And_Check_AmmountDue_Is_Not_Equal_To_ReceiptAmount);
                                                if (bill != null)
                                                {
                                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_IfCondition_And_Check_Invoice_Invoice_Is_Null_OR_Not);
                                                    bill.BalanceAmount = detail.AmmountDue - detail.ReceiptAmount;
                                                    bill.DocumentState = InvoiceState.PartialPaid;
                                                    bill.BaseBalanceAmount -= Math.Round(detail.ReceiptAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                                    bill.ModifiedBy = ReceiptConstants.System;
                                                    bill.ModifiedDate = DateTime.UtcNow;
                                                    bill.ObjectState = ObjectState.Modified;
                                                    _debitNoteService.Update(bill);

                                                    #region Update_journal
                                                    using (con = new SqlConnection(ConnectionString))
                                                    {
                                                        if (con.State != ConnectionState.Open)
                                                            con.Open();
                                                        cmd = new SqlCommand("[Bean].[CommonPostingUpdate_proc]", con);
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.AddWithValue("@Id", bill.Id);
                                                        cmd.Parameters.AddWithValue("@CompanyId", bill.CompanyId);
                                                        cmd.Parameters.AddWithValue("@DocumentState", bill.DocumentState);
                                                        cmd.Parameters.AddWithValue("@BalanceAmount", bill.BalanceAmount);
                                                        cmd.Parameters.AddWithValue("@ModifiedBy", bill.ModifiedBy);
                                                        cmd.Parameters.AddWithValue("@ModifiedDate", bill.ModifiedDate);
                                                        cmd.ExecuteNonQuery();
                                                        con.Close();
                                                    }
                                                    #endregion Update_journal

                                                    //UpdatePosting up = new UpdatePosting();
                                                    //FillJournalState(up, bill);
                                                    //UpdatePosting(up);
                                                }
                                            }
                                        }
                                        #region Documentary History
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, bill.DocDescription, TObject.DocDate, -detail.ReceiptAmount, roundingAmount);

                                            if (lstdocumet.Any())
                                                lstDocuments.AddRange(lstdocumet);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion Documentary History
                                    }
                                    receiptDetailNew.ReceiptAmount = detail.ReceiptAmount;
                                    receiptDetailNew.ObjectState = ObjectState.Added;
                                    _receiptDetailService.Insert(receiptDetailNew);
                                }
                                else if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                                {
                                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entered_ElseIf_and_Check_DocumentType);
                                    if (receiptDetailNew.ReceiptAmount > detail.ReceiptAmount || receiptDetailNew.ReceiptAmount < detail.ReceiptAmount)
                                    {
                                        CreditMemoCompact creditMemo = lstCM?.FirstOrDefault(c => c.Id == detail.DocumentId);


                                        #region CreditMemo_Apllication_Creation
                                        receiptDetailNew.ReceiptAmount = detail.ReceiptAmount;
                                        if (detail.ReceiptAmount > 0 && detail.DocumentType == DocTypeConstants.BillCreditMemo)
                                        {
                                            CreditMemoApplicationModel creditMemoModel = new CreditMemoApplicationModel();
                                            detail.Id = receiptDetailNew.Id;
                                            FillCreditMemoAplication(creditMemoModel, creditMemo, detail, _receipt, servEntCount, icId, clearingReceiptCOA, isICActive);
                                            creditMemoModel.DocumentId = receiptDetailNew.Id;
                                            creditMemoModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                            creditMemoModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                            creditMemoModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                            CMAplicationREST(creditMemoModel);
                                        }
                                        #endregion CreditMemo_Apllication_Creation
                                    }
                                    receiptDetailNew.ObjectState = ObjectState.Added;
                                    _receiptDetailService.Insert(receiptDetailNew);
                                }
                            }
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Else_If_Condition_And_Check_RecordStatus_Is_Equal_To_Deleted_Or_Not);
                        ReceiptDetail receiptDetail = _receipt.ReceiptDetails.FirstOrDefault(a => a.Id == detail.Id);
                        if (receiptDetail != null)
                        {
                            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_receiptDetail_Is_Not_Null_Or_Not);
                            receiptDetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
            }
        }
        private void FillCreditNoteAplication(CreditNoteApplicationModel model, Invoice creditNote, ReceiptDetailModel receiptDetail, Receipt receipt, int? serEntityCount, long? icCOA, long clearingReceiptCOA, bool isICActive)
        {
            model.Id = Guid.NewGuid();
            model.CompanyId = creditNote.CompanyId;
            model.InvoiceId = creditNote.Id;
            model.DocNo = creditNote.DocNo;
            model.DocDate = creditNote.DocDate;
            model.CreditNoteApplicationNumber = creditNote.DocNo;
            model.DocCurrency = creditNote.DocCurrency;
            model.UserCreated = creditNote.UserCreated;
            model.CreatedDate = DateTime.UtcNow;
            model.CreditNoteApplicationDate = receipt.DocDate;
            model.IsNoSupportingDocument = creditNote.IsNoSupportingDocument;
            model.NoSupportingDocument = creditNote.NoSupportingDocs;
            model.CreditAmount = receiptDetail.ReceiptAmount;
            model.DocumentId = receiptDetail.Id;
            model.CreditNoteAmount = creditNote.GrandTotal;
            model.CreditNoteBalanceAmount = creditNote.DocumentState != InvoiceState.FullyApplied ? creditNote.BalanceAmount : receiptDetail.ReceiptAmount;
            model.IsGstSettings = creditNote.IsGstSettings;
            model.Status = CreditNoteApplicationStatus.Posted;
            model.DocSubType = DocTypeConstants.Application;
            model.ExchangeRate = creditNote.ExchangeRate;
            model.GSTExchangeRate = creditNote.GSTExchangeRate;
            model.IsOffset = true;
            model.Remarks = "CN Application - " + receipt.DocNo;
            model.CreditNoteApplicationDetailModels.Add(new CreditNoteApplicationDetailModel()
            {
                Id = Guid.NewGuid(),
                CreditNoteApplicationId = model.Id,
                DocType = DocTypeConstants.Receipt,
                DocumentId = receiptDetail.DocumentId,
                DocCurrency = creditNote.DocCurrency,
                CreditAmount = receiptDetail.ReceiptAmount,
                DocNo = receipt.DocNo,
                DocDate = receipt.DocDate,
                Nature = creditNote.Nature,
                ServiceEntityId = receipt.ServiceCompanyId,
                DocState = "Posted",
                COAId = ((receiptDetail.DocumentType == DocTypeConstants.CreditNote || receiptDetail.DocumentType == DocTypeConstants.BillCreditMemo) && receipt.ServiceCompanyId == receiptDetail.ServiceCompanyId) ? clearingReceiptCOA : (serEntityCount == 1 && isICActive != true) ? clearingReceiptCOA : serEntityCount > 1 || isICActive ? (receipt.BankReceiptAmmountCurrency != receipt.DocCurrency ? clearingReceiptCOA : icCOA) : _chartOfAccountService.GetByNameId(receiptDetail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, receipt.CompanyId),
                BaseCurrencyExchangeRate = receipt.DocCurrency == receipt.BankChargesCurrency ? receipt.ExchangeRate : receipt.SystemCalculatedExchangeRate
            });

        }
        private void FillCreditMemoAplication(CreditMemoApplicationModel model, CreditMemoCompact creditMemo, ReceiptDetailModel receiptDetail, Receipt receipt, int? serEntCount, long? icCOA, long clearingReceiptCOA, bool isICActive)
        {
            model.Id = Guid.NewGuid();
            model.CompanyId = creditMemo.CompanyId;
            model.CreditMemoId = creditMemo.Id;
            model.DocNo = creditMemo.DocNo;
            model.DocDate = creditMemo.DocDate;
            model.CreditMemoApplicationNumber = creditMemo.DocNo;
            model.DocCurrency = creditMemo.DocCurrency;
            model.UserCreated = receipt.UserCreated;
            model.CreatedDate = DateTime.UtcNow;
            model.CreditMemoApplicationDate = receipt.DocDate;
            model.IsNoSupportingDocument = receipt.IsNoSupportingDocument;
            model.NoSupportingDocument = receipt.NoSupportingDocs;
            model.CreditAmount = receiptDetail.ReceiptAmount;
            model.DocumentId = receiptDetail.Id;
            model.CreditMemoAmount = creditMemo.GrandTotal;
            model.CreditMemoBalanceAmount = creditMemo.DocumentState != InvoiceState.FullyApplied ? creditMemo.BalanceAmount : receiptDetail.ReceiptAmount;
            model.IsGstSettings = receipt.IsGstSettings;
            model.Status = CreditMemoApplicationStatus.Posted;
            model.DocSubType = DocTypeConstants.CMApplication;
            model.ExchangeRate = creditMemo.ExchangeRate;
            model.GSTExchangeRate = creditMemo.GSTExchangeRate;
            model.IsOffset = true;
            model.Remarks = "CM Application - " + receipt.DocNo;
            model.CreditMemoApplicationDetailModels.Add(new CreditMemoApplicationDetailModel()
            {
                Id = Guid.NewGuid(),
                CreditMemoApplicationId = model.Id,
                DocType = DocTypeConstants.Receipt,
                DocumentId = receiptDetail.DocumentId,
                DocCurrency = creditMemo.DocCurrency,
                CreditAmount = receiptDetail.ReceiptAmount,
                DocNo = receipt.DocNo,
                DocDate = receipt.DocDate,
                Nature = creditMemo.Nature,
                ServiceEntityId = receipt.ServiceCompanyId,
                DocState = "Posted",
                COAId = ((receiptDetail.DocumentType == DocTypeConstants.CreditNote || receiptDetail.DocumentType == DocTypeConstants.BillCreditMemo) && receipt.ServiceCompanyId == receiptDetail.ServiceCompanyId) ? clearingReceiptCOA : (serEntCount == 1 && isICActive != true) ? clearingReceiptCOA : serEntCount > 1 || isICActive ? (receipt.BankReceiptAmmountCurrency != receipt.DocCurrency ? clearingReceiptCOA : icCOA) : _chartOfAccountService.GetByNameId(receiptDetail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, receipt.CompanyId),
                BaseCurrencyExchangeRate = receipt.DocCurrency == receipt.BankChargesCurrency ? receipt.ExchangeRate : receipt.SystemCalculatedExchangeRate
            });

        }
        private static void CNAplicationREST(CreditNoteApplicationModel creditNoteModel)
        {
            var json = RestSharpHelper.ConvertObjectToJason(creditNoteModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                object obj = creditNoteModel;
                var response = RestSharpHelper.Post(url, "api/v2/invoice/savecreditnoteapplication", json);
                if (response.ErrorMessage != null)
                {
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
        private static void CMAplicationREST(CreditMemoApplicationModel creditMemoModel)
        {
            var json = RestSharpHelper.ConvertObjectToJason(creditMemoModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == ReceiptConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = creditMemoModel;
                var response = RestSharpHelper.Post(url, "/api/creditmemo/savecreditmemoapplication", json);
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
        private static void CNAplicationVoidREST(DocumentResetModel voidModel)
        {
            var json = RestSharpHelper.ConvertObjectToJason(voidModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == ReceiptConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = voidModel;
                var response = RestSharpHelper.Post(url, "api/v2/invoice/savecreditnoteapplicationvoid", json);
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
        private static void CMAplicationVoidREST(DocumentResetModel voidModel)
        {
            var json = RestSharpHelper.ConvertObjectToJason(voidModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == ReceiptConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = voidModel;
                var response = RestSharpHelper.Post(url, "/api/creditmemo/savecreditmemovoid", json);
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
        private void UpdateReceiptBalancingItems(ReceiptModel TObject, Receipt _receipt)
        {
            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptBalancingItems_Method);
            foreach (ReceiptBalancingItem detail in TObject.ReceiptBalancingItems)
            {
                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Foreach_Of_ReceiptBalancingItem_Method);
                if (detail.RecordStatus == "Added")
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_RecordStatus_IsEqual_To_Added);
                    detail.ObjectState = ObjectState.Added;
                    _receipt.ReceiptBalancingItems.Add(detail);
                }
                else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_RecordStatus_Is_Added_Or_Deleted);
                    ReceiptBalancingItem RBItem = _receipt.ReceiptBalancingItems.Where(a => a.Id == detail.Id).FirstOrDefault();
                    if (RBItem != null)
                    {
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_And_Check_RBItem_Is_Null_Or_Not);
                        RBItem.ReceiptId = TObject.Id;
                        RBItem.COAId = detail.COAId;
                        RBItem.Account = detail.Account;
                        RBItem.DocAmount = detail.DocAmount;
                        RBItem.Currency = detail.Currency;
                        RBItem.IsDisAllow = detail.IsDisAllow;
                        RBItem.ReciveORPay = detail.ReciveORPay;
                        RBItem.DocTaxAmount = detail.DocTaxAmount;
                        RBItem.TaxIdCode = detail.TaxIdCode;
                        RBItem.TaxId = detail.TaxId;
                        RBItem.TaxType = detail.TaxType;
                        RBItem.DocTotalAmount = detail.DocTotalAmount;
                        RBItem.CreatedDate = detail.CreatedDate;
                        RBItem.UserCreated = detail.UserCreated;
                        RBItem.ModifiedBy = detail.ModifiedBy;
                        RBItem.ModifiedDate = detail.ModifiedDate;
                        RBItem.Remarks = detail.Remarks;
                        RBItem.Status = detail.Status;
                        RBItem.IsPLAccount = detail.IsPLAccount;
                        RBItem.ObjectState = ObjectState.Modified;
                    }
                }
                else if (detail.RecordStatus == "Deleted")
                {
                    Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_ElseIf_And_Check_RecordStatus_Is_Equal_To_Deleted_Or_Not);
                    ReceiptBalancingItem receiptBalancingItem =
                        _receipt.ReceiptBalancingItems.Where(a => a.Id == detail.Id).FirstOrDefault();
                    if (receiptBalancingItem != null)
                    {
                        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_receiptBalancingItem_Is_Not_Equal_To_Null_Or_Not);
                        receiptBalancingItem.ObjectState = ObjectState.Deleted;
                    }
                }
            }
        }
        //private void UpdateReceiptGSTDetails(ReceiptModel TObject, Receipt _receipt)
        //{
        //    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_UpdateReceiptGSTDetails_Method);
        //    if (TObject.ReceiptGSTDetails != null)
        //    {
        //        Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_ReceiptGSTDetails_IsNot_Equal_To_Null_Or_Not);
        //        foreach (ReceiptGSTDetail detail in TObject.ReceiptGSTDetails)
        //        {
        //            Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Foreach_Of_ReceiptGSTDetail);
        //            if (detail.RecordStatus == "Added")
        //            {
        //                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_If_Condition_And_Check_RecordStatus_is_Added);
        //                detail.ObjectState = ObjectState.Added;
        //                _receipt.ReceiptGSTDetails.Add(detail);
        //            }
        //            else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
        //            {
        //                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_Into_Else_If_Condition_And_Check_RecordStatus_is_Not_Eqalto_added_And_Deleted);
        //                ReceiptGSTDetail receiptGSTDetail = _receipt.ReceiptGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                if (receiptGSTDetail != null)
        //                {
        //                    receiptGSTDetail.ReceiptId = TObject.Id;
        //                    receiptGSTDetail.TaxId = detail.TaxId;
        //                    receiptGSTDetail.Amount = detail.Amount;
        //                    receiptGSTDetail.TaxAmount = detail.TaxAmount;
        //                    receiptGSTDetail.TotalAmount = detail.TotalAmount;
        //                    receiptGSTDetail.TaxCode = detail.TaxCode;

        //                    receiptGSTDetail.ObjectState = ObjectState.Modified;
        //                }
        //            }
        //            else if (detail.RecordStatus == "Deleted")
        //            {
        //                Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_RecordStatus_Is_Equal_To_Deleted_Or_Not);
        //                ReceiptGSTDetail receiptGSTDetail = _receipt.ReceiptGSTDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
        //                if (receiptGSTDetail != null)
        //                {
        //                    Log.Logger.ZInfo(ReceiptLoggingValidation.Entred_If_Condition_And_Check_receiptGSTDetail_Is_NotEqual_To_Null_Or_Not);
        //                    receiptGSTDetail.ObjectState = ObjectState.Deleted;
        //                }
        //            }
        //        }
        //    }
        //}
        private void FillCreditNoteApplicationModel(ReceiptDetailModel RDModel, ReceiptDetail receiptDetail)
        {
            RDModel.Id = receiptDetail.Id;
            RDModel.ReceiptId = receiptDetail.ReceiptId;
            RDModel.AmmountDue = receiptDetail.AmmountDue;
            RDModel.Currency = receiptDetail.Currency;
            RDModel.DocumentDate = receiptDetail.DocumentDate;
            RDModel.DocumentNo = receiptDetail.DocumentNo;
            RDModel.DocumentState = receiptDetail.DocumentState;
            RDModel.DocumentAmmount = receiptDetail.DocumentAmmount;
            RDModel.DocumentType = receiptDetail.DocumentType;
            RDModel.Nature = receiptDetail.Nature;
            RDModel.ReceiptAmount = receiptDetail.ReceiptAmount;
            RDModel.SystemReferenceNumber = receiptDetail.SystemReferenceNumber;
            RDModel.DocumentId = receiptDetail.DocumentId;
        }
        private void FillReceiptModel(ReceiptModel receiptModel, Receipt receipt)
        {
            receiptModel.Id = receipt.Id;
            receiptModel.CompanyId = receipt.CompanyId;
            receiptModel.BankCharges = receipt.BankCharges;
            receiptModel.IsModify = receipt.ClearCount > 0;
            receiptModel.BankChargesCurrency = receipt.BankChargesCurrency;
            receiptModel.BankClearingDate = receipt.BankClearingDate;
            receiptModel.BankReceiptAmmount = receipt.BankReceiptAmmount;
            receiptModel.BankReceiptAmmountCurrency = receipt.BankReceiptAmmountCurrency;
            receiptModel.BaseCurrency = receipt.BaseCurrency;
            receiptModel.ExtensionType = DocTypeConstants.General;
            receiptModel.COAId = receipt.COAId;
            receiptModel.DocSubType = DocTypeConstants.Receipt;
            Invoice creditNote = _invoiceService.GetInvoiceByDocumentByState(receipt.Id);
            if (creditNote != null && creditNote.DocumentState != "Void")
            {
                receiptModel.CreditNoteId = creditNote.Id;
                receiptModel.CreditNoteDocNumber = creditNote.DocNo;
                receiptModel.IsEditableExcessPaid = creditNote.DocumentState == InvoiceState.Not_Applied ? true : false;
            }
            receiptModel.DocCurrency = receipt.DocCurrency;
            receiptModel.DocDate = receipt.DocDate;
            receiptModel.DocNo = receipt.DocNo;
            receiptModel.IsVendor = receipt.IsVendor.Value;
            receiptModel.EntityId = receipt.EntityId;
            receiptModel.EntityName = _beanEntityService.GetEntityNameById(receipt.EntityId);
            receiptModel.CustCreditlimit = _beanEntityService.GetEntityCreditTermsValue(receipt.EntityId);
            receiptModel.ExcessPaidByClient = receipt.ExcessPaidByClient;
            receiptModel.ExcessPaidByClientAmmount = receipt.ExcessPaidByClientAmmount;
            receiptModel.ExDurationFrom = receipt.ExDurationFrom;
            receiptModel.GrandTotal = receipt.GrandTotal;
            receiptModel.GstdurationFrom = receipt.GSTExDurationFrom;
            receiptModel.GstExchangeRate = receipt.GSTExchangeRate;
            receiptModel.GstReportingCurrency = receipt.GSTExCurrency;
            receiptModel.GSTTotalAmount = receipt.GSTTotalAmount;
            receiptModel.IsGstSettings = receipt.IsGstSettings;
            receiptModel.ISMultiCurrency = receipt.IsMultiCurrency;
            receiptModel.IsNoSupportingDocument = receipt.IsNoSupportingDocument;
            receiptModel.ModeOfReceipt = receipt.ModeOfReceipt;
            receiptModel.NoSupportingDocument = receipt.NoSupportingDocs;
            receiptModel.ReceiptRefNo = receipt.ReceiptRefNo;
            receiptModel.ServiceCompanyId = receipt.ServiceCompanyId;
            receiptModel.SystemRefNo = receipt.SystemRefNo;
            receiptModel.Remarks = receipt.Remarks;
            receiptModel.UserCreated = receipt.UserCreated;
            receiptModel.CreatedDate = receipt.CreatedDate;
            receiptModel.ModifiedBy = receipt.ModifiedBy;
            receiptModel.ModifiedDate = receipt.ModifiedDate;
            receiptModel.ExcessPaidByClientCurrency = receipt.ExcessPaidByClientCurrency;
            receiptModel.Version = "0x" + string.Concat(Array.ConvertAll(receipt.Version, x => x.ToString("X2")));
            receiptModel.PeriodLockPassword = receipt.PeriodLockPassword;
            receiptModel.BalancingItemReciveCRAmount = receipt.BalancingItemReciveCRAmount;
            receiptModel.BalancingItemReciveCRCurrency = receipt.BalancingItemReciveCRCurrency;
            receiptModel.BalancingItemPayDRAmount = receipt.BalancingItemPayDRAmount;
            receiptModel.BalancingItemPayDRCurrency = receipt.BalancingItemPayDRCurrency;
            receiptModel.ExcessPaidByClient = receipt.ExcessPaidByClient;
            receiptModel.ExCurrency = receipt.ExCurrency;
            receiptModel.ExchangeRate = receipt.ExchangeRate;
            receiptModel.ReceiptApplicationCurrency = receipt.ReceiptApplicationCurrency;
            receiptModel.ReceiptApplicationAmmount = receipt.ReceiptApplicationAmmount;
            receiptModel.SystemCalculatedExchangeRate = receipt.SystemCalculatedExchangeRate;
            var deci = Convert.ToDecimal(receipt.VarianceExchangeRate);
            receiptModel.VarianceExchangeRate = Math.Round(deci, 2) + "%";
            receiptModel.DocumentState = receipt.DocumentState;
            receiptModel.Status = receipt.Status;
            receiptModel.IsExchangeRateLabel = receipt.IsExchangeRateLabel;
            receiptModel.IsInterCompanyActive = receipt.IsInterCompanyActive;
            receiptModel.IsBaseCurrencyRateChanged = receipt.IsBaseCurrencyRateChanged;
            receiptModel.IsGSTCurrencyRateChanged = receipt.IsGSTCurrencyRateChanged;
            receiptModel.ReceiptBalancingItems = receipt.ReceiptBalancingItems.OrderBy(c => c.RecOrder).ToList();
        }
        private void FillReceiptModelNew(ReceiptModel receiptModel, Receipt receipt, Invoice invoice, string entityName, decimal? creditTermDecimal)
        {
            receiptModel.Id = receipt.Id;
            receiptModel.CompanyId = receipt.CompanyId;
            receiptModel.BankCharges = receipt.BankCharges;
            receiptModel.IsModify = receipt.ClearCount > 0;
            receiptModel.BankChargesCurrency = receipt.BankChargesCurrency;
            receiptModel.BankClearingDate = receipt.BankClearingDate;
            receiptModel.BankReceiptAmmount = receipt.BankReceiptAmmount;
            receiptModel.BankReceiptAmmountCurrency = receipt.BankReceiptAmmountCurrency;
            receiptModel.BaseCurrency = receipt.BaseCurrency;
            receiptModel.ExtensionType = DocTypeConstants.General;
            receiptModel.COAId = receipt.COAId;
            receiptModel.DocSubType = DocTypeConstants.Receipt;
            // Invoice creditNote = _invoiceService.GetInvoiceByDocumentByState(receipt.Id);
            Invoice creditNote = invoice;
            if (creditNote != null && creditNote.DocumentState != "Void")
            {
                receiptModel.CreditNoteId = creditNote.Id;
                receiptModel.CreditNoteDocNumber = creditNote.DocNo;
                receiptModel.IsEditableExcessPaid = creditNote.DocumentState == InvoiceState.Not_Applied ? true : false;
            }
            receiptModel.DocCurrency = receipt.DocCurrency;
            receiptModel.DocDate = receipt.DocDate;
            receiptModel.DocNo = receipt.DocNo;
            receiptModel.IsVendor = receipt.IsVendor.Value;
            receiptModel.EntityId = receipt.EntityId;
            // receiptModel.EntityName = _beanEntityService.GetEntityNameById(receipt.EntityId);
            receiptModel.EntityName = entityName;
            // receiptModel.CustCreditlimit = _beanEntityService.GetEntityCreditTermsValue(receipt.EntityId);
            receiptModel.CustCreditlimit = creditTermDecimal;
            receiptModel.ExcessPaidByClient = receipt.ExcessPaidByClient;
            receiptModel.ExcessPaidByClientAmmount = receipt.ExcessPaidByClientAmmount;
            receiptModel.ExDurationFrom = receipt.ExDurationFrom;
            receiptModel.GrandTotal = receipt.GrandTotal;
            receiptModel.GstdurationFrom = receipt.GSTExDurationFrom;
            receiptModel.GstExchangeRate = receipt.GSTExchangeRate;
            receiptModel.GstReportingCurrency = receipt.GSTExCurrency;
            receiptModel.GSTTotalAmount = receipt.GSTTotalAmount;
            receiptModel.IsGstSettings = receipt.IsGstSettings;
            receiptModel.ISMultiCurrency = receipt.IsMultiCurrency;
            receiptModel.IsNoSupportingDocument = receipt.IsNoSupportingDocument;
            receiptModel.ModeOfReceipt = receipt.ModeOfReceipt;
            receiptModel.NoSupportingDocument = receipt.NoSupportingDocs;
            receiptModel.ReceiptRefNo = receipt.ReceiptRefNo;
            receiptModel.ServiceCompanyId = receipt.ServiceCompanyId;
            receiptModel.SystemRefNo = receipt.SystemRefNo;
            receiptModel.Remarks = receipt.Remarks;
            receiptModel.UserCreated = receipt.UserCreated;
            receiptModel.CreatedDate = receipt.CreatedDate;
            receiptModel.ModifiedBy = receipt.ModifiedBy;
            receiptModel.ModifiedDate = receipt.ModifiedDate;
            receiptModel.ExcessPaidByClientCurrency = receipt.ExcessPaidByClientCurrency;
            receiptModel.Version = "0x" + string.Concat(Array.ConvertAll(receipt.Version, x => x.ToString("X2")));
            receiptModel.PeriodLockPassword = receipt.PeriodLockPassword;
            receiptModel.BalancingItemReciveCRAmount = receipt.BalancingItemReciveCRAmount;
            receiptModel.BalancingItemReciveCRCurrency = receipt.BalancingItemReciveCRCurrency;
            receiptModel.BalancingItemPayDRAmount = receipt.BalancingItemPayDRAmount;
            receiptModel.BalancingItemPayDRCurrency = receipt.BalancingItemPayDRCurrency;
            receiptModel.ExcessPaidByClient = receipt.ExcessPaidByClient;
            receiptModel.ExCurrency = receipt.ExCurrency;
            receiptModel.ExchangeRate = receipt.ExchangeRate;
            receiptModel.ReceiptApplicationCurrency = receipt.ReceiptApplicationCurrency;
            receiptModel.ReceiptApplicationAmmount = receipt.ReceiptApplicationAmmount;
            receiptModel.SystemCalculatedExchangeRate = receipt.SystemCalculatedExchangeRate;
            var deci = Convert.ToDecimal(receipt.VarianceExchangeRate);
            receiptModel.VarianceExchangeRate = Math.Round(deci, 2) + "%";
            receiptModel.DocumentState = receipt.DocumentState;
            receiptModel.Status = receipt.Status;
            receiptModel.IsExchangeRateLabel = receipt.IsExchangeRateLabel;
            receiptModel.IsInterCompanyActive = receipt.IsInterCompanyActive;
            receiptModel.IsBaseCurrencyRateChanged = receipt.IsBaseCurrencyRateChanged;
            receiptModel.IsGSTCurrencyRateChanged = receipt.IsGSTCurrencyRateChanged;
            receiptModel.ReceiptBalancingItems = receipt.ReceiptBalancingItems.OrderBy(c => c.RecOrder).ToList();
        }
        private void FillNewReceiptModel(ReceiptModel receiptModel, AppsWorld.ReceiptModule.Entities.FinancialSetting financSettings)
        {
            long companyId = financSettings.CompanyId;
            DateTime lastReceipt = _receiptService.CreateReceiptNew(companyId);
            // Receipt lastReceipt1 = _receiptService.CreateReceipt(companyId);
            receiptModel.Id = Guid.NewGuid();
            receiptModel.CompanyId = companyId;
            //receiptModel.DocDate = lastReceipt1 == null ? DateTime.Now : lastReceipt1.DocDate;
            receiptModel.DocDate = lastReceipt == null ? DateTime.Now : lastReceipt;
            receiptModel.IsVendor = false;
            receiptModel.ExtensionType = ExtensionType.General;
            receiptModel.SaveType = "Direct";
            receiptModel.NoSupportingDocument = false;
            receiptModel.CreatedDate = DateTime.UtcNow;

            receiptModel.BaseCurrency = financSettings.BaseCurrency;
            receiptModel.DocumentState = AppsWorld.ReceiptModule.Infra.ReceiptState.NotPaid;
        }

        private string GetAutoNumberByEntityType(long companyId, Receipt lastInvoice, string entityType, AppsWorld.ReceiptModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.ReceiptModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewReceiptDocumentNumber(companyId);
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

        private string GetNewReceiptDocumentNumber(long CompanyId)
        {
            Receipt receipt = _receiptService.CreateReceipt(CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (receipt != null)
            {
                string strOldNo = String.Empty;
                Receipt duplicatReceipt;
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
                {
                    docNo = long.Parse(strOldNo);
                }
                catch
                {
                }
                index = strOldDocNo.LastIndexOf(strOldNo);
                do
                {
                    docNo++;
                    strNewNo = docNo.ToString().PadLeft(strOldNo.Length, '0');
                    strNewDocNo = (docNo == 1) ? strOldDocNo + strNewNo : strOldDocNo.Substring(0, index) + strNewNo;

                    duplicatReceipt = _receiptService.GetDocNo(strNewDocNo, CompanyId);
                } while (duplicatReceipt != null);
            }
            return strNewDocNo;
        }
        private void FillReceiptDetail(ReceiptDetailModel rdModel, ReceiptDetail receiptDetail)
        {
            rdModel.Id = receiptDetail.Id;
            rdModel.ReceiptId = receiptDetail.ReceiptId;
            rdModel.AmmountDue = receiptDetail.AmmountDue;
            rdModel.Currency = receiptDetail.Currency;
            rdModel.DocumentDate = receiptDetail.DocumentDate;
            rdModel.DocumentNo = receiptDetail.DocumentNo;
            rdModel.DocumentState = receiptDetail.DocumentState;
            rdModel.DocumentAmmount = receiptDetail.DocumentAmmount;
            rdModel.DocumentType = receiptDetail.DocumentType;
            rdModel.Nature = receiptDetail.Nature;
            rdModel.ReceiptAmount = receiptDetail.ReceiptAmount;
            rdModel.SystemReferenceNumber = receiptDetail.SystemReferenceNumber;
        }
        private bool IsDocumentNumberExists(string DocNo, Guid id, long companyId)
        {
            Receipt doc = _receiptService.CheckDocNo(id, DocNo, companyId);
            return doc != null;
        }

        private void FillJournalState(UpdatePosting _posting, Invoice invoice)
        {
            _posting.Id = invoice.Id;
            _posting.CompanyId = invoice.CompanyId;
            _posting.ModifiedDate = invoice.ModifiedDate;
            _posting.ModifiedBy = invoice.ModifiedBy;
            _posting.DocumentState = invoice.DocumentState;
            _posting.BalanceAmount = invoice.BalanceAmount;
        }
        private void FillJournalState(UpdatePosting _posting, DebitNote debit)
        {
            _posting.Id = debit.Id;
            _posting.CompanyId = debit.CompanyId;
            _posting.DocumentState = debit.DocumentState;
            _posting.ModifiedDate = debit.ModifiedDate;
            _posting.ModifiedBy = debit.ModifiedBy;
            _posting.BalanceAmount = debit.BalanceAmount;
        }
        private void FillJournalState(UpdatePosting _posting, BillCompact bill)
        {
            _posting.Id = bill.Id;
            _posting.CompanyId = bill.CompanyId;
            _posting.DocumentState = bill.DocumentState;
            _posting.ModifiedDate = bill.ModifiedDate;
            _posting.ModifiedBy = bill.ModifiedBy;
            _posting.BalanceAmount = bill.BalanceAmount;
        }
        private void FillJournalState(UpdatePosting _posting, CreditMemoCompact memo)
        {
            _posting.Id = memo.Id;
            _posting.CompanyId = memo.CompanyId;
            _posting.DocumentState = memo.DocumentState;
            _posting.ModifiedDate = memo.ModifiedDate;
            _posting.ModifiedBy = memo.ModifiedBy;
            _posting.BalanceAmount = memo.BalanceAmount;
        }

        public void FillWokflowInvoice(Invoice invoice)
        {
            InvoiceVM invoicevm = new InvoiceVM();
            invoicevm.Id = invoice.DocumentId.Value;
            //invoicevm.Id = invoice.Id;
            invoicevm.CompanyId = invoice.CompanyId;
            invoicevm.TotalFee = invoice.GrandTotal;
            invoicevm.BalanceFee = invoice.BalanceAmount;
            string state = invoice.DocumentState;
            invoicevm.InvoiceState = state == "Partial Paid" ? "Partially paid" : invoice.DocumentState;
            invoicevm.ModifiedBy = invoice.ModifiedBy;
            invoicevm.ModifiedDate = DateTime.UtcNow;
            invoicevm.Status = RecordStatusEnum.Active;
            WorkflowInvoicePosting(invoicevm);
        }
        public void FillWFInvoice(Invoice invoice, string ConnectionString)
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

        #endregion

        #region Kendo Grid Call
        public IQueryable<ReceiptModelK> GetAllReceiptsK(string username, long companyId)
        {
            try
            {
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptApplicationService, ReceiptLoggingValidation.Log_Receipts_GetReceiptsApplicationK_Request_Message);
                return _receiptService.GetAllReceiptsK(username, companyId);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ReceiptLoggingValidation.ReceiptApplicationService, ex, ex.Message);
                LoggingHelper.LogMessage(ReceiptLoggingValidation.ReceiptService, ReceiptLoggingValidation.Log_Receipts_GetReceiptsAllicationK_Exeception_Message);
                throw;
            }
        }
        #endregion

        #region Jv Posting Calls
        private void FillJournal(JVModel headJournal, Receipt invoice, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<ReceiptDetail> lstDetail)
        {
            if (isfirst1)
                doc = invoice.SystemRefNo;
            //else
            //    doc = doc;
            isFirst = true;
            //string strServiceCompany = _companyService.GetById(invoice.ServiceCompanyId).ShortName;

            //invoice.ExchangeRate = invoice.SystemCalculatedExchangeRate != null ? invoice.SystemCalculatedExchangeRate : invoice.ExchangeRate;
            //TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice);
            //JournalModel headJournal = new JournalModel();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillJV(headJournal, invoice, isBalancing);
            doc = GetNextApplicationNumber(doc, isfirst1, invoice.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == true)
            {
                JVVDetailModel Jmodel = new JVVDetailModel();
                FillJDetail(Jmodel, invoice, "BankReceiptAmmount", headJournal.ExchangeRate);
                Jmodel.RecOrder = recorder + 1;
                recorder = Jmodel.RecOrder;
                lstJD.Add(Jmodel);
                Jmodel = new JVVDetailModel();
                if (invoice.BankCharges != null)
                {
                    FillJDetail(Jmodel, invoice, "BankCharges", headJournal.ExchangeRate);
                    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = Jmodel.RecOrder;
                    lstJD.Add(Jmodel);
                }
                Jmodel = new JVVDetailModel();
                if (invoice.ExcessPaidByClientAmmount != null)
                {
                    FillJDetail(Jmodel, invoice, "ExcesPaidByClient", headJournal.ExchangeRate);
                    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = Jmodel.RecOrder;
                    lstJD.Add(Jmodel);
                }


                //Jmodel = new JVVDetailModel();
                //FillJDetail(Jmodel, invoice, "BankChargesTax");
                //Jmodel.RecOrder = recorder + 1;
                //recorder = Jmodel.RecOrder;
                //lstJD.Add(Jmodel);

                foreach (ReceiptBalancingItem rBI in invoice.ReceiptBalancingItems)
                {
                    JVVDetailModel jmodel1 = new JVVDetailModel();
                    FillBalancingitems(jmodel1, rBI, invoice, headJournal.ExchangeRate);
                    if (isNew)
                        jmodel1.Id = Guid.NewGuid();
                    else
                        jmodel1.Id = rBI.Id;
                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = jmodel1.RecOrder;
                    lstJD.Add(jmodel1);
                }
                if (invoice.IsGstSettings)
                {
                    foreach (ReceiptBalancingItem rBI in invoice.ReceiptBalancingItems.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                    {
                        JVVDetailModel jmodel1 = new JVVDetailModel();
                        FillBalancingGstitems(jmodel1, rBI, invoice, headJournal.ExchangeRate);
                        if (isNew)
                            jmodel1.Id = Guid.NewGuid();
                        else
                            jmodel1.Id = rBI.Id;
                        jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        // recorder = jmodel1.RecOrder;
                        lstJD.Add(jmodel1);
                    }
                }
                recorder1 = recorder;
                if (invoice.BankChargesCurrency != invoice.DocCurrency)
                {
                    JVVDetailModel jmodel1 = new JVVDetailModel();
                    FillClearingRecipt(jmodel1, invoice, true, headJournal.ExchangeRate);
                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    // recorder = jmodel1.RecOrder;
                    lstJD.Add(jmodel1);
                }
                if (invoice.BankChargesCurrency == invoice.DocCurrency)
                {
                    foreach (ReceiptDetail detail in invoice.ReceiptDetails.Where(a => a.ReceiptAmount != 0))
                    {
                        if (detail.ReceiptAmount != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            if (isNew)
                                jmodel.Id = Guid.NewGuid();
                            else
                                jmodel.Id = detail.Id;
                            FillDetail(jmodel, detail, invoice, lstDetail);
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            //recorder = jmodel.RecOrder;
                            lstJD.Add(jmodel);
                            if (invoice.DocCurrency != invoice.BaseCurrency && invoice.ServiceCompanyId == detail.ServiceCompanyId)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, invoice, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }
                        }
                    }
                }
            }
            if (isBalancing == false)
            {
                recorder = 0;
                JVVDetailModel jmodel3 = new JVVDetailModel();
                jmodel3 = new JVVDetailModel();
                FillClearingRecipt(jmodel3, invoice, false, headJournal.ExchangeRate);
                jmodel3.RecOrder = recorder + 1;
                recorder = jmodel3.RecOrder;
                lstJD.Add(jmodel3);
                foreach (ReceiptDetail detail in invoice.ReceiptDetails.Where(a => a.ReceiptAmount > 0))
                {
                    if (detail.ReceiptAmount != 0)
                    {
                        JVVDetailModel jmodel = new JVVDetailModel();
                        if (isNew)
                            jmodel.Id = Guid.NewGuid();
                        else
                            jmodel.Id = detail.Id;
                        FillDetail(jmodel, detail, invoice, lstDetail);
                        jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //recorder = jmodel.RecOrder;
                        lstJD.Add(jmodel);
                        if (invoice.DocCurrency != invoice.BaseCurrency && invoice.ServiceCompanyId == detail.ServiceCompanyId)
                        {
                            JVVDetailModel jmodel2 = new JVVDetailModel();
                            if (isNew)
                                jmodel2.Id = Guid.NewGuid();
                            else
                                jmodel2.Id = detail.Id;
                            FillGstDetail(jmodel2, detail, invoice, jmodel);
                            // jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            jmodel2.RecOrder = recorder - 1;
                            recorder = jmodel2.RecOrder;
                            if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                lstJD.Add(jmodel2);
                        }
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }

        private void FillJournalOffset(JVModel headJournal, Receipt invoice, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<ReceiptDetail> lstDetail, bool? isBankDocDiff, long clearingCOAId)
        {
            if (isfirst1)
                doc = invoice.SystemRefNo;
            isFirst = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillJV(headJournal, invoice, isBalancing);
            doc = GetNextApplicationNumber(doc, isfirst1, invoice.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == false)
            {
                recorder = 0;
                JVVDetailModel jmodel3 = new JVVDetailModel();
                decimal? amt = invoice.ReceiptDetails.Where(c => c.ServiceCompanyId == invoice.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.ReceiptAmount) - invoice.ReceiptDetails.Where(c => c.ServiceCompanyId == invoice.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
                FillClearingReciptOffset2(jmodel3, invoice, amt, headJournal.ExchangeRate, clearingCOAId);
                jmodel3.RecOrder = ++recorder;
                lstJD.Add(jmodel3);
                foreach (ReceiptDetail detail in invoice.ReceiptDetails.Where(a => a.ReceiptAmount != 0 && (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.Bills || a.DocumentType == DocTypeConstants.DebitNote)))
                {
                    JVVDetailModel jmodel = new JVVDetailModel();
                    if (isNew)
                        jmodel.Id = Guid.NewGuid();
                    else
                        jmodel.Id = detail.Id;
                    FillDetailOffset1(jmodel, detail, invoice, lstDetail, detail.ReceiptAmount);
                    jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(jmodel);
                    if (invoice.DocCurrency != invoice.BaseCurrency)
                    {
                        JVVDetailModel jmodel2 = new JVVDetailModel();
                        if (isNew)
                            jmodel2.Id = Guid.NewGuid();
                        else
                            jmodel2.Id = detail.Id;
                        FillGstDetail(jmodel2, detail, invoice, jmodel);
                        jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                            lstJD.Add(jmodel2);
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }

        private void FillClearingRecipt(JVVDetailModel jmodel1, Receipt invoice, bool isBank, decimal? exchangeRate)
        {
            jmodel1.DocType = invoice.DocSubType;
            jmodel1.DocumentDetailId = invoice.Id;
            jmodel1.DocumentId = invoice.Id;
            jmodel1.DocType = DocTypeConstants.Receipt;
            jmodel1.DocSubType = DocTypeConstants.General;
            jmodel1.SystemRefNo = invoice.SystemRefNo;
            jmodel1.DocNo = invoice.DocNo;
            jmodel1.PostingDate = invoice.DocDate;
            jmodel1.EntityId = invoice.EntityId;
            AppsWorld.ReceiptModule.Entities.BeanEntity entity = _beanEntityService.Query(a => a.Id == invoice.EntityId).Select().FirstOrDefault();
            jmodel1.EntityName = entity.Name;
            jmodel1.EntityType = invoice.EntityType;
            jmodel1.BaseCurrency = invoice.ExCurrency;
            jmodel1.DocCurrency = invoice.BankChargesCurrency;
            jmodel1.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel1.AccountDescription = invoice.Remarks;
            jmodel1.DocDate = invoice.DocDate;
            if (invoice.DocCurrency != invoice.BaseCurrency || isBank == true)
                jmodel1.ExchangeRate = exchangeRate;
            //invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            else
                jmodel1.ExchangeRate = 1;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chart = _chartOfAccountService.GetByName(COANameConstants.Clearing_Receipts, invoice.CompanyId);
            if (chart != null)
            {
                jmodel1.COAId = chart.Id;
                jmodel1.AccountName = chart.Name;
            }
            if (isBank == true)
            {
                jmodel1.DocCredit = invoice.GrandTotal;
                jmodel1.BaseCredit = Math.Round((decimal)jmodel1.DocCredit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            }
            if (isBank == false)
            {
                jmodel1.DocCurrency = invoice.DocCurrency;
                jmodel1.DocDebit = invoice.ReceiptApplicationAmmount.Value;
                jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            }
            jmodel1.IsTax = false;
        }

        private void FillClearingReciptOffset2(JVVDetailModel jmodel1, Receipt invoice, decimal? amt, decimal? exchangeRate, long clearingCOAId)
        {
            jmodel1.DocType = invoice.DocSubType;
            jmodel1.DocumentDetailId = invoice.Id;
            jmodel1.DocumentId = invoice.Id;
            jmodel1.DocType = DocTypeConstants.Receipt;
            jmodel1.DocSubType = DocTypeConstants.General;
            jmodel1.SystemRefNo = invoice.SystemRefNo;
            jmodel1.DocNo = invoice.DocNo;
            jmodel1.PostingDate = invoice.DocDate;
            jmodel1.EntityId = invoice.EntityId;
            jmodel1.EntityType = invoice.EntityType;
            jmodel1.BaseCurrency = invoice.ExCurrency;
            jmodel1.DocCurrency = invoice.BankChargesCurrency;
            jmodel1.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel1.AccountDescription = invoice.Remarks;
            jmodel1.DocDate = invoice.DocDate;
            if (invoice.DocCurrency != invoice.BaseCurrency)
                jmodel1.ExchangeRate = exchangeRate;
            else
                jmodel1.ExchangeRate = 1;
            jmodel1.COAId = clearingCOAId;
            jmodel1.DocCurrency = invoice.DocCurrency;
            jmodel1.DocDebit = amt;
            jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            jmodel1.IsTax = false;
        }
        private void FillGstDetail(JVVDetailModel jmodel2, ReceiptDetail detail, Receipt invoice, JVVDetailModel jmodel)
        {
            jmodel2.DocType = DocTypeConstants.Receipt;
            jmodel2.DocSubType = DocTypeConstants.General;
            jmodel2.AccountDescription = invoice.Remarks;
            jmodel2.SystemRefNo = invoice.SystemRefNo;
            // jmodel2.DocType = detail.DocumentType;
            jmodel2.DocumentDetailId = detail.Id;
            jmodel2.DocumentId = detail.ReceiptId;
            jmodel2.PostingDate = invoice.DocDate;
            jmodel2.DocNo = invoice.DocNo;
            //jmodel2.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel2.DocDate = invoice.DocDate;
            jmodel2.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account2 = _chartOfAccountService.GetByName(COANameConstants.ExchangeGainLossRealised, invoice.CompanyId);
            if (account2 != null)
            {
                jmodel2.COAId = account2.Id;
                jmodel2.AllowDisAllow = account2.DisAllowable;
            }
            jmodel2.EntityId = invoice.EntityId;
            jmodel2.BaseCurrency = invoice.BaseCurrency;
            jmodel2.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            jmodel2.OffsetDocument = detail.SystemReferenceNumber;
            jmodel2.ServiceCompanyId = invoice.ServiceCompanyId;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote)
            {
                if (jmodel2.ExchangeRate > jmodel.ExchangeRate)
                {
                    jmodel2.BaseCredit = Math.Round((decimal)(((jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate) - (jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate)) * detail.ReceiptAmount), 2, MidpointRounding.AwayFromZero);
                }
                if (jmodel2.ExchangeRate < jmodel.ExchangeRate)
                {
                    jmodel2.BaseDebit = Math.Round((decimal)(((jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate) - (jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate))) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                if (jmodel2.ExchangeRate > jmodel.ExchangeRate)
                {
                    jmodel2.BaseDebit = Math.Round((decimal)(((jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate) - (jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate)) * detail.ReceiptAmount), 2, MidpointRounding.AwayFromZero);
                }
                if (jmodel2.ExchangeRate < jmodel.ExchangeRate)
                {
                    jmodel2.BaseCredit = Math.Round((decimal)(((jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate) - (jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate))) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
        }
        private void FillDetail(JVVDetailModel jmodel, ReceiptDetail detail, Receipt invoice, List<ReceiptDetail> lstDetail)
        {
            ReceiptDetail detailM = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
            string shotCode = string.Empty;
            if (detailM != null)
            {
                shotCode = _companyService.Query(a => a.Id == detailM.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                shotCode = "I/C" + " - " + shotCode;
            }
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == detail.ServiceCompanyId*/).Select().FirstOrDefault();
            if (invoice.ServiceCompanyId == detail.ServiceCompanyId)
            {
                if (account1 != null)
                    jmodel.COAId = account1.Id;
                jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            }
            else
            {
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
                if (detail.ServiceCompanyId != null)
                    jmodel.ServiceCompanyId = detail.ServiceCompanyId.Value;
            }

            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                var inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
                //jmodel.SystemReferenceNo = inv.InvoiceNumber;
                //jmodel.DocDate = inv.DocDate;
                //jmodel.DocNo = inv.DocNo;
                //jmodel.DocType = inv.DocType;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                var inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
                //jmodel.SystemReferenceNo = inv.DebitNoteNumber;
                //jmodel.DocDate = inv.DocDate;
                //jmodel.DocNo = inv.DocNo;
                //jmodel.DocType = inv.DocSubType;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            jmodel.DocDate = invoice.DocDate;
            jmodel.DocNo = invoice.DocNo;
            jmodel.PostingDate = invoice.DocDate;
            //jmodel.DocType = detail.DocumentType;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;

            if (detail.DocumentType != DocTypeConstants.Bills)
            {
                jmodel.DocCredit = detail.ReceiptAmount;
                if ((invoice.DocCurrency == invoice.BaseCurrency) || (invoice.DocCurrency != invoice.BaseCurrency && invoice.ServiceCompanyId == detail.ServiceCompanyId))
                    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                else
                {
                    jmodel.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
                    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                }
            }

            else
            {
                jmodel.DocDebit = detail.ReceiptAmount;
                if ((invoice.DocCurrency == invoice.BaseCurrency) || (invoice.DocCurrency != invoice.BaseCurrency && invoice.ServiceCompanyId == detail.ServiceCompanyId))
                    jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                else
                {
                    jmodel.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
                    jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                }
            }
        }

        private void FillDetailOffset(JVVDetailModel jmodel, ReceiptDetail detail, Receipt invoice, List<ReceiptDetail> lstDetail, decimal? receiptAmount, bool? isBankDocDiff, long clearingCOAId, bool? isCN)
        {
            ReceiptDetail detailM = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
            string shotCode = string.Empty;
            if (detailM != null)
            {
                shotCode = _companyService.Query(a => a.Id == detailM.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                shotCode = "I/C" + " - " + shotCode;
            }
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == detail.ServiceCompanyId*/).Select().FirstOrDefault();
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                var inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                var inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (invoice.ServiceCompanyId == detail.ServiceCompanyId)
            {
                if (account1 != null)
                    jmodel.COAId = (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.BillCreditMemo) ? clearingCOAId : isBankDocDiff == true ? clearingCOAId : account1.Id;
                jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
                jmodel.ExchangeRate = invoice.BaseCurrency == invoice.BankChargesCurrency ? 1 : jmodel.ExchangeRate;
            }
            else
            {
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
                if (detail.ServiceCompanyId != null)
                    jmodel.ServiceCompanyId = detail.ServiceCompanyId.Value;
                jmodel.ExchangeRate = invoice.BaseCurrency == invoice.BankChargesCurrency ? 1 : invoice.ExchangeRate;
            }

            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;

            jmodel.DocDate = invoice.DocDate;
            jmodel.DocNo = invoice.DocNo;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;

            //Pradhan commented

            //jmodel.DocCredit = receiptAmount;
            //if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
            //{
            //    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            //}
            //else
            //{
            //    //if ((invoice.DocCurrency == invoice.BaseCurrency) || (invoice.DocCurrency != invoice.BaseCurrency && invoice.ServiceCompanyId == detail.ServiceCompanyId))
            //    //    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            //    //else
            //    //{
            //    //jmodel.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            //    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            //    //  }
            //}

            //if (invoice.DocCurrency != invoice.BaseCurrency && invoice.BaseCurrency == invoice.BankChargesCurrency)
            //{
            //    jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            //    jmodel.DocDebit = jmodel.BaseDebit;
            //}
            if (invoice.DocCurrency == invoice.BaseCurrency && invoice.DocCurrency != invoice.BankChargesCurrency)
            {
                jmodel.BaseCredit = receiptAmount;
                jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                jmodel.DocCredit = receiptAmount;
                jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (invoice.BaseCurrency == invoice.BankChargesCurrency && invoice.BankChargesCurrency != invoice.DocCurrency)
            {
                jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.DocCredit = jmodel.BaseCredit;
            }

            if (isCN == true)
            {
                jmodel.DocDebit = jmodel.DocCredit;
                jmodel.BaseDebit = jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
            if (receiptAmount < 0)
            {
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }

        private void FillDetailOffset2(JVVDetailModel jmodel, ReceiptDetail detail, Receipt invoice, List<ReceiptDetail> lstDetail, decimal? receiptAmount, long clearingCOAId)
        {
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                var inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                var inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (account1 != null)
                jmodel.COAId = detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.CreditNote ? clearingCOAId : account1.Id;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel.ExchangeRate = invoice.BaseCurrency == invoice.BankChargesCurrency ? 1 : jmodel.ExchangeRate;
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.DocDate = invoice.DocDate;
            jmodel.DocNo = invoice.DocNo;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
            {
                jmodel.DocCredit = receiptAmount;
                jmodel.BaseCredit = detail.DocumentType == DocTypeConstants.BillCreditMemo ? Math.Round((decimal)jmodel.DocCredit * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills)
            {
                jmodel.DocDebit = receiptAmount;
                jmodel.BaseDebit = detail.DocumentType == DocTypeConstants.Bills ? Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (receiptAmount < 0)
            {
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }
        private void FillDetailOffset1(JVVDetailModel jmodel, ReceiptDetail detail, Receipt invoice, List<ReceiptDetail> lstDetail, decimal? receiptAmount)
        {
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                var inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                var inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (account1 != null)
                jmodel.COAId = account1.Id;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel.ExchangeRate = invoice.BaseCurrency == invoice.DocCurrency ? 1 : jmodel.ExchangeRate;
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.DocDate = invoice.DocDate;
            jmodel.DocNo = invoice.DocNo;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;

            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.DocCredit = receiptAmount;
                jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(invoice.DocCurrency == invoice.BaseCurrency ? 1 : jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                jmodel.DocDebit = receiptAmount;
                jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)(invoice.DocCurrency == invoice.BaseCurrency ? 1 : jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
        }

        private void FillDetailOffsetDetailNew(JVVDetailModel jmodel, string docType, Receipt _receipt, List<ReceiptDetail> lstDetail, decimal? receiptAmount, bool? isBankDocDiff, long clearingCOAId)
        {
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = _receipt.Remarks;
            jmodel.SystemRefNo = _receipt.SystemRefNo;
            jmodel.DocumentId = _receipt.Id;
            jmodel.COAId = clearingCOAId;
            jmodel.ServiceCompanyId = _receipt.ServiceCompanyId;
            jmodel.ExchangeRate = _receipt.BaseCurrency == _receipt.BankChargesCurrency ? 1 : (_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate);
            jmodel.EntityId = _receipt.EntityId;
            jmodel.BaseCurrency = _receipt.BaseCurrency;
            jmodel.DocDate = _receipt.DocDate;
            jmodel.DocNo = _receipt.DocNo;
            jmodel.PostingDate = _receipt.DocDate;
            if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.BillCreditMemo)
            {
                if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                {
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = jmodel.BaseCredit;

                }
                if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.DocCurrency != _receipt.BankChargesCurrency)
                {
                    jmodel.BaseCredit = receiptAmount;
                    jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? jmodel.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.DocCredit = Math.Round((decimal)receiptAmount * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                //jmodel.DocCredit = receiptAmount;
                //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else
            {

                if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                {
                    jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? jmodel.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = jmodel.BaseDebit;
                }
                if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.DocCurrency != _receipt.BankChargesCurrency)
                {
                    jmodel.BaseDebit = receiptAmount;
                    jmodel.DocDebit = Math.Round((decimal)receiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? jmodel.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                //jmodel.DocDebit = receiptAmount;
                //jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (receiptAmount < 0)
            {
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
            }
        }

        private void FillDetailOffsetDetail(JVVDetailModel jmodel, string docType, Receipt invoice, List<ReceiptDetail> lstDetail, decimal? receiptAmount, bool? isBankDocDiff, long clearingCOAId)
        {

            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentId = invoice.Id;
            jmodel.COAId = clearingCOAId;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel.ExchangeRate = invoice.BaseCurrency == invoice.BankChargesCurrency ? 1 : (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate);
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.DocDate = invoice.DocDate;
            jmodel.DocNo = invoice.DocNo;
            jmodel.PostingDate = invoice.DocDate;
            if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.BillCreditMemo)
            {
                jmodel.DocCredit = receiptAmount;
                jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                jmodel.DocDebit = receiptAmount;
                jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (receiptAmount < 0)
            {
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
            }
        }

        private void FillJV(JVModel headJournal, Receipt invoice, bool isBalancing)
        {
            headJournal.DocumentId = invoice.Id;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = invoice.DocDate;
            headJournal.DocNo = invoice.DocNo;
            headJournal.DocType = DocTypeConstants.Receipt;
            //headJournal.DocSubType = invoice.DocSubType;
            headJournal.DocSubType = DocTypeConstants.General;
            headJournal.DocDate = invoice.DocDate;
            headJournal.DueDate = invoice.DueDate;
            headJournal.DocumentState = invoice.DocumentState;
            //headJournal.SystemReferenceNo = invoice.SystemRefNo;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.ExDurationFrom = invoice.ExDurationFrom;
            headJournal.ExDurationTo = invoice.ExDurationTo;
            headJournal.GSTExDurationFrom = invoice.GSTExDurationFrom;
            headJournal.GSTExDurationTo = invoice.GSTExDurationTo;
            headJournal.NoSupportingDocument = invoice.NoSupportingDocs;
            headJournal.IsNoSupportingDocs = invoice.IsNoSupportingDocument;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            AppsWorld.ReceiptModule.Entities.BeanEntity entity = _beanEntityService.Query(a => a.Id == invoice.EntityId).Select().FirstOrDefault();
            if (entity != null)
            {
                headJournal.EntityName = entity.Name;
                headJournal.Nature = entity.CustNature;
            }
            headJournal.EntityType = invoice.EntityType;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account = _chartOfAccountService.GetChartOfAccountById(invoice.COAId);
            headJournal.COAId = account.Id;
            headJournal.AccountCode = account.Code;
            headJournal.AccountName = account.Name;
            headJournal.BankClearingDate = invoice.BankClearingDate;
            if (isBalancing)
            {
                headJournal.DocCurrency = invoice.BankReceiptAmmountCurrency;
                //headJournal.IsAllowableNonAllowable = true;
            }
            else
            {
                headJournal.DocCurrency = invoice.DocCurrency;
                headJournal.IsBalancing = false;
            }
            headJournal.IsAllowableNonAllowable = invoice.IsAllowableDisallowable;
            headJournal.GrandDocDebitTotal = invoice.GrandTotal != 0 ? invoice.GrandTotal : invoice.BankReceiptAmmount;
            headJournal.BaseCurrency = invoice.BaseCurrency;
            headJournal.ExchangeRate = headJournal.DocCurrency == headJournal.BaseCurrency ? 1 : invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            // headJournal.ExchangeRate = invoice.SystemCalculatedExchangeRate;
            headJournal.GrandBaseDebitTotal = invoice.GrandTotal != 0 ? Math.Round((decimal)(invoice.GrandTotal * (invoice.ExchangeRate == null ? 1 : invoice.ExchangeRate)), 2, MidpointRounding.AwayFromZero) : invoice.BankReceiptAmmount;
            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.ModeOfReceipt = invoice.ModeOfReceipt;
            headJournal.Remarks = invoice.Remarks;
            headJournal.DocumentDescription = invoice.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = invoice.CreatedDate;
            headJournal.ModifiedBy = invoice.ModifiedBy;
            headJournal.ModifiedDate = invoice.ModifiedDate;
            headJournal.ActualSysRefNo = invoice.SystemRefNo;
            headJournal.TransferRefNo = invoice.ReceiptRefNo;//added by lokanath
        }

        private void FillBalancingGstitems(JVVDetailModel jmodel1, ReceiptBalancingItem rBI, Receipt invoice, decimal? exchangeRate)
        {

            jmodel1.DocumentId = invoice.Id;
            jmodel1.DocumentDetailId = rBI.Id;
            jmodel1.SystemRefNo = invoice.SystemRefNo;
            jmodel1.DocType = DocTypeConstants.Receipt;
            jmodel1.DocSubType = DocTypeConstants.General;
            jmodel1.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel1.DocNo = invoice.DocNo;
            jmodel1.DocDate = invoice.DocDate;
            jmodel1.EntityId = invoice.EntityId;
            jmodel1.AccountDescription = invoice.Remarks;
            //jmodel1.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            jmodel1.ExchangeRate = exchangeRate;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount coa = _chartOfAccountService.GetByName(COANameConstants.TaxPayableGST, invoice.CompanyId);
            if (coa != null)
                jmodel1.COAId = coa.Id;
            if (rBI.TaxId != null)
            {
                //jmodel1.TaxId = rBI.TaxId;

                //AppsWorld.ReceiptModule.Entities.TaxCode tax = _taxCodeService.GetTaxCode(rBI.TaxId.Value);
                //if (tax != null)
                //{
                if (rBI.ReciveORPay == "Pay(Dr)")
                {
                    jmodel1.DocDebit = Math.Round(((decimal)(rBI.DocTaxAmount == null ? null : (decimal?)rBI.DocTaxAmount.Value)), 2);
                    jmodel1.BaseDebit = jmodel1.DocDebit != null ? ((decimal?)Math.Round((decimal)jmodel1.DocDebit * (jmodel1.ExchangeRate == null ? 0 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero)) : null;

                }
                else
                {
                    jmodel1.DocCredit = Math.Round(((decimal)(rBI.DocTaxAmount == null ? null : (decimal?)rBI.DocTaxAmount.Value)), 2);
                    jmodel1.BaseCredit = jmodel1.DocCredit != null ? ((decimal?)Math.Round((decimal)jmodel1.DocCredit * (jmodel1.ExchangeRate == null ? 0 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero)) : null;
                }
                // }
            }
            if (rBI.TaxId != null)
            {
                //AppsWorld.CommonModule.Entities.TaxCode tax = _taxCodeService.GetTaxCode(rBI.TaxId.Value);
                //jmodel1.TaxId = tax.Id;
                //jmodel1.TaxCode = tax.Code;
                //jmodel1.TaxRate = tax.TaxRate;
                //jmodel1.TaxType = tax.TaxType;
                jmodel1.TaxId = rBI.TaxId;
                jmodel1.TaxRate = rBI.TaxRate;
            }
            jmodel1.DocCurrency = invoice.DocCurrency;
            jmodel1.BaseCurrency = invoice.BaseCurrency;
            jmodel1.GSTExCurrency = invoice.GSTExCurrency;
            jmodel1.PostingDate = invoice.DocDate;
            jmodel1.GSTExchangeRate = invoice.GSTExchangeRate;
            jmodel1.IsTax = true;
        }

        private void FillBalancingitems(JVVDetailModel jmodel1, ReceiptBalancingItem rBI, Receipt invoice, decimal? exchangeRate)
        {
            jmodel1.DocumentId = invoice.Id;
            jmodel1.DocumentDetailId = rBI.Id;
            jmodel1.SystemRefNo = invoice.SystemRefNo;
            jmodel1.DocType = DocTypeConstants.Receipt;
            jmodel1.DocSubType = DocTypeConstants.General;
            jmodel1.ServiceCompanyId = invoice.ServiceCompanyId;
            jmodel1.DocNo = invoice.DocNo;
            jmodel1.DocDate = invoice.DocDate;
            jmodel1.EntityId = invoice.EntityId;
            jmodel1.COAId = rBI.COAId;
            jmodel1.AllowDisAllow = rBI.IsDisAllow;
            jmodel1.TaxId = rBI.TaxId;
            jmodel1.AccountDescription = invoice.Remarks;
            //jmodel1.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            jmodel1.ExchangeRate = exchangeRate;
            //if (jmodel1.TaxId != null)
            //{
            //AppsWorld.ReceiptModule.Entities.TaxCode tax = _taxCodeService.GetTaxCode(rBI.TaxId.Value);
            //if (tax != null)
            //{
            if (rBI.ReciveORPay == "Pay(Dr)")
            {
                jmodel1.DocDebit = rBI.DocAmount;
                jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                jmodel1.DocTaxDebit = rBI.DocTaxAmount;
                jmodel1.BaseTaxDebit = Math.Round((decimal)jmodel1.DocTaxDebit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                jmodel1.GSTDebit = Math.Round((decimal)jmodel1.DocTaxDebit * (invoice.GSTExchangeRate == null ? 1 : invoice.GSTExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                jmodel1.GSTTaxDebit = Math.Round((decimal)jmodel1.DocDebit * (invoice.GSTExchangeRate == null ? 1 : invoice.GSTExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                //jmodel1.DocTaxDebit = Math.Round((decimal)((jmodel1.DocDebit) * Convert.ToDecimal(rBI.TaxRate / 100)), 2, MidpointRounding.AwayFromZero);
                //jmodel1.BaseTaxDebit = Math.Round((decimal)((jmodel1.DocTaxDebit) * Convert.ToDecimal(jmodel1.ExchangeRate.Value)), 2, MidpointRounding.AwayFromZero);

            }
            else
            {
                jmodel1.DocCredit = rBI.DocAmount;
                jmodel1.BaseCredit = Math.Round((decimal)jmodel1.DocCredit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                jmodel1.DocTaxCredit = rBI.DocTaxAmount;
                jmodel1.BaseTaxCredit = Math.Round((decimal)jmodel1.DocTaxCredit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                jmodel1.GSTCredit = Math.Round((decimal)jmodel1.DocTaxCredit * (invoice.GSTExchangeRate == null ? 1 : invoice.GSTExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                jmodel1.GSTTaxCredit = Math.Round((decimal)jmodel1.DocCredit * (invoice.GSTExchangeRate == null ? 1 : invoice.GSTExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
                //jmodel1.DocTaxCredit = Math.Round((decimal)((jmodel1.DocCredit) * Convert.ToDecimal(rBI.TaxRate / 100)), 2, MidpointRounding.AwayFromZero);
                //jmodel1.BaseTaxCredit = Math.Round((decimal)((jmodel1.DocTaxCredit) * Convert.ToDecimal(jmodel1.ExchangeRate.Value)), 2, MidpointRounding.AwayFromZero);

            }
            //    }
            //}
            if (rBI.TaxId != null)
            {
                //AppsWorld.CommonModule.Entities.TaxCode tax = _taxCodeService.GetTaxCode(rBI.TaxId.Value);
                //jmodel1.TaxId = tax.Id;
                //jmodel1.TaxCode = tax.Code;
                //jmodel1.TaxRate = tax.TaxRate;
                //jmodel1.TaxType = tax.TaxType;
                jmodel1.TaxId = rBI.TaxId;
                jmodel1.TaxRate = rBI.TaxRate;
            }
            jmodel1.DocCurrency = invoice.DocCurrency;
            jmodel1.BaseCurrency = invoice.BaseCurrency;
            jmodel1.GSTExCurrency = invoice.GSTExCurrency;
            jmodel1.GSTExchangeRate = invoice.GSTExchangeRate;
            jmodel1.PostingDate = invoice.DocDate;
        }
        private void FillJDetail(JVVDetailModel Jmodel, Receipt invoice, string type, decimal? exchangeRate)
        {

            Jmodel.DocumentId = invoice.Id;
            Jmodel.DocNo = invoice.DocNo;
            Jmodel.DocumentDetailId = type == "BankReceiptAmmount" ? new Guid() : invoice.Id;
            Jmodel.DocType = DocTypeConstants.Receipt;
            //Jmodel.DocSubType = invoice.DocSubType;
            Jmodel.DocSubType = DocTypeConstants.General;
            Jmodel.DocDate = invoice.DocDate;
            Jmodel.SystemRefNo = invoice.SystemRefNo;
            Jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            Jmodel.ExDurationFrom = invoice.ExDurationFrom;
            Jmodel.ExDurationTo = invoice.ExDurationTo;
            Jmodel.AccountDescription = invoice.Remarks;
            //Jmodel = invoice.IsNoSupportingDocument;
            //Jmodel.EntityId = invoice.EntityId;
            AppsWorld.ReceiptModule.Entities.BeanEntity entity1 = _beanEntityService.Query(a => a.Id == invoice.EntityId).Select().FirstOrDefault();
            Jmodel.EntityId = entity1.Id;
            Jmodel.EntityName = entity1.Name;
            Jmodel.EntityType = invoice.EntityType;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = _chartOfAccountService.GetChartOfAccountById(invoice.COAId);
            Jmodel.COAId = account1.Id;
            Jmodel.AccountName = account1.Name;
            Jmodel.AllowDisAllow = invoice.IsAllowableDisallowable;
            Jmodel.BaseCurrency = invoice.ExCurrency;
            Jmodel.PostingDate = invoice.DocDate;
            //Jmodel.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            Jmodel.ExchangeRate = exchangeRate;
            if (invoice.IsGstSettings)
            {
                Jmodel.GSTExCurrency = invoice.GSTExCurrency;
                Jmodel.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            Jmodel.ReceiptType = type;
            Jmodel.DocCurrency = invoice.DocCurrency;
            if (type == "BankReceiptAmmount")
            {
                Jmodel.DocDebit = invoice.BankReceiptAmmount;
                Jmodel.BaseDebit = Math.Round((decimal)Jmodel.DocDebit * (Jmodel.ExchangeRate == null ? 1 : Jmodel.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            }
            else if (type == "BankCharges")
            {
                Jmodel.TaxId = _taxCodeService.Query(c => c.Code == "EP").Select(c => c.Id).FirstOrDefault();
                Jmodel.DocDebit = invoice.BankCharges == null ? null : (decimal?)invoice.BankCharges.Value;
                Jmodel.BaseDebit = Jmodel.DocDebit != null ? ((decimal?)Math.Round((decimal)Jmodel.DocDebit * (Jmodel.ExchangeRate == null ? 1 : Jmodel.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero)) : null;
                //Jmodel.TaxId = _taxCodeService.Query(c => c.Code == "EP").Select(c => c.Id).FirstOrDefault();
                AppsWorld.ReceiptModule.Entities.ChartOfAccount account = _chartOfAccountService.GetByName(COANameConstants.BankCharges, invoice.CompanyId);
                if (account != null)
                {
                    Jmodel.COAId = account.Id;
                    Jmodel.AccountName = account.Name;
                }
            }
            //else if (type == "BankChargesTax")
            //{
            //    Jmodel.TaxId = _taxCodeService.Query(c => c.Code == "EP").Select(c => c.Id).FirstOrDefault();
            //    AppsWorld.ReceiptModule.Entities.ChartOfAccount account = _chartOfAccountService.GetByName(COANameConstants.TaxPayableGST, invoice.CompanyId);
            //    Jmodel.COAId = account.Id;
            //}
            else if (type == "ExcesPaidByClient")
            {
                Jmodel.DocCredit = invoice.ExcessPaidByClientAmmount == null ? null : (decimal?)invoice.ExcessPaidByClientAmmount.Value;
                Jmodel.BaseCredit = Jmodel.DocCredit != null ? ((decimal?)Math.Round((decimal)Jmodel.DocCredit * (Jmodel.ExchangeRate == null ? 1 : Jmodel.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero)) : null;
                if (invoice.ExcessPaidByClient != null)
                {

                    if (invoice.ExcessPaidByClient.StartsWith("(TR)") || invoice.ExcessPaidByClient.StartsWith("(OR)"))
                    {
                        //Jmodel.Nature = invoice.ExcessPaidByClient.StartsWith("(AR)") ? "Trade" : "Others";
                        AppsWorld.ReceiptModule.Entities.ChartOfAccount account =
                            _chartOfAccountService.GetByName(ReceiptConstant.Clearing_Receipts, invoice.CompanyId);
                        if (account != null)
                        {
                            Jmodel.COAId = account.Id;
                            Jmodel.AccountName = account.Name;
                        }
                    }
                }
            }
            if (Jmodel.TaxId != null)
            {
                AppsWorld.CommonModule.Entities.TaxCode tax = _taxCodeService.GetTaxCode(Jmodel.TaxId.Value);
                Jmodel.TaxId = tax.Id;
                Jmodel.TaxCode = tax.Code;
                Jmodel.TaxRate = tax.TaxRate;
                Jmodel.TaxType = tax.TaxType;
            }
        }
        public void SaveReceiptJv(JVModel clientModel)
        {
            //Log.Logger.ZInfo(clientModel);
            Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_SaveReceiptJv_Method);
            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == ReceiptConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                Log.Logger.ZInfo(ReceiptLoggingValidation.Enter_Into_Try_Block_Of_SaveReceiptJv);
                object obj = clientModel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                //Log.Logger.ZCritical(ex.StackTrace);

                var message = ex.Message;
            }
        }

        public void deleteJVPostInvoce(JournalSaveModel tObject)
        {
            //Log.Logger.ZInfo(tObject);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == ReceiptConstants.IdentityBean)
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
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                //Log.Logger.ZCritical(ex.StackTrace);

                var message = ex.Message;
            }
        }
        public void UpdatePosting(UpdatePosting upmodel)
        {
            try
            {
                var json = RestSharpHelper.ConvertObjectToJason(upmodel);
                string url = ConfigurationManager.AppSettings["BeanUrl"]?.ToString();
                const int maxRetryAttempts = 3;
                const int delayBetweenRetries = 2000;
                for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
                {
                    var response = RestSharpHelper.Post(url, "api/journal/updateposting", json);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return;
                    }
                    else
                    {
                        if (attempt < maxRetryAttempts)
                            System.Threading.Thread.Sleep(delayBetweenRetries);
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        //public void UpdatePosting(UpdatePosting upmodel)
        //{
        //    var json = RestSharpHelper.ConvertObjectToJason(upmodel);
        //    try
        //    {
        //        string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
        //        object obj = upmodel;
        //        var response = RestSharpHelper.Post(url, "api/journal/updateposting", json);
        //        if (response.ErrorMessage != null)
        //        {
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //    }
        //}

        private string GetNextApplicationNumber(string sysNumber, bool isFirst, string originalSysNumber)
        {
            string DocNumber = "";
            try
            {
                int DocNo = 0;
                if (!isFirst)
                {
                    DocNo = Convert.ToInt32(sysNumber.Substring(sysNumber.LastIndexOf("-JV") + 3));
                }
                DocNo++;
                DocNumber = originalSysNumber + ("-JV" + DocNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DocNumber;
        }

        private void FillICReceiptJournal(JVModel headJournal, Receipt _receipt, bool isNew, bool isBalancing, ReceiptDetail detail/*, long? subCompanyId*/, string shotCode)
        {
            //if (isfirst1)
            //    doc = _receipt.SystemRefNo;
            //isFirst = true;
            bool? isInterCompany = true;
            //string strServiceCompany = _companyService.GetById(_receipt.ServiceCompanyId).ShortName;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _receipt.Id;
            FillJV(headJournal, _receipt, isBalancing);
            doc = GetNextApplicationNumber(doc, false, _receipt.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            headJournal.ServiceCompanyId = detail.ServiceCompanyId;
            headJournal.IsGstSettings = false;
            //isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            FillICReceiptDetail(_receipt, detail, isNew, lstJD/*, subCompanyId*/, isInterCompany, shotCode);
            headJournal.DocCurrency = detail.Currency;
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();

        }
        private void FillICReceiptOffsetJournal(JVModel headJournal, Receipt _receipt, bool isNew, bool isBalancing, ReceiptDetail detail, string shotCode, bool? isBankDocDiff, long clearingCOAId)
        {
            //if (isfirst1)
            //    doc = _receipt.SystemRefNo;
            //isFirst = true;
            bool? isInterCompany = true;
            //string strServiceCompany = _companyService.GetById(_receipt.ServiceCompanyId).ShortName;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _receipt.Id;
            FillJV(headJournal, _receipt, isBalancing);
            doc = GetNextApplicationNumber(doc, false, _receipt.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            headJournal.ServiceCompanyId = detail.ServiceCompanyId;
            headJournal.IsGstSettings = false;
            //isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            FillICOffsetReceiptDetail(_receipt, detail, isNew, lstJD/*, subCompanyId*/, isInterCompany, shotCode, isBankDocDiff, clearingCOAId);
            headJournal.DocCurrency = detail.Currency;
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();

        }
        private void FillICReceiptOffsetJournal1(JVModel headJournal, Receipt _receipt, bool isNew, bool isBalancing, ReceiptDetail detail/*, long? subCompanyId*/, string shotCode, bool? isBankDocDiff, long clearingCOAId)
        {
            //if (isfirst1)
            //    doc = _receipt.SystemRefNo;
            //isFirst = true;
            decimal? baseDebit;
            bool? isInterCompany = true;
            if (_receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.Bill)).Any())
            {
                if (isNew)
                    headJournal.Id = Guid.NewGuid();
                else
                    headJournal.Id = _receipt.Id;
                FillJV(headJournal, _receipt, isBalancing);
                doc = GetNextApplicationNumber(doc, false, _receipt.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                headJournal.IsGstSettings = false;
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                FillICOffsetReceiptDetail1(_receipt, detail, isNew, lstJD/*, subCompanyId*/, isInterCompany, shotCode, isBankDocDiff, clearingCOAId);
                headJournal.DocCurrency = detail.Currency;
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                baseDebit = headJournal.JVVDetailModels.Where(c => c.COAId == clearingCOAId).Select(c => c.BaseDebit).FirstOrDefault();
                SaveReceiptJv(headJournal);
                if (detail.ServiceCompanyId != _receipt.ServiceCompanyId)
                {
                    headJournal = new JVModel();
                    if (isNew)
                        headJournal.Id = Guid.NewGuid();
                    else
                        headJournal.Id = _receipt.Id;
                    FillJV(headJournal, _receipt, isBalancing);
                    doc = GetNextApplicationNumber(doc, false, _receipt.SystemRefNo);
                    headJournal.SystemReferenceNo = doc;
                    headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                    headJournal.IsGstSettings = false;
                    lstJD = new List<JVVDetailModel>();

                    // Pradhan Commented
                    //baseDebit = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? baseDebit : Math.Round((decimal)baseDebit / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    if (_receipt.BaseCurrency == _receipt.BankChargesCurrency && _receipt.BankChargesCurrency != _receipt.DocCurrency)
                        baseDebit = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.ReceiptAmount) - _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
                    FillICOffsetClearingData(_receipt, detail, lstJD, shotCode, isBankDocDiff, clearingCOAId, baseDebit);
                    //headJournal.DocCurrency = detail.Currency;
                    headJournal.DocCurrency = _receipt.BankChargesCurrency != _receipt.DocCurrency ? _receipt.BankChargesCurrency : detail.Currency;
                    headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                    SaveReceiptJv(headJournal);
                }
            }
            List<Invoice> lstCreditNote = _invoiceService.GetListOfInvoices(_receipt.CompanyId, _receipt.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.CreditNote).Select(c => c.DocumentId).ToList());
            foreach (ReceiptDetail receiptDetail in _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.CreditNote))
            {
                baseDebit = 0;
                Invoice creditNote = lstCreditNote.Where(c => c.Id == receiptDetail.DocumentId).FirstOrDefault();
                if (creditNote != null)
                {
                    baseDebit = receiptDetail.ReceiptAmount;//_receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round(receiptDetail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round(receiptDetail.ReceiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                headJournal = new JVModel();
                if (isNew)
                    headJournal.Id = Guid.NewGuid();
                else
                    headJournal.Id = _receipt.Id;
                FillJV(headJournal, _receipt, isBalancing);
                doc = GetNextApplicationNumber(doc, false, _receipt.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                headJournal.IsGstSettings = false;
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                FillICOffsetClearingData(_receipt, receiptDetail, lstJD, shotCode, isBankDocDiff, clearingCOAId, baseDebit);
                //headJournal.DocCurrency = detail.Currency;
                headJournal.DocCurrency = _receipt.BankChargesCurrency != _receipt.DocCurrency ? _receipt.BankChargesCurrency : detail.Currency;
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                SaveReceiptJv(headJournal);
            }
            List<CreditMemoCompact> lstCreditMemo = _debitNoteService.GetAllCMByDocId(_receipt.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList(), _receipt.CompanyId);
            foreach (ReceiptDetail receiptDetail in _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.BillCreditMemo))
            {
                baseDebit = 0;
                CreditMemoCompact creditMemo = lstCreditMemo.Where(c => c.Id == receiptDetail.DocumentId).FirstOrDefault();
                if (creditMemo != null)
                {
                    baseDebit = receiptDetail.ReceiptAmount;//_receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round(receiptDetail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round(receiptDetail.ReceiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                headJournal = new JVModel();
                if (isNew)
                    headJournal.Id = Guid.NewGuid();
                else
                    headJournal.Id = _receipt.Id;
                FillJV(headJournal, _receipt, isBalancing);
                //FillJV(headJournal, _receipt, true);
                doc = GetNextApplicationNumber(doc, false, _receipt.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                headJournal.IsGstSettings = false;
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                FillICOffsetClearingData(_receipt, detail, lstJD, shotCode, isBankDocDiff, clearingCOAId, baseDebit);
                //headJournal.DocCurrency = detail.Currency;//commented by lokanath
                headJournal.DocCurrency = _receipt.BankChargesCurrency != _receipt.DocCurrency ? _receipt.BankChargesCurrency : detail.Currency;
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                SaveReceiptJv(headJournal);
            }
        }
        private void FillICOffsetReceiptDetail(Receipt _receipt, ReceiptDetail detail, bool isNew, List<JVVDetailModel> lstJD/*, long? serviceCompanyId*/, bool? isInterCompany, string shotCode, bool? isBankDocDiff, long clearingCOAId)
        {
            int? recOrder = 0;
            bool? isCustomer = false;
            decimal amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).Sum(c => c.ReceiptAmount) - _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
            if (amt != 0)
            {
                if (isNew)
                    journalHead.Id = Guid.NewGuid();
                else
                    journalHead.Id = detail.Id;
                isCustomer = null;
                FillICOffsetReceiptOutstandingDetail(detail, journalHead, _receipt, true, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journalHead.RecOrder = recOrder + 1;
                recOrder = journalHead.RecOrder;
                lstJD.Add(journalHead);
            }
            amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).Sum(c => c.ReceiptAmount);
            if (amt != 0)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                isCustomer = true;
                detail = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).FirstOrDefault();
                FillICOffsetReceiptOutstandingDetail(detail, journal, _receipt, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journal.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                lstJD.Add(journal);
                if (_receipt.DocCurrency != _receipt.BaseCurrency)
                {
                    JVVDetailModel journal1 = new JVVDetailModel();
                    if (isNew)
                        journal1.Id = Guid.NewGuid();
                    else
                        journal1.Id = detail.Id;
                    //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                    journalHead.DocCurrency = detail.Currency;
                    journalHead.ExchangeRate = journal.ExchangeRate;

                    FillExchangeGainLossItemOffsetValue(_receipt, journal1, amt, journalHead, detail.DocumentType);
                    _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
                    journal1.DocumentDetailId = detail.Id;
                    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recOrder = journal1.RecOrder;
                    if (_receipt.ExchangeRate != journal1.ExchangeRate)
                        lstJD.Add(journal1);
                }
            }
            amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
            if (amt != 0)
            {
                JVVDetailModel journal2 = new JVVDetailModel();
                if (isNew)
                    journal2.Id = Guid.NewGuid();
                else
                    journal2.Id = detail.Id;
                isCustomer = false;
                detail = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).FirstOrDefault();
                FillICOffsetReceiptOutstandingDetail(detail, journal2, _receipt, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                lstJD.Add(journal2);
                if (_receipt.DocCurrency != _receipt.BaseCurrency)
                {
                    JVVDetailModel journal1 = new JVVDetailModel();
                    if (isNew)
                        journal1.Id = Guid.NewGuid();
                    else
                        journal1.Id = detail.Id;
                    //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                    journalHead.DocCurrency = detail.Currency;
                    journalHead.ExchangeRate = journal2.ExchangeRate;
                    FillExchangeGainLossItemOffsetValue(_receipt, journal1, amt, journalHead, detail.DocumentType);
                    _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
                    journal1.DocumentDetailId = detail.Id;
                    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recOrder = journal1.RecOrder;
                    if (_receipt.ExchangeRate != journal1.ExchangeRate)
                        lstJD.Add(journal1);
                }
            }
            //if (_receipt.DocCurrency != _receipt.BaseCurrency)
            //{
            //    JVVDetailModel journal1 = new JVVDetailModel();
            //    if (isNew)
            //        journal1.Id = Guid.NewGuid();
            //    else
            //        journal1.Id = detail.Id;
            //    //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
            //    journalHead.DocCurrency = detail.Currency;
            //    FillCurrrencyCheckJv2(_receipt, journal1, detail, journalHead);
            //    _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
            //    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            //    //recOrder = journal1.RecOrder;
            //    if (_receipt.ExchangeRate != journal1.ExchangeRate)
            //        lstJD.Add(journal1);
            //}
        }
        private void FillICOffsetReceiptDetail1(Receipt _receipt, ReceiptDetail detail, bool isNew, List<JVVDetailModel> lstJD/*, long? serviceCompanyId*/, bool? isInterCompany, string shotCode, bool? isBankDocDiff, long clearingCOAId)
        {
            int? recOrder = 0;
            bool? isCustomer = false;
            decimal? amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).Sum(c => c.ReceiptAmount) - _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);

            if (amt != 0)
            {
                if (isNew)
                    journalHead.Id = Guid.NewGuid();
                else
                    journalHead.Id = detail.Id;
                isCustomer = null;
                FillICOffsetReceiptOutstandingDetail1(detail, journalHead, _receipt, true, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journalHead.RecOrder = ++recOrder;
                lstJD.Add(journalHead);
            }
            amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).Sum(c => c.ReceiptAmount);

            detail = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).FirstOrDefault();
            if (amt != 0)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                isCustomer = true;
                FillICOffsetReceiptOutstandingDetail1(detail, journal, _receipt, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journal.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                lstJD.Add(journal);
                if (_receipt.DocCurrency != _receipt.BaseCurrency)
                {
                    JVVDetailModel journal1 = new JVVDetailModel();
                    if (isNew)
                        journal1.Id = Guid.NewGuid();
                    else
                        journal1.Id = detail.Id;
                    //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                    journalHead.DocCurrency = detail.Currency;
                    journalHead.ExchangeRate = journal.ExchangeRate;
                    FillExchangeGainLossItemOffset(_receipt, journal1, detail, journalHead);
                    _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
                    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recOrder = journal1.RecOrder;
                    if (_receipt.ExchangeRate != journal1.ExchangeRate)
                        lstJD.Add(journal1);
                }
            }

            amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
            detail = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).FirstOrDefault();
            if (amt != 0)
            {
                JVVDetailModel journal2 = new JVVDetailModel();
                if (isNew)
                    journal2.Id = Guid.NewGuid();
                else
                    journal2.Id = detail.Id;
                isCustomer = false;
                FillICOffsetReceiptOutstandingDetail1(detail, journal2, _receipt, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                lstJD.Add(journal2);
                if (_receipt.DocCurrency != _receipt.BaseCurrency)
                {
                    JVVDetailModel journal1 = new JVVDetailModel();
                    if (isNew)
                        journal1.Id = Guid.NewGuid();
                    else
                        journal1.Id = detail.Id;
                    //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                    journalHead.DocCurrency = detail.Currency;
                    journalHead.ExchangeRate = journal2.ExchangeRate;
                    FillExchangeGainLossItemOffset(_receipt, journal1, detail, journalHead);
                    _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
                    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recOrder = journal1.RecOrder;
                    if (_receipt.ExchangeRate != journal1.ExchangeRate)
                        lstJD.Add(journal1);
                }
            }
        }
        private void FillICOffsetClearingData(Receipt _receipt, ReceiptDetail detail, List<JVVDetailModel> lstJD, string shotCode, bool? isBankDocDiff, long clearingCOAId, decimal? amt)
        {
            int? recOrder = 0;
            //decimal? amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            //amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            journalHead.Id = Guid.NewGuid();
            FillICOffsetReceiptOutstandingClearing(detail, journalHead, _receipt, shotCode, amt, true, isBankDocDiff, clearingCOAId);
            journalHead.RecOrder = recOrder + 1;
            recOrder = journalHead.RecOrder;
            lstJD.Add(journalHead);
            JVVDetailModel journal = new JVVDetailModel();
            journal.Id = Guid.NewGuid();
            FillICOffsetReceiptOutstandingClearing(detail, journal, _receipt, shotCode, amt, false, isBankDocDiff, clearingCOAId);
            journal.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
            lstJD.Add(journal);
            //if (_receipt.DocCurrency != _receipt.BaseCurrency)
            //{
            //    JVVDetailModel journal1 = new JVVDetailModel();
            //    journal1.Id = Guid.NewGuid();
            //    journalHead.DocCurrency = detail.Currency;
            //    FillCurrrencyCheckJv2(_receipt, journal1, detail, journalHead);
            //    _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
            //    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            //    //recOrder = journal1.RecOrder;
            //    if (_receipt.ExchangeRate != journal1.ExchangeRate)
            //        lstJD.Add(journal1);
            //}
        }
        private void FillICReceiptDetail(Receipt _receipt, ReceiptDetail detail, bool isNew, List<JVVDetailModel> lstJD/*, long? serviceCompanyId*/, bool? isInterCompany, string shotCode)
        {
            int? recOrder = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            if (isNew)
                journalHead.Id = Guid.NewGuid();
            else
                journalHead.Id = detail.Id;
            FillICReceiptOutstandingDetail(detail, journalHead, _receipt, true, shotCode);
            journalHead.RecOrder = recOrder + 1;
            recOrder = journalHead.RecOrder;
            lstJD.Add(journalHead);
            JVVDetailModel journal = new JVVDetailModel();
            if (isNew)
                journal.Id = Guid.NewGuid();
            else
                journal.Id = detail.Id;
            FillICReceiptOutstandingDetail(detail, journal, _receipt, false, shotCode);
            journal.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
            lstJD.Add(journal);
            if (_receipt.DocCurrency != _receipt.BaseCurrency)
            {
                JVVDetailModel journal1 = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal1.Id = detail.Id;
                //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                journalHead.DocCurrency = detail.Currency;
                FillCurrrencyCheckJv2(_receipt, journal1, detail, journal);
                _receipt.ExchangeRate = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
                journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                //recOrder = journal1.RecOrder;
                if (_receipt.ExchangeRate != journal1.ExchangeRate)
                    lstJD.Add(journal1);
            }
        }
        private void FillICReceiptOutstandingDetail(ReceiptDetail detail, JVVDetailModel jmodel, Receipt invoice, bool? isInterCompany, string shotCode)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (invoice.SystemCalculatedExchangeRate == null || invoice.SystemCalculatedExchangeRate == 0) ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.DocNo = invoice.DocNo;
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            //if (account1 != null)
            //    jmodel.COAId = account1.Id;
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.AccountDescription = invoice.Remarks;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                Invoice inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                DebitNote inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            //jmodel.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel.DocDate = invoice.DocDate;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            if (isInterCompany == false)
            {
                jmodel.DocCredit = detail.ReceiptAmount;
                jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = account1.Id;
            }
            else
            {
                jmodel.DocDebit = detail.ReceiptAmount;
                //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2);
                jmodel.BaseDebit = invoice.BaseCurrency != invoice.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
            }
        }
        private void FillICOffsetReceiptOutstandingDetail(ReceiptDetail detail, JVVDetailModel jmodel, Receipt invoice, bool? isInterCompany, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (invoice.SystemCalculatedExchangeRate == null || invoice.SystemCalculatedExchangeRate == 0) ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.DocNo = invoice.DocNo;
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (isCustomer == true)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            //if (account1 != null)
            //    jmodel.COAId = account1.Id;
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.AccountDescription = invoice.Remarks;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                Invoice inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                DebitNote inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            //jmodel.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel.DocDate = invoice.DocDate;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            if (isInterCompany == false && isCustomer == true)
            {
                if (detail.DocumentType != DocTypeConstants.CreditNote)
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isBankDocDiff == true ? clearingCOAId : account1.Id;
                }
                else
                {
                    jmodel.DocDebit = recAmt;
                    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isBankDocDiff == true ? clearingCOAId : account1.Id;
                }
            }
            else
            {
                if (detail.DocumentType != DocTypeConstants.CreditNote)
                {
                    jmodel.DocDebit = recAmt;
                    //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2);
                    jmodel.BaseDebit = isCustomer == false ? Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : invoice.BaseCurrency != invoice.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    if (chartOfAccount != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : chartOfAccount.Id;
                }
                else
                {
                    jmodel.DocCredit = recAmt;
                    //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2);
                    jmodel.BaseCredit = isCustomer == false ? Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : invoice.BaseCurrency != invoice.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocCredit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    if (chartOfAccount != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : chartOfAccount.Id;
                }
            }
            if (recAmt < 0)
            {
                jmodel.DocCredit = -jmodel.DocDebit;
                jmodel.BaseCredit = -jmodel.BaseDebit;
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
            }
        }
        private void FillICOffsetReceiptOutstandingDetail1(ReceiptDetail detail, JVVDetailModel jmodel, Receipt invoice, bool? isInterCompany, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (invoice.SystemCalculatedExchangeRate == null || invoice.SystemCalculatedExchangeRate == 0) ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.DocNo = invoice.DocNo;
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (isCustomer == true)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            //if (account1 != null)
            //    jmodel.COAId = account1.Id;
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.AccountDescription = invoice.Remarks;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                Invoice inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                DebitNote inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            //jmodel.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel.DocDate = invoice.DocDate;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            if (isInterCompany == false && isCustomer == true)
            {
                //if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
                //{
                //    jmodel.DocCredit = recAmt;
                //    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2, MidpointRounding.AwayFromZero);
                //    jmodel.DocDebit = null;
                //    jmodel.BaseDebit = null;
                //    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                //    if (account1 != null)
                //        jmodel.COAId = /*isBankDocDiff == true ? clearingCOAId :*/ account1.Id;
                //}
                //else
                //{
                if (detail.DocumentType != DocTypeConstants.CreditNote)
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = /*isBankDocDiff == true ? clearingCOAId :*/ account1.Id;
                }
                else
                {
                    jmodel.DocDebit = recAmt;
                    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = /*isBankDocDiff == true ? clearingCOAId :*/ account1.Id;
                }
                //  }
            }
            else
            {
                //if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
                //{
                //    jmodel.DocCredit = recAmt;
                //    //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2);
                //    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2, MidpointRounding.AwayFromZero);
                //    jmodel.DocDebit = null;
                //    jmodel.BaseDebit = null;
                //    if (chartOfAccount != null)
                //        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                //}
                //else
                //{
                if (detail.DocumentType != DocTypeConstants.CreditNote)
                {
                    jmodel.DocDebit = recAmt;
                    //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2);
                    //jmodel.BaseDebit = invoice.BaseCurrency != invoice.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                    jmodel.BaseDebit = isCustomer == false ? Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : invoice.BaseCurrency != invoice.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    if (chartOfAccount != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
                else
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = isCustomer == false ? Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : invoice.BaseCurrency != invoice.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocCredit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    if (chartOfAccount != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
                // }
            }
            if (recAmt < 0)
            {
                jmodel.DocCredit = -jmodel.DocDebit;
                jmodel.BaseCredit = -jmodel.BaseDebit;
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
            }
        }
        private void FillICOffsetReceiptOutstandingClearing(ReceiptDetail detail, JVVDetailModel jmodel, Receipt invoice, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.DocNo = invoice.DocNo;
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.ExchangeRate = invoice.BankChargesCurrency == invoice.BaseCurrency ? 1 : invoice.ExchangeRate;
            jmodel.DocDate = invoice.DocDate;
            jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            if (isCustomer == false)
            {
                if (detail.DocumentType != DocTypeConstants.CreditNote)
                {
                    if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
                    {
                        //jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.BaseCredit = recAmt;
                        jmodel.DocCredit = Math.Round((decimal)recAmt / (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        jmodel.DocCredit = recAmt;
                        jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    if (invoice.BaseCurrency == invoice.BankChargesCurrency && invoice.BankChargesCurrency != invoice.DocCurrency)
                    {
                        jmodel.BaseCredit = Math.Round((decimal)(recAmt * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocCredit = jmodel.BaseCredit;
                    }
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    jmodel.COAId = clearingCOAId;
                }
                else
                {
                    if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
                    {
                        jmodel.BaseDebit = recAmt;
                        jmodel.DocDebit = Math.Round((decimal)recAmt / (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = recAmt;
                        jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    }
                    if (invoice.BaseCurrency == invoice.BankChargesCurrency && invoice.BankChargesCurrency != invoice.DocCurrency)
                    {
                        jmodel.BaseDebit = Math.Round((decimal)(recAmt * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = jmodel.BaseDebit;
                    }
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    jmodel.COAId = clearingCOAId;
                }
            }
            else
            {
                if (detail.DocumentType != DocTypeConstants.CreditNote)
                {
                    if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
                    {
                        //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.BaseDebit = recAmt;
                        jmodel.DocDebit = Math.Round((decimal)recAmt / (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        jmodel.DocDebit = recAmt;
                        jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    if (invoice.BaseCurrency == invoice.BankChargesCurrency && invoice.BankChargesCurrency != invoice.DocCurrency)
                    {
                        jmodel.BaseDebit = Math.Round((decimal)(recAmt * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = jmodel.BaseDebit;
                    }
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    if (chartOfAccount != null)
                        jmodel.COAId = chartOfAccount.Id;
                }
                else
                {
                    if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
                    {
                        jmodel.BaseCredit = recAmt;
                        jmodel.DocCredit = Math.Round((decimal)recAmt / (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? jmodel.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        //jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocCredit = recAmt;
                        jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    }
                    if (invoice.BaseCurrency == invoice.BankChargesCurrency && invoice.BankChargesCurrency != invoice.DocCurrency)
                    {
                        jmodel.BaseCredit = Math.Round((decimal)(recAmt * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocCredit = jmodel.BaseCredit;
                    }
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    if (chartOfAccount != null)
                        jmodel.COAId = chartOfAccount.Id;
                }
            }
        }
        private void FillCurrrencyCheckJv2(Receipt _receipt, JVVDetailModel journal1, ReceiptDetail detail, JVVDetailModel journalDetailData)
        {
            var originalExchangeRate1 = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate == null ? 0 : _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
            journal1.DocumentDetailId = detail.Id;
            journal1.DocumentId = _receipt.Id;
            journal1.PostingDate = _receipt.DocDate;
            journal1.SystemRefNo = _receipt.SystemRefNo;
            journal1.ServiceCompanyId = _receipt.ServiceCompanyId;
            journal1.AccountDescription = _receipt.Remarks;
            journal1.DocDate = _receipt.DocDate.Date;
            journal1.DocType = DocTypeConstants.Receipt;
            journal1.DocSubType = DocTypeConstants.General;
            journal1.DocNo = _receipt.DocNo;
            if (journalDetailData != null)
            {
                journal1.Nature = journalDetailData.Nature;
                journal1.EntityId = journalDetailData.EntityId;
                journal1.OffsetDocument = journalDetailData.SystemRefNo;
                journal1.ExchangeRate = journalDetailData.ExchangeRate;
            }
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == _receipt.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
            {
                journal1.COAId = account2.Id;
            }
            journal1.BaseCurrency = _receipt.BaseCurrency;
            if (originalExchangeRate1 > journal1.ExchangeRate)
            {
                if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    journal1.BaseCredit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
            }
            if (originalExchangeRate1 < journal1.ExchangeRate)
            {
                if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                {
                    journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                }

            }
        }

        private void FillExchangeGainLossItemOffset(Receipt _receipt, JVVDetailModel journal1, ReceiptDetail detail, JVVDetailModel journalDetailData)
        {
            var originalExchangeRate1 = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate == null ? 0 : _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
            journal1.DocumentDetailId = detail.Id;
            journal1.DocumentId = _receipt.Id;
            journal1.PostingDate = _receipt.DocDate;
            journal1.SystemRefNo = _receipt.SystemRefNo;
            journal1.ServiceCompanyId = _receipt.ServiceCompanyId;
            journal1.AccountDescription = _receipt.Remarks;
            journal1.DocDate = _receipt.DocDate.Date;
            journal1.DocType = DocTypeConstants.Receipt;
            journal1.DocSubType = DocTypeConstants.General;
            journal1.DocNo = _receipt.DocNo;
            if (journalDetailData != null)
            {
                journal1.Nature = journalDetailData.Nature;
                journal1.EntityId = journalDetailData.EntityId;
                journal1.OffsetDocument = journalDetailData.SystemRefNo;
                journal1.ExchangeRate = journalDetailData.ExchangeRate;
            }
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == _receipt.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
            {
                journal1.COAId = account2.Id;
            }
            journal1.BaseCurrency = _receipt.BaseCurrency;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseCredit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            else
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseDebit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.ReceiptAmount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
        }
        private void FillExchangeGainLossItemOffsetValue(Receipt _receipt, JVVDetailModel journal1, decimal amount, JVVDetailModel journalDetailData, string docType)
        {
            var originalExchangeRate1 = (_receipt.SystemCalculatedExchangeRate == null || _receipt.SystemCalculatedExchangeRate == 0) ? _receipt.ExchangeRate == null ? 0 : _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate;
            journal1.DocumentId = _receipt.Id;
            journal1.PostingDate = _receipt.DocDate;
            journal1.SystemRefNo = _receipt.SystemRefNo;
            journal1.ServiceCompanyId = _receipt.ServiceCompanyId;
            journal1.AccountDescription = _receipt.Remarks;
            journal1.DocDate = _receipt.DocDate.Date;
            journal1.DocType = DocTypeConstants.Receipt;
            journal1.DocSubType = DocTypeConstants.General;
            journal1.DocNo = _receipt.DocNo;
            if (journalDetailData != null)
            {
                journal1.Nature = journalDetailData.Nature;
                journal1.EntityId = journalDetailData.EntityId;
                journal1.OffsetDocument = journalDetailData.SystemRefNo;
                journal1.ExchangeRate = journalDetailData.ExchangeRate;
            }
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == _receipt.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
            {
                journal1.COAId = account2.Id;
            }
            journal1.BaseCurrency = _receipt.BaseCurrency;
            if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.DebitNote || docType == DocTypeConstants.BillCreditMemo)
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseCredit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * amount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * amount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            else
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseDebit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * amount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * amount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
        }

        private void FillReceiptOffsetJournalNew(JVModel headJournal, Receipt _receipt, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<ReceiptDetail> lstDetail, bool? isBankDocDiff, long clearingCOAId)
        {
            if (isfirst1)
                doc = _receipt.SystemRefNo;
            isFirst = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _receipt.Id;
            FillJV(headJournal, _receipt, isBalancing);
            doc = GetNextApplicationNumber(doc, isfirst1, _receipt.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == true)
            {
                JVVDetailModel Jmodel = new JVVDetailModel();
                FillJDetail(Jmodel, _receipt, "BankReceiptAmmount", headJournal.ExchangeRate);
                Jmodel.RecOrder = recorder + 1;
                recorder = Jmodel.RecOrder;
                lstJD.Add(Jmodel);
                Jmodel = new JVVDetailModel();
                if (_receipt.BankCharges != null)
                {
                    FillJDetail(Jmodel, _receipt, "BankCharges", headJournal.ExchangeRate);
                    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = Jmodel.RecOrder;
                    lstJD.Add(Jmodel);
                }
                Jmodel = new JVVDetailModel();
                if (_receipt.ExcessPaidByClientAmmount != null)
                {
                    FillJDetail(Jmodel, _receipt, "ExcesPaidByClient", headJournal.ExchangeRate);
                    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = Jmodel.RecOrder;
                    lstJD.Add(Jmodel);
                }
                foreach (ReceiptBalancingItem rBI in _receipt.ReceiptBalancingItems)
                {
                    JVVDetailModel jmodel1 = new JVVDetailModel();
                    FillBalancingitems(jmodel1, rBI, _receipt, headJournal.ExchangeRate);
                    if (isNew)
                        jmodel1.Id = Guid.NewGuid();
                    else
                        jmodel1.Id = rBI.Id;
                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = jmodel1.RecOrder;
                    lstJD.Add(jmodel1);
                }
                if (_receipt.IsGstSettings)
                {
                    foreach (ReceiptBalancingItem rBI in _receipt.ReceiptBalancingItems.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                    {
                        JVVDetailModel jmodel1 = new JVVDetailModel();
                        FillBalancingGstitems(jmodel1, rBI, _receipt, headJournal.ExchangeRate);
                        if (isNew)
                            jmodel1.Id = Guid.NewGuid();
                        else
                            jmodel1.Id = rBI.Id;
                        jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        // recorder = jmodel1.RecOrder;
                        lstJD.Add(jmodel1);
                    }
                }
                recorder1 = recorder;
                //if (_receipt.BankChargesCurrency != _receipt.DocCurrency)
                //{
                //    JVVDetailModel jmodel1 = new JVVDetailModel();
                //    FillClearingReciptOffset(jmodel1, _receipt, true, headJournal.ExchangeRate, clearingCOAId);
                //    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                //    // recorder = jmodel1.RecOrder;
                //    lstJD.Add(jmodel1);
                //}
                //if (_receipt.BankChargesCurrency == _receipt.DocCurrency)
                //{
                if (_receipt.ReceiptDetails.Any(c => c.ServiceCompanyId != _receipt.ServiceCompanyId) || _receipt.ReceiptDetails.GroupBy(c => c.ServiceCompanyId).Count() != 1)
                {
                    var lstGrpDetail = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount > 0).GroupBy(c => c.ServiceCompanyId).Select(c => new { Detail = c.ToList(), Count = c.Count() }).ToList();
                    if (lstGrpDetail.Any())
                    {
                        decimal? amt = 0;
                        bool? isCN = false;
                        foreach (ReceiptDetail detail in lstGrpDetail.Where(c => c.Count >= 2).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.ReceiptAmount != 0)
                            {
                                JVVDetailModel jmodel = new JVVDetailModel();
                                amt = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).Sum(c => c.ReceiptAmount) - lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
                                //if (_receipt.DocCurrency != _receipt.BankChargesCurrency)
                                //    amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;

                                FillDetailOffset(jmodel, detail, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId, isCN);
                                jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                lstJD.Add(jmodel);
                                JVVDetailModel jmodel1 = new JVVDetailModel();
                                jmodel1.Id = Guid.NewGuid();
                                jmodel1.Id = detail.Id;
                                amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.CreditNote).Sum(c => c.ReceiptAmount);
                                //if (_receipt.DocCurrency != _receipt.BankChargesCurrency)
                                //    amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                if (amt != 0)
                                {
                                    isCN = true;
                                    FillDetailOffset(jmodel1, detail, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId, isCN);
                                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;

                                    //Pradhan commented

                                    //jmodel1.DocDebit = amt;
                                    //if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.BaseCurrency != _receipt.BankChargesCurrency)
                                    //    jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    //else
                                    //    jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (decimal)jmodel1.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                                    //jmodel1.DocCredit = null;
                                    //jmodel1.BaseCredit = null;
                                    lstJD.Add(jmodel1);
                                }
                                //if (_receipt.DocCurrency == _receipt.BankChargesCurrency && _receipt.DocCurrency != _receipt.BaseCurrency)
                                //{
                                //    JVVDetailModel jmodel2 = new JVVDetailModel();
                                //    if (isNew)
                                //        jmodel2.Id = Guid.NewGuid();
                                //    else
                                //        jmodel2.Id = detail.Id;
                                //    FillGstDetail(jmodel2, detail, _receipt, jmodel);
                                //    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                //    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                //        lstJD.Add(jmodel2);
                                //}
                            }
                        }
                        foreach (ReceiptDetail detail in lstGrpDetail.Where(c => c.Count == 1).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.ReceiptAmount != 0)
                            {
                                amt = 0;
                                JVVDetailModel jmodel = new JVVDetailModel();
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;
                                //if (_receipt.DocCurrency != _receipt.BankChargesCurrency)
                                //    amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round(detail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round(detail.ReceiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                //else
                                isCN = detail.DocumentType == DocTypeConstants.CreditNote;
                                amt = detail.ReceiptAmount;
                                FillDetailOffset(jmodel, detail, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId, isCN);
                                jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                lstJD.Add(jmodel);
                                if (_receipt.DocCurrency == _receipt.BankChargesCurrency && _receipt.DocCurrency != _receipt.BaseCurrency)
                                {
                                    JVVDetailModel jmodel2 = new JVVDetailModel();
                                    if (isNew)
                                        jmodel2.Id = Guid.NewGuid();
                                    else
                                        jmodel2.Id = detail.Id;
                                    FillGstDetail(jmodel2, detail, _receipt, jmodel);
                                    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                        lstJD.Add(jmodel2);
                                }
                            }
                        }
                    }
                }
                else
                {
                    decimal? amt = 0;
                    if (_receipt.DocCurrency == _receipt.BankChargesCurrency)
                    {
                        foreach (ReceiptDetail detail in _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0).OrderBy(c => c.RecOrder))
                        {
                            amt = 0;
                            JVVDetailModel jmodel = new JVVDetailModel();
                            if (isNew)
                                jmodel.Id = Guid.NewGuid();
                            else
                                jmodel.Id = detail.Id;
                            if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                                amt = Math.Round(detail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            else
                                amt = detail.ReceiptAmount;
                            FillDetailOffset2(jmodel, detail, _receipt, lstDetail, amt, clearingCOAId);
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            //if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.Bills)
                            //{
                            //    jmodel.COAId = clearingCOAId;
                            //    if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills)
                            //    {
                            //        jmodel.DocDebit = amt;
                            //        jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                            //        jmodel.DocCredit = null;
                            //        jmodel.BaseCredit = null;
                            //    }
                            //}
                            //recorder = jmodel.RecOrder;
                            lstJD.Add(jmodel);
                            if (_receipt.DocCurrency == _receipt.BankChargesCurrency && _receipt.DocCurrency != _receipt.BaseCurrency && detail.DocumentType != DocTypeConstants.CreditNote && detail.DocumentType != DocTypeConstants.BillCreditMemo)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, _receipt, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }
                        }
                    }
                    else
                    {
                        amt = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote) && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount) - _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.DocumentType == DocTypeConstants.Bills && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();

                            FillDetailOffsetDetailNew(jmodel, DocTypeConstants.Invoice, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);
                        }
                        amt = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.DocumentType == DocTypeConstants.CreditNote && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();

                            FillDetailOffsetDetailNew(jmodel, DocTypeConstants.CreditNote, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);
                        }
                        amt = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.DocumentType == DocTypeConstants.BillCreditMemo && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();
                            //if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                            //    amt = Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            //if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.DocCurrency != _receipt.BankChargesCurrency)
                            //    amt = Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            FillDetailOffsetDetailNew(jmodel, DocTypeConstants.BillCreditMemo, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);
                        }
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }
        private void FillDetailOffsetNew(JVVDetailModel jmodel, ReceiptDetail detail, Receipt invoice, List<ReceiptDetail> lstDetail, decimal? receiptAmount, bool? isBankDocDiff, long clearingCOAId)
        {
            ReceiptDetail detailM = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
            string shotCode = string.Empty;
            if (detailM != null)
            {
                shotCode = _companyService.Query(a => a.Id == detailM.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                shotCode = "I/C" + " - " + shotCode;
            }
            jmodel.DocType = DocTypeConstants.Receipt;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = invoice.Remarks;
            jmodel.SystemRefNo = invoice.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.ReceiptId;
            jmodel.Nature = detail.Nature;
            AppsWorld.ReceiptModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, invoice.CompanyId);
            AppsWorld.ReceiptModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == invoice.CompanyId /*&& a.SubsidaryCompanyId == detail.ServiceCompanyId*/).Select().FirstOrDefault();
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                var inv = _invoiceService.GetInvoiceById(detail.DocumentId, invoice.CompanyId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                var inv = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills)
                jmodel.ExchangeRate = _debitNoteService.GetBillExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _debitNoteService.GetMemoExchangeRate(detail.DocumentId, invoice.CompanyId);
            if (invoice.ServiceCompanyId == detail.ServiceCompanyId)
            {
                if (account1 != null)
                    jmodel.COAId = isBankDocDiff == true ? clearingCOAId : account1.Id;
                jmodel.ServiceCompanyId = invoice.ServiceCompanyId;
                jmodel.ExchangeRate = invoice.BaseCurrency == invoice.BankChargesCurrency ? 1 : jmodel.ExchangeRate;
            }
            else
            {
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
                if (detail.ServiceCompanyId != null)
                    jmodel.ServiceCompanyId = detail.ServiceCompanyId.Value;
                jmodel.ExchangeRate = invoice.BaseCurrency == invoice.BankChargesCurrency ? 1 : invoice.ExchangeRate;
            }

            jmodel.EntityId = invoice.EntityId;
            jmodel.BaseCurrency = invoice.BaseCurrency;

            jmodel.DocDate = invoice.DocDate;
            jmodel.DocNo = invoice.DocNo;
            jmodel.PostingDate = invoice.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;
            jmodel.DocCredit = receiptAmount;
            if (invoice.DocCurrency == invoice.BaseCurrency && invoice.BaseCurrency != invoice.BankChargesCurrency)
            {
                jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                if ((invoice.DocCurrency == invoice.BaseCurrency) || (invoice.DocCurrency != invoice.BaseCurrency && invoice.ServiceCompanyId == detail.ServiceCompanyId))
                    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                else
                {
                    //jmodel.ExchangeRate = invoice.SystemCalculatedExchangeRate == 0 || invoice.SystemCalculatedExchangeRate == null ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate;
                    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                }
            }
            if (receiptAmount < 0)
            {
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }

        private void FillReceiptOffsetJournal(JVModel headJournal, Receipt _receipt, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<ReceiptDetail> lstDetail, bool? isBankDocDiff, long clearingCOAId)
        {
            if (isfirst1)
                doc = _receipt.SystemRefNo;
            isFirst = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _receipt.Id;
            FillJV(headJournal, _receipt, isBalancing);
            doc = GetNextApplicationNumber(doc, isfirst1, _receipt.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == true)
            {
                JVVDetailModel Jmodel = new JVVDetailModel();
                FillJDetail(Jmodel, _receipt, "BankReceiptAmmount", headJournal.ExchangeRate);
                Jmodel.RecOrder = recorder + 1;
                recorder = Jmodel.RecOrder;
                lstJD.Add(Jmodel);
                Jmodel = new JVVDetailModel();
                if (_receipt.BankCharges != null)
                {
                    FillJDetail(Jmodel, _receipt, "BankCharges", headJournal.ExchangeRate);
                    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = Jmodel.RecOrder;
                    lstJD.Add(Jmodel);
                }
                Jmodel = new JVVDetailModel();
                if (_receipt.ExcessPaidByClientAmmount != null)
                {
                    FillJDetail(Jmodel, _receipt, "ExcesPaidByClient", headJournal.ExchangeRate);
                    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = Jmodel.RecOrder;
                    lstJD.Add(Jmodel);
                }
                foreach (ReceiptBalancingItem rBI in _receipt.ReceiptBalancingItems)
                {
                    JVVDetailModel jmodel1 = new JVVDetailModel();
                    FillBalancingitems(jmodel1, rBI, _receipt, headJournal.ExchangeRate);
                    if (isNew)
                        jmodel1.Id = Guid.NewGuid();
                    else
                        jmodel1.Id = rBI.Id;
                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    //recorder = jmodel1.RecOrder;
                    lstJD.Add(jmodel1);
                }
                if (_receipt.IsGstSettings)
                {
                    foreach (ReceiptBalancingItem rBI in _receipt.ReceiptBalancingItems.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                    {
                        JVVDetailModel jmodel1 = new JVVDetailModel();
                        FillBalancingGstitems(jmodel1, rBI, _receipt, headJournal.ExchangeRate);
                        if (isNew)
                            jmodel1.Id = Guid.NewGuid();
                        else
                            jmodel1.Id = rBI.Id;
                        jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        // recorder = jmodel1.RecOrder;
                        lstJD.Add(jmodel1);
                    }
                }
                recorder1 = recorder;
                //if (_receipt.BankChargesCurrency != _receipt.DocCurrency)
                //{
                //    JVVDetailModel jmodel1 = new JVVDetailModel();
                //    FillClearingReciptOffset(jmodel1, _receipt, true, headJournal.ExchangeRate, clearingCOAId);
                //    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                //    // recorder = jmodel1.RecOrder;
                //    lstJD.Add(jmodel1);
                //}
                //if (_receipt.BankChargesCurrency == _receipt.DocCurrency)
                //{
                if (_receipt.ReceiptDetails.Any(c => c.ServiceCompanyId != _receipt.ServiceCompanyId) || _receipt.ReceiptDetails.GroupBy(c => c.ServiceCompanyId).Count() != 1)
                {
                    var lstGrpDetail = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount > 0).GroupBy(c => c.ServiceCompanyId).Select(c => new { Detail = c.ToList(), Count = c.Count() }).ToList();
                    if (lstGrpDetail.Any())
                    {
                        decimal? amt = 0;
                        foreach (ReceiptDetail detail in lstGrpDetail.Where(c => c.Count >= 2).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.ReceiptAmount != 0)
                            {
                                JVVDetailModel jmodel = new JVVDetailModel();
                                amt = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.BillCreditMemo)).Sum(c => c.ReceiptAmount) - lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.ReceiptAmount);
                                if (_receipt.DocCurrency != _receipt.BankChargesCurrency)
                                    amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;
                                FillDetailOffset(jmodel, detail, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId, false);
                                jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                lstJD.Add(jmodel);
                                JVVDetailModel jmodel1 = new JVVDetailModel();
                                jmodel1.Id = Guid.NewGuid();
                                jmodel1.Id = detail.Id;
                                amt = _receipt.ReceiptDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.CreditNote).Sum(c => c.ReceiptAmount);
                                if (_receipt.DocCurrency != _receipt.BankChargesCurrency)
                                    amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                if (amt != 0)
                                {
                                    FillDetailOffset(jmodel1, detail, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId, false);
                                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    jmodel1.DocDebit = amt;
                                    if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.BaseCurrency != _receipt.BankChargesCurrency)
                                        jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                    else
                                        jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (decimal)jmodel1.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                                    jmodel1.DocCredit = null;
                                    jmodel1.BaseCredit = null;
                                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    lstJD.Add(jmodel1);
                                }
                                //if (_receipt.DocCurrency == _receipt.BankChargesCurrency && _receipt.DocCurrency != _receipt.BaseCurrency)
                                //{
                                //    JVVDetailModel jmodel2 = new JVVDetailModel();
                                //    if (isNew)
                                //        jmodel2.Id = Guid.NewGuid();
                                //    else
                                //        jmodel2.Id = detail.Id;
                                //    FillGstDetail(jmodel2, detail, _receipt, jmodel);
                                //    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                //    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                //        lstJD.Add(jmodel2);
                                //}
                            }
                        }
                        foreach (ReceiptDetail detail in lstGrpDetail.Where(c => c.Count == 1).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.ReceiptAmount != 0)
                            {
                                amt = 0;
                                JVVDetailModel jmodel = new JVVDetailModel();
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;
                                if (_receipt.DocCurrency != _receipt.BankChargesCurrency)
                                    amt = _receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round(detail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round(detail.ReceiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                else
                                    amt = detail.ReceiptAmount;
                                FillDetailOffset(jmodel, detail, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId, false);
                                jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                //recorder = jmodel.RecOrder;
                                lstJD.Add(jmodel);
                                if (_receipt.DocCurrency == _receipt.BankChargesCurrency && _receipt.DocCurrency != _receipt.BaseCurrency)
                                {
                                    JVVDetailModel jmodel2 = new JVVDetailModel();
                                    if (isNew)
                                        jmodel2.Id = Guid.NewGuid();
                                    else
                                        jmodel2.Id = detail.Id;
                                    FillGstDetail(jmodel2, detail, _receipt, jmodel);
                                    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                        lstJD.Add(jmodel2);
                                }
                            }
                        }
                    }
                }
                else
                {
                    decimal? amt = 0;
                    if (_receipt.DocCurrency == _receipt.BankChargesCurrency)
                    {
                        foreach (ReceiptDetail detail in _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0).OrderBy(c => c.RecOrder))
                        {
                            amt = 0;
                            JVVDetailModel jmodel = new JVVDetailModel();
                            if (isNew)
                                jmodel.Id = Guid.NewGuid();
                            else
                                jmodel.Id = detail.Id;
                            if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                                amt = Math.Round(detail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            else
                                amt = detail.ReceiptAmount;
                            FillDetailOffset2(jmodel, detail, _receipt, lstDetail, amt, clearingCOAId);
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            //if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.Bills)
                            //{
                            //    jmodel.COAId = clearingCOAId;
                            //    if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills)
                            //    {
                            //        jmodel.DocDebit = amt;
                            //        jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                            //        jmodel.DocCredit = null;
                            //        jmodel.BaseCredit = null;
                            //    }
                            //}
                            //recorder = jmodel.RecOrder;
                            lstJD.Add(jmodel);
                            if (_receipt.DocCurrency == _receipt.BankChargesCurrency && _receipt.DocCurrency != _receipt.BaseCurrency && detail.DocumentType != DocTypeConstants.CreditNote && detail.DocumentType != DocTypeConstants.BillCreditMemo)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, _receipt, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }
                        }
                    }
                    else
                    {
                        amt = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote) && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount) - _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.DocumentType == DocTypeConstants.Bills && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();
                            if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                                amt = Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.DocCurrency != _receipt.BankChargesCurrency)
                                amt = Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            FillDetailOffsetDetail(jmodel, DocTypeConstants.Invoice, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);
                        }
                        amt = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.DocumentType == DocTypeConstants.CreditNote && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();
                            if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                                amt = Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.DocCurrency != _receipt.BankChargesCurrency)
                                amt = Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            FillDetailOffsetDetail(jmodel, DocTypeConstants.CreditNote, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);
                        }
                        amt = _receipt.ReceiptDetails.Where(c => c.ReceiptAmount != 0 && c.DocumentType == DocTypeConstants.BillCreditMemo && c.ServiceCompanyId == _receipt.ServiceCompanyId).Sum(c => c.ReceiptAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();
                            if (_receipt.DocCurrency != _receipt.BaseCurrency && _receipt.BaseCurrency == _receipt.BankChargesCurrency)
                                amt = Math.Round((decimal)amt * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            if (_receipt.DocCurrency == _receipt.BaseCurrency && _receipt.DocCurrency != _receipt.BankChargesCurrency)
                                amt = Math.Round((decimal)amt / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            FillDetailOffsetDetail(jmodel, DocTypeConstants.BillCreditMemo, _receipt, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);
                        }
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
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
                //        if (section.Ziraff[i].Name == "IdentityWorkflow")
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
                var message = ex.Message;
            }
        }
        #endregion
    }
}

