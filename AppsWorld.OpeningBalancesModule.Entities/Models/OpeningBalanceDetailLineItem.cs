using AppsWorld.CommonModule.Entities;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class OpeningBalanceDetailLineItem : Entity
    {
        public System.Guid Id { get; set; }
        public System.Guid OpeningBalanceDetailId { get; set; }
        public System.DateTime? Date { get; set; }
        public long COAId { get; set; }
        public string Description { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public string DocumentCurrency { get; set; }
        public Nullable<decimal> DocCredit { get; set; }
        public Nullable<decimal> DoCDebit { get; set; }
        public string DocumentReference { get; set; }
        public Nullable<System.Guid> EntityId { get; set; }
        //public string DocCurrency { get; set; }
        //public string EntityName { get; set; }
        public long? SegmentMasterid1 { get; set; }
        public long? SegmentMasterid2 { get; set; }
        public string SegmentCategory1 { get; set; }
        public string SegmentCategory2 { get; set; }
        public long? SegmentDetailid1 { get; set; }
        public long? SegmentDetailid2 { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public long ServiceCompanyId { get; set; }
        public int Recorder { get; set; }
        public bool? IsDisAllow { get; set; }
        public DateTime? DueDate { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsProcressed { get; set; }
        public string ProcressedRemarks { get; set; }
        [ForeignKey("COAId")]
        public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual BeanEntity Entity { get; set; }
    }
}
