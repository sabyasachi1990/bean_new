using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Service
{
	public interface IControlCodeCategoryService : IService<ControlCodeCategory>
    {
		LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode);

        Task<LookUpCategory<string>> GetByCategoryCodeCategoryAsync(long CompanyId, string CategoryCode);
    }
}
