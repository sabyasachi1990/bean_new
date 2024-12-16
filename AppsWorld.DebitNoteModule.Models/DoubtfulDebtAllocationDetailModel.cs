using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
    public class DoubtfulDebtAllocationDetailModel
    {
        public System.Guid Id { get; set; }
        public System.Guid DoubtfulDebitAllocationId { get; set; }
        public string DocType { get; set; }
        public System.Guid DocumentId { get; set; }
        public string DocCurrency { get; set; }
        public decimal AllocateAmount { get; set; }
        public string DocNo { get; set; }
        public Nullable<System.DateTime> DocDate { get; set; }
        public string SystemReferenceNumber { get; set; }
        public string Nature { get; set; }
        public decimal? DocAmount { get; set; }
        public decimal? BalanceAmount { get; set; }
		public decimal? PreviousAllocateAmmount { get; set; }
    }
}
