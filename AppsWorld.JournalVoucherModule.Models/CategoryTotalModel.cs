using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class CategoryTotalModel
    {
        public CategoryTotalModel()
        {
            this.CategoryTotalModels = new List<CategoryTotalModel>();
            this.AccountModels = new List<AccountModel>();
            this.SubCategoryModels = new List<SubCategoryModel>();
        }
        public Guid Id { get; set; }
        public bool? IsCategory { get; set; }
        public string Name { get; set; }
        public Guid? CommonId { get; set; }
        public bool? IsCollapse { get; set; }
        public decimal? Balance { get; set; }
        public List<YearModels> YearModels { get; set; }
        public Guid? LeadSheetId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ParentId { get; set; }
        public long? CompanyId { get; set; }
        public string LeadSheetName { get; set; }
        public List<AccountModel> AccountModels { get; set; }
        public List<SubCategoryModel> SubCategoryModels { get; set; }
        public int? RecOrder { get; set; }
        public bool? IsCategorised { get; set; }
        public string ColorCode { get; set; }
        public List<CategoryTotalModel> CategoryTotalModels { get; set; }
        public bool IsShowZero { get; set; }
    }
}
