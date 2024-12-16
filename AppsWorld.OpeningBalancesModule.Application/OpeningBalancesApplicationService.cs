using AppsWorld.OpeningBalancesModule.Models;
using AppsWorld.OpeningBalancesModule.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Repository.Pattern.Infrastructure;
using AppsWorld.Framework;
using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Entities;
using AppsWorld.OpeningBalancesModule.Infra.Resources;
using Serilog;
using Logger;
using FrameWork;
using System.Data.Entity.Validation;
using Domain.Events;
using AppsWorld.OpeningBalancesModule.RepositoryPattern;
using AppsWorld.OpeningBalancesModule.Infra;
using AppsWorld.CommonModule.Models;
using System.Configuration;
using AppsWorld.CommonModule.Infra;
using System.Data.SqlClient;
using System.Data;
using Ziraff.Section;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using Hangfire;
using System.Net;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
 

namespace AppsWorld.OpeningBalancesModule.Application
{
    public class OpeningBalancesApplicationService
    {
        private readonly IOpeningBalanceService _OpeningBalanceService;
        private readonly IOpeningBalanceDetailService _OpeningBalanceDetailService;
        private readonly ICurrencyService _currencyService;
        private readonly ICompanyService _companyService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IMultiCurrencySettingService _multiCurrencySettingService;
        //private readonly ISegmentMasterService _segmentMasterService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IOpeningBalanceDetailLineItemService _openingBalanceDetailLineItemService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly IOpeningBalancesModuleUnitOfWorkAsync _openingBalancesModuleUnitOfWork;
        private readonly IAccountTypeService _accountTypeService;
        private readonly AppsWorld.OpeningBalancesModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly AppsWorld.OpeningBalancesModule.Service.IAutoNumberService _autoNumberService;
        private readonly IJournalService _journalService;
        private readonly AppsWorld.CommonModule.Service.IItemService _iItemService;
        private readonly AppsWorld.CommonModule.Service.ITaxCodeService _taxcodeService;
        private readonly AppsWorld.CommonModule.Service.IGSTSettingService _gstSettingService;
        private readonly IBillService _billService;
        private readonly AppaWorld.Bean.Common _beanCommon;

        public OpeningBalancesApplicationService(IOpeningBalanceService OpeningBalanceService, IOpeningBalanceDetailService OpeningBalanceDetailService, ICurrencyService currencyService, ICompanyService companyService, IChartOfAccountService chartOfAccountService, IMultiCurrencySettingService multiCurrencySettingService, IFinancialSettingService financialSettingService, IOpeningBalanceDetailLineItemService openingBalanceDetailLineItemService, IBeanEntityService beanEntityService, IOpeningBalancesModuleUnitOfWorkAsync openingBalancesModuleUnitOfWork, IAccountTypeService accountTypeService, AppsWorld.OpeningBalancesModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.OpeningBalancesModule.Service.IAutoNumberService autoNumberService, IJournalService journalService, AppsWorld.CommonModule.Service.IItemService iItemService, AppsWorld.CommonModule.Service.ITaxCodeService taxcodeService, IBillService billService, AppsWorld.CommonModule.Service.IGSTSettingService gstSettingService, AppaWorld.Bean.Common beanCommon)
        {
            this._OpeningBalanceService = OpeningBalanceService;
            this._OpeningBalanceDetailService = OpeningBalanceDetailService;
            this._currencyService = currencyService;
            this._companyService = companyService;
            this._chartOfAccountService = chartOfAccountService;
            this._multiCurrencySettingService = multiCurrencySettingService;
            //this._segmentMasterService = segmentMasterService;
            this._financialSettingService = financialSettingService;
            this._openingBalanceDetailLineItemService = openingBalanceDetailLineItemService;
            this._beanEntityService = beanEntityService;
            this._openingBalancesModuleUnitOfWork = openingBalancesModuleUnitOfWork;
            this._accountTypeService = accountTypeService;
            this._autoNumberService = autoNumberService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._journalService = journalService;
            this._iItemService = iItemService;
            this._taxcodeService = taxcodeService;
            this._billService = billService;
            this._gstSettingService = gstSettingService;
            this._beanCommon = beanCommon;
        }

        #region Kendo GetAllOpeningBalancessK

        public IQueryable<OpeningBalanceModelK> GetAllOpeningBalancessK(long companyId, string userName)
        {
            return _OpeningBalanceService.GetAllOpeningBalancessK(companyId, userName);
        }

        #endregion

        #region Lookup GetOpeningBalance

        public OpeningBalanceLU GetOpeningBalance(long companyId, long ServiceCompanyId, string userName)
        {

            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_GetOpeningBalance_Method);
            OpeningBalance openingBalance = _OpeningBalanceService.GetOpeningBalance(companyId, ServiceCompanyId);
            List<OpeningBalance> lstopeningBalance = _OpeningBalanceService.lstopeningbalance(companyId);
            OpeningBalanceLU openingBalanceLU = new OpeningBalanceLU();
            openingBalanceLU.CompanyId = companyId;
            //MultiCurrencySetting multi = _multiCurrencySettingService.GetByCompanyId(companyId);
            //openingBalanceLU.IsMultiCurrency = multi != null;
            openingBalanceLU.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_GetByCurrencies_Method);
            //long comp = openingBalance == null ? 0 : openingBalance.CompanyId;
            long? comp = openingBalance == null ? 0 : openingBalance.ServiceCompanyId == null ? 0 : openingBalance.ServiceCompanyId;
            List<Company> lstCompanies = _companyService.GetCompany(companyId, comp, userName);

            if (openingBalance != null)
            {
                openingBalanceLU.SubsideryCompanyLU = lstCompanies.Where(c => c.Id == openingBalance.ServiceCompanyId).Select(x => new LookUpCompany<string>()
                {
                    Id = x.Id,
                    Name = x.Name,
                    ShortName = x.ShortName,
                    isGstActivated = x.IsGstSetting
                }).OrderBy(x => x.ShortName).ToList();
            }
            else
            {
                if (lstopeningBalance.Any())
                {
                    //List<LookUpCompany<string>> SubsideryCompanyLU = lstCompanies.Select(x => new LookUpCompany<string>()
                    //{
                    //    Id = x.Id,
                    //    Name = x.Name,
                    //    ShortName = x.ShortName
                    //}).ToList();
                    openingBalanceLU.SubsideryCompanyLU = lstCompanies.Where(x => !lstopeningBalance.Any(ob => x.Id == ob.ServiceCompanyId && ob.IsTemporary == false)).Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.IsGstSetting
                    }).OrderBy(x => x.ShortName).ToList();

