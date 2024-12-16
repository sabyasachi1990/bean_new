using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IBankTransfersService : IService<BankTransfer>
    {
        IQueryable<BankTransferModelK> GetAllBankTransferK(long companyId);
    }
}
