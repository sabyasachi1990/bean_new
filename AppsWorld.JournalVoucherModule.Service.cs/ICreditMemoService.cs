using AppsWorld.JournalVoucherModule.Entities;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface ICreditMemoService : IService<CreditMemo>
    {
        CreditMemo GetMemo(Guid? id);
    }
}
