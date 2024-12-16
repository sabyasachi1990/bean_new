using Service.Pattern;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;

namespace AppsWorld.InvoiceModule.Service
{
    //public class JournalLedgerService : Service<JournalLedger>, IJournalLedgerService
    //{
    //    private readonly IInvoiceModuleRepositoryAsync<JournalLedger> _journalEntryRepository;
      

    //    public JournalLedgerService(IInvoiceModuleRepositoryAsync<JournalLedger> journalEntryRepository)
    //        : base(journalEntryRepository)
    //    {
    //        _journalEntryRepository = journalEntryRepository;           
    //    }    

    //    //public void PostInvoice(Invoice invoice, string docType)
    //    //{
    //    //    int count = 1;
    //    //    decimal baseTotal = 0;
    //    //    string strServiceCompany = _companyRepository.Query(a => a.Id == invoice.ServiceCompanyId).Select(a => a.ShortName).FirstOrDefault();
    //    //    TermsOfPayment top = _termsOfPaymentService.GetTermsOfPaymentById(invoice.CreditTermsId);

    //    //    JournalLedger headJournal = new JournalLedger();
    //    //    headJournal.RecOrder = count++;
    //    //    headJournal.Id = Guid.NewGuid();
    //    //    headJournal.CompanyId = invoice.CompanyId;
    //    //    headJournal.PostingDate = invoice.DocDate;
    //    //    headJournal.DocNo = invoice.DocNo;
    //    //    headJournal.DocType = docType;
    //    //    headJournal.DocSubType = invoice.DocSubType;
    //    //    headJournal.DocDate = invoice.DocDate;
    //    //    headJournal.DueDate = invoice.DueDate;
    //    //    headJournal.SystemReferenceNumber = invoice.InvoiceNumber;
    //    //    headJournal.ServiceCompanyId = invoice.ServiceCompanyId;
    //    //    headJournal.ServiceCompany = strServiceCompany;
    //    //    headJournal.Nature = invoice.Nature;
    //    //    headJournal.PONo = invoice.PONo;
    //    //    headJournal.CreditTerms = (int)top.TOPValue;
    //    //    headJournal.SegmentCategory1 = invoice.SegmentCategory1;
    //    //    headJournal.SegmentCategory2 = invoice.SegmentCategory2;
    //    //    headJournal.NoSupportingDocs = invoice.NoSupportingDocs;
    //    //    headJournal.Status = RecordStatusEnum.Active;
    //    //    //headJournal.ServiceId = invoice.ServiceId;

    //    //    headJournal.EntityId = invoice.EntityId;
    //    //    BeanEntity entity = _beanEntityService.Query(a => a.Id == invoice.EntityId).Select().FirstOrDefault();
    //    //    headJournal.EntityName = entity.Name;
    //    //    //headJournal.EntityRefNo = invoice.EntityRefNo;
    //    //    headJournal.EntityType = invoice.EntityType;

    //    //    ChartOfAccount account = _chartOfAccountService.GetChartOfAccountByName(invoice.Nature == "Trade" ? COANameConstants.AccountsReceivables : COANameConstants.OtherReceivables);
    //    //    headJournal.COAId = account.Id;
    //    //    headJournal.AccountCode = account.Code;
    //    //    headJournal.AccountName = account.Name;
    //    //    //headJournal.Subledgerrequired = account.IsSubLedger;
    //    //    headJournal.Subledgerrequired = true;

    //    //    headJournal.DocCurrency = invoice.DocCurrency;
    //    //    headJournal.DebitDC = invoice.GrandTotal;

    //    //    headJournal.BaseCurrency = invoice.ExCurrency;
    //    //    headJournal.ExchangeRateBc = invoice.ExchangeRate;
    //    //    headJournal.DebitBC = Math.Round((decimal)(invoice.GrandTotal * invoice.ExchangeRate), 2);

    //    //    if (invoice.IsGstSettings)
    //    //    {
    //    //        headJournal.GSTReportingCurrency = invoice.GSTExCurrency;
    //    //        headJournal.ExchangeRateGSTR = invoice.GSTExchangeRate;
    //    //    }

