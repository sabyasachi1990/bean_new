using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class InvoiceDocModel
    {
        public InvoiceDocModel()
        {
            this.JournalDocModels = new List<InvoiceDocNoModel>();
        }
        public DateTime? NextDue { get; set; }
        public List<InvoiceDocNoModel> JournalDocModels { get; set; }
    }
    public class InvoiceDocNoModel
    {
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
    }
}
