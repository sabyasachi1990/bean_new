using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.GLClearingModule.Models
{
    public class ClearingModelLUs
    {
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public List<COALookup<string>> ChartOfAccountNewLU { get; set; }
    }
}
