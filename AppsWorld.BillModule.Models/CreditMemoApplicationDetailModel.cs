
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class CreditMemoApplicationDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid CreditMemoApplicationId { get; set; }
        public string DocType { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocCurrency { get; set; }
        public decimal? CreditAmount { get; set; }
        public string DocNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public DateTime? PostingDate { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string Nature { get; set; }
        public decimal? DocAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
        public string DocState { get; set; }
        public decimal BaseCurrencyExchangeRate { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public string ServiceEntityName { get; set; }
        public long? SereviceEntityId { get; set; }
        public bool? IsHyperLinkEnable { get; set; }
    }
}
