
using AppsWorld.CommonModule.Infra;
using AppsWorld.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public partial class ItemModelLU
    {
        public ItemModelLU()
        {
            this.ChartOfAccountLU = new List<COALookup<string>>();
        }
        public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        //public List<LookUpCategory<string>> SegmentCategory1LU { get; set; }
        //public List<LookUpCategory<string>> SegmentCategory2LU { get; set; }
        public List<COALookup<string>> ChartOfAccountLU { get; set; }
       // public List<TaxCodeLookUp<string>> TaxCodeLU { get; set; }
        public LookUpCategory<string> UnitsLU { get; set; }
       
        //public bool showTaxCode { get; set; }
        //public bool showSegmentCategory1 { get; set; }
        //public bool showSegmentCategory2 { get; set; }
        //public bool showAccount { get; set; }
        //public bool showAllowable { get; set; }
    }
 }
