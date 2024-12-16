using AppsWorld.CommonModule.Entities;
using AppsWorld.OpeningBalancesModule.Entities;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class OpeningBalanceDetail : Entity
    {
        public OpeningBalanceDetail()
        {
            this.OpeningBalanceDetailLineItems = new List<OpeningBalanceDetailLineItem>();
        }

        public System.Guid Id { get; set; }
        public System.Guid OpeningBalanceId { get; set; }
        public long COAId { get; set; }
        public string BaseCurrency { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public string DocCurrency { get; set; }
        public Nullable<decimal> DocCredit { get; set; }
        public Nullable<decimal> DocDebit { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Recorder { get; set; }
        public bool? IsOrginalAccount { get; set; }
        public string ClearingState { get; set; }
        public DateTime? ClearingDate { get; set; }
        public DateTime? ReconciliationDate { get; set; }
        public Guid? ReconciliationId { get; set; }
        //  [ForeignKey("COAId")]
        //public virtual ChartOfAccount ChartOfAccount { get; set; }
        public virtual ICollection<OpeningBalanceDetailLineItem> OpeningBalanceDetailLineItems { get; set; }
    }
}
