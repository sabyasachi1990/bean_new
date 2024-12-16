using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class CurrencyModel
    {
        //[JsonProperty("base")]
        public String Base { get; set; }

        //[JsonProperty("date")]
        public String Date { get; set; }

        //[JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
