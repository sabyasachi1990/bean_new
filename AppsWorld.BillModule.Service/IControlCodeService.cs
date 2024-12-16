using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
//using AppsWorld.BillModule.Models;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
	public interface IControlCodeService : IService<ControlCode>
    {
		ControlCode GetControlCodeCode(string code);

		List<ControlCode> GetControlCodesByCatId(long CatId);

		ControlCode GetControlCode(long controlCategoryId, string name);
    }
}
