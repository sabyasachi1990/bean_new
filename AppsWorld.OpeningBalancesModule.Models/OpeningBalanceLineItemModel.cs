using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OpeningBalanceLineItemModel
    {
        public OpeningBalanceLineItemModel()
        {

        }
        public System.Guid Id { get; set; }
        public Guid OpeningBalanceDetailId { get; set; }
        public DateTime? Date { get; set; }
        public long COAId { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string BaseCurrency { get; set; }
        public string Description { get; set; }
        public string DocCurrency { get; set; }
        public decimal? DocCredit { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? BaseCredit { get; set; }
        public decimal? BaseDebit { get; set; }
        public decimal? ExchangeRate { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        public string DocumentReference { get; set; }
        //public string  EntityName { get; set; }
        //public string  SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        //public string SegmentMasterid1 { get; set; }
        //public string SegmentMasterid2 { get; set; }
        //public long? SegmentDetailid1 { get; set; }
        //public long? SegmentDetailid2 { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ServiceCompanyId { get; set; }
        public string RecordStatus { get; set; }
        public bool? IsDisAllow { get; set; }
        public int RecOrder { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsProcressed { get; set; }
        public string ProcressedRemarks { get; set; }

    }
}
