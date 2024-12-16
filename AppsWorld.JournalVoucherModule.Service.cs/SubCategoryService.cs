using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class SubCategoryService : Service<SubCategory>, ISubCategoryService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<SubCategory> _subCategoryTypeRepository;
        public SubCategoryService(IJournalVoucherModuleRepositoryAsync<SubCategory> subCategoryTypeRepository)
            : base(subCategoryTypeRepository)
        {
            _subCategoryTypeRepository = subCategoryTypeRepository;
        }

        public bool GetDuplicateName(string name, Guid? id, long companyId)
        {
            return _subCategoryTypeRepository.Query(c => c.Name.ToUpper() == name.ToUpper() && c.CompanyId == companyId && c.Id != id).Select().Any();
        }
        public Guid? GetMaxIdFromSubCategory()
        {

            var lstData = _subCategoryTypeRepository.Query().Select(c => c.Id).FirstOrDefault();
            return lstData;

        }
        public SubCategory GetSubCategory(Guid? id)
        {
            return _subCategoryTypeRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
        public List<SubCategory> GetSubcategoryDetails(Guid? parentId)
        {
            return _subCategoryTypeRepository.Query(c => c.ParentId == parentId).Select().ToList();
        }

        public List<SubCategory> GetSubcategoryDetailsForTypeId(Guid? parentId)
        {
            return _subCategoryTypeRepository.Query(c => c.TypeId == parentId).Select().ToList();
        }
        public List<SubCategory> GetByCid(Guid categoryid)
        {
            return _subCategoryTypeRepository.Query(a => a.CategoryId == categoryid).Select().ToList();
        }
        public bool GetsubcategoryName(long? companyId, string name, Guid? lid, string leadsheetName)
        {
            //var leadsheetcategory = _leadSheetCategoryRepository.Queryable().Where(a => a.LeadsheetId == lid && a.Name == name).Any();
            var category = _subCategoryTypeRepository.Queryable().Where(a => a.TypeId == lid && a.Name == name && a.CompanyId == companyId && a.Type == leadsheetName).Any();
            //if (/*leadsheetcategory ||*/ category)
            //    return false;
            //else
            return category;
        }

        public List<SubCategory> GetsubcategoryByType(string leadSheetName, long? companyId)
        {
            return _subCategoryTypeRepository.Query(a => a.CompanyId == companyId && a.Type == leadSheetName).Select().ToList();
        }
        public SubCategory GetCategory(Guid categoryId, Guid id)
        {
            return _subCategoryTypeRepository.Query(a => a.CategoryId == categoryId && a.Id == id).Select().FirstOrDefault();
        }
        public SubCategory Getcategoryname(long? companyId, string name, Guid lid, Guid cid, string leadSheetName)
        {
            return _subCategoryTypeRepository.Queryable().Where(a => a.TypeId == lid && a.Name == name && a.CompanyId == companyId && a.Id == cid && a.Type == leadSheetName).FirstOrDefault();
        }

        public List<SubCategory> GetSubCategoryByCompanyId(long companyId)
        {
            return _subCategoryTypeRepository.Queryable().Where(a => a.CompanyId == companyId && a.IsIncomeStatement == true).ToList();
        }

        public List<SubCategory> GetBalanceSheetSubCategoryByCompanyId(long companyid)
        {
            return _subCategoryTypeRepository.Queryable().Where(a => a.CompanyId == companyid && a.IsIncomeStatement != true).ToList();

        }
    }
}


