using AppsWorld.InvoiceModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.InvoiceModule.Service
{
    public interface IDoubtfulDebtallocationDetailService : IService<DoubtfulDebtAllocationDetail>
    {
        List<DoubtfulDebtAllocationDetail> GetDoubtfulDebtallocationdetailById(Guid Id);
        List<DoubtfulDebtAllocationDetail> GetDoubtfuDebtById(Guid ddAclocationId);
        DoubtfulDebtAllocationDetail DDDetail(Guid invoiceId);
        List<DoubtfulDebtAllocationDetail> GetDoubtfuDebtByDocId(List<Guid> documentIds);
        List<DoubtfulDebtAllocationDetail> GetDoubtfulDebtallocationdetailByDocumentId(Guid Id, string docType);
        List<DoubtfulDebtAllocationDetail> GetDoubtfuAllocationByIds(List<Guid> ids);
        DoubtfulDebtAllocationDetail GetDDallocationdetailById(Guid guid);
    }
}
