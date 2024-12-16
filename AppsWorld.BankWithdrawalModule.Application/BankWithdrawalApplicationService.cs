using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.BankWithdrawalModule.Service;
using AppsWorld.BankWithdrawalModule.Models;
using AppsWorld.BankWithdrawalModule.Entities;
using AppsWorld.CommonModule.Application;
using AppsWorld.CommonModule.Entities;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Models;
using AppsWorld.BankWithdrawalModule.Infra;
using AppsWorld.Framework;
using Ziraff.FrameWork.Logging;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;
using Logger;
using Serilog;
using Repository.Pattern.Infrastructure;
using System.Data.Entity.Validation;
using System.Configuration;
using AppsWorld.CommonModule.Infra;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using System.Data;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Application
{
    public class BankWithdrawalApplicationService
    {
        private readonly IWithdrawalService _withdrawalService;
        private readonly IWithdrawalDetailService _withdrawalDetailService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly ITaxCodeService _taxCodeService;
        private readonly IControlCodeCategoryService _controlCodeCatService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ICompanyService _companyService;
        private readonly IBankWithdrawalModuleUnitOfWorkAsync _unitOfWork;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly AppsWorld.BankWithdrawalModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly AppsWorld.BankWithdrawalModule.Service.IAutoNumberService _autoNumberService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        private readonly CommonApplicationService _commonApplicationService;

        #region Constructor Region
        public BankWithdrawalApplicationService(IWithdrawalService withdrawalService, IWithdrawalDetailService withdrawalDetailService, IBeanEntityService beanEntityService, IFinancialSettingService financialSettingService, ITaxCodeService taxCodeService, IControlCodeCategoryService controlCodeCatService, IChartOfAccountService chartOfAccountService, ICompanyService companyService, IGSTSettingService gstSettingService, IBankWithdrawalModuleUnitOfWorkAsync unitOfWork, IAccountTypeService accountTypeService, /*IBankReconciliationSettingService bankReconciliationService,*/ IEmployeeService employeeService, AppsWorld.BankWithdrawalModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.BankWithdrawalModule.Service.IAutoNumberService autoNumberService, IJournalService journalService, AppsWorld.CommonModule.Service.IAutoNumberService autoService, CommonApplicationService commonApplicationService)
        {
            this._withdrawalService = withdrawalService;
            this._withdrawalDetailService = withdrawalDetailService;
            this._beanEntityService = beanEntityService;
            this._financialSettingService = financialSettingService;
            this._taxCodeService = taxCodeService;
            this._controlCodeCatService = controlCodeCatService;
            this._chartOfAccountService = chartOfAccountService;
            this._companyService = companyService;
            this._unitOfWork = unitOfWork;
            this._accountTypeService = accountTypeService;
            this._employeeService = employeeService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._autoNumberService = autoNumberService;
            this._autoService = autoService;
            this._commonApplicationService = commonApplicationService;
        }
        #endregion

        #region Create and Lookup Call
        public async Task<List<AppsWorld.CommonModule.Infra.LookUpVendor<string>>> GetEntityLU(Guid id, long companyId, string entityType, DateTime? docDate)
        {
            List<AppsWorld.CommonModule.Infra.LookUpVendor<string>> entityLU = new List<AppsWorld.CommonModule.Infra.LookUpVendor<string>>();
            try
            {
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Execute_getetity_lookup);
                Withdrawal withdrawalExists = await _withdrawalService.GetWithdrawalById(id, companyId);
                List<AppsWorld.CommonModule.Entities.BeanEntity> lstBean = null;
                List<Employee> lstemployee = null;
                Guid? EntityId = withdrawalExists?.EntityId ?? Guid.Empty;
                FinancialSetting finance = await _financialSettingService.GetFinancialSettingAsync(companyId);
                
                if (entityType == "Employee")
                {
                    lstemployee = await _employeeService.EntityLookUp(companyId, EntityId);
                    entityLU = lstemployee?.Select(x => new AppsWorld.CommonModule.Infra.LookUpVendor<string>()
                    {
                        Name = x.FirstName,
                        Id = x.Id
                    }).OrderBy(a => a.Name).ToList();
                }
                else if (entityType == "Customer" || entityType == "Vendor")
                {
                    List<TaxCode> lstTaxCode = await _taxCodeService.GetListOfTaxCode(companyId);
                    lstBean =await  _beanEntityService.GetAllEntities(entityType, companyId, EntityId);
                    string code = string.Empty;
                    entityLU = lstBean?.Select(x => new AppsWorld.CommonModule.Infra.LookUpVendor<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        Code = x.VenCurrency == null || x.VenCurrency == String.Empty || x.VenCurrency == "0" ? finance.BaseCurrency : x.VenCurrency,
                        TaxCode = code = lstTaxCode.Where(d => d.Id == x.TaxId).Select(d => d.Code).FirstOrDefault(),
                        COAId = x.COAId,
                        TaxId = x.TaxId != null ? lstTaxCode.Where(c => c.Code == code).Select(c => c.Id).ToList() : new List<long>()

                    }).OrderBy(a => a.Name).ToList();
                }
                else if (entityType == null)
                {
                    List<TaxCode> lstTaxCode = await _taxCodeService.GetListOfTaxCode(companyId);
                    lstBean = await _beanEntityService.GetListOfEntity(companyId, EntityId);
                    string code = string.Empty;
                    entityLU = lstBean?.Select(x => new AppsWorld.CommonModule.Infra.LookUpVendor<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        Code = x.VenCurrency == null || x.VenCurrency == String.Empty || x.VenCurrency == "0" ? finance.BaseCurrency : x.VenCurrency,
                        TaxCode = code = lstTaxCode.Where(d => d.Id == x.TaxId).Select(d => d.Code).FirstOrDefault(),
                        COAId = x.COAId,
                        TaxId = x.TaxId != null ? lstTaxCode.Where(c => c.Code == code).Select(c => c.Id).ToList() : new List<long>()

                    }).OrderBy(a => a.Name).ToList();
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Out_from_getetity_lookup);
            return entityLU;
        }

        public async Task<WithdrawalModelLU> GetAllWithdrawalLUs(Guid withdrawalId, long companyId, string type, string userName)
        {
            WithdrawalModelLU withdrawalLU = new WithdrawalModelLU();
            try
            {
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Execute_withdrawals_lookup_method);
                Withdrawal lastWid = await _withdrawalService.CreateWithdrawal(companyId, type);
                Withdrawal withDrawal = await _withdrawalService.GetWithdrawalByIdAsync(withdrawalId, companyId);
                DateTime date= withDrawal?.DocDate ?? lastWid?.DocDate ?? DateTime.Now;
                var entityTypeLu = await _controlCodeCatService.GetByCategoryCodeCategoryAsync(companyId,
                ControlCodeConstants.Control_codes_EntityType);
                if (type == DocTypeConstants.Deposit || type == DocTypeConstants.Withdrawal)
                {
                    entityTypeLu.Lookups = entityTypeLu.Lookups.TakeWhile(x => x.Name != "Employee").ToList();
                    withdrawalLU.EntityTypeLU = entityTypeLu;
                }
                else
                {
                    entityTypeLu.Lookups = entityTypeLu.Lookups.Where(x => x.Name == "Vendor").ToList();
                    withdrawalLU.EntityTypeLU = entityTypeLu;
                }

               if (withDrawal != null)
                {

                    withdrawalLU.ModeOfWithdrawLU = await _controlCodeCatService.GetByCategoryCodeCategoryAsync(companyId,
                    ControlCodeConstants.Control_codes_ModeOfTransfer, withDrawal.ModeOfWithDrawal);
                }
                else
                {
                    withdrawalLU.ModeOfWithdrawLU = await _controlCodeCatService.GetByCategoryCodeCategoryAsync(companyId,
                    ControlCodeConstants.Control_codes_ModeOfTransfer, string.Empty);
                }

                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<AccountType> accType =await _accountTypeService.GetAllAccounyType(companyId);
                List<AccountType> commonList = null;
                if (type == DocTypeConstants.CashPayment)
                {
                    List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                    commonList = accType.Where(c => !coaName.Contains(c.Name)).ToList();
                    List<COALookup<string>> lstCoas = commonList.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsAllowDisAllow = x.DisAllowable,
                        IsPLAccount = x.Category == "Income Statement" ,
                        Class = x.Class,
                        Status = x.Status,
                        IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                    })).OrderBy(d => d.Name).ToList();
                    withdrawalLU.ChartOfAccountNewLU = lstCoas.OrderBy(s => s.Name).ToList();
                }
                else
                {
                    List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                    commonList = accType.Where(c => !coaName.Contains(c.Name)).ToList();
                    List<COALookup<string>> lstCoas = commonList.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsAllowDisAllow = x.DisAllowable,
                        IsPLAccount = x.Category == "Income Statement",
                        Class = x.Class,
                        Status = x.Status,
                        IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity"),
                    })).OrderBy(d => d.Name).ToList();
                    withdrawalLU.ChartOfAccountNewLU = lstCoas.OrderBy(s => s.Name).ToList();
                }
                List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(companyId, date);
                if (allTaxCodes.Any())
                    withdrawalLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        IsTaxAmountEditable = (x.TaxRate == 0 || x.TaxRate == null),
                        TaxType = x.TaxType,
                        Status = x.Status,
                        IsApplicable = x.IsApplicable,
                        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                    }).OrderBy(c => c.Code).ToList();
                if (withDrawal != null && withDrawal.WithdrawalDetails.Count > 0)
                {
                    List<long> CoaIds = withDrawal.WithdrawalDetails.Select(c => c.COAId).ToList();
                    if (withdrawalLU.ChartOfAccountNewLU.Any())
                        CoaIds = CoaIds.Except(withdrawalLU.ChartOfAccountNewLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = withDrawal.WithdrawalDetails.Select(x => x.TaxId).ToList();
                    if (withdrawalLU.TaxCodeLU != null && withdrawalLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(withdrawalLU.TaxCodeLU.Select(d => d.Id)).ToList();
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
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity")
                        }).OrderBy(d => d.Name)).ToList();
                        withdrawalLU.ChartOfAccountNewLU.AddRange(lstEditCoa);
                    }

                    if (withDrawal.IsGstSettingsActivated && taxIds.Any())
                    {
                        lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            IsTaxAmountEditable = (x.TaxRate == 0 || x.TaxRate == null),
                            TaxType = x.TaxType,
                            Status = x.Status,
                            IsApplicable = x.IsApplicable,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                        }).OrderBy(c => c.Code).ToList();
                        withdrawalLU.TaxCodeLU.AddRange(lstEditTax);
                        withdrawalLU.TaxCodeLU = withdrawalLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }


                }
                List<long> coaIds = new List<long>();
                if (withDrawal != null)
                    coaIds.Add(withDrawal.COAId);
                long comp = withDrawal == null ? 0 : withDrawal.ServiceCompanyId;
                withdrawalLU.SubsideryCompanyLU = await _companyService.ListOfSubsudaryCompanyActiveInactiveAsync(companyId, comp, withdrawalId,  coaIds , userName);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Out_fom_withdrawals_lookup_method);
            return withdrawalLU;
        }

        public async Task<WithdrawalModel> CreateWithdrwal(Guid id, long companyId, string docType, bool isCopy, string connectionString)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Enter_into_create_method_of_withdrawal);
            WithdrawalModel withdrawalModel = new WithdrawalModel();
            FinancialSetting financSettings = await _financialSettingService.GetFinancialSettingAsync(companyId);
            if (financSettings == null)
            {
                throw new InvalidOperationException(CommonConstant.The_Financial_setting_should_be_activated);
            }
            withdrawalModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
            withdrawalModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
            Withdrawal withdrawal = await _withdrawalService.GetWithdrawAsync(id, companyId, docType);
          
            if (withdrawal == null)
            {
                withdrawalModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, docType);
                if (withdrawalModel.IsDocNoEditable == true)
                    withdrawalModel.DocNo = _autoService.GetAutonumber(companyId, docType, connectionString);

              await  FillNewWithdrawalModel(withdrawalModel, financSettings, docType);
            }
            else
            {
               
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Validating_withdrawal_and_going_to_enter_FillWithdrawalModel_method);
                FillWithdrawalModel(withdrawalModel, withdrawal, isCopy);
                withdrawalModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyId, docType);
                if (isCopy)
                    withdrawalModel.DocNo = isCopy && withdrawalModel.IsDocNoEditable == true ? _autoService.GetAutonumber(companyId, docType, connectionString) : null;
                string newdirectiory = _commonApplicationService.StringCharactersReplaceFunction(withdrawal.DocNo);
                withdrawalModel.Path = withdrawal.DocType + "s" + "/" + newdirectiory;
                withdrawalModel.WithdrawalDetailModels = withdrawal.WithdrawalDetails.Select(x => new WithdrawalDetailModel()
                {
                    Id = isCopy ? Guid.NewGuid() : x.Id,
                    WithdrawalId = x.WithdrawalId,
                    TaxId = x.TaxId,
                    TaxRate = x.TaxRate,
                    COAId = x.COAId,
                    TaxIdCode = x.TaxIdCode,
                    AllowDisAllow = x.AllowDisAllow,
                    BaseAmount = x.BaseAmount,
                    BaseTaxAmount = x.BaseTaxAmount,
                    BaseTotalAmount = x.BaseTotalAmount,
                    DocAmount = x.DocAmount,
                    DocTaxAmount = x.DocTaxAmount,
                    Currency = x.Currency,
                    Description = x.Description,
                    DocTotalAmount = x.DocTotalAmount,
                    RecOrder = x.RecOrder,
                    IsPLAccount = x.IsPLAccount,
                    ClearingState = x.ClearingState,
                }).OrderBy(d => d.RecOrder).ToList();

            }

           
            return withdrawalModel;
        }

        #endregion

        #region Save Call
        public Withdrawal SaveWithdrawal(WithdrawalModel TObject, string connectionString)
        {
            bool isAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, "ObjectSave", AdditionalInfo);
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_SaveWithdrawal_method);
                string _errors = CommonValidation.ValidateObject(TObject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }

                if (TObject.DocDate == null)
                {
                    throw new InvalidOperationException(CommonConstant.Invalid_Document_Date);
                }


                //to check if it is void or not
                if (_withdrawalService.IsVoid(TObject.CompanyId, TObject.Id))
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                if (TObject.GrandTotal <= 0)
                    throw new InvalidOperationException(WithdrawalValidation.Doc_Curency_Amount_Should_Be_Grater_Than_0);
                Withdrawal _wothdrawDoc = _withdrawalService.GetWithdrawalDocNo(TObject.Id, TObject.DocNo, TObject.CompanyId, TObject.DocType);
                if (TObject.IsDocNoEditable == true)
                    if (_wothdrawDoc != null)
                    {
                        throw new InvalidOperationException(CommonConstant.Document_number_already_exist);
                    }

                if (TObject.WithdrawalDetailModels == null || TObject.WithdrawalDetailModels.Count == 0)
                {
                    throw new InvalidOperationException(WithdrawalValidation.Atleast_one_Chart_of_Account_is_required_in_the_Withdrawal);
                }
                else
                {
                    int itemCount = TObject.WithdrawalDetailModels.Where(a => a.RecordStatus != "Deleted").Count();
                    if (itemCount == 0)
                    {
                        throw new InvalidOperationException(WithdrawalValidation.Atleast_one_Chart_of_Account_is_required_in_the_Withdrawal);
                    }
                }

                if (TObject.ExchangeRate == 0)
                    throw new InvalidOperationException(CommonConstant.ExchangeRate_Should_Be_Grater_Than_0);
                if (TObject.GstExchangeRate == 0)
                    throw new InvalidOperationException(CommonConstant.GSTExchangeRate_Should_Be_Grater_Than_0);

                //Need to verify the invoice is within Financial year
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.DocDate.Date, TObject.CompanyId))
                {
                    throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new InvalidOperationException(CommonConstant.Invalid_Financial_Period_Lock_Password);
                    }
                }
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Validation_checking_finished);
                Withdrawal _withDraw = _withdrawalService.GetWithdraw(TObject.Id, TObject.CompanyId, TObject.DocType);
                string oldDocumentNo = string.Empty;
                if (_withDraw != null)
                {
                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Validating_withdrawal_entity_in_edit_mode);

                    //Data concurrency verify
                    oldDocumentNo = _withDraw.DocNo;
                    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_withDraw.Version, x => x.ToString("X2")));
                    if (!timeStamp.Equals(TObject.Version))
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Going_to_execute_InsertWithdrawal_method);
                    InsertWithdrawal(TObject, _withDraw);
                    _withDraw.DocNo = TObject.DocNo;
                    _withDraw.SystemRefNo = _withDraw.DocNo;
                    _withDraw.ModifiedBy = TObject.ModifiedBy;
                    _withDraw.ModifiedDate = DateTime.UtcNow;
                    _withDraw.ObjectState = ObjectState.Modified;
                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Going_to_execute_UpdateWithdrwalDetail_method);
                    UpdateWithdrwalDetail(TObject, _withDraw);
                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Going_execute_UpdateWithDrawalGSTDetail_method);
                    _withdrawalService.Update(_withDraw);

                }
                else
                {
                    isAdd = true;
                    int? RecOrder = 0;
                    _withDraw = new Withdrawal();
                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Going_to_execute_InsertWithdrawal_method_in_insert_new_mode);
                    InsertWithdrawal(TObject, _withDraw);
                    _withDraw.DocumentState = WithdrawalState.Posted;
                    _withDraw.Id = Guid.NewGuid();
                    if (TObject.WithdrawalDetailModels.Count > 0 || TObject.WithdrawalDetailModels.Any())
                    {
                        List<WithdrawalDetail> lstWithdrawalDetail = new List<WithdrawalDetail>();
                        decimal? baseAmount = 0;
                        decimal? baseTaxAmount = 0;
                        lstWithdrawalDetail.AddRange(TObject.WithdrawalDetailModels.Select(d => new WithdrawalDetail
                        {
                            Id = Guid.NewGuid(),
                            WithdrawalId = _withDraw.Id,
                            TaxId = d.TaxId,
                            TaxRate = d.TaxRate,
                            COAId = d.COAId,
                            AllowDisAllow = d.AllowDisAllow,
                            DocAmount = d.DocAmount,
                            DocTaxAmount = d.DocTaxAmount,
                            DocTotalAmount = d.DocTotalAmount,
                            BaseAmount = baseAmount = TObject.ExchangeRate != null ? Math.Round(d.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : d.DocAmount,
                            BaseTaxAmount = baseTaxAmount = TObject.ExchangeRate != null && d.DocTaxAmount != null ? Math.Round((decimal)d.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : d.DocTaxAmount,
                            BaseTotalAmount = Math.Round((decimal)(baseAmount + baseTaxAmount), 2, MidpointRounding.AwayFromZero),
                            Currency = d.Currency,
                            Description = d.Description,
                            RecOrder = ++RecOrder,
                            IsPLAccount = d.IsPLAccount,
                            TaxIdCode = d.TaxIdCode,
                            ObjectState = ObjectState.Added
                        }));
                        if (lstWithdrawalDetail.Any())
                            _withdrawalDetailService.InsertRange(lstWithdrawalDetail);
                    }
                    _withDraw.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                    _withDraw.UserCreated = TObject.UserCreated;
                    _withDraw.CreatedDate = DateTime.UtcNow;
                    _withDraw.ObjectState = ObjectState.Added;
                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Executing_auto_number_method);
                    _withDraw.SystemRefNo = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, TObject.DocType, connectionString) : TObject.DocNo;
                    isDocAdd = true;
                    _withDraw.DocNo = _withDraw.SystemRefNo;
                    _withdrawalService.Insert(_withDraw);
                }
                try
                {
                    _unitOfWork.SaveChanges();
                    AppaWorld.Bean.Common.SavePosting(_withDraw.CompanyId, _withDraw.Id, _withDraw.DocType, connectionString);

                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.SaveChanges_method_execution_happened);
                    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Going_to_execute_EventStore_process);

                    #region DocumentAttachment_Save_Call
                    if (isAdd == true)
                    {
                        if (TObject.TileAttachments != null && TObject.TileAttachments.Count() > 0)
                        {
                            string DocNo = _commonApplicationService.StringCharactersReplaceFunction(_withDraw.DocNo);
                            string path = _withDraw.DocType + "s" + "/" + DocNo;
                            SaveTailsAttachments(TObject.CompanyId, path, TObject.UserCreated, TObject.TileAttachments);
                        }
                    }
                    #endregion

                    #region Document Folder rename

                    if (isAdd == false && oldDocumentNo != TObject.DocNo && DocTypeConstants.Withdrawal == TObject.DocType)
                        _commonApplicationService.ChangeFolderName(TObject.CompanyId, TObject.DocNo, oldDocumentNo, "Withdrawals");
                    else if (isAdd == false && oldDocumentNo != TObject.DocNo && DocTypeConstants.Deposit == TObject.DocType)
                        _commonApplicationService.ChangeFolderName(TObject.CompanyId, TObject.DocNo, oldDocumentNo, "Deposits");
                    else if (isAdd == false && oldDocumentNo != TObject.DocNo && DocTypeConstants.CashPayment == TObject.DocType)
                        _commonApplicationService.ChangeFolderName(TObject.CompanyId, TObject.DocNo, oldDocumentNo, "Cash Payments");

                    #endregion
                }
                catch (DbEntityValidationException e)
                {
                    LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, e, e.Message);
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
                    throw new Exception("An error has occurred!Please try after sometimes.");
                }
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.End_of_the_SaveWithdrawal_method);
                return _withDraw;
            }
            catch (Exception ex)
            {
                if (isAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocType, connectionString);
                }
                throw ex;
            }
        }



        public Withdrawal SaveWithdrawalDocumentVoid(DocumentVoidModel TObject)
        {

            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_SaveWithdrawalDocumentVoid_method);
            //to check if it is void or not
            if (_withdrawalService.IsVoid(TObject.CompanyId, TObject.Id))
                throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
            string DocNo = "-V";
            Withdrawal _withDrawal = _withdrawalService.GetWithdraw(TObject.Id, TObject.CompanyId, TObject.DocType);
            if (_withDrawal.WithdrawalDetails.Any(s => s.ClearingState == WithdrawalState.Cleared))
                throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Validating_model_and_proceed_towards_the_functional_validation);
            if (_withDrawal == null)
                throw new Exception(WithdrawalValidation.Invalid_Withdrawal);
            else
            {
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_withDrawal.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            if (_withDrawal.BankClearingDate != null)
                throw new Exception(_withDrawal.DocType + WithdrawalValidation.Bank_ClearingDate_activated);
            //if (_withDrawal.DocumentState != DebitNoteState.NotPaid)
            //    throw new Exception("State should be " + DebitNoteState.NotPaid);

            //Need to verify the withdrawal within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_withDrawal.DocDate, TObject.CompanyId))
            {
                throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Functionality_validation_going_on);
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_withDrawal.DocDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_withDrawal.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.End_of_the_Functionality_validation);
            }
            _withDrawal.DocumentState = WithdrawalState.Void;
            _withDrawal.DocNo = _withDrawal.DocNo + DocNo;
            _withDrawal.ModifiedDate = DateTime.UtcNow;
            _withDrawal.ModifiedBy = TObject.ModifiedBy;
            _withDrawal.ObjectState = ObjectState.Modified;
            try
            {
                _unitOfWork.SaveChanges();
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.SaveChanges_method_execution_happened_in_void_method);
                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocNo = _withDrawal.DocNo;
                tObject.ModifiedBy = TObject.ModifiedBy;
                deleteJVPostWithdrawal(tObject);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, "End of the Doc Void method");
            return _withDrawal;
        }


        #endregion

        #region Kendo Call
        public async Task<IQueryable<BankWithdrawalModelK>> GetAllBankWithdrawalk(string username, long companyId)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Execute_withdrawal_Kendo_call);
            return await _withdrawalService.GetAllBankWithdralK(username, companyId, "Withdrawal"); 
        }
        public async Task<IQueryable<BankWithdrawalModelK>> GetAllDepositK(string username, long companyId)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Execute_deposit_Kendo_call);
            return await  _withdrawalService.GetAllBankWithdralK(username, companyId, "Deposit");
        }
        public async Task<IQueryable<BankWithdrawalModelK>> GetAllCashPaymentK(string username, long companyId)
        {
           
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Execute_cash_paymewnt_Kendo_call);
            return await _withdrawalService.GetAllBankWithdralK(username, companyId, DocTypeConstants.CashPayment);
           
        }
        #endregion

        #region PrivateMethod
        private void FillWithdrawalModel(WithdrawalModel withdrawalModel, Withdrawal withdrawal, bool isCopy)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_FillWithdrawalModel_method);
            withdrawalModel.Id = isCopy ? Guid.NewGuid() : withdrawal.Id;
            withdrawalModel.CompanyId = withdrawal.CompanyId;
            withdrawalModel.BankClearingDate = isCopy ? null : withdrawal.BankClearingDate;
            withdrawalModel.BankWithdrawalAmmount = withdrawal.BankWithdrawalAmmount;
            withdrawalModel.BaseCurrency = withdrawal.ExCurrency;
            withdrawalModel.IsLocked = withdrawal.IsLocked;
            withdrawalModel.COAId = withdrawal.COAId;
            withdrawalModel.DocType = withdrawal.DocType;
            withdrawalModel.Version = "0x" + string.Concat(Array.ConvertAll(withdrawal.Version, x => x.ToString("X2")));
            withdrawalModel.DocCurrency = withdrawal.DocCurrency;
            withdrawalModel.DocDate = withdrawal.DocDate;
            withdrawalModel.DocNo = withdrawal.DocNo;
            withdrawalModel.DocDescription = withdrawal.DocDescription;
            withdrawalModel.EntityId = withdrawal.EntityId;
            withdrawalModel.EntityName = withdrawal.EntityId != null ? _beanEntityService.GetEntityName(withdrawal.EntityId.Value) : null;
            withdrawalModel.ExDurationFrom = withdrawal.ExDurationFrom;
            withdrawalModel.ExDurationTo = withdrawal.ExDurationTo;
            withdrawalModel.GrandTotal = withdrawal.GrandTotal;
            withdrawalModel.GSTExDurationFrom = withdrawal.GSTExDurationFrom;
            withdrawalModel.GSTExDurationTo = withdrawal.GSTExDurationTo;
            withdrawalModel.GstExchangeRate = withdrawal.GSTExchangeRate;
            withdrawalModel.GstReportingCurrency = withdrawal.GSTExCurrency;
            withdrawalModel.GSTTotalAmount = withdrawal.GSTTotalAmount;
            withdrawalModel.IsGSTCurrencyRateChanged = withdrawal.IsBaseCurrencyRateChanged;
            withdrawalModel.IsGSTCurrencyRateChanged = withdrawal.IsGSTCurrencyRateChanged;
            withdrawalModel.EntityType = withdrawal.EntityType;
            withdrawalModel.IsDisAllow = withdrawal.IsDisAllow;
            withdrawalModel.IsGstSettingsActivated = withdrawal.IsGstSettingsActivated;
            withdrawalModel.IsMultiCurrencyActivated = withdrawal.IsMultiCurrencyActivated;
            withdrawalModel.ModeOfWithdrawal = withdrawal.ModeOfWithDrawal;
            withdrawalModel.NoSupportingDocument = withdrawal.NoSupportingDocs;
            withdrawalModel.WithdrawalRefNo = withdrawal.WithDrawalRefNo;
            withdrawalModel.ServiceCompanyId = withdrawal.ServiceCompanyId;
            withdrawalModel.IsBaseCurrencyRateChanged = withdrawal.IsBaseCurrencyRateChanged;
            withdrawalModel.SystemRefNo = withdrawal.SystemRefNo;
            withdrawalModel.UserCreated = isCopy ? null : withdrawal.UserCreated;
            withdrawalModel.CreatedDate = isCopy ? null : withdrawal.CreatedDate;
            withdrawalModel.ModifiedBy = isCopy ? null : withdrawal.ModifiedBy;
            withdrawalModel.ModifiedDate = isCopy ? null : withdrawal.ModifiedDate;
            withdrawalModel.ExchangeRate = withdrawal.ExchangeRate;
            withdrawalModel.DocumentState = isCopy ? null : withdrawal.DocumentState;
            withdrawalModel.Status = withdrawal.Status;
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Exited_from_FillWithdrawalModel_method);
        }
        private async Task FillNewWithdrawalModel(WithdrawalModel wModel, FinancialSetting finSetting, string docType)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_FillNewWithdrawalModel_method);
            long companyId = finSetting.CompanyId;
            Withdrawal lastWithdrawal = await  _withdrawalService.CreateWithdrawal(companyId, docType);
            wModel.Id = Guid.NewGuid();
            wModel.CompanyId = companyId;
            wModel.DocDate = lastWithdrawal == null ? DateTime.Now : lastWithdrawal.DocDate;
           
            wModel.NoSupportingDocument = false;
         
            wModel.CreatedDate = DateTime.UtcNow;
            wModel.BaseCurrency = finSetting.BaseCurrency;
            wModel.DocType = docType;
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Exited_from_FillNewWithdrawalModel_method);
        }

     
  
        private void InsertWithdrawal(WithdrawalModel TObject, Withdrawal withDrawalNew)
        {
            try
            {
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_InsertWithdrawal_method);
                withDrawalNew.CompanyId = TObject.CompanyId;
                if (TObject.DocType == DocTypeConstants.Withdrawal)
                    withDrawalNew.DocType = DocTypeConstants.Withdrawal;
                if (TObject.DocType == DocTypeConstants.Deposit)
                    withDrawalNew.DocType = DocTypeConstants.Deposit;
                if (TObject.DocType == DocTypeConstants.CashPayment)
                    withDrawalNew.DocType = DocTypeConstants.CashPayment;
                withDrawalNew.DocDate = TObject.DocDate.Date;
                withDrawalNew.EntityType = TObject.EntityType;
                //withDrawalNew.DueDate = TObject.DueDate.Date;
                withDrawalNew.ModeOfWithDrawal = TObject.ModeOfWithdrawal;
                //withDrawalNew.DocNo = TObject.DocNo;
                if (TObject.EntityId == new Guid())
                    withDrawalNew.EntityId = null;
                else
                    withDrawalNew.EntityId = TObject.EntityId;
                withDrawalNew.ServiceCompanyId = TObject.ServiceCompanyId;
                withDrawalNew.DocCurrency = TObject.DocCurrency;
                withDrawalNew.DocDescription = TObject.DocDescription;
                withDrawalNew.BankClearingDate = TObject.BankClearingDate;
                withDrawalNew.IsMultiCurrencyActivated = TObject.IsMultiCurrencyActivated;
                withDrawalNew.ExCurrency = TObject.BaseCurrency;
                withDrawalNew.WithDrawalRefNo = TObject.WithdrawalRefNo;
                withDrawalNew.ExchangeRate = TObject.ExchangeRate == null ? 1 : TObject.ExchangeRate;
                withDrawalNew.COAId = TObject.COAId;
                withDrawalNew.ExDurationFrom = TObject.ExDurationFrom;
                withDrawalNew.ExDurationTo = TObject.ExDurationTo;
                withDrawalNew.BankWithdrawalAmmount = TObject.BankWithdrawalAmmount;
                //withDrawalNew.IsGSTApplied = TObject.IsGSTApplied;
                //debitnoteNew.IsGstSettings = _gstSettingService.IsGSTSettingActivated(TObject.CompanyId) && !_gstSettingService.IsGSTDeregistered(TObject.CompanyId);
                withDrawalNew.IsGstSettingsActivated = TObject.IsGstSettingsActivated;
                withDrawalNew.GSTExCurrency = TObject.GstReportingCurrency;
                if (TObject.IsGstSettingsActivated)
                {
                    withDrawalNew.GSTExchangeRate = TObject.GstExchangeRate;
                    withDrawalNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    withDrawalNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }
                withDrawalNew.GrandTotal = TObject.GrandTotal;
                withDrawalNew.GSTTotalAmount = TObject.GSTTotalAmount;

                withDrawalNew.IsAllowableNonAllowableActivated = TObject.IsAllowableNonAllowableActivated;
                //withDrawalNew.IsSegmentReportingActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, TObject.CompanyId);
                //if (TObject.IsSegmentActive1 != null || TObject.IsSegmentActive1 == true)
                //{
                //    withDrawalNew.SegmentMasterid1 = TObject.SegmentMasterid1;
                //    withDrawalNew.SegmentDetailid1 = TObject.SegmentDetailid1;
                //    withDrawalNew.SegmentCategory1 = TObject.SegmentCategory1;
                //}
                //if (TObject.IsSegmentActive2 != null || TObject.IsSegmentActive2 == true)
                //{
                //    withDrawalNew.SegmentMasterid2 = TObject.SegmentMasterid2;
                //    withDrawalNew.SegmentDetailid2 = TObject.SegmentDetailid2;
                //    withDrawalNew.SegmentCategory2 = TObject.SegmentCategory2;
                //}
                withDrawalNew.IsNoSupportingDocumentActivated = TObject.IsNoSupportingDocument;
                //withDrawalNew.NoSupportingDocs = withDrawalNew.IsNoSupportingDocumentActivated.Value ? TObject.NoSupportingDocument : null;
                withDrawalNew.NoSupportingDocs = TObject.NoSupportingDocument;

                //withDrawalNew.IsSegmentReportingActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.SegmentReporting, TObject.CompanyId);
                //if (withDrawalNew.IsSegmentReportingActivated)
                //{
                //    withDrawalNew.SegmentCategory1 = TObject.SegmentCategory1;
                //    withDrawalNew.SegmentCategory2 = TObject.SegmentCategory2;
                //}
                //else
                //{
                //    withDrawalNew.SegmentCategory1 = null;
                //    withDrawalNew.SegmentCategory2 = null;
                //}

                withDrawalNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                withDrawalNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                withDrawalNew.Status = TObject.Status;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Exited_from_InsertWithdrawal_method);
        }
        private void UpdateWithdrwalDetail(WithdrawalModel TObject, Withdrawal _withDrawNew)
        {
            try
            {
                int? Recorder = 0;
                JournalSaveModel postingDetail = new JournalSaveModel();
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_UpdateWithdrwalDetail_method_and_checking_the_conditions);
                foreach (WithdrawalDetailModel detail in TObject.WithdrawalDetailModels)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        WithdrawalDetail WDetail = new WithdrawalDetail();
                        FillWithdrawalDetail(WDetail, detail, TObject.ExchangeRate);
                        WDetail.RecOrder = Recorder + 1;
                        Recorder = WDetail.RecOrder;
                        WDetail.Id = Guid.NewGuid();
                        WDetail.WithdrawalId = TObject.Id;
                        WDetail.ObjectState = ObjectState.Added;
                        _withdrawalDetailService.Insert(WDetail);
                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        WithdrawalDetail withdrawalDetail = _withdrawalDetailService.GetWithDrawal(detail.Id);
                        if (withdrawalDetail != null)
                        {
                            FillWithdrawalDetail(withdrawalDetail, detail, TObject.ExchangeRate);
                            withdrawalDetail.RecOrder = Recorder + 1;
                            Recorder = withdrawalDetail.RecOrder;
                            withdrawalDetail.ObjectState = ObjectState.Modified;
                            _withdrawalDetailService.Update(withdrawalDetail);
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        WithdrawalDetail withdrawalDetail = _withdrawalDetailService.GetWithDrawal(detail.Id);
                        if (withdrawalDetail != null)
                            withdrawalDetail.ObjectState = ObjectState.Deleted;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Exited_from_UpdateWithdrwalDetail_method);
        }
        private void FillWithdrawalDetail(WithdrawalDetail wDetailNew, WithdrawalDetailModel TObject, decimal? exchangeRate)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_FillWithdrawalDetail_private_method);

            wDetailNew.TaxId = TObject.TaxId;
            wDetailNew.TaxRate = TObject.TaxRate;
            wDetailNew.COAId = TObject.COAId;
            wDetailNew.AllowDisAllow = TObject.AllowDisAllow;

            wDetailNew.DocAmount = TObject.DocAmount;
            wDetailNew.DocTaxAmount = TObject.DocTaxAmount;
            wDetailNew.DocTotalAmount = TObject.DocTotalAmount;
            wDetailNew.BaseAmount = exchangeRate != null ? Math.Round(wDetailNew.DocAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : wDetailNew.DocAmount;
            wDetailNew.BaseTaxAmount = exchangeRate != null ? wDetailNew.DocTaxAmount != null ? Math.Round((decimal)wDetailNew.DocTaxAmount * (decimal)exchangeRate, 2, MidpointRounding.AwayFromZero) : wDetailNew.DocTaxAmount : wDetailNew.DocTaxAmount;
            wDetailNew.BaseTotalAmount = Math.Round((decimal)wDetailNew.BaseAmount + (wDetailNew.BaseTaxAmount != null ? (decimal)wDetailNew.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
            wDetailNew.Currency = TObject.Currency;
            wDetailNew.Description = TObject.Description;

            wDetailNew.IsPLAccount = TObject.IsPLAccount;
            wDetailNew.TaxIdCode = TObject.TaxIdCode;
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Exited_from_FillWithdrawalDetail_private_method);
        }
        //private void UpdateWithDrawalGSTDetail(WithdrawalModel TObject, Withdrawal withdrawalNew)
        //{
        //    try
        //    {
        //        LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService,WithdrawalLoggingValidation.Entered_into_UpdateWithDrawalGSTDetail_private_method);
        //        foreach (GSTDetailModel gstDModel in TObject.GSTDetailModels)
        //        {
        //            if (gstDModel.RecordStatus == "Added")
        //            {
        //                GSTDetail gstDetail = new GSTDetail();
        //                FillGstDetail(gstDetail, gstDModel);
        //                gstDetail.Id = Guid.NewGuid();
        //                gstDetail.ObjectState = ObjectState.Added;
        //                _gstDetailService.Insert(gstDetail);
        //            }
        //            else if (gstDModel.RecordStatus == "Added" && gstDModel.RecordStatus == "Deleted")
        //            {
        //                GSTDetail gstDetail = _gstDetailService.GetGSTById(gstDModel.Id);
        //                if (gstDetail != null)
        //                {
        //                    FillGstDetail(gstDetail, gstDModel);
        //                    gstDetail.ObjectState = ObjectState.Modified;
        //                    _gstDetailService.Update(gstDetail);
        //                }
        //            }
        //            else if (gstDModel.RecordStatus == "Deleted")
        //            {
        //                GSTDetail gstDetail = _gstDetailService.GetGSTById(gstDModel.Id);
        //                if (gstDetail != null)
        //                    gstDetail.ObjectState = ObjectState.Deleted;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService,WithdrawalLoggingValidation.Exited_from_UpdateWithDrawalGSTDetail_method);
        //}
        private void FillGstDetail(GSTDetail detail, GSTDetailModel detailModel)
        {
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_into_FillGstDetail_private_method);
            detail.Amount = detailModel.Amount;
            detail.DocType = "Withdrawal";
            detail.ModuleMasterId = detailModel.ModuleMasterId;
            detail.TaxAmount = detailModel.TaxAmount;
            detail.TaxId = detailModel.TaxId;
            detail.TotalAmount = detailModel.TotalAmount;
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Exited_from_FillGstDetail_private_method);
        }
        #endregion

        #region AutoNumber Implimentation
        string value = "";

        public string GenerateAutoNumberForType(long companyId, string Type, string companyCode)
        {
            string generatedAutoNumber = "";
            try
            {
                LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Entered_Into_GenerateAutoNumberForType_Of_Payment);
                AppsWorld.BankWithdrawalModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
                if (Type == DocTypeConstants.Withdrawal || Type == DocTypeConstants.Deposit || Type == DocTypeConstants.CashPayment)
                {
                    generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                        _autoNo.GeneratedNumber, companyId, companyCode);

                    if (_autoNo != null)
                    {
                        _autoNo.GeneratedNumber = value;
                        _autoNo.IsDisable = true;
                        _autoNumberService.Update(_autoNo);
                        _autoNo.ObjectState = ObjectState.Modified;
                    }
                    var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
                    if (_autonumberCompany.Any())
                    {
                        AppsWorld.BankWithdrawalModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                        _autoNumberCompanyNew.GeneratedNumber = value;
                        _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                        _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                        _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                    }
                    else
                    {
                        AppsWorld.BankWithdrawalModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.BankWithdrawalModule.Entities.AutoNumberCompany();
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
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService, WithdrawalLoggingValidation.Come_Out_From_GenerateAutoNumberForType_Of_Payment_Method);
            return generatedAutoNumber;
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
            long companyId, string Companycode = null)
        {
            List<Withdrawal> lstWithdrawal = null;
            int? currentMonth = 0;
            bool ifMonthContains = false;

            string OutputNumber = "";
            try
            {
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
                if (Type == DocTypeConstants.Withdrawal || Type == DocTypeConstants.Deposit || Type == DocTypeConstants.CashPayment)
                {
                    lstWithdrawal = _withdrawalService.GetAllPaymentModel(companyId);

                    if (lstWithdrawal.Any() && ifMonthContains)
                    {
                        if (DateTime.Now.Year == lstWithdrawal.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                        {
                            int? lastCreatedMonth = lstWithdrawal.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                            if (currentMonth == lastCreatedMonth)
                            {
                                AppsWorld.BankWithdrawalModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                                foreach (var withdrawal in lstWithdrawal)
                                {
                                    if (withdrawal.SystemRefNo != autonumber.Preview)
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
                    else if (lstWithdrawal.Any() && ifMonthContains == false)
                    {
                        AppsWorld.BankWithdrawalModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        foreach (var withdrawal in lstWithdrawal)
                        {
                            if (withdrawal.SystemRefNo != autonumber.Preview)
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

                if (lstWithdrawal.Any())
                {
                    OutputNumber = GetNewNumber(lstWithdrawal, Type, OutputNumber, counter, companyFormatHere, counterLength);
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
                throw ex;
            }
            return OutputNumber;
        }
        private string GetNewNumber(List<Withdrawal> lstWithdrawal, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstWithdrawal.Where(a => a.SystemRefNo == outputNumber).FirstOrDefault();
            bool isNotexist = false;
            int i = Convert.ToInt32(counter);
            if (invoice != null)
            {
                var lstWT = lstWithdrawal.Where(a => a.SystemRefNo.Contains(format)).ToList();
                if (lstWT.Any())
                {
                    while (isNotexist == false)
                    {
                        i++;
                        string length = i.ToString();
                        value = length.PadLeft(counterLength, '0');
                        val2 = format + value;
                        var inv = lstWT.Where(c => c.SystemRefNo == val2).FirstOrDefault();
                        if (inv == null)
                            isNotexist = true;
                    }
                    val1 = val2;
                }
                val1 = val2;
            }
            return val1;
        }
        #endregion

        #region Posting Block
        public void SaveInvoice(JVModel clientModel)
        {
            //LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService,clientModel);
            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {

                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == WithdrawalConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;
                // string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);

                var message = ex.Message;
            }
        }
        private void FillJournal(JVModel headJournal, Withdrawal withdrawal, bool isNew)
        {
            int count = 1;
            //string strServiceCompany = _companyService.GetIdBy(withdrawal.ServiceCompanyId);
            //JournalModel headJournal = new JournalModel();
            if (isNew)
            {
                headJournal.Id = Guid.NewGuid();
                headJournal.CreatedDate = DateTime.UtcNow;
            }
            else
                headJournal.Id = withdrawal.Id;
            headJournal.DocumentId = withdrawal.Id;
            headJournal.CompanyId = withdrawal.CompanyId;
            headJournal.PostingDate = withdrawal.DocDate;
            headJournal.DocNo = withdrawal.DocNo;
            headJournal.DocType = withdrawal.DocType;
            headJournal.DocSubType = DocTypeConstants.General;
            headJournal.DocDate = withdrawal.DocDate;
            headJournal.DocumentState = withdrawal.DocumentState;
            headJournal.SystemReferenceNo = withdrawal.SystemRefNo;
            headJournal.ServiceCompanyId = withdrawal.ServiceCompanyId;
            //headJournal.ServiceCompany = strServiceCompany;
            headJournal.ExDurationFrom = withdrawal.ExDurationFrom;
            headJournal.ExDurationTo = withdrawal.ExDurationTo;
            headJournal.GSTExDurationFrom = withdrawal.GSTExDurationFrom;
            headJournal.IsGstSettings = withdrawal.IsGstSettingsActivated;
            headJournal.IsGSTApplied = withdrawal.IsGstSettingsActivated;
            headJournal.IsMultiCurrency = withdrawal.IsMultiCurrencyActivated;
            headJournal.IsNoSupportingDocs = withdrawal.IsNoSupportingDocumentActivated;
            headJournal.IsAllowableNonAllowable = withdrawal.IsAllowableNonAllowableActivated;
            headJournal.GSTExDurationTo = withdrawal.GSTExDurationTo;
            headJournal.IsSegmentReporting = withdrawal.IsSegmentReportingActivated;
            //headJournal.SegmentCategory1 = withdrawal.SegmentCategory1;
            //headJournal.SegmentCategory2 = withdrawal.SegmentCategory2;
            //headJournal.SegmentMasterid1 = withdrawal.SegmentMasterid1;
            //headJournal.SegmentMasterid2 = withdrawal.SegmentMasterid2;
            //headJournal.SegmentDetailid1 = withdrawal.SegmentDetailid1;
            //headJournal.SegmentDetailid2 = withdrawal.SegmentDetailid2;
            headJournal.BankClearingDate = withdrawal.BankClearingDate;
            headJournal.NoSupportingDocument = withdrawal.NoSupportingDocs;
            headJournal.TransferRefNo = withdrawal.WithDrawalRefNo;//added by lokanath
            headJournal.Status = RecordStatusEnum.Active;
            if (withdrawal.EntityId != null && withdrawal.EntityId != new Guid())
            {
                headJournal.EntityId = withdrawal.EntityId.Value;
                //AppsWorld.CommonModule.Entities.BeanEntity entity = _beanEntityService.Query(a => a.Id == withdrawal.EntityId).Select().FirstOrDefault();
                //headJournal.EntityName = entity != null ? entity.Name : null;
            }
            headJournal.EntityType = withdrawal.EntityType;
            //ChartOfAccount account = _chartOfAccountService.GetChartOfAccountByName(withdrawal.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, invoice.CompanyId);
            var account1 = _chartOfAccountService.GetChartOfAccountById(withdrawal.COAId);
            headJournal.COAId = account1.Id;
            headJournal.AccountCode = "DBS (SGD)";
            headJournal.AccountName = "DBS (SGD)";
            headJournal.DocCurrency = withdrawal.DocCurrency;
            headJournal.GrandDocDebitTotal = withdrawal.GrandTotal;
            headJournal.GrandBaseDebitTotal = Math.Round(
                (decimal)(withdrawal.GrandTotal * (withdrawal.ExchangeRate != null ? withdrawal.ExchangeRate : 1)), 2);
            headJournal.BaseCurrency = withdrawal.ExCurrency;
            headJournal.ExchangeRate = withdrawal.ExchangeRate;

            if (withdrawal.IsGstSettingsActivated)
            {
                headJournal.GSTExCurrency = withdrawal.GSTExCurrency;
                headJournal.GSTExchangeRate = withdrawal.GSTExchangeRate;
            }
            headJournal.ModeOfReceipt = withdrawal.ModeOfWithDrawal;
            //headJournal.Remarks = withdrawal.DocDescription;
            headJournal.DocumentDescription = withdrawal.DocDescription;
            headJournal.UserCreated = withdrawal.UserCreated;
            headJournal.CreatedDate = withdrawal.CreatedDate;
            headJournal.ModifiedBy = withdrawal.ModifiedBy;
            headJournal.ModifiedDate = withdrawal.ModifiedDate;

            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            int? recOreder = 0;
            JVVDetailModel jModel = new JVVDetailModel();
            jModel.DocumentId = withdrawal.Id;
            jModel.SystemRefNo = withdrawal.SystemRefNo;
            jModel.DocNo = withdrawal.DocNo;
            jModel.DocDate = withdrawal.DocDate;
            jModel.ServiceCompanyId = withdrawal.ServiceCompanyId;
            jModel.PostingDate = withdrawal.DocDate;
            //jModel.Remarks = withdrawal.Remarks;
            //jModel.DocType = withdrawal.DocType == DocTypeConstants.Withdrawal ? DocTypeConstants.Withdrawal : DocTypeConstants.Deposit;
            jModel.DocType = withdrawal.DocType;
            jModel.DocSubType = DocTypeConstants.General;
            jModel.AccountDescription = withdrawal.DocDescription;
            var accoun1t = _chartOfAccountService.GetChartOfAccountById(withdrawal.COAId);
            jModel.COAId = account1.Id;
            jModel.Type = withdrawal.DocType;
            if (withdrawal.EntityId != null && withdrawal.EntityId != new Guid())
                jModel.EntityId = withdrawal.EntityId;
            jModel.DocCurrency = withdrawal.DocCurrency;
            jModel.BaseCurrency = withdrawal.ExCurrency;
            jModel.ExchangeRate = withdrawal.ExchangeRate;
            jModel.GSTExCurrency = withdrawal.GSTExCurrency;
            jModel.GSTExchangeRate = withdrawal.GSTExchangeRate;
            //if (withdrawal.DocType == DocTypeConstants.Deposit)
            //{
            //    jModel.DocDebit = withdrawal.GrandTotal;
            //    jModel.BaseDebit = Math.Round((decimal)(jModel.ExchangeRate == null ? jModel.DocDebit : (jModel.DocDebit * jModel.ExchangeRate)), 2);
            //}
            //else
            //{
            //    jModel.DocCredit = withdrawal.GrandTotal;
            //    jModel.BaseCredit = Math.Round((decimal)(jModel.ExchangeRate == null ? jModel.DocCredit : (jModel.DocCredit * jModel.ExchangeRate)), 2);

            //}
            if (withdrawal.DocType == DocTypeConstants.Deposit)
            {
                jModel.DocDebit = withdrawal.GrandTotal;
                decimal amount = 0;
                foreach (var detail in withdrawal.WithdrawalDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseDebit = amount;
            }
            else
            {
                jModel.DocCredit = withdrawal.GrandTotal;
                decimal amount = 0;
                foreach (var detail in withdrawal.WithdrawalDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseCredit = amount;
            }
            //jModel.IsGstSettings = invoice.IsGstSettings;
            //jModel.IsGSTApplied = invoice.IsGSTApplied;
            //jModel.IsMultiCurrency = invoice.IsMultiCurrency;
            //jModel.IsNoSupportingDocs = invoice.IsNoSupportingDocument;
            //jModel.SegmentCategory1 = withdrawal.SegmentCategory1;
            //jModel.SegmentCategory2 = withdrawal.SegmentCategory2;
            //jModel.SegmentMasterid1 = withdrawal.SegmentMasterid1;
            //jModel.SegmentMasterid2 = withdrawal.SegmentMasterid2;
            //jModel.SegmentDetailid1 = withdrawal.SegmentDetailid1;
            //jModel.SegmentDetailid2 = withdrawal.SegmentDetailid2;
            jModel.RecOrder = recOreder + 1;
            recOreder = jModel.RecOrder;
            AppsWorld.CommonModule.Entities.ChartOfAccount gstAccount = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.TaxPayableGST, withdrawal.CompanyId);
            foreach (var detail in withdrawal.WithdrawalDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                journal.DocumentDetailId = detail.Id;
                journal.DocumentId = withdrawal.Id;
                journal.ServiceCompanyId = withdrawal.ServiceCompanyId;
                journal.DocNo = withdrawal.DocNo;
                //var account = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
                //journal.COAId = account.Id;
                journal.COAId = detail.COAId;
                journal.Type = withdrawal.DocType;
                journal.SystemRefNo = withdrawal.SystemRefNo;
                //journal.DocType = withdrawal.DocType == DocTypeConstants.Withdrawal ? DocTypeConstants.Withdrawal : DocTypeConstants.Deposit;
                journal.DocType = withdrawal.DocType;
                journal.DocSubType = DocTypeConstants.General;
                //journal.AccountCode = account.Code;
                journal.DocCurrency = withdrawal.DocCurrency;
                journal.BaseCurrency = withdrawal.ExCurrency;
                journal.DocDate = withdrawal.DocDate;
                //journal.AccountName = account.Name;
                journal.AllowDisAllow = detail.AllowDisAllow;
                journal.AccountDescription = detail.Description;
                if (withdrawal.IsGstSettingsActivated)
                {
                    if (detail.TaxId != null)
                    {
                        //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                        //journal.TaxId = detail.TaxId;
                        //journal.TaxCode = tax.Code;
                        ////if (journal.TaxCode == "NA")
                        ////    journal.TaxId = null;
                        //journal.TaxRate = tax.TaxRate;
                        //journal.TaxType = tax.TaxType;
                        journal.TaxId = detail.TaxId;
                        journal.TaxRate = detail.TaxRate;
                    }
                }
                //if (detail.DocAmount < 0)
                //{
                //    string value = detail.DocAmount.ToString("C");
                //    detail.DocAmount = Convert.ToDecimal(value);
                //}
                if (detail.DocAmount >= 0)
                {
                    if (withdrawal.DocType == DocTypeConstants.Deposit)
                    {

                        journal.DocCredit = detail.DocAmount;
                        journal.BaseCredit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocCredit : (journal.DocCredit * withdrawal.ExchangeRate)), 2);
                        journal.DocTaxCredit = detail.DocTaxAmount;
                        journal.BaseTaxCredit = detail.BaseTaxAmount;
                    }
                    else
                    {
                        journal.DocDebit = detail.DocAmount;
                        journal.BaseDebit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocDebit : (journal.DocDebit * withdrawal.ExchangeRate)), 2);
                        journal.DocTaxDebit = detail.DocTaxAmount;
                        journal.BaseTaxDebit = detail.BaseTaxAmount;

                    }
                }
                else
                {
                    if (withdrawal.DocType == DocTypeConstants.Deposit)
                    {
                        journal.DocDebit = detail.DocAmount;
                        journal.BaseDebit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocDebit : (journal.DocDebit * withdrawal.ExchangeRate)), 2);
                        journal.DocTaxDebit = detail.DocTaxAmount;
                        journal.BaseTaxDebit = detail.BaseTaxAmount;
                    }
                    else
                    {
                        journal.DocCredit = detail.DocAmount;
                        journal.BaseCredit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocCredit : (journal.DocCredit * withdrawal.ExchangeRate)), 2);
                        journal.DocTaxCredit = detail.DocTaxAmount;
                        journal.BaseTaxCredit = detail.BaseTaxAmount;
                    }
                }
                journal.DocTaxableAmount = detail.DocAmount;
                journal.DocTaxAmount = detail.DocTaxAmount;
                journal.BaseTaxableAmount = detail.BaseAmount;
                journal.BaseTaxAmount = detail.BaseTaxAmount;
                journal.IsTax = false;
                journal.RecOrder = detail.RecOrder;
                journal.PostingDate = withdrawal.DocDate;
                journal.BaseCurrency = withdrawal.ExCurrency;
                //journal.RecOrder = recOreder + 1;
                //recOreder = journal.RecOrder;
                lstJD.Add(journal);
                //journal.BaseCreditTotal = detail.BaseAmount == null ? 0 : detail.BaseAmount.Value;
                //baseTotal += journal.CreditBC.Value;
            }
            if (withdrawal.IsGstSettingsActivated)
            {


                foreach (var detail in withdrawal.WithdrawalDetails.Where(a => a.TaxRate != null && a.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    journal.DocType = withdrawal.DocType;
                    journal.DocSubType = DocTypeConstants.General;
                    //journal.AccountDescription = detail.Description;
                    journal.AccountDescription = withdrawal.DocDescription;
                    journal.DocumentDetailId = detail.Id;
                    journal.DocumentId = withdrawal.Id;
                    journal.ServiceCompanyId = withdrawal.ServiceCompanyId;
                    journal.DocNo = withdrawal.DocNo;
                    journal.PostingDate = withdrawal.DocDate;
                    //var account = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
                    journal.COAId = gstAccount.Id;
                    journal.AccountCode = gstAccount.Code;
                    journal.AccountName = gstAccount.Name;
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
                        //journal.TaxType = detail.TaxType;
                    }
                    //if (detail.DocTaxAmount!= null || detail.DocTaxAmount < 0)
                    //{
                    //    //string value = detail.DocTaxAmount.Value.ToString();
                    //    //CultureInfo usCulture = new CultureInfo("hi-IN");
                    //    //decimal amount = decimal.Parse(value,NumberStyles.Currency, usCulture);
                    //    string value = detail.DocTaxAmount.Value.ToString("C");
                    //    detail.DocTaxAmount = Convert.ToDecimal(value);
                    //}
                    if (detail.DocTaxAmount >= 0)
                    {
                        if (withdrawal.DocType == DocTypeConstants.Deposit)
                        {
                            journal.DocCredit = detail.DocTaxAmount.Value;
                            journal.BaseCredit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocCredit : (journal.DocCredit * withdrawal.ExchangeRate)), 2);
                        }
                        else
                        {
                            journal.DocDebit = detail.DocTaxAmount.Value;
                            journal.BaseDebit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocDebit : (journal.DocDebit * withdrawal.ExchangeRate)), 2);
                        }
                    }
                    else
                    {
                        if (withdrawal.DocType == DocTypeConstants.Deposit)
                        {
                            journal.DocDebit = detail.DocTaxAmount.Value;
                            journal.BaseDebit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocDebit : (journal.DocDebit * withdrawal.ExchangeRate)), 2);
                        }
                        else
                        {
                            journal.DocCredit = detail.DocTaxAmount.Value;
                            journal.BaseCredit = Math.Round((decimal)(withdrawal.ExchangeRate == null ? journal.DocCredit : (journal.DocCredit * withdrawal.ExchangeRate)), 2);
                        }
                    }
                    journal.IsTax = true;
                    journal.RecOrder = detail.RecOrder;
                    journal.BaseCurrency = withdrawal.ExCurrency;
                    if (journal.TaxRate != null || journal.TaxRate > 0 || journal.TaxCode != "NA")
                        lstJD.Add(journal);
                    //journal.CreditDC = detail.DocTaxAmount == null ? 0 : detail.DocTaxAmount.Value;
                    //journal.CreditBC = detail.BaseTaxAmount == null ? 0 : detail.BaseTaxAmount.Value;
                    //baseTotal += journal.CreditBC.Value;
                    //journal.CreditGSTR = Math.Round((decimal)(journal.CreditDC * invoice.GSTExchangeRate), 2);
                }
            }
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD.OrderBy(x => x.RecOrder).ToList();
        }
        //private void FillJournalDetail(JVVDetailModel jModel, Withdrawal withdrawal)
        //{
        //    jModel.COAId = withdrawal.COAId;
        //    jModel.DocumentId = withdrawal.Id;
        //    jModel.SystemReferenceNo = withdrawal.WithDrawalRefNo;
        //    jModel.DocNo = withdrawal.DocNo;
        //    jModel.ServiceCompanyId = withdrawal.ServiceCompanyId;
        //    jModel.DocSubType = withdrawal.DocType;
        //    jModel.EntityId = withdrawal.EntityId;
        //    jModel.DocCurrency = withdrawal.DocCurrency;
        //    jModel.BaseCurrency = withdrawal.ExCurrency;
        //    jModel.ExchangeRate = withdrawal.ExchangeRate;
        //    jModel.GSTExCurrency = withdrawal.GSTExCurrency;
        //    jModel.GSTExchangeRate = withdrawal.GSTExchangeRate;
        //    jModel.DocDate = withdrawal.DocDate;
        //}
        //public void deleteJVPostInvoce(JournalDeleteModel tObject)
        //{
        //    LoggingHelper.LogMessage(WithdrawalLoggingValidation.BankWithdrawalApplicationService,tObject);
        //    var json = RestSharpHelper.ConvertObjectToJason(tObject);
        //    try
        //    {
        //        object obj = tObject;
        //        string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
        //        var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
        //        if (response.ErrorMessage != null)
        //        {
        //            Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.ZCritical(ex.StackTrace);

        //        var message = ex.Message;
        //    }
        //}
        public void deleteJVPostWithdrawal(JournalSaveModel tObject)
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
                //        if (section.Ziraff[i].Name == WithdrawalConstants.IdentityBean)
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
                    // Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(WithdrawalLoggingValidation.BankWithdrawalApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }
        #endregion

        #region EmptyGridDetail
        public WithdrawalDetailModel GetWithdrawalDetail()
        {
            WithdrawalDetailModel withdrawalDetail = new WithdrawalDetailModel();
            return withdrawalDetail;
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
            tails.CursorName = "Bean";
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
