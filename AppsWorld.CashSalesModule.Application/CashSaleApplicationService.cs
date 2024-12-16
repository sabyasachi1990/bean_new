using AppsWorld.CashSalesModule.Infra;
using AppsWorld.CashSalesModule.Models;
using AppsWorld.CashSalesModule.RepositoryPattern;
using AppsWorld.CashSalesModule.Service;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.Service;
using AppsWorld.Framework;
using Domain.Events;
using FrameWork;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using AppsWorld.CashSalesModule.Infra;
using AppsWorld.CashSalesModule.Entities.Models;
using System.Configuration;
using AppsWorld.CommonModule.Infra;
using Ziraff.Section;
using Ziraff.FrameWork.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;

namespace AppsWorld.CashSalesModule.Application
{
    public class CashSaleApplicationService
    {
        private readonly ICashSalesService _cashSaleService;
        private readonly ICashSalesDetailService _cashSalesDetailService;
        private readonly IControlCodeCategoryService _controlCodeCatService;
        private readonly ICurrencyService _currencyService;
        private readonly ICompanyService _companyService;
        private readonly ITaxCodeService _taxCodeService;
        private readonly IChartOfAccountService _chartOfAccountRepository;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly ICashSalesModuleUnitOfWorkAsync _unitOfWork;
        private readonly IAccountTypeService _accountTypeService;
        private readonly AppsWorld.CashSalesModule.Service.IAutoNumberService _autoNumberService;
        private readonly AppsWorld.CashSalesModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly IJournalService _journalService;
        private readonly IItemService _itemService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        private readonly CommonApplicationService _commonApplicationService;

        public CashSaleApplicationService(ICashSalesService cashSaleService, ICashSalesDetailService cashSalesDetailService, IControlCodeCategoryService controlCodeCatService, ICurrencyService currencyService, ICompanyService companyService, ITaxCodeService taxCodeService, IChartOfAccountService chartOfAccountRepository, IFinancialSettingService financialSettingService, IBeanEntityService beanEntityService, ICashSalesModuleUnitOfWorkAsync unitOfWork, IAccountTypeService accountTypeService, AppsWorld.CashSalesModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.CashSalesModule.Service.IAutoNumberService autoNumberService, IJournalService journalService, IItemService itemService, AppsWorld.CommonModule.Service.IAutoNumberService autoService, CommonApplicationService commonApplicationService)
        {
            _cashSaleService = cashSaleService;
            _cashSalesDetailService = cashSalesDetailService;
            _controlCodeCatService = controlCodeCatService;
            _currencyService = currencyService;
            _companyService = companyService;
            _taxCodeService = taxCodeService;
            _chartOfAccountRepository = chartOfAccountRepository;
            _financialSettingService = financialSettingService;
            _beanEntityService = beanEntityService;
            _unitOfWork = unitOfWork;
            _accountTypeService = accountTypeService;
            _autoNumberCompanyService = autoNumberCompanyService;
            _autoNumberService = autoNumberService;
            _journalService = journalService;
            _itemService = itemService;
            _autoService = autoService;
            _commonApplicationService = commonApplicationService;
        }

        #region SaveCall

