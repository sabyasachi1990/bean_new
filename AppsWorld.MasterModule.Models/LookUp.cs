using AppsWorld.MasterModule.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.MasterModule.Models
{
    public class LookUp
    {

        public List<AccountType> AccountTypes { get; set; }
        public List<TaxCode> TaxCodes { get; set; }
        //public List<SegmentDetail> SegmentReporting1 { get; set; }
        //public List<SegmentDetail> SegmentReporting2 { get; set; }

        public bool showTaxCode { get; set; }
        public bool showSegmentCategory1 { get; set; }
        public bool showCategoryReport2 { get; set; }
        public bool IsEditabled { get; set; }
        public bool IsAllowable { get; set; }
        public bool showAccountType { get; set; }
    }
}
