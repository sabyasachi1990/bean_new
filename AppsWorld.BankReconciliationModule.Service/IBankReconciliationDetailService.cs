using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using Service.Pattern;

namespace AppsWorld.BankReconciliationModule.Service
{
    public interface IBankReconciliationDetailService : IService<BankReconciliationDetail>
    {
        BankReconciliationDetail GetBankReconciliationDetail(Guid id);

        List<BankReconciliationDetail> GetBankRDetails(Guid BankRId);
        BankReconciliationDetail GetBabkdetail(Guid id, Guid bankReconciliationId);
        List<BankReconciliationDetail> GetListOfBankRecDetail(Guid bankRecId, DateTime? bankRecDate);
        List<BankReconciliationDetail> GetBRCDetailByDocumentId(List<Guid> documentId);
        BankReconciliationDetail GetBankReconciliationDetailbyDocId(Guid? docid);

        //Brc Details based in the brc and document id
        List<BankReconciliationDetail> GetBRCDetailByDocumentIdandBrcId(List<Guid> documentId, Guid brcId);
    }
}
