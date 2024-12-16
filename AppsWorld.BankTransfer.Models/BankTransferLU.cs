using AppsWorld.CommonModule.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Models
{
    public class BankTransferLU
    {
        public long CompanyId { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
        public LookUpCategory<string> ModeOfTransferLU { get; set; }
    }
}
