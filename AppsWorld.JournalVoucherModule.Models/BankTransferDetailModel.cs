using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class BankTransferDetailModel
    {


        public System.Guid Id { get; set; }
        public System.Guid BankTransferId { get; set; }
        public string ChartOfAccountName { get; set; }
        public string ServiceCompanyName { get; set; }
        public long COAId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public decimal? DocDebit { get; set; }
        public decimal? DocCredit { get; set; }
        public string DocDescription { get; set; }
        public Nullable<decimal> BaseDebit { get; set; }
        public Nullable<decimal> BaseCredit { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        //public Nullable<decimal> GrandBaseDebit { get; set; }
        //public Nullable<decimal> GrandBaseCredit { get; set; }
        //public Nullable<decimal> GrandDocCredit { get; set; }
        //public Nullable<decimal> GrandDocDebit { get; set; }
        public string AccountName { get; set; }
        public string TransferFrom { get; set; }
        public string TransferTo { get; set; }


        public decimal? RecOrder { get; set; }
    }
}
