using AppsWorld.JournalVoucherModule.Entities;
using AppsWorld.JournalVoucherModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.JournalVoucherModule.Service
{
    public interface IOrderService : IService<Order>
    {
        List<Order> GetOrderByEid(long companyid);
        Order GetOrderByEidLeadSheetType(long companyId, string type, string accountclass);
        Order GetIncomeStatementOrderByCompanyId(long companyId);
    }
}
