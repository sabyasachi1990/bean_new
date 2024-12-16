using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ISubCategoryService : IService<SubCategory>
    {
        bool GetDuplicateName(string name, Guid? id, long companyId);
        Guid? GetMaxIdFromSubCategory();
        SubCategory GetSubCategory(Guid? id);
        List<SubCategory> GetSubcategoryDetails(Guid? parentId);
        List<SubCategory> GetSubcategoryDetailsForTypeId(Guid? parentId);
        List<SubCategory> GetByCid(Guid id);
        bool GetsubcategoryName(long? companyId, string name, Guid? leadSheetId, string leadSheetName);
        List<SubCategory> GetsubcategoryByType(string leadSheetName, long? companyId);
        SubCategory GetCategory(Guid categoryId, Guid Id);
        SubCategory Getcategoryname(long? companyId, string name, Guid lid, Guid cid, string leadSheetName);
        List<SubCategory> GetSubCategoryByCompanyId(long companyId);
        List<SubCategory> GetBalanceSheetSubCategoryByCompanyId(long companyid);
    }
}
