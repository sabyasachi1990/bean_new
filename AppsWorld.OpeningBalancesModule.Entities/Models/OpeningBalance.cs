using AppsWorld.CommonModule.Entities;
using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Entities
{
    public class OpeningBalance : Entity
    {
        public OpeningBalance()
        {
            this.OpeningBalanceDetails = new List<OpeningBalanceDetail>();
        }

        public System.Guid Id { get; set; }
        public System.DateTime Date { get; set; }
        public long CompanyId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string BaseCurrency { get; set; }
        //public string CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool Status { get; set; }
        public string DocType { get; set; }
        [Timestamp]
        public byte[] Version { get; set; }
        public string Remarks { get; set; }
        public virtual Company Company { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string SaveType { get; set; }
        public string SystemRefNo { get; set; }
        public bool? IsMultiCurrency { get; set; }
        public bool? IsSegmentReporting { get; set; }
        public bool? IsNoSupportingDoc { get; set; }
        public bool? IsEditable { get; set; }
        public Guid? PostedId { get; set; }
        //  public int Recorder { get; set; }
        public bool? IsLocked { get; set; }
        public bool? IsTemporary { get; set; }
        public virtual List<OpeningBalanceDetail> OpeningBalanceDetails { get; set; }
    }
}
