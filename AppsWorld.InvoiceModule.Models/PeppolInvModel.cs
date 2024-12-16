using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class PeppolInvModel
    {
        public string  Id { get; set; }
        public string IssueDate { get; set; }
        public string DueDate { get; set; }
        public string InvoiceTypeCode { get; set; }
        public string Note { get; set; }
        public string DocumentCurrencyCode { get; set; }
        public string AccountingCost { get; set; }
        public string BuyerReference { get; set; }
        public string InvStartDate { get; set; }
        public string InvEndDate { get; set; }
        public string OrderReferenceId { get; set; }
        public string OrderRefSalesOrderId { get; set; }
        public string BillingRefId { get; set; }
        public string BillingRefIssueDate { get; set; }
        public string DespatchDocId { get; set; }
        public string ReceiptDocId { get; set; }
        public string OriginatorDocumentRefId { get; set; }
        public string ContractDocumentRefId { get; set; }
        public string AdditionalDocRefID { get; set; }
        public string AdditionalDocRefDocDesc { get; set; }
        public string AdditionalDocRefURl { get; set; }
        public string AdditionalDocRefEmbeddedDocBinaryObject { get; set; }
        public string AdditionalDocRefDocTypeCode { get; set; }
        public string ProjectReferenceID { get; set; }
        public string AccSupplierEndpointID { get; set; }
        public string AccSupplierPartyIdentifiID{ get; set; }
        public string AccSupplierPartyIdentiPartyName { get; set; }
        public string AccSupplierStreetName { get; set; }
        public string AccSupplierAddiStreetName { get; set; }
        public string AccSupplierCityName { get; set; }
        public string AccSupplierPostalZone { get; set; }
        public string AccSupplierCountrySubentity { get; set; }
        public string AccSupplierAddressLine { get; set; }
        public string AccSupplierIdentificationCode { get; set; }
        public string AccSupPartyTaxSchemeCompanyId { get; set; }
        public string AccSupPartyTaxSchemeID { get; set; }
        public string AccSupLegalEntityRegName { get; set; }
        public string AccSupContactName { get; set; }
        public string AccSupContactTelephone { get; set; }
        public string AccSupContactEmail { get; set; }

        public string SupRegisteredBlock { get; set; }
        public string SupRegisteredStreet { get; set; }
        public string SupRegisteredCity { get; set; }
        public string SupRegisteredState { get; set; }
        //public string RegisteredAddLine1 { get; set; }
        //public string RegisteredAddLine2 { get; set; }
        public string SupRegisteredUnit { get; set; }
        public string SupRegisteredBuilding { get; set; }
        public string SupRegisteredCountry { get; set; }
        public string SupRegisteredPostalCode { get; set; }

        public string SupMaillingBlock { get; set; }
        public string SupMaillingStreet { get; set; }
        public string SupMaillingCity { get; set; }
        public string SupMaillingState { get; set; }
        //public string MaillingAddLine1 { get; set; }
        //public string MaillingAddLine2 { get; set; }
        public string SupMaillingUnit { get; set; }
        public string SupMaillingBuilding { get; set; }
        public string SupMaillingCountry { get; set; }
        public string SupMaillingPostalCode { get; set; }

        public string AccCustEndpointID { get; set; }
        public string AccCustPartyIdentifiID { get; set; }
        public string AccCustPartyIdentiPartyName { get; set; }
        public string AccCustStreetName { get; set; }
        public string AccCustAddiStreetName { get; set; }
        public string AccCustCityName { get; set; }
        public string AccCustPostalZone { get; set; }
        public string AccCustCountrySubentity { get; set; }
        public string AccCustAddressLine { get; set; }
        public string AccCustIdentificationCode { get; set; }
        //public string AccSupPartyTaxSchemeCompanyId { get; set; }
        //public string AccSupPartyTaxSchemeID { get; set; }

        public string CustRegisteredBlock { get; set; }
        public string CustRegisteredStreet { get; set; }
        public string CustRegisteredCity { get; set; }
        public string CustRegisteredState { get; set; }
        //public string RegisteredAddLine1 { get; set; }
        //public string RegisteredAddLine2 { get; set; }
        public string CustRegisteredUnit { get; set; }
        public string CustRegisteredBuilding { get; set; }
        public string CustRegisteredCountry { get; set; }
        public string CustRegisteredPostalCode { get; set; }

        public string CustMaillingBlock { get; set; }
        public string CustMaillingStreet { get; set; }
        public string CustMaillingCity { get; set; }
        public string CustMaillingState { get; set; }
        //public string MaillingAddLine1 { get; set; }
        //public string MaillingAddLine2 { get; set; }
        public string CustMaillingUnit { get; set; }
        public string CustMaillingBuilding { get; set; }
        public string CustMaillingCountry { get; set; }
        public string CustMaillingPostalCode { get; set; }

        public string AccCustLegalEntityRegName { get; set; }
        public string AccCustContactName { get; set; }
        public string AccCustContactTelephone { get; set; }
        public string AccCustContactEmail { get; set; }
        public string PayeePartyIdentifiId { get; set; }
        public string PayeePartyName { get; set; }
        public string PayeePartyLegalEntityCompanyId { get; set; }
        public string PaymentMeansCode { get; set; }
        public string PaymentID { get; set; }
        public string PayeeFinancialAccountID { get; set; }
        public string PayeeFinancialAccountName { get; set; }
        public string FinancialInstituBranchId { get; set; }
        public string PaymentTerms { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string TaxCategoryID { get; set; }
        public int TaxCategoryPercent { get; set; }
        public string TaxSchemeID { get; set; }
        public decimal LegalLineExtensionAmount { get; set; }
        public decimal LegalTaxExclusiveAmount { get; set; }
        public decimal LegalTaxInclusiveAmount { get; set; }
        public string LegalAllowanceTotalAmount { get; set; }
        public string LegalChargeTotalAmount { get; set; }
        public string LegalPrepaidAmount { get; set; }
        public string LegalPayableRoundingAmount { get; set; }
        public decimal LegalPayableAmount { get; set; }
        public List<PeppolInvLineItemModel> InvLineItems { get; set; }

    }
    public class PeppolInvLineItemModel
    {
        public int ID { get; set; }
        public string Note { get; set; }
        public double? InvoicedQuantity { get; set; }
        public decimal  LineExtensionAmount { get; set; }
        public string AccountingCost { get; set; }
        public string InvStartDate { get; set; }
        public string InvEndDate { get; set; }
        public string OrderLineRefLineId { get; set; }
        public string DocumentRefID { get; set; }
        public string DocumentTypeCode { get; set; }
        public string ChargeIndicator { get; set; }
        public string AllowanceChargeReasonCode { get; set; }
        public string AllowanceChargeReason { get; set; }
        public double? MultiplierFactorNumeric { get; set; }
        public decimal  AllowanceAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public string ItemName { get; set; }
        public string SellersItemIdentifiID { get; set; }
        public string StandardItemIdentifiID { get; set; }
        public string IdentificationCode { get; set; }
        public string ItemClassificationCode { get; set; }
        public string  ClassifiedTaxCategID { get; set; }
        public int ClassifiedTaxCategPercent { get; set; }
        public string TaxSchemeID { get; set; }
        public decimal PriceAmount { get; set; }
        public double? BaseQuantity { get; set; }

        public decimal singleTaxAmount { get; set; }
    }

    public class PeppolResponseModel
    {
        public string docId { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
        public string transmissionTime { get; set; }
    }
    public class CommunicationBindingModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
