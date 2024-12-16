using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class CreditNoteAppTypeModel
    {
        public System.Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public long CompanyId { get; set; }
        public System.DateTime CreditNoteApplicationDate { get; set; }
        public Nullable<System.DateTime> CreditNoteApplicationResetDate { get; set; }
        public decimal CreditAmount { get; set; }
        public string Remarks { get; set; }
        public string CreditNoteApplicationNumber { get; set; }
        public string UserCreated { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int Status { get; set; }
        public decimal? ExchangeRate { get; set; }
        public bool? IsRevExcess { get; set; }
        public Guid? DocumentId { get; set; }



    }
}
