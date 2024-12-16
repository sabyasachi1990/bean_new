using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;

namespace AppsWorld.BillModule.Service
{
	public interface IAutoNumberService : IService<AutoNumber> /*IService<BeanAutoNumber>*/
    {
        AutoNumber GetAutoNumber(long companyId, string entityType);
        //BeanAutoNumber GetAutoNumber(long companyId, string entityType);
    }
}
