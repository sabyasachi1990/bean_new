using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class LeadSheetTypeModel
    {

        public LeadSheetTypeModel()
        {
            this.LeadSheetTotalModels = new List<LeadSheetTotalModel>();
        }

        public bool? IsNonCurrent { get; set; }
        public NonCurrentModel NonCurrent { get; set; }
        public bool? IsCurrent { get; set; }
        public CurrentModel Current { get; set; }
        public bool? IsLeadSheet { get; set; }
        public Guid MainId { get; set; }
        public List<LeadSheetTotalModel> LeadSheetTotalModels { get; set; }
        public AccountModel NonCurrentTotal { get; set; }
        public AccountModel CurrentTotal { get; set; }
        public List<YearModels> YearModels { get; set; }
    }

    public class NonCurrentModel
    {
        public Guid MainId { get; set; }
        public List<LeadSheetTotalModel> LeadSheetTotalModels { get; set; }
        public List<YearModels> YearModels { get; set; }
    }
    public class CurrentModel
    {
        public Guid MainId { get; set; }
        public List<LeadSheetTotalModel> LeadSheetTotalModels { get; set; }
        public List<YearModels> YearModels { get; set; }
    }
}