    //    //    headJournal.Remarks = invoice.Remarks;
    //    //    headJournal.UserCreated = invoice.UserCreated;
    //    //    headJournal.CreatedDate = DateTime.UtcNow;

    //    //    headJournal.ObjectState = ObjectState.Added;
    //    //    _journalEntryRepository.Insert(headJournal);

    //    //    foreach (InvoiceDetail detail in invoice.InvoiceDetails)
    //    //    {
    //    //        JournalLedger journal = new JournalLedger();

    //    //        journal.RecOrder = count++;
    //    //        journal.Id = Guid.NewGuid();
    //    //        journal.CompanyId = invoice.CompanyId;
    //    //        journal.PostingDate = invoice.DocDate;
    //    //        journal.DocNo = invoice.DocNo;
    //    //        journal.DocType = docType;
    //    //        journal.DocSubType = invoice.DocSubType;
    //    //        journal.DocDate = invoice.DocDate;
    //    //        journal.DueDate = invoice.DueDate;
    //    //        journal.SystemReferenceNumber = invoice.InvoiceNumber;
    //    //        journal.ServiceCompanyId = invoice.ServiceCompanyId;
    //    //        journal.ServiceCompany = strServiceCompany;
    //    //        journal.Nature = invoice.Nature;
    //    //        journal.PONo = invoice.PONo;
    //    //        journal.CreditTerms = (int)top.TOPValue;
    //    //        journal.SegmentCategory1 = invoice.SegmentCategory1;
    //    //        journal.SegmentCategory2 = invoice.SegmentCategory2;
    //    //        journal.NoSupportingDocs = invoice.NoSupportingDocs;

    //    //        journal.EntityId = invoice.EntityId;
    //    //        journal.EntityName = entity.Name;
    //    //        journal.EntityType = invoice.EntityType;

    //    //        account = _chartOfAccountService.GetChartOfAccount(detail.COAId);
    //    //        journal.COAId = account.Id;
    //    //        journal.AccountCode = account.Code;
    //    //        journal.AccountName = account.Name;

    //    //        journal.ItemId = detail.ItemId;
    //    //        journal.ItemCode = detail.ItemCode;
    //    //        journal.ItemDescription = detail.ItemDescription;
    //    //        journal.Qty = detail.Qty;
    //    //        journal.Unit = detail.Unit;
    //    //        journal.UnitPrice = detail.UnitPrice;
    //    //        journal.Discount = detail.Discount;
    //    //        journal.DiscountType = detail.DiscountType;
    //    //        journal.AllowDisAllow = detail.AllowDisAllow;
    //    //        journal.Status = RecordStatusEnum.Active;
    //    //        if (invoice.IsGstSettings)
    //    //        {
    //    //            if (detail.TaxId != null)
    //    //            {
    //    //                TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
    //    //                journal.TaxCode = tax.Code;
    //    //                journal.TaxRate = tax.TaxRate;
    //    //                journal.TaxType = tax.TaxType;
    //    //            }
    //    //        }
    //    //        journal.DocCurrency = invoice.DocCurrency;
    //    //        journal.CreditDC = detail.DocAmount;
    //    //        if (invoice.IsGstSettings)
    //    //        {
    //    //            journal.TaxableamountDC = -detail.DocAmount;
    //    //            journal.TaxAmountDC = -detail.DocTaxAmount;
    //    //        }
    //    //        journal.BaseCurrency = invoice.ExCurrency;
    //    //        journal.ExchangeRateBc = invoice.ExchangeRate;
    //    //        journal.CreditBC = detail.BaseAmount == null ? 0 : detail.BaseAmount.Value;
    //    //        baseTotal += journal.CreditBC.Value;
    //    //        if (invoice.IsGstSettings)
    //    //        {
    //    //            journal.TaxableamountBC = -detail.BaseAmount;
    //    //            journal.TaxAmountBC = -detail.BaseTaxAmount;
    //    //        }
    //    //        //if (invoice.IsGstSettings)
    //    //        //{
    //    //        //	journal.GSTReportingCurrency = invoice.GSTExCurrency;
    //    //        //	journal.ExchangeRateGSTR = invoice.GSTExchangeRate;

