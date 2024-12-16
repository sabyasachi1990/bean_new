using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.ReceiptModule.Models
{
    public  class CurrencyModel
    {
       public string CurrencyCode {get; set;}
       public string CurrencyName { get; set; }
        public String Base { get; set; }

        //[JsonProperty("date")]
        public String Date { get; set; }

        //[JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
