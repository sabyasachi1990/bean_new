using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankTransferModule.Entities;
using AppsWorld.BankTransferModule.Models;

namespace AppsWorld.BankTransferModule.Service
{
    public interface IBankTransferService : IService<BankTransfer>
    {
        Task<IQueryable<BankTransferModelK>> GetAllBankTransferKAsync(string username, long companyId);
      
        BankTransfer GetBankTransferById(Guid id, long companyId);
        BankTransfer GetCompanyIdById(long companyId);
        BankTransfer GetDocTypeAndCompanyid(string DocType, long companyId);
        BankTransfer DuplicateBankTransfer(string DocNo, string docType, long companyId);
        BankTransfer GetBankTransferLU(Guid banktransferId, long companyId);
        BankTransfer GetBankTransferDocNo(Guid id, string docNo, long companyId);
        List<string> GetAutoNumber(long companyId);
        List<BankTransfer> GetAllBankTransfer(long companyId);

        bool IsVoid(long companyId, Guid id);
        List<long> lstServiceCompanyIds(long companyId, string username);
    }
}
