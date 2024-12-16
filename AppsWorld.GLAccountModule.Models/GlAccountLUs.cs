using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLAccountModule.Models
{
    public class GlAccountLUs
    {
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool? IsCleared { get; set; }
    }
}
