using AppsWorld.CommonModule.Infra;
using AppsWorld.DebitNoteModule.Entities;
using AppsWorld.DebitNoteModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.DebitNoteModule.Service
{
    public class DoubtfulDebtAllocationService : Service<DoubtfulDebtAllocation>, IDoubtfulDebtAllocationService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<DoubtfulDebtAllocation> _doubtfulDebtAllocation;
        public DoubtfulDebtAllocationService(IDebitNoteMoluleRepositoryAsync<DoubtfulDebtAllocation> doubtfulDebtAllocation)
            : base(doubtfulDebtAllocation)
        {
            _doubtfulDebtAllocation = doubtfulDebtAllocation;
        }

        public DoubtfulDebtAllocation GetDoubtfulDebtAllocation(Guid Id)
        {
            return _doubtfulDebtAllocation.Query(c => c.Id == Id && c.Status != DoubtfulDebtAllocationStatus.Reset).Select().FirstOrDefault();
        }
    }
}
