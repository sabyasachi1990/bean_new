using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.ReceiptModule.Entities;
using AppsWorld.ReceiptModule.Models;
using Service.Pattern;

namespace AppsWorld.ReceiptModule.Service
{
	public interface IBeanEntityService : IService<BeanEntity>
    {
		BeanEntity GetEntityById(Guid id);
        string GetEntityNameById(Guid id);
        decimal? GetEntityCreditTermsValue(Guid id);
    }
}
