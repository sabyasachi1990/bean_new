using AppsWorld.OpeningBalancesModule.Entities;
using AppsWorld.OpeningBalancesModule.Models;
using Service.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsWorld.OpeningBalancesModule.Service
{
    public interface IOpeningBalanceService : IService<OpeningBalance>
    {
        IQueryable<OpeningBalanceModelK> GetAllOpeningBalancessK(long companyId,string username);
        OpeningBalance GetOpeningBalance(long companyId, long ServiceCompanyId);
        OpeningBalance GetServiceCompanyOpeningBalance(long companyId, long ServiceCompanyId);
        OpeningBalance GetOpeningBalanceById(Guid Id);
        OpeningBalance GetServiceCompanyOpeningBalanceNew(long companyId, long ServiceCompanyId);
        OpeningBalance CheckOpeningBalanceById(Guid id);
        OpeningBalance GetServiceCompanyOpeningBalance(long ServiceCompanyId);
        List<OpeningBalance> lstopeningbalance(long companyid);

    }
}
