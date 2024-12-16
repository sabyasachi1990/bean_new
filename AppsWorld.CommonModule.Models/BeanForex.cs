using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CommonModule.Models
{
    public class BeanForex
    {
        public decimal? GSTUnitPerUSD { get; set; }
        public decimal? BaseUnitPerUSD { get; set; }
        public string DocumentDate { get; set; }
        public string Provider { get; set; }
    }
}
