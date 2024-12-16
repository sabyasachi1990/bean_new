using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.RevaluationModule.Entities.Models;
using AppsWorld.RevaluationModule.Models;
using AppsWorld.RevaluationModule.RepositoryPattern;
using AppsWorld.RevaluationModule.Service;
using AppsWorld.RevaluationModule.Infra;
using FrameWork;
using Domain.Events;
using Repository.Pattern.Infrastructure;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Infra;
using Serilog;
using Logger;
using System.Configuration;
using AppsWorld.CommonModule.Application;
using Ziraff.Section;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using AppaWorld.Bean;

namespace AppsWorld.RevaluationModule.Application
{
    public class RevaluationApplicationService
    {
        private readonly IRevaluationService _revaluationService;
        private readonly AppsWorld.RevaluationModule.Service.IJournalService _journalService;
        private readonly IJournalDetailService _journalDetailService;
        private readonly AppsWorld.RevaluationModule.Service.IMultiCurrencySettingService _multiCurrencyService;
        private readonly AppsWorld.RevaluationModule.Service.IChartOfAccountService _chartOfAccountService;
        private readonly AppsWorld.RevaluationModule.Service.IForexService _forexService;
        private readonly IRevaluationDetailService _revaluationDetailService;
        private readonly IRevaluationModuleUnitOfWorkAsync _unitOfWorkAsync;
        private readonly AppsWorld.RevaluationModule.Service.IBeanEntityService _beanService;
        private readonly AppsWorld.RevaluationModule.Service.IGSTSettingService _gstSettingService;
        private readonly AppsWorld.CommonModule.Service.ICompanySettingService _companySettingService;
        private readonly AppsWorld.CommonModule.Service.IFinancialSettingService _finalcialSettingService;
        private readonly IInvoiceService _invoiceService;
        private readonly IDebitNoteService _debitNoteService;
        private readonly IBillService _billService;
        private readonly ICompanyService _companyService;
        private readonly AppsWorld.RevaluationModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        private readonly AppsWorld.RevaluationModule.Service.IAutoNumberService _autoNumberService;
        private readonly IAccountTypeService _accountTypeService;
        string doc = "";
		SqlConnection con = null;
		SqlCommand cmd = null;
		string query = string.Empty;

		public RevaluationApplicationService(IRevaluationService revaluationService, AppsWorld.RevaluationModule.Service.IJournalService journalService, AppsWorld.RevaluationModule.Service.IMultiCurrencySettingService multiCurrencyService, IJournalDetailService journalDetailService, AppsWorld.RevaluationModule.Service.IChartOfAccountService chartOfAccountService, AppsWorld.RevaluationModule.Service.IForexService forexService, IRevaluationDetailService revaluationDetailService, IRevaluationModuleUnitOfWorkAsync unitOfWorkAsync, AppsWorld.RevaluationModule.Service.IBeanEntityService beanService, AppsWorld.RevaluationModule.Service.IGSTSettingService gstSettingService, AppsWorld.CommonModule.Service.ICompanySettingService companySettingService, IInvoiceService invoiceService, IDebitNoteService debitNoteService, IBillService billService, ICompanyService companyService, AppsWorld.RevaluationModule.Service.IAutoNumberCompanyService autoNumberCompanyService, AppsWorld.RevaluationModule.Service.IAutoNumberService autoNumberService, AppsWorld.CommonModule.Service.IFinancialSettingService finalcialSettingService, IAccountTypeService accountTypeService)
        {
            this._revaluationService = revaluationService;
            this._journalService = journalService;
            this._multiCurrencyService = multiCurrencyService;
            this._journalDetailService = journalDetailService;
            this._chartOfAccountService = chartOfAccountService;
            _forexService = forexService;
            _revaluationDetailService = revaluationDetailService;
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._beanService = beanService;
            this._gstSettingService = gstSettingService;
            this._companySettingService = companySettingService;
            this._invoiceService = invoiceService;
            this._debitNoteService = debitNoteService;
            this._billService = billService;
            this._companyService = companyService;
            this._autoNumberService = autoNumberService;
            this._autoNumberCompanyService = autoNumberCompanyService;
            this._finalcialSettingService = finalcialSettingService;
            this._accountTypeService = accountTypeService;

        }

