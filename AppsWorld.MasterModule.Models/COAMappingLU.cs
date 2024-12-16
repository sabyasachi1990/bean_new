using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class COAMappingLU
    {
        public List<COALookup<string>> CustomerChartOfAccountLU { get; set; }
        public List<COALookup<string>> VendorChartOfAccountLU { get; set; }

    }
}
