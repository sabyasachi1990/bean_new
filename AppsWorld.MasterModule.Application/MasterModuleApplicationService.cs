using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using AppsWorld.MasterModule.Entities;
using AppsWorld.MasterModule.Models;
using AppsWorld.MasterModule.RepositoryPattern;
using AppsWorld.MasterModule.Service;
using Domain.Events;
using Repository.Pattern.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AppsWorld.MasterModule.Infra.Resources;
using Serilog;
using Logger;
using AppsWorld.MasterModule.Infra;
using AppsWorld.CommonModule.Models;
using AppsWorld.CommonModule.Application;
using System.Net;
using Newtonsoft.Json;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using AppsWorld.MasterModule.Entities.Models;

namespace AppsWorld.MasterModule.Application
{
    public class MasterModuleApplicationService
    {
        private readonly IBeanEntityService _beanEntityService;
        private readonly IControlCodeCategoryService _controlCodeCategoryService;
        private readonly ITermsOfPaymentService _termsOfPaymentService;
        private readonly IMultiCurrencySettingService _multiCurrencySettingService;
        private readonly ICurrencyService _currencyService;
        private readonly IIdTypeService _idTypeService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IAddressService _addressService;
        private readonly IAddressBookService _addressBookService;
        private readonly IMasterModuleUnitOfWorkAsync _unitOfWorkAsync;
        private readonly IInvoiceService _invoiceService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IGSTSettingService _gSTSettingService;
        private readonly IJournalService _journalService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly ICompanySettingService _companySettingService;
        //private readonly IBankReconciliationSettingService _bankReconciliationSettingService;
        //private readonly ISegmentMasterService _segmentMasterService;
        //private readonly ISegmentDetailService _segmentDetailService;
        //private readonly IForexService _forexService;
        private string fxErrorMsg = "";
        private readonly IItemService _itemService;
        //private readonly IJournalLedgerService _journalLedgerService;
        private readonly ITaxCodeService _taxCodeService;
        private readonly IControlCodeService _controlCodeService;
        private readonly ICompanyFeatureService _companyFeatureService;
        private readonly IModuleMosterService _moduleMasterService;
        private readonly ICCAccountTypeService _ccAccountTypeService;
        private readonly IAccountTypeIdtypeService _accountTypeIdtypeService;
        private readonly IActivityHistoryService _activityHistoryService;
        private readonly IJournalDetailService _journalDetailService;
        private readonly IDebitNoteService _debitNoteService;
        private readonly ICreditNoteApplicationService _creditNoteApplicationService;
        private readonly IReceiptService _receiptService;
        private readonly IFeatureService _featureService;
        private readonly ICashSalesService _cashSalesService;
        private readonly IContactService _contactService;
        private readonly IContactDetailService _contactDetailService;
        private readonly IOpeningBalanceDetail _openingBalanceDetail;
        private readonly ICommunication _communicationservice;
        private readonly IInterCompanySettingService _interCompanySettingService;
        private readonly IInterCompanySettingDetailService _interCompanySettingDetailService;
        private readonly ICOAMappingDetailService _cOAMappingDetailService;
        private readonly ICOAMappingService _cOAMappingServiceService;
        private readonly ITaxCodeMappingDetailService _taxCodeMappingDetailService;
        private readonly ITaxCodeMappingService _taxCodeMappingService;

        private readonly CommonApplicationService _commonAppService;
        private readonly ICommonForexService _commonForexService;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string query = string.Empty;

        public MasterModuleApplicationService(IBeanEntityService beanEntityService, IControlCodeCategoryService controlCodeCategoryService, ITermsOfPaymentService termsOfPaymentService, IMultiCurrencySettingService multiCurrencySettingService, ICurrencyService currencyService, IIdTypeService idTypeService, IAccountTypeService accountTypeService, IAddressService addressService, IAddressBookService addressBookService, IMasterModuleUnitOfWorkAsync unitOfWorkAsync, IInvoiceService invoiceService, IFinancialSettingService financialSettingService, IGSTSettingService gSTSettingService, IJournalService journalService, IChartOfAccountService chartOfAccountService, ICompanySettingService companySettingService/*, IBankReconciliationSettingService bankReconciliationSettingService*/, IItemService itemService, ITaxCodeService taxCodeService, IControlCodeService controlCodeService, ICompanyFeatureService companyFeatureService, IModuleMosterService moduleMasterService, ICCAccountTypeService ccAccountTypeService, IAccountTypeIdtypeService accountTypeIdtypeService, IActivityHistoryService activityHistoryService, IJournalDetailService journalDetailService, IDebitNoteService debitNoteService, ICreditNoteApplicationService creditNoteApplicationService, IReceiptService receiptService, IFeatureService featureService, ICashSalesService cashSalesService, IContactService contactService, IContactDetailService contactDetailService, IOpeningBalanceDetail openingBalanceDetail, ICommunication communicationservice, IInterCompanySettingService interCompanySettingService, IInterCompanySettingDetailService interCompanySettingDetailService, ICOAMappingDetailService cOAMappingDetailService, ICOAMappingService cOAMappingServiceService, ITaxCodeMappingDetailService taxCodeMappingDetailService, ITaxCodeMappingService taxCodeMappingService, CommonApplicationService commonAppService, ICommonForexService commonForexService)
        {
            this._beanEntityService = beanEntityService;
            this._controlCodeCategoryService = controlCodeCategoryService;
            this._termsOfPaymentService = termsOfPaymentService;
            this._multiCurrencySettingService = multiCurrencySettingService;
            this._currencyService = currencyService;
            this._idTypeService = idTypeService;
            this._accountTypeService = accountTypeService;
            this._addressService = addressService;
            this._addressBookService = addressBookService;
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._invoiceService = invoiceService;
            this._financialSettingService = financialSettingService;
            this._gSTSettingService = gSTSettingService;
            this._journalService = journalService;
            this._chartOfAccountService = chartOfAccountService;
            this._companySettingService = companySettingService;
            //this._bankReconciliationSettingService = bankReconciliationSettingService;
            //this._segmentMasterService = segmentMasterService;
            //this._segmentDetailService = segmentDetailService;
            //this._forexService = forexService;
            this._itemService = itemService;
            this._taxCodeService = taxCodeService;
            this._controlCodeService = controlCodeService;
            this._companyFeatureService = companyFeatureService;
            this._moduleMasterService = moduleMasterService;
            this._ccAccountTypeService = ccAccountTypeService;
            this._accountTypeIdtypeService = accountTypeIdtypeService;
            this._activityHistoryService = activityHistoryService;
            this._journalDetailService = journalDetailService;
            this._creditNoteApplicationService = creditNoteApplicationService;
            this._debitNoteService = debitNoteService;
            this._receiptService = receiptService;
            this._featureService = featureService;
            this._cashSalesService = cashSalesService;
            this._contactService = contactService;
            this._contactDetailService = contactDetailService;
            _openingBalanceDetail = openingBalanceDetail;
            _communicationservice = communicationservice;
            _interCompanySettingService = interCompanySettingService;
            _interCompanySettingDetailService = interCompanySettingDetailService;
            _cOAMappingDetailService = cOAMappingDetailService;
            _cOAMappingServiceService = cOAMappingServiceService;
            _taxCodeMappingDetailService = taxCodeMappingDetailService;
            _taxCodeMappingService = taxCodeMappingService;
            this._commonAppService = commonAppService;
            this._commonForexService = commonForexService;
        }

        #region BeanEntity

        #region Kendo

        public IQueryable<BeanEntityModelk> GetAllBeanEntitysK(long companyId)
        {
            return _beanEntityService.GetAllBeanEntitysK(companyId);
        }

        #endregion

        #region Create

        public async Task<BeanEntityModel> CreateEntity(long CompanyId, Guid id, string connectionString)
        {
            try
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Entered_into_CreateEntity_Method);

                BeanEntityModel beDTO = new BeanEntityModel();

                beDTO.NatureLU = new List<string> { "Trade", "Others", "Interco" };
                beDTO.VendorTypeLU = await _controlCodeCategoryService.GetByCategoryCodeCategoryAsync(CompanyId, ControlCodeConstants.Control_codes_VendorType);
                beDTO.AddressBookLU = await _controlCodeCategoryService.GetByCategoryCodeCategoryAsync(CompanyId, ControlCodeConstants.Control_Codes_CommunicationType);
                beDTO.IndustriesLU = await _controlCodeCategoryService.GetByCategoryCodeCategoryAsync(CompanyId, ControlCodeConstants.Control_codes_Industry);
                beDTO.CommunicationLU = await _controlCodeCategoryService.GetByCategoryCodeCategory1Asnc(CompanyId, ControlCodeConstants.Control_Codes_CommunicationType);
                var peppolEnable = GetPeppolActivation(CompanyId, connectionString);
                beDTO.IsPeppolEnable = peppolEnable?.ToLower() == "true".ToLower();

                BeanEntity beanEntity = await _beanEntityService.GetBeanEntitiesAsync(CompanyId, id);
                if (beanEntity != null)
                {
                    beDTO.IsContactExist = await _contactDetailService.IsContactExist(beanEntity.Id);
                    await FillBeanEntityModal(beDTO, beanEntity);
                    if (beanEntity.VendorType != null)
                    {
                        string[] vendorTypes = beanEntity.VendorType.Split(",".ToCharArray(),
                         StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < vendorTypes.Count(); i++)
                        {
                            string vendorType = vendorTypes[i];
                            var lookUp = await _controlCodeCategoryService.GetInactiveControlCodeAsync(CompanyId, ControlCodeConstants.Control_codes_VendorType, vendorType);
                            if (lookUp != null)
                            {
                                beDTO.VendorTypeLU.Lookups.Add(lookUp);
                            }
                        }
                    }
                    beDTO.CurrencyLU = await _currencyService.GetByCurrenciesByEntity(beanEntity.IsCustomer == true ? beanEntity.CustCurrency : beanEntity.VenCurrency, CompanyId);
                }
                else
                {
                    beDTO.Id = id;
                    beDTO.CompanyId = CompanyId;
                    beDTO.CustCurrency = MasterModuleValidations.Empty;
                    beDTO.VenCurrency = MasterModuleValidations.Empty;
                    beDTO.CurrencyLU = await _currencyService.GetByCurrenciesAsync(CompanyId, ControlCodeConstants.Currency_DefaultCode);
                }
                long tempId = beanEntity == null ? 0 : beanEntity.IdTypeId == null ? 0 : beanEntity.IdTypeId.Value;

                beDTO.IdTypeLU = (await _idTypeService.GetAllIdTypes(tempId, CompanyId))
               .Select(x => new LookUp<string>()
               {
                   Name = x.Name,
                   Id = x.Id,
                   RecOrder = x.RecOrder
               }).ToList();

                IEnumerable<CCAccountType> lstAccountTypes = await _ccAccountTypeService.GetAllCCAccountType(CompanyId);

                List<LookUpCategory<string>> CCAccountTypeLU = new List<LookUpCategory<string>>();

                if (lstAccountTypes.Any())
                {
                    foreach (var accountType in lstAccountTypes)
                    {
                        var lookUpCategorySingle = new LookUpCategory<string>();
                        lookUpCategorySingle.CategoryName = accountType.Name;
                        lookUpCategorySingle.Id = accountType.Id;
                        lookUpCategorySingle.Lookups = accountType.AccountTypeIdTypes.Where(c => c.AccountTypeId == accountType.Id).Select(x => new LookUp<string>()
                        {
                            Name = x.IdType != null ? x.IdType.Name : string.Empty,
                            Id = x.IdType != null ? x.IdType.Id : (long?)null,
                            RecOrder = x.IdType != null ? x.IdType.RecOrder : null,
                        }).OrderBy(c => c.RecOrder).ToList();
                        CCAccountTypeLU.Add(lookUpCategorySingle);
                    }
                }
                beDTO.CCAccountTypeLU = CCAccountTypeLU;

                long CTOPId = beanEntity == null ? 0 : beanEntity.CustTOPId == null ? 0 : beanEntity.CustTOPId.Value;

                beDTO.CreditTermsLU = (await _termsOfPaymentService.GetAllTermsOfPayment(CompanyId, CTOPId))
                .Select(x => new LookUp<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    TOPValue = x.TOPValue
                }).OrderBy(a => a.TOPValue).ToList();

                long VTOPId = beanEntity == null ? 0 : beanEntity.VenTOPId == null ? 0 : beanEntity.VenTOPId.Value;

                beDTO.PaymentTermsLU = (await _termsOfPaymentService.GetAllTermsOfPayments(CompanyId, VTOPId)).Select(x => new LookUp<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    TOPValue = x.TOPValue
                }).OrderBy(a => a.TOPValue).ToList();
                List<COALookup<string>> lstEditCoa = null;
                List<TaxCodeLookUp<string>> lstEditTax = null;
                List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                List<long> accType = await _accountTypeService.GetAllNameByAccountTypeAsync(CompanyId, coaName);
                List<ChartOfAccount> chartofaAccount = await _chartOfAccountService.GetChartOfAccountByAccountTypeCOAAsync(accType);
                List<COALookup<string>> lstCoas = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable ?? false,
                    IsPLAccount = x.Category == "Income Statement",
                    Class = x.Class,
                    Status = x.Status
                }).OrderBy(d => d.Name).ToList();
                beDTO.ChartOfAccountLU = lstCoas.OrderBy(s => s.Name).ToList();
                List<TaxCode> allTaxCodes = await _taxCodeService.GetTaxCodesAsync(CompanyId);
                if (allTaxCodes.Any())
                {
                    var TAX = allTaxCodes.GroupBy(a => a.Code).Select(a => new { code = a.Key, lstTax = a.FirstOrDefault() }).ToList();
                    beDTO.TaxCodeLU = TAX.Where(c => c.lstTax.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.lstTax.Id,
                        Code = x.lstTax.Code + " - " + x.lstTax.Name,
                        Name = x.lstTax.Name,
                        TaxRate = x.lstTax.TaxRate,
                        TaxType = x.lstTax.TaxType,
                        Status = x.lstTax.Status,
                        TaxIdCode = x.lstTax.Code != "NA" ? x.lstTax.Code + "-" + x.lstTax.TaxRate + (x.lstTax.TaxRate != null ? "%" : "NA") : x.lstTax.Code
                    }).OrderBy(c => c.Code).ToList();
                }
                if (beanEntity != null)
                {
                    List<long> lstCoa = new List<long>();
                    List<long> lstTax = new List<long>();
                    if (beanEntity.COAId != null)
                        lstCoa.Add(beanEntity.COAId.Value);
                    if (beDTO.ChartOfAccountLU.Any())
                        lstCoa = lstCoa.Except(beDTO.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    if (beanEntity.TaxId != null)
                        lstTax.Add(beanEntity.TaxId.Value);
                    if (beDTO.TaxCodeLU != null && beDTO.TaxCodeLU.Any())
                        lstTax = lstTax.Except(beDTO.TaxCodeLU.Select(x => x.Id.Value)).ToList();
                    if (lstCoa.Any())
                    {

                        lstEditCoa = chartofaAccount.Where(x => lstCoa.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable ?? false,
                            IsPLAccount = x.Category == "Income Statement",
                            Class = x.Class,
                            Status = x.Status
                        }).OrderBy(d => d.Name).ToList();
                        beDTO.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                    if (lstTax.Any())
                    {
                        lstEditTax = allTaxCodes.Where(c => lstTax.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.Code + " - " + x.Name,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
                        }).OrderBy(c => c.Code).ToList();
                        beDTO.TaxCodeLU.AddRange(lstEditTax);
                        var data = beDTO.TaxCodeLU;
                        beDTO.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                    }
                }
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Existed_from_CreateEntity_Method);
                return beDTO;

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        public List<SSICCodesModel> GetSSICodeByType(string Type, long companyid)
        {
            SSICCodesModel ssicCodesModel = new SSICCodesModel();
            List<SSICCodesModel> lstSSICCodeModel = new List<SSICCodesModel>();
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.GetSSICodeByType_Entered);
            try
            {
                var controlcodes = _beanEntityService.GetSSICCodesByType(Type);
                lstSSICCodeModel = controlcodes.Select(s => new SSICCodesModel()
                {
                    Id = s.Id,
                    Code = s.Code,
                    Description = s.Industry,
                    Status = s.Status,
                    CreatedDate = s.CreatedDate,
                    UserCreated = s.UserCreated,
                    ModifiedBy = s.ModifiedBy,
                    ModifiedDate = s.ModifiedDate,
                    IndustryName = s.Code + '-' + s.Industry
                }).ToList();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return lstSSICCodeModel;
        }
        public QuickBeanEntityModel QuickCreateEntity(long CompanyId, bool isCustomer, DateTime? docDate)
        {
            QuickBeanEntityModel beDTO = new QuickBeanEntityModel();
            beDTO.Id = Guid.NewGuid();
            //beDTO.IsMultyCurrency = false;
            //MultiCurrencySetting multi = _multiCurrencySettingService.Getmulticurrency(CompanyId);
            //beDTO.IsMultyCurrency = multi != null;
            //beDTO.NatureLU = _controlCodeCategoryService.GetByCategoryCodeCategory(CompanyId, ControlCodeConstants.Control_Codes_Nature);
            beDTO.NatureLU = new List<string> { "Trade", "Others" };
            beDTO.VendorTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(CompanyId, ControlCodeConstants.Control_codes_VendorType);

            //**added for commnuciation look up and communicatio nlook up
            //beDTO.CommunicationLU = _controlCodeCategoryService.GetByCategoryCodeCategory(CompanyId, ControlCodeConstants.Control_codes_CommunicationType);
            beDTO.SalutationLU = _controlCodeCategoryService.GetByCategoryCodeCategory(CompanyId, ControlCodeConstants.Control_codes_SalutationLU);
            beDTO.CommunicationLU = _controlCodeCategoryService.GetByCategoryCodeCategory1(CompanyId, ControlCodeConstants.Control_Codes_CommunicationType);
            beDTO.CurrencyLU = _currencyService.GetByCurrencies(CompanyId, ControlCodeConstants.Currency_DefaultCode);
            if (isCustomer == true)
                beDTO.TermsOfPaymentLU = _termsOfPaymentService.GetByTOPCustomers(CompanyId);
            else
            {
                beDTO.TermsOfPaymentLU = _termsOfPaymentService.GetByTOPVendors(CompanyId);

                //Optimization Required
                List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
                //List<AccountType> accType = _accountTypeService.GetAllAccounyType(CompanyId);
                List<long> accType = _accountTypeService.GetAllNameByAccountType(CompanyId, coaName);
                List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountTypeCOA(accType);
                beDTO.ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status
                }).OrderBy(d => d.Name).ToList();

                //beDTO.ChartOfAccountLU = _chartOfAccountService.GetChartOfAccountByCId(CompanyId);

                //GSTSetting gstSetting = _gSTSettingService.GetGSTSettingByCompanyId(CompanyId);
                //if (gstSetting != null)
                //{
                //long taxId = beanEntity == null ? 0 : beanEntity.TaxId == null ? 0 : beanEntity.TaxId.Value;

                beDTO.TaxCodeLU = docDate != null ? _taxCodeService.GetAllTaxCodesBydocDate(CompanyId, docDate) :
                   _taxCodeService.GetAllTaxCodes(CompanyId, true);
                var data = beDTO.TaxCodeLU;
                beDTO.TaxCodeLU = data.OrderBy(c => c.Code).ToList();

                //}
            }
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Existed_from_QuickCreateEntity_Method);
            return beDTO;
        }
        //catch (Exception ex)
        //{
        //    LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //    throw ex;
        //}


        public async Task<List<LookUpGuid<string>>> GetCustomers(string invoiceId, long CompanyId, string docType)
        {
            try
            {
                Guid guid = new Guid(invoiceId);
                Guid? invoiceExists = _invoiceService.GetEntityIdById(guid, CompanyId);
                List<BeanEntity> lstBean = new List<BeanEntity>();
                if (invoiceExists == null)
                {
                    if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.CreditNote || docType == DocTypeConstants.DebitNote)
                        lstBean = await _beanEntityService.GetAllBeanEntitys(CompanyId);
                    else
                        lstBean = await _beanEntityService.GetAllBeanEntitiesExpInv(CompanyId);
                }
                else
                {
                    if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.CreditNote || docType == DocTypeConstants.DebitNote)
                        lstBean = await _beanEntityService.GetAllBeanEntity(invoiceExists, CompanyId);
                    else
                        lstBean = await _beanEntityService.GetAllBeanEntityExpInv(invoiceExists, CompanyId);
                }
                string data = _financialSettingService.GetFinancial(CompanyId);
                List<LookUpGuid<string>> customerLU = lstBean.Where(x => x.IsShowPayroll == true).Select(x => new LookUpGuid<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    Code = x.CustCurrency == null || x.CustCurrency == String.Empty || x.CustCurrency == "0" ? data : x.CustCurrency,
                    TOPId = x.CustTOPId,
                    Nature = x.CustNature,
                    RecOrder = x.RecOrder,
                    CustCreditlimit = x.CreditLimitValue,
                    ServiceEntityId = x.ServiceEntityId
                }).OrderBy(a => a.Name).ToList();
                return customerLU;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IQueryable<LookUpGuid<string>> GetCustomersNew(string invoiceId, long CompanyId, string docType)
        {
            try
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_ModuleMaster_GetCustomersNewLU_Request_Message);
                //Guid guid = new Guid(invoiceId);
                // Guid? invoiceExists = _invoiceService.GetEntityIdById(guid, CompanyId);
                IQueryable<BeanEntity> lstBean = null;
                  if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.CreditNote || docType ==  DocTypeConstants.DebitNote)
                     lstBean = _beanEntityService.GetAllBeanEntitysNew(CompanyId);
                  else
                     lstBean = _beanEntityService.GetAllBeanEntitiesExpInvNew(CompanyId);

                string data = _financialSettingService.GetFinancial(CompanyId);
                IQueryable<LookUpGuid<string>> customerLU = lstBean.Where(x => x.IsShowPayroll == true).Select(x => new LookUpGuid<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    Code = x.CustCurrency == null || x.CustCurrency == String.Empty || x.CustCurrency == "0" ? data : x.CustCurrency,
                    TOPId = x.CustTOPId,
                    Nature = x.CustNature,
                    RecOrder = x.RecOrder,
                    CustCreditlimit = x.CreditLimitValue,
                    ServiceEntityId = x.ServiceEntityId
                }).OrderBy(a => a.Name).AsQueryable();
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_ModuleMaster_GetCustomersNewLU_Request_Message_Completed);
                return customerLU;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        public async Task<AppsWorld.MasterModule.Models.BeanForex> GetExRateInformation(string DocumentCurrency, DateTime Documentdate, bool IsBase, long CompanyId, bool IsGst)
        {
            try
            {
                AppsWorld.MasterModule.Models.BeanForex forex = new AppsWorld.MasterModule.Models.BeanForex();
                string BaseCurrency = string.Empty;
                string GstBaseCurrency = string.Empty;
                string date = Documentdate.ToString("yyyy-MM-dd");
                forex.DocumentDate = date;
                forex.Provider = "Fixer";
                if (IsBase)
                {

                    BaseCurrency = await _financialSettingService.GetBaseCurrencyByCompanyId(CompanyId);

                    if (DocumentCurrency == BaseCurrency)
                        forex.BaseUnitPerUSD = 1;
                    else
                    {

                        CommonForex commonForex = await _commonForexService.GetForexyByDateAndCurrency(CompanyId, BaseCurrency, DocumentCurrency, Convert.ToDateTime(Documentdate));
                        if (commonForex != null)
                        {
                            forex.BaseUnitPerUSD = commonForex.FromForexRate;
                        }
                        else
                        {
                            var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + DocumentCurrency + "&symbols=" + BaseCurrency;
                            AppsWorld.CommonModule.Models.CurrencyModel currencyRates = _download_serialized_json_data<AppsWorld.CommonModule.Models.CurrencyModel>(url);
                            if (currencyRates.Base == null)
                            {
                                AppsWorld.MasterModule.Models.BeanForex forex1 = new AppsWorld.MasterModule.Models.BeanForex();
                                return forex1;
                            }

                            if (currencyRates.Rates != null)
                            {
                                forex.BaseUnitPerUSD = currencyRates.Rates.Where(c => c.Key == BaseCurrency).Select(c => c.Value).FirstOrDefault();
                                FillCommonForexFrom(DocumentCurrency, Documentdate, forex, BaseCurrency, true);
                            }

                        }
                    }
                }
                if (IsGst)
                {

                    GstBaseCurrency = await _gSTSettingService.GetGSTSettingRepoAsync(CompanyId);
                    if (GstBaseCurrency == BaseCurrency)
                    {
                        forex.GSTUnitPerUSD = forex.BaseUnitPerUSD;
                    }
                    else if (DocumentCurrency == GstBaseCurrency)
                        forex.GSTUnitPerUSD = 1;
                    else
                    {
                        CommonForex commonForex = await _commonForexService.GetForexyByDateAndCurrency(CompanyId, GstBaseCurrency, DocumentCurrency, Convert.ToDateTime(Documentdate));
                        if (commonForex != null)
                        {
                            forex.GSTUnitPerUSD = commonForex.FromForexRate;
                        }
                        else
                        {
                            var gstCurrencyUrl = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + DocumentCurrency + "&symbols=" + GstBaseCurrency;
                            AppsWorld.CommonModule.Models.CurrencyModel gstcurrencyRates = _download_serialized_json_data<AppsWorld.CommonModule.Models.CurrencyModel>(gstCurrencyUrl);
                            if (gstcurrencyRates.Rates != null)
                            {
                                forex.GSTUnitPerUSD = gstcurrencyRates.Rates.Where(c => c.Key == GstBaseCurrency).Select(c => c.Value).FirstOrDefault();
                                FillCommonForexFrom(DocumentCurrency, Documentdate, forex, GstBaseCurrency, false);
                            }

                        }

                    }
                }
                return forex;
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;

            }
        }
        public string SubmitExchangeRate(string FromCurrency, string ToCurrency, string connectionString)
        {
            List<DateTime> lstAllDates = new List<DateTime>();
            CommonForex commons = new CommonForex();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("Bean_ExchangeRate_Insertion", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FromCurrency", FromCurrency);
                    cmd.Parameters.AddWithValue("@ToCurrency", ToCurrency);
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            commons = new CommonForex();
                            DateTime dates = Convert.ToDateTime(dr["alldates"]);
                            string date = dates.ToString("yyyy-MM-dd");
                            var url = "https://data.fixer.io/api/" + date + "?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + ToCurrency + "&symbols=" + FromCurrency;
                            AppsWorld.CommonModule.Models.CurrencyModel currencyRates = _download_serialized_json_data<AppsWorld.CommonModule.Models.CurrencyModel>(url);
                            decimal? BaseUnitPerUSD = currencyRates.Rates.Where(c => c.Key == FromCurrency).Select(c => c.Value).FirstOrDefault();
                            FillCommonForexFrom1(ToCurrency, dates, BaseUnitPerUSD, FromCurrency, commons);
                            commons.ObjectState = ObjectState.Added;
                            _commonForexService.Insert(commons);
                        }
                    }
                    con.Close();
                }
                _unitOfWorkAsync.SaveChanges();
                return "Inserted Successfully.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillCommonForexFrom(string DocumentCurrency, DateTime Documentdate, Models.BeanForex forex, string GstCurrency, bool isBase)
        {
            CommonForex commonForex = new CommonForex();
            commonForex.Id = Guid.NewGuid();
            commonForex.CompanyId = 0;
            commonForex.DateFrom = Convert.ToDateTime(Documentdate);
            commonForex.Dateto = commonForex.DateFrom;
            commonForex.FromForexRate = isBase ? forex.BaseUnitPerUSD : forex.GSTUnitPerUSD;
            commonForex.ToForexRate = commonForex.FromForexRate;
            commonForex.FromCurrency = GstCurrency;
            commonForex.ToCurrency = DocumentCurrency;
            commonForex.Status = RecordStatusEnum.Active;
            commonForex.Source = "Fixer";
            commonForex.UserCreated = "System";
            commonForex.CreatedDate = DateTime.UtcNow;

            commonForex.ObjectState = ObjectState.Added;
            _commonForexService.Insert(commonForex);
            _unitOfWorkAsync.SaveChanges();
        }
        private void FillCommonForexFrom1(string ToCurrency, DateTime Documentdate, decimal? BaseUnitPerUSD, string BaseCurrency, CommonForex commonFor)
        {
            commonFor.Id = Guid.NewGuid();
            commonFor.CompanyId = 0;
            commonFor.DateFrom = Documentdate;
            commonFor.Dateto = Documentdate;
            commonFor.FromForexRate = BaseUnitPerUSD;
            commonFor.ToForexRate = BaseUnitPerUSD;
            commonFor.FromCurrency = BaseCurrency;
            commonFor.ToCurrency = ToCurrency;
            commonFor.Status = RecordStatusEnum.Active;
            commonFor.Source = "Fixer";
            commonFor.UserCreated = "System";
            commonFor.CreatedDate = DateTime.UtcNow;
        }
        //public Forex GetExRateInformation(string DocumentCurrency, DateTime Documentdate, bool IsBase, long CompanyId, bool IsGst)
        //{
        //    Forex forex = null;
        //    //DateTime mydate = Documentdate.Date;
        //    bool isBase = false;
        //    //bool isGst = false;
        //    if (IsBase)
        //    {
        //        forex = _forexService.GetForex(DocumentCurrency, Documentdate, CompanyId);

        //        if (forex == null)
        //        {
        //            forex = _forexService.GetForexbycId(DocumentCurrency, CompanyId, Documentdate);
        //        }
        //        if (forex != null)
        //        {
        //            isBase = true;
        //            forex.UnitPerUSD = decimal.Round(1 / forex.UnitPerUSD, 10);
        //        }
        //    }
        //    if (IsGst)
        //    {
        //        Forex forexa = null;
        //        GSTSetting GSTSetting = _gSTSettingService.GetGSTSettingByCompanyId(CompanyId);
        //        if (GSTSetting != null)
        //        {
        //            FinancialSetting financialSetting = _financialSettingService.GetFinancialNyCompanyId(CompanyId);
        //            if (financialSetting != null && financialSetting.BaseCurrency == GSTSetting.GSTRepoCurrency)
        //            {
        //                forexa = _forexService.GetForex(DocumentCurrency, Documentdate, CompanyId);
        //                if (forexa == null)
        //                {
        //                    forexa = _forexService.GetForexbycId(DocumentCurrency, CompanyId, Documentdate);
        //                }
        //            }
        //            else
        //            {
        //                forexa = _forexService.GetByGSTCurrency(DocumentCurrency, Documentdate, CompanyId);
        //                if (forexa == null)
        //                {
        //                    forexa = _forexService.GetByGSTCurrency(DocumentCurrency, CompanyId, Documentdate);
        //                }

        //            }
        //        }
        //        if (forexa != null)
        //        {
        //            //isGst = true;
        //            forexa.GSTUnitPerUSD = forexa != null ? (decimal?)decimal.Round(1 / forexa.UnitPerUSD, 10) : null;
        //            forexa.GSTDateFrom = forexa != null ? forexa.DateFrom : (DateTime?)null;
        //            forexa.GSTDateTo = forexa != null ? forexa.Dateto : (DateTime?)null;
        //        }
        //        if (forex != null && forexa != null)
        //        {
        //            if (forex.Currency == forexa.Currency/*) && (forex.Dateto == forexa.Dateto) && (forex.DateFrom == forexa.DateFrom)*/)
        //            {
        //                forex.GSTUnitPerUSD = decimal.Round(1 / forexa.UnitPerUSD, 10);
        //                forex.GSTDateFrom = forexa.DateFrom;
        //                forex.GSTDateTo = forexa.Dateto;
        //            }
        //        }
        //        else
        //        {
        //            if (forexa != null)
        //                forex = forexa;
        //        }
        //    }
        //    if (forex != null)
        //    {
        //        if (!isBase)
        //        {
        //            forex.BaseDateTo = null;
        //            forex.BaseDateFrom = null;
        //            forex.BaseUnitPerUSD = null;
        //        }
        //        else
        //        {
        //            forex.BaseDateTo = forex.Dateto;
        //            forex.BaseDateFrom = forex.DateFrom;
        //            forex.BaseUnitPerUSD = forex.UnitPerUSD;
        //        }
        //    }
        //    //if (!isGst)
        //    //{
        //    //    forex.GSTDateTo = null;
        //    //    forex.GSTDateFrom = null;
        //    //    forex.GSTUnitPerUSD = null;
        //    //}
        //    return forex;
        //}
        public async Task<List<LookUpVendor<string>>> GetVendors(string invoiceId, long CompanyId, DateTime? docDate, string docType)
        {
            Guid guid = new Guid(invoiceId);
            List<BeanEntity> lstBean = new List<BeanEntity>();
            if (docType == DocTypeConstants.Bills || docType == DocTypeConstants.BillCreditMemo)
                lstBean = await _beanEntityService.GetBeanEntityByVendor(CompanyId);
            else
                lstBean = await _beanEntityService.GetBeanEntityByVendorExpBill(CompanyId);
            List<TaxCode> lstTaxcode = await _taxCodeService.GetTaxCodeAsync(CompanyId);
            string code = null;
            string data = _financialSettingService.GetFinancial(CompanyId);
            List<LookUpVendor<string>> VendorLU = lstBean.Select(x => new LookUpVendor<string>()
            {

                Name = x.Name,
                Id = x.Id,
                Code = x.VenCurrency == null || x.VenCurrency == String.Empty || x.VenCurrency == "0" ? data : x.VenCurrency,
                TOPId = x.VenTOPId,
                Nature = x.VenNature,
                RecOrder = x.RecOrder,
                CustCreditlimit = x.VenCreditLimit,
                TaxCode = code = lstTaxcode.Where(c => c.Id == x.TaxId).Select(c => c.Code).FirstOrDefault(),
                COAId = x.COAId,
                TaxId = x.TaxId != null ? lstTaxcode.Where(c => c.Code == code).Select(c => c.Id).ToList() : new List<long>()

            }).OrderBy(a => a.Name).ToList();
            return VendorLU;
        }


        public GetEntityModel GetEntity(Guid documentId)
        {
            GetEntityModel beTDo = new GetEntityModel();
            var beanentity = _beanEntityService.GetBeanEntityByDocumentId(documentId);
            if (beanentity != null)
            {
                beTDo.Id = beanentity.Id;
                beTDo.Name = beanentity.Name;
                beTDo.CustTOPId = beanentity.CustTOPId;
                beTDo.CustTOPValue = beanentity.CustTOPValue;
                beTDo.CustNature = beanentity.CustNature;
            }
            return beTDo;
        }

        #endregion

        #region Save

        public BeanEntity SaveEntityModel(BeanEntityModel beDTO, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(beDTO));
            Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, "ObjectSave", AdditionalInfo);

            bool isEntity = false;
            string state;
            Guid entityId;
            bool isFirst = false;
            string oldName = null;

            //decimal? oldCreditLimit;
            //decimal? oldRemainingCreditLimit;
            decimal? balanceAmount;
            bool isEdit = false;

            //Guid oldEntityId;
            string _errors = CommonValidation.ValidateObject(beDTO);
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_Save_SaveEntityModel_Request_Message);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new InvalidOperationException(_errors);
            }
            if (_beanEntityService.IfEntityNameExistsByNature(beDTO.CompanyId, beDTO.Id, beDTO.Name, beDTO.CustNature ?? beDTO.VenNature))
                throw new InvalidOperationException(MasterModuleValidations.Entity_Name_Already_Exists_for_this_company);

            BeanEntity _beanEntity = null;
            if (beDTO.IsExternalData == true && beDTO.DocumentId != null)
                _beanEntity = _beanEntityService.GetBeanEntityByDocumentId(beDTO.DocumentId ?? Guid.Empty);
            else
                _beanEntity = _beanEntityService.GetBeanEntityByIdAndCompanyId(beDTO.CompanyId, beDTO.Id);
            if (_beanEntity != null)
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_Save_SaveEntityModel_UpdateRequest_Message);
                oldName = _beanEntity.Name;
                _beanEntity.Name = beDTO.Name;
                Log.Logger.ZInfo(MasterModuleLoggingValidation.Log_BeanEntity_Entered_into_fill_Method_SaveEntityModel_Request_Message);
                if (_beanEntity.IsCustomer == true && beDTO.IsCustomer == true && beDTO.CustNature != "Interco")
                {
                    List<ContactDetail> lstcontactDetail = _contactDetailService.GetContactDetails(_beanEntity.Id);
                    if (lstcontactDetail.Where(s => s.IsPrimaryContact == true).Select(s => s.IsPrimaryContact).FirstOrDefault() != true)
                    {
                        throw new InvalidOperationException("Minimum one Primary Contact Is Mandatory");
                    }
                }

                //for credit limit calculation
                balanceAmount = _beanEntity.CustBal;
                FillGstSetting(beDTO, _beanEntity);
                //creditLimit updation
                if (beDTO.CustCreditLimit != null && beDTO.IsCustomer == true && beDTO.IsExternalData != true)
                {
                    _beanEntity.CustCreditLimit = beDTO.CustCreditLimit;
                    _beanEntity.CreditLimitValue = beDTO.CustCreditLimit - (balanceAmount != null ? balanceAmount : 0);
                }
                else
                {
                    _beanEntity.CustCreditLimit = null;
                    _beanEntity.CreditLimitValue = null;
                }
                _beanEntity.IsExternalData = (beDTO.CustNature == "Interco" || beDTO.VenNature == "Interco") ? true : beDTO.IsExternalData;
                isEdit = true;
                if (beDTO.Industry != null)
                    _beanEntity.Industry = beDTO.Industry;
                if (beDTO.IndustryCode != null)
                    _beanEntity.IndustryCode = beDTO.IndustryCode;
                if (beDTO.PrincipalActivities != null)
                    _beanEntity.PrincipalActivities = beDTO.PrincipalActivities;
                _beanEntity.ObjectState = ObjectState.Modified;
                entityId = _beanEntity.Id;
                _beanEntityService.Update(_beanEntity);
                if (beDTO.IsExternalData == true)
                    beDTO.Id = _beanEntity.Id;
                UpdateAddressForEntity(beDTO);
                if (_beanEntity.IsCustomer == true)
                    isEntity = true;
                state = Constrant.Edit;
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_Entered_into_fill_Method_Update_SaveEntityModel_Request_Message);
                isFirst = true;
                if (beDTO.ContactModelList != null && beDTO.ContactModelList.Count > 0 && isFirst)
                {
                    foreach (var contactModel in beDTO.ContactModelList)
                    {
                        SaveBeanEntityContact(contactModel, entityId, isFirst, ConnectionString, beDTO);
                    }
                }
            }
            else
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_Save_SaveEntityModel_NewRequest_Message);
                Guid value = Guid.NewGuid();
                _beanEntity = new BeanEntity();
                oldName = beDTO.Name;
                _beanEntity.Name = beDTO.Name;
                _beanEntity.TypeId = beDTO.TypeId;
                _beanEntity.IdTypeId = beDTO.IdTypeId;
                _beanEntity.IdNo = beDTO.IdNo;
                _beanEntity.CompanyId = beDTO.CompanyId;
                _beanEntity.GSTRegNo = beDTO.GSTRegNo;
                _beanEntity.Remarks = beDTO.Remarks;
                _beanEntity.IsCustomer = beDTO.IsCustomer == null ? false : beDTO.IsCustomer;
                _beanEntity.UserCreated = beDTO.UserCreated;
                _beanEntity.CreatedDate = beDTO.CreatedDate;
                _beanEntity.ExternalEntityType = beDTO.ExternalEntityType;
                _beanEntity.IsVendor = beDTO.IsVendor == null ? false : beDTO.IsVendor;
                _beanEntity.Communication = beDTO.Communication;
                _beanEntity.VendorType = beDTO.VendorType;
                _beanEntity.PeppolDocumentId = beDTO.PeppolDocumentId;
                _beanEntity.IsShowPayroll = true;
                if (beDTO.IsCustomer == true)
                {
                    if (beDTO.CustTOPId != null)
                    {
                        TermsOfPayment top = _termsOfPaymentService.GetTermsById(beDTO.CustTOPId.Value);
                        if (top != null)
                        {
                            _beanEntity.CustTOPId = beDTO.CustTOPId;
                            _beanEntity.CustTOP = top.Name;
                            _beanEntity.CustTOPValue = top.TOPValue;
                        }
                    }
                    isEntity = true;
                }
                _beanEntity.CustCreditLimit = beDTO.CustCreditLimit;
                _beanEntity.CreditLimitValue = _beanEntity.CustCreditLimit;
                _beanEntity.CustCurrency = beDTO.CustCurrency;
                _beanEntity.CustNature = beDTO.CustNature;
                if (beDTO.Industry != null)
                    _beanEntity.Industry = beDTO.Industry;
                if (beDTO.IndustryCode != null)
                    _beanEntity.IndustryCode = beDTO.IndustryCode;
                if (beDTO.PrincipalActivities != null)
                    _beanEntity.PrincipalActivities = beDTO.PrincipalActivities;
                if (beDTO.IsVendor == true && beDTO.VenTOPId != null)
                {
                    TermsOfPayment top = _termsOfPaymentService.GetTermsById(beDTO.VenTOPId.Value);
                    if (top != null)
                    {
                        _beanEntity.VenTOPId = beDTO.VenTOPId;
                        _beanEntity.VenTOP = top.Name;
                        _beanEntity.VenTOPValue = top.TOPValue;
                    }
                }
                _beanEntity.VenCurrency = beDTO.VenCurrency;
                _beanEntity.VenNature = beDTO.VenNature;
                _beanEntity.Id = value;
                _beanEntity.Status = beDTO.Status;
                _beanEntity.IsExternalData = (beDTO.CustNature == "Interco" || beDTO.VenNature == "Interco") ? true : beDTO.IsExternalData;
                _beanEntity.DocumentId = beDTO.DocumentId;
                _beanEntity.COAId = beDTO.COAId;
                _beanEntity.TaxId = beDTO.TaxId;
                _beanEntity.ObjectState = ObjectState.Added;
                entityId = _beanEntity.Id;
                isFirst = true;
                _beanEntityService.Insert(_beanEntity);
                if (beDTO.Addresses != null && beDTO.Addresses.Count > 0)
                {
                    LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_Save_Address_Exisist_NewRequest_Message);
                    foreach (var add in beDTO.Addresses)
                    {
                        AddressBook singleAddressBook = new AddressBook();

                        AddressBook addressBook = null;
                        if (addressBook == null)
                        {

                            singleAddressBook = add.AddressBook;
                            var addId = add.AddressBook.Id;
                            singleAddressBook.Id = Guid.NewGuid();
                            singleAddressBook.DocumentId = addId;
                            singleAddressBook.ObjectState = ObjectState.Added;
                            _addressBookService.Insert(singleAddressBook);
                        }
                        else
                        {
                            AddressBookFillForAddress(addressBook, add);//
                            addressBook.ObjectState = ObjectState.Modified;
                            _addressBookService.Update(addressBook);
                        }
                        Address address = new Address();
                        address.AddressBookId = singleAddressBook.Id;
                        address.AddSectionType = add.AddSectionType;
                        address.AddType = MasterModuleValidations.Entity;
                        address.AddTypeId = value;
                        address.Id = Guid.NewGuid();
                        address.DocumentId = add.Id;
                        address.Status = RecordStatusEnum.Active;
                        address.ObjectState = ObjectState.Added;
                        _addressService.Insert(address);
                    }
                }
                state = Constrant.Add;
                //**here new functionality contact save @first time only

                if ((beDTO.ContactModelList == null || beDTO.ContactModelList.Count == 0) && beDTO.IsCustomer == true)
                {
                    throw new InvalidOperationException("Minimum one Primary Conatct Is Mandatory");
                }

                if (beDTO.ContactModelList != null && beDTO.ContactModelList.Count > 0 && isFirst)
                    SaveBeanEntityContact(beDTO.ContactModelList.FirstOrDefault(), entityId, isFirst, ConnectionString, beDTO);

            }
            try
            {
                _unitOfWorkAsync.SaveChanges();

                if (isEntity)
                {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("[dbo].[Common_Sync_MasterData]", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", beDTO.CompanyId);
                        cmd.Parameters.AddWithValue("@Type", Constrant.Entity);
                        cmd.Parameters.AddWithValue("@SourceId", entityId);
                        cmd.Parameters.AddWithValue("@Action", state);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                if (isFirst && beDTO.ContactModelList.Any())
                {
                    //**update adress is copy selected
                    Sp_CopyAddresses(beDTO.ContactModelList.FirstOrDefault(), ConnectionString);
                }
                //long? id=
                //string result = $"select  from Common.CompanyModule where ModuleId =(select Id from Common.ModuleMaster where Name='Doc Cursor') and SetUpDone=1 and CompanyId={beDTO.CompanyId }";


                #region commented_Code_for_FolderCreation
                //Create screenFolder in Documents Screen
                //if (beDTO.IsVendor == true)
                //{
                //    if (isFirst)
                //    {
                //        saveScreenRecords(entityId.ToString(), entityId.ToString(), entityId.ToString(), beDTO.Name, beDTO.UserCreated, isFirst ? beDTO.CreatedDate.Value : beDTO.ModifiedDate, isFirst, beDTO.CompanyId, MasterModuleValidations.Entities, entityId.ToString());
                //    }
                //    else
                //    {
                //        if (beDTO.IsExternalData == true && beDTO.ExternalEntityType == "Employee")
                //        {
                //            if (!isFileExist(beDTO.CompanyId, entityId.ToString(), entityId.ToString()))
                //            {
                //                saveScreenRecords(entityId.ToString(), entityId.ToString(), entityId.ToString(), beDTO.Name, beDTO.UserCreated, isFirst ? beDTO.CreatedDate.Value : beDTO.ModifiedDate, true, beDTO.CompanyId, MasterModuleValidations.Entities, oldEntityId.ToString());
                //            }
                //        }
                //        if (oldName != beDTO.Name)
                //        {
                //            saveScreenRecords(entityId.ToString(), entityId.ToString(), entityId.ToString(), beDTO.Name, beDTO.UserCreated, isFirst ? beDTO.CreatedDate.Value : beDTO.ModifiedDate, isFirst, beDTO.CompanyId, oldName, oldEntityId.ToString());
                //        }
                //    }
                //}
                #endregion

                #region Folder_creation
                if (isFirst)
                {
                    CreateDynamicFolder(beDTO.CompanyId, beDTO.Name, MasterModuleValidations.Entities);
                }
                #endregion
                if (isEdit && oldName != beDTO.Name)
                    ChangeFolderName(beDTO.CompanyId, beDTO.Name, oldName);

                if (isEdit && oldName != beDTO.Name)
                    ChangeFolderName(beDTO.CompanyId, beDTO.Name, oldName);

                Log.Logger.ZInfo(MasterModuleLoggingValidation.Log_BeanEntity_Save_Saved_Data_Request_Message);
                //if (_beanEntitySelect != null)
                //{
                //    DomainEventChannel.Raise(new BeanEntityUpdated(beDTO));
                //    DomainEventChannel.Raise(new BeanEntityStatusChanged(beDTO));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new BeanEntityCreated(beDTO));
                //}
                //if (EventStatus == MasterModuleValidations.statuschanged)
                //{
                //    DomainEventChannel.Raise(new BeanEntityStatusChanged(beDTO));
                //}
                //Log.Logger.ZInfo(MasterModuleLoggingValidation.Log_BeanEntity_Save_SaveEntityModel_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return _beanEntity;
        }


        public BeanEntity QuickEntitySave(QuickBeanEntityModel beanEntity)
        {
            {
                string _errors = CommonValidation.ValidateObject(beanEntity);
                BeanEntity _beanEntityNew = new BeanEntity();
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_QuickEntitySave_Request_Message);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new Exception(_errors);
                }

                if (_beanEntityService.IfBeanEntityExists(beanEntity.Id, beanEntity.CompanyId, beanEntity.Name) == true)
                {
                    throw new Exception(MasterModuleValidations.Entity_Name_Already_Exists_for_this_company);
                }

                var _beanEntitySelect = _beanEntityService.GetBeanEntityByIdAndCompanyId(beanEntity.CompanyId, beanEntity.Id);
                DateTime _date = DateTime.UtcNow;
                if (_beanEntitySelect == null)
                {
                    //var _beanEntityNameCheck = _beanEntityService.GetBeanEntityNameChec(beanEntity.Name, beanEntity.CompanyId);
                    //if (_beanEntityNameCheck.Any())
                    //{
                    //    if (beanEntity.IsExternalData == true || beanEntity.DocumentId != null)
                    //    {
                    //        _beanEntityNew.Name = beanEntity.Name + "-" + 1;
                    //        //if(_beanEntityNameCheck.Where(a=>a.Name==_beanEntityNew.Name).FirstOrDefault())
                    //    }
                    //    else
                    //        throw new Exception(MasterModuleValidations.Entity_Name_Already_Exists_for_this_company);
                    //}
                    _beanEntityNew.Id = beanEntity.IsExternalData == true ? Guid.NewGuid() : beanEntity.Id;
                    _beanEntityNew.Name = beanEntity.Name;
                    _beanEntityNew.CompanyId = beanEntity.CompanyId;
                    _beanEntityNew.CustCurrency = beanEntity.CustCurrency;
                    _beanEntityNew.CustCreditLimit = beanEntity.CustCreditLimit;
                    _beanEntityNew.CreditLimitValue = beanEntity.CustCreditLimit;
                    _beanEntityNew.VenCurrency = beanEntity.VenCurrency;
                    _beanEntityNew.IsExternalData = beanEntity.IsExternalData;
                    _beanEntityNew.DocumentId = beanEntity.DocumentId;
                    TermsOfPayment top = _termsOfPaymentService.GetAllTermsOfPaymentsAllPaymentByNameAndCompanyId(beanEntity.TermsOfPayment, beanEntity.CompanyId);
                    if (beanEntity.IsCustomer == true)
                    {
                        if (top != null)
                        {
                            _beanEntityNew.CustTOPId = top.Id;
                            _beanEntityNew.CustTOP = top.Name;
                            _beanEntityNew.CustTOPValue = top.TOPValue;
                        }
                    }
                    else if (beanEntity.IsVendor == true)
                    {
                        if (top != null)
                        {
                            _beanEntityNew.VenTOPId = top.Id;
                            _beanEntityNew.VenTOP = top.Name;
                            _beanEntityNew.VenTOPValue = top.TOPValue;
                        }
                    }
                    _beanEntityNew.CustNature = beanEntity.CustNature;
                    _beanEntityNew.VendorType = beanEntity.VendorType;
                    _beanEntityNew.VenNature = beanEntity.VenNature;
                    _beanEntityNew.Status = beanEntity.Status;
                    if (_beanEntityNew.IsExternalData == false)
                    {
                        if (beanEntity.IsCustomer == true)
                            _beanEntityNew.CustTOPId = Convert.ToInt32(beanEntity.TermsOfPayment);
                    }
                    if (beanEntity.IsVendor == true)
                        _beanEntityNew.VenTOPId = Convert.ToInt32(beanEntity.TermsOfPayment);
                    _beanEntityNew.Version = beanEntity.Version;
                    _beanEntityNew.UserCreated = beanEntity.UserCreated;
                    _beanEntityNew.CreatedDate = DateTime.UtcNow;
                    //_beanEntityNew.Communication = beanEntity.Communication;
                    _beanEntityNew.IsVendor = beanEntity.IsVendor;
                    _beanEntityNew.IsShowPayroll = true;
                    _beanEntityNew.IsCustomer = beanEntity.IsCustomer;
                    _beanEntityNew.COAId = beanEntity.COAId;
                    _beanEntityNew.TaxId = beanEntity.TaxId;
                    _beanEntityNew.ObjectState = ObjectState.Added;
                    _beanEntityService.Insert(_beanEntityNew);
                    //**here contactSave 
                    if (beanEntity.ContactName != null && beanEntity.ContactName != string.Empty)
                        SaveQuickContact(beanEntity, _beanEntityNew);
                }
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    try
                    {
                        CreateDynamicFolder(beanEntity.CompanyId, beanEntity.Name, MasterModuleValidations.Entities);
                        //if (beanEntity.IsVendor == true)
                        //    saveScreenRecords(_beanEntityNew.Id.ToString(), _beanEntityNew.Id.ToString(), _beanEntityNew.Id.ToString(), _beanEntityNew.Name, _beanEntityNew.UserCreated, _beanEntityNew.CreatedDate.Value, true, _beanEntityNew.CompanyId, MasterModuleValidations.Entities, _beanEntityNew.Id.ToString());
                        //if (beanEntity.IsCustomer == true)
                        //    saveScreenRecords(_beanEntityNew.Id.ToString(), _beanEntityNew.Id.ToString(), _beanEntityNew.Id.ToString(), _beanEntityNew.Name, _beanEntityNew.UserCreated, _beanEntityNew.CreatedDate.Value, true, _beanEntityNew.CompanyId, MasterModuleValidations.Customers);
                    }
                    catch (Exception)
                    {
                        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Issues_In_Entity_Folder_creation);
                    }
                    // DomainEventChannel.Raise(new QuickBeanEntityCreated(beanEntity));
                    LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BeanEntity_QuickEntitySave_SuccessFully_Message);
                }
                catch (Exception ex)
                {
                    // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,MasterModuleLoggingValidation.Log_BeanEntity_QuickEntitySave_Exception_Message);
                    LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);


                    throw ex;


                }

                return _beanEntityNew;
            }
        }

        private void SaveQuickContact(QuickBeanEntityModel beanEntity, BeanEntity _beanEntityNew)
        {
            //if (beanEntity.ContactName == null || (beanEntity.Communication == null || beanEntity.Communication == string.Empty))
            //{
            //    throw new Exception("ContactName and Communication is Mandatory");
            //}
            beanEntity.ContactId = beanEntity.ContactId == null ? Guid.Empty : beanEntity.ContactId;
            Guid contactId = new Guid();
            Contact contact = _contactService.GetContact((Guid)beanEntity.ContactId, (long)beanEntity.CompanyId);
            if (contact != null)
            {
                contactId = contact.Id;
                contact.Salutation = beanEntity.Salutation;
                contact.FirstName = beanEntity.ContactName;
                contact.Status = RecordStatusEnum.Active;
                contact.ObjectState = ObjectState.Added;
                _contactService.Update(contact);
            }
            else
            {
                Contact contactInsert = new Contact();
                contactInsert.Id = Guid.NewGuid();
                contactId = contactInsert.Id;
                contactInsert.CompanyId = beanEntity.CompanyId;
                contactInsert.Salutation = beanEntity.Salutation;
                contactInsert.Communication = beanEntity.Communication;
                contactInsert.FirstName = beanEntity.ContactName;
                contactInsert.Status = RecordStatusEnum.Active;
                contactInsert.ObjectState = ObjectState.Added;
                _contactService.Insert(contactInsert);
            }
            //--contact detail
            ContactDetail insertContactDetail = new ContactDetail();
            insertContactDetail.Id = Guid.NewGuid();
            insertContactDetail.ContactId = contactId;
            insertContactDetail.EntityId = _beanEntityNew.Id;
            insertContactDetail.EntityType = "Entity";
            insertContactDetail.CursorShortCode = "Bean";
            insertContactDetail.IsPrimaryContact = true;
            //insertContactDetail.Communication = beanEntity.Communication;
            insertContactDetail.CreatedDate = DateTime.UtcNow;
            insertContactDetail.UserCreated = beanEntity.UserCreated;
            insertContactDetail.Status = RecordStatusEnum.Active;
            insertContactDetail.ObjectState = ObjectState.Added;
            _contactDetailService.Insert(insertContactDetail);
        }

        public List<string> EntityMigration(long companyId)
        {
            List<BeanEntity> lstBeanEntites = _beanEntityService.GetListOfEntity(companyId);
            List<string> lstEntityIds = new List<string>();
            if (lstBeanEntites.Any())
            {
                foreach (var entity in lstBeanEntites)
                {
                    bool isMigrated = saveScreenRecords(entity.Id.ToString(), entity.Id.ToString(), entity.Id.ToString(), entity.Name, entity.UserCreated, entity.CreatedDate.Value, true, companyId, "Entities", entity.Id.ToString());
                    if (isMigrated)//if sucess then add that Entity id to list
                        lstEntityIds.Add(entity.Name.ToString());//to know the no of Entity Created
                }
            }
            return lstEntityIds;
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

        public bool saveScreenRecords(string recordId, string refrenceId, string featureId, string recordName, string userName, DateTime? date, bool isAdd, long comapnyid, string screenName, string oldFeatureId)
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
                //var message = ex.Message;
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Issues_In_Entity_Folder_creation);
                return false;
            }
        }

        #endregion

        #region Fill
        public async Task FillBeanEntityModal(BeanEntityModel beDTO, BeanEntity entity)
        {
            try
            {
                beDTO.Id = entity.Id;
                beDTO.CompanyId = entity.CompanyId;
                beDTO.Name = entity.Name;
                beDTO.TypeId = entity.TypeId;
                beDTO.IdTypeId = entity.IdTypeId;
                beDTO.IdNo = entity.IdNo;
                beDTO.GSTRegNo = entity.GSTRegNo;
                beDTO.IsCustomer = entity.IsCustomer;
                beDTO.CustTOPId = entity.CustTOPId;
                beDTO.CustTOP = entity.CustTOP;
                beDTO.CustTOPValue = entity.CustTOPValue;
                beDTO.CustCreditLimit = entity.CustCreditLimit;
                beDTO.CustCurrency = entity.CustCurrency;
                beDTO.IsShowPayroll = entity.IsShowPayroll;
                beDTO.ExternalEntityType = entity.ExternalEntityType;
                beDTO.CustNature = entity.CustNature;
                beDTO.IsVendor = entity.IsVendor;
                beDTO.VenTOPId = entity.VenTOPId;
                beDTO.VenTOP = entity.VenTOP;
                beDTO.VenTOPValue = entity.VenTOPValue;
                beDTO.VenCreditLimit = entity.VenCreditLimit;
                beDTO.VenCurrency = entity.VenCurrency;
                beDTO.VenNature = entity.VenNature;
                beDTO.RecOrder = entity.RecOrder;
                beDTO.Remarks = entity.Remarks;
                beDTO.UserCreated = entity.UserCreated;
                beDTO.CreatedDate = entity.CreatedDate;
                beDTO.ModifiedBy = entity.ModifiedBy;
                beDTO.ModifiedDate = entity.ModifiedDate;
                beDTO.Version = entity.Version;
                beDTO.Communication = entity.Communication;
                beDTO.VendorType = entity.VendorType;
                beDTO.Status = entity.Status;
                beDTO.IsExternalData = (entity.CustNature == "Interco" || entity.VenNature == "Interco") ? false : entity.IsExternalData;
                beDTO.COAId = entity.COAId;
                beDTO.TaxId = entity.TaxId;
                beDTO.DocumentId = entity.DocumentId;
                beDTO.PeppolDocumentId = entity.PeppolDocumentId;
                beDTO.PrincipalActivities = entity.PrincipalActivities;
                beDTO.IndustryCode = entity.IndustryCode;
                beDTO.Industry = entity.Industry;
                List<Address> lstAddress = new List<Address>();
                List<Address> _lstAddresses = await _addressService.GetAddres(entity.Id);
                List<Guid?> lstAddressbookIds = _lstAddresses.Select(c => c.AddressBookId).ToList();
                List<AddressBook> lstAddressBook = await _addressBookService.GetAllAddressBook(lstAddressbookIds);
                if (_lstAddresses.Any())
                {
                    foreach (var add in _lstAddresses)
                    {

                        AddressBook book = lstAddressBook.FirstOrDefault(c => c.Id == add.AddressBookId);
                        if (book == null)
                            book = new AddressBook();
                        else
                        {
                            add.AddressBook = book;
                            add.AddressBookId = book.Id;
                        }
                        lstAddress.Add(add);
                    }
                    beDTO.Addresses = lstAddress;
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void FillGstSetting(BeanEntityModel beDTO, BeanEntity _beanEntitySelect)
        {
            _beanEntitySelect.TypeId = beDTO.TypeId;
            _beanEntitySelect.IdTypeId = beDTO.IdTypeId;
            _beanEntitySelect.IdNo = beDTO.IdNo;
            _beanEntitySelect.CompanyId = beDTO.CompanyId;
            _beanEntitySelect.GSTRegNo = beDTO.GSTRegNo;
            _beanEntitySelect.IsCustomer = beDTO.IsCustomer;
            _beanEntitySelect.Communication = beDTO.Communication;
            _beanEntitySelect.VendorType = beDTO.VendorType;
            _beanEntitySelect.CustTOPId = beDTO.CustTOPId;
            _beanEntitySelect.CustTOP = beDTO.CustTOP;
            _beanEntitySelect.CustTOPValue = beDTO.CustTOPValue;
            _beanEntitySelect.CustCreditLimit = beDTO.CustCreditLimit;
            _beanEntitySelect.CreditLimitValue = beDTO.CustCreditLimit;
            _beanEntitySelect.CustCurrency = beDTO.CustCurrency;
            _beanEntitySelect.CustNature = beDTO.CustNature;
            _beanEntitySelect.IsVendor = beDTO.IsVendor;
            _beanEntitySelect.VenTOPId = beDTO.VenTOPId;
            _beanEntitySelect.VenTOP = beDTO.VenTOP;
            _beanEntitySelect.VenTOPValue = beDTO.VenTOPValue;
            _beanEntitySelect.VenCurrency = beDTO.VenCurrency;
            _beanEntitySelect.VenNature = beDTO.VenNature;
            _beanEntitySelect.Status = beDTO.Status;
            _beanEntitySelect.ModifiedBy = beDTO.ModifiedBy;
            _beanEntitySelect.ModifiedDate = beDTO.ModifiedDate;
            _beanEntitySelect.RecOrder = beDTO.RecOrder;
            _beanEntitySelect.Remarks = beDTO.Remarks;
            _beanEntitySelect.ExternalEntityType = beDTO.ExternalEntityType;
            _beanEntitySelect.Version = beDTO.Version;
            _beanEntitySelect.IsExternalData = beDTO.IsExternalData;
            _beanEntitySelect.DocumentId = beDTO.DocumentId;
            _beanEntitySelect.COAId = beDTO.COAId;
            _beanEntitySelect.TaxId = beDTO.TaxId;
            _beanEntitySelect.PeppolDocumentId = beDTO.PeppolDocumentId;
        }

        private void UpdateAddressForEntity(BeanEntityModel model)
        {
            try
            {
                if (model.Addresses != null && model.Addresses.Any())
                    //List<Address> lstAddresses = _addressService.GetAddresById(model.Id);
                    foreach (var address in model.Addresses)
                    {
                        if (address.RecordStatus == "Deleted")
                        {
                            //if (lstAddresses != null && lstAddresses.Count > 0)
                            //{
                            //foreach (var addres in lstAddresses)
                            //{
                            // var addresse = _addressService.GetAddressId(address.Id);
                            address.ObjectState = ObjectState.Deleted;
                            _addressService.Delete(address.Id);
                            //_unitOfWorkAsync.SaveChanges();
                            _addressBookService.Delete(address.AddressBookId);
                            _unitOfWorkAsync.SaveChanges();
                        }
                        //}
                        //}
                    }
                if (model.Addresses != null && model.Addresses.Count > 0)
                {

                    foreach (var add in model.Addresses)
                    {
                        if (add.RecordStatus == "Added" || add.RecordStatus == "Modified")
                        {
                            AddressBook singleAddressBook = new AddressBook();
                            Address address = null;
                            if (model.IsExternalData == true)
                                address = _addressService.GetAddressByDocumentId(add.Id);
                            else
                                address = _addressService.GetAddressId(add.Id);

                            if (address == null)
                            {
                                AddressBook addressBook = null;
                                if (model.IsExternalData == true)
                                    addressBook = _addressBookService.GetAddressBookByDocumentId(add.AddressBookId.Value);
                                else
                                    addressBook = _addressBookService.GetAddressBook(add.AddressBookId.Value);
                                if (addressBook == null)
                                {

                                    singleAddressBook = add.AddressBook;
                                    singleAddressBook.Id = Guid.NewGuid();
                                    singleAddressBook.DocumentId = add.AddressBook.DocumentId;
                                    singleAddressBook.ObjectState = ObjectState.Added;
                                    _addressBookService.Insert(singleAddressBook);
                                }
                                else
                                {
                                    AddressBookFillForAddress(addressBook, add);
                                    addressBook.ObjectState = ObjectState.Modified;
                                    _addressBookService.Update(addressBook);
                                }
                                Address addNew = new Address();
                                addNew.AddressBookId = singleAddressBook.Id;
                                addNew.AddSectionType = add.AddSectionType;
                                addNew.AddType = "Entity";
                                addNew.AddTypeId = model.Id;
                                addNew.Id = Guid.NewGuid();
                                addNew.Status = add.Status;
                                addNew.DocumentId = add.Id;
                                addNew.ObjectState = ObjectState.Added;
                                _addressService.Insert(addNew);
                                //lstAddresses.Add(addNew);
                            }
                            else
                            {
                                AddressBook addressBook = new AddressBook();
                                if (model.IsExternalData == true)
                                    addressBook = _addressBookService.GetAddressBookByDocumentId(add.AddressBookId.Value);
                                else
                                    addressBook = _addressBookService.GetAddressBook(add.AddressBookId.Value);

                                if (addressBook == null)
                                {
                                    //AddressBook singleAddressBook = new AddressBook();
                                    singleAddressBook = add.AddressBook;
                                    singleAddressBook.Id = Guid.NewGuid();
                                    singleAddressBook.DocumentId = add.AddressBook.Id;
                                    singleAddressBook.ObjectState = ObjectState.Added;
                                    _addressBookService.Insert(singleAddressBook);
                                }
                                else
                                {

                                    AddressBookFillForAddress(addressBook, add);
                                    //addressBook.DocumentId =model.IsExternalData==true ? address.AddressBook.I;
                                    addressBook.ObjectState = ObjectState.Modified;
                                    _addressBookService.Update(addressBook);

                                }
                                //  address.AddressBookId = add.AddressBookId;

                                address.AddSectionType = add.AddSectionType;
                                address.AddType = "Entity";
                                address.AddTypeIdInt = add.AddTypeIdInt;
                                address.AddTypeId = model.Id;
                                address.DocumentId = add.Id;
                                //address.Id = add.Id;
                                address.Status = add.Status;
                                address.ObjectState = ObjectState.Modified;
                                _addressService.Update(address);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;

            }
        }

        private void AddressBookFillForAddress(AddressBook addressBook, Address address)
        {
            //addressBook.Id = address.AddressBook.Id;
            addressBook.BlockHouseNo = address.AddressBook.BlockHouseNo;
            addressBook.BuildingEstate = address.AddressBook.BuildingEstate;
            addressBook.City = address.AddressBook.City;
            addressBook.Country = address.AddressBook.Country;
            addressBook.CreatedDate = address.AddressBook.CreatedDate;
            addressBook.Email = address.AddressBook.Email;
            addressBook.IsLocal = address.AddressBook.IsLocal;
            addressBook.Latitude = address.AddressBook.Latitude;
            addressBook.Longitude = address.AddressBook.Longitude;
            addressBook.ModifiedBy = address.AddressBook.ModifiedBy;
            addressBook.ModifiedDate = DateTime.UtcNow;
            addressBook.Phone = address.AddressBook.Phone;
            addressBook.PostalCode = address.AddressBook.PostalCode;
            addressBook.RecOrder = address.AddressBook.RecOrder;
            addressBook.Remarks = address.AddressBook.Remarks;
            addressBook.State = address.AddressBook.State;
            addressBook.Status = RecordStatusEnum.Active;
            addressBook.Street = address.AddressBook.Street;
            addressBook.UnitNo = address.AddressBook.UnitNo;
            addressBook.UserCreated = address.AddressBook.UserCreated;
            addressBook.Version = address.AddressBook.Version;
            addressBook.Website = address.AddressBook.Website;

        }


        #endregion
        public async Task<FinancialSummaryModel> GetFinancialSummary(Guid EntityId, long CompanyId, string username, string connectionString)
        {
            SqlCommand cmd;
            SqlDataReader dr;
            SqlConnection con;
            try
            {
                FinancialSummaryModel model = new FinancialSummaryModel();
                BeanEntity beanEntity = await _beanEntityService.GetBeanEntities(CompanyId, EntityId);
                if (beanEntity != null)
                {
                    using (con = new SqlConnection(connectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_Get_Financial_summary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.AddWithValue("@companyId", CompanyId);
                        cmd.Parameters.AddWithValue("@EntityId", EntityId.ToString());
                        cmd.Parameters.AddWithValue("@username", username);
                        dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            model.BillingsAmount = dr["Billings"] != DBNull.Value ? Convert.ToDecimal(dr["Billings"]) : (decimal?)null;
                            model.ReceiptsAmount = dr["PaidAmt"] != DBNull.Value ? Convert.ToDecimal(dr["PaidAmt"]) : (decimal?)null;
                            model.CreditAmount = dr["CreditAmount"] != DBNull.Value ? Convert.ToDecimal(dr["CreditAmount"]) : (decimal?)null;
                            model.BalanceAmount = dr["GrossBalance"] != DBNull.Value ? Convert.ToDecimal(dr["GrossBalance"]) : (decimal?)null;
                            model.DebtProvAmount = dr["DebtProvAmount"] != DBNull.Value ? Convert.ToDecimal(dr["DebtProvAmount"]) : (decimal?)null;
                            model.NetBalAmount = dr["NetBalance"] != DBNull.Value ? Convert.ToDecimal(dr["NetBalance"]) : (decimal?)null;
                        }
                        con.Close();
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Financialsetting

        public FinancialSetting GetFinancialSetting(long CompanyId)
        {
            FinancialSetting setting = _financialSettingService.GetFinancialNyCompanyId(CompanyId);
            if (setting != null)
            {
                if (setting.IsPosted == null || setting.IsPosted == false)
                {
                    var journal = _journalService.GetByCompanyId(CompanyId);
                    //var forex = _forexService.GetForx(CompanyId);
                    //setting.IsPosted = journal != null ? true : forex != null ? true : false;
                    setting.IsPosted = journal != null;
                    if (setting.IsPosted.Value)
                    {
                        setting.ObjectState = ObjectState.Modified;
                        _financialSettingService.Update(setting);
                        try
                        {
                            _unitOfWorkAsync.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
                string[] obj = setting.FinancialYearEnd.Split("-".ToCharArray(),
                      StringSplitOptions.RemoveEmptyEntries);
                setting.Date = obj[0];
                setting.Month = obj[1];
                setting.IsEdit = true;
            }
            else
            {
                throw new Exception("please set the financial settings in admin cursor General Settings");
            }

            return setting;
        }

        public bool VerifyFinancialLockPeriodPassword(string password, long companyId)
        {
            FinancialSetting VerifyFinancialLockPeriodPassword = _financialSettingService.VerifyFinancialLockPeriodPassword(password, companyId);
            return VerifyFinancialLockPeriodPassword != null;
        }

        public FinancialSetting Save(FinancialSetting financialSetting, string name)
        {
            string RecStatus = MasterModuleValidations.Empty;
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_FinancialSetting_Save_FinancialSettingModel_Request_Message);
            //	 LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,FinancialSettingValidation.Log_FinancialSetting_Save_Request_Message);
            string _errors = CommonValidation.ValidateObject(financialSetting);
            string BaseCurrencyChangedStatus = MasterModuleValidations.Empty;
            string FinancialYearEndChangedStatus = MasterModuleValidations.Empty;

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            if (!ValidateFainancialSetting(financialSetting))
            {
                //  throw new Exception(fsErrorMsg);
            }

            var _financialSettingSelect = _financialSettingService.GetFinancialByIdCId(financialSetting.Id, financialSetting.CompanyId);
            // _financialSettingRepository.Query(e => e.Id == financialSetting.Id && e.CompanyId == financialSetting.CompanyId).Select();
            var financialEdit = _financialSettingService.GetFinancialNyCompanyId(financialSetting.CompanyId);

            //_financialSettingRepository.Query(e => e.CompanyId == financialSetting.CompanyId).Select();
            FinancialSetting _financialSetting = new FinancialSetting();
            if (_financialSettingSelect != null)
            {
                _financialSetting = _financialSettingSelect;
                if (_financialSetting.FinancialYearEnd != financialSetting.FinancialYearEnd)
                {
                    FinancialYearEndChangedStatus = MasterModuleValidations.Updated;
                }
                _financialSetting.FinancialYearEnd = financialSetting.Date + "-" + financialSetting.Month;
                _financialSetting.TimeZone = financialSetting.TimeZone;
                _financialSetting.PeriodLockDate = financialSetting.PeriodLockDate;
                _financialSetting.PeriodLockDatePassword = financialSetting.PeriodLockDatePassword;
                _financialSetting.EndOfYearLockDate = financialSetting.EndOfYearLockDate;
                _financialSetting.Status = financialSetting.Status;
                _financialSetting.ModifiedBy = financialSetting.ModifiedBy; // HttpContext.Current.User.Identity.Name;
                _financialSetting.ModifiedDate = financialSetting.ModifiedDate;
                if (_financialSetting.BaseCurrency != financialSetting.BaseCurrency)
                {
                    BaseCurrencyChangedStatus = MasterModuleValidations.Updated;
                }
                _financialSetting.BaseCurrency = financialSetting.BaseCurrency;
                _financialSetting.LongDateFormat = financialSetting.LongDateFormat;
                _financialSetting.ShortDateFormat = financialSetting.ShortDateFormat;
                _financialSetting.TimeFormat = financialSetting.TimeFormat;
                _financialSetting.IsbaseCurrency = financialSetting.IsbaseCurrency;
                _financialSetting.PeriodEndDate = financialSetting.PeriodEndDate;
                //_financialSetting.IsPosted = financialSetting.IsPosted;
                RecStatus = MasterModuleValidations.Updated;

                _financialSetting.ObjectState = ObjectState.Modified;
                _financialSettingService.Update(_financialSetting);
            }
            else
            {
                var _finCompany = _financialSettingService.GetFinancialNyCompanyId(financialSetting.CompanyId);
                //   _financialSettingRepository.Query(e => e.CompanyId == financialSetting.CompanyId).Select();
                if (_finCompany == null)
                {
                    long value = 0;
                    var GetAllFinancialSettings = _financialSettingService.Queryable().ToList();
                    if (GetAllFinancialSettings.Any())
                    {
                        value = Convert.ToInt64(GetAllFinancialSettings.Max(c => c.Id));
                    }

                    financialSetting.IsPosted = false;
                    //financialSetting.UserCreated = financialSetting.UserCreated;
                    financialSetting.CreatedDate = DateTime.UtcNow;

                    financialSetting.Id = (value + 1);
                    financialSetting.FinancialYearEnd = financialSetting.Date + "-" + financialSetting.Month;
                    financialSetting.ObjectState = ObjectState.Added;
                    _financialSettingService.Insert(financialSetting);
                }
                else
                {
                    _financialSetting = _financialSettingService.GetFinancialNyCompanyId(financialSetting.CompanyId);
                    //   _financialSettingRepository.Query(e => e.CompanyId == financialSetting.CompanyId).Select().FirstOrDefault();

                    _financialSetting.FinancialYearEnd = financialSetting.Date + "-" + financialSetting.Month;
                    _financialSetting.TimeZone = financialSetting.TimeZone;
                    _financialSetting.PeriodLockDate = financialSetting.PeriodLockDate;
                    _financialSetting.PeriodLockDatePassword = financialSetting.PeriodLockDatePassword;
                    _financialSetting.EndOfYearLockDate = financialSetting.EndOfYearLockDate;
                    _financialSetting.Status = financialSetting.Status;
                    //_financialSetting.ModifiedBy = financialSetting.ModifiedBy; // HttpContext.Current.User.Identity.Name;
                    _financialSetting.ModifiedDate = financialSetting.ModifiedDate;
                    _financialSetting.ModifiedBy = financialSetting.ModifiedBy;
                    _financialSetting.BaseCurrency = financialSetting.BaseCurrency;
                    _financialSetting.LongDateFormat = financialSetting.LongDateFormat;
                    _financialSetting.ShortDateFormat = financialSetting.ShortDateFormat;
                    _financialSetting.TimeFormat = financialSetting.TimeFormat;
                    _financialSetting.IsbaseCurrency = financialSetting.IsbaseCurrency;
                    _financialSetting.PeriodEndDate = financialSetting.PeriodEndDate;
                    //_financialSetting.IsPosted = financialSetting.IsPosted;


                    _financialSetting.ObjectState = ObjectState.Modified;
                    _financialSettingService.Update(_financialSetting);
                    financialSetting = _financialSetting;
                }
            }

            try
            {
                _unitOfWorkAsync.SaveChanges();
                //awasthy curser initial setup
                if (financialSetting.ModuleDetailId != null)
                    UpadateInitialSetup(financialSetting.CompanyId, financialSetting.ModuleDetailId.Value);

                //if (FinancialYearEndChangedStatus == MasterModuleValidations.Updated)
                //{
                //    DomainEventChannel.Raise(new FinancialYearEndChangedCreated(financialSetting));
                //}

                //if (BaseCurrencyChangedStatus == "Updated")
                //{
                //    //    DomainEventChannel.Raise(new BaseCurrencyChangedCreated(financialSetting));
                //}
                //if (RecStatus == MasterModuleValidations.Updated)
                //{
                //    DomainEventChannel.Raise(new FinancialSettingUpdated(financialSetting));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new FinancialSettingCreated(financialSetting));
                //}
                // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,FinancialSettingValidation.Log_FinancialSetting_Save_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,FinancialSettingValidation.Log_FinancialSetting_Save_Exception_Message);
                //	Log.Logger.ZCritical(ex.StackTrace);

                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;

            }
            MultiCurrencySetting multiCurrency = _multiCurrencySettingService.Getmulticurrency(financialSetting.CompanyId);
            if (multiCurrency != null)
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_FinancialSetting_Save_FinancialSettingModel_multicurrency_Exisist_Request_Message);
                if (multiCurrency.BaseCurrency != financialSetting.BaseCurrency)
                {
                    multiCurrency.BaseCurrency = financialSetting.BaseCurrency;
                    multiCurrency.ModifiedBy = financialSetting.ModifiedBy;
                    multiCurrency.ModifiedDate = DateTime.UtcNow;
                    SaveMulti(multiCurrency, name);
                }
            }
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_FinancialSetting_Save_FinancialSettingModel_SavedSuccesfully_Request_Message);
            return financialSetting;
        }

        private bool ValidateFainancialSetting(FinancialSetting financialSetting)
        {
            if (financialSetting.PeriodLockDate != null || financialSetting.PeriodEndDate != null)
            {
                if (financialSetting.PeriodLockDatePassword == null || financialSetting.PeriodLockDatePassword == "")
                {
                    //   fsErrorMsg = "Period Lock Date Password is mandatory.";
                    return false;
                }
            }
            if (financialSetting.PeriodEndDate < DateTime.UtcNow && (financialSetting.PeriodLockDatePassword == null || financialSetting.PeriodLockDatePassword == ""))
            {
                // fsErrorMsg = "Period Lock Date Achieved, cannot be saved.";
                return false;
            }
            return true;
        }

        #endregion

        #region MultiCurrency

        public MultiCurrencySetting SaveMulti(MultiCurrencySetting multiCurrencySetting, string name)
        {
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_MultiCurrencySetting_Save_MultiCurrencySettingModel_Request_Message);

            string eventStatus = MasterModuleValidations.Empty;
            string _errors = CommonValidation.ValidateObject(multiCurrencySetting);

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            var _multiCurrencySettingSelect = _multiCurrencySettingService.GetmulticurrencyByIdandcompanyId(multiCurrencySetting.Id, multiCurrencySetting.CompanyId);
            DateTime _date = DateTime.UtcNow;
            if (_multiCurrencySettingSelect != null)
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_MultiCurrencySetting_Save_Update_Block_MultiCurrencySettingModel_Request_Message);
                MultiCurrencySetting _multiCurrencySetting = _multiCurrencySettingSelect;
                _multiCurrencySetting.CompanyId = multiCurrencySetting.CompanyId;
                _multiCurrencySetting.BaseCurrency = multiCurrencySetting.BaseCurrency;
                _multiCurrencySetting.Revaluation = multiCurrencySetting.Revaluation;
                _multiCurrencySetting.ModifiedBy = multiCurrencySetting.ModifiedBy;
                _multiCurrencySetting.ModifiedDate = multiCurrencySetting.ModifiedDate;
                _multiCurrencySetting.UserCreated = multiCurrencySetting.UserCreated;
                _multiCurrencySetting.CreatedDate = multiCurrencySetting.CreatedDate;
                _multiCurrencySetting.Status = multiCurrencySetting.Status;
                eventStatus = MasterModuleValidations.Updated;
                _multiCurrencySetting.ObjectState = ObjectState.Modified;
                _multiCurrencySettingService.Update(_multiCurrencySetting);
            }
            else
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_MultiCurrencySetting_Save_Create_Block_MultiCurrencySettingModel_Request_Message);
                var _multiCurrencyCompany = _multiCurrencySettingService.Getmulticurrency(multiCurrencySetting.CompanyId);
                if (_multiCurrencyCompany == null)
                {
                    long value = 0;
                    var GetAllMultiCurrencySettings = _multiCurrencySettingService.Queryable().ToList();
                    if (GetAllMultiCurrencySettings.Any())
                    {
                        value = Convert.ToInt64(GetAllMultiCurrencySettings.Max(c => c.Id));
                    }

                    //multiCurrencySetting.UserCreated = multiCurrencySetting.UserCreated;
                    multiCurrencySetting.CreatedDate = _date;

                    multiCurrencySetting.Id = (value + 1);
                    //  multiCurrencySetting.EventStatus = "Inserted";
                    multiCurrencySetting.ObjectState = ObjectState.Added;
                    multiCurrencySetting.Status = RecordStatusEnum.Active;
                    multiCurrencySetting.Revaluation = multiCurrencySetting.Revaluation;
                    _multiCurrencySettingService.Insert(multiCurrencySetting);
                    #region MultiCurrency COA inserting
                    using (SqlConnection con = new SqlConnection(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand("Bean_MultiCurrency_COA_Add", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@companyId", multiCurrencySetting.CompanyId);
                        cmd.ExecuteNonQuery();
                        if (con.State != ConnectionState.Closed)
                            con.Close();
                    }
                    #endregion
                    var moduleDetail = _moduleMasterService.GetModuleMaster(multiCurrencySetting.CompanyId, "Bean Cursor");
                    if (moduleDetail != null)
                    {
                        var feature = _companyFeatureService.GetFeature("Multi-Currency", multiCurrencySetting.CompanyId, moduleDetail.Id);
                        if (feature != null)
                        {
                            feature.Status = RecordStatusEnum.Active;
                            feature.ModifiedBy = multiCurrencySetting.ModifiedBy;
                            feature.ModifiedDate = multiCurrencySetting.ModifiedDate;
                            feature.ObjectState = ObjectState.Modified;
                            feature.ObjectState = ObjectState.Modified;
                            _companyFeatureService.Update(feature);


                        }
                    }
                }
                else
                {
                    MultiCurrencySetting _multiCurrencySetting = _multiCurrencySettingService.Getmulticurrency(multiCurrencySetting.CompanyId);
                    _multiCurrencySetting.CompanyId = multiCurrencySetting.CompanyId;
                    _multiCurrencySetting.BaseCurrency = multiCurrencySetting.BaseCurrency;
                    _multiCurrencySetting.Revaluation = multiCurrencySetting.Revaluation;
                    _multiCurrencySetting.ModifiedBy = multiCurrencySetting.ModifiedBy;
                    _multiCurrencySetting.ModifiedDate = multiCurrencySetting.ModifiedDate;
                    _multiCurrencySetting.Status = multiCurrencySetting.Status;
                    _multiCurrencySetting.ObjectState = ObjectState.Modified;
                    _multiCurrencySettingService.Update(_multiCurrencySetting);
                    multiCurrencySetting = _multiCurrencySetting;
                }

            }

            if (multiCurrencySetting.Revaluation == true)
            {
                //for new Cindi Requriment
                Log.Logger.ZInfo(MasterModuleLoggingValidation.Log_MultiCurrencySetting_Save_Revalution_Checked_True_MultiCurrencySettingModel_Request_Message);
                var lstCoa = _chartOfAccountService.Queryable();
                List<ChartOfAccount> lstchartofAccount = _chartOfAccountService.GetChartOfAccountByDisable(0);
                List<ChartOfAccount> lstChartofAccount = lstCoa.Where(a => a.CompanyId == multiCurrencySetting.CompanyId && a.IsRevaluation == 1).ToList();
                bool? isIbEnabled = _interCompanySettingService.GetIBIsActivatedOrNot(multiCurrencySetting.CompanyId, "Billing");
                if (isIbEnabled != true)
                    lstchartofAccount = lstchartofAccount.Where(a => a.Name != "Interco billing revaluation").ToList();

                if (lstChartofAccount.Count == 0)
                {
                    if (lstchartofAccount.Any())
                    {
                        foreach (var chartofaccount in lstchartofAccount)
                        {
                            ChartOfAccount newCOA = new ChartOfAccount();

                            var coa = _chartOfAccountService.CheckName(chartofaccount.Id, chartofaccount.Name, multiCurrencySetting.CompanyId);
                            List<ChartOfAccount> lstCoaCode = _chartOfAccountService.CheckCode(chartofaccount.Code, multiCurrencySetting.CompanyId);
                            var coaCode = lstCoaCode.OrderByDescending(a => a.CreatedDate).Take(1).FirstOrDefault();
                            if (coa != null)
                            {
                                //newCOA.Name = chartofaccount.Name + "-" + 1;
                                int c = 0;
                                string coaName = string.Empty;
                                bool exist = false;
                                while (exist == false)
                                {
                                    c++;
                                    coaName = chartofaccount.Name + "-" + c;
                                    var existName = lstCoa.Where(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Name.ToLower() == coaName.ToLower()).FirstOrDefault();
                                    if (existName == null)
                                        exist = true;
                                }
                                newCOA.Name = coaName;
                            }
                            else
                                newCOA.Name = chartofaccount.Name;

                            if (coaCode != null)
                            {
                                int i = 0;
                                string newCode = string.Empty;
                                bool isExist = false;
                                while (isExist == false)
                                {
                                    i++;
                                    string cCode = coaCode.Code.Substring(2);
                                    long number = Convert.ToInt64(cCode) + i;
                                    newCode = "BS" + number;

                                    var existCode = lstCoa.Where(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Code == newCode).FirstOrDefault();
                                    if (existCode == null)
                                        isExist = true;
                                }
                                newCOA.Code = newCode;

                            }
                            else
                                newCOA.Code = chartofaccount.Code;

                            newCOA.Id = lstCoa.Max(a => a.Id) + 1;
                            newCOA.FRCOAId = Guid.NewGuid();
                            newCOA.FRPATId = _accountTypeService.Query(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Name == chartofaccount.AccountType.Name).Select(a => a.FRATId).FirstOrDefault();
                            newCOA.AccountTypeId = _accountTypeService.Query(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Name == chartofaccount.AccountType.Name).Select(a => a.Id).FirstOrDefault();
                            newCOA.AppliesTo = chartofaccount.AppliesTo;
                            newCOA.CashflowType = chartofaccount.CashflowType;
                            newCOA.Category = chartofaccount.Category;
                            newCOA.Class = chartofaccount.Class;
                            newCOA.CompanyId = multiCurrencySetting.CompanyId;
                            newCOA.CreatedDate = DateTime.UtcNow;
                            newCOA.Currency = chartofaccount.Currency;
                            newCOA.DisAllowable = chartofaccount.DisAllowable;
                            newCOA.IsAllowableNotAllowableActivated = chartofaccount.IsAllowableNotAllowableActivated;
                            newCOA.IsBank = chartofaccount.IsBank;
                            newCOA.IsCodeEditable = chartofaccount.IsCodeEditable;
                            newCOA.IsLinkedAccount = chartofaccount.IsLinkedAccount;
                            newCOA.IsRealCOA = chartofaccount.IsRealCOA;
                            newCOA.IsSeedData = chartofaccount.IsSeedData;
                            newCOA.IsShowforCOA = chartofaccount.IsShowforCOA;
                            newCOA.IsSubLedger = chartofaccount.IsSubLedger;
                            newCOA.IsSystem = chartofaccount.IsSystem;
                            newCOA.ModifiedBy = chartofaccount.ModifiedBy;
                            newCOA.ModifiedDate = chartofaccount.ModifiedDate;
                            newCOA.ModuleType = chartofaccount.ModuleType;
                            newCOA.Nature = chartofaccount.Nature;
                            newCOA.RealisedExchangeGainOrLoss = chartofaccount.RealisedExchangeGainOrLoss;
                            newCOA.RecOrder = chartofaccount.RecOrder;
                            newCOA.Remarks = chartofaccount.Remarks;
                            newCOA.Revaluation = chartofaccount.Revaluation;
                            newCOA.ShowAllowable = chartofaccount.ShowAllowable;
                            newCOA.ShowCashFlow = chartofaccount.ShowCashFlow;
                            newCOA.ShowCurrency = chartofaccount.ShowCurrency;
                            newCOA.ShowRevaluation = chartofaccount.ShowRevaluation;
                            newCOA.SubCategory = chartofaccount.SubCategory;
                            newCOA.UserCreated = multiCurrencySetting.UserCreated;
                            newCOA.Version = chartofaccount.Version;

                            newCOA.Status = RecordStatusEnum.Active;

                            newCOA.IsRevaluation = multiCurrencySetting.Revaluation == true ? 1 : 0;
                            newCOA.ObjectState = ObjectState.Added;
                            _chartOfAccountService.Insert(newCOA);
                            try
                            {
                                _unitOfWorkAsync.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                Log.Logger.ZInfo(ex.Message, newCOA.Name);
                            }
                        }
                    }
                }
            }
            if (multiCurrencySetting.Revaluation == false)
            {
                List<ChartOfAccount> lstchartofAccount = _chartOfAccountService.GetChartOfAccountByRevaluation(multiCurrencySetting.CompanyId);
                if (lstchartofAccount.Any())
                {
                    foreach (var chartofaccount in lstchartofAccount)
                    {
                        chartofaccount.Status = RecordStatusEnum.Disable;
                        chartofaccount.IsRevaluation = 3;
                        chartofaccount.ObjectState = ObjectState.Deleted;
                    }
                }
            }

            #region Coomented_code
            //if (multiCurrencySetting.Revaluation == true)
            //{
            //     LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,MasterModuleLoggingValidation.Log_MultiCurrencySetting_Save_Revalution_Checked_True_MultiCurrencySettingModel_Request_Message);
            //    var lstCoa = _chartOfAccountService.Queryable();
            //    List<ChartOfAccount> lstchartofAccount = _chartOfAccountService.GetChartOfAccountByDisable(0);
            //    List<ChartOfAccount> lstChartofAccount = lstCoa.Where(a => a.CompanyId == multiCurrencySetting.CompanyId && a.IsRevaluation == 1).ToList();

            //    if (lstChartofAccount.Count == 0)
            //    {
            //        if (lstchartofAccount.Any())
            //        {
            //            foreach (var chartofaccount in lstchartofAccount)
            //            {
            //                ChartOfAccount newCOA = new ChartOfAccount();

            //                var coa = _chartOfAccountService.CheckName(chartofaccount.Id, chartofaccount.Name, multiCurrencySetting.CompanyId);
            //                List<ChartOfAccount> lstCoaCode = _chartOfAccountService.CheckCode(chartofaccount.Code, multiCurrencySetting.CompanyId);
            //                var coaCode = lstCoaCode.OrderByDescending(a => a.CreatedDate).Take(1).FirstOrDefault();
            //                if (coa != null)
            //                {
            //                    //newCOA.Name = chartofaccount.Name + "-" + 1;
            //                    int c = 0;
            //                    string coaName = string.Empty;
            //                    bool exist = false;
            //                    while (exist == false)
            //                    {
            //                        c++;
            //                        coaName = chartofaccount.Name + "-" + c;
            //                        var existName = lstCoa.Where(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Name.ToLower() == coaName.ToLower()).FirstOrDefault();
            //                        if (existName == null)
            //                            exist = true;
            //                    }
            //                    newCOA.Name = coaName;
            //                }
            //                else
            //                    newCOA.Name = chartofaccount.Name;

            //                if (coaCode != null)
            //                {
            //                    int i = 0;
            //                    string newCode = string.Empty;
            //                    bool isExist = false;
            //                    while (isExist == false)
            //                    {
            //                        i++;
            //                        string cCode = coaCode.Code.Substring(2);
            //                        long number = Convert.ToInt64(cCode) + i;
            //                        newCode = "BS" + number;

            //                        var existCode = lstCoa.Where(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Code == newCode).FirstOrDefault();
            //                        if (existCode == null)
            //                            isExist = true;
            //                    }
            //                    newCOA.Code = newCode;

            //                }
            //                else
            //                    newCOA.Code = chartofaccount.Code;

            //                newCOA.Id = lstCoa.Max(a => a.Id) + 1;
            //                newCOA.AccountTypeId = _accountTypeService.Query(a => a.CompanyId == multiCurrencySetting.CompanyId && a.Name == chartofaccount.AccountType.Name).Select(a => a.Id).FirstOrDefault();
            //                newCOA.AppliesTo = chartofaccount.AppliesTo;
            //                newCOA.CashflowType = chartofaccount.CashflowType;
            //                newCOA.Category = chartofaccount.Category;
            //                newCOA.Class = chartofaccount.Class;
            //                newCOA.CompanyId = multiCurrencySetting.CompanyId;
            //                newCOA.CreatedDate = DateTime.UtcNow;
            //                newCOA.Currency = chartofaccount.Currency;
            //                newCOA.DisAllowable = chartofaccount.DisAllowable;
            //                newCOA.IsAllowableNotAllowableActivated = chartofaccount.IsAllowableNotAllowableActivated;
            //                newCOA.IsBank = chartofaccount.IsBank;
            //                newCOA.IsCodeEditable = chartofaccount.IsCodeEditable;
            //                newCOA.IsLinkedAccount = chartofaccount.IsLinkedAccount;
            //                newCOA.IsRealCOA = chartofaccount.IsRealCOA;
            //                newCOA.IsSeedData = chartofaccount.IsSeedData;
            //                newCOA.IsShowforCOA = chartofaccount.IsShowforCOA;
            //                newCOA.IsSubLedger = chartofaccount.IsSubLedger;
            //                newCOA.IsSystem = chartofaccount.IsSystem;
            //                newCOA.ModifiedBy = chartofaccount.ModifiedBy;
            //                newCOA.ModifiedDate = chartofaccount.ModifiedDate;
            //                newCOA.ModuleType = chartofaccount.ModuleType;
            //                newCOA.Nature = chartofaccount.Nature;
            //                newCOA.RealisedExchangeGainOrLoss = chartofaccount.RealisedExchangeGainOrLoss;
            //                newCOA.RecOrder = chartofaccount.RecOrder;
            //                newCOA.Remarks = chartofaccount.Remarks;
            //                newCOA.Revaluation = chartofaccount.Revaluation;
            //                newCOA.ShowAllowable = chartofaccount.ShowAllowable;
            //                newCOA.ShowCashFlow = chartofaccount.ShowCashFlow;
            //                newCOA.ShowCurrency = chartofaccount.ShowCurrency;
            //                newCOA.ShowRevaluation = chartofaccount.ShowRevaluation;
            //                newCOA.SubCategory = chartofaccount.SubCategory;
            //                newCOA.UserCreated = multiCurrencySetting.UserCreated;
            //                newCOA.Version = chartofaccount.Version;

            //                newCOA.Status = RecordStatusEnum.Active;
            //                newCOA.IsRevaluation = 1;
            //                newCOA.ObjectState = ObjectState.Added;
            //                _chartOfAccountService.Insert(newCOA);

            //                _unitOfWorkAsync.SaveChanges();
            //            }
            //        }
            //    }
            //}
            ////commented by pradhan

            //if (multiCurrencySetting.Revaluation == false)
            //{
            //    List<ChartOfAccount> lstchartofAccount = _chartOfAccountService.GetChartOfAccountByRevaluation(multiCurrencySetting.CompanyId);
            //    if (lstchartofAccount.Any())
            //    {
            //        foreach (var chartofaccount in lstchartofAccount)
            //        {
            //            chartofaccount.Status = RecordStatusEnum.Disable;
            //            chartofaccount.IsRevaluation = 3;
            //            chartofaccount.ObjectState = ObjectState.Deleted;
            //        }
            //    }
            //}
            #endregion

            try
            {
                _unitOfWorkAsync.SaveChanges();

                //awasthy curser initial setup
                if (multiCurrencySetting.ModuleDetailId != null)
                    UpadateInitialSetup(multiCurrencySetting.CompanyId, multiCurrencySetting.ModuleDetailId.Value);

                //if (eventStatus == MasterModuleValidations.Updated)
                //{
                //    DomainEventChannel.Raise(new MultiCurrencySettingUpdated(multiCurrencySetting));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new MultiCurrencySettingCreated(multiCurrencySetting));
                //}
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_MultiCurrencySetting_Save_MultiCurrencySettingModel_saved_SuccessFully_Request_Message);
            return multiCurrencySetting;
        }

        public bool ActivateMultiCurrencyModule(long CompanyId)
        {
            CompanySetting setting = _companySettingService.ActivateModule(ModuleNameConstants.MultiCurrency, CompanyId);
            return ActivateModule(setting);
        }

        public MultiCurrencySetting GetAllMultiCurrencySettings(long CompanyId)
        {
            MultiCurrencySetting MultiCurrencySetting = _multiCurrencySettingService.Getmulticurrency(CompanyId);

            bool? ifExists = false;
            List<long> lstCoaId = _chartOfAccountService.GetChartOfAccountId(CompanyId);
            if (lstCoaId.Any())
            {
                ifExists = _journalDetailService.GetAllByCoaId(lstCoaId);
            }
            else
                ifExists = false;


            // MultiCurrencySetting.IsEdit = MultiCurrencySetting == null ? false : true;
            if (MultiCurrencySetting != null)
            {
                MultiCurrencySetting.IsEdit = MultiCurrencySetting.UserCreated == null ? false : true;
                MultiCurrencySetting.IsRevalutionCoaPosted = ifExists;
            }
            else
            {
                MultiCurrencySetting = new MultiCurrencySetting();
                MultiCurrencySetting.IsEdit = false;
                MultiCurrencySetting.IsRevalutionCoaPosted = ifExists;
                MultiCurrencySetting.CreatedDate = DateTime.UtcNow;
            }
            return MultiCurrencySetting;
        }
        public bool GetMultiCurrencyModuleStatus(long companyId)
        {
            //  CompanySetting MultiCurrencyModuleStatus = _companySettingService.ActivateModuleM(ModuleNameConstants.MultiCurrency, companyId);
            MultiCurrencySetting multi = _multiCurrencySettingService.Getmulticurrency(companyId);
            return multi != null ? true : false;
        }


        #endregion

        #region Currency

        public List<Currency> GetAllCurrencyes(long companyId)
        {
            List<Currency> currencys = new List<Currency>();
            List<Currency> lstcurrencies = _currencyService.GetByCurrencyById(companyId);
            foreach (Currency currency in lstcurrencies)
            {
                currency.DefaultCurrency = MasterModuleValidations.SGD;
                currencys.Add(currency);
            }
            //return currencys.AsEnumerable<Currency>().OrderBy(a => a.Status).ThenBy(a => a.Name).ToList(); 
            return currencys.AsQueryable<Currency>().OrderBy(a => a.Status).ThenBy(a => a.Name).ToList();

        }

        public IQueryable<Currency> GetAllCurrency(long companyId)
        {
            return _currencyService.GetAllCurrency(companyId).AsQueryable();
        }

        #endregion

        #region GstSetting

        public GSTSetting GetGstSettingcompany(long companyId)
        {
            GSTSetting Gst = _gSTSettingService.GetGSTSettingByCompanyId(companyId);
            if (Gst != null)
            {
                var journal = _journalService.GetByCompanyId(companyId);
                Gst.IsCurrencyEditable = journal != null ? false : true;
            }
            return Gst;
        }

        public GSTSetting SaveGst(GSTSetting gstSetting, string ConnectionString)
        {
            string RecStatus = MasterModuleValidations.Empty;
            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_GSTSetting_Save_GSTSettingModel_Request_Message);
            string deregistrationStatus = MasterModuleValidations.Empty;
            string gSTReportingCurrencyStatus = MasterModuleValidations.Empty;
            string _errors = CommonValidation.ValidateObject(gstSetting);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            var _gstSettingSelect = _gSTSettingService.GetGSTSettingByCIdAndId(gstSetting.Id, gstSetting.CompanyId);
            DateTime _date = DateTime.UtcNow;
            GSTSetting _gstSetting = new GSTSetting();

            if (_gstSettingSelect != null)
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_GSTSetting_Save_Update_GSTSettingModel_Request_Message);
                _gstSetting = _gstSettingSelect;

                _gstSetting.CompanyId = gstSetting.CompanyId;
                _gstSetting.Number = gstSetting.Number;

                if (gstSetting.DeRegistration != null && !string.IsNullOrEmpty(gstSetting.DeRegistration.ToString()))
                {
                    if (ValidateDeGenerationDate(gstSetting.DeRegistration, gstSetting.CompanyId, ConnectionString))
                        throw new Exception(MasterModuleValidations.It_is_overlapping_the_existing_document_dates);
                    else
                        _gstSetting.DeRegistration = gstSetting.DeRegistration;
                }
                if (_gstSetting.DeRegistration != gstSetting.DeRegistration)
                {
                    deregistrationStatus = MasterModuleValidations.Updated;
                }
                _gstSetting.DateOfRegistration = gstSetting.DateOfRegistration;
                _gstSetting.ReportingInterval = gstSetting.ReportingInterval;
                _gstSetting.ReportingYearEnd = gstSetting.ReportingYearEnd;
                _gstSetting.ModifiedBy = gstSetting.ModifiedBy;
                _gstSetting.ModifiedDate = _date;
                _gstSetting.IsDeregistered = gstSetting.IsDeregistered;
                _gstSetting.Status = gstSetting.Status;
                if (_gstSetting.GSTRepoCurrency != gstSetting.GSTRepoCurrency)
                {
                    gSTReportingCurrencyStatus = MasterModuleValidations.Updated;
                }
                _gstSetting.GSTRepoCurrency = gstSetting.GSTRepoCurrency;
                RecStatus = MasterModuleValidations.Updated;

                _gstSetting.ObjectState = ObjectState.Modified;
                _gSTSettingService.Update(_gstSetting);
            }
            else
            {
                LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_GSTSetting_Save_Create_GSTSettingModel_Request_Message);
                var _gstCompany = _gSTSettingService.GetGSTSettingByCompanyId(gstSetting.CompanyId);
                CompanySetting setting = _companySettingService.ActivateModule(ModuleNameConstants.GST, gstSetting.CompanyId);
                if (setting != null)
                {
                    setting.IsEnabled = true;
                    setting.ObjectState = ObjectState.Modified;
                    _companySettingService.Update(setting);
                    // _unitOfWorkAsync.SaveChanges();
                }
                if (_gstCompany == null)
                {
                    long value = 0;
                    List<GSTSetting> GetAllGSTSettings = _gSTSettingService.Queryable().ToList();
                    if (GetAllGSTSettings.Any())
                    {
                        value = Convert.ToInt64(GetAllGSTSettings.Max(c => c.Id));
                    }
                    gstSetting.Status = RecordStatusEnum.Active;
                    //gstSetting.UserCreated = gstSetting.UserCreated;
                    gstSetting.CreatedDate = _date;

                    gstSetting.Id = (value + 1);
                    gstSetting.ObjectState = ObjectState.Added;
                    _gSTSettingService.Insert(gstSetting);
                    var moduleDetail = _moduleMasterService.GetModuleMaster(gstSetting.CompanyId, "Bean Cursor");
                    if (moduleDetail != null)
                    {
                        var feature = _companyFeatureService.GetFeature("GST", gstSetting.CompanyId, moduleDetail.Id);
                        if (feature != null)
                        {
                            feature.Status = RecordStatusEnum.Active;
                            feature.ObjectState = ObjectState.Modified;
                            _companyFeatureService.Update(feature);
                        }
                    }

                }
                else
                {
                    _gstSetting = _gSTSettingService.GetGSTSettingByCompanyId(gstSetting.CompanyId);
                    _gstSetting.CompanyId = gstSetting.CompanyId;
                    _gstSetting.Number = gstSetting.Number;
                    _gstSetting.DateOfRegistration = gstSetting.DateOfRegistration;
                    _gstSetting.DeRegistration = gstSetting.DeRegistration;
                    _gstSetting.ReportingInterval = gstSetting.ReportingInterval;
                    _gstSetting.ReportingYearEnd = gstSetting.ReportingYearEnd;
                    _gstSetting.ModifiedBy = gstSetting.ModifiedBy;
                    _gstSetting.ModifiedDate = _date;
                    _gstSetting.IsDeregistered = gstSetting.IsDeregistered;
                    _gstSetting.Status = gstSetting.Status;
                    _gstSetting.GSTRepoCurrency = gstSetting.GSTRepoCurrency;
                    _gstSetting.ObjectState = ObjectState.Modified;
                    _gSTSettingService.Update(_gstSetting);
                    gstSetting = _gstSetting;
                }
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                //if (gSTReportingCurrencyStatus == MasterModuleValidations.Updated)
                //{
                //    DomainEventChannel.Raise(new GSTReportingCurrencyCreated(gstSetting));
                //}
                //if (deregistrationStatus == MasterModuleValidations.Updated)
                //{
                //    DomainEventChannel.Raise(new DeRegristationCreated(gstSetting));
                //}

                //if (RecStatus == MasterModuleValidations.Updated)
                //{
                //    DomainEventChannel.Raise(new GSTSettingUpdated(gstSetting));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new GSTSettingCreated(gstSetting));
                //}
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }

            LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_GSTSetting_Save_GSTSettingModel_Saved_Successfully_Request_Message);
            return gstSetting;
        }

        public bool GetGSTSettingByCompanyIdAndDate(long companyId, DateTime docDate, long serviceCompanyId)
        {
            //GstModel gst = new GstModel();
            GSTSetting setting = _gSTSettingService.GetGSTSetting(companyId, docDate, serviceCompanyId);
            bool isGST = setting != null;
            if (isGST)
            {
                if (setting.IsDeregistered != null && setting.IsDeregistered.Value)
                {
                    isGST = docDate < setting.DeRegistration.Value;
                }
                //gst.IsGstActive = isGST;
                //gst.GstExCurrency = setting.GSTRepoCurrency;
            }
            return isGST;
        }

        private bool ValidateDeGenerationDate(DateTime? date, long CompanyId, string ConnectionString)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                SqlCommand command = new SqlCommand("SP_UAT_IsDeRegistrationDateIsValid", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@DATE", date);
                command.Parameters.Add("@COMPANY_ID", CompanyId);
                command.CommandTimeout = 0;
                DataSet data = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(data);
                int count = data.Tables[0].Rows.Count;
                //return count>0 ?true:false;
                return count > 0;

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        public bool GetModuleStatus(string moduleName, long companyId)
        {
            CompanySetting setting = _companySettingService.ActivateModuleM(moduleName, companyId);
            if (setting != null)
                return setting.IsEnabled;
            return false;
        }

        public bool ActivateModule(string moduleName, long companyId)
        {
            CompanySetting setting = _companySettingService.ActivateModule(moduleName, companyId);
            if (setting != null)
            {
                setting.IsEnabled = true;
                setting.ObjectState = ObjectState.Modified;
                _companySettingService.Update(setting);
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            return false;
        }


        #endregion

        #region AllowableDisallowable

        public bool GetModuleStatus(long companyId)
        {
            CompanySetting setting = _companySettingService.ActivateModuleM(ModuleNameConstants.AllowableNonAllowable, companyId);
            return setting != null ? setting.IsEnabled : false;
        }

        #endregion

        #region Nosupporting

        public bool GetNoSupportingDocuments(long companyId)
        {
            CompanySetting noSupportingDocument = _companySettingService.ActivateModuleM(ModuleNameConstants.NoSupportingDocuments, companyId);
            return noSupportingDocument != null ? noSupportingDocument.IsEnabled : false;
        }

        #endregion

        #region BankReconciliationSetting

        //public BankReconciliationSetting GetBankReconciliationBYCompany(long companyId)
        //{
        //    return _bankReconciliationSettingService.GetByCompanyId(companyId);
        //}

        public bool GetModuleStatuss(long companyId)
        {
            CompanySetting setting = _companySettingService.ActivateModuleM(ModuleNameConstants.BankReconciliation, companyId);
            return setting != null ? setting.IsEnabled : false;
        }

        //public BankReconciliationSetting Save(BankReconciliationSetting bankReconciliation)
        //{
        //    // string RecStatus=;
        //    LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BankReconciliationSetting_Save_BankReconciliationSettingModel_Request_Message);
        //    string _errors = CommonValidation.ValidateObject(bankReconciliation);
        //    if (!string.IsNullOrEmpty(_errors))
        //    {
        //        throw new Exception(_errors);
        //    }
        //    var __bankReconciliationSelect = _bankReconciliationSettingService.GetByCompanyIdAndId(bankReconciliation.Id, bankReconciliation.CompanyId);
        //    DateTime _date = DateTime.UtcNow;
        //    BankReconciliationSetting __bankReconciliation = new BankReconciliationSetting();
        //    if (__bankReconciliationSelect != null)
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BankReconciliationSetting_Save_Updated_BankReconciliationSettingModel_Request_Message);
        //        __bankReconciliation = __bankReconciliationSelect;
        //        __bankReconciliation.CompanyId = bankReconciliation.CompanyId;
        //        __bankReconciliation.BankClearingDate = bankReconciliation.BankClearingDate;
        //        __bankReconciliation.ModifiedBy = bankReconciliation.ModifiedBy;
        //        __bankReconciliation.ModifiedDate = _date;
        //        __bankReconciliation.Status = RecordStatusEnum.Active;
        //        __bankReconciliation.ObjectState = ObjectState.Modified;
        //        _bankReconciliationSettingService.Update(__bankReconciliation);
        //    }
        //    else
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BankReconciliationSetting_Save_Created_BankReconciliationSettingModel_Request_Message);
        //        var _gbankreconciliationCompany = _bankReconciliationSettingService.GetByCompanyId(bankReconciliation.CompanyId);
        //        _companySettingService.ActivateModule(ModuleNameConstants.BankReconciliation, bankReconciliation.CompanyId);
        //        if (_gbankreconciliationCompany == null)
        //        {
        //            long value = 0;
        //            var GetAllBankReconciliation = _bankReconciliationSettingService.Queryable().ToList();
        //            if (GetAllBankReconciliation.Any())
        //            {
        //                value = Convert.ToInt64(GetAllBankReconciliation.Max(c => c.Id));
        //            }
        //            //bankReconciliation.UserCreated = bankReconciliation.UserCreated;
        //            bankReconciliation.CreatedDate = _date;
        //            bankReconciliation.Id = (value + 1);
        //            //bankReconciliation.BankClearingDate = bankReconciliation.BankClearingDate;
        //            bankReconciliation.ObjectState = ObjectState.Added;
        //            bankReconciliation.Status = RecordStatusEnum.Active;
        //            _bankReconciliationSettingService.Insert(bankReconciliation);
        //            var moduleDetail = _moduleMasterService.GetModuleMaster(bankReconciliation.CompanyId, "Bean Cursor");
        //            if (moduleDetail != null)
        //            {
        //                var feature = _companyFeatureService.GetFeature("Bank Reconciliation", bankReconciliation.CompanyId, moduleDetail.Id);
        //                if (feature != null)
        //                {
        //                    feature.Status = RecordStatusEnum.Active;
        //                    feature.ObjectState = ObjectState.Modified;
        //                    _companyFeatureService.Update(feature);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            __bankReconciliation = _bankReconciliationSettingService.GetByCompanyId(bankReconciliation.CompanyId);
        //            __bankReconciliation.CompanyId = bankReconciliation.CompanyId;
        //            __bankReconciliation.BankClearingDate = bankReconciliation.BankClearingDate;
        //            __bankReconciliation.ModifiedBy = bankReconciliation.ModifiedBy;
        //            __bankReconciliation.ModifiedDate = _date;
        //            __bankReconciliation.Status = RecordStatusEnum.Active;
        //            __bankReconciliation.ObjectState = ObjectState.Modified;
        //            _bankReconciliationSettingService.Update(__bankReconciliation);
        //            bankReconciliation = __bankReconciliation;
        //        }
        //    }
        //    try
        //    {
        //        _unitOfWorkAsync.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        throw ex;
        //    }
        //    LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BankReconciliationSetting_Save__BankReconciliationSettingModel_Saved_Succefully_Request_Message);
        //    return bankReconciliation;
        //}

        #endregion

        #region SegmentMaster

        //public IEnumerable<SegmentMasterDTO> GetAllMasters(long CompanyId)
        //{
        //    List<SegmentMaster> segmentMasters = _segmentMasterService.GetAllSegmentMasters(CompanyId).ToList();
        //    List<SegmentMasterDTO> lstSegmentMasterDTO = new List<SegmentMasterDTO>();
        //    foreach (SegmentMaster segmentMaster in segmentMasters)
        //    {
        //        SegmentMasterDTO segmentMasterDTO = new SegmentMasterDTO();
        //        segmentMasterDTO.Id = segmentMaster.Id;
        //        segmentMasterDTO.CompanyId = segmentMaster.CompanyId;
        //        segmentMasterDTO.IsSystem = segmentMaster.IsSystem;
        //        segmentMasterDTO.ModifiedBy = segmentMaster.ModifiedBy;
        //        segmentMasterDTO.ModifiedDate = segmentMaster.ModifiedDate;
        //        segmentMasterDTO.UserCreated = segmentMaster.UserCreated;
        //        segmentMasterDTO.CreatedDate = segmentMaster.CreatedDate;
        //        segmentMasterDTO.Name = segmentMaster.Name;
        //        segmentMasterDTO.RecOrder = segmentMaster.RecOrder;
        //        segmentMasterDTO.Status = segmentMaster.Status;
        //        segmentMasterDTO.Version = segmentMaster.Version;
        //        List<SegmentDetail> SegmentDetails = _segmentDetailService.GetSegmentDetails(segmentMaster.Id);
        //        List<SegmentDetailDTO> lstServices = new List<SegmentDetailDTO>();
        //        foreach (var serv in SegmentDetails)
        //        {
        //            //serv.SegmentMaster = null;
        //            SegmentDetailDTO segDetailsDTO = new SegmentDetailDTO();
        //            segDetailsDTO.Id = serv.Id;
        //            segDetailsDTO.CreatedDate = serv.CreatedDate;
        //            segDetailsDTO.IsSystem = serv.IsSystem;
        //            segDetailsDTO.SegmentMasterId = serv.SegmentMasterId;
        //            segDetailsDTO.ModifiedBy = serv.ModifiedBy;
        //            segDetailsDTO.ModifiedDate = serv.ModifiedDate;
        //            segDetailsDTO.Name = serv.Name;
        //            segDetailsDTO.ObjectState = serv.ObjectState;
        //            segDetailsDTO.ParentId = serv.ParentId;
        //            segDetailsDTO.RecOrder = serv.RecOrder;
        //            segDetailsDTO.Remarks = serv.Remarks;
        //            segDetailsDTO.Status = serv.Status;
        //            var segmentDetails = _segmentDetailService.GetSegmentDetail(segmentMaster.Id, serv.Id);
        //            List<SegmentDetail> lstServicesDetail = new List<SegmentDetail>();
        //            foreach (var servDetail in segmentDetails)
        //            {
        //                lstServicesDetail.Add(servDetail);
        //            }
        //            segDetailsDTO.SegmentDetails = lstServicesDetail;
        //            lstServices.Add(segDetailsDTO);
        //        }
        //        segmentMasterDTO.SegmentDetailsDTO = lstServices;
        //        lstSegmentMasterDTO.Add(segmentMasterDTO);
        //    }
        //    return lstSegmentMasterDTO.AsEnumerable().OrderByDescending(c => c.CreatedDate);
        //}

        public bool GetModuleStatu(long companyId)
        {
            CompanySetting setting = _companySettingService.ActivateModuleM(ModuleNameConstants.SegmentReporting, companyId);
            return setting != null ? setting.IsEnabled : false;
        }

        //public SegmentMasterDTO SaveSegmentMaster(SegmentMasterDTO segmentMasterDTO)
        //{
        //    LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SaveSegmentMaster_Save_SaveSegmentMasterModel_Request_Message);
        //    var _segmentMasterSelect = _segmentMasterService.GetByCidAndId(segmentMasterDTO.Id, segmentMasterDTO.CompanyId);
        //    var _segmentName = _segmentMasterService.GetByCidAndname(segmentMasterDTO.Name, segmentMasterDTO.CompanyId);
        //    var segmentName = _segmentMasterService.GetByIdAndNameAndCid(segmentMasterDTO.Id, segmentMasterDTO.Name, segmentMasterDTO.CompanyId);
        //    DateTime _date = DateTime.UtcNow;
        //    string eventstatus = string.Empty;
        //    long masterId;

        //    foreach (var SegmentDetail in segmentMasterDTO.SegmentDetailsDTO)
        //    {
        //        var delsegde = SegmentDetail.SegmentDetails.Where(c => c.Status == RecordStatusEnum.Delete).ToList();
        //        if (delsegde.Any())
        //        {
        //            foreach (var detail in delsegde)
        //            {
        //                var deta = _segmentDetailService.GetById(detail.Id);
        //                deta.Status = RecordStatusEnum.Delete;
        //                _segmentDetailService.Update(deta);
        //            }
        //        }
        //        if (SegmentDetail.Status == RecordStatusEnum.Delete)
        //        {
        //            if (SegmentDetail.Id != -1)
        //            {
        //                var segdel = _segmentDetailService.GetById(SegmentDetail.Id);
        //                segdel.Status = RecordStatusEnum.Delete;
        //                _segmentDetailService.Update(segdel);
        //                foreach (var detail in SegmentDetail.SegmentDetails)
        //                {
        //                    var deta = _segmentDetailService.GetById(detail.Id);
        //                    deta.Status = RecordStatusEnum.Delete;
        //                    _segmentDetailService.Update(deta);
        //                }
        //            }
        //        }
        //    }

        //    _unitOfWorkAsync.SaveChanges();
        //    if (_segmentMasterSelect != null && !segmentName.Any() && !_segmentName.Any())
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SaveSegmentMaster_Save_segmentMasterSelect_Not_equal_To_Null_Request_Message);
        //        var lstCnt = _segmentMasterService.GetSegmentS(segmentMasterDTO.Id, segmentMasterDTO.CompanyId);
        //        if (lstCnt.Count >= 2)
        //        {
        //            throw new Exception(MasterModuleValidations.You_Have_Already_Two_Activated_Segment_Masters);
        //        }
        //        if (segmentMasterDTO.Name == MasterModuleValidations.Empty)
        //            throw new Exception(MasterModuleValidations.Segment_Category_is_required);
        //        masterId = segmentMasterDTO.Id;
        //        SegmentMaster _segmentMaster = _segmentMasterSelect;
        //        FillSegment(segmentMasterDTO, _segmentMaster);
        //        _segmentMaster.ModifiedDate = segmentMasterDTO.ModifiedDate;
        //        _segmentMaster.ModifiedBy = segmentMasterDTO.ModifiedBy;
        //        _segmentMaster.ObjectState = ObjectState.Modified;
        //        _segmentMasterService.Update(_segmentMaster);
        //    }
        //    else if (_segmentMasterSelect != null && segmentName.Any())
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SaveSegmentMaster_Save_segmentMaster_First_Else_If_Request_Message);
        //        var lstCnt = _segmentMasterService.GetSegmentS(segmentMasterDTO.Id, segmentMasterDTO.CompanyId);
        //        if (lstCnt.Count >= 2)
        //        {
        //            throw new Exception(MasterModuleValidations.You_Have_Already_Two_Activated_Segment_Masters);
        //        }
        //        if (segmentMasterDTO.Name == MasterModuleValidations.Empty)
        //            throw new Exception(MasterModuleValidations.Segment_Category_is_required);
        //        masterId = segmentMasterDTO.Id;
        //        var _segmentMaster = _segmentMasterSelect;
        //        if (_segmentMaster == null)
        //            _segmentMaster = new SegmentMaster();
        //        FillSegment(segmentMasterDTO, _segmentMaster);
        //        _segmentMaster.ModifiedDate = segmentMasterDTO.ModifiedDate;
        //        _segmentMaster.ModifiedBy = segmentMasterDTO.ModifiedBy;
        //        _segmentMaster.ObjectState = ObjectState.Modified;
        //        _segmentMasterService.Update(_segmentMaster);
        //    }
        //    else if (_segmentMasterSelect == null && !_segmentName.Any())
        //    {
        //        eventstatus = "insert";
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SaveSegmentMaster_Save_segmentMaster_Second_Else_If_Request_Message);
        //        //var lstCnt = _segmentMasterService.GetByCid(segmentMasterDTO.CompanyId);
        //        var lstCnt = _segmentMasterService.GetSegmentS(segmentMasterDTO.Id, segmentMasterDTO.CompanyId);
        //        if (lstCnt.Count >= 2)
        //        {
        //            throw new Exception(MasterModuleValidations.You_Have_Already_Two_Activated_Segment_Masters);
        //        }
        //        if (segmentMasterDTO.Name == MasterModuleValidations.Empty)
        //            throw new Exception(MasterModuleValidations.Segment_Category_is_required);
        //        long value = 0;
        //        var GetAllSegmentMasters = _segmentMasterService.Queryable().ToList();
        //        if (GetAllSegmentMasters.Any())
        //        {
        //            value = Convert.ToInt64(GetAllSegmentMasters.Max(c => c.Id));
        //            segmentMasterDTO.Id = value + 1;
        //        }
        //        SegmentMaster _segmentMaster = new SegmentMaster();
        //        _segmentMaster.CompanyId = segmentMasterDTO.CompanyId;
        //        _segmentMaster.IsSystem = segmentMasterDTO.IsSystem;
        //        _segmentMaster.Name = segmentMasterDTO.Name;
        //        _segmentMaster.RecOrder = segmentMasterDTO.RecOrder;
        //        _segmentMaster.Status = segmentMasterDTO.Status;
        //        _segmentMaster.Version = segmentMasterDTO.Version;
        //        _segmentMaster.CreatedDate = DateTime.UtcNow;
        //        _segmentMaster.UserCreated = segmentMasterDTO.UserCreated;
        //        masterId = (value + 1);
        //        _segmentMaster.Id = masterId;
        //        _segmentMaster.ObjectState = ObjectState.Added;
        //        _segmentMasterService.Insert(_segmentMaster);
        //        var moduleDetail = _moduleMasterService.GetModuleMaster(segmentMasterDTO.CompanyId, "Bean Cursor");
        //        if (moduleDetail != null)
        //        {
        //            var feature = _companyFeatureService.GetFeature("SegmentReporting", segmentMasterDTO.CompanyId, moduleDetail.Id);
        //            if (feature != null)
        //            {
        //                feature.Status = RecordStatusEnum.Active;
        //                feature.ObjectState = ObjectState.Modified;
        //                _companyFeatureService.Update(feature);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        throw new Exception(MasterModuleValidations.Segment_Name_Already_Exist);
        //    }
        //    if (segmentMasterDTO.SegmentDetailsDTO == null)
        //        segmentMasterDTO.SegmentDetailsDTO = new List<SegmentDetailDTO>();
        //    List<SegmentDetailDTO> SegmentDetails = segmentMasterDTO.SegmentDetailsDTO.Where(c => c.Status == RecordStatusEnum.Active).ToList();
        //    var qryParentName = SegmentDetails.GroupBy(x => x.Name).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
        //    if (qryParentName.Count > 0)
        //        throw new Exception(MasterModuleValidations.Segment_Parent_Name_Already_Exist);
        //    foreach (SegmentDetailDTO parent in segmentMasterDTO.SegmentDetailsDTO)
        //    {
        //        parent.SegmentDetails = parent.SegmentDetails.Where(c => c.Status == RecordStatusEnum.Active).ToList();
        //        var qryChildName = parent.SegmentDetails.GroupBy(x => x.Name).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
        //        if (qryChildName.Count > 0)
        //            throw new Exception(MasterModuleValidations.Segment_Child_Name_Already_Exist);
        //    }
        //    SaveSegmentDetail(segmentMasterDTO.SegmentDetailsDTO, masterId);
        //    try
        //    {
        //        _unitOfWorkAsync.SaveChanges();

        //        //awasthy curser Initial SetUp
        //        if (segmentMasterDTO.ModuleDetailId != null)
        //            UpadateInitialSetup(segmentMasterDTO.CompanyId, segmentMasterDTO.ModuleDetailId.Value);

        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SaveSegmentMaster_Save_segmentMaster_Saved_SuccessFully_Request_Message);
        //        //if (eventstatus == string.Empty)
        //        //{
        //        //    DomainEventChannel.Raise(new SegmentMasterUpdated(segmentMasterDTO));
        //        //}
        //        //else
        //        //{
        //        //    DomainEventChannel.Raise(new SegmentMasterCreated(segmentMasterDTO));
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        throw new Exception(MasterModuleValidations.Save_Data_Failed);

        //    }

        //    segmentMasterDTO.Id = masterId;

        //    return segmentMasterDTO;
        //}

        //public void SaveSegmentDetail(List<SegmentDetailDTO> lstSegmentDetail, long masterId)
        //{
        //    LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_BankReconciliationSetting_Save_SaveSegmentDetailModel_Request_Message);
        //    long value = 0;
        //    var GetAllSegmentDetailsMaxId = _segmentDetailService.Queryable().ToList();
        //    if (GetAllSegmentDetailsMaxId.Any())
        //    {
        //        value = Convert.ToInt64(GetAllSegmentDetailsMaxId.Max(c => c.Id));
        //    }
        //    List<SegmentDetail> DetailMaster = new List<SegmentDetail>();
        //    foreach (SegmentDetailDTO segmentDetail in lstSegmentDetail)
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SegmentDetail_Save_SaveSegmentDetailModel_Exissist_Request_Message);
        //        if (segmentDetail.Name == MasterModuleValidations.Empty)
        //            throw new Exception(MasterModuleValidations.Segment_Category_parent_node_is_required);
        //        var segmentDetailNameExist = _segmentDetailService.Getby(segmentDetail.Id, segmentDetail.SegmentMasterId, segmentDetail.Name, segmentDetail.ParentId);
        //        var segmentNameDeleteCheck1 = _segmentDetailService.getByHuge(segmentDetail.Name, segmentDetail.ParentId, segmentDetail.SegmentMasterId);
        //        //SegmentDetail _segmentDetail = new SegmentDetail();
        //        // _segmentDetail = segmentNameDeleteCheck1;
        //        if (segmentNameDeleteCheck1 != null)
        //        {
        //            segmentNameDeleteCheck1.IsSystem = segmentDetail.IsSystem;
        //            segmentNameDeleteCheck1.Name = segmentDetail.Name;
        //            segmentNameDeleteCheck1.ParentId = segmentDetail.ParentId;
        //            segmentNameDeleteCheck1.RecOrder = segmentDetail.RecOrder;
        //            segmentNameDeleteCheck1.SegmentMasterId = segmentDetail.SegmentMasterId;
        //            segmentNameDeleteCheck1.Version = segmentDetail.Version;
        //            segmentNameDeleteCheck1.Status = segmentDetail.Status;
        //            segmentNameDeleteCheck1.ModifiedBy = segmentDetail.ModifiedBy;
        //            segmentNameDeleteCheck1.ModifiedDate = DateTime.UtcNow;
        //            segmentNameDeleteCheck1.ObjectState = ObjectState.Modified;
        //            _segmentDetailService.Update(segmentNameDeleteCheck1);
        //        }
        //        else if (segmentDetailNameExist != null)
        //        {
        //            throw new Exception(MasterModuleValidations.Segment_Detail_Name_Already_Exist);
        //        }
        //        SegmentDetail sDetail = segmentNameDeleteCheck1 != null ? segmentNameDeleteCheck1 : new SegmentDetail();
        //        if (segmentNameDeleteCheck1 == null || segmentDetailNameExist == null)
        //        {
        //            if (segmentDetail.Id == -1)
        //                sDetail.Id = ++value;
        //            else
        //                sDetail.Id = segmentDetail.Id;
        //            sDetail.SegmentMasterId = segmentDetail.SegmentMasterId;
        //            sDetail.IsSystem = segmentDetail.IsSystem;
        //            sDetail.ModifiedBy = segmentDetail.ModifiedBy;
        //            sDetail.ModifiedDate = segmentDetail.ModifiedDate;
        //            sDetail.Name = segmentDetail.Name;
        //            sDetail.RecOrder = segmentDetail.RecOrder;
        //            sDetail.Remarks = segmentDetail.Remarks;
        //            sDetail.Status = segmentDetail.Status;
        //            sDetail.UserCreated = segmentDetail.UserCreated;
        //            sDetail.ParentId = segmentDetail.ParentId;
        //            DetailMaster.Add(sDetail);
        //        }
        //        if (segmentDetail.SegmentDetails != null)
        //        {
        //            foreach (SegmentDetail sgd in segmentDetail.SegmentDetails)
        //            {
        //                segmentDetailNameExist = _segmentDetailService.GetbyNameandParentId(sgd.Id, sgd.SegmentMasterId, sgd.ParentId, sgd.Name);
        //                var segmentNameDeleteCheck = _segmentDetailService.GetByIdAndCidAndParentId(sgd.Name, sgd.ParentId, sgd.SegmentMasterId);
        //                if (segmentNameDeleteCheck != null)
        //                {
        //                    segmentNameDeleteCheck.Status = sgd.Status;
        //                    segmentNameDeleteCheck.ObjectState = ObjectState.Modified;
        //                    _segmentDetailService.Update(segmentNameDeleteCheck);
        //                }
        //                else if (segmentDetailNameExist != null)
        //                {
        //                    throw new Exception(MasterModuleValidations.Segment_Detail_Name_Already_Exist);
        //                }
        //                if (segmentNameDeleteCheck == null || segmentDetailNameExist == null)
        //                {
        //                    //SegmentDetail sgd1 = sgd;
        //                    sgd.ParentId = sDetail != null ? sDetail.Id : 0;
        //                    if (sgd.Id == -1)
        //                        sgd.Id = ++value;
        //                    DetailMaster.Add(sgd);
        //                }
        //            }
        //        }
        //    }
        //    foreach (SegmentDetail segmentDetail in DetailMaster)
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SegmentDetail_Save_SaveSegmentDetailModel_New_Request_Message);
        //        if (segmentDetail.Name == "")
        //            throw new Exception(MasterModuleValidations.Segment_Category_child_node_is_required);
        //        var _segmentDetailSelect = _segmentDetailService.GetById(segmentDetail.Id);
        //        DateTime _date = DateTime.UtcNow;
        //        if (_segmentDetailSelect != null)
        //        {
        //            SegmentDetail _segmentDetail = _segmentDetailSelect;
        //            _segmentDetail.Id = segmentDetail.Id;
        //            _segmentDetail.SegmentMasterId = segmentDetail.SegmentMasterId;
        //            _segmentDetail.IsSystem = segmentDetail.IsSystem;
        //            _segmentDetail.ModifiedBy = segmentDetail.ModifiedBy;
        //            _segmentDetail.ModifiedDate = segmentDetail.ModifiedDate;
        //            _segmentDetail.Name = segmentDetail.Name;
        //            _segmentDetail.RecOrder = segmentDetail.RecOrder;
        //            _segmentDetail.Remarks = segmentDetail.Remarks;
        //            _segmentDetail.Status = segmentDetail.Status;
        //            _segmentDetail.UserCreated = segmentDetail.UserCreated;
        //            _segmentDetail.ParentId = segmentDetail.ParentId;
        //            _segmentDetail.ObjectState = ObjectState.Modified;
        //            _segmentDetailService.Update(_segmentDetail);
        //        }
        //        else
        //        {
        //            segmentDetail.CreatedDate = DateTime.UtcNow;
        //            segmentDetail.ObjectState = ObjectState.Added;
        //            segmentDetail.SegmentMasterId = masterId;
        //            _segmentDetailService.Insert(segmentDetail);
        //        }
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, MasterModuleLoggingValidation.Log_SegmentDetail_Save_SaveSegmentDetailModel_Saved_SuccessFully_Request_Message);
        //    }
        //}

        //public RecordStatusEnum EnableOrDisableSegmentReport(long companyId, long Id, RecordStatusEnum sgStatus, string userName)
        //{
        //    SegmentMaster segment = _segmentMasterService.GetByCidAndId(Id, companyId);
        //    if (segment == null)
        //    {
        //        throw new Exception(MasterModuleValidations.No_Segment_Category_is_found_with_provided_details);
        //    }
        //    if (sgStatus == RecordStatusEnum.Active)
        //    {
        //        int count = _segmentMasterService.GetAllSegmentMaster(companyId);
        //        if (count > 1)
        //        {
        //            throw new Exception(MasterModuleValidations.Two_Segment_Categories_already_exist_in_active_state);
        //        }
        //    }
        //    var lstSegment = _segmentMasterService.GetAllSegmentName(segment.Name, companyId);
        //    var secSegment = _segmentMasterService.GetByCidAndname(segment.Name, companyId);
        //    if (segment.Status != sgStatus)
        //    {
        //        //foreach (var seg in lstSegment)
        //        //{
        //        //    if (seg.Name == segment.Name && seg.Status == RecordStatusEnum.Inactive && seg.Id != segment.Id)
        //        //    {
        //        //        throw new Exception("You shouldn't activate two segment with same name ");
        //        //    }
        //        //}
        //        if (lstSegment.Count > 0 && secSegment.Count == 1 && sgStatus != RecordStatusEnum.Inactive)
        //            throw new Exception("You shouldn't activate two segment with same name");
        //        segment.Status = sgStatus;
        //        segment.ModifiedBy = userName;
        //        segment.ModifiedDate = DateTime.UtcNow;
        //        segment.ObjectState = ObjectState.Modified;
        //        _segmentMasterService.Update(segment);

        //    }
        //    try
        //    {
        //        _unitOfWorkAsync.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //    return sgStatus;
        //}

        //private void FillSegment(SegmentMasterDTO segmentMasterDTO, SegmentMaster _segmentMaster)
        //{
        //    _segmentMaster.CompanyId = segmentMasterDTO.CompanyId;
        //    _segmentMaster.IsSystem = segmentMasterDTO.IsSystem;
        //    _segmentMaster.Name = segmentMasterDTO.Name;
        //    _segmentMaster.RecOrder = segmentMasterDTO.RecOrder;
        //    _segmentMaster.Status = segmentMasterDTO.Status;
        //    _segmentMaster.Version = segmentMasterDTO.Version;
        //}

        #endregion

        #region ChartOfAccount

        #region Kendo
        public async Task<IQueryable<ChartOfAccountModelK>> GetAllChartOfAccountsK(long companyId, string username)
        {

            return await _chartOfAccountService.GetAllCOAK(companyId, username);
        }

        public IQueryable<ChartOfAccountDTO> GetAllModel(long CompanyId)
        {
            List<ChartOfAccountDTO> lstChart = new List<ChartOfAccountDTO>();
            IQueryable<ChartOfAccount> lstservice = _chartOfAccountService.GetChartOfAccountBycid(CompanyId);
            MultiCurrencySetting multi = _multiCurrencySettingService.Getmulticurrency(CompanyId);
            IQueryable<AccountType> lstAccountType = _accountTypeService.Queryable().Where(c => c.CompanyId == CompanyId).AsQueryable();
            bool gst = _gSTSettingService.IsGSTSettingActivated(CompanyId);
            foreach (var chartofAccount in lstservice)
            {

                if (chartofAccount.Name == "Exchange Gain/Loss - Realised" || chartofAccount.Name == "Exchange Gain/Loss - Unrealised")
                {
                    //CompanySetting MultiCurrencyModuleStatus = _companySettingService.ActivateModuleM(ModuleNameConstants.MultiCurrency, CompanyId);
                    //bool multi = MultiCurrencyModuleStatus != null ? MultiCurrencyModuleStatus.IsEnabled : false;
                    if (multi != null)
                    {
                        ChartOfAccountDTO Dto = new ChartOfAccountDTO();
                        UpdateCOA(Dto, chartofAccount, lstAccountType.ToList());
                        lstChart.Add(Dto);
                    }
                }

                else if (chartofAccount.Name == "Tax payable (GST)")
                {
                    if (gst == true)
                    {
                        ChartOfAccountDTO Dto = new ChartOfAccountDTO();
                        UpdateCOA(Dto, chartofAccount, lstAccountType.ToList());
                        lstChart.Add(Dto);
                    }
                }
                else
                {
                    ChartOfAccountDTO coaDTO = new ChartOfAccountDTO();
                    UpdateCOA(coaDTO, chartofAccount, lstAccountType.ToList());
                    lstChart.Add(coaDTO);
                }
            }
            return lstChart.AsQueryable();
        }

        public void UpdateCOA(ChartOfAccountDTO coaDTO, ChartOfAccount chartofAccount, List<AccountType> lstAccountType)
        {
            coaDTO.Id = chartofAccount.Id;
            coaDTO.AccountTypeId = chartofAccount.AccountTypeId;
            coaDTO.AppliesTo = chartofAccount.AppliesTo;
            coaDTO.CashflowType = chartofAccount.CashflowType;
            coaDTO.Category = chartofAccount.Category;
            coaDTO.Class = chartofAccount.Class;
            coaDTO.Code = chartofAccount.Code;
            coaDTO.CompanyId = chartofAccount.CompanyId;
            coaDTO.CreatedDate = chartofAccount.CreatedDate;
            coaDTO.Currency = chartofAccount.Currency;
            coaDTO.IsShowforCOA = chartofAccount.IsShowforCOA;
            coaDTO.IsSystem = chartofAccount.IsSystem;
            coaDTO.ModifiedBy = chartofAccount.ModifiedBy;
            coaDTO.ModifiedDate = chartofAccount.ModifiedDate;
            coaDTO.Name = chartofAccount.Name;
            coaDTO.Nature = chartofAccount.Nature;
            coaDTO.RecOrder = chartofAccount.RecOrder;
            coaDTO.Remarks = chartofAccount.Remarks;
            coaDTO.ShowRevaluation = chartofAccount.ShowRevaluation;
            coaDTO.Status = chartofAccount.Status;
            coaDTO.SubCategory = chartofAccount.SubCategory;
            coaDTO.UserCreated = chartofAccount.UserCreated;
            coaDTO.Version = chartofAccount.Version;
            coaDTO.IsSubLedger = chartofAccount.IsSubLedger;
            coaDTO.IsCodeEditable = chartofAccount.IsCodeEditable;
            coaDTO.ShowCurrency = chartofAccount.ShowCurrency;
            coaDTO.ShowCashFlow = chartofAccount.ShowCashFlow;
            coaDTO.ShowAllowable = chartofAccount.ShowAllowable;
            coaDTO.Revaluation = chartofAccount.Revaluation;
            coaDTO.DisAllowable = chartofAccount.DisAllowable;
            coaDTO.RealisedExchangeGainOrLoss = chartofAccount.RealisedExchangeGainOrLoss;
            coaDTO.UnrealisedExchangeGainOrLoss = chartofAccount.UnrealisedExchangeGainOrLoss;
            AccountType accountType = lstAccountType.AsQueryable().Where(a => a.Id == chartofAccount.AccountTypeId).FirstOrDefault();
            coaDTO.AccountTypeName = accountType != null ? accountType.Name : string.Empty;
            coaDTO.IsLinkedAccount = chartofAccount.IsLinkedAccount;
            coaDTO.ModuleType = chartofAccount.ModuleType;
            //  coaDTO.AccountTypeIndex = accountType.Indexs;
            //   LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,ChartOfAccountLoggingValidation.Log_ChartOfAccount_UpdateCOA_SuccessFully_Message);
        }


        #endregion

        #region Get

        public IEnumerable<AccountType> GetAccountTypes(long companyId, bool IsSystem)
        {
            if (IsSystem)
                return _accountTypeService.GetAllAccountTypes(companyId).OrderBy(c => c.RecOrder);
            //  return  _accountTypeService.Query(c => c.CompanyId == companyId && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean").Select().AsEnumerable();
            else
                return _accountTypeService.GetAllAccountTypeByCidIssys(companyId, IsSystem).OrderBy(c => c.RecOrder);
            // return _accountTypeService.Query(c => c.CompanyId == companyId && c.IsSystem == IsSystem && c.Status < RecordStatusEnum.Disable && c.ModuleType == "Bean").Select().AsEnumerable();
        }

        public ChartOfAccountModelLU GetChartOfAccountLU(long companyId)
        {
            ChartOfAccountModelLU chartOfAccountModel = new ChartOfAccountModelLU();
            //chartOfAccountModel.AllowableNonalowablLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
            chartOfAccountModel.CashFlowTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_CashflowType);
            //chartOfAccountModel.IsMultiCurrencyActivated = _multiCurrencySettingService.IsMultiCurrencyActivated(companyId);
            //CompanySetting setting = _companySettingService.ActivateModuleM(ModuleNameConstants.AllowableNonAllowable, companyId);
            //chartOfAccountModel.IsAllowableNotAllowableActivated = setting != null ? setting.IsEnabled : false;
            MultiCurrencySetting msetting = _multiCurrencySettingService.Getmulticurrency(companyId);
            chartOfAccountModel.IsRevaluationActivated = (msetting == null) ? false : msetting.Revaluation == true ? true : false;
            chartOfAccountModel.IsSeedData = false;
            chartOfAccountModel.CurrencyLU = _currencyService.GetByCurrencyById(companyId).Select(x => new CurrencyLU()
            {
                Id = x.Id,
                Code = x.Code,
                DefaultValue = x.DefaultValue,
                DefaultCurrency = "SGD",
                Name = x.Name,
                Status = x.Status
            }).ToList();
            return chartOfAccountModel;
        }

        public ChartOfAccountDTO GetChartOfAccountById(long id)
        {
            ChartOfAccountDTO coaDTO = new ChartOfAccountDTO();
            ChartOfAccount chartofaccount = _chartOfAccountService.GetChartOfAccountById(id);
            FillChartOfAccount(coaDTO, chartofaccount);
            List<AccountType> lstaccType = _accountTypeService.GetAllAccountTypes().Where(a => a.Status < RecordStatusEnum.Disable && a.CompanyId == chartofaccount.CompanyId).ToList();
            LookUp lookup = new LookUp();
            lookup.AccountTypes = lstaccType;
            //List<JournalDetail> lstjournalDetail = _journalDetailService.GetAllDetailByCOAId(id);
            //coaDTO.IsPostedJournal = lstjournalDetail.Any() || lstjournalDetail.Count > 0 ? true : false;
            coaDTO.IsPostedJournal = _journalDetailService.GetAllJDetailByCid(id);
            //coaDTO.IsAllowableNotAllowableActivated = _companySettingService.GetModuleStatuss(ModuleNameConstants.AllowableNonAllowable, chartofaccount.CompanyId);
            coaDTO.IsAllowableNotAllowableActivated = chartofaccount.IsAllowableNotAllowableActivated;
            MultiCurrencySetting msetting = _multiCurrencySettingService.Getmulticurrency(chartofaccount.CompanyId);
            if (msetting != null)
                coaDTO.IsRevaluationActivated = msetting.Revaluation == true ? true : false;

            coaDTO.LookUp = lookup;

            return coaDTO;
        }

        private void FillChartOfAccount(ChartOfAccountDTO coaDTO, ChartOfAccount chartofAccount)
        {
            coaDTO.Id = chartofAccount.Id;
            coaDTO.AccountTypeId = chartofAccount.AccountTypeId;
            coaDTO.AppliesTo = chartofAccount.AppliesTo;
            coaDTO.CashflowType = chartofAccount.CashflowType;
            coaDTO.Category = chartofAccount.Category;
            coaDTO.Class = chartofAccount.Class;
            coaDTO.Code = chartofAccount.Code;
            coaDTO.CompanyId = chartofAccount.CompanyId;
            coaDTO.CreatedDate = chartofAccount.CreatedDate;
            if (chartofAccount.Currency != null)
            {
                coaDTO.Currency = chartofAccount.Currency;
            }
            else
            {
                var financial = _financialSettingService.GetFinancialNyCompanyId(coaDTO.CompanyId);
                if (financial != null)
                    coaDTO.Currency = financial.BaseCurrency;
            }
            coaDTO.IsShowforCOA = chartofAccount.IsShowforCOA;
            coaDTO.IsSystem = chartofAccount.IsSystem;
            coaDTO.ModifiedBy = chartofAccount.ModifiedBy;
            coaDTO.ModifiedDate = chartofAccount.ModifiedDate;
            coaDTO.Name = chartofAccount.Name;
            coaDTO.Nature = chartofAccount.Nature;
            coaDTO.RecOrder = chartofAccount.RecOrder;
            coaDTO.Remarks = chartofAccount.Remarks;
            coaDTO.ShowRevaluation = chartofAccount.ShowRevaluation;
            coaDTO.Status = chartofAccount.Status;
            coaDTO.SubCategory = chartofAccount.SubCategory;
            coaDTO.UserCreated = chartofAccount.UserCreated;
            coaDTO.Version = chartofAccount.Version;
            coaDTO.IsSubLedger = chartofAccount.IsSubLedger;
            coaDTO.IsCodeEditable = chartofAccount.IsCodeEditable;
            coaDTO.ShowCurrency = chartofAccount.ShowCurrency;
            coaDTO.ShowCashFlow = chartofAccount.ShowCashFlow;
            coaDTO.ShowAllowable = chartofAccount.ShowAllowable;
            coaDTO.Revaluation = chartofAccount.Revaluation;
            coaDTO.DisAllowable = chartofAccount.DisAllowable;
            coaDTO.RealisedExchangeGainOrLoss = chartofAccount.RealisedExchangeGainOrLoss;
            coaDTO.UnrealisedExchangeGainOrLoss = chartofAccount.UnrealisedExchangeGainOrLoss;
            coaDTO.ModuleType = chartofAccount.ModuleType;
            var accountType = _accountTypeService.Query(a => a.Id == chartofAccount.AccountTypeId).Select().FirstOrDefault();
            coaDTO.AccountTypeName = accountType.Name;
            if (accountType.Name == COANameConstants.Cashandbankbalances)
                coaDTO.IsCashAndBank = true;
            coaDTO.AccountTypeIndex = accountType.Indexs;
            coaDTO.IsSeedData = chartofAccount.IsSeedData;
            coaDTO.IsBank = chartofAccount.IsBank;
            coaDTO.IsLinkedAccount = chartofAccount.IsLinkedAccount;
        }

        #endregion

        #region Save
        public ChartOfAccount SaveChartOfAccount(ChartOfAccount chartOfAccount)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(chartOfAccount));
            Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, "ObjectSave", AdditionalInfo);

            string Eventstatus = "null";
            string _errors = CommonValidation.ValidateObject(chartOfAccount);

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            var nameCheck = _chartOfAccountService.CheckName(chartOfAccount.Id, chartOfAccount.Name, chartOfAccount.CompanyId);
            if (nameCheck != null)
                throw new Exception("Duplicate account name. Verify that you’ve entered the correct information.");
            var _chartOfAccountSelect = _chartOfAccountService.GetChartOfAccountByIdcid(chartOfAccount.Id, chartOfAccount.CompanyId);
            var _chartOfAccountCodeDB = _chartOfAccountService.GetChartOfAccountBycidcode(chartOfAccount.Code, chartOfAccount.CompanyId);
            var chartOfAccountCodeEdit = _chartOfAccountService.GetChartOfAccountBycidIdCode(chartOfAccount.Id, chartOfAccount.Code, chartOfAccount.CompanyId);
            DateTime _date = DateTime.UtcNow;
            if (_chartOfAccountSelect.Any() && !chartOfAccountCodeEdit.Any() && !_chartOfAccountCodeDB.Any())
            {
                ChartOfAccount _chartOfAccountNew = _chartOfAccountSelect.FirstOrDefault();
                _chartOfAccountNew.AccountTypeId = chartOfAccount.AccountTypeId;
                _chartOfAccountNew.AppliesTo = chartOfAccount.AppliesTo;
                _chartOfAccountNew.CashflowType = chartOfAccount.CashflowType;
                _chartOfAccountNew.Category = chartOfAccount.Category;
                _chartOfAccountNew.Class = chartOfAccount.Class;
                _chartOfAccountNew.Code = chartOfAccount.Code;
                _chartOfAccountNew.Currency = chartOfAccount.Currency;
                _chartOfAccountNew.ShowRevaluation = chartOfAccount.ShowRevaluation;
                _chartOfAccountNew.IsShowforCOA = chartOfAccount.IsShowforCOA;
                _chartOfAccountNew.Name = chartOfAccount.Name;
                _chartOfAccountNew.Status = chartOfAccount.Status;
                _chartOfAccountNew.IsSystem = chartOfAccount.IsSystem;
                _chartOfAccountNew.Nature = chartOfAccount.Nature;
                _chartOfAccountNew.SubCategory = chartOfAccount.SubCategory;
                _chartOfAccountNew.ModifiedBy = chartOfAccount.ModifiedBy;
                _chartOfAccountNew.ModifiedDate = _date;
                _chartOfAccountNew.RecOrder = chartOfAccount.RecOrder;
                _chartOfAccountNew.Remarks = chartOfAccount.Remarks;
                _chartOfAccountNew.Version = chartOfAccount.Version;
                _chartOfAccountNew.IsSubLedger = chartOfAccount.IsSubLedger;
                _chartOfAccountNew.IsCodeEditable = chartOfAccount.IsCodeEditable;
                _chartOfAccountNew.ShowCurrency = chartOfAccount.ShowCurrency;
                _chartOfAccountNew.ShowCashFlow = chartOfAccount.ShowCashFlow;
                _chartOfAccountNew.ShowAllowable = chartOfAccount.ShowAllowable;
                _chartOfAccountNew.Revaluation = chartOfAccount.Revaluation;
                _chartOfAccountNew.RecOrder = chartOfAccount.RecOrder;
                _chartOfAccountNew.Remarks = chartOfAccount.Remarks;
                _chartOfAccountNew.Version = chartOfAccount.Version;
                _chartOfAccountNew.FRPATId = chartOfAccount.FRATId;
                _chartOfAccountNew.IsAllowableNotAllowableActivated = chartOfAccount.IsAllowableNotAllowableActivated;
                _chartOfAccountNew.UnrealisedExchangeGainOrLoss = chartOfAccount.UnrealisedExchangeGainOrLoss;
                _chartOfAccountNew.RealisedExchangeGainOrLoss = chartOfAccount.RealisedExchangeGainOrLoss;
                _chartOfAccountNew.DisAllowable = chartOfAccount.DisAllowable;
                _chartOfAccountNew.IsRealCOA = true;

                _chartOfAccountNew.ObjectState = ObjectState.Modified;
                _chartOfAccountService.Update(_chartOfAccountNew);
                Eventstatus = "update";
            }
            else if (_chartOfAccountSelect.Any() && chartOfAccountCodeEdit.Any())
            {
                ChartOfAccount _chartOfAccountNew = _chartOfAccountSelect.FirstOrDefault();
                _chartOfAccountNew.AccountTypeId = chartOfAccount.AccountTypeId;
                _chartOfAccountNew.AppliesTo = chartOfAccount.AppliesTo;
                _chartOfAccountNew.CashflowType = chartOfAccount.CashflowType;
                _chartOfAccountNew.Category = chartOfAccount.Category;
                _chartOfAccountNew.Class = chartOfAccount.Class;
                _chartOfAccountNew.Code = chartOfAccount.Code;
                _chartOfAccountNew.Currency = chartOfAccount.Currency;
                _chartOfAccountNew.ShowRevaluation = chartOfAccount.ShowRevaluation;
                _chartOfAccountNew.IsShowforCOA = chartOfAccount.IsShowforCOA;
                _chartOfAccountNew.Name = chartOfAccount.Name;
                _chartOfAccountNew.Status = chartOfAccount.Status;
                _chartOfAccountNew.IsSystem = chartOfAccount.IsSystem;
                _chartOfAccountNew.IsSubLedger = chartOfAccount.IsSubLedger;
                _chartOfAccountNew.IsCodeEditable = chartOfAccount.IsCodeEditable;
                _chartOfAccountNew.ShowCurrency = chartOfAccount.ShowCurrency;
                _chartOfAccountNew.ShowCashFlow = chartOfAccount.ShowCashFlow;
                _chartOfAccountNew.ShowAllowable = chartOfAccount.ShowAllowable;
                _chartOfAccountNew.Revaluation = chartOfAccount.Revaluation;
                _chartOfAccountNew.Nature = chartOfAccount.Nature;
                _chartOfAccountNew.SubCategory = chartOfAccount.SubCategory;
                _chartOfAccountNew.ModifiedBy = chartOfAccount.ModifiedBy;
                _chartOfAccountNew.ModifiedDate = _date;
                _chartOfAccountNew.RecOrder = chartOfAccount.RecOrder;
                _chartOfAccountNew.Remarks = chartOfAccount.Remarks;
                _chartOfAccountNew.Version = chartOfAccount.Version;
                _chartOfAccountNew.FRPATId = chartOfAccount.FRATId;
                _chartOfAccountNew.IsAllowableNotAllowableActivated = chartOfAccount.IsAllowableNotAllowableActivated;
                _chartOfAccountNew.UnrealisedExchangeGainOrLoss = chartOfAccount.UnrealisedExchangeGainOrLoss;
                _chartOfAccountNew.RealisedExchangeGainOrLoss = chartOfAccount.RealisedExchangeGainOrLoss;
                _chartOfAccountNew.DisAllowable = chartOfAccount.DisAllowable;
                _chartOfAccountNew.IsRealCOA = true;
                _chartOfAccountNew.ObjectState = ObjectState.Modified;
                _chartOfAccountService.Update(_chartOfAccountNew);
                Eventstatus = "update";
            }
            else if (!_chartOfAccountSelect.Any() && !_chartOfAccountCodeDB.Any())
            {
                long COAId = _chartOfAccountService.Queryable().Max(c => c.Id);
                string cashBankName = _accountTypeService.GetAccountTypeName(chartOfAccount.AccountTypeId);
                // Query( 1==1).Select().AsEnumerable();
                long value = 0;
                if (COAId != 0)
                {
                    value = COAId;
                }
                //value = _chartOfAccountService.Queryable().Max(c => c.Id);
                //chartOfAccount.UserCreated = chartOfAccount.UserCreated;
                chartOfAccount.CreatedDate = _date;
                chartOfAccount.Status = RecordStatusEnum.Active;
                chartOfAccount.Id = (value + 1);
                chartOfAccount.FRCOAId = Guid.NewGuid();
                chartOfAccount.FRPATId = chartOfAccount.FRATId;
                chartOfAccount.IsRealCOA = true;
                chartOfAccount.IsSeedData = false;
                chartOfAccount.IsLinkedAccount = cashBankName == COANameConstants.Cashandbankbalances ? false : (bool?)null;
                // chartOfAccount.Id = chartOfAccount.Id;
                chartOfAccount.ObjectState = ObjectState.Added;
                chartOfAccount.IsSystem = null;
                _chartOfAccountService.Insert(chartOfAccount);
                Eventstatus = "insert";

            }
            else
            {
                string responseMessage = "Account Code Already Exists";
                throw new Exception(responseMessage);
            }

            try
            {
                _unitOfWorkAsync.SaveChanges();
                //if (Eventstatus == "insert")
                //{
                //    DomainEventChannel.Raise(new ChartOfAccountCreated(chartOfAccount));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new ChartOfAccountUpdated(chartOfAccount));
                //    DomainEventChannel.Raise(new ChartOfAccountStatusChanged(chartOfAccount));
                //}
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return chartOfAccount;
        }

        #endregion

        #region Delete_COA
        public string DeleteChartofaccount(DeleteChartofaccount deleteChartofaccount, string ConnectionString)
        {
            try
            {
                string message = string.Empty;
                ChartOfAccount chartOfAccountLinkedandlockedAccount = _chartOfAccountService.GetChartOfAccount(deleteChartofaccount.CompanyId, deleteChartofaccount.CoaId);
                if (chartOfAccountLinkedandlockedAccount != null)
                {
                    if (chartOfAccountLinkedandlockedAccount.IsSystem == false)
                    {
                        throw new Exception(MasterModuleValidations.This_ChartOfAccount_is_linked_to_Linked_Account);
                    }
                    else if (chartOfAccountLinkedandlockedAccount.IsSystem == true)
                    {
                        throw new Exception(MasterModuleValidations.This_ChartOfAccount_is_linked_to_Syatem_Account);
                    }
                }
                JournalDetail posting = _journalService.GetJournaldetailPosting(deleteChartofaccount.CompanyId, deleteChartofaccount.CoaId);
                if (posting != null)
                    throw new Exception(MasterModuleValidations.This_chartOfAccount_have_postings_you_cantbe_be_delete);
                BeanEntity beanEntity = _beanEntityService.GetBeanEntity(deleteChartofaccount.CompanyId, deleteChartofaccount.CoaId);
                if (beanEntity != null)
                    throw new Exception(MasterModuleValidations.This_ChartOfAccount_CanNotBedeleted_ThisIS_Linked_to_Entity + " " + beanEntity.Name + MasterModuleValidations.entity_you_cant_delete);
                Item beanItem = _itemService.GetBeanItem(deleteChartofaccount.CompanyId, deleteChartofaccount.CoaId);
                if (beanItem != null)
                    throw new Exception(MasterModuleValidations.This_ChartOfAccount_CanNotBedeleted_ThisIS_Linked_to_Entity + " " + beanItem.Code + MasterModuleValidations.Item_you_cant_delete);
                OpeningBalanceDetail openingBal = _openingBalanceDetail.GetOpenaningBalance(deleteChartofaccount.CoaId);
                if (openingBal != null)
                {
                    if ((openingBal.DocCredit != null || openingBal.DocDebit != null) && (openingBal.DocCredit != 0 || openingBal.DocDebit != null) && (openingBal.DocCredit != null || openingBal.DocDebit != 0))
                    {
                        throw new Exception(MasterModuleValidations.This_COA_is_used_in_Opening_Balance_Draft_State_you_cant_be_deleted);
                    }
                    //openingBal.ObjectState = ObjectState.Deleted;
                    //_openingBalanceDetail.Delete(openingBal);
                    //_unitOfWorkAsync.SaveChanges();
                    using (con = new SqlConnection(ConnectionString))
                    {
                        query = $"DELETE  from Bean.OpeningBalanceDetail where COAId={openingBal.COAId}";
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                if (chartOfAccountLinkedandlockedAccount != null)
                {
                    chartOfAccountLinkedandlockedAccount.ObjectState = ObjectState.Deleted;
                    //_chartOfAccountService.Delete(chartOfAccountLinkedandlockedAccount);
                    _unitOfWorkAsync.SaveChanges();
                }
                return MasterModuleValidations.Deleted_Successfully;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Delete_COA

        #endregion

        #region Forex Block
        #region Kendo Call
        //public IQueryable<ForexkModel> GetAllForexById(long companyId, string type)
        //{
        //    List<ForexkModel> lstforex = new List<ForexkModel>();
        //    DateTime? FinancialDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
        //    var forexDetails =
        //                            from x in _forexService.Queryable()
        //                            where x.CompanyId == companyId && x.Type == type
        //                            select new
        //                            {
        //                                Id = x.Id,
        //                                CompanyId = x.CompanyId,
        //                                DateFrom = x.DateFrom,
        //                                Dateto = x.Dateto,
        //                                Currency = x.Currency,
        //                                UnitPerUSDStr = x.UnitPerUSD,
        //                                CreatedDate = x.CreatedDate,
        //                                Status = x.Status,
        //                                IsWithinFinancialPeriod = (FinancialDate == null) ? true : (x.DateFrom >= FinancialDate && x.Dateto >= FinancialDate)
        //                            };
        //    var lstforexs = forexDetails.OrderBy(a => a.CreatedDate).AsQueryable();
        //    foreach (var forex in lstforexs)
        //    {
        //        ForexkModel forexkModel = new ForexkModel()
        //        {

        //            Id = forex.Id,
        //            DateFrom = forex.DateFrom,
        //            Dateto = forex.Dateto,
        //            CompanyId = forex.CompanyId,
        //            Currency = forex.Currency,
        //            Status = forex.Status,
        //            UnitPerUSDStr = forex.UnitPerUSDStr.ToString()

        //        };
        //        lstforex.Add(forexkModel);
        //    }

        //    return lstforex.AsQueryable();
        //}
        #endregion
        #region  Create Call
        //public ForexModel GetForexByIdNew(long id, long companyId, string type)
        //{
        //    ForexModel forexModel = new ForexModel();
        //    try
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Get_GetForexByIdNew_Request_Message);
        //        Forex forex = new Forex();

        //        forex = _forexService.GetForex(id, companyId);

        //        if (type == "Base Currency")
        //        {
        //            forexModel.BaseCurrency = _financialSettingService.GetFinancial(companyId);
        //        }
        //        else
        //        {
        //            forexModel.BaseCurrency = "";
        //        }
        //        if (type == "GST Currency")
        //        {
        //            forexModel.GSTRepoCurrency = _gSTSettingService.GetGSTSettingRepo(companyId);
        //        }
        //        else
        //        {
        //            forexModel.GSTRepoCurrency = "";
        //        }

        //        MultiCurrencySetting multiCurrencyVal = _multiCurrencySettingService.Getmulticurrency(companyId);

        //        if (multiCurrencyVal != null)
        //            forexModel.MultyCurrencyStatus = multiCurrencyVal.Status.ToString();
        //        else
        //            forexModel.MultyCurrencyStatus = "";

        //        GSTSetting gstSettingsVal = _gSTSettingService.GetGSTSettingByCompanyId(companyId);

        //        if (gstSettingsVal != null)
        //            forexModel.GstStatus = gstSettingsVal.Status.ToString();
        //        else
        //            forexModel.GstStatus = "";

        //        FinancialSetting financialSettingsVal = _financialSettingService.GetFinancialNyCompanyId(companyId);

        //        if (financialSettingsVal != null)
        //            forexModel.FinancialStatus = financialSettingsVal.Status.ToString();
        //        else
        //            forexModel.FinancialStatus = "";

        //        //forexModel.GstStatus = _gstSettingRepository.Query(c => c.CompanyId == companyId).Select(c => c.Status).FirstOrDefault().ToString();

        //        string currencyCode = forex == null ? "" : forex.Currency == null ? "" : forex.Currency;
        //        forexModel.CurrencyLU = _currencyService.GetByCurrency(companyId, currencyCode).Select(x => new LookUp<string>()
        //        {
        //            Code = x.Code,
        //            Id = x.Id,
        //            Name = x.Name,
        //            RecOrder = x.RecOrder
        //        }).ToList();

        //        if (forex == null)
        //        {
        //            forexModel.DateFrom = DateTime.Now;
        //            forexModel.Dateto = DateTime.Now;
        //        }
        //        else
        //        {
        //            forexModel.CompanyId = forex.CompanyId;
        //            forexModel.Currency = forex.Currency;
        //            forexModel.DateFrom = forex.DateFrom;
        //            forexModel.Dateto = forex.Dateto;
        //            forexModel.Id = forex.Id;
        //            forexModel.Notes = forex.Notes;
        //            forexModel.RecOrder = forex.RecOrder;
        //            forexModel.Remarks = forex.Remarks;
        //            forexModel.Status = forex.Status;
        //            forexModel.Type = forex.Type;
        //            forexModel.UnitPerUSD = forex.UnitPerUSD;
        //            forexModel.UnitPerUSDStr = forex.UnitPerUSDStr;
        //            forexModel.USDPerUnit = forex.USDPerUnit;
        //            forexModel.ModifiedBy = forex.ModifiedBy;
        //            forexModel.ModifiedDate = forex.ModifiedDate;
        //            forexModel.CreatedDate = forex.CreatedDate;
        //            forexModel.UserCreated = forex.UserCreated;
        //        }
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Get_GetForexByIdNew_SuccessFully_Message);

        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,ForexLoggingValidation.Log_Forex_Get_GetForexByIdNew_Exception_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }

        //    return forexModel;// lstForex.AsEnumerable<ForexModel>().Where(x => x.Id == id).AsEnumerable();
        //                      // return _forexRepository.Queryable().Where(x => x.Id == id).AsEnumerable();
        //}

        //public ForexGridModel GetForexGridModel(long companyId, DateTime date)
        //{
        //    ForexGridModel forexModel = new ForexGridModel();
        //    try
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Get_GetForexGridModel_Request_Message);

        //        forexModel.CompanyId = companyId;

        //        MultiCurrencySetting multiCurrencyVal = _multiCurrencySettingService.Getmulticurrency(companyId);

        //        if (multiCurrencyVal != null)
        //            forexModel.MultyCurrencyStatus = multiCurrencyVal.Status.ToString();
        //        else
        //            forexModel.MultyCurrencyStatus = "";

        //        GSTSetting gstSettingsVal = _gSTSettingService.GetGSTSettingByCompanyId(companyId);

        //        if (gstSettingsVal != null)
        //        {
        //            forexModel.GstStatus = gstSettingsVal.Status.ToString();
        //            forexModel.GSTRepoCurrency = gstSettingsVal.GSTRepoCurrency;
        //        }
        //        else
        //        {
        //            forexModel.GstStatus = "";
        //            forexModel.GSTRepoCurrency = "";
        //        }
        //        FinancialSetting financialSettingsVal = _financialSettingService.GetFinancialNyCompanyId(companyId);

        //        if (financialSettingsVal != null)
        //        {
        //            forexModel.FinancialStatus = financialSettingsVal.Status.ToString();
        //            forexModel.BaseCurrency = financialSettingsVal.BaseCurrency;
        //        }
        //        else
        //        {
        //            forexModel.FinancialStatus = "";
        //            forexModel.BaseCurrency = "";
        //        }
        //        forexModel.IsGSTAllowed = _gSTSettingService.IsGSTAllowed(companyId, date);
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Get_GetForexGridModel_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,ForexLoggingValidation.Log_Forex_Get_GetForexGridModel_Exception_Message);
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return forexModel;
        //}

        public FinancialDetailModel GetFinancialDetailModel(long companyId)
        {
            FinancialDetailModel financialModel = new FinancialDetailModel();
            financialModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
            financialModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
            financialModel.FinancialYearEndLockDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
            financialModel.CompanyId = companyId;

            return financialModel;
        }
        //public IEnumerable<Forex> GetAllForex(long companyId, string type)
        //{
        //    List<Forex> lstForex = _forexService.GetForexAll(companyId, type);
        //    try
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Get_GetAllForex_Request_Message);
        //        DateTime? FinancialDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
        //        foreach (Forex forex in lstForex)
        //            forex.IsWithinFinancialPeriod = FinancialDate == null ? true : (forex.DateFrom >= FinancialDate && forex.Dateto >= FinancialDate);
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Get_GetAllForex_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,ForexLoggingValidation.Log_Forex_Get_GetAllForex_Exception_Message);
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return lstForex;
        //}

        //public bool EnableOrDisableForex(string Id, string tableName, string status, string userName)
        //{
        //    string responseMessage = "";

        //    if (Id != null)
        //    {
        //        try
        //        {
        //            int intId;
        //            bool result = Int32.TryParse(Id, out intId);
        //            Forex baseForex = _forexService.Query(a => a.Id == intId).Select().FirstOrDefault();
        //            Forex gstForex = _forexService.Query(a => a.DateFrom == baseForex.DateFrom && a.Dateto == baseForex.Dateto && a.Type != baseForex.Type).Select().FirstOrDefault();

        //            int statusVal;

        //            if (status == RecordStatusEnum.Active.ToString())
        //            {
        //                statusVal = 2;
        //            }
        //            else
        //            {
        //                Forex val = CheckForexExist(Convert.ToInt32(Id));
        //                if (val == null)
        //                {
        //                    statusVal = 1;
        //                }
        //                else
        //                {
        //                    responseMessage = "Forex already exist in active state.";
        //                    throw new Exception(responseMessage);
        //                }
        //            }

        //            if (result)
        //            {
        //                using (var context = new MasterModuleContext())
        //                {
        //                    context.Database.ExecuteSqlCommand(string.Format(" UPDATE {0} SET Status = {1}, ModifiedBy='{2}', ModifiedDate=GETUTCDATE()  WHERE Id = '{3}'", tableName, statusVal, userName, intId));
        //                }
        //                if (gstForex != null)
        //                {
        //                    using (var context = new MasterModuleContext())
        //                    {
        //                        context.Database.ExecuteSqlCommand(string.Format(" UPDATE {0} SET Status = {1}, ModifiedBy='{2}', ModifiedDate=GETUTCDATE()  WHERE Id = '{3}'", tableName, statusVal, userName, gstForex.Id));
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                using (var context = new MasterModuleContext())
        //                {
        //                    context.Database.ExecuteSqlCommand(string.Format(" UPDATE {0} SET Status = {1}, ModifiedBy='{2}', ModifiedDate=GETUTCDATE() WHERE Id =  '{3}'  ", tableName, statusVal, userName, Id));
        //                }
        //                if (gstForex != null)
        //                {
        //                    using (var context = new MasterModuleContext())
        //                    {
        //                        context.Database.ExecuteSqlCommand(string.Format(" UPDATE {0} SET Status = {1}, ModifiedBy='{2}', ModifiedDate=GETUTCDATE()  WHERE Id = '{3}'", tableName, statusVal, userName, gstForex.Id));
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        //public Forex CheckForexExist(long Id)
        //{
        //    var _forexSelect =
        //      _forexService.Forx(Id);
        //    var _forexCheck = _forexService.GetByGSTCurr(_forexSelect.Type, _forexSelect.Currency, _forexSelect.DateFrom, _forexSelect.Dateto, _forexSelect.CompanyId);

        //    return _forexCheck;
        //}
        #endregion

        #region Save Call
        //public String SaveForex(ForexModel forex)
        //{
        //    DateTime datefrom2 = forex.DateFrom.Date;
        //    DateTime dateto2 = forex.Dateto.Date;
        //    forex.DateFrom = forex.DateFrom.Date;
        //    forex.Dateto = forex.Dateto.Date;
        //    decimal? cal = 0;
        //    decimal? setValue = 0;


        //    //double count = Math.Floor(Math.Log10(Convert.ToDouble(forex.UnitPerUSDStr)) + 1);
        //    //if (count > 5)
        //    //    throw new Exception("The format is not correct.");

        //    var forex1 = _forexService.GetForex(forex.Id, forex.CompanyId);
        //    if (forex1 != null)
        //    {
        //        datefrom2 = forex1.DateFrom.Date;
        //        dateto2 = forex1.Dateto.Date;
        //        forex1.DateFrom = forex1.DateFrom.Date;
        //        forex1.Dateto = forex1.Dateto.Date;

        //        if (!_financialSettingService.ValidateYearEndLockDate(forex1.DateFrom, forex1.CompanyId))
        //        {
        //            throw new Exception("Transaction date is in closed financial period and cannot be posted.");
        //        }

        //        //Verify if the invoice is out of open financial period and lock password is entered and valid
        //        if (!_financialSettingService.ValidateFinancialOpenPeriod(forex1.DateFrom, forex1.CompanyId))
        //        {
        //            if (String.IsNullOrEmpty(forex.PeriodLockPassword))
        //            {
        //                throw new Exception("Transaction date is in locked accounting period and cannot be posted.");
        //            }
        //            else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(forex1.DateFrom, forex.PeriodLockPassword, forex1.CompanyId))
        //            {
        //                throw new Exception("Invalid Financial Period Lock Password.");
        //            }
        //        }
        //        if (!_financialSettingService.ValidateFinancialOpenPeriod(forex1.Dateto, forex1.CompanyId))
        //        {
        //            if (String.IsNullOrEmpty(forex.PeriodLockPassword))
        //            {
        //                throw new Exception("Transaction date is in locked accounting period and cannot be posted.");
        //            }
        //            else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(forex1.Dateto, forex.PeriodLockPassword, forex1.CompanyId))
        //            {
        //                throw new Exception("Invalid Financial Period Lock Password.");
        //            }
        //        }
        //    }
        //    Forex _forex = SaveForexInternal(forex, out cal, true, setValue, true);
        //    setValue = cal;
        //    try
        //    {
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Save_SaveForex_Request_Message);
        //        if (forex.Type.ToUpper() == "Base Currency".ToUpper())
        //        {
        //            GSTSetting gstSettingsVal = _gSTSettingService.GetGSTSettingByCompanyId(forex.CompanyId);
        //            if (gstSettingsVal != null)
        //            //if (_companySettingService.GetModuleStatus(ModuleNameConstants.GST, forex.CompanyId))
        //            {
        //                if (gstSettingsVal.GSTRepoCurrency.ToUpper() == forex.Currency)
        //                {
        //                    FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(forex.CompanyId);
        //                    forex.Type = "GST Currency";
        //                    forex.Currency = financialSetting.BaseCurrency;
        //                    forex.USDPerUnit = 1;
        //                    decimal val = Decimal.Round(1 / decimal.Parse(forex.UnitPerUSDStr), 10);
        //                    forex.UnitPerUSDStr = val.ToString();
        //                    Forex objForex = _forexService.GetByGSTCurr(forex.Type, financialSetting.BaseCurrency, datefrom2, dateto2, forex.CompanyId);
        //                    if (objForex != null)
        //                    {
        //                        forex.Id = objForex.Id;
        //                    }
        //                    SaveForexInternal(forex, out cal, false, setValue, false);
        //                }
        //            }
        //        }
        //        else if (forex.Type.ToUpper() == "GST Currency".ToUpper())
        //        {
        //            FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(forex.CompanyId);
        //            if (financialSetting.BaseCurrency.ToUpper() == forex.Currency)
        //            {
        //                GSTSetting gstSetting = _gSTSettingService.GetGSTSettingByCompanyId(forex.CompanyId);
        //                forex.Type = "Base Currency";
        //                forex.Currency = gstSetting.GSTRepoCurrency;
        //                forex.USDPerUnit = 1;
        //                decimal val = 1 / decimal.Parse(forex.UnitPerUSDStr);
        //                forex.UnitPerUSDStr = val.ToString();
        //                forex.UnitPerCal = val;
        //                Forex objForex = _forexService.GetByGSTCurr(forex.Type, forex.Currency, datefrom2, dateto2, forex.CompanyId);
        //                if (objForex != null)
        //                {
        //                    forex.Id = objForex.Id;
        //                }
        //                SaveForexInternal(forex, out cal, false, setValue, false);
        //            }
        //        }
        //        LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, ForexLoggingValidation.Log_Forex_Save_SaveForex_SuccessFully_Message);
        //        _unitOfWorkAsync.SaveChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        //LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,ForexLoggingValidation.Log_Forex_Save_SaveForex_Exception_Message);
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return "";
        //}
        #endregion

        #region Private Block//Forex code commented
        //private Forex SaveForexInternal(ForexModel forex, out decimal? cal, bool isFirst, decimal? setValue, bool isfirst)
        //{
        //    Forex _forex = new Forex();

        //    // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,ForexLoggingValidation.);
        //    string _errors = CommonValidation.ValidateObject(forex);

        //    if (!string.IsNullOrEmpty(_errors))
        //    {
        //        throw new Exception(_errors);
        //    }

        //    var _forexSelect = _forexService.GetForexs(forex.Id, forex.CompanyId);
        //    DateTime _date = DateTime.UtcNow;
        //    string _dateString = _date.ToString("yyyy'/'MM'/'dd HH:mm:ss.fff");
        //    //string _dateString = _date.ToString("s") + "Z";
        //    if (_forexSelect.Any())
        //    {
        //        if (!ValidateForexModelUpdate(forex))
        //        {
        //            throw new Exception(fxErrorMsg);
        //        }

        //        _forex = _forexSelect.FirstOrDefault();
        //        string notes = _forex.Notes;
        //        _forex.Id = forex.Id;
        //        _forex.CompanyId = forex.CompanyId;
        //        _forex.Type = forex.Type;
        //        _forex.DateFrom = forex.DateFrom.Date;
        //        _forex.Dateto = forex.Dateto.Date;
        //        _forex.Currency = forex.Currency;
        //        _forex.UnitPerUSD = forex.UnitPerUSD;
        //        _forex.UnitPerUSDStr = forex.UnitPerUSDStr;

        //        _forex.UnitPerCal = Math.Round((forex.USDPerUnit == 0 ? 1 : forex.USDPerUnit / forex.UnitPerUSD), 10);
        //        if (isFirst)
        //            _forex.UnitPerCal = Math.Round((forex.USDPerUnit == 0 ? 1 : forex.USDPerUnit / forex.UnitPerUSD), 10);
        //        else
        //            _forex.UnitPerCal = setValue;
        //        cal = _forex.UnitPerUSD;
        //        _forex.USDPerUnit = forex.USDPerUnit;
        //        //_forex.Notes = " Rate " + forex.UnitPerUSDStr + " provided by " + forex.ModifiedBy + " on " + _dateString + " <br/> " + notes;

        //        //_forex.Notes = " Rate " + forex.UnitPerUSDStr + " provided by " + forex.ModifiedBy + " on " + "{{" + "'" + _dateString + "' | clientZoneDateTimeFormat }}" + " <br/> " + notes;

        //        string[] no = notes.Split("[".ToCharArray(),
        //             StringSplitOptions.RemoveEmptyEntries);
        //        string no1 = no[0];
        //        _forex.Notes = "[{ Rate: '" + forex.UnitPerUSDStr + "',Username: '" + forex.ModifiedBy + "',CreatedDate: '" + _dateString + "'}," + no1 + " ";
        //        _forex.RecOrder = forex.RecOrder;
        //        _forex.Remarks = forex.Remarks;
        //        _forex.ModifiedBy = forex.ModifiedBy;
        //        _forex.ModifiedDate = _date;
        //        _forex.Version = forex.Version;
        //        _forex.Status = forex.Status;

        //        _forex.ObjectState = ObjectState.Modified;
        //        _forexService.Update(_forex);
        //        forex.Notes = _forex.Notes;
        //        forex.ModifiedDate = _forex.ModifiedDate;
        //    }
        //    else
        //    {
        //        if (!ValidateForexModelInsert(forex))
        //        {
        //            throw new Exception(fxErrorMsg);
        //        }

        //        long value = 0;
        //        var lstForexes = _forexService.Queryable().ToList();
        //        if (lstForexes.Any())
        //            value = lstForexes.Max(c => c.Id);
        //        //if (GetAllForexs().Any())
        //        //{
        //        //    value = Convert.ToInt64(GetAllForexs().Max(c => c.Id));
        //        //}

        //        _forex.UserCreated = forex.UserCreated;
        //        _forex.CreatedDate = _date;
        //        if (isfirst)
        //            _forex.Id = (value + 1);
        //        else
        //            _forex.Id = (value + 2);
        //        _forex.Notes = "[{ Rate: '" + forex.UnitPerUSDStr + "',Username: '" + forex.UserCreated + "',CreatedDate: '" + _dateString + "'}]";

        //        _forex.CompanyId = forex.CompanyId;
        //        _forex.Type = forex.Type;
        //        _forex.DateFrom = forex.DateFrom.Date;
        //        _forex.Dateto = forex.Dateto.Date;
        //        _forex.Currency = forex.Currency;
        //        _forex.UnitPerUSD = forex.UnitPerUSD;
        //        _forex.UnitPerCal = isFirst ? Math.Round(((forex.USDPerUnit == 0 ? 1 : forex.USDPerUnit) / forex.UnitPerUSD), 10) : setValue;
        //        cal = _forex.UnitPerUSD;
        //        _forex.UnitPerUSDStr = forex.UnitPerUSDStr;
        //        _forex.USDPerUnit = forex.USDPerUnit;
        //        _forex.RecOrder = forex.RecOrder;
        //        _forex.Remarks = forex.Remarks;
        //        //_forex.ModifiedBy = forex.ModifiedBy;
        //        //_forex.ModifiedDate = _date;
        //        _forex.Version = forex.Version;
        //        _forex.Status = forex.Status;

        //        _forex.ObjectState = ObjectState.Added;
        //        _forexService.Insert(_forex);
        //    }

        //    try
        //    {
        //        //_unitOfWorkAsync.SaveChanges();
        //        //if (_forexSelect.Any())
        //        //{
        //        //    DomainEventChannel.Raise(new ForexUpdated(forex));
        //        //}
        //        //else
        //        //{
        //        //    DomainEventChannel.Raise(new ForexCreated(forex));
        //        //}
        //    }

        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
        //        throw ex;
        //    }

        //    return _forex;
        //}

        //private bool ValidateForexModelUpdate(ForexModel forex)
        //{
        //    if (forex.DateFrom > forex.Dateto)
        //    {
        //        fxErrorMsg = "From date should be less than todate";
        //        return false;
        //    }
        //    List<Forex> lstForex = _forexService.GetAllForex(forex.Type, forex.CompanyId, forex.Currency);
        //    //foreach (Forex _forex in lstForex)
        //    //{
        //    //    //if (_forex.Id == forex.Id)
        //    //    //{
        //    //        if (forex.DateFrom <= _forex.Dateto && forex.Dateto >= _forex.DateFrom && forex.Status == _forex.Status)
        //    //        //if ((_forex.DateFrom <= forex.DateFrom && forex.DateFrom <= _forex.Dateto) || (_forex.DateFrom <= forex.Dateto && forex.Dateto <= _forex.Dateto))
        //    //        {
        //    //            fxErrorMsg = "Date range overlapped with this currency type";
        //    //            return false;
        //    //        }
        //    //    //}
        //    //}

        //    List<Forex> lstForexSameDates = lstForex.Where(a => a.DateFrom == forex.DateFrom && a.Dateto == forex.Dateto && a.Id != forex.Id && a.Type == forex.Type).ToList();
        //    if (lstForexSameDates.Count > 0)
        //    {
        //        fxErrorMsg = "Dates are Conflicting with this currency.";
        //        return false;
        //    }

        //    List<Forex> lstForexOverlappedDates = lstForex.Where(a => forex.DateFrom <= a.Dateto && forex.Dateto >= a.DateFrom && a.Id != forex.Id).ToList();
        //    if (lstForexOverlappedDates.Count > 0)
        //    {
        //        fxErrorMsg = "Dates are overlapping with this currency.";
        //        return false;
        //    }
        //    return true;
        //}

        //private bool ValidateForexModelInsert(ForexModel forex)
        //{
        //    if (forex.DateFrom > forex.Dateto)
        //    {
        //        fxErrorMsg = "From date should be less than todate";
        //        return false;
        //    }
        //    List<Forex> lstForex = _forexService.GetAllForex(forex.Type, forex.CompanyId, forex.Currency);
        //    //foreach (Forex _forex in lstForex)
        //    //{
        //    //    //if (_forex.Id == forex.Id)
        //    //    //{
        //    //        if (forex.DateFrom <= _forex.Dateto && forex.Dateto >= _forex.DateFrom && forex.Status == _forex.Status)
        //    //        //if ((_forex.DateFrom <= forex.DateFrom && forex.DateFrom <= _forex.Dateto) || (_forex.DateFrom <= forex.Dateto && forex.Dateto <= _forex.Dateto))
        //    //        {
        //    //            fxErrorMsg = "Date range overlapped with this currency type";
        //    //            return false;
        //    //        }
        //    //    //}
        //    //}

        //    List<Forex> lstForexSameDates = lstForex.Where(a => a.DateFrom == forex.DateFrom && a.Dateto == forex.Dateto).ToList();
        //    if (lstForexSameDates.Count > 0)
        //    {
        //        fxErrorMsg = "Dates are Conflicting with this currency.";
        //        return false;
        //    }

        //    List<Forex> lstForexOverlappedDates = lstForex.Where(a => forex.DateFrom <= a.Dateto && forex.Dateto >= a.DateFrom).ToList();
        //    if (lstForexOverlappedDates.Count > 0)
        //    {
        //        fxErrorMsg = "Dates are overlapping with this currency.";
        //        return false;
        //    }
        //    return true;
        //}

        #endregion

        #endregion

        #region Items

        #region Kendo
        public async Task<IQueryable<ItemkModel>> GetAllItemK(long companyId)
        {
            return await _itemService.GetByCompanyId(companyId);

        }
        #endregion

        #region Create Item

        public ItemModel CreateItem(Guid id, long companyId)
        {
            ItemModel itemModel = new ItemModel();
            Item item = _itemService.GetByIdAndCompanyId(id, companyId);
            if (item != null)
            {
                itemModel.Id = item.Id;
                itemModel.CompanyId = item.CompanyId;
                itemModel.Code = item.Code;
                itemModel.Description = item.Description;
                itemModel.UOM = item.UOM;
                itemModel.UnitPrice = item.UnitPrice;
                itemModel.Currency = item.Currency;
                itemModel.COAId = item.COAId;
                itemModel.DefaultTaxcodeId = item.DefaultTaxcodeId;
                //if (itemModel.DefaultTaxcodeId != null)
                //{
                //    var tax = _taxCodeService.GetByTaxId(itemModel.DefaultTaxcodeId);
                //    itemModel.TaxCode = tax.Code;
                //}
                itemModel.AllowableDis = item.AllowableDis;
                itemModel.Notes = item.Notes;
                itemModel.Status = item.Status;
                itemModel.ModifiedBy = item.ModifiedBy;
                itemModel.ModifiedDate = item.ModifiedDate;
                itemModel.RecOrder = item.RecOrder;
                itemModel.Remarks = item.Remarks;
                itemModel.Version = item.Version;
                itemModel.UserCreated = item.UserCreated;
                itemModel.CreatedDate = item.CreatedDate;
                itemModel.IsAllowable = item.IsAllowable;
                itemModel.IsEditabled = item.IsEditabled;
                itemModel.IsSaleItem = item.IsSaleItem;
                itemModel.IsPurchaseItem = item.IsPurchaseItem;
                //itemModel.IsAllowableNotAllowableActivated = item.IsAllowableNotAllowableActivated;
                itemModel.IsPLAccount = item.IsPLAccount;
                itemModel.IsExternalData = item.IsExternalData;
                itemModel.IsIncidental = item.IsIncidental;
            }
            else
            {
                //Item itemNew = _itemService.GetItemByCompanyId(companyId);
                itemModel.Id = Guid.NewGuid();
                itemModel.CompanyId = companyId;
            }
            //itemModel.IsAllowableNonAllowableActivated = _companySettingService.GetModuleStatuss(ModuleNameConstants.AllowableNonAllowable, itemModel.CompanyId);
            //itemModel.IsMultiCurrencyActivated = _companySettingService.GetModuleStatuss(ModuleNameConstants.MultiCurrency, itemModel.CompanyId);
            //itemModel.IsGSTActivated = _companySettingService.GetModuleStatuss(ModuleNameConstants.GST, itemModel.CompanyId);
            //itemModel.IsAccountEditable = !_journalLedgerService.IsItemTransactionPosted(itemModel.Id, itemModel.CompanyId);
            return itemModel;
        }


        public ItemWFModel CreateWFItem(long companyId)
        {
            ItemWFModel itemModel = new ItemWFModel();
            Item item = _itemService.GetWorkFlowItemByRounding(companyId);
            if (item != null)
            {
                itemModel.Id = item.Id;
                itemModel.Code = item.Code;
                itemModel.Description = item.Description;
                itemModel.COAId = item.COAId;
                itemModel.DefaultTaxcodeId = item.DefaultTaxcodeId;
                itemModel.Status = item.Status;
                itemModel.CreatedDate = item.CreatedDate;
                itemModel.IsExternalData = item.IsExternalData;
            }
            else
            {
                //Item itemNew = _itemService.GetItemByCompanyId(companyId);
                itemModel.Id = Guid.NewGuid();
                itemModel.CompanyId = companyId;
            }
            return itemModel;
        }
        public ItemModelLU CreateItemLU(long CompanyId, Guid id, bool isGst)
        {
            ItemModelLU itemDTO = new ItemModelLU();

            List<Item> lstItem = _itemService.GetAllItemByIdCompanyId(id, CompanyId);
            Item OBItem = lstItem.Where(c => c.Code == "Opening Balance").FirstOrDefault();
            //  Query(c => c.CompanyId == CompanyId && c.Id == id).Select().ToList();

            //bool showAccount = false;
            //bool showTaxCode = false;
            //bool showSegmentCategory1 = false;
            //bool showSegmentCategory2 = false;
            //bool showAllowable = false;
            //GSTSetting gstSetting = _gSTSettingService.GetGSTSettingByCompanyId(CompanyId);
            //if (gstSetting != null)
            //{
            //    if (lstItem.Any())
            //    {
            //        foreach (var items in lstItem)
            //        {
            //            long? TaxId = items.DefaultTaxcodeId;
            //            if (TaxId != null)
            //                itemDTO.TaxCodeLU = _taxCodeService.GetTaxCodeByIdandCid(CompanyId, TaxId);

            //        }
            //    }
            //    else
            //    {
            //        itemDTO.TaxCodeLU = _taxCodeService.GetById(CompanyId);
            //    }
            //    if (itemDTO.TaxCodeLU != null)
            //    {
            //        itemDTO.showTaxCode = true;
            //    }
            //}
            itemDTO.UnitsLU = _controlCodeCategoryService.GetByCategoryCodeCategory(CompanyId, ControlCodeConstants.Control_Codes_Unit);
            if (lstItem.Any())
            {
                var lookUpUnit = _controlCodeCategoryService.GetInactiveControlcode(CompanyId, ControlCodeConstants.Control_Codes_Unit, lstItem.FirstOrDefault().UOM);
                if (lookUpUnit != null)
                {
                    itemDTO.UnitsLU.Lookups.Add(lookUpUnit);
                }
            }
            List<long> CoaIds33 = lstItem.Select(x => x.COAId).ToList();
            List<COALookup<string>> lstEditCoa = null;
            List<TaxCode> allTaxCodes = null;
            List<LookUpCategory<string>> lstLus = new List<LookUpCategory<string>>();
            //string coaName = COANameConstants.Revenue;
            List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
            List<long> accTypeId = _accountTypeService.GetAllAccounyTypeByNameByCOA(CompanyId, coaName);
            List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountTypeCOA(accTypeId);
            if (id == new Guid())
            {
                itemDTO.ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status
                }).OrderBy(d => d.Name).ToList();
            }
            if (isGst)
            {
                allTaxCodes = _taxCodeService.GetTaxCodes(CompanyId);
                if (allTaxCodes.Any())
                {

                    //itemDTO.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new LookUp<string>()
                    //{
                    //    Id = x.Id,
                    //    Code = (x.Code + "-" + x.Name),
                    //    Name = x.Name,
                    //    TaxCode = x.Code,
                    //    RecOrder = x.RecOrder
                    //}).AsEnumerable().OrderBy(c => c.Name).Distinct().ToList();
                    //itemDTO.showTaxCode = true;

                    var TAX = allTaxCodes.GroupBy(a => a.Code).Select(a => new { code = a.Key, lstTax = a.FirstOrDefault() }).ToList();
                    itemDTO.TaxCodeLU = TAX.Where(c => c.lstTax.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.lstTax.Id,
                        Code = x.lstTax.Code + " - " + x.lstTax.Name,
                        Name = x.lstTax.Name,
                        TaxRate = x.lstTax.TaxRate,
                        TaxType = x.lstTax.TaxType,
                        Status = x.lstTax.Status,
                        TaxIdCode = x.lstTax.Code != "NA" ? x.lstTax.Code + "-" + x.lstTax.TaxRate + (x.lstTax.TaxRate != null ? "%" : "NA") /*+ "(" + x.lstTax.TaxType[0] + ")"*/ : x.lstTax.Code
                    }).OrderBy(c => c.Code).ToList();
                    //itemDTO.showTaxCode = true;
                    var data = itemDTO.TaxCodeLU;
                    itemDTO.TaxCodeLU = data.OrderBy(c => c.Code).ToList();
                }
            }
            if (lstItem.Any())
            {
                List<long> CoaIds = lstItem.Select(c => c.COAId).ToList();
                if (OBItem != null)
                {
                    ChartOfAccount coa = _chartOfAccountService.GetOBChartOfdAccount(CompanyId);
                    if (coa != null)
                    {
                        COALookup<string> ObCOa = new COALookup<string>()
                        {
                            Name = coa.Name,
                            Id = coa.Id,
                            RecOrder = coa.RecOrder,
                            IsAllowDisAllow = coa.DisAllowable == true ? true : false,
                            IsPLAccount = coa.Category == "Income Statement" ? true : false,
                            Class = coa.Class,
                            Status = coa.Status
                        };
                        itemDTO.ChartOfAccountLU.Add(ObCOa);
                    }
                }
                else
                {
                    CoaIds = CoaIds.Except(itemDTO.ChartOfAccountLU.Select(c => c.Id)).ToList();
                    if (CoaIds.Any())
                    {
                        lstEditCoa = chartofaAccount.Where(x => CoaIds.Contains(x.Id) || x.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status
                        })/*.ToList())*/.OrderBy(d => d.Name).ToList();
                        itemDTO.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                }
                if (isGst)
                {
                    List<long?> taxIds = lstItem.Select(x => x.DefaultTaxcodeId).ToList();
                    taxIds = taxIds.Except(itemDTO.TaxCodeLU.Select(x => x.Id)).ToList();
                    if (taxIds.Any())
                    {
                        //LookUpCategory<string> taxLu = new LookUpCategory<string>()
                        //{
                        //    CategoryName = "Default Tax Code",
                        //    Lookups = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new LookUp<string>()
                        //    {
                        //        Id = x.Id,
                        //        Code = (x.Code + "-" + x.Name),
                        //        Name = x.Name,
                        //        TaxCode = x.Code,
                        //        RecOrder = x.RecOrder
                        //    }).AsEnumerable().OrderBy(c => c.Code).ToList()
                        //};
                        //lstLus.Add(taxLu);
                        itemDTO.TaxCodeLU.AddRange(allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = (x.Code + "-" + x.Name),
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            //TaxCode = x.Code,
                            RecOrder = x.RecOrder
                        }).AsEnumerable().OrderBy(c => c.Code).ToList());
                        //itemDTO.showTaxCode = true;
                        itemDTO.TaxCodeLU = itemDTO.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                }
            }
            //if (lstItem.Any())
            //{
            //    foreach (var item in lstItem)
            //    {
            //        long coa = item == null ? 0 : item.COAId;
            //        itemDTO.ChartOfAccountLU = _chartOfAccountService.GetChartOfAccountBycidAndId(coa, CompanyId);
            //    }
            //}
            //else
            //{
            //    itemDTO.ChartOfAccountLU = _chartOfAccountService.GetChartOfAccountByCId(CompanyId);
            //}

            return itemDTO;
        }

        #endregion

        #region Save Item
        public Item Save(Item item, string _name, string ConnectionString)
        {
            string status = null;
            long? COAId = 0;
            string _errors = CommonValidation.ValidateObject(item);

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            //_redisService.InvalidateCache<Item>("GetAllItems");

            // var _itemSelect = _itemService.Query(e => e.Id == item.Id && e.CompanyId == item.CompanyId).Select();

            //item Code Duplicate checking for BenItem

            if (item.IsExternalData != true ? _itemService.IsDuplicateItem(item.CompanyId, item.Id, item.Code) == true : _itemService.IsExternalDuplicateItem(item.CompanyId, item.DocumentId, item.Code, item.Id) == true)
            {
                throw new Exception("Item Code already exist");
            }



            //IEnumerable<Item> _itemSelect = null;
            //if (item.IsExternalData == true && item.IsIncidental == null)
            //    _itemSelect = _itemService.GetItemByDocumentId(item.CompanyId, item.DocumentId.Value);
            //else if (item.IsExternalData == true && item.IsIncidental == true)
            //    _itemSelect = _itemService.GetItemByIdandIncidental(item.Id, item.CompanyId);
            //else
            //    _itemSelect = _itemService.GetAllItemByIdCompanyId(item.Id, item.CompanyId);
            Item _itemSelect = new Item();
            DateTime _date = DateTime.UtcNow;
            if (item.IsExternalData == true && item.IsIncidental == true)
                _itemSelect = _itemService.GetWFItem(item.CompanyId, item.DocumentId, item.Id);
            else if (item.IsExternalData == true && item.IsIncidental == null)
                _itemSelect = _itemService.GetWFServiceItem(item.CompanyId, item.DocumentId);
            else
                _itemSelect = _itemService.GetItem(item.CompanyId, item.Id);
            if (_itemSelect != null)
            {
                //Item _itemNew = _itemSelect.FirstOrDefault();
                COAId = _itemSelect.COAId;
                Item _itemNew = _itemSelect;

                //if (item.IsExternalData == true && item.DocumentId != null)
                //    duplicateItem = _itemSelect.Where(a => a.Code.ToLower() == item.Code.ToLower() && a.CompanyId == item.CompanyId && a.DocumentId != item.DocumentId).FirstOrDefault();
                //else
                //    duplicateItem = _itemService.GetAllByItem(item.Id, item.Code, item.CompanyId);
                //if (duplicateItem != null)
                //{
                //    if (item.IsExternalData == true && item.IsIncidental == false)
                //        _itemNew.Code = item.Code + "-" + 1; //GetItemName(item.CompanyId, item.Code);
                //    else if (item.IsExternalData == false || item.IsExternalData == null)
                //        throw new Exception("Item Code already exist");
                //    else if (item.IsExternalData == true && item.IsIncidental == true)
                //        throw new Exception("Item Code already exist");
                //}
                //else _itemNew.Code = item.Code;

                _itemNew.Code = item.Code;
                _itemNew.ModifiedDate = _date;
                _itemNew.Description = item.Description;
                _itemNew.UOM = item.UOM;
                _itemNew.UnitPrice = item.UnitPrice;
                _itemNew.Currency = item.Currency;
                _itemNew.COAId = item.COAId;
                _itemNew.DefaultTaxcodeId = item.DefaultTaxcodeId;
                _itemNew.AllowableDis = item.AllowableDis;
                _itemNew.IsAllowableNotAllowableActivated = item.IsAllowableNotAllowableActivated;
                _itemNew.Notes = item.Notes;
                _itemNew.Status = item.Status;
                _itemNew.ModifiedBy = item.ModifiedBy;
                _itemNew.RecOrder = item.RecOrder;
                _itemNew.Remarks = item.Remarks;
                _itemNew.Version = item.Version;
                _itemNew.IsPLAccount = item.IsPLAccount;
                _itemNew.IsAllowable = item.IsAllowable;
                _itemNew.IsEditabled = item.IsEditabled;
                _itemNew.IsSaleItem = item.IsSaleItem;
                _itemNew.IsPurchaseItem = item.IsPurchaseItem;
                _itemNew.IsAccountEditable = item.IsAccountEditable;
                _itemNew.IsExternalData = item.IsExternalData;
                _itemNew.IsIncidental = item.IsIncidental;
                _itemNew.DocumentId = item.DocumentId;
                _itemNew.IncidentalType = item.IncidentalType;
                //FillItemModel(item, _itemNew);
                _itemNew.ObjectState = ObjectState.Modified;
                _itemService.Update(_itemNew);
                status = "update";
            }
            else
            {
                //duplicateItem = _itemService.GetAllItemByCodeAndCompanyId(item.Code, item.CompanyId);
                //if (duplicateItem != null)
                //{
                //    //if (item.IsExternalData == false || item.IsExternalData == null)
                //    //{
                //    if ((item.IsExternalData == false || item.IsExternalData == null))
                //        throw new Exception("Item Code already exist");
                //    else if (item.IsExternalData == true && item.IsIncidental == true)
                //        throw new Exception("Item Code already exist");
                //    else if (item.IsExternalData == true && item.IsIncidental == false)
                //        item.Code = item.Code + "-" + 1; //GetItemName(item.CompanyId, item.Code);
                //                                         // }
                //}

                item.CreatedDate = item.IsExternalData == true ? item.CreatedDate : _date;
                item.Id = (item.Id == Guid.Empty) ? item.Id = Guid.NewGuid() : item.Id;
                if (item.IsIncidental == true)
                    item.DocumentId = _itemService.Queryable().Where(c => c.CompanyId == item.CompanyId).Max(c => c.DocumentId) == null || _itemService.Queryable().Where(c => c.CompanyId == item.CompanyId).Max(c => c.DocumentId) == 0 ? 1 : _itemService.Queryable().Where(c => c.CompanyId == item.CompanyId).Max(c => c.DocumentId) + 1;
                item.ObjectState = ObjectState.Added;
                _itemService.Insert(item);
                status = "insert";
            }
            try
            {
                if (item.IsIncidental == true)
                {
                    string Cursorname = "Admin Cursor";
                    string Ids = (_itemSelect != null ? COAId : 0) + "," + item.COAId.ToString() + "," + (_itemSelect != null ? _itemSelect.Id.ToString() : item.Id.ToString());
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("COALinkedAccounts_SP", conn);
                    cmd.Parameters.AddWithValue("@Ids", Ids);
                    cmd.Parameters.AddWithValue("@DocType", "Item");
                    cmd.Parameters.AddWithValue("@CusrsorName", Cursorname);
                    cmd.Parameters.AddWithValue("@CompanyId", item.CompanyId);
                    cmd.Parameters.AddWithValue("@CreateBy", item.UserCreated);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    // _unitOfWorkAsync.SaveChanges();
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                }
                _unitOfWorkAsync.SaveChanges();
                //if (status == "update")
                //{
                //    DomainEventChannel.Raise(new ItemUpdated(item));
                //    DomainEventChannel.Raise(new ItemStatusChanged(item));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new ItemCreated(item));
                //}

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return item;
        }

        #endregion

        public List<Item> GetSaleItems(bool IsActiveItems, long companyId)
        {
            if (IsActiveItems)
            {
                //   List<Item> lstitem = new List<Item>();
                var lstItems = _itemService.Query().Include(a => a.TaxCodes).Select().Where(a => a.Status == RecordStatusEnum.Active && a.IsSaleItem == true && a.CompanyId == companyId).OrderBy(a => a.Code).ToList();
                foreach (var item in lstItems)
                {
                    if (item.COAId > 0)
                    {
                        ChartOfAccountModel cmodel = new ChartOfAccountModel();
                        var chartofaccount = _chartOfAccountService.GetChartOfAccountById(item.COAId);
                        cmodel.Code = chartofaccount.Code;
                        cmodel.Id = chartofaccount.Id;
                        cmodel.Name = chartofaccount.Name;
                        if (item.TaxCodes != null)
                        {
                            item.Taxcode = item.TaxCodes.Code;
                            item.TaxCodes.TaxIdCode = item.TaxCodes.Code != "NA" ? item.TaxCodes.Code + "-" + item.TaxCodes.TaxRate + (item.TaxCodes.TaxRate != null ? "%" : "NA") + "(" + item.TaxCodes.TaxType[0] + ")" : item.TaxCodes.Code;
                        }
                        item.ChartOfAccounts = cmodel;

                    }
                }
                return lstItems;
            }
            else
            {
                // List<Item> lstitem = new List<Item>();
                var lstItems = _itemService.Query().Include(a => a.TaxCodes).Select().Where(a => (a.Status == RecordStatusEnum.Active || a.Status == RecordStatusEnum.Inactive) && a.IsSaleItem == true && a.CompanyId == companyId).OrderBy(a => a.Code).ToList();
                foreach (var item in lstItems)
                {
                    if (item.COAId > 0)
                    {
                        ChartOfAccountModel cmodel = new ChartOfAccountModel();
                        var chartofaccount = _chartOfAccountService.GetChartOfAccountById(item.COAId);
                        cmodel.Code = chartofaccount.Code;
                        cmodel.Id = chartofaccount.Id;
                        cmodel.Name = chartofaccount.Name;
                        if (item.TaxCodes != null)
                        {
                            item.Taxcode = item.TaxCodes.Code;
                            item.TaxCodes.TaxIdCode = item.TaxCodes.Code != "NA" ? item.TaxCodes.Code + "-" + item.TaxCodes.TaxRate + (item.TaxCodes.TaxRate != null ? "%" : "NA") + "(" + item.TaxCodes.TaxType[0] + ")" : item.TaxCodes.Code;
                        }

                        item.ChartOfAccounts = cmodel;
                    }
                }
                return lstItems;
            }
        }

        public async Task<List<ItemModelVM>> GetSaleItems(long companyId, Guid? DocumentId, string type)
        {
            try
            {
                Invoice invoice = null;
                List<ItemModelVM> lstofItems = null;
                bool isOBInvoice = false;
                List<TaxCode> lstTaxcode = await _taxCodeService.GetTaxCodeAsync(companyId);
                string tCode;
                if (type == DocTypeConstants.Invoice)
                {
                    invoice = await _invoiceService.GetALlinvocesByItems(DocumentId);
                    if (invoice != null)
                    {
                        if (invoice.IsOBInvoice == true)
                            isOBInvoice = true;
                    }
                }

                List<Item> lstItems = isOBInvoice ? await _itemService.GetAllItemNameAsync(companyId, "Opening Balance") : await _itemService.GetAllItems(companyId);
                if (DocumentId != Guid.Empty)
                {
                    List<Guid> lstActive = lstItems.Where(x => x.Status == RecordStatusEnum.Active).Select(c => c.Id).ToList();
                    List<Guid> lstIncoiceCashSales = null;
                    if (type == DocTypeConstants.Invoice)
                    {

                        lstIncoiceCashSales = invoice.InvoiceDetails.Select(s => s.ItemId).ToList();
                        if (lstIncoiceCashSales.Any())
                        {
                            lstIncoiceCashSales = lstIncoiceCashSales.Except(lstActive).ToList();
                            lstIncoiceCashSales.AddRange(lstActive);
                        }
                    }
                    else
                    {
                        CashSale cashsale = await _cashSalesService.GetALlCashSaleByItems(DocumentId);
                        lstIncoiceCashSales = cashsale.CashSaleDetails.Select(s => s.ItemId.Value).ToList();
                        if (lstIncoiceCashSales.Any())
                        {
                            lstIncoiceCashSales = lstIncoiceCashSales.Except(lstActive).ToList();
                            lstIncoiceCashSales.AddRange(lstActive);
                        }

                    }
                    lstofItems = lstItems.Where(s => lstIncoiceCashSales.Contains(s.Id)).Select(x => new ItemModelVM()
                    {
                        Id = x.Id,
                        AllowableDis = x.AllowableDis,
                        COAId = x.COAId,
                        COAName = x.COAName,
                        Code = x.Code,
                        CompanyId = x.CompanyId,
                        CreatedDate = x.CreatedDate,
                        Currency = x.Currency,
                        DefaultTaxcodeId = x.DefaultTaxcodeId,
                        Description = x.Description,
                        DocumentId = x.DocumentId,
                        IsAccountEditable = x.IsAccountEditable,
                        IsAllowable = x.IsAllowable,
                        IsAllowableNotAllowableActivated = x.IsAllowableNotAllowableActivated,
                        IsEditabled = x.IsEditabled,
                        IsExternalData = x.IsExternalData,
                        IsIncidental = x.IsIncidental,
                        IsPLAccount = x.IsPLAccount,
                        IsPurchaseItem = x.IsPurchaseItem,
                        IsSaleItem = x.IsSaleItem,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        Notes = x.Notes,
                        RecOrder = x.RecOrder,
                        Remarks = x.Remarks,
                        Status = x.Status,
                        Taxcode = tCode = x.DefaultTaxcodeId != null ? lstTaxcode.Where(c => c.Id == x.DefaultTaxcodeId).Select(c => c.Code).FirstOrDefault() : null,
                        LstTaxcodes = x.DefaultTaxcodeId != null ? lstTaxcode.Where(c => c.Code == tCode).Select(c => c.Id).ToList() : new List<long>(),
                        UnitPrice = x.UnitPrice,
                        UOM = x.UOM,
                        UserCreated = x.UserCreated,
                        Version = x.Version
                    }).OrderBy(s => s.Code).ToList();
                }
                else
                {

                    lstofItems = lstItems.Where(x => x.Status == RecordStatusEnum.Active).Select(x => new ItemModelVM()
                    {
                        Id = x.Id,
                        AllowableDis = x.AllowableDis,
                        COAId = x.COAId,
                        COAName = x.COAName,
                        Code = x.Code,
                        CompanyId = x.CompanyId,
                        CreatedDate = x.CreatedDate,
                        Currency = x.Currency,
                        DefaultTaxcodeId = x.DefaultTaxcodeId,
                        Description = x.Description,
                        DocumentId = x.DocumentId,
                        IsAccountEditable = x.IsAccountEditable,
                        IsAllowable = x.IsAllowable,
                        IsAllowableNotAllowableActivated = x.IsAllowableNotAllowableActivated,
                        IsEditabled = x.IsEditabled,
                        IsExternalData = x.IsExternalData,
                        IsIncidental = x.IsIncidental,
                        IsPLAccount = x.IsPLAccount,
                        IsPurchaseItem = x.IsPurchaseItem,
                        IsSaleItem = x.IsSaleItem,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        Notes = x.Notes,
                        RecOrder = x.RecOrder,
                        Remarks = x.Remarks,
                        Status = x.Status,
                        Taxcode = tCode = x.DefaultTaxcodeId != null ? lstTaxcode.Where(c => c.Id == x.DefaultTaxcodeId).Select(c => c.Code).FirstOrDefault() : null,
                        LstTaxcodes = x.DefaultTaxcodeId != null ? lstTaxcode.Where(c => c.Code == tCode).Select(c => c.Id).ToList() : new List<long>(),
                        UnitPrice = x.UnitPrice,
                        UOM = x.UOM,
                        UserCreated = x.UserCreated,
                        Version = x.Version
                    }).OrderBy(s => s.Code).ToList();
                }
                return lstofItems;

            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region TaxCode

        #region GetAllTaxCodeModelsK

        public IQueryable<TaxCodeModelK> GetAllTaxCodeModelsK(long companyId, string ConnectionString)
        {
            string query = $"select Jurisdiction from common.company where id = {companyId}";
            string jurisdictions = null;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                jurisdictions = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
            }
            return _taxCodeService.GetAllTaxCodeModelsK(companyId, jurisdictions).Where(c => c.TaxcodeStatus <= RecordStatusEnum.Inactive);

        }
        #endregion

        #region GetTaxCodeLU
        public TaxCodeModel GetTaxCodeLU(long companyId)
        {
            //companyId = 0;
            TaxCodeModel taxCodeModel = new TaxCodeModel();
            taxCodeModel.AppliesToLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AppliesTo);
            taxCodeModel.TaxTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_TaxType);
            return taxCodeModel;
        }


        #endregion

        #region Save Tax Code
        public TaxCode Save(TaxCode taxCode, string name, string ConnectionString)
        {
            taxCode.CompanyId = 0;

            string _errors = CommonValidation.ValidateObject(taxCode);

            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            TaxCode _taxCode = new TaxCode();
            var lstCode = _taxCodeService.GetCodes(taxCode.Code);
            if (lstCode.Any())
            {
                var taxes = lstCode.Where(c => c.EffectiveFrom < taxCode.EffectiveFrom).ToList();
                if (taxes.Count > 0)
                {
                    //TaxCode tax1 = taxes.OrderByDescending(c => c.EffectiveFrom).FirstOrDefault();
                    //if (tax1 != null)
                    //{
                    //    tax1.EffectiveTo = taxCode.EffectiveFrom.AddDays(-1);
                    //    tax1.ObjectState = ObjectState.Modified;
                    //    _taxCodeService.Update(tax1);
                    //}

                    using (con = new SqlConnection(ConnectionString))
                    {
                        long taxId = 0;
                        DateTime? effectFrom = null;
                        //if (taxCode.Id != -1)
                        //    query = $"SELECT * from Bean.TaxCode where Code='{taxCode.Code}' Order by EffectiveFrom";
                        //else
                        query = $"SELECT * from Bean.TaxCode where Code='{taxCode.Code}' Order by EffectiveFrom desc";
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            taxId = Convert.ToInt64(dr["Id"]);
                            effectFrom = Convert.ToDateTime(dr["EffectiveFrom"]);
                            dr.Close();
                            query = $"Update Bean.TaxCode set EffectiveTo='{String.Format("{0:MM/dd/yyyy}", taxCode.EffectiveFrom.AddDays(-1))}' where Id={taxId}";
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            try
                            {
                                if (effectFrom > taxCode.EffectiveFrom)
                                {
                                    TaxCode code1 = lstCode.Where(c => c.Id == taxId).FirstOrDefault();
                                    code1.EffectiveTo = null;
                                    code1.ObjectState = ObjectState.Modified;
                                }
                                if (taxes.Any())
                                {
                                    TaxCode code2 = taxes.OrderByDescending(c => c.EffectiveFrom).FirstOrDefault();
                                    if (code2 != null)
                                    {
                                        code2.EffectiveTo = taxCode.EffectiveFrom.AddDays(-1);
                                        code2.ObjectState = ObjectState.Modified;
                                    }
                                }
                                _unitOfWorkAsync.SaveChanges();
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
                taxes = lstCode.Where(c => c.Id != taxCode.Id && c.EffectiveFrom > taxCode.EffectiveFrom).ToList();
                if (taxes.Count > 0)
                {
                    TaxCode tax2 = taxes.OrderBy(c => c.EffectiveFrom).FirstOrDefault();
                    if (tax2 != null)
                        taxCode.EffectiveTo = tax2.EffectiveFrom.AddDays(-1);
                }
                else
                {
                    taxCode.EffectiveTo = null;
                }
            }
            var _taxCodeSelect = _taxCodeService.GetAllCompanyById(taxCode.Id, taxCode.CompanyId);
            //var _taxCodeDB = _taxCodeService.Query(e => e.Code == taxCode.Code && e.EffectiveFrom == taxCode.EffectiveFrom && e.CompanyId == taxCode.CompanyId).Select();//Change by Sreenivas
            var _taxCodeDB = _taxCodeService.GetAllTaxCodeCodeAndCompanyId(taxCode.Code, taxCode.EffectiveFrom, taxCode.CompanyId);
            var taxCodeEdit = _taxCodeService.GetAllTaxcodeCodeAndCompanyId(taxCode.Id, taxCode.Code, taxCode.CompanyId);
            DateTime _date = DateTime.UtcNow;

            if (_taxCodeSelect.Any() && !taxCodeEdit.Any() && !_taxCodeDB.Any())
            {
                _taxCode = _taxCodeSelect.FirstOrDefault();
                _taxCode.Code = taxCode.Code;
                _taxCode.Name = taxCode.Name;
                _taxCode.Description = taxCode.Description;
                _taxCode.AppliesTo = taxCode.AppliesTo;
                _taxCode.TaxType = taxCode.TaxType;
                _taxCode.TaxRate = taxCode.TaxRate;
                _taxCode.EffectiveFrom = taxCode.EffectiveFrom;
                _taxCode.IsSystem = taxCode.IsSystem;
                _taxCode.RecOrder = taxCode.RecOrder;
                _taxCode.Remarks = taxCode.Remarks;
                _taxCode.ModifiedBy = taxCode.ModifiedBy;
                _taxCode.ModifiedDate = _date;
                _taxCode.Version = taxCode.Version;
                _taxCode.Status = taxCode.Status;
                _taxCode.TaxRateFormula = taxCode.TaxRateFormula;
                _taxCode.IsApplicable = taxCode.IsApplicable;
                _taxCode.AppliesTo = taxCode.AppliesTo;
                _taxCode.IsSeedData = false;
                if (taxCode.Jurisdiction != null)
                    _taxCode.Jurisdiction = taxCode.Jurisdiction;
                else
                    throw new Exception(MasterModuleValidations.Need_Jurisdiction_To_Save_TaxCode);
                _taxCode.RecStatus = "Updated";
                _taxCode.ObjectState = ObjectState.Modified;
                _taxCodeService.Update(_taxCode);
            }
            else if (_taxCodeSelect.Any() && taxCodeEdit.Any())
            {
                _taxCode = _taxCodeSelect.FirstOrDefault();
                _taxCode.Code = taxCode.Code;
                _taxCode.Name = taxCode.Name;
                _taxCode.Description = taxCode.Description;
                _taxCode.AppliesTo = taxCode.AppliesTo;
                _taxCode.TaxType = taxCode.TaxType;
                _taxCode.TaxRate = taxCode.TaxRate;
                _taxCode.EffectiveFrom = taxCode.EffectiveFrom;
                _taxCode.IsSystem = taxCode.IsSystem;
                _taxCode.RecOrder = taxCode.RecOrder;
                _taxCode.Remarks = taxCode.Remarks;
                _taxCode.ModifiedBy = taxCode.ModifiedBy;
                _taxCode.ModifiedDate = _date;
                _taxCode.Version = taxCode.Version;
                _taxCode.Status = taxCode.Status;
                _taxCode.TaxRateFormula = taxCode.TaxRateFormula;
                _taxCode.IsApplicable = taxCode.IsApplicable;
                _taxCode.EffectiveTo = taxCode.EffectiveTo;
                _taxCode.AppliesTo = taxCode.AppliesTo;
                _taxCode.IsSeedData = false;
                if (taxCode.Jurisdiction != null)
                    _taxCode.Jurisdiction = taxCode.Jurisdiction;
                else
                    throw new Exception(MasterModuleValidations.Need_Jurisdiction_To_Save_TaxCode);
                _taxCode.RecStatus = "Updated";
                _taxCode.ObjectState = ObjectState.Modified;
                _taxCodeService.Update(_taxCode);
            }
            else if (!_taxCodeSelect.Any() && !_taxCodeDB.Any())
            {
                IEnumerable<TaxCode> GetAllTaxCodes = _taxCodeService.Queryable().AsEnumerable().ToList().ToList();
                long Taxids = GetAllTaxCodes.Max(c => c.Id);
                taxCode.CreatedDate = _date;
                taxCode.Id = (Taxids + 1);
                taxCode.IsSeedData = false;
                taxCode.ObjectState = ObjectState.Added;
                _taxCodeService.Insert(taxCode);
            }
            else
            {
                string responseMessage = "Tax Code Already Exist";
                throw new Exception(responseMessage);
            }

            try
            {
                _unitOfWorkAsync.SaveChanges();
                //if (_taxCode.RecStatus == "Updated")
                //{
                //    DomainEventChannel.Raise(new TaxCodeUpdated(taxCode));
                //}
                //else
                //{
                //    DomainEventChannel.Raise(new TaxCodeCreated(taxCode));
                //}
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return taxCode;
        }
        #endregion

        #region GetTaxCode
        public TaxCode TaxCode(long Id)
        {
            var taxCode = _taxCodeService.GetByTaxId(Id);
            //List<TaxCode> lstTaxRate = _taxCodeService.GetAllTaxCodeByCompany(0);
            //TaxCode taxCode = lstTaxRate.Where(x => x.Id == Id).FirstOrDefault();
            //if (taxCode != null)
            //{
            //    List<TaxRateModel> grplstTaxCode = lstTaxRate.Where(x => x.Code == taxCode.Code && x.Id != Id).Select(x => new TaxRateModel
            //    {
            //        TaxCode = x.Code,
            //        TaxRate = x.TaxRate,
            //        EffectiveFrom = x.EffectiveFrom,
            //        Status = x.Status
            //    }).ToList();

            //    taxCode.TaxRateModels = grplstTaxCode;
            //}
            //return _taxCodeService.GetTaxCodeById(Id);
            return taxCode;
        }
        #endregion

        #endregion

        #region Bean_GeneralSetting_Feature
        public GeneralSettingFeatureModel SaveGeneralSettingFeature(GeneralSettingFeatureModel generalSettingFeatureModel, string ConnectionString, long companyId)
        {
            try
            {
                foreach (var feature in generalSettingFeatureModel.Feature)
                {
                    if (feature.Name == "Financial" && feature.IsChecked == true)
                        generalSettingFeatureModel.IsFinancial = true;
                    if (feature.Name == "No Supporting Documents" && feature.IsChecked == true)
                        generalSettingFeatureModel.IsNoSupportingDocuments = true;
                    if (feature.Name == "Revaluation" && feature.IsChecked == true)
                        generalSettingFeatureModel.IsRevaluation = true;
                }
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Bean_General_Setting_Features", conn);
                    cmd.Parameters.AddWithValue("@IsFinancial", generalSettingFeatureModel.IsFinancial != null ? (object)generalSettingFeatureModel.IsFinancial : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsRevaluation", generalSettingFeatureModel.IsRevaluation != null ? (object)generalSettingFeatureModel.IsRevaluation : DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsNoSupportingDocuments", generalSettingFeatureModel.IsNoSupportingDocuments != null ? (object)generalSettingFeatureModel.IsNoSupportingDocuments : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PeriodLockStartDate", generalSettingFeatureModel.PeriodLockStartDate != null ? (object)generalSettingFeatureModel.PeriodLockStartDate : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PeriodLockEndDate", generalSettingFeatureModel.PeriodLockEndDate != null ? (object)generalSettingFeatureModel.PeriodLockEndDate : DBNull.Value);
                    cmd.Parameters.AddWithValue("@PeriodLockDatePassword", generalSettingFeatureModel.PeriodLockDatePassword != null ? (object)generalSettingFeatureModel.PeriodLockDatePassword : DBNull.Value);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@UserCreated", generalSettingFeatureModel.UserCreated);
                    cmd.Parameters.AddWithValue("@ModifiedBy", generalSettingFeatureModel.ModifiedBy);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return generalSettingFeatureModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        public ModuleActivationModel ModuleActivations(long companyId)
        {
            ModuleActivationModel module = new ModuleActivationModel();
            CompanySetting IsNoSupporting = _companySettingService.ActivateModuleM(ModuleNameConstants.NoSupportingDocuments, companyId);
            module.IsNoSupportingDocumentActive = IsNoSupporting != null;
            CompanySetting AllowableNon = _companySettingService.ActivateModuleM(ModuleNameConstants.AllowableNonAllowable, companyId);
            module.IsAllowableNonAllowable = AllowableNon != null;
            //module.IsGSTActive = GetGSTSettingByCompanyIdAndDate(companyId, DateTime.UtcNow.Date);
            MultiCurrencySetting multi = _multiCurrencySettingService.Getmulticurrency(companyId);
            module.IsMultiCurrencyActive = multi != null;
            FinancialSetting financSettings = GetFinancialSetting(companyId);
            module.IsFinancialActive = financSettings != null;
            if (financSettings != null)
                module.BaseCurrency = financSettings.BaseCurrency;
            return module;
        }

        public bool ActivateAllowableNonAllowable(string modulename, long companyId)
        {
            var setting = _companySettingService.ActivateModule(modulename, companyId);
            var sa = ActivateModule(setting);
            return sa;
        }

        public bool ActivateModule(long companyId)
        {
            var setting = _companySettingService.ActivateModule(ModuleNameConstants.BankReconciliation, companyId);
            var sa = ActivateModule(setting);
            return sa;
        }

        private bool ActivateModule(CompanySetting setting)
        {
            if (setting != null)
            {
                setting.IsEnabled = true;
                setting.ObjectState = ObjectState.Modified;
                _companySettingService.Update(setting);
                _unitOfWorkAsync.SaveChanges();
                return true;
            }
            return false;
        }

        public List<ActivityHistory> GetHistory(long companyid, Guid id, string type)
        {
            List<ActivityHistory> ach = new List<ActivityHistory>();
            List<ActivityHistory> ah = _activityHistoryService.GetByCIdAId(companyid, id, type);
            if (ah.Any())
            {
                return ah;
            }
            return ach;
        }

        public EnableOrDisable CurrencyEnableOrDisable(EnableOrDisable enableOrDisable)
        {
            try
            {
                int statusCount = 0;
                if (enableOrDisable.id.Any())
                {
                    string financial1 = _financialSettingService.GetFinancial(enableOrDisable.CompanyId);
                    string gstSettings1 = _gSTSettingService.GetGSTSettingRepo(enableOrDisable.CompanyId);
                    if (financial1 == null)
                        throw new Exception(MasterModuleValidations.Set_the_fiancial_settings_in_admin_general_settings);
                    if (enableOrDisable.tableName == "Bean.Currency")
                    {
                        foreach (string id in enableOrDisable.id)
                        {
                            int intId1;
                            bool result1 = Int32.TryParse(id, out intId1);
                            long companyId1 = 0;
                            Currency currency1 = _currencyService.GetByIdandCompanyId(intId1, enableOrDisable.CompanyId);
                            if (currency1 != null)
                            {
                                companyId1 = currency1.CompanyId;
                            }

                            if (result1)
                            {
                                if (currency1 != null)
                                {
                                    if (financial1 != null)
                                    {
                                        if (financial1 == currency1.Code)
                                        {
                                            throw new Exception("Base Currency (" + financial1 + ") Cannot Be Inactive");
                                        }
                                    }
                                    if (gstSettings1 != null)
                                    {
                                        if (gstSettings1 == currency1.Code)
                                        {
                                            throw new Exception("Gst Currency (" + gstSettings1 + ") Cannot Be Inactive");
                                        }
                                    }
                                }
                            }
                        }
                        foreach (string ids in enableOrDisable.id)
                        {
                            int intId;
                            bool result = Int32.TryParse(ids, out intId);
                            int statusVal = 0;
                            long companyId = 0;
                            List<string> lstStatus = enableOrDisable.status;
                            if (lstStatus[statusCount] == RecordStatusEnum.Active.ToString())
                            {
                                statusVal = 2;
                            }
                            else
                            {
                                statusVal = 1;
                            }
                            statusCount++;
                            var Currency = _currencyService.GetByIdandCompanyId(intId, enableOrDisable.CompanyId);
                            if (Currency != null)
                            {
                                var currency = _currencyService.GetById(intId);
                                if (currency != null)
                                {
                                    companyId = currency.CompanyId;
                                }
                                if (companyId == 0)
                                {
                                    using (var context = new MasterModuleContext())
                                    {
                                        context.Database.ExecuteSqlCommand(
                                           string.Format(
                                              " UPDATE {0} SET Status = {1}, ModifiedBy='{2}', ModifiedDate=GETUTCDATE()  WHERE Id = '{3}'",
                                              enableOrDisable.tableName, statusVal, enableOrDisable.UserName, intId));
                                    }
                                }
                                else
                                {
                                    if (result)
                                    {
                                        if (financial1 != null /*&& gstSettings != null*/ && currency != null)
                                        {
                                            //if (financial.BaseCurrency != currency.Code &&
                                            //    gstSettings.GSTRepoCurrency != currency.Code)
                                            if (financial1 != currency.Code)
                                            {
                                                using (var context = new MasterModuleContext())
                                                {
                                                    context.Database.ExecuteSqlCommand(
                                                      string.Format(
                                                         " UPDATE {0} SET Status = {1}, ModifiedBy='{2}', ModifiedDate=GETUTCDATE()  WHERE Id = '{3}'",
                                                         enableOrDisable.tableName, statusVal, enableOrDisable.UserName, intId));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Currency == null)
                                {
                                    using (SqlConnection con = new SqlConnection(Ziraff.FrameWork.SingleTon.CommonConnection.DBConnection))
                                    {
                                        if (con.State != ConnectionState.Open)
                                            con.Open();
                                        SqlCommand cmd = new SqlCommand("Bean_Currency_Insertion", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@companyId", enableOrDisable.CompanyId);
                                        cmd.Parameters.AddWithValue("@CurrencyId", intId);
                                        cmd.Parameters.AddWithValue("@UserCreated", enableOrDisable.UserName);
                                        cmd.ExecuteNonQuery();
                                        if (con.State != ConnectionState.Closed)
                                            con.Close();
                                    }
                                }
                                //var financial = _financialSettingService.GetFinancialNyCompanyId(companyId);
                                //var gstSettings = _gSTSettingService.GetGSTSettingByCompanyId(companyId);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;

            }
            //MultiCurrencySetting multicurrency = _multiCurrencySettingService.Queryable().Where(x => x.CompanyId == enableOrDisable.CompanyId).FirstOrDefault();
            //string baseCurrencys = string.Join(",", _currencyService.Queryable().Where(x => x.CompanyId == enableOrDisable.CompanyId && x.Status == RecordStatusEnum.Active).Select(x => x.Code)).ToString();
            //if (multicurrency != null)
            //{

            //    //if (multicurrency.IsAutomated == true)
            //    //{
            //    var url = "https://data.fixer.io/api/latest?access_key=49a0338b2cf1c26023b2a28f719b1dd2&base=" + multicurrency.BaseCurrency + "&symbols=" + baseCurrencys;
            //    var currencyRates = _download_serialized_json_data<AppsWorld.CommonModule.Models.CurrencyModel>(url);
            //    decimal sgdorusdValue;
            //    string baseCurrency = multicurrency.BaseCurrency;
            //    var value = currencyRates.Rates.TryGetValue(baseCurrency, out sgdorusdValue);
            //    SqlConnection con =
            //   new SqlConnection(ConfigurationManager.ConnectionStrings["AppsWorldDBContext"].ToString());
            //    con.Open();
            //    foreach (var rates in currencyRates.Rates)
            //    {
            //        SqlCommand cmd = new SqlCommand("UpdateFrorex", con);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@currencyValue", rates.Value);
            //        cmd.Parameters.AddWithValue("@companyId", multicurrency.CompanyId);
            //        cmd.Parameters.AddWithValue("@date", currencyRates.Date);
            //        cmd.Parameters.AddWithValue("@currency", rates.Key);
            //        int i = cmd.ExecuteNonQuery();
            //    }

            //    //}
            //}

            return enableOrDisable;

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

        public IEnumerable<Currency> GetCurrencyById(long id)
        {
            List<Currency> lstCurrency = _currencyService.Queryable().ToList();
            //  _redisService.GetDataFromRedisCache<Currency>("GetAllCurrencies").ToList();
            try
            {
                //   LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,CurrencyLoggingValidation.Log_Currency_LU_GetByCurrenciesEdit_Request_Message);
                //if (lstCurrency.Count == 0 || lstCurrency == null)
                //{
                //    //lstCurrency =
                //    //    _redisService.GetDataFromRedisCache<Currency>("GetAllCurrencies",
                //    //        new CacheModel<Currency>() { Data = _currencyRepository.Queryable().AsEnumerable().ToList() })
                //    //        .ToList();
                //    //   LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,CurrencyLoggingValidation.Log_Currency_LU_GetByCurrenciesEdit_SuccessFully_Message);

                //}

            }
            catch (Exception ex)
            {
                // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,CurrencyLoggingValidation.Log_Currency_LU_GetByCurrenciesEdit_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return lstCurrency.AsEnumerable<Currency>().Where(x => x.Id == id).AsEnumerable();
            //return _CurrencyRepository.Queryable().Where(x => x.Code == code).AsEnumerable();
        }

        public Currency Save(Currency currency, string name)
        {
            string EventStatus = null;
            string _errors = CommonValidation.ValidateObject(currency);

            if (!string.IsNullOrEmpty(_errors))
            {
                //throw new Exception(_errors);
                throw new Exception(_errors);

            }
            //_redisService.InvalidateCache<Currency>("GetAllCurrencies");
            #region Previous code
            //var _currencySelect = _currencyService.Query(e => e.Code == currency.Code && e.CompanyId == currency.CompanyId).Select();
            //var _currencyDB = _currencyService.Query(e => e.Code == currency.Code && e.CompanyId == currency.CompanyId).Select();
            //var currencyEdit = _currencyService.Query(e => e.Id == currency.Id && e.Code == currency.Code && e.CompanyId == currency.CompanyId).Select();
            ////string _name = HttpContext.Current.User.Identity.Name;
            //DateTime _date = DateTime.UtcNow;

            //if (_currencySelect.Any() && !currencyEdit.Any() && !_currencyDB.Any())
            //{
            //    Currency _currencyNew = _currencySelect.FirstOrDefault();

            //    _currencyNew.CompanyId = currency.CompanyId;
            //    _currencyNew.Name = currency.Name;
            //    _currencyNew.Code = currency.Code;
            //    _currencyNew.RecOrder = currency.RecOrder;
            //    _currencyNew.Status = currency.Status;
            //    _currencyNew.DefaultValue = currency.DefaultValue;
            //    _currencyNew.ModifiedBy = currency.ModifiedBy;
            //    _currencyNew.ModifiedDate = _date;

            //    _currencyNew.ObjectState = ObjectState.Modified;
            //    _currencyService.Update(_currencyNew);
            //}
            //else if (_currencySelect.Any() && currencyEdit.Any())
            //{
            //    Currency _currencyNew = _currencySelect.FirstOrDefault();

            //    _currencyNew.CompanyId = currency.CompanyId;
            //    _currencyNew.Name = currency.Name;
            //    _currencyNew.Code = currency.Code;
            //    _currencyNew.RecOrder = currency.RecOrder;
            //    _currencyNew.Status = currency.Status;
            //    _currencyNew.DefaultValue = currency.DefaultValue;
            //    _currencyNew.ModifiedBy = currency.ModifiedBy;
            //    _currencyNew.ModifiedDate = _date;
            //    EventStatus = "Updated";
            //    _currencyNew.ObjectState = ObjectState.Modified;
            //    _currencyService.Update(_currencyNew);
            //}
            //else if (!_currencySelect.Any() && !_currencyDB.Any())
            //{
            //    long value = 0;
            //    var GetAllCurrencies = _currencyService.Queryable().ToList();
            //    if (GetAllCurrencies.Any())
            //    {
            //        value = Convert.ToInt64(GetAllCurrencies.Max(c => c.Id));
            //    }
            //    currency.CreatedDate = _date;
            //    currency.Id = (value + 1);
            //    EventStatus = "Inserted";
            //    currency.ObjectState = ObjectState.Added;
            //    _currencyService.Insert(currency);
            //}
            //else
            //{
            //    string responseMessage = "Currency Code Already Exist";
            //    throw new Exception(responseMessage);
            //}
            #endregion Previous code

            var currencyEdit = _currencyService.Query(e => e.Id == currency.Id && e.Code == currency.Code && e.CompanyId == currency.CompanyId).Select().FirstOrDefault();
            //string _name = HttpContext.Current.User.Identity.Name;
            DateTime _date = DateTime.UtcNow;

            if (currencyEdit != null)
            {
                //Currency _currencyNew = _currencySelect.FirstOrDefault();
                currencyEdit.CompanyId = currency.CompanyId;
                currencyEdit.Name = currency.Name;
                currencyEdit.Code = currency.Code;
                currencyEdit.RecOrder = currency.RecOrder;
                currencyEdit.Status = currency.Status;
                currencyEdit.DefaultValue = currency.DefaultValue;
                currencyEdit.ModifiedBy = currency.ModifiedBy;
                currencyEdit.ModifiedDate = _date;
                currencyEdit.ObjectState = ObjectState.Modified;
                _currencyService.Update(currencyEdit);
            }
            else
            {
                var _currencySelect = _currencyService.Query(e => e.Code == currency.Code).Select().FirstOrDefault();
                if (_currencySelect != null)
                {
                    string responseMessage = "Currency Code Already Exist";
                    throw new Exception(responseMessage);
                }
                long value = 0;
                var GetAllCurrencies = _currencyService.Queryable().ToList();
                if (GetAllCurrencies.Any())
                {
                    value = Convert.ToInt64(GetAllCurrencies.Max(c => c.Id));
                }
                currency.CreatedDate = _date;
                currency.Id = (value + 1);
                EventStatus = "Inserted";
                currency.ObjectState = ObjectState.Added;
                _currencyService.Insert(currency);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                // LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService,CurrencyLoggingValidation.Log_Currency_Save_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }

            return currency;
        }

        public decimal? GetCreditlimit(Guid id, decimal? creditLimitAmount/*, bool? isMultiCurrency*/)
        {
            //creditLimitAmount = creditLimitAmount == null ? 0 : creditLimitAmount;
            if (creditLimitAmount == null)
                return creditLimitAmount;
            decimal? creditamount = 0;
            List<InvoiceDetail> details = new List<InvoiceDetail>();
            List<CreditNoteApplication> lstCNApplication = new List<CreditNoteApplication>();
            var lstInvoice = _invoiceService.GetAllByEntityId(id);
            var lstDebitNote = _debitNoteService.GetDebitNoteByEntity(id);
            var lstCN = _invoiceService.GetAllCnByEntityId(id);
            var lstreceipt = _receiptService.GetAllReceiptByEntity(id);
            decimal? invoiceAmount = 0, debitAmount = 0, receiptAmount = 0, creditAmount = 0;

            //if (invoice.Count > 0)
            //{
            //    foreach (var inv in invoice)
            //    {
            //        details.AddRange(inv.InvoiceDetails);
            //    }
            //    creditamount = isMultiCurrency == true ? details.Sum(c => c.DocTotalAmount) : details.Sum(x => x.DocAmount);
            //}
            if (lstInvoice.Any())
                invoiceAmount = lstInvoice.Sum(x => x.GrandTotal);
            //if (lstCN.Any())
            //{
            //    foreach (var creditNote in lstCN)
            //    {
            //        var creditNoteApplication = _creditNoteApplicationService.GetCNAppById(creditNote.Id);
            //        if (creditNoteApplication != null)
            //            lstCNApplication.Add(creditNoteApplication);
            //    }
            //    creditAmount = lstCNApplication.Sum(c => c.CreditAmount);
            //}
            if (lstCN.Any())
                creditAmount = lstCN.Sum(c => c.BalanceAmount);
            if (lstDebitNote.Any())
                debitAmount = lstDebitNote.Sum(x => x.GrandTotal);
            if (lstreceipt.Any())
                receiptAmount = lstreceipt.Sum(x => x.ReceiptApplicationAmmount);
            creditamount = (creditLimitAmount) - ((invoiceAmount + debitAmount) - (creditAmount + receiptAmount));
            return creditamount;
        }

        #region AuditIncidentalSetup
        public IncidentalModel GetIncidetalModel(long companyId, string connectionString)
        {

            Item item = _itemService.GetIncidentalSetup(companyId);
            IncidentalModel incidentalModel = new IncidentalModel();
            try
            {
                if (item != null)
                {
                    incidentalModel.Id = item.Id;
                    incidentalModel.Code = item.Code;
                    incidentalModel.CompanyId = item.CompanyId;
                    incidentalModel.COAId = item.COAId;
                    incidentalModel.Description = item.Description;
                    incidentalModel.IsIncidental = item.IsIncidental;
                    incidentalModel.UserCreated = item.UserCreated;
                    incidentalModel.CreatedDate = item.CreatedDate;
                    incidentalModel.ModifiedBy = item.ModifiedBy;
                    incidentalModel.ModifiedDate = item.ModifiedDate;
                    incidentalModel.Status = item.Status;
                    incidentalModel.DocumentId = item.DocumentId;
                    incidentalModel.IsExternalData = item.IsExternalData;
                    incidentalModel.DefaultTaxcodeId = item.DefaultTaxcodeId;
                    incidentalModel.IsAllowableNotAllowableActivated = item.IsAllowableNotAllowableActivated;
                    incidentalModel.AllowDisAllow = item.AllowableDis;
                    incidentalModel.IsSaleItem = item.IsSaleItem;
                    incidentalModel.IsPLAccount = item.IsPLAccount;
                    incidentalModel.IncidentalType = item.IncidentalType;
                    //incidentalModel.TaxCodeLU = _taxCodeService.GetAllTaxCode(companyId);
                    //incidentalModel.ChartOfAccountLU = _chartOfAccountService.GetChartOfAccountByCId(companyId);
                }
                else
                {
                    incidentalModel.Id = Guid.Empty;
                    incidentalModel.Status = RecordStatusEnum.Active;
                    incidentalModel.CreatedDate = DateTime.UtcNow;
                    incidentalModel.CompanyId = companyId;
                    incidentalModel.GroupByLU = _controlCodeCategoryService.GetByCategoryCodeCategory1(companyId, ControlCodeConstants.Control_codes_Incidental_Type);
                }
                incidentalModel.TaxCodeLU = _taxCodeService.GetAllTaxCode(0);
                List<COALookup<string>> lstEditCoa = null;
                List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
                //string coaName = COANameConstants.Revenue;
                List<long> accType = _accountTypeService.GetAllAccounyTypeByNameByCOA(companyId, coaName);
                List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountTypeCOA(accType);
                incidentalModel.ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Code = x.Code,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status
                    //IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                }).OrderBy(d => d.Name).ToList();

                if (item != null)
                {
                    long CoaIds = item.COAId;
                    bool isExist = (incidentalModel.ChartOfAccountLU.Where(a => a.Id == CoaIds).Any());
                    if (isExist == false)
                    {
                        lstEditCoa = chartofaAccount.Where(x => x.Id == CoaIds).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status
                        })/*.ToList())*/.OrderBy(d => d.Name).ToList();
                        incidentalModel.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                }

                //incidentalModel.ChartOfAccountLU = _chartOfAccountService.GetChartOfAccountByCId(companyId);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return incidentalModel;
        }

        public List<IncidentalModel> GetListOfItemWF(long companyId, long documentId)
        {
            List<IncidentalModel> lstIncidentalItem = new List<IncidentalModel>();
            var lstItem = _itemService.GetAllItemByDidAndCid(companyId, documentId);
            foreach (Item item in lstItem)
            {
                IncidentalModel incidentalModel = new IncidentalModel();
                incidentalModel.Id = item.Id;
                incidentalModel.Code = item.Code;
                incidentalModel.CompanyId = item.CompanyId;
                incidentalModel.COAId = item.COAId;
                incidentalModel.Description = item.Description;
                incidentalModel.IsIncidental = item.IsIncidental;
                incidentalModel.UserCreated = item.UserCreated;
                incidentalModel.CreatedDate = item.CreatedDate;
                incidentalModel.ModifiedBy = item.ModifiedBy;
                incidentalModel.ModifiedDate = item.ModifiedDate;
                incidentalModel.Status = item.Status;
                incidentalModel.IsExternalData = item.IsExternalData;
                incidentalModel.DefaultTaxcodeId = item.DefaultTaxcodeId;
                incidentalModel.DocumentId = item.DocumentId;
                incidentalModel.IsPLAccount = item.IsPLAccount;
                incidentalModel.AllowDisAllow = item.AllowableDis;
                lstIncidentalItem.Add(incidentalModel);
            }
            return lstIncidentalItem;
        }
        #endregion

        #region CurserInitialSetup

        private void UpadateInitialSetup(long companyid, long moduledetailid)
        {
            var moduleMaster = _moduleMasterService.GetModuleMaster(companyid, "Bean Cursor");
            if (moduleMaster != null)
            {
                var listFeaturesId = _featureService.GetFeatureIdsByMId(moduleMaster.Id);
                if (listFeaturesId.Count > 0)
                {
                    var listCompanyFeatures = _companyFeatureService.GetFeaturesByCidandFid(companyid, listFeaturesId);
                    if (listCompanyFeatures.Count == listCompanyFeatures.Where(x => x.IsChecked == true).Count())
                    {
                        var getFinancialSetting = _financialSettingService.GetFinancialByCid(companyid);
                        if (getFinancialSetting)
                        {
                            var getMultiCurrency = _multiCurrencySettingService.Getmulticurrency(companyid);
                            if (getMultiCurrency != null)
                            {
                                //var getSegmentMaster = _segmentMasterService.GetAllSegmentMaster(companyid);
                                //if (getSegmentMaster > 0)
                                //{
                                //    FillRestClientCall(companyid, moduledetailid);
                                //}
                            }
                        }
                    }
                }
            }
        }
        private static void FillRestClientCall(long companyid, long moduledetailid)
        {
            string companyId = companyid.ToString();
            string moduledetailId = moduledetailid.ToString();
            string url = ConfigurationManager.AppSettings["AdminUrl"].ToString();
            List<List<string, string>> lstParameter = new List<List<string, string>>();
            lstParameter.Add(new List<string, string>() { Name = "companyId", Value = companyId });
            lstParameter.Add(new List<string, string>() { Name = "moduleDetailId", Value = moduledetailId });
            var requestUrl = "api/common/updatesetupdoneininitialcursorsetup";
            var response = RestHelper.ZGet(url, requestUrl, lstParameter);
            if (response.ErrorMessage != null)
            {
                Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
            }
        }

        #endregion

        #region EntityAndItemName
        private string GetEntityName(long companyId, string entityName)
        {
            string name = null;
            int i = 0;
            var _beanEntityNameCheck = _beanEntityService.GetBeanEntityNameChec(entityName, companyId);


            if (_beanEntityNameCheck.Any())
            {
                foreach (var names in _beanEntityNameCheck)
                {
                    i++;
                    //name = entityName + "-" + i;
                    // var eNames = _beanEntityNameCheck.Where(a => a.Name == name).FirstOrDefault();
                    var entity = _beanEntityNameCheck.OrderByDescending(a => a.CreatedDate).Select(n => n.Name).FirstOrDefault();

                    //if (eNames == null)
                    name = entityName + "-" + i;
                }
            }
            else
                name = entityName;
            return name;
        }

        private string GetItemName(long companyId, string itemName)
        {
            string name = null;
            int i = 0;
            var lstItem = _itemService.GetAllItemName(companyId, itemName);
            if (lstItem.Any())
            {
                foreach (var code in lstItem)
                {
                    i++;
                    name = itemName + "-" + i;
                    var names = lstItem.Where(a => a.Code == name).FirstOrDefault();
                    if (names == null)
                        name = itemName + "-" + i;
                }
            }
            else
                name = itemName;
            return name;
        }
        #endregion







        #region Contacts

        public ContactModel CreateContactModel(long companyId)
        {
            ContactModel contactModel = new ContactModel();
            contactModel.CompanyId = companyId;
            contactModel.Status = RecordStatusEnum.Active;
            contactModel.ContactDetailModel = new ContactDetailModel();
            contactModel.ContactDetailModel.Status = RecordStatusEnum.Active;
            contactModel.ContactDetailModel.Addresses = new List<Address>();
            contactModel.Addresses = new List<Address>();
            return contactModel;
        }
        public List<ContactModel> GetEntityContacts(long companyId, Guid entityId)
        {
            try
            {
                List<ContactModel> _lstContact = new List<ContactModel>();
                BeanEntity beanEntity = _beanEntityService.GetBeanEntityByIdAndCompanyId(companyId, entityId);
                var lstContactDetail = _contactService.GetContactForAccount(companyId, entityId, "Entity");
                var lstContact = lstContactDetail.Select(c => c.Contact).ToList();
                var lstGetAllMedias = _contactService.GetAllMediaRepo(lstContactDetail.Select(d => d.Contact.PhotoId).ToList());
                var getAllAddressBookByAddId = _addressService.GetAddressByAddId(lstContactDetail.Select(d => d.Contact.Id).ToList());
                foreach (var item in lstContactDetail)
                {
                    ContactModel _contact = new ContactModel();
                    ContactDetailModel _contactDetailModel = new ContactDetailModel();
                    FillContacts(lstContact, item, _contact, lstGetAllMedias, getAllAddressBookByAddId);
                    FillContactDetails(item, _contactDetailModel, beanEntity, getAllAddressBookByAddId);
                    GetIsAssociate(_contact, _contactDetailModel);
                    _contact.ContactDetailModel = _contactDetailModel;
                    _lstContact.Add(_contact);
                }
                return _lstContact.OrderByDescending(a => a.ContactDetailModel.IsPrimaryContact == true).ThenBy(a => a.ContactDetailModel.CreatedDate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetIsAssociate(ContactModel _contact, ContactDetailModel _contactDetailModel)
        {
            bool isAssociate = _contactDetailService.GetAssociationsByContactIdDistinct(_contact.Id, _contact.EntityId);
            if (isAssociate)
            {
                _contact.IsAssociate = true;
                _contactDetailModel.IsAssociate = true;
            }
            else
            {
                _contact.IsAssociate = false;
                _contactDetailModel.IsAssociate = false;
            }
        }
        private void FillContacts(List<Contact> lstContact, ContactDetail item, ContactModel _contact, List<MediaRepository> mediaRepositories, List<Address> addresses)
        {
            item.Contact = lstContact.Find(c => c.Id == item.ContactId);
            var mediaRepo = mediaRepositories?.Find(d => d.Id == item.Contact.PhotoId);
            _contact.Id = item.Contact.Id;
            _contact.ContactId = item.Contact.Id;
            _contact.CompanyId = item.Contact.CompanyId;
            _contact.EntityId = (Guid)item.EntityId;
            _contact.Salutation = item.Contact.Salutation;
            _contact.Communication = item.Contact.Communication;
            _contact.CountryOfResidence = item.Contact.CountryOfResidence;
            _contact.DOB = item.Contact.DOB;
            _contact.LastName = item.Contact.LastName;
            _contact.FirstName = item.Contact.FirstName;
            _contact.IdNo = item.Contact.IdNo ?? string.Empty;
            _contact.IdType = item.Contact.IdType == "0" ? "" : item.Contact.IdType;
            //photo
            _contact.PhotoId = item.Contact.PhotoId;
            _contact.SourceType = "company";
            _contact.MediaType = "Audio";
            _contact.Original = "default";
            if (_contact.PhotoId != null && mediaRepo != null)
            {
                //var mediaRepo = _contactService.GetMediaRepo((Guid)_contact.PhotoId);
                _contact.Small = mediaRepo.Small;
                _contact.Large = mediaRepo.Large;
                _contact.Medium = mediaRepo.Medium;
            }
            _contact.Status = item.Status;
            //_contact.Addresses = _addressService.Query(a => a.AddTypeId == _contact.Id).Include(c => c.AddressBook).Select().ToList();
            //_contact.Addresses = _contact.Addresses.OrderBy(c => c.AddressBook.RecOrder).ToList();
            _contact.Addresses = addresses?.OrderBy(c => c.AddressBook.RecOrder).ToList();
        }
        private void FillContactDetails(ContactDetail item, ContactDetailModel _contactDetailModel, BeanEntity beanEntity, List<Address> addresses)
        {
            _contactDetailModel.Id = item.Id;
            _contactDetailModel.ContactId = item.ContactId;
            _contactDetailModel.CompanyName = beanEntity.Name;
            _contactDetailModel.Designation = item.Designation;
            _contactDetailModel.EntityId = item.EntityId;
            _contactDetailModel.EntityType = item.EntityType;
            _contactDetailModel.Communication = item.Communication;
            _contactDetailModel.IsPrimaryContact = item.IsPrimaryContact;
            _contactDetailModel.IsCopy = item.IsCopy ?? false;
            _contactDetailModel.Remarks = item.Remarks;
            _contactDetailModel.Matters = item.Matters;
            _contactDetailModel.IsReminderReceipient = item.IsReminderReceipient;
            _contactDetailModel.CreatedDate = item.CreatedDate;
            _contactDetailModel.UserCreated = item.UserCreated;
            _contactDetailModel.ModifiedBy = item.ModifiedBy;
            _contactDetailModel.ModifiedDate = item.ModifiedDate;
            _contactDetailModel.Status = item.Status;
            _contactDetailModel.Addresses = addresses?.OrderBy(c => c.AddressBook.RecOrder).ToList();
        }
        public void SaveBeanEntityContact(ContactModel contactModel, Guid entityId, bool isSaveChange, string ConnectionString, BeanEntityModel beDTO = null, bool? isWorkFlow = null)
        {
            try
            {
                contactModel.CompanyId = beDTO == null ? contactModel.CompanyId : beDTO.CompanyId;
                Contact contact = _contactService.GetContact(contactModel.Id, (long)contactModel.CompanyId);
                ContactDuplicateValidation(contactModel);
                if (isWorkFlow == true)
                {
                    BeanEntity _beanEntity = _beanEntityService.GetBeanEntityByClientId((long)contactModel.CompanyId, entityId);
                    entityId = _beanEntity.Id;
                }

                if (contact != null)
                {
                    contactModel.Id = contact.Id;
                    FillContact(contactModel, contact);
                    contact.ObjectState = ObjectState.Modified;
                    _contactService.Update(contact);
                }
                else
                {
                    Contact contactInsert = new Contact
                    {
                        Id = Guid.NewGuid()
                    };
                    contactModel.Id = contactInsert.Id;
                    contactInsert.CompanyId = contactModel.CompanyId;
                    FillContact(contactModel, contactInsert);
                    contactInsert.Status = RecordStatusEnum.Active;
                    contactInsert.ObjectState = ObjectState.Added;
                    contactInsert.UserCreated = contactModel.UserCreated;
                    contactInsert.CreatedDate = DateTime.UtcNow;
                    _contactService.Insert(contactInsert);
                }
                SaveAddressByRestClient(contactModel.Addresses, contactModel.Id, Constrant.Contact, false);
                ContactDeailSaving(entityId, contactModel);
                //**if copy selected communicatipon
                UpdateCopyCommunicationandAddress(contactModel);

                if (!isSaveChange)
                {
                    _unitOfWorkAsync.SaveChanges();
                    //**update adress is copy selected
                    Sp_CopyAddresses(contactModel, ConnectionString);
                    SqlConnection con = new SqlConnection(ConnectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[Common_Sync_MasterData]", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", contactModel.CompanyId);
                    cmd.Parameters.AddWithValue("@Type", Constrant.Entity);
                    cmd.Parameters.AddWithValue("@SourceId", entityId);
                    cmd.Parameters.AddWithValue("@Action", Constrant.Edit);
                    int Count = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ContactDuplicateValidation(ContactModel contactModel)
        {
            var entityContacts = _contactDetailService.Queryable().Where(a => a.EntityId == contactModel.ContactDetailModel.EntityId && a.ContactId == contactModel.Id).Count();
            if (entityContacts > 1)
            {
                throw new Exception("Contact already created");
            }
        }
        private void FillContact(ContactModel contactModel, Contact contact)
        {
            contact.Salutation = contactModel.Salutation;
            contact.PhotoId = contactModel.PhotoId;
            contact.FirstName = contactModel.FirstName;
            contact.LastName = contactModel.LastName;
            contact.Communication = contactModel.Communication;
            contact.DOB = contactModel.DOB;
            contact.IdType = contactModel.IdType;
            contact.IdNo = contactModel.IdNo;
            contact.CountryOfResidence = contactModel.CountryOfResidence;
        }
        private void ContactDeailSaving(Guid entityId, ContactModel contactModel)
        {
            if (contactModel != null && contactModel.ContactDetailModel != null)
            {
                ContactDetail upDateContactDetail = _contactDetailService.GetClientContact(contactModel.Id, entityId);
                if ((upDateContactDetail != null && upDateContactDetail.IsPrimaryContact != contactModel.ContactDetailModel.IsPrimaryContact) || (upDateContactDetail == null && contactModel.ContactDetailModel.IsPrimaryContact == true))
                {
                    ContactDetail updatePrimaryContact = _contactDetailService.GetLeadPrimaryContact(entityId);
                    if (updatePrimaryContact != null)
                    {
                        updatePrimaryContact.IsPrimaryContact = false;
                        updatePrimaryContact.ObjectState = ObjectState.Modified;
                        _contactDetailService.Update(updatePrimaryContact);
                    }
                }

                if (upDateContactDetail != null)
                {
                    FillContactDetail(contactModel, upDateContactDetail);
                    contactModel.ContactDetailModel.Id = upDateContactDetail.Id;
                    upDateContactDetail.CreatedDate = contactModel.ContactDetailModel.CreatedDate;
                    upDateContactDetail.ModifiedDate = DateTime.UtcNow;
                    upDateContactDetail.ObjectState = ObjectState.Modified;
                    _contactDetailService.Update(upDateContactDetail);
                }
                else
                {
                    ContactDetail insertContactDetail = new ContactDetail();
                    insertContactDetail.Id = Guid.NewGuid();
                    contactModel.ContactDetailModel.Id = insertContactDetail.Id;
                    contactModel.ContactDetailModel.EntityId = entityId;
                    insertContactDetail.ContactId = contactModel.Id;
                    insertContactDetail.EntityId = entityId;
                    insertContactDetail.DocId = contactModel.ContactDetailModel.DocId;
                    insertContactDetail.DocType = contactModel.ContactDetailModel.DocType;
                    FillContactDetail(contactModel, insertContactDetail);
                    insertContactDetail.CreatedDate = DateTime.UtcNow;
                    insertContactDetail.ModifiedDate = contactModel.ContactDetailModel.ModifiedDate;
                    insertContactDetail.ObjectState = ObjectState.Added;
                    _contactDetailService.Insert(insertContactDetail);
                }
                SaveAddressByRestClient(contactModel.ContactDetailModel.Addresses, contactModel.ContactDetailModel.Id, "BeanContact", false);
            }
        }
        private void FillContactDetail(ContactModel contactModel, ContactDetail insertContactDetail)
        {
            insertContactDetail.EntityType = "Entity";
            insertContactDetail.Designation = contactModel.ContactDetailModel.Designation;
            insertContactDetail.Communication = contactModel.ContactDetailModel.Communication;
            insertContactDetail.Matters = contactModel.ContactDetailModel.Matters;
            insertContactDetail.IsPrimaryContact = contactModel.ContactDetailModel.IsPrimaryContact;
            insertContactDetail.IsCopy = contactModel.ContactDetailModel.IsCopy;
            insertContactDetail.RecOrder = 1;
            insertContactDetail.CursorShortCode = "Bean";
            insertContactDetail.UserCreated = contactModel.ContactDetailModel.UserCreated;
            insertContactDetail.ModifiedBy = contactModel.ContactDetailModel.ModifiedBy;
            insertContactDetail.Status = contactModel.ContactDetailModel.Status;
            insertContactDetail.Remarks = contactModel.ContactDetailModel.Remarks;
        }
        public List<Address> SaveAddressByRestClient(List<Address> lstAddress, Guid typeId, string type, bool? isCopy = null)
        {
            try
            {
                SaveAddressModel addressSave = new SaveAddressModel();

                addressSave.ListAddress = lstAddress;
                addressSave.Type = type;
                addressSave.TypeId = typeId;
                addressSave.IsCopy = isCopy;
                SaveAddress(addressSave);
                //var json = RestSharpHelper.ConvertObjectToJason(addressSave);

                //object obj = lstAddress;
                //string url = ConfigurationManager.AppSettings["AdminUrl"];
                //var response = RestSharpHelper.Post(url, "/api/common/saveaddress", json);
                //if (response.ErrorMessage != null)
                //{
                //    Log.Logger.Error(string.Format("Error Message {0}", response));
                //}

                //if (response.StatusCode == HttpStatusCode.OK)
                //{
                //    var data = JsonConvert.DeserializeObject<List<Address>>(response.Content);
                //    lstAddress = data;
                //}
                //else
                //{
                //    throw new Exception(response.Content);
                //}
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return lstAddress;
        }
        public List<Address> SaveAddress(SaveAddressModel saveAddressModel)
        {
            List<Address> returnlstAddress = new List<Address>();
            returnlstAddress = saveAddressModel.ListAddress.Where(c => c.RecordStatus == null).ToList();
            try
            {
                int rec = 1;
                foreach (var address in saveAddressModel.ListAddress)
                {
                    if (address.RecordStatus == "Added")
                    {
                        Address _insertAddress = new Address();
                        AddressBook _insertAddresBook = new AddressBook();

                        _insertAddresBook.Id = Guid.NewGuid();
                        FillAddressBook(_insertAddresBook, address, rec);
                        _insertAddresBook.CreatedDate = DateTime.UtcNow;
                        _insertAddresBook.ObjectState = ObjectState.Added;
                        _addressBookService.Insert(_insertAddresBook);

                        _insertAddress.Id = Guid.NewGuid();
                        _insertAddress.AddressBookId = _insertAddresBook.Id;
                        _insertAddress.AddTypeId = saveAddressModel.TypeId;
                        //_insertAddress.AddTypeIdInt = address.AddTypeIdInt;
                        _insertAddress.AddTypeIdInt = saveAddressModel.AddTypeIntId;
                        _insertAddress.CopyId = address.CopyId;
                        _insertAddress.AddType = saveAddressModel.Type;
                        _insertAddress.AddSectionType = address.AddSectionType;
                        _insertAddress.Status = address.Status;
                        _insertAddress.RecordStatus = address.RecordStatus;
                        _insertAddress.ObjectState = ObjectState.Added;
                        _addressService.Insert(_insertAddress);

                        if (saveAddressModel.IsCopy == true)
                        {
                            if (address.CopyId != null && address.CopyId != Guid.Empty)
                            {
                                var updateCopyId = _addressService.GetAddressId(address.CopyId);
                                if (updateCopyId != null)
                                {
                                    updateCopyId.CopyId = _insertAddress.Id;
                                    updateCopyId.ObjectState = ObjectState.Modified;
                                    _addressService.Update(updateCopyId);
                                }
                            }
                        }
                        _insertAddress.AddressBook = _insertAddresBook;
                        returnlstAddress.Add(_insertAddress);
                    }
                    else if (address.RecordStatus == "Modified")
                    {
                        var updateAddress = _addressService.GetAddressId(address.Id);
                        if (updateAddress != null)
                        {

                            if (updateAddress.CopyId != null && updateAddress.CopyId != Guid.Empty)
                            {
                                var updateCopyAddress = _addressService.GetAddressId((Guid)updateAddress.CopyId);
                                if (updateCopyAddress != null)
                                {
                                    var updateCopyAddresBook = _addressBookService.GetAddressBook(updateCopyAddress.AddressBookId);
                                    if (updateCopyAddresBook != null)
                                    {
                                        FillAddressBook(updateCopyAddresBook, address, rec);
                                        updateCopyAddresBook.ModifiedDate = DateTime.UtcNow;
                                        updateCopyAddresBook.ObjectState = ObjectState.Modified;
                                        _addressBookService.Update(updateCopyAddresBook);
                                    }
                                }
                            }
                            var updateAddresBook = _addressBookService.GetAddressBook(updateAddress.AddressBookId);
                            if (updateAddresBook != null)
                            {
                                FillAddressBook(updateAddresBook, address, rec);
                                updateAddresBook.ModifiedDate = DateTime.UtcNow;
                                updateAddresBook.ObjectState = ObjectState.Modified;
                                _addressBookService.Update(updateAddresBook);
                            }
                        }
                        returnlstAddress.Add(updateAddress);
                    }
                    else if (address.RecordStatus == "Deleted")
                    {
                        var deleteAddress = _addressService.GetAddressId(address.Id);
                        if (deleteAddress != null)
                        {
                            if (deleteAddress.CopyId != null && deleteAddress.CopyId != Guid.Empty)
                            {
                                var deleteCopyAddress = _addressService.GetAddressId(deleteAddress.CopyId);
                                if (deleteCopyAddress != null)
                                {
                                    var deleteCopyAddresBook = _addressBookService.GetAddressBook(deleteCopyAddress.AddressBookId);
                                    if (deleteCopyAddresBook != null)
                                    {
                                        deleteCopyAddress.ObjectState = ObjectState.Deleted;
                                        _addressService.Delete(deleteCopyAddress);
                                        deleteCopyAddresBook.ObjectState = ObjectState.Deleted;
                                        _addressBookService.Delete(deleteCopyAddresBook);
                                    }
                                }
                            }

                            var deleteAddresBook = _addressBookService.GetAddressBook(deleteAddress.AddressBookId);
                            if (deleteAddresBook != null)
                            {
                                deleteAddress.ObjectState = ObjectState.Deleted;
                                _addressService.Delete(deleteAddress);
                                deleteAddresBook.ObjectState = ObjectState.Deleted;
                                _addressBookService.Delete(deleteAddresBook);
                            }
                        }
                    }
                    rec++;
                }
                _unitOfWorkAsync.SaveChanges();
                return returnlstAddress;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FillAddressBook(AddressBook addressBook, Address address, int rec)
        {
            addressBook.BlockHouseNo = address.AddressBook.BlockHouseNo;
            addressBook.BuildingEstate = address.AddressBook.BuildingEstate;
            addressBook.City = address.AddressBook.City;
            addressBook.Country = address.AddressBook.Country;
            addressBook.Email = address.AddressBook.Email;
            addressBook.IsLocal = address.AddressBook.IsLocal;
            addressBook.Latitude = address.AddressBook.Latitude;
            addressBook.Longitude = address.AddressBook.Longitude;
            addressBook.ModifiedBy = address.AddressBook.ModifiedBy;
            addressBook.Phone = address.AddressBook.Phone;
            addressBook.PostalCode = address.AddressBook.PostalCode;
            addressBook.RecOrder = rec;
            addressBook.Remarks = address.AddressBook.Remarks;
            addressBook.State = address.AddressBook.State;
            addressBook.Status = address.AddressBook.Status;
            addressBook.Street = address.AddressBook.Street;
            addressBook.UnitNo = address.AddressBook.UnitNo;
            addressBook.UserCreated = address.AddressBook.UserCreated;
            addressBook.Version = address.AddressBook.Version;
            addressBook.Website = address.AddressBook.Website;
        }

        private void UpdateCopyCommunicationandAddress(ContactModel contactModel)
        {
            List<ContactDetail> lstContactDetail = _contactDetailService.GetAllClientContactsbyContactId(contactModel.Id).Where(c => c.Id != contactModel.ContactDetailModel.Id).ToList();
            foreach (var upDateCommunicationandAddress in lstContactDetail)
            {
                upDateCommunicationandAddress.Communication = contactModel.Communication;
                _contactDetailService.Update(upDateCommunicationandAddress);
            }
        }
        private void Sp_CopyAddresses(ContactModel TObject, string ConnectionString)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            SqlCommand selectCommand = new SqlCommand("Sp_UpdtAddressBookTbl", connection);
            selectCommand.CommandType = CommandType.StoredProcedure;
            selectCommand.Parameters.AddWithValue("@ContactId", TObject.Id);
            int i = selectCommand.ExecuteNonQuery();
            connection.Close();
            //Update Addresee Copy id 
            connection.Open();
            SqlCommand selectCommand1 = new SqlCommand("Sp_UpdtAddressCopyIds", connection);
            selectCommand1.CommandType = CommandType.StoredProcedure;
            selectCommand1.Parameters.AddWithValue("@ContactId", TObject.Id);
            int j = selectCommand1.ExecuteNonQuery();
            connection.Close();
        }

        #endregion Contacts




        #region common_folder_creation
        public bool CreateDynamicFolder(long companyId, string recordName, string heading)
        {
            try
            {
                //string CursorShortName = "Bean";
                CommonFolderModel folderModel = new CommonFolderModel();
                folderModel.CompanyId = companyId;
                folderModel.CursorName = "Bean Cursor";
                folderModel.CursorShortCode = "Bean";
                folderModel.Heading = heading;
                string newdirectiory = _commonAppService.StringCharactersReplaceFunction(recordName);
                //recordName.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
                // .Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
                folderModel.LeadName = newdirectiory;

                var json = RestHelper.ConvertObjectToJason(folderModel);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureUrl"];
                    // string url = "http://localhost:64453/";
                    var response = RestHelper.ZPost(url, "api/storage/createdynamicfolder", json);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<bool>(response.Content);
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
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion







        #region


        public IncidentalNewModel GetIncidetalModelNew(Guid id, long companyId, string connectionString)
        {

            Item item = _itemService.GetByIdAndCompanyId(id, companyId);
            IncidentalNewModel incidentalModel = new IncidentalNewModel();
            try
            {
                if (item != null)
                {
                    incidentalModel.Id = item.Id;
                    incidentalModel.Code = item.Code;
                    incidentalModel.IncidentalType = item.IncidentalType;
                    incidentalModel.CompanyId = item.CompanyId;
                    incidentalModel.COAId = item.COAId;
                    incidentalModel.Description = item.Description;
                    incidentalModel.IsIncidental = item.IsIncidental;
                    incidentalModel.UserCreated = item.UserCreated;
                    incidentalModel.CreatedDate = item.CreatedDate;
                    incidentalModel.ModifiedBy = item.ModifiedBy;
                    incidentalModel.ModifiedDate = item.ModifiedDate;
                    incidentalModel.Status = item.Status;
                    incidentalModel.DocumentId = item.DocumentId;
                    incidentalModel.IsExternalData = item.IsExternalData;
                    incidentalModel.DefaultTaxcodeId = item.DefaultTaxcodeId;
                    incidentalModel.IsAllowableNotAllowableActivated = item.IsAllowableNotAllowableActivated;
                    incidentalModel.AllowDisAllow = item.AllowableDis;
                    incidentalModel.IsSaleItem = item.IsSaleItem;
                    incidentalModel.IsPLAccount = item.IsPLAccount;
                }
                else
                {
                    incidentalModel.Id = Guid.Empty;
                    incidentalModel.Status = RecordStatusEnum.Active;
                    //incidentalModel.CreatedDate = DateTime.UtcNow;
                    incidentalModel.CompanyId = companyId;
                }
                incidentalModel.TaxCodeLU = _taxCodeService.GetAllTaxCode(0);
                List<COALookup<string>> lstEditCoa = null;
                incidentalModel.IncidentalTypeLU = _controlCodeCategoryService.GetByCategoryCodeCategory1(companyId, ControlCodeConstants.Control_codes_Incidental_Type);
                List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
                List<long> accType = _accountTypeService.GetAllAccounyTypeByNameByCOA(companyId, coaName);
                List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountTypeCOA(accType);
                incidentalModel.ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Code = x.Code,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status
                }).OrderBy(d => d.Name).ToList();

                if (item != null)
                {
                    long CoaIds = item.COAId;
                    bool isExist = (incidentalModel.ChartOfAccountLU.Where(a => a.Id == CoaIds).Any());
                    if (isExist == false)
                    {
                        lstEditCoa = chartofaAccount.Where(x => x.Id == CoaIds).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status
                        }).OrderBy(d => d.Name).ToList();
                        incidentalModel.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return incidentalModel;
        }


        public IncidentalNewModel SaveItems(IncidentalNewModel itemmodel, string _name, string ConnectionString, long companyId)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(itemmodel));
            Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, "ObjectSave", AdditionalInfo);

            Item item = new Item();
            long? COAId = 0;
            string UserCreated = null;
            //item Code Duplicate checking for BenItem
            itemmodel.CompanyId = companyId;

            if (itemmodel.IsExternalData != true ? _itemService.IsDuplicateItem(itemmodel.CompanyId, itemmodel.Id, itemmodel.Code) == true : _itemService.IsExternalDuplicateItem(itemmodel.CompanyId, itemmodel.DocumentId, itemmodel.Code, itemmodel.Id) == true)
            {
                throw new Exception("Duplicate item code. Verify that you’ve entered the correct information.");
            }
            if (itemmodel.IsIncidental == true && itemmodel.IsExternalData == true)
            {
                if (_itemService.GetIncidentalTypeExistOrNot(itemmodel.IncidentalType, companyId, itemmodel.Id))
                {
                    throw new Exception("Incidental Type Should not be duplicate");
                }
            }
            Item _itemSelect = new Item();
            DateTime _date = DateTime.UtcNow;
            if (itemmodel.IsExternalData == true && itemmodel.IsIncidental == true)
                _itemSelect = _itemService.GetWFItem(companyId, itemmodel.DocumentId, itemmodel.Id);
            else if (itemmodel.IsExternalData == true && itemmodel.IsIncidental == null)
                _itemSelect = _itemService.GetWFServiceItem(companyId, itemmodel.DocumentId);
            else
                _itemSelect = _itemService.GetItem(companyId, itemmodel.Id);
            if (_itemSelect != null)
            {
                COAId = _itemSelect.COAId;
                UserCreated = itemmodel.ModifiedBy;
                _itemSelect.Code = itemmodel.Code;
                _itemSelect.ModifiedDate = _date;
                _itemSelect.Description = itemmodel.Description;
                _itemSelect.IncidentalType = itemmodel.IncidentalType;
                _itemSelect.COAId = itemmodel.COAId.Value;
                _itemSelect.DefaultTaxcodeId = itemmodel.DefaultTaxcodeId;
                _itemSelect.IsAllowableNotAllowableActivated = itemmodel.IsAllowableNotAllowableActivated;
                _itemSelect.Status = itemmodel.Status;
                _itemSelect.ModifiedBy = itemmodel.ModifiedBy;
                _itemSelect.IsPLAccount = itemmodel.IsPLAccount;
                _itemSelect.IsSaleItem = itemmodel.IsSaleItem;
                _itemSelect.IsExternalData = itemmodel.IsExternalData;
                _itemSelect.IsIncidental = itemmodel.IsIncidental;
                _itemSelect.DocumentId = itemmodel.DocumentId;
                _itemSelect.UnitPrice = itemmodel.UnitPrice;
                _itemSelect.Notes = itemmodel.Notes;
                _itemSelect.ObjectState = ObjectState.Modified;
                _itemService.Update(_itemSelect);
            }
            else
            {
                item.CreatedDate = _date;
                item.Id = (itemmodel.Id == Guid.Empty) ? itemmodel.Id = Guid.NewGuid() : itemmodel.Id;
                if (itemmodel.IsIncidental == true)
                    item.DocumentId = _itemService.Queryable().Where(c => c.CompanyId == companyId).Max(c => c.DocumentId) == null || _itemService.Queryable().Where(c => c.CompanyId == companyId).Max(c => c.DocumentId) == 0 ? 1 : _itemService.Queryable().Where(c => c.CompanyId == companyId).Max(c => c.DocumentId) + 1;
                item.Code = itemmodel.Code;
                item.Description = itemmodel.Description;
                item.COAId = itemmodel.COAId.Value;
                item.IncidentalType = itemmodel.IncidentalType;
                item.DefaultTaxcodeId = itemmodel.DefaultTaxcodeId;
                item.IsIncidental = itemmodel.IsIncidental;
                item.IsExternalData = itemmodel.IsExternalData;
                item.IsPLAccount = itemmodel.IsPLAccount;
                item.IsSaleItem = itemmodel.IsSaleItem;
                item.IsAllowableNotAllowableActivated = itemmodel.IsAllowableNotAllowableActivated;
                item.CompanyId = companyId;
                item.UserCreated = itemmodel.UserCreated;
                item.UnitPrice = itemmodel.UnitPrice;
                item.Notes = itemmodel.Notes;
                item.Status = RecordStatusEnum.Active;
                item.ObjectState = ObjectState.Added;
                _itemService.Insert(item);
                UserCreated = itemmodel.UserCreated;

            }
            try
            {
                _unitOfWorkAsync.SaveChanges();
                if (itemmodel.IsIncidental == true)
                {
                    string Cursorname = "Admin Cursor";
                    string Ids = (_itemSelect != null ? COAId : 0) + "," + itemmodel.COAId.ToString() + "," + (_itemSelect != null ? _itemSelect.Id.ToString() : item.Id.ToString());
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("COALinkedAccounts_SP", conn);
                    cmd.Parameters.AddWithValue("@Ids", Ids);
                    cmd.Parameters.AddWithValue("@DocType", "Item");
                    cmd.Parameters.AddWithValue("@CusrsorName", Cursorname);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@CreateBy", UserCreated);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();
                }


            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(MasterModuleLoggingValidation.MasterModuleApplicationService, ex, ex.Message);
                throw ex;
            }
            return itemmodel;
        }






        public string DeleteIncidentalItem(Guid id, long companyId, string ConnectionString)
        {
            string UserCreated = null;
            long? COAId = 0;
            try
            {
                var itemExits = _itemService.GetItem(companyId, id);
                if (itemExits != null)
                {
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("[dbo].[Bean_IsItemUsed_Sp]", conn);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@ItemId", id);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    int i = cmd.ExecuteNonQuery();
                    conn.Close();

                    COAId = itemExits.COAId;
                    Guid deleteditemId = itemExits.Id;
                    UserCreated = itemExits.ModifiedBy != null ? itemExits.ModifiedBy : itemExits.UserCreated;
                    itemExits.ObjectState = ObjectState.Deleted;
                    _itemService.Delete(itemExits.Id);
                    _unitOfWorkAsync.SaveChanges();

                    string Cursorname = "Admin Cursor";
                    string Ids = COAId + "," + 0 + "," + deleteditemId;

                    SqlConnection connection = new SqlConnection(ConnectionString);
                    connection.Open();
                    SqlCommand cmd1 = new SqlCommand("COALinkedAccounts_SP", connection);
                    cmd1.Parameters.AddWithValue("@Ids", Ids);
                    cmd1.Parameters.AddWithValue("@DocType", "Item");
                    cmd1.Parameters.AddWithValue("@CusrsorName", Cursorname);
                    cmd1.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd1.Parameters.AddWithValue("@CreateBy", UserCreated);
                    cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                    int j = cmd1.ExecuteNonQuery();
                    connection.Close();
                }
                return MasterModuleValidations.Deleted_Successfully;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public List<IncidentalNewModel> GetAllIncidentalItems(long companyId)
        {

            List<IncidentalNewModel> lstincidentalNewModels = new List<IncidentalNewModel>();
            List<string> coaName = new List<string> { COANameConstants.Revenue, COANameConstants.Other_income };
            List<long> accType = _accountTypeService.GetAllAccounyTypeByNameByCOA(companyId, coaName);
            List<ChartOfAccount> chartofaAccount = _chartOfAccountService.GetChartOfAccountByAccountTypeCOA(accType);
            var ChartOfAccountLU = chartofaAccount.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
            {
                Name = x.Name,
                Code = x.Code,
                Id = x.Id,
                RecOrder = x.RecOrder,
                IsAllowDisAllow = x.DisAllowable == true ? true : false,
                IsPLAccount = x.Category == "Income Statement" ? true : false,
                Class = x.Class,
                Status = x.Status
            }).OrderBy(d => d.Name).ToList();
            var allincidentals = _itemService.GetIncidentalSetups(companyId);
            if (allincidentals != null && allincidentals.Count > 0)
            {
                foreach (var incidental in allincidentals)
                {
                    IncidentalNewModel incidentalNewModel = new IncidentalNewModel();
                    incidentalNewModel.Id = incidental.Id;
                    incidentalNewModel.Code = incidental.Code;
                    incidentalNewModel.IncidentalType = incidental.IncidentalType;
                    incidentalNewModel.CompanyId = incidental.CompanyId;
                    incidentalNewModel.COAId = incidental.COAId;
                    incidentalNewModel.DefaultTaxcodeId = incidental.DefaultTaxcodeId;
                    incidentalNewModel.Description = incidental.Description;
                    incidentalNewModel.DocumentId = incidental.DocumentId;
                    incidentalNewModel.IsIncidental = incidental.IsIncidental;
                    incidentalNewModel.IsExternalData = incidental.IsExternalData;
                    incidentalNewModel.IsSaleItem = incidental.IsSaleItem;
                    incidentalNewModel.IsPLAccount = incidental.IsPLAccount;
                    incidentalNewModel.IsAllowableNotAllowableActivated = incidental.IsAllowableNotAllowableActivated;
                    incidentalNewModel.Status = incidental.Status;
                    incidentalNewModel.CreatedDate = incidental.CreatedDate;
                    incidentalNewModel.UserCreated = incidental.UserCreated;
                    incidentalNewModel.ModifiedBy = incidental.ModifiedBy;
                    incidentalNewModel.ModifiedDate = incidental.ModifiedDate;
                    incidentalNewModel.COAName = ChartOfAccountLU.Where(x => x.Id == incidental.COAId).Select(c => c.Name).FirstOrDefault();
                    lstincidentalNewModels.Add(incidentalNewModel);

                }
            }
            return lstincidentalNewModels.OrderByDescending(x => x.CreatedDate).ToList();

        }
        #endregion

        public IQueryable<CommunicationModel> GetCommunication(Guid clientId, string connectionString)
        {
            return _communicationservice.GetCommunications(clientId, connectionString);

        }

        #region InterCo

        #region InterCompanyClearingAndBilling
        public InterCoClearingVM InterCompnayClearingLu(long CompanyId, bool? isClearing, string connectionString)
        {
            try
            {
                InterCoClearingVM interCoClearingVM = new InterCoClearingVM();
                interCoClearingVM.Id = Guid.NewGuid();
                //interCoClearingVM.IsInterCompanyEnabled = true;
                interCoClearingVM.CompanyId = CompanyId;
                interCoClearingVM.InterCompanyType = isClearing == true ? "Clearing" : "Billing";
                string query = null;
                InterCompanySetting setting = _interCompanySettingService.GetInterCoCompanyIdAndType(CompanyId, interCoClearingVM.InterCompanyType);
                if (setting != null)
                {
                    interCoClearingVM.Id = setting.Id;
                    interCoClearingVM.CreatedDate = setting.CreatedDate;
                    interCoClearingVM.UserCreated = setting.UserCreated;
                    interCoClearingVM.ModifiedBy = setting.ModifiedBy;
                    interCoClearingVM.ModifiedDate = setting.ModifiedDate;
                    interCoClearingVM.IsInterCompanyEnabled = setting.IsInterCompanyEnabled;
                }
                else
                    interCoClearingVM.IsInterCompanyEnabled = false;

                if (isClearing == true)
                {
                    interCoClearingVM.ISIBActivated = _interCompanySettingService.GetIBIsActivatedOrNot(CompanyId, "Billing");
                }

                List<LstClearingServiceCompany> lstclearingServiceCompany = new List<LstClearingServiceCompany>();
                if (isClearing == true)
                {
                    query = $"Select distinct comp.Name, 'SERVICECOMPANY' as TABLENAME, comp.Id,comp.ShortName,comp.Name,interDetail.Id as DetailId,interDetail.InterCompanySettingId, CASE WHEN comp.Id = interDetail.ServiceEntityId and interDetail.status = 1 THEN 1 ELSE 0 END AS ISRight, comp.Status as Status,COA.Status as COAStatus from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Clearing' Right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId left join Bean.ChartOfAccount COA on COA.SubsidaryCompanyId = comp.Id and COA.Name like '%I/C -%'where comp.ParentId = {CompanyId} order by comp.ShortName";
                }
                else
                {
                    query = $"Select distinct comp.Name, 'SERVICECOMPANY' as TABLENAME, comp.Id,comp.ShortName,comp.Name,interDetail.Id as DetailId,interDetail.InterCompanySettingId, CASE WHEN comp.Id = interDetail.ServiceEntityId and interDetail.status = 1 THEN 1 ELSE 0 END AS ISRight, comp.Status as Status,COA.Status as COAStatus from Bean.InterCompanySetting inter Join Bean.InterCompanySettingDetail interDetail on inter.Id = interDetail.InterCompanySettingId  and inter.InterCompanyType = 'Billing' Right Join Common.Company comp on comp.Id = interDetail.ServiceEntityId left join Bean.ChartOfAccount COA on COA.SubsidaryCompanyId = comp.Id and COA.Name like '%I/B -%'where comp.ParentId = {CompanyId} order by comp.ShortName";
                }

                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    int recOrder = 0;
                    while (dr.Read())
                    {
                        LstClearingServiceCompany serviceCompany = new LstClearingServiceCompany();
                        serviceCompany.IsMoved = dr["ISRight"] != DBNull.Value ? Convert.ToBoolean(dr["ISRight"]) : (bool?)null;
                        serviceCompany.ServiceEntityName = dr["ShortName"] != DBNull.Value ? Convert.ToString(dr["ShortName"]) : null;
                        serviceCompany.SVCName = dr["Name"] != DBNull.Value ? Convert.ToString(dr["Name"]) : null;
                        serviceCompany.ServiceEntityId = dr["Id"] != DBNull.Value ? Convert.ToInt64(dr["Id"]) : (long?)null;
                        serviceCompany.Id = dr["DetailId"] != DBNull.Value ? Guid.Parse(dr["DetailId"].ToString()) : Guid.NewGuid();
                        serviceCompany.InterCompanySettingId = dr["InterCompanySettingId"] != DBNull.Value ? Guid.Parse(dr["InterCompanySettingId"].ToString()) : new Guid();
                        serviceCompany.RecOrder = ++recOrder;
                        serviceCompany.RecordStatus = null;
                        serviceCompany.Status = dr["Status"] != DBNull.Value ? (RecordStatusEnum)dr["Status"] : RecordStatusEnum.Active;
                        serviceCompany.COAStatus = dr["COAStatus"] != DBNull.Value ? (RecordStatusEnum)(dr["COAStatus"]) : RecordStatusEnum.Active;
                        lstclearingServiceCompany.Add(serviceCompany);
                    }
                    con.Close();
                };



                //    List<LstClearingServiceCompany> lstclearingServiceCompany = new List<LstClearingServiceCompany> {
                //new  LstClearingServiceCompany { Id=Guid.NewGuid(), InterCompanySettingId=Guid.NewGuid(), IsMoved=true, RecOrder=1, RecordStatus="Added", ServiceEntityId= 245,ServiceEntityName="PGPL"},
                //new  LstClearingServiceCompany { Id=Guid.NewGuid(), InterCompanySettingId=Guid.NewGuid(), IsMoved=false, RecOrder=2, RecordStatus="Modified", ServiceEntityId= 245,ServiceEntityName="PTPL"},
                //new  LstClearingServiceCompany { Id=Guid.NewGuid(), InterCompanySettingId=Guid.NewGuid(), IsMoved=true, RecOrder=3, RecordStatus="Modified", ServiceEntityId= 245,ServiceEntityName="ZTPL"},
                //new  LstClearingServiceCompany { Id=Guid.NewGuid(), InterCompanySettingId=Guid.NewGuid(), IsMoved=false, RecOrder=4, RecordStatus="Added", ServiceEntityId= 245,ServiceEntityName="CASG"},
                //new  LstClearingServiceCompany { Id=Guid.NewGuid(), InterCompanySettingId=Guid.NewGuid(), IsMoved=true, RecOrder=5, RecordStatus="Added", ServiceEntityId= 245,ServiceEntityName="ZTP"}



                List<LstClearingServiceCompany> listClearingCompany = new List<LstClearingServiceCompany>
                {
                     new  LstClearingServiceCompany()
                };
                interCoClearingVM.LstInterCoClearing = lstclearingServiceCompany.Where(s => s.Status == RecordStatusEnum.Active).ToList();
                interCoClearingVM.LstClearingDetail = listClearingCompany;
                return interCoClearingVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public InterCoClearingVM SaveInterCoClearing(InterCoClearingVM interCoClearingVM, string ConnectionString)
        {
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(interCoClearingVM));
                Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(MasterModuleLoggingValidation.MasterModuleApplicationService, "ObjectSave", AdditionalInfo);
                bool isFirst = false;
                InterCompanySetting interCompany = _interCompanySettingService.GetInterCompanyClearingById(interCoClearingVM.Id);
                if (interCompany == null)
                {
                    interCompany = new InterCompanySetting();
                    isFirst = true;
                    interCompany.CreatedDate = DateTime.UtcNow;
                }
                else
                    interCompany.CreatedDate = interCoClearingVM.CreatedDate;

                interCompany.Id = interCoClearingVM.Id;
                interCompany.CompanyId = interCoClearingVM.CompanyId;
                interCompany.InterCompanyType = interCoClearingVM.InterCompanyType;
                interCompany.IsInterCompanyEnabled = interCoClearingVM.IsInterCompanyEnabled;
                interCompany.UserCreated = interCoClearingVM.UserCreated;

                if (isFirst != true)
                {
                    interCompany.ModifiedBy = interCoClearingVM.ModifiedBy;
                    interCompany.ModifiedDate = DateTime.UtcNow;
                }

                interCompany.ObjectState = isFirst == true ? ObjectState.Added : ObjectState.Modified;
                if (isFirst)
                    _interCompanySettingService.Insert(interCompany);
                else
                    _interCompanySettingService.Update(interCompany);

                if (interCoClearingVM.LstClearingDetail.Where(a => (a.RecordStatus == "Added" || a.RecordStatus == "Modified")).Any())
                {
                    List<InterCompanySettingDetail> lstInterCompanySettingDetails = _interCompanySettingDetailService.GetListOfInterCompanySetttingDetail(interCoClearingVM.LstClearingDetail.Where(a => (a.RecordStatus == "Added" || a.RecordStatus == "Modified") && a.Id != Guid.Empty).Select(a => a.Id).ToList());

                    InterCompanySettingDetail interCompanySettingDetail = null;
                    foreach (var interCo in interCoClearingVM.LstClearingDetail.Where(a => (a.RecordStatus == "Added" || a.RecordStatus == "Modified")))
                    {
                        //InterCompanySettingDetail interCompanySettingDetail = new InterCompanySettingDetail();
                        if (interCo.RecordStatus == "Added")
                        {
                            if (lstInterCompanySettingDetails.Any())
                            {
                                //interCompanySettingDetail = interCompany.InterCompanySettingDetails.Where(a => a.ServiceEntityId == interCo.ServiceEntityId && a.Id == interCo.Id).FirstOrDefault();
                                interCompanySettingDetail = lstInterCompanySettingDetails.Where(a => a.ServiceEntityId == interCo.ServiceEntityId && a.Id == interCo.Id).FirstOrDefault();
                                if (interCompanySettingDetail != null)
                                {
                                    interCompanySettingDetail.Status = RecordStatusEnum.Active;
                                    interCompanySettingDetail.ObjectState = ObjectState.Modified;
                                    _interCompanySettingDetailService.Update(interCompanySettingDetail);
                                }
                                else
                                {
                                    interCompanySettingDetail = new InterCompanySettingDetail();
                                    interCompanySettingDetail.Id = Guid.NewGuid();
                                    interCompanySettingDetail.InterCompanySettingId = interCompany.Id;
                                    interCompanySettingDetail.ServiceEntityId = interCo.ServiceEntityId;
                                    interCompanySettingDetail.RecOrder = interCo.RecOrder;
                                    interCompanySettingDetail.Status = RecordStatusEnum.Active;
                                    interCompanySettingDetail.ObjectState = ObjectState.Added;
                                    _interCompanySettingDetailService.Insert(interCompanySettingDetail);
                                }
                            }
                            else
                            {
                                interCompanySettingDetail = new InterCompanySettingDetail();
                                interCompanySettingDetail.Id = Guid.NewGuid();
                                interCompanySettingDetail.InterCompanySettingId = interCompany.Id;
                                interCompanySettingDetail.ServiceEntityId = interCo.ServiceEntityId;
                                interCompanySettingDetail.RecOrder = interCo.RecOrder;
                                interCompanySettingDetail.Status = RecordStatusEnum.Active;
                                interCompanySettingDetail.ObjectState = ObjectState.Added;
                                _interCompanySettingDetailService.Insert(interCompanySettingDetail);
                            }
                        }
                        else
                        {
                            //interCompanySettingDetail = _interCompanySettingDetailService.GetInterCompanyDetails(interCo.ServiceEntityId, interCo.Id);
                            interCompanySettingDetail = lstInterCompanySettingDetails.Where(a => a.ServiceEntityId == interCo.ServiceEntityId && a.Id == interCo.Id).FirstOrDefault();
                            if (interCompanySettingDetail != null)
                            {
                                interCompanySettingDetail.Status = RecordStatusEnum.Inactive;
                                interCompanySettingDetail.ObjectState = ObjectState.Modified;
                                _interCompanySettingDetailService.Update(interCompanySettingDetail);
                            }
                        }
                    }
                }
                if (interCoClearingVM.LstClearingDetail.Any())
                {
                    SqlConnection con = null;
                    using (con = new SqlConnection(ConnectionString))
                    {
                        string newIds = string.Join(",", interCoClearingVM.LstClearingDetail.Where(s => s.RecordStatus == "Added").Select(x => x.ServiceEntityId));
                        string editIds = string.Join(",", interCoClearingVM.LstClearingDetail.Where(s => s.RecordStatus == "Modified").Select(x => x.ServiceEntityId));
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand iCcmd = new SqlCommand("[Bean_InterCO_I_C_I_B_COA]", con);
                        iCcmd.CommandType = CommandType.StoredProcedure;
                        iCcmd.Parameters.AddWithValue("@CompanyId", interCoClearingVM.CompanyId);
                        iCcmd.Parameters.AddWithValue("@NewIds", /*newIds == string.Empty ? null :*/ newIds.ToString());
                        iCcmd.Parameters.AddWithValue("@EditIds", editIds.ToString());
                        iCcmd.Parameters.AddWithValue("@IsIC", interCoClearingVM.InterCompanyType == "Clearing" ? true : false);
                        int res = iCcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                _unitOfWorkAsync.SaveChanges();
                if (interCoClearingVM.InterCompanyType == "Billing" && interCoClearingVM.LstClearingDetail.Any())
                    foreach (var detail in interCoClearingVM.LstClearingDetail.Where(a => a.RecordStatus == "Added"))
                    {
                        CreateDynamicFolder(interCoClearingVM.CompanyId, detail.SVCName, MasterModuleValidations.Entities);
                    }

                return interCoClearingVM;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion InterCompanyClearingAndBilling

        #region CoaMapping
        public COAMappingModel CreateCOAMapping(long companyId, string ConnectionString)
        {
            COAMappingModel cOAMappingModel = new COAMappingModel();
            SqlConnection con = null;
            SqlCommand cmd = null;
            SqlDataReader dr = null;
            try
            {
                COAMapping coaMapping = _cOAMappingServiceService.GetCOAMappingById(companyId);
                if (coaMapping != null)
                {
                    cOAMappingModel.Id = coaMapping.Id;
                    cOAMappingModel.CompanyId = coaMapping.CompanyId;
                    cOAMappingModel.UserCreated = coaMapping.UserCreated;
                    cOAMappingModel.CreatedDate = coaMapping.CreatedDate;
                    cOAMappingModel.ModifiedBy = coaMapping.ModifiedBy;
                    cOAMappingModel.ModifiedDate = coaMapping.ModifiedDate;

                    #region COA Mapping Delete
                    string custQuery = string.Empty;
                    string venQuery = string.Empty;
                    string custKeys = String.Join(",", coaMapping.COAMappingDetails.Select(c => c.CustCOAId).ToList());
                    string venKeys = String.Join(",", coaMapping.COAMappingDetails.Select(c => c.VenCOAId).ToList());
                    List<long?> lstVenCOAIds = new List<long?>();
                    if (custKeys != null)
                    {
                        custQuery = $"Select ID.COAId from Bean.Invoice as I JOIN Bean.InvoiceDetail ID on I.Id = ID.InvoiceId where COAId in (Select items from dbo.SplitToTable('{custKeys}', ',')) and I.CompanyId = {companyId} and I.Nature = 'Interco' and I.DocumentState<>'Void'";
                        using (con = new SqlConnection(ConnectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd = new SqlCommand(custQuery, con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    long? COAId = Convert.ToInt64(dr["COAId"]) > 0 ? Convert.ToInt64(dr["COAId"]) : 0;
                                    if (COAId > 0)
                                        lstVenCOAIds.Add(COAId);
                                }
                            }
                            con.Close();
                        }
                    }
                    if (venKeys != null)
                    {
                        venQuery = $"Select BD.COAId as COAId from Bean.Bill as B JOIN Bean.BillDetail as BD on B.Id = BD.BillId where BD.COAId in (Select items from dbo.SplitToTable('{venKeys}', ',')) and B.CompanyId = {companyId} and B.Nature = 'Interco' and B.DocumentState<>'Void'";
                        using (con = new SqlConnection(ConnectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd = new SqlCommand(venQuery, con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    long? COAId = Convert.ToInt64(dr["COAId"]) > 0 ? Convert.ToInt64(dr["COAId"]) : 0;
                                    if (COAId > 0)
                                        lstVenCOAIds.Add(COAId);
                                }
                            }
                            con.Close();
                        }
                    }
                    #endregion COA Mapping Delete

                    if (coaMapping.COAMappingDetails.Any())
                    {
                        List<COAMappingDetailModel> lstCOAMappingDetailModel = new List<COAMappingDetailModel>();
                        foreach (var coaMap in coaMapping.COAMappingDetails)
                        {
                            COAMappingDetailModel cOAMappingDetailModel = new COAMappingDetailModel();
                            cOAMappingDetailModel.Id = coaMap.Id;
                            cOAMappingDetailModel.COAMappingId = cOAMappingModel.Id;
                            cOAMappingDetailModel.CustCOAId = coaMap.CustCOAId;
                            cOAMappingDetailModel.VenCOAId = coaMap.VenCOAId;
                            cOAMappingDetailModel.Status = coaMap.Status;
                            cOAMappingDetailModel.RecOrder = coaMap.RecOrder;
                            //long cust = lstVenCOAIds.Where(c => c.Value == coaMap.CustCOAId).Select(c => c.Value).FirstOrDefault();
                            //long ven = lstVenCOAIds.Where(c => c.Value == coaMap.VenCOAId).Select(c => c.Value).FirstOrDefault();
                            cOAMappingDetailModel.IsDeleted = lstVenCOAIds.Where(c => c.Value == coaMap.CustCOAId).Select(c => c.Value) != null && lstVenCOAIds.Where(c => c.Value == coaMap.CustCOAId).Select(c => c.Value).FirstOrDefault() != 0 ? true : lstVenCOAIds.Where(c => c.Value == coaMap.VenCOAId).Select(c => c.Value) != null && lstVenCOAIds.Where(c => c.Value == coaMap.VenCOAId).Select(c => c.Value).FirstOrDefault() != 0;
                            lstCOAMappingDetailModel.Add(cOAMappingDetailModel);
                        }
                        cOAMappingModel.COAMappingDetailsModel = lstCOAMappingDetailModel.OrderBy(s => s.RecOrder).ToList();
                        //IQueryable<COAMappingDetailModel> lstDetails =( from coaMappings in _cOAMappingServiceService.Queryable()
                        //											   join coaMappingDetails in _cOAMappingDetailService.Queryable()
                        //											   on coaMapping.Id equals coaMappingDetails.COAMappingId
                        //											   where (coaMappings.CompanyId == companyId)
                        //											   select new COAMappingDetailModel
                        //											   {
                        //												   Id = coaMappingDetails.Id,
                        //												   COAMappingId = coaMappingDetails.COAMappingId,
                        //												   CustCOAId = coaMappingDetails.CustCOAId,
                        //												   VenCOAId = coaMappingDetails.VenCOAId,
                        //												   Status = coaMappingDetails.Status,
                        //												   RecOrder = coaMappingDetails.RecOrder
                        //											   }).ToList();
                        //cOAMappingModel.COAMappingDetailsModel = lstDetails.ToList();
                    }
                }
                else
                {

                    COAMappingDetailModel cOAMappingDetailModel = new COAMappingDetailModel();
                    List<COAMappingDetailModel> lstCOAMappingDetails = new List<COAMappingDetailModel>();
                    lstCOAMappingDetails.Add(cOAMappingDetailModel);
                    cOAMappingModel.COAMappingDetailsModel = lstCOAMappingDetails;
                }

                COAMappingDetailModel cOAMappingDetailModelnew = new COAMappingDetailModel();
                List<COAMappingDetailModel> lstCOAMappingDetailsnew = new List<COAMappingDetailModel>();
                lstCOAMappingDetailsnew.Add(cOAMappingDetailModelnew);
                cOAMappingModel.LstCOAMappingDetail = lstCOAMappingDetailsnew;
                return cOAMappingModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public COAMappingLU GetCoaMappingLu(long companyId)
        {
            try
            {
                COAMapping coaMapping = _cOAMappingServiceService.GetCOAMappingById(companyId);
                COAMappingLU cOAMappingLU = new COAMappingLU();
                List<COALookup<string>> lstEditCoa = new List<COALookup<string>>();
                List<COALookup<string>> lstEditCoavendor = null;

                //commented on 08/04/2020
                //List<string> coaName = new List<string> { COANameConstants.Direct_costs, COANameConstants.General_and_admin_expenses, COANameConstants.Interest_expense, COANameConstants.Rounding, COANameConstants.Interest_income, COANameConstants.Operating_expenses, COANameConstants.Other_expenses, COANameConstants.Other_income, COANameConstants.Sales_and_marketing_expenses, COANameConstants.Taxation, COANameConstants.Revenue, COANameConstants.Accruals,COANameConstants.Staff_cost, COANameConstants.Amortisation };

                List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };

                Dictionary<long, string> accType = _accountTypeService.GetAllAccounyTypeIdNames(companyId, coaName);
                List<ChartOfAccount> lstCOA = _chartOfAccountService.GetChartOfAccountByAccountTypeCOA(accType.Select(c => c.Key).ToList());
                //string coaName = COANameConstants.Revenue;
                //cOAMappingLU.CustomerChartOfAccountLU = lstCOA.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true && (accType.Where(c => c.Value == COANameConstants.Revenue || c.Value == COANameConstants.Other_income).Select(d => d.Key).Contains(z.AccountTypeId))).Select(x => new COALookup<string>()

                cOAMappingLU.CustomerChartOfAccountLU = lstCOA.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true && (accType.Where(c => c.Value != COANameConstants.Cashandbankbalances || c.Value != COANameConstants.AccountsReceivables || c.Value != COANameConstants.Other_receivables || c.Value != COANameConstants.AccountsPayable || c.Value != COANameConstants.OtherPayables || c.Value != COANameConstants.RetainedEarnings || c.Value != COANameConstants.Intercompany_clearing || c.Value != COANameConstants.Intercompany_billing || c.Value != COANameConstants.System).Select(d => d.Key).Contains(z.AccountTypeId))).Select(x => new COALookup<string>()
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
                }).OrderBy(d => d.Name).ToList();
                //List<string> coaNameven = new List<string> { COANameConstants.Direct_costs, COANameConstants.General_and_admin_expenses, COANameConstants.Interest_expense, COANameConstants.Rounding, COANameConstants.Interest_income, COANameConstants.Operating_expenses, COANameConstants.Other_expenses, COANameConstants.Other_income, COANameConstants.Sales_and_marketing_expenses, COANameConstants.Taxation };
                //List<AccountType> accTypeven = _accountTypeService.GetAllAccounyTypeByNames(companyId, coaNameven);
                cOAMappingLU.VendorChartOfAccountLU = lstCOA.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true /*&& (accType.Where(c => c.Value != COANameConstants.Revenue).Select(d => d.Key).Contains(z.AccountTypeId))*/).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status
                }).OrderBy(d => d.Name).ToList();

                if (coaMapping != null && coaMapping.COAMappingDetails.Count > 0)
                {

                    List<long> CoaIds = coaMapping.COAMappingDetails.Select(c => c.CustCOAId.Value).ToList();
                    if (cOAMappingLU.CustomerChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(cOAMappingLU.CustomerChartOfAccountLU.Select(x => x.Id)).ToList();
                    if (CoaIds.Any())
                    {
                        lstEditCoa = lstCOA.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
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
                        }).OrderBy(d => d.Name).ToList().ToList();
                        cOAMappingLU.CustomerChartOfAccountLU.AddRange(lstEditCoa);
                    }
                    lstEditCoavendor = lstCOA.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        RecOrder = x.RecOrder,
                        IsAllowDisAllow = x.DisAllowable == true ? true : false,
                        IsPLAccount = x.Category == "Income Statement" ? true : false,
                        Class = x.Class,
                        Status = x.Status
                    }).ToList().OrderBy(d => d.Name).ToList();
                    cOAMappingLU.VendorChartOfAccountLU.AddRange(lstEditCoa);
                }
                return cOAMappingLU;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public COAMappingModel SaveCOAMapping(COAMappingModel cOAMappingModel)
        {
            try
            {
                COAMapping coaMapping = _cOAMappingServiceService.GetCOAMappingById(cOAMappingModel.CompanyId);
                bool? isNew = false;
                if (coaMapping != null)
                {
                    InsertMappings(cOAMappingModel, coaMapping);
                    coaMapping.ObjectState = ObjectState.Modified;
                    _cOAMappingServiceService.Update(coaMapping);
                }
                else
                {
                    isNew = true;
                    coaMapping = new COAMapping();
                    InsertMappings(cOAMappingModel, coaMapping);
                    coaMapping.Id = Guid.NewGuid();
                    coaMapping.ObjectState = ObjectState.Added;
                    _cOAMappingServiceService.Insert(coaMapping);
                }

                if (cOAMappingModel.LstCOAMappingDetail.Any())
                {
                    int? recorder = 0;
                    recorder = isNew == false ? coaMapping.COAMappingDetails.Max(c => c.RecOrder) : 0;
                    COAMappingDetail cOAMappingDetail = null;
                    List<COAMappingDetail> lstCOAMappings = _cOAMappingDetailService.GetAllMappingSDetailById(cOAMappingModel.LstCOAMappingDetail.Select(c => c.Id).ToList());
                    foreach (var coamapping in cOAMappingModel.LstCOAMappingDetail)
                    {
                        if (coamapping.RecordStatus == "Added")
                        {
                            cOAMappingDetail = new COAMappingDetail();
                            cOAMappingDetail.Id = Guid.NewGuid();
                            cOAMappingDetail.COAMappingId = coaMapping.Id;
                            cOAMappingDetail.CustCOAId = coamapping.CustCOAId;
                            cOAMappingDetail.VenCOAId = coamapping.VenCOAId;
                            cOAMappingDetail.RecOrder = ++recorder;
                            cOAMappingDetail.Status = RecordStatusEnum.Active;
                            cOAMappingDetail.ObjectState = ObjectState.Added;
                            _cOAMappingDetailService.Insert(cOAMappingDetail);
                        }
                        else if (coamapping.RecordStatus == "Modified")
                        {
                            if (coaMapping.COAMappingDetails != null && coaMapping.COAMappingDetails.Any())
                            {
                                cOAMappingDetail = lstCOAMappings.Where(c => c.Id == coamapping.Id).FirstOrDefault();
                                if (cOAMappingDetail != null)
                                {
                                    cOAMappingDetail.CustCOAId = coamapping.CustCOAId;
                                    cOAMappingDetail.VenCOAId = coamapping.VenCOAId;
                                    cOAMappingDetail.Status = coamapping.Status;
                                    cOAMappingDetail.RecOrder = coamapping.RecOrder;
                                    cOAMappingDetail.ObjectState = ObjectState.Modified;
                                    _cOAMappingDetailService.Update(cOAMappingDetail);
                                }
                            }
                        }
                        else if (coamapping.RecordStatus == "Deleted")
                        {
                            cOAMappingDetail = lstCOAMappings.Where(c => c.Id == coamapping.Id).FirstOrDefault();
                            if (cOAMappingDetail != null)
                                cOAMappingDetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
                _unitOfWorkAsync.SaveChanges();
                return cOAMappingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion CoaMapping

        #region Private Methods
        private static void InsertMappings(COAMappingModel cOAMappingModel, COAMapping coaMapping)
        {
            coaMapping.CompanyId = cOAMappingModel.CompanyId;
            coaMapping.UserCreated = cOAMappingModel.UserCreated;
            coaMapping.CreatedDate = cOAMappingModel.CreatedDate;
            coaMapping.ModifiedBy = cOAMappingModel.ModifiedBy;
            coaMapping.ModifiedDate = cOAMappingModel.ModifiedDate;
        }
        #endregion Private Methods

        #region TaxcodeMapping
        public TaxCodeMappingLU GetAllTaxCodeMappingLu(long companyId)
        {
            TaxCodeMappingLU taxCodeMappingLU = new TaxCodeMappingLU();
            List<TaxCode> allTaxCodes = null;
            TaxCodeMapping taxCodeMapping = _taxCodeMappingService.GetTaxCodeById(companyId);
            allTaxCodes = _taxCodeService.GetTaxCodes(companyId);

            if (allTaxCodes.Any())
            {
                var TAX = allTaxCodes.Where(a => a.TaxType == "Output").GroupBy(a => a.Code).Select(a => new { code = a.Key, lstTax = a.FirstOrDefault() }).ToList();
                taxCodeMappingLU.CustTaxCodeLU = TAX.Where(c => c.lstTax.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.lstTax.Id,
                    Code = x.lstTax.Code,/* x.lstTax.Code + " - " + x.lstTax.Name,*/
                    Name = x.lstTax.Name,
                    TaxRate = x.lstTax.TaxRate,
                    TaxType = x.lstTax.TaxType,
                    Status = x.lstTax.Status,
                    //TaxIdCode = x.lstTax.Code != "NA" ? x.lstTax.Code + "-" + x.lstTax.TaxRate + (x.lstTax.TaxRate != null ? "%" : "NA") /*+ "(" + x.lstTax.TaxType[0] + ")"*/ : x.lstTax.Code
                }).OrderBy(c => c.Code).ToList();
                //var data = taxCodeMappingLU.CustTaxCodeLU;
                //taxCodeMappingLU.CustTaxCodeLU = data.Where(s => s.TaxType == "Output").OrderBy(c => c.Code).ToList();

                var venTax = allTaxCodes.Where(s => s.TaxType == "Input" && s.Code != "NA").GroupBy(a => a.Code).Select(a => new { code = a.Key, lstTax = a.FirstOrDefault() }).ToList();
                taxCodeMappingLU.VenTaxCodeLU = venTax.Where(c => c.lstTax.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.lstTax.Id,
                    Code = x.lstTax.Code,/* x.lstTax.Code + " - " + x.lstTax.Name,*/
                    Name = x.lstTax.Name,
                    TaxRate = x.lstTax.TaxRate,
                    TaxType = x.lstTax.TaxType,
                    Status = x.lstTax.Status,
                    //TaxIdCode = x.lstTax.Code != "NA" ? x.lstTax.Code + "-" + x.lstTax.TaxRate + (x.lstTax.TaxRate != null ? "%" : "NA") /*+ "(" + x.lstTax.TaxType[0] + ")"*/ : x.lstTax.Code
                }).OrderBy(c => c.Code).ToList();
                //var vendata = taxCodeMappingLU.VenTaxCodeLU;
                //taxCodeMappingLU.VenTaxCodeLU = vendata.Where(s => s.TaxType == "Input").OrderBy(c => c.Code).ToList();
            }
            if (taxCodeMapping != null && taxCodeMapping.TaxCodeMappingDetails.Count > 0)
            {
                List<long?> taxIds = taxCodeMapping.TaxCodeMappingDetails.Select(x => x.CustTaxId).ToList();
                taxIds = taxIds.Except(taxCodeMappingLU.CustTaxCodeLU.Select(x => x.Id)).ToList();
                if (taxIds.Any())
                {
                    taxCodeMappingLU.CustTaxCodeLU.AddRange(allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,//(x.Code + "-" + x.Name),
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        //TaxCode = x.Code,
                        //TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.lstTax.TaxType[0] + ")"*/ : x.Code,

                        RecOrder = x.RecOrder
                    }).AsEnumerable().OrderBy(c => c.Code).ToList());
                    taxCodeMappingLU.CustTaxCodeLU = taxCodeMappingLU.CustTaxCodeLU.OrderBy(c => c.Code).ToList();
                }
                List<long?> venTaxIds = taxCodeMapping.TaxCodeMappingDetails.Select(x => x.VenTaxId).ToList();
                venTaxIds = venTaxIds.Except(taxCodeMappingLU.VenTaxCodeLU.Select(x => x.Id)).ToList();
                if (venTaxIds.Any())
                {
                    taxCodeMappingLU.VenTaxCodeLU.AddRange(allTaxCodes.Where(c => venTaxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.Code,//(x.Code + "-" + x.Name),
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        //TaxCode = x.Code,
                        //TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.lstTax.TaxType[0] + ")"*/ : x.Code,

                        RecOrder = x.RecOrder
                    }).AsEnumerable().OrderBy(c => c.Code).ToList());
                    taxCodeMappingLU.VenTaxCodeLU = taxCodeMappingLU.VenTaxCodeLU.OrderBy(c => c.Code).ToList();
                }
            }
            return taxCodeMappingLU;
        }

        public TaxCodeMappingModel CreateTaxCodeMapping(long companyId)
        {
            TaxCodeMappingModel taxCodeMappingModel = new TaxCodeMappingModel();
            try
            {
                TaxCodeMapping taxCodeMapping = _taxCodeMappingService.GetTaxCodeById(companyId);
                if (taxCodeMapping != null)
                {
                    taxCodeMappingModel.Id = taxCodeMapping.Id;
                    taxCodeMappingModel.CompanyId = taxCodeMapping.CompanyId;
                    taxCodeMappingModel.UserCreated = taxCodeMapping.UserCreated;
                    taxCodeMappingModel.CreatedDate = taxCodeMapping.CreatedDate;
                    taxCodeMappingModel.ModifiedBy = taxCodeMapping.ModifiedBy;
                    taxCodeMappingModel.ModifiedDate = taxCodeMapping.ModifiedDate;
                    if (taxCodeMapping.TaxCodeMappingDetails.Any())
                    {
                        taxCodeMappingModel.TaxCodeMappingDetailsModel = taxCodeMapping.TaxCodeMappingDetails.Select(x => new TaxCodeMappingDetailModel()
                        {
                            Id = x.Id,
                            TaxCodeMappingId = x.TaxCodeMappingId,
                            CustTaxId = x.CustTaxId,
                            VenTaxId = x.VenTaxId,
                            Status = x.Status,
                            CustTaxCode = x.CustTaxCode,
                            VenTaxCode = x.VenTaxCode,
                            RecOrder = x.RecOrder,
                        }).OrderBy(x => x.RecOrder).ToList();

                        //modified on 14-11-2019
                        //List<TaxCodeMappingDetailModel> lstTaxCodeMappingDetailModel = new List<TaxCodeMappingDetailModel>();
                        //foreach (var coaMap in taxCodeMapping.TaxCodeMappingDetails)
                        //{
                        //    TaxCodeMappingDetailModel taxCodeMappingDetailModel = new TaxCodeMappingDetailModel();
                        //    taxCodeMappingDetailModel.Id = coaMap.Id;
                        //    taxCodeMappingDetailModel.TaxCodeMappingId = coaMap.TaxCodeMappingId;
                        //    taxCodeMappingDetailModel.CustTaxId = coaMap.CustTaxId;
                        //    taxCodeMappingDetailModel.VenTaxId = coaMap.VenTaxId;
                        //    taxCodeMappingDetailModel.Status = coaMap.Status;
                        //    taxCodeMappingDetailModel.CustTaxCode = coaMap.CustTaxCode;
                        //    taxCodeMappingDetailModel.VenTaxCode = coaMap.VenTaxCode;
                        //    taxCodeMappingDetailModel.RecOrder = coaMap.RecOrder;
                        //    lstTaxCodeMappingDetailModel.Add(taxCodeMappingDetailModel);
                        //}
                        //taxCodeMappingModel.TaxCodeMappingDetailsModel = lstTaxCodeMappingDetailModel.OrderBy(s => s.RecOrder).ToList();
                        //modified on 14-11-2019


                        //IQueryable<TaxCodeMappingDetailModel> lstDetails = from taxcodeMapping in taxCodeMapping.TaxCodeMappingDetails
                        //                                                                     join taxCodeMappigDetails in _taxCodeMappingDetailService.Queryable()
                        //												   on taxcodeMapping.Id equals taxCodeMappigDetails.TaxCodeMappingId
                        //												   where (taxcodeMapping.CompanyId == companyId)
                        //												   select new TaxCodeMappingDetailModel
                        //												   {
                        //													   Id = taxCodeMappigDetails.Id,
                        //													   TaxCodeMappingId = taxCodeMappigDetails.TaxCodeMappingId,
                        //													   CustTaxId = taxCodeMappigDetails.CustTaxId,
                        //													   VenTaxId = taxCodeMappigDetails.VenTaxId,
                        //													   Status = taxCodeMappigDetails.Status,
                        //													   RecOrder = taxCodeMappigDetails.RecOrder
                        //												   };
                        //taxCodeMappingModel.LstTaxCodeMappingDetail = lstDetails.ToList();
                    }
                }
                else
                {
                    //TaxCodeMappingDetailModel taxCodeMappingDetail = new TaxCodeMappingDetailModel();
                    //List<TaxCodeMappingDetailModel> lstTaxCodeMappingDetails = new List<TaxCodeMappingDetailModel>();
                    //lstTaxCodeMappingDetails.Add(taxCodeMappingDetail);
                    //taxCodeMappingModel.LstTaxCodeMappingDetail = lstTaxCodeMappingDetails;


                }

                //TaxCodeMappingDetailModel taxCodeMappingDetailModelnew = new TaxCodeMappingDetailModel();
                //List<TaxCodeMappingDetailModel> lstTaxCodeMappingDetailsnew = new List<TaxCodeMappingDetailModel>();
                //lstTaxCodeMappingDetailsnew.Add(taxCodeMappingDetailModelnew);
                //taxCodeMappingModel.LstTaxCodeMappingDetail = lstTaxCodeMappingDetailsnew;

                taxCodeMappingModel.LstTaxCodeMappingDetail = new List<TaxCodeMappingDetailModel>();
                return taxCodeMappingModel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public TaxCodeMappingModel SaveTaxCodeMapping(TaxCodeMappingModel taxCodeMappingModel)
        {
            try
            {
                bool? isNew = false;
                TaxCodeMapping taxCodeMapping = _taxCodeMappingService.GetTaxCodeById(taxCodeMappingModel.CompanyId);
                if (taxCodeMapping != null)
                {
                    taxCodeMapping.CompanyId = taxCodeMappingModel.CompanyId;
                    taxCodeMapping.UserCreated = taxCodeMappingModel.UserCreated;
                    taxCodeMapping.CreatedDate = taxCodeMappingModel.CreatedDate;
                    taxCodeMapping.ModifiedBy = taxCodeMappingModel.ModifiedBy;
                    taxCodeMapping.ModifiedDate = taxCodeMappingModel.ModifiedDate;
                    taxCodeMapping.ObjectState = ObjectState.Modified;
                    _taxCodeMappingService.Update(taxCodeMapping);
                }
                else
                {
                    isNew = true;
                    taxCodeMapping = new TaxCodeMapping();
                    taxCodeMapping.Id = Guid.NewGuid();
                    taxCodeMapping.CompanyId = taxCodeMappingModel.CompanyId;
                    taxCodeMapping.UserCreated = taxCodeMappingModel.UserCreated;
                    taxCodeMapping.CreatedDate = taxCodeMappingModel.CreatedDate;
                    taxCodeMapping.ModifiedBy = taxCodeMappingModel.ModifiedBy;
                    taxCodeMapping.ModifiedDate = taxCodeMappingModel.ModifiedDate;
                    taxCodeMapping.ObjectState = ObjectState.Added;
                    _taxCodeMappingService.Insert(taxCodeMapping);
                }

                if (taxCodeMappingModel.LstTaxCodeMappingDetail.Any())
                {
                    int? recorder = isNew == false ? taxCodeMapping.TaxCodeMappingDetails.Max(c => c.RecOrder) : 0;
                    TaxCodeMappingDetail taxCodeMappingDetail = null;
                    List<TaxCodeMappingDetail> lstTaxCodes = _taxCodeMappingDetailService.GetAllTaxCodeDetailById(taxCodeMappingModel.LstTaxCodeMappingDetail.Select(c => c.Id).ToList());
                    foreach (var taxCodeMappings in taxCodeMappingModel.LstTaxCodeMappingDetail)
                    {
                        if (taxCodeMappings.RecordStatus == "Added")
                        {
                            taxCodeMappingDetail = new TaxCodeMappingDetail();
                            taxCodeMappingDetail.Id = Guid.NewGuid();
                            taxCodeMappingDetail.TaxCodeMappingId = taxCodeMapping.Id;
                            taxCodeMappingDetail.CustTaxId = taxCodeMappings.CustTaxId;
                            taxCodeMappingDetail.VenTaxId = taxCodeMappings.VenTaxId;
                            taxCodeMappingDetail.RecOrder = ++recorder;
                            taxCodeMappingDetail.Status = RecordStatusEnum.Active;
                            taxCodeMappingDetail.CustTaxCode = taxCodeMappings.CustTaxCode;
                            taxCodeMappingDetail.VenTaxCode = taxCodeMappings.VenTaxCode;
                            taxCodeMappingDetail.ObjectState = ObjectState.Added;
                            _taxCodeMappingDetailService.Insert(taxCodeMappingDetail);
                        }
                        else if (taxCodeMappings.RecordStatus == "Modified")
                        {
                            if (taxCodeMapping.TaxCodeMappingDetails != null && taxCodeMapping.TaxCodeMappingDetails.Any())
                            {
                                taxCodeMappingDetail = lstTaxCodes.Where(x => x.Id == taxCodeMappings.Id).FirstOrDefault();
                                if (taxCodeMappingDetail != null)
                                {
                                    taxCodeMappingDetail.CustTaxId = taxCodeMappings.CustTaxId;
                                    taxCodeMappingDetail.VenTaxId = taxCodeMappings.VenTaxId;
                                    taxCodeMappingDetail.CustTaxCode = taxCodeMappings.CustTaxCode;
                                    taxCodeMappingDetail.VenTaxCode = taxCodeMappings.VenTaxCode;
                                    taxCodeMappingDetail.Status = RecordStatusEnum.Active;
                                    taxCodeMappingDetail.RecOrder = taxCodeMappings.RecOrder;
                                    taxCodeMappingDetail.ObjectState = ObjectState.Modified;
                                    _taxCodeMappingDetailService.Update(taxCodeMappingDetail);
                                }
                            }
                        }
                        else if (taxCodeMappings.RecordStatus == "Deleted")
                        {
                            taxCodeMappingDetail = lstTaxCodes.Where(c => c.Id == taxCodeMappings.Id).FirstOrDefault();
                            if (taxCodeMappingDetail != null)
                                taxCodeMappingDetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
                _unitOfWorkAsync.SaveChanges();
                return taxCodeMappingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion TaxcodeMapping

        #endregion InterCo




        #region LinkedAccounts

        public IQueryable<LinkedAccountsModel> GetLinkedAccountsK(long? companyId, string connectionString, string username)
        {
            return _beanEntityService.GetLinkedAccountsK(companyId, connectionString, username);
        }

        #endregion

        #region common_folder_Name change
        public void ChangeFolderName(long companyId, string recordName, string oldname)
        {
            if (recordName.Equals(oldname + "."))
                return;
            try
            {
                FolderModel folderModel = new FolderModel();
                folderModel.FileShareName = Convert.ToInt32(companyId);
                folderModel.NewName = _commonAppService.StringCharactersReplaceFunction(recordName);
                folderModel.CursorName = DocumentConstants.CursorName;
                folderModel.OldName = _commonAppService.StringCharactersReplaceFunction(oldname);
                folderModel.Path = DocumentConstants.Entities + "/" + folderModel.OldName;
                var json = RestHelper.ConvertObjectToJason(folderModel);
                try
                {
                    string url = ConfigurationManager.AppSettings["AzureFuncUrl"];
                    var response = RestHelper.ZPost(url, "api/RenameFolder", json);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var data = JsonConvert.DeserializeObject<string>(response.Content);
                    }
                    else
                    {
                        throw new Exception(response.Content);
                    }

                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    //return false;
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            // return true;
        }
        #endregion











        public string GetPeppolActivation(long companyId, string connectionString)
        {
            string PeppolEnable = null;
            using (con = new SqlConnection(connectionString))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                string query = "Select case when isnull(cf.IsChecked,0) = 1 then 'true' else 'false' end as PeppolEnable from Common.Feature as f join Common.CompanyFeatures  as cf on f.Id=cf.FeatureId where f.Name='Peppol' and f.Status=1 and cf.CompanyId=" + companyId;
                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                //int recOrder = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PeppolEnable = dr["PeppolEnable"].ToString();
                    }
                }
                con.Close();
            };
            return PeppolEnable;
        }
    }
}

