using System.Linq;
using AppsWorld.InvoiceModule.Service.V2;
using AppsWorld.InvoiceModule.Models;
using AppsWorld.InvoiceModule.Entities.V2;
using AppsWorld.InvoiceModule.Infra;
using AppsWorld.InvoiceModule.RepositoryPattern.V2;
using AppsWorld.CommonModule.Models;
using AppsWorld.InvoiceModule.Infra.Resources;
using System;
using Ziraff.FrameWork.Logging;
using System.Collections.Generic;
using AppsWorld.Framework;
using Repository.Pattern.Infrastructure;
using System.Data.SqlClient;
using System.Data;
using AppsWorld.CommonModule.Infra;
using Ziraff.Section;
using System.Configuration;
using System.Data.Entity.Validation;
using AppaWorld.Bean;
using Newtonsoft.Json;
using System.ComponentModel;

namespace AppsWorld.InvoiceModule.Application.V2
{
    public class CNAApplicationService
    {
        private readonly ICNApplicationService _cnApplicationService;
        private readonly IMasterCompactService _masterService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemCompactService _itemService;
        private readonly ICurrencyCompactService _currencyService;
        private readonly IApplicationCompactModuleUnitOfWorkAsync _unitOfWork;
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader dr = null;
        string docNo = string.Empty;
        string query = string.Empty;
        public CNAApplicationService(ICNApplicationService cnApplicationService, IMasterCompactService masterService, IAutoNumberService autoNumberService, IItemCompactService itemService, IApplicationCompactModuleUnitOfWorkAsync unitOfWork, ICurrencyCompactService currencyService)
        {
            this._cnApplicationService = cnApplicationService;
            this._masterService = masterService;
            this._autoNumberService = autoNumberService;
            this._itemService = itemService;
            this._unitOfWork = unitOfWork;
            this._currencyService = currencyService;
        }

        #region CreateCreditNoteApplication
        public CreditNoteApplicationModel CreateCreditNoteApplication(Guid creditNoteId, Guid cnApplicationId, long companyId, string username, bool isView, DateTime applicationDate, bool isICActive, string connectionString)
        {
            CreditNoteApplicationModel CNAModel = new CreditNoteApplicationModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplication_CreateCall_Request_Message);
                InvoiceCompact creditNote = _cnApplicationService.GetCreditNoteByCompanyIdAndId(companyId, creditNoteId);
                Dictionary<long, string> lstCompanies1 = new Dictionary<long, string>();
                long? serviceEntityId = null;
                string companyName = null;
                bool isIC = false;
                #region Interco_ServiceEntity_Settings_Changes
                using (con = new SqlConnection(connectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    query = $"Select ICG.ServiceEntityId as ServiceEntityId,COM.ShortName as Name from Bean.InterCompanySetting IC JOIN Bean.InterCompanySettingDetail ICG on IC.Id=ICG.InterCompanySettingId Left JOIN Common.Company COM On ICG.ServiceEntityId = COM.Id JOIN Common.CompanyUser CU on CU.CompanyId = COM.ParentId JOIN Common.CompanyUserDetail CUD on CUD.CompanyUserId = CU.Id and CUD.ServiceEntityId = COM.Id where COM.ParentId = {companyId} and IC.InterCompanyType = 'Clearing' and ICG.Status=1 and CU.username ='{username}'";
                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        serviceEntityId = dr["ServiceEntityId"] != null ? Convert.ToInt64(dr["ServiceEntityId"]) : (long?)null;
                        companyName = dr["Name"].ToString();
                        if (serviceEntityId != null)
                            lstCompanies1.Add(serviceEntityId.Value, companyName);
                    }
                }
                if (lstCompanies1.Any())
                    isIC = lstCompanies1.Any(c => c.Key == creditNote.ServiceCompanyId);

                #endregion Interco_ServiceEntity_Settings_Changes

