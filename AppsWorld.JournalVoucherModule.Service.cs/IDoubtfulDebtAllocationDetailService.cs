using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IDoubtfulDebtAllocationDetailService:IService<DoubtfulDebtAllocationDetail>
    {
        List<DoubtfulDebtAllocationDetail> GetApplicationDetails(Guid? id);
    }
}
