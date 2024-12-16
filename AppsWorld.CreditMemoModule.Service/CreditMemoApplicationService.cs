using AppsWorld.CreditMemoModule.Entities;
using AppsWorld.CreditMemoModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.CreditMemoModule.Service
{
    public class CreditMemoApplicationService : Service<CreditMemoApplication>, ICreditMemoApplicationService
    {
        public readonly ICreditMemoModuleRepositoryAsync<CreditMemoApplication> _creditMemoApplicatonRepository;
        public CreditMemoApplicationService(ICreditMemoModuleRepositoryAsync<CreditMemoApplication> creditMemoApplicatonRepository) : base(creditMemoApplicatonRepository)
        {
            _creditMemoApplicatonRepository = creditMemoApplicatonRepository;
        }
        public List<CreditMemoApplication> GetAllMemoApplication(Guid creditMemoId)
        {
            return _creditMemoApplicatonRepository.Query(c => c.CreditMemoId == creditMemoId).Select().OrderByDescending(a => a.CreatedDate).ToList();
        }
        public CreditMemoApplication GetCreditMemoByCompanyId(long companyId)
        {
            return _creditMemoApplicatonRepository.Query(a => a.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CreditMemoApplication GetAllCreditMemo(Guid creditMemoId, Guid cmApplicationId, long companyId)
        {
            return _creditMemoApplicatonRepository.Query(a => a.Id == cmApplicationId && a.CreditMemoId == creditMemoId && a.CompanyId == companyId).Include(c => c.CreditMemoApplicationDetails).Select().FirstOrDefault();
        }
        public CreditMemoApplication GetAllCreditMemoApplication(Guid cmApplicationId, long companyId)
        {
            return _creditMemoApplicatonRepository.Query(a => a.Id == cmApplicationId && a.CompanyId == companyId).Include(c => c.CreditMemoApplicationDetails).Select().FirstOrDefault();
        }
        public CreditMemoApplication GetCreditMemoById(Guid id)
        {
            return _creditMemoApplicatonRepository.Query(x => x.Id == id).Include(c => c.CreditMemoApplicationDetails).Select().OrderByDescending(c => c.CreatedDate).FirstOrDefault();
        }
        public List<CreditMemoApplication> GetCreditMemoApp(Guid id)
        {
            return _creditMemoApplicatonRepository.Query(x => x.CreditMemoId == id && x.Status != CommonModule.Infra.CreditMemoApplicationStatus.Void).Select().ToList();
        }
        public CreditMemoApplication GetCreditMemo(Guid id)
        {
            return _creditMemoApplicatonRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
    }
}
