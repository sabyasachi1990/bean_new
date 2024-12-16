using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{

    public class MultiInBoundInvoiceModel
    {
        public Xml Xml { get; set; }
        public MultiInvoiceStandardBusinessDocument StandardBusinessDocument { get; set; }
    }
    public class MultiInvoiceStandardBusinessDocument
    {
        [JsonProperty("@xmlns:xs")]
        public string XmlnsXs { get; set; }
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public StandardBusinessDocumentHeader StandardBusinessDocumentHeader { get; set; }
        public MultiInvoice Invoice { get; set; }
    }
    public class InBoundInvoiceModel
    {
        public Xml Xml { get; set; }
        public StandardBusinessDocument StandardBusinessDocument { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }
        [JsonProperty("@encoding")]
        public string Encoding { get; set; }
    }
    public class StandardBusinessDocument
    {
        [JsonProperty("@xmlns:xs")]
        public string XmlnsXs { get; set; }
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public StandardBusinessDocumentHeader StandardBusinessDocumentHeader { get; set; }
        public SingleInvoice Invoice { get; set; }
    }
    public class StandardBusinessDocumentHeader
    {
        public string HeaderVersion { get; set; }
        public Sender Sender { get; set; }
        public Receiver Receiver { get; set; }
        public DocumentIdentification DocumentIdentification { get; set; }
        public BusinessScope BusinessScope { get; set; }
    }
    public class Sender
    {
        public Identifier Identifier { get; set; }
    }
    public class Receiver
    {
        public Identifier2 Identifier { get; set; }
    }
    public class DocumentIdentification
    {
        public string Standard { get; set; }
        public string TypeVersion { get; set; }
        public string InstanceIdentifier { get; set; }
        public string Type { get; set; }
        public DateTime CreationDateAndTime { get; set; }
    }

    public class BusinessScope
    {
        public List<Scope> Scope { get; set; }
    }

    public class Identifier
    {
        [JsonProperty("@Authority")]
        public string Authority { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class Identifier2
    {
        [JsonProperty("@Authority")]
        public string Authority { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }
    public class Scope
    {
        public string Type { get; set; }
        public string InstanceIdentifier { get; set; }
        public string Identifier { get; set; }
    }

    public class SingleInvoice: InvoiceProperties
    {
        [JsonProperty("cac:InvoiceLine")]
        public CacInvoiceLine CacInvoiceLine { get; set; }
    }
    public class MultiInvoice : InvoiceProperties
    {
        [JsonProperty("cac:InvoiceLine")]
        public List<CacInvoiceLine> CacInvoiceLine { get; set; }
    }
    public class InvoiceProperties
    {
        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        [JsonProperty("@xmlns:cac")]
        public string XmlnsCac { get; set; }
        [JsonProperty("@xmlns:cbc")]
        public string XmlnsCbc { get; set; }
        [JsonProperty("@xmlns:ccts")]
        public string XmlnsCcts { get; set; }
        [JsonProperty("@xmlns:ext")]
        public string XmlnsExt { get; set; }
        [JsonProperty("@xmlns:qdt")]
        public string XmlnsQdt { get; set; }
        [JsonProperty("@xmlns:udt")]
        public string XmlnsUdt { get; set; }
        [JsonProperty("@xmlns:xsd")]
        public string XmlnsXsd { get; set; }
        [JsonProperty("@xmlns:xsi")]
        public string XmlnsXsi { get; set; }
        [JsonProperty("cbc:UBLVersionID")]
        public string CbcUBLVersionID { get; set; }
        [JsonProperty("cbc:CustomizationID")]
        public string CbcCustomizationID { get; set; }
        [JsonProperty("cbc:ProfileID")]
        public string CbcProfileID { get; set; }
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:IssueDate")]
        public string CbcIssueDate { get; set; }
        [JsonProperty("cbc:DueDate")]
        public string CbcDueDate { get; set; }
        [JsonProperty("cbc:InvoiceTypeCode")]
        public string CbcInvoiceTypeCode { get; set; }
        [JsonProperty("cbc:Note")]
        public string CbcNote { get; set; }
        [JsonProperty("cbc:TaxPointDate")]
        public string CbcTaxPointDate { get; set; }
        [JsonProperty("cbc:DocumentCurrencyCode")]
        public string CbcDocumentCurrencyCode { get; set; }
        [JsonProperty("cbc:AccountingCost")]
        public string CbcAccountingCost { get; set; }
        [JsonProperty("cbc:BuyerReference")]
        public string CbcBuyerReference { get; set; }
        [JsonProperty("cac:InvoicePeriod")]
        public CacInvoicePeriod CacInvoicePeriod { get; set; }
        [JsonProperty("cac:OrderReference")]
        public CacOrderReference CacOrderReference { get; set; }
        [JsonProperty("cac:OrderLineReference")]
        public CacOrderLineReference CacOrderLineReference { get; set; }
        [JsonProperty("cac:DocumentReference")]
        public CacDocumentReference CacDocumentReference { get; set; }
        [JsonProperty("cac:BillingReference")]
        public CacBillingReference CacBillingReference { get; set; }

        [JsonProperty("cac:DespatchDocumentReference")]
        public CacDespatchDocumentReference CacDespatchDocumentReference { get; set; }

        [JsonProperty("cac:ReceiptDocumentReference")]
        public CacReceiptDocumentReference CacReceiptDocumentReference { get; set; }

        [JsonProperty("cac:OriginatorDocumentReference")]
        public CacOriginatorDocumentReference CacOriginatorDocumentReference { get; set; }
        [JsonProperty("cac:ContractDocumentReference")]
        public CacContractDocumentReference CacContractDocumentReference { get; set; }

        [JsonProperty("cac:AdditionalDocumentReference")]
        public CacAdditionalDocumentReference CacAdditionalDocumentReference { get; set; }

        [JsonProperty("cac:ExternalReference")]
        public CacExternalReference CacExternalReference { get; set; }
        [JsonProperty("cac:ProjectReference")]
        public CacProjectReference CacProjectReference { get; set; }

        [JsonProperty("cac:AccountingSupplierParty")]
        public CacAccountingSupplierParty CacAccountingSupplierParty { get; set; }
        [JsonProperty("cac:AccountingCustomerParty")]
        public CacAccountingCustomerParty CacAccountingCustomerParty { get; set; }

        [JsonProperty("cac:PayeeParty")]
        public CacPayeeParty CacPayeeParty { get; set; }

        [JsonProperty("cac:TaxRepresentativeParty")]
        public CacTaxRepresentativeParty CacTaxRepresentativeParty { get; set; }


        [JsonProperty("cac:Delivery")]
        public CacDelivery CacDelivery { get; set; }


        [JsonProperty("cac:PaymentMeans")]
        public CacPaymentMeans CacPaymentMeans { get; set; }


        [JsonProperty("cac:PaymentTerms")]
        public CacPaymentTerms CacPaymentTerms { get; set; }

        [JsonProperty("cac:TaxTotal")]
        public CacTaxTotal CacTaxTotal { get; set; }
        [JsonProperty("cac:LegalMonetaryTotal")]
        public CacLegalMonetaryTotal CacLegalMonetaryTotal { get; set; }

        //public CacInvoiceLine CacInvoiceLines { get; set; }
        //  [JsonProperty("cac:InvoiceLine")]
        // public CacInvoiceLine CacInvoiceLine { get; set; }
        //  public List<List<CacInvoiceLine>> CacInvoiceLine { get; set; }
        //  public CacInvoiceLine[] CacInvoiceLine;
    }






    public class CacInvoicePeriod
    {
        [JsonProperty("cbc:StartDate")]
        public string CbcStartDate { get; set; }
        [JsonProperty("cbc:EndDate")]
        public string CbcEndDate { get; set; }
        [JsonProperty("cbc:DescriptionCode")]
        public string CbcDescriptionCode { get; set; }
    }
    public class CacOrderReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:SalesOrderID")]
        public string CbcSalesOrderID { get; set; }
    }
    public class CacOrderLineReference
    {
        [JsonProperty("cbc:LineID")]
        public string CbcLineID { get; set; }
    }

    public class CacInvoiceDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:IssueDate")]
        public string CbcIssueDate { get; set; }
    }

    public class CacBillingReference
    {
        [JsonProperty("cac:InvoiceDocumentReference")]
        public CacInvoiceDocumentReference CacInvoiceDocumentReference { get; set; }
    }
    public class CacDespatchDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CacReceiptDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CbcEndpointID
    {
        [JsonProperty("@schemeID")]
        public string SchemeID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcID
    {
        [JsonProperty("@schemeID")]
        public string SchemeID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CacPartyIdentification
    {
        [JsonProperty("cbc:ID")]
        public CbcID CbcID { get; set; }
    }

    public class CacPartyName
    {
        [JsonProperty("cbc:Name")]
        public string CbcName { get; set; }
    }

    public class CacCountry
    {
        [JsonProperty("cbc:IdentificationCode")]
        public string CbcIdentificationCode { get; set; }
    }

    public class CacPostalAddress
    {
        [JsonProperty("cbc:StreetName")]
        public string CbcStreetName { get; set; }
        [JsonProperty("cbc:AdditionalStreetName")]
        public string CbcAdditionalStreetName { get; set; }
        [JsonProperty("cbc:CityName")]
        public string CbcCityName { get; set; }
        [JsonProperty("cbc:PostalZone")]
        public string CbcPostalZone { get; set; }
        [JsonProperty("cbc:CountrySubentity")]
        public string CbcCountrySubentity { get; set; }
        [JsonProperty("cbc:AddressLine")]
        public string CbcAddressLine { get; set; }
        [JsonProperty("cac:Country")]
        public CacCountry CacCountry { get; set; }
    }

    public class CacTaxScheme
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }

    public class CacPartyTaxScheme
    {
        [JsonProperty("cbc:CompanyID")]
        public string CbcCompanyID { get; set; }
        [JsonProperty("cac:TaxScheme")]
        public CacTaxScheme CacTaxScheme { get; set; }
    }

    public class CacPartyLegalEntity1
    {

        [JsonProperty("cbc:CompanyID")]
        public CbcCompanyID CbcCompanyID { get; set; }
    }

    public class CacPartyLegalEntity
    {
        [JsonProperty("cbc:RegistrationName")]
        public string CbcRegistrationName { get; set; }
        [JsonProperty("cbc:CompanyID")]
        public string  CbcCompanyID { get; set; }
        [JsonProperty("cbc:CompanyLegalForm")]
        public string CbcCompanyLegalForm { get; set; }
    }

    public class CacParty
    {
        [JsonProperty("cbc:EndpointID")]
        public CbcEndpointID CbcEndpointID { get; set; }
        [JsonProperty("cac:PartyIdentification")]
        public CacPartyIdentification CacPartyIdentification { get; set; }
        [JsonProperty("cac:PartyName")]
        public CacPartyName CacPartyName { get; set; }
        [JsonProperty("cac:PostalAddress")]
        public CacPostalAddress CacPostalAddress { get; set; }
        [JsonProperty("cac:PartyTaxScheme")]
        public CacPartyTaxScheme CacPartyTaxScheme { get; set; }
        [JsonProperty("cac:PartyLegalEntity")]
        public CacPartyLegalEntity CacPartyLegalEntity { get; set; }
        [JsonProperty("cac:Contact")]
        public CacContact CacContact { get; set; }
    }

    public class CacContact
    {
        [JsonProperty("cbc:Name")]
        public string CbcName { get; set; }
        [JsonProperty("cbc:Telephone")]
        public string CbcTelephone { get; set; }
        [JsonProperty("cbc:ElectronicMail")]
        public string CbcElectronicMail { get; set; }
    }
    public class CbcCompanyID
    {
        [JsonProperty("@schemeID")]
        public string SchemeID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }
    public class CacAccountingSupplierParty
    {
        [JsonProperty("cac:Party")]
        public CacParty CacParty { get; set; }
    }

    public class CbcEndpointID2
    {
        [JsonProperty("@schemeID")]
        public string SchemeID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcID2
    {
        [JsonProperty("@schemeID")]
        public string SchemeID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CacPartyIdentification2
    {
        [JsonProperty("cbc:ID")]
        public CbcID2 CbcID { get; set; }
    }

    public class CacPartyName2
    {
        [JsonProperty("cbc:Name")]
        public string CbcName { get; set; }
    }

    public class CacCountry2
    {
        [JsonProperty("cbc:IdentificationCode")]
        public string CbcIdentificationCode { get; set; }
    }

    public class CacPostalAddress2
    {
        [JsonProperty("cbc:StreetName")]
        public string CbcStreetName { get; set; }
        [JsonProperty("cbc:CityName")]
        public string CbcCityName { get; set; }
        [JsonProperty("cbc:PostalZone")]
        public string CbcPostalZone { get; set; }
        [JsonProperty("cbc:CountrySubentity")]
        public string CbcCountrySubentity { get; set; }
        [JsonProperty("cac:Country")]
        public CacCountry2 CacCountry { get; set; }
    }

    public class CacPartyLegalEntity2
    {
        [JsonProperty("cbc:RegistrationName")]
        public string CbcRegistrationName { get; set; }
    }

    public class CacParty2
    {
        [JsonProperty("cbc:EndpointID")]
        public CbcEndpointID2 CbcEndpointID { get; set; }
        [JsonProperty("cac:PartyIdentification")]
        public CacPartyIdentification2 CacPartyIdentification { get; set; }
        [JsonProperty("cac:PartyName")]
        public CacPartyName2 CacPartyName { get; set; }
        [JsonProperty("cac:PostalAddress")]
        public CacPostalAddress2 CacPostalAddress { get; set; }
        [JsonProperty("cac:PartyLegalEntity")]
        public CacPartyLegalEntity2 CacPartyLegalEntity { get; set; }
    }

    public class CacAccountingCustomerParty
    {
        //[JsonProperty("cac:Party")]
        //public CacParty2 CacParty { get; set; }
        [JsonProperty("cac:Party")]
        public CacParty CacParty { get; set; }
    }

    public class CbcTaxAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcTaxableAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcTaxAmount2
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CacTaxScheme2
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }

    public class CacTaxCategory
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:Percent")]
        public string CbcPercent { get; set; }
        [JsonProperty("cbc:TaxExemptionReasonCode")]
        public string CbcTaxExemptionReasonCode { get; set; }
        [JsonProperty("cbc:TaxExemptionReason")]
        public string CbcTaxExemptionReason { get; set; }
        [JsonProperty("cac:TaxScheme")]
        public CacTaxScheme2 CacTaxScheme { get; set; }
    }

    public class CacTaxSubtotal
    {
        [JsonProperty("cbc:TaxableAmount")]
        public CbcTaxableAmount CbcTaxableAmount { get; set; }
        [JsonProperty("cbc:TaxAmount")]
        public CbcTaxAmount2 CbcTaxAmount { get; set; }
        [JsonProperty("cac:TaxCategory")]
        public CacTaxCategory CacTaxCategory { get; set; }
    }

    public class CacTaxTotal
    {
        [JsonProperty("cbc:TaxAmount")]
        public CbcTaxAmount CbcTaxAmount { get; set; }
        [JsonProperty("cac:TaxSubtotal")]
        public CacTaxSubtotal CacTaxSubtotal { get; set; }
    }

    public class CbcLineExtensionAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcTaxExclusiveAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcTaxInclusiveAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcAllowanceTotalAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcChargeTotalAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcPrepaidAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcPayableRoundingAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcPayableAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CacLegalMonetaryTotal
    {
        [JsonProperty("cbc:LineExtensionAmount")]
        public CbcLineExtensionAmount CbcLineExtensionAmount { get; set; }
        [JsonProperty("cbc:TaxExclusiveAmount")]
        public CbcTaxExclusiveAmount CbcTaxExclusiveAmount { get; set; }
        [JsonProperty("cbc:TaxInclusiveAmount")]
        public CbcTaxInclusiveAmount CbcTaxInclusiveAmount { get; set; }
        [JsonProperty("cbc:AllowanceTotalAmount")]
        public CbcAllowanceTotalAmount CbcAllowanceTotalAmount { get; set; }
        [JsonProperty("cbc:ChargeTotalAmount")]
        public CbcChargeTotalAmount CbcChargeTotalAmount { get; set; }
        [JsonProperty("cbc:PrepaidAmount")]
        public CbcPrepaidAmount CbcPrepaidAmount { get; set; }
        [JsonProperty("cbc:PayableRoundingAmount")]
        public CbcPayableRoundingAmount CbcPayableRoundingAmount { get; set; }
        [JsonProperty("cbc:PayableAmount")]
        public CbcPayableAmount CbcPayableAmount { get; set; }
    }

    public class CbcInvoicedQuantity
    {
        [JsonProperty("@unitCode")]
        public string UnitCode { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcLineExtensionAmount2
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CbcBaseAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CacAllowanceCharge
    {
        [JsonProperty("cbc:ChargeIndicator")]
        public string CbcChargeIndicator { get; set; }
        [JsonProperty("cbc:AllowanceChargeReasonCode")]
        public string CbcAllowanceChargeReasonCode { get; set; }
        [JsonProperty("cbc:AllowanceChargeReason")]
        public string CbcAllowanceChargeReason { get; set; }
        [JsonProperty("cbc:MultiplierFactorNumeric")]
        public string CbcMultiplierFactorNumeric { get; set; }
        [JsonProperty("cbc:Amount")]
        public CbcAmount CbcAmount { get; set; }
        [JsonProperty("cbc:BaseAmount")]
        public CbcBaseAmount CbcBaseAmount { get; set; }
        [JsonProperty("cac:TaxCategory")]
        public CacTaxCategory CacTaxCategory { get; set; } //not reuired
    }
    public class CacAllowanceCharge1
    {
        [JsonProperty("cbc:ChargeIndicator")]
        public string CbcChargeIndicator { get; set; }

        [JsonProperty("cbc:Amount")]
        public CbcAmount CbcAmount { get; set; }
        [JsonProperty("cbc:BaseAmount")]
        public CbcBaseAmount CbcBaseAmount { get; set; }


    }
    public class CacBuyersItemIdentification
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CacSellersItemIdentification
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CacStandardItemIdentification
    {
        [JsonProperty("cbc:ID")]
        public CbcID3 CbcID { get; set; }
    }
    public class CacOriginCountry
    {
        [JsonProperty("cbc:IdentificationCode")]
        public string CbcIdentificationCode { get; set; }
    }
    public class CbcID3
    {
        [JsonProperty("@schemeID")]
        public string SchemeID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }
    public class CacTaxScheme3
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CacCommodityClassification
    {
        [JsonProperty("cbc:ItemClassificationCode")]
        public CbcItemClassificationCode CbcItemClassificationCode { get; set; }

    }

    public class CacClassifiedTaxCategory
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:Percent")]
        public string CbcPercent { get; set; }
        [JsonProperty("cac:TaxScheme")]
        public CacTaxScheme3 CacTaxScheme { get; set; }
    }

    public class CacItem
    {
        [JsonProperty("cbc:Name")]
        public string CbcName { get; set; }
        [JsonProperty("cbc:Description")]
        public string CbcDescription { get; set; }
        [JsonProperty("cac:BuyersItemIdentification")]
        public CacBuyersItemIdentification CacBuyersItemIdentification { get; set; }
        [JsonProperty("cac:SellersItemIdentification")]
        public CacSellersItemIdentification CacSellersItemIdentification { get; set; }
        [JsonProperty("cac:StandardItemIdentification")]
        public CacStandardItemIdentification CacStandardItemIdentification { get; set; }
        [JsonProperty("cac:OriginCountry")]
        public CacOriginCountry CacOriginCountry { get; set; }
        [JsonProperty("cac:CommodityClassification")]
        public CacCommodityClassification CacCommodityClassification { get; set; }
        [JsonProperty("cac:ClassifiedTaxCategory")]
        public CacClassifiedTaxCategory CacClassifiedTaxCategory { get; set; }
        [JsonProperty("cac:AdditionalItemProperty")]
        public CacAdditionalItemProperty CacAdditionalItemProperty { get; set; }
    }
    public class CacAdditionalItemProperty
    {
        [JsonProperty("cbc:Name")]
        public string CbcName { get; set; }
        [JsonProperty("cbc:Value")]
        public string CbcValue { get; set; }
    }
    public class CbcItemClassificationCode
    {
        [JsonProperty("@listID")]
        public string ListID { get; set; }
        [JsonProperty("@listVersionID")]
        public string ListVersionID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }
    public class CbcPriceAmount
    {
        [JsonProperty("@currencyID")]
        public string CurrencyID { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property,Inherited = false,AllowMultiple = false)]
    internal sealed class OptionalAttribute : Attribute { }
    public class CbcBaseQuantity
    {
       [Optional] [JsonProperty("@unitCode")]
        public string UnitCode { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }

    public class CacPrice
    {
        [JsonProperty("cbc:PriceAmount")]
        public CbcPriceAmount CbcPriceAmount { get; set; }
        //[JsonProperty("cbc:BaseQuantity")]
        //public CbcBaseQuantity CbcBaseQuantity { get; set; }
        [JsonProperty("cbc:BaseQuantity")]
        public string  CbcBaseQuantity { get; set; }
        [JsonProperty("cac:AllowanceCharge")]
        public CacAllowanceCharge1 CacAllowanceCharge { get; set; }
    }

    //public class CacInvoiceLine
    //{
    //    [JsonProperty("cbc:ID")]
    //    public string CbcID { get; set; }

    //    [JsonProperty("cbc:Note")]
    //    public string CbcNote { get; set; }

    //    [JsonProperty("cbc:InvoicedQuantity")]
    //    public CbcInvoicedQuantity CbcInvoicedQuantity { get; set; }

    //    [JsonProperty("cbc:LineExtensionAmount")]
    //    public CbcLineExtensionAmount2 CbcLineExtensionAmount { get; set; }

    //    [JsonProperty("cbc:AccountingCost")]
    //    public string CbcAccountingCost { get; set; }


    //    [JsonProperty("cac:AllowanceCharge")]
    //    public CacAllowanceCharge CacAllowanceCharge { get; set; }


    //    [JsonProperty("cac:Item")]
    //    public CacItem CacItem { get; set; }


    //    [JsonProperty("cac:Price")]
    //    public CacPrice CacPrice { get; set; }
    //}



    public class CacInvoiceLine
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:InvoicedQuantity")]
        public CbcInvoicedQuantity CbcInvoicedQuantity { get; set; }
        [JsonProperty("cbc:LineExtensionAmount")]
        public CbcLineExtensionAmount2 CbcLineExtensionAmount { get; set; }
        [JsonProperty("cac:AllowanceCharge")]
        public CacAllowanceCharge CacAllowanceCharge { get; set; }
        [JsonProperty("cac:Item")]
        public CacItem CacItem { get; set; }
        [JsonProperty("cac:Price")]
        public CacPrice CacPrice { get; set; }
    }

    public class Root
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }
        public StandardBusinessDocument StandardBusinessDocument { get; set; }
    }

    public class CacPayeeParty
    {
        [JsonProperty("cac:PartyIdentification")]
        public CacPartyIdentification CacPartyIdentification { get; set; }
        [JsonProperty("cac:PartyName")]
        public CacPartyName CacPartyName { get; set; }

        [JsonProperty("cac:PartyLegalEntity")]
        public CacPartyLegalEntity1 CacPartyLegalEntity { get; set; }
    }


    public class CacOriginatorDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CacTaxRepresentativeParty
    {
        [JsonProperty("cac:PartyName")]
        public CacPartyName CacPartyName { get; set; }
        [JsonProperty("cac:PostalAddress")]
        public CacPostalAddress CacPostalAddress { get; set; }
        [JsonProperty("cac:PartyTaxScheme")]
        public CacPartyTaxScheme CacPartyTaxScheme { get; set; }
    }


    public class CacPaymentTerms
    {
        [JsonProperty("cbc:Note")]
        public string CbcNote { get; set; }
    }

    public class CacDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public CbcID CbcID { get; set; }
        [JsonProperty("cbc:DocumentTypeCode")]
        public string CbcDocumentTypeCode { get; set; }
    }
    public class CacContractDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }

    public class CacDelivery
    {
        [JsonProperty("cbc:ActualDeliveryDate")]
        public string CbcActualDeliveryDate { get; set; }
        [JsonProperty("cac:DeliveryLocation")]
        public CacDeliveryLocation CacDeliveryLocation { get; set; }
        [JsonProperty("cac:DeliveryParty")]
        public CacDeliveryParty CacDeliveryParty { get; set; }
    }

    public class CacDeliveryLocation
    {
        [JsonProperty("cbc:ID")]
        public CbcID CbcID { get; set; }
        [JsonProperty("cac:Address")]
        public CacAddress CacAddress { get; set; }
    }
    public class CacDeliveryParty
    {
        [JsonProperty("cac:PartyName")]
        public CacPartyName CacPartyName { get; set; }
    }

    public class CacPaymentMeans
    {
        [JsonProperty("cbc:PaymentMeansCode")]
        public CbcPaymentMeansCode CbcPaymentMeansCode { get; set; }
        [JsonProperty("cbc:PaymentID")]
        public string CbcPaymentID { get; set; }

        [JsonProperty("cac:CardAccount")]
        public CacCardAccount CacCardAccount { get; set; }
        [JsonProperty("cac:PayeeFinancialAccount")]
        public CacPayeeFinancialAccount CacPayeeFinancialAccount { get; set; }
        [JsonProperty("cac:PaymentMandate")]
        public CacPaymentMandate CacPaymentMandate { get; set; }
    }
    public class CbcPaymentMeansCode
    {
        [JsonProperty("@name")]
        public string Name { get; set; }
        [JsonProperty("#text")]
        public string Text { get; set; }
    }
    public class CacCardAccount
    {
        [JsonProperty("cbc:PrimaryAccountNumberID")]
        public string CbcPrimaryAccountNumberID { get; set; }
        [JsonProperty("cbc:NetworkID")]
        public string CbcNetworkID { get; set; }
        [JsonProperty("cbc:HolderName")]
        public string CbcHolderName { get; set; }
    }

    public class CacPayeeFinancialAccount
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:Name")]
        public string CbcName { get; set; }
        [JsonProperty("cac:FinancialInstitutionBranch")]
        public CacFinancialInstitutionBranch CacFinancialInstitutionBranch { get; set; }
    }

    public class CacFinancialInstitutionBranch
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }

    }

    public class CacPaymentMandate
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cac:PayerFinancialAccount")]
        public CacPayerFinancialAccount CacPayerFinancialAccount { get; set; }
    }
    public class CacPayerFinancialAccount
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }
    public class CacAddress
    {
        [JsonProperty("cbc:StreetName")]
        public string CbcStreetName { get; set; }
        [JsonProperty("cbc:AdditionalStreetName")]
        public string CbcAdditionalStreetName { get; set; }
        [JsonProperty("cbc:CityName")]
        public string CbcCityName { get; set; }
        [JsonProperty("cbc:PostalZone")]
        public string CbcPostalZone { get; set; }
        [JsonProperty("cbc:CountrySubentity")]
        public string CbcCountrySubentity { get; set; }
        [JsonProperty("cbc:AddressLine")]
        public CbcAddressLine CbcAddressLine { get; set; }
        [JsonProperty("cac:Country")]
        public CacCountry CacCountry { get; set; }
    }
    public class CbcAddressLine
    {
        [JsonProperty("cbc:Line")]
        public string CbcLine { get; set; }
    }
    public class CacAdditionalDocumentReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
        [JsonProperty("cbc:DocumentTypeCode")]
        public string CbcDocumentTypeCode { get; set; }
        [JsonProperty("cbc:DocumentDescription")]
        public string CbcDocumentDescription { get; set; }
        [JsonProperty("cac:Attachment")]
        public CacAttachment CacAttachment { get; set; }
    }

    public class CacAttachment
    {
        [JsonProperty("cac:ExternalReference")]
        public CacExternalReference CacExternalReference { get; set; }
        //[JsonProperty("cac:ExternalReference")]
        //public CacExternalReference CacExternalReference { get; set; } //Doubt property or class
    }

    public class CacExternalReference
    {
        [JsonProperty("cbc:URI")]
        public string CbcURI { get; set; }
    }
    //public class CbcEmbeddedDocumentBinaryObject
    //{
    //    [JsonProperty("cbc:URI")]
    //    public string CbcURI { get; set; }
    //}

    public class CacProjectReference
    {
        [JsonProperty("cbc:ID")]
        public string CbcID { get; set; }
    }



    public class InvBindingModel
    {
        public System.Guid? PeppolDocumentId { get; set; }
        public string SenderPeppolId { get; set; }
        public string ReciverPeppolId { get; set; }
        public string DocNo { get; set; }
        public string ServiceEntityName { get; set; }
        public string EntityName { get; set; }
        public string InvDate { get; set; }
        public string DueDate { get; set; }
        public string DocCurrency { get; set; }
        public int TermsOfPayment { get; set; }
        public string ContactName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        //public string MyProperty { get; set; }
        //public string MyProperty { get; set; }
    }

    public class CommunicationBindingModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
