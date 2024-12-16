using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class SubCategoryModel
    {
        public SubCategoryModel()
        {
            this.AccountModels = new List<AccountModel>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? CommonId { get; set; }
        public decimal? Balance { get; set; }
        public List<AccountModel> AccountModels { get; set; }
        public bool? IsCollapse { get; set; }
        public string RecordStatus { get; set; }
        public string ColorCode { get; set; }
        public bool IsShowZero { get; set; }

    }
}
