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
    public class CreditMemoDetailService : Service<CreditMemoDetail>,ICreditMemoDetailService
    {
        private readonly ICreditMemoModuleRepositoryAsync<CreditMemoDetail> _creditMemoDetailRepository;
        public CreditMemoDetailService(ICreditMemoModuleRepositoryAsync<CreditMemoDetail> creditMemoDetailRepository) : base(creditMemoDetailRepository)
        {
            _creditMemoDetailRepository = creditMemoDetailRepository;
        }
        public List<CreditMemoDetail> GetCreditMemoDetailById(Guid Id)
        {
            return _creditMemoDetailRepository.Query(x => x.CreditMemoId == Id).Select().ToList();
        }
        public CreditMemoDetail GetCreditMemoDetail(Guid id)
        {
            return _creditMemoDetailRepository.Query(c => c.Id == id).Select().FirstOrDefault();
        }
    }
}


