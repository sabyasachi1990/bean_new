using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class InvoiceVoidK
    {
        public Guid Id { get; set; }
        public long CompanyId { get; set; }
        public string DocNo { get; set; }
        public string DocType { get; set; }
        public string DocCurrency { get; set; }
        public string BaseCurrency { get; set; }
        public string DocDescription { get; set; }
        public string DocumentState { get; set; }
        public string EntityName { get; set; }
        public System.DateTime DocDate { get; set; }
        public string ServiceCompanyName { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string InternalState { get; set; }
        public string DocSubType { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
