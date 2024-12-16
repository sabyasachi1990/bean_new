using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Models
{
    public class CNAModelLu
    {
        public long CompanyId { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
    }
}
