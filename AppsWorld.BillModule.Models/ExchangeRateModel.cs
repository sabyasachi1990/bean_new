using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Models
{
    public class ExchangeRateModel
    {
        public String Base { get; set; }
        public String Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
