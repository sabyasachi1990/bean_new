using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.GLClearingModule.Service;
using AppsWorld.GLClearingModule.RepositoryPattern;
using AppsWorld.GLClearingModule.Models;
using Serilog;
using Logger;
using AppsWorld.GLClearingModule.Entities;
using AppsWorld.CommonModule.Service;
using AppsWorld.GLClearingModule.Infra;
using AppsWorld.CommonModule.Infra;
using Repository.Pattern.Infrastructure;
using System.Data.Entity.Validation;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Models;
using System.Configuration;
using Ziraff.Section;
using Domain.Events;
using AppsWorld.CommonModule.Entities;
using Logger;
using Serilog;
using Ziraff.FrameWork.Logging;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using AppaWorld.Bean;

namespace AppsWorld.GLClearingModule.Application
{
    public class GLClearingApplicationService
    {
        private readonly IClearingService _clearingService;
        private readonly IClearingDetailService _clearingDetailService;
        private readonly IClearingModuleUnitOfWorkAsync _unitOfWork;
        private readonly ICompanyService _companyService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly AppsWorld.GLClearingModule.Service.IJournalService _journalService;
        private readonly AppsWorld.GLClearingModule.Service.IJournalDetailService _journalDetailService;
        private readonly AppsWorld.GLClearingModule.Service.IAutoNumberService _autoNumberService;
        private readonly AppsWorld.GLClearingModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly IMultiCurrencySettingService _multiCurrencySettingService;
        private readonly AppsWorld.CommonModule.Service.IBeanEntityService _beanEntityService;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        SqlConnection con = null;
        string query = string.Empty;
        SqlCommand cmd = null;
        SqlDataReader dr = null;

        public GLClearingApplicationService(IClearingModuleUnitOfWorkAsync unitOfWork, IClearingService clearingService, IClearingDetailService clearingDetailService, ICompanyService companyService, IAccountTypeService accountTypeService, IChartOfAccountService chartOfAccountService, IFinancialSettingService financialSettingService, AppsWorld.GLClearingModule.Service.IJournalService journalService, AppsWorld.GLClearingModule.Service.IJournalDetailService journalDetailService, AppsWorld.GLClearingModule.Service.IAutoNumberService autoNumberService, AppsWorld.GLClearingModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.CommonModule.Service.IBeanEntityService beanEntityService, IMultiCurrencySettingService multiCurrencySettingService, AppsWorld.CommonModule.Service.IAutoNumberService autoService)
        {
            this._clearingService = clearingService;
            this._clearingDetailService = clearingDetailService;
            this._unitOfWork = unitOfWork;
            this._companyService = companyService;
            this._accountTypeService = accountTypeService;
            this._chartOfAccountService = chartOfAccountService;
            this._financialSettingService = financialSettingService;
            this._journalService = journalService;
            this._journalDetailService = journalDetailService;
            this._autoNumberService = autoNumberService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._beanEntityService = beanEntityService;
            _multiCurrencySettingService = multiCurrencySettingService;
            this._autoService = autoService;
        }

