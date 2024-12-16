using AppsWorld.DebitNoteModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Service
{
    public interface IDoubtfulDebtAllocationService:IService<DoubtfulDebtAllocation>
    {
        DoubtfulDebtAllocation GetDoubtfulDebtAllocation(Guid Id);
    }
}
