using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models.V3
{
    public class BalanceSheetSpModel
    {
        public long? Recorder { get; set; }
        public Guid FRCoaId { get; set; }
        public Guid FRPATId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? SubCategoryId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public decimal? Balance { get; set; }
        public decimal? Percentage { get; set; }
        public Nullable<int> FRRecOrder { get; set; }
        public long? CoaId { get; set; }
    }
}
