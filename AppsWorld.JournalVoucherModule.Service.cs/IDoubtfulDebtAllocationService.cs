using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IDoubtfulDebtAllocationService:IService<DoubtfulDebtAllocation>
    {
        DoubtfulDebtAllocation GetAppication(Guid id);
    }
}