        #region Create Revaluation
        public List<RevaluationModel> CreateRevaluation(DateTime dateTime, long companyId, Guid id, long serviceCompanyId)
        {
            List<RevaluationModel> lstRevaluationModel = new List<RevaluationModel>();
            Revaluation revaluation = _revaluationService.GetAllRevaluationAndDetail(id, companyId);
            MultiCurrencySetting multiCurrency = _multiCurrencyService.GetMultiCurrency(companyId);
            if (multiCurrency == null)
                throw new Exception(RevaluationConstant.Multi_Currency_should_be_activate);
            if (revaluation == null)
            {
                //List<Journal> lstJournalAll = new List<Journal>();
                List<Journal> lstJournals = _journalService.GetPostingJournal(dateTime, companyId, serviceCompanyId);
                //if (lstJournals.Any())
                //{
                //    lstJournals = (from j in lstJournals
                //                   join coa in _chartOfAccountService.Queryable()
                //                   on j.COAId equals coa.Id
                //                   where  j.DocCurrency != multiCurrency.BaseCurrency && coa.IsBank == false
                //                   select j).ToList();
                //    lstJournalAll.AddRange(lstJournals);
                //    lstJournals= (from j in lstJournals
                //                  join coa in _chartOfAccountService.Queryable()
                //                  on j.COAId equals coa.Id
                //                  where   j.DocCurrency != multiCurrency.BaseCurrency && coa.IsBank == true
                //                  select j).ToList();
                //}


                var lstDetail = (from j in lstJournals
                                 join jd in _journalDetailService.Queryable()
                                 on j.Id equals jd.JournalId
                                 where j.ServiceCompanyId == serviceCompanyId
                                 select jd)
                                 .ToList();
                var lstjdDetail = _journalDetailService.Queryable().Where(c => c.ServiceCompanyId == serviceCompanyId && c.DocDate <= dateTime).ToList();
                //var lstJDetail = lstDetail.Where(x => (x.IsTax != true) || x.ClearingDate != null || x.BankClearingDate != null).ToList();

                List<JournalDetail> lstJDetailFinal = lstDetail.Where(x => x.DocCurrency != x.BaseCurrency && (x.ClearingStatus != "Cleared" || x.ClearingDate == null) && x.ServiceCompanyId == serviceCompanyId && x.IsTax != true/*&&(x.DocType=="Withdrawal"||x.DocType=="Cash Payment"||x.DocType=="Deposit")*/).ToList();
                List<JournalDetail> lstJournalDetail = (from j in lstJDetailFinal
                                                        join coa in _chartOfAccountService.Queryable()
                                                        on j.COAId equals coa.Id
                                                        where j.COAId == coa.Id && coa.CompanyId == companyId && coa.Revaluation == true /*&& coa.IsBank != true*/ /*&& j.DocumentDetailId != new Guid()*/
                                                        select j).ToList();

                if (lstJournalDetail.Any())
                {
                    List<ChartOfAccount> lstCOAs = _chartOfAccountService.GetAllChartOfAccountByCIdAndId(lstJournalDetail.Select(c => c.COAId).ToList(), companyId);
                    List<BeanEntity> lstEntiy = new List<BeanEntity>();
                    if (lstJournalDetail.Select(c => c.EntityId).Any())
                        lstEntiy = _beanService.GetAllEntityById(lstJournalDetail.Select(c => c.EntityId.Value).ToList());
                    foreach (var journal in lstJournalDetail)
                    {

                        //if (journal.COAId != null)
                        //{
                        //    var coaDetail =
                        //        _chartOfAccountService.GetChartOfAccountRevaluation(journal.COAId, companyId);
                        //    if (coaDetail == true)
                        //    {

                        RevaluationModel revaluationModule = new RevaluationModel();
                        FillRevaluationDetail(revaluationModule, journal, companyId, dateTime, lstCOAs, lstEntiy);

                        //revaluationModule.DueDate = lstJournals.Where(a => a.Id == journal.JournalId).Select(a => a.DueDate).FirstOrDefault();
                        //revaluationModule.IsGSTSetting = IsGSTSetting;
                        revaluationModule.ServiceEntityName = _companyService.GetIdBy(serviceCompanyId);
                        revaluationModule.ServiceCompanyId = serviceCompanyId;
                        //revaluationModule.IsSegmentReporting = IsSegmentReporting;
                        //if (jdDetail.Any())
                        //{
                        //    decimal? diff = 0;
                        //    decimal? baseDebit = jdDetail.Sum(c => Math.Abs((decimal)c.BaseDebit));
                        //    decimal? baseCredit = jdDetail.Sum(c => Math.Abs((decimal)c.BaseCredit));
                        //    if (baseDebit != revaluationModule.BaseCurrencyAmount1)
                        //    {
                        //        if (baseDebit > revaluationModule.BaseCurrencyAmount1)
                        //        {
                        //            diff = baseDebit - revaluationModule.BaseCurrencyAmount1;
                        //            // journal.BaseCredit = diff;
                        //            revaluationModule.BaseCurrencyAmount1 = revaluationModule.BaseCurrencyAmount1 + diff;
                        //            revaluationModule.UnrealisedExchangegainorlose = Math.Round((decimal)(revaluationModule.BaseCurrencyAmount1 - revaluationModule.BaseCurrencyAmount2), 2);
                        //        }
                        //        if (baseDebit < revaluationModule.BaseCurrencyAmount1)
                        //        {
                        //            diff = revaluationModule.BaseCurrencyAmount1 - baseDebit;
                        //            // journal.BaseCredit = diff;
                        //            revaluationModule.BaseCurrencyAmount1 = revaluationModule.BaseCurrencyAmount1 + diff;
                        //            revaluationModule.UnrealisedExchangegainorlose = Math.Round((decimal)(revaluationModule.BaseCurrencyAmount1 - revaluationModule.BaseCurrencyAmount2), 2);
                        //        }
                        //    }
                        //    if (baseCredit != revaluationModule.BaseCurrencyAmount1)
                        //    {
                        //        if (baseCredit > revaluationModule.BaseCurrencyAmount1)
                        //        {
                        //            diff = baseCredit - revaluationModule.BaseCurrencyAmount1;
                        //            // journal.BaseCredit = diff;
                        //            revaluationModule.BaseCurrencyAmount1 = revaluationModule.BaseCurrencyAmount1 + diff;
                        //            revaluationModule.UnrealisedExchangegainorlose = Math.Round((decimal)(revaluationModule.BaseCurrencyAmount1 - revaluationModule.BaseCurrencyAmount2), 2);
                        //        }
                        //        if (baseCredit < revaluationModule.BaseCurrencyAmount1)
                        //        {
                        //            diff = revaluationModule.BaseCurrencyAmount1 - baseCredit;
                        //            // journal.BaseCredit = diff;
                        //            revaluationModule.BaseCurrencyAmount1 = revaluationModule.BaseCurrencyAmount1 + diff;
                        //            revaluationModule.UnrealisedExchangegainorlose = Math.Round((decimal)(revaluationModule.BaseCurrencyAmount1 - revaluationModule.BaseCurrencyAmount2), 2);
                        //        }
                        //    }
                        //}
                        lstRevaluationModel.Add(revaluationModule);
                        //    }
                        //}
                    }
                }
            }
            else
            {
                // List<JVViewModel> lstViewModel = new List<JVViewModel>();
                //var details = revaluation.RevalutionDetails;
                //if (details.Count > 0)
                //{
                //    Journal journal = _journalService.GetJournal(companyId, id);
                //    if (journal != null)
                //    {
                //        foreach (var journalList in lstRevaluationModel)
                //        {
                //            journalList.JournalId = journal.Id;
                //            journalList.JounalSystemreferenceNo = journal.SystemReferenceNo;
                //        }
                //    }
                //var lstLournal = _journalService.GetAllJournal(companyId, id);
                //if (lstLournal.Any())
                //{
                //    foreach (var data in lstLournal)
                //    {
                //        JVViewModel view = new JVViewModel();
                //        view.Id = data.Id;
                //        view.SystemReferenceNo = data.SystemReferenceNo;
                //        view.DocType = data.DocSubType;
                //        lstViewModel.Add(view);
                //    }
                //}
                foreach (var detail in revaluation.RevalutionDetails)
                {
                    RevaluationModel rModel = new RevaluationModel();
                    RevaluationDetailEntityToModel(companyId, rModel, detail);
                    //rModel.CreatedDate = revaluation.CreatedDate;
                    //rModel.UserCreated = revaluation.UserCreated;
                    rModel.ServiceCompanyId = serviceCompanyId;
                    lstRevaluationModel.Add(rModel);
                }
            }
            return lstRevaluationModel.Where(a => a.DocBal != 0).ToList();
        }

        private void RevaluationDetailEntityToModel(long companyId, RevaluationModel rModel, RevalutionDetail detail)
        {
            rModel.COAId = detail.COAId.Value;
            //var coaName = _chartOfAccountService.GetChartOfAccountName(rModel.COAId, companyId);
            //if (coaName != null)
            //{
            //    rModel.AccountName = coaName.Name;
            //    rModel.AccountCode = coaName.Code;
            //}
            rModel.EntityId = detail.EntityId;
            //if (detail.EntityId != null)
            //{
            //    var entityDetail = _beanService.GetEntityById(detail.EntityId.Value);
            //    if (entityDetail != null)
            //    {
            //        rModel.EntityName = entityDetail.Name;
            //        rModel.EntityType = "Customer";
            //    }
            //}
            //rModel.BaseCurrency = detail.BaseCurrency;
            //rModel.DocumentId = detail.DocId;
            rModel.DocCurrency = detail.DocCurrency;
            rModel.DocDate = detail.DocumentDate;
            //rModel.DocumentDescription = detail.DocumentDescription;
            rModel.DocNo = detail.DocumentNumber;
            //rModel.DocumentSubType = detail.DocumentSubType;
            //rModel.DocumentType = detail.DocumentType;
            rModel.OrgExchangeRate = detail.ExchangerateOld;
            //rModel.PostingDate = detail.PostingDate;
            //rModel.SegmentCategory1 = detail.SegmentCategory1;
            //rModel.SegmentCategory2 = detail.SegmentCategory2;
            //rModel.SystemReferenceNumber = detail.SystemReferenceNumber;
            //rModel.DocCurrencyAmount = detail.DocCurrencyAmount;
            //rModel.BaseCurrencyAmount1 = detail.BaseCurrencyAmount1;
            //rModel.ExchangerateNew = detail.ExchangerateNew;
            //rModel.BaseCurrencyAmount2 = detail.BaseCurrencyAmount2;
            //rModel.Status = detail.Status;
            rModel.DocBal = detail.DocBal;
            rModel.UnrealisedExchangegainorlose = detail.UnrealisedExchangegainorlose;
        }