                    //var SubsideryCompanyLU1 = (from sc in SubsideryCompanyLU
                    //                           where !lstopeningBalance.Any(ob => sc.Id == ob.ServiceCompanyId)
                    //                           select sc).ToList();
                    //openingBalanceLU.SubsideryCompanyLU = SubsideryCompanyLU1;
                }
                else
                {
                    openingBalanceLU.SubsideryCompanyLU = lstCompanies.Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.IsGstSetting
                    }).ToList();
                }
            }
            var financials = _financialSettingService.GetFinancialSetting(companyId);
            //   LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService,OpeningBalanceLoggingValidation.Checking_financials_Is_Not_Equal_To_Null);
            if (financials == null)
                throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
            if (financials != null)
                openingBalanceLU.BaseCurrency = financials.BaseCurrency;

            openingBalanceLU.IsLocked = openingBalance?.IsLocked;

            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_GetOpeningBalance_Method);
            //if (openingBalance != null)
            //{
            //    if (openingBalance.SegmentMasterid1 != null)
            //        invoiceLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyid, invoice.SegmentMasterid1);
            //    if (invoice.SegmentMasterid2 != null)
            //        invoiceLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyid, invoice.SegmentMasterid2);
            //}
            //List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
            //if (segments.Count > 0)
            //    openingBalanceLU.SegmentCategory1LU = segments[0];
            //if (segments.Count > 1)
            //    openingBalanceLU.SegmentCategory2LU = segments[1];
            return openingBalanceLU;
        }

        #endregion

        #region GetCall

        #region GetServiceCompanyOpeningBalance

        public OpeningBalanceModel GetServiceCompanyOpeningBalance(long companyId, long ServiceCompanyId, bool? isInterCompanyActive, string username)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_Get_Service_Company_OpeningBalance_Method);
            OpeningBalanceModel openingbalanceModel = new OpeningBalanceModel();
            bool? multi = _multiCurrencySettingService.GetByCompanyId(companyId);
            openingbalanceModel.IsMultiCurrencyActive = multi;
            //var segmentmaster = _segmentMasterService.GetSegmentMastersById(companyId);
            //openingbalanceModel.IsSegmentActive = segmentmaster != null;
            openingbalanceModel.CompanyId = companyId;
            openingbalanceModel.ServiceCompanyId = ServiceCompanyId;
            openingbalanceModel.DocState = "Posted";
            FinancialSetting financials = _financialSettingService.GetFinancialSetting(companyId);
            if (financials == null)
                throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
            openingbalanceModel.FinancialPeriodLockStartDate = financials.PeriodLockDate;
            openingbalanceModel.FinancialPeriodLockEndDate = financials.PeriodEndDate;
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_financials_Is_Not_Equal_To_Null);
            //if (financials != null)
            openingbalanceModel.BaseCurrency = financials.BaseCurrency;
            //openingbalanceModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
            //openingbalanceModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
            //openingbalanceModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
            //openingbalanceModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);

            OpeningBalance openingBalance = _OpeningBalanceService.GetServiceCompanyOpeningBalance(companyId, ServiceCompanyId);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.OpeningBalance_Is_Not_Equal_To_Null);
            List<long> lstServiceCompanyIds = _companyService.GetAllSubCompaniesId(username, companyId);
            if (openingBalance != null)
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_FillOpeningBalance_Method);
                FillOpeningBalance(openingbalanceModel, openingBalance);
                openingbalanceModel.IsTemporary = openingBalance.IsTemporary;
                openingbalanceModel.ServiceCompanyId = openingBalance.ServiceCompanyId;
                openingbalanceModel.IsLocked = openingBalance.IsLocked;
                //Journal journal = _journalService.GetJournals(openingBalance.Id, companyId);
                //if (journal != null)
                //    openingbalanceModel.JournalId = journal.Id;
                //var servicename = _companyService.GetCompanyByCompanyid(ServiceCompanyId);
                //if (servicename != null)
                //{
                //    openingbalanceModel.ServiceCompanyName = servicename.ShortName;
                //}
                openingbalanceModel.SaveType = openingBalance.SaveType;
                List<OpeningBalanceDetailModel> lstOpeningBalanceDetailModel = new List<OpeningBalanceDetailModel>();
                List<OpeningBalanceDetailModel> lstOBDetailForCoa = new List<OpeningBalanceDetailModel>();

                List<OpeningBalanceDetail> lstdetails = _OpeningBalanceDetailService.GetServiceCompanyOpeningBalance(openingBalance.Id).OrderByDescending(a => a.CreatedDate).OrderBy(a => a.Recorder).ToList();
                //var lstCoaid = lstdetails.Select(c => c.COAId).ToList();
                List<ChartOfAccount> newAddedCOAList = _chartOfAccountService.GetAllCOAByBank(companyId, lstdetails.Select(c => c.COAId).ToList(), lstServiceCompanyIds);
                List<ChartOfAccount> newCOA1 = newAddedCOAList.Where(a => (a.IsBank == false || a.IsBank == null) && (a.SubsidaryCompanyId != ServiceCompanyId || a.SubsidaryCompanyId == null)).ToList();
                List<ChartOfAccount> newCOA2 = newAddedCOAList.Where(a => a.IsBank == true && a.SubsidaryCompanyId == ServiceCompanyId).ToList();
                if (newCOA2.Any())
                    newCOA1.AddRange(newCOA2);
                if (lstdetails.Any())
                {
                    AccountType acc = _accountTypeService.GetById(companyId, COANameConstants.Cashandbankbalances);
                    var data = lstdetails.SelectMany(c => c.OpeningBalanceDetailLineItems).Select(c => c.COAId).ToList();
                    List<ChartOfAccount> lstCOAs = new List<ChartOfAccount>();
                    List<ChartOfAccount> lstCOAs2 = new List<ChartOfAccount>();
                    List<long> COAIds = lstdetails.Any() ? lstdetails.SelectMany(c => c.OpeningBalanceDetailLineItems).Select(c => c.COAId).ToList() : new List<long>();
                    if (COAIds.Any())
                    {
                        lstCOAs = _chartOfAccountService.GetAllCOAByIds(COAIds);
                    }
                    List<long> COAIds2 = lstdetails.Select(c => c.COAId).ToList();
                    if (COAIds2.Any())
                    {
                        lstCOAs2 = _chartOfAccountService.GetAllCOAByIds(COAIds2);
                    }
                    foreach (OpeningBalanceDetail detail in lstdetails)
                    {
                        OpeningBalanceDetailModel oBDetail = new OpeningBalanceDetailModel();
                        ChartOfAccount coa = lstCOAs2.Where(C => C.Id == detail.COAId).FirstOrDefault();
                        oBDetail.IsEditEnable = coa.SubsidaryCompanyId != null ? lstServiceCompanyIds.Contains((Int64)(coa.SubsidaryCompanyId)) ? true : false : true;
                        FillOpeningBalanceDetail(oBDetail, detail, coa);
                        oBDetail.BaseCurrency = financials.BaseCurrency;
                        if (acc != null)
                        {
                            if (coa != null)
                            {
                                oBDetail.IsCashandbank = coa.AccountTypeId == acc.Id ? true : false;
                                oBDetail.DocCurrency = coa.Currency;
                                if (oBDetail.IsCashandbank == true && oBDetail.DocCurrency == null)
                                {
                                    if (financials != null)
                                        oBDetail.DocCurrency = financials.BaseCurrency;
                                }
                                if (oBDetail.IsCashandbank == false)
                                {
                                    oBDetail.DocCurrency = detail.DocCurrency;
                                }
                            }
                            if (detail.OpeningBalanceDetailLineItems.Count > 0)
                            {
                                //List<OpeningBalanceLineItemModel> lstItems = new List<OpeningBalanceLineItemModel>();
                                //foreach (var item in detail.OpeningBalanceDetailLineItems)
                                //{
                                //    OpeningBalanceLineItemModel lineItem = new OpeningBalanceLineItemModel();
                                //    FillLineItem(lineItem, item, lstCOAs);
                                //    lstItems.Add(lineItem);
                                //}
                                //oBDetail.LineItems = lstItems;
                                oBDetail.LineItems = detail.OpeningBalanceDetailLineItems.Select(openingBalancelineitem => new OpeningBalanceLineItemModel
                                {
                                    Date = openingBalancelineitem.Date,
                                    BaseCurrency = openingBalancelineitem.BaseCurrency,
                                    Id = openingBalancelineitem.Id,
                                    OpeningBalanceDetailId = openingBalancelineitem.OpeningBalanceDetailId,
                                    COAId = openingBalancelineitem.COAId,
                                    AccountName = lstCOAs.Any() ? lstCOAs.Where(c => c.Id == openingBalancelineitem.COAId).Select(c => c.Name).FirstOrDefault() : string.Empty,
                                    AccountCode = lstCOAs.Any() ? lstCOAs.Where(c => c.Id == openingBalancelineitem.COAId).Select(c => c.Code).FirstOrDefault() : string.Empty,
                                    BaseCredit = openingBalancelineitem.BaseCredit,
                                    BaseDebit = openingBalancelineitem.BaseDebit,
                                    DocCurrency = openingBalancelineitem.DocumentCurrency,
                                    DocCredit = openingBalancelineitem.DocCredit,
                                    DocDebit = openingBalancelineitem.DoCDebit,
                                    Description = openingBalancelineitem.Description,
                                    ExchangeRate = openingBalancelineitem.ExchangeRate,
                                    EntityId = openingBalancelineitem.EntityId,
                                    ServiceCompanyId = openingBalancelineitem.ServiceCompanyId,
                                    DocumentReference = openingBalancelineitem.DocumentReference,
                                    IsDisAllow = openingBalancelineitem.IsDisAllow,
                                    UserCreated = openingBalancelineitem.UserCreated,
                                    CreatedDate = openingBalancelineitem.CreatedDate,
                                    ModifiedBy = openingBalancelineitem.ModifiedBy,
                                    ModifiedDate = openingBalancelineitem.ModifiedDate,
                                    DueDate = openingBalancelineitem.DueDate,
                                    IsEditable = openingBalancelineitem.IsEditable,
                                    RecOrder = openingBalancelineitem.Recorder,
                                    IsProcressed = openingBalancelineitem.IsProcressed,
                                    ProcressedRemarks = openingBalancelineitem.ProcressedRemarks,
                                }).OrderBy(x => x.RecOrder).ToList();
                            }
                        }
                        //if (openingbalanceModel.IsMultiCurrencyActive == false)
                        //{
                        //    oBDetail.DocCurrency = detail.BaseCurrency;
                        //}
                        if (multi == false)
                            oBDetail.DocCurrency = financials.BaseCurrency;
                        if (coa != null)
                        {
                            oBDetail.AccountName = coa.Name;
                            oBDetail.PayReceivableAccName = oBDetail.AccountName.ToLower() == OpeningBalnceValidations.accounts_receivables ? "TR"
                                                  : oBDetail.AccountName.ToLower() == OpeningBalnceValidations.other_receivables ? "OR"
                                                  : oBDetail.AccountName.ToLower() == OpeningBalnceValidations.accounts_payable ? "TP"
                                                  : oBDetail.AccountName.ToLower() == OpeningBalnceValidations.other_payables ? "OP" : null;
                        }
                        oBDetail.IsPLAccount = coa.Category != null ? coa.Category.ToLower() == OpeningBalnceValidations.income_statement ? true : false : false;
                        oBDetail.IsAllowDisAllow = coa.DisAllowable ?? false;
                        lstOpeningBalanceDetailModel.Add(oBDetail);
                    }
                    if (newCOA1.Any() && newCOA1.Count > 0)
                    {
                        //foreach (var newCoa in newAddedCOAList)
                        //{
                        //    OpeningBalanceDetailModel obDetail = new OpeningBalanceDetailModel();
                        //    obDetail.COAId = newCoa.Id;
                        //    obDetail.Id = Guid.NewGuid();
                        //    obDetail.OpeningBalanceId = openingBalance.Id;
                        //    obDetail.AccountName = newCoa.Name;
                        //    obDetail.AccountCode = newCoa.Code;
                        //    obDetail.BaseCurrency = financials.BaseCurrency;
                        //    obDetail.DocCurrency = newCoa.Currency;
                        //    obDetail.RecordStatus = RecordStatus.Added;
                        //    obDetail.IsCashandbank = newCoa.AccountTypeId == acc.Id;
                        //    obDetail.Nature = newCoa.Nature;
                        //    // obDetail.IsPLAccount = oBDetail.IsPLAccount;
                        //    lstOBDetailForCoa.Add(obDetail);
                        //}
                        lstOBDetailForCoa = newCOA1.Select(newCoa => new OpeningBalanceDetailModel()
                        {
                            COAId = newCoa.Id,
                            Id = Guid.NewGuid(),
                            OpeningBalanceId = openingBalance.Id,
                            AccountName = newCoa.Name,
                            AccountCode = newCoa.Code,
                            BaseCurrency = financials.BaseCurrency,
                            DocCurrency = newCoa.Currency,
                            RecordStatus = RecordStatus.Added,
                            IsCashandbank = newCoa.AccountTypeId == acc.Id,
                            Nature = newCoa.Nature,
                            IsEditEnable = true
                        }).ToList();
                    }
                }
                lstOpeningBalanceDetailModel.AddRange(lstOBDetailForCoa);
                openingbalanceModel.Details = lstOpeningBalanceDetailModel.OrderBy(c => c.AccountCode).ToList();
                //List<ChartOfAccount> lstchartofaccount = _chartOfAccountService.GetAllChartOfAccounts(companyId).Where(a => (a.SubsidaryCompanyId == ServiceCompanyId || a.SubsidaryCompanyId == null) && (a.Name != "Accounts Receivables" && a.Name != "Other receivables" && a.Name != "Other payables" && a.Name != "Accounts payables" && a.Name != "I/C")).ToList();
                //List<ChartOfAccount> lst = lstOpeningBalanceDetailModel.q.Select(c => c.AccountName).ToList();
                //var lstCoa = lstchartofaccount.Except(lstOpeningBalanceDetailModel.Select(c => c.AccountName).ToList());
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_openingBalance_Not_Equal_To_Null_Method);
            }
            else
            {
                openingbalanceModel.Id = Guid.NewGuid();
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Else_OpeningBalance_Method);
                List<OpeningBalanceDetailModel> lstOpeningBalanceDetailModel = new List<OpeningBalanceDetailModel>();
                List<ChartOfAccount> lstchartofaccount = new List<ChartOfAccount>();
                List<ChartOfAccount> lstchartofaccount1 = new List<ChartOfAccount>();
                List<ChartOfAccount> lstchartofaccount2 = new List<ChartOfAccount>();
                List<ChartOfAccount> lstCOANew = _chartOfAccountService.GetAllChartOfAccountsByUsername(companyId, username);
                if (isInterCompanyActive == true)
                {
                    lstchartofaccount1 = lstCOANew.Where(a => (a.SubsidaryCompanyId != ServiceCompanyId || a.SubsidaryCompanyId == null) && a.IsBank != true && a.IsRealCOA == true && a.Class != "System").ToList();
                    lstchartofaccount.AddRange(lstchartofaccount1);
                    lstchartofaccount2 = lstCOANew.Where(a => a.SubsidaryCompanyId == ServiceCompanyId && a.IsBank == true && a.IsRealCOA == true && a.Class != "System").ToList();
                    lstchartofaccount.AddRange(lstchartofaccount2);
                }
                else
                {
                    //lstchartofaccount = _chartOfAccountService.GetAllChartOfAccounts(companyId).Where(a => (a.SubsidaryCompanyId == ServiceCompanyId || a.SubsidaryCompanyId == null) && !a.Name.Contains("I/C") && !a.Name.Contains("I/B") && a.IsRealCOA == true && a.Class != "System").ToList();
                    lstchartofaccount = lstCOANew.Where(a => (a.SubsidaryCompanyId == ServiceCompanyId || a.SubsidaryCompanyId == null) && !a.Name.Contains("I/C") && !a.Name.Contains("I/B") && a.IsRealCOA == true && a.Class != "System").ToList();
                }

                AccountType acc = _accountTypeService.GetById(companyId, COANameConstants.Cashandbankbalances);
                foreach (var chartofaccount in lstchartofaccount.OrderBy(a => a.Name))
                {
                    OpeningBalanceDetailModel openingBalanceDetailModel = new OpeningBalanceDetailModel();
                    FillChartofAccount(openingBalanceDetailModel, chartofaccount, companyId, acc);
                    //ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountById(openingBalanceDetailModel.COAId);
                    ChartOfAccount coa = lstchartofaccount.Where(c => c.Id == openingBalanceDetailModel.COAId).FirstOrDefault();
                    if (acc != null && coa != null)
                    {
                        openingBalanceDetailModel.IsCashandbank = coa.AccountTypeId == acc.Id ? true : false;
                        openingBalanceDetailModel.DocCurrency = coa.Currency;
                        if (openingBalanceDetailModel.IsCashandbank == true && openingBalanceDetailModel.DocCurrency == null)
                        {
                            openingBalanceDetailModel.DocCurrency = financials.BaseCurrency;
                        }
                    }
                    //if (openingbalanceModel.IsMultiCurrencyActive == false)
                    //{
                    //    openingBalanceDetailModel.DocCurrency = financials.BaseCurrency;
                    //}
                    if (multi == false)
                        openingBalanceDetailModel.DocCurrency = financials.BaseCurrency;
                    openingBalanceDetailModel.BaseCurrency = financials.BaseCurrency;
                    openingBalanceDetailModel.PayReceivableAccName = chartofaccount.Name.ToLower() == OpeningBalnceValidations.accounts_receivables ? "TR"
                                          : chartofaccount.Name.ToLower() == OpeningBalnceValidations.other_receivables ? "OR"
                                          : chartofaccount.Name.ToLower() == OpeningBalnceValidations.accounts_payable ? "TP"
                                          : chartofaccount.Name.ToLower() == OpeningBalnceValidations.other_payables ? "OP" : null;
                    if (coa != null)
                        openingBalanceDetailModel.IsPLAccount = coa.Category != null ? coa.Category.ToLower() == OpeningBalnceValidations.income_statement ? true : false : false;
                    lstOpeningBalanceDetailModel.Add(openingBalanceDetailModel);
                }
                openingbalanceModel.Date = DateTime.UtcNow;
                //var lstServiceCompany = lstchartofaccount.Where(x => x.SubsidaryCompanyId == ServiceCompanyId).ToList();
                //lstOpeningBalanceDetailModel.AddRange(lstServiceCompany);
                openingbalanceModel.Details = lstOpeningBalanceDetailModel.OrderBy(a => a.AccountCode).ToList();
            }
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_Get_ServiceCompany_OpeningBalance_Method);
            return openingbalanceModel;
        }


        public OpeningBalanceModel SaveOpeningBalanceMasterNew(OpeningBalanceModel TObject, long companyId, long ServiceCompanyId)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_Get_Service_Company_OpeningBalance_Method);
            try
            {
                OpeningBalance _openingBalance = new OpeningBalance();
                if (TObject.CurrentState == RecordStatus.Added)
                {
                    _openingBalance.Id = TObject.Id;
                    FillOpeningBalances1(_openingBalance, TObject);
                    _openingBalance.IsTemporary = true;
                    TObject.IsTemporary = _openingBalance.IsTemporary;
                    if (TObject.Details.Any())
                    {
                        int Recoder = 0;
                        OpeningBalanceDetail openingBalanceDetail = null;
                        foreach (var details in TObject.Details)
                        {
                            openingBalanceDetail = new OpeningBalanceDetail();
                            openingBalanceDetail.Recorder = ++Recoder;
                            openingBalanceDetail.Id = details.Id;
                            FillDetails1(openingBalanceDetail, details);
                            openingBalanceDetail.OpeningBalanceId = _openingBalance.Id;
                            openingBalanceDetail.ObjectState = ObjectState.Added;
                            _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                        }
                    }
                    _openingBalance.CreatedDate = DateTime.UtcNow;
                    _openingBalance.UserCreated = TObject.UserCreated;
                    _openingBalance.ObjectState = ObjectState.Added;
                    _OpeningBalanceService.Insert(_openingBalance);
                    try
                    {
                        _openingBalancesModuleUnitOfWork.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                    OpeningBalance openingBalance = _OpeningBalanceService.GetOpeningBalanceById(_openingBalance.Id);
                    TObject.Version = "0x" + string.Concat(Array.ConvertAll(openingBalance.Version, x => x.ToString("X2")));
                    TObject.Details.ForEach(c => c.OpeningBalanceId = _openingBalance.Id);
                }
                return TObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_Get_ServiceCompany_OpeningBalance_Method);
        }
        #endregion

        #region GetLineItemsForCOA
        public List<OpeningBalanceLineItemModel> GetLineItemsForCOA(long COAId, long ServiceCompanyId, string currency)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Get_LineItemsForCOA_Method);
            List<OpeningBalanceLineItemModel> lstopeningbalancelineitemmodel = new List<OpeningBalanceLineItemModel>();
            List<OpeningBalanceDetailLineItem> lstopeningBalancelineitem = _openingBalanceDetailLineItemService.GetLineItemsForCOA(COAId, ServiceCompanyId, currency);
            List<ChartOfAccount> lstCOAs = new List<ChartOfAccount>();
            List<long> COAIds = lstopeningBalancelineitem.Any() ? lstopeningBalancelineitem.Select(c => c.COAId).ToList() : new List<long>();
            if (COAIds.Any())
            {
                lstCOAs = _chartOfAccountService.GetAllCOAByIds(COAIds);
            }
            //foreach (var openingBalancelineitem in lstopeningBalancelineitem)
            //{
            //    OpeningBalanceLineItemModel openingBalanceDetaillineitem = new OpeningBalanceLineItemModel();
            //    FillLineItem(openingBalanceDetaillineitem, openingBalancelineitem, lstCOAs);
            //    lstopeningbalancelineitemmodel.Add(openingBalanceDetaillineitem);
            //    //MultiCurrencySetting multi = _multiCurrencySettingService.GetByCompanyId(openingBalancelineitem.ServiceCompanyId);
            //    //if (multi==null)
            //    //    openingBalanceDetaillineitem.DocCurrency = openingBalancelineitem.BaseCurrency;
            //}
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_Get_LineItemsForCOA_Method);
            return lstopeningBalancelineitem.Select(openingBalancelineitem => new OpeningBalanceLineItemModel
            {
                Date = openingBalancelineitem.Date,
                BaseCurrency = openingBalancelineitem.BaseCurrency,
                Id = openingBalancelineitem.Id,
                OpeningBalanceDetailId = openingBalancelineitem.OpeningBalanceDetailId,
                COAId = openingBalancelineitem.COAId,
                AccountName = lstCOAs.Any() ? lstCOAs.Where(c => c.Id == openingBalancelineitem.COAId).Select(c => c.Name).FirstOrDefault() : string.Empty,
                AccountCode = lstCOAs.Any() ? lstCOAs.Where(c => c.Id == openingBalancelineitem.COAId).Select(c => c.Code).FirstOrDefault() : string.Empty,
                BaseCredit = openingBalancelineitem.BaseCredit,
                BaseDebit = openingBalancelineitem.BaseDebit,
                DocCurrency = openingBalancelineitem.DocumentCurrency,
                DocCredit = openingBalancelineitem.DocCredit,
                DocDebit = openingBalancelineitem.DoCDebit,
                Description = openingBalancelineitem.Description,
                ExchangeRate = openingBalancelineitem.ExchangeRate,
                EntityId = openingBalancelineitem.EntityId,
                ServiceCompanyId = openingBalancelineitem.ServiceCompanyId,
                DocumentReference = openingBalancelineitem.DocumentReference,
                IsDisAllow = openingBalancelineitem.IsDisAllow,
                UserCreated = openingBalancelineitem.UserCreated,
                CreatedDate = openingBalancelineitem.CreatedDate,
                ModifiedBy = openingBalancelineitem.ModifiedBy,
                ModifiedDate = openingBalancelineitem.ModifiedDate,
                DueDate = openingBalancelineitem.DueDate,
                IsEditable = openingBalancelineitem.IsEditable,
                RecOrder = openingBalancelineitem.Recorder,
                IsProcressed = openingBalancelineitem.IsProcressed,
                ProcressedRemarks = openingBalancelineitem.ProcressedRemarks,
            }).OrderBy(x => x.RecOrder).ToList();

            // return lstopeningbalancelineitemmodel.OrderBy(x => x.RecOrder).ToList();
        }
        #endregion
        public IQueryable<OpeningBalanceLineItemModel> GetLineItemsForCOAs(long COAId, long ServiceCompanyId, string currency, long companyId)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Get_LineItemsForCOA_Method);
            //IQueryable<OpeningBalanceLineItemModel> lstopeningbalancelineitemmodel = new IQueryable<OpeningBalanceLineItemModel>();
            return _openingBalanceDetailLineItemService.GetLineItemsForCOAs(COAId, ServiceCompanyId, currency, companyId);
            //var xyz = lineItems.ToList();

            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_Get_LineItemsForCOA_Method);

        }
        public IQueryable<OpeningBalanceLineItemModel> GetLineItemsForCOA(long COAId, long ServiceCompanyId, string currency, long companyId, Guid detailId,string connectionString)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Get_LineItemsForCOA_Method);

            using (SqlConnection DwhConn = new SqlConnection(connectionString))
            {
                if (DwhConn.State == ConnectionState.Closed)
                    DwhConn.Open();
                SqlCommand cmd = new SqlCommand("Bean_OpeningBalance_Line_Item", DwhConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COAId", COAId);
                cmd.Parameters.AddWithValue("@ServiceCompanyId", ServiceCompanyId);
                cmd.Parameters.AddWithValue("@Currency", currency);
                cmd.Parameters.AddWithValue("@CompanyId", companyId);
                cmd.Parameters.AddWithValue("@detailId", detailId);
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return (from row in dt.AsEnumerable()
                        select new OpeningBalanceLineItemModel
                        {
                            Date = row.Field<DateTime?>(0) != null ? row.Field<DateTime?>(0) : null,
                            BaseCurrency = row.Field<string>(1) != null ? row.Field<string>(1) : string.Empty,
                            //Id = row.Field<Guid>(2) != null ? row.Field<Guid>(2) : Guid.Empty,
                            Id = row["Id"] != DBNull.Value ?(Guid)row["Id"] : Guid.Empty,
                            OpeningBalanceDetailId = row["OpeningBalanceDetailId"] !=DBNull.Value?(Guid)row["OpeningBalanceDetailId"]:Guid.Empty,
                            COAId =   row.Field<long>(4),
                            AccountName = row.Field<string>(5) != null ? row.Field<string>(5) : string.Empty,
                            AccountCode = row.Field<string>(6) != null ? row.Field<string>(6) : string.Empty,
                            BaseCredit = row.Field<decimal?>(7) != null ? row.Field<decimal?>(7) : null,
                            BaseDebit = row.Field<decimal?>(8) != null ? row.Field<decimal?>(8) : null,
                            DocCurrency = row.Field<string>(9) != null ? row.Field<string>(9) : string.Empty,
                            DocCredit = row.Field<decimal?>(10) != null ? row.Field<decimal?>(10) : null,
                            DocDebit = row.Field<decimal?>(11) != null ? row.Field<decimal?>(11) : null,
                            Description = row.Field<string>(12) != null ? row.Field<string>(12) : string.Empty,
                            ExchangeRate = row.Field<decimal?>(13) != null ? row.Field<decimal?>(13) : null,
                            //EntityId = row.Field<Guid>(14) != null ? row.Field<Guid>(14) : Guid.Empty,
                            EntityId = row["EntityId"] != DBNull.Value ? (Guid)row["EntityId"] : Guid.Empty,
                            ServiceCompanyId = row.Field<long>(15) != null ? row.Field<long>(15) : 0,
                            DocumentReference = row.Field<string>(16) != null ? row.Field<string>(16) : string.Empty,
                            IsDisAllow = row.Field<bool?>(17) != null ? row.Field<bool?>(17) : null,
                            UserCreated = row.Field<string>(18) != null ? row.Field<string>(18) : string.Empty,
                            CreatedDate = row.Field<DateTime>(19) != null ? row.Field<DateTime>(19) : DateTime.Now,
                            ModifiedBy = row.Field<string>(20) != null ? row.Field<string>(20) : string.Empty,
                            ModifiedDate = row.Field<DateTime>(21) != null ? row.Field<DateTime>(21) : DateTime.Now,
                            DueDate = row.Field<DateTime?>(22) != null ? row.Field<DateTime?>(22) : null,
                            IsEditable = row.Field<bool?>(23) != null ? row.Field<bool?>(23) : null,
                            RecOrder = row.Field<int>(24) != null ? row.Field<int>(24) : 0,
                            IsProcressed = row.Field<bool?>(25) != null ? row.Field<bool?>(25) : null,
                            ProcressedRemarks = row.Field<string>(26) != null ? row.Field<string>(26) : string.Empty
                        }).AsQueryable();
            }

            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Leave_From_Get_LineItemsForCOA_Method);

        }

        #region GetCall_Getlineitems
        public OpeningBalanceLineItemModel Getlineitems()
        {
            OpeningBalanceLineItemModel openingBalanceLineItemModel = new OpeningBalanceLineItemModel();
            return openingBalanceLineItemModel;
        }
        #endregion

        #region _GetLineItemsByCOAId

        public List<OpeningBalanceDetailModel> GetDetailsByCOAID(long COAId, long ServiceCompanyId)
        {
            //List<OpeningBalanceLineItemModel> lstopeningbalancelineitemmodelByCoaid = new List<OpeningBalanceLineItemModel>();
            //List<OpeningBalanceDetail> lstopeningBalancelineitemByCoaid = _openingBalanceDetailLineItemService.GetLineItemsByCOAID(COAId, ServiceCompanyId);

            //foreach (var openingBalancelineitem in lstopeningBalancelineitemByCoaid)
            //{
            //    OpeningBalanceLineItemModel openingBalanceDetaillineitemM = new OpeningBalanceLineItemModel();
            //    FillLineItemByCOAID(openingBalanceDetaillineitemM, openingBalancelineitem);
            //    lstopeningbalancelineitemmodelByCoaid.Add(openingBalanceDetaillineitemM);
            //}
            //return lstopeningbalancelineitemmodelByCoaid;
            List<OpeningBalanceDetailModel> lstOpeningBalanceDetailModel = new List<OpeningBalanceDetailModel>();
            OpeningBalance openingBalanceByServiceCompanyId = _OpeningBalanceService.GetServiceCompanyOpeningBalance(ServiceCompanyId);
            ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountById(COAId);
            if (openingBalanceByServiceCompanyId != null)
            {
                //  List<OpeningBalanceDetail> lstopeningBalancelineitemByCoaid = openingBalanceByServiceCompanyId.OpeningBalanceDetails.Where(c => c.COAId == COAId && openingBalanceByServiceCompanyId.Id).Select();
                List<OpeningBalanceDetail> lstopeningBalanceDetailByCoaid = _OpeningBalanceDetailService.openingBalanceDetailByCOAID(COAId, openingBalanceByServiceCompanyId.Id);


                if (lstopeningBalanceDetailByCoaid.Count > 0)
                {
                    foreach (var openingBalancelineitemByCoaid in lstopeningBalanceDetailByCoaid)
                    {
                        OpeningBalanceDetailModel openingBalanceM = new OpeningBalanceDetailModel();
                        FillOpeningBalanceDetailModel(openingBalanceM, openingBalancelineitemByCoaid);
                        if (coa != null)
                        {
                            openingBalanceM.AccountName = coa.Name;
                            openingBalanceM.AccountCode = coa.Code;
                            openingBalanceM.DocCurrency = coa.Currency;
                        }
                        lstOpeningBalanceDetailModel.Add(openingBalanceM);
                    }
                }
            }
            else
            {
                OpeningBalanceDetailModel openingBalanceM = new OpeningBalanceDetailModel();
                openingBalanceM.AccountCode = coa.Code;
                openingBalanceM.AccountName = coa.Name;
                openingBalanceM.COAId = COAId;
                lstOpeningBalanceDetailModel.Add(openingBalanceM);

            }
            return lstOpeningBalanceDetailModel;

        }

        #endregion

        #region DeleteTempRecord_FromOB_OBD_OBLD
        public string DeleteTempRecordFromOB_OBD_OBLD(Guid obId,string connectionString)
        {
            try
            {
                SqlConnection con = null;
                SqlCommand cmd = null;
                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("DeleteTempRecordFromOB_OBD_OBLD_proc", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OBId", obId);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                 
            }
            return "Records has deleted Successfully!";
        }
        #endregion DeleteTempRecord_FromOB_OBD_OBLD

        #endregion

        #region Save Call
        string serviceCompanyName;
        public OpeningBalance SaveOpeningBalance(OpeningBalanceModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_SaveOpeningBalance_Method);
            OpeningBalanceModel openingBalanceModel = new OpeningBalanceModel();
            string update = null;
            string _errors = CommonValidation.ValidateObject(TObject);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_errors);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            Dictionary<long, string> lstCOAs = _chartOfAccountService.GetChartofAccounts(TObject.Details.Select(c => c.AccountName).ToList(), TObject.CompanyId);
            if (lstCOAs.Any())
            {
                string name = TObject.Details.Where(c => !lstCOAs.Values.Contains(c.AccountName)).Select(c => c.AccountName).FirstOrDefault();
                if (name != null || name != string.Empty)
                    throw new Exception("The " + name + " COA has been deleted, can't post");
            }
            if (TObject.SaveType == "Draft")
            {
                JournalSaveModel tObject = new JournalSaveModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                deleteJVPostInvoce(tObject);
            }
            bool? IsEdit = false;


            //for Doc No checking
            List<OpeningBalanceLineItemModel> listItems = TObject.Details.SelectMany(c => c.LineItems.Where(d => d != null)).ToList();
            string docNo = null;
            if (listItems.Any())
            {
                IDictionary<string, Guid> invoiceDocNo = _billService.GetAllInvoiceDocNo(TObject.CompanyId);
                List<OpeningBalanceLineItemModel> LstTROR = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade receivables" || d.AccountName == "Other receivables" && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId)).ToList();
                if (LstTROR.Any())
                {
                    if (invoiceDocNo.Any())
                    {
                        List<string> demo = LstTROR.GroupBy(c => new { DocNo = c.DocumentReference, value = c.Id }).Where(s => invoiceDocNo.Keys.Contains(s.Key.DocNo) && !invoiceDocNo.Values.Contains(s.Key.value)).Select(s => s.Key.DocNo).ToList();
                        List<Guid> invoiceIds = LstTROR.Select(c => c.Id).ToList();
                        if (demo.Any())
                            docNo = string.Join(",", demo);
                        if (LstTROR.GroupBy(c => new { DocNo = c.DocumentReference, value = c.Id }).Where(s => invoiceDocNo.Keys.Contains(s.Key.DocNo) && !invoiceDocNo.Values.Contains(s.Key.value)).ToList().Count > 0)
                            throw new Exception(docNo + " " + "Document number(s) are existing in Invoice");
                    }
                    //List<string> allDocNo = LstTROR.GroupBy(c => c.DocumentReference).Select(c => c.Key).ToList();
                    var allDocNo = LstTROR.GroupBy(c => new { DocNo = c.DocumentReference }).Select(a => new { DocNo = a.Key.DocNo, Count = a.Count() });
                    //allDocNo.Remove(string.Empty);
                    if (allDocNo.Any())
                        docNo = string.Join(",", allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).ToList());
                    //if (LstTROR.GroupBy(c => c.DocumentReference).ToList().Any(d => d.Count() > 1))
                    //    throw new Exception(docNo + " " + "Document number(s) are duplicated in TR/OR lines");

                    if (allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).Any())
                        throw new Exception(docNo + " " + "Document number(s) are duplicated in TR/OR lines");
                }
                List<OpeningBalanceLineItemModel> LstTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade payables" || d.AccountName == "Other payables" && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId)).ToList();
                if (LstTPOP.Any())
                {
                    //IDictionary<string, Guid> billDocNo = _billService.GetAllBillDocNo(TObject.CompanyId);
                    List<Bill> lstBills = _billService.GetAllBillDocNo(TObject.CompanyId);
                    if (lstBills.Any())
                    {
                        List<string> billDocs = LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, EntityId = c.EntityId, Id = c.Id }).Where(s => lstBills.Select(c => c.DocNo).ToList().Contains(s.Key.DocNo) && lstBills.Select(c => c.EntityId).ToList().Any(d => d == s.Key.EntityId) && !lstBills.Select(c => c.Id).ToList().Contains(s.Key.Id)).Select(c => c.Key.DocNo).ToList();
                        if (billDocs.Any())
                            docNo = string.Join(",", billDocs);
                        if (LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, EntityIds = c.EntityId, Ids = c.Id }).Where(s => lstBills.Select(c => c.DocNo).ToList().Contains(s.Key.DocNo) && lstBills.Select(c => c.EntityId).ToList().Any(d => d == s.Key.EntityIds) && !lstBills.Select(c => c.Id).ToList().Contains(s.Key.Ids)).ToList().Count > 0)
                            throw new Exception(docNo + " " + "Document number(s) are existing in Bill");
                        var lstOPTP = LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).Select(c => new { c.Key.DocNo, Count = c.Count(), c.Key.Entity }).ToList();
                        if (lstOPTP.Any(c => c.Count > 1))
                            docNo = string.Join(",", lstOPTP.Where(c => c.Count > 1).Select(c => c.DocNo));

                        //var xx= LstTPOP.Select(z=>new { DocNo = z.DocumentReference, Entity = z.EntityId })
                        //var x = (from l in LstTPOP
                        //         group l by new { l.DocumentReference, l.EntityId }
                        //       into ll
                        //         select new
                        //         {
                        //             R = ll.Key.EntityId,
                        //             Do = ll.Key.DocumentReference,
                        //             D = ll.Count()
                        //         });

                        if (LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).ToList().Any(d => d.Count() > 1) /*&& LstTPOP.Where(c => c.EntityId == item.Key).Select(c => c.EntityId).FirstOrDefault() != item.Key*/)


                            throw new Exception(docNo + " " + "Document number(s) are duplicated in TP/OP lines");
                        //foreach (var item in LstTPOP.GroupBy(c => c.EntityId).ToList())
                        //{
                        //    if ((LstTPOP.GroupBy(c => c.DocumentReference).ToList().Where(s => billDocNo.Keys.Contains(s.Key)).ToList().Count > 0) && (billDocNo.Values.Any(d => d == item.Key)))
                        //        throw new Exception("Document no alredy exist in bill");
                        //    if (LstTPOP.Where(c => c.EntityId == item.Key).GroupBy(c => c.DocumentReference).ToList().Any(d => d.Count() > 1) /*&& LstTPOP.Where(c => c.EntityId == item.Key).Select(c => c.EntityId).FirstOrDefault() != item.Key*/)
                        //        throw new Exception("Doc No. shouldn't duplicate of line items");
                        //}
                    }
                    //if (LstTPOP.GroupBy(c => c.DocumentReference).ToList().Any(d => d.Count() > 1))
                    //    throw new Exception("Doc No. shouldn't duplicate of line items");
                    //else
                    //{

                    //}
                }
            }

            //if (TObject.Details.Any())
            //{
            //    foreach (var details in TObject.Details)
            //    {
            //        if ((details.BaseCredit != null || details.BaseDebit != null || details.DocCredit != null || details.DocDebit != null) && details.DocCurrency == null)
            //            throw new Exception(OpeningBalnceValidations.Doc_Currency_Should_Not_Be_Empty);
            //        if (details.BaseCredit != null && details.BaseDebit != null)
            //            throw new Exception(OpeningBalnceValidations.BaseCredit_And_BaseDebit_are_not_exisit_at_a_time);
            //        if (details.DocCredit != null && details.DocDebit != null)
            //            throw new Exception(OpeningBalnceValidations.DocCredit_And_DocDebit_are_not_exisit_at_a_time);
            //    }
            //}

            if (TObject.SaveType != "Draft")
            {
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.Date, TObject.CompanyId))
                    throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.Date, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                        throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.Date, TObject.PeriodLockPassword, TObject.CompanyId))
                        throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
                if (TObject.Details.Any())
                {
                    var basecredit = Math.Round(TObject.Details.Sum(c => c.BaseCredit).Value, 2);
                    var basedebit = Math.Round(TObject.Details.Sum(c => c.BaseDebit).Value, 2);

                    if (basecredit != basedebit)
                        throw new Exception(OpeningBalnceValidations.BaseCredit_And_BaseDebit_Shoulb_Be_Equal);
                }
            }

            //OpeningBalanceDetail openingBalanceDetail = new OpeningBalanceDetail();
            //OpeningBalanceDetailLineItem openingBalanceDetailLineItem = new OpeningBalanceDetailLineItem();
            OpeningBalance _openingBalance = _OpeningBalanceService.CheckOpeningBalanceById(TObject.Id);
            if (_openingBalance != null)
            {
                //TObject.Details.Where(c => c.LineItems != null).Select(a => a.LineItems.Any(x => x.Date <= TObject.Date)).Any() == false) == false;
                // var lstDate = TObject.Details.Where(c => c.LineItems != null).Select(a => a.LineItems.Count>=1).ToList();
                //var br = TObject.Details.Where(c => c.LineItems != null).SelectMany(a => a.LineItems).Where(x => x.Date > TObject.Date).Any();
                if (TObject.Details.Where(c => c.LineItems != null).SelectMany(a => a.LineItems).Where(x => x.Date > TObject.Date).Any())
                {
                    throw new Exception("Opening Balance Date cannot be less than Opening Balance Line Item Date");
                }
                //if (TObject.Details.Where(c => c.LineItems != null).Select(a => a.LineItems.Any(x => x.Date > TObject.Date) == true).Any())
                //{
                //    throw new Exception("Opening Balance Date cannot be less than Opening Balance Line Item Date");
                //}

                //if (lstObDetail.Select(a => a.OpeningBalanceDetailLineItems.Where(c => c.Date >= TObject.Date)/*.Any()==true*/).Any() == true)
                //{
                //    throw new Exception("Opening Balance Date cannot be less than Opening Balance Line Item Date");
                //}

                update = "updated";
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_openingBalance_Is_Not_Equal_To_Null);
                FillOpeningBalances(_openingBalance, TObject);
                _openingBalance.IsEditable = TObject.IsEditable;
                _openingBalance.SystemRefNo = TObject.SystemRefNo;
                _openingBalance.ModifiedDate = DateTime.UtcNow;
                _openingBalance.ModifiedBy = TObject.UserModified;
                _openingBalance.ObjectState = ObjectState.Modified;
                _OpeningBalanceService.Update(_openingBalance);

                #region Commented Code
                //if (TObject.Details != null)
                //{
                //foreach (var details in TObject.Details)
                //{
                //    openingBalanceDetail = new OpeningBalanceDetail();
                //    if (details.RecordStatus != "Added" && details.RecordStatus != "Deleted")
                //    {
                //        FillOpeningBalanceVmToEntity(details, openingBalanceDetail, TObject);
                //        openingBalanceDetail.ObjectState = ObjectState.Modified;
                //        _OpeningBalanceDetailService.Update(openingBalanceDetail);
                //        if (details.LineItems != null)
                //        {
                //            foreach (var lineItems in details.LineItems)
                //            {
                //                if (lineItems.RecordStatus != "Added" && lineItems.RecordStatus != "Deleted")
                //                  {
                //                    openingBalanceDetailLineItem = new OpeningBalanceDetailLineItem();
                //                    FillDetailLineItemVmToEntity(lineItems, openingBalanceDetailLineItem);
                //                    openingBalanceDetailLineItem.OpeningBalanceDetailId = details.Id;
                //                    openingBalanceDetailLineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                //                    openingBalanceDetailLineItem.Id = lineItems.Id;
                //                    openingBalanceDetailLineItem.ObjectState = ObjectState.Modified;
                //                    openingBalanceDetailLineItem.ModifiedBy = lineItems.ModifiedBy;
                //                    openingBalanceDetailLineItem.ModifiedDate = lineItems.ModifiedDate;
                //                    _openingBalanceDetailLineItemService.Update(openingBalanceDetailLineItem);
                //                }
                //                else if (lineItems.RecordStatus == "Added")
                //                {
                //                    openingBalanceDetailLineItem = new OpeningBalanceDetailLineItem();
                //                    openingBalanceDetailLineItem.Id = Guid.NewGuid();
                //                    openingBalanceDetailLineItem.OpeningBalanceDetailId = details.Id;
                //                    openingBalanceDetailLineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                //                    FillDetailLineItemVmToEntity(lineItems, openingBalanceDetailLineItem);
                //                    openingBalanceDetailLineItem.ObjectState = ObjectState.Added;
                //                    openingBalanceDetailLineItem.UserCreated = lineItems.UserCreated;
                //                    openingBalanceDetailLineItem.CreatedDate = lineItems.CreatedDate;
                //                    _openingBalanceDetailLineItemService.Insert(openingBalanceDetailLineItem);
                //                }
                //                else if (lineItems.RecordStatus == "Deleted")
                //                {
                //                    deleteLineItem(lineItems.Id);
                //                }
                //            }
                //        }
                //    }
                //    else if (details.RecordStatus == "Added")
                //    {
                //        FillDetails(openingBalanceDetail, details, TObject);
                //        openingBalanceDetail.ObjectState = ObjectState.Added;
                //        _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                //        if (details.LineItems != null)
                //        {
                //            foreach (var lineItems in details.LineItems)
                //            {
                //                OpeningBalanceDetailLineItem lineItem = new OpeningBalanceDetailLineItem();
                //                lineItem.Id = Guid.NewGuid();
                //                lineItem.OpeningBalanceDetailId = openingBalanceDetail.Id;
                //                lineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                //                FillDetailLineItemVmToEntity(lineItems, lineItem);
                //                lineItem.UserCreated = lineItems.UserCreated;
                //                lineItem.CreatedDate = DateTime.UtcNow;
                //                lineItem.ObjectState = ObjectState.Added;
                //                _openingBalanceDetailLineItemService.Insert(lineItem);
                //            }
                //        }

                //    }
                //    else if (details.RecordStatus == "Deleted")
                //    {
                //        deleteDetail(details.Id);
                //    }
                //}
                #endregion

                IsEdit = true;

                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("PROC_OPENINGBALANCE_DELETE", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", TObject.Id);
                int deletedRowCount = cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_openingBalance_Else_Block);
                _openingBalance = new OpeningBalance();
                _openingBalance.Id = Guid.NewGuid();
                //TObject.Id = _openingBalance.Id;
                FillOpeningBalances(_openingBalance, TObject);
                _openingBalance.CreatedDate = DateTime.UtcNow;
                _openingBalance.UserCreated = TObject.UserCreated;
                _openingBalance.IsEditable = true;
                Company company = _companyService.GetById(TObject.CompanyId);
                _openingBalance.SystemRefNo = GenerateAutoNumberForType(company.Id, OpeningBalanceConstants.DocType, company.ShortName);
                _openingBalance.ObjectState = ObjectState.Added;
                _OpeningBalanceService.Insert(_openingBalance);
            }

            if (TObject.Details != null)
            {
                int Recoder = 0;
                int Recorder1 = 0;

                //if (IsEdit == true)
                //{
                //    if ((details.AccountName == "Trade receivables" || details.AccountName == "Other receivables" || details.AccountName == "Trade payables" || details.AccountName == "Other payables"))
                //}
                //List<OpeningBalanceDetail> lstObDetail = null;
                //if (IsEdit == true)
                //{
                //    lstObDetail = _OpeningBalanceDetailService.GetOpeningBalanceDetail(_openingBalance.Id);
                //    if (lstObDetail.Any())
                //    {
                //        foreach (var Detail in lstObDetail)
                //        {
                //            if (Detail.OpeningBalanceDetailLineItems.Any() == false)
                //                Detail.ObjectState = ObjectState.Deleted;
                //            _openingBalancesModuleUnitOfWork.SaveChanges();
                //        }
                //    }
                //}

                //List<OpeningBalanceLineItemModel> listItems = TObject.Details.SelectMany(c => c.LineItems.Where(d => d != null)).ToList();
                //if (listItems.Any())
                //{
                //    List<string> invoiceDocNo = _billService.GetAllInvoiceDocNo(TObject.CompanyId);
                //    List<OpeningBalanceLineItemModel> LstTROR = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade receivables" || d.AccountName == "Other receivables").Select(x => x.COAId).Contains(c.COAId)).ToList();
                //    if (LstTROR.Any())
                //    {
                //        if (invoiceDocNo.Any())
                //        {
                //            if (LstTROR.GroupBy(c => c.DocumentReference).ToList().Where(s => invoiceDocNo.Contains(s.Key)).ToList().Count > 0)
                //                throw new Exception("Document no alredy exist in invoice");
                //        }
                //        if (LstTROR.GroupBy(c => c.DocumentReference).ToList().Any(d => d.Count() > 1))
                //            throw new Exception("Doc No. shouldn't duplicate of line items");
                //        else
                //        { }
                //    }
                //    List<OpeningBalanceLineItemModel> LstTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade payables" || d.AccountName == "Other payables").Select(x => x.COAId).Contains(c.COAId)).ToList();
                //    if (LstTPOP.Any())
                //    {
                //        IDictionary<string, Guid> billDocNo = _billService.GetAllBillDocNo(TObject.CompanyId);
                //        if (billDocNo.Any())
                //        {
                //            foreach (var item in LstTPOP.GroupBy(c => c.EntityId).ToList())
                //            {
                //                if ((LstTPOP.GroupBy(c => c.DocumentReference).ToList().Where(s => billDocNo.Keys.Contains(s.Key)).ToList().Count > 0) && (billDocNo.Values.Any(d => d == item.Key)))
                //                    throw new Exception("Document no alredy exist in bill");
                //                if (LstTPOP.Where(c => c.EntityId == item.Key).GroupBy(c => c.DocumentReference).ToList().Any(d => d.Count() > 1) /*&& LstTPOP.Where(c => c.EntityId == item.Key).Select(c => c.EntityId).FirstOrDefault() != item.Key*/)
                //                    throw new Exception("Doc No. shouldn't duplicate of line items");
                //            }
                //        }
                //        //if (LstTPOP.GroupBy(c => c.DocumentReference).ToList().Any(d => d.Count() > 1))
                //        //    throw new Exception("Doc No. shouldn't duplicate of line items");
                //        //else
                //        //{

                //        //}
                //    }
                //}
                foreach (var details in TObject.Details)
                {
                    //Guid? detailId = null;
                    bool? isDetailEditable = false;

                    #region Commented_Code
                    //if (IsEdit == true)
                    //{
                    //    if ((details.AccountName == "Trade receivables" || details.AccountName == "Other receivables" || details.AccountName == "Trade payables" || details.AccountName == "Other payables"))
                    //    {
                    //        //isDetailEditable = details.LineItems != null ? details.LineItems.Where(a => a.OpeningBalanceDetailId == details.Id).Select(a => a.IsEditable).FirstOrDefault() : false;
                    //        //if (details.LineItems != null && (details.DocCredit != null || details.DocDebit != null))
                    //        OpeningBalanceDetail obDetail = _OpeningBalanceDetailService.GetOBDetail(details.Id, _openingBalance.Id);
                    //        //OpeningBalanceDetail obDetail = lstObDetail.Any() ? lstObDetail.Where(a => a.Id == details.Id).FirstOrDefault() : null;
                    //        if (obDetail != null)
                    //            if (obDetail.DocCredit == null && obDetail.DocDebit == null)
                    //                obDetail.ObjectState = ObjectState.Deleted;
                    //            else
                    //            {
                    //                obDetail.Recorder = Recoder + 1;
                    //                Recoder = obDetail.Recorder;
                    //                obDetail.COAId = details.COAId;
                    //                obDetail.BaseCurrency = details.BaseCurrency;
                    //                obDetail.BaseCredit = details.BaseCredit;
                    //                obDetail.BaseDebit = details.BaseDebit;
                    //                obDetail.DocCurrency = details.DocCurrency;
                    //                obDetail.DocCredit = details.DocCredit;
                    //                obDetail.DocDebit = details.DocDebit;
                    //                obDetail.UserCreated = details.UserCreated;
                    //                obDetail.CreatedDate = DateTime.UtcNow;
                    //                obDetail.IsOrginalAccount = details.IsOrginalAccount;
                    //                obDetail.OpeningBalanceId = _openingBalance.Id;
                    //                obDetail.ObjectState = ObjectState.Modified;
                    //                detailId = obDetail.Id;
                    //                _OpeningBalanceDetailService.Update(obDetail);
                    //                isDetailEditable = true;
                    //            }
                    //        //else
                    //        //{
                    //        //    OpeningBalanceDetail obDetail = _OpeningBalanceDetailService.GetOBDetail(details.Id);
                    //        //    if (obDetail != null && (obDetail.DocCredit == null || obDetail.DocDebit == null))
                    //        //        obDetail.ObjectState = ObjectState.Deleted;
                    //        //}
                    //    }
                    //}
                    //if (isDetailEditable != true)
                    //{
                    #endregion

                    OpeningBalanceDetail openingBalanceDetail = new OpeningBalanceDetail();
                    openingBalanceDetail.Recorder = ++Recoder;
                    //Recoder = openingBalanceDetail.Recorder;
                    openingBalanceDetail.Id = IsEdit == true ? details.Id : Guid.NewGuid();
                    FillDetails(openingBalanceDetail, details, TObject);
                    openingBalanceDetail.OpeningBalanceId = _openingBalance.Id;
                    openingBalanceDetail.ObjectState = ObjectState.Added;
                    _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                    //}
                    if (details.LineItems != null)
                    {
                        foreach (var lineItems in details.LineItems)
                        {
                            try
                            {
                                if (lineItems.RecordStatus == "Deleted" && ((details.AccountName == "Trade receivables" || details.AccountName == "Other receivables" || details.AccountName == "Trade payables" || details.AccountName == "Other payables")) && lineItems.IsEditable != false)
                                {
                                    SqlConnection conn = new SqlConnection(ConnectionString);
                                    if (conn.State != ConnectionState.Open)
                                        conn.Open();
                                    SqlCommand oBcmd = new SqlCommand("Proc_DeleteOBInvoiceAndOBBillLineItem", conn);
                                    oBcmd.CommandType = CommandType.StoredProcedure;
                                    oBcmd.Parameters.AddWithValue("@OBId", TObject.Id);
                                    oBcmd.Parameters.AddWithValue("@DocID", lineItems.Id);
                                    oBcmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId);
                                    oBcmd.Parameters.AddWithValue("@IsInvoice", (details.AccountName == "Trade receivables" || details.AccountName == "Other receivables") ? true : false);
                                    int res = oBcmd.ExecuteNonQuery();
                                    conn.Close();
                                }
                                if (lineItems.RecordStatus != "Deleted")
                                {
                                    //bool? isDeletedLineItem = false;

                                    #region Coomented_Code
                                    //if (lineItems.IsEditable == false)
                                    //{
                                    //    OpeningBalanceDetailLineItem lineItem1 = new OpeningBalanceDetailLineItem();
                                    //    lineItem1.Recorder = (Recorder1) + 1;
                                    //    lineItem1.Id = lineItems.Id;
                                    //    //lineItem1.OpeningBalanceDetailId = openingBalanceDetail.Id;
                                    //    lineItem1.OpeningBalanceDetailId = detailId.Value;
                                    //    lineItem1.COAId = lineItems.COAId;
                                    //    Recorder1 = lineItem1.Recorder;
                                    //    lineItem1.ObjectState = ObjectState.Modified;
                                    //    _openingBalanceDetailLineItemService.Update(lineItem1);
                                    //}
                                    //else
                                    //{
                                    #endregion

                                    if (IsEdit == true && ((details.AccountName == "Trade receivables" || details.AccountName == "Other receivables" || details.AccountName == "Trade payables" || details.AccountName == "Other payables")) && lineItems.IsEditable != false)
                                    {
                                        //isDeletedLineItem = _openingBalanceDetailLineItemService.IsLineItemExist(lineItems.Id, details.Id);
                                        //if (isDeletedLineItem == true)
                                        //{
                                        SqlConnection conn = new SqlConnection(ConnectionString);
                                        if (conn.State != ConnectionState.Open)
                                            conn.Open();
                                        SqlCommand oBcmd = new SqlCommand("Proc_DeleteOBInvoiceAndOBBillLineItem", conn);
                                        oBcmd.CommandType = CommandType.StoredProcedure;
                                        oBcmd.Parameters.AddWithValue("@OBId", TObject.Id);
                                        oBcmd.Parameters.AddWithValue("@DocID", lineItems.Id);
                                        oBcmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId);
                                        oBcmd.Parameters.AddWithValue("@IsInvoice", (details.AccountName == "Trade receivables" || details.AccountName == "Other receivables") ? true : false);
                                        int res = oBcmd.ExecuteNonQuery();
                                        conn.Close();
                                        //}
                                        //line item update
                                        //isDeletedLineItem = _openingBalanceDetailLineItemService.IsLineItemExist(lineItems.Id, details.Id);
                                    }
                                    OpeningBalanceDetailLineItem lineItem = new OpeningBalanceDetailLineItem();
                                    lineItem.Id = IsEdit == true ? lineItems.Id : Guid.NewGuid();
                                    lineItem.OpeningBalanceDetailId = openingBalanceDetail.Id;
                                    //lineItem.OpeningBalanceDetailId = detailId.Value;
                                    lineItem.EntityId = lineItems.EntityId;
                                    lineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                                    FillDetailLineItemVmToEntity(lineItems, lineItem);
                                    lineItem.Recorder = (Recorder1) + 1;
                                    Recorder1 = lineItem.Recorder;
                                    lineItem.UserCreated = lineItems.UserCreated;
                                    lineItem.CreatedDate = DateTime.UtcNow;
                                    lineItem.IsEditable = lineItems.IsEditable;
                                    //if (isDeletedLineItem == true)
                                    //{
                                    //    lineItem.ObjectState = ObjectState.Modified;
                                    //    _openingBalanceDetailLineItemService.Update(lineItem);
                                    //}
                                    //else
                                    //{
                                    lineItem.ObjectState = ObjectState.Added;
                                    _openingBalanceDetailLineItemService.Insert(lineItem);

                                    //}

                                    //FOR TR/OR/TP/OP line item
                                    if (lineItems.IsEditable != false && TObject.SaveType != "Draft" && lineItems.RecordStatus != "Deleted")
                                    {
                                        long? taxId = null;
                                        long? coaId = null;
                                        if ((details.AccountName == "Trade receivables" || details.AccountName == "Other receivables" || details.AccountName == "Trade payables" || details.AccountName == "Other payables"))
                                        {
                                            coaId = _chartOfAccountService.GetChartOfAccountIDByName("Opening balance", TObject.CompanyId);
                                        }
                                        if (details.AccountName == "Trade receivables" || details.AccountName == "Other receivables")
                                        {


                                            Guid? itemId = Item(TObject, lineItem, TObject.IsGSTActivated, coaId, out taxId, ConnectionString);
                                            InvoiceModel invoiceModel = new InvoiceModel();
                                            FillInvoiceModel(invoiceModel, lineItem, TObject, itemId, taxId, coaId, details.AccountName == "Trade receivables" ? "Trade" : "Others", _openingBalance.Id, false);

                                        }
                                        else if (details.AccountName == "Trade payables" || details.AccountName == "Other payables")
                                        {
                                            InvoiceModel invoiceModel = new InvoiceModel();
                                            FillInvoiceModel(invoiceModel, lineItem, TObject, null, taxId, coaId, details.AccountName == "Trade payables" ? "Trade" : "Others", _openingBalance.Id, true);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //throw new Exception("Error Occured While Updating the line item");
                            }
                        }
                    }
                }
            }
            if (update == "updated")
            {
                _openingBalance.ModifiedBy = TObject.UserModified;
                _openingBalance.ModifiedDate = TObject.ModifiedDate;
            }
            try
            {
                //serviceCompanyName = TObject.ServiceCompanyName;
                serviceCompanyName = TObject.Date.ToString("dd/MM/yyyy");

                _openingBalancesModuleUnitOfWork.SaveChanges();

                #region Posting
                if (TObject.SaveType != "Draft")
                {
                    List<long> coaId = TObject.Details.Where(c => c.AccountName == COANameConstants.AccountsPayable || c.AccountName == COANameConstants.OtherPayables || c.AccountName == COANameConstants.AccountsReceivables || c.AccountName == COANameConstants.OtherReceivables).Select(d => d.COAId).ToList();
                    List<ChartOfAccount> lstCOA = _chartOfAccountService.GetAllCOAById(TObject.CompanyId, coaId);
                    JVModel jvm = new JVModel();
                    _openingBalance.OpeningBalanceDetails = _OpeningBalanceDetailService.GetServiceCompanyOpeningBalance(_openingBalance.Id);
                    FillJournal(jvm, _openingBalance, false, lstCOA, TObject.ServiceCompanyName);
                    jvm.Id = _openingBalance.Id;
                    jvm.DocumentDescription = DocTypeConstants.OpeningBalance + " - " + serviceCompanyName;
                    SaveOBPostings(jvm);
                }

                #endregion

                // LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService,OpeningBalanceLoggingValidation.Entered_Into_EventStore_Method);
                //if (update == "updated")
                //    DomainEventChannel.Raise(new OpeningBalanceUpdated(TObject));
                //else
                //    DomainEventChannel.Raise(new OpeningBalanceCreated(TObject));
            }
            catch (DbEntityValidationException ex)
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Catch_Block);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }

            return _openingBalance;
        }

        public OpeningBalance SaveOpeningBalanceNew(OpeningBalanceModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, "ObjectSave", AdditionalInfo);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_SaveOpeningBalance_Method);
            OpeningBalanceModel openingBalanceModel = new OpeningBalanceModel();
            SqlConnection con = null;
            SqlCommand cmd = null;

            List<Guid?> newEntityIds = new List<Guid?>();
            List<Guid?> oldEntityIds = new List<Guid?>();
            Dictionary<Guid, string> lstLineItem = null;
            Dictionary<Guid, string> lstEntity = null;
            List<OpeningBalanceDetailLineItem> lstObLineItem = new List<OpeningBalanceDetailLineItem>();
            List<OpeningBalanceLineItemModel> lstOBDLitemsModel = new List<OpeningBalanceLineItemModel>();
            bool isAdd = false;

            DateTime? oldOBDate = null;

            string _errors = CommonValidation.ValidateObject(TObject);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_errors);

            //For Deleted COA
            Dictionary<long, string> lstCOAs = _chartOfAccountService.GetAllChartofAccountsById(TObject.Details.Where(a => (a.RecordStatus == "Modified" || a.RecordStatus == "Added")).Select(c => c.COAId).ToList(), TObject.CompanyId);
            if (lstCOAs.Any())
            {
                string name = TObject.Details.Where(c => (c.RecordStatus == "Modified" || c.RecordStatus == "Added") && /*!lstCOAs.Values.Contains(c.AccountName) &&*/ !lstCOAs.Keys.Contains(c.COAId)).Select(c => c.AccountName).FirstOrDefault();
                if (name != null && name != string.Empty)
                    throw new Exception(name + " COA has been deleted, can't post");
            }

            List<long> CoaId = _chartOfAccountService.GetCOAIDsByName(new List<string> { COANameConstants.AccountsPayable, COANameConstants.OtherPayables }, TObject.CompanyId);

            #region DocNo Checking
            //for Doc No checking
            List<OpeningBalanceLineItemModel> listItems = TObject.Details.SelectMany(c => c.LineItems.Where(d => d != null)).ToList();
            string docNo = null;
            if (listItems.Any())
            {

                List<OpeningBalanceLineItemModel> LstCNTROR = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade receivables" || d.AccountName == "Other receivables" && d.RecordStatus != RecordStatus.Deleted).Select(x => x.COAId).Contains(c.COAId) && c.DocDebit < 0).ToList();

                //For CN Doc No
                if (LstCNTROR.Any())
                {
                    var allDocNo = LstCNTROR.GroupBy(c => new { DocNo = c.DocumentReference }).Select(a => new { DocNo = a.Key.DocNo, Count = a.Count() });
                    if (allDocNo.Any())
                        docNo = string.Join(",", allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).ToList());
                    if (allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).Any())
                        throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TR_OR_lines);
                    IDictionary<Guid, string> lstCNDocNo = _billService.GetAllCreditNoteDocNo(TObject.CompanyId);
                    if (lstCNDocNo.Any())
                    {
                        //List<string> demo = LstCNTROR.GroupBy(c => new { DocNo = c.DocumentReference, value = c.Id }).Where(s => lstCNDocNo.Keys.Contains(s.Key.DocNo) && !lstCNDocNo.Values.Contains(s.Key.value)).Select(s => s.Key.DocNo).ToList();
                        List<string> demo = lstCNDocNo.Where(c => !LstCNTROR.Select(d => d.Id).Contains(c.Key) && LstCNTROR.Select(d => d.DocumentReference).Contains(c.Value)).Select(c => c.Value).ToList();
                        if (demo.Any())
                            docNo = string.Join(",", demo);
                        if (demo.Count > 0)
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_CreditNote);
                    }
                }
                //For Invoice Doc No
                List<OpeningBalanceLineItemModel> LstTROR = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade receivables" || d.AccountName == "Other receivables" && d.RecordStatus != RecordStatus.Deleted).Select(x => x.COAId).Contains(c.COAId) && c.DocDebit > 0).ToList();
                if (LstTROR.Any())
                {
                    var allDocNo = LstTROR.GroupBy(c => new { DocNo = c.DocumentReference }).Select(a => new { DocNo = a.Key.DocNo, Count = a.Count() });
                    if (allDocNo.Any())
                        docNo = string.Join(",", allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).ToList());
                    if (allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).Any())
                        throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TR_OR_lines);

                    IDictionary<Guid, string> invoiceDocNo = _billService.GetAllInvoiceDocNos(TObject.CompanyId);
                    if (invoiceDocNo.Any())
                    {
                        //var demo = LstTROR.GroupBy(c => new { Id = c.Id, DocNo = c.DocumentReference, }).Where(s => !(invoiceDocNo.Keys.Contains(s.Key.Id)) && invoiceDocNo.Values.Contains(s.Key.DocNo)).Select(s => new { DocNo = s.Key.DocNo, Count = s.Count() }).ToList();
                        List<string> nod = invoiceDocNo.Where(c => !LstTROR.Select(d => d.Id).Contains(c.Key) && LstTROR.Select(d => d.DocumentReference).Contains(c.Value)).Select(c => c.Value).ToList();
                        if (nod.Any())
                            docNo = string.Join(",", nod);
                        if (nod.Count > 0)
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_Invoice);
                    }
                }
                //For Credit Memo
                #region OLD_CM_Doc_No_Check

                //List<OpeningBalanceLineItemModel> LstCMTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsPayable || d.AccountName == COANameConstants.Other_payables && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId) && c.DocCredit < 0).ToList();
                //if (LstCMTPOP.Any())
                //{
                //    var allDocNo = LstCMTPOP.GroupBy(c => new { DocNo = c.DocumentReference }).Select(a => new { DocNo = a.Key.DocNo, Count = a.Count() });
                //    if (allDocNo.Any())
                //        docNo = string.Join(",", allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).ToList());
                //    if (allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).Any())
                //        throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TP_OP_lines);

                //    IDictionary<Guid, string> lstCreditMemo = _billService.GetAllCreditMemoDocNo(TObject.CompanyId);
                //    if (lstCreditMemo.Any())
                //    {
                //        List<string> demo = lstCreditMemo.Where(c => !LstCMTPOP.Select(d => d.Id).Contains(c.Key) && LstCMTPOP.Select(d => d.DocumentReference).Contains(c.Value)).Select(c => c.Value).ToList();
                //        if (demo.Any())
                //            docNo = string.Join(",", demo);
                //        if (demo.Count > 0)
                //            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_CreditMemo);
                //    }
                //}

                #endregion
                List<OpeningBalanceLineItemModel> LstCMTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsPayable || d.AccountName == COANameConstants.Other_payables && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId) && c.DocCredit < 0).ToList();
                if (LstCMTPOP.Any())
                {

                    var lstOPTP = LstCMTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).Select(c => new { c.Key.DocNo, Count = c.Count(), c.Key.Entity }).ToList();
                    if (lstOPTP.Any())
                        docNo = string.Join(",", lstOPTP.Where(c => c.Count > 1).Select(c => c.DocNo));
                    if (LstCMTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).ToList().Any(d => d.Count() > 1))
                        throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TP_OP_lines);
                    List<CreditMemo> lstCreditMemos = _billService.GetlistOfCreditMemoDocNos(TObject.CompanyId);
                    if (lstCreditMemos.Any())
                    {
                        List<string> lstCMDocs = lstCreditMemos.Where(c => !LstCMTPOP.Select(d => d.Id).Contains(c.Id) && LstCMTPOP.Select(d => d.DocumentReference).Contains(c.DocNo) && LstCMTPOP.Select(d => d.EntityId).Contains(c.EntityId)).Select(c => c.DocNo).ToList();
                        if (lstCMDocs.Any())
                            docNo = string.Join(",", lstCMDocs);
                        if (lstCMDocs.Count > 0)
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_CreditMemo);
                    }

                }
                //For Bill
                List<OpeningBalanceLineItemModel> LstTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsPayable || d.AccountName == COANameConstants.Other_payables && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId) && c.DocCredit > 0).ToList();
                if (LstTPOP.Any())
                {
                    var lstOPTP = LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).Select(c => new { c.Key.DocNo, Count = c.Count(), c.Key.Entity }).ToList();
                    if (lstOPTP.Any())
                        docNo = string.Join(",", lstOPTP.Where(c => c.Count > 1).Select(c => c.DocNo));
                    if (LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).ToList().Any(d => d.Count() > 1) /*&& LstTPOP.Where(c => c.EntityId == item.Key).Select(c => c.EntityId).FirstOrDefault() != item.Key*/)
                        throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TP_OP_lines);

                    //IDictionary<string, Guid> billDocNo = _billService.GetAllBillDocNo(TObject.CompanyId);
                    List<Bill> lstBills = _billService.GetAllBillDocNo(TObject.CompanyId);
                    if (lstBills.Any())
                    {
                        //List<string> billDocs = LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, EntityId = c.EntityId, Id = c.Id }).Where(s => lstBills.Select(c => c.DocNo).ToList().Contains(s.Key.DocNo) && lstBills.Select(c => c.EntityId).ToList().Any(d => d == s.Key.EntityId) && !lstBills.Select(c => c.Id).ToList().Contains(s.Key.Id)).Select(c => c.Key.DocNo).ToList();
                        List<string> billDocs = lstBills.Where(c => !LstTPOP.Select(d => d.Id).Contains(c.Id) && LstTPOP.Select(d => d.DocumentReference).Contains(c.DocNo) && LstTPOP.Select(d => d.EntityId).Contains(c.EntityId)).Select(c => c.DocNo).ToList();
                        if (billDocs.Any())
                            docNo = string.Join(",", billDocs);
                        //if (LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, EntityIds = c.EntityId, Ids = c.Id }).Where(s => lstBills.Select(c => c.DocNo).ToList().Contains(s.Key.DocNo) && lstBills.Select(c => c.EntityId).ToList().Any(d => d == s.Key.EntityIds) && !lstBills.Select(c => c.Id).ToList().Contains(s.Key.Ids)).ToList().Count > 0)
                        if (billDocs.Count > 0)
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_Bill);
                    }
                }
            }
            #endregion


            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }

            if (TObject.SaveType != OpeningBalanceConstants.DraftType)
            {
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.Date, TObject.CompanyId))
                    throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.Date, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                        throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.Date, TObject.PeriodLockPassword, TObject.CompanyId))
                        throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
                //if (TObject.Details.Any())
                //{
                //    var basecredit = Math.Round(TObject.Details.Sum(c => c.BaseCredit).Value, 2);
                //    var basedebit = Math.Round(TObject.Details.Sum(c => c.BaseDebit).Value, 2);

                //    if (basecredit != basedebit)
                //        throw new Exception(OpeningBalnceValidations.BaseCredit_And_BaseDebit_Shoulb_Be_Equal);
                //}
            }

            OpeningBalance _openingBalance = _OpeningBalanceService.CheckOpeningBalanceById(TObject.Id);

            if (_openingBalance != null)
            {
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_openingBalance.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }

            //Prepare list Deleted Details & Detail Items
            //Call DeleteOBItems SP with these parameters
            #region Deletion_Records
            if (TObject.Id != null)
            {
                string detailIds = string.Empty;
                string lineItemsIds = string.Empty;
                List<Guid> lstOBDetailIds = TObject.Details.Where(c => c.RecordStatus == RecordStatus.Deleted).Select(c => c.Id).ToList();
                List<Guid> lstOBDetailLinsIds = TObject.Details.SelectMany(c => c.LineItems.Where(d => d != null && d.RecordStatus == RecordStatus.Deleted)).Select(d => d.Id).ToList();

                if (lstOBDetailLinsIds.Any() && TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    try
                    {
                        lstLineItem = _openingBalanceDetailLineItemService.GetListOfDeleteTPOPlineItem(lstOBDetailLinsIds, CoaId, TObject.ServiceCompanyId);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                if (lstOBDetailIds.Any())
                    detailIds = string.Join(",", lstOBDetailIds);
                if (lstOBDetailLinsIds.Any())
                    lineItemsIds = string.Join(",", lstOBDetailLinsIds);
                if (lstOBDetailIds.Any() || lstOBDetailLinsIds.Any())
                {
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_DeleteOBItems", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId);
                    cmd.Parameters.AddWithValue("@OpeningBalanceId", TObject.Id.ToString());
                    cmd.Parameters.AddWithValue("@OBDIds", detailIds);
                    cmd.Parameters.AddWithValue("@OBDLIIds", lineItemsIds);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            #endregion Deletion_Records

            //Remove Onpening Balance Postings//Commented on 07/04/2020 for concurrency issue
            //if (TObject.SaveType == OpeningBalanceConstants.SaveType && TObject.Id != null)
            //{
            //    JournalSaveModel tObject = new JournalSaveModel();
            //    tObject.Id = TObject.Id;
            //    tObject.CompanyId = TObject.CompanyId;
            //    deleteJVPostInvoce(tObject);
            //}
            //List<Guid> lineItemGuids = null;

            if (_openingBalance != null)
            {
                if (TObject.Details.Where(c => c.LineItems != null).SelectMany(a => a.LineItems).Where(x => x.Date > TObject.Date).Any())
                {
                    throw new Exception(OpeningBalanceLoggingValidation.Opening_Balance_Date_cannot_be_less_than_Opening_Balance_Line_Item_Date);
                }
                oldOBDate = _openingBalance.Date;
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_openingBalance_Is_Not_Equal_To_Null);
                FillOpeningBalances(_openingBalance, TObject);
                _openingBalance.IsTemporary = false;
                UpdateOBDetail(TObject, newEntityIds, oldEntityIds, lstObLineItem, CoaId, lstOBDLitemsModel);
                _openingBalance.IsEditable = TObject.IsEditable;
                _openingBalance.SystemRefNo = TObject.SystemRefNo;
                _openingBalance.ModifiedDate = DateTime.UtcNow;
                _openingBalance.ModifiedBy = TObject.UserModified;
                _openingBalance.ObjectState = ObjectState.Modified;
                _OpeningBalanceService.Update(_openingBalance);
                isAdd = false;
            }
            else
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_openingBalance_Else_Block);
                oldOBDate = TObject.Date;
                _openingBalance = new OpeningBalance();
                _openingBalance.Id = Guid.NewGuid();
                FillOpeningBalances(_openingBalance, TObject);
                _openingBalance.IsTemporary = false;
                isAdd = true;
                if (TObject.Details.Any())
                {
                    int Recoder = 0;
                    int Recorder1 = 0;
                    OpeningBalanceDetail openingBalanceDetail = null;
                    foreach (var details in TObject.Details)
                    {
                        openingBalanceDetail = new OpeningBalanceDetail();
                        openingBalanceDetail.Recorder = ++Recoder;//) + 1;
                        //Recoder = openingBalanceDetail.Recorder;
                        openingBalanceDetail.Id = Guid.NewGuid();
                        FillDetails(openingBalanceDetail, details, TObject);
                        openingBalanceDetail.OpeningBalanceId = _openingBalance.Id;
                        openingBalanceDetail.ObjectState = ObjectState.Added;
                        _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                        if (details.LineItems != null && details.LineItems.Any())
                        {
                            foreach (var lineItems in details.LineItems)
                            {
                                if (lineItems.RecordStatus != RecordStatus.Deleted)
                                {
                                    if (lineItems.EntityId != null)
                                    {
                                        newEntityIds.Add(lineItems.EntityId);//for new Entity
                                    }
                                    OpeningBalanceDetailLineItem lineItem = new OpeningBalanceDetailLineItem();
                                    lineItem.Id = lineItems.Id;
                                    lineItem.OpeningBalanceDetailId = openingBalanceDetail.Id;
                                    lineItem.EntityId = lineItems.EntityId;
                                    lineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                                    FillDetailLineItemVmToEntity(lineItems, lineItem);
                                    lineItem.Recorder = ++Recorder1;
                                    //Recorder1 = lineItem.Recorder;
                                    lineItem.UserCreated = lineItems.UserCreated;
                                    lineItem.CreatedDate = DateTime.UtcNow;
                                    lineItem.IsEditable = true;
                                    lineItem.IsProcressed = false;
                                    lineItem.ObjectState = ObjectState.Added;
                                    //lineItemGuids.Add(lineItems.Id);
                                    _openingBalanceDetailLineItemService.Insert(lineItem);

                                }
                            }
                        }
                    }
                }
                _openingBalance.CreatedDate = DateTime.UtcNow;
                _openingBalance.UserCreated = TObject.UserCreated;
                _openingBalance.IsEditable = true;
                _openingBalance.ObjectState = ObjectState.Added;
                _OpeningBalanceService.Insert(_openingBalance);
            }
            if (TObject.SaveType == OpeningBalanceConstants.Save && (TObject.SystemRefNo == null || TObject.SystemRefNo == string.Empty))
            {
                Company company = _companyService.GetById(TObject.CompanyId);
                _openingBalance.SystemRefNo = GenerateAutoNumberForType(company.Id, OpeningBalanceConstants.DocType, company.ShortName);
            }
            else
                _openingBalance.SystemRefNo = TObject.SystemRefNo;
            try
            {
                serviceCompanyName = TObject.Date.ToString("dd/MM/yyyy");
                _openingBalancesModuleUnitOfWork.SaveChanges();

                //Call SaveOBInvoice SP

                #region OB_Invoice_Creation
                string detailsIds = string.Empty;
                bool? isMultiCurrency = _multiCurrencySettingService.GetMultiCurrency(_openingBalance.CompanyId);
                bool? isGstSettings = _gstSettingService.GetgstDetail(_openingBalance.ServiceCompanyId);
                //List<Guid> lstInvoiceDetailIDs = TObject.Details.Where(c => (c.RecordStatus == RecordStatus.Added || c.RecordStatus == RecordStatus.Modified || c.RecordStatus == null) && (c.AccountName == COANameConstants.AccountsReceivables || c.AccountName == COANameConstants.OtherReceivables)).Select(c => c.Id).ToList();

                //List<Guid> lstInvoiceDetailIDs = TObject.Details.Where(c => c.AccountName == COANameConstants.AccountsReceivables || c.AccountName == COANameConstants.OtherReceivables).SelectMany(c => c.LineItems.Where(d => d != null && d.IsProcressed == false/*d.RecordStatus == RecordStatus.Added || d.RecordStatus == RecordStatus.Modified*/)).Select(d => d.Id).ToList();

                //List<long> Coaids = TObject.Details.Where(a => a.AccountName == COANameConstants.AccountsReceivables || a.AccountName == COANameConstants.OtherReceivables).Select(a => a.COAId).ToList();

                List<long> Coaids = _chartOfAccountService.GetCOAIDsByName(new List<string> { COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables }, TObject.CompanyId);


                //List<OpeningBalanceDetail> lstDetail = _OpeningBalanceDetailService.GetAllOBDetailAndLineItmsById((_openingBalance.OpeningBalanceDetails.Where(c => Coaids.Contains(c.COAId)).Select(c => c.Id)).ToList());
                // List<Guid> lstInvoiceDetailIDs = lstDetail.Any() ? lstDetail.SelectMany(a => a.OpeningBalanceDetailLineItems.Where(d => d.IsProcressed == false).Select(c => c.Id)).ToList() : new List<Guid>();
                List<Guid> lstInvoiceDetailIDs = _openingBalanceDetailLineItemService.GetListOfLineItemId(Coaids, TObject.ServiceCompanyId);

                if (_openingBalance != null && lstInvoiceDetailIDs.Any() && TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    detailsIds = string.Join(",", lstInvoiceDetailIDs);
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_SaveOBInvoice", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyId", _openingBalance.CompanyId.ToString());
                    cmd.Parameters.AddWithValue("@openingBalanceId", _openingBalance.Id.ToString());
                    cmd.Parameters.AddWithValue("@lineItemIds", detailsIds);
                    cmd.Parameters.AddWithValue("@isMultiCurrency", isMultiCurrency);
                    cmd.Parameters.AddWithValue("@isGstActivated", isGstSettings);
                    int res = cmd.ExecuteNonQuery();
                    con.Close();
                }
                #endregion OB_Invoice_Creation

                //Call SaveOBBill SP

                #region OB_Bill_Creation
                //List<Guid> lstBillDetailIDs = TObject.Details.Where(c => c.AccountName == COANameConstants.AccountsPayable || c.AccountName == COANameConstants.OtherPayables).SelectMany(c => c.LineItems.Where(d => d != null && d.IsProcressed==false/*d.RecordStatus == RecordStatus.Added || d.RecordStatus == RecordStatus.Modified*/)).Select(d => d.Id).ToList();

                //List<Guid> lstBillDetailIDs = _OpeningBalanceDetailService.GetAllOBDetailAndLineItmsById(TObject.Details.Where(c => c.AccountName == COANameConstants.AccountsPayable || c.AccountName == COANameConstants.OtherPayables).SelectMany(a=>a.LineItems.Where(d=>d.IsProcressed==false).Select(c => c.Id)).ToList());

                //List<OpeningBalanceDetail> lstDetails = _OpeningBalanceDetailService.GetAllOBDetailAndLineItmsById((TObject.Details.Where(c => TObject.Details.Where(a => a.AccountName == COANameConstants.AccountsPayable || a.AccountName == COANameConstants.OtherPayables).Select(a => a.COAId).ToList().Contains(c.COAId)).Select(c => c.Id)).ToList());
                //List<Guid> lstBillDetailIDs = lstDetails.Any() ? lstDetails.SelectMany(a => a.OpeningBalanceDetailLineItems.Where(d => d.IsProcressed == false).Select(c => c.Id)).ToList() : new List<Guid>();

                //List<long> Coaid1s = TObject.Details.Where(a => a.AccountName == COANameConstants.AccountsPayable || a.AccountName == COANameConstants.OtherPayables).Select(a => a.COAId).ToList();


                List<Guid> lstBillDetailIDs = _openingBalanceDetailLineItemService.GetListOfLineItemId(CoaId, TObject.ServiceCompanyId);

                if (_openingBalance != null && lstBillDetailIDs.Any() && TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    detailsIds = string.Empty;
                    detailsIds = string.Join(",", lstBillDetailIDs);
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_SaveOBBillAndCreditMemo", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyId", _openingBalance.CompanyId);
                    cmd.Parameters.AddWithValue("@openingBalanceId", _openingBalance.Id.ToString());
                    cmd.Parameters.AddWithValue("@lineItemIds", detailsIds);
                    cmd.Parameters.AddWithValue("@isMultiCurrency", isMultiCurrency);
                    cmd.Parameters.AddWithValue("@isGstActivated", isGstSettings);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                #endregion OB_Bill_Creation

                #region Posting
                string Description = null;
                if (TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    List<long> coaId = TObject.Details.Where(c => c.AccountName == COANameConstants.AccountsPayable || c.AccountName == COANameConstants.OtherPayables || c.AccountName == COANameConstants.AccountsReceivables || c.AccountName == COANameConstants.OtherReceivables).Select(d => d.COAId).ToList();
                    List<ChartOfAccount> lstCOA = _chartOfAccountService.GetAllCOAById(TObject.CompanyId, coaId);
                    JVModel jvm = new JVModel();
                    _openingBalance.OpeningBalanceDetails = _OpeningBalanceDetailService.GetServiceCompanyOpeningBalance(_openingBalance.Id);
                    FillJournal(jvm, _openingBalance, false, lstCOA, TObject.ServiceCompanyName);
                    jvm.Id = _openingBalance.Id;
                    jvm.DocumentDescription = Description = DocTypeConstants.OpeningBalance + " - " + serviceCompanyName;
                    SaveOBPostings(jvm);
                }

                #endregion

                try
                {
                    if (TObject.SaveType == OpeningBalanceConstants.Save)
                    {
                        if (newEntityIds.Any())
                            newEntityIds.Remove(null);
                        if (oldEntityIds.Any())
                            oldEntityIds.Remove(null);
                        List<Guid?> lstEntityIds = new List<Guid?>();
                        if (newEntityIds.Any() || oldEntityIds.Any())
                        {
                            lstEntityIds.AddRange(newEntityIds);
                            lstEntityIds.AddRange(oldEntityIds);
                        }
                        lstEntityIds.AddRange(TObject.Details.SelectMany(c => c.LineItems.Where(d => d != null && d.RecordStatus == RecordStatus.Deleted && d.EntityId != null)).Select(d => d.EntityId).ToList());
                        //ls
                        //List<OpeningBalanceDetail> detail = _OpeningBalanceDetailService.GetOpeningBalanceDetail(TObject.Id);
                        newEntityIds = _openingBalanceDetailLineItemService.ListOfEntityIds(Coaids, TObject.ServiceCompanyId);
                        if (newEntityIds.Any())
                        {
                            //newEntityIds = detail.SelectMany(a => a.OpeningBalanceDetailLineItems.Where(x => x.EntityId != null).Select(c => c.EntityId)).ToList();

                            lstEntityIds.AddRange(newEntityIds);
                            //newEntityIds.Any();
                            //lstEntityIds.AddRange(newEntityIds);
                        }
                        //BackgroundJob.Enqueue(() => new HangFireService().UpdateCustBalance());
                        string lstOfEids = string.Join(",", lstEntityIds.Distinct().ToList());
                        if (lstOfEids != null && lstOfEids != string.Empty)
                            //BackgroundJob.Enqueue(() => new HangFireService().UpdateCustBalance(TObject.CompanyId, lstOfEids, ConnectionString));
                            new HangFireService().UpdateCustBalance(TObject.CompanyId, lstOfEids, ConnectionString);
                        //new HangFireService().UpdateCustBalance(TObject.CompanyId, lstOfEids, ConnectionString);

                        //if (isAdd)
                        //{
                        List<OpeningBalanceDetailLineItem> obdls = _openingBalanceDetailLineItemService.GetListOfTPOPLineItemId(lstBillDetailIDs, TObject.ServiceCompanyId);
                        List<OpeningBalanceDetailLineItem> detailLineItem = new List<OpeningBalanceDetailLineItem>();
                        if (lstOBDLitemsModel.Any())
                        {
                            lstOBDLitemsModel = lstOBDLitemsModel.Where(a => CoaId.Contains(a.COAId)).ToList();
                        }

                        lstEntity = _beanEntityService.GetListOfEntity(TObject.CompanyId, lstEntityIds.Distinct().ToList());

                        //new HangFireService().SaveScreenFolders(obdls, true, TObject.CompanyId, lstOBDLitemsModel);

                        new HangFireService().SaveScreenFoldersNew(lstEntity, obdls, true, TObject.CompanyId, lstOBDLitemsModel);

                        if (lstLineItem != null)
                        {
                            //BackgroundJob.Enqueue(() => new HangFireService().DeleteOBDeleteLineItem(lstLineItem, TObject.CompanyId));
                            new HangFireService().DeleteOBDeleteLineItemNew(lstEntity, TObject.CompanyId, lstOBDLitemsModel);
                        }
                    }

                    #region Update_Bean_OB_Bill_CM_posting_Date
                    try
                    {
                        if (isAdd == false && oldOBDate != TObject.Date)
                        {
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand("Bean_Update_Posting_date", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId);
                                cmd.Parameters.AddWithValue("@OpeningBalanceId", _openingBalance.Id);
                                cmd.Parameters.AddWithValue("@ServiceCompanyId", _openingBalance.ServiceCompanyId);
                                int res = cmd.ExecuteNonQuery();
                                if (con.State != ConnectionState.Closed)
                                    con.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }


                    #endregion


                    #region Documentary History
                    try
                    {
                        List<AppaWorld.Bean.DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_openingBalance.Id, _openingBalance.CompanyId, _openingBalance.Id, DocTypeConstants.JournalVocher, DocTypeConstants.OpeningBalance, _openingBalance.SaveType == "Save" ? "Posted" : _openingBalance.SaveType, string.Empty, 0, 0, 0, _openingBalance.ModifiedBy != null ? _openingBalance.ModifiedBy : _openingBalance.UserCreated, Description, _openingBalance.Date, 0, 0);
                        if (lstdocumet.Any())
                            AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    #endregion Documentary History
                }
                catch (Exception ex)
                {

                }

            }
            catch (DbEntityValidationException ex)
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Catch_Block);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
            return _openingBalance;

        }

        public OpeningBalance SaveOpeningBalanceNewRecent(OpeningBalanceModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, "ObjectSave", AdditionalInfo);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entred_Into_SaveOpeningBalance_Method);
            OpeningBalanceModel openingBalanceModel = new OpeningBalanceModel();
            SqlConnection con = null;
            SqlCommand cmd = null;

            List<Guid?> newEntityIds = new List<Guid?>();
            List<Guid?> oldEntityIds = new List<Guid?>();
            Dictionary<Guid, string> lstLineItem = null;
            Dictionary<Guid, string> lstEntity = null;
            List<OpeningBalanceDetailLineItem> lstObLineItem = new List<OpeningBalanceDetailLineItem>();
            List<OpeningBalanceLineItemModel> lstOBDLitemsModel = new List<OpeningBalanceLineItemModel>();
            bool isAdd = false;

            DateTime? oldOBDate = null;

            string _errors = CommonValidation.ValidateObject(TObject);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_errors);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            //For Deleted COA
            Dictionary<long, string> lstCOAs = _chartOfAccountService.GetAllChartofAccountsById(TObject.Details.Where(a => (a.RecordStatus == "Modified" || a.RecordStatus == "Added")).Select(c => c.COAId).ToList(), TObject.CompanyId);
            if (lstCOAs.Any())
            {
                string name = TObject.Details.Where(c => (c.RecordStatus == "Modified" || c.RecordStatus == "Added") && /*!lstCOAs.Values.Contains(c.AccountName) &&*/ !lstCOAs.Keys.Contains(c.COAId)).Select(c => c.AccountName).FirstOrDefault();
                if (name != null && name != string.Empty)
                    throw new Exception(name + " COA has been deleted, can't post");
            }

            List<long> CoaId = _chartOfAccountService.GetCOAIDsByName(new List<string> { COANameConstants.AccountsPayable, COANameConstants.OtherPayables }, TObject.CompanyId);
            #region FOR_TR_LINE_ITEMS
            //long TRCOAId = TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsReceivables).Select(c => c.COAId).FirstOrDefault();
            //List<OpeningBalanceDetailLineItem> lstOBDetail2 = new List<OpeningBalanceDetailLineItem>();
            //lstOBDetail2 = _openingBalanceDetailLineItemService.GetLineItemsForCOA(TRCOAId, TObject.ServiceCompanyId, TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsReceivables).Select(c => c.DocCurrency).FirstOrDefault());
            #endregion FOR_TR_LINE_ITEMS


            

            if (TObject.SaveType != OpeningBalanceConstants.DraftType)
            {
                if (!_financialSettingService.ValidateYearEndLockDate(TObject.Date, TObject.CompanyId))
                    throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                if (!_financialSettingService.ValidateFinancialOpenPeriod(TObject.Date, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                        throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(TObject.Date, TObject.PeriodLockPassword, TObject.CompanyId))
                        throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }

            }

            OpeningBalance _openingBalance = _OpeningBalanceService.CheckOpeningBalanceById(TObject.Id);

            //if (_openingBalance != null)
            //{
            //    //Data concurrency verify
            //    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_openingBalance.Version, x => x.ToString("X2")));
            //    if (!timeStamp.Equals(TObject.Version))
            //        throw new Exception(CommonConstant.Document_has_been_modified_outside);
            //}

            //Prepare list Deleted Details & Detail Items
            //Call DeleteOBItems SP with these parameters

            #region Deletion_Records
            if (TObject.Id != null)
            {
                string detailIds = string.Empty;
                string lineItemsIds = string.Empty;
                List<Guid> lstOBDetailIds = TObject.Details.Where(c => c.RecordStatus == RecordStatus.Deleted).Select(c => c.Id).ToList();

                if (lstOBDetailIds.Any())
                    detailIds = string.Join(",", lstOBDetailIds);

                if (lstOBDetailIds.Any())
                {
                    con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_DeleteOBItems", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId);
                    cmd.Parameters.AddWithValue("@OpeningBalanceId", TObject.Id.ToString());
                    cmd.Parameters.AddWithValue("@OBDIds", detailIds);
                    cmd.Parameters.AddWithValue("@OBDLIIds", string.Empty);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            #endregion Deletion_Records

            if (_openingBalance != null)
            {
                //if (TObject.Details.Where(c => c.LineItems != null).SelectMany(a => a.LineItems).Where(x => x.Date > TObject.Date).Any())
                //{
                //    throw new Exception(OpeningBalanceLoggingValidation.Opening_Balance_Date_cannot_be_less_than_Opening_Balance_Line_Item_Date);
                //}
                oldOBDate = _openingBalance.Date;
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Checking_openingBalance_Is_Not_Equal_To_Null);
                FillOpeningBalances(_openingBalance, TObject);
                _openingBalance.IsTemporary = false;
                UpdateOBDetailNew(TObject, newEntityIds, oldEntityIds, lstObLineItem, CoaId, lstOBDLitemsModel);
                _openingBalance.IsEditable = TObject.IsEditable;
                _openingBalance.SystemRefNo = TObject.SystemRefNo;
                _openingBalance.ModifiedDate = DateTime.UtcNow;
                _openingBalance.ModifiedBy = TObject.UserModified;
                _openingBalance.ObjectState = ObjectState.Modified;
                _OpeningBalanceService.Update(_openingBalance);
                isAdd = false;
            }
            else
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_openingBalance_Else_Block);
                oldOBDate = TObject.Date;
                _openingBalance = new OpeningBalance();
                _openingBalance.Id = Guid.NewGuid();
                FillOpeningBalances(_openingBalance, TObject);
                _openingBalance.IsTemporary = false;
                isAdd = true;
                if (TObject.Details.Any())
                {
                    int Recoder = 0;
                    OpeningBalanceDetail openingBalanceDetail = null;
                    foreach (var details in TObject.Details)
                    {
                        openingBalanceDetail = new OpeningBalanceDetail();
                        openingBalanceDetail.Recorder = ++Recoder;//) + 1;
                        //Recoder = openingBalanceDetail.Recorder;
                        openingBalanceDetail.Id = Guid.NewGuid();
                        FillDetails(openingBalanceDetail, details, TObject);
                        openingBalanceDetail.OpeningBalanceId = _openingBalance.Id;
                        openingBalanceDetail.ObjectState = ObjectState.Added;
                        _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                    }
                }
                _openingBalance.CreatedDate = DateTime.UtcNow;
                _openingBalance.UserCreated = TObject.UserCreated;
                _openingBalance.IsEditable = true;
                _openingBalance.ObjectState = ObjectState.Added;
                _OpeningBalanceService.Insert(_openingBalance);
            }
            if (TObject.SaveType == OpeningBalanceConstants.Save && (TObject.SystemRefNo == null || TObject.SystemRefNo == string.Empty))
            {
                Company company = _companyService.GetById(TObject.CompanyId);
                _openingBalance.SystemRefNo = GenerateAutoNumberForType(company.Id, OpeningBalanceConstants.DocType, company.ShortName);
            }
            else
                _openingBalance.SystemRefNo = TObject.SystemRefNo;
            try
            {
                serviceCompanyName = TObject.Date.ToString("dd/MM/yyyy");
                _openingBalancesModuleUnitOfWork.SaveChanges();

                //Call SaveOBInvoice SP

                #region OB_Invoice_Creation
                string detailsIds = string.Empty;
                bool? isMultiCurrency = _multiCurrencySettingService.GetMultiCurrency(_openingBalance.CompanyId);
                bool? isGstSettings = _gstSettingService.GetgstDetail(_openingBalance.ServiceCompanyId);
                List<long> Coaids = _chartOfAccountService.GetCOAIDsByName(new List<string> { COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables }, TObject.CompanyId);
                List<Guid> lstInvoiceDetailIDs = _openingBalanceDetailLineItemService.GetListOfLineItemId1(Coaids, TObject.ServiceCompanyId,TObject.CompanyId);

                if (_openingBalance != null && lstInvoiceDetailIDs.Any() && TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    OBDetailModel oBDetailModel = new OBDetailModel();
                    detailsIds = string.Join(",", lstInvoiceDetailIDs);
                    oBDetailModel.CompanyId = _openingBalance.CompanyId;
                    oBDetailModel.OpeningBalanceId = _openingBalance.Id;
                    oBDetailModel.DocType = "Invoice";
                    oBDetailModel.ServiceCompanyId = _openingBalance.ServiceCompanyId;
                    oBDetailModel.IsMultiCurrency = isMultiCurrency.Value;
                    oBDetailModel.IsGSTSettings = isGstSettings.Value;

                    UploadMessgeInQueue(oBDetailModel);
                    //con = new SqlConnection(ConnectionString);
                    //if (con.State != ConnectionState.Open)
                    //    con.Open();
                    //cmd = new SqlCommand("Bean_SaveOBInvoice", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@companyId", _openingBalance.CompanyId.ToString());
                    //cmd.Parameters.AddWithValue("@openingBalanceId", _openingBalance.Id.ToString());
                    //cmd.Parameters.AddWithValue("@lineItemIds", detailsIds);
                    //cmd.Parameters.AddWithValue("@isMultiCurrency", isMultiCurrency);
                    //cmd.Parameters.AddWithValue("@isGstActivated", isGstSettings);
                    //int res = cmd.ExecuteNonQuery();
                    //con.Close();
                }
                #endregion OB_Invoice_Creation

                //Call SaveOBBill SP
                #region OB_Bill_Creation

                List<Guid> lstBillDetailIDs = _openingBalanceDetailLineItemService.GetListOfLineItemId1(CoaId, TObject.ServiceCompanyId,TObject.CompanyId);

                if (_openingBalance != null && lstBillDetailIDs.Any() && TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    detailsIds = string.Empty;
                    detailsIds = string.Join(",", lstBillDetailIDs);

                    OBDetailModel oBDetailModel1 = new OBDetailModel();
                    oBDetailModel1.CompanyId = _openingBalance.CompanyId;
                    oBDetailModel1.OpeningBalanceId = _openingBalance.Id;
                    oBDetailModel1.DocType = "Bill";
                    oBDetailModel1.ServiceCompanyId = _openingBalance.ServiceCompanyId;
                    oBDetailModel1.IsMultiCurrency = isMultiCurrency.Value;
                    oBDetailModel1.IsGSTSettings = isGstSettings.Value;
                    UploadMessgeInQueue(oBDetailModel1);
                    //con = new SqlConnection(ConnectionString);
                    //if (con.State != ConnectionState.Open)
                    //    con.Open();
                    //cmd = new SqlCommand("Bean_SaveOBBillAndCreditMemo", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@companyId", _openingBalance.CompanyId);
                    //cmd.Parameters.AddWithValue("@openingBalanceId", _openingBalance.Id.ToString());
                    //cmd.Parameters.AddWithValue("@lineItemIds", detailsIds);
                    //cmd.Parameters.AddWithValue("@isMultiCurrency", isMultiCurrency);
                    //cmd.Parameters.AddWithValue("@isGstActivated", isGstSettings);
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                }
                #endregion OB_Bill_Creation

                #region Posting
                string Description = null;
                if (TObject.SaveType == OpeningBalanceConstants.Save)
                {
                    List<long> coaId = TObject.Details.Where(c => c.AccountName == COANameConstants.AccountsPayable || c.AccountName == COANameConstants.OtherPayables || c.AccountName == COANameConstants.AccountsReceivables || c.AccountName == COANameConstants.OtherReceivables).Select(d => d.COAId).ToList();
                    List<ChartOfAccount> lstCOA = _chartOfAccountService.GetAllCOAById(TObject.CompanyId, coaId);
                    JVModel jvm = new JVModel();
                    _openingBalance.OpeningBalanceDetails = _OpeningBalanceDetailService.GetServiceCompanyOpeningBalance(_openingBalance.Id);
                    FillJournal(jvm, _openingBalance, false, lstCOA, TObject.ServiceCompanyName);
                    jvm.Id = _openingBalance.Id;
                    jvm.DocumentDescription = Description = DocTypeConstants.OpeningBalance + " - " + serviceCompanyName;
                    SaveOBPostings(jvm);
                }

                #endregion

                try
                {
                    if (TObject.SaveType == OpeningBalanceConstants.Save)
                    {
                        if (newEntityIds.Any())
                            newEntityIds.Remove(null);
                        if (oldEntityIds.Any())
                            oldEntityIds.Remove(null);
                        List<Guid?> lstEntityIds = new List<Guid?>();
                        if (newEntityIds.Any() || oldEntityIds.Any())
                        {
                            lstEntityIds.AddRange(newEntityIds);
                            lstEntityIds.AddRange(oldEntityIds);
                        }
                        lstEntityIds.AddRange(TObject.Details.SelectMany(c => c.LineItems.Where(d => d != null && d.RecordStatus == RecordStatus.Deleted && d.EntityId != null)).Select(d => d.EntityId).ToList());
                        //ls
                        //List<OpeningBalanceDetail> detail = _OpeningBalanceDetailService.GetOpeningBalanceDetail(TObject.Id);
                        newEntityIds = _openingBalanceDetailLineItemService.ListOfEntityIds(Coaids, TObject.ServiceCompanyId);
                        if (newEntityIds.Any())
                        {
                            //newEntityIds = detail.SelectMany(a => a.OpeningBalanceDetailLineItems.Where(x => x.EntityId != null).Select(c => c.EntityId)).ToList();

                            lstEntityIds.AddRange(newEntityIds);
                            //newEntityIds.Any();
                            //lstEntityIds.AddRange(newEntityIds);
                        }
                        //BackgroundJob.Enqueue(() => new HangFireService().UpdateCustBalance());
                        string lstOfEids = string.Join(",", lstEntityIds.Distinct().ToList());
                        if (lstOfEids != null && lstOfEids != string.Empty)
                            //BackgroundJob.Enqueue(() => new HangFireService().UpdateCustBalance(TObject.CompanyId, lstOfEids, ConnectionString));
                            new HangFireService().UpdateCustBalance(TObject.CompanyId, lstOfEids, ConnectionString);
                        //new HangFireService().UpdateCustBalance(TObject.CompanyId, lstOfEids, ConnectionString);

                        //if (isAdd)
                        //{
                        List<OpeningBalanceDetailLineItem> obdls = _openingBalanceDetailLineItemService.GetListOfTPOPLineItemId(lstBillDetailIDs, TObject.ServiceCompanyId);
                        List<OpeningBalanceDetailLineItem> detailLineItem = new List<OpeningBalanceDetailLineItem>();
                        if (lstOBDLitemsModel.Any())
                        {
                            lstOBDLitemsModel = lstOBDLitemsModel.Where(a => CoaId.Contains(a.COAId)).ToList();
                        }

                        lstEntity = _beanEntityService.GetListOfEntity(TObject.CompanyId, lstEntityIds.Distinct().ToList());

                        new HangFireService().SaveScreenFoldersNewRecent(lstEntity, obdls, true, TObject.CompanyId);

                        if (obdls.Any())
                        {
                            //BackgroundJob.Enqueue(() => new HangFireService().DeleteOBDeleteLineItem(lstLineItem, TObject.CompanyId));
                            new HangFireService().DeleteOBDeleteLineItemNewRecent(lstEntity, TObject.CompanyId, obdls);
                        }
                    }

                    #region Update_Bean_OB_Bill_CM_posting_Date
                    try
                    {
                        if (isAdd == false && oldOBDate != TObject.Date)
                        {
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand("Bean_Update_Posting_date", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CompanyId", TObject.CompanyId);
                                cmd.Parameters.AddWithValue("@OpeningBalanceId", _openingBalance.Id);
                                cmd.Parameters.AddWithValue("@ServiceCompanyId", _openingBalance.ServiceCompanyId);
                                int res = cmd.ExecuteNonQuery();
                                if (con.State != ConnectionState.Closed)
                                    con.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }


                    #endregion


                    #region Documentary History
                    try
                    {
                        List<AppaWorld.Bean.DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_openingBalance.Id, _openingBalance.CompanyId, _openingBalance.Id, DocTypeConstants.JournalVocher, DocTypeConstants.OpeningBalance, _openingBalance.SaveType == "Save" ? "Posted" : _openingBalance.SaveType, string.Empty, 0, 0, 0, _openingBalance.ModifiedBy != null ? _openingBalance.ModifiedBy : _openingBalance.UserCreated, Description, _openingBalance.Date, 0, 0);
                        if (lstdocumet.Any())
                            AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                    }
                    #endregion Documentary History
                }
                catch (Exception ex)
                {

                }

            }
            catch (DbEntityValidationException ex)
            {
                LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Catch_Block);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
            return _openingBalance;

        }
        private static void UploadMessgeInQueue(OBDetailModel transactionModel)
        {
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Opening_Balance_web_job_calling);
            // get the storage account key (Log into Azure – Storage Accounts – Access Keys)
            var queueconnectionstring = ConfigurationManager.AppSettings[OpeningBalanceConstants.WebJobStorageAccConnectionString];
            // converting the transaction model into string 
            string obObject = JsonConvert.SerializeObject(transactionModel);
            // connecting to the Azure
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(queueconnectionstring);
            // to execute the queue service
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            // Queueing the job
            CloudQueue queue = queueClient.GetQueueReference(ConfigurationManager.AppSettings[OpeningBalanceConstants.WebJobQueueName]);
            // creating the queue if not exist
            queue.CreateIfNotExists();
            CloudQueueMessage message = new CloudQueueMessage(obObject);
            queue.AddMessage(message);
            LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Opening_Balance_web_job_calling_completed);
        }
        public OpeningBalanceDetailModel SaveOBDetailLineItems(OpeningBalanceDetailModel openingBalanceDetailModel, long serviceEntityId, string ConnectionString, long companyId, Guid OBId)
        {
            try
            {
                List<OpeningBalanceLineItemModel> TObject = openingBalanceDetailModel.LineItems;
                SqlConnection con = null;
                SqlCommand cmd = null;
                //SqlDataReader dr = null;
                string lineItemsIds = string.Empty;
                List<OpeningBalanceDetailLineItem> existObDetails = null;
                Guid OBDetailId = Guid.Empty;
                List<Guid> lstDetailLineIds = null;
                List<Guid> lineItemIds = TObject.Where(d => d.RecordStatus == RecordStatus.Modified).Select(a => a.Id).ToList();
                lstDetailLineIds = TObject.Where(d => d.RecordStatus == RecordStatus.Deleted).Select(a => a.Id).ToList();
                if (lineItemIds != null && lineItemIds.Any())
                {
                    existObDetails = _openingBalanceDetailLineItemService.GetListOfTPOPLineItemId(lineItemIds, serviceEntityId);
                }


                #region DocNo Checking

                string docNo = null;
                if (TObject.Any())
                {
                    List<OpeningBalanceLineItemModel> LstCNTROR = TObject.Where(c => c.DocDebit < 0 && openingBalanceDetailModel.AccountName == "Trade receivables" || openingBalanceDetailModel.AccountName == "Other receivables" && c.COAId == openingBalanceDetailModel.COAId && c.RecordStatus != null).ToList();
                    //List <OpeningBalanceLineItemModel> LstCNTROR = TObject.Where(c => (openingBalanceDetailModel.AccountName == "Trade receivables" || openingBalanceDetailModel.AccountName == "Other receivables" && openingBalanceDetailModel.RecordStatus != RecordStatus.Deleted).Select(x => x.COAId).Contains(openingBalanceDetailModel.COAId) && c.DocDebit < 0).ToList();

                    //For CN Doc No
                    if (LstCNTROR.Any())
                    {
                        var allDocNo = LstCNTROR.GroupBy(c => new { DocNo = c.DocumentReference }).Select(a => new { DocNo = a.Key.DocNo, Count = a.Count() });
                        if (allDocNo.Any())
                            docNo = string.Join(",", allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).ToList());
                        if (allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).Any())
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TR_OR_lines);
                        IDictionary<Guid, string> lstCNDocNo = _billService.GetAllCreditNoteDocNo(companyId);
                        if (lstCNDocNo.Any())
                        {
                            List<string> demo = lstCNDocNo.Where(c => !LstCNTROR.Select(d => d.Id).Contains(c.Key) && LstCNTROR.Select(d => d.DocumentReference).Contains(c.Value)).Select(c => c.Value).ToList();
                            if (demo.Any())
                                docNo = string.Join(",", demo);
                            if (demo.Count > 0)
                                throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_CreditNote);
                        }
                    }
                    //For Invoice Doc No
                    //List<OpeningBalanceLineItemModel> LstTROR = listItems.Where(c => TObject.Details.Where(d => d.AccountName == "Trade receivables" || d.AccountName == "Other receivables" && d.RecordStatus != RecordStatus.Deleted).Select(x => x.COAId).Contains(c.COAId) && c.DocDebit > 0).ToList();
                    List<OpeningBalanceLineItemModel> LstTROR = TObject.Where(c => c.DocDebit > 0 && openingBalanceDetailModel.AccountName == "Trade receivables" || openingBalanceDetailModel.AccountName == "Other receivables" && c.COAId == openingBalanceDetailModel.COAId && c.RecordStatus != null).ToList();
                    if (LstTROR.Any())
                    {
                        var allDocNo = LstTROR.GroupBy(c => new { DocNo = c.DocumentReference }).Select(a => new { DocNo = a.Key.DocNo, Count = a.Count() });
                        if (allDocNo.Any())
                            docNo = string.Join(",", allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).ToList());
                        if (allDocNo.Where(a => a.Count > 1).Select(c => c.DocNo).Any())
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TR_OR_lines);

                        IDictionary<Guid, string> invoiceDocNo = _billService.GetAllInvoiceDocNos(companyId);
                        if (invoiceDocNo.Any())
                        {
                            List<string> nod = invoiceDocNo.Where(c => !LstTROR.Select(d => d.Id).Contains(c.Key) && LstTROR.Select(d => d.DocumentReference).Contains(c.Value)).Select(c => c.Value).ToList();
                            if (nod.Any())
                                docNo = string.Join(",", nod);
                            if (nod.Count > 0)
                                throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_Invoice);
                        }
                    }

                    //List<OpeningBalanceDetailLineItem> LstCMTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsPayable || d.AccountName == COANameConstants.Other_payables && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId) && c.DocCredit < 0).ToList();
                    List<OpeningBalanceLineItemModel> LstCMTPOP = TObject.Where(c => openingBalanceDetailModel.AccountName == COANameConstants.AccountsPayable || openingBalanceDetailModel.AccountName == COANameConstants.Other_payables && openingBalanceDetailModel.RecordStatus != RecordStatus.Deleted && c.COAId == openingBalanceDetailModel.COAId && c.DocCredit < 0 && c.RecordStatus != null).ToList();
                    if (LstCMTPOP.Any())
                    {

                        var lstOPTP = LstCMTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).Select(c => new { c.Key.DocNo, Count = c.Count(), c.Key.Entity }).ToList();
                        if (lstOPTP.Any())
                            docNo = string.Join(",", lstOPTP.Where(c => c.Count > 1).Select(c => c.DocNo));
                        if (LstCMTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).ToList().Any(d => d.Count() > 1))
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TP_OP_lines);
                        List<CreditMemo> lstCreditMemos = _billService.GetlistOfCreditMemoDocNos(companyId);
                        if (lstCreditMemos.Any())
                        {
                            List<string> lstCMDocs = lstCreditMemos.Where(c => !LstCMTPOP.Select(d => d.Id).Contains(c.Id) && LstCMTPOP.Select(d => d.DocumentReference).Contains(c.DocNo) && LstCMTPOP.Select(d => d.EntityId).Contains(c.EntityId)).Select(c => c.DocNo).ToList();
                            if (lstCMDocs.Any())
                                docNo = string.Join(",", lstCMDocs);
                            if (lstCMDocs.Count > 0)
                                throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_CreditMemo);
                        }

                    }
                    //For Bill
                    //List<OpeningBalanceLineItemModel> LstTPOP = listItems.Where(c => TObject.Details.Where(d => d.AccountName == COANameConstants.AccountsPayable || d.AccountName == COANameConstants.Other_payables && d.RecordStatus != "Deleted").Select(x => x.COAId).Contains(c.COAId) && c.DocCredit > 0).ToList();
                    List<OpeningBalanceLineItemModel> LstTPOP = TObject.Where(c => c.DocCredit > 0 && openingBalanceDetailModel.AccountName == COANameConstants.AccountsPayable || openingBalanceDetailModel.AccountName == COANameConstants.Other_payables && openingBalanceDetailModel.RecordStatus != RecordStatus.Deleted && c.RecordStatus != null).ToList();
                    if (LstTPOP.Any())
                    {
                        var lstOPTP = LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).Select(c => new { c.Key.DocNo, Count = c.Count(), c.Key.Entity }).ToList();
                        if (lstOPTP.Any())
                            docNo = string.Join(",", lstOPTP.Where(c => c.Count > 1).Select(c => c.DocNo));
                        if (LstTPOP.GroupBy(c => new { DocNo = c.DocumentReference, Entity = c.EntityId }).ToList().Any(d => d.Count() > 1))
                            throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_duplicated_in_TP_OP_lines);

                        List<Bill> lstBills = _billService.GetAllBillDocNo(companyId);
                        if (lstBills.Any())
                        {
                            List<string> billDocs = lstBills.Where(c => !LstTPOP.Select(d => d.Id).Contains(c.Id) && LstTPOP.Select(d => d.DocumentReference).Contains(c.DocNo) && LstTPOP.Select(d => d.EntityId).Contains(c.EntityId)).Select(c => c.DocNo).ToList();
                            if (billDocs.Any())
                                docNo = string.Join(",", billDocs);
                            if (billDocs.Count > 0)
                                throw new Exception(docNo + " " + OpeningBalanceConstants.Document_number_are_existing_in_Bill);
                        }
                    }
                }
                #endregion


                #region OB_DETAIL_LINEITEM_DELETION
                if (lstDetailLineIds != null && lstDetailLineIds.Any())
                    lineItemsIds = string.Join(",", lstDetailLineIds);

                if (lineItemsIds.Any())
                {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_DeleteOBItems", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", companyId);
                        cmd.Parameters.AddWithValue("@OpeningBalanceId", OBId.ToString());
                        cmd.Parameters.AddWithValue("@OBDIds", string.Empty);
                        cmd.Parameters.AddWithValue("@OBDLIIds", lineItemsIds);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                #endregion OB_DETAIL_LINEITEM_DELETION

                OpeningBalanceDetail openingBalanceDetail = _openingBalanceDetailLineItemService.GetOpeningBalanceDetail(openingBalanceDetailModel.OpeningBalanceId,openingBalanceDetailModel.Id,openingBalanceDetailModel.COAId);
                if(openingBalanceDetail == null )
                {
                    //int Recoder = Convert.ToInt32(_openingBalanceDetailLineItemService.GetOpeningBalanceDetailRecOrder(openingBalanceDetailModel.OpeningBalanceId));
                    if (openingBalanceDetailModel.RecordStatus == RecordStatus.Added)
                    {
                        openingBalanceDetail = new OpeningBalanceDetail();
                        //openingBalanceDetail.Recorder = ++Recoder;
                        openingBalanceDetail.Id = openingBalanceDetailModel.Id;
                        FillDetails1(openingBalanceDetail, openingBalanceDetailModel);
                        openingBalanceDetail.OpeningBalanceId = openingBalanceDetailModel.OpeningBalanceId;
                        openingBalanceDetail.ObjectState = ObjectState.Added;
                        _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                    }
                }

                foreach (var lineItems in TObject)
                {
                    OBDetailId = lineItems.OpeningBalanceDetailId;
                    if (lineItems.RecordStatus == RecordStatus.Added)
                    {
                        OpeningBalanceDetailLineItem lineItem = new OpeningBalanceDetailLineItem();
                        lineItem.Id = lineItems.Id;
                        lineItem.OpeningBalanceDetailId = lineItems.OpeningBalanceDetailId;
                        lineItem.EntityId = lineItems.EntityId;
                        lineItem.ServiceCompanyId = serviceEntityId;
                        FillDetailLineItemVmToEntity(lineItems, lineItem);
                        lineItem.UserCreated = lineItems.UserCreated;
                        lineItem.CreatedDate = DateTime.UtcNow;
                        lineItem.IsEditable = true;
                        lineItem.IsProcressed = false;
                        lineItem.ObjectState = ObjectState.Added;
                        _openingBalanceDetailLineItemService.Insert(lineItem);
                    }
                    else if (lineItems.RecordStatus == RecordStatus.Modified)
                    {
                        if (existObDetails != null && existObDetails.Any())
                        {
                            OpeningBalanceDetailLineItem editLineItem = existObDetails.Where(d => d.Id == lineItems.Id).First();
                            if (editLineItem != null)
                            {
                                editLineItem.OpeningBalanceDetailId = lineItems.OpeningBalanceDetailId;
                                editLineItem.EntityId = lineItems.EntityId;
                                editLineItem.ServiceCompanyId = serviceEntityId;
                                FillDetailLineItemVmToEntity(lineItems, editLineItem);
                                editLineItem.ModifiedBy = lineItems.ModifiedBy;
                                editLineItem.ModifiedDate = DateTime.UtcNow;
                                editLineItem.ObjectState = ObjectState.Modified;
                                _openingBalanceDetailLineItemService.Update(editLineItem);
                            }
                        }
                    }
                    else
                    {

                    }
                    //else if (lineItems.RecordStatus == RecordStatus.Deleted)
                    //{
                    //    if (existObDetails != null && existObDetails.Any())
                    //    {
                    //        OpeningBalanceDetailLineItem DeleteLineItem = existObDetails.Where(d => d.Id == lineItems.Id).First();
                    //        if (DeleteLineItem != null)
                    //            DeleteLineItem.ObjectState = ObjectState.Deleted;
                    //    }
                    //}
                }
                try
                {
                    _openingBalancesModuleUnitOfWork.SaveChanges();

                    #region PROC_TO_SUM_Amount
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_UPDATE_OB_LINEITEM", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ServiceEntityID", serviceEntityId);
                        cmd.Parameters.AddWithValue("@OBDetailId", OBDetailId.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    #endregion PROC_TO_SUM_Amount
                }
                catch (DbEntityValidationException ex)
                {
                    LoggingHelper.LogMessage(OpeningBalanceLoggingValidation.OpeningBalancesApplicationService, OpeningBalanceLoggingValidation.Entered_Into_Catch_Block);
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                    throw;
                }
                openingBalanceDetailModel.DocCredit = TObject.Sum(x => x.DocCredit);
                openingBalanceDetailModel.DocDebit = TObject.Sum(x => x.DocDebit);
                openingBalanceDetailModel.BaseCredit = TObject.Sum(x => x.BaseCredit);
                openingBalanceDetailModel.BaseDebit = TObject.Sum(x => x.BaseDebit);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return openingBalanceDetailModel;
        }

        public void SaveScreenFolders(List<OpeningBalanceDetailLineItem> listOfDetailLineItems, bool isAdd, long companyId)
        {
            foreach (OpeningBalanceDetailLineItem lItems in listOfDetailLineItems)
            {
                ScreenRecordsSave screenRecords = new ScreenRecordsSave();
                screenRecords.ReferenceId = lItems.EntityId.ToString();
                screenRecords.FeatureId = lItems.EntityId.ToString();
                screenRecords.RecordId = lItems.Id.ToString();
                screenRecords.recordName = lItems.DocumentReference;
                screenRecords.UserName = lItems.UserCreated;
                screenRecords.Date = DateTime.UtcNow;
                screenRecords.isAdd = isAdd;
                screenRecords.CursorName = "Bean Cursor";
                screenRecords.ScreenName = "Bills";
                screenRecords.CompanyId = companyId;
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
                    //return true;
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    //return false;
                }
            }
        }
        public void UpdateCustomerBalance(long companyId, string entityIds, string connectionString)
        {
            SqlConnection con = new SqlConnection(connectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
            SqlCommand cmd = new SqlCommand("Bean_Update_CustBalance", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", companyId.ToString());
            cmd.Parameters.AddWithValue("@entitIds", entityIds);
            int res = cmd.ExecuteNonQuery();
            con.Close();
        }

        private Guid? Item(OpeningBalanceModel TObject, OpeningBalanceDetailLineItem lineItem, bool? isGstActivated, long? coaId, out long? taxid, string ConnectionString)
        {
            taxid = null;
            Guid? itemId = _iItemService.GetItemByObId("Opening Balance", TObject.CompanyId);
            if (itemId == Guid.Empty)
            {
                string Desc = "Opening Balance - " + TObject.Date.ToString("dd/MM/yyyy");
                //taxid = null;
                itemId = Guid.NewGuid();
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("InsertOpeningBalanceItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", itemId);
                cmd.Parameters.AddWithValue("@companyId", TObject.CompanyId);
                //cmd.Parameters.AddWithValue("@unitPrice", lineItem.DoCDebit);
                //cmd.Parameters.AddWithValue("@COAId", lineItem.COAId);
                cmd.Parameters.AddWithValue("@desc", Desc);
                cmd.Parameters.AddWithValue("@COAId", coaId);
                cmd.Parameters.AddWithValue("@userCreated", TObject.UserCreated);
                cmd.Parameters.AddWithValue("@isGstActivated", isGstActivated == null ? false : isGstActivated);
                cmd.Parameters.Add("@taxId", SqlDbType.BigInt, 32);
                cmd.Parameters["@taxId"].Direction = ParameterDirection.Output;

                int result = cmd.ExecuteNonQuery();
                //SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    while (dr.Read())
                //    {
                //        itemId = Guid.Parse(dr["Id"].ToString());
                //    }
                //}
                con.Close();
                taxid = cmd.Parameters["@taxId"].Value != DBNull.Value ? Convert.ToInt64(cmd.Parameters["@taxId"].Value) : (long?)null;
            }

            return itemId;

        }

        private void FillInvoiceModel(InvoiceModel invoiceModel, OpeningBalanceDetailLineItem lineItem, OpeningBalanceModel openingBalanceModel, Guid? itemId, long? taxId, long? coaId, string nature, Guid OpeningBalanceId, bool isBill)
        {
            decimal? docAmount = 0;
            decimal? baseAmount = 0;
            invoiceModel.CompanyId = openingBalanceModel.CompanyId;
            invoiceModel.ServiceCompanyId = openingBalanceModel.ServiceCompanyId;
            invoiceModel.DocType = isBill == true ? DocTypeConstants.Bills : DocTypeConstants.Invoice;
            invoiceModel.DocSubType = "Opening Bal";
            invoiceModel.BaseCurrency = openingBalanceModel.BaseCurrency;
            invoiceModel.DocCurrency = lineItem.DocumentCurrency;
            invoiceModel.DocDate = lineItem.Date;
            invoiceModel.DueDate = lineItem.DueDate;

            //if (isBill == true)
            //    invoiceModel.PostingDate = lineItem.Date;//for Bill

            invoiceModel.DocNo = lineItem.DocumentReference;
            invoiceModel.EntityId = lineItem.EntityId;
            //invoiceModel.EntityName = lineItem.EntityName;
            //invoiceModel.DocDescription = lineItem.Description;
            //invoiceModel.ExchangeRate = lineItem.ExchangeRate != null ? lineItem.ExchangeRate : 1.0000000000m;
            invoiceModel.CreditTermsId = null;
            //invoiceModel.ServiceCompanyId = lineItem.ServiceCompanyId;
            invoiceModel.ServiceCompanyId = openingBalanceModel.ServiceCompanyId;
            invoiceModel.Nature = nature;
            invoiceModel.IsMultiCurrency = openingBalanceModel.IsMultiCurrencyActive != null ? openingBalanceModel.IsMultiCurrencyActive.Value : false;
            invoiceModel.Id = lineItem.Id;
            invoiceModel.IsNoSupportingDocument = false;
            invoiceModel.NoSupportingDocument = false;
            invoiceModel.PONo = null;
            invoiceModel.GSTExCurrency = "SGD";
            invoiceModel.IsOBInvoice = true;
            invoiceModel.DocumentState = "Not Paid";
            invoiceModel.Status = RecordStatusEnum.Active;
            invoiceModel.OpeningBalanceId = OpeningBalanceId;
            invoiceModel.UserCreated = openingBalanceModel.UserCreated;
            invoiceModel.PeriodLockPassword = openingBalanceModel.PeriodLockPassword;
            //invoiceModel.Nature = lineItem.Description;
            //invoiceModel.PONo = lineItem.Description;

            if (isBill == true)
            {
                if (lineItem.DocCredit > 0 && lineItem.DocCredit != null)
                {
                    docAmount = lineItem.DocCredit;
                    baseAmount = lineItem.BaseCredit;
                }
                else if (lineItem.DocCredit < 0 && lineItem.DocCredit != null)
                {
                    docAmount = -(lineItem.DocCredit);
                    baseAmount = -(lineItem.BaseCredit);
                }
            }
            else
            {
                if (lineItem.DoCDebit > 0 && lineItem.DoCDebit != null)
                {
                    docAmount = lineItem.DoCDebit;
                    baseAmount = lineItem.BaseDebit;
                }
                else if (lineItem.DoCDebit < 0 && lineItem.DoCDebit != null)
                {
                    docAmount = -(lineItem.DoCDebit);
                    baseAmount = -(lineItem.BaseDebit);
                }
            }


            //docAmount = isBill == true ? (lineItem.DocCredit != null && lineItem.DocCredit != 0) ? lineItem.DocCredit : 0 : (lineItem.DoCDebit != 0 && lineItem.DoCDebit != null) ? lineItem.DoCDebit : 0;
            //baseAmount = isBill == true ? (lineItem.BaseCredit != null && lineItem.BaseCredit != 0) ? lineItem.BaseCredit : 0 : (lineItem.BaseDebit != 0 && lineItem.BaseDebit != null) ? lineItem.BaseDebit : 0;

            invoiceModel.ExchangeRate = lineItem.BaseCurrency == lineItem.DocumentCurrency ? lineItem.ExchangeRate /*: (docAmount != 0 && baseAmount != 0) ? baseAmount / docAmount*/: lineItem.ExchangeRate;
            invoiceModel.IsDocNoEditable = true;
            if (isBill == true)//for Bill
            {
                invoiceModel.PostingDate = lineItem.Date;
                invoiceModel.IsExternal = true;
                invoiceModel.ISAllowDisAllow = false;
                invoiceModel.GstReportingCurrency = "SGD";
            }
            if (isBill == true)
            {
                List<BillDetailModel> lstBillDetail = new List<BillDetailModel>();
                BillDetailModel billDetail = new BillDetailModel();
                billDetail.COAId = coaId.Value;
                billDetail.Description = lineItem.Description != null ? lineItem.Description : "Opening Balance - " + invoiceModel.DocDate.Value.ToString("dd/MM/yyyy");
                if (openingBalanceModel.IsGSTActivated == true)
                {
                    billDetail.TaxId = taxId != null ? taxId : _taxcodeService.GetTaxID("NA", 0);
                    billDetail.TaxIdCode = "NA";
                    billDetail.TaxRate = null;
                    invoiceModel.IsGstSettings = true;
                    //invoiceModel.GSTExchangeRate = 1.0000000000m;
                    invoiceModel.GSTExchangeRate = invoiceModel.ExchangeRate;
                }
                //billDetail.DocAmount = (lineItem.DoCDebit != null && lineItem.DoCDebit != 0) ? lineItem.DoCDebit.Value : lineItem.DocCredit.Value;
                billDetail.DocAmount = docAmount != null ? docAmount.Value : 0;
                billDetail.DocTaxAmount = 0;
                //invoiceDetail.DocTaxAmount = (lineItem.ExchangeRate != null && lineItem.ExchangeRate != 0) ? Math.Round(invoiceDetail.DocAmount * (decimal)lineItem.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0;
                billDetail.DocTotalAmount = billDetail.DocAmount;
                billDetail.IsDisallow = false;

                invoiceModel.GrandTotal = billDetail.DocAmount;
                invoiceModel.BalanceAmount = invoiceModel.GrandTotal;
                invoiceModel.DocDescription = billDetail.Description;
                billDetail.RecordStatus = "Added";
                lstBillDetail.Add(billDetail);
                invoiceModel.BillDetailModels = lstBillDetail;
                saveOBInvoice(invoiceModel, true);
            }
            else
            {
                List<InvoiceDetail> lstInvoiceDetail = new List<InvoiceDetail>();
                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.Id = Guid.NewGuid();
                invoiceDetail.ItemId = itemId;
                invoiceDetail.ItemCode = "Opening Balance";
                invoiceDetail.ItemDescription = lineItem.Description != null ? lineItem.Description : "Opening Balance - " + invoiceModel.DocDate.Value.ToString("dd/MM/yyyy");
                //invoiceDetail.UnitPrice = lineItem.DoCDebit;
                //invoiceDetail.UnitPrice = (lineItem.DoCDebit != null && lineItem.DoCDebit != 0) ? lineItem.DoCDebit.Value : lineItem.DocCredit.Value;
                invoiceDetail.UnitPrice = docAmount;
                invoiceDetail.COAId = coaId.Value;
                invoiceDetail.IsPLAccount = false;
                invoiceDetail.Qty = 1;
                invoiceDetail.AmtCurrency = lineItem.DocumentCurrency;
                if (openingBalanceModel.IsGSTActivated == true)
                {
                    invoiceDetail.TaxId = taxId != null ? taxId : _taxcodeService.GetTaxID("NA", 0);
                    invoiceDetail.TaxIdCode = "NA";
                    invoiceDetail.TaxRate = null;
                    invoiceModel.IsGstSettings = true;
                    //invoiceModel.GSTExchangeRate = 1.0000000000m;
                    invoiceModel.GSTExchangeRate = invoiceModel.ExchangeRate;
                }

                //invoiceDetail.DocAmount = (lineItem.DoCDebit != null && lineItem.DoCDebit != 0) ? lineItem.DoCDebit.Value : lineItem.DocCredit.Value;
                invoiceDetail.DocAmount = docAmount != null ? docAmount.Value : 0;
                invoiceDetail.DocTaxAmount = 0;
                //invoiceDetail.DocTaxAmount = (lineItem.ExchangeRate != null && lineItem.ExchangeRate != 0) ? Math.Round(invoiceDetail.DocAmount * (decimal)lineItem.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0;
                invoiceDetail.DocTotalAmount = invoiceDetail.DocAmount + (decimal)invoiceDetail.DocTaxAmount;
                //invoiceDetail.BaseTaxAmount = invoiceDetail.DocAmount;
                //invoiceDetail.BaseTotalAmount = lineItem.DoCDebit;

                //calculating Amount related things
                invoiceModel.GrandTotal = invoiceDetail.DocAmount;
                invoiceModel.BalanceAmount = invoiceModel.GrandTotal;
                invoiceModel.DocDescription = invoiceDetail.ItemDescription;
                //invoiceModel.IsDocNoEditable = false;
                invoiceDetail.RecordStatus = "Added";
                lstInvoiceDetail.Add(invoiceDetail);
                invoiceModel.InvoiceDetails = lstInvoiceDetail;
                saveOBInvoice(invoiceModel, false);
            }
            //invoiceModel.InvoiceDetails.Add(invoiceDetail);

        }

        #endregion

        #region private

        #region getcall

        private void FillOpeningBalance(OpeningBalanceModel PopeningbalanceModel, OpeningBalance openingBalance)
        {
            PopeningbalanceModel.Id = openingBalance.Id;
            PopeningbalanceModel.CompanyId = openingBalance.CompanyId;
            PopeningbalanceModel.BaseCurrency = openingBalance.BaseCurrency;
            PopeningbalanceModel.Version = "0x" + string.Concat(Array.ConvertAll(openingBalance.Version, x => x.ToString("X2")));
            PopeningbalanceModel.Date = openingBalance.Date;
            PopeningbalanceModel.DocType = openingBalance.DocType;
            PopeningbalanceModel.UserCreated = openingBalance.UserCreated;
            PopeningbalanceModel.CreatedDate = openingBalance.CreatedDate;
            PopeningbalanceModel.UserModified = openingBalance.ModifiedBy;
            PopeningbalanceModel.ModifiedDate = openingBalance.ModifiedDate;
            PopeningbalanceModel.IsMultiCurrencyActive = openingBalance.IsMultiCurrency;
            PopeningbalanceModel.IsNoSupportingDoc = openingBalance.IsNoSupportingDoc;
            PopeningbalanceModel.IsSegmentActive = openingBalance.IsSegmentReporting;
            PopeningbalanceModel.SystemRefNo = openingBalance.SystemRefNo;
            PopeningbalanceModel.IsEditable = openingBalance.IsEditable;
        }

        private void FillOpeningBalanceDetail(OpeningBalanceDetailModel OpeningBalanceDetailModel, OpeningBalanceDetail Details, ChartOfAccount coa)
        {
            OpeningBalanceDetailModel.Id = Details.Id;
            OpeningBalanceDetailModel.COAId = Details.COAId;
            //var coa = _chartOfAccountService.GetChartOfAccountById(Details.COAId);
            if (coa != null)
            {
                OpeningBalanceDetailModel.AccountName = coa.Name;
                OpeningBalanceDetailModel.AccountCode = coa.Code;
                OpeningBalanceDetailModel.DocCurrency = coa.Currency;
                OpeningBalanceDetailModel.Nature = coa.Nature;
            }
            OpeningBalanceDetailModel.BaseCredit = Details.BaseCredit;
            OpeningBalanceDetailModel.BaseDebit = Details.BaseDebit;

            OpeningBalanceDetailModel.DocCurrency = Details.DocCurrency;
            OpeningBalanceDetailModel.DocCredit = Details.DocCredit;
            OpeningBalanceDetailModel.DocDebit = Details.DocDebit;
            OpeningBalanceDetailModel.OpeningBalanceId = Details.OpeningBalanceId;
            OpeningBalanceDetailModel.ShowLineItemCount = Details.OpeningBalanceDetailLineItems.Count();
            OpeningBalanceDetailModel.UserCreated = Details.UserCreated;
            OpeningBalanceDetailModel.CreatedDate = Details.CreatedDate;
            OpeningBalanceDetailModel.ModifiedBy = Details.ModifiedBy;
            OpeningBalanceDetailModel.ModifiedDate = Details.ModifiedDate;
            OpeningBalanceDetailModel.IsSystemAccount = true;
            OpeningBalanceDetailModel.IsOrginalAccount = Details.IsOrginalAccount;
            OpeningBalanceDetailModel.ClearingState = Details.ClearingState;
            OpeningBalanceDetailModel.RecordStatus = null;
            OpeningBalanceDetailModel.ClearingDate = Details.ClearingDate;
            OpeningBalanceDetailModel.ReconciliationDate = Details.ReconciliationDate;
            OpeningBalanceDetailModel.ReconciliationId = Details.ReconciliationId;
            OpeningBalanceDetailModel.RecOrder = Details.Recorder;

        }


        private void FillChartofAccount(OpeningBalanceDetailModel openingBalanceDetailModel, ChartOfAccount chartofaccount, long companyId, AccountType accType)
        {
            openingBalanceDetailModel.Id = Guid.NewGuid();
            openingBalanceDetailModel.COAId = chartofaccount.Id;
            openingBalanceDetailModel.AccountCode = chartofaccount.Code;
            openingBalanceDetailModel.AccountName = chartofaccount.Name;
            openingBalanceDetailModel.Nature = chartofaccount.Nature;
            openingBalanceDetailModel.IsAllowDisAllow = chartofaccount.DisAllowable ?? false;
            openingBalanceDetailModel.IsPLAccount = chartofaccount.Category == "Income Statement" ? true : false;
            //var acc = _accountTypeService.GetById(companyId, COANameConstants.Cashandbankbalances);
            if (accType != null)
                openingBalanceDetailModel.IsCashandbank = chartofaccount.AccountTypeId == accType.Id ? true : false;
            openingBalanceDetailModel.UserCreated = chartofaccount.UserCreated;
            openingBalanceDetailModel.CreatedDate = chartofaccount.CreatedDate;
            openingBalanceDetailModel.RecordStatus = RecordStatus.Added;
            openingBalanceDetailModel.IsSystemAccount = true;
            openingBalanceDetailModel.IsOrginalAccount = chartofaccount.IsSeedData;
        }
        private void FillChartofAccountNew(OpeningBalanceDetail openingBalanceDetailModel, ChartOfAccount chartofaccount, long companyId, AccountType accType)
        {
            int Recoder = 0;
            openingBalanceDetailModel.Id = Guid.NewGuid();
            openingBalanceDetailModel.COAId = chartofaccount.Id;
            openingBalanceDetailModel.UserCreated = chartofaccount.UserCreated;
            openingBalanceDetailModel.CreatedDate = chartofaccount.CreatedDate;
            openingBalanceDetailModel.Recorder = ++Recoder;
            openingBalanceDetailModel.IsOrginalAccount = chartofaccount.IsSeedData;
        }

        private void FillLineItem(OpeningBalanceLineItemModel openingBalanceDetaillineitem, OpeningBalanceDetailLineItem openingBalancelineitem, List<ChartOfAccount> lstCOA)
        {
            openingBalanceDetaillineitem.Date = openingBalancelineitem.Date;
            //openingBalanceDetaillineitem.COAId = openingBalancelineitem.COAId;
            //var coa = _chartOfAccountService.GetChartOfAccountById(openingBalancelineitem.COAId);
            //if (coa != null)
            //{
            //    openingBalanceDetaillineitem.AccountName = coa.Name;
            //    openingBalanceDetaillineitem.AccountCode = coa.Code;
            //}
            openingBalanceDetaillineitem.BaseCurrency = openingBalancelineitem.BaseCurrency;
            openingBalanceDetaillineitem.Id = openingBalancelineitem.Id;
            openingBalanceDetaillineitem.OpeningBalanceDetailId = openingBalancelineitem.OpeningBalanceDetailId;
            openingBalanceDetaillineitem.COAId = openingBalancelineitem.COAId;
            //var coa = _chartOfAccountService.GetChartOfAccountById(openingBalancelineitem.COAId);
            //if (coa != null)
            //{
            //    openingBalanceDetaillineitem.AccountName = coa.Name;
            //    openingBalanceDetaillineitem.AccountCode = coa.Code;
            //}
            if (lstCOA.Any())
            {
                openingBalanceDetaillineitem.AccountName = lstCOA.Where(c => c.Id == openingBalancelineitem.COAId).Select(c => c.Name).FirstOrDefault();
                openingBalanceDetaillineitem.AccountCode = lstCOA.Where(c => c.Id == openingBalancelineitem.COAId).Select(c => c.Code).FirstOrDefault();
            }
            openingBalanceDetaillineitem.BaseCredit = openingBalancelineitem.BaseCredit;
            openingBalanceDetaillineitem.BaseDebit = openingBalancelineitem.BaseDebit;
            openingBalanceDetaillineitem.DocCurrency = openingBalancelineitem.DocumentCurrency;
            openingBalanceDetaillineitem.DocCredit = openingBalancelineitem.DocCredit;
            openingBalanceDetaillineitem.DocDebit = openingBalancelineitem.DoCDebit;
            openingBalanceDetaillineitem.Description = openingBalancelineitem.Description;
            openingBalanceDetaillineitem.ExchangeRate = openingBalancelineitem.ExchangeRate;
            openingBalanceDetaillineitem.EntityId = openingBalancelineitem.EntityId;
            //if (openingBalancelineitem.EntityId != null)
            //{
            //    var beanentity = _beanEntityService.GetEntityById(openingBalancelineitem.EntityId.Value);
            //    if (beanentity != null)
            //    {
            //        openingBalanceDetaillineitem.EntityName = beanentity.Name;
            //    }
            //}
            openingBalanceDetaillineitem.ServiceCompanyId = openingBalancelineitem.ServiceCompanyId;
            //openingBalanceDetaillineitem.SegmentMasterid1 = openingBalancelineitem.SegmentMasterid1.ToString();
            //openingBalanceDetaillineitem.SegmentMasterid2 = openingBalancelineitem.SegmentMasterid2.ToString();
            //openingBalanceDetaillineitem.SegmentCategory1 = openingBalancelineitem.SegmentCategory1;
            //openingBalanceDetaillineitem.SegmentCategory2 = openingBalancelineitem.SegmentCategory2;
            //openingBalanceDetaillineitem.SegmentDetailid1 = openingBalancelineitem.SegmentDetailid1;
            //openingBalanceDetaillineitem.SegmentDetailid2 = openingBalancelineitem.SegmentDetailid2;
            openingBalanceDetaillineitem.DocumentReference = openingBalancelineitem.DocumentReference;
            openingBalanceDetaillineitem.IsDisAllow = openingBalancelineitem.IsDisAllow;
            openingBalanceDetaillineitem.UserCreated = openingBalancelineitem.UserCreated;
            openingBalanceDetaillineitem.CreatedDate = openingBalancelineitem.CreatedDate;
            openingBalanceDetaillineitem.ModifiedBy = openingBalancelineitem.ModifiedBy;
            openingBalanceDetaillineitem.ModifiedDate = openingBalancelineitem.ModifiedDate;
            openingBalanceDetaillineitem.DueDate = openingBalancelineitem.DueDate;
            openingBalanceDetaillineitem.IsEditable = openingBalancelineitem.IsEditable;
            openingBalanceDetaillineitem.RecOrder = openingBalancelineitem.Recorder;
            openingBalanceDetaillineitem.IsProcressed = openingBalancelineitem.IsProcressed;
            openingBalanceDetaillineitem.ProcressedRemarks = openingBalancelineitem.ProcressedRemarks;
        }

        private void FillLineItemByCOAID(OpeningBalanceLineItemModel openingBalanceDetaillineitemM, OpeningBalanceDetailLineItem openingBalancelineitem)
        {
            openingBalanceDetaillineitemM.BaseCredit = openingBalancelineitem.BaseCredit;
            openingBalanceDetaillineitemM.BaseDebit = openingBalancelineitem.BaseDebit;
            openingBalanceDetaillineitemM.DocCurrency = openingBalancelineitem.DocumentCurrency;
            openingBalanceDetaillineitemM.DocCredit = openingBalancelineitem.DocCredit;
            openingBalanceDetaillineitemM.DocDebit = openingBalancelineitem.DoCDebit;
            openingBalanceDetaillineitemM.Description = openingBalancelineitem.Description;
            openingBalanceDetaillineitemM.ExchangeRate = openingBalancelineitem.ExchangeRate;
            openingBalanceDetaillineitemM.DocumentReference = openingBalancelineitem.DocumentReference;
            openingBalanceDetaillineitemM.EntityId = openingBalancelineitem.EntityId;

        }

        private void FillOpeningBalanceDetailModel(OpeningBalanceDetailModel openingBalanceM, OpeningBalanceDetail openingBalancelineitemByCoaid)
        {
            openingBalanceM.Id = openingBalancelineitemByCoaid.Id;
            openingBalanceM.COAId = openingBalancelineitemByCoaid.COAId;
            openingBalanceM.BaseCurrency = openingBalancelineitemByCoaid.BaseCurrency;
            openingBalanceM.BaseCredit = openingBalancelineitemByCoaid.BaseCredit;
            openingBalanceM.BaseDebit = openingBalancelineitemByCoaid.BaseDebit;
            openingBalanceM.DocCurrency = openingBalancelineitemByCoaid.BaseCurrency;
            openingBalanceM.DocCredit = openingBalancelineitemByCoaid.DocCredit;
            openingBalanceM.DocDebit = openingBalancelineitemByCoaid.DocDebit;
            openingBalanceM.OpeningBalanceId = openingBalancelineitemByCoaid.OpeningBalanceId;
            openingBalanceM.ShowLineItemCount = openingBalancelineitemByCoaid.OpeningBalanceDetailLineItems.Count();
            openingBalanceM.UserCreated = openingBalancelineitemByCoaid.UserCreated;
            openingBalanceM.CreatedDate = openingBalancelineitemByCoaid.CreatedDate;
            openingBalanceM.ModifiedBy = openingBalancelineitemByCoaid.ModifiedBy;
            openingBalanceM.ModifiedDate = openingBalancelineitemByCoaid.ModifiedDate;
        }

        private void UpdateOBDetail(OpeningBalanceModel TObject, List<Guid?> newEntityIds, List<Guid?> oldEntityIds, List<OpeningBalanceDetailLineItem> lstObLineItem, List<long> coaIds, List<OpeningBalanceLineItemModel> lstOBDLitemsModel)
        {
            if (TObject.Details.Any())
            {
                //Dictionary<Guid, string> lstLineitemIdsandDocno = new Dictionary<Guid, string>();
                //List<Tuple<Guid, Guid?, string, string>> lstItems = new List<Tuple<Guid, Guid, string, string>>();
                List<OpeningBalanceDetail> lstOBDetail = _OpeningBalanceDetailService.GetAllOBDetailAndLineItmsById(TObject.Details.Where(c => c.RecordStatus == RecordStatus.Modified).Select(c => c.Id).ToList());
                //lstObLineItem = _openingBalanceDetailLineItemService.GetListOfOBDLineItemByCoaId(coaIds, TObject.ServiceCompanyId);
                foreach (var details in TObject.Details)
                {
                    OpeningBalanceDetail openingBalanceDetail = new OpeningBalanceDetail();
                    if (details.RecordStatus != RecordStatus.Added && details.RecordStatus != RecordStatus.Deleted && details.RecordStatus != null)
                    {
                        openingBalanceDetail = lstOBDetail.Where(c => c.Id == details.Id).FirstOrDefault();
                        if (openingBalanceDetail != null)
                        {
                            FillOpeningBalanceVmToEntity(details, openingBalanceDetail, TObject);
                            openingBalanceDetail.ObjectState = ObjectState.Modified;
                        }
                        //_OpeningBalanceDetailService.Update(openingBalanceDetail);
                        if (details.LineItems != null)
                        {
                            OpeningBalanceDetailLineItem openingBalanceDetailLineItem = null;
                            foreach (var lineItems in details.LineItems)
                            {
                                if (lineItems.RecordStatus != RecordStatus.Added && lineItems.RecordStatus != RecordStatus.Deleted && lineItems.RecordStatus != null)
                                {
                                    openingBalanceDetailLineItem = lstOBDetail.SelectMany(c => c.OpeningBalanceDetailLineItems).Where(d => d != null && d.Id == lineItems.Id).FirstOrDefault();
                                    if (openingBalanceDetailLineItem != null)
                                    {
                                        if (lineItems.EntityId != null)
                                        {
                                            //OpeningBalanceDetailLineItem item = openingBalanceDetailLineItem;
                                            newEntityIds.Add(lineItems.EntityId);
                                            oldEntityIds.Add(openingBalanceDetailLineItem.EntityId);
                                            OpeningBalanceLineItemModel model = new OpeningBalanceLineItemModel();
                                            model.Id = openingBalanceDetailLineItem.Id;
                                            model.EntityId = openingBalanceDetailLineItem.EntityId;
                                            model.DocumentReference = openingBalanceDetailLineItem.DocumentReference;
                                            model.DocCredit = openingBalanceDetailLineItem.DocCredit;
                                            model.DocDebit = openingBalanceDetailLineItem.DoCDebit;
                                            model.UserCreated = openingBalanceDetailLineItem.UserCreated;
                                            model.CreatedDate = openingBalanceDetailLineItem.CreatedDate;
                                            model.ModifiedBy = openingBalanceDetailLineItem.ModifiedBy;
                                            model.ModifiedDate = openingBalanceDetailLineItem.ModifiedDate;
                                            model.COAId = openingBalanceDetailLineItem.COAId;
                                            model.RecOrder = openingBalanceDetailLineItem.Recorder;
                                            lstOBDLitemsModel.Add(model);
                                            //lstObLineItem.Add(item);
                                            //Tuple<Guid, Guid?, string, string> tple = Tuple.Create(openingBalanceDetailLineItem.Id, openingBalanceDetailLineItem.EntityId, openingBalanceDetailLineItem.DocumentReference, openingBalanceDetailLineItem.UserCreated);
                                            //lstLineitemIdsandDocno.Add(lineItems.Id,lineItems.EntityId)
                                        }
                                        FillDetailLineItemVmToEntity(lineItems, openingBalanceDetailLineItem);
                                        openingBalanceDetailLineItem.OpeningBalanceDetailId = details.Id;
                                        openingBalanceDetailLineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                                        //openingBalanceDetailLineItem.Id = lineItems.Id;
                                        openingBalanceDetailLineItem.CreatedDate = lineItems.CreatedDate;
                                        openingBalanceDetailLineItem.UserCreated = lineItems.UserCreated;
                                        openingBalanceDetailLineItem.ModifiedBy = lineItems.ModifiedBy;
                                        openingBalanceDetailLineItem.ModifiedDate = DateTime.UtcNow;
                                        openingBalanceDetailLineItem.ObjectState = ObjectState.Modified;
                                    }
                                    //_openingBalanceDetailLineItemService.Update(openingBalanceDetailLineItem);
                                }
                                else if (lineItems.RecordStatus == RecordStatus.Added)
                                {
                                    if (lineItems.EntityId != null)
                                    {
                                        newEntityIds.Add(lineItems.EntityId);
                                    }
                                    openingBalanceDetailLineItem = new OpeningBalanceDetailLineItem();
                                    openingBalanceDetailLineItem.Id = lineItems.Id;
                                    openingBalanceDetailLineItem.OpeningBalanceDetailId = details.Id;
                                    openingBalanceDetailLineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                                    FillDetailLineItemVmToEntity(lineItems, openingBalanceDetailLineItem);
                                    openingBalanceDetailLineItem.ObjectState = ObjectState.Added;
                                    openingBalanceDetailLineItem.IsEditable = true;
                                    openingBalanceDetailLineItem.UserCreated = lineItems.UserCreated;
                                    openingBalanceDetailLineItem.CreatedDate = DateTime.UtcNow;
                                    _openingBalanceDetailLineItemService.Insert(openingBalanceDetailLineItem);
                                }
                                //else if (lineItems.RecordStatus == "Deleted")
                                //{
                                //    deleteLineItem(lineItems.Id);
                                //}
                            }
                        }
                    }
                    if (details.RecordStatus == RecordStatus.Added)
                    {
                        FillDetails(openingBalanceDetail, details, TObject);
                        openingBalanceDetail.OpeningBalanceId = TObject.Id;
                        openingBalanceDetail.Recorder = details.RecOrder;
                        openingBalanceDetail.Id = Guid.NewGuid();
                        openingBalanceDetail.ObjectState = ObjectState.Added;
                        _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                        if (details.LineItems != null)
                        {
                            foreach (var lineItems in details.LineItems)
                            {
                                if (lineItems.EntityId != null)
                                {
                                    newEntityIds.Add(lineItems.EntityId);
                                }
                                OpeningBalanceDetailLineItem lineItem = new OpeningBalanceDetailLineItem();
                                lineItem.Id = lineItems.Id;
                                lineItem.OpeningBalanceDetailId = openingBalanceDetail.Id;
                                lineItem.ServiceCompanyId = TObject.ServiceCompanyId;
                                FillDetailLineItemVmToEntity(lineItems, lineItem);
                                lineItem.Recorder = lineItems.RecOrder;
                                lineItem.UserCreated = lineItems.UserCreated;
                                lineItem.IsEditable = true;
                                lineItem.CreatedDate = DateTime.UtcNow;
                                lineItem.ObjectState = ObjectState.Added;
                                _openingBalanceDetailLineItemService.Insert(lineItem);

                            }
                        }

                    }
                    //else if (details.RecordStatus == "Deleted")
                    //{
                    //    deleteDetail(details.Id);
                    //}
                }
            }
        }

        private void UpdateOBDetailNew(OpeningBalanceModel TObject, List<Guid?> newEntityIds, List<Guid?> oldEntityIds, List<OpeningBalanceDetailLineItem> lstObLineItem, List<long> coaIds, List<OpeningBalanceLineItemModel> lstOBDLitemsModel)
        {
            if (TObject.Details.Any())
            {
                List<OpeningBalanceDetail> lstOBDetail = _OpeningBalanceDetailService.GetAllOBDetailAndLineItmsById(TObject.Details.Where(c => c.RecordStatus == RecordStatus.Modified).Select(c => c.Id).ToList());
                foreach (var details in TObject.Details)
                {
                    OpeningBalanceDetail openingBalanceDetail = new OpeningBalanceDetail();
                    if (details.RecordStatus != RecordStatus.Added && details.RecordStatus != RecordStatus.Deleted && details.RecordStatus != null)
                    {
                        openingBalanceDetail = lstOBDetail.Where(c => c.Id == details.Id).FirstOrDefault();
                        if (openingBalanceDetail != null)
                        {
                            FillOpeningBalanceVmToEntity(details, openingBalanceDetail, TObject);
                            openingBalanceDetail.ObjectState = ObjectState.Modified;
                        }
                    }
                    else if(details.RecordStatus == RecordStatus.Added)
                    {
                        FillDetails(openingBalanceDetail, details, TObject);
                        openingBalanceDetail.OpeningBalanceId = TObject.Id;
                        openingBalanceDetail.Recorder = details.RecOrder;
                        openingBalanceDetail.Id = Guid.NewGuid();
                        openingBalanceDetail.ObjectState = ObjectState.Added;
                        _OpeningBalanceDetailService.Insert(openingBalanceDetail);
                    }
                }
            }
        }

        #endregion

        #region SaveCall_Private_Block

        private void FillOpeningBalances(OpeningBalance _openingBalance, OpeningBalanceModel TObject)
        {
            _openingBalance.Date = TObject.Date;
            _openingBalance.BaseCurrency = TObject.BaseCurrency;
            _openingBalance.DocType = DocTypeConstants.OpeningBalance;
            _openingBalance.SaveType = TObject.SaveType;
            _openingBalance.CompanyId = TObject.CompanyId;
            _openingBalance.IsSegmentReporting = TObject.IsSegmentActive;
            _openingBalance.IsMultiCurrency = TObject.IsMultiCurrencyActive;
            _openingBalance.IsNoSupportingDoc = TObject.IsNoSupportingDoc;
            _openingBalance.ServiceCompanyId = TObject.ServiceCompanyId;
            _openingBalance.IsEditable = TObject.IsEditable;
        }
        private void FillOpeningBalances1(OpeningBalance _openingBalance, OpeningBalanceModel TObject)
        {
            _openingBalance.BaseCurrency = TObject.BaseCurrency;
            _openingBalance.DocType = DocTypeConstants.OpeningBalance;
            _openingBalance.CompanyId = TObject.CompanyId;
            _openingBalance.ServiceCompanyId = TObject.ServiceCompanyId;
        }

        private void FillDetailLineItemVmToEntity(OpeningBalanceLineItemModel lineItems, OpeningBalanceDetailLineItem openingBalanceDetailLineItem)
        {
            openingBalanceDetailLineItem.Date = lineItems.Date;
            openingBalanceDetailLineItem.BaseCurrency = lineItems.BaseCurrency;
            openingBalanceDetailLineItem.COAId = lineItems.COAId;
            openingBalanceDetailLineItem.BaseCredit = lineItems.BaseCredit;
            openingBalanceDetailLineItem.BaseDebit = lineItems.BaseDebit;
            openingBalanceDetailLineItem.DocumentCurrency = lineItems.DocCurrency;
            openingBalanceDetailLineItem.DocCredit = lineItems.DocCredit;
            openingBalanceDetailLineItem.DoCDebit = lineItems.DocDebit;
            openingBalanceDetailLineItem.ExchangeRate = lineItems.BaseCurrency == lineItems.DocCurrency ? 1.0000000000M : lineItems.ExchangeRate;
            openingBalanceDetailLineItem.Description = lineItems.Description;
            //openingBalanceDetailLineItem.SegmentMasterid1 = Convert.ToInt64(lineItems.SegmentMasterid1);
            //openingBalanceDetailLineItem.SegmentMasterid2 = Convert.ToInt64(lineItems.SegmentMasterid2);
            //openingBalanceDetailLineItem.SegmentCategory1 = lineItems.SegmentCategory1;
            //openingBalanceDetailLineItem.SegmentCategory2 = lineItems.SegmentCategory2;
            //openingBalanceDetailLineItem.SegmentDetailid1 = lineItems.SegmentDetailid1;
            //openingBalanceDetailLineItem.SegmentDetailid2 = lineItems.SegmentDetailid2;
            openingBalanceDetailLineItem.EntityId = lineItems.EntityId;
            openingBalanceDetailLineItem.DocumentReference = lineItems.DocumentReference;
            openingBalanceDetailLineItem.IsDisAllow = lineItems.IsDisAllow;
            openingBalanceDetailLineItem.DueDate = lineItems.DueDate;
            openingBalanceDetailLineItem.IsProcressed = false;
            openingBalanceDetailLineItem.Recorder = lineItems.RecOrder;

        }

        private void FillOpeningBalanceVmToEntity(OpeningBalanceDetailModel details, OpeningBalanceDetail openingBalanceDetail, OpeningBalanceModel TObject)
        {
            //openingBalanceDetail.Id = details.Id;
            openingBalanceDetail.OpeningBalanceId = TObject.Id;
            openingBalanceDetail.ModifiedBy = details.ModifiedBy;
            openingBalanceDetail.ModifiedDate = DateTime.UtcNow;
            openingBalanceDetail.UserCreated = details.UserCreated;
            openingBalanceDetail.CreatedDate = details.CreatedDate;
            openingBalanceDetail.COAId = details.COAId;
            openingBalanceDetail.BaseCurrency = details.BaseCurrency;
            openingBalanceDetail.BaseCredit = details.BaseCredit;
            openingBalanceDetail.BaseDebit = details.BaseDebit;
            openingBalanceDetail.DocCurrency = details.DocCurrency;
            openingBalanceDetail.DocCredit = details.DocCredit;
            openingBalanceDetail.DocDebit = details.DocDebit;
            openingBalanceDetail.ClearingDate = details.ClearingDate;
            openingBalanceDetail.ClearingState = details.ClearingState;
            openingBalanceDetail.ReconciliationDate = details.ReconciliationDate;
            openingBalanceDetail.ReconciliationId = details.ReconciliationId;
            openingBalanceDetail.Recorder = details.RecOrder;
        }

        private void deleteDetail(Guid Id)
        {
            OpeningBalanceDetail openingBalanceDetailE = _OpeningBalanceDetailService.CheckOpeningBalanceId(Id);
            if (openingBalanceDetailE != null)
            {
                var lstLineItems = _openingBalanceDetailLineItemService.GetLstLineItemById(Id);
                if (lstLineItems.Any())
                {
                    foreach (var item in lstLineItems)
                        deleteLineItem(item.Id);
                }
                _OpeningBalanceDetailService.Delete(openingBalanceDetailE);
                openingBalanceDetailE.ObjectState = ObjectState.Deleted;
            }
        }

        private void deleteLineItem(Guid Id)
        {
            OpeningBalanceDetailLineItem openingBalanceDetailLineItemE = _openingBalanceDetailLineItemService.GetLineItemById(Id);
            //_openingBalanceDetailLineItemService.Delete(openingBalanceDetailLineItemE);
            if (openingBalanceDetailLineItemE != null)
                openingBalanceDetailLineItemE.ObjectState = ObjectState.Deleted;
        }

        private void FillDetails(OpeningBalanceDetail openingBalanceDetail, OpeningBalanceDetailModel details, OpeningBalanceModel TObject)
        {
            openingBalanceDetail.COAId = details.COAId;
            openingBalanceDetail.BaseCurrency = details.BaseCurrency;
            openingBalanceDetail.BaseCredit = details.BaseCredit;
            openingBalanceDetail.BaseDebit = details.BaseDebit;
            openingBalanceDetail.DocCurrency = details.DocCurrency;
            openingBalanceDetail.DocCredit = details.DocCredit;
            openingBalanceDetail.DocDebit = details.DocDebit;
            openingBalanceDetail.UserCreated = details.UserCreated;
            openingBalanceDetail.CreatedDate = DateTime.UtcNow;
            openingBalanceDetail.IsOrginalAccount = details.IsOrginalAccount;
        }
        private void FillDetails1(OpeningBalanceDetail openingBalanceDetail, OpeningBalanceDetailModel details)
        {
            openingBalanceDetail.COAId = details.COAId;
            openingBalanceDetail.DocCurrency = details.DocCurrency;
            openingBalanceDetail.BaseCurrency = details.BaseCurrency;
        }

        #endregion SaveCall_Private_Block

        #endregion SaveCall_Private_Block

        #region AutoNumber Implimentation
        string value = "";
        public string GenerateAutoNumberForType(long companyId, string Type, string companyCode)
        {
            string generatedAutoNumber = "";
            try
            {
                AppsWorld.OpeningBalancesModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
                //AppsWorld.OpeningBalancesModule.Entities.BeanAutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);

                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
                    _autoNo.GeneratedNumber, companyId, companyCode);
                if (_autoNo != null)
                {
                    _autoNo.GeneratedNumber = value;
                    _autoNo.IsDisable = true;
                    _autoNumberService.Update(_autoNo);
                    _autoNo.ObjectState = ObjectState.Modified;
                }

                //Remove Common.AutonumberCompany, as it is not used in the autonumber generation stored procedure.
                /*var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
                if (_autonumberCompany.Any())
                {
                    AppsWorld.OpeningBalancesModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AppsWorld.OpeningBalancesModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.OpeningBalancesModule.Entities.AutoNumberCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }*/
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return generatedAutoNumber;
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
            long companyId, string Companycode = null)
        {
            List<OpeningBalance> lstOp = null;
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
                lstOp = _OpeningBalanceService.lstopeningbalance(companyId);
                if (lstOp.Any() && ifMonthContains == true)
                {
                    AppsWorld.OpeningBalancesModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    //AppsWorld.OpeningBalancesModule.Entities.BeanAutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastCretedDate = lstOp.Select(a => a.CreatedDate.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstOp.Select(a => a.CreatedDate.Year).FirstOrDefault())
                    {
                        if (lastCretedDate == currentMonth)
                        {
                            foreach (var bill in lstOp)
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
                            val = 1;
                    }
                    else
                        val = 1;
                }
                else if (lstOp.Any() && ifMonthContains == false)
                {
                    AppsWorld.OpeningBalancesModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    //AppsWorld.OpeningBalancesModule.Entities.BeanAutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var op in lstOp)
                    {
                        if (op.SystemRefNo != autonumber.Preview)
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

                if (lstOp.Any())
                {
                    OutputNumber = GetNewNumber(lstOp, Type, OutputNumber, counter, companyFormatHere, counterLength);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return OutputNumber;
        }
        private string GetNewNumber(List<OpeningBalance> lstCashsale, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstCashsale.Where(a => a.SystemRefNo == outputNumber).FirstOrDefault();
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
                    var inv = lstCashsale.Where(c => c.SystemRefNo == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }
        #endregion

        #region Posting
        public void SaveOBPostings(JVModel clientModel)
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
                //        if (section.Ziraff[i].Name == OpeningBalanceConstants.IdentityBean)
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
        public void saveOBInvoice(InvoiceModel clientModel, bool? isBill)
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
                //        if (section.Ziraff[i].Name == OpeningBalanceConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();

                var response = isBill == true ? RestSharpHelper.Post(url, "api/bill/savebill", json) : RestSharpHelper.Post(url, "api/invoice/saveinvoice", json);
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
        private void FillJournal(JVModel jvm, OpeningBalance _openingBalance, bool isNew, List<ChartOfAccount> lstCOA, string serviceEntityName)
        {
            //int count = 1;
            if (_openingBalance.SaveType == OpeningBalanceConstants.Save)
            {
                //string strServiceCompany = _companyService.Query(a => a.Id == _openingBalance.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                FillJV(jvm, _openingBalance);
                //var arid = _chartOfAccountService.GetChartOfAccountIDByName(COANameConstants.AccountsReceivables, _openingBalance.CompanyId);
                //var apid = _chartOfAccountService.GetChartOfAccountIDByName(COANameConstants.AccountsPayable, _openingBalance.CompanyId);
                //var orid = _chartOfAccountService.GetChartOfAccountIDByName(COANameConstants.OtherPayables, _openingBalance.CompanyId);
                //var opid = _chartOfAccountService.GetChartOfAccountIDByName(COANameConstants.OtherReceivables, _openingBalance.CompanyId);
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                JVVDetailModel jModel = new JVVDetailModel();
                List<long> coaIds = _chartOfAccountService.GetAllCOAIdsByIds(_openingBalance.OpeningBalanceDetails.Select(c => c.COAId).ToList());
                var lstOb = _openingBalance.OpeningBalanceDetails.Where(detail => detail.DocDebit > 0 || detail.DocCredit > 0 || detail.BaseDebit > 0 || detail.BaseCredit > 0 || _openingBalance.OpeningBalanceDetails.Select(c => c.OpeningBalanceDetailLineItems).Count() > 0).ToList();
                foreach (var detail in lstOb)
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    FillJVDetails(journal, detail);
                    journal.SystemRefNo = _openingBalance.SystemRefNo;
                    journal.DocNo = _openingBalance.SystemRefNo;
                    journal.DocumentId = _openingBalance.Id;
                    journal.PostingDate = _openingBalance.Date;
                    journal.DocDate = _openingBalance.Date;
                    journal.ServiceCompanyId = _openingBalance.ServiceCompanyId;
                    journal.RecOrder = detail.Recorder;
                    journal.AccountDescription = DocTypeConstants.OpeningBalance + "-" + serviceCompanyName;
                    //var acc = _chartOfAccountService.GetChartOfAccountById(detail.COAId);
                    journal.COAId = coaIds.Where(c => c == detail.COAId).FirstOrDefault();
                    ChartOfAccount arAcc = lstCOA.Where(c => c.Id == detail.COAId && (c.Name == COANameConstants.AccountsReceivables || c.Name == COANameConstants.AccountsPayable)).FirstOrDefault();
                    ChartOfAccount orAcc = lstCOA.Where(c => c.Id == detail.COAId && (c.Name == COANameConstants.OtherReceivables || c.Name == COANameConstants.OtherPayables)).FirstOrDefault();
                    journal.Nature = arAcc != null ? "Trade" : orAcc != null ? "Others" : null;
                    journal.ClearingDate = detail.ClearingDate;
                    journal.ClearingStatus = detail.ClearingState == "Reconciled" ? "Cleared" : detail.ClearingState;
                    journal.ReconciliationDate = detail.ReconciliationDate;
                    journal.ReconciliationId = detail.ReconciliationId;
                    journal.IsBankReconcile = detail.ClearingState == "Reconciled";
                    //if (arid != null && apid != null && orid != null && opid != null)
                    //{
                    //    if (journal.COAId == arid || journal.COAId == apid)
                    //        journal.Nature = "Trade";
                    //    if (journal.COAId == orid || journal.COAId == opid)
                    //        journal.Nature = "Others";
                    //}
                    foreach (var lineitem in detail.OpeningBalanceDetailLineItems)
                    {

                        JVVDetailModel journal1 = new JVVDetailModel();
                        FillJVLineDetails(journal1, lineitem);
                        journal1.Id = Guid.NewGuid();
                        journal1.DocumentId = _openingBalance.Id;
                        journal1.SystemRefNo = lineitem.DocumentReference != null ? lineitem.DocumentReference.Trim() == string.Empty ? _openingBalance.SystemRefNo : lineitem.DocumentReference : _openingBalance.SystemRefNo;
                        journal1.DocNo = lineitem.DocumentReference != null ? lineitem.DocumentReference.Trim() == string.Empty ? _openingBalance.SystemRefNo : lineitem.DocumentReference : _openingBalance.SystemRefNo;
                        journal1.DocumentDetailId = lineitem.Id;
                        journal1.ServiceCompanyId = _openingBalance.ServiceCompanyId;
                        //journal1.RecOrder = detail.Recorder + 1;
                        journal1.RecOrder = lineitem.Recorder;
                        journal1.AccountDescription = (lineitem.Description == null || lineitem.Description == string.Empty) ? DocTypeConstants.OpeningBalance + "-" + serviceCompanyName : lineitem.Description;
                        journal1.PostingDate = _openingBalance.Date;
                        journal1.DocDate = lineitem.Date;
                        journal1.Nature = arAcc != null ? (arAcc.Id == lineitem.COAId ? "Trade" : orAcc != null ? orAcc.Id == lineitem.COAId ? "Others" : null : null) : (orAcc != null ? orAcc.Id == lineitem.COAId ? "Others" : null : null);
                        //if (arid != null && apid != null && orid != null && opid != null)
                        //{
                        //    if (journal1.COAId == arid || journal1.COAId == apid)
                        //        journal1.Nature = "Trade";
                        //    if (journal1.COAId == orid || journal1.COAId == opid)
                        //        journal1.Nature = "Others";
                        //}
                        lstJD.Add(journal1);
                    }
                    if (!(detail.OpeningBalanceDetailLineItems != null && detail.OpeningBalanceDetailLineItems.Count > 0) && (detail.DocDebit > 0 || detail.DocCredit > 0 || detail.BaseDebit > 0 || detail.BaseCredit > 0))
                        lstJD.Add(journal);
                }
                jvm.GrandDocDebitTotal = Math.Round((decimal)lstJD.Sum(c => c.DocDebit), 2);
                jvm.GrandDocCreditTotal = Math.Round((decimal)lstJD.Sum(c => c.DocCredit), 2);
                jvm.GrandBaseDebitTotal = Math.Round((decimal)lstJD.Sum(c => c.BaseDebit), 2);
                jvm.GrandBaseCreditTotal = Math.Round((decimal)lstJD.Sum(c => c.BaseCredit), 2);
                jvm.JVVDetailModels = lstJD;
            }
        }

        private void FillJVLineDetails(JVVDetailModel journal1, OpeningBalanceDetailLineItem lineitem)
        {
            journal1.EntityId = lineitem.EntityId;
            journal1.COAId = lineitem.COAId;
            journal1.DocCurrency = lineitem.DocumentCurrency;

            if (lineitem.DoCDebit != null)
            {
                if (lineitem.DoCDebit < 0)
                {
                    journal1.DocCredit = -(lineitem.DoCDebit);
                    journal1.BaseCredit = -(lineitem.BaseDebit);
                }
                else
                {
                    journal1.DocDebit = lineitem.DoCDebit != null ? lineitem.DoCDebit : null;
                    journal1.BaseDebit = lineitem.BaseDebit != null ? lineitem.BaseDebit : null;
                }
            }
            if (lineitem.DocCredit != null)
            {
                if (lineitem.DocCredit < 0)
                {
                    journal1.DocDebit = -(lineitem.DocCredit);
                    journal1.BaseDebit = -(lineitem.BaseCredit);
                }
                else
                {
                    journal1.DocCredit = lineitem.DocCredit != null ? lineitem.DocCredit : null;
                    journal1.BaseCredit = lineitem.BaseCredit != null ? lineitem.BaseCredit : null;
                }
            }
            //journal1.DocDebit = lineitem.DoCDebit;
            //journal1.DocCredit = lineitem.DocCredit;
            //journal1.BaseDebit = lineitem.BaseDebit;
            //journal1.BaseCredit = lineitem.BaseCredit;
            journal1.BaseCurrency = lineitem.BaseCurrency;
            journal1.ExchangeRate = lineitem.ExchangeRate;
            journal1.DueDate = lineitem.DueDate;
            //journal1.SystemRefNo = lineitem.DocumentReference;
            //journal1.SegmentCategory1 = lineitem.SegmentCategory1;
            //journal1.SegmentCategory2 = lineitem.SegmentCategory2;
            //journal1.SegmentMasterid1 = lineitem.SegmentMasterid1;
            //journal1.SegmentMasterid2 = lineitem.SegmentMasterid2;
            //journal1.SegmentDetailid1 = lineitem.SegmentDetailid1;
            //journal1.SegmentDetailid2 = lineitem.SegmentDetailid2;
        }

        private void FillJVDetails(JVVDetailModel journal, OpeningBalanceDetail detail)
        {
            journal.Id = Guid.NewGuid();
            // journal.DocumentDetailId = detail.Id;
            journal.BaseCurrency = detail.BaseCurrency;
            journal.BaseCredit = detail.BaseCredit;
            journal.BaseDebit = detail.BaseDebit;
            journal.DocCurrency = detail.DocCurrency;
            journal.DocDebit = detail.DocDebit;
            journal.DocCredit = detail.DocCredit;
            journal.ExchangeRate = (detail.BaseDebit != null && detail.DocDebit != null && detail.BaseDebit > 0 && detail.DocDebit > 0) ? Math.Round((decimal)(detail.BaseDebit / detail.DocDebit), 10) : (detail.BaseCredit != null && detail.DocCredit != null && detail.BaseCredit > 0 && detail.DocCredit > 0) ? Math.Round((decimal)(detail.BaseCredit / detail.DocCredit), 10) : 0;
        }

        private void FillJV(JVModel jvm, OpeningBalance _openingBalance)
        {
            jvm.Id = Guid.NewGuid();
            jvm.CompanyId = _openingBalance.CompanyId;
            jvm.DocumentId = _openingBalance.Id;
            var postingDateByServiceCompany = _OpeningBalanceService.GetOpeningBalance(_openingBalance.CompanyId, _openingBalance.ServiceCompanyId);
            //var isMultiCurrency = _multiCurrencySettingService.GetByCompanyId(_openingBalance.CompanyId);
            jvm.IsMultiCurrency = _openingBalance.IsMultiCurrency;
            jvm.PostingDate = postingDateByServiceCompany != null ? postingDateByServiceCompany.Date : DateTime.UtcNow;
            jvm.DocType = "Journal";
            jvm.DocSubType = DocTypeConstants.OpeningBalance;
            //var isSegment = _segmentMasterService.GetSegmentMasterActive(_openingBalance.CompanyId);
            jvm.IsSegmentReporting = _openingBalance.IsSegmentReporting;
            jvm.ServiceCompanyId = _openingBalance.ServiceCompanyId;
            jvm.IsNoSupportingDocs = _openingBalance.IsNoSupportingDoc;
            jvm.DocDate = _openingBalance.Date;
            jvm.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            jvm.BaseCurrency = _openingBalance.BaseCurrency;
            jvm.DocumentState = OpeningBalanceConstants.Posted;
            jvm.CreatedDate = DateTime.UtcNow;
            jvm.UserCreated = _openingBalance.UserCreated;
            jvm.ModifiedBy = _openingBalance.ModifiedBy;
            jvm.ModifiedDate = _openingBalance.ModifiedDate;
            jvm.SystemReferenceNo = _openingBalance.SystemRefNo;
            //jvm.DocNo = DocTypeConstants.OpeningBalance;
            jvm.DocNo = _openingBalance.SystemRefNo;//for cindi new changes
        }

        public void deleteJVPostInvoce(JournalSaveModel tObject)
        {
            string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
            var json = RestSharpHelper.ConvertObjectToJason(tObject);

            //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
            //if (section.Ziraff.Count > 0)
            //{
            //    for (int i = 0; i < section.Ziraff.Count; i++)
            //    {
            //        if (section.Ziraff[i].Name == PaymentsConstants.IdentityBean)
            //        {
            //            url = section.Ziraff[i].ServerUrl;
            //            break;
            //        }
            //    }
            //}
            object obj = tObject;
            //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();

            try
            {
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                if (response.ErrorMessage != null)
                {
                    // Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                //try to call one more time here
                try
                {
                    var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                    var message = ex.Message;
                }
                catch (Exception exe)
                {
                }
            }
            #endregion

        }

    }
}


