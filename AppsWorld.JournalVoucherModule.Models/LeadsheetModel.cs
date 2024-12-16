using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Models
{
    public class LeadsheetModel
    {
        public LeadsheetModel()
        {
            this.CategoryTotalModels = new List<CategoryTotalModel>();
            this.AccountModels = new List<AccountModel>();
            this.CategoryModels = new List<Models.CategoryModel>();
        }

        public Guid Id { get; set; }
        public Guid MainId { get; set; }
        public Guid? LeadSheetId { get; set; }
        public string Name { get; set; }
        public Guid? CommonId { get; set; }
        public bool? IsCollapse { get; set; }
        public string PriorYear { get; set; }
        public string CurrentYear { get; set; }
        public Guid ParentId { get; set; }
        public string LeadSheetType { get; set; }
        public string Index { get; set; }
        public List<CategoryTotalModel> CategoryTotalModels { get; set; }
        public List<CategoryModel> CategoryModels { get; set; }
        public List<AccountModel> AccountModels { get; set; }
        //public List<AccountAnnotationModel> AccountAnnotationModel { get; set; }
    }


    public class CategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid? LeadSheetId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? CommonId { get; set; }
        public Guid ParentId { get; set; }
        public Nullable<long> CompanyId { get; set; }
        public string LeadSheetName { get; set; }
        public List<AccountModel> AccountModels { get; set; }
        public List<SubCategoryModel> SubCategoryModels { get; set; }
        public bool? IsCollapse { get; set; }
        public string RecordStatus { get; set; }
        public bool? IsCategorised { get; set; }
        public string ScreenName { get; set; }
        public string ColorCode { get; set; }
    }
}
