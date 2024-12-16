using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class InvoiceNoteModel
    {
        public System.Guid Id { get; set; }
        public System.Guid InvoiceId { get; set; }
        public Nullable<System.DateTime> ExpectedPaymentDate { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string RecordStatus { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
