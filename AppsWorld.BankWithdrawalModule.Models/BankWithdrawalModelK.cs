using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankWithdrawalModule.Models
{
    public class BankWithdrawalModelK
    {
        public Guid Id { get; set; }
        public Guid? EntityId { get; set; }
        public long CompanyId { get; set; }
        public string SystemRefNo { get; set; }
        public string ServiceCompanyName { get; set; }
        public DateTime DocDate { get; set; }
        public string DocNo { get; set; }
        //public Nullable<System.DateTime> CreatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        //public decimal BankWithdrawalAmmount { get; set; }
        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        //public string DocType { get; set; }
        public string CashBankAccount { get; set; }
        //public string DocDescription { get; set; }
        public string WithdrawalRefNo { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public string ExchangeRate { get; set; }
        //public bool? NoSupportingDocument { get; set; }
        public string UserCreated { get; set; }
        //public string DocCurrency { get; set; }
        //public string ExCurrency { get; set; }
        public double GrandTotal { get; set; }
        public double BaseAmount { get; set; }
        public string ModeOfWithdrawal { get; set; }
        public string ModifiedBy { get; set; }
        public long COAId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<System.DateTime> BankClearingDate { get; set; }
        public string DocCurrency { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
    }
}
