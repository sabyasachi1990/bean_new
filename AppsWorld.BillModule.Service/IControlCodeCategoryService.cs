using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
	public interface IControlCodeCategoryService : IService<ControlCodeCategory>
    {
		LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode);

		ControlCodeCategory GetByCategoryCode(long CompanyId, string CategoryCode);
        LookUp<string> GetInactiveControlcode(long CompanyId, string CategoryCode,
          string controlCodeKey);
    }
}
