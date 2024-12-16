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
    public class DoubtfulDebtAllocationDetailService : Service<DoubtfulDebtAllocationDetail>,IDoubtfulDebtAllocationDetailService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<DoubtfulDebtAllocationDetail> _doubtfulDebtAllocationDetailRepository;

        public DoubtfulDebtAllocationDetailService(IJournalVoucherModuleRepositoryAsync<DoubtfulDebtAllocationDetail> doubtfulDebtAllocationDetailRepository)
            : base(doubtfulDebtAllocationDetailRepository)
        {
            this._doubtfulDebtAllocationDetailRepository = doubtfulDebtAllocationDetailRepository;
        }
        public List<DoubtfulDebtAllocationDetail> GetApplicationDetails(Guid? id)
        {
            return _doubtfulDebtAllocationDetailRepository.Query(x => x.DocumentId == id.Value && x.DocumentType == DocTypeConstants.Invoice).Select().ToList();
        }
    }
}
