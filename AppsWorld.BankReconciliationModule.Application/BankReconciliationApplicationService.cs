using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using AppsWorld.BankReconciliationModule.Service;
using AppsWorld.BankReconciliationModule.Models;
using Repository.Pattern.Infrastructure;
using AppsWorld.CommonModule.Infra;
using AppsWorld.ReceiptModule.Infra;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Entities;
using AppsWorld.Framework;
using Serilog;
using Logger;
using AppsWorld.BankReconciliationModule.Infra;
using Domain.Events;
using System.Data.Entity.Validation;
using Hangfire;
using System.Configuration;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using System.Data.SqlClient;
using AppaWorld.Bean;
using Newtonsoft.Json;
using System.Data;
using System.Drawing.Printing;

namespace AppsWorld.BankReconciliationModule.Application
{
    public class BankReconciliationApplicationService
    {

        public readonly IBankReconciliationService _bankReconciliationService;
        private readonly IBankReconciliationDetailService _bankReconciliationDetailService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IBeanEntityService _beanEntityService;
        private readonly ICompanyService _companyService;
        private readonly AppsWorld.BankReconciliationModule.Service.IJournalService _journalService;
        private readonly IBankReconciliationModuleUnitOfWorkAsync _unitOfWorkAsync;


        public BankReconciliationApplicationService(IBankReconciliationModuleUnitOfWorkAsync unitOfWorkAsync, IBankReconciliationService bankReconciliationService, IBankReconciliationDetailService BankReconciliationDetailService, IFinancialSettingService financialSettingService, IBeanEntityService beanEntityService, ICompanyService companyService, AppsWorld.BankReconciliationModule.Service.IJournalService journalService)
        {
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._bankReconciliationService = bankReconciliationService;
            this._bankReconciliationDetailService = BankReconciliationDetailService;
            this._beanEntityService = beanEntityService;
            this._companyService = companyService;
            this._financialSettingService = financialSettingService;
            this._journalService = journalService;

        }

        #region KendoGrid
        public IQueryable<BankReconciliationModelk> GetAllBankReconciliationsK(string username, long companyId)
        {
            return _bankReconciliationService.GetAllBankReconciliationsK(username, companyId);
        }
        public IQueryable<BankReconciliationDetailModel> GetClearingTransactionK(string username, long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime? toDate)
        {
            return _bankReconciliationService.GetClearingTransaction(username, companyId, subcompanyId, chartid, fromDate, toDate);
        }

        #endregion

        #region Lookup
        public BankReconciliationLu BankReconciliationLu(Guid id, long companyId, string userName)
        {
            BankReconciliationLu BankReconciliationLu = new BankReconciliationLu();
            //long? coaId = 0;
            AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
            if (financSettings == null)
            {
                throw new Exception(BankReconciliationValidation.The_Financial_setting_should_be_activated);
            }
            BankReconciliationLu.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
            BankReconciliationLu.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
            BankReconciliation bankReconciliation = _bankReconciliationService.GetBankReconciliation(id, companyId);
            //if (bankReconciliation != null)
            //{
            //    List<BankReconciliation> lstbank = _bankReconciliationService.GetAllBankById(id);
            //    foreach (var detail in lstbank)
            //    {
            //        if (detail.Status == RecordStatusEnum.Active)
            //        {
            //            long? coa = detail == null ? 0 : detail.COAId == null ? 0 : detail.COAId;
            //        }
            //    }
            //}
            //string AccountName = BankReconciliationLoggingValidation.Cash_and_bank_balances;
            //AccountType account = _accountTypeService.GetAccountTypeId(companyId, AccountName);

            //if (account != null)
            //{
            //List<ChartOfAccount> lstchart = _chartOfAccountService.lstchartofaccount(account.Id); //Commented by lokanath
            long comp = bankReconciliation == null ? 0 : bankReconciliation.ServiceCompanyId;

            //var lstCompany = _companyService.GetCompany(companyId, comp).Select(x => new LookUpCompany<string>()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    ShortName = x.ShortName,
            //    LookUps = lstchart.Where(c => c.SubsidaryCompanyId == x.Id && c.Status == RecordStatusEnum.Active).Select(a => new LookUp<string>()
            //    {
            //        Id = a.Id,
            //        Name = a.Name,
            //        Code = a.Code,
            //        Currency = a.Currency,
            //    }).ToList()
            //}).ToList();
            //var lstbankss=lstCompany.Select(x=>x.LookUps.Where(y=>y.Code==)).

            //subSidiary company changes
            List<long> coaIds = new List<long>();
            if (bankReconciliation != null)
                coaIds.Add(bankReconciliation.COAId);

            BankReconciliationLu.SubsideryCompanyLU = _companyService.Listofsubsidarycompany(companyId, comp, bankReconciliation != null ? coaIds : coaIds, id, userName);
            //BankReconciliationLu.CompanyId = account.CompanyId;

            //BankReconciliationLu.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
            //BankReconciliationLu.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
            //BankReconciliationLu.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
            //BankReconciliationLu.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
            BankReconciliationLu.CompanyId = companyId;
            return BankReconciliationLu;
        }
        #endregion

        #region Create

