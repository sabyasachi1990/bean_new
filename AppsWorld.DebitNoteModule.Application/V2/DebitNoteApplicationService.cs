using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using AppsWorld.DebitNoteModule.Service.V2;
using AppsWorld.DebitNoteModule.Models;
using AppsWorld.DebitNoteModule.Infra.Resources;
using AppsWorld.DebitNoteModule.Entities.V2;
using AppsWorld.DebitNoteModule.Infra;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.DebitNoteModule.Application.V2
{
    public class DebitNoteApplicationService
    {
        readonly IDebitNoteService _debitNoteService;
        readonly IMasterCompactService _masterService;
        readonly IAutoNumberService _autoNumberService;
        public DebitNoteApplicationService(IDebitNoteService debitNoteService, IMasterCompactService masterService, IAutoNumberService autoNumberService)
        {
            this._debitNoteService = debitNoteService;
            this._masterService = masterService;
            this._autoNumberService = autoNumberService;
        }

        #region Create and Lookup Call

        //public DebitNoteModelLU GetDebitNoteAllLUs(string username, Guid debitNoteId, long companyId)
        //{
        //    DebitNoteModelLU debitNoteLU = new DebitNoteModelLU();
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteAllLUs_GetCall_Request_Message);
        //        DebitNote lastDebitNote = _debitNoteService.CreateDebitNote(companyId);
        //        DebitNote debitNote = _debitNoteService.GetDebitNoteById(debitNoteId, companyId);
        //        DateTime date = debitNote == null ? lastDebitNote == null ? DateTime.Now : lastDebitNote.DocDate : debitNote.DocDate;
        //        //List<DebitNoteDetail> lstDebitNotedetails = _debitNoteDetailService.GetAllDebitNoteDetail(debitNoteId);
        //        //debitNoteLU.NatureLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_Nature);
        //        debitNoteLU.NatureLU = new List<string> { "Trade", "Others" };
        //        //debitNoteLU.AllowableNonAllowableLU = _controlCodeCatService.GetByCategoryCodeCategory(companyId, ControlCodeConstants.Control_Codes_AllowableNonalowabl);
        //        debitNoteLU.CompanyId = companyId;
        //        if (debitNote != null)
        //        {
        //            //string currencyCode = debitNote.DocCurrency;
        //            debitNoteLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, debitNote.DocCurrency, ControlCodeConstants.Currency_DefaultCode);
        //            //var lookUpNature = _controlCodeCatService.GetInactiveControlcode(companyId,
        //            //      ControlCodeConstants.Control_codes_VendorType, debitNote.Nature);
        //            //if (lookUpNature != null)
        //            //{
        //            //    debitNoteLU.NatureLU.Lookups.Add(lookUpNature);
        //            //}

        //        }
        //        else
        //        {
        //            debitNoteLU.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
        //        }
        //        #region Segment_reporting_Commented_code
        //        //List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> segments = _segmentMasterService.GetSegments(companyId);
        //        //if (debitNote == null)
        //        //{
        //        //    if (segments.Count > 0)
        //        //        debitNoteLU.SegmentCategory1LU = segments[0];
        //        //    if (segments.Count > 1)
        //        //        debitNoteLU.SegmentCategory2LU = segments[1];
        //        //}
        //        //else
        //        //{
        //        //    if (debitNote.SegmentMasterid1 != null)
        //        //        debitNoteLU.SegmentCategory1LU = _segmentMasterService.GetSegmentsEdit(companyId, debitNote.SegmentMasterid1);
        //        //    else
        //        //        if (segments.Count > 0)
        //        //        debitNoteLU.SegmentCategory1LU = segments[0];
        //        //    if (debitNote.SegmentMasterid2 != null)
        //        //        debitNoteLU.SegmentCategory2LU = _segmentMasterService.GetSegmentsEdit(companyId, debitNote.SegmentMasterid2);
        //        //    else
        //        //        if (segments.Count > 1)
        //        //        debitNoteLU.SegmentCategory2LU = segments[1];
        //        //}
        //        #endregion Segment_reporting_Commented_code

        //        long credit = debitNote == null ? 0 : debitNote.CreditTermsId;
        //        debitNoteLU.TermsOfPaymentLU = _termsOfPaymentService.Queryable()/*.AsEnumerable()*/.Where(a => (a.Status == RecordStatusEnum.Active || a.Id == credit) && a.CompanyId == companyId && a.IsCustomer == true).Select(x => new LookUp<string>()
        //        {
        //            Name = x.Name,
        //            Id = x.Id,
        //            TOPValue = x.TOPValue,
        //            RecOrder = x.RecOrder
        //        }).OrderBy(a => a.TOPValue).ToList();

        //        //if (lstDebitNotedetails.Count > 0)
        //        //{
        //        //    foreach (var debitNoteDetail in lstDebitNotedetails)
        //        //    {
        //        //        long? coa = debitNoteDetail == null ? 0 : debitNoteDetail.COAId;
        //        //        debitNoteLU.ChartOfAccountLU = _chartOfAccountService.ListCOADetail(companyId, true);
        //        //    }
        //        //    debitNoteLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, true, companyId);
        //        //}
        //        //else
        //        //{
        //        //debitNoteLU.TaxCodeLU = _taxCodeService.Listoftaxes(date, false, companyId);
        //        //debitNoteLU.ChartOfAccountLU = _chartOfAccountService.ListCOADetail(companyId, false);
        //        List<COALookup<string>> lstEditCoa = null;
        //        List<TaxCodeLookUp<string>> lstEditTax = null;
        //        List<string> coaName = new List<string> { COANameConstants.Cashandbankbalances, COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables, COANameConstants.AccountsPayable, COANameConstants.OtherPayables, COANameConstants.RetainedEarnings, COANameConstants.Intercompany_clearing, COANameConstants.Intercompany_billing, COANameConstants.System };
        //        List<AccountType> accType = _accountTypeService.GetAllAccountTypeNameByCompanyId(companyId, coaName);
        //        debitNoteLU.ChartOfAccountLU = accType.SelectMany(c => c.ChartOfAccounts.Where(z => z.Status == RecordStatusEnum.Active && z.IsRealCOA == true).Select(x => new COALookup<string>()
        //        {
        //            Name = x.Name,
        //            Id = x.Id,
        //            RecOrder = x.RecOrder,
        //            IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //            IsPLAccount = x.Category == "Income Statement" ? true : false,
        //            Class = x.Class,
        //            Status = x.Status,
        //            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
        //        })).OrderBy(x => x.Name).ToList();

        //        //for observation
        //        //List<COALookup<string>> emptyCoa = new List<COALookup<string>> { new COALookup<string>() { Name = "Select Option", Id = 0 } };
        //        //debitNoteLU.ChartOfAccountLU.AddRange(emptyCoa);



        //        List<TaxCode> allTaxCodes = _taxCodeService.GetTaxAllCodes(0, date);
        //        if (allTaxCodes.Any())
        //            debitNoteLU.TaxCodeLU = allTaxCodes.Where(c => c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
        //            {
        //                Id = x.Id,
        //                Code = x.Code,
        //                Name = x.Name,
        //                TaxRate = x.TaxRate,
        //                TaxType = x.TaxType,
        //                Status = x.Status,
        //                TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
        //            }).OrderBy(c => c.Code).ToList();
        //        if (debitNote != null && debitNote.DebitNoteDetails.Count > 0)
        //        {
        //            List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
        //            if (debitNoteLU.ChartOfAccountLU.Any())
        //                CoaIds = CoaIds.Except(debitNoteLU.ChartOfAccountLU.Select(x => x.Id)).ToList();
        //            List<long?> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId).ToList();
        //            if (debitNoteLU.TaxCodeLU.Any())
        //                taxIds = taxIds.Except(debitNoteLU.TaxCodeLU.Select(d => d.Id)).ToList();
        //            if (CoaIds.Any())
        //            {
        //                //lstEditCoa = _chartOfAccountService.GetAllCOAById(companyid, CoaIds).Select(x => new COALookup<string>()
        //                //{
        //                //    Name = x.Name,
        //                //    Id = x.Id,
        //                //    RecOrder = x.RecOrder,
        //                //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //                //    IsPLAccount = x.Category == "Income Statement" ? true : false,
        //                //    Class = x.Class
        //                //}).ToList();
        //                lstEditCoa = accType.SelectMany(r => r.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
        //                {
        //                    Name = x.Name,
        //                    Id = x.Id,
        //                    RecOrder = x.RecOrder,
        //                    IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //                    IsPLAccount = x.Category == "Income Statement" ? true : false,
        //                    Class = x.Class,
        //                    Status = x.Status,
        //                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
        //                }).OrderBy(d => d.Name).ToList()).ToList();
        //                debitNoteLU.ChartOfAccountLU.AddRange(lstEditCoa);
        //            }
        //            if (taxIds.Any())
        //            {
        //                lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
        //                {
        //                    Id = x.Id,
        //                    Code = x.Code,
        //                    Name = x.Name,
        //                    TaxRate = x.TaxRate,
        //                    TaxType = x.TaxType,
        //                    Status = x.Status,
        //                    TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.Code
        //                }).OrderBy(c => c.Code).ToList();
        //                debitNoteLU.TaxCodeLU.AddRange(lstEditTax);
        //                debitNoteLU.TaxCodeLU = debitNoteLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
        //            }
        //            #region commnted_Code
        //            //List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
        //            //List<long> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId.Value).ToList();

        //            ////creditLU.ChartOfAccountLU.Where(c => c.Id == CoaIds.Contains());
        //            //if (CoaIds.Any())
        //            //{
        //            //    List<long> lstcoaId = debitNoteLU.ChartOfAccountLU.Select(c => c.Id).ToList().Intersect(CoaIds.ToList()).ToList();
        //            //    var coalst = lstcoaId.Except(debitNoteLU.ChartOfAccountLU.Select(x => x.Id));
        //            //    if (coalst.Any())
        //            //    {
        //            //        lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => coalst.Contains(x.Id)).Select(x => new COALookup<string>()
        //            //        {
        //            //            Name = x.Name,
        //            //            Id = x.Id,
        //            //            RecOrder = x.RecOrder,
        //            //            IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //            //            IsPLAccount = x.Category == "Income Statement" ? true : false,
        //            //            Class = c.Class
        //            //        }).ToList()).ToList();
        //            //        debitNoteLU.ChartOfAccountLU.AddRange(lstEditCoa);
        //            //    }
        //            //}


        //            ////var common = creditLU.ChartOfAccountLU.Intersect(lstEditCoa.Select(x=>x.Id));


        //            //if (taxIds.Any())
        //            //{
        //            //    List<long> lsttaxId = debitNoteLU.TaxCodeLU.Select(d => d.Id).ToList().Intersect(taxIds.ToList()).ToList();
        //            //    var taxlst = lsttaxId.Except(debitNoteLU.TaxCodeLU.Select(x => x.Id));
        //            //    if (taxlst.Any())
        //            //    {
        //            //        lstEditTax = allTaxCodes.Where(c => taxlst.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
        //            //        {
        //            //            Id = x.Id,
        //            //            Code = x.Code,
        //            //            Name = x.Name,
        //            //            TaxRate = x.TaxRate,
        //            //            TaxType = x.TaxType,
        //            //            Status = x.Status,
        //            //            TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
        //            //        }).OrderBy(c => c.Code).ToList();
        //            //        debitNoteLU.TaxCodeLU.AddRange(lstEditTax);
        //            //    }
        //            //}
        //            //List<long> CoaIds = debitNote.DebitNoteDetails.Select(c => c.COAId).ToList();
        //            //List<long?> taxIds = debitNote.DebitNoteDetails.Select(x => x.TaxId).ToList();
        //            //if (CoaIds.Any())
        //            //    //lstEditCoa = _chartOfAccountService.GetAllCOAById(companyid, CoaIds).Select(x => new COALookup<string>()
        //            //    //{
        //            //    //    Name = x.Name,
        //            //    //    Id = x.Id,
        //            //    //    RecOrder = x.RecOrder,
        //            //    //    IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //            //    //    IsPLAccount = x.Category == "Income Statement" ? true : false,
        //            //    //    Class = x.Class
        //            //    //}).ToList();
        //            //    lstEditCoa = accType.SelectMany(c => c.ChartOfAccounts.Where(x => CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
        //            //    {
        //            //        Name = x.Name,
        //            //        Id = x.Id,
        //            //        RecOrder = x.RecOrder,
        //            //        IsAllowDisAllow = x.DisAllowable == true ? true : false,
        //            //        IsPLAccount = x.Category == "Income Statement" ? true : false,
        //            //        Class = c.Class
        //            //    }).ToList().OrderBy(d => d.Name)).ToList();
        //            //debitNoteLU.ChartOfAccountLU.AddRange(lstEditCoa);
        //            //if (taxIds.Any())
        //            //{
        //            //    lstEditTax = allTaxCodes.Where(c => taxIds.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
        //            //    {
        //            //        Id = x.Id,
        //            //        Code = x.Code,
        //            //        Name = x.Name,
        //            //        TaxRate = x.TaxRate,
        //            //        TaxType = x.TaxType,
        //            //        Status = x.Status,
        //            //        TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") + "(" + x.TaxType[0] + ")" : x.Code
        //            //    }).OrderBy(c => c.Code).ToList();
        //            //    debitNoteLU.TaxCodeLU.AddRange(lstEditTax);
        //            //}
        //            #endregion

        //        }
        //        //}
        //        //long comp = debitNote == null ? 0 : debitNote.CompanyId;
        //        long? comp = debitNote == null ? 0 : debitNote.ServiceCompanyId == null ? 0 : debitNote.ServiceCompanyId;
        //        debitNoteLU.SubsideryCompanyLU = _companyService.GetSubCompany(username, companyId, comp);
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteAllLUs_GetCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return debitNoteLU;
        //}
        public DebitNoteModel CreateDebitNote(long companyid, Guid id)
        {
            DebitNoteModel debitNoteModel = new DebitNoteModel();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDebitNote_CreateCall_Request_Message);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyid);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                debitNoteModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                debitNoteModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                DebitNote debitNote = _debitNoteService.GetDebitNote(id, companyid);
                DateTime? date = _debitNoteService.GetDNLastPostedDate(companyid);
                if (debitNote == null)
                {
                    AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyid, DocTypeConstants.DebitNote);
                    FillNewDebitNoteModel(debitNoteModel, financSettings, companyid, _autoNo);
                }
                else
                {
                    FillViewModel(debitNoteModel, debitNote);
                    debitNoteModel.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstants.DebitNote);
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDebitNote_CreateCall_SuccessFully_Message);
            }

            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw ex;
            }
            return debitNoteModel;
        }
        public DebitNoteDetail GetDebitNoteDetail(Guid id, Guid detailId)
        {
            DebitNoteDetail detail = _debitNoteService.GetDebitNoteDetail(id, detailId);
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteDetail_GetCall_Request_Message);
                if (detail == null)
                {
                    detail = new DebitNoteDetail();
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteDetail_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw ex;
            }
            return detail;
        }
        //public DebitNoteNote GetDebitNoteNote(Guid id, Guid noteId)
        //{
        //    DebitNoteNote note = _debitNoteNoteService.GetDebitNoteNote(id, noteId);
        //    try
        //    {
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteNote_GetCall_Request_Message);
        //        if (note == null)
        //            note = new DebitNoteNote();
        //        LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetDebitNoteNote_GetCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return note;
        //}

        public CreditNoteModel CreateCreditNoteByDebitNote(Guid debitNoteId, long companyId)
        {
            CreditNoteModel invDTO = new CreditNoteModel();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateCreditNoteByDebitNote_CreateCall_Request_Message);
                AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CreditNote);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(DebitNoteValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                DateTime? lastCreditNote = _debitNoteService.GetInvoiceByCompany(companyId, DocTypeConstants.CreditNote);
                DebitNote debitNote = _debitNoteService.GetDebitNote(debitNoteId, companyId);
                DateTime? lastDebitNote = _debitNoteService.GetDebitNoteCreatedDate(debitNoteId, companyId);

                FillCreditNoteByDebitNote(invDTO, debitNote);
                invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, invDTO.CompanyId);

                bool? isEdit = false;
                invDTO.DocNo = GetAutoNumberByEntityType(companyId, lastDebitNote, DocTypeConstants.CreditNote, _autoNo, lastCreditNote, ref isEdit);
                invDTO.IsDocNoEditable = isEdit;

                List<InvoiceDetailModel> lstInvDetail = new List<InvoiceDetailModel>();
                if (debitNote.DebitNoteDetails.Any())
                {
                    foreach (var detail in debitNote.DebitNoteDetails)
                    {
                        InvoiceDetailModel cnDetail = new InvoiceDetailModel();
                        FillInvoiceDetailV(cnDetail, detail, invDTO);
                        lstInvDetail.Add(cnDetail);
                    }
                }
                invDTO.InvoiceDetailModels = lstInvDetail;
                CreditNoteApplicationModel CNAModel = new CreditNoteApplicationModel();
                FillCreditNoteApplication(CNAModel, debitNote, invDTO, financSettings);
                invDTO.CreditNoteApplicationModel = CNAModel;
                List<CreditNoteApplicationDetailModel> lstCNADModel = new List<CreditNoteApplicationDetailModel>();
                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                FillCreditNoteApplicationDetail(detailModel, debitNote, CNAModel, invDTO);
                lstCNADModel.Add(detailModel);
                invDTO.CreditNoteApplicationModel.CreditNoteApplicationDetailModels = lstCNADModel;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateCreditNoteByDebitNote_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw ex;
            }
            return invDTO;
        }
        public DoubtfulDebtModel CreateDoubtFulDebtByDebitNote(Guid debitNoteId, long companyId)
        {
            DoubtfulDebtModel invDTO = new DoubtfulDebtModel();
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_Request_Message);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(DebitNoteValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.DoubtFulDebitNote);
                DateTime? lastCreditNote = _debitNoteService.GetInvoiceByCompany(companyId, DocTypeConstants.CreditNote);
                DebitNote debitNote = _debitNoteService.GetDebitNote(debitNoteId, companyId);
                DateTime? lastDebitNote = _debitNoteService.GetDebitNoteCreatedDate(debitNoteId, companyId);
                FillDoubtfulDebt(invDTO, debitNote);
                bool? isEdit = false;
                invDTO.DocNo = GetAutoNumberByEntityType(companyId, lastDebitNote, DocTypeConstants.DoubtFulDebitNote, _autoNo, lastCreditNote, ref isEdit);
                invDTO.IsDocNoEditable = isEdit;

                DoubtfulDebtAllocationModel DDAModel = new DoubtfulDebtAllocationModel();
                FillDoubtfulDebtAllocation(DDAModel, debitNote, invDTO, financSettings);
                invDTO.DoubtfulDebtAllocation = DDAModel;

                List<DoubtfulDebtAllocationDetailModel> lstDDAD = new List<DoubtfulDebtAllocationDetailModel>();
                DoubtfulDebtAllocationDetailModel dDAD = new DoubtfulDebtAllocationDetailModel();
                FillDoubtfulDebtAllocationDetail(dDAD, debitNote, DDAModel);
                lstDDAD.Add(dDAD);

                invDTO.DoubtfulDebtAllocation.DoubtfulDebtAllocationDetailModels = lstDDAD;
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_CreateDoubtFulDebtByDebitNote_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw ex;
            }

            return invDTO;
        }

        #endregion

        #region Private_Block
        private void FillNewDebitNoteModel(DebitNoteModel dtoModel, FinancialSettingCompact financSettings, long companyId, AutoNumberCompact _autoNo)
        {
            Dictionary<DateTime, DateTime?> lastInvoice = _debitNoteService.GetDocDate(companyId);
            dtoModel.Id = Guid.NewGuid();
            dtoModel.CompanyId = companyId;
            dtoModel.DocDate = lastInvoice == null ? DateTime.Now : lastInvoice.Keys.FirstOrDefault();
            dtoModel.DocumentState = "Not Paid";
            dtoModel.DueDate = DateTime.UtcNow;
            bool? isEdit = false;
            dtoModel.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice.Values.FirstOrDefault(), DocTypeConstants.DebitNote, _autoNo, null, ref isEdit);
            dtoModel.IsDocNoEditable = isEdit;
            dtoModel.CreatedDate = DateTime.UtcNow;
            dtoModel.BaseCurrency = financSettings.BaseCurrency;
            dtoModel.DocCurrency = dtoModel.BaseCurrency;

        }

        private void FillViewModel(DebitNoteModel dtoModel, DebitNote debitNote)
        {
            try
            {
                Dictionary<string, decimal?> beanEntity = _masterService.GetEntityDataById(debitNote.EntityId);
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillViewModel_FillCall_Request_Message);
                dtoModel.Id = debitNote.Id;
                dtoModel.CompanyId = debitNote.CompanyId;
                dtoModel.EntityType = debitNote.EntityType;
                dtoModel.DocSubType = debitNote.DocSubType;
                dtoModel.DebitNoteNumber = debitNote.DebitNoteNumber;
                dtoModel.DocNo = debitNote.DocNo;
                dtoModel.DocDate = debitNote.DocDate;
                dtoModel.DueDate = debitNote.DueDate;
                dtoModel.PONo = debitNote.PONo;
                dtoModel.EntityId = debitNote.EntityId;
                dtoModel.CreditTermsId = debitNote.CreditTermsId;
                dtoModel.Nature = debitNote.Nature;
                dtoModel.DocCurrency = debitNote.DocCurrency;
                dtoModel.ServiceCompanyId = debitNote.ServiceCompanyId;
                dtoModel.EntityName = beanEntity.Select(c => c.Key).FirstOrDefault();
                dtoModel.CustCreditlimit = beanEntity.Select(c => c.Value).FirstOrDefault();
                dtoModel.IsGSTApplied = debitNote.IsGSTApplied;
                dtoModel.IsMultiCurrency = debitNote.IsMultiCurrency;
                dtoModel.BaseCurrency = debitNote.ExCurrency;
                dtoModel.ExchangeRate = debitNote.ExchangeRate;
                dtoModel.IsGstSettings = debitNote.IsGstSettings;
                dtoModel.GSTExCurrency = debitNote.GSTExCurrency;
                dtoModel.GSTExchangeRate = debitNote.GSTExchangeRate;
                dtoModel.Remarks = debitNote.Remarks;
                dtoModel.IsSegmentReporting = debitNote.IsSegmentReporting;
                dtoModel.BalanceAmount = debitNote.BalanceAmount;
                dtoModel.GSTTotalAmount = debitNote.GSTTotalAmount;
                dtoModel.GrandTotal = debitNote.GrandTotal;
                dtoModel.IsAllowableNonAllowable = debitNote.IsAllowableNonAllowable;
                dtoModel.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
                dtoModel.NoSupportingDocument = debitNote.NoSupportingDocs;
                dtoModel.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
                dtoModel.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
                dtoModel.Status = debitNote.Status;
                dtoModel.DocumentState = debitNote.DocumentState;
                dtoModel.ModifiedDate = debitNote.ModifiedDate;
                dtoModel.ModifiedBy = debitNote.ModifiedBy;
                dtoModel.CreatedDate = debitNote.CreatedDate;
                dtoModel.UserCreated = debitNote.UserCreated;
                dtoModel.AllocatedAmount = debitNote.AllocatedAmount;
                //dtoModel.DebitNoteDetails = debitNote.DebitNoteDetails.OrderBy(c => c.RecOrder).ToList();
                //dtoModel.DebitNoteNotes = _debitNoteNoteService.AllDebitNoteNote(debitNote.Id);
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillViewModel_FillCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_FillViewModel_FillCall_Exception_Message);
                throw ex;
            }
        }

        private void FillCreditNoteByDebitNote(CreditNoteModel invDTO, DebitNote debitNote)
        {
            invDTO.Id = Guid.NewGuid();
            invDTO.CompanyId = debitNote.CompanyId;
            invDTO.EntityType = debitNote.EntityType;
            invDTO.DocSubType = DocTypeConstants.CreditNote;

            invDTO.CreditTermsId = debitNote.CreditTermsId;
            invDTO.DocDate = debitNote.DocDate;
            var top = _masterService.GetTermsOfPayment(invDTO.CreditTermsId);
            if (top != null)
            {
                invDTO.DueDate = invDTO.DocDate.AddDays(top.Keys.FirstOrDefault().Value);
                invDTO.CreditTermsName = top.Values.FirstOrDefault();

            }
            invDTO.EntityId = debitNote.EntityId;
            invDTO.EntityName = _masterService.GetEntityName(debitNote.EntityId);
            invDTO.Nature = debitNote.Nature;
            invDTO.DocCurrency = debitNote.DocCurrency;
            invDTO.ServiceCompanyId = debitNote.ServiceCompanyId;
            invDTO.IsMultiCurrency = debitNote.IsMultiCurrency;
            invDTO.BaseCurrency = debitNote.ExCurrency;
            invDTO.ExchangeRate = debitNote.ExchangeRate;
            invDTO.ExDurationFrom = debitNote.ExDurationFrom;
            invDTO.ExDurationTo = debitNote.ExDurationTo;
            invDTO.IsGstSettings = debitNote.IsGstSettings;
            invDTO.GSTExCurrency = debitNote.GSTExCurrency;
            invDTO.GSTExchangeRate = debitNote.GSTExchangeRate;
            invDTO.GSTExDurationFrom = debitNote.GSTExDurationFrom;
            invDTO.GSTExDurationTo = debitNote.GSTExDurationTo;
            invDTO.ExtensionType = ExtensionType.DebitNote;
            invDTO.GSTTotalAmount = debitNote.GSTTotalAmount;
            invDTO.GrandTotal = debitNote.BalanceAmount;
            invDTO.BalanceAmount = debitNote.BalanceAmount;
            //invDTO.IsAllowableNonAllowable = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, debitNote.CompanyId);
            invDTO.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
            invDTO.NoSupportingDocument = debitNote.NoSupportingDocs;
            invDTO.Remarks = debitNote.Remarks;
            invDTO.Status = debitNote.Status;
            invDTO.DocumentState = debitNote.DocumentState;
            invDTO.CreatedDate = debitNote.CreatedDate;
            invDTO.UserCreated = debitNote.UserCreated;
            invDTO.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
            invDTO.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
        }

        private void FillInvoiceDetailV(InvoiceDetailModel cnDetail, DebitNoteDetail detail, CreditNoteModel invDTO)
        {
            cnDetail.Id = Guid.NewGuid();
            cnDetail.InvoiceId = invDTO.Id;
            cnDetail.BaseAmount = detail.BaseAmount;
            cnDetail.BaseTaxAmount = detail.BaseTaxAmount;
            cnDetail.BaseTotalAmount = detail.BaseTotalAmount;
            cnDetail.COAId = detail.COAId;
            cnDetail.DocAmount = detail.DocAmount;
            cnDetail.DocTaxAmount = detail.DocTaxAmount;
            cnDetail.DocTotalAmount = detail.DocTotalAmount;
            cnDetail.TaxId = detail.TaxId;
            cnDetail.TaxRate = detail.TaxRate;
            if (cnDetail.TaxId != null)
            {
                TaxCodeCompact tax = _masterService.GetTaxById(detail.TaxId.Value);
                cnDetail.TaxIdCode = tax.Code;
                cnDetail.TaxType = tax.TaxType;
            }
        }
        private void FillCreditNoteApplication(CreditNoteApplicationModel CNAModel, DebitNote debitNote, CreditNoteModel invDTO, FinancialSettingCompact financial)
        {
            CNAModel.Id = Guid.NewGuid();
            CNAModel.InvoiceId = invDTO.Id;
            CNAModel.CompanyId = debitNote.CompanyId;
            CNAModel.DocNo = debitNote.DocNo;
            CNAModel.DocCurrency = debitNote.DocCurrency;
            CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
            CNAModel.DocDate = invDTO.DocDate;
            decimal? sumLineTotal = 0;
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
            CNAModel.FinancialPeriodLockStartDate = financial.PeriodLockDate;
            CNAModel.FinancialPeriodLockEndDate = financial.PeriodEndDate;
            CNAModel.CreatedDate = DateTime.UtcNow;
            CNAModel.UserCreated = debitNote.UserCreated;
            CNAModel.Status = CreditNoteApplicationStatus.Posted;
        }
        private void FillCreditNoteApplicationDetail(CreditNoteApplicationDetailModel detailModel, DebitNote debitNote, CreditNoteApplicationModel CNAModel, CreditNoteModel invDTO)
        {
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
            detailModel.SystemReferenceNumber = debitNote.DebitNoteNumber;
            detailModel.BaseCurrencyExchangeRate = debitNote.ExchangeRate.Value;
            if (invDTO.InvoiceDetails.Any())
            {
                detailModel.CreditAmount = debitNote.BalanceAmount;
            }
        }

        private void FillDoubtfulDebt(DoubtfulDebtModel invDTO, DebitNote debitNote)
        {
            invDTO.Id = Guid.NewGuid();

            invDTO.CompanyId = debitNote.CompanyId;
            invDTO.EntityType = debitNote.EntityType;
            invDTO.DocSubType = DocTypeConstants.DoubtFulDebitNote;

            invDTO.DocDate = debitNote.DocDate;

            invDTO.EntityId = debitNote.EntityId;
            invDTO.EntityName = _masterService.GetEntityName(debitNote.EntityId);
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
            invDTO.IsSegmentReporting = debitNote.IsSegmentReporting;
            invDTO.SegmentCategory1 = debitNote.SegmentCategory1;
            invDTO.SegmentCategory2 = debitNote.SegmentCategory2;
            invDTO.GrandTotal = debitNote.BalanceAmount;
            invDTO.BalanceAmount = debitNote.BalanceAmount;
            invDTO.IsAllowableNonAllowable = debitNote.IsAllowableNonAllowable;
            //invDTO.IsAllowableDisallowableActivated = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable, debitNote.CompanyId);
            invDTO.IsNoSupportingDocument = debitNote.IsNoSupportingDocument;
            invDTO.NoSupportingDocument = debitNote.NoSupportingDocs;
            invDTO.SegmentMasterid1 = debitNote.SegmentMasterid1;
            invDTO.Remarks = debitNote.Remarks;
            invDTO.Status = debitNote.Status;
            invDTO.DocumentState = debitNote.DocumentState;
            invDTO.CreatedDate = debitNote.CreatedDate;
            invDTO.UserCreated = debitNote.UserCreated;
            invDTO.IsBaseCurrencyRateChanged = debitNote.IsBaseCurrencyRateChanged;
            invDTO.IsGSTCurrencyRateChanged = debitNote.IsGSTCurrencyRateChanged;
        }
        private void FillDoubtfulDebtAllocation(DoubtfulDebtAllocationModel DDAModel, DebitNote debitNote, DoubtfulDebtModel invDTO, FinancialSettingCompact financial)
        {
            DDAModel.Id = Guid.NewGuid();
            DDAModel.CompanyId = debitNote.CompanyId;
            DDAModel.InvoiceId = invDTO.Id;
            DDAModel.DocNo = debitNote.DocNo;
            DDAModel.DoubtfulDebitAllocationDate = invDTO.DocDate;
            DDAModel.DocCurrency = debitNote.DocCurrency;
            DDAModel.DoubtfulDebtAmount = debitNote.BalanceAmount;
            DDAModel.DoubtfulDebtBalanceAmount = debitNote.BalanceAmount;
            DDAModel.AllocateAmount = debitNote.BalanceAmount;
            DDAModel.DoubtfulDebtAllocationNumber = debitNote.DebitNoteNumber;
            DDAModel.FinancialPeriodLockStartDate = financial.PeriodLockDate;
            DDAModel.FinancialPeriodLockEndDate = financial.PeriodEndDate;
            DDAModel.Status = DoubtfulDebtAllocationStatus.Posted;
            DDAModel.DocDate = debitNote.DocDate;
            //DDAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, invDTO.CompanyId);
            DDAModel.NoSupportingDocument = false;
        }
        private void FillDoubtfulDebtAllocationDetail(DoubtfulDebtAllocationDetailModel dDAD, DebitNote debitNote, DoubtfulDebtAllocationModel DDAModel)
        {
            dDAD.Id = Guid.NewGuid();
            dDAD.DoubtfulDebitAllocationId = DDAModel.Id;
            dDAD.DocType = DocTypeConstants.DebitNote;
            dDAD.DocCurrency = DDAModel.DocCurrency;
            dDAD.DocAmount = debitNote.GrandTotal;
            dDAD.DocDate = debitNote.DocDate;
            dDAD.DocumentId = debitNote.Id;
            dDAD.DocNo = debitNote.DocNo;
            dDAD.SystemReferenceNumber = debitNote.DebitNoteNumber;
            dDAD.AllocateAmount = debitNote.BalanceAmount;
            dDAD.BalanceAmount = debitNote.BalanceAmount;
            dDAD.Nature = debitNote.Nature;
        }
        #endregion Private_Block

        #region Auto_Number_Block
        private string GetAutoNumberByEntityType(long companyId, DateTime? lastInvoice, string entityType, AutoNumberCompact _autoNo, DateTime? oldInvoice, ref bool? isEdit)
        {
            string outPutNumber = null;
            string output = null;
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    if (entityType != DocTypeConstants.DebitNote)
                        outPutNumber = GetNewInvoiceDocumentNumber(entityType, companyId);
                    else
                        outPutNumber = GetNewDebitNoteDocumentNumber(companyId);
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
                    if (entityType != DocTypeConstants.DebitNote)
                    {
                        if (oldInvoice != null)
                        {
                            if (_autoNo.Format.Contains("{MM/YYYY}"))
                            {
                                //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                                if (oldInvoice.Value.Month != DateTime.UtcNow.Month)
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

                    else
                    {
                        if (lastInvoice != null)
                        {
                            if (_autoNo.Format.Contains("{MM/YYYY}"))
                            {
                                //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                                if (lastInvoice.Value.Month != DateTime.UtcNow.Month)
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
                }
            }
            return outPutNumber;
        }

        //private string GetAutoNumberForReceipt(long companyId, Receipt lastInvoice, string entityType, AppsWorld.DebitNoteModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        //{
        //    string outPutNumber = null;
        //    //isEdit = false;
        //    //AppsWorld.ReceiptModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
        //    if (_autoNo != null)
        //    {
        //        if (_autoNo.IsEditable == true)
        //        {
        //            outPutNumber = GetNewReceiptDocNo(DocTypeConstants.ReceiptDoc, companyId);
        //            //invDTO.IsEditable = true;
        //            isEdit = true;
        //        }
        //        else
        //        {
        //            //invDTO.IsEditable = false;
        //            isEdit = false;
        //            //List<Invoice> lstInvoice = _invoiceEntityService.GetAllInvoiceByCIDandType(companyid, DocTypeConstants.Invoice);
        //            string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
        //            string number = "1";
        //            if (lastInvoice != null)
        //            {
        //                if (_autoNo.Format.Contains("{MM/YYYY}"))
        //                {
        //                    //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
        //                    if (lastInvoice.CreatedDate.Value.Month != DateTime.UtcNow.Month)
        //                    {
        //                        //number = "1";
        //                        outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');
        //                    }
        //                    else
        //                    {
        //                        string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
        //                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
        //                    }
        //                }
        //                else
        //                {
        //                    string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
        //                    outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
        //                }
        //            }
        //            else
        //            {
        //                string output = (Convert.ToInt32(_autoNo.GeneratedNumber)).ToString();
        //                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
        //                //counter = Convert.ToInt32(value);
        //            }
        //        }
        //    }
        //    return outPutNumber;
        //}

        private string GetNewDebitNoteDocumentNumber(long CompanyId)
        {
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewDebitNoteDocumentNumber_GetCall_Request_Message);
                DebitNote debitnote = _debitNoteService.CreateDebitNoteForDocNo(CompanyId);
                if (debitnote != null)
                {
                    string strOldNo = String.Empty;
                    DebitNote duplicatDebiteNote;
                    int index;
                    strOldDocNo = debitnote.DocNo;

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

                        duplicatDebiteNote = _debitNoteService.GetDocNo(strNewDocNo, CompanyId);
                    } while (duplicatDebiteNote != null);
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewDebitNoteDocumentNumber_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(DebitNoteConstants.DebitNoteApplicationService, ex, ex.Message);
                throw ex;
            }
            return strNewDocNo;
        }

        private string GetNewInvoiceDocumentNumber(string docType, long CompanyId)
        {
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            try
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_Request_Message);
                string documentNo = _debitNoteService.GetCNDocDate(CompanyId, docType);

                if (documentNo != null)
                {
                    string strOldNo = String.Empty;
                    string duplicatInvoice;
                    int index;
                    strOldDocNo = documentNo;

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

                        duplicatInvoice = _debitNoteService.GetDuplicateInvoice(CompanyId, docType, strNewDocNo);
                    } while (duplicatInvoice != null);
                }
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogMessage(DebitNoteConstants.DebitNoteApplicationService, DebitNoteLoggingValaidation.Log_GetNewInvoiceDocumentNumber_GetCall_Exception_Message);
                throw ex;
            }
            return strNewDocNo;
        }

        #endregion Auto_Number_Block

    }
}
