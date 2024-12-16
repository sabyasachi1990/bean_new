using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class BillModelK
    {
        public Guid Id { get; set; }

        public long CompanyId { get; set; }
        public string DocNo { get; set; }
        //public DateTime PostingDate { get; set; }
        public DateTime DueDate { get; set; }
        //public string BaseCurrency { get; set; }
        public string EntityName { get; set; }
        public string DocumentState { get; set; }
        public System.DateTime DocDate { get; set; }
        public double? BalanceAmount { get; set; }
        //public string DocDescription { get; set; }
        //public string SegmentCategory1 { get; set; }
        //public string SegmentCategory2 { get; set; }
        public string ExchangeRate { get; set; }
        //public Nullable<bool> NoSupportingDocument { get; set; }
        public string Nature { get; set; }
        public double GrandTotal { get; set; }
        public double? BaseTotal { get; set; }
        public string DocCurrency { get; set; }
        //public string VendorType { get; set; }
        //public string SystemReferenceNumber { get; set; }
        //public string CreditTermName { get; set; }
        public string ServiceCompanyName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string UserCreated { get; set; }
        public double? BaseBal { get; set; }
        //public DateTime? BankClearingDate { get; set; }
        public string DocSubType { get; set; }
        public bool? IsExternal { get; set; }
        public System.DateTime PostingDate { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
    }
}
