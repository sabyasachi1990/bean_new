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
    public class CreditMemoService : Service<CreditMemo>, ICreditMemoService
    {
        private readonly IBillModuleRepositoryAsync<CreditMemo> _creditMemoRepository;
        public CreditMemoService(IBillModuleRepositoryAsync<CreditMemo> creditMemoRepository) : base(creditMemoRepository)
        {
            _creditMemoRepository = creditMemoRepository;
        }
        public CreditMemo GetCreditMemoByCompanyId(long companyId)
        {
            return _creditMemoRepository.Query(x => x.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CreditMemo GetDocNo(string docNo, long companyId)
        {
            return _creditMemoRepository.Query(c => c.DocNo == docNo && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CreditMemo GetCmById(Guid? id, long companyId)
        {
            return _creditMemoRepository.Query(c => c.Id == id && c.CompanyId == companyId).Select().FirstOrDefault();
        }
        public CreditMemo GetLastCreditMemo(long companyId)
        {
            return _creditMemoRepository.Query(a => a.CompanyId == companyId && a.DocSubType != DocTypeConstants.OpeningBalance).Select().OrderByDescending(a => a.CreatedDate).FirstOrDefault();
        }
    }
}
