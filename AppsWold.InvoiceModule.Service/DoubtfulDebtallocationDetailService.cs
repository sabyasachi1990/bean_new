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
    public class DoubtfulDebtallocationDetailService : Service<DoubtfulDebtAllocationDetail>, IDoubtfulDebtallocationDetailService
    {
        private readonly IInvoiceModuleRepositoryAsync<DoubtfulDebtAllocationDetail> _doubtfulDebtAllocationDetail;
        public DoubtfulDebtallocationDetailService(IInvoiceModuleRepositoryAsync<DoubtfulDebtAllocationDetail> doubtfulDebtAllocationDetail)
            : base(doubtfulDebtAllocationDetail)
        {
            _doubtfulDebtAllocationDetail = doubtfulDebtAllocationDetail;
        }

        public List<DoubtfulDebtAllocationDetail> GetDoubtfulDebtallocationdetailById(Guid Id)
        {
            return _doubtfulDebtAllocationDetail.Query(c => c.DocumentId == Id && c.DocumentType == DocTypeConstants.Invoice && c.AllocateAmount > 0).Select().ToList();
        }

        public List<DoubtfulDebtAllocationDetail> GetDoubtfuDebtById(Guid ddAclocationId)
        {
            return _doubtfulDebtAllocationDetail.Query(c => c.DoubtfulDebtAllocationId == ddAclocationId).Select().ToList();
        }
        public DoubtfulDebtAllocationDetail DDDetail(Guid invoiceId)
        {
            return _doubtfulDebtAllocationDetail.Query(x => x.DoubtfulDebtAllocationId == invoiceId).Select().FirstOrDefault();
        }
        public List<DoubtfulDebtAllocationDetail> GetDoubtfuDebtByDocId(List<Guid> documentIds)
        {
            return _doubtfulDebtAllocationDetail.Query(c => documentIds.Contains(c.DocumentId)).Select().ToList();
        }
        public List<DoubtfulDebtAllocationDetail> GetDoubtfulDebtallocationdetailByDocumentId(Guid Id, string docType)
        {
            return _doubtfulDebtAllocationDetail.Query(c => c.DocumentId == Id && c.DocumentType == docType && c.AllocateAmount > 0)/*.Include(a=>a.DoubtfulDebtAllocation.Invoice)*/.Select().ToList();
        }
        public List<DoubtfulDebtAllocationDetail> GetDoubtfuAllocationByIds(List<Guid> ids)
        {
            return _doubtfulDebtAllocationDetail.Query(c => ids.Contains(c.Id)).Select().ToList();
        }

        public DoubtfulDebtAllocationDetail GetDDallocationdetailById(Guid guid)
        {
            return _doubtfulDebtAllocationDetail.Query(x => x.Id == guid).Select().FirstOrDefault();
        }
    }

}
