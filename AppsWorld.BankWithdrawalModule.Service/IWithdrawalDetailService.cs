using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsWorld.BankWithdrawalModule.Entities;
using Service.Pattern;

namespace AppsWorld.BankWithdrawalModule.Service
{
    public interface IWithdrawalDetailService : IService<WithdrawalDetail>
    {
        List<WithdrawalDetail> GetAllWithdraw(Guid withdrawalId);
        WithdrawalDetail GetWithDrawal(Guid id);
    }
}