        public RevaluationSaveModel CreateRevaluationModel(long companyId)
        {
            RevaluationSaveModel revaluationSaveModel = new RevaluationSaveModel();
            revaluationSaveModel.RevaluationDate = DateTime.UtcNow;
            List<RevaluationModel> lstRevaluationModel = new List<RevaluationModel>();
            lstRevaluationModel.Add(new RevaluationModel());
            revaluationSaveModel.RevaluationModels = lstRevaluationModel;
            return revaluationSaveModel;
        }
        public RevaluationFinancialModel FinancialModel(long companyId)
        {
            RevaluationFinancialModel model = new RevaluationFinancialModel();
            model.FinancialPeriodLockStartDate = _finalcialSettingService.GetFinancialOpenPeriodStarDate(companyId);
            model.FinancialPeriodLockEndDate = _finalcialSettingService.GetFinancialOpenPeriodEndDate(companyId);
            //model.FinancialStartDate = _finalcialSettingService.GetFinancialYearEndLockDate(companyId);
            //model.FinancialEndDate = _finalcialSettingService.GetFinancialYearEndLockDate(companyId);
            return model;
        }
        #endregion

        #region Revaluation Lookup
        public RevaluationLUs RevaluationLUs(long companyId, Guid revaluationId, string userName)
        {
            Revaluation revaluation = _revaluationService.GetAllRevaluationById(revaluationId, companyId);
            AppsWorld.CommonModule.Entities.FinancialSetting financialSettings = _finalcialSettingService.GetFinancialSetting(companyId);
            if (financialSettings == null)
            {
                throw new Exception(RevaluationConstant.The_Financial_setting_should_be_activated);
            }
            RevaluationLUs revaluationLus = new Models.RevaluationLUs();
            revaluationLus.FinancialPeriodLockStartDate = financialSettings.PeriodLockDate;
            revaluationLus.FinancialPeriodLockEndDate = financialSettings.PeriodEndDate;
            revaluationLus.IsRevaluation = _multiCurrencyService.GetMultiCurrencyByCompanyId(companyId);
            //revaluationLus.SubsideryCompanyLU = _companyService.Listofsubsudarycompany(userName, companyId, 0);
            //revaluationLus.FinancialPeriodLockStartDate = _finalcialSettingService.GetFinancialOpenPeriodStarDate(companyId);
            //revaluationLus.FinancialPeriodLockEndDate = _finalcialSettingService.GetFinancialOpenPeriodEndDate(companyId);
            //revaluationLus.FinancialStartDate = _finalcialSettingService.GetFinancialYearEndLockDate(companyId);
            //revaluationLus.FinancialEndDate = _finalcialSettingService.GetFinancialYearEndLockDate(companyId);

            List<JVViewModel> lstViewModel = new List<JVViewModel>();
            var lstLournal = _journalService.GetAllJournal(companyId, revaluationId);
            if (lstLournal.Any())
            {
                foreach (var data in lstLournal)
                {
                    JVViewModel view = new JVViewModel();
                    view.Id = data.Id;
                    view.SystemReferenceNo = data.SystemReferenceNo;
                    view.DocType = data.DocSubType;
                    view.DocState = data.DocumentState;
                    lstViewModel.Add(view);
                }
            }
            //revaluationLus.JVViewModels = lstViewModel.OrderBy(c => c.SystemReferenceNo).ToList();
            if (revaluation != null)
            {
                revaluationLus.CreatedDate = revaluation.CreatedDate;
                revaluationLus.UserCreated = revaluation.UserCreated;
                revaluationLus.IsMultiCurrency = revaluation.IsMultiCurrency;
                //revaluationLus.IsSegmentReporting = revaluation.IsSegmentReporting;
                revaluationLus.DocState = revaluation.DocState;
            }
            return revaluationLus;
        }
        #endregion

