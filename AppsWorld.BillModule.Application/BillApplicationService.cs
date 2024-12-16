using AppaWorld.Bean;
using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.Infra;
using AppsWorld.BillModule.Infra.Resources;
using AppsWorld.BillModule.Models;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.BillModule.Service;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Logger;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Ziraff.FrameWork.Logging;
namespace AppsWorld.BillModule.Application
{
    public class BillApplicationService
    {
        private readonly IBillService _billService;
        private readonly ITermsOfPaymentService _termsOfPaymentService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly ICompanyService _companyService;
        private readonly IGSTSettingService _gstSettingService;
        private readonly ICurrencyService _currencyService;
        private readonly IControlCodeService _controlCodeService;
        private readonly IControlCodeCategoryService _controlCodeCategoryService;
        private readonly IBillDetailService _billDetailService;
        private readonly ITaxCodeService _taxCodeService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IMultiCurrencySettingService _multiCurrencySettingService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IPaymentDetailService _paymentDetailService;
        private readonly IPaymentService _paymentService;
        private readonly ICreditMemoService _creditMemoService;
        private readonly ICreditMemoApplicationService _creditMemoApplicationService;
        private readonly ICreditMemoApplicationDetailService _creditMemoApplicationDetailService;
        private readonly AppsWorld.CommonModule.Service.IAccountTypeService _accountTypeService;
        private readonly ILocalizationService _localizationService;
        private readonly IBillModuleUnitOfWorkAsync _unitOfWorkAsync;
        private readonly CommonApplicationService _commonApplicationService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        private readonly IJournalService _journalService;
        private readonly IJournalDetailService _journalDetailService;
        private readonly ICommonForexService _commonForexService;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string query = string.Empty;

        #region Constructor region
        public BillApplicationService(IBillService billService, ITermsOfPaymentService termsOfPaymentService, IFinancialSettingService financialSettingService, IMultiCurrencySettingService multiCurrencySettingService, IBeanEntityService beanEntityService, ICompanyService companyService, IGSTSettingService gstSettingService, ICurrencyService currencyService, IControlCodeService controlCodeService, IControlCodeCategoryService controlCodeCategoryService, IBillDetailService billDetailService, ITaxCodeService taxCodeService, IChartOfAccountService chartOfAccountService, IAutoNumberService autoNumberService, IAutoNumberCompanyService autoNumberCompanyService, IBillModuleUnitOfWorkAsync unitOfWorkAsync, IPaymentDetailService paymentDetailService, IPaymentService paymentService, ICreditMemoApplicationService creditMemoApplicationService, ICreditMemoApplicationDetailService creditMemoApplicationDetailService, ICreditMemoService creditMemoService, ILocalizationService localizationService, AppsWorld.CommonModule.Service.IAccountTypeService accountTypeService, CommonApplicationService commonApplicationService, AppsWorld.CommonModule.Service.IAutoNumberService autoService, IJournalService journalService, IJournalDetailService journalDetailService, ICommonForexService commonForexService)
        {
            _billService = billService;
            _unitOfWorkAsync = unitOfWorkAsync;
            _termsOfPaymentService = termsOfPaymentService;
            _beanEntityService = beanEntityService;
            _companyService = companyService;
            _gstSettingService = gstSettingService;
            _currencyService = currencyService;
            _controlCodeService = controlCodeService;
            _controlCodeCategoryService = controlCodeCategoryService;
            _billDetailService = billDetailService;
            _taxCodeService = taxCodeService;
            _chartOfAccountService = chartOfAccountService;
            _financialSettingService = financialSettingService;
            _multiCurrencySettingService = multiCurrencySettingService;
            _autoNumberService = autoNumberService;
            _paymentDetailService = paymentDetailService;
            _paymentService = paymentService;
            _creditMemoApplicationService = creditMemoApplicationService;
            _creditMemoApplicationDetailService = creditMemoApplicationDetailService;
            _creditMemoService = creditMemoService;
            _localizationService = localizationService;
            _accountTypeService = accountTypeService;
            _commonApplicationService = commonApplicationService;
            _autoService = autoService;
            _journalService = journalService;
            _journalDetailService = journalDetailService;
            _commonForexService = commonForexService;
        }
        #endregion Constructor

        #region Create and Lookup Calls
        public List<AppsWorld.CommonModule.Infra.LookUp<string>> VendorLu(Guid EntityId, long companyId)
        {
            List<AppsWorld.CommonModule.Infra.LookUp<string>> VendorrypeLU = new List<AppsWorld.CommonModule.Infra.LookUp<string>>();
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_VendorLu_LookUPCall_Request_Message);
                var bean = _beanEntityService.GetByEntityId(EntityId, companyId);

