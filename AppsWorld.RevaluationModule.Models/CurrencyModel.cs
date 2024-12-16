using System;
using System.Collections.Generic;
namespace AppsWorld.RevaluationModule.Models
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