        #region Private Methods
        private void FillRevaluationDetail(RevaluationModel rModel, JournalDetail journal, long companyId, DateTime dateTime, List<ChartOfAccount> lstCOas, List<BeanEntity> lstEntites)
        {
            decimal? amntDue = 0;
            if (journal.DocumentDetailId == new Guid())
            {
                JournalDetail jdDetail = _journalDetailService.GetAllJournalDetails(journal.JournalId);
                amntDue = jdDetail.AmountDue;
            }
            rModel.COAId = journal.COAId;
            //ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountName(rModel.COAId, companyId);
            ChartOfAccount coa = lstCOas.Where(c => c.Id == rModel.COAId).FirstOrDefault();

            //if (coa != null)
            //{
            //    rModel.AccountName = coa.Name;
            //    rModel.AccountCode = coa.Code;
            //}
            rModel.EntityId = journal.EntityId;

            //BeanEntity entityDetail = _beanService.GetEntityById(journal.EntityId.Value);
            //BeanEntity entityDetail = lstEntites.Where(c => c.Id == journal.EntityId).FirstOrDefault();
            //if (entityDetail != null)
            //{
            //    rModel.EntityName = entityDetail.Name;
            //    rModel.EntityType = entityDetail.IsCustomer == true ? "Customer" : "Vendor";
            //}
            //rModel.BaseCurrency = journal.BaseCurrency;
            //rModel.DocumentId = journal.DocumentId;
            rModel.DocCurrency = journal.DocCurrency;
            //rModel.DocCurrencyAmount = journal.doc
            rModel.DocDate = journal.DocDate;
            //rModel.DocumentDescription = journal.AccountDescription != null ? journal.AccountDescription : journal.ItemDescription != null ? journal.ItemDescription : journal.DocDescription != null ? journal.DocDescription : journal.Remarks;
            rModel.DocNo = journal.DocNo;
            //rModel.DocumentSubType = (journal.DocType == DocTypeConstants.JournalVocher || journal.DocSubType == "Application" || journal.DocSubType == "CM Application") ? journal.DocSubType : string.Empty;
            //rModel.DocumentType = journal.DocType;
            //rModel.PostingDate = journal.PostingDate;
            //Journal journal1 = _journalService.GetJournalById(companyId, journal.JournalId);
            //if (journal1 != null)
            //{
            //rModel.SegmentCategory1 = journal.SegmentCategory1;
            //rModel.SegmentCategory2 = journal.SegmentCategory2;
            //  }
            //rModel.SystemReferenceNumber = journal.SystemRefNo;
            //revaluationModule.UnrealisedExchangegainorlose = journal.UnrealisedExchangegainorlose;
            //rModel.DocCurrencyAmount = lstJournalDetails.Sum(c => c.DocCredit);
            //decimal? docCredit = -Math.Abs(journal.DocCredit);
            //decimal? docDebit = journal.DocDebit;
            //if (journal.DocDebit > 0)
            //    rModel.DocCurrencyAmount = docDebit;
            //else
            //    rModel.DocCurrencyAmount = docCredit;

            //if ((coa.Name == COANameConstants.AccountsPayable || coa.Name == COANameConstants.OtherPayables || coa.Name == COANameConstants.AccountsReceivables || coa.Name == COANameConstants.OtherReceivables) && (journal.DocType == DocTypeConstants.Invoice || journal.DocType == DocTypeConstants.DebitNote || journal.DocType == DocTypeConstants.CreditNote))
            //{
            //    rModel.DocCurrencyAmount = (journal.DocDebit != null && journal.DocDebit != 0) ? journal.DocDebit : -(journal.DocCredit);
            //}
            //else if ((coa.Name == COANameConstants.AccountsPayable || coa.Name == COANameConstants.OtherPayables || coa.Name == COANameConstants.AccountsReceivables || coa.Name == COANameConstants.OtherReceivables) && (journal.DocType == DocTypeConstants.Bills || journal.DocType == DocTypeConstants.PayrollBill || journal.DocType == DocTypeConstants.BillCreditMemo))
            //{
            //    rModel.DocCurrencyAmount = (journal.DocCredit != null && journal.DocCredit != 0) ? journal.DocCredit : -(journal.DocDebit);
            //}
            //else
            //    rModel.DocCurrencyAmount = (journal.DocCredit == null || journal.DocCredit == 0) ? journal.DocDebit : journal.DocCredit;

            //if (coa != null)
            //    rModel.DocBal = (coa.Name == COANameConstants.AccountsPayable || coa.Name == COANameConstants.OtherPayables || coa.Name == COANameConstants.AccountsReceivables || coa.Name == COANameConstants.OtherReceivables) ? ((amntDue == null || amntDue == 0) ? rModel.DocCurrencyAmount : amntDue) /* rModel.DocCurrencyAmount*/  : rModel.DocCurrencyAmount;
            if ((journal.DocType == DocTypeConstants.CreditNote || journal.DocType == DocTypeConstants.BillCreditMemo) && journal.DocumentDetailId == new Guid())
                rModel.DocBal = -rModel.DocBal;
            rModel.OrgExchangeRate = journal.ExchangeRate == null ? 0 : journal.ExchangeRate;
            //if (rModel.BaseCurrency == rModel.DocCurrency)
            //{
            //    rModel.ExchangerateNew = 1;
            //}
            //rModel.BaseCurrencyAmount1 = Math.Round((decimal)((rModel.DocBal != null ? rModel.DocBal : 0) * rModel.ExchangerateOld), 2);
            //if (rModel.ExchangerateNew != 1)
            //{
            //    Forex forexBean = _forexService.GetMultiCurrencyInformation(journal.DocCurrency, dateTime, true, companyId);
            //    if (forexBean != null)
            //    {
            //        rModel.ExchangerateNew = rModel.BaseCurrency == rModel.DocCurrency ? 1 : Math.Round((decimal)forexBean.UnitPerCal, 10);
            //        rModel.BaseCurrencyAmount2 = Math.Round((decimal)((rModel.DocBal != null ? rModel.DocBal : 0) * rModel.ExchangerateNew), 2);
            //        decimal? gainOrLoss = Math.Round((decimal)(rModel.BaseCurrencyAmount1 - rModel.BaseCurrencyAmount2), 2);
            //        //if (gainOrLoss < 0)
            //        //{
            //        //    rModel.UnrealisedExchangegainorlose = "(" + gainOrLoss + ")";
            //        //    rModel.UnrealisedExchangegainorlose = rModel.UnrealisedExchangegainorlose.Remove(1, 1);
            //        //}
            //        //else
            //        //{
            //        //    rModel.UnrealisedExchangegainorlose = gainOrLoss.ToString();
            //        //}
            //        rModel.UnrealisedExchangegainorlose = gainOrLoss;
            //    }
            //}
        }
        private void UpdateRevaluation(Revaluation revaluation, RevaluationSaveModel revaluationModel)
        {
            revaluation.CompanyId = revaluationModel.CompanyId;
            revaluation.RevalutionDate = revaluationModel.RevaluationDate;
            revaluation.DocState = revaluationModel.DocState;
            revaluation.ServiceCompanyId = revaluationModel.ServiceCompanyId;
            revaluation.IsMultiCurrency = revaluationModel.IsMultiCurrency;
            revaluation.IsNoSupportingDocument = revaluationModel.IsNoSupportingDocument;
        }

        private void RevaluationDetailModelToEntity(RevalutionDetail revaluation, RevaluationModel revaluationDetail)
        {
            revaluation.Id = Guid.NewGuid();
            revaluation.COAId = revaluationDetail.COAId;
            revaluation.CreatedDate = DateTime.UtcNow;
            revaluation.DocCurrency = revaluationDetail.DocCurrency;
            revaluation.DocumentDate = revaluationDetail.DocDate;
            revaluation.DocumentNumber = revaluationDetail.DocNo;
            revaluation.EntityId = revaluationDetail.EntityId;
            revaluation.ExchangerateNew = revaluationDetail.RevalExchangeRate;
            revaluation.ExchangerateOld = revaluationDetail.OrgExchangeRate;
            revaluation.UnrealisedExchangegainorlose = revaluationDetail.UnrealisedExchangegainorlose;
            revaluation.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            revaluation.DocBal = revaluationDetail.DocBal;
        }

        #endregion

        #region Save Revaluation
        public Revaluation SaveRevaluation(RevaluationSaveModel Robject, string ConnectionString)
        {
			bool isAdd = false;
			bool isDocAdd = false;
			try
            {
				var AdditionalInfo = new Dictionary<string, object>();
				AdditionalInfo.Add("Data", JsonConvert.SerializeObject(Robject));
				Ziraff.FrameWork.Logging.LoggingHelper.LogMessage(RevaluationLoggingValidation.RevaluationApplicationService, "ObjectSave", AdditionalInfo);
				if (Robject.RevaluationModels == null)
				{
					throw new Exception(RevaluationValidation.Revaluation_records_are_not_found);
				}
				var lst = Robject.RevaluationModels.Where(c => c.RevalExchangeRate == null).ToList();
				if (lst != null && lst.Any())
				{
					throw new Exception(RevaluationValidation.Exchange_rates_are_not_found);
				}
				List<Revaluation> lstChechBeforeRevaluation = _revaluationService.GetAllPostedRevaluation(Robject.RevaluationDate, Robject.ServiceCompanyId);
				if (lstChechBeforeRevaluation.Any())
					throw new Exception(RevaluationValidation.Revaluation_can_run_once_per_same_date);
				Revaluation revaluation = _revaluationService.GetAllRevaluationById(Robject.Id, Robject.CompanyId);
				if (revaluation != null)
					revaluation.RevalutionDetails = _revaluationDetailService.GetDetails(Robject.Id);
				List<RevalutionDetail> lstRevaluation = new List<RevalutionDetail>();
				if (revaluation != null)
				{
					UpdateRevaluation(revaluation, Robject);
					revaluation.Status = Robject.Status;
					revaluation.DocState = "Posted";
					revaluation.ModifiedBy = Robject.ModifiedBy;
					revaluation.ModifiedDate = DateTime.UtcNow;
					revaluation.ObjectState = ObjectState.Modified;
					_revaluationService.Update(revaluation);
				}
				else
				{
                    isAdd = true;
					revaluation = new Revaluation();
					UpdateRevaluation(revaluation, Robject);
					revaluation.Id = Guid.NewGuid();
					revaluation.UserCreated = Robject.UserCreated;
					revaluation.DocState = "Posted";
					revaluation.IsMultiCurrency = Robject.IsMultiCurrency;
					revaluation.IsNoSupportingDocument = Robject.IsNoSupportingDocument;
					revaluation.Status = AppsWorld.Framework.RecordStatusEnum.Active;
					revaluation.CreatedDate = Robject.CreatedDate;
					AppsWorld.CommonModule.Entities.Company company = _companyService.GetById(Robject.CompanyId);
					revaluation.SystemRefNo = GenerateAutoNumberForType(company.Id, DocTypeConstants.Revaluation, company.ShortName);
                    isDocAdd = true;
					revaluation.ObjectState = ObjectState.Added;
					_revaluationService.Insert(revaluation);
					var revaluationDetails = Robject.RevaluationModels;
					if (revaluationDetails.Any())
					{
						foreach (var revaluationDetail in revaluationDetails)
						{
							RevalutionDetail rdetail = new RevalutionDetail();
							RevaluationDetailModelToEntity(rdetail, revaluationDetail);
							rdetail.RevalutionId = revaluation.Id;
							rdetail.ObjectState = ObjectState.Added;
							lstRevaluation.Add(rdetail);
							_revaluationDetailService.Insert(rdetail);
						}
					}
					revaluation.RevalutionDetails = lstRevaluation;
					JVModel jvm = null;
					FillRevaluationPosting(jvm, revaluation, true, false);
				}
				try
				{
					_unitOfWorkAsync.SaveChanges();
				}
				catch (Exception ex)
				{
					throw ex;
				}
				return revaluation;
			}
            catch (Exception ex)
            {
				if (isAdd && isDocAdd)
				{
                    Common.SaveDocNoSequence(Robject.CompanyId, DocTypeConstants.Revaluations, ConnectionString);
				}
				throw ex;
            }
        }


