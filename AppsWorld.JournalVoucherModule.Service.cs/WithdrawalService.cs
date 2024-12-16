using System;
using System.Collections.Generic;
using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.RepositoryPattern;
using AppsWorld.JournalVoucherModule.Service;
using Service.Pattern;
using System.Linq;

namespace AppsWorld.JournalVoucherModule.Service
{
    public class WithdrawalService : Service<Withdrawal>, IWithdrawalService
    {
        private readonly IJournalVoucherModuleRepositoryAsync<Withdrawal> _bankWithdrawalRepository;

        public WithdrawalService(IJournalVoucherModuleRepositoryAsync<Withdrawal> bankWithdrwalRepository)
            : base(bankWithdrwalRepository)
        {
            _bankWithdrawalRepository = bankWithdrwalRepository;
        }
        public Withdrawal GetWithdrawal(Guid? id, long companyId, string docType)
        {
            return _bankWithdrawalRepository.Query(x => x.Id == id && x.CompanyId == companyId && x.DocType == docType).Select().FirstOrDefault();
        }
        public Withdrawal GetById(Guid? id)
        {
            return _bankWithdrawalRepository.Query(x => x.Id == id).Select().FirstOrDefault();
        }
    }
}
