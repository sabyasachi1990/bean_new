using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.Entities;
using AppsWorld.BankWithdrawalModule.RepositoryPattern;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public class WithdrawalDetailService : Service<WithdrawalDetail>, IWithdrawalDetailService
    {
        private readonly IBankWithdrawalModuleRepositoryAsync<WithdrawalDetail> _bankWithdrawalRepository;

        public WithdrawalDetailService(IBankWithdrawalModuleRepositoryAsync<WithdrawalDetail> bankWithdrawalRepository)
            : base(bankWithdrawalRepository)
        {
            _bankWithdrawalRepository = bankWithdrawalRepository;
        }
        public List<WithdrawalDetail> GetAllWithdraw(Guid withdrawalId)
        {
            return _bankWithdrawalRepository.Query(x => x.WithdrawalId == withdrawalId).Select().ToList();
        }
        public WithdrawalDetail GetWithDrawal(Guid id)
        {
            return _bankWithdrawalRepository.Queryable().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
