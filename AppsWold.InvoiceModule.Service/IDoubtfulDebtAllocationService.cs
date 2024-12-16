using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IDoubtfulDebtAllocationService:IService<DoubtfulDebtAllocation>
    {
        DoubtfulDebtAllocation GetDoubtfulDebtAllocation(Guid Id);
        List<DoubtfulDebtAllocation> GetDoubtfuldbyId(Guid id);
        List<DoubtfulDebtAllocation> GetAllDoubtfuldbtById(Guid Id);
        DoubtfulDebtAllocation GetAllDebtful(Guid Id);
        DoubtfulDebtAllocation GetAllDebtNote(Guid id);
        DoubtfulDebtAllocation GetAllDoubtFulNote(Guid doubtfulDebtId, Guid Id, long companyId);
        DoubtfulDebtAllocation GetDoubtfuldebtIdAndCompanyId(Guid Id, Guid ddAclocationId, long companyId);
        DoubtfulDebtAllocation GetDoubtfilDebtCompanyId(long companyId);
        List<DoubtfulDebtAllocation> GetDDAllocationByCDetailid(List<Guid> Id);
    }
}