        public CashSale SaveCashSale(CashSaleModel TObject, string connectionString)
        {
            bool isNewAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, "ObjectSave", AdditionalInfo);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_Into_Save_Cashsale);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Checking_All_Validations);
                string _errors = CommonValidation.ValidateObject(TObject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }

                //to check if the record is void or not
                if (_cashSaleService.IsVoid(TObject.CompanyId, TObject.Id))
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_to_void_cannot_save_the_record);
                if (TObject.DocDate == null)
                {
                    throw new InvalidOperationException(CashSaleValidation.Invalid_Document_Date);
                }
                if (TObject.ServiceCompanyId == null)
                    throw new InvalidOperationException();
                if (TObject.IsDocNoEditable == true)
                {
                    CashSale _cashSaleDoc = _cashSaleService.GetCashSaleDocNo(TObject.Id, TObject.DocNo, TObject.CompanyId);
                    if (_cashSaleDoc != null)
                    {
                        throw new InvalidOperationException(CommonConstant.Document_number_already_exist);
                    }
                }
                if (TObject.GrandTotal < 0)
                {
                    throw new InvalidOperationException(CashSaleValidation.Grand_total_should_be_greater_than_zero);
                }
                if (TObject.CashSaleDetails.Any())
                {
                    foreach (var Cashsale in TObject.CashSaleDetails)
                    {
                        if (Cashsale.ItemId != null && Cashsale.Qty == null)
                            throw new InvalidOperationException(CashSaleValidation.Pleas_Enter_Quantity);
                        if (Cashsale.ItemId == null && Cashsale.Qty != null)
                            throw new InvalidOperationException(CashSaleValidation.Please_Select_Item);
                    }
                }

                if (TObject.CashSaleDetails == null || TObject.CashSaleDetails.Count == 0 || !TObject.CashSaleDetails.Any())
                {
                    throw new InvalidOperationException(CashSaleValidation.Atleast_one_Sale_Item_is_required);
                }
                if (TObject.ExchangeRate == 0)
                    throw new InvalidOperationException(CashSaleValidation.ExchangeRate_Should_Be_Grater_Than_zero);
                if (TObject.GSTExchangeRate == 0)
                    throw new InvalidOperationException(CashSaleValidation.GSTExchangeRate_Should_Be_Grater_Than_zero);
                //Need to verify the invoice is within Financial year
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }
                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(CashSaleValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new InvalidOperationException(CashSaleValidation.Invalid_Financial_Period_Lock_Password);
                    }
                }
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Validations_Checking_Finished);
                CashSale _cashSale = _cashSaleService.GetCashSaleByIdAndCompanyId(TObject.Id, TObject.CompanyId);
                string oldDocumentNo = string.Empty;
                if (_cashSale != null)
                {
                    oldDocumentNo = _cashSale.DocNo;
                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Validationg_The_CashSale_In_Edit_Mode);
                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_To_Execute_InsertCashSale_Methos);

                    //Data Concurancy check
                    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_cashSale.Version, x => x.ToString("X2")));
                    if (!timeStamp.Equals(TObject.Version))
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                    InsertCashSale(TObject, _cashSale);
                    _cashSale.DocNo = TObject.DocNo;
                    _cashSale.CashSaleNumber = _cashSale.DocNo;
                    _cashSale.ModifiedBy = TObject.ModifiedBy;
                    _cashSale.ModifiedDate = DateTime.UtcNow;
                    _cashSale.ObjectState = ObjectState.Modified;
                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_To_Execute_UpdateCashSaleDetail_Methos);
                    UpdateCashSaleDetails(TObject, _cashSale);
                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_to_Execute_UpdateCashSaleGstDetail_Method);
                    _cashSaleService.Update(_cashSale);
                }
                else
                {
                    _cashSale = new CashSale();
                    isNewAdd = true;
                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_to_execute_InsertCashSle_method_in_insert_new_mode);
                    InsertCashSale(TObject, _cashSale);
                    _cashSale.Id = Guid.NewGuid();
                    int? recorder = 0;
                    if (TObject.CashSaleDetails.Count > 0 || TObject.CashSaleDetails != null)
                    {
                        List<CashSaleDetail> lstCashSaleDetail = new List<CashSaleDetail>();
                        lstCashSaleDetail.AddRange(TObject.CashSaleDetails.Select(detail => new CashSaleDetail
                        {
                            Id=Guid.NewGuid(),
                            CashSaleId=_cashSale.Id,
                            AllowDisAllow = detail.AllowDisAllow,
                            AmtCurrency = detail.AmtCurrency,
                            DocAmount = detail.DocAmount,
                            DocTaxAmount = detail.DocTaxAmount,
                            DocTotalAmount = detail.DocTotalAmount,
                            TaxCurrency = detail.TaxCurrency,
                            TaxId = detail.TaxId,
                            TaxRate = detail.TaxRate,
                            Unit = detail.Unit,
                            UnitPrice = detail.UnitPrice,
                            BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount,
                            BaseTaxAmount = TObject.ExchangeRate != null && detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2,
                        MidpointRounding.AwayFromZero) : detail.DocTaxAmount,
                            BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2,
                        MidpointRounding.AwayFromZero),
                            COAId = detail.COAId,
                            Discount = detail.Discount,
                            DiscountType = detail.DiscountType,
                            ItemDescription = detail.ItemDescription,
                            ItemId = detail.ItemId,
                            Qty = detail.Qty,
                            IsPLAccount = detail.IsPLAccount,
                            TaxIdCode = detail.TaxIdCode,
                            RecOrder=++recorder,
                            ObjectState=ObjectState.Added
                        }));
                        if (lstCashSaleDetail.Any())
                            _cashSalesDetailService.InsertRange(lstCashSaleDetail);

                    }

                    _cashSale.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                    _cashSale.UserCreated = TObject.UserCreated;
                    _cashSale.CreatedDate = DateTime.UtcNow;

                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Going_To_Execute_Auto_Number_Method);
                    _cashSale.CashSaleNumber = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.CashSale, connectionString) : TObject.DocNo;
                    isDocAdd = true;
                    _cashSale.DocNo = _cashSale.CashSaleNumber;
                    _cashSale.ObjectState = ObjectState.Added;
                    _cashSaleService.Insert(_cashSale);
                }
                try
                {
                    _unitOfWork.SaveChanges();
                    //new posting changes
                    AppaWorld.Bean.Common.SavePosting(_cashSale.CompanyId, _cashSale.Id, _cashSale.DocType, connectionString);

                    #region Document Folder Rename

                    if (isNewAdd == false && oldDocumentNo != TObject.DocNo)
                        _commonApplicationService.ChangeFolderName(TObject.CompanyId, TObject.DocNo, oldDocumentNo, "Cash Sales");

                    #endregion

                    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.SaveChanges_method_execution_happened);

                }
                catch (DbEntityValidationException e)
                {
                    LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, e, e.Message);
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
                    throw e;
                }
                return _cashSale;
            }
            catch (Exception ex)
            {
                if (isNewAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocType, connectionString);
                }
                throw ex;
            }
        }
        public CashSale SaveCashSaleDocumentVoid(DocumentVoidModel TObject)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_into_SaveCashSaleDocumentVoid_method);
            //to check if it is void or not
            if (_cashSaleService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
            string DocNo = "-V";
            CashSale cashSale = _cashSaleService.GetCashSaleByIdAndCompanyId(TObject.Id, TObject.CompanyId);
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Validating_model_and_proceed_towards_the_functional_validation);
            if (cashSale == null)
                throw new Exception(CashSaleValidation.Invalid_CashSale);
            else
            {
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(cashSale.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            if (cashSale.CashSaleDetails.Any(a => a.ClearingState == CashSaleStatus.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);


            //if (_withDrawal.DocumentState != DebitNoteState.NotPaid)
            //    throw new Exception("State should be " + DebitNoteState.NotPaid);

            //Need to verify the withdrawal within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(cashSale.DocDate, cashSale.CompanyId))
            {
                throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Functionality_validation_going_on);
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(cashSale.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(cashSale.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.End_of_the_Functionality_validation);
            }
            cashSale.DocumentState = CashSaleStatus.Void;
            cashSale.DocNo = cashSale.DocNo + DocNo;
            cashSale.ModifiedDate = DateTime.UtcNow;
            cashSale.ModifiedBy = TObject.ModifiedBy;
            //cashSale.DocNo = cashSale.DocNo + DocNo;
            cashSale.ObjectState = ObjectState.Modified;
            try
            {
                _unitOfWork.SaveChanges();
                DocumentVoidModel tObject = new DocumentVoidModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocType = cashSale.DocType;
                tObject.DocNo = cashSale.DocNo;
                tObject.ModifiedBy = TObject.ModifiedBy;
                VoidJVPostCashSale(tObject);
                LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.End_of_the_Doc_Void_method);
            return cashSale;
        }


        #endregion

        #region Create And Lookup Call

        public CashSaleModelLU GetAllCashSalesLUs(string userName, Guid cashsaleId, long companyId)
        {
            CashSaleModelLU cashsaleLU = new CashSaleModelLU();
            try
            {
                CashSale lastCashSale = _cashSaleService.GetCashsaleByCompanyId(companyId);
                CashSale cashSale = _cashSaleService.GetCashSaleLU(companyId, cashsaleId);
                DateTime date = cashSale == null ? lastCashSale == null ? DateTime.Now : lastCashSale.DocDate : cashSale.DocDate;
                //Guid cashsaleGuid = cashsaleId;
                //cashsaleLU.ModeOfReciptLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_codes_ModeOfTransfer);
                //cashsaleLU.AllowableNonallowableLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
                //cashsaleLU.DocumentTypeLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_codes_DocumentType);
                //List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
                //if (cashSale == null)
                //{
                //    if (segments.Count > 0)
                //        cashsaleLU.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        cashsaleLU.SegmentCategory2LU = segments[1];
                //    // invoiceLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyid);
                //}
                //else
                //{
                //    if (cashSale.SegmentMasterid1 != null)
                //        cashsaleLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyId, cashSale.SegmentMasterid1);
                //    else
                //        if (segments.Count > 0)
                //        cashsaleLU.SegmentCategory1LU = segments[0];
                //    if (cashSale.SegmentMasterid2 != null)
                //        cashsaleLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyId, cashSale.SegmentMasterid2);
                //    else
                //        if (segments.Count > 1)
                //        cashsaleLU.SegmentCategory2LU = segments[1];
                //}

                //if (cashSale != null)
                //{
                //    if (cashSale.SegmentMasterid1 != null)
                //        cashsaleLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyId, cashSale.SegmentMasterid1);
                //    if (cashSale.SegmentMasterid2 != null)
                //        cashsaleLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyId, cashSale.SegmentMasterid2);
                //}
                //else
                //{
                //    List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
                //    if (segments.Count > 0)
                //        cashsaleLU.SegmentCategory1LU = segments[0];
                //    if (segments.Count > 1)
                //        cashsaleLU.SegmentCategory2LU = segments[1];
                //}
                cashsaleLU.CompanyId = companyId;
                if (cashSale != null)
                {
                    string currencyCode = cashSale.DocCurrency;
                    cashsaleLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, currencyCode, ControlCodeConstants.Currency_DefaultCode);
                    //var lookUpModeOfRecipt = _controlCodeCatService.GetByCategoryCodeCategory(companyId,
                    //       ControlCodeConstants.Control_codes_ModeOfTransfer, cashSale.ModeOfReceipt);
                    //if (lookUpModeOfRecipt != null)
                    //{

                    //    cashsaleLU.ModeOfReciptLU.Lookups.Add(lookUpModeOfRecipt);
                    //}
                    cashsaleLU.ModeOfReciptLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId,
                           ControlCodeConstants.Control_codes_ModeOfTransfer, cashSale.ModeOfReceipt);
                }
                else
                {
                    cashsaleLU.ModeOfReciptLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId,
                           ControlCodeConstants.Control_codes_ModeOfTransfer, string.Empty);
                    cashsaleLU.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
                }
                long? comp = cashSale == null ? 0 : cashSale.ServiceCompanyId == null ? 0 : cashSale.ServiceCompanyId;
                List<long> coaIds = new List<long>();
                if (cashSale != null)
                    coaIds.Add(cashSale.COAId);
                cashsaleLU.SubsideryCompanyLU = _companyService.ListOfSubsudaryCompanyActiveInactive(companyId, comp, cashsaleId, coaIds, userName);
                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
                //string coaName = COANameConstants.Revenue;
                List<AccountType> accType = _accountTypeService.GetAllAccounyTypeByName(companyId, coaName);
                cashsaleLU.ChartOfAccountLU = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Code = x.Code,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                }).OrderBy(d => d.Name).ToList()).ToList();
                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    cashsaleLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (cashSale != null && cashSale.CashSaleDetails.Count > 0)
                {
                    List<long> CoaIds = cashSale.CashSaleDetails.Select(c => c.COAId).ToList();
                    if (cashsaleLU.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(cashsaleLU.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = cashSale.CashSaleDetails.Select(x => x.TaxId).ToList();
                    if (cashsaleLU.TaxCodeLU != null && cashsaleLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(cashsaleLU.TaxCodeLU.Select(d => d.Id)).ToList();
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
                        lstEditCoa = accType.SelectMany(a => a.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.Code,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        }).OrderBy(d => d.Name).ToList()).ToList();
                        cashsaleLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                    if (cashSale.IsGstSettings && taxIds.Any())
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
                        cashsaleLU.TaxCodeLU.AddRange(lstEditTax);
                        cashsaleLU.TaxCodeLU = cashsaleLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                    //List<long> CoaIds = cashSale.CashSaleDetails.Select(c => c.COAId).ToList();
                    //List<long> taxIds = cashSale.CashSaleDetails.Select(x => x.TaxId.Value).ToList();



                    ////creditLU.ChartOfAccountLU.Where(c => c.Id == CoaIds.Contains());
                    //if (CoaIds.Any())
                    //{
                    //    List<long> lstcoaId = cashsaleLU.ChartOfAccountLU.Select(c => c.Id).ToList().Intersect(CoaIds.ToList()).ToList();
                    //    var coalst = lstcoaId.Except(cashsaleLU.ChartOfAccountLU.Select(x => x.Id));
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
                    //        cashsaleLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    //    }
                    //}


                    ////var common = creditLU.ChartOfAccountLU.Intersect(lstEditCoa.Select(x=>x.Id));


                    //if (taxIds.Any())
                    //{
                    //    List<long> lsttaxId = cashsaleLU.TaxCodeLU.Select(d => d.Id).ToList().Intersect(taxIds.ToList()).ToList();
                    //    var taxlst = lsttaxId.Except(cashsaleLU.TaxCodeLU.Select(x => x.Id));
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
                    //        cashsaleLU.TaxCodeLU.AddRange(lstEditTax);
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }

            return cashsaleLU;
        }

        //public CashAndBankCurrencyLU GetCashAndBankCurrencyLU(Guid id, long companyId, string currency)
        //{
        //    CashAndBankCurrencyLU cashAndBankCurrencyLU = new CashAndBankCurrencyLU();
        //    try
        //    {
        //        CashSale cashSale = _cashSaleService.GetAllCashSalesLUs(id, companyId);
        //        long? subCompanyId = cashSale.ServiceCompanyId == null ? 0 : cashSale.ServiceCompanyId;
        //        cashAndBankCurrencyLU.BankCurrencyLU = _companyService.CashAndBankCurrency(companyId, subCompanyId, currency);

        //    }
        //    catch(Exception e)
        //    {
        //        throw e;
        //    }
        //    return cashAndBankCurrencyLU;
        //}

        public CashSaleModel CreateCashSales(long companyId, Guid id, bool isCopy, string connectionString)
        {
            CashSaleModel cashSaleDTO = new CashSaleModel();
            try
            {

                FinancialSetting financialsetting = _financialSettingService.GetFinancialSetting(companyId);
                if (financialsetting == null)
                {
                    throw new Exception(CashSaleValidation.The_Financial_setting_should_be_activated);
                }
                cashSaleDTO.FinancialPeriodLockStartDate = financialsetting.PeriodLockDate;
                cashSaleDTO.FinancialPeriodLockEndDate = financialsetting.PeriodEndDate;
                CashSale cashSale = _cashSaleService.GetCashSaleByIdAndCompanyId(id, companyId);

                //List<GSTDetailModel> listGstDetailModel = new List<GSTDetailModel>();
                //cashSaleDTO.IsSegmentActive1 = true;
                //cashSaleDTO.IsSegmentActive2 = true;
                //var bank = _bankReconciliationService.GetByCompanyId(companyId);

                //if (bank != null)
                //    cashSaleDTO.IsBankReconciliation = true;
                if (cashSale == null)
                {
                    //AppsWorld.CashSalesModule.Entities.Models.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CashSale);
                    CashSale listcashSale = _cashSaleService.GetCashsaleByCompanyId(companyId);
                    cashSaleDTO.Id = Guid.NewGuid();
                    cashSaleDTO.CompanyId = companyId;
                    cashSaleDTO.DocDate = listcashSale == null ? DateTime.Now : listcashSale.DocDate;
                    //cashSaleDTO.DocNo = GetNewCashSleDocumentNumber(DocTypeConstants.CashSale, companyId);
                    //if (bank != null)
                    //    cashSaleDTO.BankClearingDate = bank.BankClearingDate;
                    cashSaleDTO.NoSupportingDocument = false;

                    cashSaleDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberIsEditable(companyId, DocTypeConstants.CashSale);
                    if (cashSaleDTO.IsDocNoEditable == true)
                        cashSaleDTO.DocNo = _autoService.GetAutonumber(companyId, DocTypeConstants.CashSale, connectionString);

                    //bool? isEdit = false;
                    //cashSaleDTO.DocNo = GetAutoNumberByEntityType(companyId, listcashSale, _autoNo, ref isEdit);
                    //cashSaleDTO.IsDocNoEditable = isEdit;

                    //cashSaleDTO.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
                    //cashSaleDTO.IsRepeatingInvoice = false;
                    //cashSaleDTO.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, companyId);
                    cashSaleDTO.CreatedDate = DateTime.UtcNow;
                    //cashSaleDTO.IsGSTDeregistered = _gstSettingService.IsGSTAllowed(companyId, cashSaleDTO.DocDate);
                    //cashSaleDTO.IsGstSettings = _gstSettingService.IsGSTSettingActivated(companyId);
                    //cashSaleDTO.IsMultiCurrency = false;
                    //cashSaleDTO.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, companyId);
                    cashSaleDTO.BaseCurrency = financialsetting.BaseCurrency;
                    cashSaleDTO.DocCurrency = cashSaleDTO.BaseCurrency;
                    cashSaleDTO.DocType = DocTypeConstants.CashSale;
                    cashSaleDTO.DocSubType = DocTypeConstants.CashSale;
                    cashSaleDTO.IsLocked = false;
                    //Forex forexBean;
                    //MultiCurrencySetting multi = _multiCurrencyRepository.GetMultiCurrency(companyId);
                    ////cashSaleDTO.IsMultiCurrency = multi != null;
                    //if (multi != null)
                    //{
                    //    var financial = _financialSettingService.Query(c => c.CompanyId == cashSaleDTO.CompanyId).Select().FirstOrDefault();
                    //    if (financial != null)
                    //        cashSaleDTO.DocCurrency = financial.BaseCurrency;
                    //}
                    //forexBean = _forexService.GetMultiCurrencyInformation(cashSaleDTO.BaseCurrency, cashSaleDTO.DocDate, true, cashSaleDTO.CompanyId);
                    //if (forexBean != null)
                    //{
                    //    cashSaleDTO.ExchangeRate = forexBean.UnitPerUSD;
                    //    cashSaleDTO.ExDurationFrom = forexBean.DateFrom;
                    //    cashSaleDTO.ExDurationTo = forexBean.Dateto;
                    //}
                    //GSTSetting GSTSetting = _gSTSettingRepository.GetgstDetail(companyId);
                    //if ( GSTSetting != null)
                    //{
                    //    cashSaleDTO.GSTExCurrency = GSTSetting.GSTRepoCurrency;
                    //    //forexBean = _forexService.GetMultiCurrencyInformation(cashSaleDTO.BaseCurrency, cashSaleDTO.DocDate, false, cashSaleDTO.CompanyId);
                    //}
                    //else
                    //cashSaleDTO.IsGstSettings = false;
                    //cashSaleDTO.IsBaseCurrencyRateChanged = false;
                    //cashSaleDTO.IsGSTCurrencyRateChanged = false;
                }
                else
                {
                    List<CashSaleDetailModel> listCashsaleDetailModel = new List<CashSaleDetailModel>();
                    FillCashSales(cashSaleDTO, cashSale, isCopy);
                    cashSaleDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberIsEditable(companyId, DocTypeConstants.CashSale);
                    cashSaleDTO.IsLocked = cashSale.IsLocked;
                    if (isCopy)
                        cashSaleDTO.DocNo = isCopy && cashSaleDTO.IsDocNoEditable == true ? _autoService.GetAutonumber(companyId, DocTypeConstants.CashSale, connectionString) : null;
                    List<Item> lstItem = _itemService.GetAllItemById(cashSale.CashSaleDetails.Select(c => c.ItemId).ToList(), cashSale.CompanyId);
                    foreach (var detail in cashSale.CashSaleDetails)
                    {
                        CashSaleDetailModel cashSaleDetailModel = new CashSaleDetailModel();
                        FillCashDetails(cashSaleDetailModel, detail, isCopy);
                        cashSaleDetailModel.ItemDescription = (detail.ItemDescription != null || detail.ItemDescription != string.Empty) ? detail.ItemDescription : lstItem.Where(c => c.Id == detail.ItemId).Select(d => d.Description).FirstOrDefault();
                        listCashsaleDetailModel.Add(cashSaleDetailModel);
                    }
                    cashSaleDTO.CashSaleDetails = listCashsaleDetailModel.OrderBy(c => c.RecOrder).ToList();
                    if (cashSale.DocumentState != CashSaleStatus.Void)
                    {
                        Journal journal = _journalService.GetJournals(id, companyId);
                        if (journal != null)
                            cashSaleDTO.JournalId = journal.Id;
                    }
                    //var lstGSTDetail = _gstDetailService.GetAllGstDetail(cashSale.Id);
                    //var lstGSTModel = new List<GSTDetailModel>();
                    //if (lstGSTDetail != null)
                    //{
                    //    foreach (var gstDetail in lstGSTDetail)
                    //    {
                    //        GSTDetailModel gstModel = new GSTDetailModel();
                    //        FillGstDetail(gstModel, gstDetail);
                    //        lstGSTModel.Add(gstModel);
                    //    }

                    //}
                    //cashSaleDTO.GSTDetailModels = lstGSTModel;
                }

                //cashSaleDTO.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
                //cashSaleDTO.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
                //cashSaleDTO.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
                //cashSaleDTO.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
            }

            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            return cashSaleDTO;
        }

        public CashSaleDetailModel GetCashSale(Guid id)
        {
            CashSaleDetailModel cashSaledetailModel = new CashSaleDetailModel();
            return cashSaledetailModel;
        }
        #endregion

        #region Kendo Call
        public IQueryable<CashSaleModelK> GetAllCashSalesK(string username, long companyId)
        {
            return _cashSaleService.GetAllCashSalesK(username, companyId);
        }
        #endregion

        #region Private Methods

        private string GetAutoNumberByEntityType(long companyId, CashSale listcashSale, AppsWorld.CashSalesModule.Entities.Models.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;

            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewCashSleDocumentNumber(DocTypeConstants.CashSale, companyId);
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
                    if (listcashSale != null)
                    {
                        if (_autoNo.Format.Contains("{MM/YYYY}"))
                        {
                            //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                            if (listcashSale.CreatedDate.Value.Month != DateTime.UtcNow.Month)
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

        public string GetNewCashSleDocumentNumber(string docType, long companyId)
        {
            CashSale cashSale = _cashSaleService.GetDocTypeAndCompanyid(docType, companyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (cashSale != null)
            {
                string strOldNo = String.Empty;
                CashSale duplicatCashSale;
                int index;
                strOldDocNo = cashSale.DocNo;

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

                    duplicatCashSale = _cashSaleService.DuplicateCashsale(strNewDocNo, docType, companyId);
                } while (duplicatCashSale != null);
            }
            return strNewDocNo;
        }

        //private void FillCashSaleDetail(CashSaleDetailModel cashSaleDetailModel, CashSaleDetail detail)
        //{
        //    cashSaleDetailModel.AllowDisAllow = detail.AllowDisAllow;
        //    cashSaleDetailModel.AmtCurrency = detail.AmtCurrency;
        //    cashSaleDetailModel.BaseAmount = detail.BaseAmount;
        //    cashSaleDetailModel.BaseTaxAmount = detail.BaseTaxAmount;
        //    cashSaleDetailModel.BaseTotalAmount = detail.BaseTotalAmount;
        //    // detail.Cashsales= casDet.CashSale;
        //    cashSaleDetailModel.CashSaleId = detail.CashSaleId;
        //    cashSaleDetailModel.COAId = detail.COAId;
        //    cashSaleDetailModel.Discount = detail.Discount;
        //    cashSaleDetailModel.DiscountType = detail.DiscountType;
        //    cashSaleDetailModel.DocAmount = detail.DocAmount;
        //    cashSaleDetailModel.DocTaxAmount = detail.DocTaxAmount;
        //    cashSaleDetailModel.DocTotalAmount = detail.DocTotalAmount;
        //    cashSaleDetailModel.Id = detail.Id;
        //    cashSaleDetailModel.ItemCode = detail.ItemCode;
        //    cashSaleDetailModel.ItemDescription = detail.ItemDescription;
        //    cashSaleDetailModel.ItemId = detail.ItemId;
        //    cashSaleDetailModel.Qty = detail.Qty;
        //    cashSaleDetailModel.RecOrder = detail.RecOrder;
        //    //cashSaleDetailModel.Remarks = detail.Remarks;
        //    cashSaleDetailModel.TaxCurrency = detail.TaxCurrency;
        //    cashSaleDetailModel.TaxId = detail.TaxId;
        //    cashSaleDetailModel.Unit = detail.Unit;
        //    cashSaleDetailModel.UnitPrice = detail.UnitPrice;
        //    //cashSaleDetailModel.TaxIdCode=detail.ta
        //    if (detail.TaxId != null)
        //    {
        //        var tax = _taxCodeService.GetTaxCode(detail.TaxId.Value);
        //        if (tax != null)
        //        {
        //            cashSaleDetailModel.TaxCode = tax.Code;
        //            cashSaleDetailModel.TaxIdCode = tax.Code + "(" + tax.TaxRate + "%" + ")";
        //        }
        //    }

        //}

        private void FillCashSales(CashSaleModel cashSaleDTO, CashSale cashSale, bool isCopy)
        {
            cashSaleDTO.Id = isCopy ? Guid.NewGuid() : cashSale.Id;
            cashSaleDTO.CompanyId = cashSale.CompanyId;
            cashSaleDTO.EntityType = cashSale.EntityType;
            cashSaleDTO.DocSubType = DocTypeConstants.CashSale;
            cashSaleDTO.DocType = cashSale.DocType;
            cashSaleDTO.DocNo = cashSale.DocNo;
            cashSaleDTO.Version = "0x" + string.Concat(Array.ConvertAll(cashSale.Version, x => x.ToString("X2")));
            cashSaleDTO.DocDate = cashSale.DocDate;
            cashSaleDTO.PONo = cashSale.PONo;
            cashSaleDTO.ModeOfReceipt = cashSale.ModeOfReceipt;
            cashSaleDTO.EntityId = cashSale.EntityId;
            //BeanEntity beanEntity = _beanEntityService.Query(a => a.Id == cashSale.EntityId).Select().FirstOrDefault();
            cashSaleDTO.EntityName = cashSale.EntityId != null ? _beanEntityService.GetEntityName(cashSale.EntityId.Value) : null;
            cashSaleDTO.COAId = cashSale.COAId;
            //cashSaleDTO.CustCreditlimit = beanEntity.CustCreditLimit;

            cashSaleDTO.ReceiptrefNo = cashSale.ReceiptrefNo;
            cashSaleDTO.CashSaleNumber = cashSale.CashSaleNumber;

            cashSaleDTO.DocCurrency = cashSale.DocCurrency;
            cashSaleDTO.ServiceCompanyId = cashSale.ServiceCompanyId;

            //var company = _companyService.GetByNameByServiceCompany(cashSale.ServiceCompanyId.Value);            
            //if (company != null)
            //    cashSaleDTO.ServiceCompanyModels.ServiceCompanyName = company.ShortName;

            cashSaleDTO.ReceiptrefNo = cashSale.ReceiptrefNo;
            cashSaleDTO.IsMultiCurrency = cashSale.IsMultiCurrency;
            cashSaleDTO.BaseCurrency = cashSale.ExCurrency;
            cashSaleDTO.ExchangeRate = cashSale.ExchangeRate;
            cashSaleDTO.ExDurationFrom = cashSale.ExDurationFrom;
            cashSaleDTO.ExDurationTo = cashSale.ExDurationTo;

            cashSaleDTO.IsGstSettings = cashSale.IsGstSettings;
            cashSaleDTO.GSTExCurrency = cashSale.GSTExCurrency;
            cashSaleDTO.GSTExchangeRate = cashSale.GSTExchangeRate;
            cashSaleDTO.GSTExDurationFrom = cashSale.GSTExDurationFrom;
            cashSaleDTO.GSTExDurationTo = cashSale.GSTExDurationTo;

            cashSaleDTO.IsSegmentReporting = cashSale.IsSegmentReporting;
            //cashSaleDTO.SegmentCategory1 = cashSale.SegmentCategory1;
            //cashSaleDTO.SegmentCategory2 = cashSale.SegmentCategory2;
            cashSaleDTO.BankClearingDate = cashSale.BankClearingDate;

            cashSaleDTO.BalanceAmount = isCopy ? cashSale.GrandTotal : cashSale.BalanceAmount;
            cashSaleDTO.GSTTotalAmount = cashSale.GSTTotalAmount;
            cashSaleDTO.GrandTotal = cashSale.GrandTotal;
            cashSaleDTO.Remarks = cashSale.Remarks;
            cashSaleDTO.IsGSTApplied = cashSale.IsGSTApplied;
            cashSaleDTO.DocDescription = cashSale.DocDescription;

            cashSaleDTO.IsAllowableNonAllowable = cashSale.IsAllowableNonAllowable;

            cashSaleDTO.IsNoSupportingDocument = cashSale.IsNoSupportingDocument;
            cashSaleDTO.NoSupportingDocument = cashSale.NoSupportingDocs;
            //cashSaleDTO.SegmentMasterid1 = cashSale.SegmentMasterid1;

            cashSaleDTO.IsBaseCurrencyRateChanged = cashSale.IsBaseCurrencyRateChanged;
            cashSaleDTO.IsGSTCurrencyRateChanged = cashSale.IsGSTCurrencyRateChanged;

            //if (cashSale.SegmentMasterid1 != null)
            //{
            //    var segment1 = _segmentMasterService.GetSegmentMastersById(cashSale.SegmentMasterid1.Value).FirstOrDefault();
            //    cashSaleDTO.IsSegmentActive1 = segment1.Status == AppsWorld.Framework.RecordStatusEnum.Active;
            //}
            //if (cashSale.SegmentMasterid2 != null)
            //{
            //    var segment2 = _segmentMasterService.GetSegmentMastersById(cashSale.SegmentMasterid2.Value).FirstOrDefault();
            //    cashSaleDTO.IsSegmentActive2 = segment2.Status == AppsWorld.Framework.RecordStatusEnum.Active;
            //}

            //cashSaleDTO.SegmentMasterid2 = cashSale.SegmentMasterid2;
            //cashSaleDTO.SegmentDetailid1 = cashSale.SegmentDetailid1;
            //cashSaleDTO.SegmentDetailid2 = cashSale.SegmentDetailid2;

            cashSaleDTO.Status = cashSale.Status;
            cashSaleDTO.DocumentState = isCopy ? null : cashSale.DocumentState;
            cashSaleDTO.ModifiedDate = isCopy ? null : cashSale.ModifiedDate;
            cashSaleDTO.ModifiedBy = isCopy ? null : cashSale.ModifiedBy;
            cashSaleDTO.CreatedDate = isCopy ? null : cashSale.CreatedDate;
            cashSaleDTO.UserCreated = isCopy ? null : cashSale.UserCreated;
        }

        private void FillCashDetails(CashSaleDetailModel detail, CashSaleDetail casDet, bool isCopy)
        {
            detail.AllowDisAllow = casDet.AllowDisAllow;
            detail.AmtCurrency = casDet.AmtCurrency;
            detail.BaseAmount = casDet.BaseAmount;
            detail.BaseTaxAmount = casDet.BaseTaxAmount;
            detail.BaseTotalAmount = casDet.BaseTotalAmount;
            // detail.Cashsales= casDet.CashSale;
            detail.CashSaleId = casDet.CashSaleId;
            detail.COAId = casDet.COAId;
            detail.Discount = casDet.Discount;
            detail.DiscountType = casDet.DiscountType;
            detail.DocAmount = casDet.DocAmount;
            detail.DocTaxAmount = casDet.DocTaxAmount;
            detail.DocTotalAmount = casDet.DocTotalAmount;
            detail.Id = isCopy ? Guid.NewGuid() : casDet.Id;
            //detail.ItemCode = casDet.ItemCode;
            detail.ItemId = casDet.ItemId;
            detail.Qty = casDet.Qty;
            detail.RecOrder = casDet.RecOrder;
            //detail.Remarks = casDet.Remarks;
            detail.TaxCurrency = casDet.TaxCurrency;
            detail.Unit = casDet.Unit;
            detail.UnitPrice = casDet.UnitPrice;
            detail.IsPLAccount = casDet.IsPLAccount;
            detail.TaxId = casDet.TaxId;
            detail.TaxRate = casDet.TaxRate;
            detail.TaxIdCode = casDet.TaxIdCode;
            detail.ClearingState = casDet.ClearingState;
            //ChartOfAccount coa = _chartOfAccountRepository.Query(a => a.Id == casDet.COAId).Select().FirstOrDefault();
            //if (coa != null)
            //    detail.AccountName = coa.Name;
            //if (casDet.TaxId != null)
            //{
            //    TaxCode tax = _taxCodeService.GetTaxCode(casDet.TaxId.Value);
            //    if (tax != null)
            //    {
            //        detail.TaxId = tax.Id;
            //        detail.TaxRate = tax.TaxRate;
            //        detail.TaxIdCode = tax.Code != "NA" ? tax.Code + "-" + tax.TaxRate + (tax.TaxRate != null ? "%" : "NA") + "(" + tax.TaxType[0] + ")" : tax.Code;
            //        detail.TaxType = tax.TaxType;
            //        detail.TaxCode = tax.Code;
            //    }
            //}
            //var lstGSTDetail = _gstDetailService.GetAllGstDetail(casDet.Id);
            //var lstGSTModel = new List<GSTDetailModel>();
            //if (lstGSTDetail != null)
            //{
            //    foreach (var gstDetail in lstGSTDetail)
            //    {
            //        GSTDetailModel gstModel = new GSTDetailModel();
            //        FillGstDetail(gstModel, gstDetail);
            //        lstGSTModel.Add(gstModel);
            //    }
            //}
            //detail.GstDetailModels = lstGSTModel;
        }



        //private void UpdateCashSaleGSTDetails(CashSaleModel TObject, CashSale _cashSaleNew)
        //{
        //    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService,CashSaleLoggingValidation.Entred_Into_UpdateCashsaleGstDetail_Method);
        //    try
        //    {
        //        foreach (GSTDetailModel detail in TObject.GSTDetailModels)
        //        {
        //            if (detail.RecordStatus == "Added")
        //            {
        //                GSTDetail gstDetail = new GSTDetail();
        //                FillCashSaleGstDetail(gstDetail, detail);
        //                gstDetail.Id = Guid.NewGuid();
        //                gstDetail.ObjectState = ObjectState.Added;
        //                _gstDetailService.Insert(gstDetail);
        //            }
        //            else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
        //            {
        //                GSTDetail cashSaleGSTDetail = _gstDetailService.GetGSTById(detail.Id);

        //                if (cashSaleGSTDetail != null)
        //                {
        //                    cashSaleGSTDetail.TaxId = detail.TaxId;
        //                    cashSaleGSTDetail.Amount = detail.Amount;
        //                    cashSaleGSTDetail.TaxAmount = detail.TaxAmount;
        //                    cashSaleGSTDetail.TotalAmount = detail.TotalAmount;
        //                    cashSaleGSTDetail.ObjectState = ObjectState.Modified;
        //                }
        //            }
        //            else if (detail.RecordStatus == "Deleted")
        //            {
        //                GSTDetail CashSaleGSTDetail = _gstDetailService.GetGSTById(detail.Id);
        //                if (CashSaleGSTDetail != null)
        //                {
        //                    CashSaleGSTDetail.ObjectState = ObjectState.Deleted;
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService,CashSaleLoggingValidation.Exited_From_UpdateCashSaleGstDetail_Method);
        //}



        private void UpdateCashSaleDetails(CashSaleModel TObject, CashSale _CashSaleNew)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entred_Into_UpdateCashSaleDetail_Method_And_checking_Condition);
            try
            {
                int? recorder = 0;

                foreach (CashSaleDetailModel detail in TObject.CashSaleDetails)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        CashSaleDetail cashCaleNew = new CashSaleDetail();
                        FillCashSaleDetail(cashCaleNew, detail, TObject.ExchangeRate);
                        cashCaleNew.RecOrder = recorder + 1;
                        recorder = cashCaleNew.RecOrder;
                        cashCaleNew.Id = Guid.NewGuid();
                        cashCaleNew.CashSaleId = TObject.Id;
                        cashCaleNew.ObjectState = ObjectState.Added;
                        _cashSalesDetailService.Insert(cashCaleNew);

                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        CashSaleDetail cashSalesDetailE = _CashSaleNew.CashSaleDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (cashSalesDetailE != null)
                        {
                            CashSaleDetail cashsalesDetail = new CashSaleDetail();
                            FillCashSalesDetails(detail, cashSalesDetailE, TObject.ExchangeRate);
                            //cashSalesDetail.AccountName = detail.AccountName;
                            cashsalesDetail.RecOrder = recorder + 1;
                            recorder = cashsalesDetail.RecOrder;
                            cashsalesDetail.ObjectState = ObjectState.Modified;
                            _cashSalesDetailService.Update(cashSalesDetailE);
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        CashSaleDetail CashSaleDetails = _CashSaleNew.CashSaleDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                        if (CashSaleDetails != null)
                        {
                            CashSaleDetails.ObjectState = ObjectState.Deleted;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Exited_From_UpdateCashsaleDetail_Method);
        }

        private void FillCashSaleDetail(CashSaleDetail cashCaleNew, CashSaleDetailModel detail, decimal? exchangeRate)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_Into_Fill_Method_Of_Fill_CashSaleDeatil_Method);
            cashCaleNew.AllowDisAllow = detail.AllowDisAllow;
            cashCaleNew.AmtCurrency = detail.AmtCurrency;
            cashCaleNew.DocAmount = detail.DocAmount;
            cashCaleNew.DocTaxAmount = detail.DocTaxAmount;
            cashCaleNew.DocTotalAmount = detail.DocTotalAmount;
            cashCaleNew.TaxCurrency = detail.TaxCurrency;
            cashCaleNew.TaxId = detail.TaxId;
            cashCaleNew.TaxRate = detail.TaxRate;
            cashCaleNew.Unit = detail.Unit;
            cashCaleNew.UnitPrice = detail.UnitPrice;
            cashCaleNew.BaseAmount = exchangeRate != null ? Math.Round(cashCaleNew.DocAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : cashCaleNew.DocAmount;
            cashCaleNew.BaseTaxAmount = exchangeRate != null ? cashCaleNew.DocTaxAmount != null ? Math.Round((decimal)cashCaleNew.DocTaxAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : cashCaleNew.DocTaxAmount : cashCaleNew.DocTaxAmount;
            cashCaleNew.BaseTotalAmount = Math.Round((decimal)cashCaleNew.BaseAmount + (cashCaleNew.BaseTaxAmount != null ? (decimal)cashCaleNew.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
            cashCaleNew.CashSaleId = detail.CashSaleId;
            cashCaleNew.COAId = detail.COAId;
            cashCaleNew.Discount = detail.Discount;
            cashCaleNew.DiscountType = detail.DiscountType;
            cashCaleNew.Id = detail.Id;
            cashCaleNew.ItemDescription = detail.ItemDescription;
            cashCaleNew.ItemId = detail.ItemId;
            cashCaleNew.Qty = detail.Qty;
            cashCaleNew.IsPLAccount = detail.IsPLAccount;
            cashCaleNew.TaxIdCode = detail.TaxIdCode;
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Exited_From_FillCashSaleDetail_Method);
        }

        private void FillCashSalesDetails(CashSaleDetailModel cDetailNew, CashSaleDetail cashSalesDetail, decimal? exchangeRate)
        {
            //cashSalesDetail.CashSaleId = cDetailNew.Id;
            cashSalesDetail.ItemId = cDetailNew.ItemId;
            //cashSalesDetail.ItemCode = cDetailNew.ItemCode;
            cashSalesDetail.ItemDescription = cDetailNew.ItemDescription;
            cashSalesDetail.Qty = cDetailNew.Qty;
            cashSalesDetail.Unit = cDetailNew.Unit;
            cashSalesDetail.UnitPrice = cDetailNew.UnitPrice;
            cashSalesDetail.DiscountType = cDetailNew.DiscountType;
            cashSalesDetail.Discount = cDetailNew.Discount;
            cashSalesDetail.COAId = cDetailNew.COAId;
            cashSalesDetail.AllowDisAllow = cDetailNew.AllowDisAllow;
            cashSalesDetail.TaxId = cDetailNew.TaxId;
            TaxCode taxcode = _taxCodeService.Query(a => a.Id == cashSalesDetail.TaxId).Select().FirstOrDefault();
            cashSalesDetail.TaxRate = cDetailNew.TaxRate;
            cashSalesDetail.DocTaxAmount = cDetailNew.DocTaxAmount;
            cashSalesDetail.TaxCurrency = cDetailNew.TaxCurrency;
            cashSalesDetail.DocAmount = cDetailNew.DocAmount;
            cashSalesDetail.AmtCurrency = cDetailNew.AmtCurrency;
            cashSalesDetail.DocTotalAmount = cDetailNew.DocTotalAmount;
            //cashSalesDetail.Remarks = cDetailNew.Remarks;

            //cashSalesDetail.BaseAmount = cDetailNew.BaseAmount;
            //cashSalesDetail.BaseTaxAmount = cDetailNew.BaseTaxAmount;
            //cashSalesDetail.BaseTotalAmount = cDetailNew.BaseTotalAmount;

            cashSalesDetail.BaseAmount = exchangeRate != null ? Math.Round(cashSalesDetail.DocAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : cDetailNew.DocAmount;
            cashSalesDetail.BaseTaxAmount = (cashSalesDetail.TaxRate != null && cashSalesDetail.TaxRate != 0) ? Math.Round((decimal)cashSalesDetail.BaseAmount * (decimal)cashSalesDetail.TaxRate / 100, 2, MidpointRounding.AwayFromZero) : 0;
            cashSalesDetail.BaseTotalAmount = Math.Round((decimal)cashSalesDetail.BaseAmount + (decimal)cashSalesDetail.BaseTaxAmount, 2, MidpointRounding.AwayFromZero);

            cashSalesDetail.IsPLAccount = cDetailNew.IsPLAccount;
            cashSalesDetail.TaxIdCode = cDetailNew.TaxIdCode;
        }

        private void InsertCashSale(CashSaleModel TObject, CashSale cashsalenew)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entred_into_UpdateCashSaleDetail_method_and_checking_the_conditions);
            try
            {
                //cashsalenew.AllocatedAmount = TObject.AllocatedAmount;
                cashsalenew.BalanceAmount = TObject.BalanceAmount;
                cashsalenew.BankClearingDate = TObject.BankClearingDate;
                cashsalenew.CashSaleNumber = TObject.CashSaleNumber;
                cashsalenew.CompanyId = TObject.CompanyId;
                cashsalenew.CreatedDate = TObject.CreatedDate;
                cashsalenew.COAId = TObject.COAId;
                cashsalenew.DocDate = TObject.DocDate;
                //cashsalenew.DocNo = TObject.DocNo;
                cashsalenew.DocSubType = "General";
                cashsalenew.DocType = DocTypeConstants.CashSale;
                //cashsalenew.DocumentState = "Fully Paid";
                cashsalenew.DocumentState = "Posted";
                cashsalenew.EntityId = TObject.EntityId;
                cashsalenew.EntityType = "Customer";
                cashsalenew.ExchangeRate = TObject.ExchangeRate == null ? 1 : TObject.ExchangeRate;
                cashsalenew.ExDurationFrom = TObject.ExDurationFrom;
                cashsalenew.ExDurationTo = TObject.ExDurationTo;
                cashsalenew.ExCurrency = TObject.BaseCurrency;
                cashsalenew.ModeOfReceipt = TObject.ModeOfReceipt;
                cashsalenew.ReceiptrefNo = TObject.ReceiptrefNo;
                cashsalenew.DocDescription = TObject.DocDescription;

                cashsalenew.GrandTotal = TObject.GrandTotal;
                cashsalenew.GSTExCurrency = TObject.GSTExCurrency;
                cashsalenew.GSTTotalAmount = TObject.GSTTotalAmount;
                cashsalenew.Id = TObject.Id;
                cashsalenew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;
                cashsalenew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                cashsalenew.IsGSTApplied = TObject.IsGSTApplied;
                cashsalenew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                cashsalenew.IsGstSettings = TObject.IsGstSettings;
                if (TObject.IsGstSettings)
                {
                    cashsalenew.GSTExchangeRate = TObject.GSTExchangeRate;

                    cashsalenew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    cashsalenew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(cashsalenew.CompanyId);
                if (TObject.IsMultiCurrency == true)
                    cashsalenew.DocCurrency = TObject.DocCurrency;
                else
                    cashsalenew.DocCurrency = financSettings.BaseCurrency;
                cashsalenew.IsMultiCurrency = TObject.IsMultiCurrency;
                cashsalenew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                cashsalenew.ModifiedBy = TObject.ModifiedBy;
                cashsalenew.ModifiedDate = TObject.ModifiedDate;
                cashsalenew.NoSupportingDocs = TObject.NoSupportingDocument;
                cashsalenew.PONo = TObject.PONo;
                cashsalenew.RecOrder = TObject.RecOrder;
                //cashsalenew.Remarks = TObject.Remarks;
                //if (TObject.IsSegmentActive1 != null || TObject.IsSegmentActive1 == true)
                //{
                //    cashsalenew.SegmentMasterid1 = TObject.SegmentMasterid1;
                //    cashsalenew.SegmentDetailid1 = TObject.SegmentDetailid1;
                //    cashsalenew.SegmentCategory1 = TObject.SegmentCategory1;
                //}
                //if (TObject.IsSegmentActive2 != null || TObject.IsSegmentActive2 == true)
                //{
                //    cashsalenew.SegmentMasterid2 = TObject.SegmentMasterid2;
                //    cashsalenew.SegmentDetailid2 = TObject.SegmentDetailid2;
                //    cashsalenew.SegmentCategory2 = TObject.SegmentCategory2;
                //}
                //cashsalenew.IsSegmentReporting = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, TObject.CompanyId);
                //if (cashsalenew.IsSegmentReporting)
                //{
                //    if (TObject.IsSegmentActive1 != null)
                //    {
                //        if (TObject.IsSegmentActive1.Value)
                //        {
                //            cashsalenew.SegmentCategory1 = TObject.SegmentCategory1;
                //            cashsalenew.SegmentDetailid1 = TObject.SegmentDetailid1;
                //            cashsalenew.SegmentMasterid1 = TObject.SegmentMasterid1;
                //        }
                //    }
                //    if (TObject.IsSegmentActive2 != null)
                //    {
                //        if (TObject.IsSegmentActive2.Value)
                //        {
                //            cashsalenew.SegmentCategory2 = TObject.SegmentCategory2;
                //            cashsalenew.SegmentDetailid2 = TObject.SegmentDetailid2;
                //            cashsalenew.SegmentMasterid2 = TObject.SegmentMasterid2;
                //        }
                //    }
                //}
                //else
                //{
                //    cashsalenew.SegmentCategory1 = null;
                //    cashsalenew.SegmentDetailid1 = null;
                //    cashsalenew.SegmentMasterid1 = null;
                //    cashsalenew.SegmentCategory2 = null;
                //    cashsalenew.SegmentDetailid2 = null;
                //    cashsalenew.SegmentMasterid2 = null;
                //}

                cashsalenew.ServiceCompanyId = TObject.ServiceCompanyId;
                cashsalenew.Status = TObject.Status;
                cashsalenew.UserCreated = TObject.UserCreated;

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Exited_From_InsertCashSale_Methos);
        }

        #endregion

        //#region FillAutoNumber
        //private string GetCashSaleAutoNumber(long companyId, string Type, List<string> lstCashSale, string companyCode, string serviceGroupCode)
        //{
        //    string autonumber = null;
        //    try
        //    {
        //        LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService,CashSaleLoggingValidation.Entred_From_Cashsale_AutoNumber_Method);
        //        string url = System.Configuration.ConfigurationManager.AppSettings["IdentityBean"].ToString();
        //        AutoNumberViewModel autoNumbreModel = new AutoNumberViewModel();
        //        autoNumbreModel = FillAutoNumberModel(companyId, Type, lstCashSale, companyCode, serviceGroupCode);
        //        var json = RestHelper.ConvertObjectToJason(autoNumbreModel);
        //        var response = RestHelper.ZPost(url, "api/common/generateautonumberfortype", json);
        //        if (response.ErrorMessage != null)
        //        {
        //            //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
        //        }
        //        autonumber = response.Content;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Log.Logger.ZCritical(ex.StackTrace);
        //        var message = ex.Message;
        //    }
        //    return autonumber;
        //    LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService,CashSaleLoggingValidation.Exited_From_Cashsale_AutoNumber);
        //}
        //private AutoNumberViewModel FillAutoNumberModel(long companyId, string Type, List<string> lstSystemRefNos, string companyCode, string serviceGroupCode)
        //{
        //    AutoNumberViewModel autoNumebrModel = new AutoNumberViewModel();
        //    autoNumebrModel.SystemRefNo = lstSystemRefNos;
        //    autoNumebrModel.Type = Type;
        //    autoNumebrModel.CompanyId = companyId;
        //    autoNumebrModel.CompanyCode = companyCode;
        //    autoNumebrModel.ServiceGroupCode = serviceGroupCode;

        //    return autoNumebrModel;
        //}
        //#endregion

        #region GenerateAutoNumberForType
        string value = "";
        public string GenerateAutoNumberForType(long companyId, string Type, string DocSubType)
        {

            AppsWorld.CashSalesModule.Entities.Models.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
            string generatedAutoNumber = "";
            try
            {
                if (Type == DocTypeConstants.CashSale)
                {
                    generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                        _autoNo.GeneratedNumber, companyId, _autoNo, DocSubType);

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
                        AppsWorld.CashSalesModule.Entities.Models.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                        _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                    }
                    else
                    {
                        AppsWorld.CashSalesModule.Entities.Models.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.CashSalesModule.Entities.Models.AutoNumberCompany();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.Id = Guid.NewGuid();
                        _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                        _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                throw ex;
            }
            return generatedAutoNumber;
        }
        #endregion GenerateAutoNumberForType

        #region GenerateFromFormat
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
         long companyId, AppsWorld.CashSalesModule.Entities.Models.AutoNumber autonumber, string Companycode = null)
        {
            List<CashSale> lstCashsale = null;
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
            if (Type == DocTypeConstants.CashSale)
            {
                lstCashsale = _cashSaleService.GetAllCashSale(companyId);

                if (lstCashsale.Any() && ifMonthContains)
                {
                    //AppsWorld.CashSalesModule.Entities.Models.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastCretedDate = lstCashsale.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstCashsale.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (lastCretedDate == currentMonth)
                        {
                            foreach (var bill in lstCashsale)
                            {
                                if (bill.CashSaleNumber != autonumber.Preview)
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

                else if (lstCashsale.Any() && ifMonthContains == false)
                {
                    //AppsWorld.CashSalesModule.Entities.Models.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var bill in lstCashsale)
                    {
                        if (bill.CashSaleNumber != autonumber.Preview)
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

            if (lstCashsale.Any())
            {
                OutputNumber = GetNewNumber(lstCashsale, Type, OutputNumber, counter, companyFormatHere, counterLength);
            }

            return OutputNumber;
        }
        private string GetNewNumber(List<CashSale> lstCashsale, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstCashsale.Where(a => a.CashSaleNumber == outputNumber).FirstOrDefault();
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
                    var inv = lstCashsale.Where(c => c.CashSaleNumber == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
                //value = counter = val1;
            }
            return val1;
        }
        #endregion GenerateFromFormat

        #region Posting

        private void FillJournal(JVModel headJournal, CashSale _cashSale, bool isNew, string type)
        {

            string strServiceCompany = _companyService.Query(a => a.Id == _cashSale.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();

            //JournalModel headJournal = new JournalModel();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _cashSale.Id;
            FillJv(headJournal, _cashSale);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            //jModel.PostingDate = _cashSale.PostingDate;
            foreach (CashSaleDetail detail in _cashSale.CashSaleDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillJvDetail(_cashSale, journal, detail);
                journal.RecOrder = detail.RecOrder;
                lstJD.Add(journal);
                //journal.BaseCreditTotal = detail.BaseAmount == null ? 0 : detail.BaseAmount.Value;
            }

            if (_cashSale.IsGstSettings)
            {
                ChartOfAccount gstAccount = _chartOfAccountRepository.Query(c => c.Name == COANameConstants.TaxPayableGST && c.CompanyId == _cashSale.CompanyId).Select().FirstOrDefault();

                foreach (CashSaleDetail detail in _cashSale.CashSaleDetails.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    FillJvGstDetail(_cashSale, journal, detail);
                    //journal.RecOrder = recOrder + 1;
                    //recOrder = journal.RecOrder;
                    journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(journal);
                }
            }
            FillJvInDetail(_cashSale, jModel);
            jModel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            jModel.AccountDescription = _cashSale.DocDescription;
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0).OrderBy(c => c.RecOrder).ToList();
        }

        private void FillJvGstDetail(CashSale _cashSale, JVVDetailModel journal, CashSaleDetail detail)
        {
            ChartOfAccount account;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = _cashSale.Id;
            journal.PostingDate = _cashSale.DocDate;
            //journal.Nature = _cashSale.Nature;
            journal.ServiceCompanyId = _cashSale.ServiceCompanyId.Value;
            journal.DocNo = _cashSale.DocNo;
            journal.DocType = _cashSale.DocType;
            journal.DocSubType = _cashSale.DocSubType;
            account = _chartOfAccountRepository.Query(a => a.CompanyId == _cashSale.CompanyId && a.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();
            if (account != null)
                journal.COAId = account.Id;
            //journal.AccountCode = account.Code;
            journal.AccountName = account.Name;
            journal.DocCurrency = _cashSale.DocCurrency;
            journal.BaseCurrency = _cashSale.ExCurrency;
            journal.ExchangeRate = _cashSale.ExchangeRate;
            journal.GSTExCurrency = _cashSale.GSTExCurrency;
            journal.GSTExchangeRate = _cashSale.GSTExchangeRate;
            //journal.AccountDescription = detail.ItemDescription;
            journal.AccountDescription = _cashSale.DocDescription;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCode(detail.TaxId.Value);
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxRate = detail.TaxRate;
            }
            journal.DocCredit = detail.DocTaxAmount.Value;
            journal.BaseCredit = Math.Round((decimal)_cashSale.ExchangeRate == null
                ? journal.DocCredit.Value
                : (journal.DocCredit.Value * _cashSale.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            journal.IsTax = true;
        }

        private void FillJvDetail(CashSale _cashSale, JVVDetailModel journal, CashSaleDetail detail)
        {
            //ChartOfAccount account;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = _cashSale.Id;
            journal.DocNo = _cashSale.DocNo;
            journal.DocType = _cashSale.DocType;
            journal.SystemRefNo = _cashSale.CashSaleNumber;
            journal.DocSubType = _cashSale.DocSubType;
            //journal.DocSubType = DocTypeConstants.CashSale;
            //account = _chartOfAccountRepository.Query(a => a.Id == detail.COAId).Select().FirstOrDefault();
            //journal.COAId = account.Id;
            journal.COAId = detail.COAId;
            journal.ServiceCompanyId = _cashSale.ServiceCompanyId.Value;
            //journal.AccountCode = account.Code;
            //journal.AccountName = account.Name;
            if (detail.ItemId != null)
            {
                journal.ItemId = detail.ItemId.Value;
                journal.ItemCode = detail.ItemCode;
                journal.AccountDescription = detail.ItemDescription;
            }
            journal.Qty = detail.Qty.Value;
            journal.Unit = detail.Unit;
            journal.UnitPrice = detail.UnitPrice.Value;
            journal.Discount = detail.Discount.Value;
            journal.DiscountType = detail.DiscountType;
            journal.AllowDisAllow = detail.AllowDisAllow;
            journal.DocCurrency = _cashSale.DocCurrency;
            journal.BaseCurrency = _cashSale.ExCurrency;
            journal.ExchangeRate = _cashSale.ExchangeRate;
            journal.GSTExCurrency = _cashSale.GSTExCurrency;
            journal.GSTExchangeRate = _cashSale.GSTExchangeRate;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCode(detail.TaxId.Value);
                //journal.TaxId = tax.Id;
                //journal.TaxCode = tax.Code;
                //journal.TaxRate = tax.TaxRate;
                //journal.TaxType = tax.TaxType;
                journal.TaxId = detail.TaxId;
                journal.TaxRate = detail.TaxRate;
            }
            journal.DocCredit = detail.DocAmount;
            journal.BaseCredit = Math.Round((decimal)(_cashSale.ExchangeRate == null
                ? journal.DocCredit
                : (journal.DocCredit * _cashSale.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
            journal.DocTaxCredit = detail.DocTaxAmount;
            journal.BaseTaxCredit = detail.BaseTaxAmount;
            //else if (type == DocTypeConstants.CreditNote)
            //{
            //    journal.DocDebit = detail.DocAmount;
            //    journal.BaseDebit = detail.BaseAmount;
            //}
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.BaseTaxableAmount = detail.BaseAmount;
            journal.BaseTaxAmount = detail.BaseTaxAmount;
            journal.IsTax = false;
            journal.DocCreditTotal = detail.DocTotalAmount;
            journal.PostingDate = _cashSale.DocDate;
        }

        private static void FillJvInDetail(CashSale _cashSale, JVVDetailModel jModel)
        {
            jModel.DocumentId = _cashSale.Id;
            jModel.SystemReferenceNo = _cashSale.CashSaleNumber;
            jModel.SystemRefNo = _cashSale.CashSaleNumber;
            jModel.DocNo = _cashSale.DocNo;
            jModel.ServiceCompanyId = _cashSale.ServiceCompanyId.Value;
            //jModel.Nature = _cashSale.Nature;
            jModel.DocSubType = _cashSale.DocSubType;
            jModel.DocType = DocTypeConstants.CashSale;
            jModel.COAId = _cashSale.COAId;
            //jModel.Remarks = _cashSale.Remarks;
            //jModel.PONo = _cashSale.PONo;
            //jModel.CreditTermsId = _cashSale.CreditTermsId;
            //jModel.DueDate = _cashSale.DueDate;
            jModel.AccountDescription = _cashSale.DocDescription;
            jModel.PostingDate = _cashSale.DocDate;
            jModel.EntityId = _cashSale.EntityId;
            jModel.DocCurrency = _cashSale.DocCurrency;
            jModel.BaseCurrency = _cashSale.ExCurrency;
            jModel.ExchangeRate = _cashSale.ExchangeRate;
            jModel.GSTExCurrency = _cashSale.GSTExCurrency;
            jModel.GSTExchangeRate = _cashSale.GSTExchangeRate;
            jModel.DocDebit = _cashSale.GrandTotal;
            //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? jModel.DocDebit.Value : (jModel.DocDebit * jModel.ExchangeRate).Value, 2);
            decimal amount = 0;
            foreach (var detail in _cashSale.CashSaleDetails)
            {
                amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
            }
            jModel.BaseDebit = amount;

            jModel.SegmentCategory1 = _cashSale.SegmentCategory1;
            jModel.SegmentCategory2 = _cashSale.SegmentCategory2;
            jModel.SegmentMasterid1 = _cashSale.SegmentMasterid1;
            jModel.SegmentMasterid2 = _cashSale.SegmentMasterid2;
            jModel.SegmentDetailid1 = _cashSale.SegmentDetailid1;
            jModel.SegmentDetailid2 = _cashSale.SegmentDetailid2;
            jModel.BaseAmount = _cashSale.BalanceAmount;
            jModel.DocDate = _cashSale.DocDate;
        }

        private void FillJv(JVModel headJournal, CashSale _cashSale)
        {
            headJournal.DocumentId = _cashSale.Id;
            headJournal.CompanyId = _cashSale.CompanyId;
            headJournal.PostingDate = _cashSale.DocDate;
            headJournal.DocNo = _cashSale.DocNo;
            headJournal.DocumentDescription = _cashSale.DocDescription;
            headJournal.DocType = DocTypeConstants.CashSale;
            headJournal.DocSubType = _cashSale.DocSubType;
            //headJournal.DocSubType = DocTypeConstants.CashSale;
            headJournal.DocDate = _cashSale.DocDate;
            //headJournal.DueDate = _cashSale.DueDate;
            headJournal.DocumentState = _cashSale.DocumentState;
            headJournal.SystemReferenceNo = _cashSale.CashSaleNumber;
            headJournal.ServiceCompanyId = _cashSale.ServiceCompanyId;
            //headJournal.ServiceCompany = strServiceCompany;
            //headJournal.Nature = invoice.Nature;
            headJournal.PoNo = _cashSale.PONo;
            headJournal.ExDurationFrom = _cashSale.ExDurationFrom;
            headJournal.ExDurationTo = _cashSale.ExDurationTo;
            headJournal.IsAllowableNonAllowable = _cashSale.IsAllowableNonAllowable;
            headJournal.GSTExDurationFrom = _cashSale.GSTExDurationFrom;
            headJournal.GSTExDurationTo = _cashSale.GSTExDurationTo;
            //headJournal.CreditTerms = _cashSale;
            //headJournal.BalanceAmount = invoice.BalanceAmount;
            //if (top != null)
            //    headJournal.CreditTerms = (int)top.TOPValue;
            headJournal.IsSegmentReporting = _cashSale.IsSegmentReporting;
            headJournal.SegmentCategory1 = _cashSale.SegmentCategory1;
            headJournal.SegmentCategory2 = _cashSale.SegmentCategory2;
            headJournal.SegmentMasterid1 = _cashSale.SegmentMasterid1;
            headJournal.SegmentMasterid2 = _cashSale.SegmentMasterid2;
            headJournal.SegmentDetailid1 = _cashSale.SegmentDetailid1;
            headJournal.SegmentDetailid2 = _cashSale.SegmentDetailid2;
            headJournal.NoSupportingDocument = _cashSale.NoSupportingDocs;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = _cashSale.EntityId;
            headJournal.EntityType = _cashSale.EntityType;
            headJournal.ModeOfReceipt = _cashSale.ModeOfReceipt;
            //headJournal.IsRepeatingInvoice = _cashSale.IsRepeatingInvoice;
            //headJournal.RepEveryPeriod = _cashSale.RepEveryPeriod;
            //headJournal.RepEveryPeriodNo = _cashSale.RepEveryPeriodNo;
            //headJournal.EndDate = invoice.RepEndDate;
            //ChartOfAccount account =
            //    _chartOfAccountRepository.Query(
            //            a =>
            //                a.Name ==
            //                (_cashSale.CashSaleNumber == "Trade"
            //                    ? COANameConstants.AccountsReceivables
            //                    : COANameConstants.OtherReceivables) && a.CompanyId == _cashSale.CompanyId)
            //        .Select()
            //        .FirstOrDefault();
            headJournal.COAId = _cashSale.COAId;
            //headJournal.AccountCode = account.Code;
            //headJournal.AccountName = account.Name;
            headJournal.DocCurrency = _cashSale.DocCurrency;
            headJournal.BaseCurrency = _cashSale.ExCurrency;
            headJournal.ExchangeRate = _cashSale.ExchangeRate;
            headJournal.IsGstSettings = _cashSale.IsGstSettings;
            headJournal.IsGSTApplied = _cashSale.IsGSTApplied;
            headJournal.IsMultiCurrency = _cashSale.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = _cashSale.IsNoSupportingDocument;
            headJournal.GrandDocDebitTotal = _cashSale.GrandTotal;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(_cashSale.GrandTotal * (_cashSale.ExchangeRate != null ? _cashSale.ExchangeRate : 1)), 2);
            if (_cashSale.IsGstSettings)
            {
                headJournal.GSTExCurrency = _cashSale.GSTExCurrency;
                headJournal.GSTExchangeRate = _cashSale.GSTExchangeRate;
            }

            headJournal.Remarks = _cashSale.Remarks;
            headJournal.UserCreated = _cashSale.UserCreated;
            headJournal.CreatedDate = _cashSale.CreatedDate;
            headJournal.ModifiedBy = _cashSale.ModifiedBy;
            headJournal.ModifiedDate = _cashSale.ModifiedDate;
            headJournal.TransferRefNo = _cashSale.ReceiptrefNo;//added by lokanath
        }

        public void SaveInvoice1(JVModel jvModel)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_into_SaveInvoice1_Method);

            var json = RestSharpHelper.ConvertObjectToJason(jvModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == CommonConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = jvModel;
                //url = "http://localhost:57584/";
                // string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            //var json = RestHelper.ConvertObjectToJason(jvModel);
            //try
            //{
            //    object obj = jvModel;
            //    string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
            //    var response = RestHelper.ZPost(url, "api/journal/saveposting", json);
            //    if (response.ErrorMessage != null)
            //    {
            //        Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
            //    }
            //}
            catch (Exception ex)
            {
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }
        #endregion

        #region VoidPosting
        public void VoidJVPostCashSale(DocumentVoidModel tObject)
        {
            LoggingHelper.LogMessage(CommonConstants.CashSaleApplicationService, CashSaleLoggingValidation.Entered_into_VoidJVPostCashSale_Method);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == CommonConstants.IdentityBean)
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
                LoggingHelper.LogError(CommonConstants.CashSaleApplicationService, ex, ex.Message);

                var message = ex.Message;
            }
        }
        #endregion
    }
}