                if (bean != null)
                {
                    //string vendor = bean.VendorType;
                    if (bean.VendorType != null)
                    {
                        string[] lstvendor = bean.VendorType.Split(",".ToCharArray(),
                            StringSplitOptions.RemoveEmptyEntries);
                        List<ControlCode> lstCodes = new List<ControlCode>();
                        foreach (var vendor in lstvendor)
                        {
                            var controlcategory = _controlCodeCategoryService.GetByCategoryCode(companyId, "VendorType");
                            if (controlcategory != null)
                            {
                                var contrcode = _controlCodeService.GetControlCode(controlcategory.Id, vendor);
                                lstCodes.Add(contrcode);
                            }
                        }
                        VendorrypeLU = lstCodes.Select(x => new AppsWorld.CommonModule.Infra.LookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.CodeKey,
                            Name = x.CodeValue,
                            RecOrder = x.RecOrder
                        }).OrderBy(x => x.Name).ToList();
                    }
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_VendorLu_LookUPCall_SuccessFully_Message);
                }
            }

            catch (Exception ex)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
                throw ex;
            }
            return VendorrypeLU;
        }

        public List<Bill> GetAllBills(long companyId)
        {
            return _billService.GetAllBillModel(companyId);
        }

        public IQueryable<BillModel> GetAllBillModel(long companyId)
        {
            List<BillModel> lstBill = new List<BillModel>();
            IQueryable<Bill> bills = _billService.GetAllBills(companyId);
            foreach (Bill bill in bills)
            {
                BillModel billModel = new BillModel();
                FillBillModel(billModel, bill, false);
                lstBill.Add(billModel);
            }
            return lstBill.AsQueryable();
        }

        public BillDetailModel GetAllBillDetailModel(Guid billId, Guid billDetailId)
        {
            BillDetailModel billDModel = new BillDetailModel();
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillDetailModel_GetById_Request_Message);
                BillDetail bdetail = new BillDetail();
                // BillCreditMemoDetailModel billCreditMemoDetail = new BillCreditMemoDetailModel();
                bdetail = _billDetailService.CreateBillDetail(billId, billDetailId);
                if (bdetail != null)
                {
                    FillBillDetailModel(billDModel, bdetail);
                }
                else
                {
                    //billDModel.BeanCOA.COAId = 
                    billDModel.BillId = billId;
                    //billDModel.BillCreditMemoDetailModel = null;
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillDetailModel_GetById_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);

                throw ex;
            }

            return billDModel;
        }

        public BillLU GetAllBillLUs(string userName, Guid billId, long companyId, string docType)
        {
            BillLU billLu = new BillLU();
            try
            {
                Bill lastBill = _billService.CreateBill(companyId);
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillLUs_LookUPCall_Request_Message);
                Bill bill = _billService.GetBillById(billId, companyId, docType);
                DateTime date = bill == null ? lastBill == null ? DateTime.Now : lastBill.PostingDate : bill.PostingDate;
                List<BillDetail> lstBD = _billDetailService.GetAllBillDetailModel(billId);
                //billLu.NatureLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_Nature);
                //billLu.VendorTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_codes_VendorType);
                //billLu.AllowableNonallowableLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                billLu.CompanyId = companyId;
                if (bill != null)
                {

                    billLu.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, bill.DocCurrency, ControlCodeConstants.Currency_DefaultCode);
                    //var lookUp = _controlCodeCategoryService.GetInactiveControlcode(companyId,
                    //       ControlCodeConstants.Control_codes_VendorType, bill.VendorType);
                    //if (lookUp != null)
                    //{
                    //    billLu.VendorTypeLU.Lookups.Add(lookUp);
                    //}

                    //var lookUpNature = _controlCodeCategoryService.GetInactiveControlcode(companyId,
                    //       ControlCodeConstants.Control_Codes_Nature, bill.Nature);
                    //if (lookUpNature != null)
                    //{
                    //    billLu.NatureLU.Lookups.Add(lookUpNature);
                    //}
                }
                else
                {
                    billLu.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
                }

                // receiptLU.ModeOfReceiptLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyid, ControlCodeConstants.Control_codes_ModeOfReceipt);
                long comp = bill == null ? 0 : bill.CompanyId == null ? 0 : bill.CompanyId;
                List<Company> lstCompanies = _companyService.GetCompany(companyId, comp, userName);
                billLu.SubsideryCompanyLU = lstCompanies.Select(x => new LookUpCompany<string>()
                {
                    Id = x.Id,
                    Name = x.Name,
                    isGstActivated = x.IsGstSetting,
                    ShortName = x.ShortName
                }).ToList();
                #region commented_code
                //List<TaxCode> lstTaxcodes = _taxCodeService.GetTaxCodeNew(companyId, date);
                //foreach (var taxcode in lstTaxcodes)
                //{
                //    if (taxcode.Code == "OS" || taxcode.Code == "ES33" || taxcode.Code == "ESN33" || taxcode.Code == "NR" || taxcode.Code == "EP" || taxcode.Code == "OP" || taxcode.Code == "Fr")
                //    {
                //        taxcode.TaxRate = 0;
                //    }
                //}
                //billLu.TaxCodeLU = lstTaxcodes.Select(x => new TaxCodeLookUp<string>()
                //{
                //    Id = x.Id,
                //    //Code = (x.Code + "-" + x.Name),
                //    Code = x.Code,
                //    Name = x.Name,
                //    TaxRate = x.TaxRate,
                //    TaxType = x.TaxType,
                //    TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code,
                //}).OrderBy(c => c.Code).ToList();


                //if (lstBD.Count > 0)
                //{

                //    billLu.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyId);
                //    billLu.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, true);
                //}
                //else
                //{
                //    //List<ChartOfAccount> lstCOA = _chartOfAccountService.GetCOANew(companyId);
                //    //billLu.ChartOfAccountLU = lstCOA.Select(x => new AppsWorld.CommonModule.Infra.LookUp<string>()
                //    //{
                //    //    Id = x.Id,
                //    //    Code = x.Code,
                //    //    Name = x.Name
                //    //}).OrderBy(c => c.Name).ToList();
                //    billLu.TaxCodeLU = _taxCodeService.Listoftaxes(date, false, companyId);
                //    billLu.ChartOfAccountLU = _chartOfAccountService.listOfChartOfAccounts(companyId, false);
                //}
                #endregion


                #region  Commented 
                //List<COALookup<string>> lstEditCoa = null;
                //List<TaxCodeLookUp<string>> lstEditTax = null;
                //List<string> coaName = new List<string> { COANameConstants.Direct_costs, COANameConstants.General_and_admin_expenses, COANameConstants.Interest_expense, COANameConstants.Rounding, COANameConstants.Interest_income, COANameConstants.Operating_expenses, COANameConstants.Other_expenses, COANameConstants.Other_income, COANameConstants.Sales_and_marketing_expenses };
                //List<long> accType = _accountTypeService.GetAllAccounyTypeByName(companyId, coaName);
                //List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountType(accType);
                //billLu.ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
                //{
                //    Name = x.Name,
                //    Id = x.Id,
                //    RecOrder = x.RecOrder,
                //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                //    Class = x.Class
                //}).ToList();
                //List<TaxCode> allTaxCodes = _taxCodeService.GetTaxCodes(0);
                //if (allTaxCodes.Any())
                //    billLu.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                //    {
                //        Id = x.Id,
                //        Code = x.Code,
                //        Name = x.Name,
                //        TaxRate = x.TaxRate,
                //        TaxType = x.TaxType,
                //        Status = x.Status,
                //        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                //    }).OrderBy(c => c.Code).ToList();
                //if (bill != null && bill.BillDetails.Count > 0)
                //{
                //    List<long> CoaIds = bill.BillDetails.Select(c => c.COAId).ToList();
                //    List<long?> taxIds = bill.BillDetails.Select(x => x.TaxId).ToList();

                //    if (CoaIds.Any())
                //        lstEditCoa = chartofaAccount.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                //        {
                //            Name = x.Name,
                //            Id = x.Id,
                //            RecOrder = x.RecOrder,
                //            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                //            IsPLAccount = x.Category == "Income Statement" ? true : false,
                //            Class = x.Class
                //        })/*.ToList())*/.ToList();
                //    billLu.ChartOfAccountLU.AddRange(lstEditCoa);
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
                //        billLu.TaxCodeLU.AddRange(lstEditTax);
                //    }
                //}
                #endregion Commented 

                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<string> coaName = new List<string> { COANameConstants.Direct_costs, COANameConstants.General_and_admin_expenses, COANameConstants.Interest_expense, COANameConstants.Rounding, COANameConstants.Interest_income, COANameConstants.Operating_expenses, COANameConstants.Other_expenses, COANameConstants.Other_income, COANameConstants.Sales_and_marketing_expenses, COANameConstants.Taxation };
                List<AppsWorld.CommonModule.Entities.AccountType> accType = _accountTypeService.GetAllAccounyTypeByName(companyId, coaName);
                List<COALookup<string>> lstCoas = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status
                }).OrderBy(d => d.Name)).ToList();
                billLu.ChartOfAccountLU = lstCoas.OrderBy(s => s.Name).ToList();
                //List<TaxCode> allTaxCodes = _taxCodeService.GetTaxCodes(0);
                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    billLu.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (bill != null && bill.BillDetails.Count > 0)
                {

                    List<long> CoaIds = bill.BillDetails.Select(c => c.COAId).ToList();
                    if (billLu.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(billLu.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = bill.BillDetails.Select(x => x.TaxId).ToList();
                    if (billLu.TaxCodeLU.Any())
                        taxIds = taxIds.Except(billLu.TaxCodeLU.Select(d => d.Id)).ToList();
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
                            Status = x.Status
                        }).ToList().OrderBy(d => d.Name)).ToList();
                        billLu.ChartOfAccountLU.AddRange(lstEditCoa);

                    }
                    if (bill.DocSubType == DocTypeConstants.OpeningBalance)
                    {
                        var chartOfAccount = _chartOfAccountService.GetByName("Opening balance", companyId);
                        if (chartOfAccount != null)
                        {
                            List<COALookup<string>> lstOBCoa = new List<COALookup<string>>() { new COALookup<string>() { Name=chartOfAccount.Name,Code=chartOfAccount.Code,Id=chartOfAccount.Id,RecOrder=chartOfAccount.RecOrder,IsAllowDisAllow=chartOfAccount.DisAllowable==true?true:false,IsPLAccount=chartOfAccount.Category=="Income Statement"?true:false,Class=chartOfAccount.Class,Status=chartOfAccount.Status
                                } }.ToList();
                            billLu.ChartOfAccountLU.AddRange(lstOBCoa);
                        }
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
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
                        }).OrderBy(c => c.Code).ToList();
                        billLu.TaxCodeLU.AddRange(lstEditTax);
                        var data = billLu.TaxCodeLU;
                        billLu.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                    }
                    #region commented_code
                    //List<long> CoaIds = bill.BillDetails.Select(c => c.COAId).ToList();
                    //List<long> taxIds = bill.BillDetails.Select(x => x.TaxId.Value).ToList();



                    ////creditLU.ChartOfAccountLU.Where(c => c.Id == CoaIds.Contains());
                    //if (CoaIds.Any())
                    //{
                    //    List<long> lstcoaId = billLu.ChartOfAccountLU.Select(c => c.Id).ToList().Intersect(CoaIds.ToList()).ToList();
                    //    var coalst = lstcoaId.Except(billLu.ChartOfAccountLU.Select(x => x.Id));
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
                    //        billLu.ChartOfAccountLU.AddRange(lstEditCoa);
                    //    }
                    //}


                    ////var common = creditLU.ChartOfAccountLU.Intersect(lstEditCoa.Select(x=>x.Id));


                    //if (taxIds.Any())
                    //{
                    //    List<long> lsttaxId = billLu.TaxCodeLU.Select(d => d.Id).ToList().Intersect(taxIds.ToList()).ToList();
                    //    var taxlst = lsttaxId.Except(billLu.TaxCodeLU.Select(x => x.Id));
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
                    //        billLu.TaxCodeLU.AddRange(lstEditTax);
                    //    }
                    //}
                    //---------------------------
                    //List<long> CoaIds = bill.BillDetails.Select(c => c.COAId).ToList();
                    //List<long?> taxIds = bill.BillDetails.Select(x => x.TaxId).ToList();
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
                    //billLu.ChartOfAccountLU.AddRange(lstEditCoa);
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
                    //    billLu.TaxCodeLU.AddRange(lstEditTax);
                    //}
                    #endregion


                }
                #region commented_code
                //if (bill == null)
                //{
                //List<LookUpCategory<string>> segments = _SegmentMasterService.GetSegments(companyId);
                //if (bill == null)
                //{
                //    if (segments.Count > 0)
                //        billLu.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        billLu.SegmentCategory2LU = segments[1];
                //    // invoiceLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyid);
                //}
                //else
                //{
                //    if (bill.SegmentMasterid1 != null)
                //        billLu.SegmentCategory1LU = _SegmentMasterService.GetSegmentsEdit(companyId, bill.SegmentMasterid1);
                //    else
                //        if (segments.Count > 0)
                //        billLu.SegmentCategory1LU = segments[0];
                //    if (bill.SegmentMasterid2 != null)
                //        billLu.SegmentCategory2LU = _SegmentMasterService.GetSegmentsEdit(companyId, bill.SegmentMasterid2);
                //    else
                //        if (segments.Count > 1)
                //        billLu.SegmentCategory2LU = segments[1];
                //}
                //if (bill != null)
                //{
                //    if (bill.SegmentMasterid1 != null)
                //        billLu.SegmentCategory1LU = _SegmentMasterService.GetSegmentsEdit(companyId, bill.SegmentMasterid1);
                //    if (bill.SegmentMasterid2 != null)
                //        billLu.SegmentCategory2LU = _SegmentMasterService.GetSegmentsEdit(companyId, bill.SegmentMasterid2);
                //}
                //else
                //{
                //    List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _SegmentMasterService.GetSegments(companyId);
                //    if (segments.Count > 0)
                //        billLu.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        billLu.SegmentCategory2LU = segments[1];
                //}
                //}
                //else
                //{
                //LookUpCategory<string> segments1 = _SegmentMasterService.GetSegmentsEdit(companyId, bill.SegmentMasterid1);
                //if (segments1 != null)
                //	billLu.SegmentCategory1LU = segments1;
                //LookUpCategory<string> segments2 = _SegmentMasterService.GetSegmentsEdit(companyId, bill.SegmentMasterid2);
                //if (segments2 != null)
                //	billLu.SegmentCategory2LU = segments2;
                //}
                #endregion

                if (bill == null)
                {

                    List<TermsOfPayment> lstTerm = _termsOfPaymentService.TOPVendorLUNew(companyId);
                    billLu.TermsOfPaymentLU = lstTerm.Select(x => new AppsWorld.CommonModule.Infra.LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();

                }
                else
                {
                    long credit = bill == null ? 0 : bill.CreditTermsId == null ? 0 : bill.CreditTermsId.Value;
                    List<TermsOfPayment> lstTerm = _termsOfPaymentService.TOPVendorLUEdit(credit, companyId);
                    billLu.TermsOfPaymentLU = lstTerm.Select(x => new AppsWorld.CommonModule.Infra.LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillLUs_LookUPCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
                throw ex;
            }
            return billLu;
        }

        public BillModel CreateBill(string userName, Guid id, long companyId, string cursorType, string docType, bool isCopy)
        {
            BillModel billmodel = new BillModel();
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_CreateBill_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new InvalidOperationException(CommonConstant.The_Financial_setting_should_be_activated);
                }
                billmodel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                billmodel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                billmodel.PeriodLockPassword = financSettings.PeriodLockDatePassword;
                Bill bill = _billService.GetBillById(companyId, id);
                billmodel.IsDocNoEditable = true;
                Bill lastbill = _billService.CreateBill(companyId);
                DateTime date = bill == null ? lastbill?.DocumentDate ?? DateTime.Now : bill.DocumentDate;
                if (bill != null)
                {
                    FillBillModel(billmodel, bill, isCopy);
                    billmodel.IsLocked = bill.IsLocked;
                    billmodel.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, bill.DocCurrency, ControlCodeConstants.Currency_DefaultCode);
                }
                else
                {
                    billmodel.Id = Guid.NewGuid();
                    billmodel.BaseCurrency = financSettings.BaseCurrency;
                    billmodel.DocCurrency = billmodel.BaseCurrency;
                    billmodel.CreatedDate = DateTime.UtcNow;
                    billmodel.CompanyId = companyId;
                    billmodel.DocDate = lastbill == null ? DateTime.Now : lastbill.DocumentDate;
                    billmodel.DocNo = null;
                    billmodel.DocSubType = DocTypeConstants.General;
                    billmodel.DocumentState = BillNoteState.NotPaid;
                    billmodel.PostingDate = lastbill == null ? DateTime.Now : lastbill.PostingDate;
                    billmodel.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);

                }

                //modified_code
                long comp = bill == null ? 0 : bill.ServiceCompanyId;
                List<Company> lstCompanies = _companyService.GetCompany(companyId, comp, userName);
                billmodel.SubsideryCompanyLU = lstCompanies.Select(x => new LookUpCompany<string>()
                {
                    Id = x.Id,
                    Name = x.Name,
                    isGstActivated = x.IsGstSetting,
                    ShortName = x.ShortName
                }).OrderBy(x => x.ShortName).ToList();

                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<string> coaName = new List<string> { /*COANameConstants.Revenue,*/ COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };

                List<AppsWorld.CommonModule.Entities.AccountType> accType = _accountTypeService.GetAllAccountTypeNameByCompanyId(companyId, coaName);
                List<COALookup<string>> lstCoas = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable,
                    IsPLAccount = x.Category == "Income Statement",
                    Class = x.Class,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                    Status = x.Status
                }).OrderBy(d => d.Name)).ToList();
                billmodel.ChartOfAccountLU = lstCoas.OrderBy(s => s.Name).ToList();
                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    billmodel.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        IsTaxAmountEditable = !(x.TaxRate == 0 || x.TaxRate == null),
                        TaxType = x.TaxType,
                        Status = x.Status,
                        IsApplicable = x.IsApplicable,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (bill != null && bill.BillDetails.Count > 0)
                {

                    List<long> CoaIds = bill.BillDetails.Select(c => c.COAId).ToList();
                    if (billmodel.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(billmodel.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = bill.BillDetails.Select(x => x.TaxId).ToList();
                    if (billmodel.TaxCodeLU != null && billmodel.TaxCodeLU.Any())
                        taxIds = taxIds.Except(billmodel.TaxCodeLU.Select(d => d.Id)).ToList();
                    if (CoaIds.Any())
                    {
                        lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable,
                            IsPLAccount = x.Category == "Income Statement",
                            Class = x.Class,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                            Status = x.Status
                        }).OrderBy(d => d.Name)).ToList();
                        billmodel.ChartOfAccountLU.AddRange(lstEditCoa);

                    }
                    if (bill.DocSubType == DocTypeConstants.OpeningBalance)
                    {
                        var chartOfAccount = _chartOfAccountService.GetByName("Opening balance", companyId);
                        if (chartOfAccount != null)
                        {
                            List<COALookup<string>> lstOBCoa = new List<COALookup<string>>() { new COALookup<string>() { Name=chartOfAccount.Name,Code=chartOfAccount.Code,Id=chartOfAccount.Id,RecOrder=chartOfAccount.RecOrder,IsAllowDisAllow=chartOfAccount.DisAllowable,IsPLAccount=chartOfAccount.Category=="Income Statement",Class=chartOfAccount.Class,Status=chartOfAccount.Status
                                } }.ToList();
                            billmodel.ChartOfAccountLU.AddRange(lstOBCoa);
                        }
                    }
                    if (bill.IsGstSettings && taxIds.Any())
                    {
                        if (bill.DocSubType != DocTypeConstants.Claim)
                        {
                            lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                            {
                                Id = x.Id,
                                Code = x.Code,
                                Name = x.Name,
                                TaxRate = x.TaxRate,
                                IsTaxAmountEditable = !(x.TaxRate == 0 || x.TaxRate == null),
                                TaxType = x.TaxType,
                                Status = x.Status,
                                IsApplicable = x.IsApplicable,
                                TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA")/* + "(" + x.TaxType[0] + ")"*/ : x.Code
                            }).OrderBy(c => c.Code).ToList();
                            if (billmodel.TaxCodeLU != null)
                            {
                                billmodel.TaxCodeLU.AddRange(lstEditTax);
                                billmodel.TaxCodeLU = billmodel.TaxCodeLU.OrderBy(c => c.Code).ToList();
                            }
                        }
                        else
                        {
                            List<TaxCode> taxCodes = _taxCodeService.GetTaxAllCodesByIds(bill.BillDetails.Select(c => c.TaxId).ToList());
                            if (taxCodes.Any())
                            {
                                billmodel.TaxCodeLU = taxCodes.Select(x => new TaxCodeLookUp<string>()
                                {
                                    Id = x.Id,
                                    Code = x.Code,
                                    Name = x.Name,
                                    TaxRate = x.TaxRate,
                                    IsTaxAmountEditable = !(x.TaxRate == 0 || x.TaxRate == null),
                                    TaxType = x.TaxType,
                                    Status = x.Status,
                                    IsApplicable = x.IsApplicable,
                                    TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA")/* + "(" + x.TaxType[0] + ")"*/ : x.Code
                                }).OrderBy(c => c.Code).ToList();
                            }
                        }
                    }
                }
                if (bill == null)
                {
                    List<TermsOfPayment> lstTerm = _termsOfPaymentService.TOPVendorLUNew(companyId);
                    billmodel.TermsOfPaymentLU = lstTerm.Select(x => new AppsWorld.CommonModule.Infra.LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();

                }
                else
                {
                    long credit = bill == null ? 0 : bill?.CreditTermsId ?? 0;
                    List<TermsOfPayment> lstTerm = _termsOfPaymentService.TOPVendorLUEdit(credit, companyId);
                    billmodel.TermsOfPaymentLU = lstTerm.Select(x => new AppsWorld.CommonModule.Infra.LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_CreateBill_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
                throw ex;
            }
            return billmodel;

        }

        public DocumentVoidModel CreateBillDocumentVoid(Guid id, long companyId)
        {
            DocumentVoidModel DVModel = new DocumentVoidModel();
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_CreateBillDocumentVoid_CreateCall_Request_Message);
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);

                Bill bill = _billService.GetBillById(id, companyId, DocTypeConstants.General);
                if (bill == null)
                    throw new Exception("Invalid bill");
                if (bill != null)
                {
                    DVModel.CompanyId = companyId;
                    DVModel.Id = bill.Id;
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_CreateBillDocumentVoid_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
                throw ex;
            }
            return DVModel;
        }

        #endregion

        #region Save Bill and PayrollBill
        public Bill SaveBill(BillModel Tobject, string ConnectionString)
        {
            bool isNewAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(Tobject));
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, "ObjectSave", AdditionalInfo);
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_into_SaveBill_method);
                string _errors = CommonValidation.ValidateObject(Tobject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }

                if (Tobject.EntityId == null)
                {
                    throw new InvalidOperationException(CommonConstant.Entity_is_mandatory);
                }

                //to check if it is void or not
                if (_billService.IsVoid(Tobject.CompanyId, Tobject.Id))
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                string baseCurrency = _localizationService.GetLocalization(Tobject.CompanyId);
                if (Tobject.DocSubType == DocTypeConstants.PayrollBill && (baseCurrency != Tobject.BaseCurrency))
                {
                    throw new InvalidOperationException(BillConstant.Please_enter_the_valid_Base_Currency);
                }

                if (Tobject.DocDate == null)
                {
                    throw new InvalidOperationException(CommonConstant.Invalid_Document_Date);
                }

                if (Tobject.DueDate == null || Tobject.DueDate < Tobject.DocDate)
                {
                    throw new InvalidOperationException(BillConstant.Invalid_Due_Date);
                }
                if (Tobject.ServiceCompanyId == 0)
                    throw new InvalidOperationException(CommonConstant.ServiceCompany_is_mandatory);
                if (Tobject.DocSubType != "Opening Bal" && Tobject.CreditTermId == null)
                    throw new InvalidOperationException(BillConstant.Terms_Payment_is_mandatory);

                if (Tobject.IsDocNoEditable == true && IsDocumentNumberExists(DocTypeConstants.Bills, Tobject.DocNo, Tobject.Id, Tobject.CompanyId, Tobject.EntityId, Tobject.DocSubType))
                {
                    throw new InvalidOperationException(CommonConstant.Document_number_already_exist);
                }

                if (Tobject.BillDetailModels == null || Tobject.BillDetailModels.Count == 0 || !Tobject.BillDetailModels.Any())
                {
                    throw new InvalidOperationException(BillConstant.Atleast_one_Sale_Item_is_required_in_the_Bill);
                }
                if (Tobject.ExchangeRate == 0)
                    throw new InvalidOperationException(CommonConstant.ExchangeRate_Should_Be_Grater_Than_0);
                if (Tobject.GstExchangeRate == 0 && Tobject.IsGstSettings)
                    throw new InvalidOperationException(CommonConstant.GSTExchangeRate_Should_Be_Grater_Than_0);
                if (Tobject.DocSubType == DocTypeConstants.PayrollBill && Tobject.GrandTotal == 0)
                    return new Bill();

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                var financialSetting = _financialSettingService.GetFinancialSettingNew(Tobject.CompanyId);
                if (Tobject.DocSubType != DocTypeConstants.OpeningBalance && !Tobject.IsFromPayroll && financialSetting != null && !Tobject.IsFromPeppol)
                {
                    if (!ValidateFinancialOpenPeriodNew(Tobject.PostingDate, financialSetting))
                    {
                        if (String.IsNullOrEmpty(Tobject.PeriodLockPassword))
                        {
                            throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                        }
                        else if (financialSetting.Item3 != Tobject.PeriodLockPassword)
                        {
                            throw new InvalidOperationException(CommonConstant.Invalid_Financial_Period_Lock_Password);
                        }
                    }
                }
                if (!Tobject.IsFromPayroll && Tobject.BillDetailModels != null && Tobject.BillDetailModels.Count > 0)
                {
                    if (Tobject.BillDetailModels.Any(a => a.RecordStatus != "Deleted" && a.DocAmount == 0))
                        throw new InvalidOperationException(CommonConstant.DocCurrencyAmount_Should_Be_Grater_Than_0);
                }
                Bill _bill = null;
                string oldDocumentNo = null;
                if (Tobject.Nature == "Interco")
                {
                    using (var con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        var query = "UPDATE Bean.Bill SET DocDescription = @DocDescription, ModifiedBy = @ModifiedBy, ModifiedDate = GETUTCDATE() WHERE Id = @Id AND CompanyId = @CompanyId";
                        using (var cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@DocDescription", Tobject.DocDescription);
                            cmd.Parameters.AddWithValue("@ModifiedBy", Tobject.ModifiedBy);
                            cmd.Parameters.AddWithValue("@Id", Tobject.Id);
                            cmd.Parameters.AddWithValue("@CompanyId", Tobject.CompanyId);

                            cmd.ExecuteNonQuery();
                        }
                        using (var cmd = new SqlCommand("Bean_Insert_Document_History", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CompanyId", Tobject.CompanyId.ToString());
                            cmd.Parameters.AddWithValue("@DocumentId", Tobject.ToString());
                            cmd.Parameters.AddWithValue("@DocumentType", Tobject.DocType);
                            cmd.Parameters.AddWithValue("@IsVoid", false);

                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    _bill = _billService.GetByAllBillDetails(Tobject.Id, Tobject.DocSubType);
                }

                else
                {
                    _bill = _billService.GetByAllBillDetails(Tobject.Id, Tobject.DocSubType);
                    if (Tobject.DocSubType == DocTypeConstants.Claim && _bill == null)
                        Tobject.EntityId = _beanEntityService.GetExternalEntity(Tobject.EntityId);
                    if (_bill != null)
                    {
                        if (_bill.ExchangeRate != Tobject.ExchangeRate)
                            _bill.roundingamount = 0;
                        oldDocumentNo = _bill.DocNo;

                        //Data Concurrancy check
                        string timeStamp = "0x" + string.Concat(Array.ConvertAll(_bill.Version, x => x.ToString("X2")));
                        if (!timeStamp.Equals(Tobject.Version))
                            throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                        isNewAdd = false;
                        decimal? original = 0;

                        if (_bill.DocumentState == "Partial Paid")
                        {
                            original = (_bill.GrandTotal) - (_bill.BalanceAmount);
                            if (Tobject.GrandTotal < original)
                            {
                                throw new InvalidOperationException("Grand Total Should Be Grater Than Or Equal To Payment Amount");
                            }
                        }
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_into_if_condition_of_Journal_and_check_Journal_is_null_or_not);
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Insert_Bill);
                        if (_bill.GrandTotal < Tobject.GrandTotal)
                        {
                            original = Tobject.GrandTotal - _bill.GrandTotal;
                            _bill.BalanceAmount += original;
                        }
                        else if (_bill.GrandTotal > Tobject.GrandTotal)
                        {
                            original = _bill.GrandTotal - Tobject.GrandTotal;
                            _bill.BalanceAmount -= original;
                        }
                        InsertBill(Tobject, _bill);
                        _bill.DocNo = Tobject.DocNo;
                        _bill.SystemReferenceNumber = _bill.DocNo;
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Insert_Bill_Method);
                        _bill.ModifiedBy = Tobject.ModifiedBy;
                        _bill.ModifiedDate = DateTime.UtcNow;
                        _bill.ObjectState = ObjectState.Modified;
                        _billService.Update(_bill);
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Update_Bill_Details_Method);
                        UpdateBillDetails(Tobject, _bill);
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Update_Bill_Details);
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Come Out From Save Bill");
                    }
                    else
                    {
                        isNewAdd = true;
                        _bill = new Bill();
                        int? recOreder = 0;
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Insert_Bill);
                        InsertBill(Tobject, _bill);
                        _bill.BalanceAmount = Tobject.GrandTotal;
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Insert_Bill);
                        _bill.Id = Tobject.Id;
                        _bill.DocumentState = BillNoteState.NotPaid;
                        if (Tobject.BillDetailModels.Count > 0 || Tobject.BillDetailModels != null)
                        {
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_If_Condition_And_Check_Tobject_BillDetail_Modules_Count_And_Bill_Details_Module);
                            List<BillDetail> lstBillDetail = new List<BillDetail>();
                            List<TaxCode> lstTaxCode = _taxCodeService.GetTaxAllCodesByIds(Tobject.BillDetailModels.Select(c => c.TaxId).ToList());
                            lstBillDetail.AddRange(Tobject.BillDetailModels.Select(detail => new BillDetail
                            {
                                Id = Guid.NewGuid(),
                                BillId = _bill.Id,
                                Description = detail.Description,
                                COAId = detail.COAId,
                                IsDisallow = detail.IsDisallow,
                                TaxId = detail.TaxId,
                                TaxIdCode = detail.TaxIdCode,
                                DocAmount = detail.DocAmount,
                                DocTaxAmount = detail.DocTaxAmount,
                                DocTotalAmount = detail.DocAmount + detail.DocTaxAmount,
                                BaseAmount = Tobject.DocCurrency == Tobject.BaseCurrency ? detail.DocAmount : Math.Round(detail.DocAmount * (decimal)_bill.ExchangeRate, 2, MidpointRounding.AwayFromZero),
                                BaseTaxAmount = Tobject.DocCurrency == Tobject.BaseCurrency ? detail.DocTaxAmount : Math.Round(detail.DocTaxAmount * (decimal)_bill.ExchangeRate, 2, MidpointRounding.AwayFromZero),
                                BaseTotalAmount = Tobject.DocCurrency == Tobject.BaseCurrency ? detail.DocTotalAmount : detail.BaseAmount + detail.BaseTaxAmount,
                                TaxCode = Tobject.DocSubType == DocTypeConstants.PayrollBill ? "NA" : detail.TaxCode,
                                TaxRate = detail.TaxRate,
                                TaxType = lstTaxCode.Find(c => c.Id == detail.TaxId) != null ? lstTaxCode.Find(c => c.Id == detail.TaxId).TaxType : string.Empty,
                                IsPLAccount = detail.IsPLAccount,
                                RecOrder = _bill.DocSubType == DocTypeConstants.Claim ? detail.RecOrder : ++recOreder,
                                ObjectState = ObjectState.Added
                            }));
                            if (lstBillDetail.Any())
                                _billDetailService.InsertRange(lstBillDetail);
                        }
                        _bill.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                        _bill.UserCreated = Tobject.UserCreated;
                        _bill.CreatedDate = DateTime.UtcNow;
                        _bill.ObjectState = ObjectState.Added;
                        _bill.SystemReferenceNumber = Tobject.IsDocNoEditable != true ? GenerateAutoNumberForType(_bill.CompanyId, DocTypeConstants.Bill, null, Tobject.DocSubType) : Tobject.DocNo;
                        isDocAdd = true;
                        _bill.DocNo = _bill.SystemReferenceNumber;
                        if (Tobject.DocSubType == DocTypeConstants.Claim)
                        {
                            _bill.SyncHRPayrollId = Tobject.PayrollId;
                            _bill.SyncHRPayrollStatus = BillConstants.Completed;
                            _bill.SyncHRPayrollDate = DateTime.UtcNow;
                            _bill.SyncHRPayrollRemarks = "";
                        }
                        _billService.Insert(_bill);
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Come Out From Save Bill");
                    }
                    try
                    {
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Try_Block_Of_the_SaveCall);
                        _unitOfWorkAsync.SaveChanges();
                        if (_bill.DocSubType == DocTypeConstants.Claim)
                        {
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.HR_bill_status_call_executing);
                            SmartCursorSyncing(_bill.CompanyId, CursorConstants.HRCursor, DocTypeConstants.Claim, Tobject.PayrollId, _bill.Id, BillConstants.Completed, string.Empty, ConnectionString);
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Out_from_HR_bill_status_call_executing);
                        }
                        if (Tobject.DocSubType != "Opening Bal")
                        {
                            #region Bill_Posting_Through_SP

                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand("Bean_Posting", con);
                                cmd.CommandTimeout = 0;
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@SourceId", _bill.Id);
                                cmd.Parameters.AddWithValue("@Type", DocTypeConstants.Bills);
                                cmd.Parameters.AddWithValue("@CompanyId", _bill.CompanyId);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                            #endregion Bill_Posting_Through_SP
                        }
                        try
                        {
                            if (isNewAdd && Tobject.TileAttachments != null && Tobject.TileAttachments.Count > 0)
                            {
                                string name = _beanEntityService.GetEntityName(Tobject.CompanyId, Tobject.EntityId);
                                string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                                string DocuNo = _commonApplicationService.StringCharactersReplaceFunction(Tobject.DocNo);
                                string path = DocumentConstants.Entities + "/" + EntityName + "/" + DocumentConstants.Bills + "/" + DocuNo;
                                SaveTailsAttachments(Tobject.CompanyId, path, Tobject.UserCreated, Tobject.TileAttachments);
                            }
                            #region DocumentFolder Rename
                            if (!isNewAdd && oldDocumentNo != Tobject.DocNo)
                            {
                                string name = _beanEntityService.GetEntityName(Tobject.CompanyId, Tobject.EntityId);
                                string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                                _commonApplicationService.ChangeFolderName(Tobject.CompanyId, Tobject.DocNo, oldDocumentNo, EntityName, "Bills");
                            }
                            #endregion
                        }
                        catch (Exception)
                        {

                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Catch_Of_The_SaveCall);
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
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_SaveBill_SaveCall_Exception_Message);
                        Log.Logger.ZCritical(e.StackTrace);
                        if (_bill.DocSubType == DocTypeConstants.Claim)
                            SmartCursorSyncing(Tobject.CompanyId, CursorConstants.HRCursor, DocTypeConstants.Claim, Tobject.SyncPayrollId, null, BillConstants.Failed, e.Message, ConnectionString);
                        LoggingHelper.LogError(BillConstants.BillApplicationService, e, e.Message);
                        throw e;
                    }
                }
                return _bill;
            }
            catch (Exception ex)
            {
                if (isNewAdd && isDocAdd && Tobject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(Tobject.CompanyId, Tobject.DocSubType == DocTypeConstants.Payroll ? DocTypeConstants.PayrollBills : DocTypeConstants.Bill, ConnectionString);
                }
                throw ex;
            }
        }
        private bool ValidateFinancialOpenPeriodNew(DateTime DocDate, Tuple<DateTime?, DateTime?, string> setting)
        {
            if (setting.Item1 != null && setting.Item2 != null)
            {
                return DocDate.Date >= setting.Item1 && DocDate.Date <= setting.Item2;
            }
            else if (setting.Item1 != null && setting.Item2 == null)
            {
                return DocDate.Date >= setting.Item1;
            }
            else if (setting.Item1 == null && setting.Item2 != null)
            {
                return DocDate.Date <= setting.Item2;
            }
            else
            {
                return true;
            }
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
                //var json = RestHelper.ConvertObjectToJason(tails);
                var json = RestSharpHelper.ConvertObjectToJason(tails);
                try
                {
                    //string url = "http://localhost:64453/";
                    string url = ConfigurationManager.AppSettings["AzureUrl"];
                    var response = RestSharpHelper.ZPost(url, "api/storage/tailsaddmodesave", json);
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

        public void SmartCursorSyncing(long? companyId, string cursor, string doctype, Guid? sourceId, Guid? targetId, string status, string remarks, string ConnectionString)
        {
            if (doctype == DocTypeConstants.Claim || doctype == DocTypeConstants.PayrollBill)
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.HR_bill_status_call_executing);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("SmartCursors_Syncing_Process", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TargetId", targetId);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@SourceId", sourceId);
                    cmd.Parameters.AddWithValue("@Cursor", cursor);
                    cmd.Parameters.AddWithValue("@DocType", doctype);
                    cmd.Parameters.AddWithValue("@Remarks", remarks);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Out_from_HR_bill_status_call_executing);
                }
            }
        }
        //public void SaveBillRelatedFile(BillModel Tobject, string oldDocNo, Guid oldEntityId, bool isAdd, Bill _bill)
        //{
        //    //for DocumentScreenSave
        //    try
        //    {
        //        if (Tobject.DocSubType != DocTypeConstants.Payroll)
        //        {
        //            if (isAdd == true)
        //            {
        //                if (Tobject.DocSubType == DocTypeConstants.Claim && Tobject.IsExternal == true)
        //                {
        //                    //bool isExist = _foldersDataService.isFolderDataExist(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.CompanyId);
        //                    //if (!isExist)
        //                    //{

        //                    //}
        //                    //if (!isFileExist(Tobject.CompanyId, Tobject.EntityId.ToString(), Tobject.EntityId.ToString()))
        //                    //{
        //                    //    //string name=
        //                    //    //saveScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), beDTO.Name, beDTO.UserCreated, isFirst ? beDTO.CreatedDate.Value : beDTO.ModifiedDate, isFirst, beDTO.CompanyId, MasterModuleValidations.Entities, entityId.ToString());
        //                    //}

        //                }
        //                else
        //                    saveScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, isAdd, Tobject.CompanyId, "Bills", Tobject.EntityId.ToString());
        //            }
        //            else
        //            {
        //                if (oldEntityId != Tobject.EntityId)
        //                    saveScreenRecords(Tobject.Id.ToString(), oldEntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, false, Tobject.CompanyId, oldDocNo, oldEntityId.ToString());
        //                if (oldDocNo != Tobject.DocNo)
        //                    saveScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, false, Tobject.CompanyId, oldDocNo, oldEntityId.ToString());
        //            }
        //            if (Tobject.DocSubType != DocTypeConstants.Claim)
        //            {
        //                if (Tobject.TileAttachments != null && Tobject.TileAttachments.Any())
        //                {
        //                    foreach (var fileAttachMent in Tobject.TileAttachments)
        //                    {
        //                        SaveBillScreenFiles(fileAttachMent.FileId, fileAttachMent.Name, fileAttachMent.FileSize, Tobject.EntityId.ToString(), _bill.Id.ToString(), _bill.Id.ToString(), Tobject.CompanyId, fileAttachMent.RecordStatus, fileAttachMent.Description, Tobject.UserCreated, Tobject.ModifiedBy, fileAttachMent.IsSystem);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Issues_in_Bill_Folder_creation);
        //    }
        //}

        private bool SaveScreenFiles(long companyId, string fileName, string billId, string entityId, string fileId, string fileSize, string userCreated, string modifiedBy, string description, string recordStatus)
        {
            RefFilesModel refModel = new RefFilesModel();
            refModel.ReferenceId = billId;
            refModel.RecordId = billId;
            refModel.FeatureId = entityId;
            refModel.FileId = fileId;
            refModel.FileName = fileName;
            refModel.FileSize = fileSize;
            refModel.ModuleName = "Bean Cursor";
            refModel.IsSystem = true;
            refModel.IsMyDrive = false;
            refModel.TabName = "Bills";
            refModel.CreatedBy = userCreated;
            refModel.ModifiedBy = modifiedBy;
            refModel.Description = description;
            refModel.RecordStatus = recordStatus;
            refModel.Description = description;
            var json = RestHelper.ConvertObjectToJason(refModel);
            try
            {
                object obj = refModel;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestHelper.ZPost(url, "api/document/savesscreenfiles", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<RefFilesModel>(response.Content);
                    refModel = data;
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


        private bool SaveBillScreenFiles(string FileId, string fileName, string fileSize, string featureId, string recordId, string referenceId, long companyId, string recordStatus, string desc, string createdBy, string modifiedBy, bool isSystem)
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
            tilesFileVM.IsSystem = isSystem;
            tilesFileVM.ModuleName = "Bean Cursor";
            tilesFileVM.TabName = "Bills";
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



        public List<string> BillMigrationCall(long companyId)
        {
            //Guid entityId=new
            List<Bill> lstBills = _billService.GetListOfBill(companyId);
            //Guid id = new Guid("2BEF3A7D-08A0-4700-9C3B-60ED64B9DA47");
            List<BeanEntity> lstBeanEntites = _beanEntityService.GetListOfEntity(companyId).Where(x => x.IsVendor == true).ToList();
            List<string> lstEntityIds = new List<string>();
            List<ScreenRecordsSave> lstscreenrecords = new List<ScreenRecordsSave>();
            List<Bill> lstBill = null;
            if (lstBeanEntites.Any())
            {
                foreach (var entity in lstBeanEntites)
                {
                    lstscreenrecords = saveScreenRecords1(entity.Id.ToString(), null, entity.Id.ToString(), entity.Name, entity.UserCreated, entity.CreatedDate.Value, true, companyId, "Entities", entity.Id.ToString(), ref lstscreenrecords, entity.Status, DateTime.UtcNow);
                    lstBill = _billService.Query(a => a.EntityId == entity.Id).Select().ToList();
                    List<string> lstBillIds = new List<string>();
                    if (lstBill != null && lstBill.Any())
                    {
                        foreach (var bill in lstBill)
                        {
                            lstscreenrecords = saveScreenRecords1(bill.Id.ToString(), bill.EntityId.ToString(), bill.EntityId.ToString(), bill.DocNo, bill.UserCreated, bill.CreatedDate, true, bill.CompanyId, "Bills", bill.EntityId.ToString(), ref lstscreenrecords, bill.Status, /*bill.ModifiedDate != null ? bill.ModifiedDate.Value : (DateTime?)null*/DateTime.UtcNow);
                        }
                        lstEntityIds.Add(entity.Name.ToString());//to know the no of Entity Created
                    }
                }
                saveAllScreenRecords(lstscreenrecords);
            }
            return lstEntityIds;
        }
        #endregion


        #region Fill Methods

        public bool saveScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldEntityId)
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
            screenRecords.OldFeatureId = oldEntityId;
            screenRecords.CreatedDate = date.Value;
            var json = RestHelper.ConvertObjectToJason(screenRecords);
            try
            {
                object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestHelper.ZPost(url, "api/document/savescreenfolders", json);
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
                var message = ex.Message;
                return false;
            }
        }

        //public bool SaveScreenFiles(string )
        //{
        //    TilesFileVM tilesFileVM = new TilesFileVM();

        //}

        private void FillBillModelk(BillModel billModel, Bill bill)
        {
            try
            {

                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillModelk_FillCall_Request_Message);
                billModel.DocNo = bill.DocNo;
                billModel.PostingDate = bill.PostingDate;
                billModel.DueDate = bill.DueDate;
                billModel.EntityId = bill.EntityId;
                //BeanEntity bes = _beanEntityService.GetEntityById(billModel.EntityId);
                //if (bes != null)
                //    billModel.BeanEntity.EntityName = bes.Name;
                billModel.BaseCurrency = bill.BaseCurrency;
                billModel.GrandTotal = bill.GrandTotal;
                billModel.DocumentState = bill.DocumentState;
                billModel.DocCurrency = bill.DocCurrency;
                billModel.SystemReferenceNumber = bill.SystemReferenceNumber;
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillModelk_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillModelk_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }

        private void FillBillModel(BillModel billModel, Bill bill, bool isCopy)
        {
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillModel_FillCall_Request_Message);
                billModel.Id = isCopy ? Guid.NewGuid() : bill.Id;
                billModel.CompanyId = bill.CompanyId;
                billModel.DocNo = isCopy ? null : bill.DocNo;
                billModel.IsModify = isCopy ? false : bill.ClearCount > 0;
                billModel.DocDate = bill.DocumentDate;
                billModel.PostingDate = bill.PostingDate;
                billModel.DueDate = bill.DueDate;
                billModel.EntityId = bill.EntityId;
                billModel.EntityName = _beanEntityService.GetEntityNameById(bill.EntityId);
                billModel.DocDescription = bill.DocDescription;
                //if (bes != null)
                //    billModel.BeanEntity.EntityName = bes.Name;
                billModel.BaseCurrency = bill.BaseCurrency;
                billModel.ExchangeRate = bill.ExchangeRate;
                billModel.ModifiedBy = isCopy ? null : bill.ModifiedBy;
                billModel.CreatedDate = isCopy ? null : bill.CreatedDate;
                billModel.CreditTermId = bill.CreditTermsId;
                //var ct = _termsOfPaymentService.GetById(billModel.CreditTerm.CreditTermId);
                //if (ct != null)
                //    billModel.CreditTerm.CreditTermName = ct.Name;
                //billModel.ExDurationFrom = bill.ExDurationFrom;
                //billModel.exDurationTo = bill.ExDurationTo;
                billModel.Version = "0x" + string.Concat(Array.ConvertAll(bill.Version, x => x.ToString("X2")));
                billModel.GrandTotal = bill.GrandTotal;
                billModel.ModifiedBy = isCopy ? null : bill.ModifiedBy;
                //billModel.VendorType = bill.VendorType;
                billModel.DocSubType = bill.DocSubType;
                billModel.DocType = bill.DocType;
                billModel.Nature = bill.Nature;
                //billModel.Remarks = bill.Remarks;
                string name = billModel.EntityName != null ? billModel.EntityName : _beanEntityService.GetEntityName(bill.CompanyId, bill.EntityId);
                string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                string DocuNo = _commonApplicationService.StringCharactersReplaceFunction(bill.DocNo);
                billModel.Path = DocumentConstants.Entities + "/" + EntityName + "/" + DocumentConstants.Bills + "/" + DocuNo;
                billModel.Status = bill.Status;
                billModel.UserCreated = isCopy ? null : bill.UserCreated;
                billModel.ModifiedDate = isCopy ? null : bill.ModifiedDate;
                //billModel.ISSegmentReporting = bill.IsSegmentReporting;
                billModel.IsNoSupportingDocument = bill.IsNoSupportingDocument;
                billModel.NoSupportingDocument = bill.NoSupportingDocument;
                billModel.ISMultiCurrency = bill.IsMultiCurrency;
                billModel.GstExchangeRate = bill.GSTExchangeRate;
                billModel.GstReportingCurrency = bill.GSTExCurrency;
                //billModel.ISAllowDisAllow = bill.IsAllowableDisallowable;
                billModel.IsGstSettings = bill.IsGstSettings;
                billModel.ServiceCompanyId = bill.ServiceCompanyId;
                //var cmp = _companyService.GetById(billModel.ServiceCompany.ServiceCompanyId);
                //if (cmp != null)
                //    billModel.ServiceCompany.ServiceCompanyName = cmp.ShortName;
                //billModel.GstDurationTo = bill.GSTExDurationTo;
                //billModel.GstdurationFrom = bill.GSTExDurationFrom;
                billModel.IsBaseCurrencyRateChanged = bill.IsBaseCurrencyRateChanged;
                billModel.IsGSTCurrencyRateChanged = bill.IsGSTCurrencyRateChanged;

                billModel.DocumentState = isCopy ? null : bill.DocumentState;
                billModel.DocCurrency = bill.DocCurrency;
                billModel.IsGSTApplied = bill.IsGSTApplied;
                billModel.BalanceAmount = isCopy ? bill.GrandTotal : bill.BalanceAmount;
                billModel.SyncPayrollId = bill.PayrollId;
                billModel.PayrollId = bill.PayrollId;
                billModel.SystemReferenceNumber = bill.SystemReferenceNumber;
                billModel.OpeningBalanceId = bill.OpeningBalanceId;
                billModel.IsExternal = bill.IsExternal;
                #region commented_code
                //billModel.SegmentCategory.SegmentCategoryName1 = bill.SegmentCategory1;
                //billModel.SegmentCategory.SegmentCategoryName2 = bill.SegmentCategory2;
                //billModel.SegmentCategory.SegmentCategoryId1 = bill.SegmentDetailid1;
                //billModel.SegmentCategory.SegmentMasterId1 = bill.SegmentMasterid1;
                //billModel.SegmentCategory.SegmentCategoryId2 = bill.SegmentDetailid2;
                //billModel.SegmentCategory.SegmentMasterId2 = bill.SegmentMasterid2;

                //billModel.ISGstDeRegistered = _gstSettingService.IsGSTDeregistered(billModel.CompanyId);
                //billModel.SegmentCategory1 = bill.SegmentCategory1;
                //billModel.SegmentCategory2 = bill.SegmentCategory2;
                //if (bill.SegmentMasterid1 != null)
                //{
                //    var segment1 = _SegmentMasterService.GetSegmentMastersById(bill.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    billModel.IsSegmentActive1 = segment1.Status == AppsWorld.Framework.RecordStatusEnum.Active;
                //}
                //if (bill.SegmentMasterid2 != null)
                //{
                //    var segment2 = _SegmentMasterService.GetSegmentMastersById(bill.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    billModel.IsSegmentActive2 = segment2.Status == AppsWorld.Framework.RecordStatusEnum.Active;
                //}


                //BillCreditMemoModel billCreditMemoModel = new BillCreditMemoModel();
                //var billCredit = _billCreditMemoService.GetCreditMemo(billModel.Id);
                //if (billCredit != null)
                //{
                //    List<BillCreditMemoGSTDetail> billCreditMemoGstDetails = new List<BillCreditMemoGSTDetail>();
                //    billCreditMemoModel.Id = billCredit.Id;
                //    billCreditMemoModel.BillId = billCredit.BillId;
                //    billCreditMemoModel.DocumentType = "Credit Memo";
                //    billCreditMemoModel.DocumentDate = billCredit.DocumentDate;
                //    billCreditMemoModel.DocumentNumber = billCredit.DocumentNumber;
                //    billCreditMemoModel.Currency = billCredit.Currency;
                //    billCreditMemoModel.CompanyId = billCredit.CompanyId;
                //    billCreditMemoModel.IsNoSupportingDocument = billCredit.IsNoSupportingDocument;
                //    billCreditMemoModel.NoSupportingDocument = billCredit.NoSupportingDocument;
                //    billCreditMemoModel.Remarks = billCredit.Remarks;
                //    billCreditMemoModel.CreatedDate = billCredit.CreatedDate;
                //    billCreditMemoModel.UserCreated = billCredit.UserCreated;
                //    billCreditMemoModel.ModifiedBy = billCredit.ModifiedBy;
                //    billCreditMemoModel.ModifiedDate = billCredit.ModifiedDate;
                //    billCreditMemoModel.Status = billCredit.Status;
                //    var gstCreditMemo = _billCreditMemoGstDetailService.GetAllCreditMemoGst(billCreditMemoModel.Id);
                //    if (gstCreditMemo.Any())
                //    {
                //        billCreditMemoModel.BillCreditMemoGSTDetails = gstCreditMemo;
                //    }
                //}
                //else
                //{
                //    billCreditMemoModel.DocumentDate = DateTime.UtcNow.Date;
                //    billCreditMemoModel.IsNoSupportingDocument = _CompanySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, bill.CompanyId);
                //    billCreditMemoModel.DocumentNumber = GetNewBillCreditMemoDocumentNumber(billModel.CompanyId);
                //}
                //billModel.BillCreditMemoModel = billCreditMemoModel;
                #endregion

                #region Commented
                //List<BillPaymentModel> lstBillPayNModel = new List<BillPaymentModel>();
                //var paydetails = _paymentDetailService.GetById(bill.Id);
                //if (paydetails.Any())
                //{
                //    foreach (var payment in paydetails)
                //    {
                //        BillPaymentModel billpayModel = new BillPaymentModel();
                //        //var payDetail = _paymentDetailService.GetBillPaymentById(payment.PaymentId);

                //        if (payment != null)
                //        {
                //            var paymentdetail = _paymentService.GetPaymentById(payment.PaymentId);
                //            if (paymentdetail != null)
                //            {
                //                billpayModel.PaymentDetailId = paymentdetail.Id;
                //                billpayModel.DocDate = paymentdetail.DocDate;
                //                billpayModel.DocNo = paymentdetail.DocNo;
                //                billpayModel.SystemRefNo = paymentdetail.SystemRefNo;
                //                if (paymentdetail.DocumentState != BillNoteState.Void)
                //                    billpayModel.Ammount = payment.PaymentAmount;
                //                else
                //                    billpayModel.Ammount = null;
                //                billpayModel.DocType = "Payment";
                //                lstBillPayNModel.Add(billpayModel);
                //            }
                //        }
                //    }
                //    billModel.BillPaymentModels = lstBillPayNModel.OrderBy(c => c.DocDate).ToList();
                //    if (billModel.BillPaymentModels.Any())
                //    {
                //        billModel.PaymentTotalAmount = billModel.BillPaymentModels.Sum(c => c.Ammount);
                //    }
                //}
                //// List<BillMemoModel> lstMemoModel = new List<BillMemoModel>();
                //var lstMemoDetails = _creditMemoApplicationDetailService.GetCreditMemoDetailById(bill.Id);
                //if (lstMemoDetails.Any())
                //{
                //    foreach (var memoD in lstMemoDetails)
                //    {
                //        BillPaymentModel billmemoModel = new BillPaymentModel();
                //        if (memoD != null)
                //        {
                //            var creditMemo = _creditMemoApplicationService.GetCreditMemoByCompanyId(memoD.CreditMemoApplicationId);
                //            if (creditMemo != null)
                //            {
                //                CreditMemo memo = _creditMemoService.GetCmById(creditMemo.CreditMemoId, creditMemo.CompanyId);
                //                billmemoModel.PaymentDetailId = memo != null ? memo.Id : Guid.Empty;
                //                billmemoModel.DocDate = creditMemo.CreditMemoApplicationDate;
                //                billmemoModel.DocNo = memo != null ? memo.DocNo : string.Empty;
                //                billmemoModel.SystemRefNo = creditMemo.CreditMemoApplicationNumber;
                //                billmemoModel.Ammount = memoD.CreditAmount;
                //                billmemoModel.DocType = "CM Application";
                //                lstBillPayNModel.Add(billmemoModel);
                //            }
                //        }
                //    }
                //    billModel.BillPaymentModels = lstBillPayNModel.OrderBy(c => c.DocDate).ToList();
                //    if (billModel.BillPaymentModels.Any())
                //    {
                //        billModel.MemoTotalAmount = billModel.BillPaymentModels.Sum(c => c.Ammount);
                //    }
                //}
                #endregion Commented

                if (billModel.IsGstSettings == true)
                {
                    if (bill.DocSubType == DocTypeConstants.PayrollBill)
                    {
                        List<TaxModel> lstGstTaxModel = new List<TaxModel>();
                        var lstTaxCode = bill.BillDetails.Where(x => x.DocAmount > 0 && x.TaxCode != "NA").GroupBy(c => c.TaxCode).ToList();
                        if (lstTaxCode.Any())
                        {
                            foreach (var gstDetail in lstTaxCode)
                            {
                                TaxModel gstTax = new TaxModel();
                                var tax = _taxCodeService.GetTaxByCode(gstDetail.Key);
                                gstTax.TaxId = tax.Id;
                                gstTax.TaxCode = tax.Code;
                                gstTax.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") /*+ "(" + tax.TaxType[0] + ")"*/ : tax.Code;
                                gstTax.Amount = Math.Round((decimal)(gstDetail.Select(x => x.DocAmount).FirstOrDefault()), 2);
                                gstTax.Amount = Math.Round((decimal)(gstTax.Amount * (bill.GSTExchangeRate == null ? 1 : bill.GSTExchangeRate)), 2, MidpointRounding.AwayFromZero);
                                gstTax.TaxAmount = Math.Round((decimal)(gstDetail.Select(c => c.DocTaxAmount).FirstOrDefault()), 2);
                                gstTax.TaxAmount = Math.Round((decimal)(gstTax.TaxAmount * (bill.GSTExchangeRate == null ? 1 : bill.GSTExchangeRate)), 2, MidpointRounding.AwayFromZero);
                                gstTax.TotalAmount = Math.Round((decimal)(gstTax.Amount + gstTax.TaxAmount), 2);
                                lstGstTaxModel.Add(gstTax);
                            }
                            billModel.TaxModels = lstGstTaxModel;
                        }
                    }
                }
                #region Commented_code
                //var lstBillDetails = _billDetailService.GetAllBillDetailModel(billModel.Id);
                ////List<BillDetailModel> lstDetailModels = new List<BillDetailModel>();
                //foreach (var billDetail in lstBillDetails)
                //{
                //    BillDetailModel billDetailModel = new BillDetailModel();
                //    billDetailModel.Id = billDetail.Id;
                //    billDetailModel.BillId = billDetail.BillId;
                //    billDetailModel.BeanCOA.COAId = billDetail.COAId;
                //    //var coa = _chartOfAccountService.GetChartOfAccountById(billDetailModel.BeanCOA.COAId);
                //    billDetailModel.BeanCOA.COAName = billDetail.Account;
                //    billDetailModel.IsPLAccount = billDetail.IsPLAccount;
                //    if (bill.DocSubType == DocTypeConstants.Bills)
                //    {
                //        if (billDetail.TaxId != null)
                //        {
                //            TaxCode tax = _taxCodeService.GetTaxCode(billDetail.TaxId.Value);
                //            if (tax != null)
                //            {
                //                billDetailModel.TaxId = tax.Id;
                //                billDetailModel.TaxRate = tax.TaxRate;
                //                billDetailModel.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
                //                billDetailModel.TaxType = tax.TaxType;
                //                billDetailModel.TaxCode = tax.Code;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        TaxCode tax = _taxCodeService.GetTaxByCode(billDetail.TaxCode);
                //        if (tax != null)
                //        {
                //            billDetailModel.TaxId = tax.Id;
                //            billDetailModel.TaxRate = tax.TaxRate;
                //            billDetailModel.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
                //            billDetailModel.TaxType = tax.TaxType;
                //            billDetailModel.TaxCode = tax.Code;
                //        }
                //    }
                //    billDetailModel.TaxId = billDetail.TaxId;
                //    billDetailModel.TaxCode = billDetail.TaxCode;
                //    //billDetailModel.TaxRate = billDetail.TaxRate;
                //    //billDetailModel.TaxType = billDetail.TaxType;
                //    //billDetailModel.TaxIdCode = billDetail.TaxCode + "-" + billDetail.TaxRate + "%" + "(" + billDetail.TaxType[0] + ")";
                //    billDetailModel.DocAmount = billDetail.DocAmount;
                //    billDetailModel.DocTaxAmount = billDetail.DocTaxAmount;
                //    billDetailModel.DocTotalAmount = billDetail.DocTotalAmount;
                //    billDetailModel.IsDisallow = billDetail.IsDisallow;
                //    billDetailModel.BaseAmount = billDetail.BaseAmount;
                //    billDetailModel.BaseTaxAmount = billDetail.BaseTaxAmount;
                //    billDetailModel.BaseTotalAmount = billDetail.BaseTotalAmount;
                //    billDetailModel.Description = billDetail.Description;
                //    billDetailModel.RecOrder = billDetail.RecOrder;
                //    //   var creditMemo = _billCreditMemoDetailService.GetBillCreditMemoDetail(billDetailModel.Id);
                //    //BillCreditMemoDetailModel billCreditMemoDetail = new BillCreditMemoDetailModel();
                //    //if (creditMemo != null)
                //    //{
                //    //    billCreditMemoDetail.Id = creditMemo.Id;
                //    //    billCreditMemoDetail.BillDetailId = creditMemo.BillDetailId;
                //    //    billCreditMemoDetail.CreditMemoId = creditMemo.CreditMemoId;
                //    //    billCreditMemoDetail.DocCreditMemoAmount = creditMemo.DocCreditMemoAmount;
                //    //    billCreditMemoDetail.DocCreditMemoTaxAmount = creditMemo.DocCreditMemoTaxAmount;
                //    //    billCreditMemoDetail.DocToTal = creditMemo.DocTotal;
                //    //    billCreditMemoDetail.BaseCreditMemoAmount = creditMemo.BaseCreditMemoAmount;
                //    //    billCreditMemoDetail.BaseCreditMemoTaxAmount = creditMemo.BaseCreditMemoTaxAmount;
                //    //    billCreditMemoDetail.BaseToTal = creditMemo.BaseTotal;


                //    //    billDetailModel.BillCreditMemoDetailModel = billCreditMemoDetail;
                //    //}
                //    //else
                //    //{
                //    //    billDetailModel.BillCreditMemoDetailModel = billCreditMemoDetail;
                //    //}

                //    lstDetailModels.Add(billDetailModel);
                //}
                #endregion


                billModel.BillDetailModels = bill.BillDetails.Any() ? bill.BillDetails.Select(a => new BillDetailModel()
                {
                    Id = isCopy ? Guid.NewGuid() : a.Id,
                    BillId = a.BillId,
                    COAId = a.COAId,
                    //COAName = a.Account,
                    IsPLAccount = a.IsPLAccount,
                    TaxId = a.TaxId,
                    TaxCode = a.TaxCode,
                    DocAmount = a.DocAmount,
                    DocTaxAmount = a.DocTaxAmount,
                    DocTotalAmount = a.DocTotalAmount,
                    IsDisallow = a.IsDisallow,
                    BaseAmount = a.BaseAmount,
                    BaseTaxAmount = a.BaseTaxAmount,
                    BaseTotalAmount = a.BaseTotalAmount,
                    Description = a.Description,
                    TaxIdCode = a.TaxIdCode,
                    TaxRate = a.TaxRate,
                    RecOrder = a.RecOrder,
                    ClearingState = a.ClearingState,
                }).OrderBy(a => a.RecOrder).ToList() : null;

                #region commented_code
                //List<BillDetailModel> lstDetailModels = (from billDetail in _billDetailService.Queryable()
                //                                         select new BillDetailModel()
                //                                         {
                //                                             Id = billDetail.Id,
                //                                             BillId = billDetail.BillId,
                //                                             COAId = billDetail.COAId,
                //                                             COAName = billDetail.Account,
                //                                             IsPLAccount = billDetail.IsPLAccount,
                //                                             TaxId = billDetail.TaxId,
                //                                             TaxCode = billDetail.TaxCode,
                //                                             DocAmount = billDetail.DocAmount,
                //                                             DocTaxAmount = billDetail.DocTaxAmount,
                //                                             DocTotalAmount = billDetail.DocTotalAmount,
                //                                             IsDisallow = billDetail.IsDisallow,
                //                                             BaseAmount = billDetail.BaseAmount,
                //                                             BaseTaxAmount = billDetail.BaseTaxAmount,
                //                                             BaseTotalAmount = billDetail.BaseTotalAmount,
                //                                             Description = billDetail.Description,
                //                                             RecOrder = billDetail.RecOrder,
                //                                         }).ToList();


                //billModel.BillDetailModels = lstDetailModels.OrderBy(x => x.RecOrder).ToList();

                //billModel.BillGSTDetails = _billGstDetailService.GetAllGstBillDetailModel(billModel.Id);
                #endregion


                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillModel_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillModel_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

        }

        public void UpdateBillDetails(BillModel TObject, Bill _billNew)
        {
            try
            {
                int count = TObject.BillDetailModels.Max(c => c.RecOrder) ?? 0;
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_UpdateBillDetails_UpdateCall_Request_Message);
                foreach (BillDetailModel detail in TObject.BillDetailModels)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        BillDetail billDetail = new BillDetail();
                        billDetail.Id = Guid.NewGuid();
                        billDetail.BillId = TObject.Id;
                        billDetail.Description = detail.Description;
                        billDetail.COAId = detail.COAId;
                        billDetail.IsDisallow = detail.IsDisallow;
                        billDetail.TaxId = detail.TaxId;
                        billDetail.TaxCode = detail.TaxCode;
                        billDetail.TaxRate = detail.TaxRate;
                        billDetail.TaxIdCode = detail.TaxIdCode;
                        billDetail.DocAmount = detail.DocAmount;
                        billDetail.DocTaxAmount = detail.DocTaxAmount;
                        billDetail.DocTotalAmount = detail.DocTotalAmount;
                        billDetail.BaseAmount = TObject.DocCurrency == TObject.BaseCurrency ? billDetail.DocAmount : Math.Round((decimal)billDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                        billDetail.BaseTaxAmount = TObject.DocCurrency == TObject.BaseCurrency ? billDetail.DocTaxAmount : Math.Round((decimal)billDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                        billDetail.BaseTotalAmount = TObject.DocCurrency == TObject.BaseCurrency ? billDetail.DocTotalAmount : billDetail.BaseAmount + billDetail.BaseTaxAmount;
                        billDetail.IsPLAccount = detail.IsPLAccount;
                        billDetail.RecOrder = ++count;
                        billDetail.ObjectState = ObjectState.Added;
                        _billNew.BillDetails.Add(billDetail);

                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        BillDetail billDetail = _billNew.BillDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (billDetail != null)
                        {
                            billDetail.ObjectState = ObjectState.Deleted;
                        }

                    }
                    else
                    {
                        BillDetail billDetail = _billNew.BillDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (billDetail != null)
                        {

                            billDetail.Description = detail.Description;
                            billDetail.COAId = detail.COAId;
                            billDetail.IsDisallow = detail.IsDisallow;
                            billDetail.TaxId = detail.TaxId;
                            billDetail.TaxCode = detail.TaxCode;
                            billDetail.TaxRate = detail.TaxRate;
                            billDetail.TaxIdCode = detail.TaxIdCode;
                            billDetail.DocAmount = detail.DocAmount;
                            billDetail.DocTaxAmount = detail.DocTaxAmount;
                            billDetail.DocTotalAmount = detail.DocTotalAmount;

                            billDetail.BaseAmount = TObject.DocCurrency == TObject.BaseCurrency ? billDetail.DocAmount : Math.Round((decimal)billDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                            billDetail.BaseTaxAmount = TObject.DocCurrency == TObject.BaseCurrency ? billDetail.DocTaxAmount : Math.Round((decimal)billDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                            billDetail.BaseTotalAmount = TObject.DocCurrency == TObject.BaseCurrency ? billDetail.DocTotalAmount : billDetail.BaseAmount + billDetail.BaseTaxAmount;
                            billDetail.IsPLAccount = detail.IsPLAccount;
                            billDetail.RecOrder = detail.RecOrder;
                            billDetail.ObjectState = ObjectState.Modified;
                        }
                    }
                }

                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_UpdateBillDetails_UpdateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_UpdateBillDetails_UpdateCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }

        private void InsertBill(BillModel TObject, Bill billNew)
        {
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InsertBill_FillCall_Request_Message);
                billNew.Id = TObject.Id;

                //newlly added code for DocType and 
                billNew.DocType = DocTypeConstants.Bills;
                billNew.EntityType = "Vendor";
                //switch (TObject.DocSubType)
                //{
                //    case DocTypeConstants.General:
                //        billNew.DocSubType = DocTypeConstants.General;
                //        billNew.ExchangeRate = TObject.ExchangeRate;
                //        billNew.IsGstSettings = TObject.IsGstSettings;
                //        billNew.GSTExCurrency = TObject.GstReportingCurrency;
                //        break;
                //    case DocTypeConstants.PayrollBill:
                //        string gstRepoCurrency = _gstSettingService.GetGSTByServiceCompany(TObject.ServiceCompanyId);
                //        bool isGstActivate = _gstSettingService.IsGSTSettingActivateByServiceCompany(TObject.CompanyId, TObject.DocDate, TObject.ServiceCompanyId);
                //        if (TObject.DocCurrency != TObject.BaseCurrency)
                //        {
                //            billNew.DocSubType = DocTypeConstants.PayrollBill;
                //            billNew.ExchangeRate = TObject.IsBaseCurrencyRateChanged == true ? TObject.ExchangeRate : GetExRateInformations(TObject.DocCurrency, TObject.DocDate, TObject.CompanyId, TObject.BaseCurrency);
                //            billNew.GSTExchangeRate = GetExRateInformations(gstRepoCurrency, TObject.DocDate, TObject.CompanyId, TObject.BaseCurrency);
                //            billNew.IsGstSettings = isGstActivate;
                //            billNew.GSTExCurrency = gstRepoCurrency != null || gstRepoCurrency != string.Empty ? gstRepoCurrency : null;
                //        }
                //        else
                //        {
                //            billNew.DocSubType = DocTypeConstants.PayrollBill;
                //            billNew.ExchangeRate = 1;
                //            billNew.IsGstSettings = isGstActivate;
                //            billNew.GSTExCurrency =/* gstActivate != null ? gstActivate.GSTRepoCurrency : null*/"SGD";
                //            billNew.GSTExchangeRate = 1;
                //        }
                //        break;

                //    default:
                //        billNew.ExchangeRate = TObject.BaseCurrency == TObject.DocCurrency ? 1 : TObject.ExchangeRate;
                //        billNew.IsGstSettings = TObject.IsGstSettings;
                //        billNew.GSTExCurrency = TObject.GstReportingCurrency;
                //        billNew.GSTExchangeRate = TObject.BaseCurrency == TObject.DocCurrency ? 1 : TObject.GstExchangeRate;
                //        break;
                //}
                #region oldCode 
                if (TObject.DocSubType == DocTypeConstants.General)
                {
                    billNew.DocSubType = DocTypeConstants.General;
                    billNew.ExchangeRate = TObject.ExchangeRate;
                    billNew.IsGstSettings = TObject.IsGstSettings;
                    billNew.GSTExCurrency = TObject.GstReportingCurrency;
                }
                else if (TObject.DocSubType == DocTypeConstants.PayrollBill)
                {
                    string gstRepoCurrency = _gstSettingService.GetGSTByServiceCompany(TObject.ServiceCompanyId);
                    bool isGstActivate = _gstSettingService.IsGSTSettingActivateByServiceCompany(TObject.CompanyId, TObject.DocDate, TObject.ServiceCompanyId);
                    if (TObject.DocCurrency != TObject.BaseCurrency)
                    {
                        billNew.DocSubType = DocTypeConstants.PayrollBill;
                        billNew.ExchangeRate = TObject.IsBaseCurrencyRateChanged == true ? TObject.ExchangeRate : GetExRateInformations(TObject.DocCurrency, TObject.DocDate, TObject.CompanyId, TObject.BaseCurrency);
                        billNew.GSTExchangeRate = GetExRateInformations(gstRepoCurrency, TObject.DocDate, TObject.CompanyId, TObject.BaseCurrency);
                        billNew.IsGstSettings = isGstActivate;
                        billNew.GSTExCurrency = gstRepoCurrency != null || gstRepoCurrency != string.Empty ? gstRepoCurrency : null;
                    }
                    else
                    {
                        //GST currency hard coded to SGD and in future this should modified to handle the different GST currencies
                        billNew.DocSubType = DocTypeConstants.PayrollBill;
                        billNew.ExchangeRate = 1;
                        billNew.IsGstSettings = isGstActivate;
                        billNew.GSTExCurrency =/* gstActivate != null ? gstActivate.GSTRepoCurrency : null*/"SGD";
                        billNew.GSTExchangeRate = 1;
                    }
                }
                else    //If other than Payroll and General Bill
                {
                    billNew.ExchangeRate = TObject.BaseCurrency == TObject.DocCurrency ? 1 : TObject.ExchangeRate;
                    billNew.IsGstSettings = TObject.IsGstSettings;
                    billNew.GSTExCurrency = TObject.GstReportingCurrency;
                    billNew.GSTExchangeRate = TObject.BaseCurrency == TObject.DocCurrency ? 1 : TObject.GstExchangeRate;
                }
                #endregion oldCode
                billNew.DocSubType = TObject.DocSubType;
                billNew.IsMultiCurrency = TObject.ISMultiCurrency;
                billNew.DocumentDate = TObject.DocDate.Date;
                billNew.DueDate = TObject.DueDate;
                billNew.CompanyId = TObject.CompanyId;
                billNew.CreatedDate = TObject.CreatedDate;
                billNew.CreditTermsId = TObject.CreditTermId;
                billNew.EntityId = TObject.EntityId;
                billNew.DocDescription = TObject.DocDescription;
                billNew.ServiceCompanyId = TObject.ServiceCompanyId;
                billNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                if (TObject.IsGSTCurrencyRateChanged == true)
                    billNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged.Value;
                billNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                billNew.NoSupportingDocument = TObject.NoSupportingDocument;
                billNew.Nature = TObject.Nature;
                billNew.UserCreated = TObject.UserCreated;
                billNew.GrandTotal = TObject.GrandTotal;
                billNew.DocCurrency = TObject.DocCurrency;
                billNew.BaseCurrency = TObject.BaseCurrency;
                billNew.IsGSTApplied = TObject.IsGSTApplied;
                billNew.PostingDate = TObject.PostingDate;
                billNew.Status = TObject.Status;
                billNew.PayrollId = TObject.SyncPayrollId;
                billNew.PayrollId = TObject.PayrollId;
                billNew.OpeningBalanceId = TObject.OpeningBalanceId;
                billNew.IsExternal = TObject.IsExternal != null ? TObject.IsExternal : TObject.DocSubType != DocTypeConstants.General;/*TObject.IsExternal*/
                if (TObject.IsGstSettings && TObject.DocSubType == DocTypeConstants.General)
                {
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_In_If_Conditionand_Check_TObject_IsGstSetting);
                    billNew.GSTExchangeRate = TObject.GstExchangeRate;
                }
                billNew.PeppolDocumentId = TObject.PeppolDocumentId; //newly added nullable field

                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InsertBill_FillCall_SuccessFully_Message);
            }

            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InsertBill_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }

        private void FillBillDetailModel(BillDetailModel billDModel, BillDetail bDetail)
        {
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillDetailModel_FillCall_Request_Message);
                billDModel.BillId = bDetail.BillId;
                billDModel.Id = bDetail.Id;
                billDModel.COAId = bDetail.COAId;
                var coa = _chartOfAccountService.GetChartOfAccountById(billDModel.COAId);
                billDModel.COAName = coa.Name;
                billDModel.TaxId = bDetail.TaxId;
                billDModel.TaxCode = bDetail.TaxCode;
                var txname = _taxCodeService.GetTaxById(billDModel.TaxId);
                billDModel.TaxName = txname.Name;
                billDModel.TaxRate = bDetail.TaxRate;
                billDModel.TaxType = bDetail.TaxType;
                billDModel.BaseAmount = bDetail.BaseAmount;
                billDModel.BaseTaxAmount = bDetail.BaseTaxAmount;
                billDModel.BaseTotalAmount = bDetail.BaseTotalAmount;
                billDModel.Description = bDetail.Description;
                billDModel.DocAmount = bDetail.DocAmount;
                billDModel.DocTaxAmount = bDetail.DocTaxAmount;
                billDModel.DocTotalAmount = bDetail.DocTotalAmount;
                billDModel.IsDisallow = bDetail.IsDisallow;
                // var creditMemo = _billCreditMemoDetailService.GetBillCreditMemoDetail(billDModel.Id);
                //BillCreditMemoDetailModel billCreditMemoDetail = new BillCreditMemoDetailModel();
                //if (creditMemo != null)
                //{
                //    billCreditMemoDetail.Id = creditMemo.Id;
                //    billCreditMemoDetail.BillDetailId = creditMemo.BillDetailId;
                //    billCreditMemoDetail.CreditMemoId = creditMemo.CreditMemoId;
                //    billCreditMemoDetail.DocCreditMemoAmount = creditMemo.DocCreditMemoAmount;
                //    billCreditMemoDetail.DocCreditMemoTaxAmount = creditMemo.DocCreditMemoTaxAmount;
                //    billCreditMemoDetail.DocToTal = creditMemo.DocTotal;
                //    billCreditMemoDetail.BaseCreditMemoAmount = creditMemo.BaseCreditMemoAmount;
                //    billCreditMemoDetail.BaseCreditMemoTaxAmount = creditMemo.BaseCreditMemoTaxAmount;
                //    billCreditMemoDetail.BaseToTal = creditMemo.BaseTotal;


                //    billDModel.BillCreditMemoDetailModel = billCreditMemoDetail;
                //}
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillDetailModel_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_FillBillDetailModel_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

        }
        #endregion

        #region Extra Calls
        public bool IsGSTAllowed(long companyId, DateTime docDate)
        {
            GSTSetting setting = _gstSettingService.GetGSTSettings(companyId);
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

        #region AutoNumber and DocNo Block
        string value = "";
        public string GenerateAutoNumberForType(long companyId, string type, string companyCode, string docType)
        {
            string generatedAutoNumber = "";
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateAutoNumberForType_GetById_Request_Message);
                if (docType == "Payroll Bill")
                {
                    AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, docType);
                    //BeanAutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, docType);
                    generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength), _autoNo.GeneratedNumber, companyId, companyCode);

                    if (_autoNo != null)
                    {
                        _autoNo.GeneratedNumber = value;
                        _autoNo.IsDisable = true;
                        _autoNo.ObjectState = ObjectState.Modified;
                        _autoNumberService.Update(_autoNo);
                    }
                    //Remove Common.AutonumberCompany, as it is not used in the autonumber generation stored procedure.
                    /*var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
                    if (_autonumberCompany.Any())
                    {
                        AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                        _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                    }
                    else
                    {
                        AutoNumberCompany _autoNumberCompanyNew = new AutoNumberCompany();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.Id = Guid.NewGuid();
                        _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                        _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                    }*/
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateAutoNumberForType_GetById_SuccessFully_Message);
                }
                else
                {
                    AutoNumber _autoNumber = _autoNumberService.GetAutoNumber(companyId, type);
                    //BeanAutoNumber _autoNumber = _autoNumberService.GetAutoNumber(companyId, type);
                    generatedAutoNumber = GenerateFromFormat(_autoNumber.EntityType, _autoNumber.Format, Convert.ToInt32(_autoNumber.CounterLength), _autoNumber.GeneratedNumber, companyId, companyCode);

                    if (_autoNumber != null)
                    {
                        _autoNumber.GeneratedNumber = value;
                        _autoNumber.IsDisable = true;
                        _autoNumber.ObjectState = ObjectState.Modified;
                        _autoNumberService.Update(_autoNumber);
                    }
                    //Remove Common.AutonumberCompany, as it is not used in the autonumber generation stored procedure.
                    /* var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNumber.Id);
                     if (_autonumberCompany.Any())
                     {
                         AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                         _autoNumberCompanyNew.GeneratedNumber = value;
                         _autoNumberCompanyNew.AutonumberId = _autoNumber.Id;
                         _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                         _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                     }
                     else
                     {
                         AutoNumberCompany _autoNumberCompanyNew = new AutoNumberCompany();
                         _autoNumberCompanyNew.GeneratedNumber = value;
                         _autoNumberCompanyNew.AutonumberId = _autoNumber.Id;
                         _autoNumberCompanyNew.Id = Guid.NewGuid();
                         _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                         _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                     }*/

                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateAutoNumberForType_GetById_SuccessFully_Message);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateAutoNumberForType_GetById_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return generatedAutoNumber;
        }

        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, string Companycode = null)
        {
            int? currentMonth = '0';
            List<Bill> lstBill = null;
            bool ifContainMonth = false;

            string OutputNumber = "";
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateFromFormat_GetById_Request_Message);
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
                    ifContainMonth = true;
                }
                else if (companyFormatHere.Contains("{COMPANYCODE}"))
                {
                    companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
                }
                double val = 0;
                if (Type == DocTypeConstants.Bill || Type == DocTypeConstants.PayrollBill)
                {
                    lstBill = _billService.GetAllBillModel(companyId);
                    if (lstBill.Any() && ifContainMonth)
                    {
                        int? lastCreatedDate = lstBill.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                        if (DateTime.Now.Year == lstBill.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                        {
                            if (lastCreatedDate == currentMonth)
                            {
                                AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                                //BeanAutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                                foreach (var bill in lstBill)
                                {
                                    if (bill.SystemReferenceNumber != autonumber.Preview)
                                    {
                                        val = Convert.ToInt32(IncreamentVal);
                                    }
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
                    else if (lstBill.Any() && ifContainMonth == false)
                    {
                        AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        //BeanAutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        foreach (var bill in lstBill)
                        {
                            if (bill.SystemReferenceNumber != autonumber.Preview)
                            {
                                val = Convert.ToInt32(IncreamentVal);
                            }
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

                if (lstBill.Any())
                {
                    OutputNumber = GetNewNumber(lstBill, Type, OutputNumber, counter, companyFormatHere, counterLength);
                }

                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateFromFormat_GetById_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GenerateFromFormat_GetById_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return OutputNumber;
        }
        private string GetNewNumber(List<Bill> lstBill, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstBill.Where(a => a.SystemReferenceNumber == outputNumber).FirstOrDefault();
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
                    var inv = lstBill.Where(c => c.SystemReferenceNumber == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }

        private string GetNewBillDocumentNumber(long CompanyId)
        {
            CreditMemo creditMemo = _creditMemoService.GetCreditMemoByCompanyId(CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetNewBillDocumentNumber_GetById_Request_Message);
                if (creditMemo != null)
                {
                    string strOldNo = String.Empty;
                    CreditMemo duplicatMemo;
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

                        duplicatMemo = _creditMemoService.GetDocNo(strNewDocNo, CompanyId);
                    } while (duplicatMemo != null);
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetNewBillDocumentNumber_GetById_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetNewBillDocumentNumber_GetById_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return strNewDocNo;
        }

        private bool IsDocumentNumberExists(string DocType, string DocNo, Guid id, long companyId, Guid entityId, string DocSubType)
        {
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_IsDocumentNumberExists_GetById_Request_Message);
            return _billService.GetByDocSubTypeId(id, DocNo, companyId, DocType, entityId, DocSubType);

            // LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_IsDocumentNumberExists_GetById_SuccessFully_Message);
            //try
            //{
            //    string DocumentVoidState = "";
            //    switch (DocType)
            //    {
            //        case DocTypeConstants.Bill:
            //            DocumentVoidState = BillNoteState.Void;
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_IsDocumentNumberExists_GetById_Exception_Message);
            //    Log.Logger.ZCritical(ex.StackTrace);
            //    throw ex;
            //}
            //return doc != null;
        }

        private string GetNewPaymentDocumentNo(long companyId, string docType, string billDocNo)
        {
            //docType==BillConstant.Payroll_Bill?BillConstant.Payroll_Payment:BillConstant.Payment
            Payment payement = _paymentService.GetPaymentByComapnyId(companyId, docType == BillConstant.Payroll_Bill ? BillConstant.Payroll_Payment : BillConstant.Payment);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetNewBillDocumentNumber_GetById_Request_Message);
                if (payement != null)
                {
                    string strOldNo = String.Empty;
                    Payment duplicatePayement;
                    int index;
                    strOldDocNo = payement.DocNo;

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

                        duplicatePayement = _paymentService.GetDocNo(strNewDocNo, companyId);
                    } while (duplicatePayement != null);
                }
                else
                {
                    if (docType == BillConstant.Payroll_Bill)
                    {
                        if (billDocNo.Contains("PRB"))
                        {
                            string value = billDocNo.Substring(3);
                            strNewDocNo = "PRPY" + value;
                        }
                    }
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetNewBillDocumentNumber_GetById_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetNewBillDocumentNumber_GetById_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return strNewDocNo;
        }
        #endregion

        #region Create CreditMemo By Bill
        public CreditMemoModel CreateCreditMemoByBill(Guid billId, long companyId, string connectionString)
        {
            CreditMemoModel memoDTO = new CreditMemoModel();
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_CreateCreditMemoBybill_CreateCall_Request_Message);
                //AppsWorld.BillModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.BillCreditMemo);

                //to check if it is void or not
                if (_billService.IsVoid(companyId, billId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(BillConstant.The_Financial_setting_should_be_activated);
                }
                memoDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                memoDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                //Invoice lastCreditNote = _invoiceEntityService.GetCreditNoteByCompanyId(companyId);
                //CreditMemo lastMemo = _creditMemoService.GetLastCreditMemo(companyId);
                Bill bill = _billService.GetbillById(billId);
                memoDTO.Id = Guid.NewGuid();

                memoDTO.CompanyId = bill.CompanyId;
                memoDTO.EntityType = bill.EntityType;
                memoDTO.DocSubType = DocTypeConstants.BillCreditMemo;

                //memoDTO.DocNo = GetNewBillDocumentNumber(memoDTO.CompanyId);


                //bool? isEdit = false;
                //memoDTO.DocNo = GetAutoNumberForCreditMemo(companyId, lastMemo, DocTypeConstants.BillCreditMemo, _autoNo, ref isEdit);
                //memoDTO.IsDocNoEditable = isEdit;

                memoDTO.IsDocNoEditable = _autoService.GetAutoNumberIsEditable(companyId, DocTypeConstants.BillCreditMemo);
                if (memoDTO.IsDocNoEditable == true)
                    memoDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.BillCreditMemo, connectionString);


                memoDTO.CreditTermsId = bill.CreditTermsId;
                memoDTO.DocDate = bill.DocumentDate;
                if (bill.DocSubType != DocTypeConstants.OpeningBalance)
                {
                    var top = _termsOfPaymentService.GetById(memoDTO.CreditTermsId.Value);
                    if (top != null)
                    {
                        memoDTO.DueDate = memoDTO.DocDate.AddDays(top.TOPValue.Value);
                        memoDTO.CreditTermsName = top.Name;
                    }
                }
                else
                    memoDTO.DueDate = bill.DueDate;

                memoDTO.EntityId = bill.EntityId;
                BeanEntity beanEntity = _beanEntityService.GetEntityById(bill.EntityId);
                memoDTO.EntityName = beanEntity.Name;
                memoDTO.IsAllowableNonAllowable = bill.IsAllowableDisallowable;
                memoDTO.Nature = bill.Nature;
                memoDTO.DocCurrency = bill.DocCurrency;
                memoDTO.ServiceCompanyId = bill.ServiceCompanyId;

                memoDTO.IsMultiCurrency = bill.IsMultiCurrency;
                memoDTO.BaseCurrency = bill.BaseCurrency;
                memoDTO.ExchangeRate = bill.ExchangeRate;
                memoDTO.ExDurationFrom = bill.ExDurationFrom;
                memoDTO.ExDurationTo = bill.ExDurationTo;
                memoDTO.IsGSTApplied = bill.IsGSTApplied;

                memoDTO.IsGstSettings = bill.IsGstSettings;
                memoDTO.GSTExCurrency = bill.GSTExCurrency;
                memoDTO.GSTExchangeRate = bill.GSTExchangeRate;
                memoDTO.GSTExDurationFrom = bill.GSTExDurationFrom;
                memoDTO.GSTExDurationTo = bill.GSTExDurationTo;
                // memoDTO.ExtensionType = ExtensionType.bill;
                memoDTO.PostingDate = DateTime.UtcNow;
                //memoDTO.IsSegmentReporting = bill.IsSegmentReporting;
                //memoDTO.SegmentCategory1 = bill.SegmentCategory1;
                //memoDTO.SegmentCategory2 = bill.SegmentCategory2;

                memoDTO.GSTTotalAmount = bill.GSTTotalAmount;
                memoDTO.GrandTotal = bill.BalanceAmount.Value;
                memoDTO.BalanceAmount = bill.BalanceAmount.Value;
                memoDTO.DocDescription = bill.DocDescription;
                //memoDTO.IsAllowableNonAllowable = _CompanySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);

                memoDTO.IsNoSupportingDocument = bill.IsNoSupportingDocument;
                memoDTO.NoSupportingDocument = bill.NoSupportingDocument;
                memoDTO.ExtensionType = bill.DocSubType == DocTypeConstants.OpeningBalance ? DocTypeConstants.OBBill : DocTypeConstants.Bills;
                //memoDTO.SegmentMasterid1 = bill.SegmentMasterid1;

                //if (bill.SegmentMasterid1 != null)
                //{
                //    var segment1 = _SegmentMasterService.GetSegmentMastersById(bill.SegmentMasterid1.Value).FirstOrDefault(); ;
                //    memoDTO.IsSegmentActive1 = segment1.Status == RecordStatusEnum.Active;
                //}
                //if (bill.SegmentMasterid2 != null)
                //{
                //    var segment2 = _SegmentMasterService.GetSegmentMastersById(bill.SegmentMasterid2.Value).FirstOrDefault(); ;
                //    memoDTO.IsSegmentActive2 = segment2.Status == RecordStatusEnum.Active;
                //}

                //memoDTO.SegmentMasterid2 = bill.SegmentMasterid2;
                //memoDTO.SegmentDetailid1 = bill.SegmentDetailid1;
                //memoDTO.SegmentDetailid2 = bill.SegmentDetailid2;
                memoDTO.Status = bill.Status;
                memoDTO.DocumentState = CreditNoteState.NotApplied;
                memoDTO.CreatedDate = bill.CreatedDate;
                memoDTO.UserCreated = bill.UserCreated;
                memoDTO.IsBaseCurrencyRateChanged = bill.IsBaseCurrencyRateChanged;
                memoDTO.IsGSTCurrencyRateChanged = bill.IsGSTCurrencyRateChanged;
                memoDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(memoDTO.CompanyId);
                memoDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(memoDTO.CompanyId);
                //memoDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(memoDTO.CompanyId);
                //memoDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(memoDTO.CompanyId);

                //var lstTaxCodes = _taxCodeService.GetAllTaxs(companyId);
                //memoDTO.CreditMemoDetailModels = (from detail in bill.BillDetails
                //                                  join taxCode in _taxCodeService.Queryable().Where(z=>z.CompanyId==companyId) on detail.TaxId equals taxCode.Id into empty
                //                                  from taxCode in empty.DefaultIfEmpty()
                //                                  select new CreditMemoDetailModel
                //                                  {
                //                                      Id = Guid.NewGuid(),
                //                                      CreditMemoId = detail.BillId,
                //                                      TaxId = detail.TaxId,
                //                                      TaxRate = detail.TaxRate,
                //                                      TaxType = detail.TaxType,
                //                                      TaxIdCode = taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") + "(" + taxCode.TaxType[0] + ")" : taxCode.Code,
                //                                      TaxCode = taxCode.Code,
                //                                      COAId = detail.COAId,
                //                                      BaseAmount = detail.BaseAmount,
                //                                      BaseTaxAmount = detail.BaseTaxAmount,
                //                                      BaseTotalAmount = detail.BaseTotalAmount,
                //                                  }).ToList();
                List<CreditMemoDetailModel> lstdetailModel = new List<CreditMemoDetailModel>();
                foreach (var item in bill.BillDetails)
                {
                    CreditMemoDetailModel model = new CreditMemoDetailModel();
                    model.Id = Guid.NewGuid();
                    model.CreditMemoId = memoDTO.Id;
                    model.TaxId = item.TaxId;
                    model.TaxRate = item.TaxRate;
                    model.DocAmount = item.DocAmount;
                    model.DocTaxAmount = item.DocTaxAmount;
                    model.DocTotalAmount = item.DocTotalAmount;
                    model.TaxType = item.TaxType;
                    model.Description = item.Description;
                    model.AllowDisAllow = item.IsDisallow;
                    if (item.TaxId != null)
                    {
                        TaxCode taxCode = _taxCodeService.GetTaxCode(item.TaxId.Value);
                        if (taxCode != null)
                        {
                            model.TaxIdCode = taxCode.Code != "NA" ? taxCode.Code + "-" + taxCode.TaxRate + (taxCode.TaxRate != null ? "%" : "NA") /*+ "(" + taxCode.TaxType[0] + ")"*/ : taxCode.Code;
                            model.TaxCode = taxCode.Code;
                        }
                    }
                    ChartOfAccount account = _chartOfAccountService.GetChartOfAccountById(item.COAId);
                    if (account != null)
                        model.AccountName = account.Name;
                    model.COAId = bill.DocSubType == DocTypeConstants.OpeningBalance ? 0 : item.COAId;
                    model.BaseAmount = item.BaseAmount;
                    model.BaseTaxAmount = item.BaseTaxAmount;
                    model.BaseTotalAmount = item.BaseTotalAmount;
                    model.IsPLAccount = item.IsPLAccount;
                    model.RecOrder = item.RecOrder;
                    lstdetailModel.Add(model);
                }
                memoDTO.CreditMemoDetailModels = lstdetailModel.OrderBy(x => x.RecOrder).ToList();

                //List<InvoiceGSTDetail> lstInvGstDetail = new List<InvoiceGSTDetail>();
                //if (bill.billGSTDetails.Any())
                //{
                //    foreach (var gstDetail in bill.billGSTDetails)
                //    {
                //        InvoiceGSTDetail invoiceGSTDetail = new InvoiceGSTDetail();
                //        invoiceGSTDetail.Id = Guid.NewGuid();
                //        invoiceGSTDetail.InvoiceId = memoDTO.Id;
                //        invoiceGSTDetail.TaxId = gstDetail.TaxId;
                //        invoiceGSTDetail.Amount = gstDetail.Amount;
                //        invoiceGSTDetail.TaxAmount = gstDetail.TaxAmount;
                //        invoiceGSTDetail.TotalAmount = gstDetail.TotalAmount;

                //        lstInvGstDetail.Add(invoiceGSTDetail);
                //    }
                //}
                //memoDTO.InvoiceGSTDetails = lstInvGstDetail;

                CreditMemoApplicationModel CNAModel = new CreditMemoApplicationModel();

                CNAModel.Id = Guid.NewGuid();
                CNAModel.CreditMemoId = memoDTO.Id;
                CNAModel.CompanyId = bill.CompanyId;
                CNAModel.DocNo = bill.DocNo;
                CNAModel.DocCurrency = bill.DocCurrency;
                CNAModel.CreditMemoApplicationDate = DateTime.UtcNow;
                CNAModel.DocDate = memoDTO.DocDate;
                CNAModel.Remarks = bill.DocDescription;
                CNAModel.EntityName = beanEntity.Name;
                decimal sumLineTotal = 0;
                if (memoDTO.CreditMemoDetailModels.Any())
                {
                    sumLineTotal = memoDTO.CreditMemoDetailModels.Sum(od => od.DocTotalAmount);
                }
                CNAModel.CreditAmount = bill.BalanceAmount;
                CNAModel.CreditMemoAmount = bill.BalanceAmount;
                CNAModel.CreditMemoBalanceAmount = bill.BalanceAmount;
                //CNAModel.CreditMemoApplicationDate = bill.DocumentDate;
                CNAModel.CreditMemoApplicationDate = bill.PostingDate;
                CNAModel.IsNoSupportingDocument = bill.IsNoSupportingDocument;
                CNAModel.NoSupportingDocument = bill.NoSupportingDocument;
                //CNAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(memoDTO.CompanyId);
                //CNAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(memoDTO.CompanyId);
                CNAModel.FinancialPeriodLockStartDate = memoDTO.FinancialPeriodLockStartDate;
                CNAModel.FinancialPeriodLockEndDate = memoDTO.FinancialPeriodLockEndDate;
                CNAModel.CreatedDate = DateTime.UtcNow;
                CNAModel.UserCreated = bill.UserCreated;
                CNAModel.Status = CreditMemoApplicationStatus.Posted;
                CNAModel.ExchangeRate = bill.ExchangeRate;
                memoDTO.CreditMemoApplicationModel = CNAModel;

                List<CreditMemoApplicationDetailModel> lstCNADModel = new List<CreditMemoApplicationDetailModel>();
                CreditMemoApplicationDetailModel detailModel = new CreditMemoApplicationDetailModel();

                detailModel.Id = Guid.NewGuid();
                detailModel.CreditMemoApplicationId = CNAModel.Id;
                detailModel.BalanceAmount = bill.BalanceAmount;
                detailModel.DocCurrency = CNAModel.DocCurrency;
                detailModel.DocType = DocTypeConstants.Bills;
                detailModel.Nature = bill.Nature;
                detailModel.DocAmount = bill.GrandTotal;
                detailModel.DocDate = bill.DocumentDate;
                detailModel.PostingDate = bill.PostingDate;
                detailModel.DocumentId = bill.Id;
                detailModel.DocNo = bill.DocNo;
                detailModel.DocState = bill.DocumentState;
                detailModel.SystemReferenceNumber = bill.SystemReferenceNumber;
                if (memoDTO.ServiceCompanyId != null)
                {
                    Company company = _companyService.GetById(memoDTO.ServiceCompanyId.Value);
                    if (company != null)
                    {
                        detailModel.SereviceEntityId = company.Id;
                        detailModel.ServiceEntityName = company.ShortName;
                    }
                }
                detailModel.BaseCurrencyExchangeRate = bill.ExchangeRate.Value;
                detailModel.IsHyperLinkEnable = true;
                decimal sumLineTotal1 = 0;
                if (memoDTO.CreditMemoDetailModels.Any())
                {
                    sumLineTotal1 = memoDTO.CreditMemoDetailModels.Sum(od => od.DocTotalAmount);
                }
                detailModel.CreditAmount = bill.BalanceAmount;
                lstCNADModel.Add(detailModel);

                memoDTO.CreditMemoApplicationModel.CreditMemoApplicationDetailModels = lstCNADModel;
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_CreateCreditMemoBybill_CreateCall_SuccessFully_Message);
            }

            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return memoDTO;
        }

        #endregion

        #region CreatePayment By Bill
        public PaymentModel CreatePaymentByBill(Guid paymentId, long companyId, string docType, string connectionString)
        {
            PaymentModel paymentModel = new PaymentModel();
            try
            {
                //AppsWorld.BillModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, docType == /*BillConstant.Payroll_Bill*/DocTypeConstants.PayrollBill ? BillConstant.Payroll_Payment : BillConstant.Payment);
                //Payment lastPayment = _paymentService.GetPaymentByComapnyId(companyId, docType == DocTypeConstants.Payroll ? /*BillConstant.Payroll_Payment*/ DocTypeConstants.Payroll : BillConstant.Payment);

                //to check if it is void or not
                if (_billService.IsVoid(companyId, paymentId))
                    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                paymentModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                paymentModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Bill _bill = _billService.GetBillById(paymentId, companyId, docType);
                if (_bill != null)
                {
                    paymentModel.Id = Guid.NewGuid();
                    paymentModel.CompanyId = _bill.CompanyId;
                    paymentModel.BaseCurrency = _bill.BaseCurrency;
                    //paymentModel.IsGSTApplied = _bill.IsGSTApplied;
                    //paymentModel.ExtensionType = docType;
                    paymentModel.DocSubType = _bill.DocSubType;

                    paymentModel.CurrencyCode = _bill.DocCurrency;
                    var currency = _currencyService.GetCurrencyByCode(_bill.CompanyId, paymentModel.CurrencyCode);
                    if (currency != null)
                        paymentModel.CurrencyName = currency.Name;
                    paymentModel.DocCurrency = _bill.DocCurrency;
                    paymentModel.DocDate = _bill.DocumentDate;

                    //paymentModel.DocNo = GetNewPaymentDocumentNo(_bill.CompanyId, docType, _bill.DocNo);

                    //bool? isEdit = false;
                    //paymentModel.DocNo = GetAutoNumberByEntityType(companyId, lastPayment, docType, _autoNo, _bill.DocNo, ref isEdit);
                    //paymentModel.IsDocNoEditable = isEdit;

                    paymentModel.IsDocNoEditable = _autoService.GetAutoNumberIsEditable(companyId, docType == DocTypeConstants.Payroll ? BillConstant.Payroll_Payment : BillConstant.Payment);
                    if (paymentModel.IsDocNoEditable == true)
                        paymentModel.DocNo = _autoService.GetAutonumber(companyId, docType == DocTypeConstants.Payroll ? BillConstant.Payroll_Payment : BillConstant.Payment, connectionString);

                    paymentModel.DueDate = _bill.DueDate;
                    paymentModel.EntityId = _bill.EntityId;
                    //BeanEntity beanEntity = _beanEntityService.GetEntityById(_bill.EntityId);
                    //paymentModel.EntityName = beanEntity.Name;
                    paymentModel.EntityName = _beanEntityService.GetEntityName(companyId, _bill.EntityId);
                    paymentModel.ExDurationFrom = _bill.ExDurationFrom;
                    paymentModel.ExDurationTo = _bill.ExDurationTo;
                    //paymentModel.GrandTotal = _bill.GrandTotal;
                    paymentModel.GstdurationFrom = _bill.GSTExDurationFrom;
                    paymentModel.GstDurationTo = _bill.GSTExDurationTo;
                    paymentModel.GstReportingCurrency = _bill.GSTExCurrency;
                    //paymentModel.GSTTotalAmount = _bill.GSTTotalAmount;
                    //Journal journal = _journalService.GetJournals(_bill.Id, _bill.CompanyId);
                    //if (journal != null)
                    //    paymentModel.JournalId = journal.Id;
                    //paymentModel.IsAllowDisAllow = _bill.IsAllowableDisallowable;
                    //paymentModel.IsDisAllow = _bill.IsAllowableDisallowable;
                    paymentModel.ISMultiCurrency = _bill.IsMultiCurrency;
                    paymentModel.IsNoSupportingDocument = _bill.IsNoSupportingDocument;

                    paymentModel.NoSupportingDocument = _bill.NoSupportingDocument;
                    paymentModel.ServiceCompanyId = _bill.ServiceCompanyId;
                    var company = _companyService.GetById(_bill.ServiceCompanyId);
                    //if (company != null)
                    //    paymentModel.ServiceCompanyName = company.ShortName;
                    paymentModel.SystemRefNo = _bill.SystemReferenceNumber;
                    paymentModel.Remarks = _bill.DocDescription;
                    paymentModel.UserCreated = _bill.UserCreated;
                    paymentModel.CreatedDate = _bill.CreatedDate;
                    paymentModel.ModifiedBy = _bill.ModifiedBy;
                    paymentModel.ModifiedDate = _bill.ModifiedDate;
                    paymentModel.ExtensionType = _bill.DocSubType == DocTypeConstants.General || _bill.DocSubType == DocTypeConstants.Claim ? DocTypeConstants.Bills : _bill.DocSubType == DocTypeConstants.Payroll ? DocTypeConstants.Payroll : DocTypeConstants.OBBill;
                    paymentModel.ExCurrency = _bill.BaseCurrency;
                    paymentModel.ExchangeRate = _bill.ExchangeRate;

                    //paymentModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(_bill.CompanyId);
                    //paymentModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(_bill.CompanyId);
                    paymentModel.DocumentState = _bill.DocumentState;

                    List<PaymentDetailModel> lstpDetailModel = new List<PaymentDetailModel>();
                    PaymentDetailModel model = new PaymentDetailModel();
                    model.Id = Guid.NewGuid();
                    model.PaymentId = paymentModel.Id;
                    model.AmmountDue = _bill.BalanceAmount;
                    model.DocumentDate = _bill.PostingDate;
                    model.DocumentNo = _bill.DocNo;
                    model.DocumentState = _bill.DocumentState;
                    model.PaymentAmount = _bill.BalanceAmount;
                    model.DocumentAmmount = _bill.GrandTotal;
                    model.Nature = _bill.Nature;
                    model.DocumentId = _bill.Id;
                    model.SystemReferenceNumber = _bill.SystemReferenceNumber;
                    if (company != null)
                    {
                        model.ServiceCompanyName = company.ShortName;
                        model.ServiceCompanyId = company.Id;
                    }
                    model.Currency = _bill.DocCurrency;
                    model.DocumentType = _bill.DocSubType;
                    lstpDetailModel.Add(model);
                    paymentModel.PaymentDetailModels = lstpDetailModel;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return paymentModel;
        }
        #endregion

        #region common_Call
        public DocTypeModel GetAllDocumentByBillId(long companyId, Guid billId, string docType, string connectionString, string username)
        {
            try
            {
                List<VendorDocumentDetailModel> billDetails = new List<VendorDocumentDetailModel>();
                DocTypeModel doctypeModel = new DocTypeModel();
                List<long> lstCompanyIds = _companyService.GetAllSubCompaniesId(username, companyId);
                //for Payment and Payroll Payment
                List<PaymentDetail> lstPaymentDetail = _paymentDetailService.GetById(billId);
                if (lstPaymentDetail.Any())
                {
                    List<VendorDocumentDetailModel> paymentDocumentDetails = null;
                    var receipts = lstPaymentDetail.Select(a => a.Payment);
                    paymentDocumentDetails = receipts.Select(a => new VendorDocumentDetailModel()
                    {
                        Id = a.Id,
                        DocNo = a.DocNo,
                        DocDate = a.DocDate,
                        DocType = docType == DocTypeConstants.Bills ? DocTypeConstants.Payment : DocTypeConstants.PayrollPayment,
                        Amount = a.DocumentState != InvoiceState.Void ? lstPaymentDetail.Where(c => c.PaymentId == a.Id).Select(x => x.PaymentAmount).FirstOrDefault() : (decimal?)null,
                        IsHyperLinkEnable = lstCompanyIds.Contains(a.ServiceCompanyId)
                    }).ToList();
                    doctypeModel.PaymentTotalAmount = paymentDocumentDetails.Sum(a => a.Amount);
                    billDetails.AddRange(paymentDocumentDetails);
                }

                //for Credit Memo
                CreditMemo cm = new CreditMemo();
                List<CreditMemoApplicationDetail> lstCMADetail = _creditMemoApplicationDetailService.GetCreditMemoDetailById(billId);
                if (lstCMADetail.Any())
                {
                    List<VendorDocumentDetailModel> creditMemos = new List<VendorDocumentDetailModel>();
                    List<Guid> cnaDetailIds = lstCMADetail.Select(a => a.CreditMemoApplicationId).ToList();
                    List<CreditMemoApplication> lstCreditMemosApps = _creditMemoApplicationService.GetListofCreditMemoById(cnaDetailIds);
                    if (lstCreditMemosApps.Any())
                    {
                        cm = _creditMemoService.GetCmById(lstCreditMemosApps.Select(c => c.CreditMemoId).FirstOrDefault(), companyId);
                        creditMemos = lstCMADetail.Select(a => new VendorDocumentDetailModel()
                        {
                            Id = cm.Id,
                            DocNo = lstCreditMemosApps.Where(c => c.Id == a.CreditMemoApplicationId).Select(c => c.CreditMemoApplicationNumber).FirstOrDefault(),
                            DocDate = lstCreditMemosApps.Where(c => c.Id == a.CreditMemoApplicationId).Select(c => c.CreditMemoApplicationDate).FirstOrDefault(),
                            DocType = DocTypeConstants.CMApplication,
                            Amount = lstCreditMemosApps.Where(d => d.Id == a.CreditMemoApplicationId).Select(d => d.Status).FirstOrDefault() != CreditMemoApplicationStatus.Void ? a.CreditAmount : (decimal?)null,
                            IsHyperLinkEnable = lstCompanyIds.Contains(cm.ServiceCompanyId)
                        }).ToList();
                        doctypeModel.CreditMemoTotalAmount = creditMemos.Sum(a => a.Amount);
                    }
                    billDetails.AddRange(creditMemos);
                }
                //For Receipt
                List<ReceiptDetailCompact> lstReceiptDetail = _billDetailService.GetById(billId);
                if (lstReceiptDetail.Any())
                {
                    List<VendorDocumentDetailModel> receiptDocumentDetails = new List<VendorDocumentDetailModel>();
                    var receipts = lstReceiptDetail.Select(a => a.Receipts);
                    receiptDocumentDetails = receipts.Select(a => new VendorDocumentDetailModel()
                    {
                        Id = a.Id,
                        DocNo = a.DocNo,
                        DocDate = a.DocDate,
                        DocType = DocTypeConstants.Receipt,
                        Amount = a.DocumentState != InvoiceState.Void ? lstReceiptDetail.Where(c => c.ReceiptId == a.Id).Select(x => x.ReceiptAmount).FirstOrDefault() : (decimal?)null,
                        IsHyperLinkEnable = lstCompanyIds.Contains(a.ServiceCompanyId)
                    }).ToList();
                    doctypeModel.PaymentTotalAmount = receiptDocumentDetails.Sum(a => a.Amount);
                    billDetails.AddRange(receiptDocumentDetails);
                }
                if (docType == DocTypeConstants.Bills)
                {
                    List<VendorDocumentDetailModel> transferDocumentDetails = new List<VendorDocumentDetailModel>();
                    query = $"Select BT.Id,BT.DocNo,BT.TransferDate,SD.SettledAmount,BT.DocType from Bean.BankTransfer BT  join Bean.SettlementDetail SD  on BT.Id = SD.BankTransferId and BT.CompanyId ={companyId} join Bean.Bill B on B.PayrollId = SD.DocumentId where BT.DocumentState != 'Void' and B.Id = '{billId}' and B.DocSubType = 'Interco' and B.CompanyId ={companyId}";
                    using (con = new SqlConnection(connectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            VendorDocumentDetailModel docModel = new VendorDocumentDetailModel();
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
                        doctypeModel.PaymentTotalAmount = transferDocumentDetails.Sum(a => a.Amount);
                        billDetails.AddRange(transferDocumentDetails);
                    }
                }

                doctypeModel.BillDetailModels = billDetails.OrderBy(c => c.DocNo).ThenBy(d => d.DocDate).ToList();
                if (doctypeModel.BillDetailModels.Any())
                    doctypeModel.AmountDue = _billService.GetBillBalAmount(billId, companyId) ?? null;//for Balane Amount
                return doctypeModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region KendoGrid Block
        public IQueryable<Bill> GetAllBillsK(long companyId)
        {
            return _billService.GetAllBills(companyId);
        }

        public IQueryable<BillModel> GetAllBillModelk(long companyId)
        {
            List<BillModel> lstBill = new List<BillModel>();
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillModelk_GetCall_Request_Message);
                IQueryable<Bill> bills = _billService.GetAllBills(companyId);
                foreach (Bill bill in bills)
                {
                    BillModel billModel = new BillModel();
                    FillBillModelk(billModel, bill);
                    lstBill.Add(billModel);
                }
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillModelk_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_GetAllBillModelk_GetCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return lstBill.AsQueryable();
        }

        public IQueryable<BillModelK> GetAllBillssK(string username, long companyId)
        {
            //return _billService.GetAllBillsK(companyId, "Bill");
            return _billService.GetAllBillsK(username, companyId, "Bill");
        }

        public IQueryable<BillModelK> GetAllPayrollBillsK(string username, long companyId)
        {
            return _billService.GetAllBillsK(username, companyId, DocTypeConstants.PayrollBill);
        }
        #endregion KendoGrid

        #region Save Void For Bill and Payroll Bill
        public Bill SaveBillNoteDocumentVoid(DocumentVoidModel TObject)
        {
            string DocNo = "-V";
            string DocDescription = "Void-";
            string oldDocNo;
            Bill _document = _billService.GetDocNoById(TObject.Id, TObject.CompanyId);
            if (_document == null)
                throw new Exception(BillConstant.Invalid_BillNote);
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }

            if (_document.DocumentState != BillNoteState.NotPaid)
                throw new Exception("State should be " + BillNoteState.NotPaid);
            if (_billService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
            if (_document.BillDetails.Any(s => s.ClearingState == BillNoteState.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            if (_document.ClearCount != null && _document.ClearCount > 0)
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);
            //Need to verify the invoice is within Financial year

            if (!_financialSettingService.ValidateYearEndLockDate(_document.PostingDate, TObject.CompanyId))
            {
                throw new Exception(BillConstant.Posting_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.PostingDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(BillConstant.Posting_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.PostingDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
            }
            if (_document.BillDetails.Any(s => s.ClearingState == BillNoteState.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

            _document.DocumentState = BillNoteState.Void;
            oldDocNo = _document.DocNo;
            _document.DocNo = _document.DocNo + DocNo;
            _document.ModifiedBy = TObject.ModifiedBy;
            _document.ModifiedDate = DateTime.UtcNow;
            //_document.DocumentDescription = DocDescription + _document.DocumentDescription;
            _document.ObjectState = ObjectState.Modified;
            try
            {
                _unitOfWorkAsync.SaveChanges();
                JournalDeleteModel tObject = new JournalDeleteModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = _document.DocNo;
                tObject.ModifiedBy = TObject.ModifiedBy;
                deleteJVPostInvoce(tObject);
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_SaveBillNoteDocumentVoid_SaveCall_SuccessFully_Message);

                //try
                //{
                //    if (_document.DocSubType != DocTypeConstants.Payroll)
                //        saveScreenRecords(_document.Id.ToString(), _document.EntityId.ToString(), _document.EntityId.ToString(), _document.DocNo, _document.UserCreated, DateTime.UtcNow, false, _document.CompanyId, oldDocNo, _document.EntityId.ToString());
                //}
                //catch (Exception ex)
                //{

                //    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Issues_in_Bill_Folder_creation);
                //}

            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_SaveBillNoteDocumentVoid_SaveCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return _document;
        }

        public bool? SavePayrollBillDocVoid(DocumentVoidModel TObject)
        {
            string DocNo = "-V";
            string oldDocNo = null;
            //string DocDescription = "Void-";
            List<Bill> lstPayrollBill = _billService.GetAllPayrollBill(TObject.PayrollId, TObject.CompanyId);
            if (lstPayrollBill.Any())
            {
                foreach (var _document in lstPayrollBill)
                {
                    if (_document == null)
                        throw new Exception(BillConstant.Invalid_BillNote);

                    if (_document.DocumentState != BillNoteState.NotPaid)
                        throw new Exception("State should be " + BillNoteState.NotPaid);

                    //Need to verify the invoice is within Financial year

                    //if (!_financialSettingService.ValidateYearEndLockDate(_document.PostingDate, TObject.CompanyId))
                    //{
                    //    throw new Exception(BillConstant.Posting_date_is_in_closed_financial_period_and_cannot_be_posted);
                    //}

                    //Verify if the invoice is out of open financial period and lock password is entered and valid
                    //if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.PostingDate, TObject.CompanyId))
                    //{
                    //    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    //    {
                    //        throw new Exception(BillConstant.Posting_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    //    }
                    //    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.PostingDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    //    {
                    //        throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                    //    }
                    //}

                    _document.DocumentState = BillNoteState.Void;
                    oldDocNo = _document.DocNo;
                    _document.DocNo = _document.DocNo + DocNo;
                    _document.ModifiedBy = TObject.ModifiedBy;
                    _document.ModifiedDate = TObject.ModifiedDate;
                    //_document.DocumentDescription = DocDescription + _document.DocumentDescription;
                    _document.ObjectState = ObjectState.Modified;
                    //var journal = _journalService.GetJournals(_document.Id, _document.CompanyId);
                    //if (journal != null)
                    //{

                    //    journal.ObjectState = ObjectState.Deleted;
                    //    _journalService.Delete(journal);
                    //}
                    try
                    {
                        JournalDeleteModel tObject = new JournalDeleteModel();
                        tObject.Id = _document.Id;
                        tObject.CompanyId = TObject.CompanyId;
                        tObject.DocNo = _document.DocNo;
                        tObject.ModifiedBy = TObject.ModifiedBy;
                        deleteJVPostInvoce(tObject);
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_SaveBillNoteDocumentVoid_SaveCall_SuccessFully_Message);
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_SaveBillNoteDocumentVoid_SaveCall_Exception_Message);
                        Log.Logger.ZCritical(ex.StackTrace);
                        throw ex;
                    }
                }
            }
            _unitOfWorkAsync.SaveChanges();

            //try
            //{
            //    if (lstPayrollBill.Any())
            //    {
            //        Bill bill = lstPayrollBill.FirstOrDefault();
            //        if (bill != null)
            //        {
            //            if (bill.DocSubType == DocTypeConstants.Claim)
            //            {
            //                //if (_document.DocSubType != DocTypeConstants.Payroll)
            //                saveScreenRecords(bill.Id.ToString(), bill.EntityId.ToString(), bill.EntityId.ToString(), bill.DocNo, bill.UserCreated, DateTime.UtcNow, false, bill.CompanyId, oldDocNo, bill.EntityId.ToString());
            //            }
            //        }

            //    }

            //}
            //catch (Exception ex)
            //{

            //    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Issues_in_Bill_Folder_creation);
            //}


            return true;
        }
        #endregion

        #region posting
        public void SaveBillJv(JVModel clientModel)
        {
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Entred_into_SaveBillJv_Method);
            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == BillConstants.IdentityBean)
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
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }
        private void FillJournal(JVModel headJournal, Bill invoice, bool isNew)
        {

            string strServiceCompany = _companyService.GetById(invoice.ServiceCompanyId).ShortName;

            //TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice);
            //JournalModel headJournal = new JournalModel();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillJv(headJournal, invoice);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel Jmodel = new JVVDetailModel();
            foreach (BillDetail detail in invoice.BillDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillJvDetals(invoice, journal, detail, Jmodel);
                //journal.RecOrder = recOreder + 1;
                //recOreder = journal.RecOrder;
                journal.RecOrder = detail.RecOrder;
                lstJD.Add(journal);

            }
            FillJvInDetail(invoice, Jmodel);
            if (headJournal.ModifiedDate != null && headJournal.ModifiedBy != null)
                Jmodel.AmountDue = invoice.DocumentState != InvoiceState.NotPaid ? headJournal.BalanceAmount : null;
            //Jmodel.RecOrder = recOreder + 1;
            //recOreder = Jmodel.RecOrder;
            lstJD.Add(Jmodel);
            if (invoice.IsGstSettings)
            {
                ChartOfAccount gstAccount = _chartOfAccountService.GetByName(COANameConstants.TaxPayableGST, invoice.CompanyId);
                foreach (BillDetail detail in invoice.BillDetails.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    FillJvGstDetail(invoice, journal, detail, gstAccount, Jmodel);
                    journal.RecOrder = detail.RecOrder;
                    lstJD.Add(journal);
                }
            }
            headJournal.JVVDetailModels = lstJD/*.Where(a => a.DocCredit > 0 || a.DocDebit > 0)*/.OrderBy(x => x.RecOrder).ToList();
        }

        private void FillJvGstDetail(Bill invoice, JVVDetailModel journal, BillDetail detail, ChartOfAccount gstAccount,
            JVVDetailModel Jmodel)
        {
            ChartOfAccount account;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.ServiceCompanyId = invoice.ServiceCompanyId;
            journal.DocNo = invoice.DocNo;
            //journal.DocType = DocTypeConstants.Bills;
            //journal.AccountDescription = detail.Description;
            journal.AccountDescription = invoice.DocDescription;
            journal.DocDate = invoice.DocumentDate;
            journal.PostingDate = invoice.PostingDate;
            journal.DocSubType = invoice.DocSubType;
            journal.DocType = invoice.DocType;
            account = _chartOfAccountService.GetChartOfAccountById(gstAccount.Id);
            //_chartOfAccountRepository.Query(a => a.Id == detail.COAId).Select().FirstOrDefault();
            journal.COAId = account.Id;
            journal.AccountCode = account.Code;
            journal.AccountName = account.Name;
            journal.DocCurrency = invoice.DocCurrency;
            journal.BaseCurrency = invoice.BaseCurrency;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxById(detail.TaxId);
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxCode = detail.TaxCode;
                journal.TaxRate = detail.TaxRate;
                journal.TaxType = detail.TaxType;
            }
            journal.DocDescription = detail.Description;
            if (invoice.DocSubType == DocTypeConstants.General)
            {
                if (detail.DocAmount >= 0)
                {
                    journal.DocDebit = detail.DocTaxAmount;
                    journal.BaseDebit = Math.Round(journal.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * Jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    journal.GSTDebit = journal.DocDebit == 0 ? 0 : journal.DocDebit * invoice.GSTExchangeRate;
                }
                else
                {
                    journal.DocCredit = -(detail.DocTaxAmount);
                    //decimal? bd = journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate;
                    journal.BaseCredit = Math.Round((decimal)(journal.ExchangeRate == null ? journal.DocCredit : journal.DocCredit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                if (detail.DocTaxAmount >= 0)
                {
                    journal.DocDebit = detail.DocTaxAmount;
                    journal.BaseDebit = Math.Round((decimal)journal.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * Jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    journal.GSTDebit = journal.DocDebit == 0 ? 0 : journal.DocDebit * invoice.GSTExchangeRate;
                }
                else
                {
                    journal.DocCredit = detail.DocTaxAmount;
                    journal.BaseCredit = Math.Round((decimal)journal.ExchangeRate != null ? (decimal)journal.DocCredit : (decimal)(journal.DocCredit * Jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    journal.GSTCredit = journal.DocCredit == 0 ? 0 : journal.DocCredit * invoice.GSTExchangeRate;
                }
            }
            journal.IsTax = true;
        }

        private void FillJvDetals(Bill bill, JVVDetailModel journal, BillDetail detail, JVVDetailModel Jmodel)
        {
            journal.DocumentDetailId = detail.Id;
            //if (invoice.DocSubType == DocTypeConstants.Bills)
            journal.DocumentId = bill.Id;
            //else
            //    journal.DocumentId = invoice.PayrollId.Value;
            journal.Nature = bill.Nature;
            journal.SystemRefNo = bill.SystemReferenceNumber;
            journal.DocDate = bill.DocumentDate;
            journal.ServiceCompanyId = bill.ServiceCompanyId;
            journal.DocNo = bill.DocNo;
            //journal.DocType = DocTypeConstants.Bill;
            //journal.DocSubType = bill.DocSubType;

            //if (bill.DocSubType == DocTypeConstants.Bills)
            //    journal.DocSubType = DocTypeConstants.General;
            //else
            //    journal.DocSubType = DocTypeConstants.PayrollBill;
            //journal.DocType = bill.DocSubType;

            journal.DocType = bill.DocType;
            journal.DocSubType = bill.DocSubType;

            journal.EntityId = bill.EntityId;
            journal.AccountDescription = detail.Description;
            //ChartOfAccount account1 =_chartOfAccountService.Query(a => a.Name ==(journal.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables) &&
            //            a.CompanyId == invoice.CompanyId).Select().FirstOrDefault();
            //if (account1 != null)
            //{
            //    journal.COAId = account1.Id;
            //    journal.AccountCode = account1.Code;
            //    journal.AccountName = account1.Name;
            //}
            journal.PostingDate = bill.PostingDate; ;
            journal.AllowDisAllow = detail.IsDisallow;
            journal.COAId = detail.COAId;
            journal.DocCurrency = bill.DocCurrency;
            journal.BaseCurrency = bill.BaseCurrency;
            journal.ExchangeRate = bill.ExchangeRate;
            journal.GSTExCurrency = bill.GSTExCurrency;
            journal.GSTExchangeRate = bill.GSTExchangeRate;
            if (bill.IsGstSettings)
            {
                if (detail.TaxId != null)
                {
                    //TaxCode tax = _taxCodeService.GetTaxById(detail.TaxId);
                    //journal.TaxId = tax.Id;
                    //journal.TaxCode = tax.Code;
                    //journal.TaxRate = tax.TaxRate;
                    //journal.TaxType = tax.TaxType;
                    journal.TaxId = detail.TaxId;
                    journal.TaxCode = detail.TaxCode;
                    journal.TaxRate = detail.TaxRate;
                    journal.TaxType = detail.TaxType;
                }
            }
            journal.DocDescription = detail.Description;
            if (bill.DocSubType == DocTypeConstants.General)
            {
                if (detail.DocAmount >= 0)
                {
                    journal.DocDebit = detail.DocAmount;
                    //decimal? bd = journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate;
                    journal.BaseDebit = journal.DocDebit != null ? ((decimal?)Math.Round((decimal)(journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero)) : null;
                    journal.DocTaxDebit = detail.DocTaxAmount;
                    journal.BaseTaxDebit = detail.BaseTaxAmount;
                }
                else
                {
                    journal.DocCredit = -(detail.DocAmount);
                    //decimal? bd = journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate;
                    journal.BaseCredit = Math.Round((decimal)(journal.ExchangeRate == null ? journal.DocCredit : journal.DocCredit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    journal.DocTaxCredit = detail.DocTaxAmount;
                    journal.BaseTaxCredit = detail.BaseTaxAmount;
                }
            }
            else
            {
                if (detail.DocAmount >= 0)
                {
                    journal.DocDebit = detail.DocAmount;
                    //decimal? bd = journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate;
                    journal.BaseDebit = journal.DocDebit != null ? ((decimal?)Math.Round((decimal)(journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero)) : null;
                    journal.DocTaxDebit = detail.DocTaxAmount;
                    journal.BaseTaxDebit = detail.BaseTaxAmount;
                }
                else
                {
                    journal.DocCredit = detail.DocAmount;
                    //decimal? bd = journal.ExchangeRate == null ? journal.DocDebit : journal.DocDebit * journal.ExchangeRate;
                    journal.BaseCredit = Math.Round((decimal)(journal.ExchangeRate == null ? journal.DocCredit : journal.DocCredit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    journal.DocTaxCredit = detail.DocTaxAmount;
                    journal.BaseTaxCredit = detail.BaseTaxAmount;
                }
            }
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.BaseTaxableAmount = detail.BaseAmount;
            journal.BaseTaxAmount = detail.BaseTaxAmount;
            //journal.SegmentCategory1 = bill.SegmentCategory1;
            //journal.SegmentCategory2 = bill.SegmentCategory2;
            //journal.SegmentMasterid1 = bill.SegmentMasterid1;
            //journal.SegmentMasterid2 = bill.SegmentMasterid2;
            //journal.SegmentDetailid1 = bill.SegmentDetailid1;
            //journal.SegmentDetailid2 = bill.SegmentDetailid2;
            journal.IsTax = false;
            if (bill.GSTExchangeRate != null)
            {
                journal.GSTTaxableAmount = Math.Round((decimal)journal.DocTaxableAmount * bill.GSTExchangeRate.Value, 2);
                journal.GSTTaxAmount = Math.Round((decimal)journal.DocTaxAmount == 0 ? 0 : journal.DocTaxAmount.Value * bill.GSTExchangeRate.Value, 2);
            }
            journal.DocCreditTotal = detail.DocTotalAmount;
        }

        private void FillJvInDetail(Bill bill, JVVDetailModel Jmodel)
        {
            Jmodel.DocumentId = bill.Id;
            Jmodel.DocNo = bill.DocNo;

            //if (invoice.DocSubType == DocTypeConstants.Bills)
            //{
            //    Jmodel.DocType = DocTypeConstants.Bills;
            //    // Jmodel.DocumentId = invoice.Id;
            //}
            //else
            //{
            //    Jmodel.DocType = DocTypeConstants.PayrollBill;
            //    //Jmodel.DocumentId = invoice.PayrollId.Value;
            //}
            //Jmodel.DocSubType = invoice.DocSubType;

            //if (bill.DocSubType == DocTypeConstants.Bills)
            //    Jmodel.DocSubType = DocTypeConstants.General;
            //else
            //    Jmodel.DocSubType = DocTypeConstants.PayrollBill;
            //Jmodel.DocType = bill.DocSubType;

            Jmodel.DocType = bill.DocType;
            Jmodel.DocSubType = bill.DocSubType;

            Jmodel.DocDate = bill.DocumentDate;
            Jmodel.DocCurrency = bill.DocCurrency;
            Jmodel.Nature = bill.Nature;
            Jmodel.SystemReferenceNo = bill.SystemReferenceNumber;
            Jmodel.SystemRefNo = bill.SystemReferenceNumber;
            Jmodel.AccountDescription = bill.DocDescription;
            Jmodel.PostingDate = bill.PostingDate;
            Jmodel.ServiceCompanyId = bill.ServiceCompanyId;
            Jmodel.ExDurationFrom = bill.ExDurationFrom;
            Jmodel.ExDurationTo = bill.ExDurationTo;
            //Jmodel.NoSupportingDocument = invoice.NoSupportingDocs;
            //Jmodel.EntityId = invoice.EntityId;
            //BeanEntity entity1 = _beanEntityService.Query(a => a.Id == bill.EntityId).Select().FirstOrDefault();
            //Jmodel.EntityName = entity1.Name;
            Jmodel.EntityType = bill.EntityType;
            Jmodel.EntityId = bill.EntityId;
            ChartOfAccount account1 =
                _chartOfAccountService.GetByName(
                    bill.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables,
                    bill.CompanyId);
            Jmodel.COAId = account1.Id;
            Jmodel.AccountName = account1.Name;
            Jmodel.BaseCurrency = bill.BaseCurrency;
            Jmodel.ExchangeRate = bill.ExchangeRate;
            if (bill.IsGstSettings)
            {
                Jmodel.GSTExCurrency = bill.GSTExCurrency;
                Jmodel.GSTExchangeRate = bill.GSTExchangeRate;
            }
            if (bill.DocSubType == /*DocTypeConstants.Bills*/DocTypeConstants.General)
            {
                if (bill.GrandTotal >= 0)
                    Jmodel.DocCredit = bill.GrandTotal;
                else
                    Jmodel.DocDebit = bill.GrandTotal;
                //Jmodel.DocCredit = bill.GrandTotal;
                decimal amount = 0;

                amount = (bill.GrandTotal != null && bill.GrandTotal != 0) ? Jmodel.ExchangeRate != null ? Math.Round(bill.GrandTotal * (decimal)Jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero) : Math.Round(bill.GrandTotal, 2, MidpointRounding.AwayFromZero) : bill.GrandTotal;
                //foreach (var detail in bill.BillDetails)
                //{
                //    amount = Math.Round((decimal)(amount + (detail.DocAmount * Jmodel.ExchangeRate)), 2/*, MidpointRounding.AwayFromZero*/);
                //    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * Jmodel.ExchangeRate)), 2/*, MidpointRounding.AwayFromZero*/);
                //}
                //Jmodel.BaseCredit = amount;
                if (amount >= 0)
                    Jmodel.BaseCredit = Math.Abs(bill.BaseGrandTotal.Value);
                else
                    Jmodel.BaseDebit = Math.Abs(bill.BaseGrandTotal.Value);
            }
            else
            {
                if (bill.GrandTotal >= 0)
                    Jmodel.DocCredit = bill.GrandTotal;
                else
                    Jmodel.DocDebit = bill.GrandTotal;
                decimal amount = 0;
                amount = (bill.GrandTotal != null && bill.GrandTotal != 0) ? Jmodel.ExchangeRate != null ? Math.Round(bill.GrandTotal * (decimal)Jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero) : Math.Round(bill.GrandTotal, 2, MidpointRounding.AwayFromZero) : bill.GrandTotal;
                //foreach (var detail in bill.BillDetails)
                //{
                //    amount = Math.Round((decimal)(amount + (detail.DocAmount * Jmodel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                //    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * Jmodel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                //}
                if (amount >= 0)
                    Jmodel.BaseCredit = Math.Abs(bill.BaseGrandTotal.Value);
                else
                    Jmodel.BaseDebit = Math.Abs(bill.BaseGrandTotal.Value);
            }
            //Math.Round((decimal)Jmodel.ExchangeRate == null ? Jmodel.DocCredit : (Jmodel.DocCredit * Jmodel.ExchangeRate).Value, 2,MidpointRounding.AwayFromZero);
        }

        private void FillJv(JVModel headJournal, Bill bill)
        {
            //if (invoice.DocSubType == DocTypeConstants.Bills)
            headJournal.DocumentId = bill.Id;
            //else
            //    headJournal.DocumentId = invoice.PayrollId.Value;
            headJournal.CompanyId = bill.CompanyId;
            headJournal.PostingDate = bill.PostingDate;
            headJournal.DocNo = bill.DocNo;

            //if (invoice.DocSubType == DocTypeConstants.Bills)
            //    headJournal.DocType = DocTypeConstants.Bills;
            //else
            //    headJournal.DocType = DocTypeConstants.PayrollBill;
            //headJournal.DocSubType = invoice.DocSubType;

            headJournal.DocType = bill.DocType;
            headJournal.DocSubType = bill.DocSubType;

            //if (bill.DocSubType == DocTypeConstants.Bills)
            //    headJournal.DocSubType = DocTypeConstants.General;
            //else
            //    headJournal.DocSubType = DocTypeConstants.PayrollBill;
            //headJournal.DocType = bill.DocSubType;

            headJournal.DocDate = bill.DocumentDate;
            headJournal.DueDate = bill.DueDate;
            headJournal.DocumentState = bill.DocumentState;
            headJournal.DocumentDescription = bill.DocDescription;
            headJournal.SystemReferenceNo = bill.SystemReferenceNumber;
            headJournal.ServiceCompanyId = bill.ServiceCompanyId;
            headJournal.ExDurationFrom = bill.ExDurationFrom;
            headJournal.IsGstSettings = bill.IsGstSettings;
            headJournal.IsGSTApplied = bill.IsGSTApplied;
            headJournal.IsMultiCurrency = bill.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = bill.IsNoSupportingDocument;
            headJournal.ExDurationTo = bill.ExDurationTo;
            headJournal.GSTExDurationFrom = bill.GSTExDurationFrom;
            headJournal.GSTExDurationTo = bill.GSTExDurationTo;
            headJournal.NoSupportingDocument = bill.NoSupportingDocument;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            //headJournal.SegmentCategory1 = bill.SegmentCategory1;
            //headJournal.SegmentCategory2 = bill.SegmentCategory2;
            //headJournal.SegmentMasterid1 = bill.SegmentMasterid1;
            //headJournal.SegmentMasterid2 = bill.SegmentMasterid2;
            //headJournal.SegmentDetailid1 = bill.SegmentDetailid1;
            //headJournal.SegmentDetailid2 = bill.SegmentDetailid2;
            headJournal.IsAllowableNonAllowable = bill.IsAllowableDisallowable;
            headJournal.EntityId = bill.EntityId;
            //BeanEntity entity = _beanEntityService.Query(a => a.Id == bill.EntityId).Select().FirstOrDefault();
            //headJournal.EntityName = entity.Name;
            headJournal.EntityType = bill.EntityType;
            ChartOfAccount account =
                _chartOfAccountService.GetByName(
                    bill.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables,
                    bill.CompanyId);
            //_chartOfAccountRepository.Query(a => a.Name == (invoice.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables)).Select().FirstOrDefault();
            headJournal.COAId = account.Id;
            //headJournal.AccountCode = account.Code;
            headJournal.AccountName = account.Name;
            headJournal.DocCurrency = bill.DocCurrency;
            // headJournal.GrandBaseCreditTotal = invoice.GrandTotal;
            headJournal.BaseCurrency = bill.BaseCurrency;
            headJournal.ExchangeRate = bill.ExchangeRate;
            headJournal.Nature = bill.Nature;
            headJournal.GrandDocDebitTotal = bill.GrandTotal;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(bill.GrandTotal * (bill.ExchangeRate != null ? bill.ExchangeRate : 1)), 2);
            if (bill.IsGstSettings)
            {
                headJournal.GSTExCurrency = bill.GSTExCurrency;
                headJournal.GSTExchangeRate = bill.GSTExchangeRate;
            }
            headJournal.CreditTermsId = bill.CreditTermsId.Value;
            headJournal.Remarks = bill.DocDescription;
            headJournal.BalanceAmount = bill.BalanceAmount;
            headJournal.UserCreated = bill.UserCreated;
            headJournal.CreatedDate = bill.CreatedDate;
            headJournal.ModifiedBy = bill.ModifiedBy;
            headJournal.ModifiedDate = bill.ModifiedDate;
        }

        public void deleteJVPostInvoce(JournalDeleteModel tObject)
        {
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Entred_into_deleteJVPostInvoce_Method);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == BillConstants.IdentityBean)
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
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }
        #endregion posting

        //public IQueryable<BillModel> GetAllBillModelk(long companyId)
        //{
        //	return GetAllBillModel(companyId).AsQueryable();
        //}




        //private string GetNewBillCreditMemoDocumentNumber(long CompanyId)
        //{
        //    BillCreditMemo bill = _billCreditMemoService.CreateBillCreditMemo(CompanyId);
        //    string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
        //    try
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_GetNewBillCreditMemoDocumentNumber_GetById_Request_Message);
        //        if (bill != null)
        //        {
        //            string strOldNo = String.Empty;
        //            BillCreditMemo duplicatBill;
        //            int index;
        //            strOldDocNo = bill.DocumentNumber;

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

        //               // duplicatBill = _billCreditMemoService.GetDocNo(strNewDocNo, CompanyId);
        //            } while (duplicatBill != null);
        //        }
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_GetNewBillCreditMemoDocumentNumber_GetById_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_GetNewBillCreditMemoDocumentNumber_GetById_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return strNewDocNo;
        //}









        //public void UpdateBillCreditMemo(BillModel TObject, Bill _billNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCreditMemo_UpdateCall_Request_Message);
        //        if (TObject.BillCreditMemoModel != null)
        //        {
        //            if (TObject.BillCreditMemoModel.RecordStatus == "Added")
        //            {
        //                if (TObject.BillCreditMemoModel.BillId == new Guid("00000000-0000-0000-0000-000000000000"))
        //                {
        //                    TObject.BillCreditMemoModel = null;
        //                }
        //                else
        //                {

        //                    BillCreditMemo BCMemo = new BillCreditMemo();
        //                    BillCreditMemoGSTDetail billCreditMemoGstDetail = new BillCreditMemoGSTDetail();
        //                    BCMemo.Id = TObject.BillCreditMemoModel.Id;
        //                    BCMemo.CompanyId = TObject.CompanyId;
        //                    BCMemo.BillId = TObject.Id;
        //                    BCMemo.DocumentType = DocTypeConstants.BillCreditMemo;
        //                    BCMemo.DocumentDate = TObject.BillCreditMemoModel.DocumentDate;
        //                    //BCMemo.DocumentNumber = GetNewBillCreditMemoDocumentNumber(TObject.CompanyId);
        //                    BCMemo.DocumentNumber = TObject.BillCreditMemoModel.DocumentNumber;
        //                    BCMemo.NoSupportingDocument = TObject.BillCreditMemoModel.NoSupportingDocument;
        //                    BCMemo.IsNoSupportingDocument = TObject.BillCreditMemoModel.IsNoSupportingDocument;
        //                    BCMemo.UserCreated = TObject.BillCreditMemoModel.UserCreated;
        //                    BCMemo.CreatedDate = DateTime.UtcNow;
        //                    BCMemo.Remarks = TObject.BillCreditMemoModel.Remarks;
        //                    BCMemo.Currency = TObject.BillCreditMemoModel.Currency;
        //                    BCMemo.Status = AppsWorld.Framework.RecordStatusEnum.Active;

        //                    BCMemo.ObjectState = ObjectState.Added;
        //                    _billCreditMemoService.Insert(BCMemo);
        //                    if (TObject.BillCreditMemoModel.BillCreditMemoGSTDetails != null)
        //                    {
        //                        foreach (var cmGst in TObject.BillCreditMemoModel.BillCreditMemoGSTDetails)
        //                        {
        //                            var CMGstDetail = _billCreditMemoGstDetailService.GetBillCreditMemoGstDetailById(cmGst.Id, cmGst.CreditMemoId);
        //                            if (CMGstDetail == null)
        //                            {
        //                                billCreditMemoGstDetail = cmGst;
        //                                billCreditMemoGstDetail.ObjectState = ObjectState.Added;
        //                                _billCreditMemoGstDetailService.Insert(billCreditMemoGstDetail);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else if (TObject.BillCreditMemoModel.RecordStatus != "Added" && TObject.BillCreditMemoModel.RecordStatus != "Deleted")
        //            {
        //                BillCreditMemo BCMemo = _billCreditMemoService.GetCreditMemoById(TObject.BillCreditMemoModel.Id);
        //                if (BCMemo != null)
        //                {
        //                    BillCreditMemoGSTDetail billCreditMemoGstDetail = new BillCreditMemoGSTDetail();
        //                    BCMemo.CompanyId = TObject.CompanyId;
        //                    BCMemo.BillId = TObject.BillCreditMemoModel.BillId;
        //                    BCMemo.DocumentType = DocTypeConstants.BillCreditMemo;
        //                    BCMemo.DocumentDate = TObject.BillCreditMemoModel.DocumentDate;
        //                    BCMemo.DocumentNumber = TObject.BillCreditMemoModel.DocumentNumber;
        //                    BCMemo.NoSupportingDocument = TObject.BillCreditMemoModel.NoSupportingDocument;
        //                    BCMemo.IsNoSupportingDocument = TObject.BillCreditMemoModel.IsNoSupportingDocument;
        //                    BCMemo.Remarks = TObject.BillCreditMemoModel.Remarks;
        //                    BCMemo.Currency = TObject.BillCreditMemoModel.Currency;
        //                    BCMemo.ModifiedBy = TObject.BillCreditMemoModel.ModifiedBy;
        //                    BCMemo.ModifiedDate = DateTime.UtcNow;
        //                    BCMemo.Status = TObject.BillCreditMemoModel.Status;

        //                    BCMemo.ObjectState = ObjectState.Modified;
        //                    if (TObject.BillCreditMemoModel.BillCreditMemoGSTDetails != null)
        //                    {
        //                        foreach (var cmGst in TObject.BillCreditMemoModel.BillCreditMemoGSTDetails)
        //                        {
        //                            var CMGstDetail = _billCreditMemoGstDetailService.GetBillCreditMemoGstDetailById(cmGst.Id, cmGst.CreditMemoId);
        //                            if (CMGstDetail == null)
        //                            {
        //                                billCreditMemoGstDetail = cmGst;
        //                                //billCreditMemoGstDetail.ObjectState = ObjectState.Added;
        //                                _billCreditMemoGstDetailService.Insert(billCreditMemoGstDetail);
        //                            }
        //                            else
        //                            {

        //                                UpdateBillCreditMemoGstDetail(CMGstDetail, cmGst);
        //                                CMGstDetail.ObjectState = ObjectState.Modified;
        //                                _billCreditMemoGstDetailService.Update(CMGstDetail);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else if (TObject.BillCreditMemoModel.RecordStatus == "Deleted")
        //            {
        //                BillCreditMemo BCMemo = _billCreditMemoService.GetCreditMemoById(TObject.BillCreditMemoModel.Id);
        //                if (BCMemo != null)
        //                {
        //                    BCMemo.ObjectState = ObjectState.Deleted;
        //                    _billCreditMemoService.Delete(BCMemo);
        //                }
        //            }
        //            LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCreditMemo_UpdateCall_SuccessFully_Message);
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCreditMemo_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}
        //private void AddBillCreditMemoDetails(BillCreditMemoDetail BCMdetail, BillDetailModel detail)
        //{
        //    BCMdetail.Id = Guid.NewGuid();
        //    BCMdetail.BillDetailId = detail.Id;
        //    BCMdetail.DocCreditMemoAmount = detail.BillCreditMemoDetailModel.DocCreditMemoAmount;
        //    BCMdetail.DocCreditMemoTaxAmount = detail.BillCreditMemoDetailModel.DocCreditMemoTaxAmount;
        //    BCMdetail.DocTotal = detail.BillCreditMemoDetailModel.DocToTal;
        //    BCMdetail.BaseCreditMemoAmount = detail.BillCreditMemoDetailModel.BaseCreditMemoAmount;
        //    BCMdetail.BaseCreditMemoTaxAmount = detail.BillCreditMemoDetailModel.BaseCreditMemoTaxAmount;
        //    BCMdetail.BaseTotal = detail.BillCreditMemoDetailModel.BaseToTal;
        //}
        //private void UpdateBillCrediMemoDetailsDetails(BillCreditMemoDetail billCMDetail, BillDetailModel detail)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCrediMemoDetailsDetails_UpdateCall_Request_Message);
        //        billCMDetail.BillDetailId = detail.Id;
        //        billCMDetail.DocCreditMemoAmount = detail.BillCreditMemoDetailModel.DocCreditMemoAmount;
        //        billCMDetail.DocCreditMemoTaxAmount = detail.BillCreditMemoDetailModel.DocCreditMemoTaxAmount;
        //        billCMDetail.DocTotal = detail.BillCreditMemoDetailModel.DocToTal;
        //        billCMDetail.BaseCreditMemoAmount = detail.BillCreditMemoDetailModel.BaseCreditMemoAmount;
        //        billCMDetail.BaseCreditMemoTaxAmount = detail.BillCreditMemoDetailModel.BaseCreditMemoTaxAmount;
        //        billCMDetail.BaseTotal = detail.BillCreditMemoDetailModel.BaseToTal;
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCrediMemoDetailsDetails_UpdateCall_SuccessFully_Message);
        //    }

        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCrediMemoDetailsDetails_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}
        //private void UpdateBillCreditMemoGstDetail(BillCreditMemoGSTDetail billCreditMemoGstDetail,
        //    BillCreditMemoGSTDetail CMGstDetail)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCreditMemoGstDetail_UpdateCall_Request_Message);
        //        billCreditMemoGstDetail.Id = CMGstDetail.Id;
        //        billCreditMemoGstDetail.CreditMemoId = CMGstDetail.CreditMemoId;
        //        billCreditMemoGstDetail.TaxId = CMGstDetail.TaxId;
        //        billCreditMemoGstDetail.TaxCode = CMGstDetail.TaxCode;
        //        billCreditMemoGstDetail.Amount = CMGstDetail.Amount;
        //        billCreditMemoGstDetail.Total = CMGstDetail.Total;
        //        billCreditMemoGstDetail.TaxAmount = CMGstDetail.TaxAmount;
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCreditMemoGstDetail_UpdateCall_SuccessFully_Message);
        //    }

        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService,BillLoggingValidation.Log_UpdateBillCreditMemoGstDetail_UpdateCall_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //}

        #region Private Method Block
        private string GetAutoNumberByEntityType(long companyId, Payment lastInvoice, string docType, AppsWorld.BillModule.Entities.AutoNumber _autoNo, string docNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.PaymentModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewPaymentDocumentNo(companyId, docType, docNo);
                    //invDTO.IsEditable = true;
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
        private string GetAutoNumberForCreditMemo(long companyId, CreditMemo lastInvoice, string entityType, AppsWorld.BillModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.CreditMemoModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewBillDocumentNumber(companyId);
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
        private string GetAutoNumberByEntityType(long companyId, Bill lastInvoice, AppsWorld.BillModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            string output = null;
            //isEdit = false;
            //AppsWorld.BillModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
                    string number = "1";
                    if (lastInvoice != null)
                    {
                        //if (_autoNo.Format.Contains("{MM/YYYY}"))
                        //{
                        //    //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                        //    if (lastInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
                        //    {
                        //        //number = "1";
                        //        outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');

                        //    }
                        //    else
                        //    {
                        //        output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                        //        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //    }
                        //}
                        //else
                        //{
                        //    output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                        //    outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //}
                    }
                    else
                    {
                        output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //counter = Convert.ToInt32(value);
                    }
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
                        //if (_autoNo.Format.Contains("{MM/YYYY}"))
                        //{
                        //    //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                        //    if (lastInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
                        //    {
                        //        //number = "1";
                        //        outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');

                        //    }
                        //    else
                        //    {
                        //        output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                        //        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //    }
                        //}
                        //else
                        //{
                        //    output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                        //    outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //}
                    }
                    else
                    {
                        output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //counter = Convert.ToInt32(value);
                    }
                }
            }
            return outPutNumber;
        }
        private decimal GetExRateInformations(string docCurr, DateTime? Documentdate, long CompanyId, string baseCurr)
        {
            decimal exchangeRates = 0;
            try
            {
                ForexModel forex = new ForexModel();
                string date = Documentdate.Value.ToString("yyyy-MM-dd");
                forex.DocumentDate = date;
                forex.Provider = "Fixer";
                var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + docCurr + "&symbols=" + baseCurr;
                ExchangeRateModel currencyRates = DownloadSerializedJSONData<ExchangeRateModel>(url);
                exchangeRates = currencyRates.Rates.Select(c => c.Value).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return exchangeRates;
        }
        private static T DownloadSerializedJSONData<T>(string url) where T : new()
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
        #endregion

        #region HR_Claims_Syncing
        public bool? GetBillForClaims(Guid id, long? companyId)
        {
            return _billService.GetClaimsBill(id, companyId);
        }

        #endregion HR_Claims_Syncing





        public List<ScreenRecordsSave> saveScreenRecords1(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldEntityId, ref List<ScreenRecordsSave> lstscreenRecords, RecordStatusEnum status, DateTime? modifiedDate)
        {


            ScreenRecordsSave screenRecords = new ScreenRecordsSave();
            screenRecords.Id = recordId;
            screenRecords.ReferenceId = refrenceId;
            screenRecords.FeatureId = featureId;
            screenRecords.RecordId = recordId;
            screenRecords.recordName = recordName;
            screenRecords.UserName = userName;
            screenRecords.ModifiedDate = modifiedDate;
            screenRecords.CreatedDate = date.Value;
            screenRecords.isAdd = isAdd;
            screenRecords.CursorName = "Bean Cursor";
            screenRecords.ScreenName = screenName;
            screenRecords.CompanyId = comapnyid;
            screenRecords.OldFeatureId = oldEntityId;
            screenRecords.Status = status;
            lstscreenRecords.Add(screenRecords);
            return lstscreenRecords;



        }



        public bool saveAllScreenRecords(List<ScreenRecordsSave> lstscreenRecords)
        {
            var json = RestHelper.ConvertObjectToJason(lstscreenRecords);
            try
            {
                object obj = lstscreenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestHelper.ZPost(url, "api/document/migrationfolders", json);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<List<ScreenRecordsSave>>(response.Content);
                    lstscreenRecords = data;
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
        #region RestCall_Client_Changes

        //public void SaveBillRelatedFile(BillModel Tobject, string oldDocNo, Guid oldEntityId, bool isAdd, Bill _bill, string name)
        //{
        //    try
        //    {
        //        if (Tobject.DocSubType != DocTypeConstants.Payroll)
        //        {
        //            if (isAdd == true)
        //            {
        //                //if (Tobject.DocSubType == DocTypeConstants.Claim && Tobject.IsExternal == true)
        //                //{
        //                if (!isFileExist(Tobject.CompanyId, Tobject.EntityId.ToString(), Tobject.EntityId.ToString()))
        //                {

        //                    saveBillScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), name, Tobject.UserCreated, DateTime.UtcNow, true, Tobject.CompanyId, "Entities", Tobject.EntityId.ToString());
        //                    //string name=
        //                    //saveScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), beDTO.Name, beDTO.UserCreated, isFirst ? beDTO.CreatedDate.Value : beDTO.ModifiedDate, isFirst, beDTO.CompanyId, MasterModuleValidations.Entities, entityId.ToString());
        //                }
        //                //}
        //                //else
        //                saveBillScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, isAdd, Tobject.CompanyId, "Bills", Tobject.EntityId.ToString());
        //            }
        //            else
        //            {
        //                if (oldEntityId != Tobject.EntityId)
        //                {
        //                    if (!isFileExist(Tobject.CompanyId, Tobject.EntityId.ToString(), Tobject.EntityId.ToString()))
        //                    {
        //                        saveBillScreenRecords(Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), name, Tobject.UserCreated, DateTime.UtcNow, true, Tobject.CompanyId, "Entities", Tobject.EntityId.ToString());
        //                    }
        //                    //else
        //                    saveBillScreenRecords(Tobject.Id.ToString(), oldEntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, false, Tobject.CompanyId, oldDocNo, oldEntityId.ToString());
        //                }
        //                if (oldDocNo != Tobject.DocNo)
        //                    saveBillScreenRecords(Tobject.Id.ToString(), Tobject.EntityId.ToString(), Tobject.EntityId.ToString(), Tobject.DocNo, Tobject.UserCreated, DateTime.UtcNow, false, Tobject.CompanyId, oldDocNo, oldEntityId.ToString());
        //            }
        //            if (Tobject.DocSubType != DocTypeConstants.Claim)
        //            {
        //                if (Tobject.TileAttachments != null && Tobject.TileAttachments.Any())
        //                {
        //                    foreach (var fileAttachMent in Tobject.TileAttachments)
        //                    {
        //                        SaveBillScreenFiles(fileAttachMent.FileId, fileAttachMent.Name, fileAttachMent.FileSize, Tobject.EntityId.ToString(), _bill.Id.ToString(), _bill.Id.ToString(), Tobject.CompanyId, fileAttachMent.RecordStatus, fileAttachMent.Description, Tobject.UserCreated, Tobject.ModifiedBy, fileAttachMent.IsSystem);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Issues_in_Bill_Folder_creation);
        //    }
        //}


        private bool SaveBillScreenRecordFiles(string FileId, string fileName, string fileSize, string featureId, string recordId, string referenceId, long companyId, string recordStatus, string desc, string createdBy, string modifiedBy, bool isSystem)
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
            tilesFileVM.IsSystem = isSystem;
            tilesFileVM.ModuleName = "Bean Cursor";
            tilesFileVM.TabName = "Bills";
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
                var response = RestSharpHelper.ZPost(url, "api/document/savesscreenfiles", json);
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


        public bool saveBillScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldEntityId)
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
            screenRecords.OldFeatureId = oldEntityId;
            screenRecords.CreatedDate = date.Value;
            var json = RestHelper.ConvertObjectToJason(screenRecords);
            try
            {
                object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.ZPost(url, "api/document/savescreenfolders", json);
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
                var message = ex.Message;
                return false;
            }
        }

        public bool isFileExist(long companyId, string recordId, string featureId)
        {
            bool isExist = false;
            try
            {
                List<List<string, string>> lstParams = new List<List<string, string>>();
                lstParams.Add(new List<string, string>() { Name = "companyId", Value = companyId.ToString() });
                lstParams.Add(new List<string, string>() { Name = "featureId", Value = featureId.ToString() });
                //object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestHelper.RestGet(url, "api/document/isfolderexists", lstParams);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var data = JsonConvert.DeserializeObject<bool>(response.Content);
                    //screenRecords = data;
                    if (data != null)
                    {
                        if (data)
                            isExist = true;
                        else
                            isExist = false;
                    }
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
            return isExist;
        }
        public bool saveEntityScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldFeatureId)
        {
            ScreenRecordsSave screenRecords = new ScreenRecordsSave();
            screenRecords.ReferenceId = refrenceId;
            screenRecords.FeatureId = featureId;
            screenRecords.RecordId = recordId;
            screenRecords.recordName = recordName;
            screenRecords.UserName = userName;
            screenRecords.CreatedDate = date.Value;
            screenRecords.isAdd = isAdd;
            screenRecords.CursorName = "Bean Cursor";
            screenRecords.ScreenName = screenName;
            screenRecords.CompanyId = comapnyid;
            screenRecords.OldFeatureId = oldFeatureId;
            var json = RestHelper.ConvertObjectToJason(screenRecords);
            try
            {
                object obj = screenRecords;
                string url = ConfigurationManager.AppSettings["AdminUrl"];
                var response = RestSharpHelper.ZPost(url, "api/document/savescreenfolders", json);
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
                var message = ex.Message;
                return false;
            }
        }
        #endregion



        #region Save_HR_Payroll_Claims_Batch_Job_save
        public string SavePayrollandClaims(List<BillModel> lstBillModel, string connectionString)
        {
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_into_SaveBill_method);
            List<DocumentHistoryModel> lstDocHistoryModels = new List<DocumentHistoryModel>();
            //string _errors = CommonValidation.ValidateObject(Tobject);
            //string oldDocNo = null;
            //Guid oldEntityId = new Guid();
            //string eventStore = "";

            //if (!string.IsNullOrEmpty(_errors))
            //{
            //    throw new Exception(_errors);
            //}
            bool isNew = false;

            if (lstBillModel.Where(a => a.EntityId == null).Any())
            {
                throw new Exception(CommonConstant.Entity_is_mandatory);
            }

            string baseCurrency = _localizationService.GetLocalization(lstBillModel.Select(a => a.CompanyId).FirstOrDefault());
            if (lstBillModel.Select(a => a.DocSubType).FirstOrDefault() == DocTypeConstants.PayrollBill && (baseCurrency != lstBillModel.Select(a => a.BaseCurrency).FirstOrDefault()))
            {
                throw new Exception(BillConstant.Please_enter_the_valid_Base_Currency);
            }

            if (lstBillModel.Select(a => a.DocDate).FirstOrDefault() == null)
            {
                throw new Exception(CommonConstant.Invalid_Document_Date);
            }

            if (lstBillModel.Select(a => a.DueDate).FirstOrDefault() == null || lstBillModel.Select(a => a.DueDate).FirstOrDefault() < lstBillModel.Select(a => a.DocDate).FirstOrDefault())
            {
                throw new Exception(BillConstant.Invalid_Due_Date);
            }
            if (lstBillModel.Select(a => a.ServiceCompanyId == null).FirstOrDefault())
                throw new Exception(CommonConstant.ServiceCompany_is_mandatory);
            if (lstBillModel.Select(a => a.CreditTermId).FirstOrDefault() == null)
                throw new Exception(BillConstant.Terms_Payment_is_mandatory);
            //bool isNew = false;

            if (lstBillModel.Select(a => a.IsDocNoEditable).FirstOrDefault() == true)
                //if (IsDocumentNumberExists(DocTypeConstants.Bills, Tobject.DocNo, Tobject.Id, Tobject.CompanyId, Tobject.EntityId))
                //{
                //    throw new Exception(CommonConstant.Document_number_already_exist);
                //}

                if (lstBillModel.Select(a => a.BillDetailModels) == null || (lstBillModel.SelectMany(a => a.BillDetailModels)).Count() == 0)
                {
                    throw new Exception(BillConstant.Atleast_one_Sale_Item_is_required_in_the_Bill);
                }

            if (lstBillModel.Select(a => a.ExchangeRate).FirstOrDefault() == 0)
                throw new Exception(CommonConstant.ExchangeRate_Should_Be_Grater_Than_0);
            if (lstBillModel.Select(a => a.GstExchangeRate).FirstOrDefault() == 0 && lstBillModel.Select(a => a.IsGstSettings).FirstOrDefault() == true)
                throw new Exception(CommonConstant.GSTExchangeRate_Should_Be_Grater_Than_0);
            //if (lstBillModel.Select(a => a.DocSubType).FirstOrDefault() == DocTypeConstants.PayrollBill && lstBillModel.Select(a => a.GrandTotal).FirstOrDefault() == 0)
            //{

            //}
            //return new List<Bill>();
            #region Commented_Code
            //if (Tobject.DocSubType != DocTypeConstants.OpeningBalance && Tobject.IsFromPayroll != true)
            //{
            //    if (!_financialSettingService.ValidateFinancialOpenPeriod(Tobject.PostingDate, Tobject.CompanyId))
            //    {
            //        if (String.IsNullOrEmpty(Tobject.PeriodLockPassword))
            //        {
            //            throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
            //        }
            //        else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(Tobject.PostingDate, Tobject.PeriodLockPassword, Tobject.CompanyId))
            //        {
            //            throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
            //        }
            //    }
            //}
            //if (Tobject.IsFromPayroll != true && Tobject.BillDetailModels != null && Tobject.BillDetailModels.Count > 0)
            //{

            //    if (Tobject.BillDetailModels.Any(a => a.RecordStatus != "Deleted" && a.DocAmount == 0))
            //        throw new Exception(CommonConstant.DocCurrencyAmount_Should_Be_Grater_Than_0);
            //}
            #endregion

            //bool isAdd = false;
            long companyId = lstBillModel.Select(a => a.CompanyId).FirstOrDefault();
            string docSubType = lstBillModel.Select(a => a.DocSubType).FirstOrDefault();
            long subsidaryCompanyId = lstBillModel.Select(a => a.ServiceCompanyId).FirstOrDefault();
            string docCurrency = lstBillModel.Select(a => a.DocCurrency).FirstOrDefault();
            //baseCurrency= lstBillModel.Select(a => a.BaseCurrency).FirstOrDefault();
            DateTime? billDocDate = lstBillModel.Select(a => a.DocDate).FirstOrDefault();
            List<Guid> lstBillIds = lstBillModel.Select(a => a.Id).ToList();
            List<Bill> lstOfBills = _billService.GetListOfBills(lstBillIds, companyId, docSubType);

            string gstRepoCurrency = _gstSettingService.GetGSTByServiceCompany(subsidaryCompanyId);
            bool isGstActivate = _gstSettingService.IsGSTSettingActivateByServiceCompany(companyId, billDocDate.Value, subsidaryCompanyId);
            decimal? exchangeRate = null;
            decimal? gstExchangeRate = null;
            bool? IsMultiCurrencyActivate = _multiCurrencySettingService.GetByFlagCompanyId(companyId);
            if (docCurrency != baseCurrency)
            {
                //exchangeRate = GetExRateInformations(docCurrency, billDocDate, companyId, baseCurrency);
                //gstExchangeRate = GetExRateInformations(gstRepoCurrency, billDocDate, companyId, baseCurrency);
                exchangeRate = GetExRateInformation(docCurrency, baseCurrency, billDocDate.Value, companyId);
            }
            else
                exchangeRate = 1;
            if (docCurrency != gstRepoCurrency)
            {
                if (gstRepoCurrency == baseCurrency)
                    gstExchangeRate = exchangeRate;
                else
                    //gstExchangeRate = GetExRateInformations(gstRepoCurrency, billDocDate, companyId, baseCurrency);
                    gstExchangeRate = GetExRateInformation(docCurrency, gstRepoCurrency, billDocDate.Value, companyId);
            }
            else
                gstExchangeRate = 1;

            using (BillContext conn = new BillContext())
            {
                using (System.Data.Entity.DbContextTransaction tran = conn.Database.BeginTransaction())
                {
                    try
                    {
                        if (lstOfBills.Any())
                        {
                            isNew = false;
                            foreach (BillModel billModel in lstBillModel)
                            {
                                Bill _bill = lstOfBills.Where(a => a.Id == billModel.Id).FirstOrDefault();
                                if (_bill != null)
                                {
                                    //isAdd = false;
                                    decimal? original = 0;
                                    //oldEntityId = _bill.EntityId;
                                    if (_bill.DocumentState == "Partial Paid")
                                    {
                                        original = (_bill.GrandTotal) - (_bill.BalanceAmount);
                                        if (billModel.GrandTotal < original)
                                        {
                                            throw new Exception("Grand Total Should Be Grater Than Or Equal To Payment Amount");
                                        }
                                    }
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_into_if_condition_of_Journal_and_check_Journal_is_null_or_not);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Insert_Bill);
                                    if (_bill.GrandTotal < billModel.GrandTotal)
                                    {
                                        original = billModel.GrandTotal - _bill.GrandTotal;
                                        _bill.BalanceAmount += original;
                                    }
                                    else if (_bill.GrandTotal > billModel.GrandTotal)
                                    {
                                        original = _bill.GrandTotal - billModel.GrandTotal;
                                        _bill.BalanceAmount -= original;
                                    }
                                    //oldDocNo = _bill.DocNo;
                                    //InsertBill(billModel, _bill);
                                    FillPayrollModelToEntity(billModel, _bill, gstExchangeRate, exchangeRate, isGstActivate, gstRepoCurrency, IsMultiCurrencyActivate);
                                    _bill.DocNo = billModel.DocNo;
                                    _bill.SystemReferenceNumber = _bill.DocNo;
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Insert_Bill_Method);
                                    _bill.ModifiedBy = billModel.ModifiedBy;
                                    _bill.ModifiedDate = DateTime.UtcNow;
                                    _bill.ObjectState = ObjectState.Modified;
                                    _billService.Update(_bill);


                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Update_Bill_Details_Method);
                                    UpdateBillDetails(billModel, _bill);

                                    //lstOfBills.Add(_bill);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Update_Bill_Details);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Update_Bill_GST_Details);

                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_Form_Update_GST_Details);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Update_Bill_Credit_Memo);

                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Update_Bill_Credit_Memo);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Come Out From Save Bill");

                                    //_billService.InsertRange(lstOfBills);
                                }
                            }
                        }
                        else
                        {
                            foreach (BillModel billModel in lstBillModel)
                            {
                                //isAdd = true;
                                isNew = true;
                                Bill _bill = new Bill();
                                int? recOreder = 0;
                                //isNew = true;
                                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_Insert_Bill);
                                //InsertBill(billModel, _bill);
                                FillPayrollModelToEntity(billModel, _bill, gstExchangeRate, exchangeRate, isGstActivate, gstRepoCurrency, IsMultiCurrencyActivate);
                                _bill.BalanceAmount = billModel.GrandTotal;
                                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_Insert_Bill);
                                _bill.Id = billModel.Id;
                                _bill.DocumentState = "Not Paid";
                                //_bill.DocumentState = "Posted";
                                if (billModel.BillDetailModels.Count > 0 || billModel.BillDetailModels != null)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_If_Condition_And_Check_Tobject_BillDetail_Modules_Count_And_Bill_Details_Module);
                                    List<TaxCode> lstTaxCode = _taxCodeService.GetTaxAllCodesByIds(billModel.BillDetailModels.Select(c => c.TaxId).ToList());
                                    foreach (BillDetailModel detail in billModel.BillDetailModels)
                                    {
                                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Enter_Into_BillDetaulModel_Foreach);
                                        BillDetail billDetail = new BillDetail();
                                        // BillCreditMemoDetail BCMdetail = new BillCreditMemoDetail();
                                        //var coa = _chartOfAccountService.GetChartOfAccountById(detail.BeanCOA.COAId);
                                        //var taxi = _taxCodeService.GetTaxiId(detail.TaxId);
                                        billDetail.Id = Guid.NewGuid();
                                        billDetail.BillId = _bill.Id;
                                        //billDetail.Account = detail.COAName;
                                        billDetail.Description = detail.Description;
                                        billDetail.COAId = detail.COAId;
                                        billDetail.IsDisallow = detail.IsDisallow;
                                        billDetail.TaxId = detail.TaxId != null || detail.TaxId != 0 ? detail.TaxId : lstTaxCode.Where(a => a.Code == "NA").Select(a => a.Id).FirstOrDefault();
                                        billDetail.TaxIdCode = detail.TaxIdCode != null ? detail.TaxIdCode : "NA";
                                        billDetail.DocAmount = detail.DocAmount;
                                        billDetail.DocTaxAmount = detail.DocTaxAmount;
                                        billDetail.DocTotalAmount = billDetail.DocAmount + billDetail.DocTaxAmount;
                                        billDetail.BaseAmount = billModel.DocCurrency == billModel.BaseCurrency ? billDetail.DocAmount : Math.Round((decimal)billDetail.DocAmount * (decimal)_bill.ExchangeRate, 2);
                                        billDetail.BaseTaxAmount = billModel.DocCurrency == billModel.BaseCurrency ? billDetail.DocTaxAmount : Math.Round((decimal)billDetail.DocTaxAmount * (decimal)_bill.ExchangeRate, 2);
                                        billDetail.BaseTotalAmount = billModel.DocCurrency == billModel.BaseCurrency ? billDetail.DocTotalAmount : billDetail.BaseAmount + billDetail.BaseTaxAmount;
                                        billDetail.TaxCode = "NA";
                                        billDetail.TaxRate = detail.TaxRate;
                                        //TaxCode tax = lstTaxCode.Where(c => c.Id == detail.TaxId).FirstOrDefault();
                                        //if (tax != null)
                                        //    billDetail.TaxType = tax.TaxType;
                                        billDetail.IsPLAccount = detail.IsPLAccount;
                                        billDetail.RecOrder = _bill.DocSubType == DocTypeConstants.Claim ? detail.RecOrder : ++recOreder;
                                        billDetail.ObjectState = ObjectState.Added;
                                        _billDetailService.Insert(billDetail);
                                        _bill.BillDetails.Add(billDetail);
                                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Come_Out_From_BillDetailModule_Foereach);
                                    }
                                }
                                //_bill.BaseGrandTotal = Math.Round(_bill.BillDetails.Sum(a => (decimal)a.BaseTotalAmount), 2, MidpointRounding.AwayFromZero);
                                _bill.BaseGrandTotal = (_bill.GrandTotal != null && _bill.GrandTotal != 0) ? _bill.ExchangeRate != null ? Math.Round(_bill.GrandTotal * (decimal)_bill.ExchangeRate, 2, MidpointRounding.AwayFromZero) : Math.Round(_bill.GrandTotal, 2, MidpointRounding.AwayFromZero) : _bill.GrandTotal;
                                _bill.BaseBalanceAmount = _bill.BaseGrandTotal;
                                _bill.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                                _bill.UserCreated = billModel.UserCreated;
                                _bill.CreatedDate = DateTime.UtcNow;
                                _bill.ObjectState = ObjectState.Added;

                                //Company company = _companyService.GetById(billModel.CompanyId);
                                _bill.SystemReferenceNumber = billModel.IsDocNoEditable != true ? GenerateAutoNumberForType(billModel.CompanyId, DocTypeConstants.Bill, null, billModel.DocSubType) : billModel.DocNo;
                                _bill.DocNo = _bill.SystemReferenceNumber;
                                _billService.Insert(_bill);
                                lstOfBills.Add(_bill);
                                LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Come Out From Save Bill");
                                //_journalEntryService.PostInvoice(_bill);


                                #region Documentary History
                                try
                                {
                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_bill.Id, _bill.CompanyId, _bill.Id, _bill.DocType, _bill.DocSubType, _bill.DocumentState, _bill.DocCurrency, _bill.GrandTotal, _bill.BalanceAmount.Value, _bill.ExchangeRate.Value, _bill.ModifiedBy != null ? _bill.ModifiedBy : _bill.UserCreated, _bill.Remarks, _bill.PostingDate, _bill.GrandTotal, 0);
                                    if (lstdocumet.Any())
                                        //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                                        lstDocHistoryModels.AddRange(lstdocumet);
                                }
                                catch (Exception ex)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, CommonConstant.Failed_While_inserting_the_record_in_Document_History);
                                }
                                #endregion Documentary History
                            }
                        }
                        FillPayrollBillJournal(lstOfBills, isNew);

                        _unitOfWorkAsync.SaveChanges();
                        tran.Commit();
                        try
                        {
                            if (lstDocHistoryModels.Any())
                                AppaWorld.Bean.Common.SaveDocumentHistory(lstDocHistoryModels, connectionString);
                        }
                        catch (Exception)
                        {
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, CommonConstant.Failed_While_inserting_the_record_in_Document_History);
                        }

                        //throw new Exception();
                        //syncing 
                        Guid? payrollId = lstOfBills.Select(a => a.PayrollId).FirstOrDefault();
                        //string lstOfBillId = string.Join(",", lstOfBills.Select(a => a.Id).ToList());
                        try
                        {
                            SmartCursorSyncing(companyId, CursorConstants.HRCursor, DocTypeConstants.Payroll, payrollId, Guid.Empty, BillConstants.Completed, string.Empty, connectionString);
                        }
                        catch (Exception)
                        {
                            //throw;
                        }

                        return "Data Saved";
                    }
                    catch (Exception ex)
                    {
                        //tran.Dispose();
                        tran.Rollback();
                        Guid? payrollId = lstOfBills.Select(a => a.PayrollId).FirstOrDefault();
                        SmartCursorSyncing(companyId, CursorConstants.HRCursor, DocTypeConstants.PayrollBill, payrollId, Guid.Empty, BillConstants.Failed, ex.Message, connectionString);
                        //throw;
                        return "Failed";
                    }
                }
            }

        }
        //private  void Dispose(bool disposing)
        //{
        //    if (disposing && this. != null)
        //    {
        //        this._disposing = true;
        //        this.Rollback();
        //    }
        //}

        private void FillPayrollModelToEntity(BillModel TObject, Bill billNew, decimal? gstExchangeRate, decimal? exchangeRate, bool isGstActivate, string gstCurrency, bool? IsMultiCurrencyActivate)
        {
            try
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InsertBill_FillCall_Request_Message);
                billNew.DocType = DocTypeConstants.Bills;
                billNew.EntityType = "Vendor";
                billNew.ExchangeRate = exchangeRate != null ? exchangeRate : 1;
                billNew.GSTExchangeRate = gstExchangeRate != null ? gstExchangeRate : 1;
                billNew.IsGstSettings = isGstActivate;
                billNew.GSTExCurrency = gstCurrency != null ? gstCurrency : null;
                billNew.DocSubType = TObject.DocSubType;
                //billNew.IsMultiCurrency = TObject.DocCurrency != TObject.BaseCurrency ? true : false;
                billNew.IsMultiCurrency = IsMultiCurrencyActivate != null ? IsMultiCurrencyActivate.Value : false;
                billNew.DocumentDate = TObject.DocDate.Date;
                billNew.DueDate = TObject.DueDate;
                billNew.CompanyId = TObject.CompanyId;
                billNew.CreatedDate = TObject.CreatedDate;
                billNew.CreditTermsId = TObject.CreditTermId;
                billNew.EntityId = TObject.EntityId;
                billNew.DocDescription = TObject.DocDescription;
                billNew.ServiceCompanyId = TObject.ServiceCompanyId;
                billNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                if (TObject.IsGSTCurrencyRateChanged == true)
                    billNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged.Value;
                billNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                billNew.NoSupportingDocument = TObject.NoSupportingDocument;
                billNew.Nature = TObject.Nature;
                billNew.UserCreated = TObject.UserCreated;
                billNew.GrandTotal = TObject.GrandTotal;
                billNew.DocCurrency = TObject.DocCurrency;
                billNew.BaseCurrency = TObject.BaseCurrency;
                billNew.IsGSTApplied = TObject.IsGSTApplied;
                billNew.PostingDate = TObject.PostingDate;
                billNew.Status = TObject.Status;
                billNew.PayrollId = TObject.SyncPayrollId;
                billNew.PayrollId = TObject.PayrollId;
                billNew.IsExternal = true;
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InsertBill_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InsertBill_FillCall_Exception_Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }


        private void FillPayrollBillJournal(List<Bill> lstOfBills, bool isAdd)
        {
            if (isAdd == false)
            {
                List<Journal> lstOfJournal = _journalService.gstListOfJournal(lstOfBills.Select(a => a.CompanyId).FirstOrDefault(), lstOfBills.Select(a => a.Id).ToList());
                if (lstOfJournal.Any())
                {
                    foreach (Journal journals in lstOfJournal)
                    {
                        Journal journal = _journalService.GetJournal(journals.Id, journals.CompanyId);
                        List<JournalDetail> lstOfJDetails = _journalDetailService.getListOfJDetails(journal.Id);
                        if (lstOfJDetails.Any())
                        {
                            foreach (var jd in lstOfJDetails)
                            {
                                jd.ObjectState = ObjectState.Deleted;
                                _journalDetailService.Delete(jd);
                            }
                        }
                        journal.ObjectState = ObjectState.Deleted;
                        _journalService.Delete(journal);
                    }
                }
                foreach (Bill _bill in lstOfBills)
                {
                    JVModel jvMOdel = new JVModel();
                    FillJournal(jvMOdel, _bill, isAdd);
                    Journal journal = new Journal();
                    journal.Id = Guid.NewGuid();
                    journal.DocumentId = _bill.Id;
                    journal.DocDate = _bill.DocumentDate;
                    journal.DueDate = _bill.DueDate;
                    journal.PostingDate = _bill.PostingDate;
                    FillJournalMaster(journal, jvMOdel);
                }
            }
            else
            {
                foreach (Bill _bill in lstOfBills)
                {
                    JVModel jvMOdel = new JVModel();
                    FillJournal(jvMOdel, _bill, isAdd);
                    Journal journal = new Journal();
                    journal.Id = Guid.NewGuid();
                    journal.DocumentId = _bill.Id;
                    journal.DocDate = _bill.DocumentDate;
                    journal.DueDate = _bill.DueDate;
                    journal.PostingDate = _bill.PostingDate;
                    FillJournalMaster(journal, jvMOdel);
                }
            }
        }

        private void FillJournalMaster(Journal journal, JVModel journalModel)
        {
            journal.PostingDate = journalModel.PostingDate;
            //journal.COAId = journalModel.COAId;
            journal.CompanyId = journalModel.CompanyId;
            journal.Nature = journalModel.Nature;
            journal.CreditTermsId = journalModel.CreditTermsId;
            journal.CreatedDate = journalModel.CreatedDate;
            journal.CreationType = "System";
            journal.DocCurrency = journalModel.DocCurrency;
            //journal.DocDate = journalModel.DocDate.Date;
            journal.DocNo = journalModel.DocNo;
            journal.DocSubType = journalModel.DocSubType;
            journal.DocType = journalModel.DocType;
            journal.DocumentDescription = journalModel.Remarks;
            journal.DocumentState = journalModel.DocumentState;
            journal.DueDate = journalModel.DueDate;
            journal.EntityId = journalModel.EntityId;
            journal.EntityType = journalModel.EntityType;
            journal.ExchangeRate = journalModel.ExchangeRate;
            journal.GrandBaseCreditTotal = journalModel.GrandBaseCreditTotal;
            journal.GrandBaseDebitTotal = journalModel.GrandBaseDebitTotal;
            journal.GrandDocCreditTotal = journalModel.GrandDocCreditTotal;
            journal.GrandDocDebitTotal = journalModel.GrandDocDebitTotal;
            journal.GSTExchangeRate = journalModel.GSTExchangeRate;
            journal.GSTExCurrency = journalModel.GSTExCurrency;
            journal.GSTTotalAmount = journalModel.GSTTotalAmount;
            journal.BalanceAmount = journalModel.BalanceAmount;
            //journal.IsAllowableNonAllowable = journalModel.IsAllowableNonAllowable;
            journal.IsBaseCurrencyRateChanged = journalModel.IsBaseCurrencyRateChanged;
            journal.IsGSTApplied = journalModel.IsGSTApplied;
            journal.IsGSTCurrencyRateChanged = journalModel.IsGSTCurrencyRateChanged;
            journal.IsGstSettings = journalModel.IsGstSettings;
            journal.IsMultiCurrency = journalModel.IsMultiCurrency;
            //journal.IsNoSupportingDocs = journalModel.IsNoSupportingDocs;
            journal.IsNoSupportingDocument = journalModel.IsNoSupportingDocument;
            journal.IsSegmentReporting = journalModel.IsSegmentReporting;
            //journal.ModeOfReceipt = journalModel.ModeOfReceipt;
            journal.ModifiedBy = journalModel.ModifiedBy;
            journal.ModifiedDate = journalModel.ModifiedDate;
            journal.NoSupportingDocument = journalModel.NoSupportingDocument;
            journal.Remarks = journalModel.Remarks;
            journal.ServiceCompanyId = journalModel.ServiceCompanyId;
            journal.Status = journalModel.Status;
            //journal.IsShow = true;

            //journal.SystemReferenceNo = journalModel.SystemReferenceNo;
            //journal.ActualSysRefNo =  journalModel.SystemReferenceNo;
            journal.UserCreated = journalModel.UserCreated;
            //journal.Version = journalModel.Version;
            journal.ObjectState = ObjectState.Added;
            _journalService.Insert(journal);

            var basecredit = journalModel.JVVDetailModels.Sum(c => Math.Abs(c.BaseCredit == null ? 0 : Math.Abs(c.BaseCredit.Value)));
            var basedebit = journalModel.JVVDetailModels.Sum(c => Math.Abs(c.BaseDebit == null ? 0 : c.BaseDebit.Value));
            decimal diff = 0;
            if (basecredit != basedebit)
            {
                JVVDetailModel detail = new JVVDetailModel();
                if (basecredit > basedebit)
                {
                    diff = (basecredit - basedebit);
                    detail.BaseDebit = diff;

                }
                if (basecredit < basedebit)
                {
                    diff = (basedebit - basecredit);
                    detail.BaseCredit = diff;
                }
                if (diff <= Convert.ToDecimal(0.1))
                {
                    ChartOfAccount account1 = _chartOfAccountService.GetByName(/*"Rounding Account"*/COANameConstants.Rounding, journalModel.CompanyId); //? COANameConstants//);
                    if (account1 != null)
                    {
                        detail.AccountDescription = journalModel.DocumentDescription;//Cindi changes for Rounding account
                        detail.COAId = account1.Id;
                        detail.DocumentId = journal.DocumentId.Value;
                        detail.BaseCurrency = journalModel.ExCurrency != null ? journalModel.ExCurrency : journalModel.BaseCurrency;
                        detail.DocCurrency = journalModel.DocCurrency;
                        detail.IsTax = false;
                        detail.ServiceCompanyId = journal.ServiceCompanyId.Value;
                        detail.DocDate = journalModel.DocDate;
                        detail.PostingDate = journalModel.PostingDate;
                        detail.DocType = journalModel.DocType;
                        detail.EntityId = journalModel.EntityId;
                        detail.DocSubType = journalModel.DocSubType;
                        detail.DocNo = journalModel.DocNo;
                        detail.DocumentDetailId = Guid.NewGuid();
                        detail.RecOrder = (journalModel.JVVDetailModels.Count + 1);
                    }
                    journalModel.JVVDetailModels.Add(detail);
                }
            }
            foreach (JVVDetailModel detail in journalModel.JVVDetailModels.OrderBy(a => a.RecOrder))
            {
                JournalDetail JDetail = new JournalDetail();
                JDetail.Id = Guid.NewGuid();
                JDetail.JournalId = journal.Id;
                FillJournalDetail(JDetail, detail, journal);
            }
        }
        private void FillJournalDetail(JournalDetail JDetail, JVVDetailModel JvDetail, Journal journal)
        {
            JDetail.AccountDescription = JvDetail.AccountDescription;
            JDetail.AllowDisAllow = JvDetail.AllowDisAllow;
            JDetail.BaseCredit = JvDetail.BaseCredit == null ? null : JvDetail.BaseCredit < 0 ? Math.Abs(JvDetail.BaseCredit.Value) : JvDetail.BaseCredit;
            JDetail.BaseCreditTotal = JvDetail.BaseCreditTotal;
            JDetail.BaseDebit = JvDetail.BaseDebit == null ? null : JvDetail.BaseDebit < 0 ? Math.Abs(JvDetail.BaseDebit.Value) : JvDetail.BaseDebit;
            JDetail.BaseDebitTotal = JvDetail.BaseDebitTotal;
            JDetail.BaseTaxCredit = JvDetail.BaseTaxCredit;
            JDetail.BaseTaxDebit = JvDetail.BaseTaxDebit;
            JDetail.Nature = JvDetail.Nature;
            JDetail.DocCurrency = JvDetail.DocCurrency;
            JDetail.ServiceCompanyId = JvDetail.ServiceCompanyId;
            JDetail.DocNo = JvDetail.DocNo;
            JDetail.DocType = JvDetail.DocType;
            JDetail.DocSubType = JvDetail.DocSubType;
            JDetail.BaseCurrency = JvDetail.BaseCurrency;
            JDetail.ExchangeRate = JvDetail.ExchangeRate;
            JDetail.GSTExCurrency = JvDetail.GSTExCurrency;
            JDetail.GSTExchangeRate = JvDetail.GSTExchangeRate;
            JDetail.COAId = JvDetail.COAId;
            JDetail.DocumentDetailId = JvDetail.DocumentDetailId;
            JDetail.DocumentId = JvDetail.DocumentId;
            JDetail.DocCredit = JvDetail.DocCredit == null ? null : JvDetail.DocCredit < 0 ? Math.Abs(JvDetail.DocCredit.Value) : JvDetail.DocCredit;
            JDetail.DocCreditTotal = JvDetail.DocCreditTotal;
            JDetail.DocDebit = JvDetail.DocDebit == null ? null : JvDetail.DocDebit < 0 ? Math.Abs(JvDetail.DocDebit.Value) : JvDetail.DocDebit;
            JDetail.DocTaxableAmount = JvDetail.DocTaxableAmount;
            JDetail.DocTaxAmount = JvDetail.DocTaxAmount;
            JDetail.BaseTaxableAmount = JvDetail.DocTaxableAmount;
            JDetail.BaseTaxAmount = JvDetail.BaseTaxAmount;
            JDetail.DocDebitTotal = JvDetail.DocDebitTotal;
            JDetail.DocTaxCredit = JvDetail.DocTaxCredit;
            JDetail.DocTaxDebit = JvDetail.DocTaxDebit;
            JDetail.DocTaxCredit = JvDetail.DocTaxCredit;
            JDetail.DocTaxDebit = JvDetail.DocTaxDebit;
            JDetail.TaxId = JvDetail.TaxId;
            JDetail.TaxRate = JvDetail.TaxRate;
            JDetail.IsTax = JvDetail.IsTax;
            JDetail.EntityId = JvDetail.EntityId;
            // JDetail.SystemRefNo = JvDetail.SystemRefNo;
            JDetail.CreditTermsId = JvDetail.CreditTermsId;
            JDetail.DueDate = journal.DueDate;
            JDetail.NoSupportingDocs = journal.NoSupportingDocument;
            JDetail.BaseAmount = JvDetail.BaseAmount;
            JDetail.DocDate = JvDetail.DocDate;
            JDetail.PostingDate = JvDetail.PostingDate;
            JDetail.RecOrder = JvDetail.RecOrder;
            JDetail.AmountDue = JvDetail.AmountDue;
            JDetail.ObjectState = ObjectState.Added;
            _journalDetailService.Insert(JDetail);

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
                LoggingHelper.LogError(BillConstants.BillApplicationService, ex, ex.Message);
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



        #endregion
















































        #region Peppol
        public string BillCreation(InvBindingModel invBindingModel, bool? IsMultiCurrencyActivate, Company subCompany, BeanEntity beanEntity, List<CacInvoiceLine> lstInvoiceLineItems, FinancialSetting financSettings, ChartOfAccount chartOfAccount, long? termsOfPaymentId, CacLegalMonetaryTotal cacLegalMonetaryTotal)
        {
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "BillCreation Request message");
            //var gstSetting = _gstSettingService.GetByCompanyId(subCompany.Id);
            bool IsNoSupportingDocument = false;
            string str = "Select cf.IsChecked from common.Feature as f join Common.CompanyFeatures  as cf on f.Id=cf.FeatureId where f.Name='No Supporting Documents' and cf.CompanyId=" + subCompany.ParentId;
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

                        var SupportingDocumentChecked = dr["IsChecked"].ToString();
                        if (!String.IsNullOrWhiteSpace(SupportingDocumentChecked) && (SupportingDocumentChecked == "True" || SupportingDocumentChecked == "1"))
                            IsNoSupportingDocument = true;
                        else
                            IsNoSupportingDocument = false;
                    }

                }
                dr.NextResult();
            }
            con.Close();
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "No Supporting Documents is exists or not checked throught Ado.net suceess");
            BillModel billModel = new BillModel();

            billModel.Id = Guid.NewGuid();
            billModel.PeppolDocumentId = invBindingModel.PeppolDocumentId;
            billModel.Nature = "Trade";
            billModel.DocNo = invBindingModel.DocNo;
            billModel.DocDate = !string.IsNullOrEmpty(invBindingModel.InvDate) ? Convert.ToDateTime(invBindingModel.InvDate) : DateTime.UtcNow;
            billModel.PostingDate = !string.IsNullOrEmpty(invBindingModel.InvDate) ? Convert.ToDateTime(invBindingModel.InvDate) : DateTime.UtcNow;
            billModel.DueDate = !string.IsNullOrEmpty(invBindingModel.DueDate) ? Convert.ToDateTime(invBindingModel.DueDate) : DateTime.UtcNow;
            billModel.DocSubType = "General";
            billModel.DocType = "Bill";
            billModel.BaseCurrency = financSettings.BaseCurrency != null ? financSettings.BaseCurrency : "SGD";
            billModel.DocCurrency = invBindingModel.DocCurrency;
            billModel.DocumentState = "Not Paid";
            billModel.IsDocNoEditable = false;
            billModel.IsExternal = true;
            billModel.ExchangeRate = 1;
            billModel.DocDescription = invBindingModel.Remarks;
            if (billModel.DocCurrency.ToLower() == billModel.BaseCurrency.ToLower())
                billModel.ExchangeRate = 1;

            //else
            //{
            //    AppsWorld.MasterModule.Models.BeanForex forex = new AppsWorld.MasterModule.Models.BeanForex();
            //    var url = "https://data.fixer.io/api/" + billModel.DocDate + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + billModel.DocCurrency + "&symbols=" + billModel.BaseCurrency;
            //    AppsWorld.CommonModule.Models.CurrencyModel currencyRates = _download_serialized_json_data<AppsWorld.CommonModule.Models.CurrencyModel>(url);
            //    if (currencyRates.Base == null)
            //    {
            //        AppsWorld.MasterModule.Models.BeanForex forex1 = new AppsWorld.MasterModule.Models.BeanForex();
            //        return forex1;
            //    }

            //    forex.BaseUnitPerUSD = currencyRates.Rates.Where(c => c.Key == billModel.BaseCurrency).Select(c => c.Value).FirstOrDefault();
            //    FillCommonForexFrom(billModel.DocCurrency, billModel.DocDate, forex, billModel.BaseCurrency, true);
            //    billModel.ExchangeRate = forex.BaseUnitPerUSD;
            //}


            billModel.ISMultiCurrency = IsMultiCurrencyActivate != null ? IsMultiCurrencyActivate.Value : false;
            billModel.IsNoSupportingDocument = IsNoSupportingDocument;
            billModel.IsGstSettings = subCompany.IsGstSetting != null ? subCompany.IsGstSetting.Value : false;
            billModel.GstReportingCurrency = "SGD";
            billModel.GstExchangeRate = 1;
            billModel.CompanyId = subCompany.ParentId != null ? subCompany.ParentId.Value : 0;
            billModel.ServiceCompanyId = subCompany.Id;
            billModel.EntityId = beanEntity.Id;
            billModel.EntityName = beanEntity.Name;
            billModel.CreditTermId = termsOfPaymentId;
            billModel.IsFromPeppol = true;
            billModel.UserCreated = "System";
            billModel.CreatedDate = DateTime.UtcNow;
            billModel.Status = RecordStatusEnum.Active;

            List<BillDetailModel> lstbillDetailModels = new List<BillDetailModel>();

            foreach (var invLineItem in lstInvoiceLineItems)
            {
                BillDetailModel billDetailModel = new BillDetailModel();
                billDetailModel.BillId = billModel.Id;
                billDetailModel.Id = Guid.NewGuid();
                billDetailModel.COAId = chartOfAccount != null ? chartOfAccount.Id : 0;
                billDetailModel.COAName = chartOfAccount != null ? chartOfAccount.Name : null;
                billDetailModel.RecordStatus = "Added";
                if (invLineItem.CacItem != null)
                {
                    if (invLineItem.CacItem.CacClassifiedTaxCategory != null)
                    {
                        if (!string.IsNullOrEmpty(invLineItem.CacItem.CacClassifiedTaxCategory.CbcPercent))

                            billDetailModel.TaxRate = Convert.ToDouble(invLineItem.CacItem.CacClassifiedTaxCategory.CbcPercent);
                        else
                            billDetailModel.TaxRate = 0;

                        if (!string.IsNullOrEmpty(invLineItem.CacItem.CacClassifiedTaxCategory.CbcID))
                        {
                            billDetailModel.TaxCode = invLineItem.CacItem.CacClassifiedTaxCategory.CbcID;
                            var taxcodeEntity = _taxCodeService.GetTaxByCode(billDetailModel.TaxCode);
                            if (taxcodeEntity != null)
                            {
                                billDetailModel.TaxId = taxcodeEntity.Id;
                                billDetailModel.TaxName = taxcodeEntity.Name;
                                billDetailModel.TaxIdCode = taxcodeEntity.Code + "-" + (taxcodeEntity.TaxRate != null ? taxcodeEntity.TaxRate + "%" : "NA");
                            }
                            else
                            {
                                var NAtaxcode = _taxCodeService.GetTaxByCode("NA");
                                if (NAtaxcode != null)
                                {
                                    billDetailModel.TaxId = NAtaxcode.Id;
                                    billDetailModel.TaxName = NAtaxcode.Name;
                                    billDetailModel.TaxIdCode = NAtaxcode.Name;
                                    billDetailModel.TaxRate = null;
                                }
                            }
                        }

                    }

                }

                //if(invLineItem.CacAllowanceCharge!= null)
                //{
                //    if (invLineItem.CacAllowanceCharge.CbcBaseAmount != null)
                //    {
                //        if(!string.IsNullOrEmpty(invLineItem.CacAllowanceCharge.CbcBaseAmount.Text))
                //        {
                //            var docuAmunt = Convert.ToDecimal(invLineItem.CacAllowanceCharge.CbcBaseAmount.Text);
                //            billDetailModel.DocAmount = Math.Round(docuAmunt,2);
                //        }

                //    }
                //}

                //if (cacLegalMonetaryTotal != null)
                //{
                //    var lineExtAmt = cacLegalMonetaryTotal.CbcLineExtensionAmount.Text;
                //   if(lstInvoiceLineItems.Count > 1)
                //    {
                //        // var lstExtAmts= lstInvoiceLineItems.Select(x => x.CbcLineExtensionAmount.Text).ToList();
                //        var invLineExtAmt = invLineItem.CbcLineExtensionAmount.Text;
                //        var docuAmunt = Convert.ToDecimal(invLineExtAmt);
                //        billDetailModel.DocAmount = Math.Round(docuAmunt, 2);
                //    }
                //   else
                //    {
                //        var docuAmunt = Convert.ToDecimal(lineExtAmt);
                //        billDetailModel.DocAmount = Math.Round(docuAmunt, 2);
                //    }
                //}
                if (invLineItem.CacAllowanceCharge != null)
                {
                    if (invLineItem.CacAllowanceCharge.CbcChargeIndicator != null)
                    {
                        //if(invLineItem.CacAllowanceCharge.CbcChargeIndicator.ToLower() == "false".ToLower())
                        //{
                        if (invLineItem.CacAllowanceCharge.CbcMultiplierFactorNumeric != null)
                        {
                            var discount = Convert.ToDecimal(invLineItem.CacAllowanceCharge.CbcMultiplierFactorNumeric);
                            if (invLineItem.CacPrice != null)
                            {
                                if (invLineItem.CacPrice.CbcPriceAmount != null)
                                {
                                    var invQuantity = !string.IsNullOrEmpty(invLineItem.CbcInvoicedQuantity.Text) ? Convert.ToDecimal(invLineItem.CbcInvoicedQuantity.Text) : 0;
                                    var priceAmunt = !string.IsNullOrEmpty(invLineItem.CacPrice.CbcPriceAmount.Text) ? Convert.ToDecimal(invLineItem.CacPrice.CbcPriceAmount.Text) : 0;
                                    var docAmountWithOutDiscount = priceAmunt * invQuantity;
                                    var discountAmount = (docAmountWithOutDiscount * discount) / 100;
                                    var docAmount = docAmountWithOutDiscount - discountAmount;
                                    billDetailModel.DocAmount = Math.Round(docAmount, 2);
                                }
                            }
                        }
                        //}
                    }
                }
                else
                {
                    if (invLineItem.CacPrice != null)
                    {
                        if (invLineItem.CacPrice.CbcPriceAmount != null)
                        {
                            var invQuantity = !string.IsNullOrEmpty(invLineItem.CbcInvoicedQuantity.Text) ? Convert.ToDecimal(invLineItem.CbcInvoicedQuantity.Text) : 0;
                            var priceAmunt = !string.IsNullOrEmpty(invLineItem.CacPrice.CbcPriceAmount.Text) ? Convert.ToDecimal(invLineItem.CacPrice.CbcPriceAmount.Text) : 0;
                            var docAmount = priceAmunt * invQuantity;
                            billDetailModel.DocAmount = Math.Round(docAmount, 2);
                        }
                    }
                }

                var taxrate = Convert.ToDecimal(billDetailModel.TaxRate) / 100;
                billDetailModel.DocTaxAmount = Math.Round(billDetailModel.DocAmount * taxrate, 2);
                billDetailModel.DocTotalAmount = billDetailModel.DocAmount + billDetailModel.DocTaxAmount;
                if (billModel.ExchangeRate == 1)
                {
                    billDetailModel.BaseAmount = billDetailModel.DocAmount;
                    billDetailModel.BaseTaxAmount = billDetailModel.DocTaxAmount;
                    billDetailModel.BaseTotalAmount = billDetailModel.DocTotalAmount;
                }
                else
                {
                    billDetailModel.BaseAmount = Math.Round(billDetailModel.DocAmount * (decimal)billModel.ExchangeRate, 2);
                    billDetailModel.BaseTaxAmount = Math.Round(billDetailModel.DocTaxAmount * (decimal)billModel.ExchangeRate, 2);
                    billDetailModel.BaseTotalAmount = billDetailModel.BaseAmount + billDetailModel.BaseTaxAmount;
                }
                billDetailModel.IsTaxCodeNotEditable = false;
                billDetailModel.IsTaxAmountEditable = (billDetailModel.TaxRate == 0 || billDetailModel.TaxRate == null) ? false : true;
                billDetailModel.IsTaxAmountEditables = billDetailModel.IsTaxAmountEditable;
                lstbillDetailModels.Add(billDetailModel);
                billModel.BillDetailModels = lstbillDetailModels;
                billModel.GrandTotal = Math.Round(lstbillDetailModels.Select(x => x.DocTotalAmount).Sum(), 2);

            }

            if (billModel != null)
            {
                var json = RestSharpHelper.ConvertObjectToJason(billModel);
                try
                {
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "calling savebillduplicate rest call ");
                    string url = ConfigurationManager.AppSettings["BeanUrl"];
                    //string url = "http://localhost:57584/";
                    var response = RestSharpHelper.Post(url, "api/bill/savebillduplicate", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "calling savebillduplicate rest call Success");
                    }
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "savebillduplicate rest call Failed");
                }
            }
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "BillCreation method executed Success message");
            return "";
        }


        public InBoundFilesModel InBoundAllInvoice(InBoundFilesModel model)
        {

            // string apiKey = ConfigurationManager.AppSettings["PeppolApiKey"].ToString();
            // var client = new RestClient("https://api.ap-connect.dev.einvoice.sg/v1/invoice/inbound?api_key=" + apiKey);
            // client.Timeout = -1;
            // var request = new RestRequest(Method.GET);
            //// request.AddParameter("text/plain", "<file contents here>", ParameterType.RequestBody);
            // IRestResponse response = client.Execute(request);
            // var getRespModel = JsonConvert.DeserializeObject<List<InBoundFilesModel>>(response.Content);


            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_InBoundAllInvoiceMethod_Request_Message);
            PeppolInboundInvoice peppolInboundInvoice = new PeppolInboundInvoice();

            //WebClient client1 = new WebClient();
            //byte[] dataBytes = client1.DownloadData(new Uri(model.invoiceFileUrl));
            //string encodedFileAsBase64 = Convert.ToBase64String(dataBytes);
            //var jsonXmlData1 = DecodeStringFromBase64(encodedFileAsBase64);
            //string fileName = "PeppolOutBoundInvoice";//"SampleWithSBDHFile";//
            //string filePath = @"C:\temp\" + fileName + ".xml";
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_XMLDocument_Start_Message);
            XmlDocument doc = new XmlDocument();
            if (!string.IsNullOrEmpty(model.invoiceFileUrl))
                doc.Load(model.invoiceFileUrl);
            else
                throw new Exception("Invoice FileUrl not getting.");
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "load URL from to XmlDocument Success.");
            XmlNodeList xmlNL = doc.GetElementsByTagName("cac:InvoiceLine");
            var lineItemsCount = xmlNL.Count;
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "InvoiceLine count in XML Document");
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_XMLDOCSerilization);

            peppolInboundInvoice.Id = Guid.NewGuid();
            peppolInboundInvoice.DocId = model.docId;
            peppolInboundInvoice.XmlFilepath = model.invoiceFileUrl;
            peppolInboundInvoice.XMLFileData = jsonText;
            peppolInboundInvoice.CreatedDate = DateTime.UtcNow;
            peppolInboundInvoice.UserCreated = "System";
            peppolInboundInvoice.Status = RecordStatusEnum.Active;
            _companyService.InsertIntoPeppolInboundInvoice(peppolInboundInvoice);
            _unitOfWorkAsync.SaveChanges();

            var inboundData = _companyService.GetInboundData(peppolInboundInvoice.Id);
            CacLegalMonetaryTotal cacLegalMonetaryTotal = new CacLegalMonetaryTotal();


            InvBindingModel invBindingModel = new InvBindingModel();
            List<CacInvoiceLine> lstInvoiceLineItems = new List<CacInvoiceLine>();
            CommunicationBindingModel communication = new CommunicationBindingModel();

            try
            {

                if (lineItemsCount > 1)
                {
                    var multinvModel = JsonConvert.DeserializeObject<MultiInBoundInvoiceModel>(jsonText);
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_XMLDOC_DeSerilization_MultipleLineItems);
                    if (multinvModel != null)
                    {
                        if (multinvModel.StandardBusinessDocument != null)
                        {
                            if (multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader != null)
                            {
                                if (multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender != null)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Sender PeppolId");
                                    var senderId = multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender.Identifier.Text;
                                    var senderPPLId = senderId.Remove(0, 5);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Remove singaporecode in  sender PeppolId");
                                    //peppolInboundInvoice.SenderPeppolId = senderPPLId;
                                    if (inboundData != null)
                                        inboundData.SenderPeppolId = senderPPLId;


                                }
                                if (multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver != null)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Reciever PeppolId");
                                    var reciverID =
                                multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver.Identifier.Text;
                                    var reciverPPLID = reciverID.Remove(0, 5);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Remove singaporecode in  Reciever PeppolId");
                                    //peppolInboundInvoice.ReciverPeppolId = reciverPPLID;
                                    if (inboundData != null)
                                        inboundData.ReciverPeppolId = reciverPPLID;

                                }
                            }
                        }
                        //peppolInboundInvoice.DocId = model.docId;
                        //peppolInboundInvoice.XmlFilepath = model.invoiceFileUrl;
                        //peppolInboundInvoice.XMLFileData = jsonText;
                        //peppolInboundInvoice.CreatedDate = DateTime.UtcNow;
                        //peppolInboundInvoice.UserCreated = "System";
                        //peppolInboundInvoice.Status = RecordStatusEnum.Active;
                        //_companyService.InsertIntoPeppolInboundInvoice(peppolInboundInvoice);

                        _companyService.UpdatetoPeppolInboundInvoice(inboundData);
                        _unitOfWorkAsync.SaveChanges();

                        if (multinvModel.StandardBusinessDocument != null)
                        {
                            if (multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader != null)
                            {
                                if (multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender != null)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Sender PeppolId");
                                    var senderId = multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender.Identifier.Text;
                                    var senderPPLId = senderId.Remove(0, 5);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Remove singaporecode in  sender PeppolId");
                                    invBindingModel.SenderPeppolId = senderPPLId;
                                    invBindingModel.PeppolDocumentId = model.docId;
                                }
                                if (multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver != null)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Reciever PeppolId");
                                    var reciverID = multinvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver.Identifier.Text;
                                    var reciverPPLID = reciverID.Remove(0, 5);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Remove singaporecode in  Reciever PeppolId");
                                    invBindingModel.ReciverPeppolId = reciverPPLID;
                                }
                            }

                            if (multinvModel.StandardBusinessDocument.Invoice != null)
                            {
                                invBindingModel.InvDate = multinvModel.StandardBusinessDocument.Invoice.CbcIssueDate;
                                invBindingModel.DueDate = multinvModel.StandardBusinessDocument.Invoice.CbcDueDate;
                                invBindingModel.DocCurrency = multinvModel.StandardBusinessDocument.Invoice.CbcDocumentCurrencyCode;
                                invBindingModel.Remarks = multinvModel.StandardBusinessDocument.Invoice.CbcNote;
                                //invBindingModel.TermsOfPayment = peppolModel.StandardBusinessDocument.Invoice.CacPaymentTerms != null ? peppolModel.StandardBusinessDocument.Invoice.CacPaymentTerms.CbcNote : null;

                                var diff = Convert.ToDateTime(invBindingModel.DueDate).Subtract(Convert.ToDateTime(invBindingModel.InvDate));
                                invBindingModel.TermsOfPayment = Convert.ToInt32(diff.TotalDays);

                                if (multinvModel.StandardBusinessDocument.Invoice.CacBillingReference != null)
                                {
                                    if (multinvModel.StandardBusinessDocument.Invoice.CacBillingReference.CacInvoiceDocumentReference != null)
                                    {
                                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "get InvoiceDocumentReference");
                                        invBindingModel.DocNo = multinvModel.StandardBusinessDocument.Invoice.CacBillingReference.CacInvoiceDocumentReference.CbcID;
                                    }

                                }

                                if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty != null)
                                {
                                    if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty != null)
                                    {
                                        if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacPartyName
                                            != null)
                                        {
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Service EntityName");
                                            invBindingModel.ServiceEntityName = multinvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacPartyName.CbcName;
                                        }
                                    }
                                }

                                if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty != null)
                                {
                                    if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty != null)
                                    {
                                        if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacPartyName
                                            != null)
                                        {
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get EntityName");
                                            invBindingModel.EntityName = multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacPartyName.CbcName;
                                        }

                                        if (multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacContact != null)
                                        {
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Contact Name");
                                            invBindingModel.ContactName = multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacContact.CbcName;
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get PhoneNO");
                                            invBindingModel.PhoneNo = multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacContact.CbcTelephone;
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Email");
                                            invBindingModel.Email = multinvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacContact.CbcElectronicMail;

                                            if (!string.IsNullOrEmpty(invBindingModel.Email))
                                            {
                                                communication.Key = "Email";
                                                communication.Value = invBindingModel.Email;
                                            }
                                            else if (!string.IsNullOrEmpty(invBindingModel.PhoneNo))
                                            {
                                                communication.Key = "Phone";
                                                communication.Value = invBindingModel.PhoneNo;
                                            }
                                        }
                                    }
                                }

                                lstInvoiceLineItems = multinvModel.StandardBusinessDocument.Invoice.CacInvoiceLine.ToList();
                                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_MultipleLineItems_ModelBinding_Sucessful);
                            }



                        }
                        if (multinvModel.StandardBusinessDocument.Invoice.CacLegalMonetaryTotal != null)
                        {
                            cacLegalMonetaryTotal = multinvModel.StandardBusinessDocument.Invoice.CacLegalMonetaryTotal;
                        }
                    }

                }
                else
                {
                    var singleInvModel = JsonConvert.DeserializeObject<InBoundInvoiceModel>(jsonText);
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_XMLDOC_DeSerilization_SingleLineItems);
                    if (singleInvModel != null)
                    {
                        if (singleInvModel.StandardBusinessDocument != null)
                        {
                            if (singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader != null)
                            {
                                if (singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender != null)
                                {

                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Sender PeppolId"); var senderId = singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender.Identifier.Text;
                                    var senderPPLId = senderId.Remove(0, 5);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Remove singaporecode in  sender PeppolId");
                                    // peppolInboundInvoice.SenderPeppolId = senderPPLId;
                                    if (inboundData != null)
                                        inboundData.SenderPeppolId = senderPPLId;

                                }
                                if (singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver != null)
                                {
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Reciver PeppolId");
                                    var reciverID =
                                singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver.Identifier.Text;
                                    var reciverPPLID = reciverID.Remove(0, 5);
                                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Remove singaporecode in  Reciver PeppolId");
                                    // peppolInboundInvoice.ReciverPeppolId = reciverPPLID;
                                    if (inboundData != null)
                                        inboundData.ReciverPeppolId = reciverPPLID;
                                }
                            }
                        }


                        _companyService.UpdatetoPeppolInboundInvoice(inboundData);
                        _unitOfWorkAsync.SaveChanges();

                        if (singleInvModel.StandardBusinessDocument != null)
                        {
                            if (singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader != null)
                            {
                                if (singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender != null)
                                {
                                    var senderId = singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Sender.Identifier.Text;
                                    var senderPPLId = senderId.Remove(0, 5);
                                    invBindingModel.SenderPeppolId = senderPPLId;
                                    invBindingModel.PeppolDocumentId = model.docId;
                                }
                                if (singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver != null)
                                {
                                    var reciverID = singleInvModel.StandardBusinessDocument.StandardBusinessDocumentHeader.Receiver.Identifier.Text;
                                    var reciverPPLID = reciverID.Remove(0, 5);
                                    invBindingModel.ReciverPeppolId = reciverPPLID;
                                }
                            }

                            if (singleInvModel.StandardBusinessDocument.Invoice != null)
                            {
                                invBindingModel.InvDate = singleInvModel.StandardBusinessDocument.Invoice.CbcIssueDate;
                                invBindingModel.DueDate = singleInvModel.StandardBusinessDocument.Invoice.CbcDueDate;
                                invBindingModel.DocCurrency = singleInvModel.StandardBusinessDocument.Invoice.CbcDocumentCurrencyCode;
                                invBindingModel.Remarks = singleInvModel.StandardBusinessDocument.Invoice.CbcNote;
                                //invBindingModel.TermsOfPayment = peppolModel.StandardBusinessDocument.Invoice.CacPaymentTerms != null ? peppolModel.StandardBusinessDocument.Invoice.CacPaymentTerms.CbcNote : null;

                                var diff = Convert.ToDateTime(invBindingModel.DueDate).Subtract(Convert.ToDateTime(invBindingModel.InvDate));
                                invBindingModel.TermsOfPayment = Convert.ToInt32(diff.TotalDays);

                                if (singleInvModel.StandardBusinessDocument.Invoice.CacBillingReference != null)
                                {
                                    if (singleInvModel.StandardBusinessDocument.Invoice.CacBillingReference.CacInvoiceDocumentReference != null)
                                    {
                                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get DocumentRef");
                                        invBindingModel.DocNo = singleInvModel.StandardBusinessDocument.Invoice.CacBillingReference.CacInvoiceDocumentReference.CbcID;
                                    }

                                }

                                if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty != null)
                                {
                                    if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty != null)
                                    {
                                        if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacPartyName
                                            != null)
                                        {
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get ServiceEntityName");
                                            invBindingModel.EntityName = singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacPartyName.CbcName;
                                        }

                                        if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacContact != null)
                                        {
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Contact Name");
                                            invBindingModel.ContactName = singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacContact.CbcName;
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get PhoneNo");
                                            invBindingModel.PhoneNo = singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacContact.CbcTelephone;
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Email");
                                            invBindingModel.Email = singleInvModel.StandardBusinessDocument.Invoice.CacAccountingSupplierParty.CacParty.CacContact.CbcElectronicMail;

                                            if (!string.IsNullOrEmpty(invBindingModel.Email))
                                            {
                                                communication.Key = "Email";
                                                communication.Value = invBindingModel.Email;
                                            }
                                            else if (!string.IsNullOrEmpty(invBindingModel.PhoneNo))
                                            {
                                                communication.Key = "Phone";
                                                communication.Value = invBindingModel.PhoneNo;
                                            }
                                        }
                                    }
                                }

                                if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty != null)
                                {
                                    if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty != null)
                                    {
                                        if (singleInvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacPartyName
                                            != null)
                                        {
                                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Get Entity name");

                                            invBindingModel.ServiceEntityName = singleInvModel.StandardBusinessDocument.Invoice.CacAccountingCustomerParty.CacParty.CacPartyName.CbcName;
                                        }
                                    }
                                }


                                var cacInvoiceLine = singleInvModel.StandardBusinessDocument.Invoice.CacInvoiceLine;
                                if (cacInvoiceLine != null)
                                    lstInvoiceLineItems.Add(cacInvoiceLine);
                                LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_SingleLineItems_ModelBinding_Sucessful);

                            }

                            if (singleInvModel.StandardBusinessDocument.Invoice.CacLegalMonetaryTotal != null)
                            {
                                cacLegalMonetaryTotal = singleInvModel.StandardBusinessDocument.Invoice.CacLegalMonetaryTotal;
                            }
                        }

                    }



                }


                LoggingHelper.LogMessage(BillConstants.BillApplicationService, "save changes Sucessful.");
                if (!string.IsNullOrEmpty(jsonText))
                {
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_ConvertInBoundInvoiceToBill_Request_Message);
                    ConvertInBoundInvoiceToBill(invBindingModel, lstInvoiceLineItems, communication, cacLegalMonetaryTotal);
                    LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_ConvertInBoundInvoiceToBill_Request_Sucessful);
                }
            }
            catch (Exception e)
            {
                LoggingHelper.LogError(BillConstants.BillApplicationService, e, e.Message);
                inboundData.ErrorMessage = e.Message;
                _companyService.UpdatetoPeppolInboundInvoice(inboundData);
                _unitOfWorkAsync.SaveChanges();
            }


            return model;
        }
        public string DecodeStringFromBase64(string stringToDecode)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(stringToDecode));
        }

        public string ConvertInBoundInvoiceToBill(InvBindingModel invBindingModel, List<CacInvoiceLine> lstInvoiceLineItems, CommunicationBindingModel communication, CacLegalMonetaryTotal cacLegalMonetaryTotal)
        {

            //XmlDocument doc1 = new XmlDocument();
            //doc1.Load(@"c:\temp\SampleWithSBDHFile1.xml");
            //string xml = doc1.InnerXml;
            //doc1.LoadXml(xml);
            //string jsonText = JsonConvert.SerializeXmlNode(doc1, Formatting.Indented, false);

            LoggingHelper.LogMessage(BillConstants.BillApplicationService, BillLoggingValidation.Log_ConvertInBoundInvoiceToBill_Request_Message);
            var subCompanies = _companyService.GetCompanyByName(invBindingModel.ReciverPeppolId);
            var subCompany = subCompanies.FirstOrDefault();

            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "GetFinancialSetting Request Message");
            FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(subCompany.ParentId.Value);
            bool? IsMultiCurrencyActivate = _multiCurrencySettingService.GetByFlagCompanyId(subCompany.ParentId.Value);
            var chartOfAccount = _chartOfAccountService.GetByName("Other Income", subCompany.ParentId.Value);
            var TermsOfPayment = _termsOfPaymentService.GetTermsOfPayments(invBindingModel.TermsOfPayment, subCompany.ParentId.Value);
            long? termsOfPaymentId = 0;
            if (TermsOfPayment != null)
            {
                termsOfPaymentId = TermsOfPayment.Id;
            }
            else
            {
                TermsOfPayment termsOfPayment = new TermsOfPayment();
                termsOfPayment.Id = 0;
                termsOfPayment.CompanyId = subCompany.ParentId != null ? subCompany.ParentId.Value : 0;
                termsOfPayment.IsVendor = true;
                termsOfPayment.Name = "Credit - " + invBindingModel.TermsOfPayment;
                termsOfPayment.TOPValue = invBindingModel.TermsOfPayment;
                termsOfPayment.UserCreated = "System";
                termsOfPayment.CreatedDate = DateTime.UtcNow;
                termsOfPayment.TermsType = "Vendor";
                termsOfPayment.RecOrder = 1;
                termsOfPayment.Version = 1;
                termsOfPayment.ObjectState = ObjectState.Added;
                termsOfPayment.Status = RecordStatusEnum.Active;
                if (termsOfPayment != null)
                {
                    var json = RestSharpHelper.ConvertObjectToJason(termsOfPayment);
                    try
                    {

                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "calling TermsOfPayment Rest call");
                        string url = ConfigurationManager.AppSettings["AdminUrl"];
                        //string url = "http://localhost:57588/";
                        var response = RestSharpHelper.Post(url, "api/master/termsofpaymentduplicate", json);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "calling TermsOfPayment Rest call sucess");
                        }
                    }
                    catch (Exception ex)
                    {
                        // var message = ex.Message;
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "calling TermsOfPayment Rest call failed");
                    }
                    var termsofPymt = _termsOfPaymentService.GetTermsOfPayments(invBindingModel.TermsOfPayment, subCompany.ParentId);
                    if (termsofPymt != null)
                        termsOfPaymentId = termsofPymt.Id;
                }
            }

            BeanEntity beanEntity = new BeanEntity();
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "GetEntityByName Request message");
            beanEntity = _beanEntityService.GetEntityByName(invBindingModel.EntityName, subCompany.ParentId);
            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "GetEntityByName Request Sucess message");
            if (beanEntity != null)
            {

                LoggingHelper.LogMessage(BillConstants.BillApplicationService, "BillCreation methode started");
                BillCreation(invBindingModel, IsMultiCurrencyActivate, subCompany, beanEntity,
                     lstInvoiceLineItems, financSettings, chartOfAccount, termsOfPaymentId, cacLegalMonetaryTotal);
            }
            else
            {

                LoggingHelper.LogMessage(BillConstants.BillApplicationService, "Bean Entity Creation started");
                BeanEntityModel BeanEntityModel = new BeanEntityModel();
                BeanEntityModel.Id = Guid.NewGuid();
                BeanEntityModel.CompanyId = subCompany.ParentId != null ? subCompany.ParentId.Value : 0;
                BeanEntityModel.Name = invBindingModel.EntityName;
                BeanEntityModel.VenTOPId = termsOfPaymentId;
                BeanEntityModel.IsVendor = true;
                BeanEntityModel.VenCurrency = "SGD";
                BeanEntityModel.VenNature = "Trade";
                BeanEntityModel.IsMultiCurrencyActivated = IsMultiCurrencyActivate;
                BeanEntityModel.UserCreated = "System";
                BeanEntityModel.CreatedDate = DateTime.UtcNow;
                BeanEntityModel.VendorType = "Supplier";
                BeanEntityModel.IsExternalData = true;
                BeanEntityModel.Status = RecordStatusEnum.Active;
                BeanEntityModel.PeppolDocumentId = invBindingModel.SenderPeppolId;
                List<ContactModel> lstContactModels = new List<ContactModel>();
                ContactModel contactModel = new ContactModel();
                contactModel.Id = Guid.Empty;
                contactModel.ContactId = Guid.Empty;
                //contactModel.Salutation =;
                contactModel.FirstName = invBindingModel.ContactName;
                //contactModel.LastName =;
                contactModel.Status = RecordStatusEnum.Active;

                ContactDetailModel contactDetailModel = new ContactDetailModel();
                contactDetailModel.Id = Guid.NewGuid();
                var communiSerialize = JsonConvert.SerializeObject(communication);
                contactDetailModel.Communication = communiSerialize;
                contactDetailModel.IsPrimaryContact = true;
                contactDetailModel.UserCreated = "System";
                contactDetailModel.CreatedDate = DateTime.UtcNow;
                contactDetailModel.Status = RecordStatusEnum.Active;
                contactModel.ContactDetailModel = contactDetailModel;

                lstContactModels.Add(contactModel);

                BeanEntityModel.ContactModelList = lstContactModels;

                if (BeanEntityModel != null)
                {
                    var json = RestSharpHelper.ConvertObjectToJason(BeanEntityModel);
                    try
                    {

                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "savebeanentitymodelduplicate Rest call started");
                        string url = ConfigurationManager.AppSettings["BeanUrl"];
                        // string url = "http://localhost:57584/";
                        var response = RestSharpHelper.Post(url, "api/mastermodule/savebeanentitymodelduplicate", json);
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "savebeanentitymodelduplicate Rest call success and start  BillCreation method start");
                            var data = JsonConvert.DeserializeObject<BeanEntityModel>(response.Content);
                            LoggingHelper.LogMessage(BillConstants.BillApplicationService, "DeserializeObject BeanEntityModel Success message");
                            beanEntity = _beanEntityService.GetByEntityId(data.Id, data.CompanyId);
                            BillCreation(invBindingModel, IsMultiCurrencyActivate, subCompany, beanEntity, lstInvoiceLineItems, financSettings, chartOfAccount, termsOfPaymentId, cacLegalMonetaryTotal);
                        }
                    }
                    catch (Exception ex)
                    {
                        // var message = ex.Message;
                        LoggingHelper.LogMessage(BillConstants.BillApplicationService, "savebeanentitymodelduplicate Rest call failed");
                    }
                }
            }





            return "";
        }


        #endregion


    }
}
