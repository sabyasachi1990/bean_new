using ModuleUrl.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleUrl.CommonConstant
{
	public class ModuleDetailUrl
	{
		CommonModuleContext commonModuleContext = new CommonModuleContext();
		public long GetByCompany(long companyId)
		{
			return commonModuleContext.ModuleMasters.Where(a => a.CompanyId == 0).Select(a => a.Id).FirstOrDefault();
		}

		public string GetUrl(string heading, string moduleName, long moduleMasterId)
		{
			return commonModuleContext.ModuleDetails.Where(a => a.Heading == heading).Select(a => a.PageUrl).FirstOrDefault();
		}

	}
}
