using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Models
{
    public class DebitNoteNoteModel
    {
        public System.Guid Id { get; set; }
        public System.Guid DebitNoteId { get; set; }
        public Nullable<System.DateTime> ExpectedPaymentDate { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UserCreated { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string RecordStatus;
    }
}
