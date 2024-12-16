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
    public class DoubtfulDebtallocationDetailService : Service<DoubtfulDebtAllocationDetail>, IDoubtfulDebtallocationDetailService
    {
        private readonly IDebitNoteMoluleRepositoryAsync<DoubtfulDebtAllocationDetail> _doubtfulDebtAllocationDetail;
        public DoubtfulDebtallocationDetailService(IDebitNoteMoluleRepositoryAsync<DoubtfulDebtAllocationDetail> doubtfulDebtAllocationDetail)
            : base(doubtfulDebtAllocationDetail)
        {
            _doubtfulDebtAllocationDetail = doubtfulDebtAllocationDetail;
        }

        public List<DoubtfulDebtAllocationDetail> GetDoubtfulDebtallocationdetailById(Guid Id)
        {
            return _doubtfulDebtAllocationDetail.Query(c => c.DocumentId == Id && c.DocumentType == DocTypeConstants.DebitNote && c.AllocateAmount > 0).Select().ToList();
        }
    }

}