        #region Create and Lookup Block
        public ClearingModelLUs GetClearingLUs(string username, Guid id, long companyId)
        {
            ClearingModelLUs clearingLU = new ClearingModelLUs();
            try
            {
                GLClearing clearing = _clearingService.GetClearing(id, companyId);
                long comp = clearing == null ? 0 : clearing.ServiceCompanyId;
                clearingLU.SubsideryCompanyLU = _companyService.GetSubCompany(username, companyId, comp);
                //string AccountName = ClearingValidations.Balance_Sheet;
                //var account = _chartOfAccountService.GetAllBalanceSheet(companyId, AccountName);
                List<COALookup<string>> lstEditCoa = null;
                //List<string> coaName = new List<string> { "Assets", "Liabilities", "Equity" };
                List<string> coaName = new List<string> { COANameConstants.Deposits_and_prepayments, COANameConstants.Inventory, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.Other_current_assets, COANameConstants.Fixed_assets, COANameConstants.Other_investments, COANameConstants.Intangible_assets, COANameConstants.Other_non_current_assets, COANameConstants.Accruals, COANameConstants.Loans_and_borrowings_Current, COANameConstants.Tax_payable, COANameConstants.Other_current_liabilities, COANameConstants.Other_non_current_liabilities, COANameConstants.Loans_and_borrowings_Non_current, COANameConstants.Deferred_tax, COANameConstants.Capital, COANameConstants.Retained_earnings, COANameConstants.System, COANameConstants.Dividend_appropriation };
                List<AccountType> accType = _accountTypeService.GetAllAccounyTypeByName(companyId, coaName);
                List<COALookup<string>> lstCoas = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && /*(z.Name != COANameConstants.AccountsPayable && z.Name != COANameConstants.OtherReceivables && z.Name != COANameConstants.AccountsReceivables && z.Name != COANameConstants.OtherPayables) &&*/ z.IsRealCOA == true /*&& z.IsBank != true*/).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                    IsPLAccount = x.Category == "Income Statement" ? true : false,
                    Class = c.Class,
                    Status = c.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                })).OrderBy(e => e.Name).ToList();
                clearingLU.ChartOfAccountLU = lstCoas.OrderBy(s => s.Name).ToList();
                if (clearing != null)
                {
                    long? CoaIds = clearing.COAId;
                    if (CoaIds != null)
                        //lstEditCoa = _chartOfAccountService.GetAllCOAById(companyid, CoaIds).Select(x => new COALookup<string>()
                        //{
                        //    Name = x.Name,
                        //    Id = x.Id,
                        //    RecOrder = x.RecOrder,
                        //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
                        //    IsPLAccount = x.Category == "Income Statement" ? true : false,
                        //    Class = x.Class
                        //}).ToList();
                        lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => x.Id == CoaIds).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsAllowDisAllow = x.DisAllowable == true ? true : false,
                            IsPLAccount = x.Category == "Income Statement" ? true : false,
                            Class = c.Class,
                            Status = c.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        }).OrderBy(d => d.Name)).ToList();
                    clearingLU.ChartOfAccountLU.AddRange(lstEditCoa);

                }
                //if (clearing != null && clearing.GLClearingDetails.Count >= 1)
                //    clearingLU.ChartOfAccountNewLU = _chartOfAccountService.GLClearingCOAs(companyId, true);
                //else
                //    clearingLU.ChartOfAccountNewLU = _chartOfAccountService.GLClearingCOAs(companyId, false);
                //clearingLU.ChartOfAccountLU = account.Select(x => new COALookup<string>()
                //{
                //    Name = x.Name,
                //    Id = x.Id,
                //    RecOrder = x.RecOrder,
                //    PayReceivableAccName = x.Name == COANameConstants.AccountsPayable ? "AP" : x.Name == COANameConstants.OtherPayables ? "OP" : x.Name == COANameConstants.AccountsReceivables ? "AR" : x.Name == COANameConstants.OtherReceivables ? "OR" : string.Empty
                //}).OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, ex, ex.Message);
                throw ex;
            }
            return clearingLU;
        }

        public ClearingModel CreateClearing(Guid id, long companyId, string connectionString)
        {
            LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_CreateClearing_CreateCall_Request_Message);
            ClearingModel clearingModel = new ClearingModel();
            FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
            if (financSettings == null)
            {
                throw new Exception(ClearingValidations.The_Financial_setting_should_be_activated);
            }
            clearingModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
            clearingModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
            List<ClearingDetailModel> lstCDetailModel = new List<ClearingDetailModel>();
            GLClearing clearing = _clearingService.GetClearing(id, companyId);
            if (clearing == null)
            {
                //AppsWorld.GLClearingModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.GLClearing);
                GLClearing lastclrearing = _clearingService.GetByCompanyId(companyId);
                clearingModel.Id = Guid.NewGuid();
                clearingModel.CompanyId = companyId;
                clearingModel.DocType = DocTypeConstants.GLClearing;
                clearingModel.CreatedDate = lastclrearing == null ? DateTime.Now : lastclrearing.CreatedDate;
                clearingModel.FromDate = null;
                clearingModel.ToDate = null;
                clearingModel.DocDate = null;
                clearingModel.IsClearingChecked = false;
                clearingModel.ServiceCompanyId = null;
                clearingModel.COAId = null;
                //FOR DOCNO
                clearingModel.IsDocNoEditable = _autoNumberService.GetAutoById(companyId, DocTypeConstants.GLClearing);
                if (clearingModel.IsDocNoEditable == true)
                {
                    clearingModel.DocNo = _autoService.GetAutonumber(clearingModel.CompanyId, DocTypeConstants.GLClearing, connectionString);
                }


                //bool? isEdit = false;
                //clearingModel.DocNo = GetAutoNumberByEntityType(companyId, lastclrearing, DocTypeConstants.GLClearing, _autoNo, ref isEdit);
                //clearingModel.IsDocNoEditable = isEdit;
            }
            else
            {
                FillCLearing(clearingModel, clearing);
                clearingModel.IsDocNoEditable = _autoNumberService.GetAutoById(companyId, DocTypeConstants.GLClearing);
                clearingModel.Id = clearing.Id;

                List<GLClearingDetail> lstGLDetail = _clearingDetailService.Queryable().Where(s => s.GLClearingId == id).ToList();
                //List<JournalDetail> lstJdetail = _journalDetailService.Query(c => lstGLDetail.Contains(c.DocumentId) && c.ClearingStatus == "Cleared").Select().ToList();


                #region CN and CM for Hyperlink

                string lstCNAppDocId = string.Join(",", lstGLDetail.Where(c => c.DocType == "Credit Note" && (c.DocSubType == "Application" /*|| c.DocSubType == DocTypeConstants.Interco*/)).Select(d => d.DocumentId).ToList());
                string lstCMAppDocId = string.Join(",", lstGLDetail.Where(c => c.DocType == "Credit Memo" && (c.DocSubType == "Application" /*|| c.DocSubType == DocTypeConstants.Interco*/)).Select(d => d.DocumentId).ToList());
                Dictionary<Guid, Guid?> combineIds = new Dictionary<Guid, Guid?>();
                if (!string.IsNullOrEmpty(lstCMAppDocId) && !string.IsNullOrEmpty(lstCNAppDocId))
                {
                    using (con = new SqlConnection(connectionString))
                    {
                        query = $"Select CNA.Id as ID,CNA.InvoiceId as CreditNoteId from Bean.CreditNoteApplication CNA where Id in (Select items from dbo.SplitToTable('{lstCNAppDocId}',','));Select CMA.Id as ID,CMA.CreditMemoId as CreditNoteId from Bean.CreditMemoApplication CMA where Id in (Select items from dbo.SplitToTable('{lstCMAppDocId}',','));";
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        dr = cmd.ExecuteReader();
                        for (int i = 1; i <= 2; i++)
                        {
                            while (dr.Read())
                            {
                                combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditNoteId"] != DBNull.Value ? Guid.Parse(dr["CreditNoteId"].ToString()) : (Guid?)null);

                            }
                            dr.NextResult();
                            while (dr.Read())
                                combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditNoteId"] != DBNull.Value ? Guid.Parse(dr["CreditNoteId"].ToString()) : (Guid?)null);
                        }
                        con.Close();
                    }
                }
                else if (!string.IsNullOrEmpty(lstCMAppDocId))
                {
                    using (con = new SqlConnection(connectionString))
                    {
                        query = $"Select CMA.Id as ID,CMA.CreditMemoId as CreditMemoId from Bean.CreditMemoApplication CMA where Id in (Select items from dbo.SplitToTable('{lstCMAppDocId}',','));";
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                            combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditMemoId"] != DBNull.Value ? Guid.Parse(dr["CreditMemoId"].ToString()) : (Guid?)null);
                        con.Close();
                    }
                }
                else if (!string.IsNullOrEmpty(lstCNAppDocId))
                {
                    using (con = new SqlConnection(connectionString))
                    {
                        query = $"Select CNA.Id as ID,CNA.InvoiceId as CreditNoteId from Bean.CreditNoteApplication CNA where Id in (Select items from dbo.SplitToTable('{lstCNAppDocId}',','));";
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                            combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditNoteId"] != DBNull.Value ? Guid.Parse(dr["CreditNoteId"].ToString()) : (Guid?)null);
                        con.Close();
                    }
                }

                #endregion CN and CM for Hyperlink

                decimal? previousBaseCredit = 0M;
                decimal? previousBaseDebit = 0M;
                int? recOrder = 0;
                decimal? previousDocDebit = 0M;
                decimal? previousDocCredit = 0M;
                foreach (var detail in lstGLDetail)
                {
                    ClearingDetailModel model = new ClearingDetailModel();
                    model.Id = detail.JournalDetailId.Value;
                    model.DocType = detail.DocSubType == "Payroll" ? detail.DocSubType : detail.DocType;
                    model.DocSubType = detail.DocSubType;
                    model.DocDate = detail.DocDate;
                    model.SystemRefNo = detail.SystemRefNo;
                    model.DocNo = detail.DocNo;
                    model.DocCurrency = detail.DocCurrency;
                    //model.DocumentId = detail.DocSubType == DocTypeConstants.Application /*|| detail.DocSubType == DocTypeConstants.Interco*/ ? combineIds.Any(c => c.Value != new Guid()) ? combineIds.Where(d => d.Key == detail.DocumentId).Select(d => d.Value).FirstOrDefault() : (Guid?)null : detail.DocumentId;

                    model.DocumentId = detail.DocumentId;
                    model.DocAmount = (((previousDocDebit != null ? previousDocDebit : previousDocCredit ?? 0) + (detail.DocDebit != null ? detail.DocDebit ?? 0 : 0)) - (detail.DocCredit != null ? detail.DocCredit : 0));
                    model.BaseCurrency = detail.BaseCurrency;
                    model.BaseAmount = (((previousBaseDebit != null ? previousBaseDebit : previousBaseCredit ?? 0) + (detail.BaseDebit != null ? detail.BaseDebit ?? 0 : 0)) - (detail.BaseCredit != null ? detail.BaseCredit : 0));
                    model.DocCredit = detail.DocCredit;
                    model.DocDebit = detail.DocDebit;
                    model.BaseCredit = detail.BaseCredit;
                    model.BaseDebit = detail.BaseDebit;
                    previousBaseDebit = model.BaseDebit != null ? previousBaseDebit + model.BaseDebit : previousBaseDebit - model.BaseCredit;
                    previousBaseCredit = previousBaseCredit + model.BaseCredit;
                    previousDocDebit = model.DocDebit != null ? previousDocDebit + model.DocDebit : previousDocDebit - model.DocCredit;
                    previousDocCredit = previousDocCredit + model.DocCredit;
                    model.IsCheck = false;
                    //model.ClearingStatus = detail.ClearingStatus;
                    //model.RecOrder = detail.RecOrder; 
                    model.RecOrder = recOrder + 1;
                    model.BankClearingDate = detail.ClearingDate;
                    model.EntityName = detail.EntityName;
                    model.AccountDescription = detail.AccountDescription;
                    model.DocumentDetailId = detail.DocSubType == DocTypeConstants.Application /*|| detail.DocSubType == DocTypeConstants.Interco*/ ? combineIds.Any(c => c.Value != new Guid()) ? combineIds.Where(d => d.Key == detail.DocumentId).Select(d => d.Value).FirstOrDefault() : (Guid?)null : detail.DocumentId;
                    lstCDetailModel.Add(model);
                }
                clearingModel.ClearingDetailModel = lstCDetailModel.OrderBy(a => a.DocDate).ThenBy(c => c.DocNo).ToList();
            }
            return clearingModel;
        }

        public List<ClearingDetailModel> CreateClearingDetailNew(DateTime? fromDate, DateTime? toDate, long coaId, long serviceComId, bool? IsClearingChecked, string connectionString)
        {
            LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_CreateClearingDetail_CreateCall_Request_Message);
            List<ClearingDetailModel> clearingDetail = new List<ClearingDetailModel>();
            List<JournalDetail> lstJournal = null;
            lstJournal = _journalDetailService.GetAllJournalD(fromDate, toDate, coaId, serviceComId, IsClearingChecked);

            string lstCNAppDocId = string.Join(",", lstJournal.Where(c => c.DocType == "Credit Note" && (c.DocSubType == "Application" /*|| c.DocSubType == DocTypeConstants.Interco*/)).Select(d => d.DocumentId).ToList());
            string lstCMAppDocId = string.Join(",", lstJournal.Where(c => c.DocType == "Credit Memo" && (c.DocSubType == "Application" /*|| c.DocSubType == DocTypeConstants.Interco*/)).Select(d => d.DocumentId).ToList());

            #region CN and CM for Hyperlink
            Dictionary<Guid, Guid?> combineIds = new Dictionary<Guid, Guid?>();
            if (!string.IsNullOrEmpty(lstCMAppDocId) && !string.IsNullOrEmpty(lstCNAppDocId))
            {
                using (con = new SqlConnection(connectionString))
                {
                    query = $"Select CNA.Id as ID,CNA.InvoiceId as CreditNoteId from Bean.CreditNoteApplication CNA where Id in (Select items from dbo.SplitToTable('{lstCNAppDocId}',','));Select CMA.Id as ID,CMA.CreditMemoId as CreditNoteId from Bean.CreditMemoApplication CMA where Id in (Select items from dbo.SplitToTable('{lstCMAppDocId}',','));";
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();
                    for (int i = 1; i <= 2; i++)
                    {
                        while (dr.Read())
                        {
                            combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditNoteId"] != DBNull.Value ? Guid.Parse(dr["CreditNoteId"].ToString()) : (Guid?)null);
                        }
                        dr.NextResult();
                        while (dr.Read())
                            combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditNoteId"] != DBNull.Value ? Guid.Parse(dr["CreditNoteId"].ToString()) : (Guid?)null);
                    }
                    con.Close();
                }
            }
            else if (!string.IsNullOrEmpty(lstCMAppDocId))
            {
                using (con = new SqlConnection(connectionString))
                {
                    query = $"Select CMA.Id as ID,CMA.CreditMemoId as CreditMemoId from Bean.CreditMemoApplication CMA where Id in (Select items from dbo.SplitToTable('{lstCMAppDocId}',','));";
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditMemoId"] != DBNull.Value ? Guid.Parse(dr["CreditMemoId"].ToString()) : (Guid?)null);
                    con.Close();
                }
            }
            else if (!string.IsNullOrEmpty(lstCNAppDocId))
            {
                using (con = new SqlConnection(connectionString))
                {
                    query = $"Select CNA.Id as ID,CNA.InvoiceId as CreditNoteId from Bean.CreditNoteApplication CNA where Id in (Select items from dbo.SplitToTable('{lstCNAppDocId}',','));";
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                        combineIds.Add(dr["ID"] != DBNull.Value ? Guid.Parse(dr["ID"].ToString()) : new Guid(), dr["CreditNoteId"] != DBNull.Value ? Guid.Parse(dr["CreditNoteId"].ToString()) : (Guid?)null);
                    con.Close();
                }
            }

            #endregion CN and CM for Hyperlink

            lstJournal = (from jd in lstJournal
                          join j in _journalService.Queryable() on jd.JournalId equals j.Id
                          where j.DocumentState != "Void" && j.DocumentState != "Recurring" && j.DocumentState != "Parked" && j.DocumentState != "Deleted" && j.DocumentState != "Reset"
                          select jd).ToList();
            lstJournal = lstJournal.Where(x => x.DocDebit > 0 || x.DocCredit > 0 || x.BaseDebit > 0 || x.BaseCredit > 0).OrderByDescending(s => s.DocDate).ToList();
            if (lstJournal.Any())
            {
                decimal? previousBaseCredit = 0M;
                decimal? previousBaseDebit = 0M;

                decimal? previousDocDebit = 0M;
                decimal? previousDocCredit = 0M;
                foreach (var journal in lstJournal)
                {
                    ClearingDetailModel journalDetail = new ClearingDetailModel();
                    journalDetail.Id = journal.Id;
                    //journalDetail.DocumentId = journal.DocSubType == DocTypeConstants.Application /*|| journal.DocSubType == DocTypeConstants.Interco*/ ? combineIds.Any(c => c.Value != new Guid()) ? combineIds.Where(d => d.Key == journal.DocumentId).Select(d => d.Value).FirstOrDefault() : (Guid?)null : journal.DocumentId;
                    journalDetail.DocumentId = journal.DocumentId;
                    //journalDetail.DocumentId = journal.Id;
                    journalDetail.DocDate = journal.DocDate.Value;
                    //journalDetail.DocType =  journal.DocSubType == "Payroll" ? journal.DocSubType : journal.DocType;
                    journalDetail.DocType = journal.DocType;
                    journalDetail.DocSubType = journal.DocSubType;
                    journalDetail.SystemRefNo = journal.SystemRefNo;
                    journalDetail.DocNo = journal.DocNo;
                    journalDetail.DocSubType = journal.DocSubType;
                    journalDetail.IsCheck = (journal.ClearingDate != null && (journal.ClearingStatus == ClearingState.Cleared || journal.ClearingStatus != null)) ? true : false;
                    //journalDetail.IsCheck = journal.ClearingDate != null ? false : true;
                    journalDetail.DocCurrency = journal.DocCurrency;
                    journalDetail.DocAmount = (((previousDocDebit != null ? previousDocDebit : previousDocCredit ?? 0) + (journal.DocDebit != null ? journal.DocDebit ?? 0 : 0)) - (journal.DocCredit != null ? journal.DocCredit : 0));
                    //var previousDebit = ((previousDebitBal != null ? previousDebitBal : previousCreditBal ?? 0) + (journal.BaseDebit != null ? journal.BaseDebit ?? 0 : 0));
                    //var previousCredit = (journal.BaseCredit != null ? journal.BaseCredit : 0);
                    journalDetail.BaseAmount = (((previousBaseDebit != null ? previousBaseDebit : previousBaseCredit ?? 0) + (journal.BaseDebit != null ? journal.BaseDebit ?? 0 : 0)) - (journal.BaseCredit != null ? journal.BaseCredit : 0));
                    //detail.DocType == DocTypeConstants.Withdrawal || detail.DocSubType == DocTypeConstants.Invoice || detail.DocSubType == DocTypeConstants.DebitNote ? detail.DocCredit :detail.DocType==DocTypeConstants.Bill?detail.DocDebit: -detail.DocDebit,
                    journalDetail.DocCredit = journal.DocCredit;
                    journalDetail.DocDebit = journal.DocDebit;
                    journalDetail.BaseCredit = journal.BaseCredit;
                    journalDetail.BaseDebit = journal.BaseDebit;

                    previousBaseDebit = journalDetail.BaseDebit != null ? previousBaseDebit + journalDetail.BaseDebit : previousBaseDebit - journalDetail.BaseCredit;
                    previousBaseCredit = previousBaseCredit + journalDetail.BaseCredit;
                    previousDocDebit = journalDetail.DocDebit != null ? previousDocDebit + journalDetail.DocDebit : previousDocDebit - journalDetail.DocCredit;
                    previousDocCredit = previousDocCredit + journalDetail.DocCredit;
                    journalDetail.BaseCurrency = journal.BaseCurrency;
                    journalDetail.ClearingStatus = journal.ClearingStatus;
                    journalDetail.BankClearingDate = journal.ClearingDate;
                    journalDetail.AccountDescription = journal.AccountDescription;
                    journalDetail.EntityName = journal.EntityId != null ? _beanEntityService.GetEntityName(journal.EntityId) : "";
                    //journalDetail.DocumentDetailId = journal.DocumentDetailId;
                    journalDetail.DocumentDetailId = journal.DocSubType == DocTypeConstants.Application /*|| journal.DocSubType == DocTypeConstants.Interco*/ ? combineIds.Any(c => c.Value != new Guid()) ? combineIds.Where(d => d.Key == journal.DocumentId).Select(d => d.Value).FirstOrDefault() : (Guid?)null : journal.DocumentId;
                    //detail.DocType == DocTypeConstants.Withdrawal || detail.DocSubType == DocTypeConstants.Invoice || detail.DocSubType == DocTypeConstants.DebitNote ? detail.BaseCredit.Value : detail.DocType == DocTypeConstants.Bill?detail.BaseDebit.Value :-detail.BaseDebit.Value
                    clearingDetail.Add(journalDetail);
                }
            }
            LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_CreateClearingDetail_CreateCall_SuccessFully_Message);
            return clearingDetail.Any() ? clearingDetail.OrderBy(a => a.DocDate).ThenBy(c => c.DocNo).ToList() : clearingDetail;
        }
        #endregion

        #region Kendo Call

        #region GetAllClearingK
        public IQueryable<ClearingModelK> GetAllClearingK(string username, long companyId)
        {
            return _clearingService.GetAllClearingK(username, companyId);
        }
        #endregion

        public IQueryable<ClearedModelK> GetAllClrearedK(long companyId,string username)
        {
            return _clearingService.GetAllClrearedK(companyId,username);
        }

        #endregion

        #region Save Block
        public GLClearing SaveClearing(ClearingModel TObject, string ConnectionString)
        {
			bool isAdd = false;
			bool isDocAdd = false;
			try
            {
				LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_SaveClearing_SaveCall_Request_Message);
				string eventStore = "";
				string _errors = CommonValidation.ValidateObject(TObject);
				if (!string.IsNullOrEmpty(_errors))
				{
					throw new Exception(_errors);
				}
				var AdditionalInfo = new Dictionary<string, object>();
				AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
				Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(ClearingLoggingValidations.ClearingApplicationService, "ObjectSave", AdditionalInfo);
				if (TObject.IsDocNoEditable == true)
				{
					if (TObject.DocNo != null && TObject.DocNo != string.Empty)
						if (_clearingService.IsDocNoExists(TObject.CompanyId, TObject.Id, TObject.DocNo))
						{
							throw new Exception(CommonConstant.Document_number_already_exist);
						}
				}
				bool? multi = _multiCurrencySettingService.GetByCompanyId(TObject.CompanyId);

				JournalDetail journalDetail = _journalDetailService.GetJournalDetalByJournal(TObject.ClearingDetailModel.Select(s => s.Id).FirstOrDefault());
				if (journalDetail != null)
				{

					//GLClearing clearings = _clearingService.GetClearing(clearingId, companyId);
					if (!_financialSettingService.ValidateYearEndLockDate(journalDetail.DocDate.Value, TObject.CompanyId))
					{
						throw new Exception(ClearingValidations.Posting_date_is_in_closed_financial_period_and_cannot_be_posted);
					}

					//Verify if the invoice is out of open financial period and lock password is entered and valid
					if (!_financialSettingService.ValidateFinancialOpenPeriod(journalDetail.DocDate.Value, TObject.CompanyId))
					{
						if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
						{
							throw new Exception(ClearingValidations.Posting_date_is_in_locked_accounting_period_and_cannot_be_posted);
						}
						else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(journalDetail.DocDate.Value, TObject.PeriodLockPassword, TObject.CompanyId))
						{
							throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
						}
					}
				}
				if (multi == false)
				{
					if ((TObject.ClearingDetailModel.Count > 0 || TObject.ClearingDetailModel.Any()))
					{
						if ((TObject.ClearingDetailModel.Where(s => s.IsCheck == true && s.RecordStatus == "Added").Sum(s => s.DocDebit)) != (TObject.ClearingDetailModel.Where(s => s.IsCheck == true && s.RecordStatus == "Added").Sum(s => s.DocCredit)))
						{
							throw new Exception("Should match credit and debit amount's");
						}
					}
				}
				else
				{
					if ((TObject.ClearingDetailModel.Count > 0 || TObject.ClearingDetailModel.Any()))
					{
						if ((TObject.ClearingDetailModel.Where(s => s.IsCheck == true && s.RecordStatus == "Added").Sum(s => s.BaseDebit)) != (TObject.ClearingDetailModel.Where(s => s.IsCheck == true && s.RecordStatus == "Added").Sum(s => s.BaseCredit)))
						{
							throw new Exception("Should match credit and debit amount's");
						}
					}
				}

				if (!(TObject.ClearingDetailModel.Select(s => s.IsCheck).Count() > 0))
				{
					throw new Exception("Atleast check two line items");
				}
				GLClearing clearing = _clearingService.GetClearing(TObject.Id, TObject.CompanyId);
				if (clearing != null)
				{
					//Data concurrency verify
					string timeStamp = "0x" + string.Concat(Array.ConvertAll(clearing.Version, x => x.ToString("X2")));
					if (!timeStamp.Equals(TObject.Version))
						throw new Exception(CommonConstant.Document_has_been_modified_outside);
					eventStore = "update";
					InsertClearing(clearing, TObject);
					clearing.DocNo = TObject.DocNo;
					clearing.SystemRefNo = clearing.DocNo;
					clearing.ModifiedBy = TObject.ModifiedBy;
					clearing.ModifiedDate = DateTime.UtcNow;
					clearing.ObjectState = ObjectState.Modified;
					clearing.TransactionCount = TObject.ClearingDetailModel.Where(s => s.IsCheck == true && s.RecordStatus == "Added").Count();
					UpdateClearingDetail(clearing, TObject, ConnectionString);
					_clearingService.Update(clearing);
				}
				else
				{
                    isAdd = true;
					eventStore = "new";
					clearing = new GLClearing();
					InsertClearing(clearing, TObject);
					clearing.Id = Guid.NewGuid();
					clearing.DocumentState = ClearingState.Cleared;
					if (TObject.ClearingDetailModel.Count > 0 || TObject.ClearingDetailModel.Any())
					{
						clearing.TransactionCount = TObject.ClearingDetailModel.Where(s => s.IsCheck == true && s.RecordStatus == "Added").Count();
						UpdateClearingDetail(clearing, TObject, ConnectionString);
					}
					clearing.Status = RecordStatusEnum.Active;
					clearing.UserCreated = TObject.UserCreated;
					clearing.CreatedDate = DateTime.UtcNow;
					clearing.ObjectState = ObjectState.Added;
					clearing.SystemRefNo = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, DocTypeConstants.GLClearing, ConnectionString) /*GenerateAutoNumberForType(company.Id, DocTypeConstants.GLClearing, company.ShortName)*/ : TObject.DocNo;
                    isDocAdd = true;
					clearing.DocNo = clearing.SystemRefNo;
					_clearingService.Insert(clearing);
				}
				try
				{
					_unitOfWork.SaveChanges();
				}
				catch (DbEntityValidationException e)
				{
					LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, e, e.Message);
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
				LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_SaveClearing_SaveCall_SuccessFully_Message);
				return clearing;
			}
            catch (Exception)
            {
				if (isAdd && isDocAdd && TObject.IsDocNoEditable == false)
				{
                    Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocType, ConnectionString);
				}
				throw;
            }
        }
        #endregion

        #region CreateClearing Reset
        //public DocumentResetModel CreateClearingReset(Guid id, long companyId)
        //{
        //    Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_Request_Message);
        //    DocumentResetModel DDAModel = new DocumentResetModel();
        //    try
        //    {
        //        Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_Request_Message);
        //        FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
        //        if (financSettings == null)
        //        {
        //            throw new Exception(ClearingValidations.The_Financial_setting_should_be_activated);
        //        }
        //        DDAModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
        //        DDAModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
        //        GLClearing clearing = _clearingService.GetClearing(id, companyId);
        //        if (clearing == null)
        //        {
        //            throw new Exception(ClearingValidations.Invalid_Clearing);
        //        }
        //        else
        //        {
        //            DDAModel.CompanyId = companyId;
        //            DDAModel.Id = id;
        //            //DDAModel.FinancialPeriodLockStartDate = _financialSettingService.GetFinancialOpenPeriodStarDate(companyId);
        //            //DDAModel.FinancialPeriodLockEndDate = _financialSettingService.GetFinancialOpenPeriodEndDate(companyId);
        //            //DDAModel.FinancialStartDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
        //            //DDAModel.FinancialEndDate = _financialSettingService.GetFinancialYearEndLockDate(companyId);
        //        }
        //        Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_SuccessFully_Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_SuccessFully_Message);
        //    return DDAModel;
        //}
        #endregion

        #region Save Clearing Reset


        public void SaveUnclearForm(DocumentResetModel TObject, long companyId, string ConnectionString)
        {
            Guid glClearingId = _clearingService.GetClearingByDetailId(TObject.ClearingIds.FirstOrDefault(), companyId);
            GLClearing glClearing = _clearingService.GetClearing(glClearingId, companyId);
            if (glClearing != null)
            {
                //Data concurrency verify
                //string timeStamp = "0x" + string.Concat(Array.ConvertAll(glClearing.Version, x => x.ToString("X2")));
                //if (!timeStamp.Equals(TObject.Version))
                //    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                ////GLClearing clearings = _clearingService.GetClearing(clearingId, companyId);
                //if (!_financialSettingService.ValidateYearEndLockDate(glClearing.DocDate.Value, companyId))
                //{
                //    throw new Exception(ClearingValidations.Posting_date_is_in_closed_financial_period_and_cannot_be_posted);
                //}

                ////Verify if the invoice is out of open financial period and lock password is entered and valid
                //if (!_financialSettingService.ValidateFinancialOpenPeriod(glClearing.DocDate.Value, companyId))
                //{
                //    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                //    {
                //        throw new Exception(ClearingValidations.Posting_date_is_in_locked_accounting_period_and_cannot_be_posted);
                //    }
                //    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(glClearing.DocDate.Value, TObject.PeriodLockPassword, companyId))
                //    {
                //        throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                //    }
                //}
            }
            //List<Guid> lstClearingDetailIds = new List<Guid>();
            GLClearing clearing = new GLClearing();
            List<JournalDetail> lstJDetail = new List<JournalDetail>();
            List<GLClearingDetail> lstClearigD = new List<GLClearingDetail>();
            Dictionary<Guid, decimal?> lstJDetailId = _journalDetailService.GetAllJDByIds(TObject.ClearingIds);
            lstJDetail = _journalDetailService.GetAllDetail(TObject.ClearingIds.Select(d => new Guid?(d)).ToList());
            //List<Guid?> emp = lstJDetailId.Select(d => new Guid?(d.Key)).ToList();
            if (lstJDetail.Any())
            {                //lstJDetail = _journalDetailService.GetAllDetail(lstJDetailId.Select(d => new Guid?(d.Key)).ToList());
                //lstClearingDetailIds = _clearingDetailService.GetAllClearingDetailsByIds(lstJDetailId.Select(d => new Guid?(d.Key)).Distinct().ToList());

                lstClearigD = _clearingDetailService.GetAllClearingDetails(TObject.ClearingIds.Where(d => d != null || d != Guid.Empty).Select(a => new Guid?(a)).ToList());
            }
            if (lstClearigD.Any())
                clearing = _clearingService.GetClearingById(lstClearigD.Select(d => d.GLClearingId).FirstOrDefault(), companyId);

            #region List_System_IBIC_COA
            List<long?> lstCOAIds = new List<long?>();
            using (con = new SqlConnection(ConnectionString))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                query = $"Select COA.Id as COAId from Bean.ChartOfAccount COA inner join Bean.AccountType ATC on COA.AccountTypeId = ATC.Id where ATC.Name in ('Intercompany clearing','Intercompany billing','System') and coa.CompanyId = {companyId}";
                cmd = new SqlCommand(query, con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lstCOAIds.Add(Convert.ToInt64(dr["COAId"]));
                }
                dr.Close();
                con.Close();
            }

            #endregion List_System_IBIC_COA

            using (con = new SqlConnection(ConnectionString))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                int count = 0;
                foreach (var clearingId in TObject.ClearingIds)
                {

                    //JournalDetail journalDetail = _journalDetailService.GetJournalDetalByJournal(clearingId);
                    Guid journalDetail = lstJDetail.Where(c => c.Id == clearingId).Select(g => g.Id).FirstOrDefault();
                    if (journalDetail != null)
                    {
                        //above validations

                    }

                    Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_Request_Message);
                    //string docNo = "-U";
                    //Guid clearingDetailId = _clearingDetailService.GetAllClearingDetail(journalDetail);
                    //Guid clearingDetailId = lstClearingDetailIds.Where(c => c == journalDetail).FirstOrDefault();
                    Guid clearingDetailId = lstClearigD.Where(c => c.JournalDetailId == journalDetail).Select(d => d.GLClearingId).FirstOrDefault();
                    if (lstClearigD.Any())
                    {
                        //GLClearing clearing = _clearingService.GetClearing(clearingDetailId, companyId);
                        //GLClearing clearing = lstClearing.Where(d => d.Id == clearingDetailId && d.CompanyId == companyId).FirstOrDefault();
                        if (clearingDetailId == null)
                        {
                            throw new Exception(ClearingValidations.Invalid_Clearing);
                        }
                        else
                        {
                            if (clearing != null)
                            {
                                clearing.DocNo = clearing.DocNo;
                                clearing.TransactionCount = clearing.TransactionCount - 1;
                                //clearing.CheckAmount = clearing.CheckAmount - (journalDetail.DocCredit ?? 0);
                                clearing.CheckAmount = clearing.CheckAmount - (lstJDetail.Where(c => c.Id == clearingId).Select(g => g.DocCredit).FirstOrDefault() ?? 0);
                                clearing.ObjectState = ObjectState.Modified;
                                clearing.ModifiedDate = DateTime.UtcNow;
                                clearing.ModifiedBy = clearing.UserCreated;
                                if (clearing.TransactionCount <= 0)
                                {
                                    clearing.DocumentState = ClearingState.Unclear;
                                }
                                _clearingService.Update(clearing);
                            }
                        }
                        //var jd = _journalDetailService.Queryable().Where(x => x.Id == clearingId && x.COAId == clearing.COAId).FirstOrDefault();
                        var jd = lstJDetail.Where(x => x.Id == clearingId && x.COAId == clearing.COAId).FirstOrDefault();
                        if (jd != null)
                        {
                            jd.ClearingStatus = null;
                            jd.DocNo = jd.DocNo;
                            jd.SystemRefNo = jd.SystemRefNo;
                            jd.ClearingDate = null;
                            jd.ObjectState = ObjectState.Modified;
                            _journalDetailService.Update(jd);

                            //SqlConnection con = new SqlConnection(ConnectionString);
                            SqlCommand cmd = null;
                            //int updateCount = 0;
                            if (jd.DocType == DocTypeConstants.Withdrawal || jd.DocType == DocTypeConstants.Deposit || jd.DocType == DocTypeConstants.CashPayment)
                            {
                                //if (con.State != ConnectionState.Open)
                                //    con.Open();
                                cmd = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                cmd.ExecuteNonQuery();
                                cmd = null;
                                cmd = new SqlCommand($"Update Bean.WithDrawal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            else if (jd.DocType == DocTypeConstants.BankTransfer)
                            {
                                //if (con.State != ConnectionState.Open)
                                //    con.Open();
                                if (lstCOAIds.Any(d => d == jd.COAId))
                                {

                                    using (cmd = new SqlCommand($"Select Isnull(TRN.ClearCount,0) as Count from Bean.BankTransfer TRN where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                    {
                                        //dr = null;
                                        //dr = cmd.ExecuteReader();
                                        //while (dr.Read())
                                        //    count = dr["Count"] != DBNull.Value ? Convert.ToInt32(dr["Count"]) : 0;
                                        //--count;
                                        //dr.Close();

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.BankTransfer set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    cmd = new SqlCommand("update [Bean].[BankTransferDetail] set [ClearingState] = null where BankTransferId = '" + jd.DocumentId + "' and ServiceCompanyId = '" + jd.ServiceCompanyId + "'", con);
                                    cmd.ExecuteNonQuery();
                                }
                                //con.Close();
                            }
                            else if (jd.DocType == DocTypeConstants.BillCreditMemo)
                            {
                                ////if (con.State != ConnectionState.Open)
                                ////    con.Open();
                                //cmd = new SqlCommand("update[Bean].[CreditMemoDetail] set[ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                //cmd.ExecuteNonQuery();

                                //cmd = null;
                                //cmd = new SqlCommand($"Update Bean.CreditMemo set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                //cmd.ExecuteNonQuery();
                                ////con.Close();

                                //cmd = new SqlCommand("update[Bean].[CreditMemoDetail] set[ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                //cmd.ExecuteNonQuery();
                                //cmd = null;
                                //cmd = new SqlCommand($"Update Bean.CreditMemo set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                //cmd.ExecuteNonQuery();


                                if (jd.DocType == DocTypeConstants.BillCreditMemo && jd.DocSubType == "Application")
                                {

                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditMemoApplication CM where Id='{jd.DocumentId}'", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.CreditMemoApplication set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }


                                }
                                else
                                {
                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditMemo CM where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.CreditMemo set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }

                                }
                            }
                            //else if (jd.DocType == DocTypeConstants.CashPayment)
                            //{
                            //    //if (con.State != ConnectionState.Open)
                            //    //    con.Open();
                            //    cmd = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                            //    cmd.ExecuteNonQuery();
                            //    //con.Close();
                            //}
                            else if (jd.DocType == DocTypeConstants.JournalVocher)
                            {
                                if (jd.DocType == DocTypeConstants.JournalVocher && jd.DocSubType == DocTypeConstants.OpeningBalance)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[OpeningBalanceDetail] set [ClearingState] = null where  OpeningBalanceId= '" + jd.DocumentId + "' and  COAId='" + jd.COAId + "'", con);
                                    cmd.ExecuteNonQuery();

                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.OpeningBalance set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();

                                }
                                else
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[JournalDetail] set [ClearingStatus] = null,ClearingDate=GETUTCDATE(),IsBankReconcile=1 where Id = '" + jd.Id + "'", con);
                                    cmd.ExecuteNonQuery();
                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.Journal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();

                                }

                            }

                            //else if (jd.DocType == DocTypeConstants.JournalVocher)
                            //{
                            //    if (con.State != ConnectionState.Open)
                            //        con.Open();
                            //    cmd = new SqlCommand("update [Bean].[JournalDetail] set [ClearingStatus] = null,ClearingDate=GETDATE(),IsBankReconcile=1 where Id = '" + jd.Id + "'", con);
                            //    updateCount = cmd.ExecuteNonQuery();
                            //    con.Close();
                            //}
                            else if (jd.DocType == DocTypeConstants.Receipt)
                            {

                                if (lstCOAIds.Any(d => d == jd.COAId))
                                {
                                    using (cmd = new SqlCommand($"Select Isnull(REC.ClearCount,0) as Count from Bean.Receipt REC where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Receipt set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[ReceiptBalancingItem] set [ClearingState] = null where ReceiptId = '" + jd.DocumentId + "' and COAId=" + jd.COAId + "", con);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                    cmd = null;
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[ReceiptDetail] set [ClearingState] = null where ReceiptId = '" + jd.DocumentId + "'", con);
                                    cmd.ExecuteNonQuery();

                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.Receipt set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                            }
                            else if (jd.DocType == DocTypeConstants.BillPayment || jd.DocType == DocTypeConstants.PayrollPayment)
                            {
                                //if (con.State != ConnectionState.Open)
                                //    con.Open();
                                if (lstCOAIds.Any(d => d == jd.COAId))
                                {

                                    using (cmd = new SqlCommand($"Select Isnull(PAY.ClearCount,0) as Count from Bean.Payment PAY where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                    {
                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Payment set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    cmd = new SqlCommand("update [Bean].[PaymentDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    cmd.ExecuteNonQuery();
                                }
                                //con.Close();
                            }
                            else if (jd.DocType == DocTypeConstants.DebitNote)
                            {
                                //if (con.State != ConnectionState.Open)
                                //    con.Open();
                                cmd = new SqlCommand("update [Bean].[DebitNoteDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                cmd.ExecuteNonQuery();

                                cmd = null;
                                cmd = new SqlCommand($"Update Bean.DebitNote set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            else if (jd.DocType == DocTypeConstants.CreditNote)
                            {
                                ////if (con.State != ConnectionState.Open)
                                ////    con.Open();
                                //cmd = new SqlCommand("update [Bean].[InvoiceDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                //cmd.ExecuteNonQuery();

                                //cmd = null;
                                //cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                //cmd.ExecuteNonQuery();
                                ////con.Close();

                                if (jd.DocType == DocTypeConstants.CreditNote && jd.DocSubType == "Application")
                                {

                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditNoteApplication CM where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.CreditNoteApplication set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }


                                }
                                else
                                {
                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Invoice CM where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }

                                }
                            }
                            else if (jd.DocType == DocTypeConstants.Invoice)
                            {
                                ////if (con.State != ConnectionState.Open)
                                ////    con.Open();
                                //cmd = new SqlCommand("update [Bean].[InvoiceDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                //cmd.ExecuteNonQuery();

                                //cmd = null;
                                //cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                //cmd.ExecuteNonQuery();
                                ////con.Close();

                                using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Invoice CM where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                {

                                    count = Convert.ToInt32(cmd.ExecuteScalar());
                                    count = count != null ? --count : 0;
                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                    cmd.ExecuteNonQuery();
                                }

                            }
                            else if (jd.DocType == DocTypeConstants.CashSale)
                            {
                                //if (con.State != ConnectionState.Open)
                                //    con.Open();
                                cmd = new SqlCommand("update [Bean].[CashSaleDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                cmd.ExecuteNonQuery();

                                cmd = null;
                                cmd = new SqlCommand($"Update Bean.CashSale set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            else if (jd.DocType == DocTypeConstants.Bills)
                            {
                                ////if (con.State != ConnectionState.Open)
                                ////    con.Open();
                                //cmd = new SqlCommand("update [Bean].[BillDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                //cmd.ExecuteNonQuery();

                                //cmd = null;
                                //cmd = new SqlCommand($"Update Bean.Bill set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                //cmd.ExecuteNonQuery();
                                ////con.Close();
                                using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Bill CM where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                {

                                    count = Convert.ToInt32(cmd.ExecuteScalar());
                                    count = count != null ? --count : 0;
                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.Bill set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                    cmd.ExecuteNonQuery();
                                }

                            }


                            #region Commented
                            //}
                            //switch (jd.DocType)
                            //{

                            //    case DocTypeConstants.Withdrawal:
                            //        if (con.State != ConnectionState.Open)
                            //            con.Open();
                            //        SqlCommand cmd = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                            //        int updateCount = cmd.ExecuteNonQuery();
                            //        con.Close();
                            //        
                            //    case DocTypeConstants.Deposit:
                            //        if (con.State != ConnectionState.Open)
                            //            con.Open();
                            //        SqlCommand cmd1 = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                            //        int updateCount1 = cmd1.ExecuteNonQuery();
                            //        
                            //    case DocTypeConstants.BankTransfer:
                            //        if (con.State != ConnectionState.Open)
                            //            con.Open();
                            //        SqlCommand cmd2 = new SqlCommand("update [Bean].[BankTransferDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                            //        int updateCount2 = cmd2.ExecuteNonQuery();
                            //        
                            //    case DocTypeConstants.CashPayment:
                            //        if (con.State != ConnectionState.Open)
                            //            con.Open();
                            //        SqlCommand cmd3 = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                            //        int updateCount3 = cmd3.ExecuteNonQuery();
                            //        
                            //    case DocTypeConstants.JournalVocher:
                            //        if (con.State != ConnectionState.Open)
                            //            con.Open();
                            //        SqlCommand cmd4 = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentId + "'", con);
                            //        int updateCount4 = cmd4.ExecuteNonQuery();
                            //        

                            //}
                            #endregion
                            //GLClearingDetail clearingDetail = _clearingDetailService.GetAllClearingByDetailId(clearingId);
                            GLClearingDetail clearingDetail = lstClearigD.Where(c => c.JournalDetailId == clearingId).FirstOrDefault();
                            if (clearingDetail != null)
                            {
                                clearingDetail.ObjectState = ObjectState.Deleted;
                                //_clearingDetailService.Delete(clearingDetail);
                            }
                        }
                    }
                    //try
                    //{
                    //    _unitOfWork.SaveChanges();
                    //    Log.Logger.ZInfo(ClearingLoggingValidations.Log_ClearingReset_SaveCall_SuccessFully_Message);
                    //}
                    //catch (Exception ex)
                    //{
                    //    LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, ex, ex.Message);
                    //    Log.Logger.ZCritical(ex.StackTrace);
                    //    throw ex;
                    //}
                    //LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_SuccessFully_Message);
                    //return DDAModel;
                }
                if (con.State == ConnectionState.Open)
                    con.Close();

            }
            try
            {
                _unitOfWork.SaveChanges();
                Log.Logger.ZInfo(ClearingLoggingValidations.Log_ClearingReset_SaveCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_SuccessFully_Message);
        }

        public void SaveClearingGrid(UnclearModel TObject, long companyId, string ConnectionString)
        {
            //foreach (var clearIds in TObject.ClearingIds)
            //{
            Log.Logger.ZInfo(ClearingLoggingValidations.Log_CreateClearingReset_CreateCall_Request_Message);
            //  string docNo = "-U";
            GLClearing clearing = _clearingService.GetClearing(TObject.Id, companyId);
            if (clearing == null)
            {
                throw new Exception(ClearingValidations.Invalid_Clearing);
            }
            else
            {

                //GLClearing clearings = _clearingService.GetClearing(clearingId, companyId);
                if (!_financialSettingService.ValidateYearEndLockDate(clearing.ClearingDate.Value, companyId))
                {
                    throw new Exception(ClearingValidations.Posting_date_is_in_closed_financial_period_and_cannot_be_posted);
                }

                //Verify if the invoice is out of open financial period and lock password is entered and valid
                if (!_financialSettingService.ValidateFinancialOpenPeriod(clearing.ClearingDate.Value, companyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new Exception(ClearingValidations.Posting_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(clearing.ClearingDate.Value, TObject.PeriodLockPassword, companyId))
                    {
                        throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                    }
                }
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(clearing.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                clearing.DocumentState = ClearingState.Unclear;
                clearing.DocNo = clearing.DocNo;
                clearing.TransactionCount = 0;
                clearing.CheckAmount = 0;
                clearing.ObjectState = ObjectState.Modified;
                clearing.ModifiedDate = DateTime.UtcNow;
                _clearingService.Update(clearing);
            }

            #region List_System_IBIC_COA
            List<long?> lstCOAIds = new List<long?>();
            using (con = new SqlConnection(ConnectionString))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                query = $"Select COA.Id as COAId from Bean.ChartOfAccount COA inner join Bean.AccountType ATC on COA.AccountTypeId = ATC.Id where ATC.Name in ('Intercompany clearing','Intercompany billing','System') and coa.CompanyId = {companyId}";
                cmd = new SqlCommand(query, con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lstCOAIds.Add(Convert.ToInt64(dr["COAId"]));
                }
                dr.Close();
                con.Close();
            }

            #endregion List_System_IBIC_COA

            int count = 0;
            var gldetail = clearing.GLClearingDetails;
            if (gldetail.Any())
            {
                List<JournalDetail> lstJDs = _journalDetailService.GetAllJDByDocId(clearing.GLClearingDetails.Where(c => c.DocumentId != null || c.DocumentId != Guid.NewGuid()).Select(d => d.DocumentId).ToList());
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();

                    foreach (var detail in gldetail)
                    {
                        //var jd = _journalDetailService.Queryable().Where(x => x.DocumentId == detail.DocumentId && x.COAId == clearing.COAId && x.Id == detail.JournalDetailId).FirstOrDefault();
                        if (lstJDs.Any())
                        {
                            var jd = lstJDs.Where(x => x.DocumentId == detail.DocumentId && x.COAId == clearing.COAId && x.Id == detail.JournalDetailId).FirstOrDefault();
                            if (jd != null)
                            {
                                jd.ClearingStatus = null;
                                jd.DocNo = jd.DocNo;
                                jd.SystemRefNo = jd.SystemRefNo;
                                jd.ClearingDate = null;
                                jd.ObjectState = ObjectState.Modified;
                                _journalDetailService.Update(jd);
                                //SqlConnection con = new SqlConnection(ConnectionString);
                                SqlCommand cmd = null;

                                if (jd.DocType == DocTypeConstants.Withdrawal || jd.DocType == DocTypeConstants.Deposit || jd.DocType == DocTypeConstants.CashPayment)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    cmd.ExecuteNonQuery();

                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.WithDrawal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                                else if (jd.DocType == DocTypeConstants.BankTransfer)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    if (lstCOAIds.Any(d => d == jd.COAId))
                                    {
                                        using (cmd = new SqlCommand($"Select Isnull(TRF.ClearCount,0) as Count from Bean.BankTransfer TRF where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.BankTransfer set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("update [Bean].[BankTransferDetail] set [ClearingState] = null where BankTransferId = '" + jd.DocumentId + "' and ServiceCompanyId = '" + jd.ServiceCompanyId + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    //con.Close();
                                }
                                else if (jd.DocType == DocTypeConstants.BillCreditMemo)
                                {

                                    //cmd = new SqlCommand("update[Bean].[CreditMemoDetail] set[ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    //cmd.ExecuteNonQuery();
                                    //cmd = null;
                                    //cmd = new SqlCommand($"Update Bean.CreditMemo set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    //cmd.ExecuteNonQuery();


                                    if (jd.DocType == DocTypeConstants.BillCreditMemo && jd.DocSubType == "Application")
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditMemoApplication CM where Id='{jd.DocumentId}'", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.CreditMemoApplication set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }


                                    }
                                    else
                                    {
                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditMemo CM where Id='{detail.DocumentId}' and CompanyId={companyId}", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.CreditMemo set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }

                                    }



                                }
                                //else if (jd.DocType == DocTypeConstants.CashPayment)
                                //{
                                //    //if (con.State != ConnectionState.Open)
                                //    //    con.Open();
                                //    cmd = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                //    cmd.ExecuteNonQuery();

                                //    //con.Close();
                                //}
                                else if (jd.DocType == DocTypeConstants.JournalVocher)
                                {
                                    if (jd.DocType == DocTypeConstants.JournalVocher && jd.DocSubType == DocTypeConstants.OpeningBalance)
                                    {
                                        //if (con.State != ConnectionState.Open)
                                        //    con.Open();
                                        cmd = new SqlCommand("update [Bean].[OpeningBalanceDetail] set [ClearingState] = null where  OpeningBalanceId= '" + jd.DocumentId + "' and  COAId='" + jd.COAId + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.OpeningBalance set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                        cmd.ExecuteNonQuery();
                                        //con.Close();

                                    }
                                    else
                                    {
                                        //if (con.State != ConnectionState.Open)
                                        //    con.Open();
                                        cmd = new SqlCommand("update [Bean].[JournalDetail] set [ClearingStatus] = null,ClearingDate=GETUTCDATE(),IsBankReconcile=1 where Id = '" + jd.Id + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Journal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                        cmd.ExecuteNonQuery();
                                        //con.Close();
                                    }
                                }

                                //else if (jd.DocType == DocTypeConstants.JournalVocher)
                                //{
                                //    if (con.State != ConnectionState.Open)
                                //        con.Open();
                                //    cmd = new SqlCommand("update [Bean].[JournalDetail] set [ClearingStatus] = null,ClearingDate=GETDATE(),IsBankReconcile=1 where Id = '" + jd.Id + "'", con);
                                //    updateCount = cmd.ExecuteNonQuery();
                                //    con.Close();
                                //}
                                else if (jd.DocType == DocTypeConstants.Receipt)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    if (lstCOAIds.Any(d => d == jd.COAId))
                                    {
                                        using (cmd = new SqlCommand($"Select Isnull(REC.ClearCount,0) as Count from Bean.Receipt REC where Id='{detail.DocumentId}' and CompanyId={companyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Receipt set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("update [Bean].[ReceiptBalancingItem] set [ClearingState] = 'Cleared' where ReceiptId = '" + jd.DocumentId + "' and COAId=" + jd.COAId + "", con);
                                        cmd.ExecuteNonQuery();
                                        //con.Close();
                                        cmd = null;
                                        //if (con.State != ConnectionState.Open)
                                        //    con.Open();
                                        cmd = new SqlCommand("update [Bean].[ReceiptDetail] set [ClearingState] = null where ReceiptId = '" + jd.DocumentId + "'", con);
                                        cmd.ExecuteNonQuery();
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Receipt set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                        cmd.ExecuteNonQuery();
                                        //con.Close();
                                    }
                                }
                                else if (jd.DocType == DocTypeConstants.BillPayment || jd.DocType == DocTypeConstants.PayrollPayment)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    if (lstCOAIds.Any(d => d == jd.COAId))
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(PAY.ClearCount,0) as Count from Bean.Payment PAY where Id='{jd.DocumentId}' and CompanyId={companyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Payment set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + jd.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        cmd = new SqlCommand("update [Bean].[PaymentDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                        cmd.ExecuteNonQuery();
                                    }
                                    //con.Close();
                                }
                                else if (jd.DocType == DocTypeConstants.DebitNote)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[DebitNoteDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    cmd.ExecuteNonQuery();
                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.DebitNote set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                                else if (jd.DocType == DocTypeConstants.CreditNote)
                                {
                                    ////if (con.State != ConnectionState.Open)
                                    ////    con.Open();
                                    //cmd = new SqlCommand("update [Bean].[InvoiceDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    //cmd.ExecuteNonQuery();
                                    //cmd = null;
                                    //cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    //cmd.ExecuteNonQuery();
                                    ////con.Close();

                                    if (jd.DocType == DocTypeConstants.CreditNote && jd.DocSubType == "Application")
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(CM.ClearCount,0) as Count From Bean.CreditNoteApplication CM where Id='{detail.DocumentId}' and CompanyId={companyId}", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.CreditNoteApplication set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }


                                    }
                                    else
                                    {
                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Invoice CM where Id='{detail.DocumentId}' and CompanyId={companyId}", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? --count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }

                                    }

                                }
                                else if (jd.DocType == DocTypeConstants.Invoice)
                                {
                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Invoice CM where Id='{detail.DocumentId}' and CompanyId={companyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }

                                    ////if (con.State != ConnectionState.Open)
                                    ////    con.Open();
                                    //cmd = new SqlCommand("update [Bean].[InvoiceDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    //cmd.ExecuteNonQuery();
                                    //cmd = null;
                                    //cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    //cmd.ExecuteNonQuery();
                                    ////con.Close();
                                }
                                else if (jd.DocType == DocTypeConstants.CashSale)
                                {
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    cmd = new SqlCommand("update [Bean].[CashSaleDetail] set [ClearingState] = null where Id = '" + jd.DocumentDetailId + "'", con);
                                    cmd.ExecuteNonQuery();
                                    cmd = null;
                                    cmd = new SqlCommand($"Update Bean.CashSale set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    //con.Close();

                                }
                                else if (jd.DocType == DocTypeConstants.Bills)
                                {
                                    ////if (con.State != ConnectionState.Open)
                                    ////    con.Open();
                                    //cmd = new SqlCommand($"update [Bean].[BillDetail] set [ClearingState] = null where Id = '{jd.DocumentDetailId}'", con);
                                    //cmd.ExecuteNonQuery();
                                    //cmd = null;
                                    //cmd = new SqlCommand($"Update Bean.Bill set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{jd.DocumentId}'", con/*, transaction*/);
                                    //cmd.ExecuteNonQuery();
                                    ////con.Close();
                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Bill CM where Id='{detail.DocumentId}' and CompanyId={companyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? --count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Bill set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }

                                }
                            }
                        }
                    }
                    if (con.State == ConnectionState.Open)
                        con.Close();

                }
            }

            //List<GLClearingDetail> lstClearigD = _clearingDetailService.GetAllClearingDetails(clearing.GLClearingDetails.Where(d => d.DocumentId != null || d.DocumentId != Guid.Empty).Select(a => a.DocumentId).ToList());
            //foreach (var clear in clearing.GLClearingDetails.ToList())
            //{
            //    //GLClearingDetail clearingDetail = _clearingDetailService.GetAllClearingByDetailId(clear.JournalDetailId.Value);
            //    GLClearingDetail clearingDetail = lstClearigD.Where(d => d.DocumentDetailId == clear.DocumentDetailId).FirstOrDefault();
            //    if (clearingDetail != null)
            //    {
            //        clearingDetail.ObjectState = ObjectState.Deleted;
            //        //_clearingDetailService.Delete(clearingDetail);

            //    }
            //    //_clearingDetailService.Delete(clear);
            //}

            List<GLClearingDetail> lstClearigD = _clearingDetailService.GetAllClearingDetails(clearing.GLClearingDetails.Where(d => d.JournalDetailId != null || d.JournalDetailId != Guid.Empty).Select(a => a.JournalDetailId).ToList());
            foreach (var clear in clearing.GLClearingDetails.ToList())
            {
                //GLClearingDetail clearingDetail = _clearingDetailService.GetAllClearingByDetailId(clear.JournalDetailId.Value);
                GLClearingDetail clearingDetail = lstClearigD.Where(d => d.JournalDetailId == clear.JournalDetailId).FirstOrDefault();
                if (clearingDetail != null)
                {
                    clearingDetail.ObjectState = ObjectState.Deleted;
                    //_clearingDetailService.Delete(clearingDetail);

                }
                //_clearingDetailService.Delete(clear);
            }
            //}
            try
            {
                _unitOfWork.SaveChanges();
                LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService, ClearingLoggingValidations.Log_ClearingReset_SaveCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, ex, ex.Message);
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
        }
        #endregion

        #region AutoNumber Block
        string value = "";
        public string GenerateAutoNumberForType(long CompanyId, string Type, string companyCode)
        {
            AppsWorld.GLClearingModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(CompanyId, Type);
            string generatedAutoNumber = "";

            if (Type == DocTypeConstants.GLClearing)
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
                    AppsWorld.GLClearingModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AppsWorld.GLClearingModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.GLClearingModule.Entities.AutoNumberCompany();
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
            List<GLClearing> lstCreditMemo = null;
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
                companyFormatHere = companyFormatHere.Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
                currentMonth = DateTime.Now.Month;
                ifMonthContains = true;
            }
            else if (companyFormatHere.Contains("{COMPANYCODE}"))
            {
                companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
            }
            double val = 0;
            if (Type == DocTypeConstants.GLClearing)
            {
                lstCreditMemo = _clearingService.GelAllClearings(companyId);

                if (lstCreditMemo.Any() && ifMonthContains == true)
                {
                    AppsWorld.GLClearingModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastCretedDate = lstCreditMemo.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstCreditMemo.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (lastCretedDate == currentMonth)
                        {
                            foreach (var bill in lstCreditMemo)
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

                else if (lstCreditMemo.Any() && ifMonthContains == false)
                {
                    AppsWorld.GLClearingModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var creditMemo in lstCreditMemo)
                    {
                        if (creditMemo.SystemRefNo != autonumber.Preview)
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
        private string GetNewNumber(List<GLClearing> lstCashsale, string type, string outputNumber, string counter, string format, int counterLength)
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

        #region PrivateMethods
        private string GetAutoNumberByEntityType(long companyId, GLClearing lastInvoice, string entityType, AppsWorld.GLClearingModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.GLClearingModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    //outPutNumber = GetNewInvoiceDocumentNumber(entityType, companyId);
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

        private void FillCLearing(ClearingModel model, GLClearing clearing)
        {
            //long? coiaId = clearing.COAId2;
            model.CompanyId = clearing.CompanyId;
            model.COAId = clearing.COAId;
            model.ServiceCompanyId = clearing.ServiceCompanyId;
            //model.DocDate = clearing.DocDate;
            //model.DocDescription = clearing.DocDescription;
            model.DocNo = clearing.DocNo;
            model.DocType = clearing.DocType;
            //model.IsMultiCurrency = clearing.IsMultiCurrency;
            model.ModifiedBy = clearing.ModifiedBy;
            model.ModifiedDate = clearing.ModifiedDate;
            model.UserCreated = clearing.UserCreated;
            model.CreatedDate = clearing.CreatedDate;
            //model.Remarks = clearing.Remarks;
            //model.SystemRefNo = clearing.SystemRefNo;
            model.DocumentState = clearing.DocumentState;
            model.Version = "0x" + string.Concat(Array.ConvertAll(clearing.Version, x => x.ToString("X2")));
            model.IsLocked = clearing.IsLocked;
            //model.CrDr = clearing.CrDr;
            //if (coiaId != null && coiaId > 0)
            //{
            //    var coaId = _chartOfAccountService.GetChartOfAccountById(coiaId.Value);
            //    model.COAId2 = coaId.Id;
            //}
            //model.CheckAmount = clearing.CheckAmount;

            //if (clearing.EntityId != null)
            //{
            //    model.EntityId = clearing.EntityId;
            //    BeanEntity entity = _beanEntityService.GetEntityById(clearing.EntityId.Value);
            //    if (entity != null)
            //    {
            //        model.EntityName = entity.Name;
            //        model.EntityType = entity.IsCustomer == true ? "Customer" : entity.IsVendor == true ? "Vendor" : null;
            //    }
            //}
        }
        private void FillClearingDetail(GLClearingDetail detailMedel, ClearingDetailModel clearingDetail)
        {
            detailMedel.DocCurrency = clearingDetail.DocCurrency;
            detailMedel.GLClearingId = clearingDetail.GLClearingId;
            detailMedel.SystemRefNo = clearingDetail.DocNo;
            detailMedel.DocType = clearingDetail.DocType;
            detailMedel.DocNo = clearingDetail.DocNo;
            detailMedel.DocDate = clearingDetail.DocDate;
            detailMedel.DocAmount = clearingDetail.DocAmount != null ? clearingDetail.DocAmount.Value : 0;
            detailMedel.DocumentId = clearingDetail.DocumentId;
            //detailMedel.CrDr = clearingDetail.CrDr;
            detailMedel.BaseAmount = clearingDetail.BaseAmount != null ? clearingDetail.BaseAmount.Value : 0;
            detailMedel.BaseCurrency = clearingDetail.BaseCurrency;
            detailMedel.DocCredit = clearingDetail.DocCredit;
            detailMedel.DocDebit = clearingDetail.DocDebit;
            detailMedel.BaseCredit = clearingDetail.BaseCredit;
            detailMedel.BaseDebit = clearingDetail.BaseDebit;
            detailMedel.EntityName = clearingDetail.EntityName;
            detailMedel.AccountDescription = clearingDetail.AccountDescription;
            detailMedel.JournalDetailId = clearingDetail.Id;
            detailMedel.DocSubType = clearingDetail.DocSubType;
        }
        private void InsertClearing(GLClearing clearing, ClearingModel model)
        {
            clearing.CompanyId = model.CompanyId;
            clearing.COAId = model.COAId.Value;
            clearing.ServiceCompanyId = model.ServiceCompanyId.Value;
            clearing.DocNo = model.DocNo;
            clearing.DocType = model.DocType;
            clearing.ModifiedBy = model.ModifiedBy;
            clearing.ModifiedDate = model.ModifiedDate;
            clearing.CheckAmount = model.ClearingDetailModel.Where(s => s.RecordStatus == "Added").Sum(s => s.DocDebit);
            clearing.ExCurrency = model.ExCurrency;
        }
        private void UpdateClearingDetail(GLClearing clearing, ClearingModel Tobject, string ConnectionString)
        {
            try
            {
                List<ClearingDetailModel> anotherRecords = Tobject.ClearingDetailModel.Where(c => c.RecordStatus != "Deleted" && c.IsCheck == true && c.RecordStatus == "Added").ToList();
                List<Guid> lstCLDetailIds = anotherRecords.Select(c => c.Id).ToList();
                List<GLClearingDetail> lstGLDetail = _clearingDetailService.Query(c => lstCLDetailIds.Contains(c.Id)).Select().ToList();
                List<JournalDetail> lstJdetail = _journalDetailService.Query(c => lstCLDetailIds.Contains(c.Id)).Select().ToList();
                int? recOrder = 0;
                DateTime? clearingDate = anotherRecords.OrderByDescending(s => s.DocDate).Select(s => s.DocDate).First();

                #region List_System_IBIC_COA
                List<long?> lstCOAIds = new List<long?>();
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    query = $"Select COA.Id as COAId from Bean.ChartOfAccount COA inner join Bean.AccountType ATC on COA.AccountTypeId = ATC.Id where ATC.Name in ('Intercompany clearing','Intercompany billing','System') and coa.CompanyId = {Tobject.CompanyId}";
                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lstCOAIds.Add(Convert.ToInt64(dr["COAId"]));
                    }
                    dr.Close();
                    con.Close();
                }

                #endregion List_System_IBIC_COA


                clearing.ClearingDate = clearingDate;
                if (anotherRecords.Any())
                {

                    foreach (var item in anotherRecords)
                    {
                        GLClearingDetail detail = new GLClearingDetail();
                        if (item.RecordStatus != "Added")
                        {
                            GLClearingDetail glDetail = lstGLDetail.Where(c => c.Id == item.Id).FirstOrDefault();
                            if (glDetail != null)
                            {
                                FillClearingDetail(glDetail, item);
                                glDetail.RecOrder = recOrder + 1;
                                recOrder = glDetail.RecOrder;
                                glDetail.ObjectState = ObjectState.Modified;
                                _clearingDetailService.Update(glDetail);
                            }
                        }
                        else if (item.RecordStatus == "Added")
                        {
                            FillClearingDetail(detail, item);
                            detail.Id = Guid.NewGuid();
                            detail.GLClearingId = clearing.Id;
                            detail.ClearingDate = clearing.ClearingDate;
                            detail.RecOrder = recOrder + 1;
                            recOrder = detail.RecOrder;
                            detail.ObjectState = ObjectState.Added;
                            _clearingDetailService.Insert(detail);
                        }
                    }
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        int count = 0;
                        foreach (var detail in lstJdetail)
                        {
                            count = 0;
                            if (detail != null)
                            {
                                detail.ClearingStatus = ClearingState.Cleared;
                                detail.ClearingDate = clearingDate;
                                detail.ObjectState = ObjectState.Modified;
                                _journalDetailService.Update(detail);
                            }

                            switch (detail.DocType)
                            {

                                case DocTypeConstants.Withdrawal:

                                    SqlCommand cmd = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con/*, transaction*/);
                                    cmd.ExecuteNonQuery();
                                    SqlCommand cmdwith = new SqlCommand($"Update Bean.WithDrawal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{detail.DocumentId}'", con/*, transaction*/);
                                    cmdwith.ExecuteNonQuery();

                                    break;
                                case DocTypeConstants.Deposit:

                                    SqlCommand cmd1 = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con/*, transaction1*/);
                                    cmd1.ExecuteNonQuery();
                                    string s = "Update Bean.WithDrawal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='" + detail.DocumentId + "'";
                                    SqlCommand cmdDeposit = new SqlCommand(s, con/*, transaction1*/);
                                    cmdDeposit.ExecuteNonQuery();

                                    break;
                                case DocTypeConstants.BankTransfer:

                                    if (lstCOAIds.Any(d => d == detail.COAId))
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(TRN.ClearCount,0) as Count from Bean.BankTransfer TRN where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;

                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.BankTransfer set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        SqlCommand cmd2 = new SqlCommand("update [Bean].[BankTransferDetail] set [ClearingState] = 'Cleared' where BankTransferId = '" + detail.DocumentId + "' and ServiceCompanyId = '" + detail.ServiceCompanyId + "'", con);
                                        cmd2.ExecuteNonQuery();
                                        SqlCommand cmdTransfer = new SqlCommand(" Update Bean.BankTransfer set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='" + detail.DocumentId + "'", con/*, transaction2*/);
                                        cmdTransfer.ExecuteNonQuery();
                                    }

                                    break;
                                case DocTypeConstants.BillCreditMemo:

                                    if (detail.DocType == DocTypeConstants.BillCreditMemo && detail.DocSubType == "Application")
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditMemoApplication CM where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.CreditMemoApplication set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditMemo CM where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.CreditMemo set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                        break;
                                    }

                                case DocTypeConstants.CashPayment:
                                    SqlCommand cmd14 = new SqlCommand("update [Bean].[WithdrawalDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con/*, transaction3*/);
                                    cmd14.ExecuteNonQuery();

                                    SqlCommand cmdCreditMoemo = new SqlCommand("Update Bean.WithDrawal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                    cmdCreditMoemo.ExecuteNonQuery();
                                    break;
                                case DocTypeConstants.JournalVocher:
                                    if (detail.DocSubType == DocTypeConstants.OpeningBalance)
                                    {
                                        SqlCommand cmd8 = new SqlCommand("update [Bean].[OpeningBalanceDetail] set [ClearingState] = 'Cleared' where  OpeningBalanceId= '" + detail.DocumentId + "' and  COAId='" + detail.COAId + "'", con);
                                        cmd8.ExecuteNonQuery();
                                        SqlCommand cmdpayOpeningBalance = new SqlCommand("Update Bean.OpeningBalance set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmdpayOpeningBalance.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        SqlCommand cmd4 = new SqlCommand("update [Bean].[JournalDetail] set [ClearingStatus] = 'Cleared',ClearingDate=GETUTCDATE(),IsBankReconcile=1 where Id = '" + detail.Id + "'", con);
                                        cmd4.ExecuteNonQuery();
                                    }
                                    break;
                                case DocTypeConstants.Receipt:
                                    if (lstCOAIds.Any(d => d == detail.COAId))
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(REC.ClearCount,0) as Count from Bean.Receipt REC where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Receipt set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        SqlCommand cmd5 = new SqlCommand("update [Bean].[ReceiptBalancingItem] set [ClearingState] = 'Cleared' where ReceiptId = '" + detail.DocumentId + "' and COAId=" + detail.COAId + "", con);
                                        cmd5.ExecuteNonQuery();
                                        SqlCommand cmd16 = new SqlCommand("update [Bean].[ReceiptDetail] set [ClearingState] = 'Cleared' where ReceiptId = '" + detail.DocumentId + "'", con);
                                        cmd16.ExecuteNonQuery();
                                        SqlCommand cmdCashReceipt = new SqlCommand("Update Bean.Receipt set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmdCashReceipt.ExecuteNonQuery();
                                    }
                                    break;
                                case DocTypeConstants.BillPayment:
                                    if (lstCOAIds.Any(d => d == detail.COAId))
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(PAY.ClearCount,0) as Count from Bean.Payment PAY where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Payment set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        SqlCommand cmd6 = new SqlCommand("update [Bean].[PaymentDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con);
                                        cmd6.ExecuteNonQuery();
                                        SqlCommand cmdPayment = new SqlCommand("Update Bean.Payment set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmdPayment.ExecuteNonQuery();
                                    }
                                    break;
                                case DocTypeConstants.PayrollPayment:
                                    if (lstCOAIds.Any(d => d == detail.COAId))
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(PAY.ClearCount,0) as Count from Bean.Payment PAY where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {
                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Payment set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        SqlCommand cmd7 = new SqlCommand("update [Bean].[PaymentDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con);
                                        cmd7.ExecuteNonQuery();
                                        SqlCommand cmdpayPayment = new SqlCommand("Update Bean.Payment set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmdpayPayment.ExecuteNonQuery();
                                    }
                                    break;
                                case DocTypeConstants.DebitNote:
                                    SqlCommand cmd9 = new SqlCommand("update [Bean].[DebitNoteDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con);
                                    cmd9.ExecuteNonQuery();
                                    SqlCommand cmdDebitNote = new SqlCommand("Update Bean.DebitNote set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                    cmdDebitNote.ExecuteNonQuery();
                                    break;
                                case DocTypeConstants.CreditNote:
                                    if (detail.DocType == DocTypeConstants.CreditNote && detail.DocSubType == "Application")
                                    {

                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.CreditNoteApplication CM where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.CreditNoteApplication set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                        break;
                                    }
                                    else
                                    {
                                        using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Invoice CM where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                        {

                                            count = Convert.ToInt32(cmd.ExecuteScalar());
                                            count = count != null ? ++count : 0;
                                            cmd = null;
                                            cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                            cmd.ExecuteNonQuery();
                                        }
                                        break;
                                    }
                                case DocTypeConstants.Invoice:

                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Invoice CM where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? ++count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Invoice set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                                case DocTypeConstants.CashSale:
                                    SqlCommand cmd12 = new SqlCommand("update [Bean].[CashSaleDetail] set [ClearingState] = 'Cleared' where Id = '" + detail.DocumentDetailId + "'", con);
                                    cmd12.ExecuteNonQuery();
                                    SqlCommand cmdCashSales = new SqlCommand("Update Bean.CashSale set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                    cmdCashSales.ExecuteNonQuery();
                                    break;
                                case DocTypeConstants.Bills:

                                    using (cmd = new SqlCommand($"Select Isnull(Cm.ClearCount,0) as Count From Bean.Bill CM where Id='{detail.DocumentId}' and CompanyId={Tobject.CompanyId}", con))
                                    {

                                        count = Convert.ToInt32(cmd.ExecuteScalar());
                                        count = count != null ? ++count : 0;
                                        cmd = null;
                                        cmd = new SqlCommand($"Update Bean.Bill set ModifiedBy='System',ModifiedDate=GETUTCDATE(),ClearCount={count} where Id = '" + detail.DocumentId + "'", con/*, transaction3*/);
                                        cmd.ExecuteNonQuery();
                                    }
                                    break;
                            }
                            query = $"Update Bean.Journal set ModifiedBy='System',ModifiedDate=GETUTCDATE() where Id ='{detail.JournalId}'";
                            SqlCommand cmdDepositWithdrawal = new SqlCommand(query, con);
                            cmdDepositWithdrawal.ExecuteNonQuery();
                        }
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, ex, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Posting
        public void SaveClearingJournal(JVModel clientModel)
        {
            //LoggingHelper.LogMessage(ClearingConstant.GLClearingApplicationService,clientModel);
            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {

                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == ClearingConstant.IdentityBean)
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
                //Log.Logger.ZCritical(ex.StackTrace);
                LoggingHelper.LogError(ClearingConstant.GLClearingApplicationService, ex, ex.Message);
                var message = ex.Message;
            }
        }
        private void FillJournal(JVModel headJournal, GLClearing clearing)
        {
            string strServiceCompany = _companyService.GetById(clearing.ServiceCompanyId).ShortName;

            //if (isNew)
            //    headJournal.Id = Guid.NewGuid();
            //else
            headJournal.Id = clearing.Id;
            FillJv(headJournal, clearing);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel Jmodel = new JVVDetailModel();
            int? recOreder = 0;
            JVVDetailModel journal = new JVVDetailModel();
            //if (isNew)
            //    journal.Id = Guid.NewGuid();
            //else
            journal.Id = Guid.NewGuid();
            FillJvDetals(clearing, journal);
            journal.RecOrder = recOreder + 1;
            recOreder = journal.RecOrder;
            lstJD.Add(journal);
            FillJvHeadDetail(clearing, Jmodel);
            Jmodel.RecOrder = recOreder + 1;
            recOreder = Jmodel.RecOrder;
            lstJD.Add(Jmodel);
            //if (clearing.IsGstSettings)
            //{
            //    ChartOfAccount gstAccount = _chartOfAccountService.GetByName(COANameConstants.TaxPayableGST, invoice.CompanyId);
            //    //_chartOfAccountRepository.Query(c => c.Name == COANameConstants.TaxPayableGST).Select().FirstOrDefault();
            //    foreach (GLClearingDetail detail in clearing.GLClearingDetails)
            //    {
            //        JVVDetailModel journal = new JVVDetailModel();
            //        if (isNew)
            //            journal.Id = Guid.NewGuid();
            //        else
            //            journal.Id = detail.Id;
            //       // FillJvGstDetail(invoice, journal, detail, gstAccount, Jmodel);
            //        journal.RecOrder = recOreder + 1;
            //        recOreder = journal.RecOrder;
            //        lstJD.Add(journal);
            //    }
            //}
            headJournal.JVVDetailModels = lstJD;
        }
        private void FillJv(JVModel headJournal, GLClearing clearing)
        {
            headJournal.DocumentId = clearing.Id;
            headJournal.CompanyId = clearing.CompanyId;
            headJournal.DocNo = clearing.DocNo;
            headJournal.DocType = "Clearing";
            //headJournal.DocDate = clearing.DocDate;
            headJournal.ClearingDate = clearing.DocDate;
            headJournal.ClearingStatus = ClearingState.Cleared;
            headJournal.DocumentState = clearing.DocumentState;
            headJournal.AllocatedAmount = clearing.CheckAmount;
            headJournal.SystemReferenceNo = clearing.SystemRefNo;
            headJournal.ServiceCompanyId = clearing.ServiceCompanyId;
            headJournal.IsMultiCurrency = clearing.IsMultiCurrency;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            if (clearing.EntityId != null)
                headJournal.EntityId = clearing.EntityId.Value;

            ChartOfAccount account = _chartOfAccountService.GetChartOfAccountById(clearing.COAId);
            if (account != null)
            {
                headJournal.COAId = account.Id;
                headJournal.AccountCode = account.Code;
                headJournal.AccountName = account.Name;
            }
            //headJournal.GrandBaseDebitTotal = Math.Round((decimal)(clearing.GrandTotal * clearing.ExchangeRate), 2);

            headJournal.Remarks = clearing.DocDescription;
            headJournal.UserCreated = clearing.UserCreated;
            headJournal.CreatedDate = clearing.CreatedDate;
            headJournal.ModifiedBy = clearing.ModifiedBy;
            headJournal.ModifiedDate = clearing.ModifiedDate;
            headJournal.DueDate = null;
            headJournal.BaseCurrency = clearing.ExCurrency;
            headJournal.GrandDocDebitTotal = clearing.CheckAmount.Value;
            headJournal.GrandBaseDebitTotal = clearing.CheckAmount;
        }
        private void FillJvHeadDetail(GLClearing clearing, JVVDetailModel Jmodel)
        {
            clearing.CheckAmount = -(clearing.CheckAmount);
            Jmodel.Id = Guid.NewGuid();
            Jmodel.DocumentId = clearing.Id;
            Jmodel.DocNo = clearing.DocNo;
            Jmodel.DocType = DocTypeConstants.GLClearing;
            Jmodel.DocDate = clearing.DocDate;
            // Jmodel.DocDate = clearing.DocumentDate;
            Jmodel.SystemRefNo = clearing.SystemRefNo;
            Jmodel.ServiceCompanyId = clearing.ServiceCompanyId;
            Jmodel.ClearingDate = clearing.DocDate;
            Jmodel.ClearingStatus = ClearingState.Cleared;
            Jmodel.EntityId = clearing.EntityId;
            Jmodel.DueDate = null;
            if (clearing.COAId != null)
            {
                ChartOfAccount account1 = _chartOfAccountService.GetChartOfAccountById(clearing.COAId);
                Jmodel.COAId = account1.Id;
                Jmodel.AccountName = account1.Name;
            }
            if (clearing.CheckAmount > 0)
            {
                Jmodel.DocDebit = clearing.CheckAmount;
                if (clearing.IsMultiCurrency)
                    Jmodel.BaseDebit = clearing.CheckAmount;
            }
            else
            {
                Jmodel.DocCredit = clearing.CheckAmount;
                if (clearing.IsMultiCurrency)
                    Jmodel.BaseCredit = clearing.CheckAmount;
            }
        }
        private void FillJvDetals(GLClearing clearing, JVVDetailModel journal)
        {
            ChartOfAccount account;
            long? coaId = clearing.COAId2;
            journal.DocumentDetailId = clearing.Id;
            journal.DocumentId = clearing.Id;
            //            journal.DocDate = clearing.DocumentDate;
            journal.ServiceCompanyId = clearing.ServiceCompanyId;
            journal.DocNo = clearing.DocNo;
            journal.DocType = DocTypeConstants.GLClearing;
            journal.EntityId = clearing.EntityId;
            if (coaId != null && coaId > 0)
            {
                account = _chartOfAccountService.GetChartOfAccountById(coaId.Value);
                journal.COAId = account.Id;
                journal.AccountCode = account.Code;
                journal.AccountName = account.Name;
            }
            journal.DocDate = clearing.DocDate;
            journal.ClearingDate = clearing.DocDate;
            journal.ClearingStatus = ClearingState.Cleared;
            journal.DocCurrency = clearing.ExCurrency;
            journal.BaseCurrency = clearing.ExCurrency;
            journal.DueDate = null;
            if (clearing.CheckAmount > 0)
            {
                journal.DocDebit = Math.Abs(clearing.CheckAmount.Value);
                if (clearing.IsMultiCurrency)
                    journal.BaseDebit = Math.Abs(clearing.CheckAmount.Value);
            }
            else
            {
                journal.DocCredit = Math.Abs(clearing.CheckAmount.Value);
                if (clearing.IsMultiCurrency)
                    journal.BaseCredit = Math.Abs(clearing.CheckAmount.Value);
            }
            //journal.BaseDebit = Math.Round((decimal)journal.ExchangeRate == null ? journal.DocDebit : (journal.DocDebit * journal.ExchangeRate).Value, 2);

            //journal.DocTaxableAmount = detail.DocAmount;
            //journal.DocTaxAmount = detail.DocTaxAmount;
            //journal.BaseTaxableAmount = detail.BaseAmount;
            //journal.BaseTaxAmount = detail.BaseTaxAmount;

            //if (clearing.GSTExchangeRate != null)
            //{
            //    journal.GSTTaxableAmount = Math.Round((decimal)journal.DocTaxableAmount * clearing.GSTExchangeRate.Value, 2);
            //    journal.GSTTaxAmount = Math.Round((decimal)journal.DocTaxAmount == 0 ? 0 : journal.DocTaxAmount.Value * clearing.GSTExchangeRate.Value, 2);
            //}
            //journal.DocCreditTotal = detail.DocTotalAmount;
        }
        #endregion
    }
}
