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
   public  class CreditMemoApplicationDetailService:Service<CreditMemoApplicationDetail>,ICreditMemoApplicationDetailService
    {
        private readonly ICreditMemoModuleRepositoryAsync<CreditMemoApplicationDetail> _creditMemoApplicationDetailRepository;
        public CreditMemoApplicationDetailService(ICreditMemoModuleRepositoryAsync<CreditMemoApplicationDetail> creditMemoApplicationDetailRepository) :base(creditMemoApplicationDetailRepository)
        {
            _creditMemoApplicationDetailRepository = creditMemoApplicationDetailRepository;
        }
        public List<CreditMemoApplicationDetail> GetAllCreditMemoDetail(Guid creditMemoApplicationId)
        {
            return _creditMemoApplicationDetailRepository.Query(c => c.CreditMemoApplicationId == creditMemoApplicationId && c.CreditAmount>0).Select().ToList();
        }
        //public List<CreditMemoApplicationDetail> GetCreditMemoDetailById(Guid documentId)
        //{
        //    return _creditMemoApplicationDetailRepository.Query(c => c.DocumentId == documentId).Select().ToList();
        //}
    }
}
