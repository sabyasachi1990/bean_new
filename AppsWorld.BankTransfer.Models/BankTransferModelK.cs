using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Models
{
    public class BankTransferModelK
    {
        public System.Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocumentState { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public System.DateTime TransferDate { get; set; }
        public Nullable<System.DateTime> WthBankClearing { get; set; }
        public Nullable<System.DateTime> DepBankClearing { get; set; }
        public string DocNo { get; set; }
        public string WthCurr { get; set; }
        public string DepCurr { get; set; }
        public string WthCashBank { get; set; }
        public string DepCashBank { get; set; }
        public double? DepAmt { get; set; }
        public double? WthAmt { get; set; }
        public string ModeOfTransfer { get; set; }
        public string TransferRefNo { get; set; }
        public string WthCo { get; set; }
        public string DepCo { get; set; }
        public string ExchangeRate { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
        //public Nullable<decimal> SystemCalculatedExchangeRate { get; set; }
        //public Nullable<decimal> VarianceExchangeRate { get; set; }

        //public Nullable<System.DateTime> ExDurationFrom { get; set; }
        //public Nullable<System.DateTime> ExDurationTo { get; set; }

        //public string Remarks { get; set; }

        //public Nullable<int> Status { get; set; }
        //public Nullable<short> Version { get; set; }
    }
}
