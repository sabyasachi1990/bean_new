using System;
using Repository.Pattern.Ef6;

namespace AppsWorld.CommonModule.Entities
{
    public partial class DocumentHistory : Entity
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocState { get; set; }
        public decimal DocAmount { get; set; }
        public decimal BaseAmount { get; set; }
        public Guid TransactionId { get; set; }
        public string DocCurrency { get; set; }
        public decimal DocBalanaceAmount { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal BaseBalanaceAmount { get; set; }
        public string Remarks { get; set; }
        public string StateChangedBy { get; set; }
        public Nullable<System.DateTime> StateChangedDate { get; set; }
    }
}