        public ClearingModel GetClearingTransaction(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate)
        {

            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Enter_into_GetClearingTransaction_method);
            ClearingModel model = new ClearingModel();
            model.Details = lstBankReconciliation(companyId, subcompanyId, chartid, fromDate, toDate, true, false);
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Come_out_from_GetClearingTransaction_method);
            return model;
        }

        //public BankReconciliationModel CreateBankReconciliation(Guid id, long companyId, long subcompanyId, long chartid, DateTime reconciledDate/*, DateTime toDate*/)
        //{
        //    LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Enter_into_the_Create_call_of_BankReconciliation);
        //    BankReconciliationModel master = new BankReconciliationModel();

        //    DateTime? lastRecDate = _bankReconciliationService.GetLastReocnciledDate(companyId, chartid, subcompanyId, id, reconciledDate, true);


        //    AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
        //    if (financSettings == null)
        //    {
        //        throw new Exception(BankReconciliationValidation.The_Financial_setting_should_be_activated);
        //    }
        //    master.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
        //    master.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;


        //    if (id == Guid.Empty)
        //    {
        //        //Get Last Reconsiled Date based on COAID, ServiceCompanyId and companyId to put Restriction on ClearingDate in Edit and Add mode
        //        master.LastReconciledDate = _bankReconciliationService.GetLastReocnciledDate(companyId, chartid, subcompanyId, id, reconciledDate, false);
        //        //FillNewBrs(id, companyId, subcompanyId, chartid, fromDate, toDate, master);
        //        //new Method for BRC in View Mode
        //        master.GLAmount = GetGLBalanceFromDetail(chartid, companyId, subcompanyId, reconciledDate);
        //        master.BankReconciliationDetailModels = fillBankReconcilaition(companyId, subcompanyId, chartid, reconciledDate, master, lastRecDate);
        //        master.BankReconciliationDetailModels = master.BankReconciliationDetailModels != null ? master.BankReconciliationDetailModels.OrderBy(a => a.DocumentDate).ToList() : null;
        //    }
        //    else
        //    {
        //        master.LastReconciledDate = _bankReconciliationService.GetLastReocnciledDate(companyId, chartid, subcompanyId, id, reconciledDate, false);
        //        //var bankre = _bankReconciliationService.Query(c => c.CompanyId == companyId && c.Id == id).Select().FirstOrDefault();
        //        LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Asign_the_value_to_BankReconciliationDetails);

        //        BankReconciliation bankre = _bankReconciliationService.GetBankReconciliationByIdandSIdAndCid(companyId, subcompanyId, chartid, reconciledDate, id);
        //        if (bankre != null)
        //        {
        //            master.Id = id;
        //            master.BankReconciliationDate = bankre.BankReconciliationDate;
        //            master.IsDraft = bankre.IsDraft;
        //            master.ClearingId = bankre.Id;
        //            master.CreatedDate = bankre.CreatedDate;
        //            master.UserCreated = bankre.UserCreated;
        //            master.COAId = bankre.COAId;
        //            master.ModifiedBy = bankre.ModifiedBy;
        //            master.ModifiedDate = bankre.ModifiedDate;
        //            master.ServiceCompanyId = bankre.ServiceCompanyId;
        //            master.StatementAmount = bankre.StatementAmount;
        //            master.State = bankre.State;
        //            master.Status = bankre.Status;
        //            master.Version = "0x" + string.Concat(Array.ConvertAll(bankre.Version, x => x.ToString("X2")));
        //            //master.GLAmount = bankre.GLAmount;
        //            //master.GLAmount = _journalService.GetGLBalance(master.COAId, bankre.CompanyId, bankre.ServiceCompanyId, reconciledDate);
        //            master.GLAmount = GetGLBalanceFromDetail(master.COAId, bankre.CompanyId, bankre.ServiceCompanyId, reconciledDate);
        //            master.IsLocked = bankre.IsLocked;
        //            List<BankReconciliationDetailModel> detailModel = new List<BankReconciliationDetailModel>();
        //            if (bankre.BankReconciliationDetails.Any())
        //            {
        //                List<Guid?> lstEntityId = bankre.BankReconciliationDetails.Select(a => a.EntityId).ToList();
        //                lstEntityId.Remove(null);
        //                Dictionary<Guid, string> lstofEntity = _beanEntityService.GetListOfEntity(companyId, lstEntityId);
        //                //detailModel = bankre.BankReconciliationDetails.Where(a => a.ClearingDate > master.BankReconciliationDate).Select(a => new BankReconciliationDetailModel()
        //                detailModel = bankre.BankReconciliationDetails.Select(a => new BankReconciliationDetailModel()
        //                {
        //                    Id = a.Id,
        //                    BankReconciliationId = a.BankReconciliationId,
        //                    EntityId = a.EntityId,
        //                    EntityName = a.EntityId != null ? lstofEntity.Where(c => c.Key == a.EntityId).Select(x => x.Value).FirstOrDefault() : null,
        //                    DocumentType = a.DocumentType,
        //                    DocumentDate = a.DocumentDate,
        //                    isWithdrawl = a.isWithdrawl,
        //                    DocRefNo = a.DocRefNo,
        //                    Ammount = a.Ammount,
        //                    //BaseAmount = a.Ammount > 0 ? a.Ammount : -(a.Ammount),
        //                    DocumentId = a.DocumentId,
        //                    ClearingDate = a.ClearingDate,
        //                    ClearingStatus = a.ClearingStatus,
        //                    IsDisable = a.IsDisable,
        //                    IsChecked = a.IsChecked,
        //                    COAId = master.COAId,
        //                    RecordStatus = null,
        //                    IsUncleared = (a.ClearingDate == null || a.ClearingDate > reconciledDate),
        //                    ServiceEntityId = master.ServiceCompanyId,
        //                    Mode = a.Mode,
        //                    RefNo = a.RefNo,
        //                    DocSubType = a.DocSubType,
        //                    JournalDetailId = a.JournalDetailId
        //                }).ToList();

        //                //List<Guid?> lstDocumentId = _journalService.GetListJournalDetailIds(subcompanyId, chartid, reconciledDate, master.LastReconciledDate);
        //                List<Guid> lstBrcDocId = bankre.BankReconciliationDetails.Select(a => a.DocumentId).ToList();
        //                List<JournalDetail> lstJournalDetail = _journalService.GetlstJournalDetailByCoaId(subcompanyId, chartid, reconciledDate, /*master.LastReconciledDate*/lastRecDate);
        //                List<JournalDetail> lstJournalDetails = lstJournalDetail.Where(a => !lstBrcDocId.Contains(a.DocumentId.Value) /*&& (a.ClearingDate == null || a.ClearingDate > master.BankReconciliationDate)*/).ToList();
        //                if (lstJournalDetails.Any())
        //                {
        //                    List<BankReconciliationDetailModel> detailModel1 = new List<BankReconciliationDetailModel>();
        //                    //detailModel = fillBankReconcilaition(companyId, subcompanyId, chartid, reconciledDate, master);
        //                    //if (detailModel.Any())
        //                    //{
        //                    //    foreach (var brcDeatil in detailModel)
        //                    //    {
        //                    //        BankReconciliationDetailModel detail = master.BankReconciliationDetailModels.Where(c => c.DocumentId == brcDeatil.DocumentId).FirstOrDefault();
        //                    //        if (detail == null)
        //                    //            master.BankReconciliationDetailModels.Add(detail);
        //                    //    }
        //                    //}

        //                    lstJournalDetails = (from jd in lstJournalDetails
        //                                         join j in _journalService.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
        //                                         where (j.DocumentState != "Void" && j.DocumentState != "Reset" && j.DocumentState != "Recurring" && j.DocumentState != "Parked")
        //                                         select jd).ToList();

        //                    List<AppsWorld.BankReconciliationModule.Entities.Journal> lstJournals = _journalService.GetListOfJournalByJournalId(companyId, lstJournalDetail != null ? lstJournalDetail.Select(a => a.JournalId).ToList() : new List<Guid>());

        //                    List<Guid?> lstEntityIds = lstJournalDetails.Select(a => a.EntityId).ToList();
        //                    lstEntityIds.Remove(null);
        //                    Dictionary<Guid, string> lstofEntitys = _beanEntityService.GetListOfEntity(companyId, lstEntityIds);
        //                    bool? isWithDrawal = false;

        //                    if (lstJournalDetail != null)
        //                    {
        //                        detailModel1 = lstJournalDetails.Select(a => new BankReconciliationDetailModel()
        //                        {
        //                            DocumentDate = a.DocDate,
        //                            DocumentType = a.DocType,
        //                            DocRefNo = a.DocNo,
        //                            isWithdrawl = isWithDrawal = (a.DocType == DocTypeConstants.BillPayment
        //                                   || a.DocType == BankReconciliationValidation.Withdrawal || a.DocType == BankReconciliationValidation.CashPayments || (a.DocType == DocTypeConstants.BankTransfer && a.Type == "Withdrawal") || (a.DocType == DocTypeConstants.JournalVocher && a.DocCredit != null)) ? true : false,
        //                            EntityId = /*a.EntityId != null ? lstofEntity.Where(c => c.Key == a.EntityId).Select(x=>x.Key).FirstOrDefault() : a.EntityId*/a.EntityId,
        //                            EntityName = a.EntityId != null ? lstofEntitys.Where(c => c.Key == a.EntityId).Select(x => x.Value).FirstOrDefault() : null,
        //                            ClearingDate = a.ClearingDate,
        //                            //Ammount = isWithDrawal == true ? -(a.BaseDebit != null ? a.BaseDebit : a.BaseCredit) : (a.BaseDebit != null ? a.BaseDebit : a.BaseCredit),
        //                            Ammount = isWithDrawal == true ? -(a.DocDebit != null ? a.DocDebit : a.DocCredit) : (a.DocDebit != null ? a.DocDebit : a.DocCredit),
        //                            //BaseAmount = a.BaseDebit != null ? a.BaseDebit : a.BaseCredit,
        //                            DocumentId = a.DocumentId,
        //                            JournalId = a.Id,
        //                            DocSubType = a.DocSubType,
        //                            IsChecked = a.ClearingDate != null ? true : false,
        //                            COAId = master.COAId,
        //                            IsUncleared = (a.ClearingDate == null || a.ClearingDate > reconciledDate),
        //                            RecordStatus = null,
        //                            isClearedTab = a.ClearingDate != null ? (a.ReconciliationId == null || a.IsBankReconcile == null) : false,
        //                            ServiceEntityId = master.ServiceCompanyId,
        //                            Mode = lstJournals.Any() ? lstJournals.Where(c => c.Id == a.JournalId).Select(c => c.ModeOfReceipt).FirstOrDefault() : null,
        //                            RefNo = lstJournals.Any() ? lstJournals.Where(c => c.Id == a.JournalId).Select(c => c.TransferRefNo).FirstOrDefault() : null,
        //                            JournalDetailId = a.Id
        //                        }).ToList();
        //                    }
        //                    detailModel.AddRange(detailModel1);

        //                    //detailModel = detailModel.Where(a => a.ClearingDate > master.BankReconciliationDate).ToList();
        //                }
        //                //decimal? clearedDeposit = detailModel.Any() ? Math.Round((decimal)detailModel.Where(a => a.ClearingDate != null && a.isWithdrawl != true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;
        //                //decimal? clearedWithdrawal = detailModel.Any() ? Math.Round((decimal)detailModel.Where(a => a.ClearingDate != null && a.isWithdrawl == true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;
        //                //decimal? outStandingDeposit = detailModel.Any() ? Math.Round((decimal)detailModel.Where(a => a.ClearingDate == null && a.isWithdrawl != true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;
        //                //decimal? outStandingWithdrawal = detailModel.Any() ? Math.Round((decimal)detailModel.Where(a => a.ClearingDate == null && a.isWithdrawl == true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;

        //                //master.GLAmount = (outStandingDeposit + clearedDeposit) - (outStandingWithdrawal + clearedWithdrawal);

        //                master.BankReconciliationDetailModels = detailModel.Where(a => a.DocumentDate <= master.BankReconciliationDate /*&& (a.ClearingDate == null || a.ClearingDate > master.BankReconciliationDate)*/).OrderBy(a => a.DocumentDate).ToList();

        //                #region oldCommentedCode

        //                //master.BankReconciliationDetailModels = ClearingRecords(id, companyId, subcompanyId, chartid, master);
        //                //List<BankReconciliationDetailModel> lst = lstBankReconciliation(companyId, subcompanyId, chartid, reconciledDate, toDate, false, false);
        //                //if (master.BankReconciliationDetailModels.Any())
        //                //{
        //                //    if (lst.Any())
        //                //    {
        //                //        foreach (var detail1 in lst)
        //                //        {
        //                //            BankReconciliationDetailModel detail = master.BankReconciliationDetailModels.Where(c => c.DocumentId == detail1.DocumentId).FirstOrDefault();
        //                //            if (detail == null)
        //                //                master.BankReconciliationDetailModels.Add(detail1);
        //                //        }
        //                //    }
        //                //}
        //                //else
        //                //{
        //                //    if (lst.Any())
        //                //    {
        //                //        master.BankReconciliationDetailModels = lst;
        //                //    }
        //                //}
        //                //master.BankReconciliationDetailModels.AddRange(lst);
        //                //FillBrs(companyId, subcompanyId, chartid, master, bankre, reconciledDate, toDate);
        //                #endregion
        //            }
        //            else
        //            {
        //                master.BankReconciliationDetailModels = fillBankReconcilaition(companyId, subcompanyId, chartid, reconciledDate, master, lastRecDate);
        //                master.BankReconciliationDetailModels = master.BankReconciliationDetailModels != null ? master.BankReconciliationDetailModels.OrderBy(a => a.DocumentDate).ToList() : null;
        //                //master.BankReconciliationDetailModels = master.BankReconciliationDetailModels != null ? master.BankReconciliationDetailModels.Where(a => a.ClearingDate > reconciledDate || a.ClearingDate == null).ToList() : null;
        //            }
        //        }
        //        //master.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
        //        //master.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
        //        //master.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
        //        //master.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
        //        Log.Logger.ZInfo(BankReconciliationLoggingValidation.Come_out_from_CreateBankReconciliation_method);
        //    }
        //    return master;
        //}
        //public async Task<BankReconciliationModel> CreateBankReconciliationNew(Guid id, long companyId, long subcompanyId, long COAId, DateTime reconciledDate, string connectionString/*, DateTime toDate*/)
        //{
        //    LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Enter_into_the_Create_call_of_BankReconciliation);
        //    BankReconciliationModel master = new BankReconciliationModel();
        //    DateTime? lastRecDate = _bankReconciliationService.GetLastReocnciledDate(companyId, COAId, subcompanyId, id, reconciledDate, true);
        //    AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
        //    if (financSettings == null)
        //    {
        //        throw new InvalidOperationException(BankReconciliationValidation.The_Financial_setting_should_be_activated);
        //    }
        //    master.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
        //    master.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;

        //    if (id == Guid.Empty)
        //    {
        //        //Get Last Reconsiled Date based on COAID, ServiceCompanyId and companyId to put Restriction on ClearingDate in Edit and Add mode
        //        master.LastReconciledDate = _bankReconciliationService.GetLastReocnciledDate(companyId, COAId, subcompanyId, id, reconciledDate, false);
        //        //new Method for BRC in View Mode
        //        //master.GLAmount = await Task.Run(()=>GetGLBalanceFromDetail(chartid, companyId, subcompanyId, reconciledDate));
        //        master.GLAmount = await _journalService.GetGLBalance(COAId, companyId, subcompanyId, reconciledDate);
        //        master.ServiceCompanyId = subcompanyId;
        //        master.COAId = COAId;
        //        master.CompanyId = companyId;
        //        master.BankReconciliationDate = reconciledDate;
        //    }
        //    else
        //    {
        //        master.LastReconciledDate = _bankReconciliationService.GetLastReocnciledDate(companyId, COAId, subcompanyId, id, reconciledDate, false);
        //        LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Asign_the_value_to_BankReconciliationDetails);
        //        BankReconciliation bankre = _bankReconciliationService.GetBankReconciliationByIdandSIdAndCid(companyId, subcompanyId, COAId, reconciledDate, id);
        //        if (bankre != null)
        //        {
        //            master.Id = id;
        //            master.BankReconciliationDate = bankre.BankReconciliationDate;
        //            master.CompanyId = companyId;
        //            master.IsDraft = bankre.IsDraft;
        //            master.ClearingId = bankre.Id;
        //            master.CreatedDate = bankre.CreatedDate;
        //            master.UserCreated = bankre.UserCreated;
        //            master.COAId = bankre.COAId;
        //            master.ModifiedBy = bankre.ModifiedBy;
        //            master.ModifiedDate = bankre.ModifiedDate;
        //            master.ServiceCompanyId = bankre.ServiceCompanyId;
        //            master.StatementAmount = bankre.StatementAmount;
        //            master.State = bankre.State;
        //            master.Status = bankre.Status;
        //            master.Version = "0x" + string.Concat(Array.ConvertAll(bankre.Version, x => x.ToString("X2")));
        //            //master.GLAmount = await Task.Run(()=>GetGLBalanceFromDetail(master.COAId, bankre.CompanyId, bankre.ServiceCompanyId, reconciledDate));
        //            master.GLAmount = await _journalService.GetGLBalance(COAId, companyId, subcompanyId, reconciledDate);
        //            master.IsLocked = bankre.IsLocked;
        //        }
        //        Log.Logger.ZInfo(BankReconciliationLoggingValidation.Come_out_from_CreateBankReconciliation_method);
        //    }
        //    string brcDate = string.Format("{0:MM/dd/yyyy}", reconciledDate);

        //    #region OLD_CODE Commented
        //    //string query = null;
        //    //if (lastRecDate != null)
        //    //{
        //    //    string lastReconDate = String.Format("{0:MM/dd/yyyy}", lastRecDate);
        //    //    query = $"Select  JD.Id as JournalId,jD.PostingDate as PostingDate,JD.DocType,JD.DocSubType,JD.DocNo,JD.EntityId,ENT.Name,isnull(JD.DocDebit,0) DocDebit,isnull(JD.DocCredit,0) DocCredit,JD.DocumentId,JD.COAID,JD.ServiceCompanyId,J.ModeOfReceipt,J.TransferRefNo,Jd.Type,JD.ClearingDate  from Bean.Journal J Join Bean.JournalDetail JD on J.Id = Jd.JournalId join Bean.ChartOfAccount COA on JD.COAId = COA.Id and COA.IsBank = 1 Left Join Bean.Entity Ent on Ent.Id = JD.EntityId where J.CompanyId = {companyId} and J.DocumentState Not In('Void', 'Recurring', 'Deleted', 'Reset', 'Parked') and JD.COAId = {COAId} and JD.ServiceCompanyId = {subcompanyId} and(JD.PostingDate <= '{brcDate}') and JD.ClearingDate is null Union ALL Select JD.Id as JournalId,jD.PostingDate as PostingDate,JD.DocType,JD.DocSubType,JD.DocNo,JD.EntityId,ENT.Name,jd.DocDebit,JD.DocCredit,JD.DocumentId,JD.COAID,JD.ServiceCompanyId,J.ModeOfReceipt,J.TransferRefNo,Jd.Type,JD.ClearingDate from Bean.Journal J Join Bean.JournalDetail JD on J.Id = Jd.JournalId join Bean.ChartOfAccount COA on JD.COAId = COA.Id and COA.IsBank = 1 Left Join Bean.Entity Ent on Ent.Id = JD.EntityId where J.CompanyId = {companyId} and J.DocumentState Not In('Void', 'Recurring', 'Deleted', 'Reset', 'Parked') and JD.COAId = {COAId} and JD.ServiceCompanyId = {subcompanyId} and(JD.PostingDate <= '{brcDate}') and (JD.ClearingDate is not null  AND(JD.ClearingDate > '{brcDate}' or JD.ClearingDate > '{lastReconDate}'))";
        //    //    //query = $"Select  JD.Id as JournalId,jD.PostingDate as PostingDate,JD.DocType,JD.DocSubType,JD.DocNo,JD.EntityId,ENT.Name,jd.DocDebit,JD.DocCredit,JD.DocumentId,JD.COAID,JD.ServiceCompanyId,J.ModeOfReceipt,J.TransferRefNo,Jd.Type,JD.ClearingDate  from Bean.Journal J Join Bean.JournalDetail JD on J.Id = Jd.JournalId join Bean.ChartOfAccount COA on JD.COAId = COA.Id and COA.IsBank = 1 Left Join Bean.Entity Ent on Ent.Id = JD.EntityId where J.CompanyId = {companyId} and J.DocumentState Not In('Void', 'Recurring', 'Deleted', 'Reset', 'Parked') and JD.COAId = {chartid} and JD.ServiceCompanyId = {subcompanyId} and(JD.PostingDate <= '{brcDate}' and JD.PostingDate >= '{lastReconDate}') and (JD.ClearingDate is null  or(JD.ClearingDate > '{brcDate}' or JD.ClearingDate > '{lastReconDate}'))";
        //    //}
        //    //else
        //    //{
        //    //    query = $"Select  JD.Id as JournalId,jD.PostingDate as PostingDate,JD.DocType,JD.DocSubType,JD.DocNo,JD.EntityId,ENT.Name,isnull(jd.DocDebit,0) DocDebit,isnull(JD.DocCredit,0) DocCredit,JD.DocumentId,JD.COAID,JD.ServiceCompanyId,J.ModeOfReceipt,J.TransferRefNo,Jd.Type,JD.ClearingDate  from Bean.Journal J Join Bean.JournalDetail JD on J.Id = Jd.JournalId join Bean.ChartOfAccount COA on JD.COAId = COA.Id and COA.IsBank = 1 Left Join Bean.Entity Ent on Ent.Id = JD.EntityId where J.CompanyId = {companyId} and J.DocumentState Not In('Void', 'Recurring', 'Deleted', 'Reset', 'Parked') and JD.COAId = {COAId} and JD.ServiceCompanyId = {subcompanyId} and(JD.PostingDate <= '{brcDate}')";
        //    //}
        //    //List<BankReconciliationDetailModel> lstDetailModel = new List<BankReconciliationDetailModel>();
        //    //bool isCredit = false;
        //    //string type = string.Empty;
        //    //using (SqlConnection con = new SqlConnection(connectionString))
        //    //{
        //    //    if (con.State != System.Data.ConnectionState.Open)
        //    //        con.Open();
        //    //    SqlCommand cmd = new SqlCommand(query, con);
        //    //    cmd.CommandType = System.Data.CommandType.Text;
        //    //    SqlDataReader dr = cmd.ExecuteReader();
        //    //    while (dr.Read())
        //    //    {
        //    //        BankReconciliationDetailModel detailModel = new BankReconciliationDetailModel();
        //    //        detailModel.Id = Guid.NewGuid();
        //    //        detailModel.JournalId = dr["JournalId"] != DBNull.Value ? Guid.Parse(dr["JournalId"].ToString()) : Guid.Empty;
        //    //        detailModel.DocumentDate = dr["PostingDate"] != DBNull.Value ? Convert.ToDateTime(dr["PostingDate"]) : (DateTime?)null;
        //    //        detailModel.DocumentType = Convert.ToString(dr["DocType"]);
        //    //        detailModel.DocSubType = Convert.ToString(dr["DocSubType"]);
        //    //        detailModel.DocRefNo = Convert.ToString(dr["DocNo"]);
        //    //        detailModel.EntityId = dr["EntityId"] != DBNull.Value ? Guid.Parse(dr["EntityId"].ToString()) : (Guid?)null;
        //    //        detailModel.EntityName = Convert.ToString(dr["Name"]);
        //    //        type = Convert.ToString(dr["Type"]);
        //    //        isCredit = dr["DocCredit"] != DBNull.Value && (decimal?)(dr["DocCredit"]) != 0;
        //    //        detailModel.isWithdrawl = detailModel.DocumentType == DocTypeConstants.BillPayment
        //    //                       || detailModel.DocumentType == BankReconciliationValidation.Withdrawal || detailModel.DocumentType == BankReconciliationValidation.CashPayments || (detailModel.DocumentType == DocTypeConstants.BankTransfer && type == "Withdrawal") || (detailModel.DocumentType == DocTypeConstants.JournalVocher && isCredit);

        //    //        detailModel.Ammount = (dr["DocCredit"] == DBNull.Value && dr["DocDebit"] == DBNull.Value) ? 0 : isCredit ? detailModel.isWithdrawl == true ? -Convert.ToDecimal(dr["DocCredit"]) : Convert.ToDecimal(dr["DocCredit"]) : detailModel.isWithdrawl == true ? -Convert.ToDecimal(dr["DocDebit"]) : Convert.ToDecimal(dr["DocDebit"]);
        //    //        detailModel.DocumentId = dr["DocumentId"] != DBNull.Value ? Guid.Parse(dr["DocumentId"].ToString()) : (Guid?)null;
        //    //        detailModel.COAId = dr["COAID"] != DBNull.Value ? Convert.ToInt64(dr["COAID"]) : (long?)null;
        //    //        detailModel.ServiceEntityId = dr["ServiceCompanyId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceCompanyId"]) : (long?)null;
        //    //        detailModel.Mode = Convert.ToString(dr["ModeOfReceipt"]);
        //    //        detailModel.RefNo = Convert.ToString(dr["TransferRefNo"]);
        //    //        detailModel.ClearingDate = dr["ClearingDate"] != DBNull.Value ? Convert.ToDateTime(dr["ClearingDate"]) : (DateTime?)null;
        //    //        detailModel.IsChecked = detailModel.ClearingDate != null;
        //    //        detailModel.RecordStatus = null;
        //    //        detailModel.IsUncleared = (detailModel.ClearingDate == null || detailModel.ClearingDate > reconciledDate);
        //    //        detailModel.JournalDetailId = detailModel.JournalId;
        //    //        lstDetailModel.Add(detailModel);
        //    //    }
        //    //    if (con.State != System.Data.ConnectionState.Closed)
        //    //        con.Close();
        //    //}
        //    #endregion OLD_CODE

        //    #region New Code
        //    List<BankReconciliationDetailModel> lstDetailModel = null;
        //    bool IsLastRecDate = lastRecDate != null;
        //    string lastReconDate = string.Empty;
        //    if (IsLastRecDate)
        //        lastReconDate = $"{lastRecDate:MM/dd/yyyy}";
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        SqlDataAdapter sqlDataAdapter;
        //        DataSet dataSet = new DataSet();
        //        if (con.State != ConnectionState.Open)
        //            con.Open();
        //        sqlDataAdapter = new SqlDataAdapter("GetAllBankReconciliationData_proc", con);
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@SubCompanyId", subcompanyId);
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@LastReconDate", lastReconDate);
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@COAId", COAId);
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@ReconciliationDate", brcDate);
        //        sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@IsLastRecon", IsLastRecDate);
        //        sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
        //        sqlDataAdapter.SelectCommand.CommandTimeout = 0;
        //        sqlDataAdapter.Fill(dataSet);
        //        DataTable dt = dataSet.Tables[0];
        //        con.Close();
        //        string JSONbankreconsile = JsonConvert.SerializeObject(dt);
        //        lstDetailModel = JsonConvert.DeserializeObject<List<BankReconciliationDetailModel>>(JSONbankreconsile);
        //    }
        //    #endregion New Code

        //    master.BankReconciliationDetailModels = lstDetailModel.Any() ? lstDetailModel.OrderBy(a => a.DocumentDate).ToList() : null;
        //    return master;
        //}

        public MasterModel CreateBankReconciliationDetails(Guid id, long companyId, long subcompanyId, long COAId, DateTime reconciledDate, bool IsClearedTab, string connectionString, int pageIndex, int pageSize)
        {
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Enter_into_the_Create_call_of_BankReconciliation);
            DateTime? lastRecDate = _bankReconciliationService.GetLastReocnciledDate(companyId, COAId, subcompanyId, id, reconciledDate, true);
            string brcDate = string.Format("{0:MM/dd/yyyy}", reconciledDate);
            List<BankReconciliationDetailModel> lstDetailModel = null;
            bool IsLastRecDate = lastRecDate != null;
            string lastReconDate = string.Empty;
            if (IsLastRecDate)
                lastReconDate = $"{lastRecDate:MM/dd/yyyy}";
            int recCount = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter sqlDataAdapter;
                DataSet dataSet = new DataSet();
                if (con.State != ConnectionState.Open)
                    con.Open();
                sqlDataAdapter = new SqlDataAdapter("GetAllBankReconciliationData_proc_v1", con);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@CompanyId", companyId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@SubCompanyId", subcompanyId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@LastReconDate", lastReconDate);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@COAId", COAId);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@ReconciliationDate", brcDate);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@IsLastRecon", IsLastRecDate);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@IsClearedTab", IsClearedTab);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@PageIndex", pageIndex);
                sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@PageSize", pageSize);
                sqlDataAdapter.SelectCommand.Parameters.Add("@RecordCount", SqlDbType.VarChar, 30);
                sqlDataAdapter.SelectCommand.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDataAdapter.SelectCommand.CommandTimeout = 0;
                sqlDataAdapter.Fill(dataSet);
                DataTable dt = dataSet.Tables[0];
                recCount = Convert.ToInt32(sqlDataAdapter.SelectCommand.Parameters["@RecordCount"].Value);
                con.Close();
                string JSONbankreconsile = JsonConvert.SerializeObject(dt);
                lstDetailModel = JsonConvert.DeserializeObject<List<BankReconciliationDetailModel>>(JSONbankreconsile);
            }

            return new MasterModel
            {
                BankReconciliationDetailModels = lstDetailModel.Any() ? lstDetailModel.OrderByDescending(a => a.DocumentDate).AsQueryable() : Enumerable.Empty<BankReconciliationDetailModel>().AsQueryable(),
                RecCount = recCount
            };

        }
        public async Task<BankReconciliationModel> CreateBankReconciliationMaster(Guid id, long companyId, long subcompanyId, long COAId, DateTime reconciledDate)
        {
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Enter_into_the_Create_call_of_BankReconciliation);
            BankReconciliationModel master = new BankReconciliationModel();
            AppsWorld.CommonModule.Entities.FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
            if (financSettings == null)
            {
                throw new InvalidOperationException(BankReconciliationValidation.The_Financial_setting_should_be_activated);
            }
            master.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
            master.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;

            if (id == Guid.Empty)
            {
                //Get Last Reconsiled Date based on COAID, ServiceCompanyId and companyId to put Restriction on ClearingDate in Edit and Add mode
                master.LastReconciledDate = _bankReconciliationService.GetLastReocnciledDate(companyId, COAId, subcompanyId, id, reconciledDate, false);
                //new Method for BRC in View Mode
                //master.GLAmount = await Task.Run(()=>GetGLBalanceFromDetail(chartid, companyId, subcompanyId, reconciledDate));
                master.GLAmount = await _journalService.GetGLBalance(COAId, companyId, subcompanyId, reconciledDate);
                master.ServiceCompanyId = subcompanyId;
                master.COAId = COAId;
                master.CompanyId = companyId;
                master.BankReconciliationDate = reconciledDate;
            }
            else
            {
                master.LastReconciledDate = _bankReconciliationService.GetLastReocnciledDate(companyId, COAId, subcompanyId, id, reconciledDate, false);
                LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Asign_the_value_to_BankReconciliationDetails);
                BankReconciliation bankre = _bankReconciliationService.GetBankReconciliationByIdandSIdAndCid(companyId, subcompanyId, COAId, reconciledDate, id);
                if (bankre != null)
                {
                    master.Id = id;
                    master.BankReconciliationDate = bankre.BankReconciliationDate;
                    master.CompanyId = companyId;
                    master.IsDraft = bankre.IsDraft;
                    master.ClearingId = bankre.Id;
                    master.CreatedDate = bankre.CreatedDate;
                    master.UserCreated = bankre.UserCreated;
                    master.COAId = bankre.COAId;
                    master.ModifiedBy = bankre.ModifiedBy;
                    master.ModifiedDate = bankre.ModifiedDate;
                    master.ServiceCompanyId = bankre.ServiceCompanyId;
                    master.StatementAmount = bankre.StatementAmount;
                    master.State = bankre.State;
                    master.Status = bankre.Status;
                    master.StatementExpectedAmount = bankre.StatementExpectedAmount;
                    master.Version = "0x" + string.Concat(Array.ConvertAll(bankre.Version, x => x.ToString("X2")));
                    //master.GLAmount = await Task.Run(()=>GetGLBalanceFromDetail(master.COAId, bankre.CompanyId, bankre.ServiceCompanyId, reconciledDate));
                    master.GLAmount = await _journalService.GetGLBalance(COAId, companyId, subcompanyId, reconciledDate);
                    master.IsLocked = bankre.IsLocked;
                }
                Log.Logger.ZInfo(BankReconciliationLoggingValidation.Come_out_from_CreateBankReconciliation_method);
            }
            return master;
        }
        private void FillNewBrs(Guid id, long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate,
            BankReconciliationModel master)
        {
            decimal TotalClearedDepprevtran = 0;
            decimal TotalClearedWithprevtran = 0;
            master.CompanyId = companyId;
            master.ServiceCompanyId = subcompanyId;
            //var scn = _companyService.GetById(subcompanyId);
            //master.ServiceCompanyName = scn.Name;
            master.BankReconciliationDetailModels = lstBankReconciliation(companyId, subcompanyId, chartid, fromDate, toDate,
                false, false);
            foreach (var Brdetail in master.BankReconciliationDetailModels)
            {
                //Brdetail.isWithdrawl = (Brdetail.DocumentType == BankReconciliationValidation.payments || Brdetail.DocumentType == BankReconciliationValidation.Payroll_Bill || Brdetail.DocumentType == BankReconciliationValidation.Payroll_Payment ||
                //                       Brdetail.DocumentType == BankReconciliationValidation.Bill ||
                //                       (Brdetail.DocumentType == BankReconciliationValidation.CreditNote && Brdetail.DocSubType == "CreditNote") || Brdetail.DocumentType == BankReconciliationValidation.Withdrawal || Brdetail.DocumentType == BankReconciliationValidation.CashPayments ||
                //                       (Brdetail.DocumentType == BankReconciliationValidation.Bank_Transfer && Brdetail.Withdrawal == true));

                Brdetail.isWithdrawl = (Brdetail.DocumentType == BankReconciliationValidation.payments || Brdetail.DocumentType == BankReconciliationValidation.Payroll_Payment
                                       || Brdetail.DocumentType == BankReconciliationValidation.Withdrawal || Brdetail.DocumentType == BankReconciliationValidation.CashPayments ||
                                       (Brdetail.DocumentType == BankReconciliationValidation.Bank_Transfer && Brdetail.Withdrawal == true));
                Brdetail.Ammount = Brdetail.Ammount ?? 0;
            }
            //master.OutstandingWithdrawals =
            //    master.BankReconciliationDetailModels.Where(
            //            c => (c.DocumentType == BankReconciliationValidation.payments || c.DocumentType == BankReconciliationValidation.Payroll_Bill || c.DocumentType == BankReconciliationValidation.Payroll_Payment ||
            //                           c.DocumentType == BankReconciliationValidation.Bill ||
            //                           (c.DocumentType == BankReconciliationValidation.CreditNote && c.DocSubType == "CreditNote") || c.DocumentType == BankReconciliationValidation.Withdrawal || c.DocumentType == BankReconciliationValidation.CashPayments ||
            //                           (c.DocumentType == BankReconciliationValidation.Bank_Transfer && c.Withdrawal == true)))
            //        .Sum(a => a.Ammount.Value);
            //master.OutstandingDeposits =
            //    master.BankReconciliationDetailModels.Where(
            //            c =>
            //                c.DocumentType == BankReconciliationValidation.receipt ||
            //            c.DocumentType == BankReconciliationValidation.CreditMemo ||
            //                c.DocumentType == BankReconciliationValidation.Invoice ||
            //                c.DocumentType == BankReconciliationValidation.DebitNote ||
            //                c.DocumentType == BankReconciliationValidation.DoubtfulDebt ||
            //                c.DocumentType == BankReconciliationValidation.deposit ||
            //                c.DocumentType == BankReconciliationValidation.CashSale || c.DocumentType == BankReconciliationValidation.Journal ||
            //                (c.DocumentType == BankReconciliationValidation.Bank_Transfer && c.Withdrawal == false))
            //        .Sum(c => c.Ammount.Value);



            var pa = GetClearingRec(companyId, subcompanyId, chartid, fromDate, toDate);
            if (pa != null)
            {
                //master.ClearingId = pa.Id;
                TotalClearedDepprevtran =
                    pa.Where(a => a.isWithdrawl == true).Sum(a => a.Ammount.Value);
                TotalClearedWithprevtran =
                    pa.Where(a => a.isWithdrawl == false).Sum(a => a.Ammount.Value);
            }
            //master.GLAmount = ((master.OutstandingDeposits) + TotalClearedWithprevtran) -
            //((master.OutstandingWithdrawals) + TotalClearedDepprevtran);
            //master.SubTotal = master.GLAmount + master.OutstandingWithdrawals;
            //master.StatementExpectedAmount = master.SubTotal + master.OutstandingDeposits;
            master.CreatedDate = DateTime.UtcNow;
        }
        private void FillBrs(long companyId, long subcompanyId, long chartId, BankReconciliationModel master, BankReconciliation bankre, DateTime? fromDate, DateTime toDate)
        {
            decimal outDepositamount = 0;
            decimal? TotalClearedDepprevtran = 0;
            decimal? TotalClearedWithprevtran = 0;
            master.CompanyId = companyId;
            master.ServiceCompanyId = subcompanyId;
            //var scn = _companyService.GetById(subcompanyId);
            master.ServiceCompanyName = _companyService.GetByName(subcompanyId);
            master.Id = bankre.Id;
            master.IsDraft = bankre.IsDraft;
            master.ModifiedBy = bankre.ModifiedBy;
            master.ModifiedDate = bankre.ModifiedDate;
            master.BankReconciliationDate = bankre.BankReconciliationDate;
            outDepositamount = master.BankReconciliationDetailModels.Where(
                        c =>
                            c.DocumentType == BankReconciliationValidation.receipt ||
                            c.DocumentType == BankReconciliationValidation.Invoice ||
                            c.DocumentType == BankReconciliationValidation.DebitNote ||
                            c.DocumentType == BankReconciliationValidation.DoubtfulDebt ||
                            c.DocumentType == BankReconciliationValidation.deposit ||
                            c.DocumentType == BankReconciliationValidation.CashSale ||
                            c.DocumentType == BankReconciliationValidation.Journal ||
                            (c.DocumentType == BankReconciliationValidation.Bank_Transfer && c.Withdrawal == false))
                    .Sum(c => c.Ammount.Value);

            //master.OutstandingDeposits = (-outDepositamount);

            //master.OutstandingWithdrawals = master.BankReconciliationDetailModels.Where(
            //            c => (c.DocumentType == BankReconciliationValidation.payments || c.DocumentType == BankReconciliationValidation.Payroll_Bill || c.DocumentType == BankReconciliationValidation.Payroll_Payment ||
            //                           c.DocumentType == BankReconciliationValidation.Bill ||
            //                           (c.DocumentType == BankReconciliationValidation.CreditNote && c.DocSubType == "CreditNote")
            //                           //|| c.DocumentType == BankReconciliationValidation.Journal 
            //                           || c.DocumentType == BankReconciliationValidation.Withdrawal || c.DocumentType == BankReconciliationValidation.CashPayments ||
            //                           (c.DocumentType == BankReconciliationValidation.Bank_Transfer && c.Withdrawal == true)))
            //        .Sum(a => a.Ammount.Value);
            master.COAId = bankre.COAId;
            master.ServiceCompanyId = bankre.ServiceCompanyId;
            master.State = bankre.State;
            master.Currency = bankre.Currency;
            master.BankAccount = bankre.BankAccount;
            master.StatementAmount = bankre.StatementAmount;
            master.StatementDate = bankre.StatementDate;
            master.StatementExpectedAmount = bankre.StatementExpectedAmount;
            master.Status = bankre.Status;
            master.SubTotal = bankre.SubTotal;
            master.UserCreated = bankre.UserCreated;
            master.CreatedDate = bankre.CreatedDate;
            var pa = GetClearingRec(companyId, subcompanyId, chartId, fromDate, toDate);
            if (pa != null)
            {
                //master.ClearingId = pa.Id;
                TotalClearedWithprevtran =
                    pa.Where(a => a.isWithdrawl == true).Sum(a => a.Ammount.Value);
                TotalClearedDepprevtran =
                    pa.Where(a => a.isWithdrawl == false).Sum(a => a.Ammount.Value);
            }
            //master.GLAmount = ((outDepositamount) + TotalClearedDepprevtran) -
            //((master.OutstandingWithdrawals) + TotalClearedWithprevtran);
            //master.SubTotal = master.GLAmount + master.OutstandingWithdrawals;
            //master.StatementExpectedAmount = master.SubTotal + master.OutstandingDeposits;
        }
        public List<BankReconciliationDetailModel> GetClearingRec(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate)
        {
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Enter_into_GetClearingRec_method);
            List<BankReconciliationDetailModel> bankReconciliationDetailModels = lstBankReconciliation(companyId, subcompanyId, chartid, fromDate,
                toDate, true, false).Where(c => c.ClearingDate <= toDate).ToList();
            //bankReconciliationDetailModels = bankReconciliationDetailModels.Where(c => c.ClearingDate <= toDate).ToList();
            //ClearingRecords(id, companyId, subcompanyId, chartid, master);
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Come_out_from_GetClearingRec_metod);

            foreach (BankReconciliationDetailModel Brdetail in bankReconciliationDetailModels)
            {
                if (Brdetail.isWithdrawl == true)
                {
                    Brdetail.isWithdrawl = (Brdetail.DocumentType == BankReconciliationValidation.payments ||
                       Brdetail.DocumentType == BankReconciliationValidation.CashPayments ||
                                           Brdetail.DocumentType == BankReconciliationValidation.Bill ||
                                           (Brdetail.DocumentType == BankReconciliationValidation.CreditNote &&
                                            Brdetail.DocSubType == "CreditNote") ||
                                           Brdetail.DocumentType == BankReconciliationValidation.Journal ||
                                           Brdetail.DocumentType == BankReconciliationValidation.Withdrawal ||
                                           (Brdetail.DocumentType == BankReconciliationValidation.Bank_Transfer &&
                                            Brdetail.Withdrawal == true));
                    Brdetail.Ammount = Brdetail.Ammount ?? 0;
                }
            }
            return bankReconciliationDetailModels;
        }

        public List<BankReconciliationDetailModel> GetListOfClearingRec(long companyId, long serviceCompanyId, Guid bankRecId, DateTime? bankRecDate, DateTime? lastRecDate, long coaId)
        {
            List<BankReconciliationDetailModel> lstDetailModel = new List<BankReconciliationDetailModel>();
            List<BankReconciliationDetail> lstBankRecDetail = new List<BankReconciliationDetail>();
            List<JournalDetail> lstJDetails = new List<JournalDetail>();
            try
            {
                if (bankRecId != Guid.Empty)
                {
                    //lstBankRecDetail = _bankReconciliationDetailService.GetListOfBankRecDetail(bankRecId, bankRecDate);
                    //if (lstBankRecDetail.Any())
                    //{
                    //    lstDetailModel = FillBankClearingDetail(companyId, lstBankRecDetail, coaId, serviceCompanyId);
                    //}
                    lstJDetails = _journalService.GetListOfClearedDetail(companyId, serviceCompanyId, coaId, bankRecDate);
                    if (lstJDetails.Any())
                    {
                        if (lastRecDate != null)
                        {
                            //lstJDetails = lstJDetails.Where(a => a.ClearingDate > lastRecDate && a.ClearingDate <= bankRecDate && a.ReconciliationId == bankRecId).ToList();
                            lstJDetails = lstJDetails.Where(a => /*a.ClearingDate > lastRecDate &&*/ a.ClearingDate <= bankRecDate && (a.ReconciliationId == bankRecId /*|| a.ReconciliationId == null*/)).ToList();
                        }
                        lstDetailModel = FillClearedItems(companyId, lstJDetails, coaId, serviceCompanyId);
                    }
                }
                else if (bankRecId == Guid.Empty)
                {
                    if (lastRecDate != null)
                    {
                        lstJDetails = _journalService.GetListOfClearedDetail(companyId, serviceCompanyId, coaId, bankRecDate);
                        #region Commented_code
                        //List<BankReconciliation> lstBankRec = _bankReconciliationService.GetListOfClearingByCoaIdandScId(serviceCompanyId, coaId);
                        //if (lstBankRec.Any())
                        //{
                        //    //to Get the cleared record for last reconciliation date
                        //    lstBankRecDetail = lstBankRec.SelectMany(a => a.BankReconciliationDetails.Where(c => c.ClearingDate > lastRecDate && c.ClearingDate <= bankRecDate)).ToList();

                        //    //var abc = lstBankRec.SelectMany(a => a.BankReconciliationDetails.Where(c => c.ClearingDate > lastRecDate && c.ClearingDate <= bankRecDate)).GroupBy(t => t.DocRefNo).Select(c=>c.Key).ToList();
                        //    //var abc = lstBankRec.SelectMany(a => a.BankReconciliationDetails.Where(c => c.ClearingDate > lastRecDate && c.ClearingDate <= bankRecDate)).GroupBy(c => new { DocNo = c.DocRefNo }).Select(c => c.Key).ToList();
                        //    //var abc=   lstBankRecDetail.GroupBy(t => t.DocRefNo);

                        //    if (lstBankRecDetail.Any())
                        //    {
                        //        lstDetailModel = FillBankClearingDetail(companyId, lstBankRecDetail, coaId, serviceCompanyId);
                        //    }
                        //}
                        #endregion
                        if (lstJDetails.Any())
                        {
                            //to Get the cleared record for last reconciliation date
                            //lstJDetails = lstJDetails.Where(c => c.ClearingDate > lastRecDate && c.ClearingDate <= bankRecDate)).ToList();
                            lstJDetails = lstJDetails.Where(a => a.ClearingDate > lastRecDate && a.ClearingDate <= bankRecDate).ToList();
                            if (lstJDetails.Any())
                            {
                                lstDetailModel = FillClearedItems(companyId, lstJDetails, coaId, serviceCompanyId);
                            }
                        }
                    }
                    else
                    {
                        lstJDetails = _journalService.GetListOfClearedDetail(companyId, serviceCompanyId, coaId, bankRecDate);
                        lstJDetails = lstJDetails.Where(a => a.ClearingDate <= bankRecDate).ToList();
                        if (lstJDetails.Any())
                        {
                            lstDetailModel = FillClearedItems(companyId, lstJDetails, coaId, serviceCompanyId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return lstDetailModel;
        }

        private List<BankReconciliationDetailModel> FillClearedItems(long companyId, List<JournalDetail> lstOfJournalDetail, long coaId, long serviceCompanyId)
        {
            List<BankReconciliationDetailModel> lstBrDetailModel = new List<BankReconciliationDetailModel>();
            try
            {
                List<Guid?> lstEntityId = lstOfJournalDetail.Select(a => a.EntityId).ToList();
                lstEntityId.Remove(null);
                Dictionary<Guid, string> lstofEntity = _beanEntityService.GetListOfEntity(companyId, lstEntityId);
                bool? isWithDrawal = false;
                if (lstOfJournalDetail.Any())
                {
                    lstBrDetailModel = lstOfJournalDetail.Select(a => new BankReconciliationDetailModel()
                    {
                        DocumentDate = a.DocDate,
                        DocumentType = a.DocType,
                        DocRefNo = a.DocNo,
                        isWithdrawl = isWithDrawal = (a.DocType == DocTypeConstants.BillPayment
                                           || a.DocType == BankReconciliationValidation.Withdrawal || a.DocType == BankReconciliationValidation.CashPayments || (a.DocType == DocTypeConstants.BankTransfer && a.Type == "Withdrawal") || (a.DocType == DocTypeConstants.JournalVocher && a.DocCredit != null)) ? true : false,
                        EntityId = /*a.EntityId != null ? lstofEntity.Where(c => c.Key == a.EntityId).Select(x=>x.Key).FirstOrDefault() : a.EntityId*/a.EntityId,
                        EntityName = a.EntityId != null ? lstofEntity.Where(c => c.Key == a.EntityId).Select(x => x.Value).FirstOrDefault() : null,
                        ClearingDate = a.ClearingDate,
                        //Ammount = isWithDrawal == true ? -(a.BaseDebit != null ? a.BaseDebit : a.BaseCredit) : (a.BaseDebit != null ? a.BaseDebit : a.BaseCredit),
                        Ammount = isWithDrawal == true ? -(a.DocDebit != null ? a.DocDebit : a.DocCredit) : (a.DocDebit != null ? a.DocDebit : a.DocCredit),
                        //BaseAmount = (a.BaseDebit != null ? a.BaseDebit : a.BaseCredit),
                        DocumentId = a.DocumentId,
                        JournalId = a.Id,
                        DocSubType = a.DocSubType,
                        IsChecked = a.ClearingDate != null,
                        COAId = coaId,
                        ServiceEntityId = serviceCompanyId,
                        ClearingStatus = a.ClearingStatus
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                //throw;
            }
            return lstBrDetailModel;
        }

        #endregion

        #region Save


        //public BankReconciliationModel SaveBankReconciliationNew(BankReconciliationModel bankReconciliation, string ConnectionString)
        //{

        //}



        public BankReconciliationModel SaveClearingDate(BankReconciliationModel bankReconciliation, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(bankReconciliation));
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, "ObjectSave", AdditionalInfo);
            SqlConnection con1 = null;
            SqlCommand cmd1 = null;
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Entred_Into_SaveBankReconciliation_Method);
            string _errors = CommonValidation.ValidateObject(bankReconciliation);
            LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Checking_errors);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new InvalidOperationException(_errors);
            }

            List<DocumentHistoryModel> lstDocHistoryModel = new List<DocumentHistoryModel>();
            BankReconciliationDetail Brdetail = new BankReconciliationDetail();
            List<BankReconciliationDetail> brDetails = new List<BankReconciliationDetail>();
            BankReconciliation getbr = _bankReconciliationService.GetBankReconciliationBySid(bankReconciliation.Id, bankReconciliation.CompanyId, bankReconciliation.ServiceCompanyId, bankReconciliation.COAId, bankReconciliation.StatementDate.Value);
            if (getbr != null)
                throw new InvalidOperationException("Bank reconciliation can only be performed once per day for each bank account.");

            //to check whether any void transaction is there in the outstanding or not.
            if (bankReconciliation != null && bankReconciliation.BankReconciliationDetailModels != null
                && _journalService.IfAnyJournalVoid(bankReconciliation.CompanyId, bankReconciliation.BankReconciliationDetailModels.Where(a => a.ClearingDate != null).Select(a => a.DocumentId).ToList()))
            {
                throw new InvalidOperationException(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);
            }

            try
            {
                if (bankReconciliation != null && bankReconciliation.BankReconciliationDetailModels != null)
                {
                    if (bankReconciliation.BankReconciliationDetailModels.Exists(a => a.RecordStatus == "Modified" && a.ClearingDate == null))
                    {
                        string brcDate = string.Format("{0:MM/dd/yyyy}", bankReconciliation.BankReconciliationDate);
                        if (_bankReconciliationService.IsBrcToBeReRun(bankReconciliation.CompanyId, bankReconciliation.ServiceCompanyId, bankReconciliation.COAId, bankReconciliation.BankReconciliationDate) == true)
                        {
                            string query = $"Update Bean.BankReconciliation set IsReRunBR=1 where CompanyId={bankReconciliation.CompanyId} and ServiceCompanyId={bankReconciliation.ServiceCompanyId} and COAId={bankReconciliation.COAId} and BankReconciliationDate>='{brcDate}' and State='Reconciled'";
                            using (con1 = new SqlConnection(ConnectionString))
                            {
                                if (con1.State != System.Data.ConnectionState.Open)
                                    con1.Open();
                                cmd1 = new SqlCommand(query, con1);
                                cmd1.ExecuteNonQuery();
                                con1.Close();
                            }
                        }
                    }
                    //commented code for BRC for new clearing date
                    //if (bankReconciliation.BankReconciliationDetailModels.Where(a => a.RecordStatus == "Added" && a.ClearingDate != null).Any())
                    //{
                    //    List<DateTime?> lstOfDocDates = bankReconciliation.BankReconciliationDetailModels.Where(a => a.RecordStatus == "Added" && a.ClearingDate != null).Select(a => a.DocumentDate).ToList();
                    //    if (lstOfDocDates.Any())
                    //    {
                    //        List<BankReconciliation> lstBankRecons = _bankReconciliationService.GetListOfBankReconciliation(bankReconciliation.CompanyId, bankReconciliation.ServiceCompanyId, bankReconciliation.COAId);
                    //        if (lstBankRecons.Any())
                    //        {
                    //            foreach (var brc in lstBankRecons.Where(a => a.BankReconciliationDate != bankReconciliation.BankReconciliationDate))
                    //            {
                    //                if (lstOfDocDates.Where(a => a.Value <= brc.BankReconciliationDate).Any())
                    //                {
                    //                    brc.IsReRunBR = true;
                    //                    brc.ObjectState = ObjectState.Modified;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //if clearing date is modified
                    if (bankReconciliation.BankReconciliationDetailModels.Exists(a => a.RecordStatus == "Modified" && a.ClearingDate != null))
                    {
                        List<DateTime?> lstOfDocDates = bankReconciliation.BankReconciliationDetailModels.Where(a => a.RecordStatus == "Modified" && a.ClearingDate != null).Select(a => a.DocumentDate).ToList();
                        if (lstOfDocDates.Any())
                        {
                            List<BankReconciliation> lstBankRecons = _bankReconciliationService.GetListOfBankReconciliation(bankReconciliation.CompanyId, bankReconciliation.ServiceCompanyId, bankReconciliation.COAId);
                            if (lstBankRecons.Any())
                            {
                                foreach (var brc in from brc in lstBankRecons.Where(a => a.BankReconciliationDate != bankReconciliation.BankReconciliationDate).ToList()
                                                    where lstOfDocDates.Any(a => a.Value <= brc.BankReconciliationDate)
                                                    select brc)
                                {
                                    brc.IsReRunBR = true;
                                    brc.ObjectState = ObjectState.Modified;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { LoggingHelper.LogError(BRConstants.BankReconciliationApplicationService, ex, ex.Message); }

            BankReconciliation bank = _bankReconciliationService.GetBankReconciliation(bankReconciliation.Id,
               bankReconciliation.CompanyId);

            if (bank == null)
            {
                LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Entered_Into_New_Block);
                bank = new BankReconciliation();
                bank.Id = Guid.NewGuid();
                bankReconciliation.Id = bank.Id;
                bank.CompanyId = bankReconciliation.CompanyId;
                bank.ServiceCompanyId = bankReconciliation.ServiceCompanyId;
                FillBankrec(bankReconciliation, bank);
                if (bankReconciliation.BankReconciliationDetailModels != null && bankReconciliation.BankReconciliationDetailModels.Any())
                {
                    LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Entered_Into_Details_Block);
                    if (bankReconciliation.BankReconciliationDetailModels.Count > 0)
                    {
                        List<BankReconciliationDetail> lstBrDetails = _bankReconciliationDetailService.GetBRCDetailByDocumentId(bankReconciliation.BankReconciliationDetailModels.Select(a => a.DocumentId.Value).ToList());
                        if (lstBrDetails.Any())
                        {
                            foreach (var detail in lstBrDetails)
                            {
                                detail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        foreach (var BrDetails in bankReconciliation.BankReconciliationDetailModels.Where(a => a.ClearingDate != null))
                        {
                            Brdetail = new BankReconciliationDetail();
                            Brdetail.Id = Guid.NewGuid();
                            Brdetail.BankReconciliationId = bank.Id;
                            FillBankRecondetails(Brdetail, BrDetails);
                            Brdetail.ObjectState = ObjectState.Added;
                            _bankReconciliationDetailService.Insert(Brdetail);
                            brDetails.Add(Brdetail);
                        }
                        bank.BankReconciliationDetails = brDetails;
                    }
                }
                bank.CreatedDate = DateTime.UtcNow;
                bank.UserCreated = bankReconciliation.UserCreated;
                bank.ObjectState = ObjectState.Added;
                _bankReconciliationService.Insert(bank);
            }
            else
            {

                ////Data concurrency verify
                //string timeStamp = "0x" + string.Concat(Array.ConvertAll(bank.Version, x => x.ToString("X2")));
                //if (!timeStamp.Equals(bankReconciliation.Version))
                //    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Entered_Into_Updated);
                FillBankrec(bankReconciliation, bank);
                bank.IsReRunBR = false;

                if (bank.BankReconciliationDetails.Count >= 1)
                {
                    foreach (var detail in bank.BankReconciliationDetails)
                    {
                        detail.ObjectState = ObjectState.Deleted;
                    }
                }

                //to delete the previous documentId Data, if exists in other reconcliation

                if (bankReconciliation.BankReconciliationDetailModels != null && bankReconciliation.BankReconciliationDetailModels.Any())
                {
                    List<BankReconciliationDetail> lstBrDetails = _bankReconciliationDetailService.GetBRCDetailByDocumentIdandBrcId(bankReconciliation.BankReconciliationDetailModels.Select(a => a.DocumentId.Value).ToList(), bank.Id);
                    if (lstBrDetails.Any())
                    {
                        foreach (var detail in lstBrDetails)
                        {
                            detail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
                if (bankReconciliation.BankReconciliationDetailModels != null && bankReconciliation.BankReconciliationDetailModels.Count >= 1)
                {
                    LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Entered_Into_Update_Details_Block);
                    foreach (var BrDetails in bankReconciliation.BankReconciliationDetailModels.Where(a => a.ClearingDate != null))
                    {
                        BankReconciliationDetail detail = new BankReconciliationDetail();
                        FillBankRecondetails(detail, BrDetails);
                        Brdetail.BankReconciliationId = bankReconciliation.Id;
                        detail.Id = Guid.NewGuid();
                        detail.BankReconciliationId = bank.Id;
                        detail.ObjectState = ObjectState.Added;
                        _bankReconciliationDetailService.Insert(detail);
                    }
                }
                bank.CreatedDate = bankReconciliation.CreatedDate;
                bank.ModifiedBy = bankReconciliation.ModifiedBy;
                bank.ModifiedDate = DateTime.UtcNow;
                bank.ObjectState = ObjectState.Modified;
                _bankReconciliationService.Update(bank);
            }
            try
            {
                _unitOfWorkAsync.SaveChanges();//saving on the top due to concurrency issue

                //BackgroundJob.Enqueue(() => new AppsWorld.BankReconciliationModule.Service.HangfireService().DifferentDocumentsClearingdateUpdation(bankReconciliation));
                new AppsWorld.BankReconciliationModule.Service.HangfireService().DifferentDocumentsClearingdateUpdation(bankReconciliation, ConnectionString, lstDocHistoryModel);
                //new AppsWorld.BankReconciliationModule.Service.HangfireService().DifferentDocumentsClearingdateUpdation(bankReconciliation);

                //BankReconciliation brc = _bankReconciliationService.GetBRByCOAID(bankReconciliation.COAId, bankReconciliation.ServiceCompanyId, bankReconciliation.CompanyId);
                //if (brc != null)
                //{
                //    brc.IsReRunBR = null;
                //    brc.ObjectState = ObjectState.Modified;
                //}

                //_unitOfWorkAsync.SaveChanges();//commented on 20-12-2019 for BRC concurrency issue

                #region Document_History
                //BankReconciliation docBrc = new BankReconciliation();
                //docBrc = bank != null ? bank : br;
                if (bank != null)
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(bank.Id, bank.CompanyId, bank.Id, "Bank Reconciliation", "Bank Reconciliation", bank.State, string.Empty, bank.StatementAmount, 0, 0, bank.ModifiedBy != null ? bank.ModifiedBy : bank.UserCreated, string.Empty, bank.BankReconciliationDate, 0, 0);
                    try
                    {
                        lstDocHistoryModel.AddRange(lstdocumet);
                        AppaWorld.Bean.Common.SaveDocumentHistory(lstDocHistoryModel, ConnectionString);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                #endregion Document_History

                if (bankReconciliation.State == BRConstants.Draft)
                    try
                    {
                        using (SqlConnection Con = new SqlConnection(ConnectionString))
                        {
                            if (Con.State != System.Data.ConnectionState.Open)
                            {
                                Con.Open();
                                SqlCommand cmd = new SqlCommand("Bean_UpdateJDetailIfBrcVoid", Con);
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@CompanyId", bankReconciliation.CompanyId);
                                cmd.Parameters.AddWithValue("@serviceCompanyId", bankReconciliation.ServiceCompanyId);
                                cmd.Parameters.AddWithValue("@CoaId", bankReconciliation.COAId);
                                cmd.Parameters.AddWithValue("@BankRecId", bankReconciliation.Id);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }

                LoggingHelper.LogMessage(BRConstants.BankReconciliationApplicationService, BankReconciliationLoggingValidation.Data_Saved_Successfully_And_EventRised);
            }
            catch (DbEntityValidationException ex)
            {
                LoggingHelper.LogError(BRConstants.BankReconciliationApplicationService, ex, ex.Message);
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine(BankReconciliationLoggingValidation.Entity_Error, eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                        Console.WriteLine(BankReconciliationLoggingValidation._Property, ve.PropertyName, ve.ErrorMessage);
                }
                throw;
            }
            return bankReconciliation;

        }

        public void SaveClearings(List<BankReconciliationDetailModel> model, string ConnectionStrings)
        {
            try
            {
                long companyId = model.Take(1).Select(c => c.CompanyId).FirstOrDefault();
                //BackgroundJob.Enqueue(() => new HangfireService().UpdateDates(model, companyId, false));
                new HangfireService().UpdateClearingDates(model, companyId, false, ConnectionStrings);
                foreach (var details in model)
                {
                    var brde = _bankReconciliationDetailService.GetBankReconciliationDetailbyDocId(details.DocumentId);
                    if (brde != null)
                    {
                        brde.ObjectState = ObjectState.Deleted;
                        _bankReconciliationDetailService.Delete(brde);

                        //brde.ClearingDate = details.ClearingDate;
                        //if (details.ClearingDate != null)
                        //    brde.ClearingStatus = "Cleared";
                        //_bankReconciliationDetailService.Update(brde);
                    }
                }
                //   update(model.Details, model.CompanyId);
                _unitOfWorkAsync.SaveChanges();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(BRConstants.BankReconciliationApplicationService, ex, ex.Message);
                throw;
            }

        }

        public void SaveReconcile(List<BankReconciliationDetailModel> model)
        {
            if (model.Any())
            {
                long? companyId = model.Take(1).Select(c => c.CompanyId).FirstOrDefault();
                //BackgroundJob.Enqueue(() => new HangfireService().UpdateDates(model, companyId.Value, true));
                new HangfireService().UpdateDates(model, companyId.Value, true);
            }
        }
        public BankReconciliationModel Voidbankreconciliation(BankReconciliationModel VBankReconciliation, string connectionString)
        {
            BankReconciliation vbr = _bankReconciliationService.GetAllBankReconciliations(VBankReconciliation.Id, VBankReconciliation.ServiceCompanyId);
            if (vbr != null)
            {
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(vbr.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(VBankReconciliation.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                vbr.State = BankReconciliationValidation.BankReconciliation_Void;
                vbr.ModifiedDate = DateTime.UtcNow;
                vbr.ModifiedBy = VBankReconciliation.ModifiedBy;
                vbr.ObjectState = ObjectState.Modified;
                vbr.IsReRunBR = null;
                _bankReconciliationService.Update(vbr);

                try
                {
                    using (SqlConnection Con = new SqlConnection(connectionString))
                    {
                        if (Con.State != System.Data.ConnectionState.Open)
                        {
                            Con.Open();
                            SqlCommand cmd = new SqlCommand("Bean_UpdateJDetailIfBrcVoid", Con);
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CompanyId", vbr.CompanyId);
                            cmd.Parameters.AddWithValue("@serviceCompanyId", vbr.ServiceCompanyId);
                            cmd.Parameters.AddWithValue("@CoaId", vbr.COAId);
                            cmd.Parameters.AddWithValue("@BankRecId", vbr.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception)
                {

                    //throw;
                }


                try
                {
                    _unitOfWorkAsync.SaveChanges();

                    #region Document_History
                    if (vbr != null)
                    {
                        try
                        {
                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(vbr.Id, vbr.CompanyId, vbr.Id, "Bank Reconciliation", "Bank Reconciliation", vbr.State, string.Empty, vbr.StatementAmount, 0, 0, VBankReconciliation.ModifiedBy != null ? VBankReconciliation.ModifiedBy : vbr.UserCreated, string.Empty, vbr.BankReconciliationDate, 0, 0);
                            if (lstdocumet.Any())
                                AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, connectionString);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    #endregion Document_History

                }
                catch (Exception ex)
                {
                    LoggingHelper.LogError(BRConstants.BankReconciliationApplicationService, ex, ex.Message);
                    throw ex;
                }
            }
            return VBankReconciliation;
        }
        #endregion

        #region Private

        private List<BankReconciliationDetailModel> lstBankReconciliation(long companyId, long subcompanyId, long chartid, DateTime? fromDate, DateTime toDate, bool isClearing, bool isBankReconcile)
        {
            BankReconciliationDetailModel bankReconciliationDetail1 = new BankReconciliationDetailModel();
            List<BankReconciliationDetailModel> lstbrd = new List<BankReconciliationDetailModel>();
            List<BankReconciliationDetailModel> lstbrd1 = new List<BankReconciliationDetailModel>();
            //if (fromDate == null)
            //{
            //    var financials = _financialSettingService.GetFinancialSetting(companyId);
            //    if (financials == null)
            //        throw new Exception(BankReconciliationValidation.No_Active_Finance_Setting_found);
            //    string s = financials != null ? (financials.FinancialYearEnd) : null;
            //    string[] a = s.Split('-').ToArray();
            //    int year = toDate.Year;
            //    string str = a[0] + a[1] + year.ToString();
            //    DateTime fromDat = DateTime.Parse(str);
            //    fromDate = fromDat.AddDays(-1);
            //    if (fromDate >= DateTime.UtcNow)
            //        fromDate = fromDat.AddYears(-1);
            //}
            //if (financials.PeriodLockDate != null && financials.PeriodEndDate != null)
            //{
            //    if (fromDate >= financials.PeriodLockDate && toDate <= financials.PeriodEndDate)
            //    { }
            //    else
            //        throw new Exception(BankReconciliationValidation.From_Date_And_To_Date_Must_be_in_Financial_Year_End);
            //}
            var lstbankR = _journalService.GetAllJournalDetails(companyId, subcompanyId, chartid, fromDate, toDate, isClearing, isBankReconcile, null);
            lstbankR = (from jd in lstbankR
                        join j in _journalService.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
                        where (j.DocumentState != "Void" && j.DocumentState != "Reset")
                        select jd).ToList();
            var lstbankr1 = lstbankR.Where(c => c.EntityId == Guid.Empty || c.EntityId == null).ToList();
            if (lstbankR.Any())
            {
                List<BeanEntity> lstEntity = _beanEntityService.GetEntityByCId(companyId);
                lstbrd = (from br in lstbankR
                          join e in lstEntity on br.EntityId equals e.Id
                          select new BankReconciliationDetailModel
                          {
                              Id = br.Id,
                              JournalId = br.Id,
                              DocSubType = br.DocSubType,
                              DocumentDate = br.DocDate,
                              DocumentType = br.DocType,
                              CompanyId = companyId,
                              DocRefNo = br.DocNo,
                              //RefNo = br.SystemRefNo,
                              EntityId = e.Id,
                              EntityName = e.Name,
                              Withdrawal = (br.DocType == BankReconciliationValidation.payments ||
                                       br.DocType == BankReconciliationValidation.Bill || br.DocType == BankReconciliationValidation.Payroll_Bill || br.DocType == BankReconciliationValidation.Payroll_Payment ||
                                       (br.DocType == BankReconciliationValidation.CreditNote && br.DocSubType == "CreditNote") || br.DocType == BankReconciliationValidation.Withdrawal || br.DocType == BankReconciliationValidation.CashPayments ||
                                       (br.DocType == BankReconciliationValidation.Bank_Transfer && br.Type == "Withdrawal")),

                              isWithdrawl = (br.DocType == BankReconciliationValidation.payments ||
                              br.DocType == BankReconciliationValidation.Bill || br.DocType == BankReconciliationValidation.Payroll_Bill || br.DocType == BankReconciliationValidation.Payroll_Payment ||
                              (br.DocType == BankReconciliationValidation.CreditNote && br.DocSubType == "CreditNote") || br.DocType == BankReconciliationValidation.Withdrawal || br.DocType == BankReconciliationValidation.CashPayments ||
                              (br.DocType == BankReconciliationValidation.Bank_Transfer && br.Type == "Withdrawal")),

                              Ammount = (br.DocType == BankReconciliationValidation.payments || br.DocType == BankReconciliationValidation.Payroll_Payment ||
                                         br.DocType == BankReconciliationValidation.CashPayments ||
                                       br.DocType == "Journal" || br.DocType == "Withdrawal" ||
                                       (br.DocType == BankReconciliationValidation.Bank_Transfer && br.Type == "Withdrawal")) ? ((br.DocCredit != 0 && br.DocCredit != null) ? br.DocCredit : br.DocDebit) : ((br.DocDebit != 0 && br.DocDebit != null) ? br.DocDebit : br.DocCredit),
                              ClearingDate = br.ClearingDate,
                              ClearingStatus = br.ClearingDate != null ? "Cleared" : string.Empty,
                              DocumentId = br.DocType == "Journal" ? br.Id : br.DocumentId
                          }).ToList();
                lstbrd1 = (from br1 in lstbankr1
                           select new BankReconciliationDetailModel
                           {
                               Id = br1.Id,
                               JournalId = br1.Id,
                               DocumentDate = br1.DocDate,
                               DocSubType = br1.DocSubType,
                               DocumentType = br1.DocType,
                               DocRefNo = br1.DocNo,
                               CompanyId = companyId,
                               //RefNo = br1.SystemRefNo,
                               Withdrawal = (br1.DocType == BankReconciliationValidation.payments || br1.DocType == BankReconciliationValidation.Payroll_Bill || br1.DocType == BankReconciliationValidation.Payroll_Payment ||
                                       br1.DocType == BankReconciliationValidation.Bill ||
                                       (br1.DocType == BankReconciliationValidation.CreditNote && br1.DocSubType == "CreditNote") || br1.DocType == BankReconciliationValidation.Withdrawal || br1.DocType == BankReconciliationValidation.CashPayments ||
                                       (br1.DocType == BankReconciliationValidation.Bank_Transfer && br1.Type == "Withdrawal")),
                               isWithdrawl = (br1.DocType == BankReconciliationValidation.payments || br1.DocType == BankReconciliationValidation.Payroll_Bill || br1.DocType == BankReconciliationValidation.Payroll_Payment ||
                                       br1.DocType == BankReconciliationValidation.Bill ||
                                       (br1.DocType == BankReconciliationValidation.CreditNote && br1.DocSubType == "CreditNote") || br1.DocType == BankReconciliationValidation.Withdrawal || br1.DocType == BankReconciliationValidation.CashPayments ||
                                       (br1.DocType == BankReconciliationValidation.Bank_Transfer && br1.Type == "Withdrawal")),
                               Ammount = (br1.DocType == BankReconciliationValidation.payments || br1.DocType == BankReconciliationValidation.Payroll_Payment ||
                               br1.DocType == BankReconciliationValidation.CashPayments ||
                                       br1.DocType == "Journal" || br1.DocType == "Withdrawal" ||
                                       (br1.DocType == BankReconciliationValidation.Bank_Transfer)) ? ((br1.DocCredit != 0 & br1.DocCredit != null) ? br1.DocCredit : br1.DocDebit) : ((br1.DocDebit != 0 && br1.DocDebit != null) ? br1.DocDebit : br1.DocCredit),
                               ClearingDate = br1.ClearingDate,
                               ClearingStatus = br1.ClearingDate != null ? "Cleared" : string.Empty,
                               DocumentId = br1.DocType == "Journal" ? br1.Id : br1.DocumentId
                           }).ToList();
            }
            if (lstbrd1.Any())
                lstbrd.AddRange(lstbrd1);
            return lstbrd.Where(c => c.Ammount > 0).OrderBy(a => a.DocumentDate).ToList();
        }

        private List<BankReconciliationDetailModel> ClearingRecords(Guid id, long companyId, long subcompanyId, long chartid, BankReconciliationModel master)
        {
            List<BankReconciliationDetailModel> lstwithdrawal = new List<BankReconciliationDetailModel>();
            // BankReconciliationDetailModel withdr = new BankReconciliationDetailModel();
            List<BankReconciliationDetailModel> lstbrd1 = new List<BankReconciliationDetailModel>();
            var pa = _bankReconciliationService.GetBankRDetailsBychartid(id, companyId, subcompanyId, chartid, true);

            if (pa != null)
            {
                IEnumerable<BankReconciliationDetail> lstbankr1 = pa.BankReconciliationDetails.Where(c => c.EntityId == Guid.Empty || c.EntityId == null).ToList();

                List<BeanEntity> lstEntity = _beanEntityService.GetEntityByCId(companyId);
                lstwithdrawal = (from br in pa.BankReconciliationDetails.Where(a => a.ClearingDate != null)
                                 join e in lstEntity on br.EntityId equals e.Id
                                 select new BankReconciliationDetailModel
                                 {
                                     Id = br.Id,
                                     DocumentDate = br.DocumentDate,
                                     DocumentType = br.DocumentType,
                                     DocRefNo = br.DocRefNo,
                                     //RefNo = br.RefernceNo,
                                     EntityId = e.Id,
                                     EntityName = e.Name,
                                     Ammount = br.Ammount,
                                     ClearingDate = br.ClearingDate,
                                     isWithdrawl = br.isWithdrawl,
                                     ClearingStatus = br.ClearingDate != null ? (br.ClearingDate).ToString() : string.Empty,
                                     DocumentId = br.DocumentId,
                                     //Withdrawlamount = pa.BankReconciliationDetails.Where(a => a.isWithdrawl == true).Sum(a => a.Ammount.Value),
                                     //Depositamount = pa.BankReconciliationDetails.Where(a => a.isWithdrawl == false).Sum(a => a.Ammount.Value)

                                 }).ToList();
                lstbrd1 = (from br1 in lstbankr1
                           select new BankReconciliationDetailModel
                           {
                               Id = br1.Id,
                               DocumentDate = br1.DocumentDate,
                               DocumentType = br1.DocumentType,
                               DocRefNo = br1.DocRefNo,
                               //RefNo = br1.RefernceNo,
                               //EntityId = e.Id,
                               //EntityName = e.Name,
                               Ammount = br1.Ammount,
                               ClearingDate = br1.ClearingDate,
                               isWithdrawl = br1.isWithdrawl,
                               ClearingStatus = br1.ClearingDate != null ? (br1.ClearingDate).ToString() : string.Empty,
                               DocumentId = br1.DocumentId,
                               //Withdrawlamount = pa.BankReconciliationDetails.Where(a => a.isWithdrawl == true).Sum(a => a.Ammount.Value),
                               //Depositamount = pa.BankReconciliationDetails.Where(a => a.isWithdrawl == false).Sum(a => a.Ammount.Value)
                           }).ToList();

            }
            if (lstbrd1.Any())
                lstwithdrawal.AddRange(lstbrd1);

            return lstwithdrawal.OrderBy(a => a.DocumentDate).ToList();
        }
        private void FillBankrec(BankReconciliationModel bankReconciliation, BankReconciliation br)
        {
            br.BankReconciliationDate = bankReconciliation.StatementDate.Value.Date;
            br.BankAccount = bankReconciliation.BankAccount;
            br.COAId = bankReconciliation.COAId;
            br.Currency = bankReconciliation.Currency;
            br.BankAccount = bankReconciliation.BankAccount;
            br.StatementAmount = bankReconciliation.StatementAmount;
            br.SubTotal = bankReconciliation.SubTotal ?? 0;
            br.StatementExpectedAmount = bankReconciliation.StatementExpectedAmount ?? 0;
            br.GLAmount = bankReconciliation.GLAmount ?? 0;
            br.State = bankReconciliation.State;
            br.StatementDate = bankReconciliation.StatementDate;
            br.IsDraft = bankReconciliation.IsDraft;
            br.Status = RecordStatusEnum.Active;
            br.UserCreated = bankReconciliation.UserCreated;
        }
        private static void FillBankRecondetails(BankReconciliationDetail Brdetail, BankReconciliationDetailModel BrDetails)
        {
            Brdetail.Ammount = BrDetails.Ammount ?? 0;
            Brdetail.ClearingDate = BrDetails.ClearingDate;
            if (BrDetails.ClearingDate != null)
                Brdetail.ClearingStatus = BrDetails.ClearingStatus;
            Brdetail.DocumentDate = BrDetails.DocumentDate;
            Brdetail.DocumentType = BrDetails.DocumentType;
            Brdetail.DocRefNo = BrDetails.DocRefNo;
            Brdetail.EntityId = BrDetails.EntityId;
            Brdetail.DocumentId = BrDetails.DocumentId.Value;
            Brdetail.isWithdrawl = BrDetails.isWithdrawl;
            Brdetail.IsDisable = BrDetails.IsDisable;
            Brdetail.IsChecked = BrDetails.IsChecked;
            Brdetail.Mode = BrDetails.Mode;
            Brdetail.RefNo = BrDetails.RefNo;
            Brdetail.DocSubType = BrDetails.DocSubType;
            Brdetail.JournalDetailId = BrDetails.JournalDetailId;
        }


        private List<BankReconciliationDetailModel> fillBankReconcilaition(long companyId, long serviceCompanyId, long coaId, DateTime? reconciledDate, BankReconciliationModel brcModel, DateTime? lastRecDate)
        {
            brcModel.CompanyId = companyId;
            brcModel.ServiceCompanyId = serviceCompanyId;
            //brcModel.
            List<BankReconciliationDetailModel> lstBrDetail = new List<BankReconciliationDetailModel>();
            List<JournalDetail> lstJournalDetail = _journalService.GetlstJournalDetailByCoaId(serviceCompanyId, coaId, reconciledDate, /*brcModel.LastReconciledDate*/lastRecDate);
            if (lstJournalDetail.Any())
            {
                //if (brcModel.LastReconciledDate != null)
                //    lstJournalDetail.Where(a => (a.ClearingDate == null || a.ClearingDate > reconciledDate)).ToList();

                lstJournalDetail = (from jd in lstJournalDetail
                                    join j in _journalService.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
                                    where (j.DocumentState != "Void" && j.DocumentState != "Reset" && j.DocumentState != "Recurring" && j.DocumentState != "Parked")
                                    select jd).ToList();
                List<AppsWorld.BankReconciliationModule.Entities.Journal> lstJournals = _journalService.GetListOfJournalByJournalId(companyId, lstJournalDetail != null ? lstJournalDetail.Select(a => a.JournalId).ToList() : new List<Guid>());
                List<Guid?> lstEntityId = lstJournalDetail.Select(a => a.EntityId).ToList();
                lstEntityId.Remove(null);
                Dictionary<Guid, string> lstofEntity = _beanEntityService.GetListOfEntity(companyId, lstEntityId);
                bool? isWithDrawal = false;

                if (lstJournalDetail != null)
                {
                    lstBrDetail = lstJournalDetail.Select(a => new BankReconciliationDetailModel()
                    {
                        DocumentDate = a.DocDate,
                        DocumentType = a.DocType,
                        DocRefNo = a.DocNo,
                        isWithdrawl = isWithDrawal = (a.DocType == DocTypeConstants.BillPayment
                                           || a.DocType == BankReconciliationValidation.Withdrawal || a.DocType == BankReconciliationValidation.CashPayments || (a.DocType == DocTypeConstants.BankTransfer && a.Type == "Withdrawal") || (a.DocType == DocTypeConstants.JournalVocher && a.DocCredit != null)) ? true : false,
                        EntityId = /*a.EntityId != null ? lstofEntity.Where(c => c.Key == a.EntityId).Select(x=>x.Key).FirstOrDefault() : a.EntityId*/a.EntityId,
                        EntityName = a.EntityId != null ? lstofEntity.Where(c => c.Key == a.EntityId).Select(x => x.Value).FirstOrDefault() : null,
                        ClearingDate = a.ClearingDate,
                        //Ammount = isWithDrawal == true ? -(a.BaseDebit != null ? a.BaseDebit : a.BaseCredit) : (a.BaseDebit != null ? a.BaseDebit : a.BaseCredit), //Commented for Cindi changes as she dont wants as Base amount 
                        Ammount = isWithDrawal == true ? -(a.DocDebit != null ? a.DocDebit : a.DocCredit) : (a.DocDebit != null ? a.DocDebit : a.DocCredit),
                        //BaseAmount = (a.BaseDebit != null ? a.BaseDebit : a.BaseCredit),
                        DocumentId = a.DocumentId,
                        JournalId = a.Id,
                        DocSubType = a.DocSubType,
                        IsChecked = a.ClearingDate != null ? true : false,
                        COAId = coaId,
                        IsUncleared = a.ClearingDate == null || a.ClearingDate > reconciledDate,
                        RecordStatus = null,
                        isClearedTab = a.ClearingDate != null ? (a.ReconciliationId == null || a.IsBankReconcile == null) : false,
                        ServiceEntityId = serviceCompanyId,
                        Mode = lstJournals.Any() ? lstJournals.Where(c => c.Id == a.JournalId).Select(c => c.ModeOfReceipt).FirstOrDefault() : null,
                        RefNo = lstJournals.Any() ? lstJournals.Where(c => c.Id == a.JournalId).Select(c => c.TransferRefNo).FirstOrDefault() : null,
                        JournalDetailId = a.Id
                    }).ToList();
                }
                //decimal? clearedDeposit = lstBrDetail.Any() ? Math.Round((decimal)lstBrDetail.Where(a => a.ClearingDate != null && a.isWithdrawl != true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;
                //decimal? clearedWithdrawal = lstBrDetail.Any() ? Math.Round((decimal)lstBrDetail.Where(a => a.ClearingDate != null && a.isWithdrawl == true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;
                //decimal? outStandingDeposit = lstBrDetail.Any() ? Math.Round((decimal)lstBrDetail.Where(a => a.ClearingDate == null && a.isWithdrawl != true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;
                //decimal? outStandingWithdrawal = lstBrDetail.Any() ? Math.Round((decimal)lstBrDetail.Where(a => a.ClearingDate == null && a.isWithdrawl == true).Sum(a => a.BaseAmount), 2, MidpointRounding.AwayFromZero) : 0;

                //brcModel.GLAmount = (outStandingDeposit + clearedDeposit) - (outStandingWithdrawal + clearedWithdrawal);

                //brcModel.GLAmount = _journalService.GetGLBalance(coaId, companyId, serviceCompanyId, reconciledDate.Value);



                brcModel.CreatedDate = DateTime.UtcNow;

                return brcModel.BankReconciliationDetailModels = lstBrDetail.Where(a => a.DocumentDate <= reconciledDate).OrderBy(a => a.DocumentDate).ToList();
            }
            return brcModel.BankReconciliationDetailModels = null;
        }


        public async Task<decimal?> GetGLBalanceFromDetail(long coaId, long companyId, long serviceCompanyId, DateTime reconciledDate)
        {
            //DateTime? systemStartDate = _JournalRepository.Query(a => a.COAId == coaId && a.ServiceCompanyId == serviceCompanyId).Select().OrderByDescending(a => a.CreatedDate).Select(a => a.PostingDate).FirstOrDefault();
            //DateTime? systemStartDate = _JournalRepository.Query(a => a.CompanyId == companyId).Select().OrderBy(a => a.CreatedDate).Select(a => a.PostingDate).FirstOrDefault();
            var systemStartDate = (from i in _journalService.Queryable() where i.CompanyId == companyId orderby i.PostingDate ascending select i.PostingDate).FirstOrDefault();
            //List<JournalDetail> lstJDetail = _journalDetailRepository.Query(a => a.ServiceCompanyId == serviceCompanyId && a.COAId == coaId && a.docume && (a.DocDate >= systemStartDate && a.DocDate <= reconciledDate)).Select().ToList();

            List<JournalDetail> lstJournalDetail = await Task.Run(() => _journalService.GetListOfJournalDetail(coaId, serviceCompanyId));

            lstJournalDetail = (from jd in lstJournalDetail
                                join j in _journalService.Queryable().Where(a => a.CompanyId == companyId) on jd.JournalId equals j.Id
                                where (j.DocumentState != "Void" && j.DocumentState != "Reset" && j.DocumentState != "Recurring" && j.DocumentState != "Parked" && (jd.DocDate >= systemStartDate && jd.DocDate <= reconciledDate))
                                select jd).ToList();

            //decimal? glBalance = Math.Round((decimal)lstJournalDetail.Sum(a => a.BaseDebit) - (decimal)lstJournalDetail.Sum(a => a.BaseCredit), 2, MidpointRounding.AwayFromZero);
            decimal? glBalance = Math.Round((decimal)lstJournalDetail.Sum(a => a.DocDebit) - (decimal)lstJournalDetail.Sum(a => a.DocCredit), 2, MidpointRounding.AwayFromZero);
            return glBalance;
        }


        //private List<BankReconciliationDetailModel> GetBankReconciliationClearingRecord(Guid id, long companyId, long serviceCompanyId, long coaId)
        //{
        //    BankReconciliation bankRec = _bankReconciliationService.GetBankRDetailsBychartid(id, companyId, serviceCompanyId, coaId, true);
        //    if (bankRec != null)
        //    {
        //        bankRec.Id = id;
        //        bankRec.IsDraft = bankRec.IsDraft
        //    }
        //}


        #region Commented

        //private void DifferentDocumentsClearingdateUpdation(BankReconciliationModel bankReconciliation, BankReconciliation br)
        //{
        //    if (bankReconciliation.BankReconciliationDetailModels.Any())
        //    {
        //        update(bankReconciliation.BankReconciliationDetailModels, bankReconciliation.CompanyId);
        //    }


        //}
        //private void update(List<BankReconciliationDetailModel> details, long companyId)
        //{
        //    foreach (var BrDetails in details)
        //    {
        //        //  AppsWorld.BankReconciliationModule.Entities.Journal journal = new AppsWorld.BankReconciliationModule.Entities.Journal();
        //        var journ = _journalService.GetJournal(BrDetails.DocumentId.Value, companyId);
        //        if (journ != null)
        //        {
        //            journ.ClearingDate = BrDetails.ClearingDate;
        //            if (BrDetails.ClearingDate != null)
        //            {
        //                journ.ClearingStatus = "Cleared";
        //            }
        //            _journalService.Update(journ);
        //        }
        //        if (BrDetails.DocumentType == "Reciept")
        //        {
        //            //  Receipt reciep = new Receipt();
        //            var reciept = _receiptService.GetReceipt(BrDetails.DocumentId.Value, companyId);
        //            if (reciept != null)
        //            {
        //                reciept.BankClearingDate = BrDetails.ClearingDate;
        //                _receiptService.Update(reciept);
        //            }
        //        }
        //        else if (BrDetails.DocumentType == "Payment")
        //        {
        //            //Payment payment = new Payment();
        //            var paym = _paymentService.GetPayment(BrDetails.DocumentId.Value, companyId);
        //            if (paym != null)
        //            {
        //                paym.BankClearingDate = BrDetails.ClearingDate;
        //                _paymentService.Update(paym);
        //            }
        //        }
        //        else if (BrDetails.DocumentType == "Deposit")
        //        {
        //            // Withdrawal withdrawal = new Withdrawal();
        //            var withdraw = _withdrawalService.GetWithdraw(BrDetails.DocumentId.Value, companyId);
        //            if (withdraw != null)
        //            {
        //                withdraw.BankClearingDate = BrDetails.ClearingDate;
        //                _withdrawalService.Update(withdraw);
        //            }
        //        }
        //        else
        //        {
        //            // BankTransfer banktransfer = new BankTransfer();
        //            var banktran = _bankTransferService.GetBankTran(BrDetails.DocumentId.Value, companyId);
        //            if (banktran != null)
        //            {
        //                banktran.BankClearingDate = BrDetails.ClearingDate;
        //                _bankTransferService.Update(banktran);
        //            }
        //        }

        //    }
        //}

        #endregion Commented

        #endregion
    }
}


