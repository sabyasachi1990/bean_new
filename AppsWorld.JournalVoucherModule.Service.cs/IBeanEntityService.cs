using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.JournalVoucherModule.Entities;

namespace AppsWorld.JournalVoucherModule.Service
{
	public interface IBeanEntityService : IService<BeanEntity>
    {
		BeanEntity GetEntityById(Guid? id);
        Dictionary<Guid, string> GetEntityByid(List<Guid?> entityIds, long companyId);
        string GetEntityByid(Guid? entityIds, long companyId);
        decimal? GetCteditLimitsValue(Guid id);
    }
}
