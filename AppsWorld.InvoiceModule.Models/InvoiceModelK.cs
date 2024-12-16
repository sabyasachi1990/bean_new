
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public partial class InvoiceModelK : ISvcEntityFilter
    {

        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        //public string InvoiceNumber { get; set; }
        public string DocNo { get; set; }
        public string DocCurrency { get; set; }
        //public string BaseCurrency { get; set; }
        public double? BaseAmount { get; set; }
        public string PONo { get; set; }

        public string ExchangeRate { get; set; }
        //public string DocDescription { get; set; }
        public double BalanceAmount { get; set; }
        public string Nature { get; set; }
        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        public System.DateTime DocDate { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public double GrandTotal { get; set; }
        //public string CreditTermName { get; set; }
        //public Nullable<bool> NoSupportingDocument { get; set; }
        //public DateTime? PostingDate { get; set; }
        public string ServiceCompanyName { get; set; }
        public long ServiceCompanyId { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public bool? Repeating { get; set; }
        //public double? BaseBalance { get; set; }
        public double? BaseBal { get; set; }
        //public bool? IsWorkFlowInvoice { get; set; }
        //public string CursorType { get; set; }
        public string InternalState { get; set; }
        public string DocType { get; set; }
        public string DocSubType { get; set; }
        public bool? IsSystem { get; set; }
        public string ScreenName { get; set; }//public bool? IsOBInvoice { get; set; }
        public string Action { get; set; }
        public Guid EntityId { get; set; }
        public bool? IsLocked { get; set; }
    }

}
