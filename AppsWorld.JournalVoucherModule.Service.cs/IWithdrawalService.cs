using Service.Pattern;
using System;
using AppsWorld.JournalVoucherModule.Entities;
using System.Linq;
using System.Collections.Generic;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IWithdrawalService : IService<Withdrawal>
    {
        Withdrawal GetWithdrawal(Guid? id, long companyId, string docType);
        Withdrawal GetById(Guid? id);
    }
}
