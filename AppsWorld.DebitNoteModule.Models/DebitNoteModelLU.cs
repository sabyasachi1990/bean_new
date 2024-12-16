using AppsWorld.CommonModule.Infra;
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

namespace AppsWorld.DebitNoteModule.Models
{
    public class DebitNoteModelLU
    {
        public long CompanyId { get; set; }
        //public LookUpCategory<string> SegmentCategory1LU { get; set; }
        //public LookUpCategory<string> SegmentCategory2LU { get; set; }
        //public List<LookUpCategory<string>> AccountLU { get; set; }
        public LookUpCategory<string> CurrencyLU { get; set; }
        //public LookUpCategory<string> NatureLU { get; set; }
        public List<string> NatureLU { get; set; }
        //public LookUpCategory<string> AllowableNonAllowableLU { get; set; }
        public List<LookUp<string>> TermsOfPaymentLU { get; set; }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public List<LookUpCompany<string>> SubsideryCompanyLU { get; set; }
		public List<COALookup<string>> ChartOfAccountLU { get; set; }
        public string TaxCode { get; set; }
    }
}