    //    //        //	journal.TaxableamountGSTR = Math.Round((decimal)(journal.TaxableamountDC * invoice.GSTExchangeRate), 2);
    //    //        //	journal.TaxAmountGSTR = Math.Round((decimal)(journal.TaxAmountDC * invoice.GSTExchangeRate), 2);
    //    //        //}

    //    //        journal.Remarks = invoice.Remarks;
    //    //        journal.UserCreated = invoice.UserCreated;
    //    //        journal.CreatedDate = DateTime.UtcNow;
    //    //        journal.ObjectState = ObjectState.Added;
    //    //        _journalEntryRepository.Insert(journal);
    //    //    }
    //    //    if (invoice.IsGstSettings)
    //    //    {
    //    //        ChartOfAccount gstAccount = _chartOfAccountService.GetChartOfAccountByName(COANameConstants.TaxPayableGST);

    //    //        foreach (InvoiceDetail detail in invoice.InvoiceDetails)
    //    //        {
    //    //            JournalLedger journal = new JournalLedger();
    //    //            journal.RecOrder = count++;
    //    //            journal.Id = Guid.NewGuid();
    //    //            journal.CompanyId = invoice.CompanyId;
    //    //            journal.PostingDate = invoice.DocDate;
    //    //            journal.DocNo = invoice.DocNo;
    //    //            journal.DocType = "Invoice";
    //    //            journal.DocSubType = invoice.DocSubType;
    //    //            journal.DocDate = invoice.DocDate;
    //    //            journal.DueDate = invoice.DueDate;
    //    //            journal.SystemReferenceNumber = invoice.InvoiceNumber;
    //    //            journal.ServiceCompanyId = invoice.ServiceCompanyId;
    //    //            journal.ServiceCompany = strServiceCompany;
    //    //            journal.Nature = invoice.Nature;
    //    //            journal.PONo = invoice.PONo;
    //    //            journal.CreditTerms = (int)top.TOPValue;
    //    //            journal.SegmentCategory1 = invoice.SegmentCategory1;
    //    //            journal.SegmentCategory2 = invoice.SegmentCategory2;
    //    //            journal.NoSupportingDocs = invoice.NoSupportingDocs;
    //    //            journal.Status = RecordStatusEnum.Active;

    //    //            journal.EntityId = invoice.EntityId;
    //    //            journal.EntityName = entity.Name;
    //    //            journal.EntityType = invoice.EntityType;

    //    //            journal.COAId = gstAccount.Id;
    //    //            journal.AccountCode = gstAccount.Code;
    //    //            journal.AccountName = gstAccount.Name;
    //    //            if (detail.TaxId != null)
    //    //            {
    //    //                TaxCode tax = _taxCodeService.GetTaxCodesById(detail.TaxId.Value).FirstOrDefault();
    //    //                journal.TaxCode = tax.Code;
    //    //                journal.TaxRate = tax.TaxRate;
    //    //                journal.TaxType = tax.TaxType;
    //    //            }

    //    //            journal.DocCurrency = invoice.DocCurrency;
    //    //            journal.CreditDC = detail.DocTaxAmount == null ? 0 : detail.DocTaxAmount.Value;

    //    //            journal.BaseCurrency = invoice.ExCurrency;
    //    //            journal.ExchangeRateBc = invoice.ExchangeRate;
    //    //            journal.CreditBC = detail.BaseTaxAmount == null ? 0 : detail.BaseTaxAmount.Value;
    //    //            baseTotal += journal.CreditBC.Value;

    //    //            journal.GSTReportingCurrency = invoice.GSTExCurrency;
    //    //            journal.ExchangeRateGSTR = invoice.GSTExchangeRate;

    //    //            //journal.CreditGSTR = Math.Round((decimal)(journal.CreditDC * invoice.GSTExchangeRate), 2);

    //    //            journal.Remarks = invoice.Remarks;
    //    //            journal.UserCreated = invoice.UserCreated;
    //    //            journal.CreatedDate = DateTime.UtcNow;

    //    //            journal.ObjectState = ObjectState.Added;
    //    //            _journalEntryRepository.Insert(journal);
    //    //        }
    //    //    }
    //    //    headJournal.DebitBC = baseTotal;
    //    //}

      

    //}
}