        public Revaluation SaveCancelRevaluation(RevaluationCancelModel RevCancelModel)
        {
            Revaluation revaluation = _revaluationService.GetAllRevaluationById(RevCancelModel.Id, RevCancelModel.CompanyId);
            if (revaluation == null)
            {
                throw new Exception("Invalid Revaluation");
            }
            else
            {
                if (revaluation.DocState == "Cancelled")
                    throw new Exception("State is already canceled");
                revaluation.DocState = "Cancelled";
                revaluation.ObjectState = ObjectState.Modified;
            }
            RVModel rvm = new RVModel();
            rvm.Id = revaluation.Id;
            rvm.CompanyId = revaluation.CompanyId;
            //FillRevaluationPosting(rvm, revaluation, true);
            SaveReversalRevaluation(rvm);
            //List<Journal> lstJournal = _journalService.GetAllJournal(RevCancelModel.CompanyId, RevCancelModel.Id);
            //if (lstJournal.Any())
            //{
            //    try
            //    {
            //        bool? isNew = false;
            //        bool? isCancel = false;
            //        FillCancelRMethod(lstJournal, isCancel, isNew);
            //        isCancel = true;
            //        isNew = true;
            //        FillCancelRMethod(lstJournal, isCancel, isNew);
            //    }
            //    catch
            //    {

            //    }
            //}
            _unitOfWorkAsync.SaveChanges();
            return revaluation;
        }

        //private void FillCancelRMethod(List<Journal> lstJournal, bool? isCancel, bool? isFirst)
        //{

        //    List<JVModel> lstJournalModel = new List<JVModel>();
        //    foreach (var journal in lstJournal)
        //    {
        //        JVModel jvmodel = new JVModel();
        //        FillCancelJournal(jvmodel, journal, isCancel, isFirst);
        //        List<JVVDetailModel> lstDetails = new List<JVVDetailModel>();
        //        foreach (var detail in journal.JournalDetails)
        //        {
        //            JVVDetailModel rDetail = new JVVDetailModel();
        //            FillJVCancelRevaluationDetail(rDetail, detail, jvmodel, isCancel);
        //            lstDetails.Add(rDetail);
        //        }
        //        jvmodel.JVVDetailModels = lstDetails;
        //        lstJournalModel.Add(jvmodel);
        //        SaveRevaluationPosting(jvmodel);
        //    }
        //}
        #endregion

        #region Grid Call
        public IQueryable<RevaluationModelK> GetAllRevaluationK(string username, long companyId)
        {
            return _revaluationService.GetAllRevaluationsK(username, companyId);
        }
        #endregion

        #region AutoNumber Implimentation
        string value = "";
        public string GenerateAutoNumberForType(long companyId, string Type, string companyCode)
        {
            string generatedAutoNumber = "";
            try
            {
                AppsWorld.RevaluationModule.Entities.Models.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);

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
                    AppsWorld.RevaluationModule.Entities.Models.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    _autoNumberCompanyService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AppsWorld.RevaluationModule.Entities.Models.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.RevaluationModule.Entities.Models.AutoNumberCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
                }
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
            List<Revaluation> lstRev = null;
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
                lstRev = _revaluationService.GetAllRevaluationCompanyId(companyId);

