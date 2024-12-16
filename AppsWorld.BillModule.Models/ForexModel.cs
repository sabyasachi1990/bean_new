using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class ForexModel
    {
        public decimal? GSTUnitPerUSD { get; set; }
        public decimal? BaseUnitPerUSD { get; set; }
        public string DocumentDate { get; set; }
        public string Provider { get; set; }
    }
}
