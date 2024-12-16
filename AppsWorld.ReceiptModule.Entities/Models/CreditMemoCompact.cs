using Repository.Pattern.Ef6;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppsWorld.ReceiptModule.Entities
{
    public partial class CreditMemoCompact:Entity
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocSubType { get; set; }
        public System.DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        public DateTime PostingDate { get; set; }
        public string Nature { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
       // public string ExCurrency { get; set; }
        public string DocumentState { get; set; }
        public decimal BalanceAmount { get; set; }
        public Nullable<long> ServiceCompanyId { get; set; }
        public System.Guid EntityId { get; set; }
        public string DocType { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public decimal? GSTExchangeRate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
       // public Guid? OpeningBalanceId { get; set; }
       // public Nullable<decimal> AllocatedAmount { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
       // public string DocDescription { get; set; }
        public decimal? BaseGrandTotal { get; set; }
        public decimal? BaseBalanceAmount { get; set; }
        public decimal? RoundingAmount { get; set; }
    }
}
