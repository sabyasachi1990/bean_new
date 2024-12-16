using AppsWorld.DashBoardModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DashBoardModule.Model
{
    public class FinancialsAdminDashBoardVM
    {
        public DateTime FromDate { get; set; }
        public string ToDate { get; set; }
        public FinancialsAdminKPI FinancialsAdminKPI { get; set; }
        public MultiDrillDownVM FinancialsAdminBlanceSheet { get; set; }
        public MultiDrillDownVM FinancialsAdminProfitandLoss { get; set; }
    }
    public class FinancialsAdminKPI
    {
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal NetProfit { get; set; }
    }
    public class FinancialsAdminData
    {
        public string Subcategory { get; set; }
        public decimal Value { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthYear { get; set; }
        public DateTime CreatedDate { get; set; }
    }
    public class FinancialsData
    {
        public string Class { get; set; }
        public decimal Value { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Monthyear { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
