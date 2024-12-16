using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;

using Service.Pattern;
using AppsWorld.Framework;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.ReceiptModule.Service
{
	public interface IControlCodeCategoryService : IService<ControlCodeCategory>
    {
       LookUpCategory<string> GetByCategoryCodeCategory(long CompanyId, string CategoryCode, string codeKey);
       
    }
}
