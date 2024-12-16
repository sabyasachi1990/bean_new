using AppsWorld.BankTransferModule.Entities.V2;
using AppsWorld.BankTransferModule.Models;
using AppsWorld.BankTransferModule.Service.V2;
using System.Linq;

namespace AppsWorld.BankTransferModule.Application.V2
{
    public class TransferKApplicationService
    {
        readonly ITransferKService _transferKService;

        public TransferKApplicationService(ITransferKService transferKService)
        {
            _transferKService = transferKService;
        }
        #region Kendo Call
        public IQueryable<BankTransferModelK> GetAllBankTransferK(string username, long companyId)
        {
            return _transferKService.GetAllBankTransferK(username, companyId);
        }
        #endregion
    }

}
