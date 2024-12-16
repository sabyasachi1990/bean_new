using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.PaymentModule.Entities;
using AppsWorld.PaymentModule.Models;
using AppsWorld.PaymentModule.RepositoryPattern;
using AppsWorld.PaymentModule.Service;
using AppsWorld.PaymentModule.Infra;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Service;
using AppsWorld.CommonModule.Entities;
using System.Configuration;
using FrameWork;
using Repository.Pattern.Infrastructure;
using System.Data.Entity.Validation;
using AppsWorld.CommonModule.Models;
using Logger;
using Serilog;
using Domain.Events;
using RestSharp;
using AppsWorld.CommonModule.Infra;
using AppsWorld.PaymentModule.Infra.Resources;
using Ziraff.Section;
using System.Data.SqlClient;
using System.Data;
using Ziraff.FrameWork;
using Ziraff.FrameWork.Logging;
using AppaWorld.Bean;
using System.Globalization;
using AppsWorld.CommonModule.Application;
using System.Net;
using Newtonsoft.Json;

namespace AppsWorld.PaymentModule.Application
{
    public class PaymentApplicationSevice
    {
        private readonly IPaymentService _paymentService;
        private readonly IPaymentDetailService _paymentDetailService;
        private readonly IBillService _billService;
        private readonly ICompanyService _companyService;
        private readonly ICurrencyService _currencyService;
        private readonly IControlCodeCategoryService _controlCodeCategoryService;
        private readonly IChartOfAccountService _chartOfAccountService;
        private readonly IAccountTypeService _accountTypeService;
        private readonly IFinancialSettingService _financialSettingService;
        private readonly IPaymentModuleUnitOfWorkAsync _unitOfWorkAsync;
        //private readonly IControlCodeService _controlCodeService;
        private readonly IBeanEntityService _beanEntityService;
        //private readonly IGSTSettingService _gstSettingService;
        private readonly ICompanySettingService _companySettingService;
        //private readonly IMultiCurrencySettingService _multiCurrencySettingService;
        //private readonly IBankReconciliationSettingService _bankReconciliationService;
        //private readonly AppsWorld.PaymentModule.Service.IAutoNumberService _autoNumberService;
        //private readonly AppsWorld.PaymentModule.Service.IAutoNumberCompanyService _autoNumberCompanyService;
        //private readonly IJournalService _journalService;
        //private readonly IJournalDetailService _journalDetailService;
        //private readonly IFeatureService _featureService;
        private readonly ICompanyFeatureService _companyFeatureService;
        private readonly IJournalServices _journalServices;
        private readonly AppsWorld.CommonModule.Service.IAutoNumberService _autoService;
        private readonly IInvoiceCompactService _invoiceCompactService;
        private readonly IDebitNoteCompactService _debitNoteCompactService;
        private readonly ICreditMemoCompactService _creditMemoCompactService;
        private readonly CommonApplicationService _commonApplicationService;
        string doc = "";
        SqlCommand cmd = null;
        SqlConnection con = null;
        SqlDataReader dr = null;
        string query = string.Empty;


        #region Constroctor
        public PaymentApplicationSevice(IPaymentService paymentService, IPaymentDetailService paymentDetailService, IBillService billSrvice, ICompanyService companyService, ICurrencyService currencyService, IControlCodeCategoryService controlCodeCategoryService, IPaymentModuleUnitOfWorkAsync unitOfWorkAsync, IControlCodeService controlCodeService, IBeanEntityService beanEntityService, ICompanySettingService companySettingService, IAccountTypeService accountTypeService, IChartOfAccountService chartOfAccountService, IFinancialSettingService financialSettingService, AppsWorld.PaymentModule.Service.IAutoNumberService autoNumberService, AppsWorld.PaymentModule.Service.IAutoNumberCompanyService autoNumberCompanyService, ICompanyFeatureService companyFeatureService, IJournalServices journalServices, AppsWorld.CommonModule.Service.IAutoNumberService autoService, IInvoiceCompactService invoiceCompactService, IDebitNoteCompactService debitNoteCompactService, ICreditMemoCompactService creditMemoCompactService, CommonApplicationService commonApplicationService)
        {
            this._paymentService = paymentService;
            this._paymentDetailService = paymentDetailService;
            this._billService = billSrvice;
            this._companyService = companyService;
            this._currencyService = currencyService;
            this._controlCodeCategoryService = controlCodeCategoryService;
            this._unitOfWorkAsync = unitOfWorkAsync;
            this._beanEntityService = beanEntityService;
            this._companySettingService = companySettingService;
            this._accountTypeService = accountTypeService;
            this._chartOfAccountService = chartOfAccountService;
            this._financialSettingService = financialSettingService;
            this._companyFeatureService = companyFeatureService;
            this._journalServices = journalServices;
            this._autoService = autoService;
            this._invoiceCompactService = invoiceCompactService;
            this._debitNoteCompactService = debitNoteCompactService;
            this._creditMemoCompactService = creditMemoCompactService;
            this._commonApplicationService = commonApplicationService;

        }
        #endregion

        #region Kendo Grid Call
        public IQueryable<PaymentModelK> GetAllPaymentsK(string username, long companyId)
        {
            return _paymentService.GetAllPaymentK(username, companyId, DocTypeConstants.General);
        }
        #endregion

        #region Create Call and LookUp Call
        public PaymentModelLU GetAllPaymentLUs(Guid paymentId, long companyId, string userName)
        {
            Payment payment = _paymentService.GetPayment(paymentId, companyId);
            PaymentModelLU paymentLU = new PaymentModelLU();
            paymentLU.ModeOfPaymentLU = _controlCodeCategoryService.GetByCategoryCodeCategory(companyId,
                ControlCodeConstants.Control_codes_ModeOfTransfer);
            paymentLU.CompanyId = companyId;
            if (payment != null)
            {
                string currencyCode = payment.DocCurrency;
                paymentLU.CurrencyLU = _currencyService.GetByCurrenciesEdit(companyId, currencyCode,
                    ControlCodeConstants.Currency_DefaultCode);

                //added by lokanath
                var lookupcategory = _controlCodeCategoryService.GetInactiveControlcode(companyId, ControlCodeConstants.Control_codes_ModeOfTransfer, payment.ModeOfPayment);
                if (lookupcategory != null)
                {
                    paymentLU.ModeOfPaymentLU.Lookups.Add(lookupcategory);
                }
            }
            else
            {
                paymentLU.CurrencyLU = _currencyService.GetByCurrencies(companyId, ControlCodeConstants.Currency_DefaultCode);
            }
            paymentLU.NatureLU = new List<string> { "Trade", "Others" };
            long comp = payment == null ? 0 : payment.ServiceCompanyId;
            List<long> coaIds = new List<long>();
            if (payment != null)
                coaIds.Add(payment.COAId);
            paymentLU.SubsideryCompanyLU = _companyService.ListOfSubsudaryCompanyActiveInactive(companyId, comp, paymentId, payment != null ? coaIds : coaIds, userName);
            return paymentLU;
        }

        public CurrencyLU GetCurrencyLookup(Guid entityId, long companyId, string baseCurrency, string bankCurrency, bool isMultyCurrency, Guid paymentId)
        {

            CurrencyLU currencyLU = new CurrencyLU();
            List<string> allCurrencies = new List<string>();
            List<string> lstBillsCurr = null;
            if (isMultyCurrency)
            {
                if (baseCurrency == bankCurrency)
                {

                    //SP
                    if (paymentId == new Guid())
                        lstBillsCurr = _invoiceCompactService.GetByEntityId(entityId, companyId);
                    else
                        lstBillsCurr = _invoiceCompactService.GetByStateandEntity(entityId, companyId);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);
                    if (paymentId == new Guid())
                        lstBillsCurr = _debitNoteCompactService.GetByEntityId(entityId, companyId);
                    else
                        lstBillsCurr = _debitNoteCompactService.GetByIdState(entityId, companyId);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);

                    if (paymentId == new Guid())
                        lstBillsCurr = _billService.GetByEntityId(entityId, companyId);
                    else
                        lstBillsCurr = _billService.GetByEntityIdState(entityId, companyId);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);

                    //Credit Memo
                    if (paymentId == new Guid())
                        lstBillsCurr = _creditMemoCompactService.GetByCreditMemoId(entityId, companyId);
                    else
                        lstBillsCurr = _creditMemoCompactService.GetByStateandCreditMemoEntity(entityId, companyId);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);
                    if (paymentId != new Guid())
                        lstBillsCurr = _paymentDetailService.GetByPaymentId(paymentId).Select(c => c.Currency).ToList();
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);
                    if (allCurrencies.Any())
                    {
                        currencyLU.CurrencyLu = allCurrencies.GroupBy(d => d).Select(c => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                        {
                            Code = c.Key
                        }).ToList();
                    }
                }
                else
                {

                    if (paymentId == new Guid())
                        lstBillsCurr = _invoiceCompactService.GetAllInvoiceByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                    else
                        lstBillsCurr = _invoiceCompactService.GetAllInvoiceByEntityIdState(entityId, companyId, baseCurrency, bankCurrency);

                    allCurrencies.AddRange(lstBillsCurr);
                    if (paymentId == new Guid())
                        lstBillsCurr = _debitNoteCompactService.GetAllDNByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                    else
                        lstBillsCurr = _debitNoteCompactService.GetAllDNByEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);



                    if (paymentId == new Guid())
                        lstBillsCurr = _billService.GetAllCurrencyByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                    else
                        lstBillsCurr = _billService.GetAllCurrencyByEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);
                    //Credit Memo
                    if (paymentId == new Guid())
                        lstBillsCurr = _creditMemoCompactService.GetAllCreditMemoByEntityId(entityId, companyId, baseCurrency, bankCurrency);
                    else
                        lstBillsCurr = _creditMemoCompactService.GetAllCreditMemoEntityIdState(entityId, companyId, baseCurrency, bankCurrency);
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);
                    if (paymentId != new Guid())
                        lstBillsCurr = _paymentDetailService.GetByPaymentId(paymentId).Select(c => c.Currency).ToList();
                    if (lstBillsCurr.Any())
                        allCurrencies.AddRange(lstBillsCurr);
                    allCurrencies.Add(baseCurrency);
                    if (allCurrencies.Any())
                    {
                        currencyLU.CurrencyLu = allCurrencies.GroupBy(d => d).Select(c => new AppsWorld.CommonModule.Infra.LookUpCategory<string>()
                        {
                            Code = c.Key
                        }).ToList();
                    }
                }
            }
            else
            {
                List<AppsWorld.CommonModule.Infra.LookUpCategory<string>> lstLookup1 = new List<AppsWorld.CommonModule.Infra.LookUpCategory<string>>();
                AppsWorld.CommonModule.Infra.LookUpCategory<string> lookup = new AppsWorld.CommonModule.Infra.LookUpCategory<string>();
                lookup.Code = baseCurrency;
                lstLookup1.Add(lookup);
                currencyLU.CurrencyLu = lstLookup1;
            }
            if (!currencyLU.CurrencyLu.Any() && currencyLU.CurrencyLu.Count == 0)
                currencyLU.CurrencyLu.Add(new CommonModule.Infra.LookUpCategory<string>() { Code = bankCurrency });
            return currencyLU;
        }
        public PaymentModel CreatePayment(Guid id, long companyId, string username, string docType, string connectionString)
        {
            PaymentModel paymentModel = new PaymentModel();
            FinancialSetting financSettings = _financialSettingService.GetFinancialSetting(companyId);
            if (financSettings == null)
            {
                throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
            }
            paymentModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
            paymentModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
            Payment payment = _paymentService.GetPayments(id, companyId, docType);
            if (payment != null)
            {
                if (!_companyService.GetPermissionBasedOnUser(payment.ServiceCompanyId, payment.CompanyId, username))
                    throw new Exception(CommonConstant.Access_denied);
                FillPaymentModel(paymentModel, payment);
                //paymentModel.IsDocNoEditable = _autoNo != null ? _autoNo.IsEditable : null;
                paymentModel.IsDocNoEditable = _autoService.GetAutoNumberIsEditable(companyId, docType == "Payroll" ? "Payroll Payment" : "Payment");
                paymentModel.PaymentDetailModels = /*payment.DocumentState != PaymentState.Void ?*/ CreatePaymentModel(id, companyId, payment.DocDate, payment.DocSubType, username).Where(x => x.PaymentAmount != 0).OrderBy(x => x.DocumentDate).ThenBy(x => x.SystemReferenceNumber).ToList() /*: CreatePaymentModel(id, companyId, payment.DocDate, payment.DocSubType)*/;
                string name = _beanEntityService.GetEntityName(payment.CompanyId, payment.EntityId);
                string DocNo = _commonApplicationService.StringCharactersReplaceFunction(payment.DocNo);
                string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                //   name.Replace('"', '_').Replace('\\', '_').Replace('/', '_')
                //.Replace(':', '_').Replace('|', '_').Replace('<', '_').Replace('>', '_').Replace('*', '_').Replace('?', '_').Replace("'", "_");
                paymentModel.Path = DocumentConstants.Entities + "/" + EntityName + "/" + payment.DocType + "s" + "/" + DocNo;
                paymentModel.IsLocked = payment.IsLocked;
            }
            else
            {
                FillNewPaymentModel(paymentModel, financSettings, docType/*, _autoNo*/);
                paymentModel.IsDocNoEditable = _autoService.GetAutoNumberIsEditable(companyId, docType == "Payroll" ? "Payroll Payment" : "Payment");
                if (paymentModel.IsDocNoEditable == true)
                    paymentModel.DocNo = _autoService.GetAutonumber(companyId, docType == "Payroll" ? "Payroll Payment" : "Payment", connectionString);
                paymentModel.IsLocked = false;
            }
            return paymentModel;
        }
        public List<PaymentDetailModel> GetPaymentDetails(Guid paymentId, Guid entityId, string currency, long companyId, long serviceCompanyId, string username, DateTime? docDate, string docType, bool isInterCompanyActive, bool? isCustomer, string ConnectionString)
        {
            List<PaymentDetailModel> lstPDetailModel = new List<PaymentDetailModel>();
            List<PaymentDetail> lstPaymentDetail = null;
            Dictionary<long, string> lstComapny = null;
            Dictionary<long, string> lstSubComapny = null;
            Dictionary<long, string> lstComp = null;
            Dictionary<long, string> lstComapny1 = new Dictionary<long, string>();

            long? serviceEntityId = null;
            string companyName = null;
            bool isIC = false;
            #region Interco_ServiceEntity_Settings_Changes
            using (con = new SqlConnection(ConnectionString))
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                query = $"Select ICG.ServiceEntityId as ServiceEntityId,COM.ShortName as Name from Bean.InterCompanySetting IC JOIN Bean.InterCompanySettingDetail ICG on IC.Id=ICG.InterCompanySettingId Left JOIN Common.Company COM On ICG.ServiceEntityId = COM.Id JOIN Common.CompanyUser CU on CU.CompanyId = COM.ParentId JOIN Common.CompanyUserDetail CUD on CUD.CompanyUserId = CU.Id and CUD.ServiceEntityId = COM.Id where COM.ParentId = {companyId} and IC.InterCompanyType = 'Clearing' and ICG.Status=1 and CU.username ='{username}'";
                cmd = new SqlCommand(query, con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        serviceEntityId = dr["ServiceEntityId"] != null ? Convert.ToInt64(dr["ServiceEntityId"]) : (long?)null;
                        companyName = dr["Name"].ToString();
                        if (serviceEntityId != null)
                            lstComapny1.Add(serviceEntityId.Value, companyName);
                    }
                }
            }

            if (lstComapny1.Any())
                isIC = lstComapny1.Any(c => c.Key == serviceCompanyId) ? true : false;

            #endregion Interco_ServiceEntity_Settings_Changes

            if (isInterCompanyActive)
                //lstPaymentDetail = _paymentDetailService.GetByPaymentId(paymentId);
                lstPaymentDetail = _paymentDetailService.GetByPaymentDetailById(paymentId, docDate, currency);
            else
                lstPaymentDetail = _paymentDetailService.GetByPaymentIdServiceId(paymentId, serviceCompanyId, docDate, currency);

            if (lstPaymentDetail.Any())
            {
                List<long> servEntIds = new List<long>();
                //if (isInterCompanyActive == true)
                //    lstComapny = _companyService.GetAllCompaniesNameByParentId(companyId, true, true);
                //else
                //{
                servEntIds = lstPaymentDetail.Select(a => a.ServiceCompanyId.Value).Distinct().ToList();
                servEntIds.Add(serviceCompanyId);
                lstComapny = _companyService.GetAllCompaniesName(servEntIds.Distinct().ToList());

                // }
                lstSubComapny = _companyService.GetAllSubCompanies(servEntIds.Distinct().ToList(), username, companyId);
                lstComp = lstComapny.Except(lstSubComapny).ToDictionary(Id => Id.Key, Name => Name.Value);
            }
            //else
            //{
            //    if (isInterCompanyActive == true)
            //    {
            //        lstComapny = _companyService.GetAllCompaniesNameByParentId(companyId, true, false);
            //    }
            //    else
            //        lstComapny = _companyService.GetAllCompaniesNameByParentId(companyId, false, false);
            //}

            //lstComapny = _companyService.GetAllCompaniesNameByParentId(companyId);
            //var lstComapnies = _companyService.GetAllCompanies(lstPaymentDetail.Select(c => c.ServiceCompanyId.Value).ToList());
            //Payment payment = _paymentService.GetPayments(paymentId, companyId, docType);
            Payment payment = _paymentService.GetPaymentsById(paymentId, companyId, docType, entityId);
            if (lstPaymentDetail.Count > 0 && payment != null)
            {
                List<Bill> lstBills = _billService.GetAllBillsByDocId(lstPaymentDetail.Select(c => c.DocumentId).ToList(), lstPaymentDetail.Select(c => c.DocumentType).ToList(), currency, entityId);

                List<CreditMemoCompact> lstCreditMemo = _creditMemoCompactService.GetListOfCreditMemos(payment.CompanyId, DocTypeConstants.BillCreditMemo, payment.EntityId, payment.PaymentDetails.Where(a => a.DocumentType == DocTypeConstants.BillCreditMemo && a.PaymentAmount > 0).Select(a => a.DocumentId).ToList());
                Dictionary<long, RecordStatusEnum> entityStatus = _companyService.GetAllCompaniesStatus(payment.PaymentDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                List<InvoiceCompact> lstInvoiceAndCNs = null;
                List<DebitNoteCompact> lstDebitNotes = null;
                if (isCustomer == true && docType != DocTypeConstants.Payroll)
                {
                    lstInvoiceAndCNs = _invoiceCompactService.GetListOfInvoiceAndCN(payment.CompanyId, payment.PaymentDetails.Where(a => (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.CreditNote) /*&& a.PaymentAmount > 0*/).Select(a => a.DocumentId).ToList(), payment.EntityId, payment.DocCurrency);
                    lstDebitNotes = _debitNoteCompactService.GetListOfDebitNotes(payment.CompanyId, DocTypeConstants.DebitNote, payment.EntityId, payment.PaymentDetails.Where(a => a.DocumentType == DocTypeConstants.DebitNote /*&& a.PaymentAmount > 0*/).Select(a => a.DocumentId).ToList());
                }

                foreach (var pDetail in lstPaymentDetail)
                {
                    PaymentDetailModel PDetailModel = new PaymentDetailModel();
                    PDetailModel.Id = pDetail.Id;
                    if (pDetail.ServiceCompanyId != null)
                    {
                        PDetailModel.ServiceCompanyName = lstComapny != null ? lstComapny.Where(a => a.Key == pDetail.ServiceCompanyId).Select(a => a.Value).FirstOrDefault() : null;
                        PDetailModel.ServiceCompanyId = lstComapny != null ? lstComapny.Where(a => a.Key == pDetail.ServiceCompanyId).Select(a => a.Key).FirstOrDefault() : (long?)null;
                        PDetailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == pDetail.ServiceCompanyId).Any() ? false : true : true;
                    }
                    if (entityStatus.Any())
                        PDetailModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == pDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                    if (pDetail.DocumentType == DocTypeConstants.General || pDetail.DocumentType == DocTypeConstants.PayrollBill || pDetail.DocumentType == DocTypeConstants.OpeningBalance || pDetail.DocumentType == DocTypeConstants.Claim)
                    {
                        //var bill = _billService.GetBillById(pDetail.DocumentId, companyId);
                        Bill bill = lstBills.Where(c => c.Id == pDetail.DocumentId).FirstOrDefault();
                        if (bill != null)
                        {
                            //var company = _companyService.GetById(bill.ServiceCompanyId);

                            //var code = _companyService.GetCompanyByCompanyid(bill.ServiceCompanyId);
                            //if (code != null)
                            //    PDetailModel.ServiceCode = code.ShortName;
                            //FillPaymentDetailModel(PDetailModel, bill, pDetail, true, false);
                            PDetailModel.PaymentId = paymentId;
                            PDetailModel.DocumentDate = bill.PostingDate;
                            PDetailModel.DocumentNo = bill.DocNo;
                            PDetailModel.DocumentState = bill.DocumentState;
                            PDetailModel.DocumentAmmount = bill.GrandTotal;
                            PDetailModel.Nature = bill.Nature;
                            PDetailModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? bill.BalanceAmount.Value : bill.BalanceAmount.Value + pDetail.PaymentAmount;
                            PDetailModel.PaymentAmount = pDetail.PaymentAmount;
                            PDetailModel.SystemReferenceNumber = bill.SystemReferenceNumber;
                            PDetailModel.Currency = bill.DocCurrency;
                            PDetailModel.BaseExchangeRate = bill.ExchangeRate.Value;
                            PDetailModel.DocumentId = pDetail.DocumentId;
                            PDetailModel.DocumentType = pDetail.DocumentType;
                            lstPDetailModel.Add(PDetailModel);
                        }
                    }
                    else if (pDetail.DocumentType == DocTypeConstants.BillCreditMemo)
                    {
                        CreditMemoCompact creditMemo = lstCreditMemo.Where(a => a.Id == pDetail.DocumentId).FirstOrDefault();
                        if (creditMemo != null)
                        {
                            PDetailModel.DocumentDate = creditMemo.PostingDate;
                            PDetailModel.PaymentId = paymentId;
                            PDetailModel.DocumentNo = creditMemo.DocNo;
                            PDetailModel.DocumentState = creditMemo.DocumentState;
                            PDetailModel.DocumentAmmount = creditMemo.GrandTotal;
                            PDetailModel.Nature = creditMemo.Nature;
                            PDetailModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? -creditMemo.BalanceAmount : -(creditMemo.BalanceAmount + pDetail.PaymentAmount);
                            PDetailModel.PaymentAmount = -pDetail.PaymentAmount;
                            PDetailModel.SystemReferenceNumber = creditMemo.DocNo;
                            PDetailModel.Currency = creditMemo.DocCurrency;
                            PDetailModel.DocumentId = pDetail.DocumentId;
                            PDetailModel.DocumentType = pDetail.DocumentType;
                            lstPDetailModel.Add(PDetailModel);
                        }
                    }
                    if (payment.IsCustomer == true)
                    {
                        if (pDetail.DocumentType == DocTypeConstants.Invoice || pDetail.DocumentType == DocTypeConstants.CreditNote || docType != DocTypeConstants.Payroll)
                        {
                            InvoiceCompact invoiceAndCn = lstInvoiceAndCNs != null ? lstInvoiceAndCNs.Where(a => a.Id == pDetail.DocumentId).FirstOrDefault() : null;
                            if (invoiceAndCn != null)
                            {
                                PDetailModel.DocumentDate = invoiceAndCn.DocDate;
                                PDetailModel.DocumentNo = invoiceAndCn.DocNo;
                                PDetailModel.DocumentState = invoiceAndCn.DocumentState;
                                PDetailModel.PaymentId = paymentId;
                                PDetailModel.DocumentAmmount = pDetail.DocumentType == DocTypeConstants.Invoice ? -invoiceAndCn.GrandTotal : invoiceAndCn.GrandTotal;
                                PDetailModel.Nature = invoiceAndCn.Nature;
                                PDetailModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? invoiceAndCn.DocType == DocTypeConstants.Invoice ? -invoiceAndCn.BalanceAmount : invoiceAndCn.BalanceAmount : invoiceAndCn.DocType == DocTypeConstants.Invoice ? -(invoiceAndCn.BalanceAmount + pDetail.PaymentAmount) : (invoiceAndCn.BalanceAmount + pDetail.PaymentAmount);
                                PDetailModel.PaymentAmount = invoiceAndCn.DocType == DocTypeConstants.Invoice ? -pDetail.PaymentAmount : pDetail.PaymentAmount;
                                PDetailModel.SystemReferenceNumber = invoiceAndCn.DocNo;
                                PDetailModel.Currency = invoiceAndCn.DocCurrency;
                                PDetailModel.DocumentId = pDetail.DocumentId;
                                PDetailModel.DocumentType = pDetail.DocumentType;
                                lstPDetailModel.Add(PDetailModel);
                            }
                        }
                        DebitNoteCompact debitNote = lstDebitNotes != null ? lstDebitNotes.Where(a => a.Id == pDetail.DocumentId).FirstOrDefault() : null;
                        if (debitNote != null)
                        {
                            PDetailModel.DocumentDate = debitNote.DocDate;
                            PDetailModel.DocumentNo = debitNote.DocNo;
                            PDetailModel.PaymentId = paymentId;
                            PDetailModel.DocumentState = debitNote.DocumentState;
                            PDetailModel.DocumentAmmount = -debitNote.GrandTotal;
                            PDetailModel.Nature = debitNote.Nature;
                            PDetailModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? -debitNote.BalanceAmount : -(debitNote.BalanceAmount + pDetail.PaymentAmount);
                            PDetailModel.PaymentAmount = -pDetail.PaymentAmount;
                            PDetailModel.SystemReferenceNumber = debitNote.DocNo;
                            PDetailModel.Currency = debitNote.DocCurrency;
                            PDetailModel.DocumentId = pDetail.DocumentId;
                            PDetailModel.DocumentType = pDetail.DocumentType;
                            lstPDetailModel.Add(PDetailModel);
                        }
                    }
                }
                //lstPDetailModel.Where(c => c.PaymentAmount > 0).OrderBy(c => c.DocumentDate).ThenBy(d => d.SystemReferenceNumber).ToList();
            }
            //var interCompany = from feature in _featureService.Queryable().Where(c => c.ModuleId == null && c.Name == "Inter-Company")
            //                   join companyFeature in _companyFeatureService.Queryable()
            //                   on feature.Id equals companyFeature.FeatureId
            //                   select companyFeature.Status;
            //Feature feature = _featureService.Queryable().Where(c => c.ModuleId == null && c.Name == "Inter-Company").FirstOrDefault();
            //CompanyFeature companyFeature = _companyFeatureService.Query(c => c.FeatureId == feature.Id && c.CompanyId == companyId).Select().FirstOrDefault();
            //if (/*lstPaymentDetail == null || lstPaymentDetail.Count == 0*/payment == null)
            //{
            List<Bill> lstBill = new List<Bill>();
            if (/*!isInterCompanyActive &&*/ !isIC)
                lstBill = _billService.GetBillByEntity(docType == DocTypeConstants.Payment ? DocTypeConstants.General : DocTypeConstants.PayrollBill, companyId,
                     entityId, currency, serviceCompanyId, docDate);
            else
                lstBill = _billService.InterCompany(docType == DocTypeConstants.Payment ? DocTypeConstants.General : DocTypeConstants.PayrollBill, companyId,
                     entityId, currency, docDate);
            //List<Company> lstComapny = null;
            //if (lstBill.Any())
            //    lstComapny = _companyService.GetAllCompanies(lstBill.Select(c => c.ServiceCompanyId).ToList());
            if (lstBill.Any())
            {
                lstSubComapny = _companyService.GetAllSubCompanies(lstBill.Select(c => c.ServiceCompanyId).ToList(), username, companyId);
                lstComp = lstComapny1.Except(lstSubComapny).ToDictionary(Id => Id.Key, Name => Name.Value);
            }
            foreach (Bill detail in isIC ? lstBill.Where(a => lstComapny1.Keys.Contains(a.ServiceCompanyId)) : lstBill)
            {
                var d = lstPDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                if (d == null)
                {
                    //Company company = _companyService.GetById(detail.ServiceCompanyId);
                    //PaymentDetail pDetail = new PaymentDetail();
                    PaymentDetailModel detailModel = new PaymentDetailModel();
                    //FillPaymentDetailModel(detailModel, detail, pDetail, false, false);
                    detailModel.ServiceCompanyName = lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Value).FirstOrDefault() : null;
                    detailModel.ServiceCompanyId = isIC && lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Key).FirstOrDefault() : serviceCompanyId;
                    detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId).Any() ? false : true : true;
                    detailModel.DocumentNo = detail.DocNo;
                    detailModel.DocumentType = detail.DocSubType;
                    detailModel.DocumentState = detail.DocumentState;
                    detailModel.DocumentId = detail.Id;
                    detailModel.DocumentDate = detail.PostingDate;
                    detailModel.DocumentAmmount = detail.GrandTotal;
                    detailModel.Currency = detail.DocCurrency;
                    detailModel.BaseExchangeRate = detail.ExchangeRate;
                    detailModel.AmmountDue = detail.BalanceAmount;
                    detailModel.Nature = detail.Nature;
                    //detailModel.ServiceCompanyId = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.Id).FirstOrDefault();
                    //detailModel.ServiceCompanyName = lstComapny.Where(c => c.Id == detail.ServiceCompanyId).Select(c => c.ShortName).FirstOrDefault(); ;
                    detailModel.SystemReferenceNumber = detail.SystemReferenceNumber;
                    lstPDetailModel.Add(detailModel);
                }
            }
            if (docType != DocTypeConstants.Payroll)
            {
                List<CreditMemoCompact> lstCreditMemeoCompact = new List<CreditMemoCompact>();
                if (/*!isInterCompanyActive &&*/ !isIC)
                    lstCreditMemeoCompact = _creditMemoCompactService.GetListOfCreditMemoWithOutInter(companyId, serviceCompanyId, currency, entityId, docDate.Value);
                else
                    lstCreditMemeoCompact = _creditMemoCompactService.GetListOfCreditMemoWithInter(companyId, currency, entityId, docDate.Value);
                if (lstCreditMemeoCompact.Any())
                {
                    lstSubComapny = _companyService.GetAllSubCompanies(lstCreditMemeoCompact.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId);
                    lstComp = lstComapny1.Except(lstSubComapny).ToDictionary(Id => Id.Key, Name => Name.Value);
                }
                if (lstCreditMemeoCompact.Any())
                {
                    foreach (CreditMemoCompact detail in isIC ? lstCreditMemeoCompact.Where(a => lstComapny1.Keys.Contains(a.ServiceCompanyId.Value)) : lstCreditMemeoCompact)
                    {
                        PaymentDetailModel pDeatil = lstPDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                        if (pDeatil == null)
                        {
                            PaymentDetailModel detailModel = new PaymentDetailModel();
                            detailModel.ServiceCompanyName = lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Value).FirstOrDefault() : null;
                            detailModel.ServiceCompanyId = isIC && lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Key).FirstOrDefault() : serviceCompanyId;
                            detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId.Value).Any() ? false : true : true;
                            detailModel.DocumentDate = detail.PostingDate;
                            detailModel.DocumentNo = detail.DocNo;
                            detailModel.DocumentState = detail.DocumentState;
                            detailModel.DocumentAmmount = -detail.GrandTotal;
                            detailModel.Nature = detail.Nature;
                            detailModel.AmmountDue = -detail.BalanceAmount;
                            //detailModel.PaymentAmount = -pDetail.PaymentAmount;
                            detailModel.SystemReferenceNumber = detail.DocNo;
                            detailModel.Currency = detail.DocCurrency;
                            detailModel.DocumentId = detail.Id;
                            detailModel.DocumentType = detail.DocType;
                            lstPDetailModel.Add(detailModel);
                        }
                    }
                }
                if (isCustomer == true)
                {
                    List<InvoiceCompact> lstInvAndCNs = new List<InvoiceCompact>();
                    if (/*!isInterCompanyActive && */!isIC)
                        lstInvAndCNs = _invoiceCompactService.GetListOfInvoiceAndCNWithOutInter(companyId, serviceCompanyId, entityId, currency, docDate);
                    else
                        lstInvAndCNs = _invoiceCompactService.GetListOfInvoiceAndCNWithInter(companyId, entityId, currency, docDate);
                    if (lstInvAndCNs.Any())
                    {
                        lstSubComapny = _companyService.GetAllSubCompanies(lstInvAndCNs.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId);
                        lstComp = lstComapny1.Except(lstSubComapny).ToDictionary(Id => Id.Key, Name => Name.Value);
                    }
                    if (lstInvAndCNs.Any())
                    {
                        foreach (InvoiceCompact detail in isIC ? lstInvAndCNs.Where(a => ((a.DocType == DocTypeConstants.Invoice || a.DocType == DocTypeConstants.CreditNote) && a.DocSubType != "Recurring" && lstComapny1.Keys.Contains(a.ServiceCompanyId.Value))) : lstInvAndCNs)
                        {
                            PaymentDetailModel pDetail = lstPDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                            if (pDetail == null)
                            {
                                PaymentDetailModel detailModel = new PaymentDetailModel();
                                detailModel.ServiceCompanyName = lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Value).FirstOrDefault() : null;
                                detailModel.ServiceCompanyId = isIC && lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Key).FirstOrDefault() : serviceCompanyId;
                                detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId.Value).Any() ? false : true : true;
                                detailModel.DocumentDate = detail.DocDate;
                                detailModel.DocumentNo = detail.DocNo;
                                detailModel.DocumentState = detail.DocumentState;
                                detailModel.DocumentAmmount = detail.DocType == DocTypeConstants.Invoice ? -detail.GrandTotal : detail.GrandTotal;
                                detailModel.Nature = detail.Nature;
                                detailModel.AmmountDue = detail.DocType == DocTypeConstants.Invoice ? -detail.BalanceAmount : detail.BalanceAmount;
                                detailModel.SystemReferenceNumber = detail.DocNo;
                                detailModel.Currency = detail.DocCurrency;
                                detailModel.DocumentId = detail.Id;
                                detailModel.DocumentType = detail.DocType;
                                lstPDetailModel.Add(detailModel);
                            }
                        }
                    }
                    List<DebitNoteCompact> lstDebitNotes = new List<DebitNoteCompact>();
                    if (/*!isInterCompanyActive &&*/ !isIC)
                        lstDebitNotes = _debitNoteCompactService.GetListOfDebitNoteWithOutInter(companyId, serviceCompanyId, entityId, currency, docDate);
                    else
                        lstDebitNotes = _debitNoteCompactService.GetListOfDebitNoteWithInter(companyId, entityId, currency, docDate);
                    if (lstDebitNotes.Any())
                    {
                        lstSubComapny = _companyService.GetAllSubCompanies(lstDebitNotes.Select(c => c.ServiceCompanyId.Value).ToList(), username, companyId);
                        lstComp = lstComapny1.Except(lstSubComapny).ToDictionary(Id => Id.Key, Name => Name.Value);
                    }
                    if (lstDebitNotes.Any())
                    {
                        foreach (DebitNoteCompact detail in isIC ? lstDebitNotes.Where(a => lstComapny1.Keys.Contains(a.ServiceCompanyId.Value)) : lstDebitNotes)
                        {
                            PaymentDetailModel pDetail = lstPDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                            if (pDetail == null)
                            {
                                PaymentDetailModel detailModel = new PaymentDetailModel();
                                detailModel.ServiceCompanyName = lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Value).FirstOrDefault() : null;
                                detailModel.ServiceCompanyId = isIC && lstComapny1 != null ? lstComapny1.Where(a => a.Key == detail.ServiceCompanyId).Select(a => a.Key).FirstOrDefault() : serviceCompanyId;
                                detailModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == detail.ServiceCompanyId.Value).Any() ? false : true : true;
                                detailModel.DocumentDate = detail.DocDate;
                                detailModel.DocumentNo = detail.DocNo;
                                detailModel.DocumentState = detail.DocumentState;
                                detailModel.DocumentAmmount = -detail.GrandTotal;
                                detailModel.Nature = detail.Nature;
                                detailModel.AmmountDue = -detail.BalanceAmount;
                                detailModel.SystemReferenceNumber = detail.DocNo;
                                detailModel.Currency = detail.DocCurrency;
                                detailModel.DocumentId = detail.Id;
                                detailModel.DocumentType = detail.DocSubType;
                                lstPDetailModel.Add(detailModel);
                            }
                        }
                    }
                }
            }

            return lstPDetailModel.OrderBy(c => c.DocumentDate).ThenBy(d => d.SystemReferenceNumber).ToList();
            //}
            //return lstPDetailModel;
        }
        public List<PaymentDetailModel> CreatePaymentModel(Guid paymentId, long companyId, DateTime docDate, string DocSubType, string username)
        {
            FinancialSetting financeSetting = _financialSettingService.GetFinancialSetting(companyId);
            if (financeSetting == null)
            {
                throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
            }
            Payment payment = _paymentService.GetPayments(paymentId, companyId, DocSubType);
            Dictionary<long, string> lstOfServiceCompanies = null;
            List<PaymentDetailModel> lstPDetailModel = new List<PaymentDetailModel>();
            if (payment == null)
            {

            }
            else
            {
                List<PaymentDetail> lstPDetail = _paymentDetailService.GetByPaymentId(paymentId);
                if (lstPDetail.Count > 0)
                {
                    lstOfServiceCompanies = _companyService.GetAllCompaniesName(payment.PaymentDetails.Select(a => a.ServiceCompanyId.Value).ToList());
                    if (payment.DocumentState == "Void")
                    {
                        foreach (PaymentDetail detail in lstPDetail.Where(x => x.PaymentAmount > 0).ToList())
                        {
                            PaymentDetailModel paymentDetailModel = new PaymentDetailModel();
                            paymentDetailModel.Id = detail.Id;
                            paymentDetailModel.PaymentId = detail.PaymentId;
                            paymentDetailModel.DocumentDate = detail.DocumentDate;
                            paymentDetailModel.DocumentNo = detail.DocumentNo;
                            paymentDetailModel.DocumentType = detail.DocumentType;
                            paymentDetailModel.DocumentState = detail.DocumentState;
                            paymentDetailModel.DocumentId = detail.DocumentId;
                            paymentDetailModel.AmmountDue = detail.AmmountDue;
                            paymentDetailModel.DocumentAmmount = detail.DocumentAmmount;
                            paymentDetailModel.Currency = detail.Currency;
                            paymentDetailModel.ServiceCompanyId = detail.ServiceCompanyId;
                            paymentDetailModel.PaymentAmount = (detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote) ? -detail.PaymentAmount : detail.PaymentAmount;

                            if (detail.ServiceCompanyId != null)
                            {
                                paymentDetailModel.ServiceCompanyName = lstOfServiceCompanies.Where(x => x.Key == paymentDetailModel.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                            }
                            lstPDetailModel.Add(paymentDetailModel);
                        }
                    }
                    else
                    {
                        List<InvoiceCompact> lstInvoiceAndCNs = null;
                        //List<InvoiceCompact> lstCreditNotes;
                        List<DebitNoteCompact> lstDebitNotes = null;
                        //List<CreditMemoCompact> lstCreditMemos;
                        if (payment.IsCustomer == true)
                        {
                            lstInvoiceAndCNs = _invoiceCompactService.GetListOfInvoiceAndCN(payment.CompanyId, payment.PaymentDetails.Where(a => (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.CreditNote) && a.PaymentAmount > 0).Select(a => a.DocumentId).ToList(), payment.EntityId, payment.DocCurrency);
                            //lstCreditNotes = _invoiceCompactService.GetListOfInvoiceAndCN(payment.CompanyId, payment.PaymentDetails.Where(a => a.DocumentType == DocTypeConstants.CreditNote && a.PaymentAmount > 0).Select(a => a.DocumentId).ToList(), payment.EntityId, payment.DocCurrency);
                            lstDebitNotes = _debitNoteCompactService.GetListOfDebitNotes(payment.CompanyId, DocTypeConstants.DebitNote, payment.EntityId, payment.PaymentDetails.Where(a => a.DocumentType == DocTypeConstants.DebitNote && a.PaymentAmount > 0).Select(a => a.DocumentId).ToList());
                        }

                        List<CreditMemoCompact> lstCreditMemo = _creditMemoCompactService.GetListOfCreditMemos(payment.CompanyId, DocTypeConstants.BillCreditMemo, payment.EntityId, payment.PaymentDetails.Where(a => a.DocumentType == DocTypeConstants.BillCreditMemo && a.PaymentAmount > 0).Select(a => a.DocumentId).ToList());
                        List<Bill> lstBills = _billService.GetAllBillsByDocId(payment.PaymentDetails.Select(c => c.DocumentId).ToList(), payment.PaymentDetails.Select(c => c.DocumentType).ToList(), payment.DocCurrency, payment.EntityId);

                        //for service company
                        lstOfServiceCompanies = _companyService.GetAllCompaniesName(payment.PaymentDetails.Select(a => a.ServiceCompanyId.Value).ToList());
                        Dictionary<long, string> lstSubComapny = _companyService.GetAllSubCompanies(payment.PaymentDetails.Select(a => a.ServiceCompanyId.Value).ToList(), username, companyId);
                        Dictionary<long, string> lstComp = lstOfServiceCompanies.Except(lstSubComapny).ToDictionary(Id => Id.Key, Name => Name.Value);
                        Dictionary<long, RecordStatusEnum> entityStatus = _companyService.GetAllCompaniesStatus(payment.PaymentDetails.Select(c => c.ServiceCompanyId.Value).ToList());
                        foreach (var pDetail in payment.PaymentDetails)
                        {
                            PaymentDetailModel PDModel = new PaymentDetailModel();
                            PDModel.Id = pDetail.Id;
                            PDModel.PaymentId = pDetail.PaymentId;

                            if (pDetail.ServiceCompanyId != null)
                            {
                                PDModel.ServiceCompanyName = lstOfServiceCompanies.Where(a => a.Key == pDetail.ServiceCompanyId).Select(a => a.Value).FirstOrDefault();
                                PDModel.ServiceCompanyId = lstOfServiceCompanies.Where(a => a.Key == pDetail.ServiceCompanyId).Select(a => a.Key).FirstOrDefault();
                                PDModel.IsHyperLinkEnable = lstComp.Any() ? lstComp.Where(c => c.Key == pDetail.ServiceCompanyId).Any() ? false : true : true;
                            }

                            if (pDetail.DocumentType == /*DocTypeConstants.Bills*/ DocTypeConstants.General || pDetail.DocumentType == DocTypeConstants.PayrollBill || pDetail.DocumentType == DocTypeConstants.OpeningBalance || pDetail.DocumentType == DocTypeConstants.Claim || pDetail.DocumentType == DocTypeConstants.Bills)
                            {
                                //var bill = _billService.GetBill(pDetail.DocumentId, pDetail.DocumentType, payment.DocCurrency, payment.EntityId);
                                Bill bill = lstBills.Where(c => c.Id == pDetail.DocumentId).FirstOrDefault();
                                if (bill != null)
                                {
                                    //FillPaymentDetailModel(PDModel, bill, pDetail, true, true);
                                    PDModel.DocumentDate = bill.PostingDate;
                                    PDModel.DocumentNo = bill.DocNo;
                                    PDModel.DocumentState = bill.DocumentState;
                                    PDModel.DocumentAmmount = bill.GrandTotal;
                                    PDModel.Nature = bill.Nature;
                                    if (entityStatus.Any())
                                        PDModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == pDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    PDModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? bill.BalanceAmount.Value : bill.BalanceAmount + pDetail.PaymentAmount;
                                    PDModel.PaymentAmount = pDetail.PaymentAmount;
                                    PDModel.SystemReferenceNumber = bill.SystemReferenceNumber;
                                    PDModel.Currency = bill.DocCurrency;
                                    PDModel.DocumentId = pDetail.DocumentId;
                                    PDModel.DocumentType = pDetail.DocumentType;
                                    lstPDetailModel.Add(PDModel);
                                }
                            }
                            else if (pDetail.DocumentType == DocTypeConstants.BillCreditMemo)
                            {
                                CreditMemoCompact creditMemo = lstCreditMemo.Where(a => a.Id == pDetail.DocumentId).FirstOrDefault();
                                if (creditMemo != null)
                                {
                                    PDModel.DocumentDate = creditMemo.PostingDate;
                                    PDModel.DocumentNo = creditMemo.DocNo;
                                    PDModel.DocumentState = creditMemo.DocumentState;
                                    PDModel.DocumentAmmount = -creditMemo.GrandTotal;
                                    PDModel.Nature = creditMemo.Nature;
                                    PDModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? -creditMemo.BalanceAmount : -(creditMemo.BalanceAmount + pDetail.PaymentAmount);
                                    PDModel.PaymentAmount = -pDetail.PaymentAmount;
                                    if (entityStatus.Any())
                                        PDModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == pDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    PDModel.SystemReferenceNumber = creditMemo.DocNo;
                                    PDModel.Currency = creditMemo.DocCurrency;
                                    PDModel.DocumentId = pDetail.DocumentId;
                                    PDModel.DocumentType = pDetail.DocumentType;
                                    lstPDetailModel.Add(PDModel);
                                }
                            }
                            else if (pDetail.DocumentType == DocTypeConstants.Invoice || pDetail.DocumentType == DocTypeConstants.CreditNote)
                            {
                                InvoiceCompact invoiceAndCn = lstInvoiceAndCNs != null ? lstInvoiceAndCNs.Where(a => a.Id == pDetail.DocumentId).FirstOrDefault() : null;
                                if (invoiceAndCn != null)
                                {
                                    PDModel.DocumentDate = invoiceAndCn.DocDate;
                                    PDModel.DocumentNo = invoiceAndCn.DocNo;
                                    PDModel.DocumentState = invoiceAndCn.DocumentState;
                                    PDModel.DocumentAmmount = pDetail.DocumentType == DocTypeConstants.Invoice ? -invoiceAndCn.GrandTotal : invoiceAndCn.GrandTotal;
                                    PDModel.Nature = invoiceAndCn.Nature;
                                    if (entityStatus.Any())
                                        PDModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == pDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    PDModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? invoiceAndCn.DocType == DocTypeConstants.Invoice ? -invoiceAndCn.BalanceAmount : invoiceAndCn.BalanceAmount : invoiceAndCn.DocType == DocTypeConstants.Invoice ? -(invoiceAndCn.BalanceAmount + pDetail.PaymentAmount) : (invoiceAndCn.BalanceAmount + pDetail.PaymentAmount);
                                    PDModel.PaymentAmount = invoiceAndCn.DocType == DocTypeConstants.Invoice ? -pDetail.PaymentAmount : pDetail.PaymentAmount;
                                    PDModel.SystemReferenceNumber = invoiceAndCn.DocNo;
                                    PDModel.Currency = invoiceAndCn.DocCurrency;
                                    PDModel.DocumentId = pDetail.DocumentId;
                                    PDModel.DocumentType = pDetail.DocumentType;
                                    lstPDetailModel.Add(PDModel);
                                }
                            }
                            else if (pDetail.DocumentType == DocTypeConstants.DebitNote)
                            {
                                DebitNoteCompact debitNote = lstDebitNotes != null ? lstDebitNotes.Where(a => a.Id == pDetail.DocumentId).FirstOrDefault() : null;
                                if (debitNote != null)
                                {
                                    PDModel.DocumentDate = debitNote.DocDate;
                                    PDModel.DocumentNo = debitNote.DocNo;
                                    PDModel.DocumentState = debitNote.DocumentState;
                                    PDModel.DocumentAmmount = -debitNote.GrandTotal;
                                    PDModel.Nature = debitNote.Nature;
                                    PDModel.AmmountDue = payment != null && payment.DocumentState == PaymentState.Void ? -debitNote.BalanceAmount : -(debitNote.BalanceAmount + pDetail.PaymentAmount);
                                    if (entityStatus.Any())
                                        PDModel.ServiceEntityStatus = entityStatus.Where(c => c.Key == pDetail.ServiceCompanyId).Select(c => c.Value).FirstOrDefault();
                                    PDModel.PaymentAmount = -pDetail.PaymentAmount;
                                    PDModel.SystemReferenceNumber = debitNote.DocNo;
                                    PDModel.Currency = debitNote.DocCurrency;
                                    PDModel.DocumentId = pDetail.DocumentId;
                                    PDModel.DocumentType = pDetail.DocumentType;
                                    lstPDetailModel.Add(PDModel);
                                }
                            }
                            // lstPDetailModel.Add(PDModel);
                        }
                        #region commented_code_if_any_newrecord_created_at_that_time_span_for_that_Entity
                        //List<Bill> lstBill = _billService.GetBillByEntity(DocSubType == DocTypeConstants.PayrollBill/*DocTypeConstants.Payment*/ ? DocTypeConstants.PayrollBill /*DocTypeConstants.Bills*/ : DocTypeConstants.General /*DocTypeConstants.PayrollBill*/, companyId, payment.EntityId, payment.DocCurrency, payment.ServiceCompanyId, docDate);
                        //foreach (Bill detail in lstBill)
                        //{
                        //    var d = lstPDetailModel.Where(a => a.DocumentId == detail.Id).FirstOrDefault();
                        //    if (d == null)
                        //    {
                        //        var serviceCompanyName = _companyService.GetCompanyByCompanyid(detail.ServiceCompanyId);

                        //        PaymentDetail pdetail = new PaymentDetail();
                        //        PaymentDetailModel detailModel = new PaymentDetailModel();
                        //        //FillPaymentDetailModel(detailModel, detail, pdetail, false, false);
                        //        if (serviceCompanyName != null)
                        //        {
                        //            detailModel.ServiceCompanyId = serviceCompanyName.Id;
                        //            detailModel.ServiceCompanyName = serviceCompanyName.ShortName;
                        //        }
                        //        detailModel.DocumentNo = detail.DocNo;
                        //        detailModel.DocumentType = detail.DocSubType;
                        //        detailModel.DocumentId = detail.Id;
                        //        detailModel.DocumentState = detail.DocumentState;
                        //        detailModel.DocumentDate = detail.PostingDate;
                        //        detailModel.DocumentAmmount = detail.GrandTotal;
                        //        detailModel.Currency = detail.DocCurrency;
                        //        detailModel.AmmountDue = detail.BalanceAmount;
                        //        detailModel.Nature = detail.Nature;
                        //        detailModel.SystemReferenceNumber = detail.SystemReferenceNumber;

                        //        lstPDetailModel.Add(detailModel);
                        //    }
                        //}
                        #endregion

                    }
                }
            }
            return lstPDetailModel;
        }

        #endregion

        #region Save Call
        public Payment SavePayment(PaymentModel TObject, string ConnectionString)
        {
            bool isAdd = false;
            bool isDocAdd = false;
            try
            {
                var AdditionalInfo = new Dictionary<string, object>();
                AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
                LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, "ObjectSave", AdditionalInfo);

                LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entred_Into_SavePayment_Method);
                long? serviceCompanyId;
                decimal? oldExchangeRate = null;
                decimal? oldSysCalExRate = null;
                DateTime? oldDocDate = null;
                Dictionary<Guid?, decimal?> lstADocValue = new Dictionary<Guid?, decimal?>();
                string _errors = CommonValidation.ValidateObject(TObject);
                if (!string.IsNullOrEmpty(_errors))
                {
                    throw new InvalidOperationException(_errors);
                }
                if (_paymentService.CheckDocumentState(TObject.Id, TObject.DocSubType) == true)
                    throw new InvalidOperationException(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
                if (TObject.GrandTotal <= 0)
                    throw new InvalidOperationException(PaymentValidations.Payment_Total_Amount_Should_Be_Grater_Than_Zero);

                #region Interco_Validation_on_ServiceEntitties
                int entityCount = 0;
                bool? isICActive = TObject.PaymentDetailModels.Any(a => a.ServiceCompanyId != TObject.ServiceCompanyId);
                List<long?> lstEntityIds = null;
                if (isICActive == true)
                {
                    entityCount = TObject.PaymentApplicationAmmount > 0 ? TObject.PaymentDetailModels.Where(d => d.PaymentAmount != 0).Select(c => c.ServiceCompanyId).Distinct().Count() : TObject.PaymentDetailModels.Select(c => c.ServiceCompanyId).Distinct().Count();
                    lstEntityIds = TObject.PaymentApplicationAmmount > 0 ? TObject.PaymentDetailModels.Where(d => d.PaymentAmount != 0).Select(c => c.ServiceCompanyId).Distinct().ToList() : TObject.PaymentDetailModels.Select(c => c.ServiceCompanyId).Distinct().ToList();
                    lstEntityIds.Add(TObject.ServiceCompanyId);
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

                if (TObject.EntityId == null)
                {
                    throw new InvalidOperationException(CommonConstant.Entity_is_mandatory);
                }
                if (TObject.ServiceCompanyId == null)
                    throw new InvalidOperationException(CommonConstant.ServiceCompany_is_mandatory);
                if (TObject.DocDate == null)
                {
                    throw new InvalidOperationException(CommonConstant.Invalid_Document_Date);
                }

                if (TObject.IsDocNoEditable == true)
                    if (IsDocumentNumberExists(TObject.DocNo, TObject.Id, TObject.CompanyId, TObject.DocSubType))
                    {
                        throw new InvalidOperationException(CommonConstant.Document_number_already_exist);
                    }
                if (TObject.GrandTotal <= 0)
                {
                    throw new InvalidOperationException(CommonConstant.Grand_Total_should_be_greater_than_zero);
                }
                if (TObject.ExchangeRate == 0)
                    throw new InvalidOperationException(CommonConstant.ExchangeRate_Should_Be_Grater_Than_0);

                FinancialSetting financialSetting = _financialSettingService.GetFinancialSetting(TObject.CompanyId);
                if (financialSetting != null)
                {
                    if (!(financialSetting.EndOfYearLockDate == null || TObject.DocDate >= financialSetting.EndOfYearLockDate))
                    {
                        throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                    }
                    if (!ValidateFinancialOpenPeriod(TObject.DocDate, financialSetting))
                    {
                        if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                        {
                            throw new InvalidOperationException(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                        }
                        else if (financialSetting.PeriodLockDatePassword != TObject.PeriodLockPassword)
                        {
                            throw new InvalidOperationException(CommonConstant.Invalid_Financial_Period_Lock_Password);
                        }
                    }

                }

                List<DocumentHistoryModel> lstOfDocumentHistory = new List<DocumentHistoryModel>();
                bool isEdit = false;
                List<long?> lstServeIds = new List<long?>();
                lstServeIds.AddRange(TObject.PaymentDetailModels.Select(d => d.ServiceCompanyId));
                lstServeIds.Add(TObject.ServiceCompanyId);
                int? serviceEntityCount = lstServeIds.GroupBy(a => a.Value).Count();
                long? clearingPaymetCoaId = _chartOfAccountService.GetChartOfAccountIDByName(COANameConstants.Clearing_Payment, TObject.CompanyId);
                Payment _payment = _paymentService.CheckPaymentById(TObject.Id, TObject.DocSubType);
                string oldDocumentNo = string.Empty;
                if (_payment != null)
                {
                    oldDocumentNo = _payment.DocNo;
                    //Data concurrency verify
                    string timeStamp = "0x" + string.Concat(Array.ConvertAll(_payment.Version, x => x.ToString("X2")));
                    if (!timeStamp.Equals(TObject.Version))
                        throw new InvalidOperationException(CommonConstant.Document_has_been_modified_outside);

                    isEdit = true;
                    oldDocDate = _payment.DocDate;
                    serviceCompanyId = _payment.ServiceCompanyId;
                    oldExchangeRate = _payment.ExchangeRate;
                    oldSysCalExRate = _payment.SystemCalculatedExchangeRate;
                    LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Checking_Payment_Is_Not_Equal_To_Null);
                    InsertPayment(TObject, _payment);
                    _payment.DocNo = TObject.DocNo;
                    _payment.SystemRefNo = _payment.DocNo;
                    _payment.ModifiedBy = TObject.ModifiedBy;
                    _payment.ModifiedDate = DateTime.UtcNow;
                    _payment.ObjectState = ObjectState.Modified;
                    _paymentService.UpdatePayment(_payment);
                    UpdatePaymentDetails(TObject, _payment, isEdit, ConnectionString, serviceEntityCount, clearingPaymetCoaId, oldExchangeRate, oldSysCalExRate, lstOfDocumentHistory, oldDocDate, lstADocValue, serviceCompanyId);
                }
                else
                {
                    isAdd = true;
                    _payment = new Payment();
                    InsertPayment(TObject, _payment);
                    _payment.Id = Guid.NewGuid();
                    _payment.Status = AppsWorld.Framework.RecordStatusEnum.Active;
                    _payment.UserCreated = TObject.UserCreated;
                    _payment.CreatedDate = DateTime.UtcNow;
                    _payment.ObjectState = ObjectState.Added;
                    _payment.SystemRefNo = TObject.IsDocNoEditable != true ? _autoService.GetAutonumber(TObject.CompanyId, TObject.DocSubType != DocTypeConstants.PayrollPayment ? "Payment" : "Payroll Payment", ConnectionString) : TObject.DocNo;
                    isDocAdd = true;
                    _payment.DocNo = _payment.SystemRefNo;
                    _paymentService.Insert(_payment);
                    oldDocDate = _payment.DocDate;
                    if (TObject.PaymentDetailModels.Any())
                    {
                        UpdatePaymentDetails(TObject, _payment, isEdit, ConnectionString, serviceEntityCount, clearingPaymetCoaId, oldExchangeRate, oldSysCalExRate, lstOfDocumentHistory, oldDocDate, lstADocValue, _payment.ServiceCompanyId);
                    }
                }
                try
                {
                    _unitOfWorkAsync.SaveChanges();
                    #region Documentary History
                    try
                    {
                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(_payment.Id, _payment.CompanyId, _payment.Id, _payment.DocType, _payment.DocSubType, _payment.DocumentState, _payment.DocCurrency, _payment.GrandTotal, 0, _payment.ExchangeRate != null ? _payment.ExchangeRate.Value : _payment.SystemCalculatedExchangeRate != null ? _payment.SystemCalculatedExchangeRate.Value : 1, _payment.ModifiedBy != null ? _payment.ModifiedBy : _payment.UserCreated, _payment.Remarks, _payment.DocDate, _payment.GrandTotal, 0);
                        if (lstdocumet.Any())
                            lstOfDocumentHistory.AddRange(lstdocumet);
                        if (lstOfDocumentHistory.Any())
                        {
                            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entering_Into_Saving_DocumentHistory_Block);
                            AppaWorld.Bean.Common.SaveDocumentHistory(lstOfDocumentHistory, ConnectionString);
                            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Sucessfully_inserted_the_documents_in_DocumentHistory);
                        }

                        if (oldDocDate != TObject.DocDate)
                        {
                            query = string.Empty;
                            query = $"Update Bean.DocumentHistory Set PostingDate='{String.Format("{0:MM/dd/yyyy}", TObject.DocDate)}' where TransactionId='{_payment.Id}' and CompanyId={_payment.CompanyId} and TransactionId<>DocumentId and AgingState is null;";

                            using (con = new SqlConnection(ConnectionString))
                            {
                                if (con.State != ConnectionState.Open)
                                    con.Open();
                                cmd = new SqlCommand(query, con);
                                cmd.ExecuteNonQuery();
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Sucessfully_Updated_the_documents_in_DocumentHistory_if_DocDate_is_changed_in_EditMode);
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Issues_While_inserting_the_record_in_document_history);
                    }
                    #endregion Documentary History
                    if (_payment.DocSubType == DocTypeConstants.Payroll)
                    {
                        JVModel jvm = new JVModel();
                        _payment.PaymentDetails = _paymentDetailService.GetByPaymentId(_payment.Id);
                        List<PaymentDetailModel> lstPaymentDetail = TObject.PaymentDetailModels.Where(c => c.ServiceCompanyId != TObject.ServiceCompanyId && c.PaymentAmount > 0).ToList();
                        JVModel jvm1 = new JVModel();
                        jvm1.DocNo = _payment.DocNo;
                        jvm1.SystemReferenceNo = _payment.SystemRefNo;
                        bool isFirst = true;
                        bool isFirstExicute = false;
                        FillJournal(jvm1, _payment, false, true, out isFirst, true, TObject.IsInterCompanyActive, lstPaymentDetail, lstADocValue);
                        if (jvm1.JVVDetailModels != null)
                        {
                            jvm1.IsFirst = true;
                            isFirstExicute = true;
                            SaveInvoice1(jvm1);
                        }
                        FillJournal(jvm, _payment, false, false, out isFirst, isFirst, TObject.IsInterCompanyActive, lstPaymentDetail, lstADocValue);
                        jvm.Id = _payment.Id;
                        jvm1.DocNo = _payment.DocNo;
                        jvm1.SystemReferenceNo = _payment.SystemRefNo;
                        jvm.IsFirst = isFirstExicute == isFirst;
                        SaveInvoice1(jvm);
                        if (TObject.IsInterCompanyActive == true && (_payment.PaymentDetails != null || _payment.PaymentDetails.Count > 0))
                        {
                            List<PaymentDetail> details = _payment.PaymentDetails.Where(c => c.ServiceCompanyId != TObject.ServiceCompanyId && c.PaymentAmount > 0).OrderBy(c => c.ServiceCompanyId).ToList();
                            string shotCode = _companyService.Query(a => a.Id == TObject.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                            foreach (var detail in details)
                            {
                                jvm = new JVModel();
                                FillInterCompanyJournal(jvm, _payment, true, true, detail, shotCode, lstADocValue);
                                SaveInvoice1(jvm);
                            }
                        }
                    }
                    else
                    {
                        using (con = new SqlConnection(ConnectionString))
                        {
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            cmd = new SqlCommand("Payment_Posting_Sp", con);
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@SourceId", _payment.Id);
                            cmd.Parameters.AddWithValue("@Type", DocTypeConstants.BillPayment);
                            cmd.Parameters.AddWithValue("@CompanyId", _payment.CompanyId);
                            cmd.Parameters.AddWithValue("@RoundingAmount", string.Join(":", lstADocValue));
                            cmd.ExecuteNonQuery();
                            if (con.State != ConnectionState.Closed)
                                con.Close();
                        }
                    }

                    #region DocumentAttachment_Save_Call
                    if (isAdd && TObject.TileAttachments != null && TObject.TileAttachments.Count > 0)
                    {
                        string name = _beanEntityService.GetEntityName(TObject.CompanyId, TObject.EntityId);
                        string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                        string DocuNo = _commonApplicationService.StringCharactersReplaceFunction(_payment.DocNo);
                        string path = DocumentConstants.Entities + "/" + EntityName + "/" + _payment.DocType + "s" + "/" + DocuNo;
                        SaveTailsAttachments(TObject.CompanyId, path, TObject.UserCreated, TObject.TileAttachments);
                    }
                    #endregion

                    #region Document Folder Rename

                    if (!isAdd && oldDocumentNo != TObject.DocNo)
                    {
                        string name = _beanEntityService.GetEntityName(TObject.CompanyId, TObject.EntityId);
                        string EntityName = _commonApplicationService.StringCharactersReplaceFunction(name);
                        _commonApplicationService.ChangeFolderName(TObject.CompanyId, TObject.DocNo, oldDocumentNo, EntityName, "Bill Payments");
                    }

                    #endregion

                }
                catch (DbEntityValidationException e)
                {
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
                    throw new InvalidOperationException("An error has occurred!Please try after sometimes.");
                }
                return _payment;
            }
            catch (Exception ex)
            {
                if (isAdd && isDocAdd && TObject.IsDocNoEditable == false)
                {
                    AppaWorld.Bean.Common.SaveDocNoSequence(TObject.CompanyId, TObject.DocSubType == DocTypeConstants.Payroll ? DocTypeConstants.PayrollPayments : DocTypeConstants.Payment, ConnectionString);
                }
                throw ex;
            }
        }


        public Payment SavePayementVoidNew(DocumentVoidModel TObject, string ConnectionString)
        {
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entered_Into_SavePaymentDocumentVoid_Of_payment_Method);
            string DocNo = "-V";
            bool? isVoid = true;
            List<DocumentHistoryModel> lstOfDocumentHistoryModel = new List<DocumentHistoryModel>();
            decimal roundingSum = 0;
            //string DocDescription = "Void-";
            Payment _document = _paymentService.GetPayments(TObject.Id, TObject.CompanyId, TObject.DocType);
            if (_document == null)
                throw new Exception(PaymentValidations.Invalid_Payment);
            else
            {
                //Data concurrency verify
                string timeStamp = "0x" + string.Concat(Array.ConvertAll(_document.Version, x => x.ToString("X2")));
                if (!timeStamp.Equals(TObject.Version))
                    throw new Exception(CommonConstant.Document_has_been_modified_outside);
            }
            if (_paymentService.CheckDocumentState(TObject.Id, TObject.DocType) == true)
                throw new Exception(CommonConstant.DocumentState_has_been_changed_please_kindly_refresh_the_screen);
            //if (_document.DocumentState != CreditNoteState.NotApplied)
            //	throw new Exception("State should be " + CreditNoteState.NotApplied);

            //Need to verify the invoice is within Financial year
            if (!_financialSettingService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
            {
                throw new Exception(CommonConstant.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }
            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (!_financialSettingService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
            {
                //LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, ReceiptLoggingValidation.Enter_Into_Not_Null_Of_DocDate_And_CompanyId);
                if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                {
                    throw new Exception(CommonConstant.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                }
                else if (!_financialSettingService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                {
                    throw new Exception(CommonConstant.Invalid_Financial_Period_Lock_Password);
                }
            }
            _document.DocumentState = PaymentState.Void;
            _document.DocNo = _document.DocNo + DocNo;
            _document.ModifiedBy = TObject.ModifiedBy;
            _document.ModifiedDate = DateTime.UtcNow;
            //_document.DocumentDescription = DocDescription + _document.DocumentDescription;
            _document.ObjectState = ObjectState.Modified;
            List<InvoiceCompact> lstInv = null;
            List<DebitNoteCompact> lstDN = null;
            List<Bill> lstBills = null;
            List<CreditMemoCompact> lstCM = null;
            if (_document.PaymentDetails.Any())
            {
                lstBills = _billService.GetBillsByDocId(_document.PaymentDetails.Where(c => c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.PayrollBill || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim && c.PaymentAmount != 0).Select(c => c.DocumentId).ToList(), _document.CompanyId);
                lstCM = _creditMemoCompactService.GetAllCMByDocId(_document.PaymentDetails.Where(c => c.DocumentType == DocTypeConstants.BillCreditMemo && c.PaymentAmount > 0).Select(c => c.DocumentId).ToList(), _document.CompanyId);
            }
            if (_document.IsCustomer == true)
            {
                lstInv = _invoiceCompactService.GetListOfInvoices(_document.CompanyId, _document.PaymentDetails.Where(c => (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.CreditNote) && c.PaymentAmount != 0).Select(c => c.DocumentId).ToList());
                lstDN = _debitNoteCompactService.GetListOfDebitNote(_document.CompanyId, _document.PaymentDetails.Where(c => c.DocumentType == DocTypeConstants.DebitNote && c.PaymentAmount > 0).Select(c => c.DocumentId).ToList());
            }
            if (_document.PaymentDetails != null && _document.PaymentDetails.Count > 0)
            {
                foreach (PaymentDetail detail in _document.PaymentDetails.Where(c => c.PaymentAmount != 0).ToList())
                {

                    if (detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.PayrollBill || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                    {
                        //DebitNote debitNote = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                        //detail.ReceiptAmount = -detail.ReceiptAmount;
                        Bill bills = lstBills != null ? lstBills.Where(d => d.Id == detail.DocumentId).FirstOrDefault() : null;
                        if (bills != null)
                        {
                            bills.BalanceAmount += detail.PaymentAmount;
                            if (bills.BalanceAmount == bills.GrandTotal)
                            {
                                bills.DocumentState = InvoiceState.NotPaid;
                                bills.BaseBalanceAmount = bills.BaseGrandTotal;
                                bills.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                bills.ModifiedDate = DateTime.UtcNow;
                                bills.ModifiedBy = "System";
                                bills.ObjectState = ObjectState.Modified;
                            }
                            else if (bills.BalanceAmount != bills.GrandTotal)
                            {
                                bills.DocumentState = InvoiceState.PartialPaid;
                                //roundingSum = (Math.Round((bills.GrandTotal - detail.PaymentAmount) * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.PaymentAmount * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)bills.BaseGrandTotal;
                                bills.BaseBalanceAmount += (Math.Round((detail.PaymentAmount * (bills.ExchangeRate != null ? (decimal)bills.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                bills.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                bills.ModifiedDate = DateTime.UtcNow;
                                bills.ModifiedBy = "System";
                                bills.ObjectState = ObjectState.Modified;
                            }
                        }
                        if (bills.IsExternal == true && bills.DocSubType == DocTypeConstants.OpeningBalance)
                        {
                            SqlConnection conn = new SqlConnection(ConnectionString);
                            if (conn.State != ConnectionState.Open)
                                conn.Open();
                            SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                            oBcmd.CommandTimeout = 30;
                            oBcmd.CommandType = CommandType.StoredProcedure;
                            oBcmd.Parameters.AddWithValue("@OBId", bills.OpeningBalanceId);
                            oBcmd.Parameters.AddWithValue("@DocumentId", bills.Id);
                            oBcmd.Parameters.AddWithValue("@CompanyId", bills.CompanyId);
                            oBcmd.Parameters.AddWithValue("@IsEqual", bills.BalanceAmount == bills.GrandTotal ? true : false);
                            int res = oBcmd.ExecuteNonQuery();
                            conn.Close();

                        }
                        #region Update_Journal_Detail_Clearing_Status
                        SqlConnection con = new SqlConnection(ConnectionString);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                        cmd.CommandTimeout = 30;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@companyId", bills.CompanyId);
                        cmd.Parameters.AddWithValue("@documentId", bills.Id);
                        cmd.Parameters.AddWithValue("@docState", bills.DocumentState);
                        cmd.Parameters.AddWithValue("@balanceAmount", bills.BalanceAmount);
                        int count = cmd.ExecuteNonQuery();
                        con.Close();
                        #endregion Update_Journal_Detail_Clearing_Status
                        #region Documentary History
                        try
                        {
                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, bills.CompanyId, bills.Id, bills.DocType, bills.DocSubType, bills.DocumentState, bills.DocCurrency, bills.GrandTotal, bills.BalanceAmount.Value, bills.ExchangeRate.Value, bills.ModifiedBy != null ? bills.ModifiedBy : bills.UserCreated, bills.Remarks, null, 0, 0);
                            if (lstdocumet.Any())
                                lstOfDocumentHistoryModel.AddRange(lstdocumet);
                            //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                        }
                        catch (Exception ex)
                        {

                        }
                        #endregion Documentary History
                    }
                    if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                    {
                        //DebitNote debitNote = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                        CreditMemoCompact creditMemo = lstCM != null ? lstCM.Where(d => d.Id == detail.DocumentId).FirstOrDefault() : null;
                        if (creditMemo != null)
                        {
                            //creditMemo.BalanceAmount += detail.PaymentAmount;
                            //if (creditMemo.BalanceAmount == creditMemo.GrandTotal)
                            //{
                            //    creditMemo.DocumentState = CreditMemoState.Not_Applied;
                            //    creditMemo.ModifiedDate = DateTime.UtcNow;
                            //    creditMemo.ModifiedBy = "System";
                            //    creditMemo.ObjectState = ObjectState.Modified;
                            //}
                            //else if (creditMemo.BalanceAmount != creditMemo.GrandTotal)
                            //{
                            //    creditMemo.DocumentState = CreditMemoState.Partial_Applied;
                            //    creditMemo.ModifiedDate = DateTime.UtcNow;
                            //    creditMemo.ModifiedBy = "System";
                            //    creditMemo.ObjectState = ObjectState.Modified;
                            //}
                        }
                        CreditMemoApplicationCompact cmApplication = _creditMemoCompactService.GetCMApplicationByDocId(detail.Id);
                        if (cmApplication != null)
                        {
                            DocumentResetModel voidModel = new DocumentResetModel();
                            voidModel.Id = cmApplication.Id;
                            voidModel.CreditMemoId = cmApplication.CreditMemoId;
                            voidModel.ResetDate = DateTime.UtcNow;
                            voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                            voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));

                            CMAplicationREST(null, voidModel, true);
                        }

                        //if (creditMemo.DocSubType == DocTypeConstants.OpeningBalance)
                        //{
                        //    SqlConnection conn = new SqlConnection(ConnectionString);
                        //    if (conn.State != ConnectionState.Open)
                        //        conn.Open();
                        //    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                        //    oBcmd.CommandType = CommandType.StoredProcedure;
                        //    oBcmd.Parameters.AddWithValue("@OBId", creditMemo.OpeningBalanceId);
                        //    oBcmd.Parameters.AddWithValue("@DocumentId", creditMemo.Id);
                        //    oBcmd.Parameters.AddWithValue("@CompanyId", creditMemo.CompanyId);
                        //    oBcmd.Parameters.AddWithValue("@IsEqual", creditMemo.BalanceAmount == creditMemo.GrandTotal && (creditMemo.AllocatedAmount == 0 || creditMemo.AllocatedAmount == null) ? true : false);
                        //    int res = oBcmd.ExecuteNonQuery();
                        //    conn.Close();

                        //}

                        #region Update_Journal_Detail_Clearing_Status
                        //SqlConnection con = new SqlConnection(ConnectionString);
                        //if (con.State != ConnectionState.Open)
                        //    con.Open();
                        //SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@companyId", creditMemo.CompanyId);
                        //cmd.Parameters.AddWithValue("@documentId", creditMemo.Id);
                        //cmd.Parameters.AddWithValue("@docState", creditMemo.DocumentState);
                        //cmd.Parameters.AddWithValue("@balanceAmount", creditMemo.BalanceAmount);
                        //int count = cmd.ExecuteNonQuery();
                        //con.Close();
                        #endregion Update_Journal_Detail_Clearing_Status

                        #region Documentary History
                        //try
                        //{
                        //    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, creditMemo.CompanyId, creditMemo.Id, creditMemo.DocType, creditMemo.DocSubType, creditMemo.DocumentState, creditMemo.DocCurrency, creditMemo.GrandTotal, creditMemo.BalanceAmount, creditMemo.ExchangeRate.Value, creditMemo.ModifiedBy != null ? creditMemo.ModifiedBy : creditMemo.UserCreated, creditMemo.Remarks, null, 0,0);
                        //    if (lstdocumet.Any())
                        //        lstOfDocumentHistoryModel.AddRange(lstdocumet);
                        //    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                        #endregion Documentary History
                    }
                    //detail.ReceiptAmount = 0;
                    //detail.ObjectState = ObjectState.Modified;
                    //paymentDetailService.Update(detail);

                    if (_document.IsCustomer == true)
                    {
                        if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
                        {
                            //Invoice invoice = _invoiceService.GetInvoiceById(detail.DocumentId);
                            InvoiceCompact invoice = lstInv != null ? lstInv.Where(c => c.Id == detail.DocumentId).FirstOrDefault() : null;
                            if (invoice != null && invoice.DocType == DocTypeConstants.Invoice)
                            {
                                invoice.BalanceAmount += detail.PaymentAmount;
                                if (invoice.BalanceAmount == invoice.GrandTotal)
                                {
                                    invoice.DocumentState =/* detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.Not_Applied :*/ InvoiceState.NotPaid;
                                    invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                                    invoice.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                    invoice.ModifiedDate = DateTime.UtcNow;
                                    invoice.ModifiedBy = "System";
                                    invoice.ObjectState = ObjectState.Modified;

                                    if (invoice.IsWorkFlowInvoice == true)
                                        //FillWokflowInvoice(invoice);
                                        FillWFInvoice(invoice, ConnectionString);

                                }
                                else if (invoice.BalanceAmount != invoice.GrandTotal)
                                {
                                    invoice.DocumentState =/* detail.DocumentType == DocTypeConstants.CreditNote ? InvoiceState.PartialApplied :*/ InvoiceState.PartialPaid;

                                    //roundingSum = (Math.Round((invoice.GrandTotal - detail.PaymentAmount) * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.PaymentAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)invoice.BaseGrandTotal;
                                    invoice.BaseBalanceAmount += (Math.Round((detail.PaymentAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                    invoice.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;

                                    invoice.ModifiedDate = DateTime.UtcNow;
                                    invoice.ModifiedBy = "System";
                                    invoice.ObjectState = ObjectState.Modified;
                                    if (invoice.IsWorkFlowInvoice == true)
                                        //FillWokflowInvoice(invoice);
                                        FillWFInvoice(invoice, ConnectionString);
                                }

                                if (invoice.IsOBInvoice == true)
                                {
                                    SqlConnection conn = new SqlConnection(ConnectionString);
                                    if (conn.State != ConnectionState.Open)
                                        conn.Open();
                                    SqlCommand oBcmd = new SqlCommand("Proc_UpdateOBLineItem", conn);
                                    cmd.CommandTimeout = 30;
                                    oBcmd.CommandType = CommandType.StoredProcedure;
                                    oBcmd.Parameters.AddWithValue("@OBId", invoice.OpeningBalanceId);
                                    oBcmd.Parameters.AddWithValue("@DocumentId", invoice.Id);
                                    oBcmd.Parameters.AddWithValue("@CompanyId", invoice.CompanyId);
                                    oBcmd.Parameters.AddWithValue("@IsEqual", invoice.BalanceAmount == invoice.GrandTotal && (invoice.AllocatedAmount == 0 || invoice.AllocatedAmount == null) ? true : false);
                                    int res = oBcmd.ExecuteNonQuery();
                                    conn.Close();

                                }
                                if (invoice.IsOBInvoice != true)
                                {
                                    #region Update_Journal_Detail_Clearing_Status
                                    SqlConnection con = new SqlConnection(ConnectionString);
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                                    cmd.CommandTimeout = 30;
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@companyId", invoice.CompanyId);
                                    cmd.Parameters.AddWithValue("@documentId", invoice.Id);
                                    cmd.Parameters.AddWithValue("@docState", invoice.DocumentState);
                                    cmd.Parameters.AddWithValue("@balanceAmount", invoice.BalanceAmount);
                                    int count = cmd.ExecuteNonQuery();
                                    con.Close();
                                    #endregion Update_Journal_Detail_Clearing_Status
                                }
                                #region Documentary History
                                try
                                {
                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, null, 0, 0);
                                    if (lstdocumet.Any())
                                        lstOfDocumentHistoryModel.AddRange(lstdocumet);
                                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion Documentary History
                            }
                            else
                            {
                                CreditNoteApplicationCompact cnApplication = _invoiceCompactService.GetCNApplicationByDocId(detail.Id);
                                if (cnApplication != null)
                                {
                                    DocumentResetModel voidModel = new DocumentResetModel();
                                    voidModel.Id = cnApplication.Id;
                                    voidModel.InvoiceId = cnApplication.InvoiceId;
                                    voidModel.ResetDate = DateTime.UtcNow;
                                    voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                    voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2")));

                                    CNAplicationREST(null, voidModel, true);
                                }
                            }

                        }
                        else if (detail.DocumentType == DocTypeConstants.DebitNote)
                        {
                            //DebitNote debitNote = _debitNoteService.GetDebitNoteById(detail.DocumentId);
                            DebitNoteCompact debitNote = lstDN != null ? lstDN.Where(d => d.Id == detail.DocumentId).FirstOrDefault() : null;
                            if (debitNote != null)
                            {
                                debitNote.BalanceAmount += detail.PaymentAmount;
                                if (debitNote.BalanceAmount == debitNote.GrandTotal)
                                {
                                    debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                                    debitNote.DocumentState = InvoiceState.NotPaid;
                                    debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;
                                    debitNote.ModifiedDate = DateTime.UtcNow;
                                    debitNote.ModifiedBy = "System";
                                    debitNote.ObjectState = ObjectState.Modified;
                                }
                                else if (debitNote.BalanceAmount != debitNote.GrandTotal)
                                {
                                    debitNote.DocumentState = InvoiceState.PartialPaid;

                                    //roundingSum = (Math.Round((debitNote.GrandTotal - detail.PaymentAmount) * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) + Math.Round((detail.PaymentAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) - (decimal)debitNote.BaseGrandTotal;
                                    debitNote.BaseBalanceAmount += (Math.Round((detail.PaymentAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                    debitNote.RoundingAmount += (detail.RoundingAmount != null && detail.RoundingAmount != 0) ? detail.RoundingAmount : 0;

                                    debitNote.ModifiedDate = DateTime.UtcNow;
                                    debitNote.ModifiedBy = "System";
                                    debitNote.ObjectState = ObjectState.Modified;
                                }
                            }

                            #region Update_Journal_Detail_Clearing_Status
                            SqlConnection con = new SqlConnection(ConnectionString);
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            SqlCommand cmd = new SqlCommand("PROC_UPDATE_JOURNALDETAIL_CLEARING_STATUS", con);
                            cmd.CommandTimeout = 30;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@companyId", debitNote.CompanyId);
                            cmd.Parameters.AddWithValue("@documentId", debitNote.Id);
                            cmd.Parameters.AddWithValue("@docState", debitNote.DocumentState);
                            cmd.Parameters.AddWithValue("@balanceAmount", debitNote.BalanceAmount);
                            int count = cmd.ExecuteNonQuery();
                            con.Close();
                            #endregion Update_Journal_Detail_Clearing_Status
                            #region Documentary History
                            try
                            {
                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, debitNote.CompanyId, debitNote.Id, debitNote.DocSubType, debitNote.DocSubType, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, null, 0, 0);
                                if (lstdocumet.Any())
                                    //AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                                    lstOfDocumentHistoryModel.AddRange(lstdocumet);
                            }
                            catch (Exception ex)
                            {

                            }
                            #endregion Documentary History
                        }
                    }
                }
            }
            try
            {
                Log.Logger.ZInfo(PaymentLoggingValidation.Entered_Into_SavePaymentDocumentVoid_Of_payment_Method);
                _unitOfWorkAsync.SaveChanges();
                #region Documentary History
                try
                {
                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(TObject.Id, _document.CompanyId, _document.Id, _document.DocType, _document.DocSubType, _document.DocumentState, _document.DocCurrency, _document.GrandTotal, _document.GrandTotal, _document.ExchangeRate != null ? _document.ExchangeRate.Value : _document.SystemCalculatedExchangeRate != null ? _document.SystemCalculatedExchangeRate.Value : 1, _document.ModifiedBy != null ? _document.ModifiedBy : _document.UserCreated, _document.Remarks, null, 0, 0);
                    if (lstdocumet.Any())
                    {
                        lstOfDocumentHistoryModel.AddRange(lstdocumet);
                        AppaWorld.Bean.Common.SaveDocumentHistory(lstOfDocumentHistoryModel, ConnectionString);
                    }
                }
                catch (Exception ex)
                {

                }
                #endregion Documentary History
                //if (docSutype != DocTypeConstants.OpeningBalance)
                //{
                DocumentVoidModel tObject = new DocumentVoidModel();
                tObject.Id = TObject.Id;
                tObject.CompanyId = TObject.CompanyId;
                tObject.DocType = _document.DocSubType;
                tObject.DocNo = _document.DocNo;
                tObject.ModifiedBy = _document.ModifiedBy;
                PaymentJVPostVoid(tObject);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Come_Out_From_SavePaymentDocumentVoid_Of_Payment);
            return _document;
        }

        #endregion

        #region AutoNumber Implimentation

        #region commented_code
        //public string GenerateAutoNumberForType(long companyId, string Type, string companyCode)
        //{
        //    string generatedAutoNumber = "";
        //    try
        //    {
        //        LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entered_Into_GenerateAutoNumberForType_Of_Payment);
        //        AppsWorld.PaymentModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, Type);
        //        //if (Type == DocTypeConstants.Payment || Type == DocTypeConstants.PayrollPayment)
        //        //{
        //        generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength),
        //            _autoNo.GeneratedNumber, companyId, companyCode);

        //        if (_autoNo != null)
        //        {
        //            _autoNo.GeneratedNumber = value;
        //            _autoNo.IsDisable = true;
        //            _autoNumberService.Update(_autoNo);
        //            _autoNo.ObjectState = ObjectState.Modified;
        //        }
        //        var _autonumberCompany = _autoNumberCompanyService.GetAutoNumberCompany(_autoNo.Id);
        //        if (_autonumberCompany.Any())
        //        {
        //            AppsWorld.PaymentModule.Entities.AutoNumberCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
        //            _autoNumberCompanyNew.GeneratedNumber = value;
        //            _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
        //            _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
        //            _autoNumberCompanyService.Update(_autoNumberCompanyNew);
        //        }
        //        else
        //        {
        //            AppsWorld.PaymentModule.Entities.AutoNumberCompany _autoNumberCompanyNew = new AppsWorld.PaymentModule.Entities.AutoNumberCompany();
        //            _autoNumberCompanyNew.GeneratedNumber = value;
        //            _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
        //            _autoNumberCompanyNew.Id = Guid.NewGuid();
        //            _autoNumberCompanyNew.ObjectState = ObjectState.Added;
        //            _autoNumberCompanyService.Insert(_autoNumberCompanyNew);
        //        }
        //    }
        //    //////}
        //    catch (Exception ex)
        //    {
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Come_Out_From_GenerateAutoNumberForType_Of_Payment_Method);
        //    return generatedAutoNumber;
        //}
        //public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal,
        //    long companyId, string Companycode = null)
        //{
        //    List<Payment> lstPayment = null;
        //    int? currentMonth = 0;
        //    bool ifMonthContains = false;

        //    string OutputNumber = "";
        //    try
        //    {
        //        string counter = "";
        //        string companyFormatHere = companyFormatFrom.ToUpper();

        //        if (companyFormatHere.Contains("{YYYY}"))
        //        {
        //            companyFormatHere = companyFormatHere.Replace("{YYYY}", DateTime.Now.Year.ToString());
        //        }
        //        else if (companyFormatHere.Contains("{MM/YYYY}"))
        //        {
        //            companyFormatHere = companyFormatHere.Replace("{MM/YYYY}",
        //                string.Format("{0:00}", DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString());
        //            currentMonth = DateTime.Now.Month;
        //            ifMonthContains = true;
        //        }
        //        else if (companyFormatHere.Contains("{COMPANYCODE}"))
        //        {
        //            companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
        //        }
        //        double val = 0;
        //        //if (Type == DocTypeConstants.Payment || Type == DocTypeConstants.PayrollPayment)
        //        //{
        //        lstPayment = _paymentService.GetAllPaymentModel(companyId);


        //        if (lstPayment.Any() && ifMonthContains)
        //        {
        //            if (DateTime.Now.Year == lstPayment.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
        //            {
        //                int? lastCreatedMonth = lstPayment.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
        //                if (currentMonth == lastCreatedMonth)
        //                {
        //                    AppsWorld.PaymentModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //                    foreach (var payment in lstPayment)
        //                    {
        //                        if (payment.SystemRefNo != autonumber.Preview)
        //                            val = Convert.ToInt32(IncreamentVal);
        //                        else
        //                        {
        //                            val = Convert.ToInt32(IncreamentVal) + 1;
        //                            break;
        //                        }
        //                    }
        //                }
        //                else
        //                    val = 1;
        //            }
        //            else
        //                val = 1;
        //        }
        //        else if (lstPayment.Any() && ifMonthContains == false)
        //        {
        //            AppsWorld.PaymentModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
        //            foreach (var payment in lstPayment)
        //            {
        //                if (payment.SystemRefNo != autonumber.Preview)
        //                    val = Convert.ToInt32(IncreamentVal);
        //                else
        //                {
        //                    val = Convert.ToInt32(IncreamentVal) + 1;
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            val = Convert.ToInt32(IncreamentVal);
        //        }
        //        //}

        //        if (counterLength == 1)
        //            counter = string.Format("{0:0}", val);
        //        else if (counterLength == 2)
        //            counter = string.Format("{0:00}", val);
        //        else if (counterLength == 3)
        //            counter = string.Format("{0:000}", val);
        //        else if (counterLength == 4)
        //            counter = string.Format("{0:0000}", val);
        //        else if (counterLength == 5)
        //            counter = string.Format("{0:00000}", val);
        //        else if (counterLength == 6)
        //            counter = string.Format("{0:000000}", val);
        //        else if (counterLength == 7)
        //            counter = string.Format("{0:0000000}", val);
        //        else if (counterLength == 8)
        //            counter = string.Format("{0:00000000}", val);
        //        else if (counterLength == 9)
        //            counter = string.Format("{0:000000000}", val);
        //        else if (counterLength == 10)
        //            counter = string.Format("{0:0000000000}", val);

        //        value = counter;
        //        OutputNumber = companyFormatHere + counter;

        //        if (lstPayment.Any())
        //        {
        //            OutputNumber = GetNewNumber(lstPayment, Type, OutputNumber, counter, companyFormatHere, counterLength);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Logger.ZCritical(ex.StackTrace);
        //        throw ex;
        //    }
        //    return OutputNumber;
        //}
        //private string GetNewNumber(List<Payment> lstPayment, string type, string outputNumber, string counter, string format, int counterLength)
        //{
        //    string val1 = outputNumber;
        //    string val2 = "";
        //    var invoice = lstPayment.Where(a => a.SystemRefNo == outputNumber).FirstOrDefault();
        //    bool isNotexist = false;
        //    int i = Convert.ToInt32(counter);
        //    if (invoice != null)
        //    {
        //        var lstPayments = lstPayment.Where(a => a.SystemRefNo.Contains(format)).ToList();
        //        if (lstPayments.Any())
        //        {
        //            while (isNotexist == false)
        //            {
        //                i++;
        //                string length = i.ToString();
        //                value = length.PadLeft(counterLength, '0');
        //                val2 = format + value;
        //                var inv = lstPayments.Where(c => c.SystemRefNo == val2).FirstOrDefault();
        //                if (inv == null)
        //                    isNotexist = true;
        //            }
        //            val1 = val2;
        //        }
        //        val1 = val2;
        //    }
        //    return val1;
        //}
        #endregion commented_code

        string value = "";

        #endregion

        #region Private Method
        private void InsertPayment(PaymentModel paymentModel, Payment payment)
        {
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entred_Into_InsertPayment_Method_Of_Payment);
            payment.Id = paymentModel.Id;
            payment.CompanyId = paymentModel.CompanyId;
            payment.BankChargesCurrency = paymentModel.BankChargesCurrency;
            payment.BankClearingDate = paymentModel.BankClearingDate;
            payment.BankPaymentAmmount = paymentModel.BankPaymentAmmount;
            payment.BankPaymentAmmountCurrency = paymentModel.BankPaymentAmmountCurrency;
            payment.BaseCurrency = paymentModel.BaseCurrency;
            payment.COAId = paymentModel.COAId;
            payment.DocCurrency = paymentModel.DocCurrency;
            payment.DocDate = paymentModel.DocDate.Date;
            payment.DocSubType = paymentModel.DocSubType;
            payment.EntityType = "Vendor";
            payment.DueDate = paymentModel.DueDate.Date;
            payment.IsGstSettings = paymentModel.IsGstSettings;
            payment.PaymentApplicationAmmount = paymentModel.PaymentApplicationAmmount;
            payment.PaymentApplicationCurrency = paymentModel.PaymentApplicationCurrency;
            payment.SystemCalculatedExchangeRate = paymentModel.SystemCalculatedExchangeRate;
            if (paymentModel.VarianceExchangeRate != null)
            {
                var varience = Convert.ToDecimal(paymentModel.VarianceExchangeRate);
                payment.VarianceExchangeRate = varience;
            }
            payment.EntityId = paymentModel.EntityId;
            payment.ExDurationFrom = paymentModel.ExDurationFrom;
            payment.ExDurationTo = paymentModel.ExDurationTo;
            payment.ExCurrency = paymentModel.ExCurrency;
            payment.ExchangeRate = paymentModel.ExchangeRate;
            payment.GSTExCurrency = paymentModel.GstReportingCurrency;
            payment.IsMultiCurrency = paymentModel.ISMultiCurrency;
            payment.IsNoSupportingDocument = paymentModel.IsNoSupportingDocument;
            payment.NoSupportingDocs = paymentModel.NoSupportingDocument;
            payment.GrandTotal = paymentModel.GrandTotal;
            payment.IsBaseCurrencyRateChanged = paymentModel.IsBaseCurrencyRateChanged;
            payment.IsGSTCurrencyRateChanged = paymentModel.IsGSTCurrencyRateChanged;
            payment.Remarks = paymentModel.Remarks;
            payment.DocumentState = PaymentState.Posted;
            payment.Status = paymentModel.Status;
            payment.SystemRefNo = paymentModel.SystemRefNo;
            payment.PaymentRefNo = paymentModel.PaymentRefNo;
            payment.ServiceCompanyId = paymentModel.ServiceCompanyId;
            payment.ModeOfPayment = paymentModel.ModeOfPayment;
            payment.IsExchangeRateLabel = paymentModel.IsExchangeRateLabel;
            payment.DocType = DocTypeConstants.BillPayment;
            payment.IsCustomer = paymentModel.IsCustomer;
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Come_Out_From_InsertPayment_Of_Payment);
        }
        private bool IsDocumentNumberExists(string DocNo, Guid id, long companyId, String docType)
        {
            Payment doc = _paymentService.CheckDocNo(id, DocNo, companyId, docType);
            return doc != null;
        }
        private void FillPaymentDetail(PaymentDetail pd, PaymentDetailModel pDetail)
        {
            pd.PaymentId = pDetail.PaymentId;
            pd.DocumentDate = pDetail.DocumentDate;
            pd.DocumentId = pDetail.DocumentId;
            pd.DocumentNo = pDetail.DocumentNo;
            pd.DocumentState = pDetail.DocumentState;
            pd.DocumentType = pDetail.DocumentType;
            pd.DocumentAmmount = (pDetail.DocumentType == DocTypeConstants.BillCreditMemo || pDetail.DocumentType == DocTypeConstants.Invoice || pDetail.DocumentType == DocTypeConstants.DebitNote) ? -pDetail.DocumentAmmount : pDetail.DocumentAmmount;
            pd.Currency = pDetail.Currency;
            pd.AmmountDue = (pDetail.DocumentType == DocTypeConstants.BillCreditMemo || pDetail.DocumentType == DocTypeConstants.Invoice || pDetail.DocumentType == DocTypeConstants.DebitNote) ? -pDetail.AmmountDue.Value : pDetail.AmmountDue.Value;
            pd.Nature = pDetail.Nature;
            pd.SystemReferenceNumber = pDetail.SystemReferenceNumber;
            pd.ServiceCompanyId = pDetail.ServiceCompanyId;
        }
        private void UpdatePaymentDetails(PaymentModel TObject, Payment payment, bool isEdit, string ConnectionString, int? serEntCount, long? clearingPaymentsCoaId, decimal? oldExchangerate, decimal? oldSysCalExRate, List<DocumentHistoryModel> lstOfDocumentHistory, DateTime? oldDocDate, Dictionary<Guid?, decimal?> lstADocValue, long? ServiceCompanyId)
        {
            long? icCOAId = null;
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Enterd_Into_UpdatePaymentDetails_Of_Payment);
            decimal? roundingSum = 0;
            decimal oldPaymentAmount = 0;
            #region Delete_Old_not_matching_Payments
            if (payment != null && isEdit)
            {
                List<CreditMemoCompact> lstCreditMemos = null;
                List<InvoiceCompact> lstInvAndCns = null;
                List<DebitNoteCompact> lstDebitNote = null;
                //  List<Guid?> lstCMApps = null;
                // List<Guid?> lstCNApplications = null;
                List<Bill> lstBill = _billService.GetBill(payment.PaymentDetails.Where(a => a.DocumentId != Guid.Empty).Select(c => c.DocumentId).ToList());
                lstCreditMemos = _creditMemoCompactService.GetListOfCreditMemoCompacts(payment.CompanyId, payment.PaymentDetails.Where(a => a.DocumentId != Guid.Empty && a.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList());
                lstInvAndCns = _invoiceCompactService.GetListOfInvoiceAndCNs(payment.CompanyId, payment.PaymentDetails.Where(a => a.DocumentId != Guid.Empty && (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.CreditNote)).Select(c => c.DocumentId).ToList());
                lstDebitNote = _debitNoteCompactService.GetListOfDebitNotesByDocIds(payment.CompanyId, payment.PaymentDetails.Where(a => a.DocumentType == DocTypeConstants.DebitNote && a.DocumentId != Guid.NewGuid()).Select(c => c.DocumentId).ToList());

                List<AppsWorld.PaymentModule.Entities.Journal> lstJournal = _journalServices.GetAllJournalsByDocId(payment.PaymentDetails.Select(c => c.DocumentId).ToList(), TObject.CompanyId);

                foreach (PaymentDetail pdetail in payment.PaymentDetails)
                {
                    if (!TObject.PaymentDetailModels.Any(a => a.DocumentId == pdetail.DocumentId))
                    {
                        if (pdetail.DocumentType == DocTypeConstants.Bills || pdetail.DocumentType == DocTypeConstants.General || pdetail.DocumentType == DocTypeConstants.OpeningBalance || pdetail.DocumentType == DocTypeConstants.Claim || pdetail.DocumentType == DocTypeConstants.Payroll)
                        {
                            if (lstBill.Any())
                            {
                                Bill bill = lstBill.FirstOrDefault(c => c.Id == pdetail.DocumentId);
                                if (bill != null)
                                {
                                    bill.BalanceAmount += pdetail.PaymentAmount;
                                    if (bill.GrandTotal == bill.BalanceAmount)
                                    {
                                        bill.DocumentState = BillNoteState.NotPaid;
                                        bill.BaseBalanceAmount = bill.BaseGrandTotal;
                                    }
                                    if (bill.GrandTotal != bill.BalanceAmount)
                                    {
                                        bill.DocumentState = BillNoteState.PartialPaid;
                                        //Newly Added for 0.01 issue
                                        bill.BaseBalanceAmount += (Math.Round((pdetail.PaymentAmount * (bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                        bill.RoundingAmount = pdetail.RoundingAmount;
                                    }
                                    if (bill.BalanceAmount == 0)
                                        bill.DocumentState = BillNoteState.FullyPaid;
                                    if (bill.DocSubType == DocTypeConstants.OpeningBalance)
                                    {
                                        UpdateOBLineItem(bill.OpeningBalanceId, bill.Id, bill.CompanyId, bill.BalanceAmount == bill.GrandTotal, ConnectionString);
                                    }
                                    bill.ObjectState = ObjectState.Modified;
                                    bill.ModifiedDate = DateTime.UtcNow;
                                    bill.ModifiedBy = PaymentsConstants.System;
                                    bill.ObjectState = ObjectState.Modified;
                                    _billService.Update(bill);

                                    try
                                    {
                                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(payment.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount != null ? bill.BalanceAmount.Value : bill.GrandTotal, bill.ExchangeRate != null ? bill.ExchangeRate.Value : 1, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, payment.Remarks, null, 0, 0);
                                        if (lstdocumet.Any())
                                            lstOfDocumentHistory.AddRange(lstdocumet);

                                    }
                                    catch (Exception ex)
                                    {
                                        //throw ex;
                                    }

                                }
                                AppsWorld.PaymentModule.Entities.Journal journal = lstJournal.FirstOrDefault(c => c.DocumentId == pdetail.DocumentId);
                                if (journal != null)
                                {
                                    journal.BalanceAmount += pdetail.PaymentAmount;
                                    if (journal.GrandDocDebitTotal == journal.BalanceAmount)
                                        journal.DocumentState = BillNoteState.NotPaid;
                                    if (journal.GrandDocDebitTotal < journal.BalanceAmount)
                                        journal.DocumentState = BillNoteState.PartialPaid;
                                    if (journal.BalanceAmount == 0)
                                        journal.DocumentState = BillNoteState.FullyPaid;
                                    journal.ObjectState = ObjectState.Modified;
                                    journal.ModifiedDate = DateTime.UtcNow;
                                    journal.ModifiedBy = PaymentsConstants.System;
                                    _journalServices.Update(journal);
                                }
                                pdetail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        else if (pdetail.DocumentType == DocTypeConstants.BillCreditMemo)
                        {
                            if (lstCreditMemos != null)
                            {
                                CreditMemoCompact creditMemo = lstCreditMemos.FirstOrDefault(a => a.Id == pdetail.DocumentId);
                                if (creditMemo != null)
                                {
                                    creditMemo.BalanceAmount += pdetail.PaymentAmount;
                                    if (creditMemo.GrandTotal == creditMemo.BalanceAmount)
                                    {
                                        creditMemo.DocumentState = CreditMemoState.Not_Applied;
                                        creditMemo.BaseBalanceAmount = creditMemo.BaseGrandTotal;
                                    }
                                    else if (creditMemo.GrandTotal != creditMemo.BalanceAmount)
                                    {
                                        creditMemo.DocumentState = CreditMemoState.Partial_Applied;
                                        //Newly Added for 0.01 issue

                                        creditMemo.BaseBalanceAmount += (Math.Round((pdetail.PaymentAmount * (creditMemo.ExchangeRate != null ? (decimal)creditMemo.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                        creditMemo.RoundingAmount = pdetail.RoundingAmount;
                                    }
                                    else if (creditMemo.BalanceAmount == 0)
                                        creditMemo.DocumentState = CreditMemoState.Not_Applied;
                                    creditMemo.ModifiedBy = PaymentsConstants.System;
                                    creditMemo.ModifiedDate = DateTime.UtcNow;
                                    creditMemo.ObjectState = ObjectState.Modified;
                                    _creditMemoCompactService.Update(creditMemo);

                                    if (creditMemo.DocSubType != DocTypeConstants.OpeningBalance)
                                    {
                                        AppsWorld.PaymentModule.Entities.Journal journal = lstJournal.Where(a => a.DocumentId == creditMemo.Id).FirstOrDefault();
                                        if (journal != null)
                                        {
                                            journal.BalanceAmount = creditMemo.BalanceAmount;
                                            journal.DocumentState = creditMemo.DocumentState;
                                            journal.ModifiedBy = PaymentsConstants.System;
                                            journal.ModifiedDate = DateTime.UtcNow;
                                            journal.ObjectState = ObjectState.Modified;
                                            _journalServices.Update(journal);
                                        }
                                    }
                                    else
                                    {
                                        UpdateOBLineItem(creditMemo.OpeningBalanceId, creditMemo.Id, creditMemo.CompanyId, creditMemo.BalanceAmount == creditMemo.GrandTotal, ConnectionString);
                                    }
                                    CreditMemoApplicationCompact cmApplication = _creditMemoCompactService.GetCMApplicationByDocId(pdetail.Id);
                                    if (cmApplication != null)
                                    {
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(cmApplication != null ? cmApplication.Id : payment.Id, creditMemo.CompanyId, creditMemo.Id, creditMemo.DocType, creditMemo.DocSubType, creditMemo.DocumentState, creditMemo.DocCurrency, creditMemo.GrandTotal, creditMemo.BalanceAmount, creditMemo.ExchangeRate != null ? creditMemo.ExchangeRate.Value : 1, creditMemo.ModifiedBy != null ? creditMemo.ModifiedBy : creditMemo.UserCreated, payment.Remarks, null, 0, 0);

                                            if (lstdocumet.Any())
                                            {

                                                lstOfDocumentHistory.AddRange(lstdocumet);
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            //throw ex;
                                        }

                                        con = new SqlConnection(ConnectionString);
                                        if (con.State != ConnectionState.Open)
                                            con.Open();
                                        cmd = new SqlCommand("DELETE_POSTING_JOURNAL", con);
                                        cmd.CommandTimeout = 30;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@DocumentId", cmApplication.Id);
                                        cmd.Parameters.AddWithValue("@CompanyId", cmApplication.CompanyId);
                                        cmd.Parameters.AddWithValue("@DocType", DocTypeConstants.CMApplication);
                                        cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                                pdetail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        else if (pdetail.DocumentType == DocTypeConstants.Invoice || pdetail.DocumentType == DocTypeConstants.CreditNote)
                        {
                            if (lstInvAndCns != null)
                            {
                                InvoiceCompact invoice = lstInvAndCns.FirstOrDefault(a => a.Id == pdetail.DocumentId);
                                if (invoice != null && invoice.DocType == DocTypeConstants.Invoice || invoice.DocType == DocTypeConstants.CreditNote)
                                {
                                    invoice.BalanceAmount += pdetail.PaymentAmount;
                                    if (invoice.GrandTotal == invoice.BalanceAmount)
                                    {
                                        invoice.DocumentState = pdetail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.NotApplied : InvoiceStates.NotPaid;
                                        invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                                    }
                                    else if (invoice.BalanceAmount == 0)
                                        invoice.DocumentState = pdetail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.FullyApplied : InvoiceStates.FullyPaid;
                                    else
                                    {
                                        invoice.DocumentState = pdetail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.PartialApplied : InvoiceStates.PartialPaid;
                                        //Newly Added for 0.01 issue
                                        invoice.BaseBalanceAmount += (Math.Round((pdetail.PaymentAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                        invoice.RoundingAmount = pdetail.RoundingAmount;
                                    }

                                    if (invoice.IsWorkFlowInvoice == true)
                                        FillWFInvoice(invoice, ConnectionString);
                                    invoice.ModifiedBy = PaymentsConstants.System;
                                    invoice.ModifiedDate = DateTime.UtcNow;
                                    invoice.ObjectState = ObjectState.Modified;
                                    if (invoice.IsOBInvoice != true)
                                    {
                                        AppsWorld.PaymentModule.Entities.Journal journal = lstJournal.FirstOrDefault(a => a.DocumentId == invoice.Id);
                                        if (journal != null)
                                        {
                                            journal.BalanceAmount = invoice.BalanceAmount;
                                            journal.DocumentState = invoice.DocumentState;
                                            journal.ModifiedBy = PaymentsConstants.System;
                                            journal.ModifiedDate = DateTime.UtcNow;
                                            journal.ObjectState = ObjectState.Modified;
                                            _journalServices.Update(journal);
                                        }
                                    }
                                    else
                                    {
                                        UpdateOBLineItem(invoice.OpeningBalanceId, invoice.Id, invoice.CompanyId, invoice.DocumentState == InvoiceStates.NotPaid, ConnectionString);

                                    }
                                    Guid cnAppId = Guid.Empty;
                                    if (pdetail.DocumentType == DocTypeConstants.CreditNote)
                                    {
                                        CreditNoteApplicationCompact cnApplication = _invoiceCompactService.GetCNApplicationByDocId(pdetail.Id);
                                        if (cnApplication != null)
                                        {
                                            cnAppId = cnApplication.Id;
                                            con = new SqlConnection(ConnectionString);
                                            if (con.State != ConnectionState.Open)
                                                con.Open();
                                            cmd = new SqlCommand("DELETE_POSTING_JOURNAL", con);
                                            cmd.CommandTimeout = 30;
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@DocumentId", cnApplication.Id);
                                            cmd.Parameters.AddWithValue("@CompanyId", cnApplication.CompanyId);
                                            cmd.Parameters.AddWithValue("@DocType", DocTypeConstants.Application);
                                            cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                    }
                                    try
                                    {
                                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(pdetail.DocumentType == DocTypeConstants.CreditNote ? cnAppId : payment.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate != null ? invoice.ExchangeRate.Value : 1, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, payment.Remarks, null, 0, 0);


                                        if (lstdocumet.Any())
                                            lstOfDocumentHistory.AddRange(lstdocumet);
                                    }
                                    catch (Exception ex)
                                    {
                                        //throw ex;
                                    }
                                }
                                pdetail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        else if (pdetail.DocumentType == DocTypeConstants.DebitNote)
                        {
                            if (lstDebitNote != null)
                            {
                                DebitNoteCompact debitNote = lstDebitNote.FirstOrDefault(a => a.Id == pdetail.DocumentId);
                                if (debitNote != null)
                                {
                                    debitNote.BalanceAmount += pdetail.PaymentAmount;
                                    if (debitNote.GrandTotal == debitNote.BalanceAmount)
                                    {
                                        debitNote.DocumentState = InvoiceState.NotPaid;
                                        debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                                    }
                                    else if (debitNote.BalanceAmount == 0)
                                        debitNote.DocumentState = InvoiceState.FullyPaid;
                                    else
                                    {
                                        debitNote.DocumentState = InvoiceState.PartialPaid;
                                        //Newly Added for 0.01 issue
                                        debitNote.BaseBalanceAmount += (Math.Round((pdetail.PaymentAmount * (debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                        debitNote.RoundingAmount = pdetail.RoundingAmount;
                                    }

                                    debitNote.ModifiedBy = PaymentsConstants.System;
                                    debitNote.ModifiedDate = DateTime.UtcNow;
                                    debitNote.ObjectState = ObjectState.Modified;
                                    AppsWorld.PaymentModule.Entities.Journal journal = lstJournal.FirstOrDefault(a => a.DocumentId == debitNote.Id);
                                    if (journal != null)
                                    {
                                        journal.BalanceAmount = debitNote.BalanceAmount;
                                        journal.DocumentState = debitNote.DocumentState;
                                        journal.ModifiedBy = PaymentsConstants.System;
                                        journal.ModifiedDate = DateTime.UtcNow;
                                        journal.ObjectState = ObjectState.Modified;
                                        _journalServices.Update(journal);
                                    }
                                }
                                try
                                {
                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(payment.Id, debitNote.CompanyId, debitNote.Id, DocTypeConstants.DebitNote, DocTypeConstants.General, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate != null ? debitNote.ExchangeRate.Value : 1, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, payment.Remarks, null, 0, 0);
                                    if (lstdocumet.Any())
                                        lstOfDocumentHistory.AddRange(lstdocumet);
                                }
                                catch (Exception ex)
                                {
                                    //throw ex;
                                }
                            }
                            pdetail.ObjectState = ObjectState.Deleted;
                        }
                    }
                }
            }
            #endregion Delete_Old_not_matching_Payments

            List<Bill> lstAllBills = null;
            List<CreditMemoCompact> lstCMs = null;
            List<InvoiceCompact> lstInvAndCNs = null;
            List<AppsWorld.PaymentModule.Entities.Journal> lstJournals = null;
            List<CreditMemoApplicationCompact> lstCreditMemoApps = null;
            List<CreditNoteApplicationCompact> lstCNApps = null;
            List<DebitNoteCompact> lstDebitNotes = null;
            decimal? sumAllValue = 0;
            decimal? allGrandTotal = 0;
            string docState = null;


            if (TObject.PaymentDetailModels.Any())
            {
                lstAllBills = _billService.GetAllBillsByDocumentId(TObject.PaymentDetailModels.Where(a => (a.DocumentType != DocTypeConstants.BillCreditMemo && a.DocumentType != DocTypeConstants.Invoice && a.DocumentType != DocTypeConstants.CreditNote)).Select(c => c.DocumentId).ToList(), TObject.PaymentDetailModels.Select(c => c.DocumentType).ToList(), TObject.PaymentDetailModels.Select(c => c.Currency).FirstOrDefault(), TObject.EntityId);
                if (TObject.DocSubType != DocTypeConstants.Payroll)
                {
                    lstCMs = _creditMemoCompactService.GetAllCMsByDocId(TObject.PaymentDetailModels.Where(a => a.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList(), TObject.PaymentDetailModels.Select(c => c.Currency).FirstOrDefault(), TObject.EntityId, TObject.CompanyId);
                    if (lstCMs.Any())
                    {
                        lstCreditMemoApps = _creditMemoCompactService.GetListOfCreditMemoAppsByCMIds(lstCMs.Select(a => a.Id).ToList(), payment.CompanyId);
                    }
                }
                if (TObject.IsCustomer == true)
                {
                    lstInvAndCNs = _invoiceCompactService.GetlistOfInvAndCNByDocIds(TObject.PaymentDetailModels.Where(a => (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.CreditNote)).Select(c => c.DocumentId).ToList(), TObject.PaymentDetailModels.Select(c => c.Currency).FirstOrDefault(), TObject.EntityId, TObject.CompanyId);
                    if (lstInvAndCNs.Any())
                    {
                        lstCNApps = _invoiceCompactService.GetListOfCNAppsByCNId(lstInvAndCNs.Where(a => a.DocType == DocTypeConstants.CreditNote).Select(a => a.Id).ToList(), payment.CompanyId);
                    }
                    lstDebitNotes = _debitNoteCompactService.GetlistDNByDocIds(TObject.PaymentDetailModels.Where(a => (a.DocumentType == DocTypeConstants.DebitNote)).Select(c => c.DocumentId).ToList(), TObject.PaymentDetailModels.Select(c => c.Currency).FirstOrDefault(), TObject.EntityId, TObject.CompanyId);
                }
                lstJournals = _journalServices.GetAllJournalsByDocId(TObject.PaymentDetailModels.Select(c => c.DocumentId).ToList(), TObject.CompanyId);
                if (serEntCount > 1)
                    icCOAId = _chartOfAccountService.GetICAccountId(payment.CompanyId, payment.ServiceCompanyId);

                //Checking the documentstate before proceeding to save the payment details details
                if ((lstInvAndCNs != null && lstInvAndCNs.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstDebitNotes != null && lstDebitNotes.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstAllBills != null && lstAllBills.Any(a => a.DocumentState == InvoiceStates.Void)) || (lstCMs != null && lstCMs.Any(a => a.DocumentState == InvoiceStates.Void)))
                    throw new InvalidOperationException(CommonConstant.Outstanding_transactions_list_has_changed_Please_refresh_to_proceed);
            }

            #region 2_Tab_Manupulation 

            if (lstAllBills != null)
            {
                sumAllValue += lstAllBills.Sum(c => c.BalanceAmount);
                allGrandTotal += lstAllBills.Sum(d => d.GrandTotal);
            }
            if (lstCMs != null)
            {
                sumAllValue += lstCMs.Sum(c => c.BalanceAmount);
                allGrandTotal += lstCMs.Sum(d => d.GrandTotal);
            }
            if (lstInvAndCNs != null)
            {
                sumAllValue += lstInvAndCNs.Sum(d => d.BalanceAmount);
                allGrandTotal += lstInvAndCNs.Sum(d => d.GrandTotal);
            }
            if (lstDebitNotes != null)
            {
                sumAllValue += lstDebitNotes.Sum(d => d.BalanceAmount);
                allGrandTotal += lstDebitNotes.Sum(d => d.GrandTotal);
            }
            if ((sumAllValue != 0 || allGrandTotal != 0) && TObject.PaymentDetailModels.Any())
            {

                if (isEdit)
                {
                    if (allGrandTotal < (TObject.PaymentDetailModels.Sum(d => Math.Abs(d.PaymentAmount))) || allGrandTotal != TObject.PaymentDetailModels.Sum(c => Math.Abs(c.DocumentAmmount)))
                        throw new InvalidOperationException(PaymentValidations.Payment_Status_Change);
                }
                else
                        if ((sumAllValue != TObject.PaymentDetailModels.Sum(d => Math.Abs((decimal)d.AmmountDue))) || allGrandTotal != TObject.PaymentDetailModels.Sum(c => Math.Abs(c.DocumentAmmount)))
                    throw new InvalidOperationException(PaymentValidations.Payment_Status_Change);

            }

            #endregion 2_Tab_Manupulation

            List<PaymentDetail> lstPaymentDetails = _paymentDetailService.GetListOfPaymentDetails(TObject.PaymentDetailModels.Where(a => a.RecordStatus != "Added" || a.RecordStatus != "Deleted").Select(c => c.Id).ToList());
            foreach (PaymentDetailModel detail in isEdit ? TObject.PaymentDetailModels : TObject.PaymentDetailModels.Where(d => d.PaymentAmount != 0))
            {
                roundingSum = 0;
                if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                {
                    PaymentDetail paymentDetail = lstPaymentDetails.FirstOrDefault(a => a.Id == detail.Id);
                    if (paymentDetail != null)
                    {
                        oldPaymentAmount = Math.Abs(paymentDetail.PaymentAmount);
                        FillPaymentDetail(paymentDetail, detail);


                        if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.PayrollBill || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                        {
                            #region Bill
                            Bill bill = lstAllBills != null ? lstAllBills.FirstOrDefault(c => c.Id == detail.DocumentId && c.CompanyId == TObject.CompanyId) : null;
                            if (bill != null && bill.DocumentState == InvoiceState.FullyPaid && oldPaymentAmount == detail.PaymentAmount && oldPaymentAmount != 0 && detail.PaymentAmount != 0)
                            {

                                if (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null)
                                {
                                    lstADocValue.Add(bill.Id, paymentDetail.RoundingAmount.Value);
                                    roundingSum = paymentDetail.RoundingAmount;
                                }
                            }
                            if (paymentDetail.PaymentAmount > detail.PaymentAmount || paymentDetail.PaymentAmount < detail.PaymentAmount)
                            {
                                if (paymentDetail.PaymentAmount > detail.PaymentAmount)
                                {
                                    decimal Ammount = 0;
                                    Ammount = paymentDetail.PaymentAmount - detail.PaymentAmount;
                                    paymentDetail.PaymentAmount = paymentDetail.PaymentAmount - Ammount;
                                    if (bill != null)
                                    {
                                        docState = bill.DocumentState;
                                        bill.BalanceAmount = bill.BalanceAmount + Ammount;
                                        if (bill.GrandTotal == bill.BalanceAmount)
                                        {
                                            bill.DocumentState = InvoiceState.NotPaid;
                                            bill.BaseBalanceAmount = bill.BaseGrandTotal;
                                            bill.RoundingAmount += (paymentDetail.RoundingAmount != null && paymentDetail.RoundingAmount != 0) ? paymentDetail.RoundingAmount : 0;
                                            paymentDetail.RoundingAmount = 0;
                                        }
                                        else if (bill.GrandTotal > bill.BalanceAmount)
                                        {
                                            bill.DocumentState = InvoiceState.PartialPaid;
                                            bill.BaseBalanceAmount = oldPaymentAmount > detail.PaymentAmount ? bill.BaseBalanceAmount + (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : bill.BaseBalanceAmount - (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                            if (docState == InvoiceStates.FullyPaid && (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null))
                                            {
                                                bill.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? paymentDetail.RoundingAmount : bill.RoundingAmount;
                                                paymentDetail.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? 0 : paymentDetail.RoundingAmount;
                                            }
                                        }
                                        bill.ObjectState = ObjectState.Modified;
                                        bill.ModifiedDate = DateTime.UtcNow;
                                        bill.ModifiedBy = PaymentsConstants.System;
                                        _billService.Update(bill);
                                        if (bill.DocSubType != DocTypeConstants.OpeningBalance)
                                        {
                                            UpdatePosting up = new UpdatePosting();
                                            FillJournalState(up, bill);
                                            UpdatePosting(up);
                                        }
                                        else
                                        {
                                            UpdateOBLineItem(bill.OpeningBalanceId, bill.Id, bill.CompanyId, bill.BalanceAmount == bill.GrandTotal, ConnectionString);

                                        }
                                    }
                                }
                                else if (paymentDetail.PaymentAmount < detail.PaymentAmount)
                                {
                                    if (detail.AmmountDue == detail.PaymentAmount)
                                    {
                                        bill = lstAllBills != null ? lstAllBills.FirstOrDefault(c => c.Id == detail.DocumentId && c.CompanyId == TObject.CompanyId) : null;
                                        if (bill != null)
                                        {
                                            //Newly added for 0.01 changes

                                            if (detail.PaymentAmount == bill.GrandTotal)
                                            {
                                                roundingSum = Math.Round(Math.Abs(detail.PaymentAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)bill.BaseGrandTotal);
                                                if (roundingSum != 0)
                                                    lstADocValue.Add(bill.Id, roundingSum);
                                                paymentDetail.RoundingAmount = roundingSum;
                                                bill.RoundingAmount = (roundingSum != null && roundingSum != 0 && (bill.RoundingAmount != null && bill.RoundingAmount != 0)) ? bill.RoundingAmount - roundingSum : 0;
                                                bill.BaseBalanceAmount = 0;
                                            }

                                            else if (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null)
                                            {
                                                bill.BaseBalanceAmount = 0;
                                                lstADocValue.Add(bill.Id, paymentDetail.RoundingAmount.Value);
                                                paymentDetail.RoundingAmount = paymentDetail.RoundingAmount;
                                                roundingSum = paymentDetail.RoundingAmount;
                                            }
                                            else
                                            {
                                                if (bill.RoundingAmount != null && bill.RoundingAmount != 0)
                                                    roundingSum = ((bill.RoundingAmount != null && bill.RoundingAmount != 0) ? (decimal)bill.RoundingAmount : 0);
                                                else
                                                    roundingSum = Math.Round(Math.Abs(detail.PaymentAmount - oldPaymentAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)bill.BaseBalanceAmount);

                                                bill.BaseBalanceAmount = 0;
                                                if (roundingSum != 0)
                                                    lstADocValue.Add(bill.Id, roundingSum);
                                                paymentDetail.RoundingAmount = roundingSum;
                                                bill.RoundingAmount = (roundingSum != null && roundingSum != 0 && (bill.RoundingAmount != null && bill.RoundingAmount != 0)) ? bill.RoundingAmount - roundingSum : 0;
                                            }


                                            bill.BalanceAmount = 0;
                                            bill.DocumentState = InvoiceState.FullyPaid;
                                            bill.BaseBalanceAmount = 0;
                                            bill.ObjectState = ObjectState.Modified;
                                            bill.ModifiedDate = DateTime.UtcNow;
                                            bill.ModifiedBy = PaymentsConstants.System;
                                            _billService.Update(bill);
                                            if (bill.DocSubType != DocTypeConstants.OpeningBalance)
                                            {
                                                UpdatePosting up = new UpdatePosting();
                                                FillJournalState(up, bill);
                                                UpdatePosting(up);
                                            }
                                            else
                                            {
                                                UpdateOBLineItem(bill.OpeningBalanceId, bill.Id, bill.CompanyId, false, ConnectionString);

                                            }
                                        }
                                    }
                                    else if (detail.AmmountDue != detail.PaymentAmount)
                                    {
                                        bill = lstAllBills != null ? lstAllBills.Where(c => c.Id == detail.DocumentId && c.CompanyId == TObject.CompanyId).FirstOrDefault() : null;
                                        if (bill != null)
                                        {
                                            bill.BalanceAmount = detail.AmmountDue - detail.PaymentAmount;
                                            bill.DocumentState = InvoiceState.PartialPaid;
                                            //Newly Added for 0.01 issue
                                            bill.BaseBalanceAmount = oldPaymentAmount > detail.PaymentAmount ? bill.BaseBalanceAmount + (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : bill.BaseBalanceAmount - (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                            if (docState == InvoiceStates.FullyPaid && (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null))
                                            {
                                                bill.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? paymentDetail.RoundingAmount : bill.RoundingAmount;
                                                paymentDetail.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != bill.DocumentState)) ? 0 : paymentDetail.RoundingAmount;
                                            }


                                            bill.ObjectState = ObjectState.Modified;
                                            bill.ModifiedDate = DateTime.UtcNow;
                                            bill.ModifiedBy = PaymentsConstants.System;
                                            _billService.Update(bill);

                                            if (bill.DocSubType != DocTypeConstants.OpeningBalance)
                                            {
                                                UpdatePosting up = new UpdatePosting();
                                                FillJournalState(up, bill);
                                                UpdatePosting(up);
                                            }
                                            else
                                            {
                                                UpdateOBLineItem(bill.OpeningBalanceId, bill.Id, bill.CompanyId, false, ConnectionString);

                                            }
                                        }

                                    }


                                }
                                #region Documentary History
                                try
                                {
                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, bill.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);
                                    if (lstdocumet.Any())
                                        lstOfDocumentHistory.AddRange(lstdocumet);
                                }
                                catch (Exception ex)
                                {

                                }
                                #endregion Documentary History
                            }

                            paymentDetail.PaymentAmount = detail.PaymentAmount;
                            paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                            #endregion
                        }
                        else
                        {
                            if (payment.DocSubType != DocTypeConstants.Payroll)
                            {
                                #region Credit_Memo
                                if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                                {
                                    decimal? oldAmount = paymentDetail.PaymentAmount;
                                    paymentDetail.PaymentAmount = -detail.PaymentAmount;
                                    paymentDetail.DocumentAmmount = -detail.DocumentAmmount;
                                    detail.PaymentAmount = -detail.PaymentAmount;
                                    detail.AmmountDue = -detail.AmmountDue;
                                    paymentDetail.AmmountDue = detail.AmmountDue.Value;
                                    CreditMemoCompact creditMemo = lstCMs.Where(a => a.Id == detail.DocumentId).FirstOrDefault();
                                    CreditMemoApplicationModel creditMemoAppModel = new CreditMemoApplicationModel();
                                    if (creditMemo != null)
                                    {
                                        CreditMemoApplicationCompact cmApplication = lstCreditMemoApps != null ? lstCreditMemoApps.Where(a => a.CreditMemoId == creditMemo.Id && a.DocumentId == detail.Id).FirstOrDefault() : null;
                                        if (cmApplication != null)
                                        {
                                            if (detail.PaymentAmount > 0)
                                            {
                                                if (oldAmount != detail.PaymentAmount || oldExchangerate != payment.ExchangeRate || oldSysCalExRate != payment.SystemCalculatedExchangeRate || oldDocDate != TObject.DocDate || ServiceCompanyId != TObject.ServiceCompanyId)
                                                {
                                                    FillCreditMemoAplication(creditMemoAppModel, creditMemo, paymentDetail, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                    creditMemoAppModel.Id = cmApplication.Id;
                                                    creditMemoAppModel.CreditMemoBalanceAmount = creditMemo.DocumentState != CreditMemoState.Fully_Applied ? creditMemo.BalanceAmount + cmApplication.CreditAmount : paymentDetail.PaymentAmount;
                                                    creditMemoAppModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                    creditMemoAppModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                    creditMemoAppModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    creditMemoAppModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));
                                                    CMAplicationREST(creditMemoAppModel, null, false);
                                                }
                                            }
                                            else
                                            {
                                                //need to void the record
                                                DocumentResetModel voidModel = new DocumentResetModel();
                                                voidModel.Id = cmApplication.Id;
                                                voidModel.CreditMemoId = cmApplication.CreditMemoId;
                                                voidModel.ResetDate = DateTime.UtcNow;
                                                voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cmApplication.Version, x => x.ToString("X2")));
                                                CMAplicationREST(null, voidModel, true);
                                            }
                                        }
                                        else
                                        {
                                            //create a new Credit Memo Application
                                            if (detail.PaymentAmount > 0)
                                            {
                                                FillCreditMemoAplication(creditMemoAppModel, creditMemo, paymentDetail, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                creditMemoAppModel.Id = Guid.NewGuid();
                                                creditMemoAppModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                creditMemoAppModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                creditMemoAppModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                CMAplicationREST(creditMemoAppModel, null, false);
                                            }
                                        }
                                    }
                                    paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                                    _paymentDetailService.Update(paymentDetail);
                                }
                                #endregion

                                if (TObject.IsCustomer == true)
                                {
                                    #region Invoice_and_CN
                                    InvoiceCompact invoice = new InvoiceCompact();
                                    if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
                                    {
                                        decimal Ammount = 0;
                                        if (detail.DocumentType == DocTypeConstants.Invoice)
                                        {
                                            detail.PaymentAmount = -detail.PaymentAmount;
                                            detail.AmmountDue = -detail.AmmountDue;
                                            detail.DocumentAmmount = -detail.DocumentAmmount;
                                        }
                                        invoice = lstInvAndCNs != null ? lstInvAndCNs.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                        if (invoice != null && invoice.DocumentState == InvoiceState.FullyPaid && oldPaymentAmount == detail.PaymentAmount && oldPaymentAmount != 0 && detail.PaymentAmount != 0)
                                        {

                                            if (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null)
                                            {
                                                lstADocValue.Add(invoice.Id, paymentDetail.RoundingAmount.Value);
                                                roundingSum = paymentDetail.RoundingAmount;
                                            }
                                        }
                                        if (paymentDetail.PaymentAmount > detail.PaymentAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                        {
                                            Ammount = paymentDetail.PaymentAmount - (detail.PaymentAmount);
                                            paymentDetail.PaymentAmount = paymentDetail.PaymentAmount - Ammount;

                                            if (invoice != null)
                                            {
                                                docState = invoice.DocumentState;
                                                invoice.BalanceAmount = invoice.BalanceAmount + Ammount;
                                                if (invoice.GrandTotal == invoice.BalanceAmount)
                                                {
                                                    invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.NotApplied : InvoiceState.NotPaid;
                                                    invoice.BaseBalanceAmount = invoice.BaseGrandTotal;
                                                    invoice.RoundingAmount += (paymentDetail.RoundingAmount != null && paymentDetail.RoundingAmount != 0) ? paymentDetail.RoundingAmount : 0;
                                                    paymentDetail.RoundingAmount = 0;
                                                }
                                                else if (invoice.GrandTotal > invoice.BalanceAmount)
                                                {
                                                    invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.PartialApplied : InvoiceState.PartialPaid;
                                                    //Newly Added for 0.01 issue
                                                    invoice.BaseBalanceAmount = oldPaymentAmount > detail.PaymentAmount ? invoice.BaseBalanceAmount + (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : invoice.BaseBalanceAmount - (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));
                                                    if (docState == InvoiceStates.FullyPaid && (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null))
                                                    {
                                                        invoice.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? paymentDetail.RoundingAmount : invoice.RoundingAmount;
                                                        paymentDetail.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? 0 : paymentDetail.RoundingAmount;
                                                    }

                                                }
                                                invoice.ModifiedBy = PaymentsConstants.System;
                                                invoice.ModifiedDate = DateTime.UtcNow;
                                                invoice.ObjectState = ObjectState.Modified;
                                                _invoiceCompactService.Update(invoice);
                                                if (invoice.IsWorkFlowInvoice == true)
                                                    FillWFInvoice(invoice, ConnectionString);
                                                if (invoice.IsOBInvoice == true)
                                                {

                                                    UpdateOBLineItem(invoice.OpeningBalanceId, invoice.Id, invoice.CompanyId, invoice.DocumentState == InvoiceStates.NotPaid, ConnectionString);

                                                }
                                                if (invoice.IsOBInvoice != true)
                                                {
                                                    AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault() : null;
                                                    if (journal != null)
                                                    {
                                                        journal.BalanceAmount = invoice.BalanceAmount;
                                                        journal.DocumentState = invoice.DocumentState;
                                                        journal.ModifiedBy = PaymentsConstants.System;
                                                        journal.ModifiedDate = DateTime.UtcNow;
                                                        journal.ObjectState = ObjectState.Modified;
                                                        _journalServices.Update(journal);
                                                    }

                                                }
                                                #region Documentary History
                                                if (detail.DocumentType != DocTypeConstants.CreditNote)
                                                {
                                                    try
                                                    {
                                                        List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);
                                                        if (lstdocumet.Any())
                                                            lstOfDocumentHistory.AddRange(lstdocumet);
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                                #endregion Documentary History

                                            }
                                            paymentDetail.PaymentAmount = detail.PaymentAmount;
                                            paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                                        }
                                        else if (paymentDetail.PaymentAmount < detail.PaymentAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                        {
                                            Ammount = paymentDetail.PaymentAmount - (detail.PaymentAmount);
                                            paymentDetail.PaymentAmount = paymentDetail.PaymentAmount - Ammount;
                                            if (detail.AmmountDue == detail.PaymentAmount)
                                            {
                                                invoice = lstInvAndCNs.FirstOrDefault(a => a.Id == detail.DocumentId);
                                                if (invoice != null)
                                                {
                                                    //Newly added for 0.01 changes
                                                    if (detail.PaymentAmount == invoice.GrandTotal)
                                                    {
                                                        roundingSum = Math.Round(Math.Abs(detail.PaymentAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)invoice.BaseGrandTotal);
                                                        if (roundingSum != 0)
                                                            lstADocValue.Add(invoice.Id, roundingSum);
                                                        paymentDetail.RoundingAmount = roundingSum;
                                                        invoice.RoundingAmount = (roundingSum != null && roundingSum != 0 && (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)) ? invoice.RoundingAmount - roundingSum : 0;
                                                        invoice.BaseBalanceAmount = 0;
                                                    }

                                                    else if (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null)
                                                    {
                                                        invoice.BaseBalanceAmount = 0;
                                                        lstADocValue.Add(invoice.Id, paymentDetail.RoundingAmount.Value);
                                                        paymentDetail.RoundingAmount = paymentDetail.RoundingAmount;
                                                        roundingSum = paymentDetail.RoundingAmount;
                                                    }
                                                    else
                                                    {
                                                        if (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)
                                                            roundingSum = ((invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? (decimal)invoice.RoundingAmount : 0);
                                                        else
                                                            roundingSum = Math.Round(Math.Abs(detail.PaymentAmount - oldPaymentAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)invoice.BaseBalanceAmount);

                                                        invoice.BaseBalanceAmount = 0;
                                                        if (roundingSum != 0)
                                                            lstADocValue.Add(invoice.Id, roundingSum);
                                                        paymentDetail.RoundingAmount = roundingSum;
                                                        invoice.RoundingAmount = (roundingSum != null && roundingSum != 0 && (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)) ? invoice.RoundingAmount - roundingSum : 0;
                                                    }
                                                    invoice.BalanceAmount = 0;
                                                    invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.FullyApplied : InvoiceState.FullyPaid;
                                                    invoice.BaseBalanceAmount = 0;
                                                    invoice.ModifiedBy = PaymentsConstants.System;
                                                    invoice.ModifiedDate = DateTime.UtcNow;
                                                    invoice.ObjectState = ObjectState.Modified;
                                                    _invoiceCompactService.Update(invoice);
                                                    if (invoice.IsWorkFlowInvoice == true)
                                                        FillWFInvoice(invoice, ConnectionString);
                                                    if (invoice.IsOBInvoice == true)
                                                    {
                                                        UpdateOBLineItem(invoice.OpeningBalanceId, invoice.Id, invoice.CompanyId, invoice.DocumentState == InvoiceStates.NotPaid, ConnectionString);
                                                    }
                                                    if (invoice.IsOBInvoice != true)
                                                    {
                                                        AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault() : null;
                                                        if (journal != null)
                                                        {
                                                            journal.BalanceAmount = invoice.BalanceAmount;
                                                            journal.DocumentState = invoice.DocumentState;
                                                            journal.ModifiedBy = PaymentsConstants.System;
                                                            journal.ModifiedDate = DateTime.UtcNow;
                                                            journal.ObjectState = ObjectState.Modified;
                                                            _journalServices.Update(journal);
                                                        }
                                                    }
                                                    #region Documentary History
                                                    if (detail.DocumentType != DocTypeConstants.CreditNote)
                                                    {
                                                        try
                                                        {
                                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);
                                                            //if (lstdocumet.Any())
                                                            //    AppaWorld.Bean.Common.SaveDocumentHistory(lstdocumet, ConnectionString);
                                                            if (lstdocumet.Any())
                                                                lstOfDocumentHistory.AddRange(lstdocumet);

                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                    }
                                                    #endregion Documentary History
                                                }
                                                paymentDetail.PaymentAmount = detail.PaymentAmount;
                                                paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;
                                            }
                                            else if (detail.AmmountDue != detail.PaymentAmount && detail.DocumentType != DocTypeConstants.CreditNote)
                                            {
                                                Ammount = paymentDetail.PaymentAmount - (detail.PaymentAmount);
                                                paymentDetail.PaymentAmount = paymentDetail.PaymentAmount - Ammount;
                                                invoice = lstInvAndCNs != null ? lstInvAndCNs.FirstOrDefault(a => a.Id == detail.DocumentId && a.DocType == DocTypeConstants.Invoice) : null;
                                                if (invoice != null)
                                                {
                                                    invoice.BalanceAmount = (decimal)detail.AmmountDue - detail.PaymentAmount;
                                                    invoice.DocumentState = detail.DocumentType == DocTypeConstants.CreditNote ? CreditNoteState.PartialApplied : InvoiceState.PartialPaid;
                                                    //Newly Added for 0.01 issue
                                                    invoice.BaseBalanceAmount = oldPaymentAmount > detail.PaymentAmount ? invoice.BaseBalanceAmount + (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : invoice.BaseBalanceAmount - (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                    if (docState == InvoiceStates.FullyPaid)
                                                    {
                                                        invoice.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? paymentDetail.RoundingAmount : invoice.RoundingAmount;
                                                        paymentDetail.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != invoice.DocumentState)) ? 0 : paymentDetail.RoundingAmount;
                                                    }

                                                    invoice.ModifiedBy = PaymentsConstants.System;
                                                    invoice.ModifiedDate = DateTime.UtcNow;
                                                    invoice.ObjectState = ObjectState.Modified;
                                                    _invoiceCompactService.Update(invoice);
                                                    if (invoice.IsWorkFlowInvoice == true)

                                                        FillWFInvoice(invoice, ConnectionString);

                                                    if (invoice.IsOBInvoice == true)
                                                    {
                                                        UpdateOBLineItem(invoice.OpeningBalanceId, invoice.Id, invoice.CompanyId, invoice.DocumentState == InvoiceStates.NotPaid, ConnectionString);

                                                    }
                                                    if (invoice.IsOBInvoice != true)
                                                    {
                                                        AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault() : null;
                                                        if (journal != null)
                                                        {
                                                            journal.BalanceAmount = invoice.BalanceAmount;
                                                            journal.DocumentState = invoice.DocumentState;
                                                            journal.ModifiedBy = PaymentsConstants.System;
                                                            journal.ModifiedDate = DateTime.UtcNow;
                                                            journal.ObjectState = ObjectState.Modified;
                                                            _journalServices.Update(journal);
                                                        }
                                                    }
                                                    #region Documentary History
                                                    if (detail.DocumentType != DocTypeConstants.CreditNote)
                                                    {
                                                        try
                                                        {
                                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);

                                                            if (lstdocumet.Any())
                                                                lstOfDocumentHistory.AddRange(lstdocumet);

                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                    }
                                                    #endregion Documentary History
                                                }
                                            }
                                            paymentDetail.PaymentAmount = detail.PaymentAmount;
                                            paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;



                                        }
                                        if (detail.DocumentType == DocTypeConstants.CreditNote)
                                        {
                                            decimal? oldAmount = paymentDetail.PaymentAmount;
                                            invoice = lstInvAndCNs != null ? lstInvAndCNs.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                            paymentDetail.PaymentAmount = detail.PaymentAmount;
                                            CreditNoteApplicationCompact cnApplication = lstCNApps != null ? lstCNApps.FirstOrDefault(a => a.DocumentId == detail.Id) : null;
                                            if (cnApplication != null)
                                            {
                                                if (detail.PaymentAmount > 0)
                                                {
                                                    if (oldAmount != detail.PaymentAmount || oldExchangerate != payment.ExchangeRate || oldSysCalExRate != payment.SystemCalculatedExchangeRate || oldDocDate != TObject.DocDate || ServiceCompanyId != TObject.ServiceCompanyId)
                                                    {
                                                        CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                                        FillCreditNoteAplication(creditNoteModel, invoice, paymentDetail, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                        creditNoteModel.Id = cnApplication.Id;
                                                        creditNoteModel.CreditNoteBalanceAmount = invoice.DocumentState != CreditNoteState.FullyApplied ? invoice.BalanceAmount + cnApplication.CreditAmount : paymentDetail.PaymentAmount;
                                                        creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                        creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                        creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                        creditNoteModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2")));

                                                        CNAplicationREST(creditNoteModel, null, false);
                                                    }
                                                }
                                                else
                                                {
                                                    DocumentResetModel voidModel = new DocumentResetModel();
                                                    voidModel.Id = cnApplication.Id;
                                                    voidModel.InvoiceId = cnApplication.InvoiceId;
                                                    voidModel.ResetDate = DateTime.UtcNow;
                                                    voidModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    voidModel.Version = "0x" + string.Concat(Array.ConvertAll(cnApplication.Version, x => x.ToString("X2")));

                                                    CNAplicationREST(null, voidModel, true); ;
                                                }
                                            }
                                            else
                                            {
                                                if (detail.PaymentAmount > 0)
                                                {
                                                    CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                                    FillCreditNoteAplication(creditNoteModel, invoice, paymentDetail, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                    creditNoteModel.Id = cnApplication != null ? cnApplication.Id : Guid.NewGuid();
                                                    creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                    creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                    creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    CNAplicationREST(creditNoteModel, null, false);
                                                }
                                            }
                                            paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted;

                                        }
                                        #endregion

                                    }
                                    #region DebitNote
                                    else if (detail.DocumentType == DocTypeConstants.DebitNote)
                                    {
                                        DebitNoteCompact debitNote = new DebitNoteCompact();
                                        detail.PaymentAmount = -detail.PaymentAmount;
                                        detail.AmmountDue = -detail.AmmountDue;
                                        debitNote = lstDebitNotes != null ? lstDebitNotes.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                        if (debitNote != null && debitNote.DocumentState == InvoiceState.FullyPaid && oldPaymentAmount == detail.PaymentAmount && oldPaymentAmount != 0 && detail.PaymentAmount != 0)
                                        {
                                            if (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null)
                                            {
                                                lstADocValue.Add(invoice.Id, paymentDetail.RoundingAmount.Value);
                                                roundingSum = paymentDetail.RoundingAmount;
                                            }
                                        }
                                        if (paymentDetail.PaymentAmount > detail.PaymentAmount)
                                        {
                                            decimal Ammount = 0;
                                            Ammount = paymentDetail.PaymentAmount - detail.PaymentAmount;
                                            paymentDetail.PaymentAmount = paymentDetail.PaymentAmount - Ammount;

                                            if (debitNote != null)
                                            {
                                                docState = debitNote.DocumentState;
                                                debitNote.BalanceAmount = debitNote.BalanceAmount + Ammount;
                                                if (debitNote.GrandTotal == debitNote.BalanceAmount)
                                                {
                                                    debitNote.DocumentState = InvoiceState.NotPaid;
                                                    debitNote.BaseBalanceAmount = debitNote.BaseGrandTotal;
                                                    debitNote.RoundingAmount += (paymentDetail.RoundingAmount != null && paymentDetail.RoundingAmount != 0) ? paymentDetail.RoundingAmount : 0;
                                                    paymentDetail.RoundingAmount = 0;
                                                }
                                                else if (debitNote.GrandTotal > debitNote.BalanceAmount)
                                                {
                                                    debitNote.DocumentState = InvoiceState.PartialPaid;
                                                    //Newly Added for 0.01 issue
                                                    debitNote.BaseBalanceAmount = oldPaymentAmount > detail.PaymentAmount ? debitNote.BaseBalanceAmount + (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : debitNote.BaseBalanceAmount - (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                    if (docState == InvoiceStates.FullyPaid && (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null))
                                                    {
                                                        debitNote.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? paymentDetail.RoundingAmount : debitNote.RoundingAmount;
                                                        paymentDetail.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? 0 : paymentDetail.RoundingAmount;
                                                    }
                                                }
                                                debitNote.ModifiedBy = PaymentsConstants.System;
                                                debitNote.ModifiedDate = DateTime.UtcNow;
                                                debitNote.ObjectState = ObjectState.Modified;
                                                _debitNoteCompactService.Update(debitNote);

                                                AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault() : null;
                                                if (journal != null)
                                                {
                                                    journal.BalanceAmount = debitNote.BalanceAmount;
                                                    journal.DocumentState = debitNote.DocumentState;
                                                    journal.ModifiedBy = PaymentsConstants.System;
                                                    journal.ModifiedDate = DateTime.UtcNow;
                                                    journal.ObjectState = ObjectState.Modified;
                                                    _journalServices.Update(journal);
                                                }
                                            }
                                        }
                                        else if (paymentDetail.PaymentAmount < detail.PaymentAmount)
                                        {
                                            if (detail.AmmountDue == detail.PaymentAmount)
                                            {
                                                debitNote = lstDebitNotes != null ? lstDebitNotes.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                                if (debitNote != null)
                                                {

                                                    //Newly added for 0.01 changes
                                                    if (detail.PaymentAmount == debitNote.GrandTotal)
                                                    {
                                                        roundingSum = Math.Round(Math.Abs(detail.PaymentAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)debitNote.BaseGrandTotal);
                                                        if (roundingSum != 0)
                                                            lstADocValue.Add(debitNote.Id, roundingSum);
                                                        paymentDetail.RoundingAmount = roundingSum;
                                                        debitNote.RoundingAmount = (roundingSum != null && roundingSum != 0 && (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)) ? debitNote.RoundingAmount - roundingSum : 0;
                                                        debitNote.BaseBalanceAmount = 0;
                                                    }
                                                    else if (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null)
                                                    {
                                                        debitNote.BaseBalanceAmount = 0;
                                                        lstADocValue.Add(debitNote.Id, paymentDetail.RoundingAmount.Value);
                                                        paymentDetail.RoundingAmount = paymentDetail.RoundingAmount;
                                                        roundingSum = paymentDetail.RoundingAmount;
                                                    }
                                                    else
                                                    {
                                                        if (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)
                                                            roundingSum = ((debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0) ? (decimal)debitNote.RoundingAmount : 0);
                                                        else
                                                            roundingSum = Math.Round(Math.Abs(detail.PaymentAmount - oldPaymentAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - ((decimal)debitNote.BaseBalanceAmount);


                                                        debitNote.BaseBalanceAmount = 0;
                                                        if (roundingSum != 0)
                                                            lstADocValue.Add(debitNote.Id, roundingSum);
                                                        paymentDetail.RoundingAmount = roundingSum;
                                                        debitNote.RoundingAmount = (roundingSum != null && roundingSum != 0 && (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)) ? debitNote.RoundingAmount - roundingSum : 0;
                                                    }
                                                    debitNote.BalanceAmount = 0;
                                                    debitNote.BaseBalanceAmount = 0;
                                                    debitNote.DocumentState = InvoiceState.FullyPaid;
                                                    debitNote.ModifiedBy = PaymentsConstants.System;
                                                    debitNote.ModifiedDate = DateTime.UtcNow;
                                                    debitNote.ObjectState = ObjectState.Modified;
                                                    _debitNoteCompactService.Update(debitNote);

                                                    AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault() : null;
                                                    if (journal != null)
                                                    {
                                                        journal.BalanceAmount = debitNote.BalanceAmount;
                                                        journal.DocumentState = debitNote.DocumentState;
                                                        journal.ModifiedBy = PaymentsConstants.System;
                                                        journal.ModifiedDate = DateTime.UtcNow;
                                                        journal.ObjectState = ObjectState.Modified;
                                                        _journalServices.Update(journal);
                                                    }
                                                }
                                            }
                                            else if (detail.AmmountDue != detail.PaymentAmount)
                                            {
                                                debitNote = lstDebitNotes != null ? lstDebitNotes.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                                if (debitNote != null)
                                                {
                                                    debitNote.BalanceAmount = (decimal)detail.AmmountDue - detail.PaymentAmount;
                                                    debitNote.DocumentState = InvoiceState.PartialPaid;
                                                    //Newly Added for 0.01 issue
                                                    debitNote.BaseBalanceAmount = oldPaymentAmount > detail.PaymentAmount ? debitNote.BaseBalanceAmount + (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero)) : debitNote.BaseBalanceAmount - (Math.Round(Math.Abs(oldPaymentAmount - detail.PaymentAmount) * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero));

                                                    if (docState == InvoiceStates.FullyPaid && (paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null))
                                                    {
                                                        debitNote.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? paymentDetail.RoundingAmount : debitNote.RoundingAmount;
                                                        paymentDetail.RoundingAmount = ((paymentDetail.RoundingAmount != 0 && paymentDetail.RoundingAmount != null) && (docState != debitNote.DocumentState)) ? 0 : paymentDetail.RoundingAmount;
                                                    }

                                                    debitNote.ModifiedBy = PaymentsConstants.System;
                                                    debitNote.ModifiedDate = DateTime.UtcNow;
                                                    debitNote.ObjectState = ObjectState.Modified;
                                                    _debitNoteCompactService.Update(debitNote);

                                                    AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == debitNote.Id).FirstOrDefault() : null;
                                                    if (journal != null)
                                                    {
                                                        journal.BalanceAmount = debitNote.BalanceAmount;
                                                        journal.DocumentState = debitNote.DocumentState;
                                                        journal.ModifiedBy = PaymentsConstants.System;
                                                        journal.ModifiedDate = DateTime.UtcNow;
                                                        journal.ObjectState = ObjectState.Modified;
                                                        _journalServices.Update(journal);
                                                    }

                                                }
                                            }
                                        }
                                        paymentDetail.PaymentAmount = detail.PaymentAmount;
                                        paymentDetail.ObjectState = detail.PaymentAmount != 0 ? ObjectState.Modified : ObjectState.Deleted; ;
                                        #region Documentary History
                                        try
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, debitNote.CompanyId, debitNote.Id, debitNote.DocSubType, debitNote.DocSubType, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);
                                            if (lstdocumet.Any())
                                                lstOfDocumentHistory.AddRange(lstdocumet);

                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        #endregion Documentary History

                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    else
                    {
                        if (detail.PaymentAmount != 0)
                        {
                            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entred_Into_Else_And_check_PaymentDetail);
                            PaymentDetail paymentDetailNew = new PaymentDetail();
                            paymentDetailNew.Id = Guid.NewGuid();
                            paymentDetailNew.PaymentId = payment.Id;
                            paymentDetailNew.AmmountDue = detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.Invoice ? -detail.AmmountDue.Value : detail.AmmountDue.Value;
                            paymentDetailNew.Currency = detail.Currency;
                            paymentDetailNew.DocumentAmmount = detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote ? -detail.DocumentAmmount : detail.DocumentAmmount;
                            paymentDetailNew.DocumentDate = detail.DocumentDate;
                            paymentDetailNew.DocumentNo = detail.DocumentNo;
                            paymentDetailNew.DocumentState = detail.DocumentState;
                            paymentDetailNew.DocumentId = detail.DocumentId;
                            paymentDetailNew.DocumentType = detail.DocumentType;
                            paymentDetailNew.DocumentId = detail.DocumentId;
                            paymentDetailNew.Nature = detail.Nature;
                            paymentDetailNew.SystemReferenceNumber = detail.SystemReferenceNumber;
                            paymentDetailNew.ServiceCompanyId = detail.ServiceCompanyId;

                            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.PayrollBill || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                            {

                                #region Bill
                                if (paymentDetailNew.PaymentAmount > detail.PaymentAmount || paymentDetailNew.PaymentAmount < detail.PaymentAmount)
                                {
                                    Bill bill = new Bill();
                                    UpdatePosting up = new UpdatePosting();
                                    if (paymentDetailNew.PaymentAmount < detail.PaymentAmount)
                                    {
                                        if (detail.AmmountDue == detail.PaymentAmount)
                                        {
                                            bill = lstAllBills != null ? lstAllBills.Where(c => c.Id == detail.DocumentId && c.CompanyId == TObject.CompanyId).FirstOrDefault() : null;
                                            if (bill != null)
                                            {

                                                if (bill.RoundingAmount != null && bill.RoundingAmount != 0)
                                                    roundingSum = ((bill.RoundingAmount != null && bill.RoundingAmount != 0) ? (decimal)bill.RoundingAmount : 0);
                                                else
                                                    roundingSum = Math.Round(detail.PaymentAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)bill.BaseBalanceAmount;
                                                bill.RoundingAmount = (roundingSum != null && roundingSum != 0 && (bill.RoundingAmount != null && bill.RoundingAmount != 0)) ? bill.RoundingAmount - roundingSum : 0;
                                                paymentDetailNew.RoundingAmount = roundingSum;
                                                bill.BaseBalanceAmount = 0;
                                                if (roundingSum != 0)
                                                    lstADocValue.Add(bill.Id, roundingSum);

                                                bill.BalanceAmount = 0;
                                                bill.DocumentState = InvoiceState.FullyPaid;
                                                bill.ObjectState = ObjectState.Modified;
                                                bill.ModifiedDate = DateTime.UtcNow;
                                                bill.ModifiedBy = PaymentsConstants.System;
                                                _billService.Update(bill);
                                                if (bill.DocSubType != DocTypeConstants.OpeningBalance)
                                                {
                                                    FillJournalState(up, bill);
                                                    UpdatePosting(up);
                                                }
                                                else
                                                {
                                                    UpdateOBLineItem(bill.OpeningBalanceId, bill.Id, bill.CompanyId, false, ConnectionString);
                                                }
                                            }
                                        }
                                        else if (detail.AmmountDue != detail.PaymentAmount)
                                        {
                                            bill = lstAllBills != null ? lstAllBills.FirstOrDefault(c => c.Id == detail.DocumentId && c.CompanyId == TObject.CompanyId) : null;
                                            if (bill != null)
                                            {
                                                bill.BalanceAmount = detail.AmmountDue - detail.PaymentAmount;
                                                bill.DocumentState = InvoiceState.PartialPaid;
                                                //Newly Added for 0.01 issue
                                                bill.BaseBalanceAmount -= Math.Round(detail.PaymentAmount * ((bill.ExchangeRate != null ? (decimal)bill.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);

                                                bill.ObjectState = ObjectState.Modified;
                                                bill.ModifiedDate = DateTime.UtcNow;
                                                bill.ModifiedBy = PaymentsConstants.System;
                                                _billService.Update(bill);
                                                if (bill.DocSubType != DocTypeConstants.OpeningBalance)
                                                {
                                                    FillJournalState(up, bill);
                                                    UpdatePosting(up);
                                                }
                                                else
                                                {
                                                    UpdateOBLineItem(bill.OpeningBalanceId, bill.Id, bill.CompanyId, false, ConnectionString);

                                                }
                                            }
                                        }
                                    }
                                    #region Documentary History
                                    try
                                    {

                                        if (bill != null)
                                        {
                                            List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, bill.CompanyId, bill.Id, bill.DocType, bill.DocSubType, bill.DocumentState, bill.DocCurrency, bill.GrandTotal, bill.BalanceAmount.Value, bill.ExchangeRate.Value, bill.ModifiedBy != null ? bill.ModifiedBy : bill.UserCreated, bill.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);


                                            if (lstdocumet.Any())
                                                lstOfDocumentHistory.AddRange(lstdocumet);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    #endregion Documentary History
                                }
                                paymentDetailNew.PaymentAmount = detail.PaymentAmount;
                                if (detail.PaymentAmount != 0)
                                {
                                    paymentDetailNew.ObjectState = ObjectState.Added;
                                    _paymentDetailService.Insert(paymentDetailNew);
                                }
                                #endregion

                            }
                            else
                            {
                                if (payment.DocSubType != DocTypeConstants.Payroll)
                                {
                                    #region CreditMemo
                                    if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                                    {
                                        paymentDetailNew.PaymentAmount = -detail.PaymentAmount;
                                        detail.PaymentAmount = -detail.PaymentAmount;
                                        detail.AmmountDue = -detail.AmmountDue;
                                        paymentDetailNew.AmmountDue = detail.AmmountDue.Value;

                                        CreditMemoCompact creditMemo = lstCMs.FirstOrDefault(a => a.Id == detail.DocumentId);
                                        CreditMemoApplicationModel creditMemoAppModel = new CreditMemoApplicationModel();
                                        if (creditMemo != null)
                                        {
                                            CreditMemoApplicationCompact cmApplication = lstCreditMemoApps != null ? lstCreditMemoApps.FirstOrDefault(a => a.CreditMemoId == creditMemo.Id && a.DocumentId == detail.Id) : null;
                                            if (cmApplication != null)
                                            {
                                                if (detail.PaymentAmount > 0)
                                                {
                                                    FillCreditMemoAplication(creditMemoAppModel, creditMemo, paymentDetailNew, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                    creditMemoAppModel.Id = cmApplication.Id;
                                                    creditMemoAppModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                    creditMemoAppModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                    creditMemoAppModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                    CMAplicationREST(creditMemoAppModel, null, false);
                                                }
                                            }
                                            else
                                            {
                                                //create a new Credit Memo Application
                                                FillCreditMemoAplication(creditMemoAppModel, creditMemo, paymentDetailNew, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                creditMemoAppModel.Id = Guid.NewGuid();
                                                creditMemoAppModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                creditMemoAppModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                creditMemoAppModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                CMAplicationREST(creditMemoAppModel, null, false);
                                            }
                                        }
                                        if (detail.PaymentAmount != 0)
                                        {
                                            paymentDetailNew.ObjectState = ObjectState.Added;
                                            _paymentDetailService.Insert(paymentDetailNew);
                                        }

                                    }
                                    #endregion
                                    if (TObject.IsCustomer == true)
                                    {
                                        #region Invoice_and_CN

                                        if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
                                        {
                                            decimal Ammount = 0;
                                            Ammount = detail.DocumentType == DocTypeConstants.Invoice ? -detail.PaymentAmount : detail.PaymentAmount;
                                            paymentDetailNew.PaymentAmount = Ammount;
                                            detail.PaymentAmount = Ammount;
                                            detail.AmmountDue = detail.DocumentType == DocTypeConstants.Invoice ? -detail.AmmountDue : detail.AmmountDue;
                                            if (detail.DocumentType != DocTypeConstants.CreditNote)
                                            {
                                                InvoiceCompact invoice = lstInvAndCNs != null ? lstInvAndCNs.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                                if (invoice != null)
                                                {
                                                    if (detail.AmmountDue == detail.PaymentAmount)
                                                    {
                                                        //Newly added for 0.01 changes
                                                        if (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)
                                                            roundingSum = ((invoice.RoundingAmount != null && invoice.RoundingAmount != 0) ? (decimal)invoice.RoundingAmount : 0);
                                                        else
                                                            roundingSum = Math.Round(detail.PaymentAmount * (invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1), 2, MidpointRounding.AwayFromZero) - (decimal)invoice.BaseBalanceAmount;
                                                        invoice.RoundingAmount = (roundingSum != null && roundingSum != 0 && (invoice.RoundingAmount != null && invoice.RoundingAmount != 0)) ? invoice.RoundingAmount - roundingSum : 0;
                                                        paymentDetailNew.RoundingAmount = roundingSum;
                                                        invoice.BaseBalanceAmount = 0;
                                                        if (roundingSum != 0)
                                                            lstADocValue.Add(invoice.Id, roundingSum);


                                                        invoice.BalanceAmount = 0;
                                                        invoice.DocumentState = InvoiceStates.FullyPaid;
                                                        invoice.ModifiedBy = PaymentsConstants.System;
                                                        invoice.ModifiedDate = DateTime.UtcNow;

                                                        invoice.ObjectState = ObjectState.Modified;
                                                        _invoiceCompactService.Update(invoice);
                                                        if (invoice.IsWorkFlowInvoice == true)
                                                            FillWFInvoice(invoice, ConnectionString);
                                                        if (invoice.IsOBInvoice == true)
                                                        {
                                                            UpdateOBLineItem(invoice.OpeningBalanceId, invoice.Id, invoice.CompanyId, invoice.DocumentState == InvoiceStates.NotPaid, ConnectionString);
                                                        }
                                                        if (invoice.IsOBInvoice != true)
                                                        {
                                                            AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault() : null;
                                                            if (journal != null)
                                                            {
                                                                journal.BalanceAmount = invoice.BalanceAmount;
                                                                journal.DocumentState = invoice.DocumentState;
                                                                journal.ModifiedBy = PaymentsConstants.System;
                                                                journal.ModifiedDate = DateTime.UtcNow;
                                                                journal.ObjectState = ObjectState.Modified;
                                                                _journalServices.Update(journal);
                                                            }
                                                        }
                                                    }
                                                    else if (detail.AmmountDue != detail.PaymentAmount)
                                                    {
                                                        invoice.BalanceAmount = (decimal)detail.AmmountDue - (detail.PaymentAmount);
                                                        invoice.DocumentState = invoice.BalanceAmount == invoice.GrandTotal ? InvoiceStates.NotPaid : InvoiceState.PartialPaid;
                                                        //Newly Added for 0.01 issue
                                                        if (invoice.DocumentState == InvoiceState.PartialPaid)
                                                            invoice.BaseBalanceAmount -= Math.Round(detail.PaymentAmount * ((invoice.ExchangeRate != null ? (decimal)invoice.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
                                                        invoice.ModifiedBy = PaymentsConstants.System;
                                                        invoice.ModifiedDate = DateTime.UtcNow;
                                                        invoice.ObjectState = ObjectState.Modified;
                                                        _invoiceCompactService.Update(invoice);
                                                        if (invoice.IsWorkFlowInvoice == true)
                                                            FillWFInvoice(invoice, ConnectionString);

                                                        if (invoice.IsOBInvoice == true)
                                                        {
                                                            UpdateOBLineItem(invoice.OpeningBalanceId, invoice.Id, invoice.CompanyId, invoice.DocumentState == InvoiceStates.NotPaid ? true : false, ConnectionString);

                                                        }
                                                        if (invoice.IsOBInvoice != true)
                                                        {
                                                            AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.Where(a => a.DocumentId == invoice.Id).FirstOrDefault() : null;
                                                            if (journal != null)
                                                            {
                                                                journal.BalanceAmount = invoice.BalanceAmount;
                                                                journal.DocumentState = invoice.DocumentState;
                                                                journal.ModifiedBy = PaymentsConstants.System;
                                                                journal.ModifiedDate = DateTime.UtcNow;
                                                                journal.ObjectState = ObjectState.Modified;
                                                                _journalServices.Update(journal);
                                                            }
                                                        }
                                                    }
                                                }
                                                #region Documentary History
                                                try
                                                {
                                                    List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, invoice.CompanyId, invoice.Id, invoice.DocType, invoice.DocSubType, invoice.DocumentState, invoice.DocCurrency, invoice.GrandTotal, invoice.BalanceAmount, invoice.ExchangeRate.Value, invoice.ModifiedBy != null ? invoice.ModifiedBy : invoice.UserCreated, invoice.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);

                                                    if (lstdocumet.Any())
                                                        lstOfDocumentHistory.AddRange(lstdocumet);
                                                }
                                                catch (Exception ex)
                                                {

                                                }
                                                #endregion Documentary History
                                            }
                                            if (detail.DocumentType == DocTypeConstants.CreditNote)
                                            {
                                                InvoiceCompact invoice = lstInvAndCNs != null ? lstInvAndCNs.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                                CreditNoteApplicationCompact cnApplication = lstCNApps != null ? lstCNApps.FirstOrDefault(a => a.DocumentId == detail.Id) : null;
                                                paymentDetailNew.PaymentAmount = detail.PaymentAmount;
                                                paymentDetailNew.AmmountDue = detail.AmmountDue.Value;
                                                if (cnApplication != null)
                                                {
                                                    if (detail.PaymentAmount > 0)
                                                    {
                                                        CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                                        FillCreditNoteAplication(creditNoteModel, invoice, paymentDetailNew, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                        creditNoteModel.Id = cnApplication.Id;
                                                        creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                        creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                        creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                        CNAplicationREST(creditNoteModel, null, false);
                                                    }
                                                }
                                                else
                                                {
                                                    if (detail.PaymentAmount > 0)
                                                    {
                                                        CreditNoteApplicationModel creditNoteModel = new CreditNoteApplicationModel();
                                                        FillCreditNoteAplication(creditNoteModel, invoice, paymentDetailNew, payment, serEntCount, icCOAId, clearingPaymentsCoaId, serEntCount > 1);
                                                        creditNoteModel.Id = Guid.NewGuid();
                                                        creditNoteModel.FinancialPeriodLockEndDate = TObject.FinancialPeriodLockEndDate;
                                                        creditNoteModel.FinancialPeriodLockStartDate = TObject.FinancialPeriodLockStartDate;
                                                        creditNoteModel.PeriodLockPassword = TObject.PeriodLockPassword;
                                                        CNAplicationREST(creditNoteModel, null, false);
                                                    }
                                                }
                                            }
                                            if (detail.PaymentAmount != 0)
                                            {
                                                paymentDetailNew.ObjectState = ObjectState.Added;
                                                _paymentDetailService.Insert(paymentDetailNew);
                                            }
                                        }
                                        #endregion
                                        #region Debit_note
                                        else if (detail.DocumentType == DocTypeConstants.DebitNote && paymentDetailNew.PaymentAmount > detail.PaymentAmount || paymentDetailNew.PaymentAmount < detail.PaymentAmount)
                                        {
                                            decimal? amount = 0;
                                            amount = -detail.PaymentAmount;
                                            paymentDetailNew.PaymentAmount = -detail.PaymentAmount;
                                            detail.PaymentAmount = -detail.PaymentAmount;
                                            detail.AmmountDue = -detail.AmmountDue;
                                            paymentDetailNew.AmmountDue = detail.AmmountDue.Value;
                                            paymentDetailNew.DocumentAmmount = -detail.DocumentAmmount;

                                            DebitNoteCompact debitNote = lstDebitNotes != null ? lstDebitNotes.FirstOrDefault(a => a.Id == detail.DocumentId) : null;
                                            if (debitNote != null)
                                            {
                                                if (detail.AmmountDue == detail.PaymentAmount)
                                                {
                                                    //Newly added for 0.01 changes
                                                    if (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)
                                                        roundingSum = ((debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0) ? (decimal)debitNote.RoundingAmount : 0);
                                                    else
                                                        roundingSum = Math.Round(detail.PaymentAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero) - (decimal)debitNote.BaseBalanceAmount;


                                                    debitNote.RoundingAmount = (roundingSum != null && roundingSum != 0 && (debitNote.RoundingAmount != null && debitNote.RoundingAmount != 0)) ? debitNote.RoundingAmount - roundingSum : 0;
                                                    paymentDetailNew.RoundingAmount = roundingSum;
                                                    debitNote.BaseBalanceAmount = 0;
                                                    if (roundingSum != 0)
                                                        lstADocValue.Add(debitNote.Id, roundingSum);

                                                    debitNote.BalanceAmount = 0;
                                                    debitNote.DocumentState = InvoiceStates.FullyPaid;
                                                    debitNote.ModifiedBy = PaymentsConstants.System;
                                                    debitNote.ModifiedDate = DateTime.UtcNow;

                                                    debitNote.ObjectState = ObjectState.Modified;
                                                    _debitNoteCompactService.Update(debitNote);

                                                    AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.FirstOrDefault(a => a.DocumentId == debitNote.Id) : null;
                                                    if (journal != null)
                                                    {
                                                        journal.BalanceAmount = debitNote.BalanceAmount;
                                                        journal.DocumentState = debitNote.DocumentState;
                                                        journal.ModifiedBy = PaymentsConstants.System;
                                                        journal.ModifiedDate = DateTime.UtcNow;
                                                        journal.ObjectState = ObjectState.Modified;
                                                        _journalServices.Update(journal);
                                                    }
                                                }
                                                else if (detail.AmmountDue != detail.PaymentAmount)
                                                {
                                                    debitNote.BalanceAmount = (decimal)detail.AmmountDue - (detail.PaymentAmount);
                                                    debitNote.DocumentState = debitNote.BalanceAmount == debitNote.GrandTotal ? InvoiceStates.NotPaid : InvoiceState.PartialPaid;
                                                    //Newly Added for 0.01 issue
                                                    if (debitNote.DocumentState == InvoiceState.PartialPaid)
                                                        debitNote.BaseBalanceAmount -= Math.Round(detail.PaymentAmount * ((debitNote.ExchangeRate != null ? (decimal)debitNote.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);

                                                    debitNote.ModifiedBy = PaymentsConstants.System;
                                                    debitNote.ModifiedDate = DateTime.UtcNow;
                                                    debitNote.ObjectState = ObjectState.Modified;
                                                    _debitNoteCompactService.Update(debitNote);

                                                    AppsWorld.PaymentModule.Entities.Journal journal = lstJournals != null ? lstJournals.FirstOrDefault(a => a.DocumentId == debitNote.Id) : null;
                                                    if (journal != null)
                                                    {
                                                        journal.BalanceAmount = debitNote.BalanceAmount;
                                                        journal.DocumentState = debitNote.DocumentState;
                                                        journal.ModifiedBy = PaymentsConstants.System;
                                                        journal.ModifiedDate = DateTime.UtcNow;
                                                        journal.ObjectState = ObjectState.Modified;
                                                        _journalServices.Update(journal);
                                                    }

                                                }
                                            }

                                            if (detail.PaymentAmount != 0)
                                            {
                                                paymentDetailNew.ObjectState = ObjectState.Added;
                                                _paymentDetailService.Insert(paymentDetailNew);
                                            }
                                            #region Documentary History
                                            try
                                            {
                                                List<DocumentHistoryModel> lstdocumet = AppaWorld.Bean.Common.FillDocumentHistory(isEdit ? TObject.Id : payment.Id, debitNote.CompanyId, debitNote.Id, debitNote.DocSubType, debitNote.DocSubType, debitNote.DocumentState, debitNote.DocCurrency, debitNote.GrandTotal, debitNote.BalanceAmount, debitNote.ExchangeRate.Value, debitNote.ModifiedBy != null ? debitNote.ModifiedBy : debitNote.UserCreated, debitNote.Remarks, TObject.DocDate, -detail.PaymentAmount, roundingSum);
                                                if (lstdocumet.Any())
                                                    lstOfDocumentHistory.AddRange(lstdocumet);
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                            #endregion Documentary History

                                        }
                                        #endregion


                                    }
                                }
                            }
                        }
                    }
                }
                roundingSum = 0;
            }
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Come_Out_From_UpdatePaymentDetails_Of_Payment);
        }

        public void FillWokflowInvoice(InvoiceCompact invoice)
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
            invoicevm.ModifiedDate = DateTime.UtcNow;
            invoicevm.Status = RecordStatusEnum.Active;
            WorkflowInvoicePosting(invoicevm);
        }
        public void FillWFInvoice(InvoiceCompact invoice, string ConnectionString)
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
                    cmd.CommandTimeout = 30;
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
                //        if (section.Ziraff[i].Name == "IdentityWorkflow")
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
                var message = ex.Message;
            }
        }


        //private void FillCreditMemoAplication(CreditMemoApplicationModel model, CreditMemoCompact creditMemo, PaymentDetail paymentDetail, Payment payment, int? serEntCount, long? icCOA, long? clearingReceiptCOA, bool isICActive)
        //{
        //    model.Id = Guid.NewGuid();
        //    model.CompanyId = creditMemo.CompanyId;
        //    model.CreditMemoId = creditMemo.Id;
        //    model.DocNo = creditMemo.DocNo;
        //    model.DocDate = creditMemo.DocDate;
        //    model.CreditMemoApplicationNumber = creditMemo.DocNo;
        //    model.DocCurrency = creditMemo.DocCurrency;
        //    model.UserCreated = PaymentsConstants.System;
        //    model.CreatedDate = DateTime.UtcNow;
        //    model.CreditMemoApplicationDate = payment.DocDate;
        //    model.IsNoSupportingDocument = payment.IsNoSupportingDocument;
        //    model.NoSupportingDocument = payment.NoSupportingDocs;
        //    model.CreditAmount = paymentDetail.PaymentAmount;
        //    model.DocumentId = paymentDetail.Id;
        //    model.CreditMemoAmount = creditMemo.GrandTotal;
        //    model.CreditMemoBalanceAmount = creditMemo.DocumentState != CreditMemoState.Fully_Applied ? creditMemo.BalanceAmount : paymentDetail.PaymentAmount;
        //    model.IsGstSettings = payment.IsGstSettings;
        //    model.Status = CreditMemoApplicationStatus.Posted;
        //    model.DocSubType = DocTypeConstants.CMApplication;
        //    model.ExchangeRate = creditMemo.ExchangeRate;
        //    model.GSTExchangeRate = creditMemo.GSTExchangeRate;
        //    model.IsOffset = true;
        //    model.Remarks = "CM Application - " + payment.DocNo;
        //    model.CreditMemoApplicationDetailModels.Add(new CreditMemoApplicationDetailModel()
        //    {
        //        Id = Guid.NewGuid(),
        //        CreditMemoApplicationId = model.Id,
        //        DocType = DocTypeConstants.Receipt,
        //        DocumentId = paymentDetail.DocumentId,
        //        DocCurrency = creditMemo.DocCurrency,
        //        CreditAmount = paymentDetail.PaymentAmount,
        //        DocNo = payment.DocNo,
        //        DocDate = payment.DocDate,
        //        Nature = creditMemo.Nature,
        //        ServiceEntityId = payment.ServiceCompanyId,
        //        DocState = "Posted",
        //        COAId = serEntCount == 1 && isICActive == false ? clearingReceiptCOA : (serEntCount > 1 || isICActive) ? icCOA : clearingReceiptCOA,
        //        BaseCurrencyExchangeRate = payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate
        //    });

        //}
        //private void FillCreditNoteAplication(CreditNoteApplicationModel model, InvoiceCompact creditNote, PaymentDetail paymentDetail, Payment payment, int? serEntityCount, long? icCOA, long? clearingReceiptCOA, bool isICActive)
        //{
        //    model.Id = Guid.NewGuid();
        //    model.CompanyId = creditNote.CompanyId;
        //    model.InvoiceId = creditNote.Id;
        //    model.DocNo = creditNote.DocNo;
        //    model.DocDate = creditNote.DocDate;
        //    model.CreditNoteApplicationNumber = creditNote.DocNo;
        //    model.DocCurrency = creditNote.DocCurrency;
        //    model.UserCreated = PaymentsConstants.System;
        //    model.CreatedDate = DateTime.UtcNow;
        //    model.CreditNoteApplicationDate = payment.DocDate;
        //    model.IsNoSupportingDocument = creditNote.IsNoSupportingDocument;
        //    model.NoSupportingDocument = creditNote.NoSupportingDocs;
        //    model.CreditAmount = paymentDetail.PaymentAmount;
        //    model.DocumentId = paymentDetail.Id;
        //    model.CreditNoteAmount = creditNote.GrandTotal;
        //    model.CreditNoteBalanceAmount = creditNote.DocumentState != CreditNoteState.FullyApplied ? creditNote.BalanceAmount : paymentDetail.PaymentAmount;
        //    model.IsGstSettings = creditNote.IsGstSettings;
        //    model.Status = CreditNoteApplicationStatus.Posted;
        //    model.DocSubType = DocTypeConstants.Application;
        //    model.ExchangeRate = creditNote.ExchangeRate;
        //    model.GSTExchangeRate = creditNote.GSTExchangeRate;
        //    model.IsOffset = true;
        //    model.Remarks = "CN Application - " + payment.DocNo;
        //    model.CreditNoteApplicationDetailModels.Add(new CreditNoteApplicationDetailModel()
        //    {
        //        Id = Guid.NewGuid(),
        //        CreditNoteApplicationId = model.Id,
        //        DocType = DocTypeConstants.Receipt,
        //        DocumentId = paymentDetail.DocumentId,
        //        DocCurrency = creditNote.DocCurrency,
        //        CreditAmount = paymentDetail.PaymentAmount,
        //        DocNo = payment.DocNo,
        //        DocDate = payment.DocDate,
        //        Nature = creditNote.Nature,
        //        ServiceEntityId = creditNote.ServiceCompanyId,
        //        DocState = "Posted",
        //        COAId = serEntityCount == 1 && isICActive == false ? clearingReceiptCOA : (serEntityCount > 1 || isICActive) ? icCOA : clearingReceiptCOA,
        //        BaseCurrencyExchangeRate = payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate
        //    });

        //}
        //private static void CNAplicationREST(CreditNoteApplicationModel creditNoteModel, DocumentResetModel voidModel, bool? isVoid)
        //{
        //    //var json = RestSharpHelper.ConvertObjectToJason(creditNoteModel);
        //    //var json1 = RestSharpHelper.ConvertObjectToJason(voidModel);
        //    string json = null;
        //    if (isVoid != true)
        //        json = RestSharpHelper.ConvertObjectToJason(creditNoteModel);
        //    else
        //        json = RestSharpHelper.ConvertObjectToJason(voidModel);
        //    try
        //    {
        //        string url = ConfigurationManager.AppSettings.Get("BeanUrl");
        //        //object obj = creditNoteModel;
        //        IRestResponse response = null;
        //        if (isVoid != true)
        //            response = RestSharpHelper.Post(url, "api/v2/invoice/savecreditnoteapplication", json);
        //        else
        //            response = RestSharpHelper.Post(url, "api/v2/invoice/savecreditnoteapplicationvoid", json);
        //        if (response.ErrorMessage != null)
        //        {
        //            //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //    }
        //}
        //private static void CMAplicationREST(CreditMemoApplicationModel creditMemoModel, DocumentResetModel voidModel, bool? isVoid)
        //{
        //    string json = null;
        //    if (isVoid != true)
        //    {
        //        json = RestSharpHelper.ConvertObjectToJason(creditMemoModel);
        //    }
        //    else
        //    {
        //        json = RestSharpHelper.ConvertObjectToJason(voidModel);
        //    }
        //    try
        //    {
        //        //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
        //        string url = ConfigurationManager.AppSettings.Get("BeanUrl");
        //        IRestResponse response = null;
        //        //object obj = creditMemoModel;
        //        if (isVoid == true)
        //        {
        //            response = RestSharpHelper.Post(url, "/api/creditmemo/savecreditmemovoid", json);
        //        }
        //        else
        //        {
        //            response = RestSharpHelper.Post(url, "/api/creditmemo/savecreditmemoapplication", json);
        //        }
        //        if (response != null)
        //        {
        //            if (response.ErrorMessage != null)
        //            {
        //                //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message;
        //    }
        //}


        #endregion

        #region Fill Methods
        private void FillPaymentModel(PaymentModel pModel, Payment _payment)
        {
            pModel.Id = _payment.Id;
            pModel.CompanyId = _payment.CompanyId;
            //pModel.BankCharges = _payment.BankCharges;
            pModel.IsModify = _payment.ClearCount > 0;
            pModel.BankChargesCurrency = _payment.BankChargesCurrency;
            pModel.BankClearingDate = _payment.BankClearingDate;
            pModel.BankPaymentAmmount = _payment.BankPaymentAmmount;
            pModel.BankPaymentAmmountCurrency = _payment.BankPaymentAmmountCurrency;
            pModel.BaseCurrency = _payment.BaseCurrency;
            //pModel.IsGSTApplied = _payment.IsGSTApplied;
            pModel.COAId = _payment.COAId;
            pModel.DocType = _payment.DocType;
            pModel.DocSubType = _payment.DocSubType;
            pModel.Version = "0x" + string.Concat(Array.ConvertAll(_payment.Version, x => x.ToString("X2")));
            //var BankRecociliation = _bankReconciliationService.GetByCompanyId(_payment.CompanyId);
            //if (BankRecociliation != null)
            //{
            //    //pModel.BankClearingDate = BankRecociliation.BankClearingDate;
            //    pModel.IsBankReconciliation = true;
            //}
            //var coa = _chartOfAccountService.GetChartOfAccountById(_payment.COAId);
            //if (coa != null)
            //    pModel.COAName = coa.Name;
            pModel.CurrencyCode = _payment.DocCurrency;
            //var currency = _currencyService.GetCurrencyByCode(_payment.CompanyId, pModel.CurrencyCode);
            //if (currency != null)
            //    pModel.CurrencyName = currency.Name;
            pModel.DocCurrency = _payment.DocCurrency;
            pModel.DocDate = _payment.DocDate;
            pModel.DocNo = _payment.DocNo;
            pModel.DueDate = _payment.DueDate;
            pModel.EntityId = _payment.EntityId;
            //BeanEntity beanEntity = _beanEntityService.GetEntityById(_payment.EntityId);
            pModel.EntityName = _beanEntityService.GetEntityName(_payment.EntityId);
            //pModel.ExcessPaidByClient = _payment.ExcessPaidByClient;
            //pModel.ExcessPaidByClientAmmount = _payment.ExcessPaidByClientAmmount;
            pModel.ExDurationFrom = _payment.ExDurationFrom;
            pModel.ExDurationTo = _payment.ExDurationTo;
            pModel.GrandTotal = _payment.GrandTotal;
            pModel.GstdurationFrom = _payment.GSTExDurationFrom;
            pModel.GstDurationTo = _payment.GSTExDurationTo;
            pModel.GstReportingCurrency = _payment.GSTExCurrency;
            //pModel.GSTTotalAmount = _payment.GSTTotalAmount;
            //pModel.EntityType = _payment.EntityType;
            //AppsWorld.CommonModule.Entities.Journal journal = _journalService.GetJournal(_payment.CompanyId, _payment.Id);
            //if (journal != null)
            //{
            //    pModel.JournalId = journal.Id;
            //    //pModel.JournalRefNo = journal.SystemReferenceNo;
            //}
            //pModel.IsAllowDisAllow = _payment.IsAllowableDisallowable;
            //pModel.IsDisAllow = _payment.IsDisAllow;
            pModel.ISMultiCurrency = _payment.IsMultiCurrency;
            pModel.IsNoSupportingDocument = _payment.IsNoSupportingDocument;
            pModel.ModeOfPayment = _payment.ModeOfPayment;
            //var control = _controlCodeService.GetControlCodeCode(pModel.code);
            //if (control != null)
            //    pModel.Name = control.CodeValue;
            pModel.NoSupportingDocument = _payment.NoSupportingDocs;
            pModel.PaymentRefNo = _payment.PaymentRefNo;
            pModel.ServiceCompanyId = _payment.ServiceCompanyId;
            //var company = _companyService.GetByNameByServiceCompany(_payment.ServiceCompanyId);
            //if (company != null)
            //    pModel.ServiceCompanyMOdels.ServiceCompanyName = company.ShortName;
            //pModel.ServiceCompanyName = _companyService.GetByNameByServiceCompany(_payment.ServiceCompanyId);
            pModel.SystemRefNo = _payment.SystemRefNo;
            pModel.Remarks = _payment.Remarks;
            pModel.UserCreated = _payment.UserCreated;
            pModel.CreatedDate = _payment.CreatedDate;
            pModel.ModifiedBy = _payment.ModifiedBy;
            pModel.ModifiedDate = _payment.ModifiedDate;
            pModel.ExtensionType = ExtensionType.General;
            //pModel.ExcessPaidByClientCurrency = _payment.ExcessPaidByClientCurrency;
            pModel.PeriodLockPassword = _payment.PeriodLockPassword;
            //pModel.BalancingItemReciveCRAmount = _payment.BalancingItemPaymentCRAmount;
            //pModel.BalancingItemReciveCRCurrency = _payment.BalancingItemPaymentCRCurrency;
            //pModel.BalancingItemPayDRAmount = _payment.BalancingItemPayDRAmount;
            //pModel.BalancingItemPayDRCurrency = _payment.BalancingItemPayDRCurrency;
            //pModel.ExcessPaidByClient = _payment.ExcessPaidByClient;
            pModel.ExCurrency = _payment.ExCurrency;
            pModel.ExchangeRate = _payment.ExchangeRate;
            //pModel.ExchangeRate1 = string.Format("{0:0.0000000000}", _payment.ExchangeRate);
            pModel.PaymentApplicationCurrency = _payment.PaymentApplicationCurrency;
            pModel.PaymentApplicationAmmount = _payment.PaymentApplicationAmmount;
            pModel.SystemCalculatedExchangeRate = _payment.SystemCalculatedExchangeRate;
            var deci = Math.Round(Convert.ToDecimal(_payment.VarianceExchangeRate), 2);
            pModel.VarianceExchangeRate = deci + "%";
            pModel.DocumentState = _payment.DocumentState;
            pModel.IsExchangeRateLabel = _payment.IsExchangeRateLabel;
            pModel.IsBaseCurrencyRateChanged = _payment.IsBaseCurrencyRateChanged;
            pModel.IsCustomer = _payment.IsCustomer;
        }
        private void FillNewPaymentModel(PaymentModel pModel, FinancialSetting financialSetting, string docType/*, AppsWorld.PaymentModule.Entities.AutoNumber _autoNo*/)
        {
            long companyId = financialSetting.CompanyId;
            Payment lastPayment = _paymentService.CreatePayment(companyId, docType);
            pModel.Id = Guid.NewGuid();
            pModel.CompanyId = companyId;
            pModel.DocumentState = PaymentState.Posted;
            pModel.DocDate = lastPayment == null ? DateTime.Now : lastPayment.DocDate;
            pModel.DocType = DocTypeConstants.BillPayment;
            pModel.DocSubType = DocTypeConstants.General;
            pModel.ExtensionType = ExtensionType.General;
            //pModel.DocNo = GetNewReceiptDocumentNumber(companyId, docType);
            //bool? isEdit = false;
            //pModel.DocNo = GetAutoNumberByEntityType(pModel.CompanyId, lastPayment, docType, _autoNo, ref isEdit);
            //pModel.IsDocNoEditable = _autoNumberService.GetAutoNumberIsEditable(companyId,/* DocTypeConstants.CashSale*/docType == "Payroll" ? "Payroll Payment" : "Payment");

            //var BankRecociliation = _bankReconciliationService.GetByCompanyId(companyId);
            //if (BankRecociliation != null)
            //{
            //    pModel.BankClearingDate = BankRecociliation.BankClearingDate;
            //    pModel.IsBankReconciliation = true;
            //}
            pModel.DueDate = DateTime.UtcNow;
            //pModel.IsAllowDisAllow = _companySettingService.GetModuleStatus(ModuleNameConstants.AllowableNonAllowable,
            //    companyId);

            //pModel.IsNoSupportingDocument = _companySettingService.GetModuleStatus(ModuleNameConstants.NoSupportingDocuments, companyId);
            pModel.CreatedDate = DateTime.UtcNow;
            //pModel.ISGstDeRegistered = _gstSettingService.IsGSTDeregistered(companyId);
            //pModel.IsGstSettings = _gstSettingService.IsGSTSettingActivated(companyId);
            pModel.BaseCurrency = financialSetting.BaseCurrency;
            pModel.DocumentState = InvoiceState.NotPaid;
            //MultiCurrencySetting multi = _multiCurrencySettingService.GetByCompanyId(companyId);
            //pModel.ISMultiCurrency = multi != null;
            pModel.IsBaseCurrencyRateChanged = false;
            pModel.IsGSTCurrencyRateChanged = false;
            pModel.NoSupportingDocument = false;
        }

        private string GetAutoNumberByEntityType(long companyId, Payment lastInvoice, string entityType, AppsWorld.PaymentModule.Entities.AutoNumber _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;
            //AppsWorld.PaymentModule.Entities.AutoNumber _autoNo = _autoNumberService.GetAutoNumber(companyId, entityType);
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewReceiptDocumentNumber(companyId, entityType);
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
                        else if (_autoNo.Format.Contains("{YYYY}"))
                        {
                            if (DateTime.UtcNow.Year == lastInvoice.CreatedDate.Value.Year)
                            {
                                string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                            else
                            {
                                string output = "1";
                                //string output = Convert.ToString(_autoNo.StartNumber);
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

        private string GetNewReceiptDocumentNumber(long companyId, string docType)
        {
            Payment payment = _paymentService.CreatePaymentDocNo(companyId, docType);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (payment != null)
            {
                string strOldNo = String.Empty;
                Payment duplicatPayment;
                int index;
                strOldDocNo = payment.DocNo;

                for (int i = strOldDocNo.Length - 1; i >= 0; i--)
                {
                    if (Char.IsDigit(strOldDocNo[i]))
                        strOldNo = strOldDocNo[i] + strOldNo;
                    else
                        break;
                }
                long docNo = 0;
                try
                {
                    docNo = long.Parse(strOldNo);
                }
                catch
                {
                }
                index = strOldDocNo.LastIndexOf(strOldNo);
                do
                {
                    docNo++;
                    strNewNo = docNo.ToString().PadLeft(strOldNo.Length, '0');
                    strNewDocNo = (docNo == 1) ? strOldDocNo + strNewNo : strOldDocNo.Substring(0, index) + strNewNo;

                    duplicatPayment = _paymentService.GetDocNo(strNewDocNo, companyId);
                } while (duplicatPayment != null);
            }
            return strNewDocNo;
        }
        private void FillPaymentDetailModel(PaymentDetailModel PDetailModel, Bill bill, PaymentDetail PDetail, bool isView, bool isCal)
        {
            PDetailModel.DocumentDate = bill.DocumentDate;
            PDetailModel.DocumentNo = bill.DocNo;
            PDetailModel.DocumentState = bill.DocumentState;
            PDetailModel.DocumentAmmount = bill.GrandTotal;
            PDetailModel.Nature = bill.Nature;
            PDetailModel.DocumentId = bill.Id;
            PDetailModel.PaymentAmount = PDetail.PaymentAmount;
            PDetailModel.AmmountDue = bill.BalanceAmount.Value;
            if (isView == true)
            {
                if (isCal == true)
                    PDetailModel.AmmountDue = bill.BalanceAmount.Value + PDetail.PaymentAmount;
                else
                    PDetailModel.AmmountDue = bill.BalanceAmount.Value;

                PDetailModel.DocumentId = PDetail.DocumentId;
                PDetailModel.DocumentType = DocTypeConstants.Bills;
            }
            PDetailModel.SystemReferenceNumber = bill.SystemReferenceNumber;
            PDetailModel.Currency = bill.DocCurrency;
            PDetailModel.BaseExchangeRate = bill.ExchangeRate.Value;
            PDetailModel.DocumentType = bill.DocSubType;
        }
        #endregion

        private void FillJournalState(UpdatePosting _posting, Bill bill)
        {
            _posting.Id = bill.Id;
            _posting.CompanyId = bill.CompanyId;
            _posting.DocumentState = bill.DocumentState;
            _posting.BalanceAmount = bill.BalanceAmount;
            _posting.ModifiedDate = DateTime.UtcNow;
            _posting.ModifiedBy = PaymentsConstants.System;
        }

        #region posting
        public void SaveInvoice1(JVModel clientModel)
        {
            //LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, clientModel);
            var json = RestSharpHelper.ConvertObjectToJason(clientModel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
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

                var message = ex.Message;
            }
        }
        private void FillJournal(JVModel headJournal, Payment payment, bool isNew, bool isClearing, out bool isFirst, bool isfirst1, bool? isICActive, List<PaymentDetailModel> lstPaymentDetail, Dictionary<Guid?, decimal?> lstOfRoundingAmount)
        {
            if (isfirst1)
                doc = payment.SystemRefNo;

            isFirst = true;
            decimal? exchangeRate = 0;
            int recOrder = 0;
            bool isOut = true;

            ChartOfAccount clearing =
                _chartOfAccountService.Query(
                    a =>
                        a.Name ==
                         COANameConstants.Clearing_Payment &&
                        a.CompanyId == payment.CompanyId).Select().FirstOrDefault();

            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = payment.Id;
            if (isClearing)
            {

                if (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency)
                {
                    FillJv(headJournal, payment, isClearing, false);
                    doc = GetNextApplicationNumber(doc, isfirst1, payment.SystemRefNo);
                    headJournal.SystemReferenceNo = doc;
                    isFirst = false;
                }
                else
                {
                    headJournal = null;
                }
            }
            if (!isClearing)
            {
                FillJv(headJournal, payment, isClearing, false);
                doc = GetNextApplicationNumber(doc, isfirst1, payment.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                isFirst = false;
            }
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (!isClearing)
            {
                JVVDetailModel jModel = new JVVDetailModel();
                FillJournalDetail(jModel, payment);
                jModel.DocCredit = payment.PaymentApplicationAmmount;

                jModel.COAId = (payment.PaymentApplicationCurrency == payment.BankPaymentAmmountCurrency) ? payment.COAId : clearing != null ? clearing.Id : payment.COAId;
                FillOutstandingDetails(payment, isNew, lstJD, isICActive, lstPaymentDetail, lstOfRoundingAmount, ref recOrder, ref isOut);
                decimal? Rate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                if (payment.BaseCurrency == payment.PaymentApplicationCurrency)
                {
                    Rate = 1;
                }
                jModel.BaseCredit = Math.Round((decimal)(jModel.DocCredit * Rate), 2, MidpointRounding.AwayFromZero);
                jModel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                lstJD.Add(jModel);
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            }
            if (isClearing)
            {
                if (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency)
                {
                    FillClearing(payment, exchangeRate, clearing, lstJD, isICActive);
                    headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                }
                else
                {
                    headJournal = null;
                }
            }

        }

        private void FillClearing(Payment payment, decimal? exchangeRate, ChartOfAccount clearing, List<JVVDetailModel> lstJD, bool? isICActive)
        {
            int? recOrder = 0;
            if (isICActive == false)
            {
                if (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency)
                {
                    JVVDetailModel journalDetail = new JVVDetailModel();
                    FillJournalDetail(journalDetail, payment);
                    journalDetail.DocumentDetailId = Guid.NewGuid();
                    journalDetail.DocCredit = payment.GrandTotal;
                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                        exchangeRate = 1;
                    else
                        exchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                    journalDetail.BaseCredit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail.DocCredit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail.DocCredit, 2, MidpointRounding.AwayFromZero);
                    journalDetail.RecOrder = recOrder + 1;
                    recOrder = journalDetail.RecOrder;
                    lstJD.Add(journalDetail);
                    JVVDetailModel journalDetail1 = new JVVDetailModel();
                    FillJournalDetail(journalDetail1, payment);
                    journalDetail1.COAId = clearing != null ? clearing.Id : 0;
                    journalDetail1.DocDebit = payment.GrandTotal;
                    journalDetail1.BaseDebit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail1.DocDebit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail1.DocDebit, 2, MidpointRounding.AwayFromZero);
                    journalDetail1.RecOrder = lstJD.Max(d => d.RecOrder) + 1;
                    lstJD.Add(journalDetail1);
                }
            }
            else
            {
                JVVDetailModel journalDetail = new JVVDetailModel();
                FillJournalDetail(journalDetail, payment);
                journalDetail.DocumentDetailId = Guid.NewGuid();
                journalDetail.DocCredit = payment.GrandTotal;
                if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                    exchangeRate = 1;
                else
                    exchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                journalDetail.BaseCredit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail.DocCredit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail.DocCredit, 2, MidpointRounding.AwayFromZero);
                journalDetail.RecOrder = recOrder + 1;
                recOrder = journalDetail.RecOrder;
                lstJD.Add(journalDetail);
                JVVDetailModel journalDetail1 = new JVVDetailModel();
                FillJournalDetail(journalDetail1, payment);
                journalDetail1.COAId = clearing != null ? clearing.Id : 0;
                journalDetail1.DocDebit = payment.GrandTotal;
                journalDetail1.BaseDebit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail1.DocDebit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail1.DocDebit, 2, MidpointRounding.AwayFromZero);
                journalDetail1.RecOrder = lstJD.Max(d => d.RecOrder) + 1;
                lstJD.Add(journalDetail1);
            }
        }

        private void FillOutstandingDetails(Payment payment, bool isNew, List<JVVDetailModel> lstJD, bool? isICActive, List<PaymentDetailModel> lstPaymentDetailM, Dictionary<Guid?, decimal?> lstOfRoundingAmount, ref int recOrder, ref bool isOut)
        {
            foreach (var detail in payment.PaymentDetails.OrderBy(x => x.SystemReferenceNumber))
            {
                if (isOut)
                    recOrder = 0;
                PaymentDetailModel detailM = lstPaymentDetailM.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                if (detail.PaymentAmount > 0)
                {
                    Bill bill = FillJvPayementDetail(payment, journal, detail, isICActive, detailM, lstOfRoundingAmount);
                    journal.SystemRefNo = payment.SystemRefNo;
                    journal.DocNo = payment.DocNo;
                    journal.AccountDescription = payment.Remarks;
                    journal.RecOrder = ++recOrder;
                    lstJD.Add(journal);

                    if (payment.PaymentApplicationCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        if (payment.ServiceCompanyId == bill.ServiceCompanyId)
                            FillCurrrencyCheckJv(payment, journal1, detail, bill);
                        payment.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        recOrder = journal1.RecOrder.Value;
                        isOut = false;
                        if (payment.ExchangeRate != journal1.ExchangeRate && journal1.ExchangeRate != null)
                        {
                            lstJD.Add(journal1);
                        }
                    }
                }
            }
        }

        private void FillCurrrencyCheckJv(Payment payment, JVVDetailModel journal1, PaymentDetail detail, Bill bill)
        {
            var originalExchangeRate1 = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            journal1.DocumentDetailId = detail.Id;
            journal1.DocumentId = payment.Id;
            journal1.SystemRefNo = payment.SystemRefNo;
            journal1.ServiceCompanyId = payment.ServiceCompanyId;
            journal1.DocDate = payment.DocDate;
            journal1.PostingDate = payment.DocDate;
            journal1.AccountDescription = payment.Remarks;
            journal1.DocNo = payment.DocNo;
            journal1.DocType = payment.DocType;
            journal1.DocSubType = payment.DocSubType;
            journal1.DocCurrency = payment.DocCurrency;

            if (bill != null)
            {
                journal1.Nature = bill.Nature;
                journal1.EntityId = bill.EntityId;
                journal1.OffsetDocument = bill.SystemReferenceNumber;
                journal1.ExchangeRate = bill.ExchangeRate;
            }
            ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == payment.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
            {
                journal1.COAId = account2.Id;
            }
            journal1.BaseCurrency = payment.BaseCurrency;
            if (originalExchangeRate1 > journal1.ExchangeRate)
            {
                if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    journal1.BaseDebit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
            }
            if (originalExchangeRate1 < journal1.ExchangeRate)
            {
                if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                {
                    journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
        }
        private void FillCurrrencyCheckJv2(Payment payment, JVVDetailModel journal1, PaymentDetail detail, Bill bill)
        {
            var originalExchangeRate1 = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            journal1.DocumentDetailId = detail.Id;
            journal1.DocumentId = payment.Id;
            journal1.SystemRefNo = payment.SystemRefNo;
            journal1.AccountDescription = payment.Remarks;
            journal1.DocDate = payment.DocDate;
            journal1.DocType = payment.DocType;
            journal1.DocNo = payment.DocNo;
            journal1.DocSubType = payment.DocSubType;
            journal1.PostingDate = payment.DocDate;
            if (bill != null)
            {
                journal1.Nature = bill.Nature;
                journal1.EntityId = bill.EntityId;
                journal1.OffsetDocument = bill.SystemReferenceNumber;
                journal1.ExchangeRate = bill.ExchangeRate;
            }
            ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == payment.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
                journal1.COAId = account2.Id;
            journal1.BaseCurrency = payment.BaseCurrency;
            if (originalExchangeRate1 > journal1.ExchangeRate)
                if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    journal1.BaseDebit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
            if (originalExchangeRate1 < journal1.ExchangeRate)
                if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
        }
        private Bill FillJvPayementDetail(Payment payment, JVVDetailModel journal, PaymentDetail detail, bool? isICActive, PaymentDetailModel detailM, Dictionary<Guid?, decimal?> lstOfRoundingAmount)
        {
            string name = string.Empty;
            bool isIC = false;
            if (detailM != null)
                name = "I/C" + " - " + detailM.ServiceCompanyName;
            var originalExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = payment.Id;
            journal.SystemRefNo = payment.SystemRefNo;
            journal.AccountDescription = payment.Remarks;
            Bill bill = _billService.Query(c => c.Id == detail.DocumentId).Select().FirstOrDefault();
            if (bill != null)
            {
                journal.Nature = bill.Nature;
                journal.EntityId = bill.EntityId;
                journal.OffsetDocument = bill.SystemReferenceNumber;
                journal.ExchangeRate = bill.ExchangeRate;
            }
            if (detail.ServiceCompanyId == payment.ServiceCompanyId)
            {
                ChartOfAccount account1 =
                    _chartOfAccountService.Query(
                        a =>
                            a.Name ==
                            (journal.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables) &&
                            a.CompanyId == payment.CompanyId).Select().FirstOrDefault();
                if (account1 != null)
                    journal.COAId = account1.Id;
            }
            else
            {
                journal.COAId =
                    _chartOfAccountService.Query(
                        a =>
                            a.Name == name
                          &&
                            a.CompanyId == payment.CompanyId).Select(c => c.Id).FirstOrDefault();
                isIC = true;
            }
            journal.SettlementMode = "IT";
            if (detail.ServiceCompanyId != null)
                journal.ServiceCompanyId = detail.ServiceCompanyId.Value;
            journal.DocDate = payment.DocDate;
            journal.PostingDate = payment.DocDate;
            journal.ExchangeRate = (payment.PaymentApplicationCurrency == payment.BaseCurrency) || (payment.PaymentApplicationCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId) ? journal.ExchangeRate : originalExchangeRate;
            journal.DocType = payment.DocType;
            journal.DocNo = payment.DocNo;
            journal.DocSubType = payment.DocSubType;
            journal.DocDebit = detail.PaymentAmount;
            journal.BaseCurrency = payment.BaseCurrency;

            journal.BaseDebit = (isIC != true && bill.DocCurrency != bill.BaseCurrency && lstOfRoundingAmount.Where(a => a.Key == detail.DocumentId).Any()) ? Math.Round((decimal)journal.DocDebit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero) - lstOfRoundingAmount.Where(a => a.Key == detail.DocumentId).Select(a => a.Value).FirstOrDefault() : Math.Round((decimal)(journal.DocDebit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            return bill;
        }
        private Bill FillInterComDetailModel(Payment payment, JVVDetailModel journal, PaymentDetail detail, bool? isICActive, string shotCode, Dictionary<Guid?, decimal?> lstOfRoundingAmount)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = payment.Id;
            journal.SystemRefNo = payment.SystemRefNo;
            Bill bill = _billService.Query(c => c.Id == detail.DocumentId).Select().FirstOrDefault();
            if (bill != null)
            {
                journal.Nature = bill.Nature;
                journal.EntityId = bill.EntityId;
                journal.OffsetDocument = bill.SystemReferenceNumber;
                journal.ExchangeRate = bill.ExchangeRate;
            }

            if (isICActive == true)
            {
                journal.DocCredit = detail.PaymentAmount;
                journal.BaseCredit = payment.BaseCurrency != payment.PaymentApplicationCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(journal.DocCredit * originalExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)(journal.DocCredit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journal.DocCredit, 2, MidpointRounding.AwayFromZero);
                journal.DocDebit = null;
                journal.BaseDebit = null;
                journal.COAId = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId).Select(a => a.Id).FirstOrDefault();

            }
            else
            {
                journal.DocDebit = detail.PaymentAmount;
                if (bill != null)
                    journal.BaseDebit = (bill.DocCurrency != bill.BaseCurrency && lstOfRoundingAmount.Where(a => a.Key == detail.DocumentId).Any()) ? Math.Round((decimal)journal.DocDebit * (decimal)(journal.ExchangeRate == null ? 1 : (decimal)journal.ExchangeRate), 2, MidpointRounding.AwayFromZero) - lstOfRoundingAmount.Where(a => a.Key == detail.DocumentId).Select(a => a.Value).FirstOrDefault() : Math.Round((decimal)(journal.DocDebit * journal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                journal.DocCredit = null;
                journal.BaseCredit = null;
                journal.COAId = _chartOfAccountService.Query(a => a.Name == (journal.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables) && a.CompanyId == payment.CompanyId).Select(a => a.Id).FirstOrDefault();
            }
            journal.SettlementMode = "IT";
            journal.ServiceCompanyId = payment.ServiceCompanyId;
            journal.DocNo = payment.DocNo;
            journal.DocType = payment.DocType;
            journal.DocSubType = payment.DocSubType;
            journal.DocDate = payment.DocDate;
            journal.PostingDate = payment.DocDate;
            journal.BaseCurrency = payment.BaseCurrency;
            journal.AccountDescription = payment.Remarks;
            return bill;
        }
        private void FillJv(JVModel headJournal, Payment payment, bool isClearing, bool? isIC)
        {
            headJournal.DocumentId = payment.Id;
            headJournal.CompanyId = payment.CompanyId;
            headJournal.PostingDate = payment.DocDate;
            headJournal.DocNo = payment.DocNo;
            headJournal.DocType = payment.DocType;
            headJournal.DocSubType = payment.DocSubType;
            headJournal.DocDate = payment.DocDate;
            headJournal.DueDate = payment.DueDate;
            headJournal.DocumentState = payment.DocumentState;
            headJournal.DocumentDescription = payment.Remarks;
            headJournal.ServiceCompanyId = payment.ServiceCompanyId;
            headJournal.ExDurationFrom = payment.ExDurationFrom;
            headJournal.ExDurationTo = payment.ExDurationTo;
            headJournal.GSTExDurationFrom = payment.GSTExDurationFrom;
            headJournal.GSTExDurationTo = payment.GSTExDurationTo;
            headJournal.NoSupportingDocument = payment.NoSupportingDocs;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = payment.EntityId;
            headJournal.EntityType = payment.EntityType;
            ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountById(payment.COAId);
            headJournal.COAId = coa.Id;
            headJournal.AccountCode = coa.Code;
            headJournal.AccountName = coa.Name;

            headJournal.DocCurrency = (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency) && isClearing == true ? payment.BankPaymentAmmountCurrency : payment.DocCurrency;
            headJournal.GrandDocCreditTotal = payment.GrandTotal;
            headJournal.BaseCurrency = payment.BaseCurrency;
            headJournal.IsGstSettings = payment.IsGstSettings;
            headJournal.IsMultiCurrency = payment.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = payment.IsNoSupportingDocument;
            if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && isClearing == true)
                headJournal.ExchangeRate = 1;
            else if (payment.BaseCurrency == payment.PaymentApplicationCurrency && isClearing == false)
                headJournal.ExchangeRate = 1;
            else
                headJournal.ExchangeRate = payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0 ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            headJournal.GrandBaseCreditTotal = Math.Round((decimal)(payment.GrandTotal * (headJournal.ExchangeRate != null ? headJournal.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(payment.GrandTotal * (headJournal.ExchangeRate != null ? headJournal.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
            headJournal.GrandDocDebitTotal = payment.GrandTotal;

            if (payment.IsGstSettings)
            {
                headJournal.GSTExCurrency = payment.GSTExCurrency;
                headJournal.GSTExchangeRate = payment.GSTExchangeRate;
            }
            headJournal.UserCreated = payment.UserCreated;
            headJournal.CreatedDate = payment.CreatedDate;
            headJournal.ModifiedBy = payment.ModifiedBy;
            headJournal.ModifiedDate = payment.ModifiedDate;
            headJournal.ActualSysRefNo = payment.SystemRefNo;
            headJournal.TransferRefNo = payment.PaymentRefNo;
            headJournal.ModeOfReceipt = payment.ModeOfPayment;
        }
        private void ICFillJv(JVModel headJournal, Payment payment, bool isClearing, bool? isIC, PaymentDetail detail)
        {
            headJournal.DocumentId = payment.Id;
            headJournal.CompanyId = payment.CompanyId;
            headJournal.PostingDate = payment.DocDate;
            headJournal.DocNo = payment.DocNo;
            headJournal.DocType = payment.DocType;
            headJournal.DocSubType = payment.DocSubType;
            headJournal.DocDate = payment.DocDate;
            headJournal.DueDate = payment.DueDate;
            headJournal.DocumentState = payment.DocumentState;

            headJournal.ServiceCompanyId = detail.ServiceCompanyId;
            headJournal.ExDurationFrom = payment.ExDurationFrom;
            headJournal.ExDurationTo = payment.ExDurationTo;
            headJournal.GSTExDurationFrom = payment.GSTExDurationFrom;
            headJournal.GSTExDurationTo = payment.GSTExDurationTo;
            headJournal.NoSupportingDocument = payment.NoSupportingDocs;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = payment.EntityId;
            headJournal.EntityType = payment.EntityType;
            headJournal.COAId = payment.COAId;

            headJournal.DocCurrency = (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency) && isClearing == true ? payment.BankPaymentAmmountCurrency : payment.DocCurrency;
            headJournal.GrandDocCreditTotal = payment.GrandTotal;
            headJournal.ExCurrency = payment.BaseCurrency;
            headJournal.IsGstSettings = payment.IsGstSettings;
            headJournal.IsMultiCurrency = payment.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = payment.IsNoSupportingDocument;
            if (payment.BaseCurrency == payment.PaymentApplicationCurrency)
                headJournal.ExchangeRate = 1;
            else
                headJournal.ExchangeRate = payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0 ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            headJournal.GrandBaseCreditTotal = Math.Round((decimal)(payment.GrandTotal * headJournal.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            if (payment.IsGstSettings)
            {
                headJournal.GSTExCurrency = payment.GSTExCurrency;
                headJournal.GSTExchangeRate = payment.GSTExchangeRate;
            }
            headJournal.UserCreated = payment.UserCreated;
            headJournal.CreatedDate = payment.CreatedDate;
            headJournal.ModifiedBy = payment.ModifiedBy;
            headJournal.ModifiedDate = payment.ModifiedDate;
            headJournal.ActualSysRefNo = payment.SystemRefNo;
            headJournal.DocumentDescription = payment.Remarks;
        }

        private void FillJournalDetail(JVVDetailModel jModel, Payment payment)
        {
            jModel.COAId = payment.COAId;
            jModel.DocumentId = payment.Id;
            jModel.SystemReferenceNo = payment.SystemRefNo;
            jModel.SystemRefNo = payment.SystemRefNo;
            jModel.DocNo = payment.DocNo;
            jModel.PostingDate = payment.DocDate;
            jModel.ServiceCompanyId = payment.ServiceCompanyId;
            jModel.DocType = payment.DocType;
            jModel.DocSubType = payment.DocSubType;
            jModel.EntityId = payment.EntityId;
            jModel.DocCurrency = payment.DocCurrency;
            jModel.BaseCurrency = payment.BaseCurrency;
            jModel.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jModel.GSTExCurrency = payment.GSTExCurrency;
            jModel.GSTExchangeRate = payment.GSTExchangeRate;
            jModel.DocDate = payment.DocDate;
            jModel.DocDescription = payment.Remarks;
            jModel.AccountDescription = payment.Remarks;
        }
        public void deleteJVPostInvoce(JournalDeleteModel tObject)
        {
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entred_into_deleteJVPostInvoce_Method);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                object obj = tObject;
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
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
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Existed_from_deleteJVPostInvoce_Method);
        }

        public void UpdatePosting(UpdatePosting upmodel)
        {
            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entred_into_UpdatePosting_Method);
            var json = RestSharpHelper.ConvertObjectToJason(upmodel);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                object obj = upmodel;
                var response = RestSharpHelper.Post(url, "api/journal/updateposting", json);
                if (response.ErrorMessage != null)
                {
                }
                LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Existed_from_UpdatePosting_Method);
            }

            catch (Exception ex)
            {
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

        private void FillInterCompanyJournal(JVModel headJournal, Payment payment, bool isNew, bool isClearing, PaymentDetail detail, string shotCode, Dictionary<Guid?, decimal?> lstOfRoundingAmount)
        {

            bool? isInterCompany = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = payment.Id;
            if (isClearing)
            {
                ICFillJv(headJournal, payment, isClearing, isInterCompany, detail);
                doc = GetNextApplicationNumber(doc, false, payment.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
            }
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            FillIntraOutstandingDetails(payment, detail, true, lstJD, shotCode, lstOfRoundingAmount);
            headJournal.DocCurrency = detail.Currency;
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
        }
        private void FillIntraOutstandingDetails(Payment payment, PaymentDetail detail, bool isNew, List<JVVDetailModel> lstJD, string shotCode, Dictionary<Guid?, decimal?> lstOfRoundingAmount)
        {
            if (detail.PaymentAmount > 0)
            {
                int? recOrder = 0;
                JVVDetailModel journalHead = new JVVDetailModel();
                if (isNew)
                    journalHead.Id = Guid.NewGuid();
                else
                    journalHead.Id = detail.Id;
                Bill bill = FillInterComDetailModel(payment, journalHead, detail, true, shotCode, lstOfRoundingAmount);
                journalHead.DocumentDetailId = new Guid();
                journalHead.RecOrder = recOrder + 1;
                recOrder = journalHead.RecOrder;
                lstJD.Add(journalHead);

                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                Bill bill1 = FillInterComDetailModel(payment, journal, detail, false, shotCode, lstOfRoundingAmount);
                journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                lstJD.Add(journal);
                if (payment.PaymentApplicationCurrency != payment.BaseCurrency)
                {
                    JVVDetailModel journal1 = new JVVDetailModel();
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal1.Id = detail.Id;
                    journalHead.DocCurrency = detail.Currency;
                    FillCurrrencyCheckJv2(payment, journal1, detail, bill1);
                    payment.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                    journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    if (payment.ExchangeRate != journal1.ExchangeRate)
                        lstJD.Add(journal1);
                }
            }
        }

        #endregion posting

        #region PaymentVoidPosting
        public void PaymentJVPostVoid(DocumentVoidModel tObject)
        {

            LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Entred_into_PaymentJVPostVoid_Method);
            var json = RestSharpHelper.ConvertObjectToJason(tObject);
            try
            {
                string url = ConfigurationManager.AppSettings["BeanUrl"].ToString();
                object obj = tObject;
                // string url = ConfigurationManager.AppSettings["IdentityBean"].ToString();
                //url = "http://localhost:57584/";
                var response = RestSharpHelper.Post(url, "api/journal/deletevoidposting", json);
                if (response.ErrorMessage != null)
                {
                    Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
                LoggingHelper.LogMessage(PaymentsConstants.PaymentApplicationSevice, PaymentLoggingValidation.Existed_from_PaymentJVPostVoid_Method);
            }
            catch (Exception ex)
            {
                Log.Logger.ZCritical(ex.StackTrace);

                var message = ex.Message;
            }
        }
        #endregion

        #region Payroll Payment Kendo call

        public IQueryable<PaymentModelK> GetAllPayrollPaymentsK(string username, long companyId)
        {
            try
            {
                return _paymentService.GetAllPaymentK(username, companyId, DocTypeConstants.PayrollPayment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PayrollPayment lookup
        public List<AppsWorld.CommonModule.Infra.LookUpGuid<string>> GetEntityLU(long companyId)
        {
            try
            {
                var lstEntity = _beanEntityService.GetEntityByCompanyId(companyId);
                List<AppsWorld.CommonModule.Infra.LookUpGuid<string>> EntityLu = lstEntity.Select(a => new AppsWorld.CommonModule.Infra.LookUpGuid<string>()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Nature = a.Name,
                    TOPId = a.VenTOPId,
                    CustCreditlimit = a.VenCreditLimit
                }).OrderBy(a => a.Name).ToList();

                //PayrollPaymentModelLU payrollPayment = new PayrollPaymentModelLU();
                //payrollPayment.EntityLU = lstEntity.Select(a => new AppsWorld.CommonModule.Infra.LookUpGuid<string>()
                //{
                //    Id = a.Id,
                //    Name = a.Name,
                //    Nature = a.VenNature,
                //    TOPId = a.VenTOPId,
                //    CustCreditlimit = a.VenCreditLimit
                //}).OrderBy(b => b.Name).ToList();



                return EntityLu;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region WFGetCompanyFeature
        public List<string> GetCompanyFeature(long companyId, long moduleId)
        {
            try
            {
                var lstofFeature = _companyFeatureService.GetCompanyFeature(companyId);
                List<string> names = lstofFeature.Where(a => a.Feature.ModuleId == moduleId).Select(a => a.Feature.Name).ToList();
                return names;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region NewJournal_posting
        private void FillBillJournal(JVModel headJournal, Payment payment, bool isNew, bool isClearing, out bool isFirst, bool isfirst1, bool? isICActive, List<PaymentDetailModel> lstPaymentDetail, long? clearingCoaId, Dictionary<Guid?, decimal?> lstOfRoundingAmount)
        {
            if (isfirst1)
                doc = payment.SystemRefNo;
            //else
            //    doc = doc;
            isFirst = true;
            decimal? exchangeRate = 0;
            int recOrder = 0;
            bool isOut = true;
            //string strServiceCompany = _companyService.Query(a => a.Id == payment.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
            ChartOfAccount clearing =
                _chartOfAccountService.Query(
                    a =>
                        a.Name ==
                         COANameConstants.Clearing_Payment &&
                        a.CompanyId == payment.CompanyId).Select().FirstOrDefault();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = payment.Id;
            if (isClearing)
            {
                //if (isICActive == false)
                //{
                if (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency)
                {
                    FillJvNew(headJournal, payment, isClearing, false);
                    doc = GetNextApplicationNumber(doc, isfirst1, payment.SystemRefNo);
                    headJournal.SystemReferenceNo = doc;
                    isFirst = false;
                }
                else
                {
                    headJournal = null;
                }
            }
            if (!isClearing)
            {
                FillJvNew(headJournal, payment, isClearing, false);
                doc = GetNextApplicationNumber(doc, isfirst1, payment.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                isFirst = false;
            }
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (!isClearing)
            {
                JVVDetailModel jModel = new JVVDetailModel();
                FillJournalDetailNew(jModel, payment);
                jModel.DocCredit = payment.PaymentApplicationAmmount;

                //jModel.COAId = isICActive == true ? clearing != null ? clearing.Id : 0 : (payment.PaymentApplicationCurrency == payment.BankPaymentAmmountCurrency) ? payment.COAId : clearing != null ? clearing.Id : 0;

                jModel.COAId = (payment.PaymentApplicationCurrency == payment.BankPaymentAmmountCurrency) ? payment.COAId : clearing != null ? clearing.Id : payment.COAId;


                //jModel.COAId = isICActive == true ? payment.PaymentApplicationCurrency == payment.BankPaymentAmmountCurrency && (payment.BaseCurrency == payment.PaymentApplicationCurrency || payment.BaseCurrency == payment.BankPaymentAmmountCurrency) ? payment.COAId : clearing != null ? clearing.Id : payment.COAId : (payment.PaymentApplicationCurrency == payment.BankPaymentAmmountCurrency) ? payment.COAId : clearing != null ? clearing.Id : payment.COAId;

                FillOutstandingDetailsNew(payment, isNew, lstJD, isICActive, lstPaymentDetail, lstOfRoundingAmount, ref recOrder, ref isOut);
                decimal? Rate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                if (payment.BaseCurrency == payment.PaymentApplicationCurrency)
                {
                    Rate = 1;
                }
                jModel.BaseCredit = Math.Round((decimal)(jModel.DocCredit * Rate), 2, MidpointRounding.AwayFromZero);
                jModel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                // recOrder = jModel.RecOrder;
                lstJD.Add(jModel);
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            }
            if (isClearing)
            {
                //if (isICActive == false)
                //{
                if (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency)
                {
                    FillClearing(payment, exchangeRate, clearing, lstJD, isICActive);
                    headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                }
                else
                {
                    headJournal = null;
                }

            }

        }
        private void FillJvNew(JVModel headJournal, Payment payment, bool isClearing, bool? isIC)
        {
            headJournal.DocumentId = payment.Id;
            headJournal.CompanyId = payment.CompanyId;
            headJournal.PostingDate = payment.DocDate;
            headJournal.DocNo = payment.DocNo;
            //headJournal.DocType = DocTypeConstants.Payment;
            //headJournal.DocType = payment.DocSubType;
            headJournal.DocType = payment.DocType;
            headJournal.DocSubType = payment.DocSubType;
            //headJournal.DocSubType = payment.DocSubType;
            //headJournal.DocSubType = DocTypeConstants.General;
            headJournal.DocDate = payment.DocDate;
            headJournal.DueDate = payment.DueDate;
            headJournal.DocumentState = payment.DocumentState;
            headJournal.DocumentDescription = payment.Remarks;

            headJournal.ServiceCompanyId = payment.ServiceCompanyId;
            //headJournal.ServiceCompany = strServiceCompany;
            headJournal.ExDurationFrom = payment.ExDurationFrom;
            headJournal.ExDurationTo = payment.ExDurationTo;
            //headJournal.IsAllowableNonAllowable = payment.IsAllowableDisallowable;
            headJournal.GSTExDurationFrom = payment.GSTExDurationFrom;
            headJournal.GSTExDurationTo = payment.GSTExDurationTo;
            headJournal.NoSupportingDocument = payment.NoSupportingDocs;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = payment.EntityId;
            headJournal.EntityType = payment.EntityType;
            ChartOfAccount coa = _chartOfAccountService.GetChartOfAccountById(payment.COAId);
            headJournal.COAId = coa.Id;
            headJournal.AccountCode = coa.Code;
            headJournal.AccountName = coa.Name;

            headJournal.DocCurrency = (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency) && isClearing == true ? payment.BankPaymentAmmountCurrency : payment.DocCurrency;
            headJournal.GrandDocCreditTotal = payment.GrandTotal;
            headJournal.BaseCurrency = payment.BaseCurrency;
            //headJournal.ExCurrency = payment.BaseCurrency;
            //headJournal.ExchangeRate = payment.DocCurrency == payment.BaseCurrency ? 1 : payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0 ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            headJournal.IsGstSettings = payment.IsGstSettings;
            //headJournal.IsGSTApplied = payment.IsGSTApplied;
            headJournal.IsMultiCurrency = payment.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = payment.IsNoSupportingDocument;
            if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && isClearing == true)
                headJournal.ExchangeRate = 1;
            else if (payment.BaseCurrency == payment.PaymentApplicationCurrency && isClearing == false)
                headJournal.ExchangeRate = 1;
            else
                headJournal.ExchangeRate = /*payment.DocCurrency == payment.BaseCurrency ? 1 : */payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0 ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            headJournal.GrandBaseCreditTotal = Math.Round((decimal)(payment.GrandTotal * (headJournal.ExchangeRate != null ? headJournal.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
            headJournal.GrandBaseDebitTotal = Math.Round((decimal)(payment.GrandTotal * (headJournal.ExchangeRate != null ? headJournal.ExchangeRate : 1)), 2, MidpointRounding.AwayFromZero);
            headJournal.GrandDocDebitTotal = payment.GrandTotal;

            if (payment.IsGstSettings)
            {
                headJournal.GSTExCurrency = payment.GSTExCurrency;
                headJournal.GSTExchangeRate = payment.GSTExchangeRate;
            }

            //headJournal.Remarks = payment.Remarks;
            headJournal.UserCreated = payment.UserCreated;
            headJournal.CreatedDate = payment.CreatedDate;
            headJournal.ModifiedBy = payment.ModifiedBy;
            headJournal.ModifiedDate = payment.ModifiedDate;
            headJournal.ActualSysRefNo = payment.SystemRefNo;
            headJournal.TransferRefNo = payment.PaymentRefNo;
            headJournal.ModeOfReceipt = payment.ModeOfPayment;
        }
        private void FillJournalDetailNew(JVVDetailModel jModel, Payment payment)
        {
            jModel.COAId = payment.COAId;
            jModel.DocumentId = payment.Id;
            jModel.SystemReferenceNo = payment.SystemRefNo;
            jModel.SystemRefNo = payment.SystemRefNo;
            jModel.DocNo = payment.DocNo;
            jModel.PostingDate = payment.DocDate;
            jModel.ServiceCompanyId = payment.ServiceCompanyId;
            jModel.DocType = payment.DocType;
            jModel.DocSubType = payment.DocSubType;
            jModel.EntityId = payment.EntityId;
            //jModel.DocType = payment.DocSubType == DocTypeConstants.Payment ? DocTypeConstants.Payment : DocTypeConstants.PayrollPayment;

            jModel.DocCurrency = payment.DocCurrency;
            jModel.BaseCurrency = payment.BaseCurrency;
            jModel.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jModel.GSTExCurrency = payment.GSTExCurrency;
            jModel.GSTExchangeRate = payment.GSTExchangeRate;
            jModel.DocDate = payment.DocDate;
            jModel.DocDescription = payment.Remarks;
            jModel.AccountDescription = payment.Remarks;
        }
        private void FillOutstandingDetailsNew(Payment payment, bool isNew, List<JVVDetailModel> lstJD, bool? isICActive, List<PaymentDetailModel> lstPaymentDetailM, Dictionary<Guid?, decimal?> lstOfRoundingAmount, ref int recOrder, ref bool isOut)
        {
            // int? recOrder2 = 0;
            foreach (var detail in payment.PaymentDetails.OrderBy(x => x.SystemReferenceNumber))
            {
                if (isOut)
                    recOrder = 0;
                PaymentDetailModel detailM = lstPaymentDetailM.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
                JVVDetailModel journal = new JVVDetailModel();
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                if (detail.PaymentAmount > 0)
                {
                    Bill bill = FillJvPayementDetail(payment, journal, detail, isICActive, detailM, lstOfRoundingAmount);
                    //journal.DocCreditTotal = detail.CreditAmount;
                    journal.SystemRefNo = payment.SystemRefNo;
                    journal.DocNo = payment.DocNo;
                    journal.AccountDescription = payment.Remarks;
                    journal.RecOrder = ++recOrder;

                    //recOrder = journal.RecOrder;
                    lstJD.Add(journal);

                    if (payment.PaymentApplicationCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        if (payment.ServiceCompanyId == bill.ServiceCompanyId)
                            FillCurrrencyCheckJv(payment, journal1, detail, bill);
                        payment.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        recOrder = journal1.RecOrder.Value;
                        isOut = false;
                        //recOrder2 = journal1.RecOrder;
                        if (payment.ExchangeRate != journal1.ExchangeRate && journal1.ExchangeRate != null)
                        {
                            lstJD.Add(journal1);
                        }
                    }
                }
            }
        }
        private void FillClearingNew(Payment payment, decimal? exchangeRate, ChartOfAccount clearing, List<JVVDetailModel> lstJD, bool? isICActive)
        {
            int? recOrder = 0;
            if (isICActive == false)
            {
                if (payment.BankPaymentAmmountCurrency != payment.PaymentApplicationCurrency)
                {
                    JVVDetailModel journalDetail = new JVVDetailModel();
                    FillJournalDetail(journalDetail, payment);
                    journalDetail.DocumentDetailId = Guid.NewGuid();
                    journalDetail.DocCredit = payment.GrandTotal;
                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                        exchangeRate = 1;
                    else
                        exchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                    journalDetail.BaseCredit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail.DocCredit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail.DocCredit, 2, MidpointRounding.AwayFromZero);
                    journalDetail.RecOrder = recOrder + 1;
                    recOrder = journalDetail.RecOrder;
                    lstJD.Add(journalDetail);
                    JVVDetailModel journalDetail1 = new JVVDetailModel();
                    FillJournalDetail(journalDetail1, payment);
                    journalDetail1.COAId = clearing != null ? clearing.Id : 0;
                    journalDetail1.DocDebit = payment.GrandTotal;
                    journalDetail1.BaseDebit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail1.DocDebit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail1.DocDebit, 2, MidpointRounding.AwayFromZero);
                    journalDetail1.RecOrder = lstJD.Max(d => d.RecOrder) + 1;
                    //recOrder = journalDetail1.RecOrder;
                    lstJD.Add(journalDetail1);
                }
            }
            else
            {
                JVVDetailModel journalDetail = new JVVDetailModel();
                FillJournalDetail(journalDetail, payment);
                journalDetail.DocumentDetailId = Guid.NewGuid();
                journalDetail.DocCredit = payment.GrandTotal;
                if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                    exchangeRate = 1;
                else
                    exchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                journalDetail.BaseCredit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail.DocCredit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail.DocCredit, 2, MidpointRounding.AwayFromZero);
                journalDetail.RecOrder = recOrder + 1;
                recOrder = journalDetail.RecOrder;
                lstJD.Add(journalDetail);
                JVVDetailModel journalDetail1 = new JVVDetailModel();
                FillJournalDetail(journalDetail1, payment);
                journalDetail1.COAId = clearing != null ? clearing.Id : 0;
                journalDetail1.DocDebit = payment.GrandTotal;
                journalDetail1.BaseDebit = (exchangeRate != 0 && exchangeRate != null) ? Math.Round((decimal)(journalDetail1.DocDebit * exchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)journalDetail1.DocDebit, 2, MidpointRounding.AwayFromZero);
                journalDetail1.RecOrder = lstJD.Max(d => d.RecOrder) + 1;
                //recOrder = journalDetail1.RecOrder;
                lstJD.Add(journalDetail1);
            }
        }


        private void FillBillJournalNew(JVModel headJournal, Payment payment, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<PaymentDetail> lstDetail, long? clearingPaymentsCoaId)
        {
            if (isfirst1)
                doc = payment.DocNo;
            //else
            //    doc = doc;
            isFirst = true;
            //string strServiceCompany = _companyService.GetById(invoice.ServiceCompanyId).ShortName;

            //invoice.ExchangeRate = invoice.SystemCalculatedExchangeRate != null ? invoice.SystemCalculatedExchangeRate : invoice.ExchangeRate;
            //TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice);
            //JournalModel headJournal = new JournalModel();
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = payment.Id;
            FillHeadJVNew(headJournal, payment, isBalancing);
            doc = GetNextApplicationNumber(doc, isfirst1, payment.DocNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == true)
            {
                //JVVDetailModel Jmodel = new JVVDetailModel();
                //FillJDetail(Jmodel, invoice, "BankReceiptAmmount", headJournal.ExchangeRate);
                //Jmodel.RecOrder = recorder + 1;
                //recorder = Jmodel.RecOrder;
                //lstJD.Add(Jmodel);
                //Jmodel = new JVVDetailModel();
                //if (invoice.BankCharges != null)
                //{
                //    FillJDetail(Jmodel, invoice, "BankCharges", headJournal.ExchangeRate);
                //    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                //    //recorder = Jmodel.RecOrder;
                //    lstJD.Add(Jmodel);
                //}
                //Jmodel = new JVVDetailModel();
                //if (invoice.ExcessPaidByClientAmmount != null)
                //{
                //    FillJDetail(Jmodel, invoice, "ExcesPaidByClient", headJournal.ExchangeRate);
                //    Jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                //    //recorder = Jmodel.RecOrder;
                //    lstJD.Add(Jmodel);
                //}

                recorder1 = recorder;
                if (payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                {
                    JVVDetailModel jmodel1 = new JVVDetailModel();
                    FillClearingPayments(jmodel1, payment, true, headJournal.ExchangeRate, clearingPaymentsCoaId);
                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    // recorder = jmodel1.RecOrder;
                    lstJD.Add(jmodel1);
                }
                if (payment.BankPaymentAmmountCurrency == payment.DocCurrency)
                {
                    foreach (PaymentDetail detail in payment.PaymentDetails.Where(a => a.PaymentAmount != 0))
                    {
                        if (detail.PaymentAmount != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            if (isNew)
                                jmodel.Id = Guid.NewGuid();
                            else
                                jmodel.Id = detail.Id;
                            FillDetail(jmodel, detail, payment, lstDetail);
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            //recorder = jmodel.RecOrder;
                            lstJD.Add(jmodel);
                            if (payment.DocCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, payment, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }
                        }
                    }
                }
            }
            if (isBalancing == false)
            {
                recorder = 0;
                JVVDetailModel jmodel3 = new JVVDetailModel();
                jmodel3 = new JVVDetailModel();
                FillClearingPayments(jmodel3, payment, false, headJournal.ExchangeRate, clearingPaymentsCoaId);
                jmodel3.RecOrder = recorder + 1;
                recorder = jmodel3.RecOrder;
                lstJD.Add(jmodel3);
                foreach (PaymentDetail detail in payment.PaymentDetails.Where(a => a.PaymentAmount > 0))
                {
                    if (detail.PaymentAmount != 0)
                    {
                        JVVDetailModel jmodel = new JVVDetailModel();
                        if (isNew)
                            jmodel.Id = Guid.NewGuid();
                        else
                            jmodel.Id = detail.Id;
                        FillDetail(jmodel, detail, payment, lstDetail);
                        jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //recorder = jmodel.RecOrder;
                        lstJD.Add(jmodel);
                        if (payment.DocCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId)
                        {
                            JVVDetailModel jmodel2 = new JVVDetailModel();
                            if (isNew)
                                jmodel2.Id = Guid.NewGuid();
                            else
                                jmodel2.Id = detail.Id;
                            FillGstDetail(jmodel2, detail, payment, jmodel);
                            // jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            jmodel2.RecOrder = recorder - 1;
                            recorder = jmodel2.RecOrder;
                            if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                lstJD.Add(jmodel2);
                        }
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }

        private void FillHeadJVNew(JVModel headJournal, Payment payment, bool isBalancing)
        {
            headJournal.DocumentId = payment.Id;
            headJournal.CompanyId = payment.CompanyId;
            headJournal.PostingDate = payment.DocDate;
            headJournal.DocNo = payment.DocNo;
            headJournal.DocType = payment.DocType;
            //headJournal.DocSubType = invoice.DocSubType;
            headJournal.DocSubType = payment.DocSubType;
            headJournal.DocDate = payment.DocDate;
            headJournal.DueDate = payment.DueDate;
            headJournal.DocumentState = payment.DocumentState;
            //headJournal.SystemReferenceNo = invoice.SystemRefNo;
            headJournal.ServiceCompanyId = payment.ServiceCompanyId;
            headJournal.ExDurationFrom = payment.ExDurationFrom;
            headJournal.ExDurationTo = payment.ExDurationTo;
            headJournal.GSTExDurationFrom = payment.GSTExDurationFrom;
            headJournal.GSTExDurationTo = payment.GSTExDurationTo;
            headJournal.NoSupportingDocument = payment.NoSupportingDocs;
            headJournal.IsNoSupportingDocs = payment.IsNoSupportingDocument;
            headJournal.Status = AppsWorld.Framework.RecordStatusEnum.Active;
            headJournal.EntityId = payment.EntityId;
            //AppsWorld.ReceiptModule.Entities.BeanEntity entity = _beanEntityService.Query(a => a.Id == invoice.EntityId).Select().FirstOrDefault();
            //if (entity != null)
            //{
            //    headJournal.EntityName = entity.Name;
            //    headJournal.Nature = entity.CustNature;
            //}
            headJournal.EntityType = payment.EntityType;
            //AppsWorld.CommonModule.Entities.ChartOfAccount account = _chartOfAccountService.GetChartOfAccountById(payment.COAId);
            headJournal.COAId = payment.COAId;
            //headJournal.AccountCode = account.Code;
            //headJournal.AccountName = account.Name;
            headJournal.BankClearingDate = payment.BankClearingDate;
            if (isBalancing)
            {
                headJournal.DocCurrency = payment.BankPaymentAmmountCurrency;
                //headJournal.IsAllowableNonAllowable = true;
            }
            else
            {
                headJournal.DocCurrency = payment.DocCurrency;
                headJournal.IsBalancing = false;
            }

            headJournal.GrandDocDebitTotal = payment.GrandTotal != 0 ? payment.GrandTotal : payment.BankPaymentAmmount.Value;
            headJournal.BaseCurrency = payment.BaseCurrency;
            headJournal.ExchangeRate = (payment.BaseCurrency == payment.BankPaymentAmmountCurrency || payment.BaseCurrency == payment.PaymentApplicationCurrency) ? 1 : payment.SystemCalculatedExchangeRate == 0 || payment.SystemCalculatedExchangeRate == null ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            // headJournal.ExchangeRate = invoice.SystemCalculatedExchangeRate;
            headJournal.GrandBaseDebitTotal = payment.GrandTotal != 0 ? Math.Round((decimal)(payment.GrandTotal * (payment.ExchangeRate == null ? 1 : payment.ExchangeRate)), 2, MidpointRounding.AwayFromZero) : payment.BankPaymentAmmount;
            if (payment.IsGstSettings)
            {
                headJournal.GSTExCurrency = payment.GSTExCurrency;
                headJournal.GSTExchangeRate = payment.GSTExchangeRate;
            }
            headJournal.ModeOfReceipt = payment.ModeOfPayment;
            headJournal.Remarks = payment.Remarks;
            headJournal.DocumentDescription = payment.Remarks;
            headJournal.UserCreated = payment.UserCreated;
            headJournal.CreatedDate = payment.CreatedDate;
            headJournal.ModifiedBy = payment.ModifiedBy;
            headJournal.ModifiedDate = payment.ModifiedDate;
            headJournal.ActualSysRefNo = payment.SystemRefNo;
            headJournal.TransferRefNo = payment.PaymentRefNo;//added by lokanath
        }


        private void FillClearingPayments(JVVDetailModel jmodel1, Payment payment, bool isBank, decimal? exchangeRate, long? clearingPaymentsCOAId)
        {
            jmodel1.DocType = payment.DocType;
            jmodel1.DocumentDetailId = payment.Id;
            jmodel1.DocumentId = payment.Id;
            jmodel1.DocType = payment.DocType;
            jmodel1.DocSubType = payment.DocSubType;
            jmodel1.SystemRefNo = payment.SystemRefNo;
            jmodel1.DocNo = payment.DocNo;
            jmodel1.PostingDate = payment.DocDate;
            jmodel1.EntityId = payment.EntityId;
            //AppsWorld.ReceiptModule.Entities.BeanEntity entity = _beanEntityService.Query(a => a.Id == payment.EntityId).Select().FirstOrDefault();
            //jmodel1.EntityName = entity.Name;
            jmodel1.EntityType = payment.EntityType;
            jmodel1.BaseCurrency = payment.ExCurrency;
            jmodel1.DocCurrency = payment.BankChargesCurrency;
            jmodel1.ServiceCompanyId = payment.ServiceCompanyId;
            jmodel1.AccountDescription = payment.Remarks;
            jmodel1.DocDate = payment.DocDate;
            if (payment.DocCurrency != payment.BaseCurrency || isBank == true)
                jmodel1.ExchangeRate = exchangeRate;
            //payment.SystemCalculatedExchangeRate == 0 || payment.SystemCalculatedExchangeRate == null ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            else
                jmodel1.ExchangeRate = 1;
            //AppsWorld.ReceiptModule.Entities.ChartOfAccount chart = _chartOfAccountService.GetByName(COANameConstants.Clearingpayments, payment.CompanyId);
            //if (chart != null)
            //{
            jmodel1.COAId = clearingPaymentsCOAId;
            //jmodel1.AccountName = chart.Name;
            //}
            if (isBank == true)
            {
                jmodel1.DocCredit = payment.GrandTotal;
                jmodel1.BaseCredit = Math.Round((decimal)jmodel1.DocCredit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            }
            if (isBank == false)
            {
                jmodel1.DocCurrency = payment.DocCurrency;
                jmodel1.DocDebit = payment.PaymentApplicationAmmount.Value;
                jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            }
            jmodel1.IsTax = false;
        }


        private void FillDetail(JVVDetailModel jmodel, PaymentDetail detail, Payment payment, List<PaymentDetail> lstDetail)
        {
            PaymentDetail detailM = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
            string shotCode = string.Empty;
            if (detailM != null)
            {
                shotCode = _companyService.Query(a => a.Id == detailM.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                shotCode = "I/C" + " - " + shotCode;
            }
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.Nature = detail.Nature;
            jmodel.DocCurrency = payment.DocCurrency;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == detail.ServiceCompanyId*/).Select().FirstOrDefault();
            if (payment.ServiceCompanyId == detail.ServiceCompanyId)
            {
                if (account1 != null)
                    jmodel.COAId = account1.Id;
                jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            }
            else
            {
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
                if (detail.ServiceCompanyId != null)
                    jmodel.ServiceCompanyId = detail.ServiceCompanyId.Value;
            }
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            jmodel.DocDate = payment.DocDate;
            jmodel.DocNo = payment.DocNo;
            jmodel.PostingDate = payment.DocDate;
            //jmodel.DocType = detail.DocumentType;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;

            if (detail.DocumentType != DocTypeConstants.Bills)
            {
                jmodel.DocCredit = detail.PaymentAmount;
                if ((payment.DocCurrency == payment.BaseCurrency) || (payment.DocCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId))
                    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                else
                {
                    jmodel.ExchangeRate = payment.SystemCalculatedExchangeRate == 0 || payment.SystemCalculatedExchangeRate == null ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                    jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                jmodel.DocDebit = detail.PaymentAmount;
                if ((payment.DocCurrency == payment.BaseCurrency) || (payment.DocCurrency != payment.BaseCurrency && payment.ServiceCompanyId == detail.ServiceCompanyId))
                    jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                else
                {
                    jmodel.ExchangeRate = payment.SystemCalculatedExchangeRate == 0 || payment.SystemCalculatedExchangeRate == null ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                    jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                }
            }
        }

        private void FillGstDetail(JVVDetailModel jmodel2, PaymentDetail detail, Payment payment, JVVDetailModel jmodel)
        {
            jmodel2.DocType = payment.DocType;
            jmodel2.DocSubType = payment.DocSubType;
            jmodel2.AccountDescription = payment.Remarks;
            jmodel2.SystemRefNo = payment.SystemRefNo;
            // jmodel2.DocType = detail.DocumentType;
            jmodel2.DocumentDetailId = detail.Id;
            jmodel2.DocumentId = detail.PaymentId;
            jmodel2.PostingDate = payment.DocDate;
            jmodel2.DocNo = payment.DocNo;
            //jmodel2.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel2.DocDate = payment.DocDate;
            jmodel2.Nature = detail.Nature;
            jmodel2.DocCurrency = payment.DocCurrency;
            AppsWorld.CommonModule.Entities.ChartOfAccount account2 = _chartOfAccountService.GetByName(COANameConstants.ExchangeGainLossRealised, payment.CompanyId);
            if (account2 != null)
            {
                jmodel2.COAId = account2.Id;
                jmodel2.AllowDisAllow = account2.DisAllowable;
            }
            jmodel2.EntityId = payment.EntityId;
            jmodel2.BaseCurrency = payment.BaseCurrency;
            jmodel2.ExchangeRate = payment.SystemCalculatedExchangeRate == 0 || payment.SystemCalculatedExchangeRate == null ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jmodel2.OffsetDocument = detail.SystemReferenceNumber;
            jmodel2.ServiceCompanyId = payment.ServiceCompanyId;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote)
            {
                if (jmodel2.ExchangeRate > jmodel.ExchangeRate)
                {
                    jmodel2.BaseCredit = Math.Round((decimal)(((jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate) - (jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate)) * detail.PaymentAmount), 2, MidpointRounding.AwayFromZero);
                }
                if (jmodel2.ExchangeRate < jmodel.ExchangeRate)
                {
                    jmodel2.BaseDebit = Math.Round((decimal)(((jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate) - (jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate))) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
            else
            {
                if (jmodel2.ExchangeRate > jmodel.ExchangeRate)
                {
                    jmodel2.BaseDebit = Math.Round((decimal)(((jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate) - (jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate)) * detail.PaymentAmount), 2, MidpointRounding.AwayFromZero);
                }
                if (jmodel2.ExchangeRate < jmodel.ExchangeRate)
                {
                    jmodel2.BaseCredit = Math.Round((decimal)(((jmodel.ExchangeRate == null ? 0 : jmodel.ExchangeRate) - (jmodel2.ExchangeRate == null ? 0 : jmodel2.ExchangeRate))) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                }
            }
        }


        private void FillPaymentOffsetJournal(JVModel headJournal, Payment _payment, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<PaymentDetail> lstDetail, bool? isBankDocDiff, long? clearingCOAId, bool? icActive)
        {
            if (isfirst1)
                doc = _payment.DocNo;
            isFirst = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _payment.Id;
            FillHeadJVNew(headJournal, _payment, false);
            doc = GetNextApplicationNumber(doc, isfirst1, _payment.DocNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == true)
            {
                recorder = 1;
                recorder1 = recorder;
                if (_payment.DocCurrency == _payment.BankPaymentAmmountCurrency)
                {
                    JVVDetailModel jDetil = new JVVDetailModel();
                    FillJDetailNew(jDetil, _payment, clearingCOAId);
                    jDetil.RecOrder = 1;
                    lstJD.Add(jDetil);
                }
                if (icActive == true && isBankDocDiff == true)
                {
                    JVVDetailModel jDetil = new JVVDetailModel();
                    FillJDetailNew(jDetil, _payment, clearingCOAId);
                    jDetil.RecOrder = 1;
                    lstJD.Add(jDetil);
                }


                if (_payment.PaymentDetails.Any(c => c.ServiceCompanyId != _payment.ServiceCompanyId) || _payment.PaymentDetails.GroupBy(c => c.ServiceCompanyId).Count() != 1)
                {
                    var lstGrpDetail1 = _payment.PaymentDetails.Where(c => c.PaymentAmount > 0 && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim || c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).GroupBy(c => /*new { c.ServiceCompanyId, c.DocumentType }*/ c.ServiceCompanyId).Select(c => new { Detail = c.ToList(), Count = c.Count() }).ToList();
                    if (lstGrpDetail1.Any())
                    {
                        decimal? amt = 0;
                        foreach (PaymentDetail detail in lstGrpDetail1.Where(c => c.Count >= 2).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            amt = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim)).Sum(c => c.PaymentAmount) - lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.PaymentAmount);
                            if (isNew)
                                jmodel.Id = Guid.NewGuid();
                            else
                                jmodel.Id = detail.Id;

                            FillDetailOffset(jmodel, detail, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId, false, detail.DocumentType);
                            jmodel.RecOrder = lstJD.Any() ? lstJD.Max(c => c.RecOrder) + 1 : 1;
                            lstJD.Add(jmodel);

                            if (_payment.DocCurrency == _payment.BankPaymentAmmountCurrency && _payment.DocCurrency != _payment.BaseCurrency)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, _payment, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }

                        }
                        foreach (PaymentDetail detail in lstGrpDetail1.Where(c => c.Count == 1).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.PaymentAmount != 0)
                            {
                                amt = 0;
                                JVVDetailModel jmodel = new JVVDetailModel();
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;
                                //isCN = detail.DocumentType == DocTypeConstants.CreditNote;
                                amt = detail.PaymentAmount;
                                FillDetailOffset(jmodel, detail, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId, false, detail.DocumentType);
                                jmodel.RecOrder = lstJD.Any() ? lstJD.Max(c => c.RecOrder) + 1 : 1;
                                lstJD.Add(jmodel);
                                if (_payment.DocCurrency == _payment.BankPaymentAmmountCurrency && _payment.DocCurrency != _payment.BaseCurrency)
                                {
                                    JVVDetailModel jmodel2 = new JVVDetailModel();
                                    if (isNew)
                                        jmodel2.Id = Guid.NewGuid();
                                    else
                                        jmodel2.Id = detail.Id;
                                    FillGstDetail(jmodel2, detail, _payment, jmodel);
                                    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                        lstJD.Add(jmodel2);
                                }
                            }

                        }
                    }

                    var lstGrpDetail = _payment.PaymentDetails.Where(c => c.PaymentAmount > 0 && c.DocumentType == DocTypeConstants.CreditNote || c.DocumentType == DocTypeConstants.BillCreditMemo).GroupBy(c => new { c.ServiceCompanyId, c.DocumentType } /*c.ServiceCompanyId*/).Select(c => new { Detail = c.ToList(), Count = c.Count() }).ToList();
                    if (lstGrpDetail.Any())
                    {
                        decimal? amt = 0;
                        bool? isCN = false;
                        foreach (PaymentDetail detail in lstGrpDetail.Where(c => c.Count >= 2).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.PaymentAmount != 0)
                            {
                                JVVDetailModel jmodel = new JVVDetailModel();
                                //amt=lstGrpDetail.
                                amt = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim)).Sum(c => c.PaymentAmount) - lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.PaymentAmount);
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;

                                FillDetailOffset(jmodel, detail, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId, isCN, detail.DocumentType);
                                jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                lstJD.Add(jmodel);
                                JVVDetailModel jmodel1 = new JVVDetailModel();
                                jmodel1.Id = Guid.NewGuid();
                                jmodel1.Id = detail.Id;
                                amt = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.CreditNote).Sum(c => c.PaymentAmount);
                                if (amt != 0)
                                {
                                    isCN = true;
                                    FillDetailOffset(jmodel1, detail, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId, isCN, detail.DocumentType);
                                    jmodel1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    lstJD.Add(jmodel1);
                                }
                                if (_payment.DocCurrency == _payment.BankPaymentAmmountCurrency && _payment.DocCurrency != _payment.BaseCurrency)
                                {
                                    JVVDetailModel jmodel2 = new JVVDetailModel();
                                    if (isNew)
                                        jmodel2.Id = Guid.NewGuid();
                                    else
                                        jmodel2.Id = detail.Id;
                                    FillGstDetail(jmodel2, detail, _payment, jmodel);
                                    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                        lstJD.Add(jmodel2);
                                }
                            }
                        }
                        foreach (PaymentDetail detail in lstGrpDetail.Where(c => c.Count == 1).Select(c => c.Detail.FirstOrDefault()).ToList())
                        {
                            if (detail.PaymentAmount != 0)
                            {
                                amt = 0;
                                JVVDetailModel jmodel = new JVVDetailModel();
                                if (isNew)
                                    jmodel.Id = Guid.NewGuid();
                                else
                                    jmodel.Id = detail.Id;
                                isCN = detail.DocumentType == DocTypeConstants.CreditNote;
                                amt = detail.PaymentAmount;
                                FillDetailOffset(jmodel, detail, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId, isCN, detail.DocumentType);
                                jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                lstJD.Add(jmodel);
                                if (_payment.DocCurrency == _payment.BankPaymentAmmountCurrency && _payment.DocCurrency != _payment.BaseCurrency)
                                {
                                    JVVDetailModel jmodel2 = new JVVDetailModel();
                                    if (isNew)
                                        jmodel2.Id = Guid.NewGuid();
                                    else
                                        jmodel2.Id = detail.Id;
                                    FillGstDetail(jmodel2, detail, _payment, jmodel);
                                    jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                    if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                        lstJD.Add(jmodel2);
                                }
                            }
                        }
                    }
                }
                else
                {
                    decimal? amt = 0;
                    if (_payment.DocCurrency == _payment.BankPaymentAmmountCurrency)
                    {
                        foreach (PaymentDetail detail in _payment.PaymentDetails.Where(c => c.PaymentAmount != 0)/*.OrderBy(c => c.RecOrder)*/)
                        {
                            amt = 0;
                            JVVDetailModel jmodel = new JVVDetailModel();
                            if (isNew)
                                jmodel.Id = Guid.NewGuid();
                            else
                                jmodel.Id = detail.Id;
                            if (_payment.DocCurrency != _payment.BaseCurrency && _payment.BaseCurrency == _payment.BankPaymentAmmountCurrency)
                                amt = Math.Round(detail.PaymentAmount * (decimal)(_payment.DocCurrency == _payment.BankPaymentAmmountCurrency ? _payment.ExchangeRate : _payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                            else
                                amt = detail.PaymentAmount;
                            FillDetailOffset2(jmodel, detail, _payment, lstDetail, amt, clearingCOAId);
                            jmodel.RecOrder = lstJD.Any() ? lstJD.Max(c => c.RecOrder) + 1 : 1;
                            //if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.Bills)
                            //{
                            //    jmodel.COAId = clearingCOAId;
                            //    if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills)
                            //    {
                            //        jmodel.DocDebit = amt;
                            //        jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                            //        jmodel.DocCredit = null;
                            //        jmodel.BaseCredit = null;
                            //    }
                            //}
                            //recorder = jmodel.RecOrder;
                            lstJD.Add(jmodel);
                            if (_payment.DocCurrency != _payment.BaseCurrency && detail.DocumentType != DocTypeConstants.CreditNote && detail.DocumentType != DocTypeConstants.BillCreditMemo)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, _payment, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }
                        }
                    }
                    else
                    {
                        amt = _payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim && c.ServiceCompanyId == _payment.ServiceCompanyId).Sum(c => c.PaymentAmount) - _payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote) && c.ServiceCompanyId == _payment.ServiceCompanyId).Sum(c => c.PaymentAmount);
                        if (amt != 0)
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();

                            FillDetailOffsetDetailNew(jmodel, DocTypeConstants.Bills, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId, false);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Any() ? lstJD.Max(c => c.RecOrder) + 1 : 1;
                            lstJD.Add(jmodel);
                        }

                        foreach (PaymentDetail detail in _payment.PaymentDetails.Where(c => (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim || c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote) && c.PaymentAmount != 0))
                        {
                            JVVDetailModel jmodel = new JVVDetailModel();
                            jmodel.Id = Guid.NewGuid();
                            //FillJDetailIfCurrencyDiff(jmodel, detail.DocumentType, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId);
                            FillJDetailIfCurrencyDiff(jmodel, detail, _payment, lstDetail, amt, clearingCOAId);
                            jmodel.DocumentDetailId = jmodel.Id;
                            jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                            lstJD.Add(jmodel);

                            if (_payment.DocCurrency != _payment.BaseCurrency)
                            {
                                JVVDetailModel jmodel2 = new JVVDetailModel();
                                if (isNew)
                                    jmodel2.Id = Guid.NewGuid();
                                else
                                    jmodel2.Id = detail.Id;
                                FillGstDetail(jmodel2, detail, _payment, jmodel);
                                jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                                if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                                    lstJD.Add(jmodel2);
                            }
                        }

                        //amt = _payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.DocumentType == DocTypeConstants.CreditNote && c.ServiceCompanyId == _payment.ServiceCompanyId).Sum(c => c.PaymentAmount);
                        //if (amt != 0)
                        //{
                        //    JVVDetailModel jmodel = new JVVDetailModel();
                        //    jmodel.Id = Guid.NewGuid();

                        //    FillDetailOffsetDetailNew(jmodel, DocTypeConstants.CreditNote, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId);
                        //    jmodel.DocumentDetailId = jmodel.Id;
                        //    jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //    lstJD.Add(jmodel);
                        //}
                        //amt = _payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.DocumentType == DocTypeConstants.BillCreditMemo && c.ServiceCompanyId == _payment.ServiceCompanyId).Sum(c => c.PaymentAmount);
                        //if (amt != 0)
                        //{
                        //    JVVDetailModel jmodel = new JVVDetailModel();
                        //    jmodel.Id = Guid.NewGuid();
                        //    FillDetailOffsetDetailNew(jmodel, DocTypeConstants.BillCreditMemo, _payment, lstDetail, amt, isBankDocDiff, clearingCOAId);
                        //    jmodel.DocumentDetailId = jmodel.Id;
                        //    jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //    lstJD.Add(jmodel);
                        //}
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }


        private void FillDetailOffset(JVVDetailModel jmodel, PaymentDetail detail, Payment payment, List<PaymentDetail> lstDetail, decimal? receiptAmount, bool? isBankDocDiff, long? clearingCOAId, bool? isCN, string docType)
        {
            PaymentDetail detailM = lstDetail.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId).FirstOrDefault();
            string shotCode = string.Empty;
            if (detailM != null)
            {
                shotCode = _companyService.Query(a => a.Id == detailM.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
                shotCode = "I/C" + " - " + shotCode;
            }
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == detail.ServiceCompanyId*/).Select().FirstOrDefault();
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            if (payment.ServiceCompanyId == detail.ServiceCompanyId)
            {
                if (account1 != null)
                    //jmodel.COAId = isBankDocDiff == true ? clearingCOAId : account1.Id;
                    jmodel.COAId = (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.BillCreditMemo) ? clearingCOAId : isBankDocDiff == true ? clearingCOAId : account1.Id;
                jmodel.ServiceCompanyId = payment.ServiceCompanyId;
                jmodel.ExchangeRate = payment.BaseCurrency == payment.BankChargesCurrency ? 1 : jmodel.ExchangeRate;
            }
            else
            {
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
                if (detail.ServiceCompanyId != null)
                    jmodel.ServiceCompanyId = detail.ServiceCompanyId.Value;
                jmodel.ExchangeRate = payment.BaseCurrency == payment.BankPaymentAmmountCurrency ? 1 : payment.ExchangeRate;
            }

            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;

            jmodel.DocDate = payment.DocDate;
            jmodel.DocNo = payment.DocNo;
            jmodel.PostingDate = payment.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;


            if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.DebitNote || docType == DocTypeConstants.BillCreditMemo)
            {
                if (payment.DocCurrency != payment.BaseCurrency && payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = jmodel.BaseCredit;

                }
                else if (payment.DocCurrency == payment.BaseCurrency && payment.DocCurrency != payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseCredit = receiptAmount;
                    jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else if (payment.DocCurrency == payment.BankPaymentAmmountCurrency && payment.BaseCurrency != payment.DocCurrency)
                {
                    jmodel.DocCredit = receiptAmount;
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    jmodel.BaseCredit = receiptAmount;
                    jmodel.DocCredit = receiptAmount;
                }

                //jmodel.DocCredit = receiptAmount;
                //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else if (detail.DocumentType == DocTypeConstants.Bills)
            {
                if (payment.DocCurrency != payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency == payment.BaseCurrency)
                {
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = receiptAmount;
                }
                else if (payment.DocCurrency != payment.BankPaymentAmmountCurrency && payment.BaseCurrency == payment.DocCurrency)
                {
                    jmodel.DocCredit = receiptAmount;
                    jmodel.BaseCredit = jmodel.DocCredit;
                }
                else if (payment.DocCurrency == payment.BankPaymentAmmountCurrency && payment.BaseCurrency != payment.DocCurrency)
                {
                    jmodel.DocCredit = receiptAmount;
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    jmodel.BaseCredit = receiptAmount;
                    jmodel.DocCredit = receiptAmount;
                }
            }
            else
            {
                if (payment.DocCurrency != payment.BaseCurrency && payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = jmodel.BaseDebit;
                }
                else if (payment.DocCurrency == payment.BaseCurrency && payment.DocCurrency != payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseDebit = receiptAmount;
                    jmodel.DocDebit = Math.Round((decimal)receiptAmount / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.DocDebit = Math.Round((decimal)receiptAmount / (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else if (payment.DocCurrency == payment.BankPaymentAmmountCurrency && payment.BaseCurrency != payment.DocCurrency)
                {
                    jmodel.DocDebit = receiptAmount;
                    jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    jmodel.BaseDebit = receiptAmount;
                    jmodel.DocDebit = receiptAmount;
                }
                //jmodel.DocDebit = receiptAmount;
                //jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }

            //if (payment.DocCurrency == payment.BaseCurrency && payment.DocCurrency != payment.BankPaymentAmmountCurrency)
            //{
            //    jmodel.BaseCredit = receiptAmount;
            //    jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            //}
            //else
            //{
            //    jmodel.DocCredit = receiptAmount;
            //    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            //}
            //if (isCN == true)
            //{
            //    jmodel.DocDebit = jmodel.DocCredit;
            //    jmodel.BaseDebit = jmodel.BaseCredit;
            //    jmodel.DocCredit = null;
            //    jmodel.BaseCredit = null;
            //}
            if (receiptAmount < 0)
            {
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }


        private void FillDetailOffset2(JVVDetailModel jmodel, PaymentDetail detail, Payment payment, List<PaymentDetail> lstDetail, decimal? receiptAmount, long? clearingCOAId)
        {
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            if (account1 != null)
                jmodel.COAId = detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.CreditNote ? clearingCOAId : account1.Id;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            jmodel.ExchangeRate = payment.BaseCurrency == payment.BankPaymentAmmountCurrency ? 1 : jmodel.ExchangeRate;
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.DocDate = payment.DocDate;
            jmodel.DocNo = payment.DocNo;
            jmodel.PostingDate = payment.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
            {
                jmodel.DocCredit = receiptAmount;
                jmodel.BaseCredit = detail.DocumentType == DocTypeConstants.BillCreditMemo ? Math.Round((decimal)jmodel.DocCredit * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
            {
                jmodel.DocDebit = receiptAmount;
                jmodel.BaseDebit = (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General) ? Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (receiptAmount < 0)
            {
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }


        private void FillDetailOffsetDetailNew(JVVDetailModel jmodel, string docType, Payment payment, List<PaymentDetail> lstDetail, decimal? receiptAmount, bool? isBankDocDiff, long? clearingCOAId, bool? isBank)
        {
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentId = payment.Id;
            jmodel.COAId = clearingCOAId;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            //jmodel.ExchangeRate = payment.BaseCurrency == payment.BankPaymentAmmountCurrency ? 1 : (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate);

            jmodel.ExchangeRate = isBank == true ? payment.BaseCurrency == payment.BankPaymentAmmountCurrency ? 1 : (payment.SystemCalculatedExchangeRate != null || payment.SystemCalculatedExchangeRate != 0) ? payment.SystemCalculatedExchangeRate : payment.ExchangeRate : payment.BaseCurrency != payment.DocCurrency ? (payment.SystemCalculatedExchangeRate != null || payment.SystemCalculatedExchangeRate != 0) ? payment.SystemCalculatedExchangeRate : payment.ExchangeRate : 1;
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.DocDate = payment.DocDate;
            jmodel.DocNo = payment.DocNo;
            jmodel.PostingDate = payment.DocDate;
            if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.BillCreditMemo)
            {
                if (payment.DocCurrency != payment.BaseCurrency && payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = jmodel.BaseCredit;

                }
                if (payment.DocCurrency == payment.BaseCurrency && payment.DocCurrency != payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseCredit = receiptAmount;
                    jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.DocCredit = Math.Round((decimal)receiptAmount / (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                //jmodel.DocCredit = receiptAmount;
                //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else if (docType == DocTypeConstants.Bills)
            {
                if (payment.DocCurrency != payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency == payment.BaseCurrency)
                {
                    jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.BaseCredit = Math.Round((decimal)receiptAmount * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = receiptAmount;
                }
                if (payment.DocCurrency != payment.BankPaymentAmmountCurrency && payment.BaseCurrency == payment.DocCurrency)
                {
                    jmodel.DocCredit = receiptAmount;
                    jmodel.BaseCredit = jmodel.DocCredit;
                }
            }
            else
            {
                if (payment.DocCurrency != payment.BaseCurrency && payment.BaseCurrency == payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.BaseDebit = Math.Round((decimal)receiptAmount * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = jmodel.BaseDebit;
                }
                if (payment.DocCurrency == payment.BaseCurrency && payment.DocCurrency != payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseDebit = receiptAmount;
                    jmodel.DocDebit = Math.Round((decimal)receiptAmount / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    //jmodel.DocDebit = Math.Round((decimal)receiptAmount / (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                //jmodel.DocDebit = receiptAmount;
                //jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (receiptAmount < 0)
            {
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
            }
        }

        private void FillJDetailIfCurrencyDiff(JVVDetailModel jmodel, PaymentDetail detail, Payment payment, List<PaymentDetail> lstDetail, decimal? receiptAmount, long? clearingCOAId)
        {
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            if (account1 != null)
                jmodel.COAId = detail.DocumentType == DocTypeConstants.BillCreditMemo || detail.DocumentType == DocTypeConstants.CreditNote ? clearingCOAId : account1.Id;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            jmodel.ExchangeRate = payment.DocCurrency == payment.BankPaymentAmmountCurrency ? 1 : jmodel.ExchangeRate;
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.DocDate = payment.DocDate;
            jmodel.DocNo = payment.DocNo;
            jmodel.PostingDate = payment.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
            {
                jmodel.DocCredit = detail.PaymentAmount;
                jmodel.BaseCredit = detail.DocumentType == DocTypeConstants.BillCreditMemo ? Math.Round((decimal)jmodel.DocCredit * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            if (detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
            {
                jmodel.DocDebit = detail.PaymentAmount;
                jmodel.BaseDebit = /*detail.DocumentType == DocTypeConstants.Bills ?*/ Math.Round((decimal)jmodel.DocDebit * (decimal)(jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) /*: Math.Round((decimal)jmodel.DocDebit * (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero)*/;
            }
            if (receiptAmount < 0)
            {
                jmodel.DocDebit = -jmodel.DocCredit;
                jmodel.BaseDebit = -jmodel.BaseCredit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }

        private void FillJournalOffsetNew(JVModel headJournal, Payment payment, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<PaymentDetail> lstDetail, bool? isBankDocDiff, long? clearingCOAId)
        {
            if (isfirst1)
                doc = payment.DocNo;
            isFirst = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = payment.Id;
            FillHeadJVNew(headJournal, payment, true);
            doc = GetNextApplicationNumber(doc, isfirst1, payment.DocNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == false)
            {
                recorder = 0;

                JVVDetailModel jDetil = new JVVDetailModel();
                FillJDetailNew(jDetil, payment, clearingCOAId);
                jDetil.RecOrder = 1;
                lstJD.Add(jDetil);

                JVVDetailModel jmodel3 = new JVVDetailModel();
                decimal? amt = payment.PaymentDetails.Where(c => c.ServiceCompanyId == payment.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim).Sum(c => c.PaymentAmount) - payment.PaymentDetails.Where(c => c.ServiceCompanyId == payment.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.PaymentAmount);

                FillDetailOffsetDetailNew(jmodel3, DocTypeConstants.General, payment, lstDetail, amt, isBankDocDiff, clearingCOAId, true);
                jmodel3.RecOrder = ++recorder;
                lstJD.Add(jmodel3);


                amt = payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.DocumentType == DocTypeConstants.CreditNote && c.ServiceCompanyId == payment.ServiceCompanyId).Sum(c => c.PaymentAmount);
                if (amt != 0)
                {
                    JVVDetailModel jmodel = new JVVDetailModel();
                    jmodel.Id = Guid.NewGuid();

                    FillDetailOffsetDetailNew(jmodel, DocTypeConstants.CreditNote, payment, lstDetail, amt, isBankDocDiff, clearingCOAId, true);
                    jmodel.DocumentDetailId = jmodel.Id;
                    jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(jmodel);
                }
                amt = payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.DocumentType == DocTypeConstants.BillCreditMemo && c.ServiceCompanyId == payment.ServiceCompanyId).Sum(c => c.PaymentAmount);
                if (amt != 0)
                {
                    JVVDetailModel jmodel = new JVVDetailModel();
                    jmodel.Id = Guid.NewGuid();
                    FillDetailOffsetDetailNew(jmodel, DocTypeConstants.BillCreditMemo, payment, lstDetail, amt, isBankDocDiff, clearingCOAId, true);
                    jmodel.DocumentDetailId = jmodel.Id;
                    jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(jmodel);
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }

        private void FillJournalOffset(JVModel headJournal, Payment payment, bool isNew, bool isBalancing, out int? recorder1, int? recorder, out bool isFirst, bool isfirst1, List<PaymentDetail> lstDetail, bool? isBankDocDiff, long? clearingCOAId)
        {
            if (isfirst1)
                doc = payment.DocNo;
            isFirst = true;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = payment.Id;
            FillHeadJVNew(headJournal, payment, isBalancing);
            doc = GetNextApplicationNumber(doc, isfirst1, payment.DocNo);
            headJournal.SystemReferenceNo = doc;
            isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            if (isBalancing == false)
            {
                recorder = 0;
                JVVDetailModel jmodel3 = new JVVDetailModel();
                decimal? amt = payment.PaymentDetails.Where(c => c.ServiceCompanyId == payment.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.PaymentAmount) - payment.PaymentDetails.Where(c => c.ServiceCompanyId == payment.ServiceCompanyId && c.DocumentType == DocTypeConstants.Bills).Sum(c => c.PaymentAmount);
                FillClearingReciptOffset2(jmodel3, payment, amt, headJournal.ExchangeRate, clearingCOAId);
                jmodel3.RecOrder = ++recorder;
                lstJD.Add(jmodel3);
                foreach (PaymentDetail detail in payment.PaymentDetails.Where(a => a.PaymentAmount != 0 && (a.DocumentType == DocTypeConstants.Invoice || a.DocumentType == DocTypeConstants.Bills || a.DocumentType == DocTypeConstants.DebitNote)))
                {
                    JVVDetailModel jmodel = new JVVDetailModel();
                    if (isNew)
                        jmodel.Id = Guid.NewGuid();
                    else
                        jmodel.Id = detail.Id;
                    FillDetailOffset1(jmodel, detail, payment, lstDetail, detail.PaymentAmount);
                    jmodel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(jmodel);
                    if (payment.DocCurrency != payment.BaseCurrency)
                    {
                        JVVDetailModel jmodel2 = new JVVDetailModel();
                        if (isNew)
                            jmodel2.Id = Guid.NewGuid();
                        else
                            jmodel2.Id = detail.Id;
                        FillGstDetail(jmodel2, detail, payment, jmodel);
                        jmodel2.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        if ((jmodel2.BaseCredit != 0 && jmodel2.BaseCredit != null) || (jmodel2.BaseDebit != 0 && jmodel2.BaseDebit != null))
                            lstJD.Add(jmodel2);
                    }
                }
            }
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
            recorder1 = recorder;
        }

        private void FillClearingReciptOffset2(JVVDetailModel jmodel1, Payment payment, decimal? amt, decimal? exchangeRate, long? clearingCOAId)
        {
            jmodel1.DocType = payment.DocSubType;
            jmodel1.DocumentDetailId = payment.Id;
            jmodel1.DocumentId = payment.Id;
            jmodel1.DocType = DocTypeConstants.BillPayment;
            jmodel1.DocSubType = DocTypeConstants.General;
            jmodel1.SystemRefNo = payment.SystemRefNo;
            jmodel1.DocNo = payment.DocNo;
            jmodel1.PostingDate = payment.DocDate;
            jmodel1.EntityId = payment.EntityId;
            jmodel1.EntityType = payment.EntityType;
            jmodel1.BaseCurrency = payment.ExCurrency;
            //jmodel1.DocCurrency = payment.BankPaymentAmmountCurrency;
            jmodel1.ServiceCompanyId = payment.ServiceCompanyId;
            jmodel1.AccountDescription = payment.Remarks;
            jmodel1.DocDate = payment.DocDate;
            if (payment.DocCurrency != payment.BaseCurrency)
                jmodel1.ExchangeRate = exchangeRate;
            else
                jmodel1.ExchangeRate = 1;
            jmodel1.COAId = clearingCOAId;
            jmodel1.DocCurrency = payment.DocCurrency;
            jmodel1.DocDebit = amt;
            jmodel1.BaseDebit = Math.Round((decimal)jmodel1.DocDebit * (jmodel1.ExchangeRate == null ? 1 : jmodel1.ExchangeRate).Value, 2, MidpointRounding.AwayFromZero);
            jmodel1.IsTax = false;
        }

        private void FillDetailOffset1(JVVDetailModel jmodel, PaymentDetail detail, Payment payment, List<PaymentDetail> lstDetail, decimal? receiptAmount)
        {
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = DocTypeConstants.General;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.Nature = detail.Nature;
            jmodel.DocCurrency = payment.DocCurrency;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote || detail.DocumentType == DocTypeConstants.DebitNote)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            if (account1 != null)
                jmodel.COAId = account1.Id;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            jmodel.ExchangeRate = payment.BaseCurrency == payment.DocCurrency ? 1 : jmodel.ExchangeRate;
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.DocDate = payment.DocDate;
            jmodel.DocNo = payment.DocNo;
            jmodel.PostingDate = payment.DocDate;
            jmodel.OffsetDocument = detail.SystemReferenceNumber;

            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.DocCredit = receiptAmount;
                jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)(payment.DocCurrency == payment.BaseCurrency ? 1 : jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
            else
            {
                jmodel.DocDebit = receiptAmount;
                jmodel.BaseDebit = Math.Round((decimal)jmodel.DocDebit * (decimal)(payment.DocCurrency == payment.BaseCurrency ? 1 : jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            }
        }



        private void FillJDetailNew(JVVDetailModel jModel, Payment payment, long? clearingPaymentCOAId)
        {
            jModel.COAId = payment.COAId;
            jModel.DocumentId = payment.Id;
            jModel.SystemReferenceNo = payment.SystemRefNo;
            jModel.SystemRefNo = payment.SystemRefNo;
            jModel.DocNo = payment.DocNo;
            jModel.PostingDate = payment.DocDate;
            jModel.ServiceCompanyId = payment.ServiceCompanyId;
            jModel.DocType = payment.DocType;
            jModel.DocSubType = payment.DocSubType;
            jModel.EntityId = payment.EntityId;
            jModel.DocCurrency = payment.DocCurrency;
            jModel.BaseCurrency = payment.BaseCurrency;
            //jModel.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jModel.ExchangeRate = payment.BankPaymentAmmountCurrency == payment.BaseCurrency ? 1.0000000000m : (payment.SystemCalculatedExchangeRate != null && payment.SystemCalculatedExchangeRate != 0) ? payment.SystemCalculatedExchangeRate : payment.ExchangeRate;
            jModel.GSTExCurrency = payment.GSTExCurrency;
            jModel.GSTExchangeRate = payment.GSTExchangeRate;
            jModel.DocDate = payment.DocDate;
            jModel.DocDescription = payment.Remarks;
            jModel.AccountDescription = payment.Remarks;
            jModel.DocCredit = payment.GrandTotal;
            jModel.COAId = /*(payment.PaymentApplicationCurrency == payment.BankPaymentAmmountCurrency) ?*/ payment.COAId /*: clearingPaymentCOAId*/;
            //jModel.BaseCredit = Math.Round((decimal)(jModel.DocCredit * ((payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);//commented by lokanath
            jModel.BaseCredit = Math.Round((decimal)(jModel.DocCredit * jModel.ExchangeRate), 2, MidpointRounding.AwayFromZero);

        }


        //IC Posting
        private void FillICPayemntOffsetJournal(JVModel headJournal, Payment _payment, bool isNew, bool isBalancing, PaymentDetail detail, string shotCode, bool? isBankDocDiff, long? clearingCOAId)
        {
            //if (isfirst1)
            //    doc = payment.SystemRefNo;
            //isFirst = true;
            bool? isInterCompany = true;
            //string strServiceCompany = _companyService.GetById(payment.ServiceCompanyId).ShortName;
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = _payment.Id;
            FillHeadJVNew(headJournal, _payment, isBalancing);
            doc = GetNextApplicationNumber(doc, false, _payment.SystemRefNo);
            headJournal.SystemReferenceNo = doc;
            headJournal.ServiceCompanyId = detail.ServiceCompanyId;
            headJournal.IsGstSettings = false;
            //isFirst = false;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            FillICOffsetPaymentDetail(_payment, detail, isNew, lstJD/*, subCompanyId*/, isInterCompany, shotCode, isBankDocDiff, clearingCOAId);
            headJournal.DocCurrency = detail.Currency;
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();

        }

        private void FillICOffSetJournalDetail(PaymentDetail detail, JVVDetailModel jmodel, Payment payment, bool? isInterCompany, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long? clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = payment.DocDate;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.DocNo = payment.DocNo;
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (isCustomer == true)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            //if (account1 != null)
            //    jmodel.COAId = account1.Id;
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.AccountDescription = payment.Remarks;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            //jmodel.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel.DocDate = payment.DocDate;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);

            if (isInterCompany == true)
            {
                if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
                else
                {
                    jmodel.DocDebit = recAmt;
                    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
            }
            else
            {
                if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.Invoice)
                {
                    jmodel.DocDebit = recAmt;
                    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
                else
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isBankDocDiff == true ? clearingCOAId : account1.Id;
                }
            }
            if (recAmt < 0)
            {
                jmodel.DocCredit = -jmodel.DocDebit;
                jmodel.BaseCredit = -jmodel.BaseDebit;
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
            }
        }

        private void FillICOffSetJournalDetailNew(PaymentDetail detail, JVVDetailModel jmodel, Payment payment, bool? isInterCompany, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long? clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = payment.DocDate;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.DocNo = payment.DocNo;
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (isCustomer == true)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.AccountDescription = payment.Remarks;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            jmodel.DocDate = payment.DocDate;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            if (isInterCompany == true && isCustomer == null && recAmt < 0)
            {
                jmodel.DocDebit = -recAmt;
                //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.BaseDebit = payment.BaseCurrency != payment.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
            }
            if (isInterCompany == true && isCustomer == null && recAmt > 0)
            {
                jmodel.DocCredit = recAmt;
                //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.BaseCredit = payment.BaseCurrency != payment.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocCredit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit, 2, MidpointRounding.AwayFromZero);
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
            }
            if (isInterCompany == false && isCustomer == true)
            {
                jmodel.DocCredit = recAmt;
                jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = account1.Id;
            }
            if (isInterCompany == false && isCustomer == false)
            {
                jmodel.DocDebit = recAmt;
                jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = account1.Id;
            }
            //if (recAmt < 0)
            //{
            //    jmodel.DocCredit = -jmodel.DocDebit;
            //    jmodel.BaseCredit = -jmodel.BaseDebit;
            //    jmodel.DocDebit = null;
            //    jmodel.BaseDebit = null;
            //}
        }

        private void FillICOffsetPaymentDetail(Payment _payment, PaymentDetail detail, bool isNew, List<JVVDetailModel> lstJD/*, long? serviceCompanyId*/, bool? isInterCompany, string shotCode, bool? isBankDocDiff, long? clearingCOAId)
        {
            int? recOrder = 0;
            bool? isCustomer = false;
            decimal amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            amt = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim)).Sum(c => c.PaymentAmount) - _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote /*|| c.DocumentType == DocTypeConstants.BillCreditMemo*/)).Sum(c => c.PaymentAmount);
            if (amt != 0)
            {
                if (isNew)
                    journalHead.Id = Guid.NewGuid();
                else
                    journalHead.Id = detail.Id;
                isCustomer = null;
                FillICOffSetJournalDetailNew(detail, journalHead, _payment, true, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journalHead.RecOrder = recOrder + 1;
                recOrder = journalHead.RecOrder;
                lstJD.Add(journalHead);
            }

            var res1 = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote && c.PaymentAmount != 0)).ToList();
            foreach (PaymentDetail pDetail in res1)
            {
                amt = pDetail.PaymentAmount;
                if (amt != 0)
                {
                    JVVDetailModel journal2 = new JVVDetailModel();
                    if (isNew)
                        journal2.Id = Guid.NewGuid();
                    else
                        journal2.Id = detail.Id;
                    isCustomer = true;
                    //detail = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim)).FirstOrDefault();
                    FillICOffSetJournalDetailNew(pDetail, journal2, _payment, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                    journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                    lstJD.Add(journal2);
                    if (_payment.DocCurrency != _payment.BaseCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                        journalHead.DocCurrency = detail.Currency;
                        journalHead.ExchangeRate = journal2.ExchangeRate;
                        FillExchangeGainLossItemOffsetValue(_payment, journal1, amt, journalHead, detail.DocumentType);
                        _payment.ExchangeRate = (_payment.SystemCalculatedExchangeRate == null || _payment.SystemCalculatedExchangeRate == 0) ? _payment.ExchangeRate : _payment.SystemCalculatedExchangeRate;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //recOrder = journal1.RecOrder;
                        if (_payment.ExchangeRate != journal1.ExchangeRate)
                            lstJD.Add(journal1);
                    }
                }
            }
            var res = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim && c.PaymentAmount != 0))/*.GroupBy(c => c.ServiceCompanyId).Select(c => new { Detail = c.ToList() })*/.ToList();
            foreach (PaymentDetail pDetail in res)
            {
                amt = pDetail.PaymentAmount;
                if (amt != 0)
                {
                    JVVDetailModel journal2 = new JVVDetailModel();
                    if (isNew)
                        journal2.Id = Guid.NewGuid();
                    else
                        journal2.Id = detail.Id;
                    isCustomer = false;
                    //detail = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim)).FirstOrDefault();
                    FillICOffSetJournalDetailNew(pDetail, journal2, _payment, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                    journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                    lstJD.Add(journal2);
                    if (_payment.DocCurrency != _payment.BaseCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                        journalHead.DocCurrency = detail.Currency;
                        journalHead.ExchangeRate = journal2.ExchangeRate;
                        FillExchangeGainLossItemOffsetValue(_payment, journal1, amt, journalHead, detail.DocumentType);
                        _payment.ExchangeRate = (_payment.SystemCalculatedExchangeRate == null || _payment.SystemCalculatedExchangeRate == 0) ? _payment.ExchangeRate : _payment.SystemCalculatedExchangeRate;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //recOrder = journal1.RecOrder;
                        if (_payment.ExchangeRate != journal1.ExchangeRate)
                            lstJD.Add(journal1);
                    }
                }
            }


        }

        private void FillExchangeGainLossItemOffsetValue(Payment payment, JVVDetailModel journal1, decimal amount, JVVDetailModel journalDetailData, string docType)
        {
            var originalExchangeRate1 = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            journal1.DocumentId = payment.Id;
            journal1.PostingDate = payment.DocDate;
            journal1.SystemRefNo = payment.SystemRefNo;
            journal1.ServiceCompanyId = payment.ServiceCompanyId;
            journal1.AccountDescription = payment.Remarks;
            journal1.DocDate = payment.DocDate.Date;
            journal1.DocType = payment.DocType;
            journal1.DocSubType = payment.DocSubType;
            journal1.DocNo = payment.DocNo;
            if (journalDetailData != null)
            {
                journal1.Nature = journalDetailData.Nature;
                journal1.EntityId = journalDetailData.EntityId;
                journal1.OffsetDocument = journalDetailData.SystemRefNo;
                journal1.ExchangeRate = journalDetailData.ExchangeRate;
            }
            AppsWorld.CommonModule.Entities.ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == payment.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
            {
                journal1.COAId = account2.Id;
            }
            journal1.BaseCurrency = payment.BaseCurrency;
            if (docType == DocTypeConstants.Invoice || docType == DocTypeConstants.DebitNote || docType == DocTypeConstants.BillCreditMemo)
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseCredit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * amount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * amount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            else
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseDebit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * amount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * amount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
        }



        private void FillICPaymentOffsetJournal(JVModel headJournal, Payment payment, bool isNew, bool isBalancing, PaymentDetail detail, string shotCode, bool? isBankDocDiff, long? clearingCOAId)
        {
            //if (isfirst1)
            //    doc = payment.SystemRefNo;
            //isFirst = true;
            decimal? baseDebit;
            bool? isInterCompany = true;
            if (payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote || c.DocumentType == DocTypeConstants.Bill || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim)).Any())
            {
                if (isNew)
                    headJournal.Id = Guid.NewGuid();
                else
                    headJournal.Id = payment.Id;
                FillHeadJVNew(headJournal, payment, isBalancing);
                doc = GetNextApplicationNumber(doc, false, payment.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                headJournal.IsGstSettings = false;
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                FillICOffsetPaymentDetail1(payment, detail, isNew, lstJD, isInterCompany, shotCode, isBankDocDiff, clearingCOAId);
                headJournal.DocCurrency = detail.Currency;
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                baseDebit = headJournal.JVVDetailModels.Where(c => c.COAId == clearingCOAId).Select(c => c.BaseDebit).FirstOrDefault() == null ? headJournal.JVVDetailModels.Where(c => c.COAId == clearingCOAId).Select(c => c.BaseCredit).FirstOrDefault() : headJournal.JVVDetailModels.Where(c => c.COAId == clearingCOAId).Select(c => c.BaseDebit).FirstOrDefault();
                SaveInvoice1(headJournal);
                if (detail.ServiceCompanyId != payment.ServiceCompanyId)
                {
                    headJournal = new JVModel();
                    if (isNew)
                        headJournal.Id = Guid.NewGuid();
                    else
                        headJournal.Id = payment.Id;
                    FillHeadJVNew(headJournal, payment, isBalancing);
                    doc = GetNextApplicationNumber(doc, false, payment.SystemRefNo);
                    headJournal.SystemReferenceNo = doc;
                    headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                    headJournal.IsGstSettings = false;
                    lstJD = new List<JVVDetailModel>();


                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                        baseDebit = payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bill || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim)).Sum(c => c.PaymentAmount) - payment.PaymentDetails.Where(c => c.PaymentAmount != 0 && c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote)).Sum(c => c.PaymentAmount);
                    if (baseDebit < 0)
                        baseDebit = -baseDebit;
                    FillICOffsetClearingDataNew(payment, detail, lstJD, shotCode, isBankDocDiff, clearingCOAId, baseDebit);
                    //headJournal.DocCurrency = detail.Currency;
                    headJournal.DocCurrency = payment.BankPaymentAmmountCurrency != payment.DocCurrency ? payment.BankPaymentAmmountCurrency : detail.Currency;
                    headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                    SaveInvoice1(headJournal);
                }
            }
            //List<Invoice> lstCreditNote = _invoiceService.GetListOfInvoices(_receipt.CompanyId, _receipt.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.CreditNote).Select(c => c.DocumentId).ToList());
            //if (detail.DocumentType == DocTypeConstants.CreditNote)
            foreach (PaymentDetail paymentDetail in payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.CreditNote))
            {
                baseDebit = 0;
                //InvoiceCompact creditNote = lstCreditNote.Where(c => c.Id == receiptDetail.DocumentId).FirstOrDefault();
                //if (creditNote != null)
                //{
                baseDebit = paymentDetail.PaymentAmount;//_receipt.BankChargesCurrency == _receipt.BaseCurrency ? Math.Round(receiptDetail.ReceiptAmount * (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round(receiptDetail.ReceiptAmount / (decimal)(_receipt.DocCurrency == _receipt.BankChargesCurrency ? _receipt.ExchangeRate : _receipt.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                                                        //}
                headJournal = new JVModel();
                if (isNew)
                    headJournal.Id = Guid.NewGuid();
                else
                    headJournal.Id = payment.Id;
                FillHeadJVNew(headJournal, payment, isBalancing);
                doc = GetNextApplicationNumber(doc, false, payment.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                headJournal.IsGstSettings = false;
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                FillICOffsetClearingData(payment, paymentDetail, lstJD, shotCode, isBankDocDiff, clearingCOAId, baseDebit);
                //headJournal.DocCurrency = detail.Currency;
                headJournal.DocCurrency = payment.BankPaymentAmmountCurrency != payment.DocCurrency ? payment.BankPaymentAmmountCurrency : detail.Currency;
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                SaveInvoice1(headJournal);
            }
            //List<CreditMemoCompact> lstCreditMemo = _debitNoteService.GetAllCMByDocId(_receipt.ReceiptDetails.Where(c => c.DocumentType == DocTypeConstants.BillCreditMemo).Select(c => c.DocumentId).ToList(), _receipt.CompanyId);
            //if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
            foreach (PaymentDetail paymentDetail in payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && c.DocumentType == DocTypeConstants.BillCreditMemo))
            {
                baseDebit = 0;
                baseDebit = paymentDetail.PaymentAmount;
                headJournal = new JVModel();
                if (isNew)
                    headJournal.Id = Guid.NewGuid();
                else
                    headJournal.Id = payment.Id;
                FillHeadJVNew(headJournal, payment, isBalancing);
                //FillJV(headJournal, _receipt, true);
                doc = GetNextApplicationNumber(doc, false, payment.SystemRefNo);
                headJournal.SystemReferenceNo = doc;
                headJournal.ServiceCompanyId = detail.ServiceCompanyId;
                headJournal.IsGstSettings = false;
                List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
                FillICOffsetClearingDataNew(payment, paymentDetail, lstJD, shotCode, isBankDocDiff, clearingCOAId, baseDebit);
                //headJournal.DocCurrency = detail.Currency;//commented by lokanath
                headJournal.DocCurrency = payment.BankPaymentAmmountCurrency != payment.DocCurrency ? payment.BankPaymentAmmountCurrency : detail.Currency;
                headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0 || a.BaseCredit > 0 || a.BaseDebit > 0).OrderBy(c => c.RecOrder).ToList();
                SaveInvoice1(headJournal);
            }
        }
        private void FillICOffsetClearingData(Payment payment, PaymentDetail detail, List<JVVDetailModel> lstJD, string shotCode, bool? isBankDocDiff, long? clearingCOAId, decimal? amt)
        {
            int? recOrder = 0;
            //decimal? amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            //amt = payment.BankChargesCurrency == payment.BaseCurrency ? Math.Round((decimal)amt * (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            journalHead.Id = Guid.NewGuid();
            FillICOffsetPaymentOutstandingClearing(detail, journalHead, payment, shotCode, amt, true, isBankDocDiff, clearingCOAId);
            journalHead.RecOrder = recOrder + 1;
            recOrder = journalHead.RecOrder;
            lstJD.Add(journalHead);
            JVVDetailModel journal = new JVVDetailModel();
            journal.Id = Guid.NewGuid();
            FillICOffsetPaymentOutstandingClearing(detail, journal, payment, shotCode, amt, false, isBankDocDiff, clearingCOAId);
            journal.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
            lstJD.Add(journal);

        }
        private void FillICOffsetClearingDataNew(Payment payment, PaymentDetail detail, List<JVVDetailModel> lstJD, string shotCode, bool? isBankDocDiff, long? clearingCOAId, decimal? amt)
        {
            int? recOrder = 0;
            //decimal? amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            //amt = payment.BankChargesCurrency == payment.BaseCurrency ? Math.Round((decimal)amt * (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)amt / (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            journalHead.Id = Guid.NewGuid();
            FillICOffsetPaymentOutstandingClearingNew(detail, journalHead, payment, shotCode, amt, true, isBankDocDiff, clearingCOAId);
            journalHead.RecOrder = recOrder + 1;
            recOrder = journalHead.RecOrder;
            lstJD.Add(journalHead);
            JVVDetailModel journal = new JVVDetailModel();
            journal.Id = Guid.NewGuid();
            FillICOffsetPaymentOutstandingClearingNew(detail, journal, payment, shotCode, amt, false, isBankDocDiff, clearingCOAId);
            journal.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
            lstJD.Add(journal);

        }
        private void FillICOffsetPaymentOutstandingClearing(PaymentDetail detail, JVVDetailModel jmodel, Payment payment, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long? clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = payment.DocDate;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.DocNo = payment.DocNo;
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.ExchangeRate = payment.BankPaymentAmmountCurrency == payment.BaseCurrency ? 1 : payment.ExchangeRate;
            jmodel.DocDate = payment.DocDate;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);



            //if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
            //{
            //    jmodel.DocCredit = recAmt;
            //    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            //    jmodel.DocDebit = null;
            //    jmodel.BaseDebit = null;
            //    jmodel.OffsetDocument = detail.SystemReferenceNumber;
            //    if (account1 != null)
            //        jmodel.COAId = isBankDocDiff == true ? account1.Id : chartOfAccount.Id;
            //}
            //else
            //{
            //    jmodel.DocDebit = recAmt;
            //    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            //    jmodel.DocCredit = null;
            //    jmodel.BaseCredit = null;
            //    jmodel.OffsetDocument = detail.SystemReferenceNumber;
            //    if (account1 != null)
            //        jmodel.COAId = isBankDocDiff == true ? account1.Id : chartOfAccount.Id;
            //}


            if (isCustomer == true)
            {
                if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseCredit = recAmt;
                    jmodel.DocCredit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                {
                    jmodel.BaseCredit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = jmodel.BaseCredit;
                }
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (chartOfAccount != null)
                    jmodel.COAId = chartOfAccount.Id;
            }
            else
            {
                if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
                {
                    jmodel.BaseDebit = recAmt;
                    jmodel.DocDebit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = recAmt;
                    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                {
                    jmodel.BaseDebit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = jmodel.BaseDebit;
                }
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                jmodel.COAId = clearingCOAId;
            }

            //else
            //{
            //    if (detail.DocumentType != DocTypeConstants.CreditNote)
            //    {
            //        if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
            //        {
            //            //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * (invoice.DocCurrency == invoice.BankChargesCurrency ? invoice.ExchangeRate : invoice.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
            //            jmodel.BaseDebit = recAmt;
            //            jmodel.DocDebit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            //        }
            //        else
            //        {
            //            jmodel.DocDebit = recAmt;
            //            jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            //        }
            //        if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
            //        {
            //            jmodel.BaseDebit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
            //            jmodel.DocDebit = jmodel.BaseDebit;
            //        }
            //        jmodel.DocCredit = null;
            //        jmodel.BaseCredit = null;
            //        if (chartOfAccount != null)
            //            jmodel.COAId = chartOfAccount.Id;
            //    }
            //    else
            //    {
            //        if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
            //        {
            //            jmodel.BaseCredit = recAmt;
            //            jmodel.DocCredit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
            //        }
            //        else
            //        {
            //            //jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
            //            jmodel.DocCredit = recAmt;
            //            jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
            //        }
            //        if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
            //        {
            //            jmodel.BaseCredit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
            //            jmodel.DocCredit = jmodel.BaseCredit;
            //        }
            //        jmodel.DocDebit = null;
            //        jmodel.BaseDebit = null;
            //        if (chartOfAccount != null)
            //            jmodel.COAId = chartOfAccount.Id;
            //    }
            //}
        }
        private void FillICOffsetPaymentOutstandingClearingNew(PaymentDetail detail, JVVDetailModel jmodel, Payment payment, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long? clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = payment.DocDate;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.DocNo = payment.DocNo;
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == invoice.ServiceCompanyId*/).Select().FirstOrDefault();
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.AccountDescription = payment.Remarks;
            jmodel.ExchangeRate = payment.BankPaymentAmmountCurrency == payment.BaseCurrency ? 1 : payment.ExchangeRate;
            jmodel.DocDate = payment.DocDate;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            if (isCustomer == true)
            {
                if (detail.DocumentType == DocTypeConstants.Bill || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.CreditNote)
                {
                    if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
                    {
                        jmodel.BaseCredit = recAmt;
                        jmodel.DocCredit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        jmodel.DocCredit = recAmt;
                        jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                    {
                        jmodel.BaseCredit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocCredit = jmodel.BaseCredit;
                    }
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (chartOfAccount != null)
                        jmodel.COAId = chartOfAccount.Id;
                }
                if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
                {
                    if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
                    {
                        jmodel.BaseDebit = recAmt;
                        jmodel.DocDebit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = recAmt;
                        jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    }
                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                    {
                        jmodel.BaseDebit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = jmodel.BaseDebit;
                    }
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (chartOfAccount != null)
                        jmodel.COAId = chartOfAccount.Id;
                }
            }
            else
            {
                if (detail.DocumentType == DocTypeConstants.Bill || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.CreditNote)
                {
                    if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
                    {
                        jmodel.BaseDebit = recAmt;
                        jmodel.DocDebit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankChargesCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = recAmt;
                        jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    }
                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                    {
                        jmodel.BaseDebit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankChargesCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocDebit = jmodel.BaseDebit;
                    }
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    jmodel.COAId = clearingCOAId;
                }
                if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
                {
                    if (payment.DocCurrency == payment.BaseCurrency && payment.BaseCurrency != payment.BankPaymentAmmountCurrency)
                    {
                        jmodel.BaseCredit = recAmt;
                        jmodel.DocCredit = Math.Round((decimal)recAmt / (decimal)(payment.DocCurrency == payment.BankPaymentAmmountCurrency ? jmodel.ExchangeRate : payment.SystemCalculatedExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        jmodel.DocCredit = recAmt;
                        jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    }
                    if (payment.BaseCurrency == payment.BankPaymentAmmountCurrency && payment.BankPaymentAmmountCurrency != payment.DocCurrency)
                    {
                        jmodel.BaseCredit = Math.Round((decimal)(recAmt * (payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate)), 2, MidpointRounding.AwayFromZero);
                        jmodel.DocCredit = jmodel.BaseCredit;
                    }
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    jmodel.COAId = clearingCOAId;
                }
            }
        }
        private void FillICOffsetPaymentDetail1(Payment payment, PaymentDetail detail, bool isNew, List<JVVDetailModel> lstJD/*, long? serviceCompanyId*/, bool? isInterCompany, string shotCode, bool? isBankDocDiff, long? clearingCOAId)
        {
            int? recOrder = 0;
            bool? isCustomer = false;
            decimal? amt = 0;
            JVVDetailModel journalHead = new JVVDetailModel();
            amt = payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bill || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.Claim) && c.PaymentAmount != 0).Sum(c => c.PaymentAmount) - payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote) && c.PaymentAmount != 0).Sum(c => c.PaymentAmount);

            if (amt != 0)
            {
                if (isNew)
                    journalHead.Id = Guid.NewGuid();
                else
                    journalHead.Id = detail.Id;
                isCustomer = null;
                FillICOffsetReceiptOutstandingDetailNew(detail, journalHead, payment, true, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                journalHead.RecOrder = ++recOrder;
                lstJD.Add(journalHead);
            }
            var res1 = payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Invoice || c.DocumentType == DocTypeConstants.DebitNote && c.PaymentAmount != 0)).ToList();
            foreach (PaymentDetail pDetail in res1)
            {
                amt = pDetail.PaymentAmount;
                if (amt != 0)
                {
                    JVVDetailModel journal2 = new JVVDetailModel();
                    if (isNew)
                        journal2.Id = Guid.NewGuid();
                    else
                        journal2.Id = detail.Id;
                    isCustomer = true;

                    //FillICOffSetJournalDetail(pDetail, journal2, payment, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                    FillICOffsetReceiptOutstandingDetailNew(pDetail, journal2, payment, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                    journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                    lstJD.Add(journal2);
                    if (payment.DocCurrency != payment.BaseCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                        journalHead.DocCurrency = detail.Currency;
                        journalHead.ExchangeRate = journal2.ExchangeRate;
                        FillExchangeGainLossItemOffsetValue(payment, journal1, amt.Value, journalHead, detail.DocumentType);
                        payment.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //recOrder = journal1.RecOrder;
                        if (payment.ExchangeRate != journal1.ExchangeRate)
                            lstJD.Add(journal1);
                    }
                }
            }
            var res = payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim && c.PaymentAmount != 0))/*.GroupBy(c => c.ServiceCompanyId).Select(c => new { Detail = c.ToList() })*/.ToList();
            foreach (PaymentDetail pDetail in res)
            {
                amt = pDetail.PaymentAmount;
                if (amt != 0)
                {
                    JVVDetailModel journal2 = new JVVDetailModel();
                    if (isNew)
                        journal2.Id = Guid.NewGuid();
                    else
                        journal2.Id = detail.Id;
                    isCustomer = false;
                    //detail = _payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim)).FirstOrDefault();
                    FillICOffSetJournalDetail(pDetail, journal2, payment, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
                    journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
                    lstJD.Add(journal2);
                    if (payment.DocCurrency != payment.BaseCurrency)
                    {
                        JVVDetailModel journal1 = new JVVDetailModel();
                        if (isNew)
                            journal1.Id = Guid.NewGuid();
                        else
                            journal1.Id = detail.Id;
                        //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
                        journalHead.DocCurrency = detail.Currency;
                        journalHead.ExchangeRate = journal2.ExchangeRate;
                        FillExchangeGainLossItemOffsetValue(payment, journal1, amt.Value, journalHead, detail.DocumentType);
                        payment.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
                        journal1.DocumentDetailId = detail.Id;
                        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                        //recOrder = journal1.RecOrder;
                        if (payment.ExchangeRate != journal1.ExchangeRate)
                            lstJD.Add(journal1);
                    }
                }
            }

            //amt = payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim)).Sum(c => c.PaymentAmount);
            //detail = payment.PaymentDetails.Where(c => c.ServiceCompanyId == detail.ServiceCompanyId && (c.DocumentType == DocTypeConstants.Bills || c.DocumentType == DocTypeConstants.OpeningBalance || c.DocumentType == DocTypeConstants.General || c.DocumentType == DocTypeConstants.Claim)).FirstOrDefault();
            //if (amt != 0)
            //{
            //    JVVDetailModel journal2 = new JVVDetailModel();
            //    if (isNew)
            //        journal2.Id = Guid.NewGuid();
            //    else
            //        journal2.Id = detail.Id;
            //    isCustomer = false;
            //    FillICOffsetReceiptOutstandingDetail1(detail, journal2, payment, false, shotCode, amt, isCustomer, isBankDocDiff, clearingCOAId);
            //    journal2.RecOrder = lstJD.Max(x => x.RecOrder) + 1;
            //    lstJD.Add(journal2);
            //    if (payment.DocCurrency != payment.BaseCurrency)
            //    {
            //        JVVDetailModel journal1 = new JVVDetailModel();
            //        if (isNew)
            //            journal1.Id = Guid.NewGuid();
            //        else
            //            journal1.Id = detail.Id;
            //        //payment.ServiceCompanyId = detail.ServiceCompanyId.Value;
            //        journalHead.DocCurrency = detail.Currency;
            //        journalHead.ExchangeRate = journal2.ExchangeRate;
            //        FillExchangeGainLossItemOffset(payment, journal1, detail, journalHead);
            //        payment.ExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            //        journal1.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            //        //recOrder = journal1.RecOrder;
            //        if (payment.ExchangeRate != journal1.ExchangeRate)
            //            lstJD.Add(journal1);
            //    }
            //}
        }


        private void FillICOffsetReceiptOutstandingDetail1(PaymentDetail detail, JVVDetailModel jmodel, Payment payment, bool? isInterCompany, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long? clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = payment.DocDate;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.DocNo = payment.DocNo;
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (isCustomer == true)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId /*&& a.SubsidaryCompanyId == payment.ServiceCompanyId*/).Select().FirstOrDefault();
            //if (account1 != null)
            //    jmodel.COAId = account1.Id;
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.AccountDescription = payment.Remarks;

            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            //jmodel.DocDate = detail.DocumentDate != null ? detail.DocumentDate.Date : (DateTime?)null;
            jmodel.DocDate = payment.DocDate;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            //jmodel.DocCredit = detail.ReceiptAmount;
            //jmodel.BaseCredit = Math.Round((decimal)jmodel.DocCredit * (decimal)jmodel.ExchangeRate, 2, MidpointRounding.AwayFromZero);
            if (isInterCompany == true)
            {
                if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim)
                {
                    jmodel.DocCredit = recAmt;
                    //jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.BaseCredit = isCustomer == false ? Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : payment.BaseCurrency != payment.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocCredit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
                else
                {
                    jmodel.DocDebit = recAmt;
                    //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.BaseDebit = isCustomer == false ? Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero) : payment.BaseCurrency != payment.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
                }
            }
            else
            {
                if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.General || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.Claim/* || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.Invoice*/)
                {
                    jmodel.DocDebit = recAmt;
                    jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocCredit = null;
                    jmodel.BaseCredit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = account1.Id;
                }
                else
                {
                    jmodel.DocCredit = recAmt;
                    jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                    jmodel.DocDebit = null;
                    jmodel.BaseDebit = null;
                    jmodel.OffsetDocument = detail.SystemReferenceNumber;
                    if (account1 != null)
                        jmodel.COAId = account1.Id;
                }
            }
            if (recAmt < 0)
            {
                jmodel.DocDebit = -jmodel.DocDebit;
                jmodel.BaseDebit = -jmodel.BaseDebit;
                jmodel.DocCredit = null;
                jmodel.BaseCredit = null;
            }
        }

        private void FillICOffsetReceiptOutstandingDetailNew(PaymentDetail detail, JVVDetailModel jmodel, Payment payment, bool? isInterCompany, string shotCode, decimal? recAmt, bool? isCustomer, bool? isBankDocDiff, long? clearingCOAId)
        {
            shotCode = "I/C" + " - " + shotCode;
            var originalExchangeRate = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            jmodel.SystemRefNo = payment.SystemRefNo;
            jmodel.DocumentDetailId = detail.Id;
            jmodel.PostingDate = payment.DocDate;
            jmodel.DocumentId = detail.PaymentId;
            jmodel.DocNo = payment.DocNo;
            jmodel.DocType = payment.DocType;
            jmodel.DocSubType = payment.DocSubType;
            jmodel.Nature = detail.Nature;
            AppsWorld.CommonModule.Entities.ChartOfAccount account1 = null;
            if (isCustomer == true)
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables, payment.CompanyId);
            else
                account1 = _chartOfAccountService.GetByName(detail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId);
            AppsWorld.CommonModule.Entities.ChartOfAccount chartOfAccount = _chartOfAccountService.Query(a => a.Name == shotCode
                     && a.CompanyId == payment.CompanyId).Select().FirstOrDefault();
            jmodel.EntityId = payment.EntityId;
            jmodel.BaseCurrency = payment.BaseCurrency;
            jmodel.AccountDescription = payment.Remarks;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.CreditNote)
            {
                jmodel.ExchangeRate = _invoiceCompactService.GetInvoiceAndCNByDocId(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.DebitNote)
            {
                jmodel.ExchangeRate = _debitNoteCompactService.GetDebitNoteCompact(payment.CompanyId, detail.DocumentId);
                //jmodel.ExchangeRate = inv.ExchangeRate;
            }
            if (detail.DocumentType == DocTypeConstants.Bills || detail.DocumentType == DocTypeConstants.Claim || detail.DocumentType == DocTypeConstants.OpeningBalance || detail.DocumentType == DocTypeConstants.General)
                jmodel.ExchangeRate = _billService.GetExchangerateByDocId(payment.CompanyId, detail.DocumentId);
            if (detail.DocumentType == DocTypeConstants.BillCreditMemo)
                jmodel.ExchangeRate = _creditMemoCompactService.GetExchangeRateByDocId(payment.CompanyId, detail.DocumentId);
            jmodel.DocDate = payment.DocDate;
            jmodel.ServiceCompanyId = payment.ServiceCompanyId;
            if (isInterCompany == true && isCustomer == null && recAmt < 0)
            {
                jmodel.DocDebit = -recAmt;
                //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.BaseDebit = payment.BaseCurrency != payment.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocDebit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocDebit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocDebit, 2, MidpointRounding.AwayFromZero);
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
            }
            if (isInterCompany == true && isCustomer == null && recAmt > 0)
            {
                jmodel.DocCredit = recAmt;
                //jmodel.BaseDebit = Math.Round((decimal)(jmodel.DocDebit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.BaseCredit = payment.BaseCurrency != payment.DocCurrency ? (originalExchangeRate != 0 && originalExchangeRate != null) ? Math.Round((decimal)(jmodel.DocCredit * originalExchangeRate), 2) : Math.Round((decimal)(jmodel.DocCredit * 1), 2, MidpointRounding.AwayFromZero) : Math.Round((decimal)jmodel.DocCredit, 2, MidpointRounding.AwayFromZero);
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = isCustomer == false ? account1.Id : isBankDocDiff == true ? clearingCOAId : chartOfAccount.Id;
            }
            if (isInterCompany == false && isCustomer == true)
            {
                jmodel.DocCredit = recAmt;
                jmodel.BaseCredit = Math.Round((decimal)(jmodel.DocCredit * jmodel.ExchangeRate), 2, MidpointRounding.AwayFromZero);
                jmodel.DocDebit = null;
                jmodel.BaseDebit = null;
                jmodel.OffsetDocument = detail.SystemReferenceNumber;
                if (account1 != null)
                    jmodel.COAId = account1.Id;
            }

        }

        private void FillExchangeGainLossItemOffset(Payment payment, JVVDetailModel journal1, PaymentDetail detail, JVVDetailModel journalDetailData)
        {
            var originalExchangeRate1 = (payment.SystemCalculatedExchangeRate == null || payment.SystemCalculatedExchangeRate == 0) ? payment.ExchangeRate == null ? 0 : payment.ExchangeRate : payment.SystemCalculatedExchangeRate;
            journal1.DocumentDetailId = detail.Id;
            journal1.DocumentId = payment.Id;
            journal1.PostingDate = payment.DocDate;
            journal1.SystemRefNo = payment.SystemRefNo;
            journal1.ServiceCompanyId = payment.ServiceCompanyId;
            journal1.AccountDescription = payment.Remarks;
            journal1.DocDate = payment.DocDate.Date;
            journal1.DocType = payment.DocType;
            journal1.DocSubType = payment.DocSubType;
            journal1.DocNo = payment.DocNo;
            if (journalDetailData != null)
            {
                journal1.Nature = journalDetailData.Nature;
                journal1.EntityId = journalDetailData.EntityId;
                journal1.OffsetDocument = journalDetailData.SystemRefNo;
                journal1.ExchangeRate = journalDetailData.ExchangeRate;
            }
            AppsWorld.CommonModule.Entities.ChartOfAccount account2 =
                _chartOfAccountService.Query(a => a.Name == "Exchange Gain/Loss - Realised" && a.CompanyId == payment.CompanyId)
                    .Select()
                    .FirstOrDefault();
            if (account2 != null)
            {
                journal1.COAId = account2.Id;
            }
            journal1.BaseCurrency = payment.BaseCurrency;
            if (detail.DocumentType == DocTypeConstants.Invoice || detail.DocumentType == DocTypeConstants.DebitNote || detail.DocumentType == DocTypeConstants.BillCreditMemo)
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseCredit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseDebit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
            else
            {
                if (originalExchangeRate1 > journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                        journal1.BaseDebit = Math.Round((decimal)(originalExchangeRate1 - journal1.ExchangeRate) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                }
                if (originalExchangeRate1 < journal1.ExchangeRate)
                {
                    if (originalExchangeRate1 != null && journal1.ExchangeRate != null)
                    {
                        journal1.BaseCredit = Math.Round((decimal)(journal1.ExchangeRate - originalExchangeRate1) * detail.PaymentAmount, 2, MidpointRounding.AwayFromZero);
                    }
                }
            }
        }


        #endregion


        #region CM Application_and_CN App

        private void FillCreditMemoAplication(CreditMemoApplicationModel model, CreditMemoCompact creditMemo, PaymentDetail paymentDetail, Payment payment, int? serEntCount, long? icCOA, long? clearingReceiptCOA, bool isICActive)
        {
            model.Id = Guid.NewGuid();
            model.CompanyId = creditMemo.CompanyId;
            model.CreditMemoId = creditMemo.Id;
            model.DocNo = creditMemo.DocNo;
            model.DocDate = creditMemo.DocDate;
            model.CreditMemoApplicationNumber = creditMemo.DocNo;
            model.DocCurrency = creditMemo.DocCurrency;
            model.UserCreated = PaymentsConstants.System;
            model.CreatedDate = DateTime.UtcNow;
            model.CreditMemoApplicationDate = payment.DocDate;
            model.IsNoSupportingDocument = payment.IsNoSupportingDocument;
            model.NoSupportingDocument = payment.NoSupportingDocs;
            model.CreditAmount = paymentDetail.PaymentAmount;
            model.DocumentId = paymentDetail.Id;
            model.CreditMemoAmount = creditMemo.GrandTotal;
            model.CreditMemoBalanceAmount = creditMemo.DocumentState != CreditMemoState.Fully_Applied ? creditMemo.BalanceAmount : paymentDetail.PaymentAmount;
            model.IsGstSettings = payment.IsGstSettings;
            model.Status = CreditMemoApplicationStatus.Posted;
            model.DocSubType = DocTypeConstants.CMApplication;
            model.ExchangeRate = creditMemo.ExchangeRate;
            model.GSTExchangeRate = creditMemo.GSTExchangeRate;
            model.IsOffset = true;
            model.Remarks = "CM Application - " + payment.DocNo;
            model.CreditMemoApplicationDetailModels.Add(new CreditMemoApplicationDetailModel()
            {
                Id = Guid.NewGuid(),
                CreditMemoApplicationId = model.Id,
                DocType = DocTypeConstants.BillPayment,
                DocumentId = paymentDetail.DocumentId,
                DocCurrency = creditMemo.DocCurrency,
                CreditAmount = paymentDetail.PaymentAmount,
                DocNo = payment.DocNo,
                DocDate = payment.DocDate,
                Nature = creditMemo.Nature,
                ServiceEntityId = payment.ServiceCompanyId,
                DocState = "Posted",
                //COAId = serEntCount == 1 && isICActive == false ? clearingReceiptCOA : (serEntCount > 1 || isICActive) ? icCOA : clearingReceiptCOA,
                COAId = ((paymentDetail.DocumentType == DocTypeConstants.CreditNote || paymentDetail.DocumentType == DocTypeConstants.BillCreditMemo) && payment.ServiceCompanyId == paymentDetail.ServiceCompanyId) ? clearingReceiptCOA : serEntCount == 1 && isICActive == false ? clearingReceiptCOA : (serEntCount > 1 || isICActive) ? (payment.BankPaymentAmmountCurrency != payment.DocCurrency ? clearingReceiptCOA : icCOA) : _chartOfAccountService.GetCoaIdByNameAndCompanyId(paymentDetail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId),
                BaseCurrencyExchangeRate = payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate
            });

        }
        private void FillCreditNoteAplication(CreditNoteApplicationModel model, InvoiceCompact creditNote, PaymentDetail paymentDetail, Payment payment, int? serEntityCount, long? icCOA, long? clearingReceiptCOA, bool isICActive)
        {
            model.Id = Guid.NewGuid();
            model.CompanyId = creditNote.CompanyId;
            model.InvoiceId = creditNote.Id;
            model.DocNo = creditNote.DocNo;
            model.DocDate = creditNote.DocDate;
            model.CreditNoteApplicationNumber = creditNote.DocNo;
            model.DocCurrency = creditNote.DocCurrency;
            model.UserCreated = PaymentsConstants.System;
            model.CreatedDate = DateTime.UtcNow;
            model.CreditNoteApplicationDate = payment.DocDate;
            model.IsNoSupportingDocument = creditNote.IsNoSupportingDocument;
            model.NoSupportingDocument = creditNote.NoSupportingDocs;
            model.CreditAmount = paymentDetail.PaymentAmount;
            model.DocumentId = paymentDetail.Id;
            model.CreditNoteAmount = creditNote.GrandTotal;
            model.CreditNoteBalanceAmount = creditNote.DocumentState != CreditNoteState.FullyApplied ? creditNote.BalanceAmount : paymentDetail.PaymentAmount;
            model.IsGstSettings = creditNote.IsGstSettings;
            model.Status = CreditNoteApplicationStatus.Posted;
            model.DocSubType = DocTypeConstants.Application;
            model.ExchangeRate = creditNote.ExchangeRate;
            model.GSTExchangeRate = creditNote.GSTExchangeRate;
            model.IsOffset = true;
            model.Remarks = "CN Application - " + payment.DocNo;

            model.CreditNoteApplicationDetailModels.Add(new CreditNoteApplicationDetailModel()
            {
                Id = Guid.NewGuid(),
                CreditNoteApplicationId = model.Id,
                DocType = DocTypeConstants.BillPayment,
                DocumentId = paymentDetail.DocumentId,
                DocCurrency = creditNote.DocCurrency,
                CreditAmount = paymentDetail.PaymentAmount,
                DocNo = payment.DocNo,
                DocDate = payment.DocDate,
                Nature = creditNote.Nature,
                ServiceEntityId = payment.ServiceCompanyId,
                DocState = "Posted",
                //COAId = serEntityCount == 1 && isICActive == false ? clearingReceiptCOA : (serEntityCount > 1 || isICActive) ? icCOA : clearingReceiptCOA,
                COAId = ((paymentDetail.DocumentType == DocTypeConstants.CreditNote || paymentDetail.DocumentType == DocTypeConstants.BillCreditMemo) && payment.ServiceCompanyId == paymentDetail.ServiceCompanyId) ? clearingReceiptCOA : serEntityCount == 1 && isICActive == false ? clearingReceiptCOA : (serEntityCount > 1 || isICActive) ? (payment.BankPaymentAmmountCurrency != payment.DocCurrency ? clearingReceiptCOA : icCOA) : _chartOfAccountService.GetCoaIdByNameAndCompanyId(paymentDetail.Nature == "Trade" ? COANameConstants.AccountsPayable : COANameConstants.OtherPayables, payment.CompanyId),
                BaseCurrencyExchangeRate = payment.DocCurrency == payment.BankPaymentAmmountCurrency ? payment.ExchangeRate : payment.SystemCalculatedExchangeRate
            });

        }
        private static void CNAplicationREST(CreditNoteApplicationModel creditNoteModel, DocumentResetModel voidModel, bool? isVoid)
        {
            string json = null;
            if (isVoid == true)
            { json = RestSharpHelper.ConvertObjectToJason(voidModel); }
            else
            { json = RestSharpHelper.ConvertObjectToJason(creditNoteModel); }
            try
            {
                string url = ConfigurationManager.AppSettings.Get("BeanUrl");
                //object obj = creditNoteModel;
                IRestResponse response = null;
                if (isVoid == true)
                {
                    response = RestSharpHelper.Post(url, "api/v2/invoice/savecreditnoteapplicationvoid", json);
                }
                else
                {
                    response = RestSharpHelper.Post(url, "api/v2/invoice/savecreditnoteapplication", json);
                }
                if (response.ErrorMessage != null)
                {
                    //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }
        private static void CMAplicationREST(CreditMemoApplicationModel creditMemoModel, DocumentResetModel voidModel, bool? isVoid)
        {
            string json = null;
            if (isVoid != true)
            {
                json = RestSharpHelper.ConvertObjectToJason(creditMemoModel);
            }
            else
            {
                json = RestSharpHelper.ConvertObjectToJason(voidModel);
            }
            try
            {
                //ZiraffSection section = (ZiraffSection)ConfigurationManager.GetSection("Server");
                string url = ConfigurationManager.AppSettings.Get("BeanUrl");
                IRestResponse response = null;
                //object obj = creditMemoModel;
                if (isVoid == true)
                {
                    response = RestSharpHelper.Post(url, "/api/creditmemo/savecreditmemovoid", json);
                }
                else
                {
                    response = RestSharpHelper.Post(url, "/api/creditmemo/savecreditmemoapplication", json);
                }
                if (response != null)
                {
                    if (response.ErrorMessage != null)
                    {
                        //Log.Logger.Error(string.Format("Error Message {0}", response.ErrorMessage));
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
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


        private void UpdateOBLineItem(Guid? openingBalanceId, Guid? documentId, long companyId, bool isEqual, string connectionString)
        {
            string storedProcedure = "Proc_UpdateOBLineItem";
            using (con = new SqlConnection(connectionString))
            {
                using (cmd = new SqlCommand(storedProcedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@OBId", openingBalanceId);
                    cmd.Parameters.AddWithValue("@DocumentId", documentId);
                    cmd.Parameters.AddWithValue("@CompanyId", companyId);
                    cmd.Parameters.AddWithValue("@IsEqual", isEqual);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
