using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public partial class CreditNoteApplicationDetail : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid CreditNoteApplicationId { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocumentType { get; set; }
        public string DocCurrency { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal BaseCurrencyExchangeRate { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public virtual CreditNoteApplication CreditNoteApplication { get; set; }
    }
}