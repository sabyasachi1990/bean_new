using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Entities;
using AppsWorld.InvoiceModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public class DoubtfulDebtAllocationService : Service<DoubtfulDebtAllocation>, IDoubtfulDebtAllocationService
    {
        private readonly IInvoiceModuleRepositoryAsync<DoubtfulDebtAllocation> _doubtfulDebtAllocation;
        public DoubtfulDebtAllocationService(IInvoiceModuleRepositoryAsync<DoubtfulDebtAllocation> doubtfulDebtAllocation)
            : base(doubtfulDebtAllocation)
        {
            _doubtfulDebtAllocation = doubtfulDebtAllocation;
        }

        public DoubtfulDebtAllocation GetDoubtfulDebtAllocation(Guid Id)
        {
            return _doubtfulDebtAllocation.Query(c => c.Id == Id && c.Status != DoubtfulDebtAllocationStatus.Reset).Select().FirstOrDefault();
        }
        public List<DoubtfulDebtAllocation> GetDoubtfuldbyId(Guid id)
        {
            return _doubtfulDebtAllocation.Query(c => c.InvoiceId == id).Select().OrderByDescending(c => c.DoubtfulDebtAllocationNumber).ToList();
        }
        public List<DoubtfulDebtAllocation> GetAllDoubtfuldbtById(Guid Id)
        {
            return _doubtfulDebtAllocation.Query(c => c.InvoiceId == Id && c.Status != DoubtfulDebtAllocationStatus.Reset).Select().ToList();
        }
        public DoubtfulDebtAllocation GetAllDebtful(Guid Id)
        {
            return _doubtfulDebtAllocation.Query(e => e.Id == Id).Include(a => a.DoubtfulDebtAllocationDetails).Select().FirstOrDefault();
        }
        public DoubtfulDebtAllocation GetAllDebtNote(Guid id)
        {
            return _doubtfulDebtAllocation.Query(a => a.InvoiceId == id).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
        public DoubtfulDebtAllocation GetAllDoubtFulNote(Guid doubtfulDebtId, Guid Id, long companyId)
        {
            return _doubtfulDebtAllocation.Query(c => c.InvoiceId == doubtfulDebtId && c.Id == Id && c.CompanyId == companyId).Include(c => c.DoubtfulDebtAllocationDetails).Select().FirstOrDefault();
        }
        public DoubtfulDebtAllocation GetDoubtfuldebtIdAndCompanyId(Guid Id, Guid ddAclocationId, long companyId)
        {
            return _doubtfulDebtAllocation.Query(c => c.InvoiceId == Id && c.Id == ddAclocationId && c.CompanyId == companyId).Include(c => c.DoubtfulDebtAllocationDetails).Select().FirstOrDefault();
        }
        public DoubtfulDebtAllocation GetDoubtfilDebtCompanyId(long companyId)
        {
            return _doubtfulDebtAllocation.Query(c => c.CompanyId == companyId).Select().FirstOrDefault();
        }

        public DoubtfulDebtAllocation GetDebtAllocationByInvoiceId(Guid Id)
        {
            return _doubtfulDebtAllocation.Query(c => c.Id == Id && c.Status != DoubtfulDebtAllocationStatus.Reset).Select().FirstOrDefault();
        }
        public List<DoubtfulDebtAllocation> GetDDAllocationByCDetailid(List<Guid> Id)
        {
            return _doubtfulDebtAllocation.Query(c => Id.Contains(c.Id)/* && c.Status != DoubtfulDebtAllocationStatus.Reset*/)/*.Include(a => a.Invoice)*/.Select().ToList();
        }
    }
}
