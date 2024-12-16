using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using FrameWork;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class DoubtfulDebtAllocationService:Service<DoubtfulDebtAllocation>,IDoubtfulDebtAllocationService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<DoubtfulDebtAllocation> _doubtfulDebtAllocationRepository;

        public DoubtfulDebtAllocationService(IJournalVoucherModuleRepositoryAsync<DoubtfulDebtAllocation> doubtfulDebtAllocationRepository)
            : base(doubtfulDebtAllocationRepository)
        {
            this._doubtfulDebtAllocationRepository = doubtfulDebtAllocationRepository;
        }
        public DoubtfulDebtAllocation GetAppication(Guid id)
        {
           return _doubtfulDebtAllocationRepository.Query(x => x.Id == id && x.Status != DoubtfulDebtAllocationStatus.Reset).Select().FirstOrDefault();
        }
    }
}
