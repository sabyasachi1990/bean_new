using AppsWorld.BankReconciliationModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.BankReconciliationModule.Service
{
    public interface IWithdrawalService : IService<Withdrawal>
    {
        Withdrawal GetWithdraw(Guid id, long companyid);
    }
}
