using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Models
{
    public class CreditmemoApplicationLu
    {
        public long CompanyId { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
    }
}
