using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppaWorld.Bean
{
    public class DocumentHistoryModel
    {
        public Guid TransactionId { get; set; }
        public long CompanyId { get; set; }
        //public Guid Id { get; set; }

        public Guid DocumentId { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public string DocState { get; set; }
        public string DocCurrency { get; set; }
        public decimal DocAmount { get; set; }
        public decimal DocBalanaceAmount { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal BaseBalanaceAmount { get; set; }
        public string StateChangedBy { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> PostingDate { get; set; }
        
        public decimal? DocAppliedAmount { get; set; }
        public decimal? BaseAppliedAmount { get; set; }

        public string AgingState { get; set; }
        //public DateTime? StateChangedDate { get; set; }
        //public decimal GrandDocBaseDebitTotal { get; set; }
        //public decimal? BalanceAmount { get; set; }
        //public string ModifiedBy { get; set; }
        //public string ConnectionString { get; set; }

    }
}
