using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Models
{
    public class MasterModel
    {
        public int RecCount { get; set; }
        public IEnumerable<BankReconciliationDetailModel> BankReconciliationDetailModels { get; set; }
    }
}
