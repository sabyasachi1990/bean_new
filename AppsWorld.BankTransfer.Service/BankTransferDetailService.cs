using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public class BankTransferDetailService : Service<BankTransferDetail>, IBankTransferDetailService
    {
        private readonly IBankTransferModuleRepositoryAsync<BankTransferDetail> _bankTransferDetailRepository;
        public BankTransferDetailService(IBankTransferModuleRepositoryAsync<BankTransferDetail> bankTransferDetailRepository)
            : base(bankTransferDetailRepository)
        {
            _bankTransferDetailRepository = bankTransferDetailRepository;
        }
        public List<BankTransferDetail> GetBankTransfeById(Guid BankTransfeId)
        {
            return _bankTransferDetailRepository.Query(a => a.BankTransferId == BankTransfeId).Select().ToList();
        }
    }
}
