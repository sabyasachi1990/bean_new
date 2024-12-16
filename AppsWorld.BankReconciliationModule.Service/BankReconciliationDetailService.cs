using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using Service.Pattern;

namespace AppsWorld.BankReconciliationModule.Service
{
    public class BankReconciliationDetailService : Service<BankReconciliationDetail>, IBankReconciliationDetailService
    {
        private readonly IBankReconciliationModuleRepositoryAsync<BankReconciliationDetail> _bankReconciliationRepository;

        public BankReconciliationDetailService(IBankReconciliationModuleRepositoryAsync<BankReconciliationDetail> bankReconciliationRepository)
            : base(bankReconciliationRepository)
        {
            this._bankReconciliationRepository = bankReconciliationRepository;
        }

        public BankReconciliationDetail GetBankReconciliationDetail(Guid id)
        {
            return _bankReconciliationRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }

        public List<BankReconciliationDetail> GetBankRDetails(Guid BankRId)
        {
            return _bankReconciliationRepository.Queryable().Where(c => c.BankReconciliationId == BankRId).ToList();
        }

        public BankReconciliationDetail GetBabkdetail(Guid id, Guid bankReconciliationId)
        {
            return _bankReconciliationRepository.Query(a => a.Id == id && a.BankReconciliationId == bankReconciliationId).Select().FirstOrDefault();
        }
        public List<BankReconciliationDetail> GetListOfBankRecDetail(Guid bankRecId, DateTime? bankRecDate)
        {
            return _bankReconciliationRepository.Query(a => a.BankReconciliationId == bankRecId && a.ClearingStatus == "Cleared" && a.ClearingDate <= bankRecDate).Select().ToList();
        }
        public List<BankReconciliationDetail> GetBRCDetailByDocumentId(List<Guid> documentId)
        {
            return _bankReconciliationRepository.Query(a => documentId.Contains(a.DocumentId)).Select().ToList();
        }
        public BankReconciliationDetail GetBankReconciliationDetailbyDocId(Guid? docid)
        {
            return _bankReconciliationRepository.Query(x => x.DocumentId == docid).Select().FirstOrDefault();
        }
        public List<BankReconciliationDetail> GetBRCDetailByDocumentIdandBrcId(List<Guid> documentId, Guid brcId)
        {
            return _bankReconciliationRepository.Query(a => documentId.Contains(a.DocumentId) && a.BankReconciliationId != brcId).Select().ToList();
        }
    }
}
