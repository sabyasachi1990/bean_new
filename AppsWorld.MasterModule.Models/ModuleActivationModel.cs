using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class ModuleActivationModel
    {
        public bool IsGSTActive { get; set; }
        public bool IsMultiCurrencyActive { get; set; }
        public bool IsNoSupportingDocumentActive { get; set; }
        public bool IsFinancialActive { get; set; }
        public bool IsAllowableNonAllowable { get; set; }
        public string BaseCurrency { get; set; }

    }
}
