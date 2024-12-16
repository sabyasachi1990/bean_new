using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public partial class CreditNoteModelK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string InvoiceNumber { get; set; }
        public string DocNo { get; set; }
        public string DocCurrency { get; set; }

        public double BalanceAmount { get; set; }
        //public string ExCurrency { get; set; }
        public double BaseAmount { get; set; }
        public string ServiceCompanyName { get; set; }
        public long ServiceCompanyId { get; set; }
        public string CreditTermName { get; set; }
        public string Nature { get; set; }
        public string DocDescription { get; set; }
        //public Nullable<bool> NoSupportingDocument { get; set; }
        public string ExchangeRate { get; set; }
        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        public System.DateTime DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public double GrandTotal { get; set; }
        public double? BaseBal { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserCreated { get; set; }
        public string DocSubType { get; set; }
        public string ExtensionType { get; set; }
        public bool? IsExternal { get; set; }
        public bool? IsSystem { get; set; }
        public string ScreenName { get; set; }
        public Guid EntityId { get; set; }
        public bool? IsLocked { get; set; }
    }
}
