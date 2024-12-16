using AppsWorld.BankReconciliationModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
    public interface IBankTransferService:IService<BankTransfer>
    {
        BankTransfer GetBankTran(Guid id, long companyid);
    }
}
