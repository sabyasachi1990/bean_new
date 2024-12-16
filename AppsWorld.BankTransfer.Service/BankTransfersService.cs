using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.Models;
using AppsWorld.BankTransferModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public class BankTransfersService : Service<BankTransfer>, IBankTransferService
    {
        private readonly IBankTransferModuleRepositoryAsync<BankTransfer> _bankTransferRepository;
        public BankTransfersService(IBankTransferModuleRepositoryAsync<BankTransfer> bankTransferRepository)
            : base(bankTransferRepository)
        {
           this._bankTransferRepository = bankTransferRepository;
        }
        public IQueryable<BankTransferModelK> GetAllBankTransferK(long companyId)
        {
            IQueryable<BankTransfer> banktransferRepository = _bankTransferRepository.Queryable();
            IQueryable<BankTransferModelK> bankTransferModelK = from b in banktransferRepository
                                                                where b.CompanyId == companyId
                                                                select new BankTransferModelK()
                                                                {
                                                                    Id = b.Id,
                                                                    CompanyId = b.CompanyId,
                                                                    DocType = b.DocType,
                                                                    SystemRefNo = b.SystemRefNo,
                                                                    DocumentState = b.DocumentState,
                                                                    DocNo = b.DocNo,
                                                                    ExCurrency = b.ExCurrency,
                                                                    UserCreated = b.UserCreated,
                                                                    CreatedDate = b.CreatedDate,
                                                                    ModifiedBy = b.ModifiedBy,
                                                                    ModifiedDate = b.ModifiedDate,
                                                                    BankClearingDate = b.BankClearingDate
                                                                };
            return bankTransferModelK.OrderByDescending(a => a.CreatedDate).AsQueryable();
        }

    }
}
