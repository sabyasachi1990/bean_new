using AppsWorld.BillModule.Entities;
using AppsWorld.BillModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BillModule.Service
{
    public class CreditMemoApplicationDetailService : Service<CreditMemoApplicationDetail>, ICreditMemoApplicationDetailService
    {
        private readonly IBillModuleRepositoryAsync<CreditMemoApplicationDetail> _creditMemoDetailRepository;
        public CreditMemoApplicationDetailService(IBillModuleRepositoryAsync<CreditMemoApplicationDetail> creditMemoDetailRepository) : base(creditMemoDetailRepository)
        {
            _creditMemoDetailRepository = creditMemoDetailRepository;
        }
        public List<CreditMemoApplicationDetail> GetCreditMemoDetailById(Guid Id)
        {
            return _creditMemoDetailRepository.Query(x => x.DocumentId == Id && x.CreditAmount > 0).Select().ToList();
        }
    }
}


