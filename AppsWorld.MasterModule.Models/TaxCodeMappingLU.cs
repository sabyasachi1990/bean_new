using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class TaxCodeMappingLU
    {
        public List<TaxCodeLookUp<string>> CustTaxCodeLU { get; set; }
        public List<TaxCodeLookUp<string>> VenTaxCodeLU { get; set; }
    }
}
