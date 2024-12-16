
using AppsWorld.BankTransferModule.Entities.V2;
using AppsWorld.BankTransferModule.Models;
using Service.Pattern;
using System.Linq;

namespace AppsWorld.BankTransferModule.Service.V2
{
    public interface ITransferKService : IService<BankTransferK>
    {
        IQueryable<BankTransferModelK> GetAllBankTransferK(string username, long companyId);
    }
}
