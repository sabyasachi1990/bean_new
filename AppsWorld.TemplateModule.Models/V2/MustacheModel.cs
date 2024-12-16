
using AppsWorld.TemplateModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.TemplateModule.Models.V2
{
    public class MustacheModel
    {
        //public Templates.Template.Invoice Invoice { get; set; }
        //public Templates.Template.Invoice CreditNote { get; set; }
        //public Templates.Template.Invoice DebitNote { get; set; }
        public Invoices Invoice { get; set; }
        public Invoices CreditNote { get; set; }
        public Invoices DebitNote { get; set; }
        public Invoices CashSale { get; set; }
        public ServiceEntitys Company { get; set; }
        public ServiceEntitys ServiceEntity { get; set; }
        public PrimaryContact Contact { get; set; }
        public List<LineItems> ItemDetail { get; set; }
        public PrimaryContact PrimaryContact { get; set; }
        //public Templates.Template.Entity Entity { get; set; }
        public BeanEntityModel Entity { get; set; }
        public Receipt receipt { get; set; } // receiptModel
        public Receipt Receipt { get; set; }
        public SOAOutstandingAmount StatementModel { get; set; }
        public SOAOutstanding Outstanding { get; set; }
        //public Templates.Template.GSTReporting GSTReporting { get; set; }
        public GSTReporting GSTReporting { get; set; }
        public string InvoiceAmountDue { get; set; }
        public BankReceiptForApplications BankReceiptForApplication { get; set; } // Old BankReceiptForApplication
        public List<BankReceiptForApplications> BRFADetail { get; set; }  // Old BankReceiptForApplicationDetail
        public List<AppsWorld.TemplateModule.Models.V2.Receipt> ReceiptDetailsModel { get; set; }
        public string UserName { get; set; }
        public List<SOAOutstandingAmount> SoaDetail { get; set; }
        public Templates.Template.TaxCode LstTaxCode { get; set; }
        public LineItems LineItem { get; set; }
        public string OutstandingBalance { get; set; }
        public string StatementDate { get; set; }
        public bool? IsGST { get; set; }
        public bool? IsGSTActive { get; set; }
        public bool? IsGSTNotActive { get; set; }
        public bool? IsBankDetailHide { get; set; }
        public bool? IsBankDetailNotHide { get; set; }
        public bool? IsTaxCodeHide { get; set; }
        public bool? ISFoexCurrency { get; set; }
        public bool? ISNotFoexCurrency { get; set; }
        //public bool? IsTaxCodeNotHide { get; set; }
        public string TotalReceiptApplicationAmount { get; set; }
        public List<OutstandingTotals> OutstandingTotal { get; set; }
        public string RegisteredBlock { get; set; }
        public string RegisteredBuilding { get; set; }
        public string RegisteredCountry { get; set; }
        public string RegisteredPostalCode { get; set; }
        public string RegisteredStreet { get; set; }
        public string RegisteredUnit { get; set; }
        public List<AppsWorld.TemplateModule.Models.V2.TaxCode> TaxCode { get; set; }
        public List<Templates.Template.ServiceEntity> Banks { get; set; }
        public string ToEmail { get; set; }
        public List<Invoices> LstDebitNotes { get; set; }
        public List<Invoices> LstCashSales { get; set; }
        public List<BankReceiptForApplications> BRFApplications { get; set; }
        //public bool? IsStaticBank { get; set; }
        //public bool? IsNotStaticBank { get; set; }
        //public InvoiceTemplateVM Invoice { get; set; }
        //public InvoiceTemplateVM CreditNote { get; set; }
        //public ServiceEntity ServiceEntity { get; set; }
        //public ReceiptsModel receiptModel { get; set; }
        //public StatementModel StatementModel { get; set; }
        //public List<ReceiptDetaislModel> ReceiptDetailsModel { get; set; }


    }

    public class GSTReporting
    {
        public string Amount { get; set; }
        public string TaxAmount { get; set; }
        public string TotalAmount { get; set; }
        public string ExchangeRate { get; set; }
    }
    public class SOAOutstanding
    {
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string DocumentTotal { get; set; }
        public string DocBalance { get; set; }
        public string Currency { get; set; }
        public string DocType { get; set; }
        public decimal? CreditNoteBalance { get; set; }
        public string BaseAmount { get; set; }
        public string ReceivedAmount { get; set; }
        public string Aging { get; set; }
    }
    public class ServiceEntitys
    {


        public string Currency { get; set; }
        public string AccountName { get; set; }
        public string SWIFTCode { get; set; }
        public string BankAddress { get; set; }
        public string AccountNumber { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public string Entityaddress { get; set; }
        public string MailingAddress { get; set; }
        public string RegisteredAddress { get; set; }
        public string IdentificationType { get; set; }
        public string RegistrationNo { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string GSTNumber { get; set; }
        public string EntityName { get; set; }
    }
    public class PrimaryContact
    {


        public string ContactSalutation { get; set; }
        public string ContactName { get; set; }
        public string Contactemail { get; set; }
    }
    public class OutstandingTotals
    {
        public string SubTotal { get; set; }
        public string Currency { get; set; }
    }

    public class SOAOutstandingAmount
    {


        public decimal? CreditNoteBalance { get; set; }
        public string Currency { get; set; }
        public string Remarks { get; set; }
        public string DocBalance { get; set; }
        public string DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string DocumentTotal { get; set; }
        public long ServiceCompanyId { get; set; }
        public string BaseAmount { get; set; }
        public string ReceivedAmount { get; set; }
        public string Aging { get; set; }
    }
    public class BankReceiptForApplications
    {


        public string Amount { get; set; }
        public string Balance { get; set; }
        public string Co { get; set; }
        public string DocCurrency { get; set; }
        public string DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocTotal { get; set; }
        public string DocType { get; set; }
        public string Receipt { get; set; }
    }
    public class Receipt
    {
        public string BRAmount { get; set; } // Old -- BankReceiptAmount
        public string RAAmmount { get; set; } // Old  --- ReceiptApplicationAmmount
        public string BRCurrency { get; set; } // Old BankReceiptCurrency
        public List<BankReceiptForApplications> BRFApplication { get; set; } //    --- Old BankReceiptForApplication
        public string DocDate { get; set; }
        public string DocNo { get; set; }
        public string DocumentDescription { get; set; }
        public string ModeOfReceipt { get; set; }
        public string RRNumber { get; set; } //Old ---ReceiptReferenceNumber
        public string RecieptDate { get; set; }
        public string UserCreated { get; set; }
    }
    public class Invoices
    {
        public string CreditTerms { get; set; }
        public string Currency { get; set; }
        public string BalanceAmount { get; set; }
        public string DocDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string DocNo { get; set; }
        public string DocumentDescription { get; set; }
        public string DueDate { get; set; }
        public string ExchangeRate { get; set; }
        public string ExcludeGST { get; set; }
        public string GSTPayable { get; set; }
        public string IncludeGST { get; set; }
        public List<LineItems> LineItem { get; set; }
        public string PO { get; set; }
        public string SubTotal { get; set; }
        public List<TaxCodess> TaxCode { get; set; }
        public string OutStandingAmount { get; set; }
        public string Total { get; set; }
        public string BaseCurrency { get; set; }
        public string TaxTotal { get; set; }
    }
    public class TaxCode
    {


        public string SubTotal { get; set; }
        public string TaxCodes { get; set; }
        public string TaxName { get; set; }
        public double? TaxRate { get; set; }
    }
    public class LineItems
    {


        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Discount { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string DocNo { get; set; }
        public string DocDate { get; set; }
        public string Quantity { get; set; }
        public string TaxAmount { get; set; }
        public string TaxCode { get; set; }
        public string Total { get; set; }
        public string Unit { get; set; }
        public string UnitPrice { get; set; }
        public string BaseAmount { get; set; }
        public string BaseTotal { get; set; }
    }
    public class TaxCodess
    {


        public string SubTotal { get; set; }
        public string TaxCodes { get; set; }
        public string TaxName { get; set; }
        public double? TaxRate { get; set; }
    }


    public class InvoiceModel
    {
        public System.Guid EntityId { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public string Currency { get; set; }
        public string DocType { get; set; }
        public System.Guid DocumentId { get; set; }
        public decimal BaseBalanceAmount { get; set; }
        public decimal DocBalanceAmount { get; set; }
        public long ServiceCompanyId { get; set; }
        public string DocSubType { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal BaseGrandTotal { get; set; }
    }
    public class BeanEntityModel
    {
        public string Entityname { get; set; }
        public string RegisteredAddress { get; set; }
        public string MailingAddress { get; set; }
        public string RegisteredBlock { get; set; }
        public string RegisteredStreet { get; set; }
        public string RegisteredUnit { get; set; }
        public string RegisteredBuilding { get; set; }
        public string RegisteredCountry { get; set; }
        public string RegisteredPostalCode { get; set; }
        public string RegisteredEmail { get; set; }
        public string MailingBlock { get; set; }
        public string MailingStreet { get; set; }
        public string MailingUnit { get; set; }
        public string MailingBuilding { get; set; }
        public string MailingCountry { get; set; }
        public string MailingEmail { get; set; }
        public string MailingPostalCode { get; set; }
    }
}
