using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Models
{
    public partial class CreditMemoModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        //public string CreditMemoNumber { get; set; }
        public string DocNo { get; set; }
        public string DocCurrency { get; set; }
        public string ExCurrency { get; set; }
        public double BaseAmount { get; set; }

        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        public System.DateTime DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public double GrandTotal { get; set; }
        public string DocSubType { get; set; }
        public double? BalanceAmount { get; set; }
        public string Nature { get; set; }
        public string ServiceCompanyName { get; set; }
        public double? BaseBalance { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ExchangeRate { get; set; }
        public bool? IsExternal { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public DateTime PostingDate { get; set; }
        public bool? IsLocked { get; set; }
        public string DocType { get; set; }
    }
}