                //to check if it is void or not
                //if (_cnApplicationService.IsVoid(companyId, creditNoteId))
                //    throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);

                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CreditNoteValidation.The_Financial_setting_should_be_activated);
                }
                CNAModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                CNAModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;

                if (creditNote == null)
                    throw new Exception(CreditNoteValidation.Invalid_CreditNote);
                Dictionary<long, string> lstCOAName = _masterService.GetReceivableAccounts(companyId);
                IDictionary<long, string> lstComapny = null;
                IDictionary<long, string> lstSubCompanies = null;
                IDictionary<long, string> lstComp = null;
                IDictionary<long, string> lstSubCompany = null;
                IDictionary<long, string> lstComp1 = null;
                IDictionary<long, string> lstCompanies = null;
                List<long?> lstServiceEntityIds = new List<long?>();
                CreditNoteApplication CNApplication = _cnApplicationService.GetAllCreditNote(creditNoteId, cnApplicationId, companyId);
                if (CNApplication != null)
                {
                    FillCreditNoteApplicationModel(CNAModel, CNApplication);
                    CNAModel.EntityName = _masterService.GetEntityName(creditNote.EntityId);
                    CNAModel.DocDate = creditNote.DocDate;
                    CNAModel.Remarks = CNApplication.Remarks != null || CNApplication.Remarks != string.Empty ? CNApplication.Remarks : creditNote.DocDescription;
                    List<CreditNoteApplicationDetailModel> lstDetailModel = new List<CreditNoteApplicationDetailModel>();
                    List<CreditNoteApplicationDetail> lstCNAD = _cnApplicationService.GetCreditNoteDetail(cnApplicationId);
                    List<Guid> invoiceIds = lstCNAD.Where(c => c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.Receipt || c.DocumentType == DocTypeConstants.BillPayment).Select(d => d.DocumentId).ToList();
                    List<Guid> debitNoteIds = lstCNAD.Where(c => c.DocumentType == DocTypeConstants.DebitNote).Select(d => d.DocumentId).ToList();
                    List<InvoiceCompact> lstInvoices = _cnApplicationService.GetAllDDByInvoiceId(invoiceIds);
                    List<DebitNoteCompact> lstDNs = _cnApplicationService.GetDDByDebitNoteId(debitNoteIds);
                    //if (lstInvoices.Any())
                    //    lstServiceEntityIds.AddRange(lstInvoices.Select(c => c.ServiceCompanyId).ToList());
                    //if (lstDNs.Any())
                    //    lstServiceEntityIds.AddRange(lstDNs.Select(c => c.ServiceCompanyId).ToList());
                    //if (lstServiceEntityIds.Any())
                    //    lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);

                    #region Payment/Receipt Hiper_Link
                    Guid? receiptId = Guid.Empty;
                    Guid? paymentId = Guid.Empty;
                    long? receiptComp = null;
                    long? paymentComp = null;
                    if (CNApplication.DocumentId != null)
                    {
                        using (con = new SqlConnection(connectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            if (CNApplication.CreditNoteApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.BillPayment).Any())
                                query = "Select PD.PaymentId as 'PaymentId',P.ServiceCompanyId as 'ServiceCompanyId' from Bean.Payment P JOIN Bean.PaymentDetail PD on P.Id = PD.PaymentId where PD.Id ='" + CNApplication.DocumentId + "'";
                            else if (CNApplication.CreditNoteApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.Receipt).Any())
                                query = "Select RD.ReceiptId as 'ReceiptId',R.ServiceCompanyId as 'ServiceCompanyId' from Bean.Receipt R JOIN Bean.ReceiptDetail RD on R.Id = RD.ReceiptId where RD.Id ='" + CNApplication.DocumentId + "'";
                            cmd = new SqlCommand(query, con);
                            dr = cmd.ExecuteReader();
                            if (dr.HasRows)
                            {
                                if (dr.Read())
                                {
                                    if (CNApplication.CreditNoteApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.BillPayment).Any())
                                    {
                                        paymentId = dr["PaymentId"] != DBNull.Value ? Guid.Parse(dr["PaymentId"].ToString()) : Guid.Empty;
                                        paymentComp = dr["ServiceCompanyId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceCompanyId"]) : Convert.ToInt64(dr["ServiceCompanyId"]);
                                    }
                                    else if (CNApplication.CreditNoteApplicationDetails.Where(c => c.DocumentType == DocTypeConstants.Receipt).Any())
                                    {
                                        receiptId = dr["ReceiptId"] != DBNull.Value ? Guid.Parse(dr["ReceiptId"].ToString()) : Guid.Empty;
                                        receiptComp = dr["ServiceCompanyId"] != DBNull.Value ? Convert.ToInt64(dr["ServiceCompanyId"]) : Convert.ToInt64(dr["ServiceCompanyId"]);
                                    }

                                }
                            }
                        }
                    }

                    #endregion Payment/Receipt Hiper_Link
                    if (CNApplication.Status == CreditNoteApplicationStatus.Void || _cnApplicationService.IsVoid(companyId, creditNoteId))
                    {
                        List<long?> serviceEntityIds = null;
                        if (receiptComp.HasValue)
                            serviceEntityIds = new List<long?> { receiptComp };
                        else if (paymentComp.HasValue)
                            serviceEntityIds = new List<long?> { paymentComp };
                        if (lstInvoices.Any())
                        {
                            if (serviceEntityIds != null)
                                serviceEntityIds.AddRange(lstInvoices.Select(c => c.ServiceCompanyId).ToList());
                            else
                                serviceEntityIds = lstInvoices.Select(c => c.ServiceCompanyId).ToList();
                            lstCompanies = _masterService.GetAllCompaniesCode(serviceEntityIds);
                            lstSubCompanies = _masterService.GetAllSubCompanies(serviceEntityIds, username, companyId);
                            lstComp = lstCompanies.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                        }
                        if (lstDNs.Any())
                        {
                            lstComapny = _masterService.GetAllCompaniesCode(lstDNs.Select(c => c.ServiceCompanyId).ToList());
                            lstSubCompany = _masterService.GetAllSubCompanies(lstDNs.Select(a => a.ServiceCompanyId).ToList(), username, companyId);
                            lstComp1 = lstComapny.Except(lstSubCompany).ToDictionary(Id => Id.Key, Name => Name.Value);
                        }
                        foreach (CreditNoteApplicationDetail cd in lstCNAD.Where(c => c.CreditAmount > 0).ToList())
                        {
                            CreditNoteApplicationDetailModel CNAdetailModel = new CreditNoteApplicationDetailModel();
                            CNAdetailModel.BaseCurrencyExchangeRate = Convert.ToDecimal(cd.BaseCurrencyExchangeRate);
                            CNAdetailModel.COAId = cd.COAId;
                            CNAdetailModel.CreditAmount = cd.CreditAmount;
                            CNAdetailModel.CreditNoteApplicationId = cd.CreditNoteApplicationId;
                            CNAdetailModel.DocCurrency = cd.DocCurrency;
                            CNAdetailModel.Id = cd.Id;
                            CNAdetailModel.DocNo = cd.DocNo;
                            CNAdetailModel.DocType = cd.DocumentType;
                            CNAdetailModel.DocumentId = cd.DocumentId;
                            CNAdetailModel.ServiceEntityId = cd.ServiceEntityId;
                            CNAdetailModel.SystemReferenceNumber = cd.DocNo;

                            if (cd.DocumentType == DocTypeConstants.Invoice || cd.DocumentType == DocTypeConstants.Receipt || cd.DocumentType == DocTypeConstants.BillPayment)
                            {
                                InvoiceCompact invoice = lstInvoices.Where(c => c.Id == cd.DocumentId).FirstOrDefault();
                                if (invoice != null)
                                {
                                    CNAdetailModel.DocAmount = invoice.GrandTotal;
                                    CNAdetailModel.DocDate = (cd.DocumentType == DocTypeConstants.Receipt || cd.DocumentType == DocTypeConstants.BillPayment) ? CNApplication.CreditNoteApplicationDate : invoice.DocDate;

                                    CNAdetailModel.DocumentId = cd.DocumentType == DocTypeConstants.Receipt ? (receiptId != Guid.Empty ? receiptId.Value : Guid.Empty) : paymentId != Guid.Empty ? paymentId.Value : invoice.Id;
                                    CNAdetailModel.DocNo = (cd.DocumentType == DocTypeConstants.Receipt || cd.DocumentType == DocTypeConstants.BillPayment) ? cd.DocNo : invoice.DocNo;
                                    CNAdetailModel.ServiceEntityId = isIC ? lstCompanies.Where(c => c.Key == invoice.ServiceCompanyId).Select(d => d.Key).FirstOrDefault() : invoice.ServiceCompanyId;
                                    CNAdetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == invoice.ServiceCompanyId).Any() ? false : true : true;
                                    CNAdetailModel.ServEntityName = isIC ? lstCompanies.Where(c => c.Key == invoice.ServiceCompanyId).Select(d => d.Value).FirstOrDefault() : string.Empty;
                                    if (cd.DocumentType == DocTypeConstants.Receipt || cd.DocumentType == DocTypeConstants.BillPayment)
                                    {
                                        if (cd.DocumentType == DocTypeConstants.Receipt)
                                            CNAdetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == receiptComp).Any() ? false : false : true;
                                        else if (cd.DocumentType == DocTypeConstants.BillPayment)
                                            CNAdetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == paymentComp).Any() ? false : false : true;
                                    }

                                    CNAdetailModel.SystemReferenceNumber = invoice.DocNo;
                                    if (CNApplication.Status == CreditNoteApplicationStatus.Void)
                                        CNAdetailModel.BalanceAmount = invoice.BalanceAmount;
                                    else
                                        CNAdetailModel.BalanceAmount = invoice.BalanceAmount + cd.CreditAmount;
                                    CNAdetailModel.DocType = (cd.DocumentType == DocTypeConstants.Receipt || cd.DocumentType == DocTypeConstants.BillPayment) ? cd.DocumentType : invoice.DocType;
                                    CNAdetailModel.Nature = invoice.Nature;
                                    CNAdetailModel.DocState = (cd.DocumentType == DocTypeConstants.Receipt || cd.DocumentType == DocTypeConstants.BillPayment) ? ReceiptState.Posted : invoice.DocumentState;
                                    CNAdetailModel.BaseCurrencyExchangeRate = invoice.ExchangeRate.Value;
                                }
                            }
                            else if (cd.DocumentType == DocTypeConstants.DebitNote)
                            {
                                DebitNoteCompact debitNote = lstDNs.Where(c => c.Id == cd.DocumentId).FirstOrDefault();
                                if (debitNote != null)
                                {
                                    CNAdetailModel.DocAmount = debitNote.GrandTotal;
                                    CNAdetailModel.DocDate = debitNote.DocDate;
                                    CNAdetailModel.DocumentId = debitNote.Id;
                                    CNAdetailModel.DocNo = debitNote.DocNo;
                                    CNAdetailModel.ServiceEntityId = isIC ? lstComapny.Where(c => c.Key == debitNote.ServiceCompanyId).Select(d => d.Key).FirstOrDefault() : debitNote.ServiceCompanyId;
                                    CNAdetailModel.IsHyperLinkEnable = lstComp1.Any() ? lstComp1.Where(c => c.Key == debitNote.ServiceCompanyId).Any() ? false : true : true;
                                    CNAdetailModel.ServEntityName = isIC ? lstComapny.Where(c => c.Key == debitNote.ServiceCompanyId).Select(d => d.Value).FirstOrDefault() : string.Empty;
                                    CNAdetailModel.SystemReferenceNumber = debitNote.DocNo;
                                    if (CNApplication.Status == CreditNoteApplicationStatus.Void)
                                        CNAdetailModel.BalanceAmount = debitNote.BalanceAmount;
                                    else
                                        CNAdetailModel.BalanceAmount = debitNote.BalanceAmount + cd.CreditAmount;
                                    CNAdetailModel.DocType = debitNote.DocSubType;
                                    CNAdetailModel.Nature = debitNote.Nature;
                                    CNAdetailModel.DocState = debitNote.DocumentState;
                                    CNAdetailModel.BaseCurrencyExchangeRate = debitNote.ExchangeRate.Value;
                                }
                            }

                            else if (CNApplication.IsRevExcess == true)
                                CNAModel.ReverseExcessModels = CNApplication.CreditNoteApplicationDetails.Select(d => new ReverseExcessModel()
                                {
                                    Id = d.Id,
                                    CompanyId = companyId,
                                    DocAmount = d.CreditAmount,
                                    DocTaxAmount = d.TaxAmount,
                                    DocTotalAmount = d.TotalAmount,
                                    TaxId = d.TaxId,
                                    TaxRate = d.TaxRate,
                                    TaxIdCode = d.TaxIdCode,
                                    RecOrder = d.RecOrder,
                                    COAId = d.COAId,
                                    ItemDescription = d.DocDescription
                                }).OrderBy(d => d.RecOrder).ToList();
                            lstDetailModel.Add(CNAdetailModel);
                        }
                        CNAModel.CreditNoteApplicationDetailModels = lstDetailModel.OrderBy(c => c.DocDate).ThenBy(d => d.SystemReferenceNumber).ToList();
                    }
                    else
                    {
                        if (CNApplication.IsRevExcess != true)
                        {
                            List<long?> serviceEntityIds = null;
                            if (receiptComp.HasValue)
                                serviceEntityIds = new List<long?> { receiptComp };
                            else if (paymentComp.HasValue)
                                serviceEntityIds = new List<long?> { paymentComp };
                            if (lstInvoices.Any())
                            {
                                if (serviceEntityIds != null)
                                    serviceEntityIds.AddRange(lstInvoices.Select(c => c.ServiceCompanyId).ToList());
                                else
                                    serviceEntityIds = lstInvoices.Select(c => c.ServiceCompanyId).ToList();
                                lstCompanies = _masterService.GetAllCompaniesCode(serviceEntityIds);
                                lstSubCompanies = _masterService.GetAllSubCompanies(serviceEntityIds, username, companyId);
                                lstComp = lstCompanies.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                            }
                            if (lstDNs.Any())
                            {
                                lstComapny = _masterService.GetAllCompaniesCode(lstDNs.Select(c => c.ServiceCompanyId).ToList());
                                lstSubCompany = _masterService.GetAllSubCompanies(lstDNs.Select(a => a.ServiceCompanyId).ToList(), username, companyId);
                                lstComp1 = lstComapny.Except(lstSubCompany).ToDictionary(Id => Id.Key, Name => Name.Value);
                            }
                            foreach (CreditNoteApplicationDetail detail in lstCNAD)
                            {
                                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                detailModel.Id = detail.Id;
                                detailModel.CreditNoteApplicationId = detail.CreditNoteApplicationId;
                                detailModel.DocCurrency = CNAModel.DocCurrency;
                                detailModel.CreditAmount = detail.CreditAmount;
                                if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment)
                                {
                                    //var invoice = _invoiceEntityService.GetCreditNoteByDocumentId(detail.DocumentId);
                                    InvoiceCompact invoice = lstInvoices.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                                    if (invoice != null)
                                    {
                                        detailModel.DocAmount = invoice.GrandTotal;
                                        detailModel.DocDate = (detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment) ? CNApplication.CreditNoteApplicationDate : invoice.DocDate;
                                        //detailModel.DocumentId = invoice.Id;
                                        detailModel.DocumentId = detail.DocumentType == DocTypeConstants.Receipt ? (receiptId != Guid.Empty ? receiptId.Value : Guid.Empty) : paymentId != Guid.Empty ? paymentId.Value : invoice.Id;
                                        detailModel.DocNo = (detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment) ? detail.DocNo : invoice.DocNo;
                                        detailModel.COAId = detail.COAId;

                                        //detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == invoice.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                                        //detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == invoice.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                                        detailModel.ServiceEntityId = isIC ? lstCompanies.Where(c => c.Key == invoice.ServiceCompanyId).Select(d => d.Key).FirstOrDefault() : invoice.ServiceCompanyId;
                                        detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == invoice.ServiceCompanyId).Any() ? false : true : true;
                                        detailModel.ServEntityName = isIC ? lstCompanies.Where(c => c.Key == invoice.ServiceCompanyId).Select(d => d.Value).FirstOrDefault() : string.Empty;
                                        if (detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment)
                                        {
                                            if (detail.DocumentType == DocTypeConstants.Receipt)
                                            {
                                                detailModel.ServiceEntityId = isIC ? lstCompanies.Where(c => c.Key == receiptComp).Select(d => d.Key).FirstOrDefault() : receiptComp;
                                                detailModel.ServEntityName = isIC ? lstCompanies.Where(c => c.Key == receiptComp).Select(d => d.Value).FirstOrDefault() : string.Empty;
                                                detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == receiptComp).Any() ? false : false : true;
                                            }
                                            else if (detail.DocumentType == DocTypeConstants.BillPayment)
                                            {
                                                detailModel.ServiceEntityId = isIC ? lstCompanies.Where(c => c.Key == paymentComp).Select(d => d.Key).FirstOrDefault() : paymentComp;
                                                detailModel.ServEntityName = isIC ? lstCompanies.Where(c => c.Key == paymentComp).Select(d => d.Value).FirstOrDefault() : string.Empty;
                                                detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == paymentComp).Any() ? false : false : true;
                                            }
                                        }
                                        if (CNApplication.Status == CreditNoteApplicationStatus.Void)
                                            detailModel.BalanceAmount = invoice.BalanceAmount;
                                        else
                                            detailModel.BalanceAmount = invoice.BalanceAmount + detail.CreditAmount;
                                        detailModel.DocType = (detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment) ? detail.DocumentType : invoice.DocType;
                                        detailModel.Nature = invoice.Nature;
                                        detailModel.DocState = (detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment) ? ReceiptState.Posted : invoice.DocumentState;
                                        //detailModel.SegmentCategory1 = invoice.SegmentCategory1;
                                        //detailModel.SegmentCategory2 = invoice.SegmentCategory2;
                                        detailModel.BaseCurrencyExchangeRate = invoice.ExchangeRate.Value;
                                    }
                                    detailModel.COAId = detail.COAId;
                                }
                                else if (detail.DocumentType == DocTypeConstants.DebitNote)
                                {
                                    //var debitNote = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                                    DebitNoteCompact debitNote = lstDNs.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                                    if (debitNote != null)
                                    {
                                        detailModel.DocAmount = debitNote.GrandTotal;
                                        detailModel.DocDate = debitNote.DocDate;
                                        detailModel.DocumentId = debitNote.Id;
                                        detailModel.DocNo = debitNote.DocNo;
                                        detailModel.COAId = detail.COAId;
                                        //detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == debitNote.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                                        //detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == debitNote.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;

                                        detailModel.ServiceEntityId = isIC ? lstComapny.Where(c => c.Key == debitNote.ServiceCompanyId).Select(d => d.Key).FirstOrDefault() : debitNote.ServiceCompanyId;
                                        detailModel.IsHyperLinkEnable = lstComp1.Any() ? lstComp1.Where(c => c.Key == debitNote.ServiceCompanyId).Any() ? false : true : true;
                                        detailModel.ServEntityName = isIC ? lstComapny.Where(c => c.Key == debitNote.ServiceCompanyId).Select(d => d.Value).FirstOrDefault() : string.Empty;
                                        detailModel.SystemReferenceNumber = debitNote.DocNo;
                                        if (CNApplication.Status == CreditNoteApplicationStatus.Void)
                                            detailModel.BalanceAmount = debitNote.BalanceAmount;
                                        else
                                            detailModel.BalanceAmount = debitNote.BalanceAmount + detail.CreditAmount;
                                        detailModel.DocType = debitNote.DocSubType;
                                        detailModel.Nature = debitNote.Nature;
                                        detailModel.DocState = debitNote.DocumentState;
                                        //detailModel.SegmentCategory1 = debitNote.SegmentCategory1;
                                        //detailModel.SegmentCategory2 = debitNote.SegmentCategory2;
                                        detailModel.BaseCurrencyExchangeRate = debitNote.ExchangeRate.Value;
                                    }
                                    detailModel.COAId = detail.COAId;
                                }
                                lstDetailModel.Add(detailModel);
                            }
                        }
                        if (isView != true)
                        {
                            if (creditNote.Nature != "Interco")
                            {
                                #region Normal CN_App_Outstanding_ForEdit
                                List<InvoiceCompact> lstInvoice = isIC ? _cnApplicationService.GetAllInvoiceByEntityId(companyId, creditNote.EntityId, creditNote.DocCurrency, applicationDate) : _cnApplicationService.GetAllCreditNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                                List<DebitNoteCompact> lstDebitNote = isIC ? _cnApplicationService.GetAllDebitNoteByEntityId(companyId, creditNote.EntityId, creditNote.DocCurrency, applicationDate) : _cnApplicationService.GetAllDebitNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                                //lstComapny = null;
                                //lstServiceEntityIds = new List<long?>();
                                //if (lstInvoice.Any())
                                //    lstServiceEntityIds.AddRange(lstInvoice.Select(c => c.ServiceCompanyId).ToList());
                                //if (lstDebitNote.Any())
                                //    lstServiceEntityIds.AddRange(lstDebitNote.Select(c => c.ServiceCompanyId).ToList());
                                //if (lstServiceEntityIds.Any())
                                //    lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);
                                if (lstInvoice.Any())
                                {
                                    lstSubCompanies = _masterService.GetAllSubCompanies(lstInvoice.Select(a => a.ServiceCompanyId).ToList(), username, companyId);
                                    lstComp = lstCompanies1.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                                }
                                foreach (InvoiceCompact detail in isIC ? lstInvoice.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstInvoice)
                                {
                                    var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                    if (d == null)
                                    {
                                        CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                        detailModel.DocNo = detail.DocNo;
                                        detailModel.DocType = detail.DocType;
                                        detailModel.DocumentId = detail.Id;
                                        detailModel.DocDate = detail.DocDate;
                                        detailModel.DocAmount = detail.GrandTotal;
                                        detailModel.DocCurrency = detail.DocCurrency;
                                        detailModel.BalanceAmount = detail.BalanceAmount;
                                        detailModel.Nature = detail.Nature;
                                        detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                                        //detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                                        //detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                                        detailModel.ServiceEntityId = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(e => e.Key).FirstOrDefault() : detail.ServiceCompanyId;
                                        detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                                        detailModel.ServEntityName = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(e => e.Value).FirstOrDefault() : string.Empty;
                                        detailModel.DocState = detail.DocumentState;
                                        detailModel.SystemReferenceNumber = detail.DocNo;
                                        detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                                        lstDetailModel.Add(detailModel);
                                    }
                                }
                                //if (lstDNs.Any())
                                //    lstServiceEntityIds.AddRange(lstDNs.Select(c => c.ServiceCompanyId).ToList());
                                if (lstDebitNote.Any())
                                {
                                    lstSubCompanies = _masterService.GetAllSubCompanies(lstDebitNote.Select(a => a.ServiceCompanyId).ToList(), username, companyId);
                                    lstComp = lstCompanies1.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                                }
                                foreach (DebitNoteCompact detail in isIC ? lstDebitNote.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstDebitNote)
                                {
                                    var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                    if (d == null)
                                    {
                                        CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                        detailModel.DocNo = detail.DocNo;
                                        detailModel.DocType = DocTypeConstants.DebitNote;
                                        detailModel.DocumentId = detail.Id;
                                        detailModel.DocDate = detail.DocDate;
                                        detailModel.DocAmount = detail.GrandTotal;
                                        detailModel.DocCurrency = detail.DocCurrency;
                                        detailModel.BalanceAmount = detail.BalanceAmount;
                                        detailModel.Nature = detail.Nature;
                                        detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                                        //detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                                        //detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                                        detailModel.ServiceEntityId = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(e => e.Key).FirstOrDefault() : detail.ServiceCompanyId;
                                        detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                                        detailModel.ServEntityName = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(e => e.Value).FirstOrDefault() : string.Empty;
                                        detailModel.SystemReferenceNumber = detail.DocNo;
                                        detailModel.DocState = detail.DocumentState;
                                        //detailModel.SegmentCategory1 = detail.SegmentCategory1;
                                        //detailModel.SegmentCategory2 = detail.SegmentCategory2;
                                        detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;

                                        lstDetailModel.Add(detailModel);
                                    }
                                }
                                #endregion Normal CN_App_Outstanding_ForEdit
                            }
                            else
                            {
                                #region  Interco CN_App_Outstanding_ForEdit

                                #region Invoice
                                List<InvoiceCompact> lstInvoice = /*isICActive == true ?*/ /*_cnApplicationService.GetAllInvoiceByEntityId(companyId, creditNote.EntityId, creditNote.DocCurrency, applicationDate) :*/ _cnApplicationService.GetAllCreditNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);

                                lstComapny = null;
                                lstServiceEntityIds = new List<long?>();
                                if (lstInvoice.Any())
                                    lstServiceEntityIds.AddRange(lstInvoice.Select(c => c.ServiceCompanyId).ToList());
                                if (lstServiceEntityIds.Any())
                                {
                                    lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);
                                    lstSubCompanies = _masterService.GetAllSubCompanies(lstServiceEntityIds, username, companyId);
                                    lstComp = lstComapny.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                                }
                                foreach (InvoiceCompact detail in lstInvoice)
                                {
                                    var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                    if (d == null)
                                    {
                                        CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                        detailModel.Id = Guid.NewGuid();
                                        detailModel.DocNo = detail.DocNo;
                                        detailModel.DocType = detail.DocType;
                                        detailModel.DocumentId = detail.Id;
                                        detailModel.DocDate = detail.DocDate;
                                        detailModel.DocAmount = detail.GrandTotal;
                                        detailModel.DocCurrency = detail.DocCurrency;
                                        detailModel.BalanceAmount = detail.BalanceAmount;
                                        detailModel.Nature = detail.Nature;
                                        detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                                        //detailModel.ServiceEntityId = creditNote.ServiceCompanyId;
                                        detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                                        detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                                        detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                                        detailModel.DocState = detail.DocumentState;
                                        detailModel.SystemReferenceNumber = detail.DocNo;
                                        detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                                        lstDetailModel.Add(detailModel);
                                    }
                                }
                                #endregion Invoice

                                #region Debit Note
                                List<DebitNoteCompact> lstOfDebitnotes = _cnApplicationService.GetAllDebitNotesById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                                lstComapny = null;
                                lstServiceEntityIds = new List<long?>();
                                if (lstOfDebitnotes.Any())
                                    lstServiceEntityIds.AddRange(lstOfDebitnotes.Select(c => c.ServiceCompanyId).ToList());
                                if (lstServiceEntityIds.Any())
                                {
                                    lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);
                                    lstSubCompanies = _masterService.GetAllSubCompanies(lstServiceEntityIds, username, companyId);
                                    lstComp = lstComapny.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                                }
                                foreach (DebitNoteCompact detail in lstOfDebitnotes)
                                {
                                    var d = lstDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                                    if (d == null)
                                    {
                                        CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                                        detailModel.Id = Guid.NewGuid();
                                        detailModel.DocNo = detail.DocNo;
                                        detailModel.DocType = detail.DocSubType;
                                        detailModel.DocumentId = detail.Id;
                                        detailModel.DocDate = detail.DocDate;
                                        detailModel.DocAmount = detail.GrandTotal;
                                        detailModel.DocCurrency = detail.DocCurrency;
                                        detailModel.BalanceAmount = detail.BalanceAmount;
                                        detailModel.Nature = detail.Nature;
                                        detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                                        detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                                        detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                                        detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                                        detailModel.DocState = detail.DocumentState;
                                        detailModel.SystemReferenceNumber = detail.DocNo;
                                        detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                                        lstDetailModel.Add(detailModel);
                                    }
                                }


                                #endregion

                                #endregion Interco CN_App_Outstanding_ForEdit
                            }

                        }
                        if (CNApplication.IsRevExcess != true)
                            CNAModel.CreditNoteApplicationDetailModels = lstDetailModel.OrderBy(c => c.DocDate).ThenBy(d => d.SystemReferenceNumber).ToList();
                        else
                            CNAModel.ReverseExcessModels = CNApplication.CreditNoteApplicationDetails.Select(d => new ReverseExcessModel()
                            {
                                Id = d.Id,
                                CompanyId = companyId,
                                DocAmount = d.CreditAmount,
                                DocTaxAmount = d.TaxAmount,
                                DocTotalAmount = d.TotalAmount,
                                TaxId = d.TaxId,
                                TaxRate = d.TaxRate,
                                TaxIdCode = d.TaxIdCode,
                                RecOrder = d.RecOrder,
                                COAId = d.COAId,
                                ItemDescription = d.DocDescription
                            }).OrderBy(d => d.RecOrder).ToList();
                    }
                }
                else
                {
                    CreditNoteApplication CNA = _cnApplicationService.GetCreditNoteByCompanyId(companyId);
                    CNAModel.Id = Guid.NewGuid();
                    CNAModel.EntityName = _masterService.GetEntityName(creditNote.EntityId);
                    CNAModel.CompanyId = companyId;
                    CNAModel.InvoiceId = creditNoteId;
                    InvoiceCompact invoice = _cnApplicationService.GetinvoiceById(creditNoteId);
                    CNAModel.DocCurrency = invoice.DocCurrency;
                    CNAModel.DocNo = invoice.DocNo;
                    CNAModel.DocDate = creditNote.DocDate;
                    CNAModel.CreditNoteAmount = invoice.GrandTotal;
                    CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
                    CNAModel.CreditNoteBalanceAmount = invoice.BalanceAmount;
                    CNAModel.CreditAmount = invoice.GrandTotal;
                    CNAModel.CreditNoteApplicationNumber = invoice.DocNo;
                    CNAModel.ExchangeRate = invoice.ExchangeRate;
                    CNAModel.GSTExchangeRate = invoice.GSTExchangeRate;
                    CNAModel.Remarks = invoice.DocDescription;
                    //CNAModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
                    CNAModel.NoSupportingDocument = false;
                    if (invoice.Nature != "Interco")
                    {
                        #region Normal CN_Application_Outstandings
                        List<InvoiceCompact> lstInvoice = !isIC ? _cnApplicationService.GetAllCreditNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate) : _cnApplicationService.GetAllInvoiceByEntityId(companyId, creditNote.EntityId, creditNote.DocCurrency, applicationDate);
                        List<DebitNoteCompact> lstDebitNote = !isIC ? _cnApplicationService.GetAllDebitNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate) : _cnApplicationService.GetAllDebitNoteByEntityId(companyId, creditNote.EntityId, creditNote.DocCurrency, applicationDate);
                        if (lstInvoice.Any())
                            lstServiceEntityIds.AddRange(lstInvoice.Select(c => c.ServiceCompanyId).ToList());
                        if (lstDebitNote.Any())
                            lstServiceEntityIds.AddRange(lstDebitNote.Select(c => c.ServiceCompanyId).ToList());
                        if (lstServiceEntityIds.Any())
                            lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);
                        if (lstInvoice.Any())
                        {
                            lstSubCompanies = _masterService.GetAllSubCompanies(lstInvoice.Select(a => a.ServiceCompanyId).ToList(), username, companyId);
                            lstComp = lstCompanies1.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                        }
                        List<CreditNoteApplicationDetailModel> lstCredtNoteDetail = new List<CreditNoteApplicationDetailModel>();
                        foreach (InvoiceCompact detail in isIC ? lstInvoice.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstInvoice)
                        {
                            CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                            detailModel.DocNo = detail.DocNo;
                            detailModel.DocType = detail.DocType;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocDate = detail.DocDate;
                            detailModel.DocAmount = detail.GrandTotal;
                            detailModel.DocCurrency = detail.DocCurrency;
                            detailModel.BalanceAmount = detail.BalanceAmount;
                            detailModel.Nature = detail.Nature;
                            detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                            //detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                            detailModel.ServiceEntityId = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(d => d.Key).FirstOrDefault() : detail.ServiceCompanyId;
                            detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                            detailModel.ServEntityName = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(d => d.Value).FirstOrDefault() : string.Empty;
                            //detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                            detailModel.DocState = detail.DocumentState;
                            detailModel.SystemReferenceNumber = detail.DocNo;
                            detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                            lstCredtNoteDetail.Add(detailModel);
                        }

                        if (lstDebitNote.Any())
                        {
                            lstSubCompanies = _masterService.GetAllSubCompanies(lstDebitNote.Select(a => a.ServiceCompanyId).ToList(), username, companyId);
                            lstComp = lstCompanies1.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value);
                        }
                        foreach (DebitNoteCompact detail in isIC ? lstDebitNote.Where(a => lstCompanies1.Keys.Contains(a.ServiceCompanyId.Value)) : lstDebitNote)
                        {
                            CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                            detailModel.DocNo = detail.DocNo;
                            detailModel.DocType = DocTypeConstants.DebitNote;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocDate = detail.DocDate;
                            detailModel.DocAmount = detail.GrandTotal;
                            detailModel.DocCurrency = detail.DocCurrency;
                            //detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                            detailModel.ServiceEntityId = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(d => d.Key).FirstOrDefault() : detail.ServiceCompanyId;
                            detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                            detailModel.ServEntityName = isIC ? lstCompanies1.Where(c => c.Key == detail.ServiceCompanyId).Select(d => d.Value).FirstOrDefault() : string.Empty;
                            detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                            //detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                            detailModel.DocState = detail.DocumentState;
                            detailModel.BalanceAmount = detail.BalanceAmount;
                            detailModel.Nature = detail.Nature;
                            detailModel.SystemReferenceNumber = detail.DocNo;
                            detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                            lstCredtNoteDetail.Add(detailModel);
                        }
                        CNAModel.CreditNoteApplicationDetailModels = lstCredtNoteDetail.OrderBy(x => x.DocDate).ThenBy(x => x.SystemReferenceNumber).ToList();

                        #endregion Normal CN_Application_Outstandings
                    }
                    else
                    {
                        #region Interco CN_Application_Outsatndings
                        #region Invoice
                        List<InvoiceCompact> lstInvoice = /*isICActive != true ?*/ _cnApplicationService.GetAllCreditNoteById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate) /*: _cnApplicationService.GetAllInvoiceByEntityId(companyId, creditNote.EntityId, creditNote.DocCurrency, applicationDate)*/;
                        if (lstInvoice.Any())
                            lstServiceEntityIds.AddRange(lstInvoice.Select(c => c.ServiceCompanyId).ToList());
                        if (lstServiceEntityIds.Any())
                        {
                            lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);
                            lstSubCompanies = _masterService.GetAllSubCompanies(lstServiceEntityIds, username, companyId);
                            lstComp = lstComapny.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value); ;
                        }
                        List<CreditNoteApplicationDetailModel> lstCredtNoteDetail = new List<CreditNoteApplicationDetailModel>();
                        foreach (InvoiceCompact detail in lstInvoice)
                        {
                            CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                            detailModel.Id = Guid.NewGuid();
                            detailModel.DocNo = detail.DocNo;
                            detailModel.DocType = detail.DocType;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocDate = detail.DocDate;
                            detailModel.DocAmount = detail.GrandTotal;
                            detailModel.DocCurrency = detail.DocCurrency;
                            detailModel.BalanceAmount = detail.BalanceAmount;
                            detailModel.Nature = detail.Nature;
                            detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                            //detailModel.ServiceEntityId = creditNote.ServiceCompanyId;
                            detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                            detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                            detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                            detailModel.DocState = detail.DocumentState;
                            detailModel.SystemReferenceNumber = detail.DocNo;
                            detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                            lstCredtNoteDetail.Add(detailModel);
                        }
                        //CNAModel.CreditNoteApplicationDetailModels = lstCredtNoteDetail.OrderBy(x => x.DocDate).ThenBy(x => x.SystemReferenceNumber).ToList();
                        #endregion Invoice

                        #region  Debitnote
                        List<DebitNoteCompact> lstDebitnotes = _cnApplicationService.GetAllDebitNotesById(companyId, creditNote.EntityId, creditNote.DocCurrency, creditNote.ServiceCompanyId.Value, applicationDate);
                        if (lstDebitnotes.Any())
                            lstServiceEntityIds.AddRange(lstDebitnotes.Select(c => c.ServiceCompanyId).ToList());
                        if (lstServiceEntityIds.Any())
                        {
                            lstComapny = _masterService.GetAllCompaniesCode(lstServiceEntityIds);
                            lstSubCompanies = _masterService.GetAllSubCompanies(lstServiceEntityIds, username, companyId);
                            lstComp = lstComapny.Except(lstSubCompanies).ToDictionary(Id => Id.Key, Name => Name.Value); ;
                        }
                        foreach (DebitNoteCompact detail in lstDebitnotes)
                        {
                            CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();
                            detailModel.Id = Guid.NewGuid();
                            detailModel.DocNo = detail.DocNo;
                            detailModel.DocType = detail.DocSubType;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocDate = detail.DocDate;
                            detailModel.DocAmount = detail.GrandTotal;
                            detailModel.DocCurrency = detail.DocCurrency;
                            detailModel.BalanceAmount = detail.BalanceAmount;
                            detailModel.Nature = detail.Nature;
                            detailModel.COAId = detail.Nature == "Trade" ? lstCOAName.Where(c => c.Value == COANameConstants.AccountsReceivables).Select(c => c.Key).FirstOrDefault() : lstCOAName.Where(c => c.Value == COANameConstants.OtherReceivables).Select(c => c.Key).FirstOrDefault();
                            detailModel.ServiceEntityId = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Key).FirstOrDefault() : 0;
                            detailModel.ServEntityName = lstComapny != null ? lstComapny.Where(c => c.Key == detail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault() : string.Empty;
                            detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                            detailModel.DocState = detail.DocumentState;
                            detailModel.SystemReferenceNumber = detail.DocNo;
                            detailModel.BaseCurrencyExchangeRate = detail.ExchangeRate.Value;
                            lstCredtNoteDetail.Add(detailModel);
                        }
                        CNAModel.CreditNoteApplicationDetailModels = lstCredtNoteDetail.OrderBy(x => x.DocDate).ThenBy(x => x.SystemReferenceNumber).ToList();
                        #endregion  Debitnote
                        #endregion Interco CN_Application_Outsatndings


                    }

                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplication_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return CNAModel;
        }
        #endregion
        #region CreditnoteApplicationLu
        public CNAModelLu CreditNoteApplicationLU(long companyid, Guid CNAId, string connectionString)
        {
            CNAModelLu CNALu = new CNAModelLu();
            try
            {
                string query = null;
                CNALu.CompanyId = companyid;
                CreditNoteApplication CNA = _cnApplicationService.GetAllCreditNoteApplication(CNAId, companyid);
                List<CommonLookUps<string>> lstAlltax = new List<CommonLookUps<string>>();
                if (CNA.IsRevExcess == true)
                {
                    query = CommonCNAQuery(companyid, CNA.CreditNoteApplicationDate);
                    using (con = new SqlConnection(connectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                lstAlltax.Add(new CommonLookUps<string>
                                {
                                    TableName = dr["TABLENAME"].ToString(),
                                    Code = dr["CODE"].ToString(),
                                    Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0,
                                    Name = dr["NAME"].ToString(),
                                    RecOrder = dr["RECORDER"] != DBNull.Value ? Convert.ToInt32(dr["RECORDER"]) : (int?)null,
                                    TaxRate = dr["TAXRATE"] != DBNull.Value ? Convert.ToDouble(dr["TAXRATE"]) : (double?)null,
                                    TaxType = dr["TAXTYPE"].ToString(),
                                    TaxCode = dr["TXCODE"].ToString(),
                                    Status = (RecordStatusEnum)dr["STATUS"],
                                    IsInterCo = dr["IsInterCo"] != DBNull.Value ? Convert.ToBoolean(dr["IsInterCo"]) : (bool?)null
                                });
                            }
                        }
                        con.Close();
                    }
                    CNALu.TaxCodeLU = lstAlltax.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                    {
                        Id = x.Id,
                        Code = x.TaxCode,
                        Name = x.Name,
                        TaxRate = x.TaxRate,
                        TaxType = x.TaxType,
                        Status = x.Status,
                        TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.TaxCode,
                        IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                    }).OrderBy(c => c.Code).ToList();
                    if (CNA != null)
                    {
                        var lsttax = lstAlltax.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Inactive).Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.TaxCode,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.TaxCode,
                            IsInterCoBillingTaxCode = x.TaxCode == "NA" ? true : x.IsInterCo
                        }).OrderBy(c => c.Code).ToList();
                        List<long?> taxIdss = CNA.CreditNoteApplicationDetails.Select(x => x.TaxId).ToList();
                        if (CNALu.TaxCodeLU.Any())
                            taxIdss = taxIdss.Except(CNALu.TaxCodeLU.Select(d => d.Id)).ToList();
                        if (taxIdss.Any())
                        {
                            var lstTax = lsttax.Where(c => taxIdss.Contains(c.Id)).Select(x => new TaxCodeLookUp<string>()
                            {
                                Id = x.Id,
                                Code = x.Code,
                                Name = x.Name,
                                TaxRate = x.TaxRate,
                                TaxType = x.TaxType,
                                Status = x.Status,
                                TaxIdCode = x.Code != "NA" ? x.Code + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") : x.Code,
                                IsInterCoBillingTaxCode = x.Code == "NA" ? true : x.IsInterCoBillingTaxCode
                            }).OrderBy(c => c.Code).ToList();
                            CNALu.TaxCodeLU.AddRange(lstTax);
                            CNALu.TaxCodeLU = CNALu.TaxCodeLU.OrderBy(c => c.Code).ToList();
                        }
                    }

                }
                return CNALu;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private static string CommonCNAQuery(long companyid, DateTime date)
        {
            return $"SELECT distinct Tax.Code,'TAXCODE' as TABLENAME,Tax.Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Tax.Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY,Case When TaxMapDetail.CustTaxId=Tax.Id Then 1 Else 0 END as IsInterCo FROM Bean.TaxCodeMapping TaxMap Join Bean.TaxCodeMappingDetail TaxMapDetail on TaxMap.Id=TaxMapDetail.TaxCodeMappingId and TaxMap.CompanyId={companyid} Right Join Bean.TaxCode Tax on Tax.Id=TaxMapDetail.CustTaxId where Tax.CompanyId= {companyid}  and Tax.Status<3 and EffectiveFrom<='{date}' and (EffectiveTo>='{date}' OR EffectiveTo is null)";
        }
        #endregion
        #region SaveCall
        public CreditNoteApplication SaveCreditNoteApplication(CreditNoteApplicationModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(InvoiceLoggingValidation.CNAApplicationService, "ObjectSave", AdditionalInfo);
            //Guid transationId = Guid.NewGuid();
            DateTime? oldAppDate = null;
            string oldDocState = null;
            decimal oldCreditAmount = 0;
            InvoiceCompact creditNote = _cnApplicationService.GetinvoiceById(TObject.InvoiceId);
            ValidateCreditNoteApplication(creditNote, TObject);
            if (TObject.CreditAmount > TObject.CreditNoteBalanceAmount)
                throw new InvalidOperationException(CreditNoteValidation.Credit_Amount_should_be_less_than_or_equal_to_Remaining_Amount);

            #region Interco_Validation_on_ServiceEntitties
            //int entityCount = 0;
            bool isICActive = TObject.CreditNoteApplicationDetailModels.Any(c => c.ServiceEntityId != creditNote.ServiceCompanyId);
            List<long?> lstEntityIds = null;
            if (isICActive && creditNote.Nature != DocTypeConstants.Interco)
            {
               int entityCount = TObject.CreditNoteApplicationDetailModels.Where(d => d.CreditAmount != 0).Select(c => c.ServiceEntityId).Distinct().ToList().Count();
                lstEntityIds = TObject.CreditNoteApplicationDetailModels.Where(d => d.CreditAmount != 0).Select(c => c.ServiceEntityId).Distinct().ToList();
                lstEntityIds.Add(creditNote.ServiceCompanyId);
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
                        throw new InvalidOperationException(CommonConstant.The_State_of_the_service_entity_had_been_changed);
                }
            }
            #endregion Interco_Validation_on_ServiceEntitties

            CreditNoteApplication application = null;
            if (creditNote.Nature == DocTypeConstants.Interco)
            {
                FillDocumentAndDetailType(TObject, ConnectionString, creditNote.ExchangeRate.Value, creditNote.ServiceCompanyId);
                application = _cnApplicationService.GetCreditNoteByIds(TObject.Id);
            }
            else
            {
                application = _cnApplicationService.GetCreditNoteByIds(TObject.Id);
                bool isNew = false;
                if (application == null)
                {
                    application = new CreditNoteApplication();
                    isNew = true;
                    oldAppDate = TObject.CreditNoteApplicationDate;
                }
                else
                {
                    if (application.Status == CreditNoteApplicationStatus.Void)
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);
                    creditNote.BalanceAmount += application.CreditAmount;
                    oldAppDate = application.CreditNoteApplicationDate;
                    oldCreditAmount = application.CreditAmount;
                }
                oldDocState = creditNote.DocumentState;
                application.CreditNoteApplicationDate = TObject.CreditNoteApplicationDate;
                application.ExchangeRate = creditNote.ExchangeRate;

                if (TObject.IsNoSupportingDocument != null)
                    application.IsNoSupportingDocument = TObject.IsNoSupportingDocument.Value;
                if (application.IsNoSupportingDocument == true)
                    application.IsNoSupportingDocumentActivated = TObject.NoSupportingDocument == true ? true : false;
                application.CreditAmount = TObject.CreditAmount;
                application.Remarks = TObject.Remarks;
                application.DocumentId = TObject.DocumentId;
                application.Status = CreditNoteApplicationStatus.Posted;
                application.IsRevExcess = TObject.IsRevExcess;
                List<DocumentHistoryModel> lstDocuments = new List<DocumentHistoryModel>();
                Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
                decimal roundingAmount = 0;
                if (isNew)
                {
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteApplication_SaveCall_NewRequest_Message);
                    application.Id = TObject.Id;
                    application.InvoiceId = TObject.InvoiceId;
                    application.CompanyId = TObject.CompanyId;
                    application.ExchangeRate = creditNote.ExchangeRate;
                    if (TObject.IsRevExcess != true)
                        UpdateCreditNoteApplicationDetails(TObject, application, ConnectionString, TObject.Id, application.CreditNoteApplicationDate, lstDocuments, lstOfRoundingAmount);
                    else
                        UpdateCreditNoteApplicationRevExcessDetails(TObject, application, ConnectionString, lstDocuments);
                    application.CreditNoteApplicationNumber = GetNextApplicationNumber(TObject.InvoiceId);
                    application.UserCreated = TObject.UserCreated;
                    application.CreatedDate = DateTime.UtcNow;
                    application.CreditNoteApplicationDetails = application.CreditNoteApplicationDetails.Where(c => c.CreditAmount != 0).ToList();
                    application.ObjectState = ObjectState.Added;
                    _cnApplicationService.Insert(application);
                    //JVModel jvm = new JVModel();
                    ////application.CreditNoteApplicationDetails = application.CreditNoteApplicationDetails.Where(c => c.CreditAmount != 0).ToList();      
                    //FillCreditNoteJournal(jvm, application, true, TObject.IsGstSettings);
                    //SaveInvoice1(jvm);
                }
                else
                {
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteApplication_SaveCall_UpdateRequest_Message);

                    //2 Tabs manupulation with delete/add operation
                    //string timeStamp = "0x" + string.Concat(Array.ConvertAll(application.Version, x => x.ToString("X2")));
                    //if (!timeStamp.Equals(TObject.Version))
                    //    throw new Exception(CommonConstant.Document_has_been_modified_outside);

                    if (TObject.IsRevExcess != true)
                        UpdateCreditNoteApplicationDetails(TObject, application, ConnectionString, TObject.Id, application.CreditNoteApplicationDate, lstDocuments, lstOfRoundingAmount);
                    else
                        UpdateCreditNoteApplicationRevExcessDetails(TObject, application, ConnectionString, lstDocuments);
                    application.ModifiedBy = TObject.ModifiedBy;
                    application.ModifiedDate = DateTime.UtcNow;
                    application.ObjectState = ObjectState.Modified;
                    _cnApplicationService.Update(application);
                    isNew = false;
                }
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(creditNote.Id, creditNote.CompanyId, TObject.Id, creditNote.DocType, DocTypeConstants.Application, "Posted", creditNote.DocCurrency, application.CreditAmount, application.CreditAmount, creditNote.ExchangeRate.Value, application.ModifiedBy != null ? application.ModifiedBy : application.UserCreated, application.Remarks, application.CreditNoteApplicationDate, application.CreditAmount, 0);
                    if (lstdocumet1.Any())
                        lstDocuments.AddRange(lstdocumet1);
                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet1, ConnectionString);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                #endregion Documentary History
                creditNote.BalanceAmount -= TObject.CreditAmount;
                if (creditNote.BalanceAmount == 0)
                {
                    creditNote.DocumentState = CreditNoteState.FullyApplied;
                    if (isNew)
                    {
                        //roundingAmount = Math.Round(application.CreditAmount * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)creditNote.BaseBalanceAmount;
                        //creditNote.BaseBalanceAmount = 0;
                        //if (roundingAmount != 0)
                        //    lstOfRoundingAmount.Add(creditNote.Id, roundingAmount);
                        if (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)
                            roundingAmount = ((creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0) ? (decimal)creditNote.RoundingAmount : 0);
                        else
                            roundingAmount = Math.Round(application.CreditAmount * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)creditNote.BaseBalanceAmount;

                        creditNote.BaseBalanceAmount = 0;
                        if (roundingAmount != 0)
                            lstOfRoundingAmount.Add(creditNote.Id, roundingAmount);
                        creditNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)) ? creditNote.RoundingAmount - roundingAmount : 0;
                        application.RoundingAmount = roundingAmount;
                    }
                    else
                    {
                        if (oldCreditAmount != application.CreditAmount)
                        {
                            //roundingAmount = (((decimal)creditNote.BaseGrandTotal - (Math.Round(Math.Abs(oldCreditAmount - application.CreditAmount) * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) + (decimal)creditNote.BaseBalanceAmount) - (decimal)creditNote.BaseGrandTotal;
                            //creditNote.BaseBalanceAmount = 0;
                            //if (roundingAmount != 0)
                            //    lstOfRoundingAmount.Add(creditNote.Id, roundingAmount);
                            if (application.CreditAmount == creditNote.GrandTotal)
                            {
                                roundingAmount = Math.Round(Math.Abs(application.CreditAmount) * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)creditNote.BaseGrandTotal);
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(creditNote.Id, roundingAmount);
                                application.RoundingAmount = roundingAmount;
                                creditNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)) ? creditNote.RoundingAmount - roundingAmount : 0;
                                creditNote.BaseBalanceAmount = 0;
                            }
                            else if (application.RoundingAmount != null && application.RoundingAmount != 0)
                            {
                                creditNote.BaseBalanceAmount = 0;
                                lstOfRoundingAmount.Add(creditNote.Id, application.RoundingAmount.Value);
                                //newRoundingAmount = detailRoundingAmount.Value;
                                roundingAmount = application.RoundingAmount.Value;
                            }
                            else
                            {
                                if (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)
                                    roundingAmount = ((creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0) ? (decimal)creditNote.RoundingAmount : 0);
                                else
                                    roundingAmount = Math.Round(Math.Abs(application.CreditAmount - oldCreditAmount) * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)creditNote.BaseBalanceAmount);

                                creditNote.BaseBalanceAmount = 0;
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(creditNote.Id, roundingAmount);
                                application.RoundingAmount = roundingAmount;
                                creditNote.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (creditNote.RoundingAmount != null && creditNote.RoundingAmount != 0)) ? creditNote.RoundingAmount - roundingAmount : 0;
                            }
                        }
                        else
                        {
                            if (application.RoundingAmount != null && application.RoundingAmount != 0)
                            {
                                creditNote.BaseBalanceAmount = 0;
                                lstOfRoundingAmount.Add(creditNote.Id, application.RoundingAmount.Value);
                                roundingAmount = application.RoundingAmount.Value;
                                //newRoundingAmount = detailRoundingAmount.Value;
                            }
                        }
                    }
                }
                else
                {
                    creditNote.DocumentState = CreditNoteState.PartialApplied;
                    if (isNew)
                        creditNote.BaseBalanceAmount -= Math.Round(application.CreditAmount * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                    else
                    if (oldCreditAmount != application.CreditAmount)
                    {
                        creditNote.BaseBalanceAmount = oldCreditAmount > application.CreditAmount ? creditNote.BaseBalanceAmount + (Math.Round(Math.Abs(oldCreditAmount - application.CreditAmount) * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : creditNote.BaseBalanceAmount - (Math.Round(Math.Abs(oldCreditAmount - application.CreditAmount) * ((creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));


                        if (oldDocState == CreditNoteState.FullyApplied)
                        {
                            creditNote.RoundingAmount = ((application.RoundingAmount != null && application.RoundingAmount != 0) && (oldDocState != creditNote.DocumentState)) ? application.RoundingAmount : creditNote.RoundingAmount;
                            application.RoundingAmount = ((application.RoundingAmount != null && application.RoundingAmount != 0) && (oldDocState != creditNote.DocumentState)) ? 0 : application.RoundingAmount;
                        }

                    }

                }

                creditNote.ModifiedBy = InvoiceConstants.System;
                creditNote.ModifiedDate = DateTime.UtcNow;
                creditNote.ObjectState = ObjectState.Modified;

                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, creditNote.CompanyId, creditNote.Id, creditNote.DocType, creditNote.DocSubType, creditNote.DocumentState, creditNote.DocCurrency, creditNote.GrandTotal, creditNote.BalanceAmount, creditNote.ExchangeRate.Value, creditNote.ModifiedBy != null ? creditNote.ModifiedBy : creditNote.UserCreated, creditNote.DocDescription, application.CreditNoteApplicationDate, application.CreditAmount < 0 ? application.CreditAmount : -application.CreditAmount, roundingAmount);
                    if (lstdocumet.Any())
                        lstDocuments.AddRange(lstdocumet);
                    if (lstDocuments.Any())
                        AppaWorld.Bean.Common.SaveDocumentHistory(lstDocuments, ConnectionString);

                    if (oldAppDate != TObject.CreditNoteApplicationDate)
                    {

                        string query = $"Update Bean.DocumentHistory Set PostingDate='{String.Format("{0:MM/dd/yyyy}", application.CreditNoteApplicationDate)}' where TransactionId='{application.Id}' and CompanyId={application.CompanyId} and TransactionId<>DocumentId and doctype in ('Invoice','Debit Note') and AgingState is null;";
                        SqlConnection con = null;
                        using (con = new SqlConnection(ConnectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            SqlCommand cmd = new SqlCommand(query, con);
                            cmd.ExecuteNonQuery();
                            if (con.State == ConnectionState.Open)
                                con.Close();
                        }

                    }

                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                #endregion Documentary History
                //_invoiceEntityService.Update(creditNote);
                //var updateState = _journalService.GetJournal(TObject.CompanyId, TObject.InvoiceId);

                query = $"Select * from Bean.Journal J where J.CompanyId={TObject.CompanyId} and J.DocumentId='{TObject.InvoiceId}'";
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand(query, con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Close();
                        query = $"Update Bean.Journal set BalanceAmount={creditNote.BalanceAmount},DocumentState='{creditNote.DocumentState}',ModifiedBy='{InvoiceConstants.System}',ModifiedDate='{String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)}' where CompanyId={TObject.CompanyId} and DocumentId='{TObject.InvoiceId}' ";
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_SaveCreditNoteApplication_SaveCall_SuccessFully_Message);

                if (creditNote != null)
                {
                    if (creditNote.DocSubType == DocTypeConstants.OpeningBalance && creditNote.DocType == DocTypeConstants.CreditNote && creditNote.DocSubType == DocTypeConstants.OpeningBalance)
                    {
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Enter_Into_Update_OB_LineItem);
                        SqlConnection con = new SqlConnection(ConnectionString);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                        oBcmd.CommandType = CommandType.StoredProcedure;
                        oBcmd.Parameters.AddWithValue("@OBId", creditNote.OpeningBalanceId);
                        oBcmd.Parameters.AddWithValue("@DocumentId", creditNote.Id);
                        oBcmd.Parameters.AddWithValue("@CompanyId", creditNote.CompanyId);
                        oBcmd.Parameters.AddWithValue("@IsEqual", creditNote.BalanceAmount == creditNote.GrandTotal ? true : false);
                        int res = oBcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                try
                {
                    _unitOfWork.SaveChanges();

                    JVModel jvm = new JVModel();
                    IDictionary<long, long?> lstIC = null;
                    IDictionary<long, long?> lstICs = null;
                    long? ICCoaId = 0;
                    if (TObject.IsICActive == true && TObject.IsRevExcess != true)
                    {
                        List<long?> serviceEntityIds = null;
                        if (application.CreditNoteApplicationDetails.Any())
                        {
                            //serviceEntityIds = application.CreditNoteApplicationDetails.Where(d => d.ServiceEntityId != creditNote.ServiceCompanyId).Select(c => c.ServiceEntityId).ToList();
                            serviceEntityIds = application.CreditNoteApplicationDetails.GroupBy(d => d.ServiceEntityId).Select(c => c.Key).ToList();
                            if (serviceEntityIds.Any())
                            {
                                serviceEntityIds.Add(creditNote.ServiceCompanyId);
                                lstICs = _masterService.GetAllICAccount(serviceEntityIds.Distinct().ToList());
                                lstIC = lstICs.Where(c => c.Value != creditNote.ServiceCompanyId).ToDictionary(d => d.Key, value => value.Value);
                                ICCoaId = lstICs.Where(c => c.Value == creditNote.ServiceCompanyId).Select(c => c.Key).FirstOrDefault();
                            }
                        }
                    }
                    List<InvoiceCompact> lstAllInvoice = null;
                    List<DebitNoteCompact> lstAllDN = null;
                    long exchangeGainLossId = _masterService.GetChartOfAccountByName(COANameConstants.ExchangeGainLossRealised, creditNote.CompanyId);
                    FillCreditNoteJournal(jvm, application, isNew, TObject.IsGstSettings, lstIC, exchangeGainLossId, out lstAllInvoice, out lstAllDN, TObject.IsOffset, lstOfRoundingAmount);
                    jvm.IsFirst = true;
                    SaveInvoice1(jvm);
                    if (TObject.IsICActive == true && TObject.CreditNoteApplicationDetailModels.Where(a => a.CreditAmount > 0).Select(c => c.ServiceEntityId).GroupBy(d => d.Value).Count() >= 1 && TObject.IsRevExcess != true)
                    {
                        foreach (CreditNoteApplicationDetailModel detail in TObject.CreditNoteApplicationDetailModels.Where(c => c.ServiceEntityId != creditNote.ServiceCompanyId && c.CreditAmount > 0).GroupBy(c => c.ServiceEntityId).Select(c => c.FirstOrDefault()).ToList())
                        {
                            FillCreditNoteICJournal(jvm, application, detail, isNew, lstIC, exchangeGainLossId, lstAllInvoice, lstAllDN, TObject, ICCoaId, lstOfRoundingAmount);
                            jvm.IsFirst = false;
                            SaveInvoice1(jvm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                    throw ex;
                }
            }
            return application;
        }
        #endregion SaveCall

        #region SaveCNApplicationVoid
        public CreditNoteApplication SaveCreditNoteApplicationVoid(DocumentResetModel TObject, string ConnectionString)
        {
            //Guid transationId = Guid.NewGuid();
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_Request_Message);
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_masterService.ValidateFinancialOpenPeriod(TObject.ResetDate.Value, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CreditNoteValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_masterService.ValidateFinancialLockPeriodPassword(TObject.ResetDate.Value, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CreditNoteValidation.Invalid_Financial_Period_Lock_Password);
                }
            }
            InvoiceCompact creditNote = _cnApplicationService.GetCreditNoteByCompanyIdAndId(TObject.CompanyId, TObject.InvoiceId);
            if (creditNote == null)
            {
                throw new Exception(CreditNoteValidation.Invalid_CreditNote);
            }

            CreditNoteApplication allocation = _cnApplicationService.GetAllCreditNote(TObject.InvoiceId, TObject.Id, TObject.CompanyId);
            List<DocumentHistoryModel> lstDocuments = new List<DocumentHistoryModel>();
            Dictionary<Guid, decimal> lstOfRoundingAmount = new Dictionary<Guid, decimal>();
            decimal roundingAmount = 0;
            if (allocation != null)
            {
                //2 Tabs manupulation with delete/add operation
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(allocation.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);


                if (allocation.ClearCount != null && allocation.ClearCount > 0)
                    throw new Exception(CommonConstant.The_State_of_the_transaction_has_been_changed_by_Clering_Bank_Reconciliation_CannotSave_The_Transaction);

                allocation.Status = CreditNoteApplicationStatus.Void;
                allocation.CreditNoteApplicationResetDate = TObject.ResetDate;
                allocation.CreditNoteApplicationNumber = allocation.CreditNoteApplicationNumber + "-V";
                allocation.ModifiedDate = DateTime.UtcNow;
                allocation.ModifiedBy = TObject.ModifiedBy;
                allocation.ObjectState = ObjectState.Modified;
                _cnApplicationService.Update(allocation);
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(creditNote.Id, creditNote.CompanyId, TObject.Id, creditNote.DocType, DocTypeConstants.Application, "Void", creditNote.DocCurrency, allocation.CreditAmount, allocation.CreditAmount, creditNote.ExchangeRate.Value, allocation.ModifiedBy != null ? allocation.ModifiedBy : allocation.UserCreated, creditNote.Remarks, null, 0, 0);
                    if (lstdocumet1.Any())
                        lstDocuments.AddRange(lstdocumet1);
                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet1, ConnectionString);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                #endregion Documentary History
                //var updateJournal = _journalService.GetJournal(TObject.CompanyId, TObject.Id);
                //if (updateJournal != null)
                //{
                //    updateJournal.Status = CreditNoteApplicationStatus.Reset;
                //    updateJournal.DocumentState = "Reset";
                //    _journalService.Update(updateJournal);
                //}
                List<InvoiceCompact> lstInvoice = _cnApplicationService.GetAllDDByInvoiceId(allocation.CreditNoteApplicationDetails.Select(c => c.DocumentId).ToList());
                List<DebitNoteCompact> lstDebitNote = _cnApplicationService.GetDDByDebitNoteId(allocation.CreditNoteApplicationDetails.Select(c => c.DocumentId).ToList());
                decimal? roundAmount = 0;
                foreach (CreditNoteApplicationDetail detail in allocation.CreditNoteApplicationDetails)
                    UpdateDocumentState(detail.DocumentId, detail.DocumentType, -detail.CreditAmount, ConnectionString, lstInvoice, lstDebitNote, TObject.Id, null, lstDocuments, true, 0, lstOfRoundingAmount, false, 0, 0, detail.RoundingAmount, out roundAmount);

                creditNote.BalanceAmount += allocation.CreditAmount;

                //if (creditNote.BalanceAmount == 0)
                //    creditNote.DocumentState = CreditNoteState.FullyApplied;
                //else
                //    creditNote.DocumentState = CreditNoteState.PartialApplied;


                if (creditNote.BalanceAmount == 0)
                    creditNote.DocumentState = CreditNoteState.FullyApplied;
                else if (creditNote.BalanceAmount > 0)
                {
                    creditNote.DocumentState = CreditNoteState.PartialApplied;
                    //roundingAmount = (Math.Round((creditNote.GrandTotal - allocation.CreditAmount) * (creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((allocation.CreditAmount * (creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)creditNote.BaseGrandTotal;
                    creditNote.BaseBalanceAmount += (Math.Round((allocation.CreditAmount * (creditNote.ExchangeRate != null ? (decimal)creditNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                }
                else
                {
                    throw new Exception(String.Format("CreditNote ({0}) Balance Amount is becoming negative", creditNote.DocNo));
                }
                if (creditNote.GrandTotal == creditNote.BalanceAmount)
                {
                    creditNote.DocumentState = CreditNoteState.NotApplied;
                    creditNote.BaseBalanceAmount = creditNote.BaseGrandTotal;
                }
                creditNote.RoundingAmount += (allocation.RoundingAmount != 0 && allocation != null) ? allocation.RoundingAmount : 0;
                creditNote.ModifiedBy = "System";
                creditNote.ModifiedDate = DateTime.UtcNow;
                creditNote.ObjectState = ObjectState.Modified;
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, creditNote.CompanyId, creditNote.Id, creditNote.DocType, creditNote.DocSubType, creditNote.DocumentState, creditNote.DocCurrency, creditNote.GrandTotal, creditNote.BalanceAmount, creditNote.ExchangeRate.Value, creditNote.ModifiedBy != null ? creditNote.ModifiedBy : creditNote.UserCreated, creditNote.Remarks, null, 0, 0);
                    if (lstdocumet1.Any())
                        lstDocuments.AddRange(lstdocumet1);
                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet1, ConnectionString);
                }
                catch (Exception ex)
                {

                }

                if (lstDocuments.Any())
                    AppaWorld.Bean.Common.SaveDocumentHistory(lstDocuments, ConnectionString);
                #endregion Documentary History
                //#region Update_Journal_Detail_Clearing_Status
                if (creditNote.IsOBInvoice != true && creditNote.DocSubType != DocTypeConstants.OpeningBalance)
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@companyId", TObject.CompanyId);
                    cmd.Parameters.AddWithValue("@documentId", creditNote.Id);
                    cmd.Parameters.AddWithValue("@docState", creditNote.DocumentState);
                    cmd.Parameters.AddWithValue("@balanceAmount", creditNote.BalanceAmount);
                    int count = cmd.ExecuteNonQuery();
                    con.Close();
                }
                //#endregion Update_Journal_Detail_Clearing_Status
            }
            else
            {
                throw new Exception(CreditNoteValidation.Invalid_Credit_Note_Application);
            }
            try
            {
                _unitOfWork.SaveChanges();
                if (creditNote != null)
                {
                    if (creditNote.DocSubType == DocTypeConstants.OpeningBalance && creditNote.DocType == DocTypeConstants.CreditNote && creditNote.DocSubType == DocTypeConstants.OpeningBalance)
                    {
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Enter_Into_Update_OB_LineItem);
                        SqlConnection con = new SqlConnection(ConnectionString);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", con);
                        oBcmd.CommandType = CommandType.StoredProcedure;
                        oBcmd.Parameters.AddWithValue("@OBId", creditNote.OpeningBalanceId);
                        oBcmd.Parameters.AddWithValue("@DocumentId", creditNote.Id);
                        oBcmd.Parameters.AddWithValue("@CompanyId", creditNote.CompanyId);
                        oBcmd.Parameters.AddWithValue("@IsEqual", creditNote.BalanceAmount == creditNote.GrandTotal ? true : false);
                        int res = oBcmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                if (creditNote.DocSubType != DocTypeConstants.Interco && creditNote.Nature != DocTypeConstants.Interco)
                {
                    JournalSaveModel tObject = new JournalSaveModel();
                    tObject.Id = TObject.Id;
                    tObject.CompanyId = TObject.CompanyId;
                    tObject.DocNo = creditNote.DocNo;
                    tObject.ModifiedBy = TObject.ModifiedBy;
                    deleteJVPostInvoce(tObject);
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_SuccessFully_Message);

                if (creditNote.DocSubType == DocTypeConstants.Interco && creditNote.Nature == DocTypeConstants.Interco)
                {
                    using (con = new SqlConnection(ConnectionString))
                    {
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        cmd = new SqlCommand("Bean_Insert_Document_History", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CompanyId", creditNote.CompanyId);
                        cmd.Parameters.AddWithValue("@DocumentId", allocation.Id);
                        cmd.Parameters.AddWithValue("@DocumentType", DocTypeConstants.Application);
                        cmd.Parameters.AddWithValue("@IsVoid", true);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_CreateCreditNoteApplicationReset_CreateCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex; 
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return allocation;
        }

        #endregion SaveCNApplicationVoid

        #region Invoice LookUp
        //public InvoiceModelLU GetAllInvoiceNewLUs(string username, Guid invoiceId, long companyid, string ConnectionString)
        //{
        //    InvoiceModelLU invoiceLU = new InvoiceModelLU();
        //    SqlConnection con = null;
        //    SqlCommand cmd = null;
        //    try
        //    {
        //        DateTime? lastInvoice = _invoiceService.GetByCompanyId(companyid, DocTypeConstant.Invoice);
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Request_Message);
        //        Invoice invoice = _invoiceService.GetAllInvoiceLu(companyid, invoiceId);
        //        DateTime date = invoice == null ? lastInvoice == null ? DateTime.Now : lastInvoice.Value : invoice.DocDate;
        //        long? credit = invoice == null ? 0 : invoice.CreditTermsId == null ? 0 : invoice.CreditTermsId;
        //        long? comp = invoice == null ? 0 : invoice.ServiceCompanyId == null ? 0 : invoice.ServiceCompanyId;
        //        List<CommonLookUps<string>> lstLookUps = new List<CommonLookUps<string>>();
        //        LookUpCategory<string> currency = new LookUpCategory<string>();
        //        string currencyCode = invoice != null ? invoice.DocCurrency : string.Empty;
        //        string query = InvoiceCommonQuery(username, companyid, date, credit, comp, currencyCode);
        //        int? resultsetCount = query.Split(';').Count();
        //        con = new SqlConnection(ConnectionString);
        //        if (con.State != ConnectionState.Open)
        //            con.Open();
        //        using (cmd = new SqlCommand(query, con))
        //        {
        //            cmd.CommandType = CommandType.Text;
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            for (int i = 1; i <= resultsetCount; i++)
        //            {
        //                if (dr.HasRows)
        //                {
        //                    while (dr.Read())
        //                    {
        //                        lstLookUps.Add(new CommonLookUps<string>
        //                        {
        //                            TableName = dr["TABLENAME"].ToString(),
        //                            Code = dr["CODE"].ToString(),
        //                            Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0,
        //                            Name = dr["NAME"].ToString(),
        //                            RecOrder = dr["RECORDER"] != DBNull.Value ? Convert.ToInt32(dr["RECORDER"]) : (int?)null,
        //                            TaxRate = dr["TAXRATE"] != DBNull.Value ? Convert.ToDouble(dr["TAXRATE"]) : (double?)null,
        //                            TaxType = dr["TAXTYPE"].ToString(),
        //                            TaxCode = dr["TXCODE"].ToString(),
        //                            TOPValue = dr["TOPVALUE"] != DBNull.Value ? Convert.ToDouble(dr["TOPVALUE"]) : (double?)null,
        //                            Currency = dr["CURRENCY"].ToString(),
        //                            Class = dr["Class"].ToString(),
        //                            isGstActivated = dr["IsGstActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstActive"]) : (bool?)null,
        //                            ShortName = dr["SHOTNAME"].ToString(),
        //                            //ServiceCompanyId = row["Id"] != DBNull.Value ? Convert.ToInt64(row["Id"]) : (long?)null,
        //                            DefaultValue = "SGD",
        //                            CategoryName = "SGD",
        //                            Status = (RecordStatusEnum)dr["STATUS"]
        //                        });
        //                    }
        //                }
        //                dr.NextResult();
        //            }
        //        }
        //        invoiceLU.NatureLU = new List<string> { "Trade", "Others" };
        //        if (lstLookUps.Any())
        //        {
        //            if (invoice != null)
        //            {
        //                currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
        //                currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
        //                currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYEDIT").Select(c => new LookUp<string>()
        //                {
        //                    Code = c.Code,
        //                    Name = c.Name,
        //                    RecOrder = c.RecOrder
        //                }).ToList();
        //                invoiceLU.CurrencyLU = currency;
        //            }
        //            else
        //            {
        //                currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
        //                currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
        //                currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYNEW").Select(c => new LookUp<string>()
        //                {
        //                    Code = c.Code,
        //                    Name = c.Name,
        //                    RecOrder = c.RecOrder
        //                }).ToList();
        //                invoiceLU.CurrencyLU = currency;
        //            }
        //            invoiceLU.TermsOfPaymentLU = lstLookUps.Where(c => c.TableName == "TERMSOFPAY").Select(x => new LookUp<string>()
        //            {
        //                Name = x.Name,
        //                Id = x.Id,
        //                TOPValue = x.TOPValue,
        //                RecOrder = x.RecOrder
        //            }).OrderBy(c => c.TOPValue).ToList();
        //            invoiceLU.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new LookUpCompany<string>()
        //            {
        //                Id = x.Id,
        //                Name = x.Name,
        //                ShortName = x.ShortName,
        //                isGstActivated = x.isGstActivated
        //            }).ToList();
        //        }
        //        List<COALookup<string>> lstEditCoa = new List<COALookup<string>>();
        //        List<TaxCodeLookUp<string>> lstEditTax = null;
        //        invoiceLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT" && z.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
        //        {
        //            Name = x.Name,
        //            Code = x.Code,
        //            Id = x.Id,
        //            RecOrder = x.RecOrder,
        //            IsPLAccount = x.COACategory == "Income Statement" ? true : false,
        //            Class = x.Class,
        //            Status = x.Status,
        //            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
        //        }).OrderBy(d => d.Name).ToList();
        //        invoiceLU.TaxCodeLU = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
        //        {
        //            Id = x.Id,
        //            Code = x.TaxCode,
        //            Name = x.Name,
        //            TaxRate = x.TaxRate,
        //            TaxType = x.TaxType,
        //            Status = x.Status,
        //            TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.TaxCode
        //        }).OrderBy(c => c.Code).ToList();
        //        if (invoice != null && invoice.InvoiceDetails.Count > 0)
        //        {
        //            List<long> CoaIds = invoice.InvoiceDetails.Select(c => c.COAId).ToList();
        //            if (invoiceLU.ChartOfAccountLU.Any())
        //                CoaIds = CoaIds.Except(invoiceLU.ChartOfAccountLU.Select(x => x.Id)).ToList();
        //            List<long?> taxIds = invoice.InvoiceDetails.Select(x => x.TaxId).ToList();
        //            if (invoiceLU.TaxCodeLU.Any())
        //                taxIds = taxIds.Except(invoiceLU.TaxCodeLU.Select(d => d.Id)).ToList();
        //            if (CoaIds.Any())
        //            {
        //                lstEditCoa = lstLookUps.Where(x => x.TableName == "CHARTOFACCOUNT" && CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
        //                {
        //                    Name = x.Name,
        //                    Code = x.TaxCode,
        //                    Id = x.Id,
        //                    RecOrder = x.RecOrder,
        //                    IsPLAccount = x.COACategory == "Income Statement" ? true : false,
        //                    Class = x.Class,
        //                    Status = x.Status,
        //                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
        //                }).OrderBy(d => d.Name).ToList();
        //                invoiceLU.ChartOfAccountLU.AddRange(lstEditCoa);
        //            }
        //            if (invoice.IsOBInvoice == true)
        //            {
        //                CommonLookUps<string> ObCOA = lstLookUps.Where(c => c.TableName == "OBCHARTOFACCOUNT").FirstOrDefault();
        //                if (ObCOA != null)
        //                {
        //                    invoiceLU.ChartOfAccountLU.Add(new COALookup<string>()
        //                    {
        //                        Name = ObCOA.Name,
        //                        Code = ObCOA.Code,
        //                        Id = ObCOA.Id,
        //                        RecOrder = ObCOA.RecOrder,
        //                        IsPLAccount = ObCOA.COACategory == "Income Statement" ? true : false,
        //                        Class = ObCOA.Class,
        //                        Status = ObCOA.Status,
        //                        IsTaxCodeNotEditable = (ObCOA.Class == "Assets" || ObCOA.Class == "Liabilities" || ObCOA.Class == "Equity") ? true : false
        //                    });
        //                }
        //            }

        //            if (taxIds.Any())
        //            {
        //                lstEditTax = lstLookUps.Where(c => taxIds.Contains(c.Id) && c.TableName == "TAXCODE").Select(x => new TaxCodeLookUp<string>()
        //                {
        //                    Id = x.Id,
        //                    Code = x.TaxCode,
        //                    Name = x.Name,
        //                    TaxRate = x.TaxRate,
        //                    TaxType = x.TaxType,
        //                    Status = x.Status,
        //                    TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.TaxCode
        //                }).ToList();
        //                invoiceLU.TaxCodeLU.AddRange(lstEditTax);
        //                invoiceLU.TaxCodeLU = invoiceLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
        //            }
        //        }
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_SuccessFully_Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Exception_Message);
        //        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message + " for " + companyid);
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return invoiceLU;
        //}

        #endregion

        #region Private_Block
        private string GetNextApplicationNumber(Guid id)
        {
            string DocNumber = "";
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetNextApplicationNumber_GetCall_Request_Message);
                string invoice = _cnApplicationService.GetCNNextNo(id);
                CreditNoteApplication CNAM = _cnApplicationService.GetCreditNoteById(id);/*Select(x => new { CNAppNo = x.CreditNoteApplicationNumber, Status = x.Status, Date = x.CreatedDate }).OrderByDescending(a => a.Date).FirstOrDefault();*/
                int DocNo = 0;
                if (CNAM != null)
                {
                    DocNo = CNAM.Status != CreditNoteApplicationStatus.Void ? Convert.ToInt32(CNAM.CreditNoteApplicationNumber.Substring(CNAM.CreditNoteApplicationNumber.LastIndexOf("-A") + 2)) : Convert.ToInt32(CNAM.CreditNoteApplicationNumber.Substring(CNAM.CreditNoteApplicationNumber.IndexOf("-A") + 2).Remove(CNAM.CreditNoteApplicationNumber.Substring(CNAM.CreditNoteApplicationNumber.IndexOf("-A") + 2).LastIndexOf("-V"), 2));
                }
                DocNo++;
                DocNumber = invoice + ("-A" + DocNo);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_GetNextApplicationNumber_GetCall_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_GetNextApplicationNumber_GetCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return DocNumber;
        }
        private static string InvoiceCommonQuery(string username, long companyid, DateTime date, long? credit, long? comp, string currencyCode)
        {
            return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY FROM Bean.Currency WHERE CompanyId={ companyid  } AND (Status=1 OR Code={currencyCode} OR DefaultValue='SGD');SELECT 'CURRENCYNEW' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY  FROM Bean.Currency WHERE CompanyId={ companyid } AND (Status=1 OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY FROM Common.TermsOfPayment where CompanyId={ companyid } AND (Status=1 OR Id= { credit } AND IsCustomer=1;SELECT 'SERVICECOMPANY' as TABLENAME,C.Id as ID,c.Name as NAME,c.ShortName as SHOTNAME,c.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY FROM Common.Company c JOIN Common.CompanyUser CU on C.ParentId=CU.CompanyId where (c.Status = 1 or c.Id = { comp } and c.ParentId ={companyid } and CU.Username={ username };SELECT 'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY FROM Bean.AccountType A JOIN Bean.ChartOfAccount COA on A.Id = COA.AccountTypeId where COA.CompanyId ={ companyid } and a.Name in ('Revenue','Other income') and COA.IsRealCOA=1;SELECT 'TAXCODE' as TABLENAME,Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY FROM Bean.TaxCode where CompanyId=0 and Status<3 and EffectiveFrom<={ String.Format("{0:MM/dd/yyyy}", date) } and (EffectiveTo>={ String.Format("{0:MM/dd/yyyy}", date) } OR EffectiveTo is null);SELECT 'OBCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY FROM Bean.ChartOfAccount COA where COA.CompanyId ={ companyid } and COA.Name={ COANameConstants.Opening_balance };";
        }

        private void FillCreditNoteApplicationModel(CreditNoteApplicationModel CNAModel, CreditNoteApplication CCA)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_FillCreditNoteApplicationModel_FillCall_Request_Message);
                CNAModel.Id = CCA.Id;
                CNAModel.InvoiceId = CCA.InvoiceId;
                CNAModel.CompanyId = CCA.CompanyId;
                CNAModel.IsModify = CCA.ClearCount > 0;
                CNAModel.IsLocked = CCA.IsLocked;
                InvoiceCompact invoice = _cnApplicationService.GetinvoiceById(CCA.InvoiceId);
                if (invoice != null)
                {
                    CNAModel.DocNo = invoice.DocNo;
                    CNAModel.DocCurrency = invoice.DocCurrency;
                    CNAModel.CreditNoteAmount = invoice.GrandTotal;
                    CNAModel.ExchangeRate = invoice.ExchangeRate;
                    CNAModel.GSTExchangeRate = invoice.GSTExchangeRate;
                    if (CCA.Status == CreditNoteApplicationStatus.Void)
                        CNAModel.CreditNoteBalanceAmount = CNAModel.CreditNoteAmount;
                    else
                        CNAModel.CreditNoteBalanceAmount = invoice.BalanceAmount + CCA.CreditAmount;
                }
                CNAModel.CreditNoteApplicationNumber = CCA.CreditNoteApplicationNumber;
                CNAModel.Version = "0x" + string.Concat(Array.ConvertAll(CCA.Version, x => x.ToString("X2")));
                CNAModel.CreditAmount = CCA.CreditAmount;
                CNAModel.IsNoSupportingDocument = CCA.IsNoSupportingDocument;
                CNAModel.NoSupportingDocument = CCA.IsNoSupportingDocumentActivated;
                CNAModel.CreditNoteApplicationDate = CCA.CreditNoteApplicationDate;
                CNAModel.CreditNoteApplicationResetDate = CCA.CreditNoteApplicationResetDate;
                CNAModel.Remarks = CCA.Remarks;
                CNAModel.CreatedDate = CCA.CreatedDate;
                CNAModel.UserCreated = CCA.UserCreated;
                CNAModel.Status = CCA.Status;
                CNAModel.DocumentId = CCA.DocumentId;
                CNAModel.ModifiedBy = CCA.ModifiedBy;
                CNAModel.ModifiedDate = CCA.ModifiedDate;
                CNAModel.IsRevExcess = CCA.IsRevExcess;
                CNAModel.ClearingState = CCA.ClearingState;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_FillCreditNoteApplicationModel_FillCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_FillCreditNoteApplicationModel_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        private void UpdateDocumentState(Guid documentId, string DocType, decimal amount, string ConnectionString, List<InvoiceCompact> lstInv, List<DebitNoteCompact> lstDn, Guid transationId, DateTime? postingDate, List<DocumentHistoryModel> lstDocuments, bool? isVoid, decimal creditAmount, Dictionary<Guid, decimal> lstOfRoundingAmount, bool isAdded, decimal oldCreditAmount, decimal newCreditAmount, decimal? detailRoundingAmount, out decimal? newRoundingAmount)
        {
            string docState = null;
            decimal roundingAmount = 0;
            decimal deductAmount = Math.Abs(amount);
            bool? isUpdated = false;
            newRoundingAmount = detailRoundingAmount;
            UpdatePosting up = new UpdatePosting();
            if (DocType == DocTypeConstants.Invoice)
            {
                InvoiceCompact document = lstInv.Where(a => a.Id == documentId).FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Invoice_to_Update_Balance_Amount);
                }
                if (isVoid != true && amount == 0 && isAdded == false && document.DocumentState == InvoiceStates.FullyPaid)
                {
                    //if ()
                    //{
                    //roundingAmount = (Math.Round((document.GrandTotal - newCreditAmount) * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((oldCreditAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)document.BaseGrandTotal;
                    if (detailRoundingAmount != 0 && detailRoundingAmount != null)
                        lstOfRoundingAmount.Add(document.Id, detailRoundingAmount.Value);
                    //}
                    newRoundingAmount = detailRoundingAmount != null ? detailRoundingAmount.Value : 0;
                    return;
                }
                docState = document.DocumentState;

                document.BalanceAmount -= amount;
                if (document.BalanceAmount == 0)
                {
                    document.DocumentState = DocType == DocTypeConstants.Receipt ? CreditNoteState.FullyApplied : InvoiceStates.FullyPaid;
                    if (isAdded)
                    {
                        if (document.RoundingAmount != null && document.RoundingAmount != 0)
                            roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                        else
                            roundingAmount = Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)document.BaseBalanceAmount;

                        document.BaseBalanceAmount = 0;
                        if (roundingAmount != 0)
                            lstOfRoundingAmount.Add(document.Id, roundingAmount);
                        document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                        newRoundingAmount = roundingAmount;
                    }
                    else
                    {
                        if (oldCreditAmount != newCreditAmount)
                        {
                            if (newCreditAmount == document.GrandTotal)
                            {
                                roundingAmount = Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)document.BaseGrandTotal);
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(document.Id, roundingAmount);
                                newRoundingAmount = roundingAmount;
                                document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                                document.BaseBalanceAmount = 0;
                            }
                            else if (detailRoundingAmount != 0 && detailRoundingAmount != null)
                            {
                                document.BaseBalanceAmount = 0;
                                lstOfRoundingAmount.Add(document.Id, detailRoundingAmount.Value);
                                newRoundingAmount = detailRoundingAmount.Value;
                                roundingAmount = detailRoundingAmount.Value;
                            }
                            else
                            {
                                if (document.RoundingAmount != null && document.RoundingAmount != 0)
                                    roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                                else
                                    //roundingAmount = (((Math.Round(Math.Abs(document.GrandTotal - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) + (Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) - (decimal)document.BaseGrandTotal) - (document.RoundingAmount != null ? (decimal)document.RoundingAmount : 0);

                                    roundingAmount = Math.Round(Math.Abs(newCreditAmount - oldCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)document.BaseBalanceAmount);

                                document.BaseBalanceAmount = 0;
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(document.Id, roundingAmount);
                                newRoundingAmount = roundingAmount;
                                document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                            }
                        }
                    }

                    if (document.IsOBInvoice == true)
                    {
                        SqlConnection conn = new SqlConnection(ConnectionString);
                        if (conn.State != ConnectionState.Open)
                            conn.Open();
                        SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                        oBcmd.CommandType = CommandType.StoredProcedure;
                        oBcmd.Parameters.AddWithValue("@OBId", document.OpeningBalanceId);
                        oBcmd.Parameters.AddWithValue("@DocumentId", document.Id);
                        oBcmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                        oBcmd.Parameters.AddWithValue("@IsEqual", false);
                        oBcmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                else if (document.BalanceAmount > 0)
                {
                    document.DocumentState = DocType == DocTypeConstants.Receipt ? CreditNoteState.PartialApplied : InvoiceStates.PartialPaid;
                    if (isVoid != true)
                    {
                        if (isAdded)
                            document.BaseBalanceAmount -= Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                        else
                        {
                            if (oldCreditAmount != newCreditAmount)
                            {
                                document.BaseBalanceAmount = oldCreditAmount > newCreditAmount ? document.BaseBalanceAmount + (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : document.BaseBalanceAmount - (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                if (docState == InvoiceStates.FullyPaid)
                                {
                                    document.RoundingAmount = ((detailRoundingAmount != 0 && detailRoundingAmount != null) && (docState != document.DocumentState)) ? detailRoundingAmount : document.RoundingAmount;
                                    newRoundingAmount = ((detailRoundingAmount != 0 && detailRoundingAmount != null) && (docState != document.DocumentState)) ? 0 : detailRoundingAmount;
                                }

                            }
                        }
                    }
                    else
                    {
                        //if (detailRoundingAmount != null && detailRoundingAmount != 0)
                        //{
                        //    roundingAmount = (Math.Round((document.GrandTotal - deductAmount) * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((deductAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)document.BaseGrandTotal;
                        //    document.BaseBalanceAmount += (Math.Round((deductAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - detailRoundingAmount);
                        //}
                        //else
                        //{
                        document.BaseBalanceAmount += (Math.Round((deductAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                        //}
                        document.RoundingAmount += (detailRoundingAmount != null && detailRoundingAmount != 0) ? detailRoundingAmount : 0;
                        isUpdated = true;
                        roundingAmount = 0;
                    }
                }


                else if (document.BalanceAmount < 0)
                    throw new Exception("Credit note amount shouldn't be greater than outstanding balance of invoice");
                if (document.GrandTotal == document.BalanceAmount)
                {
                    document.DocumentState = InvoiceStates.NotPaid;
                    document.BaseBalanceAmount = document.BaseGrandTotal;
                    document.RoundingAmount += (detailRoundingAmount != null && detailRoundingAmount != 0) ? detailRoundingAmount : 0;
                    newRoundingAmount = 0;
                }
                document.ModifiedBy = InvoiceConstants.System;
                document.ModifiedDate = DateTime.UtcNow;
                document.ObjectState = ObjectState.Modified;
                if (document.IsWorkFlowInvoice == true)
                    FillWokflowInvoice(document, ConnectionString);
                //FillWokflowInvoice(document);
                #region OB_Invoice_State_change
                if (document.IsOBInvoice == true)
                {
                    SqlConnection conn = new SqlConnection(ConnectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                    oBcmd.CommandType = CommandType.StoredProcedure;
                    oBcmd.Parameters.AddWithValue("@OBId", document.OpeningBalanceId);
                    oBcmd.Parameters.AddWithValue("@DocumentId", document.Id);
                    oBcmd.Parameters.AddWithValue("@CompanyId", document.CompanyId);
                    oBcmd.Parameters.AddWithValue("@IsEqual", document.BalanceAmount == document.GrandTotal && (document.AllocatedAmount == 0 || document.AllocatedAmount == null) ? true : false);
                    oBcmd.ExecuteNonQuery();
                    conn.Close();
                }
                #endregion
                else
                {
                    #region Update_Journal_Detail_Clearing_Status

                    SqlConnection conn = new SqlConnection(ConnectionString);
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    SqlCommand cmd1 = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", conn);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@companyId", document.CompanyId);
                    cmd1.Parameters.AddWithValue("@documentId", document.Id);
                    cmd1.Parameters.AddWithValue("@docState", document.DocumentState);
                    cmd1.Parameters.AddWithValue("@balanceAmount", document.BalanceAmount);
                    cmd1.ExecuteNonQuery();
                    conn.Close();

                    #endregion Update_Journal_Detail_Clearing_Status
                }
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(transationId, document.CompanyId, document.Id, document.DocType, document.DocSubType, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.DocDescription, isVoid == true ? null : postingDate, isVoid == true ? 0 : creditAmount < 0 ? creditAmount : -creditAmount, roundingAmount);
                    if (lstdocumet.Any())
                        //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                        lstDocuments.AddRange(lstdocumet);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                #endregion Documentary History
            }
            else if (DocType == DocTypeConstants.DebitNote)
            {
                DebitNoteCompact document = lstDn.Where(a => a.Id == documentId).FirstOrDefault();
                if (document == null)
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Debit_Note_to_Update_Balance_Amount);
                }
                if (isVoid != true && amount == 0 && isAdded == false && document.DocumentState == InvoiceStates.FullyPaid)
                {
                    //if ()
                    //{
                    //roundingAmount = (Math.Round((document.GrandTotal - newCreditAmount) * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((oldCreditAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)document.BaseGrandTotal;
                    if (detailRoundingAmount != 0 && detailRoundingAmount != null)
                        lstOfRoundingAmount.Add(document.Id, detailRoundingAmount.Value);
                    //}
                    newRoundingAmount = detailRoundingAmount != null ? detailRoundingAmount.Value : 0;
                    return;
                }
                docState = document.DocumentState;
                document.BalanceAmount -= amount;
                if (document.BalanceAmount == 0)
                {
                    document.DocumentState = DebitNoteStates.FullyPaid;
                    if (isAdded)
                    {
                        document.DocumentState = DocType == DocTypeConstants.Receipt ? CreditNoteState.FullyApplied : InvoiceStates.FullyPaid;
                        //roundingAmount = Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)document.BaseBalanceAmount;
                        if (document.RoundingAmount != null && document.RoundingAmount != 0)
                            roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                        else
                            roundingAmount = Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)document.BaseBalanceAmount;

                        document.BaseBalanceAmount = 0;
                        if (roundingAmount != 0)
                            lstOfRoundingAmount.Add(document.Id, roundingAmount);
                        //document.RoundingAmount = roundingAmount;
                        newRoundingAmount = roundingAmount;
                        document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                    }
                    else
                    {
                        if (oldCreditAmount != newCreditAmount)
                        {
                            if (newCreditAmount == document.GrandTotal)
                            {
                                roundingAmount = Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)document.BaseGrandTotal);
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(document.Id, roundingAmount);
                                newRoundingAmount = roundingAmount;
                                document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                                document.BaseBalanceAmount = 0;
                            }
                            else if (detailRoundingAmount != 0 && detailRoundingAmount != null)
                            {
                                document.BaseBalanceAmount = 0;
                                lstOfRoundingAmount.Add(document.Id, detailRoundingAmount.Value);
                                newRoundingAmount = detailRoundingAmount.Value;
                                roundingAmount = detailRoundingAmount.Value;
                            }
                            else
                            {
                                //roundingAmount = ((((decimal)document.BaseGrandTotal - (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) + (decimal)document.BaseBalanceAmount) - (decimal)document.BaseGrandTotal) - document.RoundingAmount != null ? (decimal)document.RoundingAmount : 0;
                                //roundingAmount = (((Math.Round(Math.Abs(document.GrandTotal - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) + (Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) - (decimal)document.BaseGrandTotal) - (document.RoundingAmount != null ? (decimal)document.RoundingAmount : 0);
                                if (document.RoundingAmount != null && document.RoundingAmount != 0)
                                    roundingAmount = ((document.RoundingAmount != null && document.RoundingAmount != 0) ? (decimal)document.RoundingAmount : 0);
                                else
                                    //roundingAmount = (((Math.Round(Math.Abs(document.GrandTotal - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) + (Math.Round(Math.Abs(newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero))) - (decimal)document.BaseGrandTotal) - (document.RoundingAmount != null ? (decimal)document.RoundingAmount : 0);
                                    roundingAmount = Math.Round(Math.Abs(newCreditAmount - oldCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)document.BaseBalanceAmount);

                                document.BaseBalanceAmount = 0;
                                if (roundingAmount != 0)
                                    lstOfRoundingAmount.Add(document.Id, roundingAmount);
                                newRoundingAmount = roundingAmount;
                                document.RoundingAmount = (roundingAmount != null && roundingAmount != 0 && (document.RoundingAmount != null && document.RoundingAmount != 0)) ? document.RoundingAmount - roundingAmount : 0;
                            }
                        }

                    }
                }
                else if (document.BalanceAmount > 0)
                {
                    document.DocumentState = DebitNoteStates.PartialPaid;
                    if (isVoid != true)
                    {
                        if (isAdded)
                            document.BaseBalanceAmount -= Math.Round(amount * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                        else
                        {
                            if (oldCreditAmount != newCreditAmount)
                            {
                                document.BaseBalanceAmount = oldCreditAmount > newCreditAmount ? document.BaseBalanceAmount + (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : document.BaseBalanceAmount - (Math.Round(Math.Abs(oldCreditAmount - newCreditAmount) * ((document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                if (docState == InvoiceStates.FullyPaid)
                                {
                                    document.RoundingAmount = ((detailRoundingAmount != 0 && detailRoundingAmount != null) && (docState != document.DocumentState)) ? detailRoundingAmount : document.RoundingAmount;
                                    newRoundingAmount = ((detailRoundingAmount != 0 && detailRoundingAmount != null) && (docState != document.DocumentState)) ? 0 : detailRoundingAmount;
                                }
                            }
                        }
                    }
                    else
                    {
                        //if (detailRoundingAmount != null && detailRoundingAmount != 0)
                        //{
                        //    roundingAmount = (Math.Round((document.GrandTotal - deductAmount) * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((deductAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)document.BaseGrandTotal;
                        //    document.BaseBalanceAmount += (Math.Round((deductAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - detailRoundingAmount);
                        //}
                        //else
                        //{
                        document.BaseBalanceAmount += (Math.Round((deductAmount * (document.ExchangeRate != null ? (decimal)document.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                        document.RoundingAmount += (detailRoundingAmount != null && detailRoundingAmount != 0) ? detailRoundingAmount : 0;
                        isUpdated = true;
                        //}
                    }
                }

                else if (document.BalanceAmount < 0)
                    throw new Exception("Credit note amount shouldn't be greater than outstanding balance of debit note");
                if (document.GrandTotal == document.BalanceAmount)
                {
                    document.DocumentState = DebitNoteStates.NotPaid;
                    document.BaseBalanceAmount = document.BaseGrandTotal;
                    if (isUpdated == false)
                        document.RoundingAmount += (detailRoundingAmount != null && detailRoundingAmount != 0) ? detailRoundingAmount : 0;
                    newRoundingAmount = 0;
                }
                document.ModifiedBy = InvoiceConstants.System;
                document.ModifiedDate = DateTime.UtcNow;
                document.ObjectState = ObjectState.Modified;
                //_debitNoteService.Update(document);
                #region Update_Journal_Detail_Clearing_Status
                SqlConnection conn = new SqlConnection(ConnectionString);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SqlCommand cmd1 = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@companyId", document.CompanyId);
                cmd1.Parameters.AddWithValue("@documentId", document.Id);
                cmd1.Parameters.AddWithValue("@docState", document.DocumentState);
                cmd1.Parameters.AddWithValue("@balanceAmount", document.BalanceAmount);
                cmd1.ExecuteNonQuery();
                conn.Close();
                #endregion Update_Journal_Detail_Clearing_Status
                //}
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(transationId, document.CompanyId, document.Id, DocType, DocTypeConstants.General, document.DocumentState, document.DocCurrency, document.GrandTotal, document.BalanceAmount, document.ExchangeRate.Value, document.ModifiedBy != null ? document.ModifiedBy : document.UserCreated, document.Remarks, isVoid == true ? null : postingDate, isVoid == true ? 0 : creditAmount < 0 ? creditAmount : -creditAmount, roundingAmount);
                    if (lstdocumet1.Any())
                        lstDocuments.AddRange(lstdocumet1);
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                #endregion Documentary History
            }

        }
        public void FillWokflowInvoice(InvoiceCompact invoice)
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
            WorkflowInvoicePosting(invoicevm);
        }
        public void deleteJVPostInvoce(JournalSaveModel tObject)
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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityBean)
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
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void ValidateCreditNoteApplication(InvoiceCompact creditNote, CreditNoteApplicationModel TObject)
        {
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            if (TObject.IsICActive != true && TObject.CreditNoteApplicationDetailModels.Select(c => c.ServiceEntityId).GroupBy(d => d.Value).Count() > 1)
                throw new Exception(DoubtfulDebitValidation.You_cannt_run_I_C_transaction_rather_I_C_is_not_activate_in_this_in_this_company);
            if (TObject.CreditNoteApplicationDate == null)
            {
                throw new Exception(DoubtfulDebitValidation.Invalid_Application_Date);
            }

            if ((TObject.CreditNoteApplicationDetailModels == null || TObject.CreditNoteApplicationDetailModels.Count == 0) && (TObject.ReverseExcessModels == null || TObject.ReverseExcessModels.Count == 0))
            {
                throw new Exception(DoubtfulDebitValidation.Atleast_one_Application_is_required);
            }
            else
            {
                if (TObject.IsRevExcess != true)
                {
                    int itemCount = TObject.CreditNoteApplicationDetailModels.Where(a => a.CreditAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(DoubtfulDebitValidation.Total_Amount_To_Credit_should_be_greater_than_Zero);
                    }
                }
                else
                {
                    int itemCount = TObject.ReverseExcessModels.Where(a => a.DocAmount > 0).Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(DoubtfulDebitValidation.Total_Amount_To_Credit_should_be_greater_than_Zero);
                    }
                }
            }

            //Need to verify the invoice is within Financial year
            if (!_masterService.ValidateYearEndLockDate(TObject.CreditNoteApplicationDate, TObject.CompanyId))
            {
                throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_masterService.ValidateFinancialOpenPeriod(TObject.CreditNoteApplicationDate, TObject.CompanyId))
            {
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(DoubtfulDebitValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_masterService.ValidateFinancialLockPeriodPassword(TObject.CreditNoteApplicationDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(DoubtfulDebitValidation.Invalid_Financial_Period_Lock_Password);
                }
            }

            //Verify if any Invoices or Debit Notes have Tagged amount, so that it is not allowed credit the amount. 
            // ( That tagged amount should be reset in Debt Provision allocations )
            string DocNumbers = "";
            List<Guid> lstDocIds = TObject.CreditNoteApplicationDetailModels.Where(a => a.DocType == DocTypeConstants.Invoice).Select(a => a.DocumentId).ToList();
            //List<InvoiceCompact> lstTaggedInvoices = new List<InvoiceCompact>();
            if (lstDocIds.Count > 0)
            {
                var lstTaggedInvoices = _cnApplicationService.GetTaggedInvoicesByCustomerAndCurrency(creditNote.EntityId, creditNote.DocCurrency, creditNote.CompanyId).Where(a => lstDocIds.Contains(a.Key));
                if (lstTaggedInvoices.Select(c => c.Value).Count() > 0)
                {
                    foreach (string docNo in lstTaggedInvoices.Select(c => c.Value).ToList())
                        DocNumbers += docNo + ",";
                }
            }
            lstDocIds = TObject.CreditNoteApplicationDetailModels.Where(a => a.DocType == DocTypeConstants.DebitNote).Select(a => a.DocumentId).ToList();
            //List<DebitNoteCompact> lstTaggedDebitNotes = new List<DebitNoteCompact>();
            if (lstDocIds.Count > 0)
            {
                var lstTaggedDebitNotes = _cnApplicationService.GetTaggedDebitNotesByCustomerAndCurrency(creditNote.EntityId, creditNote.DocCurrency, creditNote.CompanyId).Where(a => lstDocIds.Contains(a.Key)).ToList();
                if (lstTaggedDebitNotes.Count > 0)
                {
                    foreach (string docNo in lstTaggedDebitNotes.Select(c => c.Value).ToList())
                        DocNumbers += docNo + ",";
                }
            }
            //Verify if any of the application have amount
            if (TObject.IsRevExcess != true)
            {
                var amountDocuments = TObject.CreditNoteApplicationDetailModels.Where(a => a.CreditAmount > 0).ToList();
                if (amountDocuments.Count == 0)
                    throw new Exception(DoubtfulDebitValidation.Atleast_one_Application_should_be_given);
                //Verify Duplication Documents in details
                var duplicateDocuments = TObject.CreditNoteApplicationDetailModels.GroupBy(x => x.DocumentId).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
                if (duplicateDocuments.Count > 0)
                    throw new Exception(DoubtfulDebitValidation.Duplicate_documents_in_details);
            }
        }

        public void UpdateCreditNoteApplicationDetails(CreditNoteApplicationModel model, CreditNoteApplication cnApplication, string ConnectionString, Guid transationId, DateTime? postingDate, List<DocumentHistoryModel> lstDocuments, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteApplicationDetails_Update_Request_Message);
                List<CreditNoteApplicationDetail> lstDetails = cnApplication.CreditNoteApplicationDetails.Where(a => !model.CreditNoteApplicationDetailModels.Any(b => b.Id == a.Id)).ToList();

                foreach (CreditNoteApplicationDetail detailDelete in lstDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;
                //List<Guid> lstInvDocIds = cnApplication.CreditNoteApplicationDetails.Where(c => model.CreditNoteApplicationDetailModels.Select(d => d.Id).Contains(c.Id) && c.DocumentType == DocTypeConstants.Invoice).Select(c => c.DocumentId).ToList();
                List<Guid> lstInvDocIds = model.CreditNoteApplicationDetailModels.Where(c => c.DocType == DocTypeConstants.Invoice || c.DocType == DocTypeConstants.Receipt).Select(c => c.DocumentId).ToList();
                //List<Guid> lstDNDocIds = cnApplication.CreditNoteApplicationDetails.Where(c => model.CreditNoteApplicationDetailModels.Select(d => d.Id).Contains(c.Id) && c.DocumentType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList();
                List<Guid> lstDNDocIds = model.CreditNoteApplicationDetailModels.Where(c => c.DocType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList();
                List<InvoiceCompact> lstInvoice = _cnApplicationService.GetAllDDByInvoiceId(lstInvDocIds);
                List<DebitNoteCompact> lstDN = _cnApplicationService.GetDDByDebitNoteId(lstDNDocIds);

                //Checking the documentstate before proceeding to save the Credit note app details
                if ((lstInvoice.Any() && lstInvoice.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstDN.Any() && lstDN.Any(a => a.DocumentState == InvoiceStates.Void)))
                    throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                bool isAdded = false;
                decimal oldCreditAmount = 0;
                decimal? roundingAmount = 0;
                foreach (CreditNoteApplicationDetailModel detailModel in model.CreditNoteApplicationDetailModels)
                {
                    roundingAmount = 0;
                    oldCreditAmount = cnApplication.CreditNoteApplicationDetails.Where(a => a.Id == detailModel.Id).Select(a => a.CreditAmount).FirstOrDefault();
                    CreditNoteApplicationDetail detail = cnApplication.CreditNoteApplicationDetails.Where(a => a.Id == detailModel.Id).FirstOrDefault();

                    if (detail == null)
                    {
                        if (detailModel.CreditAmount != 0)
                        {
                            detail = new CreditNoteApplicationDetail();
                            detail.Id = Guid.NewGuid();
                            detail.CreditNoteApplicationId = model.Id;
                            detail.DocumentId = detailModel.DocumentId;
                            detail.DocumentType = detailModel.DocType;
                            detail.DocCurrency = detailModel.DocCurrency;
                            detail.CreditAmount = detailModel.CreditAmount;
                            detail.BaseCurrencyExchangeRate = detailModel.BaseCurrencyExchangeRate;
                            detail.ServiceEntityId = detailModel.ServiceEntityId;
                            detail.COAId = detailModel.COAId;
                            detail.DocNo = detailModel.DocNo;
                            isAdded = true;
                            UpdateDocumentState(detail.DocumentId, detail.DocumentType, detail.CreditAmount, ConnectionString, lstInvoice, lstDN, transationId, postingDate, lstDocuments, false, detail.CreditAmount, lstOfRoundingAmount, isAdded, detail.CreditAmount, detail.CreditAmount, 0, out roundingAmount);
                            detail.RoundingAmount = roundingAmount;
                            detail.ObjectState = ObjectState.Added;
                            cnApplication.CreditNoteApplicationDetails.Add(detail);
                        }
                    }
                    else
                    {
                        UpdateDocumentState(detail.DocumentId, detail.DocumentType, detailModel.CreditAmount - detail.CreditAmount, ConnectionString, lstInvoice, lstDN, transationId, postingDate, lstDocuments, false, detailModel.CreditAmount, lstOfRoundingAmount, isAdded, oldCreditAmount, detailModel.CreditAmount, detail.RoundingAmount, out roundingAmount);
                        if (detailModel.CreditAmount == 0)
                        {
                            detail.ObjectState = ObjectState.Deleted;
                        }
                        else
                        {
                            detail.RoundingAmount = roundingAmount;
                            detail.ServiceEntityId = detailModel.ServiceEntityId;
                            detail.COAId = detailModel.COAId;
                            detail.CreditAmount = detailModel.CreditAmount;
                            detail.ObjectState = ObjectState.Modified;
                        }
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteApplicationDetails_Update_SuccessFully_Message);

            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,CreditNoteLoggingValidation.Log_UpdateCreditNoteApplicationDetails_Update_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        private void UpdateCreditNoteApplicationRevExcessDetails(CreditNoteApplicationModel TObject, CreditNoteApplication cnApplication, string ConnectionString, List<DocumentHistoryModel> lstDocuments)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteDetails_Update_Request_Message);

                #region CNApplication_ReverseExcess_Check_UnCheck_DataUpdation_Invoice_DN

                if (cnApplication.CreditNoteApplicationDetails.Any())
                {
                    List<Guid> invoiceIds = cnApplication.CreditNoteApplicationDetails.Where(d => d.DocumentType == DocTypeConstants.Invoice).Select(c => c.DocumentId).ToList();
                    List<Guid> debitNoteIds = cnApplication.CreditNoteApplicationDetails.Where(d => d.DocumentType == DocTypeConstants.DebitNote).Select(c => c.DocumentId).ToList();
                    List<Guid> documentIds = new List<Guid>();
                    documentIds.AddRange(invoiceIds);
                    documentIds.AddRange(debitNoteIds);
                    //List<AppsWorld.InvoiceModule.Entities.Journal> lstJournal = _journalService.GetListOfJournalByDocId(documentIds, TObject.CompanyId);
                    decimal roundingAmount = 0;
                    if (invoiceIds.Any())
                    {
                        List<InvoiceCompact> lstInvoice = _cnApplicationService.GetAllDDByInvoiceId(invoiceIds);
                        foreach (InvoiceCompact inv in lstInvoice)
                        {
                            CreditNoteApplicationDetail detail = cnApplication.CreditNoteApplicationDetails.Where(c => c.DocumentId == inv.Id).FirstOrDefault();
                            inv.BalanceAmount += detail.CreditAmount;
                            inv.DocumentState = inv.BalanceAmount == inv.GrandTotal ? InvoiceState.NotPaid : inv.DocumentState;
                            inv.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                            if (inv.DocumentState == InvoiceState.NotPaid)
                                inv.BaseBalanceAmount = inv.BaseGrandTotal;
                            else
                            {
                                //roundingAmount = (Math.Round((inv.GrandTotal - detail.CreditAmount) * (inv.ExchangeRate != null ? (decimal)inv.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.CreditAmount * (inv.ExchangeRate != null ? (decimal)inv.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)inv.BaseGrandTotal;
                                inv.BaseBalanceAmount += (Math.Round((detail.CreditAmount * (inv.ExchangeRate != null ? (decimal)inv.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                            }
                            query = $"Select * from Bean.Journal J where J.CompanyId={TObject.CompanyId} and J.DocumentId='{inv.Id}'";
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand(query, con);
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Close();
                                    query = $"Update Bean.Journal  set BalanceAmount={inv.BalanceAmount},DocumentState='{inv.DocumentState}',ModifiedBy='{InvoiceConstants.System}',ModifiedDate='{String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)}' where CompanyId={TObject.CompanyId} and  DocumentId='{inv.Id}' ";
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            inv.ModifiedBy = "System";
                            inv.ModifiedDate = DateTime.UtcNow;
                            inv.ObjectState = ObjectState.Modified;
                            _cnApplicationService.UpdateInvoice(inv);
                            //if (lstJournal.Any())
                            //{
                            //    AppsWorld.InvoiceModule.Entities.Journal journal = lstJournal.Where(a => a.CompanyId == TObject.CompanyId && a.DocumentId == inv.Id).FirstOrDefault();
                            //    if (journal != null)
                            //    {
                            //        journal.BalanceAmount = inv.BalanceAmount;
                            //        journal.DocumentState = inv.DocumentState;
                            //        journal.ObjectState = ObjectState.Modified;
                            //        _journalService.Update(journal);
                            //    }
                            //}

                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(cnApplication.Id, cnApplication.CompanyId, inv.Id, inv.DocType, inv.DocSubType, inv.DocumentState, inv.DocCurrency, inv.GrandTotal, inv.BalanceAmount, inv.ExchangeRate.Value, inv.ModifiedBy != null ? inv.ModifiedBy : inv.UserCreated, inv.DocDescription, null, 0, 0);
                                if (lstdocumet.Any())
                                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                                    lstDocuments.AddRange(lstdocumet);
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                            #endregion Documentary History


                        }
                    }
                    if (debitNoteIds.Any())
                    {
                        List<DebitNoteCompact> lstDebitNote = _cnApplicationService.GetDDByDebitNoteId(debitNoteIds);
                        foreach (DebitNoteCompact debitNote in lstDebitNote)
                        {
                            CreditNoteApplicationDetail detail = cnApplication.CreditNoteApplicationDetails.Where(c => c.DocumentId == debitNote.Id).FirstOrDefault();
                            debitNote.BalanceAmount += detail.CreditAmount;
                            debitNote.DocumentState = debitNote.BalanceAmount == debitNote.GrandTotal ? InvoiceState.NotPaid : debitNote.DocumentState;
                            debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                            if (debitNote.DocumentState == InvoiceState.NotPaid)
                                debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                            else
                            {
                                //roundingAmount = (Math.Round((debitNote.GrandTotal - detail.CreditAmount) * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.CreditAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)debitNote.BaseGrandTotal;
                                debitNote.BaseBalanceAmount += (Math.Round((detail.CreditAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                            }
                            query = $"Select * from Bean.Journal J where J.CompanyId={TObject.CompanyId} and J.DocumentId='{debitNote.Id}'";
                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand(query, con);
                                dr = cmd.ExecuteReader();
                                if (dr.HasRows)
                                {
                                    dr.Close();
                                    query = $"Update Bean.Journal set BalanceAmount={debitNote.BalanceAmount},DocumentState='{debitNote.DocumentState}',ModifiedBy='{InvoiceConstants.System}',ModifiedDate='{String.Format("{0:MM/dd/yyyy}", DateTime.UtcNow)}' where CompanyId={TObject.CompanyId} and DocumentId='{debitNote.Id}' ";
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                            debitNote.ModifiedBy = "System";
                            debitNote.ModifiedDate = DateTime.UtcNow;
                            debitNote.ObjectState = ObjectState.Modified;
                            _cnApplicationService.UpdateDebitNote(debitNote);
                            //if (lstJournal.Any())
                            //{
                            //    AppsWorld.InvoiceModule.Entities.Journal journal = lstJournal.Where(a => a.CompanyId == TObject.CompanyId && a.DocumentId == debitNote.Id).FirstOrDefault();
                            //    if (journal != null)
                            //    {
                            //        journal.BalanceAmount = debitNote.BalanceAmount;
                            //        journal.DocumentState = debitNote.DocumentState;
                            //        journal.ObjectState = ObjectState.Modified;
                            //        _journalService.Update(journal);
                            //    }

                            //}

                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet1 = AppaWorld.Bean.Common.FillDocumentHistory(cnApplication.Id, cnApplication.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, null, 0, 0);
                                if (lstdocumet1.Any())
                                    lstDocuments.AddRange(lstdocumet1);
                            }
                            catch (Exception ex)
                            {
                                //throw ex;
                            }
                            #endregion Documentary History

                        }
                    }
                }

                #region deleting_existing_record

                foreach (CreditNoteApplicationDetail detailDelete in cnApplication.CreditNoteApplicationDetails)
                    detailDelete.ObjectState = ObjectState.Deleted;

                #endregion deleting_existing_record
                #endregion CNApplication_ReverseExcess_Check_UnCheck_DataUpdation_Invoice_DN

                #region CNApplication_ReverseExcess_Account_Added_Modified
                List<CreditNoteApplicationDetail> lstCNApplicationDetail = _cnApplicationService.GetCreditNoteDetail(cnApplication.Id);
                foreach (ReverseExcessModel detail in TObject.ReverseExcessModels)
                {
                    if (detail.RecordStatus == "Added")
                    {
                        CreditNoteApplicationDetail cnAppDetail = new CreditNoteApplicationDetail();
                        cnAppDetail.Id = Guid.NewGuid();
                        FillCNApplicationReverseModel(cnApplication, detail, cnAppDetail);
                        cnAppDetail.RecOrder = recorder + 1;
                        recorder = detail.RecOrder;
                        cnAppDetail.DocCurrency = TObject.DocCurrency;
                        cnAppDetail.RoundingAmount = 0;
                        cnAppDetail.ObjectState = ObjectState.Added;
                        _cnApplicationService.Insert(cnAppDetail);
                    }
                    else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                    {
                        CreditNoteApplicationDetail cnAppDetail = lstCNApplicationDetail.Where(c => c.Id == detail.Id).FirstOrDefault();
                        if (cnAppDetail != null)
                        {
                            FillCNApplicationReverseModel(cnApplication, detail, cnAppDetail);
                            cnAppDetail.RecOrder = detail.RecOrder;
                            cnAppDetail.DocCurrency = TObject.DocCurrency;
                            cnAppDetail.RoundingAmount = 0;
                            cnAppDetail.ObjectState = ObjectState.Modified;
                            _cnApplicationService.Update(cnAppDetail);
                        }
                    }
                    else if (detail.RecordStatus == "Deleted")
                    {
                        CreditNoteApplicationDetail cnAppDetail = lstCNApplicationDetail.Where(c => c.Id == detail.Id).FirstOrDefault();
                        if (cnAppDetail != null)
                            cnAppDetail.ObjectState = ObjectState.Deleted;
                    }
                }
                #endregion CNApplication_ReverseExcess_Account_Added_Modified

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, CreditNoteLoggingValidation.Log_UpdateCreditNoteDetails_Update_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void FillCNApplicationReverseModel(CreditNoteApplication cnApplication, ReverseExcessModel detail, CreditNoteApplicationDetail cnAppDetail)
        {
            cnAppDetail.CreditNoteApplicationId = cnApplication.Id;
            cnAppDetail.DocDescription = detail.ItemDescription;
            cnAppDetail.CreditAmount = detail.DocAmount;
            cnAppDetail.TaxAmount = detail.DocTaxAmount;
            cnAppDetail.TotalAmount = detail.DocTotalAmount;
            cnAppDetail.TaxId = detail.TaxId;
            cnAppDetail.TaxRate = detail.TaxRate;
            cnAppDetail.TaxIdCode = detail.TaxIdCode;
            cnAppDetail.COAId = detail.COAId;
        }
        private string GetNextApplicationNumberSeries(string sysNumber, bool isFirst, string originalSysNumber)
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

        public void FillWokflowInvoice(InvoiceCompact invoice, string ConnectionString)
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
            invoicevm.Status = RecordStatusEnum.Active;
            //WorkflowInvoicePosting(invoicevm);
            try
            {
                SqlConnection con = null;
                using (con = new SqlConnection(ConnectionString))
                {
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("Bean_BCInvoice_State_To_WFInvoice", con);
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

                //throw;
            }

        }
        #endregion Private_Block

        #region Auto_Number
        //public string GetDocNumbers(long companyId, string docNumber)
        //{
        //    string docNo = docNumber;
        //    int i = 0;
        //    bool isBreak = false;
        //    List<Invoice> lstInvoices = _invoiceService.GetDocNumber(companyId, docNumber);
        //    if (lstInvoices.Any())
        //    {
        //        while (isBreak == false)
        //        {
        //            i++;
        //            docNo = docNumber + "-" + i;
        //            var inc = lstInvoices.Where(a => a.DocNo == docNo).Select(a => a.DocNo).FirstOrDefault();
        //            if (inc == null)
        //            {
        //                isBreak = true;
        //            }
        //        }
        //    }
        //    return docNo;
        //}
        //string value = "";
        //public string GenerateAutoNumberForType(long CompanyId, string Type, string companyCode)
        //{
        //    AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(CompanyId, Type);
        //    string generatedAutoNumber = "";

        //    if (Type == DocTypeConstants.Invoice || Type == DocTypeConstants.DoubtFulDebitNote || Type == DocTypeConstants.CreditNote || Type == DocTypeConstants.Provision)
        //    {
        //        generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength), _autoNo.GeneratedNumber, CompanyId, companyCode);

        //        if (_autoNo != null)
        //        {
        //            _autoNo.GeneratedNumber = value;
        //            _autoNo.IsDisable = true;
        //            _autoNo.ObjectState = ObjectState.Modified;
        //            _autoNumberService.Update(_autoNo);
        //        }
        //        var _autonumberCompany = _autoNumberService.GetAutoNumberCompany(_autoNo.Id);
        //        if (_autonumberCompany.Any())
        //        {
        //            AutoNumberComptCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
        //            _autoNumberCompanyNew.GeneratedNumber = value;
        //            _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
        //            _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
        //            //_autoNumberService.Update(_autoNumberCompanyNew);
        //        }
        //        else
        //        {
        //            AutoNumberComptCompany _autoNumberCompanyNew = new AutoNumberComptCompany();
        //            _autoNumberCompanyNew.GeneratedNumber = value;
        //            _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
        //            _autoNumberCompanyNew.Id = Guid.NewGuid();
        //            _autoNumberCompanyNew.ObjectState = ObjectState.Added;
        //            //_autoNumberService.Insert(_autoNumberCompanyNew);
        //        }
        //    }
        //    return generatedAutoNumber;
        //}
        //public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, string Companycode = null)
        //{
        //    List<Invoice> lstInvoices = null;
        //    bool ifMonthcontains = false;
        //    int currentMonth = 0;
        //    string OutputNumber = "";
        //    string counter = "";
        //    string companyFormatHere = companyFormatFrom.ToUpper();

        //    if (companyFormatHere.Contains("{YYYY}"))
        //    {
        //        companyFormatHere = companyFormatHere.Replace("{YYYY}", DateTime.Now.Year.ToString());
        //    }
        //    else if (companyFormatHere.Contains("{MM/YYYY}"))
        //    {
        //        companyFormatHere = companyFormatHere.Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
        //        currentMonth = DateTime.Now.Month;
        //        ifMonthcontains = true;
        //    }
        //    else if (companyFormatHere.Contains("{COMPANYCODE}"))
        //    {
        //        companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
        //    }
        //    double val = 0;
        //    if (Type == DocTypeConstants.Invoice)
        //    {
        //        lstInvoices = _invoiceService.GetCompanyIdAndDocType(companyId);

        //        if (lstInvoices.Any() && ifMonthcontains)
        //        {
        //            AutoNumberCompact autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //            int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
        //            if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
        //            {
        //                if (currentMonth == lastInvCreatedMonth)
        //                {
        //                    val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
        //                }
        //                else
        //                { val = 1; }
        //            }
        //            else
        //                val = 1;
        //        }
        //        else
        //        {
        //            if (lstInvoices.Any())
        //            {
        //                AutoNumberCompact autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //                #region Commented Code
        //                //foreach (var invoice in lstInvoices)
        //                //{
        //                //    if (invoice.InvoiceNumber != autonumber.Preview)
        //                //        val = Convert.ToInt32(IncreamentVal);
        //                //    else
        //                //    {
        //                //        val = Convert.ToInt32(IncreamentVal) + 1;
        //                //        break;
        //                //    }
        //                //}
        //                #endregion

        //                val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
        //            }
        //            else
        //            {
        //                val = Convert.ToInt32(IncreamentVal);
        //            }
        //        }
        //    }
        //    //else if (Type == /*DocTypeConstants.DoubtFulDebitNote*/"Debt Provision")
        //    //{
        //    //    lstInvoices = _invoiceEntityService.GetCompanyIdByDoubtFulDbt(companyId);

        //    //    if (lstInvoices.Any() && ifMonthcontains)
        //    //    {
        //    //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //    //        int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
        //    //        if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
        //    //        {
        //    //            if (currentMonth == lastInvCreatedMonth)
        //    //            {
        //    //                val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
        //    //            }
        //    //            else
        //    //            {
        //    //                val = 1;
        //    //            }
        //    //        }
        //    //        else
        //    //        { val = 1; }

        //    //    }
        //    //    else
        //    //    {
        //    //        if (lstInvoices.Any())
        //    //        {
        //    //            AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //    //            #region Commented Code
        //    //            //foreach (var invoice in lstInvoices)
        //    //            //{
        //    //            //    if (invoice.InvoiceNumber != autonumber.Preview)
        //    //            //        val = Convert.ToInt32(IncreamentVal);
        //    //            //    else
        //    //            //    {
        //    //            //        val = Convert.ToInt32(IncreamentVal) + 1;
        //    //            //        break;
        //    //            //    }
        //    //            //}
        //    //            #endregion

        //    //            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
        //    //        }
        //    //        else
        //    //        {
        //    //            val = Convert.ToInt32(IncreamentVal);
        //    //        }
        //    //    }

        //    //}
        //    //else if (Type == DocTypeConstants.CreditNote)
        //    //{
        //    //    lstInvoices = _invoiceEntityService.GetCompanyIdByCreditNote(companyId);

        //    //    if (lstInvoices.Any() && ifMonthcontains)
        //    //    {
        //    //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);

        //    //        int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
        //    //        if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
        //    //        {
        //    //            if (currentMonth == lastInvCreatedMonth)
        //    //            {
        //    //                val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
        //    //            }
        //    //            else
        //    //            {
        //    //                val = 1;
        //    //            }
        //    //        }
        //    //        else
        //    //            val = 1;
        //    //    }
        //    //    else
        //    //    {
        //    //        if (lstInvoices.Any())
        //    //        {
        //    //            AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //    //            #region commented Code
        //    //            //foreach (var invoice in lstInvoices)
        //    //            //{
        //    //            //    if (invoice.InvoiceNumber != autonumber.Preview)
        //    //            //        val = Convert.ToInt32(IncreamentVal);
        //    //            //    else
        //    //            //    {
        //    //            //        val = Convert.ToInt32(IncreamentVal) + 1;
        //    //            //        break;
        //    //            //    }
        //    //            //}
        //    //            #endregion

        //    //            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
        //    //        }
        //    //        else
        //    //        {
        //    //            val = Convert.ToInt32(IncreamentVal);
        //    //        }
        //    //    }
        //    //}
        //    //else if (Type == DocTypeConstants.Provision)
        //    //{
        //    //    List<Provision> lstProvisions = _provisionService.lstInvoiceProvision(companyId);
        //    //    if (lstProvisions.Any())
        //    //    {
        //    //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //    //        foreach (var provision in lstProvisions)
        //    //        {
        //    //            if (provision.SystemRefNo != autonumber.Preview)
        //    //                val = Convert.ToInt32(IncreamentVal);
        //    //            else
        //    //            {
        //    //                val = Convert.ToInt32(IncreamentVal) + 1;
        //    //                break;
        //    //            }
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        val = Convert.ToInt32(IncreamentVal);
        //    //    }
        //    //}
        //    if (counterLength == 1)
        //        counter = string.Format("{0:0}", val);
        //    else if (counterLength == 2)
        //        counter = string.Format("{0:00}", val);
        //    else if (counterLength == 3)
        //        counter = string.Format("{0:000}", val);
        //    else if (counterLength == 4)
        //        counter = string.Format("{0:0000}", val);
        //    else if (counterLength == 5)
        //        counter = string.Format("{0:00000}", val);
        //    else if (counterLength == 6)
        //        counter = string.Format("{0:000000}", val);
        //    else if (counterLength == 7)
        //        counter = string.Format("{0:0000000}", val);
        //    else if (counterLength == 8)
        //        counter = string.Format("{0:00000000}", val);
        //    else if (counterLength == 9)
        //        counter = string.Format("{0:000000000}", val);
        //    else if (counterLength == 10)
        //        counter = string.Format("{0:0000000000}", val);

        //    value = counter;
        //    OutputNumber = companyFormatHere + counter;

        //    if (lstInvoices.Any())
        //    {
        //        OutputNumber = GetNewNumber(lstInvoices, Type, OutputNumber, counter, companyFormatHere, counterLength);
        //    }

        //    return OutputNumber;
        //}
        private static double GetValue(string IncreamentVal, List<Invoice> lstInvoices, double val, AutoNumberCompact autonumber)
        {
            foreach (var invoice in lstInvoices)
            {
                if (invoice.InvoiceNumber != autonumber.Preview)
                    //val = Convert.ToInt32(IncreamentVal);
                    val = 1;
                else
                {
                    val = Convert.ToInt32(IncreamentVal) + 1;

                    break;
                }
            }
            return val;
        }
        //private string GetNewNumber(List<Invoice> lstInvoice, string type, string outputNumber, string counter, string format, int counterLength)
        //{
        //    string val1 = outputNumber;
        //    string val2 = "";
        //    var invoice = lstInvoice.Where(a => a.InvoiceNumber == outputNumber).FirstOrDefault();
        //    bool isNotexist = false;
        //    int i = Convert.ToInt32(counter);
        //    if (invoice != null)
        //    {
        //        while (isNotexist == false)
        //        {
        //            i++;
        //            string length = i.ToString();
        //            value = length.PadLeft(counterLength, '0');
        //            val2 = format + value;
        //            var inv = lstInvoice.Where(c => c.InvoiceNumber == val2).FirstOrDefault();
        //            if (inv == null)
        //                isNotexist = true;
        //        }
        //        val1 = val2;
        //    }
        //    return val1;
        //}

        #endregion Auto_Number

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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityWorkflow)
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
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region Posting_Block
        public void SaveInvoice1(JVModel clientModel)
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
                //        if (section.Ziraff[i].Name == InvoiceConstants.IdentityBean)
                //        {
                //            url = section.Ziraff[i].ServerUrl;
                //            break;
                //        }
                //    }
                //}
                object obj = clientModel;

                var response = RestSharpHelper.Post(url, "api/journal/saveposting", json);

                if (response.ErrorMessage != null)
                {

                }
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

        }

        private void FillCreditNoteJournal(JVModel headJournal, CreditNoteApplication CreditNoteApplication, bool isNew, bool? isGstSettings, IDictionary<long, long?> lstIC, long exchangeGainLossId, out List<InvoiceCompact> lstAllInvoice, out List<DebitNoteCompact> lstAllDN, bool? isOffset, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            decimal? amountDue = 0;
            InvoiceCompact invoice = _cnApplicationService.GetinvoiceById(CreditNoteApplication.InvoiceId);

            //modified for COA
            Dictionary<long, string> coaNames = _masterService.GetChartofAccounts(new List<string>() { COANameConstants.AccountsReceivables, COANameConstants.OtherReceivables }, CreditNoteApplication.CompanyId);

            if (coaNames != null)
            {
                headJournal.COAId = invoice.Nature == "Trade" ? coaNames.Where(a => a.Value == COANameConstants.AccountsReceivables).Select(a => a.Key).FirstOrDefault() : coaNames.Where(a => a.Value == COANameConstants.OtherReceivables).Select(a => a.Key).FirstOrDefault();
            }
            else
            {
                headJournal.COAId = _masterService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            }

            //headJournal.COAId = _masterService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            int? recOreder = 0;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillCNJV(headJournal, CreditNoteApplication, invoice);
            headJournal.SystemReferenceNo = docNo = GetNextApplicationNumberSeries(invoice.DocNo, true, invoice.DocNo);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            FillJVHeadCNDetails(jModel, CreditNoteApplication, invoice, lstOfRoundingAmount);
            jModel.AmountDue = amountDue;
            jModel.COAId = headJournal.COAId;
            lstJD.Add(jModel);
            //ChartOfAccountCompact account2 = _masterService.GetChartOfAccountByName(COANameConstants.ExchangeGainLossRealised, invoice.CompanyId);
            List<InvoiceCompact> lstInvoice = null;
            List<DebitNoteCompact> lstDN = null;
            lstAllInvoice = _cnApplicationService.GetAllDDByInvoiceId(CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted && (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.Receipt || a.DocumentType == DocTypeConstants.BillPayment)).Select(d => d.DocumentId).ToList());
            lstAllDN = _cnApplicationService.GetDDByDebitNoteId(CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted && a.DocumentType == DocTypeConstants.DebitNote).Select(d => d.DocumentId).ToList());
            if (lstAllInvoice.Any())
                lstInvoice = lstAllInvoice.Where(x => x.ServiceCompanyId == invoice.ServiceCompanyId).ToList();
            if (lstAllDN.Any())
                lstDN = lstAllDN.Where(d => d.ServiceCompanyId == invoice.ServiceCompanyId).ToList();
            foreach (CreditNoteApplicationDetail detail in CreditNoteApplication.IsRevExcess != true ? CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted && (a.DocumentType == DocTypeConstants.Receipt || a.DocumentType == DocTypeConstants.BillPayment) ? (a.ServiceEntityId == invoice.ServiceCompanyId || a.ServiceEntityId != invoice.ServiceCompanyId) : a.ServiceEntityId == invoice.ServiceCompanyId) : CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted))
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillCNDetails(journal, CreditNoteApplication, invoice, detail, null, lstInvoice, lstDN, isOffset, coaNames, lstOfRoundingAmount);
                journal.BaseCurrency = invoice.ExCurrency;
                journal.DocCurrency = invoice.DocCurrency;
                journal.IsTax = false;
                journal.RecOrder = ++recOreder;
                //recOreder = journal.RecOrder;
                lstJD.Add(journal);
                if (CreditNoteApplication.IsRevExcess == true)
                {
                    if (isGstSettings == true && (detail.TaxRate != null && detail.TaxIdCode != "NA"))
                    {
                        JVVDetailModel gstJournal = new JVVDetailModel();
                        if (isNew)
                            gstJournal.Id = Guid.NewGuid();
                        else
                            gstJournal.Id = detail.Id;
                        FillGSTSettings(gstJournal, CreditNoteApplication, invoice, detail);
                        gstJournal.RecOrder = lstJD.Max(a => a.RecOrder) + 1;
                        lstJD.Add(gstJournal);
                    }
                }
                if (CreditNoteApplication.IsRevExcess != true)
                {
                    if (invoice.DocCurrency != invoice.ExCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        journal1.DocType = invoice.DocType;
                        journal1.DocSubType = DocTypeConstants.Application;
                        journal1.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
                        journal1.AccountDescription = CreditNoteApplication.Remarks;
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.DocumentId = CreditNoteApplication.Id;
                        if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.Receipt || detail.DocumentType == DocTypeConstants.BillPayment)
                        {
                            //InvoiceCompact inv = _cnApplicationService.GetinvoiceById(detail.DocumentId);
                            InvoiceCompact inv = lstInvoice.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                            if (inv != null)
                            {
                                journal1.Nature = inv.Nature;
                                journal1.EntityId = inv.EntityId;
                                journal1.OffsetDocument = inv.DocNo;
                                journal1.ExchangeRate = inv.ExchangeRate;
                            }
                        }
                        if (detail.DocumentType == DocTypeConstants.DebitNote)
                        {
                            //DebitNoteCompact inv = _cnApplicationService.GetDebitNoteById(detail.DocumentId);
                            DebitNoteCompact inv = lstDN.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                            if (inv != null)
                            {
                                journal1.Nature = inv.Nature;
                                journal1.EntityId = inv.EntityId;
                                journal1.OffsetDocument = inv.DocNo;
                                journal1.ExchangeRate = inv.ExchangeRate;
                            }
                        }
                        //ChartOfAccountCompact account2 = _masterService.GetChartOfAccountByName(COANameConstants.ExchangeGainLossRealised, invoice.CompanyId);
                        if (exchangeGainLossId != 0)
                            journal1.COAId = exchangeGainLossId;
                        journal1.DocCurrency = detail.DocCurrency;
                        journal1.BaseCurrency = invoice.ExCurrency;
                        journal1.ServiceCompanyId = invoice != null ? invoice.ServiceCompanyId.Value : 0;
                        journal1.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
                        journal1.RecOrder = recOreder + 1;
                        recOreder = journal1.RecOrder;
                        journal1.ExchangeRate = isOffset == true ? detail.BaseCurrencyExchangeRate : journal1.ExchangeRate;
                        if (invoice.ExchangeRate > journal1.ExchangeRate)
                        {
                            journal1.BaseCredit = Math.Round((decimal)(invoice.ExchangeRate - journal1.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (invoice.ExchangeRate < journal1.ExchangeRate)
                        {
                            journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - invoice.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (invoice.ExchangeRate != journal1.ExchangeRate)
                        {
                            journal1.RecOrder = lstJD.Max(a => a.RecOrder) + 1;
                            lstJD.Add(journal1);
                        }
                    }
                }
            }

            #region For I/C Transaction
            //List<long?> serviceEntityIds = null;
            //IDictionary<string, long> lstIC = null;
            //if (CreditNoteApplication.CreditNoteApplicationDetails.Any())
            //{
            //    serviceEntityIds = CreditNoteApplication.CreditNoteApplicationDetails.Where(d => d.ServiceEntityId != invoice.ServiceCompanyId).Select(c => c.COAId).ToList();
            //    if (serviceEntityIds.Any())
            //        lstIC = _masterService.GetAllICAccount(serviceEntityIds);
            //}

            if (lstIC != null && lstIC.Any() && CreditNoteApplication.IsRevExcess != true)
            {
                if (lstAllInvoice.Any())
                    lstInvoice = lstAllInvoice.Where(x => x.ServiceCompanyId != invoice.ServiceCompanyId).ToList();
                if (lstAllDN.Any())
                    lstDN = lstAllDN.Where(d => d.ServiceCompanyId != invoice.ServiceCompanyId).ToList();
                var details = CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted && a.ServiceEntityId != invoice.ServiceCompanyId).GroupBy(d => d.ServiceEntityId).Select(a => a.FirstOrDefault()).ToList();
                var grpEntity = CreditNoteApplication.CreditNoteApplicationDetails.Where(a => a.CreditAmount > 0 && a.ObjectState != ObjectState.Deleted && a.ServiceEntityId != invoice.ServiceCompanyId).GroupBy(d => d.ServiceEntityId).Select(a => new { ServEntId = a.Select(c => c.ServiceEntityId), Sum = a.Sum(x => x.CreditAmount) }).ToList();
                foreach (CreditNoteApplicationDetail detail in details)
                {
                    detail.CreditAmount = grpEntity.Where(c => c.ServEntId.FirstOrDefault() == detail.ServiceEntityId).Select(c => c.Sum).FirstOrDefault();
                    JVVDetailModel journal = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    //FillCNDetails(journal, CreditNoteApplication, invoice, detail, lstIC, lstInvoice, lstAllDN);
                    FillInterCompDetails(journal, CreditNoteApplication, invoice, detail, lstIC);
                    journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(journal);

                }
            }
            #endregion For I/C Transaction

            headJournal.JVVDetailModels = lstJD.OrderBy(x => x.RecOrder).ToList();
        }

        private void FillCreditNoteICJournal(JVModel headJournal, CreditNoteApplication CreditNoteApplication, CreditNoteApplicationDetailModel applicationdetail, bool isNew, IDictionary<long, long?> lstIC, long exchageGainLossId, List<InvoiceCompact> lstAllInvoice, List<DebitNoteCompact> lstAllDN, CreditNoteApplicationModel TObject, long? ICCOAId, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            InvoiceCompact invoice = _cnApplicationService.GetinvoiceById(CreditNoteApplication.InvoiceId);
            headJournal.COAId = _masterService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            FillCNJV(headJournal, CreditNoteApplication, invoice);
            headJournal.ServiceCompanyId = lstIC.Where(c => c.Value == applicationdetail.ServiceEntityId).Select(c => c.Value).FirstOrDefault();
            headJournal.SystemReferenceNo = docNo = GetNextApplicationNumberSeries(docNo, false, invoice.DocNo);
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();

            //JVVDetailModel journalDetail = new JVVDetailModel();
            //if (isNew)
            //    journalDetail.Id = Guid.NewGuid();
            //else
            //    journalDetail.Id = applicationdetail.Id;
            //FillInterCompCCNDetails(journalDetail, CreditNoteApplication, invoice, applicationdetail, null);
            //journalDetail.RecOrder = ++recOrder;
            //lstJD.Add(journalDetail);
            //List<InvoiceCompact> lstInvoice = null;
            //List<DebitNoteCompact> lstDN = null;
            //if (lstAllInvoice.Any())
            //    lstInvoice = lstAllInvoice.Where(x => x.ServiceCompanyId == invoice.ServiceCompanyId).ToList();
            //if (lstAllDN.Any())
            //    lstDN = lstAllDN.Where(d => d.ServiceCompanyId == invoice.ServiceCompanyId).ToList();
            foreach (CreditNoteApplicationDetailModel detail in TObject.CreditNoteApplicationDetailModels.Where(a => a.CreditAmount > 0 && a.ServiceEntityId == applicationdetail.ServiceEntityId))
            {
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                FillInterCompCCNDetails(journal, CreditNoteApplication, invoice, detail, null, lstOfRoundingAmount);
                journal.BaseCurrency = invoice.ExCurrency;
                journal.ServiceCompanyId = headJournal.ServiceCompanyId.Value;
                journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                lstJD.Add(journal);
                if (CreditNoteApplication.IsRevExcess != true)
                {
                    if (invoice.DocCurrency != invoice.ExCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        journal1.DocType = invoice.DocType;
                        journal1.DocSubType = DocTypeConstants.Application;
                        journal1.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
                        journal1.AccountDescription = CreditNoteApplication.Remarks;
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.DocumentId = CreditNoteApplication.Id;
                        if (detail.DocType == DocTypeConstants.Invoice)
                        {
                            //InvoiceCompact inv = _cnApplicationService.GetinvoiceById(detail.DocumentId);
                            InvoiceCompact inv = lstAllInvoice.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                            if (inv != null)
                            {
                                journal1.Nature = inv.Nature;
                                journal1.EntityId = inv.EntityId;
                                journal1.OffsetDocument = inv.DocNo;
                                journal1.ExchangeRate = inv.ExchangeRate;
                            }
                        }
                        if (detail.DocType == DocTypeConstants.DebitNote)
                        {
                            //DebitNoteCompact inv = _cnApplicationService.GetDebitNoteById(detail.DocumentId);
                            DebitNoteCompact inv = lstAllDN.Where(c => c.Id == detail.DocumentId).FirstOrDefault();
                            if (inv != null)
                            {
                                journal1.Nature = inv.Nature;
                                journal1.EntityId = inv.EntityId;
                                journal1.OffsetDocument = inv.DocNo;
                                journal1.ExchangeRate = inv.ExchangeRate;
                            }
                        }
                        if (exchageGainLossId != 0)
                            journal1.COAId = exchageGainLossId;
                        journal1.DocCurrency = detail.DocCurrency;
                        journal1.BaseCurrency = invoice.ExCurrency;
                        //journal1.ServiceCompanyId = invoice != null ? invoice.ServiceCompanyId.Value : 0;
                        journal1.ServiceCompanyId = headJournal.ServiceCompanyId.Value;
                        journal1.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
                        journal1.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1; ;
                        if (invoice.ExchangeRate > journal1.ExchangeRate)
                        {
                            journal1.BaseCredit = Math.Round((decimal)(invoice.ExchangeRate - journal1.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (invoice.ExchangeRate < journal1.ExchangeRate)
                        {
                            journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - invoice.ExchangeRate) * detail.CreditAmount, 2, MidpointRounding.AwayFromZero);
                        }
                        if (invoice.ExchangeRate != journal1.ExchangeRate)
                        {
                            lstJD.Add(journal1);
                        }
                    }
                }
            }
            JVVDetailModel jModel = new JVVDetailModel();
            FillJVHeadIApplnDetails(jModel, CreditNoteApplication, invoice, TObject.CreditNoteApplicationDetailModels.Where(c => c.ServiceEntityId == applicationdetail.ServiceEntityId).Sum(d => d.CreditAmount));
            jModel.COAId = ICCOAId;
            jModel.ServiceCompanyId = headJournal.ServiceCompanyId.Value;
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD.OrderBy(x => x.RecOrder).ToList();
        }

        private void FillGSTSettings(JVVDetailModel jVDetail, CreditNoteApplication creditNoteApplication, InvoiceCompact invoice, CreditNoteApplicationDetail CNAppDetail)
        {
            jVDetail.DocumentDetailId = CNAppDetail.Id;
            jVDetail.DocumentId = creditNoteApplication.Id;
            jVDetail.Nature = invoice.Nature;
            jVDetail.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jVDetail.DocNo = creditNoteApplication.CreditNoteApplicationNumber;
            jVDetail.DocType = DocTypeConstants.CreditNote;
            jVDetail.DocSubType = DocTypeConstants.Application;
            jVDetail.PostingDate = creditNoteApplication.CreditNoteApplicationDate;
            jVDetail.EntityId = invoice.EntityId;
            jVDetail.COAId = _masterService.GetTaxPaybleGstCOA(creditNoteApplication.CompanyId, COANameConstants.TaxPayableGST);
            jVDetail.DocCurrency = CNAppDetail.DocCurrency;
            jVDetail.BaseCurrency = invoice.ExCurrency;
            jVDetail.ExchangeRate = invoice.ExchangeRate;
            //jVDetail.GSTExCurrency = invoice.GSTExCurrency;
            //jVDetail.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            jVDetail.GSTExchangeRate = invoice.GSTExchangeRate;
            jVDetail.AccountDescription = creditNoteApplication.Remarks;
            if (CNAppDetail.TaxId != null)
            {
                jVDetail.TaxId = CNAppDetail.TaxId;
                jVDetail.TaxCode = CNAppDetail.TaxIdCode;
                jVDetail.TaxRate = CNAppDetail.TaxRate;
            }
            jVDetail.DocCredit = CNAppDetail.TaxAmount;
            jVDetail.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)jVDetail.DocCredit : (decimal)(jVDetail.DocCredit * invoice.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            jVDetail.IsTax = true;
        }

        private void FillCNDetails(JVVDetailModel journal, CreditNoteApplication CreditNoteApplication, InvoiceCompact invoice, CreditNoteApplicationDetail detail, IDictionary<long, long?> lstIC, List<InvoiceCompact> lstInvoice, List<DebitNoteCompact> lstDNCompact, bool? isOffset, Dictionary<long, string> coaNames, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            Guid? docId = Guid.Empty;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = CreditNoteApplication.Id;
            journal.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.Receipt)
            {
                //InvoiceCompact inv = _cnApplicationService.GetinvoiceById(detail.DocumentId);
                InvoiceCompact inv = lstInvoice.Any() ? lstInvoice.Where(d => d.Id == detail.DocumentId).FirstOrDefault() : null;
                if (inv != null)
                {
                    journal.Nature = inv.Nature;
                    journal.EntityId = inv.EntityId;
                    journal.OffsetDocument = inv.DocNo;
                    journal.ExchangeRate = inv.ExchangeRate;
                    docId = inv.Id;
                    //journal.DocDate = inv.DocDate;
                }
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                //DebitNoteCompact inv = _cnApplicationService.GetDebitNoteById(detail.DocumentId);
                DebitNoteCompact inv = lstDNCompact.Any() ? lstDNCompact.Where(d => d.Id == detail.DocumentId).FirstOrDefault() : null;
                if (inv != null)
                {
                    journal.Nature = inv.Nature;
                    journal.EntityId = inv.EntityId;
                    journal.OffsetDocument = inv.DocNo;
                    journal.ExchangeRate = inv.ExchangeRate;
                    docId = inv.Id;
                    //journal.DocDate = inv.DocDate;
                }
            }
            //if (CreditNoteApplication.IsRevExcess != true)
            //{
            //    long? account1 = _masterService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            //    if (account1 != 0 && account1 != null)
            //        journal.COAId = account1;
            //}
            if (CreditNoteApplication.IsRevExcess == true)
            {
                journal.ExchangeRate = CreditNoteApplication.ExchangeRate;
                journal.EntityId = invoice.EntityId;
            }
            journal.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            journal.COAId = (isOffset == true || CreditNoteApplication.IsRevExcess == true) ? detail.COAId : lstIC == null ? journal.Nature == "Trade" ? coaNames.Where(a => a.Value == COANameConstants.AccountsReceivables).Select(a => a.Key).FirstOrDefault() : coaNames.Where(a => a.Value == COANameConstants.OtherReceivables).Select(a => a.Key).FirstOrDefault() : lstIC.Where(c => c.Value == detail.ServiceEntityId).Select(c => c.Key).FirstOrDefault();
            journal.SettlementMode = "CN Application";
            journal.SettlementRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            journal.SettlementDate = CreditNoteApplication.CreatedDate;
            //journal.ServiceCompanyId = CreditNoteApplication.IsRevExcess != true ? detail.ServiceEntityId.Value : invoice.ServiceCompanyId.Value;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.ExchangeRate = isOffset == true && invoice.ExCurrency != invoice.DocCurrency ? detail.BaseCurrencyExchangeRate : journal.ExchangeRate;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            journal.DocType = invoice.DocType;
            journal.DocSubType = DocTypeConstants.Application;
            journal.AccountDescription = CreditNoteApplication.IsRevExcess == true ? detail.DocDescription : CreditNoteApplication.Remarks;
            journal.DocCredit = detail.CreditAmount;

            //journal.BaseCredit = Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);//commented on 05/02/2019 for new approch

            journal.BaseCredit = (isOffset != true && invoice.DocCurrency != invoice.ExCurrency && lstOfRoundingAmount.Where(a => a.Key == docId).Any()) ? Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero) - (lstOfRoundingAmount.Where(a => a.Key == docId).Select(a => a.Value).FirstOrDefault()) : Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);

            journal.DocTaxCredit = (CreditNoteApplication.IsRevExcess == true && detail.TaxAmount != null) ? detail.TaxAmount : null;
            journal.BaseTaxCredit = (CreditNoteApplication.IsRevExcess == true && detail.TaxAmount != null && invoice.ExchangeRate != null) ? Math.Round((decimal)detail.TaxAmount * (decimal)(invoice.ExchangeRate == null ? 1 : invoice.ExchangeRate), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
            journal.DocCreditTotal = detail.CreditAmount;
            journal.TaxId = CreditNoteApplication.IsRevExcess == true ? detail.TaxId : null;
            journal.TaxRate = CreditNoteApplication.IsRevExcess == true ? detail.TaxRate : null;
            journal.GSTCredit = (CreditNoteApplication.IsRevExcess == true && detail.TaxAmount != null && invoice.GSTExchangeRate != null) ? Math.Round((decimal)detail.TaxAmount * (decimal)(invoice.GSTExchangeRate == null ? 1 : invoice.GSTExchangeRate), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
            journal.GSTTaxCredit = (CreditNoteApplication.IsRevExcess == true && detail.CreditAmount != null && invoice.GSTExchangeRate != null) ? Math.Round((decimal)detail.CreditAmount * (decimal)(invoice.GSTExchangeRate == null ? 1 : invoice.GSTExchangeRate), 2, MidpointRounding.AwayFromZero) : (decimal?)null;
        }
        private void FillInterCompCCNDetails(JVVDetailModel journal, CreditNoteApplication CreditNoteApplication, InvoiceCompact invoice, CreditNoteApplicationDetailModel detail, IDictionary<string, long> lstIC, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = CreditNoteApplication.Id;
            journal.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            journal.ExchangeRate = detail.BaseCurrencyExchangeRate;
            journal.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            journal.EntityId = invoice.EntityId;
            journal.COAId = lstIC == null ? detail.COAId : lstIC.Where(c => c.Value == detail.COAId).Select(c => c.Value).FirstOrDefault();
            //journal.ServiceCompanyId = detail.ServiceEntityId.Value;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            journal.DocType = invoice.DocType;
            journal.DocSubType = DocTypeConstants.Application;
            journal.AccountDescription = CreditNoteApplication.Remarks;
            journal.DocCredit = detail.CreditAmount;
            //journal.BaseCredit = Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);//commented on 05-02-2020 for new approach

            journal.BaseCredit = (invoice.DocCurrency != invoice.ExCurrency && lstOfRoundingAmount.Where(a => a.Key == detail.DocumentId).Any()) ? Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero) - (lstOfRoundingAmount.Where(a => a.Key == detail.DocumentId).Select(a => a.Value).FirstOrDefault()) : Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);

            journal.DocCreditTotal = detail.CreditAmount;
        }
        private void FillInterCompDetails(JVVDetailModel journal, CreditNoteApplication CreditNoteApplication, InvoiceCompact invoice, CreditNoteApplicationDetail detail, IDictionary<long, long?> lstIC)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = CreditNoteApplication.Id;
            journal.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            journal.ExchangeRate = CreditNoteApplication.ExchangeRate;
            journal.DocDate = invoice.DocDate;
            journal.EntityId = invoice.EntityId;
            journal.COAId = lstIC == null ? detail.COAId : lstIC.Where(c => c.Value == detail.ServiceEntityId).Select(c => c.Key).FirstOrDefault();
            //journal.ServiceCompanyId = detail.ServiceEntityId.Value;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            journal.DocType = invoice.DocType;
            journal.DocSubType = DocTypeConstants.Application;
            journal.AccountDescription = CreditNoteApplication.Remarks;
            journal.DocCredit = detail.CreditAmount;
            journal.BaseCredit = Math.Round((decimal)journal.DocCredit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            journal.DocCreditTotal = detail.CreditAmount;
            journal.BaseCurrency = invoice.ExCurrency;
        }
        private void FillJVHeadCNDetails(JVVDetailModel jModel, CreditNoteApplication CreditNoteApplication, InvoiceCompact invoice, Dictionary<Guid, decimal> lstOfRoundingAmount)
        {
            jModel.DocumentId = CreditNoteApplication.Id;
            jModel.SystemRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            //jModel.DocDate = invoice.DocDate;
            jModel.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocType = DocTypeConstants.CreditNote;
            jModel.DocSubType = DocTypeConstants.Application;
            jModel.AccountDescription = CreditNoteApplication.Remarks;
            jModel.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            //jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            jModel.DocDebit = CreditNoteApplication.CreditAmount;
            jModel.BaseDebit = (invoice.DocumentState == CreditNoteState.FullyApplied && invoice.DocCurrency != invoice.ExCurrency && lstOfRoundingAmount.Where(a => a.Key == invoice.Id).Any()) ? Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocDebit) : (decimal)jModel.DocDebit, 2, MidpointRounding.AwayFromZero) - (lstOfRoundingAmount.Where(a => a.Key == invoice.Id).Select(a => a.Value).FirstOrDefault()) : Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocDebit) : (decimal)jModel.DocDebit, 2, MidpointRounding.AwayFromZero);
            jModel.SettlementMode = "CN Application";
            jModel.SettlementRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.OffsetDocument = invoice.DocNo;
        }
        private void FillJVHeadIApplnDetails(JVVDetailModel jModel, CreditNoteApplication CreditNoteApplication, InvoiceCompact invoice, decimal? amount)
        {
            jModel.DocumentId = CreditNoteApplication.Id;
            jModel.SystemRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            //jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocType = DocTypeConstants.CreditNote;
            jModel.DocSubType = DocTypeConstants.Application;
            jModel.AccountDescription = CreditNoteApplication.Remarks;
            jModel.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            //jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            jModel.DocDebit = amount;
            jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate != null ? (decimal)(jModel.ExchangeRate * jModel.DocDebit) : (decimal)jModel.DocDebit, 2, MidpointRounding.AwayFromZero);
            jModel.SettlementMode = "CN Application";
            jModel.SettlementRefNo = CreditNoteApplication.CreditNoteApplicationNumber;
            jModel.OffsetDocument = invoice.DocNo;
        }
        private void FillCNJV(JVModel headJournal, CreditNoteApplication CreditNoteApplication, InvoiceCompact invoice)
        {
            headJournal.DocumentId = CreditNoteApplication.Id;
            //headJournal.DocumentId = invoice.Id;
            headJournal.ParentId = CreditNoteApplication.InvoiceId;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = CreditNoteApplication.CreditNoteApplicationDate;
            headJournal.DocNo = CreditNoteApplication.CreditNoteApplicationNumber;
            headJournal.DocumentDescription = CreditNoteApplication.Remarks;
            headJournal.DocType = DocTypeConstants.CreditNote;
            headJournal.DocSubType = DocTypeConstants.Application;
            //headJournal.DocDate = invoice.DocDate;
            headJournal.DocDate = CreditNoteApplication.CreditNoteApplicationDate;
            headJournal.DueDate = invoice.DueDate.Value;
            headJournal.DocumentState = CreditNoteApplication.Status.ToString();
            headJournal.SystemReferenceNo = CreditNoteApplication.CreditNoteApplicationNumber;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.Nature = invoice.Nature;
            headJournal.IsGstSettings = invoice.IsGstSettings;
            headJournal.IsMultiCurrency = invoice.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = CreditNoteApplication.IsNoSupportingDocument;
            headJournal.PONo = invoice.PONo;
            headJournal.NoSupportingDocument = CreditNoteApplication.IsNoSupportingDocumentActivated;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            headJournal.CreditTermsId = invoice.CreditTermsId;
            headJournal.DocCurrency = invoice.DocCurrency;
            headJournal.BaseCurrency = invoice.ExCurrency;
            headJournal.ExchangeRate = invoice.ExchangeRate;
            headJournal.GrandDocDebitTotal = CreditNoteApplication.CreditAmount;
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(CreditNoteApplication.CreditAmount * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.Remarks = CreditNoteApplication.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = CreditNoteApplication.CreatedDate;
            headJournal.ModifiedBy = invoice.ModifiedBy;
            headJournal.ModifiedDate = CreditNoteApplication.ModifiedDate;
        }
        #endregion Posting_Block


        #region InterCoCreditNote_Save_Block
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(
            prop.PropertyType) ?? prop.PropertyType);


            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        private void FillDocumentAndDetailType(CreditNoteApplicationModel CNAppModel, string connectionString, decimal exchangeRate, long? serviceCompanyID)
        {
            
            List<CreditNoteAppDetailTypeModel> lstCreditNoteAppTypeDetail = null;

            if (CNAppModel != null)
            {
                List<CreditNoteAppTypeModel> lstCNAppTypeModel = new List<CreditNoteAppTypeModel>();
                CreditNoteAppTypeModel CNAppTypeModel = new CreditNoteAppTypeModel();
                CNAppTypeModel.Id = CNAppModel.Id;
                CNAppTypeModel.InvoiceId = CNAppModel.InvoiceId;
                CNAppTypeModel.IsRevExcess = CNAppModel.IsRevExcess;
                CNAppTypeModel.UserCreated = CNAppModel.UserCreated;
                CNAppTypeModel.CreatedDate = CNAppModel.CreatedDate;
                CNAppTypeModel.ModifiedBy = CNAppModel.ModifiedBy;
                CNAppTypeModel.ModifiedDate = CNAppModel.ModifiedDate;
                CNAppTypeModel.Remarks = CNAppModel.Remarks;
                CNAppTypeModel.Status = 1;
                CNAppTypeModel.ExchangeRate = exchangeRate;
                CNAppTypeModel.CreditNoteApplicationDate = CNAppModel.CreditNoteApplicationDate;
                CNAppTypeModel.CreditNoteApplicationResetDate = CNAppModel.CreditNoteApplicationResetDate;
                CNAppTypeModel.CreditNoteApplicationNumber = CNAppModel.CreditNoteApplicationNumber;
                CNAppTypeModel.CreditAmount = CNAppModel.CreditAmount;
                CNAppTypeModel.CompanyId = CNAppModel.CompanyId;
                lstCNAppTypeModel.Add(CNAppTypeModel);
                int recOrder = 0;
                if (CNAppModel.IsRevExcess != true)
                    lstCreditNoteAppTypeDetail = CNAppModel.CreditNoteApplicationDetailModels.Select(a => new CreditNoteAppDetailTypeModel
                    {
                        Id = a.Id,
                        COAId = a.COAId,
                        TaxId = null,
                        TaxRate = null,
                        TaxAmount = null,
                        TotalAmount = a.CreditAmount,
                        DocumentId = a.DocumentId,
                        DocumentType = a.DocType,
                        DocNo = a.DocNo,
                        DocDescription = null,
                        CreditAmount = a.CreditAmount,
                        BaseCurrencyExchangeRate = Convert.ToDecimal(a.BaseCurrencyExchangeRate),
                        ServiceEntityId = a.ServiceEntityId,
                        CreditNoteApplicationId = CNAppModel.Id,
                        DocCurrency = a.DocCurrency,
                        RecOrder = recOrder++,
                        TaxIdCode = null,

                    }).ToList();
                if (CNAppModel.IsRevExcess == true)
                    lstCreditNoteAppTypeDetail = CNAppModel.ReverseExcessModels.Select(a => new CreditNoteAppDetailTypeModel
                    {
                        Id = a.Id,
                        COAId = a.COAId,
                        TaxId = a.TaxId,
                        TaxRate = a.TaxRate,
                        TaxAmount = a.DocTaxAmount,
                        TotalAmount = a.DocTotalAmount,
                        DocumentId = new Guid(),
                        DocumentType = string.Empty,
                        DocNo = string.Empty,
                        DocDescription = null,
                        CreditAmount = a.DocAmount,
                        BaseCurrencyExchangeRate = exchangeRate,
                        ServiceEntityId = serviceCompanyID,
                        CreditNoteApplicationId = CNAppModel.Id,
                        DocCurrency = CNAppModel.DocCurrency,
                        RecOrder = recOrder++,
                        TaxIdCode = a.TaxIdCode,

                    }).ToList();
                SaveInterCoCreditNote(lstCNAppTypeModel, lstCreditNoteAppTypeDetail, connectionString);

            }

        }


        private static void SaveInterCoCreditNote(List<CreditNoteAppTypeModel> lstCNAppType, List<CreditNoteAppDetailTypeModel> lstCNAppDetailType, string connectionString)
        {

            SqlConnection con;
            SqlDataReader dr;
            SqlCommand cmd;
            using (con = new SqlConnection(connectionString))
            {
                try
                {
                    DataSet ds = new DataSet();
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    cmd = new SqlCommand("Bean_Save_InterCo_CN", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter CNAppmaster = new SqlParameter();
                    CNAppmaster.ParameterName = "@CreditNoteApp";
                    CNAppmaster.TypeName = "dbo.CreditNoteApplication";
                    CNAppmaster.Value = ToDataTable(lstCNAppType);
                    cmd.Parameters.Add(CNAppmaster);

                    SqlParameter CNAppdetail = new SqlParameter();
                    CNAppdetail.ParameterName = "@CreditNoteAppDetial";
                    CNAppdetail.TypeName = "dbo.CreditNoteApplicationDetail";
                    CNAppdetail.Value = ToDataTable(lstCNAppDetailType);
                    cmd.Parameters.Add(CNAppdetail);

                    SqlDataAdapter sqlDA = new SqlDataAdapter();

                    sqlDA.SelectCommand = cmd;
                    sqlDA.Fill(ds);

                    con.Close();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

    }

}
