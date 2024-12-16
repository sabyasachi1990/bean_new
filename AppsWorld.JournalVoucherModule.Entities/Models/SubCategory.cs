using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Entities
{
    public class SubCategory : Entity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
        public int? Recorder { get; set; }
        public long? CompanyId { get; set; }
        public string Type { get; set; }
        public Guid? TypeId { get; set; }
        public string SubCategoryOrder { get; set; }
        public Guid? ParentId { get; set; }
        public bool? IsIncomeStatement { get; set; }
        public string AccountClass { get; set; }
        public string ColorCode { get; set; }
        public bool? IsCollapse { get; set; }
    }
}