                if (lstRev.Any() && ifMonthContains == true)
                {
                    AppsWorld.RevaluationModule.Entities.Models.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastCretedDate = lstRev.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstRev.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (lastCretedDate == currentMonth)
                        {
                            foreach (var bill in lstRev)
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

                else if (lstRev.Any() && ifMonthContains == false)
                {
                    AppsWorld.RevaluationModule.Entities.Models.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    foreach (var op in lstRev)
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

                if (lstRev.Any())
                {
                    OutputNumber = GetNewNumber(lstRev, Type, OutputNumber, counter, companyFormatHere, counterLength);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return OutputNumber;
        }
        private string GetNewNumber(List<Revaluation> lstCashsale, string type, string outputNumber, string counter, string format, int counterLength)
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

        #region posting
        private void FillRevaluationPosting(JVModel jvm, Revaluation Revaluation, bool isNew, bool? isReversal)
        {
            List<JVModel> lstJvm = new List<JVModel>();
            List<RevalutionDetail> lstDepositDetail = new List<RevalutionDetail>();
            List<RevalutionDetail> lstAccountRecevblesDetail = new List<RevalutionDetail>();
            var rDetail = Revaluation.RevalutionDetails;
            var coaGroups = rDetail.GroupBy(c => c.COAId).ToList();

            //modified code
            List<long> cid = rDetail.Select(a => a.COAId.Value).Distinct().ToList();
            List<ChartOfAccount> lstChartOfAccount = _chartOfAccountService.GetAllChartOfAccountByCid(Revaluation.CompanyId);
            List<ChartOfAccount> lstCOA = lstChartOfAccount.Where(a => cid.Contains(a.Id)).ToList();
            List<AppsWorld.CommonModule.Entities.AccountType> lstAccountType = _accountTypeService.GetAllAccountType(Revaluation.CompanyId, lstCOA.Select(a => a.AccountTypeId.Value).ToList());
            var coaId = _chartOfAccountService.GetChartOfAccountId(Revaluation.CompanyId, COANameConstants.ExchangeGainLossUnrealised);
            bool revaluationOpenPeriod = _finalcialSettingService.RevaluationPeriodLuck(Revaluation.RevalutionDate.Value.AddDays(1), Revaluation.CompanyId);
            var financial = _finalcialSettingService.GetFinancialSetting(Revaluation.CompanyId);

            List<JVVDetailModel> lstDetails1 = new List<JVVDetailModel>();
            var isFirst = isReversal == true ? false : true;
            int? Recoder = 0;
            foreach (var coagroup in coaGroups)
            {
                JVModel jvmodel = new JVModel();
                FillJournal(jvmodel, Revaluation, isReversal, revaluationOpenPeriod, financial.PeriodLockDate);
                if (isFirst == true)
                {
                    jvmodel.IsDelete = true;
                    doc = Revaluation.SystemRefNo;
                }
                else
                    jvmodel.IsDelete = false;
                long? originalCoa = 0;
                var lstentity = coagroup.GroupBy(c => c.EntityId).ToList();
                List<JVVDetailModel> lstDetails = new List<JVVDetailModel>();
                JVVDetailModel detail = new JVVDetailModel();
                if (lstentity.Count() >= 1)
                {
                    decimal? resValue = 0;
                    Guid? entityId = null;
                    foreach (var entities in lstentity)
                    {
                        detail = new JVVDetailModel();
                        originalCoa = entities.Take(1).Select(c => c.COAId).FirstOrDefault();
                        FillJVRevaluationDetail(detail, entities, Revaluation, isReversal, lstCOA.Where(a => a.Id == originalCoa).Select(a => a.Nature).FirstOrDefault(), jvmodel.DocumentState);
                        detail.COAId = coaId;
                        detail.EntityId = entities.Key;
                        entityId = detail.EntityId;
                        detail.RecOrder = Recoder + 1;
                        Recoder = detail.RecOrder;
                        resValue = (detail.DocDebit != 0 && detail.DocDebit != null) ? detail.DocDebit : detail.DocCredit;
                        if (resValue != 0)
                            lstDetails.Add(detail);
                    }
                    if (lstDetails.Any())
                    {
                        long? accId = lstCOA.Where(a => a.Id == originalCoa).Select(a => a.AccountTypeId).FirstOrDefault();
                        detail = new JVVDetailModel();
                        FillTotalJVRevaluationDetail(detail, lstDetails, Revaluation, isReversal);
                        detail.COAId = lstChartOfAccount.Where(a => a.AccountTypeId == accId && a.IsRevaluation == 1).Select(a => a.Id).FirstOrDefault();
                        detail.EntityId = entityId;
                        detail.RecOrder = lstDetails.Max(c => c.RecOrder) + 1;
                        lstDetails.Add(detail);
                        jvmodel.SystemReferenceNo = GetNextApplicationNumber(doc, isFirst, Revaluation.SystemRefNo);
                        doc = jvmodel.SystemReferenceNo;
                        jvmodel.GrandBaseCreditTotal = Math.Round((decimal)lstDetails.Sum(c => c.BaseCredit), 2);
                        jvmodel.GrandBaseDebitTotal = Math.Round((decimal)lstDetails.Sum(c => c.BaseDebit), 2);
                        jvmodel.GrandDocCreditTotal = Math.Round((decimal)lstDetails.Sum(c => c.DocCredit), 2);
                        jvmodel.GrandDocDebitTotal = Math.Round((decimal)lstDetails.Sum(c => c.DocDebit), 2);
                        jvmodel.JVVDetailModels = lstDetails.OrderBy(c => c.RecOrder).ToList();
                        jvmodel.Recorder = Recoder + 1;
                        Recoder = jvmodel.Recorder;
                        jvmodel.IsFirst = isFirst;
                        jvmodel.BaseCurrency = detail.BaseCurrency;
                        isFirst = false;
                        lstJvm.Add(jvmodel);
                    }
                }
            }
            foreach (var postjvm in lstJvm.OrderBy(c => c.Recorder))
            {
                postjvm.ReverseParentRefId = Guid.NewGuid();
                SaveRevaluationPosting(postjvm);
                FillReversalRevaluationPosting(postjvm, Revaluation, revaluationOpenPeriod, financial.PeriodLockDate);
            }
        }
        private void FillReversalRevaluationPosting(JVModel oldJvm, Revaluation Revaluation, bool revaluationPeriodDate, DateTime? finalcialLuckDate)
        {

            JVModel newJvm = new JVModel();
            FillJournal1(newJvm, oldJvm, Revaluation, revaluationPeriodDate, finalcialLuckDate);
            List<JVVDetailModel> lstJvModel = new List<JVVDetailModel>();
            foreach (var jdetail in oldJvm.JVVDetailModels)
            {
                JVVDetailModel detailModel = new JVVDetailModel();
                FillJVRevaluationDetail(detailModel, jdetail, oldJvm);
                detailModel.ServiceCompanyId = newJvm.ServiceCompanyId.Value;
                lstJvModel.Add(detailModel);
            }
            newJvm.JVVDetailModels = lstJvModel;
            newJvm.SystemReferenceNo = GetNextApplicationNumber(doc, false, Revaluation.SystemRefNo);
            doc = newJvm.SystemReferenceNo;
            newJvm.GrandBaseCreditTotal = Math.Round((decimal)lstJvModel.Sum(c => c.BaseCredit), 2);
            newJvm.GrandBaseDebitTotal = Math.Round((decimal)lstJvModel.Sum(c => c.BaseDebit), 2);
            newJvm.GrandDocCreditTotal = Math.Round((decimal)lstJvModel.Sum(c => c.DocCredit), 2);
            newJvm.GrandDocDebitTotal = Math.Round((decimal)lstJvModel.Sum(c => c.DocDebit), 2);
            SaveRevaluationPosting(newJvm);
        }

        private void FillJournal(JVModel jvm, Revaluation Revaluation, bool? isReverce, bool isFinancialCheck, DateTime? finalcialLuckDate)
        {
            jvm.Id = Guid.NewGuid();
            jvm.CompanyId = Revaluation.CompanyId;
            jvm.DocumentId = Revaluation.Id;
            jvm.ServiceCompanyId = Revaluation.ServiceCompanyId;
            jvm.IsMultiCurrency = Revaluation.IsMultiCurrency;
            jvm.IsNoSupportingDocs = Revaluation.IsNoSupportingDocument;
            jvm.IsAllowableNonAllowable = Revaluation.IsAllowableDisAllowable;
            jvm.IsSegmentReporting = Revaluation.IsSegmentReporting;
            jvm.Status = RecordStatusEnum.Active;
            jvm.DocType = DocTypeConstants.JournalVocher;
            jvm.DocSubType = DocTypeConstants.Revaluation;
            jvm.UserCreated = Revaluation.UserCreated;
            jvm.ExchangeRate = 1;
            jvm.CreatedDate = Revaluation.CreatedDate;
            jvm.DocDate = Revaluation.RevalutionDate;
            jvm.PostingDate = Revaluation.RevalutionDate.Value;
            jvm.DueDate = (DateTime?)null;
            jvm.ReversalDate = Revaluation.RevalutionDate.Value.AddDays(1);
            if (finalcialLuckDate == null)
                jvm.DocumentState = "Reversed";
            else
                jvm.DocumentState = isFinancialCheck == true ? "Reversed" : "Posted";
            jvm.IsAutoReversalJournal = jvm.DocumentState == "Reversed" ? true : false;
            jvm.DocumentDescription = jvm.DocumentState == "Reversed" ? DocTypeConstants.Revaluation + "-" + jvm.DocDate.Value.ToString("dd/MM/yyyy") : DocTypeConstants.Revaluation + "-" + jvm.DocDate.Value.ToString("dd/MM/yyyy") + "(" + "Reversal" + ")";
            jvm.DocNo = jvm.DocumentState == "Reversed" ? DocTypeConstants.Revaluation + "-" + jvm.DocDate.Value.ToString("dd/MM/yyyy") : DocTypeConstants.Revaluation + "-" + jvm.DocDate.Value.ToString("dd/MM/yyyy") + "-R";
        }
        private void FillJournal1(JVModel jvmNew, JVModel jvmOld, Revaluation Revaluation, bool revaluationPeriodDate, DateTime? finalcialLuckDate)
        {
            jvmNew.Id = Guid.NewGuid();
            jvmNew.CompanyId = Revaluation.CompanyId;
            jvmNew.DocumentId = Revaluation.Id;
            jvmNew.ServiceCompanyId = Revaluation.ServiceCompanyId;
            jvmNew.IsMultiCurrency = Revaluation.IsMultiCurrency;
            jvmNew.IsNoSupportingDocs = Revaluation.IsNoSupportingDocument;
            jvmNew.IsAllowableNonAllowable = Revaluation.IsAllowableDisAllowable;
            jvmNew.IsSegmentReporting = Revaluation.IsSegmentReporting;
            jvmNew.ReverseParentRefId = jvmOld.ReverseParentRefId;
            jvmNew.Status = RecordStatusEnum.Active;
            jvmNew.DocType = DocTypeConstants.JournalVocher;
            jvmNew.DocSubType = DocTypeConstants.Revaluation;
            jvmNew.UserCreated = Revaluation.UserCreated;
            jvmNew.ExchangeRate = 1;
            jvmNew.CreatedDate = Revaluation.CreatedDate;
            jvmNew.BaseCurrency = jvmOld.BaseCurrency;
            jvmNew.DocCurrency = jvmOld.DocCurrency;//changed by lokanath
            jvmNew.DocDate = Revaluation.RevalutionDate.Value.AddDays(1);
            jvmNew.PostingDate = Revaluation.RevalutionDate.Value.AddDays(1);
            jvmNew.ReversalDate = Revaluation.RevalutionDate.Value.AddDays(1);
            if (finalcialLuckDate == null)
                jvmNew.DocumentState = "Posted";
            else
                jvmNew.DocumentState = revaluationPeriodDate == true ? "Posted" : "Parked";
            jvmNew.IsAutoReversalJournal = (jvmNew.DocumentState == "Reversed" || jvmNew.DocumentState == "Parked") ? true : false;
            jvmNew.DocumentDescription = jvmNew.DocumentState == "Reversed" ? DocTypeConstants.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") : DocTypeConstants.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") + "(" + "Reversal" + ")";
            jvmNew.DocNo = jvmNew.DocumentState == "Reversed" ? DocTypeConstants.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") : DocTypeConstants.Revaluation + "-" + jvmNew.DocDate.Value.ToString("dd/MM/yyyy") + "-R";
        }
        private void FillCancelJournal(JVModel jvm, Journal journal, bool? isCancel, bool? isNew)
        {
            jvm.Id = Guid.NewGuid();
            jvm.CompanyId = journal.CompanyId;
            jvm.DocumentId = journal.Id;
            jvm.ServiceCompanyId = journal.ServiceCompanyId;
            jvm.IsMultiCurrency = journal.IsMultiCurrency;
            jvm.IsNoSupportingDocs = journal.IsNoSupportingDocument;
            //jvm.DocNo = DocTypeConstants.Revaluation + "-" + Revaluation.RevalutionDate.ToString();
            //jvm.DocumentDescription = DocTypeConstants.Revaluation + "-" + Revaluation.RevalutionDate.ToString();

            jvm.Status = RecordStatusEnum.Active;
            jvm.DocType = DocTypeConstants.Revaluation;
            jvm.UserCreated = journal.UserCreated;
            //jvm.ModifiedBy = Revaluation.ModifiedBy;
            jvm.ExchangeRate = 1;
            jvm.CreatedDate = journal.CreatedDate;
            // jvm.ModifiedDate = DateTime.UtcNow;
            jvm.DocDate = journal.DocDate;
            jvm.ReversalDate = journal.ReversalDate;
            jvm.DocumentState = "Cancelled";
            jvm.BaseCurrency = journal.ExCurrency;
            if (isNew == false)
                jvm.SystemReferenceNo = journal.SystemReferenceNo + "- R";
            else
            {
                bool isFirst = jvm.IsFirst ? true : false;
                doc = journal.SystemReferenceNo;
                jvm.SystemReferenceNo = GetNextApplicationNumberForCancel(doc, isFirst, journal.SystemReferenceNo);
                isFirst = true;
                jvm.IsFirst = isFirst;
            }
            //if (isReverce == true)
            //    jvm.DocumentState = "Reversed";
            //else
            //    jvm.DocumentState = "Posted";
        }
        private void FillCOAJVRevaluationDetail(JVVDetailModel detail1, JVVDetailModel detail, Revaluation Revaluation, bool isreversal)
        {
            detail1.Id = Guid.NewGuid();
            detail1.DocType = "Journal";
            detail1.DocSubType = "Revaluation";
            detail1.DocCurrency = detail.BaseCurrency;
            detail1.ExchangeRate = Convert.ToDecimal(1.0000000000);
            detail1.BaseCurrency = detail.BaseCurrency;
            if (isreversal == false)
            {
                detail1.DocDebit = detail.DocCredit;
                detail1.DocCredit = detail.DocDebit;
                detail1.BaseDebit = detail.BaseCredit;
                detail1.BaseCredit = detail.BaseDebit;
            }
            if (isreversal == true)
            {
                detail1.DocCredit = detail.DocCredit;
                detail1.DocDebit = detail.DocDebit;
                detail1.BaseCredit = detail.BaseCredit;
                detail1.BaseDebit = detail.BaseDebit;

            }

        }

        private void FillTotalJVRevaluationDetail(JVVDetailModel detail, List<JVVDetailModel> lstDetails, Revaluation Revaluation, bool? isReversal)
        {
            var details = lstDetails.Take(1).FirstOrDefault();
            detail.Id = Guid.NewGuid();
            detail.DocType = "Journal";
            detail.DocSubType = "Revaluation";
            detail.DocCurrency = details.DocCurrency;
            detail.ExchangeRate = Convert.ToDecimal(1.0000000000);
            detail.BaseCurrency = details.BaseCurrency;
            detail.ServiceCompanyId = Revaluation.ServiceCompanyId.Value;//added by lokanath
            detail.DocDate = Revaluation.RevalutionDate;
            detail.PostingDate = Revaluation.RevalutionDate;
            decimal? valDebit1 = lstDetails.Sum(c => c.DocDebit);
            decimal? valCredit1 = lstDetails.Sum(c => c.DocCredit);

            decimal? revValue = 0;

            if (valDebit1 > valCredit1)
            {
                revValue = valDebit1 - valCredit1;
                detail.DocCredit = revValue.Value;
                detail.BaseCredit = detail.DocCredit;
            }
            else if (valCredit1 > valDebit1)
            {
                revValue = valCredit1 - valDebit1;
                detail.DocDebit = revValue.Value;
                detail.BaseDebit = detail.DocDebit;
            }
            decimal valDebit = valDebit1 == null ? 0 : valDebit1.Value;
            decimal valCredit = valCredit1 == null ? 0 : valCredit1.Value;
            if (valCredit == valDebit)
            {
                detail.DocCredit = 0;
                detail.BaseCredit = 0;
                detail.DocDebit = 0;
                detail.BaseDebit = 0;
            }
            if (valDebit1 == 0)
            {
                detail.DocDebit = (valCredit - valDebit);
                detail.BaseDebit = detail.DocDebit * 1;
            }
            if (valCredit1 == 0)
            {
                detail.DocCredit = (valDebit - valCredit);
                detail.BaseCredit = detail.DocCredit * 1;
            }

        }
        private void FillJVRevaluationDetail(JVVDetailModel detail, IGrouping<Guid?, RevalutionDetail> coagroup, Revaluation Revaluation, bool? isReversal, string nature, string state)
        {
            var details = coagroup.Take(1).FirstOrDefault();
            detail.Id = Guid.NewGuid();
            detail.DocType = "Journal";
            detail.DocSubType = "Revaluation";
            detail.DocCurrency = details.DocCurrency;
            detail.ExchangeRate = Convert.ToDecimal(1.0000000000);
            detail.BaseCurrency = details.BaseCurrency;
            detail.DocDate = Revaluation.RevalutionDate;
            detail.PostingDate = Revaluation.RevalutionDate;
            detail.ServiceCompanyId = Revaluation.ServiceCompanyId.Value;//added by lokanath
            decimal? val = coagroup.Sum(c => c.UnrealisedExchangegainorlose);
            if (isReversal == false && state == "Posted" || state == "Parked")
            {
                if (val > 0 && nature == "Debit")
                {
                    detail.DocCredit = val == null ? 0 : val.Value;
                    detail.BaseCredit = detail.DocCredit * 1;
                }
                else if (val > 0 && nature == "Credit")
                {
                    detail.DocDebit = val == null ? 0 : val.Value;
                    detail.BaseDebit = detail.DocDebit * 1;
                }
                else if (val < 0 && nature == "Debit")
                {
                    detail.DocDebit = val == null ? 0 : -(val.Value);
                    detail.BaseDebit = detail.DocDebit * 1;
                }
                else if (val < 0 && nature == "Credit")
                {
                    detail.DocCredit = val == null ? 0 : -(val.Value);
                    detail.BaseCredit = detail.DocCredit * 1;
                }
            }
            else
            {
                if (val > 0 && nature == "Debit")
                {
                    detail.DocDebit = val == null ? 0 : val.Value;
                    detail.BaseDebit = detail.DocDebit * 1;
                }
                else if (val > 0 && nature == "Credit")
                {
                    detail.DocCredit = val == null ? 0 : val.Value;
                    detail.BaseCredit = detail.DocCredit * 1;
                }
                else if (val < 0 && nature == "Debit")
                {
                    detail.DocCredit = val == null ? 0 : -(val.Value);
                    detail.BaseCredit = detail.DocCredit * 1;
                }
                else if (val < 0 && nature == "Credit")
                {
                    detail.DocDebit = val == null ? 0 : -(val.Value);
                    detail.BaseDebit = detail.DocDebit * 1;
                }
            }
        }


        private void FillJVRevaluationDetail(JVVDetailModel detail, IGrouping<Guid?, RevalutionDetail> entity, Revaluation Revaluation, bool isreversal)
        {
            var details = entity.Take(1).FirstOrDefault();
            detail.Id = Guid.NewGuid();
            detail.DocType = "Journal";
            detail.DocSubType = "Revaluation";
            detail.DocCurrency = details.DocCurrency;
            detail.ExchangeRate = Convert.ToDecimal(1.0000000000);
            detail.BaseCurrency = details.BaseCurrency;
            decimal? val = entity.Sum(c => c.UnrealisedExchangegainorlose);
            if (val == 0)
            {
                detail.DocDebit = 0;
                detail.DocCredit = 0;
                detail.BaseDebit = 0;
                detail.BaseCredit = 0;
            }
            if (val > 0)
            {
                detail.DocDebit = val == null ? 0 : val.Value;
                detail.DocDebit = Math.Abs(detail.DocDebit);
                detail.BaseDebit = detail.DocDebit * 1;
            }
            if (val < 0)
            {
                detail.DocCredit = val == null ? 0 : val.Value;
                detail.DocCredit = Math.Abs(detail.DocCredit);
                detail.BaseCredit = detail.DocCredit * 1;
            }
            //if (isreversal == true)
            //{
            //    detail.DocDebit = detail.DocCredit;
            //    detail.DocCredit = detail.DocDebit;
            //    detail.BaseDebit = detail.BaseCredit;
            //    detail.BaseCredit = detail.BaseDebit;
            //}
        }
        public void SaveRevaluationPosting(JVModel jvModel)
        {
            Log.Logger.ZInfo(jvModel);
            var json = RestHelper.ConvertObjectToJason(jvModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                object obj = jvModel;
                var response = RestHelper.ZPost(url, "api/journal/saveposting", json);
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

        public void SaveReversalRevaluation(RVModel rvModel)
        {
            Log.Logger.ZInfo(rvModel);
            var json = RestHelper.ConvertObjectToJason(rvModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                //if (section.Ziraff.Count > 0)
                //{
                //    for (int i = 0; i < section.Ziraff.Count; i++)
                //    {
                //        if (section.Ziraff[i].Name == RevaluationConstant.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = rvModel;
                //string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                var response = RestHelper.ZPost(url, "api/journal/saverevaluationjournaleverse", json);
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
        private string GetNextApplicationNumberForCancel(string sysNumber, bool isFirst, string originalSysNumber)
        {
            string DocNumber = "";
            try
            {
                int DocNo = 0;
                if (!isFirst)
                {
                    DocNo = Convert.ToInt32(sysNumber.Substring(sysNumber.LastIndexOf("-CL") + 3));
                }
                DocNo++;
                DocNumber = originalSysNumber + ("-CL" + DocNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return DocNumber;
        }
        private void FillJVRevaluationDetail(JVVDetailModel detailNew, JVVDetailModel detailOld, JVModel journal)
        {
            detailNew.Id = Guid.NewGuid();
            detailNew.JournalId = journal.Id;
            detailNew.DocType = "Journal";
            detailNew.DocSubType = "Revaluation";
            detailNew.DocCurrency = detailOld.DocCurrency;
            detailNew.ExchangeRate = Convert.ToDecimal(1.0000000000);
            detailNew.BaseCurrency = detailOld.BaseCurrency;
            detailNew.DocDate = journal.DocDate;
            detailNew.PostingDate = journal.PostingDate;
            detailNew.DocDebit = detailOld.DocCredit;
            detailNew.DocCredit = detailOld.DocDebit;
            detailNew.BaseDebit = detailOld.BaseCredit;
            detailNew.BaseCredit = detailOld.BaseDebit;
            detailNew.COAId = detailOld.COAId;
            detailNew.EntityId = detailOld.EntityId;
            detailNew.RecOrder = detailOld.RecOrder;
        }
        #endregion
    }
}
