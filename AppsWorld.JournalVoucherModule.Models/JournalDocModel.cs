using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class JournalDocModel
    {
        public JournalDocModel()
        {
            this.JournalDocModels = new List<JournalDocNoModel>();
        }
        public DateTime? NextDue { get; set; }

        public List<JournalDocNoModel> JournalDocModels { get; set; }
    }
    public class JournalDocNoModel
    {
        public string DocNo { get; set; }
        public DateTime? DocDate { get; set; }
    }
}
