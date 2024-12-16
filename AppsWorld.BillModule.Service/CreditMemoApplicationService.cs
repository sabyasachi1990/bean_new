using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BillModule.RepositoryPattern;
using AppsWorld.BillModule.Entities;
using Service.Pattern;
using AppsWorld.CommonModule.Infra;

namespace AppsWorld.BillModule.Service
{
    public class CreditMemoApplicationService : Service<CreditMemoApplication>, ICreditMemoApplicationService
    {
        private readonly IBillModuleRepositoryAsync<CreditMemoApplication> _creditMemoRepository;
        public CreditMemoApplicationService(IBillModuleRepositoryAsync<CreditMemoApplication> creditMemoRepository) : base(creditMemoRepository)
        {
            _creditMemoRepository = creditMemoRepository;
        }

        public CreditMemoApplication GetCreditMemoByCompanyId(Guid id)
        {
            return _creditMemoRepository.Query(e => e.Id == id && e.Status != CreditMemoApplicationStatus.Void).Select().FirstOrDefault();
        }
        public List<CreditMemoApplication> GetListofCreditMemoById(List<Guid> Id)
        {
            return _creditMemoRepository.Query(c => Id.Contains(c.Id) /*&& c.Status != CreditMemoApplicationStatus.Void */&& c.CreditAmount > 0).Select().ToList();
        }
    }
}
