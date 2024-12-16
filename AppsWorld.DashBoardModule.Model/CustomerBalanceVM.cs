using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Models
{
  public  class CustomerBalanceVM
    {
        public DrillDownChartVM  CustomerBalanceDetails { get; set; }
        public List<Top30CustomersOrVendors> Top30CustomerOrVendorDetails { get; set; }
        public string ToDate { get; set; }
        public DateTime FromDate { get; set; }
     }

    public class CustomerBalanceData
    {
        public string MonthYear { get; set; }
        public string Aging { get; set; }
        public double TotalBalance { get; set; }

    }

    public class Top30CustomersOrVendors
    {
        public string CustomerOrVendor{ get; set; }
        public double Billing { get; set; }
        public double Balance { get; set; }
        public string Current { get; set; }
        public double OverDue1to30 { get; set; }
        public double OverDue31to60 { get; set; }
        public double OverDue61more { get; set; }
    }
}
