using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities.Models.V3.Journal
{
    public class CategoryV3 :Entity
    {
        public Guid Id { get; set; }
        public long? CompanyId { get; set; }
        public string Name { get; set; }
       // public string Type { get; set; }
        //public int? Recorder { get; set; }
        public bool? IsIncomeStatement { get; set; }
       // public Guid? LeadsheetId { get; set; }
       // public string AccountClass { get; set; }
       // public string ColorCode { get; set; }
        //public bool? IsCollapse { get; set; }

    }
}
