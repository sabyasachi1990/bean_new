using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.CommonModule.Entities;
using Service.Pattern;

namespace AppsWorld.CommonModule.Service
{
	public interface IControlCodeService : IService<ControlCode>
    {
		ControlCode GetControlCodeCode(string code);

		List<ControlCode> GetControlCodesByCatId(long CatId);

        List<ControlCode> GetControlCodeByCategoryId(long categoryId);
        Task<List<ControlCode>> GetControlCodeByCategoryIdAsync(long categoryId);
        Task<List<ControlCode>> GetControlCodesByCatIdAsync(long CatId);
    }
}
