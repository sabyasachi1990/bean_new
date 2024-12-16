using AppsWorld.BankTransferModule.Entities.Models;
using AppsWorld.BankTransferModule.RepositoryPattern;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankTransferModule.Service
{
    public class SettlementDetailService : Service<SettlementDetail>, ISettlementDetailService
    {
        private readonly IBankTransferModuleRepositoryAsync<SettlementDetail> _settlementDetailRepository;
        public SettlementDetailService(IBankTransferModuleRepositoryAsync<SettlementDetail> settlementDetailRepository) : base(settlementDetailRepository)
        {
            this._settlementDetailRepository = settlementDetailRepository;
        }

        public List<SettlementDetail> GetListOfSettlemetDetails(Guid transferId, long withdrawalServiceEntityId, long depositServiceEntityId, string currency, DateTime transferDate)
        {
            return _settlementDetailRepository.Query(a => a.BankTransferId == transferId && (a.ServiceCompanyId == withdrawalServiceEntityId || a.ServiceCompanyId == depositServiceEntityId) && a.Currency == currency).Select().ToList();
        }
        public List<SettlementDetail> GetListOfSettlementDetails(Guid transferId, List<Guid> lstOfSettlemntDetailids)
        {
            return _settlementDetailRepository.Query(a => a.BankTransferId == transferId && lstOfSettlemntDetailids.Contains(a.Id)).Select().ToList();
        }
    }
}
