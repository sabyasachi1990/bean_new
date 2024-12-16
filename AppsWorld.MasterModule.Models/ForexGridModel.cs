using AppsWorld.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public partial class ForexGridModel
    {
        public long CompanyId { get; set; }
        [StringLength(50)]
        public string GSTRepoCurrency { get; set; }
        [StringLength(50)]
        public string BaseCurrency { get; set; }
        public string GstStatus { get; set; }
        public string MultyCurrencyStatus { get; set; }
        public string FinancialStatus { get; set; }

		public bool? IsGSTAllowed { get; set; }
    }
}
