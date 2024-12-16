using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class DocumentTypeModel
    {
        public Guid? Id { get; set; }
        public long CompanyId { get; set; }
        public Guid? EntityId { get; set; }
        public long? ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string DocNo { get; set; }
        public string DocumentState { get; set; }
        public string DocCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string GSTCurrency { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? BalanaceAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? GSTExchangeRate { get; set; }
        public long? CreditTermsId { get; set; }
        public string DocDescription { get; set; }
        public string PONo { get; set; }
        public bool? NoSupportingDocs { get; set; }
        public string Nature { get; set; }
        public decimal? GSTTotalAmount { get; set; }
        public decimal? GrandTotal { get; set; }
        public bool? IsGstSettings { get; set; }
        public bool? IsGSTApplied { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsAllowableNonAllowable { get; set; }
        public bool? IsNoSupportingDocument { get; set; }
        public bool? IsAllowableDisallowableActivated { get; set; }
        public bool? Status { get; set; }
        public string UserCreated { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsBaseCurrencyRateChanged { get; set; }
        public bool? IsGSTCurrencyRateChanged { get; set; }
    }
}
