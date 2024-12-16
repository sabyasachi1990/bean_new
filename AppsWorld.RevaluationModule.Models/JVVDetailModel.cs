using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.RevaluationModule.Models
{
    public class JVVDetailModel
    {
        public System.Guid Id { get; set; }
        public Guid DocumentId { get; set; }
        public Guid DocumentDetailId { get; set; }
		public string EntityType { get; set; }
        public System.Guid JournalId { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public Guid? EntityId { get; set; }
        public string DocType { get; set; }
        public long? COAId { get; set; }
        public string AccountDescription { get; set; }
        public decimal DocDebit { get; set; }
        public decimal DocCredit { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public decimal DocDebitTotal { get; set; }
		public string SystemReferenceNo { get; set; }
        public decimal DocCreditTotal { get; set; }
		public string BaseCurrency { get; set; }
		public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> BaseDebitTotal { get; set; }
        public Nullable<decimal> BaseCreditTotal { get; set; }
		public string DocSubType { get; set; }
		public string DocNo { get; set; }
		public long? ServiceCompanyId { get; set; }
		public bool IsTax { get; set; }
        public int? RecOrder { get; set; }
        public string DocCurrency { get; set; }
    }
}
