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
using Newtonsoft.Json;

namespace AppsWorld.InvoiceModule.Application.V2
{
    public class InvoiceApplicationService
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMasterCompactService _masterService;
        private readonly IAutoNumberService _autoNumberService;
        private readonly IItemCompactService _itemService;
        private readonly ICurrencyCompactService _currencyService;
        private readonly IInvoiceComptModuleUnitOfWorkAsync _unitOfWork;
        public InvoiceApplicationService(IInvoiceService invoiceService, IMasterCompactService masterService, IAutoNumberService autoNumberService, IItemCompactService itemService, IInvoiceComptModuleUnitOfWorkAsync unitOfWork, ICurrencyCompactService currencyService)
        {
            this._invoiceService = invoiceService;
            this._masterService = masterService;
            this._autoNumberService = autoNumberService;
            this._itemService = itemService;
            this._unitOfWork = unitOfWork;
            this._currencyService = currencyService;
        }

        #region Create Invoice
        public InvoiceModel CreateInvoice(long companyid, Guid id)
        {
            InvoiceModel invDTO = new InvoiceModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_Request_Message);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyid);
                if (financSettings == null)
                {
                    throw new Exception(InvoiceValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                Invoice invoice = _invoiceService.GetCompanyAndId(companyid, id);
                if (invoice == null)
                {
                    AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyid, DocTypeConstant.Invoice);
                    DateTime? lastInvoiceDate = _invoiceService.GetByCompanyId(companyid, DocTypeConstant.Invoice);
                    invDTO.Id = Guid.NewGuid();
                    invDTO.CompanyId = companyid;
                    invDTO.DocDate = lastInvoiceDate == null ? DateTime.Now : lastInvoiceDate.Value;
                    bool? isEdit = false;
                    invDTO.DocNo = GetAutoNumberByEntityType(companyid, lastInvoiceDate, DocTypeConstant.Invoice, _autoNo, true, ref isEdit);
                    invDTO.IsDocNoEditable = isEdit;

                    invDTO.DocumentState = "Not Paid";
                    invDTO.DueDate = DateTime.UtcNow;
                    invDTO.NoSupportingDocument = false;
                    invDTO.IsRepeatingInvoice = false;
                    invDTO.CreatedDate = DateTime.UtcNow;

                    invDTO.BaseCurrency = financSettings.BaseCurrency;
                    invDTO.DocCurrency = invDTO.BaseCurrency;
                    invDTO.DocCurrency = invDTO.BaseCurrency;
                }
                else
                {

                    FillInvoice(invDTO, invoice);
                    invDTO.IsDocNoEditable = _autoNumberService.GetAutoNumberFlag(companyid, DocTypeConstant.Invoice);
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return invDTO;
        }

        #endregion

        #region Invoice LookUp
        public InvoiceModelLU GetAllInvoiceNewLUs(string username, Guid invoiceId, long companyid, string ConnectionString)
        {
            InvoiceModelLU invoiceLU = new InvoiceModelLU();
            SqlConnection con = null;
            SqlCommand cmd = null;
            try
            {
                DateTime? lastInvoice = _invoiceService.GetByCompanyId(companyid, DocTypeConstant.Invoice);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Request_Message);
                Invoice invoice = _invoiceService.GetAllInvoiceLu(companyid, invoiceId);
                DateTime date = invoice == null ? lastInvoice == null ? DateTime.Now : lastInvoice.Value : invoice.DocDate;
                long? credit = invoice == null ? 0 : invoice.CreditTermsId == null ? 0 : invoice.CreditTermsId;
                long? comp = invoice == null ? 0 : invoice.ServiceCompanyId == null ? 0 : invoice.ServiceCompanyId;
                List<CommonLookUps<string>> lstLookUps = new List<CommonLookUps<string>>();
                LookUpCategory<string> currency = new LookUpCategory<string>();
                string currencyCode = invoice != null ? invoice.DocCurrency : string.Empty;
                string query = InvoiceCommonQuery(username, companyid, date, credit, comp, currencyCode);
                int? resultsetCount = query.Split(';').Count();
                con = new SqlConnection(ConnectionString);
                if (con.State != ConnectionState.Open)
                    con.Open();
                using (cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlDataReader dr = cmd.ExecuteReader();
                    for (int i = 1; i <= resultsetCount; i++)
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                lstLookUps.Add(new CommonLookUps<string>
                                {
                                    TableName = dr["TABLENAME"].ToString(),
                                    Code = dr["CODE"].ToString(),
                                    Id = dr["ID"] != DBNull.Value ? Convert.ToInt64(dr["ID"]) : 0,
                                    Name = dr["NAME"].ToString(),
                                    RecOrder = dr["RECORDER"] != DBNull.Value ? Convert.ToInt32(dr["RECORDER"]) : (int?)null,
                                    TaxRate = dr["TAXRATE"] != DBNull.Value ? Convert.ToDouble(dr["TAXRATE"]) : (double?)null,
                                    TaxType = dr["TAXTYPE"].ToString(),
                                    TaxCode = dr["TXCODE"].ToString(),
                                    TOPValue = dr["TOPVALUE"] != DBNull.Value ? Convert.ToDouble(dr["TOPVALUE"]) : (double?)null,
                                    Currency = dr["CURRENCY"].ToString(),
                                    Class = dr["Class"].ToString(),
                                    isGstActivated = dr["IsGstActive"] != DBNull.Value ? Convert.ToBoolean(dr["IsGstActive"]) : (bool?)null,
                                    ShortName = dr["SHOTNAME"].ToString(),
                                    //ServiceCompanyId= row["Id"] != DBNull.Value ? Convert.ToInt64(row["Id"]) : (long?)null,
                                    DefaultValue = "SGD",
                                    CategoryName = "SGD",
                                    Status = (RecordStatusEnum)dr["STATUS"]
                                });
                            }
                        }
                        dr.NextResult();
                    }
                }
                invoiceLU.NatureLU = new List<string> { "Trade", "Others" };
                if (lstLookUps.Any())
                {
                    if (invoice != null)
                    {
                        currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                        currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                        currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYEDIT").Select(c => new LookUp<string>()
                        {
                            Code = c.Code,
                            Name = c.Name,
                            RecOrder = c.RecOrder
                        }).ToList();
                        invoiceLU.CurrencyLU = currency;
                    }
                    else
                    {
                        currency.CategoryName = ControlCodeConstants.Currency_DefaultCode;
                        currency.DefaultValue = ControlCodeConstants.Currency_DefaultCode;
                        currency.Lookups = lstLookUps.Where(c => c.TableName == "CURRENCYNEW").Select(c => new LookUp<string>()
                        {
                            Code = c.Code,
                            Name = c.Name,
                            RecOrder = c.RecOrder
                        }).ToList();
                        invoiceLU.CurrencyLU = currency;
                    }
                    invoiceLU.TermsOfPaymentLU = lstLookUps.Where(c => c.TableName == "TERMSOFPAY").Select(x => new LookUp<string>()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        TOPValue = x.TOPValue,
                        RecOrder = x.RecOrder
                    }).OrderBy(c => c.TOPValue).ToList();
                    invoiceLU.SubsideryCompanyLU = lstLookUps.Where(c => c.TableName == "SERVICECOMPANY").Select(x => new LookUpCompany<string>()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ShortName = x.ShortName,
                        isGstActivated = x.isGstActivated
                    }).ToList();
                }
                List<COALookup<string>> lstEditCoa = new List<COALookup<string>>();
                List<TaxCodeLookUp<string>> lstEditTax = null;
                invoiceLU.ChartOfAccountLU = lstLookUps.Where(z => z.TableName == "CHARTOFACCOUNT" && z.Status == RecordStatusEnum.Active).Select(x => new COALookup<string>()
                {
                    Name = x.Name,
                    Code = x.Code,
                    Id = x.Id,
                    RecOrder = x.RecOrder,
                    IsPLAccount = x.COACategory == "Income Statement" ? true : false,
                    Class = x.Class,
                    Status = x.Status,
                    IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                }).OrderBy(d => d.Name).ToList();
                invoiceLU.TaxCodeLU = lstLookUps.Where(c => c.TableName == "TAXCODE" && c.Status == RecordStatusEnum.Active).Select(x => new TaxCodeLookUp<string>()
                {
                    Id = x.Id,
                    Code = x.TaxCode,
                    Name = x.Name,
                    TaxRate = x.TaxRate,
                    TaxType = x.TaxType,
                    Status = x.Status,
                    TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.TaxCode
                }).OrderBy(c => c.Code).ToList();
                if (invoice != null && invoice.InvoiceDetails.Count > 0)
                {
                    List<long> CoaIds = invoice.InvoiceDetails.Select(c => c.COAId).ToList();
                    if (invoiceLU.ChartOfAccountLU.Any())
                        CoaIds = CoaIds.Except(invoiceLU.ChartOfAccountLU.Select(x => x.Id)).ToList();
                    List<long?> taxIds = invoice.InvoiceDetails.Select(x => x.TaxId).ToList();
                    if (invoiceLU.TaxCodeLU.Any())
                        taxIds = taxIds.Except(invoiceLU.TaxCodeLU.Select(d => d.Id)).ToList();
                    if (CoaIds.Any())
                    {
                        lstEditCoa = lstLookUps.Where(x => x.TableName == "CHARTOFACCOUNT" && CoaIds.Contains(x.Id)).Select(x => new COALookup<string>()
                        {
                            Name = x.Name,
                            Code = x.TaxCode,
                            Id = x.Id,
                            RecOrder = x.RecOrder,
                            IsPLAccount = x.COACategory == "Income Statement" ? true : false,
                            Class = x.Class,
                            Status = x.Status,
                            IsTaxCodeNotEditable = (x.Class == "Assets" || x.Class == "Liabilities" || x.Class == "Equity") ? true : false,
                        }).OrderBy(d => d.Name).ToList();
                        invoiceLU.ChartOfAccountLU.AddRange(lstEditCoa);
                    }
                    if (invoice.IsOBInvoice == true)
                    {
                        CommonLookUps<string> ObCOA = lstLookUps.Where(c => c.TableName == "OBCHARTOFACCOUNT").FirstOrDefault();
                        if (ObCOA != null)
                        {
                            invoiceLU.ChartOfAccountLU.Add(new COALookup<string>()
                            {
                                Name = ObCOA.Name,
                                Code = ObCOA.Code,
                                Id = ObCOA.Id,
                                RecOrder = ObCOA.RecOrder,
                                IsPLAccount = ObCOA.COACategory == "Income Statement" ? true : false,
                                Class = ObCOA.Class,
                                Status = ObCOA.Status,
                                IsTaxCodeNotEditable = (ObCOA.Class == "Assets" || ObCOA.Class == "Liabilities" || ObCOA.Class == "Equity") ? true : false
                            });
                        }
                    }

                    if (taxIds.Any())
                    {
                        lstEditTax = lstLookUps.Where(c => taxIds.Contains(c.Id) && c.TableName == "TAXCODE").Select(x => new TaxCodeLookUp<string>()
                        {
                            Id = x.Id,
                            Code = x.TaxCode,
                            Name = x.Name,
                            TaxRate = x.TaxRate,
                            TaxType = x.TaxType,
                            Status = x.Status,
                            TaxIdCode = x.TaxCode != "NA" ? x.TaxCode + "-" + x.TaxRate + (x.TaxRate != null ? "%" : "NA") /*+ "(" + x.TaxType[0] + ")"*/ : x.TaxCode
                        }).ToList();
                        invoiceLU.TaxCodeLU.AddRange(lstEditTax);
                        invoiceLU.TaxCodeLU = invoiceLU.TaxCodeLU.OrderBy(c => c.Code).ToList();
                    }
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_GetAllInvoiceLUs_LookupCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message + " for " + companyid);
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return invoiceLU;
        }

        #endregion

        #region GetInvoiceDetail
        public InvoiceDetail GetInvoiceDetail(Guid invoiceId, Guid invoiceDetalId)
        {


            InvoiceDetail detail = _invoiceService.GetAllInvoiceIdAndId(invoiceId, invoiceDetalId);
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetInvoiceDetail_GetCall_Request_Message);
                if (detail == null)
                {
                    detail = new InvoiceDetail();
                    detail.Qty = 1;
                    detail.Discount = 0;
                }
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_GetInvoiceDetail_GetCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return detail;
        }
        #endregion

        #region Direct_Get_Call_from_Invoice
        public CreditNoteModel CreateCreditNoteByInvoice(Guid invoiceId, long companyId)
        {
            CreditNoteModel invDTO = new CreditNoteModel();
            try
            {
                AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.CreditNote);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_Request_Message);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(InvoiceValidation.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                DateTime? lastInvoice = _invoiceService.GetByCompanyId(companyId, DocTypeConstants.CreditNote);
                Invoice invoice = _invoiceService.GetAllInvoiceByIdDocType(invoiceId);
                invDTO.Id = Guid.NewGuid();

                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = DocTypeConstants.CreditNote;

                bool? isEdit = false;
                invDTO.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice, DocTypeConstants.CreditNote, _autoNo, false, ref isEdit);
                invDTO.IsDocNoEditable = isEdit;

                //invDTO.DocNo = GetNewInvoiceDocumentNumber(DocTypeConstants.CreditNote, invDTO.CompanyId, false);
                invDTO.CreditTermsId = invoice.CreditTermsId;
                invDTO.DocDate = invoice.DocDate;
                IDictionary<double?, string> top = _masterService.GetTermsOfPayment(invDTO.CreditTermsId);
                if (top != null)
                {
                    invDTO.CreditTermsName = top.Values.FirstOrDefault();
                    invDTO.DueDate = invDTO.DocDate.AddDays(top.Keys.FirstOrDefault().Value);
                }
                invDTO.EntityId = invoice.EntityId;
                invDTO.EntityName = _masterService.GetEntityName(invoice.EntityId);
                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;

                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;
                invDTO.IsGstSettings = invoice.IsGstSettings;
                invDTO.GSTExCurrency = invoice.GSTExCurrency;
                invDTO.GSTExchangeRate = invoice.GSTExchangeRate;
                invDTO.GSTExDurationFrom = invoice.GSTExDurationFrom;
                invDTO.GSTExDurationTo = invoice.GSTExDurationTo;
                invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                invDTO.GSTTotalAmount = invoice.GSTTotalAmount;
                invDTO.GrandTotal = invoice.DocumentState == InvoiceState.NotPaid ? invoice.GrandTotal : invoice.BalanceAmount;
                invDTO.BalanceAmount = invoice.BalanceAmount;
                invDTO.ExtensionType = invoice.IsOBInvoice == true ? "OBInvoice" : ExtensionType.Invoice;
                invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;
                invDTO.Remarks = invoice.DocDescription;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = CreditNoteState.NotApplied;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;

                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                List<InvoiceDetailModel> lstInvDetail = new List<InvoiceDetailModel>();
                if (invoice.InvoiceDetails.Any())
                {
                    foreach (var detail in invoice.InvoiceDetails)
                    {
                        InvoiceDetailModel cnDetail = new InvoiceDetailModel();
                        cnDetail.Id = Guid.NewGuid();
                        cnDetail.InvoiceId = invDTO.Id;
                        cnDetail.AmtCurrency = detail.AmtCurrency;
                        cnDetail.BaseAmount = detail.BaseAmount;
                        cnDetail.BaseTaxAmount = detail.BaseTaxAmount;
                        cnDetail.BaseTotalAmount = detail.BaseTotalAmount;
                        cnDetail.COAId = invoice.IsOBInvoice == true ? 0 : detail.COAId;
                        cnDetail.DocAmount = detail.DocAmount;
                        cnDetail.DocTaxAmount = detail.DocTaxAmount;
                        cnDetail.DocTotalAmount = detail.DocTotalAmount;
                        cnDetail.ItemDescription = detail.ItemDescription;
                        cnDetail.Remarks = detail.Remarks;
                        cnDetail.TaxId = detail.TaxId;
                        cnDetail.TaxIdCode = detail.TaxIdCode;
                        cnDetail.TaxRate = detail.TaxRate;
                        cnDetail.RecOrder = detail.RecOrder;
                        lstInvDetail.Add(cnDetail);
                    }
                }
                invDTO.InvoiceDetailModels = lstInvDetail.OrderBy(c => c.RecOrder).ToList();
                CreditNoteApplicationModel CNAModel = new CreditNoteApplicationModel();
                CNAModel.Id = Guid.NewGuid();
                CNAModel.InvoiceId = invDTO.Id;
                CNAModel.CompanyId = invoice.CompanyId;
                CNAModel.DocNo = invoice.DocNo;
                CNAModel.DocCurrency = invoice.DocCurrency;
                CNAModel.CreditNoteApplicationDate = DateTime.UtcNow;
                CNAModel.DocDate = invDTO.DocDate;
                CNAModel.Remarks = invoice.DocDescription;
                decimal sumLineTotal = 0;
                if (invDTO.InvoiceDetails.Any())
                {
                    sumLineTotal = invDTO.InvoiceDetails.Sum(od => od.DocTotalAmount);
                }
                CNAModel.CreditAmount = invoice.GrandTotal;
                CNAModel.CreditNoteAmount = invoice.GrandTotal;
                CNAModel.CreditNoteBalanceAmount = invoice.GrandTotal;
                CNAModel.CreditNoteApplicationDate = invoice.DocDate;
                CNAModel.EntityName = invDTO.EntityName;
                CNAModel.ExchangeRate = invDTO.ExchangeRate;
                CNAModel.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                CNAModel.NoSupportingDocument = invoice.NoSupportingDocs;
                CNAModel.FinancialPeriodLockStartDate = invDTO.FinancialPeriodLockStartDate;
                CNAModel.FinancialPeriodLockEndDate = invDTO.FinancialPeriodLockEndDate;
                CNAModel.CreatedDate = DateTime.UtcNow;
                CNAModel.UserCreated = invoice.UserCreated;
                CNAModel.Status = CreditNoteApplicationStatus.Posted;

                invDTO.CreditNoteApplicationModel = CNAModel;

                List<CreditNoteApplicationDetailModel> lstCNADModel = new List<CreditNoteApplicationDetailModel>();
                CreditNoteApplicationDetailModel detailModel = new CreditNoteApplicationDetailModel();

                detailModel.Id = Guid.NewGuid();
                detailModel.CreditNoteApplicationId = CNAModel.Id;
                detailModel.BalanceAmount = invoice.BalanceAmount;
                detailModel.Nature = invoice.Nature;
                detailModel.DocCurrency = CNAModel.DocCurrency;
                detailModel.DocType = DocTypeConstants.Invoice;
                detailModel.DocAmount = invoice.GrandTotal;
                detailModel.DocDate = invoice.DocDate;
                detailModel.DocumentId = invoice.Id;
                detailModel.DocNo = invoice.DocNo;
                detailModel.DocState = invoice.DocumentState;
                detailModel.SystemReferenceNumber = invoice.InvoiceNumber;
                if (invDTO.ServiceCompanyId != null)
                {
                    IDictionary<long, string> company = _masterService.GetById(invDTO.ServiceCompanyId.Value);
                    if (company != null)
                    {
                        detailModel.ServiceEntityId = company.Keys.FirstOrDefault();
                        detailModel.ServEntityName = company.Values.FirstOrDefault();
                    }
                }
                detailModel.BaseCurrencyExchangeRate = invoice.ExchangeRate.Value;
                decimal sumLineTotal1 = 0;
                //decimal diff = 0;
                if (invDTO.InvoiceDetails.Any())
                {
                    sumLineTotal1 = invDTO.InvoiceDetails.Sum(od => od.DocTotalAmount);
                }
                detailModel.CreditAmount = invoice.BalanceAmount;
                lstCNADModel.Add(detailModel);

                invDTO.CreditNoteApplicationModel.CreditNoteApplicationDetailModels = lstCNADModel;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_Exception_Message);

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return invDTO;
        }

        public DoubtfulDebtModel CreateDoubtFulDebtByInvoice(Guid invoiceId, long companyId)
        {
            DoubtfulDebtModel invDTO = new DoubtfulDebtModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateDoubtFulDebtByInvoice_CreateCall_Request_Message);
                AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.DoubtFulDebitNote);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyId);
                if (financSettings == null)
                {
                    throw new Exception(CommonConstant.The_Financial_setting_should_be_activated);
                }
                invDTO.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                invDTO.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                DateTime? lastInvoice = _invoiceService.GetByCompanyId(companyId, DocTypeConstants.DoubtFulDebitNote);
                Invoice invoice = _invoiceService.GetAllInvoiceByIdDocType(invoiceId);
                invDTO.Id = Guid.NewGuid();
                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = DocTypeConstants.DoubtFulDebitNote;
                bool? isEdit = false;
                invDTO.DocNo = GetAutoNumberByEntityType(companyId, lastInvoice, DocTypeConstants.DoubtFulDebitNote, _autoNo, false, ref isEdit);
                invDTO.IsDocNoEditable = isEdit;
                invDTO.DocDate = invoice.DocDate;
                invDTO.EntityId = invoice.EntityId;
                invDTO.EntityName = _masterService.GetEntityName(invoice.EntityId);
                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;
                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;
                invDTO.ExtensionType = ExtensionType.Invoice;
                invDTO.GrandTotal = invoice.BalanceAmount;
                invDTO.BalanceAmount = invoice.BalanceAmount;
                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;
                invDTO.Remarks = invoice.DocDescription;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = DoubtfulDebtState.NotAllocated;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;
                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                DoubtfulDebtAllocationModel DDAModel = new DoubtfulDebtAllocationModel();
                DDAModel.Id = Guid.NewGuid();
                DDAModel.CompanyId = companyId;
                DDAModel.ExchangeRate = invDTO.ExchangeRate;
                DDAModel.EntityName = invDTO.EntityName;
                DDAModel.InvoiceId = invDTO.Id;
                DDAModel.DocNo = invoice.DocNo;
                DDAModel.DoubtfulDebitAllocationDate = invDTO.DocDate;
                DDAModel.DocCurrency = invoice.DocCurrency;
                DDAModel.DoubtfulDebtAmount = invoice.BalanceAmount;
                DDAModel.DoubtfulDebtBalanceAmount = invoice.BalanceAmount;
                DDAModel.AllocateAmount = invoice.BalanceAmount;
                DDAModel.DoubtfulDebtAllocationNumber = invoice.InvoiceNumber;
                DDAModel.FinancialPeriodLockStartDate = invDTO.FinancialPeriodLockStartDate;
                DDAModel.FinancialPeriodLockEndDate = invDTO.FinancialPeriodLockEndDate;
                DDAModel.EntityName = invDTO.EntityName;
                DDAModel.ExchangeRate = invDTO.ExchangeRate;
                DDAModel.DocDate = invoice.DocDate;
                DDAModel.Status = DoubtfulDebtAllocationStatus.Tagged;
                DDAModel.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                DDAModel.NoSupportingDocument = false;

                invDTO.DoubtfulDebtAllocation = DDAModel;

                List<DoubtfulDebtAllocationDetailModel> lstDDAD = new List<DoubtfulDebtAllocationDetailModel>();

                DoubtfulDebtAllocationDetailModel dDAD = new DoubtfulDebtAllocationDetailModel();

                dDAD.Id = Guid.NewGuid();
                dDAD.DoubtfulDebitAllocationId = DDAModel.Id;
                dDAD.DocType = DocTypeConstants.Invoice;
                dDAD.DocumentId = invoiceId;
                dDAD.DocCurrency = DDAModel.DocCurrency;
                dDAD.DocAmount = invoice.GrandTotal;
                dDAD.DocDate = invoice.DocDate;
                dDAD.DocumentId = invoice.Id;
                dDAD.DocNo = invoice.DocNo;
                dDAD.DocState = invoice.DocumentState;
                if (invDTO.ServiceCompanyId != null)
                {
                    IDictionary<long, string> company = _masterService.GetById(invDTO.ServiceCompanyId.Value);
                    if (company != null)
                    {
                        dDAD.ServiceEntityId = company.Keys.FirstOrDefault();
                        dDAD.ServEntityName = company.Values.FirstOrDefault();
                    }
                }
                dDAD.SystemReferenceNumber = invoice.InvoiceNumber;
                dDAD.AllocateAmount = invoice.BalanceAmount;
                dDAD.BalanceAmount = invoice.BalanceAmount;
                dDAD.Nature = invoice.Nature;
                lstDDAD.Add(dDAD);

                invDTO.DoubtfulDebtAllocation.DoubtfulDebtAllocationDetailModels = lstDDAD;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_CreateDoubtFulDebtByInvoice_CreateCall_SuccessFully_Message);
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

            return invDTO;

        }

        public ReceiptModel CreateReceiptByInvoice(Guid invoiceId, long companyId)
        {
            ReceiptModel _receiptModel = new ReceiptModel();
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_Enter_into_CreateReceiptByInvoice_action);
                AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(companyId, DocTypeConstants.Receipt);
                DateTime? lastReceipt = _invoiceService.GetReceiptsDate(companyId);
                FinancialSettingCompact financSettings = _masterService.GetFinancialSetting(companyId);
                Invoice invoice = _invoiceService.GetAllInvoiceByIdDocType(invoiceId);
                _receiptModel.Id = Guid.NewGuid();
                _receiptModel.CompanyId = invoice.CompanyId;
                _receiptModel.DocSubType = DocTypeConstants.Receipt;
                bool? isEdit = false;
                _receiptModel.DocNo = GetAutoNumberForReceipt(companyId, lastReceipt, DocTypeConstants.Receipt, _autoNo, ref isEdit);
                _receiptModel.IsDocNoEditable = isEdit;
                _receiptModel.DocDate = invoice.DocDate;
                _receiptModel.CreditTermId = invoice.CreditTermsId;
                _receiptModel.EntityId = invoice.EntityId;
                _receiptModel.EntityName = _masterService.GetEntityName(invoice.EntityId);
                _receiptModel.ISMultiCurrency = invoice.IsMultiCurrency;
                _receiptModel.DocCurrency = invoice.DocCurrency;
                _receiptModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
                _receiptModel.GstExchangeRate = invoice.GSTExchangeRate;
                _receiptModel.BaseCurrency = invoice.ExCurrency;
                _receiptModel.ExchangeRate = invoice.ExchangeRate;
                _receiptModel.ExDurationFrom = invoice.ExDurationFrom;
                _receiptModel.IsGstSettings = invoice.IsGstSettings;
                _receiptModel.GSTTotalAmount = invoice.GSTTotalAmount;
                _receiptModel.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                _receiptModel.NoSupportingDocument = invoice.NoSupportingDocs;
                _receiptModel.Remarks = invoice.DocDescription;
                _receiptModel.Status = invoice.Status;
                _receiptModel.DocumentState = ReceiptState.Created;
                _receiptModel.CreatedDate = invoice.CreatedDate;
                _receiptModel.UserCreated = invoice.UserCreated;
                _receiptModel.GstReportingCurrency = invoice.GSTExCurrency;
                _receiptModel.SaveType = "InDirect";
                _receiptModel.ExtensionType = ExtensionType.Invoice;
                _receiptModel.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                _receiptModel.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                if (financSettings != null)
                {
                    _receiptModel.FinancialPeriodLockStartDate = financSettings.PeriodLockDate;
                    _receiptModel.FinancialPeriodLockEndDate = financSettings.PeriodEndDate;
                }

                List<ReceiptDetailModel> lstDetail = new List<ReceiptDetailModel>();
                ReceiptDetailModel detail = new ReceiptDetailModel();
                detail.Id = Guid.NewGuid();
                detail.ReceiptId = _receiptModel.Id;
                detail.DocumentId = invoice.Id;
                detail.DocumentDate = invoice.DocDate;
                detail.DocumentType = invoice.DocType;
                detail.DocumentNo = invoice.DocNo;
                detail.DocumentState = invoice.DocumentState;
                detail.Nature = invoice.Nature;
                detail.DocumentAmmount = invoice.GrandTotal;
                detail.Currency = invoice.DocCurrency;
                detail.SystemReferenceNumber = invoice.InvoiceNumber;
                detail.AmmountDue = invoice.BalanceAmount;
                detail.ReceiptAmount = invoice.BalanceAmount;
                detail.ServiceCompanyId = invoice.ServiceCompanyId;
                detail.ServiceCompanyName = _masterService.GetIdBy(invoice.ServiceCompanyId.Value);
                lstDetail.Add(detail);
                _receiptModel.ReceiptDetailModels = lstDetail;//.OrderBy(x=>x.DocumentDate).ThenBy(d=>d.SystemReferenceNumber).ToList();
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_CreateCreditNoteByInvoice_CreateCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }
            return _receiptModel;
        }
        #endregion Direct_Get_Call_from_Invoice

        #region Save Invoice
        private RecordStatusEnum eventStatuschanged;

        public Invoice SaveInvoice(InvoiceModel TObject, string ConnectionString)
        {
            var AdditionalInfo = new Dictionary<string, object>();
            AdditionalInfo.Add("Data", JsonConvert.SerializeObject(TObject));
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, "ObjectSave", AdditionalInfo);
            string eventDocStatusChanged = "";
            bool? isExeedLimitEdit = false;
            LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_Request_Message);
            string _errors = CommonValidation.ValidateObject(TObject);
            if (!string.IsNullOrEmpty(_errors))
            {
                throw new Exception(_errors);
            }
            InvoiceValidations(TObject);
            Invoice _invoice = null;
            bool isNew = false;
            if (TObject.IsWorkFlowInvoice == true)
                _invoice = _invoiceService.GetInvoiceByIdAndDocumentId(TObject.DocumentId ?? Guid.Empty, TObject.CompanyId);
            else
                _invoice = _invoiceService.GetAllInvoiceByIdDocType(TObject.Id);
            if (_invoice != null)
            {
                decimal? docTotal = _invoice.GrandTotal - TObject.GrandTotal;
                isExeedLimitEdit = true;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Checking_Invoice_is_null_or_not);
                eventStatuschanged = _invoice.Status;
                eventDocStatusChanged = _invoice.DocumentState;
                _invoice.DocNo = TObject.DocNo;
                _invoice.InvoiceNumber = _invoice.DocNo;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.InsertInvoice_method_came);
                InsertInvoice(TObject, _invoice);
                TObject.CustCreditlimit -= docTotal;
                _invoice.DocSubType = TObject.DocSubType;
                _invoice.InternalState = TObject.InternalState;
                _invoice.BalanceAmount = _invoice.DocumentState == InvoiceState.NotPaid ? TObject.GrandTotal : /*TObject.BalanceAmount*/_invoice.BalanceAmount;
                _invoice.AllocatedAmount = _invoice.AllocatedAmount;
                _invoice.ModifiedBy = TObject.ModifiedBy;
                _invoice.ModifiedDate = DateTime.UtcNow;
                _invoice.ObjectState = ObjectState.Modified;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceDetails_method_came);
                UpdateInvoiceDetails(TObject, _invoice);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceNotes_method_came);
                UpdateInvoiceNotes(TObject, _invoice);
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.UpdateInvoiceGSTDetails_method_came);
                _invoiceService.Update(_invoice);
            }
            else
            {
                _invoice = new Invoice();
                isNew = true;
                InsertInvoice(TObject, _invoice);
                _invoice.DocSubType = (TObject.IsWorkFlowInvoice == true || TObject.IsOBInvoice == true) ? TObject.DocSubType : "General";
                if (_invoice.BalanceAmount == 0)
                    _invoice.DocumentState = InvoiceState.FullyPaid;
                _invoice.Id = TObject.IsOBInvoice == true ? TObject.Id : TObject.IsWorkFlowInvoice == true ? TObject.WFDocumentId.Value : Guid.NewGuid();
                _invoice.InternalState = InvoiceState.Posted;
                isExeedLimitEdit = false;
                if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null)
                {
                    if (TObject.InvoiceDetailModels.Count > 0 || TObject.InvoiceDetailModels != null)
                        UpdateInvoiceDetails(TObject, _invoice);
                    if (TObject.InvoiceNoteModels != null && TObject.InvoiceNoteModels.Any())
                    {
                        foreach (InvoiceNoteModel note in TObject.InvoiceNoteModels)
                        {
                            InvoiceNoteCompact newNote = new InvoiceNoteCompact();
                            FillInvoiceNotes(TObject, note, newNote);
                        }
                    }
                }
                else
                {
                    if (TObject.IsWorkFlowInvoice == true && TObject.InvoiceDetailModels != null)
                        FillWorkflowInvoiceDeatils(TObject, _invoice);
                    else if (TObject.IsWorkFlowInvoice == true)
                        UpdateInvoiceDetails(TObject, _invoice);

                }
                _invoice.Status = RecordStatusEnum.Active;
                _invoice.UserCreated = TObject.UserCreated;
                _invoice.CreatedDate = DateTime.UtcNow;
                _invoice.ObjectState = ObjectState.Added;
                CompanyCompact company = _masterService.GetCompanyByCompanyid(TObject.CompanyId);

                if (TObject.IsWorkFlowInvoice == true)
                {
                    var lstInvoiceNumber = _invoiceService.GetInvoiceNumber(TObject.CompanyId, TObject.InvoiceNumber);
                    if (lstInvoiceNumber.Any())
                        throw new Exception(InvoiceValidation.Invoice_Number_already_exists);
                }
                else
                    _invoice.InvoiceNumber = TObject.IsWorkFlowInvoice == true ? TObject.InvoiceNumber : TObject.IsDocNoEditable != true ? GenerateAutoNumberForType(company.Id, DocTypeConstants.Invoice, company.ShortName) : TObject.DocNo;
                _invoice.DocNo = TObject.IsWorkFlowInvoice == true ? GetDocNumbers(TObject.CompanyId, TObject.DocNo) : _invoice.InvoiceNumber;
                _invoiceService.Insert(_invoice);
            }
            try
            {
                _unitOfWork.SaveChanges();
                if (TObject.IsOBInvoice != true)
                {
                    JVModel jvm = new JVModel();
                    FillJournal(jvm, _invoice, isNew, DocTypeConstants.Invoice);
                    jvm.DocumentState = InvoiceStates.NotPaid;
                    SaveInvoice1(jvm);
                }

                #region Cust_CreditLimit_Updation
                if (TObject.CustCreditlimit != null)
                {
                    SqlConnection con = new SqlConnection(ConnectionString);
                    if (con.State != ConnectionState.Open)
                        con.Open();
                    SqlCommand cmd = new SqlCommand("BC_UPDATE_ENTITY_CREDITTERMS", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EntityId", _invoice.EntityId.ToString());
                    cmd.Parameters.AddWithValue("@BaseAmount", isExeedLimitEdit == true ? TObject.CustCreditlimit : _invoice.GrandTotal);
                    cmd.Parameters.AddWithValue("@DocType", _invoice.DocType);
                    cmd.Parameters.AddWithValue("@CompanyId", _invoice.CompanyId);
                    cmd.Parameters.AddWithValue("@isEdit", isExeedLimitEdit);
                    cmd.Parameters.AddWithValue("@isVoid", false);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                #endregion

                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoice_SaveCall_SuccessFully_Message);
            }

            catch (DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
            return _invoice;
        }

        public Invoice SaveInvoiceDocumentVoid(DocumentVoidModel TObject, string ConnectionString)
        {
            string DocNo = "-V";
            bool? isVoid = true;
            string DocDescription = "Void-";
            //Invoice _document = _invoiceEntityService.Query(e => e.Id == TObject.Id && e.CompanyId == TObject.CompanyId && e.DocType == DocTypeConstants.Invoice).Select().FirstOrDefault();
            Invoice _document = _invoiceService.GetAllInvoiceLu(TObject.CompanyId, TObject.Id);
            try
            {
                if (TObject.IsDelete != true)
                {
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoiceDocumentVoid_SaveCall_Request_Message);
                    if (_document == null)
                        throw new Exception(InvoiceValidation.Invalid_Invoice);
                    if (_document.InternalState != InvoiceState.Recurring)
                        if (_document.DocumentState != InvoiceState.NotPaid)
                            throw new Exception(InvoiceValidation.State_should_be + InvoiceState.NotPaid);

                    //Need to verify the invoice is within Financial year
                    if (!_masterService.ValidateYearEndLockDate(_document.DocDate, TObject.CompanyId))
                    {
                        throw new Exception(InvoiceValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
                    }

                    //Verify if the invoice is out of open financial period and lock password is entered and valid
                    if (!_masterService.ValidateFinancialOpenPeriod(_document.DocDate, TObject.CompanyId))
                    {
                        if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                        {
                            throw new Exception(InvoiceValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                        }
                        else if (!_masterService.ValidateFinancialLockPeriodPassword(_document.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                        {
                            throw new Exception(InvoiceValidation.Invalid_Financial_Period_Lock_Password);
                        }
                    }
                    _document.DocNo = _document.DocNo + DocNo;
                    _document.DocDescription = DocDescription + _document.DocDescription;
                    _document.DocumentState = InvoiceState.Void;
                    if (_document.InternalState == InvoiceState.Recurring)
                        _document.NextDue = null;
                    //_document.InternalState = InvoiceState.Void;
                    _document.ModifiedDate = DateTime.UtcNow;
                    _document.ModifiedBy = _document.UserCreated;
                    _document.ObjectState = ObjectState.Modified;
                    _invoiceService.Update(_document);
                    try
                    {
                        _unitOfWork.SaveChanges();
                        if (_document.InternalState != InvoiceState.Recurring)
                        {
                            JournalSaveModel tObject = new JournalSaveModel();
                            tObject.Id = TObject.Id;
                            tObject.CompanyId = TObject.CompanyId;
                            tObject.DocNo = _document.DocNo;
                            tObject.ModifiedBy = TObject.ModifiedBy;
                            deleteJVPostInvoce(tObject);
                        }
                        #region Cust_CreditLimit_Updation
                        decimal? custCreditLimit = _masterService.GetCteditLimitsValue(_document.EntityId);
                        if (custCreditLimit != null)
                        {
                            SqlConnection con = new SqlConnection(ConnectionString);
                            if (con.State != ConnectionState.Open)
                                con.Open();
                            SqlCommand cmd = new SqlCommand("BC_UPDATE_ENTITY_CREDITTERMS", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EntityId", _document.EntityId.ToString());
                            cmd.Parameters.AddWithValue("@BaseAmount", _document.GrandTotal);
                            cmd.Parameters.AddWithValue("@DocType", _document.DocType);
                            cmd.Parameters.AddWithValue("@CompanyId", _document.CompanyId);
                            cmd.Parameters.AddWithValue("@isEdit", false);
                            cmd.Parameters.AddWithValue("@isVoid", isVoid);
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                        throw ex;
                    }
                    LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_SaveInvoiceDocumentVoid_SuccessFully_Message);
                }
                else
                {
                    //_document.Status = RecordStatusEnum.Delete;
                    _document.DocumentState = InvoiceState.Deleted;
                    _document.ObjectState = ObjectState.Modified;
                    _invoiceService.Update(_document);
                    try
                    {
                        _unitOfWork.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_SaveInvoiceDocumentVoid_SaveCall_Exception_Message);
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message + " for " + TObject.CompanyId);
                // Log.Logger.ZCritical(ex.StackTrace);
                throw ex;
            }

            return _document;
        }

        #endregion

        #region Private_Block
        private string GetAutoNumberByEntityType(long companyId, DateTime? lastInvoice, string entityType, AutoNumberCompact _autoNo, bool isInvoice, ref bool? isEdit)
        {
            string outPutNumber = null;
            //isEdit = false;

            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    if (entityType == InvoiceState.Recurring)
                        outPutNumber = GetRecInvoiceDocumentNumber(DocTypeConstant.Invoice, InvoiceState.Recurring, companyId);
                    else
                        outPutNumber = GetNewInvoiceDocumentNumber(entityType, companyId, isInvoice);
                    if (entityType == DocTypeConstant.CreditNote && outPutNumber == null)
                    {
                        outPutNumber = _autoNo.Preview;
                    }
                    isEdit = true;
                }
                else
                {
                    isEdit = false;
                    string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
                    string number = "1";
                    if (lastInvoice != null)
                    {
                        if (_autoNo.Format.Contains("{MM/YYYY}"))
                        {
                            //var lastCreatedInvoice = lstInvoice.FirstOrDefault();
                            if (lastInvoice.Value.Month != DateTime.UtcNow.Month)
                            {
                                //number = "1";
                                outPutNumber = autonoFormat + number.PadLeft(_autoNo.CounterLength.Value, '0');
                                //isExist = lstInvoice.Where(a => a.DocNo.Equals(outPutNumber)).Any();
                                //while (isExist)
                                //{
                                //    counter++;
                                //    outPutNumber = autonoFormat + counter.ToString().PadLeft(_autoNo.CounterLength.Value, '0');
                                //    if (lstInvoice.Where(a => a.DocNo.Equals(outPutNumber)).Any() == false)
                                //        isExist = false;
                                //}
                            }
                            else
                            {
                                string output = (Convert.ToInt32(_autoNo.GeneratedNumber) + 1).ToString();
                                outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                            }
                        }
                        else if (_autoNo.Format.Contains("{YYYY}"))
                        {
                            if (DateTime.UtcNow.Year == lastInvoice.Value.Year)
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
                        //string output = Convert.ToString(_autoNo.StartNumber);
                        outPutNumber = autonoFormat + output.PadLeft(_autoNo.CounterLength.Value, '0');
                        //counter = Convert.ToInt32(value);
                    }
                }
            }
            return outPutNumber;
        }

        private string GetAutoNumberForReceipt(long companyId, DateTime? lastInvoice, string entityType, AutoNumberCompact _autoNo, ref bool? isEdit)
        {
            string outPutNumber = null;
            if (_autoNo != null)
            {
                if (_autoNo.IsEditable == true)
                {
                    outPutNumber = GetNewReceiptDocumentNumber(companyId);
                    isEdit = true;
                }
                else
                {
                    isEdit = false;
                    string autonoFormat = _autoNo.Format.Replace("{YYYY}", DateTime.UtcNow.Year.ToString()).Replace("{MM/YYYY}", string.Format("{0:00}", DateTime.UtcNow.Month) + "/" + DateTime.UtcNow.Year.ToString());
                    string number = "1";
                    if (lastInvoice != null)
                    {
                        if (_autoNo.Format.Contains("{MM/YYYY}"))
                        {
                            if (lastInvoice.Value.Month != DateTime.UtcNow.Month)
                            {
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
                    }
                }
            }
            return outPutNumber;
        }

        private string GetRecInvoiceDocumentNumber(string docType, string internalStatel, long companyId)
        {
            Invoice invoice = _invoiceService.GetRecInvoiceByIStateAndCId(DocTypeConstant.Invoice, InvoiceState.Recurring, companyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (invoice != null)
            {
                string strOldNo = String.Empty;
                Invoice duplicatInvoice;
                int index;
                strOldDocNo = invoice.DocNo;

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

                    duplicatInvoice = _invoiceService.GetAllInvovoice(strNewDocNo, docType, companyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }

        private string GetNewInvoiceDocumentNumber(string docType, long CompanyId, bool isInvoice)
        {
            Invoice invoice = null;
            if (isInvoice == true)
            {
                invoice = _invoiceService.GetAllIvoiceByCidAndDocSubtype(docType, CompanyId, "General");
            }
            else
                invoice = _invoiceService.GetAllInvoiceByDoctypeAndCompanyId(docType, CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (invoice != null)
            {
                string strOldNo = String.Empty;
                Invoice duplicatInvoice;
                int index;
                strOldDocNo = invoice.DocNo;

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

                    duplicatInvoice = _invoiceService.GetAllInvovoice(strNewDocNo, docType, CompanyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }

        private string GetNewReceiptDocumentNumber(long CompanyId)
        {
            string invoice = _invoiceService.GetReceiptDocNo(CompanyId);
            string strOldDocNo = String.Empty, strNewDocNo = String.Empty, strNewNo = String.Empty;
            if (invoice != null)
            {
                string strOldNo = String.Empty;
                ReceiptCompact duplicatInvoice;
                int index;
                strOldDocNo = invoice;

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

                    duplicatInvoice = _invoiceService.GetDocNo(strNewDocNo, CompanyId);
                } while (duplicatInvoice != null);
            }
            return strNewDocNo;
        }

        private void FillInvoice(InvoiceModel invDTO, Invoice invoice)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_FillInvoice_FillCall_Request_Message);
                invDTO.Id = invoice.Id;
                invDTO.CompanyId = invoice.CompanyId;
                invDTO.EntityType = invoice.EntityType;
                invDTO.DocSubType = invoice.DocSubType;
                invDTO.DocType = invoice.DocType;
                invDTO.InvoiceNumber = invoice.InvoiceNumber;
                invDTO.DocNo = invoice.DocNo;
                invDTO.DocDescription = invoice.DocDescription;
                invDTO.DocDate = invoice.DocDate;
                invDTO.DueDate = invoice.DueDate;
                invDTO.PONo = invoice.PONo;
                invDTO.EntityId = invoice.EntityId;
                invDTO.EntityName = _masterService.GetEntityName(invoice.EntityId);
                invDTO.CreditTermsId = invoice.CreditTermsId;
                invDTO.CustCreditlimit = _masterService.GetCteditLimitsValue(invoice.EntityId);
                invDTO.Nature = invoice.Nature;
                invDTO.DocCurrency = invoice.DocCurrency;
                invDTO.ServiceCompanyId = invoice.ServiceCompanyId;

                invDTO.IsMultiCurrency = invoice.IsMultiCurrency;
                invDTO.BaseCurrency = invoice.ExCurrency;
                invDTO.ExchangeRate = invoice.ExchangeRate;
                invDTO.ExDurationFrom = invoice.ExDurationFrom;
                invDTO.ExDurationTo = invoice.ExDurationTo;

                invDTO.IsGstSettings = invoice.IsGstSettings;
                invDTO.GSTExCurrency = invoice.GSTExCurrency;
                invDTO.GSTExchangeRate = invoice.GSTExchangeRate;
                invDTO.GSTExDurationFrom = invoice.GSTExDurationFrom;
                invDTO.GSTExDurationTo = invoice.GSTExDurationTo;

                invDTO.IsSegmentReporting = invoice.IsSegmentReporting;
                invDTO.IsRepeatingInvoice = invoice.IsRepeatingInvoice;
                invDTO.RepEveryPeriodNo = invoice.RepEveryPeriodNo;
                invDTO.RepEveryPeriod = invoice.RepEveryPeriod;
                invDTO.EndDate = invoice.RepEndDate;
                invDTO.ParentInvoiceID = invoice.ParentInvoiceID;
                invDTO.GSTTotalAmount = invoice.GSTTotalAmount;
                invDTO.GrandTotal = invoice.GrandTotal;
                invDTO.Remarks = invoice.Remarks;
                invDTO.IsGSTApplied = invoice.IsGSTApplied;

                invDTO.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;

                invDTO.IsNoSupportingDocument = invoice.IsNoSupportingDocument;
                invDTO.NoSupportingDocument = invoice.NoSupportingDocs;
                invDTO.IsBaseCurrencyRateChanged = invoice.IsBaseCurrencyRateChanged;
                invDTO.IsGSTCurrencyRateChanged = invoice.IsGSTCurrencyRateChanged;
                invDTO.Status = invoice.Status;
                invDTO.DocumentState = invoice.DocumentState;
                invDTO.ModifiedDate = invoice.ModifiedDate;
                invDTO.ModifiedBy = invoice.ModifiedBy;
                invDTO.CreatedDate = invoice.CreatedDate;
                invDTO.UserCreated = invoice.UserCreated;
                invDTO.IsWorkFlowInvoice = invoice.IsWorkFlowInvoice;
                invDTO.CursorType = invoice.CursorType;
                invDTO.DocumentId = invoice.DocumentId;
                invDTO.InternalState = invoice.InternalState;
                invDTO.RecurInvId = invoice.RecurInvId;
                invDTO.IsOBInvoice = invoice.IsOBInvoice;
                invDTO.OpeningBalanceId = invoice.OpeningBalanceId;
                invDTO.AllocatedAmount = invoice.AllocatedAmount;
                List<ItemCompact> lstItem = _itemService.GetAllItemById(invoice.InvoiceDetails.Select(c => c.ItemId).ToList(), invoice.CompanyId);
                List<InvoiceDetailModel> lstDetail = new List<InvoiceDetailModel>();
                InvoiceDetailModel invDetail;
                foreach (var invD in invoice.InvoiceDetails)
                {
                    invDetail = new InvoiceDetailModel();
                    invDetail.TaxId = invD.TaxId;
                    invDetail.TaxRate = invD.TaxRate;
                    invDetail.Id = invD.Id;
                    invDetail.AmtCurrency = invD.AmtCurrency;
                    invDetail.BaseAmount = invD.BaseAmount;
                    invDetail.BaseTaxAmount = invD.BaseTaxAmount;
                    invDetail.BaseTotalAmount = invD.BaseTotalAmount;
                    invDetail.UnitPrice = invD.UnitPrice;
                    invDetail.Unit = invD.Unit;
                    invDetail.COAId = invD.COAId;
                    invDetail.Discount = invD.Discount;
                    invDetail.DiscountType = invD.DiscountType;
                    invDetail.DocAmount = invD.DocAmount;
                    invDetail.DocTaxAmount = invD.DocTaxAmount;
                    invDetail.DocTotalAmount = invD.DocTotalAmount;
                    invDetail.InvoiceId = invD.InvoiceId;
                    invDetail.ItemId = invD.ItemId;
                    invDetail.ItemDescription = (invD.ItemDescription != null || invD.ItemDescription != string.Empty) ? invD.ItemDescription : lstItem.Where(c => c.Id == invD.ItemId).Select(d => d.Description).FirstOrDefault();
                    invDetail.Qty = invD.Qty;
                    invDetail.Remarks = invD.Remarks;
                    invDetail.RecOrder = invD.RecOrder;
                    invDetail.TaxIdCode = invD.TaxIdCode;
                    lstDetail.Add(invDetail);
                }
                invDTO.InvoiceDetailModels = lstDetail.OrderBy(c => c.RecOrder).ToList();
                //invDTO.InvoiceNotes = _invoiceNoteService.GetInvoiceByid(invoice.Id).OrderByDescending(a => a.ModifiedDate).ThenByDescending(a => a.CreatedDate).ToList();
                invDTO.InvoiceNoteModels = invoice.InvoiceNotes.Select(c => new InvoiceNoteModel()
                {
                    Id = c.Id,
                    ExpectedPaymentDate = c.ExpectedPaymentDate,
                    Notes = c.Notes
                }).ToList();
                #region Commented_Code

                #endregion


            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_FillInvoice_FillCall_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }

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

        private void InsertInvoice(InvoiceModel TObject, Invoice invoiceNew)
        {
            try
            {
                invoiceNew.CompanyId = TObject.CompanyId;
                invoiceNew.DocType = DocTypeConstants.Invoice;
                invoiceNew.EntityType = "Customer";
                invoiceNew.DocDate = TObject.DocDate.Date;
                invoiceNew.DueDate = TObject.DueDate.Value.Date;
                invoiceNew.PONo = TObject.PONo;
                invoiceNew.EntityId = TObject.EntityId;
                invoiceNew.CreditTermsId = TObject.CreditTermsId;
                invoiceNew.Nature = TObject.Nature;
                invoiceNew.ServiceCompanyId = TObject.ServiceCompanyId;
                if (TObject.IsMultiCurrency == true)
                    invoiceNew.DocCurrency = TObject.DocCurrency;
                else
                    invoiceNew.DocCurrency = TObject.BaseCurrency;
                invoiceNew.IsMultiCurrency = TObject.IsMultiCurrency;
                invoiceNew.ExCurrency = TObject.BaseCurrency;
                invoiceNew.ExchangeRate = TObject.ExchangeRate;
                invoiceNew.ExDurationFrom = TObject.ExDurationFrom;
                invoiceNew.ExDurationTo = TObject.ExDurationTo;
                invoiceNew.IsGSTApplied = TObject.IsGSTApplied;
                invoiceNew.DocDescription = TObject.DocDescription;
                invoiceNew.IsGstSettings = TObject.IsGstSettings;
                invoiceNew.GSTExCurrency = TObject.GSTExCurrency;
                if (TObject.IsGstSettings)
                {
                    invoiceNew.GSTExchangeRate = TObject.GSTExchangeRate;
                    invoiceNew.GSTExDurationFrom = TObject.GSTExDurationFrom;
                    invoiceNew.GSTExDurationTo = TObject.GSTExDurationTo;
                }

                invoiceNew.IsRepeatingInvoice = TObject.IsRepeatingInvoice;
                if (TObject.IsRepeatingInvoice)
                {
                    invoiceNew.RepEveryPeriodNo = TObject.RepEveryPeriodNo;
                    invoiceNew.RepEveryPeriod = TObject.RepEveryPeriod;
                    if (TObject.EndDate == null)
                        invoiceNew.RepEndDate = null;
                    else
                        invoiceNew.RepEndDate = TObject.EndDate.Value.Date;
                    invoiceNew.ParentInvoiceID = TObject.ParentInvoiceID;
                }
                else
                {
                    invoiceNew.RepEveryPeriodNo = null;
                    invoiceNew.RepEveryPeriod = null;
                    invoiceNew.RepEndDate = null;
                    invoiceNew.ParentInvoiceID = null;
                }

                invoiceNew.BalanceAmount = TObject.GrandTotal;
                invoiceNew.GrandTotal = TObject.GrandTotal;
                invoiceNew.GSTTotalAmount = TObject.GSTTotalAmount;

                invoiceNew.IsAllowableNonAllowable = TObject.IsAllowableNonAllowable;

                invoiceNew.IsNoSupportingDocument = TObject.IsNoSupportingDocument;
                invoiceNew.NoSupportingDocs = TObject.NoSupportingDocument;
                invoiceNew.IsBaseCurrencyRateChanged = TObject.IsBaseCurrencyRateChanged;
                invoiceNew.IsGSTCurrencyRateChanged = TObject.IsGSTCurrencyRateChanged;
                invoiceNew.IsSegmentReporting = TObject.IsSegmentReporting;
                invoiceNew.Status = TObject.Status;
                invoiceNew.InvoiceNumber = TObject.InvoiceNumber;
                invoiceNew.DocumentState = invoiceNew.BalanceAmount == 0 ? InvoiceState.FullyPaid : String.IsNullOrEmpty(TObject.DocumentState) ? InvoiceState.NotPaid : TObject.DocumentState;
                invoiceNew.IsWorkFlowInvoice = TObject.IsWorkFlowInvoice;
                invoiceNew.CursorType = TObject.CursorType;
                invoiceNew.DocumentId = TObject.DocumentId;
                invoiceNew.IsOBInvoice = TObject.IsOBInvoice;
                invoiceNew.OpeningBalanceId = TObject.OpeningBalanceId;
            }
            catch (Exception ex)
            {

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private static void FillInvoiceNotes(InvoiceModel TObject, InvoiceNoteModel note, InvoiceNoteCompact newNote)
        {
            newNote.Id = Guid.NewGuid();
            newNote.InvoiceId = TObject.Id;
            newNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
            newNote.Notes = note.Notes;
            newNote.CreatedDate = DateTime.UtcNow;
            newNote.UserCreated = TObject.UserCreated;
            newNote.ObjectState = ObjectState.Added;
        }

        public void UpdateInvoiceDetails(InvoiceModel TObject, Invoice _invoiceNew)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Request_Message);
                if (TObject.InvoiceDetailModels != null)
                {
                    foreach (InvoiceDetailModel detail in TObject.InvoiceDetailModels)
                    {
                        if (detail.RecordStatus == "Added")
                        {
                            InvoiceDetail detailNew = new InvoiceDetail();
                            FillInvoiceDetails(TObject, _invoiceNew, detail, detailNew);
                            detailNew.Id = Guid.NewGuid();
                            detailNew.RecOrder = ++recorder;
                            //recorder = detail.RecOrder;
                            //detailNew.InvoiceId = _invoiceNew.Id;
                            //detailNew.TaxIdCode = detail.TaxIdCode;
                            //detailNew.DocAmount = detail.DocAmount;
                            //detailNew.DocTaxAmount = detail.DocTaxAmount;
                            //detailNew.BaseAmount = TObject.ExchangeRate != null ? Math.Round(detail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocAmount;
                            //detailNew.BaseTaxAmount = TObject.ExchangeRate != null ? detail.DocTaxAmount != null ? Math.Round((decimal)detail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : detail.DocTaxAmount : detail.DocTaxAmount;
                            //detailNew.BaseTotalAmount = Math.Round((decimal)detail.BaseAmount + (detail.BaseTaxAmount != null ? (decimal)detail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                            detailNew.ObjectState = ObjectState.Added;
                            _invoiceNew.InvoiceDetails.Add(detailNew);
                        }
                        else if (detail.RecordStatus != "Added" && detail.RecordStatus != "Deleted")
                        {
                            InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                            if (invoiceDetail != null)
                            {
                                FillInvoiceDetails(TObject, _invoiceNew, detail, invoiceDetail);
                                invoiceDetail.RecOrder = recorder + 1;
                                recorder = invoiceDetail.RecOrder;
                                invoiceDetail.ObjectState = ObjectState.Modified;
                            }
                        }
                        else if (detail.RecordStatus == "Deleted")
                        {
                            InvoiceDetail invoiceDetail = _invoiceNew.InvoiceDetails.Where(a => a.Id == detail.Id).FirstOrDefault();
                            if (invoiceDetail != null)
                            {
                                invoiceDetail.ObjectState = ObjectState.Deleted;
                            }
                        }
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_SuccessFully_Message);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void FillInvoiceDetails(InvoiceModel TObject, Invoice _invoiceNew, InvoiceDetailModel detail, InvoiceDetail invoiceDetail)
        {
            invoiceDetail.InvoiceId = _invoiceNew.Id;
            invoiceDetail.ItemId = detail.ItemId;
            invoiceDetail.ItemCode = detail.ItemCode;
            invoiceDetail.ItemDescription = detail.ItemDescription;
            invoiceDetail.Qty = detail.Qty;
            invoiceDetail.Unit = detail.Unit;
            invoiceDetail.UnitPrice = detail.UnitPrice;
            invoiceDetail.DiscountType = detail.DiscountType;
            invoiceDetail.Discount = detail.Discount;
            invoiceDetail.COAId = detail.COAId;
            invoiceDetail.TaxId = detail.TaxId;
            invoiceDetail.TaxRate = detail.TaxRate;
            invoiceDetail.DocTaxAmount = detail.DocTaxAmount;
            invoiceDetail.DocAmount = detail.DocAmount;
            invoiceDetail.AmtCurrency = detail.AmtCurrency;
            invoiceDetail.DocTotalAmount = detail.DocTotalAmount;
            invoiceDetail.Remarks = detail.Remarks;
            invoiceDetail.TaxIdCode = detail.TaxIdCode;
            invoiceDetail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(invoiceDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocAmount;
            invoiceDetail.BaseTaxAmount = TObject.ExchangeRate != null ? invoiceDetail.DocTaxAmount != null ? Math.Round((decimal)invoiceDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocTaxAmount : detail.DocTaxAmount;
            invoiceDetail.BaseTotalAmount = Math.Round((decimal)invoiceDetail.BaseAmount + (invoiceDetail.BaseTaxAmount != null ? (decimal)invoiceDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
        }

        public void UpdateInvoiceNotes(InvoiceModel TObject, Invoice _invoiceNew)
        {
            try
            {
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Request_Message);
                if (TObject.InvoiceNoteModels != null)
                {
                    foreach (InvoiceNoteModel note in TObject.InvoiceNoteModels)
                    {
                        if (note.RecordStatus == "Added")
                        {
                            InvoiceNoteCompact newNote = new InvoiceNoteCompact();
                            newNote.Id = Guid.NewGuid();
                            newNote.InvoiceId = TObject.Id;
                            newNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
                            newNote.Notes = note.Notes;
                            newNote.CreatedDate = DateTime.UtcNow;
                            newNote.UserCreated = TObject.UserCreated;
                            newNote.ObjectState = ObjectState.Added;
                            _invoiceNew.InvoiceNotes.Add(newNote);
                        }
                        else if (note.RecordStatus != "Added" && note.RecordStatus != "Deleted")
                        {
                            InvoiceNoteCompact invoiceNote = _invoiceNew.InvoiceNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                            if (invoiceNote != null)
                            {
                                invoiceNote.InvoiceId = TObject.Id;
                                invoiceNote.ExpectedPaymentDate = note.ExpectedPaymentDate;
                                invoiceNote.Notes = note.Notes;
                                invoiceNote.ModifiedDate = DateTime.UtcNow;
                                invoiceNote.ModifiedBy = note.ModifiedBy;
                                invoiceNote.ObjectState = ObjectState.Modified;
                            }
                        }
                        else if (note.RecordStatus == "Deleted")
                        {
                            InvoiceNoteCompact invoiceNote = _invoiceNew.InvoiceNotes.Where(a => a.Id == note.Id).FirstOrDefault();
                            if (invoiceNote != null)
                            {
                                invoiceNote.ObjectState = ObjectState.Deleted;
                            }
                        }
                        LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Exception_Message);
                    }
                }
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceNotes_Update_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void FillWorkflowInvoiceDeatils(InvoiceModel TObject, Invoice _invoiceNew)
        {
            try
            {
                int? recorder = 0;
                LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService, InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Request_Message);
                //List<long> lstCoaId = TObject.InvoiceDetailModels.Select(c => c.COAId).ToList();
                //List<ChartOfAccount> lstCoa = _chartOfAccountService.GetAllCOAById(TObject.CompanyId, lstCoaId);

                List<long?> lstTaxId = TObject.IsGstSettings == true ? TObject.InvoiceDetailModels.Where(a => a.TaxIdCode == null).Select(a => a.TaxId).ToList() : null;
                Dictionary<long, string> lstTaxs = lstTaxId != null ? _masterService.GetTaxCodes(lstTaxId, 0) : null;

                foreach (InvoiceDetailModel detail in TObject.InvoiceDetailModels)
                {
                    InvoiceDetail invoiceDetails = _invoiceNew.InvoiceDetails.Where(a => a.InvoiceId == detail.Id).FirstOrDefault();
                    if (invoiceDetails == null)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();
                        invoiceDetail.Id = Guid.NewGuid();
                        invoiceDetail.InvoiceId = _invoiceNew.Id;
                        invoiceDetail.ItemId = detail.ItemId;
                        invoiceDetail.ItemCode = detail.ItemCode;
                        invoiceDetail.ItemDescription = detail.ItemDescription;
                        invoiceDetail.Qty = detail.Qty;
                        invoiceDetail.Unit = detail.Unit;
                        invoiceDetail.UnitPrice = detail.UnitPrice;
                        invoiceDetail.COAId = detail.COAId;
                        invoiceDetail.TaxId = detail.TaxId;
                        invoiceDetail.TaxRate = detail.TaxRate;
                        if (TObject.IsGstSettings == true && detail.TaxIdCode == null)
                        {
                            invoiceDetail.TaxIdCode = lstTaxs != null ? lstTaxs.Where(a => a.Key == detail.TaxId).Select(a => a.Value != "NA" ? (a.Value + "-" + invoiceDetail.TaxRate + "%") : "NA").FirstOrDefault() : detail.TaxIdCode;
                        }
                        else
                            invoiceDetail.TaxIdCode = detail.TaxIdCode;
                        invoiceDetail.DocTaxAmount = detail.DocTaxAmount == null ? 0 : detail.DocTaxAmount;
                        invoiceDetail.DocAmount = detail.DocAmount;
                        invoiceDetail.AmtCurrency = detail.AmtCurrency;
                        invoiceDetail.DocTotalAmount = detail.DocTotalAmount;
                        invoiceDetail.BaseAmount = TObject.ExchangeRate != null ? Math.Round(invoiceDetail.DocAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocAmount;
                        invoiceDetail.BaseTaxAmount = TObject.ExchangeRate != null ? invoiceDetail.DocTaxAmount != null ? Math.Round((decimal)invoiceDetail.DocTaxAmount * (decimal)TObject.ExchangeRate, 2, MidpointRounding.AwayFromZero) : invoiceDetail.DocTaxAmount : invoiceDetail.DocTaxAmount;
                        invoiceDetail.BaseTotalAmount = Math.Round((decimal)invoiceDetail.BaseAmount + (invoiceDetail.BaseTaxAmount != null ? (decimal)invoiceDetail.BaseTaxAmount : 0), 2, MidpointRounding.AwayFromZero);
                        invoiceDetail.RecOrder = recorder + 1;
                        recorder = invoiceDetail.RecOrder;
                        invoiceDetail.ObjectState = ObjectState.Added;
                        //_invoiceService.Insert(invoiceDetail);
                    }

                }
            }
            catch (Exception ex)
            {
                //LoggingHelper.LogMessage(InvoiceLoggingValidation.InvoiceApplicationService,InvoiceLoggingValidation.Log_UpdateInvoiceDetails_Update_Exception_Message);
                //Log.Logger.ZCritical(ex.StackTrace);
                //throw ex;

                LoggingHelper.LogError(InvoiceLoggingValidation.InvoiceApplicationService, ex, ex.Message);
                throw ex;
            }
        }

        private void InvoiceValidations(InvoiceModel TObject)
        {
            if (TObject.EntityId == null)
            {
                throw new Exception(InvoiceValidation.Entity_is_mandatory);
            }
            if (TObject.IsWorkFlowInvoice == true && TObject.InvoiceDetailModels.Where(c => c.TaxId != null && c.TaxIdCode != "NA").ToList().Any(c => c.TaxRate == null))
                throw new Exception(InvoiceValidation.Tax_rate_missing_for_required_line_items);
            if (TObject.DocDate == null)
            {
                throw new Exception(InvoiceValidation.Invalid_Document_Date);
            }

            if (TObject.ServiceCompanyId == null)
                throw new Exception(InvoiceValidation.Service_Company_Is_Mandatory);

            if (TObject.DueDate == null || TObject.DueDate < TObject.DocDate)
            {
                throw new Exception(InvoiceValidation.Invalid_Due_Date);
            }
            if (TObject.IsOBInvoice != true)
                if (TObject.CreditTermsId == null)
                {
                    throw new Exception(InvoiceValidation.Terms_Payment_is_mandatory);
                }
            if (TObject.IsWorkFlowInvoice != true)
            {
                if (TObject.IsDocNoEditable == true)
                {
                    if (_invoiceService.GetRecurringDocNo(TObject.CompanyId, TObject.Id, InvoiceState.Posted, TObject.DocNo))
                    {
                        throw new Exception(InvoiceValidation.Document_number_already_exist);
                    }
                }
            }
            if (TObject.GrandTotal < 0)
            {
                throw new Exception(InvoiceValidation.Grand_Total_should_be_greater_than_zero);
            }
            if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null)
            {
                if (TObject.InvoiceDetails.Any())
                {
                    foreach (var invoice in TObject.InvoiceDetails)
                    {
                        if (invoice.ItemCode != null && invoice.Qty == null)
                            throw new Exception(InvoiceValidation.Please_Enter_Quantity);
                        if (invoice.ItemCode == null && invoice.Qty != null)
                            throw new Exception(InvoiceValidation.Please_Select_Item);
                    }
                }
            }
            if (TObject.IsOBInvoice != true)
                if (TObject.CreditTermsId == null || TObject.CreditTermsId == 0)
                {
                    throw new Exception(InvoiceValidation.Terms_of_Payment_is_mandatory);
                }
            if (TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null)
            {
                if (TObject.InvoiceDetails == null || TObject.InvoiceDetails.Count == 0)
                {
                    throw new Exception(InvoiceValidation.Atleast_one_Sale_Item_is_required_in_the_Invoice);
                }
                else
                {
                    int itemCount = TObject.InvoiceDetails.Where(a => a.RecordStatus != "Deleted").Count();
                    if (itemCount == 0)
                    {
                        throw new Exception(InvoiceValidation.Atleast_one_Sale_Item_is_required_in_the_Invoice);
                    }
                }
            }

            if ((TObject.IsWorkFlowInvoice == false || TObject.IsWorkFlowInvoice == null) && TObject.IsRepeatingInvoice && (TObject.RepEveryPeriod == null || TObject.RepEveryPeriodNo == null))
            {
                throw new Exception(InvoiceValidation.Repeating_Invoice_fields_should_be_entered);
            }

            if (TObject.ExchangeRate == 0)
                throw new Exception(InvoiceValidation.ExchangeRate_Should_Be_Grater_Than_Zero);

            if (TObject.GSTExchangeRate == 0)
                throw new Exception(InvoiceValidation.GSTExchangeRate_Should_Be_Grater_Than_Zero);

            //Need to verify the invoice is within Financial year
            if (!_masterService.ValidateYearEndLockDate(TObject.DocDate, TObject.CompanyId))
            {
                throw new Exception(InvoiceValidation.Transaction_date_is_in_closed_financial_period_and_cannot_be_posted);
            }

            //Verify if the invoice is out of open financial period and lock password is entered and valid
            if (TObject.IsOBInvoice != true)
                if (!_masterService.ValidateFinancialOpenPeriod(TObject.DocDate.Date, TObject.CompanyId))
                {
                    if (String.IsNullOrEmpty(TObject.PeriodLockPassword))
                    {
                        throw new Exception(InvoiceValidation.Transaction_date_is_in_locked_accounting_period_and_cannot_be_posted);
                    }
                    else if (!_masterService.ValidateFinancialLockPeriodPassword(TObject.DocDate, TObject.PeriodLockPassword, TObject.CompanyId))
                    {
                        throw new Exception(InvoiceValidation.Invalid_Financial_Period_Lock_Password);
                    }
                }
        }

        private static string InvoiceCommonQuery(string username, long companyid, DateTime date, long? credit, long? comp, string currencyCode)
        {
            return $"SELECT 'CURRENCYEDIT' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY FROM Bean.Currency WHERE CompanyId={ companyid  } AND (Status=1 OR Code='{currencyCode}' OR DefaultValue='SGD');SELECT 'CURRENCYNEW' AS TABLENAME,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,ID AS ID,CODE AS CODE,Name AS NAME,RecOrder AS RECORDER,'' as CATEGORY  FROM Bean.Currency WHERE CompanyId={ companyid } AND (Status=1 OR DefaultValue='SGD');SELECT 'TERMSOFPAY' as TABLENAME,Id as ID,Name as NAME,TOPValue as TOPVALUE,RecOrder as RECORDER,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,'' as CODE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,1 as STATUS,'' as CATEGORY FROM Common.TermsOfPayment where CompanyId={ companyid } AND (Status=1 OR Id= { credit } AND IsCustomer=1);SELECT 'SERVICECOMPANY' as TABLENAME,C.Id as ID,c.Name as NAME,c.ShortName as SHOTNAME,c.IsGstSetting as IsGstActive,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,'' as CURRENCY,'' as Class,'' as CODE,1 as STATUS,0 as RECORDER,'' as CATEGORY FROM Common.Company c JOIN Common.CompanyUser CU on C.ParentId=CU.CompanyId where (c.Status = 1 or c.Id = { comp }) and c.ParentId ={companyid } and CU.Username='{ username }';SELECT 'CHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY FROM Bean.AccountType A JOIN Bean.ChartOfAccount COA on A.Id = COA.AccountTypeId where COA.CompanyId ={ companyid } and a.Name in ('Revenue','Other income') and COA.IsRealCOA=1;SELECT 'TAXCODE' as TABLENAME,Id as ID,Code as TXCODE,Name as NAME,TaxRate as TAXRATE,TaxType as TAXTYPE,Status as STATUS,0 as TOPVALUE,'' as CURRENCY,'' as Class,0 as IsGstActive,'' as SHOTNAME,0 as RECORDER,'' as CODE,'' as CATEGORY FROM Bean.TaxCode where CompanyId=0 and Status<3 and EffectiveFrom<='{ String.Format("{0:MM/dd/yyyy}'", date) } and (EffectiveTo>='{ String.Format("{0:MM/dd/yyyy}", date) }' OR EffectiveTo is null);SELECT 'OBCHARTOFACCOUNT' as TABLENAME,COA.Id as ID,COA.Name as NAME,COA.RecOrder as RECORDER,COA.Code as CODE,COA.Category as Category,COA.Currency as CURRENCY,COA.Class as Class,COA.Status as STATUS,0 as TAXRATE,'' as TAXTYPE,'' as TXCODE,0 as TOPVALUE,0 as IsGstActive,'' as SHOTNAME,COA.Category as CATEGORY FROM Bean.ChartOfAccount COA where COA.CompanyId ={ companyid } and COA.Name='{ COANameConstants.Opening_balance }';";
        }

        #endregion Private_Block

        #region Auto_Number
        public string GetDocNumbers(long companyId, string docNumber)
        {
            string docNo = docNumber;
            int i = 0;
            bool isBreak = false;
            List<Invoice> lstInvoices = _invoiceService.GetDocNumber(companyId, docNumber);
            if (lstInvoices.Any())
            {
                while (isBreak == false)
                {
                    i++;
                    docNo = docNumber + "-" + i;
                    var inc = lstInvoices.Where(a => a.DocNo == docNo).Select(a => a.DocNo).FirstOrDefault();
                    if (inc == null)
                    {
                        isBreak = true;
                    }
                }
            }
            return docNo;
        }
        string value = "";
        public string GenerateAutoNumberForType(long CompanyId, string Type, string companyCode)
        {
            AutoNumberCompact _autoNo = _autoNumberService.GetAutoNumber(CompanyId, Type);
            string generatedAutoNumber = "";

            if (Type == DocTypeConstants.Invoice || Type == DocTypeConstants.DoubtFulDebitNote || Type == DocTypeConstants.CreditNote || Type == DocTypeConstants.Provision)
            {
                generatedAutoNumber = GenerateFromFormat(_autoNo.EntityType, _autoNo.Format, Convert.ToInt32(_autoNo.CounterLength), _autoNo.GeneratedNumber, CompanyId, companyCode);

                if (_autoNo != null)
                {
                    _autoNo.GeneratedNumber = value;
                    _autoNo.IsDisable = true;
                    _autoNo.ObjectState = ObjectState.Modified;
                    _autoNumberService.Update(_autoNo);
                }
                var _autonumberCompany = _autoNumberService.GetAutoNumberCompany(_autoNo.Id);
                if (_autonumberCompany.Any())
                {
                    AutoNumberComptCompany _autoNumberCompanyNew = _autonumberCompany.FirstOrDefault();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.ObjectState = ObjectState.Modified;
                    //_autoNumberService.Update(_autoNumberCompanyNew);
                }
                else
                {
                    AutoNumberComptCompany _autoNumberCompanyNew = new AutoNumberComptCompany();
                    _autoNumberCompanyNew.GeneratedNumber = value;
                    _autoNumberCompanyNew.AutonumberId = _autoNo.Id;
                    _autoNumberCompanyNew.Id = Guid.NewGuid();
                    _autoNumberCompanyNew.ObjectState = ObjectState.Added;
                    //_autoNumberService.Insert(_autoNumberCompanyNew);
                }
            }
            return generatedAutoNumber;
        }
        public string GenerateFromFormat(string Type, string companyFormatFrom, int counterLength, string IncreamentVal, long companyId, string Companycode = null)
        {
            List<Invoice> lstInvoices = null;
            bool ifMonthcontains = false;
            int currentMonth = 0;
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
                ifMonthcontains = true;
            }
            else if (companyFormatHere.Contains("{COMPANYCODE}"))
            {
                companyFormatHere = companyFormatHere.Replace("{COMPANYCODE}", Companycode);
            }
            double val = 0;
            if (Type == DocTypeConstants.Invoice)
            {
                lstInvoices = _invoiceService.GetCompanyIdAndDocType(companyId);

                if (lstInvoices.Any() && ifMonthcontains)
                {
                    AutoNumberCompact autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                    int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
                    if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
                    {
                        if (currentMonth == lastInvCreatedMonth)
                        {
                            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                        }
                        else
                        { val = 1; }
                    }
                    else
                        val = 1;
                }
                else
                {
                    if (lstInvoices.Any())
                    {
                        AutoNumberCompact autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
                        #region Commented Code
                        //foreach (var invoice in lstInvoices)
                        //{
                        //    if (invoice.InvoiceNumber != autonumber.Preview)
                        //        val = Convert.ToInt32(IncreamentVal);
                        //    else
                        //    {
                        //        val = Convert.ToInt32(IncreamentVal) + 1;
                        //        break;
                        //    }
                        //}
                        #endregion

                        val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
                    }
                    else
                    {
                        val = Convert.ToInt32(IncreamentVal);
                    }
                }
            }
            //else if (Type == /*DocTypeConstants.DoubtFulDebitNote*/"Debt Provision")
            //{
            //    lstInvoices = _invoiceEntityService.GetCompanyIdByDoubtFulDbt(companyId);

            //    if (lstInvoices.Any() && ifMonthcontains)
            //    {
            //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
            //        int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
            //        if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
            //        {
            //            if (currentMonth == lastInvCreatedMonth)
            //            {
            //                val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
            //            }
            //            else
            //            {
            //                val = 1;
            //            }
            //        }
            //        else
            //        { val = 1; }

            //    }
            //    else
            //    {
            //        if (lstInvoices.Any())
            //        {
            //            AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
            //            #region Commented Code
            //            //foreach (var invoice in lstInvoices)
            //            //{
            //            //    if (invoice.InvoiceNumber != autonumber.Preview)
            //            //        val = Convert.ToInt32(IncreamentVal);
            //            //    else
            //            //    {
            //            //        val = Convert.ToInt32(IncreamentVal) + 1;
            //            //        break;
            //            //    }
            //            //}
            //            #endregion

            //            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
            //        }
            //        else
            //        {
            //            val = Convert.ToInt32(IncreamentVal);
            //        }
            //    }

            //}
            //else if (Type == DocTypeConstants.CreditNote)
            //{
            //    lstInvoices = _invoiceEntityService.GetCompanyIdByCreditNote(companyId);

            //    if (lstInvoices.Any() && ifMonthcontains)
            //    {
            //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);

            //        int? lastInvCreatedMonth = lstInvoices.Select(a => a.CreatedDate.Value.Month).FirstOrDefault();
            //        if (DateTime.Now.Year == lstInvoices.Select(a => a.CreatedDate.Value.Year).FirstOrDefault())
            //        {
            //            if (currentMonth == lastInvCreatedMonth)
            //            {
            //                val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
            //            }
            //            else
            //            {
            //                val = 1;
            //            }
            //        }
            //        else
            //            val = 1;
            //    }
            //    else
            //    {
            //        if (lstInvoices.Any())
            //        {
            //            AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
            //            #region commented Code
            //            //foreach (var invoice in lstInvoices)
            //            //{
            //            //    if (invoice.InvoiceNumber != autonumber.Preview)
            //            //        val = Convert.ToInt32(IncreamentVal);
            //            //    else
            //            //    {
            //            //        val = Convert.ToInt32(IncreamentVal) + 1;
            //            //        break;
            //            //    }
            //            //}
            //            #endregion

            //            val = GetValue(IncreamentVal, lstInvoices, val, autonumber);
            //        }
            //        else
            //        {
            //            val = Convert.ToInt32(IncreamentVal);
            //        }
            //    }
            //}
            //else if (Type == DocTypeConstants.Provision)
            //{
            //    List<Provision> lstProvisions = _provisionService.lstInvoiceProvision(companyId);
            //    if (lstProvisions.Any())
            //    {
            //        AppsWorld.InvoiceModule.Entities.AutoNumber autonumber = _autoNumberService.GetAutoNumber(companyId, Type);
            //        foreach (var provision in lstProvisions)
            //        {
            //            if (provision.SystemRefNo != autonumber.Preview)
            //                val = Convert.ToInt32(IncreamentVal);
            //            else
            //            {
            //                val = Convert.ToInt32(IncreamentVal) + 1;
            //                break;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        val = Convert.ToInt32(IncreamentVal);
            //    }
            //}
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

            if (lstInvoices.Any())
            {
                OutputNumber = GetNewNumber(lstInvoices, Type, OutputNumber, counter, companyFormatHere, counterLength);
            }

            return OutputNumber;
        }
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
        private string GetNewNumber(List<Invoice> lstInvoice, string type, string outputNumber, string counter, string format, int counterLength)
        {
            string val1 = outputNumber;
            string val2 = "";
            var invoice = lstInvoice.Where(a => a.InvoiceNumber == outputNumber).FirstOrDefault();
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
                    var inv = lstInvoice.Where(c => c.InvoiceNumber == val2).FirstOrDefault();
                    if (inv == null)
                        isNotexist = true;
                }
                val1 = val2;
            }
            return val1;
        }

        #endregion Auto_Number

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
        private void FillJournal(JVModel headJournal, Invoice invoice, bool isNew, string type)
        {
            decimal? baseAmount = 0;
            fillJV(headJournal, invoice, type);
            headJournal.COAId = _masterService.GetChartOfAccountByNature(invoice.Nature, invoice.CompanyId);
            if (isNew)
                headJournal.Id = Guid.NewGuid();
            else
                headJournal.Id = invoice.Id;
            List<JVVDetailModel> lstJD = new List<JVVDetailModel>();
            JVVDetailModel jModel = new JVVDetailModel();
            List<TaxCodeCompact> lstTaxCode = null;
            int? recOreder = 0;
            if (invoice.IsGstSettings)
            {
                List<long?> lstTaxIds = invoice.InvoiceDetails.Select(c => c.TaxId).ToList();
                if (lstTaxIds.Any())
                {
                    lstTaxCode = _masterService.GetAllTaxById(lstTaxIds);
                }
            }
            foreach (InvoiceDetail detail in invoice.InvoiceDetails)
            {
                JVVDetailModel journal = new JVVDetailModel();
                FillJVDetail(journal, invoice, detail, type, lstTaxCode);
                if (isNew)
                    journal.Id = Guid.NewGuid();
                else
                    journal.Id = detail.Id;
                journal.COAId = detail.COAId;
                journal.RecOrder = detail.RecOrder;

                //journal.RecOrder = recOreder + 1;
                //recOreder = journal.RecOrder;
                lstJD.Add(journal);
            }
            //if (invoice.IsWorkFlowInvoice == false || invoice.IsWorkFlowInvoice == null)
            //{
            if (invoice.IsGstSettings)
            {
                //List<TaxCodeCompact> lstTaxCodes = _masterService.GetTaxCodes(0);
                foreach (InvoiceDetail detail in invoice.InvoiceDetails.Where(c => c.TaxRate != null && c.TaxIdCode != "NA"))
                {
                    JVVDetailModel journal = new JVVDetailModel();
                    FillJVGstDetail(journal, detail, invoice, type, lstTaxCode);
                    if (isNew)
                        journal.Id = Guid.NewGuid();
                    else
                        journal.Id = detail.Id;
                    //journal.RecOrder = recOreder + 1;
                    //recOreder = journal.RecOrder;
                    journal.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
                    lstJD.Add(journal);
                }
            }
            //}
            if (type == DocTypeConstants.Invoice)
            {
                baseAmount = lstJD.Sum(c => c.BaseCredit);
                headJournal.GrandBaseDebitTotal = baseAmount;
            }
            else if (type == DocTypeConstants.CreditNote)
            {
                baseAmount = lstJD.Sum(c => c.BaseDebit);
                headJournal.GrandBaseCreditTotal = baseAmount;
            }
            FillJVHeadDetail(jModel, invoice, type);
            if (type != DocTypeConstants.CreditNote)
                jModel.AmountDue = (invoice.DocumentState != InvoiceState.NotPaid /*|| invoice.DocumentState != InvoiceState.NotApplied*/) ? headJournal.BalanceAmount : null;
            //if (type == DocTypeConstants.Invoice)
            //{
            //    jModel.BaseDebit = baseAmount;
            //}
            //if (type == DocTypeConstants.CreditNote)
            //{
            //    jModel.BaseCredit = baseAmount;
            //}
            jModel.RecOrder = lstJD.Max(c => c.RecOrder) + 1;
            jModel.COAId = headJournal.COAId;
            recOreder = jModel.RecOrder;
            lstJD.Add(jModel);
            headJournal.JVVDetailModels = lstJD.Where(a => a.DocCredit > 0 || a.DocDebit > 0).OrderBy(x => x.RecOrder).ToList();
        }
        private void FillJVGstDetail(JVVDetailModel journal, InvoiceDetail detail, Invoice invoice, string type, List<TaxCodeCompact> lstTaxCodes)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.DocNo = invoice.DocNo;
            journal.DocType = invoice.DocType;
            journal.DocSubType = invoice.DocSubType;
            journal.PostingDate = invoice.DocDate;
            journal.COAId = _masterService.GetTaxPaybleGstCOA(invoice.CompanyId, COANameConstants.TaxPayableGST);
            journal.DocCurrency = invoice.DocCurrency;
            journal.BaseCurrency = invoice.ExCurrency;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.EntityId = invoice.EntityId;
            journal.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            journal.AccountDescription = invoice.DocDescription;
            if (detail.TaxId != null)
            {
                //TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
                TaxCodeCompact tax = lstTaxCodes.Where(a => a.Id == detail.TaxId).FirstOrDefault();
                journal.TaxId = tax.Id;
                journal.TaxCode = tax.Code;
                journal.TaxRate = tax.TaxRate;
                journal.TaxType = tax.TaxType;
            }
            if (type == DocTypeConstants.Invoice)
            {

                journal.DocCredit = detail.DocTaxAmount.Value;
                journal.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocCredit : (decimal)(journal.DocCredit * invoice.ExchangeRate), 2);
            }
            else if (type == DocTypeConstants.CreditNote)
            {
                journal.DocDebit = detail.DocTaxAmount.Value;
                journal.BaseDebit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * invoice.ExchangeRate), 2);
            }
            journal.IsTax = true;
        }
        private void FillJVDetail(JVVDetailModel journal, Invoice invoice, InvoiceDetail detail, string type, List<TaxCodeCompact> lstTaxCode)
        {
            journal.DocumentDetailId = detail.Id;
            journal.DocumentId = invoice.Id;
            journal.Nature = invoice.Nature;
            journal.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            journal.DocNo = invoice.DocNo;
            journal.DocDate = invoice.DocDate;
            journal.DocType = invoice.DocType;
            journal.PostingDate = invoice.DocDate;
            journal.EntityId = invoice.EntityId;
            journal.SystemRefNo = invoice.InvoiceNumber;
            journal.DocSubType = invoice.DocSubType;
            if (detail.ItemId != null)
            {
                journal.ItemId = detail.ItemId.Value;
                journal.ItemCode = detail.ItemCode;
                journal.AccountDescription = detail.ItemDescription;
            }
            journal.AccountDescription = detail.ItemDescription;
            journal.Qty = detail.Qty.Value;
            journal.Unit = detail.Unit;
            journal.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;

            journal.UnitPrice = detail.UnitPrice == null ? detail.DocTotalAmount : detail.UnitPrice.Value;


            journal.Discount = detail.Discount == null ? 0.0 : detail.Discount;
            journal.DiscountType = detail.DiscountType;
            //journal.AllowDisAllow = detail.AllowDisAllow;
            journal.DocCurrency = invoice.DocCurrency;
            journal.BaseCurrency = invoice.ExCurrency;
            journal.ExchangeRate = invoice.ExchangeRate;
            journal.GSTExCurrency = invoice.GSTExCurrency;
            journal.GSTExchangeRate = invoice.GSTExchangeRate;
            //journal.AccountDescription = invoice.Remarks;
            if (detail.TaxId != null && lstTaxCode.Any())
            {
                TaxCodeCompact tax = lstTaxCode.Where(c => c.Id == detail.TaxId).FirstOrDefault();
                journal.TaxId = tax.Id;
                journal.TaxCode = tax.Code;
                journal.TaxRate = tax.TaxRate;
                journal.TaxType = tax.TaxType;
            }
            if (type == DocTypeConstants.Invoice)
            {
                journal.DocCredit = detail.DocAmount;
                journal.BaseCredit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocCredit : (decimal)(journal.DocCredit * invoice.ExchangeRate), 2);
            }
            else if (type == DocTypeConstants.CreditNote)
            {

                journal.DocDebit = detail.DocAmount;
                journal.BaseDebit = Math.Round((decimal)invoice.ExchangeRate == null ? (decimal)journal.DocDebit : (decimal)(journal.DocDebit * invoice.ExchangeRate), 2);
                //journal.BaseDebit = detail.BaseAmount;
            }
            journal.DocTaxableAmount = detail.DocAmount;
            journal.DocTaxAmount = detail.DocTaxAmount;
            journal.BaseTaxableAmount = detail.BaseAmount;
            journal.BaseTaxAmount = detail.BaseTaxAmount;
            //journal.SegmentCategory1 = invoice.SegmentCategory1;
            //journal.SegmentCategory2 = invoice.SegmentCategory2;
            //journal.SegmentMasterid1 = invoice.SegmentMasterid1;
            //journal.SegmentMasterid2 = invoice.SegmentMasterid2;
            //journal.SegmentDetailid1 = invoice.SegmentDetailid1;
            //journal.SegmentDetailid2 = invoice.SegmentDetailid2;
            journal.IsTax = false;
            journal.DocCreditTotal = detail.DocTotalAmount;
        }
        private void FillJVHeadDetail(JVVDetailModel jModel, Invoice invoice, string type)
        {
            jModel.DocumentId = invoice.Id;
            jModel.SystemRefNo = invoice.InvoiceNumber;
            jModel.DocNo = invoice.DocNo;
            jModel.DocDate = invoice.DocDate;
            jModel.ServiceCompanyId = invoice.ServiceCompanyId.Value;
            jModel.Nature = invoice.Nature;
            jModel.DocSubType = invoice.DocSubType;
            jModel.DocType = invoice.DocType;
            //jModel.Remarks = invoice.Remarks;
            jModel.AccountDescription = invoice.DocDescription;
            jModel.PostingDate = invoice.DocDate;
            jModel.PONo = invoice.PONo;
            jModel.CreditTermsId = invoice.CreditTermsId;
            jModel.DueDate = invoice.DueDate;
            jModel.EntityId = invoice.EntityId;
            jModel.DocCurrency = invoice.DocCurrency;
            jModel.BaseCurrency = invoice.ExCurrency;
            jModel.ExchangeRate = invoice.ExchangeRate;
            jModel.NoSupportingDocs = invoice.IsNoSupportingDocument != true && invoice.NoSupportingDocs != true ? false : true;
            jModel.GSTExCurrency = invoice.GSTExCurrency;
            jModel.GSTExchangeRate = invoice.GSTExchangeRate;
            if (type == DocTypeConstants.Invoice)
            {
                jModel.DocDebit = invoice.GrandTotal;
                //jModel.BaseDebit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocDebit : (decimal)(jModel.DocDebit * jModel.ExchangeRate), 2);
                decimal amount = 0;
                foreach (var detail in invoice.InvoiceDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseDebit = amount;
            }
            if (type == DocTypeConstants.CreditNote)
            {
                jModel.DocCredit = invoice.GrandTotal;
                //jModel.BaseCredit = Math.Round((decimal)jModel.ExchangeRate == null ? (decimal)jModel.DocCredit : (decimal)(jModel.DocCredit * jModel.ExchangeRate), 2);
                decimal amount = 0;
                foreach (var detail in invoice.InvoiceDetails)
                {
                    amount = Math.Round((decimal)(amount + (detail.DocAmount * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                    amount = Math.Round((decimal)(amount + ((detail.DocTaxAmount != null ? detail.DocTaxAmount : 0) * jModel.ExchangeRate)), 2, MidpointRounding.AwayFromZero);
                }
                jModel.BaseCredit = amount;
            }
            //jModel.SegmentCategory1 = invoice.SegmentCategory1;
            //jModel.SegmentCategory2 = invoice.SegmentCategory2;
            //jModel.SegmentMasterid1 = invoice.SegmentMasterid1;
            //jModel.SegmentMasterid2 = invoice.SegmentMasterid2;
            //jModel.SegmentDetailid1 = invoice.SegmentDetailid1;
            //jModel.SegmentDetailid2 = invoice.SegmentDetailid2;
            jModel.BaseAmount = invoice.BalanceAmount;
        }
        private void fillJV(JVModel headJournal, Invoice invoice, string type)
        {
            string strServiceCompany = _masterService.GetIdBy(invoice.ServiceCompanyId.Value);
            headJournal.DocumentId = invoice.Id;
            headJournal.CompanyId = invoice.CompanyId;
            headJournal.PostingDate = invoice.DocDate;
            headJournal.DocNo = invoice.DocNo;
            if (type == DocTypeConstants.Invoice)
                headJournal.DocType = DocTypeConstants.Invoice;
            if (type == DocTypeConstants.CreditNote)
                headJournal.DocType = DocTypeConstants.CreditNote;
            if (type == DocTypeConstants.DoubtFulDebitNote)
                headJournal.DocType = DocTypeConstants.DoubtFulDebitNote;
            headJournal.DocSubType = invoice.DocSubType;
            headJournal.DocDate = invoice.DocDate;
            headJournal.PostingDate = invoice.DocDate;
            //headJournal.DocumentDescription = invoice.IsWorkFlowInvoice == true ? invoice.DocDescription : invoice.Remarks;
            headJournal.DocumentDescription = invoice.DocDescription;
            headJournal.DueDate = invoice.DueDate.Value;
            headJournal.DocumentState = invoice.DocumentState;
            headJournal.SystemReferenceNo = invoice.InvoiceNumber;
            headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
            headJournal.ServiceCompany = strServiceCompany;
            headJournal.Nature = invoice.Nature;
            headJournal.PoNo = invoice.PONo;
            headJournal.ModifiedBy = invoice.ModifiedBy;
            headJournal.ModifiedDate = invoice.ModifiedDate;
            headJournal.ExDurationFrom = invoice.ExDurationFrom;
            headJournal.ExDurationTo = invoice.ExDurationTo;
            headJournal.IsAllowableNonAllowable = invoice.IsAllowableNonAllowable;
            headJournal.GSTExDurationFrom = invoice.GSTExDurationFrom;
            headJournal.GSTExDurationTo = invoice.GSTExDurationTo;
            headJournal.CreditTermsId = invoice.CreditTermsId.Value;
            headJournal.BalanceAmount = invoice.BalanceAmount;
            //headJournal.CreditTerms = (int)_masterService.GetTermsOfPaymentById(invoice.CreditTermsId);
            headJournal.IsSegmentReporting = invoice.IsSegmentReporting;
            headJournal.SegmentCategory1 = invoice.SegmentCategory1;
            headJournal.SegmentCategory2 = invoice.SegmentCategory2;
            headJournal.SegmentMasterid1 = invoice.SegmentMasterid1;
            headJournal.SegmentMasterid2 = invoice.SegmentMasterid2;
            headJournal.SegmentDetailid1 = invoice.SegmentDetailid1;
            headJournal.SegmentDetailid2 = invoice.SegmentDetailid2;
            headJournal.NoSupportingDocument = invoice.NoSupportingDocs;
            headJournal.Status = RecordStatusEnum.Active;
            headJournal.EntityId = invoice.EntityId;
            headJournal.EntityType = invoice.EntityType;
            headJournal.IsRepeatingInvoice = invoice.IsRepeatingInvoice;
            headJournal.RepEveryPeriod = invoice.RepEveryPeriod;
            headJournal.RepEveryPeriodNo = invoice.RepEveryPeriodNo;
            headJournal.EndDate = invoice.RepEndDate;
            headJournal.DocCurrency = invoice.DocCurrency;
            if (type == DocTypeConstants.Invoice || type == DocTypeConstants.DoubtFulDebitNote)
            {
                headJournal.GrandDocDebitTotal = invoice.GrandTotal;
                headJournal.GrandBaseDebitTotal = Math.Round((decimal)(invoice.GrandTotal * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            }
            else if (type == DocTypeConstants.CreditNote)
            {
                headJournal.GrandDocCreditTotal = invoice.GrandTotal;
                headJournal.GrandBaseCreditTotal = Math.Round((decimal)(invoice.GrandTotal * invoice.ExchangeRate), 2);
                headJournal.GrandDocDebitTotal = invoice.GrandTotal;
                headJournal.GrandBaseDebitTotal = Math.Round((decimal)(invoice.GrandTotal * (invoice.ExchangeRate != null ? invoice.ExchangeRate : 1)), 2);
            }
            headJournal.BaseCurrency = invoice.ExCurrency;
            headJournal.ExchangeRate = invoice.ExchangeRate;
            headJournal.IsGstSettings = invoice.IsGstSettings;
            headJournal.IsGSTApplied = invoice.IsGSTApplied;
            headJournal.IsMultiCurrency = invoice.IsMultiCurrency;
            headJournal.IsNoSupportingDocs = invoice.IsNoSupportingDocument;

            if (invoice.IsGstSettings)
            {
                headJournal.GSTExCurrency = invoice.GSTExCurrency;
                headJournal.GSTExchangeRate = invoice.GSTExchangeRate;
            }
            headJournal.Remarks = invoice.Remarks;
            headJournal.UserCreated = invoice.UserCreated;
            headJournal.CreatedDate = invoice.CreatedDate;
        }

        #endregion Posting_Block
    }

}
