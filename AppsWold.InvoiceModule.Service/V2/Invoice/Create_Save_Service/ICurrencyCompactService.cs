using System;
using System.Collections.Generic;
using System.Linq;
using AppsWorld.CommonModule.Infra;
using AppsWorld.InvoiceModule.Entities.V2;
using Service.Pattern;

namespace AppsWorld.InvoiceModule.Service.V2
{
    public interface ICurrencyCompactService : IService<CurrencyCompact>
    {
        LookUpCategory<string> GetByCurrenciesEdit(long CompanyId, string code, string CategoryCode);
        LookUpCategory<string> GetByCurrencies(long CompanyId, string CategoryCode);
    }
}
