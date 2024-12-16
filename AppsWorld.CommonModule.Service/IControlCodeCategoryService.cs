using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.CommonModule.Service
{
	public interface IControlCodeCategoryService : IService<ControlCodeCategory>
    {
		LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode);
        
        LookUp<string> GetInactiveControlcode(long CompanyId, string CategoryCode,
            string controlCodeKey);
        LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode, string codeKey);
        Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long CompanyId, string CategoryCode);
        Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long CompanyId, string CategoryCode, string codeKey);
    }
}
