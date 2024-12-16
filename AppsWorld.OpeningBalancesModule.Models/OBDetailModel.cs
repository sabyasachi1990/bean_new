using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Models
{
    public class OBDetailModel
    {
        public long CompanyId { get; set; }
        public Guid OpeningBalanceId { get; set; }
        public long ServiceCompanyId { get; set; }
        public string DocType { get; set; }
        public bool IsMultiCurrency { get; set; }
        public bool IsGSTSettings { get; set; }
    }
}
