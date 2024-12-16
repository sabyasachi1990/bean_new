using AppsWorld.BankReconciliationModule.Entities;
using AppsWorld.BankReconciliationModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
    public class WithdrawalService:Service<Withdrawal>,IWithdrawalService
    {
        private readonly IBankReconciliationModuleRepositoryAsync<Withdrawal> _withdrawalRepository;
        public WithdrawalService(IBankReconciliationModuleRepositoryAsync<Withdrawal> withdrawalRepository)
            : base(withdrawalRepository)
        {
            _withdrawalRepository = withdrawalRepository;
        }
        public Withdrawal GetWithdraw(Guid id, long companyid)
        {
            return _withdrawalRepository.Query(a => a.Id == id && a.CompanyId == companyid).Select().FirstOrDefault();
        }


    }
}
