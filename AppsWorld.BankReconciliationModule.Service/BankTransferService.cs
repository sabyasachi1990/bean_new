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
    public class BankTransferService:Service<BankTransfer>,IBankTransferService
    {
        private readonly IBankReconciliationModuleRepositoryAsync<BankTransfer> _bankTransferrepository;
        public BankTransferService(IBankReconciliationModuleRepositoryAsync<BankTransfer> bankTransferrepository)
            : base(bankTransferrepository)
        {
            _bankTransferrepository = bankTransferrepository;
        }
        public BankTransfer GetBankTran(Guid id, long companyid)
        {
            return _bankTransferrepository.Query(a => a.Id == id && a.CompanyId == companyid).Select().FirstOrDefault();
        }

    }
}
