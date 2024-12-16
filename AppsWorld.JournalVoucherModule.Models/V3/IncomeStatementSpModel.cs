using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models.V3
{
    public class IncomeStatementSpModel
    {
        public long? Recorder { get; set; }
       // public Guid? AccountTypeId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Percentage { get; set; }

        public Guid? FRCoaId { get; set; }
        public Guid? FRPATId { get; set; }
        public Nullable<int> FRRecOrder { get; set; }
    }
}
